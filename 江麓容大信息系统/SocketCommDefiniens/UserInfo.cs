using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace SocketCommDefiniens
{
    /// <summary>
    /// 用于SOCKET通讯用的用户信息
    /// </summary>
    public class Socket_UserInfo
    {
        /// <summary>
        /// 用户登录状态
        /// </summary>
        public enum LoginInStatusEnum { None, 登录失败, 登录成功 }

        /// <summary>
        /// 用户编码
        /// </summary>
        private string userCode;

        /// <summary>
        /// 获取或设置用户编码
        /// </summary>
        [XmlElement("UserCode")]
        public string UserCode
        {
            get { return userCode; }
            set { userCode = value; }
        }

        /// <summary>
        /// 用户密码
        /// </summary>
        private string userPwd;

        /// <summary>
        /// 获取或设置用户密码
        /// </summary>
        [XmlElement("UserPwd")]
        public string UserPwd
        {
            get { return userPwd; }
            set { userPwd = value; }
        }

        /// <summary>
        /// 用户登录状态
        /// </summary>
        private LoginInStatusEnum loginInStatus;

        /// <summary>
        /// 获取或设置用户登录状态
        /// </summary>
        [XmlElement("LoginInStatus")]
        public LoginInStatusEnum LoginInStatus
        {
            get { return loginInStatus; }
            set { loginInStatus = value; }
        }

        /// <summary>
        /// 登陆信息
        /// </summary>
        private string m_errorInfo;

        /// <summary>
        /// 获取或设置登陆信息
        /// </summary>
        [XmlElement("ErrorInfo")]
        public string ErrorInfo
        {
            get { return m_errorInfo; }
            set { m_errorInfo = value; }
        }
    }
}
