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
    /// 异步通信客户端实体
    /// </summary>
    class AsynClient : IAsynClient
    {
        #region 成员变量

        /// <summary>
        /// 与服务器进行连接的事件，通过该事件可以知道与服务器是否连接上
        /// </summary>
        public event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// 接收事件
        /// </summary>
        public event ReceiveEventHandler OnReceive;

        /// <summary>
        /// 接收同步用
        /// </summary>
        private Mutex m_mutex = new Mutex();

        /// <summary>
        /// 客户端SOCKET对象
        /// </summary>
        private Socket clientSocket;

        /// <summary>
        /// 客户端IP地址
        /// </summary>
        private IPAddress m_clientIP;

        /// <summary>
        /// 服务器端IP地址
        /// </summary>
        private IPAddress m_serverIP;

        /// <summary>
        /// 服务器通讯端口
        /// </summary>
        private int m_serverPort;

        /// <summary>
        /// 表示是否连接上服务器的标志
        /// </summary>
        private bool isConnection;

        /// <summary>
        /// 用于控制与服务器的连接超时的事件
        /// </summary>
        private AutoResetEvent FConnectEvent;

        /// <summary>
        /// 与服务器的连接超时时间
        /// </summary>
        //private int FConnectTimeout = 5000;

        #endregion 成员变量

        #region 属性

        /// <summary>
        /// 获取是否连接上服务器的标志
        /// </summary>
        public bool IsConnection
        {
            get { return isConnection; }
        }

        /// <summary>
        /// 获取与服务器连接的通信端口
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
        /// 获取服务器通信端口
        /// </summary>
        public int ServerPort
        {
            get { return m_serverPort; }
        }

        /// <summary>
        /// 获取客户端的SOCKET通讯对象
        /// </summary>
        public Socket Client
        {
            get { return clientSocket; }
        }

        /// <summary>
        /// 获取客户端的IP地址
        /// </summary>
        public IPAddress ClientIPAddress
        {
            get { return m_clientIP; }
        }

        /// <summary>
        /// 获取客户端的IP地址
        /// </summary>
        public string ClientIP {
            get
            {
                return string.Format("{0}.{1}.{2}.{3}",
                    m_clientIP.GetAddressBytes()[0], m_clientIP.GetAddressBytes()[1], m_clientIP.GetAddressBytes()[2], m_clientIP.GetAddressBytes()[3]);
            }
        }

        /// <summary>
        /// 获取服务器的IP地址
        /// </summary>
        public IPAddress ServerIP
        {
            get { return m_serverIP; }
        }

        #endregion 属性

        /// <summary>
        /// 构造函数
        /// </summary>
        public AsynClient()
        {
        }

        /// <summary>
        /// 连接指定IP地址的网络设备的对应端口
        /// </summary>
        /// <param name="ipAddress">网络设备的IP地址</param>
        /// <param name="Port">通信端口</param>
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
                Console.WriteLine("{0},方法Connect,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// 发送数据到连接好的网络设备
        /// </summary>
        /// <param name="args">要发送的数据</param>
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
                Console.WriteLine("{0},方法Send,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// 发送数据后的回调函数
        /// </summary>
        /// <param name="ar">异步通信参数</param>
        private void OnSend(IAsyncResult ar)
        {
            try
            {
                clientSocket.EndSend(ar);
            }
            catch (Exception err)
            {
                Console.WriteLine("{0},方法OnSend,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// 连接网络设备后的回调函数
        /// </summary>
        /// <param name="ar">异步通信参数</param>
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
                MessageBox.Show(string.Format("{0}, OnConnect,异常：{1}", this.GetType().Name, err.Message));
                TriggerConnectedEvent(false);
                Console.WriteLine("{0},方法OnConnect,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        /// <summary>
        /// 触发客户端与服务器的连接事件
        /// </summary>
        /// <param name="connected">为true表示连接成功, 为false表示连接失败</param>
        private void TriggerConnectedEvent(bool connected)
        {
            if (OnConnected != null)
            {
                OnConnected(this, connected);
            }
        }

        /// <summary>
        /// 序列化数据对象
        /// </summary>
        /// <param name="obj">未序列化的数据对象</param>
        /// <returns>返回序列化后的byte[]数组</returns>
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
        /// 反序列化数据
        /// </summary>
        /// <param name="input">序列化后的数据</param>
        /// <returns>返回反序列化后的object数据</returns>
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
        /// 接受服务器端的数据
        /// </summary>
        private void Receive()
        {
            // 构造容器state.  
            StateObject state = new StateObject();
            state.WorkSocket = clientSocket;

            // 从远程目标接收数据.  
            clientSocket.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReceiveCallback), state);
        }

        /// <summary>
        /// 异步接受服务器端的数据后的后的函数
        /// </summary>
        /// <param name="ar">异步操作状态</param>
        private void ReceiveCallback(IAsyncResult ar)
        {
            // 从输入参数异步state对象中获取state和socket对象
            StateObject state = (StateObject)ar.AsyncState;
            Socket client = state.WorkSocket;

            try
            {
                m_mutex.WaitOne();

                //从远程设备读取数据
                int bytes = client.EndReceive(ar);

                if (bytes > 0)
                {
                    // 有数据，存储
                    state.Save(bytes);

                    if (bytes < StateObject.BufferSize ||
                        (state.Buffer[bytes - 1] == StateObject.END[1] && state.Buffer[bytes - 2] == StateObject.END[0]))
                    {
                        //接收完成
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

                    // 继续读取
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
                Console.WriteLine("{0},方法ReceiveCallback,异常：{1}", this.GetType().Name, err.Message);
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

        #region IDisposable 成员

        /// <summary>
        /// 客户端销毁时调用的方法
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
