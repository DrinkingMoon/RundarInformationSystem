using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;

namespace Expression
{
    public partial class 电子档案全部查询简体窗口 : Form
    {
        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_findElectronFile = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        public 电子档案全部查询简体窗口()
        {
            InitializeComponent();
        }

        private void 电子档案全部查询简体窗口_Load(object sender, EventArgs e)
        {
            DataTable dt = m_findElectronFile.GetAllSimpleData();

            label2.Text = dt.Rows.Count.ToString();
            dataGridView1.DataSource = dt;
        }
    }
}
