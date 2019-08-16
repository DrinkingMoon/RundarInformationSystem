using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlatformManagement;
using System.Diagnostics;
using GlobalObject;
using ServerModule;
using System.Data;

namespace ServerModule
{
    /// <summary>
    /// 知会公共信息
    /// </summary>
    public struct NoticePublicInfo
    {
        /// <summary>
        /// 获取或设置发送人
        /// </summary>
        public string Sender
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置优先级
        /// </summary>
        public TransactionPriority Priority
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置消息来源
        /// </summary>
        public NoticeSource Source
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置标题
        /// </summary>
        public string Title
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置内容
        /// </summary>
        public string Content
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 单据类消息发布器
    /// </summary>
    public class BillMessagePromulgatorServer : ServerModule.IBillMessagePromulgatorServer
    {
        /// <summary>
        /// 获取单据流消息操作接口
        /// </summary>
        IBillFlowMessage m_billFlowMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 获取通知类消息数据库操作接口
        /// </summary>
        IFlowNoticeManagement m_flowNotice = PlatformFactory.GetObject<IFlowNoticeManagement>();

        /// <summary>
        /// 角色管理器
        /// </summary>
        IRoleManagement m_roleManager = PlatformFactory.GetObject<IRoleManagement>();

        /// <summary>
        /// 用户管理器
        /// </summary>
        IUserManagement m_userManager = PlatformFactory.GetObject<IUserManagement>();

        /// <summary>
        /// 日志管理
        /// </summary>
        ILogManagement m_logManagement = PlatformFactory.GetObject<ILogManagement>();

        /// <summary>
        /// 单据类别
        /// </summary>
        private string m_billType;

        /// <summary>
        /// 错误处理
        /// </summary>
        private string m_error;

        /// <summary>
        /// 获取或设置单据类别
        /// </summary>
        public string BillType
        {
            get { return m_billType; }
            set { m_billType = value; }
        }

        /// <summary>
        /// 消息提示界面句柄
        /// </summary>
        private IntPtr m_msgPromptFormHandle;

        /// <summary>
        /// 获取或设置消息提示界面句柄
        /// </summary>
        public IntPtr MessagePromptFormHandle
        {
            get { return m_msgPromptFormHandle; }
            set { m_msgPromptFormHandle = value; }
        }

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 构造函数
        /// </summary>
        public BillMessagePromulgatorServer()
        {
            Debug.Assert(m_logManagement != null, "获取不到日志管理组件");
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billType">单据类别</param>
        public BillMessagePromulgatorServer(string billType)
        {
            m_billType = billType;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="noticeRoles">知会角色</param>
        /// <param name="noticeUserCodes">知会用户编码列表</param>
        /// <returns>获取到的用户信息</returns>
        public List<string> GetUserCodes(List<string> noticeRoles, List<string> noticeUserCodes)
        {
            Debug.Assert(noticeRoles != null || noticeUserCodes != null, "角色编码 和 用户编码不能同时为空！");

            List<string> userCodes = new List<string>();
            List<View_Auth_Role> roles = null;
            string[] roleCodes = null;

            if (noticeUserCodes != null)
            {
                userCodes.AddRange(noticeUserCodes);
            }

            #region 获取所有角色所包含的用户编码

            roles = m_roleManager.GetRoleViewFromRoleName(noticeRoles);

            if (roles != null)
            {
                roleCodes = (from r in roles select r.角色编码).Distinct().ToArray();

                List<View_Auth_User> userInfo = m_userManager.GetUsers(roleCodes).ToList();

                foreach (var user in userInfo)
                {
                    if (!userCodes.Contains(user.登录名))
                    {
                        userCodes.Add(user.登录名);
                    }
                }
            }

            #endregion 获取所有角色所包含的用户编码

            return userCodes;
        }

        /// <summary>
        /// 发布知会消息
        /// </summary>
        /// <param name="noticeInfo">知会信息</param>
        /// <param name="noticeRoles">知会角色</param>
        /// <param name="noticeUserCodes">知会用户编码列表</param>
        public void NotifyMessage(NoticePublicInfo noticeInfo, List<string> noticeRoles, List<string> noticeUserCodes)
        {
            List<string> userCodes = GetUserCodes(noticeRoles, noticeUserCodes);

            for (int i = 0; i < userCodes.Count; i++)
            {
                // 去除自己给自己发的现象
                if (noticeInfo.Sender == userCodes[i])
                {
                    continue;
                }

                Flow_Notice notice = new Flow_Notice();

                notice.标题 = noticeInfo.Title;
                notice.优先级 = noticeInfo.Priority.ToString();
                notice.来源 = noticeInfo.Source.ToString();
                notice.内容 = noticeInfo.Content;
                notice.发送时间 = ServerModule.ServerTime.Time;
                notice.发送人 = noticeInfo.Sender;
                notice.状态 = NoticeStatus.未读.ToString();
                notice.接收人 = userCodes[i];

                m_flowNotice.SendNotice(notice);
            }
        }

        /// <summary>
        /// 发布知会消息
        /// </summary>
        /// <param name="billType">单据类型</param>
        /// <param name="billNo">单据编号</param>
        /// <param name="content">知会内容</param>
        /// <param name="sender">发布人</param>
        /// <param name="noticeRoles">知会角色</param>
        /// <param name="noticeUserCodes">知会用户编码列表</param>
        public void NotifyMessage(string billType, string billNo, string content, string sender, List<string> noticeRoles, List<string> noticeUserCodes)
        {
            List<string> userCodes = GetUserCodes(noticeRoles, noticeUserCodes);

            for (int i = 0; i < userCodes.Count; i++)
            {
                // 去除自己给自己发的现象
                if (sender == userCodes[i])
                {
                    continue;
                }

                Flow_Notice notice = new Flow_Notice();

                notice.标题 = billType;
                notice.单据流水号 = billNo;
                notice.优先级 = TransactionPriority.中.ToString();
                notice.来源 = NoticeSource.单据处理后知会.ToString();
                notice.内容 = content;
                notice.发送时间 = ServerModule.ServerTime.Time;
                notice.发送人 = sender;
                notice.状态 = NoticeStatus.未读.ToString();
                notice.接收人 = userCodes[i];

                m_flowNotice.SendNotice(notice);
            }
        }

        #region 发送新消息，传递旧消息 合二为一

        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedRole">接收消息的角色</param>
        public void SendMessage(string billNo, string message, CE_RoleEnum receivedRole)
        {
            SendMessage(billNo, message, BillFlowMessage_ReceivedUserType.角色, receivedRole.ToString());
        }      
 
        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户</param>
        public void SendMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, string receiver)
        {
            List<string> lstReceiver = new List<string>();

            lstReceiver.Add(receiver);

            SendMessage(billNo, message, receivedUserType, lstReceiver);
        }     
   
        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        public void SendMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, List<string> receiver)
        {
            SendMessage(billNo, message, receivedUserType, receiver, null);
        }    
   
        /// <summary>
        /// 发送传递流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        /// <param name="lstAdditionalInfo">
        /// 附加信息，用于重定向及数据定位等功能(列表第1个位置始终为重定向单据的单据类型，列表第2个位置始终为重定向单据的单据号)
        /// </param>
        public void SendMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            List<string> receiver, List<string> lstAdditionalInfo)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(message), "消息不能为空");

            try
            {
                Flow_BillFlowMessage msg = GetMessage(billNo);

                if (msg != null)
                {
                    msg.发起方用户编码 = BasicInfo.LoginID;
                    msg.发起方消息 = message;
                    msg.接收方类型 = receivedUserType.ToString();
                    msg.接收方 = StapleFunction.CreateSplitString(",", receiver);

                    msg.期望的处理完成时间 = null;

                    InitAdditionalInfo(ref msg, lstAdditionalInfo);

                    m_billFlowMsg.ContinueMessage(BasicInfo.LoginID, msg);

                    SendFinishedFlagToMessagePromptForm(billNo);
                }
                else
                {
                    msg = new Flow_BillFlowMessage();

                    msg.初始发起方用户编码 = BasicInfo.LoginID;
                    msg.单据号 = billNo;
                    msg.单据类型 = m_billType;
                    msg.单据流水号 = billNo;
                    msg.接收方类型 = receivedUserType.ToString();
                    msg.单据状态 = BillStatus.等待处理.ToString();
                    msg.发起方消息 = message;
                    msg.接收方 = StapleFunction.CreateSplitString(",", receiver);

                    msg.期望的处理完成时间 = null;

                    InitAdditionalInfo(ref msg, lstAdditionalInfo);

                    m_billFlowMsg.SendRequestMessage(BasicInfo.LoginID, msg);
                }
            }
            catch (Exception exce)
            {
                m_logManagement.WriteException(exce.Message + "\t\r" + exce.StackTrace, out m_error);
            }
        }

        #endregion

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedRole">接收消息的角色</param>
        public void SendNewFlowMessage(string billNo, string message, CE_RoleEnum receivedRole)
        {
            SendNewFlowMessage(billNo, message, BillFlowMessage_ReceivedUserType.角色, receivedRole.ToString());
        }

        /// <summary>
        /// 传递流消息到指定角色(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedRole">接收角色编码</param>
        /// <returns>操作是否成功的标志</returns>
        public bool PassFlowMessage(string billNo, string message, CE_RoleEnum receivedRole)
        {
            return PassFlowMessage(billNo, message, BillFlowMessage_ReceivedUserType.角色, receivedRole.ToString());
        }

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户</param>
        public void SendNewFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, string receiver)
        {
            List<string> lstReceiver = new List<string>();

            lstReceiver.Add(receiver);

            SendNewFlowMessage(billNo, message, receivedUserType, lstReceiver);
        }

        /// <summary>
        /// 传递流消息到指定角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedUserType">接收方类别，角色或用户</param>
        /// <param name="receiver">接收方代码，角色编码或用户编码</param>
        /// <returns>操作是否成功的标志</returns>
        public bool PassFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, string receiver)
        {
            List<string> lstReceiver = new List<string>();

            lstReceiver.Add(receiver);

            return PassFlowMessage(billNo, message, receivedUserType, lstReceiver);
        }

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        public void SendNewFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, List<string> receiver)
        {
            SendNewFlowMessage(billNo, message, receivedUserType, receiver, null);
        }

        /// <summary>
        /// 传递流消息到多个角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedUserType">接收方类别，角色或用户</param>
        /// <param name="receiver">接收方代码数组，角色编码或用户编码</param>
        /// <returns>操作是否成功的标志</returns>
        public bool PassFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType, List<string> receiver)
        {
            return PassFlowMessage(billNo, message, receivedUserType, receiver, null);
        }

        /// <summary>
        /// 初始化指定单据信息的附加数据
        /// </summary>
        /// <param name="msg">要初始化附加数据的单据信息</param>
        /// <param name="lstAdditionalInfo">附加信息值列表</param>
        private void InitAdditionalInfo(ref Flow_BillFlowMessage msg, List<string> lstAdditionalInfo)
        {
            msg.附加信息1 = "";
            msg.附加信息2 = "";
            msg.附加信息3 = "";
            msg.附加信息4 = "";
            msg.附加信息5 = "";
            msg.附加信息6 = "";
            msg.附加信息7 = "";
            msg.附加信息8 = "";

            if (lstAdditionalInfo != null)
            {
                for (int i = 1; i <= lstAdditionalInfo.Count; i++)
                {
                    if (lstAdditionalInfo[i-1] == null)
                    {
                        lstAdditionalInfo[i - 1] = "";
                    }
                    else
                    {
                        lstAdditionalInfo[i - 1] = lstAdditionalInfo[i - 1].Trim();
                    }

                    GlobalObject.GeneralFunction.SetValue(msg, "附加信息" + i.ToString(), lstAdditionalInfo[i - 1]);
                }

                // 附加信息1始终为重定向单据的单据类型，附加信息2始终为重定向单据的单据号
                if (lstAdditionalInfo.Count >= 2)
                {
                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(lstAdditionalInfo[0]) && !GlobalObject.GeneralFunction.IsNullOrEmpty(lstAdditionalInfo[1]))
                    {
                        lstAdditionalInfo[0] = GetBillType(lstAdditionalInfo[1]);
                    }
                }
            }
        }

        /// <summary>
        /// 发送新的流消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">单据消息</param>
        /// <param name="receivedUserType">接收用户类别，角色或用户</param>
        /// <param name="receiver">接收消息的角色或用户组</param>
        /// <param name="lstAdditionalInfo">
        /// 附加信息，用于重定向及数据定位等功能(列表第1个位置始终为重定向单据的单据类型，列表第2个位置始终为重定向单据的单据号)
        /// </param>
        public void SendNewFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            List<string> receiver, List<string> lstAdditionalInfo)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(message), "消息不能为空");

            try
            {
                Flow_BillFlowMessage msg = new Flow_BillFlowMessage();

                msg.初始发起方用户编码 = BasicInfo.LoginID;
                msg.单据号 = billNo;
                msg.单据类型 = m_billType;
                msg.单据流水号 = billNo;
                msg.接收方类型 = receivedUserType.ToString();
                msg.单据状态 = BillStatus.等待处理.ToString();
                msg.发起方消息 = message;
                msg.接收方 = StapleFunction.CreateSplitString(",", receiver);

                msg.期望的处理完成时间 = null;

                InitAdditionalInfo(ref msg, lstAdditionalInfo);

                m_billFlowMsg.SendRequestMessage(BasicInfo.LoginID, msg);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 传递流消息到多个角色或用户(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receivedUserType">接收方类别，角色或用户</param>
        /// <param name="receiver">接收方代码数组，角色编码或用户编码</param>
        /// <param name="lstAdditionalInfo">
        /// 附加信息，用于重定向及数据定位等功能(列表第1个位置始终为重定向单据的单据类型，列表第2个位置始终为重定向单据的单据号)
        /// </param>
        /// <returns>操作是否成功的标志</returns>
        public bool PassFlowMessage(string billNo, string message, BillFlowMessage_ReceivedUserType receivedUserType,
            List<string> receiver, List<string> lstAdditionalInfo)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(message), "消息不能为空");

            try
            {
                Flow_BillFlowMessage msg = GetMessage(billNo);

                if (msg != null)
                {
                    msg.发起方用户编码 = BasicInfo.LoginID;
                    msg.发起方消息 = message;
                    msg.接收方类型 = receivedUserType.ToString();
                    msg.接收方 = StapleFunction.CreateSplitString(",", receiver);

                    msg.期望的处理完成时间 = null;

                    InitAdditionalInfo(ref msg, lstAdditionalInfo);

                    m_billFlowMsg.ContinueMessage(BasicInfo.LoginID, msg);

                    SendFinishedFlagToMessagePromptForm(billNo);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 传递流消息到指定角色(走流程)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="receiver">消息接收方, 角色编码或用户编码</param>
        /// <param name="flag">True 为角色：False 为用户</param>
        /// <returns>操作是否成功的标志</returns>
        public bool PassFlowMessage(string billNo, string message, string receiver, bool flag)
        {
            BillFlowMessage_ReceivedUserType receivedUserType = BillFlowMessage_ReceivedUserType.角色;

            if (!flag)
            {
                receivedUserType = BillFlowMessage_ReceivedUserType.用户;
            }

            return PassFlowMessage(billNo, message, receivedUserType, receiver);
        }

        /// <summary>
        /// 回退流消息
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <returns>操作是否成功的标志</returns>
        public bool RebackFlowMessage(string billNo, string message)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(message), "消息不能为空");

            try
            {
                Flow_BillFlowMessage msg = GetMessage(billNo);

                if (msg != null)
                {
                    m_billFlowMsg.RebackMessage(BasicInfo.LoginID, msg.序号, message);

                    SendFinishedFlagToMessagePromptForm(billNo);
                }

                return true;
            }
            catch (Exception exce)
            {
                m_logManagement.WriteException(exce.Message + "\t\r" + exce.StackTrace, out m_error);
                return false;
            }
        }

        /// <summary>
        /// 结束流消息(流程已经走完)
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <param name="message">消息内容</param>
        /// <param name="noticeRoles">要知会的角色列表</param>
        /// <param name="noticeUsers">要知会的用户列表</param>
        /// <returns>操作是否成功的标志</returns>
        public bool EndFlowMessage(string billNo, string message, List<string> noticeRoles, List<string> noticeUsers)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(message), "消息不能为空");
            //Debug.Assert(noticeRoles != null, "要知会的角色列表不能为空");

            try
            {
                m_logManagement.WriteException(string.Format("进入EndFlowMessage, BillNo:{0},Msg:{1}", billNo, message), out m_error);

                Flow_BillFlowMessage msg = GetMessage(billNo);

                if (msg != null)
                {
                    m_billFlowMsg.EndMessage(BasicInfo.LoginID, msg.序号, message);

                    // 2014-11-17 14:55 增加判断，如果没有角色和知会用户时不发送知会消息
                    if ((noticeRoles != null && noticeRoles.Count > 0) || (noticeUsers != null && noticeUsers.Count > 0))
                    {
                        // 获取要知会的用户编码
                        List<string> lstNoticeUser = GetUserCodes(noticeRoles, noticeUsers);

                        if (!lstNoticeUser.Contains(msg.初始发起方用户编码))
                            lstNoticeUser.Add(msg.初始发起方用户编码);

                        // 发送知会消息
                        NotifyMessage(msg.单据类型, msg.单据号, message, BasicInfo.LoginID, noticeRoles, lstNoticeUser);
                    }

                    SendFinishedFlagToMessagePromptForm(billNo);
                }
                else
                {
                    m_logManagement.WriteException(string.Format("没有找到单据消息，主动添加消息。EndFlowMessage, BillNo:{0},Msg:{1}", billNo, message), out m_error);

                    BillFlowMessage_ReceivedUserType receivedUserType = BillFlowMessage_ReceivedUserType.用户;

                    if (noticeRoles != null && noticeRoles.Count > 0)
                    {
                        receivedUserType = BillFlowMessage_ReceivedUserType.角色;
                    }

                    msg = new Flow_BillFlowMessage();

                    msg.初始发起方用户编码 = BasicInfo.LoginID;
                    msg.单据号 = billNo;
                    msg.单据类型 = m_billType;
                    msg.单据流水号 = billNo;
                    msg.接收方类型 = receivedUserType.ToString();
                    msg.单据状态 = BillStatus.已完成.ToString();
                    msg.发起方消息 = message;
                    msg.接收方 = StapleFunction.CreateSplitString(",", 
                        receivedUserType == BillFlowMessage_ReceivedUserType.角色 ? noticeRoles : noticeUsers);

                    msg.期望的处理完成时间 = null;

                    m_billFlowMsg.SendRequestMessage(BasicInfo.LoginID, msg);
                }

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 销毁消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>操作是否成功的标志</returns>
        public bool DestroyMessage(string billNo)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");

            try
            {
                Flow_BillFlowMessage msg = GetMessage(billNo);

                if (msg != null)
                {
                    m_billFlowMsg.DestroyMessage(BasicInfo.LoginID, msg.序号);
                }

                DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

                //添加删除单据的记录 CJB 2012.3.19
                BASE_DeleteBill lnqDeleteBill = new BASE_DeleteBill();

                lnqDeleteBill.Bill_ID = billNo;
                lnqDeleteBill.DeleteTime = ServerTime.Time;
                lnqDeleteBill.WorkID = BasicInfo.LoginID;

                ctx.BASE_DeleteBill.InsertOnSubmit(lnqDeleteBill);
                ctx.SubmitChanges();

                SendFinishedFlagToMessagePromptForm(billNo);

                return true;
            }
            catch (Exception exce)
            {
                m_logManagement.WriteException(exce.Message + "\t\r" + exce.StackTrace, out m_error);
                return false;
            }
        }

        /// <summary>
        /// 获取消息
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>成功返回获取到的消息，失败返回null</returns>
        private Flow_BillFlowMessage GetMessage(string billNo)
        {
            if (m_billType == null || m_billType == "")
            {
                m_billType = GetBillType(billNo);
            }

            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(m_billType), "单据类别不能为空");
            Debug.Assert(!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo), "单据号不能为空");

            try
            {

                Flow_BillFlowMessage msg = m_billFlowMsg.GetMessage(BasicInfo.LoginID, m_billType, billNo);

                if (msg == null)
                {
                    m_logManagement.WriteException(string.Format("没有找到对应的单据消息，类型【{0}】，单据号【{1}】，操作人【{2},{3}】", 
                        m_billType, billNo, BasicInfo.LoginID, BasicInfo.LoginName), out m_error);
                }

                //if (msg == null)
                //{
                //    StackTrace st = new StackTrace();

                //    for (int i = 0; i < st.FrameCount; i++)
                //    {
                //        if (st.GetFrame(st.FrameCount - 1 - i).GetMethod().Name.Contains("SaveData"))
                //        {
                //            return null;
                //        }
                //    }

                //    m_logManagement.WriteException(
                //     string.Format("没有找到对应的单据消息:{0},{1}", BasicInfo.LoginID, billNo), out m_error);

                //    return null;
                //}

                return msg;
            }
            catch (Exception exce)
            {
                m_logManagement.WriteException(exce.Message + "\t\r" + exce.StackTrace, out m_error);
                return null;
            }
        }

        /// <summary>
        /// 根据库房ID或者库房名判断对应仓管角色
        /// </summary>
        /// <param name="storageInfo">库房ID/库房名称</param>
        /// <returns></returns>
        public CE_RoleEnum GetRoleStringForStorage(string storageInfo)
        {
            string strName = "";

            string strSql = "select * from BASE_Storage where StorageID = '" 
                + storageInfo + "' or StorageName = '" + storageInfo + "'";

            System.Data.DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (dt.Rows.Count == 0)
            {
                return CE_RoleEnum.未知;
            }
            else
            {
                strName = dt.Rows[0]["StorageName"].ToString();
            }

            switch (strName)
            {
                case "制造库房（二）":
                    return CE_RoleEnum.制造仓库二管理员;
                case "成品库房（二）":
                    return CE_RoleEnum.成品仓库二管理员;
                case "自制半成品库（二）":
                    return CE_RoleEnum.自制半成品库二管理员;
                default:
                    break;
            }

            CE_StorageName storageName = GlobalObject.GeneralFunction.StringConvertToEnum<CE_StorageName>(strName);

            switch (storageName)
            {
                case CE_StorageName.制造库房:
                case CE_StorageName.原材料库:

                    switch (GlobalParameter.SystemName)
                    {
                        case CE_SystemName.泸州容大:
                            return CE_RoleEnum.物流公司;
                        case CE_SystemName.湖南容大:
                            return CE_RoleEnum.制造仓库管理员;
                        default:
                            return CE_RoleEnum.未知;
                    }
                case CE_StorageName.电子元器件库房:
                    return CE_RoleEnum.电子元器件仓库管理员;
                case CE_StorageName.备件库房:
                case CE_StorageName.低辅料库:
                    return CE_RoleEnum.备件仓库管理员;
                case CE_StorageName.售后库房:
                    return CE_RoleEnum.售后库房管理员;
                case CE_StorageName.油品库:
                    return CE_RoleEnum.油品库管理员;
                case CE_StorageName.成品库房:
                case CE_StorageName.产成品库:
                    return CE_RoleEnum.成品仓库管理员;
                case CE_StorageName.量检具库:
                    return CE_RoleEnum.量检具库管理员;
                case CE_StorageName.自制半成品库:
                case CE_StorageName.半成品库:
                    return CE_RoleEnum.自制半成品库管理员;
                case CE_StorageName.售后配件库房:
                    return CE_RoleEnum.售后配件库管理员;
                case CE_StorageName.受托品库房:
                    return CE_RoleEnum.受托品库房管理员;
                case CE_StorageName.材料库:
                    return CE_RoleEnum.材料库管理员;
                case CE_StorageName.原材料样品库:
                case CE_StorageName.自制半成品样品库:
                    return CE_RoleEnum.样品库管理员;
                default:
                    return CE_RoleEnum.未知;
            }
        }
        
        /// <summary>
        /// 根据角色类型与科室编码获得角色名称列表
        /// </summary>
        /// <param name="roleStyle">角色类型</param>
        /// <param name="deptInfo">角色类型为仓管则为库房代码，角色类型为上级领导则为所需操作人员的人员工号或姓名,否则为需操作的部门编码</param>
        /// <returns>返回列表</returns>
        public List<string> GetSuperior(CE_RoleStyleType roleStyle, string info)
        {
            using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
            {
                return GetSuperior(ctx, roleStyle, info);
            }
        }

        /// <summary>
        /// 根据角色类型与科室编码获得角色名称列表
        /// </summary>
        /// <param name="ctx">数据上下文</param>
        /// <param name="roleStyle">角色类型</param>
        /// <param name="deptInfo">角色类型为仓管则为库房代码，角色类型为上级领导则为所需操作人员的人员工号或姓名,否则为需操作的部门编码</param>
        /// <returns>返回列表</returns>
        public List<string> GetSuperior(DepotManagementDataContext ctx, CE_RoleStyleType roleStyle, string info)
        {
            List<string> result = new List<string>();

            if (info != null && info != "")
            {
                View_HR_Personnel personnelInfo = UniversalFunction.GetPersonnelInfo(ctx, info);

                switch (roleStyle)
                {
                    case CE_RoleStyleType.仓管:

                        List<CE_RoleEnum> tempListRole = UniversalFunction.GetStoreroomKeeperRoleEnumList(info);

                        foreach (CE_RoleEnum item in tempListRole)
                        {
                            result.Add(item.ToString());
                        }
                        break;
                    case CE_RoleStyleType.分管领导:
                        result = GetDeptLeaderRoleName(personnelInfo == null ? info : personnelInfo.部门编码).ToList();
                        break;
                    case CE_RoleStyleType.负责人:
                        result = GetDeptPrincipalRoleName(personnelInfo == null ? info : personnelInfo.部门编码).ToList();
                        break;
                    case CE_RoleStyleType.主管:
                        result = GetDeptDirectorRoleName(personnelInfo == null ? info : personnelInfo.部门编码).ToList();
                        break;
                    case CE_RoleStyleType.所有上级领导:

                        List<string> lstFGLD1 = GetDeptLeaderRoleName(personnelInfo.部门编码).ToList();
                        List<string> lstFZR1 = GetDeptPrincipalRoleName(personnelInfo.部门编码).ToList();
                        List<string> lstZG1 = GetDeptDirectorRoleName(personnelInfo.部门编码).ToList();

                        result.AddRange(lstFGLD1);
                        result.AddRange(lstFZR1);
                        result.AddRange(lstZG1);
                        break;
                    case CE_RoleStyleType.上级领导:

                        List<string> lstFGLD = GetDeptLeaderRoleName(personnelInfo.部门编码).ToList();
                        List<string> lstFZR = GetDeptPrincipalRoleName(personnelInfo.部门编码).ToList();
                        List<string> lstZG = GetDeptDirectorRoleName(personnelInfo.部门编码).ToList();

                        IRoleManagement serviceRole = PlatformFactory.GetObject<IRoleManagement>();
                        List<View_Auth_Role> lstRole = serviceRole.GetRoles(personnelInfo.工号).ToList();
                        List<CE_RoleStyleType> lstType = new List<CE_RoleStyleType>();

                        foreach (View_Auth_Role role in lstRole)
                        {
                            if (lstFGLD.Contains(role.角色名称))
                            {
                                lstType.Add(CE_RoleStyleType.分管领导);
                            }
                            else if (lstFZR.Contains(role.角色名称))
                            {
                                lstType.Add(CE_RoleStyleType.负责人);
                            }
                            else if (lstZG.Contains(role.角色名称))
                            {
                                lstType.Add(CE_RoleStyleType.主管);
                            }
                        }

                        if (lstType == null || lstType.Count == 0)
                        {
                            return lstZG;
                        }
                        else if (lstType.Contains(CE_RoleStyleType.分管领导))
                        {
                            result.Add(CE_RoleEnum.总经理.ToString());
                            result.Add(CE_RoleEnum.总经理.ToString());
                            return result;
                        }
                        else if (lstType.Contains(CE_RoleStyleType.负责人))
                        {
                            return lstFGLD;
                        }
                        else if (lstType.Contains(CE_RoleStyleType.主管))
                        {
                            return lstFZR;
                        }

                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        /// <summary>
        /// 获取部门主管角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        public string[] GetDeptDirectorRoleName(string deptInfo)
        {
            IDeptManagerRole deptManagerRole = PlatformFactory.GetObject<IDeptManagerRole>();

            return deptManagerRole.GetManagementRoleName(deptInfo, RoleStyle.主管).ToArray();

        }

        /// <summary>
        /// 获取部门负责人角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        public string[] GetDeptPrincipalRoleName(string deptInfo)
        {
            IDeptManagerRole deptManagerRole = PlatformFactory.GetObject<IDeptManagerRole>();

            return deptManagerRole.GetManagementRoleName(deptInfo, RoleStyle.负责人).ToArray();
        }

        #region 2014-11-08 夏石友
                
        /// <summary>
        /// 获取指定部门的最高部门编码（如果当前部门就是顶级部门则直接返回当前部门编码）
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>获取指定部门的最高部门的编码（如果当前部门就是顶级部门则直接返回当前部门编码）</returns>
        public string GetHighestDeptCode(string deptInfo)
        {
            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            var result = from r in ctx.View_HR_Dept
                         where r.部门代码 == deptInfo || r.部门名称 == deptInfo
                         select r;

            if (result.Count() == 0)
                throw new ArgumentException(string.Format("获取不到【{0}】的部门信息"));

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(result.Single().父级编码))
                return result.Single().部门代码;
            else
                return GetHighestDeptCode(result.Single().父级编码);
        }

        /// <summary>
        /// 获取最高部门负责人角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        public string[] GetHighestDeptPrincipalRoleName(string deptInfo)
        {
            IDeptManagerRole deptManagerRole = PlatformFactory.GetObject<IDeptManagerRole>();

            return deptManagerRole.GetManagementRoleName(GetHighestDeptCode(deptInfo), RoleStyle.负责人).ToArray();
        } 

        #endregion

        /// <summary>
        /// 获取部门分管领导角色名称
        /// </summary>
        /// <param name="deptInfo">部门编码或部门名称</param>
        /// <returns>返回获取到的角色名称数组</returns>
        public string[] GetDeptLeaderRoleName(string deptInfo)
        {
            IDeptManagerRole deptManagerRole = PlatformFactory.GetObject<IDeptManagerRole>();

            return deptManagerRole.GetManagementRoleName(deptInfo, RoleStyle.分管领导).ToArray();
        }

        /// <summary>
        /// 判断用户是否是指定部门的分管领导
        /// </summary>
        /// <param name="userCode">要判断的用户编码</param>
        /// <param name="deptCode">部门编码</param>
        /// <returns>是则返回true</returns>
        public bool IsDeptLeader(string userCode, string deptCode)
        {
            List<View_Auth_Role> lstRole = PlatformFactory.GetRoleManagement().GetRoles(userCode).ToList();

            string[] leader = GetDeptLeaderRoleName(deptCode);

            foreach (var role in leader)
            {
                if (lstRole.FindIndex(p => p.角色名称 == role) != -1)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获得部门编码
        /// </summary>
        /// <param name="departmentName">部门名称</param>
        /// <returns>返回部门编码</returns>
        string GetDepartmentCode(string departmentName)
        {
            string strSql = "select DeptCode from HR_Dept where DeptCode = '" + departmentName + "'";

            DataTable dtTemp = GlobalObject.DatabaseServer.QueryInfo(strSql);

            return dtTemp.Rows[0][0].ToString();
        }

        /// <summary>
        /// 获取单据类型
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>返回获取到的单据类型</returns>
        private string GetBillType(string billNo)
        {
            string strSql = "select * from BASE_BillType";

            DataTable dt = GlobalObject.DatabaseServer.QueryInfo(strSql);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (billNo.Contains(dt.Rows[i]["TypeCode"].ToString()))
                {
                    return dt.Rows[i]["TypeName"].ToString();
                }
            }

            throw new Exception(string.Format("无法获取到单据号为 {0} 单据的单据类型", billNo));
        }

        /// <summary>
        /// 发送完成标志信息到消息提示窗体
        /// </summary>
        /// <param name="billNo">单据编号</param>
        private void SendFinishedFlagToMessagePromptForm(string billNo)
        {
            if (m_msgPromptFormHandle == null)
            {
                return;
            }

            WndMsgData msgData = new WndMsgData();

            msgData.MessageType = MessageTypeEnum.单据消息;
            msgData.MessageContent = string.Format("{0},{1}", m_billType, billNo);

            m_wndMsgSender.SendMessage(m_msgPromptFormHandle, WndMsgSender.FinishedMsg, msgData);
        }

        /// <summary>
        /// 获得单据类型
        /// </summary>
        /// <param name="billTypeCode">单据类型代码</param>
        /// <returns>返回单据类型</returns>
        public CE_BillTypeEnum GetBillTypeEnum(string billTypeCode)
        {
            string strSql = "select * from BASE_BillType where TypeCode = '"+ billTypeCode +"'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return CE_BillTypeEnum.未知单据;
            }
            else
            {
                string billType = tempTable.Rows[0]["TypeName"].ToString();

                return GlobalObject.GeneralFunction.StringConvertToEnum<CE_BillTypeEnum>(billType);
            }
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="billNo">单据号</param>
        /// <returns>存在返回True, 不存在返回False</returns>
        public bool IsExist(string billNo)
        {
            string strSql = "select * from PlatformService.dbo.Flow_BillFlowMessage where 单据号 = '" + billNo + "'";

            DataTable tempTable = GlobalObject.DatabaseServer.QueryInfo(strSql);

            if (tempTable.Rows.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
