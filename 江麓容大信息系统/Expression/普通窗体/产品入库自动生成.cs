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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 成品批量生成入库业务界面
    /// </summary>
    public partial class 产品入库自动生成 : Form
    {
        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IBomServer m_bomService = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = 
            ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 条码表
        /// </summary>
        DataTable m_dtProductCodes = new DataTable();

        /// <summary>
        /// 营销服务组件
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

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

        public 产品入库自动生成()
        {
            InitializeComponent();

            cmbProduct.DataSource = m_bomService.GetAssemblyTypeList();
            cmbProduct.SelectedIndex = -1;


            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;

            CreateStyle();
        }

        /// <summary>
        /// 创建表格样式
        /// </summary>
        void CreateStyle()
        {
            m_dtProductCodes.Columns.Add("序号");
            m_dtProductCodes.Columns.Add("产品名称");
            m_dtProductCodes.Columns.Add("箱体编号");
            m_dtProductCodes.Columns.Add("入库方式");
            m_dtProductCodes.Columns.Add("物品ID");
            m_dtProductCodes.Columns.Add("入库库房");
            m_dtProductCodes.Columns.Add("库房ID");
        }

        /// <summary>
        /// 检查窗体
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckForm()
        {
            if (cmbStorage.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择库房名称!");
                return false;
            }

            if (cmbProduct.Text.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品名称!");
                return false;
            }

            return true;
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
                        if (!cmbRKFS.Text.Contains("售后返修") && cmbProduct.Text == "RDC15-FB(A)")
                        {
                            productCode = productCode.Insert(4, "A");
                        }
                        else if (cmbProduct.Text == "RDC15FB-C")
                        {
                            productCode = productCode + "C";
                        }
                        else if (cmbProduct.Text == "RDC15FB(A)-C")
                        {
                            productCode = productCode.Substring(0, 4) + "A" + productCode.Substring(4, 5) + "C";
                        }
                        else if (cmbProduct.Text == "RDC15FB-D")
                        {
                            productCode = productCode + "D";
                        }
                        else if (cmbProduct.Text == "RDC15FB-E")
                        {
                            productCode = productCode + "E";
                        }
                        else if (cmbProduct.Text == "RDC15FB-F")
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
                                this.Invoke(new GlobalObject.DelegateCollection.MessageHandle(this.UpdateProductCode), new object[] { 
                                    commArgs.Params[i].DataValue.ToString() });
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
        /// 判断是否有相同数据
        /// </summary>
        /// <param name="drinsert">检测数据行</param>
        /// <returns>通过返回True，否则返回False</returns>
        bool CheckSameGoods(DataRow drinsert)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt == null)
            {
                return true;
            }

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["产品名称"].ToString() == drinsert["产品名称"].ToString()
                    && dt.Rows[i]["箱体编号"].ToString() == drinsert["箱体编号"].ToString())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(DataRow drinsert)
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
                if (dataGridView1.Rows[i].Cells["产品名称"].Value.ToString() 
                    == drinsert["产品名称"].ToString()
                    && dataGridView1.Rows[i].Cells["箱体编号"].Value.ToString() 
                    == drinsert["箱体编号"].ToString()
                    && dataGridView1.Rows[i].Cells["入库方式"].Value.ToString() 
                    == drinsert["入库方式"].ToString()
                    && dataGridView1.Rows[i].Cells["入库库房"].Value.ToString() 
                    == drinsert["入库库房"].ToString())
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 获得序号
        /// </summary>
        /// <param name="Dt">数据集</param>
        /// <returns>返回获得序号后的数据集</returns>
        //DataTable GetCountNumber(DataTable Dt)
        //{
        //    for (int i = 0; i < Dt.Rows.Count; i++)
        //    {
        //        Dt.Rows[i]["序号"] = i + 1;
        //    }

        //    return Dt;
        //}

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                txtProductCode.Text = txtProductCode.Text.ToUpper();

                if (!CheckForm())
                {
                    return;
                }
                //获得入库方式
                cmbRKFS.Text = m_findSellIn.GetInStockWay(cmbProduct.Text.ToString(), txtProductCode.Text.ToString());

                //检测钢印规则
                if (!m_serverProductCode.VerifyProductCodesInfo(Convert.ToInt32(cmbProduct.Tag),
                    txtProductCode.Text.Trim(), GlobalObject.CE_BarCodeType.内部钢印码,
                    out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }

                if (!m_findSellIn.IsProductCodeOperationStandard(CE_MarketingType.入库.ToString(), typeof(CE_MarketingType),
                    Convert.ToInt32(cmbProduct.Tag), txtProductCode.Text,
                    cmbStorage.Tag.ToString(), out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }

                Service_Manufacture_WorkShop.IWorkShopBasic serverWorkShopBasic =
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                WS_WorkShopCode tempWorkCode = serverWorkShopBasic.GetPersonnelWorkShop(BasicInfo.LoginID);

                if (tempWorkCode == null)
                {
                    throw new Exception("您不是车间人员，无法执行产品入库业务");
                }

                if (!m_findSellIn.IsProductCodeOperationStandard(CE_SubsidiaryOperationType.营销入库.ToString(), typeof(CE_SubsidiaryOperationType),
                    Convert.ToInt32(cmbProduct.Tag), txtProductCode.Text,
                    tempWorkCode.WSCode, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }

                if (dataGridView1.DataSource != null)
                {
                    m_dtProductCodes = (DataTable)dataGridView1.DataSource;
                }

                DataRow dr = m_dtProductCodes.NewRow();

                dr["产品名称"] = cmbProduct.Text.ToString();
                dr["箱体编号"] = txtProductCode.Text;
                dr["入库方式"] = cmbRKFS.Text.ToString();
                dr["入库库房"] = cmbStorage.Text.ToString();
                dr["物品ID"] = Convert.ToInt32(cmbProduct.Tag);
                dr["库房ID"] = cmbStorage.Tag.ToString();

                ProductCode_AutoCreatePutIn_Subsidiary lnqTemp = new ProductCode_AutoCreatePutIn_Subsidiary();

                lnqTemp.GoodsID = Convert.ToInt32(dr["物品ID"]);
                lnqTemp.ProductCode = dr["箱体编号"].ToString();
                lnqTemp.PutInType = dr["入库方式"].ToString();
                lnqTemp.StorageID = dr["库房ID"].ToString();

                if (!CheckSameGoods(dr))
                {
                    return;
                }

                m_dtProductCodes.Rows.Add(dr);
                dataGridView1.DataSource = m_dtProductCodes;

                m_serverProductCode.Add_AutoCreatePutIn_Subsidiary(lnqTemp);
                PositioningRecord(dr);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            UpdateProductCode(txtProductCode.Text);
            this.Close();
        }

        private void 产品入库自动生成_Load(object sender, EventArgs e)
        {
            m_server = AsynSocketFactory.GetSingletonServer(m_currentPort);
            m_server.Begin();
            m_server.OnConnected += new GlobalObject.DelegateCollection.SocketConnectEvent(AsynServer_OnConnected);
            m_server.OnReceive += new ReceiveEventHandler(AsynServer_OnReceive);

            m_dtProductCodes = m_serverProductCode.Sel_AutoCreatePutIn_Subsidiary();
            dataGridView1.DataSource = m_dtProductCodes;
        }

        private void 产品入库自动生成_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_server.OnConnected -= new GlobalObject.DelegateCollection.SocketConnectEvent(AsynServer_OnConnected);
            m_server.OnReceive -= new ReceiveEventHandler(AsynServer_OnReceive);
            m_server.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            ProductCode_AutoCreatePutIn_Subsidiary lnqTemp = new ProductCode_AutoCreatePutIn_Subsidiary();

            lnqTemp.GoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            lnqTemp.ProductCode = dataGridView1.CurrentRow.Cells["箱体编号"].Value.ToString();
            lnqTemp.PutInType = dataGridView1.CurrentRow.Cells["入库方式"].Value.ToString();
            lnqTemp.StorageID = dataGridView1.CurrentRow.Cells["库房ID"].Value.ToString();

            m_dtProductCodes.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            dataGridView1.DataSource = m_dtProductCodes;

            m_serverProductCode.Del_AutoCreatePutIn_Subsidiary(lnqTemp);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            cmbRKFS.Text = dataGridView1.CurrentRow.Cells["入库方式"].Value.ToString();
            txtProductCode.Text = dataGridView1.CurrentRow.Cells["箱体编号"].Value.ToString();
            cmbProduct.Text = dataGridView1.CurrentRow.Cells["产品名称"].Value.ToString();
            cmbProduct.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            cmbStorage.Text = dataGridView1.CurrentRow.Cells["入库库房"].Value.ToString();
            cmbStorage.Tag = dataGridView1.CurrentRow.Cells["库房ID"].Value.ToString();
        }

        private void cmbProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProduct.Text.ToString() != "")
            {
                var varData = from a in UniversalFunction.GetGoodsInfoList_Attribute(CE_GoodsAttributeName.CVT, "True")
                              where a.GoodsCode == cmbProduct.Text
                              select a.ID;

                if (varData.Count() > 0)
                {
                    int goodsID = varData.First();
                    cmbProduct.Tag = goodsID;
                }
            }
        }

        private void cmbStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStorage.Text.ToString() != "")
            {
                cmbStorage.Tag = UniversalFunction.GetStorageID(cmbStorage.Text);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            Dictionary<CE_GoodsAttributeName, object> dic = UniversalFunction.GetGoodsInfList_Attribute_AttchedInfoList(Convert.ToInt32(cmbProduct.Tag));

            if (!dic.Keys.ToList().Contains(CE_GoodsAttributeName.装箱数))
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(Convert.ToInt32(cmbProduct.Tag)) + "基础属性未设置【装箱数】");
            }

            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt == null || dt.Rows.Count == 0)
            {
                return;
            }

            dt.Columns.Add("BoxNo");

            string prefix = "";

            if (Convert.ToBoolean(dic[CE_GoodsAttributeName.CVT]))
            {
                prefix = CE_GoodsAttributeName.CVT.ToString();
            }
            else if (Convert.ToBoolean(dic[CE_GoodsAttributeName.TCU]))
            {
                prefix = CE_GoodsAttributeName.TCU.ToString();
            }

            string boxNo = m_findSellIn.GetBoxNo(prefix);
            int boxCount = 0;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["入库方式"].ToString() == "未知")
                {
                    MessageDialog.ShowPromptMessage("入库方式不能为【未知】，请选择正确的入库方式");
                    return;
                }

                if (cmbStorage.Text == "售后库房" 
                    && (dt.Rows[i]["入库方式"].ToString() == "生产入库" || dt.Rows[i]["入库方式"].ToString() == "生产返修入库"))
                {
                    MessageDialog.ShowPromptMessage("入库方式错误，请选择正确的入库方式");
                    return;
                }

                if (boxCount == Convert.ToInt32(dic[CE_GoodsAttributeName.装箱数]))
                {
                    boxNo = prefix + (Convert.ToInt64(boxNo.Substring(prefix.Length, boxNo.Length - prefix.Length)) + 1).ToString();
                    boxCount = 0;
                }

                dt.Rows[i]["BoxNo"] = boxNo;
                boxCount += 1;
            }

            if (!m_findSellIn.BatchCreateBill(dt,out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("自动生成成功!");

                List<string> listBarcode = new List<string>();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    listBarcode.Add(dt.Rows[i]["箱体编号"].ToString());

                    if (i + 1 == dt.Rows.Count || dt.Rows[i]["BoxNo"] != dt.Rows[i + 1]["BoxNo"])
                    {
                        PrintPartBarcode.PrintBarcodeCVTNumberList(listBarcode);
                        PrintPartBarcode.PrintBarcodeCVTNumberList_BoxNo(dt.Rows[i]["BoxNo"].ToString());
                        listBarcode = new List<string>();
                    }
                }

                this.Close();
            }
        }

    }
}
