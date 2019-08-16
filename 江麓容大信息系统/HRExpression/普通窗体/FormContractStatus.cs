using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using ServerModule;
using GlobalObject;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 设置合同状态界面
    /// </summary>
    public partial class FormContractStatus : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 合同管理服务类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        public FormContractStatus()
        {
            InitializeComponent();

            DataTable dt = m_laborServer.GetLaborContracType();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (!cmbLaborType.Items.Contains(dt.Rows[i]["类别"].ToString()))
                {
                    cmbLaborType.Items.Add(dt.Rows[i]["类别"].ToString());
                }
            }

            dataGridView1.DataSource = m_laborServer.GetContractStatus();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() == "" ||  txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("蓝色字体的信息填写完整1");
                return;
            }

            HR_LaborContractStatus laborStatus = new HR_LaborContractStatus();

            laborStatus.LaborContractType = cmbLaborType.Text;
            laborStatus.StatusName = txtStatus.Text;
            laborStatus.DeleteFlag = cbFlag.Checked;
            laborStatus.Remark = txtRemark.Text;
            laborStatus.Recorder = BasicInfo.LoginID;
            laborStatus.RecordTime = ServerTime.Time;

            if (!m_laborServer.AddContractStatus(laborStatus, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            dataGridView1.DataSource = m_laborServer.GetContractStatus();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("选择需要删除的数据行！");
                return;
            }

            if (MessageBox.Show("您是否确定要删除" + txtStatus.Text.Trim()
                + "信息?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_laborServer.DeleteContractStatus(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value), out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            dataGridView1.DataSource = m_laborServer.GetContractStatus();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            cmbLaborType.Text = dataGridView1.CurrentRow.Cells["类别"].Value.ToString();
            txtStatus.Text = dataGridView1.CurrentRow.Cells["状态"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
        }
    }
}
