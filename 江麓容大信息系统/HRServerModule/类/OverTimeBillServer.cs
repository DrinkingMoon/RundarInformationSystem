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
    /// 加班申请操作类
    /// </summary>
    class OverTimeBillServer : Service_Peripheral_HR.IOverTimeBillServer
    {
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
        /// 获取所有加班申请表信息
        /// </summary>
        /// <param name="returnInfo">加班申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOverTimeBill(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.Query("加班申请操作", null);
            }
            else
            {
                qr = serverAuthorization.Query("加班申请操作", null, QueryResultFilter);
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
        /// 获取所有加班申请表信息
        /// </summary>
        /// <param name="returnInfo">加班申请信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        public bool GetAllOverTimeBillByWorkID(out IQueryResult returnInfo, out string error)
        {
            returnInfo = null;
            error = null;

            IAuthorization serverAuthorization = PlatformFactory.GetObject<IAuthorization>();
            IQueryResult qr = null;
            string[] paras = { BasicInfo.LoginID};

            if (QueryResultFilter == null)
            {
                qr = serverAuthorization.QueryMultParam("加班人员查看",null, paras);
            }
            else
            {
                qr = serverAuthorization.QueryMultParam("加班人员查看",null, paras);
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
        /// 通过员工编号和申请时间获得加班申请单
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOverTimeByWorkID(string workID, DateTime starDate, DateTime endDate)
        {
            string sql = "select * from dbo.view_HR_OvertimeBill left join HR_OvertimePersonnel " +
                         " on HR_OvertimePersonnel.billID=view_HR_OvertimeBill.单据号 where " +
                         " HR_OvertimePersonnel.workID='" + workID + "' and (开始时间  between  '" 
                         + starDate.ToShortDateString() + "' and '" + starDate.AddDays(1).ToShortDateString() + "')" +
                         " and 是否批准 = '" + bool.TrueString + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过员工编号和申请时间获得加班申请单
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <returns>返回数据集</returns>
        public DataTable IsExistOverTimeByWorkID(string workID, DateTime starDate, DateTime endDate)
        {
            string sql = "select * from dbo.view_HR_OvertimeBill left join HR_OvertimePersonnel " +
                         " on HR_OvertimePersonnel.billID=view_HR_OvertimeBill.单据号 where " +
                         " HR_OvertimePersonnel.workID='" + workID + "' and (开始时间  between  '" + starDate + "' and '" + endDate + "')";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过员工编号和申请时间获得加班申请单
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始时间</param>
        /// <param name="endDate">终止时间</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOverTimeByWorkIDAndDate(string workID, DateTime starDate, DateTime endDate)
        {
            string sql = "select * from dbo.view_HR_OvertimeBill left join HR_OvertimePersonnel " +
                         " on HR_OvertimePersonnel.billID=view_HR_OvertimeBill.单据号 where " +
                         " HR_OvertimePersonnel.workID='" + workID + "' and (开始时间  between  '" + starDate + "' and '" + endDate + "')" +
                          " and 是否批准 = '" + bool.TrueString + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过单据号修改加班单的实际加班小时数
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="hours">实际小时数</param>
        /// <returns>修改成功返回True失败返回False</returns>
        public bool UpdateOverTimeBillByHours(string billNo,double hours)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var result = from a in dataContxt.HR_OvertimeBill
                         where a.ID == Convert.ToInt32(billNo)
                         select a;

            if (result.Count() == 1)
            {
                HR_OvertimeBill overTime = result.Single();

                overTime.RealHours = hours;
            }
            else
            {
                return false;
            }

            dataContxt.SubmitChanges();
            return true;
        }

        /// <summary>
        /// 通过单据号获得加班人员
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOverTimePersonnelByID(string billID)
        {
            string sql = "select 员工编号,员工姓名 from View_HR_OvertimePersonnel where 单据号='" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 通过单据号获得加班信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <returns>返回数据集</returns>
        public DataTable GetOverTimeBillByID(string billID)
        {
            string sql = "select * from View_HR_OvertimeBill where 单据号='" + billID + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }

        /// <summary>
        /// 新增加班申请
        /// </summary>
        /// <param name="overTime">加班申请主信息</param>
        /// <param name="personnel">加班人员</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回新增的单据编号，失败返回-1</returns>
        public int AddOverTimeBill(HR_OvertimeBill overTime, List<HR_OvertimePersonnel> personnel, out string error)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();
            error = "";

            try
            {

                var result = from a in dataContxt.HR_OvertimeBill
                             where a.Applicant == overTime.Applicant && a.BeginTime == overTime.BeginTime
                             select a;

                if (result.Count() > 0)
                {
                    if (result.Count() == 1)
                    {
                        var resultList = from c in dataContxt.HR_OvertimePersonnel
                                         where c.BillID == result.Single().ID
                                         select c;

                        foreach (HR_OvertimePersonnel item in resultList)
                        {
                            foreach (HR_OvertimePersonnel list in personnel)
                            {
                                if (item.WorkID == list.WorkID)
                                {
                                    error = "同一员工不能在同一时间申请多个单据！";
                                    return -1;
                                }
                            }
                        }
                    }
                }

                dataContxt.HR_OvertimeBill.InsertOnSubmit(overTime);
                dataContxt.SubmitChanges();

                int billID = -1;

                var resultOvertime = from a in dataContxt.HR_OvertimeBill
                             where a.Applicant == overTime.Applicant && a.Date == overTime.Date
                             && a.BeginTime == overTime.BeginTime
                             select a;

                if (resultOvertime.Count() == 1)
                {
                    billID = resultOvertime.Single().ID;

                    foreach (var item in personnel)
                    {
                        HR_OvertimePersonnel personnleList = item;

                        personnleList.BillID = billID;
                        personnleList.WorkID = item.WorkID;

                        new AttendanceAnalysis().DataTimeIsRepeat<HR_OvertimeBill>(dataContxt, resultOvertime.Single(), personnleList.WorkID);
                        dataContxt.HR_OvertimePersonnel.InsertOnSubmit(personnleList);
                    }
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
        /// 修改加班申请
        /// </summary>
        /// <param name="overTime">加班申请主信息</param>
        /// <param name="personnel">加班人员</param>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true,失败返回False</returns>
        public bool UpdateOverTimeBill(HR_OvertimeBill overTime, List<HR_OvertimePersonnel> personnel, int billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultMain = from a in dataContxt.HR_OvertimeBill
                                 where a.ID == billID
                                 select a;

                if (resultMain.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }
                else
                {
                    HR_OvertimeBill bill = resultMain.Single();

                    bill.BeginTime = overTime.BeginTime;
                    bill.EndTime = overTime.EndTime;
                    bill.VerifyHours = overTime.VerifyHours;
                    bill.RealHours = overTime.RealHours;
                    bill.CompensateMode = overTime.CompensateMode;
                    bill.OvertimeAddress = overTime.OvertimeAddress;
                    bill.Date = overTime.Date;
                    bill.Errand = overTime.Errand;
                    bill.Hours = overTime.Hours;
                    bill.NumberOfPersonnel = overTime.NumberOfPersonnel;
                    bill.BillStatus = overTime.BillStatus;
                }

                var result = from c in dataContxt.HR_OvertimePersonnel
                             where c.BillID == billID
                             select c;

                if (result.Count() > 0)
                {
                    dataContxt.HR_OvertimePersonnel.DeleteAllOnSubmit(result);

                    foreach (var item in personnel)
                    {
                        HR_OvertimePersonnel personnleList = item;

                        personnleList.BillID = billID;
                        personnleList.WorkID = item.WorkID;

                        dataContxt.HR_OvertimePersonnel.InsertOnSubmit(personnleList);
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
        /// 通过单据号删除单据信息
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteOverTimeBill(int billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultPersonnel = from a in dataContxt.HR_OvertimePersonnel
                                      where a.BillID == billID
                                      select a;
                dataContxt.HR_OvertimePersonnel.DeleteAllOnSubmit(resultPersonnel);

                var result = from c in dataContxt.HR_OvertimeBill
                             where c.ID == billID
                             select c;
                dataContxt.HR_OvertimeBill.DeleteAllOnSubmit(result);

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
        /// 领导审核修改加班单据
        /// </summary>
        /// <param name="overTime">出差单据数据集</param>
        /// <param name="roleType">角色类型</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateOverTimeBill(HR_OvertimeBill overTime, string roleType, out string error)
        {
            error = "";
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;
            dataContxt.Connection.Open();
            dataContxt.Transaction = dataContxt.Connection.BeginTransaction();

            try
            {

                var result = from a in dataContxt.HR_OvertimeBill
                             where a.ID == overTime.ID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请查证后再操作！";
                    return false;
                }

                HR_OvertimeBill bill = result.Single();
                List<string> list = new List<string>();
                DataTable dt = GlobalObject.DatabaseServer.QueryInfo("select * from HR_OvertimePersonnel where billID=" + bill.ID);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(dt.Rows[i]["WorkID"].ToString());
                }

                bill.BillStatus = overTime.BillStatus;    

                switch (roleType)
                {
                    case "部门主管审批":
                        bill.DeptDirector = overTime.DeptDirector;
                        bill.DeptDirectorSignatureDate = overTime.DeptDirectorSignatureDate;                     
                        break;
                    case "分管领导审批":
                        bill.Leader = overTime.Leader;
                        bill.LeaderSignatureDate = overTime.LeaderSignatureDate;
                        bill.Authorize = overTime.Authorize;
                        bill.VerifyHours = overTime.VerifyHours;
                        bill.RealHours = overTime.RealHours;
                        break;
                    case "部门负责人审批":
                        bill.DeptPrincipal = overTime.DeptPrincipal;
                        bill.DeptPrincipalSignatureDate = overTime.DeptPrincipalSignatureDate;
                        bill.Authorize = overTime.Authorize;
                        bill.VerifyHours = overTime.VerifyHours;
                        bill.RealHours = overTime.RealHours;
                        break;
                    case "人力资源":
                        bill.HR_Signature = overTime.HR_Signature;
                        bill.HR_SignatureDate = overTime.HR_SignatureDate;
                        break;
                    case "确认加班完成情况":
                        bill.Verifier = overTime.Verifier;
                        bill.VerifyFinish = overTime.VerifyFinish;
                        bill.VerifyHours = overTime.VerifyHours;
                        bill.VerifySignaturDate = overTime.VerifySignaturDate;
                        bill.RealHours = Convert.ToDouble(overTime.VerifyHours);
                        break;
                    default:
                        break;
                }

                dataContxt.SubmitChanges();

                if (GlobalObject.GeneralFunction.StringConvertToEnum<OverTimeBillStatus>(bill.BillStatus) == OverTimeBillStatus.已完成)
                {
                    ITimeExceptionServer service = ServerModuleFactory.GetServerModule<ITimeExceptionServer>();
                    service.OperationTimeException_Replenishments(dataContxt, bill.ID.ToString(), CE_HR_AttendanceExceptionType.加班);
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
        /// 判断员工加班的补偿方式是否可以选择
        /// </summary>
        /// <param name="workPost">岗位名称</param>
        /// <param name="dept">部门</param>
        /// <param name="workID">员工编号</param>
        /// <returns>可以返回True，不可以返回False</returns>
        public bool IsChooseDoubleRest(string workPost,string dept,string workID)
        {
            bool flag = false;

            //if (dept != "")
            //{
            //    switch (dept)
            //    {
            //        case "ZZ01":
            //        case "装配车间":
            //            flag = true;
            //            break;
            //        case "ZZ02":
            //        case "下线车间":
            //            flag = true;
            //            break;
            //        case "ZZ03":
            //        case "机加车间":
            //            flag = true;
            //            break;
            //        case "ZZ04":
            //        case "TCU车间":
            //            flag = true;
            //            break;
            //        case "ZZ05":
            //        case "物流车间":
            //            flag = true;
            //            break;
            //        default:
            //            flag = false;
            //            break;
            //    }
            //}

            if (!flag)
            {
                switch (workPost)
                {
                    case "393":
                    case "台架试验员":
                        flag = true;
                        break;
                    case "385":
                    case "设备维修工":
                        if (workID == "0300" || workID == "0342")
                            flag = true;
                        break;
                    default:
                        flag = false;
                        break;
                }
            }

            return flag;
        }

        /// <summary>
        /// 获取员工当月的加班小时数
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <returns>当有加班时间时返回加班小时数，没有加班时间时，返回-1</returns>
        public double GetMonthRealHour(string workID, DateTime starDate, DateTime endDate)
        {
            string sql = "select sum(realhours) hours from dbo.HR_OvertimeBill " +
                         " left join  dbo.HR_OvertimePersonnel on HR_OvertimePersonnel.BillID=HR_OvertimeBill.ID" +
                         " where workID='" + workID + "' and beginTime between '" + starDate + "' and '" + endDate + "'"+
                         " and Compensatemode='加班调休' and BillStatus='已完成'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["hours"].ToString() != "")
                    return Convert.ToDouble(dt.Rows[0]["hours"]);
                else
                    return 0;
            }
            else
            {
                return 0;
            }
        }

        /// <summary>
        /// 获取自动生成的加班单的人员信息
        /// </summary>
        /// <returns>返回Table</returns>
        public DataTable AutoCreateOverTime_ShowPersonnel()
        {
            string error = null;

            return GlobalObject.DatabaseServer.QueryInfoPro("HR_GetSaturday_AutoOverTimeBillInfo", null, out error);
        }

        /// <summary>
        /// 处理自动生成的加班单的人员信息
        /// </summary>
        /// <param name="infoTable">人员信息列表</param>
        /// <param name="saveFlag">保存标志</param>
        public void AutoCreateOverTime_BatchOperation(DataTable infoTable, bool saveFlag)
        {
            string error = null;

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {

                infoTable.AcceptChanges();
                DataTable tempTable = DataSetHelper.SelectDistinct("", infoTable, "单据号");

                foreach (DataRow dr in tempTable.Rows)
                {
                    var varData = from a in ctx.HR_OvertimePersonnel
                                  where a.BillID == Convert.ToInt32(dr["单据号"])
                                  select a;

                    ctx.HR_OvertimePersonnel.DeleteAllOnSubmit(varData);

                    DataTable tempTable1 = DataSetHelper.SiftDataTable(infoTable, " 单据号 = " + Convert.ToInt32(dr["单据号"]), out error);

                    foreach (DataRow dr1 in tempTable1.Rows)
                    {
                        if (Convert.ToBoolean(dr1["选"]))
                        {
                            HR_OvertimePersonnel personnel = new HR_OvertimePersonnel();

                            personnel.BillID = Convert.ToInt32(dr["单据号"]);
                            personnel.WorkID = dr1["人员工号"].ToString();

                            ctx.HR_OvertimePersonnel.InsertOnSubmit(personnel);
                        }
                    }

                    if (!saveFlag)
                    {
                        var varBill = from a in ctx.HR_OvertimeBill
                                      where a.ID == Convert.ToInt32(dr["单据号"])
                                      select a;

                        HR_OvertimeBill billInfo = varBill.Single();

                        billInfo.Authorize = true;
                        billInfo.BillStatus = "已完成";
                    }
                }

                ctx.SubmitChanges();
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }

        }
    }
}
