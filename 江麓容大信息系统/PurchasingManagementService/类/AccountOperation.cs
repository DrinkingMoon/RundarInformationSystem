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
    public enum SelectType
    {
        待审核,
        待复核,
        可打印,
        已打印,
        全部
    }

    class AccountOperation : IAccountOperation
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.供应商应付账款.ToString(), this);

            try
            {
                var varData = from a in ctx.Bus_PurchasingMG_AccountBill
                              where a.BillNo == billNo
                              select a;

                ctx.Bus_PurchasingMG_AccountBill.DeleteAllOnSubmit(varData);

                var varData1 = from a in ctx.Bus_PurchasingMG_AccountBill_Detail
                               where a.BillNo == billNo
                               select a;

                ctx.Bus_PurchasingMG_AccountBill_Detail.DeleteAllOnSubmit(varData1);

                var varData2 = from a in ctx.Bus_PurchasingMG_AccountBill_Invoice
                               where a.BillNo == billNo
                               select a;

                ctx.Bus_PurchasingMG_AccountBill_Invoice.DeleteAllOnSubmit(varData2);


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
            var varData = from a in ctx.Bus_PurchasingMG_AccountBill
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

            var varData = from a in ctx.Bus_PurchasingMG_AccountBill
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
        public Bus_PurchasingMG_AccountBill GetSingleBillInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_PurchasingMG_AccountBill
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
        public void SaveInfo(Bus_PurchasingMG_AccountBill billInfo, List<Bus_PurchasingMG_AccountBill_Invoice> invoiceInfo,
            List<View_Bus_PurchasingMG_AccountBill_Detail> detailInfo)
        {

            try
            {
                using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                {
                    ctx.Connection.Open();
                    using (ctx.Transaction = ctx.Connection.BeginTransaction())
                    {

                        var varData = from a in ctx.Bus_PurchasingMG_AccountBill
                                      where a.BillNo == billInfo.BillNo
                                      select a;

                        if (varData.Count() == 1)
                        {
                            Bus_PurchasingMG_AccountBill lnqBill = varData.Single();

                            lnqBill.Provider = billInfo.Provider;
                            lnqBill.Remark = billInfo.Remark;
                            lnqBill.VoucherNo = billInfo.VoucherNo;
                            lnqBill.FinanceTime = billInfo.FinanceTime;
                        }
                        else if (varData.Count() == 0)
                        {
                            ctx.Bus_PurchasingMG_AccountBill.InsertOnSubmit(billInfo);
                        }
                        else
                        {
                            throw new Exception("单据数据不唯一");
                        }

                        var varInvoice = from a in ctx.Bus_PurchasingMG_AccountBill_Invoice
                                         where a.BillNo == billInfo.BillNo
                                         select a;

                        ctx.Bus_PurchasingMG_AccountBill_Invoice.DeleteAllOnSubmit(varInvoice);
                        ctx.SubmitChanges();

                        if (invoiceInfo != null && invoiceInfo.Count > 0)
                        {
                            foreach (Bus_PurchasingMG_AccountBill_Invoice item in invoiceInfo)
                            {
                                Bus_PurchasingMG_AccountBill_Invoice lnqTemp = new Bus_PurchasingMG_AccountBill_Invoice();

                                lnqTemp.F_Id = Guid.NewGuid().ToString();
                                lnqTemp.BillNo = billInfo.BillNo;
                                lnqTemp.InvoiceTime = item.InvoiceTime;
                                lnqTemp.InvoiceNo = item.InvoiceNo;

                                ctx.Bus_PurchasingMG_AccountBill_Invoice.InsertOnSubmit(lnqTemp);
                            }
                        }

                        var varDetail = from a in ctx.Bus_PurchasingMG_AccountBill_Detail
                                        where a.BillNo == billInfo.BillNo
                                        select a;

                        ctx.Bus_PurchasingMG_AccountBill_Detail.DeleteAllOnSubmit(varDetail);
                        ctx.SubmitChanges();

                        foreach (View_Bus_PurchasingMG_AccountBill_Detail item in detailInfo)
                        {
                            Bus_PurchasingMG_AccountBill_Detail lnqTemp = new Bus_PurchasingMG_AccountBill_Detail();

                            lnqTemp.F_Id = Guid.NewGuid().ToString();
                            lnqTemp.BillNo = billInfo.BillNo;
                            lnqTemp.AccountCount = item.实付数量;
                            lnqTemp.AccountPrice = item.发票金额;
                            lnqTemp.GoodsID = item.GoodsID;
                            lnqTemp.Remark = item.备注;
                            lnqTemp.YearMonth = item.挂账年月;
                            lnqTemp.YGCount = item.应付数量;
                            lnqTemp.YGUnitPrice = item.协议单价;
                            lnqTemp.TaxRate = item.税率;

                            ctx.Bus_PurchasingMG_AccountBill_Detail.InsertOnSubmit(lnqTemp);
                        }

                        ctx.SubmitChanges();
                        ctx.Transaction.Commit();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 获得业务明细信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<View_Bus_PurchasingMG_AccountBill_Detail> GetListViewDetailInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_Bus_PurchasingMG_AccountBill_Detail
                          where a.单据号 == billNo
                          orderby a.挂账年月, a.图号型号
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获得发票信息
        /// </summary>
        /// <param name="billNo">业务号</param>
        /// <returns>返回业务明细列表</returns>
        public List<Bus_PurchasingMG_AccountBill_Invoice> GetListInvoiceInfo(string billNo)
        {
            if (billNo == null)
            {
                return null;
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Bus_PurchasingMG_AccountBill_Invoice
                          where a.BillNo == billNo
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 操作流程以外的业务
        /// </summary>
        /// <param name="billNo">单据号</param>
        public void OperatarUnFlowBusiness(string billNo)
        {
            IFinancialDetailManagement serverDetail = ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();
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
                List<View_Bus_PurchasingMG_AccountBill_Detail> lstDetail = GetListViewDetailInfo(billNo);
                Bus_PurchasingMG_AccountBill billInfo = GetSingleBillInfo(billNo);

                foreach (View_Bus_PurchasingMG_AccountBill_Detail detailInfo in lstDetail)
                {
                    //插一条原始的负记录（红冲单据）
                    S_InDepotDetailBill lnqRedInDepotBill = new S_InDepotDetailBill();

                    lnqRedInDepotBill.ID = Guid.NewGuid();
                    lnqRedInDepotBill.InDepotBillID = detailInfo.单据号 + "(红冲单据)";
                    lnqRedInDepotBill.BatchNo = detailInfo.挂账年月;
                    lnqRedInDepotBill.BillTime = (DateTime)billInfo.FinanceTime;
                    lnqRedInDepotBill.Department = "";
                    lnqRedInDepotBill.FactUnitPrice = Math.Round((decimal)detailInfo.协议单价 / ( 1 + (decimal)detailInfo.税率 / 100), 6);
                    lnqRedInDepotBill.FactPrice = - Math.Round(lnqRedInDepotBill.FactUnitPrice * (decimal)detailInfo.实付数量, 2);
                    lnqRedInDepotBill.InDepotCount = -detailInfo.实付数量;
                    lnqRedInDepotBill.FillInPersonnel = BasicInfo.LoginID;
                    lnqRedInDepotBill.GoodsID = (int)detailInfo.GoodsID;
                    lnqRedInDepotBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.财务红冲;
                    lnqRedInDepotBill.Provider = billInfo.Provider;
                    lnqRedInDepotBill.Remark = billInfo.Remark + "  " + detailInfo.备注;
                    lnqRedInDepotBill.StorageID = "00";
                    lnqRedInDepotBill.UnitPrice = lnqRedInDepotBill.FactUnitPrice;
                    lnqRedInDepotBill.Price = lnqRedInDepotBill.FactPrice;
                    lnqRedInDepotBill.FillInDate = ServerTime.Time;
                    lnqRedInDepotBill.AffrimPersonnel = BasicInfo.LoginID;

                    serverDetail.ProcessInDepotDetail(dataContxt, lnqRedInDepotBill, null);

                    //插一条新的正记录（对冲单据）
                    S_InDepotDetailBill lnqHedgeInDepotBill = new S_InDepotDetailBill();

                    lnqHedgeInDepotBill.ID = Guid.NewGuid();
                    lnqHedgeInDepotBill.InDepotBillID = detailInfo.单据号 + "(对冲单据)";
                    lnqHedgeInDepotBill.BatchNo = detailInfo.挂账年月;
                    lnqHedgeInDepotBill.BillTime = (DateTime)billInfo.FinanceTime;
                    lnqHedgeInDepotBill.Department = "";
                    lnqHedgeInDepotBill.FactUnitPrice = (decimal)detailInfo.发票金额 / (decimal)detailInfo.实付数量;
                    lnqHedgeInDepotBill.FactPrice = (decimal)detailInfo.发票金额;
                    lnqHedgeInDepotBill.InDepotCount = detailInfo.实付数量;
                    lnqHedgeInDepotBill.FillInPersonnel = BasicInfo.LoginID;
                    lnqHedgeInDepotBill.GoodsID = (int)detailInfo.GoodsID;
                    lnqHedgeInDepotBill.OperationType = (int)GlobalObject.CE_SubsidiaryOperationType.财务对冲;
                    lnqHedgeInDepotBill.Provider = billInfo.Provider;
                    lnqHedgeInDepotBill.Remark = billInfo.Remark + "  " + detailInfo.备注;
                    lnqHedgeInDepotBill.StorageID = "00";
                    lnqHedgeInDepotBill.UnitPrice = lnqHedgeInDepotBill.FactUnitPrice;
                    lnqHedgeInDepotBill.Price = lnqHedgeInDepotBill.FactPrice;
                    lnqHedgeInDepotBill.FillInDate = ServerTime.Time;
                    lnqHedgeInDepotBill.AffrimPersonnel = BasicInfo.LoginID;

                    serverDetail.ProcessInDepotDetail(dataContxt, lnqHedgeInDepotBill, null);

                    var varAccountInfo = from a in dataContxt.Bus_PurchasingMG_Account
                                         where a.Provider == billInfo.Provider
                                         && a.YearMonth == detailInfo.挂账年月
                                         && a.GoodsID == Convert.ToInt32(detailInfo.GoodsID)
                                         select a;

                    if (varAccountInfo.Count() != 1)
                    {
                        throw new Exception("找不到对应的【挂账单】");
                    }

                    varAccountInfo.Single().InvoiceCount += (decimal)detailInfo.实付数量;
                    varAccountInfo.Single().InvoicePrice += (decimal)detailInfo.发票金额;

                    dataContxt.SubmitChanges();
                }

                dataContxt.Transaction.Commit();
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        public DataTable GetTable(string yearMonth ,SelectType selectType)
        {
            string strSql = " select a.挂账年月, a.供应商简码, b.ProviderName as 供应商名称, a.状态 as 单据状态 " +
                            " from (select a.YearMonth as 挂账年月, a.Provider as 供应商简码,  " +
                            " case when a.AuditTime is null then '待审核' "+
                            " when a.RecheckTime is null then '待复核' " +
                            " when a.LastPrintTime is not null then '已打印' " +
                            " else '可打印' end as '状态' from Bus_PurchasingMG_Account as a  "+
                            " where GoodsID = (select MIN(GoodsID) from Bus_PurchasingMG_Account as b  "+
                            " where a.Provider = b.Provider and a.YearMonth = b.YearMonth and b.YGCount <> 0)) as a  " +
                            " inner join Provider as b on a.供应商简码 = b.ProviderCode where a.挂账年月 = '" + yearMonth + "'";

            if (selectType != SelectType.全部)
            {
                strSql += " and a.状态 = '" + selectType.ToString() + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void SubmitList(SelectType selectType, string provider, string yearMonth)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                var varData = from a in ctx.Bus_PurchasingMG_Account
                              where a.YearMonth == yearMonth
                              && a.Provider == provider
                              select a;

                foreach (Bus_PurchasingMG_Account item in varData)
                {
                    switch (selectType)
                    {
                        case SelectType.待审核:
                            item.AuditUserID = BasicInfo.LoginID;
                            item.AuditTime = ServerTime.Time;
                            break;
                        case SelectType.待复核:

                            if (BasicInfo.LoginID == item.AuditUserID)
                            {
                                throw new Exception("【审核人】与【复核人】不能为同一个人");
                            }

                            item.RecheckUserID = BasicInfo.LoginID;
                            item.RecheckTime = ServerTime.Time;
                            break;
                        case SelectType.可打印:
                        case SelectType.已打印:
                            item.LastPrintTime = ServerTime.Time;
                            item.LastPrintUserID = BasicInfo.LoginID;
                            break;
                        default:
                            break;
                    }
                }

                ctx.SubmitChanges();
            }
        }

        public DataTable GetTable_Detail(string provider, string yearMonth)
        {
            string strSql = "select * from View_Bus_PurchasingMG_Account where 挂账年月 = '" + yearMonth + "' and 供应商 = '" + provider + "' "+
                " and (上月未挂 <> 0 or 本月入库 <> 0 or 本月应挂 <> 0 or 本月未挂 <> 0)";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable LeadInputAccount(string provider)
        {
            string strSql = " select 图号型号, 物品名称, 规格, 挂账年月, 协议单价, 税率, 本月应挂 - 到票数量 - 已勾稽数量 as 应挂数量, 物品ID "+
                            " from (select 图号型号, 物品名称, 规格, 挂账年月, 协议单价, 税率, 本月应挂, 到票数量,  "+
                            " case when b.AccountCount is null then 0 else b.AccountCount end as 已勾稽数量, a.GoodsID as 物品ID "+
                            " from View_Bus_PurchasingMG_Account as a left join "+
                            " (select SUM(b.AccountCount) as AccountCount, a.Provider, b.YearMonth, b.GoodsID from Bus_PurchasingMG_AccountBill as a  "+
                            " inner join Bus_PurchasingMG_AccountBill_Detail as b on a.BillNo = b.BillNo "+
                            " inner join Flow_FlowBillData as c on a.BillNo = c.BillNo where c.FlowID = 104 "+
                            " group by a.Provider, b.YearMonth, b.GoodsID) as b  "+
                            " on a.GoodsID = b.GoodsID and a.挂账年月 = b.YearMonth and a.供应商 = b.Provider "+
                            " where 供应商 = '" + provider + "' and a.复审时间 is not null) as a "+
                            " where 本月应挂 - 到票数量 - 已勾稽数量 <> 0 order by 挂账年月, 图号型号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void SetFinanceTime(string billNo, DateTime financeTime)
        {
            try
            {
                using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                {
                    var varData = from a in ctx.Bus_PurchasingMG_AccountBill
                                  where a.BillNo == billNo
                                  select a;

                    if (varData.Count() == 1)
                    {
                        varData.Single().FinanceTime = financeTime;
                    }

                    var varData1 = from a in ctx.S_InDepotDetailBill
                                   where a.InDepotBillID.Contains(billNo)
                                   select a;

                    foreach (S_InDepotDetailBill item in varData1)
                    {
                        item.BillTime = financeTime;
                    }

                    ctx.SubmitChanges();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void DeleteInfo_Force(string billNo)
        {
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.供应商应付账款.ToString(), this);

            try
            {
                using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                {
                    var varData = from a in ctx.Bus_PurchasingMG_AccountBill
                                  where a.BillNo == billNo
                                  select a;

                    var varData1 = from a in ctx.Bus_PurchasingMG_AccountBill_Detail
                                   where a.BillNo == billNo
                                   select a;

                    foreach (Bus_PurchasingMG_AccountBill_Detail item in varData1)
                    {
                        var varDate4 = from a in ctx.Bus_PurchasingMG_Account
                                       where a.YearMonth == item.YearMonth
                                       && a.Provider == varData.First().Provider
                                       && a.GoodsID == item.GoodsID
                                       select a;

                        if (varDate4.Count() > 0)
                        {
                            varDate4.First().InvoiceCount -= item.AccountCount;
                            varDate4.First().InvoicePrice -= item.AccountPrice;
                        }
                    }

                    ctx.Bus_PurchasingMG_AccountBill.DeleteAllOnSubmit(varData);
                    ctx.Bus_PurchasingMG_AccountBill_Detail.DeleteAllOnSubmit(varData1);

                    var varData2 = from a in ctx.Bus_PurchasingMG_AccountBill_Invoice
                                   where a.BillNo == billNo
                                   select a;

                    ctx.Bus_PurchasingMG_AccountBill_Invoice.DeleteAllOnSubmit(varData2);

                    var varData3 = from a in ctx.S_InDepotDetailBill
                                   where a.InDepotBillID.Contains(billNo)
                                   select a;

                    ctx.S_InDepotDetailBill.DeleteAllOnSubmit(varData3);

                    ctx.SubmitChanges();
                }

                billNoControl.CancelBill(billNo);

                string strSql = "delete from PlatformService.dbo.Flow_BillFlowMessage where 单据号 = '"+ billNo +"'";
                GlobalObject.DatabaseServer.QueryInfo(strSql);

                serverFlow.FlowForceDelete(billNo);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }
    }
}
