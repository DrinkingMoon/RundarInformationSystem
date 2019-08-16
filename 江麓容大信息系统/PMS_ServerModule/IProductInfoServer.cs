/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductInfoServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace ServerModule
{
    /// <summary>
    /// 产品信息管理类接口
    /// </summary>
    public interface IProductInfoServer
    {
        /// <summary>
        /// 获取产品信息信息
        /// </summary>
        /// <param name="returnProductInfo">操作后查询返回的产品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        bool GetAllProductInfo(out IQueryable<View_P_ProductInfo> returnProductInfo, out string error);

        /// <summary>
        /// 获取所有产品类型编码
        /// </summary>
        /// <param name="procuctTypes">获取到的产品类型编码</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回true</returns>
        bool GetAllProductType(out string[] procuctTypes, out string error);

        /// <summary>
        /// 获取指定产品类型的产品信息
        /// </summary>
        /// <param name="productType">产品类型</param>
        /// <returns>返回获取到的信息</returns>
        View_P_ProductInfo GetProductInfo(string productType);

        /// <summary>
        /// 获取产品信息信息
        /// </summary>
        /// <param name="returnProductInfo">操作后查询返回的产品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>获取成功返回True，获取失败返回False</returns>
        bool GetRemovedTCU(out IQueryable<View_P_ProductInfo> returnProductInfo, out string error);

        /// <summary>
        /// 添加产品信息
        /// </summary>
        /// <param name="productInfo">产品信息</param>
        /// <param name="returnProductInfo">返回重新查询到的产品信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool AddProductInfo(P_ProductInfo productInfo, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error);

        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="productInfo">更新后的产品信息</param>
        /// <param name="returnProductInfo">返回重新查询到的产品信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool UpdateProductInfo(P_ProductInfo productInfo, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error);

        /// <summary>
        /// 删除产品信息
        /// </summary>
        /// <param name="id">要删除的产品信息ID</param>
        /// <param name="returnProductInfo">返回重新查询到的产品信息</param>
        /// <param name="error">出错时返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        bool DeleteProductInfo(int id, out IQueryable<View_P_ProductInfo> returnProductInfo, out string error);
    }
}
