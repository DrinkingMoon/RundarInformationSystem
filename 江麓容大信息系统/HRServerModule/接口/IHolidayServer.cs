using System;
using ServerModule;
using System.Data;
namespace Service_Peripheral_HR
{
    /// <summary>
    /// 节假日管理类
    /// </summary>
    public interface IHolidayServer
    {
        /// <summary>
        /// 获得所有节假日类型
        /// </summary>
        /// <returns>返回数据集</returns>
        string GetHolidayType(int typeID);

        /// <summary>
        /// 通过节假日名称获得编号
        /// </summary>
        /// <param name="typeName">节假日名城</param>
        /// <returns>返回编号</returns>
        int GetHolidayType(string typeName);

        /// <summary>
        /// 获得节假日名称
        /// </summary>
        /// <returns>返回节假日名称</returns>
        System.Data.DataTable GetHolidayType();

         /// <summary>
        /// 新增节假日类别
        /// </summary>
        /// <param name="holidayType">节假日类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool AddHolidayType(HR_HolidayType holidayType, out string error);

        /// <summary>
        /// 修改节假日类别
        /// </summary>
        /// <param name="holidayType">节假日类别数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool UpdateHolidayType(HR_HolidayType holidayType, out string error);

        /// <summary>
        /// 删除节假日类别
        /// </summary>
        /// <param name="typeID">节假日编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool DeleteHolidayType(int typeID, out string error);

        /// <summary>
        /// 获得节假日信息
        /// </summary>
        /// <param name="date">节假日时间</param>
        /// <param name="flag">是否是法定节假日0：不是，1：是</param>
        /// <returns>返回节假日数据集</returns>
        DataTable GetHoliday(DateTime date, string flag);

         /// <summary>
        /// 获得节假日信息
        /// </summary>
        /// <returns>返回节假日数据集</returns>
        DataTable GetHoliday();

        /// <summary>
        /// 新增节假日信息
        /// </summary>
        /// <param name="holiday">节假日数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool AddHoliday(HR_Holiday holiday, out string error);

        /// <summary>
        /// 修改节假日信息
        /// </summary>
        /// <param name="holiday">节假日数据集</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool UpdateHoliday(HR_Holiday holiday, out string error);

        /// <summary>
        /// 删除节假日信息
        /// </summary>
        /// <param name="id">编号</param>
        /// <param name="error">错误信息</param>
        /// <returns>成功返回True 失败返回False</returns>
        bool DeleteHoliday(int id, out string error);

        /// <summary>
        /// 获得节假日放假天数
        /// </summary>
        /// <param name="starTime">开始时间</param>
        /// <param name="endTime">截止时间</param>
        /// <returns>返回节假日数据集</returns>
        DataTable GetHolidayDays(DateTime starTime, DateTime endTime);
    }
}
