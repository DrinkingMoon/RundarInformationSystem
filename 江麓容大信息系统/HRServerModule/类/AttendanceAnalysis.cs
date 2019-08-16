using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using ServerModule;
using System.Data;
using GlobalObject;

namespace Service_Peripheral_HR
{
    class AttendanceAnalysis : IAttendanceAnalysis
    {
        #region 通用变量

        string _DateTime_PunchOut_Begin = "";
        string _DateTime_PunchOut_End = "";
        string _DateTime_PunchIn_Begin = "";
        string _DateTime_PunchIn_End = "";
        string _DateTime_Morning_Begin = "";
        string _DateTime_Morning_End = "";
        string _DateTime_Afternoon_Begin = "";
        string _DateTime_Afternoon_End = "";

        View_HR_PersonnelArchive _PersonnelInfo = new View_HR_PersonnelArchive();
        DateTime _AnalysisDate = new DateTime();

        List<string> _ListHolidayType_Scheduling = new List<string>();
        List<CE_HR_AttendanceExceptionType> _List_OperationExceptionType = new List<CE_HR_AttendanceExceptionType>();
        List<Entity_AttendanceDateTimeRules> _List_GetInfo_Rules = new List<Entity_AttendanceDateTimeRules>();

        List<HR_Holiday> _List_GetInfo_Holiday = new List<HR_Holiday>();
        //List<HR_BatchException> _List_GetInfo_BatchException = new List<HR_BatchException>();

        List<Entity_AttendanceDateTimeRules> _List_Rules_Date = new List<Entity_AttendanceDateTimeRules>();
        List<Entity_AttendanceRecordTime> _List_RecordTime_Date = new List<Entity_AttendanceRecordTime>();
        List<HR_LeaveBill> _List_LeaveBillInfo_Date = new List<HR_LeaveBill>();
        List<HR_OnBusinessBill> _List_OnBusinessBillInfo_Date = new List<HR_OnBusinessBill>();
        List<HR_OvertimeBill> _List_OverTimeBillInfo_Date = new List<HR_OvertimeBill>();
        List<HR_BatchException> _List_BatchExceptionInfo_Date = new List<HR_BatchException>();

        List<HR_TimeException> _List_TimeException_Date = new List<HR_TimeException>();
        List<HR_OvertimeBill> _List_Rules_OverTimeBill = new List<HR_OvertimeBill>();
        List<HR_AttendanceDaybookList> _List_Rules_AttendanceBook = new List<HR_AttendanceDaybookList>();
        List<HR_AttendanceDaybookList> _List_Day_AttendanceBook = new List<HR_AttendanceDaybookList>();

        #endregion

        #region 信息获取赋值

        HR_AttendanceExceptionType GetExceptionTypeInfo(DepotManagementDataContext ctx, int typeID)
        {
            var varData = from a in ctx.HR_AttendanceExceptionType
                          where a.ID == typeID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else
            {
                return null;
            }
        }

        HR_HolidayType GetHolidayType(int typeID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_HolidayType
                          where a.ID == typeID
                          select a;

            return varData.First();
        }

        string GetInfo_SchedulingCode(DepotManagementDataContext ctx, CE_HR_SchedulingType schedulingType)
        {
            string schedulingTypeName = schedulingType.ToString();

            var varData = from a in ctx.HR_WorkSchedulingDefinition
                          where a.Name == schedulingTypeName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single().Code;
            }
            else
            {
                throw new Exception("排班考勤方案名称【" + schedulingType.ToString() + "】无法匹配对应的编码");
            }
        }

        string GetInfo_LeaveTypeCode(DepotManagementDataContext ctx, CE_HR_LeaveType leaveType)
        {
            string leaveTypeName = leaveType.ToString();

            if (leaveTypeName.Contains('_'))
            {
                leaveTypeName = leaveTypeName.Replace('_', '（') + "）";

                if (leaveTypeName.Substring(leaveTypeName.IndexOf("（")).Contains("至"))
                {
                    leaveTypeName = leaveTypeName.Replace('至', '-');
                }
            }

            var varData = from a in ctx.HR_LeaveType
                          where a.TypeName == leaveTypeName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single().TypeCode;
            }
            else
            {
                throw new Exception("请假类别名称【" + leaveType.ToString() + "】无法匹配对应的编码");
            }
        }

        public string GetInfo_AttendanceSchemeCode(DepotManagementDataContext ctx, CE_HR_AttendanceScheme scheme)
        {
            string schemeName = scheme.ToString();

            var varData = from a in ctx.HR_AttendanceScheme
                          where a.SchemeName == schemeName
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single().SchemeCode;
            }
            else
            {
                throw new Exception("考勤方案名称【"+ scheme.ToString() +"】无法匹配对应的编码");
            }
        }

        List<View_HR_PersonnelArchive> GetInfo_NeedAttendancePersonnel(DepotManagementDataContext ctx, DateTime startTime, string workID)
        {
            var varData = from a in ctx.View_HR_PersonnelArchive
                          where ((DateTime)a.离职时间 >= startTime || a.人员状态 == CE_HR_PersonnelStatus.在职.ToString())
                          select a;

            if (workID != null && workID.Trim().Length > 0)
            {
                varData = from a in varData
                          where a.员工编号 == workID
                          select a;
            }

            return varData.OrderBy(k => k.员工编号).ToList();
        }

        void IsAttendanceSettingNull(DepotManagementDataContext ctx, List<View_HR_PersonnelArchive> listNeedAttendancePersonnelInfo, DateTime startTime, DateTime endTime)
        {
            List<string> listTemp = new List<string>();

            listTemp.Add(GetInfo_AttendanceSchemeCode(ctx, CE_HR_AttendanceScheme.自然月考勤));
            listTemp.Add(GetInfo_AttendanceSchemeCode(ctx, CE_HR_AttendanceScheme.非自然月考勤));

            var varData1 = from a in ctx.HR_AttendanceScheme
                          where listTemp.Contains(a.SchemeCode)
                          select a;

            foreach (HR_AttendanceScheme item1 in varData1)
            {
                if (item1.BeginTimeInTheAfternoon == null 
                    || item1.BeginTimeInTheMorning == null 
                    || item1.EndTimeInTheAfternoon == null
                    || item1.EndTimeInTheMorning == null)
                {
                    throw new Exception("【"+ item1.SchemeName +"】考勤方案的考勤时间设置不完整");
                }
            }

            string code = GetInfo_SchedulingCode(ctx, CE_HR_SchedulingType.公休);

            var varData2 = from a in ctx.HR_WorkSchedulingDefinition
                           where a.Code != code
                           select a;

            foreach (HR_WorkSchedulingDefinition item2 in varData2)
            {
                string strDate = ServerTime.Time.ToShortDateString();
                DateTime tempDateTime = ServerTime.Time;

                if (item2.BeginTime == null 
                    || item2.BeginTime.Trim().Length == 0
                    || item2.EndTime == null 
                    || item2.EndTime.Trim().Length == 0
                    ||!DateTime.TryParse(strDate + " " + item2.BeginTime, out tempDateTime)
                    || !DateTime.TryParse(strDate + " " + item2.EndTime, out tempDateTime))
                {
                    throw new Exception("【" + item2.Name + "】考勤方案的考勤时间设置不完整");
                }
            }


            foreach (View_HR_PersonnelArchive item in listNeedAttendancePersonnelInfo)
            {
                var varData = from a in ctx.HR_AttendanceSetting
                              where a.WorkID == item.员工编号
                              select a;

                if (varData == null || varData.Count() == 0)
                {
                    throw new Exception(item.员工姓名 + "未设置考勤方案");
                }
                else if (varData.Count() > 1)
                {
                    throw new Exception(item.员工姓名 + "考勤方案不唯一");
                }
                else
                {
                    //HR_AttendanceSetting settingInfo = varData.Single();

                    //if (settingInfo.SchemeCode == GetInfo_AttendanceSchemeCode(ctx, CustomEnum_AttendanceScheme.自然月排班考勤))
                    //{
                    //    DateTime tempDate = startTime;

                    //    while (tempDate <= endTime)
                    //    {
                    //        var varData3 = from a in ctx.HR_WorkSchedulingDetail
                    //                       where a.WorkID == item.员工编号
                    //                       && a.Date.Date == tempDate.Date
                    //                       select a;

                    //        if (varData3.Count() == 0)
                    //        {
                    //            throw new Exception(item.员工姓名 + "【" + CustomEnum_AttendanceScheme.自然月排班考勤.ToString() + "】方案在【"
                    //                + tempDate.ToShortDateString() +"】未设置排班");
                    //        }
                    //    }
                    //}
                }
            }
        }

        List<HR_AttendanceMachineDataList> GetMachineDataList(DepotManagementDataContext ctx)
        {
            var varData = from a in ctx.HR_AttendanceMachineDataList
                          where a.WorkID == _PersonnelInfo.员工编号
                          && a.Date.Date == _AnalysisDate.Date
                          select a;

            return varData.ToList();
        }

        List<Entity_AttendanceRecordTime> GetInfo_RecordTime(DepotManagementDataContext ctx)
        {
            List<Entity_AttendanceRecordTime> listResult = new List<Entity_AttendanceRecordTime>();
            List<HR_AttendanceMachineDataList> listMachine = GetMachineDataList(ctx);

            foreach (HR_AttendanceMachineDataList item1 in listMachine)
            {
                string recordTime = item1.RecordTime;

                if (recordTime == null || recordTime.Trim().Length == 0)
                {
                    return new List<Entity_AttendanceRecordTime>();
                }

                string[] arrayTime = recordTime.Split(' ');

                foreach (string item in arrayTime)
                {
                    DateTime tempDate = new DateTime();
                    Entity_AttendanceRecordTime tempTime = new Entity_AttendanceRecordTime();

                    if (DateTime.TryParse(item, out tempDate))
                    {
                        tempTime.RecordTime = Convert.ToDateTime(item);
                        listResult.Add(tempTime);
                    }
                }
            }

            return listResult.OrderBy( k => k.RecordTime).ToList();
        }

        HR_AttendanceSetting GetInfo_AttendanceSetting(DepotManagementDataContext ctx)
        {
            var varData = from a in ctx.HR_AttendanceSetting
                          where a.WorkID == _PersonnelInfo.员工编号
                          select a;

            if (varData == null || varData.Count() != 1)
            {
                throw new Exception(_PersonnelInfo.员工姓名 + "考勤方案不存在或者不唯一");
            }
            else
            {
                return varData.Single();
            }
        }

        List<HR_WorkSchedulingDetail> GetInfo_SinglePersonnel_Scheduling(DepotManagementDataContext ctx,  DateTime date)
        {
            List<HR_WorkSchedulingDetail> listResult = new List<HR_WorkSchedulingDetail>();

            var varData = from a in ctx.HR_WorkScheduling
                          join b in ctx.HR_WorkSchedulingDetail
                          on a.ID equals b.ParentID
                          where b.Date == date.Date
                          && b.WorkID == _PersonnelInfo.员工编号
                          && (a.BillStatus == "已完成" || a.BillStatus == "等待下次排班")
                          select b;


            if (varData != null)
            {
                foreach (HR_WorkSchedulingDetail item1 in varData)
                {
                    HR_WorkSchedulingDetail schedulInfo = new HR_WorkSchedulingDetail();

                    schedulInfo.Code = item1.Code;
                    schedulInfo.Date = item1.Date;
                    schedulInfo.ID = item1.ID;
                    schedulInfo.ParentID = item1.ParentID;
                    schedulInfo.Remark = item1.Remark;
                    schedulInfo.WorkID = item1.WorkID;

                    listResult.Add(schedulInfo);
                }
            }

            return listResult;
        }

        object GetInfo_All(DepotManagementDataContext ctx, DateTime startTime, DateTime endTime, CE_HR_AttendanceExceptionType billType)
        {
            switch (billType)
            {
                case CE_HR_AttendanceExceptionType.加班:

                    var varData = from a in ctx.HR_OvertimeBill
                                  where a.BeginTime < endTime.Date.Date.AddDays(1)
                                  && ((DateTime)a.EndTime) >= startTime.Date.Date
                                  && a.BillStatus == "已完成"
                                  select a;

                    return varData.ToList();

                case CE_HR_AttendanceExceptionType.出差:

                    var varTemp1 = from a in ctx.HR_OnBusinessBill
                                   where a.RealBeginTime < endTime.Date.Date.AddDays(1)
                                   && a.RealEndTime.Date >= startTime.Date.Date
                                   && (a.BillStatus == OnBusinessBillStatus.已完成.ToString()
                                   || a.BillStatus == OnBusinessBillStatus.等待出差结果说明.ToString()
                                   || a.BillStatus == OnBusinessBillStatus.等待销差人确认.ToString())
                                   select a;

                    return varTemp1.ToList();

                case CE_HR_AttendanceExceptionType.请假:
                    var varTemp2 = from a in ctx.HR_LeaveBill
                                   where a.BeginTime < endTime.Date.AddDays(1)
                                   && a.EndTime.Date >= startTime.Date.Date
                                   && a.BillStatus == "已完成"
                                   select a;

                    return varTemp2.ToList();
                case CE_HR_AttendanceExceptionType.节假:

                    List<HR_Holiday> listResult = new List<HR_Holiday>();

                    while (endTime >= startTime)
                    {
                        var varData1 = from a in ctx.HR_Holiday
                                       where a.BeginTime < startTime.AddDays(1)
                                       && a.EndTime.Date >= startTime
                                       select a;

                        foreach (HR_Holiday item in varData1)
                        {
                            HR_Holiday holiday = new HR_Holiday();

                            holiday.Remark = item.Remark;
                            holiday.ID = item.ID;
                            holiday.Recorder = item.Recorder;
                            holiday.RecordTime = startTime.Date;
                            holiday.HolidayTypeID = item.HolidayTypeID;
                            holiday.EndTime = Convert.ToDateTime(startTime.Date.ToShortDateString() + " "
                                + item.EndTime.ToShortTimeString());
                            holiday.BeginTime = Convert.ToDateTime(startTime.Date.ToShortDateString() + " "
                                + item.BeginTime.ToShortTimeString());
                            //holiday.是否法定节假日 = item.是否法定节假日;
                            //holiday.是否周末 = item.是否周末;
                            holiday.ApplicableDeptCode = item.ApplicableDeptCode;
                            holiday.ApplicableSex = item.ApplicableSex;
                            holiday.Days = item.Days > 1 ? 1 : item.Days;

                            listResult.Add(holiday);
                        }

                        startTime = startTime.AddDays(1);
                    }

                    return listResult;
                case CE_HR_AttendanceExceptionType.集体异常:

                    List<HR_BatchException> listResult1 = new List<HR_BatchException>();

                    var varData2 = from a in ctx.HR_BatchException
                                   where a.BeginTime < endTime.Date.Date.AddDays(1)
                                   && ((DateTime)a.EndTime).Date >= startTime.Date.Date
                                   && a.HR_Director.Trim().Length > 0
                                   select a;

                    return varData2.ToList();
                default:
                    break;
            }

            return null;
        }

        object GetInfo_Personel<T>(DepotManagementDataContext ctx, List<T> list, CE_HR_AttendanceExceptionType billType)
        {
            switch (billType)
            {
                case CE_HR_AttendanceExceptionType.加班:
                    var varData = from a in list as List<HR_OvertimeBill>
                                  join b in ctx.HR_OvertimePersonnel
                                  on a.ID equals b.BillID
                                  where b.WorkID == _PersonnelInfo.员工编号
                                  select a;

                    return varData.ToList();
                case CE_HR_AttendanceExceptionType.出差:
                    var varTemp1 = from a in list as List<HR_OnBusinessBill>
                                   join b in ctx.HR_OnBusinessPersonnel
                                   on a.ID equals b.BillID
                                   where b.WorkID == _PersonnelInfo.员工编号
                                   select a;

                    return varTemp1.ToList();
                case CE_HR_AttendanceExceptionType.请假:
                    var varTemp2 = from a in list as List<HR_LeaveBill>
                                   where a.Applicant == _PersonnelInfo.员工编号
                                   select a;

                    if (GetInfo_AttendanceSetting(ctx).SchemeCode == GetInfo_AttendanceSchemeCode(ctx, CE_HR_AttendanceScheme.自然月排班考勤))
                    {
                        varTemp2 = from a in varTemp2
                                   where !_ListHolidayType_Scheduling.Contains(a.LeaveTypeID)
                                   select a;
                    }

                    return varTemp2.ToList();
                case CE_HR_AttendanceExceptionType.集体异常:
                    var varTemp3 = from a in list as List<HR_BatchException>
                                   join b in ctx.HR_BatchException_Personnel
                                   on a.ID equals b.BillID
                                   where b.WorkID == _PersonnelInfo.员工编号
                                   select a;

                    return varTemp3.ToList();
                default:
                    break;
            }

            return null;
        }

        object GetInfo_Date<T>(DepotManagementDataContext ctx, List<T> list, CE_HR_AttendanceExceptionType billType)
        {
            switch (billType)
            {
                case CE_HR_AttendanceExceptionType.应考勤:

                    List<Entity_AttendanceDateTimeRules> listResult = (from a in list as List<Entity_AttendanceDateTimeRules>
                                                                           where a.DayDate.Date == _AnalysisDate.Date
                                                                           select a).ToList();

                    foreach (HR_BatchException exception in _List_BatchExceptionInfo_Date)
                    {
                        bool flag = false;
                        listResult = SiftRules(CE_HR_AttendanceExceptionType.集体异常, exception.ID.ToString(),
                            listResult, exception.BeginTime, exception.EndTime, ref flag);
                        if (flag)
                        {
                            HR_AttendanceDaybookList tempInfo = new HR_AttendanceDaybookList();

                            tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.集体异常).ToString();
                            tempInfo.ResultSubclass = "";
                            tempInfo.BillNo = exception.ID.ToString();
                            tempInfo.Hours = (exception.EndTime - exception.BeginTime).TotalHours;
                            tempInfo.Remark = string.Format("关联{0}单", CE_HR_AttendanceExceptionType.集体异常.ToString());
                            tempInfo.ObjectClockInTime = exception.BeginTime.ToShortTimeString() + " " + exception.EndTime.ToShortTimeString();
                            tempInfo.RealClockInTime = "";

                            _List_Day_AttendanceBook.Add(tempInfo);
                        }
                    }

                    foreach (HR_OnBusinessBill onBusiness in _List_OnBusinessBillInfo_Date)
                    {
                        bool flag = false;
                        listResult = SiftRules(CE_HR_AttendanceExceptionType.出差, onBusiness.ID.ToString(), 
                            listResult, onBusiness.RealBeginTime, onBusiness.RealEndTime, ref flag);
                        if (flag)
                        {
                            HR_AttendanceDaybookList tempInfo = new HR_AttendanceDaybookList();

                            tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.出差).ToString();
                            tempInfo.ResultSubclass = "";
                            tempInfo.BillNo = onBusiness.ID.ToString();
                            tempInfo.Hours = (onBusiness.RealEndTime - onBusiness.RealBeginTime).TotalHours;
                            tempInfo.Remark = string.Format("关联{0}单", CE_HR_AttendanceExceptionType.出差.ToString());
                            tempInfo.ObjectClockInTime = onBusiness.RealBeginTime.ToShortTimeString() + " " + onBusiness.RealEndTime.ToShortTimeString();
                            tempInfo.RealClockInTime = "";

                            _List_Day_AttendanceBook.Add(tempInfo);
                        }
                    }

                    foreach (HR_LeaveBill leaveBill in _List_LeaveBillInfo_Date)
                    {
                        bool flag = false;
                        listResult = SiftRules(CE_HR_AttendanceExceptionType.请假, leaveBill.ID.ToString(), 
                            listResult, leaveBill.BeginTime, leaveBill.EndTime, ref flag);
                        if (flag)
                        {
                            HR_AttendanceDaybookList tempInfo = new HR_AttendanceDaybookList();

                            tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.请假).ToString();
                            tempInfo.ResultSubclass = leaveBill.LeaveTypeID;
                            tempInfo.BillNo = leaveBill.ID.ToString();
                            tempInfo.Hours = (leaveBill.EndTime - leaveBill.BeginTime).TotalHours;
                            tempInfo.Remark = string.Format("关联{0}单", CE_HR_AttendanceExceptionType.请假.ToString());
                            tempInfo.ObjectClockInTime = leaveBill.BeginTime.ToShortTimeString() + " " + leaveBill.EndTime.ToShortTimeString();
                            tempInfo.RealClockInTime = "";

                            _List_Day_AttendanceBook.Add(tempInfo);
                        }
                    }

                    return listResult;
                case CE_HR_AttendanceExceptionType.加班:
                    List<HR_OvertimeBill> listReuslt = new List<HR_OvertimeBill>();

                    var varData = from a in list as List<HR_OvertimeBill>
                                  where a.BeginTime < _AnalysisDate.Date.AddDays(1)
                                  && ((DateTime)a.EndTime) >= _AnalysisDate.Date
                                  select a;

                    foreach (HR_OvertimeBill item in varData)
                    {
                        HR_OvertimeBill tempLnq = new HR_OvertimeBill();

                        tempLnq.CompensateMode = item.CompensateMode;
                        tempLnq.BeginTime = item.BeginTime.Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.BeginTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " 00:00:00");
                        tempLnq.EndTime = ((DateTime)item.EndTime).Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.EndTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " 23:59:59");
                        tempLnq.Errand = item.Errand;
                        tempLnq.OvertimeAddress = item.OvertimeAddress;
                        tempLnq.Hours = (decimal)(((DateTime)tempLnq.EndTime) - item.BeginTime).TotalHours;
                        tempLnq.ID = item.ID;
                        tempLnq.RealHours = (((DateTime)tempLnq.EndTime) - item.BeginTime).TotalHours;

                        listReuslt.Add(tempLnq);
                    }

                    return listReuslt;
                case CE_HR_AttendanceExceptionType.出差:

                    List<HR_OnBusinessBill> listOnBusiness = new List<HR_OnBusinessBill>();

                    var varTemp1 = from a in list as List<HR_OnBusinessBill>
                                   where a.RealBeginTime < _AnalysisDate.Date.AddDays(1)
                                   && ((DateTime)a.RealEndTime) >= _AnalysisDate.Date
                                   select a;

                    foreach (HR_OnBusinessBill item in varTemp1)
                    {
                        HR_OnBusinessBill tempLnq = new HR_OnBusinessBill();

                        tempLnq.RealBeginTime = item.RealBeginTime.Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.RealBeginTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " " + _DateTime_Morning_Begin);
                        tempLnq.RealEndTime = ((DateTime)item.RealEndTime).Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.RealEndTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " " + _DateTime_Afternoon_End);
                        tempLnq.ID = item.ID;

                        listOnBusiness.Add(tempLnq);
                    }

                    return listOnBusiness;
                case CE_HR_AttendanceExceptionType.请假:

                    List<HR_LeaveBill> listLeave = new List<HR_LeaveBill>();

                    var varTemp2 = from a in list as List<HR_LeaveBill>
                                   where a.BeginTime < _AnalysisDate.Date.AddDays(1)
                                   && ((DateTime)a.EndTime) >= _AnalysisDate.Date
                                   select a;

                    foreach (HR_LeaveBill item in varTemp2)
                    {
                        HR_LeaveBill tempLnq = new HR_LeaveBill();

                        tempLnq.LeaveTypeID = item.LeaveTypeID;
                        tempLnq.BeginTime = item.BeginTime.Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.BeginTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " " + _DateTime_Morning_Begin);
                        tempLnq.EndTime = ((DateTime)item.EndTime).Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.EndTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " " + _DateTime_Afternoon_End);
                        tempLnq.ID = item.ID;

                        listLeave.Add(tempLnq);
                    }

                    return listLeave;
                case CE_HR_AttendanceExceptionType.集体异常:

                    List<HR_BatchException> lstException = new List<HR_BatchException>();

                    var varTemp3 = from a in list as List<HR_BatchException>
                                   where a.BeginTime < _AnalysisDate.Date.AddDays(1)
                                   && ((DateTime)a.EndTime) >= _AnalysisDate.Date
                                   select a;

                    foreach (HR_BatchException item in varTemp3)
                    {
                        HR_BatchException tempLnq = new HR_BatchException();

                        tempLnq.BeginTime = item.BeginTime.Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.BeginTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " " + _DateTime_Morning_Begin);
                        tempLnq.EndTime = ((DateTime)item.EndTime).Date == _AnalysisDate.Date ?
                            ServerTime.ConvertToDateTime(item.EndTime) : Convert.ToDateTime(_AnalysisDate.Date.ToShortDateString() + " " + _DateTime_Afternoon_End);
                        tempLnq.ID = item.ID;

                        lstException.Add(tempLnq);
                    }

                    return lstException;
                default:
                    break;
            }

            return null;
        }

        List<Entity_AttendanceDateTimeRules> SiftRules(CE_HR_AttendanceExceptionType exceptionType, string billNo, List<Entity_AttendanceDateTimeRules> listTemp,
            DateTime startTime, DateTime endTime, ref bool flag)
        {
            List<Entity_AttendanceDateTimeRules> listResult = new List<Entity_AttendanceDateTimeRules>();
            foreach (Entity_AttendanceDateTimeRules tempRules in listTemp)
            {
                if (tempRules.StartTime < endTime && tempRules.EndTime > startTime)
                {
                    flag = true;
                    if (tempRules.StartTime < startTime)
                    {
                        Entity_AttendanceDateTimeRules temp1 = new Entity_AttendanceDateTimeRules();

                        temp1.DayDate = tempRules.DayDate;

                        temp1.StartExceptionType = tempRules.StartExceptionType;
                        temp1.StartExceptionBillNo = tempRules.StartExceptionBillNo;
                        temp1.StartTime = tempRules.StartTime;
                        temp1.StartRecode_IsExist = tempRules.StartRecode_IsExist;
                        temp1.StartRecord_Begin = tempRules.StartRecord_Begin;
                        temp1.StartRecode_End = tempRules.StartRecode_End;

                        temp1.EndExceptionType = exceptionType;
                        temp1.EndExceptionBillNo = billNo;
                        temp1.EndTime = startTime;
                        temp1.EndRcord_IsExist = true;
                        temp1.EndRcord_Begin = startTime;
                        temp1.EndRecod_End = endTime;

                        listResult.Add(temp1);
                    }

                    if (tempRules.EndTime > endTime)
                    {
                        Entity_AttendanceDateTimeRules temp1 = new Entity_AttendanceDateTimeRules();

                        temp1.DayDate = tempRules.DayDate;

                        temp1.StartExceptionType = exceptionType;
                        temp1.StartExceptionBillNo = billNo;
                        temp1.StartTime = endTime;
                        temp1.StartRecode_IsExist = true;
                        temp1.StartRecord_Begin = startTime;
                        temp1.StartRecode_End = endTime;

                        temp1.EndExceptionType = tempRules.EndExceptionType;
                        temp1.EndExceptionBillNo = tempRules.EndExceptionBillNo;
                        temp1.EndTime = tempRules.EndTime;
                        temp1.EndRcord_IsExist = tempRules.EndRcord_IsExist;
                        temp1.EndRcord_Begin = tempRules.EndRcord_Begin;
                        temp1.EndRecod_End = tempRules.EndRecod_End;

                        listResult.Add(temp1);
                    }
                }
                else
                {
                    listResult.Add(tempRules);
                }
            }

            return listResult;
        }

        List<Entity_AttendanceDateTimeRules> GetSureRules_Personnel(DepotManagementDataContext ctx, DateTime startTime, DateTime endTime)
        {
            List<Entity_AttendanceDateTimeRules> listResult = new List<Entity_AttendanceDateTimeRules>();

            var varData = from a in ctx.HR_AttendanceScheme
                          join b in ctx.HR_AttendanceSetting
                          on a.SchemeCode equals b.SchemeCode
                          where b.WorkID == _PersonnelInfo.员工编号
                          select a;

            if (varData.Count() != 1)
            {
                throw new Exception("工号【" + _PersonnelInfo.员工编号 + "】:未设置考勤方案");
            }

            HR_AttendanceScheme lnqShceme = varData.Single();
            CE_HR_AttendanceScheme scheme = GlobalObject.GeneralFunction.StringConvertToEnum<CE_HR_AttendanceScheme>(lnqShceme.SchemeName);

            DateTime dateTemp = startTime.Date;

            while (dateTemp <= endTime.Date)
            {
                switch (scheme)
                {
                    case CE_HR_AttendanceScheme.自然月考勤:
                    case CE_HR_AttendanceScheme.非自然月考勤:

                        var varData1 = from a in _List_GetInfo_Holiday
                                       where a.BeginTime < dateTemp.AddDays(1) && a.EndTime >= dateTemp
                                       && ((a.ApplicableSex == "全部" || a.ApplicableSex == _PersonnelInfo.性别)
                                       || (a.ApplicableDeptCode.Split(';').Contains(_PersonnelInfo.部门) || a.ApplicableDeptCode == "全部"))
                                       select a;
                        //上午
                        Entity_AttendanceDateTimeRules tempRuleMorning = new Entity_AttendanceDateTimeRules();
                        tempRuleMorning.DayDate = dateTemp;
                        tempRuleMorning.StartTime =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Morning_Begin);
                        tempRuleMorning.EndTime =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Morning_End);

                        tempRuleMorning.StartExceptionType = CE_HR_AttendanceExceptionType.正常;
                        tempRuleMorning.StartExceptionBillNo = "";
                        tempRuleMorning.StartRecode_IsExist = true;
                        tempRuleMorning.StartRecord_Begin =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_PunchIn_Begin);
                        tempRuleMorning.StartRecode_End =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_PunchIn_End);

                        tempRuleMorning.EndExceptionType = CE_HR_AttendanceExceptionType.正常;
                        tempRuleMorning.EndExceptionBillNo = "";
                        tempRuleMorning.EndRcord_IsExist = false;
                        tempRuleMorning.EndRcord_Begin =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Morning_End);
                        tempRuleMorning.EndRecod_End =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Afternoon_Begin);

                        if (varData1.Where(k => k.BeginTime <= tempRuleMorning.StartTime && k.EndTime >= tempRuleMorning.EndTime).Count() == 0)
                        {
                            listResult.Add(tempRuleMorning);
                        }

                        //下午
                        Entity_AttendanceDateTimeRules tempRuleAfternoon = new Entity_AttendanceDateTimeRules();
                        tempRuleAfternoon.DayDate = dateTemp;
                        tempRuleAfternoon.StartTime =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Afternoon_Begin);
                        tempRuleAfternoon.EndTime =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Afternoon_End);

                        tempRuleAfternoon.StartExceptionType = CE_HR_AttendanceExceptionType.正常;
                        tempRuleAfternoon.StartExceptionBillNo = "";
                        tempRuleAfternoon.StartRecode_IsExist = false;
                        tempRuleAfternoon.StartRecord_Begin =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Morning_End);
                        tempRuleAfternoon.StartRecode_End =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_Afternoon_Begin);

                        tempRuleAfternoon.EndExceptionType = CE_HR_AttendanceExceptionType.正常;
                        tempRuleAfternoon.EndExceptionBillNo = "";
                        tempRuleAfternoon.EndRcord_IsExist = true;
                        tempRuleAfternoon.EndRcord_Begin =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_PunchOut_Begin);
                        tempRuleAfternoon.EndRecod_End =
                            Convert.ToDateTime(dateTemp.ToShortDateString() + " " + _DateTime_PunchOut_End);

                        if (varData1.Where(k => k.BeginTime <= tempRuleAfternoon.StartTime && k.EndTime >= tempRuleAfternoon.EndTime).Count() == 0)
                        {
                            listResult.Add(tempRuleAfternoon);
                        }

                        break;
                    case CE_HR_AttendanceScheme.自然月排班考勤:
                    case CE_HR_AttendanceScheme.非自然月排班考勤:

                        List<HR_WorkSchedulingDetail> listDetail = GetInfo_SinglePersonnel_Scheduling(ctx, dateTemp);

                        var varData3 = from a in listDetail
                                       join b in ctx.HR_WorkSchedulingDefinition
                                       on a.Code equals b.Code
                                       select b;

                        foreach (HR_WorkSchedulingDefinition item in varData3)
                        {
                            if (item.Name == CE_HR_SchedulingType.公休.ToString())
                            {
                                continue;
                            }
                            else if (item.EndOffsetDays == 0)
                            {
                                Entity_AttendanceDateTimeRules tempScheduling = new Entity_AttendanceDateTimeRules();
                                tempScheduling.DayDate = dateTemp;
                                tempScheduling.StartTime =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.BeginTime);
                                tempScheduling.EndTime =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.EndTime);

                                tempScheduling.StartExceptionType = CE_HR_AttendanceExceptionType.正常;
                                tempScheduling.StartExceptionBillNo = "";
                                tempScheduling.StartRecode_IsExist = true;
                                tempScheduling.StartRecord_Begin =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchInBeginTime);
                                tempScheduling.StartRecode_End =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchInEndTime);

                                tempScheduling.EndExceptionType = CE_HR_AttendanceExceptionType.正常;
                                tempScheduling.EndExceptionBillNo = "";
                                tempScheduling.EndRcord_IsExist = true;
                                tempScheduling.EndRcord_Begin =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchOutBeginTime);
                                tempScheduling.EndRecod_End =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchOutEndTime);

                                listResult.Add(tempScheduling);
                            }
                            else if (item.EndOffsetDays == 1)
                            {
                                Entity_AttendanceDateTimeRules tempScheduling = new Entity_AttendanceDateTimeRules();
                                tempScheduling.DayDate = dateTemp;
                                tempScheduling.StartTime =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.BeginTime);
                                tempScheduling.EndTime =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " 23:59:59");

                                tempScheduling.StartExceptionType = CE_HR_AttendanceExceptionType.正常;
                                tempScheduling.StartExceptionBillNo = "";
                                tempScheduling.StartRecode_IsExist = true;
                                tempScheduling.StartRecord_Begin =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchInBeginTime);
                                tempScheduling.StartRecode_End =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchInEndTime);

                                tempScheduling.EndExceptionType = CE_HR_AttendanceExceptionType.正常;
                                tempScheduling.EndExceptionBillNo = "";
                                tempScheduling.EndRcord_IsExist = false;
                                tempScheduling.EndRcord_Begin =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " 23:59:59");
                                tempScheduling.EndRecod_End =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " 23:59:59");

                                listResult.Add(tempScheduling);

                                Entity_AttendanceDateTimeRules tempScheduling1 = new Entity_AttendanceDateTimeRules();
                                tempScheduling1.DayDate = dateTemp;
                                tempScheduling1.StartTime =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " 00:00:00");
                                tempScheduling1.EndTime =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.EndTime);

                                tempScheduling1.StartExceptionType = CE_HR_AttendanceExceptionType.正常;
                                tempScheduling1.StartExceptionBillNo = "";
                                tempScheduling1.StartRecode_IsExist = false;
                                tempScheduling1.StartRecord_Begin =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " 00:00:00");
                                tempScheduling1.StartRecode_End =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " 00:00:00");

                                tempScheduling1.EndExceptionType = CE_HR_AttendanceExceptionType.正常;
                                tempScheduling1.EndExceptionBillNo = "";
                                tempScheduling1.EndRcord_IsExist = true;
                                tempScheduling1.EndRcord_Begin =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchOutBeginTime);
                                tempScheduling1.EndRecod_End =
                                    Convert.ToDateTime(dateTemp.ToShortDateString() + " " + item.PunchOutEndTime);

                                listResult.Add(tempScheduling1);
                            }
                            else
                            {
                                throw new Exception("超出程序计算范围");
                            }
                        }

                        break;
                    case CE_HR_AttendanceScheme.不考勤:
                        break;
                    default:
                        break;
                }

                dateTemp = dateTemp.AddDays(1);
            }

            return listResult;
        }

        List<Entity_AttendanceRecordTime> CombineRecordTime(List<Entity_AttendanceRecordTime> listRecord)
        {
            listRecord = listRecord.OrderBy(k => k.RecordTime).ToList();
            List<Entity_AttendanceRecordTime> listResult = new List<Entity_AttendanceRecordTime>();

            for (int i = 0; i < listRecord.Count; i++)
            {
                Entity_AttendanceRecordTime tempTime = new Entity_AttendanceRecordTime();
                tempTime.RecordTime = listRecord[i].RecordTime;

                if (i == 0)
                {
                    listResult.Add(tempTime);
                }
                else
                {
                    for (int k = 0; k < listResult.Count; k++)
                    {
                        if (Math.Abs((listResult[k].RecordTime - tempTime.RecordTime).TotalMinutes) > 2)
                        {
                            listResult.Add(tempTime);
                        }
                    }
                }
            }

            return listResult;
        }
        #endregion

        void Analusis_WorkDayOverTime_ThrowException(string message, HR_OvertimeBill overTimeBillInfo, ref HR_AttendanceDaybookList tempInfo)
        {
            HR_TimeException exception = new HR_TimeException();
            exception.Date = _AnalysisDate;
            exception.ExceptionDescription = "加班异常:" + message;
            exception.ExceptionType = (int)CE_HR_AttendanceExceptionType.加班;
            exception.ObjectClockInTime = overTimeBillInfo.BeginTime.ToShortTimeString() + " " + ((DateTime)overTimeBillInfo.EndTime).ToShortTimeString();
            exception.RealClockInTime = "";
            exception.WorkID = _PersonnelInfo.员工编号;
            exception.Recorder = BasicInfo.LoginID;
            exception.RecordTime = ServerTime.Time;
            exception.RealExceptionType = (int)CE_HR_AttendanceExceptionType.加班;

            Guid guid = Guid.NewGuid();
            exception.DayBookUniqueID = guid;

            tempInfo.TimeExceptionRelevanceID = guid;

            _List_TimeException_Date.Add(exception);
        }

        void Analusis_WorkDayOverTime(CE_HR_WaterInfoType judgeInfoType, List<Entity_AttendanceRecordTime> listRecordTime)
        {
            bool isOperationFirstBill = false;
            int recordSection = 120;
            HR_AttendanceDaybookList tempInfo = new HR_AttendanceDaybookList();

            for (int i = 0; i < _List_Rules_OverTimeBill.Count; i++)
            {
                HR_OvertimeBill overTimeBillInfo = _List_Rules_OverTimeBill[i];

                tempInfo = new HR_AttendanceDaybookList();

                tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.加班).ToString();
                tempInfo.ResultSubclass = overTimeBillInfo.CompensateMode;
                tempInfo.BillNo = overTimeBillInfo.ID.ToString();
                tempInfo.Hours = ((DateTime)overTimeBillInfo.EndTime - overTimeBillInfo.BeginTime).TotalHours;
                tempInfo.Remark = "关联加班单";
                tempInfo.ObjectClockInTime = overTimeBillInfo.BeginTime.ToShortTimeString() + " " + ((DateTime)overTimeBillInfo.EndTime).ToShortTimeString();
                tempInfo.RealClockInTime = "";

                List<Entity_AttendanceRecordTime> listRecordTime_Temp_Start = listRecordTime.Where(k => k.RecordTime >= Convert.ToDateTime(overTimeBillInfo.BeginTime).AddMinutes(-recordSection)
                    && k.RecordTime <= Convert.ToDateTime(overTimeBillInfo.BeginTime).AddMinutes(recordSection)).ToList();
                List<Entity_AttendanceRecordTime> listRecordTime_Temp_End = listRecordTime.Where(k => k.RecordTime >= Convert.ToDateTime(overTimeBillInfo.EndTime).AddMinutes(-recordSection)
                    && k.RecordTime <= Convert.ToDateTime(overTimeBillInfo.EndTime).AddMinutes(recordSection)).ToList();

                //公司内
                if (overTimeBillInfo.OvertimeAddress != "公司外")
                {
                    //跨天加班
                    if (((DateTime)overTimeBillInfo.BeginTime).ToShortTimeString() == "0:00")
                    {
                        if (listRecordTime_Temp_End.Count == 0)
                        {
                            continue;
                            //Analusis_WorkDayOverTime_ThrowException("无加班结束打卡记录", overTimeBillInfo, ref tempInfo);
                        }
                        else
                        {
                            tempInfo.RealClockInTime = overTimeBillInfo.BeginTime.ToShortTimeString() + " " + listRecordTime_Temp_End[0].RecordTime.ToShortTimeString();
                        }
                    } //跨天加班
                    else if (((DateTime)overTimeBillInfo.EndTime).ToShortTimeString() == "23:59")
                    {
                        //if (listRecordTime_Temp_Start.Count == 0)
                        //{
                        //    continue;
                        //    //Analusis_WorkDayOverTime_ThrowException("无加班起始打卡记录", overTimeBillInfo, ref tempInfo);
                        //}
                        //else
                        //{
                        //    tempInfo.RealClockInTime = listRecordTime_Temp_Start[listRecordTime_Temp_Start.Count - 1].RecordTime.ToShortTimeString()
                        //        + " " + ((DateTime)overTimeBillInfo.EndTime).ToShortTimeString();
                        //}

                        //不需要打卡
                        tempInfo.RealClockInTime = ((DateTime)overTimeBillInfo.BeginTime).ToShortTimeString()
                            + " " + ((DateTime)overTimeBillInfo.EndTime).ToShortTimeString();
                    } //节假日加班
                    else if (judgeInfoType == CE_HR_WaterInfoType.节假日加班)
                    {
                        //if (listRecordTime_Temp_Start.Count == 0 || listRecordTime_Temp_End.Count == 0)
                        //{
                        //    continue;
                        //    //Analusis_WorkDayOverTime_ThrowException("无加班起始/加班结束打卡记录", overTimeBillInfo, ref tempInfo);
                        //}
                        //else
                        //{
                        //    tempInfo.RealClockInTime = listRecordTime_Temp_Start[listRecordTime_Temp_Start.Count - 1].RecordTime.ToShortTimeString()
                        //        + " " + listRecordTime_Temp_End[0].RecordTime.ToShortTimeString();
                        //}

                        if (listRecordTime.Count < 2)
                        {
                            continue;
                            //Analusis_WorkDayOverTime_ThrowException("无加班起始/加班结束打卡记录", overTimeBillInfo, ref tempInfo); 
                        }
                        else
                        {
                            tempInfo.RealClockInTime = 
                                listRecordTime[0].RecordTime.ToShortTimeString() + " " 
                                + listRecordTime[listRecordTime.Count - 1].RecordTime.ToShortTimeString();
                        }
                    }//上班日
                    else
                    {
                        //是否为下班后第一次加班单
                        //是
                        if (!isOperationFirstBill && Convert.ToDateTime(overTimeBillInfo.BeginTime.ToShortTimeString()) >= Convert.ToDateTime(_DateTime_Afternoon_End))
                        {
                            if (listRecordTime_Temp_End.Count == 0)
                            {
                                if (listRecordTime.Where(k => k.RecordTime >= Convert.ToDateTime(overTimeBillInfo.EndTime)).ToList().Count() > 0)
                                {
                                    tempInfo.RealClockInTime = overTimeBillInfo.BeginTime.ToShortTimeString() + " "
                                        + Convert.ToDateTime(overTimeBillInfo.EndTime).ToShortTimeString();
                                }
                                else
                                {
                                    continue;
                                }

                                //Analusis_WorkDayOverTime_ThrowException("无加班结束打卡记录", overTimeBillInfo, ref tempInfo);
                            }
                            else
                            {
                                tempInfo.RealClockInTime = overTimeBillInfo.BeginTime.ToShortTimeString() + " " + listRecordTime_Temp_End[0].RecordTime.ToShortTimeString();
                            }

                            isOperationFirstBill = true;
                        }//否
                        else
                        {
                            if (listRecordTime_Temp_Start.Count == 0 || listRecordTime_Temp_End.Count == 0)
                            {
                                continue;
                                //Analusis_WorkDayOverTime_ThrowException("无加班起始/加班结束打卡记录", overTimeBillInfo, ref tempInfo);
                            }
                            else
                            {
                                tempInfo.RealClockInTime = listRecordTime_Temp_Start[listRecordTime_Temp_Start.Count - 1].RecordTime.ToShortTimeString()
                                    + " " + listRecordTime_Temp_End[0].RecordTime.ToShortTimeString();
                            }
                        }
                    }
                }

                if (tempInfo.RealClockInTime == "")
                {
                    tempInfo.RealClockInTime = tempInfo.ObjectClockInTime;
                }
                else
                {
                    List<string> lstObject = tempInfo.ObjectClockInTime.Split(' ').ToList();
                    lstObject.RemoveAll(k => k.Trim().Length == 0);
                    List<string> lstReal = tempInfo.RealClockInTime.Split(' ').ToList();
                    lstReal.RemoveAll(k => k.Trim().Length == 0);

                    if (lstObject.Count == 2 && lstReal.Count == 2)
                    {
                        string startTime = Convert.ToDateTime(lstObject[0]) > Convert.ToDateTime(lstReal[0]) ? lstObject[0] : lstReal[0];
                        string endTime = Convert.ToDateTime(lstObject[1]) > Convert.ToDateTime(lstReal[1]) ? lstReal[1] : lstObject[1];

                        tempInfo.Hours = (Convert.ToDateTime(endTime) - Convert.ToDateTime(startTime)).TotalHours;
                        tempInfo.RealClockInTime = startTime + " " + endTime;
                    }
                    else
                    {
                        throw new Exception("程序异常：应打卡时间、实际打卡时间数量不等于2");
                    }
                }

                _List_Rules_AttendanceBook.Add(tempInfo);
            }
        }

        void Analusis_WaterInfo(CE_HR_WaterInfoType judgeInfoType, Entity_AttendanceDateTimeRules rules, DateTime? startTime, DateTime? endTime, 
            List<Entity_AttendanceRecordTime> listRecordTime)
        {
            HR_AttendanceDaybookList tempInfo = new HR_AttendanceDaybookList();
            Entity_AttendanceRecordTime recordTime = new Entity_AttendanceRecordTime();

            if (rules != null)
            {
                if (judgeInfoType == CE_HR_WaterInfoType.上班旷工 || judgeInfoType == CE_HR_WaterInfoType.上班正常)
                {
                    if(!rules.StartRecode_IsExist)
                    {
                        return;
                    }
                }

                if(judgeInfoType == CE_HR_WaterInfoType.下班旷工 || judgeInfoType == CE_HR_WaterInfoType.下班正常)
                {
                    if(!rules.EndRcord_IsExist)
                    {
                        return;
                    }
                }
            }

            tempInfo.Remark = judgeInfoType.ToString();

            //维持当前判定;
            switch (judgeInfoType)
            {
                case CE_HR_WaterInfoType.上班正常:

                    recordTime = listRecordTime[0];

                    tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.正常).ToString();
                    tempInfo.ResultSubclass = "";
                    tempInfo.BillNo = "";
                    tempInfo.Hours = 0;
                    tempInfo.ObjectClockInTime = rules.StartRecord_Begin.ToShortTimeString() + " " + rules.StartRecode_End.ToShortTimeString();
                    tempInfo.RealClockInTime = recordTime.RecordTime.ToLongDateString() + recordTime.RecordTime.ToShortTimeString();

                    if (_List_OperationExceptionType.Contains(rules.StartExceptionType))
                    {
                        string exceptionTypeRemark = string.Format("关联{0}单", rules.StartExceptionType.ToString());
                        var varDataList = from a in _List_Day_AttendanceBook
                                          where a.Remark == exceptionTypeRemark
                                          && a.BillNo == rules.StartExceptionBillNo
                                          select a;

                        if (varDataList.Count() > 0)
                        {
                            HR_AttendanceDaybookList tempInfo1 = varDataList.First();
                            tempInfo1.RealClockInTime =
                                tempInfo1.RealClockInTime.Trim().Length == 0 ? recordTime.RecordTime.ToShortTimeString()
                                : tempInfo1.RealClockInTime + " " + recordTime.RecordTime.ToShortTimeString();
                        }
                    }

                    break;
                case CE_HR_WaterInfoType.迟到:

                    recordTime = listRecordTime[0];

                    tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.迟到).ToString();
                    tempInfo.ResultSubclass = "";
                    tempInfo.BillNo = "";
                    tempInfo.Hours = (recordTime.RecordTime - (DateTime)startTime).TotalHours;
                    tempInfo.ObjectClockInTime = rules.StartRecord_Begin.ToShortTimeString() + " " + rules.StartRecode_End.ToShortTimeString();
                    tempInfo.RealClockInTime = recordTime.RecordTime.ToShortTimeString();

                    //if (rules.StartExceptionType == CE_HR_AttendanceExceptionType.请假)
                    //{
                    //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联请假单" && k.BillNo == rules.StartExceptionBillNo);
                    //}
                    //else if (rules.StartExceptionType == CE_HR_AttendanceExceptionType.出差)
                    //{
                    //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联出差单" && k.BillNo == rules.StartExceptionBillNo);
                    //}

                    if (tempInfo.Hours == 0)
                    {
                        return;
                    }

                    break;
                case CE_HR_WaterInfoType.上班旷工:
                case CE_HR_WaterInfoType.旷工:
                case CE_HR_WaterInfoType.下班旷工:

                    tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.旷工).ToString();
                    tempInfo.ResultSubclass = "";
                    tempInfo.BillNo = "";

                    if (judgeInfoType == CE_HR_WaterInfoType.旷工)
                    {
                        if ( rules.StartRecode_IsExist && 
                            _List_Rules_AttendanceBook.Where(k => k.Remark == CE_HR_WaterInfoType.上班旷工.ToString()).Count() > 0)
                        {
                            _List_Rules_AttendanceBook.RemoveAt(_List_Rules_AttendanceBook.Count - 1);
                            tempInfo.Hours = (listRecordTime[listRecordTime.Count - 1].RecordTime - (DateTime)startTime).TotalHours;
                        }
                        else
                        {
                            tempInfo.Hours = ((DateTime)endTime - listRecordTime[listRecordTime.Count - 1].RecordTime).TotalHours;
                        }

                        tempInfo.ObjectClockInTime = rules.StartTime.ToShortTimeString() + " " + rules.EndTime.ToShortTimeString();
                        tempInfo.RealClockInTime = listRecordTime[listRecordTime.Count - 1].RecordTime.ToShortTimeString();
                    }
                    else if (judgeInfoType == CE_HR_WaterInfoType.下班旷工)
                    {
                        tempInfo.ObjectClockInTime = rules.EndRcord_Begin.ToShortTimeString() + " " + rules.EndRecod_End.ToShortTimeString();
                        tempInfo.RealClockInTime = "";
                        //if (rules.EndExceptionType == CE_HR_AttendanceExceptionType.请假)
                        //{
                        //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联请假单" && k.BillNo == rules.EndExceptionBillNo);
                        //}
                        //else if (rules.EndExceptionType == CE_HR_AttendanceExceptionType.出差)
                        //{
                        //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联出差单" && k.BillNo == rules.EndExceptionBillNo);
                        //}

                        if (_List_Rules_AttendanceBook.Count > 0 &&
                            _List_Rules_AttendanceBook.Where(k => k.Remark == CE_HR_WaterInfoType.旷工.ToString()).Count() > 0)
                        {
                            return;
                        }

                        var varDataTemp = from a in _List_Rules_AttendanceBook
                                          where a.Remark == CE_HR_WaterInfoType.上班旷工.ToString()
                                          select a;

                        if (varDataTemp.Count() > 0)
                        {
                            foreach (var item in varDataTemp)
                            {
                                tempInfo.Remark = CE_HR_WaterInfoType.旷工.ToString();
                            }

                            return;
                        }
                        else
                        {
                            tempInfo.Hours = ((DateTime)endTime - (DateTime)startTime).TotalHours;
                        }
                    }
                    else if (judgeInfoType == CE_HR_WaterInfoType.上班旷工)
                    {
                        tempInfo.ObjectClockInTime = rules.StartRecord_Begin.ToShortTimeString() + " " + rules.StartRecode_End.ToShortTimeString();
                        tempInfo.RealClockInTime = "";
                        //if (rules.StartExceptionType == CE_HR_AttendanceExceptionType.请假)
                        //{
                        //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联请假单" && k.BillNo == rules.StartExceptionBillNo);
                        //}
                        //else if (rules.StartExceptionType == CE_HR_AttendanceExceptionType.出差)
                        //{
                        //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联出差单" && k.BillNo == rules.StartExceptionBillNo);
                        //}

                        tempInfo.Hours = ((DateTime)endTime - (DateTime)startTime).TotalHours;
                    }

                    if (tempInfo.Hours == 0)
                    {
                        return;
                    }

                    break;
                case CE_HR_WaterInfoType.早退:

                    recordTime = listRecordTime[0];

                    tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.早退).ToString();
                    tempInfo.ResultSubclass = "";
                    tempInfo.BillNo = "";
                    tempInfo.Hours = ((DateTime)endTime - recordTime.RecordTime).TotalHours;
                    tempInfo.ObjectClockInTime = rules.EndRcord_Begin.ToShortTimeString() + " " + rules.EndRecod_End.ToShortTimeString();
                    tempInfo.RealClockInTime = recordTime.RecordTime.ToShortTimeString();

                    //if (rules.EndExceptionType == CE_HR_AttendanceExceptionType.请假)
                    //{
                    //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联请假单" && k.BillNo == rules.EndExceptionBillNo);
                    //}
                    //else if (rules.EndExceptionType == CE_HR_AttendanceExceptionType.出差)
                    //{
                    //    _List_Day_AttendanceBook.RemoveAll(k => k.Remark == "关联出差单" && k.BillNo == rules.EndExceptionBillNo);
                    //}

                    if (tempInfo.Hours == 0)
                    {
                        return;
                    }

                    break;
                case CE_HR_WaterInfoType.下班正常:
                    recordTime = listRecordTime[0];

                    tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.正常).ToString();
                    tempInfo.ResultSubclass = "";
                    tempInfo.BillNo = "";
                    tempInfo.Hours = 0;
                    tempInfo.ObjectClockInTime = rules.EndRcord_Begin.ToShortTimeString() + " " + rules.EndRecod_End.ToShortTimeString();
                    tempInfo.RealClockInTime = recordTime.RecordTime.ToLongDateString() + recordTime.RecordTime.ToShortTimeString();

                    if (_List_OperationExceptionType.Contains(rules.EndExceptionType))
                    {
                        string exceptionTypeRemark = string.Format("关联{0}单", rules.EndExceptionType.ToString());
                        var varDataList = from a in _List_Day_AttendanceBook
                                          where a.Remark == exceptionTypeRemark
                                          && a.BillNo == rules.EndExceptionBillNo
                                          select a;

                        if (varDataList.Count() > 0)
                        {
                            HR_AttendanceDaybookList tempInfo1 = varDataList.First();
                            tempInfo1.RealClockInTime =
                                tempInfo1.RealClockInTime.Trim().Length == 0 ? recordTime.RecordTime.ToShortTimeString()
                                : tempInfo1.RealClockInTime + " " + recordTime.RecordTime.ToShortTimeString();
                        }
                    }

                    break;
                case CE_HR_WaterInfoType.上班日上班前加班:
                    if (listRecordTime.Count > 0)
                    {
                        _List_Rules_AttendanceBook.RemoveAll(k => k.Remark == CE_HR_WaterInfoType.上班旷工.ToString());
                        Analusis_WorkDayOverTime(judgeInfoType, listRecordTime);
                    }
                    break;
                case CE_HR_WaterInfoType.上班日下班后加班:
                    _List_Rules_AttendanceBook.RemoveAll(k => k.Remark == CE_HR_WaterInfoType.下班旷工.ToString());
                    Analusis_WorkDayOverTime(judgeInfoType, listRecordTime);
                    break;
                case CE_HR_WaterInfoType.上班日中间加班:
                    if (listRecordTime.Count > 1)
                    {
                        _List_Rules_AttendanceBook.RemoveAll(k => k.Remark == CE_HR_WaterInfoType.下班旷工.ToString());
                        _List_Rules_AttendanceBook.RemoveAll(k => k.Remark == CE_HR_WaterInfoType.上班旷工.ToString());
                        Analusis_WorkDayOverTime(judgeInfoType, listRecordTime);
                    }
                    break;
                case CE_HR_WaterInfoType.节假日加班:
                    Analusis_WorkDayOverTime(judgeInfoType, listRecordTime);
                    break;
                case CE_HR_WaterInfoType.考勤正常:

                    if (_List_GetInfo_Holiday.Where(k => k.BeginTime < _AnalysisDate.Date.AddDays(1) 
                        && k.EndTime >= _AnalysisDate.Date).Count() == 0)
                    {
                        return;
                    }
                    //else
                    //{
                    //    _List_Day_AttendanceBook.RemoveAll(k => k.ResultType == ((int)CE_HR_AttendanceExceptionType.出差).ToString());
                    //    _List_Day_AttendanceBook.RemoveAll(k => k.ResultType == ((int)CE_HR_AttendanceExceptionType.请假).ToString());
                    //}

                    tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.节假).ToString();
                    tempInfo.ResultSubclass = "";
                    tempInfo.BillNo = "";
                    tempInfo.Hours = 0;

                    List<HR_Holiday> listHoliday = _List_GetInfo_Holiday.Where(k => k.BeginTime < _AnalysisDate.Date.AddDays(1)
                        && k.EndTime > _AnalysisDate.Date).ToList();

                    if (listHoliday.Count() > 0)
                    {
                        tempInfo.Remark = GetHolidayType(listHoliday[0].HolidayTypeID).TypeName;
                    }
                    else
                    {
                        tempInfo.Remark = "公司放假";
                    }

                    tempInfo.ObjectClockInTime = "";
                    tempInfo.RealClockInTime = "";
                    break;
                default:
                    break;
            }

            if (tempInfo.ResultType != null && tempInfo.ResultType != "")
            {
                _List_Rules_AttendanceBook.Add(tempInfo);
            }
        }

        void Analysis_TimeBucket(DepotManagementDataContext ctx)
        {
            List<Entity_AttendanceRecordTime> listRecord_Bucket = new List<Entity_AttendanceRecordTime>();
            _List_Rules_Date = _List_Rules_Date.OrderBy(k => k.StartTime).ToList();

            //是否有考勤时间段
            if (_List_Rules_Date.Count == 0)//否
            {
                _List_Rules_AttendanceBook = new List<HR_AttendanceDaybookList>();

                if (_List_OverTimeBillInfo_Date.Count > 0)
                {
                    _List_Rules_OverTimeBill = _List_OverTimeBillInfo_Date;
                    Analusis_WaterInfo(CE_HR_WaterInfoType.节假日加班, null, null, null, _List_RecordTime_Date);
                    _List_Day_AttendanceBook.AddRange(_List_Rules_AttendanceBook);
                    return;
                }

                Analusis_WaterInfo(CE_HR_WaterInfoType.考勤正常, null, null, null, null);
                _List_Day_AttendanceBook.AddRange(_List_Rules_AttendanceBook);
                return;
            }

            //分析每一个考勤时间段
            for (int i = 0; i < _List_Rules_Date.Count; i++)
            {
                Entity_AttendanceDateTimeRules rulesBucket = _List_Rules_Date[i];

                _List_Rules_OverTimeBill = new List<HR_OvertimeBill>();
                _List_Rules_AttendanceBook = new List<HR_AttendanceDaybookList>();
                listRecord_Bucket = new List<Entity_AttendanceRecordTime>();

                listRecord_Bucket =
                    _List_RecordTime_Date.Where(k => k.RecordTime >= rulesBucket.StartRecord_Begin && k.RecordTime <= rulesBucket.EndRecod_End).ToList();

                //第一个时间段
                if (i == 0)
                {
                    listRecord_Bucket =
                        _List_RecordTime_Date.Where(k => k.RecordTime <= rulesBucket.EndRecod_End).ToList();
                }

                if (i == _List_Rules_Date.Count - 1)//最后一个时间段
                {
                    listRecord_Bucket =
                        _List_RecordTime_Date.Where(k => k.RecordTime >= rulesBucket.StartRecord_Begin).ToList();
                }

                if (_List_Rules_Date.Count == 1)
                {
                    listRecord_Bucket = _List_RecordTime_Date;
                }

                //合并打卡记录
                listRecord_Bucket = CombineRecordTime(listRecord_Bucket);

                //开始分析
                List<Entity_AttendanceRecordTime> listAnalysis = new List<Entity_AttendanceRecordTime>();

                //是否存在 打卡时间 <= 上班打卡结束时间的打卡记录?
                listAnalysis = listRecord_Bucket.Where(k => k.RecordTime <= rulesBucket.StartRecode_End).ToList();

                if (listAnalysis.Count() > 0)//是
                {
                    Analusis_WaterInfo(CE_HR_WaterInfoType.上班正常, rulesBucket, rulesBucket.StartRecord_Begin,
                        rulesBucket.StartRecode_End, listAnalysis);
                }
                else//否
                {
                    //是否存在上班打卡结束时间 < 打卡时间 <= 上班时间 + 30的打卡记录?
                    listAnalysis = listRecord_Bucket.Where(k => k.RecordTime > rulesBucket.StartRecode_End
                        && k.RecordTime <= rulesBucket.StartTime.AddMinutes(30)).ToList();

                    if (listAnalysis.Count() == 0)//否
                    {
                        Analusis_WaterInfo(CE_HR_WaterInfoType.上班旷工, rulesBucket, rulesBucket.StartTime, rulesBucket.EndTime, listAnalysis);
                    }
                    else//是
                    {
                        Analusis_WaterInfo(CE_HR_WaterInfoType.迟到, rulesBucket, rulesBucket.StartTime,
                            rulesBucket.StartTime.AddMinutes(30), listAnalysis);
                    }
                }

                //是否存在上班时间 +30 < 打卡时间 < 下班时间-30分钟的打卡记录?
                listAnalysis = listRecord_Bucket.Where(k => k.RecordTime > rulesBucket.StartTime.AddMinutes(30)
                    && k.RecordTime < rulesBucket.EndTime.AddMinutes(-30)).ToList();

                if (listAnalysis.Count() > 0)//是
                {
                    Analusis_WaterInfo(CE_HR_WaterInfoType.旷工, rulesBucket, rulesBucket.StartTime, rulesBucket.EndTime, listAnalysis);
                }

                //是否存在 下班打卡起始时间 <= 打卡时间 的打卡记录?
                listAnalysis = listRecord_Bucket.Where(k => k.RecordTime >= rulesBucket.EndRcord_Begin).ToList();

                if (listAnalysis.Count() > 0)//是
                {
                    Analusis_WaterInfo(CE_HR_WaterInfoType.下班正常, rulesBucket, rulesBucket.EndRcord_Begin,
                        rulesBucket.EndRecod_End, listAnalysis);
                }
                else//否
                {
                    //是否存在下班打卡起始时间  > 打卡时间 >= 下班时间 - 30的打卡记录?
                    listAnalysis = listRecord_Bucket.Where(k => k.RecordTime < rulesBucket.EndRcord_Begin
                        && k.RecordTime >= rulesBucket.EndTime.AddMinutes(-30)).ToList();

                    if (listAnalysis.Count() == 0)//否
                    {
                        Analusis_WaterInfo(CE_HR_WaterInfoType.下班旷工, rulesBucket, rulesBucket.StartTime, rulesBucket.EndTime, listAnalysis);
                    }
                    else//是
                    {
                        Analusis_WaterInfo(CE_HR_WaterInfoType.早退, rulesBucket, rulesBucket.EndTime.AddMinutes(-30),
                            rulesBucket.EndTime, listAnalysis);
                    }
                }

                //第一个时间段
                if (i == 0)
                {
                    _List_Rules_OverTimeBill = _List_OverTimeBillInfo_Date.Where(k => (DateTime)k.EndTime <= rulesBucket.StartTime).ToList();
                    if (_List_Rules_OverTimeBill.Count() > 0)//是
                    {
                        listAnalysis = listRecord_Bucket.Where(k => k.RecordTime < rulesBucket.StartTime).ToList();
                        Analusis_WaterInfo(CE_HR_WaterInfoType.上班日上班前加班, null, null, null, listAnalysis);
                    }
                }

                if (i == _List_Rules_Date.Count - 1)
                {
                    _List_Rules_OverTimeBill = _List_OverTimeBillInfo_Date.Where(k => (DateTime)k.BeginTime >= rulesBucket.EndTime).ToList();
                    if (_List_Rules_OverTimeBill.Count() > 0)//是
                    {
                        HR_AttendanceSetting setting = GetInfo_AttendanceSetting(ctx);

                        if (setting.SchemeCode == "0001")
                        {
                            listAnalysis = listRecord_Bucket.Where(k => k.RecordTime >= rulesBucket.EndTime.AddMinutes(120)).ToList();
                        }
                        else
                        {
                            listAnalysis = listRecord_Bucket.Where(k => k.RecordTime >= rulesBucket.EndTime.AddMinutes(60)).ToList();
                        }

                        Analusis_WaterInfo(CE_HR_WaterInfoType.上班日下班后加班, null, null, null, listAnalysis);
                    }
                }
                else
                {
                    _List_Rules_OverTimeBill = _List_OverTimeBillInfo_Date.Where(k => (DateTime)k.BeginTime >= rulesBucket.EndTime
                        && (DateTime)k.EndTime <= _List_Rules_Date[i + 1].StartTime).ToList();
                    if (_List_Rules_OverTimeBill.Count() > 0)//是
                    {
                        listAnalysis = listRecord_Bucket.Where(k => k.RecordTime >= rulesBucket.EndTime
                            && k.RecordTime <= _List_Rules_Date[i + 1].StartTime).ToList();
                        Analusis_WaterInfo(CE_HR_WaterInfoType.上班日中间加班, null, null, null, listAnalysis);
                    }
                }

                if (rulesBucket.StartRecode_IsExist && !rulesBucket.EndRcord_IsExist)
                {
                    if (_List_Rules_AttendanceBook.Where(k => k.Remark == CE_HR_WaterInfoType.上班正常.ToString()).Count() > 0)
                    {
                        _List_Rules_AttendanceBook.RemoveAll(k => k.ResultType == ((int)CE_HR_WaterInfoType.旷工).ToString());
                    }
                }

                if (!rulesBucket.StartRecode_IsExist && rulesBucket.EndRcord_IsExist)
                {
                    if (_List_Rules_AttendanceBook.Where(k => k.Remark == CE_HR_WaterInfoType.下班正常.ToString()).Count() > 0)
                    {
                        _List_Rules_AttendanceBook.RemoveAll(k => k.ResultType == ((int)CE_HR_WaterInfoType.旷工).ToString());
                    }
                }

                _List_Day_AttendanceBook.AddRange(_List_Rules_AttendanceBook);
			}
        }

        /// <summary>
        /// 分析主流程
        /// </summary>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <param name="workID">工号</param>
        public void Analysis_Main(DateTime startTime, DateTime endTime, string workID)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;
            ctx.Connection.Open();
            ctx.Transaction = ctx.Connection.BeginTransaction();

            try
            {
                Analysis_Main(ctx, startTime, endTime, workID);
                ctx.Transaction.Commit();
            }
            catch (Exception ex)
            {
                ctx.Transaction.Rollback();
                throw new Exception(ex.Message);
            }
 
        }

        /// <summary>
        /// 考勤分析主方法
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="workID">工号</param>
        public void Analysis_Main(DepotManagementDataContext ctx, DateTime startTime, DateTime endTime, string workID)
        {
            startTime = Convert.ToDateTime(startTime.ToShortDateString());
            endTime = Convert.ToDateTime(endTime.ToShortDateString());
            DateTime? tempDate = GetLastDateTime(ctx);

            DateTime recordTime = ServerTime.Time;

            if (tempDate == null)
            {
                return;
            }
            else
            {
                recordTime = Convert.ToDateTime(tempDate);
            }

            if (workID != null && workID.Trim().Length > 0)
            {
                if (startTime.Date > recordTime.Date)
                {
                    return;
                }
                else
                {
                    if (endTime.Date > recordTime.Date)
                    {
                        endTime = recordTime;
                    }
                }
            }

            try
            {
                //获取需分析人员的考勤设置信息
                List<View_HR_PersonnelArchive> listNeedAttendancePersonnelInfo = GetInfo_NeedAttendancePersonnel(ctx, startTime, workID);

                //需分析人员中是否存在未设置考勤方案的现象
                IsAttendanceSettingNull(ctx, listNeedAttendancePersonnelInfo, startTime, endTime);

                //获取节假日信息
                _List_GetInfo_Holiday = GetInfo_All(ctx, startTime, endTime, CE_HR_AttendanceExceptionType.节假) as List<HR_Holiday>;

                //获取集体考勤异常
                List<HR_BatchException> listBatchExceptionInfo_All = GetInfo_All(ctx, startTime, endTime, CE_HR_AttendanceExceptionType.集体异常) as List<HR_BatchException>;
                //获得请假单信息
                List<HR_LeaveBill> listLeaveBillInfo_All = GetInfo_All(ctx, startTime, endTime, CE_HR_AttendanceExceptionType.请假) as List<HR_LeaveBill>;
                //获得出差单信息
                List<HR_OnBusinessBill> listOnBusinessBillInfo_All = GetInfo_All(ctx, startTime, endTime, CE_HR_AttendanceExceptionType.出差) as List<HR_OnBusinessBill>;
                //获得加班单信息
                List<HR_OvertimeBill> listOverTimeBillInfo_All = GetInfo_All(ctx, startTime, endTime, CE_HR_AttendanceExceptionType.加班) as List<HR_OvertimeBill>;

                //排班考勤请假类别
                _ListHolidayType_Scheduling = new List<string>();

                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.婚假));
                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.晚婚假));
                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.丧假_3天));
                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.丧假_4天));
                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.产假));
                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.陪产假));
                _ListHolidayType_Scheduling.Add(GetInfo_LeaveTypeCode(ctx, CE_HR_LeaveType.陪产假_晚婚晚育));

                _List_OperationExceptionType = new List<CE_HR_AttendanceExceptionType>();

                _List_OperationExceptionType.Add(CE_HR_AttendanceExceptionType.集体异常);
                _List_OperationExceptionType.Add(CE_HR_AttendanceExceptionType.出差);
                _List_OperationExceptionType.Add(CE_HR_AttendanceExceptionType.请假);

                //分析每人循环
                foreach (View_HR_PersonnelArchive personnelItem in listNeedAttendancePersonnelInfo)
                {
                    _PersonnelInfo = personnelItem;

                    if (GetInfo_AttendanceSetting(ctx).SchemeCode == GetInfo_AttendanceSchemeCode(ctx, CE_HR_AttendanceScheme.不考勤))
                    {
                        continue;
                    }

                    List<HR_LeaveBill> listLeaveBillInfo_Personnel =
                        GetInfo_Personel<HR_LeaveBill>(ctx, listLeaveBillInfo_All,  CE_HR_AttendanceExceptionType.请假) as List<HR_LeaveBill>;
                    List<HR_OnBusinessBill> listOnBusinessBillInfo_Personnel =
                        GetInfo_Personel<HR_OnBusinessBill>(ctx, listOnBusinessBillInfo_All,  CE_HR_AttendanceExceptionType.出差) as List<HR_OnBusinessBill>;
                    List<HR_OvertimeBill> listOverTimeBillInfo_Personnel =
                        GetInfo_Personel<HR_OvertimeBill>(ctx, listOverTimeBillInfo_All, CE_HR_AttendanceExceptionType.加班) as List<HR_OvertimeBill>;
                    List<HR_BatchException> listBatchExceptionInfo_Personnel =
                        GetInfo_Personel<HR_BatchException>(ctx, listBatchExceptionInfo_All, CE_HR_AttendanceExceptionType.集体异常) as List<HR_BatchException>;

                    //分析每一天
                    _AnalysisDate = startTime;
                    while (_AnalysisDate <= endTime)
                    {
                        if ((_PersonnelInfo.入司时间 != null && _AnalysisDate.Date < ((DateTime)_PersonnelInfo.入司时间).Date)
                            || (_PersonnelInfo.离职时间 != null && _AnalysisDate.Date >= ((DateTime)_PersonnelInfo.离职时间).Date))
                        {
                            _AnalysisDate = _AnalysisDate.AddDays(1);
                            continue;
                        }

                        HR_AttendanceScheme normalScheme;

                        if (_AnalysisDate >= Convert.ToDateTime( _AnalysisDate.Year + "-05-01 00:00:00")
                            && _AnalysisDate < Convert.ToDateTime(_AnalysisDate.Year + "-10-01 00:00:00"))
                        {
                            normalScheme = (from a in ctx.HR_AttendanceScheme
                                            where a.SchemeName == CE_HR_AttendanceScheme.夏令时.ToString()
                                            select a).Single();
                        }
                        else
                        {
                            normalScheme = (from a in ctx.HR_AttendanceScheme
                                            where a.SchemeName == CE_HR_AttendanceScheme.冬令时.ToString()
                                            select a).Single();
                        }

                        _DateTime_Morning_Begin = normalScheme.BeginTimeInTheMorning.Value.ToShortTimeString();
                        _DateTime_Morning_End = normalScheme.EndTimeInTheMorning.Value.ToShortTimeString();
                        _DateTime_Afternoon_Begin = normalScheme.BeginTimeInTheAfternoon.Value.ToShortTimeString();
                        _DateTime_Afternoon_End = normalScheme.EndTimeInTheAfternoon.Value.ToShortTimeString();
                        _DateTime_PunchIn_Begin = normalScheme.PunchInBeginTime.Value.ToShortTimeString();
                        _DateTime_PunchIn_End = normalScheme.PunchInEndTime.Value.ToShortTimeString();
                        _DateTime_PunchOut_Begin = normalScheme.PunchOutBeginTime.Value.ToShortTimeString();
                        _DateTime_PunchOut_End = normalScheme.PunchOutEndTime.Value.ToShortTimeString();

                        _List_GetInfo_Rules = GetSureRules_Personnel(ctx, _AnalysisDate, _AnalysisDate);

                        _List_Day_AttendanceBook = new List<HR_AttendanceDaybookList>();
                        _List_TimeException_Date = new List<HR_TimeException>();

                        _List_RecordTime_Date = GetInfo_RecordTime(ctx);

                        _List_LeaveBillInfo_Date = GetInfo_Date<HR_LeaveBill>(ctx, listLeaveBillInfo_Personnel, 
                            CE_HR_AttendanceExceptionType.请假) as List<HR_LeaveBill>;
                        _List_OnBusinessBillInfo_Date = GetInfo_Date<HR_OnBusinessBill>(ctx, listOnBusinessBillInfo_Personnel,
                            CE_HR_AttendanceExceptionType.出差) as List<HR_OnBusinessBill>;
                        _List_OverTimeBillInfo_Date = GetInfo_Date<HR_OvertimeBill>(ctx, listOverTimeBillInfo_Personnel,
                            CE_HR_AttendanceExceptionType.加班) as List<HR_OvertimeBill>;
                        _List_BatchExceptionInfo_Date = GetInfo_Date<HR_BatchException>(ctx, listBatchExceptionInfo_Personnel,
                            CE_HR_AttendanceExceptionType.集体异常) as List<HR_BatchException>;

                        _List_Rules_Date = GetInfo_Date<Entity_AttendanceDateTimeRules>(ctx, _List_GetInfo_Rules,
                            CE_HR_AttendanceExceptionType.应考勤) as List<Entity_AttendanceDateTimeRules>;

                        //考勤时间段流水分析
                        Analysis_TimeBucket(ctx);

                        var varDataTemp = from a in ctx.HR_AttendanceDaybook
                                          where a.WorkID == _PersonnelInfo.员工编号
                                          && a.Date == _AnalysisDate.Date
                                          select a;

                        int dayBookID = 0;
                        HR_AttendanceDaybook dayBook = new HR_AttendanceDaybook();

                        if (varDataTemp.Count() == 1)
                        {
                            dayBook = varDataTemp.Single();

                            List<HR_AttendanceMachineDataList> listTemp = GetMachineDataList(ctx);
                            dayBook.PunchInTime = (listTemp == null || listTemp.Count() == 0) ? "" : listTemp[0].RecordTime;
                            dayBook.Recorder = BasicInfo.LoginID;
                            dayBook.RecordTime = ServerTime.Time;
                            dayBook.Remark = "";

                            var varList = from a in ctx.HR_AttendanceDaybookList
                                          where a.DayBookID == dayBook.ID
                                          select a;

                            ctx.HR_AttendanceDaybookList.DeleteAllOnSubmit(varList);
                            ctx.SubmitChanges();

                            dayBookID = dayBook.ID;
                        }
                        else if (varDataTemp.Count() == 0)
                        {
                            dayBook = new HR_AttendanceDaybook();
                            dayBook.Date = _AnalysisDate.Date;

                            List<HR_AttendanceMachineDataList> listTemp = GetMachineDataList(ctx);
                            dayBook.PunchInTime = (listTemp == null || listTemp.Count() == 0) ? "" : listTemp[0].RecordTime;

                            dayBook.Recorder = BasicInfo.LoginID;
                            dayBook.RecordTime = ServerTime.Time;
                            dayBook.Remark = "";
                            dayBook.WorkID = _PersonnelInfo.员工编号;

                            ctx.HR_AttendanceDaybook.InsertOnSubmit(dayBook);
                            ctx.SubmitChanges();

                            dayBookID = (from a in ctx.HR_AttendanceDaybook
                                         where a.WorkID == _PersonnelInfo.员工编号
                                         && a.Date == _AnalysisDate.Date
                                         select a.ID).Single();
                        }

                        var varTimeException = from a in ctx.HR_TimeException
                                               where a.Date.Date == _AnalysisDate.Date
                                               && a.WorkID == _PersonnelInfo.员工编号
                                               select a;

                        ctx.HR_TimeException.DeleteAllOnSubmit(varTimeException);

                        if (_List_Day_AttendanceBook.Count == 0)
                        {
                            HR_AttendanceDaybookList tempInfo = new HR_AttendanceDaybookList();

                            tempInfo.DayBookID = dayBookID;
                            tempInfo.ResultType = ((int)CE_HR_AttendanceExceptionType.正常).ToString();
                            tempInfo.ResultSubclass = "";
                            tempInfo.BillNo = "";
                            tempInfo.Hours = 0;
                            tempInfo.ObjectClockInTime = "";
                            tempInfo.RealClockInTime = "";
                            tempInfo.Remark = "正常";

                            _List_Day_AttendanceBook.Add(tempInfo);
                        }
                        else
                        {
                            foreach (HR_AttendanceDaybookList listItem in _List_Day_AttendanceBook)
                            {
                                listItem.DayBookID = dayBookID;

                                if (listItem.Hours < 0)
                                {
                                    listItem.Hours += 24;
                                }

                                if (listItem.Remark.Contains("加班单")
                                    || listItem.Remark.Contains("出差单")
                                    || listItem.Remark.Contains("请假单"))
                                {
                                    List<string> listObject = listItem.ObjectClockInTime.Split(' ').ToList();
                                    listObject.RemoveAll(k => k.Trim().Length == 0);
                                    List<string> listReal = listItem.RealClockInTime.Split(' ').ToList();
                                    listReal.RemoveAll(k => k.Trim().Length == 0);

                                    if (listReal.Count > 1)
                                    {
                                        listObject = listReal;
                                    }

                                    if (listObject.Count > 1)
                                        //&& GetInfo_AttendanceSetting(ctx).SchemeCode != GetInfo_AttendanceSchemeCode(ctx, CE_HR_AttendanceScheme.自然月排班考勤))
                                    {

                                        double minutes = 0;

                                        //剔除中午休息时间
                                        if (Convert.ToDateTime(_DateTime_Morning_End) < Convert.ToDateTime(listObject[listObject.Count - 1])
                                            && Convert.ToDateTime(listObject[0]) < Convert.ToDateTime(_DateTime_Afternoon_Begin))
                                        {
                                            if (Convert.ToDateTime(listObject[0]) <= Convert.ToDateTime(_DateTime_Morning_End) 
                                                && Convert.ToDateTime(listObject[listObject.Count - 1]) >= Convert.ToDateTime(_DateTime_Afternoon_Begin))
                                            {
                                                minutes += (Convert.ToDateTime(_DateTime_Afternoon_Begin) - Convert.ToDateTime(_DateTime_Morning_End)).TotalMinutes;
                                            }
                                            else if (Convert.ToDateTime(listObject[0]) <= Convert.ToDateTime(_DateTime_Morning_End)
                                                    && Convert.ToDateTime(listObject[listObject.Count - 1]) < Convert.ToDateTime(_DateTime_Afternoon_Begin))
                                            {
                                                minutes += (Convert.ToDateTime(listObject[listObject.Count - 1]) - Convert.ToDateTime(_DateTime_Morning_End)).TotalMinutes;
                                            }
                                            else if (Convert.ToDateTime(listObject[0]) > Convert.ToDateTime(_DateTime_Morning_End)
                                                    && Convert.ToDateTime(listObject[listObject.Count - 1]) >= Convert.ToDateTime(_DateTime_Afternoon_Begin))
                                            {
                                                minutes += (Convert.ToDateTime(_DateTime_Afternoon_Begin) - Convert.ToDateTime(listObject[0])).TotalMinutes;
                                            }
                                            else
                                            {
                                                minutes += 0;
                                            }
                                        }

                                        //剔除晚饭时间
                                        if (Convert.ToDateTime(_DateTime_Afternoon_End) < Convert.ToDateTime(listObject[listObject.Count - 1])
                                            && Convert.ToDateTime(listObject[0]) < Convert.ToDateTime(_DateTime_Afternoon_End).AddHours(0.5))
                                        {
                                            if (Convert.ToDateTime(listObject[0]) <= Convert.ToDateTime(_DateTime_Afternoon_End)
                                                && Convert.ToDateTime(listObject[listObject.Count - 1]) >= Convert.ToDateTime(_DateTime_Afternoon_End).AddHours(0.5))
                                            {
                                                minutes += (Convert.ToDateTime(_DateTime_Afternoon_End).AddHours(0.5) - Convert.ToDateTime(_DateTime_Afternoon_End)).TotalMinutes;
                                            }
                                            else if (Convert.ToDateTime(listObject[0]) <= Convert.ToDateTime(_DateTime_Afternoon_End)
                                                    && Convert.ToDateTime(listObject[listObject.Count - 1]) < Convert.ToDateTime(_DateTime_Afternoon_End).AddHours(0.5))
                                            {
                                                minutes += (Convert.ToDateTime(listObject[listObject.Count - 1]) - Convert.ToDateTime(_DateTime_Afternoon_End)).TotalMinutes;
                                            }
                                            else if (Convert.ToDateTime(listObject[0]) > Convert.ToDateTime(_DateTime_Afternoon_End)
                                                    && Convert.ToDateTime(listObject[listObject.Count - 1]) >= Convert.ToDateTime(_DateTime_Afternoon_End).AddHours(0.5))
                                            {
                                                minutes += (Convert.ToDateTime(_DateTime_Afternoon_End).AddHours(0.5) - Convert.ToDateTime(listObject[0])).TotalMinutes;
                                            }
                                            else
                                            {
                                                minutes += 0;
                                            }
                                        }

                                        listItem.Hours = (listItem.Hours * 60 - minutes) / 60;
                                    }

                                    double pointAfter = GlobalObject.GeneralFunction.GetDecimalPointAfter(listItem.Hours);
                                    int pointBefore = GlobalObject.GeneralFunction.GetDecimalPointBefore(listItem.Hours);

                                    if (pointAfter > 0.98)
                                    {
                                        listItem.Hours = (double)pointBefore + 1;
                                    }
                                    else if (pointAfter > 0.48 && pointAfter < 0.5)
                                    {
                                        listItem.Hours = (double)pointBefore + 0.5;
                                    }
                                    else
                                    {
                                        if (pointAfter >= 0.5)
                                        {
                                            listItem.Hours = (double)pointBefore + 0.5;
                                        }
                                        else
                                        {
                                            listItem.Hours = (double)pointBefore;
                                        }
                                    }
                                }

                                if (listItem.ResultType == ((int)CE_HR_AttendanceExceptionType.迟到).ToString()
                                    || listItem.ResultType == ((int)CE_HR_AttendanceExceptionType.早退).ToString()
                                    || listItem.ResultType == ((int)CE_HR_AttendanceExceptionType.旷工).ToString())
                                {

                                    HR_TimeException exception = new HR_TimeException();
                                    exception.Date = _AnalysisDate;
                                    exception.ExceptionDescription = GetExceptionTypeInfo(ctx, Convert.ToInt32(listItem.ResultType)).TypeName 
                                        + "时间: " + listItem.Hours + "小时 ";

                                    if (listItem.Remark == CE_HR_WaterInfoType.上班旷工.ToString())
                                    {
                                        exception.ExceptionDescription += "【上班无打卡时间】";
                                    }
                                    else if (listItem.Remark == CE_HR_WaterInfoType.下班旷工.ToString())
                                    {
                                        exception.ExceptionDescription += "【下班无打卡时间】";
                                    }

                                    exception.ExceptionType =  Convert.ToInt32(listItem.ResultType);
                                    exception.ObjectClockInTime = listItem.ObjectClockInTime;
                                    exception.RealClockInTime = listItem.RealClockInTime;
                                    exception.WorkID = _PersonnelInfo.员工编号;
                                    exception.Recorder = BasicInfo.LoginID;
                                    exception.RecordTime = ServerTime.Time;
                                    exception.RealExceptionType = Convert.ToInt32(listItem.ResultType);


                                    Guid guid = Guid.NewGuid();
                                    exception.DayBookUniqueID = guid;

                                    listItem.TimeExceptionRelevanceID = guid;

                                    _List_TimeException_Date.Add(exception);
                                }
                            }
                        }

                        ctx.HR_TimeException.InsertAllOnSubmit(_List_TimeException_Date);
                        ctx.HR_AttendanceDaybookList.InsertAllOnSubmit(_List_Day_AttendanceBook);
                        ctx.SubmitChanges();

                        //Console.Write(_PersonnelInfo.员工编号 + "   " + _AnalysisDate.ToShortDateString() + "\r\n");

                        _AnalysisDate = _AnalysisDate.AddDays(1);
                    }
                }

                ctx.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message + _PersonnelInfo.员工编号 + "   " + _AnalysisDate.ToShortDateString());
            }
        }

        public DataTable GetBusinessInfo_Exception(CE_HR_AttendanceExceptionType billType, CE_OperatorMode operationMode)
        {
            string strSql = "";

            if (operationMode == CE_OperatorMode.删除)
            {
                strSql = " select * from (select ID as 单据号, '请假' as 单据类型, b.TypeCode + ' '+ b.TypeName as 业务类型, a.BeginTime as 执行开始时间," +
                            " a.EndTime as 执行结束时间, a.RealHours as 小时数, a.Reason as 执行内容, a.Applicant as 申请人 , a.Date as 申请日期, a.Applicant as 执行人 ," +
                            " a.BillStatus as 业务状态 from HR_LeaveBill as a inner join HR_LeaveType as b on a.LeaveTypeID = b.TypeCode " +
                            " Union all select ID, '加班', CompensateMode + '_' + OvertimeAddress, a.BeginTime , a.EndTime, a.Hours , a.Errand, a.Applicant, a.Date, " +
                            " b.WorkID, a.BillStatus from HR_OvertimeBill as a inner join HR_OvertimePersonnel as b on a.ID = b.BillID "+
                            " Union all select a.ID, '出差', null, RealBeginTime, RealEndTime, null, a.Purpose, a.Applicant, a.ApplicantDate, b.WorkID, a.BillStatus " +
                            " from HR_OnBusinessBill as a inner join HR_OnBusinessPersonnel as b on a.ID = b.BillID " +
                            ") as a where 业务状态 = '已完成' ";
            }
            else if(operationMode == CE_OperatorMode.添加)
            {
                strSql = " select distinct * from (select ID as 单据号, '请假' as 单据类型, b.TypeCode + ' '+ b.TypeName as 业务类型, a.BeginTime as 执行开始时间," +
                            " a.EndTime as 执行结束时间, a.RealHours as 小时数, a.Reason as 执行内容, a.Applicant as 申请人 , a.Date as 申请日期," +
                            " a.BillStatus as 业务状态 from HR_LeaveBill as a inner join HR_LeaveType as b on a.LeaveTypeID = b.TypeCode " +
                            " Union all select distinct ID, '加班', CompensateMode + '_' + OvertimeAddress, a.BeginTime , a.EndTime, a.Hours , a.Errand, a.Applicant, a.Date, " +
                            " a.BillStatus from HR_OvertimeBill as a inner join HR_OvertimePersonnel as b on a.ID = b.BillID " +
                            " Union all select distinct a.ID, '出差', null, RealBeginTime, RealEndTime, null, a.Purpose, a.Applicant, a.ApplicantDate, a.BillStatus " +
                            " from HR_OnBusinessBill as a inner join HR_OnBusinessPersonnel as b on a.ID = b.BillID " +
                            ") as a where 1=1 ";
            }
            else if (operationMode == CE_OperatorMode.修改)
            {
                strSql = " select distinct * from (select ID as 单据号, '请假' as 单据类型, b.TypeCode + ' '+ b.TypeName as 业务类型, a.BeginTime as 执行开始时间," +
                            " a.EndTime as 执行结束时间, a.RealHours as 小时数, a.Reason as 执行内容, a.Applicant as 申请人 , a.Date as 申请日期," +
                            " a.BillStatus as 业务状态 from HR_LeaveBill as a inner join HR_LeaveType as b on a.LeaveTypeID = b.TypeCode " +
                            " Union all select distinct ID, '加班', CompensateMode + '_' + OvertimeAddress, a.BeginTime , a.EndTime, a.Hours , a.Errand, a.Applicant, a.Date, " +
                            " a.BillStatus from HR_OvertimeBill as a inner join HR_OvertimePersonnel as b on a.ID = b.BillID " +
                            " Union all select distinct a.ID, '出差', null, RealBeginTime, RealEndTime, null, a.Purpose, a.Applicant, a.ApplicantDate, a.BillStatus " +
                            " from HR_OnBusinessBill as a inner join HR_OnBusinessPersonnel as b on a.ID = b.BillID " +
                            " ) as a where 业务状态 = '已完成' ";
            }

            strSql += " and 执行结束时间 > (select  Cast(Year as varchar(50)) + '-' + Cast(Month as varchar(50)) + '-25' "+
                " from HR_AttendanceSummary where ID in (select Max(ID) - 20 from HR_AttendanceSummary)) "+
                " and 单据类型 = '" + billType.ToString() + "' order by 执行结束时间 desc";

            return GlobalObject.DatabaseServer.QueryInfo(strSql);
        }

        public void Operation_Exception(CE_HR_AttendanceExceptionType billType, CE_OperatorMode operationMode,
            List<object> lstInfo, List<PersonnelBasicInfo> lstPersonnel)
        {
            bool errorFlag = false;
            System.Data.Common.DbTransaction dbTransaction = null;

            try
            {
                using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                {
                    ctx.Connection.Open();
                    ctx.Transaction = ctx.Connection.BeginTransaction();
                    dbTransaction = ctx.Transaction;

                    ILeaveServer serviceLeave = ServerModuleFactory.GetServerModule<ILeaveServer>();

                    switch (billType)
                    {
                        case CE_HR_AttendanceExceptionType.请假:
                            HR_LeaveBill leaveBill = new HR_LeaveBill();

                            var varData = from a in ctx.HR_LeaveBill select a;
                            if (lstInfo[5] != null)
                            {
                                varData = from a in varData
                                          where a.ID == Convert.ToInt32(lstInfo[5])
                                          select a;

                                if (varData.Count() != 1)
                                {
                                    throw new Exception("获取请假单失败，修改记录不唯一");
                                }

                                leaveBill = varData.Single();
                            }

                            switch (operationMode)
                            {
                                case CE_OperatorMode.添加:

                                    foreach (PersonnelBasicInfo item in lstPersonnel)
                                    {
                                        leaveBill = new HR_LeaveBill();

                                        leaveBill.Applicant = item.工号;
                                        leaveBill.BeginTime = Convert.ToDateTime(lstInfo[0]);
                                        leaveBill.BillStatus = "已完成";
                                        leaveBill.Date = ServerTime.Time;
                                        leaveBill.Authorize = true;
                                        leaveBill.EndTime = Convert.ToDateTime(lstInfo[1]);
                                        leaveBill.LeaveTypeID = lstInfo[2].ToString().Split(' ')[0].ToString();
                                        leaveBill.OtherExplanation = "";
                                        leaveBill.RealHours = Convert.ToDouble(lstInfo[4]);
                                        leaveBill.Reason = lstInfo[3].ToString();

                                        DataTimeIsRepeat<HR_LeaveBill>(ctx, leaveBill, leaveBill.Applicant);

                                        ctx.HR_LeaveBill.InsertOnSubmit(leaveBill);
                                    }

                                    break;
                                case CE_OperatorMode.修改:
                                    if (lstInfo[5] != null)
                                    {
                                        leaveBill.BeginTime = Convert.ToDateTime(lstInfo[0]);
                                        leaveBill.EndTime = Convert.ToDateTime(lstInfo[1]);
                                        leaveBill.LeaveTypeID = lstInfo[2].ToString().Split(' ')[0].ToString();
                                        leaveBill.RealHours = Convert.ToDouble(lstInfo[4]);
                                        leaveBill.Reason = lstInfo[3].ToString();

                                        DataTimeIsRepeat<HR_LeaveBill>(ctx, leaveBill, leaveBill.Applicant);
                                    }
                                    break;
                                case CE_OperatorMode.删除:
                                    if (lstInfo[5] != null)
                                    {
                                        ctx.HR_LeaveBill.DeleteAllOnSubmit(varData);
                                    }
                                    break;
                                default:
                                    break;
                            }

                            break;
                        case CE_HR_AttendanceExceptionType.加班:
                            HR_OvertimeBill overTimeBill = new HR_OvertimeBill();

                            var varData1 = from a in ctx.HR_OvertimeBill select a;
                            if (lstInfo[5] != null)
                            {
                                varData1 = from a in varData1
                                           where a.ID == Convert.ToInt32(lstInfo[5])
                                           select a;

                                if (varData1.Count() != 1)
                                {
                                    throw new Exception("获取加班单失败，修改记录不唯一");
                                }

                                overTimeBill = varData1.Single();
                            }

                            switch (operationMode)
                            {
                                case CE_OperatorMode.添加:

                                    overTimeBill.Applicant = BasicInfo.LoginID;
                                    overTimeBill.Authorize = true;
                                    overTimeBill.BeginTime = Convert.ToDateTime(lstInfo[0]);
                                    overTimeBill.BillStatus = "已完成";
                                    overTimeBill.CompensateMode = lstInfo[2].ToString().Split('_')[0].ToString();
                                    overTimeBill.Date = ServerTime.Time;
                                    overTimeBill.EndTime = Convert.ToDateTime(lstInfo[1]);
                                    overTimeBill.Errand = lstInfo[3].ToString();
                                    overTimeBill.Hours = Convert.ToDecimal(lstInfo[4]);
                                    overTimeBill.NumberOfPersonnel = lstPersonnel.Count();
                                    overTimeBill.OvertimeAddress = lstInfo[2].ToString().Split('_')[1].ToString();
                                    overTimeBill.RealHours = (double)overTimeBill.Hours;
                                    overTimeBill.VerifyHours = overTimeBill.Hours;
                                    overTimeBill.VerifyFinish = true;

                                    ctx.HR_OvertimeBill.InsertOnSubmit(overTimeBill);
                                    ctx.SubmitChanges();

                                    int billID = (from a in ctx.HR_OvertimeBill select a.ID).Max();

                                    foreach (PersonnelBasicInfo item in lstPersonnel)
                                    {
                                        HR_OvertimePersonnel personnel = new HR_OvertimePersonnel();

                                        personnel.BillID = billID;
                                        personnel.WorkID = item.工号;

                                        DataTimeIsRepeat<HR_OvertimeBill>(ctx, overTimeBill, personnel.WorkID);
                                        ctx.HR_OvertimePersonnel.InsertOnSubmit(personnel);
                                    }

                                    break;
                                case CE_OperatorMode.修改:
                                    if (lstInfo[5] != null)
                                    {
                                        overTimeBill.BeginTime = Convert.ToDateTime(lstInfo[0]);
                                        overTimeBill.CompensateMode = lstInfo[2].ToString().Split('_')[0].ToString();
                                        overTimeBill.EndTime = Convert.ToDateTime(lstInfo[1]);
                                        overTimeBill.Errand = lstInfo[3].ToString();
                                        overTimeBill.Hours = Convert.ToDecimal(lstInfo[4]);
                                        overTimeBill.OvertimeAddress = lstInfo[2].ToString().Split('_')[1].ToString();
                                        overTimeBill.RealHours = (double)overTimeBill.Hours;
                                        overTimeBill.VerifyHours = overTimeBill.Hours;

                                        var personnel = from a in ctx.HR_OvertimePersonnel
                                                        where a.BillID == overTimeBill.ID
                                                        select a;

                                        foreach (HR_OvertimePersonnel pl in personnel)
                                        {
                                            DataTimeIsRepeat<HR_OvertimeBill>(ctx, overTimeBill, pl.WorkID);
                                        }
                                    }
                                    break;
                                case CE_OperatorMode.删除:
                                    if (lstInfo[5] != null)
                                    {
                                        var varPersonnel = from a in ctx.HR_OvertimePersonnel
                                                           where a.WorkID == lstPersonnel[0].工号
                                                           && a.BillID == overTimeBill.ID
                                                           select a;

                                        ctx.HR_OvertimePersonnel.DeleteAllOnSubmit(varPersonnel);
                                        ctx.SubmitChanges();


                                        varPersonnel = from a in ctx.HR_OvertimePersonnel
                                                       where a.BillID == overTimeBill.ID
                                                       select a;

                                        if (varPersonnel.Count() == 0)
                                        {
                                            ctx.HR_OvertimeBill.DeleteOnSubmit(overTimeBill);
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }

                            break;
                        default:
                            break;
                    }

                    ctx.SubmitChanges();
                    ctx.Transaction.Commit();
                }

            }
            catch (Exception ex)
            {
                #region 夏石友，2017-12-6，修正事务冲突异常
                errorFlag = true;
                dbTransaction.Rollback();
                #endregion 

                throw new Exception(ex.Message);
            }
            finally
            {
                #region 夏石友，2017-12-6，修正事务冲突异常
                if (!errorFlag)
                {
                    foreach (PersonnelBasicInfo pl in lstPersonnel)
                    {
                        Analysis_Main(Convert.ToDateTime(lstInfo[0]), Convert.ToDateTime(lstInfo[1]), pl.工号);
                    }
                }
                #endregion
            }
        }

        DateTime? GetLastDateTime(DepotManagementDataContext ctx)
        {
            var varData = from a in ctx.HR_AttendanceDaybook select a;

            if (varData.Count() == 0)
            {
                return null;
            }
            else
            {
                return (from a in ctx.HR_AttendanceDaybook select a.Date).Max();
            }
        }

        public void DataTimeIsRepeat<T>(DepotManagementDataContext ctx, T obj, string workID)
        {
            DateTime startTime, endTime;
            int? billID = null;
            CE_HR_AttendanceExceptionType type;
            List<object> lstBusinessType = new List<object>();

            switch (typeof(T).ToString().Substring(typeof(T).ToString().LastIndexOf('.') + 1))
            {
                case "HR_LeaveBill":
                    HR_LeaveBill tempLeave = obj as HR_LeaveBill;

                    type = CE_HR_AttendanceExceptionType.请假;
                    startTime = tempLeave.BeginTime;
                    endTime = tempLeave.EndTime;
                    billID = tempLeave.ID == 0 ? null : (int?)tempLeave.ID;
                    break;
                case "HR_OvertimeBill":
                    HR_OvertimeBill tempOverTime = obj as HR_OvertimeBill;

                    type = CE_HR_AttendanceExceptionType.加班;
                    startTime = tempOverTime.BeginTime;
                    endTime = Convert.ToDateTime(tempOverTime.EndTime);

                    lstBusinessType.Add(tempOverTime.CompensateMode);
                    lstBusinessType.Add(tempOverTime.OvertimeAddress);

                    billID = tempOverTime.ID == 0 ? null : (int?)tempOverTime.ID;
                    break;
                case "HR_OnBusinessBill":
                    HR_OnBusinessBill tempOnBuisness = obj as HR_OnBusinessBill;

                    type = CE_HR_AttendanceExceptionType.出差;
                    startTime = tempOnBuisness.RealBeginTime;
                    endTime = tempOnBuisness.RealEndTime;
                    billID = tempOnBuisness.ID == 0 ? null : (int?)tempOnBuisness.ID;
                    break;
                case "HR_BatchException":
                    HR_BatchException tempBatchException = obj as HR_BatchException;

                    type = CE_HR_AttendanceExceptionType.集体异常;
                    startTime = tempBatchException.BeginTime;
                    endTime = tempBatchException.EndTime;
                    billID = tempBatchException.ID == 0 ? null : (int?)tempBatchException.ID;
                    break;
                default:
                    throw new Exception("类型失效");
            }

            var varLeave = from a in ctx.HR_LeaveBill
                           where a.BeginTime < endTime && a.EndTime > startTime
                           && a.Applicant == workID
                           select a;

            if (type == CE_HR_AttendanceExceptionType.请假 && billID != null && billID != 0)
            {
                varLeave = from a in varLeave
                           where a.ID != billID
                           select a;
            }

            var varOverTime = from a in ctx.HR_OvertimeBill
                              join b in ctx.HR_OvertimePersonnel
                              on a.ID equals b.BillID
                              where a.BeginTime < endTime && a.EndTime > startTime
                              && b.WorkID == workID
                              select a;

            if (type == CE_HR_AttendanceExceptionType.加班 && billID != null && billID != 0)
            {
                varOverTime = from a in varOverTime
                              where a.ID != billID
                              select a;
            }

            var varOnBusiness = from a in ctx.HR_OnBusinessBill
                                join b in ctx.HR_OnBusinessPersonnel
                                on a.ID equals b.BillID
                                where a.RealBeginTime < endTime && a.RealEndTime > startTime
                                && b.WorkID == workID
                                select a;

            if (type == CE_HR_AttendanceExceptionType.出差 && billID != null && billID != 0)
            {
                varOnBusiness = from a in varOnBusiness
                                where a.ID != billID
                                select a;
            }

            var varException = from a in ctx.HR_BatchException
                                join b in ctx.HR_BatchException_Personnel
                                on a.ID equals b.BillID
                                where a.BeginTime < endTime && a.EndTime > startTime
                                && b.WorkID == workID
                                select a;

            if (type == CE_HR_AttendanceExceptionType.集体异常 && billID != null && billID != 0)
            {
                varException = from a in varException
                               where a.ID != billID
                               select a;
            }

            switch (type)
            {
                case CE_HR_AttendanceExceptionType.请假:

                    if (varOverTime.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.加班.ToString(), varOverTime.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    if (varLeave.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.请假.ToString(), varLeave.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    if (varOnBusiness.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.出差.ToString(), varOnBusiness.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    if (varException.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.集体异常.ToString(), varException.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }
                    break;
                case CE_HR_AttendanceExceptionType.加班:

                    varOverTime = from a in varOverTime
                                  where a.OvertimeAddress == lstBusinessType[1].ToString()
                                  select a;

                    if (varOverTime.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.加班.ToString(), varOverTime.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    if (varLeave.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.请假.ToString(), varLeave.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }
                    break;
                case CE_HR_AttendanceExceptionType.出差:

                    if (varLeave.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.请假.ToString(), varLeave.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    if (varOnBusiness.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.出差.ToString(), varOnBusiness.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    break;
                case CE_HR_AttendanceExceptionType.集体异常:

                    if (varLeave.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.请假.ToString(), varLeave.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }

                    if (varException.Count() > 0)
                    {
                        throw new Exception(string.Format("当前提交的单据时间与【{0}单】 单号：【{1}】的单据时间重复 【人员】{2}，请重新检查后再提交",
                            CE_HR_AttendanceExceptionType.集体异常.ToString(), varException.First().ID.ToString(), UniversalFunction.GetPersonnelInfo(ctx, workID).姓名));
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
