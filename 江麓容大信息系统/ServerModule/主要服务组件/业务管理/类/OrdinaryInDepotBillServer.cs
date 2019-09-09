/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  OrdinaryInDepotBillServer.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/09/27
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/09/27 作者: 夏石友 当前版本: V1.00
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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using DBOperate;
using PlatformManagement;
using System.Transactions;
using GlobalObject;


namespace ServerModule
{
    /// <summary>
    /// 普通入库单管理类
    /// </summary>
    class OrdinaryInDepotBillServer : BasicServer, IOrdinaryInDepotBillServer
    {
        /// <summary>
        /// 工装信息服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 计划价格服务
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据唯一码
        /// </summary>
        int m_billUniqueID = -1;

        /// <summary>
        /// 获取普通入库单的单据到票标志
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回普通入库单是否到票的标志</returns>
        public int GetHavingInvoice(DepotManagementDataContext ctx, string billID, out string error)
        {
            int intFlagA = 0;
            int intFlagB = 0;
            int intMath = 0;

            error = null;

            try
            {
                var varData = from a in ctx.S_OrdinaryInDepotGoodsBill
                              where a.Bill_ID == billID
                              select a;

                foreach (S_OrdinaryInDepotGoodsBill item in varData)
                {
                    if (item.HavingInvoice)
                    {
                        intFlagA = 1;
                    }
                    else
                    {
                        intFlagB = 1;
                    }
                }

                if (intFlagA == 1 && intFlagB == 0)
                {
                    intMath = 2;
                }
                else if (intFlagA == 1 && intFlagB == 1)
                {
                    intMath = 1;
                }
                else
                {
                    intMath = 0;
                }

                return intMath;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 4;
            }
        }

        /// <summary>
        /// 获取普通入库单的单据到票标志
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回普通入库单是否到票的标志</returns>
        public int GetHavingInvoice(string billID, out string error)
        {
            int intFlagA = 0;
            int intFlagB = 0;
            int intMath = 0;

            error = null;

            try
            {
                string strSql = "select * from  S_OrdinaryInDepotGoodsBill where Bill_ID = '" + billID + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    if (dt.Rows[i]["HavingInvoice"].ToString() == "True")
                    {
                        intFlagA = 1;
                    }
                    else
                    {
                        intFlagB = 1;
                    }

                }

                if (intFlagA == 1 && intFlagB == 0)
                {
                    intMath = 2;
                }
                else if (intFlagA == 1 && intFlagB == 1)
                {
                    intMath = 1;
                }
                else
                {
                    intMath = 0;
                }

                return intMath;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return 4;
            }
        }

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_OrdinaryInDepotBill
                          where a.Bill_ID == billNo
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_OrdinaryInDepotBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取普通入库单信息
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <returns>成功返回获取到的单据信息,失败返回null</returns>
        public S_OrdinaryInDepotBill GetBill(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.S_OrdinaryInDepotBill where r.Bill_ID == billNo select r;

            if (result.Count() == 0)
            {
                return null;
            }

            return result.Single();
        }

        /// <summary>
        /// 获取普通入库单信息
        /// </summary>
        /// <param name="returnInfo">入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("普通入库单查询", null);
            }
            else
            {
                qr = authorization.Query("普通入库单查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            if (m_billUniqueID < 0)
            {
                IBillTypeServer server = ServerModuleFactory.GetServerModule<IBillTypeServer>();
                BASE_BillType billType = server.GetBillTypeFromName("普通入库单");

                if (billType == null)
                {
                    error = "获取不到单据类型信息";
                    return false;
                }

                m_billUniqueID = billType.UniqueID;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 添加普通入库单
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool AddBill(S_OrdinaryInDepotBill bill, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                string strSql = @"select Provider from B_OrderFormInfo where OrderFormNumber = '"+ bill.OrderBill_ID +"' ";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dt.Rows.Count == 0)
                {
                    error = "订单号有问题，请核实";
                    return false;
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(bill.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
                }

                bill.Provider = dt.Rows[0][0].ToString();

                dataContxt.S_OrdinaryInDepotBill.InsertOnSubmit(bill);
                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 插入工装所有信息
        /// </summary>
        /// <param name="ordinarybill">普通入库单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool CreateNewFrockMessage(S_OrdinaryInDepotBill ordinarybill, out string error)
        {
            error = null;

            FrockProvingReport serverFrockProvingReport = new FrockProvingReport();

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_OrdinaryInDepotGoodsBill
                              where a.Bill_ID == ordinarybill.Bill_ID
                              select a;

                foreach (var item in varData)
                {
                    for (int i = 0; i < item.Amount; i++)
                    {

                        string strFrockNumber = m_serverFrockStandingBook.GetNewFrockNumber();

                        S_FrockProvingReport lnqReport = new S_FrockProvingReport();

                        lnqReport.DJH = m_assignBill.AssignNewNo(serverFrockProvingReport, "工装验证报告单");
                        lnqReport.DJZT = "等待检验要求";
                        lnqReport.BillType = "入库检验";
                        lnqReport.BZRQ = ServerTime.Time;
                        lnqReport.ConnectBillNumber = ordinarybill.Bill_ID;
                        lnqReport.FrockNumber = strFrockNumber;
                        lnqReport.GoodsID = item.GoodsID;

                        if (!serverFrockProvingReport.AddBill(lnqReport, null, out error))
                        {
                            return false;
                        }

                        S_FrockStandingBook lnqBook = new S_FrockStandingBook();

                        lnqBook.GoodsID = item.GoodsID;
                        lnqBook.FrockNumber = strFrockNumber;
                        lnqBook.Designer = UniversalFunction.GetPersonnelName(ordinarybill.Designer);

                        if (!m_serverFrockStandingBook.UpdateFrockStandingBook(lnqBook, null, out error))
                        {
                            return false;
                        }
                        
                        View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(lnqBook.GoodsID);

                        m_billMessageServer.DestroyMessage(lnqReport.DJH);
                        m_billMessageServer.SendNewFlowMessage(lnqReport.DJH,
                            string.Format("【工装编号】：{0} 【图号型号】：{1} 【物品名称】：{2}，※※※ 等待【工艺人员】处理", 
                            lnqReport.FrockNumber, goodsInfo.图号型号, goodsInfo.物品名称),
                            CE_RoleEnum.工艺人员);
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
        /// 采购员提交单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="status">提交后的单据状态</param>
        /// <param name="returnInfo">返回更新后重新查询的领料单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加领料单</returns>
        public bool SubmitNewBill(string billNo, OrdinaryInDepotBillStatus status, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_OrdinaryInDepotBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的普通入库单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().BillStatus = status.ToString();
                result.Single().Bill_Time = ServerModule.ServerTime.Time;

                if (!m_serverFrockStandingBook.DeleteFrockOrdinaryInDepotBill(dataContxt, billNo, out error))
                {
                    return false;
                }

                //插入工装信息
                IDepotTypeForPersonnel serverDepot = ServerModuleFactory.GetServerModule<IDepotTypeForPersonnel>();

                if (serverDepot.GetDepotInfo(result.Single().Depot).DepotName == GlobalObject.CE_GoodsType.工装.ToString()
                    && !CreateNewFrockMessage(result.Single(), out error))
                {
                    return false;
                }

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交质量信息
        /// </summary>
        /// <param name="billInfo">取单据中质量信息部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool SubmitQualityInfo(S_OrdinaryInDepotBill billInfo, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_OrdinaryInDepotBill where r.Bill_ID == billInfo.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的普通入库单！", billInfo.Bill_ID);
                    return false;
                }

                S_OrdinaryInDepotBill updateBill = result.Single();

                updateBill.Checker = billInfo.Checker;
                updateBill.QualityEligibilityFlag = billInfo.QualityEligibilityFlag;
                updateBill.BillStatus = billInfo.BillStatus;

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交工装验证信息
        /// </summary>
        /// <param name="billInfo">取单据中工装验证信息部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool SubmitMachineValidationInfo(S_OrdinaryInDepotBill billInfo, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                var result = from r in dataContxt.S_OrdinaryInDepotBill where r.Bill_ID == billInfo.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的普通入库单！", billInfo.Bill_ID);
                    return false;
                }

                S_OrdinaryInDepotBill updateBill = result.Single();

                updateBill.MachineValidationID = billInfo.MachineValidationID;
                updateBill.MachineManager = billInfo.MachineManager;
                updateBill.AllowInDepot = billInfo.AllowInDepot;
                updateBill.BillStatus = billInfo.BillStatus;

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据工装验证报告表更新工装台帐的库存状态
        /// </summary>
        /// <param name="context">LINQ数据上下文</param>
        /// <param name="billNo">普通入库单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        bool UpdateFrockStockStatus(DepotManagementDataContext context, string billNo, out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_OrdinaryInDepotGoodsBill where Bill_ID = '" + billNo + "'";

                DataTable dtOrdinary = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dtOrdinary.Rows.Count; i++)
                {
                    strSql = "select Count(*) from S_FrockProvingReport where ConnectBillNumber = '"
                        + billNo + "' and IsInStock = 1  and GoodsID = " + Convert.ToInt32(dtOrdinary.Rows[i]["GoodsID"]);

                    DataTable dtFrock = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (Convert.ToDecimal(dtOrdinary.Rows[i]["Amount"]) != Convert.ToDecimal(dtFrock.Rows[0][0]))
                    {
                        error = "入库数量与工装检验报告单所需入库数量不符";
                        return false;
                    }
                }

                strSql = "select * from S_FrockProvingReport where ConnectBillNumber = '" + billNo + "' and IsInStock = 1 ";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (!m_serverFrockStandingBook.UpdateFrockStandingBookStock(context, dt, true, out error))
                {
                    return false;
                }

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    S_FrockOperation lnqOperation = new S_FrockOperation();

                    lnqOperation.BillID = billNo;
                    lnqOperation.BillTime = ServerTime.Time;
                    lnqOperation.BillType = "入库";
                    lnqOperation.FrockNumber = dt.Rows[i]["FrockNumber"].ToString();
                    lnqOperation.GoodsID = Convert.ToInt32(dt.Rows[i]["GoodsID"]);
                    lnqOperation.IsTrue = true;

                    context.S_FrockOperation.InsertOnSubmit(lnqOperation);
                }

                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 提交入库信息
        /// </summary>
        /// <param name="inDepotInfo">入库信息, 只取其中入库部分</param>
        /// <param name="returnInfo">添加完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作结果</returns>
        public bool SubmitInDepotInfo(S_OrdinaryInDepotBill inDepotInfo, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_OrdinaryInDepotBill where r.Bill_ID == inDepotInfo.Bill_ID select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的普通入库单！", inDepotInfo.Bill_ID);
                    return false;
                }

                S_OrdinaryInDepotBill bill = result.Single();

                if (bill.BillStatus == CheckInDepotBillStatus.已入库.ToString())
                {
                    error = string.Format("入库单号为 [{0}] 单据状态为已入库", inDepotInfo.Bill_ID);
                    return false;
                }

                bill.DepotManager = inDepotInfo.DepotManager;
                bill.BillStatus = inDepotInfo.BillStatus;
                bill.InDepotDate = ServerTime.Time;

                // 添加信息到入库明细表
                OpertaionDetailAndStock(dataContxt, bill);

                //更新工装库存状态

                IDepotTypeForPersonnel serverDepot = ServerModuleFactory.GetServerModule<IDepotTypeForPersonnel>();

                if (serverDepot.GetDepotInfo(bill.Depot).DepotName == GlobalObject.CE_GoodsType.工装.ToString()
                    && bill.Bill_Time > Convert.ToDateTime("2012-4-21")
                    && !UpdateFrockStockStatus(dataContxt, bill.Bill_ID, out error))
                {
                    return false;
                }

                // 正式使用单据号
                m_assignBill.UseBillNo(dataContxt, "普通入库单", bill.Bill_ID);

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 根据单据信息操作账务信息与库存信息
        /// </summary>
        /// <param name="dataContext">数据上下文</param>
        /// <param name="bill">单据信息</param>
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_OrdinaryInDepotBill bill)
        {
            OrdinaryInDepotGoodsBill serverOrdinaryBill = new OrdinaryInDepotGoodsBill();
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_OrdinaryInDepotGoodsBill
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            foreach (var item in result)
            {
                S_InDepotDetailBill detailInfo = serverOrdinaryBill.AssignDetailInfo(dataContext, bill, item);
                S_Stock stockInfo = serverOrdinaryBill.AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 修改单据
        /// </summary>
        /// <param name="bill">单据信息</param>
        /// <param name="returnInfo">修改完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateBill(S_OrdinaryInDepotBill bill, out IQueryResult returnInfo, out string error)
        {
            error = null;
            returnInfo = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from c in dataContxt.S_OrdinaryInDepotBill where c.Bill_ID == bill.Bill_ID select c;

                if (result.Count() > 0)
                {
                    S_OrdinaryInDepotBill updateBill = result.Single();

                    updateBill.Bill_Time = bill.Bill_Time;
                    //updateBill.OrderBill_ID = bill.OrderBill_ID;
                    updateBill.Invoice_ID = bill.Invoice_ID;
                    updateBill.Cess = bill.Cess;
                    updateBill.Provider = bill.Provider;
                    updateBill.Proposer = bill.Proposer;
                    updateBill.Designer = bill.Designer;
                    updateBill.NeedQualityAffirmance = bill.NeedQualityAffirmance;
                    updateBill.NeedMachineManager = bill.NeedMachineManager;
                    updateBill.Depot = bill.Depot;
                    updateBill.Checker = bill.Checker;
                    updateBill.Remark = bill.Remark;

                    dataContxt.SubmitChanges();
                    return GetAllBill(out returnInfo, out error);
                }

                error = string.Format("找不到 {0} 单据, 无法进行此操作", bill.Bill_ID);
                return false;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 更新指定单据物品类别
        /// </summary>
        /// <param name="billNo">单据编号</param>
        /// <param name="depotType">物品类别</param>
        /// <param name="returnInfo">操作完毕后查询数据库的返回结果</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool UpdateGoodsType(string billNo, string depotType, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                OrdinaryInDepotGoodsBill serverOrdinaryBill = new OrdinaryInDepotGoodsBill();

                var result = from r in dataContxt.S_OrdinaryInDepotBill where r.Bill_ID == billNo select r;

                if (result.Count() == 0)
                {
                    error = string.Format("找不到入库单号为 [{0}] 的普通入库单！", billNo);
                    return false;
                }

                S_OrdinaryInDepotBill bill = result.Single();

                bill.Depot = depotType;

                IQueryable<S_OrdinaryInDepotGoodsBill> goodsBill = serverOrdinaryBill.GetGoodsInfo(billNo);

                foreach (var item in goodsBill)
                {
                    View_F_GoodsPlanCost goodsPlanCost = m_serverBasicGoods.GetGoodsInfoView(item.GoodsID);

                    if (goodsPlanCost != null)// && goodsPlanCost.日期 == ServerTime.Time.Date)
                    {
                        if (goodsPlanCost.录入员编码 != bill.BuyerCode)
                        {
                            error = string.Format("图号型号：{0}, 物品名称：{1}, 规格：{2} 的物品信息在基础物品信息表中由 {3} 创建而不是当前采购员录单时自动创建，无法进行修改，如要更改请与系统管理员联系",
                                goodsPlanCost.图号型号, goodsPlanCost.物品名称, goodsPlanCost.规格, goodsPlanCost.录入员编码);

                            return false;
                        }

                        //m_serverBasicGoods.UpdateGoodsType(dataContxt, item.GoodsID, depotType);
                    }
                }

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 删除普通入库单
        /// </summary>
        /// <param name="billNo">入库单号</param>
        /// <param name="returnInfo">普通入库单</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除普通入库单号</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<S_OrdinaryInDepotBill> table = dataContxt.GetTable<S_OrdinaryInDepotBill>();

                var delRow = from c in table
                             where c.Bill_ID == billNo
                             select c;

                table.DeleteAllOnSubmit(delRow);

                m_assignBill.CancelBillNo(dataContxt, "普通入库单", billNo);

                if (!m_serverFrockStandingBook.DeleteFrockOrdinaryInDepotBill(dataContxt, billNo, out error))
                {
                    return false;
                }

                dataContxt.SubmitChanges();

                return GetAllBill(out returnInfo, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="returnBill">返回回退后查询到的单据信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>回退成功返回True，回退失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                var varData = from a in ctx.S_OrdinaryInDepotBill
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_OrdinaryInDepotBill lnqMRequ = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号普通入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                lnqMRequ.BuyerCode, false);

                            lnqMRequ.BillStatus = "新建单据";
                            lnqMRequ.Checker = "0000";
                            lnqMRequ.MachineManager = "0000";

                            if (!m_serverFrockStandingBook.DeleteFrockOrdinaryInDepotBill(ctx, djh, out error))
                            {
                                return false;
                            }

                            break;

                        case "等待质检":

                            strMsg = string.Format("{0}号普通入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.Checker), false);

                            lnqMRequ.BillStatus = "等待质检";
                            lnqMRequ.Checker = "0000";

                            break;

                        case "等待工装验证":

                            strMsg = string.Format("{0}号普通入库单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.MachineManager), false);

                            lnqMRequ.BillStatus = "等待工装验证";
                            lnqMRequ.MachineManager = "0000";

                            break;

                        default:
                            break;
                    }

                    ctx.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
    }
}
