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
    class TrainSurvey : ITrainSurvey
    {
        public HR_Train_Plan GetSingleInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_Plan
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
        /// 删除业务
        /// </summary>
        /// <param name="billNo">业务号</param>
        public void DeleteInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
            BillNumberControl billNoControl = new BillNumberControl(CE_BillTypeEnum.培训计划申请表.ToString(), this);

            try
            {
                var varData = from a in ctx.HR_Train_Plan
                              where a.BillNo == billNo
                              select a;

                var varData1 = from a in ctx.HR_Train_PlanCourse
                               where a.BillNo == billNo
                               select a;

                var varData2 = from a in ctx.HR_Train_PlanUser
                               join b in ctx.HR_Train_PlanCourse
                               on a.PlanCourseID equals b.ID
                               where b.BillNo == billNo
                               select a;

                ctx.HR_Train_PlanUser.DeleteAllOnSubmit(varData2);
                ctx.HR_Train_PlanCourse.DeleteAllOnSubmit(varData1);
                ctx.HR_Train_Plan.DeleteAllOnSubmit(varData);

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

            var varData = from a in ctx.HR_Train_Plan
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
            var varData = from a in ctx.HR_Train_Plan
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

        public void SaveInfo(HR_Train_Plan planInfo, List<View_HR_Train_PlanCourse> lstPlanCourse, 
            List<View_HR_Train_PlanUser> lstUser)
        {
            IFlowServer service = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                if (service.GetNowBillStatus(planInfo.BillNo) != CE_CommonBillStatus.新建单据.ToString())
                {
                    return;
                }

                var varData = from a in ctx.HR_Train_Plan
                              where a.BillNo == planInfo.BillNo
                              select a;

                var varData1 = from a in ctx.HR_Train_PlanCourse
                               where a.BillNo == planInfo.BillNo
                               select a;

                var varData2 = from a in ctx.HR_Train_PlanUser
                               join b in ctx.HR_Train_PlanCourse
                               on a.PlanCourseID equals b.ID
                               where b.BillNo == planInfo.BillNo
                               select a;

                ctx.HR_Train_PlanUser.DeleteAllOnSubmit(varData2);
                ctx.HR_Train_PlanCourse.DeleteAllOnSubmit(varData1);
                ctx.HR_Train_Plan.DeleteAllOnSubmit(varData);

                planInfo.Department = UniversalFunction.GetDept_Belonge(ctx, planInfo.Department).DeptCode;
                ctx.HR_Train_Plan.InsertOnSubmit(planInfo);

                foreach (View_HR_Train_PlanCourse course in lstPlanCourse)
                {
                    HR_Train_PlanCourse tempCourse = new HR_Train_PlanCourse();

                    tempCourse.BillNo = planInfo.BillNo;
                    tempCourse.CourseID = course.课程ID;
                    tempCourse.ID = course.ID;
                    tempCourse.MonthValue = course.月份;

                    ctx.HR_Train_PlanCourse.InsertOnSubmit(tempCourse);
                }

                foreach (View_HR_Train_PlanUser user in lstUser)
                {
                    HR_Train_PlanUser tempUser = new HR_Train_PlanUser();

                    tempUser.PlanCourseID = user.PlanCourseID;
                    tempUser.WorkID = user.员工编号;

                    ctx.HR_Train_PlanUser.InsertOnSubmit(tempUser);
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

        public void OperationBusiness(string billNo)
        {
            try
            {
                ITrainPlanCollect serviceCollect = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainPlanCollect>();
                IFlowServer serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

                if (serviceFlow.GetNextBillStatus(billNo) == CE_CommonBillStatus.单据完成.ToString())
                {
                    serviceCollect.GenerateCollectPlan_Temp(billNo);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public List<View_HR_Train_PlanCourse> GetPlanCourseInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_PlanCourse
                          where a.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        public List<View_HR_Train_PlanUser> GetPlanUserInfo(string billNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_HR_Train_PlanUser
                          join b in ctx.View_HR_Train_PlanCourse 
                          on a.PlanCourseID equals b.ID
                          where b.单据号 == billNo
                          select a;

            return varData.ToList();
        }

        public List<View_HR_Train_PlanUser> GetPlanUserInfoAll(Guid planCourseID, int courseID)
        {
            ITrainBasicInfo service = ServerModuleFactory.GetServerModule<ITrainBasicInfo>();

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            List<string> lstUser = service.GetCourse_User(courseID);

            List<View_HR_Train_PlanUser> lstResult = new List<View_HR_Train_PlanUser>();

            foreach (string workID in lstUser)
            {
                var varData = from a in ctx.View_HR_PersonnelArchive
                              where a.员工编号 == workID
                              select a;

                if (varData.Count() == 1)
                {
                    View_HR_Train_PlanUser temp = new View_HR_Train_PlanUser();

                    temp.PlanCourseID = planCourseID;

                    temp.岗位 = varData.Single().岗位;
                    temp.员工编号 = workID;
                    temp.员工姓名 = varData.Single().员工姓名;

                    lstResult.Add(temp);
                }
            }

            return lstResult;
        }

        public List<string> GetBillNoList_Temp(int yearValue)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_Train_Plan
                          join b in ctx.Flow_FlowBillData
                          on a.BillNo equals b.BillNo
                          join c in ctx.Flow_FlowInfo
                          on b.FlowID equals c.FlowID
                          where a.PlanType == CE_HR_Train_PlanType.临时培训计划.ToString()
                          && a.YearValue == yearValue && c.BusinessStatus == CE_CommonBillStatus.单据完成.ToString()
                          select a;

            return varData.Select(k => k.BillNo).ToList();
        }

        public DataTable GetBillInfo_Year(int yearValue)
        {
            string strSql = " select a.* from View_HR_Train_Plan as a " +
                            " inner join Flow_FlowBillData as b on a.单据号 = b.BillNo " +
                            " inner join Flow_FlowInfo as c on b.FlowID = c.FlowID " +
                            " where a.年份 = " + yearValue.ToString() 
                            + " and c.BusinessStatus = '" + CE_CommonBillStatus.单据完成.ToString() + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
