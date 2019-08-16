using System;
using PlatformManagement;
using ServerModule;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤异常登记操作接口
    /// </summary>
    public interface ITimeExceptionServer
    {

        /// <summary>
        /// 获取考勤异常登记表
        /// </summary>
        /// <param name="returnInfo">考勤异常登记表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllBill(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 修改考勤异常登记信息
        /// </summary>
        /// <param name="timeException">考勤异常登记信息数据集</param>
        /// <param name="role">角色（部门审核、人力资源审核）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回False</returns>
        bool UpdateTimeException(ServerModule.HR_TimeException timeException, string role, out string error);

        /// <summary>
        /// 操作补单情况下的考勤流水与异常信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="billNo">关联单号</param>
        /// <param name="exceptionType">异常类型</param>
        /// <returns>成功返回True失败返回False</returns>
        void OperationTimeException_Replenishments(DepotManagementDataContext dataContxt, string billNo, GlobalObject.CE_HR_AttendanceExceptionType exceptionType);

        /// <summary>
        /// 修改考勤异常登记信息（强制处理）
        /// </summary>
        /// <param name="timeException">考勤异常登记信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回False</returns>
        bool UpdateTimeException(ServerModule.HR_TimeException timeException, out string error);
    }
}
