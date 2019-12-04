using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using GlobalObject;
using PlatformManagement;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤异常登记操作类
    /// </summary>
    class TimeExceptionServer : Service_Peripheral_HR.ITimeExceptionServer
    {
        /// <summary>
        /// 查询结果过滤器
        /// </summary>
        string m_queryResultFilter = null;

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 获取或设置查询结果过滤器
        /// </summary>
        public string QueryResultFilter
        {
            get { return m_queryResultFilter; }
            set { m_queryResultFilter = value; }
        }
        
        /// <summary>
        /// 获取考勤异常登记表
        /// </summary>
        /// <param name="returnInfo">考勤异常登记表</param>
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
                qr = serverAuthorization.Query("考勤异常登记管理", null);
            }
            else
            {
                qr = serverAuthorization.Query("考勤异常登记管理", null, QueryResultFilter);
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
        /// 修改考勤异常登记信息（强制处理）
        /// </summary>
        /// <param name="timeException">考勤异常登记信息数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回False</returns>
        public bool UpdateTimeException(HR_TimeException timeException, out string error)
        {
            error = "";
            DateTime starTime = Convert.ToDateTime(timeException.Date.Date);
            DateTime endTime = Convert.ToDateTime(timeException.Date.ToShortDateString() + " " + "23:59:59");
            string[] leaveType = new LeaveServer().GetLeaveTypeByWorkID(timeException.WorkID, starTime, endTime).Split(';');
            string onbusinessBill = new OnBusinessBillServer().IsExistOnBusinessBillByWorkIDAndTime(
                                     timeException.WorkID, starTime, Convert.ToDateTime(endTime.AddDays(1).ToShortDateString()));
            DataTable dtOverTime = new OverTimeBillServer().IsExistOverTimeByWorkID(timeException.WorkID, starTime.Date, starTime.AddDays(1).Date);

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_TimeException
                             where a.ID == timeException.ID
                             select a;

                if (result.Count() > 0)
                {
                    HR_TimeException exceptionList = result.Single();
                    exceptionList.RealExceptionType = timeException.RealExceptionType;
                    exceptionList.HR_Signature = timeException.HR_Signature;
                    exceptionList.HR_SignatureDate = timeException.HR_SignatureDate;
                    exceptionList.ExceptionDescription = timeException.ExceptionDescription;

                    var resultList = from a in dataContxt.HR_AttendanceDaybookList
                                     where a.TimeExceptionRelevanceID == exceptionList.DayBookUniqueID
                                     select a;

                    if (resultList.Count() > 0)
                    {
                        HR_AttendanceDaybookList dayBookList = resultList.Single();

                        if (timeException.RealExceptionType == 4 && leaveType != null && leaveType[0] != "")
                        {
                            dayBookList.ResultType = "4";
                            dayBookList.BillNo = leaveType[0];

                            if (dayBookList.BillNo != "")
                            {
                                dayBookList.ResultSubclass = leaveType[1];
                                dayBookList.Remark = timeException.ExceptionDescription;
                            }
                            else
                            {
                                dayBookList.ResultSubclass = "";
                            }
                        }
                        else if (timeException.RealExceptionType == 7 && onbusinessBill != null)
                        {
                            dayBookList.ResultType = "7";
                            dayBookList.BillNo = onbusinessBill;
                        }
                        else if (timeException.RealExceptionType == 9 || timeException.RealExceptionType == 8
                            || timeException.RealExceptionType == 10 || timeException.RealExceptionType == 1
                            || timeException.RealExceptionType == 2 || timeException.RealExceptionType == 3
                            || timeException.RealExceptionType == 11 || timeException.RealExceptionType == 12)
                        {
                            dayBookList.ResultType = timeException.RealExceptionType.ToString();
                            dayBookList.BillNo = "";
                            dayBookList.ResultSubclass = "";
                        }
                        else if (timeException.RealExceptionType == 5 && dtOverTime != null && dtOverTime.Rows.Count > 0)
                        {
                            dayBookList.ResultType = "5";

                            if (dayBookList.BillNo == null || dayBookList.BillNo.Trim().Length == 0 || dayBookList.Hours == 0)
                            {
                                dayBookList.BillNo = dtOverTime.Rows[0]["单据号"].ToString();
                                dayBookList.ResultSubclass = dtOverTime.Rows[0]["补偿方式"].ToString();
                                dayBookList.Hours = Convert.ToDouble(dtOverTime.Rows[0]["实际小时数"].ToString());
                            }
                        }
                        else
                        {
                            timeException.RealExceptionType = timeException.RealExceptionType;
                            timeException.ExceptionDescription += " 没有检测到关联单；已被" + BasicInfo.LoginName + "强制处理";
                        }

                        dayBookList.Remark = timeException.ExceptionDescription +
                                            " 异常类型由之前的" + new AttendanceMachineServer().GetExceptionTypeName(result.Single().ExceptionType) +
                                            "更改为" + new AttendanceMachineServer().GetExceptionTypeName(timeException.RealExceptionType);
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

        double GetAlreadyHours(DepotManagementDataContext ctx, HR_AttendanceDaybookList dayBookList, string workID)
        {
            var varData = from a in ctx.HR_AttendanceDaybookList
                          join b in ctx.HR_AttendanceDaybook 
                          on a.DayBookID equals b.ID
                          where a.ResultType == dayBookList.ResultType
                          && a.BillNo == dayBookList.BillNo
                          && b.WorkID == workID
                          select a;

            if (varData.Count() == 0)
            {
                return 0;
            }
            else
            {
                return varData.Select(k => k.Hours).Sum();
            }
        }

        /// <summary>
        /// 修改考勤异常登记信息
        /// </summary>
        /// <param name="timeException">考勤异常登记信息数据集</param>
        /// <param name="role">角色（部门审核、人力资源审核）</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回true失败返回False</returns>
        public bool UpdateTimeException(HR_TimeException timeException,string role,out string error)
        {
            error = "";
            DateTime starTime = Convert.ToDateTime(timeException.Date.ToShortDateString() + " " + "08:30:00");
            DateTime endTime = Convert.ToDateTime(timeException.Date.ToShortDateString() + " " + "17:30:00");
            //string[] leaveType = new LeaveServer().GetLeaveTypeByWorkID(timeException.WorkID, endTime, starTime).Split(';');
            //DataTable onbusinessDt = new OnBusinessBillServer().GetOnBusinessInfo_TimeExceptionJudge(timeException);
            //DataTable dtOverTime = new OverTimeBillServer().GetOverTimeByWorkID(timeException.WorkID,starTime.Date,starTime.AddDays(1).Date);        

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_TimeException
                             where a.ID == timeException.ID
                             select a;

                if (result.Count() > 0)
                {
                    HR_TimeException exceptionList = result.Single();

                    switch (role)
                    {
                        case "部门审核":
                            exceptionList.DeptAuditor = timeException.DeptAuditor;
                            exceptionList.DeptAuditorSignatureDate = timeException.DeptAuditorSignatureDate;
                            exceptionList.ExceptionDescription = timeException.ExceptionDescription;
                            exceptionList.RealExceptionType = timeException.RealExceptionType;

                            if (timeException.RealExceptionType == 8)
                            {
                                //DateTime starDate = new DateTime(timeException.Date.Year, timeException.Date.Month, 1);
                                //int Days = DateTime.DaysInMonth(timeException.Date.Year, timeException.Date.Month);
                                //DateTime endDate = new DateTime(timeException.Date.Year, timeException.Date.Month, Days);

                                //if (!IsTimeExceptionCount(timeException.WorkID, starDate, endDate,
                                //    timeException.RealExceptionType.ToString(), out error))//漏打卡次数大于3
                                //{
                                //    error = "每人每个月漏打卡次数不能超过3次！" +
                                //        UniversalFunction.GetPersonnelName(timeException.WorkID) + "已有三次";
                                //    return false;
                                //}

                                exceptionList.RealExceptionType = timeException.RealExceptionType;
                            }

                            #region 文员只能处理已登记与漏打卡
                            //if (timeException.RealExceptionType == 7)
                            //{
                            //    if (onbusinessDt.Rows.Count > 0 && onbusinessDt.Rows[0]["单据号"].ToString() != "")
                            //    {
                            //        exceptionList.RealExceptionType = timeException.RealExceptionType;
                            //    }
                            //    else
                            //    {
                            //        error = m_personnerServer.GetPersonnelInfo(timeException.WorkID).Name + timeException.Date + "没有出差单或单据没有完成";
                            //        return false;
                            //    }
                            //}
                            //else if (timeException.RealExceptionType == 4)
                            //{
                            //    if (leaveType != null && leaveType[0] != "")
                            //    {
                            //        exceptionList.RealExceptionType = timeException.RealExceptionType;
                            //    }
                            //    else
                            //    {
                            //        error = m_personnerServer.GetPersonnelInfo(timeException.WorkID).Name + timeException.Date + "没有请假单或单据没有完成";
                            //        return false;
                            //    }
                            //}
                            //else if (timeException.RealExceptionType == 8)
                            //{
                            //    DateTime starDate = new DateTime(timeException.Date.Year, timeException.Date.Month, 1);
                            //    int Days = DateTime.DaysInMonth(timeException.Date.Year, timeException.Date.Month);
                            //    DateTime endDate = new DateTime(timeException.Date.Year, timeException.Date.Month, Days);

                            //    if (!IsTimeExceptionCount(timeException.WorkID, starDate, endDate, 
                            //        timeException.RealExceptionType.ToString(), out error))//漏打卡次数大于3
                            //    {                                    
                            //        error = "每人每个月漏打卡次数不能超过3次！" + 
                            //            UniversalFunction.GetPersonnelName(timeException.WorkID) + "已有三次";
                            //        return false;
                            //    }

                            //    exceptionList.RealExceptionType = timeException.RealExceptionType;
                            //}
                            #endregion
                            break;
                        case "人力资源审核":
                            exceptionList.RealExceptionType = timeException.RealExceptionType;
                            exceptionList.HR_Signature = timeException.HR_Signature;
                            exceptionList.HR_SignatureDate = timeException.HR_SignatureDate;
                            exceptionList.ExceptionDescription = timeException.ExceptionDescription;

                            var varData = from a in dataContxt.HR_AttendanceDaybookList
                                          where a.TimeExceptionRelevanceID == exceptionList.DayBookUniqueID
                                          select a;

                            if (varData.Count() == 1)
                            {
                                HR_AttendanceDaybookList dayBookList = varData.Single();

                                dayBookList.ResultType = timeException.RealExceptionType.ToString();
                                dayBookList.BillNo = "";
                                dayBookList.ResultSubclass = "";

                                if (result.Single().ExceptionType != timeException.RealExceptionType)
                                {
                                    if (exceptionList.DeptAuditor != null && exceptionList.DeptAuditor != "")
                                    {
                                        dayBookList.Remark = timeException.ExceptionDescription +
                                            " 异常类型由之前的" + new AttendanceMachineServer().GetExceptionTypeName(result.Single().ExceptionType) +
                                            "更改为" + new AttendanceMachineServer().GetExceptionTypeName(timeException.RealExceptionType) +
                                            "；更改人：" + new PersonnelArchiveServer().GetPersonnelInfo(exceptionList.DeptAuditor).Name;
                                    }
                                    else
                                    {
                                        dayBookList.Remark = timeException.ExceptionDescription +
                                            " 异常类型由之前的" + new AttendanceMachineServer().GetExceptionTypeName(result.Single().ExceptionType) +
                                            "更改为" + new AttendanceMachineServer().GetExceptionTypeName(timeException.RealExceptionType) +
                                            "；更改人：" + BasicInfo.LoginName;
                                    }
                                }
                            }


                            #region 文员只能处理已登记与漏打卡
                            //var varData = from a in dataContxt.HR_AttendanceDaybookList
                            //              where a.TimeExceptionRelevanceID == exceptionList.DayBookUniqueID
                            //              select a;

                            //if (varData.Count() == 1)
                            //{
                            //    HR_AttendanceDaybookList dayBookList = varData.Single();
                                
                            //    //请假
                            //    if (timeException.RealExceptionType == 4 && leaveType != null && leaveType[0] != "")
                            //    {
                            //        dayBookList.ResultType = "4";
                            //        dayBookList.BillNo = leaveType[0];
                            //        dayBookList.ResultSubclass = leaveType[1];

                            //    }//加班
                            //    else if (timeException.RealExceptionType == 5 && dtOverTime != null && dtOverTime.Rows.Count > 0)
                            //    {
                            //        dayBookList.ResultType = "5";

                            //        if (dayBookList.BillNo == null || dayBookList.BillNo.Trim().Length == 0 || dayBookList.Hours == 0)
                            //        {
                            //            dayBookList.BillNo = dtOverTime.Rows[0]["单据号"].ToString();
                            //            dayBookList.ResultSubclass = dtOverTime.Rows[0]["补偿方式"].ToString();

                            //            double alreadyHours = GetAlreadyHours(dataContxt, dayBookList, timeException.WorkID);
                            //            dayBookList.Hours = Convert.ToDouble(dtOverTime.Rows[0]["实际小时数"].ToString()) < alreadyHours ?
                            //                dayBookList.Hours : Convert.ToDouble(dtOverTime.Rows[0]["实际小时数"].ToString()) - alreadyHours;
                            //        }
                            //    }
                            //    else if (timeException.RealExceptionType == 7 && (onbusinessDt != null && onbusinessDt.Rows.Count > 0
                            //        && onbusinessDt.Rows[0]["单据号"].ToString() != ""))
                            //    {
                            //        dayBookList.ResultType = "7";
                            //        dayBookList.BillNo = onbusinessDt.Rows[0]["单据号"].ToString();
                            //    }
                            //    else if (timeException.RealExceptionType == 9 || timeException.RealExceptionType == 8
                            //        || timeException.RealExceptionType == 10 || timeException.RealExceptionType == 1
                            //        || timeException.RealExceptionType == 2 || timeException.RealExceptionType == 3
                            //        || timeException.RealExceptionType == 11 || timeException.RealExceptionType == 12 
                            //        || timeException.RealExceptionType == 13)
                            //    {
                            //        dayBookList.ResultType = timeException.RealExceptionType.ToString();
                            //        dayBookList.BillNo = "";
                            //        dayBookList.ResultSubclass = "";
                            //    }
                            //    else
                            //    {
                            //        error = "没有检测到关联单";
                            //        error += "【工号】：" + exceptionList.WorkID + "【时间】：" + exceptionList.Date.ToShortDateString();
                            //        timeException.ExceptionDescription += "；没有检测到关联单；";
                            //        timeException.ExceptionDescription += "【工号】：" + exceptionList.WorkID + "【时间】：" + exceptionList.Date.ToShortDateString();
                            //        return false;
                            //    }

                            //    if (result.Single().ExceptionType != timeException.RealExceptionType)
                            //    {
                            //        if (exceptionList.DeptAuditor != null && exceptionList.DeptAuditor != "")
                            //        {
                            //            dayBookList.Remark = timeException.ExceptionDescription +
                            //                " 异常类型由之前的" + new AttendanceMachineServer().GetExceptionTypeName(result.Single().ExceptionType) +
                            //                "更改为" + new AttendanceMachineServer().GetExceptionTypeName(timeException.RealExceptionType) +
                            //                "；更改人：" + new PersonnelArchiveServer().GetPersonnelInfo(exceptionList.DeptAuditor).Name;
                            //        }
                            //        else
                            //        {
                            //            dayBookList.Remark = timeException.ExceptionDescription +
                            //                " 异常类型由之前的" + new AttendanceMachineServer().GetExceptionTypeName(result.Single().ExceptionType) +
                            //                "更改为" + new AttendanceMachineServer().GetExceptionTypeName(timeException.RealExceptionType) +
                            //                "；更改人：" + BasicInfo.LoginName;
                            //        }
                            //    }

                            //    break;
                            //}
                            #endregion
                            break;
                        default:
                            exceptionList.RealExceptionType = timeException.RealExceptionType;
                            exceptionList.ExceptionDescription = timeException.ExceptionDescription;
                            break;
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
        /// 判断员工在一段时间内漏打卡异常类别的次数是否小于3
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="starDate">起始日期</param>
        /// <param name="endDate">截止日期</param>
        /// <param name="resultType">考勤类型</param>
        /// <param name="error">错误信息</param>
        /// <returns>大于返回false小于返回true</returns>
        bool IsTimeExceptionCount(string workID, DateTime starDate, DateTime endDate,string resultType, out string error)
        {
            error = "";

            try
            {
                string sql = "select Count(*) as count from DepotManagement.dbo.HR_TimeException " +
                             " where workID='" + workID + "' and (Date>= '" + starDate + "'" +
                             " and Date<= '" + endDate + "') and (RealExceptionType='8')";

                DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

                if (dt != null && dt.Rows.Count > 0)
                {
                    if (Convert.ToInt32(dt.Rows[0]["count"].ToString()) <= 2)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    error = "数据有误，考勤流水中操作终止！";
                    return false;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 操作补单情况下的考勤流水与异常信息
        /// </summary>
        /// <param name="dataContxt">数据上下文</param>
        /// <param name="billNo">关联单号</param>
        /// <param name="exceptionType">异常类型</param>
        /// <returns>成功返回True失败返回False</returns>
        public void OperationTimeException_Replenishments(DepotManagementDataContext dataContxt, string billNo, CE_HR_AttendanceExceptionType exceptionType)
        {
            IAttendanceAnalysis service = ServerModuleFactory.GetServerModule<IAttendanceAnalysis>();
            try
            {
                switch (exceptionType)
                {
                    case CE_HR_AttendanceExceptionType.请假:

                        var varDataLeave = from a in dataContxt.HR_LeaveBill
                                           where a.ID == Convert.ToInt32(billNo)
                                           select a;

                        if (varDataLeave.Count() != 1)
                        {
                            throw new Exception("请假单数据有误");
                        }

                        service.Analysis_Main(dataContxt, varDataLeave.Single().BeginTime, varDataLeave.Single().EndTime, varDataLeave.Single().Applicant);
                        break;
                    case CE_HR_AttendanceExceptionType.加班:
                        var varOverTime = from a in dataContxt.HR_OvertimeBill
                                          where a.ID == Convert.ToInt32(billNo)
                                          select a;

                        if (varOverTime.Count() != 1)
                        {
                            throw new Exception("请假单数据有误");
                        }

                        var varOverPersonnel = from a in dataContxt.HR_OvertimePersonnel
                                               where a.BillID == varOverTime.Single().ID
                                               select a;

                        foreach (var item in varOverPersonnel)
                        {
                            service.Analysis_Main(dataContxt, varOverTime.Single().BeginTime, Convert.ToDateTime(varOverTime.Single().EndTime), item.WorkID);
                        }

                        break;
                    case CE_HR_AttendanceExceptionType.出差:
                        var varOnBusiness = from a in dataContxt.HR_OnBusinessBill
                                            where a.ID == Convert.ToInt32(billNo)
                                            select a;

                        if (varOnBusiness.Count() != 1)
                        {
                            throw new Exception("请假单数据有误");
                        }

                        var varPersonnel = from a in dataContxt.HR_OnBusinessPersonnel
                                           where a.BillID == varOnBusiness.Single().ID
                                           select a;

                        foreach (var item in varPersonnel)
                        {
                            service.Analysis_Main(dataContxt, varOnBusiness.Single().RealBeginTime, varOnBusiness.Single().RealEndTime, item.WorkID);
                        }

                        break;
                    default:
                        break;
                }

                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
