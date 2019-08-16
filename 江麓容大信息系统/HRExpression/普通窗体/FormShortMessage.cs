using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TaskManagementServer;
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;
using TaskServerModule;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 短信信息界面
    /// </summary>
    public partial class FormShortMessage : Form
    {
        /// <summary>
        /// 为短信业务操作的服务接口
        /// </summary>
        IShowMessageServer m_shortMsgServer = TaskObjectFactory.GetOperator<IShowMessageServer>();

        /// <summary>
        /// 短信数据
        /// </summary>
        View_Task_ShortMessage m_shortMsg = new View_Task_ShortMessage();

        /// <summary>
        /// 短信数据
        /// </summary>
        public View_Task_ShortMessage ShortMessage
        {
            get { return m_shortMsg; }
        }

        /// <summary>
        /// 是否发生改变的标志
        /// </summary>
        private bool m_changeFlag = false;

        /// <summary>
        /// 信息是否发生
        /// </summary>
        public bool ChangeFlag
        {
            get { return m_changeFlag; }
        }

        ///// <summary>
        ///// 操作模式
        ///// </summary>
        //public enum OperateMode
        //{
        //    /// <summary>
        //    /// 查看模式
        //    /// </summary>
        //    View,

        //    /// <summary>
        //    /// 更新模式
        //    /// </summary>
        //    Update
        //};

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FormShortMessage()
        {
            InitializeComponent();

            dtpkSendTime.Value = ServerTime.Time;

            cmbShortMsgType.DataSource = m_shortMsgServer.GetShortMsgType();
            cmbShortMsgType.DisplayMember = "TypeName";
            cmbShortMsgType.ValueMember = "TypeID";
            cmbShortMsgType.SelectedIndex = 0;

            AuthorityControl();

            InitForm(null);
        }

        ///// <summary>
        ///// 构造函数（带要显示信息）
        ///// </summary>
        ///// <param name="operateMode">操作模式</param>
        ///// <param name="shortMsg">要显示的信息</param>
        //public FormShortMessage(OperateMode operateMode, View_Task_ShortMessage shortMsg)
        //{
        //    InitializeComponent();

        //    if (operateMode == OperateMode.Update)
        //    {
        //        AuthorityControl();
        //    }
        //    else
        //    {
        //        toolStrip.Visible = false;
        //    }

        //    InitForm(shortMsg);
        //}

        /// <summary>
        /// 权限控制
        /// </summary>
        void AuthorityControl()
        {
            if (!BasicInfo.IsFuzzyContainsRoleName("短信发布员"))
            {
                toolStrip.Visible = false;
            }
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="row">要显示的信息</param>
        void InitForm(View_Task_ShortMessage shortMsg)
        {
            if (shortMsg != null)
            {
                m_shortMsg = shortMsg;

                RefreshData(shortMsg);
            }
            else
            {
                btnNew.PerformClick();
            }
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="data">要显示的数据</param>
        private void RefreshData(View_Task_ShortMessage shortMsg)
        {
            txtContent.Text = m_shortMsg.短信内容;

            dtpkSendTime.Value = m_shortMsg.发送时间;
            dtpkSendTime.Checked = true;

            dataGridView1.Rows.Clear();
            dataGridView1.Rows.Add(new object[] { shortMsg.接收人姓名, shortMsg.接收人手机号 });
        }

        /// <summary>
        /// 检查数据是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckData()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtContent.Text))
            {
                MessageDialog.ShowPromptMessage("请输入短信内容");
                txtContent.Focus();
                return false;
            }

            if (dataGridView1.Rows.Count < 2)
            {
                MessageDialog.ShowPromptMessage("请选择短信接收人");
                return false;
            }

            if (!dtpkSendTime.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择短信发送时间");
                dtpkSendTime.Focus();
                return false;
            }
            else
            {
                if (dtpkSendTime.Value < ServerTime.Time.AddMinutes(5))
                {
                    MessageDialog.ShowPromptMessage("短信发送时间必须是当前时间5分钟后");
                    dtpkSendTime.Focus();
                    return false;
                }
            }

            if (m_shortMsg != null && m_shortMsg.编制人工号 != null && m_shortMsg.编制人工号 != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制人，不能进行此操作");
                return false;
            }

            return true;
        }

        private void FormShortMsg_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtContent.Text = "";
            txtRemark.Text = "";

            dtpkSendTime.Checked = false;
            dtpkSendTime.Value = DateTime.Now.Date;

            dataGridView1.Rows.Clear();

            chk带签名.Checked = false;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (Save(ShortMessageStatus.暂存))
            {
                MessageDialog.ShowPromptMessage("成功【保存】编制的短信信息");
            }
            else
            {
                MessageDialog.ShowErrorMessage("保存失败");
            }
        }

        private void 提交_Click(object sender, EventArgs e)
        {
            if (Save(ShortMessageStatus.待发送))
            {
                MessageDialog.ShowPromptMessage("成功【发布】编制的短信信息");
            }
            else
            {
                MessageDialog.ShowErrorMessage("发送失败");
            }
        }

        /// <summary>
        /// 保存短信
        /// </summary>
        /// <param name="status">保存后的短信状态</param>
        /// <returns>操作是否成功的标志</returns>
        private bool Save(ShortMessageStatus status)
        {
            try
            {
                if (!CheckData())
                {
                    return false;
                }

                List<View_Task_ShortMessage> lstMsg = new List<View_Task_ShortMessage>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["手机号码"].Value != null && !GlobalObject.GeneralFunction.IsNullOrEmpty(dataGridView1.Rows[i].Cells["手机号码"].Value.ToString()))
                    {
                        if (dataGridView1.Rows[i].Cells["姓名"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dataGridView1.Rows[i].Cells["姓名"].Value.ToString().Trim()))
                        {
                            MessageDialog.ShowErrorMessage("姓名不允许为空");
                            return false;
                        }
                    }

                    if (dataGridView1.Rows[i].Cells["手机号码"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dataGridView1.Rows[i].Cells["手机号码"].Value.ToString()))
                        continue;

                    View_Task_ShortMessage msg = new View_Task_ShortMessage();

                    if (chk带签名.Checked)
                    {
                        if (txtContent.Text[0] != ' ')
                        {
                            txtContent.Text = "    " + txtContent.Text;
                        }

                        msg.短信内容 = string.Format(@"亲爱的{0}:{1}{2}",
                            dataGridView1.Rows[i].Cells["姓名"].Value.ToString(),
                            System.Environment.NewLine,
                            txtContent.Text);
                    }
                    else
                    {
                        msg.短信内容 = txtContent.Text;
                    }

                    msg.发送时间 = dtpkSendTime.Value;
                    msg.接收人姓名 = dataGridView1.Rows[i].Cells["姓名"].Value.ToString();

                    msg.接收人手机号 = dataGridView1.Rows[i].Cells["手机号码"].Value.ToString();

                    msg.备注 = txtRemark.Text;

                    msg.短信类别 = cmbShortMsgType.Text;

                    lstMsg.Add(msg);
                }

                if (lstMsg.Count > 0)
                {
                    m_shortMsgServer.Add(lstMsg, status);

                    m_changeFlag = true;

                    return true;
                }
                else
                {
                    MessageDialog.ShowErrorMessage("没有获取到要发送的人员信息");
                    return false;
                }
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
                return false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// 检查选择的人员是否能发送短信
        /// </summary>
        /// <param name="data">要检查的人员列表</param>
        /// <returns>人员有效返回true</returns>
        private bool CheckSelectedPersonnel(object data)
        {
            List<PersonnelBasicInfo> lstData = data as List<PersonnelBasicInfo>;

            Dictionary<string, string> dicMobileNo = m_shortMsgServer.GetMobileNo(lstData);

            foreach (KeyValuePair<string, string> userInfo in dicMobileNo)
            {
                dataGridView1.Rows.Add(new object[] { userInfo.Key, userInfo.Value });
            }

            return true;
        }

        /// <summary>
        /// 选择短信发送人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectPersonnel_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel2 form = new FormSelectPersonnel2();

            form.OnCheckSelectedPersonnel += new GlobalObject.DelegateCollection.CheckSelectedPersonnel(CheckSelectedPersonnel);

            form.ShowDialog();
        }

        /// <summary>
        /// 用于确定数据控件中的检查框值已经更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView1.IsCurrentCellDirty)
            {
                dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
