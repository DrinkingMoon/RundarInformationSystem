using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 产品基础零件信息服务接口
    /// </summary>
    public interface IProductParts
    {
        /// <summary>
        /// 获取指定产品型号一次性零件信息
        /// </summary>
        /// <param name="productType">产品型号，举例：RDC15-FB</param>
        /// <returns>获取成功则返回指定产品型号一次性零件信息，否则返回null</returns>
        IQueryable<ZPX_DisposableGoods> GetDisposableParts(string productType);

        /// <summary>
        /// 检查是否是指定产品型号一次性零件
        /// </summary>
        /// <param name="productType">产品型号，举例：RDC15-FB</param>
        /// <param name="goodsCode">零件图号</param>
        /// <returns>是返回true，不是返回false</returns>
        bool IsDisposableParts(string productType, string goodsCode);
    }
}
