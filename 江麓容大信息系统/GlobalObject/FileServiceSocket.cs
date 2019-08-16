/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FTPService.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/01/22
 * 开发平台:  Visual C# 2008
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 ******************************************************************************/
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Data;

namespace GlobalObject
{
    /// <summary>
    /// FTP类
    /// </summary>
    public class FileServiceSocket
    {
        #region 变量声明

        /// <summary>
        /// 服务器连接地址
        /// </summary>
        private string server;

        /// <summary>
        /// 登陆帐号
        /// </summary>
        private string user;

        /// <summary>
        /// 登陆口令
        /// </summary>
        private string pass;
        /// <summary>
        /// 端口号
        /// </summary>
        private int port;
        /// <summary>
        /// 无响应时间（FTP在指定时间内无响应）
        /// </summary>
        private int timeout;
        /// <summary>
        /// 服务器错误状态信息
        /// </summary>
        private string errormessage;

        /// <summary>
        /// 服务器错误状态信息
        /// </summary>
        public string Errormessage
        {
            get { return errormessage; }
            set { errormessage = value; }
        }

        /// <summary>
        /// 服务器状态返回信息
        /// </summary>
        private string messages;

        /// <summary>
        /// 服务器的响应信息
        /// </summary>
        private string responseStr;

        /// <summary>
        /// 链接模式（主动或被动，默认为被动）
        /// </summary>
        private bool passive_mode;

        /// <summary>
        /// 上传或下载信息字节数
        /// </summary>
        private long bytes_total;

        /// <summary>
        /// 上传或下载的文件大小
        /// </summary>
        private long file_size;

        /// <summary>
        /// 主套接字
        /// </summary>
        private Socket main_sock;

        /// <summary>
        /// 要链接的网络地址终结点
        /// </summary>
        private IPEndPoint main_ipEndPoint;

        /// <summary>
        /// 侦听套接字
        /// </summary>
        private Socket listening_sock;

        /// <summary>
        /// 数据套接字
        /// </summary>
        private Socket data_sock;

        /// <summary>
        /// 要链接的网络数据地址终结点
        /// </summary>
        private IPEndPoint data_ipEndPoint;

        /// <summary>
        /// 用于上传或下载的文件流对象
        /// </summary>
        private FileStream file;

        /// <summary>
        /// 与FTP服务器交互的状态值
        /// </summary>
        private int response;

        /// <summary>
        /// 读取并保存当前命令执行后从FTP服务器端返回的数据信息
        /// </summary>
        private string bucket;

        #endregion

        #region 构造函数

        /// <summary>
        /// 构造函数
        /// </summary>
        public FileServiceSocket()
        {
            server = null;
            user = null;
            pass = null;
            port = 21;
            passive_mode = true;
            main_sock = null;
            main_ipEndPoint = null;
            listening_sock = null;
            data_sock = null;
            data_ipEndPoint = null;
            file = null;
            bucket = "";
            bytes_total = 0;
            timeout = 10000;    //无响应时间为10秒
            messages = "";
            errormessage = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Server">服务器IP或名称</param>
        /// <param name="User">登陆帐号</param>
        /// <param name="Pass">登陆口令</param>
        public FileServiceSocket(string server, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            port = 21;
            passive_mode = true;
            main_sock = null;
            main_ipEndPoint = null;
            listening_sock = null;
            data_sock = null;
            data_ipEndPoint = null;
            file = null;
            bucket = "";
            bytes_total = 0;
            timeout = 10000;    //无响应时间为10秒
            messages = "";
            errormessage = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Server">服务器IP或名称</param>
        /// <param name="Port">端口号</param>
        /// <param name="User">登陆帐号</param>
        /// <param name="Pass">登陆口令</param>
        public FileServiceSocket(string server, int port, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            passive_mode = true;
            main_sock = null;
            main_ipEndPoint = null;
            listening_sock = null;
            data_sock = null;
            data_ipEndPoint = null;
            file = null;
            bucket = "";
            bytes_total = 0;
            timeout = 10000;    //无响应时间为10秒
            messages = "";
            errormessage = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Server">服务器IP或名称</param>
        /// <param name="Port">端口号</param>
        /// <param name="User">登陆帐号</param>
        /// <param name="Pass">登陆口令</param>
        /// <param name="mode">链接方式</param>
        public FileServiceSocket(string server, int port, string user, string pass, int mode)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            passive_mode = mode <= 1 ? true : false;
            main_sock = null;
            main_ipEndPoint = null;
            listening_sock = null;
            data_sock = null;
            data_ipEndPoint = null;
            file = null;
            bucket = "";
            bytes_total = 0;
            this.timeout = 10000;    //无响应时间为10秒
            messages = "";
            errormessage = "";
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="Server">服务器IP或名称</param>
        /// <param name="Port">端口号</param>
        /// <param name="User">登陆帐号</param>
        /// <param name="Pass">登陆口令</param>
        /// <param name="mode">链接方式</param>
        /// <param name="Timeout">无响应时间(限时),单位:秒 (小于或等于0为不受时间限制)</param>
        public FileServiceSocket(string server, int port, string user, string pass, int mode, int timeout_sec)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;
            passive_mode = mode <= 1 ? true : false;
            main_sock = null;
            main_ipEndPoint = null;
            listening_sock = null;
            data_sock = null;
            data_ipEndPoint = null;
            file = null;
            bucket = "";
            bytes_total = 0;
            this.timeout = (timeout_sec <= 0) ? int.MaxValue : (timeout_sec * 1000);    //无响应时间
            messages = "";
            errormessage = "";
        }

        #endregion

        #region 属性
        /// <summary>
        /// 当前是否已连接
        /// </summary>
        public bool IsConnected
        {
            get
            {
                if (main_sock != null)
                    return main_sock.Connected;
                return false;
            }
        }

        /// <summary>
        /// 当message缓冲区有数据则返回
        /// </summary>
        public bool MessagesAvailable
        {
            get
            {
                if (messages.Length > 0)
                    return true;
                return false;
            }
        }

        /// <summary>
        /// 获取服务器状态返回信息, 并清空messages变量
        /// </summary>
        public string Messages
        {
            get
            {
                string tmp = messages;
                messages = "";
                return tmp;
            }
        }

        /// <summary>
        /// 最新指令发出后服务器的响应
        /// </summary>
        public string ResponseString
        {
            get
            {
                return responseStr;
            }
        }

        /// <summary>
        ///在一次传输中,发送或接收的字节数
        /// </summary>
        public long BytesTotal
        {
            get
            {
                return bytes_total;
            }
        }

        /// <summary>
        ///被下载或上传的文件大小,当文件大小无效时为0
        /// </summary>
        public long FileSize
        {
            get
            {
                return file_size;
            }
        }

        /// <summary>
        /// 链接模式: 
        /// true 被动模式 [默认]
        /// false: 主动模式
        /// </summary>
        public bool PassiveMode
        {
            get
            {
                return passive_mode;
            }
            set
            {
                passive_mode = value;
            }
        }

        #endregion

        #region 操作

        #region 基础部分

        /// <summary>
        /// 操作失败
        /// </summary>
        private void Fail()
        {
            Disconnect();
            errormessage += responseStr;
            //throw new Exception(responseStr);
        }

        /// <summary>
        /// 下载文件类型
        /// </summary>
        /// <param name="mode">true:二进制文件 false:字符文件</param>
        private void SetBinaryMode(bool mode)
        {
            if (mode)
                SendCommand("TYPE I");
            else
                SendCommand("TYPE A");

            ReadResponse();
            if (response != 200)
                Fail();
        }

        /// <summary>
        /// 发送命令
        /// </summary>
        /// <param name="command"></param>
        private void SendCommand(string command)
        {
            Byte[] cmd = Encoding.GetEncoding("GB2312").GetBytes((command + "\r\n").ToCharArray());

            if (command.Length > 3 && command.Substring(0, 4) == "PASS")
            {
                messages = "\rPASS xxx";
            }
            else
            {
                messages = "\r" + command;
            }

            try
            {
                main_sock.Send(cmd, cmd.Length, 0);
            }
            catch (Exception ex)
            {
                try
                {
                    Disconnect();
                    throw new Exception(ex.Message);
                    //errormessage += ex.Message;
                    //return;
                }
                catch
                {
                    main_sock.Close();
                    if (file != null)
                        file.Close();
                    main_sock = null;
                    main_ipEndPoint = null;
                    file = null;
                }
            }
        }

        private void FillBucket()
        {
            Byte[] bytes = new Byte[512];
            long bytesgot;
            int msecs_passed = 0;

            while (main_sock.Available < 1)
            {
                System.Threading.Thread.Sleep(50);
                msecs_passed += 50;
                //当等待时间到,则断开链接
                if (msecs_passed > timeout)
                {
                    Disconnect();
                    errormessage += "Timed out waiting on Server to respond.";
                    return;
                }
            }

            while (main_sock.Available > 0)
            {
                bytesgot = main_sock.Receive(bytes, 512, 0);
                bucket += Encoding.GetEncoding("GB2312").GetString(bytes, 0, (int)bytesgot);
                System.Threading.Thread.Sleep(50);
            }
        }

        private string GetLineFromBucket()
        {
            int i;
            string buf = "";

            if ((i = bucket.IndexOf('\n')) < 0)
            {
                while (i < 0)
                {
                    FillBucket();
                    i = bucket.IndexOf('\n');
                }
            }

            buf = bucket.Substring(0, i);
            bucket = bucket.Substring(i + 1);

            return buf;
        }

        /// <summary>
        /// 返回服务器端返回信息
        /// </summary>
        private void ReadResponse()
        {
            string buf;
            messages = "";

            while (true)
            {
                buf = GetLineFromBucket();

                if (Regex.Match(buf, "^[0-9]+ ").Success)
                {
                    responseStr = buf;
                    response = int.Parse(buf.Substring(0, 3));
                    break;
                }
                else
                    messages += Regex.Replace(buf, "^[0-9]+-", "") + "\n";
            }
        }

        /// <summary>
        /// 打开数据套接字
        /// </summary>
        private void OpenDataSocket()
        {
            if (passive_mode)
            {
                string[] pasv;
                string server;
                int port;

                Connect();
                SendCommand("PASV");
                ReadResponse();
                if (response != 227)
                    Fail();

                try
                {
                    int i1, i2;

                    i1 = responseStr.IndexOf('(') + 1;
                    i2 = responseStr.IndexOf(')') - i1;
                    pasv = responseStr.Substring(i1, i2).Split(',');
                }
                catch (Exception)
                {
                    Disconnect();
                    errormessage += "Malformed PASV response: " + responseStr;
                    return;
                }

                if (pasv.Length < 6)
                {
                    Disconnect();
                    errormessage += "Malformed PASV response: " + responseStr;
                    return;
                }

                server = String.Format("{0}.{1}.{2}.{3}", pasv[0], pasv[1], pasv[2], pasv[3]);
                port = (int.Parse(pasv[4]) << 8) + int.Parse(pasv[5]);

                try
                {
                    CloseDataSocket();

                    data_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

#if NET1
                    IPAddress[] addrIP = Dns.GetHostByName(server).AddressList;
                    IPAddress localAddress = null;
                    Boolean ipv4 = false;

                    foreach (IPAddress ip in addrIP)
                    {
                        //筛选出IPV4地址
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localAddress = ip;
                            ipv4 = true;
                            break;
                        }
                    }

                    if (!ipv4)
                    {
                        localAddress = addrIP[0];
                    }

                    data_ipEndPoint = new IPEndPoint(localAddress, port);
#else
                    IPAddress[] addrIP = System.Net.Dns.GetHostEntry(server).AddressList;
                    IPAddress localAddress = null;
                    Boolean ipv4 = false;

                    foreach (IPAddress ip in addrIP)
                    {
                        //筛选出IPV4地址
                        if (ip.AddressFamily == AddressFamily.InterNetwork)
                        {
                            localAddress = ip;
                            ipv4 = true;
                            break;
                        }
                    }

                    if (!ipv4)
                    {
                        localAddress = addrIP[0];
                    }

                    data_ipEndPoint = new IPEndPoint(localAddress, port);
#endif

                    data_sock.Connect(data_ipEndPoint);

                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to connect for data transfer: " + ex.Message);
                    //errormessage += "Failed to connect for data transfer: " + ex.Message;
                    //return;
                }
            }
            else
            {
                Connect();

                try
                {
                    CloseDataSocket();

                    listening_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    // 对于端口,则发送IP地址.下面则提取相应信息
                    string sLocAddr = main_sock.LocalEndPoint.ToString();
                    int ix = sLocAddr.IndexOf(':');
                    if (ix < 0)
                    {
                        errormessage += "Failed to parse the local address: " + sLocAddr;
                        return;
                    }
                    string sIPAddr = sLocAddr.Substring(0, ix);
                    // 系统自动绑定一个端口号(设置 Port = 0)
                    System.Net.IPEndPoint localEP = new IPEndPoint(IPAddress.Parse(sIPAddr), 0);

                    listening_sock.Bind(localEP);
                    sLocAddr = listening_sock.LocalEndPoint.ToString();
                    ix = sLocAddr.IndexOf(':');
                    if (ix < 0)
                    {
                        errormessage += "Failed to parse the local address: " + sLocAddr;

                    }
                    int nPort = int.Parse(sLocAddr.Substring(ix + 1));

                    // 开始侦听链接请求
                    listening_sock.Listen(1);
                    string sPortCmd = string.Format("PORT {0},{1},{2}",
                                                    sIPAddr.Replace('.', ','),
                                                    nPort / 256, nPort % 256);
                    SendCommand(sPortCmd);
                    ReadResponse();
                    if (response != 200)
                        Fail();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to connect for data transfer: " + ex.Message);
                    //errormessage += "Failed to connect for data transfer: " + ex.Message;
                    //return;
                }
            }
        }

        private void ConnectDataSocket()
        {
            if (data_sock != null)        // 已链接
                return;

            try
            {
                data_sock = listening_sock.Accept();    // Accept is blocking
                listening_sock.Close();
                listening_sock = null;

                if (data_sock == null)
                {
                    throw new Exception("Winsock error: " +
                        Convert.ToString(System.Runtime.InteropServices.Marshal.GetLastWin32Error()));
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to connect for data transfer: " + ex.Message);
                //errormessage += "Failed to connect for data transfer: " + ex.Message;
            }
        }

        private void CloseDataSocket()
        {
            if (data_sock != null)
            {
                if (data_sock.Connected)
                {
                    data_sock.Close();
                }
                data_sock = null;
            }

            data_ipEndPoint = null;
        }

        /// <summary>
        /// 关闭所有链接
        /// </summary>
        public void Disconnect()
        {
            CloseDataSocket();

            if (main_sock != null)
            {
                if (main_sock.Connected)
                {
                    SendCommand("QUIT");
                    main_sock.Close();
                }
                main_sock = null;
            }

            if (file != null)
                file.Close();

            main_ipEndPoint = null;
            file = null;
        }

        /// <summary>
        /// 链接到FTP服务器
        /// </summary>
        /// <param name="Server">要链接的IP地址或主机名</param>
        /// <param name="Port">端口号</param>
        /// <param name="User">登陆帐号</param>
        /// <param name="Pass">登陆口令</param>
        public void Connect(string server, int port, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;
            this.port = port;

            Connect();
        }

        /// <summary>
        /// 链接到FTP服务器
        /// </summary>
        /// <param name="Server">要链接的IP地址或主机名</param>
        /// <param name="User">登陆帐号</param>
        /// <param name="Pass">登陆口令</param>
        public void Connect(string server, string user, string pass)
        {
            this.server = server;
            this.user = user;
            this.pass = pass;

            Connect();
        }

        /// <summary>
        /// 链接到FTP服务器
        /// </summary>
        public bool Connect()
        {
            errormessage = "";

            if (server == null)
            {
                errormessage += "No Server has been set.\r\n";
            }
            if (user == null)
            {
                errormessage += "No Server has been set.\r\n";
            }

            if (main_sock != null)
                if (main_sock.Connected)
                    return true;

            try
            {
                //FtpWebResponse response1 = null; 
                //Stream stream = null;

                //Uri baseUri = new Uri("ftp://" + Server +"/");//txtServer输入形如ftp://server的字符串 
              
                //FtpWebRequest request =(FtpWebRequest)WebRequest.Create(baseUri);

                ////第二步，设置访问服务器的用户名和密码               
                //request.Credentials = new NetworkCredential(User, Pass);

                ////访问Ftp服务器的用户名和密码//第三步，设置访问服务器的方法               
                //request.Method = WebRequestMethods.Ftp.ListDirectory; 

                ////访问ftp服务器的方法，              
                //// Send the request to the Server.
                ////第四步，将request发送到服务器//将请求发送到ftp服务器               
                //response1 = (FtpWebResponse)request.GetResponse();

                ////第五步，获取服务器的response              
                //// Read the response and fill the list box.              
                //stream = response1.GetResponseStream();

                ////第六步，对从服务器获取的stream进行处理               
                //Encoding encode = System.Text.Encoding.GetEncoding("gb2312");

                main_sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
#if NET1
                IPAddress[] addrIP = Dns.GetHostByName(server).AddressList;
                IPAddress localAddress = null;
                Boolean ipv4 = false;

                foreach (IPAddress ip in addrIP)
                {
                    //筛选出IPV4地址
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localAddress = ip;
                        ipv4 = true;
                        break;
                    }
                }

                if (!ipv4)
                {
                    localAddress = addrIP[0];
                }

                main_ipEndPoint = new IPEndPoint(localAddress, port);
#else
                IPAddress[] addrIP = System.Net.Dns.GetHostEntry(server).AddressList;
                IPAddress localAddress = null;
                Boolean ipv4 = false;

                foreach (IPAddress ip in addrIP)
                {
                    //筛选出IPV4地址
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        localAddress = ip;
                        ipv4 = true;
                        break;
                    }
                }

                if (!ipv4)
                {
                    localAddress = addrIP[0];
                }

                main_ipEndPoint = new IPEndPoint(localAddress, port);
#endif
                main_sock.Connect(main_ipEndPoint);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //errormessage += ex.Message;
                //return false;
            }

            ReadResponse();
            if (response != 220)
                Fail();

            SendCommand("USER " + user);
            ReadResponse();

            switch (response)
            {
                case 331:
                    if (pass == null)
                    {
                        Disconnect();
                        errormessage += "No password has been set.";
                        return false;
                    }
                    SendCommand("PASS " + pass);
                    ReadResponse();
                    if (response != 230)
                    {
                        Fail();
                        return false;
                    }
                    break;
                case 230:
                    break;
            }

            return true;
        }
        #endregion

        #region 简单部分
        /// <summary>
        /// 获取FTP当前(工作)目录下的文件列表
        /// </summary>
        /// <returns>返回文件列表数组</returns>
        public ArrayList List(string cmd)
        {
            Byte[] bytes = new Byte[512];
            string file_list = "";
            long bytesgot = 0;
            int msecs_passed = 0;
            ArrayList list = new ArrayList();

            Connect();
            OpenDataSocket();
            SendCommand(cmd);
            ReadResponse();

            switch (response)
            {
                case 125:
                case 150:
                    break;
                default:
                    CloseDataSocket();
                    throw new Exception(responseStr);
            }
            ConnectDataSocket();

            while (data_sock.Available < 1)
            {
                System.Threading.Thread.Sleep(50);
                msecs_passed += 50;

                if (msecs_passed > (timeout / 10))
                {
                    break;
                }
            }

            while (data_sock.Available > 0)
            {
                bytesgot = data_sock.Receive(bytes, bytes.Length, 0);
                file_list += Encoding.GetEncoding("GB2312").GetString(bytes, 0, (int)bytesgot);
                System.Threading.Thread.Sleep(50);
            }

            CloseDataSocket();

            ReadResponse();
            if (response != 226)
                throw new Exception(responseStr);

            foreach (string f in file_list.Split('\n'))
            {
                if (f.Length > 0 && !Regex.Match(f, "^total").Success)
                    list.Add(f.Substring(0, f.Length - 1));
            }

            return list;
        }

        /// <summary>
        /// 获取所有文件明细信息
        /// </summary>
        /// <returns></returns>
        public ArrayList ListFilesDetail()
        {
            return List("LIST");
        }

        /// <summary>
        /// 获取所有文件明细信息
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns></returns>
        public ArrayList ListFilesDetail(string path)
        {
            ArrayList list = new ArrayList();
            if (ChangeDir(path))
            {
                list = List("LIST");
                ChangeDir("");
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取到文件名列表(包括文件和目录)
        /// </summary>
        /// <returns>返回文件名列表</returns>
        public ArrayList ListFiles()
        {
            ArrayList list = new ArrayList();

            foreach (string f in List("NLST "))
            {
                if ((f.Length > 0))
                {
                    if ((f[0] != 'd') && (f.ToUpper().IndexOf("<DIR>") < 0))
                        list.Add(f);
                }
            }

            return list;
        }

        /// <summary>
        /// 获取到文件名列表(包括文件和目录)
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>返回列表</returns>
        public ArrayList ListFiles(string path)
        {
            if (ChangeDir(path))
            {
                ArrayList list = new ArrayList();

                foreach (string f in List("NLST "))
                {
                    if ((f.Length > 0))
                    {
                        if ((f[0] != 'd') && (f.ToUpper().IndexOf("<DIR>") < 0))
                            list.Add(f);
                    }
                }

                ChangeDir("");

                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取到文件名列表（仅文件）
        /// </summary>
        /// <returns>返回文件名列表</returns>
        public ArrayList ListFile()
        {
            ArrayList list = new ArrayList();

            foreach (string f in List("NLST "))
            {
                if ((f.Length > 0))
                {
                    if ((f[0] != 'd') && (f.ToUpper().IndexOf("<DIR>") < 0))
                        list.Add(f);
                }
            }

            ArrayList listDir = ListDir();

            foreach (string strDir in listDir)
            {
                list.Remove(strDir);
            }

            return list;
        }

        /// <summary>
        /// 获取到文件名列表（仅文件）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>返回列表</returns>
        public ArrayList ListFile(string path)
        {
            if (ChangeDir(path))
            {
                ArrayList list = new ArrayList();

                foreach (string f in List("NLST "))
                {
                    if ((f.Length > 0))
                    {
                        if ((f[0] != 'd') && (f.ToUpper().IndexOf("<DIR>") < 0))
                            list.Add(f);
                    }
                }

                ArrayList listDir = ListDir();

                foreach (string strDir in listDir)
                {
                    list.Remove(strDir);
                }

                ChangeDir("");

                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取文件名列表（仅目录）
        /// </summary>
        /// <returns>返回列表</returns>
        public ArrayList ListDir()
        {
            ArrayList list = new ArrayList();

            foreach (string f in List("LIST"))
            {
                if (f.Length > 0)
                {
                    if ((f[0] == 'd') || (f.ToUpper().IndexOf("<DIR>") >= 0))
                        list.Add(f.Substring(f.ToUpper().IndexOf("<DIR>") + 5).Trim());
                }
            }

            return list;
        }

        /// <summary>
        /// 获取文件名列表（仅目录）
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>返回列表</returns>
        public ArrayList ListDir(string path)
        {
            if (ChangeDir(path))
            {
                ArrayList list = new ArrayList();

                foreach (string f in List("LIST"))
                {
                    if (f.Length > 0)
                    {
                        if ((f[0] == 'd') || (f.ToUpper().IndexOf("<DIR>") >= 0))
                            list.Add(f.Substring(f.ToUpper().IndexOf("<DIR>") + 5).Trim());
                    }
                }

                ChangeDir("");
                return list;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 获取原始数据信息.
        /// </summary>
        /// <param name="fileName">远程文件名</param>
        /// <returns>返回原始数据信息.</returns>
        public string GetFileDateRaw(string fileName)
        {
            Connect();

            SendCommand("MDTM " + fileName);
            ReadResponse();
            if (response != 213)
            {
                errormessage += responseStr;
                return "";
            }

            return (this.responseStr.Substring(4));
        }

        /// <summary>
        /// 得到文件日期.
        /// </summary>
        /// <param name="fileName">远程文件名</param>
        /// <returns>返回远程文件日期</returns>
        public DateTime GetFileDate(string fileName)
        {
            return ConvertFTPDateToDateTime(GetFileDateRaw(fileName));
        }

        /// <summary>
        /// FTP时间转换
        /// </summary>
        /// <param name="input">传输值</param>
        /// <returns></returns>
        private DateTime ConvertFTPDateToDateTime(string input)
        {
            if (input.Length < 14)
                throw new ArgumentException("Input Value for ConvertFTPDateToDateTime method was too short.");

            //YYYYMMDDhhmmss": 
            int year = Convert.ToInt16(input.Substring(0, 4));
            int month = Convert.ToInt16(input.Substring(4, 2));
            int day = Convert.ToInt16(input.Substring(6, 2));
            int hour = Convert.ToInt16(input.Substring(8, 2));
            int min = Convert.ToInt16(input.Substring(10, 2));
            int sec = Convert.ToInt16(input.Substring(12, 2));

            return new DateTime(year, month, day, hour, min, sec);
        }

        /// <summary>
        /// 获取FTP上的当前(工作)路径
        /// </summary>
        /// <returns>返回FTP上的当前(工作)路径</returns>
        public string GetWorkingDirectory()
        {
            //PWD - 显示工作路径
            Connect();
            SendCommand("PWD");
            ReadResponse();

            if (response != 257)
            {
                errormessage += responseStr;
            }

            string pwd;
            try
            {
                pwd = responseStr.Substring(responseStr.IndexOf("\"", 0) + 1);//5);
                pwd = pwd.Substring(0, pwd.LastIndexOf("\""));
                pwd = pwd.Replace("\"\"", "\""); // 替换带引号的路径信息符号
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //errormessage += ex.Message;
                //return null;
            }

            return pwd;
        }

        /// <summary>
        /// 跳转服务器上的当前(工作)路径
        /// </summary>
        /// <param name="path">要跳转的路径</param>
        public bool ChangeDir(string path)
        {
            Connect();
            SendCommand("CWD " + path);
            ReadResponse();
            if (response != 250)
            {
                errormessage += responseStr;
                return false;
            }
            return true;
        }

        /// <summary>
        /// 获得指定文件的大小(如果FTP支持)
        /// </summary>
        /// <param name="filename">指定的文件</param>
        /// <returns>返回指定文件的大小</returns>
        public long GetFileSize(string filename)
        {
            Connect();
            SendCommand("SIZE " + filename);
            ReadResponse();
            if (response != 213)
            {
                errormessage += responseStr;
            }

            return Int64.Parse(responseStr.Substring(4));
        }

        /// <summary>
        /// 上传文件(循环调用直到上传完毕)
        /// </summary>
        /// <returns>发送的字节数</returns>
        public long DoUpload()
        {
            Byte[] bytes = new Byte[512];
            long bytes_got;

            try
            {
                bytes_got = file.Read(bytes, 0, bytes.Length);
                bytes_total += bytes_got;
                data_sock.Send(bytes, (int)bytes_got, 0);

                if (bytes_got <= 0)
                {
                    //上传完毕或有错误发生
                    if (file != null)
                        file.Close();
                    file = null;

                    CloseDataSocket();
                    ReadResponse();
                    switch (response)
                    {
                        case 226:
                        case 250:
                            break;
                        default: //当上传中断时
                            {
                                errormessage += responseStr;
                                return -1;
                            }
                    }

                    SetBinaryMode(false);
                }
            }
            catch (Exception ex)
            {
                if (file != null)
                {
                    file.Close();
                    file = null;
                }

                CloseDataSocket();
                ReadResponse();
                SetBinaryMode(false);

                throw new Exception(ex.Message);
                //当上传中断时
                //errormessage += ex.Message;
                //return -1;
            }

            return bytes_got;
        }

        /// <summary>
        /// 下载文件(循环调用直到下载完毕)
        /// </summary>
        /// <returns>接收到的字节点</returns>
        public long DoDownload()
        {
            Byte[] bytes = new Byte[512];
            long bytes_got;

            try
            {
                bytes_got = data_sock.Receive(bytes, bytes.Length, 0);

                if (bytes_got <= 0)
                {
                    //下载完毕或有错误发生
                    CloseDataSocket();
                    if (file != null)
                        file.Close();
                    file = null;

                    ReadResponse();
                    switch (response)
                    {
                        case 226:
                        case 250:
                            break;
                        default:
                            {
                                errormessage += responseStr;
                                return -1;
                            }
                    }

                    SetBinaryMode(false);

                    return bytes_got;
                }

                file.Write(bytes, 0, (int)bytes_got);
                bytes_total += bytes_got;
            }
            catch (Exception ex)
            {
                CloseDataSocket();
                if (file != null)
                    file.Close();
                file = null;
                ReadResponse();
                SetBinaryMode(false);

                throw new Exception(ex.Message);
                //当下载中断时
                //errormessage += ex.Message;
                //return -1;
            }

            return bytes_got;
        }

        #endregion

        #region 复杂部分

        /// <summary>
        /// 判断是否存在文件或者目录
        /// </summary>
        /// <param name="path">路径</param>
        /// <returns>存在返回True,不存在返回False</returns>
        public bool IsExistsFiles(string path)
        {
            if (path.Substring(0, 1).ToString() != @"/")
            {
                path = @"/" + path;
            }

            if (path.Substring(path.Length - 1, 1) != @"/")
            {
                path = path + @"/";
            }

            ChangeDir(GetWorkingDirectory());

            path = path.Substring(1, path.Length - 1);

            if (path.Length == 0)
            {
                return true;
            }

            string strDir = path.Substring(0, path.IndexOf("/"));

            foreach (string item in ListFiles())
            {
                if (strDir == item)
                {
                    if (path.IndexOf("/") != path.LastIndexOf("/"))
                    {
                        ChangeDir(strDir);

                        if (!IsExistsFiles(path.Substring(path.IndexOf("/"))))
                        {
                            Reback();
                            return false;
                        }
                        else
                        {
                            Reback();
                            return true;
                        }
                    }
                    else
                    {
                        Reback();
                        return true;
                    }
                }
            }

            Reback();
            return false;
        }

        /// <summary>
        /// 返回
        /// </summary>
        public void Reback()
        {
            string strDirPath = GetWorkingDirectory();

            ChangeDir(strDirPath.Substring(0, strDirPath.ToUpper().LastIndexOf("/")));
        }

        /// <summary>
        /// 剪切
        /// </summary>
        /// <param name="inFilesName">输入路径</param>
        /// <param name="outFilesName">输出路径</param>
        public void Cut(string inFilesName, string outFilesName)
        {
            Copy(inFilesName, outFilesName);
            Delete(outFilesName);
        }

        /// <summary>
        /// 复制文件
        /// </summary>
        /// <param name="inFilesName">输入路径</param>
        /// <param name="outFilesName">输出路径</param>
        public void Copy(string inFilesName, string outFilesName)
        {
            Download(outFilesName, "C:\\temp" + outFilesName.Substring(outFilesName.LastIndexOf(".")));
            Upload("C:\\temp" + inFilesName.Substring(inFilesName.LastIndexOf(".")), inFilesName);

            File.Delete("C:\\temp" + inFilesName.Substring(inFilesName.LastIndexOf(".")));

        }

        /// <summary>
        /// 创建指定的目录
        /// </summary>
        /// <param name="dir">要创建的目录</param>
        public void MakeDir(string dir)
        {
            if (dir.Substring(0, 1).ToString() != @"/")
            {
                dir = @"/" + dir;
            }

            if (dir.Substring(dir.Length - 1, 1) != @"/")
            {
                dir = dir + @"/";
            }

            ChangeDir(GetWorkingDirectory());

            dir = dir.Substring(1, dir.Length - 1);

            if (dir.Length == 0)
            {
                return;
            }

            string strDir = dir.Substring(0, dir.IndexOf("/"));

            bool flag = false;

            foreach (string item in ListDir())
            {
                if (strDir == item)
                {
                    ChangeDir(strDir);
                    MakeDir(dir.Substring(dir.IndexOf("/")));
                    Reback();
                    flag = true;
                }
            }

            if (!flag)
            {
                Connect();
                SendCommand("MKD " + strDir);
                ReadResponse();

                switch (response)
                {
                    case 257:
                    case 250:
                        break;
                    default:
                        {
                            errormessage += responseStr;
                            break;
                        }
                }

                ChangeDir(strDir);
                MakeDir(dir.Substring(dir.IndexOf("/")));
                Reback();
            }
        }

        /// <summary>
        /// 删除文件或者文件夹
        /// </summary>
        /// <param name="path">路径</param>
        public void Delete(string path)
        {
            RemoveFile(path);

            if (errormessage.Length > 0)
            {
                RemoveRecursionDir(path);
            }
        }

        /// <summary>
        /// 重命名FTP上的文件
        /// </summary>
        /// <param name="oldfilename">原文件名</param>
        /// <param name="newfilename">新文件名</param>
        public void RenameFile(string oldfilename, string newfilename)
        {
            Connect();
            SendCommand("RNFR " + oldfilename);
            ReadResponse();
            if (response != 350)
            {
                errormessage += responseStr;
            }
            else
            {
                SendCommand("RNTO " + newfilename);
                ReadResponse();
                if (response != 250)
                {
                    errormessage += responseStr;
                }
            }
        }

        /// <summary>
        /// 下载指定文件
        /// </summary>
        /// <param name="filename">远程文件名称</param>
        /// <param name="localfilename">本地文件名</param>
        public void Download(string remote_filename, string localfilename)
        {
            Connect();

            if (!Directory.Exists(localfilename.Substring(0, localfilename.LastIndexOf("\\") + 1)))
            {
                Directory.CreateDirectory(localfilename.Substring(0, localfilename.LastIndexOf("\\") + 1));
            }

            OpenDownload(remote_filename, localfilename, false);

            while (DoDownload() > 0)
            {
                int perc = (int)(((BytesTotal) * 100) / FileSize);
            }

            Disconnect();
        }

        /// <summary>
        /// 上传指定的文件
        /// </summary>
        /// <param name="filename">本地文件名</param>
        /// <param name="remotefilename">远程要覆盖的文件名</param>
        public void Upload(string filename, string remotefilename)
        {
            Connect();
            OpenUpload(filename, remotefilename, false);

            while (DoUpload() > 0)
            {
                int perc = (int)(((BytesTotal) * 100) / FileSize);
            }

            Disconnect();
        }
        #endregion

        #region 辅助部分

        /// <summary>
        /// 递归删除文件夹
        /// </summary>
        /// <param name="dirPath">路径</param>
        public void RemoveRecursionDir(string dirPath)
        {
            ArrayList listFile = ListFile(dirPath);

            if (listFile != null)
            {
                foreach (string strFile in listFile)
                {
                    RemoveFile(dirPath + @"/" + strFile);
                }
            }

            ArrayList listDir = ListDir(dirPath);

            if (listDir != null)
            {
                foreach (string strDir in listDir)
                {
                    RemoveRecursionDir(dirPath + @"/" + strDir);
                }
            }

            RemoveDir(dirPath);
        }

        /// <summary>
        /// 移除FTP上的指定目录
        /// </summary>
        /// <param name="dir">要移除的目录</param>
        public void RemoveDir(string dir)
        {
            Connect();
            SendCommand("RMD " + dir);
            ReadResponse();
            if (response != 250)
            {
                errormessage += responseStr;
                return; ;
            }
        }

        /// <summary>
        /// 移除FTP上的指定文件
        /// </summary>
        /// <param name="filename">要移除的文件名称</param>
        public void RemoveFile(string filename)
        {
            Connect();
            SendCommand("DELE " + filename);
            ReadResponse();
            if (response != 250)
            {
                errormessage += responseStr;
            }
        }

        /// <summary>
        /// 上传指定的文件
        /// </summary>
        /// <param name="filename">本地文件名</param>
        /// <param name="remote_filename">远程要覆盖的文件名</param>
        /// <param name="resume">如果存在,则尝试恢复</param>
        public void OpenUpload(string filename, string remote_filename, bool resume)
        {
            MakeDir(remote_filename.Substring(0, remote_filename.LastIndexOf("/")));

            Connect();
            SetBinaryMode(true);
            OpenDataSocket();

            bytes_total = 0;

            try
            {
                file = new FileStream(filename, FileMode.Open);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
                //file = null;
                //errormessage += ex.Message;
                //return;
            }

            file_size = file.Length;

            if (resume)
            {
                long size = GetFileSize(remote_filename);
                SendCommand("REST " + size);
                ReadResponse();
                if (response == 350)
                    file.Seek(size, SeekOrigin.Begin);
            }

            if (remote_filename.Substring(0,1) != "/")
            {
                remote_filename = "/" + remote_filename;
            }

            SendCommand("STOR " + remote_filename);
            ReadResponse();

            switch (response)
            {
                case 125:
                case 150:
                    break;
                default:
                    if (file != null)
                        file.Close();
                    file = null;
                    errormessage += responseStr;
                    return;
            }

            ConnectDataSocket();

            return;
        }

        /// <summary>
        /// 打开并下载文件
        /// </summary>
        /// <param name="remote_filename">远程文件名称</param>
        /// <param name="local_filename">本地文件名</param>
        /// <param name="resume">如果文件存在则恢复</param>
        public void OpenDownload(string remote_filename, string local_filename, bool resume)
        {
            Connect();
            SetBinaryMode(true);

            bytes_total = 0;

            try
            {
                file_size = GetFileSize(remote_filename);
            }
            catch
            {
                file_size = 0;
            }

            if (resume && File.Exists(local_filename))
            {
                try
                {
                    file = new FileStream(local_filename, FileMode.Open);
                }
                catch (Exception ex)
                {
                    file = null;
                    throw new Exception(ex.Message);
                }

                SendCommand("REST " + file.Length);
                ReadResponse();
                if (response != 350)
                    throw new Exception(responseStr);
                file.Seek(file.Length, SeekOrigin.Begin);
                bytes_total = file.Length;
            }
            else
            {
                try
                {
                    file = new FileStream(local_filename, FileMode.Create);
                }
                catch (Exception ex)
                {
                    file = null;
                    throw new Exception(ex.Message);
                }
            }

            OpenDataSocket();
            SendCommand("RETR " + remote_filename);
            ReadResponse();

            switch (response)
            {
                case 125:
                case 150:
                    break;
                default:
                    if (file != null)
                        file.Close();
                    file = null;
                    errormessage += responseStr;
                    return;
            }
            ConnectDataSocket();

            return;
        }

        #endregion

        #endregion
    }
}
