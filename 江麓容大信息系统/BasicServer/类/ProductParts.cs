using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ServerModule
{
    /// <summary>
    /// 产品基础零件信息服务类
    /// </summary>
    internal class ProductParts : IProductParts
    {
        #region IProductParts 成员

        /// <summary>
        /// 获取指定产品型号一次性零件信息
        /// </summary>
        /// <param name="productType">产品型号，举例：RDC15-FB</param>
        /// <returns>获取成功则返回指定产品型号一次性零件信息，否则返回null</returns>
        public IQueryable<ZPX_DisposableGoods> GetDisposableParts(string productType)
        {
            // 如果是再制造用的产品型号则修正
            productType = productType.Replace(" FX", "");

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.ZPX_DisposableGoods
                         where r.ProductType == productType
                         select r;

            if (result.Count() == 0)
            {
                return null;
            }
            else
            {
                return result;
            }
        }

        /// <summary>
        /// 检查是否是指定产品型号一次性零件
        /// </summary>
        /// <param name="productType">产品型号，举例：RDC15-FB</param>
        /// <param name="goodsCode">零件图号</param>
        /// <returns>是返回true，不是返回false</returns>
        public bool IsDisposableParts(string productType, string goodsCode)
        {
            // 如果是再制造用的产品型号则修正
            productType = productType.Replace(" FX", "");

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.ZPX_DisposableGoods
                         where r.ProductType == productType && r.GoodsCode == goodsCode
                         select r;

            if (result.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        #endregion
    }
}
