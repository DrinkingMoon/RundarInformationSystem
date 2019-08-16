using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using Expression;
using PlatformManagement;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 库存信息 : Form
    {

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 二级库房数据集
        /// </summary>
        Out_Stock m_lnqStock = new Out_Stock();

        /// <summary>
        /// 业务服务组件
        /// </summary>
        IBusinessOperation m_serverBusiness = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBusinessOperation>();

        public 库存信息(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            AuthorityControl(m_authFlag);

            DataTable dt = UniversalFunction.GetSecStorageTb();

            if (dt != null)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbStorage.Items.Add(dt.Rows[i]["SecStorageName"].ToString());
                }
            }

            RefrshData();
            chbIsShowZero_CheckedChanged(null, null);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            dataGridView1.DataSource = m_serverBusiness.GetStockInfo();

            dataGridView1.Columns["序号"].Visible = false;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            txtAccountingStorage.Text = "";
            txtAccountingStorage.Tag = -1;

            txtGoodsName.Tag = 0;
            txtGoodsCode.Text = "";
            txtGoodsCode.Tag = -1;
            txtGoodsName.Text = "";
            txtGoodsName.Tag = -1;
            txtPersonnel.Text = "";
            txtPrincipal.Text = "";
            txtRemark.Text = "";
            txtSpec.Text = "";
            numOperationCount.Value = 0;
            dtpDate.Value = ServerTime.Time;
            cmbStorage.SelectedIndex = -1;
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetMessage()
        {
            m_lnqStock.ID = Convert.ToInt32(txtGoodsName.Tag);
            m_lnqStock.Date = dtpDate.Value;
            m_lnqStock.GoodsID = Convert.ToInt32(txtGoodsCode.Tag);
            m_lnqStock.Personnel = BasicInfo.LoginName;
            m_lnqStock.Remark = txtRemark.Text;
            m_lnqStock.SecStorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            m_lnqStock.StockQty = numOperationCount.Value;
            m_lnqStock.StorageID = txtAccountingStorage.Tag.ToString();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefrshData();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="info">定位用的信息</param>
        /// <param name="info1">定位用的信息</param>
        void PositioningRecord(string info,string info1)
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
                if (dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == info1 
                    && (string)dataGridView1.Rows[i].Cells["所属库房ID"].Value == info)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            txtGoodsCode.Tag = Convert.ToInt32(txtGoodsCode.DataResult["序号"]);
            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
            lbUnit.Text = txtGoodsCode.DataResult["单位"].ToString();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtGoodsCode.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtPersonnel.Text = dataGridView1.CurrentRow.Cells["操作人员"].Value.ToString();
                //txtPrincipal.Text = dataGridView1.CurrentRow.Cells["负责人"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                numOperationCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
                dtpDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["操作日期"].Value);
                cmbStorage.Text = dataGridView1.CurrentRow.Cells["所属库房"].Value.ToString();
                cmbStorage.Tag = dataGridView1.CurrentRow.Cells["所属库房ID"].Value.ToString();
                txtAccountingStorage.Text = dataGridView1.CurrentRow.Cells["所属账务库房"].Value.ToString();
                txtAccountingStorage.Tag = dataGridView1.CurrentRow.Cells["所属账务库房ID"].Value.ToString();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverBusiness.OperationStockInfo(m_lnqStock,"添加",out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }

            RefrshData();
            PositioningRecord(m_lnqStock.SecStorageID,m_lnqStock.GoodsID.ToString());
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverBusiness.OperationStockInfo(m_lnqStock, "修改", out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }

            RefrshData();
            PositioningRecord(m_lnqStock.SecStorageID, m_lnqStock.GoodsID.ToString());
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (MessageDialog.ShowEnquiryMessage("是否要删除此条库存记录?") == DialogResult.Yes)
            {
                if (!m_serverBusiness.OperationStockInfo(m_lnqStock, "删除", out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
            }

            RefrshData();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void txtAccountingStorage_OnCompleteSearch()
        {
            txtAccountingStorage.Text = txtAccountingStorage.DataResult["库房名称"].ToString();
            txtAccountingStorage.Tag = txtAccountingStorage.DataResult["库房编码"].ToString();
        }

        private void 库存信息_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        private void chbIsShowZero_CheckedChanged(object sender, EventArgs e)
        {
            DataTable dtSource = m_serverBusiness.GetStockInfo();

            if (!chbIsShowZero.Checked)
            {
                DataTable dtNew = dtSource.Clone();

                DataRow[] dr = dtSource.Select("数量 > 0");

                for (int i = 0; i < dr.Length; i++)
                {
                    dtNew.ImportRow(dr[i]);
                }

                dataGridView1.DataSource = dtNew;
            }
            else
            {
                dataGridView1.DataSource = dtSource;
            }
        }
    }
}
