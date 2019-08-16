using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 消息提示窗体
    /// </summary>
    public partial class FormMessagePrompt : Form
    {
        /// <summary>
        /// 保存得到焦点前拥有活动窗体的柄
        /// </summary>
        private IntPtr m_activedForm = IntPtr.Zero;

        /// <summary>
        /// 是否激活窗体的标志
        /// </summary>
        //private bool m_activedFormFlag = false;

        /// <summary>
        /// 显示窗体的消息
        /// </summary>
        const int WM_SHOWWINDOW = 0x0018;

        /// <summary>
        /// 点击标题栏消息
        /// </summary>
        const int WM_NCLBUTTONDOWN = 0xA1;

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 获取通知类消息数据库操作接口
        /// </summary>
        IFlowNoticeManagement m_flowNotice = PlatformFactory.GetObject<IFlowNoticeManagement>();

        /// <summary>
        /// 单据流消息数据库操作接口
        /// </summary>
        IBillFlowMessage m_billMsg = PlatformFactory.GetObject<IBillFlowMessage>();

        /// <summary>
        /// 用户管理数据库操作接口
        /// </summary>
        IUserManagement m_user = PlatformFactory.GetObject<IUserManagement>();

        /// <summary>
        /// 用户编码与用户姓名的字典
        /// </summary>
        Dictionary<string, string> m_dicUserName = new Dictionary<string,string>();

        /// <summary>
        /// 消息标签字典
        /// </summary>
        Dictionary<string, UserControlMessageLabel> m_dicMsgLabel = new Dictionary<string, UserControlMessageLabel>();

        /// <summary>
        /// 是否已经检查过消息的标志
        /// </summary>
        bool m_check;

        /// <summary>
        /// 是否首次显示的标志
        /// </summary>
        bool m_firstShow = false;

        /// <summary>
        /// 消息数据
        /// </summary>
        WndMsgData m_msgData = new WndMsgData();

        /// <summary>
        /// 滚动条位置
        /// </summary>
        int m_scrollPosition;

        /// <summary>
        /// 获取消息数量
        /// </summary>
        public int MsgAmount
        {
            get { return this.Controls.Count; }
        }

        /// <summary>
        /// 是否静默的标志
        /// </summary>
        private bool m_silent = false;

        /// <summary>
        /// 获取或设置是否静默的标志
        /// </summary>
        public bool Silent
        {
            get { return m_silent; }

            set
            {
                m_silent = value;

                if (!m_silent)
                {
                    RefreshData();
                }
            }
        }

        /// <summary>
        /// 需要退出应用程序的标志
        /// </summary>
        public bool ExitApp
        {
            get;
            set;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormMessagePrompt()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_SHOWWINDOW:

                    if (!m_check)
                    {
                        m_activedForm = StapleInfo.GetActiveWindow();
                        this.Location = new Point(-800, -800);
                        RefreshData();
                        this.Activate();
                        return;
                    }

                    base.DefWndProc(ref m);
                    break;

                case WM_NCLBUTTONDOWN:
                    //m_activedFormFlag = true;
                    base.DefWndProc(ref m);
                    break;

                case WndMsgSender.CloseMsg:
                    ExitApp = true;
                    this.Close();
                    break;

                case WndMsgSender.FinishedMsg:
                    DisposeMessage(m);
                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg">要处理的消息</param>
        private void DisposeMessage(Message msg)
        {
            Type dataType = m_msgData.GetType();
            m_msgData = (WndMsgData)msg.GetLParam(dataType);

            if (msg.Msg == WndMsgSender.FinishedMsg)
            {
                if (m_msgData.MessageType == MessageTypeEnum.单据消息)
                {
                    string[] info = m_msgData.MessageContent.Split(StapleInfo.SplitChar);
                    string billType = info[0];
                    string billNo = info[1];

                    for (int i = 0; i < this.Controls.Count; i++)
                    {
                        UserControlMessageLabel msgLabel = this.Controls[i] as UserControlMessageLabel;
                        if (msgLabel.Message.Contains(billType) && msgLabel.Message.Contains(billNo))
                        {
                            this.Controls.RemoveAt(i);
                            m_dicMsgLabel.Remove(msgLabel.Name);
                            break;
                        }
                    }
                }
                else if (m_msgData.MessageType == MessageTypeEnum.知会消息)
                {
                    string name = m_msgData.NoticeSource.ToString().Substring(0, 2) + m_msgData.MessageContent;

                    for (int i = 0; i < this.Controls.Count; i++)
                    {
                        if (this.Controls[i].Name == name)
                        {
                            m_dicMsgLabel.Remove(this.Controls[i].Name);
                            this.Controls.RemoveAt(i);
                            //Controls[i].Dispose();
                            break;
                        }
                    }
                }

                ControlFormSize();

                if (this.Controls.Count == 0)
                {
                    this.Visible = false;
                }
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshData()
        {
            m_check = true;

            CheckNoticeMessage();
            CheckBillMessage();
            ControlFormSize();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerCheckMsg_Tick(object sender, EventArgs e)
        {
            if (this.Visible)
                RefreshData();
        }

        /// <summary>
        /// 控制滚动条位置
        /// </summary>
        /// <param name="position">位置值</param>
        private void ControlScrollPosition(int position)
        {
            if (position < this.VerticalScroll.Minimum)
            {
                position = this.VerticalScroll.Minimum;
            }
            else if (position > this.VerticalScroll.Maximum)
            {
                position = this.VerticalScroll.Maximum;
            }

            if (this.Visible && this.Controls.Count > 0)
            {
                this.VerticalScroll.Value = position;
                this.VerticalScroll.Value = position;
                this.Validate();
                this.Invalidate();
                this.Update();
                this.VerticalScroll.Value = position;
            }
        }

        /// <summary>
        /// 设置窗体显示位置
        /// </summary>
        private void SetFormLocation()
        {
            Rectangle rect = Screen.GetWorkingArea(this);
            Point pt1 = new Point(rect.Location.X + rect.Width - this.Width, rect.Location.Y + rect.Height - this.Height);
            this.Location = pt1;

            ControlScrollPosition(m_scrollPosition);
        }

        /// <summary>
        /// 控制窗体高度
        /// </summary>
        private void ControlFormSize()
        {
            if (this.Controls.Count > 0)
            {
                int controlHeight = this.Controls.Count * this.Controls[0].Height + 35;

                this.Height = controlHeight;

                if (this.Height > 265)
                {
                    this.Height = 265;
                }

                this.Text = string.Format("您有待处理事件：{0}条", this.Controls.Count);
            }
            else
            {
                this.Height = 26;
                this.Text = "没有要处理的事件";
            }

            m_activedForm = StapleInfo.GetActiveWindow();

            SetFormLocation();

            if (!Silent && !m_firstShow && !this.Visible && this.Controls.Count > 0)
            {
                m_firstShow = true;
                this.Visible = true;
                ControlScrollPosition(0);
            }
        }

        /// <summary>
        /// 获取通知类消息字符串
        /// </summary>
        /// <param name="notice">知会消息</param>
        /// <returns>返回获取到的字符串</returns>
        private string GetNoticeMessage(Flow_Notice notice)
        {
            return string.Format("{0} 优先级：{1} {2}, 内容：{3}", notice.来源, notice.优先级, notice.标题, notice.内容);
        }

        /// <summary>
        /// 获取单据类消息字符串
        /// </summary>
        /// <param name="message">单据消息</param>
        /// <returns>返回获取到的字符串</returns>
        private string GetBillMessage(Flow_BillFlowMessage message)
        {
            //return string.Format("{0}{1} 内容：{2}", message.单据流水号, message.单据类型, message.发起方消息);
            return string.Format("{0}：{1}", message.单据类型, message.发起方消息);
        }

        /// <summary>
        /// 检查是否有新的知会类消息
        /// </summary>
        private void CheckNoticeMessage()
        {
            // 发消息到主窗体的标志
            bool sendMsgToMainForm = false;

            foreach (NoticeSource noticeEnum in Enum.GetValues(typeof(NoticeSource)))
            {
                IQueryable<Flow_Notice> noticeData = m_flowNotice.GetNotice(BasicInfo.LoginID, noticeEnum);

                List<Flow_Notice> dataSource = new List<Flow_Notice>();

                // 按时间顺序逆向排序
                noticeData = from r in noticeData
                             orderby r.发送时间 descending
                             select r;

                // 剔除自己发送的信息
                if (noticeData.Count() > 0)
                    dataSource = (from r in noticeData where r.发送人 != BasicInfo.LoginID select r).Take(20).ToList();

                string prefix = noticeEnum.ToString().Substring(0, 2);

                foreach (var item in dataSource)
                {
                    if (m_dicMsgLabel.ContainsKey(prefix + item.序号.ToString()))
                    {
                        continue;
                    }

                    UserControlMessageLabel msgLabel = new UserControlMessageLabel(StapleInfo.MainForm, GetNoticeMessage(item));
                    msgLabel.Name = prefix + item.序号.ToString();
                    msgLabel.MessageID = item.序号;
                    msgLabel.MessageType = MessageTypeEnum.知会消息;
                    msgLabel.NoticeSource = noticeEnum;
                    msgLabel.Date = item.发送时间;

                    if (!GlobalObject.GeneralFunction.IsNullOrEmpty(item.发送人))
                    {
                        if (!m_dicUserName.ContainsKey(item.发送人))
                        {
                            m_dicUserName.Add(item.发送人, m_user.GetUser(item.发送人).姓名);
                        }
                    }

                    msgLabel.UserName = m_dicUserName[item.发送人];

                    if (item.优先级 == "高")
                    {
                        msgLabel.ForeColor = Color.Red;
                    }

                    msgLabel.Dock = DockStyle.Top;
                    this.Controls.Add(msgLabel);

                    m_dicMsgLabel.Add(msgLabel.Name, msgLabel);
                    msgLabel.OnControlClick += new EventHandler(this.UserControlMessageLabel_Clicked);

                    if (!sendMsgToMainForm)
                    {
                        sendMsgToMainForm = true;

                        WndMsgData sendData = new WndMsgData();

                        sendData.MessageType = msgLabel.MessageType;
                        sendData.NoticeSource = msgLabel.NoticeSource;
                        sendData.MessageContent = msgLabel.MessageID.ToString();
                        m_wndMsgSender.SendMessage(StapleInfo.MainForm.Handle, WndMsgSender.NewFlowMsg, sendData);
                    }
                }

                for (int i = 0; i < this.Controls.Count; i++)
                {
                    Control msgLabel = this.Controls[i];

                    if (msgLabel.Name.Contains(prefix))
                    {
                        int findIndex = Convert.ToInt32(msgLabel.Name.Substring(2));

                        if (dataSource.FindIndex(p => p.序号 == findIndex) < 0)
                        {
                            this.Controls.RemoveAt(i--);
                            m_dicMsgLabel.Remove(msgLabel.Name);
                            msgLabel.Dispose();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 检查是否有新的单据消息
        /// </summary>
        private void CheckBillMessage()
        {
            // 发消息到主窗体的标志
            bool sendMsgToMainForm = false;

            IQueryable<Flow_BillFlowMessage> result = m_billMsg.GetReceivedMessage(BasicInfo.LoginID);

            List<Flow_BillFlowMessage> dataSource = (from r in result
                                                     where r.单据状态 != BillStatus.已完成.ToString()
                                                     orderby r.发起时间 descending
                                                     select r).Take(20).ToList();

            foreach (var item in dataSource)
            {
                if (m_dicMsgLabel.ContainsKey("BillMsg" + item.序号.ToString()))
                {
                    continue;
                }

                UserControlMessageLabel msgLabel = new UserControlMessageLabel(StapleInfo.MainForm, GetBillMessage(item));
                msgLabel.Name = "BillMsg" + item.序号.ToString();
                msgLabel.MessageID = item.序号;
                msgLabel.MessageType = MessageTypeEnum.单据消息;
                msgLabel.Date = item.发起时间;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(item.发起方用户编码))
                {
                    if (!m_dicUserName.ContainsKey(item.发起方用户编码))
                    {
                        m_dicUserName.Add(item.发起方用户编码, m_user.GetUser(item.发起方用户编码).姓名);
                    }
                }

                msgLabel.UserName = m_dicUserName[item.发起方用户编码];

                if (msgLabel.Message.Contains("退货"))
                {
                    msgLabel.ForeColor = Color.Red;
                }
                else
                {
                    msgLabel.ForeColor = Color.Blue;
                }
                
                msgLabel.Dock = DockStyle.Top;
                this.Controls.Add(msgLabel);

                m_dicMsgLabel.Add(msgLabel.Name, msgLabel);
                msgLabel.OnControlClick += new EventHandler(this.UserControlMessageLabel_Clicked);

                if (!sendMsgToMainForm)
                {
                    sendMsgToMainForm = true;
                    WndMsgData sendData = new WndMsgData();
                    sendData.MessageType = msgLabel.MessageType;
                    sendData.NoticeSource = msgLabel.NoticeSource;
                    sendData.MessageContent = msgLabel.MessageID.ToString();
                    m_wndMsgSender.SendMessage(StapleInfo.MainForm.Handle, WndMsgSender.NewFlowMsg, sendData);
                }
            }

            for (int i = 0; i < this.Controls.Count; i++)
            {
                Control msgLabel = this.Controls[i];

                if (msgLabel.Name.Contains("BillMsg"))
                {
                    if (dataSource.FindIndex(p => p.序号 == Convert.ToInt32(msgLabel.Name.Substring(7))) < 0)
                    {
                        this.Controls.RemoveAt(i--);
                        m_dicMsgLabel.Remove(msgLabel.Name);
                        msgLabel.Dispose();
                    }
                }
            }
        }

        private void FormMessagePrompt_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!ExitApp)
            {
                e.Cancel = true;
                this.Visible = false;
                //m_activedFormFlag = false;
            }
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);

            SetFormLocation();
        }

        /// <summary>
        /// 消息控件被点击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlMessageLabel_Clicked(object sender, EventArgs e)
        {
            m_scrollPosition = this.VerticalScroll.Value;
        }

        private void FormMessagePrompt_MouseDown(object sender, MouseEventArgs e)
        {
            //m_activedFormFlag = true;
        }
    }
}
