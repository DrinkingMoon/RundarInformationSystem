using System;
using System.Collections.Generic;
using System.Text;

namespace AsynSocketService
{
    /// <summary>
    /// 异步SOCKET通信类厂
    /// </summary>
    public class AsynSocketFactory
    {
        /// <summary>
        /// 异步SOCKET通信服务器对象
        /// </summary>
        static IAsynServer server = null;

        /// <summary>
        /// 异步SOCKET通信客户端对象
        /// </summary>
        static IAsynClient client = null;

        /// <summary>
        /// 用于锁定的对象，防止多线程误操作
        /// </summary>
        static Object lockObj = new object();

        /// <summary>
        /// 采用唯一实例的服务器端字典
        /// </summary>
        static Dictionary<int, IAsynServer> m_dicServer = new Dictionary<int, IAsynServer>();

        /// <summary>
        /// 采用唯一实例的客户端字典
        /// </summary>
        static Dictionary<string, IAsynClient> m_dicClient = new Dictionary<string, IAsynClient>();

        /// <summary>
        /// 获取单一的服务器
        /// </summary>
        /// <returns>返回获取到的服务器</returns>
        static public IAsynServer GetSingletonServer()
        {
            if (server != null)
            {
                return server;
            }

            lock (lockObj)
            {
                server = new AsynServer();
            }

            return server;
        }

        /// <summary>
        /// 获取单一的服务器
        /// </summary>
        /// <param name="listeningPort">服务器侦听的端口号</param>
        /// <returns>返回获取到的服务器</returns>
        static public IAsynServer GetSingletonServer(int listeningPort)
        {
            if (m_dicServer.ContainsKey(listeningPort))
            {
                return m_dicServer[listeningPort];
            }

            lock (lockObj)
            {
                server = new AsynServer(listeningPort);
                m_dicServer.Add(listeningPort, server);
            }

            return server;
        }

        /// <summary>
        /// 获取客户端
        /// </summary>
        /// <returns>返回获取到的客户端</returns>
        static public IAsynClient GetClient()
        {
            return new AsynClient();
        }

        /// <summary>
        /// 获取单一的客户端
        /// </summary>
        /// <returns>返回获取到的客户端实例</returns>
        static public IAsynClient GetSingletonClient()
        {
            if (client != null)
            {
                return client;
            }

            lock (lockObj)
            {
                client = new AsynClient();
            }

            return client;
        }

        /// <summary>
        /// 获取单一的客户端
        /// </summary>
        /// <param name="clientName">客户端名称</param>
        /// <returns>返回获取到的客户端实例</returns>
        static public IAsynClient GetSingletonClient(string clientName)
        {
            if (m_dicClient.ContainsKey(clientName))
            {
                return m_dicClient[clientName];
            }

            lock (lockObj)
            {
                IAsynClient client = new AsynClient();

                m_dicClient.Add(clientName, client);
                return client;
            }
        }
    }
}
