using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;
using Service_Peripheral_CompanyQuality;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class 重点工作查询 : Form
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IFocalWork _Service_FocalWork = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        public 重点工作查询()
        {
            InitializeComponent();

            for (int i = 2011; i <= ServerTime.Time.Year; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Text == "单工作查询")
            {
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_FocalWork();
            }
            else if (tabControl1.SelectedTab.Text == "月度查询")
            {
                if (GeneralFunction.IsNullOrEmpty(cmbYear.Text) || GeneralFunction.IsNullOrEmpty(cmbMonth.Text))
                {
                    return;
                }

                customDataGridView2.DataSource = _Service_FocalWork.GetTable_FocalWork(cmbYear.Text + cmbMonth.Text);
            }
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            txtTaskName.Text = customDataGridView1.CurrentRow.Cells["重点工作1"].Value.ToString();
            txtTaskName.Tag = customDataGridView1.CurrentRow.Cells["F_Id1"].Value.ToString();
        }

        private void btnSelectDetail_Click(object sender, EventArgs e)
        {
            重点工作详细信息 frm = new 重点工作详细信息(txtTaskName.Tag.ToString(), null);
            frm.Show();
        }

        private void customDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            btnSelectDetail_Click(null, e);
        }

        private void 重点工作查询_Load(object sender, EventArgs e)
        {
            customDataGridView1.DataSource = _Service_FocalWork.GetTable_FocalWork();
            customDataGridView2.DataSource = _Service_FocalWork.GetTable_FocalWork(cmbYear.Text + cmbMonth.Text);
        }

        private void cmbYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GeneralFunction.IsNullOrEmpty(cmbYear.Text) || GeneralFunction.IsNullOrEmpty(cmbMonth.Text))
            {
                return;
            }

            customDataGridView2.DataSource = _Service_FocalWork.GetTable_FocalWork(cmbYear.Text + cmbMonth.Text);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (GeneralFunction.IsNullOrEmpty(cmbYear.Text) || GeneralFunction.IsNullOrEmpty(cmbMonth.Text))
            {
                MessageDialog.ShowPromptMessage("请选择【查询年份】与【查询月份】");
                return;
            }

            customDataGridView2.DataSource = _Service_FocalWork.GetTable_FocalWork(cmbYear.Text + cmbMonth.Text);
        }

        private void customDataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView2.CurrentRow == null)
            {
                return;
            }
            else if (customDataGridView2.CurrentRow.Cells["当月评价"].Value.ToString() == "未填写")
            {
                MessageDialog.ShowPromptMessage("【未填写】当月评价");
                return;
            }

            重点工作详细信息 frm = new 重点工作详细信息(customDataGridView2.CurrentRow.Cells["F_Id"].Value.ToString(), 
                cmbYear.Text + cmbMonth.Text);
            frm.Show();
        }

        private void customDataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgvr in (sender as CustomDataGridView).Rows)
            {
                if (dgvr.Cells["当月评价"].Value.ToString() == "未完成")
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void customDataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            foreach (DataGridViewRow dgvr in (sender as CustomDataGridView).Rows)
            {
                if (dgvr.Cells["状态"].Value.ToString() == "延期")
                {
                    dgvr.DefaultCellStyle.BackColor = Color.Yellow;
                }
            }
        }
    }
}
