using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using GlobalObject;
using System.Data.Linq;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤方案操作类
    /// </summary>
    class AttendanceSchemeServer : Service_Peripheral_HR.IAttendanceSchemeServer
    {
        #region 考勤方案
        /// <summary>
        /// 获取所有的考勤方案
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAllAttendanceScheme()
        {
            string sql = "select * from View_HR_AttendanceScheme";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取所有的考勤方案(得到第一条数据)
        /// </summary>
        /// <returns>返回数据集</returns>
        public IQueryable<HR_AttendanceScheme> GetLinqResult()
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return from a in dataContxt.HR_AttendanceScheme.Take(1)
                   select a;
        }

        /// <summary>
        /// 通过编号获取考勤方案信息
        /// </summary>
        /// <returns>返回数据集</returns>
        public HR_AttendanceScheme GetAttendanceSchemeByCode(string schemeCode)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            return (from a in dataContxt.HR_AttendanceScheme
                    where a.SchemeCode == schemeCode
                    select a).Single();
        }
        
        /// <summary>
        /// 获取考勤方案(编码+名称)
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAttendanceScheme()
        {
            string sql = "select 考勤方案编码+' '+考勤方案名称 as 考勤方案 from View_HR_AttendanceScheme";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 新增修改考勤方案
        /// </summary>
        /// <param name="attendance">考勤方案数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool AddAttendanceScheme(HR_AttendanceScheme attendance,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_AttendanceScheme
                             where a.SchemeCode == attendance.SchemeCode
                             select a;

                if (result.Count() > 0)
                {
                    HR_AttendanceScheme attendanceList = result.Single();

                    attendanceList.SchemeName = attendance.SchemeName;
                    attendanceList.Remark = attendance.Remark;
                    attendanceList.RecordTime = attendance.RecordTime;
                    attendanceList.Recorder = attendance.Recorder;
                    attendanceList.PunchOutEndTime = attendance.PunchOutEndTime;
                    attendanceList.PunchOutBeginTime = attendance.PunchOutBeginTime;
                    attendanceList.PunchInEndTime = attendance.PunchInEndTime;
                    attendanceList.PunchInBeginTime = attendance.PunchInBeginTime;
                    attendanceList.EndTimeInTheMorning = attendance.EndTimeInTheMorning;
                    attendanceList.EndTimeInTheAfternoon = attendance.EndTimeInTheAfternoon;
                    attendanceList.EndDateOfThisMonth = attendance.EndDateOfThisMonth;
                    attendanceList.BeginTimeInTheMorning = attendance.BeginTimeInTheMorning;
                    attendanceList.BeginTimeInTheAfternoon = attendance.BeginTimeInTheAfternoon;
                    attendanceList.BeginDateOfLastMonth = attendance.BeginDateOfLastMonth;
                    attendanceList.AutoOvertimeInPublicHoliday = attendance.AutoOvertimeInPublicHoliday;
                    attendanceList.AttendanceMode = attendance.AttendanceMode;
                }
                else
                {
                    dataContxt.HR_AttendanceScheme.InsertOnSubmit(attendance);
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
        /// 通过考勤编码删除考勤方案
        /// </summary>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True失败返回False</returns>
        public bool DeleteAttendanceScheme(string schemeCode,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultUse = from a in dataContxt.HR_AttendanceSetting
                                where a.SchemeCode == schemeCode
                                select a;

                if (resultUse.Count() > 0)
                {
                    error = "已有人员适用于该考勤，不能删除";
                    return false;
                }

                var result = from c in dataContxt.HR_AttendanceScheme
                             where c.SchemeCode == schemeCode
                             select c;

                if (result.Count() > 0)
                {
                    dataContxt.HR_AttendanceScheme.DeleteAllOnSubmit(result);
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
        #endregion

        #region 人员考勤设置
        /// <summary>
        /// 获取员工考勤方案
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAttendanceSetting()
        {
            string sql = "select * from View_HR_AttendanceSetting where 人员状态='在职' order by 部门";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 判断是否所有人员都设置了考勤方案
        /// </summary>
        /// <returns>全部设置返回true，否则返回false</returns>
        public bool GetIsSetting()
        {
            string sql = "select count(*) count from View_HR_AttendanceSetting where 人员状态='在职' and 考勤方案 is null";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                if (Convert.ToInt32(dt.Rows[0]["count"].ToString()) > 0)
                    return false;
                else
                    return true;
            }

            return true;
        }

        /// <summary>
        /// 通过员工编号获取员工的考勤方案
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <returns>返回结果集</returns>
        public HR_AttendanceSetting GetAttendanceSettingByWorkID(string workID)
        {
            DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

            var varData = from a in dataContxt.HR_AttendanceSetting
                          where a.WorkID == workID
                          select a;

            if (varData.Count() == 1)
            {
                return varData.Single();
            }
            else if (varData.Count() > 1)
            {
                throw new Exception("工号【"+ workID +"】考勤方案不唯一");
            }
            else
            {
                throw new Exception("工号【"+ workID +"】无考勤方案");
            }
        }

        /// <summary>
        /// 批量设置考勤方案
        /// </summary>
        /// <param name="dept">部门编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="flag">是否替换（true替换，false不替换）</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddAttendanceSettingByDept(string dept, string schemeCode, bool flag, bool isSubsidize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (dept == "全部")
                {
                    IQueryable<View_SelectPersonnel> personnel = new PersonnelArchiveServer().GetAllInfo();

                    foreach (var item in personnel)
                    {
                        var result = from a in dataContxt.HR_AttendanceSetting
                                     where a.WorkID == item.员工编号
                                     select a;

                        if (!flag)
                        {
                            if (result.Count() == 1)
                            {
                                continue;
                            }
                        }

                        HR_AttendanceSetting attendance = new HR_AttendanceSetting();

                        attendance.SchemeCode = schemeCode;
                        attendance.IsSubsidize = isSubsidize;
                        attendance.WorkID = item.员工编号;
                        attendance.Recorder = BasicInfo.LoginID;
                        attendance.RecordTime = ServerTime.Time;

                        dataContxt.HR_AttendanceSetting.InsertOnSubmit(attendance);
                    }
                }
                else
                {
                    string[] deptList = dept.Split(';');

                    for (int i = 0; i < deptList.Count() - 1; i++)
                    {
                        IQueryable<View_SelectPersonnel> personnel = new PersonnelArchiveServer().GetAllInfo();

                        foreach (var item in personnel)
                        {
                            if (item.部门编号.Substring(0, 2) == new OrganizationServer().GetDeptCode(deptList[i]))
                            {
                                var result = from a in dataContxt.HR_AttendanceSetting
                                             where a.WorkID == item.员工编号
                                             select a;

                                if (!flag)
                                {
                                    if (result.Count() == 1)
                                    {
                                        continue;
                                    }
                                }

                                HR_AttendanceSetting attendance = new HR_AttendanceSetting();

                                attendance.SchemeCode = schemeCode;
                                attendance.WorkID = item.员工编号;
                                attendance.Recorder = BasicInfo.LoginID;
                                attendance.RecordTime = ServerTime.Time;

                                dataContxt.HR_AttendanceSetting.InsertOnSubmit(attendance);
                            }
                        }
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
        /// 单个员工设置考勤
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool AddAttendanceSetting(string workID, string schemeCode,bool isSubsidize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_AttendanceSetting
                             where a.SchemeCode == schemeCode && a.WorkID == workID
                             select a;

                if (result.Count() > 0)
                {
                    error = "该员工已经设置了该考勤！";
                    return false;
                }

                HR_AttendanceSetting attendance = new HR_AttendanceSetting();

                attendance.SchemeCode = schemeCode;
                attendance.WorkID = workID;
                attendance.IsSubsidize = isSubsidize;
                attendance.Recorder = BasicInfo.LoginID;
                attendance.RecordTime = ServerTime.Time;

                dataContxt.HR_AttendanceSetting.InsertOnSubmit(attendance);
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
        /// 单个员工修改考勤
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateAttendanceSetting(string workID, string schemeCode, bool isSubsidize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_AttendanceSetting
                             where a.WorkID == workID
                             select a;

                if (result.Count() == 0)
                {
                    error = "该员工未设置考勤方案，请先为该员工添加考勤方案！";
                    return false;
                }

                HR_AttendanceSetting attendance = result.Single();

                attendance.SchemeCode = schemeCode; 
                attendance.IsSubsidize = isSubsidize;
                attendance.Recorder = BasicInfo.LoginID;
                attendance.RecordTime = ServerTime.Time;

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
        /// 批量修改考勤方案
        /// </summary>
        /// <param name="dept">部门编号</param>
        /// <param name="schemeCode">考勤编码</param>
        /// <param name="isSubsidize">是否有餐补</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool UpdateAttendanceSettingByDept(string dept, string schemeCode, bool isSubsidize, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                if (dept == "全部")
                {
                    var result = from a in dataContxt.HR_AttendanceSetting
                                 select a;

                    if (result.Count() > 0)
                    {
                        dataContxt.HR_AttendanceSetting.DeleteAllOnSubmit(result);
                        dataContxt.SubmitChanges();
                    }

                    IQueryable<View_SelectPersonnel> personnel = new PersonnelArchiveServer().GetAllInfo();

                    foreach (var item in personnel)
                    {
                        HR_AttendanceSetting attendance = new HR_AttendanceSetting();

                        attendance.SchemeCode = schemeCode;
                        attendance.WorkID = item.员工编号; 
                        attendance.IsSubsidize = isSubsidize;
                        attendance.Recorder = BasicInfo.LoginID;
                        attendance.RecordTime = ServerTime.Time;

                        dataContxt.HR_AttendanceSetting.InsertOnSubmit(attendance);
                    }
                }
                else
                {
                    string[] deptList = dept.Split(';');

                    for (int i = 0; i < deptList.Count() - 1; i++)
                    {

                        IQueryable<View_SelectPersonnel> personnel = new PersonnelArchiveServer().GetAllInfo();

                        foreach (var item in personnel)
                        {
                            if (item.部门编号 == new OrganizationServer().GetDeptCode(deptList[i]))
                            {
                                var result = from a in dataContxt.HR_AttendanceSetting
                                             where a.WorkID == item.员工编号
                                             select a;

                                if (result.Count() == 1)
                                {
                                    HR_AttendanceSetting attendance = result.Single();

                                    attendance.SchemeCode = schemeCode;
                                    attendance.Recorder = BasicInfo.LoginID;
                                    attendance.RecordTime = ServerTime.Time;
                                }
                            }
                        }
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
        /// 通过员工编号删除
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true,失败返回False</returns>
        public bool DeleteAttendanceSetting(List<string> workID, out string error)
        {
            try
            {
                error = null;

                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                Table<HR_AttendanceSetting> table = dataContxt.GetTable<HR_AttendanceSetting>();

                for (int i = 0; i < workID.Count; i++)
                {
                    var delRow = from c in table
                                 where c.WorkID == workID[i].ToString()
                                 select c;

                    foreach (var item in delRow)
                    {
                        table.DeleteOnSubmit(item);
                    }
                }

                dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception exce)
            {
                error = exce.Message;
                return false;
            }
        }
        #endregion
    }
}
