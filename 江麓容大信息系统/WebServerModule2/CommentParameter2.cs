using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBOperate;

namespace WebServerModule2
{
    /// <summary>
    /// CommentParameter2
    /// </summary>
    public class CommentParameter2
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
        public static WebSiteDataClassesDataContext WebDataContext
        {
            get
            {
                WebSiteDataClassesDataContext dataContext = new WebSiteDataClassesDataContext(GlobalObject.GlobalParameter.WebServerConnectionString);
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
