using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using DBOperate;
using System.Collections;
using ServerModule;

namespace Service_Economic_Financial
{
    class MonthProcServer : IMonthProcServer
    {
        /// <summary>
        /// 台帐
        /// </summary>
        /// <param name="productName">查询方式</param>
        /// <param name="startDate">开始时间</param>
        /// <param name="endDate">结束时间</param>
        /// <param name="storageID">库房ID</param>
        /// <param name="showTable">返回table数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool GetAllGather(string productName,DateTime startDate, DateTime endDate, string storageID,
            out DataTable showTable, out string error)
        {
            IDBOperate dbOperate = CommentParameter.GetDBOperatorOfDepotManagement();

            error = null;
            showTable = null;

            try
            {
                Hashtable paramTable = new Hashtable();

                if (productName == "pro_B_GoodsListGather")
                {
                    paramTable.Add("@StartDate", startDate);
                    paramTable.Add("@EndDate", endDate);
                }

                DataSet ds = new DataSet();
                Dictionary<OperateCMD, object> dicOperateCMD = dbOperate.RunProc_CMD(productName, ds, paramTable);

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
        /// 修改财务月结汇总的调整金额
        /// </summary>
        /// <param name="yearAndMonth">修改的年月</param>
        /// <param name="dt">数据源</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回false</returns>
        public bool UpdateGather(string yearAndMonth, DataTable dt, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.B_GoodsListGather
                             where a.YearAndMonth == yearAndMonth
                             select a;

                if (result.Count() > 0)
                {
                    IQueryable<B_GoodsListGather> goodsGather = result;

                    foreach (var item in goodsGather)
                    {
                        B_GoodsListGather goodsList = item;

                        for (int i = 0; i < dt.Rows.Count-1; i++)
                        {
                            if (goodsList.GoodsID == Convert.ToInt32(dt.Rows[i]["物品ID"]))
                            {
                                if (Convert.ToDecimal(dt.Rows[i]["财务调整金额"]) != 0)
                                {
                                    goodsList.FinanceAdjust = Convert.ToDecimal(dt.Rows[i]["财务调整金额"]);
                                    goodsList.Adjustor = dt.Rows[i]["财务调整人"] == DBNull.Value ? ""
                                                             : dt.Rows[i]["财务调整人"].ToString();

                                    goodsList.Remark = dt.Rows[i]["备注"] == DBNull.Value ? ""
                                                             : dt.Rows[i]["备注"].ToString();
                                    goodsList.AdjustTime = Convert.ToDateTime(dt.Rows[i]["调整日期"]);

                                    dataContxt.SubmitChanges();
                                }
                            }
                        }
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
    }
}
