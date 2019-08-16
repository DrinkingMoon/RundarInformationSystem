/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  DetailSummaryInfo.cs
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

namespace ServerModule
{
    /// <summary>
    /// 入库、出库名细汇总信息操作类
    /// </summary>
    class DetailSummaryInfo : BasicServer, ServerModule.IDetailSummaryInfo
    {
        /// <summary>
        /// 获取指定日期的领料明细的TABLE
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的明细信息</returns>
        public DataTable GetFetchGoodstDetailInfoForTable(DateTime beginDate, DateTime endDate, string storageID)
        {
            string strSql = "select * from View_S_FetchGoodsDetailBill where 单据日期 >= '"
                    + beginDate + "' and 单据日期 <= '" + endDate + "' ";

            if (storageID != "全部库房")
            {
                strSql += " and 库房代码 = '" + storageID + "'";
            }
            else
            {
                strSql += " and 库房代码 not in (select StorageID from BASE_Storage where FinancialAccountingFlag = 0) ";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取指定日期的新账套的领料明细
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的明细信息</returns>
        public DataTable GetFetchGoodstDetailInfoForTable_New(DateTime beginDate, DateTime endDate, string storageID)
        {
            string error = "";
            DataTable result = new DataTable();

            System.Collections.Hashtable hsTable = new Hashtable();

            hsTable.Add("@StartTime", beginDate);
            hsTable.Add("@EndTime", endDate);
            hsTable.Add("@StorageID", storageID);

            result = GlobalObject.DatabaseServer.QueryInfoPro("Report_NewFDB_OutStockList", hsTable, out error);

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            return result;
        }

        /// <summary>
        /// 获取指定日期的入库明细的TABLE
        /// </summary>
        /// <param name="lstWhere">查询条件</param>
        /// <returns>返回获取到的明细信息</returns>
        public DataTable GetInDepotDetailInfoForTable(List<object> lstWhere)
        {
            if (lstWhere[0].ToString() == "新账套")
            {
                string error = "";
                DataTable result = new DataTable();

                System.Collections.Hashtable hsTable = new Hashtable();

                hsTable.Add("@StorageID", lstWhere[1].ToString());
                hsTable.Add("@Where", lstWhere[2].ToString());
                hsTable.Add("@StartTime", lstWhere[3].ToString());
                hsTable.Add("@EndTime", lstWhere[4].ToString());

                result = GlobalObject.DatabaseServer.QueryInfoPro("Report_NewFDB_InStockList", hsTable, out error);

                if (error != null && error.Trim().Length > 0)
                {
                    throw new Exception(error);
                }

                return result;
            }
            else if (lstWhere[0].ToString() == "旧账套")
            {
                string strSql = "select * from View_S_InDepotDetailBill where 1=1 ";

                if (lstWhere[1].ToString().Trim().Length > 0)
                {
                    strSql += " and 库房代码 = '" + lstWhere[1].ToString() + "'";
                }
                else
                {
                    strSql += " and 库房代码 not in (select StorageID from BASE_Storage where FinancialAccountingFlag = 0) ";
                }

                if (lstWhere[2].ToString() == "入库时间")
                {
                    strSql += " and 单据日期 >= '" + lstWhere[3].ToString() + "' and 单据日期 <= '" + lstWhere[4].ToString() + "'";
                }
                else if (lstWhere[2].ToString() == "到票时间")
                {
                    strSql += " and 到票时间 >= '" + lstWhere[3].ToString() + "' and 到票时间 <= '" + lstWhere[4].ToString() + "'";
                }

                return GlobalObject.DatabaseServer.QueryInfo(strSql);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取指定日期的入库名细
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的明细信息</returns>
        public IQueryable<View_S_InDepotDetailBill> GetInDepotDetailInfo(DateTime beginDate, DateTime endDate, SummaryMode orderMode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (orderMode == SummaryMode.日期)
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.单据日期
                       select r;
            }
            else if (orderMode == SummaryMode.名称)
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.物品名称
                       select r;
            }
            else if (orderMode == SummaryMode.供应商)
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.供应商
                       select r;
            }
            else if (orderMode == SummaryMode.库房)
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.库房名称
                       select r;
            }
            else if (orderMode == SummaryMode.材料类别)
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.材料类别
                       select r;
            }
            else if (orderMode == SummaryMode.单据编号)
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.单据编号
                       select r;
            }
            else
            {
                return from r in dataContxt.View_S_InDepotDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.图号型号
                       select r;
            }
        }

        /// <summary>
        /// 获取指定日期的入库汇总信息
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的汇总信息</returns>
        public IQueryable<View_S_InDepotSummaryTable> GetInDepotSummarInfo(DateTime beginDate,DateTime endDate, SummaryMode orderMode)
        {

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_S_InDepotSummaryTable
                         where r.日期 == endDate
                         orderby r.图号型号
                         select r;

            if (result.Count() == 0)
            {
                #region Modify by cjb on 2016.10.9, reason:由于LINQ无法调用存储过程AddInStockSummary，为保证操作在同一事务里，故用代码实现存储过程中的功能

                var varData = from a in dataContxt.S_InDepotGatherBill
                              where a.Date >= beginDate && a.Date <= endDate
                              select a;

                dataContxt.S_InDepotGatherBill.DeleteAllOnSubmit(varData);


                var varGroupBy = (from a in dataContxt.S_InDepotDetailBill
                                  where a.BillTime >= beginDate && a.BillTime <= endDate
                                  group a by a.GoodsID
                                      into s
                                      select new S_InDepotGatherBill
                                      {
                                          GoodsID = s.Select(p => p.GoodsID).First(),
                                          Amount = s.Sum(p => p.InDepotCount),
                                          UnitPrice = s.Sum(p => p.InDepotCount) == 0 ? 0 : (decimal)s.Sum(p => p.Price) / (decimal)s.Sum(p => p.InDepotCount),
                                          Price = (decimal)s.Sum(p => p.Price),
                                          Date = endDate
                                      }).OrderBy(s => s.GoodsID);


                dataContxt.S_InDepotGatherBill.InsertAllOnSubmit(varGroupBy);
                #endregion
                //dataContxt.AddInStockSummary(beginDate, endDate);
            }

            if (orderMode == SummaryMode.名称)
            {
                return from r in dataContxt.View_S_InDepotSummaryTable
                       where r.日期 == endDate
                       orderby r.物品名称
                       select r;
            }
            else if (orderMode == SummaryMode.材料类别)
            {
                return from r in dataContxt.View_S_InDepotSummaryTable
                       where r.日期 == endDate
                       orderby r.材料类别
                       select r;
            }
            else
            {
                if (result.Count() > 0)
                {
                    return result;
                }

                return from r in dataContxt.View_S_InDepotSummaryTable
                       where r.日期 == endDate
                       orderby r.图号型号
                       select r;
            }
        }

        /// <summary>
        /// 获取指定日期的领料名细
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的明细信息</returns>
        public IQueryable<View_S_FetchGoodsDetailBill> GetFetchGoodstDetailInfo(DateTime beginDate, DateTime endDate, SummaryMode orderMode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            if (orderMode == SummaryMode.日期)
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.单据日期
                       select r;
            }
            else if (orderMode == SummaryMode.图号)
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.图号型号
                       select r;
            }
            else if (orderMode == SummaryMode.名称)
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.物品名称
                       select r;
            }
            else if (orderMode == SummaryMode.库房)
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.库房名称
                       select r;
            }
            else if (orderMode == SummaryMode.材料类别)
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.材料类别
                       select r;
            }
            else if (orderMode == SummaryMode.单据编号)
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.单据编号
                       select r;
            }
            else
            {
                return from r in dataContxt.View_S_FetchGoodsDetailBill
                       where r.单据日期 >= beginDate && r.单据日期 <= endDate
                       orderby r.供应商
                       select r;
            }
        }

        /// <summary>
        /// 分库房查询入库汇总统计
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回分库房查询入库汇总统计数据</returns>
        public DataTable GetInDepotSummarInfoForStorageID(DateTime beginDate, DateTime endDate, string storageID)
        {
            string strSql = "select 图号型号,物品名称,规格, 单位,Count as 数量,UnitPrice as 实际单价," +
                " Price as 实际金额,物品类别名称 as 材料类别,GetDate() as 日期 " +
                " from (select *,Price/Count as UnitPrice from " +
                " (select GoodsID,Sum(InDepotCount) as Count,Sum(FactPrice) as Price " +
                " from S_InDepotDetailBill where BillTime >= '" + beginDate + "' and BillTime <= '" + endDate + "' " +
                " and InDepotBillID not like '%冲%' and StorageID = '" + storageID + "'" +
                " group by GoodsID) as a where Count <> 0) as a " +
                " inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 分库房查询领料汇总统计
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回分库房查询领料汇总统计数据</returns>
        public DataTable GetGetFetchGoodsSummarInfoForStorageID(DateTime beginDate,DateTime endDate,string storageID)
        {
            string strSql = "select 图号型号,物品名称,规格, 单位,Count as 数量,UnitPrice as 实际单价," +
                " Price as 实际金额,物品类别名称 as 材料类别,GetDate() as 日期 "+
                " from (select *,Price/Count as UnitPrice from " +
                " (select GoodsID,Sum(FetchCount) as Count,Sum(Price) as Price " +
                " from S_FetchGoodsDetailBill where BillTime >= '"+ beginDate +"' and BillTime <= '"+ endDate +"' "+
                " and FetchBIllID not like '%冲%' and StorageID = '"+ storageID +"'"+
                " group by GoodsID) as a where Count <> 0) as a " +
                " inner join View_F_GoodsPlanCost as b on a.GoodsID = b.序号 ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获取指定日期的领料汇总信息
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的汇总信息</returns>
        public IQueryable<View_S_FetchGoodsSummaryTable> GetFetchGoodsSummarInfo(DateTime beginDate,DateTime endDate, SummaryMode orderMode)
        {
            //DateTime beginDate;
            //DateTime endDate;

            //FinancingReportBasic.GetSummaryDate(date, out beginDate, out endDate);
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            var result = from r in dataContxt.View_S_FetchGoodsSummaryTable
                         where r.日期 == endDate
                         orderby r.图号型号
                         select r;

            if (result.Count() == 0)
            {
                #region Modify by cjb on 2016.10.9, reason:由于LINQ无法调用存储过程AddFetchGoodsSummary，为保证操作在同一事务里，故用代码实现存储过程中的功能

                var varData = from a in dataContxt.S_FetchGoodsGatherBill
                              where a.Date >= beginDate && a.Date <= endDate
                              select a;

                dataContxt.S_FetchGoodsGatherBill.DeleteAllOnSubmit(varData);


                var varGroupBy = (from a in dataContxt.S_FetchGoodsDetailBill
                                  where a.BillTime >= beginDate && a.BillTime <= endDate
                                  group a by a.GoodsID
                                      into s
                                      select new S_FetchGoodsGatherBill
                                      {
                                          GoodsID = s.Select(p => p.GoodsID).First(),
                                          Amount = s.Sum(p => p.FetchCount),
                                          UnitPrice = s.Sum(p => p.FetchCount) == 0 ? 0 : (decimal)s.Sum(p => p.Price) / (decimal)s.Sum(p => p.FetchCount),
                                          Price = (decimal)s.Sum(p => p.Price),
                                          Date = endDate
                                      }).OrderBy(s => s.GoodsID);


                dataContxt.S_FetchGoodsGatherBill.InsertAllOnSubmit(varGroupBy);
                #endregion

                //dataContxt.AddFetchGoodsSummary(beginDate, endDate);
            }

            if (orderMode == SummaryMode.名称)
            {
                return from r in dataContxt.View_S_FetchGoodsSummaryTable
                       where r.日期 == endDate
                       orderby r.物品名称
                       select r;
            }
            else if (orderMode == SummaryMode.材料类别)
            {
                return from r in dataContxt.View_S_FetchGoodsSummaryTable
                       where r.日期 == endDate
                       orderby r.材料类别
                       select r;
            }
            else
            {
                if (result.Count() > 0)
                {
                    return result;
                }

                return from r in dataContxt.View_S_FetchGoodsSummaryTable
                       where r.日期 == endDate
                       orderby r.图号型号
                       select r;
            }
        }

        /// <summary>
        /// 检查入库明细表里是否包含指定单据编号
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>包含返回true</returns>
        public bool CheckInDepotDetail(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_InDepotDetailBill
                         where r.InDepotBillID == billNo
                         select r;

            if (result.Count() > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 检查领料明细表里是否包含指定单据编号
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>包含返回true</returns>
        public bool CheckFetchGoodsDetail(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_FetchGoodsDetailBill
                         where r.FetchBIllID == billNo
                         select r;

            if (result.Count() > 0)
                return true;
            else
                return false;
        }
    }
}
