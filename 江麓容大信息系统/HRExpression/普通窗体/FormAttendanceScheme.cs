using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 考勤方案设置界面
    /// </summary>
    public partial class FormAttendanceScheme : Form
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
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        public FormAttendanceScheme()
        {
            InitializeComponent();

            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            DataTable dt = m_attendanceServer.GetAllAttendanceScheme();

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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtSchemeCode.Text = dataGridView1.CurrentRow.Cells["考勤方案编码"].Value.ToString();
            txtSchemeName.Text = dataGridView1.CurrentRow.Cells["考勤方案名称"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            cmbAttendanceMode.Text = dataGridView1.CurrentRow.Cells["考勤模式"].Value.ToString();
            cbInPublicHoliday.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["法定假日上班是否自动算加班"].Value);

            if (dataGridView1.CurrentRow.Cells["上月开始时间"].Value.ToString() != "")
            {
                numBeginDateMonth.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["上月开始时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["下午工作开始时间"].Value.ToString() != "")
            {
                dtpBeginTimeAfternoon.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["下午工作开始时间"].Value);
                dtpBeginTimeAfternoon.Checked = true;
            }
            else
            {
                dtpBeginTimeAfternoon.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["上午工作开始时间"].Value.ToString() != "")
            {
                dtpBeginTimeMorning.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上午工作开始时间"].Value);
                dtpBeginTimeMorning.Checked = true;
            }
            else
            {
                dtpBeginTimeMorning.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["本月结束时间"].Value.ToString() != "")
            {
                numEndDateMonth.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["本月结束时间"].Value);
            }

            if (dataGridView1.CurrentRow.Cells["下午工作结束时间"].Value.ToString() != "")
            {
                dtpEndTimeAfternoon.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["下午工作结束时间"].Value);
                dtpEndTimeAfternoon.Checked = true;
            }
            else
            {
                dtpEndTimeAfternoon.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["上午工作结束时间"].Value.ToString() != "")
            {
                dtpEndTimeMorning.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上午工作结束时间"].Value);
                dtpEndTimeMorning.Checked = true;
            }
            else
            {
                dtpEndTimeMorning.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["上班打卡起始时间"].Value.ToString() != "")
            {
                dtpPunchInBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上班打卡起始时间"].Value);
                dtpPunchInBeginTime.Checked = true;
            }
            else
            {
                dtpPunchInBeginTime.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["上班打卡截止时间"].Value.ToString() != "")
            {
                dtpPunchInEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["上班打卡截止时间"].Value);
                dtpPunchInEndTime.Checked = true;
            }
            else
            {
                dtpPunchInEndTime.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["下班打卡起始时间"].Value.ToString() != "")
            {
                dtpPunchOutBeginTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["下班打卡起始时间"].Value);
                dtpPunchOutBeginTime.Checked = true;
            }
            else
            {
                dtpPunchOutBeginTime.Checked = false;
            }

            if (dataGridView1.CurrentRow.Cells["下班打卡截止时间"].Value.ToString() != "")
            {
                dtpPunchOutEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["下班打卡截止时间"].Value);
                dtpPunchOutEndTime.Checked = true;
            }
            else
            {
                dtpPunchOutEndTime.Checked = false;
            }

            txtSchemeCode.ReadOnly = true;
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>正确返回True，否则返回False</returns>
        bool CheckControl()
        {
            if (txtSchemeCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入考勤方案编码！");
                return false;
            }

            if (txtSchemeName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入考勤方案名称！");
                return false;
            }

            if (cmbAttendanceMode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择考勤模式！");
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

            HR_AttendanceScheme attendance = new HR_AttendanceScheme();

            attendance.AttendanceMode = cmbAttendanceMode.Text;
            attendance.AutoOvertimeInPublicHoliday = cbInPublicHoliday.Checked;

            if (cmbAttendanceMode.SelectedIndex != 3)
            {
                if (dtpBeginTimeAfternoon.Checked == false || dtpBeginTimeMorning.Checked == false
                    || dtpEndTimeAfternoon.Checked == false || dtpEndTimeMorning.Checked == false
                    || dtpPunchInBeginTime.Checked == false || dtpPunchInEndTime.Checked == false
                    || dtpPunchOutBeginTime.Checked == false || dtpPunchOutEndTime.Checked == false)
                {
                    MessageDialog.ShowPromptMessage("请填写上下班时间和上下班的打卡时间！");
                    return;
                }

                if (cmbAttendanceMode.SelectedIndex == 1)
                {
                    attendance.BeginDateOfLastMonth = Convert.ToInt32(numBeginDateMonth.Value);
                    attendance.EndDateOfThisMonth = Convert.ToInt32(numEndDateMonth.Value);
                }

                attendance.BeginTimeInTheAfternoon = dtpBeginTimeAfternoon.Value;
                attendance.BeginTimeInTheMorning = dtpBeginTimeMorning.Value;
                attendance.EndTimeInTheAfternoon = dtpEndTimeAfternoon.Value;
                attendance.EndTimeInTheMorning = dtpEndTimeMorning.Value;
                attendance.PunchInBeginTime = dtpPunchInBeginTime.Value;
                attendance.PunchInEndTime = dtpPunchInEndTime.Value;
                attendance.PunchOutBeginTime = dtpPunchOutBeginTime.Value;
                attendance.PunchOutEndTime = dtpPunchOutEndTime.Value;
            }

            attendance.Recorder = BasicInfo.LoginID;
            attendance.RecordTime = ServerTime.Time;
            attendance.Remark = txtRemark.Text;
            attendance.SchemeCode = txtSchemeCode.Text;
            attendance.SchemeName = txtSchemeName.Text;

            if (!m_attendanceServer.AddAttendanceScheme(attendance, out error))
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

            HR_AttendanceScheme attendance = new HR_AttendanceScheme();

            attendance.AttendanceMode = cmbAttendanceMode.Text;
            attendance.AutoOvertimeInPublicHoliday = cbInPublicHoliday.Checked;

            if (cmbAttendanceMode.SelectedIndex != 3)
            {
                if (dtpBeginTimeAfternoon.Checked == false || dtpBeginTimeMorning.Checked == false
                    || dtpEndTimeAfternoon.Checked == false || dtpEndTimeMorning.Checked == false
                    || dtpPunchInBeginTime.Checked == false || dtpPunchInEndTime.Checked == false
                    || dtpPunchOutBeginTime.Checked == false || dtpPunchOutEndTime.Checked == false)
                {
                    MessageDialog.ShowPromptMessage("请填写上下班时间和上下班的打卡时间！");
                    return;
                }

                if (cmbAttendanceMode.SelectedIndex == 1)
                {
                    attendance.BeginDateOfLastMonth = Convert.ToInt32(numBeginDateMonth.Value);
                    attendance.EndDateOfThisMonth = Convert.ToInt32(numEndDateMonth.Value);
                }

                attendance.BeginTimeInTheAfternoon = dtpBeginTimeAfternoon.Value;
                attendance.BeginTimeInTheMorning = dtpBeginTimeMorning.Value;
                attendance.EndTimeInTheAfternoon = dtpEndTimeAfternoon.Value;
                attendance.EndTimeInTheMorning = dtpEndTimeMorning.Value;
                attendance.PunchInBeginTime = dtpPunchInBeginTime.Value;
                attendance.PunchInEndTime = dtpPunchInEndTime.Value;
                attendance.PunchOutBeginTime = dtpPunchOutBeginTime.Value;
                attendance.PunchOutEndTime = dtpPunchOutEndTime.Value;
            }

            attendance.Recorder = BasicInfo.LoginID;
            attendance.RecordTime = ServerTime.Time;
            attendance.Remark = txtRemark.Text;
            attendance.SchemeCode = txtSchemeCode.Text;
            attendance.SchemeName = txtSchemeName.Text;

            if (!m_attendanceServer.AddAttendanceScheme(attendance, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void 删除toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count != 1)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的一行数据");
                return;
            }

            if (!m_attendanceServer.DeleteAttendanceScheme(txtSchemeCode.Text, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            txtSchemeCode.ReadOnly = false;
        }
    }
}
