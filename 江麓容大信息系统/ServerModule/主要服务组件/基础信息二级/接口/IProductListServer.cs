/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  IProductListServer.cs
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
    /// 用于营销产品类管理类接口
    /// </summary>
    public interface IProductListServer
    {

        /// <summary>
        /// 获得产品信息
        /// </summary>
        /// <returns>返回产品信息</returns>
        DataTable GetProductInfo();

        /// <summary>
        /// 判断此物品是否存在于成品库中
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>存在返回True,不存在返回False</returns>
        bool IsInProductStock(int goodsID);

        /// <summary>
        /// 获得车型
        /// </summary>
        /// <returns>返回车型记录列表</returns>
        DataTable GetMotorcycleType();

        /// <summary>
        /// 根据车型获得ID
        /// </summary>
        /// <param name="carModel">车型</param>
        /// <returns>返回车型ID</returns>
        int GetMotorcycleType(string carModel);

        /// <summary>
        /// 根据车型ID获得车型
        /// </summary>
        /// <param name="carModelID">车型ID</param>
        /// <returns>返回车型</returns>
        string GetMotorcycleInfo(int carModelID);

        /// <summary>
        /// 获得产品的GoodsID且同时判断此物品ID是否属于产品
        /// </summary>
        /// <param name="productCode">产品型号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="flagTCU">是否过滤TCU， True： 是，False： 否</param>
        /// <returns>返回物品ID，若为0则不属于产品</returns>
        int GetProductGoodsID(string productCode, int goodsID, bool flagTCU);

        /// <summary>
        /// 获得某一个产品信息
        /// </summary>
        /// <returns>返回一个产品信息</returns>
        DataRow GetGoodsPlanID(string GoodsCode);

        /// <summary>
        /// 获取与营销有关的所有产品信息
        /// </summary>
        /// <param name="error">出错时输出的错误信息</param>
        /// <returns>成功返回获取到的产品信息，失败返回null</returns>
        DataTable GetAllProductList(out string error);
    }
}
