using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;

namespace Expression
{
    /// <summary>
    /// 销售订单部门评审意见历史信息
    /// </summary>
    public partial class 销售订单部门评审意见历史信息 : Form
    {
        /// <summary>
        /// 销售合同/订单评审服务类
        /// </summary>
        ISalesOrderServer m_salesOrderServer = ServerModuleFactory.GetServerModule<ISalesOrderServer>();

        public 销售订单部门评审意见历史信息(string billNo,string deptCode)
        {
            InitializeComponent();

            dataGridView1.DataSource = m_salesOrderServer.GetReviewHistory(billNo,deptCode);
        }
    }
}
