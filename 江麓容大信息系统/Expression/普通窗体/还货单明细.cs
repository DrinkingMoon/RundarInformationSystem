using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using Service_Manufacture_WorkShop;
using GlobalObject;

namespace Expression
{
    public partial class 还货单明细 : Form
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
        IBillMessagePromulgatorServer m_billMessageServer =
            BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

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
        /// 单据LINQ数据集
        /// </summary>
        S_ProductReturnBill m_lnqBill = null;

        /// <summary>
        /// 明细信息
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 车间耗用服务组件
        /// </summary>
        IProductReturnService m_serverReturn = ServerModule.ServerModuleFactory.GetServerModule<IProductReturnService>();

        public 还货单明细(string billNo)
        {
            InitializeComponent();

            m_strBillNo = billNo;

            if (m_strBillNo != null)
            {
                m_lnqBill = m_serverReturn.GetBillSingle(m_strBillNo);
            }

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.还货单.ToString(), m_serverReturn);
            m_billMessageServer.BillType = CE_BillTypeEnum.还货单.ToString();

            cmbStorage.DataSource = UniversalFunction.GetStorageTb();

            cmbStorage.DisplayMember = "StorageName";
            cmbStorage.ValueMember = "StorageID";
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        void GetInfo()
        {
            m_lnqBill = new S_ProductReturnBill();

            m_lnqBill.BillNo = txtBillNo.Text;
            m_lnqBill.BillStatus = lbBillStatus.Text;
            m_lnqBill.Remark = txtBillRemark.Text;
            m_lnqBill.StorageID = cmbStorage.SelectedValue.ToString();
            m_lnqBill.DeptCode = txtDepartment.Tag.ToString();

            m_dtList = (DataTable)dataGridView1.DataSource;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            if (m_lnqBill == null)
            {
                lbPropose.Text = BasicInfo.LoginName;
                lbProposerDate.Text = ServerTime.Time.ToString();
                return;
            }

            txtBillNo.Text = m_lnqBill.BillNo;
            txtBillRemark.Text = m_lnqBill.Remark;

            txtDepartment.Text = UniversalFunction.GetDeptName(m_lnqBill.DeptCode);
            txtDepartment.Tag = m_lnqBill.DeptCode;
            lbBillStatus.Text = m_lnqBill.BillStatus;
            lbAffirm.Text = m_lnqBill.Affirm == null ? "" : m_lnqBill.Affirm;
            lbAffirmDate.Text = m_lnqBill.AffirmDate == null ? "" : m_lnqBill.AffirmDate.ToString();
            lbAudit.Text = m_lnqBill.Audit == null ? "" : m_lnqBill.Audit;
            lbAuditDate.Text = m_lnqBill.AuditDate == null ? "" : m_lnqBill.AuditDate.ToString();
            lbPropose.Text = m_lnqBill.Proposer == null ? "" : m_lnqBill.Proposer;
            lbProposerDate.Text = m_lnqBill.ProposerDate == null ? "" : m_lnqBill.ProposerDate.ToString();
            cmbStorage.SelectedValue = m_lnqBill.StorageID;
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
            txtProvider.Text = "";
            txtBatchNo.Text = "";
            numOperationCount.Value = 0;
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckData()
        {
            if (txtDepartment.Tag == null || txtDepartment.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择操作部门");
                return false;
            }
            else if (cmbStorage.SelectedValue == null || cmbStorage.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择还货库房");
                return false;
            }
            else if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请录入还货物品");
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

            switch (GlobalObject.GeneralFunction.StringConvertToEnum<ProductLendReturnBillStatus>(lbBillStatus.Text))
            {
                case ProductLendReturnBillStatus.新建单据:
                    btnPropose.Visible = true;
                    break;
                case ProductLendReturnBillStatus.等待审核:

                    if (GlobalObject.BasicInfo.LoginName == lbPropose.Text)
                    {
                        btnPropose.Visible = true;
                    }

                    btnAudit.Visible = visible;

                    break;
                case ProductLendReturnBillStatus.等待确认:

                    if (GlobalObject.BasicInfo.LoginName == lbPropose.Text)
                    {
                        btnPropose.Visible = true;
                    }

                    btnAffirm.Visible = visible;
                    break;
                case ProductLendReturnBillStatus.单据已完成:
                    break;
                default:
                    break;
            }
        }

        private void 还货单明细_Load(object sender, EventArgs e)
        {
            ClearData();
            ShowInfo();

            if (m_strBillNo == null)
            {
                m_strBillNo = m_billNoControl.GetNewBillNo();
                txtBillNo.Text = m_strBillNo;
                lbBillStatus.Text = MaterialsTransferStatus.新建单据.ToString();
                txtDepartment.Text = UniversalFunction.GetDeptName(BasicInfo.DeptCode);
                txtDepartment.Tag = BasicInfo.DeptCode;
            }

            m_dtList = m_serverReturn.GetListInfo(m_strBillNo);
            dataGridView1.DataSource = m_dtList;
            dataGridView1.Columns["单据号"].Visible = false;

            FlowControl();
        }

        private void 还货单明细_FormClosing(object sender, FormClosingEventArgs e)
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

            if (!m_serverReturn.ProposeBill(m_lnqBill, m_dtList, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                m_billMessageServer.DestroyMessage(m_lnqBill.BillNo);
                m_billMessageServer.SendNewFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号还货单已提交，请等待上级审核", m_lnqBill.BillNo),
                    PlatformManagement.BillFlowMessage_ReceivedUserType.角色,
                m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            if (!m_serverReturn.AuditBill(m_lnqBill.BillNo, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                List<string> tempList =
                    GlobalObject.GeneralFunction.ConvertListTypeToStringList<CE_RoleEnum>(
                    UniversalFunction.GetStoreroomKeeperRoleEnumList(cmbStorage.SelectedValue.ToString()));

                m_billMessageServer.PassFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号还货单已审核，请相关物料管理员确认", m_lnqBill.BillNo),
                    PlatformManagement.BillFlowMessage_ReceivedUserType.角色, tempList);

                MessageDialog.ShowPromptMessage("审核成功");
                this.Close();
            }
        }

        private void btnAffirm_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            if (!m_serverReturn.AffirmBill(m_lnqBill.BillNo, m_dtList, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                List<string> listPersonnel = new List<string>();

                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqBill.Proposer));
                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqBill.Audit));

                m_billMessageServer.EndFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号还货单已完成", m_lnqBill.BillNo), null, listPersonnel);
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

            lbHYDW.Text = txtCode.DataResult["单位"].ToString();

            txtCode.Tag = txtCode.DataResult["物品ID"];
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtProvider.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("供应商不能为空");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtCode.Text;
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["供应商"] = txtProvider.Text;
            dr["物品ID"] = txtCode.Tag;
            dr["批次号"] = txtBatchNo.Text;
            dr["数量"] = numOperationCount.Value;
            dr["备注"] = txtListRemark.Text;
            dr["单位"] = lbHYDW.Text;

            dtTemp.Rows.Add(dr);

            dataGridView1.DataSource = dtTemp;
            ListClear();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (txtProvider.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("供应商不能为空");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            int rowIndex = dataGridView1.CurrentRow.Index;

            dtTemp.Rows[rowIndex]["图号型号"] = txtCode.Text;
            dtTemp.Rows[rowIndex]["物品名称"] = txtName.Text;
            dtTemp.Rows[rowIndex]["规格"] = txtSpec.Text;
            dtTemp.Rows[rowIndex]["供应商"] = txtProvider.Text;
            dtTemp.Rows[rowIndex]["物品ID"] = txtCode.Tag;
            dtTemp.Rows[rowIndex]["批次号"] = txtBatchNo.Text;
            dtTemp.Rows[rowIndex]["数量"] = numOperationCount.Value;
            dtTemp.Rows[rowIndex]["备注"] = txtListRemark.Text;
            dtTemp.Rows[rowIndex]["单位"] = lbHYDW.Text;

            dataGridView1.DataSource = dtTemp;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            dtTemp.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

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
            txtProvider.Text = dataGridView1.CurrentRow.Cells["供应商"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            numOperationCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
            lbHYDW.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
        }

        private void txtDepartment_OnCompleteSearch()
        {
            txtDepartment.Text = txtDepartment.DataResult["部门名称"].ToString();
            txtDepartment.Tag = txtDepartment.DataResult["部门编码"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 数量 > 0 and 借方代码 = '" + txtDepartment.Tag.ToString() + "' and 贷方代码 = '"
                + cmbStorage.SelectedValue.ToString() +"' and 物品ID = " + Convert.ToInt32(txtCode.Tag);
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtProvider.Text = txtBatchNo.DataResult["供应商"].ToString();
        }

        private void txtCode_Enter(object sender, EventArgs e)
        {
            txtCode.StrEndSql = " and 数量 > 0 and 借方代码 = '" + txtDepartment.Tag.ToString() + "' and 贷方代码 = '"
                + cmbStorage.SelectedValue.ToString() + "'";
        }
    }
}
