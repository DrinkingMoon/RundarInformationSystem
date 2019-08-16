using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using Service_Peripheral_HR;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 设置请假类别界面
    /// </summary>
    public partial class UserControlLeaveType : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 请假类别编号
        /// </summary>
        string typeCode;

        /// <summary>
        /// 请假操作类
        /// </summary>
        ILeaveServer m_leaveServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILeaveServer>();

        public UserControlLeaveType(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            RefreshControl();
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
        /// 定位记录
        /// </summary>
        /// <param name="id">定位用的编号</param>
        public void PositioningRecord(string id)
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
                if (id != "")
                {
                    if (dataGridView1.Rows[i].Cells["请假类别编号"].Value.ToString() == id)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            DataTable dt = m_leaveServer.GetAllLeaveType();

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }

            DataTable dtLeaveType = m_leaveServer.GetLeaveTypeByCode(null);

            if (dtLeaveType != null && dtLeaveType.Rows.Count > 0)
            {
                for (int i = 0; i < dtLeaveType.Rows.Count; i++)
                {
                    cmbParentTypeCode.Items.Add(dtLeaveType.Rows[i]["请假类别"].ToString());
                }
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

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlLeaveType_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void ClearControl()
        {
            txtRemark.Text = "";
            txtTypeCode.Text = "";
            txtTypeName.Text = "";
            numMaxHours.Value = 0;
            numMaxTimes.Value = 0;
            numMinHours.Value = 0;
            cbIncludeHoliday.Checked = false;
            cbPaidLeave.Checked = false;
            cmbLeaveMode.SelectedIndex = -1;
            cmbParentTypeCode.Items.Clear();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtTypeCode.Text = dataGridView1.CurrentRow.Cells["请假类别编号"].Value.ToString();
            txtTypeName.Text = dataGridView1.CurrentRow.Cells["请假类别名称"].Value.ToString();
            numMinHours.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["允许的最小小时数"].Value);
            numMaxHours.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["允许的最大小时数"].Value);
            numMaxTimes.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["允许的最多次数"].Value);
            cbIncludeHoliday.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否包含休息日"].Value);
            cbPaidLeave.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否带薪"].Value);
            cmbLeaveMode.Text = dataGridView1.CurrentRow.Cells["请假模式"].Value.ToString();
            cbNeedAnnex.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否需附件证明"].Value);
            chbIsDelete.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["禁用"].Value);

            DataTable dt = m_leaveServer.GetLeaveTypeByCode(dataGridView1.CurrentRow.Cells["父级类别"].Value.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbParentTypeCode.Text = dt.Rows[0]["请假类别"].ToString();
            }
        }

        /// <summary>
        /// 检查控件的完整性
        /// </summary>
        /// <returns>正确返回true，否则返回false</returns>
        bool CheckControl()
        {
            if (txtTypeCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写【请假类别编号】！");
                return false;
            }

            if (txtTypeName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写【请假类别名称】！");
                return false;
            }

            if (txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写【备注】！");
                return false;
            }

            if (cmbLeaveMode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择【请假模式】！");
                return false;
            }

            if (numMinHours.Value > numMaxHours.Value)
            {
                MessageDialog.ShowPromptMessage("允许的最小时间不能大于允许的最多小时数！");
                return false;
            }

            if (numMinHours.Value == 0)
            {
                if (MessageBox.Show("是否允许的最小小时数为0？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageDialog.ShowPromptMessage("请填写【允许的最小小时数】！");
                    return false;
                }
            }

            if (numMaxHours.Value == 0)
            {
                if (MessageBox.Show("是否允许的最大小时数为0？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageDialog.ShowPromptMessage("请填写【允许的最大小时数】！");
                    return false;
                }
            }

            if (numMaxTimes.Value == 0)
            {
                if (MessageBox.Show("是否允许的最多次数为0？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                {
                    MessageDialog.ShowPromptMessage("请填写【允许的最多次数】！");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取HR_LeaveType数据集
        /// </summary>
        /// <returns></returns>
        HR_LeaveType GetLeaveType()
        {
            string[] type = cmbParentTypeCode.Text.Split(' ');

            HR_LeaveType leaveType = new HR_LeaveType();

            leaveType.TypeCode = txtTypeCode.Text;
            leaveType.TypeName = txtTypeName.Text;
            leaveType.ParentTypeCode = type[0].ToString();
            leaveType.Remark = txtRemark.Text;
            leaveType.RecordTime = ServerTime.Time;
            leaveType.Recorder = BasicInfo.LoginID;
            leaveType.PaidLeave = cbPaidLeave.Checked;
            leaveType.MinHours = numMinHours.Value;
            leaveType.MaxTimes = Convert.ToInt32(numMaxTimes.Value);
            leaveType.MaxHours = numMaxHours.Value;
            leaveType.LeaveMode = cmbLeaveMode.Text;
            leaveType.IncludeHoliday = cbIncludeHoliday.Checked;
            leaveType.NeedAnnex = cbNeedAnnex.Checked;
            leaveType.DeleteFlag = chbIsDelete.Checked;

            return leaveType;
        }

        private void 提交toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_leaveServer.AddLeaveType(GetLeaveType(), out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
            PositioningRecord(dataGridView1.Rows[dataGridView1.Rows.Count-1].Cells["请假类别编号"].Value.ToString());
        }

        private void 修改toolStripButton1_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            if (!m_leaveServer.UpdateLeaveType(GetLeaveType(), out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
            PositioningRecord(typeCode);
        }

        private void 删除toolStripButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0 || dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的一行记录！");
                return;
            }

            if (!m_leaveServer.DeleteLeaveType(typeCode, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            typeCode = dataGridView1.CurrentRow.Cells["请假类别编号"].Value.ToString();
        }

        private void UserControlLeaveType_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }
    }
}
