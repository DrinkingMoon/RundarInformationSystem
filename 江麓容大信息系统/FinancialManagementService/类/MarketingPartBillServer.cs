using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Service_Peripheral_HR;
using GlobalObject;
using PlatformManagement;
using ServerModule;

namespace Service_Economic_Financial
{
    class MarketingPartBillServer : IMarketingPartBillServer
    {
        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerArchiveServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 客户信息服务类
        /// </summary>
        IClientServer m_clientServer = ServerModule.ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 营销出库服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModule.ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <param name="returnInfo">销售清单信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("销售清单", null);
            }
            else
            {
                qr = serverAuthorization.Query("销售清单", null, QueryResultFilter);
            }

            if (!qr.Succeeded)
            {
                error = qr.Error;
                return false;
            }

            returnInfo = qr;
            return true;
        }

        /// <summary>
        /// 通过单据号获取主表
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDataByBillNo(string billNo)
        {
            string sql = "select * from View_S_MarketingPartBill where 单据号 = '" + billNo + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过关联单据号获取销售清单的信息
        /// </summary>
        /// <param name="associatedNo">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetDataByAssociatedNo(string associatedNo)
        {
            string sql = "select * from View_S_MarketingPartBill where 营销出库单号 = '" + associatedNo + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过单据号获取明细信息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回数据集</returns>
        public　List<View_S_MarketintPartList> GetListDataByBillNo(string billNo)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.View_S_MarketintPartList
                         where a.单据号 == billNo
                         select a;

            return result.ToList();
        }

        /// <summary>
        /// 修改销售清单
        /// </summary>
        /// <param name="marketPartBill">销售清单主表信息</param>
        /// <param name="marketPritList">销售清单子表信息</param>
        /// <param name="role">操作角色</param>
        /// <param name="error">错误信息</param>
        /// <returns>修改成功返回True否则返回False</returns>
        public bool UpdateData(S_MarketingPartBill marketPartBill, List<View_S_MarketintPartList> marketPritList,string role, out string error)
        {
            error = "";

            try
            {
                DataTable dt = m_findSellIn.GetBill(marketPartBill.AssociatedBillNo, 0);

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_MarketingPartBill
                             where a.BillNo == marketPartBill.BillNo && a.AssociatedBillNo == marketPartBill.AssociatedBillNo
                             select a;

                error = "判断角色";

                if (result.Count() > 0)
                {
                    S_MarketingPartBill bill = result.Single();

                    switch (role)
                    {
                        case "销售主管":
                            bill.YX_Auditor = marketPartBill.YX_Auditor;
                            bill.YX_AuditTime = marketPartBill.YX_AuditTime;
                            bill.Status = marketPartBill.Status;

                            SystemLog_MarketPart YXAuditorlog = new SystemLog_MarketPart();

                            YXAuditorlog.BillNo = marketPartBill.BillNo;
                            YXAuditorlog.Content = marketPartBill.YX_AuditTime + "销售主管【" 
                                + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.YX_Auditor).Name + "】审核";
                            YXAuditorlog.Recorder = marketPartBill.YX_Auditor;
                            YXAuditorlog.RecordTime = Convert.ToDateTime(marketPartBill.YX_AuditTime);

                            dataContxt.SystemLog_MarketPart.InsertOnSubmit(YXAuditorlog);

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (m_findSellIn.AuditingBill(Convert.ToInt32(dt.Rows[0]["ID"].ToString()), marketPartBill.Remark, out error))
                                {
                                    string storage = dt.Rows[0]["storageID"].ToString();

                                    m_billMessageServer.BillType = "营销出库单";
                                    m_billMessageServer.PassFlowMessage(marketPartBill.AssociatedBillNo,
                                        string.Format("{0} 号营销出库单，请财务审核", marketPartBill.AssociatedBillNo),
                                        BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.会计.ToString());

                                    error = error + "/n" + "主管审核成功";
                                }
                                else
                                {
                                    return false;
                                }
                            }
                            else
                            {
                                error = "找不到营销出库单【" + marketPartBill.AssociatedBillNo + "】";
                                return false;
                            }

                            break;
                        case "销售":
                            bill.Recorder = marketPartBill.Recorder;
                            bill.RecordTime = marketPartBill.RecordTime;
                            bill.Status = marketPartBill.Status;
                            bill.CiteTerminalClient = marketPartBill.CiteTerminalClient;
                            bill.IsCarLoad = marketPartBill.IsCarLoad;

                            if (bill.ClientID != marketPartBill.ClientID)
                            {
                                bill.Remark = marketPartBill.Remark;

                                SystemLog_MarketPart log = new SystemLog_MarketPart();

                                log.BillNo = marketPartBill.BillNo;
                                log.Content = marketPartBill.RecordTime + "销售人员【" 
                                    + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.Recorder).Name + "】修改客户，将"
                                    + m_clientServer.GetClientName(result.Single().ClientID) + "修改为："
                                    + m_clientServer.GetClientName(bill.ClientID) + "】";
                                log.Recorder = marketPartBill.Recorder;
                                log.RecordTime = marketPartBill.RecordTime;

                                dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                            }
                            else
                            {
                                bill.Remark = marketPartBill.Remark;

                                SystemLog_MarketPart log = new SystemLog_MarketPart();

                                log.BillNo = marketPartBill.BillNo;
                                log.Content = marketPartBill.RecordTime + "销售人员【" 
                                    + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.Recorder).Name + "】确认";
                                log.Recorder = marketPartBill.Recorder;
                                log.RecordTime = marketPartBill.RecordTime;

                                dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                            }

                            bill.ClientID = marketPartBill.ClientID;
                            dataContxt.SubmitChanges();

                            foreach (View_S_MarketintPartList item in marketPritList)
                            {
                                var resultList = from c in dataContxt.S_MarketintPartList
                                                 where c.BillNo == item.单据号 && c.GoodsID == item.GoodsID && c.BatchNo == item.批次号
                                                 select c;

                                if (resultList.Count() > 0)
                                {
                                    S_MarketintPartList list = resultList.Single();

                                    if (list.SellUnitPrice != item.销售单价)
                                    {
                                        SystemLog_MarketPart log = new SystemLog_MarketPart();

                                        log.BillNo = marketPartBill.BillNo;
                                        log.Content = marketPartBill.RecordTime + "销售人员【" 
                                            + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.Recorder).Name 
                                            + "】修改销售单价，将" + item.物品名称 + "销售单价" + resultList.Single().SellUnitPrice 
                                            + "修改为：" + item.销售单价 + "】";
                                        log.Recorder = marketPartBill.Recorder;
                                        log.RecordTime = marketPartBill.RecordTime;

                                        dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                                        dataContxt.SubmitChanges();
                                    }

                                    list.Reamrk = item.备注;
                                    list.SellUnitPrice = item.销售单价;
                                    list.UnitPrice = item.最低定价;
                                    list.AssemblyCarCode = item.主机厂代码;
                                    list.AssemblyCarName = item.主机厂物品名称;

                                    dataContxt.SubmitChanges();
                                }                               
                            }

                            var resultMarketing = from c in dataContxt.S_MarketingBill
                                             where c.DJH == marketPartBill.AssociatedBillNo
                                             select c;

                            if (resultMarketing.Count() > 0)
                            {
                                S_MarketingBill marketing = resultMarketing.Single();

                                marketing.DJZT_FLAG = "已保存";

                                m_billMessageServer.BillType = "营销出库单";
                                m_billMessageServer.PassFlowMessage(marketPartBill.AssociatedBillNo,
                                    string.Format("{0} 号营销出库单，请主管审核", marketPartBill.AssociatedBillNo),
                                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.营销主管.ToString());

                            }
                            else
                            {
                                error = "找不到营销出库单【" + marketPartBill.AssociatedBillNo + "】";
                                return false;
                            }

                            break;
                        case "财务":
                            bill.CW_Auditor = marketPartBill.CW_Auditor;
                            bill.CW_AuditTime = marketPartBill.CW_AuditTime;
                            bill.Status = marketPartBill.Status;

                            if (bill.ClientID != marketPartBill.ClientID)
                            {
                                SystemLog_MarketPart log = new SystemLog_MarketPart();

                                log.BillNo = marketPartBill.BillNo;
                                log.Content = marketPartBill.CW_AuditTime + "财务人员【" 
                                    + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.CW_Auditor).Name 
                                    + "】修改客户，将" + m_clientServer.GetClientName(result.Single().ClientID) 
                                    + "修改为：" + m_clientServer.GetClientName(bill.ClientID) + "】";
                                log.Recorder = marketPartBill.CW_Auditor;
                                log.RecordTime = Convert.ToDateTime(marketPartBill.CW_AuditTime);

                                dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                            }
                            else
                            {
                                SystemLog_MarketPart log = new SystemLog_MarketPart();

                                log.BillNo = marketPartBill.BillNo;
                                log.Content = marketPartBill.CW_AuditTime + "财务人员【" 
                                    + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.CW_Auditor).Name + "】审核";
                                log.Recorder = marketPartBill.CW_Auditor;
                                log.RecordTime = Convert.ToDateTime(marketPartBill.CW_AuditTime);

                                dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                            }

                            bill.ClientID = marketPartBill.ClientID;
                            bill.Remark = marketPartBill.Remark;

                            foreach (View_S_MarketintPartList item in marketPritList)
                            {
                                var resultList = from c in dataContxt.S_MarketintPartList
                                                 where c.BillNo == item.单据号 && c.GoodsID == item.GoodsID && c.BatchNo == item.批次号
                                                 && c.Provider == item.供应商
                                                 select c;

                                if (resultList.Count() > 0)
                                {
                                    S_MarketintPartList list = resultList.Single();

                                    if (list.SellUnitPrice != item.销售单价)
                                    {
                                        SystemLog_MarketPart log = new SystemLog_MarketPart();

                                        log.BillNo = marketPartBill.BillNo;
                                        log.Content = marketPartBill.CW_AuditTime + "财务人员【" 
                                            + m_personnerArchiveServer.GetPersonnelInfo(marketPartBill.CW_Auditor).Name 
                                            + "】修改销售单价，将销售单价" + resultList.Single().SellUnitPrice + "修改为："
                                            + item.销售单价 + "】";
                                        log.Recorder = marketPartBill.CW_Auditor;
                                        log.RecordTime = Convert.ToDateTime(marketPartBill.CW_AuditTime);

                                        dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                                    }

                                    list.Reamrk = item.备注;
                                    list.SellUnitPrice = item.销售单价;
                                }
                            }

                            if (dt != null && dt.Rows.Count > 0)
                            {
                                if (dt.Rows[0]["DJZT_FLAG"].ToString() == "等待财务审核")
                                {
                                    if (m_findSellIn.RetrialBill(marketPartBill.AssociatedBillNo, marketPartBill.Remark, out error))
                                    {
                                        string storage = dt.Rows[0]["storageID"].ToString();

                                        m_billMessageServer.BillType = "营销出库单";
                                        m_billMessageServer.PassFlowMessage(marketPartBill.AssociatedBillNo, 
                                            string.Format("{0} 号营销出库单，请仓管员确认", marketPartBill.AssociatedBillNo),
                                                m_billMessageServer.GetRoleStringForStorage(storage).ToString(), true);
                                    }
                                    else
                                    {
                                        return false;
                                    }
                                }
                            }
                            else
                            {
                                error = "找不到营销出库单【"+marketPartBill.AssociatedBillNo+"】";
                                return false;
                            }

                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    error = "未找到相关单据";
                    return false;
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 打印单据添加打印次数，记录日志
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功反复True，失败返回false</returns>
        public bool PrintUpodateData(string billNo, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_MarketingPartBill
                             where a.BillNo == billNo
                             select a;

                if (result.Count() == 1)
                {
                    S_MarketingPartBill bill = result.Single();

                    bill.PrintCount += 1;

                    SystemLog_MarketPart log = new SystemLog_MarketPart();

                    log.BillNo = billNo;
                    log.Content =ServerTime.Time + "财务人员【" +BasicInfo.LoginName + "】打印。";
                    log.Recorder = BasicInfo.LoginID;
                    log.RecordTime = ServerTime.Time;

                    dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                }

                dataContxt.SubmitChanges();
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
        /// <param name="billNo">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <param name="strRebackReason">回退原因</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string billNo, string strBillStatus, out string error, string strRebackReason)
        {
            error = null;

            try
            {
                m_billMessageServer.BillType = "销售清单";
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.S_MarketingPartBill
                              where a.BillNo == billNo
                              select a;

                string strMsg = "";

                if (result.Count() == 1)
                {

                    S_MarketingPartBill bill = result.Single();

                    switch (strBillStatus)
                    {
                        case "等待销售人员确认":
                            bill.CW_Auditor = "";
                            bill.YX_Auditor = "";
                            bill.Status = "等待销售人员确认";

                            strMsg = string.Format("{0}号销售清单已回退，请您重新处理单据; 回退原因为" + strRebackReason, billNo);
                            m_billMessageServer.PassFlowMessage(billNo, strMsg,BillFlowMessage_ReceivedUserType.用户, result.Single().Recorder);

                            break;
                        //case "等待主管审核":
                        //    bill.YX_Auditor = "";
                        //    bill.CW_Auditor = "";
                        //    bill.Status = "等待主管审核";

                        //    strMsg = string.Format("{0}号销售清单已回退，请您重新处理单据; 回退原因为" + strRebackReason, billNo);
                        //    m_billMessageServer.PassFlowMessage(billNo, strMsg, RoleEnum.营销主管.ToString(), false);
                            //break;
                    }
                    
                    SystemLog_MarketPart log = new SystemLog_MarketPart();

                    log.BillNo = billNo;
                    log.Content = ServerTime.Time + "【" + BasicInfo.LoginName + "】回退。";
                    log.Recorder = BasicInfo.LoginID;
                    log.RecordTime = ServerTime.Time;

                    dataContxt.SystemLog_MarketPart.InsertOnSubmit(log);
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 通过单据号获得操作日志
        /// </summary>
        /// <param name="BillNo">单据号</param>
        /// <returns>返回操作日志</returns>
        public DataTable GetSystemLog(string BillNo)
        {
            string sql = "select * from View_SystemLog_MarketPart where 单据号 = '" + BillNo + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 导出Excel
        /// </summary>
        /// <param name="startTime">起始时间</param>
        /// <param name="endTime">终止时间</param>
        /// <param name="status">单据状态</param>
        /// <returns>返回数据集</returns>
        public List<View_销售清单零件单价查询> GetExcelData(DateTime startTime, DateTime endTime,string status)
        {
            string sql = "select * from View_销售清单零件单价查询 where 销售时间 >= '" + startTime + "' " +
                         " and 销售时间<='" + endTime + "' and 单据状态='" + status + "'";

            sql += " order by 单据号";

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            return ctx.ExecuteQuery<View_销售清单零件单价查询>(sql, new object[] { }).ToList();
        }
    }
}
