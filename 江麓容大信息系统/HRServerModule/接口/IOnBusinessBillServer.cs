using System;
using System.Data;
using ServerModule;
using System.Collections.Generic;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum OnBusinessBillStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待部门负责人审核
        /// </summary>
        等待部门负责人审核,

        /// <summary>
        /// 等待随行人员部门确认
        /// </summary>
        等待随行人员部门确认,

        /// <summary>
        /// 等待分管领导审批
        /// </summary>
        等待分管领导审批,

        /// <summary>
        /// 等待销差人确认
        /// </summary>
        等待销差人确认,

        /// <summary>
        /// 等待总经理批准
        /// </summary>
        等待总经理批准,

        /// <summary>
        /// 等待出差结果说明
        /// </summary>
        等待出差结果说明,

        /// <summary>
        /// 审批未通过
        /// </summary>
        审批未通过,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 出差申请表操作接口
    /// </summary>
    public interface IOnBusinessBillServer
    {

        List<HR_OnBusinessPersonnel> GetPersonnel(int billID);

        /// <summary>
        /// 获得单条记录信息
        /// </summary>
        /// <param name="billID">单据ID</param>
        /// <returns>返回单条记录信息</returns>
        HR_OnBusinessBill GetSingleInfo(int billID);

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        string QueryResultFilter { get; set; }

        /// <summary>
        /// 获取所有出差申请表信息
        /// </summary>
        /// <param name="returnInfo">出差申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOnBusinessBill(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获取所有出差申请表信息
        /// </summary>
        /// <param name="returnInfo">出差申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOnBusinessBillByWorkID(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 出差随行人员部门负责人查看
        /// </summary>
        /// <param name="returnInfo">出差申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllOnBusinessBillByDeptCode(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 获得出差申请表信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">单据状态</param>
        /// <returns>返回出差申请表数据集</returns>
        DataTable GetOnBusinessBillByTime(DateTime startTime, DateTime endTime, string status);

        /// <summary>
        /// 通过单据编号获得单据信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <returns>返回数据集</returns>
        DataTable GetOnBusinessBillByID(int billID);

        /// <summary>
        /// 新增出差申请
        /// </summary>
        /// <param name="onBusiness">出差申请主信息</param>
        /// <param name="personnel">出差人员</param>
        /// <param name="schedule">出差行程安排</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回新增的单据编号，失败返回0</returns>
        int AddOnBusinessBill(HR_OnBusinessBill onBusiness, List<HR_OnBusinessPersonnel> personnel,
                                     List<HR_OnBusinessSchedule> schedule, out string error);

        /// <summary>
        /// 通过单据号获得出差人员
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        DataTable GetOnBusinessPersonnel(string billID);

        /// <summary>
        /// 通过单据号获得出差行程
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        DataTable GetOnBusinessSchedule(string billID);

        /// <summary>
        /// 修改出差申请
        /// </summary>
        /// <param name="onBusiness">出差申请主信息</param>
        /// <param name="personnel">出差人员</param>
        /// <param name="schedule">出差行程安排</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        bool UpdateOnBusinessBill(HR_OnBusinessBill onBusiness, List<HR_OnBusinessPersonnel> personnel,
                                     List<HR_OnBusinessSchedule> schedule, int billID, out string error);

        /// <summary>
        /// 领导审核修改出差单据
        /// </summary>
        /// <param name="onBusiness">出差单据数据集</param>
        /// <param name="roleType">角色类型</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateOnBusinessBill(HR_OnBusinessBill onBusiness, string roleType, out string error);

        /// <summary>
        /// 随行人员部门确认修改出差单据
        /// </summary>
        /// <param name="personnel">出差人员</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool UpdateOnBusinessPersonnel(HR_OnBusinessPersonnel personnel, int billID, out string error);

        /// <summary>
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        bool DeleteOnBusinessBill(int billID, out string error);

        /// <summary>
        /// 通过日期判断人员在当天是否出差
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="recordDate">打卡日期</param>
        /// <returns>返回数据集</returns>
        DataTable GetOnBusinessBillByWorkIDAndTime(string workID, DateTime recordDate);

        /// <summary>
        /// 通过日期判断人员在当天是否出差
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回数据集</returns>
        string IsExistOnBusinessBillByWorkIDAndTime(string workID, DateTime starTime, DateTime endTime);

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="strRebackReason">回退状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnBill(string strDJH, string strBillStatus, string strRebackReason, out string error);
    }
}
