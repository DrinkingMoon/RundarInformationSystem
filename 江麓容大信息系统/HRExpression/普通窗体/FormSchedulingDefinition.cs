using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Service_Peripheral_HR;
using GlobalObject;
using Expression;
using PlatformManagement;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 排班定义界面
    /// </summary>
    public partial class FormSchedulingDefinition : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 排班信息操作类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        public FormSchedulingDefinition()
        {
            InitializeComponent();

            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            DataTable dt = m_workSchedulingServer.GetWorkSchedulingDefinition();

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void ClearControl()
        {
            txtCode.Text = "";
            txtName.Text = "";
            txtRemark.Text = "";
            cbIsHoliday.Checked = false;

            foreach (Control cl in this.panel1.Controls)
            {
                if (cl is DateTimePicker)
                {
                    ((DateTimePicker)cl).Value = ServerTime.Time;
                }
            }

            txtCode.ReadOnly = false;
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        /// <summary>
        /// 检查输入的正确性
        /// </summary>
        /// <returns>正确返回true，否则返回False</returns>
        bool CheckControl()
        {
            if (txtCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入排班定义编码！");
                return false;
            }

            if (txtName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入排班定义名称！");
                return false;
            }

            if (txtRemark.Text.Trim() == "")
            {
                txtRemark.Text = " ";
            }

            return true;
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            HR_WorkSchedulingDefinition definition = new HR_WorkSchedulingDefinition();

            definition.Code = txtCode.Text;
            definition.Name = txtName.Text;
            definition.Remark = txtRemark.Text;
            definition.Recorder = BasicInfo.LoginID;
            definition.RecordTime = ServerTime.Time;
            definition.IsHoliday = cbIsHoliday.Checked;
            definition.EndOffsetDays = Convert.ToInt32(numDays.Value);

            if (!cbIsHoliday.Checked)
            {
                definition.BeginTime = dtpBeginTime.Value.ToShortTimeString();
                definition.EndTime = dtpEndTime.Value.ToShortTimeString();
                definition.PunchInBeginTime = dtpPunchInBeginTime.Value.ToShortTimeString();
                definition.PunchInEndTime = dtpPunchInEndTime.Value.ToShortTimeString();
                definition.PunchOutBeginTime = dtpPunchOutBeginTime.Value.ToShortTimeString();
                definition.PunchOutEndTime = dtpPunchOutEndTime.Value.ToShortTimeString();
            }

            if (!m_workSchedulingServer.AddDefinition(definition,out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void 修改toolStripButton2_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            HR_WorkSchedulingDefinition definition = new HR_WorkSchedulingDefinition();

            definition.Code = txtCode.Text;
            definition.Name = txtName.Text;
            definition.Remark = txtRemark.Text;
            definition.Recorder = BasicInfo.LoginID;
            definition.RecordTime = ServerTime.Time;
            definition.IsHoliday = cbIsHoliday.Checked;
            definition.EndOffsetDays = Convert.ToInt32(numDays.Value);

            if (!cbIsHoliday.Checked)
            {
                definition.BeginTime = dtpBeginTime.Value.ToShortTimeString();
                definition.EndTime = dtpEndTime.Value.ToShortTimeString();
                definition.PunchInBeginTime = dtpPunchInBeginTime.Value.ToShortTimeString();
                definition.PunchInEndTime = dtpPunchInEndTime.Value.ToShortTimeString();
                definition.PunchOutBeginTime = dtpPunchOutBeginTime.Value.ToShortTimeString();
                definition.PunchOutEndTime = dtpPunchOutEndTime.Value.ToShortTimeString();
            }

            if (!m_workSchedulingServer.AddDefinition(definition, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCode.Text = dataGridView1.CurrentRow.Cells["定义编码"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["定义名称"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            cbIsHoliday.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否休假"].Value);
            numDays.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["偏移天数"].Value);

            if (dataGridView1.CurrentRow.Cells["开始上班时间"].Value.ToString() != "")
            {
                dtpBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["开始上班时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["上班结束时间"].Value.ToString() != "")
            {
                dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上班结束时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["上班打卡起始时间"].Value.ToString() != "")
            {
                dtpPunchInBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上班打卡起始时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["上班打卡截止时间"].Value.ToString() != "")
            {
                dtpPunchInEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上班打卡截止时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["下班打卡起始时间"].Value.ToString() != "")
            {
                dtpPunchOutBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["下班打卡起始时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["下班打卡截止时间"].Value.ToString() != "")
            {
                dtpPunchOutEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["下班打卡截止时间"].Value);
            }

            txtCode.ReadOnly = true;
        }

        private void 删除toolStripButton1_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("您确定要删除选中的数据吗？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_workSchedulingServer.DeleteDefinition(txtCode.Text, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            RefreshControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }
    }
}
