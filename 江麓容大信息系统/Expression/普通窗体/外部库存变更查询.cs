using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;


namespace Expression
{
    /// <summary>
    /// 外部库存变更明细界面
    /// </summary>
    public partial class 外部库存变更查询 : Form
    {
        /// <summary>
        /// 营销业务服务组件
        /// </summary>
        ISellIn m_serverSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        public 外部库存变更查询(int goodid,string stroageid)
        {
            InitializeComponent();
            dataGridView1.DataSource = m_serverSellIn.GetOutStockInfo(goodid, stroageid);
            dataGridView1.Columns["序号"].Width = 40;
        }
    }
}
