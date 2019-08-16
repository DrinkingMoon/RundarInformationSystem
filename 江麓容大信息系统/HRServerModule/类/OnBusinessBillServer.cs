using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using System.Data;
using ServerModule;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 出差申请表操作类
    /// </summary>
    class OnBusinessBillServer : Service_Peripheral_HR.IOnBusinessBillServer
    {
        /// <summary>
        /// 最高部门
        /// </summary>
        string m_highDept;

        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 获得单条记录信息
        /// </summary>
        /// <param name="billID">单据ID</param>
        /// <returns>返回单条记录信息</returns>
        public HR_OnBusinessBill GetSingleInfo(int billID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_OnBusinessBill
                          where a.ID == billID
                          select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else if (varData.Count() > 1)
            {
                throw new Exception("记录不唯一");
            }
            else
            {
                return varData.Single();
            }
        }

        public List<HR_OnBusinessPersonnel> GetPersonnel(int billID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_OnBusinessPersonnel
                          where a.BillID == billID
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }

        /// <summary>
        /// 获取所有出差申请表信息
        /// </summary>
        /// <param name="returnInfo">出差申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOnBusinessBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("出差申请表操作", null);
            }
            else
            {
                qr = serverAuthorization.Query("出差申请表操作", null, QueryResultFilter);
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
        /// 获取所有出差申请表信息
        /// </summary>
        /// <param name="returnInfo">出差申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOnBusinessBillByWorkID(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;
            string[] paras = { BasicInfo.LoginID };

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.QueryMultParam("出差人员查看",null, paras);
            }
            else
            {
                qr = serverAuthorization.QueryMultParam( "出差人员查看",null, paras);
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
        /// 出差随行人员部门负责人查看
        /// </summary>
        /// <param name="returnInfo">出差申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOnBusinessBillByDeptCode(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;
            string[] paras = { BasicInfo.DeptCode };

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.QueryMultParam("出差随行人员部门负责人查看", null,paras);
            }
            else
            {
                qr = serverAuthorization.QueryMultParam("出差随行人员部门负责人查看",null, paras);
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
        /// 通过单据编号获得单据信息
        /// </summary>
        /// <param name="billID">单据编号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOnBusinessBillByID(int billID)
        {
            string sql = "select * from View_HR_OnBusinessBill where 编号=" + billID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        ///// <summary>
        ///// 通过员工编号和时间获得出差信息
        ///// </summary>
        ///// <param name="workID">员工编号</param>
        ///// <param name="starTime">起始时间</param>
        ///// <param name="endTime">截止时间</param>
        ///// <returns>返回数据集</returns>
        //public DataTable GetOnBusinessBillByWorkIDAndTime(string workID,DateTime starTime,DateTime endTime)
        //{
        //    string sql = @"select 单据号,View_HR_OnBusinessPersonnel.员工编号,预定出发时间,预定返程时间 "+
        //                  " from dbo.View_HR_OnBusinessPersonnel inner join "+
        //                  " dbo.View_HR_OnBusinessBill on View_HR_OnBusinessBill.编号=View_HR_OnBusinessPersonnel.单据号"+
        //                  " where View_HR_OnBusinessPersonnel.员工编号='" + workID + "' and ("+
        //                  " (实际出发时间 between '" + starTime + "' and '" + endTime + "'" +
        //                  " or (实际返程时间 >= '" + starTime + "' and '" + endTime + "')) and 是否批准 = '"+ bool.TrueString +"'";

        //    DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
        //    return dt;
        //}

        /// <summary>
        /// 考勤异常处理_判断是否出差
        /// </summary>
        /// <param name="timeException">考勤异常对象</param>
        /// <returns>返回Table</returns>
        public DataTable GetOnBusinessInfo_TimeExceptionJudge(HR_TimeException timeException)
        {
            string strSql = " select 单据号,a.员工编号,预定出发时间,预定返程时间 ,c.* "+
                            " from dbo.View_HR_OnBusinessPersonnel as a   "+
                            " inner join dbo.View_HR_OnBusinessBill as b on b.编号= a.单据号 "+
                            " inner join HR_TimeException as c on a.员工编号 = c.WorkID and c.ID = "+ timeException.ID +
                            " where a.员工编号='" + timeException.WorkID + "' and 是否批准 = '" + bool.TrueString + "' " +
                            " and CAST( CONVERT(varchar(100),  b.实际出发时间, 23) as dateTime) <= c.Date "+
                            " and CAST( CONVERT(varchar(100),  b.实际返程时间, 23) as dateTime) >= c.Date";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 通过日期判断人员在当天是否出差
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="recordDate">打卡日期</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOnBusinessBillByWorkIDAndTime(string workID, DateTime recordDate)
        {
            string sql = @"select 单据号,View_HR_OnBusinessPersonnel.员工编号,预定出发时间,预定返程时间 " +
                          " from dbo.View_HR_OnBusinessPersonnel inner join " +
                          " dbo.View_HR_OnBusinessBill on View_HR_OnBusinessBill.编号=View_HR_OnBusinessPersonnel.单据号" +
                          " where View_HR_OnBusinessPersonnel.员工编号='" + workID + "'and 是否批准 = '" + bool.TrueString + "' " +
                          " and 实际出发时间 <= '" + recordDate + "' and 实际返程时间 >= '" + recordDate + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过日期判断人员在当天是否出差
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starTime">起始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回数据集</returns>
        public string IsExistOnBusinessBillByWorkIDAndTime(string workID, DateTime starTime, DateTime endTime)
        {
            string sql = @"select 单据号 from dbo.View_HR_OnBusinessPersonnel inner join " +
                          " dbo.View_HR_OnBusinessBill on View_HR_OnBusinessBill.编号=View_HR_OnBusinessPersonnel.单据号" +
                          " where View_HR_OnBusinessPersonnel.员工编号='" + workID + "' and (" +
                          " (实际出发时间 <= '" + starTime.ToShortDateString() + " 17:30:00'" +
                          "  and (实际返程时间 >= '" + endTime + "') or (实际返程时间 >= '" + starTime.ToShortDateString() + " 17:30:00' and 实际返程时间<='" + endTime + "')))";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["单据号"].ToString();
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获得出差申请表信息
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="status">单据状态</param>
        /// <returns>返回出差申请表数据集</returns>
        public DataTable GetOnBusinessBillByTime(DateTime startTime, DateTime endTime, string status)
        {
            string strSql = "select * from View_HR_OnBusinessBill where 申请时间 >= '"
                + startTime + "' and 申请时间 <= '" + endTime + "'";

            if (status != "全  部")
            {
                strSql += " and 单据状态 = '" + status + "'";
            }

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        /// <summary>
        /// 新增出差申请
        /// </summary>
        /// <param name="onBusiness">出差申请主信息</param>
        /// <param name="personnel">出差人员</param>
        /// <param name="schedule">出差行程安排</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回新增的单据编号，失败返回0</returns>
        public int AddOnBusinessBill(HR_OnBusinessBill onBusiness, List<HR_OnBusinessPersonnel> personnel,
                                     List<HR_OnBusinessSchedule> schedule, out string error)
        {
            error = "";
            int billID = -1;
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                dataContxt.HR_OnBusinessBill.InsertOnSubmit(onBusiness);
                dataContxt.SubmitChanges();

                var resultList = from a in dataContxt.HR_OnBusinessBill
                                 where a.Applicant == onBusiness.Applicant && a.ApplicantDate.Day == onBusiness.ApplicantDate.Day
                                 && a.ApplicantDate.Minute == onBusiness.ApplicantDate.Minute
                                 select a;

                if (resultList.Count() == 1)
                {
                    billID = resultList.Single().ID;

                    foreach (var item in personnel)
                    {
                        HR_OnBusinessPersonnel personnleList = item;

                        personnleList.BillID = billID;
                        personnleList.DeptSignatureDate = ServerTime.Time;

                        new AttendanceAnalysis().DataTimeIsRepeat<HR_OnBusinessBill>(dataContxt, resultList.Single(), personnleList.WorkID);
                        dataContxt.HR_OnBusinessPersonnel.InsertOnSubmit(personnleList);
                    }

                    foreach (var item in schedule)
                    {
                        HR_OnBusinessSchedule scheduleList = item;

                        scheduleList.BillID = billID;
                        dataContxt.HR_OnBusinessSchedule.InsertOnSubmit(scheduleList);
                    }
                }
                else
                {
                    dataContxt.HR_OnBusinessBill.DeleteOnSubmit(onBusiness);
                }

                dataContxt.SubmitChanges();
                dataContxt.Transaction.Commit();
                return billID;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                dataContxt.Transaction.Rollback();
                return -1;
            }
        }

        /// <summary>
        /// 修改出差申请
        /// </summary>
        /// <param name="onBusiness">出差申请主信息</param>
        /// <param name="personnel">出差人员</param>
        /// <param name="schedule">出差行程安排</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        public bool UpdateOnBusinessBill(HR_OnBusinessBill onBusiness, List<HR_OnBusinessPersonnel> personnel,
                                     List<HR_OnBusinessSchedule> schedule,int billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultMain = from a in dataContxt.HR_OnBusinessBill
                                 where a.ID == billID
                                 select a;

                if (resultMain.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {
                    HR_OnBusinessBill bill = resultMain.Single();

                    bill.BorrowingAmount = onBusiness.BorrowingAmount;
                    bill.EntertainmentExprense = onBusiness.EntertainmentExprense;
                    bill.ETC = onBusiness.ETC;
                    bill.ETD = onBusiness.ETD;
                    bill.IsBorrowing = onBusiness.IsBorrowing;
                    bill.OtherExprense = onBusiness.OtherExprense;
                    bill.Purpose = onBusiness.Purpose;
                    bill.Vehicle = onBusiness.Vehicle;
                    bill.WithinBudget = onBusiness.WithinBudget;
                    //bill.BillStatus = onBusiness.BillStatus;
                    //bill.Confirmor = "";
                    //bill.DeptPrincipal = "";
                    //bill.GeneralManager = "";
                    //bill.LeaderSignature = "";
                }

                var result = from c in dataContxt.HR_OnBusinessPersonnel
                             where c.BillID == billID
                             select c;

                if (result.Count() > 0)
                {
                    dataContxt.HR_OnBusinessPersonnel.DeleteAllOnSubmit(result);

                    foreach (var item in personnel)
                    {
                        HR_OnBusinessPersonnel personnleList = item;

                        personnleList.BillID = billID;
                        personnleList.PersonnelType = item.PersonnelType;
                        personnleList.WorkID = item.WorkID;
                        personnleList.DeptSignatureDate = ServerTime.Time;

                        dataContxt.HR_OnBusinessPersonnel.InsertOnSubmit(personnleList);
                    }
                }

                var resultSchedule = from e in dataContxt.HR_OnBusinessSchedule
                             where e.BillID == billID
                             select e;

                if (resultSchedule.Count() > 0)
                {
                    dataContxt.HR_OnBusinessSchedule.DeleteAllOnSubmit(resultSchedule);

                    foreach (var item in schedule)
                    {
                        HR_OnBusinessSchedule scheduleList = item;

                        scheduleList.BillID = billID;
                        scheduleList.Contact = item.Contact;
                        scheduleList.Place = item.Place;
                        scheduleList.Remark = item.Remark;
                        scheduleList.StartTime = item.StartTime;
                        scheduleList.EndTime = item.EndTime;
                        scheduleList.WorkContent = item.WorkContent;
                        scheduleList.Vehicle = item.Vehicle;

                        dataContxt.HR_OnBusinessSchedule.InsertOnSubmit(scheduleList);
                    }
                }
                else
                {
                    foreach (var item in schedule)
                    {
                        HR_OnBusinessSchedule scheduleList = item;

                        scheduleList.BillID = billID;
                        scheduleList.Contact = item.Contact;
                        scheduleList.Place = item.Place;
                        scheduleList.Remark = item.Remark;
                        scheduleList.StartTime = item.StartTime;
                        scheduleList.EndTime = item.EndTime;
                        scheduleList.WorkContent = item.WorkContent;
                        scheduleList.Vehicle = item.Vehicle;

                        dataContxt.HR_OnBusinessSchedule.InsertOnSubmit(scheduleList);
                    }
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
        /// 通过单据号获得出差人员
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOnBusinessPersonnel(string billID)
        {
            string sql = "select * from View_HR_OnBusinessPersonnel where 单据号='" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过单据号获得出差行程
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOnBusinessSchedule(string billID)
        {
            string sql = "select starttime 起始时间,endTime 截止时间, Place 地点,Vehicle 交通工具, WorkContent 工作内容, Contact 联系人, Remark 备注 " +
                         " from HR_OnBusinessSchedule where BillID='"+billID+"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 领导审核修改出差单据
        /// </summary>
        /// <param name="onBusiness">出差单据数据集</param>
        /// <param name="roleType">角色类型</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateOnBusinessBill(HR_OnBusinessBill onBusiness, string roleType, out string error)
        {
            error = "";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {
                var result = from a in dataContxt.HR_OnBusinessBill
                             where a.ID == onBusiness.ID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }

                HR_OnBusinessBill bill = result.Single();
                bill.BillStatus = onBusiness.BillStatus;

                switch (roleType)
                {
                    case "部门主管审批":
                        bill.DeptPrincipal += "," + BasicInfo.LoginID;
                        bill.DeptSignatureDate = onBusiness.DeptSignatureDate;
                        bill.WithinBudget = onBusiness.WithinBudget;

                        bill.LeaderSignature = onBusiness.LeaderSignature;
                        bill.LeaderSignatureDate = onBusiness.LeaderSignatureDate;
                        bill.Authorize = onBusiness.Authorize;

                        break;
                    case "分管领导审批":
                        bill.LeaderSignature = onBusiness.LeaderSignature;
                        bill.LeaderSignatureDate = onBusiness.LeaderSignatureDate;
                        bill.Authorize = onBusiness.Authorize;
                        break;
                    case "总经理审批":
                        bill.GeneralManager = onBusiness.GeneralManager;
                        bill.GM_SignatureDate = onBusiness.GM_SignatureDate;
                        bill.Authorize = onBusiness.Authorize;
                        break;
                    case "随行人员部门确认":
                        break;
                    case "销差人确认":
                        bill.RealBeginTime = onBusiness.RealBeginTime;
                        bill.RealEndTime = onBusiness.RealEndTime;
                        bill.Confirmor = onBusiness.Confirmor;
                        bill.ConfirmorDate = onBusiness.ConfirmorDate;
                        break;
                    case "出差结果说明":
                        bill.Result = onBusiness.Result;
                        break;
                    default:
                        break;
                }

                dataContxt.SubmitChanges();

                if (GlobalObject.GeneralFunction.StringConvertToEnum<OnBusinessBillStatus>(bill.BillStatus) == OnBusinessBillStatus.已完成
                    || GlobalObject.GeneralFunction.StringConvertToEnum<OnBusinessBillStatus>(bill.BillStatus) == OnBusinessBillStatus.等待出差结果说明)
                {
                    ITimeExceptionServer service = ServerModuleFactory.GetServerModule<ITimeExceptionServer>();
                    service.OperationTimeException_Replenishments(dataContxt, bill.ID.ToString(), CE_HR_AttendanceExceptionType.出差);
                }

                dataContxt.Transaction.Commit();
                return true;
            }
            catch (Exception ex)
            {
                dataContxt.Transaction.Rollback();
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 随行人员部门确认修改出差单据
        /// </summary>
        /// <param name="personnel">出差人员</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateOnBusinessPersonnel(HR_OnBusinessPersonnel personnel, int billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_OnBusinessPersonnel
                             where a.BillID == billID && a.WorkID == personnel.WorkID
                             select a;

                if (result.Count() < 0)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }

                HR_OnBusinessPersonnel bill = result.Single();

                bill.DeptPrincipal = personnel.DeptPrincipal;
                bill.DeptSignatureDate = personnel.DeptSignatureDate;

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
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteOnBusinessBill(int billID,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from e in dataContxt.HR_OnBusinessBill
                             where e.ID == billID
                             select e;

                HR_OnBusinessBill bill = result.Single();

                dataContxt.HR_OnBusinessBill.DeleteOnSubmit(bill);

                var resultPersonnel = from a in dataContxt.HR_OnBusinessPersonnel
                                      where a.BillID == billID
                                      select a;
                dataContxt.HR_OnBusinessPersonnel.DeleteAllOnSubmit(resultPersonnel);

                var resultSchedule = from c in dataContxt.HR_OnBusinessSchedule
                                     where c.BillID == billID
                                     select c;
                dataContxt.HR_OnBusinessSchedule.DeleteAllOnSubmit(resultSchedule);

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
        /// <param name="strDJH">单据号</param>
        /// <param name="strBillStatus">单据状态</param>
        /// <param name="strRebackReason">回退状态</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作成功返回True，操作失败返回False</returns>
        public bool ReturnBill(string strDJH, string strBillStatus, string strRebackReason, out string error)
        {
            error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_OnBusinessBill
                              where a.ID == Convert.ToInt32(strDJH)
                              select a;

                string strMsg = "";

                if (result.Count() == 1)
                {
                    HR_OnBusinessBill onBusiness = result.Single();

                    m_billMessageServer.BillType = "出差申请单";

                    DataTable dt = new PersonnelArchiveServer().GetHighestDept(onBusiness.Applicant);

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        m_highDept = dt.Rows[0]["deptCode"].ToString();
                    }

                    switch (strBillStatus)
                    {
                        case "等待主管审核":
                            strMsg = string.Format("{0}号出差申请单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetDeptDirectorRoleName(m_highDept).ToList());

                            onBusiness.BillStatus = OnBusinessBillStatus.等待部门负责人审核.ToString();
                            onBusiness.Authorize = false;
                            onBusiness.Confirmor = "";
                            onBusiness.ConfirmorDate = ServerTime.Time;
                            onBusiness.DeptPrincipal = "";
                            onBusiness.DeptSignatureDate = ServerTime.Time;
                            onBusiness.GeneralManager = "";
                            onBusiness.GM_SignatureDate = ServerTime.Time;
                            onBusiness.LeaderSignature = "";
                            onBusiness.LeaderSignatureDate = ServerTime.Time;
                            onBusiness.RealBeginTime = ServerTime.Time;
                            onBusiness.RealEndTime = ServerTime.Time;
                            //onBusiness.Result = "";
                            break;
                        case "等待分管领导审批":
                            strMsg = string.Format("{0}号出差申请单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetDeptLeaderRoleName(m_highDept).ToList());

                            onBusiness.BillStatus = OnBusinessBillStatus.等待分管领导审批.ToString();
                            onBusiness.Confirmor = "";
                            onBusiness.ConfirmorDate = ServerTime.Time;
                            onBusiness.GeneralManager = "";
                            onBusiness.GM_SignatureDate = ServerTime.Time;
                            onBusiness.LeaderSignature = "";
                            onBusiness.LeaderSignatureDate = ServerTime.Time;
                            onBusiness.RealBeginTime = ServerTime.Time;
                            onBusiness.RealEndTime = ServerTime.Time;
                            //onBusiness.Result = "";
                            break;
                        case "等待总经理批准":
                            strMsg = string.Format("{0}号出差申请单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, BillFlowMessage_ReceivedUserType.角色,CE_RoleEnum.总经理.ToString());

                            onBusiness.BillStatus = OnBusinessBillStatus.等待总经理批准.ToString();
                            onBusiness.Authorize = false;
                            onBusiness.Confirmor = "";
                            onBusiness.ConfirmorDate = ServerTime.Time;
                            onBusiness.GeneralManager = "";
                            onBusiness.GM_SignatureDate = ServerTime.Time;
                            onBusiness.RealBeginTime = ServerTime.Time;
                            onBusiness.RealEndTime = ServerTime.Time;
                            //onBusiness.Result = "";
                            break;
                        case "等待销差人确认":
                            strMsg = string.Format("{0}号出差申请单已回退，请您重新处理单据; 回退原因为" + strRebackReason, strDJH);
                            m_billMessageServer.PassFlowMessage(strDJH, strMsg, BillFlowMessage_ReceivedUserType.用户,onBusiness.Applicant.ToString());

                            onBusiness.BillStatus = OnBusinessBillStatus.等待销差人确认.ToString();
                            onBusiness.Confirmor = "";
                            onBusiness.ConfirmorDate = ServerTime.Time;
                            onBusiness.RealBeginTime = ServerTime.Time;
                            onBusiness.RealEndTime = ServerTime.Time;
                            //onBusiness.Result = "";
                            break;
                        default:
                            break;
                    }

                    dataContxt.SubmitChanges();

                    return true;
                }
                else
                {
                    error = "数据不唯一或者为空";

                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;

                return false;
            }
        }
    }
}
