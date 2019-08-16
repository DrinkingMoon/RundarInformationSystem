using System;
using System.Data;
using PlatformManagement;
using ServerModule;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum LeaveBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待主管审核
        /// </summary>
        等待主管审核,

        /// <summary>
        /// 等待部门负责人
        /// </summary>
        等待部门负责人审核,

        /// <summary>
        /// 等待分管领导审批
        /// </summary>
        等待分管领导审批,

        /// <summary>
        /// 等待总经理审批
        /// </summary>
        等待总经理审批,

        /// <summary>
        /// 等待人力资源复核
        /// </summary>
        等待人力资源复核,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 请假操作接口
    /// </summary>
    public interface ILeaveServer
    {

        /// <summary>
        /// 获取请假类别(拼接编号和名称)
        /// </summary>
        /// <param name="typeName">请假类别名称</param>
        /// <returns>返回数据集</returns>
        DataTable GetLeaveTypeByCode_Show(string typeName);

        /// <summary>
        /// 检查类别
        /// </summary>
        /// <param name="typeCode"></param>
        /// <param name="hours"></param>
        /// <param name="billID"></param>
        void Check_LeaveType(string typeCode, decimal hours, int? billID);

        /// <summary>
        /// 获得请假类别
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="info">查询条件信息</param>
        /// <returns>返回请假单类型对象</returns>
        HR_LeaveType GetLeaveType(DepotManagementDataContext ctx, string info);

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 新增请假类别
        /// </summary>
        /// <param name="leaveType">请假类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        bool AddLeaveType(ServerModule.HR_LeaveType leaveType, out string error);

        /// <summary>
        /// 删除请假类别
        /// </summary>
        /// <param name="typeCode">请假类别编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteLeaveType(string typeCode, out string error);

        /// <summary>
        /// 获取请假类别
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetAllLeaveType();

        /// <summary>
        /// 通过类别编号获得请假类别信息
        /// </summary>
        /// <param name="typeCode">类别编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetLeaveType(string typeCode);

        /// <summary>
        /// 获取请假类别(拼接编号和名称)
        /// </summary>
        /// <param name="typeName">请假类别名称</param>
        /// <returns>返回数据集</returns>
        DataTable GetLeaveTypeByCode(string typeName);

        /// <summary>
        /// 通过类别编号获得类别名称
        /// </summary>
        /// <param name="typeCode">请假类别编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetLeaveTypeByName(string typeCode);

        /// <summary>
        /// 修改请假类别
        /// </summary>
        /// <param name="leaveType">请假类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        bool UpdateLeaveType(ServerModule.HR_LeaveType leaveType, out string error);

        /// <summary>
        /// 获取所有请假申请表信息
        /// </summary>
        /// <param name="returnInfo">请假申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllLeaveBill(out IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过请假类别编号获取请假类别信息
        /// </summary>
        /// <param name="TypeCode">请假类别编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetLeaveTypeByTypeID(string TypeCode);

        /// <summary>
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回false</returns>
        bool DeleteLeaveBill(int billID, out string error);

        /// <summary>
        /// 修改请假信息
        /// </summary>
        /// <param name="leave">请假申请数据集</param>
        /// <param name="roleType">角色类型（部门主管审批，部门负责人审批
        /// 分管领导审批，总经理审批，提交医院证明附件，人力资源部复审）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回False</returns>
        bool UpdateLeave(HR_LeaveBill leave, string roleType, out string error);

        /// <summary>
        /// 新增/修改请假申请
        /// </summary>
        /// <param name="leave">请假数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddLeaveBill(HR_LeaveBill leave, out string error);

        /// <summary>
        /// 获得最大的ID号
        /// </summary>
        /// <returns>返回最大的ID号</returns>
        int GetMaxBillNo();

        /// <summary>
        /// 通过员工编号和时间获得请假信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="leaveTime">时间</param>
        /// <param name="endDate">截止时间</param>
        /// <returns>返回请假信息数据集</returns>
        DataTable GetLeaveBill(string workID, DateTime leaveTime, DateTime endDate);

        /// <summary>
        /// 查询某一个员工当月不同类别的请假次数和累计的小时数
        /// </summary>
        /// <param name="leaveTypeID">请假类别编号</param>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">当月的起始日期</param>
        /// <param name="endDate">当月的结束日期</param>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        DataTable GetCountLeaveBill(string leaveTypeID, string workID, DateTime starDate, DateTime endDate, int billNo);

        /// <summary>
        /// 通过类别编号获得请假类别父级编号
        /// </summary>
        /// <param name="typeCode">类别编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetLeaveTypeParentCode(string typeCode);

        /// <summary>
        /// 通过员工编号和时间获得请假信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="leaveTime">时间</param>
        /// <param name="endDate">截止时间</param>
        /// /// <returns>返回请假信息数据集</returns>
        DataTable GetLeaveBillHalfway(string workID, DateTime leaveTime, DateTime endDate);

        /// <summary>
        /// 查询员工在某一天是否有请假
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回请假类别</returns>
        string GetLeaveTypeByWorkID(string workID, DateTime starTime, DateTime endTime);

        /// <summary>
        /// 获得员工在时间范围内的请假单是否已经全部走完
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>没有全部走完返回False，走完返回True</returns>
        bool GetLeaveBillByWorkID(string workID, DateTime starTime, DateTime endTime);

        /// <summary>
        /// 修改请假类别后同时修改流水明细
        /// </summary>
        /// <param name="billNo">请假单号</param>
        /// <param name="type">请假类别</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true，失败返回false</returns>
        bool UpdateAttendanceDaybook(string billNo, string type, out string error);
    }
}
