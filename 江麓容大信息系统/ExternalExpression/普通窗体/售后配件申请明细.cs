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
    public partial class 售后配件申请明细 : Form
    {

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 调运单服务组件
        /// </summary>
        IManeuverServer m_serverManeuver = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IManeuverServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        Out_AfterServicePartsApplyBill m_lnqAfterService = new Out_AfterServicePartsApplyBill();

        /// <summary>
        /// 业务服务组件
        /// </summary>
        IBusinessOperation m_serverOperation = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IBusinessOperation>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 售后配件申请服务组件
        /// </summary>
        IAfterServicePartsApply m_serverAfterService = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IAfterServicePartsApply>();

        public 售后配件申请明细(string billID, AuthorityFlag authFlag)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetEnable(this.Controls, authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip, authFlag);

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.售后配件申请单, m_serverAfterService);

            this.toolStrip.Visible = true;

            if (billID == "")
            {
                txtBill_ID.Text = "系统自动生成";
                lbBillStatus.Text = "新建单据";
            }
            else
            {
                m_lnqAfterService = m_serverAfterService.GetBillInfo(billID);

                txtBill_ID.Text = m_lnqAfterService.Bill_ID;
                lbBillStatus.Text = m_lnqAfterService.BillStatus;
                txtReceiving.Tag = m_lnqAfterService.InStorageID;
                txtShipments.Tag = m_lnqAfterService.OutStorageID;
                txtReceiving.Text = UniversalFunction.GetStorageName(m_lnqAfterService.InStorageID);
                txtShipments.Text = UniversalFunction.GetStorageName(m_lnqAfterService.OutStorageID);
                txtBillRemark.Text = m_lnqAfterService.Remark;
                txt4SLinkMan.Text = m_lnqAfterService._4SLinkMan;
                txt4SPhone.Text = m_lnqAfterService._4SPhone;
                txtAddress.Text = m_lnqAfterService.Address;
                txtApplyState.Text = m_lnqAfterService.ApplyState;
                txtCVTType.Text = m_lnqAfterService.CVTType;
                txtServiceErea.Text = m_lnqAfterService.ServiceErea;
                txtProductCode.Text = m_lnqAfterService.ProductCode;
            }

            dataGridView1.DataSource = m_serverAfterService.GetListInfo(billID);
            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["Bill_ID"].Visible = false;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqAfterService.Bill_ID = txtBill_ID.Text;
            m_lnqAfterService._4SLinkMan = txt4SLinkMan.Text;
            m_lnqAfterService._4SPhone = txt4SPhone.Text;
            m_lnqAfterService.Address = txtAddress.Text;
            m_lnqAfterService.ApplyState = txtApplyState.Text;
            m_lnqAfterService.CVTType = txtCVTType.Text;
            m_lnqAfterService.ProductCode = txtProductCode.Text;
            m_lnqAfterService.ServiceErea = txtServiceErea.Text;
            m_lnqAfterService.InStorageID = txtReceiving.Tag.ToString();
            m_lnqAfterService.OutStorageID = txtShipments.Tag.ToString();
            m_lnqAfterService.Remark = txtBillRemark.Text;
            m_lnqAfterService.BillStatus = lbBillStatus.Text;
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

            GetMessage();

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;


            if (dtTemp.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细，明细不能为空");
                return;
            }

            if (!m_serverAfterService.InsertBill(m_lnqAfterService, dtTemp, out m_strErr))
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
            txtGoodsName.Tag = txtGoodsCode.DataResult["账务库房ID"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
            lbUnit.Text = txtGoodsCode.DataResult["单位"].ToString();
            lbStock.Text = txtGoodsCode.DataResult["库存数量"].ToString();
            numApplyCount.Maximum = Convert.ToDecimal(lbStock.Text == "" ? 0 : Convert.ToDecimal(lbStock.Text));
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
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

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtGoodsCode.Text;
            dr["物品名称"] = txtGoodsName.Text;
            dr["规格"] = txtSpec.Text;
            dr["申请数量"] = numApplyCount.Value;
            dr["审核数量"] = numAuditingCount.Value;
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

                if (numAuditingCount.Value > numApplyCount.Value)
                {
                    MessageDialog.ShowPromptMessage("审核数不能大于申请数");
                    return;
                }

                dataGridView1.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;
                dataGridView1.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
                dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
                dataGridView1.CurrentRow.Cells["申请数量"].Value = numApplyCount.Value;
                dataGridView1.CurrentRow.Cells["审核数量"].Value = numAuditingCount.Value;
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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {

                numApplyCount.Maximum = 10000000000;

                txtGoodsCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtGoodsName.Tag = dataGridView1.CurrentRow.Cells["账务库房ID"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numApplyCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["申请数量"].Value);
                numAuditingCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["审核数量"].Value);
                lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();

                Out_Stock lnqStock = IntegrativeQuery.QuerySecStock(Convert.ToInt32(txtGoodsCode.Tag),
                    txtShipments.Tag.ToString(), txtGoodsName.Tag.ToString());

                if (lnqStock == null)
                {
                    numApplyCount.Maximum = 0;
                }
                else
                {
                    numApplyCount.Maximum = Convert.ToDecimal(lnqStock.StockQty);
                }

                
                lbStock.Text = numApplyCount.Maximum.ToString();
            }
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {

            if (lbBillStatus.Text != "等待主管审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;


            if (dtTemp.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("明细信息记录数为0，不能生成调运单");
                return;
            }

            for (int i = 0; i < dtTemp.Rows.Count; i++)
			{
                if (Convert.ToDecimal( dtTemp.Rows[i]["审核数量"]) == 0)
                {
                    MessageDialog.ShowPromptMessage("审核数量不能为0");
                    return;
                }
			}

            if (!m_serverAfterService.VerifyBill(m_lnqAfterService, dtTemp, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {

                MessageDialog.ShowPromptMessage("审核成功");

                List<string> noticeUser = new List<string>();

                noticeUser.Add(UniversalFunction.GetPersonnelCode(m_lnqAfterService.Proposer));
                m_billNoControl.UseBill(m_lnqAfterService.Bill_ID);
                m_billMessageServer.EndFlowMessage(m_lnqAfterService.Bill_ID, string.Format("{0}号售后配件申请单已完成",
                    m_lnqAfterService.Bill_ID), null, noticeUser);

                this.Close();
            }
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            txtGoodsCode.StrEndSql = " and SecStorageID = '" + txtShipments.Tag.ToString() + "'";
        }

        private void txtReceiving_OnCompleteSearch()
        {
            txtReceiving.Tag = txtReceiving.DataResult["编码"].ToString();
        }

        private void txtShipments_OnCompleteSearch()
        {
            txtShipments.Tag = txtShipments.DataResult["库房编码"].ToString();
        }
    }
}
