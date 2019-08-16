using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ServerModule;
using TaskServerModule;

namespace TaskManagementServer
{
    /// <summary>
    /// 提供资源业务操作的服务
    /// </summary>
    class ResourceServer : TaskManagementServer.IResourceServer
    {
        /// <summary>
        /// 获取会议资源
        /// </summary>
        /// <param name="beginTime">占用资源的开始时间</param>
        /// <param name="endTime">占用资源的结束时间</param>
        public List<View_PRJ_Resource> GetMeetingResource(DateTime beginTime, DateTime endTime)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            var varData = from r in ctx.Fun_GetResourceView_1(beginTime, endTime)
                          select r;

            List<View_PRJ_Resource> result = new List<View_PRJ_Resource>();

            foreach (var item in varData)
            {
                View_PRJ_Resource temp = new View_PRJ_Resource();

                temp.是共享类资源 = item.是共享类资源;
                temp.资源编号 = item.资源编号;
                temp.资源空闲 = item.资源空闲;
                temp.资源类别编号 = item.资源类别编号;
                temp.资源类别名称 = item.资源类别名称;
                temp.资源名称 = item.资源名称;

                result.Add(temp);
            }

            return result;
        }

        /// <summary>
        /// 判断资源是否空闲
        /// </summary>
        /// <param name="resourceID">资源编号</param>
        /// <param name="beginTime">占用资源的开始时间</param>
        /// <param name="endTime">占用资源的结束时间</param>
        /// <returns>空闲返回true</returns>
        public bool IsIdle(int resourceID, DateTime beginTime, DateTime endTime)
        {
            TaskManagementDataContext ctx = CommentParameter.TaskManagementDataContext;

            return (bool)ctx.Fun_GetResourceStatus(resourceID, beginTime, endTime);
        }
    }
}
