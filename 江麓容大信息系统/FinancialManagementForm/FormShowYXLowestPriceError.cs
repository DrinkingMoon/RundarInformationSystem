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
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    public partial class FormShowYXLowestPriceError : Form
    {
        /// <summary>
        /// 销售清单服务类
        /// </summary>
        IMarketingPartBillServer m_marketPartBillServer = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IMarketingPartBillServer>();

        public FormShowYXLowestPriceError(DataTable dt,string billNo)
        {
            InitializeComponent();

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }
            else
            {
                DataTable dtLog = m_marketPartBillServer.GetSystemLog(billNo);

                if (dtLog != null && dtLog.Rows.Count > 0)
                {
                    dataGridView1.DataSource = dtLog;
                }
                else
                {
                    MessageDialog.ShowPromptMessage(billNo+"没有操作日志！");
                }
            }
        }
    }
}
