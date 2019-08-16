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
    class OutPutService : IOutPutService
    {
        /// <summary>
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_WarehouseOutPut_OutPut GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_WarehouseOutPut_OutPut
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
        public List<View_Business_WarehouseOutPut_OutPutDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_WarehouseOutPut_OutPutDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 保存业务信息
        /// </summary>
        /// <param name="billInfo">业务总单信息</param>
        /// <param name="detailInfo">业务明细信息</param>
        public void SaveInfo(Business_WarehouseOutPut_OutPut billInfo, List<View_Business_WarehouseOutPut_OutPutDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_WarehouseOutPut_OutPut
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_WarehouseOutPut_OutPut lnqBill = varData.Single();

                    lnqBill.ApplyingDepartment = billInfo.ApplyingDepartment;
                    lnqBill.BillType = billInfo.BillType;
                    lnqBill.StorageID = billInfo.StorageID;
                    lnqBill.Remark = billInfo.Remark;
                    lnqBill.BillTypeDetail = billInfo.BillTypeDetail;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_WarehouseOutPut_OutPut.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varDetail = from a in ctx.Business_WarehouseOutPut_OutPutDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_WarehouseOutPut_OutPutDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_WarehouseOutPut_OutPutDetail item in detailInfo)
                {
                    Business_WarehouseOutPut_OutPutDetail lnqDetail = new Business_WarehouseOutPut_OutPutDetail();

                    lnqDetail.BatchNo = item.批次号;
                    lnqDetail.BillNo = billInfo.BillNo;
                    lnqDetail.BillRelate = item.关联业务;
                    lnqDetail.GoodsCount = item.数量;
                    lnqDetail.GoodsID = item.物品ID;
                    lnqDetail.Provider = item.供应商;
                    lnqDetail.Remark = item.备注;

                    ctx.Business_WarehouseOutPut_OutPutDetail.InsertOnSubmit(lnqDetail);
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
        S_FetchGoodsDetailBill AssignDetailInfo_Out(DepotManagementDataContext ctx, Business_WarehouseOutPut_OutPut lnqOutPut,
            View_Business_WarehouseOutPut_OutPutDetail detail1)
        {
            ServerModule.IStoreServer storeService = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IStoreServer>();
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            CommonProcessInfo processInfo = new CommonProcessInfo();
            CE_OutPutBusinessType OutPutType = 
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqOutPut.BillType);

            S_FetchGoodsDetailBill fetchDetail = new S_FetchGoodsDetailBill();

            fetchDetail.AssociatedBillNo = detail1.关联业务;
            fetchDetail.AssociatedBillType = CE_BillTypeEnum.入库申请单.ToString();
            fetchDetail.BatchNo = detail1.批次号;
            fetchDetail.BillTime = ServerTime.Time;
            fetchDetail.DepartDirector = "";
            fetchDetail.Department = lnqOutPut.ApplyingDepartment;
            fetchDetail.Depot = UniversalFunction.GetGoodsInfo(detail1.物品ID).物品类别名称;

            fetchDetail.Price = 0;
            //Math.Round(detail1.单价 * detail1.数量, 2);
            fetchDetail.UnitPrice = 0;
            //detail1.单价;

            processInfo = serverFlow.GetFlowData(detail1.关联业务).First();

            fetchDetail.FillInDate = Convert.ToDateTime(processInfo.时间);
            fetchDetail.FillInPersonnel = processInfo.人员;

            fetchDetail.FetchBIllID = lnqOutPut.BillNo;
            fetchDetail.FetchCount = detail1.数量;
            fetchDetail.GoodsID = detail1.物品ID;
            fetchDetail.OperationType = (int)GlobalObject.EnumOperation.OutPutBusinessTypeConvertToSubsidiaryOperationType(OutPutType);
            fetchDetail.Provider = detail1.供应商;
            fetchDetail.Remark = detail1.备注;
            fetchDetail.StorageID = lnqOutPut.StorageID;

            DataTable stockTable = storeService.GetGoodsStockInfo(fetchDetail.GoodsID, fetchDetail.BatchNo, fetchDetail.Provider, fetchDetail.StorageID);

            if (stockTable != null && stockTable.Rows.Count > 0)
            {
                fetchDetail.ProviderBatchNo = stockTable.Rows[0]["ProviderBatchNo"].ToString();
            }
            else
            {
                fetchDetail.ProviderBatchNo = "";
            }

            fetchDetail.Using = lnqOutPut.BillTypeDetail;

            return fetchDetail;
        }

        /// <summary>
        /// 赋值账务信息_入
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        /// <param name="detail1">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_InDepotDetailBill AssignDetailInfo_In(DepotManagementDataContext ctx, Business_WarehouseOutPut_OutPut lnqOutPut, 
            View_Business_WarehouseOutPut_OutPutDetail detail1)
        {
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            CommonProcessInfo processInfo = new CommonProcessInfo();
            CE_OutPutBusinessType OutPutType = 
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqOutPut.BillType);

            S_InDepotDetailBill inDepotDetail = new S_InDepotDetailBill();

            inDepotDetail.AffrimPersonnel = BasicInfo.LoginName;
            inDepotDetail.BatchNo = detail1.批次号;
            inDepotDetail.BillTime = ServerTime.Time;
            inDepotDetail.Department = UniversalFunction.GetDeptName(lnqOutPut.ApplyingDepartment);
            inDepotDetail.Depot = UniversalFunction.GetGoodsInfo(detail1.物品ID).物品类别名称;

            inDepotDetail.FactPrice = 0;
            //Math.Round(detail1.单价 * detail1.数量, 2);
            inDepotDetail.FactUnitPrice = 0;
            //detail1.单价;
            inDepotDetail.Price = inDepotDetail.FactPrice;
            inDepotDetail.UnitPrice = inDepotDetail.FactUnitPrice;
            inDepotDetail.InvoicePrice = null;
            inDepotDetail.InvoiceUnitPrice = null;

            processInfo = serverFlow.GetFlowData(detail1.关联业务).First();

            inDepotDetail.FillInDate = Convert.ToDateTime(processInfo.时间);
            inDepotDetail.FillInPersonnel = processInfo.人员;

            inDepotDetail.InDepotBillID = lnqOutPut.BillNo;
            inDepotDetail.InDepotCount = -detail1.数量;
            inDepotDetail.GoodsID = detail1.物品ID;
            inDepotDetail.OperationType = (int)GlobalObject.EnumOperation.OutPutBusinessTypeConvertToSubsidiaryOperationType(OutPutType);
            inDepotDetail.Provider = detail1.供应商;
            inDepotDetail.Remark = detail1.备注;
            inDepotDetail.StorageID = lnqOutPut.StorageID;

            return inDepotDetail;
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        /// <param name="detail1">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext ctx, Business_WarehouseOutPut_OutPut lnqOutPut, 
            View_Business_WarehouseOutPut_OutPutDetail detail1)
        {
            S_Stock tempLnqStock = new S_Stock();

            tempLnqStock.GoodsID = detail1.物品ID;
            tempLnqStock.BatchNo = detail1.批次号;
            tempLnqStock.Date = ServerTime.Time;
            tempLnqStock.Provider = detail1.供应商;
            tempLnqStock.StorageID = lnqOutPut.StorageID;
            tempLnqStock.ExistCount = Convert.ToDecimal(detail1.数量);

            return tempLnqStock;
        }

        /// <summary>
        /// 操作账务信息与库存信息_入
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="lnqOutPut">单据信息</param>
        void OperationDetailAndStock_In(DepotManagementDataContext dataContext, Business_WarehouseOutPut_OutPut lnqOutPut)
        {
            string error = "";
            IProductCodeServer productCodeService =
                ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IProductCodeServer>();
            CE_MarketingType marketingType = GlobalObject.EnumOperation.OutPutBusinessTypeConvertToMarketingType(
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqOutPut.BillType));
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            List<View_Business_WarehouseOutPut_OutPutDetail> listDetail = GetListViewDetailInfo(lnqOutPut.BillNo);

            foreach (View_Business_WarehouseOutPut_OutPutDetail detail1 in listDetail)
            {
                if (!productCodeService.UpdateProductStock(dataContext, lnqOutPut.BillNo, marketingType.ToString(), lnqOutPut.StorageID,
                    (lnqOutPut.StorageID == "05" && marketingType == CE_MarketingType.入库) ? true : false, detail1.物品ID, out error))
                {
                    throw new Exception(error);
                }

                S_InDepotDetailBill detailInfo = AssignDetailInfo_In(dataContext, lnqOutPut, detail1);
                S_Stock stockInfo = AssignStockInfo(dataContext, lnqOutPut, detail1);

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
        void OperationDetailAndStock_Out(DepotManagementDataContext dataContext, Business_WarehouseOutPut_OutPut lnqOutPut)
        {
            string error = "";
            IProductCodeServer productCodeService =
                ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IProductCodeServer>();
            CE_MarketingType marketingType = GlobalObject.EnumOperation.OutPutBusinessTypeConvertToMarketingType(
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqOutPut.BillType));
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            List<View_Business_WarehouseOutPut_OutPutDetail> listDetail = GetListViewDetailInfo(lnqOutPut.BillNo);

            foreach (View_Business_WarehouseOutPut_OutPutDetail detail1 in listDetail)
            {
                if (!productCodeService.UpdateProductStock(dataContext, lnqOutPut.BillNo, marketingType.ToString(), lnqOutPut.StorageID,
                    (lnqOutPut.StorageID == "05" && marketingType == CE_MarketingType.入库) ? true : false, detail1.物品ID, out error))
                {
                    throw new Exception(error);
                }

                S_FetchGoodsDetailBill detailInfo = AssignDetailInfo_Out(dataContext, lnqOutPut, detail1);
                S_Stock stockInfo = AssignStockInfo(dataContext, lnqOutPut, detail1);

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
                Business_WarehouseOutPut_OutPut lnqOutPut = GetSingleBillInfo(billNo);
                CE_OutPutBusinessType OutPutType =
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_OutPutBusinessType>(lnqOutPut.BillType);

                switch (OutPutType)
                {
                    case CE_OutPutBusinessType.采购退货:
                    case CE_OutPutBusinessType.自制件退货:
                    case CE_OutPutBusinessType.营销退货:
                        OperationDetailAndStock_In(ctx, lnqOutPut);
                        break;
                    case CE_OutPutBusinessType.领料:
                    case CE_OutPutBusinessType.营销出库:
                        OperationDetailAndStock_Out(ctx, lnqOutPut);
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
                var varData = from a in ctx.Business_WarehouseOutPut_OutPut
                              where a.BillNo == billNo
                              select a;

                ctx.Business_WarehouseOutPut_OutPut.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Business_WarehouseOutPut_OutPut
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

            var varData = from a in ctx.Business_WarehouseOutPut_OutPut
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

            result = GlobalObject.DatabaseServer.QueryInfoPro("Business_WarehouseOutPut_OutPut_ReferenceBill", hashTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            return result;
        }

        public List<View_Business_WarehouseOutPut_OutPutDetail> GetReferenceListViewDetail(string billNo, List<string> listBillNo, bool isRepeat,
            out string deptCode)
        {
            string error = "";
            deptCode = null;
            List<View_Business_WarehouseOutPut_OutPutDetail> listResult = new List<View_Business_WarehouseOutPut_OutPutDetail>();

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

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("Business_WarehouseOutPut_OutPut_ReferenceDetail", hashTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            if (tempTable != null && tempTable.Rows.Count > 0)
            {
                deptCode = tempTable.Rows[0]["ApplyingDepartment"].ToString();

                foreach (DataRow dr in tempTable.Rows)
                {
                    View_Business_WarehouseOutPut_OutPutDetail tempLnq = new View_Business_WarehouseOutPut_OutPutDetail();

                    tempLnq.单据号 = billNo;
                    tempLnq.单位 = dr["单位"].ToString();
                    tempLnq.供应商 = dr["供应商"].ToString();
                    tempLnq.关联业务 = dr["单据号"].ToString();
                    tempLnq.物品ID = Convert.ToInt32(dr["物品ID"]);
                    tempLnq.图号型号 = dr["图号型号"].ToString();
                    tempLnq.物品名称 = dr["物品名称"].ToString();
                    tempLnq.规格 = dr["规格"].ToString();
                    tempLnq.数量 = Convert.ToDecimal(dr["数量"]);
                    tempLnq.批次号 = dr["批次号"].ToString();

                    listResult.Add(tempLnq);
                }
            }

            return listResult;
        }
    }
}
