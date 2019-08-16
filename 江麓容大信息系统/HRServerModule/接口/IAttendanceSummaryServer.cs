using System;
using PlatformManagement;
using ServerModule;
using System.Collections.Generic;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤分析汇总操作接口类
    /// </summary>
    public interface IAttendanceSummaryServer
    {
        /// <summary>
        /// 添加考勤分析汇总
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">考勤起始日期</param>
        /// <param name="endDate">考勤截止日期</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddAttendanceSummary(string workID, DateTime starDate, DateTime endDate, out string error);

        /// <summary>
        /// 重置允许加班调休的时间
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddAttendanceSummaryByAllowOverTime(out string error);

        /// <summary>
        /// 获得考勤统计
        /// </summary>
        /// <param name="returnInfo">结果集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllSummary(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获得允许调休小时数
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回允许调休小时数</returns>
        string GetAllowMobileHours(string workID);

        /// <summary>
        /// 获得请年假的次数
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="year">年份</param>
        /// <returns>返回请年假的次数</returns>
        string GetLeaveYearTimes(string workID, int year);

        /// <summary>
        /// 判断调休时间是否不小于0
        /// </summary>
        /// <param name="workid">员工工号</param>
        /// <returns>调休时间小于0返回false，否则返回true</returns>
        bool GetAllowMobileVacationHour(string workid);
                
        /// <summary>
        /// 更新员工允许调休小时数
        /// </summary>
        /// <param name="lstUpdate">要更新的数据列表</param>
        void UpdateAllowMobileHours(List<HR_AttendanceSummary> lstUpdate);
    }
}
