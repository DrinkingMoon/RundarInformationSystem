using System;
namespace ServerModule
{
    /// <summary>
    /// 自制件退货单明细服务接口
    /// </summary>
    public interface IHomemadeRejectList
    {

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(S_HomemadeRejectList goods, string storageID, out string error);

        /// <summary>
        /// 删除某自制件退货单下的所有物品信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(string billNo, out string error);

        /// <summary>
        /// 删除某自制件退货单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">退货单号</param>
        void DeleteGoods(DepotManagementDataContext context, string billNo);

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="idList">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(System.Collections.Generic.List<long> idList, out string error);

        /// <summary>
        /// 获取自制件退货单视图信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        System.Data.DataTable GetBillView(string billNo);

        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateGoods(S_HomemadeRejectList goods, string storageID, out string error);
    }
}
