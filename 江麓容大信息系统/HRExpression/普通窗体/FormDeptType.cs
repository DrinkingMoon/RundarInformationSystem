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
    /// 设置部门类别界面
    /// </summary>
    public partial class FormDeptType : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 部门信息服务组件
        /// </summary>
        IDeptTypeServer m_deptTypeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IDeptTypeServer>();

        public FormDeptType()
        {
            InitializeComponent();

            RefreshControl();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtDeptCode.Text = dataGridView1.CurrentRow.Cells["类别编号"].Value.ToString();
            txtDeptName.Text = dataGridView1.CurrentRow.Cells["类别名称"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
        }

        /// <summary>
        /// 清空控件值
        /// </summary>
        void ClearControl()
        {
            txtDeptName.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            ClearControl();

            dataGridView1.DataSource = m_deptTypeServer.GetAllDeptType();

            dataGridView1.Refresh();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtDeptName.Text.Trim() == "")
            {
                MessageDialog.ShowErrorMessage("类别不允许为空");
                return;
            }

            if (txtDeptCode.Text.Trim() == "")
            {
                MessageDialog.ShowErrorMessage("类别编号不允许为空");
                return;
            }

            HR_DeptType deptType = new HR_DeptType();

            deptType.TypeID = Convert.ToInt32(txtDeptCode.Text);
            deptType.TypeName = txtDeptName.Text;
            deptType.Remark = txtRemark.Text;
            deptType.Recorder = BasicInfo.LoginID;
            deptType.RecordTime = ServerTime.Time;

            if (!m_deptTypeServer.AddDeptType(deptType, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

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

            if (txtDeptName.Text.Trim() == "")
            {
                MessageDialog.ShowErrorMessage("部门类别不允许为空");
                return;
            }

            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["类别编号"].Value);

            HR_DeptType deptType = new HR_DeptType();

            deptType.TypeID = id;
            deptType.TypeName = txtDeptName.Text;
            deptType.Remark = txtRemark.Text;
            deptType.Recorder = BasicInfo.LoginID;
            deptType.RecordTime = ServerTime.Time;

            if (!m_deptTypeServer.UpdateDeptType(deptType, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            RefreshControl();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }
            else if (dataGridView1.SelectedRows.Count > 1)
            {
                int n = dataGridView1.SelectedRows.Count;
                int[] arrayID = new int[n];

                for (int i = 0; i < n; i++)
                {
                    arrayID[i] = Convert.ToInt32(dataGridView1.SelectedRows[i].Cells["类别编号"].Value);
                }

                if (MessageBox.Show("您是否确定要删除类别信息?", "消息", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayID.Length; i++)
                    {
                        if (!m_deptTypeServer.DeleteDeptType(arrayID[i], out error))
                        {
                            MessageDialog.ShowPromptMessage(error);
                            return;
                        }
                    }
                }
            }
            else
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["类别编号"].Value);

                if (MessageBox.Show("您是否确定要删除类别" + dataGridView1.CurrentRow.Cells["类别名称"].Value.ToString() + "信息?", "消息",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_deptTypeServer.DeleteDeptType(id, out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
                }
            }

            RefreshControl();
        }
    }
}
