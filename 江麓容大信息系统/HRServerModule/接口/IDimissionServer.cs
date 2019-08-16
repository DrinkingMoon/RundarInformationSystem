using System;
using ServerModule;
using System.Linq;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum DimissionBillStatus
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
        /// 等待人力资源审阅
        /// </summary>
        等待人力资源审阅,

        /// <summary>
        /// 等待分管领导审核
        /// </summary>
        等待分管领导审核,

        /// <summary>
        /// 等待总经理批准
        /// </summary>
        等待总经理批准,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 员工离职操作类
    /// </summary>
    public interface IDimissionServer
    {
        /// <summary>
        /// 获取员工离职所有信息
        /// </summary>
        /// <param name="returnInfo">员工离职信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool GetAllDimission(out PlatformManagement.IQueryResult returnInfo, out string error);

        /// <summary>
        /// 新增或修改员工离职申请信息
        /// </summary>
        /// <param name="dimission">员工离职数据集</param>
        /// <param name="type">进行步骤(提交申请/部门负责人审批/提交附加信息/人力资源部审批/分管领导审批/总经理批准)</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回单据编号，失败返回0</returns>
        string AddAndUpdateDimission(HR_DimissionBill dimission, string type, out string error);

        /// <summary>
        /// 获取员工离职所有信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <returns>返回数据集</returns>
        HR_DimissionBill GetAllDimission(int billID);

        /// <summary>
        /// 删除离职申请表
        /// </summary>
        /// <param name="userCode">用户编号</param>
        /// <param name="allowDate">申请时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteDimission(string userCode, DateTime allowDate, out string error);

        /// <summary>
        /// 获得某个员工的岗位调动信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回数据集</returns>
        System.Data.DataTable GetDimissionBillByWorkID(string workID, out string error);
    }
}
