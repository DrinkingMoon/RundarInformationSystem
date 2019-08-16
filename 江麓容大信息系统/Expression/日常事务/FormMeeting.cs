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

namespace Expression
{
    /// <summary>
    /// 会议信息界面
    /// </summary>
    public partial class FormMeeting : Form
    {
        /// <summary>
        /// 为日常会议提供业务操作的服务接口
        /// </summary>
        IMeetingServer m_meetingServer = TaskObjectFactory.GetOperator<IMeetingServer>();

        /// <summary>
        /// 会议数据
        /// </summary>
        MeetingData m_meetingData = new MeetingData();

        /// <summary>
        /// 会议数据
        /// </summary>
        public MeetingData MeetingData
        {
            get { return m_meetingData; }
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

        /// <summary>
        /// 操作模式
        /// </summary>
        public enum OperateMode
        {
            /// <summary>
            /// 查看模式
            /// </summary>
            View,

            /// <summary>
            /// 更新模式
            /// </summary>
            Update
        };

        /// <summary>
        /// 默认构造函数
        /// </summary>
        public FormMeeting()
        {
            InitializeComponent();

            AuthorityControl();

            InitForm(null);
        }

        /// <summary>
        /// 构造函数（带要显示信息）
        /// </summary>
        /// <param name="operateMode">操作模式</param>
        /// <param name="row">要显示的信息</param>
        public FormMeeting(OperateMode operateMode, DataRow row)
        {
            InitializeComponent();

            if (operateMode == OperateMode.Update)
            {
                AuthorityControl();
            }
            else
            {
                toolStrip.Visible = false;
            }

            InitForm(row);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        void AuthorityControl()
        {
            if (!BasicInfo.IsFuzzyContainsRoleName("会议发布员"))
            {
                toolStrip.Visible = false;
            }
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        /// <param name="row">要显示的信息</param>
        void InitForm(DataRow row)
        {
            cmdImportance.Items.AddRange(Enum.GetNames(typeof(TaskImportance)));

            if (row != null)
            {
                MeetingData data = m_meetingServer.GetMeetingData(row);

                m_meetingData = data;

                RefreshData(data);
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
        private void RefreshData(MeetingData data)
        {
            lblBillNo.Text = m_meetingData.会议编号;
            lblBillStatus.Text = m_meetingData.会议状态.ToString();
            txtContent.Text = m_meetingData.会议正文;
            txtTitle.Text = m_meetingData.标题;
            txt主持人.Text = m_meetingData.主持人姓名;
            txt记录员.Text = m_meetingData.记录人姓名;

            cmd与会人员.Items[0] = m_meetingData.与会人员;
            cmd与会人员.SelectedIndex = 0;

            cmd与会资源.Items[0] = m_meetingData.会议资源;
            cmd与会资源.SelectedIndex = 0;

            cmdImportance.Text = m_meetingData.重要性.ToString();

            dtpkBeginTime.Value = m_meetingData.开始时间;
            dtpkBeginTime.Checked = true;

            dtpkEndTime.Value = m_meetingData.结束时间;
            dtpkEndTime.Checked = true;

            numMinute.Value = m_meetingData.提醒提前分钟数;

            switch (m_meetingData.提醒方式)
            {
                case MeetingAlarmMode.短信及消息框提醒:
                    radio短信及消息框.Checked = true;
                    break;
                case MeetingAlarmMode.仅短信提醒:
                    radio短信.Checked = true;
                    break;
                case MeetingAlarmMode.仅消息框提醒:
                    radio消息框.Checked = true;
                    break;
            }
        }

        /// <summary>
        /// 从对话框获取数据
        /// </summary>
        private void GetDataFromDialog()
        {
            m_meetingData.会议编号 = lblBillNo.Text;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(lblBillStatus.Text))
            {
                m_meetingData.会议状态 = MeetingStatus.待发;
            }
            else
            {
                m_meetingData.会议状态 = (MeetingStatus)Enum.Parse(typeof(MeetingStatus), lblBillStatus.Text);
            }

            m_meetingData.会议正文 = txtContent.Text;
            m_meetingData.标题 = txtTitle.Text;

            if (txt主持人.DataResult != null)
            {
                m_meetingData.主持人工号 = txt主持人["工号"].ToString();
                m_meetingData.主持人姓名 = txt主持人.Text;
            }

            if (txt记录员.DataResult != null)
            {
                m_meetingData.记录人工号 = txt记录员["工号"].ToString();
                m_meetingData.记录人姓名 = txt记录员.Text;
            }

            m_meetingData.重要性 = (TaskImportance)Enum.Parse(typeof(TaskImportance), cmdImportance.Text);
            m_meetingData.开始时间 = dtpkBeginTime.Value;
            m_meetingData.结束时间 = dtpkEndTime.Value;
            m_meetingData.提醒提前分钟数 = Convert.ToInt32(numMinute.Value);
            m_meetingData.事务类别 = DailyWorkType.会议;

            if (radio短信及消息框.Checked)
            {
                m_meetingData.提醒方式 = MeetingAlarmMode.短信及消息框提醒;
            }
            else if (radio短信.Checked)
            {
                m_meetingData.提醒方式 = MeetingAlarmMode.仅短信提醒;
            }
            else
            {
                m_meetingData.提醒方式 = MeetingAlarmMode.仅消息框提醒;
            }
        }

        /// <summary>
        /// 检查数据是否正确
        /// </summary>
        /// <returns>正确返回true</returns>
        private bool CheckData()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtTitle.Text))
            {
                MessageDialog.ShowPromptMessage("请输入会议标题");
                txtTitle.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txt主持人.Text))
            {
                MessageDialog.ShowPromptMessage("请选择会议主持人");
                txt主持人.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txt记录员.Text))
            {
                MessageDialog.ShowPromptMessage("请选择会议记录员");
                txt记录员.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmd与会人员.Items[0].ToString()))
            {
                MessageDialog.ShowPromptMessage("请选择与会人员");
                cmd与会人员.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmd与会资源.Items[0].ToString()))
            {
                MessageDialog.ShowPromptMessage("请选择会议资源");
                cmd与会资源.Focus();
                return false;
            }

            if (cmdImportance.SelectedIndex < 0)
            {
                MessageDialog.ShowPromptMessage("请选择会议重要性");
                cmdImportance.Focus();
                return false;
            }

            if (!dtpkBeginTime.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择会议开始时间");
                dtpkBeginTime.Focus();
                return false;
            }

            if (!radio消息框.Checked && numMinute.Value < 5)
            {
                MessageDialog.ShowPromptMessage("需要发送短信时，必须在会议开始时间 5 分钟 前发布消息");
                numMinute.Focus();
                return false;
            }

            if (!dtpkEndTime.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择会议结束时间");
                dtpkEndTime.Focus();
                return false;
            }

            if (dtpkBeginTime.Value >= dtpkEndTime.Value)
            {
                MessageDialog.ShowPromptMessage("结束时间必须 > 开始时间");
                dtpkEndTime.Focus();
                return false;
            }

            if (m_meetingData != null && m_meetingData.创建人工号 != null && m_meetingData.创建人工号 != BasicInfo.LoginID)
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制人，不能进行此操作");
                return false;
            }

            return true;
        }

        private void FormMeeting_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 检查选择的人员是否能参加会议
        /// </summary>
        /// <param name="data">要检查的人员列表</param>
        /// <returns>人员有效返回true</returns>
        private bool CheckSelectedPersonnel(object data)
        {
            List<PersonnelBasicInfo> lstData = data as List<PersonnelBasicInfo>;
            string error;

            if (m_meetingServer.CheckParticipants(lstData, dtpkBeginTime.Value, dtpkEndTime.Value, out error))
            {
                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < lstData.Count; i++)
                {
                    sb.AppendFormat("{0},", lstData[i].姓名);
                }

                m_meetingData.与会人员 = sb.ToString(0, sb.Length - 1);
                m_meetingData.与会人员对象集 = lstData;

                cmd与会人员.Items[0] = m_meetingData.与会人员;
                return true;
            }
            else
            {
                MessageDialog.ShowPromptMessage(error);
                return false;
            }
        }

        private void cmd与会人员_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmd与会人员.SelectedIndex > 0)
            {
                if (!dtpkBeginTime.Checked || !dtpkEndTime.Checked)
                {
                    MessageDialog.ShowPromptMessage("请先确定会议起止时间后再选择与会人员");
                    cmd与会人员.SelectedIndex = 0;
                    return;
                }

                FormSelectPersonnel2 form = new FormSelectPersonnel2();

                form.OnCheckSelectedPersonnel += new GlobalObject.DelegateCollection.CheckSelectedPersonnel(CheckSelectedPersonnel);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    cmd与会人员.SelectedIndex = 0;
                }
            }
        }

        private void cmd与会资源_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmd与会资源.SelectedIndex > 0)
            {
                if (!dtpkBeginTime.Checked || !dtpkEndTime.Checked)
                {
                    MessageDialog.ShowPromptMessage("请先确定会议起止时间后再选择会议资源");
                    cmd与会资源.SelectedIndex = 0;
                    return;
                }

                FormQueryInfo dialog = QueryInfoDialog.GetMeetingResourceDialog(dtpkBeginTime.Value, dtpkEndTime.Value);

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    if (!(bool)dialog.GetDataItem("是共享类资源") && dialog["资源空闲"] == "使用中")
                    {
                        MessageDialog.ShowPromptMessage(
                            string.Format("【{0}】资源已经被占用，只允许选择状态为“空闲”的资源", dialog["资源名称"]));

                        cmd与会资源.SelectedIndex = 0;

                        return;
                    }

                    StringBuilder sb = new StringBuilder();

                    sb.Append(dialog["资源名称"]);

                    cmd与会资源.Items[0] = sb.ToString(0, sb.Length);
                    cmd与会资源.SelectedIndex = 0;

                    m_meetingData.会议资源 = cmd与会资源.Items[0].ToString();

                    View_PRJ_Resource resource = new View_PRJ_Resource();

                    resource.资源编号 = (int)dialog.GetDataItem("资源编号");
                    resource.资源类别名称 = dialog["资源类别名称"];
                    resource.资源名称 = dialog["资源名称"];

                    m_meetingData.会议资源对象集 = new List<View_PRJ_Resource>();
                    m_meetingData.会议资源对象集.Add(resource);
                }
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            radio短信及消息框.Checked = true;

            lblBillNo.Text = "系统自动分配";
            lblBillStatus.Text = "待发";
            txtContent.Text = "";
            txtTitle.Text = "";
            txt主持人.Text = "";
            txt记录员.Text = "";
            cmd与会人员.Items[0] = "";
            cmd与会资源.Items[0] = "";
            cmdImportance.Text = TaskImportance.普通.ToString();

            dtpkBeginTime.Checked = false;
            dtpkBeginTime.Value = DateTime.Now.Date;

            dtpkEndTime.Checked = false;
            dtpkEndTime.Value = DateTime.Now.Date;

            numMinute.Value = 30m;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!CheckData())
                {
                    return;
                }

                GetDataFromDialog();

                m_meetingServer.Save(m_meetingData);

                MessageDialog.ShowPromptMessage("成功保存编制的会议信息");

                lblBillNo.Text = m_meetingData.会议编号;

                lblBillStatus.Text = m_meetingData.会议状态.ToString();

                m_changeFlag = true;
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void 提交_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(lblBillNo.Text) || lblBillNo.Text == "系统自动分配")
            {
                MessageDialog.ShowPromptMessage("请保存后再进行此操作");
                return;
            }

            if (!CheckData())
            {
                return;
            }

            if (lblBillStatus.Text != MeetingStatus.已发.ToString())
            {
                if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage("发布前请确认已经保存您所做的修改，没有修改则可以直接发布。是否继续发布？"))
                {
                    return;
                }

                try
                {
                    m_meetingServer.UpdateStatus(lblBillNo.Text, MeetingStatus.已发);

                    m_meetingData.会议状态 = MeetingStatus.已发;

                    lblBillStatus.Text = m_meetingData.会议状态.ToString();

                    if (radio短信.Checked || radio短信及消息框.Checked)
                    {
                        MessageDialog.ShowPromptMessage("短信将会在提醒时间发送到所有与会人员");
                    }

                    m_changeFlag = true;
                }
                catch (Exception exce)
                {
                    if (exce.Message.Contains("将截断字符串或二进制数据"))
                    {
                        MessageDialog.ShowErrorMessage("会议名称太长无法发送短信，请改短一些试试，记得先保存后再发布");
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(exce.Message);
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("当前会议已经是发布状态，请不要重复操作。");
            }
        }

        private void 撤销_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != MeetingStatus.已发.ToString())
            {
                MessageDialog.ShowPromptMessage("会议还没有发布无法撤销");
                return;
            }

            if (DialogResult.Yes != MessageDialog.ShowEnquiryMessage("您真的要撤销当前会议记录吗？"))
            {
                return;
            }

            try
            {
                m_meetingServer.UpdateStatus(lblBillNo.Text, MeetingStatus.撤销);

                m_meetingData.会议状态 = MeetingStatus.撤销;

                lblBillStatus.Text = m_meetingData.会议状态.ToString();

                m_changeFlag = true;
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cmd与会人员_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(cmd与会人员, cmd与会人员.Items[0].ToString());
        }

        private void cmd与会资源_MouseEnter(object sender, EventArgs e)
        {
            this.toolTip1.SetToolTip(cmd与会资源, cmd与会资源.Items[0].ToString());
        }

        private void dtpkBeginTime_ValueChanged(object sender, EventArgs e)
        {
            dtpkEndTime.Value = new DateTime(dtpkBeginTime.Value.Year, dtpkBeginTime.Value.Month, dtpkBeginTime.Value.Day,
                dtpkEndTime.Value.Hour, dtpkEndTime.Value.Minute, dtpkEndTime.Value.Second);
        }
    }
}
