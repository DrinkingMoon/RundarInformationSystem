using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.IO;
using System.Threading;
using SocketCommDefiniens;
using System.Xml.Serialization;
using GlobalObject;
using System.Xml;
using System.Windows.Forms;

namespace AsynSocketService
{
    /// <summary>
    /// �첽ͨ�ſͻ���ʵ��
    /// </summary>
    class AsynClient : IAsynClient
    {
        #region ��Ա����

        /// <summary>
        /// ��������������ӵ��¼���ͨ�����¼�����֪����������Ƿ�������
        /// </summary>
        public event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// �����¼�
        /// </summary>
        public event ReceiveEventHandler OnReceive;

        /// <summary>
        /// ����ͬ����
        /// </summary>
        private Mutex m_mutex = new Mutex();

        /// <summary>
        /// �ͻ���SOCKET����
        /// </summary>
        private Socket clientSocket;

        /// <summary>
        /// �ͻ���IP��ַ
        /// </summary>
        private IPAddress m_clientIP;

        /// <summary>
        /// ��������IP��ַ
        /// </summary>
        private IPAddress m_serverIP;

        /// <summary>
        /// ������ͨѶ�˿�
        /// </summary>
        private int m_serverPort;

        /// <summary>
        /// ��ʾ�Ƿ������Ϸ������ı�־
        /// </summary>
        private bool isConnection;

        /// <summary>
        /// ���ڿ���������������ӳ�ʱ���¼�
        /// </summary>
        private AutoResetEvent FConnectEvent;

        /// <summary>
        /// ������������ӳ�ʱʱ��
        /// </summary>
        //private int FConnectTimeout = 5000;

        #endregion ��Ա����

        #region ����

        /// <summary>
        /// ��ȡ�Ƿ������Ϸ������ı�־
        /// </summary>
        public bool IsConnection
        {
            get { return isConnection; }
        }

        /// <summary>
        /// ��ȡ����������ӵ�ͨ�Ŷ˿�
        /// </summary>
        public int Port
        {
            get
            {
                if (IsConnection)
                {
                    return (clientSocket.LocalEndPoint as IPEndPoint).Port;
                }

                return -1;
            }
        }


        /// <summary>
        /// ��ȡ������ͨ�Ŷ˿�
        /// </summary>
        public int ServerPort
        {
            get { return m_serverPort; }
        }

        /// <summary>
        /// ��ȡ�ͻ��˵�SOCKETͨѶ����
        /// </summary>
        public Socket Client
        {
            get { return clientSocket; }
        }

        /// <summary>
        /// ��ȡ�ͻ��˵�IP��ַ
        /// </summary>
        public IPAddress ClientIPAddress
        {
            get { return m_clientIP; }
        }

        /// <summary>
        /// ��ȡ�ͻ��˵�IP��ַ
        /// </summary>
        public string ClientIP {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}",
                    m_clientIP.GetAddressBytes()[0], m_clientIP.GetAddressBytes()[1], m_clientIP.GetAddressBytes()[2], m_clientIP.GetAddressBytes()[3]);
            }
        }

        /// <summary>
        /// ��ȡ��������IP��ַ
        /// </summary>
        public IPAddress ServerIP
        {
            get { return m_serverIP; }
        }

        #endregion ����

        /// <summary>
        /// ���캯��
        /// </summary>
        public AsynClient()
        {
        }

        /// <summary>
        /// ����ָ��IP��ַ�������豸�Ķ�Ӧ�˿�
        /// </summary>
        /// <param name="ipAddress">�����豸��IP��ַ</param>
        /// <param name="Port">ͨ�Ŷ˿�</param>
        public void Connect(string ipAddress, int port)
        {
            if (IsConnection)
            {
                return;
            }

            try
            {
                m_serverPort = port;

                FConnectEvent = new AutoResetEvent(false);
                FConnectEvent.Reset();

                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                m_serverIP = IPAddress.Parse(ipAddress);

                IPEndPoint ipEndPoint = new IPEndPoint(m_serverIP, m_serverPort);

                //Connect to the Server
                IAsyncResult result = clientSocket.BeginConnect(ipEndPoint, new AsyncCallback(OnConnect), clientSocket);
                FConnectEvent.WaitOne();

                //bool completed = result.AsyncWaitHandle.WaitOne(FConnectTimeout, false);

                //int signal = WaitHandle.WaitAny(new WaitHandle[] { FConnectEvent }, FConnectTimeout, false);

                //if (signal == WaitHandle.WaitTimeout)
                //if (!completed)
                //{
                //    TriggerConnectedEvent(false);
                //}
            }
            catch (Exception err)
            {
                //TriggerConnectedEvent(false);
                Console.WriteLine("{0},����Connect,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// �������ݵ����Ӻõ������豸
        /// </summary>
        /// <param name="args">Ҫ���͵�����</param>
        public void Send(CommEventArgs args)
        {
            try
            {
                if (IsConnection)
                {
                    byte[] buffer = SerializeObject(args);
                    byte[] frame = new byte[buffer.Length + 2];
                    Array.Copy(buffer, frame, buffer.Length);

                    frame[frame.Length - 2] = StateObject.END[0];
                    frame[frame.Length - 1] = StateObject.END[1];

                    clientSocket.BeginSend(frame, 0, frame.Length, SocketFlags.None, new AsyncCallback(OnSend), null);
                }
            }
            catch (Exception err)
            {
                TriggerConnectedEvent(false);
                Console.WriteLine("{0},����Send,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// �������ݺ�Ļص�����
        /// </summary>
        /// <param name="ar">�첽ͨ�Ų���</param>
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (Exception err)
            {
                Console.WriteLine("{0},����OnSend,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// ���������豸��Ļص�����
        /// </summary>
        /// <param name="ar">�첽ͨ�Ų���</param>
        private void OnConnect(IAsyncResult ar)
        {
            try
            {
                FConnectEvent.Set();
                clientSocket.EndConnect(ar);
                isConnection = true;
                m_clientIP = (clientSocket.LocalEndPoint as IPEndPoint).Address;
                TriggerConnectedEvent(isConnection);
                Receive();
            }
            catch (Exception err)
            {
                MessageBox.Show(string.Format("{0}, OnConnect,�쳣��{1}", this.GetType().Name, err.Message));
                TriggerConnectedEvent(false);
                Console.WriteLine("{0},����OnConnect,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// �����ͻ�����������������¼�
        /// </summary>
        /// <param name="connected">Ϊtrue��ʾ���ӳɹ�, Ϊfalse��ʾ����ʧ��</param>
        private void TriggerConnectedEvent(bool connected)
        {
            if (OnConnected != null)
            {
                OnConnected(this, connected);
            }
        }

        /// <summary>
        /// ���л����ݶ���
        /// </summary>
        /// <param name="obj">δ���л������ݶ���</param>
        /// <returns>�������л����byte[]����</returns>
        public byte[] SerializeObject(object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("null input");
            }
            try
            {
                XmlSerializer serializer = new XmlSerializer(obj.GetType(), GlobalParameter.SerializerTypes.ToArray());
                MemoryStream memory = new MemoryStream();
                XmlTextWriter writer = new XmlTextWriter(memory, Encoding.Default);

                serializer.Serialize(writer, obj);
                writer.Close();

                return memory.ToArray();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// �����л�����
        /// </summary>
        /// <param name="input">���л��������</param>
        /// <returns>���ط����л����object����</returns>
        public object DeserializeObject(byte[] input)
        {
            if (input == null)
            {
                throw new ArgumentNullException("null input");
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(CommEventArgs), GlobalParameter.SerializerTypes.ToArray());
                StreamReader stmRead = new StreamReader(new MemoryStream(input), System.Text.Encoding.Default);

                return serializer.Deserialize(stmRead);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// ���ܷ������˵�����
        /// </summary>
        private void Receive()
        {
            // ��������state.  
            StateObject state = new StateObject();
            state.WorkSocket = clientSocket;

            // ��Զ��Ŀ���������.  
            clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        /// <summary>
        /// �첽���ܷ������˵����ݺ�ĺ�ĺ���
        /// </summary>
        /// <param name="ar">�첽����״̬</param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            // ����������첽state�����л�ȡstate��socket����
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.WorkSocket;

            try
            {
                m_mutex.WaitOne();

                //��Զ���豸��ȡ����
                int bytes = client.EndReceive(ar);

                if (bytes > 0)
                {
                    // �����ݣ��洢
                    state.Save(bytes);

                    if (bytes < StateObject.BufferSize ||
                        (state.Buffer[bytes - 1] == StateObject.END[1] && state.Buffer[bytes - 2] == StateObject.END[0]))
                    {
                        //�������
                        if (OnReceive != null)
                        {
                            byte[] validData = new byte[state.FullData.Length - 2];
                            Array.Copy(state.FullData, validData, validData.Length);

                            CommEventArgs commArgs = DeserializeObject(validData) as CommEventArgs;
                            state.ClearData();

                            if (commArgs != null)
                            {
                                OnReceive(this, commArgs);
                            }
                        }
                    }// if

                    // ������ȡ
                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
                }
                else
                {
                    TriggerConnectedEvent(false);
                    Dispose();
                }
            }
            catch (Exception err)
            {
                TriggerConnectedEvent(false);
                Dispose();
                Console.WriteLine("{0},����ReceiveCallback,�쳣��{1}", this.GetType().Name, err.Message);
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

        #region IDisposable ��Ա

        /// <summary>
        /// �ͻ�������ʱ���õķ���
        /// </summary>
        public void Dispose()
        {
            if (IsConnection)
            {
                isConnection = false;
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.Close();
            }

            if (FConnectEvent != null)
            {
                FConnectEvent.Close();
                FConnectEvent = null;
            }
        }

        #endregion
    }
}
