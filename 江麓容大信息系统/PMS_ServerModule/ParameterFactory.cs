using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DBOperate;
using ServerModule;

namespace PMS_ServerModule
{
    /// <summary>
    /// 数据库参数类
    /// </summary>
    public static class ParameterFactory
    {
        #region variants

        /// <summary>
        /// 数据库名称
        /// </summary>
        const string m_databaseName = "PlatformService";

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
        public static PlatformServiceDataContext PlatformDataContext
        {
            get
            {
                PlatformServiceDataContext dataContext =
                    new PlatformServiceDataContext(GlobalObject.GlobalParameter.PlatformServiceConnectionString);

                //2019-5-29 夏石友，在终止执行命令的尝试并生成错误之前的等待时间，默认为30。 
                dataContext.CommandTimeout = 120;

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
            m_dboperate = DBFactory.Init(DBFactory.DBType.SQL, GlobalObject.GlobalParameter.PlatformServiceConnectionString, DatabaseName);
            return m_dboperate;
        }
    }
}
