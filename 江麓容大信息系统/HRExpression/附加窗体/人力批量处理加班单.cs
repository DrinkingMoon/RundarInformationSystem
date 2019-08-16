using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using ServerModule;
using PlatformManagement;
using Service_Peripheral_HR;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 人力批量处理加班单 : Form
    {
        IOverTimeBillServer _OverTimeService = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOverTimeBillServer>();

        public 人力批量处理加班单()
        {
            InitializeComponent();

            customDataGridView1.DataSource = _OverTimeService.AutoCreateOverTime_ShowPersonnel();
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            if (e.ColumnIndex == 0)
            {
                customDataGridView1.CurrentRow.Cells["选"].Value = !Convert.ToBoolean(customDataGridView1.CurrentRow.Cells["选"].Value);
            }
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.SelectedRows)
            {
                dgrv.Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.SelectedRows)
            {
                dgrv.Cells["选"].Value = false;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.Rows)
            {
                dgrv.Cells["选"].Value = true;
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgrv in customDataGridView1.Rows)
            {
                dgrv.Cells["选"].Value = false;
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            Submit(false);
            MessageDialog.ShowPromptMessage("审核成功");
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Submit(true);
            MessageDialog.ShowPromptMessage("保存成功");
            customDataGridView1.DataSource = _OverTimeService.AutoCreateOverTime_ShowPersonnel();
        }

        /// <summary>
        /// 提交信息
        /// </summary>
        /// <param name="saveFlag">是否保存标志</param>
        void Submit(bool saveFlag)
        {
            try
            {
                _OverTimeService.AutoCreateOverTime_BatchOperation((DataTable)customDataGridView1.DataSource, saveFlag);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }
    }
}
