/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IWorkShopStock.cs
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
using ServerModule;
using System.Data;

namespace Service_Manufacture_WorkShop
{
    /// <summary>
    /// 车间库存操作服务接口
    /// </summary>
    public interface IWorkShopStock
    {
        /// <summary>
        /// 获得单条车间库存记录
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单条LNQ数据集</returns>
        WS_WorkShopStock GetStockSingleInfo(DepotManagementDataContext ctx, string wsCode, int goodsID, string batchNo);

        /// <summary>
        /// 查询车间物品流程账
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="wsCode">车间代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        DataTable QueryRunningAccount(int goodsID, string batchNo, string wsCode,
            DateTime startTime, DateTime endTime, out string error);

        /// <summary>
        /// 获得库存信息
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetStockInfo();

        /// <summary>
        /// 获得单条车间库存记录
        /// </summary>
        /// <param name="wsCode">车间代码</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回单条LNQ数据集</returns>
        WS_WorkShopStock GetStockSingleInfo(string wsCode, int goodsID, string batchNo);

        /// <summary>
        /// 操作明细账（并且操作库存）
        /// </summary>
        /// <param name="subsidiary">明细账对象</param>
        void OperationSubsidiary(ServerModule.WS_Subsidiary subsidiary);

        /// <summary>
        /// 操作明细账（并且操作库存）
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="subsidiary">明细账对象</param>
        void OperationSubsidiary(ServerModule.DepotManagementDataContext ctx, ServerModule.WS_Subsidiary subsidiary);

        /// <summary>
        /// 加减库存
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="workShopStock">加减库存记录</param>
        void PlusReductionStock(ServerModule.DepotManagementDataContext ctx, ServerModule.WS_WorkShopStock workShopStock);

        /// <summary>
        /// 加减库存
        /// </summary>
        /// <param name="workShopStock">加减库存记录</param>
        void PlusReductionStock(ServerModule.WS_WorkShopStock workShopStock);
    }
}
