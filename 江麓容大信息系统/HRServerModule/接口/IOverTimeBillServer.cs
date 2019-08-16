using System;
using System.Data;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum OverTimeBillStatus
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
        /// 确认加班完成情况
        /// </summary>
        确认加班完成情况,

        /// <summary>
        /// 等待人力资源复审
        /// </summary>
        等待人力资源复核,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 加班申请操作类
    /// </summary>
    public interface IOverTimeBillServer
    {
        /// <summary>
        /// 处理自动生成的加班单的人员信息
        /// </summary>
        /// <param name="infoTable">人员信息列表</param>
        /// <param name="saveFlag">保存标志</param>
        void AutoCreateOverTime_BatchOperation(DataTable infoTable, bool saveFlag);

        /// <summary>
        /// 获取自动生成的加班单的人员信息
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable AutoCreateOverTime_ShowPersonnel();

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 新增加班申请
        /// </summary>
        /// <param name="overTime">加班申请主信息</param>
        /// <param name="personnel">加班人员</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回新增的单据编号，失败返回-1</returns>
        int AddOverTimeBill(ServerModule.HR_OvertimeBill overTime, System.Collections.Generic.List<ServerModule.HR_OvertimePersonnel> personnel, out string error);

        /// <summary>
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteOverTimeBill(int billID, out string error);

        /// <summary>
        /// 获取所有加班申请表信息
        /// </summary>
        /// <param name="returnInfo">加班申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOverTimeBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获取所有加班申请表信息
        /// </summary>
        /// <param name="returnInfo">加班申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOverTimeBillByWorkID(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 通过单据号获得加班人员
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        DataTable GetOverTimePersonnelByID(string billID);

        /// <summary>
        /// 通过单据号获得加班信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        DataTable GetOverTimeBillByID(string billID);

        /// <summary>
        /// 领导审核修改加班单据
        /// </summary>
        /// <param name="overTime">出差单据数据集</param>
        /// <param name="roleType">角色类型</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateOverTimeBill(ServerModule.HR_OvertimeBill overTime, string roleType, out string error);

        /// <summary>
        /// 修改加班申请
        /// </summary>
        /// <param name="overTime">加班申请主信息</param>
        /// <param name="personnel">加班人员</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        bool UpdateOverTimeBill(ServerModule.HR_OvertimeBill overTime, System.Collections.Generic.List<ServerModule.HR_OvertimePersonnel> personnel, int billID, out string error);

        /// <summary>
        /// 通过员工编号和申请时间获得加班申请单
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <returns></returns>
        DataTable GetOverTimeByWorkID(string workID, DateTime starDate, DateTime endDate);

        /// <summary>
        /// 通过员工编号和申请时间获得加班申请单
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <returns>返回数据集</returns>
        DataTable IsExistOverTimeByWorkID(string workID, DateTime starDate, DateTime endDate);

        /// <summary>
        /// 判断员工加班的补偿方式是否可以选择
        /// </summary>
        /// <param name="workPost">岗位名称</param>
        /// <param name="workID">员工编号</param>
        /// <param name="dept">科室</param>
        /// <returns>可以返回True，不可以返回False</returns>
        bool IsChooseDoubleRest(string workPost, string dept, string workID);

        /// <summary>
        /// 获取员工当月的加班小时数
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns>当有加班时间时返回加班小时数，没有加班时间时，返回-1</returns>
        double GetMonthRealHour(string workID, DateTime starDate, DateTime endDate);

        /// <summary>
        /// 通过单据号修改加班单的实际加班小时数
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="hours">实际小时数</param>
        /// <returns>修改成功返回True失败返回False</returns>
        bool UpdateOverTimeBillByHours(string billNo, double hours);

        /// <summary>
        /// 通过员工编号和申请时间获得加班申请单
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <returns>返回数据集</returns>
        DataTable GetOverTimeByWorkIDAndDate(string workID, DateTime starDate, DateTime endDate);
    }
}
