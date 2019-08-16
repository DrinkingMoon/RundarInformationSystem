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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 更新通知
    /// </summary>
    public partial class FormUpdateNotice : Form
    {
        /// <summary>
        /// 获取通知类消息数据库操作接口
        /// </summary>
        IFlowNoticeManagement m_flowNoticeManagement = PlatformFactory.GetObject<IFlowNoticeManagement>();

        /// <summary>
        /// 通知内容对象
        /// </summary>
        Flow_Notice m_notice;

        /// <summary>
        /// 原设置的接收人
        /// </summary>
        string m_oldReceivedPersonal;

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="source">通知来源</param>
        /// <param name="notice">如果此参数不为空表示是更新通知，否则为添加</param>
        public FormUpdateNotice(NoticeSource source, Flow_Notice notice)
        {
            InitializeComponent();

            cmbSource.Enabled = false;
            cmbSource.Items.AddRange(Enum.GetNames(typeof(NoticeSource)));
            cmbSource.Text = source.ToString();
            m_notice = notice;

            if (notice != null)
            {
                this.Text = "更新通知";

                cmbPriority.Text = notice.优先级;
                m_oldReceivedPersonal = txtReceivedPersonal.Text = notice.接收人;
                txtTitle.Text = notice.标题;

                txtContent.Text = notice.内容.Substring(notice.内容.IndexOf("：") + 1);
            }
            else
            {
                cmbPriority.SelectedIndex = 0;
            }
        }

        private void FormUpdateNotice_Load(object sender, EventArgs e)
        {
        }

        /// <summary>
        /// 检查输入数据是否正确
        /// </summary>
        /// <returns></returns>
        bool CheckData()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtReceivedPersonal.Text))
            {
                txtReceivedPersonal.Focus();
                MessageDialog.ShowPromptMessage("接收人不能为空！");
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtTitle.Text))
            {
                txtTitle.Focus();
                MessageDialog.ShowPromptMessage("标题不能为空！");
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtContent.Text))
            {
                txtContent.Focus();
                MessageDialog.ShowPromptMessage("内容不能为空！");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 发布通知
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSend_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            try
            {
                if (m_notice == null)
                {
                    m_notice = new Flow_Notice();
                    m_notice.发送人 = BasicInfo.LoginID;
                }

                string[] users = txtReceivedPersonal.Text.Split(new char[] { ',' });

                foreach (var user in users)
                {
                    Flow_Notice notice = new Flow_Notice();

                    notice.发送人 = BasicInfo.LoginID;
                    notice.接收人 = user;
                    notice.来源 = cmbSource.Text;
                    notice.标题 = txtTitle.Text;
                    notice.优先级 = cmbPriority.Text;
                    notice.内容 = txtContent.Text;
                    notice.状态 = NoticeStatus.未读.ToString();

                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(m_oldReceivedPersonal))
                    {
                        m_flowNoticeManagement.SendNotice(notice);
                    }
                    else if (user == m_oldReceivedPersonal)
                    {
                        notice.序号 = m_notice.序号;
                        m_flowNoticeManagement.UpdateNotice(BasicInfo.LoginID, notice);
                    }
                }

                txtReceivedPersonal.Text = "";
                txtTitle.Text = "";
                txtContent.Text = "";
                MessageDialog.ShowPromptMessage("发送成功！");
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        /// <summary>
        /// 退出窗体
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 选择接收人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPersonal_Click(object sender, EventArgs e)
        {
            FormSelectUsers form = new FormSelectUsers(txtReceivedPersonal.Text.Trim());
            form.ShowDialog();

            if (form.SelectedUsers != null)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var user in form.SelectedUsers)
                {
                    sb.Append(user.登录名);
                    sb.Append(",");
                }

                if (sb.Length > 0)
                {
                    sb.Remove(sb.Length - 1, 1);
                }

                txtReceivedPersonal.Text = sb.ToString();
            }
        }
    }
}
