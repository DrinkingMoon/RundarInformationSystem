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
using System.Data;
using System.Windows.Forms;
using System.Management;

namespace AutoUpgradeSystem
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
        /// 获取程序运行路径，最后不包含"\"
        /// </summary>
        /// <returns>程序运行路径</returns>
        public static String GetAppRunPath()
        {
            return Environment.CurrentDirectory;
        }

        /// <summary>
        /// 初始化公有参数类
        /// </summary>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>成功初始化返回true，失败返回false</returns>
        public static bool Init(out string error)
        {
            error = null;

            m_xmlParams = new XmlParams(GetAppRunPath() + "\\SystemParams.xml");

            m_dicParams = m_xmlParams.GetParams();

            Dictionary<string, string> paramItem = m_dicParams["系统参数"] as Dictionary<string, string>;

            m_dataServerIP = paramItem["数据库服务器地址"];

            string serverInfo = string.Format("Data Source={0},1433;Network Library=DBMSSOCN;Initial Catalog=", m_dataServerIP);
            string pwd = ";User ID=InfoSysUser;PWD=meimima123";

            m_dbPlatformServiceConnectionString = string.Format("{0}{1}{2}", serverInfo, "PlatformService", pwd);

            string[] ips = m_dataServerIP.Split(new char[] { '.' });

            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");

            ManagementObjectCollection moc = mc.GetInstances();

            List<string> lstAddress = new List<string>();

            Dictionary<string, string> dicIp = new Dictionary<string, string>();

            foreach (ManagementObject mo in moc)
            {
                string[] ip = (string[])mo["IPAddress"];

                if (ip != null && ip.Length > 0)
                {
                    lstAddress.Add(string.Format("{0}.{1}.{2}.{3}", ips[0], ips[1], ips[2], ips[3]));
                }
            }

            if (lstAddress.Count == 0)
            {
                error = "您没有可用的网络连接，请检查网络是否正常";
                return false;
            }

            if (lstAddress.Count > 1)
            {
                for (int i = 0; i < lstAddress.Count; i++)
                {
                    if (!TestDBServer(lstAddress[i]))
                    {
                        lstAddress.RemoveAt(i--);
                    }

                    // 至少留一个IP地址
                    if (lstAddress.Count == 1)
                    {
                        break;
                    }
                }
            }

            if (m_dataServerIP != lstAddress[0])
            {
                m_dataServerIP = lstAddress[0];

                serverInfo = string.Format("Data Source={0},1433;Network Library=DBMSSOCN;Initial Catalog=", m_dataServerIP);

                m_dbPlatformServiceConnectionString = string.Format("{0}{1}{2}", serverInfo, "PlatformService", pwd);

                Save();
            }

            return true;
        }

        /// <summary>
        /// 测试指定IP连接数据库服务器是否成功
        /// </summary>
        /// <param name="ip">要测试的IP地址</param>
        /// <returns>连接成功返回true, 失败返回false</returns>
        public static bool TestDBServer(string ip)
        {
            string connString = PlatformServiceConnectionString;
            int startIndex = connString.IndexOf('=') + 1;
            int endIndex = connString.IndexOf(',');

            connString = connString.Replace(connString.Substring(startIndex, endIndex - startIndex), ip);

            try
            {
                using (System.Data.SqlClient.SqlConnection sqlConn = new System.Data.SqlClient.SqlConnection(connString))
                {
                    sqlConn.Open();

                    return true;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 保存信息到配置文件
        /// </summary>
        public static void Save()
        {
            Dictionary<string, string> paramItem = m_dicParams["系统参数"] as Dictionary<string, string>;

            m_dbPlatformServiceConnectionString = m_dbPlatformServiceConnectionString.Replace(paramItem["数据库服务器地址"], m_dataServerIP);

            paramItem["数据库服务器地址"] = m_dataServerIP;

            m_xmlParams.SaveParams(m_dicParams, false);
        }
    }
}
