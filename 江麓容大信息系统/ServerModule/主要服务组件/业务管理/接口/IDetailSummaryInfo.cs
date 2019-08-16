using System;
using System.Data;
using System.Linq;
using System.Collections.Generic;


namespace ServerModule
{
    /// <summary>
    /// 入库、出库名细汇总信息操作接口
    /// </summary>
    public interface IDetailSummaryInfo
    {
        /// <summary>
        /// 分库房查询入库汇总统计
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回Table</returns>
        DataTable GetInDepotSummarInfoForStorageID(DateTime beginDate, DateTime endDate, string storageID);

        /// <summary>
        /// 分库房查询领料汇总统计
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回Table</returns>
        DataTable GetGetFetchGoodsSummarInfoForStorageID(DateTime beginDate, DateTime endDate, string storageID);

        /// <summary>
        /// 获取指定日期的领料汇总信息
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的汇总信息</returns>
        IQueryable<View_S_FetchGoodsSummaryTable> GetFetchGoodsSummarInfo(DateTime beginDate, 
            DateTime endDate, SummaryMode orderMode);

        /// <summary>
        /// 获取指定日期的领料名细
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的明细信息</returns>
        IQueryable<View_S_FetchGoodsDetailBill> GetFetchGoodstDetailInfo(DateTime beginDate,
            DateTime endDate, SummaryMode orderMode);

        /// <summary>
        /// 获取指定日期的入库名细
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的明细信息</returns>
        IQueryable<View_S_InDepotDetailBill> GetInDepotDetailInfo(DateTime beginDate, DateTime endDate, SummaryMode orderMode);

        /// <summary>
        /// 获取指定日期的入库汇总信息
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="orderMode">模式</param>
        /// <returns>返回获取到的汇总信息</returns>
        IQueryable<View_S_InDepotSummaryTable> GetInDepotSummarInfo(DateTime beginDate, DateTime endDate, SummaryMode orderMode);

        /// <summary>
        /// 检查入库明细表里是否包含指定单据编号
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>包含返回true</returns>
        bool CheckInDepotDetail(string billNo);

        /// <summary>
        /// 检查领料明细表里是否包含指定单据编号
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>包含返回true</returns>
        bool CheckFetchGoodsDetail(string billNo);

        /// <summary>
        /// 获取指定日期的入库明细的TABLE
        /// </summary>
        /// <param name="lstWhere">查询条件</param>
        /// <returns>返回获取到的明细信息</returns>
        DataTable GetInDepotDetailInfoForTable(List<object> lstWhere);

        /// <summary>
        /// 获取指定日期的领料明细的TABLE
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的明细信息</returns>
        DataTable GetFetchGoodstDetailInfoForTable(DateTime beginDate, DateTime endDate, string storageID);

        /// <summary>
        /// 获取指定日期的新账套的领料明细
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <returns>返回获取到的明细信息</returns>
        DataTable GetFetchGoodstDetailInfoForTable_New(DateTime beginDate, DateTime endDate, string storageID);
    }
}
