using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using System.Data;
using PlatformManagement;
using GlobalObject;
using System.Collections;
using DBOperate;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 请假操作类
    /// </summary>
    class LeaveServer : Service_Peripheral_HR.ILeaveServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        #region 请假类别操作
        /// <summary>
        /// 获取请假类别
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAllLeaveType()
        {
            string sql = "select * from View_HR_LeaveType";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过请假类别编号获取请假类别信息
        /// </summary>
        /// <param name="TypeCode">请假类别编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetLeaveTypeByTypeID(string TypeCode)
        {
            string sql = "select * from View_HR_LeaveType where TypeCode='" + TypeCode + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取请假类别(拼接编号和名称)
        /// </summary>
        /// <param name="typeName">请假类别名称</param>
        /// <returns>返回数据集</returns>
        public DataTable GetLeaveTypeByCode_Show(string typeName)
        {
            string sql = "select 请假类别编号+' '+请假类别名称 as 请假类别 from View_HR_LeaveType where 1=1 ";

            if (typeName != null)
            {
                sql += " and 请假类别名称='" + typeName + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取请假类别(拼接编号和名称)
        /// </summary>
        /// <param name="typeName">请假类别名称</param>
        /// <returns>返回数据集</returns>
        public DataTable GetLeaveTypeByCode(string typeName)
        {
            string sql = "select 请假类别编号+' '+请假类别名称 as 请假类别 from View_HR_LeaveType where 禁用 = 0 ";

            if (typeName != null)
            {
                sql += " and 请假类别名称='" + typeName + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过类别编号获得类别名称
        /// </summary>
        /// <param name="typeCode">请假类别编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetLeaveTypeByName(string typeCode)
        {
            string sql = "select 请假类别名称 from View_HR_LeaveType where 请假类别编号='"+typeCode+"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过类别编号获得请假类别信息
        /// </summary>
        /// <param name="typeCode">类别编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetLeaveType(string typeCode)
        {
            string sql = "select * from dbo.View_HR_LeaveType where 请假类别编号='" + typeCode + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过类别编号获得请假类别父级编号
        /// </summary>
        /// <param name="typeCode">类别编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetLeaveTypeParentCode(string typeCode)
        {
            string sql = "select * from dbo.HR_LeaveType where typename='" + typeCode + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 新增请假类别
        /// </summary>
        /// <param name="leaveType">请假类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AddLeaveType(HR_LeaveType leaveType,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_LeaveType
                             where a.TypeCode == leaveType.TypeCode
                             select a;

                if (result.Count() != 0)
                {
                    error = "【" + leaveType.TypeCode + "】请假编号已经存在，请查证后再操作！";
                    return false;
                }

                dataContxt.HR_LeaveType.InsertOnSubmit(leaveType);
                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改请假类别
        /// </summary>
        /// <param name="leaveType">请假类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        public bool UpdateLeaveType(HR_LeaveType leaveType, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_LeaveType
                             where a.TypeCode == leaveType.TypeCode
                             select a;

                if (result.Count() != 1)
                {
                    error = "【" + leaveType.TypeCode + "】请假编号已经存在，请查证后再操作！";
                    return false;
                }

                HR_LeaveType leaveList = result.Single();

                leaveList.TypeName = leaveType.TypeName;
                leaveList.ParentTypeCode = leaveType.ParentTypeCode;
                leaveList.IncludeHoliday = leaveType.IncludeHoliday;
                leaveList.LeaveMode = leaveType.LeaveMode;
                leaveList.MaxHours = leaveType.MaxHours;
                leaveList.MaxTimes = leaveType.MaxTimes;
                leaveList.MinHours = leaveType.MinHours;
                leaveList.PaidLeave = leaveType.PaidLeave;
                leaveList.Remark = leaveType.Remark;
                leaveList.Recorder = leaveType.Recorder;
                leaveList.RecordTime = leaveType.RecordTime;
                leaveList.NeedAnnex = leaveType.NeedAnnex;
                leaveList.DeleteFlag = leaveType.DeleteFlag;

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除请假类别
        /// </summary>
        /// <param name="typeCode">请假类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteLeaveType(string typeCode, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_LeaveBill
                             where a.LeaveTypeID == typeCode
                             select a;

                if (result.Count() > 0)
                {
                    error = "【" + typeCode + "】请假编号已经运用到请假单中，不能进行删除操作！";
                    return false;
                }

                var resultList = from c in dataContxt.HR_LeaveType
                                 where c.TypeCode == typeCode
                                 select c;

                if (resultList.Count() != 1)
                {
                    error = "信息有误，请查证后再进行此操作！";
                    return false;
                }

                dataContxt.HR_LeaveType.DeleteAllOnSubmit(resultList);
                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
        #endregion

        #region 请假申请操作

        /// <summary>
        /// 获取所有请假申请表信息
        /// </summary>
        /// <param name="returnInfo">请假申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllLeaveBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("请假申请操作", null);
            }
            else
            {
                qr = serverAuthorization.Query("请假申请操作", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 通过员工编号和时间获得请假信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="leaveTime">时间</param>
        /// <param name="endDate">截止时间</param>
        /// /// <returns>返回请假信息数据集</returns>
        public DataTable GetLeaveBill(string workID, DateTime leaveTime,DateTime endDate)
        {
            string sql = "select * from dbo.View_HR_LeaveBill where 员工编号='" + workID + "' and " +
                         " (请假时间 <= '" + leaveTime + "') and (终止时间 >= '" + endDate + "')" +
                         " and  (是否批准 = '" + bool.TrueString + "')";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过员工编号和时间获得请假信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="leaveTime">时间</param>
        /// <param name="endDate">截止时间</param>
        /// /// <returns>返回请假信息数据集</returns>
        public DataTable GetLeaveBillHalfway(string workID, DateTime leaveTime, DateTime endDate)
        {
            string sql = "select * from dbo.View_HR_LeaveBill where 员工编号='" + workID + "' and " +
                         " (day(请假时间)=day('" + leaveTime + "')) and (终止时间 >= '" + endDate + "')" +
                         " and  (是否批准 = '" + bool.TrueString + "')";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 查询某一个员工当月不同类别的请假次数和累计的小时数
        /// </summary>
        /// <param name="leaveTypeID">请假类别编号</param>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">当月的起始日期</param>
        /// <param name="endDate">当月的结束日期</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetCountLeaveBill(string leaveTypeID,string workID,DateTime starDate,DateTime endDate,int billNo)
        {
            string sql = "select count(*) count,CONVERT(float,ISNULL(sum(realHours), 0)) hours from dbo.HR_LeaveBill " +
                         " where leavetypeID='" + leaveTypeID + "' and applicant='" + workID + "' and " +
                         " (beginTime>='" + starDate + "' and endTime<='" + endDate + "')";

            if (billNo != 0)
            {
                sql += " and ID=" + billNo;
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 新增/修改请假申请
        /// </summary>
        /// <param name="leave">请假数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddLeaveBill(HR_LeaveBill leave, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_LeaveBill
                             where a.ID == leave.ID
                             select a;

                if (result.Count() > 0)
                {
                    HR_LeaveBill leaveList = result.Single();

                    leaveList.BeginTime = leave.BeginTime;
                    leaveList.BillStatus = leave.BillStatus;
                    leaveList.EndTime = leave.EndTime;
                    leaveList.OtherExplanation = leave.OtherExplanation;
                    leaveList.RealHours = leave.RealHours;
                    leaveList.Date = leaveList.Date;

                    new AttendanceAnalysis().DataTimeIsRepeat<HR_LeaveBill>(dataContxt, leaveList, leave.Applicant);
                }
                else
                {
                    var resultList = from a in dataContxt.HR_LeaveBill
                                     where a.Applicant == leave.Applicant && a.BeginTime == leave.BeginTime
                                     select a;

                    if (resultList.Count() == 0)
                    {
                        new AttendanceAnalysis().DataTimeIsRepeat<HR_LeaveBill>(dataContxt, leave, leave.Applicant);
                        dataContxt.HR_LeaveBill.InsertOnSubmit(leave);
                    }
                    else 
                    {
                        error = "您在同一时间已经申请了请假单";
                        return false;
                    }
                }


                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改请假信息
        /// </summary>
        /// <param name="leave">请假申请数据集</param>
        /// <param name="roleType">角色类型（部门主管审批，部门负责人审批
        /// 分管领导审批，总经理审批，人力资源部复审）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回False</returns>
        public bool UpdateLeave(HR_LeaveBill leave, string roleType, out string error)
        {
            error = "";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var result = from a in dataContxt.HR_LeaveBill
                             where a.ID == leave.ID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }

                HR_LeaveBill bill = result.Single();
                bill.BillStatus = leave.BillStatus;

                switch (roleType)
                {
                    case "部门主管审批":
                        bill.DeptDirector = leave.DeptDirector;
                        bill.DeptDirectorSignatureDate = leave.DeptDirectorSignatureDate;
                        break;
                    case "部门负责人审批":
                        bill.DeptPrincipal = leave.DeptPrincipal;
                        bill.DeptPrincipalSignatureDate = leave.DeptPrincipalSignatureDate;
                        bill.Authorize = leave.Authorize;
                        bill.UnexcusedReason = leave.UnexcusedReason;
                        bill.Leader = leave.Leader;
                        bill.LeaderSignatureDate = leave.LeaderSignatureDate;
                        break;
                    case "分管领导审批":
                        bill.Leader = leave.Leader;
                        bill.LeaderSignatureDate = leave.LeaderSignatureDate;
                        bill.Authorize = leave.Authorize;
                        bill.UnexcusedReason = leave.UnexcusedReason;
                        break;
                    case "总经理审批":
                        bill.GeneralManager = leave.GeneralManager;
                        bill.GM_SignatureDate = leave.GM_SignatureDate;
                        bill.Authorize = leave.Authorize;
                        bill.UnexcusedReason = leave.UnexcusedReason;
                        break;
                    case "人力资源部复审":
                        bill.HR_Signature = leave.HR_Signature;
                        bill.HR_SignatureDate = leave.HR_SignatureDate;
                        bill.LeaveTypeID = leave.LeaveTypeID;
                        break;
                    default:
                        break;
                }

                dataContxt.SubmitChanges();

                if (GlobalObject.GeneralFunction.StringConvertToEnum<LeaveBillStatus>(bill.BillStatus) == LeaveBillStatus.已完成)
                {
                    ITimeExceptionServer service = ServerModuleFactory.GetServerModule<ITimeExceptionServer>();
                    service.OperationTimeException_Replenishments(dataContxt, bill.ID.ToString(), CE_HR_AttendanceExceptionType.请假);
                }

                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        public bool DeleteLeaveBill(int billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_LeaveBill
                             where a.ID == billID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {
                    dataContxt.HR_LeaveBill.DeleteAllOnSubmit(result);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得最大的ID号
        /// </summary>
        /// <returns>返回最大的ID号</returns>
        public int GetMaxBillNo()
        {
            string sql = "SELECT MAX(ID) AS id FROM HR_LeaveBill";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["id"].ToString());
        }

        #endregion

        public HR_LeaveType GetLeaveType(DepotManagementDataContext ctx, string info)
        {
            var varData = from a in ctx.HR_LeaveType
                          where a.TypeCode == info || a.TypeName == info
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("获取请假类型失败");
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 查询员工在某一天是否有请假
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回请假类别</returns>
        public string GetLeaveTypeByWorkID(string workID, DateTime starTime, DateTime endTime)
        {
            string sql = "select ID,LeaveTypeID,RealHours from HR_LeaveBill where beginTime <= '" + starTime + "' " +
                         " and EndTime >= '" + endTime + "' and Applicant='" + workID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["ID"].ToString() + ";" + dt.Rows[0]["LeaveTypeID"].ToString() + ";" +
                    dt.Rows[0]["RealHours"].ToString();
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获得员工在时间范围内的请假单是否已经全部走完
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>没有全部走完返回False，走完返回True</returns>
        public bool GetLeaveBillByWorkID(string workID, DateTime starTime, DateTime endTime)
        {
            string sql = "select * from HR_LeaveBill where beginTime >= '" + starTime + "' and beginTime <= '" + endTime + "' "+
                         " and Applicant='" + workID + "' and BillStatus <> '已完成'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 修改请假类别后同时修改流水明细
        /// </summary>
        /// <param name="billNo">请假单号</param>
        /// <param name="type">请假类别</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool UpdateAttendanceDaybook(string billNo,string type,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_AttendanceDaybookList
                             where a.BillNo == billNo && a.ResultType == "4"
                             select a;

                if (result.Count() > 0)
                {
                    foreach (HR_AttendanceDaybookList item in result)
                    {
                        HR_AttendanceDaybookList list = item;

                        list.ResultSubclass = type;
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        public void Check_LeaveType(string typeCode, decimal hours, int? billID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            HR_LeaveType leaveType = GetLeaveType(ctx, typeCode);

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@TypeCode", leaveType.TypeCode);
            paramTable.Add("@Hours", hours);
            paramTable.Add("@WorkID", BasicInfo.LoginID);
            paramTable.Add("@BillID", billID == null ? 0 : billID);

            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("HR_LeaveBill_Type_Check", paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                throw new Exception(Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]));
            }
        }
    }
}
