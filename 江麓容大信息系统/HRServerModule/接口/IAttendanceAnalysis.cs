using System;
using GlobalObject;
using System.Data;
using System.Collections.Generic;
using ServerModule;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 人力分析服务组件
    /// </summary>
    public interface IAttendanceAnalysis
    {
        /// <summary>
        /// 检测业务日期是否重复
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ctx"></param>
        /// <param name="obj"></param>
        /// <param name="workID"></param>
        void DataTimeIsRepeat<T>(DepotManagementDataContext ctx, T obj, string workID);

        /// <summary>
        /// 异常单据操作
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="operationMode">操作类型</param>
        /// <param name="lstInfo">操作数据</param>
        /// <param name="lstPersonnel">人员列表</param>
        void Operation_Exception(CE_HR_AttendanceExceptionType billType, CE_OperatorMode operationMode,
            List<object> lstInfo, List<PersonnelBasicInfo> lstPersonnel);

        /// <summary>
        /// 获得需要操作的单据信息
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="operationMode">操作类型</param>
        /// <returns>返回DataTable</returns>
        DataTable GetBusinessInfo_Exception(CE_HR_AttendanceExceptionType billType, CE_OperatorMode operationMode);

        /// <summary>
        /// 分析主流程
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="workID">工号</param>
        void Analysis_Main(DateTime startTime, DateTime endTime, string workID);

        /// <summary>
        /// 考勤分析主方法
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="workID">工号</param>
        void Analysis_Main(DepotManagementDataContext ctx, DateTime startTime, DateTime endTime, string workID);
    }
}
