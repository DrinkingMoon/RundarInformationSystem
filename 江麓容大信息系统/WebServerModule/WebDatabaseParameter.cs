/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  CommentParameter.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/07/17
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/07/17 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using DBOperate;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace WebServerModule
{
    /// <summary>
    /// 数据库参数类
    /// </summary>
    public static class WebDatabaseParameter
    {
        #region variants

        /// <summary>
        /// 数据库名称
        /// </summary>
        const string m_databaseName = "RundarWebServer";

        /// <summary>
        /// 数据库操作接口
        /// </summary>
        static IDBOperate m_dboperate;

        #endregion

        #region properties

        /// <summary>
        /// 数据库名称
        /// </summary>
        public static string DatabaseName
        {
            get { return m_databaseName; }
        }

        #endregion

        /// <summary>
        /// 获取仓库管理数据库操作上下文
        /// </summary>
        public static WebSeverDataContext WebDataContext
        {
            get
            {
                WebSeverDataContext dataContext = new WebSeverDataContext(GlobalObject.GlobalParameter.WebServerConnectionString);
                dataContext.DeferredLoadingEnabled = false;
                return dataContext;
            }
        }

        /// <summary>
        /// 获取数据库操作接口
        /// </summary>
        /// <returns>返回是否成功获取数据库操作接口</returns>
        public static IDBOperate GetDBOperate()
        {
            m_dboperate = DBFactory.Init(DBFactory.DBType.SQL, GlobalObject.GlobalParameter.WebServerConnectionString, DatabaseName);
            return m_dboperate;
        }
    }
}
