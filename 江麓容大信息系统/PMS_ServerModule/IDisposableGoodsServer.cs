using System;
namespace ServerModule
{
    /// <summary>
    /// 一次性物料清单服务接口
    /// </summary>
    public interface IDisposableGoodsServer
    {
        /// <summary>
        /// 删除一条信息
        /// </summary>
        /// <param name="disposeGoods">一次性物料数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，失败返回False</returns>
        bool DeleteData(ServerModule.ZPX_DisposableGoods disposeGoods, out string error);

        /// <summary>
        /// 获取一次性物料信息
        /// </summary>
        /// <param name="returnInfo">操作后查询返回的产品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        bool GetAllDataInfo(out System.Linq.IQueryable<ServerModule.View_ZPX_DisposableGoods> returnInfo, out string error);

        /// <summary>
        /// 通过产品型号返回物料信息
        /// </summary>
        /// <param name="productType">产品型号</param>
        /// <returns>成功返回View_ZPX_DisposableGoods数据集，否则返回NULL</returns>
        System.Linq.IQueryable<ServerModule.View_ZPX_DisposableGoods> GetDataByProductType(string productType);

        /// <summary>
        /// 添加一条信息
        /// </summary>
        /// <param name="disposeGoods">一次性物料数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，失败返回False</returns>
        bool InsertData(ServerModule.ZPX_DisposableGoods disposeGoods, out string error);

        /// <summary>
        /// 批量添加物料信息
        /// </summary>
        /// <param name="copyProductType">复制的产品型号</param>
        /// <param name="productType">复制给该产品的产品型号</param>
        /// <param name="error">错误信息</param>
        /// <returns>添加成功返回True，失败返回False</returns>
        bool InsertBatchData(string copyProductType, string productType, out string error);
    }
}
