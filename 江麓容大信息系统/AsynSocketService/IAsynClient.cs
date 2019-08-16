using System;
using System.Net.Sockets;
using System.Net;
namespace AsynSocketService
{
    /// <summary>
    /// SOCKET异步通信客户端接口
    /// </summary>
    public interface IAsynClient : IDisposable
    {
        /// <summary>
        /// 与服务器进行连接的事件，通过该事件可以知道与服务器是否连接上
        /// </summary>
        event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// 接收事件
        /// </summary>
        event ReceiveEventHandler OnReceive;

        /// <summary>
        /// 获取是否连接上服务器的标志
        /// </summary>
        bool IsConnection { get; }

        /// <summary>
        /// 获取与服务器连接的通信端口
        /// </summary>
        int Port { get; }

        /// <summary>
        /// 获取服务器通信端口
        /// </summary>
        int ServerPort { get; }

        /// <summary>
        /// 获取客户端的SOCKET通讯对象
        /// </summary>
        Socket Client { get;}

        /// <summary>
        /// 获取客户端的IP地址
        /// </summary>
        IPAddress ClientIPAddress { get;}

        /// <summary>
        /// 获取客户端的IP地址
        /// </summary>
        string ClientIP { get;}

        /// <summary>
        /// 获取服务器的IP地址
        /// </summary>
        IPAddress ServerIP { get;}

        /// <summary>
        /// 连接指定IP地址的网络设备的对应端口
        /// </summary>
        /// <param name="ipAddress">网络设备的IP地址</param>
        /// <param name="Port">通信端口</param>
        void Connect(string ipAddress, int port);

        /// <summary>
        /// 发送数据到连接好的网络设备
        /// </summary>
        /// <param name="args">要发送的数据</param>
        void Send(CommEventArgs args);
    }
}
