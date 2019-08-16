using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;

namespace Expression
{
    public partial class UserControlMessageLabel : UserControl
    {
        /// <summary>
        /// 消息类型
        /// </summary>
        public MessageTypeEnum MessageType
        {
            get;
            set;
        }

        /// <summary>
        /// 仅针对知会类消息可用
        /// </summary>
        public NoticeSource NoticeSource
        {
            get;
            set;
        }

        public long MessageID
        {
            get;
            set;
        }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message
        {
            set { lblMsg.Text = value; }
            get { return lblMsg.Text; }
        }

        public string UserName
        {
            set { lblName.Text = value; }
            get { return lblName.Text; }
        }

        public DateTime Date
        {
            set { lblTime.Text = string.Format("{0:D2}-{1:D2} {2}", value.Month, value.Day, value.ToShortTimeString()); }
        }

        /// <summary>
        /// 窗体消息发布器
        /// </summary>
        WndMsgSender m_wndMsgSender = new WndMsgSender();

        /// <summary>
        /// 当点击消息时需知会的窗体
        /// </summary>
        Form m_noticeForm = null;

        /// <summary>
        /// 默认光标
        /// </summary>
        Cursor m_defaultCursor;

        /// <summary>
        /// 点击事件
        /// </summary>
        public event EventHandler OnControlClick;

        public UserControlMessageLabel(Form noticeForm, string msg)
        {
            InitializeComponent();

            m_noticeForm = noticeForm;
            lblMsg.Text = msg;
            m_defaultCursor = this.Cursor;
        }

        private void lblMsg_Click(object sender, EventArgs e)
        {
            WndMsgData sendData = new WndMsgData();
            sendData.MessageType = this.MessageType;
            sendData.NoticeSource = this.NoticeSource;
            sendData.MessageContent = this.MessageID.ToString();
            m_wndMsgSender.SendMessage(m_noticeForm.Handle, WndMsgSender.PositioningMsg, sendData);

            if (OnControlClick != null)
            {
                OnControlClick(sender, e);
            }
        }

        private void lblMsg_MouseHover(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Hand;
            lblMsg.ForeColor = Color.OrangeRed;

            //if (lblMsg.Text.Length > 56)
            {
                string[] info = lblMsg.Text.Split(new char[] { ',' });
                StringBuilder sb = new StringBuilder();

                foreach (var item in info)
                {
                    sb.AppendLine(item);
                }

                toolTip1.SetToolTip(lblMsg, sb.ToString());
            }
        }

        private void lblMsg_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = m_defaultCursor;
            lblMsg.ForeColor = this.ForeColor;
        }
    }
}
