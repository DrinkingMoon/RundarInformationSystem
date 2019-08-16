using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;

namespace Expression
{
    /// <summary>
    /// 未领用的报废物品查询界面
    /// </summary>
    public partial class 未领用的报废物品的查询窗体 : Form
    {
        /// <summary>
        /// 领料单服务
        /// </summary>
        IMaterialRequisitionServer m_billServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 未领用的报废物品的查询窗体(string loginName)
        {
            InitializeComponent();
            dataGridView1.DataSource = m_billServer.GetScrapGoods(loginName, out m_err);
        }
    }
}
