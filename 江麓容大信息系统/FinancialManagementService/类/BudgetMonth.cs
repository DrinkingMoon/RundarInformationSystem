using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;
using FlowControlService;
using System.Collections;

namespace Service_Economic_Financial
{
    class BudgetMonth : IBudgetMonth
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.月度预算申请表.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_Finance_Budget_MonthDetail
                              where a.BillNo == billNo
                              select a;

                var varData1 = from a in ctx.Business_Finance_Budget_Month
                               where a.BillNo == billNo
                               select a;

                ctx.Business_Finance_Budget_MonthDetail.DeleteAllOnSubmit(varData);
                ctx.Business_Finance_Budget_Month.DeleteAllOnSubmit(varData1);

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
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Finance_Budget_Month
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
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_Finance_Budget_Month
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

        public void SaveInfo(DataTable detailTable, Business_Finance_Budget_Month billInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            try
            {
                if (serviceFlow.GetNowBillStatus(billInfo.BillNo) == CE_CommonBillStatus.新建单据.ToString())
                {
                    if (detailTable == null)
                    {
                        throw new Exception("无明细信息，无法保存");
                    }

                    var varDetail = from a in ctx.Business_Finance_Budget_MonthDetail
                                    where a.BillNo == billInfo.BillNo
                                    select a;

                    var varBill = from a in ctx.Business_Finance_Budget_Month
                                  where a.BillNo == billInfo.BillNo
                                  select a;

                    ctx.Business_Finance_Budget_MonthDetail.DeleteAllOnSubmit(varDetail);
                    ctx.Business_Finance_Budget_Month.DeleteAllOnSubmit(varBill);

                    billInfo.DeptCode = UniversalFunction.GetDept_Belonge(ctx, BasicInfo.DeptCode).DeptCode;
                    ctx.Business_Finance_Budget_Month.InsertOnSubmit(billInfo);

                    List<Business_Finance_Budget_MonthDetail> lstDetail = new List<Business_Finance_Budget_MonthDetail>();
                    Business_Finance_Budget_MonthDetail detail = new Business_Finance_Budget_MonthDetail();

                    foreach (DataRow dr in detailTable.Rows)
                    {
                        decimal yeardec = Convert.ToDecimal(dr["年度预算"]);
                        decimal monthdec = Convert.ToDecimal(dr["月度预算"]);

                        if (monthdec > 0 && monthdec >= yeardec * (decimal)1.2
                            && GlobalObject.GeneralFunction.IsNullOrEmpty(dr["差异说明(年)"].ToString()))
                        {
                            throw new Exception("请填写【差异说明(年)】");
                        }

                        detail = new Business_Finance_Budget_MonthDetail();

                        detail.BillNo = billInfo.BillNo;
                        detail.BudgetAmount = dr["月度预算"] == null ? 0 : Convert.ToDecimal(dr["月度预算"]);
                        detail.ProjectID = dr["科目ID"].ToString();
                        detail.ActualAmount = 0;
                        detail.DifferenceRemarkYear = dr["差异说明(年)"] == null ? "" : dr["差异说明(年)"].ToString();

                        lstDetail.Add(detail);
                    }

                    ctx.Business_Finance_Budget_MonthDetail.InsertAllOnSubmit(lstDetail);
                }
                else if (serviceFlow.GetNextBillStatus(billInfo.BillNo) == CE_CommonBillStatus.等待确认.ToString())
                {
                    foreach (DataRow dr in detailTable.Rows)
                    {
                        var varDataDetail = from a in ctx.Business_Finance_Budget_MonthDetail
                                            where a.BillNo == billInfo.BillNo
                                            && a.ProjectID == dr["科目ID"].ToString()
                                            select a;

                        if (varDataDetail.Count() == 1)
                        {
                            varDataDetail.Single().ActualAmount = dr["实际金额"] == null ? 0 : Convert.ToDecimal(dr["实际金额"]);
                        }
                    }
                }
                else if (serviceFlow.GetNextBillStatus(billInfo.BillNo) == CE_CommonBillStatus.单据完成.ToString())
                {
                    foreach (DataRow dr in detailTable.Rows)
                    {
                        decimal monthdec = Convert.ToDecimal(dr["月度预算"]);
                        decimal actarldec = Convert.ToDecimal(dr["实际金额"]);

                        if (actarldec > 0 && actarldec >= monthdec * (decimal)1.2
                            && GlobalObject.GeneralFunction.IsNullOrEmpty(dr["差异说明(月)"].ToString()))
                        {
                            throw new Exception("请填写【差异说明(月)】");
                        }

                        var varDataDetail = from a in ctx.Business_Finance_Budget_MonthDetail
                                            where a.BillNo == billInfo.BillNo
                                            && a.ProjectID == dr["科目ID"].ToString()
                                            select a;

                        if (varDataDetail.Count() == 1)
                        {
                            varDataDetail.Single().DifferenceRemarkMonth = dr["差异说明(月)"] == null ? "" : dr["差异说明(月)"].ToString();
                        }
                    }
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void OperationBusiness(DataTable detailTable, string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            try
            {
                if (serviceFlow.GetNextBillStatus(billNo) != CE_CommonBillStatus.单据完成.ToString())
                {
                    return;
                }

                Business_Finance_Budget_Month billInfo = GetBillSingleInfo(ctx, billNo);

                if (billInfo == null)
                {
                    throw new Exception("单据不存在");
                }

                var varDetail = from a in ctx.Business_Finance_Budget_MonthDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                foreach (Business_Finance_Budget_MonthDetail detail in varDetail)
                {
                    var varInfo = from a in ctx.Business_Finance_BudgetInfo
                                  where a.DeptCode == billInfo.DeptCode
                                  && a.YearValue == billInfo.YearValue
                                  && a.MonthValue == billInfo.MonthValue
                                  && a.ProjectID == detail.ProjectID
                                  select a;

                    if (varInfo.Count() == 1)
                    {
                        varInfo.Single().BudgetAmountMonth = detail.BudgetAmount;
                        varInfo.Single().ActualAmount = detail.ActualAmount;
                    }
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        Business_Finance_Budget_Month GetBillSingleInfo(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.Business_Finance_Budget_Month
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

        public Business_Finance_Budget_Month GetBillSingleInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetBillSingleInfo(ctx, billNo);
        }

        public DataTable GetDetailInfo(Business_Finance_Budget_Month billInfo)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@BillNo", billInfo.BillNo);
            hsTable.Add("@Year", billInfo.YearValue);
            hsTable.Add("@Month", billInfo.MonthValue);
            hsTable.Add("@DeptCode", billInfo.DeptCode);

            return GlobalObject.DatabaseServer.QueryInfoPro("Business_Finance_Get_BudgetMonthDetail", hsTable, out error);
        }
    }
}
