using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using DBOperate;
using System.Collections;
using GlobalObject;
using PlatformManagement;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤分析汇总操作类
    /// </summary>
    class AttendanceSummaryServer : Service_Peripheral_HR.IAttendanceSummaryServer
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

        /// <summary>
        /// 添加考勤分析汇总
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">考勤起始日期</param>
        /// <param name="endDate">考勤截止日期</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddAttendanceSummary(string workID, DateTime starDate, DateTime endDate, out string error)
        {
            error = "";

            string sql = "SELECT * FROM HR_AttendanceSummary where year=" + endDate.Year + " and month=" + endDate.Month;
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null)
            {
                Hashtable paramTable = new Hashtable();

                paramTable.Add("@workID", workID);
                paramTable.Add("@starDate", starDate.Date);
                paramTable.Add("@endDate", endDate.Date);
                paramTable.Add("@recorder", BasicInfo.LoginID);

                if (!AccessDB.ExecuteDbProcedure("HR_Add_AttendanceSummary", paramTable, out error))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 重置允许加班调休的时间
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddAttendanceSummaryByAllowOverTime(out string error)
        {
            error = "";

            if (!AccessDB.ExecuteDbProcedure("HR_Update_OverTime", out error))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 获得考勤统计
        /// </summary>
        /// <param name="returnInfo">结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllSummary(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("考勤统计", null);
            }
            else
            {
                qr = serverAuthorization.Query("考勤统计", null, QueryResultFilter);
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
        /// 获得允许调休小时数
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回允许调休小时数</returns>
        public string GetAllowMobileHours(string workID)
        {
            string sql = " select Top 1 允许调休小时数 from dbo.view_HR_AttendanceSummary  " +
                         " where 员工编号 = '" + workID + "' order by 年份 desc,月份 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["允许调休小时数"].ToString();
            }

            return "0";
        }

        /// <summary>
        /// 获得请年假的次数
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="year">年份</param>
        /// <returns>返回请年假的次数</returns>
        public string GetLeaveYearTimes(string workID, int year)
        {
            string sql = "select 年假次数 from dbo.view_HR_AttendanceSummary " +
                        " where 员工编号 ='" + workID + "'and 年份=" + year;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["年假次数"].ToString();
            }

            return "0";
        }

        /// <summary>
        /// 判断调休时间是否不小于0
        /// </summary>
        /// <param name="workid">员工工号</param>
        /// <returns>调休时间小于0返回false，否则返回true</returns>
        public bool GetAllowMobileVacationHour(string workid)
        {
            string sql = "select AllowMobileVacationHours, Year, Month from HR_AttendanceSummary where id = (" +
                         "select MAX(ID) from dbo.HR_AttendanceSummary" +
                         " where workid='" + workid + "')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);


            if (dt != null && dt.Rows.Count > 0)
            {
                object tempHors = dt.Rows[0]["AllowMobileVacationHours"];

                if (tempHors == null)
                {
                    tempHors = 0;
                }

                if (Convert.ToDouble(tempHors) >= 0)
                {
                    return true;
                }
                else
                {
                    DateTime bTime = Convert.ToDateTime(dt.Rows[0]["Year"].ToString() + "-" + dt.Rows[0]["Month"].ToString() + "-1");

                    sql = " select SUM(a.VerifyHours) from HR_OvertimeBill as a inner join HR_OvertimePersonnel as b on a.ID = b.BillID " +
                          " where a.Date >= '" + bTime.ToShortDateString() + "' and a.BillStatus = '已完成' and b.WorkID = '" + workid + "'";

                    dt = GlobalObject.DatabaseServer.QueryInfo(sql);

                    if (dt == null || dt.Rows.Count == 0 
                        || dt.Rows[0][0] == null 
                        || GeneralFunction.IsNullOrEmpty(dt.Rows[0][0].ToString()))
                    {
                        return false;
                    }
                    else if (Convert.ToDecimal(dt.Rows[0][0]) - Convert.ToDecimal(tempHors) > 0)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        #region 2017.11.08, 夏石友, 根据信息化系统变更处置申请单变更允许调休小时数(向菲斐提)

        /// <summary>
        /// 更新员工允许调休小时数
        /// </summary>
        /// <param name="lstUpdate">要更新的数据列表</param>
        public void UpdateAllowMobileHours(List<HR_AttendanceSummary> lstUpdate)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                foreach (var summary in lstUpdate)
                {
                    var info = from r in ctx.HR_AttendanceSummary
                               where r.WorkID == summary.WorkID && r.Year == summary.Year && r.Month == summary.Month
                               select r;

                    if (info.Count() == 1)
                    {
                        HR_AttendanceSummary updatedInfo = info.Single();

                        string log = string.Format("将工号{0},{1}年{2}月允许调休小时数从{3}变更为{4}",
                            summary.WorkID, summary.Year, summary.Month, updatedInfo.AllowMobileVacationHours, summary.AllowMobileVacationHours);

                        updatedInfo.AllowMobileVacationHours = summary.AllowMobileVacationHours;

                        _Sys_Log sysLog = new _Sys_Log();

                        sysLog.Date = ServerTime.Time;
                        sysLog.EventInfo = log;
                        sysLog.EventType = "变更考勤统计数据";
                        sysLog.HostName = sysLog.EventType;
                        sysLog.LoginName = BasicInfo.LoginName;

                        ctx._Sys_Log.InsertOnSubmit(sysLog);
                    }
                    else
                    {
                        string exce = string.Format("考勤统计表中【工号{0},{1}年{2}月】数据记录不等于1，可能不存在或存在多条记录",
                            summary.WorkID, summary.Year, summary.Month);

                        throw new Exception(exce);
                    }
                }

                ctx.SubmitChanges();
            }
        }

        #endregion
    }
}
