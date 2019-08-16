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
    public partial class 调运单明细 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        Out_ManeuverBill m_lnqManeuverBill = new Out_ManeuverBill();

        /// <summary>
        /// 调运单服务组件
        /// </summary>
        IManeuverServer m_serverManeuver = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>();

        /// <summary>
        /// 业务服务组件
        /// </summary>
        IBusinessOperation m_serverOperation = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBusinessOperation>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        public 调运单明细(string billID, AuthorityFlag authFlag)
        {
            InitializeComponent();


            FaceAuthoritySetting.SetEnable(this.Controls, authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip, authFlag);
            this.toolStrip.Visible = true;

            if (billID == "")
            {
                txtBill_ID.Text = "系统自动生成";
                lbBillStatus.Text = "新建单据";
            }
            else
            {
                m_lnqManeuverBill = m_serverManeuver.GetBillInfo(billID);

                txtBill_ID.Text = m_lnqManeuverBill.Bill_ID;
                lbBillStatus.Text = m_lnqManeuverBill.BillStatus;
                txtReceiving.Tag = m_lnqManeuverBill.InStorageID;
                txtShipments.Tag = m_lnqManeuverBill.OutStorageID;
                txtReceiving.Text = UniversalFunction.GetStorageName(m_lnqManeuverBill.InStorageID);
                txtShipments.Text = UniversalFunction.GetStorageName(m_lnqManeuverBill.OutStorageID);
                txtBillRemark.Text = m_lnqManeuverBill.Remark;
                txtLogisticsBillNo.Text = m_lnqManeuverBill.LogisticsBillNo;
                txtLogisticsName.Text = m_lnqManeuverBill.LogisticsName;
                txtPhone.Text = m_lnqManeuverBill.Phone;
                txtScrapBillNo.Text = m_lnqManeuverBill.ScrapBillNo;

                btnReceiving.Visible =
                    IntegrativeQuery.IsStockPrincipal(txtReceiving.Tag.ToString(), BasicInfo.LoginName);
                btnShipments.Visible =
                    IntegrativeQuery.IsStockPrincipal(txtShipments.Tag.ToString(), BasicInfo.LoginName);

                if (IntegrativeQuery.IsSalesStorage(m_lnqManeuverBill.InStorageID))
                {
                    btnReceiving.Visible = false;
                }
            }

            switch (lbBillStatus.Text)
            {

                case "新建单据":

                    numProposerCount.ReadOnly = false;
                    btnAdd.Visible = true;
                    btnModify.Visible = true;
                    btnDelete.Visible = true;
                    break;

                case "等待主管审核":

                    numProposerCount.ReadOnly = false;

                    break;
                case "等待出库":

                    numShipperCount.ReadOnly = false;
                    btnModify.Visible = true;
                    txtGoodsCode.Enabled = false;

                    break;
                case "等待入库":

                    numConfirmorCount.ReadOnly = false;
                    btnModify.Visible = true;
                    txtGoodsCode.Enabled = false;

                    break;
                default:
                    break;
            }

            dataGridView1.DataSource = m_serverManeuver.GetListInfo(billID);
            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["Bill_ID"].Visible = false;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqManeuverBill.Bill_ID = txtBill_ID.Text;
            m_lnqManeuverBill.InStorageID = txtReceiving.Tag.ToString();
            m_lnqManeuverBill.OutStorageID = txtShipments.Tag.ToString();
            m_lnqManeuverBill.Remark = txtBillRemark.Text;
            m_lnqManeuverBill.LogisticsBillNo = txtLogisticsBillNo.Text;
            m_lnqManeuverBill.LogisticsName = txtLogisticsName.Text;
            m_lnqManeuverBill.Phone = txtPhone.Text;
            m_lnqManeuverBill.BillStatus = lbBillStatus.Text;
            m_lnqManeuverBill.ScrapBillNo = txtScrapBillNo.Text;
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            if (txtShipments.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择发货方");
                return;
            }

            txtGoodsCode.Tag = Convert.ToInt32(txtGoodsCode.DataResult["序号"]);
            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
            lbUnit.Text = txtGoodsCode.DataResult["单位"].ToString();
            txtGoodsName.Tag = txtGoodsCode.DataResult["账务库房ID"].ToString();
            lbStock.Text = txtGoodsCode.DataResult["库存数量"].ToString();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(lbStock.Text) < numProposerCount.Value)
            {
                MessageDialog.ShowPromptMessage("申请数量超出库存数量");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (Convert.ToInt32(txtGoodsCode.Tag) == Convert.ToInt32(dtTemp.Rows[i]["物品ID"])
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
            dr["申请数量"] = numProposerCount.Value;
            dr["发货数量"] = numShipperCount.Value;
            dr["收货数量"] = numConfirmorCount.Value;
            dr["单位"] = lbUnit.Text;
            dr["备注"] = txtListRemark.Text;
            dr["物品ID"] = txtGoodsCode.Tag;
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

                if (Convert.ToDecimal(lbStock.Text) < numShipperCount.Value)
                {
                    MessageDialog.ShowPromptMessage("发货数量超出库存数量");
                    return;
                }

                if (numConfirmorCount.Value > numShipperCount.Value)
                {
                    MessageDialog.ShowPromptMessage("收货数不能大于发货数");
                    return;
                }

                if (numShipperCount.Value > numProposerCount.Value)
                {
                    MessageDialog.ShowPromptMessage("发货数不能大于申请数");
                    return;
                }

                dataGridView1.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;
                dataGridView1.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
                dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
                dataGridView1.CurrentRow.Cells["申请数量"].Value = numProposerCount.Value;
                dataGridView1.CurrentRow.Cells["发货数量"].Value = numShipperCount.Value;
                dataGridView1.CurrentRow.Cells["收货数量"].Value = numConfirmorCount.Value;
                dataGridView1.CurrentRow.Cells["单位"].Value = lbUnit.Text;
                dataGridView1.CurrentRow.Cells["备注"].Value = txtListRemark.Text;
                dataGridView1.CurrentRow.Cells["物品ID"].Value = txtGoodsCode.Tag;
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

        /// <summary>
        /// 提交数据
        /// </summary>
        /// <returns>成功返回True，失败返回False</returns>
        bool SubmitData()
        {
            GetMessage();

            if (!m_serverManeuver.OperationInfo(m_lnqManeuverBill, (DataTable)dataGridView1.DataSource, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return false;
            }

            return true;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "新建单据")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (txtShipments.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择发货方");
                return;
            }
            else if (txtReceiving.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择收货方");
                return;
            }

            if (txtBillRemark.Text == "")
            {
                MessageDialog.ShowPromptMessage("请填写【调运原因】");
                return;
            }

            GetMessage();

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            if (dtTemp.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return;
            }

            m_lnqManeuverBill.Proposer = BasicInfo.LoginName;
            m_lnqManeuverBill.ProposerTime = ServerTime.Time;

            if (!m_serverManeuver.InsertBill(m_lnqManeuverBill, dtTemp, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
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

                txtGoodsCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtGoodsName.Tag = dataGridView1.CurrentRow.Cells["账务库房ID"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numProposerCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["申请数量"].Value);
                numShipperCount.Value = dataGridView1.CurrentRow.Cells["发货数量"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["发货数量"].Value);
                numConfirmorCount.Value = dataGridView1.CurrentRow.Cells["收货数量"].Value.ToString() == "" ? 0 : Convert.ToDecimal(dataGridView1.CurrentRow.Cells["收货数量"].Value);
                lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();

                Out_Stock lnqStock = IntegrativeQuery.QuerySecStock(Convert.ToInt32(txtGoodsCode.Tag), txtShipments.Tag.ToString(), txtGoodsName.Tag.ToString());

                lbStock.Text = Convert.ToDecimal(lnqStock == null ? 0 : lnqStock.StockQty).ToString();
            }
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待主管审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (SubmitData())
            {
                MessageDialog.ShowPromptMessage("审核成功");

                m_billMessageServer.PassFlowMessage(m_lnqManeuverBill.Bill_ID,
                    string.Format("{0}号调运单已审核，请发货方出库", m_lnqManeuverBill.Bill_ID), 
                    BillFlowMessage_ReceivedUserType.用户, 
                    IntegrativeQuery.GetStorageOrStationPrincipal(m_lnqManeuverBill.OutStorageID));

                this.Close();
            }
        }

        private void btnShipments_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待出库")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (SubmitData())
            {
                m_billMessageServer.PassFlowMessage(m_lnqManeuverBill.Bill_ID, string.Format("{0}号调运单已出库，请发货人发货", m_lnqManeuverBill.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户, CE_RoleEnum.营销发货员.ToString());
                MessageDialog.ShowPromptMessage("出库成功");
                this.Close();
            }
        }

        private void btnReceiving_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待入库")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (SubmitData())
            {
                MessageDialog.ShowPromptMessage("入库成功");


                m_billMessageServer.EndFlowMessage(m_lnqManeuverBill.Bill_ID,
                    string.Format("{0}号调运单收货方已入库", m_lnqManeuverBill.Bill_ID),null,
                    IntegrativeQuery.GetStorageOrStationPrincipal(m_lnqManeuverBill.OutStorageID));

                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            bool blOperation = false;

            if (lbBillStatus.Text == "等待出库")
            {
                blOperation = true;
            }

            Out_ManeuverList lnqList = new Out_ManeuverList();

            lnqList.Bill_ID = m_lnqManeuverBill.Bill_ID;
            lnqList.GoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
            lnqList.ShipperCount = dataGridView1.CurrentRow.Cells["发货数量"].Value.ToString() == "" ? Convert.ToDecimal(dataGridView1.CurrentRow.Cells["申请数量"].Value) :
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["发货数量"].Value);
            lnqList.StorageID = dataGridView1.CurrentRow.Cells["账务库房ID"].Value.ToString();

            唯一标识码录入窗体 form = new 唯一标识码录入窗体(lnqList,blOperation);
            form.ShowDialog();
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {

            txtGoodsCode.StrEndSql = " and SecStorageID = '" + UniversalFunction.GetStorageID(txtShipments.Text) + "'";
        }

        private void txtShipments_OnCompleteSearch()
        {
            txtShipments.Tag = txtShipments.DataResult["编码"].ToString();
        }

        private void txtReceiving_OnCompleteSearch()
        {
            txtReceiving.Tag = txtReceiving.DataResult["编码"].ToString();
        }

        private void btnExcShipper_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待发货")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (txtLogisticsBillNo.Text.Trim() == "" && txtLogisticsName.Text.Trim() == "" && txtPhone.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请完善物流信息");
                return;
            }

            if (SubmitData())
            {

                m_billMessageServer.PassFlowMessage(m_lnqManeuverBill.Bill_ID,
                    string.Format("{0}号调运单已发货，请收货人确认收货", m_lnqManeuverBill.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户,CE_RoleEnum.营销收货员.ToString());
                MessageDialog.ShowPromptMessage("发货成功");
                this.Close();
            }
        }

        private void btnExcConfirmor_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != "等待收货")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (SubmitData())
            {

                m_billMessageServer.PassFlowMessage(m_lnqManeuverBill.Bill_ID,
                    string.Format("{0}号调运单已收货，请收货方入库", m_lnqManeuverBill.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户,
                    IntegrativeQuery.GetStorageOrStationPrincipal(m_lnqManeuverBill.InStorageID));
                MessageDialog.ShowPromptMessage("收货成功");
                this.Close();
            }
        }

        private void btnFindBillNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo dialog = GetScrapBillDialogForFetchGoods();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtScrapBillNo.Text = dialog.GetStringDataItem("报废单号");

                IScrapGoodsServer scarpGoodsServer = ServerModule.ServerModuleFactory.GetServerModule<IScrapGoodsServer>();
                IEnumerable<ServerModule.GoodsGroup> goodsGroup = scarpGoodsServer.GetGoodsByGroup(txtScrapBillNo.Text);

                DataTable tempTable = ((DataTable)dataGridView1.DataSource).Clone();

                foreach (GoodsGroup item in goodsGroup)
                {
                    DataRow tempRow = tempTable.NewRow();

                    tempRow["图号型号"] = item.图号型号;
                    tempRow["物品名称"] = item.物品名称;
                    tempRow["规格"] = item.规格;
                    tempRow["账务库房"] = txtReceiving.Text;
                    tempRow["申请数量"] = item.数量;
                    tempRow["发货数量"] = 0;
                    tempRow["收货数量"] = 0;
                    tempRow["单位"] = UniversalFunction.GetGoodsInfo(item.物品ID).单位;
                    tempRow["备注"] = "";
                    tempRow["物品ID"] = item.物品ID;
                    tempRow["账务库房ID"] = txtReceiving.Tag;

                    tempTable.Rows.Add(tempRow);
                }

                dataGridView1.DataSource = tempTable;
            }
        }

        public FormQueryInfo GetScrapBillDialogForFetchGoods()
        {
            IScrapBillServer scrapBillServer = ServerModule.ServerModuleFactory.GetServerModule<IScrapBillServer>();
            PlatformManagement.IQueryResult queryInfo;

            if (!scrapBillServer.GetAllBillForFetchGoods(out queryInfo, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return null;
            }

            FormQueryInfo form = new FormQueryInfo(queryInfo);
            form.ShowColumns = new string[] { "报废单号", "报废时间", "报废类别", "报废原因", "是否冲抵领料单", "申请人签名", "仓管签名" };
            return form;
        }

        private void 替换件ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strSql = " select c.序号 as 物品ID, c.图号型号, c.物品名称, c.规格 " +
                            " from F_GoodsAttributeRecord as a inner join F_GoodsReplaceInfo as b " +
                            " on a.AttributeRecordID = b.AttributeRecordID " +
                            " inner join View_F_GoodsPlanCost as c on b.ReplaceGoodsID = c.序号 " +
                            " where a.GoodsID = " + Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);

            FormGoodsSelect frm = new FormGoodsSelect(strSql);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                dataGridView1.CurrentRow.Cells["物品ID"].Value = frm.GoodsInfo.序号;
                dataGridView1.CurrentRow.Cells["图号型号"].Value = frm.GoodsInfo.图号型号;
                dataGridView1.CurrentRow.Cells["物品名称"].Value = frm.GoodsInfo.物品名称;
                dataGridView1.CurrentRow.Cells["规格"].Value = frm.GoodsInfo.规格;
            }
        }

    }
}
