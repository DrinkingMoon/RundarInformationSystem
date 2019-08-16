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
    /// 领料物品详单
    /// </summary>
    public partial class FormFetchGoods : Form
    {
        #region 成员变量

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 库房服务组件
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 条形码服务组件
        /// </summary>
        IBarCodeServer m_serverBarCode = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 营销出库服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 仓库编码
        /// </summary>
        string m_strStorageID;

        /// <summary>
        /// 操作模式
        /// </summary>
        public enum OperateMode { 查看, 新建, 修改, 仓库核实, 打印条形码 }

        /// <summary>
        /// 报表标题
        /// </summary>
        private string m_reportTitle = "领料单物料清单";

        /// <summary>
        /// 获取或设置领料清单报表标题
        /// </summary>
        public string ReportTitle
        {
            get { return m_reportTitle; }
            set { m_reportTitle = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 操作模式
        /// </summary>
        OperateMode m_operateMode;

        /// <summary>
        /// 领料单号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 领料单信息
        /// </summary>
        View_S_MaterialRequisition m_billInfo;

        /// <summary>
        /// 产品类型编码
        /// </summary>
        string m_productType;

        /// <summary>
        /// 总成领料数量
        /// </summary>
        int m_amount;

        /// <summary>
        /// 关联单号(报废单号、领料退库单号)
        /// </summary>
        string m_reliantBillNo;

        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 领料单总单服务
        /// </summary>
        IMaterialRequisitionServer m_materialserver = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        /// <summary>
        /// 领料单物品清单服务
        /// </summary>
        IMaterialRequisitionGoodsServer m_goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

        /// <summary>
        /// 报废物品清单服务
        /// </summary>
        IScrapGoodsServer m_scrapGoodsServer = ServerModuleFactory.GetServerModule<IScrapGoodsServer>();

        /// <summary>
        /// 领料退库物品清单服务
        /// </summary>
        IMaterialListReturnedInTheDepot m_returnedInDepotServer = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

        /// <summary>
        /// 基础物品服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// BOM信息服务
        /// </summary>
        IBomServer m_bomServer = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 装配BOM服务
        /// </summary>
        IAssemblingBom m_assemblingBom = ServerModuleFactory.GetServerModule<IAssemblingBom>();

        /// <summary>
        /// 查询到的物品信息集
        /// </summary>
        IEnumerable<View_S_MaterialRequisitionGoods> m_queryGoodsInfo;

        /// <summary>
        /// 用于控制整台领料单生成顺序的数据列表
        /// </summary>
        List<BASE_ProductOrder> m_mrGoodsOrder = null;

        /// <summary>
        /// 库存为零的物品列表
        /// </summary>
        List<View_S_MaterialRequisitionGoods> m_lstZeroStockGoods = new List<View_S_MaterialRequisitionGoods>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 图号型号
        /// </summary>
        string m_goodsCode;

        /// <summary>
        /// 物品名称
        /// </summary>
        string m_goodsName;

        /// <summary>
        /// 规格
        /// </summary>
        string m_spec;

        /// <summary>
        /// 整台领料时显示用的信息
        /// </summary>
        StringBuilder m_sb;

        /// <summary>
        /// 状态标志
        /// </summary>
        public string m_strFlag;

        #region 无线通信

        /// <summary>
        /// 服务器接口
        /// </summary>
        IAsynServer m_server;

        /// <summary>
        /// 当前监听端口号
        /// </summary>
        readonly int m_currentPort = 8229;

        #endregion

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="operateMode">操作模式</param>
        /// <param name="billNo">领料单号</param>
        /// <param name="reliantBillNo">关联单号(报废单、领料退库单号)</param>
        public FormFetchGoods(OperateMode operateMode, string billNo, string reliantBillNo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();

            S_MaterialRequisition tempBill = m_materialserver.GetBill(billNo);

            m_operateMode = operateMode;
            m_billNo = billNo;
            lblBillNo.Text = m_billNo;
            m_reliantBillNo = reliantBillNo;

            m_billInfo = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>().GetBillView(m_billNo);

            numFetchCount.Enabled = false;
            panelSocketInfo.Visible = false;

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_reliantBillNo))
            {
                btnNew.Enabled = false;
                btnAdd.Enabled = false;
                btnDeleteAll.Enabled = false;
                btnFindCode.Enabled = false;
            }

            if (m_operateMode == OperateMode.查看)
            {
                toolStrip1.Visible = false;
            }
            else if (m_operateMode == OperateMode.仓库核实)
            {
                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_reliantBillNo))
                {
                    toolStrip1.Visible = true;
                    btnAdd.Enabled = true;
                    btnDelete.Enabled = true;
                    btnDeleteAll.Enabled = false;
                    btnFindCode.Enabled = true;
                }

                numRequestCount.Enabled = false;
                numFetchCount.Enabled = true;
                panelSocketInfo.Visible = true;

                m_server = AsynSocketFactory.GetSingletonServer(m_currentPort);
                lblServerIP.Text = m_server.IP.ToString();
                m_server.OnConnected += new GlobalObject.DelegateCollection.SocketConnectEvent(AsynServer_OnConnected);
                m_server.OnReceive += new ReceiveEventHandler(AsynServer_OnReceive);

                if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启借还货账务管理]))
                {
                    btnReturnList.Visible = true;
                }
            }
            else if (m_operateMode == OperateMode.打印条形码)
            {
                for (int i = 0; i < toolStrip1.Items.Count; i++)
                {
                    toolStrip1.Items[i].Visible = false;
                }

                btnPrintAllBarCode.Visible = true;
                btnPrintBarCode.Visible = true;
            }
            else
            {
                for (int i = 0; i < toolStrip1.Items.Count; i++)
                {
                    if (toolStrip1.Items[i].Tag != null && toolStrip1.Items[i].Tag.ToString() == "仓管")
                    {
                        toolStrip1.Items[i].Visible = false;
                    }
                }
            }

            m_strStorageID = m_serverStorageInfo.GetStorageID(billNo, "S_MaterialRequisition", "Bill_ID");

            if (m_strStorageID == "05")
            {
                label11.Visible = true;
                cmbProductStatus.Visible = true;
            }
        }

        private void FormFetchGoods_Load(object sender, EventArgs e)
        {
            RefreshForm();

            if (m_strFlag == "已出库")
            {
                btnAdd.Visible = false;
                btnNew.Visible = false;
                btnDeleteAll.Visible = false;
                btnDelete.Visible = false;
                btnUpdate.Visible = false;
                btnRefresh.Visible = false;
                btnInputExcel.Visible = false;
                btnReferenceBill.Visible = false;
                btnPackSending.Visible = false;
            }
            else 
            {
                if (m_operateMode != OperateMode.打印条形码)
                {
                    btnPrintBarCode.Visible = false;
                    btnPrintAllBarCode.Visible = false;
                }
            }
        }

        /// <summary>
        /// 根据关联单号获取关联单类别
        /// </summary>
        /// <param name="reliantBillNo">关联单号</param>
        /// <returns>返回获取到的类别</returns>
        private BASE_BillType GetReliantBillType(string reliantBillNo)
        {
            int intTemp = 0;

            for (int i = 0; i < reliantBillNo.Length; i++)
            {
                if (Char.IsNumber(reliantBillNo,i))
                {
                    intTemp = i;
                    break;
                }
            }

            string prefix = reliantBillNo.Substring(0, intTemp);

            IBillTypeServer billTypeServer = ServerModuleFactory.GetServerModule<IBillTypeServer>();
            IQueryable<BASE_BillType> billTypeInfo = billTypeServer.GetAllType();

            return (from r in billTypeInfo where r.TypeCode == prefix select r).Single();
        }

        /// <summary>
        /// 根据关联单据编号生成详细物品清单
        /// </summary>
        /// <param name="billNo">关联单据编号</param>
        public void GenerateGoodsBill(string billNo)
        {
            if (m_goodsServer.IsExist(m_billNo))
            {
                return;
            }

            string billTypeName = GetReliantBillType(billNo).TypeName;

            if (billTypeName == "报废单")
            {
                GenerateDetailInfoFromScrapBill(billNo);
            }
            else
            {
                GenerateGoodsInfoFromReturnedInDepotBill(billNo);
            }
        }

        /// <summary>
        /// 生成整台领料清单
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <param name="amount">领料台数</param>
        public void GenerateGoodsBill(FetchGoodsType fetchType, string productType, int amount)
        {
            m_productType = productType;
            m_amount = amount;

            if (!m_goodsServer.IsExist(m_billNo))
            {
                Service_Manufacture_Storage.IProductOrder server = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();
                m_mrGoodsOrder = server.GetAllDataList(fetchType, productType, CE_DebitScheduleApplicable.正常装配, true);

                if (m_mrGoodsOrder.Count == 0)
                {
                    MessageDialog.ShowPromptMessage(string.Format("没有找到{0}整台份的领料排序规则，无法进行此操作！", productType));
                    return;
                }

                GenerateCVTGoodsBill(productType, amount);
            }
        }

        /// <summary>
        /// 生成总成领料清单(阀块总成、行星轮合件总成等)
        /// </summary>
        /// <param name="assemblyName">总成名称</param>
        /// <param name="amount">领料数量</param>
        public void GenerateAssemblyGoodsBill(string assemblyName, int amount, string producttype)
        {
            m_amount = amount;

            if (m_goodsServer.IsExist(m_billNo))
            {
                return;
            }

            Service_Manufacture_Storage.IProductOrder server = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();
            string assemblyCode = null;

            if (assemblyName == "液压阀块总成")
            {
                assemblyCode = "RDC15-1512000";
            }
            else if (assemblyName == "行星轮合件")
            {
                assemblyCode = "RDC15-1503400";
            }
            else if (assemblyName == "油底壳总成")
            {
                assemblyCode = m_bomServer.GetBomProductParentCode(producttype, assemblyName).Rows[0]["PartCode"].ToString();
            }
            else
            {
                MessageDialog.ShowPromptMessage(string.Format("没有配置 [{0}] 的装配信息", assemblyName));
                return;
            }

            m_mrGoodsOrder = server.GetAllDataList(FetchGoodsType.整台领料, assemblyCode, CE_DebitScheduleApplicable.正常装配, true);

            if (m_mrGoodsOrder.Count == 0)
            {
                MessageDialog.ShowPromptMessage(string.Format("获取不到 [{0}] 的装配信息", assemblyName));
                return;
            }

            List<View_S_MaterialRequisitionGoods> lstGoodsInfo = new List<View_S_MaterialRequisitionGoods>();
            double showPosition = 0;

            foreach (var curItem in m_mrGoodsOrder)
            {
                View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(curItem.GoodsID);

                // 在库存中找到能满足数量要求的记录在原有数据的基础上添加供应商、批次号等信息
                List<View_S_Stock> lstStock = m_storeServer.GetGoodsStoreOnlyForAssembly(curItem.GoodsID,  m_strStorageID).ToList();
                
                // 请领数
                decimal requestCount = curItem.Redices * m_amount;
                decimal requestCountTemp = requestCount;

                if (lstStock.Count == 0)
                {
                    View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                    goods.领料单号 = m_billNo;
                    goods.基数 = curItem.Redices;
                    goods.实领数 = 0;
                    goods.图号型号 = tempGoodsInfo.图号型号;
                    goods.物品名称 = tempGoodsInfo.物品名称;
                    goods.规格 = tempGoodsInfo.规格;
                    goods.批次号 = "";
                    goods.供应商编码 = "";
                    goods.备注 = "";

                    m_lstZeroStockGoods.Add(goods);
                }
                else
                {
                    int stockIndex = 0;

                    foreach (var stockItem in lstStock)
                    {
                        if (stockItem.库存数量 == 0)
                        {
                            continue;
                        }

                        stockIndex++;

                        View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                        goods.领料单号 = m_billNo;

                        if (stockIndex == 1)
                        {
                            goods.基数 = curItem.Redices;
                        }
                        else
                        {
                            goods.基数 = 0;
                        }

                        goods.物品ID = stockItem.物品ID;
                        goods.实领数 = 0;
                        goods.图号型号 = tempGoodsInfo.图号型号;
                        goods.物品名称 = tempGoodsInfo.物品名称;
                        goods.规格 = tempGoodsInfo.规格;

                        if (GlobalObject.GeneralFunction.IsNullOrEmpty(tempGoodsInfo.规格) && !GlobalObject.GeneralFunction.IsNullOrEmpty(stockItem.规格))
                        {
                            goods.规格 = stockItem.规格;
                        }

                        goods.显示位置 = showPosition++;
                        goods.备注 = "";

                        goods.物品类别 = stockItem.材料类别编码;
                        goods.库存数 = (decimal)stockItem.库存数量;
                        goods.供应商编码 = stockItem.供货单位;
                        goods.批次号 = stockItem.批次号;
                        goods.实领数 = (decimal)(requestCount > stockItem.库存数量 ? stockItem.库存数量 : requestCount);
                        goods.单位 = stockItem.单位;
                        goods.货架 = stockItem.货架;
                        goods.层 = stockItem.层;
                        goods.列 = stockItem.列;
                        goods.请领数 = requestCountTemp;

                        if (stockItem.库存数量 >= requestCount)
                        {
                            lstGoodsInfo.Add(goods);
                            break;
                        }
                        else
                        {
                            if (lstStock.Count != 1 || (stockIndex != lstStock.Count))
                            {
                                requestCount -= (decimal)stockItem.库存数量;
                            }

                            lstGoodsInfo.Add(goods);
                        }
                    }
                }
            }

            if (!m_goodsServer.AddGoods(lstGoodsInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        void FormFetchGoods_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        #region SOCKET通信

        /// <summary>
        /// 刷新窗体显示
        /// </summary>
        void RefreshForm()
        {
            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
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

            //this.Invoke(new MessageHandle(this.ShowSocketMessage), new object[] { msg });
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
                    #region 通信数据处理

                    if (commArgs.Params[i].CMD == CommCMD.用户登陆)
                    {
                        Socket_UserInfo userInfo = commArgs.Params[i].DataValue as Socket_UserInfo;
                        RequestUserProcessor userProcessor = new RequestUserProcessor();
                        commArgs.Params[i].DataValue = userProcessor.ReceiveUserInfo(userInfo);
                    }
                    else if (commArgs.Params[i].CMD == CommCMD.请求)
                    {
                        if (commArgs.Params[i].Code == TagCode.获取领料条形码对应零件信息)
                        {
                            Socket_FetchMaterial fetchMaterialInfo = commArgs.Params[i].DataValue as Socket_FetchMaterial;
                            RequestFetchMaterialInfo fetchMaterialProcessor = new RequestFetchMaterialInfo();
                            commArgs.Params[i].DataValue = fetchMaterialProcessor.ReceiveReadBarCodeInfo(fetchMaterialInfo);
                        }
                        else if (commArgs.Params[i].Code == TagCode.获取领料单号)
                        {
                            Socket_FetchMaterial fetchMaterialInfo = commArgs.Params[i].DataValue as Socket_FetchMaterial;
                            fetchMaterialInfo.BillID = m_billNo;
                            fetchMaterialInfo.FetchState = Socket_FetchMaterial.FetchStateEnum.操作成功;
                            commArgs.Params[i].DataValue = fetchMaterialInfo;
                        }
                    }
                    else if (commArgs.Params[i].CMD == CommCMD.领料)
                    {
                        if (commArgs.Params[i].Code == TagCode.领料信息)
                        {
                            Socket_FetchMaterial fetchMaterialInfo = commArgs.Params[i].DataValue as Socket_FetchMaterial;
                            RequestFetchMaterialInfo fetchMaterialProcessor = new RequestFetchMaterialInfo();
                            commArgs.Params[i].DataValue = fetchMaterialProcessor.ReceiveSaveBarCodeInfo(fetchMaterialInfo);

                            if (fetchMaterialInfo.FetchState == Socket_FetchMaterial.FetchStateEnum.操作成功)
                            {
                                this.Invoke(new GlobalObject.DelegateCollection.NonArgumentHandle(this.RefreshForm));
                            }
                        }
                    }

                    #endregion
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
        /// 根据报废单号生成
        /// </summary>
        /// <param name="billNo">报废单号</param>
        void GenerateDetailInfoFromScrapBill(string billNo)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(billNo))
            {
                return;
            }

            IEnumerable<GoodsGroup> scrapGoods = m_scrapGoodsServer.GetGoodsByGroup(billNo);

            if (scrapGoods.Count() == 0)
            {
                return;
            }

            List<View_S_MaterialRequisitionGoods> lstGoods = new List<View_S_MaterialRequisitionGoods>(scrapGoods.Count());
            decimal alreadyFetchAmount = 0; // 已经领料数
            int index = 0;

            foreach (var curItem in scrapGoods)
            {
                // 在库存中找到能满足数量要求的记录在原有数据的基础上添加供应商、批次号等信息
                List<View_S_Stock> lstStock = m_storeServer.GetGoodsStoreNorml(
                    curItem.图号型号, curItem.物品名称, curItem.规格,m_strStorageID).ToList();

                #region 2013-06-07 夏石友 除“售后返修用”用途可以领返修状态的零件，“0公里返修用”和“生产返修用”都只能领新箱用的零件

                if (m_billInfo.用途说明.Contains("0公里返修用") || m_billInfo.用途说明.Contains("生产返修用"))
                {
                    lstStock = lstStock.FindAll(p => p.物品状态 == CE_StockGoodsStatus.正常.ToString());
                }

                #endregion

                if (lstStock.Count == 0 || (lstStock.Count == 1 && lstStock[0].库存数量 == 0))
                {
                    if (MessageDialog.ShowEnquiryMessage(
                        string.Format("库存中无 图号：{0},名称：{1},规格：{2} 的物品，是否继续生成库存中存在的物品？",
                        curItem.图号型号, curItem.物品名称, curItem.规格)) == DialogResult.No)
                        return;
                    else
                        continue;
                }

                alreadyFetchAmount = m_goodsServer.GetGoodsAmount(billNo, curItem.物品ID);

                if (alreadyFetchAmount == curItem.数量)
                {
                    #region 夏石友，2012-03-28，增加提示，防止领料单中没有物品而不知道问题出现的原因

                    string info = string.Format("关联单据【{0}】中的物品【{1}，图号：{2}，规格：{3}】，已经存在相应的领料单，不允许重复领料",
                        billNo, curItem.物品名称, curItem.图号型号, curItem.规格);

                    MessageDialog.ShowPromptMessage(info);

                    #endregion

                    continue;
                }

                decimal requestCount = curItem.数量 - alreadyFetchAmount;
                decimal requestTemp = requestCount;
                int stockIndex = 0;

                foreach (var stockItem in lstStock)
                {
                    if (stockItem.库存数量 == 0)
                    {
                        continue;
                    }

                    View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                    goods.物品ID = stockItem.物品ID;
                    goods.领料单号 = m_billNo;
                    goods.基数 = 0;

                    goods.实领数 = 0;
                    goods.图号型号 = curItem.图号型号;
                    goods.物品名称 = curItem.物品名称;
                    goods.规格 = curItem.规格;
                    goods.显示位置 = index++;

                    if (alreadyFetchAmount > 0)
                        goods.备注 = string.Format("此报废单的领料数已领数：{0} ", alreadyFetchAmount);
                    else
                        goods.备注 = "";

                    goods.物品类别 = stockItem.材料类别编码;
                    goods.库存数 = (decimal)stockItem.库存数量;
                    goods.供应商编码 = stockItem.供货单位;
                    goods.批次号 = stockItem.批次号;
                    goods.实领数 = (decimal)(requestCount > stockItem.库存数量 ? stockItem.库存数量 : requestCount);
                    goods.单位 = stockItem.单位;
                    goods.货架 = stockItem.货架;
                    goods.层 = stockItem.层;
                    goods.列 = stockItem.列;
                    goods.请领数 = requestTemp;


                    if (stockItem.库存数量 >= requestCount)
                    {
                        lstGoods.Add(goods);
                        break;
                    }
                    else
                    {
                        if (lstStock.Count != 1 || (stockIndex != lstStock.Count))
                        {
                            requestCount -= (decimal)stockItem.库存数量;
                        }

                        lstGoods.Add(goods);
                    }
                }
            }

            if (!m_goodsServer.AddGoods(lstGoods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        /// <summary>
        /// 根据退库单号生成
        /// </summary>
        /// <param name="billNo">单号</param>
        void GenerateGoodsInfoFromReturnedInDepotBill(string billNo)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(billNo))
            {
                return;
            }

            IEnumerable<View_S_MaterialListReturnedInTheDepot> riGoods = m_returnedInDepotServer.GetGoods(billNo);

            if (riGoods.Count() == 0)
            {
                return;
            }

            List<View_S_MaterialRequisitionGoods> lstGoods = new List<View_S_MaterialRequisitionGoods>(riGoods.Count());
            int index = 0;

            foreach (View_S_MaterialListReturnedInTheDepot curItem in riGoods)
            {
                // 在库存中找到能满足数量要求的记录在原有数据的基础上添加供应商、批次号等信息
                List<View_S_Stock> lstStock = m_storeServer.GetGoodsStoreNorml(
                    curItem.图号型号, curItem.物品名称, curItem.规格,m_strStorageID).ToList();

                #region 2013-06-07 夏石友 除“售后返修用”用途可以领返修状态的零件，“0公里返修用”和“生产返修用”都只能领新箱用的零件

                if (m_billInfo.用途说明.Contains("0公里返修用") || m_billInfo.用途说明.Contains("生产返修用"))
                {
                    lstStock = lstStock.FindAll(p => p.物品状态 == CE_StockGoodsStatus.正常.ToString());
                }

                #endregion

                if (lstStock.Count == 0 || (lstStock.Count == 1 && lstStock[0].库存数量 == 0))
                {
                    if (MessageDialog.ShowEnquiryMessage(
                        string.Format("库存中无 图号：{0},名称：{1},规格：{2} 的物品，是否继续生成库存中存在的物品？",
                        curItem.图号型号, curItem.物品名称, curItem.规格)) == DialogResult.No)
                        return;
                    else
                        continue;
                }

                decimal requestCount = (decimal)curItem.退库数;
                decimal requestTemp = requestCount;
                int stockIndex = 0;

                foreach (var stockItem in lstStock)
                {
                    if (stockItem.库存数量 == 0)
                    {
                        continue;
                    }

                    View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                    goods.物品ID = stockItem.物品ID;
                    goods.领料单号 = m_billNo;
                    goods.基数 = 0;

                    goods.实领数 = 0;
                    goods.图号型号 = stockItem.图号型号;
                    goods.物品名称 = stockItem.物品名称;
                    goods.规格 = stockItem.规格;
                    goods.显示位置 = index++;
                    goods.备注 = "";

                    goods.物品类别 = stockItem.材料类别编码;
                    goods.库存数 = (decimal)stockItem.库存数量;
                    goods.供应商编码 = stockItem.供货单位;
                    goods.批次号 = stockItem.批次号;
                    goods.实领数 = (decimal)(requestCount > stockItem.库存数量 ? stockItem.库存数量 : requestCount);
                    goods.单位 = stockItem.单位;
                    goods.货架 = stockItem.货架;
                    goods.层 = stockItem.层;
                    goods.列 = stockItem.列;
                    goods.请领数 = requestTemp;

                    if (stockItem.库存数量 >= requestCount)
                    {
                        lstGoods.Add(goods);
                        break;
                    }
                    else
                    {
                        if (lstStock.Count != 1 || (stockIndex != lstStock.Count))
                        {
                            requestCount -= (decimal)stockItem.库存数量;
                        }
                        lstGoods.Add(goods);
                    }
                }
            }

            if (!m_goodsServer.AddGoods(lstGoods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        /// <summary>
        /// 生成整台领料单
        /// </summary>
        /// <param name="productType">产品类型编码</param>
        /// <param name="cvtCount">台数</param>
        void GenerateCVTGoodsBill(string productType, int cvtCount)
        {
            m_sb = new StringBuilder();

            List<View_S_MaterialRequisitionGoods> goodsInfo = new List<View_S_MaterialRequisitionGoods>();
            BASE_ProductOrder preItem = null;
 
            foreach (var item in m_mrGoodsOrder)
            {
                goodsInfo.AddRange(GenerateMRGoodsRecord(productType, m_mrGoodsOrder, ref preItem, item));
            }

            if (!m_goodsServer.AddGoods(goodsInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
        }

        /// <summary>
        /// 获取工位信息
        /// </summary>
        /// <param name="mapInfo">BOM映射表</param>
        /// <returns>从映射信息中提取的工位信息, 没有返回空串</returns>
        string GetWorkBench(List<View_P_AssemblingBom> info)
        {
            if (info.Count() > 0)
            {
                string workbench = "";

                foreach (var item in info)
                {
                    workbench += item.工位 + ",";
                }

                workbench = workbench.Remove(workbench.Length - 1);
                return workbench;
            }

            return "";
        }

        /// <summary>
        /// 生成领料物品记录
        /// </summary>
        /// <param name="productCode">产品编码</param>
        /// <param name="lstOrderInfo">整台领料排序信息列表</param>
        /// <param name="preItem">上一次处理的零件信息</param>
        /// <param name="curItem">要生成物品记录的零件信息</param>
        /// <returns>生成的物品记录列表</returns>
        List<View_S_MaterialRequisitionGoods> GenerateMRGoodsRecord(string productCode,
            List<BASE_ProductOrder> lstOrderInfo, ref BASE_ProductOrder preItem, BASE_ProductOrder curItem)
        {
            List<View_S_MaterialRequisitionGoods> lstGoodsInfo = new List<View_S_MaterialRequisitionGoods>();
            View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(curItem.GoodsID);

            // 请领数
            decimal requestAmount = curItem.Redices * m_amount;
            decimal requestTempCount = requestAmount;

            // 工位
            List<View_P_AssemblingBom> assemblingBomInfo = m_assemblingBom.GetAssemblingBom(productCode, tempGoodsInfo.图号型号, tempGoodsInfo.规格);

            string workbench = GetWorkBench(assemblingBomInfo);

            List<View_S_Stock> lstStock = m_storeServer.GetGoodsStoreOnlyForAssembly(curItem.GoodsID, m_strStorageID).ToList();
            
            double showPositon = -0.01;

            if (lstStock.Count == 0)
            {
                View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                goods.领料单号 = m_billNo;
                goods.基数 = curItem.Redices;
                goods.实领数 = 0;
                goods.图号型号 = tempGoodsInfo.图号型号;
                goods.物品名称 = tempGoodsInfo.物品名称;
                goods.规格 = tempGoodsInfo.规格;
                goods.批次号 = "";
                goods.供应商编码 = "";

                if (assemblingBomInfo.Count > 0 && assemblingBomInfo[0].是否清洗)
                {
                    goods.显示位置 = curItem.Position;
                    goods.备注 = "清洗," + workbench;
                }
                else
                {
                    goods.显示位置 = curItem.Position;
                    goods.备注 = workbench;
                }

                m_lstZeroStockGoods.Add(goods);
            }
            else
            {
                int stockIndex = 0;

                foreach (var stockItem in lstStock)
                {
                    if (stockItem.库存数量 == 0)
                    {
                        continue;
                    }

                    stockIndex++;
                    showPositon += 0.01;

                    View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                    goods.物品ID = stockItem.物品ID;
                    goods.领料单号 = m_billNo;

                    if (stockIndex == 1)
                    {
                        goods.基数 = curItem.Redices;
                    }
                    else
                    {
                        goods.基数 = 0;
                    }

                    goods.实领数 = 0;
                    goods.图号型号 = tempGoodsInfo.图号型号;
                    goods.物品名称 = tempGoodsInfo.物品名称;
                    goods.规格 = tempGoodsInfo.规格;

                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(tempGoodsInfo.规格) && !GlobalObject.GeneralFunction.IsNullOrEmpty(stockItem.规格))
                    {
                        goods.规格 = stockItem.规格;
                    }

                    if (assemblingBomInfo != null && assemblingBomInfo.Count > 0)
                    {
                        goods.显示位置 = (double)curItem.Position + showPositon;

                        if (assemblingBomInfo[0].是否清洗)
                        {
                            goods.备注 = "清洗," + workbench;
                        }
                        else
                        {
                            goods.显示位置 = (double)curItem.Position + showPositon;
                            goods.备注 = workbench;
                        }
                    }
                    else
                    {
                        goods.显示位置 = 90000;
                        goods.备注 = "未知工位";
                    }

                    goods.物品类别 = stockItem.材料类别编码;
                    goods.库存数 = (decimal)stockItem.库存数量;
                    goods.供应商编码 = stockItem.供货单位;
                    goods.批次号 = stockItem.批次号;
                    goods.实领数 = (decimal)(requestTempCount > stockItem.库存数量 ? stockItem.库存数量 : requestTempCount);
                    goods.单位 = stockItem.单位;
                    goods.货架 = stockItem.货架;
                    goods.层 = stockItem.层;
                    goods.列 = stockItem.列;
                    goods.请领数 = requestAmount;

                    if (stockItem.库存数量 >= requestTempCount)
                    {
                        lstGoodsInfo.Add(goods);
                        break;
                    }
                    else
                    {
                        if (lstStock.Count != 1 && (stockIndex != lstStock.Count))
                        {
                            requestTempCount -= (decimal)stockItem.库存数量;
                        }

                        lstGoodsInfo.Add(goods);
                    }
                }
            }

            return lstGoodsInfo;
        }

        /// <summary>
        /// 生成领料物品记录
        /// </summary>
        /// <param name="listMaterialRequisition">领用物品信息字典</param>
        /// <returns>生成的物品记录列表</returns>
        List<View_S_MaterialRequisitionGoods> GenerateListGoodsInfo(Dictionary<int, decimal> listMaterialRequisition)
        {
            List<View_S_MaterialRequisitionGoods> listResult = new List<View_S_MaterialRequisitionGoods>();

            if (listMaterialRequisition != null && listMaterialRequisition.Count > 0)
            {
                IStoreServer serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();
                int index = 0;

                foreach (KeyValuePair<int, decimal> keyValue in listMaterialRequisition)
                {
                    decimal dcRequisitionCount = keyValue.Value;
                    View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(keyValue.Key);
                    List<View_S_Stock> tempStock = serverStore.GetGoodsStoreNorml(tempGoodsInfo.图号型号, tempGoodsInfo.物品名称,
                        tempGoodsInfo.规格, m_strStorageID).ToList();

                    if (m_billInfo.用途说明.Contains("0公里返修用") || m_billInfo.用途说明.Contains("生产返修用"))
                    {
                        tempStock = tempStock.FindAll(p => p.物品状态 == CE_StockGoodsStatus.正常.ToString());
                    }

                    if (tempStock.Count == 0 || (tempStock.Count == 1 && tempStock[0].库存数量 == 0))
                    {
                        if (MessageDialog.ShowEnquiryMessage("库存中无 " + UniversalFunction.GetGoodsMessage(tempGoodsInfo.序号) 
                            + " ,是否继续生成库存中存在的物品?") == DialogResult.No)
                            return listResult;
                        else
                            continue;
                    }

                    decimal requestCount = keyValue.Value;
                    decimal requestTemp = requestCount;
                    int stockIndex = 0;

                    foreach (View_S_Stock stockItem in tempStock)
                    {
                        if (stockItem.库存数量 == 0)
                        {
                            continue;
                        }

                        View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

                        goods.物品ID = stockItem.物品ID;
                        goods.领料单号 = m_billNo;
                        goods.基数 = 0;

                        goods.实领数 = 0;
                        goods.图号型号 = tempGoodsInfo.图号型号;
                        goods.物品名称 = tempGoodsInfo.物品名称;
                        goods.规格 = tempGoodsInfo.规格;
                        goods.显示位置 = index++;

                        goods.物品类别 = tempGoodsInfo.物品类别;
                        goods.库存数 = (decimal)stockItem.库存数量;
                        goods.供应商编码 = stockItem.供货单位;
                        goods.批次号 = stockItem.批次号;
                        goods.实领数 = (decimal)(requestCount > stockItem.库存数量 ? stockItem.库存数量 : requestCount);
                        goods.单位 = stockItem.单位;
                        goods.货架 = stockItem.货架;
                        goods.层 = stockItem.层;
                        goods.列 = stockItem.列;
                        goods.请领数 = requestTemp;

                        if (stockItem.库存数量 >= requestCount)
                        {
                            listResult.Add(goods);
                            break;
                        }
                        else
                        {
                            if (tempStock.Count != 1 || (stockIndex != tempStock.Count))
                            {
                                requestCount -= (decimal)stockItem.库存数量;
                            }

                            listResult.Add(goods);
                        }
                    }
                }
            }

            return listResult;
        }

        /// <summary>
        /// 清除窗体上的信息
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtCode.Tag = null;
            txtName.Text = "";
            txtSpec.Text = "";
            txtProvider.Text = "";
            txtBatchNo.Text = "";

            numRequestCount.Value = 0;
            numFetchCount.Value = 0;

            cmbUnit.Items.Clear();
            cmbUnit.Text = "";
            txtMaterialType.Text = "";
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";

            txtRemark.Text = "";
        }

        /// <summary>
        /// 刷新控件
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            if (dataGridView1.CurrentRow != null)
            {
                View_S_MaterialRequisitionGoods goods = GetGoodsInfo(dataGridView1.CurrentRow);

                if (goods.返修状态.ToString() == "")
                {
                    cmbProductStatus.SelectedIndex = -1;
                }
                else
                {
                    if ((bool)goods.返修状态)
                    {
                        cmbProductStatus.Text = "已返修";
                    }
                    else
                    {
                        cmbProductStatus.Text = "待返修";
                    }
                }

                txtCode.Tag = goods.物品ID;
                txtCode.Text = goods.图号型号;
                txtName.Text = goods.物品名称;
                txtSpec.Text = goods.规格;
                txtProvider.Text = goods.供应商编码;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(goods.批次号))
                {
                    txtBatchNo.Text = goods.批次号;
                }

                numRequestCount.Value = goods.请领数;
                numFetchCount.Value = goods.实领数;

                SetUnit((int)txtCode.Tag, goods.单位);
                txtMaterialType.Text = goods.物品类别;
                txtShelf.Text = goods.货架;
                txtColumn.Text = goods.列;
                txtLayer.Text = goods.层;

                txtRemark.Text = goods.备注;
            }
        }

        /// <summary>
        /// 检测有关数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (txtName.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择物品信息");
                return false;
            }

            //检测是否为售后库房的总成并且要求必须选择产品状态
            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(txtCode.Tag), CE_GoodsAttributeName.CVT)) 
                && cmbProductStatus.Text.Trim() == "" && m_strStorageID == "05")
            {
                cmbProductStatus.Focus();
                MessageDialog.ShowPromptMessage("请选择产品状态");
                return false;
            }

            if (txtRemark.Text.Trim() == "" && m_operateMode != OperateMode.仓库核实)
            {
                MessageDialog.ShowPromptMessage("请详细填写【备注】，如有疑问请咨询财务！");
                return false;
            }

            //由于领料单在仓管核实时，领料单明细可多次重复操作 新建/添加/删除/修改 的操作，
            //并且单据明细无法记录申请人当初的申请的状态，故无法控制请领数、实领数 自身以及相互的关系
            //Modify by cjb on 2014.12.16
            #region
            if (numRequestCount.Value == 0 && m_operateMode == OperateMode.新建)
            {
                numRequestCount.Focus();
                MessageDialog.ShowPromptMessage("请领数必须 > 0");
                return false;
            }

            //#region 夏石友, 2013-04-02, 修正没有检查 实领数 > 请领数的现象

            //if (numRequestCount.Value < numFetchCount.Value)
            //{
            //    numFetchCount.Focus();
            //    MessageDialog.ShowPromptMessage("实领数必须 <= 请领数");
            //    return false;
            //}

            //#endregion
            #endregion

            return true;
        }

        void SetUnit(int goodsID, string unitName)
        {
            cmbUnit.Items.Clear();
            cmbUnit.Text = "";

            cmbUnit.Items.Add(unitName);

            if (UniversalFunction.GetGoodsAttributeInfo(goodsID, CE_GoodsAttributeName.整包发料).ToString() != "False")
            {
                cmbUnit.Items.Add("包");
            }

            cmbUnit.Text = unitName;
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="goods">物品信息</param>
        void RefreshDataGridView(IEnumerable<View_S_MaterialRequisitionGoods> goods)
        {
            if (goods == null)
            {
                return;
            }

            this.dataGridView1.Visible = false;
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter -= new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.DataSource = GlobalObject.GeneralFunction.ConvertToDataTable<View_S_MaterialRequisitionGoods>(goods);
            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            this.dataGridView1.CellEnter += new DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            dataGridView1.Refresh();

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;
                dataGridView1.Columns["物品ID"].Visible = false;
                dataGridView1.Columns["领料单号"].Visible = false;
                dataGridView1.Columns["显示位置"].Visible = false;
                dataGridView1.Columns["StorageID"].Visible = false;
            }

            //if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.制造仓库管理员.ToString())
            //    && BasicInfo.ListRoles.Contains(CE_RoleEnum.备件仓库管理员.ToString())
            //    && BasicInfo.ListRoles.Contains(CE_RoleEnum.电子元器件仓库管理员.ToString())
            //    && BasicInfo.ListRoles.Contains(CE_RoleEnum.营销仓库管理员.ToString()))
            //{
            //    if (dataGridView1.Rows.Count > 0)
            //    {
            //        dataGridView1.Columns["库存数"].Visible = false;
            //    }
            //}

            if (m_dataLocalizer == null)
            {
                // 添加数据定位控件
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelTop.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            this.dataGridView1.Visible = true;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="goodsCode">定位用的编码</param>
        /// <param name="goodsName">定位用的名称</param>
        /// <param name="spec">定位用的规格</param>
        void PositioningRecord(string goodsCode, string goodsName, string spec)
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
                if ((string)dataGridView1.Rows[i].Cells["图号型号"].Value == goodsCode &&
                    (string)dataGridView1.Rows[i].Cells["物品名称"].Value == goodsName &&
                    (string)dataGridView1.Rows[i].Cells["规格"].Value == spec)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="rowIndex">定位行号</param>
        void PositioningRecord(int rowIndex)
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

            if (dataGridView1.Rows.Count > 0 && rowIndex < dataGridView1.Rows.Count)
            {
                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[strColName];
            }
        }

        /// <summary>
        /// 更新新增物品在清单中的位置及工位信息
        /// </summary>
        /// <param name="newGoods">新增物品信息</param>
        void UpdateData(S_MaterialRequisitionGoods newGoods)
        {
            bool findSameGoods = false; // 是否找到相同的物品信息
            double pos = 0;
            double findPos = 0;
            string workbench = "";

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                pos = (double)row.Cells["显示位置"].Value;

                if ((int)row.Cells["物品ID"].Value == newGoods.GoodsID)
                {
                    findSameGoods = true;
                    findPos = pos;
                    workbench = (string)row.Cells["备注"].Value;
                }
                else if (findSameGoods)
                {
                    pos = findPos + 0.01;
                    break;
                }
            }

            if (!findSameGoods)
            {
                pos++;
            }

            newGoods.ShowPosition = pos;

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_productType))
                newGoods.Remark = string.Format("{0}，{1}", newGoods.Remark, workbench);
        }

        /// <summary>
        /// 检查界面上的物品在数据控件中是否存在
        /// </summary>
        /// <param name="operation">操作模式，添加或修改</param>
        /// <returns>存在返回true</returns>
        bool IsExistSameGoods(string operation)
        {
            if (operation == "修改" && dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];

                if (row.Cells["图号型号"].Value.ToString() != txtCode.Text || row.Cells["物品名称"].Value.ToString() != txtName.Text
                    || row.Cells["规格"].Value.ToString() != txtSpec.Text  || row.Cells["批次号"].Value.ToString() != txtBatchNo.Text)
                {
                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (row.Cells["图号型号"].Value.ToString() == txtCode.Text 
                            && item.Cells["物品名称"].Value.ToString() == txtName.Text
                            && item.Cells["规格"].Value.ToString() == txtSpec.Text 
                            && item.Cells["批次号"].Value.ToString() == txtBatchNo.Text
                            && item.Cells["供应商编码"].Value.ToString() == txtProvider.Text)
                        {
                            return true;
                        }
                    }
                }
            }
            else if (operation == "添加")
            {
                foreach (DataGridViewRow item in dataGridView1.Rows)
                {
                    if (item.Cells["图号型号"].Value.ToString() == txtCode.Text 
                        && item.Cells["物品名称"].Value.ToString() == txtName.Text
                        && item.Cells["规格"].Value.ToString() == txtSpec.Text 
                        && item.Cells["批次号"].Value.ToString() == txtBatchNo.Text
                        && item.Cells["供应商编码"].Value.ToString() == txtProvider.Text)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 检查类型是否同属于同一个仓库
        /// </summary>
        /// <param name="intGoodsID">物品ID</param>
        /// <returns>属于或者检测无效返回True，否则返回False</returns>
        bool CheckSame(int intGoodsID)
        {
            if (dataGridView1.RowCount.ToString() == "0")
            {
                return true;
            }
            else
            {
                return m_goodsServer.CheckDepot(UniversalFunction.GetGoodsInfo(intGoodsID).物品类别,
                    dataGridView1.Rows[0].Cells["物品类别"].Value.ToString(), m_strStorageID);
            }
        }

        /// <summary>
        /// 对数据集进行剔除操作
        /// </summary>
        /// <param name="dttb">数据集</param>
        /// <returns>返回剔除后的数据集</returns>
        DataTable GetTb(DataTable dttb)
        {
            bool bFlag = true;
            DataTable ReTurTb = dttb.Clone();

            for (int i = 0; i < dttb.Rows.Count; i++)
            {
                for (int k = 0; k < dttb.Rows.Count; k++)
                {
                    bFlag = true;

                    if (k != i)
                    {
                        if (dttb.Rows[i]["物品ID"].ToString() ==
                            dttb.Rows[k]["物品ID"].ToString())
                        {
                            if ((decimal)dttb.Rows[i]["实领数"] < (decimal)dttb.Rows[k]["实领数"])
                            {
                                bFlag = false;
                                break;
                            }
                        }
                    }
                }
                if (bFlag)
                {
                    ReTurTb.ImportRow(dttb.Rows[i]);
                }
            }

            return ReTurTb;
        }

        /// <summary>
        /// 获得合计数量
        /// </summary>
        /// <param name="dgvr">DataGridView数据行</param>
        /// <returns>合计成功返回True，合计失败返回False</returns>
        bool SumCount(DataGridViewRow dgvr)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (Convert.ToDecimal(dgvr.Cells["请领数"].Value) >
                Convert.ToDecimal(dt.Compute("sum(库存数)", "物品ID = " + Convert.ToInt32(dgvr.Cells["物品ID"].Value))))
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        /// <summary>
        /// 从界面获取图号、名称、规格
        /// </summary>
        void GetCodeInfoFromForm()
        {
            m_goodsCode = txtCode.Text;
            m_goodsName = txtName.Text;
            m_spec = txtSpec.Text;
        }

        /// <summary>
        /// 从行记录中提取物品对象信息
        /// </summary>
        /// <param name="row">行记录</param>
        /// <returns>提取的物品信息</returns>
        View_S_MaterialRequisitionGoods GetGoodsInfo(DataGridViewRow row)
        {
            if (row == null)
            {
                return null;
            }

            View_S_MaterialRequisitionGoods goods = new View_S_MaterialRequisitionGoods();

            if (row.Cells["返修状态"].Value != null && row.Cells["返修状态"].Value.ToString() != "")
            {
                goods.返修状态 = (bool)row.Cells["返修状态"].Value;
            }

            goods.序号 = (long)row.Cells["序号"].Value;
            goods.领料单号 = (string)row.Cells["领料单号"].Value;
            goods.基数 = (decimal)row.Cells["基数"].Value;
            goods.物品ID = (int)row.Cells["物品ID"].Value;
            goods.图号型号 = (string)row.Cells["图号型号"].Value;
            goods.物品名称 = (string)row.Cells["物品名称"].Value;
            goods.规格 = (string)row.Cells["规格"].Value;
            goods.供应商编码 = (string)row.Cells["供应商编码"].Value;
            goods.批次号 = (string)row.Cells["批次号"].Value;
            goods.请领数 = (decimal)row.Cells["请领数"].Value;
            goods.实领数 = (decimal)row.Cells["实领数"].Value;

            View_F_GoodsPlanCost basicGoodsInfo = null;

            if (row.Cells["物品类别"].Value != System.DBNull.Value)
            {
                goods.物品类别 = (string)row.Cells["物品类别"].Value;
            }
            else
            {
                basicGoodsInfo = m_basicGoodsServer.GetGoodsInfo(goods.图号型号, goods.物品名称, goods.规格, out m_error);

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return null;
                }

                goods.物品类别 = basicGoodsInfo.物品类别;
                goods.单位 = basicGoodsInfo.单位;
            }

            if (row.Cells["单位"].Value != System.DBNull.Value)
            {
                goods.单位 = (string)row.Cells["单位"].Value;
            }

            if (row.Cells["库存数"].Value != System.DBNull.Value)
            {
                goods.库存数 = (decimal)row.Cells["库存数"].Value;
            }
            else
            {
                goods.库存数 = 0;
            }

            if (row.Cells["货架"].Value != System.DBNull.Value)
            {
                goods.货架 = (string)row.Cells["货架"].Value;
            }
            else
            {
                goods.货架 = "";
            }

            if (row.Cells["列"].Value != System.DBNull.Value)
            {
                goods.列 = (string)row.Cells["列"].Value;
            }
            else
            {
                goods.列 = "";
            }

            if (row.Cells["层"].Value != System.DBNull.Value)
            {
                goods.层 = (string)row.Cells["层"].Value;
            }
            else
            {
                goods.层 = "";
            }

            if (row.Cells["备注"].Value != System.DBNull.Value)
            {
                goods.备注 = (string)row.Cells["备注"].Value;
            }

            goods.显示位置 = (double)row.Cells["显示位置"].Value;

            return goods;
        }

        /// <summary>
        /// 允许关联单更新物品(避免一份领料单关联多个报废单时多领物品的现象)
        /// </summary>
        /// <param name="fetchAmount">领用数量</param>
        /// <returns>允许返回true</returns>
        bool AllowReliantBillUpdateGoods(DataGridViewRow row, decimal fetchAmount, out string error)
        {
            error = null;
            string billTypeName = GetReliantBillType(m_reliantBillNo).TypeName;

            if (billTypeName == "报废单")
            {
                IEnumerable<View_S_ScrapGoods> scrapGoodsGroup = m_scrapGoodsServer.GetGoods(m_reliantBillNo);

                IEnumerable<View_S_ScrapGoods> findScrapGoods = from r in scrapGoodsGroup
                                                                where r.图号型号 == row.Cells["图号型号"].Value.ToString()
                                                                && r.物品名称 == row.Cells["物品名称"].Value.ToString() &&
                                                                r.规格 == row.Cells["规格"].Value.ToString()
                                                                select r;

                if (findScrapGoods == null || findScrapGoods.Count() == 0)
                {
                    error = string.Format("关联单据中不包含图号型号 [{0}], 物品名称 [{1}], 规格 [{2}]的物品, 不能生成领料单清单！",
                        row.Cells["图号型号"].Value.ToString(), row.Cells["物品名称"].Value.ToString(), row.Cells["规格"].Value.ToString());
                    return false;
                }

                var varData = from r in scrapGoodsGroup
                              where r.图号型号 == row.Cells["图号型号"].Value.ToString()
                              && r.物品名称 == row.Cells["物品名称"].Value.ToString()
                              && r.规格 == row.Cells["规格"].Value.ToString()
                              select r;
                decimal dcCount = 0;

                foreach (var item in varData)
                {
                    dcCount = dcCount + item.报废数量;
                }


                IMaterialRequisitionServer server = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();
                IQueryable<S_MaterialRequisition> mrBills = server.ContainAssociatedBill(m_reliantBillNo);

                if (mrBills == null || mrBills.Count() == 0)
                {
                    return true;
                }

                decimal alreadyFetchGoods = 0;

                foreach (var item in mrBills)
                {
                    IEnumerable<View_S_MaterialRequisitionGoods> mrGoods = m_goodsServer.GetGoods(item.Bill_ID);

                    var result = from r in mrGoods
                                 where r.图号型号 == row.Cells["图号型号"].Value.ToString()
                                 && r.物品名称 == row.Cells["物品名称"].Value.ToString()
                                 && r.规格 == row.Cells["规格"].Value.ToString()
                                 select r;

                    if (result.Count() == 1)
                    {
                        if ((long)row.Cells["序号"].Value == result.Single().序号)
                        {
                            continue;
                        }

                        alreadyFetchGoods += result.Single().实领数;
                    }
                }

                return fetchAmount <= (dcCount - alreadyFetchGoods);
            }
            else if (billTypeName == "领料退库单")
            {
                IEnumerable<View_S_MaterialListReturnedInTheDepot> returnedGoodsGroup = m_returnedInDepotServer.GetGoods(m_reliantBillNo);
                List<View_S_MaterialListReturnedInTheDepot> returnedGoodsTemp =
                    (from r in returnedGoodsGroup
                     where r.图号型号 == row.Cells["图号型号"].Value.ToString() && r.物品名称 == row.Cells["物品名称"].Value.ToString()
                     && r.规格 == row.Cells["规格"].Value.ToString()
                     select r).ToList();

                if (returnedGoodsTemp == null || returnedGoodsTemp.Count() == 0)
                {
                    error = string.Format("关联单据中不包含图号型号 [{0}], 物品名称 [{1}], 规格 [{2}]的物品, 不能生成领料单清单！",
                        row.Cells["图号型号"].Value.ToString(), row.Cells["物品名称"].Value.ToString(), row.Cells["规格"].Value.ToString());
                    return false;
                }

                View_S_MaterialListReturnedInTheDepot returnedGoods = returnedGoodsTemp[0];
                decimal returnedAmount = 0;

                foreach (var item in returnedGoodsTemp)
                {
                    returnedAmount += (decimal)item.退库数;
                }

                returnedGoods.退库数 = returnedAmount;

                IMaterialRequisitionServer server = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();
                IQueryable<S_MaterialRequisition> mrBills = server.ContainAssociatedBill(m_reliantBillNo);

                if (mrBills == null || mrBills.Count() == 0)
                {
                    return true;
                }

                decimal alreadyFetchGoods = 0;

                foreach (var item in mrBills)
                {
                    IEnumerable<View_S_MaterialRequisitionGoods> mrGoods = m_goodsServer.GetGoods(item.Bill_ID);

                    var result = from r in mrGoods
                                 where r.图号型号 == returnedGoods.图号型号
                                 && r.物品名称 == returnedGoods.物品名称
                                 && r.规格 == returnedGoods.规格
                                 select r;

                    if (result.Count() == 1)
                    {
                        if ((long)row.Cells["序号"].Value == result.Single().序号)
                        {
                            continue;
                        }

                        alreadyFetchGoods += result.Single().实领数;
                    }
                }

                return fetchAmount <= (returnedGoods.退库数 - alreadyFetchGoods);
            }
            else
            {
                return true;
            }

        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = null;
            QueryInfoDialog.LstStockStatus = new List<int>();
            //仅限用于返修箱优先选择
            //2013-06-07 夏石友 除“售后返修用”用途可以领返修状态的零件，“0公里返修用”和“生产返修用”都只能领新箱用的零件
            // 周妙要求 三包外可领用仅限于售后返修用的物品 2013.9.27
            //if (m_billInfo.用途说明.Contains("售后返修用")
            //    || m_billInfo.用途说明.Contains("三包外配件"))
            //{
            //    QueryInfoDialog.LstStockStatus.Add(1);
            //}

            if (m_strStorageID == ((int)CE_StorageName.原材料样品库).ToString("D2")
                || m_strStorageID == ((int)CE_StorageName.自制半成品样品库).ToString("D2"))//样品领用
            {
                QueryInfoDialog.LstStockStatus.Add(2);
            }

            //if (m_billInfo.用途编码.Substring(0, 2) == "15")//仅限用于售后备件优先选择
            //{
            //    QueryInfoDialog.LstStockStatus.Add(3);
            //}

            if (chbIsRepair.Checked)//含返修
            {
                QueryInfoDialog.LstStockStatus.Add(1);
            }

            if (chbIsIsolation.Checked)//含隔离
            {
                QueryInfoDialog.LstStockStatus.Add(4);
            }

            form = QueryInfoDialog.GetStoreGoodsInfoDialog(CE_BillTypeEnum.领料单, m_strStorageID);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = (string)form.GetDataItem("图号型号");
                txtCode.Tag = (int)form.GetDataItem("物品ID");
                txtName.Text = (string)form.GetDataItem("物品名称");
                txtName.Tag = (decimal)form.GetDataItem("库存数量");
                txtSpec.Text = (string)form.GetDataItem("规格");
                txtProvider.Text = (string)form.GetDataItem("供货单位");

                if (m_operateMode == OperateMode.仓库核实
                    || QueryInfoDialog.LstStockStatus.Count > 0)
                {
                    txtBatchNo.Text = form.GetStringDataItem("批次号");

                    SetUnit((int)txtCode.Tag, form.GetStringDataItem("单位"));
                    txtMaterialType.Text = form.GetStringDataItem("材料类别名称");
                    txtShelf.Text = form.GetStringDataItem("货架");
                    txtColumn.Text = form.GetStringDataItem("列");
                    txtLayer.Text = form.GetStringDataItem("层");
                }
                else
                {
                    View_S_Stock stockInfo = new View_S_Stock();

                    ////仅限用于返修箱优先选择
                    ////2013-06-07 除“售后返修用”用途可以领返修状态的零件，“0公里返修用”和“生产返修用”都只能领新箱用的零件
                    //if (m_billInfo.用途说明.Contains("售后返修用") || m_billInfo.用途说明.Contains("三包外配件"))
                    //{
                    //    IQueryable<View_S_Stock> lnqStock = m_storeServer.GetGoodsStoreOnlyForRepair(txtCode.Text,
                    //        txtName.Text, txtSpec.Text, m_strStorageID);

                    //    if (lnqStock.Count() != 0)
                    //    {
                    //        stockInfo = lnqStock.First();
                    //    }
                    //}

                    ////仅限用于售后备件优先选择
                    //if (m_billInfo.用途编码.Substring(0, 2) == "15")
                    //{
                    //    IQueryable<View_S_Stock> lnqStock = m_storeServer.GetGoodsStoreOnlyForAttachment(txtCode.Text,
                    //        txtName.Text, txtSpec.Text, m_strStorageID);

                    //    if (lnqStock.Count() != 0)
                    //    {
                    //        stockInfo = lnqStock.First();
                    //    }
                    //}

                    //上述情况记录数为0时，则按先进先出的原则
                    if (stockInfo == null || stockInfo.物品ID == 0)
                    {
                        stockInfo = m_storeServer.GetGoodsStoreNorml(txtCode.Text,
                            txtName.Text, txtSpec.Text, m_strStorageID).First();
                    }

                    txtBatchNo.Text = stockInfo.批次号;
                    txtProvider.Text = stockInfo.供货单位;

                    SetUnit((int)txtCode.Tag, stockInfo.单位);
                    txtMaterialType.Text = stockInfo.材料类别编码;
                    txtShelf.Text = stockInfo.货架;
                    txtColumn.Text = stockInfo.列;
                    txtLayer.Text = stockInfo.层;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            //if (!CheckSame((int)txtCode.Tag))
            //{
            //    MessageBox.Show("请选择同一仓库的物品再进行添加", "提示");
            //    return;
            //}

            if (IsExistSameGoods("添加"))
            {
                MessageDialog.ShowPromptMessage("单据中已经存在与您操作的完全相同的物品，不允许进行此操作！");
                return;
            }

            View_S_MaterialRequisition lnqMaterialBill = m_materialserver.GetBillView(m_billNo);

            if (lnqMaterialBill.关联单号.ToString() != "" 
                && GetReliantBillType(lnqMaterialBill.关联单号).TypeName == "报废单" 
                && !m_goodsServer.CheckScrapGoods(m_billNo, (int)txtCode.Tag, numFetchCount.Value, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            S_MaterialRequisitionGoods goods = new S_MaterialRequisitionGoods();

            goods.ID = 0;
            goods.Bill_ID = m_billNo;
            goods.BasicCount = 0;
            goods.BatchNo = txtBatchNo.Text;
            goods.GoodsID = (int)txtCode.Tag;
            goods.ProviderCode = txtProvider.Text;
            goods.Remark = txtRemark.Text;

            //产品状态 设置 2012.3.30 by cjb
            if (cmbProductStatus.Text.Trim() != "")
            {
                if (cmbProductStatus.Text.Trim() == "已返修")
                {
                    goods.RepairStatus = true;
                }
                else
                {
                    goods.RepairStatus = false;
                }
            }

            if (cmbUnit.Items.Count > 1 && cmbUnit.Text == "包")
            {
                decimal dcPackCount = m_basicGoodsServer.GetPackCount(goods.GoodsID, goods.ProviderCode);

                if (m_operateMode == OperateMode.仓库核实)
                {
                    goods.RealCount = numFetchCount.Value * dcPackCount;
                }
                else
                {
                    goods.RealCount = numRequestCount.Value * dcPackCount;
                    goods.RequestCount = numRequestCount.Value * dcPackCount;
                }
            }
            else
            {
                if (m_operateMode == OperateMode.仓库核实)
                {
                    goods.RealCount = numFetchCount.Value;
                }
                else
                {
                    goods.RealCount = numRequestCount.Value;
                    goods.RequestCount = numRequestCount.Value;
                }
            }

            // 更新显示位置及工位
            UpdateData(goods);

            if (!m_goodsServer.AddGoods(goods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            GetCodeInfoFromForm();

            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_goodsCode, m_goodsName, m_spec);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要修改的记录后再进行此操作");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只能选择要修改的一条记录后再进行此操作");
                return;
            }

            if (!CheckDataItem())
            {
                return;
            }

            if (IsExistSameGoods("修改"))
            {
                MessageDialog.ShowPromptMessage("单据中已经存在与您操作的完全相同的物品，不允许进行此操作！");
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            if (m_operateMode == OperateMode.仓库核实)
            {
                if (row.Cells["图号型号"].Value.ToString() != txtCode.Text || 
                    row.Cells["物品名称"].Value.ToString() != txtName.Text || 
                    row.Cells["规格"].Value.ToString() != txtSpec.Text)
                {
                    MessageDialog.ShowPromptMessage("不允许修改图号型号、物品名称、规格信息");
                    return;
                }
            }

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_reliantBillNo))
            {
                if (!AllowReliantBillUpdateGoods(row, numRequestCount.Value,out m_error))
                {
                    if (m_error != null)
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(string.Format("此领料单领用 {0} 物品合计数量不允许超过关联单据 {1} 此物品数量。", 
                                                       txtName.Text, m_reliantBillNo));
                    }

                    return;
                }
            }

            S_MaterialRequisitionGoods goods = new S_MaterialRequisitionGoods();
            View_S_MaterialRequisitionGoods viewGoods = GetGoodsInfo(row);

            goods.ID = viewGoods.序号;
            goods.Bill_ID = m_billNo;
            goods.BasicCount = viewGoods.基数;
            goods.BatchNo = txtBatchNo.Text;

            if (txtCode.Tag != null)
            {
                goods.GoodsID = (int)txtCode.Tag;
            }
            else
            {
                goods.GoodsID = viewGoods.物品ID;
            }
            
            goods.ProviderCode = txtProvider.Text;
            goods.Remark = txtRemark.Text;

            if (cmbUnit.Items.Count > 1 && cmbUnit.Text == "包")
            {
                decimal dcPackCount = m_basicGoodsServer.GetPackCount(goods.GoodsID, goods.ProviderCode);

                if (m_operateMode == OperateMode.仓库核实)
                {
                    if (numFetchCount.Value == 0)
                    {
                        numFetchCount.Focus();
                        MessageDialog.ShowPromptMessage("实领数不能等于0");
                        return;
                    }

                    goods.RealCount = numFetchCount.Value * dcPackCount;
                }
                else
                {
                    goods.RealCount = numRequestCount.Value * dcPackCount;
                    goods.RequestCount = numRequestCount.Value * dcPackCount;
                }
            }
            else
            {
                if (m_operateMode == OperateMode.仓库核实)
                {
                    goods.RealCount = numFetchCount.Value;
                }
                else
                {
                    goods.RealCount = numRequestCount.Value;
                    goods.RequestCount = numRequestCount.Value;
                }
            }

            goods.ShowPosition = viewGoods.显示位置;

            //产品状态 设置 2012.3.30 by cjb
            if (cmbProductStatus.Text.Trim() != "")
            {
                if (cmbProductStatus.Text.Trim() == "已返修")
                {
                    goods.RepairStatus = true;
                }
                else
                {
                    goods.RepairStatus = false;
                }
            }

            if (goods.RealCount > viewGoods.库存数 
                && goods.RealCount > (txtName.Tag == null? 0 : Convert.ToDecimal(txtName.Tag)))
            {
                MessageDialog.ShowPromptMessage("实领数不能大于库存数");
                return;
            }

            if (!m_goodsServer.UpdateGoods(goods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            GetCodeInfoFromForm();

            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(m_goodsCode, m_goodsName, m_spec);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshControl();

            if (m_dataLocalizer != null && e.RowIndex > -1)
            {
                m_dataLocalizer.StartIndex = e.RowIndex;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要删除的记录后再进行此操作");
                return;
            }

            string info = string.Format("您当前选择了 {0} 条记录, 是否确定删除？", dataGridView1.SelectedRows.Count);

            if (MessageDialog.ShowEnquiryMessage(info) == DialogResult.No)
            {
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;
            List<long> lstId = new List<long>(dataGridView1.SelectedRows.Count);

            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                lstId.Add((long)row.Cells["序号"].Value);
            }

            if (!m_goodsServer.DeleteGoods(lstId , out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
            RefreshDataGridView(m_queryGoodsInfo);
            PositioningRecord(rowIndex);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_MaterialRequisitionGoods curGoods = GetGoodsInfo(dataGridView1.Rows[i]);

                if (m_billInfo.单据状态 != MaterialRequisitionBillStatus.已出库.ToString())
                {
                    if (curGoods.实领数 > curGoods.库存数)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (SumCount(dataGridView1.Rows[i]))
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.FromArgb(255, 128, 128);
                    }

                    if (curGoods.备注 != null)
                    {
                        if (curGoods.备注.Contains("无线"))
                            dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Green;
                        else if (curGoods.备注.Contains("未知工位"))
                            dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Purple;
                    }
                }

                if (i != dataGridView1.Rows.Count - 1)
                {
                    View_S_MaterialRequisitionGoods nextGoods = GetGoodsInfo(dataGridView1.Rows[i+1]);

                    if (curGoods.图号型号 == nextGoods.图号型号 && curGoods.物品名称 == nextGoods.物品名称)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                        dataGridView1.Rows[i+1].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void btnDeleteAll_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您真的想删除物品清单中的所有数据吗？") == DialogResult.Yes)
            {
                if (!m_goodsServer.DeleteGoods(m_billNo, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }

                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            报表_领料单物品清单 report = new 报表_领料单物品清单(m_billNo, m_reportTitle);
            report.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshForm();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void FormFetchGoods_Shown(object sender, EventArgs e)
        {
            if (m_lstZeroStockGoods.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine(string.Format("{0} 领料单一些物品库存中数量为0，其不会在领料明细中出现：", m_billNo));

                int index = 1;

                sb.AppendLine();
                sb.AppendLine("库存中数量为0的零件：");

                foreach (var item in m_lstZeroStockGoods)
                {
                    sb.AppendFormat("{0}. 图号：{1}，物品名称：{2}，规格：{3}。", index++, item.图号型号, item.物品名称, item.规格);
                    sb.AppendLine();
                }

                sb.AppendLine();

                if (m_sb != null)
                    sb.Append(m_sb.ToString());

                FormLargeMessage form = new FormLargeMessage(sb.ToString(), Color.Red);
                form.ShowDialog();
            }
            else if (m_sb != null && m_sb.Length > 0)
            {
                FormLargeMessage form = new FormLargeMessage(m_sb.ToString(), Color.Red);
                form.ShowDialog();
            }
        }

        private void btnPrintBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                if (dataGridView1.SelectedRows.Count < 1)
                {
                    MessageDialog.ShowPromptMessage("选择打印的条形码记录行不允许为空!");
                    return;
                }

                List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();
                string strWorkBench = "";

                // 保存条形码与实领数对应关系的字典
                Dictionary<int, decimal> dicAmount = new Dictionary<int, decimal>();

                Dictionary<View_S_InDepotGoodsBarCodeTable, string> dicSort = new Dictionary<View_S_InDepotGoodsBarCodeTable, string>();

                DataTable dt = new DataTable();

                if (dataGridView1.SelectedRows.Count > 0)
                {
                    DataTable orgData = (DataTable)this.dataGridView1.DataSource;
                    dt = orgData.Clone();

                    for (int r = 0; r < dataGridView1.SelectedRows.Count; r++)
                    {
                        DataGridViewRow selectedRow = this.dataGridView1.SelectedRows[r];
                        DataRow copyRow = dt.NewRow();

                        for (int j = 0; j < this.dataGridView1.Columns.Count; j++)
                        {
                            copyRow[j] = selectedRow.Cells[j].Value;
                        }

                        dt.Rows.Add(copyRow);
                    }
                }

                dt = ReturnWorkBenchTable(dt);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strWorkBench = dt.Rows[i]["备注"].ToString().Replace("清洗,", "");

                    string[] strWork = strWorkBench.Split(new char[] { ',' });

                    for (int k = 0; k < strWork.Length; k++)
                    {
                        View_S_InDepotGoodsBarCodeTable barcode = new View_S_InDepotGoodsBarCodeTable();

                        barcode.条形码 = m_serverBarCode.GetBarCode(Convert.ToInt32(dt.Rows[i]["物品ID"].ToString()),
                                                        dt.Rows[i]["批次号"].ToString(), m_strStorageID,
                                                        dt.Rows[i]["供应商编码"].ToString());

                        barcode.图号型号 = dt.Rows[i]["图号型号"].ToString();
                        barcode.物品名称 = dt.Rows[i]["物品名称"].ToString();
                        barcode.规格 = dt.Rows[i]["规格"].ToString();
                        barcode.供货单位 = dt.Rows[i]["供应商编码"].ToString();
                        barcode.批次号 = dt.Rows[i]["批次号"].ToString();
                        barcode.货架 = dt.Rows[i]["货架"].ToString();
                        barcode.层 = dt.Rows[i]["层"].ToString();
                        barcode.列 = dt.Rows[i]["列"].ToString();
                        barcode.工位 = strWork[k].ToString();
                        barcode.物品ID = Convert.ToInt32(dt.Rows[i]["物品ID"].ToString());

                        lstBarCodeInfo.Add(barcode);

                        if (!dicAmount.ContainsKey(barcode.条形码))
                        {
                            dicAmount.Add(barcode.条形码, (decimal)dt.Rows[i]["实领数"]);
                        }
                    }
                }

                List<View_S_InDepotGoodsBarCodeTable> barCodeInfo =
                    (from r in lstBarCodeInfo orderby r.工位 select r).ToList();//工位排序

                foreach (var item in barCodeInfo)
                {
                    ServerModule.PrintPartBarcode.PrintBarcodeList(item, dicAmount[item.条形码]);
                }

                MessageBox.Show("条码全部打印完成");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }

        private void btnPrintAllBarCode_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                dt = ReturnWorkBenchTable(dt);

                if (dt.Rows.Count < 1)
                {
                    MessageDialog.ShowPromptMessage("表中无数据集");
                    return;
                }

                List<View_S_InDepotGoodsBarCodeTable> lstBarCodeInfo = new List<View_S_InDepotGoodsBarCodeTable>();
                string strWorkBench = "";

                // 保存条形码与实领数对应关系的字典
                Dictionary<int, decimal> dicAmount = new Dictionary<int, decimal>();

                DataRow[] listDr = dt.Select("", "备注");

                DataTable dtTemp = dt.Clone();

                foreach (DataRow item in listDr)
                {
                    dtTemp.ImportRow(item);
                }

                DataView dvTemp = dtTemp.DefaultView;
                dvTemp.Sort = "显示位置 asc";

                dt = dvTemp.ToTable();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    strWorkBench = dt.Rows[i]["备注"].ToString().Replace("清洗,", "");

                    string[] strWork = strWorkBench.Split(new char[] { ',' });

                    for (int k = 0; k < strWork.Length; k++)
                    {
                        View_S_InDepotGoodsBarCodeTable barcode = new View_S_InDepotGoodsBarCodeTable();

                        barcode.条形码 = m_serverBarCode.GetBarCode(Convert.ToInt32(dt.Rows[i]["物品ID"].ToString()),
                                                        dt.Rows[i]["批次号"].ToString(), m_strStorageID, 
                                                        dt.Rows[i]["供应商编码"].ToString());
                        
                        barcode.图号型号 = dt.Rows[i]["图号型号"].ToString();
                        barcode.物品名称 = dt.Rows[i]["物品名称"].ToString();
                        barcode.规格 = dt.Rows[i]["规格"].ToString();
                        barcode.供货单位 = dt.Rows[i]["供应商编码"].ToString();
                        barcode.批次号 = dt.Rows[i]["批次号"].ToString();
                        barcode.货架 = dt.Rows[i]["货架"].ToString();
                        barcode.层 = dt.Rows[i]["层"].ToString();
                        barcode.列 = dt.Rows[i]["列"].ToString();
                        barcode.工位 = strWork[k].ToString();
                        barcode.物品ID = Convert.ToInt32(dt.Rows[i]["物品ID"].ToString());

                        lstBarCodeInfo.Add(barcode);

                        if (!dicAmount.ContainsKey(barcode.条形码))
                        {
                            dicAmount.Add(barcode.条形码, (decimal)dt.Rows[i]["实领数"]);
                        }
                    }
                }

                List<View_S_InDepotGoodsBarCodeTable> barCodeInfo  =
                    (from r in lstBarCodeInfo orderby r.工位 select r).ToList();//工位排序
                
                foreach (var item in barCodeInfo)
                {
                    ServerModule.PrintPartBarcode.PrintBarcodeList(item, dicAmount[item.条形码]);
                }

                MessageBox.Show("条码全部打印完成");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }

        DataTable ReturnWorkBenchTable(DataTable dtSource)
        {
            S_MaterialRequisition lnqBill = m_materialserver.GetBill(m_billNo);

            if (lnqBill.ProductType == null || lnqBill.ProductType.Length == 0
                || lnqBill.AssociatedBillNo == null || lnqBill.AssociatedBillNo.Length == 0)
            {
                return dtSource;
            }

            List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> listDetail = 
                m_materialserver.GetWholeMachineRequistionDetail(lnqBill.AssociatedBillNo);

            double position = 1;

            foreach (View_Business_WarehouseOutPut_WholeMachineRequisitionDetail item in listDetail)
            {
                DataRow[] drArray = dtSource.Select("物品ID = " + item.物品ID);

                if (drArray.Length > 0)
                {
                    List<View_P_AssemblingBom> assemblingBomInfo =
                        m_assemblingBom.GetAssemblingBom(lnqBill.ProductType, item.图号型号, item.规格);
                    string workbench = GetWorkBench(assemblingBomInfo);

                    double showPositon = 0.01;
                    foreach (DataRow dr in drArray)
                    {
                        if (assemblingBomInfo != null && assemblingBomInfo.Count > 0)
                        {
                            dr["显示位置"] = position + showPositon;

                            if (assemblingBomInfo[0].是否清洗)
                            {
                                dr["备注"] = "清洗," + workbench;
                            }
                            else
                            {
                                dr["备注"] = workbench;
                            }
                        }
                        else
                        {
                            dr["显示位置"] = 90000;
                            dr["备注"] = "未知工位";
                        }

                        showPositon = showPositon + 0.01;
                    }

                    position = position + 1;
                }
            }

            return dtSource;
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            DataTable surDt = dataGridView1.DataSource as DataTable;
            DataTable dt = surDt.Clone();
            List<DataRow> dr = new List<DataRow>();

            for (int i = 0; i < surDt.Rows.Count; i++)
            {
                dr.Add(surDt.Rows[i]);
                //if ((decimal)surDt.Rows[i]["请领数"] > (decimal)(surDt.Compute("sum(库存数)","物品ID = " + 
                //    (int)surDt.Rows[i]["物品ID"])))
                //{
                //    dr.Add(surDt.Rows[i]);
                //}
            }

            if (dr.Count > 0)
            {
                for (int a = 0; a <= dr.Count - 1; a++)
                {
                    dt.ImportRow(dr[a]);
                }

                string[] hideColumns = { "序号", "物品ID", "显示位置", "StorageID" };
                ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, hideColumns);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            View_S_MaterialRequisition lnqMaterial = m_materialserver.GetBillView(m_billNo);

            int intGoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);

            switch (UniversalFunction.GetGoodsType(intGoodsID,m_billInfo.库房代码))
            {
                case CE_GoodsType.CVT:
                case CE_GoodsType.TCU:
                    BarCodeInfo tempInfo = new BarCodeInfo();

                    tempInfo.BatchNo = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
                    tempInfo.Count = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["实领数"].Value);
                    tempInfo.GoodsCode = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                    tempInfo.GoodsID = intGoodsID;
                    tempInfo.GoodsName = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                    tempInfo.Remark = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                    tempInfo.Spec = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();

                    bool blCheck = true;

                    if (m_operateMode == OperateMode.查看)
                    {
                        blCheck = false;
                    }
                    else
                    {

                        if (lnqMaterial.单据状态 != "等待出库")
                        {
                            blCheck = false;
                        }
                    }

                    CE_BusinessType tempType = CE_BusinessType.库房业务;

                    Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                        Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                    WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(m_billInfo.编制人编码);

                    Dictionary<string, string> tempDic = new Dictionary<string, string>();

                    tempDic.Add(m_strStorageID, CE_MarketingType.领料.ToString());

                    if (tempWSCode != null)
                    {
                        tempType = CE_BusinessType.综合业务;
                        tempDic.Add(tempWSCode.WSCode, CE_SubsidiaryOperationType.领料.ToString());
                    }

                    产品编号 formCode = new 产品编号(tempInfo, tempType, m_billNo, blCheck, tempDic);

                    if (m_strStorageID == "05")
                    {
                        if (dataGridView1.CurrentRow.Cells["返修状态"].Value == null 
                            || dataGridView1.CurrentRow.Cells["返修状态"].Value.ToString() == "")
                        {
                            MessageDialog.ShowPromptMessage("请选择产品的返修状态");
                            return;
                        }
                        else
                        {
                            formCode.BlIsRepaired = (bool)dataGridView1.CurrentRow.Cells["返修状态"].Value;
                        }
                    }

                    formCode.ShowDialog();
                    break;
                case CE_GoodsType.工装:
                    工装编号录入窗体 form = new 工装编号录入窗体(m_billNo, intGoodsID, CE_BusinessBillType.领料, lnqMaterial.单据状态 == "等待工艺人员批准" ? true : false);
                    form.StartPosition = FormStartPosition.CenterScreen;
                    form.ShowDialog();
                    break;
                case CE_GoodsType.量检具:
                    量检具编号录入窗体 frm = new 量检具编号录入窗体(m_billNo, intGoodsID, 
                        Convert.ToDecimal(dataGridView1.CurrentRow.Cells["实领数"].Value), CE_BusinessBillType.领料,
                        m_operateMode == OperateMode.仓库核实 ? true : false);

                    frm.ShowDialog();
                    break;
                case CE_GoodsType.零件:
                    break;
                case CE_GoodsType.未知物品:
                    break;
                default:
                    break;
            }
        }

        private void btnReturnList_Click(object sender, EventArgs e)
        {
            还货清单 form = new 还货清单(m_strStorageID, m_billNo, Convert.ToInt32(txtCode.Tag), txtProvider.Text, txtBatchNo.Text);

            form.ShowDialog();
        }

        private void btnReferenceBill_Click(object sender, EventArgs e)
        {
            IMaterialListReturnedInTheDepot serverReturn = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

            FormDataTableCheck frm =
                new FormDataTableCheck(serverReturn.GetBatchCreatList("领料", ServerTime.Time.AddDays(-15), ServerTime.Time));

            frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);
            frm._BlDateTimeControlShow = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<string> listBillNo = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "领料单号");

                Dictionary<int, decimal> dicGoodsInfo = m_goodsServer.SumListBillNoInfo(listBillNo);
                List<View_S_MaterialRequisitionGoods> goodsInfo = GenerateListGoodsInfo(dicGoodsInfo);

                if (!m_goodsServer.AddGoods(goodsInfo, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }

                GetCodeInfoFromForm();

                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        DataTable frm_OnFormDataTableCheckFind(DateTime startTime, DateTime endTime)
        {
            IMaterialListReturnedInTheDepot serverReturn = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();
            return serverReturn.GetBatchCreatList("领料", startTime, endTime);
        }

        private void btnPackSending_Click(object sender, EventArgs e)
        {
            整台整包发料 frm = new 整台整包发料(m_billNo, m_strStorageID);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                if (!m_goodsServer.AddGoods(frm.ResultList, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }

                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);
            }
        }

        private void btnInputExcel_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                if (!dtTemp.Columns.Contains("物品ID"))
                {
                    if (!dtTemp.Columns.Contains("图号型号"))
                    {
                        MessageDialog.ShowPromptMessage("文件无【图号型号】列名");
                        return;
                    }

                    if (!dtTemp.Columns.Contains("物品名称"))
                    {
                        MessageDialog.ShowPromptMessage("文件无【物品名称】列名");
                        return;
                    }

                    if (!dtTemp.Columns.Contains("规格"))
                    {
                        MessageDialog.ShowPromptMessage("文件无【规格】列名");
                        return;
                    }
                }

                if (!dtTemp.Columns.Contains("批次号"))
                {
                    MessageDialog.ShowPromptMessage("文件无【批次号】列名");
                    return;
                }

                if (!dtTemp.Columns.Contains("数量"))
                {
                    MessageDialog.ShowPromptMessage("文件无【数量】列名");
                    return;
                }

                m_goodsServer.InsertInfoExcel(m_billNo, dtTemp);
                MessageDialog.ShowPromptMessage("导入成功");
                m_queryGoodsInfo = m_goodsServer.GetGoods(m_billNo);
                RefreshDataGridView(m_queryGoodsInfo);

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }
    }
}
