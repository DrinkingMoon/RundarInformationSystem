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
using UniversalControlLibrary;

namespace Form_Economic_Financial
{
    /// <summary>
    /// 主机厂与系统零件匹配设置界面
    /// </summary>
    public partial class 主机厂与系统零件匹配设置 : Form
    {
        /// <summary>
        /// 客户信息服务组件
        /// </summary>
        IClientServer m_serverClient = ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 主机厂报表服务组件
        /// </summary>
        ICommunicateReportBill m_serverComReportBill = ServerModuleFactory.GetServerModule<ICommunicateReportBill>();

        /// <summary>
        /// LNQ数据集
        /// </summary>
        YX_GoodsSystemMatchingCommunicate m_lnqGoodsMatching = new YX_GoodsSystemMatchingCommunicate();

        public 主机厂与系统零件匹配设置()
        {
            InitializeComponent();

            RefreshDataGirdView();
        }

        private void txtSysName_OnCompleteSearch()
        {
            txtSysName.Text = txtSysName.DataResult["物品名称"].ToString();
            txtSysCode.Text = txtSysName.DataResult["图号型号"].ToString();
            txtSysName.Tag = Convert.ToInt32(txtSysName.DataResult["序号"]);
        }
        
        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGirdView()
        {
            dataGridView1.DataSource = m_serverComReportBill.GetMatchingTable();

            dataGridView1.Columns["主机厂"].Width = 180;
            dataGridView1.Columns["主机厂图号型号"].Width = 120;
            dataGridView1.Columns["主机厂物品名称"].Width = 120;
            dataGridView1.Columns["系统图号型号"].Width = 120;
            dataGridView1.Columns["系统物品名称"].Width = 120;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqGoodsMatching.Communicate = txtClient.Tag.ToString();
            m_lnqGoodsMatching.CommunicateGoodsCode = txtCommunicateCode.Text;
            m_lnqGoodsMatching.CommunicateGoodsName = txtCommunicateName.Text;
            m_lnqGoodsMatching.GoodsID = Convert.ToInt32(txtSysName.Tag);
            m_lnqGoodsMatching.Remark = txtRemark.Text;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverComReportBill.UpdateMathchingTable("添加",m_lnqGoodsMatching,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            RefreshDataGirdView();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverComReportBill.UpdateMathchingTable("删除", m_lnqGoodsMatching, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshDataGirdView();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }
            else
            {
                txtClient.Text = dataGridView1.CurrentRow.Cells["主机厂"].Value.ToString();
                txtClient.Tag = dataGridView1.CurrentRow.Cells["主机厂编码"].Value.ToString();
                txtCommunicateCode.Text = dataGridView1.CurrentRow.Cells["主机厂图号型号"].Value.ToString();
                txtCommunicateName.Text = dataGridView1.CurrentRow.Cells["主机厂物品名称"].Value.ToString();
                txtSysCode.Text = dataGridView1.CurrentRow.Cells["系统图号型号"].Value.ToString();
                txtSysName.Text = dataGridView1.CurrentRow.Cells["系统物品名称"].Value.ToString();
                txtSysName.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["系统物品ID"].Value.ToString());
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
        }
    }
}
