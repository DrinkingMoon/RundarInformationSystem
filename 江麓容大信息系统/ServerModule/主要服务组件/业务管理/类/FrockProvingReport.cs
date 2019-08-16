/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FrockProvingReport.cs
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
using System.Text.RegularExpressions;

namespace ServerModule
{
    /// <summary>
    /// 工装验证报告管理类
    /// </summary>
    class FrockProvingReport : BasicServer, ServerModule.IFrockProvingReport
    {
        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_FrockProvingReport
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_FrockProvingReport] where DJH = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得全部单据信息
        /// </summary>
        /// <param name="billStatus">单据状态,若为“全  部”则显示所有单据信息</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <returns>返回工装检验报告视图信息</returns>
        public DataTable GetBill(string billStatus, DateTime startTime, DateTime endTime)
        {
            string strSelect = "";

            if (billStatus != "全  部")
            {
                strSelect += "单据状态 = '" + billStatus + "' and ";
            }

            strSelect += "编制日期 >= '" + startTime + "' and 编制日期 <= '" + endTime + "'";

            string strSql = "select * from View_S_FrockProvingReport where " + strSelect + " order by 单据号 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dt;
        }

        /// <summary>
        /// 获得附属表的信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="attachedType">附属信息内容类型 （检验，验证）</param>
        /// <returns>返回附属表的数据集</returns>
        public DataTable GetAttachedTable(string djh, string attachedType)
        {
            string strSql = "select AttachedType, AskContent, AnswerContent " +
                " from S_FrockProvingReportAttached where DJH = '" + djh + "' and AttachedType = '" + attachedType + "'";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得一条记录的所有信息
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <returns>返回一条工装检验报告的数据集</returns>
        public S_FrockProvingReport GetBill(string djh)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.S_FrockProvingReport
                          where a.DJH == djh
                          select a;

            if (varData.Count() != 1)
            {
                return null;
            }
            else
            {
                return varData.Single();
            }
        }

        /// <summary>
        /// 插入新的数据
        /// </summary>
        /// <param name="frockProvingReport">数据集</param>
        /// <param name="attachedTable">检验验证内容表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool AddBill(S_FrockProvingReport frockProvingReport, DataTable attachedTable, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_FrockProvingReport
                              where a.DJH == frockProvingReport.DJH
                              select a;

                S_FrockProvingReport lnqFrock = new S_FrockProvingReport();

                if (varData.Count() != 0)
                {
                    lnqFrock = varData.First();

                    lnqFrock.DJH = frockProvingReport.DJH;
                    lnqFrock.BZRQ = ServerTime.Time;
                    lnqFrock.BZRY = BasicInfo.LoginName;
                    lnqFrock.BillType = frockProvingReport.BillType;
                    lnqFrock.ConnectBillNumber = frockProvingReport.ConnectBillNumber;
                    lnqFrock.DJZT = "等待检验要求";
                    lnqFrock.FrockNumber = frockProvingReport.FrockNumber;
                    lnqFrock.GoodsID = frockProvingReport.GoodsID;

                    if (attachedTable != null)
                    {
                        if (!UpdateAttached(ctx, lnqFrock.DJH, attachedTable, out error))
                        {
                            return false;
                        }
                    }

                    ctx.S_FrockProvingReport.InsertOnSubmit(lnqFrock);
                }
                else
                {

                    lnqFrock = new S_FrockProvingReport();

                    lnqFrock.DJH = frockProvingReport.DJH;
                    lnqFrock.BZRQ = ServerTime.Time;
                    lnqFrock.BZRY = BasicInfo.LoginName;
                    lnqFrock.BillType = frockProvingReport.BillType;
                    lnqFrock.ConnectBillNumber = frockProvingReport.ConnectBillNumber;
                    lnqFrock.DJZT = "等待检验要求";
                    lnqFrock.FrockNumber = frockProvingReport.FrockNumber;
                    lnqFrock.GoodsID = frockProvingReport.GoodsID;

                    if (attachedTable != null)
                    {
                        if (!UpdateAttached(ctx, lnqFrock.DJH, attachedTable, out error))
                        {
                            return false;
                        }
                    }

                    ctx.S_FrockProvingReport.InsertOnSubmit(lnqFrock);
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
        /// 插入工装检验与验证内容与实测数据
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="djh">单据号</param>
        /// <param name="attachedTable">检验与验证内容表</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool UpdateAttached(DepotManagementDataContext ctx, string djh, DataTable attachedTable, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_FrockProvingReportAttached
                              where a.DJH == djh
                              select a;

                ctx.S_FrockProvingReportAttached.DeleteAllOnSubmit(varData);

                for (int i = 0; i < attachedTable.Rows.Count; i++)
                {
                    S_FrockProvingReportAttached lnqAttached = new S_FrockProvingReportAttached();

                    lnqAttached.AnswerContent = attachedTable.Rows[i]["AnswerContent"].ToString();
                    lnqAttached.AskContent = attachedTable.Rows[i]["AskContent"].ToString();
                    lnqAttached.AttachedType = attachedTable.Rows[i]["AttachedType"].ToString();
                    lnqAttached.DJH = djh;

                    ctx.S_FrockProvingReportAttached.InsertOnSubmit(lnqAttached);
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
        /// <param name="djh">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool DeleteBill(string djh, out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();
            try
            {

                var varData = from a in ctx.S_FrockProvingReport
                              where a.DJH == djh
                              select a;

                if (varData.Count() != 1)
                {
                    throw new Exception("数据不唯一");
                }

                S_FrockProvingReport tempLnq = varData.Single();

                ctx.S_FrockProvingReport.DeleteAllOnSubmit(varData);

                var varAttached = from a in ctx.S_FrockProvingReportAttached
                                  where a.DJH == djh
                                  select a;

                ctx.S_FrockProvingReportAttached.DeleteAllOnSubmit(varAttached);
                ctx.SubmitChanges();



                if (tempLnq.BillType == "入库检验" 
                    && !tempLnq.ConnectBillNumber.Contains("ZGB")
                    && !UpdateOrdinaryInDepotBillStatus(ctx, tempLnq, out error))
                {
                    throw new Exception(error);
                }

                if (tempLnq.ConnectBillNumber.Contains("ZGB") 
                    && !UpdateFrockInDepotBillStatus(ctx, tempLnq, out error))
                {
                    throw new Exception(error);
                }
                ctx.SubmitChanges();

                ctx.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新普通入库单的单据状态
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="frockProving">工装验证报告单的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateOrdinaryInDepotBillStatus(DepotManagementDataContext ctx, S_FrockProvingReport frockProving, out string error)
        {
            error = null;
            try
            {
                var varData = from a in ctx.S_FrockProvingReport
                              where a.ConnectBillNumber == frockProving.ConnectBillNumber
                              select a;

                foreach (var item in varData)
                {

                    if (item.DJZT != "单据已完成")
                    {
                        return true;
                    }
                }

                var varOrdinary = from a in ctx.S_OrdinaryInDepotBill
                                  where a.Bill_ID == frockProving.ConnectBillNumber
                                  select a;

                if (varOrdinary.Count() != 1)
                {
                    error = "数据以空或者不唯一";
                    return false;
                }
                else
                {
                    bool blflag = false;

                    var varOrdinaryGoods = from a in ctx.S_OrdinaryInDepotGoodsBill
                                           where a.Bill_ID == frockProving.ConnectBillNumber
                                           select a;

                    foreach (var itemGoods in varOrdinaryGoods)
                    {
                        var varFrockReport = from a in ctx.S_FrockProvingReport
                                             where a.ConnectBillNumber == frockProving.ConnectBillNumber
                                             && a.GoodsID == itemGoods.GoodsID
                                             select a;

                        int intAmount = 0;

                        foreach (var itemReport in varFrockReport)
                        {

                            if ((bool)itemReport.IsInStock)
                            {
                                intAmount++;
                            }
                            else
                            {
                                var varFrockStandingBook = from a in ctx.S_FrockStandingBook
                                                           where a.FrockNumber == itemReport.FrockNumber
                                                           && a.GoodsID == itemReport.GoodsID
                                                           select a;

                                ctx.S_FrockStandingBook.DeleteAllOnSubmit(varFrockStandingBook);
                            }

                        }

                        itemGoods.Amount = intAmount;
                        itemGoods.Price = Convert.ToDecimal(intAmount) * Convert.ToDecimal(itemGoods.UnitPrice);

                        if (Convert.ToDecimal(itemGoods.Amount) > 0)
                        {
                            blflag = true;
                        }

                    }

                    if (blflag)
                    {
                        varOrdinary.Single().BillStatus = "等待入库";

                        m_billMessageServer.PassFlowMessage(varOrdinary.Single().Bill_ID,
                            string.Format("【订单号】:{0} 【物品申请人】:{1}   ※※※ 等待【工艺人员】处理", 
                            varOrdinary.Single().OrderBill_ID, UniversalFunction.GetPersonnelName( varOrdinary.Single().Proposer)), 
                            m_billMessageServer.GetRoleStringForStorage(varOrdinary.Single().StorageID).ToString(), true);
                    }
                    else
                    {
                        varOrdinary.Single().BillStatus = "已报废";
                        m_billMessageServer.DestroyMessage(varOrdinary.Single().Bill_ID);
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
        /// 更新自制件工装报检的单据状态
        /// </summary>
        /// <param name="ctx">LINQ数据上下文</param>
        /// <param name="frockProving">工装验证报告单的数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFrockInDepotBillStatus(DepotManagementDataContext ctx, S_FrockProvingReport frockProving, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_FrockProvingReport
                              where a.ConnectBillNumber == frockProving.ConnectBillNumber
                              select a;

                foreach (var item in varData)
                {
                    if (item.DJZT != "单据已完成")
                    {
                        return true;
                    }
                }

                var varFrock = from a in ctx.S_FrockInDepotBill
                               where a.Bill_ID == frockProving.ConnectBillNumber
                               select a;

                if (varFrock.Count() != 1)
                {
                    error = "数据以空或者不唯一";
                    return false;
                }
                else
                {
                    bool blflag = false;

                    var varFrockGoods = from a in ctx.S_FrockInDepotGoodsBill
                                        where a.Bill_ID == frockProving.ConnectBillNumber
                                        select a;

                    foreach (var itemGoods in varFrockGoods)
                    {
                        var varFrockReport = from a in ctx.S_FrockProvingReport
                                             where a.ConnectBillNumber == frockProving.ConnectBillNumber
                                             && a.GoodsID == itemGoods.GoodsID
                                             select a;

                        int intAmount = 0;

                        foreach (var itemReport in varFrockReport)
                        {

                            if ((bool)itemReport.IsInStock)
                            {
                                intAmount++;
                            }
                            else
                            {
                                var varFrockStandingBook = from a in ctx.S_FrockStandingBook
                                                           where a.FrockNumber == itemReport.FrockNumber
                                                           && a.GoodsID == itemReport.GoodsID
                                                           select a;

                                ctx.S_FrockStandingBook.DeleteAllOnSubmit(varFrockStandingBook);
                            }
                        }

                        itemGoods.Amount = intAmount;

                        if (Convert.ToDecimal(itemGoods.Amount) > 0)
                        {
                            blflag = true;
                        }
                    }

                    if (blflag)
                    {
                        varFrock.Single().Bill_Status = "等待入库";

                        m_billMessageServer.PassFlowMessage(varFrock.Single().Bill_ID,
                            string.Format("{0}号自制件工装报检已提交，请仓管入库",varFrock.Single().Bill_ID), 
                            m_billMessageServer.GetRoleStringForStorage(varFrock.Single().StorageID).ToString(), true);
                    }
                    else
                    {
                        varFrock.Single().Bill_Status = "已报废";
                        m_billMessageServer.DestroyMessage(varFrock.Single().Bill_ID);
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
        /// 保存信息
        /// </summary>
        /// <param name="frockProvingReport">LINQ 数据集</param>
        /// <param name="attachedTable">检测信息</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool SaveInfo(S_FrockProvingReport frockProvingReport, DataTable attachedTable, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_FrockProvingReport
                              where a.DJH == frockProvingReport.DJH
                              select a;

                if (varData.Count() != 1 )
                {
                    if (frockProvingReport.DJZT == "新建单据")
                    {
                        return true;
                    }
                    else
                    {
                        error = "数据为空或者不唯一";
                        return false;
                    }
                }
                else
                {
                    S_FrockProvingReport lnqFrock = varData.Single();

                    if (frockProvingReport.DJZT != "等待结论" && lnqFrock.DJZT != frockProvingReport.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }
                    switch (frockProvingReport.DJZT)
                    {
                        case "等待检验要求":

                            if (!m_serverFrockStandingBook.IsIntactSatelliteInformation(lnqFrock.FrockNumber, lnqFrock.GoodsID))
                            {
                                error = "工装信息未填写完整或者数据不唯一";
                                return false;
                            }

                            if (attachedTable.Select("AttachedType = '检验'").Length == 0)
                            {
                                error = "请填写检验要求";
                                return false;
                            }

                            if (!UpdateAttached(dataContext, lnqFrock.DJH, attachedTable, out error))
                            {
                                return false;
                            }

                            break;

                        case "等待检验":

                            lnqFrock.ExamineVerdict = frockProvingReport.ExamineVerdict;
                            lnqFrock.JYRY = BasicInfo.LoginName;
                            lnqFrock.JYRQ = ServerTime.Time;

                            if (frockProvingReport.IsExamineQualified != null)
                            {
                                lnqFrock.IsExamineQualified = (bool)frockProvingReport.IsExamineQualified;
                            }

                            DataRow[] drList = attachedTable.Select("AttachedType = '检验'");

                            foreach (DataRow dr in drList)
                            {
                                if (dr["AnswerContent"] == null || dr["AnswerContent"].ToString().Trim().Length == 0)
                                {
                                    error = "请填写检验内容";
                                    return false;
                                }
                            }

                            if (!UpdateAttached(dataContext, lnqFrock.DJH, attachedTable, out error))
                            {
                                return false;
                            }

                            break;
                        case "等待验证要求":

                            if (attachedTable.Select("AttachedType = '验证'").Length == 0)
                            {
                                error = "请填写验证要求";
                                return false;
                            }

                            if (!UpdateAttached(dataContext, lnqFrock.DJH, attachedTable, out error))
                            {
                                return false;
                            }

                            break;
                        case "等待验证":

                            lnqFrock.YZRQ = ServerTime.Time;
                            lnqFrock.YZRY = BasicInfo.LoginName;
                            lnqFrock.ProvingVerdict = frockProvingReport.ProvingVerdict;

                            drList = attachedTable.Select("AttachedType = '验证'");

                            foreach (DataRow dr in drList)
                            {
                                if (dr["AnswerContent"] == null || dr["AnswerContent"].ToString().Trim().Length == 0)
                                {
                                    error = "请填写验证内容";
                                    return false;
                                }
                            }

                            if (!UpdateAttached(dataContext, lnqFrock.DJH, attachedTable, out error))
                            {
                                return false;
                            }

                            break;
                        case "等待结论":

                            //lnqFrock.IsProvingQualified = (bool)frockProvingReport.IsProvingQualified;
                            lnqFrock.FinalVerdict = frockProvingReport.FinalVerdict;
                            lnqFrock.GYRQ = ServerTime.Time;
                            lnqFrock.GYRY = BasicInfo.LoginName;
                            lnqFrock.IsInStock = frockProvingReport.IsInStock;

                            break;
                        case "等待最终审核":

                            lnqFrock.GYZGDate = ServerTime.Time;
                            lnqFrock.GYZG = BasicInfo.LoginName;

                            break;
                        default:
                            break;
                    }


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
        /// 单据流程
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="status">操作状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool UpdateBill(string billNo, string status, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_FrockProvingReport
                              where a.DJH == billNo
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据为空或者不唯一";
                    return false;
                }
                else
                {
                    S_FrockProvingReport lnqFrock = varData.Single();

                    if (status != "等待结论" && lnqFrock.DJZT != status)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (status)
                    {
                        case "等待检验要求":
                            lnqFrock.DJZT = "等待检验";
                            break;
                        case "等待检验":
                            lnqFrock.DJZT = "等待结论";
                            lnqFrock.JYRY = BasicInfo.LoginName;
                            lnqFrock.JYRQ = ServerTime.Time;
                            break;
                        case "等待验证要求":
                            lnqFrock.DJZT = "等待验证";
                            break;
                        case "等待验证":
                            lnqFrock.DJZT = "等待结论";
                            lnqFrock.YZRQ = ServerTime.Time;
                            lnqFrock.YZRY = BasicInfo.LoginName;
                            break;
                        case "等待结论":
                            lnqFrock.DJZT = "等待最终审核";
                            lnqFrock.GYRQ = ServerTime.Time;
                            lnqFrock.GYRY = BasicInfo.LoginName;
                            break;

                        case "等待最终审核":
                            lnqFrock.DJZT = "单据已完成";
                            lnqFrock.GYZGDate = ServerTime.Time;
                            lnqFrock.GYZG = BasicInfo.LoginName;

                            if (lnqFrock.BillType == "入库检验" && !varData.Single().ConnectBillNumber.Contains("ZGB")
                                && !UpdateOrdinaryInDepotBillStatus(dataContext, lnqFrock, out error))
                            {
                                return false;
                            }

                            if (varData.Single().ConnectBillNumber.Contains("ZGB") &&
                                !UpdateFrockInDepotBillStatus(dataContext, lnqFrock, out error))
                            {
                                return false;
                            }
                            break;
                        default:
                            break;
                    }

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
        /// 回退单据时对检验验证要求表进行处理
        /// </summary>
        /// <param name="ctx">LINQ</param>
        /// <param name="djh">单据号</param>
        /// <param name="billType">单据类型</param>
        /// <param name="operationFlag">处理方式 True 删除 False 更改</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool ReturnAttached(DepotManagementDataContext ctx, string djh, string billType, bool operationFlag, out string error)
        {
            error = null;

            try
            {
                var varData = from a in ctx.S_FrockProvingReportAttached
                              where a.DJH == djh
                              && a.AttachedType == billType
                              select a;

                if (operationFlag)
                {
                    ctx.S_FrockProvingReportAttached.DeleteAllOnSubmit(varData);
                }
                else
                {
                    foreach (var item in varData)
                    {
                        item.AnswerContent = "";
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
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus, out string error, string rebackReason)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_FrockProvingReport
                              where a.DJH == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_FrockProvingReport lnqFrock = varData.Single();

                    Nullable<DateTime> nulldt = null;

                    lnqFrock.DJZT = billStatus;

                    switch (billStatus)
                    {
                        case "等待结论":
                            strMsg = string.Format("{0}号工装验证报告单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, CE_RoleEnum.工艺人员.ToString(), true);

                            lnqFrock.GYRY = null;
                            lnqFrock.GYRQ = nulldt;
                            //lnqFrock.FinalVerdict = "";
                            //lnqFrock.IsInStock = false;

                            break;
                        case "等待验证":
                            strMsg = string.Format("{0}号工装验证报告单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqFrock.YZRY), false);

                            lnqFrock.GYRY = null;
                            lnqFrock.GYRQ = nulldt;
                            //lnqFrock.FinalVerdict = "";
                            //lnqFrock.IsInStock = false;

                            lnqFrock.YZRY = null;
                            lnqFrock.YZRQ = nulldt;
                            //lnqFrock.IsProvingQualified = false;
                            //lnqFrock.ProvingVerdict = "";

                            //if (!ReturnAttached(dataContext, djh, "验证", false, out error))
                            //{
                            //    return false;
                            //}

                            break;
                        case "等待验证要求":
                            strMsg = string.Format("{0}号工装验证报告单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, CE_RoleEnum.工艺人员.ToString(), true);

                            lnqFrock.GYRY = null;
                            lnqFrock.GYRQ = nulldt;
                            //lnqFrock.FinalVerdict = "";
                            //lnqFrock.IsInStock = false;

                            lnqFrock.YZRY = null;
                            lnqFrock.YZRQ = nulldt;
                            //lnqFrock.IsProvingQualified = false;
                            //lnqFrock.ProvingVerdict = "";

                            //if (!ReturnAttached(dataContext, djh, "验证", true, out error))
                            //{
                            //    return false;
                            //}

                            break;
                        case "等待检验":
                            strMsg = string.Format("{0}号工装验证报告单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqFrock.JYRY), false);

                            lnqFrock.JYRY = null;
                            lnqFrock.JYRQ = nulldt;
                            //lnqFrock.ExamineVerdict = "";

                            lnqFrock.GYRY = null;
                            lnqFrock.GYRQ = nulldt;
                            //lnqFrock.FinalVerdict = "";
                            //lnqFrock.IsExamineQualified = false;
                            //lnqFrock.IsInStock = false;

                            lnqFrock.YZRY = null;
                            lnqFrock.YZRQ = nulldt;
                            //lnqFrock.IsProvingQualified = false;
                            //lnqFrock.ProvingVerdict = "";

                            //if (!ReturnAttached(dataContext, djh, "验证", true, out error))
                            //{
                            //    return false;
                            //}

                            //if (!ReturnAttached(dataContext, djh, "检验", false, out error))
                            //{
                            //    return false;
                            //}

                            break;
                        case "等待检验要求":
                            strMsg = string.Format("{0}号工装验证报告单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, CE_RoleEnum.工艺人员.ToString(), true);

                            lnqFrock.JYRY = null;
                            lnqFrock.JYRQ = nulldt;
                            //lnqFrock.ExamineVerdict = "";

                            lnqFrock.GYRY = null;
                            lnqFrock.GYRQ = nulldt;
                            //lnqFrock.FinalVerdict = "";
                            //lnqFrock.IsExamineQualified = false;
                            //lnqFrock.IsInStock = false;

                            lnqFrock.YZRY = null;
                            lnqFrock.YZRQ = nulldt;
                            //lnqFrock.IsProvingQualified = false;
                            //lnqFrock.ProvingVerdict = "";

                            //if (!ReturnAttached(dataContext, djh, "验证", true, out error))
                            //{
                            //    return false;
                            //}

                            //if (!ReturnAttached(dataContext, djh, "检验", true, out error))
                            //{
                            //    return false;
                            //}

                            break;
                        case "新建单据":
                            strMsg = string.Format("{0}号工装验证报告单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqFrock.BZRY), false);

                            lnqFrock.JYRY = null;
                            lnqFrock.JYRQ = nulldt;
                            //lnqFrock.ExamineVerdict = "";

                            lnqFrock.GYRY = null;
                            lnqFrock.GYRQ = nulldt;
                            //lnqFrock.FinalVerdict = "";
                            //lnqFrock.IsExamineQualified = false;
                            //lnqFrock.IsInStock = false;

                            lnqFrock.YZRY = null;
                            lnqFrock.YZRQ = nulldt;
                            //lnqFrock.IsProvingQualified = false;
                            //lnqFrock.ProvingVerdict = "";

                            //if (!ReturnAttached(dataContext, djh, "验证", true, out error))
                            //{
                            //    return false;
                            //}

                            //if (!ReturnAttached(dataContext, djh, "检验", true, out error))
                            //{
                            //    return false;
                            //}

                            break;
                        default:
                            throw new Exception("单据状态不正确，请确认单据状态后，再操作");
                    }

                    dataContext.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一或者为空";
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
    }
}
