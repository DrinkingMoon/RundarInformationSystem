/******************************************************************************
 *
 * 文件名称:  零星采购明细.cs
 * 作者    :  邱瑶       日期: 2013/11/22
 * 开发平台:  vs2008(c#)
 * 用于    :  生产线管理信息系统
 ******************************************************************************/

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using ServerModule;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 零星采购明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 数据集
        /// </summary>
        B_MinorPurchaseBill m_lnqMinorPurchaseBill = new B_MinorPurchaseBill();

        /// <summary>
        /// 零星采购服务类
        /// </summary>
        IMinorPurchaseBillServer m_minorBillServer = ServerModule.ServerModuleFactory.GetServerModule<IMinorPurchaseBillServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 人员档案服务组件
        /// </summary>
        Service_Peripheral_HR.IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<Service_Peripheral_HR.IPersonnelArchiveServer>();

        public 零星采购明细(PlatformManagement.AuthorityFlag authorityFlag, string billNo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "零星采购申请单";
            m_authFlag = authorityFlag;
            cmbGoodsStatus.SelectedIndex = 0;

            if (billNo == null)
            {
                txtBillStatus.Text = "新建单据";
                txtCompiler.Text = BasicInfo.LoginName;
                txtCompiler.Tag = BasicInfo.LoginID;
                dtpCompilerDate.Value = ServerTime.Time;

                dataGridView1.DataSource = m_minorBillServer.GetListInfo("");
            }
            else
            {
                m_billNo = billNo;
                txtBillNo.Text = m_billNo;
                RefreshDataGridView();
            }

            dataGridView1.Columns["编号"].Visible = false;

            cbUnit.DataSource = UniversalFunction.GetAllUnit();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void 零星采购明细_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            MinorPurchaseBillStatus billStatus =
                GlobalObject.GeneralFunction.StringConvertToEnum<MinorPurchaseBillStatus>(txtBillStatus.Text.Trim());

            switch (billStatus)
            {
                case MinorPurchaseBillStatus.等待部门负责人审核:
                case MinorPurchaseBillStatus.等待高级负责人审批:
                    if (审核toolStripButton.Visible || 领导确认toolStripButton.Visible)
                    {
                        cbDeptDirectorRatify.Checked = true;
                    }
                    break;
                case MinorPurchaseBillStatus.等待分管领导审核:
                    if (分管toolStripButton.Visible)
                    {
                        cbLeaderRatify.Checked = true;
                    }
                    break;
                case MinorPurchaseBillStatus.等待财务审核:
                    if (财务toolStripButton.Visible)
                    {
                        cbCWRatify.Checked = true;
                    }
                    break;
                case MinorPurchaseBillStatus.等待总经理审核:
                    if (总经理toolStripButton.Visible)
                    {
                        cbGMRatify.Checked = true;
                    }
                    break;
                default:
                    break;
            }

            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待采购工程师确认采购.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认到货.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待部门负责人审核.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待财务审核.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待分管领导审核.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待总经理审核.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待高级负责人审批.ToString()
                || txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认日期.ToString())
            {
                保存toolStripButton.Visible = false;
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            B_MinorPurchaseBill minor = m_minorBillServer.GetBillInfo(txtBillNo.Text);
            m_lnqMinorPurchaseBill = minor;

            txtCompiler.Text = UniversalFunction.GetPersonnelName(minor.Compiler);
            txtCompiler.Tag = minor.Compiler;

            DateTime nowTime = ServerTime.Time;

            if (minor.DeptDirector != null)
            {
                string[] deptDirector = minor.DeptDirector.Split(',');

                if (deptDirector.Count() > 1)
                {
                    txtDeptDirector.Text = UniversalFunction.GetPersonnelName(deptDirector[0]);
                    txtDeptDirector.Text += "," + UniversalFunction.GetPersonnelName(deptDirector[1]);
                }
                else
                {
                    txtDeptDirector.Text = UniversalFunction.GetPersonnelName(minor.DeptDirector);
                }

                dtpDeptDirectorDate.Checked = true;
                dtpDeptDirectorDate.Value = Convert.ToDateTime(minor.DeptDirectorDate);
                cbDeptDirectorRatify.Checked = Convert.ToBoolean(minor.DeptDirectorRatify);
                txtDeptDirectorIdea.Text = minor.DeptDirectorIdea;
                dtpCompilerDate.Value = minor.CompileDate;
            }
            else
            {
                dtpDeptDirectorDate.Checked = false;
            }

            if (minor.CWLeader != null && minor.CWLeader != "")
            {
                txtCWLeader.Text = UniversalFunction.GetPersonnelName(minor.CWLeader);
                dtpCWLeaderDate.Checked = true;
                dtpCWLeaderDate.Value = Convert.ToDateTime(minor.CWLeaderDate);
                cbCWRatify.Checked = Convert.ToBoolean(minor.CWRatify);
                txtCWIdea.Text = minor.CWIdea;
            }
            else
            {
                dtpCWLeaderDate.Checked = false;
            }

            if (minor.Leader != null && minor.Leader != "")
            {
                txtLeader.Text = UniversalFunction.GetPersonnelName(minor.Leader);
                dtpLeaderDate.Checked = true;
                dtpLeaderDate.Value = Convert.ToDateTime(minor.LeaderDate);
                cbLeaderRatify.Checked = Convert.ToBoolean(minor.LeaderRatify);
                txtLeaderIdea.Text = minor.LeaderIdea;
            }
            else
            {
                dtpLeaderDate.Checked = false;
            }

            if (minor.GeneralManager != null && minor.GeneralManager != "")
            {
                txtGeneralManager.Text = UniversalFunction.GetPersonnelName(minor.GeneralManager);
                dtpGMDate.Checked = true;
                dtpGMDate.Value = Convert.ToDateTime(minor.GMDate);
                cbGMRatify.Checked = Convert.ToBoolean(minor.GMRatify);
            }
            else
            {
                dtpGMDate.Checked = false;
            }

            if (minor.DeployMan != null && minor.DeployMan != "")
            {
                txtDeployMan.Text = UniversalFunction.GetPersonnelName(minor.DeployMan);
                dtpDeployDate.Checked = true;
                dtpDeployDate.Value = Convert.ToDateTime(minor.DeployDate);
            }
            else
            {
                dtpDeployDate.Checked = false;
            }

            txtEngineer.Text = UniversalFunction.GetPersonnelName(minor.ProcurementEngineer);

            if (minor.BillStatus == MinorPurchaseBillStatus.等待仓管确认.ToString())
            {
                //txtKFRY.Text = BasicInfo.LoginName;
                dtpKFDate.Checked = false;
            }
            else
            {
                txtKFRY.Text = UniversalFunction.GetPersonnelName(minor.KFRY);
                dtpKFDate.Checked = true;
                dtpKFDate.Value = Convert.ToDateTime(minor.KFDate);
            }

            txtBillStatus.Text = minor.BillStatus;
            cmbPurchaseType.Text = minor.PurchaseType;
            tbsDeptCode.Text = UniversalFunction.GetDeptName(minor.ApplicantDeptCode);
            tbsDeptCode.Tag = minor.ApplicantDeptCode;
            txtApplicant.Text = UniversalFunction.GetPersonnelName(minor.Applicant);
            txtApplicant.Tag = minor.Applicant;
            dtpApplicantDate.Value = Convert.ToDateTime(minor.ApplicantDate);
            txtMainRemark.Text = minor.Remark;
            cbExigence.Checked = (bool)minor.Exigence;
            cbIsInBudget.Checked = (bool)minor.IsInBudget;
            txtDeptDirectorIdea.Text = minor.DeptDirectorIdea;
            txtLeaderIdea.Text = minor.LeaderIdea;
            txtCWIdea.Text = minor.CWIdea;
            txtGMIdrea.Text = minor.GMIdrea;
            txtPurpose.Tag = minor.PurposeCode;
            txtPurpose.Text = m_minorBillServer.GetPurposeNameByCode(minor.PurposeCode);
            txtSum.Text = m_minorBillServer.GetSumPrice(txtBillNo.Text).ToString();

            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待高级负责人审批.ToString())
            {
                领导确认toolStripButton.Visible = true;
                string dept = m_billMessageServer.GetHighestDeptCode(UniversalFunction.GetPersonnelInfo(minor.Compiler).部门编码);

                领导确认toolStripButton.Text = dept + "负责人审批";
            }
            else
            {
                领导确认toolStripButton.Visible = false;
                领导确认toolStripButton.Enabled = false;
            }

            if (txtBillStatus.Text == "已完成")
            {
                dtpFinishDate.Value = Convert.ToDateTime(minor.FinishDate);
            }

            dataGridView1.DataSource = m_minorBillServer.GetListInfo(m_billNo);

            if (dataGridView1.Rows.Count > 0)
            {
                this.dataGridView1.ColumnWidthChanged -=
                    new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView("零星采购申请单", dataGridView1);

                this.dataGridView1.ColumnWidthChanged +=
                    new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);

                userControlDataLocalizer1.Init(dataGridView1, this.Name,
                   UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                dataGridView1.Refresh();
            }
        }

        private void txtApplicant_OnCompleteSearch()
        {
            txtApplicant.Text = txtApplicant.DataResult["姓名"].ToString();
            txtApplicant.Tag = txtApplicant.DataResult["工号"].ToString();
        }

        private void txtApplicant_Enter(object sender, EventArgs e)
        {
            if (tbsDeptCode.Tag != null)
            {
                string sql = "";
                sql += " and Dept='" + tbsDeptCode.Tag + "'";
                txtApplicant.StrEndSql = sql;
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择申请的部门！");
            }
        }

        private void tbsDeptCode_OnCompleteSearch()
        {
            tbsDeptCode.Text = tbsDeptCode.DataResult["部门名称"].ToString();
            tbsDeptCode.Tag = tbsDeptCode.DataResult["部门编码"].ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtGoodsCode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择物品再进行此操作！");
                return;
            }

            if (numCount.Value <= 0)
            {
                MessageDialog.ShowPromptMessage("请填写需要购买的数量！");
                return;
            }

            if (numPrice.Value <= 0)
            {
                MessageDialog.ShowPromptMessage("请填写预算单价！");
                return;
            }

            if (!dtpRequireArriveDate.Checked)
            {
                MessageDialog.ShowPromptMessage("请选择要求到货日期！");
                return;
            }

            if (dtpRequireArriveDate.Value <= ServerTime.Time)
            {
                MessageDialog.ShowPromptMessage("要求到货日期不能小于编制日期！");
                return;
            }

            if (cmbPurchaseType.SelectedIndex == 4)
            {
                if (!rbtnYes.Checked && !rbtnNo.Checked)
                {
                    MessageDialog.ShowPromptMessage("请选择是否有图纸发质管部！");
                    return;
                }
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            if (dtTemp != null && dtTemp.Rows.Count > 0)
            {
                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (txtGoodsCode.Text == dtTemp.Rows[i]["图号型号"].ToString()
                        && txtGoodsName.Text == dtTemp.Rows[i]["物品名称"].ToString()
                        && txtSpec.Text == dtTemp.Rows[i]["规格"].ToString()
                        && txtProvider.Text == dtTemp.Rows[i]["供应商"].ToString())
                    {
                        MessageDialog.ShowPromptMessage("不能录入重复物品");
                        return;
                    }
                }
            }

            DataTable partDt = m_minorBillServer.GetPart(Convert.ToInt32(txtGoodsCode.Tag), txtBillNo.Text);

            if (partDt != null && partDt.Rows.Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("已经有人申请购买此物品，是否继续申请？") != DialogResult.Yes)
                {
                    ClearControl();
                    return;
                }
            }

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtGoodsCode.Text;

            if (txtGoodsCode.Tag != null)
            {
                dr["GoodsID"] = txtGoodsCode.Tag.ToString();
            }

            dr["物品名称"] = txtGoodsName.Text;
            dr["规格"] = txtSpec.Text;
            dr["申请数量"] = numCount.Value;
            dr["预算价格"] = numPrice.Value;
            dr["单位"] = cbUnit.Text;
            dr["备注"] = txtListRemark.Text;
            dr["要求到货日期"] = dtpRequireArriveDate.Value;
            dr["物品状态"] = "等待采购";
            dr["最终到货日期"] = dtpRequireArriveDate.Value;
            dr["库存数量"] = lbStockCount.Text;
            dr["供应商"] = txtProvider.Text;

            if (rbtnYes.Checked)
            {
                dr["是否有图纸发品保部"] = true;
            }
            else
            {
                dr["是否有图纸发品保部"] = false;
            }

            dtTemp.Rows.Add(dr);

            ClearControl();

            dataGridView1.DataSource = dtTemp;
        }

        //初始化控件
        void ClearControl()
        {
            txtGoodsCode.Text = "";
            txtGoodsCode.Tag = null;
            txtGoodsName.Text = "";
            txtSpec.Text = "";
            lbStockCount.Text = "0";
            numPrice.Value = 0;
            numCount.Value = 1;
            txtProvider.Text = "";
            cmbGoodsStatus.SelectedIndex = 0;
            dtpGoodsDate.Value = ServerTime.Time;
            txtListRemark.Text = "";
            rbtnYes.Checked = false;
            rbtnNo.Checked = false;
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString())
                    && txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认日期.ToString())
                {
                    txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString() +
                        " " + BasicInfo.LoginName + "修改到货日期为：" + dtpRequireArriveDate.Value.ToString() +
                        "原日期为：" + dataGridView1.CurrentRow.Cells["要求到货日期"].Value.ToString();
                }
                else if (BasicInfo.LoginName == txtCompiler.Text.Trim())
                {
                    dataGridView1.CurrentRow.Cells["要求到货日期"].Value = dtpRequireArriveDate.Value;
                    dataGridView1.CurrentRow.Cells["最终到货日期"].Value = dtpRequireArriveDate.Value;
                }
                else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString())
                    && txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认到货.ToString())
                {
                    if (cmbGoodsStatus.Text == "已到货")
                    {
                        dataGridView1.CurrentRow.Cells["最终到货日期"].Value = ServerTime.Time;
                    }
                }

                dataGridView1.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;

                if (txtGoodsCode.Tag != null)
                {
                    dataGridView1.CurrentRow.Cells["GoodsID"].Value = txtGoodsCode.Tag.ToString();
                }

                dataGridView1.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
                dataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
                dataGridView1.CurrentRow.Cells["申请数量"].Value = numCount.Value;
                dataGridView1.CurrentRow.Cells["预算价格"].Value = numPrice.Value;
                dataGridView1.CurrentRow.Cells["单位"].Value = cbUnit.Text;
                dataGridView1.CurrentRow.Cells["备注"].Value = txtListRemark.Text;
                dataGridView1.CurrentRow.Cells["物品状态"].Value = cmbGoodsStatus.Text;
                dataGridView1.CurrentRow.Cells["库存数量"].Value = lbStockCount.Text;
                dataGridView1.CurrentRow.Cells["供应商"].Value = txtProvider.Text;

                if (Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否有图纸发品保部"].Value))
                {
                    rbtnYes.Checked = true;
                }
                else
                {
                    rbtnNo.Checked = true;
                }
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
                DataTable dt = (DataTable)dataGridView1.DataSource;

                dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                dataGridView1.DataSource = dt;
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
                if (dataGridView1.CurrentRow.Cells["GoodsID"].Value.ToString() != "")
                {
                    txtGoodsCode.Tag = dataGridView1.CurrentRow.Cells["GoodsID"].Value.ToString();
                }

                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                numCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["申请数量"].Value);
                cbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
                txtListRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                txtProvider.Text = dataGridView1.CurrentRow.Cells["供应商"].Value.ToString();
                numPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["预算价格"].Value);
                cmbGoodsStatus.Text = dataGridView1.CurrentRow.Cells["物品状态"].Value.ToString();
                lbStockCount.Text = dataGridView1.CurrentRow.Cells["库存数量"].Value.ToString();

                dtpRequireArriveDate.Checked = true;
                dtpRequireArriveDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["要求到货日期"].Value);

                if (Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否有图纸发品保部"].Value))
                {
                    rbtnYes.Checked = true;
                }
                else
                {
                    rbtnNo.Checked = true;
                }

                if (cmbGoodsStatus.Text == "已到货")
                {
                    dtpGoodsDate.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["最终到货日期"].Value);
                }
            }
        }

        private void 保存toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.已完成.ToString() ||
                txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认到货.ToString())
            {
                MessageDialog.ShowPromptMessage("此状态下不能进行修改！");
                return;
            }

            if (!CheckControls())
            {
                return;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购部办公室文员.ToString()) &&
                txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待采购部调配人员.ToString())
            {
                if (txtEngineer.Tag == null || txtEngineer.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择负责采购的采购工程师！");
                    return;
                }

                if (!m_minorBillServer.UpdateMinorPurchaseBill(txtBillNo.Text, txtEngineer.Tag.ToString(), out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("分配成功！");

                    this.Close();
                }
            }
            else if (txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.等待采购工程师确认采购.ToString()
                && txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.等待采购部调配人员.ToString()
                && txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.等待确认到货.ToString()
                && txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.已完成.ToString())
            {
                if (txtPurpose.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择零件用途！");
                    return;
                }

                GetMessage();

                if (m_billNo != null)
                {
                    m_lnqMinorPurchaseBill.BillNo = m_billNo;
                }

                DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                m_lnqMinorPurchaseBill.Compiler = BasicInfo.LoginID;
                m_lnqMinorPurchaseBill.CompileDate = ServerTime.Time;
                m_lnqMinorPurchaseBill.BillStatus = MinorPurchaseBillStatus.等待仓管确认.ToString();
                m_lnqMinorPurchaseBill.PurposeCode = txtPurpose.Tag.ToString();

                if (!m_minorBillServer.InsertBill(m_lnqMinorPurchaseBill, dtTemp, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("提交成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqMinorPurchaseBill.Applicant = txtApplicant.Tag.ToString();
            m_lnqMinorPurchaseBill.ApplicantDate = ServerTime.Time;
            m_lnqMinorPurchaseBill.ApplicantDeptCode = tbsDeptCode.Tag.ToString();
            m_lnqMinorPurchaseBill.Remark = txtMainRemark.Text;
            m_lnqMinorPurchaseBill.DeptDirectorDate = dtpDeptDirectorDate.Value;
            m_lnqMinorPurchaseBill.KFDate = dtpKFDate.Value;
            m_lnqMinorPurchaseBill.PurchaseType = cmbPurchaseType.Text;
            m_lnqMinorPurchaseBill.Exigence = cbExigence.Checked;
            m_lnqMinorPurchaseBill.IsInBudget = cbIsInBudget.Checked;
        }

        /// <summary>
        /// 检测输入的正确性
        /// </summary>
        /// <returns>输入无误返回True否则返回False</returns>
        private bool CheckControls()
        {
            if (cmbPurchaseType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择采购类别！");
                return false;
            }

            if (tbsDeptCode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择申请部门！");
                return false;
            }

            if (txtApplicant.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请选择申请人员！");
                return false;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要购买的零件！");
                return false;
            }

            //if (!cbIsInBudget.Checked)
            //{
            //    if (MessageDialog.ShowEnquiryMessage("采购申请单的零件是否在本月部门预算内吗？") == DialogResult.Yes)
            //    {
            //        cbIsInBudget.Checked = true;
            //    }
            //}                     

            return true;
        }

        private void 日期修改toolStripButton_Click(object sender, EventArgs e)
        {
            if (m_billNo != "" && txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认日期.ToString())
            {
                m_lnqMinorPurchaseBill.BillNo = m_billNo;

                if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("日期提交成功！");

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 审核toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待部门负责人审核.ToString())
            {
                if (!cbDeptDirectorRatify.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否【不同意】此条单号为【" + txtBillNo.Text.Trim() + "】的零星采购单？")
                        == DialogResult.No)
                    {
                        return;
                    }
                }

                if (txtDeptDirectorIdea.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写负责人意见！");
                    return;
                }

                DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    DataTable partDt = m_minorBillServer.GetPart(Convert.ToInt32(dtTemp.Rows[i]["GoodsID"].ToString()), txtBillNo.Text);

                    if (partDt != null && partDt.Rows.Count > 0)
                    {
                        if (MessageDialog.ShowEnquiryMessage(
                            "【" + partDt.Rows[0]["BillNo"] + "】号单据已经有人申请购买【"
                            + dtTemp.Rows[i]["物品名称"] + "】，是否继续申请？") != DialogResult.Yes)
                        {
                            m_billMessageServer.PassFlowMessage(txtBillNo.Text, "已经有人申请购买【"
                               + dtTemp.Rows[i]["物品名称"] + "】，部门负责人退回",
                             BillFlowMessage_ReceivedUserType.用户, m_lnqMinorPurchaseBill.Compiler);
                            return;
                        }
                    }
                }

                if (txtDeptDirectorIdea.Text.Trim() != "")
                {
                    m_lnqMinorPurchaseBill.BillNo = m_billNo;
                    m_lnqMinorPurchaseBill.DeptDirectorIdea = txtDeptDirectorIdea.Text;
                    m_lnqMinorPurchaseBill.DeptDirectorRatify = cbDeptDirectorRatify.Checked;

                    if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("审批成功！");
                        this.Close();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请填写部门领导意见！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 采购toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待采购工程师确认采购.ToString())
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                if (txtEngineer.Text.Trim() == BasicInfo.LoginName)
                {
                    if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString()))
                    {
                        m_lnqMinorPurchaseBill.BillNo = m_billNo;

                        if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, dt, out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("审批成功！");
                            this.Close();
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请采购工程师【" + txtEngineer.Text + "】处理！");
                }

                this.Close();
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 分管toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待分管领导审核.ToString())
            {
                string deptCode = UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).部门编码;

                IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(deptCode, "2");
                bool flag = false;

                if (directorGroup != null && directorGroup.Count() > 0)
                {
                    foreach (var item in directorGroup)
                    {
                        if (BasicInfo.LoginID == item.员工编号)
                        {
                            flag = true;

                            break;
                        }
                    }
                }

                if (flag)
                {
                    if (txtLeaderIdea.Text.Trim() == "")
                    {
                        MessageDialog.ShowPromptMessage("请填写分管领导意见！");
                        return;
                    }

                    if (!cbLeaderRatify.Checked)
                    {
                        if (MessageDialog.ShowEnquiryMessage("您是否【不同意】此条单号为【" + txtBillNo.Text.Trim() + "】的零星采购单？")
                            == DialogResult.No)
                        {
                            return;
                        }
                    }

                    m_lnqMinorPurchaseBill.BillNo = m_billNo;
                    m_lnqMinorPurchaseBill.LeaderIdea = txtLeaderIdea.Text;
                    m_lnqMinorPurchaseBill.LeaderRatify = cbLeaderRatify.Checked;

                    if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("审批成功！");
                        this.Close();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您不是【" + tbsDeptCode.Text + "】的分管领导！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 财务toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待财务审核.ToString())
            {
                if (txtCWIdea.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写财务意见！");
                    return;
                }

                if (!cbCWRatify.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否【不同意】此条单号为【" + txtBillNo.Text.Trim() + "】的零星采购单？")
                        == DialogResult.No)
                    {
                        return;
                    }
                }

                m_lnqMinorPurchaseBill.BillNo = m_billNo;
                m_lnqMinorPurchaseBill.CWIdea = txtCWIdea.Text;
                m_lnqMinorPurchaseBill.CWRatify = cbCWRatify.Checked;

                if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 总经理toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待总经理审核.ToString())
            {
                if (txtGMIdrea.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写总经理意见！");
                    return;
                }

                if (!cbGMRatify.Checked)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否【不同意】此条单号为【" + txtBillNo.Text.Trim() + "】的零星采购单？")
                        == DialogResult.No)
                    {
                        return;
                    }
                }

                m_lnqMinorPurchaseBill.BillNo = m_billNo;
                m_lnqMinorPurchaseBill.GMIdrea = txtGMIdrea.Text;
                m_lnqMinorPurchaseBill.GMRatify = cbGMRatify.Checked;

                if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审批成功！");
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            DataTable dtGood = (DataTable)dataGridView1.DataSource;

            if (txtBillStatus.Text != MinorPurchaseBillStatus.新建单据.ToString()
                && txtBillStatus.Text != MinorPurchaseBillStatus.等待仓管确认.ToString())
            {
                MessageDialog.ShowPromptMessage("此单据状态下不能修改零件！");
                return;
            }

            if (txtCompiler.Text.Trim() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("您不是编制人，不能修改此单据的零件信息！");
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetAllGoodsInfo();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtGoodsName.ReadOnly = true;
                txtGoodsCode.ReadOnly = true;
                txtSpec.ReadOnly = true;
                cbUnit.Visible = false;

                txtGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txtGoodsName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
                cbUnit.Text = form.GetStringDataItem("单位");
                cbUnit.Visible = true;
                txtGoodsCode.Tag = form.GetDataItem("序号");

                IStoreServer storeService = ServerModule.ServerModuleFactory.GetServerModule<IStoreServer>();

                numPrice.Value = storeService.GetGoodsAveragePrice(Convert.ToInt32(txtGoodsCode.Tag), "");

                lbStockCount.Text = m_minorBillServer.GetGoodsStock(Convert.ToInt32(txtGoodsCode.Tag));
            }
        }

        /// <summary>
        /// 采购工程师确认到货数据无误
        /// </summary>
        /// <returns>无误返回True，否则返回false</returns>
        bool IsEngineerQuery()
        {
            if (m_minorBillServer.IsFinishMinorPur(txtBillNo.Text))
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                if (txtEngineer.Text.Trim() == BasicInfo.LoginName)
                {
                    m_lnqMinorPurchaseBill.BillNo = m_billNo;

                    if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, dt, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请采购工程师【" + txtEngineer.Text + "】处理！");
                    return false;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("此单据关联零星采购申请单，请先完成零星采购申请单再进行此操作！");
                return false;
            }
        }

        private void 确认到货toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待确认到货.ToString())
            {
                if (IsEngineerQuery())
                {
                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 仓管确认toolStripButton_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您确认好零件信息的正确性吗？") == DialogResult.Yes)
            {
                if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待仓管确认.ToString())
                {
                    if (cmbPurchaseType.Text == "量具采购" || cmbPurchaseType.Text == "检具加工")
                    {
                        if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.量检具库管理员.ToString()))
                        {
                            MessageDialog.ShowPromptMessage("您不是量检具库房管理员！");
                            return;
                        }
                    }

                    GetMessage();

                    m_lnqMinorPurchaseBill.BillNo = m_billNo;

                    DataTable dtTemp = (DataTable)dataGridView1.DataSource;

                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        DataTable partDt = m_minorBillServer.GetPart(Convert.ToInt32(dtTemp.Rows[i]["GoodsID"].ToString()), txtBillNo.Text);

                        if (partDt != null && partDt.Rows.Count > 0)
                        {
                            if (MessageDialog.ShowEnquiryMessage(
                                "【" + partDt.Rows[0]["BillNo"] + "】号单据已经有人申请购买【"
                                + dtTemp.Rows[i]["物品名称"] + "】，是否继续申请？") != DialogResult.Yes)
                            {
                                m_billMessageServer.PassFlowMessage(m_billNo, "已经有人申请购买【"
                                + dtTemp.Rows[i]["物品名称"] + "】，仓库退回",
                              BillFlowMessage_ReceivedUserType.用户, m_lnqMinorPurchaseBill.Compiler);
                                return;
                            }
                        }
                    }

                    if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("确认成功！");
                        this.Close();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请确认单据状态！");
                }
            }
        }

        private void cbHand_CheckedChanged(object sender, EventArgs e)
        {
            //if (cbHand.Checked)
            //{
            //    if (MessageDialog.ShowEnquiryMessage("确定已经查找过零件，并且系统中没有需要请购的零件吗？") == DialogResult.No)
            //    {
            //        cbHand.Checked = false;
            //        return;
            //    }

            //    txtGoodsCode.ReadOnly = false;
            //    txtGoodsName.ReadOnly = false;
            //    txtSpec.ReadOnly = false;
            //    cbUnit.Enabled = true;
            //    cbUnit.Visible = true;
            //}
            //else
            //{
            //    txtGoodsCode.ReadOnly = true;
            //    txtGoodsName.ReadOnly = true;
            //    txtSpec.ReadOnly = true;
            //    cbUnit.Enabled = false;
            //}
        }

        private void 回退toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.已完成.ToString()
                //&& txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.等待采购工程师确认采购.ToString()
                && txtBillStatus.Text.Trim() != MinorPurchaseBillStatus.等待确认到货.ToString())
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.零星采购单, m_billNo, txtBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_minorBillServer.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_error, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                        }
                    }

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 领导确认toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillStatus.Text.Trim() == MinorPurchaseBillStatus.等待高级负责人审批.ToString())
            {
                Service_Peripheral_HR.IPersonnelArchiveServer m_personnerServer =
                    Service_Peripheral_HR.ServerModuleFactory.GetServerModule<Service_Peripheral_HR.IPersonnelArchiveServer>();

                string deptCode = (CommentParameter.DepotDataContext).Fun_get_TopFatherDeptCode(UniversalFunction.GetPersonnelInfo(txtApplicant.Tag.ToString()).部门编码);

                IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(deptCode, "1");
                bool flag = false;

                if (directorGroup != null && directorGroup.Count() > 0)
                {
                    foreach (var item in directorGroup)
                    {
                        if (BasicInfo.LoginID == item.员工编号)
                        {
                            flag = true;

                            break;
                        }
                    }
                }

                if (flag)
                {
                    if (!cbDeptDirectorRatify.Checked)
                    {
                        if (MessageDialog.ShowEnquiryMessage("您是否【不同意】此条单号为【" + txtBillNo.Text.Trim() + "】的零星采购单？")
                            == DialogResult.No)
                        {
                            return;
                        }
                    }

                    if (txtDeptDirectorIdea.Text.Trim() != "")
                    {
                        m_lnqMinorPurchaseBill.BillNo = m_billNo;
                        m_lnqMinorPurchaseBill.DeptDirectorIdea = txtDeptDirectorIdea.Text;
                        m_lnqMinorPurchaseBill.DeptDirectorRatify = cbDeptDirectorRatify.Checked;

                        if (!m_minorBillServer.OperationInfo(m_lnqMinorPurchaseBill, (DataTable)dataGridView1.DataSource, out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("审批成功！");
                            this.Close();
                        }
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("请填写部门领导意见！");
                        return;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("您不是最高部门的负责人！");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
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

        private void txtEngineer_OnCompleteSearch()
        {
            txtEngineer.Text = txtEngineer.DataResult["姓名"].ToString();
            txtEngineer.Tag = txtEngineer.DataResult["工号"].ToString();
        }

        private void txtEngineer_Enter(object sender, EventArgs e)
        {
            string sql = "";
            sql += " and Dept like '%CG%'";
            txtEngineer.StrEndSql = sql;
        }

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            领料用途 form = new 领料用途();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPurpose.Tag = form.SelectedData.Code;
                txtPurpose.Text = form.SelectedData.Purpose;
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams("零星采购申请单", e.Column);
        }

        private void btnAllArrival_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            foreach (DataRow dr in dtTemp.Rows)
            {
                dr["物品状态"] = "已到货";
            }

            dataGridView1.DataSource = dtTemp;
        }

        /// <summary>
        /// 总经理批准checkbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbGMRatify_CheckedChanged(object sender, EventArgs e)
        {
            if (txtBillStatus.Text != MinorPurchaseBillStatus.等待总经理审核.ToString())
            {
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtGMIdrea.Text) && cbGMRatify.Checked)
            {
                txtGMIdrea.Text = "同意";
            }

            if (!cbGMRatify.Checked && txtGMIdrea.Text == "同意")
            {
                txtGMIdrea.Text = "不同意";
            }
        }

        /// <summary>
        /// 财务审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbCWRatify_CheckedChanged(object sender, EventArgs e)
        {
            if (txtBillStatus.Text != MinorPurchaseBillStatus.等待财务审核.ToString())
            {
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtCWIdea.Text) && cbCWRatify.Checked)
            {
                txtCWIdea.Text = "同意";
            }

            if (!cbCWRatify.Checked && txtCWIdea.Text == "同意")
            {
                txtCWIdea.Text = "不同意";
            }
        }

        /// <summary>
        /// 分管领导审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbLeaderRatify_CheckedChanged(object sender, EventArgs e)
        {
            if (txtBillStatus.Text != MinorPurchaseBillStatus.等待分管领导审核.ToString())
            {
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtLeaderIdea.Text) && cbLeaderRatify.Checked)
            {
                txtLeaderIdea.Text = "同意";
            }

            if (!cbLeaderRatify.Checked && txtLeaderIdea.Text == "同意")
            {
                txtLeaderIdea.Text = "不同意";
            }
        }

        /// <summary>
        /// 负责人审批
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbDeptDirectorRatify_CheckedChanged(object sender, EventArgs e)
        {
            if (txtBillStatus.Text != MinorPurchaseBillStatus.等待部门负责人审核.ToString())
            {
                return;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtDeptDirectorIdea.Text) && cbDeptDirectorRatify.Checked)
            {
                txtDeptDirectorIdea.Text = "同意";
            }

            if (!cbDeptDirectorRatify.Checked && txtDeptDirectorIdea.Text == "同意")
            {
                txtDeptDirectorIdea.Text = "不同意";
            }
        }
    }
}
