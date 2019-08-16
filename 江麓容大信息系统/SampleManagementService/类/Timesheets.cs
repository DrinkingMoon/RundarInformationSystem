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

namespace Service_Project_Project
{
    class Timesheets : Service_Project_Project.ITimesheets
    {
        public DataTable GetInfo(string workID)
        {
            string strSql = " select  dbo.fun_get_Name(ExecUser) as 执行人, ItemName as 项目名称, " +
                            " ElapsedTime as 项目工时, ItemDate as 项目工作日期, " +
                            " ItemDescription as 项目描述, dbo.fun_get_Name(RecordPersonnel) as 记录人, RecordDate as 记录时间, ID as 序号 " +
                            " from Business_Project_Timesheets where 1=1";

            if (workID != null && workID.Trim().Length != 0)
            {
                strSql += " and (RecordPersonnel = '" + workID + "' or ExecUser = '"+ workID +"')";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public bool IsOverTime(Business_Project_Timesheets timesheets)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Project_Timesheets
                          where a.ExecUser == timesheets.ExecUser
                          && a.ItemDate == timesheets.ItemDate
                          select a;

            if (varData.Count() > 0 && varData.Sum(k => k.ElapsedTime) + timesheets.ElapsedTime > 24)
            {
                return true;
            }

            return false;
        }

        public bool IsRepeat(Business_Project_Timesheets timesheets)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Project_Timesheets
                          where a.ExecUser == timesheets.ExecUser
                          && a.ItemName == timesheets.ItemName
                          && a.ItemDate == timesheets.ItemDate
                          select a;

            if (varData.Count() > 0)
            {
                return true;
            }

            return false;
        }

        public void OperationInfo(CE_OperatorMode mode ,Business_Project_Timesheets timesheets)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            switch (mode)
            {
                case CE_OperatorMode.添加:

                    timesheets.RecordDate = ServerTime.Time;
                    timesheets.RecordPersonnel = BasicInfo.LoginID;

                    ctx.Business_Project_Timesheets.InsertOnSubmit(timesheets);
                    break;
                case CE_OperatorMode.删除:

                    var varData = from a in ctx.Business_Project_Timesheets
                                  where a.ID == timesheets.ID
                                  select a;

                    ctx.Business_Project_Timesheets.DeleteAllOnSubmit(varData);
                    break;
                default:
                    break;
            }

            ctx.SubmitChanges();
        }

        public DataTable GetSetPersonnelInfo()
        {
            string strSql = " select b.WorkID as 工号, b.Name as 姓名, c.DeptName as 所属科室  "+
                            " from Business_Project_Timesheets_Personnel as a   "+
                            " inner join HR_PersonnelArchive as b on a.WorkID = b.WorkID  "+
                            " inner join HR_Dept as c on b.Dept = c.DeptCode";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void AddPersonnel(string workID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Project_Timesheets_Personnel
                          where a.WorkID == workID
                          select a;

            if (varData.Count() == 0)
            {
                Business_Project_Timesheets_Personnel personnel = new Business_Project_Timesheets_Personnel();

                personnel.WorkID = workID;
                ctx.Business_Project_Timesheets_Personnel.InsertOnSubmit(personnel);
            }

            ctx.SubmitChanges();
        }

        public void DeletePeronnnel(string workID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.Business_Project_Timesheets_Personnel
                          where a.WorkID == workID
                          select a;

            ctx.Business_Project_Timesheets_Personnel.DeleteAllOnSubmit(varData);
            ctx.SubmitChanges();
        }
    }
}
