using System;

namespace ServerModule
{
    /// <summary>
    /// 返修零件信息服务接口（打条形码用）
    /// </summary>
    public interface IReparativePartInfoServer
    {
        /// <summary>
        /// 获得指定产品图号的返修零件信息(打返修条形码用)
        /// </summary>
        /// <param name="productCode">产品图号</param>
        /// <returns>返回获取到的信息</returns>
        System.Linq.IQueryable<View_ZPX_ReparativeBarcode> GetData(string productCode);

        /// <summary>
        /// 更新返修零件信息
        /// </summary>
        /// <param name="lstInfo">要添加的条形码信息</param>
        /// <param name="error">出错时输出错误信息</param>
        /// <returns>返回操作是否成功的标志</returns>
        /// <remarks>打印条形码时如果找不到此物品的条形码时直接生成条形码用</remarks>
        bool Update(System.Collections.Generic.List<StateData<View_ZPX_ReparativeBarcode>> lstInfo, out string error);
    }
}
