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
    /// <summary>
    /// 样品库存查询界面
    /// </summary>
    public partial class 样品库库存查询 : Form
    {
        /// <summary>
        /// 样品服务组件
        /// </summary>
        IMusterAffirmBill m_MusterBill = ServerModuleFactory.GetServerModule<IMusterAffirmBill>();

        /// <summary>
        /// 数据集
        /// </summary>
        S_MusterStock m_lnqMusterStock = new S_MusterStock();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        public 样品库库存查询(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            RefreshDataGirdView(m_MusterBill.GetAllMusterStock(chkIsShowZeroStock.Checked));
        }

        private void 样品库库存查询_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
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
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(int goodsID, string batchNo)
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
                if ((int)dataGridView1.Rows[i].Cells["物品ID"].Value == goodsID
                    && (string)dataGridView1.Rows[i].Cells["批次号"].Value == batchNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
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

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 查询库存ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_MusterBill.GetAllMusterStock(chkIsShowZeroStock.Checked));
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtCode.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString());
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
                txtShelf.Text = dataGridView1.CurrentRow.Cells["区域"].Value.ToString();
                txtColumn.Text = dataGridView1.CurrentRow.Cells["列"].Value.ToString();
                txtLayer.Text = dataGridView1.CurrentRow.Cells["层"].Value.ToString();
            }
        }

        private void chkIsShowZeroStock_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_MusterBill.GetAllMusterStock(chkIsShowZeroStock.Checked));
        }

        private void 修改ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_lnqMusterStock.GoodsID = Convert.ToInt32(txtCode.Tag);
            m_lnqMusterStock.BatchNo = txtBatchNo.Text;
            m_lnqMusterStock.ColumnNumber = txtColumn.Text;
            m_lnqMusterStock.LayerNumber = txtLayer.Text;
            m_lnqMusterStock.ShelfAarea = txtShelf.Text;

            if (!m_MusterBill.UpdateMusterStockInfo(m_lnqMusterStock, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }

            RefreshDataGirdView(m_MusterBill.GetAllMusterStock(chkIsShowZeroStock.Checked));
            PositioningRecord((int)m_lnqMusterStock.GoodsID, (string)m_lnqMusterStock.BatchNo);
        }
    }
}
