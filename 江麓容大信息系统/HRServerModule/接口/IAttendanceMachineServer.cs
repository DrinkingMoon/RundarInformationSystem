using System;
using System.Data;
using System.Collections.Generic;
using ServerModule;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 异常类别
    /// </summary>
    public enum ExceptionType
    {
        /// <summary>
        /// 迟到
        /// </summary>
        迟到=1,

        /// <summary>
        /// 早退
        /// </summary>
        早退,

        /// <summary>
        /// 旷工
        /// </summary>
        旷工,

        /// <summary>
        /// 请假
        /// </summary>
        请假,

        /// <summary>
        /// 加班
        /// </summary>
        加班,

        /// <summary>
        /// 出差
        /// </summary>
        出差,

        /// <summary>
        /// 未打卡
        /// </summary>
        未打卡,

        /// <summary>
        /// 正常
        /// </summary>
        正常
    }

    /// <summary>
    /// 考勤机导入的人员考勤明细表接口
    /// </summary>
    public interface IAttendanceMachineServer
    {
        /// <summary>
        /// 通过单据号删除考勤机导入的人员考勤明细
        /// </summary>
        /// <param name="billID">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteAttendanceMachineByID(string billID, out string error);

        /// <summary>
        /// 获取考勤机导入的人员考勤明细表
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetAllInfo();

        /// <summary>
        /// 获取考勤机导入的人员考勤历史明细表
        /// </summary>
        /// <returns>返回数据集</returns>
        DataTable GetHistoryAllInfo();

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="lstDate">HR_AttendanceMachineDataList对象列表</param>
        /// <returns>成功返回true失败返回False</returns>
        void InsertAttendanceMachine(List<HR_AttendanceMachineDataList> lstDate);

        /// <summary>
        /// 获取考勤异常类型
        /// </summary>
        /// <returns>返回数据集</returns>
        List<HR_AttendanceExceptionType> GetExceptionType();

        /// <summary>
        /// 通过异常类型名获取对应的ID
        /// </summary>
        /// <param name="typeName">异常名</param>
        /// <returns>返回对应的ID</returns>
        int GetExceptionTypeID(string typeName);

        /// <summary>
        /// 通过异常类别ID获取异常类型名
        /// </summary>
        /// <param name="typeCode">ID</param>
        /// <returns>返回对应的异常类型名</returns>
        string GetExceptionTypeName(int typeCode);

        /// <summary>
        /// 通过员工编号和时间获取考勤机导入的人员考勤明细表
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="exDate">打卡日期</param>
        /// <returns>返回数据集</returns>
        DataTable GetAttendanceMachineData(string workID, DateTime exDate);

        /// <summary>
        /// 通过员工编号和打卡日期删除考勤机导入的人员考勤明细
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="date">打卡日期</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteAttendanceMachineByWorkIDAndDate(string workID, DateTime date, out string error);

        /// <summary>
        /// 通过时间和员工编号获得打卡时间
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="workID">员工编号</param>
        /// <returns>返回打卡时间</returns>
        string GetAttendanceMachineDateListByBefor(DateTime date, string workID);

        /// <summary>
        /// 获取人员是否有考勤信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="date">日期</param>
        /// <returns>返回数据集</returns>
        int GetWorkIDIsExist(string workID, DateTime date);

        /// <summary>
        /// 通过打卡号获得员工编号
        /// </summary>
        /// <param name="card">打卡号</param>
        /// <returns>返回满足条件的员工编号</returns>
        string GetCardIDWorkIDMapping(string card);
        
        /// <summary>
        /// 判断是否需要考勤的人员都有记录
        /// </summary>
        /// <param name="error">错误信息</param>
        /// <returns>没有返回false，有返回True</returns>
        bool GetIsAllPersonnelPunchRecord(out string error);
    }
}
