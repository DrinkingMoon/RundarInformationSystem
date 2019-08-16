using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.IO;
using System.Xml.Serialization;
using System.Xml;
using GlobalObject;
using System.Windows.Forms;

namespace AsynSocketService
{
    /// <summary>
    /// 异步服务器
    /// </summary>
    public class AsynServer : IAsynServer
    {
        #region 数据成员

        /// <summary>
        /// 接收事件
        /// </summary>
        public event ReceiveEventHandler OnReceive;

        /// <summary>
        /// 客户端连接到服务器后触发的事件
        /// </summary>
        public event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// 监听器
        /// </summary>
        private Socket m_listener;

        /// <summary>
        /// 监听线程
        /// </summary>
        private Thread m_listenThread;

        /// <summary>
        /// 异步锁
        /// </summary>
        private ManualResetEvent m_allDone = new ManualResetEvent(false);

        /// <summary>
        /// IP地址
        /// </summary>
        private IPAddress m_IP;

        /// <summary>
        /// 获取IP地址
        /// </summary>
        public IPAddress IP
        {
            get { return m_IP; }
        }

        /// <summary>
        /// 监听端口号
        /// </summary>
        private readonly int ListenPort = 8228;

        /// <summary>
        /// 允许最大监听数
        /// </summary>
        private const int ListenerAmount = 100;

        /// <summary>
        /// 用ID与客户端SOCKET对象构建的字典
        /// </summary>
        private Dictionary<long, Socket> m_dicClient = new Dictionary<long, Socket>();

        /// <summary>
        /// 接收同步
        /// </summary>
        private Mutex m_mutex = new Mutex();

        #endregion

        /// <summary>
        /// 初始化
        /// </summary>
        private void Init()
        {
            //IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());
            //m_IP = ipHost.AddressList[0];

            IPHostEntry ipHost = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in ipHost.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    m_IP = ip;
                    break;
                }
            }
        }

        #region 构造函数

        /// <summary>
        /// 异步通信服务器构造函数(侦听默认的通信端口)
        /// </summary>
        public AsynServer()
        {
            Init();
        }

        /// <summary>
        /// 异步通信服务器构造函数
        /// </summary>
        /// <param name="Port">服务器要侦听的通信端口</param>
        public AsynServer(int port)
        {
            Init();
            ListenPort = port;
        }

        #endregion 构造函数

        #region 监听

        /// <summary>
        /// 监听指定网络接口Ip的指定端口
        /// </summary>
        public void Begin()
        {
            m_listenThread = new Thread(new ThreadStart(AcceptThread));
            m_listenThread.IsBackground = true;
            m_listenThread.Start();
        }

        /// <summary>
        /// 触发客户端与服务器的连接事件
        /// </summary>
        /// <param name="client">与服务器连接或断开的客户端</param>
        /// <param name="connected">为true表示连接成功, 为false表示连接失败</param>
        private void TriggerConnectedEvent(Socket client, bool connected)
        {
            if (OnConnected != null)
            {
                OnConnected(client, connected);
                
            }
        }

        /// <summary>
        /// 用于连接客户端的线程
        /// </summary>
        private void AcceptThread()
        {
            try
            {
                m_listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                this.m_listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Linger, 0);
                this.m_listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.DontLinger, true);
                this.m_listener.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, 32 * 1024);

                IPEndPoint end = new IPEndPoint(m_IP, ListenPort);

                m_listener.Bind(end);

                //设置最大监听队列长度
                m_listener.Listen(ListenerAmount);

                while (true)
                {
                    m_allDone.Reset();

                    //异步监听开始
                    m_listener.BeginAccept(new AsyncCallback(AcceptCallback), m_listener);
                    m_allDone.WaitOne();
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("{0},方法AcceptThread,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        #endregion

        #region 异步接收

        /// <summary>
        /// 异步接收数据
        /// </summary>
        /// <param name="ia"></param>
        void AcceptCallback(IAsyncResult ia)
        {
            try
            {
                m_allDone.Set();

                Socket listener = ia.AsyncState as Socket;
                Socket worker = listener.EndAccept(ia);

                StateObject state = new StateObject();
                state.WorkSocket = worker;

                TriggerConnectedEvent(worker, true);

                if (worker.Connected)
                {
                    worker.BeginReceive(state.Buffer, 0, StateObject.BufferSize, SocketFlags.None,
                        new AsyncCallback(ReadCallback), state);
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("{0},方法AcceptCallback,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        #endregion

        #region 异步读取

        /// <summary>
        /// 读取缓冲区数据
        /// </summary>
        /// <param name="ar">socket</param>
        public void ReadCallback(IAsyncResult ar)
        {
            Socket client = null;
            StateObject state = null;

            try
            {
                m_mutex.WaitOne();

                state = ar.AsyncState as StateObject;
                client = state.WorkSocket;

                int bytes = client.EndReceive(ar);

                if (bytes > 0)
                {
                    state.Save(bytes);

                    TriggerConnectedEvent(client, true);


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
                                m_dicClient.Add(commArgs.Id, client);
                                OnReceive(this, commArgs);
                            }
                        }
                    }// if

                    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0,
                                      new AsyncCallback(ReadCallback), state);
                }
                else
                {
                   TriggerConnectedEvent(client,false);
                }
            }
            catch (SocketException socketError)
            {
                TriggerConnectedEvent(client, false);
                Console.WriteLine("{0},方法ReadCallback,异常：{1} {2}", this.GetType().Name, socketError.ErrorCode, socketError.Message);
            }
            catch (Exception err)
            {
                Console.WriteLine("{0},方法ReadCallback,异常：{1}", this.GetType().Name, err.Message);

                state.ClearData();
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

                //if (err.Message == "在分析完成之前就遇到流结尾。")
                //{
                //    state.ClearData();
                //    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                //}
                //else
                //{
                //    TriggerConnectedEvent(client, false);
                //    Console.WriteLine("{0},方法ReadCallback,异常：{1}", this.GetType().Name, err.Message);
                //}
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

        #endregion

        #region 同步发送

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="args">发送内容</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功返回true, 失败返回false</returns>
        public bool Send(CommEventArgs args, out string error)
        {
            try
            {
                if (m_dicClient.ContainsKey(args.Id))
                {
                    byte[] buffer = SerializeObject(args);
                    byte[] frame = new byte[buffer.Length + 2];
                    Array.Copy(buffer, frame, buffer.Length);

                    frame[frame.Length - 2] = StateObject.END[0];
                    frame[frame.Length - 1] = StateObject.END[1];

                    m_dicClient[args.Id].Send(frame);
                }

                error = null;
                return true;
            }
            catch (Exception err)
            {
                if (m_dicClient.ContainsKey(args.Id))
                {
                    TriggerConnectedEvent(m_dicClient[args.Id], false);
                }

                error = err.Message;
                //MessageBox.Show(err.Message, "异常", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("{0},方法Close,异常：{1}", this.GetType().Name, err.Message);

                return false;
            }
            finally
            {
                m_dicClient.Remove(args.Id);
            }
        }

        #endregion 异步发送

        #region 序列化

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
                
                //FileStream dumpFile = new FileStream("d:\\Dump.dat", FileMode.Create, FileAccess.ReadWrite);
                //memory.WriteTo(dumpFile);

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

        #endregion 序列化

        #region 关闭连接

        /// <summary>
        /// 关闭连接
        /// </summary>
        public void Close()
        {
            try
            {
                if (m_listener != null)
                {
                    this.m_listener.Close(0);
                    this.m_listener.Shutdown(SocketShutdown.Both);
                    this.Dispose();
                }
            }
            catch (Exception err)
            {
                Console.WriteLine("{0},方法Close,异常：{1}", this.GetType().Name, err.Message);
            }
        }

        #endregion

        #region IDisposable 成员

        /// <summary>
        /// 消毁对象
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
