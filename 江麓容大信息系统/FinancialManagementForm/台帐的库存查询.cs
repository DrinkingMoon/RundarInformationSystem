using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    /// <summary>
    /// 台帐库存查询界面
    /// </summary>
    public partial class 台帐的库存查询 : Form
    {
        /// <summary>
        /// 财务服务组件
        /// </summary>
        IGatherBillAndDetailBillServer m_findEstrade = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IGatherBillAndDetailBillServer>();

        public 台帐的库存查询(string  strDateTime,int IGoodsID)
        {
            InitializeComponent();
            dataGridView1.DataSource = m_findEstrade.GetOldStock(IGoodsID, strDateTime);
        }
    }
}
