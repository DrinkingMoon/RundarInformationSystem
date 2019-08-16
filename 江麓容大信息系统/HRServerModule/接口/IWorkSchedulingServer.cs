using System;
using ServerModule;
using System.Collections.Generic;
using System.Data;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 排班信息操作类
    /// </summary>
    public interface IWorkSchedulingServer
    {
        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 新增修改排班定义信息
        /// </summary>
        /// <param name="definition">排班定义数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AddDefinition(ServerModule.HR_WorkSchedulingDefinition definition, out string error);

        /// <summary>
        /// 通过编号删除排班定义
        /// </summary>
        /// <param name="code">排班编码</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteDefinition(string code, out string error);

        /// <summary>
        /// 获取所有排班定义
        /// </summary>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetWorkSchedulingDefinition();

        /// <summary>
        /// 获得员工的排班信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="time">日期</param>
        /// <returns>返回数据集</returns>
        DataTable GetWorkSchedulingByWorkIDAndDate(string workID, DateTime time);

        /// <summary>
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteWorkScheduling(int billID, out string error);

        /// <summary>
        /// 修改排班信息
        /// </summary>
        /// <param name="schedule">排班信息</param>
        /// <param name="personnel">排班人员</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool UpdateWorkScheduling(HR_WorkScheduling schedule, List<HR_WorkSchedulingDetail> personnel, int billID, out string error);

        /// <summary>
        /// 新增排班信息
        /// </summary>
        /// <param name="schedule">排班信息主信息</param>
        /// <param name="personnel">排班人员</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回新增的单据编号，失败返回-1</returns>
        int AddWorkScheduling(HR_WorkScheduling schedule, List<HR_WorkSchedulingDetail> personnel, out string error);

        /// <summary>
        /// 获取所有排班信息
        /// </summary>
        /// <returns>返回数据集</returns>
        DataTable GetWorkScheduling();

         /// <summary>
        /// 获取所有排班信息
        /// </summary>
        /// <param name="returnInfo">排班信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllWorkScheduling(out IQueryResult returnInfo, out string error);
                
        /// <summary>
        /// 获取指定单据号的排班信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的排班对象，失败返回null</returns>
        View_HR_WorkScheduling GetWorkSchedulingByBillNo(int billNo);

        /// <summary>
        /// 判断申请人在该月是否已经申请了排班信息
        /// </summary>
        /// <param name="workID">申请人编号</param>
        /// <param name="month">月份</param>
        /// <returns>存在返回true，不存在返回false</returns>
        bool IsExise(string workID, int month);

        /// <summary>
        /// 领导审核修改排班信息
        /// </summary>
        /// <param name="schedule">排班信息</param>
        /// <param name="role">角色（主管或负责人）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool UpdateAuditingWorkScheduling(HR_WorkScheduling schedule, string role, out string error);

        /// <summary>
        /// 通过单据号获得排班人员的排班信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="numberOfPeople">成功则返回此排班单据中包含的排班人数，失败返回-1</param>
        /// <returns>成功返回数据集，失败返回null</returns>
        List<View_HR_WorkSchedulingDetail> GetWorkSchedulingDetail(int billNo, out int numberOfPeople);

        /// <summary>
        /// 根据单号统计班次
        /// </summary>
        /// <param name="billNo">单号</param>
        /// <returns>返回数据集</returns>
        DataTable GetDefinitionStatistics(int billNo);
    }
}
