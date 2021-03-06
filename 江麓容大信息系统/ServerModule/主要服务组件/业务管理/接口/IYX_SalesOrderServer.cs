﻿using System;
using System.Data;
namespace ServerModule
{
    /// <summary>
    /// 销售合同/订单评审服务接口
    /// </summary>
    public interface IYX_SalesOrderServer
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
    }
}
