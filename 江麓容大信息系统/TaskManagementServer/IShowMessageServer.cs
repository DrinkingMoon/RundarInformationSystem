using System;
using System.Collections.Generic;
using GlobalObject;
using ServerModule;
using TaskServerModule;

namespace TaskManagementServer
{
    /// <summary>
    /// 短信操作服务接口
    /// </summary>
    public  interface IShowMessageServer
    {
        /// <summary>
        /// 添加短信
        /// </summary>
        /// <param name="lstShortMsg">短信列表</param>
        /// <param name="status">短信状态</param>
        void Add(List<TaskServerModule.View_Task_ShortMessage> lstShortMsg, ShortMessageStatus status);
                
        /// <summary>
        /// 删除短信
        /// </summary>
        /// <param name="lstShortMsgID">短信ID列表</param>
        void Delete(List<string> lstShortMsgID);

        /// <summary>
        /// 获取短信数据
        /// </summary>
        /// <param name="beginTime">发送短信的开始时间</param>
        /// <param name="endTime">发送短信的结束时间</param>
        List<TaskServerModule.View_Task_ShortMessage> GetData(DateTime beginTime, DateTime endTime);
        
        /// <summary>
        /// 获取所有的短信类别
        /// </summary>
        /// <returns>返回获取到的短信类别列表</returns>
        Task_ShortMessageType[] GetShortMsgType();

        /// <summary>
        /// 获取用户手机号码
        /// </summary>
        /// <param name="lstUser">用户基础信息列表</param>
        /// <returns>用户姓名与手机号码的字典</returns>
        Dictionary<string, string> GetMobileNo(List<PersonnelBasicInfo> lstUser);
 
        /// <summary>
        /// 更新短信列表中所有短信的状态
        /// </summary>
        /// <param name="lstShortMsgID">短信ID列表</param>
        /// <param name="status">更新后的状态</param>
        void Update(List<string> lstShortMsgID, ShortMessageStatus status);
                
        /// <summary>
        /// 更新指定短信的数据
        /// </summary>
        /// <param name="shortMsgID">短信ID</param>
        /// <param name="shortMsg">更新后的短信内容</param>
        void Update(string shortMsgID, View_Task_ShortMessage shortMsg);
    }
}
