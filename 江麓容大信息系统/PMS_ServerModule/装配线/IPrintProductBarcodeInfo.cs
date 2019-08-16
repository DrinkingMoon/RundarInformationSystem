using System;

namespace ServerModule
{
    /// <summary>    
    /// 记录装配用打印产品条形码的信息类接口，防止装配车间与下线车间箱号相同的现象
    /// </summary>
    public interface IPrintProductBarcodeInfo
    {
        /// <summary>
        /// 检查箱号是否存在
        /// </summary>
        /// <param name="productNumber">箱号</param>
        /// <returns>存在返回true</returns>
        bool IsExists(string productNumber);

        /// <summary>
        /// 添加产品条形码打印信息
        /// </summary>
        /// <param name="productNumber">变速箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志，成功返回true，失败返回false</returns>
        bool Add(string productNumber, out string error);

        /// <summary>
        /// 检查指定的变速箱号是否允许打印
        /// </summary>
        /// <param name="productNumber">变速箱号</param>
        /// <param name="error">出错时返回错误信息</param>
        /// <returns>操作是否成功的标志，成功返回true，失败返回false</returns>
        bool AllowPrint(string productNumber, out string error);
    }
}
