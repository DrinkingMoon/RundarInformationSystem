using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SocketCommDefiniens
{
    /// <summary>
    /// ����SOCKETͨѶ�õ��û���Ϣ
    /// </summary>
    public class Socket_UserInfo
    {
        /// <summary>
        /// �û���¼״̬
        /// </summary>
        public enum LoginInStatusEnum { None, ��¼ʧ��, ��¼�ɹ� }

        /// <summary>
        /// �û�����
        /// </summary>
        private string userCode;

        /// <summary>
        /// ��ȡ�������û�����
        /// </summary>
        [XmlElement("UserCode")]
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        /// <summary>
        /// �û�����
        /// </summary>
        private string userPwd;

        /// <summary>
        /// ��ȡ�������û�����
        /// </summary>
        [XmlElement("UserPwd")]
        public string UserPwd
        {
            get { return userPwd; }
            set { userPwd = value; }
        }

        /// <summary>
        /// �û���¼״̬
        /// </summary>
        private LoginInStatusEnum loginInStatus;

        /// <summary>
        /// ��ȡ�������û���¼״̬
        /// </summary>
        [XmlElement("LoginInStatus")]
        public LoginInStatusEnum LoginInStatus
        {
            get { return loginInStatus; }
            set { loginInStatus = value; }
        }

        /// <summary>
        /// ��½��Ϣ
        /// </summary>
        private string m_errorInfo;

        /// <summary>
        /// ��ȡ�����õ�½��Ϣ
        /// </summary>
        [XmlElement("ErrorInfo")]
        public string ErrorInfo
        {
            get { return m_errorInfo; }
            set { m_errorInfo = value; }
        }
    }
}
