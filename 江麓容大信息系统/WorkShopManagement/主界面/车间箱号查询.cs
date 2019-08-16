using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Manufacture_WorkShop;
using GlobalObject;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间箱号查询 : Form
    {

        /// <summary>
        /// 车间管理基础信息服务组件
        /// </summary>
        IWorkShopBasic m_serverWSBasic = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopBasic>();

        /// <summary>
        /// 车间编码服务组件
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopProductCode m_serverProductCode = 
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopProductCode>();

        public 车间箱号查询()
        {
            InitializeComponent();
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer2.IsSplitterFixed = true;
            cmbWSCode.DataSource = m_serverWSBasic.GetWorkShopBasicInfo();

            cmbWSCode.DisplayMember = "车间名称";
            cmbWSCode.ValueMember = "车间编码";
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            dataGridView2.DataSource = m_serverProductCode.GetWorkShopProductCodeInfo(cmbWSCode.SelectedValue.ToString(),
                Convert.ToInt32( dataGridView1.CurrentRow.Cells["物品ID"].Value));
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                return;
            }

            dataGridView3.DataSource = m_serverProductCode.GetWorkShopProductCodeBusiness(dataGridView2.CurrentRow.Cells["产品箱号"].Value.ToString(), 
                Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value));
        }

        private void cmbWSCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverProductCode.GetWorkShopProductCodeNumber(cmbWSCode.SelectedValue.ToString());
        }

        private void 车间箱号查询_Load(object sender, EventArgs e)
        {
            if (BasicInfo.DeptCode.Length >= 4)
            {
                WS_WorkShopCode tempLnq = m_serverWSBasic.GetWorkShopCodeInfo(BasicInfo.DeptCode.Substring(0, 4));

                if (tempLnq != null)
                {
                    cmbWSCode.Text = tempLnq.WSName;
                    cmbWSCode.SelectedValue = tempLnq.WSCode;
                }
            }
        }
    }
}
