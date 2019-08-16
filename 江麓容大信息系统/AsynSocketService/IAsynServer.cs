using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace AsynSocketService
{

    /// <summary>
    /// 接收委托
    /// </summary>
    /// <param name="sender">目标发送</param>
    /// <param name="args">通信参数</param>
    public delegate void ReceiveEventHandler(object sender, CommEventArgs args);

    /// <summary>
    /// 异步SOCKET服务器接口
    /// </summary>
    public interface IAsynServer : IDisposable
    {
        /// <summary>
        /// 获取IP地址
        /// </summary>
        IPAddress IP
        {
            get;
        }

        /// <summary>
        /// 客户端连接到服务器后触发的事件
        /// </summary>
        event GlobalObject.DelegateCollection.SocketConnectEvent OnConnected;

        /// <summary>
        /// 接收事件
        /// </summary>
        event ReceiveEventHandler OnReceive;

        /// <summary>
        /// 开始监听指定网络接口Ip的指定端口
        /// </summary>
        void Begin();

        /// <summary>
        /// 关闭连接
        /// </summary>
        void Close();

        /// <summary>
        /// 发送信息
        /// </summary>
        /// <param name="args">通信参数</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>操作是否成功的标志</returns>
        bool Send(CommEventArgs args, out string error);
    }
}
