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
    public partial class 零星采购申请统计报表 : Form
    {
        /// <summary>
        /// 零星采购服务类
        /// </summary>
        IMinorPurchaseBillServer m_minorBillServer = ServerModule.ServerModuleFactory.GetServerModule<IMinorPurchaseBillServer>();

        public 零星采购申请统计报表()
        {
            InitializeComponent();
        }

        private void 零星采购申请统计报表_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            DataTable dt = m_minorBillServer.GetStatisticsTable(numYear.Value.ToString(),numMonth.Value.ToString(),
                numEndYear.Value.ToString(), numEndMonth.Value.ToString(), tbsDeptCode.Text);

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有数据！");
            }
        }
    }
}
