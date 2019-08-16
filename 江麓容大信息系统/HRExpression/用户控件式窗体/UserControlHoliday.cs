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
    /// 节假日信息主界面
    /// </summary>
    public partial class UserControlHoliday : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 应上班小时数
        /// </summary>
        double m_workHours = 7.5;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 节假日管理类
        /// </summary>
        IHolidayServer m_holidayServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IHolidayServer>();

        /// <summary>
        /// 部门
        /// </summary>
        string deptName;

        public UserControlHoliday(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            menuStrip1.Visible = true;

            DataTable holidayTypeDt = m_holidayServer.GetHolidayType();

            if (holidayTypeDt != null && holidayTypeDt.Rows.Count > 0)
            {
                for (int i = 0; i < holidayTypeDt.Rows.Count; i++)
                {
                    cmbHolidayType.Items.Add(holidayTypeDt.Rows[i]["节假日名称"].ToString());
                }
            }

            RefreshControl();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void ClearControl()
        {
            cmbApplicableSex.SelectedIndex = -1;
            cmbApplicableDeptCode.Items.Clear();

            txtRemark.Text = "";

            dtpStarTime.Value = ServerTime.Time;
            dtpEndTime.Value = ServerTime.Time;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            DataTable dt = m_holidayServer.GetHoliday();

            dataGridView1.DataSource = dt;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["编号"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

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
        }

        private void 设置节假日类别ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormHolidayType frm = new FormHolidayType();
            frm.ShowDialog();

            cmbApplicableDeptCode.Items.Clear();

            DataTable holidayTypeDt = m_holidayServer.GetHolidayType();

            if (holidayTypeDt != null && holidayTypeDt.Rows.Count > 0)
            {
                for (int i = 0; i < holidayTypeDt.Rows.Count; i++)
                {
                    cmbHolidayType.Items.Add(holidayTypeDt.Rows[i]["节假日名称"].ToString());
                }
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlHoliday_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 选择部门
        /// </summary>
        private void btnFindPrincipal_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("部门");

            if (cmbApplicableDeptCode.Tag != null)
            {
                form.SelectedDept = (cmbApplicableDeptCode.Tag as List<View_HR_Dept>);
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                cmbApplicableDeptCode.Items.Clear();

                if (form.SelectedDept != null && form.SelectedDept.Count > 0)
                {
                    if (form.Count == "全部")
                    {
                        cmbApplicableDeptCode.Items.Add("全部");
                        cmbApplicableDeptCode.SelectedIndex = 0;
                        deptName = "全部";
                    }
                    else
                    {
                        deptName = "";
                        cmbApplicableDeptCode.Items.AddRange((from r in form.SelectedDept select r.部门名称).ToArray());
                        cmbApplicableDeptCode.Tag = form.SelectedDept;
                        cmbApplicableDeptCode.SelectedIndex = 0;

                        for (int i = 0; i < cmbApplicableDeptCode.Items.Count; i++)
                        {
                            deptName += cmbApplicableDeptCode.Items[i].ToString() + ";";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            if (Convert.ToInt32(billNo) > 0)
            {
                string strColName = "";

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    if (col.Visible)
                    {
                        strColName = col.Name;
                        break;
                    }
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["编号"].Value.ToString() == billNo)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>完整返回True，否则返回False</returns>
        bool CheckControl()
        {
            if (dtpStarTime.Value > dtpEndTime.Value)
            {
                MessageDialog.ShowPromptMessage("起始时间大于结束时间，请确认！");
                return false;
            }

            if (cmbHolidayType.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择节假日类别");
                return false;
            }

            if (cmbApplicableDeptCode.Items.Count <= 0)
            {
                MessageDialog.ShowPromptMessage("请选择适用部门！");
                return false;
            }

            if (cmbApplicableSex.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择适用性别");
                return false;
            }

            return true;
        }
        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            HR_Holiday holiday = new HR_Holiday();

            holiday.HolidayTypeID = m_holidayServer.GetHolidayType(cmbHolidayType.Text);
            holiday.ApplicableDeptCode = deptName;
            holiday.ApplicableSex = cmbApplicableSex.Text;
            holiday.Days = Convert.ToDecimal(txtDays.Text);
            holiday.BeginTime = dtpStarTime.Value;
            holiday.EndTime = dtpEndTime.Value;
            holiday.Remark = txtRemark.Text;
            holiday.Recorder = BasicInfo.LoginID;
            holiday.RecordTime = ServerTime.Time;

            if (!m_holidayServer.AddHoliday(holiday, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
            PositioningRecord("0");
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的一行数据！");
                return;
            }

            if (!CheckControl())
            {
                return;
            }

            HR_Holiday holiday = new HR_Holiday();

            holiday.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value);
            holiday.HolidayTypeID = m_holidayServer.GetHolidayType(cmbHolidayType.Text);
            holiday.ApplicableDeptCode = deptName;
            holiday.ApplicableSex = cmbApplicableSex.Text;
            holiday.Days = Convert.ToDecimal(txtDays.Text);
            holiday.BeginTime = dtpStarTime.Value;
            holiday.EndTime = dtpEndTime.Value;
            holiday.Remark = txtRemark.Text;
            holiday.Recorder = BasicInfo.LoginID;
            holiday.RecordTime = ServerTime.Time;

            if (!m_holidayServer.UpdateHoliday(holiday, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
            PositioningRecord(holiday.ID.ToString());
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count < 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的一行数据！");
                return;
            }

            if (MessageBox.Show("您确定要删除【" + dataGridView1.CurrentRow.Cells["节假日名称"].Value.ToString() + "】的信息吗？", "消息", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_holidayServer.DeleteHoliday(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value), out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            RefreshControl();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            ClearControl();

            cmbHolidayType.Text = dataGridView1.CurrentRow.Cells["节假日名称"].Value.ToString();
            cmbApplicableSex.Text = dataGridView1.CurrentRow.Cells["适用性别"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            dtpStarTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["开始时间"].Value);
            dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["结束时间"].Value);
            deptName = dataGridView1.CurrentRow.Cells["适用部门"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["适用部门"].Value.ToString() == "全部")
            {
                cmbApplicableDeptCode.Items.Add("全部");
            }
            else
            {
                string[] dept = dataGridView1.CurrentRow.Cells["适用部门"].Value.ToString().Split(';');

                for (int i = 0; i < dept.Count(); i++)
                {
                    cmbApplicableDeptCode.Items.Add(dept[i]);
                }
            }

            cmbApplicableDeptCode.SelectedIndex = 0;
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
        }

        private void dtpStarTime_ValueChanged(object sender, EventArgs e)
        {
            if ((dtpEndTime.Value - dtpStarTime.Value).Hours > m_workHours && (dtpEndTime.Value - dtpStarTime.Value).Days == 0)
            {
                txtDays.Text = "1";
            }
            else if ((dtpEndTime.Value - dtpStarTime.Value).Days > 0)
            {
                txtDays.Text = ((dtpEndTime.Value - dtpStarTime.Value).Days + 1).ToString();
            }
            else
            {
                txtDays.Text = "0.5";
            }
        }

        private void dtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            if ((dtpEndTime.Value - dtpStarTime.Value).Hours > m_workHours && (dtpEndTime.Value - dtpStarTime.Value).Days == 0)
            {
                txtDays.Text = "1";
            }
            else if ((dtpEndTime.Value - dtpStarTime.Value).Days > 0)
            {
                txtDays.Text = ((dtpEndTime.Value - dtpStarTime.Value).Days + 1).ToString();
            }
            else
            {
                txtDays.Text = "0.5";
            }
        }

        private void UserControlHoliday_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }
    }
}
