/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  PurcharsingPlan.cs
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
    /// 采购计划管理类
    /// </summary>
    class PurcharsingPlan : BasicServer, ServerModule.IPurcharsingPlan
    {
        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 记录缺件
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">单据号</param>
        public void RecordMissingParts(DepotManagementDataContext ctx, string billNo)
        {
            string strSql = @"select b.GoodsID, (FetchCount * Redices) - ExistCount as Amount 
                            from S_MaterialRequisition as a 
                            inner join BASE_ProductOrder as b on a.ProductType = b.Edition
                            inner join (select GoodsID,SUM(ExistCount) as ExistCount from S_Stock 
                            where StorageID = '01' and GoodsStatus = 0 group by GoodsID) as d on b.GoodsID = d.GoodsID
                            where FetchCount > 0 and FetchType = '整台领料' and ExistCount < (FetchCount * Redices) 
                            and Bill_ID = '" + billNo + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count == 0)
            {
                return;
            }

            foreach (DataRow dr in dtTemp.Rows)
            {
                CG_MissingParts lnqTemp = new CG_MissingParts();

                lnqTemp.Ny = ServerTime.GetMonthlyString(ServerTime.Time.AddMonths(1));
                lnqTemp.BillNo = billNo;
                lnqTemp.GoodsID = Convert.ToInt32(dr["GoodsID"]);
                lnqTemp.MissingCount = Convert.ToDecimal(dr["Amount"]);

                ctx.CG_MissingParts.InsertOnSubmit(lnqTemp);
            }
        }

        /// <summary>
        /// 删除公式
        /// </summary>
        /// <param name="mathID">公式ID</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返货False</returns>
        public bool DeleteProcurementMath(int mathID, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varPlan = from a in ctx.CG_ProcurementPlan
                              where a.MathID == mathID
                              select a;

                foreach (var item in varPlan)
                {
                    item.MathID = 0;
                }

                var varData = from a in ctx.CG_ProcurementMath
                              where a.MathID == mathID
                              select a;

                ctx.CG_ProcurementMath.DeleteAllOnSubmit(varData);

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
        /// 保存计算公式
        /// </summary>
        /// <param name="procurementMath">新公式数据</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool SaveProcurementMath(CG_ProcurementMath procurementMath,out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.CG_ProcurementMath
                              where a.MathID == procurementMath.MathID
                              select a;

                if (varData.Count() == 1)
                {
                    CG_ProcurementMath lnqMath = varData.Single();

                    lnqMath.MathName = procurementMath.MathName;
                    lnqMath.MathColumn = procurementMath.MathColumn;
                    lnqMath.MathFormula = procurementMath.MathFormula;
                }
                else if (varData.Count() == 0)
                {
                    ctx.CG_ProcurementMath.InsertOnSubmit(procurementMath);
                }
                else
                {
                    error = "数据不唯一";
                    return false;
                }

                ctx.SubmitChanges();

                varData = from a in  ctx.CG_ProcurementMath
                          where a.MathName == procurementMath.MathName
                          && a.MathFormula == procurementMath.MathFormula
                          && a.MathColumn == procurementMath.MathColumn
                          select a;

                error = varData.Single().MathID.ToString();

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 操作采购计划计算公式
        /// </summary>
        /// <param name="mode">操作方式</param>
        /// <param name="model">LINQ实体集</param>
        /// <param name="mathModel">LINQ实体集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperatorProcuremnetPlanFormla(CE_OperatorMode mode,CG_ProcurementPlan model, CG_ProcurementMath mathModel, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                CG_ProcurementPlan dataTemp = new CG_ProcurementPlan();

                var varData = from a in ctx.CG_ProcurementPlan
                              select a;

                switch (mode)
                {
                    case CE_OperatorMode.添加:

                        if (!SaveProcurementMath(mathModel, out error))
                        {
                            return false;
                        }

                        model.MathID = Convert.ToInt32(error);

                        ctx.CG_ProcurementPlan.InsertOnSubmit(model);

                        break;
                    case CE_OperatorMode.修改:

                        varData = from a in varData
                                  where a.ID == model.ID
                                  select a;

                        if (varData.Count() != 1)
                        {
                            error = "数据为空或者不唯一";
                            return false;
                        }

                        if (!SaveProcurementMath(mathModel, out error))
                        {
                            return false;
                        }

                        dataTemp = varData.Single();

                        dataTemp.GoodsID = model.GoodsID;
                        dataTemp.MathID = Convert.ToInt32(error);
                        dataTemp.StepsID = model.StepsID;

                        break;
                    case CE_OperatorMode.删除:

                        varData = from a in varData
                                  where a.ID == model.ID
                                  select a;

                        ctx.CG_ProcurementPlan.DeleteAllOnSubmit(varData);

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
        /// 获得计算公式数据
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetProcurementMath()
        {
            string strSql = "select * from View_CG_ProcurementMath";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得采购计划计算公式
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetProcurementPlanView(string sql)
        {

            string strSql = "select * from View_CG_ProcurementPlan where 1=1";

            if (sql != null && sql.Trim().Length >0)
            {
                strSql += sql;
            }

            strSql += " ORDER BY 物品ID,计算字段";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得采购计划计算公式
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable GetProcurementPlanView()
        {

            string strSql = "select * from View_CG_ProcurementPlan";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 获得最大操作步骤数
        /// </summary>
        /// <returns>返回Int</returns>
        int GetMaxProcurementStep()
        {

            string strSql = "select Max(OrderBy) from CG_ProcurementPlan";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0 || dt.Rows[0][0].ToString() == "")
            {
                return 0;
            }
            else
            {
                return Convert.ToInt32(dt.Rows[0][0]);
            }
        }

        /// <summary>
        /// 获得主表数据集
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回主表信息</returns>
        public DataRow GetBill(string yearAndMonth)
        {
            string strSql = "select * from S_PurchasingBill where Ny = '" + yearAndMonth + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
            
        }

        /// <summary>
        /// 返回明细数据
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回明细信息</returns>
        public DataTable GetList(string yearAndMonth)
        {
            string error = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@Ny", yearAndMonth);

            return GlobalObject.DatabaseServer.QueryInfoPro("CG_SEL_Purchasing", paramTable, out error);
        }

        /// <summary>
        /// 获得最新的记录集
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回最新的查询记录集</returns>
        public DataTable GetNewList(string yearAndMonth,out string error)
        {
            error = null;

            Hashtable paramTable = new Hashtable();
            paramTable.Add("@Ny", yearAndMonth);

            DataSet ds = new DataSet();
            Dictionary<OperateCMD, object> dicOperateCMD = m_dbOperate.RunProc_CMD("NewPurchasingPlan", ds, paramTable);

            if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
            {
                return null;
            }

            if (ds == null || ds.Tables.Count == 0)
            {
                return null;
            }
            else
            {
                DataTable dtResult = ds.Tables[0];

                if (!MathTable(ref dtResult, out error))
                {
                    return null;
                }
                else
                {
                    GetOrderGoodsCount(dtResult);
                    GetOrderGoodsPrice(dtResult);
                    return dtResult;
                }
            }
        }

        /// <summary>
        /// 获取订货金额
        /// </summary>
        /// <param name="messageTable">需要获取订货金额的数据集</param>
        /// <returns>返回已获取到订货金额的数据集</returns>
        public DataTable GetOrderGoodsPrice(DataTable messageTable)
        {
            BargainInfoServer serverBargainInfo = new BargainInfoServer();

            for (int i = 0; i < messageTable.Rows.Count; i++)
            {
                //if (Convert.ToDecimal(messageTable.Rows[i]["第二月订货数"]) > 0)
                //{
                //    DataRow drPrice = serverBargainInfo.GetLatelyBargainNumberInfo(Convert.ToInt32(messageTable.Rows[i]["物品ID"]),
                //        messageTable.Rows[i]["供应商"].ToString());

                //    if (drPrice == null)
                //    {
                //        messageTable.Rows[i]["订货金额"] = 0.00;
                //    }
                //    else
                //    {
                //        messageTable.Rows[i]["订货金额"] = Math.Round(
                //            Convert.ToDecimal(drPrice["UnitPrice"]) *
                //            Convert.ToDecimal(messageTable.Rows[i]["订货数"]), 2);
                //    }

                //}
                //else
                //{
                //    messageTable.Rows[i]["订货金额"] = 0.00;
                //}

                if (messageTable.Rows[i]["单价"] == null || messageTable.Rows[i]["订货数"] == null)
                {
                    messageTable.Rows[i]["订货金额"] = 0;
                }
                else
                {
                    messageTable.Rows[i]["订货金额"] = Math.Round(
                                Convert.ToDecimal(messageTable.Rows[i]["单价"]) *
                                Convert.ToDecimal(messageTable.Rows[i]["订货数"]), 2);
                }

            }

            return messageTable;
        }

        /// <summary>
        /// 计算订货数
        /// </summary>
        /// <param name="messageTable">需要获取订货数的数据集</param>
        /// <returns>返回已获取到订货数的数据集</returns>
        public DataTable GetOrderGoodsCount(DataTable messageTable)
        {
            for (int i = 0; i < messageTable.Rows.Count; i++)
            {
                DataRow drPackMessage = GetLeast(Convert.ToInt32(messageTable.Rows[i]["物品ID"]),
                    messageTable.Rows[i]["供应商"].ToString());

                if (drPackMessage == null)
                {
                    messageTable.Rows[i]["订货数"] = 0;
                }
                else
                {
                    if (Convert.ToDecimal(messageTable.Rows[i]["第二月订货数"]) <= 0)
                    {
                        messageTable.Rows[i]["订货数"] = 0;
                    }
                    else
                    {
                        //获取需要的订货总数
                        decimal dcWillCount = Convert.ToDecimal(messageTable.Rows[i]["第二月订货数"]);

                        //当第一条时且拥有两个供应商的物品
                        if (i == 0
                            && messageTable.Rows.Count > 1
                            && messageTable.Rows[i]["物品ID"].ToString() == messageTable.Rows[i + 1]["物品ID"].ToString())
                        {
                            double k = Convert.ToDouble(dcWillCount) / Convert.ToDouble(drPackMessage["最小包装数"]);

                            messageTable.Rows[i]["订货数"] = Convert.ToDecimal(drPackMessage["最小包装数"]) 
                                * Convert.ToDecimal( Math.Round(k));
                        }//当最后一条时且拥有两个供应商的物品
                        else if (i == messageTable.Rows.Count - 1
                            && messageTable.Rows.Count > 1
                            && messageTable.Rows[i]["物品ID"].ToString() == messageTable.Rows[i - 1]["物品ID"].ToString())
                        {
                            decimal dcCount = Convert.ToDecimal(Convert.ToDouble(dcWillCount)
                                / Convert.ToDouble(messageTable.Rows[i]["供应商采购份额"])) -
                                Convert.ToDecimal(messageTable.Rows[i - 1]["订货数"]);

                            if (dcCount > 0)
                            {
                                double k = Convert.ToDouble(dcCount) / Convert.ToDouble(drPackMessage["最小包装数"]);

                                messageTable.Rows[i]["订货数"] = Convert.ToDecimal(drPackMessage["最小包装数"])
                                    * Convert.ToDecimal(Math.Ceiling(k));
                            }
                            else
                            {
                                messageTable.Rows[i]["订货数"] = 0;
                            }

                        }//当拥有两个供应商且为第一供应商时
                        else if (i < messageTable.Rows.Count - 1 
                            && messageTable.Rows[i]["物品ID"].ToString() == messageTable.Rows[i + 1]["物品ID"].ToString())
                        {
                            double k = Convert.ToDouble(dcWillCount) / Convert.ToDouble(drPackMessage["最小包装数"]);

                            messageTable.Rows[i]["订货数"] = Convert.ToDecimal(drPackMessage["最小包装数"])
                                * Convert.ToDecimal(Math.Round(k));
                        }//当拥有两个供应商且为第二供应商时
                        else if (i - 1 > 0 
                            && messageTable.Rows[i]["物品ID"].ToString() == messageTable.Rows[i - 1]["物品ID"].ToString())
                        {
                            decimal dcCount = Convert.ToDecimal(Convert.ToDouble(dcWillCount)
                                / Convert.ToDouble(messageTable.Rows[i]["供应商采购份额"])) - 
                                Convert.ToDecimal(messageTable.Rows[i - 1]["订货数"]);

                            if (dcCount > 0)
                            {
                                double k = Convert.ToDouble(dcCount) / Convert.ToDouble(drPackMessage["最小包装数"]);

                                messageTable.Rows[i]["订货数"] = Convert.ToDecimal(drPackMessage["最小包装数"])
                                    * Convert.ToDecimal(Math.Ceiling(k));
                            }
                            else
                            {
                                messageTable.Rows[i]["订货数"] = 0;
                            }
                        }//其他情况
                        else
                        {
                            //当需求量小于最小采购量
                            if (dcWillCount <= Convert.ToDecimal(drPackMessage["最小采购数"]))
                            {
                                messageTable.Rows[i]["订货数"] = Convert.ToDecimal(drPackMessage["最小采购数"]);
                            }//当需求量大于等于最小采购量
                            else
                            {

                                int dbCount = 0;

                                int dbMath = Math.DivRem(Convert.ToInt32(dcWillCount),
                                    Convert.ToInt32(drPackMessage["最小包装数"]), out dbCount);

                                if (Convert.ToDecimal(dbCount) > Convert.ToDecimal(messageTable.Rows[i]["安全库存数"]))
                                {
                                    messageTable.Rows[i]["订货数"] = Convert.ToDecimal((int)dbMath + 1) *
                                        Convert.ToDecimal(drPackMessage["最小包装数"]);
                                }
                                else
                                {
                                    messageTable.Rows[i]["订货数"] = Convert.ToDecimal((int)dbMath) *
                                        Convert.ToDecimal(drPackMessage["最小包装数"]);
                                }
                            }
                        }
                    }
                }
            }

            return messageTable;
        }

        /// <summary>
        /// 添加所有信息
        /// </summary>
        /// <param name="messageList">需要添加的信息</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddAllBill(DataTable messageList,string yearAndMonth,out string error)
        {
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_PurchasingBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() > 0)
                {
                    error = "数据已存在不需添加数据";
                    return false;
                }
                else
                {
                    if (!AddBill(dataContext,yearAndMonth,out error))
                    {
                        return false;
                    }

                    if (!DeleteList(dataContext,yearAndMonth,out error))
                    {
                        return false;
                    }

                    if (!AddList(dataContext,messageList,yearAndMonth,out error))
                    {
                        return false;
                    }

                    dataContext.SubmitChanges();
                }


            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
            return true;
        }

        /// <summary>
        /// 更新子父表信息
        /// </summary>
        /// <param name="messageList">需要更新的数据信息</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateBill(DataTable messageList, string yearAndMonth, out string error)
        {
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_PurchasingBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() > 1)
                {
                    error = "数据不唯一";
                    return false;
                }
                else
                {
                    if (varData.Count() == 1)
                    {
                        S_PurchasingBill lnqBill = varData.Single();

                        if (lnqBill.DJZT == "单据已完成")
                        {
                            error = "单据状态已完成不能保存";
                            return false;
                        }

                        lnqBill.DJZT = "等待确认";
                        lnqBill.BZR = BasicInfo.LoginName;
                        lnqBill.BZRQ = ServerTime.Time;
                        lnqBill.PZR = null;
                        lnqBill.PZRQ = null;
                        lnqBill.SHR = null;
                        lnqBill.SHRQ = null;
                    }
                    else
                    {
                        S_PurchasingBill lnqBill = new S_PurchasingBill();
                        lnqBill.DJZT = "等待确认";
                        lnqBill.BZR = BasicInfo.LoginName;
                        lnqBill.BZRQ = ServerTime.Time;
                        lnqBill.Ny = yearAndMonth;

                        dataContext.S_PurchasingBill.InsertOnSubmit(lnqBill);
                    }

                    //if (!UpdateForStockPersonnelAndProvider(Contxt, dtList, 
                    //    out error, out dtList))
                    //{
                    //    return false;
                    //}

                    if (!DeleteList(dataContext,yearAndMonth,out error))
                    {
                        return false;
                    }

                    if (!AddList(dataContext,messageList,yearAndMonth,out error))
                    {
                        return false;
                    }

                    dataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }

            return true;
        }

        /// <summary>
        /// 添加主表数据
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="yearAndMonth">年月</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddBill(DepotManagementDataContext ctx,string yearAndMonth,out string error)
        {
            try
            {
                error = null;

                var varData = from a in ctx.S_PurchasingBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() > 0)
                {
                    error = "数据已存在不能添加";
                    return false;
                }
                else
                {
                    S_PurchasingBill lnqBill = new S_PurchasingBill();

                    lnqBill.Ny = yearAndMonth;
                    lnqBill.DJZT = "等待主管审核";
                    lnqBill.BZR = BasicInfo.LoginName;
                    lnqBill.BZRQ = ServerTime.Time;

                    ctx.S_PurchasingBill.InsertOnSubmit(lnqBill);
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }

            return true;
        }

        /// <summary>
        /// 添加子表数据
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="messageList">需要添加的数据集</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>添加成功返回True，添加失败返回False</returns>
        public bool AddList(DepotManagementDataContext ctx,DataTable messageList,
            string yearAndMonth,out string error)
        {
            try
            {

                error = null;

                for (int i = 0; i < messageList.Rows.Count; i++)
                {
                    S_PurchasingList lnqList = new S_PurchasingList();

                    lnqList.Ny = yearAndMonth;
                    lnqList.GoodsID = Convert.ToInt32(messageList.Rows[i]["物品ID"]);
                    lnqList.IsCommonage = messageList.Rows[i]["是否为共用件"].ToString();
                    lnqList.Buyer = messageList.Rows[i]["采购员"].ToString();
                    lnqList.Provider = messageList.Rows[i]["供应商"].ToString();
                    lnqList.ProductDay = Convert.ToInt32(messageList.Rows[i]["生产周期"]);
                    //lnqList.ImproperStockCount = Convert.ToDecimal(messageList.Rows[i]["非正常库存数"]);
                    //lnqList.NaturalStockCount = Convert.ToDecimal(messageList.Rows[i]["正常库存数"]);
                    //lnqList.SumStockCount = Convert.ToDecimal(messageList.Rows[i]["总库存数"]);
                    lnqList.MathStockCount = Convert.ToDecimal(messageList.Rows[i]["库存数"]);
                    lnqList.FirstMonthPlanCount = Convert.ToDecimal(messageList.Rows[i]["第一月计划数"]);
                    lnqList.SecondMonthPlanCount = Convert.ToDecimal(messageList.Rows[i]["第二月计划数"]);
                    lnqList.ThirdMonthPlanCount = Convert.ToDecimal(messageList.Rows[i]["第三月计划数"]);
                    //lnqList.CheckCount = Convert.ToDecimal(messageList.Rows[i]["待检数"]);
                    lnqList.MathCheckCount = Convert.ToDecimal(messageList.Rows[i]["待检数"]);
                    //lnqList.SafeStockCount = Convert.ToDecimal(messageList.Rows[i]["安全库存数"]);
                    lnqList.MathSafeStockCount = Convert.ToDecimal(messageList.Rows[i]["安全库存数"]);
                    //lnqList.AlreadyOrderFormCount = Convert.ToDecimal(messageList.Rows[i]["已下订单未到货数"]);
                    lnqList.MathAlreadyOrderFormCount = Convert.ToDecimal(messageList.Rows[i]["已下订单未到货数"]);
                    lnqList.StockQuota = Convert.ToInt32(Convert.ToDecimal(messageList.Rows[i]["供应商采购份额"]) * 100);
                    //lnqList.JumblyQuota = Convert.ToInt32(Convert.ToDecimal(messageList.Rows[i]["装配采购份额"]) * 100);
                    lnqList.FirstMonthCount = Convert.ToDecimal(messageList.Rows[i]["第一月订货总数"]);
                    lnqList.SecondMonthCount = Convert.ToDecimal(messageList.Rows[i]["第二月订货总数"]);
                    lnqList.ThirdMonthCount = Convert.ToDecimal(messageList.Rows[i]["第三月订货总数"]);
                    lnqList.OrderCount = Convert.ToDecimal(messageList.Rows[i]["订货数"]);
                    lnqList.UnitPrice = Convert.ToDecimal(messageList.Rows[i]["单价"]);
                    lnqList.OrderPrice = Convert.ToDecimal(messageList.Rows[i]["订货金额"]);

                    ctx.S_PurchasingList.InsertOnSubmit(lnqList);
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }
            return true;
        }

        /// <summary>
        /// 删除子表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>删除成功返回True，删除失败返回False</returns>
        public bool DeleteList(DepotManagementDataContext ctx, string yearAndMonth,out string error)
        {
            try
            {
                error = null;

                var varData = from a in ctx.S_PurchasingList
                              where a.Ny == yearAndMonth
                              select a;

                ctx.S_PurchasingList.DeleteAllOnSubmit(varData);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 更新主表单据状态
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="djzt">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更新成功返回True，更新失败返回False</returns>
        public bool UpdateBill(string yearAndMonth,string djzt,out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_PurchasingBill
                              where a.Ny == yearAndMonth
                              select a;

                if (varData.Count() != 1)
                {
                    error = "数据不唯一或者为空";
                    return false;
                }
                else
                {
                    S_PurchasingBill lnqBill = varData.Single();

                    if (djzt != lnqBill.DJZT)
                    {
                        error = "单据状态错误，请重新刷新单据确认单据状态";
                        return false;
                    }

                    switch (lnqBill.DJZT)
                    {
                        //case "等待主管审核":
                        //    lnqBill.DJZT = "等待领导批准";
                        //    lnqBill.SHR = BasicInfo.LoginName;
                        //    lnqBill.SHRQ = ServerTime.Time;
                        //    break;
                        case "等待确认":
                            lnqBill.DJZT = "单据已完成";
                            lnqBill.PZR = BasicInfo.LoginName;
                            lnqBill.PZRQ = ServerTime.Time;

                            //if (!InsertWebOrderFormBill(yearAndMonth,out error))
                            //{
                            //    return false;
                            //}

                            if (!AffrimBill(dataContext, yearAndMonth, out error))
                            {
                                return false;
                            }

                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }

            return true;
        }

        /// <summary>
        /// 获取最新一条的营销计划的单据号
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <returns>返回营销计划的单据号</returns>
        string GetMaxBill(string yearAndMonth)
        {
            string strSql = "select Max(DJH) from S_MarketingPlanBill where YearAndMonth = '" + yearAndMonth + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt == null)
            {
                return "";
            }
            else
            {
                return dt.Rows[0][0].ToString();
            }
        }

        /// <summary>
        /// 获得生产计划单据列表与营销要货计划单据列表
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="marketingPlan">返回的营销计划数据集</param>
        /// <param name="productPlan">返回的生产计划数据集</param>
        public void GetMarketingPlanAndProductPlan(string yearAndMonth, out DataTable marketingPlan, out DataTable productPlan)
        {
            DateTime dtStart = new DateTime();
            DateTime dtEnd = new DateTime();

            ServerTime.GetMonthlyBalance(yearAndMonth, out dtStart, out dtEnd);

            string strSql = "select * from S_ProductPlan where PlanTime >= '" 
                + dtStart + "' and  PlanTime <= '" + dtEnd + "'";

            productPlan = GlobalObject.DatabaseServer.QueryInfo(strSql);

            strSql = "select Top 1 (DJH) from S_MarketingPlanBill where YearAndMonth = '" 
                + yearAndMonth + "' order by DJH desc ";

            marketingPlan = GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 对营销计划，生产计划的单据状态进行更改
        /// </summary>
        /// <param name="contxt">数据上下文</param>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>更改成功返回True，更改失败返回False</returns>
        bool AffrimBill(DepotManagementDataContext contxt, string yearAndMonth, out string error)
        {
            error = null;

            try
            {
                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();

                ServerTime.GetMonthlyBalance(yearAndMonth, out dtStart, out dtEnd);

                var varData_ProductPlan = from a in contxt.S_ProductPlan
                                          where a.PlanTime >= dtStart
                                          && a.PlanTime < dtEnd
                                          select a;

                foreach (var varProduct in varData_ProductPlan)
                {
                    
                    varProduct.DJZT = "单据已完成";
                    m_assignBill.UseBillNo(contxt, "生产计划", varProduct.DJH);
                }

                var varData_MarketingPlan = from a in contxt.S_MarketingPlanBill
                                            where a.YearAndMonth == yearAndMonth
                                            && a.DJZT != "此单据已被变更"
                                            select a;

                foreach (S_MarketingPlanBill item in varData_MarketingPlan)
                {
                    item.DJZT = "单据已完成";
                    m_assignBill.UseBillNo(contxt, "营销要货计划", item.DJH);
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
                
            }

            return true;
        }

        /// <summary>
        /// 插入网络订单
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        private bool InsertWebOrderFormBill(string yearAndMonth,out string error)
        {
            BargainInfoServer serverBargainInfo = new BargainInfoServer();
            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                #region 清空数据
                var varData_bill = from a in dataContext.B_WebForOrderFormBill
                                   where a.Ny == yearAndMonth
                              select a;

                if (varData_bill.Count() > 0)
                {
                    dataContext.B_WebForOrderFormBill.DeleteAllOnSubmit(varData_bill);

                    var varData_list = from a in dataContext.B_WebForOrderFormList
                                       where a.Ny == yearAndMonth
                                       select a;

                    foreach (var item in varData_list)
                    {
                        var varData_date = from a in dataContext.B_WebForAffirmTime
                                           where a.ListID == item.ID
                                           select a;

                        dataContext.B_WebForAffirmTime.DeleteAllOnSubmit(varData_date);
                    }

                    dataContext.B_WebForOrderFormList.DeleteAllOnSubmit(varData_list);
                    dataContext.SubmitChanges();
                }

                
                #endregion

                DataTable dtGoodsLost = GetGoodsLostTable(yearAndMonth,out error);

                if (dtGoodsLost == null)
                {
                    return false;
                }

                decimal dcOrderFormCount = 0;
                string strBargainNumber = "";

                #region For循环插入明细数据
                //采购计划FOR循环
                for (int i = 0; i < dtGoodsLost.Rows.Count; i++)
                {
                    //获得合同号
                    DataRow drBargainNumber = serverBargainInfo.GetLatelyBargainNumberInfo(
                        Convert.ToInt32(dtGoodsLost.Rows[i]["物品ID"].ToString()),
                        dtGoodsLost.Rows[i]["供应商"].ToString());

                    if (drBargainNumber == null)
                    {
                        continue;
                    }
                    else
                    {
                        strBargainNumber = drBargainNumber["BargainNumber"].ToString();
                    }

                    dcOrderFormCount = Convert.ToDecimal(dtGoodsLost.Rows[i]["第二月订货数"]);


                    //if (Convert.ToDecimal( dtGoodsLost.Rows[i]["第一月订货数"]) > 0)
                    //{
                    //    dcOrderFormCount = Convert.ToDecimal(dtGoodsLost.Rows[i]["第一月订货数"]) +
                    //        Convert.ToDecimal(dtGoodsLost.Rows[i]["第二月订货数"]);
                    //}
                    //else
                    //{
                    //    dcOrderFormCount = Convert.ToDecimal(dtGoodsLost.Rows[i]["第二月订货数"]);
                    //}

                    //插入数据
                    if (!InsertList(yearAndMonth,Convert.ToInt32(dtGoodsLost.Rows[i]["物品ID"].ToString()),
                        Convert.ToDecimal( dtGoodsLost.Rows[i]["订货数"].ToString()), dtGoodsLost.Rows[i]["供应商"].ToString(), strBargainNumber,
                        dcOrderFormCount, out error))
                    {
                        return false;
                    }
                    

                }
                #endregion


                //插入主表
                if (dtGoodsLost.Rows.Count > 0)
                {
                    if (!InsertBill(yearAndMonth,out error))
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 插入明细
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="goodsID">物品ID</param>
        /// <param name="orderFormCount">订单数量</param>
        /// <param name="provider">供应商</param>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="stockCount">库存数量</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertList(string yearAndMonth,int goodsID, decimal orderFormCount,
            string provider, string bargainNumber,
            decimal stockCount, out string error)
        {
            try
            {
                error = null;

                Hashtable paramTable = new Hashtable();

                paramTable.Add("@Ny", yearAndMonth);
                paramTable.Add("@GoodsID", goodsID);
                paramTable.Add("@OrderFormCount", orderFormCount);
                paramTable.Add("@Provider", provider);
                paramTable.Add("@BargainNumber", bargainNumber);
                paramTable.Add("@StockCount", stockCount);

                Dictionary<OperateCMD, object> dicOperateCMD =
                    m_dbOperate.RunProc_CMD("AddWebForOrderFormList", paramTable);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                    return false;
                }

            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;
        }

        /// <summary>
        /// 插入主表
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>插入成功返回True，插入失败返回False</returns>
        bool InsertBill(string yearAndMonth,out string error)
        {
            try
            {
                error = null;
                Hashtable paramTable = new Hashtable();

                paramTable.Add("@Ny", yearAndMonth);

                Dictionary<OperateCMD, object> dicOperateCMD =
                    m_dbOperate.RunProc_CMD("AddWebForOrderFormBill", paramTable);

                if (!Convert.ToBoolean(dicOperateCMD[OperateCMD.Return_OperateResult]))
                {
                    error = Convert.ToString(dicOperateCMD[OperateCMD.Return_Errmsg]);
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获得最小采购量，最小采购量，采购份额的数据表
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="provider">供应商</param>
        /// <returns>返货获得的数据集</returns>
        DataRow GetLeast(int goodsID, string provider)
        {
            string strSql = "select * from View_B_GoodsLeastPackAndStock where 物品ID = "
                + goodsID + " and 供应商 = '" + provider + "' order by 采购份额 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return null;
            }
            else
            {
                return dt.Rows[0];
            }
        }

        /// <summary>
        /// 取出报缺数据
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回获得的数据集</returns>
        DataTable GetGoodsLostTable(string yearAndMonth,out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from View_S_PurchasingList where 年月 = '"
                    + yearAndMonth + "' and 第二月订货总数 > 0";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                return dt;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return null;
            }
        }

        /// <summary>
        /// 检查单据是否已完成
        /// </summary>
        /// <param name="yearAndMonth">年月字符串</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>单据未完成返回True，单据已完成返回False</returns>
        public bool IsFinish(string yearAndMonth,out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_PurchasingBill where Ny = '" + yearAndMonth + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt != null && dt.Rows.Count > 0
                    && dt.Rows[0]["DJZT"].ToString() == "单据已完成")
                {
                    error = "此年月的采购计划已存在，并且计划已完成，无需获取新计划";
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 检查是否具备生成采购计划的条件
        /// </summary>
        /// <param name="yearAndMonth">年月</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>都具备条件返回TRUE，返回FALSE不具备条件</returns>
        public bool IsQualified(string yearAndMonth, out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_MarketingPlanBill where YearAndMonth = '"
                    + yearAndMonth + "' and DJZT = '等待采购计划批准'";

                DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count == 0)
                {
                    error = "要求此月度的营销要货计划的单据状态为【等待采购计划批准】，才可生成采购计划";
                    return false;
                }

                DateTime dtStart = new DateTime();
                DateTime dtEnd = new DateTime();
                DateTime dtNow = Convert.ToDateTime(yearAndMonth.Substring(0, 4) + "-" + yearAndMonth.Substring(4, 2) + "-01 00:00:00");

                ServerTime.GetMonthlyBalance(dtNow, out dtStart, out dtEnd);

                strSql = "select * from S_ProductPlan where DJZT = '等待采购计划批准' "+
                    "and PlanTime >= '" + dtStart + "' and PlanTime <= '" + dtEnd + "'";

                dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtTemp.Rows.Count == 0)
                {
                    error = "要求此月度的生产计划的单据状态为【等待采购计划批准】，才可生成采购计划";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }

            return true;

        }

        /// <summary>
        /// 是否允许创建采购计划
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="nY">年月</param>
        /// <returns>允许返回True ,不允许返回False</returns>
        public bool IsAllowCreate(DateTime startTime,DateTime endTime,string nY)
        {
            bool blProduct = false;

            bool blMarketing = false;

            string strSql = "select * from S_ProductPlan where PlanTime >= '" + startTime 
                + "' and PlanTime <= '" + endTime + "' and DJZT = '等待采购计划批准'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count > 0)
            {
                blProduct = true;
            }

            strSql = "select * from S_MarketingPlanBill where YearAndMonth = '" + nY + "'  and DJZT = '等待采购计划批准'";

            dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dtTemp.Rows.Count > 0)
            {
                blMarketing = true;
            }

            if (blProduct && blMarketing)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        DataTable GetSteps()
        {
            string strSql = @"select CalculationSteps from CG_ProcurementSteps order by CalculationSteps ";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        DataTable GetStepsID(decimal orderby)
        {
            string strSql = @"  select * from (select StepsID,CalculationSteps
                                FROM CG_ProcurementSteps) as a inner join View_CG_ProcurementPlan as b on a.StepsID = b.步骤ID
                                where CalculationSteps = " + orderby;

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 计算数据集
        /// </summary>
        /// <param name="mathTable">数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        bool MathTable(ref DataTable mathTable,out string error)
        {
            error = null;

            try
            {
                GlobalObject.DynamicCompiler dyc = new DynamicCompiler();

                DateTime dtStart = ServerTime.Time;

                DataTable dtSteps = GetSteps();

                //计算步骤for循环
                foreach (DataRow drSteps in dtSteps.Rows)
                {
                    DataTable dtFormula = GetStepsID(Convert.ToDecimal(drSteps["CalculationSteps"]));

                    //循环采购计划的物品匹配计算物品项目
                    foreach (DataRow drMath in mathTable.Rows)
                    {
                        if (dtFormula.Select("物品ID in (0," + Convert.ToInt32(drMath["物品ID"]) + ")").Length > 0)
                        {
                            //计算步骤中的计算物品项目for循环
                            foreach (DataRow drFormula in dtFormula.Rows)
                            {
                                //匹配
                                if (drMath["物品ID"].ToString() == drFormula["物品ID"].ToString()
                                    || drFormula["物品ID"].ToString() == "0")
                                {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();

                                    //将原始数据以及对应的列存入字典
                                    foreach (DataColumn col in mathTable.Columns)
                                    {
                                        dic.Add("[" + col.ColumnName.ToString() + "]", drMath[col]);
                                    }

                                    string strMathFormunla = drFormula["计算公式"].ToString();

                                    //根据公式添加更多的字典项目以及数据
                                    GetValue(ref dic, ref strMathFormunla, mathTable);

                                    //数据源：字典，公式；得出：最终计算数据
                                    object objValue = dyc.DynamicMath(dic, drFormula["计算公式"].ToString(), out error);

                                    if (objValue == null)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        drMath[drFormula["计算字段"].ToString()] = objValue;
                                    }
                                }
                            }
                        }
                    }
                }

                DateTime dtEnd = ServerTime.Time;

                double dbMin = dtStart.Subtract(dtEnd).TotalMinutes;

                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        ///// <summary>
        ///// 计算数据集
        ///// </summary>
        ///// <param name="mathTable">数据集</param>
        ///// <param name="error">错误信息</param>
        ///// <returns>成功返回True，失败返回False</returns>
        //bool MathTable(ref DataTable mathTable, out string error)
        //{
        //    error = null;

        //    GlobalObject.DynamicCompiler dyc = new DynamicCompiler();

        //    DataTable dtFormula = GetProcurementPlanView();

        //    foreach (DataRow dr in mathTable.Rows)
        //    {
        //        DataRow[] drTemp1 = dtFormula.Select("物品ID = " + Convert.ToInt32(dr["物品ID"]));

        //        if (drTemp1 == null || drTemp1.Length == 0)
        //        {
        //            continue;
        //        }

        //        Dictionary<string, object> dic = new Dictionary<string, object>();

        //        foreach (DataColumn col in mathTable.Columns)
        //        {
        //            dic.Add("[" + col.ColumnName.ToString() + "]", dr[col]);
        //        }

        //        int intMath = GetMaxProcurementStep();

        //        for (int i = 1; i <= intMath; i++)
        //        {
        //            DataRow[] drTemp = dtFormula.Select("物品ID = " + Convert.ToInt32(dr["物品ID"]) + " and 计算顺序 = " + i);

        //            DataRow drFormula = null;

        //            if (drTemp == null || drTemp.Length == 0)
        //            {
        //                DataRow[] drTempRows = dtFormula.Select("物品ID = 0 and 计算顺序 = " + i);

        //                if (drTempRows == null || drTempRows.Length == 0)
        //                {
        //                    continue;
        //                }
        //                else
        //                {
        //                    drFormula = dtFormula.Select("物品ID = 0 and 计算顺序 = " + i)[0];
        //                }
        //            }
        //            else
        //            {
        //                drFormula = drTemp[0];
        //            }

        //            string strMathFormunla = drFormula["MathFormula"].ToString();

        //            GetValue(ref dic, ref strMathFormunla, mathTable);

        //            object objValue = dyc.DynamicMath(dic, drFormula["MathFormula"].ToString(), out error);

        //            if (objValue == null)
        //            {
        //                return false;
        //            }
        //            else
        //            {
        //                dr[drFormula["MathColumn"].ToString()] = objValue;
        //            }

        //            dic["[" + drFormula["MathColumn"].ToString() + "]"] = objValue;
        //        }

        //        dr["第一月订货数"] = Convert.ToDecimal(dr["第一月订货总数"]) * Convert.ToDecimal(dr["采购份额"]) * Convert.ToDecimal(dr["装配采购份额"]);
        //        dr["第二月订货数"] = Convert.ToDecimal(dr["第二月订货总数"]) * Convert.ToDecimal(dr["采购份额"]) * Convert.ToDecimal(dr["装配采购份额"]);
        //        dr["第三月订货数"] = Convert.ToDecimal(dr["第三月订货总数"]) * Convert.ToDecimal(dr["采购份额"]) * Convert.ToDecimal(dr["装配采购份额"]);
        //    }

        //    return true;
        //}

        /// <summary>
        /// 获得数据库数据
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="dataColumn">数据列名</param>
        /// <returns>返回object</returns>
        object GetDataValue(int goodsID,string dataColumn)
        {
            object obj = new object();

            string strSql = "";

            DataTable dtTemp = new DataTable();

            switch (dataColumn)
            {
                case "库存数":
                    strSql = @" select SUM(ExistCount) as ExistCount from S_Stock "+
							  " where GoodsStatus = 0 and GoodsID = "+ goodsID +"group by GoodsID";

                    dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    break;
                case "安全库存数":
                    strSql = @"select SafeStockCount from S_SafeStock where GoodsID = " + goodsID;

                    dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    break;
                case "已下订单未到货数":
                    strSql = @" select Sum(报检数量) as SumDeclareCount from View_S_CheckOutInDepotBill  "+
							  " where 单据状态 not in ('新建单据','已报废','已入库') and 单据状态 not like '%回退%' "+
							  " and 供货单位 not like '%SYS%' and 批次号 not like '%CGY%' and 物品ID = " + goodsID + " group by 物品ID";

                    dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    break;
                default:
                    break;
            }

            if (dtTemp.Rows.Count == 0 || dtTemp.Rows[0][0].ToString() == "")
            {
                obj = "0";
            }
            else
            {
                obj = dtTemp.Rows[0][0];
            }

            return obj;
        }

        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="dic">数据字典</param>
        /// <param name="mathFormunla">计算公式</param>
        /// <param name="dtTemp">数据集</param>
        void GetValue(ref Dictionary<string, object> dic, ref string mathFormunla, DataTable dtTemp)
        {
            if (mathFormunla.Contains("."))
            {
                int index = mathFormunla.IndexOf(".");
                string strTemp = mathFormunla.Substring(index + 1).Substring(0, mathFormunla.Substring(index + 1).IndexOf("]"));

                if (dic.Keys.Contains("[" + strTemp + "]"))
                {
                    string strGoodsID = mathFormunla.Substring(0, index).Substring(mathFormunla.Substring(0, index).LastIndexOf('[') + 1);
                    string dicKey = "[" + strGoodsID + "." + strTemp + "]";

                    F_GoodsPlanCost modelInf = m_serverGoods.GetGoodsInfo(Convert.ToInt32(strGoodsID));

                    object objDicValue = null;

                    DataRow[] drTemp2 = dtTemp.Select("图号型号 = '" + modelInf.GoodsCode + "' and 物品名称 = '" + modelInf.GoodsName + "' and 规格 = '" + modelInf.Spec + "'");

                    if (drTemp2 == null || drTemp2.Length != 1)
                    {
                        objDicValue = GetDataValue(Convert.ToInt32(strGoodsID), strTemp);
                    }
                    else
                    {
                        objDicValue = drTemp2[0][strTemp];
                    }

                    if (dic.Keys.Contains(dicKey))
                    {
                        dic[dicKey] = objDicValue;
                    }
                    else
                    {
                        dic.Add(dicKey, objDicValue);
                    }
                }

                mathFormunla = mathFormunla.Remove(index, 1);

                GetValue(ref dic, ref mathFormunla, dtTemp);
            }
            else
            {
                return;
            }
        }

        /// <summary>
        /// 获得计算步骤
        /// </summary>
        /// <returns>返回计算步骤TABLE</returns>
        public DataTable GetMathSteps()
        {
            string strSql = "select * from View_CG_ProcurementSteps order by 计算顺序";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 操作采购计划的计算步骤
        /// </summary>
        /// <param name="procurementSteps">新数据</param>
        /// <param name="mode">操作模式</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool OperatorMathSteps(CG_ProcurementSteps procurementSteps,CE_OperatorMode mode, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                CG_ProcurementSteps lnqProcurement = new CG_ProcurementSteps();

                var varData = from a in ctx.CG_ProcurementSteps
                              where a.StepsID == procurementSteps.StepsID
                              select a;

                if (varData.Count() == 1)
                {
                    lnqProcurement = varData.Single();
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                switch (mode)
                {
                    case CE_OperatorMode.添加:

                        ctx.CG_ProcurementSteps.InsertOnSubmit(procurementSteps);

                        break;
                    case CE_OperatorMode.修改:

                        if (lnqProcurement != null)
                        {
                            lnqProcurement.CalculationSteps = procurementSteps.CalculationSteps;
                            lnqProcurement.StepsName = procurementSteps.StepsName;
                            lnqProcurement.Remark = procurementSteps.Remark;
                        }

                        break;
                    case CE_OperatorMode.删除:

                        if (lnqProcurement != null)
                        {
                            ctx.CG_ProcurementSteps.DeleteOnSubmit(lnqProcurement);
                        }

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

    }
}
