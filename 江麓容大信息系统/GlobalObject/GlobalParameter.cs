/******************************************************************************
 * ��Ȩ���� (c) 2006-2010, С����ҵ�����ݴ��������ι�˾
 *
 * �ļ�����:  GlobalParameter.cs
 * ����    :  ��ʯ��    �汾: v1.00    ����: 2010/06/22
 * ����ƽ̨:  Visual C# 2005
 * ����    :  �ֿ�������
 *----------------------------------------------------------------------------
 * ���� : ��ȡ�������������һЩ��������Ϣ
 * ���� :
 *----------------------------------------------------------------------------
 * ������Ϣ: �μ�ϵͳ'������ĵ�'
 *----------------------------------------------------------------------------
 * ��ʷ��¼:
 *     1. ����ʱ��: 2010/06/22 8:54:12 ����: ��ʯ�� ��ǰ�汾: V1.00
 *        �޸�˵��: ����
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
    /// ������Ŀ��������
    /// </summary>
    public sealed class GlobalParameter
    {
        /// <summary>
        /// �����캯��˽�л�
        /// </summary>
        private GlobalParameter() { }

        /// <summary>
        /// ��XML�ļ��ж�ȡ���ò�������������������ļ�����
        /// </summary>
        private static XmlParams m_xmlParams;

        /// <summary>
        /// ��ȡXML�����ļ�����������
        /// </summary>
        public static XmlParams XmlParameter
        {
            get { return m_xmlParams; }
        }

        /// <summary>
        /// �����ֵ�
        /// </summary>
        private static Dictionary<string, Object> m_dicParams;

        /// <summary>
        /// SOCKET�첽ͨ��ʱ�����л����ͱ�
        /// </summary>
        private static List<Type> m_lstSerializerType = new List<Type>();

        /// <summary>
        /// ��ȡ������SOCKET�첽ͨ��ʱ�����л����ͱ�
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
        /// ��ǰ���ݿ������
        /// </summary>
        private static string m_dataServer;

        /// <summary>
        /// ��ȡ�����õ�ǰ���ݿ������
        /// </summary>
        public static string DataServer
        {
            get { return GlobalParameter.m_dataServer; }
            set { GlobalParameter.m_dataServer = value; }
        }

        /// <summary>
        /// ��ǰ���ݿ������IP��ַ
        /// </summary>
        private static string m_dataServerIP;

        /// <summary>
        /// ��ȡ�����õ�ǰ���ݿ������IP��ַ
        /// </summary>
        public static string DataServerIP
        {
            get { return GlobalParameter.m_dataServerIP; }
            set { GlobalParameter.m_dataServerIP = value; }
        }

        /// <summary>
        /// �������˿�
        /// </summary>
        private static int m_serverPort;

        /// <summary>
        /// ��ȡ�������˿�
        /// </summary>
        public static int ServerPort
        {
            get { return GlobalParameter.m_serverPort; }
        }

        /// <summary>
        /// ��ȡ��¼�û�����
        /// </summary>
        private static string m_userCode;

        /// <summary>
        /// ��ȡ�����õ�¼�û�����
        /// </summary>
        public static string UserCode
        {
            get { return GlobalParameter.m_userCode; }
            set { GlobalParameter.m_userCode = value; }
        }

        /// <summary>
        /// ƽ̨�������ݿ������ִ�
        /// </summary>
        private static string m_dbPlatformServiceConnectionString;

        /// <summary>
        /// ��ȡƽ̨�������ݿ������ַ���
        /// </summary>
        public static string PlatformServiceConnectionString
        {
            get { return GlobalParameter.m_dbPlatformServiceConnectionString; }
        }

        /// <summary>
        /// �ֿ�ҵ��ϵͳ���ݿ������ִ�
        /// </summary>
        private static string m_dbStorehouseConnectionString;

        /// <summary>
        /// ��ȡ�ֿ�ҵ��ϵͳ���ݿ������ַ���
        /// </summary>
        public static string StorehouseConnectionString
        {
            get { return GlobalParameter.m_dbStorehouseConnectionString; }
        }

        /// <summary>
        /// WEBҵ��ϵͳ���ݿ������ִ�
        /// </summary>
        private static string m_dbWebServerConnectionString;

        /// <summary>
        /// ��ȡWEBҵ��ϵͳ���ݿ������ַ���
        /// </summary>
        public static string WebServerConnectionString
        {
            get { return GlobalParameter.m_dbWebServerConnectionString; }
        }

        /// <summary>
        /// �������ҵ��ϵͳ���ݿ������ִ�
        /// </summary>
        private static string m_dbTaskConnectionString;

        /// <summary>
        /// ��ȡ�������ҵ��ϵͳ���ݿ������ַ���
        /// </summary>
        public static string TaskConnectionString
        {
            get { return GlobalParameter.m_dbTaskConnectionString; }
        }

        /// <summary>
        /// ��ʱ�ļ���·��
        /// </summary>
        private static string m_FileTempPath;

        /// <summary>
        /// ��ʱ�ļ���·��
        /// </summary>
        public static string FileTempPath
        {
            get { return GlobalParameter.m_FileTempPath; }
        }

        #region FTP��Ϣ

        /// <summary>
        /// ��ǰFTP������IP��ַ
        /// </summary>
        private static string m_FTPServerIP;

        /// <summary>
        /// ��ȡ�����õ�ǰFTP������IP��ַ
        /// </summary>
        public static string FTPServerIP
        {
            get { return GlobalParameter.m_FTPServerIP; }
            set { GlobalParameter.m_FTPServerIP = value; }
        }

        /// <summary>
        /// ��ǰFTP�������˿�
        /// </summary>
        private static int m_FTPServerPort;

        /// <summary>
        /// ��ȡ�����õ�ǰFTP�������˿�
        /// </summary>
        public static int FTPServerPort
        {
            get { return GlobalParameter.m_FTPServerPort; }
            set { GlobalParameter.m_FTPServerPort = value; }
        }

        /// <summary>
        /// ��ǰFTP�������߼��û�
        /// </summary>
        private static string m_FTPServerAdvancedUser;

        /// <summary>
        /// ��ȡ�����õ�ǰFTP�������߼��û�
        /// </summary>
        public static string FTPServerAdvancedUser
        {
            get { return GlobalParameter.m_FTPServerAdvancedUser; }
            set { GlobalParameter.m_FTPServerAdvancedUser = value; }
        }

        /// <summary>
        /// ��ǰFTP�������߼��û�����
        /// </summary>
        private static string m_FTPServerAdvancedPassword;

        /// <summary>
        /// ��ȡ�����õ�ǰFTP�������߼��û�����
        /// </summary>
        public static string FTPServerAdvancedPassword
        {
            get { return GlobalParameter.m_FTPServerAdvancedPassword; }
            set { GlobalParameter.m_FTPServerAdvancedPassword = value; }
        }

        #endregion

        /// <summary>
        /// ֻ��ʾ��Ȩ�޵Ĺ������ڵ�ı�־
        /// </summary>
        private static bool m_onlyShowAuthorizedNodes = false;

        /// <summary>
        /// ��ȡ�������Ƿ�ֻ��ʾ��Ȩ�޵Ĺ������ڵ�
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
        /// ��ȡ��������·������󲻰���"\"
        /// </summary>
        /// <returns>��������·��</returns>
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

            Dictionary<string, string> paramItem = m_dicParams["ϵͳ����"] as Dictionary<string, string>;

            m_userCode = paramItem["�û�����"];

            if (paramItem.ContainsKey("������ʾ"))
            {
                m_LoginNotice = paramItem["������ʾ"].ToString();
            }

            if (paramItem.ContainsKey("ֻ��ʾ��Ȩ�޵Ĺ������ڵ�"))
            {
                m_onlyShowAuthorizedNodes = Convert.ToBoolean(paramItem["ֻ��ʾ��Ȩ�޵Ĺ������ڵ�"]);
            }

            if (paramItem.ContainsKey("ϵͳƽ̨"))
            {
                _SystemName = GeneralFunction.StringConvertToEnum<CE_SystemName>(paramItem["ϵͳƽ̨"].ToString());
            }
        }

        /// <summary>
        /// ��ʼ�����в�����
        /// </summary>
        /// <param name="error">����ʱ���ش�����Ϣ���޴�ʱ����null</param>
        /// <returns>�ɹ���ʼ������true��ʧ�ܷ���false</returns>
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

            Dictionary<string, string> paramItem = m_dicParams["ϵͳ����"] as Dictionary<string, string>;

            m_serverPort = Int32.Parse(paramItem["�������˿�"]);
            List<string> lstIP = paramItem["������IP��ַ"].ToString().Split('|').ToList();
            _SystemName = GlobalObject.GeneralFunction.StringConvertToEnum<CE_SystemName>(selectValue);
            switch (_SystemName)
            {
                case CE_SystemName.�����ݴ�:
                    m_dataServerIP = lstIP[0];
                    break;
                case CE_SystemName.�����ݴ�:
                    m_dataServerIP = lstIP[1];
                    break;
                default:
                    m_dataServerIP = lstIP[0];
                    break;
            }


            //ͨ��IP��ַ����,����ȷ��SQL����������1433�˿ںͼ��SQL������������TCP/IPЭ��
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

        #region ������������
        ///// <summary>
        ///// ������������
        ///// </summary>
        //public static void UpgradeRoot()
        //{
        //    // ������ʱ����֤ԭ�����˳�
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
        /// ������Ϣ�������ļ�
        /// </summary>
        public static void Save()
        {
            Dictionary<string, string> paramItem = m_dicParams["ϵͳ����"] as Dictionary<string, string>;
            bool updateFlag = false;

            if (paramItem["�û�����"] != UserCode)
            {
                updateFlag = true;
                paramItem["�û�����"] = UserCode;
            }

            if (!paramItem.ContainsKey("������ʾ")
                || paramItem["������ʾ"] != m_LoginNotice.ToString())
            {
                updateFlag = true;
                paramItem["������ʾ"] = m_LoginNotice.ToString();
            }

            if (!paramItem.ContainsKey("ֻ��ʾ��Ȩ�޵Ĺ������ڵ�") || 
                paramItem["ֻ��ʾ��Ȩ�޵Ĺ������ڵ�"] != OnlyShowAuthorizedNodes.ToString())
            {
                updateFlag = true;
                paramItem["ֻ��ʾ��Ȩ�޵Ĺ������ڵ�"] = OnlyShowAuthorizedNodes.ToString();
            }

            if (paramItem["ϵͳƽ̨"] != _SystemName.ToString())
            {
                updateFlag = true;
                paramItem["ϵͳƽ̨"] = _SystemName.ToString();
            }

            if (updateFlag)
                m_xmlParams.SaveParams(m_dicParams, false);
        }
    }
}
