using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 领料单物品信息服务接口
    /// </summary>
    public interface IMaterialRequisitionGoodsServer : IBasicBillServer
    {
        /// <summary>
        /// 根据单据列表合计物品需要领用的数量
        /// </summary>
        /// <param name="listBillNo">单据列表</param>
        /// <returns>返回物品领用数量信息字典</returns>
        Dictionary<int, decimal> SumListBillNoInfo(List<string> listBillNo);

        /// <summary>
        /// 插入领料明细
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="listInfo">明细LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool AutoCreateGoods(DepotManagementDataContext ctx, S_MaterialRequisitionGoods listInfo, out string error);

        /// <summary>
        /// 检测报废项目
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="count">出库数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool CheckScrapGoods(string billID, int goodsID, decimal count, out string error);

        /// <summary>
        /// 添加物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(S_MaterialRequisitionGoods goods, out string error);

        /// <summary>
        /// 批量添加物品
        /// </summary>
        /// <param name="lstGoods">要添加的物品信息列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool AddGoods(List<View_S_MaterialRequisitionGoods> lstGoods, out string error);

        /// <summary>
        /// 由无线接收信息更新实际领取物品的信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateyGoodsFromWireless(S_MaterialRequisitionGoods goods, out string error);

        /// <summary>
        /// 删除物品信息
        /// </summary>
        /// <param name="id">要删除的物品ID列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(System.Collections.Generic.List<long> id, out string error);

        /// <summary>
        /// 删除某领料单下的所有物品信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">领料单号</param>
        void DeleteGoods(DepotManagementDataContext context, string billNo);

        /// <summary>
        /// 删除某领料单下的所有物品信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool DeleteGoods(string billNo, out string error);

        /// <summary>
        /// 获取指定领料单的物品信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <returns>返回获取的物品信息</returns>
        IEnumerable<View_S_MaterialRequisitionGoods> GetGoods(string billNo);

        /// <summary>
        /// 获取所有领料单对指定关联单据的指定物品实际领料数
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>获取此物品已经领料的数量</returns>
        Decimal GetGoodsAmount(string associatedBillNo, int goodsID);

        /// <summary>
        /// 获取指定领料单的物品信息
        /// </summary>
        /// <param name="billNo">领料单号</param>
        /// <param name="goodsBarCode">条形码物品表信息</param>
        /// <returns>返回获取的物品信息</returns>
        View_S_MaterialRequisitionGoods GetGoods(string billNo, S_InDepotGoodsBarCodeTable goodsBarCode);

        /// <summary>
        /// 获取总成装配物品明细
        /// </summary>
        /// <param name="assemblyName">总成名称</param>
        /// <returns>返回获取到的总成装配物品明细</returns>
        IQueryable<View_S_AssemblyGoodsBil> GetAssemblyGoodsBill(string assemblyName);

        #region 夏石友，2012-07-13 15:30

        /// <summary>
        /// 获取关联单号对应的物品明细
        /// </summary>
        /// <param name="associatedBillNo">关联单号</param>
        /// <returns>返回获取到的物品明细信息, 没有时结果信息数量为0</returns>
        IQueryable<View_S_MaterialRequisitionGoods> GetGoodsOfAssociatedBill(string associatedBillNo);

        #endregion

        /// <summary>
        /// 更新物品信息
        /// </summary>
        /// <param name="goods">物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        bool UpdateGoods(S_MaterialRequisitionGoods goods, out string error);

        /// <summary>
        /// 检查材料类别是否同属于同一个仓库
        /// </summary>
        /// <param name="nowDepotCode">当前的材料类别编码</param>
        /// <param name="befDepotCode">之前的材料类别编码</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>同属于返回True，不存在或者不属于返回False</returns>
        bool CheckDepot(string nowDepotCode, string befDepotCode, string storageID);
        
        /// <summary>
        /// 判断物品是否在BOM混装表中
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <returns>存在返回True，不存在返回False</returns>
        bool IsInJumblyBomGoods(string goodsCode ,string goodsName ,string spec);

        /// <summary>
        /// Excel导入
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="infoTable">信息表</param>
        void InsertInfoExcel(string billNo, DataTable infoTable);
    }
}
