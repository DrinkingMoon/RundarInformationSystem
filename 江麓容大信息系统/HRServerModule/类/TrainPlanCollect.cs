using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;
using FlowControlService;

namespace Service_Peripheral_HR
{
    class TrainPlanCollect : ITrainPlanCollect
    {
        public DataTable GetCourseInfo(int yearValue)
        {
            string strSql = "select 课程名, 计划类型, 月份 as 计划月份, 课程类型, 评估方式, "+
                " 课程ID, ID from View_HR_Train_PlanCollect where 年份 = " + yearValue;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public DataTable GetUserInfo(string guid)
        {
            string strSql = " select distinct 员工姓名, 工号, 岗位, 部门 from View_HR_Train_PlanCollectUser as a "+
                            " inner join HR_Train_PlanCollect as b on a.汇总ID = b.ID "+
                            " left join (select a.CourseID, b.WorkID from HR_Train_Feedback as a  "+
                            " inner join HR_Train_FeedbackUser as b on a.ID = b.FeedbackID) as c  "+
                            " on a.工号 = c.WorkID and b.CourseID = c.CourseID inner join HR_PersonnelArchive as d on a.工号 = d.WorkID " +
                            " where c.CourseID is null and d.PersonnelStatus = 1 and a.汇总ID = '" + guid + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public List<View_HR_Train_PlanCollect> GetCourseInfo(CE_HR_Train_PlanType planType, int yearValue, string planBillNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_PlanCollect
                          where a.计划类型 == planType.ToString()
                          && a.年份 == yearValue
                          select a;

            if (planType == CE_HR_Train_PlanType.临时培训计划)
            {
                varData = from a in varData
                          where a.计划单号 == planBillNo
                          select a;
            }

            return varData.ToList();
        }

        public List<View_HR_Train_PlanCollectUser> GetUserInfo(List<Guid> lstGuid)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_PlanCollectUser
                          where lstGuid.Contains((Guid)a.汇总ID)
                          select a;

            return varData.ToList();
        }

        public List<View_HR_Train_PlanCollectUser> GetUserInfoAll(Guid planCourseID, int courseID)
        {
            ITrainBasicInfo service = ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            List<string> lstUser = service.GetCourse_User(courseID);

            List<View_HR_Train_PlanCollectUser> lstResult = new List<View_HR_Train_PlanCollectUser>();

            foreach (string workID in lstUser)
            {
                var varData = from a in ctx.View_HR_PersonnelArchive
                              where a.员工编号 == workID
                              select a;

                if (varData.Count() == 1)
                {
                    View_HR_Train_PlanCollectUser temp = new View_HR_Train_PlanCollectUser();

                    temp.部门 = varData.Single().部门;
                    temp.岗位 = varData.Single().岗位;
                    temp.工号 = workID;
                    temp.汇总ID = planCourseID;
                    temp.员工姓名 = varData.Single().员工姓名;

                    lstResult.Add(temp);
                }
            }

            return lstResult;
        }

        public void GenerateCollectPlan_Year(List<string> lstBillNo, int yearValue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varCollect = from a in ctx.HR_Train_PlanCollect
                                 where a.YearValue == yearValue && a.PlanType == CE_HR_Train_PlanType.年度培训计划.ToString()
                                 select a;

                ctx.HR_Train_PlanCollect.DeleteAllOnSubmit(varCollect);
                ctx.SubmitChanges();


                var varData_Dept = (from a in ctx.HR_Train_Plan
                                    join b in ctx.HR_Train_PlanCourse on a.BillNo equals b.BillNo
                                    join c in ctx.HR_Train_PlanUser on b.ID equals c.PlanCourseID
                                    join d in ctx.HR_Train_Rel_CommCourse on b.CourseID equals d.CourseID
                                    into lef
                                    from LF in lef.DefaultIfEmpty()
                                    where LF.CommCourseID == null && lstBillNo.Contains(a.BillNo)
                                    select new { CourseID = b.CourseID, WorkID = c.WorkID, MonthValue = b.MonthValue.ToString() }).Distinct();

                var varData_Comm = (from a in ctx.HR_Train_Plan
                                    join b in ctx.HR_Train_PlanCourse on a.BillNo equals b.BillNo
                                    join c in ctx.HR_Train_PlanUser on b.ID equals c.PlanCourseID
                                    join d in ctx.HR_Train_Rel_CommCourse on b.CourseID equals d.CourseID
                                    into lef
                                    from LF in lef.DefaultIfEmpty()
                                    where LF.CommCourseID != null && lstBillNo.Contains(a.BillNo)
                                    select new { CourseID = LF.CommCourseID, WorkID = c.WorkID, MonthValue = "" }).Distinct();

                var varDataTemp = varData_Comm.Union(varData_Dept);

                var varTemp1 = (from a in varDataTemp
                                select new { a.CourseID, a.MonthValue }).Distinct();

                foreach (var item in varTemp1)
                {
                    HR_Train_PlanCollect collect = new HR_Train_PlanCollect();

                    Guid guid = Guid.NewGuid();

                    collect.CourseID = item.CourseID;
                    collect.ID = guid;
                    collect.MonthValue = item.MonthValue == "" ? null : (int?)Convert.ToInt32(item.MonthValue);
                    collect.PlanBillNo = null;
                    collect.PlanType = CE_HR_Train_PlanType.年度培训计划.ToString();
                    collect.YearValue = yearValue;

                    ctx.HR_Train_PlanCollect.InsertOnSubmit(collect);
                    ctx.SubmitChanges();

                    var varTemp2 = (from a in varDataTemp
                                    where a.CourseID == item.CourseID
                                    && a.MonthValue == item.MonthValue
                                    select a.WorkID).Distinct().ToList();

                    foreach (string workID in varTemp2)
                    {
                        HR_Train_PlanCollectUser user = new HR_Train_PlanCollectUser();
                        user.CollectID = guid;
                        user.WorkID = workID;

                        ctx.HR_Train_PlanCollectUser.InsertOnSubmit(user);
                    }

                    ctx.SubmitChanges();
                }

                var varPlan = from a in ctx.HR_Train_Plan
                              where lstBillNo.Contains(a.BillNo)
                              select a;

                foreach (HR_Train_Plan item in varPlan)
                {
                    item.IsCollect = true;
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

        public void GenerateCollectPlan_Temp(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                var varData = from a in ctx.HR_Train_Plan
                              join b in ctx.HR_Train_PlanCourse on a.BillNo equals b.BillNo
                              where a.BillNo == billNo
                              select new { YearValue = a.YearValue, MonthValue = b.MonthValue, 
                                  CourseID = b.CourseID, PlanCourseID = b.ID };

                foreach (var item in varData)
                {
                    HR_Train_PlanCollect collect = new HR_Train_PlanCollect();

                    Guid guid = Guid.NewGuid();

                    collect.CourseID = item.CourseID;
                    collect.ID = guid;
                    collect.MonthValue = (int?)item.MonthValue;
                    collect.PlanBillNo = billNo;
                    collect.PlanType = CE_HR_Train_PlanType.临时培训计划.ToString();
                    collect.YearValue = (int?)item.YearValue;

                    ctx.HR_Train_PlanCollect.InsertOnSubmit(collect);
                    ctx.SubmitChanges();

                    var varWorkID = from a in ctx.HR_Train_PlanUser
                                    where a.PlanCourseID == item.PlanCourseID
                                    select a;

                    foreach (var workID in varWorkID)
                    {
                        HR_Train_PlanCollectUser user = new HR_Train_PlanCollectUser();

                        user.CollectID = guid;
                        user.WorkID = workID.WorkID;

                        ctx.HR_Train_PlanCollectUser.InsertOnSubmit(user);
                    }
                    ctx.SubmitChanges();
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

        public void SaveUser(List<string> lstWork, Guid guid)
        {
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.HR_Train_PlanCollectUser
                              where a.CollectID == guid
                              select a;

                ctx.HR_Train_PlanCollectUser.DeleteAllOnSubmit(varData);

                foreach (string workID in lstWork)
                {
                    HR_Train_PlanCollectUser user = new HR_Train_PlanCollectUser();

                    user.CollectID = guid;
                    user.WorkID = workID;

                    ctx.HR_Train_PlanCollectUser.InsertOnSubmit(user);
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void SaveCollect(List<View_HR_Train_PlanCollect> lstPlan)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {

                foreach (View_HR_Train_PlanCollect plan in lstPlan)
                {
                    HR_Train_PlanCollect collect = new HR_Train_PlanCollect();

                    var varData = from a in ctx.HR_Train_PlanCollect
                                  where a.ID == plan.ID
                                  select a;

                    if (varData.Count() == 1)
                    {
                        varData.Single().MonthValue = plan.月份;
                    }
                    else
                    {
                        throw new Exception("数据异常");
                    }

                    ctx.SubmitChanges();
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
    }
}
