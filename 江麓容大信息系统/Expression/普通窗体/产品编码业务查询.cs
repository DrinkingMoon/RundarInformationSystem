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
    /// <summary>
    /// 成品编码业务查询界面
    /// </summary>
    public partial class 产品编码业务查询 : Form
    {
        /// <summary>
        /// 产品编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        public 产品编码业务查询()
        {
            InitializeComponent();

            cmbProductType.Text = "全  部";

            cmbOperationType.Text = "全  部";

            dtpStartTime.Value = ServerTime.Time.AddMonths(-1);
            dtpEndTime.Value = ServerTime.Time;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverProductCode.GetProductCodeOperationInfo(dtpStartTime.Value, dtpEndTime.Value,
                cmbOperationType.Text, cmbProductType.Text);

            dataGridView1.Columns["单据号"].Width = 120;
        }

        private void btnExcleOut_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DataTableToExcel(saveFileDialog1, (DataTable)dataGridView1.DataSource, null);
        }
    }
}
