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
using UniversalControlLibrary;

namespace Expression
{
    public partial class 营销要货计划交货期设置 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 营销计划服务组件
        /// </summary>
        IMarketingPlan m_serverMarketingPlan = ServerModuleFactory.GetServerModule<IMarketingPlan>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 营销计划交货期数据集
        /// </summary>
        S_MarketingPlanDelivery m_lnqDelivery = new S_MarketingPlanDelivery();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillID;

        /// <summary>
        /// 物品ID
        /// </summary>
        int m_intGoodsID;

        /// <summary>
        /// 计划月
        /// </summary>
        int m_intMonth;

        public 营销要货计划交货期设置(string billID,int goodsID,int month, decimal monthCount, 
            string monthName, string yearAndMonth, string billStatus)
        {
            InitializeComponent();

            m_intGoodsID = goodsID;
            m_intMonth = month;
            m_strBillID = billID;

            txtDJH.Text = m_strBillID;
            txtName.Text = m_serverBasicGoods.GetGoodsInfo(goodsID).GoodsName;
            numMonthCount.Value = monthCount;
            label7.Text = monthName + "月计划总数";
            label2.Text = yearAndMonth + "营销计划交货期设置";

            dataGridView1.DataSource = m_serverMarketingPlan.GetPlanDeliveryInfo(m_strBillID, m_intGoodsID, m_intMonth);

            if (billStatus == "新建单据")
            {
                btnAdd.Enabled = true;
                btnDelete.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
                btnDelete.Enabled = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            m_lnqDelivery.Delivery = Convert.ToDateTime( dtpDelivery.Value.ToShortDateString());
            m_lnqDelivery.DJH = m_strBillID;
            m_lnqDelivery.GoodsID = m_intGoodsID;
            m_lnqDelivery.Month = m_intMonth;
            m_lnqDelivery.DeliveryCount = Convert.ToInt32( numDeliveryCount.Value);

            if (!m_serverMarketingPlan.AddDelivery(m_lnqDelivery,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }

            dataGridView1.DataSource = m_serverMarketingPlan.GetPlanDeliveryInfo(m_strBillID, m_intGoodsID, m_intMonth);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            m_lnqDelivery.Delivery = Convert.ToDateTime( dataGridView1.CurrentRow.Cells["交货期"].Value);
            m_lnqDelivery.DJH = m_strBillID;
            m_lnqDelivery.GoodsID = m_intGoodsID;
            m_lnqDelivery.Month = m_intMonth;

            if (!m_serverMarketingPlan.DeleteDelivery(m_lnqDelivery, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }

            dataGridView1.DataSource = m_serverMarketingPlan.GetPlanDeliveryInfo(m_strBillID, m_intGoodsID, m_intMonth);

        }
    }
}
