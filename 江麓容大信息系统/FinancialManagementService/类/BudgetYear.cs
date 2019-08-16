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
    class BudgetYear : IBudgetYear
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
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.年度预算申请表.ToString(), this);

            try
            {
                var varData = from a in ctx.Business_Finance_Budget_YearDetail
                              where a.BillNo == billNo
                              select a;

                var varData1 = from a in ctx.Business_Finance_Budget_Year
                               where a.BillNo == billNo
                               select a;

                ctx.Business_Finance_Budget_YearDetail.DeleteAllOnSubmit(varData);
                ctx.Business_Finance_Budget_Year.DeleteAllOnSubmit(varData1);

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

            var varData = from a in ctx.Business_Finance_Budget_Year
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
            var varData = from a in ctx.Business_Finance_Budget_Year
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

        public void SaveInfo(DataTable detailTable, Business_Finance_Budget_Year billInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            try
            {
                if (serviceFlow.GetNowBillStatus(billInfo.BillNo) != CE_CommonBillStatus.新建单据.ToString())
                {
                    return;
                }

                if (detailTable == null)
                {
                    throw new Exception("无明细信息，无法保存");
                }

                var varDetail = from a in ctx.Business_Finance_Budget_YearDetail
                                where a.BillNo == billInfo.BillNo
                                select a;

                var varBill = from a in ctx.Business_Finance_Budget_Year
                              where a.BillNo == billInfo.BillNo
                              select a;

                ctx.Business_Finance_Budget_YearDetail.DeleteAllOnSubmit(varDetail);
                ctx.Business_Finance_Budget_Year.DeleteAllOnSubmit(varBill);

                billInfo.DeptCode = UniversalFunction.GetDept_Belonge(ctx, BasicInfo.DeptCode).DeptCode;
                ctx.Business_Finance_Budget_Year.InsertOnSubmit(billInfo);

                List<Business_Finance_Budget_YearDetail> lstDetail = new List<Business_Finance_Budget_YearDetail>();
                Business_Finance_Budget_YearDetail detail = new Business_Finance_Budget_YearDetail();

                foreach (DataRow dr in detailTable.Rows)
                {
                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 1;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["1月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 2;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["2月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 3;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["3月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 4;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["4月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 5;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["5月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 6;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["6月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 7;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["7月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 8;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["8月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 9;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["9月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 10;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["10月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 11;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["11月"]);
                    lstDetail.Add(detail);

                    detail = new Business_Finance_Budget_YearDetail();
                    detail.BillNo = billInfo.BillNo;
                    detail.MonthValue = 12;
                    detail.ProjectID = dr["科目ID"].ToString();
                    detail.BudgetAmount = Convert.ToDecimal(dr["12月"]);
                    lstDetail.Add(detail);
                }

                ctx.Business_Finance_Budget_YearDetail.InsertAllOnSubmit(lstDetail);
                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void OperationBusiness(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            try
            {
                if (serviceFlow.GetNextBillStatus(billNo) != CE_CommonBillStatus.单据完成.ToString())
                {
                    return;
                }

                var varData = from a in ctx.Business_Finance_Budget_Year
                              where a.BillNo == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("未找到主表信息");
                }

                List<CommonProcessInfo> lstPersonnel = serviceFlow.GetFlowData(billNo);
                string createUser = lstPersonnel[lstPersonnel.Count() - 1].人员;

                Business_Finance_Budget_Year yearInfo = varData.Single();

                var varDelete = from a in ctx.Business_Finance_BudgetInfo
                                where a.YearValue == yearInfo.YearValue
                                && a.DeptCode == yearInfo.DeptCode
                                select a;

                ctx.Business_Finance_BudgetInfo.DeleteAllOnSubmit(varDelete);

                var varDetail = from a in ctx.Business_Finance_Budget_YearDetail
                                where a.BillNo == billNo
                                select a;

                foreach (Business_Finance_Budget_YearDetail detail in varDetail)
                {
                    Business_Finance_BudgetInfo budgetInfo = new Business_Finance_BudgetInfo();

                    budgetInfo.ID = Guid.NewGuid();
                    budgetInfo.BudgetAmountYear = detail.BudgetAmount;
                    budgetInfo.CreateDate = ServerTime.Time;
                    budgetInfo.CreateUser = createUser;
                    budgetInfo.DeptCode = yearInfo.DeptCode;
                    budgetInfo.ProjectID = detail.ProjectID;
                    budgetInfo.MonthValue = detail.MonthValue;
                    budgetInfo.YearValue = yearInfo.YearValue;

                    ctx.Business_Finance_BudgetInfo.InsertOnSubmit(budgetInfo);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public Business_Finance_Budget_Year GetBillSingleInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Finance_Budget_Year
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

        public DataTable GetDetailInfo(string billNo)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();
            hsTable.Add("@BillNo", billNo);

            return GlobalObject.DatabaseServer.QueryInfoPro("Business_Finance_Get_BudgetYearDetail", hsTable, out error);
        }

        public DataTable GetSynthesizeBudgetInfo(int yearInt, string deptCode)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@Year", yearInt);
            hsTable.Add("@DeptCode", deptCode);

            return GlobalObject.DatabaseServer.QueryInfoPro("Business_Finance_Get_BudgetInfo", hsTable, out error);
        }
    }
}
