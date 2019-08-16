using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using WebServerModule;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;

namespace Expression
{
    /// <summary>
    /// CVT客户历史修改的信息显示界面
    /// </summary>
    public partial class CVT客户历史信息 : Form
    {
        /// <summary>
        /// 查询类型
        /// </summary>
        string m_strType = "";

        /// <summary>
        /// 售后服务组件
        /// </summary>
        IServiceFeedBack m_serverFeedBack = WebServerModule.ServerModuleFactory.GetServerModule<IServiceFeedBack>();

        /// <summary>
        /// CVT客户服务组件
        /// </summary>
        ICVTCustomerInformationServer m_serverCVTCustomerInfo = ServerModule.ServerModuleFactory.GetServerModule<ICVTCustomerInformationServer>();

        public CVT客户历史信息(string type, string vehicleShelfNumber)
        {
            InitializeComponent();

            m_strType = type;

            this.Text = m_strType;

            if (m_strType == "CVT客户历史信息")
            {

                dataGridView1.DataSource = m_serverCVTCustomerInfo.GetCVTCustomerHistoryInfo(vehicleShelfNumber);

                dataGridView1.Columns["车架号"].Width = 120;
                dataGridView1.Columns["变速箱型号"].Width = 80;

            }
            else if (m_strType == "车辆维修记录")
            {
                dataGridView1.DataSource = m_serverFeedBack.GetVehicleMaintenanceRecord(vehicleShelfNumber);
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (m_strType == "车辆维修记录")
            {
                AuthorityFlag m_authFlag = AuthorityFlag.View;

                售后服务质量反馈单明细 form = new 售后服务质量反馈单明细(m_authFlag, 
                    dataGridView1.CurrentRow.Cells["反馈单号"].Value.ToString(), 
                    dataGridView1.CurrentRow.Cells["关联号"].Value.ToString());
                form.ShowDialog();
            }
        }
    }
}
