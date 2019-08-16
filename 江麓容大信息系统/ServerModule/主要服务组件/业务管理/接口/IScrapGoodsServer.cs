using System;
using System.Collections.Generic;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 报废单物品信息服务接口
    /// </summary>
    public interface IScrapGoodsServer
    {
        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 保存单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="bill">要保存的单据对象</param>
        /// <param name="goodsList">要保存的报废物品清单</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool SaveGoods(string billNo, S_ScrapBill bill, List<View_S_ScrapGoods> goodsList,
            out string error);

        /// <summary>
        /// 检测是否与之前的数据一致
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="lstGoods">物品列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>True 一致 False 不一致</returns>
        bool IsInfoAccordance(string djh, List<View_S_ScrapGoods> lstGoods, out string error);

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(S_ScrapGoods goods, out string error);

        /// <summary>
        /// 批量添加物品
        /// </summary>
        /// <param name="dataContxt">LINQ 数据上下文</param>
        /// <param name="lstGoods">要添加的物品信息列表</param>
        void AddGoods(DepotManagementDataContext dataContxt, List<View_S_ScrapGoods> lstGoods);

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="id">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(System.Collections.Generic.List<long> id, out string error);

        /// <summary>
        /// 删除某报废单下的所有物品信息
        /// </summary>
        /// <param name="dataContxt">LINQ 数据上下文</param>
        /// <param name="billNo">报废单号</param>
        void DeleteGoods(DepotManagementDataContext dataContxt, string billNo);

        /// <summary>
        /// 获取指定报废单的物品信息
        /// </summary>
        /// <param name="billNo">报废单号</param>
        /// <returns>返回获取的物品信息</returns>
        IEnumerable<View_S_ScrapGoods> GetGoods(string billNo);

        /// <summary>
        /// 获取指定报废单的物品分组信息
        /// </summary>
        /// <param name="billNo">报废单号</param>
        /// <returns>返回获取的物品信息</returns>
        IEnumerable<GoodsGroup> GetGoodsByGroup(string billNo);
 
        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateGoods(S_ScrapGoods goods, out string error);

        /// <summary>
        /// 批量更新工时
        /// </summary>
        /// <param name="lstGoods">要更新的物品信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateGoods(List<View_S_ScrapGoods> lstGoods, out string error);
     }
}
