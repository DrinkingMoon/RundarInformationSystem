/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  SafeStockServer.cs
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
using System.Collections;
using DBOperate;

namespace ServerModule
{
    /// <summary>
    /// 安全库存类
    /// </summary>
    class SafeStockServer : BasicServer, ISafeStockServer
    {
        /// <summary>
        /// 获得某个物品的安全库存数
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回安全库存数</returns>
        public decimal GetStockCount(int goodsID)
        {
            string strSql = "select Sum(ExistCount) as Count from S_Stock where GoodsID = "+ goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows[0]["Count"].ToString() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(dt.Rows[0]["Count"]);
            }
        }

        /// <summary>
        /// 根据总成台数生成物品的安全库存数
        /// </summary>
        /// <param name="code">图号型号</param>
        /// <param name="name">名称</param>
        /// <param name="liFanCount">力帆总成数量</param>
        /// <param name="liFan320Count">力帆320总成数量</param>
        /// <param name="zhongTaiCount">众泰总成数量</param>
        /// <param name="yeMaCount">野马总成数量</param>
        /// <param name="haiMaCount">海马总成数量</param>
        /// <returns>返回安全库存数</returns>
        public decimal GetSafeStockCount(string code, string name, decimal liFanCount, decimal liFan320Count,
            decimal zhongTaiCount, decimal yeMaCount, decimal haiMaCount)
        {
            decimal dcCount = 0;

            string strSql = "select  * from (select  Counts,Edition, ParentCode," +
                " case when c.图号型号 is null then a.PartCode else c.图号型号 end as PartCode, " +
                " case when c.物品名称 is null then a.PartName else c.物品名称 end as PartName, " +
                " case when c.规格 is null then a.Spec else c.规格 end as Spec     " +
                " from View_P_ProductBomImitate as a left join P_JumblyBomGoods as b on b.BomGoodsCode = a.PartCode " +
                " and b.BomGoodsName = a.PartName and b.BomSpec = a.Spec and b.IsStock = 1 " +
                " left join View_F_GoodsPlanCost as c on b.JumblyGoodsID = c.序号) as a where PartCode = '" +
                code + "' and PartName = '" +
                name + "' and ParentCode is not null";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                switch (dt.Rows[i]["Edition"].ToString())
                {
                    case "RDC15-FB":
                        dcCount = dcCount + Convert.ToDecimal(dt.Rows[i]["Counts"]) * liFanCount;
                        break;
                    case "RDC15-FB(A)":
                        dcCount = dcCount + Convert.ToDecimal(dt.Rows[i]["Counts"]) * liFan320Count;
                        break;
                    case "RDC15-RB":
                        dcCount = dcCount + Convert.ToDecimal(dt.Rows[i]["Counts"]) * zhongTaiCount;
                        break;
                    case "RDC15-FE":
                        dcCount = dcCount + Convert.ToDecimal(dt.Rows[i]["Counts"]) * yeMaCount;
                        break;
                    case "RDC15-FF":
                        dcCount = dcCount + Convert.ToDecimal(dt.Rows[i]["Counts"]) * haiMaCount;
                        break;
                    default:
                        break;
                }
            }

            return dcCount;
        }

        /// <summary>
        /// 根据总成台数生成物品的安全库存数
        /// 夏石友，2013-06-13 添加
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="dicNumberOfProduct">总成型号与数量构成的字典</param>
        /// <returns>返回安全库存数</returns>
        public decimal GetSafeStockCount(int goodsID, Dictionary<string, int> dicNumberOfProduct)
        {
            decimal dcCount = 0;

            string strSql = "select Edition,Usage from CG_CBOM where GoodsID = " + goodsID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dicNumberOfProduct.ContainsKey(dt.Rows[i]["Edition"].ToString()))
                    dcCount += Convert.ToDecimal(dt.Rows[i]["Usage"]) * dicNumberOfProduct[dt.Rows[i]["Edition"].ToString()];
            }

            return dcCount;
        }

        /// <summary>
        /// 获得全部信息
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAllInfo()
        {
            string strSql = "select * from View_S_SafeStock";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得最大序号
        /// </summary>
        /// <returns>返回最大的序号</returns>
        public string GetMaxID()
        {
            string strSql = "select Max(序号) from View_S_SafeStock";

            return GlobalObject.DatabaseServer.QueryInfo(strSql).Rows[0][0].ToString();
        }

        /// <summary>
        /// 更新安全库存信息
        /// </summary>
        /// <param name="dtInfo">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateSafeStockInfo(DataTable dtInfo,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varAllInfo = from a in ctx.S_SafeStock
                                 select a;

                ctx.S_SafeStock.DeleteAllOnSubmit(varAllInfo);

                foreach (DataRow dr in dtInfo.Rows)
                {
                    S_SafeStock lnqSafe = new S_SafeStock();

                    lnqSafe.GoodsID = Convert.ToInt32(dr["GoodsID"]);
                    lnqSafe.SafeStockCount = Convert.ToDecimal(dr["Usage"]);
                    lnqSafe.Remark = "系统自动生成";

                    ctx.S_SafeStock.InsertOnSubmit(lnqSafe);
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
        /// 由总成自动生成安全库存表
        /// 2013-06-13 夏石友修改写法
        /// </summary>
        /// <param name="cvtList">CVT总成数据集</param>
        /// <returns>返回自动生成的数据集</returns>
        public DataTable GetSafeStockCountInfo(DataTable cvtList)
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < cvtList.Rows.Count; i++)
            {
                sb.AppendLine(string.Format(" when '{0}' then Usage * {1}",
                    cvtList.Rows[i]["总成型号"].ToString(), Convert.ToInt32(cvtList.Rows[i]["总成数量"])));
            }

            string strSql = " select GoodsID,SUM(Usage) as Usage from ( "+
                            " select case Edition "+ sb.ToString() +
                            " else Usage * 0 end as Usage,GoodsID "+
                            " from CG_CBOM ) as a  " +
                            " group by GoodsID";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 更新信息
        /// </summary>
        /// <param name="safeStock">LNQ数据集</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool UpdateInfo(S_SafeStock safeStock, int goodsID, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.S_SafeStock
                              where a.GoodsID == goodsID
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据错误";
                    return false;
                }
                else
                {
                    S_SafeStock lnqSafe = varData.Single();

                    lnqSafe.GoodsID = safeStock.GoodsID;
                    lnqSafe.Remark = safeStock.Remark;
                    lnqSafe.SafeStockCount = safeStock.SafeStockCount;
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
        /// 删除信息
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="error"></param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool DeleteInfo(int goodsID, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                var varData = from a in ctx.S_SafeStock
                              where a.GoodsID == goodsID
                              select a;

                ctx.S_SafeStock.DeleteAllOnSubmit(varData);
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
        /// 添加信息
        /// </summary>
        /// <param name="safeStock">LNQ数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AddInfo(S_SafeStock safeStock,out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            try
            {
                S_SafeStock lnqSafe = new S_SafeStock();

                lnqSafe.GoodsID = safeStock.GoodsID;
                lnqSafe.Remark = safeStock.Remark;
                lnqSafe.SafeStockCount = safeStock.SafeStockCount;

                ctx.S_SafeStock.InsertOnSubmit(lnqSafe);
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
        /// 操作数据库
        /// </summary>
        /// <param name="safeList">新的安全库存的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新数据库成功返回True，更新失败返回False</returns>
        public bool OperationInfo(DataTable safeList, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                #region 删除数据

                var varData = from a in dataContext.S_SafeStock
                              select a;

                dataContext.S_SafeStock.DeleteAllOnSubmit(varData);

                #endregion

                #region 添加数据

                for (int i = 0; i < safeList.Rows.Count; i++)
                {
                    S_SafeStock lnqSafe = new S_SafeStock();

                    lnqSafe.GoodsID = Convert.ToInt32(safeList.Rows[i]["GoodsID"]);
                    lnqSafe.SafeStockCount = Convert.ToDecimal(safeList.Rows[i]["Usage"]);

                    dataContext.S_SafeStock.InsertOnSubmit(lnqSafe);
                }

                #endregion

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
