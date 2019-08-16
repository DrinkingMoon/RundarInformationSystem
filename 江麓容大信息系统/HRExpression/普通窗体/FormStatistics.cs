using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 统计班次界面
    /// </summary>
    public partial class FormStatistics : Form
    {
        /// <summary>
        /// 排班信息操作类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        public FormStatistics(int billNo)
        {
            InitializeComponent();

            DataTable dt = m_workSchedulingServer.GetDefinitionStatistics(billNo);

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
            }
        }
    }
}
