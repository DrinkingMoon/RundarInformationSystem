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
    /// 设置岗位类别界面
    /// </summary>
    public partial class FormPostType : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 岗位管理服务类
        /// </summary>
        IOperatingPostServer m_postServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOperatingPostServer>();

        public FormPostType()
        {
            InitializeComponent();

            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            dataGridView1.DataSource = m_postServer.GetPostType();

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 添加
        /// </summary>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtTypeName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写职位类别！");
                return;
            }

            if (txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请把备注填写完整！");
                return;
            }

            HR_PostType postType = new HR_PostType();

            postType.TypeID = Convert.ToInt32(txtPostTypeCode.Text);
            postType.TypeName = txtTypeName.Text;
            postType.IsHighLevel = rbIsHighLevel.Checked;
            postType.IsMiddleLevel = rbIsMiddleLevel.Checked;
            postType.Recorder = BasicInfo.LoginID;
            postType.RecordTime = ServerTime.Time;
            postType.Remark = txtRemark.Text;

            if (!m_postServer.AddPostType(postType, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            RefreshControl();
        }

        /// <summary>
        /// 修改
        /// </summary>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtTypeName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写职位类别！");
                return;
            }

            if (txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请把备注填写完整！");
                return;
            }

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

            HR_PostType postType = new HR_PostType();

            postType.TypeID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["职位类别编号"].Value);
            postType.TypeName = txtTypeName.Text;
            postType.IsHighLevel = rbIsHighLevel.Checked;
            postType.IsMiddleLevel = rbIsMiddleLevel.Checked;
            postType.Recorder = BasicInfo.LoginID;
            postType.RecordTime = ServerTime.Time;
            postType.Remark = txtRemark.Text;

            if (!m_postServer.UpdatePostType(postType, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            RefreshControl();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtPostTypeCode.Text = dataGridView1.CurrentRow.Cells["职位类别编号"].Value.ToString();
            txtTypeName.Text = dataGridView1.CurrentRow.Cells["职位类别"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            rbIsHighLevel.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否高层人员"].Value);
            rbIsMiddleLevel.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否中层人员"].Value);
        }

        /// <summary>
        /// 删除
        /// </summary>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }
            
            if (dataGridView1.SelectedRows.Count > 1)
            {
                int n = dataGridView1.SelectedRows.Count;
                int[] arrayID = new int[n];

                for (int i = 0; i < n; i++)
                {
                    arrayID[i] = Convert.ToInt32(dataGridView1.SelectedRows[i].Cells["职位类别编号"].Value);
                }

                if (MessageBox.Show("您是否确定要删除类别信息?", "消息", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < arrayID.Length; i++)
                    {
                        if (!m_postServer.DeletePostType(arrayID[i], out error))
                        {
                            MessageDialog.ShowPromptMessage(error);
                            return;
                        }
                    }
                }
            }
            else
            {
                int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["职位类别编号"].Value);

                if (MessageBox.Show("您是否确定要删除类别" + dataGridView1.CurrentRow.Cells["职位类别编号"].Value.ToString() 
                    + "信息?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_postServer.DeletePostType(id, out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
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
