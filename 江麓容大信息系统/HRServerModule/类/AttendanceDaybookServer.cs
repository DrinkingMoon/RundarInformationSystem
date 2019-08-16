using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 人员考勤流水账操作类
    /// </summary>
    class AttendanceDaybookServer : Service_Peripheral_HR.IAttendanceDaybookServer
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
        /// 获取人员考勤流水账
        /// </summary>
        /// <param name="returnInfo">人员考勤流水账</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllDayBook(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("人员考勤流水账", null);
            }
            else
            {
                qr = serverAuthorization.Query("人员考勤流水账", null, QueryResultFilter);
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
        /// 获得考勤流水主表信息
        /// </summary>
        /// <param name="beginDate">考勤起始日期</param>
        /// <param name="endDate">考勤截止日期</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDayBookView(string beginDate,string endDate)
        {
            string sql = "select * from View_HR_AttendanceDaybookMain";

            if (beginDate != "")
            {
                sql += " where 日期 between '" + beginDate + "' and '" + endDate + "'";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 获得考勤流水中有记录的人员编号（去除重复编号）
        /// </summary>
        /// <param name="beginDate">考勤起始日期</param>
        /// <param name="endDate">考勤截止日期</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDayBookViewByDate(string beginDate, string endDate)
        {
            string sql = "select distinct(员工编号) from View_HR_AttendanceDaybookMain ";

            if (beginDate != "")
            {
                sql += " where 日期 between '" + beginDate + "' and '" + endDate + "'";
            }

            sql += " order by 员工编号";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

    }
}
