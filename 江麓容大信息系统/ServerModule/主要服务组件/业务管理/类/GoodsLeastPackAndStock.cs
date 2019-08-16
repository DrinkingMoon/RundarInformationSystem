/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  GoodsLeastPackAndStock.cs
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using GlobalObject;


namespace ServerModule
{
    /// <summary>
    /// 供应商物品采购配额管理类（最小采购量、最小包装数）
    /// </summary>
    class GoodsLeastPackAndStock : ServerModule.IGoodsLeastPackAndStock
    {
        /// <summary>
        /// 获得供应商物品采购配置的所有信息
        /// </summary>
        /// <returns>返回获取到供应商物品采购配置的所有记录</returns>
        public DataTable GetAllInfo()
        {
            string strSql = "select * from View_B_GoodsLeastPackAndStock";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得最大序号
        /// </summary>
        /// <returns>返回获取到的最大序号</returns>
        public string GetMaxID()
        {
            string strSql = "select Max(序号) from View_B_GoodsLeastPackAndStock";

            return GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0][0].ToString();
        }

        /// <summary>
        /// 向供应商配额设置表中添加数据
        /// </summary>
        /// <param name="inLeast">供应商物品采购配额信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool AddInfo(B_GoodsLeastPackAndStock inLeast, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.B_GoodsLeastPackAndStock
                              where a.GoodsID == inLeast.GoodsID
                              && a.Provider == inLeast.Provider
                              select a;

                if (varData.Count() > 0)
                {
                    error = "数据重复";
                    return false;
                }
                else
                {
                    ctx.B_GoodsLeastPackAndStock.InsertOnSubmit(inLeast);
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改供应商配额设置表中的一条数据
        /// </summary>
        /// <param name="inLeast">供应商物品采购配额信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateInfo(B_GoodsLeastPackAndStock inLeast, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.B_GoodsLeastPackAndStock
                              where a.ID == inLeast.ID
                              select a;

                var varDataTest = from a in ctx.B_GoodsLeastPackAndStock
                                   where a.GoodsID == inLeast.GoodsID
                                   && a.Provider == inLeast.Provider
                                   && a.ID != inLeast.ID
                                   select a;

                if (varDataTest.Count() > 0)
                {
                    error = "数据重复";
                    return false;
                }
                else
                {
                    B_GoodsLeastPackAndStock lnqNew = varData.Single();

                    lnqNew.GoodsID = inLeast.GoodsID;
                    lnqNew.LeastPack = inLeast.LeastPack;
                    lnqNew.LeastStock = inLeast.LeastStock;
                    lnqNew.StockQuota = inLeast.StockQuota;
                    lnqNew.Provider = inLeast.Provider;
                    lnqNew.ProductDay = inLeast.ProductDay;
                    lnqNew.ProviderLv = inLeast.ProviderLv;
                }

                ctx.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;                
            }
        }

        /// <summary>
        /// 删除供应商配额设置表中的一条数据
        /// </summary>
        /// <param name="id">序号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteInfo(int id, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.B_GoodsLeastPackAndStock
                              where a.ID == id
                              select a;

                ctx.B_GoodsLeastPackAndStock.DeleteAllOnSubmit(varData);
                ctx.SubmitChanges();
            
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;                
            }
        }

        /// <summary>
        /// 获得BOM表零部件不在安全库存或配额中
        /// </summary>
        /// <param name="editionSql">产品类型</param>
        /// <param name="whereSql">剔除条件</param>
        /// <param name="DataTableName">表名，与Bom相比较的表</param>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetGoodsUseBomTable(string editionSql,string whereSql, string DataTableName)
        {
            string strSql = "select 父总成编码, 零部件编码,零部件名称, 规格,版本 from View_P_ProductBom " +
                            " where 序号 in (select Max(序号) from View_P_ProductBom group by 零部件编码,零部件名称,规格)" +
                            " and 父总成编码<>'' and 父总成编码 is not null and (" + editionSql + ") ";

            if (whereSql != "")
            {
                strSql += whereSql;
            }

            strSql += " and 零部件编码 not in (select 图号型号 from " + DataTableName + ") order by 零部件名称";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得安全库存或配额中存在没有使用的零件
        /// </summary>
        /// <param name="whereSql">剔除条件</param>
        /// <param name="DataTableName">表名，与Bom相比较的表</param>
        /// <returns>返回满足条件的数据集</returns>
        public DataTable GetGoodsNotUseBomTable(string whereSql, string DataTableName)
        {
            string strSql = "select distinct 图号型号,物品名称,规格 from " + DataTableName + " where 图号型号 not in" +
                            " (select 零部件编码 from View_P_ProductBom) ";

            if (whereSql != "")
            {
                strSql += whereSql;
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }
    }
}
