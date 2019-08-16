/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  GatherBillAndDetailBillServer.cs
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
using System.Text;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Collections;
using DBOperate;
using ServerModule;

namespace Service_Economic_Financial
{
    /// <summary>
    /// 入库汇总/明细表管理类
    /// </summary>
    class GatherBillAndDetailBillServer : IGatherBillAndDetailBillServer
    {
        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

        /// <summary>
        /// 添加初始化收发存汇总表初始的上月结存记录
        /// </summary>
        /// <param name="goodsCode">图号型号</param>
        /// <param name="goodsName">物品名称</param>
        /// <param name="spec">规格</param>
        /// <param name="counts">数量</param>
        /// <param name="materialType">领料类型</param>
        /// <param name="dateTimes">日期</param>
        /// <param name="returnBill">返回table 数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddReceiveSendSaveBalanceTable(string goodsCode, string goodsName, string spec, 
            int counts, string dateTimes, string materialType, out DataTable returnBill, out string error)
        {
            returnBill = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@GoodsCode", goodsCode);
            paramTable.Add("@Spec", spec);
            paramTable.Add("@GoodsName", goodsName);
            paramTable.Add("@Counts", counts);
            paramTable.Add("@DateTimes", dateTimes);
            paramTable.Add("@MaterialType", materialType);

            DataSet ReceiveSendSaveGatherBillDetailBillDataSet = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("AddS_ReceiveSendSaveBalanceTable",
                ReceiveSendSaveGatherBillDetailBillDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnBill = ReceiveSendSaveGatherBillDetailBillDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 删除(初始化收发存汇总表)某一初始记录
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="returnBill">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteReceiveSendSaveBalanceTable(string id, out DataTable returnBill, out string error)
        {
            returnBill = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ID", id);

            DataSet ReceiveSendSaveGatherBillDetailBillDataSet = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("DelS_ReceiveSendSaveBalanceTable", 
                ReceiveSendSaveGatherBillDetailBillDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnBill = ReceiveSendSaveGatherBillDetailBillDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 修改初始化收发存汇总表初始上月结存记录
        /// </summary>
        /// <param name="id">ID序号</param>
        /// <param name="counts">数量</param>
        /// <param name="materialType">领料类型</param>
        /// <param name="returnBill">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateReceiveSendSaveBalanceTable(string id, int counts, string materialType, 
            out DataTable returnBill, out string error)
        {
            returnBill = null;
            error = null;

            Hashtable paramTable = new Hashtable();

            paramTable.Add("@ID", id);
            paramTable.Add("@Counts", counts);
            paramTable.Add("@MaterialType", materialType);

            DataSet ReceiveSendSaveGatherBillDetailBillDataSet = new DataSet();

            Dictionary<OperateCMD, object> dicOperateCMD = 
                m_dbOperate.RunProc_CMD("UpdateS_ReceiveSendSaveBalanceTable", 
                ReceiveSendSaveGatherBillDetailBillDataSet, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                return false;
            }

            returnBill = ReceiveSendSaveGatherBillDetailBillDataSet.Tables[0];
            return true;
        }

        /// <summary>
        /// 获取指定日期的新账套的收发存汇总表
        /// </summary>
        /// <param name="beginDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID 如果查询全部库房则为null</param>
        /// <returns>返回获取到的收发存汇总表</returns>
        public DataTable GetAllGather_New(DateTime beginDate, DateTime endDate, string storageID)
        {
            string error = "";
            DataTable result = new DataTable();

            System.Collections.Hashtable hsTable = new Hashtable();

            if (storageID == null)
            {
                hsTable.Add("@StartTime", beginDate);
                hsTable.Add("@EndTime", endDate);

                result = GlobalObject.DatabaseServer.QueryInfoPro("Report_NewFDB_InOutStockCollect", hsTable, out error);
            }
            else
            {
                hsTable.Add("@StartTime", beginDate);
                hsTable.Add("@EndTime", endDate);
                hsTable.Add("@StorageID", storageID);

                result = GlobalObject.DatabaseServer.QueryInfoPro("Report_NewFDB_InOutStockCollect_SingleStorage", hsTable, out error);
            }

            if (error != null && error.Trim().Length > 0)
            {
                throw new Exception(error);
            }

            return result;
        }

        /// <summary>
        /// 台帐
        /// </summary>
        /// <param name="productName">查询方式</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="batchNo">批次号</param>
        /// <param name="showTable">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool GetAllGather(string productName, int goodsID,
            DateTime startDate, DateTime endDate, string storageID,string batchNo,
            out DataTable showTable, out string error)
        {
            error = null;
            showTable = null;

            try
            {
                Hashtable paramTable = new Hashtable();

                if (productName == "Estrade")
                {
                    paramTable.Add("@GoodsID", goodsID);
                    paramTable.Add("@StartDate", startDate);
                    paramTable.Add("@EndDate", endDate);                    
                }
                else if (productName == "EstradeForStorage")
                {
                    paramTable.Add("@GoodsID", goodsID);
                    paramTable.Add("@StartDate", startDate);
                    paramTable.Add("@EndDate", endDate);
                    paramTable.Add("@StorageID", storageID);
                }
                //else if (productName == "GoodsListGather")
                //{
                //    paramTable.Add("@StartDate", startDate);
                //    paramTable.Add("@EndDate", endDate);
                //}
                //else if (productName == "GoodsListGatherStorageID")
                //{
                //    paramTable.Add("@StorageID", storageID);
                //    paramTable.Add("@StartDate", startDate);
                //    paramTable.Add("@EndDate", endDate);
                //}
                else if (productName == "EstradeBatchNo")
                {
                    paramTable.Add("@GoodsID", goodsID);
                    paramTable.Add("@BatchNo", batchNo);
                    paramTable.Add("@StartDate", startDate);
                    paramTable.Add("@EndDate", endDate);
                }
                else if (productName == "EstradeBatchNoForStorage")
                {
                    paramTable.Add("@GoodsID", goodsID);
                    paramTable.Add("@BatchNo", batchNo);
                    paramTable.Add("@StartDate", startDate);
                    paramTable.Add("@EndDate", endDate);
                    paramTable.Add("@StorageID", storageID);
                }

                DataSet ds = new DataSet();
                Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD(productName, ds, paramTable);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                    return false;
                }

                showTable = ds.Tables[0];

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获得旧库存批次及数量
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="time">时间</param>
        /// <returns>返回旧库存批次及数量的数据集</returns>
        public DataTable GetOldStock(int goodsID,string time)
        {
            string strSql = " select a.BatchNo as 批次号,Provider as 供应商,ICount as 数量 " +
                            " from (select GoodsID,BatchNo,Sum(ICount) as ICount,Sum(Price) as Price" +
                            " from (select GoodsID,BatchNo,ExistCount as ICount,(UnitPrice*ExistCount) as Price" +
                            " from S_Stock union all" +
                            " select GoodsID,BatchNo,-InDepotCount as ICount,-Price as Price " +
                            " from S_InDepotDetailBill where BillTime >= '" + time + "'" +
                            " union  all select GoodsID,BatchNo,FetchCount as ICount,Price " +
                            " from S_FetchGoodsDetailBill where  BillTime >= '" + time + "') " +
                            " as a where GoodsID = " + goodsID + " group by GoodsID,BatchNo" +
                            " ) as a inner join S_Stock as b on a.GoodsID = b.GoodsID and a.BatchNo = b.BatchNo" +
                            " where  ICount <> 0";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 检查是否有数据存在
        /// </summary>
        /// <param name="yearAndMonth">查询年月字符串 格式为“YYYYMM”</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool IsDataIn(string yearAndMonth, string storageID,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_MarketingInOutSaveStock
                              where a.Ny == yearAndMonth
                              && a.StorageID == storageID
                              select a;

                if (varData.Count() != 0)
                {
                    return false;
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
        /// 保存进销存表
        /// </summary>
        /// <param name="showTable">需要存储的数据表</param>
        /// <param name="yearAndMonth">年月 格式为“YYYYMM”</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool SaveMarktingGather(DataTable showTable, string yearAndMonth, string storageID,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_MarketingInOutSaveStock
                              where a.Ny == yearAndMonth
                              && a.StorageID == storageID
                              select a;

                ctx.S_MarketingInOutSaveStock.DeleteAllOnSubmit(varData);

                var varGoods = from a in ctx.S_Stock
                               where a.StorageID == storageID
                               select a;
                foreach (var itemGoods in varGoods)
                {
                    var varYWLX = (from a in ctx.S_MarketingInOutSaveStock
                                   where a.GoodsID == itemGoods.GoodsID
                                   select a.YWLX).Distinct();

                    foreach (var itemYwlx in varYWLX)
                    {
                        decimal dcWaitForRepair = 0;
                        decimal dcAlreadyRepair = 0;
                        decimal dcProduceDept = 0;
                        decimal dcAlreadyScrap = 0;
                        decimal dcOutStock = 0;
                        decimal dcRequisitionNewProduct = 0;

                        for (int i = 0; i < showTable.Rows.Count; i++)
                        {
                            if (Convert.ToInt32( showTable.Rows[i]["物品ID"]) == itemGoods.GoodsID)
                            {
                                if (showTable.Rows[i]["名称"].ToString() == "待返修")
                                {
                                    dcWaitForRepair = Convert.ToDecimal(showTable.Rows[i][itemYwlx]);
                                }
                                if (showTable.Rows[i]["名称"].ToString() == "已返修")
                                {
                                    dcAlreadyRepair = Convert.ToDecimal(showTable.Rows[i][itemYwlx]);
                                }
                                if (showTable.Rows[i]["名称"].ToString() == "制造部")
                                {
                                    dcProduceDept = Convert.ToDecimal(showTable.Rows[i][itemYwlx]);
                                }
                                if (showTable.Rows[i]["名称"].ToString() == "已报废")
                                {
                                    dcAlreadyScrap = Convert.ToDecimal(showTable.Rows[i][itemYwlx]);
                                }
                                if (showTable.Rows[i]["名称"].ToString() == "外存数量")
                                {
                                    dcOutStock = Convert.ToDecimal(showTable.Rows[i][itemYwlx]);
                                }
                                if (showTable.Rows[i]["名称"].ToString() == "领用新总成数")
                                {
                                    dcRequisitionNewProduct = Convert.ToDecimal(showTable.Rows[i][itemYwlx]);
                                }
                            }
                        }

                        S_MarketingInOutSaveStock lnqSaveStock = new S_MarketingInOutSaveStock();

                        lnqSaveStock.AlreadyRepair = dcAlreadyRepair;
                        lnqSaveStock.AlreadyScrap = dcAlreadyScrap;
                        lnqSaveStock.GoodsID = itemGoods.GoodsID;
                        lnqSaveStock.Ny = yearAndMonth;
                        lnqSaveStock.OutStock = dcOutStock;
                        lnqSaveStock.ProduceDept = dcProduceDept;
                        lnqSaveStock.RequisitionNewProduct = dcRequisitionNewProduct;
                        lnqSaveStock.StorageID = storageID;
                        lnqSaveStock.WaitForRepair = dcWaitForRepair;
                        lnqSaveStock.YWLX = itemYwlx;

                        ctx.S_MarketingInOutSaveStock.InsertOnSubmit(lnqSaveStock);
                    }
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

        public DataTable GetMonthlyBalanceInfo(string yearMonth, string selectType, string storageID)
        {
            string error = null;

            Hashtable hsTable = new Hashtable();

            hsTable.Add("@YearMonth", yearMonth);
            hsTable.Add("@SelectType", selectType);
            hsTable.Add("@StorageID", storageID);

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfoPro("MonthlyBalanceSelect", hsTable, out error);

            if (!GlobalObject.BasicInfo.IsFuzzyContainsRoleName("会计"))
            {
                tempTable = UniversalFunction.ClearTable_Price(tempTable);
            }

            return tempTable;
        }

        public DataTable GetBusDetailInfo(string selectType, DateTime startTime, DateTime endTime, string storageID)
        {
            string strSql = null;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(selectType))
            {
                return null;
            }

            if (selectType.Contains("出库明细"))
            {
                strSql = " select * from ( select FetchBIllID as 出库单号, BillTime as 出库日期, AssociatedBillType as 关联单据类型, AssociatedBillNo as 关联单号, b.GoodsCode as 图号型号, b.GoodsName as 物品名称, b.Spec as 规格, "+
                         " c.ProviderName as 供应商名称, a.Provider as 供应商简码, a.ProviderBatchNo as 供方批次号, a.BatchNo as 批次号, a.FetchCount as 出库数量, a.UnitPrice as 单价, a.Price as 金额, a.Using as 用途, "+
                         " a.Department as 申请部门, a.FillInPersonnel as 申请人, a.FillInDate as 申请时间, a.DepartDirector as 审核人, a.DepotManager as 出库人, d.Explain as 业务类型, a.Remark as 备注, "+
                         " case when a.StorageID = '00' then '系统生成' else e.StorageName end as 库房名称, a.GoodsID as 物品ID, a.StorageID as 库房ID "+
                         " from S_FetchGoodsDetailBill as a left join F_GoodsPlanCost as b on a.GoodsID = b.ID "+
                         " left join Provider as c on a.Provider = c.ProviderCode "+
                         " left join BASE_SubsidiaryOperationType as d on a.OperationType = d.OperationType "+
                         " left join BASE_Storage as e on a.StorageID = e.StorageID ) as a "+
                         " where a.出库日期 >= '" + startTime.ToShortDateString() + "' and a.出库日期 <= '"+ endTime.ToShortDateString() +"'";

                if (selectType.Contains("样品不付款"))
                {
                    strSql += " and a.批次号 in (select BillNo from Business_Sample_ConfirmTheApplication where Purchase_BillType not like '%付款%') ";
                }

                if (storageID != null)
                {
                    if (storageID == "")
                    {
                        strSql += " and (a.库房ID in (select StorageID from BASE_Storage where FinancialAccountingFlag = 1)  or a.库房ID = '00' ) ";
                    }
                    else
                    {
                        strSql += " and a.库房ID = '" + storageID + "'";
                    }
                }

                strSql += " order by a.出库日期 ";
            }
            else if (selectType.Contains("入库明细"))
            {

                strSql = " select * from ( select a.InDepotBillID as 入库单号, a.BillTime as 入库日期, b.GoodsCode as 图号型号, b.GoodsName as 物品名称, b.Spec as 规格, c.ProviderName as 供应商名称, a.Provider as 供应商简码, "+
                         " a.BatchNo as 批次号,  a.InDepotCount as 入库数量, a.FactUnitPrice as 单价, a.FactPrice as 金额, a.Department as 申请部门, a.FillInPersonnel as 申请人, a.FillInDate as 申请时间, "+
                         " a.AffrimPersonnel as 入库人, d.Explain as 业务类型, a.Remark as 备注, case when a.StorageID = '00' then '系统生成' else e.StorageName end as 库房名称,  "+
                         " a.UnitPrice as 暂估单价, a.Price as 暂估金额, a.InvoiceUnitPrice as 发票单价, a.InvoicePrice as 发票金额, a.GoodsID as 物品ID, a.StorageID as 库房ID "+
                         " from S_InDepotDetailBill as a left join F_GoodsPlanCost as b on a.GoodsID = b.ID "+
                         " left join Provider as c on a.Provider = c.ProviderCode "+
                         " left join BASE_SubsidiaryOperationType as d on a.OperationType = d.OperationType "+
                         " left join BASE_Storage as e on a.StorageID = e.StorageID) as a " +
                         " where a.入库日期 >= '" + startTime.ToShortDateString() + "' and a.入库日期 <= '" + endTime.ToShortDateString() + "'";

                if (selectType.Contains("样品不付款"))
                {
                    strSql += " and a.批次号 in (select BillNo from Business_Sample_ConfirmTheApplication where Purchase_BillType not like '%付款%') ";
                }

                if (storageID != null)
                {
                    if (storageID == "")
                    {
                        strSql += " and (a.库房ID in (select StorageID from BASE_Storage where FinancialAccountingFlag = 1)  or a.库房ID = '00' ) ";
                    }
                    else
                    {
                        strSql += " and a.库房ID = '" + storageID + "'";
                    }
                }

                strSql += " order by a.入库日期 ";
            }
            else if (selectType.Contains("应付账款"))
            {

            }

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (!GlobalObject.BasicInfo.IsFuzzyContainsRoleName("会计"))
            {
                tempTable = UniversalFunction.ClearTable_Price(tempTable);
            }

            return tempTable;
        }
    }
}
