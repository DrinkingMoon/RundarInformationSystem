/****************************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 * 
 * 文件名称:   DBFactory.cs
 * 
 * 作者    :   Dennis
 * 
 * 版本:       V1.0.0511
 * 
 * 创建日期:   2009-05-11
 * 
 * 开发平台:   vs2005(c#)
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;

namespace DBOperate
{
    /// <summary>
    /// 操作工厂
    /// </summary>
    public class DBFactory
    {
        /// <summary>
        /// 数据库连接列表
        /// </summary>
        static Dictionary<string, IDBOperate> m_ConnectionList;

        /// <summary>
        /// 数据库类型
        /// </summary>
        public enum DBType
        {
            /// <summary>
            /// SQLl连接
            /// </summary>
            SQL
        }

        /// <summary>
        /// 初始化操作组件
        /// </summary>
        /// <param name="connectionCMD">连接字符串</param>
        /// <param name="type">数据库类型</param>
        /// <param name="DataBaseName">数据库名称（一个数据库对应一个连接）</param>
        /// <returns>操作接口</returns>
        public static IDBOperate Init(DBType type, string connectionCMD, string DataBaseName)
        {
            if (m_ConnectionList == null)
            {
                m_ConnectionList = new Dictionary<string, IDBOperate>();
            }

            if (!m_ConnectionList.ContainsKey(DataBaseName))
            {
                m_ConnectionList.Add(DataBaseName, new SQLOperate(connectionCMD));
            }

            return m_ConnectionList[DataBaseName];
        }

        /// <summary>
        /// 获取数据库操作接口
        /// </summary>
        /// <param name="dbName">数据库名称</param>
        /// <returns>成功则返回获取到的操作操作，失败返回null</returns>
        public static IDBOperate GetDBOperate(string dbName)
        {
            if (m_ConnectionList == null || !m_ConnectionList.ContainsKey(dbName))
                return null;
            else
                return m_ConnectionList[dbName];
        }
    }
}
