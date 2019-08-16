using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Service_Peripheral_HR;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 职称主界面
    /// </summary>
    public partial class UserControlJobTitle : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 职称信息管理类
        /// </summary>
        IJobTitleServer m_JobServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IJobTitleServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public UserControlJobTitle(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlJobTitle_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtJobCode.Text = dataGridView1.CurrentRow.Cells["职称编号"].Value.ToString();
            txtJobName.Text = dataGridView1.CurrentRow.Cells["职称名称"].Value.ToString();
            cbJobTitle.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否是外部职称"].Value.ToString());
        }

        private void UserControlJobTitle_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            DataTable dt = m_JobServer.GetJobTitle();
            dataGridView1.DataSource = dt;

            dataGridView1.Refresh();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtJobName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写职称名称！");
                return;
            }

            HR_JobTitle jobTitle = new HR_JobTitle();

            jobTitle.JobTitle = txtJobName.Text;
            jobTitle.Recorder = BasicInfo.LoginID;
            jobTitle.RecordTime = ServerTime.Time;
            jobTitle.IsInternalJobTitle = cbJobTitle.Checked;

            if (!m_JobServer.AddAndUpdateJobTitle(jobTitle, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                HR_JobTitle jobTitle = new HR_JobTitle();

                jobTitle.JobTitle = txtJobName.Text;
                jobTitle.JobTitleID = Convert.ToInt32(txtJobCode.Text);

                if (!m_JobServer.DeleteJobTitle(jobTitle, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                RefreshControl();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选中需要删除的行！");
                return;
            }
        }
    }
}
