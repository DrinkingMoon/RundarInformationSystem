using System;
using System.Data;
namespace ServerModule
{
    /// <summary>
    /// 单据状态
    /// </summary>
    public enum SalesOrderStatus
    {
        /// <summary>
        /// 新建单据
        /// </summary>
        新建单据,

        /// <summary>
        /// 等待审核
        /// </summary>
        等待审核,

        /// <summary>
        /// 等待评审
        /// </summary>
        等待评审,

        /// <summary>
        /// 等待确认
        /// </summary>
        等待确认评审,

        /// <summary>
        /// 等待评审结果
        /// </summary>
        等待评审结果,

        /// <summary>
        /// 等待生效,自动生成出库单
        /// </summary>
        等待确认生效,

        /// <summary>
        /// 已完成
        /// </summary>
        已完成
    }

    /// <summary>
    /// 销售合同/订单评审服务接口
    /// </summary>
    public interface ISalesOrderServer
    {
        /// <summary>
        /// 删除单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool DeleteBill(string billNo, out string error);

        /// <summary>
        /// 获得单据列表
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllBillInfo(string billStatus, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 获得单条信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        ServerModule.View_YX_SalesOrder GetBillInfo(string billNo);

        /// <summary>
        /// 获得零件明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetPartListInfo(string billNo);

        /// <summary>
        /// 获得评审信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回LINQ数据集</returns>
        System.Collections.Generic.IEnumerable<View_YX_SalesOrderReview> GetReviewListInfo(string billNo);

        /// <summary>
        /// 提交单据
        /// </summary>
        /// <param name="salesOrder">单据信息数据集</param>
        /// <param name="listPartInfo">零件明细信息</param>
        /// <param name="listReviewInfo">评审信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool InsertBill(ServerModule.YX_SalesOrder salesOrder, System.Data.DataTable listPartInfo, 
            System.Data.DataTable listReviewInfo, out string error);

        /// <summary>
        /// 获取新的单据号
        /// </summary>
        /// <returns>成功返回单号，失败抛出异常</returns>
        string GetNextBillID();

        /// <summary>
        /// 操作业务
        /// </summary>
        /// <param name="salesOrder">单据信息数据集</param>
        /// <param name="listPartInfo">零件明细信息</param>
        /// <param name="listReviewInfo">评审部门信息</param>
        /// <param name="deptCode">部门编码</param>
        /// <param name="opinion">部门评审意见</param>
        /// <param name="status">单据状态</param>
        /// <param name="type">操作类型（主管审核、部门评审、评审结果）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationInfo(YX_SalesOrder salesOrder, DataTable listPartInfo,DataTable listReviewInfo, string deptCode,
                                  string opinion, string status, string type, out string error);

        /// <summary>
        /// 通过单据号，部门编码获得评审意见历史信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="deptCode">部门编码</param>
        /// <returns>返回Table</returns>
        DataTable GetReviewHistory(string billNo, string deptCode);

        /// <summary>
        /// 通过物品ID和主机厂编码获得与主机厂相对应的图号名称
        /// </summary>
        /// <param name="clientCode">主机厂编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns></returns>
        DataTable GetCommunicate(string clientCode, int goodsID);
    }
}
