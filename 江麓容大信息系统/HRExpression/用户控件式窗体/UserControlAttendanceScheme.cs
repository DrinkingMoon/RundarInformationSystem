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
using GlobalObject;
using ServerModule;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工考勤方案设置界面
    /// </summary>
    public partial class UserControlAttendanceScheme : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 适用部门
        /// </summary>
        string deptName;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        public UserControlAttendanceScheme(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            DataTable dt = m_attendanceServer.GetAttendanceScheme();

            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSchemeCode.Items.Add(dt.Rows[i]["考勤方案"].ToString());
                }
            }

            RefreshDataGridView();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlAttendanceScheme_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
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
                if (dataGridView1.Rows[i].Cells["员工编号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_attendanceServer.GetAttendanceSetting();

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;                
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

            dataGridView1.Refresh();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void txtProposer_OnCompleteSearch()
        {
            txtProposer.Text = txtProposer.DataResult["员工编号"].ToString();
            txtName.Text = txtProposer.DataResult["员工姓名"].ToString();
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

        private void 设置考勤toolStripButton1_Click(object sender, EventArgs e)
        {
            FormAttendanceScheme frm = new FormAttendanceScheme();
            frm.ShowDialog();

            DataTable dt = m_attendanceServer.GetAttendanceScheme();

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbSchemeCode.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbSchemeCode.Items.Add(dt.Rows[i]["考勤方案"].ToString());
                }
            }
        }

        private void 添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (txtProposer.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择员工");
                return;
            }

            if (cmbSchemeCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择考勤方案");
                return;
            }

            string[] schemeCode = cmbSchemeCode.Text.Split(' ');

            string strProposer = txtProposer.Text;

            if (!m_attendanceServer.AddAttendanceSetting(txtProposer.Text, schemeCode[0],cbSubsidize.Checked, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshDataGridView();
            PositioningRecord(strProposer);
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 批量添加toolStripButton1_Click(object sender, EventArgs e)
        {
            if (cmbApplicableDeptCode.Items.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择适用部门！");
                return;
            }

            if (cmbSchemeCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择考勤方案");
                return;
            }

            string[] schemeCode = cmbSchemeCode.Text.Split(' ');

            if (!m_attendanceServer.AddAttendanceSettingByDept(deptName, schemeCode[0],cbReplace.Checked,cbSubsidize.Checked, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("批量添加考勤成功！");
            RefreshDataGridView();
        }

        private void 修改toolStripButton2_Click(object sender, EventArgs e)
        {
            if (cmbSchemeCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择考勤方案");
                return;
            }

            string[] schemeCode = cmbSchemeCode.Text.Split(' ');

            string strProposer = txtProposer.Text;

            if (!m_attendanceServer.UpdateAttendanceSetting(txtProposer.Text, schemeCode[0],cbSubsidize.Checked, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshDataGridView();
            PositioningRecord(strProposer);
        }

        private void 批量修改toolStripButton2_Click(object sender, EventArgs e)
        {
            if (cmbSchemeCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择考勤方案");
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(deptName))
            {
                MessageDialog.ShowPromptMessage("请在【适用部门】中选择需要批量修改的部门");
                return;
            }

            string[] schemeCode = cmbSchemeCode.Text.Split(' ');

            if (!m_attendanceServer.UpdateAttendanceSettingByDept(deptName, schemeCode[0], cbSubsidize.Checked, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            MessageDialog.ShowPromptMessage("为【" + deptName + "】批量修改成功！");
            RefreshDataGridView();
        }

        private void 删除toolStripButton1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行");
                return;
            }

            if (MessageBox.Show("您确定要删除选中的数据吗？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                bool flag = false;

                List<string> workIDList = new List<string>();

                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    if (dataGridView1.CurrentRow.Cells["人员状态"].Value.ToString() == "在职")
                    {
                        flag = true;
                        continue;
                    }

                    workIDList.Add(dataGridView1.SelectedRows[i].Cells["员工编号"].Value.ToString());
                }

                if (!m_attendanceServer.DeleteAttendanceSetting(workIDList, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }

                if (flag)
                {
                    MessageDialog.ShowPromptMessage("选中数据中有在职员工，在职员工只能修改考勤不能删除！");
                }

                RefreshDataGridView();
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["考勤方案"].Value.ToString() == "")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtProposer.Text = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();
            cmbApplicableDeptCode.Text = dataGridView1.CurrentRow.Cells["部门"].Value.ToString();

            if (dataGridView1.CurrentRow.Cells["考勤方案"].Value.ToString() != "")
            {
                cmbSchemeCode.Text = dataGridView1.CurrentRow.Cells["考勤方案"].Value.ToString();
            }

            if (dataGridView1.CurrentRow.Cells["是否有餐补"].Value.ToString() != "")
            {
                cbSubsidize.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否有餐补"].Value);
            }
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
            //    e.RowBounds.Location.Y,
            //    dataGridView1.RowHeadersWidth - 4,
            //    e.RowBounds.Height);

            //TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            //    dataGridView1.RowHeadersDefaultCellStyle.Font,
            //    rectangle,
            //    dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
            //    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void UserControlAttendanceScheme_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }
    }
}
