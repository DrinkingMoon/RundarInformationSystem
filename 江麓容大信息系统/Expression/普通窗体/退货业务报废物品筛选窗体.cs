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

namespace Expression
{
    /// <summary>
    /// 退货业务报废物品筛选界面
    /// </summary>
    public partial class 退货业务报废物品筛选窗体 : Form
    {
        /// <summary>
        /// 单据服务组件
        /// </summary>
        IMaterialListRejectBill m_billServer = ServerModuleFactory.GetServerModule<IMaterialListRejectBill>();

        /// <summary>
        /// 报废物品集合
        /// </summary>
        private DataTable m_dtScrap = null;

        public DataTable DtScrap
        {
            get { return m_dtScrap; }
            set { m_dtScrap = value; }
        }

        /// <summary>
        /// 供应商编码
        /// </summary>
        string m_strProvider;

        public 退货业务报废物品筛选窗体(string provider)
        {
            m_strProvider = provider;
            InitializeComponent();
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                dgvr.Cells["Sel"].Value = 1;
            }

            SetBackColor();
        }

        /// <summary>
        /// 设置背景色
        /// </summary>
        private void SetBackColor()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            if (dt.Rows.Count == 0 || dt == null)
            {
                return;
            }
            else
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["Sel"].Value.ToString() == "1")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    }
                }
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                dgvr.Cells["Sel"].Value = 0;
            }

            SetBackColor();
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Sel"].Value = 0;
                dt.Rows[i]["Sel"] = 0;
            }

            dataGridView1.DataSource = dt;
            SetBackColor();
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["Sel"].Value = 1;
                dt.Rows[i]["Sel"] = 1;
            }

            dataGridView1.DataSource = dt;
            SetBackColor();
        }

        private void 退货业务报废物品筛选窗体_Load(object sender, EventArgs e)
        {
            if (m_strProvider == "")
            {
                btnSure.Visible = false;
                Sel.Visible = false;
            }

            DataTable dt = m_billServer.GetScrapGoods(m_strProvider);
            dataGridView1.DataSource = dt;
        }

        private void btnSure_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            m_dtScrap = dt.Clone();
            DataRow[] dr = dt.Select("Sel = '1'");

            if (dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    m_dtScrap.ImportRow(dr[i]);
                }
            }
              
            this.Close();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["Sel"].Value.ToString() == "0")
            {
                dataGridView1.CurrentRow.Cells["Sel"].Value = 1;
            }
            else
            {
                dataGridView1.CurrentRow.Cells["Sel"].Value = 0;
            }
        }
    }
}
