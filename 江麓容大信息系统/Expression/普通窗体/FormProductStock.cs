using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class FormProductStock : Form
    {
        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 不合格产品隔离服务类
        /// </summary>
        IQuarantine s_quarantine = ServerModuleFactory.GetServerModule<IQuarantine>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 数据集
        /// </summary>
        private DataTable dt = new DataTable();

        public DataTable Dt
        {
            get { return dt; }
            set { dt = value; }
        }

        public FormProductStock(string storage)
        {
            InitializeComponent();

            DataTable dtStock = s_quarantine.GetProductStockInfo(null, storage, out error);
            dataGridView1.DataSource = dtStock;
            dataGridView1.Columns["GoodsID"].Visible = false;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(
                    dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                                                           this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panel1.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
            
        }

        public FormProductStock(string GoodsID,string storage)
        {
            InitializeComponent();

            DataTable dtStock = s_quarantine.GetProductStockInfo(GoodsID,storage, out error);
            dataGridView1.DataSource = dtStock;
            dataGridView1.Columns["GoodsID"].Visible = false;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(
                    dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                                                           this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panel1.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
            
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (Convert.ToBoolean(dataGridView1.CurrentRow.Cells["选"].Value))
            {
                dataGridView1.CurrentRow.Cells["选"].Value = false;
            }
            else
            {
                dataGridView1.CurrentRow.Cells["选"].Value = true;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            for (int k = 0; k <= dataGridView1.Columns.Count - 1; k++)
            {
                dt.Columns.Add(dataGridView1.Columns[k].HeaderText);
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value))
                {
                    DataRow dr = dt.NewRow();

                    for (int j = 1; j <= dataGridView1.Columns.Count - 1; j++)
                    {
                        dr[j] = dataGridView1.Rows[i].Cells[j].Value;
                    }
                    dt.Rows.Add(dr);
                }
            }
            this.Close();
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = false;
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = false;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = true;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
