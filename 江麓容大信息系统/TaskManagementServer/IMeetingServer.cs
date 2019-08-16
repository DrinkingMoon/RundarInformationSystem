using System;

namespace TaskManagementServer
{
    /// <summary>
    /// 为日常会议提供业务操作的服务接口
    /// </summary>
    public interface IMeetingServer
    {
        /// <summary>
        /// 检查会议参与人员是否有时间参加会议
        /// </summary>
        /// <param name="personnels">与会人员</param>
        /// <param name="beginTime">会议开始时间</param>
        /// <param name="endTime">会议结束时间</param>
        /// <param name="error">错误时输出错误信息</param>
        /// <returns>检查是否通过的标志</returns>
        bool CheckParticipants(System.Collections.Generic.List<GlobalObject.PersonnelBasicInfo> personnels, 
            DateTime beginTime, DateTime endTime, out string error);

        /// <summary>
        /// 删除指定的会议信息
        /// </summary>
        /// <param name="meetingID">会议编号</param>
        void Delete(string meetingID);

        /// <summary>
        /// 从表格行数据中提取会议对象
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>返回获取到的会议对象</returns>
        MeetingData GetMeetingData(System.Data.DataRow row);

        /// <summary>
        /// 获取指定日期范围内的会议数据
        /// </summary>
        /// <param name="beginTime">会议开始时间，只取日期部分</param>
        /// <param name="endTime">会议结束时间，只取日期部分</param>
        /// <returns>返回获取到的数据</returns>
        System.Data.DataTable GetMeetingData(DateTime beginTime, DateTime endTime);

        /// <summary>
        /// 保存会议信息
        /// </summary>
        /// <param name="info">要保存的数据</param>
        void Save(MeetingData info);

        /// <summary>
        /// 更新会议状态
        /// </summary>
        /// <param name="meetingID">会议编号</param>
        /// <param name="status">会议状态</param>
        void UpdateStatus(string meetingID, MeetingStatus status);
        
        /// <summary>
        /// 设置会议实际完成时间
        /// </summary>
        /// <param name="meetingId">会议编号</param>
        /// <param name="realTime">实际完成时间</param>
        void SetRealFinishedTime(string meetingId, DateTime realTime);
    }
}
