/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CommunicateReportBill.cs
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
    /// 主机厂报表管理类
    /// </summary>
    class CommunicateReportBill : ServerModule.ICommunicateReportBill
    {
        /// <summary>
        /// 根据主机厂的图号名称获得系统的物品ID
        /// </summary>
        /// <param name="communicateGoodsCode">主机厂图号</param>
        /// <param name="communicateGoodsName">主机厂名称</param>
        /// <param name="communicateCode">主机厂名称</param>
        /// <returns>返回0 为空 否则返回GOODSID</returns>
        public int GetGoodsID(string communicateGoodsCode,
            string communicateGoodsName, string communicateCode)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@GoodsCode", communicateGoodsCode);
            paramTable.Add("@GoodsName", communicateGoodsName);
            paramTable.Add("@Communicate", communicateCode);

            string strErr = "";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfoPro("YX_Select_YX_GoodsSystemMatchingCommunicate", paramTable, out strErr);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0]["GoodsID"]);
            }
        }

        /// <summary>
        /// 获得主机厂与系统零件的匹配信息
        /// </summary>
        /// <returns>返回获取的数据</returns>
        public DataTable GetMatchingTable()
        {
            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "YX_Select_View_YX_GoodsSystemMatchingCommunicate", null, out strErr);
        }

        /// <summary>
        /// 对于零件匹配表的数据库操作
        /// </summary>
        /// <param name="operationType">操作类型 (添加,删除)</param>
        /// <param name="goodsMathcning">Linq系统与供应商物品匹配的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateMathchingTable(string operationType, 
            YX_GoodsSystemMatchingCommunicate goodsMathcning,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_GoodsSystemMatchingCommunicate
                              where a.Communicate == goodsMathcning.Communicate
                              && ((a.CommunicateGoodsCode == goodsMathcning.CommunicateGoodsCode
                              && a.CommunicateGoodsName == goodsMathcning.CommunicateGoodsName) ||
                              (a.GoodsID == goodsMathcning.GoodsID))
                              select a;

                if (operationType == "添加")
                {
                    if (varData.Count() != 0)
                    {
                        error = "不符合一对一的匹配规则，请重新确认";
                        return false;
                    }
                    else
                    {
                        dataContext.YX_GoodsSystemMatchingCommunicate.InsertOnSubmit(goodsMathcning);
                    }
                }
                else if (operationType == "删除")
                {
                    dataContext.YX_GoodsSystemMatchingCommunicate.DeleteAllOnSubmit(varData);
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 查询挂账汇总表
        /// </summary>
        /// <param name="yearAndMonth">查询年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <returns>返回查询到的挂账汇总表</returns>
        public DataTable GetSignTheBill(string yearAndMonth,string communicate)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@YearAndMonth", yearAndMonth);
            paramTable.Add("@Communicate", communicate);

            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "YX_Select_YX_SignTheBill", paramTable, out strErr);
        }

        /// <summary>
        /// 查询回款汇总表
        /// </summary>
        /// <param name="startNy">开始年月 格式为“YYYYMM”</param>
        /// <param name="endNy">结束年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <returns>返回查询的回款汇总表</returns>
        public DataTable GetCommunicateReturnedMoneyBill(string startNy, string endNy, string communicate)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@StartNy", startNy);
            paramTable.Add("@EndNy", endNy);
            paramTable.Add("@Communicate", communicate);

            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "YX_Select_YX_CommunicateReturnedMoneyBill", paramTable, out strErr);
        }

        /// <summary>
        /// 确认挂账
        /// </summary>
        /// <param name="yearAndMonth">年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <returns>返回确认挂账后的挂账表</returns>
        public DataTable SetSingTheBillPrice(string yearAndMonth,string communicate)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@YearAndMonth", yearAndMonth);
            paramTable.Add("@Communicate", communicate);

            string strErr = "";

            return GlobalObject.DatabaseServer.QueryInfoPro(
                "YX_SetSignTheBillPrice", paramTable, out strErr);
        }

        /// <summary>
        /// 设置回款金额
        /// </summary>
        /// <param name="yearAndMonth">年月  格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <param name="returnMoney">回款金额</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>设置成功返回True，设置失败返回False</returns>
        public bool SetReturnedMoney(string yearAndMonth,string communicate, decimal returnMoney, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.YX_CommunicateReturnedMoneyBill
                              where a.Communicate == communicate
                              && a.YearAndMonth == yearAndMonth
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    varData.Single().ReturnedMoney = returnMoney;
                    varData.Single().NotReturnedMoney = 
                        Convert.ToDecimal(varData.Single().ShouldReturnedMoney) - returnMoney;


                    //设置下个月的应回款金额
                    DateTime dtNext = Convert.ToDateTime(yearAndMonth.Substring(0, 4) + "-" + 
                        yearAndMonth.Substring(4, 2) + "-01").AddMonths(1);

                    string strNextNy = dtNext.Year.ToString() + dtNext.Month.ToString("D2");

                    var varNext = from a in dataContext.YX_CommunicateReturnedMoneyBill
                              where a.Communicate == communicate
                              && a.YearAndMonth == strNextNy
                              select a;

                    if (varNext.Count() == 1)
                    {
                        varNext.Single().ShouldReturnedMoney = Convert.ToDecimal(
                            varNext.Single().ShouldReturnedMoney) - returnMoney;
                    }
                }

                dataContext.SubmitChanges();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
        }

        /// <summary>
        /// 由EXCEL表导入且对挂账表填入实挂数量
        /// </summary>
        /// <param name="communicate">主机厂编码</param>
        /// <param name="excle">EXCLE表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>导入成功返回True，导入失败返回False</returns>
        public bool UpdateSignTheBill(string communicate,DataTable excle,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                decimal dcPrice = 0;

                string strNy = ServerTime.GetMonthlyString(ServerTime.Time);

                for (int i = 0; i < excle.Rows.Count; i++)
                {
                    decimal dcOnePrice = 0;

                    int intGoodsID = GetGoodsID(
                        excle.Rows[i]["图号型号"].ToString(), excle.Rows[i]["物品名称"].ToString(), communicate);

                    if (intGoodsID != 0)
                    {
                        DateTime dtNext = 
                            Convert.ToDateTime(strNy.Substring(0, 4) + "-" + strNy.Substring(4, 2) + "-01").AddMonths(1);

                        //对本月的挂账以及下月的期初 数量进行变更
                        string strNextNy = dtNext.Year.ToString() + dtNext.Month.ToString("D2");

                        var varNext = from a in dataContext.YX_SignTheBill
                                      where a.Communicate == communicate
                                      && a.YearAndMonth == strNextNy
                                      && a.GoodsID == intGoodsID
                                      select a;

                        var varData = from a in dataContext.YX_SignTheBill
                                      where a.Communicate == communicate
                                      && a.YearAndMonth == strNy
                                      && a.GoodsID == intGoodsID
                                      select a;

                        if (varData.Count() == 0)
                        {
                            YX_SignTheBill lnqSign = new YX_SignTheBill();

                            lnqSign.Communicate = communicate;
                            lnqSign.CurrentPeriodRealHangCount = Convert.ToDecimal(excle.Rows[i]["实挂数量"]);
                            lnqSign.GoodsID = intGoodsID;
                            lnqSign.PriorPeriodBalanceCount = 0;
                            lnqSign.UnitPrice = Convert.ToDecimal(excle.Rows[i]["协议单价"]);
                            lnqSign.YearAndMonth = strNy;

                            dataContext.YX_SignTheBill.InsertOnSubmit(lnqSign);

                            lnqSign.Communicate = communicate;
                            lnqSign.CurrentPeriodRealHangCount = 0;
                            lnqSign.GoodsID = intGoodsID;
                            lnqSign.PriorPeriodBalanceCount = 
                                GetCommunicateGoodsOperationPrice(strNy, communicate, intGoodsID) -
                                Convert.ToDecimal(excle.Rows[i]["实挂数量"]);
                            lnqSign.UnitPrice = 0;
                            lnqSign.YearAndMonth = strNextNy;

                            dataContext.YX_SignTheBill.InsertOnSubmit(lnqSign);

                        }
                        else if (varData.Count() == 1)
                        {
                            YX_SignTheBill lnqSign = varData.Single();

                            lnqSign.CurrentPeriodRealHangCount = Convert.ToDecimal(excle.Rows[i]["实挂数量"]);
                            lnqSign.UnitPrice = Convert.ToDecimal(excle.Rows[i]["协议单价"]);

                            if (varNext.Count() == 1)
                            {
                                varNext.Single().PriorPeriodBalanceCount = lnqSign.PriorPeriodBalanceCount +
                                     GetCommunicateGoodsOperationPrice(strNy, communicate, intGoodsID) - 
                                     Convert.ToDecimal(excle.Rows[i]["实挂数量"]); ;
                            }
                        }
                        else
                        {
                            error = "数据不唯一,请重新核对后再导入";
                            return false;
                        }
                        
                    }
                    else
                    {
                        error = "图号为【" + excle.Rows[i]["图号型号"].ToString() + "】,名称为【" + 
                            excle.Rows[i]["物品名称"].ToString() + "】未匹配系统零件，请匹配后再导入";
                        return false;
                    }

                    dcOnePrice = Math.Round(Convert.ToDecimal(excle.Rows[i]["实挂数量"]) 
                        * Convert.ToDecimal(excle.Rows[i]["协议单价"]), 2);

                    dcPrice = dcPrice + dcOnePrice;
                }


                var varReturnMoney = from a in dataContext.YX_CommunicateReturnedMoneyBill
                                     where a.Communicate == communicate
                                     && a.YearAndMonth == strNy
                                     select a;

                if (varReturnMoney.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    varReturnMoney.Single().BillingPrice = dcPrice;
                }

                dataContext.SubmitChanges();

                return true;
                
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
        }

        /// <summary>
        /// 获得主机厂的物品的业务综合金额
        /// </summary>
        /// <param name="yearAndMonth">年月 格式为“YYYYMM”</param>
        /// <param name="communicate">主机厂编码</param>
        /// <param name="goodsID">物品ID</param>
        /// <returns>返回金额</returns>
        public decimal GetCommunicateGoodsOperationPrice(string yearAndMonth,string communicate,int goodsID)
        {
            Hashtable paramTable = new Hashtable();

            paramTable.Add("@YearAndMonth", yearAndMonth);
            paramTable.Add("@Communicate", communicate);
            paramTable.Add("@GoodsID", goodsID);

            string strErr = "";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfoPro("YX_GetOperationPrice", paramTable, out strErr);

            if (dt.Rows.Count == 0)
            {
                return 0;
            }
            else
            {
                return Convert.ToDecimal(dt.Rows[0]["SendCount"]) - Convert.ToDecimal(dt.Rows[0]["ReturnCount"]);
            }
        }
    }
}
