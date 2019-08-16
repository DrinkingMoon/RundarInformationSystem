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
using TaskServerModule;

namespace ServerModule
{
    /// <summary>
    /// 数据库参数类
    /// </summary>
    public static class CommentParameter
    {
        #region variants

        /// <summary>
        /// 仓储管理数据库名称
        /// </summary>
        const string m_dbNameOfDepotManagement = "DepotManagement";

        /// <summary>
        /// 仓储管理数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperatorOfDepotManagement;

        /// <summary>
        /// 任务管理数据库名称
        /// </summary>
        const string m_dbNameOfTaskManagement = "TaskManagement";

        /// <summary>
        /// 仓储管理数据库操作接口
        /// </summary>
        static IDBOperate m_dbOperatorOfTaskManagement;

        #endregion

        #region properties

        /// <summary>
        /// 仓储管理数据库名称
        /// </summary>
        public static string DatabaseNameOfDepotManagement
        {
            get { return m_dbNameOfDepotManagement; }
        }

        /// <summary>
        /// 任务管理数据库名称
        /// </summary>
        public static string DatabaseNameOfTaskManagement
        {
            get { return m_dbNameOfTaskManagement; }
        }

        #endregion

        /// <summary>
        /// 获取仓库管理数据库操作上下文
        /// </summary>
        public static DepotManagementDataContext DepotDataContext
        {
            get
            {
                DepotManagementDataContext dataContext = 
                    new DepotManagementDataContext(GlobalObject.GlobalParameter.StorehouseConnectionString);

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
        public static IDBOperate GetDBOperatorOfDepotManagement()
        {
            m_dbOperatorOfDepotManagement = 
                DBFactory.Init(DBFactory.DBType.SQL, GlobalObject.GlobalParameter.StorehouseConnectionString, 
                DatabaseNameOfDepotManagement);
            return m_dbOperatorOfDepotManagement;
        }

        /// <summary>
        /// 获取任务管理数据库操作上下文
        /// </summary>
        public static TaskManagementDataContext TaskManagementDataContext
        {
            get
            {
                TaskManagementDataContext dataContext =
                    new TaskManagementDataContext(GlobalObject.GlobalParameter.TaskConnectionString);

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
        public static IDBOperate GetDBOperatorOfTaskManagement()
        {
            m_dbOperatorOfTaskManagement = DBFactory.Init(DBFactory.DBType.SQL, GlobalObject.GlobalParameter.TaskConnectionString, DatabaseNameOfTaskManagement);
            return m_dbOperatorOfTaskManagement;
        }
    }
}
