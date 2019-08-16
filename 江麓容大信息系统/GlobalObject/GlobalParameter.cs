/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  GlobalParameter.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/06/22
 * 开发平台:  Visual C# 2005
 * 用于    :  仓库管理软件
 *----------------------------------------------------------------------------
 * 描述 : 获取整个解决方案中一些公共的信息
 * 其它 :
 *----------------------------------------------------------------------------
 * 公共信息: 参见系统'类帮助文档'
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2010/06/22 8:54:12 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using System.IO;
using System.Management;

namespace GlobalObject
{
    /// <summary>
    /// 整个项目公共参数
    /// </summary>
    public sealed class GlobalParameter
    {
        /// <summary>
        /// 将构造函数私有化
        /// </summary>
        private GlobalParameter() { }

        /// <summary>
        /// 从XML文件中读取配置参数及保存参数到配置文件的类
        /// </summary>
        private static XmlParams m_xmlParams;

        /// <summary>
        /// 获取XML配置文件参数操作类
        /// </summary>
        public static XmlParams XmlParameter
        {
            get { return m_xmlParams; }
        }

        /// <summary>
        /// 参数字典
        /// </summary>
        private static Dictionary<string, Object> m_dicParams;

        /// <summary>
        /// SOCKET异步通信时的序列化类型表
        /// </summary>
        private static List<Type> m_lstSerializerType = new List<Type>();

        /// <summary>
        /// 获取或设置SOCKET异步通信时的序列化类型表
        /// </summary>
        public static List<Type> SerializerTypes
        {
            get { return GlobalParameter.m_lstSerializerType; }
            set { GlobalParameter.m_lstSerializerType = value; }
        }

        static string m_LoginNotice;

        public static string LoginNotice
        {
            get { return GlobalParameter.m_LoginNotice; }
            set { GlobalParameter.m_LoginNotice = value; }
        }

        /// <summary>
        /// 当前数据库服务器
        /// </summary>
        private static string m_dataServer;

        /// <summary>
        /// 获取或设置当前数据库服务器
        /// </summary>
        public static string DataServer
        {
            get { return GlobalParameter.m_dataServer; }
            set { GlobalParameter.m_dataServer = value; }
        }

        /// <summary>
        /// 当前数据库服务器IP地址
        /// </summary>
        private static string m_dataServerIP;

        /// <summary>
        /// 获取或设置当前数据库服务器IP地址
        /// </summary>
        public static string DataServerIP
        {
            get { return GlobalParameter.m_dataServerIP; }
            set { GlobalParameter.m_dataServerIP = value; }
        }

        /// <summary>
        /// 服务器端口
        /// </summary>
        private static int m_serverPort;

        /// <summary>
        /// 获取服务器端口
        /// </summary>
        public static int ServerPort
        {
            get { return GlobalParameter.m_serverPort; }
        }

        /// <summary>
        /// 获取登录用户编码
        /// </summary>
        private static string m_userCode;

        /// <summary>
        /// 获取或设置登录用户编码
        /// </summary>
        public static string UserCode
        {
            get { return GlobalParameter.m_userCode; }
            set { GlobalParameter.m_userCode = value; }
        }

        /// <summary>
        /// 平台服务数据库连接字串
        /// </summary>
        private static string m_dbPlatformServiceConnectionString;

        /// <summary>
        /// 获取平台服务数据库连接字符串
        /// </summary>
        public static string PlatformServiceConnectionString
        {
            get { return GlobalParameter.m_dbPlatformServiceConnectionString; }
        }

        /// <summary>
        /// 仓库业务系统数据库连接字串
        /// </summary>
        private static string m_dbStorehouseConnectionString;

        /// <summary>
        /// 获取仓库业务系统数据库连接字符串
        /// </summary>
        public static string StorehouseConnectionString
        {
            get { return GlobalParameter.m_dbStorehouseConnectionString; }
        }

        /// <summary>
        /// WEB业务系统数据库连接字串
        /// </summary>
        private static string m_dbWebServerConnectionString;

        /// <summary>
        /// 获取WEB业务系统数据库连接字符串
        /// </summary>
        public static string WebServerConnectionString
        {
            get { return GlobalParameter.m_dbWebServerConnectionString; }
        }

        /// <summary>
        /// 任务管理业务系统数据库连接字串
        /// </summary>
        private static string m_dbTaskConnectionString;

        /// <summary>
        /// 获取任务管理业务系统数据库连接字符串
        /// </summary>
        public static string TaskConnectionString
        {
            get { return GlobalParameter.m_dbTaskConnectionString; }
        }

        /// <summary>
        /// 临时文件夹路径
        /// </summary>
        private static string m_FileTempPath;

        /// <summary>
        /// 临时文件夹路径
        /// </summary>
        public static string FileTempPath
        {
            get { return GlobalParameter.m_FileTempPath; }
        }

        #region FTP信息

        /// <summary>
        /// 当前FTP服务器IP地址
        /// </summary>
        private static string m_FTPServerIP;

        /// <summary>
        /// 获取或设置当前FTP服务器IP地址
        /// </summary>
        public static string FTPServerIP
        {
            get { return GlobalParameter.m_FTPServerIP; }
            set { GlobalParameter.m_FTPServerIP = value; }
        }

        /// <summary>
        /// 当前FTP服务器端口
        /// </summary>
        private static int m_FTPServerPort;

        /// <summary>
        /// 获取或设置当前FTP服务器端口
        /// </summary>
        public static int FTPServerPort
        {
            get { return GlobalParameter.m_FTPServerPort; }
            set { GlobalParameter.m_FTPServerPort = value; }
        }

        /// <summary>
        /// 当前FTP服务器高级用户
        /// </summary>
        private static string m_FTPServerAdvancedUser;

        /// <summary>
        /// 获取或设置当前FTP服务器高级用户
        /// </summary>
        public static string FTPServerAdvancedUser
        {
            get { return GlobalParameter.m_FTPServerAdvancedUser; }
            set { GlobalParameter.m_FTPServerAdvancedUser = value; }
        }

        /// <summary>
        /// 当前FTP服务器高级用户密码
        /// </summary>
        private static string m_FTPServerAdvancedPassword;

        /// <summary>
        /// 获取或设置当前FTP服务器高级用户密码
        /// </summary>
        public static string FTPServerAdvancedPassword
        {
            get { return GlobalParameter.m_FTPServerAdvancedPassword; }
            set { GlobalParameter.m_FTPServerAdvancedPassword = value; }
        }

        #endregion

        /// <summary>
        /// 只显示有权限的功能树节点的标志
        /// </summary>
        private static bool m_onlyShowAuthorizedNodes = false;

        /// <summary>
        /// 获取及设置是否只显示有权限的功能树节点
        /// </summary>
        public static bool OnlyShowAuthorizedNodes
        {
            get { return m_onlyShowAuthorizedNodes; }
            set { m_onlyShowAuthorizedNodes = value; }
        }

        static CE_SystemName _SystemName;

        public static CE_SystemName SystemName
        {
            get { return GlobalParameter._SystemName; }
            set { GlobalParameter._SystemName = value; }
        }

        /// <summary>
        /// 获取程序运行路径，最后不包含"\"
        /// </summary>
        /// <returns>程序运行路径</returns>
        public static String GetAppRunPath()
        {
            return Environment.CurrentDirectory;
        }

        public static string ConvertIPAddress(string ipAddress)
        {
            string result = null;

            if (!ipAddress.Contains("."))
            {
                return null;
            }

            List<string> lstIp = ipAddress.Split('.').ToList();

            foreach (string str in lstIp)
            {
                result += Convert.ToInt32(str).ToString() + ".";
            }

            return result.Substring(0, result.Length - 1);
        }

        public static void Init()
        {
            if (System.Net.Dns.GetHostName() == "OD00308" || System.Net.Dns.GetHostName() == "DESKTOP-GPAUBOV")
            {
                m_xmlParams = new XmlParams(GetAppRunPath() + "\\AdminSystemParams.xml");
            }
            else
            {
                m_xmlParams = new XmlParams(GetAppRunPath() + "\\SystemParams.xml");
            }

            m_dicParams = m_xmlParams.GetParams();

            Dictionary<string, string> paramItem = m_dicParams["系统参数"] as Dictionary<string, string>;

            m_userCode = paramItem["用户编码"];

            if (paramItem.ContainsKey("公告提示"))
            {
                m_LoginNotice = paramItem["公告提示"].ToString();
            }

            if (paramItem.ContainsKey("只显示有权限的功能树节点"))
            {
                m_onlyShowAuthorizedNodes = Convert.ToBoolean(paramItem["只显示有权限的功能树节点"]);
            }

            if (paramItem.ContainsKey("系统平台"))
            {
                _SystemName = GeneralFunction.StringConvertToEnum<CE_SystemName>(paramItem["系统平台"].ToString());
            }
        }

        /// <summary>
        /// 初始化公有参数类
        /// </summary>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功初始化返回true，失败返回false</returns>
        public static void Init(string selectValue)
        {
            if (System.Net.Dns.GetHostName() == "OD00308" || System.Net.Dns.GetHostName() == "DESKTOP-GPAUBOV")
            {
                m_xmlParams = new XmlParams(GetAppRunPath() + "\\AdminSystemParams.xml");
            }
            else
            {
                m_xmlParams = new XmlParams(GetAppRunPath() + "\\SystemParams.xml");
            }

            m_dicParams = m_xmlParams.GetParams();

            Dictionary<string, string> paramItem = m_dicParams["系统参数"] as Dictionary<string, string>;

            m_serverPort = Int32.Parse(paramItem["服务器端口"]);
            List<string> lstIP = paramItem["服务器IP地址"].ToString().Split('|').ToList();
            _SystemName = GlobalObject.GeneralFunction.StringConvertToEnum<CE_SystemName>(selectValue);
            switch (_SystemName)
            {
                case CE_SystemName.湖南容大:
                    m_dataServerIP = lstIP[0];
                    break;
                case CE_SystemName.泸州容大:
                    m_dataServerIP = lstIP[1];
                    break;
                default:
                    m_dataServerIP = lstIP[0];
                    break;
            }


            //通过IP地址连接,必需确保SQL服务器开启1433端口和检查SQL网络连接启用TCP/IP协议
            string serverInfo = string.Format("Data Source={0};Connect Timeout=120;Network Library=DBMSSOCN;MultipleActiveResultSets=true;Initial Catalog=", m_dataServerIP);

            string pwd = ";User ID=InfoSysUser;PWD=meimima123";
            string webPwd = ";User ID=RundarWebUser;PWD=@HelloRundar!123";

            m_dbStorehouseConnectionString = string.Format("{0}{1}{2}", serverInfo, "DepotManagement", pwd);
            m_dbPlatformServiceConnectionString = string.Format("{0}{1}{2}", serverInfo, "PlatformService", pwd);
            m_dbTaskConnectionString = string.Format("{0}{1}{2}", serverInfo, "TaskManagement", pwd);
            m_dbWebServerConnectionString = string.Format("{0}{1}{2}", serverInfo, "RundarWebServer", webPwd);

            m_FileTempPath = System.Environment.GetEnvironmentVariable("TEMP");

            if (m_dataServerIP.IndexOf(',') < 0)
            {
                m_FTPServerIP = m_dataServerIP;
            }
            else
            {
                m_FTPServerIP = m_dataServerIP.Substring(0, m_dataServerIP.IndexOf(','));
            }

            m_FTPServerPort = 21;
            m_FTPServerAdvancedUser = "AdvUser_QualityFile";
            m_FTPServerAdvancedPassword = "~!_qweASD98";
        }

        #region 升级引导程序
        ///// <summary>
        ///// 升级引导程序
        ///// </summary>
        //public static void UpgradeRoot()
        //{
        //    // 必须延时，保证原进程退出
        //    System.Threading.Thread.Sleep(2000);

        //    System.Diagnostics.Process[] myProcesses = System.Diagnostics.Process.GetProcesses();

        //    foreach (System.Diagnostics.Process myProcess in myProcesses)
        //    {
        //        if ("AutoUpgradeSystem" == myProcess.ProcessName)
        //        {
        //            myProcess.Kill();
        //            break;
        //        }
        //    }

        //    System.Threading.Thread.Sleep(2000);

        //    string path = GetAppRunPath();
        //    string surFile = string.Format(@"{0}\TempUpgradeSystem.exe", path);
        //    string desFile = string.Format(@"{0}\AutoUpgradeSystem.exe", path);

        //    if (File.Exists(surFile))
        //    {
        //        if (File.GetLastWriteTime(surFile) > File.GetLastWriteTime(desFile))
        //        {
        //            File.Copy(surFile, desFile, true);
        //        }
        //    }
        //} 
        #endregion

        /// <summary>
        /// 保存信息到配置文件
        /// </summary>
        public static void Save()
        {
            Dictionary<string, string> paramItem = m_dicParams["系统参数"] as Dictionary<string, string>;
            bool updateFlag = false;

            if (paramItem["用户编码"] != UserCode)
            {
                updateFlag = true;
                paramItem["用户编码"] = UserCode;
            }

            if (!paramItem.ContainsKey("公告提示")
                || paramItem["公告提示"] != m_LoginNotice.ToString())
            {
                updateFlag = true;
                paramItem["公告提示"] = m_LoginNotice.ToString();
            }

            if (!paramItem.ContainsKey("只显示有权限的功能树节点") || 
                paramItem["只显示有权限的功能树节点"] != OnlyShowAuthorizedNodes.ToString())
            {
                updateFlag = true;
                paramItem["只显示有权限的功能树节点"] = OnlyShowAuthorizedNodes.ToString();
            }

            if (paramItem["系统平台"] != _SystemName.ToString())
            {
                updateFlag = true;
                paramItem["系统平台"] = _SystemName.ToString();
            }

            if (updateFlag)
                m_xmlParams.SaveParams(m_dicParams, false);
        }
    }
}
