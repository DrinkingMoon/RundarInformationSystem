using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using Service_Economic_Financial;

namespace Expression
{
    /// <summary>
    /// 产品进销存查询界面
    /// </summary>
    public partial class 产品进销存表 : Form
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IGatherBillAndDetailBillServer m_findGatherBill = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IGatherBillAndDetailBillServer>();

        /// <summary>
        /// 原TABLE样式
        /// </summary>
        DataTable m_dtStyle = new DataTable();

        public 产品进销存表()
        {
            InitializeComponent();

            InsertCombox();

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = 4;
            cmbStorage.Enabled = false;
            m_dtStyle = (DataTable)dataGridView1.DataSource;
        }

        /// <summary>
        /// COMBOX 插入数据
        /// </summary>
        void InsertCombox()
        {
            for (int i = 2010; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
            }
        }

        private void 产品进销存表_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
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
        /// 检查窗体
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        bool CheckForm()
        {
            if (cmbMonth.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择月份");
                return false;
            }

            if (cmbStorage.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择库房");
                return false;
            }

            if (cmbYear.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择年份");
                return false;
            }

            return true;
        }

        private void btnGetNew_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
