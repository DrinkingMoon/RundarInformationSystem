using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Collections;

namespace ProvidersServerModule
{
     /// <summary>
    /// 管理类厂
    /// </summary>
    public class ServerModuleFactory
    {
        /// <summary>
        /// 用于保证服务组件实例的唯一性
        /// </summary>
        static Hashtable m_hashTable = null;

        /// <summary>
        /// 获取服务组件
        /// </summary>
        /// <returns>返回组件接口</returns>
        public static T GetServerModule<T>()
        {
            string name = typeof(T).ToString();

            m_hashTable = new Hashtable();

            //if (m_hashTable.ContainsKey(name))
            //{
            //    return (T)m_hashTable[name];
            //}

            if (typeof(T) == typeof(IProvidersBaseServer))
            {
                IProvidersBaseServer serverModule = new ProvidersBaseServer();
                m_hashTable.Add(name, serverModule);
            }
            
            if (m_hashTable.ContainsKey(name))
            {
                return (T)m_hashTable[name];
            }

            return default(T);
        }
    }
}
