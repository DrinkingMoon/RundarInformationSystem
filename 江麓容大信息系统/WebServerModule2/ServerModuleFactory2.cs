using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace WebServerModule2
{
    /// <summary>
    /// ServerModuleFactory2
    /// </summary>
    public class ServerModuleFactory2
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

            if (m_hashTable == null)
            {
                m_hashTable = new Hashtable();
            }

            if (m_hashTable.ContainsKey(name))
            {
                return (T)m_hashTable[name];
            }
            if (typeof(T) == typeof(IServiceFeedBack2))
            {

                IServiceFeedBack2 serverModule = new ServiceFeedBack2();
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
