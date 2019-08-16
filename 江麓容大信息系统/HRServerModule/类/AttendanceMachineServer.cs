using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 考勤机导入的人员考勤明细表操作类
    /// </summary>
    class AttendanceMachineServer : Service_Peripheral_HR.IAttendanceMachineServer
    {
        /// <summary>
        /// 获取考勤机导入的人员考勤明细表
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetAllInfo()
        {
            string sql = "select * from dbo.View_HR_AttendanceMachineDataList order by 员工编号";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过员工编号和时间获取考勤机导入的人员考勤明细表
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="exDate">打卡日期</param>
        /// <returns>返回数据集</returns>
        public DataTable GetAttendanceMachineData(string workID,DateTime exDate)
        {
            string sql = "select 打卡时间 from dbo.View_HR_AttendanceMachineDataList where 员工编号='" + workID + "'" +
                         " and 打卡日期='" + exDate + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过打卡号获得员工编号
        /// </summary>
        /// <param name="card">打卡号</param>
        /// <returns>返回满足条件的员工编号</returns>
        public string GetCardIDWorkIDMapping(string card)
        {
            string sql = "select workID from HR_CardID_WorkID_Mapping where CardID='" + card + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["workID"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 通过时间和员工编号获得打卡时间
        /// </summary>
        /// <param name="date">日期</param>
        /// <param name="workID">员工编号</param>
        /// <returns>返回打卡时间</returns>
        public string GetAttendanceMachineDateListByBefor(DateTime date,string workID)
        {
            string sql = "select recordtime from dbo.HR_AttendanceMachineDataList where workID='" + workID + "' and date='" + date + "'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["recordtime"].ToString();
            }
            else
            {
                return "0";
            }
        }

        /// <summary>
        /// 导入数据
        /// </summary>
        /// <param name="lstDate">HR_AttendanceMachineDataList对象列表</param>
        /// <returns>成功返回true失败返回False</returns>
        public void InsertAttendanceMachine(List<HR_AttendanceMachineDataList> lstDate)
        {
            string error = null;

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                foreach (HR_AttendanceMachineDataList machineDataList in lstDate)
                {
                    error = machineDataList.WorkID;

                    var result = from a in dataContxt.HR_AttendanceMachineDataList
                                 where a.WorkID == machineDataList.WorkID && a.Date == machineDataList.Date
                                 select a;

                    dataContxt.HR_AttendanceMachineDataList.DeleteAllOnSubmit(result);

                    HR_AttendanceMachineDataList dataList = new HR_AttendanceMachineDataList();

                    dataList.WorkID = machineDataList.WorkID;
                    dataList.Date = machineDataList.Date;
                    dataList.RecordTime = machineDataList.RecordTime;
                    dataList.Remark = machineDataList.Remark;

                    dataContxt.HR_AttendanceMachineDataList.InsertOnSubmit(dataList);
                }

                dataContxt.SubmitChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 通过单据号删除考勤机导入的人员考勤明细
        /// </summary>
        /// <param name="billID">单号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteAttendanceMachineByID(string billID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_AttendanceMachineDataList
                             where a.ID == Convert.ToInt32(billID)
                             select a;

                if (result.Count() > 0)
                {
                    dataContxt.HR_AttendanceMachineDataList.DeleteAllOnSubmit(result);
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
        /// 通过员工编号和打卡日期删除考勤机导入的人员考勤明细
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="date">打卡日期</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True，失败返回False</returns>
        public bool DeleteAttendanceMachineByWorkIDAndDate(string workID,DateTime date, out string error)
        {
            error = "";

            try
            {
                //DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                //var result = from a in dataContxt.HR_AttendanceMachineDataList
                //             where a.WorkID == workID && a.Date == date
                //             select a;

                //if (result.Count() == 1)
                //{
                //    dataContxt.HR_AttendanceMachineDataList.DeleteAllOnSubmit(result);
                //}

                //dataContxt.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }

        /// <summary>
        /// 获取考勤机导入的人员考勤历史明细表
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetHistoryAllInfo()
        {
            string sql = "select * from dbo.View_HR_AttendanceMachineDataHistory order by 员工编号";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 获取人员是否有考勤信息
        /// </summary>
        /// <param name="workID">员工编号</param>
        /// <param name="date">日期</param>
        /// <returns>返回数据集</returns>
        public int GetWorkIDIsExist(string workID, DateTime date)
        {
            string sql = "select Count(*) count from dbo.[HR_AttendanceDaybook] join "+
                         " [HR_AttendanceDaybooklist] on [HR_AttendanceDaybooklist].daybookID=[HR_AttendanceDaybook].id"+
                         " where workID='"+workID+"' and Date='"+date+"'";
            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["count"].ToString());
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 获取考勤异常类型
        /// </summary>
        /// <returns>返回数据集</returns>
        public List<HR_AttendanceExceptionType> GetExceptionType()
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var varData = from a in ctx.HR_AttendanceExceptionType
                          select a;

            return varData.ToList();
        }

        /// <summary>
        /// 通过异常类型名获取对应的ID
        /// </summary>
        /// <param name="typeName">异常名</param>
        /// <returns>返回对应的ID</returns>
        public int GetExceptionTypeID(string typeName)
        {
            string sql = "select ID from HR_AttendanceExceptionType where typeName='"+typeName+"'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["ID"].ToString());
        }

        /// <summary>
        /// 通过异常类别ID获取异常类型名
        /// </summary>
        /// <param name="typeCode">ID</param>
        /// <returns>返回对应的异常类型名</returns>
        public string GetExceptionTypeName(int typeCode)
        {
            string sql = "select typeName from HR_AttendanceExceptionType where ID=" + typeCode;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0]["typeName"].ToString();
        }

        /// <summary>
        /// 判断是否需要考勤的人员都有记录
        /// </summary>  
        /// <param name="error">错误信息</param>
        /// <returns>没有返回false，有返回True</returns>
        public bool GetIsAllPersonnelPunchRecord(out string error)
        {
            error = "";

            string sql2 = "select View_HR_AttendanceSetting.员工编号 from dbo.View_HR_PersonnelArchive " +
                          " left join dbo.View_HR_AttendanceSetting" +
                          " on View_HR_AttendanceSetting.员工编号=View_HR_PersonnelArchive.员工编号 " +
                          " where View_HR_PersonnelArchive.人员状态='在职' and 考勤方案 not like '%不考勤%'"+
                          " and View_HR_AttendanceSetting.员工编号 not in (SELECT DISTINCT(WorkID) WorkID FROM HR_AttendanceMachineDataList)";
            DataTable dt2 = GlobalObject.DatabaseServer.QueryInfo(sql2);
            string workID = "";
            bool b = false;

            if (dt2 != null && dt2.Rows.Count > 0)
            {
                b = false;

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    workID += dt2.Rows[i]["员工编号"].ToString() + "、";
                }

                error = workID;
            }
            else
            {
                b = true;
            }

            return b;
        }
    }
}
