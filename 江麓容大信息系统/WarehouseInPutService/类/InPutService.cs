using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;
using ServerModule;
using FlowControlService;

namespace Service_Manufacture_Storage
{
    class InPutService : IInPutService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_WarehouseInPut_InPut GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseInPut_InPut
                          where a.BillNo == billNo
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_WarehouseInPut_InPutDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseInPut_InPutDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_WarehouseInPut_InPut billInfo, List<View_Business_WarehouseInPut_InPutDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_WarehouseInPut_InPut
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_WarehouseInPut_InPut lnqBill = varData.Single();

                    lnqBill.ApplyingDepartment = billInfo.ApplyingDepartment;
                    lnqBill.BillType = billInfo.BillType;
                    lnqBill.StorageID = billInfo.StorageID;
                    lnqBill.Remark = billInfo.Remark;
                    lnqBill.BillTypeDetail = billInfo.BillTypeDetail;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_WarehouseInPut_InPut.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varDetail = from a in ctx.Business_WarehouseInPut_InPutDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_WarehouseInPut_InPutDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_WarehouseInPut_InPutDetail item in detailInfo)
                {
                    Business_WarehouseInPut_InPutDetail lnqDetail = new Business_WarehouseInPut_InPutDetail();

                    lnqDetail.BatchNo = item.批次号;
                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.BillRelate = item.关联业务;
                    lnqDetail.GoodsCount = item.数量;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.Provider = item.供应商;
                    lnqDetail.Remark = item.备注;

                    if (!IsInput(lnqDetail))
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(lnqDetail.GoodsID) + "【批次号】:" 
                            + lnqDetail.BatchNo + "【供应商】:" + lnqDetail.Provider + "，此物品无【检验报告】/【判定报告】");
                    }

                    ctx.Business_WarehouseInPut_InPutDetail.InsertOnSubmit(lnqDetail);
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 赋值账务信息_出
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        /// <param name="detail1">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_FetchGoodsDetailBill AssignDetailInfo_Out(DepotManagementDataContext ctx, Business_WarehouseInPut_InPut lnqInPut,
            View_Business_WarehouseInPut_InPutDetail detail1)
        {
            ServerModule.IStoreServer storeService = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IStoreServer>();
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            CommonProcessInfo processInfo = new CommonProcessInfo();
            CE_InPutBusinessType inPutType =
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(lnqInPut.BillType);

            S_FetchGoodsDetailBill fetchDetail = new S_FetchGoodsDetailBill();

            fetchDetail.AssociatedBillNo = detail1.关联业务;
            fetchDetail.AssociatedBillType = CE_BillTypeEnum.入库申请单.ToString();
            fetchDetail.BatchNo = detail1.批次号;
            fetchDetail.BillTime = ServerTime.Time;
            fetchDetail.DepartDirector = "";
            fetchDetail.Department = lnqInPut.ApplyingDepartment;
            fetchDetail.Depot = UniversalFunction.GetGoodsInfo(detail1.物品ID).物品类别名称;

            fetchDetail.Price = Math.Round(detail1.单价 * detail1.数量, 2);
            fetchDetail.UnitPrice = detail1.单价;

            processInfo = serverFlow.GetFlowData(detail1.关联业务).First();

            fetchDetail.FillInDate = Convert.ToDateTime(processInfo.时间);
            fetchDetail.FillInPersonnel = processInfo.人员;

            fetchDetail.FetchBIllID = lnqInPut.BillNo;
            fetchDetail.FetchCount = -detail1.数量;
            fetchDetail.GoodsID = detail1.物品ID;
            fetchDetail.OperationType = (int)GlobalObject.EnumOperation.InPutBusinessTypeConvertToSubsidiaryOperationType(inPutType);
            fetchDetail.Provider = detail1.供应商;
            fetchDetail.Remark = detail1.备注;
            fetchDetail.StorageID = lnqInPut.StorageID;

            DataTable stockTable = storeService.GetGoodsStockInfo(fetchDetail.GoodsID, fetchDetail.BatchNo, fetchDetail.Provider, fetchDetail.StorageID);

            if (stockTable != null && stockTable.Rows.Count > 0)
            {
                fetchDetail.ProviderBatchNo = stockTable.Rows[0]["ProviderBatchNo"].ToString();
            }
            else
            {
                fetchDetail.ProviderBatchNo = "";
            }

            fetchDetail.Using = lnqInPut.BillTypeDetail;

            return fetchDetail;
        }

        /// <summary>
        /// 赋值账务信息_入
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        /// <param name="detail1">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_InDepotDetailBill AssignDetailInfo_In(DepotManagementDataContext ctx, Business_WarehouseInPut_InPut lnqInPut,
            View_Business_WarehouseInPut_InPutDetail detail1)
        {
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            CommonProcessInfo processInfo = new CommonProcessInfo();
            CE_InPutBusinessType inPutType =
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(lnqInPut.BillType);


            S_InDepotDetailBill inDepotDetail = new S_InDepotDetailBill();

            inDepotDetail.AffrimPersonnel = BasicInfo.LoginName;
            inDepotDetail.BatchNo = detail1.批次号;
            inDepotDetail.BillTime = ServerTime.Time;
            inDepotDetail.Department = UniversalFunction.GetDeptName(lnqInPut.ApplyingDepartment);
            inDepotDetail.Depot = UniversalFunction.GetGoodsInfo(detail1.物品ID).物品类别名称;

            inDepotDetail.FactPrice = Math.Round(detail1.单价 * detail1.数量, 2);
            inDepotDetail.FactUnitPrice = detail1.单价;
            inDepotDetail.Price = inDepotDetail.FactPrice;
            inDepotDetail.UnitPrice = inDepotDetail.FactUnitPrice;
            inDepotDetail.InvoicePrice = null;
            inDepotDetail.InvoiceUnitPrice = null;

            processInfo = serverFlow.GetFlowData(detail1.关联业务).First();

            inDepotDetail.FillInDate = Convert.ToDateTime(processInfo.时间);
            inDepotDetail.FillInPersonnel = processInfo.人员;

            inDepotDetail.InDepotBillID = lnqInPut.BillNo;
            inDepotDetail.InDepotCount = detail1.数量;
            inDepotDetail.GoodsID = detail1.物品ID;
            inDepotDetail.OperationType = (int)GlobalObject.EnumOperation.InPutBusinessTypeConvertToSubsidiaryOperationType(inPutType);
            inDepotDetail.Provider = detail1.供应商;
            inDepotDetail.Remark = detail1.备注;
            inDepotDetail.StorageID = lnqInPut.StorageID;

            return inDepotDetail;
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        /// <param name="detail1">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext ctx, Business_WarehouseInPut_InPut lnqInPut,
            View_Business_WarehouseInPut_InPutDetail detail1)
        {
            S_Stock tempLnqStock = new S_Stock();

            tempLnqStock.GoodsID = detail1.物品ID;
            tempLnqStock.BatchNo = detail1.批次号;
            tempLnqStock.Date = ServerTime.Time;
            tempLnqStock.Provider = detail1.供应商;
            tempLnqStock.StorageID = lnqInPut.StorageID;
            tempLnqStock.ExistCount = Convert.ToDecimal(detail1.数量);

            return tempLnqStock;
        }

        /// <summary>
        /// 操作账务信息与库存信息_入
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        void OperationDetailAndStock_In(DepotManagementDataContext dataContext, Business_WarehouseInPut_InPut lnqInPut)
        {
            string error = "";
            IProductCodeServer productCodeService =
                ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IProductCodeServer>();
            CE_MarketingType marketingType = GlobalObject.EnumOperation.OutPutBusinessTypeConvertToMarketingType(
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqInPut.BillType));
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            List<View_Business_WarehouseInPut_InPutDetail> listDetail = GetListViewDetailInfo(lnqInPut.BillNo);

            foreach (View_Business_WarehouseInPut_InPutDetail detail1 in listDetail)
            {
                if (!productCodeService.UpdateProductStock(dataContext, lnqInPut.BillNo, marketingType.ToString(), lnqInPut.StorageID,
                    (lnqInPut.StorageID == "05" && marketingType == CE_MarketingType.入库) ? true : false, detail1.物品ID, out error))
                {
                    throw new Exception(error);
                }

                S_InDepotDetailBill detailInfo = AssignDetailInfo_In(dataContext, lnqInPut, detail1);
                S_Stock stockInfo = AssignStockInfo(dataContext, lnqInPut, detail1);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 操作账务信息与库存信息_出
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        void OperationDetailAndStock_Out(DepotManagementDataContext dataContext, Business_WarehouseInPut_InPut lnqInPut)
        {
            string error = "";
            IProductCodeServer productCodeService =
                ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IProductCodeServer>();
            CE_MarketingType marketingType = GlobalObject.EnumOperation.OutPutBusinessTypeConvertToMarketingType(
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqInPut.BillType));
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            List<View_Business_WarehouseInPut_InPutDetail> listDetail = GetListViewDetailInfo(lnqInPut.BillNo);

            foreach (View_Business_WarehouseInPut_InPutDetail detail1 in listDetail)
            {
                if (!productCodeService.UpdateProductStock(dataContext, lnqInPut.BillNo, marketingType.ToString(), lnqInPut.StorageID,
                    (lnqInPut.StorageID == "05" && marketingType == CE_MarketingType.入库) ? true : false, detail1.物品ID, out error))
                {
                    throw new Exception(error);
                }

                S_FetchGoodsDetailBill detailInfo = AssignDetailInfo_Out(dataContext, lnqInPut, detail1);
                S_Stock stockInfo = AssignStockInfo(dataContext, lnqInPut, detail1);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessFetchGoodsDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 结束业务
        /// </summary>
        /// <param name="billNo">业务编号</param>
        public void FinishBill(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                Business_WarehouseInPut_InPut lnqInPut = GetSingleBillInfo(billNo);
                CE_InPutBusinessType inPutType = 
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(lnqInPut.BillType);

                switch (inPutType)
                {
                    case CE_InPutBusinessType.生产采购:
                    case CE_InPutBusinessType.普通采购:
                    case CE_InPutBusinessType.委外采购:
                    case CE_InPutBusinessType.样品采购:
                    case CE_InPutBusinessType.自制件入库:
                    case CE_InPutBusinessType.营销入库:;
                        OperationDetailAndStock_In(ctx, lnqInPut);
                        break;
                    case CE_InPutBusinessType.领料退库:
                    case CE_InPutBusinessType.营销退库:
                        OperationDetailAndStock_Out(ctx, lnqInPut);
                        break;
                    default:
                        break;
                }

                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.入库单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_WarehouseInPut_InPut
                              where a.BillNo == billNo
                              select a;

                ctx.Business_WarehouseInPut_InPut.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
                serverFlow.FlowDelete(ctx, billNo);

                ctx.Transaction.Commit();
                billNoControl.CancelBill(billNo);
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_WarehouseInPut_InPut
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseInPut_InPut
                          where a.BillNo == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public DataTable GetReferenceInfo(string billType, string deptCode, bool isRepeat)
        {
            string error = "";
            DataTable result = new DataTable();

            if (billType == null || billType.Trim().Length == 0)
            {
                throw new Exception("请选择业务类型");
            }

            Hashtable hashTable = new Hashtable();

            hashTable.Add("@BillType", billType);
            hashTable.Add("@DeptCode", deptCode == null ? "" : deptCode);
            hashTable.Add("@IsRepeat", isRepeat);

            result = GlobalObject.DatabaseServer.QueryInfoPro("Business_WarehouseInPut_InPut_ReferenceBill", hashTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            return result;
        }

        public List<View_Business_WarehouseInPut_InPutDetail> GetReferenceListViewDetail(string billNo, List<string> listBillNo, bool isRepeat)
        {
            string error = "";
            List<View_Business_WarehouseInPut_InPutDetail> listResult = new List<View_Business_WarehouseInPut_InPutDetail>();

            if (listBillNo.Count == 0)
            {
                throw new Exception("选择记录为空");
            }

            string orderFormNum = "";

            foreach (string item in listBillNo)
            {
                orderFormNum += "'" + item + "',";
            }

            orderFormNum = orderFormNum.Substring(0, orderFormNum.Length - 1);

            Hashtable hashTable = new Hashtable();

            hashTable.Add("@ListBillNo", orderFormNum);
            hashTable.Add("@IsRepeat", isRepeat);

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Business_WarehouseInPut_InPut_ReferenceDetail", hashTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                foreach (DataRow dr in tempTable.Rows)
                {
                    View_Business_WarehouseInPut_InPutDetail tempLnq = new View_Business_WarehouseInPut_InPutDetail();

                    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(Convert.ToInt32(dr["GoodsID"]));

                    tempLnq.单据号 = billNo;
                    tempLnq.单位 = goodsInfo.单位;
                    tempLnq.供应商 = dr["Provider"].ToString();
                    tempLnq.关联业务 = dr["BillNo"].ToString();
                    tempLnq.物品ID = Convert.ToInt32(dr["GoodsID"]);
                    tempLnq.图号型号 = goodsInfo.图号型号;
                    tempLnq.物品名称 = goodsInfo.物品名称;
                    tempLnq.规格 = goodsInfo.规格;
                    tempLnq.数量 = Convert.ToDecimal(dr["GoodsCount"]);

                    DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                    var varData = from a in ctx.Business_WarehouseInPut_AOGDetail
                                  where a.BillRelate == tempLnq.关联业务
                                  && a.GoodsID == tempLnq.物品ID
                                  && a.Provider == tempLnq.供应商
                                  select a;

                    if (varData.Count() == 1)
                    {
                        tempLnq.批次号 = varData.Single().BatchNo;
                    }
                    else
                    {
                        tempLnq.批次号 = dr["BatchNo"].ToString();
                    }

                    listResult.Add(tempLnq);
                }
            }

            return listResult;
        }

        public decimal GetUnitPrice(CE_InPutBusinessType businessType, string relateBillNo, int goodsID, string batchNo, string storageID)
        {
            try
            {
                decimal resultUnitPrice = 0;

                switch (businessType)
                {
                    case CE_InPutBusinessType.生产采购:
                    case CE_InPutBusinessType.普通采购:
                    case CE_InPutBusinessType.委外采购:
                    case CE_InPutBusinessType.样品采购:
                        break;
                    case CE_InPutBusinessType.自制件入库:
                        resultUnitPrice = 0;
                        break;
                    case CE_InPutBusinessType.领料退库:
                        IStoreServer storeService = ServerModuleFactory.GetServerModule<IStoreServer>();
                        resultUnitPrice = storeService.GetGoodsUnitPrice(goodsID, batchNo, storageID);
                        break;
                    case CE_InPutBusinessType.营销入库:
                    case CE_InPutBusinessType.营销退库:
                        break;
                    default:
                        break;
                }

                return resultUnitPrice;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        bool IsInput(Business_WarehouseInPut_InPutDetail detail)
        {
            string error = "";

            if (detail.GoodsCount == 0)
            {
                return true;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            Business_WarehouseInPut_RequisitionDetail requistionDetail = new Business_WarehouseInPut_RequisitionDetail();

            var varData = from a in ctx.Business_WarehouseInPut_RequisitionDetail
                          where a.BillNo == detail.BillRelate
                          && a.GoodsID == detail.GoodsID
                          && a.BatchNo == detail.BatchNo
                          && a.Provider == detail.Provider
                          select a;

            if (varData.Count() == 0)
            {
                varData = from a in ctx.Business_WarehouseInPut_RequisitionDetail
                          where a.BillNo == detail.BillRelate
                          && a.GoodsID == detail.GoodsID
                          && a.Provider == detail.Provider
                          select a;

                if (varData.Count() == 0)
                {
                    return false;
                }
            }

            requistionDetail = varData.First();

            if (requistionDetail.IsCheck)
            {
                //string strSql = " IF not (object_id('tempdb.dbo.#tempTable1') is null)  " +
                //                " drop table tempdb.dbo.#tempTable1;  " +
                //                " Create Table #tempTable1 (BillNo varchar(50));  " +
                //                " exec dbo.Business_GetReferenceBillNo_Backwards '判定报告','"+ requistionDetail.BillNo +"', "+ 
                //                requistionDetail.GoodsID +", '"+ requistionDetail.BatchNo +"', '"+ requistionDetail.Provider +"'" +
                //                " select * from tempdb.dbo.#tempTable1";

                Hashtable hsTable = new Hashtable();

                hsTable.Add("@BillType", CE_BillTypeEnum.检验报告.ToString());
                hsTable.Add("@BillNo", requistionDetail.BillNo);
                hsTable.Add("@GoodsID", requistionDetail.GoodsID);
                hsTable.Add("@BatchNo", requistionDetail.BatchNo);
                hsTable.Add("@Provider", requistionDetail.Provider);

                DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Business_GetReferenceBillNo_Backwards_Incipit", hsTable, out error);

                if (tempTable == null || tempTable.Rows.Count == 0)
                {
                    hsTable = new Hashtable();

                    hsTable.Add("@BillType", CE_BillTypeEnum.判定报告.ToString());
                    hsTable.Add("@BillNo", requistionDetail.BillNo);
                    hsTable.Add("@GoodsID", requistionDetail.GoodsID);
                    hsTable.Add("@BatchNo", requistionDetail.BatchNo);
                    hsTable.Add("@Provider", requistionDetail.Provider);

                    tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Business_GetReferenceBillNo_Backwards_Incipit", hsTable, out error);

                    if (tempTable == null || tempTable.Rows.Count == 0)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
