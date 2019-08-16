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
    public partial class 非产品件检验单明细 : Form
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 非产品件检验单服务组件
        /// </summary>
        IUnProductTestingSingle m_serverUnProductTesting = ServerModuleFactory.GetServerModule<IUnProductTestingSingle>();

        /// <summary>
        /// 物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 检验单LINQ数据集
        /// </summary>
        ZL_UnProductTestingSingleBill m_lnqBill = new ZL_UnProductTestingSingleBill();

        /// <summary>
        /// 验证记录表
        /// </summary>
        DataTable m_dtPro = new DataTable();

        /// <summary>
        /// 检验记录表
        /// </summary>
        DataTable m_dtIn = new DataTable();

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
        /// 保存标志
        /// </summary>
        private bool m_blSaveFlag = false;

        public bool BlSaveFlag
        {
            get { return m_blSaveFlag; }
            set { m_blSaveFlag = value; }
        }

        public 非产品件检验单明细(string billNo)
        {
            InitializeComponent();

            m_strBillNo = billNo;

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.非产品件检验单, m_serverUnProductTesting);

        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns>通过返回True,失败返回False</returns>
        bool CheckMessage()
        {
            switch (m_lnqBill.BillStatus)
            {
                case "新建单据":

                    if (m_lnqBill.GoodsID == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择需要检验的物品");
                        return false;
                    }
                    else if (m_lnqBill.Amount == 0)
                    {
                        MessageDialog.ShowPromptMessage("检验物品数据不能为0");
                        return false;
                    }

                    break;
                default:
                    break;
            }

            return true;
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetMessage()
        {
            if (lbBillNo.Text == "")
            {
                return;
            }

            m_lnqBill = new ZL_UnProductTestingSingleBill();

            m_lnqBill.Amount = numAmount.Value;
            m_lnqBill.Bill_ID = lbBillNo.Text;
            m_lnqBill.BillStatus = lbBillStatus.Text;
            m_lnqBill.Designer = txtDesigner.Text;

            m_lnqBill.GoodsID = txtCode.Tag == null ? 0 : Convert.ToInt32(txtCode.Tag);
            m_lnqBill.InspectorVerdict = txtInspectorVerdict.Text;

            if (rbNo.Checked)
            {
                m_lnqBill.IsOK = false;
            }
            else if (rbYes.Checked)
            {
                m_lnqBill.IsOK = true;
            }

            m_lnqBill.JudgeInfo = txtJudgeInfo.Text;
            m_lnqBill.ProvingVerdict = txtProvingVerdict.Text;
            m_lnqBill.Remark = txtRemark.Text;

            m_dtIn = (DataTable)dataGridViewIn.DataSource;
            m_dtPro = (DataTable)dataGridViewPro.DataSource;

        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowMessage()
        {
            if (m_lnqBill != null)
            {

                View_F_GoodsPlanCost lnqGoods = m_serverGoods.GetGoodsInfoView(m_lnqBill.GoodsID);

                numAmount.Value = m_lnqBill.Amount;

                txtCode.Text = lnqGoods.图号型号;
                txtName.Text = lnqGoods.物品名称;
                txtSpec.Text = lnqGoods.规格;
                txtDesigner.Text = m_lnqBill.Designer;
                txtInspectorVerdict.Text = m_lnqBill.InspectorVerdict;
                txtJudgeInfo.Text = m_lnqBill.JudgeInfo;
                txtProvingVerdict.Text = m_lnqBill.ProvingVerdict;
                txtRemark.Text = m_lnqBill.Remark;

                lbBillNo.Text = m_lnqBill.Bill_ID;
                lbBillStatus.Text = m_lnqBill.BillStatus;
                lbFinalJudge.Text = m_lnqBill.FinalJudge;
                lbFinalJudgeTime.Text = m_lnqBill.FinalJudgeTime == null ? "" : m_lnqBill.FinalJudgeTime.ToString();
                lbInspector.Text = m_lnqBill.Inspector;
                lbInspectorRequest.Text = m_lnqBill.InspectorRequest;
                lbInspectorRequestTime.Text = m_lnqBill.InspectorRequestTime == null ? "" : m_lnqBill.InspectorRequestTime.ToString();
                lbInspectorTime.Text = m_lnqBill.InspectorTime == null ? "" : m_lnqBill.InspectorTime.ToString();
                lbProposer.Text = m_lnqBill.Proposer;
                lbProposerTime.Text = m_lnqBill.ProposerTime == null ? "" : m_lnqBill.ProposerTime.ToString();
                lbProving.Text = m_lnqBill.Proving;
                lbProvingRequest.Text = m_lnqBill.ProvingRequest;
                lbProvingRequestTime.Text = m_lnqBill.ProvingRequestTime == null ? "" : m_lnqBill.ProvingRequestTime.ToString();
                lbProvingTime.Text = m_lnqBill.ProvingTime == null ? "" : m_lnqBill.ProvingTime.ToString();

                if (m_lnqBill.IsOK == null)
                {
                    rbNo.Checked = false;
                    rbYes.Checked = false;
                }
                else
                {
                    if ((bool)m_lnqBill.IsOK)
                    {
                        rbYes.Checked = true;
                        rbNo.Checked = false;
                    }
                    else
                    {
                        rbNo.Checked = true;
                        rbYes.Checked = false;
                    }
                }
            }

            switch (lbBillStatus.Text)
            {
                case "等待检验要求":

                    btnInAdd.Enabled = true;
                    btnInDelete.Enabled = true;
                    btnInUpdate.Enabled = true;

                    txtInInspectionItem.Enabled = true;
                    txtInTechnicalRequirements.Enabled = true;
                    txtInTestingMethod.Enabled = true;
                    txtInTestRecords.Enabled = false;
                    txtInspectorVerdict.Enabled = false;

                    btnProAdd.Enabled = false;
                    btnProDelete.Enabled = false;
                    btnProUpdate.Enabled = false;

                    break;
                case "等待检验":

                    btnInAdd.Enabled = false;
                    btnInDelete.Enabled = false;
                    btnInUpdate.Enabled = true;

                    txtInInspectionItem.Enabled = false;
                    txtInTechnicalRequirements.Enabled = false;
                    txtInTestingMethod.Enabled = false;
                    txtInTestRecords.Enabled = true;
                    txtInspectorVerdict.Enabled = true;

                    btnProAdd.Enabled = false;
                    btnProDelete.Enabled = false;
                    btnProUpdate.Enabled = false;

                    break;
                case "等待验证要求":

                    btnInAdd.Enabled = false;
                    btnInDelete.Enabled = false;
                    btnInUpdate.Enabled = false;

                    btnProAdd.Enabled = true;
                    btnProDelete.Enabled = true;
                    btnProUpdate.Enabled = true;

                    txtProInspectionItem.Enabled = true;
                    txtProTechnicalRequirements.Enabled = true;
                    txtProTestingMethod.Enabled = true;
                    txtProTestRecords.Enabled = false;
                    txtProvingVerdict.Enabled = false;

                    break;
                case "等待验证":

                    btnInAdd.Enabled = false;
                    btnInDelete.Enabled = false;
                    btnInUpdate.Enabled = false;

                    btnProAdd.Enabled = false;
                    btnProDelete.Enabled = false;
                    btnProUpdate.Enabled = true;

                    txtProInspectionItem.Enabled = false;
                    txtProTechnicalRequirements.Enabled = false;
                    txtProTestingMethod.Enabled = false;
                    txtProTestRecords.Enabled = true;
                    txtProvingVerdict.Enabled = true;

                    break;
                default:

                    btnInAdd.Enabled = false;
                    btnInDelete.Enabled = false;
                    btnInUpdate.Enabled = false;

                    btnProAdd.Enabled = false;
                    btnProDelete.Enabled = false;
                    btnProUpdate.Enabled = false;

                    break;
            }

            dataGridViewIn.DataSource = m_dtIn;
            dataGridViewPro.DataSource = m_dtPro;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!CheckMessage())
            {
                return;
            }

            if (!m_serverUnProductTesting.SaveInfo(m_lnqBill, m_dtIn, m_dtPro, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");
                m_blSaveFlag = true;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnInAdd_Click(object sender, EventArgs e)
        {
            m_dtIn = (DataTable)dataGridViewIn.DataSource;

            DataRow dr = m_dtIn.NewRow();

            dr["检验验证项目"] = txtInInspectionItem.Text;
            dr["技术要求"] = txtInTechnicalRequirements.Text;
            dr["检验验证方法"] = txtInTestingMethod.Text;
            dr["检验验证记录"] = txtInTestRecords.Text;

            foreach (DataRow drTemp in m_dtIn.Rows)
            {
                if (drTemp["检验验证项目"].ToString() == dr["检验验证项目"].ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加相同的检验项目");
                    return;
                }
            }

            m_dtIn.Rows.Add(dr);

            dataGridViewIn.DataSource = m_dtIn;
        }

        private void btnInDelete_Click(object sender, EventArgs e)
        {
            m_dtIn = (DataTable)dataGridViewIn.DataSource;

            foreach (DataRow dr in m_dtIn.Rows)
            {
                if (dr["检验验证项目"].ToString() == dataGridViewIn.CurrentRow.Cells["检验验证项目"].Value.ToString()
                    && dr["技术要求"].ToString() == dataGridViewIn.CurrentRow.Cells["技术要求"].Value.ToString()
                    && dr["检验验证方法"].ToString() == dataGridViewIn.CurrentRow.Cells["检验验证方法"].Value.ToString()
                    && dr["检验验证记录"].ToString() == dataGridViewIn.CurrentRow.Cells["检验验证记录"].Value.ToString())
                {
                    m_dtIn.Rows.Remove(dr);
                    break;
                }
            }

            dataGridViewIn.DataSource = m_dtIn;
        }

        private void btnInUpdate_Click(object sender, EventArgs e)
        {

            m_dtIn = (DataTable)dataGridViewIn.DataSource;

            foreach (DataRow dr in m_dtIn.Rows)
            {
                if (dr["检验验证项目"].ToString() == dataGridViewIn.CurrentRow.Cells["检验验证项目"].Value.ToString()
                    && dr["技术要求"].ToString() == dataGridViewIn.CurrentRow.Cells["技术要求"].Value.ToString()
                    && dr["检验验证方法"].ToString() == dataGridViewIn.CurrentRow.Cells["检验验证方法"].Value.ToString()
                    && dr["检验验证记录"].ToString() == dataGridViewIn.CurrentRow.Cells["检验验证记录"].Value.ToString())
                {
                    dr["检验验证项目"] = txtInInspectionItem.Text;
                    dr["技术要求"] = txtInTechnicalRequirements.Text;
                    dr["检验验证方法"] = txtInTestingMethod.Text;
                    dr["检验验证记录"] = txtInTestRecords.Text;
                }
            }

            dataGridViewIn.DataSource = m_dtIn;
        }

        private void btnProAdd_Click(object sender, EventArgs e)
        {
            m_dtPro = (DataTable)dataGridViewPro.DataSource;

            DataRow dr = m_dtPro.NewRow();

            dr["检验验证项目"] = txtProInspectionItem.Text;
            dr["技术要求"] = txtProTechnicalRequirements.Text;
            dr["检验验证方法"] = txtProTestingMethod.Text;
            dr["检验验证记录"] = txtProTestRecords.Text;

            foreach (DataRow drTemp in m_dtIn.Rows)
            {
                if (drTemp["检验验证项目"].ToString() == dr["检验验证项目"].ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加相同的验证项目");
                    return;
                }
            }

            m_dtPro.Rows.Add(dr);

            dataGridViewPro.DataSource = m_dtPro;
        }

        private void btnProDelete_Click(object sender, EventArgs e)
        {
            m_dtPro = (DataTable)dataGridViewPro.DataSource;

            foreach (DataRow dr in m_dtPro.Rows)
            {
                if (dr["检验验证项目"].ToString() == dataGridViewPro.CurrentRow.Cells["检验验证项目"].Value.ToString()
                    && dr["技术要求"].ToString() == dataGridViewPro.CurrentRow.Cells["技术要求"].Value.ToString()
                    && dr["检验验证方法"].ToString() == dataGridViewPro.CurrentRow.Cells["检验验证方法"].Value.ToString()
                    && dr["检验验证记录"].ToString() == dataGridViewPro.CurrentRow.Cells["检验验证记录"].Value.ToString())
                {
                    m_dtPro.Rows.Remove(dr);
                    break;
                }
            }

            dataGridViewPro.DataSource = m_dtPro;
        }

        private void btnProUpdate_Click(object sender, EventArgs e)
        {
            m_dtPro = (DataTable)dataGridViewPro.DataSource;

            foreach (DataRow dr in m_dtPro.Rows)
            {
                if (dr["检验验证项目"].ToString() == dataGridViewPro.CurrentRow.Cells["检验验证项目"].Value.ToString()
                    && dr["技术要求"].ToString() == dataGridViewPro.CurrentRow.Cells["技术要求"].Value.ToString()
                    && dr["检验验证方法"].ToString() == dataGridViewPro.CurrentRow.Cells["检验验证方法"].Value.ToString()
                    && dr["检验验证记录"].ToString() == dataGridViewPro.CurrentRow.Cells["检验验证记录"].Value.ToString())
                {

                    dr["检验验证项目"] = txtProInspectionItem.Text;
                    dr["技术要求"] = txtProTechnicalRequirements.Text;
                    dr["检验验证方法"] = txtProTestingMethod.Text;
                    dr["检验验证记录"] = txtProTestRecords.Text;
                }
            }

            dataGridViewPro.DataSource = m_dtPro;
        }

        private void 非产品件检验单明细_Load(object sender, EventArgs e)
        {
            if (m_blSaveFlag)
            {
                toolStrip1.Visible = false;
            }

            if (m_strBillNo == null)
            {
                lbBillStatus.Text = "新建单据";
                m_strBillNo = m_billNoControl.GetNewBillNo();
                lbBillNo.Text = m_strBillNo;
                
            }

            m_lnqBill = m_serverUnProductTesting.GetInfo(m_strBillNo);
            m_dtIn = m_serverUnProductTesting.GetListInfo(m_strBillNo, "检验");
            m_dtPro = m_serverUnProductTesting.GetListInfo(m_strBillNo, "验证");

            ShowMessage();
        }

        private void 非产品件检验单明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!m_blSaveFlag)
            {
                m_strBillNo = null;
                m_billNoControl.CancelBill();
            }
        }

        private void txtCode_OnCompleteSearch()
        {
            txtCode.Tag = Convert.ToInt32(txtCode.DataResult["序号"]);
            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();
        }

        private void dataGridViewIn_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewIn.CurrentRow == null)
            {
                return;
            }

            txtInInspectionItem.Text = dataGridViewIn.CurrentRow.Cells["检验验证项目"].Value.ToString();
            txtInTechnicalRequirements.Text = dataGridViewIn.CurrentRow.Cells["技术要求"].Value.ToString();
            txtInTestingMethod.Text = dataGridViewIn.CurrentRow.Cells["检验验证方法"].Value.ToString();
            txtInTestRecords.Text = dataGridViewIn.CurrentRow.Cells["检验验证记录"].Value.ToString();
        }

        private void dataGridViewPro_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewPro.CurrentRow == null)
            {
                return;
            }

            txtProInspectionItem.Text = dataGridViewPro.CurrentRow.Cells["检验验证项目"].Value.ToString();
            txtProTechnicalRequirements.Text = dataGridViewPro.CurrentRow.Cells["技术要求"].Value.ToString();
            txtProTestingMethod.Text = dataGridViewPro.CurrentRow.Cells["检验验证方法"].Value.ToString();
            txtProTestRecords.Text = dataGridViewPro.CurrentRow.Cells["检验验证记录"].Value.ToString();
        }
    }
}
