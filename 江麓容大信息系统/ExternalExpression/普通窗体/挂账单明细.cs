using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Expression;
using Service_Peripheral_External;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 挂账单明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 数据集
        /// </summary>
        Out_BuyingBill m_lnqBuyingBill = new Out_BuyingBill();

        /// <summary>
        /// 挂账单服务类
        /// </summary>
        IBuyingBillServer m_buyingServer = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBuyingBillServer>();

        public 挂账单明细(string billID, AuthorityFlag authFlag)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetVisibly(this.toolStrip, authFlag);
            this.toolStrip.Visible = true;

            if (billID == "")
            {
                txtStatus.Text = "新建单据";
                txtRecorder.Text = BasicInfo.LoginName;
                txtRecordTime.Text = ServerTime.Time.ToString();
            }
            else
            {
                View_Out_BuyingBill buyingBill = m_buyingServer.GetBillInfo(Convert.ToInt32(billID));

                txtStatus.Text = buyingBill.单据状态;
                txtRecorder.Text = buyingBill.记录人员;
                txtRecordTime.Text = buyingBill.记录时间.ToString();
                txtRemark.Text = buyingBill.备注;
                txtReceiving.Text = buyingBill.主机厂名称;
                txtReceiving.Tag = buyingBill.主机厂编号;

                dataGridView1.DataSource = m_buyingServer.GetListInfo(Convert.ToInt32(billID));

                dataGridView1.Columns["单号"].Visible = false;
                dataGridView1.Columns["GoodsID"].Visible = false;
            }
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            txtGoodsCode.Tag = Convert.ToInt32(txtGoodsCode.DataResult["序号"]);
            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
            lbUnit.Text = txtGoodsCode.DataResult["单位"].ToString();
            txtGoodsName.Tag = txtGoodsCode.DataResult["账务库房ID"].ToString();
            lbStock.Text = txtGoodsCode.DataResult["库存数量"].ToString();
        }

        private void txtReceiving_OnCompleteSearch()
        {
            txtReceiving.Tag = txtReceiving.DataResult["编码"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lbStock.Text) == 0)
            {
                MessageDialog.ShowPromptMessage("没有库存数，请确认零件！");
                return;
            }

            if (Convert.ToDecimal(lbStock.Text) < numCount.Value)
            {
                MessageDialog.ShowPromptMessage("挂账数量超出库存数量");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (Convert.ToInt32(txtGoodsCode.Tag) == Convert.ToInt32(dtTemp.Rows[i]["GoodsID"])
                    && txtGoodsName.Tag.ToString() == dtTemp.Rows[i]["账务库房ID"].ToString())
                {
                    MessageDialog.ShowPromptMessage("不能录入重复物品");
                    return;
                }
            }

            if (IntegrativeQuery.IsInnerStorage(txtReceiving.Tag.ToString())
                && txtGoodsName.Tag.ToString() != txtReceiving.Tag.ToString())
            {
                MessageDialog.ShowPromptMessage("物品明细中有物品的账务库房与入库库房不符");
                return;
            }

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtGoodsCode.Text;
            dr["物品名称"] = txtGoodsName.Text;
            dr["规格"] = txtSpec.Text;
            dr["账务库房"] = UniversalFunction.GetStorageName(txtGoodsName.Tag.ToString());
            dr["挂账数量"] = numCount.Value;
            dr["备注"] = txtListRemark.Text;
            dr["GoodsID"] = txtGoodsCode.Tag;
            dr["账务库房ID"] = txtGoodsName.Tag;

            dtTemp.Rows.Add(dr);

            dataGridView1.DataSource = dtTemp;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {

                if (Convert.ToDecimal(lbStock.Text) < numCount.Value)
                {
                    MessageDialog.ShowPromptMessage("挂账数量超出库存数量");
                    return;
                }

                dataGridView1.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;
                dataGridView1.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
                dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
                dataGridView1.CurrentRow.Cells["挂账数量"].Value = numCount.Value;
                dataGridView1.CurrentRow.Cells["单位"].Value = lbUnit.Text;
                dataGridView1.CurrentRow.Cells["备注"].Value = txtListRemark.Text;
                dataGridView1.CurrentRow.Cells["GoodsID"].Value = txtGoodsCode.Tag;
                dataGridView1.CurrentRow.Cells["账务库房ID"].Value = txtGoodsName.Tag;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtGoodsCode.Tag = dataGridView1.CurrentRow.Cells["GoodsID"].Value.ToString();
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtGoodsName.Tag = dataGridView1.CurrentRow.Cells["账务库房ID"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["挂账数量"].Value);
                lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();

                Out_Stock lnqStock = IntegrativeQuery.QuerySecStock(Convert.ToInt32(txtGoodsCode.Tag), txtReceiving.Tag.ToString(), txtGoodsName.Tag.ToString());

                lbStock.Text = Convert.ToDecimal(lnqStock == null ? 0 : lnqStock.StockQty).ToString();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text != "新建单据")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (txtReceiving.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择主机厂");
                return;
            }

            m_lnqBuyingBill.ClientCode = txtReceiving.Tag.ToString();
            m_lnqBuyingBill.Recorder = BasicInfo.LoginID;
            m_lnqBuyingBill.Remark = txtRemark.Text.Trim();
            m_lnqBuyingBill.RecordTime = ServerTime.Time;
            m_lnqBuyingBill.Statua = "等待审核";

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            if (dtTemp.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return;
            }

            if (!m_buyingServer.InsertBill(m_lnqBuyingBill, dtTemp, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text != "等待审核")
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            m_lnqBuyingBill.ID = Convert.ToInt32(txtGoodsCode.Tag);
            m_lnqBuyingBill.ClientCode = txtReceiving.Tag.ToString();
            m_lnqBuyingBill.Recorder = txtRecorder.Text;
            m_lnqBuyingBill.Director = BasicInfo.LoginID;
            m_lnqBuyingBill.Remark = txtRemark.Text.Trim();
            m_lnqBuyingBill.DirectTime = ServerTime.Time;
            m_lnqBuyingBill.Statua = "已完成";

            if (m_buyingServer.OperationInfo(m_lnqBuyingBill, dtTemp, out m_error))
            {
                MessageDialog.ShowPromptMessage("审核成功！");
            }
            else
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            string sql = "";
            sql += " and SecStorageID ='" + txtReceiving.Tag.ToString() + "'";
            txtGoodsCode.StrEndSql = sql;
        }
    }
}
