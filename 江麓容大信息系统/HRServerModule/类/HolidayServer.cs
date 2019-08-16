using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ServerModule;
using GlobalObject;

namespace Service_Peripheral_HR
{
    /// <summary>
    /// 节假日管理类
    /// </summary>
    class HolidayServer : Service_Peripheral_HR.IHolidayServer
    {
        /// <summary>
        /// 获得所有节假日类型
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetHolidayType()
        {
            string sql = "select * from View_HR_HolidayType";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 通过节假日名称获得编号
        /// </summary>
        /// <param name="typeName">节假日名城</param>
        /// <returns>返回编号</returns>
        public int GetHolidayType(string typeName)
        {
            string sql = "select 编号 from View_HR_HolidayType where 节假日名称='" + typeName + "'";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return Convert.ToInt32(dt.Rows[0]["编号"].ToString());
        }

        /// <summary>
        /// 通过编号获得节假日名称
        /// </summary>
        /// <param name="typeID">编号</param>
        /// <returns>返回节假日名称</returns>
        public string GetHolidayType(int typeID)
        {
            string sql = "select 节假日名称 from View_HR_HolidayType where 编号=" + typeID;

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt.Rows[0]["节假日名称"].ToString();
        }

        /// <summary>
        /// 获得节假日信息
        /// </summary>
        /// <param name="date">节假日时间</param>
        /// <param name="flag">是否是法定节假日0：不是，1：是</param>
        /// <returns>返回节假日数据集</returns>
        public DataTable GetHoliday(DateTime date,string flag)
        {
            string dateTemp = date.ToShortDateString() + " " + "08:30";

            string sql = "select * from HR_Holiday left join dbo.HR_HolidayType on HR_HolidayType.ID=HR_Holiday .HolidayTypeID" +
                         " where begintime <= '" + dateTemp + "' and endtime>='" + dateTemp + "' "+
                         " and (ApplicableDeptCode like '%" + BasicInfo.DeptName + "%' or ApplicableDeptCode ='全部') ";

            if (flag != "0")
            {
                sql += " and IsLegalHolidays=" + flag;
            }

            sql += " order by begintime desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            if (dt != null && dt.Rows.Count == 0)
            {
                dateTemp = date.ToShortDateString() + " " + "13:00";

                sql = "select * from HR_Holiday left join dbo.HR_HolidayType on HR_HolidayType.ID=HR_Holiday .HolidayTypeID" +
                         " where begintime <= '" + dateTemp + "' and endtime>='" + dateTemp + "' " +
                         " and (ApplicableDeptCode like '%" + BasicInfo.DeptName + "%' or ApplicableDeptCode ='全部') ";

                dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            }

            return dt;
        }

        /// <summary>
        /// 获得节假日信息
        /// </summary>
        /// <returns>返回节假日数据集</returns>
        public DataTable GetHoliday()
        {
            string sql = "select * from View_HR_Holiday order by 开始时间 desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }

        /// <summary>
        /// 新增节假日类别
        /// </summary>
        /// <param name="holidayType">节假日类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool AddHolidayType(HR_HolidayType holidayType,out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_HolidayType
                             where a.TypeName == holidayType.TypeName
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.HR_HolidayType.InsertOnSubmit(holidayType);
                }
                else
                {
                    error = "节假日已经存在，请重新确认！";
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
        /// 修改节假日类别
        /// </summary>
        /// <param name="holidayType">节假日类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool UpdateHolidayType(HR_HolidayType holidayType, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_HolidayType
                             where a.ID == holidayType.ID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }
                else
                {
                    HR_HolidayType type = result.Single();

                    type.TypeName = holidayType.TypeName;
                    type.IsWeekend = holidayType.IsWeekend;
                    type.IsLegalHolidays = holidayType.IsLegalHolidays;
                    type.Remark = holidayType.Remark;
                    type.Recorder = holidayType.Recorder;
                    type.RecordTime = holidayType.RecordTime;
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
        /// 删除节假日类别
        /// </summary>
        /// <param name="typeID">节假日编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool DeleteHolidayType(int typeID, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var resultList = from c in dataContxt.HR_Holiday
                                 where c.HolidayTypeID == typeID
                                 select c;

                if (resultList.Count() > 0)
                {
                    error = "节假日信息中引用到了该节假日类别，需要先删除节假日信息，然后进行此操作！";
                    return false;
                }

                var result = from a in dataContxt.HR_HolidayType
                             where a.ID == typeID
                             select a;

                if (result.Count() != 1)
                {
                    error = "信息有误，请检查！";
                    return false;
                }
                else
                {
                    dataContxt.HR_HolidayType.DeleteAllOnSubmit(result);
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
        /// 新增节假日信息
        /// </summary>
        /// <param name="holiday">节假日数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool AddHoliday(HR_Holiday holiday, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_Holiday
                             where a.BeginTime < holiday.EndTime && holiday.BeginTime < a.EndTime
                             select a;

                if (result.Count() == 0)
                {
                    dataContxt.HR_Holiday.InsertOnSubmit(holiday);
                }
                else
                {
                    error = "节假日存在【休假日期】重复的现象，请重新确认！";
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
        /// 修改节假日信息
        /// </summary>
        /// <param name="holiday">节假日数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool UpdateHoliday(HR_Holiday holiday, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var temp1 = from a in dataContxt.HR_Holiday
                            where a.BeginTime < holiday.EndTime && holiday.BeginTime < a.EndTime
                            && a.ID != holiday.ID
                            select a;

                if (temp1.Count() > 0)
                {
                    error = "节假日存在【休假日期】重复的现象，请重新确认！";
                    return false;
                }

                var result = from a in dataContxt.HR_Holiday
                             where a.ID == holiday.ID
                             select a;

                if (result.Count() > 0)
                {
                    HR_Holiday holidayList = result.Single();

                    holidayList.HolidayTypeID = holiday.HolidayTypeID;
                    holidayList.ApplicableDeptCode = holiday.ApplicableDeptCode;
                    holidayList.ApplicableSex = holiday.ApplicableSex;
                    holidayList.Days = holiday.Days;
                    holidayList.BeginTime = holiday.BeginTime;
                    holidayList.EndTime = holiday.EndTime;
                    holidayList.Remark = holiday.Remark;
                    holidayList.Recorder = holiday.Recorder;
                    holidayList.RecordTime = holiday.RecordTime;
                }
                else
                {
                    error = "此记录不存在，请重新确认！";
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
        /// 删除节假日信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        public bool DeleteHoliday(int id, out string error)
        {
            error = "";

            try
            {
                DepotManagementDataContext dataContxt = CommentParameter.DepotDataContext;

                var result = from a in dataContxt.HR_Holiday
                             where a.ID == id
                             select a;

                if (result.Count() == 1)
                {
                    if (result.Single().BeginTime.Month < ServerTime.Time.Month)
                    {
                        error = "节假日的起止时间在本月之前，不能删除！";
                        return false;
                    }

                    dataContxt.HR_Holiday.DeleteAllOnSubmit(result);
                }
                else
                {
                    error = "节假日已经存在，请重新确认！";
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
        /// 获得节假日放假天数
        /// </summary>
        /// <param name="starTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回节假日数据集</returns>
        public DataTable GetHolidayDays(DateTime starTime, DateTime endTime)
        {

            string sql = "select sum(Days) as days from HR_Holiday " +
                        " where (ApplicableDeptCode like '%" + BasicInfo.DeptName + "%' or ApplicableDeptCode ='全部')" +
                        " and ((BeginTime >= '" + starTime + "' and EndTime <= '" + endTime + "') " +
                        " or BeginTime<='" + starTime + "'and  (EndTime>='" + starTime + "') " +
                        " or (EndTime>='" + starTime + "' and EndTime<='" + starTime.ToShortDateString() + " 17:30:00'))";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);

            return dt;
        }
    }
}
