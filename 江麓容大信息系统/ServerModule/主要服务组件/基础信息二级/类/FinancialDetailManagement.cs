using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 账务明细服务组件
    /// </summary>
    class FinancialDetailManagement : IFinancialDetailManagement
    {
        /// <summary>
        /// 处理入库明细业务信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="indepotDetailInfo">入库明细信息</param>
        /// <param name="stockInfo">库存信息</param>
        public void ProcessInDepotDetail(DepotManagementDataContext ctx, S_InDepotDetailBill indepotDetailInfo, S_Stock stockInfo)
        {
            try
            {
                CE_SubsidiaryOperationType operationType = 
                    (CE_SubsidiaryOperationType)Enum.ToObject(typeof(CE_SubsidiaryOperationType), indepotDetailInfo.OperationType);
                IStoreServer storeService = ServerModule.ServerModuleFactory.GetServerModule<IStoreServer>();

                switch (operationType)
                {
                    case CE_SubsidiaryOperationType.报检入库:
                    case CE_SubsidiaryOperationType.委外报检入库:
                        InsertOnSubmitInDepotDetailBill(ctx, indepotDetailInfo);
                        InDepotToolsInfo(ctx, indepotDetailInfo);
                        InDepotGuagesInfo(ctx, indepotDetailInfo);
                        storeService.InStore(ctx, stockInfo, operationType);
                        break;
                    case CE_SubsidiaryOperationType.采购退货:
                        InsertOnSubmitInDepotDetailBill(ctx, indepotDetailInfo);
                        InDepotToolsInfo(ctx, indepotDetailInfo);
                        FetchGaugeInfo(ctx, indepotDetailInfo);
                        storeService.OutStore(ctx, stockInfo, operationType);
                        break;
                    case CE_SubsidiaryOperationType.普通入库:
                        InsertOnSubmitInDepotDetailBill(ctx, indepotDetailInfo);
                        InDepotToolsInfo(ctx, indepotDetailInfo);
                        InDepotGuagesInfo(ctx, indepotDetailInfo);
                        storeService.InStore(ctx, stockInfo, operationType);
                        storeService.UpdateAging(ctx, stockInfo, false);
                        break;
                    case CE_SubsidiaryOperationType.营销入库:
                    case CE_SubsidiaryOperationType.自制件入库:
                    case CE_SubsidiaryOperationType.自制件工装入库:
                        InsertOnSubmitInDepotDetailBill(ctx, indepotDetailInfo);
                        InDepotToolsInfo(ctx, indepotDetailInfo);
                        InDepotGuagesInfo(ctx, indepotDetailInfo);
                        InDepotWorkShop(ctx, indepotDetailInfo);
                        storeService.InStore(ctx, stockInfo, operationType);
                        break;
                    case CE_SubsidiaryOperationType.营销退货:
                    case CE_SubsidiaryOperationType.自制件退货:
                        InsertOnSubmitInDepotDetailBill(ctx, indepotDetailInfo);
                        InDepotToolsInfo(ctx, indepotDetailInfo);
                        FetchGaugeInfo(ctx, indepotDetailInfo);
                        InDepotWorkShop(ctx, indepotDetailInfo);
                        storeService.OutStore(ctx, stockInfo, operationType);
                        break;
                    case CE_SubsidiaryOperationType.财务对冲:
                    case CE_SubsidiaryOperationType.财务红冲:
                        InsertOnSubmitInDepotDetailBill(ctx, indepotDetailInfo);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 处理出库明细业务信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchGoodsDetailInfo">出库明细信息</param>
        /// <param name="stockInfo">库存信息</param>
        public void ProcessFetchGoodsDetail(DepotManagementDataContext ctx, S_FetchGoodsDetailBill fetchGoodsDetailInfo, S_Stock stockInfo)
        {
            try
            {
                CE_SubsidiaryOperationType operationType =
                    (CE_SubsidiaryOperationType)Enum.ToObject(typeof(CE_SubsidiaryOperationType), fetchGoodsDetailInfo.OperationType);
                IStoreServer storeService = ServerModule.ServerModuleFactory.GetServerModule<IStoreServer>();

                switch (operationType)
                {
                    case CE_SubsidiaryOperationType.领料:
                        decimal dcSumCount = fetchGoodsDetailInfo.Price;

                        fetchGoodsDetailInfo.Price = 
                            fetchGoodsDetailInfo.FetchCount == null ? 0 : fetchGoodsDetailInfo.UnitPrice * (decimal)fetchGoodsDetailInfo.FetchCount;
                        InsertOnSubmitFetchGoodsDetailBill(ctx, fetchGoodsDetailInfo);

                        S_FetchGoodsDetailBill newFetchDetailInfo = AssignmentInfo(fetchGoodsDetailInfo);
                        newFetchDetailInfo.FetchCount = newFetchDetailInfo.FetchCount < dcSumCount ? 0 : newFetchDetailInfo.FetchCount - dcSumCount;

                        FetchToolsInfo(ctx, newFetchDetailInfo);
                        FetchGaugeInfo(ctx, newFetchDetailInfo);
                        FetchLendReturn_MaterialRequisition(ctx, newFetchDetailInfo);

                        if (!fetchGoodsDetailInfo.AssociatedBillNo.Contains("SBW"))
                        {
                            FetchWorkShop(ctx, newFetchDetailInfo);
                        }

                        ctx.SubmitChanges(); // xsy, 2017.11.14 解决更新异常增加
                        storeService.OutStore(ctx, stockInfo, operationType);
                        ctx.SubmitChanges();// xsy, 2017.11.14 解决更新异常增加
                        break;

                    case CE_SubsidiaryOperationType.领料退库:
                        InsertOnSubmitFetchGoodsDetailBill(ctx, fetchGoodsDetailInfo);
                        FetchToolsInfo(ctx, fetchGoodsDetailInfo);
                        InDepotGuagesInfo(ctx, fetchGoodsDetailInfo);

                        if (UniversalFunction.GetStorageInfo(fetchGoodsDetailInfo.StorageID).WorkShopCurrentAccount)
                        {
                            FetchWorkShop(ctx, fetchGoodsDetailInfo);
                        }

                        storeService.InStore(ctx, stockInfo, operationType);
                        break;

                    case CE_SubsidiaryOperationType.报废:
                        FetchToolsInfo(ctx, fetchGoodsDetailInfo);
                        FetchWorkShop(ctx, fetchGoodsDetailInfo);
                        break;

                    case CE_SubsidiaryOperationType.样品耗用:
                    case CE_SubsidiaryOperationType.借货:
                    case CE_SubsidiaryOperationType.还货:
                        FetchLendReturn(ctx, fetchGoodsDetailInfo);
                        FetchToolsInfo(ctx, fetchGoodsDetailInfo);
                        FetchWorkShop(ctx, fetchGoodsDetailInfo);
                        break;
                    case CE_SubsidiaryOperationType.库房调入:
                    case CE_SubsidiaryOperationType.营销退库:
                        InsertOnSubmitFetchGoodsDetailBill(ctx, fetchGoodsDetailInfo);
                        FetchToolsInfo(ctx, fetchGoodsDetailInfo);
                        InDepotGuagesInfo(ctx, fetchGoodsDetailInfo);
                        storeService.InStore(ctx, stockInfo, operationType);
                        break;
                    case CE_SubsidiaryOperationType.库房调出:
                    case CE_SubsidiaryOperationType.营销出库:
                        InsertOnSubmitFetchGoodsDetailBill(ctx, fetchGoodsDetailInfo);
                        FetchToolsInfo(ctx, fetchGoodsDetailInfo);
                        FetchGaugeInfo(ctx, fetchGoodsDetailInfo);
                        storeService.OutStore(ctx, stockInfo, operationType);
                        break;
                    default:
                        break;
                }
            }
            catch (System.Data.Linq.ChangeConflictException)
            {
                foreach (System.Data.Linq.ObjectChangeConflict occ in ctx.ChangeConflicts)
                {
                    //以下是解决冲突的三种方法，选一种即可
                    // 使用当前数据库中的值，覆盖Linq缓存中实体对象的值
                    //occ.Resolve(System.Data.Linq.RefreshMode.OverwriteCurrentValues);
                    
                    // 使用Linq缓存中实体对象的值，覆盖当前数据库中的值
                    occ.Resolve(System.Data.Linq.RefreshMode.KeepCurrentValues);

                    // 只更新实体对象中改变的字段的值，其他的保留不变
                    //occ.Resolve(System.Data.Linq.RefreshMode.KeepChanges);
                }

                // 这个地方要注意，Catch方法中，我们前面只是指明了怎样来解决冲突，这个地方还需要再次提交更新，这样的话，值    //才会提交到数据库。
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 插入提交出库明细表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="detailInfo">明细信息</param>
        void InsertOnSubmitFetchGoodsDetailBill(DepotManagementDataContext ctx, S_FetchGoodsDetailBill detailInfo)
        {
            if (detailInfo == null)
            {
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(detailInfo.FetchBIllID))
            {
                throw new Exception("【单据号】获取失败，请重新再试");
            }

            ctx.S_FetchGoodsDetailBill.InsertOnSubmit(detailInfo);
        }

        /// <summary>
        /// 插入提交入库明细表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="detailInfo">明细信息</param>
        void InsertOnSubmitInDepotDetailBill(DepotManagementDataContext ctx, S_InDepotDetailBill detailInfo)
        {
            if (detailInfo == null)
            {
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(detailInfo.InDepotBillID))
            {
                throw new Exception("【单据号】获取失败，请重新再试");
            }

            ctx.S_InDepotDetailBill.InsertOnSubmit(detailInfo);
        }

        /// <summary>
        /// 赋值入库明细信息
        /// </summary>
        /// <param name="originalInfo">原始信息</param>
        /// <returns>入库明细对象</returns>
        S_FetchGoodsDetailBill AssignmentInfo(S_FetchGoodsDetailBill originalInfo)
        {
            S_FetchGoodsDetailBill lnqResult = new S_FetchGoodsDetailBill();

            lnqResult.AssociatedBillNo = originalInfo.AssociatedBillNo;
            lnqResult.AssociatedBillType = originalInfo.AssociatedBillType;
            lnqResult.BatchNo = originalInfo.BatchNo;
            lnqResult.BillTime = originalInfo.BillTime;
            lnqResult.DepartDirector = originalInfo.DepartDirector;
            lnqResult.Department = originalInfo.Department;
            lnqResult.Depot = originalInfo.Depot;
            lnqResult.DepotManager = originalInfo.DepotManager;
            lnqResult.FetchBIllID = originalInfo.FetchBIllID;
            lnqResult.FetchCount = originalInfo.FetchCount;
            lnqResult.FillInDate = originalInfo.FillInDate;
            lnqResult.FillInPersonnel = originalInfo.FillInPersonnel;
            lnqResult.FinanceSignatory = originalInfo.FinanceSignatory;
            lnqResult.GoodsID = originalInfo.GoodsID;
            lnqResult.ID = originalInfo.ID;
            lnqResult.OperationType = originalInfo.OperationType;
            lnqResult.Price = originalInfo.Price;
            lnqResult.Provider = originalInfo.Provider;
            lnqResult.ProviderBatchNo = originalInfo.ProviderBatchNo;
            lnqResult.Remark = originalInfo.Remark;
            lnqResult.StorageID = originalInfo.StorageID;
            lnqResult.UnitPrice = originalInfo.UnitPrice;
            lnqResult.Using = originalInfo.Using;

            return lnqResult;
        }

        /// <summary>
        /// 工具入库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="indepotDetailInfo">入库业务对象</param>
        void InDepotToolsInfo(DepotManagementDataContext ctx, S_InDepotDetailBill indepotDetailInfo)
        {
            if (indepotDetailInfo == null)
            {
                return;
            }

            IToolsManage serverTools = ServerModule.ServerModuleFactory.GetServerModule<IToolsManage>();

            if (serverTools.IsTools(indepotDetailInfo.GoodsID))
            {
                S_MachineAccount_Tools toolsInfo = new S_MachineAccount_Tools();

                toolsInfo.GoodsID = indepotDetailInfo.GoodsID;
                toolsInfo.Provider = indepotDetailInfo.Provider;
                toolsInfo.StockCount = (decimal)indepotDetailInfo.InDepotCount;
                toolsInfo.StorageCode = indepotDetailInfo.StorageID;

                serverTools.OpertionInfo(ctx, toolsInfo);
                serverTools.DayToDayAccount(ctx, toolsInfo, indepotDetailInfo.InDepotBillID);
            }
        }

        /// <summary>
        /// 量检具入库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="objInfo">入库业务对象</param>
        void InDepotGuagesInfo(DepotManagementDataContext ctx, object objInfo)
        {
            if (objInfo == null)
            {
                return;
            }

            int goodsID = 0, operationType = 0;
            string storageID = null, billNo = null;

            if (objInfo is S_FetchGoodsDetailBill)
            {
                goodsID = ((S_FetchGoodsDetailBill)objInfo).GoodsID;
                storageID = ((S_FetchGoodsDetailBill)objInfo).StorageID;
                billNo = ((S_FetchGoodsDetailBill)objInfo).FetchBIllID;
                operationType = ((S_FetchGoodsDetailBill)objInfo).OperationType;
            }
            else if (objInfo is S_InDepotDetailBill)
            {
                goodsID = ((S_InDepotDetailBill)objInfo).GoodsID;
                storageID = ((S_InDepotDetailBill)objInfo).StorageID;
                billNo = ((S_InDepotDetailBill)objInfo).InDepotBillID;
                operationType = ((S_InDepotDetailBill)objInfo).OperationType;
            }
            else
            {
                return;
            }

            IGaugeManage serverGauge = ServerModule.ServerModuleFactory.GetServerModule<IGaugeManage>();

            if (UniversalFunction.GetGoodsType(goodsID, storageID) == CE_GoodsType.量检具)
            {
                serverGauge.OperationGaugeStandingBook(ctx, billNo, CE_MarketingType.出库, operationType);
            }
            else
            {
                if (storageID == "07")
                {
                    throw new Exception("非【量检具】无法入库【量检具库】");
                }
            }
        }

        /// <summary>
        /// 车间入库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="indepotDetailInfo">入库业务对象</param>
        void InDepotWorkShop(DepotManagementDataContext ctx, S_InDepotDetailBill indepotDetailInfo)
        {
            if (indepotDetailInfo == null)
            {
                return;
            }

            Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();
            WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(ctx, indepotDetailInfo.FillInPersonnel);

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块])
                && tempWSCode != null)
            {
                WS_Subsidiary tempSubsidiary = new WS_Subsidiary();

                tempSubsidiary.BillTime = ServerTime.Time;
                tempSubsidiary.Applicant = UniversalFunction.GetStorageName(ctx, indepotDetailInfo.StorageID);
                tempSubsidiary.Affirm = indepotDetailInfo.AffrimPersonnel;
                tempSubsidiary.AffirmDate = ServerTime.Time;
                tempSubsidiary.BatchNo = indepotDetailInfo.BatchNo;
                tempSubsidiary.BillNo = indepotDetailInfo.InDepotBillID;
                tempSubsidiary.GoodsID = indepotDetailInfo.GoodsID;
                tempSubsidiary.Provider = indepotDetailInfo.Provider;
                tempSubsidiary.OperationCount = (decimal)indepotDetailInfo.InDepotCount < 0 ? -(decimal)indepotDetailInfo.InDepotCount : (decimal)indepotDetailInfo.InDepotCount;
                tempSubsidiary.OperationType = indepotDetailInfo.OperationType;
                tempSubsidiary.Proposer = indepotDetailInfo.FillInPersonnel;
                tempSubsidiary.ProposerDate = indepotDetailInfo.FillInDate == null ? ServerTime.Time : Convert.ToDateTime(indepotDetailInfo.FillInDate);
                tempSubsidiary.UnitPrice = indepotDetailInfo.FactUnitPrice;
                tempSubsidiary.WSCode = tempWSCode.WSCode;
                tempSubsidiary.Remark = indepotDetailInfo.Remark;

                Service_Manufacture_WorkShop.IWorkShopStock serverWSStock =
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopStock>();
                serverWSStock.OperationSubsidiary(ctx, tempSubsidiary);
            }
        }

        /// <summary>
        /// 工具出库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchGoodsDetailInfo">出库业务对象</param>
        void FetchToolsInfo(DepotManagementDataContext ctx, S_FetchGoodsDetailBill fetchGoodsDetailInfo)
        {
            if (fetchGoodsDetailInfo == null)
            {
                return;
            }

            IToolsManage serverTools = ServerModule.ServerModuleFactory.GetServerModule<IToolsManage>();

            if (serverTools.IsTools(ctx, fetchGoodsDetailInfo.GoodsID))
            {
                if (fetchGoodsDetailInfo.StorageID != null && fetchGoodsDetailInfo.StorageID.Trim().Length > 0)
                {
                    S_MachineAccount_Tools toolsInfo_Storage = new S_MachineAccount_Tools();

                    toolsInfo_Storage.GoodsID = fetchGoodsDetailInfo.GoodsID;
                    toolsInfo_Storage.Provider = fetchGoodsDetailInfo.Provider;
                    toolsInfo_Storage.StockCount = -(decimal)fetchGoodsDetailInfo.FetchCount;
                    toolsInfo_Storage.StorageCode = fetchGoodsDetailInfo.StorageID;

                    serverTools.OpertionInfo(ctx, toolsInfo_Storage);
                    serverTools.DayToDayAccount(ctx, toolsInfo_Storage, fetchGoodsDetailInfo.FetchBIllID);
                }

                CE_SubsidiaryOperationType operationType =
                    (CE_SubsidiaryOperationType)Enum.ToObject(typeof(CE_SubsidiaryOperationType), fetchGoodsDetailInfo.OperationType);

                if (operationType != CE_SubsidiaryOperationType.营销出库 && operationType != CE_SubsidiaryOperationType.营销退库)
                {
                    S_MachineAccount_Tools toolsInfo_Department = new S_MachineAccount_Tools();

                    toolsInfo_Department.GoodsID = fetchGoodsDetailInfo.GoodsID;
                    toolsInfo_Department.Provider = fetchGoodsDetailInfo.Provider;
                    toolsInfo_Department.StockCount = (decimal)fetchGoodsDetailInfo.FetchCount;
                    toolsInfo_Department.StorageCode = UniversalFunction.GetPersonnelInfo(ctx, fetchGoodsDetailInfo.FillInPersonnel).部门编码;

                    serverTools.OpertionInfo(ctx, toolsInfo_Department);
                    serverTools.DayToDayAccount(ctx, toolsInfo_Department, fetchGoodsDetailInfo.FetchBIllID);
                }
            }
        }

        /// <summary>
        /// 量检具出库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="objInfo">出库业务对象</param>
        void FetchGaugeInfo(DepotManagementDataContext ctx, object objInfo)
        {
            if (objInfo == null)
            {
                return;
            }

            int goodsID = 0, operationType = 0;
            string storageID = null, billNo = null;

            if (objInfo is S_FetchGoodsDetailBill)
            {
                goodsID = ((S_FetchGoodsDetailBill)objInfo).GoodsID;
                storageID = ((S_FetchGoodsDetailBill)objInfo).StorageID;
                billNo = ((S_FetchGoodsDetailBill)objInfo).FetchBIllID;
                operationType = ((S_FetchGoodsDetailBill)objInfo).OperationType;
            }
            else if (objInfo is S_InDepotDetailBill)
            {
                goodsID = ((S_InDepotDetailBill)objInfo).GoodsID;
                storageID = ((S_InDepotDetailBill)objInfo).StorageID;
                billNo = ((S_InDepotDetailBill)objInfo).InDepotBillID;
                operationType = ((S_InDepotDetailBill)objInfo).OperationType;
            }
            else
            {
                return;
            }

            IGaugeManage serverGauge = ServerModule.ServerModuleFactory.GetServerModule<IGaugeManage>();

            if (UniversalFunction.GetGoodsType(goodsID, storageID) == CE_GoodsType.量检具)
            {
                serverGauge.OperationGaugeStandingBook(ctx, billNo, CE_MarketingType.出库, operationType);
            }
        }

        /// <summary>
        /// 车间出库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchGoodsDetailInfo">出库业务对象</param>
        void FetchWorkShop(DepotManagementDataContext ctx, S_FetchGoodsDetailBill fetchGoodsDetailInfo)
        {
            if (fetchGoodsDetailInfo == null)
            {
                return;
            }

            Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();
            WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(ctx, fetchGoodsDetailInfo.FillInPersonnel);

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块])
                && tempWSCode != null)
            {
                WS_Subsidiary tempSubsidiary = new WS_Subsidiary();

                tempSubsidiary.BillTime = ServerTime.Time;
                tempSubsidiary.Applicant = UniversalFunction.GetStorageName(ctx, fetchGoodsDetailInfo.StorageID);
                tempSubsidiary.Affirm = fetchGoodsDetailInfo.DepotManager;
                tempSubsidiary.AffirmDate = ServerTime.Time;
                tempSubsidiary.BatchNo = fetchGoodsDetailInfo.BatchNo;
                tempSubsidiary.BillNo = fetchGoodsDetailInfo.FetchBIllID;
                tempSubsidiary.GoodsID = fetchGoodsDetailInfo.GoodsID;
                tempSubsidiary.Provider = fetchGoodsDetailInfo.Provider;
                tempSubsidiary.OperationCount = (decimal)fetchGoodsDetailInfo.FetchCount < 0 ? -(decimal)fetchGoodsDetailInfo.FetchCount : (decimal)fetchGoodsDetailInfo.FetchCount;
                tempSubsidiary.OperationType = fetchGoodsDetailInfo.OperationType;
                tempSubsidiary.Proposer = fetchGoodsDetailInfo.FillInPersonnel;
                tempSubsidiary.ProposerDate = fetchGoodsDetailInfo.FillInDate == null ? ServerTime.Time : Convert.ToDateTime(fetchGoodsDetailInfo.FillInDate);
                tempSubsidiary.UnitPrice = fetchGoodsDetailInfo.UnitPrice;
                tempSubsidiary.WSCode = tempWSCode.WSCode;
                tempSubsidiary.Remark = fetchGoodsDetailInfo.Remark;

                Service_Manufacture_WorkShop.IWorkShopStock serverWSStock =
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopStock>();
                serverWSStock.OperationSubsidiary(ctx, tempSubsidiary);
            }
        }

        /// <summary>
        /// 借还货出库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchGoodsDetailInfo">出库业务对象</param>
        void FetchLendReturn(DepotManagementDataContext ctx, S_FetchGoodsDetailBill fetchGoodsDetailInfo)
        {
            if (fetchGoodsDetailInfo == null)
            {
                return;
            }

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启借还货账务管理]))
            {
                BASE_SubsidiaryOperationType operationType = UniversalFunction.GetSubsidiaryOperationType(ctx, fetchGoodsDetailInfo.OperationType);

                if (operationType.LendReturnType != null)
                {
                    S_ProductLendReturnDetail tempDetail = new S_ProductLendReturnDetail();

                    tempDetail.Affirm = fetchGoodsDetailInfo.DepotManager;
                    tempDetail.AffirmDate = fetchGoodsDetailInfo.BillTime;
                    tempDetail.BatchNo = fetchGoodsDetailInfo.BatchNo;
                    tempDetail.Provider = fetchGoodsDetailInfo.Provider;
                    tempDetail.BillNo = fetchGoodsDetailInfo.FetchBIllID;
                    tempDetail.BillTime = fetchGoodsDetailInfo.BillTime;
                    tempDetail.Credit = fetchGoodsDetailInfo.StorageID;
                    tempDetail.Debtor = UniversalFunction.GetPersonnelInfo(ctx, fetchGoodsDetailInfo.FillInPersonnel).部门编码;
                    tempDetail.GoodsID = fetchGoodsDetailInfo.GoodsID;
                    tempDetail.OperationCount = fetchGoodsDetailInfo.FetchCount < 0 ? -(decimal)fetchGoodsDetailInfo.FetchCount : (decimal)fetchGoodsDetailInfo.FetchCount;
                    tempDetail.OperationType = fetchGoodsDetailInfo.OperationType;
                    tempDetail.Proposer = fetchGoodsDetailInfo.FillInPersonnel;
                    tempDetail.ProposerDate = fetchGoodsDetailInfo.FillInDate == null ? ServerTime.Time : Convert.ToDateTime(fetchGoodsDetailInfo.FillInDate);
                    tempDetail.Remark = fetchGoodsDetailInfo.Remark;
                    tempDetail.UnitPrice = fetchGoodsDetailInfo.UnitPrice;

                    IProductLendReturnService serverLendReturn = ServerModuleFactory.GetServerModule<IProductLendReturnService>();
                    serverLendReturn.OperationDetailRecord(ctx, tempDetail);
                }
            }
        }
        
        /// <summary>
        /// 借还货出库业务的处理
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="fetchGoodsDetailInfo">出库业务对象</param>
        void FetchLendReturn_MaterialRequisition(DepotManagementDataContext ctx, S_FetchGoodsDetailBill fetchGoodsDetailInfo)
        {
            if (fetchGoodsDetailInfo == null)
            {
                return;
            }

            if (Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启借还货账务管理]))
            {
                BASE_SubsidiaryOperationType operationType = UniversalFunction.GetSubsidiaryOperationType(ctx, fetchGoodsDetailInfo.OperationType);

                if (operationType.LendReturnType != null)
                {
                    IProductLendReturnService serverLendReturn = ServerModuleFactory.GetServerModule<IProductLendReturnService>();

                    var varData = from a in ctx.View_S_MaterialRequisitionProductReturnList
                                  where a.单据号 == fetchGoodsDetailInfo.FetchBIllID
                                  && a.还账物品ID == fetchGoodsDetailInfo.GoodsID
                                  && a.还账物品批次号 == fetchGoodsDetailInfo.BatchNo
                                  && a.还账物品供应商 == fetchGoodsDetailInfo.Provider
                                  select a;

                    if (varData.Count() > 0)
                    {
                        foreach (var item1 in varData)
                        {
                            S_ProductLendRecord tempRecord =
                                serverLendReturn.GetStockSingleInfo(ctx, BasicInfo.DeptCode, fetchGoodsDetailInfo.StorageID, item1.欠账物品ID,
                                item1.欠账物品批次号, item1.欠账物品供应商);

                            if (tempRecord != null)
                            {
                                S_ProductLendReturnDetail tempDetail = new S_ProductLendReturnDetail();

                                tempDetail.Affirm = fetchGoodsDetailInfo.DepotManager;
                                tempDetail.AffirmDate = ServerTime.Time;
                                tempDetail.BatchNo = item1.欠账物品批次号;
                                tempDetail.Provider = item1.欠账物品供应商;
                                tempDetail.BillNo = fetchGoodsDetailInfo.FetchBIllID;
                                tempDetail.BillTime = ServerTime.Time;
                                tempDetail.Credit = fetchGoodsDetailInfo.StorageID;
                                tempDetail.Debtor = BasicInfo.DeptCode;
                                tempDetail.GoodsID = item1.欠账物品ID;
                                tempDetail.OperationCount = item1.还账数量;
                                tempDetail.OperationType = fetchGoodsDetailInfo.OperationType;
                                tempDetail.Proposer = fetchGoodsDetailInfo.FillInPersonnel;
                                tempDetail.ProposerDate =
                                    fetchGoodsDetailInfo.FillInDate == null ? ServerTime.Time : Convert.ToDateTime(fetchGoodsDetailInfo.FillInDate);
                                tempDetail.UnitPrice = 0;

                                serverLendReturn.OperationDetailRecord(ctx, tempDetail);
                            }
                        }
                    }
                }
            }
        }
    }
}
