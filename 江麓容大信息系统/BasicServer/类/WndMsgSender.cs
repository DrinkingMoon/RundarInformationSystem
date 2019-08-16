using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using PlatformManagement;

namespace ServerModule
{
    /// <summary>
    /// 要发信息数据结构，作为SendMessage函数的LParam参数
    /// </summary>
    public struct WndMsgData
    {
        /// <summary>
        /// 附加一些个人自定义标志信息
        /// </summary>
        public IntPtr Flag
        {
            get;
            set;
        }

        /// <summary>
        /// 发送的消息类型
        /// </summary>
        public MessageTypeEnum MessageType;

        /// <summary>
        /// 消息源(只针对消息类型为知会消息时可用)
        /// </summary>
        public NoticeSource NoticeSource;

        /// <summary>
        /// 要发送的字符串信息
        /// </summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string MessageContent;

        /// <summary>
        /// 要发送的对象信息的指针
        /// </summary>
        public IntPtr ObjectMessage;

        /// <summary>
        /// 要发送的对象信息进行二进制序列化后的字节数
        /// </summary>
        public int BytesOfObjectMessage;
    }

    /// <summary>
    /// 窗体消息发布器
    /// </summary>
    public class WndMsgSender
    {
        /// <summary>
        /// 这个变量用于保存要发送窗口的句柄
        /// </summary>
        private IntPtr SendToHandle;

        /// <summary>
        /// 自定义的消息
        /// </summary>
        private const int USER = 0x500;

        /// <summary>
        /// 关闭消息
        /// </summary>
        public const int CloseMsg = USER + 1;

        /// <summary>
        /// 定位消息(定位到指定记录)
        /// </summary>
        public const int PositioningMsg = USER + 2;

        /// <summary>
        /// 闪烁托盘图标消息
        /// </summary>
        public const int GlintMsg = USER + 3;

        /// <summary>
        /// 接收到新的流消息
        /// </summary>
        public const int NewFlowMsg = USER + 4;

        /// <summary>
        /// 告知某消息已经被处理
        /// </summary>
        public const int FinishedMsg = USER + 5;

        /// <summary>
        /// 显示特定的数据（告知接收方需要展示指定的数据)
        /// </summary>
        public const int ShowSpecificData = USER + 6;

        /// <summary>
        /// 系统预警消息
        /// </summary>
        public const int WarningNotice = USER + 7;

        /// <summary>
        /// 消息发送API
        /// </summary>
        /// <param name="hWnd">信息发住的窗口的句柄</param>
        /// <param name="Msg">消息ID</param>
        /// <param name="wParam">参数1</param>
        /// <param name="lParam">参数2</param>
        /// <returns></returns>
        [DllImport("User32.dll", EntryPoint = "SendMessage")]
        private static extern int _SendMessage(
            IntPtr hWnd,
            int Msg,
            int wParam,
            ref WndMsgData lParam
        );

        /// <summary>
        /// 给指定窗口发送消息
        /// </summary>
        /// <param name="sendToHandle">窗口句柄</param>
        /// <param name="msgId">消息ID</param>
        /// <param name="content">消息内容</param>
        public void SendMessage(IntPtr sendToHandle, int msgId, WndMsgData content)
        {
            SendToHandle = sendToHandle;
            _SendMessage(sendToHandle, msgId, 100, ref content);
        }
    }
}
