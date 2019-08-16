using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using AsynSocketService;
using System.Net;
using System.Net.Sockets;
using SocketCommDefiniens;
using ServerRequestProcessorModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 物品质保有效期管理界面
    /// </summary>
    public partial class 物品质保有效期管理 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 物品保质期监控服务组件
        /// </summary>
        IGoodsShelfLife m_serverShelfLife = ServerModuleFactory.GetServerModule<IGoodsShelfLife>();

        /// <summary>
        /// 数据集
        /// </summary>
        KF_GoodsShelfLife m_lnqGoodsShelfLife = new KF_GoodsShelfLife();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 物品质保有效期管理(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            dtpEnd.Value = ServerTime.Time;
            dtpStart.Value = ServerTime.Time.AddYears(-5);

            RefreshDataGirdView();
        }

        private void 物品质保有效期管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(menuStrip, m_authFlag);
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetInfo()
        {
            m_lnqGoodsShelfLife.BatchNo = txtBatchNo.Text;
            m_lnqGoodsShelfLife.DateInProduced = dtpDateInProduced.Value;
            m_lnqGoodsShelfLife.GoodsID = Convert.ToInt32(txtCode.Tag);
            m_lnqGoodsShelfLife.ShelfLife = nudShelfLife.Value;
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                dtpDateInProduced.Value = dataGridView1.CurrentRow.Cells["生产日期"].Value == DBNull.Value ? ServerTime.Time :
                    Convert.ToDateTime(dataGridView1.CurrentRow.Cells["生产日期"].Value);
                nudShelfLife.Value = dataGridView1.CurrentRow.Cells["保质期"].Value == DBNull.Value ? 0 :
                    Convert.ToDecimal(dataGridView1.CurrentRow.Cells["保质期"].Value);
                txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        private void RefreshDataGirdView()
        {

            DataTable dtShelfLife = m_serverShelfLife.GetInfo(dtpStart.Value, dtpEnd.Value);

            DataTable dt = dtShelfLife.Clone();

            string strSql = "";

            if (chkIsNormal.Checked)
            {
                strSql = strSql + "(物品状态 = '正常') or";
            }

            if (checkBox2.Checked)
            {
                strSql = strSql + "(物品状态 = '预过期') or";
            }

            if (checkBox3.Checked)
            {
                strSql = strSql + "(物品状态 = '已过期') or";
            }

            if (strSql.Length != 0)
            {
                strSql = strSql.Substring(0, strSql.Length - 3);

                DataRow[] dr = dtShelfLife.Select(strSql);

                for (int i = 0; i < dr.Length; i++)
                {
                    dt.ImportRow(dr[i]);
                }
            }

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["删除"].Width = 40;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void chkIsNormal_CheckedChanged(object sender, EventArgs e)
        {
            if ((DataTable)dataGridView1.DataSource != null)
            {
                RefreshDataGirdView();
            }
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Columns.Count == 0)
            {
                return;
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["物品状态"].Value.ToString() == "已过期")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                    else if (dataGridView1.Rows[i].Cells["物品状态"].Value.ToString() == "预过期")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Yellow;
                    }

                    if (dataGridView1.Rows[i].Cells["图号型号"].Value.ToString().Trim().Length == 0)
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Blue;
                    }
                }
            }
        }

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!m_serverShelfLife.UpdateDeleteFlag(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), dataGridView1.CurrentRow.Cells["批次号"].Value.ToString()))
            {
                MessageDialog.ShowPromptMessage("操作失败");
            }
            else
            {
                MessageDialog.ShowPromptMessage("操作成功");
            }

            RefreshDataGirdView();
        }

        private void 审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Convert.ToBoolean(dataGridView1.CurrentRow.Cells["删除"].Value))
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录进行审核");
                return;
            }
            else
            {
                if (!m_serverShelfLife.DeleteInfo(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value), dataGridView1.CurrentRow.Cells["批次号"].Value.ToString()))
                {
                    MessageDialog.ShowPromptMessage("审核失败");
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审核成功");
                }

                RefreshDataGirdView();
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
        }

        private void 保质物品列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDataShow frm = new FormDataShow(m_serverShelfLife.GetBASEInfo());

            frm.ShowDialog();
        }

        private void 设置保质期ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!m_serverShelfLife.SetInfo(m_lnqGoodsShelfLife, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }

            RefreshDataGirdView();
        }
    }
}
