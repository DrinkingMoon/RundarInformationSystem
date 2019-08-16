using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Service_Manufacture_WorkShop;
using Expression;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间调运单明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillNo = "";

        public string StrBillNo
        {
            get { return m_strBillNo; }
            set { m_strBillNo = value; }
        }

        /// <summary>
        /// 操作库房数据集
        /// </summary>
        List<WS_CannibalizeWSCode> m_listLnqWSCode = new List<WS_CannibalizeWSCode>();

        /// <summary>
        /// 单据LINQ数据集
        /// </summary>
        WS_CannibalizeBill m_lnqBill = new WS_CannibalizeBill();

        /// <summary>
        /// 明细信息
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 车间库存管理服务组件
        /// </summary>
        IWorkShopStock m_serverStock = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopStock>();

        /// <summary>
        /// 车间耗用服务组件
        /// </summary>
        Service_Manufacture_WorkShop.ICannibalize m_serverCannibalize = 
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.ICannibalize>();

        /// <summary>
        /// 车间管理基础信息服务组件
        /// </summary>
        IWorkShopBasic m_serverWSBasic = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopBasic>();

        /// <summary>
        /// 人员管理组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        public 车间调运单明细(string billNo)
        {
            InitializeComponent();

            m_strBillNo = billNo;

            m_lnqBill = m_serverCannibalize.GetBillSingle(m_strBillNo);
            m_listLnqWSCode = m_serverCannibalize.GetOperationWSCode(m_strBillNo);
            m_dtList = m_serverCannibalize.GetListInfo(m_strBillNo);

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.车间调运单.ToString(), m_serverCannibalize);
            m_billMessageServer.BillType = CE_BillTypeEnum.车间调运单.ToString();

            cmbOutWSCode.DataSource = m_serverWSBasic.GetWorkShopBasicInfo();

            cmbOutWSCode.DisplayMember = "车间名称";
            cmbOutWSCode.ValueMember = "车间编码";

            cmbInWSCode.DataSource = m_serverWSBasic.GetWorkShopBasicInfo();

            cmbInWSCode.DisplayMember = "车间名称";
            cmbInWSCode.ValueMember = "车间编码";
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        void GetInfo()
        {
            m_lnqBill = new WS_CannibalizeBill();

            m_lnqBill.BillNo = txtBillNo.Text;
            m_lnqBill.BillStatus = lbBillStatus.Text;
            m_lnqBill.Remark = txtBillRemark.Text;

            m_listLnqWSCode = new List<WS_CannibalizeWSCode>();

            WS_CannibalizeWSCode lnqWSCode = new WS_CannibalizeWSCode();

            lnqWSCode.BillNo = txtBillNo.Text;
            lnqWSCode.OperationType = (int)CE_SubsidiaryOperationType.车间调出;
            lnqWSCode.WSCode = cmbOutWSCode.SelectedValue.ToString();

            m_listLnqWSCode.Add(lnqWSCode);

            lnqWSCode = new WS_CannibalizeWSCode();

            lnqWSCode.BillNo = txtBillNo.Text;
            lnqWSCode.OperationType = (int)CE_SubsidiaryOperationType.车间调入;
            lnqWSCode.WSCode = cmbInWSCode.SelectedValue.ToString();

            m_listLnqWSCode.Add(lnqWSCode);

            m_dtList = (DataTable)dataGridView1.DataSource;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            dataGridView1.DataSource = m_dtList;

            if (m_lnqBill == null)
            {
                lbPropose.Text = BasicInfo.LoginName;
                lbProposerDate.Text = ServerTime.Time.ToShortDateString();
                return;
            }

            txtBillNo.Text = m_lnqBill.BillNo;
            txtBillRemark.Text = m_lnqBill.Remark;

            if (m_listLnqWSCode != null)
            {
                foreach (WS_CannibalizeWSCode item in m_listLnqWSCode)
                {
                    WS_WorkShopCode tempCode = m_serverWSBasic.GetWorkShopCodeInfo(item.WSCode);

                    if (tempCode != null)
                    {
                        if (item.OperationType == (int)CE_SubsidiaryOperationType.车间调出)
                        {
                            cmbOutWSCode.Text = tempCode.WSName;
                            cmbOutWSCode.SelectedValue = tempCode.WSCode;
                        }
                        else if (item.OperationType == (int)CE_SubsidiaryOperationType.车间调入)
                        {
                            cmbInWSCode.Text = tempCode.WSName;
                            cmbInWSCode.SelectedValue = tempCode.WSCode;
                        }
                    }
                }
            }

            lbBillStatus.Text = m_lnqBill.BillStatus;
            lbAffirm.Text = m_lnqBill.Affirm == null ? "" : m_lnqBill.Affirm;
            lbAffirmDate.Text = m_lnqBill.AffirmDate == null ? "" : m_lnqBill.AffirmDate.ToString();
            lbAudit.Text = m_lnqBill.Audit == null ? "" : m_lnqBill.Audit;
            lbAuditDate.Text = m_lnqBill.AuditDate == null ? "" : m_lnqBill.AuditDate.ToString();
            lbPropose.Text = m_lnqBill.Proposer == null ? "" : m_lnqBill.Proposer;
            lbProposerDate.Text = m_lnqBill.ProposerDate == null ? "" : m_lnqBill.ProposerDate.ToString();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            lbAffirm.Text = "";
            lbAffirmDate.Text = "";
            lbAudit.Text = "";
            lbAuditDate.Text = "";
            lbBillStatus.Text = "";
            lbPropose.Text = "";
            lbProposerDate.Text = "";
            lbHYDW.Text = "单位";
            lbKCDW.Text = "单位";
            lbStockCount.Text = "";

            txtBatchNo.Text = "";
            txtBillNo.Text = "";
            txtBillRemark.Text = "";
            txtCode.Text = "";
            txtListRemark.Text = "";
            txtName.Text = "";

            txtSpec.Text = "";

        }

        /// <summary>
        /// 明细信息清空
        /// </summary>
        void ListClear()
        {
            txtCode.Text = "";
            txtCode.Tag = null;
            txtName.Text = "";
            txtSpec.Text = "";

            txtBatchNo.Text = "";
            numOperationCount.Value = 0;
            lbStockCount.Text = "";
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckData()
        {
            m_dtList = (DataTable)dataGridView1.DataSource;

            if (cmbInWSCode.SelectedValue == null || cmbInWSCode.Text == ""
                || cmbOutWSCode.SelectedValue == null || cmbOutWSCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择调出车间/调入车间");
                return false;
            }
            else if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请录入调运物品");
                return false;
            }
            else if (txtBillNo.Text == null || txtBillNo.Text == "")
            {
                MessageDialog.ShowPromptMessage("请确认单据号");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 流程控制
        /// </summary>
        void FlowControl()
        {
            bool visible = UniversalFunction.IsOperator(txtBillNo.Text);

            switch (GlobalObject.GeneralFunction.StringConvertToEnum<CannibalizeBillStatus>(lbBillStatus.Text))
            {
                case CannibalizeBillStatus.新建单据:

                    DataTable tempTable = m_serverWSBasic.GetWorkShopBasicInfo();

                    string tempRole = "";

                    foreach (string str in m_serverWSBasic.GetWorkShopCodeRole())
                    {
                        tempRole += "'" + str + "',";
                    }

                    if (tempRole.Length > 0)
                    {
                        tempRole = tempRole.Substring(0, tempRole.Length - 1);
                        tempTable = GlobalObject.DataSetHelper.SiftDataTable(tempTable, "车间编码 in (" + tempRole + ")", out m_strError);
                    }
                    else
                    {
                        tempTable = tempTable.Clone();
                    }

                    cmbOutWSCode.DataSource = tempTable;
                    btnPropose.Visible = true;
                    break;
                case CannibalizeBillStatus.等待审核:

                    if (BasicInfo.LoginName == lbPropose.Text)
                    {
                        btnPropose.Visible = true;
                    }

                    btnAudit.Visible = visible;
                    groupBox1.Enabled = false;
                    break;
                case CannibalizeBillStatus.等待确认:

                    if (BasicInfo.LoginName == lbPropose.Text)
                    {
                        btnPropose.Visible = true;
                    }

                    btnAffirm.Visible = visible;
                    groupBox1.Enabled = false;
                    break;
                case CannibalizeBillStatus.单据已完成:
                    break;
                default:
                    break;
            }
        }

        private void 车间调运单明细_Load(object sender, EventArgs e)
        {
            ClearData();
            ShowInfo();

            if (m_strBillNo == null)
            {
                m_strBillNo = m_billNoControl.GetNewBillNo();
                txtBillNo.Text = m_strBillNo;
                lbBillStatus.Text = CannibalizeBillStatus.新建单据.ToString();

                if (BasicInfo.DeptCode.Length >= 4)
                {
                    WS_WorkShopCode tempLnq = m_serverWSBasic.GetWorkShopCodeInfo(BasicInfo.DeptCode.Substring(0, 4));

                    if (tempLnq != null)
                    {
                        cmbOutWSCode.Text = tempLnq.WSName;
                        cmbOutWSCode.SelectedValue = tempLnq.WSCode;
                    }
                }

                cmbInWSCode.SelectedIndex = -1;
            }

            dataGridView1.Columns["单据号"].Visible = false;

            FlowControl();
        }

        private void 车间调运单明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void btnPropose_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            GetInfo();

            if (!m_serverCannibalize.ProposeBill(m_lnqBill, m_listLnqWSCode, m_dtList, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                m_billMessageServer.DestroyMessage(m_lnqBill.BillNo);
                m_billMessageServer.SendNewFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号车间调运单已提交，请等待上级审核", m_lnqBill.BillNo),
                    BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            GetInfo();

            if (!m_serverCannibalize.AuditBill(m_lnqBill.BillNo, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                List<string> tempList =
                    GlobalObject.GeneralFunction.ConvertListTypeToStringList<CE_RoleEnum>(
                    UniversalFunction.GetStoreroomKeeperRoleEnumList(cmbInWSCode.SelectedValue.ToString()));

                m_billMessageServer.PassFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号车间调运单已审核，请相关物料管理员确认", m_lnqBill.BillNo),
                    BillFlowMessage_ReceivedUserType.角色, tempList);

                MessageDialog.ShowPromptMessage("审核成功");
                this.Close();
            }
        }

        private void btnAffirm_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            GetInfo();

            if (!m_serverCannibalize.AffirmBill(m_lnqBill.BillNo, m_dtList, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                List<string> listPersonnel = new List<string>();

                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqBill.Proposer));
                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqBill.Audit));

                m_billMessageServer.EndFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号车间调运单已完成", m_lnqBill.BillNo), null, listPersonnel);
                m_billNoControl.UseBill(m_lnqBill.BillNo);

                MessageDialog.ShowPromptMessage("确认成功");
                this.Close();
            }
        }

        private void txtCode_OnCompleteSearch()
        {
            ListClear();

            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();

            lbKCDW.Text = txtCode.DataResult["单位"].ToString();
            lbHYDW.Text = txtCode.DataResult["单位"].ToString();

            txtCode.Tag = txtCode.DataResult["物品ID"];
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            txtCode.StrEndSql = " and 车间代码 = '"+ cmbOutWSCode.SelectedValue.ToString() +"'";
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();

            WS_WorkShopStock tempStock = m_serverStock.GetStockSingleInfo(cmbOutWSCode.SelectedValue.ToString(),
                Convert.ToInt32(txtCode.Tag), 
                txtBatchNo.DataResult["批次号"].ToString());

            lbStockCount.Text = tempStock == null ? "0" : tempStock.StockCount.ToString();

        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtCode.Tag) + " and 车间代码 = '"
                + cmbOutWSCode.SelectedValue.ToString() +"'";
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(DataGridView datagridview, int goodsID, string batchNo)
        {
            for (int i = 0; i < datagridview.Rows.Count; i++)
            {
                if ((int)datagridview.Rows[i].Cells["物品ID"].Value == goodsID
                    && (string)datagridview.Rows[i].Cells["批次号"].Value == batchNo)
                {
                    datagridview.FirstDisplayedScrollingRowIndex = i;
                    datagridview.CurrentCell = datagridview.Rows[i].Cells["批次号"];
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtCode.Text;
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["物品ID"] = txtCode.Tag;
            dr["批次号"] = txtBatchNo.Text;
            dr["数量"] = numOperationCount.Value;
            dr["备注"] = txtListRemark.Text;
            dr["单位"] = lbHYDW.Text;

            int goodsID = (int)txtCode.Tag;
            string tempbatchNo = txtBatchNo.Text;

            dtTemp.Rows.Add(dr);
            dtTemp.AcceptChanges();
            dataGridView1.DataSource = dtTemp;
            ListClear();
            PositioningRecord(dataGridView1, goodsID, tempbatchNo);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (txtCode.Tag == null || txtCode.Tag.ToString().Length == 0 || (int)txtCode.Tag == 0)
            {
                MessageDialog.ShowPromptMessage("请选择物品");
                return;
            }

            dataGridView1.CurrentRow.Cells["图号型号"].Value = txtCode.Text;
            dataGridView1.CurrentRow.Cells["物品名称"].Value = txtName.Text;
            dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
            dataGridView1.CurrentRow.Cells["物品ID"].Value = txtCode.Tag;
            dataGridView1.CurrentRow.Cells["批次号"].Value = txtBatchNo.Text;
            dataGridView1.CurrentRow.Cells["数量"].Value = numOperationCount.Value;
            dataGridView1.CurrentRow.Cells["备注"].Value = txtListRemark.Text;
            dataGridView1.CurrentRow.Cells["单位"].Value = lbHYDW.Text;

            int goodsID = (int)dataGridView1.CurrentRow.Cells["物品ID"].Value;
            string tempbatchNo = (string)dataGridView1.CurrentRow.Cells["批次号"].Value;

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;
            dtTemp.AcceptChanges();
            dataGridView1.DataSource = dtTemp;
            PositioningRecord(dataGridView1, goodsID, tempbatchNo);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;
            dtTemp.AcceptChanges();
            dataGridView1.DataSource = dtTemp;
            ListClear();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtCode.Tag = dataGridView1.CurrentRow.Cells["物品ID"].Value;

            txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
            txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            numOperationCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
            lbHYDW.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
            lbKCDW.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();

            WS_WorkShopStock tempStock = m_serverStock.GetStockSingleInfo(cmbOutWSCode.SelectedValue.ToString(),
                Convert.ToInt32(txtCode.Tag), txtBatchNo.Text);
            lbStockCount.Text = tempStock == null ? "0" : tempStock.StockCount.ToString();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            int intGoodsID = (int)dataGridView1.CurrentRow.Cells["物品ID"].Value;

            BarCodeInfo tempInfo = new BarCodeInfo();

            tempInfo.BatchNo = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
            tempInfo.Count = (decimal)dataGridView1.CurrentRow.Cells["数量"].Value;
            tempInfo.GoodsCode = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            tempInfo.GoodsID = intGoodsID;
            tempInfo.GoodsName = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            tempInfo.Remark = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            tempInfo.Spec = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();

            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            tempDic.Add(cmbOutWSCode.SelectedValue.ToString(), CE_SubsidiaryOperationType.车间调出.ToString());
            tempDic.Add(cmbInWSCode.SelectedValue.ToString(), CE_SubsidiaryOperationType.车间调入.ToString());

            产品编号 form = new 产品编号(tempInfo, CE_BusinessType.车间业务, m_strBillNo,
                lbPropose.Text == BasicInfo.LoginName, tempDic);

            form.ShowDialog();
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            switch (GeneralFunction.StringConvertToEnum<CannibalizeBillStatus>(m_lnqBill.BillStatus))
            {
                case CannibalizeBillStatus.等待审核:

                    if (btnAudit.Visible)
                    {
                        ReturnBillStatus();
                    }
                    break;
                case CannibalizeBillStatus.等待确认:

                    if (btnAffirm.Visible)
                    {
                        ReturnBillStatus();
                    }
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (GeneralFunction.StringConvertToEnum<CannibalizeBillStatus>(m_lnqBill.BillStatus) != CannibalizeBillStatus.单据已完成)
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.车间调运单, m_lnqBill.BillNo, m_lnqBill.BillStatus);

                if (form.ShowDialog() == DialogResult.OK)
                {

                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverCannibalize.ReturnBill(m_lnqBill.BillNo,
                            GeneralFunction.StringConvertToEnum<CannibalizeBillStatus>(m_lnqBill.BillStatus),
                            out m_strError, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strError);
                        }
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }
    }
}
