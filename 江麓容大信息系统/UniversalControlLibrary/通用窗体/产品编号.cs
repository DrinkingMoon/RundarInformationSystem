using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using AsynSocketService;
using System.Net;
using System.Net.Sockets;
using SocketCommDefiniens;
using ServerRequestProcessorModule;
using GlobalObject;
using System.Text.RegularExpressions;
using UniversalControlLibrary;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 成品的编号录入界面
    /// </summary>
    public partial class 产品编号 : Form
    {
        string _TempBoxNo = "";

        CE_BusinessType m_enumBusinessType;

        /// <summary>
        /// 车间业务信息
        /// </summary>
        Dictionary<string, CE_SubsidiaryOperationType> m_dicWorkShopInfo =
            new Dictionary<string, CE_SubsidiaryOperationType>();

        /// <summary>
        /// 库房业务信息
        /// </summary>
        Dictionary<string, CE_MarketingType> m_dicMarkInfo =
            new Dictionary<string, CE_MarketingType>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopProductCode m_serverWSProductCode = 
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopProductCode>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 营销产品信息服务组件
        /// </summary>
        IProductListServer m_serverProductList = ServerModuleFactory.GetServerModule<IProductListServer>();

        /// <summary>
        /// 返修状态
        /// </summary>
        private bool m_blIsRepaired = false;

        public bool BlIsRepaired
        {
            get { return m_blIsRepaired; }
            set { m_blIsRepaired = value; }
        }

        /// <summary>
        /// 条码物品的信息行
        /// </summary>
        BarCodeInfo m_barCodeInfo;

        /// <summary>
        /// 单据状态
        /// </summary>
        //CE_MarketingType m_enumMarkType;

        /// <summary>
        /// 条码表
        /// </summary>
        private DataTable m_dtProductCodes = new DataTable();

        public DataTable DtProductCodes
        {
            get { return m_dtProductCodes; }
            set { m_dtProductCodes = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillID = "";

        /// <summary>
        /// 营销服务组件
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 网格中的记录数
        /// </summary>
        private int m_intCount = 0;

        public int IntCount
        {
            get { return m_intCount; }
            set { m_intCount = value; }
        }

        /// <summary>
        /// TCU 与 CVT 的判断标志
        /// </summary>
        bool m_blIsTCUFlag = false;

        /// <summary>
        /// 库房代码
        /// </summary>
        //string m_strStorageID = "";

        /// <summary>
        /// 业务类型
        /// </summary>
        private bool? m_blAfterServer = null;

        public bool? BlAfterServer
        {
            get { return m_blAfterServer; }
            set { m_blAfterServer = value; }
        }

        #region 无线通信

        /// <summary>
        /// 服务器接口
        /// </summary>
        IAsynServer m_server;

        /// <summary>
        /// 当前监听端口号
        /// </summary>
        readonly int m_currentPort = 9000;

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="barCodeInfo">物品信息</param>
        /// <param name="businessType">业务状态</param>
        /// <param name="strDJH">业务编号</param>
        /// <param name="isEnteringState">是否为录入状态</param>
        /// <param name="dicInfo">字典信息（库房，业务类型（MarketingType（库房业务），SubsidiaryOperationType（车间业务）））</param>
        public 产品编号(BarCodeInfo barCodeInfo, CE_BusinessType businessType, string strDJH, 
            bool isEnteringState, Dictionary<string, string> dicInfo)
        {
            m_enumBusinessType = businessType;

            switch (m_enumBusinessType)
            {
                case CE_BusinessType.库房业务:

                    foreach (KeyValuePair<string, string> item in dicInfo)
                    {
                        m_dicMarkInfo.Add(item.Key,
                            GeneralFunction.StringConvertToEnum<CE_MarketingType>(item.Value));
                    }

                    break;
                case CE_BusinessType.车间业务:

                    foreach (KeyValuePair<string,string> item in dicInfo)
                    {
                        m_dicWorkShopInfo.Add(item.Key, 
                            GeneralFunction.StringConvertToEnum<CE_SubsidiaryOperationType>(item.Value));
                    }

                    break;
                case CE_BusinessType.综合业务:
                    System.Text.RegularExpressions.Regex rex = new System.Text.RegularExpressions.Regex(@"^\d+$");

                    if (dicInfo != null)
                    {
                        foreach (KeyValuePair<string, string> item in dicInfo)
                        {
                            if (rex.IsMatch(item.Key))
                            {
                                m_dicMarkInfo.Add(item.Key,
                                    GeneralFunction.StringConvertToEnum<CE_MarketingType>(item.Value));
                            }
                            else
                            {
                                m_dicWorkShopInfo.Add(item.Key,
                                    GeneralFunction.StringConvertToEnum<CE_SubsidiaryOperationType>(item.Value));
                            }
                        }
                    }

                    break;
                default:
                    break;
            }

            InitializeComponent();

            if (!isEnteringState)
            {
                ProductCode.ReadOnly = true;
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
                btnAuditing.Enabled = false;
            }

            m_barCodeInfo = barCodeInfo;
            m_strBillID = strDJH;

            m_server = AsynSocketFactory.GetSingletonServer(m_currentPort);
            m_server.Begin();

            m_server.OnConnected += new GlobalObject.DelegateCollection.SocketConnectEvent(AsynServer_OnConnected);
            m_server.OnReceive += new ReceiveEventHandler(AsynServer_OnReceive);
        }

        #region 无线通信方法

        /// <summary>
        /// 显示SOCKET信息
        /// </summary>
        /// <param name="message">信息内容</param>
        void ShowSocketMessage(string message)
        {
            lbConnectInfo.Text = message;
        }

        /// <summary>
        /// 更新产品编码
        /// </summary>
        /// <param name="productCode">产品编码</param>
        void UpdateProductCode(string productCode)
        {
            if (productCode.Trim() != "")
            {
                productCode = productCode.Substring(0, productCode.Length - 1).Trim();
                productCode = productCode.TrimStart('0');
                productCode = productCode.Trim();

                //截断字符串 取钢印码
                if (productCode.Substring(0, 5) == "RDC15")
                {
                    productCode = productCode.Substring(productCode.Length - 9, 9);
                }
                else if (productCode.Substring(0, 5) == "SQJ16")
                {
                    productCode = productCode.Substring(productCode.Length - 9, 9);
                }
                else if (productCode.Substring(0, 2) == "LF")
                {
                    //如果有AF则取10位
                    if (productCode.Contains("AF"))
                    {
                        productCode = productCode.Substring(productCode.Length - 11, 10);
                    }
                    else
                    {
                        productCode = productCode.Substring(productCode.Length - 10, 9);

                        //针对于320的截取后的条形码中添加“A” 形成钢印码
                        if ((m_blAfterServer != null && !(bool)m_blAfterServer) && txtGoodsCode.Text == "RDC15-FB(A)")
                        {
                            productCode = productCode.Insert(4, "A");
                        }
                        else if (txtGoodsCode.Text == "RDC15FB-C")
                        {
                            productCode = productCode + "C";
                        }
                        else if (txtGoodsCode.Text == "RDC15FB(A)-C")
                        {
                            productCode = productCode.Substring(0, 4) + "A" + productCode.Substring(4, 5) + "C";
                        }
                        else if (txtGoodsCode.Text == "RDC15FB-D")
                        {
                            productCode = productCode + "D";
                        }
                        else if (txtGoodsCode.Text == "RDC15FB-E")
                        {
                            productCode = productCode + "E";
                        }
                        else if (txtGoodsCode.Text == "RDC15FB-F")
                        {
                            productCode = productCode + "F";
                        }
                    }
                }
                else if (productCode.Substring(0, 5) == "HMM03")
                {
                    productCode = productCode.Substring(productCode.Length - 10, 9);
                }

                txtProductCode.Text = productCode;
                btnAdd_Click(null, null);
            }
        }

        /// <summary>
        /// SOCKET服务器连接是否成功时的回调函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="isCompleted">连接是否成功的标志</param>
        void AsynServer_OnConnected(object sender, bool isCompleted)
        {
            Socket client = sender as Socket;
            string msg = null;
            IPEndPoint endPoint = (client.RemoteEndPoint as IPEndPoint);

            if (isCompleted)
            {
                msg = String.Format("{0}, {1}端口 连接上服务器。", endPoint.Address.ToString(), endPoint.Port);
            }
            else
            {
                msg = String.Format("{0}, {1}端口 断开服务器。", endPoint.Address.ToString(), endPoint.Port);
            }

            this.Invoke(new GlobalObject.DelegateCollection.MessageHandle(this.ShowSocketMessage), new object[] { msg });
        }

        /// <summary>
        /// 接收到SOCKET服务器传来数据的函数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args">通信事件参数</param>
        void AsynServer_OnReceive(object sender, CommEventArgs args)
        {
            CommEventArgs commArgs = args;
            string address = commArgs.SourceAddress;

            commArgs.SourceAddress = args.TargetAddress;
            commArgs.TargetAddress = address;

            if (args.Params != null)
            {
                for (int i = 0; i < args.Params.Count; i++)
                {
                    if (commArgs.Params[i].CMD == CommCMD.扫描产品编号)
                    {
                        if (commArgs.Params[i].Code == TagCode.获取条形码)
                        {
                            if (txtProductCode.InvokeRequired)
                                this.Invoke(new GlobalObject.DelegateCollection.MessageHandle(this.UpdateProductCode), new object[] { commArgs.Params[i].DataValue.ToString() });
                            else
                                txtProductCode.Text = commArgs.Params[i].DataValue.ToString();
                        }
                    }
                }
            }

            for (int i = 0; i < commArgs.Params.Count; i++)
            {
                commArgs.Params[i].CMD = CommCMD.应答;
            }

            string error;
            m_server.Send(commArgs, out error);
        }

        #endregion

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string productCode)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["ProductCode"].Value == productCode)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        private void ShowDate()
        {
            string strSql = "";

            if (m_enumBusinessType == CE_BusinessType.车间业务)
            {
                strSql = " select ROW_NUMBER() OVER(order by a.ID) as Number," +
                                " ProductCode, Code,'' as ZcCode,'' as BoxNo  " +
                                " from WS_ProductCodeDetail "+
                                " as a inner join (select a.GoodsID, case when b.AttributeValue is null then '' else b.AttributeValue end as Code "+
                                " from (select GoodsID from F_GoodsAttributeRecord  " +
                                " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ", " + (int)CE_GoodsAttributeName.TCU + ") and  AttributeValue = '" + bool.TrueString + "') as a  " +
                                " left join (select GoodsID, AttributeValue from F_GoodsAttributeRecord  " +
                                " where AttributeID = " + (int)CE_GoodsAttributeName.厂商编码 + ") as b on a.GoodsID = b.GoodsID) as b on a.GoodsID = b.GoodsID " +
                                " where a.OperationType = "+ (int)m_dicWorkShopInfo.First().Value +
                                " and a.GoodsID = " + Convert.ToInt32(txtGoodsName.Tag) +
                                " and a.BillNo = '" + m_strBillID + "' order by BoxNo,Number";
            }
            else
            {
                strSql = " select ROW_NUMBER() OVER(order by ID) as Number," +
                                " ProductCode,Code,ZcCode,BoxNo " +
                                " from ProductsCodes where GoodsID = " + Convert.ToInt32(txtGoodsName.Tag) +
                                " and DJH = '" + m_strBillID + "' order by BoxNo,Number";
            }

            m_dtProductCodes = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (m_dtProductCodes.Rows.Count > 0)
            {
                txtCode.Text = m_dtProductCodes.Rows[0]["Code"].ToString();
            }
            else
            {
                object obj = UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(txtGoodsName.Tag), CE_GoodsAttributeName.厂商编码);

                if (obj != null && obj.ToString() != "False")
                {
                    txtCode.Text = obj.ToString();
                }
            }
        }

        /// <summary>
        /// 检查当前网格内是否有重复项
        /// </summary>
        /// <param name="strTxt"></param>
        /// <returns></returns>
        private bool CheckNumber(string strTxt)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt == null)
            {
                return true;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["ProductCode"].ToString().ToUpper() == strTxt.ToUpper())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 打印箱号
        /// </summary>
        private void PrintBoxCode()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            int intCount = dt.Rows.Count;

            if (Math.IEEERemainder(Convert.ToDouble(intCount), 2.0) == 0)
            {
                string FristCode = dt.Rows[intCount - 1]["ProductCode"].ToString();
                string SecondCode = dt.Rows[intCount - 2]["ProductCode"].ToString();
                PrintPartBarcode.PrintBarcodeBoxTCU(FristCode, SecondCode);
            }

            if (Math.IEEERemainder(Convert.ToDouble(intCount), 20.0) == 0)
            {
                string strBoxNo = dt.Rows[intCount - 1]["BoxNo"].ToString();
                PrintPartBarcode.PrintBarcode_120X30(strBoxNo);
            }
        }

        /// <summary>
        /// 将整盒TCU插入表中
        /// </summary>
        private void InsertCode()
        {
            try
            {
                string storageId = null;

                foreach (KeyValuePair<string, CE_MarketingType> item in m_dicMarkInfo)
                {
                    switch (item.Value)
                    {
                        case CE_MarketingType.退库:
                        case CE_MarketingType.出库:
                        case CE_MarketingType.退货:
                        case CE_MarketingType.领料:
                        case CE_MarketingType.调出:
                        case CE_MarketingType.库房报废:
                            storageId = item.Key;
                            break;
                        default:
                            break;
                    }
                }

                if (storageId == null)
                {
                    throw new Exception("找不到出库库房");
                }

                DataTable dtCodes = m_findSellIn.GetBoxInfo(txtBoxNo.Text, storageId);

                if (dtCodes == null || dtCodes.Rows.Count == 0)
                {
                    return;
                }

                bool blInsert = false;

                m_dtProductCodes = (DataTable)dataGridView1.DataSource;
                DataTable dt = m_dtProductCodes.Copy();

                for (int i = 0; i < dtCodes.Rows.Count; i++)
                {
                    blInsert = false;

                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        if (dt.Rows[k]["ProductCode"].ToString() ==
                            dtCodes.Rows[i]["ProductCode"].ToString())
                        {
                            blInsert = true;
                        }
                    }

                    if (!blInsert)
                    {
                        foreach (KeyValuePair<string, CE_MarketingType> item in m_dicMarkInfo)
                        {
                            if (m_findSellIn.IsProductCodeOperationStandard(item.Value.ToString(), typeof(CE_MarketingType),
                                Convert.ToInt32(txtGoodsName.Tag), dtCodes.Rows[i]["ProductCode"].ToString(), item.Key, out m_err))
                            {
                                DataRow dr = m_dtProductCodes.NewRow();
                                dr["Number"] = m_dtProductCodes.Rows.Count + 1;
                                dr["ProductCode"] = dtCodes.Rows[i]["ProductCode"].ToString();
                                dr["BoxNo"] = dtCodes.Rows[i]["BoxNo"].ToString();
                                m_dtProductCodes.Rows.Add(dr);
                            }
                            else
                            {
                                throw new Exception(m_err);
                            }
                        }
                    }
                }

                dataGridView1.DataSource = m_dtProductCodes;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        /// <summary>
        /// 设置箱子编码
        /// </summary>
        private void SetBoxNo()
        {
            Dictionary<CE_GoodsAttributeName, object> dic = UniversalFunction.GetGoodsInfList_Attribute_AttchedInfoList(m_barCodeInfo.GoodsID);

            if (!dic.Keys.ToList().Contains(CE_GoodsAttributeName.装箱数))
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(m_barCodeInfo.GoodsID) + "基础属性未设置【装箱数】");
            }

            string prefix = "";

            if (dic.Keys.ToList().Contains(CE_GoodsAttributeName.CVT) && Convert.ToBoolean(dic[CE_GoodsAttributeName.CVT]))
            {
                prefix = CE_GoodsAttributeName.CVT.ToString();
            }
            else if (dic.Keys.ToList().Contains(CE_GoodsAttributeName.TCU) && Convert.ToBoolean(dic[CE_GoodsAttributeName.TCU]))
            {
                prefix = CE_GoodsAttributeName.TCU.ToString();
            }

            if (prefix == "")
            {
                throw new Exception("该物品基础属性未设置【TCU】或者【CVT】");
            }

            if (dataGridView1.Rows.Count == 0 || dataGridView1 == null)
            {
                txtBoxNo.Text = m_findSellIn.GetBoxNo(prefix);
            }
            else
            {
                if (txtBoxNo.Text.Trim() == "")
                {
                    DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                    dtTemp = GlobalObject.DataSetHelper.SelectDistinct("", dtTemp, "BoxNo");

                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        DataTable dt = (DataTable)dataGridView1.DataSource;
                        DataRow[] dr = dt.Select("BoxNo = '" + dtTemp.Rows[i][0].ToString() + "'");

                        if (dr.Length != Convert.ToInt32(dic[CE_GoodsAttributeName.装箱数]))
                        {
                            txtBoxNo.Text = dtTemp.Rows[i][0].ToString();
                            return;
                        }
                    }

                    txtBoxNo.Text = m_findSellIn.GetBoxNo(prefix);
                }
                else
                {
                    DataTable dt = (DataTable)dataGridView1.DataSource;
                    DataRow[] dr = dt.Select("BoxNo = '" + txtBoxNo.Text + "'");

                    if (dr.Length == Convert.ToInt32(dic[CE_GoodsAttributeName.装箱数]))
                    {
                        string strGetNo = (Convert.ToInt32(txtBoxNo.Text.Substring(9, 4)) + 1).ToString("D4");
                        txtBoxNo.Text = txtBoxNo.Text.Substring(0, 9) + strGetNo;
                    }
                }
            }
        }

        private void 产品编号_Load(object sender, EventArgs e)
        {
            if (!UniversalFunction.IsProduct(m_barCodeInfo.GoodsID))
            {
                this.Close();
            }

            this.timer1.Enabled = true;

            m_blIsTCUFlag = Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(m_barCodeInfo.GoodsID, CE_GoodsAttributeName.TCU));
            txtGoodsName.Text = m_barCodeInfo.GoodsName;
            txtGoodsName.Tag = m_barCodeInfo.GoodsID;
            txtGoodsCode.Text = m_barCodeInfo.GoodsCode;
            txtSpec.Text = m_barCodeInfo.Spec;
            txtBatchNo.Text = m_barCodeInfo.BatchNo;
            txtCount.Text = m_barCodeInfo.Count.ToString();

            //if (!m_blIsTCUFlag)
            //{
            //    BoxNo.Visible = false;
            //    txtBoxNo.Visible = false;
            //    label12.Visible = false;
            //    timer1.Enabled = false;
            //}
            
            IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(m_barCodeInfo.GoodsID, CE_GoodsAttributeName.CVT)))
            {
                txtCode.Enabled = false;
            }

            ShowDate();
            dataGridView1.DataSource = m_dtProductCodes;
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {
            m_dtProductCodes = (DataTable)dataGridView1.DataSource;

            //由于RDC16FB在领料时，还未有组机厂，故型号为空，所以无法在此控制, 并且在入库的自动生成时，就已经未控制 Modify by cjb on 2014.10.11
            //if (txtCode.Text == "")
            //{
            //    MessageBox.Show("请录入型号", "提示");
            //    return;
            //}

            if (Convert.ToDecimal(txtCount.Text) != (decimal)m_dtProductCodes.Rows.Count
                && m_dicMarkInfo.First().Value != CE_MarketingType.入库)
            {
                MessageDialog.ShowPromptMessage("流水号数量与申请人填写数量不符，请重新核实!");
                return;
            }

            try
            {
                switch (m_enumBusinessType)
                {
                    case CE_BusinessType.库房业务:
                        if (!m_serverProductCode.UpdateProducts(m_dtProductCodes, txtCode.Text, "",
                            Convert.ToInt32(txtGoodsName.Tag), m_strBillID, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                        break;
                    case CE_BusinessType.车间业务:
                        m_serverWSProductCode.OperatorProductCodeDetail(m_dtProductCodes,
                             m_strBillID, (int)txtGoodsName.Tag, m_dicWorkShopInfo);
                        break;
                    case CE_BusinessType.综合业务:

                        if (!m_serverProductCode.UpdateProducts(m_dtProductCodes, txtCode.Text, "",
                            Convert.ToInt32(txtGoodsName.Tag), m_strBillID, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }

                        m_serverWSProductCode.OperatorProductCodeDetail(m_dtProductCodes,
                             m_strBillID, (int)txtGoodsName.Tag, m_dicWorkShopInfo);
                        break;
                    case CE_BusinessType.未知:

                        if (!m_serverProductCode.UpdateProducts(m_dtProductCodes, txtCode.Text, "",
                            Convert.ToInt32(txtGoodsName.Tag), m_strBillID, out m_err))
                        {
                            MessageDialog.ShowErrorMessage(m_err);
                            return;
                        }
                        break;
                    default:
                        break;
                }

                this.DialogResult = DialogResult.OK;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }

            this.Close();
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            //UpdateProductCode(txtProductCode.Text);
            DataRow drMessage;
            DataTable dtBill = m_findSellIn.GetBill(m_strBillID, 0);

            if (dtBill == null || dtBill.Rows.Count == 0)
            {
                drMessage = null;
            }
            else
            {
                drMessage = m_findSellIn.GetBill(m_strBillID, 0).Rows[0];
            }

            string strDate = drMessage == null ? ServerTime.Time.ToString()
                : drMessage["Date"].ToString();
            string strYwType = drMessage == null ? ""
                : drMessage["YWFS"].ToString();

            DataTable dtNew = new DataTable();
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dtNew.Columns.Add("序号");
            dtNew.Columns.Add("产品型号");
            dtNew.Columns.Add("入库日期");
            dtNew.Columns.Add("产品编号");
            dtNew.Columns.Add("产品状态");
            dtNew.Columns.Add("产品说明");
            dtNew.Columns.Add("退库状态");
            dtNew.Columns.Add("箱子编号");

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                DataRow dr = dtNew.NewRow();

                dr["序号"] = dt.Rows[i]["Number"].ToString();
                dr["产品型号"] = txtGoodsCode.Text.Trim().ToString();
                dr["入库日期"] = strDate;
                dr["产品编号"] = dt.Rows[i]["ProductCode"].ToString();
                dr["产品状态"] = m_dicMarkInfo.First().Value.ToString();
                dr["产品说明"] = "【" + strYwType + "】  " + m_barCodeInfo.Remark;
                dr["退库状态"] = "";
                dr["箱子编号"] = dt.Rows[i]["BoxNo"].ToString();
                dtNew.Rows.Add(dr);
            }

            ExcelHelperP.DataTableToExcel(saveFileDialog1, dtNew, null);
        }

        private void 产品编号_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (btnAdd.Enabled)
            {
                m_intCount = m_dtProductCodes.Rows.Count;
            }
            else
            {
                m_intCount = 0;
            }

            m_dtProductCodes = (DataTable)dataGridView1.DataSource;

            m_server.OnConnected -= new GlobalObject.DelegateCollection.SocketConnectEvent(AsynServer_OnConnected);
            m_server.OnReceive -= new ReceiveEventHandler(AsynServer_OnReceive);

            m_server.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
            {
                if (dr.Selected)
                {
                    m_dtProductCodes.Rows.RemoveAt(dr.Index);
                }
            }

            dataGridView1.DataSource = m_dtProductCodes;
        }

        void CheckBusiness(Type typeName)
        {
            switch (typeName.Name)
            {
                case "CE_MarketingType":

                    foreach (KeyValuePair<string, CE_MarketingType> item in m_dicMarkInfo)
                    {
                        if (!m_findSellIn.IsProductCodeOperationStandard(item.Value.ToString(), typeName, 
                            Convert.ToInt32(txtGoodsName.Tag), txtProductCode.Text, item.Key, out m_err))
                        {
                            throw new Exception(m_err);
                        }
                    }
                    break;
                case "CE_SubsidiaryOperationType":

                    foreach (KeyValuePair<string, CE_SubsidiaryOperationType> item in m_dicWorkShopInfo)
                    {
                        if (!m_findSellIn.IsProductCodeOperationStandard(item.Value.ToString(), typeName,
                            Convert.ToInt32(txtGoodsName.Tag), txtProductCode.Text, item.Key, out m_err))
                        {
                            throw new Exception(m_err);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtProductCode.Text = txtProductCode.Text.Trim().ToUpper();

                //if (m_blIsTCUFlag)
                //{
                //    List<string> lstTemp = txtProductCode.Text.Split(',').ToList();

                //    if (lstTemp.Count() > 1)
                //    {
                //        txtProductCode.Text = lstTemp[2] + "(" + lstTemp[3] + ")";
                //    }
                //}

                if (!m_serverProductCode.VerifyProductCodesInfo(m_barCodeInfo.GoodsID, txtProductCode.Text.Trim(),
                    GlobalObject.CE_BarCodeType.内部钢印码, out m_err))
                {
                    throw new Exception(m_err);
                }

                switch (m_enumBusinessType)
                {
                    case CE_BusinessType.库房业务:
                        CheckBusiness(typeof(CE_MarketingType));
                        break;
                    case CE_BusinessType.车间业务:
                        CheckBusiness(typeof(CE_SubsidiaryOperationType));
                        break;
                    case CE_BusinessType.综合业务:
                        CheckBusiness(typeof(CE_MarketingType));
                        CheckBusiness(typeof(CE_SubsidiaryOperationType));
                        break;
                    default:
                        break;
                }

                if (!CheckNumber(txtProductCode.Text))
                {
                    return;
                }

                string strBoxNo = "";

                if (_TempBoxNo.Trim().Length > 0)
                {
                    strBoxNo = _TempBoxNo;
                }
                else
                {
                    if (m_enumBusinessType == CE_BusinessType.库房业务 || m_enumBusinessType == CE_BusinessType.综合业务)
                    {
                        if (m_dicMarkInfo.First().Value == CE_MarketingType.入库)
                        {
                            SetBoxNo();
                        }
                        else
                        {
                            strBoxNo = m_findSellIn.GetHoldBoxNo(txtProductCode.Text);
                        }
                    }
                }

                m_dtProductCodes = (DataTable)dataGridView1.DataSource;

                DataRow dr = m_dtProductCodes.NewRow();

                dr["Number"] = m_dtProductCodes.Rows.Count + 1;
                dr["ProductCode"] = txtProductCode.Text;
                dr["BoxNo"] = strBoxNo == "" ? txtBoxNo.Text : strBoxNo;
                m_dtProductCodes.Rows.Add(dr);
                dataGridView1.DataSource = m_dtProductCodes;

                PositioningRecord(txtProductCode.Text);
                txtProductCode.Text = "";
                txtProductCode.Focus();
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                MessageDialog.ShowPromptMessage("编号:【" + txtProductCode.Text + "】  " + ex.Message);
                txtProductCode.Text = "";
                txtProductCode.Focus();
                timer1.Enabled = true;
                return;
            }

        }

        private void txtBoxNo_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtBoxNo.Text != "")
            {
                if (e.KeyValue == 13)
                {
                    InsertCode();

                    txtBoxNo.Text = "";
                    txtBoxNo.Focus();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (btnAdd.Enabled)
                {
                    Dictionary<CE_GoodsAttributeName, object> dic = UniversalFunction.GetGoodsInfList_Attribute_AttchedInfoList(m_barCodeInfo.GoodsID);

                    if (!dic.Keys.ToList().Contains(CE_GoodsAttributeName.流水码))
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(m_barCodeInfo.GoodsID) + "基础属性未设置【流水码】");
                    }

                    List<F_ProductWaterCode> lstWaterCode = dic[CE_GoodsAttributeName.流水码] as List<F_ProductWaterCode>;

                    foreach (F_ProductWaterCode item in lstWaterCode)
                    {
                        if (item.SteelSealExample.Trim().Length == txtProductCode.Text.Trim().Length)
                        {
                            KeyEventArgs k = new KeyEventArgs(Keys.Enter);

                            txtProductCode_KeyDown(sender, k);
                            txtProductCode.Text = "";
                            break;
                        }
                    }
                }

                if ((txtBoxNo.Text.Length == 16 || txtBoxNo.Text.Length == 13)
                    && btnAdd.Enabled && (m_dicMarkInfo.First().Value == CE_MarketingType.出库
                    || m_dicMarkInfo.First().Value == CE_MarketingType.退货
                    || m_dicMarkInfo.First().Value == CE_MarketingType.领料))
                {
                    KeyEventArgs k = new KeyEventArgs(Keys.Enter);
                    txtBoxNo_KeyDown(sender, k);
                }
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                MessageDialog.ShowPromptMessage(ex.Message);
                timer1.Enabled = true;
                return;
            }
        }

        private void txtProductCode_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtProductCode.Text != "")
            {
                if (e.KeyValue == 13)
                {
                    btnAdd_Click(sender, e);
                }
            }
        }

        private void btnInput_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null || dtTemp.Rows.Count == 0)
                {
                    throw new Exception("请填写对应列名的内容");
                }

                if (!dtTemp.Columns.Contains("出厂流水码"))
                {
                    MessageDialog.ShowPromptMessage("未找到列名为：【出厂流水码】的列");
                    return;
                }

                if (!dtTemp.Columns.Contains("箱体编码"))
                {
                    MessageDialog.ShowPromptMessage("未找到列名为：【箱体编码】的列");
                    return;
                }

                m_dtProductCodes.Rows.Clear();
                dataGridView1.DataSource = m_dtProductCodes;

                foreach (DataRow dr in dtTemp.Rows)
                {
                    if (dr["出厂流水码"].ToString().Trim().Length != 0)
                    {
                        txtProductCode.Text = dr["出厂流水码"].ToString().Trim();
                        _TempBoxNo = dr["箱体编码"].ToString().Trim();
                        btnAdd_Click(sender, e);
                    }
                }

                _TempBoxNo = "";
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void PrintBoxCode(List<string> lstBoxCode)
        {
            m_dtProductCodes = (DataTable)dataGridView1.DataSource;

            if (lstBoxCode == null || lstBoxCode.Count() == 0)
            {
                return;
            }

            if (m_dtProductCodes == null || m_dtProductCodes.Rows.Count == 0)
            {
                return;
            }

            lstBoxCode = lstBoxCode.Distinct().ToList();

            foreach (string boxCode in lstBoxCode)
            {
                if (UniversalFunction.GetGoodsAttributeInfo(m_barCodeInfo.GoodsID, CE_GoodsAttributeName.CVT).ToString() == "True")
                {
                    DataTable tempTable = DataSetHelper.SiftDataTable(m_dtProductCodes, "BoxNo = '" + boxCode + "'");

                    List<string> lstBarcode = new List<string>();

                    foreach (DataRow dr in tempTable.Rows)
                    {
                        lstBarcode.Add(dr["ProductCode"].ToString());
                    }

                    PrintPartBarcode.PrintBarcodeCVTNumberList(lstBarcode);
                    PrintPartBarcode.PrintBarcode_120X30(boxCode);
                }
                else if (UniversalFunction.GetGoodsAttributeInfo(m_barCodeInfo.GoodsID, CE_GoodsAttributeName.TCU).ToString() == "True")
                {
                    PrintPartBarcode.PrintBarcode_60X20(boxCode);
                }
            }
        }

        private void btnPrintSelect_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            List<string> lstBoxCode = new List<string>();

            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                lstBoxCode.Add(dgvr.Cells["BoxNo"].Value.ToString());
            }

            PrintBoxCode(lstBoxCode);
        }

        private void btnPrintAll_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            List<string> lstBoxCode = new List<string>();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                lstBoxCode.Add(dgvr.Cells["BoxNo"].Value.ToString());
            }

            PrintBoxCode(lstBoxCode);
        }
    }
}
