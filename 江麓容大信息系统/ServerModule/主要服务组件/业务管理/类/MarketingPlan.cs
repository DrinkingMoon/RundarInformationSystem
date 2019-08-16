/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MarketingPlan.cs
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
    /// 营销计划管理类
    /// </summary>
    class MarketingPlan:BasicServer, ServerModule.IMarketingPlan
    {
        public void UpdateFilePath(string billNo, string fileNo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_MarketingPlanBill
                          where a.DJH == billNo
                          select a;

            if (varData.Count() == 1)
            {
                S_MarketingPlanBill tempInfo = varData.Single();
                tempInfo.FileNo = fileNo;
            }

            ctx.SubmitChanges();
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_MarketingPlanBill
                          where a.DJH == billNo
                          select a;

            if (varData == null || varData.Count() == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// 检查某单据是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MarketingPlanBill] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得父表信息
        /// </summary>
        /// <returns>返回获得的营销计划主表信息</returns>
        public DataTable GetAllBill()
        {
            string strSql = "select * from View_S_MarketingPlanBill order by 单据年月 desc, 编制日期 ";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得子表信息
        /// </summary>
        /// <param name="djh">营销计划单号</param>
        /// <returns>返回获得的营销计划明细信息</returns>
        public DataTable GetList(string djh)
        {
            string strSql = "select * from View_S_MarketingPlanList where 单据号 = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 判断单据类型并进行相关操作
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="yearAndMonth">年月字符串 格式为“YYYYMM”</param>
        /// <param name="billType">计划类型(营销要货计划,营销变更计划,营销增量计划)</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool CheckBillType(DepotManagementDataContext ctx, string yearAndMonth,  string billType, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_MarketingPlanBill
                              where a.YearAndMonth == yearAndMonth
                              select a;

                if (varData.Count() > 0)
                {

                    switch (billType)
                    {
                        case "营销要货计划":
                            error = "此月份计划已存在不能重复添加";
                            return false;
                        case "营销变更计划":

                            foreach (var item in varData)
                            {

                                if (item.DJZT == "单据已完成")
                                {
                                    error = "此月份计划已完成不能被变更";
                                    return false;
                                }
                                else
                                {
                                    item.DJZT = "此单据已被变更";
                                }
                            }

                            break;
                        case "营销增量计划":

                            foreach (var item in varData)
                            {
                                if (item.DJZT != "单据已完成")
                                {
                                    error = "增量计划需在此月份的计划的单据状态为【单据已完成】的情况下才可添加";
                                    return false;
                                }
                            }

                            break;
                        default:
                            break;
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
        /// 添加父表信息
        /// </summary>
        /// <param name="marketingPlan">计划主表信息</param>
        /// <param name="planList">计划明细表信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddBill(S_MarketingPlanBill marketingPlan, DataTable planList, out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                if (!IsRepeatBillID(marketingPlan.DJH))
                {
                    if (!UpdateBill(ctx,marketingPlan, planList, out error))
                    {
                        return false;
                    }
                }
                else
                {
                    if (!DeletePlanList(ctx, marketingPlan.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddPlanList(ctx, planList, out error))
                    {
                        return false;
                    }

                    if (!CheckBillType(ctx, marketingPlan.YearAndMonth,marketingPlan.BillType, out error))
                    {
                        return false;
                    }

                    S_MarketingPlanBill lnqPlan = new S_MarketingPlanBill();

                    lnqPlan.DJH = marketingPlan.DJH;
                    lnqPlan.YearAndMonth = marketingPlan.YearAndMonth;
                    lnqPlan.BillType = marketingPlan.BillType;
                    lnqPlan.DJZT = "等待采购计划批准";
                    lnqPlan.BZR = BasicInfo.LoginName;
                    lnqPlan.BZRQ = ServerTime.Time;
                    lnqPlan.FirstMonthSumCount = marketingPlan.FirstMonthSumCount;
                    lnqPlan.SecondMonthSumCount = marketingPlan.SecondMonthSumCount;
                    lnqPlan.ThirdMonthSumCount = marketingPlan.ThirdMonthSumCount;
                    lnqPlan.Remark = marketingPlan.Remark;
                    lnqPlan.FileNo = marketingPlan.FileNo;

                    ctx.S_MarketingPlanBill.InsertOnSubmit(lnqPlan);
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
        /// 删除子表信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="djh">计划单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        private bool DeletePlanList(DepotManagementDataContext context, string djh, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_MarketingPlanList
                              where a.DJH == djh
                              select a;

                context.S_MarketingPlanList.DeleteAllOnSubmit(varData);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加子表信息
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="planList">计划子表信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        private bool AddPlanList(DepotManagementDataContext context, DataTable planList, out string error)
        {
            error = null;
            try
            {
                List<S_MarketingPlanList> lisPlan = new List<S_MarketingPlanList>();

                for (int i = 0; i < planList.Rows.Count; i++)
                {
                    S_MarketingPlanList lnqPlanList = new S_MarketingPlanList();

                    lnqPlanList.DJH = planList.Rows[i]["单据号"].ToString();
                    lnqPlanList.GoodsID = Convert.ToInt32(planList.Rows[i]["物品ID"]);
                    lnqPlanList.FirstMonthCount = Convert.ToDecimal(planList.Rows[i]["第一个月计划数"]);
                    lnqPlanList.SecondMonthCount = Convert.ToDecimal(planList.Rows[i]["第二个月计划数"]);
                    lnqPlanList.ThirdMonthCount = Convert.ToDecimal(planList.Rows[i]["第三个月计划数"]);
                    lnqPlanList.Remark = planList.Rows[i]["备注"].ToString();

                    lisPlan.Add(lnqPlanList);
                }

                context.S_MarketingPlanList.InsertAllOnSubmit(lisPlan);

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更改状态
        /// </summary>
        /// <param name="billStatus">单据状态</param>
        /// <param name="billIDList">计划单号列表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        public bool UpdateBill(string billStatus, DataTable billIDList, out string error)
        {
            error = null;
            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                for (int i = 0; i < billIDList.Rows.Count; i++)
                {
                    var varData = from a in ctx.S_MarketingPlanBill
                                  where a.DJH == billIDList.Rows[i]["单据号"].ToString()
                                  select a;

                    if (varData.Count() == 1)
                    {
                        S_MarketingPlanBill lnqMkPlan = varData.Single();

                        if (billStatus != lnqMkPlan.DJZT)
                        {
                            error = "单据状态错误，请重新刷新单据确认单据状态";
                            return false;
                        }

                        switch (lnqMkPlan.DJZT)
                        {
                            case "等待主管审核":
                                lnqMkPlan.DJZT = "等待领导批准";
                                lnqMkPlan.SHR = BasicInfo.LoginName;
                                lnqMkPlan.SHRQ = ServerTime.Time;
                                break;
                            case "等待领导批准":
                                lnqMkPlan.DJZT = "等待采购计划批准";
                                lnqMkPlan.PZR = BasicInfo.LoginName;
                                lnqMkPlan.PZRQ = ServerTime.Time;
                                break;
                            default:
                                break;
                        }

                        ctx.SubmitChanges();
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
        /// 删除单据
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteBill(string djh, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_MarketingPlanBill
                              where a.DJH == djh
                              select a;

                if (varData.Count() == 1)
                {
                    S_MarketingPlanBill lnqMarketingPlan = varData.Single();

                    if (DeletePlanList(ctx, djh, out error))
                    {
                        ctx.S_MarketingPlanBill.DeleteOnSubmit(lnqMarketingPlan);
                        ctx.SubmitChanges();
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    error = "数据不唯一";
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
        /// 检查单据是否重复
        /// </summary>
        /// <param name="djh">计划单号</param>
        /// <returns>未重复返回True,重复返回False</returns>
        public bool IsRepeatBillID(string djh)
        {
            string strSql = "select * from S_MarketingPlanBill where DJH = '" + djh + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0 || dt == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获得计划年月
        /// </summary>
        /// <returns>返回计划年月</returns>
        public string GetYearAndMonth()
        {
            string strSql = "select Max(YearAndMonth) from S_MarketingPlanBill";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0 || dt.Rows[0][0].ToString() == "")
            {
                return ServerTime.Time.Year.ToString() + ServerTime.Time.Month.ToString("D2");
            }
            else
            {
                return (Convert.ToInt32(dt.Rows[0][0].ToString()) + 1).ToString();
            }
        }

        /// <summary>
        /// 修改父表信息
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="inMkPlan">采购计划主表信息</param>
        /// <param name="planList">采购计划明细信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        private bool UpdateBill(DepotManagementDataContext ctx, S_MarketingPlanBill inMkPlan, DataTable planList, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_MarketingPlanBill
                              where a.DJH == inMkPlan.DJH
                              select a;

                if (varData.Count() == 1)
                {
                    if (!DeletePlanList(ctx, inMkPlan.DJH, out error))
                    {
                        return false;
                    }

                    if (!AddPlanList(ctx, planList, out error))
                    {
                        return false;
                    }

                    if (!CheckBillType(ctx, inMkPlan.YearAndMonth,inMkPlan.BillType, out error))
                    {
                        return false;
                    }

                    S_MarketingPlanBill lnqPlan = varData.Single();

                    lnqPlan.DJZT = "等待采购计划批准";
                    lnqPlan.BillType = inMkPlan.BillType;
                    lnqPlan.YearAndMonth = inMkPlan.YearAndMonth;
                    lnqPlan.BZR = BasicInfo.LoginName;
                    lnqPlan.BZRQ = ServerTime.Time;
                    lnqPlan.FirstMonthSumCount = inMkPlan.FirstMonthSumCount;
                    lnqPlan.SecondMonthSumCount = inMkPlan.SecondMonthSumCount;
                    lnqPlan.ThirdMonthSumCount = inMkPlan.ThirdMonthSumCount;
                    lnqPlan.Remark = inMkPlan.Remark;
                    lnqPlan.FileNo = inMkPlan.FileNo;

                    ctx.SubmitChanges();
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
        /// 插入交货期数据
        /// </summary>
        /// <param name="marketingPlanDelivery">交货期数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddDelivery(S_MarketingPlanDelivery marketingPlanDelivery,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_MarketingPlanDelivery
                              where a.Delivery == marketingPlanDelivery.Delivery
                              && a.DJH == marketingPlanDelivery.DJH
                              && a.GoodsID == marketingPlanDelivery.GoodsID
                              && a.Month == marketingPlanDelivery.Month
                              select a;

                if (varData.Count() > 0)
                {
                    error = "不能插入重复记录";
                    return false;
                }
                else
                {
                    dataContext.S_MarketingPlanDelivery.InsertOnSubmit(marketingPlanDelivery);
                    dataContext.SubmitChanges();
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
        /// 删除营销计划交货期
        /// </summary>
        /// <param name="marketingPlanDelivery">交货期数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteDelivery(S_MarketingPlanDelivery marketingPlanDelivery, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;


                var varData = from a in dataContext.S_MarketingPlanDelivery
                              where a.Delivery == marketingPlanDelivery.Delivery
                              && a.DJH == marketingPlanDelivery.DJH
                              && a.GoodsID == marketingPlanDelivery.GoodsID
                              && a.Month == marketingPlanDelivery.Month
                              select a;

                dataContext.S_MarketingPlanDelivery.DeleteAllOnSubmit(varData);
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
        /// 获得交货期数据集
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="month">计划月</param>
        /// <returns>返回Table</returns>
        public DataTable GetPlanDeliveryInfo(string billID, int goodsID, int month)
        {
            string strSql = "select Delivery as 交货期,DeliveryCount as 交货数 from S_MarketingPlanDelivery where DJH = '"
                + billID +"' and GoodsID = "+ goodsID +" and Month = "+ month ;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得ToolTip字符串
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="month">计划月</param>
        /// <returns>返回字符串</returns>
        public string GetCellToolTipString(string billID, int goodsID, int month)
        {
            string strSql = "select Delivery,DeliveryCount from S_MarketingPlanDelivery where DJH = '"
                + billID + "' and GoodsID = " + goodsID + " and Month = " + month;

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            string strShow = "";

            if (dtTemp != null && dtTemp.Rows.Count != 0)
            {
                strShow = "交货期      交货数";
            }

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                strShow += "\n" + Convert.ToDateTime(dtTemp.Rows[i][0]).ToShortDateString() + "  ";

                
                if (Convert.ToDateTime(dtTemp.Rows[i][0]).Month < 10)
                {
                    strShow += " ";
                }

                if (Convert.ToDateTime(dtTemp.Rows[i][0]).Day < 10)
                {
                    strShow += " ";
                }

                strShow += dtTemp.Rows[i][1].ToString();
            }

            return strShow;
        }
    }
}
