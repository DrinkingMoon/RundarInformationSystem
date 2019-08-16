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
    /// 库存物品防锈界面
    /// </summary>
    public partial class 库存物品防锈 : Form
    {
        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        IGoodsAntirust m_serverAntirust = ServerModuleFactory.GetServerModule<IGoodsAntirust>();

        /// <summary>
        /// 库存防锈物品
        /// </summary>
        DataTable m_dtStockAntriust = new DataTable();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 点击事件标志
        /// </summary>
        bool m_blCheck = false;

        public 库存物品防锈(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
        }

        private void 库存物品防锈_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            groupBox1.Visible = false;
            panelPara.Height = 40;
        }

        void tbsCode_OnCompleteSearch()
        {
            tbsCode.Tag = Convert.ToInt32(tbsCode.DataResult["序号"].ToString());
            txtName.Text = tbsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = tbsCode.DataResult["规格"].ToString();
            tbsCode.Text = tbsCode.DataResult["图号型号"].ToString();
        }

        /// <summary>
        /// 检测日期 
        /// </summary>
        /// <param name="dtAntirustDate">防锈日期</param>
        /// <param name="intAntirustTime">防锈期</param>
        /// <returns>返回值 0：正常，1：黄色报警，2：红色过期</returns>
        private int CheckAntirustDate(DateTime dtAntirustDate, int intAntirustTime)
        {
            int i = DateTime.Compare(dtAntirustDate.AddMonths(intAntirustTime), ServerTime.Time);

            int f = DateTime.Compare(dtAntirustDate.AddMonths(intAntirustTime).AddDays(-15), ServerTime.Time);

            if (i < 0)
            {
                return 2;
            }
            else
            {
                if (f < 0)
                {
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["序号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void 查询库存的防锈状况ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            m_blCheck = true;
            contextMenuStrip1.Enabled = true;
            groupBox1.Visible = false;
            panelPara.Height = 40;
            chkIsNormal.Visible = true;
            checkBox2.Visible = true;
            checkBox3.Visible = true;
            label3.Visible = true;
            dataGridView1.DataSource = null;

            DataGridViewCheckBoxColumn 选 = new DataGridViewCheckBoxColumn();

            选.DataPropertyName = "选";
            选.HeaderText = "选";
            选.Name = "选";
            选.ReadOnly = true;
            选.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            选.Width = 40;

            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { 选});
            m_dtStockAntriust = m_serverAntirust.GetStockAntirustCheck();
            GetDataTable();

            
            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["序号"].Visible = false;
        }

        private void 查询物品防锈设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_blCheck = false;
            groupBox1.Visible = true;
            panelPara.Height = 97;
            chkIsNormal.Visible = false;
            checkBox2.Visible = false;
            checkBox3.Visible = false;
            label3.Visible = false;
            dataGridView1.DataSource = null;
            contextMenuStrip1.Enabled = false;
            RefreshDataGirdView(m_serverAntirust.GetBaseGoodsAntirustSet());

            dataGridView1.Columns["序号"].Visible = false;
        }

        private void 全部选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = true;
            }
        }

        private void 全部取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = false;
            }
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                dgvr.Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in dataGridView1.SelectedRows)
            {
                dgvr.Cells["选"].Value = false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (tbsCode.Tag != null && tbsCode.Tag.ToString() != "")
            {
                if (!m_serverAntirust.AddAntirustInfo((int)tbsCode.Tag,nudAntirust.Value,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
            }

            RefreshDataGirdView(m_serverAntirust.GetBaseGoodsAntirustSet());
            PositioningRecord(m_serverAntirust.GetMaxID());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count != 0)
            {
                if (!m_serverAntirust.DeleteAntirustInfo((int)dataGridView1.CurrentRow.Cells["物品ID"].Value,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
            }

            RefreshDataGirdView(m_serverAntirust.GetBaseGoodsAntirustSet());
        }

        private void 执行防锈操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (groupBox1.Visible)
            {
                MessageDialog.ShowPromptMessage("请先点击【查询库存的防锈状况】!");
                return;
            }

            DataTable dt = ((DataTable)dataGridView1.DataSource).Clone();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean( dataGridView1.Rows[i].Cells["选"].Value))
                {
                    DataRow dr = dt.NewRow();

                    dr["物品ID"] = (int)dataGridView1.Rows[i].Cells["物品ID"].Value;
                    dr["批次号"] = dataGridView1.Rows[i].Cells["批次号"].Value.ToString();
                    dr["不合格数"] = (int)dataGridView1.Rows[i].Cells["不合格数"].Value;
                    dr["库房ID"] = dataGridView1.Rows[i].Cells["库房ID"].Value.ToString();
                    dr["供应商"] = dataGridView1.Rows[i].Cells["供应商"].Value.ToString();
                    dt.Rows.Add(dr);
                }
            }

            if (!m_serverAntirust.ExceAntirustInfo(dt,out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            m_dtStockAntriust = m_serverAntirust.GetStockAntirustCheck();
            GetDataTable();
        }

        private void 审核库存防锈ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)dataGridView1.DataSource).Clone();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean( dataGridView1.Rows[i].Cells["选"].Value) 
                    && dataGridView1.Rows[i].Cells["防锈状态"].Value.ToString() == "等待审核")
                {
                    DataRow dr = dt.NewRow();

                    dr["物品ID"] = (int)dataGridView1.Rows[i].Cells["物品ID"].Value;
                    dr["批次号"] = dataGridView1.Rows[i].Cells["批次号"].Value.ToString();
                    dr["不合格数"] = (int)dataGridView1.Rows[i].Cells["不合格数"].Value;
                    dr["库房ID"] = dataGridView1.Rows[i].Cells["库房ID"].Value.ToString();
                    dr["供应商"] = dataGridView1.Rows[i].Cells["供应商"].Value.ToString();
                    dt.Rows.Add(dr);
                }
            }

            if (!m_serverAntirust.AuditingAntirustInfo(dt, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            m_dtStockAntriust = m_serverAntirust.GetStockAntirustCheck();
            GetDataTable();
        }

        private void GetDataTable()
        {
            
            DataTable dt = m_dtStockAntriust.Clone();

            string strSql = "";

            if (chkIsNormal.Checked)
            {
                strSql = strSql + "(物品质量状态 = '正常') or";
            }

            if (checkBox2.Checked)
            {
                strSql = strSql + "(物品质量状态 = '预过期') or";
            }

            if (checkBox3.Checked)
            {
                strSql = strSql + "(物品质量状态 = '已过期') or";
            }

            if (strSql.Length != 0)
            {
                strSql = strSql.Substring(0, strSql.Length - 3);

                DataRow[] dr = m_dtStockAntriust.Select(strSql);

                for (int i = 0; i < dr.Length; i++)
                {
                    dt.ImportRow(dr[i]);
                }
            }

            RefreshDataGirdView(dt);
        }

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (m_blCheck)
            {
                if (e.ColumnIndex == 3)
                {
                    if ((dataGridView1.CurrentRow.Cells["防锈状态"].Value.ToString() == "未防锈"
                        && 仓管员操作ToolStripMenuItem.Visible == true)
                         || (dataGridView1.CurrentRow.Cells["防锈状态"].Value.ToString() == "等待审核"
                        && 质管部操作ToolStripMenuItem.Visible == true))
                    {
                        dataGridView1.ReadOnly = false;
                    }
                }
                else
                {
                    dataGridView1.ReadOnly = true;

                    if (e.ColumnIndex == 0)
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
                }
            }
        }

        private void 确认防锈操作ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = ((DataTable)dataGridView1.DataSource).Clone();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (Convert.ToBoolean(dataGridView1.Rows[i].Cells["选"].Value) 
                    && dataGridView1.Rows[i].Cells["防锈状态"].Value.ToString() == "等待确认")
                {
                    DataRow dr = dt.NewRow();

                    dr["物品ID"] = (int)dataGridView1.Rows[i].Cells["物品ID"].Value;
                    dr["批次号"] = dataGridView1.Rows[i].Cells["批次号"].Value.ToString();
                    dr["不合格数"] = (int)dataGridView1.Rows[i].Cells["不合格数"].Value;
                    dr["库房ID"] = dataGridView1.Rows[i].Cells["库房ID"].Value.ToString();
                    dr["供应商"] = dataGridView1.Rows[i].Cells["供应商"].Value.ToString();
                    dt.Rows.Add(dr);
                }
            }

            if (!m_serverAntirust.AuthorizeAntirustInfo(dt, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }


            m_dtStockAntriust = m_serverAntirust.GetStockAntirustCheck();
            GetDataTable();
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (m_blCheck)
            {
                if (Convert.ToDecimal( dataGridView1.CurrentRow.Cells["不合格数"].Value) > 
                    Convert.ToDecimal(dataGridView1.CurrentRow.Cells["库存数"].Value))
                {
                    MessageDialog.ShowPromptMessage("不合格数不能大于库存数");
                    dataGridView1.CurrentRow.Cells["不合格数"].Value = 0;
                }
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns.Count == 0)
            {
                return;
            }

            bool blFlag = false;

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].Name.ToString() == "防锈次数")
                {
                    blFlag = true;
                }
            }

            if (blFlag)
            {
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    if (dataGridView1.Rows[k].Cells["防锈次数"].Value.ToString() == "0")
                    {
                        dataGridView1.Rows[k].DefaultCellStyle.BackColor = Color.Green;
                    }

                    if (dataGridView1.Rows[k].Cells["物品质量状态"].Value.ToString() == "预过期")
                    {
                        dataGridView1.Rows[k].DefaultCellStyle.BackColor = Color.Yellow;
                    }

                    if (dataGridView1.Rows[k].Cells["物品质量状态"].Value.ToString() == "已过期")
                    {
                        dataGridView1.Rows[k].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }

        private void chkIsNormal_CheckedChanged(object sender, EventArgs e)
        {
            if ((DataTable)dataGridView1.DataSource != null)
            {
                GetDataTable();
            }
        }
    }
}
