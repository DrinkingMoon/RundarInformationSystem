/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  StoreServerStoreServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/10/20
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/10/20 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 库存管理类
    /// </summary>
    public class StoreServer : BasicServer, IStoreServer
    {
        /// <summary>
        /// 条形码服务
        /// </summary>
        IBarCodeServer m_barCodeServer = ServerModuleFactory.GetServerModule<IBarCodeServer>();

        /// <summary>
        /// 单据类别服务
        /// </summary>
        IBillTypeServer m_billTypeServer = ServerModuleFactory.GetServerModule<IBillTypeServer>();

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID 若为0则表示所有</param>
        /// <param name="batchNo">批次号 若为"所有"则表示所有</param>
        /// <param name="provider">供应商编码 若为""则表示所有</param>
        /// <param name="storageID">库房ID 若为""则表示所有</param>
        /// <returns>返回物品库存数</returns>
        public View_S_Stock Test(int goodsID, string batchNo, string provider, string storageID)
        {
            string strSql = "select * from View_S_Stock where 1=1 ";

            if (goodsID != 0)
            {
                strSql += " and 物品ID = " + goodsID;
            }

            if (batchNo.Trim() != "所有")
            {
                strSql += " and 批次号 = '" + batchNo + "' ";
            }

            if (provider.Trim() != "")
            {
                strSql += " and 供货单位 = '" + provider + "'";
            }

            if (storageID.Trim() != "")
            {
                strSql += " and 库房代码 = '" + storageID + "'";
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            List<View_S_Stock> result = ctx.ExecuteQuery<View_S_Stock>(strSql, new object[] { }).ToList();

            if (result.Count > 0)
            {
                View_S_Stock stock = result[0];
                return stock;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 判断物品ID 的批次号是否存在
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>存在返回True，否则返回False</returns>
        public bool IsBatchNoOfGoodsExist(int goodsID, string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.S_Stock
                         where r.GoodsID == goodsID && r.BatchNo == batchNo
                         select r;

            return result.Count() > 0;
        }

        /// <summary>
        /// 检测某物品是否存在于某库房
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>存在返回True，不存在返回False</returns>
        public bool IsGoodsInStock(int goodsID, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.S_Stock
                         where r.GoodsID == goodsID && r.StorageID == storageID
                         select r;

            return result.Count() > 0;
        }

        /// <summary>
        /// 获得某一个物品的库存汇总
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>库存总数量</returns>
        decimal GetGoodsSumCount(DepotManagementDataContext ctx, int goodsID, string storageID)
        {
            var varData = from r in ctx.S_Stock
                          where r.GoodsID == goodsID && r.StorageID == storageID
                          select r.ExistCount;

            if (varData == null || varData.Count() == 0)
            {
                return 0;
            }
            else
            {
                return varData.Sum();
            }
        }

        /// <summary>
        /// 获得某一个物品的库存汇总
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>库存总数量</returns>
        public decimal GetGoodsSumCount(int goodsID, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsSumCount(ctx, goodsID, storageID);
        }
        
        /// <summary>
        /// 根据物品ID，批次号获得此物品最近的入库单价
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>最近入库单价</returns>
        decimal GetGoodsNearestWarehousingPrice(DepotManagementDataContext ctx, int goodsID, string batchNo)
        {
            decimal result = 0;

            var varData = (from a in ctx.S_InDepotDetailBill
                           where a.GoodsID == goodsID
                           && a.BatchNo == batchNo
                           select a).OrderBy(k => k.BillTime);

            if (varData.Count() != 0)
            {
                result = varData.First().FactUnitPrice;
            }
            else
            {
                var varData1 = (from a in ctx.S_InDepotDetailBill
                                where a.GoodsID == goodsID
                                select a).OrderBy(k => k.BillTime);

                if (varData1.Count() != 0)
                {
                    result = varData1.First().FactUnitPrice;
                }
            }

            return result;
        }

        /// <summary>
        /// 根据物品ID，批次号获得此物品最近的入库单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>最近入库单价</returns>
        decimal GetGoodsNearestWarehousingPrice(int goodsID, string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsNearestWarehousingPrice(ctx, goodsID, batchNo);
        }

        /// <summary>
        /// 获得库存物品信息(不包括库存为0)
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="stroageID">库房ID</param>
        /// <param name="flag">显示方式</param>
        /// <returns>返回Table</returns>
        public DataTable GetGoodsStockInfo(int goodsID, string stroageID, int flag)
        {
            string strSql = "select ";

            if (flag == 0)
            {
                strSql += "0 as 选, 图号型号, 物品名称, 规格,  批次号,  库存数量, 单位, 供货单位, 物品状态, 入库时间, 版次号, 货架, 列, 层,   备注, 材料类别名称, 物品ID";
            }
            else
            {
                strSql += "*";
            }

            strSql += " from View_S_Stock where 物品ID = " + goodsID
                + " and 库房代码 = '" + stroageID + "' and 库存数量 <> 0 ";

            if (flag == 0)
            {
                strSql += " and 物品状态 <> '隔离' ";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得物品库存信息
        /// </summary>
        /// <param name="goodsID">物品ID 若为0则表示所有</param>
        /// <param name="batchNo">批次号 若为"所有"则表示所有</param>
        /// <param name="provider">供应商编码 若为""则表示所有</param>
        /// <param name="storageID">库房ID 若为""则表示所有</param>
        /// <returns>返回物品库存信息</returns>
        public View_S_Stock GetGoodsStockInfoView(int goodsID, string batchNo, string provider, string storageID)
        {
            string strSql = "select * from View_S_Stock where 1=1 ";

            if (goodsID != 0)
            {
                strSql += " and 物品ID = " + goodsID;
            }

            if (batchNo.Trim() != "所有")
            {
                strSql += " and 批次号 = '" + batchNo + "' ";
            }

            if (provider.Trim() != "")
            {
                strSql += " and 供货单位 = '" + provider + "'";
            }

            if (storageID.Trim() != "")
            {
                strSql += " and 库房代码 = '" + storageID + "'";
            }

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            List<View_S_Stock> result = ctx.ExecuteQuery<View_S_Stock>(strSql, new object[] { }).ToList();

            return result.Count > 0 ? result[0] : null;
        }

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID 若为0则表示所有</param>
        /// <param name="batchNo">批次号 若为"所有"则表示所有</param>
        /// <param name="provider">供应商编码 若为""则表示所有</param>
        /// <param name="storageID">库房ID 若为""则表示所有</param>
        /// <returns>返回物品库存数</returns>
        public DataTable GetGoodsStockInfo(int goodsID, string batchNo, string provider, string storageID)
        {
            string strSql = "select * from S_Stock where 1=1 ";

            if (goodsID != 0)
            {
                strSql += " and GoodsID = " + goodsID;
            }

            if (batchNo.Trim() != "所有")
            {
                strSql += " and BatchNo = '" + batchNo + "' ";
            }

            if (provider.Trim() != "")
            {
                strSql += " and Provider = '" + provider + "'";
            }

            if (storageID.Trim() != "")
            {
                strSql += " and StorageID = '" + storageID + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 由隔离单更改库存状态
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="islation">隔离单单据信息</param>
        /// <param name="status">库存状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>变更成功返回True，变更失败返回False</returns>
        public bool ChangeStockStatus(DepotManagementDataContext context, S_IsolationManageBill islation, int status, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_Stock
                              where a.GoodsID == islation.GoodsID
                              && a.BatchNo == islation.BatchNo
                              && a.StorageID == islation.StorageID
                              && a.Provider == islation.Provider
                              select a;

                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_Stock lnqStock = varData.Single();

                    //当需要隔离时，则记录其历史物品状态，当解除隔离时，则还原其历史的物品状态，Modify by cjb on 2012.8.24
                    if (status == 3)
                    {
                        lnqStock.OldGoodsStatus = lnqStock.GoodsStatus;
                    }

                    if (lnqStock.GoodsStatus == 3)
                    {
                        lnqStock.GoodsStatus = Convert.ToInt32(lnqStock.OldGoodsStatus);
                    }
                    else
                    {
                        lnqStock.GoodsStatus = status;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        /// <summary>
        /// 由隔离单改变库存数
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="islation">隔离单单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>改变成功返回True，改变失败返回False</returns>
        public bool ChangeStockCount(DepotManagementDataContext context, S_IsolationManageBill islation, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_Stock
                              where a.GoodsID == islation.GoodsID
                              && a.BatchNo == islation.BatchNo
                              && a.StorageID == islation.StorageID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "记录不唯一";
                    return false;
                }
                else
                {
                    S_Stock lnqStock = varData.Single();
                    lnqStock.ExistCount = (decimal)islation.QC_HGS + (decimal)islation.QC_RBS;
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }

        /// <summary>
        /// 获得物品库存
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="provider">供应商编码</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回物品库存数量</returns>
        public string GetStockCount(int goodsID, string batchNo, string provider, string storageID)
        {
            string strSql = "select ExistCount from S_Stock where GoodsID = " + goodsID
                        + " and BatchNo = '" + batchNo + "' and Provider = '"
                        + provider + "'";

            if (storageID != null)
            {
                strSql += " and StorageID = '" + storageID + "'";
            }
            else
            {
                strSql += " and StorageID = null";
            }

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return "";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获取实际单价
        /// </summary>
        /// <param name="GoodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的实际单价</returns>
        public decimal GetFactUnitPrice(int GoodsID, string provider, string batchNo, string storageID)
        {
            StoreQueryCondition sqc = new StoreQueryCondition();

            sqc.GoodsID = GoodsID;
            sqc.BatchNo = batchNo;
            sqc.Provider = provider;
            sqc.StorageID = storageID;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            S_Stock stockInfo = GetStockInfo(dataContxt, sqc);

            if (stockInfo == null)
            {
                return 0;
            }

            return stockInfo.UnitPrice;
        }

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回所查询到的库存视图信息</returns>
        public View_S_Stock GetGoodsStockInfo(int goodsID, string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.View_S_Stock
                          where a.物品ID == goodsID
                          && a.批次号 == batchNo
                          orderby a.库存数量 descending
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return varData.First();
            }
        }

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回所查询到的库存视图信息</returns>
        public DataRow GetGoodsStockInfo(int goodsID, string batchNo, string storageID)
        {
            string strSql = "select * from View_S_Stock as a where a.物品ID = " + goodsID +
                        " and a.批次号 = '" + batchNo +
                        "' and a.库房代码 = '" + storageID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count != 1)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }

        /// <summary>
        /// 获取库存中指定物品的供应商信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>成功返回获取到的供应商信息列表, 失败返回null</returns>
        public DataTable GetProviderInfo(int goodsID)
        {
            string strSql = "select distinct Provider as 供应商编码,ShortName as 供应商简称,ProviderName as 供应商全称" +
                " from S_Stock as a inner " +
                " join Provider as b on a.Provider = b.ProviderCode where a.GoodsID = " + goodsID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取所有仓库零件信息（供点击“图型/图号”按钮时进行查询用）
        /// </summary>
        /// <param name="depotType">仓库类型，取值为：零部件、产品、其他</param>
        /// <param name="groupbyBatchNo">是否要用批次分组</param>
        /// <param name="returnPartInfo">返回获取到的零件信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取信息</returns>
        public bool GetAllStorePartInfo(string depotType, bool groupbyBatchNo, out DataTable returnPartInfo, out string error)
        {
            returnPartInfo = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@depotType", depotType);
            paramTable.Add("@needGroupbyBatchNo", groupbyBatchNo);

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("SelStockPartInfo", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnPartInfo = ds.Tables[0];

            return true;
        }

        /// <summary>
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        public S_Stock GetStockInfoOverLoad(DepotManagementDataContext context, StoreQueryCondition queryInfo)
        {
            return GetStockInfo(context, queryInfo);
        }

        /// <summary>
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        public S_Stock GetStockInfo(StoreQueryCondition queryInfo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return GetStockInfo(dataContxt, queryInfo);
        }

        /// <summary>
        /// 查找指定的库存信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="queryInfo">查询条件</param>
        /// <returns>返回查找到的库存信息</returns>
        private S_Stock GetStockInfo(DepotManagementDataContext context, StoreQueryCondition queryInfo)
        {
            IQueryable<S_Stock> result = null;

            if (queryInfo.StorageID == null)
            {
                if (queryInfo.GoodsCode == null)
                {
                    result = from r in context.S_Stock
                             where r.GoodsID == queryInfo.GoodsID
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;
                }
                else
                {
                    result = from r in context.S_Stock
                             where r.GoodsCode == queryInfo.GoodsCode
                             && r.GoodsName == queryInfo.GoodsName
                             && r.Spec == queryInfo.Spec
                             && r.Provider == queryInfo.Provider
                             && r.BatchNo == queryInfo.BatchNo
                             select r;

                }
            }
            else
            {
                if (queryInfo.GoodsCode == null)
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsID == queryInfo.GoodsID
                                 && r.Provider == queryInfo.Provider
                                 && r.BatchNo == queryInfo.BatchNo
                                 && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
                else
                {
                    if (queryInfo.Provider == null || queryInfo.Provider.Trim() == "")
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                    else
                    {
                        result = from r in context.S_Stock
                                 where r.GoodsCode == queryInfo.GoodsCode
                                       && r.GoodsName == queryInfo.GoodsName
                                       && r.Spec == queryInfo.Spec
                                       && r.Provider == queryInfo.Provider
                                       && r.BatchNo == queryInfo.BatchNo
                                       && r.StorageID == queryInfo.StorageID
                                 select r;
                    }
                }
            }

            if (result.Count() > 0)
            {
                return result.First();
            }

            return null;
        }

        /// <summary>
        /// 根据物品ID获得某物品的所有库存信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回获得的库存信息</returns>
        public View_S_Stock GetGoodsStore(int goodsID)
        {
            var result = from r in CommentParameter.DepotDataContext.View_S_Stock
                         where r.物品ID == goodsID
                         select r;

            if (result.Count() > 0)
            {
                return result.First();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得某物品的当前所有库存
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回库存数量</returns>
        public decimal GetGoodsStock(int goodsID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_Stock
                          where a.GoodsID == goodsID
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return 0;
            }
            else
            {
                return varData.Sum(k => k.ExistCount);
            }
        }

        /// <summary>
        /// 获取某货物的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStore(string goodsCode, string goodsName, string spec, string storageID)
        {
            return (from r in CommentParameter.DepotDataContext.View_S_Stock
                    where r.图号型号 == goodsCode && r.物品名称 == goodsName && r.规格 == spec
                    && r.库房代码 == storageID
                    orderby r.入库时间
                    select r);
        }

        /// <summary>
        /// 获取某货物的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreNorml(string goodsCode, string goodsName, string spec, string storageID)
        {
            IQueryable<View_S_Stock> result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.正常.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            IQueryable<View_S_Stock> result2 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.正常.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            List<View_S_Stock> result = result1.ToList();
            result.AddRange(result2);

            return result.AsQueryable();

        }

        /// <summary>
        /// 获取某货物的仅仅针对于三包外领料的物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreOnlyForSBW(string goodsCode, string goodsName, string spec, string storageID)
        {
            IQueryable<View_S_Stock> result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && (r.物品状态 == CE_StockGoodsStatus.正常.ToString() || r.物品状态 == CE_StockGoodsStatus.仅限于返修箱用.ToString())
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);
            return result1;

        }

        /// <summary>
        /// 获取某货物的仅仅针对于整台份领料的物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreOnlyForAssembly(string goodsCode, string goodsName, string spec, string storageID)
        {
            IQueryable<View_S_Stock> result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.正常.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);
            return result1;

        }

        /// <summary>
        /// 获取某货物的仅仅针对于整台份领料的物品信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreOnlyForAssembly(int goodsID, string storageID)
        {
            IQueryable<View_S_Stock> result1 = null;

            if (UniversalFunction.GetStorageName(storageID) == CE_StorageName.材料库.ToString())
            {
                result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                           where r.物品ID == goodsID
                           && r.库房代码 == storageID
                           && r.库存数量 > 0
                           orderby r.入库时间
                           select r);
            }
            else
            {
                result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                           where r.物品ID == goodsID
                           && r.物品状态 == CE_StockGoodsStatus.正常.ToString()
                           && r.库房代码 == storageID
                           && r.库存数量 > 0
                           orderby r.入库时间
                           select r);
            }

            return result1;

        }

        /// <summary>
        /// 获取某货物的仅仅针对于整台份领料的混装物品信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreOnlyForJumbly(string goodsCode, string goodsName, string spec, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            IQueryable<View_S_Stock> result1 = (from r in ctx.View_S_Stock
                                                join j in ctx.P_JumblyBomGoods
                                                on r.物品ID equals j.JumblyGoodsID
                                                where j.BomGoodsCode == goodsCode
                                                && j.IsJumbly == true
                                                && j.BomGoodsName == goodsName
                                                && j.BomSpec == spec
                                                && r.物品状态 == CE_StockGoodsStatus.正常.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            List<View_S_Stock> lstStock = new List<View_S_Stock>();

            foreach (var item in result1)
            {
                View_S_Stock lnqStock = new View_S_Stock();

                lnqStock.序号 = item.序号;
                lnqStock.物品状态 = item.物品状态;
                lnqStock.物品名称 = item.物品名称;
                lnqStock.物品ID = item.物品ID;
                lnqStock.图号型号 = item.图号型号;
                lnqStock.实际金额 = item.实际金额;
                lnqStock.实际单价 = item.实际单价;
                lnqStock.入库时间 = item.入库时间;
                lnqStock.批次号 = item.批次号;
                lnqStock.列 = item.列;
                lnqStock.库房名称 = item.库房名称;
                lnqStock.库房代码 = item.库房代码;
                lnqStock.库存数量 = item.库存数量;
                lnqStock.货架 = item.货架;
                lnqStock.规格 = item.规格;
                lnqStock.供货单位 = item.供货单位;
                lnqStock.供方批次号 = item.供方批次号;
                lnqStock.单位ID = item.单位ID;
                lnqStock.单位 = item.单位;
                lnqStock.层 = item.层;
                lnqStock.材料类别名称 = item.材料类别名称;
                lnqStock.材料类别编码 = item.材料类别编码;
                lnqStock.备注 = item.备注;
                lnqStock.版次号 = item.版次号;

                lstStock.Add(lnqStock);
            }

            return lstStock.AsQueryable();
        }

        /// <summary>
        /// 获取某货物的仅限于返修箱用的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreOnlyForRepair(string goodsCode, string goodsName, string spec, string storageID)
        {
            IQueryable<View_S_Stock> result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.仅限于返修箱用.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            IQueryable<View_S_Stock> result2 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.仅限于返修箱用.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            List<View_S_Stock> result = result1.ToList();
            result.AddRange(result2);

            return result.AsQueryable();

        }

        /// <summary>
        /// 获取某货物的仅限于售后备件的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreOnlyForAttachment(string goodsCode, string goodsName, string spec, string storageID)
        {
            IQueryable<View_S_Stock> result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.仅限于售后备件.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            return result1.AsQueryable();

        }

        /// <summary>
        /// 获取某货物的样品的信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStoreMuster(string goodsCode, string goodsName, string spec, string storageID)
        {
            IQueryable<View_S_Stock> result1 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.样品.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            IQueryable<View_S_Stock> result2 = (from r in CommentParameter.DepotDataContext.View_S_Stock
                                                where r.图号型号 == goodsCode
                                                && r.物品名称 == goodsName
                                                && r.规格 == spec
                                                && r.物品状态 == CE_StockGoodsStatus.样品.ToString()
                                                && r.库房代码 == storageID
                                                && r.库存数量 > 0
                                                orderby r.入库时间
                                                select r);

            List<View_S_Stock> result = result1.ToList();
            result.AddRange(result2);

            return result.AsQueryable();

        }

        /// <summary>
        /// 获取某货物的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的货物库存信息</returns>
        public IQueryable<View_S_Stock> GetGoodsStore(string goodsCode, string goodsName, string storageID)
        {
            return (from r in CommentParameter.DepotDataContext.View_S_Stock
                    where r.图号型号 == goodsCode && r.物品名称 == goodsName
                    && r.库房代码 == storageID
                    orderby r.入库时间
                    select r);
        }

        /// <summary>
        /// 获取某图号型号的所有库存信息
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="spec">规格</param>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取某图号型号的所有库存信息</returns>
        public bool GetSomeGoodsCodeStore(string goodsCode, string spec, out DataTable returnStock, out string error)
        {
            returnStock = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@GoodsCode", goodsCode);
            paramTable.Add("@Spec", spec);

            DataSet GoodsCodeStoreDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD =
                m_dbOperate.RunProc_CMD("SelSomeS_Stock", GoodsCodeStoreDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnStock = GoodsCodeStoreDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取非零库存信息
        /// </summary>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取库存信息</returns>
        public bool GetNoZeroStore(out IQueryable<View_S_Stock> returnStock, out string error)
        {
            error = null;
            returnStock = null;

            try
            {
                Table<View_S_Stock> table = CommentParameter.DepotDataContext.GetTable<View_S_Stock>();

                returnStock = from c in table
                              where c.库存数量 > 0
                              orderby c.图号型号, c.供货单位, c.批次号
                              select c;

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnStock, out error);
            }
        }

        /// <summary>
        /// 获取库存信息
        /// </summary>
        /// <param name="findCondition">查找条件</param>
        /// <param name="sequence">排序,True为升序,False为降序</param>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取库存信息</returns>
        public bool GetAllStore(string findCondition, bool sequence, out IQueryable<View_S_Stock> returnStock, out string error)
        {
            error = null;
            returnStock = null;

            try
            {
                Table<View_S_Stock> table = CommentParameter.DepotDataContext.GetTable<View_S_Stock>();

                if (findCondition == null)
                {
                    if (sequence)
                    {
                        returnStock = from c in table orderby c.图号型号, c.供货单位, c.批次号 select c;
                    }
                    else
                    {
                        returnStock = from c in table orderby c.图号型号, c.供货单位, c.批次号 descending select c;
                    }
                }
                else
                {
                    int findLegth = findCondition.Length;

                    if (sequence)
                    {
                        returnStock = from c in table
                                      where c.材料类别编码.Substring(0, findLegth) == findCondition
                                      orderby c.图号型号, c.供货单位, c.批次号
                                      select c;
                    }
                    else if (sequence)
                    {
                        returnStock = from c in table
                                      where c.材料类别编码.Substring(0, findLegth) == findCondition
                                      orderby c.图号型号, c.供货单位, c.批次号 descending
                                      select c;
                    }

                }

                return true;
            }
            catch (Exception err)
            {
                return SetReturnError(err, out returnStock, out error);
            }
        }

        /// <summary>
        /// 获取指定订单物品库存信息
        /// </summary>
        /// <param name="orderFormNumber">订单号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的信息</returns>
        public IQueryable<View_OrderFormGoodsStockInfo> GetOrderFormGoodsStockInfo(string orderFormNumber, string storageID)
        {
            return from r in CommentParameter.DepotDataContext.View_OrderFormGoodsStockInfo
                   where r.订单号 == orderFormNumber && r.库房代码 == storageID
                   select r;
        }

        /// <summary>
        /// 设置出错返回值
        /// </summary>
        /// <param name="err">传入的错误信息</param>
        /// <param name="returnStock">库存信息</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>返回出错信息</returns>
        bool SetReturnError(object err, out IQueryable<View_S_Stock> returnStock, out string error)
        {
            returnStock = null;
            error = err.ToString();

            return false;
        }

        /// <summary>
        /// 更新库存信息(仓库直接更新)
        /// </summary>
        /// <param name="stockInfo">库存信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回操作是否成功的标志</returns>
        public bool UpdateStore(S_Stock stockInfo, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_Stock
                             where r.ID == stockInfo.ID
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到所需的数据, 无法进行此操作");
                    return false;
                }

                // 更新条形码
                m_barCodeServer.Update(dataContxt, result.Single(), stockInfo);

                S_Stock updateInfo = result.Single();

                updateInfo.GoodsID = stockInfo.GoodsID;
                updateInfo.GoodsCode = stockInfo.GoodsCode;
                updateInfo.GoodsName = stockInfo.GoodsName;
                updateInfo.Spec = stockInfo.Spec;
                updateInfo.Provider = stockInfo.Provider;
                updateInfo.ProviderBatchNo = stockInfo.ProviderBatchNo;
                updateInfo.BatchNo = stockInfo.BatchNo;

                if (GlobalObject.BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
                {
                    updateInfo.Date = ServerTime.Time;
                }

                updateInfo.ShelfArea = stockInfo.ShelfArea;
                updateInfo.ColumnNumber = stockInfo.ColumnNumber;
                updateInfo.LayerNumber = stockInfo.LayerNumber;
                updateInfo.ExistCount = stockInfo.ExistCount;
                updateInfo.Unit = stockInfo.Unit;
                updateInfo.Remark = stockInfo.Remark + "    (" + BasicInfo.LoginName + " 修改于" + ServerTime.Time.ToString() + ")";
                updateInfo.UnitPrice = stockInfo.UnitPrice;
                updateInfo.GoodsStatus = stockInfo.GoodsStatus;
                updateInfo.InputPerson = stockInfo.InputPerson;
                updateInfo.StorageID = stockInfo.StorageID;
                updateInfo.Version = stockInfo.Version;

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        #region 2014-05-20 夏石友，完善现有的库存服务
        /// <summary>
        /// 更新库存数量(在现有库存的基础上更新)
        /// </summary>
        /// <param name="dataContxt">数据库上下文</param>
        /// <param name="stockInfoID">库存表ID</param>
        /// <param name="count">更新的物品数量</param>
        /// <returns>返回操作是否成功的标志</returns>
        void UpdateStore(DepotManagementDataContext dataContxt, int stockInfoID, decimal? count)
        {
            var result = from r in dataContxt.S_Stock
                         where r.ID == stockInfoID
                         select r;

            if (result.Count() == 0)
            {
                throw new Exception("找不到所需的数据, 无法进行此操作");
            }

            S_Stock updateInfo = result.Single();

            updateInfo.ExistCount += (decimal)count;

            if (updateInfo.ExistCount < 0)
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(updateInfo.GoodsID)
                    + "批次号：【" + updateInfo.BatchNo + "】 供应商：【" + updateInfo.Provider + "】 库存不能小于0!");
            }

            updateInfo.Price = Math.Round(updateInfo.ExistCount * Convert.ToDecimal( updateInfo.UnitPrice), 2);
            //修改库存时，不修改记录时间， 但是普通入库 批次为空的物品 需要变更库存时间，但是库存服务中传值 并未带单据类型的信息
            //这是个疑问。。。 Modify by cjb 2014.8.27
            //updateInfo.Date = ServerTime.Time;
        }
        #endregion

        /// <summary>
        /// 单独更改账龄
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="stockInfo">库存信息</param>
        /// <param name="autoSubmitToDatabase">是否直接提交数据库</param>
        public void UpdateAging(DepotManagementDataContext ctx, S_Stock stockInfo, bool autoSubmitToDatabase)
        {
            if (ctx == null)
            {
                ctx = CommentParameter.DepotDataContext;
            }

            try
            {
                var result = from r in ctx.S_Stock
                             where r.GoodsID == stockInfo.GoodsID
                             && r.BatchNo == stockInfo.BatchNo
                             && r.Provider == stockInfo.Provider
                             && r.StorageID == stockInfo.StorageID
                             select r;

                if (result.Count() == 1)
                {
                    S_Stock tempStock = result.Single();

                    tempStock.Date = ServerTime.Time;

                    if (autoSubmitToDatabase)
                    {
                        ctx.SubmitChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 入库业务库房操作
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="stockInfo">库存信息对象</param>
        /// <param name="operationType">业务类型</param>
        public void InStore(DepotManagementDataContext dataContext, S_Stock stockInfo, CE_SubsidiaryOperationType operationType)
        {
            if (stockInfo == null)
            {
                return;
            }

            stockInfo.ExistCount = Math.Abs(stockInfo.ExistCount);
            IBasicGoodsServer serverBasicGoods = SCM_Level01_ServerFactory.GetServerModule<IBasicGoodsServer>();
            View_F_GoodsPlanCost basicGoods = serverBasicGoods.GetGoodsInfoView(stockInfo.GoodsID);

            if (basicGoods == null)
            {
                string tempMsg = "【图号型号】:" + stockInfo.GoodsCode
                + " 【物品名称】:" + stockInfo.GoodsName
                + " 【规格】:" + stockInfo.Spec;

                throw new Exception("系统中不存在" + tempMsg + "的物品信息");
            }

            if (basicGoods.禁用 && stockInfo.ExistCount > 0 && 
                (operationType != CE_SubsidiaryOperationType.库房调入 && operationType != CE_SubsidiaryOperationType.领料退库))
            {
                throw new Exception(UniversalFunction.GetGoodsMessage(basicGoods.序号) + " 此物品已被禁用，不能进行入库业务");
            }

            if (UniversalFunction.GetStorageInfo_View(stockInfo.StorageID).所属科目 == "原材料"
                && (new ProviderServer()).GetProviderInfo(stockInfo.Provider).IsInternalSupplier)
            {
                throw new Exception("【内部供应商】，无法进入【原材料】库房");
            }

            StoreQueryCondition sqc = new StoreQueryCondition();

            sqc.GoodsID = stockInfo.GoodsID;
            sqc.BatchNo = stockInfo.BatchNo;
            sqc.Provider = stockInfo.Provider;
            sqc.StorageID = stockInfo.StorageID;

            S_Stock stockQueryResult = GetStockInfo(dataContext, sqc);

            if (stockQueryResult != null)
            {
                UpdateStore(dataContext, stockQueryResult.ID, stockInfo.ExistCount);
            }
            else
            {
                stockInfo.GoodsCode = basicGoods.图号型号;
                stockInfo.GoodsName = basicGoods.物品名称;
                stockInfo.Spec = basicGoods.规格;
                stockInfo.Depot = basicGoods.物品类别;
                stockInfo.Unit = basicGoods.单位;
                stockInfo.Date = ServerModule.ServerTime.Time;
                stockInfo.GoodsStatus = stockInfo.GoodsStatus;

                var varTemp = from a in dataContext.BASE_Storage
                              where a.ZeroCostFlag == true && a.StorageID == stockInfo.StorageID
                              select a;

                if (varTemp.Count() > 0 && (stockInfo.Provider == null || stockInfo.Provider.Trim().Length == 0))
                {
                    stockInfo.Provider = "SYS_JLRD";
                }

                if (stockInfo.UnitPrice == 0)
                {
                    stockInfo.UnitPrice = GetGoodsUnitPrice(dataContext, stockInfo.GoodsID, stockInfo.BatchNo, stockInfo.StorageID);

                    if (stockInfo.UnitPrice == 0 && operationType == CE_SubsidiaryOperationType.领料退库)
                    {
                        IsAllowInStore(dataContext, stockInfo.GoodsID, stockInfo.BatchNo, stockInfo.StorageID);
                    }
                }

                stockInfo.Price = stockInfo.UnitPrice * (decimal)stockInfo.ExistCount;

                // 报检入库单会给出物品版本号
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(stockInfo.Version))
                {
                    // BOM表信息服务组件
                    IBomServer serverBom = ServerModuleFactory.GetServerModule<IBomServer>();
                    DataRow dr = serverBom.GetBomInfo(dataContext, basicGoods.图号型号, basicGoods.物品名称);

                    if (dr == null)
                    {
                        stockInfo.Version = "";
                    }
                    else
                    {
                        stockInfo.Version = dr["Version"].ToString();
                    }
                }

                if (stockInfo.ExistCount < 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(dataContext, stockInfo.GoodsID)
                        + "批次号：【" + stockInfo.BatchNo + "】 供应商：【" + stockInfo.Provider + "】 库存不能小于0!");
                }

                dataContext.S_Stock.InsertOnSubmit(stockInfo);

                // 添加信息到条形码表
                m_barCodeServer.Add(dataContext, stockInfo);
            }
        }

        /// <summary>
        /// 出库业务库房操作
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="stockInfo">库存信息对象</param>
        /// <param name="operationType">业务类型</param>
        public void OutStore(DepotManagementDataContext dataContext, S_Stock stockInfo, CE_SubsidiaryOperationType operationType)
        {
            if (stockInfo == null)
            {
                return;
            }

            stockInfo.ExistCount = Math.Abs(stockInfo.ExistCount);
            IQueryable<S_Stock> result = from r in dataContext.S_Stock
                                         where r.Provider == stockInfo.Provider && r.BatchNo == stockInfo.BatchNo &&
                                               r.GoodsID == stockInfo.GoodsID && r.StorageID == stockInfo.StorageID
                                         select r;

            if (result.Count() == 0)
            {
                throw new Exception(string.Format("本仓库中找不到图号型号[{0}],物品名称[{1}],物品规格[{2}],供应商[{3}],批次号[{4}]的物品信息, 无法更新库存",
                    stockInfo.GoodsCode, stockInfo.GoodsName, stockInfo.Spec, stockInfo.Provider, stockInfo.BatchNo));
            }
            else if (result.Count() > 1)
            {
                throw new Exception(string.Format("本仓库中找到图号型号[{0}],物品名称[{1}],物品规格[{2}],供应商[{3}],批次号[{4}]的物品信息的{5}条信息, 信息不唯一无法更新库存，请与管理员联系",
                    stockInfo.GoodsCode, stockInfo.GoodsName, stockInfo.Spec, stockInfo.Provider, stockInfo.BatchNo, result.Count()));
            }
            
            S_Stock stockRecord = result.Single();

            //if (operationType != CE_SubsidiaryOperationType.自制件退货 
            //    && operationType != CE_SubsidiaryOperationType.采购退货 
            //    && operationType != CE_SubsidiaryOperationType.营销退货)
            //{
            //    if (stockRecord.GoodsStatus == 3)
            //    {
            //        throw new Exception(string.Format("本仓库中图号型号[{0}],物品名称[{1}],物品规格[{2}],供应商[{3}],批次号[{4}]的物品信息的物品状态为【隔离】, 无法出库",
            //            stockInfo.GoodsCode, stockInfo.GoodsName, stockInfo.Spec, stockInfo.Provider, stockInfo.BatchNo));
            //    }
            //}

            if (stockRecord.ExistCount < stockInfo.ExistCount)
            {
                throw new Exception(string.Format("本仓库中图号型号[{0}],物品名称[{1}],物品规格[{2}],供应商[{3}],批次号[{4}]的物品信息的存货量达不到领取的数目, 请重新确定后再进行此操作",
                    stockInfo.GoodsCode, stockInfo.GoodsName, stockInfo.Spec, stockInfo.Provider, stockInfo.BatchNo));
            }

            stockRecord.ExistCount = stockRecord.ExistCount - stockInfo.ExistCount;
            stockRecord.Price = stockRecord.ExistCount * stockRecord.UnitPrice;
        }

        /// <summary>
        /// 删除库存记录
        /// </summary>
        /// <param name="id">要删除的记录ID</param>
        /// <param name="error">返回的错误信息</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DeleteStore(int id, out string error)
        {
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_Stock
                             where r.ID == id
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到所需的数据, 无法进行此操作");
                    return false;
                }

                // 删除条形码
                m_barCodeServer.Delete(dataContxt, result.Single());

                dataContxt.S_Stock.DeleteAllOnSubmit(result);
                dataContxt.SubmitChanges();

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取库存物品状态表信息
        /// </summary>
        /// <returns>获取到的信息</returns>
        public IQueryable<S_StockStatus> GetStoreStatus()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from r in dataContxt.S_StockStatus
                   select r;
        }

        /// <summary>
        /// 查询全部货物库存
        /// </summary>
        /// <param name="table">查询到的库存信息表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllStore(out DataTable table, out string error)
        {
            table = null;
            error = null;

            DataSet PurchaseStoreDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = null;
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@DepotType", DBNull.Value);
            paramTable.Add("@Order", DBNull.Value);
            dicOperateCMD = m_dbOperate.RunProc_CMD("SelAllB_Stock", PurchaseStoreDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            table = PurchaseStoreDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 克隆库存对象
        /// </summary>
        /// <param name="stockInfo">要克隆的对象</param>
        /// <returns>克隆后的新对象</returns>
        public S_Stock Clone(S_Stock stockInfo)
        {
            S_Stock newInfo = new S_Stock();

            newInfo.ID = stockInfo.ID;
            newInfo.GoodsCode = stockInfo.GoodsCode;
            newInfo.GoodsName = stockInfo.GoodsName;
            newInfo.Spec = stockInfo.Spec;
            newInfo.Provider = stockInfo.Provider;
            newInfo.ProviderBatchNo = stockInfo.ProviderBatchNo;
            newInfo.BatchNo = stockInfo.BatchNo;
            newInfo.Date = stockInfo.Date;
            newInfo.ShelfArea = stockInfo.ShelfArea;
            newInfo.ColumnNumber = stockInfo.ColumnNumber;
            newInfo.LayerNumber = stockInfo.LayerNumber;
            newInfo.ExistCount = stockInfo.ExistCount;
            newInfo.Unit = stockInfo.Unit;
            newInfo.Remark = stockInfo.Remark;
            newInfo.StorageID = stockInfo.StorageID;

            return newInfo;
        }

        /// <summary>
        /// 获取用户管辖仓库的信息
        /// </summary>
        /// <param name="workCode">工号</param>
        /// <returns>返回获取到的仓库记录</returns>
        public IQueryable<View_S_DepotForPersonnel> GetDepotForPersonnel(string workCode)
        {
            DepotManagementDataContext Dmdc = CommentParameter.DepotDataContext;
            IQueryable<View_S_DepotForPersonnel> DfpTable = Dmdc.GetTable<View_S_DepotForPersonnel>();

            return from a in DfpTable where a.人员ID == workCode select a;
        }

        /// <summary>
        /// 获取库存平均价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回物品的平均价</returns>
        public decimal GetGoodsAveragePrice(int goodsID, string batchNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsAveragePrice(ctx, goodsID, batchNo);
        }

        /// <summary>
        /// 获取库存平均价
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <returns>返回物品的平均价</returns>
        decimal GetGoodsAveragePrice(DepotManagementDataContext ctx, int goodsID, string batchNo)
        {
            decimal result = 0;

            var varData = from a in ctx.S_Stock
                          where a.GoodsID == goodsID
                          && a.BatchNo == batchNo
                          select a;

            if (varData.Count() > 0)
            {
                result = varData.First().UnitPrice;
            }
            else
            {
                var varData1 = from a in ctx.View_S_Stock
                               where a.物品ID == goodsID
                               select a;

                if (varData1.Count() > 0)
                {
                    result = varData1.Sum(k => k.实际单价) / varData1.Count();
                }
                else
                {
                    var varData2 = from a in ctx.View_F_GoodsPlanCost
                                   where a.序号 == goodsID
                                   select a;

                    result = varData2.First().单价;
                }
            }

            return result;
        }

        /// <summary>
        /// 获得库存物品实际平均价
        /// </summary>
        /// <param name="flag">True为View_S_Stock； False为View_S_InOutSaveStock </param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回获取的库存物品平均价信息</returns>
        public DataTable GetStockAveragePrice(bool flag, string yearAndMonth)
        {
            string strTableName = "";

            if (flag)
            {
                strTableName = "View_S_Stock";
            }
            else
            {
                strTableName = "View_S_InOutSaveStock";
            }

            string strSql = "select 物品ID, 图号型号, 物品名称, 规格, " +
                " case Sum(库存数量) when 0 then 0 else Sum(实际单价*库存数量)/Sum(库存数量) end " +
                " as 物品实际平均价,sum (库存数量) as 库存总数量," +
                " 单位 from " + strTableName;

            if (yearAndMonth != "" && !flag)
            {
                strSql += " where 年月 = '" + yearAndMonth + "' ";
            }

            strSql += " group by 物品ID, 图号型号, 物品名称, 规格,单位 order by 物品ID";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 更改库存单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="price">金额</param>
        /// <param name="flag">True为View_S_Stock； False为View_S_InOutSaveStock</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        public bool ChangeAveragePrice(int goodsID, decimal price, bool flag, string yearAndMonth, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext contxt = CommentParameter.DepotDataContext;

                if (flag)
                {

                    var varData = from a in contxt.S_Stock
                                  where a.GoodsID == goodsID
                                  select a;

                    foreach (var item in varData)
                    {
                        S_Stock lnqStock = item;
                        lnqStock.UnitPrice = price;
                    }
                }
                else if (yearAndMonth != "" && !flag)
                {
                    var varData = from a in contxt.S_InOutSaveStock
                                  where a.GoodsID == goodsID
                                  && a.YearAndMonth == yearAndMonth
                                  select a;

                    foreach (var item in varData)
                    {
                        S_InOutSaveStock lnqStock = item;
                        lnqStock.UnitPrice = price;
                    }
                }

                contxt.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 操作营销售后已返修待返修库存数量
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="afterServiceStock">LINQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperationYXAfterService(DepotManagementDataContext dataContext, YX_AfterServiceStock afterServiceStock, out string error)
        {
            error = null;

            try
            {
                var varData = from a in dataContext.YX_AfterServiceStock
                              where a.GoodsID == afterServiceStock.GoodsID
                              && a.StorageID == afterServiceStock.StorageID
                              && a.RepairStatus == afterServiceStock.RepairStatus
                              select a;

                if (varData.Count() == 1)
                {
                    YX_AfterServiceStock lnqAfterService = varData.Single();

                    lnqAfterService.OperationCount = lnqAfterService.OperationCount + afterServiceStock.OperationCount;

                    if (lnqAfterService.OperationCount < 0)
                    {
                        error = "营销物品状态库存表库存小于0";
                        return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得物品单价
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回单价</returns>
        public decimal GetGoodsUnitPrice(DepotManagementDataContext ctx, int goodsID, string batchNo, string storageID)
        {
            var varStraoge = from a in ctx.BASE_Storage
                             where a.StorageID == storageID
                             select a;

            if (varStraoge.Single().ZeroCostFlag)
            {
                return 0;
            }

            decimal dcStockUnitPrice = 0;
            decimal dcAverageUnitPrice = GetGoodsAveragePrice(ctx, goodsID, batchNo);
            decimal dcNearestUnitPrice = GetGoodsNearestWarehousingPrice(ctx, goodsID, batchNo);
            decimal dcStockCount = GetGoodsSumCount(ctx, goodsID, storageID);

            if (dcAverageUnitPrice == 0 && dcStockCount == 0)
            {
                dcStockUnitPrice = dcNearestUnitPrice;
            }
            else
            {
                dcStockUnitPrice = dcAverageUnitPrice;
            }

            return dcStockUnitPrice;
        }

        /// <summary>
        /// 获得物品单价
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回单价</returns>
        public decimal GetGoodsUnitPrice(int goodsID, string batchNo, string storageID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return GetGoodsUnitPrice(ctx, goodsID, batchNo, storageID);
        }

        /// <summary>
        /// 是否允许入库
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="storageID">库房ID</param>
        void IsAllowInStore(DepotManagementDataContext ctx, int goodsID, string batchNo, string storageID)
        {
            StoreQueryCondition condition = new StoreQueryCondition();

            condition.BatchNo = batchNo;
            condition.GoodsID = goodsID;
            condition.StorageID = storageID;

            S_Stock stock = GetStockInfo(ctx, condition);

            if (stock == null)
            {
                var varData = from a in ctx.S_InDepotDetailBill
                              where a.GoodsID == condition.GoodsID
                              select a;

                if (varData.Count() == 0)
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(goodsID) + "无入库记录，并且0单价无法入库");
                }
            }
        }

        /// <summary>
        /// 操作MES系统车间在产
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        public void Operation_MES_InProduction(DepotManagementDataContext ctx, string billNo)
        {
            try
            {
                ctx.ExecuteCommand(string.Format(" exec [DepotManagement].[dbo].[MES_Operation_InProduction] '{0}' ", billNo));
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
