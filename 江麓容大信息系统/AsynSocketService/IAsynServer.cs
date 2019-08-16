using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AsynSocketService
{

    /// <summary>
    /// ����ί��
    /// </summary>
    /// <param name="sender">Ŀ�귢��</param>
    /// <param name="args">ͨ�Ų���</param>
    public delegate void ReceiveEventHandler(object sender, CommEventArgs args);

    /// <summary>
    /// �첽SOCKET�������ӿ�
    /// </summary>
    public interface IAsynServer : IDisposable
    {
        /// <summary>
        /// ��ȡIP��ַ
        /// </summary>
        IPAddress IP
        {
            get;
        }

        /// <summary>
        /// �ͻ������ӵ��������󴥷����¼�
        /// </summary>
        event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// �����¼�
        /// </summary>
        event ReceiveEventHandler OnReceive;

        /// <summary>
        /// ��ʼ����ָ������ӿ�Ip��ָ���˿�
        /// </summary>
        void Begin();

        /// <summary>
        /// �ر�����
        /// </summary>
        void Close();

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="args">ͨ�Ų���</param>
        /// <param name="error">����ʱ���ش�����Ϣ���޴�ʱ����null</param>
        /// <returns>�����Ƿ�ɹ��ı�־</returns>
        bool Send(CommEventArgs args, out string error);
    }
}
