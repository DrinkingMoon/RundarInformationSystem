using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 采购退货单物品信息服务接口
    /// </summary>
    public interface IMaterialListRejectBill
    {
        /// <summary>
        /// 设置金额信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool SetPriceInfo(string orderFormNumber, S_MaterialListRejectBill goods, string storageID, out string error);

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext context, S_MaterialRejectBill bill,
            S_MaterialListRejectBill item);
        
        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext context, S_MaterialRejectBill bill,
            S_MaterialListRejectBill item);

        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(string orderFormNumber, S_MaterialListRejectBill goods, string storageID, out string error);

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="id">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(System.Collections.Generic.List<long> id, out string error);

        /// <summary>
        /// 删除某采购退货单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">采购退货单号</param>
        void DeleteGoods(DepotManagementDataContext context, string billNo);

        /// <summary>
        /// 删除某采购退货单下的所有物品信息
        /// </summary>
        /// <param name="billNo">采购退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(string billNo, out string error);

        /// <summary>
        /// 获取指定采购退货单的物品信息
        /// </summary>
        /// <param name="billNo">采购退货单号</param>
        /// <returns>返回获取的物品信息</returns>
        IEnumerable<View_S_MaterialListRejectBill> GetGoods(string billNo);

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="goods">物品信息</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateGoods(string orderFormNumber, S_MaterialListRejectBill goods, string storageID, out string error);

        /// <summary>
        /// 获得报废物品信息
        /// </summary>
        /// <param name="strProvider">报废物品供应商</param>
        /// <returns>报废物品供应商不为空是返回供应商对应的报废物品列表，
        /// 报废物品供应商为空串时返回所有报废物品信息</returns>
        DataTable GetScrapGoods(string strProvider);

        /// <summary>
        /// 获得对应隔离单信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>成功返回获取到的隔离单信息，失败返回null</returns>
        DataTable GetIsolationBill(string orderFormNumber, string storageID);

        /// <summary>
        /// 检查物品库存是否大于等于指定值
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="count">要比较的数量</param>
        /// <param name="provider">供应商</param>
        /// <param name="storageID">库房ID</param>
        /// <returns> >= 指定值返回true </returns>
        bool IsGoodsStockThan(int goodsID, string batchNo, decimal count, string provider, string storageID);
    }
}
