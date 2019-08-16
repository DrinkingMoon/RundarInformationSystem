using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 生成安全库存界面
    /// </summary>
    public partial class 由总成自动生成安全库存 : Form
    {
        /// <summary>
        /// 全局TABLE数据集
        /// </summary>
        private DataTable m_dtSafeGoods = new DataTable();

        public DataTable DtSafeGoods
        {
            get { return m_dtSafeGoods; }
            set { m_dtSafeGoods = value; }
        }

        /// <summary>
        /// 安全库存服务接口
        /// </summary>
        ISafeStockServer m_safeStockServer = ServerModuleFactory.GetServerModule<ISafeStockServer>();

        public 由总成自动生成安全库存()
        {
            InitializeComponent();

            DataTable dt = new DataTable();

            dt.Columns.Add("总成型号");
            dt.Columns.Add("总成名称");
            dt.Columns.Add("总成数量");

            dataGridView1.DataSource = dt;
        }

        void txtProductType_OnCompleteSearch()
        {
            txtProductType.Text = txtProductType.DataResult["图号型号"].ToString();
            txtProductType.Tag = Convert.ToInt32(txtProductType.DataResult["序号"]);
            txtName.Text = txtProductType.DataResult["物品名称"].ToString();

            numCounts.Focus();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            DataRow dr = dt.NewRow();

            dr["总成型号"] = txtProductType.Text;
            dr["总成名称"] = txtName.Text;
            dr["总成数量"] = numCounts.Value;
            dt.Rows.Add(dr);

            dataGridView1.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

                dataGridView1.DataSource = dt;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt != null && dt.Rows.Count != 0)
            {
                m_dtSafeGoods = m_safeStockServer.GetSafeStockCountInfo(dt);

                if (m_dtSafeGoods.Rows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("无数据生成");
                }
                else
                {
                    this.Close();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_dtSafeGoods = null;

            this.Close();
        }
    }
}
