using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using ServerModule;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤方案操作类
    /// </summary>
    public interface IAttendanceSchemeServer
    {
        /// <summary>
        /// 新增修改考勤方案
        /// </summary>
        /// <param name="attendance">考勤方案数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool AddAttendanceScheme(ServerModule.HR_AttendanceScheme attendance, out string error);

        /// <summary>
        /// 通过考勤编码删除考勤方案
        /// </summary>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteAttendanceScheme(string schemeCode, out string error);

        /// <summary>
        /// 获取所有的考勤方案
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetAllAttendanceScheme();

        /// <summary>
        /// 获取考勤方案Linq数据集(得到第一条数据)
        /// </summary>
        /// <returns>返回数据集</returns>
        IQueryable<HR_AttendanceScheme> GetLinqResult();

        /// <summary>
        /// 获取考勤方案(编码+名称)
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetAttendanceScheme();

        /// <summary>
        /// 通过编号获取考勤方案信息
        /// </summary>
        /// <returns>返回数据集</returns>
        HR_AttendanceScheme GetAttendanceSchemeByCode(string schemeCode);

        /// <summary>
        /// 通过员工编号删除
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true,失败返回False</returns>
        bool DeleteAttendanceSetting(List<string> workID, out string error);

        /// <summary>
        /// 批量修改考勤方案
        /// </summary>
        /// <param name="dept">部门编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateAttendanceSettingByDept(string dept, string schemeCode,bool isSubsidize, out string error);

        /// <summary>
        /// 单个员工修改考勤
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateAttendanceSetting(string workID, string schemeCode, bool isSubsidize, out string error);

        /// <summary>
        /// 单个员工设置考勤
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddAttendanceSetting(string workID, string schemeCode,bool isSubsidize, out string error);

        /// <summary>
        /// 批量设置考勤方案
        /// </summary>
        /// <param name="dept">部门编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="flag">是否替换（true替换，false不替换）</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddAttendanceSettingByDept(string dept, string schemeCode,bool flag,bool isSubsidize, out string error);

        /// <summary>
        /// 获取员工考勤方案
        /// </summary>
        /// <returns>返回数据集</returns>
        DataTable GetAttendanceSetting();

        /// <summary>
        /// 通过员工编号获取员工的考勤方案
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回结果集</returns>
        HR_AttendanceSetting GetAttendanceSettingByWorkID(string workID);

        /// <summary>
        /// 判断是否所有人员都设置了考勤方案
        /// </summary>
        /// <returns>全部设置返回true，否则返回false</returns>
        bool GetIsSetting();
    }
}
