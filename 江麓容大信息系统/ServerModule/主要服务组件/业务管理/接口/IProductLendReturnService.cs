/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductLendReturnService.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.Data;
using System.Collections.Generic;

namespace ServerModule
{
    /// <summary>
    /// 物品借贷服务接口
    /// </summary>
    public interface IProductLendReturnService
    {
        /// <summary>
        /// 获得借贷流水账信息列表
        /// </summary>
        /// <returns>返回对象列表</returns>
        List<View_S_ProductLendReturnDetail> GetListDetailInfo();

        /// <summary>
        /// 获得借贷账存信息列表
        /// </summary>
        /// <returns>返回对象列表</returns>
        List<View_S_ProductLendRecord> GetListRecordInfo();

        /// <summary>
        /// 获得单条借贷记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="DebtorCode">借方代码</param>
        /// <param name="CreditCode">贷方代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回单条LNQ数据集</returns>
        S_ProductLendRecord GetStockSingleInfo(DepotManagementDataContext ctx, string DebtorCode, string CreditCode, 
            int goodsID, string batchNo, string provider);

        /// <summary>
        /// 查询车间物品借贷流水账
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="wsName">车间代码</param>
        /// <param name="storage">库房代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        DataTable QueryRunningAccount(int goodsID, string provider, string batchNo, string wsName, string storage,
            DateTime startTime, DateTime endTime, out string error);

        /// <summary>
        /// 获得借贷信息
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetRecordInfo();

        /// <summary>
        /// 获得单条借贷记录
        /// </summary>
        /// <param name="DebtorCode">借方代码</param>
        /// <param name="CreditCode">贷方代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商</param>
        /// <returns>返回单条LNQ数据集</returns>
        S_ProductLendRecord GetStockSingleInfo(string DebtorCode, string CreditCode, int goodsID, string batchNo, string provider);

        /// <summary>
        /// 操作借贷明细账
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="detail">借贷明细LINQ数据集</param>
        void OperationDetail(ServerModule.DepotManagementDataContext ctx, ServerModule.S_ProductLendReturnDetail detail);

        /// <summary>
        /// 操作明细与借贷记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="detail">明细信息</param>
        void OperationDetailRecord(ServerModule.DepotManagementDataContext ctx, ServerModule.S_ProductLendReturnDetail detail);

        /// <summary>
        /// 操作借货记录
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="record">LINQ数据集</param>
        void OperationRecord(ServerModule.DepotManagementDataContext ctx, ServerModule.S_ProductLendRecord record);
    }
}
