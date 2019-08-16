using System;
using ServerModule;
using System.Data;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 人员考勤流水账操作类接口
    /// </summary>
    public interface IAttendanceDaybookServer
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 获取人员考勤流水账
        /// </summary>
        /// <param name="returnInfo">人员考勤流水账</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllDayBook(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获得考勤流水主表信息
        /// </summary>
        /// <param name="beginDate">考勤起始日期</param>
        /// <param name="endDate">考勤截止日期</param>
        /// <returns>返回数据集</returns>
        DataTable GetDayBookView(string beginDate, string endDate);

        /// <summary>
        /// 获得考勤流水中有记录的人员编号（去除重复编号）
        /// </summary>
        /// <param name="beginDate">考勤起始日期</param>
        /// <param name="endDate">考勤截止日期</param>
        /// <returns>返回数据集</returns>
        DataTable GetDayBookViewByDate(string beginDate, string endDate);
    }
}
