using System;
using ServerModule;
using TaskServerModule;

namespace TaskManagementServer
{
    /// <summary>
    /// 提供资源业务操作的服务接口
    /// </summary>
    public interface IResourceServer
    {
        /// <summary>
        /// 获取会议资源
        /// </summary>
        /// <param name="beginTime">占用资源的开始时间</param>
        /// <param name="endTime">占用资源的结束时间</param>
        System.Collections.Generic.List<View_PRJ_Resource> GetMeetingResource(DateTime beginTime, DateTime endTime);
                
        /// <summary>
        /// 判断资源是否空闲
        /// </summary>
        /// <param name="resourceID">资源编号</param>
        /// <param name="beginTime">占用资源的开始时间</param>
        /// <param name="endTime">占用资源的结束时间</param>
        /// <returns>空闲返回true</returns>
        bool IsIdle(int resourceID, DateTime beginTime, DateTime endTime);
    }
}
