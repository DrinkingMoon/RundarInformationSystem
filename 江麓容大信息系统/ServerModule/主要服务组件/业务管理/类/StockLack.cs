/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  StockLack.cs
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
    /// 库房报缺类
    /// </summary>
    class StockLack:BasicServer, IStockLack
    {
        /// <summary>
        /// 清空临时表
        /// </summary>
        public void ClearTempTable()
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            var varData = from a in dataContext.S_ForWantingReportTemp
                          select a;

            dataContext.S_ForWantingReportTemp.DeleteAllOnSubmit(varData);
            dataContext.SubmitChanges();
        }

        /// <summary>
        /// 插入临时表
        /// </summary>
        /// <param name="code">产品型号</param>
        /// <param name="count">数量</param>
        public void AddTempTable(string code, decimal count)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;
            S_ForWantingReportTemp lnqFor = new S_ForWantingReportTemp();

            IBasicGoodsServer serverGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

            string error = "";

            View_F_GoodsPlanCost tempLnq = serverGoods.GetGoodsInfo(code, "", out error);

            lnqFor.GoodsID = tempLnq == null ? 0 : tempLnq.序号;
            lnqFor.Count = count;
            dataContext.S_ForWantingReportTemp.InsertOnSubmit(lnqFor);

            dataContext.SubmitChanges();
        }

        /// <summary>
        /// 获得BOM表信息
        /// </summary>
        /// <param name="cvtType">产品类型</param>
        /// <returns>返回table</returns>
        public DataTable GetBomTable(string cvtType)
        {
            string strSql = "select ParentID,b.物品名称 as GoodsName, GoodsID " +
                " from (select case when ParentID is null then 0 else ParentID end as  ParentID, " +
                " GoodsID from dbo.fun_get_BomTree('" + cvtType + "')) as a " +
                " left join View_F_GoodsPlanCost as b on a.GoodsID = b.序号";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 报缺查询
        /// </summary>
        /// <param name="strat">开始日期字符串</param>
        /// <param name="end">结束日期字符串</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="productName">产品名称</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回查询到的信息</returns>
        public DataTable ReportWanting(string strat, string end, string storageID,
            string productName, out string error)
        {
            error = null;
            strat = strat + " 00:00:00";
            end = end + " 23:59:59";

            Hashtable paramTable = new Hashtable();

            if (productName == "ReportWanting")
            {
                paramTable.Add("@StartDate", strat);
                paramTable.Add("@EndDate", end);
                paramTable.Add("@StorageID", storageID);
            }
            else if (productName == "ReportWantingElse")
            {
                paramTable.Add("@StorageID", storageID);
            }
            else if (productName == "ReportWantingSingle")
            {
                paramTable.Add("@StorageID", storageID);
                paramTable.Add("@MathCount", Convert.ToDecimal(strat.Substring(0, strat.Length - 9)));
            }
            else if (productName == "ReportWantingCustom")
            {
                paramTable.Add("@ListID", Convert.ToInt32(end.Substring(0, end.Length - 9)));
                paramTable.Add("@StorageID", storageID);
                paramTable.Add("@MathCount", Convert.ToDecimal(strat.Substring(0, strat.Length - 9)));
            }

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD(productName, ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return null;
            }

            return ds.Tables[0];
        }

        /// <summary>
        /// 获得单一的BOM清单信息
        /// </summary>
        /// <param name="listGoods">BOM物品信息列表</param>
        /// <returns>返回Table</returns>
        public DataTable SetSingleBom(List<string> listGoods)
        {
            DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

            ClearTempTable();

            foreach (string goodsInfo in listGoods)
            {
                int goodsID = Convert.ToInt32(goodsInfo.Substring(0, goodsInfo.IndexOf("-")));
                int parentGoodsID = Convert.ToInt32(goodsInfo.Substring(goodsInfo.IndexOf("-") + 1));

                var varData = from a in dataContext.BASE_BomStruct
                              where a.GoodsID == goodsID
                              && a.ParentID == parentGoodsID
                              select a;

                if (varData.Count() == 1)
                {
                    S_ForWantingReportTemp lnqFor = new S_ForWantingReportTemp();

                    lnqFor.GoodsID = goodsID;
                    lnqFor.Count = varData.Single().Usage;

                    dataContext.S_ForWantingReportTemp.InsertOnSubmit(lnqFor);
                }
            }

            dataContext.SubmitChanges();

            return null;
        }

        /// <summary>
        /// 自定义模板操作模板
        /// </summary>
        /// <param name="mode">操作模式</param>
        /// <param name="main">LNQ信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True, 失败返回False</returns>
        public bool OperationMain(CE_OperatorMode mode, S_StockLackCustomTemplates main, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_StockLackCustomTemplates
                              select a;

                S_StockLackCustomTemplates tempLnq = new S_StockLackCustomTemplates();

                switch (mode)
                {
                    case CE_OperatorMode.添加:
                        tempLnq.TemplatesName = main.TemplatesName;
                        ctx.S_StockLackCustomTemplates.InsertOnSubmit(tempLnq);
                        break;
                    case CE_OperatorMode.修改:

                        varData = from a in ctx.S_StockLackCustomTemplates
                                  where a.ID == main.ID
                                  select a;

                        if (varData.Count() != 1)
                        {
                            throw new Exception("数据不唯一");
                        }
                        else
                        {
                            tempLnq = varData.Single();

                            tempLnq.TemplatesName = main.TemplatesName;
                        }

                        break;
                    case CE_OperatorMode.删除:
                        varData = from a in ctx.S_StockLackCustomTemplates
                                  where a.ID == main.ID
                                  select a;

                        ctx.S_StockLackCustomTemplates.DeleteAllOnSubmit(varData);
                        break;
                    default:
                        break;
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
        /// 操作自定义模板明细
        /// </summary>
        /// <param name="mode">操作模式</param>
        /// <param name="list">LNQ信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False </returns>
        public bool OperationList(CE_OperatorMode mode, S_StockLackCustomTemplatesList list, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_StockLackCustomTemplatesList
                              select a;

                S_StockLackCustomTemplatesList tempLnq = new S_StockLackCustomTemplatesList();

                switch (mode)
                {
                    case CE_OperatorMode.添加:

                        tempLnq.Counts = list.Counts;
                        tempLnq.GoodsID = list.GoodsID;
                        tempLnq.ListID = list.ListID;

                        ctx.S_StockLackCustomTemplatesList.InsertOnSubmit(tempLnq);
                        break;
                    case CE_OperatorMode.修改:

                        varData = from a in ctx.S_StockLackCustomTemplatesList
                                  where a.ID == list.ID
                                  select a;

                        if (varData.Count() != 1)
                        {
                            throw new Exception("数据不唯一");
                        }
                        else
                        {
                            tempLnq = varData.Single();

                            tempLnq.Counts = list.Counts;
                            tempLnq.GoodsID = list.GoodsID;
                        }

                        break;
                    case CE_OperatorMode.删除:
                        varData = from a in ctx.S_StockLackCustomTemplatesList
                                  where a.ID == list.ID
                                  select a;

                        ctx.S_StockLackCustomTemplatesList.DeleteAllOnSubmit(varData);
                        break;
                    default:
                        break;
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
        /// 获得自定义模板信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetCustomTemplatesMain()
        {
            string strSql = "select TemplatesName as 模板名称, ID from S_StockLackCustomTemplates";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得自定义模板明细信息
        /// </summary>
        /// <param name="listID">模板ID</param>
        /// <returns>返回Table</returns>
        public DataTable GetCustomTemplatesList(int listID)
        {
            string strSql = "select b.图号型号, b.物品名称, b.规格, a.Counts as 基数," +
                " a.GoodsID as 物品ID, a.ID as ID, a.ListID as 模板ID from S_StockLackCustomTemplatesList as a " +
                " inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 where a.ListID = " + listID;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }
    }
}
