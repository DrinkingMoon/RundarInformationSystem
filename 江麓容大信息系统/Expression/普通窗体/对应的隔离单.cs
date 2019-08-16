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
    /// 对应的隔离单显示界面
    /// </summary>
    public partial class 对应的隔离单 : Form
    {
        /// <summary>
        /// 对应的隔离单号
        /// </summary>
        public DataTable m_dtIsolation = new DataTable();

        /// <summary>
        /// 对应的订单号
        /// </summary>
        string m_strOrderFormNumber = "";

        /// <summary>
        /// 所属库房ID
        /// </summary>
        string m_strStorageID;

        /// <summary>
        /// 单据服务组件
        /// </summary>
        IMaterialListRejectBill m_billServer = ServerModuleFactory.GetServerModule<IMaterialListRejectBill>();

        public 对应的隔离单(string strOrderNumber,string strStorageID)
        {
            InitializeComponent();
            m_strOrderFormNumber = strOrderNumber;
            m_strStorageID = strStorageID;

            if (m_strOrderFormNumber == "")
            {
                btnOK.Visible = false;
                Sel.Visible = false;
            }

            dataGridView1.DataSource = m_billServer.GetIsolationBill(m_strOrderFormNumber,m_strStorageID);
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
        /// 设置背景颜色
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;
            m_dtIsolation = dt.Clone();
            DataRow[] dr = dt.Select("Sel = '1'");

            if (dr.Length > 0)
            {
                for (int i = 0; i < dr.Length; i++)
                {
                    m_dtIsolation.ImportRow(dr[i]);
                }
            }

            this.Close();
        }
    }
}
