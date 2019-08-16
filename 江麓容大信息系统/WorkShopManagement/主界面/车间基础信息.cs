using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Service_Manufacture_WorkShop;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间基础信息 : Form
    {
        IWorkShopBasic m_serverWSBasic = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopBasic>();

        public 车间基础信息()
        {
            InitializeComponent();
        }

        private void 车间基础信息_Load(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverWSBasic.GetWorkShopBasicInfo();
        }
    }
}
