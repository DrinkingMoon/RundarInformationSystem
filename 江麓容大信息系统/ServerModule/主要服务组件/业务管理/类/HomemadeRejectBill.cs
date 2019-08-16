/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  HomemadeRejectBill.cs
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
using PlatformManagement;
using System.Data;
using System.Data.Linq;
using GlobalObject;

namespace ServerModule
{
    /// <summary>
    /// 自制件退货服务类
    /// </summary>
    class HomemadeRejectBill : BasicServer, IHomemadeRejectBill
    {
        /// <summary>
        /// 基础物品信息服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号分配服务
        /// </summary>
        IAssignBillNoServer m_assignBill = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();

        /// <summary>
        /// 检查某业务是否存在
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="billNo">业务号</param>
        /// <returns>存在返回true</returns>
        public bool IsExist(DepotManagementDataContext ctx, string billNo)
        {
            var varData = from a in ctx.S_HomemadeRejectBill
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
            string sql = "SELECT * FROM [DepotManagement].[dbo].[S_HomemadeRejectBill] where Bill_ID = '" + billNo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// 获得所有的自制件
        /// </summary>
        /// <returns>返回的查询结果集</returns>
        public DataTable GetHomemadePartInfo()
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[View_S_HomemadePartInfo]";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取自制件退货单信息
        /// </summary>
        /// <param name="returnBill">返回的自制件退货单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功获取领料出库信息</returns>
        public bool GetAllBill(out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            IAuthorization m_authorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = m_authorization.Query("自制件退货单查询", null);
            }
            else
            {
                qr = m_authorization.Query("自制件退货单查询", null, QueryResultFilter);
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
        /// 获取自制件退货单视图信息
        /// </summary>
        /// <param name="billNo">退货单号</param>
        /// <returns>成功返回获取领料出库信息, 失败返回null</returns>
        public DataTable GetBillView(string billNo)
        {
            string sql = "SELECT * FROM [DepotManagement].[dbo].[View_S_HomemadeRejectBill] where 退货单号 = '" + billNo + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 添加自制件退货单
        /// </summary>
        /// <param name="bill">添加的自制件退货单据信息</param>
        /// <param name="returnBill">返回更新后的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        public bool AddBill(S_HomemadeRejectBill bill, out IQueryResult returnBill, out string error)
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

                dataContxt.S_HomemadeRejectBill.InsertOnSubmit(bill);
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
        /// 修改自制件退货单(只修改编制人涉及信息)
        /// </summary>
        /// <param name="bill">修改的自制件退货单据信息</param>
        /// <param name="returnBill">返回更新后的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        public bool UpdateBill(S_HomemadeRejectBill bill, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectBill 
                             where r.Bill_ID == bill.Bill_ID 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的自制件退货单信息，无法进行此操作", bill.Bill_ID);
                    return false;
                }

                S_HomemadeRejectBill updateBill = result.Single();

                updateBill.Bill_Time = ServerModule.ServerTime.Time;
                updateBill.Provider = bill.Provider;
                updateBill.Reason = bill.Reason;
                updateBill.Remark = bill.Remark;

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
        /// 删除自制件退货单
        /// </summary>
        /// <param name="billNo">自制件退货单号</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功删除退货单号</returns>
        public bool DeleteBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<S_HomemadeRejectBill> table = dataContxt.GetTable<S_HomemadeRejectBill>();

                var delRow = from c in table 
                             where c.Bill_ID == billNo 
                             select c;

                if (!ClearDate(dataContxt, billNo, out error))
                {
                    return false;
                }

                table.DeleteAllOnSubmit(delRow);

                Table<S_HomemadeRejectList> tableList = dataContxt.GetTable<S_HomemadeRejectList>();

                var delList = from d in tableList 
                              where d.Bill_ID == billNo 
                              select d;

                tableList.DeleteAllOnSubmit(delList);

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
        /// 清除隔离单数据
        /// </summary>
        /// <param name="context">上下文数据集</param>
        /// <param name="billNo">单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool ClearDate(DepotManagementDataContext context, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_IsolationManageBill
                              where a.AssociateRejectBillID == billNo
                              select a;

                if (varData.Count() > 0)
                {
                    foreach (var item in varData)
                    {
                        S_IsolationManageBill lnqIsolation = item;

                        lnqIsolation.DJZT = "等待采购退货";
                        lnqIsolation.AssociateRejectBillID = null;
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
        /// 退货人提交单据(交给财务审批)
        /// </summary>
        /// <param name="billNo">自制件单据号</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        public bool SubmitNewBill(string billNo, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectBill 
                             where r.Bill_ID == billNo 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的自制件退货单信息，无法进行此操作", billNo);
                    return false;
                }

                result.Single().BillStatus = MaterialRejectBillBillStatus.等待财务审核.ToString();
                result.Single().Bill_Time = ServerTime.Time;
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
        /// <param name="billNo">自制件退货单据号</param>
        /// <param name="name">审批人姓名</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加自制件退货单</returns>
        public bool FinanceAuthorizeBill(string billNo, string name, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectBill where r.Bill_ID == billNo select r;

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
        /// <param name="context">上下文数据集</param>
        /// <param name="billNo">自制件退货单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool UpdateIsolationBillStatus(DepotManagementDataContext context, string billNo, out string error)
        {
            error = null;

            try
            {
                var varData = from a in context.S_IsolationManageBill
                              where a.AssociateRejectBillID == billNo
                              select a;

                if (varData.Count() > 0)
                {
                    foreach (var item in varData)
                    {

                        S_IsolationManageBill lnqIsolation = item;

                        if (lnqIsolation.DJZT != "等待采购退货")
                        {
                            error = "请重新确认退货物品所关联的隔离单单据状态";
                            return false;
                        }

                        lnqIsolation.DJZT = "等待仓管调入";

                        string strMsg = string.Format("{0}号不合格品隔离处置单,请仓管处理", billNo.ToString());

                        m_billMessageServer.PassFlowMessage(billNo, strMsg,
                            m_billMessageServer.GetRoleStringForStorage(lnqIsolation.StorageID).ToString(), true);
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
        /// 完成自制件退货单
        /// </summary>
        /// <param name="billNo">自制件退货单据号</param>
        /// <param name="storeManager">仓库管理员</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>返回是否成功添加采购退货单</returns>
        public bool FinishBill(string billNo, string storeManager, out IQueryResult returnBill, out string error)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from r in dataContxt.S_HomemadeRejectBill 
                             where r.Bill_ID == billNo 
                             select r;

                if (result.Count() == 0)
                {
                    error = string.Format("没有找到单据号为 {0} 的自制件退货单信息，无法进行此操作", billNo);
                    return false;
                }

                S_HomemadeRejectBill bill = result.Single();

                if (bill.BillStatus == MaterialRejectBillBillStatus.已完成.ToString())
                {
                    error = "单据不能重复退货";
                    return false;
                }

                bill.DepotManager = storeManager;
                bill.BillStatus = MaterialRejectBillBillStatus.已完成.ToString();
                bill.OutDepotDate = ServerTime.Time;

                IHomemadeRejectBill goodsServer = ServerModuleFactory.GetServerModule<IHomemadeRejectBill>();

                //操作账务信息与库存信息
                OpertaionDetailAndStock(dataContxt, bill);

                //更新隔离单单据状态
                if (!UpdateIsolationBillStatus(dataContxt, billNo, out error))
                {
                    return false;
                }

                // 正式使用单据号
                m_assignBill.UseBillNo(dataContxt, "自制件退货单", bill.Bill_ID);
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
        void OpertaionDetailAndStock(DepotManagementDataContext dataContext, S_HomemadeRejectBill bill)
        {
            IFinancialDetailManagement serverDetail =
                ServerModule.ServerModuleFactory.GetServerModule<IFinancialDetailManagement>();

            var result = from r in dataContext.S_HomemadeRejectList
                         where r.Bill_ID == bill.Bill_ID
                         select r;

            foreach (var item in result)
            {
                S_InDepotDetailBill detailInfo = AssignDetailInfo(dataContext, bill, item);
                S_Stock stockInfo = AssignStockInfo(dataContext, bill, item);

                if (detailInfo == null || stockInfo == null)
                {
                    throw new Exception("获取账务信息或者库存信息失败");
                }

                serverDetail.ProcessInDepotDetail(dataContext, detailInfo, stockInfo);
            }
        }

        /// <summary>
        /// 赋值库存信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回库存信息对象</returns>
        S_Stock AssignStockInfo(DepotManagementDataContext dataContxt,S_HomemadeRejectBill bill, S_HomemadeRejectList item)
        {
            F_GoodsPlanCost info = m_basicGoodsServer.GetGoodsInfo(dataContxt, item.GoodsID);

            S_Stock stockInfo = new S_Stock();

            stockInfo.GoodsID = info.ID;
            stockInfo.GoodsCode = info.GoodsCode;
            stockInfo.GoodsName = info.GoodsName;
            stockInfo.Spec = info.Spec;
            stockInfo.Provider = item.Provider;
            stockInfo.BatchNo = item.BatchNo;
            stockInfo.ExistCount = item.Amount;
            stockInfo.StorageID = bill.StorageID;

            return stockInfo;
        }

        /// <summary>
        /// 赋值账务信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="bill">单据信息</param>
        /// <param name="item">明细信息</param>
        /// <returns>返回账务信息</returns>
        S_InDepotDetailBill AssignDetailInfo(DepotManagementDataContext dataContxt, S_HomemadeRejectBill bill, S_HomemadeRejectList item)
        {

            S_InDepotDetailBill detailBill = new S_InDepotDetailBill();

            detailBill.ID = Guid.NewGuid();
            detailBill.BillTime = (DateTime)bill.OutDepotDate;
            detailBill.FillInPersonnel = bill.FillInPersonnel;
            detailBill.Department = bill.Department;
            detailBill.FactPrice = -Math.Round(item.UnitPrice * item.Amount, 2);
            detailBill.FactUnitPrice = item.UnitPrice;
            detailBill.GoodsID = item.GoodsID;
            detailBill.BatchNo = item.BatchNo;
            detailBill.Provider = item.Provider;
            detailBill.InDepotBillID = bill.Bill_ID;
            detailBill.InDepotCount = -item.Amount;
            detailBill.UnitPrice = item.UnitPrice;
            detailBill.Price = -Math.Round(item.UnitPrice * item.Amount, 2);
            detailBill.OperationType = (int)CE_SubsidiaryOperationType.自制件退货;
            detailBill.StorageID = bill.StorageID;
            detailBill.Remark = bill.Remark + "（根据自制件退货单自动生成）";
            detailBill.AffrimPersonnel = bill.DepotManager;
            detailBill.FillInDate = bill.Bill_Time;

            return detailBill;
        }

        /// <summary>
        /// 修改报废单退货标志
        /// </summary>
        /// <param name="context">数据上下文</param>
        /// <param name="billID">自制件退货单据号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        private bool SetScrapFlag(DepotManagementDataContext context, string billID, out string error)
        {
            error = null;

            try
            {
                string strSql = "select * from S_HomemadeRejectList where Bill_ID = '" + billID + "'";
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    var varData = from a in context.S_ScrapGoods
                                  where a.GoodsID == Convert.ToInt32(dt.Rows[i]["GoodsID"].ToString())
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
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        /// <param name="djh">自制件退货单据号</param>
        /// <param name="billStatus">单据状态</param>
        /// <param name="returnBill">返回更新后重新查询的自制件退货单数据集</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="rebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string djh, string billStatus,
            out IQueryResult returnBill, out string error, string rebackReason)
        {
            returnBill = null;
            error = null;

            try
            {
                DepotManagementDataContext dataContext = CommentParameter.DepotDataContext;

                var varData = from a in dataContext.S_HomemadeRejectBill
                              where a.Bill_ID == djh
                              select a;

                string strMsg = "";

                if (varData.Count() == 1)
                {
                    S_HomemadeRejectBill lnqMRequ = varData.Single();

                    switch (billStatus)
                    {
                        case "新建单据":
                            strMsg = string.Format("{0}号自制件退货单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg, lnqMRequ.FillInPersonnelCode, false);
                            lnqMRequ.BillStatus = "新建单据";
                            lnqMRequ.FinanceSignatory = null;
                            break;
                        case "等待财务审核":
                            strMsg = string.Format("{0}号自制件退货单已回退，请您重新处理单据; 回退原因为" + rebackReason, djh);
                            m_billMessageServer.PassFlowMessage(djh, strMsg,
                                UniversalFunction.GetPersonnelCode(lnqMRequ.FinanceSignatory), false);

                            lnqMRequ.BillStatus = "等待质检";
                            lnqMRequ.FinanceSignatory = null;
                            break;
                        default:
                            break;
                    }

                    dataContext.SubmitChanges();
                }
                else
                {
                    error = "数据不唯一或者为空";
                    return false;
                }

                return GetAllBill(out returnBill, out error);
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;

            }
        }
    }
}