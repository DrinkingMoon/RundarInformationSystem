/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CBOMServer.cs
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
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// 采购物料清单服务
    /// </summary>
    class CBOMServer : ICBOMServer
    {
        /// <summary>
        /// 获得采购清单
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetAllInfo()
        {
            string error = null;

            Hashtable parameters = new Hashtable();

            return GlobalObject.DatabaseServer.QueryInfoPro("CG_SEL_CBOM",
                    parameters, out error);
        }

        /// <summary>
        /// 获得零件综合信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetSynthesisInfo()
        {
            string strSql = @"select 图号型号, 物品名称, 规格, 是否为共用件, 供应商等级, 供应商,采购份额, 采购员, "+
                             " case when  b.SafeStockCount is null then 0 else b.SafeStockCount end as 安全库存,单位, "+
                             " 生产周期, 最小包装数, 最小采购数, 物品ID "+
                             " from View_B_GoodsLeastPackAndStock as a left join S_SafeStock as b on a.物品ID = b.GoodsID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        void Operation_Add(DepotManagementDataContext ctx, int goodsID, Dictionary<string, int> DicNumberOfProduct, decimal safeStockCount)
        {
            bool isInsert = false;

            var varCBOM = from a in ctx.CG_CBOM
                          where a.GoodsID == goodsID
                          select a;

            if (varCBOM.Count() != 0)
            {
                throw new Exception("此物品已存在无法添加");
            }

            foreach (KeyValuePair<string, int> item in DicNumberOfProduct)
            {
                CG_CBOM lnqCBOM = new CG_CBOM();

                lnqCBOM.Edition = item.Key;
                lnqCBOM.GoodsID = goodsID;
                lnqCBOM.Usage = Convert.ToDecimal(item.Value);

                if (lnqCBOM.Usage != 0)
                {
                    isInsert = true;
                    ctx.CG_CBOM.InsertOnSubmit(lnqCBOM);
                }
            }

            ctx.SubmitChanges();

            if (!isInsert)
            {
                throw new Exception("此物品未设置任何【基数】");
            }
            else
            {
                S_SafeStock lnqSafe = new S_SafeStock();

                lnqSafe.GoodsID = goodsID;
                lnqSafe.SafeStockCount = safeStockCount;

                ctx.S_SafeStock.InsertOnSubmit(lnqSafe);
            }

            ctx.SubmitChanges();
        }

        void Operation_Modify(DepotManagementDataContext ctx, int goodsID, Dictionary<string, int> DicNumberOfProduct, decimal safeStockCount)
        {
            var varCBOM = from a in ctx.CG_CBOM
                          where a.GoodsID == goodsID
                          select a;

            if (varCBOM.Count() == 0)
            {
                throw new Exception("此物品不存在，无法修改相关信息");
            }

            ctx.CG_CBOM.DeleteAllOnSubmit(varCBOM);
            ctx.SubmitChanges();

            var varSafe = from a in ctx.S_SafeStock
                          where a.GoodsID == goodsID
                          select a;

            ctx.S_SafeStock.DeleteAllOnSubmit(varSafe);
            ctx.SubmitChanges();

            Operation_Add(ctx, goodsID, DicNumberOfProduct, safeStockCount);
        }

        void Operation_Delete(DepotManagementDataContext ctx, int goodsID)
        {
            var varCBOM = from a in ctx.CG_CBOM
                          where a.GoodsID == goodsID
                          select a;

            ctx.CG_CBOM.DeleteAllOnSubmit(varCBOM);
            ctx.SubmitChanges();

            var varSafe = from a in ctx.S_SafeStock
                          where a.GoodsID == goodsID
                          select a;

            ctx.S_SafeStock.DeleteAllOnSubmit(varSafe);
            ctx.SubmitChanges();

            var varGoodsLeastPackAndStock = from a in ctx.B_GoodsLeastPackAndStock
                                            where a.GoodsID == goodsID
                                            select a;

            ctx.B_GoodsLeastPackAndStock.DeleteAllOnSubmit(varGoodsLeastPackAndStock);
            ctx.SubmitChanges();

            var varProcurementPlan = from a in ctx.CG_ProcurementPlan
                                     where a.GoodsID == goodsID
                                     select a;

            ctx.CG_ProcurementPlan.DeleteAllOnSubmit(varProcurementPlan);
            ctx.SubmitChanges();
        }

        /// <summary>
        /// 操作采购清单
        /// </summary>
        /// <param name="operatorMode">操作模式</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="safeStockCount">安全库存数</param>
        /// <param name="DicNumberOfProduct">基数</param>
        public void OperatorInfo(CE_OperatorMode operatorMode, int goodsID, decimal safeStockCount, Dictionary<string, int> DicNumberOfProduct)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            try
            {

                switch (operatorMode)
                {
                    case CE_OperatorMode.添加:
                        Operation_Add(ctx, goodsID, DicNumberOfProduct, safeStockCount);
                        break;
                    case CE_OperatorMode.修改:
                        Operation_Modify(ctx, goodsID, DicNumberOfProduct, safeStockCount);
                        break;
                    case CE_OperatorMode.删除:
                        Operation_Delete(ctx, goodsID);
                        break;
                    default:
                        break;
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 批量插入数据
        /// </summary>
        /// <param name="dtSource">数据源列表</param>
        public void BatchInsertCGBom(DataTable dtSource)
        {
            if (dtSource == null || dtSource.Rows.Count == 0)
            {
                throw new Exception("数据集无效");
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                foreach (DataRow dr in dtSource.Rows)
                {
                    bool isDelete = false;
                    int goodsID = UniversalFunction.GetGoodsID(dr["图号型号"].ToString(), dr["物品名称"].ToString(), dr["规格"].ToString());

                    CG_CBOM bom = new CG_CBOM();

                    bom.Edition = dr["总成型号"].ToString();

                    if (goodsID != 0)
                    {
                        bom.GoodsID = goodsID;
                    }
                    else
                    {
                        throw new Exception("图号：" + dr["图号型号"].ToString()
                           + " 名称:" + dr["物品名称"].ToString() + " 规格:" + dr["规格"].ToString() + " 不存在于系统中");
                    }

                    if (dr["基数"] == null || Convert.ToInt32(dr["基数"]) == 0)
                    {
                        throw new Exception("图号：" + dr["图号型号"].ToString()
                            + " 名称:" + dr["物品名称"].ToString() + " 规格:" + dr["规格"].ToString() + " 基数不符合要求,请重新检查");
                    }
                    else
                    {
                        bom.Usage = Convert.ToDecimal(dr["基数"]);
                    }

                    //采购BOM
                    var varData = from a in ctx.CG_CBOM
                                  where a.GoodsID == bom.GoodsID
                                  && a.Edition == bom.Edition
                                  select a;

                    ctx.CG_CBOM.DeleteAllOnSubmit(varData);
                    ctx.SubmitChanges();

                    if (bom.Usage == 0)
                    {
                        var varData1 = from a in ctx.CG_CBOM
                                       where a.GoodsID == bom.GoodsID
                                       select a;

                        if (varData1.Count() == 0)
                        {
                            isDelete = true;
                        }
                    }
                    else
                    {
                        ctx.CG_CBOM.InsertOnSubmit(bom);
                    }

                    ctx.SubmitChanges();

                    //安全库存
                    var varSaftStock = from a in ctx.S_SafeStock
                                       where a.GoodsID == goodsID
                                       select a;

                    ctx.S_SafeStock.DeleteAllOnSubmit(varSaftStock);
                    ctx.SubmitChanges();

                    if (!isDelete)
                    {
                        S_SafeStock saftStock = new S_SafeStock();
                        saftStock.GoodsID = goodsID;
                        saftStock.SafeStockCount = Convert.ToDecimal(dr["安全库存"]);

                        ctx.S_SafeStock.InsertOnSubmit(saftStock);
                    }

                    ctx.SubmitChanges();

                    //供应商配额
                    var varProviderPack = from a in ctx.B_GoodsLeastPackAndStock
                                          where a.GoodsID == goodsID
                                          select a;

                    if (isDelete)
                    {
                        ctx.B_GoodsLeastPackAndStock.DeleteAllOnSubmit(varProviderPack);
                    }

                    ctx.SubmitChanges();

                    //采购公式
                    var varProcurement = from a in ctx.CG_ProcurementPlan
                                         where a.GoodsID == goodsID
                                         select a;

                    if (isDelete)
                    {
                        ctx.CG_ProcurementPlan.DeleteAllOnSubmit(varProcurement);
                    }

                    ctx.SubmitChanges();
                }

                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
        }
    }
}
