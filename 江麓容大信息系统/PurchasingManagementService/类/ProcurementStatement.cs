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

namespace Service_Economic_Purchase
{
    class ProcurementStatement : IProcurementStatement
    {
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.采购结算单.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_Settlement_ProcurementStatement
                              where a.BillNo == billNo
                              select a;

                ctx.Business_Settlement_ProcurementStatement.DeleteAllOnSubmit(varData);
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
            var varData = from a in ctx.Business_Settlement_ProcurementStatement
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

            var varData = from a in ctx.Business_Settlement_ProcurementStatement
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
        /// 获得单条业务总单信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务总单信息</returns>
        public Business_Settlement_ProcurementStatement GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Settlement_ProcurementStatement
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
        /// 保存信息
        /// </summary>
        /// <param name="billInfo">单据信息</param>
        /// <param name="invoiceInfo">发票信息列表</param>
        /// <param name="detailInfo">明细信息列表</param>
        public void SaveInfo(Business_Settlement_ProcurementStatement billInfo, 
            List<Business_Settlement_ProcurementStatement_Invoice> invoiceInfo, 
            List<View_Business_Settlement_ProcurementStatementDetail> detailInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.Business_Settlement_ProcurementStatement
                              where a.BillNo == billInfo.BillNo
                              select a;

                if (varData.Count() == 1)
                {
                    Business_Settlement_ProcurementStatement lnqBill = varData.Single();

                    lnqBill.InvoiceType = billInfo.InvoiceType;
                    lnqBill.BillType = billInfo.BillType;
                    lnqBill.SettlementCompany = billInfo.SettlementCompany;
                    lnqBill.TaxRate = billInfo.TaxRate;
                    lnqBill.AccoutingDocumentNo = billInfo.AccoutingDocumentNo;
                }
                else if (varData.Count() == 0)
                {
                    ctx.Business_Settlement_ProcurementStatement.InsertOnSubmit(billInfo);
                }
                else
                {
                    throw new Exception("单据数据不唯一");
                }

                var varInvoice = from a in ctx.Business_Settlement_ProcurementStatement_Invoice
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_Settlement_ProcurementStatement_Invoice.DeleteAllOnSubmit(varInvoice);
                ctx.SubmitChanges();

                if (invoiceInfo != null && invoiceInfo.Count > 0)
                {
                    foreach (Business_Settlement_ProcurementStatement_Invoice item in invoiceInfo)
                    {
                        Business_Settlement_ProcurementStatement_Invoice lnqTemp = new Business_Settlement_ProcurementStatement_Invoice();

                        lnqTemp.BillNo = billInfo.BillNo;
                        lnqTemp.InvoiceDate = item.InvoiceDate;
                        lnqTemp.InvoiceNo = item.InvoiceNo;

                        ctx.Business_Settlement_ProcurementStatement_Invoice.InsertOnSubmit(lnqTemp);
                    }
                }

                var varDetail = from a in ctx.Business_Settlement_ProcurementStatementDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                ctx.Business_Settlement_ProcurementStatementDetail.DeleteAllOnSubmit(varDetail);
                ctx.SubmitChanges();

                foreach (View_Business_Settlement_ProcurementStatementDetail item in detailInfo)
                {
                    Business_Settlement_ProcurementStatementDetail lnqTemp = new Business_Settlement_ProcurementStatementDetail();

                    lnqTemp.BillNo = billInfo.BillNo;
                    lnqTemp.ConsignedProcessingMaterials = item.委托加工材料;
                    lnqTemp.ContractRequestNotes = item.合同申请单号;
                    lnqTemp.DifferencesMean = item.差异说明;
                    lnqTemp.GoodsCount = item.入库数量;
                    lnqTemp.GoodsID = item.物品ID;
                    lnqTemp.BatchNo = item.批次号;
                    lnqTemp.InPutBillNo = item.入库单号;
                    lnqTemp.InPutPrice = item.入库金额;
                    lnqTemp.InPutUnitPrice = item.入库单价;
                    lnqTemp.InvocieUnitPrice = item.发票单价;
                    lnqTemp.InvoicePrice = item.发票金额;
                    lnqTemp.SingleCommissionMaterials = item.单件委托材料;
                    lnqTemp.SingleProcessingCost = item.单件加工费;
                    lnqTemp.TaxAmount = item.税额;
                    lnqTemp.TotalTaxPrice = item.价税合计;

                    ctx.Business_Settlement_ProcurementStatementDetail.InsertOnSubmit(lnqTemp);
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
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Business_Settlement_ProcurementStatementDetail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Business_Settlement_ProcurementStatementDetail
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得发票信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<Business_Settlement_ProcurementStatement_Invoice> GetListInvoiceInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Settlement_ProcurementStatement_Invoice
                          where a.BillNo == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 导入单据明细
        /// </summary>
        /// <param name="lstBillNo">单据号列表</param>
        /// <param name="billType">单据类型</param>
        /// <returns>返回Table</returns>
        public DataTable LeadInDetail(List<string> lstBillNo, CE_ProcurementStatementBillTypeEnum billType)
        {
            string error = "";
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Settlement_ProcurementStatement_TempTable
                          select a;

            ctx.Business_Settlement_ProcurementStatement_TempTable.DeleteAllOnSubmit(varData);

            foreach (string item in lstBillNo)
            {
                Business_Settlement_ProcurementStatement_TempTable tempLnq = new Business_Settlement_ProcurementStatement_TempTable();
                tempLnq.BillNo = item;
                ctx.Business_Settlement_ProcurementStatement_TempTable.InsertOnSubmit(tempLnq);
            }

            ctx.SubmitChanges();

            DataTable result = new DataTable();
            Hashtable hsTable = new Hashtable();
            hsTable.Add("@Flag", billType == CE_ProcurementStatementBillTypeEnum.零星采购加工 ? true : false);

            result = GlobalObject.DatabaseServer.QueryInfoPro("Business_Settlement_ReferemceDetail", hsTable, out error);
            return result;
        }

        /// <summary>
        /// 导入入库单号信息
        /// </summary>
        /// <param name="provider">供应商</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回Table</returns>
        public DataTable LeadInInPutBillInfo(string provider, DateTime startTime, DateTime endTime)
        {
            string error = "";
            Hashtable hsTable = new Hashtable();

            hsTable.Add("@Provider", provider);
            hsTable.Add("@StartTime", startTime.ToShortDateString() + " 00:00:00");
            hsTable.Add("@EndTime", endTime.ToShortDateString() + " 23:59:59");

            return GlobalObject.DatabaseServer.QueryInfoPro("Business_Settlement_ReferemceDetail_InputBillInfo", hsTable, out error);
        }

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void OperatarUnFlowBusiness(string billNo)
        {
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            string billStatus = serviceFlow.GetNextBillStatus(billNo);

            if (billStatus == null)
            {
                throw new Exception("单据状态为空，请重新确认");
            }

            if (billStatus != CE_CommonBillStatus.单据完成.ToString())
            {
                return;
            }

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                List<View_Business_Settlement_ProcurementStatementDetail> lstDetail = GetListViewDetailInfo(billNo);

                foreach (View_Business_Settlement_ProcurementStatementDetail detailInfo in lstDetail)
                {
                    var varBillType = from a in dataContxt.BASE_BillType
                                      where a.TypeCode == GlobalObject.GeneralFunction.ScreenString(CE_ScreenType.字母, detailInfo.入库单号)
                                      select a;
                    if (varBillType.Count() != 1)
                    {
                        throw new Exception("找不到此单据的单据类型或者此单据类型重复");
                    }

                    switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_BillTypeEnum>(varBillType.Single().TypeName))
                    {
                        case CE_BillTypeEnum.报检入库单:
                            ChangePrice_S_CheckOutInDepotBill(dataContxt, detailInfo);
                            break;
                        case CE_BillTypeEnum.委外报检入库单:
                            ChangePrice_S_CheckOutInDepotForOutsourcingBill(dataContxt, detailInfo);
                            break;
                        case CE_BillTypeEnum.普通入库单:
                            ChangePrice_S_OrdinaryInDepotGoodsBill(dataContxt, detailInfo);
                            break;
                        case CE_BillTypeEnum.采购退货单:
                            ChangePrice_S_MaterialRejectBill(dataContxt, detailInfo);
                            break;
                        default:
                            throw new Exception("此单据类型为非入库业务"+ detailInfo.入库单号);
                    }

                    ChangePrice_S_InDepotDetailBill(dataContxt, detailInfo);
                }


                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 变更金额_入库明细
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="detailInfo">结算单明细</param>
        void ChangePrice_S_InDepotDetailBill(DepotManagementDataContext dataContxt,
            View_Business_Settlement_ProcurementStatementDetail detailInfo)
        {

            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            Business_Settlement_ProcurementStatement statement = GetSingleBillInfo(detailInfo.单据号);

            //获得当前日期的月结起始日期与结束日期
            ServerTime.GetMonthlyBalance(ServerTime.Time, out dtStart, out dtEnd);

            var varInDepotBill = from b in dataContxt.S_InDepotDetailBill
                                 where b.GoodsID == detailInfo.物品ID
                                 && b.InDepotBillID == detailInfo.入库单号
                                 && b.BatchNo == detailInfo.批次号
                                 select b;

            if (varInDepotBill.Count() == 1)
            {
                S_InDepotDetailBill lnqInDepotBill = varInDepotBill.Single();

                var varData1 = from a in dataContxt.BASE_Storage
                               where a.StorageID == lnqInDepotBill.StorageID
                               select a;

                if (varData1.Count() == 1)
                {
                    BASE_Storage storageInfo = varData1.Single();

                    if (!storageInfo.FinancialAccountingFlag)
                    {
                        throw new Exception("单据号【"+ lnqInDepotBill.InDepotBillID +"】的入库库房不在财务核算范围之内");
                    }
                }
                else
                {
                    throw new Exception("单据号【" + lnqInDepotBill.InDepotBillID + "】的入库库房不存在或者重复");
                }

                if (statement.BillType == CE_ProcurementStatementBillTypeEnum.委托加工物资.ToString())
                {
                    lnqInDepotBill.InvoiceUnitPrice = detailInfo.发票单价 + (decimal)detailInfo.单件委托材料;
                    lnqInDepotBill.InvoicePrice = Math.Round((decimal)lnqInDepotBill.InvoiceUnitPrice * (decimal)lnqInDepotBill.InDepotCount, 2);
                }
                else
                {
                    lnqInDepotBill.InvoiceUnitPrice = detailInfo.发票单价;
                    lnqInDepotBill.InvoicePrice = detailInfo.发票金额;
                }

                if (lnqInDepotBill.FactPrice != detailInfo.发票金额)
                {
                    //当查询的记录不在当月的结算日期范围内，插入红冲单据与对冲单据
                    if (lnqInDepotBill.BillTime < dtStart || lnqInDepotBill.BillTime > dtEnd)
                    {
                        var varDetail = from d in dataContxt.S_InDepotDetailBill
                                        where d.GoodsID == detailInfo.物品ID
                                        && d.InDepotBillID.Contains(detailInfo.入库单号)
                                        && d.BatchNo == detailInfo.批次号
                                        && d.BillTime >= dtStart && d.BillTime <= dtEnd
                                        select d;

                        //判断是否已经在当前结算日期范围内插入了红冲与对冲数据
                        if (varDetail.Count() != 0)
                        {
                            foreach (var item in varDetail)
                            {
                                //针对已经插入的对冲数据进行修改
                                if (item.InDepotBillID.Contains("(对冲单据)"))
                                {
                                    if (statement.BillType == CE_ProcurementStatementBillTypeEnum.委托加工物资.ToString())
                                    {
                                        item.FactUnitPrice = detailInfo.发票单价 + (decimal)detailInfo.单件委托材料;
                                        item.FactPrice = Math.Round((decimal)item.FactUnitPrice * (decimal)item.InDepotCount, 2);
                                    }
                                    else
                                    {
                                        item.FactUnitPrice = detailInfo.发票单价;
                                        item.FactPrice = detailInfo.发票金额;
                                    }
                                }
                            }
                        }//对没有插入的红冲与对冲的记录进行插入
                        else
                        {
                            //插一条原始的负记录（红冲单据）
                            S_InDepotDetailBill lnqOldInDepotBill = new S_InDepotDetailBill();

                            lnqOldInDepotBill.ID = Guid.NewGuid();
                            lnqOldInDepotBill.InDepotBillID = lnqInDepotBill.InDepotBillID + "(红冲单据)";
                            lnqOldInDepotBill.BatchNo = lnqInDepotBill.BatchNo;
                            lnqOldInDepotBill.BillTime = ServerTime.Time;
                            lnqOldInDepotBill.Department = lnqInDepotBill.Department;
                            lnqOldInDepotBill.FactUnitPrice = lnqInDepotBill.FactUnitPrice;
                            lnqOldInDepotBill.FactPrice = -lnqInDepotBill.FactPrice;
                            lnqOldInDepotBill.FillInPersonnel = lnqInDepotBill.FillInPersonnel;
                            lnqOldInDepotBill.GoodsID = lnqInDepotBill.GoodsID;
                            lnqOldInDepotBill.InDepotCount = -lnqInDepotBill.InDepotCount;
                            lnqOldInDepotBill.Price = -lnqInDepotBill.Price;
                            lnqOldInDepotBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.财务红冲;
                            lnqOldInDepotBill.Provider = lnqInDepotBill.Provider;
                            lnqOldInDepotBill.Remark = lnqInDepotBill.Remark;
                            lnqOldInDepotBill.StorageID = lnqInDepotBill.StorageID;
                            lnqOldInDepotBill.UnitPrice = lnqInDepotBill.UnitPrice;
                            lnqOldInDepotBill.FillInDate = lnqInDepotBill.FillInDate;
                            lnqOldInDepotBill.AffrimPersonnel = lnqInDepotBill.AffrimPersonnel;

                            IFinancialDetailManagement serverDetail =
                                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();
                            serverDetail.ProcessInDepotDetail(dataContxt, lnqOldInDepotBill, null);

                            //插一条新的正记录（对冲单据）
                            S_InDepotDetailBill lnqNewInDepotBill = new S_InDepotDetailBill();

                            lnqNewInDepotBill.ID = Guid.NewGuid();
                            lnqNewInDepotBill.InDepotBillID = lnqInDepotBill.InDepotBillID + "(对冲单据)";
                            lnqNewInDepotBill.BatchNo = lnqInDepotBill.BatchNo;
                            lnqNewInDepotBill.BillTime = ServerTime.Time;
                            lnqNewInDepotBill.Department = lnqInDepotBill.Department;
                            lnqNewInDepotBill.InDepotCount = lnqInDepotBill.InDepotCount;

                            if (statement.BillType == CE_ProcurementStatementBillTypeEnum.委托加工物资.ToString())
                            {
                                lnqNewInDepotBill.FactUnitPrice = detailInfo.发票单价 + (decimal)detailInfo.单件委托材料;
                                lnqNewInDepotBill.FactPrice = Math.Round((decimal)lnqNewInDepotBill.FactUnitPrice * (decimal)lnqNewInDepotBill.InDepotCount, 2);
                            }
                            else
                            {
                                lnqNewInDepotBill.FactUnitPrice = detailInfo.发票单价;
                                lnqNewInDepotBill.FactPrice = detailInfo.发票金额;
                            }

                            lnqNewInDepotBill.FillInPersonnel = lnqInDepotBill.FillInPersonnel;
                            lnqNewInDepotBill.GoodsID = lnqInDepotBill.GoodsID;
                            lnqNewInDepotBill.Price = lnqInDepotBill.Price;
                            lnqNewInDepotBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.财务对冲;
                            lnqNewInDepotBill.Provider = lnqInDepotBill.Provider;
                            lnqNewInDepotBill.Remark = lnqInDepotBill.Remark;
                            lnqNewInDepotBill.StorageID = lnqInDepotBill.StorageID;
                            lnqNewInDepotBill.UnitPrice = lnqInDepotBill.UnitPrice;
                            lnqNewInDepotBill.FillInDate = lnqInDepotBill.FillInDate;
                            lnqNewInDepotBill.AffrimPersonnel = lnqInDepotBill.AffrimPersonnel;

                            serverDetail.ProcessInDepotDetail(dataContxt, lnqNewInDepotBill, null);
                        }
                    }
                    else
                    {
                        if (statement.BillType == CE_ProcurementStatementBillTypeEnum.委托加工物资.ToString())
                        {
                            lnqInDepotBill.FactUnitPrice = detailInfo.发票单价 + (decimal)detailInfo.单件委托材料;
                            lnqInDepotBill.FactPrice = Math.Round((decimal)lnqInDepotBill.InvoiceUnitPrice * (decimal)lnqInDepotBill.InDepotCount, 2);
                        }
                        else
                        {
                            lnqInDepotBill.FactUnitPrice = detailInfo.发票单价;
                            lnqInDepotBill.FactPrice = detailInfo.发票金额;
                        }
                    }
                }

                dataContxt.SubmitChanges();
            }
        }

        /// <summary>
        /// 变更金额_报检入库单
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="detailInfo">结算单明细</param>
        void ChangePrice_S_CheckOutInDepotBill(DepotManagementDataContext dataContxt, 
            View_Business_Settlement_ProcurementStatementDetail detailInfo)
        {
            var varCheckOutInDepot = from a in dataContxt.S_CheckOutInDepotBill
                                     where a.Bill_ID == detailInfo.入库单号
                                     && a.GoodsID == detailInfo.物品ID
                                     && a.BatchNo == detailInfo.批次号
                                     select a;

            //报检入库单单价修改
            if (varCheckOutInDepot.Count() != 0)
            {
                S_CheckOutInDepotBill lnqCheckOutInDepotBill = varCheckOutInDepot.Single();

                lnqCheckOutInDepotBill.UnitInvoicePrice = detailInfo.发票单价;
                lnqCheckOutInDepotBill.InvoicePrice = detailInfo.发票金额;

                lnqCheckOutInDepotBill.HavingInvoice = true;
                dataContxt.SubmitChanges();
            }
        }

        /// <summary>
        /// 变更金额_委外报检入库单
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="detailInfo">结算单明细</param>
        void ChangePrice_S_CheckOutInDepotForOutsourcingBill(DepotManagementDataContext dataContxt, 
            View_Business_Settlement_ProcurementStatementDetail detailInfo)
        {
            var varOutsourcing = from a in dataContxt.S_CheckOutInDepotForOutsourcingBill
                                 where a.Bill_ID == detailInfo.入库单号
                                 && a.GoodsID == detailInfo.物品ID
                                 && a.BatchNo == detailInfo.批次号
                                 select a;

            //委外报检入库单单价修改
            if (varOutsourcing.Count() != 0)
            {
                S_CheckOutInDepotForOutsourcingBill lnqOutsourcing = varOutsourcing.Single();

                lnqOutsourcing.UnitInvoicePrice = detailInfo.发票单价 + (decimal)detailInfo.单件委托材料;
                lnqOutsourcing.InvoicePrice = lnqOutsourcing.UnitInvoicePrice * detailInfo.入库数量;

                lnqOutsourcing.HavingInvoice = true;
                dataContxt.SubmitChanges();
            }
        }

        /// <summary>
        /// 变更金额_普通入库单
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="detailInfo">结算单明细</param>
        void ChangePrice_S_OrdinaryInDepotGoodsBill(DepotManagementDataContext dataContxt, 
            View_Business_Settlement_ProcurementStatementDetail detailInfo)
        {

            IOrdinaryInDepotBillServer serviceOrdinaryBill = ServerModule.ServerModuleFactory.GetServerModule<IOrdinaryInDepotBillServer>();
            string error = "";
            var varOrdinaryGoods = from a in dataContxt.S_OrdinaryInDepotGoodsBill
                                   where a.Bill_ID == detailInfo.入库单号
                                   && a.GoodsID == detailInfo.物品ID
                                   && a.BatchNo == detailInfo.批次号
                                   select a;

            //普通入库单单价修改
            if (varOrdinaryGoods.Count() != 0)
            {
                S_OrdinaryInDepotGoodsBill lnqOrdinaryGoods = varOrdinaryGoods.Single();

                lnqOrdinaryGoods.InvoiceUnitPrice = detailInfo.发票单价;
                lnqOrdinaryGoods.InvoicePrice = detailInfo.发票金额;

                lnqOrdinaryGoods.HavingInvoice = true;
                dataContxt.SubmitChanges();

                int intFlag = serviceOrdinaryBill.GetHavingInvoice(dataContxt, detailInfo.入库单号, out error);

                if (intFlag == 4)
                {
                    throw new Exception(error);
                }
                else
                {
                    var varOrdinaryBill = from a in dataContxt.S_OrdinaryInDepotBill
                                          where a.Bill_ID == detailInfo.入库单号
                                          select a;

                    if (varOrdinaryBill.Count() != 0)
                    {
                        S_OrdinaryInDepotBill lnqOrdinaryBill = varOrdinaryBill.Single();

                        lnqOrdinaryBill.InvoiceStatus = intFlag;
                        dataContxt.SubmitChanges();
                    }

                }
            }
        }

        /// <summary>
        /// 变更金额_采购退货单
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="detailInfo">结算单明细</param>
        void ChangePrice_S_MaterialRejectBill(DepotManagementDataContext dataContxt, 
            View_Business_Settlement_ProcurementStatementDetail detailInfo)
        {

            IMaterialRejectBill serviceMaterialRejectBill = ServerModule.ServerModuleFactory.GetServerModule<IMaterialRejectBill>();
            string error = "";
            var varRejectList = from a in dataContxt.S_MaterialListRejectBill
                                where a.Bill_ID == detailInfo.入库单号
                                && a.GoodsID == detailInfo.物品ID
                                && a.BatchNo == detailInfo.批次号
                                select a;

            if (varRejectList.Count() != 0)
            {
                S_MaterialListRejectBill lnqMaterialList = varRejectList.Single();

                lnqMaterialList.InvoiceUnitPrice = detailInfo.发票单价;
                lnqMaterialList.InvoicePrice = detailInfo.发票金额;

                lnqMaterialList.HavingInvoice = true;
                dataContxt.SubmitChanges();

                int intFlag = serviceMaterialRejectBill.SetHavingInvoiceReturn(dataContxt, detailInfo.入库单号, out error);

                if (intFlag == 4)
                {
                    throw new Exception(error);
                }
                else
                {
                    var varReject = from a in dataContxt.S_MaterialRejectBill
                                    where a.Bill_ID == detailInfo.入库单号
                                    select a;

                    if (varReject.Count() != 0)
                    {
                        S_MaterialRejectBill lnqMaterialBill = varReject.Single();

                        lnqMaterialBill.InvoiceFlag = intFlag;
                        dataContxt.SubmitChanges();
                    }
                }
            }
        }
    }
}
