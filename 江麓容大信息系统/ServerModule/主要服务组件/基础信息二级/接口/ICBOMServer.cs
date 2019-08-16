/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  ICBOMServer.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
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
    /// 采购物料清单服务接口
    /// </summary>
    public interface ICBOMServer
    {
        /// <summary>
        /// 获得零件综合信息
        /// </summary>
        /// <returns>返回Table</returns>
        DataTable GetSynthesisInfo();

        /// <summary>
        /// 获得采购清单
        /// </summary>
        /// <returns>返回Table</returns>
        System.Data.DataTable GetAllInfo();

        /// <summary>
        /// 操作采购清单
        /// </summary>
        /// <param name="operatorMode">操作模式</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="safeStockCount">安全库存数</param>
        /// <param name="DicNumberOfProduct">基数</param>
        void OperatorInfo(GlobalObject.CE_OperatorMode operatorMode, int goodsID, 
            decimal safeStockCount, Dictionary<string, int> DicNumberOfProduct);

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dtSource">数据源列表</param>
        void BatchInsertCGBom(DataTable dtSource);
    }
}
