using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 库房盘点明细界面
    /// </summary>
    public partial class 库房盘点表 : Form
    {
        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IStoreageCheck m_serverStroageCheck = ServerModuleFactory.GetServerModule<IStoreageCheck>();

        /// <summary>
        /// 基础物品服务
        /// </summary>
        IBasicGoodsServer m_basicGoodsServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 明细表
        /// </summary>
        public DataTable m_dtMx = new DataTable();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 保存标志
        /// </summary>
        public int m_intFlag = 0;

        /// <summary>
        /// 主表信息
        /// </summary>
        S_StorageCheck m_billInfo = new S_StorageCheck();

        public 库房盘点表(string DJH,DataTable dt,AuthorityFlag m_authFlag,string strFlag)
        {
            InitializeComponent();
            m_billInfo = m_serverStroageCheck.GetBill(DJH);
            
            dataGridView1.DataSource = m_serverStroageCheck.GetList(DJH,dt);
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                txtZMJE.Visible = false;
                txtPDJE.Visible = false;
                label5.Visible = false;
                label8.Visible = false;
                dataGridView1.Columns["盘点金额"].Visible = false;
                dataGridView1.Columns["账面金额"].Visible = false;
                dataGridView1.Columns["盈亏金额"].Visible = false;
            }

            StapleInfo.InitUnitComboBox(cmbUnit);
            StapleInfo.InitStoreStateComboBox(cmbGoodsStatus);

            cmbUnit.SelectedIndex = -1;
            cmbGoodsStatus.SelectedIndex = -1;

            if (strFlag == "0")
            {
                toolStrip1.Visible = true;
            }

            m_dtMx = (DataTable)dataGridView1.DataSource;
            RefreshDataGirdView(m_dtMx);
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
                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 检查相同物品
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckSameGoods()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["图号型号"].Value.ToString() == txtCode.Text.Trim()
                    && dataGridView1.Rows[i].Cells["物品名称"].Value.ToString() == txtName.Text.Trim()
                    && dataGridView1.Rows[i].Cells["规格"].Value.ToString() == txtSpec.Text.Trim()
                    && dataGridView1.Rows[i].Cells["批次号"].Value.ToString() == txtBatchNo.Text.Trim()
                    && dataGridView1.Rows[i].Cells["供货单位"].Value.ToString() == txtProvider.Text.Trim())
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckDate()
        {
            if (txtName.Text.Trim() == ""
                || txtCode.Tag.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择或者填写物品名称");
                return false;
            }

            if (txtMaterialType.Text.Trim() == "" || txtMaterialType.Tag.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择材料类别");
                return false;
            }

            if (txtProvider.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择供货单位");
                return false;
            }

            if (cmbUnit.Text == "" || cmbUnit.SelectedValue == null)
            {
                MessageDialog.ShowPromptMessage("请选择相应的单位");
                return false;
            }

            if (cmbGoodsStatus.Text == "" || cmbGoodsStatus.SelectedValue == null)
            {
                MessageDialog.ShowPromptMessage("请选择相应的物品状态");
                return false;
            }

            if (!CheckSameGoods())
            {
                MessageDialog.ShowPromptMessage("不能录入同种图号型号，同种物品名称，同种规格，同一个批次号,请重新确认物品信息！");
                return false;
            }

            return true;
        }

        private void btnFindProvider_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetProviderInfoDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Text = form.GetDataItem("供应商编码").ToString();
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtCode.Tag = form.GetDataItem("序号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();

                View_F_GoodsPlanCost info = m_basicGoodsServer.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text, out m_err);

                if (info != null)
                {
                    cmbUnit.SelectedValue = info.单位ID;
                    txtMaterialType.Text = info.物品类别名称;
                    txtMaterialType.Tag = info.物品类别;
                }

                txtName.ReadOnly = true;
                txtCode.ReadOnly = true;
                txtSpec.ReadOnly = true;
                btnFindMaterialType.Visible = false;
            }
        }

        private void btnFindBatchNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetGoodsForBatchNo(Convert.ToInt32(txtCode.Tag), m_billInfo.StorageID);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtBatchNo.Text = form.GetDataItem("批次号").ToString();
                txtProviderBatchNo.Text = form.GetDataItem("供方批次号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();
                cmbUnit.Text = form.GetDataItem("单位").ToString();
                cmbUnit.SelectedValue = Convert.ToInt32( form.GetDataItem("单位ID").ToString());
                txtShelf.Text = form.GetDataItem("货架").ToString();
                txtColumn.Text = form.GetDataItem("列").ToString();
                txtLayer.Text = form.GetDataItem("层").ToString();
                cmbGoodsStatus.Text = form.GetDataItem("物品状态").ToString();
                cmbGoodsStatus.SelectedValue = Convert.ToInt32( form.GetDataItem("物品状态ID").ToString());

                txtZMSL.Text = form.GetDataItem("库存数量").ToString();
                txtZMJE.Tag = m_serverStore.GetGoodsUnitPrice(Convert.ToInt32(txtCode.Tag), txtBatchNo.Text, m_billInfo.StorageID);
                txtZMJE.Text = (Convert.ToDecimal(form.GetDataItem("库存数量")) * Convert.ToDecimal(txtZMJE.Tag)).ToString();
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.DataSource == null)
            {
                return;
            }
            else
            {
                txtCode.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtMaterialType.Text = dataGridView1.CurrentRow.Cells["材料类别名称"].Value.ToString();
                txtMaterialType.Tag = dataGridView1.CurrentRow.Cells["材料类别编码"].Value.ToString();
                txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
                txtProvider.Text = dataGridView1.CurrentRow.Cells["供货单位"].Value.ToString();
                txtProviderBatchNo.Text = dataGridView1.CurrentRow.Cells["供方批次号"].Value.ToString();
                cmbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                cmbUnit.SelectedValue = Convert.ToInt32( dataGridView1.CurrentRow.Cells["单位ID"].Value.ToString());
                txtShelf.Text = dataGridView1.CurrentRow.Cells["货架"].Value.ToString();
                txtColumn.Text = dataGridView1.CurrentRow.Cells["列"].Value.ToString();
                txtLayer.Text = dataGridView1.CurrentRow.Cells["层"].Value.ToString();
                cmbGoodsStatus.Text = dataGridView1.CurrentRow.Cells["物品状态"].Value.ToString();
                cmbGoodsStatus.SelectedValue = Convert.ToInt32( dataGridView1.CurrentRow.Cells["物品状态ID"].Value.ToString());
                txtZMJE.Text = dataGridView1.CurrentRow.Cells["账面金额"].Value.ToString();
                txtZMSL.Text = dataGridView1.CurrentRow.Cells["账面数量"].Value.ToString();
                txtPDJE.Text = dataGridView1.CurrentRow.Cells["盘点金额"].Value.ToString();
                txtPDSL.Text = dataGridView1.CurrentRow.Cells["盘点数量"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;
            dtTemp.AcceptChanges();
            dataGridView1.DataSource = dtTemp;

            //DataTable Dt = (DataTable)dataGridView1.DataSource;
            //Dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckDate())
            {
                int intGoodsID = m_basicGoodsServer.GetGoodsID(txtCode.Text.Trim(),
                    txtName.Text.Trim(),
                    txtSpec.Text.Trim(),
                    txtMaterialType.Tag.ToString().Trim(),
                    (int)cmbUnit.SelectedValue,
                    "盘点单自动生成",
                    out m_err);

                if (intGoodsID == 0)
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }

                txtZMJE.Tag = m_serverStore.GetGoodsAveragePrice(intGoodsID, txtBatchNo.Text);

                m_dtMx = (DataTable)dataGridView1.DataSource;
                DataRow dr = m_dtMx.NewRow();

                dr["物品ID"] = intGoodsID;
                dr["图号型号"] = txtCode.Text;
                dr["物品名称"] = txtName.Text;
                dr["规格"] = txtSpec.Text;
                dr["批次号"] = txtBatchNo.Text;
                dr["账面数量"] = Convert.ToDecimal(txtZMSL.Text);
                dr["账面金额"] = Convert.ToDecimal(txtZMJE.Tag) * Convert.ToDecimal(txtZMSL.Text);
                dr["盘点数量"] = Convert.ToDecimal(txtPDSL.Text);
                dr["盘点金额"] = Convert.ToDecimal(txtZMJE.Tag) * Convert.ToDecimal(txtPDJE.Text);
                dr["盈亏数量"] = Convert.ToDecimal(dr["盘点数量"]) - Convert.ToDecimal(dr["账面数量"]);
                dr["盈亏金额"] = Convert.ToDecimal(dr["盘点金额"]) - Convert.ToDecimal(dr["账面金额"]);
                dr["供货单位"] = txtProvider.Text;
                dr["单位"] = cmbUnit.Text;
                dr["物品状态"] = cmbGoodsStatus.Text;
                dr["材料类别名称"] = txtMaterialType.Text;
                dr["货架"] = txtShelf.Text;
                dr["列"] = txtColumn.Text;
                dr["层"] = txtLayer.Text;
                dr["供方批次号"] = txtProviderBatchNo.Text;
                dr["备注"] = txtRemark.Text;
                dr["单位ID"] = Convert.ToInt32( cmbUnit.SelectedValue);
                dr["物品状态ID"] = Convert.ToInt32(cmbGoodsStatus.SelectedValue);
                dr["材料类别编码"] = txtMaterialType.Tag;
                dr["单据号"] = m_billInfo.DJH;

                m_dtMx.Rows.Add(dr);
                m_dtMx.AcceptChanges();
                dataGridView1.DataSource = m_dtMx;
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.CurrentRow.Cells["盈亏数量"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["盘点数量"].Value) -
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面数量"].Value);

            dataGridView1.CurrentRow.Cells["盈亏金额"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面金额"].Value) == 0 ? 0 :
                (Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面金额"].Value) /
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面数量"].Value)) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["盈亏数量"].Value);

            dataGridView1.CurrentRow.Cells["盘点金额"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面金额"].Value) == 0 ? 0 :
                (Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面金额"].Value) /
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面数量"].Value)) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["盘点数量"].Value);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_dtMx = (DataTable)dataGridView1.DataSource;
            m_intFlag = 1;
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_dtMx = (DataTable)dataGridView1.DataSource;
            this.Close();
        }

        private void btnOutExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtBatchNo.Text = "";
            txtCode.Text = "";
            txtCode.Tag = -1;
            txtColumn.Text = "";
            txtLayer.Text = "";
            txtMaterialType.Text = "";
            txtName.Text = "";
            txtPDJE.Text = "0";
            txtPDSL.Text = "0";
            txtProvider.Text = "";
            txtProviderBatchNo.Text = "";
            txtRemark.Text = "";
            txtShelf.Text = "";
            txtSpec.Text = "";
            txtZMJE.Text = "0";
            txtZMSL.Text = "0";
            txtCode.ReadOnly = false;
            txtName.ReadOnly = false;
            txtSpec.ReadOnly = false;
            btnFindMaterialType.Visible = true;
        }

        private void btnFindMaterialType_Click(object sender, EventArgs e)
        {

            FormDepotType form = new FormDepotType();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtMaterialType.Text = form.SelectedDepotType.仓库名称;
                txtMaterialType.Tag = form.SelectedDepotType.仓库编码;
            }
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (txtCode.Tag == null || txtCode.Tag.ToString().Length == 0 || (int)txtCode.Tag == 0)
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return;
            }

            //DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            //int rowIndex = dataGridView1.CurrentRow.Index;

            dataGridView1.CurrentRow.Cells["图号型号"].Value = txtCode.Text;
            dataGridView1.CurrentRow.Cells["物品名称"].Value = txtName.Text;
            dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
            dataGridView1.CurrentRow.Cells["物品ID"].Value = txtCode.Tag;
            dataGridView1.CurrentRow.Cells["批次号"].Value = txtBatchNo.Text;
            dataGridView1.CurrentRow.Cells["账面数量"].Value = Convert.ToDecimal(txtZMSL.Text);
            dataGridView1.CurrentRow.Cells["账面金额"].Value = Convert.ToDecimal(txtZMJE.Tag) * Convert.ToDecimal(txtZMSL.Text);
            dataGridView1.CurrentRow.Cells["盘点数量"].Value = Convert.ToDecimal(txtPDSL.Text);
            dataGridView1.CurrentRow.Cells["盘点金额"].Value = Convert.ToDecimal(txtZMJE.Tag) * Convert.ToDecimal(txtPDJE.Text);
            dataGridView1.CurrentRow.Cells["盈亏数量"].Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["盘点数量"].Value)
                - Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面数量"].Value);
            dataGridView1.CurrentRow.Cells["盈亏金额"].Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["盘点金额"].Value)
                - Convert.ToDecimal(dataGridView1.CurrentRow.Cells["账面金额"].Value);
            dataGridView1.CurrentRow.Cells["供货单位"].Value = txtProvider.Text;
            dataGridView1.CurrentRow.Cells["单位"].Value = cmbUnit.Text;
            dataGridView1.CurrentRow.Cells["物品状态"].Value = cmbGoodsStatus.Text;
            dataGridView1.CurrentRow.Cells["材料类别名称"].Value = txtMaterialType.Text;
            dataGridView1.CurrentRow.Cells["货架"].Value = txtShelf.Text;
            dataGridView1.CurrentRow.Cells["列"].Value = txtColumn.Text;
            dataGridView1.CurrentRow.Cells["层"].Value = txtLayer.Text;
            dataGridView1.CurrentRow.Cells["供方批次号"].Value = txtProviderBatchNo.Text;
            dataGridView1.CurrentRow.Cells["备注"].Value = txtRemark.Text;
            dataGridView1.CurrentRow.Cells["单位ID"].Value = Convert.ToInt32(cmbUnit.SelectedValue);
            dataGridView1.CurrentRow.Cells["物品状态ID"].Value = Convert.ToInt32(cmbGoodsStatus.SelectedValue);
            dataGridView1.CurrentRow.Cells["材料类别编码"].Value = txtMaterialType.Tag;
            dataGridView1.CurrentRow.Cells["单据号"].Value = m_billInfo.DJH;

            int goodsID = Convert.ToInt32(txtCode.Tag);
            string batchNo = txtBatchNo.Text;
            string provider = txtProvider.Text;

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;
            dtTemp.AcceptChanges();
            dataGridView1.DataSource = dtTemp;
            PositioningRecord(dataGridView1, goodsID, batchNo, provider);
            //dataGridView1.DataSource = dtTemp;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(DataGridView datagridview, int goodsID, string batchNo, string provider)
        {
            for (int i = 0; i < datagridview.Rows.Count; i++)
            {
                if ((int)datagridview.Rows[i].Cells["物品ID"].Value == goodsID
                    && (string)datagridview.Rows[i].Cells["批次号"].Value == batchNo
                    && (string)datagridview.Rows[i].Cells["供货单位"].Value == provider)
                {
                    datagridview.FirstDisplayedScrollingRowIndex = i;
                    datagridview.CurrentCell = datagridview.Rows[i].Cells["批次号"];
                }
            }
        }

    }
}
