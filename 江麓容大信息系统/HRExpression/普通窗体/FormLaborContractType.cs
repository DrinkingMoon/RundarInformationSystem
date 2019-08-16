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
    /// 设置合同类别界面
    /// </summary>
    public partial class FormLaborContractType : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 合同管理服务类
        /// </summary>
        ILaborContractServer m_laborServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILaborContractServer>();

        public FormLaborContractType()
        {
            InitializeComponent();

            dataGridView1.DataSource = m_laborServer.GetLaborContracType();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtTypeCode.Text = dataGridView1.CurrentRow.Cells["类别编号"].Value.ToString();
            txtTypeName.Text = dataGridView1.CurrentRow.Cells["类别名称"].Value.ToString();
            cmbType.Text = dataGridView1.CurrentRow.Cells["类别"].Value.ToString();
            txtTypeRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtTypeCode.Text.Trim() == "" || txtTypeName.Text.Trim() == "" || txtTypeRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("蓝色字体的信息填写完整1");
                return;
            }

            HR_LaborContractType laborType = new HR_LaborContractType();

            laborType.Category = cmbType.Text;
            laborType.TypeCode = txtTypeCode.Text;
            laborType.TypeName = txtTypeName.Text;
            laborType.Remark = txtTypeRemark.Text;
            laborType.Recorder = BasicInfo.LoginID;
            laborType.RecordTime = ServerTime.Time;

            if (!m_laborServer.AddLaborType(laborType, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            dataGridView1.DataSource = m_laborServer.GetLaborContracType();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("选择需要删除的数据行！");
                return;
            }

            if (MessageBox.Show("您是否确定要删除类别编号为" + txtTypeCode.Text.Trim()
                + "信息?", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_laborServer.DeleteLaborTypeByTypeCode(txtTypeCode.Text.Trim(), out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            dataGridView1.DataSource = m_laborServer.GetLaborContracType();
        }
    }
}
