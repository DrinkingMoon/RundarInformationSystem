using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 跑马灯的内容显示类
    /// </summary>
    class MarqueeServer : IMarqueeServer
    {
        /// <summary>
        /// 获得所有需要显示的内容
        /// </summary>
        /// <returns>返回数据集</returns>
        public DataTable GetMarquee()
        {
            string sql = "select ShowContent from Sys_RunShowWarning order by ID desc";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(sql);
            return dt;
        }
    }
}
