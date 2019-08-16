using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 三包外返修领料明细界面
    /// </summary>
    public partial class 三包外返修领料明细 : Form
    {
        /// <summary>
        /// 库存信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 数据集
        /// </summary>
        private YX_ThreePacketsOfTheRepairBill m_lnqBill = new YX_ThreePacketsOfTheRepairBill();

        public YX_ThreePacketsOfTheRepairBill LnqBill
        {
            get { return m_lnqBill; }
            set { m_lnqBill = value; }
        }

        /// <summary>
        /// 三包外服务组件
        /// </summary>
        IThreePacketsOfTheRepairBill m_serverThreePacketsOfTheRepair = 
            ServerModuleFactory.GetServerModule<IThreePacketsOfTheRepairBill>();

        public 三包外返修领料明细(AuthorityFlag m_authFlag,string billID)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip, m_authFlag);
            this.toolStrip.Visible = true;

            RefreshInfo(m_serverThreePacketsOfTheRepair.GetList(billID));

            userControlDataLocalizer1.OnlyLocalize = true;

            m_lnqBill = m_serverThreePacketsOfTheRepair.GetBill(billID, out m_strErr);

            if (m_lnqBill == null)
            {
                m_lnqBill.Bill_ID = billID;
            }
            else
            {
                numMarketingStrategy.Value = Convert.ToDecimal(m_lnqBill.MarketingStrategy);
                numRepairTaskTime.Value = Convert.ToDecimal(m_lnqBill.RepairTaskTime);
                txtPlantRemark.Text =
                    m_lnqBill.PlantRemark == null ? "" : m_lnqBill.PlantRemark.ToString();

                SumPrice();
            }

            txtName.Enabled = btnAdd.Enabled;

            if (dataGridView1.Rows.Count == 0 && m_lnqBill != null && m_lnqBill.DJZT == "等待领料明细申请")
            {
                dataGridView1.DataSource = m_serverThreePacketsOfTheRepair.InsertThreePacketsOfTheRepairList(m_lnqBill.Bill_ID, out m_strErr);

                if (m_strErr != null)
                {
                    FormLargeMessage form = new FormLargeMessage(m_strErr, Color.Red);
                    form.ShowDialog();
                }

                btnShortcutSelect.Visible = true;
            }
            else
            {
                btnShortcutSelect.Visible = false;
            }
        }

        /// <summary>
        /// 计算各种费用
        /// </summary>
        void SumPrice()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            lbCLF.Text = (Math.Round( Convert.ToDecimal( dt.Compute("Sum(策略金额)", "").ToString() == "" ?
                "0": dt.Compute("Sum(策略金额)", "").ToString()),2)).ToString();
            lbGSF.Text = (Math.Round( numRepairTaskTime.Value * 55,2)).ToString();
            lbGLF.Text = (Math.Round(Convert.ToDecimal(lbCLF.Text) * 20 / 100, 2)).ToString();
            lbZFY.Text =  (Convert.ToDecimal( lbCLF.Text) + Convert.ToDecimal( lbGLF.Text) + Convert.ToDecimal( lbGSF.Text)).ToString();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearMessage()
        {
            txtName.Text = "";
            txtCode.Text = "";
            txtSpec.Text = "";
            txtBatchNo.Text = "";
            txtRemark.Text = "";
            numPickingCount.Value = 0;

            numPickingCount.Tag = null;
            txtName.Tag = null;
            txtBatchNo.Tag = null;
            txtSpec.Tag = null;
        }

        private void numRepairTaskTime_ValueChanged(object sender, EventArgs e)
        {
            SumPrice();
        }

        private void numMarketingStrategy_ValueChanged(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dt.Rows[i]["策略金额"] = Math.Round( (decimal)dt.Rows[i]["金额"] * (numMarketingStrategy.Value / 100 + 1),2);
            }

            RefreshInfo(dt);

            SumPrice();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (numPickingCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("添加物品数量不能为0");
                return;
            }
            else if (!m_serverStore.IsBatchNoOfGoodsExist(Convert.ToInt32(txtName.Tag),txtBatchNo.Text.ToString()))
            {
                MessageDialog.ShowPromptMessage("此批次在库房不存在，请重新选择");
                return;
            }
            else if (numPickingCount.Value > (txtSpec.Tag == null ? 0 : Convert.ToDecimal(txtSpec.Tag)))
            {
                MessageDialog.ShowPromptMessage("领用数大于库存数【"+ txtSpec.Tag +"】，请重新核对领用数量");
                return;
            }

            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["物品ID"].ToString() == txtName.Tag.ToString()
                    && dt.Rows[i]["批次号"].ToString() == txtBatchNo.Text)
                {
                    MessageDialog.ShowPromptMessage("不能添加同一物品，同一批次号");
                    return;
                }
            }

            DataRow dr = dt.NewRow();

            dr["图号型号"] = txtCode.Text;
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["批次号"] = txtBatchNo.Text;
            dr["领用数量"] = numPickingCount.Value;
            dr["单位"] = txtBatchNo.Tag.ToString();
            dr["单价"] = 0;
            dr["金额"] = 0;
            dr["策略金额"] = 0;
            dr["备注"] = txtRemark.Text;
            dr["物品ID"] = (int)txtName.Tag;
            dr["单据号"] = m_lnqBill.Bill_ID;
            dr["是否为客户责任"] = false;

            dt.Rows.Add(dr);

            RefreshInfo(dt);

            ClearMessage();

            SumPrice();

            PositioningRecord(Convert.ToInt32(dr["物品ID"]), dr["批次号"].ToString());
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (numPickingCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("添加物品数量不能为0");
                return;
            }
            else if (!m_serverStore.IsBatchNoOfGoodsExist(Convert.ToInt32(txtName.Tag), txtBatchNo.Text.ToString()))
            {
                MessageDialog.ShowPromptMessage("此批次在库房不存在，请重新选择");
                return;
            }
            else if (numPickingCount.Value > (txtSpec.Tag == null? 0 : Convert.ToDecimal(txtSpec.Tag)))
            {
                MessageDialog.ShowPromptMessage("领用数大于库存数【" + txtSpec.Tag + "】，请重新核对领用数量");
                return;
            }

            int intIndex = dataGridView1.CurrentRow.Index;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != dataGridView1.CurrentRow.Index
                    && dataGridView1.Rows[i].Cells["物品ID"].Value.ToString() == txtName.Tag.ToString()
                    && dataGridView1.Rows[i].Cells["批次号"].Value.ToString() == txtBatchNo.Text)
                {
                    MessageDialog.ShowPromptMessage("不能添加同一物品，同一批次号");
                    return;
                }
            }

            dataGridView1.Rows[intIndex].Cells["图号型号"].Value = txtCode.Text;
            dataGridView1.Rows[intIndex].Cells["物品名称"].Value = txtName.Text;
            dataGridView1.Rows[intIndex].Cells["规格"].Value = txtSpec.Text;
            dataGridView1.Rows[intIndex].Cells["批次号"].Value = txtBatchNo.Text;
            dataGridView1.Rows[intIndex].Cells["领用数量"].Value = numPickingCount.Value;
            dataGridView1.Rows[intIndex].Cells["单位"].Value = txtBatchNo.Tag.ToString();
            dataGridView1.Rows[intIndex].Cells["单价"].Value = Convert.ToBoolean(txtCode.Tag) == false ? 0 : 
                Convert.ToDecimal(numPickingCount.Tag);
            dataGridView1.Rows[intIndex].Cells["金额"].Value = Convert.ToBoolean(txtCode.Tag) == false ? 0 : 
                Math.Round(numPickingCount.Value * Convert.ToDecimal(numPickingCount.Tag), 2);
            dataGridView1.Rows[intIndex].Cells["策略金额"].Value = Convert.ToBoolean(txtCode.Tag) == false ? 0 :
                Convert.ToDecimal( dataGridView1.Rows[intIndex].Cells["金额"].Value) * 
                (numMarketingStrategy.Value / 100 + 1);
            dataGridView1.Rows[intIndex].Cells["备注"].Value = txtRemark.Text;
            dataGridView1.Rows[intIndex].Cells["物品ID"].Value = Convert.ToInt32(txtName.Tag.ToString());
            dataGridView1.Rows[intIndex].Cells["单据号"].Value = m_lnqBill.Bill_ID;
            dataGridView1.Rows[intIndex].Cells["是否为客户责任"].Value = Convert.ToBoolean(txtCode.Tag);

            RefreshInfo((DataTable)dataGridView1.DataSource);

            PositioningRecord(Convert.ToInt32(dataGridView1.Rows[intIndex].Cells["物品ID"].Value), 
                dataGridView1.Rows[intIndex].Cells["批次号"].Value.ToString());

            ClearMessage();

            SumPrice();
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
                if ((string)dataGridView1.Rows[i].Cells["批次号"].Value == batchNo 
                    && (int)dataGridView1.Rows[i].Cells["物品ID"].Value == goodsID)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            RefreshInfo(dt);

            ClearMessage();

            SumPrice();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            m_lnqBill.MarketingStrategy = numMarketingStrategy.Value;
            m_lnqBill.PlantRemark = txtPlantRemark.Text;
            m_lnqBill.RepairTaskTime = numRepairTaskTime.Value;

            if (!m_serverThreePacketsOfTheRepair.UpdateList(m_lnqBill.Bill_ID,(DataTable)dataGridView1.DataSource,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");
            }

        }

        private void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"]);
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            txtSpec.Tag = "0";
            txtBatchNo.Tag = txtName.DataResult["单位"].ToString();
            txtBatchNo.Text = "";
            numPickingCount.Value = 0;
            numPickingCount.Tag = 0;
            
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 库房代码 in( '01','08','11') and 物品ID = " + Convert.ToInt32(txtName.Tag)
                + " and 库存数量 > 0 and 物品状态 in ( '正常' ,'仅限于返修箱用')";
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtSpec.Tag = txtBatchNo.DataResult["库存数量"];
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {

                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtName.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtCode.Tag = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否为客户责任"].Value);
                txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
                txtBatchNo.Tag = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                numPickingCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["领用数量"].Value);
                numPickingCount.Tag = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["单价"].Value);
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();

                View_S_Stock tempLnq = m_serverStore.GetGoodsStockInfo(Convert.ToInt32( txtName.Tag), txtBatchNo.Text);

                txtSpec.Tag = tempLnq == null ? 0 : tempLnq.库存数量;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0 && BasicInfo.ListRoles.Contains(CE_RoleEnum.质量工程师.ToString()))
            {
                if (Convert.ToBoolean(dataGridView1.CurrentRow.Cells[e.ColumnIndex].Value))
                {
                    dataGridView1.CurrentRow.Cells[e.ColumnIndex].Value = false;
                    dataGridView1.CurrentRow.Cells["单价"].Value = 0;
                    dataGridView1.CurrentRow.Cells["金额"].Value = 0;
                    dataGridView1.CurrentRow.Cells["策略金额"].Value = 0;
                }
                else
                {
                    dataGridView1.CurrentRow.Cells[e.ColumnIndex].Value = true;

                    decimal decGoodsUnitPrice = 
                        m_serverThreePacketsOfTheRepair.GetThreePacketGoodsUnitPrice(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value));

                    dataGridView1.CurrentRow.Cells["单价"].Value = decGoodsUnitPrice;
                    dataGridView1.CurrentRow.Cells["金额"].Value = 
                        Math.Round(Convert.ToDecimal(dataGridView1.CurrentRow.Cells["领用数量"].Value) * decGoodsUnitPrice, 2);
                    dataGridView1.CurrentRow.Cells["策略金额"].Value = 
                        Convert.ToDecimal(dataGridView1.CurrentRow.Cells["金额"].Value) * (numMarketingStrategy.Value / 100 + 1);
                }
            }
        }

        private void btnFindGoods_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog();
            form.ShowColumns = new string[] { "图号型号", "物品名称", "规格", "物品类别名称", "序号", "单位" };

            if (form != null && DialogResult.OK == form.ShowDialog())
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
                txtName.Tag = Convert.ToInt32(form.GetDataItem("序号").ToString());
                txtBatchNo.Tag = form.GetDataItem("单位").ToString();
                txtBatchNo.Text = "";

                numPickingCount.Tag = 0;
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

        /// <summary>
        /// 刷新数据
        /// </summary>
        /// <param name="source">数据源</param>
        void RefreshInfo(DataTable source)
        {
            dataGridView1.DataSource = source;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Columns["单据号"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["单位"].Width = 40;

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.营销普通人员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.质量工程师.ToString()))
            {
                if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString())
                    || BasicInfo.ListRoles.Contains(CE_RoleEnum.营销普通人员.ToString()))
                {
                    splitContainer1.Visible = true;
                }
            }
            else
            {
                dataGridView1.Columns["是否为客户责任"].Visible = false;
            }


            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString())
                   && !BasicInfo.ListRoles.Contains(CE_RoleEnum.营销普通人员.ToString()))
            {
                dataGridView1.Columns["单价"].Visible = false;
                dataGridView1.Columns["金额"].Visible = false;
                dataGridView1.Columns["策略金额"].Visible = false;
            }
 
        }

        private void btnShortcutSelect_Click(object sender, EventArgs e)
        {
            DataTable tempTable = m_serverThreePacketsOfTheRepair.GetShortcutDetail(m_lnqBill.ProductType);
            FormDataTableCheck fdc = new FormDataTableCheck(tempTable);

            if (fdc.ShowDialog() == DialogResult.OK)
            {
                List<View_YX_ThreePacketsOfTheRepairList> tempList = 
                    m_serverThreePacketsOfTheRepair.GetShortcutDetailList(fdc._DtResult, out m_strErr);

                if (m_strErr != null && m_strErr.Trim().Length > 0)
                {
                    MessageDialog.ShowPromptMessage(m_strErr + "库存不足");
                }

                if (tempList != null)
                {
                    DataTable dt = (DataTable)dataGridView1.DataSource;

                    foreach (View_YX_ThreePacketsOfTheRepairList item in tempList)
                    {
                        bool flag = false;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["物品ID"].ToString() == item.物品ID.ToString()
                                && dt.Rows[i]["批次号"].ToString() == item.批次号)
                            {
                                dt.Rows[i]["领用数量"] = Convert.ToDecimal(dt.Rows[i]["领用数量"]) + Convert.ToDecimal(item.领用数量);
                                flag = true;
                                break;
                            }
                        }

                        if (!flag)
                        {
                            DataRow dr = dt.NewRow();

                            dr["图号型号"] = item.图号型号;
                            dr["物品名称"] = item.物品名称;
                            dr["规格"] = item.规格;
                            dr["批次号"] = item.批次号;
                            dr["领用数量"] = item.领用数量;
                            dr["单位"] = item.单位;
                            dr["单价"] = item.单价;
                            dr["金额"] = item.金额;
                            dr["策略金额"] = item.策略金额;
                            dr["备注"] = item.备注;
                            dr["物品ID"] = item.物品ID;
                            dr["单据号"] = m_lnqBill.Bill_ID;
                            dr["是否为客户责任"] = item.是否为客户责任;

                            dt.Rows.Add(dr);
                        }
                    }

                    RefreshInfo(dt);
                }
            }
        }
    }
}
