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
    /// �첽������
    /// </summary>
    public class AsynServer : IAsynServer
    {
        #region ���ݳ�Ա

        /// <summary>
        /// �����¼�
        /// </summary>
        public event ReceiveEventHandler OnReceive;

        /// <summary>
        /// �ͻ������ӵ��������󴥷����¼�
        /// </summary>
        public event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// ������
        /// </summary>
        private Socket m_listener;

        /// <summary>
        /// �����߳�
        /// </summary>
        private Thread m_listenThread;

        /// <summary>
        /// �첽��
        /// </summary>
        private ManualResetEvent m_allDone = new ManualResetEvent(false);

        /// <summary>
        /// IP��ַ
        /// </summary>
        private IPAddress m_IP;

        /// <summary>
        /// ��ȡIP��ַ
        /// </summary>
        public IPAddress IP
        {
            get { return m_IP; }
        }

        /// <summary>
        /// �����˿ں�
        /// </summary>
        private readonly int ListenPort = 8228;

        /// <summary>
        /// ������������
        /// </summary>
        private const int ListenerAmount = 100;

        /// <summary>
        /// ��ID��ͻ���SOCKET���󹹽����ֵ�
        /// </summary>
        private Dictionary<long, Socket> m_dicClient = new Dictionary<long, Socket>();

        /// <summary>
        /// ����ͬ��
        /// </summary>
        private Mutex m_mutex = new Mutex();

        #endregion

        /// <summary>
        /// ��ʼ��
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

        #region ���캯��

        /// <summary>
        /// �첽ͨ�ŷ��������캯��(����Ĭ�ϵ�ͨ�Ŷ˿�)
        /// </summary>
        public AsynServer()
        {
            Init();
        }

        /// <summary>
        /// �첽ͨ�ŷ��������캯��
        /// </summary>
        /// <param name="Port">������Ҫ������ͨ�Ŷ˿�</param>
        public AsynServer(int port)
        {
            Init();
            ListenPort = port;
        }

        #endregion ���캯��

        #region ����

        /// <summary>
        /// ����ָ������ӿ�Ip��ָ���˿�
        /// </summary>
        public void Begin()
        {
            m_listenThread = new Thread(new ThreadStart(AcceptThread));
            m_listenThread.IsBackground = true;
            m_listenThread.Start();
        }

        /// <summary>
        /// �����ͻ�����������������¼�
        /// </summary>
        /// <param name="client">����������ӻ�Ͽ��Ŀͻ���</param>
        /// <param name="connected">Ϊtrue��ʾ���ӳɹ�, Ϊfalse��ʾ����ʧ��</param>
        private void TriggerConnectedEvent(Socket client, bool connected)
        {
            if (OnConnected != null)
            {
                OnConnected(client, connected);
                
            }
        }

        /// <summary>
        /// �������ӿͻ��˵��߳�
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

                //�������������г���
                m_listener.Listen(ListenerAmount);

                while (true)
                {
                    m_allDone.Reset();

                    //�첽������ʼ
                    m_listener.BeginAccept(new AsyncCallback(AcceptCallback), m_listener);
                    m_allDone.WaitOne();
                }
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message, "�쳣", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("{0},����AcceptThread,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        #endregion

        #region �첽����

        /// <summary>
        /// �첽��������
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
                //MessageBox.Show(err.Message, "�쳣", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("{0},����AcceptCallback,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        #endregion

        #region �첽��ȡ

        /// <summary>
        /// ��ȡ����������
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
                        //�������
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
                Console.WriteLine("{0},����ReadCallback,�쳣��{1} {2}", this.GetType().Name, socketError.ErrorCode, socketError.Message);
            }
            catch (Exception err)
            {
                Console.WriteLine("{0},����ReadCallback,�쳣��{1}", this.GetType().Name, err.Message);

                state.ClearData();
                client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);

                //if (err.Message == "�ڷ������֮ǰ����������β��")
                //{
                //    state.ClearData();
                //    client.BeginReceive(state.Buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                //}
                //else
                //{
                //    TriggerConnectedEvent(client, false);
                //    Console.WriteLine("{0},����ReadCallback,�쳣��{1}", this.GetType().Name, err.Message);
                //}
            }
            finally
            {
                m_mutex.ReleaseMutex();
            }
        }

        #endregion

        #region ͬ������

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="args">��������</param>
        /// <param name="error">����ʱ���ش�����Ϣ���޴�ʱ����null</param>
        /// <returns>�ɹ�����true, ʧ�ܷ���false</returns>
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
                //MessageBox.Show(err.Message, "�쳣", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Console.WriteLine("{0},����Close,�쳣��{1}", this.GetType().Name, err.Message);

                return false;
            }
            finally
            {
                m_dicClient.Remove(args.Id);
            }
        }

        #endregion �첽����

        #region ���л�

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

        #endregion ���л�

        #region �ر�����

        /// <summary>
        /// �ر�����
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
                Console.WriteLine("{0},����Close,�쳣��{1}", this.GetType().Name, err.Message);
            }
        }

        #endregion

        #region IDisposable ��Ա

        /// <summary>
        /// ���ٶ���
        /// </summary>
        public void Dispose()
        {
            Close();
        }

        #endregion
    }
}
