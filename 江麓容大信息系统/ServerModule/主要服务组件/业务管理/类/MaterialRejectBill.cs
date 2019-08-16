/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  MaterialRejectBill.cs
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
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using ServerModule;
using DBOperate;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 采购退货单管理类
    /// </summary>
    class MaterialRejectBill :BasicServer, IMaterialRejectBill
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();


        /// <summary>
        /// 获取采购退货单的单据到票标志
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billID">采购退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回采购退货单是否到票标志</returns>
        public int SetHavingInvoiceReturn(DepotManagementDataContext ctx, string billID, out string error)
        {
            int intFlagA = 0;
            int intFlagB = 0;
            int intMath = 0;
            error = null;

            try
            {
                var varData = from a in ctx.S_MaterialListRejectBill
                              where a.Bill_ID == billID
                              select a;

                foreach (S_MaterialListRejectBill item in varData)
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
        /// 获取采购退货单的单据到票标志
        /// </summary>
        /// <param name="billID">采购退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回采购退货单是否到票标志</returns>
        public int SetHavingInvoiceReturn(string billID, out string error)
        {
            int intFlagA = 0;
            int intFlagB = 0;
            int intMath = 0;
            error = null;

            try
            {
                string strSql = "select * from  S_MaterialListRejectBill where Bill_ID = '" + billID + "'";
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
            var varData = from a in ctx.S_MaterialRejectBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_MaterialRejectBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获取采购退货单信息
        /// </summary>
        /// <param name="returnBill">返回的采购退货单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料出库信息</returns>
        public bool GetAllBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = authorization.Query("采购退货单查询", null);
            }
            else
            {
                qr = authorization.Query("采购退货单查询", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnBill = qr;
            return true;
        }

        /// <summary>
        /// 获取采购退货单视图信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        public View_S_MaterialRejectBill GetBillView(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from r in dataContxt.View_S_MaterialRejectBill
                         where r.退货单号 == billNo
                         select r;

            if (result.Count() > 0)
            {
                return result.Single();
            }

            return null;
        }

        /// <summary>
        /// 添加采购退货单
        /// </summary>
        /// <param name="bill">退货单信息</param>
        /// <param name="returnBill">返回更新后的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        public bool AddBill(S_MaterialRejectBill bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(bill.Bill_ID))
                {
                    throw new Exception("【单据号】获取失败，请重新再试");
                }

                dataContxt.S_MaterialRejectBill.InsertOnSubmit(bill);
                dataContxt.SubmitChanges();

                if (!GetAllBill(out returnBill, out error))
                {
                    return false;
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
        /// 修改采购退货单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">退货单信息</param>
        /// <param name="returnBill">返回更新后的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        public bool UpdateBill(S_MaterialRejectBill bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            try
            {
                
                var result = from r in dataContxt.S_MaterialRejectBill 
                             where r.Bill_ID == bill.Bill_ID 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的采购退货单信息，无法进行此操作", bill.Bill_ID);
                    return false;
                }

                S_MaterialRejectBill updateBill = result.Single();

                updateBill.Bill_Time = ServerModule.ServerTime.Time;
                updateBill.Provider = bill.Provider;
                updateBill.Reason = bill.Reason;
                updateBill.Remark = bill.Remark;
                updateBill.BillType = bill.BillType;

                dataContxt.SubmitChanges();

                if (!GetAllBill(out returnBill, out error))
                {
                    return false;
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
        /// 删除采购退货单
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除退货单号</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                IsolationManageBill serverIsolation = new IsolationManageBill();
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
                Table<S_MaterialRejectBill> table = dataContxt.GetTable<S_MaterialRejectBill>();

                var delRow = from c in table 
                             where c.Bill_ID == billNo 
                             select c;

                m_assignBill.CancelBillNo(dataContxt, "采购退货单", billNo);

                if (!serverIsolation.ClearBillDate(dataContxt, billNo, out error))
                {
                    return false;
                }

                table.DeleteAllOnSubmit(delRow);

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 上级领导审核
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回结果集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True,失败返回False</returns>
        public bool AuditBill(string billNo, out IQueryResult returnBill, out string error)
        {

            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialRejectBill
                             where r.Bill_ID == billNo
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的采购退货单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().BillStatus = MaterialRejectBillBillStatus.等待财务审核.ToString();
                result.Single().AuditDate = ServerModule.ServerTime.Time;
                result.Single().AuditPersonnel = BasicInfo.LoginName;

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 退货人提交单据(交给财务审批)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        public bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialRejectBill 
                             where r.Bill_ID == billNo 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的采购退货单信息，无法进行此操作", billNo);
                    return false;
                }
               
                result.Single().BillStatus = MaterialRejectBillBillStatus.等待上级领导审核.ToString();
                result.Single().Bill_Time = ServerModule.ServerTime.Time;

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 财务审批单据
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="name">审批人姓名</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        public bool FinanceAuthorizeBill(string billNo, string name, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialRejectBill 
                             where r.Bill_ID == billNo 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的采购退货单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().FinanceSignatory = name;
                result.Single().BillStatus = MaterialRejectBillBillStatus.等待仓管退货.ToString();

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }

        /// <summary>
        /// 修改关联的隔离单的单据状态
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billNo">退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        private bool UpdateIsolationBillStatus(DepotManagementDataContext context, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData1 = from a in context.S_MaterialListRejectBill
                               where a.Bill_ID == billNo
                               select a;

                foreach (S_MaterialListRejectBill item1 in varData1)
                {
                    var varData = from a in context.S_IsolationManageBill
                                  where a.GoodsID == item1.GoodsID
                                  && a.BatchNo == item1.BatchNo
                                  && a.Provider == item1.Provider
                                  && a.DJZT == "等待采购退货"
                                  select a;

                    if (varData.Count() > 0)
                    {
                        foreach (var item in varData)
                        {
                            S_IsolationManageBill lnqIsolation = item;
                            lnqIsolation.DJZT = "等待仓管调入";

                            string strMsg = string.Format("{0}号不合格品隔离处置单,请仓管处理", billNo.ToString());

                            m_billMessageServer.PassFlowMessage(billNo, strMsg,
                                m_billMessageServer.GetRoleStringForStorage(lnqIsolation.StorageID).ToString(), true);
                        }
                    }

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
        /// 检查已经关联了不合格品隔离单
        /// </summary>
        /// <param name="billID">退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>检测合格返回True，不合格返回False</returns>
        bool CheckBillIsInIsolationManageBill(string billID,out string error)
        {
            error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varBill = from a in ctx.S_MaterialRejectBill
                          where a.Bill_ID == billID
                          select a;

            S_MaterialRejectBill lnqBill = new S_MaterialRejectBill();

            if (varBill.Count() != 1)
            {
                return false;
            }
            else
            {
                lnqBill = varBill.Single();
            }

            MaterialListRejectBill serverRejectBill = new MaterialListRejectBill();

            string strSql = "select * from S_MaterialListRejectBill where Bill_ID = '" + billID + "'";

            DataTable dtBill = GlobalObject.DatabaseServer.QueryInfo(strSql);

            for (int i = 0; i < dtBill.Rows.Count; i++)
            {
                strSql = "select * from S_IsolationManageBill where GoodsID = "
                    + Convert.ToInt32(dtBill.Rows[i]["GoodsID"]) + " and BatchNo = '"
                    + dtBill.Rows[i]["BatchNo"].ToString() +"' and DJZT <> '已完成'";

                DataTable dtIsolation = GlobalObject.DatabaseServer.QueryInfo(strSql);

                if (dtIsolation.Rows.Count >0)
                {
                    strSql = "select * from S_IsolationManageBill where GoodsID = "
                    + Convert.ToInt32(dtBill.Rows[i]["GoodsID"]) + " and BatchNo = '"
                    + dtBill.Rows[i]["BatchNo"].ToString() + "' and DJZT <> '已完成'"
                    + "' and AssociateRejectBillID = '" + billID + "'";

                    DataTable dtIsolationGoods = GlobalObject.DatabaseServer.QueryInfo(strSql);

                    if (dtIsolation.Rows.Count == 0)
                    {
                        error = "不能对隔离且未关联隔离单的物品进行退货！";
                        return false;
                    }
                }

                if (!serverRejectBill.IsGoodsStockThan(Convert.ToInt32(dtBill.Rows[i]["GoodsID"]),
                    dtBill.Rows[i]["BatchNo"].ToString(),
                    Convert.ToDecimal(dtBill.Rows[i]["Amount"]), dtBill.Rows[i]["Provider"].ToString(), lnqBill.StorageID))
                {
                    error = "库存不足，无法退货！";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 完成采购退货单
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的采购退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        public bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_MaterialRejectBill 
                             where r.Bill_ID == billNo 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的采购退货单信息，无法进行此操作", billNo);
                    return false;
                }

                //if (!CheckBillIsInIsolationManageBill(billNo,out error))
                //{
                //    return false;
                //}

                S_MaterialRejectBill bill = result.Single();

                if (bill.BillStatus == MaterialRejectBillBillStatus.已完成.ToString())
                {
                    error = "单据不能重复退货";
                    return false;
                }

                bill.DepotManager = storeManager;
                bill.BillStatus = MaterialRejectBillBillStatus.已完成.ToString();
                bill.OutDepotDate = ServerTime.Time;

                if (bill.BillType.ToString() == "总仓库退货单")
                {

                    //操作账务信息与库存信息
                    OpertaionDetailAndStock(dataContxt, bill);

                    ////更新隔离单单据状态
                    //if (!UpdateIsolationBillStatus(dataContxt, billNo, out error))
                    //{
                    //    return false;
                    //}
                }
                else
                {
                    if (!SetScrapFlag(dataContxt,bill.Bill_ID,out error))
                    {
                        return false;
                    }
                }

                // 正式使用单据号
                m_assignBill.UseBillNo(dataContxt, "采购退货单", bill.Bill_ID);

                dataContxt.SubmitChanges();

                return GetAllBill(out returnBill, out error);
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
        public void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_MaterialRejectBill bill)
        {
            MaterialListRejectBill rejectService = new MaterialListRejectBill();
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_MaterialListRejectBill
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            if (result == null || result.Count() == 0)
            {
                throw new Exception("获取单据信息失败");
            }

            foreach (var item in result)
            {
                S_InDepotDetailBill detailInfo = rejectService.AssignDetailInfo(dataContext, bill, item);
                S_Stock stockInfo = rejectService.AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 修改报废单退货标志
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billID">采购退货单号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>修改成功返回True，修改失败返回False</returns>
        private bool SetScrapFlag(DepotManagementDataContext context,string billID,out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_MaterialListRejectBill where Bill_ID = '" + billID + "'";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var varData = from a in context.S_ScrapGoods
                                  where a.Bill_ID == dt.Rows[i]["AssociateID"].ToString()
                                  && a.GoodsID == Convert.ToInt32(dt.Rows[i]["GoodsID"].ToString())
                                  && a.BatchNo == dt.Rows[i]["BatchNo"].ToString()
                                  select a;

                    if (varData.Count() != 1)
                    {
                        error = "记录不唯一";
                        return false;
                    }
                    else
                    {
                        S_ScrapGoods lnqScrapGoods = varData.Single();

                        lnqScrapGoods.IsReject = "1";

                        context.SubmitChanges();
                    }
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
        /// 回退单据
        /// </summary>
        /// <param name="djh">回退单据号</param>
        /// <param name="billStatus">回退单据状态</param>
        /// <param name="returnBill">返回的查询结果集</param>
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

                var varData = from a in ctx.S_MaterialRejectBill
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_MaterialRejectBill lnqMRequ = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":

                            strMsg = string.Format("{0}号采购退货单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqMRequ.FillInPersonnelCode, false);

                            lnqMRequ.BillStatus = "新建单据";
                            lnqMRequ.FinanceSignatory = null;
                            break;

                        case "等待财务审核":

                            strMsg = string.Format("{0}号采购退货单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, 
                                UniversalFunction.GetPersonnelCode( lnqMRequ.FinanceSignatory), false);

                            lnqMRequ.BillStatus = "等待质检";
                            lnqMRequ.FinanceSignatory = null;
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
