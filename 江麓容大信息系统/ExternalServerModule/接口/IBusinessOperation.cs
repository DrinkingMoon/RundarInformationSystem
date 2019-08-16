/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IBusinessOperation.cs
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
using ServerModule;

namespace Service_Peripheral_External
{
    /// <summary>
    /// 业务明细接口
    /// </summary>
    public interface IBusinessOperation
    {
        

        /// <summary>
        /// 操作库存
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="outDetail">业务明细数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationStock(DepotManagementDataContext dataContext, Out_DetailAccount outDetail, out string error);

        /// <summary>
        /// 查询外部物品流水账
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房代码</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table</returns>
        DataTable QueryRunningAccount(int goodsID, string storageID,
            DateTime startTime, DateTime endTime, out string error);

        /// <summary>
        /// 操作库存信息
        /// </summary>
        /// <param name="stockInfo">库存数据集</param>
        /// <param name="operationType">操作类型 添加，删除, 修改</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationStockInfo(ServerModule.Out_Stock stockInfo, string operationType, out string error);

        /// <summary>
        /// 操作业务明细与库存
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="detailAccount">业务明细数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool OperationDetailAndStock(ServerModule.DepotManagementDataContext dataContext, 
            ServerModule.Out_DetailAccount detailAccount, out string error);
        
        /// <summary>
        /// 获得库房视图
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetStockInfo();
    }
}
