using System;
using System.Collections.Generic;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 领料退库单物品信息服务接口
    /// </summary>
    public interface IMaterialListReturnedInTheDepot
    {
        /// <summary>
        /// Excel导入数据
        /// </summary>
        /// <param name="billNo"></param>
        /// <param name="tableInfo"></param>
        void InsertInfoExcel(string billNo, DataTable tableInfo);

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="goodsItem">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext context, S_MaterialReturnedInTheDepot bill,
            S_MaterialListReturnedInTheDepot goodsItem);

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息对象</returns>
        S_FetchGoodsDetailBill AssignDetailInfo(DepotManagementDataContext context, S_MaterialReturnedInTheDepot bill,
            S_MaterialListReturnedInTheDepot item);

        /// <summary>
        /// 对于批量生成单据明细界面的显示功能
        /// </summary>
        /// <param name="selectType">显示单据类型  (“领料”，“领料退库”)</param>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回TABLE</returns>
        DataTable GetBatchCreatList(string selectType, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 批量生成明细
        /// </summary>
        /// <param name="selectType">单据类型  (“领料”，“领料退库”)</param>
        /// <param name="billID">单据号</param>
        /// <param name="billIDGather">数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool BatchCreateList(string selectType, string billID, string billIDGather, out string error);

        /// <summary>
        /// 检查是否存在某单据物品清单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        bool IsExist(string billNo);

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(S_MaterialListReturnedInTheDepot goods, out string error);

        /// <summary>
        /// 批量添加物品
        /// </summary>
        /// <param name="lstGoods">要添加的物品信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(List<View_S_MaterialListReturnedInTheDepot> lstGoods, out string error);

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="id">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(System.Collections.Generic.List<long> id, out string error);

        /// <summary>
        /// 删除某领料退库单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">领料退库单号</param>
        void DeleteGoods(DepotManagementDataContext context, string billNo);

        /// <summary>
        /// 删除某领料退库单下的所有物品信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(string billNo, out string error);

        /// <summary>
        /// 获取指定领料退库单的物品信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>返回获取的物品信息</returns>
        IEnumerable<View_S_MaterialListReturnedInTheDepot> GetGoods(string billNo);

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateGoods(S_MaterialListReturnedInTheDepot goods, out string error);

        /// <summary>
        /// 获取领料退库单视图信息
        /// </summary>
        /// <param name="billNo">领料退库单号</param>
        /// <returns>成功返回获取领料信息, 失败返回null</returns>
        View_S_MaterialReturnedInTheDepot GetBillView(string billNo);
    }
}
