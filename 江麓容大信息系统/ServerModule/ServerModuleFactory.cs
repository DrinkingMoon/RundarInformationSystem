/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ServerModuleFactory.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;
using System.Runtime.Remoting;

namespace ServerModule
{
    /// <summary>
    /// 管理类厂
    /// </summary>
    public class ServerModuleFactory
    {
        /// <summary>
        /// 用于保证服务组件实例的唯一性
        /// </summary>
        static Hashtable m_hashTable = null;
 
        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <returns>返回组件接口</returns>
        public static T GetServerModule<T>()
        {
            string name = typeof(T).ToString();

            m_hashTable = new Hashtable();

            //if (m_hashTable.ContainsKey(name))
            //{
            //    return (T)m_hashTable[name];
            //}

            if (typeof(T) == typeof(IProviderServer))
            {
                IProviderServer serverModule = new ProviderServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAccessoryDutyInfoManageServer))
            {
                IAccessoryDutyInfoManageServer serverModule = new AccessoryDutyInfoManageServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDepartmentServer))
            {
                IDepartmentServer serverModule = new DepartmentServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialTypeServer))
            {
                IMaterialTypeServer serverModule = new MaterialTypeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IStoreServer))
            {
                IStoreServer serverModule = new StoreServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICheckOutInDepotServer))
            {
                ICheckOutInDepotServer serverModule = new CheckOutInDepotServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IUnitServer))
            {
                IUnitServer serverModule = new UnitServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IClientServer))
            {
                IClientServer serverModule = new ClientServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductListServer))
            {
                IProductListServer serverModule = new ProductListServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBasicGoodsServer))
            {
                IBasicGoodsServer serverModule = SCM_Level01_ServerFactory.GetServerModule<IBasicGoodsServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAssemblyInfoServer))
            {
                IAssemblyInfoServer serverModule = PMS_ServerFactory.GetServerModule<IAssemblyInfoServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBarCodeServer))
            {
                IBarCodeServer serverModule = BasicServerFactory.GetServerModule<IBarCodeServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBomServer))
            {
                IBomServer serverModule = PMS_ServerFactory.GetServerModule<IBomServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBomMappingServer))
            {
                IBomMappingServer serverModule = PMS_ServerFactory.GetServerModule<IBomMappingServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAssemblingBom))
            {
                IAssemblingBom serverModule = PMS_ServerFactory.GetServerModule<IAssemblingBom>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IElectronFileServer))
            {
                IElectronFileServer serverModule = PMS_ServerFactory.GetServerModule<IElectronFileServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IChoseConfectServer))
            {
                IChoseConfectServer serverModule = PMS_ServerFactory.GetServerModule<IChoseConfectServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IWorkbenchService))
            {
                IWorkbenchService serverModule = PMS_ServerFactory.GetServerModule<IWorkbenchService>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IGoodsGradeServer))
            {
                IGoodsGradeServer serverModule = new GoodsGradeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBargainInfoServer))
            {
                IBargainInfoServer serverModule = new BargainInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBargainGoodsServer))
            {
                IBargainGoodsServer serverModule = new BargainGoodsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOrderFormInfoServer))
            {
                IOrderFormInfoServer serverModule = new OrderFormInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOrderFormGoodsServer))
            {
                IOrderFormGoodsServer serverModule = new OrderFormGoodsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOrdinaryInDepotBillServer))
            {
                IOrdinaryInDepotBillServer serverModule = new OrdinaryInDepotBillServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOrdinaryInDepotGoodsBill))
            {
                IOrdinaryInDepotGoodsBill serverModule = new OrdinaryInDepotGoodsBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductInfoServer))
            {
                IProductInfoServer serverModule = PMS_ServerFactory.GetServerModule<IProductInfoServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IBillTypeServer))
            {
                IBillTypeServer serverModule = BasicServerFactory.GetServerModule<IBillTypeServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAssignBillNoServer))
            {
                IAssignBillNoServer serverModule = BasicServerFactory.GetServerModule<IAssignBillNoServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialRequisitionServer))
            {
                IMaterialRequisitionServer serverModule = new MaterialRequisitionServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPersonnelInfoServer))
            {
                IPersonnelInfoServer serverModule = SCM_Level01_ServerFactory.GetServerModule<IPersonnelInfoServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialRequisitionGoodsServer))
            {
                IMaterialRequisitionGoodsServer serverModule = new MaterialRequisitionGoodsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDeclareWastrelType))
            {
                IDeclareWastrelType serverModule = new DeclareWastrelType();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IScrapBillServer))
            {
                IScrapBillServer serverModule = new ScrapBillServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IScrapGoodsServer))
            {
                IScrapGoodsServer serverModule = new ScrapGoodsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialRequisitionPurposeServer))
            {
                IMaterialRequisitionPurposeServer serverModule = new MaterialRequisitionPurposeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IFieldWidthServer))
            {
                IFieldWidthServer serverModule = new FieldWidthServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IHomemadePartInDepotServer))
            {
                IHomemadePartInDepotServer serverModule = new HomemadePartInDepotServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialReturnedInTheDepot))
            {
                IMaterialReturnedInTheDepot serverModule = new MaterialReturnedInTheDepot();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialListReturnedInTheDepot))
            {
                IMaterialListReturnedInTheDepot serverModule = new MaterialListReturnedInTheDepot();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialRejectBill))
            {
                IMaterialRejectBill serverModule = new MaterialRejectBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialListRejectBill))
            {
                IMaterialListRejectBill serverModule = new MaterialListRejectBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDetailSummaryInfo))
            {
                IDetailSummaryInfo serverModule = new DetailSummaryInfo();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDepotTypeForPersonnel))
            {
                IDepotTypeForPersonnel serverModule = SCM_Level01_ServerFactory.GetServerModule<IDepotTypeForPersonnel>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IInvoiceServer))
            {
                IInvoiceServer serverModule = new InvoiceServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ISellIn))
            {
                ISellIn serverModule = new SellIn();
                m_hashTable.Add(name, serverModule);
            }
            //else if (typeof(T) == typeof(IProductOrder))
            //{
            //    IProductOrder serverModule = new ProductOrder();
            //    m_hashTable.Add(name, serverModule);
            //}
            else if (typeof(T) == typeof(ICannibalize))
            {
                ICannibalize serverModule = new Cannibalize();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICheckReturnRepair))
            {
                ICheckReturnRepair serverModule = new CheckReturnRepair();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IIsolationManageBill))
            {
                IIsolationManageBill serverModule = new IsolationManageBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMessMessageFeedback))
            {
                IMessMessageFeedback serverModule = new MessMessageFeedback();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITorqueConverterInfoServer))
            {
                ITorqueConverterInfoServer serverModule = new TorqueConverterInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductPlan))
            {
                IProductPlan serverModule = new ProductPlan();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IStoreageCheck))
            {
                IStoreageCheck serverModule = new StoreageCheck();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ISafeStockServer))
            {
                ISafeStockServer serverModule = new SafeStockServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMusterAffirmBill))
            {
                IMusterAffirmBill serverModule = new MusterAffirmBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMusterUse))
            {
                IMusterUse serverModule = new MusterUse();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITempElectronFileServer))
            {
                ITempElectronFileServer serverModule = PMS_ServerFactory.GetServerModule<ITempElectronFileServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMultiBatchPartServer))
            {
                IMultiBatchPartServer serverModule = PMS_ServerFactory.GetServerModule<IMultiBatchPartServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITechnologyChange))
            {
                ITechnologyChange serverModule = new TechnologyChange();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IGoodsLeastPackAndStock))
            {
                IGoodsLeastPackAndStock serverModule = new GoodsLeastPackAndStock();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOrderFormAffrim))
            {
                IOrderFormAffrim serverModule = new OrderFormAffrim();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMarketingPlan))
            {
                IMarketingPlan serverModule = new MarketingPlan();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPurcharsingPlan))
            {
                IPurcharsingPlan serverModule = new PurcharsingPlan();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IGeneratesCheckOutInDepotServer))
            {
                IGeneratesCheckOutInDepotServer serverModule = new GeneratesCheckOutInDepotServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IGoodsAntirust))
            {
                IGoodsAntirust serverModule = new GoodsAntirust();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IQuarantine))
            {
                IQuarantine serverModule = new Quarantine();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IAfterServiceMakePartsBill))
            {
                IAfterServiceMakePartsBill serverModule = new AfterServiceMakePartsBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IReparativePartInfoServer))
            {
                IReparativePartInfoServer serverModule = PMS_ServerFactory.GetServerModule<IReparativePartInfoServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IConvertCVTNumber))
            {
                IConvertCVTNumber serverModule = PMS_ServerFactory.GetServerModule<IConvertCVTNumber>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductBarcodeServer))
            {
                IProductBarcodeServer serverModule = PMS_ServerFactory.GetServerModule<IProductBarcodeServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICheckOutInDepotForOutsourcingServer))
            {
                ICheckOutInDepotForOutsourcingServer serverModule = new CheckOutInDepotForOutsourcingServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductChange))
            {
                IProductChange serverModule = new ProductChange();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IHomemadeRejectBill))
            {
                IHomemadeRejectBill serverModule = new HomemadeRejectBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IHomemadeRejectList))
            {
                IHomemadeRejectList serverModule = new HomemadeRejectList();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IFrockProvingReport))
            {
                IFrockProvingReport serverModule = new FrockProvingReport();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMaterialDetainBill))
            {
                IMaterialDetainBill serverModule = new MaterialDetainBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IPrintProductBarcodeInfo))
            {
                IPrintProductBarcodeInfo serverModule = PMS_ServerFactory.GetServerModule<IPrintProductBarcodeInfo>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IThreePacketsOfTheRepairBill))
            {
                IThreePacketsOfTheRepairBill serverModule = new ThreePacketsOfTheRepairBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IFrockIndepotBill))
            {
                IFrockIndepotBill serverModule = new FrockIndepotBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICommunicateReportBill))
            {
                ICommunicateReportBill serverModule = new CommunicateReportBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductCodeServer))
            {
                IProductCodeServer serverModule = SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IHomemadePartInfoServer))
            {
                IHomemadePartInfoServer serverModule = new HomemadePartInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IStorageInfo))
            {
                IStorageInfo serverModule = new StorageInfo();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductDeliveryInspectionServer))
            {
                IProductDeliveryInspectionServer serverModule = new ProductDeliveryInspectionServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICVTCustomerInformationServer))
            {
                ICVTCustomerInformationServer serverModule = new CVTCustomerInformationServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICVTTruckLoadingInformation))
            {
                ICVTTruckLoadingInformation serverModule = new CVTTruckLoadingInformation();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IQualityProblemRectificationDisposalBill))
            {
                IQualityProblemRectificationDisposalBill serverModule = new QualityProblemRectificationDisposalBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IGoodsShelfLife))
            {
                IGoodsShelfLife serverModule = new GoodsShelfLife();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IReport))
            {
                IReport serverModule = new Report();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMarqueeServer))
            {
                IMarqueeServer serverModule = new MarqueeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDeviceMaintenanceBill))
            {
                IDeviceMaintenanceBill serverModule = new DeviceMaintenanceBill();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ICBOMServer))
            {
                ICBOMServer serverModule = new CBOMServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ILogisticSafeStock))
            {
                ILogisticSafeStock serverModule = new LogisticSafeStock();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IUnProductTestingSingle))
            {
                IUnProductTestingSingle serverModule = new UnProductTestingSingle();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IDisposableGoodsServer))
            {
                IDisposableGoodsServer serverModule = new DisposableGoodsServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITCURepairInfoServer))
            {
                ITCURepairInfoServer serverModule = new TCURepairInfoServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ISalesOrderServer))
            {
                ISalesOrderServer serverModule = new SalesOrderServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ITechnologyAlteration))
            {
                ITechnologyAlteration serverModule = new TechnologyAlteration();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IMinorPurchaseBillServer))
            {
                IMinorPurchaseBillServer serverModule = new MinorPurchaseBillServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductLendService))
            {
                IProductLendService serverModule = new ProductLendService();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductLendReturnService))
            {
                IProductLendReturnService serverModule = new ProductLendReturnService();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IProductReturnService))
            {
                IProductReturnService serverModule = new ProductReturnService();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IStockLack))
            {
                IStockLack serverModule = new StockLack();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IOfflineFailSafeServer))
            {
                IOfflineFailSafeServer serverModule = new OfflineFailSafeServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(ISystemLogServer))
            {
                ISystemLogServer serverModule = new SystemLogServer();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IFinancialDetailManagement))
            {
                IFinancialDetailManagement serverModule = new FinancialDetailManagement();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IToolsManage))
            {
                IToolsManage serverModule = new ToolsManage();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IGaugeManage))
            {
                IGaugeManage serverModule = new GaugeManage();
                m_hashTable.Add(name, serverModule);
            }
            else if (typeof(T) == typeof(IQualitySystemDatabase))
            {
                IQualitySystemDatabase serverModule = new QualitySystemDatabase();
                m_hashTable.Add(name, serverModule);
            }

            if (m_hashTable.ContainsKey(name))
            {
                return (T)m_hashTable[name];
            }

            return default(T);
        }
    }
}
