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
using ServerModule;
using Expression;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 设置岗位主界面
    /// </summary>
    public partial class UserControlOperatingPost : Form
    {
        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_PostServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        public UserControlOperatingPost(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtPostName.Text = "";
            txtPostPrinciple.Text = "";
            txtPostStatement.Text = "";
            txtRemark.Text = "";
            cmbPostType.SelectedIndex = -1;
            cmbPostType.Items.Clear();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UserControlOperatingPost_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);

            RefreshControl();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlOperatingPost_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            IQueryable<View_HR_Dept> m_findDepartment;

            if (m_departmentServer.GetAllDeptInfo(out m_findDepartment, out error))
            {
                foreach (var item in m_findDepartment)
                {
                    cmbDept.Items.Add(item.部门名称);
                }
            }

            DataTable dt = new DataTable();

            if (BasicInfo.DeptCode != "ZB03")
            {
                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
                {
                    dt = m_PostServer.GetOperatingPost(m_personnerServer.GetPersonnelInfo(BasicInfo.LoginID).WorkPost);
                }
                else 
                {
                    dt = m_PostServer.GetOperatingPost(null);
                }
            }
            else
            {
                dt = m_PostServer.GetOperatingPost(null);
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["岗位编号"].Visible = false;

            dataGridView1.Refresh();

            for (int i = 0; i < m_PostServer.GetPostType().Rows.Count; i++)
            {
                cmbPostType.Items.Add(m_PostServer.GetPostType().Rows[i]["职位类别"].ToString());
            }
            
            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            View_HR_OperatingPost operatePost = m_PostServer.GetOperatingPostByPostName(
                dataGridView1.CurrentRow.Cells["岗位名称"].Value.ToString());

            txtPostName.Text = operatePost.岗位名称;
            txtPostPrinciple.Text = operatePost.岗位规范;
            txtPostStatement.Text = operatePost.岗位说明;
            txtRemark.Text = operatePost.备注;
            cmbPostType.Text = operatePost.职位类别;
            cbCorePost.Checked = operatePost.是否核心岗位 == "是" ? true : false;
            cmbDept.Text = operatePost.所属部门;
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns></returns>
        bool CheckControl()
        {
            if (txtPostName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("填写岗位名称！");
                return false;
            }

            if (cmbDept.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择所属部门！");
                return false;
            }

            if (txtPostPrinciple.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("填写岗位规范！");
                return false;
            }

            if (txtPostStatement.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("填写岗位说明！");
                return false;
            }

            if (cmbPostType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("选择职位类别！");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            HR_OperatingPost operateOpst = new HR_OperatingPost();

            operateOpst.PostName = txtPostName.Text;
            operateOpst.PostPrinciple = txtPostPrinciple.Text;
            operateOpst.PostStatement = txtPostStatement.Text;
            operateOpst.PostTypeID = Convert.ToInt32(m_PostServer.GetPostTypeByTypeName(cmbPostType.SelectedItem.ToString()));
            operateOpst.IsCorePost = cbCorePost.Checked;
            operateOpst.Recorder = BasicInfo.LoginID;
            operateOpst.RecordTime = ServerTime.Time;
            operateOpst.Remark = txtRemark.Text;
            operateOpst.Dept = cmbDept.Text;

            if (!m_PostServer.AddPost(operateOpst, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void 职位类别toolStripButton1_Click(object sender, EventArgs e)
        {
            FormPostType frm = new FormPostType();
            frm.ShowDialog();

            RefreshControl();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("系统不允许同时修改多行数据!");
                return;
            }

            if (!CheckControl())
            {
                return;
            }

            HR_OperatingPost operateOpst = new HR_OperatingPost();

            operateOpst.PostID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["岗位编号"].Value);
            operateOpst.PostName = txtPostName.Text;
            operateOpst.PostPrinciple = txtPostPrinciple.Text;
            operateOpst.IsCorePost = cbCorePost.Checked;
            operateOpst.PostStatement = txtPostStatement.Text;
            operateOpst.PostTypeID = Convert.ToInt32(m_PostServer.GetPostTypeByTypeName(cmbPostType.SelectedItem.ToString()));
            operateOpst.Recorder = BasicInfo.LoginID;
            operateOpst.RecordTime = ServerTime.Time;
            operateOpst.Remark = txtRemark.Text;
            operateOpst.Dept = cmbDept.Text;

            if (!m_PostServer.UpdatePost(operateOpst, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            RefreshControl();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                int n = dataGridView1.SelectedRows.Count;
                int[] arrayID = new int[n];

                for (int i = 0; i < n; i++)
                {
                    arrayID[i] = Convert.ToInt32(dataGridView1.SelectedRows[i].Cells["岗位编号"].Value);
                }

                if (MessageBox.Show("您是否确定要删除岗位信息?", "消息", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayID.Length; i++)
                    {
                        if (!m_PostServer.DeletePost(arrayID[i], out error))
                        {
                            MessageDialog.ShowPromptMessage(error);
                            return;
                        }
                    }
                }
            }
            else
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["岗位编号"].Value);

                if (MessageBox.Show("您是否确定要删除" + dataGridView1.CurrentRow.Cells["岗位名称"].Value.ToString()
                    + "信息?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_PostServer.DeletePost(id, out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
                }
            }

            RefreshControl();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            IQueryable<View_HR_PersonnelArchive> query = m_personnerServer.GetPersonnelArchiveByPostName(dataGridView1.CurrentRow.Cells["岗位名称"].Value.ToString());

            FormViewData form = new FormViewData(query);
            form.ShowDialog();
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
