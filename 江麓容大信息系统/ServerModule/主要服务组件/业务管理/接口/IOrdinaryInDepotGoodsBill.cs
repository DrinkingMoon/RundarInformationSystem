using System;
using System.Collections.Generic;
namespace ServerModule
{
    /// <summary>
    /// 普通入库单物品清单服务接口
    /// </summary>
    public interface IOrdinaryInDepotGoodsBill : IBasicService
    {

        /// <summary>
        /// 是否存在工装
        /// </summary>
        /// <param name="billID"></param>
        /// <returns></returns>
        bool IsExistFrock(string billID);

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext dataContext, S_OrdinaryInDepotBill bill,
            S_OrdinaryInDepotGoodsBill item);

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext dataContext, S_OrdinaryInDepotBill bill,
            S_OrdinaryInDepotGoodsBill item);

        /// <summary>
        /// 获得普通入库的新批次号
        /// </summary>
        /// <returns></returns>
        string GetNewBatchNo();

        /// <summary>
        /// 检查是否存在某单据物品信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 检查普通入库单中是否存在此物品相关信息
        /// </summary>
        /// <param name="id">物品ID</param>
        /// <returns>存在返回true, 不存在返回false</returns>
        bool IsExist(int id);

        /// <summary>
        /// 添加普通入库单物品
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddGoods(string billNo, S_OrdinaryInDepotGoodsBill goods, 
            out System.Linq.IQueryable<View_S_OrdinaryInDepotGoodsBill> returnInfo, out string error);

        /// <summary>
        /// 批量删除普通入库单物品
        /// </summary>
        /// <param name="lstID">物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteGoods(List<int> lstID, out string error);

        /// <summary>
        /// 删除指定单据的所有物品信息
        /// </summary>
        /// <param name="billNo">要删除的物品单据号</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(string billNo, out System.Linq.IQueryable<View_S_OrdinaryInDepotGoodsBill> returnInfo, out string error);
 
        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        System.Linq.IQueryable<View_S_OrdinaryInDepotGoodsBill> GetGoodsViewInfo(string billNo);

        /// <summary>
        /// 获取指定单据物品信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>返回获取到的物品信息</returns>
        System.Linq.IQueryable<S_OrdinaryInDepotGoodsBill> GetGoodsInfo(string billNo);

        /// <summary>
        /// 获取包含指定物品编号的信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获取到的物品信息</returns>
        System.Linq.IQueryable<S_OrdinaryInDepotGoodsBill> GetGoodsViewInfo(int goodsID);

        /// <summary>
        /// 更新普通入库单物品
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateGoods(S_OrdinaryInDepotGoodsBill goods, out System.Linq.IQueryable<View_S_OrdinaryInDepotGoodsBill> returnInfo, 
            out string error);

        /// <summary>
        /// 获得合计金额
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>获得合计的金额</returns>
        string GetSumJE(string billID);
    }
}
