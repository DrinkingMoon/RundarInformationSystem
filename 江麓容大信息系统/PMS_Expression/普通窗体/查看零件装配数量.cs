using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 查看零件装配数量 : Form
    {
        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_eleService = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        public 查看零件装配数量()
        {
            InitializeComponent();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
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

        /// <summary>
        /// 获取物品信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnGetGoods_Click(object sender, EventArgs e)
        {
            FormQueryInfo dialog = QueryInfoDialog.GetPartInfoOfElectronFile(
                dateTimePickerST.Value.Date, dateTimePickerET.Value.AddDays(1).Date);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = dialog["零部件编码"];
                txtName.Text = dialog["零部件名称"];
                txtSpec.Text = dialog["规格"];
                txtBatchNo.Text = dialog["批次号"];
            }
        }

        /// <summary>
        /// 搜索
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtName.Text))
            {
                MessageDialog.ShowPromptMessage("请选择物品后再进行此操作");
                return;
            }

            int amount = 0;

            DataTable dt = m_eleService.GetElectronFile(txtCode.Text, txtName.Text, txtSpec.Text, txtBatchNo.Text,
                dateTimePickerST.Value.Date, dateTimePickerET.Value.AddDays(1).Date, out amount);
            
            lblAmount.Text = string.Format("共计 {0} 件", amount);

            dataGridView1.DataSource = dt;

            userControlDataLocalizer1.Init(dataGridView1, this.Name, new DataTable());
        }
    }
}
