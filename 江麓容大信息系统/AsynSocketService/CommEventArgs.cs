using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Xml.Serialization;
using SocketCommDefiniens;
using System.Windows.Forms;

namespace AsynSocketService
{
    /// <summary>
    /// 参数类
    /// </summary>
    public class ParamStruct
    {
        /// <summary>
        /// 命令字
        /// </summary>
        private CommCMD m_cmd;

        /// <summary>
        /// 名称代码
        /// </summary>
        private TagCode m_code;

        /// <summary>
        /// 值
        /// </summary>
        private object m_value;

        /// <summary>
        /// 命令字
        /// </summary>
        [XmlElement("CMD")]
        public CommCMD CMD
        {
            get { return m_cmd; }
            set { m_cmd = value; }
        }

        /// <summary>
        /// 名称代码
        /// </summary>
        [XmlElement("Code")]
        public TagCode Code
        {
            get { return m_code; }
            set { m_code = value; }
        }

        /// <summary>
        /// 数据值
        /// </summary>
        [XmlElement("DataValue")]
        public object DataValue
        {
            get { return m_value; }
            set { m_value = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ParamStruct()
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="cmd">命令字</param>
        /// <param name="code">名称代码</param>
        /// <param name="dataValue">值</param>
        public ParamStruct(CommCMD cmd, TagCode code, object dataValue)
        {
            m_cmd = cmd;
            m_code = code;
            m_value = dataValue;
        }
    }

    /// <summary>
    /// 通讯参数ID生成器
    /// </summary>
    public sealed class CommIdGenerator
    {
        /// <summary>
        /// 通讯参数源ID
        /// </summary>
        private static long m_id;

        /// <summary>
        /// 获取或设置通讯参数ID(由服务器指定)
        /// </summary>
        static public long Id
        {
            get
            {
                if (m_beginValue == 0)
                {
                    IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
                    
                    byte[] ipBytes = hostEntry.AddressList[0].GetAddressBytes();
                    
                    long ip = ipBytes[0] * ipBytes[1] * ipBytes[2] * ipBytes[3];

                    m_beginValue = DateTime.Now.ToFileTime() % 10000000 + ip * 10000000;
                }

                return (m_beginValue + m_id++);
            }
        }

        static long m_beginValue;

        /// <summary>
        /// 获取或设置ID起始值
        /// </summary>
        static public long BeginValue
        {
            get { return m_beginValue; }
            set { m_beginValue = value; }
        }
    }

    /// <summary>
    /// 通信参数
    /// </summary>
    [XmlRoot("Root")]
    public class CommEventArgs
    {
        /// <summary>
        /// 通讯参数ID
        /// </summary>
        private long m_id;

        /// <summary>
        /// 获取或设置通讯参数ID
        /// </summary>
        [XmlElement("Id")]
        public long Id
        {
            get { return m_id; }
            set { m_id = value; }
        }

        /// <summary>
        /// 发送该通讯参数的源地址
        /// </summary>
        private string m_sourceAddress;

        /// <summary>
        /// 获取或设置发送该通讯参数的源地址
        /// </summary>
        [XmlElement("SourceAddress")]
        public string SourceAddress
        {
            get { return m_sourceAddress; }
            set { m_sourceAddress = value; }
        }

        /// <summary>
        /// 接收该通讯参数的目标地址
        /// </summary>
        private string m_targetAddress;

        /// <summary>
        /// 获取或设置接收该通讯参数的目标地址
        /// </summary>
        [XmlElement("TargetAddress")]
        public string TargetAddress
        {
            get { return m_targetAddress; }
            set { m_targetAddress = value; }
        }

        /// <summary>
        /// 参数集合
        /// </summary>
        private List<ParamStruct> m_params = new List<ParamStruct>();

        /// <summary>
        /// 参数集合
        /// </summary>
        [XmlElement("Params")]
        public List<ParamStruct> Params
        {
            get { return m_params; }
            set { m_params = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public CommEventArgs()
        {
            m_id = CommIdGenerator.Id;
        }
    }
}
