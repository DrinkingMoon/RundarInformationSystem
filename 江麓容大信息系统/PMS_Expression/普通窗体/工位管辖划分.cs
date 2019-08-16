using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 工位管辖划分 : Form
    {
        /// <summary>
        /// 配置管理服务接口
        /// </summary>
        IConfigManagement m_configManagement = PMS_ServerFactory.GetServerModule<IConfigManagement>();

        /// <summary>
        /// 工位服务接口
        /// </summary>
        IWorkbenchService m_workbenchService = PMS_ServerFactory.GetServerModule<IWorkbenchService>();

        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_error;

        public 工位管辖划分()
        {
            InitializeComponent();

            dgvPurpose.DataSource = m_configManagement.GetAssemblingPurpose();

            cmbPurpose.DataSource = dgvPurpose.DataSource;

            cmbPurpose.DisplayMember = "装配用途名称";
            cmbPurpose.ValueMember = "装配用途编号";

            cmbWorkbenchPurpose.DataSource = dgvPurpose.DataSource;

            cmbWorkbenchPurpose.DisplayMember = "装配用途名称";
            cmbWorkbenchPurpose.ValueMember = "装配用途编号";

            dgvWorkbench.DataSource = m_workbenchService.Workbenchs;

            cmbWorkbench.DataSource = dgvWorkbench.DataSource;

            cmbWorkbench.DisplayMember = "工位";
            cmbWorkbench.ValueMember = "工位";

            dgvPurposeAuthority.DataSource = m_configManagement.GetPurposeAuthority();

            dgvWorkbenchPurpose.DataSource = m_configManagement.GetWorkbenchPurpose();
        }

        /// <summary>
        /// 添加装配通途
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPurpose_Click(object sender, EventArgs e)
        {
            txtPurposeName.Text = txtPurposeName.Text.Trim();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtPurposeName.Text))
            {
                MessageDialog.ShowPromptMessage("用途名称不允许为空");
                return;
            }

            if (!m_configManagement.AddAssemblingPurpose(txtPurposeName.Text, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");

                dgvPurpose.DataSource = m_configManagement.GetAssemblingPurpose();
            }
        }

        /// <summary>
        /// 修改装配通途
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdatePurpose_Click(object sender, EventArgs e)
        {
            txtPurposeName.Text = txtPurposeName.Text.Trim();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtPurposeName.Text))
            {
                MessageDialog.ShowPromptMessage("用途名称不允许为空");
                return;
            }

            if (dgvPurpose.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择要修改的行后再进行此操作");
                return;
            }

            if (!m_configManagement.UpdateAssemblingPurpose(
                dgvPurpose.CurrentRow.Cells["装配用途名称"].Value.ToString(), txtPurposeName.Text, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");

                dgvPurpose.DataSource = m_configManagement.GetAssemblingPurpose();
            }
        }

        /// <summary>
        /// 添加工位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddWorkbench_Click(object sender, EventArgs e)
        {
            txtWorkbench.Text = txtWorkbench.Text.Trim();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtWorkbench.Text))
            {
                MessageDialog.ShowPromptMessage("工位号不允许为空");
                return;
            }

            IQueryable<View_P_Workbench> result = null;

            if (m_workbenchService.Add(txtWorkbench.Text, txtWorkbenchRemark.Text.Trim(), out result, out m_error))
            {
                MessageDialog.ShowPromptMessage("添加成功");

                dgvWorkbench.DataSource = result;
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        /// <summary>
        /// 修改工位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateWorkbench_Click(object sender, EventArgs e)
        {
            if (dgvWorkbench.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择要修改的行后再进行此操作");
                return;
            }

            txtWorkbench.Text = txtWorkbench.Text.Trim();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtWorkbench.Text))
            {
                MessageDialog.ShowPromptMessage("工位号不允许为空");
                return;
            }

            // 装配BOM服务接口
            IAssemblingBom assemblingBomService = PMS_ServerFactory.GetServerModule<IAssemblingBom>();

            if (assemblingBomService.IsExistsWorkbench(txtWorkbench.Text))
            {
                MessageDialog.ShowPromptMessage("该工位已经使用, 不允许进行修改");
                return;
            }

            IQueryable<View_P_Workbench> result = null;

            if (m_workbenchService.Update(dgvWorkbench.CurrentRow.Cells["工位"].Value.ToString(),
                txtWorkbench.Text, txtWorkbenchRemark.Text.Trim(), out result, out m_error))
            {
                MessageDialog.ShowPromptMessage("修改成功");

                dgvWorkbench.DataSource = result;
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        /// <summary>
        /// 人员管辖用途划分
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPurposeAuthority_Click(object sender, EventArgs e)
        {
            if (m_configManagement.AddPurposeAuthority(txtWorkID.Text, (int)cmbPurpose.SelectedValue, out m_error))
            {
                MessageDialog.ShowPromptMessage("操作成功");

                dgvPurposeAuthority.DataSource = m_configManagement.GetPurposeAuthority();
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
        }

        /// <summary>
        /// 获取人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetSimplePersonelInfo();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtWorkID.Text = form["员工编号"];
            }
        }

        /// <summary>
        /// 删除人员管辖权限
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeletePurposeAuthority_Click(object sender, EventArgs e)
        {
            if (dgvPurposeAuthority.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择要删除的行后再进行此操作");
                return;
            }

            if (m_configManagement.DeletePurposeAuthority(dgvPurposeAuthority.CurrentRow.Cells["工号"].Value.ToString(), 
                (int)dgvPurposeAuthority.CurrentRow.Cells["装配用途编号"].Value, out m_error))
            {
                MessageDialog.ShowPromptMessage("操作成功");

                dgvPurposeAuthority.DataSource = m_configManagement.GetPurposeAuthority();
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
        }

        /// <summary>
        /// 更新工位装配用途
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdateWorkbenchPurpose_Click(object sender, EventArgs e)
        {
            if (dgvWorkbenchPurpose.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择要更新的行后再进行此操作");
                return;
            }

            if (m_configManagement.UpdateWorkbenchPurpose(
                dgvPurposeAuthority.CurrentRow.Cells["工位"].Value.ToString(), 
                (int)dgvPurposeAuthority.CurrentRow.Cells["装配用途编号"].Value, out m_error))
            {
                MessageDialog.ShowPromptMessage("操作成功");

                dgvWorkbenchPurpose.DataSource = m_configManagement.GetWorkbenchPurpose();
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_error);
            }        
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dataGridView1 = sender as DataGridView;

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

        private void dgvPurpose_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtPurposeName.Text = dgvPurpose.CurrentRow.Cells["装配用途名称"].Value.ToString();
        }

        private void dgvWorkbench_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtWorkbench.Text = dgvWorkbench.CurrentRow.Cells["工位"].Value.ToString();
            txtWorkbenchRemark.Text = dgvWorkbench.CurrentRow.Cells["备注"].Value.ToString();
        }

        private void dgvPurposeAuthority_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            txtWorkID.Text = dgvPurposeAuthority.CurrentRow.Cells["工号"].Value.ToString();
            cmbPurpose.Text = dgvPurposeAuthority.CurrentRow.Cells["装配用途名称"].Value.ToString();
        }

        private void dgvWorkbenchPurpose_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            cmbWorkbench.Text = dgvWorkbenchPurpose.CurrentRow.Cells["工位"].Value.ToString();
            cmbWorkbenchPurpose.Text = dgvWorkbenchPurpose.CurrentRow.Cells["装配用途名称"].Value.ToString();
        }
    }
}
