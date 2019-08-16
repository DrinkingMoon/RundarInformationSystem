using System;
using System.Collections.Generic;
using System.Text;

namespace AsynSocketService
{
    /// <summary>
    /// �첽SOCKETͨ���೧
    /// </summary>
    public class AsynSocketFactory
    {
        /// <summary>
        /// �첽SOCKETͨ�ŷ���������
        /// </summary>
        static IAsynServer server = null;

        /// <summary>
        /// �첽SOCKETͨ�ſͻ��˶���
        /// </summary>
        static IAsynClient client = null;

        /// <summary>
        /// ���������Ķ��󣬷�ֹ���߳������
        /// </summary>
        static Object lockObj = new object();

        /// <summary>
        /// ����Ψһʵ���ķ��������ֵ�
        /// </summary>
        static Dictionary<int, IAsynServer> m_dicServer = new Dictionary<int, IAsynServer>();

        /// <summary>
        /// ����Ψһʵ���Ŀͻ����ֵ�
        /// </summary>
        static Dictionary<string, IAsynClient> m_dicClient = new Dictionary<string, IAsynClient>();

        /// <summary>
        /// ��ȡ��һ�ķ�����
        /// </summary>
        /// <returns>���ػ�ȡ���ķ�����</returns>
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
        /// ��ȡ��һ�ķ�����
        /// </summary>
        /// <param name="listeningPort">�����������Ķ˿ں�</param>
        /// <returns>���ػ�ȡ���ķ�����</returns>
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
        /// ��ȡ�ͻ���
        /// </summary>
        /// <returns>���ػ�ȡ���Ŀͻ���</returns>
        static public IAsynClient GetClient()
        {
            return new AsynClient();
        }

        /// <summary>
        /// ��ȡ��һ�Ŀͻ���
        /// </summary>
        /// <returns>���ػ�ȡ���Ŀͻ���ʵ��</returns>
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
        /// ��ȡ��һ�Ŀͻ���
        /// </summary>
        /// <param name="clientName">�ͻ�������</param>
        /// <returns>���ػ�ȡ���Ŀͻ���ʵ��</returns>
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
