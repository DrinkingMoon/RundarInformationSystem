/****************************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 * 
 * 文件名称:   StateObject.cs
 * 
 * 作者    :   Dennis
 * 
 * 版本:       V1.0.609.1
 * 
 * 创建日期:   2009-06-09
 * 
 * 开发平台:   vs2005(c#)
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;

namespace AsynSocketService
{
    /// <summary>
    /// 异步通信时的状态对象
    /// </summary>
    internal class StateObject
    {
        /// <summary>
        /// 套接字
        /// </summary>
        internal Socket WorkSocket
        {
            get
            {
                return m_workSocket;
            }
            set
            {
                m_workSocket = value;
            }
        }

        /// <summary>
        /// 通信Socket
        /// </summary>
        private Socket m_workSocket;

        /// <summary>
        /// 接收缓冲区大小
        /// </summary>
        internal const int BufferSize = 102400;

        /// <summary>
        /// 缓冲区
        /// </summary>
        internal byte[] Buffer
        {
            get
            {
                return m_buffer;
            }
            set
            {
                m_buffer = value;
            }
        }

        /// <summary>
        /// 结束帧
        /// </summary>
        internal static byte[] END = new byte[] { 0xEE, 0x77 };

        /// <summary>
        /// 接收缓冲区
        /// </summary>
        private byte[] m_buffer = new byte[BufferSize];

        /// <summary>
        /// 完整数据
        /// </summary>
        private byte[] m_Receives;

        /// <summary>
        /// 完整数据
        /// </summary>
        internal byte[] FullData
        {
            get
            {
                return m_Receives;
            }
        }

        public void ClearData()
        {
            m_Receives = null;
        }

        /// <summary>
        /// 保存完成整数
        /// </summary>
        /// <param name="len">长度</param>
        public void Save(int len)
        {
            if (m_Receives == null)
            {
                m_Receives = new byte[len];
            }
            else
            {
                byte[] buffer = new byte[m_Receives.Length + len];
                Array.Copy(m_Receives, buffer, m_Receives.Length);
                m_Receives = buffer;
            }

            Array.Copy(m_buffer, 0, m_Receives, m_Receives.Length - len, len);
        }

    }
}
