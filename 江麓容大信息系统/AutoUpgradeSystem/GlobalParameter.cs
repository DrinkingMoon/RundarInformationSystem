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
using System.Data;
using System.Windows.Forms;
using System.Management;

namespace AutoUpgradeSystem
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
        /// ��ȡ��������·������󲻰���"\"
        /// </summary>
        /// <returns>��������·��</returns>
        public static String GetAppRunPath()
        {
            return Environment.CurrentDirectory;
        }

        /// <summary>
        /// ��ʼ�����в�����
        /// </summary>
        /// <param name="error">����ʱ���ش�����Ϣ���޴�ʱ����null</param>
        /// <returns>�ɹ���ʼ������true��ʧ�ܷ���false</returns>
        public static bool Init(out string error)
        {
            error = null;

            m_xmlParams = new XmlParams(GetAppRunPath() + "\\SystemParams.xml");

            m_dicParams = m_xmlParams.GetParams();

            Dictionary<string, string> paramItem = m_dicParams["ϵͳ����"] as Dictionary<string, string>;

            m_dataServerIP = paramItem["���ݿ��������ַ"];

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
                error = "��û�п��õ��������ӣ����������Ƿ�����";
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

                    // ������һ��IP��ַ
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
        /// ����ָ��IP�������ݿ�������Ƿ�ɹ�
        /// </summary>
        /// <param name="ip">Ҫ���Ե�IP��ַ</param>
        /// <returns>���ӳɹ�����true, ʧ�ܷ���false</returns>
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
        /// ������Ϣ�������ļ�
        /// </summary>
        public static void Save()
        {
            Dictionary<string, string> paramItem = m_dicParams["ϵͳ����"] as Dictionary<string, string>;

            m_dbPlatformServiceConnectionString = m_dbPlatformServiceConnectionString.Replace(paramItem["���ݿ��������ַ"], m_dataServerIP);

            paramItem["���ݿ��������ַ"] = m_dataServerIP;

            m_xmlParams.SaveParams(m_dicParams, false);
        }
    }
}
