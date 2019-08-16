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
    /// <summary>
    /// 工装台帐明细信息界面
    /// </summary>
    public partial class 工装总成信息 : Form
    {
        DataTable _ApplicableGoods = new DataTable();

        /// <summary>
        /// 工装名称
        /// </summary>
        private string m_strFrockName = "";

        public string StrFrockName
        {
            get { return m_strFrockName; }
            set { m_strFrockName = value; }
        }

        /// <summary>
        /// 工装编号
        /// </summary>
        private string m_strFrockNumber = "";

        public string StrFrockNumber
        {
            get { return m_strFrockNumber; }
            set { m_strFrockNumber = value; }
        }

        /// <summary>
        /// 保存标志
        /// </summary>
        private bool m_blSave = false;

        public bool BlSave
        {
            get { return m_blSave; }
            set { m_blSave = value; }
        }

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 权限管理
        /// </summary>
        AuthorityFlag m_strAuthFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据集
        /// </summary>
        S_FrockStandingBook m_lnqStandingBook;

        /// <summary>
        /// 服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        public 工装总成信息(int goodsID, string frocknumber, bool flag, AuthorityFlag m_authFlag, bool addflag)
        {
            InitializeComponent();
            m_strAuthFlag = m_authFlag;

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);
            toolStrip1.Visible = true;

            lbScarpPersonnel.Text = "";
            lbScarpTime.Text = "";

            if (!flag)
            {
                labelTitle.Text = "工装分装台帐信息";
                this.StartPosition = FormStartPosition.WindowsDefaultLocation;
                tabControl1.TabPages.RemoveAt(1);
            }
            else
            {
                labelTitle.Text = "工装总装台帐信息";
            }

            if (addflag)
            {
                txtName.ShowResultForm = true;
                txtFrockNumber.Text = m_serverFrockStandingBook.GetNewFrockNumber();
                txtFrockNumber.ReadOnly = false;

                if (goodsID != 0 && frocknumber != "")
                {
                    txtParentFrockNumber.Text = frocknumber;
                    F_GoodsPlanCost lnqGoodsInfo = m_serverBasicGoods.GetGoodsInfo(goodsID);
                    txtParentCode.Text = lnqGoodsInfo.GoodsCode;
                    txtParentName.Text = lnqGoodsInfo.GoodsName;
                    txtParentName.Tag = goodsID;
                }
            }
            else
            {
                txtName.ShowResultForm = false;
                txtFrockNumber.ReadOnly = true;

                DataRow drInfo = m_serverFrockStandingBook.GetInDepotBillInfo(frocknumber);

                if (drInfo != null)
                {
                    txtProposer.Text = drInfo["申请人"].ToString();
                    txtProposer.Tag = drInfo["申请人工号"].ToString();
                    txtProviderCode.Text = drInfo["供应商编码"].ToString();
                    txtProviderName.Text = drInfo["供应商简称"].ToString();
                    txtDesigner.Text = drInfo["设计人"].ToString();
                    txtDesigner.Tag = drInfo["设计人工号"].ToString();
                }

                m_lnqStandingBook = m_serverFrockStandingBook.GetBookInfo(goodsID, frocknumber);
            }

            ShowMessage();
        }

        void txtDesigner_OnCompleteSearch()
        {
            txtDesigner.Text = txtDesigner.DataResult["姓名"].ToString();
        }

        void txtParentFrockNumber_OnCompleteSearch()
        {
            txtParentFrockNumber.Text = txtParentFrockNumber.DataResult["工装编号"].ToString();
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"].ToString());
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
        }

        void txtParentName_OnCompleteSearch()
        {
            txtParentName.Text = txtParentName.DataResult["物品名称"].ToString();
            txtParentName.Tag = Convert.ToInt32(txtParentName.DataResult["序号"].ToString());
            txtParentCode.Text = txtParentName.DataResult["图号型号"].ToString();
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckMessage()
        {
            if (txtName.Text.Trim() == "" || txtName.Tag.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请录入工装名称");
                return false;
            }
            else if (txtFrockNumber.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请录入工装编号");
                return false;
            }
            else if (txtName.Text.Trim() != "" && txtName.Tag.ToString() != "" && txtFrockNumber.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请录入工装的编号");
                return false;
            }
            else if (txtParentName.Text.Trim() != "" && txtParentName.Tag.ToString() != "" && txtParentFrockNumber.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请录入父级工装的编号");
                return false;
            }
            else if (cmbApplyToWorkShop.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择适用车间");
                return false;
            }
            else if (txtName.Tag.ToString() == (txtParentName.Tag == null ? "": txtParentName.Tag.ToString() )
                || txtFrockNumber.Text.Trim() == txtParentFrockNumber.Text.Trim())
            {
                MessageDialog.ShowPromptMessage("工装不能与父级工装一样，且编号相同");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowMessage()
        {
            if (m_lnqStandingBook != null)
            {
                View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(m_lnqStandingBook.GoodsID));
                View_F_GoodsPlanCost tempParentGoodsLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(m_lnqStandingBook.ParentGoodsID));

                if (tempParentGoodsLnq != null)
                {
                    txtParentCode.Text = tempParentGoodsLnq.图号型号;
                    txtParentName.Text = tempParentGoodsLnq.物品名称;
                    txtParentName.Tag = Convert.ToInt32(m_lnqStandingBook.ParentGoodsID);
                    txtParentFrockNumber.Text = m_lnqStandingBook.ParentFrockNumber;

                }

                if (tempGoodsLnq == null)
                {
                    MessageDialog.ShowPromptMessage("系统中无此物品信息");
                }

                txtCode.Text = tempGoodsLnq.图号型号;
                txtName.Text = tempGoodsLnq.物品名称;
                txtName.Tag = Convert.ToInt32(m_lnqStandingBook.GoodsID);
                txtFrockNumber.Text = m_lnqStandingBook.FrockNumber;


                chkIsInStock.Checked = m_lnqStandingBook.IsInStock;
                txtScarpBillID.Text = m_lnqStandingBook.ScarpBillID;
                txtScarpReason.Text = m_lnqStandingBook.ScarpReason;
                lbScarpPersonnel.Text = m_lnqStandingBook.ScarpPersonnel;
                lbScarpTime.Text = m_lnqStandingBook.ScarpTime.ToString();

                numIdentifyCycle.Value = Convert.ToDecimal(m_lnqStandingBook.IdentifyCycle);
                txtApplyToDevice.Text = m_lnqStandingBook.ApplyToDevice;
                txtApplyToProcess.Text = m_lnqStandingBook.ApplyToProcess;
                txtApplyToProductCode.Text = m_lnqStandingBook.ApplyToProductCode;
                txtApplyToProductName.Text = m_lnqStandingBook.ApplyToProductName;
                txtDesigner.Text = m_lnqStandingBook.Designer;
                cmbApplyToWorkShop.Text = m_lnqStandingBook.ApplyToWorkShop;

                _ApplicableGoods = m_serverFrockStandingBook.GetApplicableGoods(m_lnqStandingBook.FrockNumber);

                if (m_lnqStandingBook.IdentifyCycleType != null)
                {
                    rbTypeCount.Checked = (bool)(m_lnqStandingBook.IdentifyCycleType == rbTypeCount.Text);
                    rbTypeTime.Checked = (bool)(m_lnqStandingBook.IdentifyCycleType == rbTypeTime.Text);
                }
            }

            dgvFrockReport.DataSource = m_serverFrockStandingBook.GetProvingReport(
                Convert.ToInt32( txtName.Tag), txtFrockNumber.Text);
            dgvDispensingInfo.DataSource = m_serverFrockStandingBook.GetSplitCharging(
                Convert.ToInt32(txtName.Tag), txtFrockNumber.Text, chkIsShowDispensing.Checked);

            dgvCheckOutItems.DataSource = m_serverFrockStandingBook.GetCheckItemsContent(txtFrockNumber.Text);

            dgvCheckOutItems.Columns[0].Width = 250;

            dgvOperationBillInfo.DataSource = m_serverFrockStandingBook.GetFrockOperation(Convert.ToInt32(txtName.Tag), txtFrockNumber.Text);
            dgvRepairInfo.DataSource = m_serverFrockStandingBook.GetServiceTable(Convert.ToInt32(txtName.Tag), txtFrockNumber.Text);
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqStandingBook = new S_FrockStandingBook();

            m_lnqStandingBook.IsInStock = chkIsInStock.Checked;
            m_lnqStandingBook.ApplyToDevice = txtApplyToDevice.Text;
            m_lnqStandingBook.ApplyToProcess = txtApplyToProcess.Text;
            m_lnqStandingBook.ApplyToProductCode = txtApplyToProductCode.Text;
            m_lnqStandingBook.ApplyToProductName = txtApplyToProductName.Text;
            m_lnqStandingBook.ApplyToWorkShop = cmbApplyToWorkShop.Text;
            m_lnqStandingBook.Designer = txtDesigner.Text;
            m_lnqStandingBook.IdentifyCycle = numIdentifyCycle.Value;

            m_lnqStandingBook.FinalPersonnel = BasicInfo.LoginName;
            m_lnqStandingBook.FinalTime = ServerTime.Time;
            m_lnqStandingBook.FrockNumber = txtFrockNumber.Text;
            m_lnqStandingBook.GoodsID = Convert.ToInt32(txtName.Tag);

            if (rbTypeCount.Checked)
            {
                m_lnqStandingBook.IdentifyCycleType = rbTypeCount.Text;
            }
            else if (rbTypeTime.Checked)
            {
                m_lnqStandingBook.IdentifyCycleType = rbTypeTime.Text;
            }

            if (txtParentName.Text.Trim() == "")
            {
                m_lnqStandingBook.ParentGoodsID = null;
                m_lnqStandingBook.ParentFrockNumber = null;
            }
            else
            {
                m_lnqStandingBook.ParentFrockNumber = txtParentFrockNumber.Text;
                m_lnqStandingBook.ParentGoodsID = Convert.ToInt32(txtParentName.Tag);
            }
            

            if (txtScarpBillID.Text.Trim() != "")
            {
                m_lnqStandingBook.ScarpBillID = txtScarpBillID.Text;
                m_lnqStandingBook.ScarpPersonnel = lbScarpPersonnel.Text.Trim() == "" ? BasicInfo.LoginName : lbScarpPersonnel.Text;
                m_lnqStandingBook.ScarpTime = lbScarpTime.Text.Trim() == "" ? ServerTime.Time : Convert.ToDateTime(lbScarpTime.Text);
                m_lnqStandingBook.ScarpReason = txtScarpReason.Text;

            }
            
        }

        private void btnServiceAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvRepairInfo.DataSource;

            DataRow dr = dt.NewRow();

            dr["单据号"] = txtServiceBillID.Text;
            dr["维修内容"] = txtServiceContent.Text;
            dr["人员名称"] = BasicInfo.LoginName;
            dr["时间"] = ServerTime.Time;
            dr["工装编号"] = txtFrockNumber.Text;
            dr["物品ID"] = Convert.ToInt32( txtName.Tag);

            dt.Rows.Add(dr);

            dgvRepairInfo.DataSource = dt;

            txtServiceBillID.Text = "";
            txtServiceContent.Text = "";
        }

        private void btnServiceDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dgvRepairInfo.DataSource;

            dt.Rows.RemoveAt(dgvRepairInfo.CurrentRow.Index);

            dgvRepairInfo.DataSource = dt;
        }

        private void btnServiceUpdate_Click(object sender, EventArgs e)
        {
            btnServiceDelete_Click(sender,e);
            btnServiceAdd_Click(sender,e);
        }

        private void txtParentName_Enter(object sender, EventArgs e)
        {
            txtParentName.StrEndSql = " and 物品类别名称 = '工装'";
        }

        private void txtName_Enter(object sender, EventArgs e)
        {
            txtName.StrEndSql = " and 物品类别名称 = '工装'";
        }

        private void txtParentFrockNumber_Enter(object sender, EventArgs e)
        {
            txtParentFrockNumber.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtParentName.Tag);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckMessage())
            {
                return;
            }

            GetMessage();

            if (!m_serverFrockStandingBook.UpdateFrockStandingBook(m_lnqStandingBook,(DataTable)dgvRepairInfo.DataSource,out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                List<string> listResult = new List<string>();

                if (dgvCheckOutItems.DataSource != null && dgvCheckOutItems.Rows.Count != 0)
                {
                    listResult = ((DataTable)dgvCheckOutItems.DataSource).AsEnumerable().Select(r => r.Field<string>("检测项目")).ToList();
                }

                if (!m_serverFrockStandingBook.SaveCheckItemContent(m_lnqStandingBook.FrockNumber, listResult, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }

                if (m_lnqStandingBook.ApplyToWorkShop == "机加")
                {
                    m_serverFrockStandingBook.SaveApplicableGoods(m_lnqStandingBook.FrockNumber, _ApplicableGoods);
                }

                MessageDialog.ShowPromptMessage("保存成功");
                m_strFrockName = txtName.Text + "(" +  m_lnqStandingBook.FrockNumber + ")";
                m_strFrockNumber = m_lnqStandingBook.GoodsID.ToString() + "-" + m_lnqStandingBook.FrockNumber;
                m_blSave = true;
                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            chkIsShowDispensing.Visible = dgvDispensingInfo.Visible;
        }

        private void dgvFrockReport_DoubleClick(object sender, EventArgs e)
        {
            if (dgvFrockReport.CurrentRow == null)
            {
                return;
            }

            工装验证报告 form = new 工装验证报告(dgvFrockReport.CurrentRow.Cells["单据号"].Value.ToString());

            form.ShowDialog();
        }

        private void dgvDispensingInfo_DoubleClick(object sender, EventArgs e)
        {
            if (dgvDispensingInfo.Rows.Count == 0)
            {
                return;
            }

            工装总成信息 form = new 工装总成信息(Convert.ToInt32(dgvDispensingInfo.CurrentRow.Cells["物品ID"].Value),
                dgvDispensingInfo.CurrentRow.Cells["工装编号"].Value.ToString(), false, m_strAuthFlag, false);

            form.ShowDialog();

            dgvDispensingInfo.DataSource = m_serverFrockStandingBook.GetSplitCharging(
                Convert.ToInt32(txtName.Tag), txtFrockNumber.Text, chkIsShowDispensing.Checked);
        }

        private void dgvRepairInfo_Click(object sender, EventArgs e)
        {
            if (dgvRepairInfo.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtServiceBillID.Text = dgvRepairInfo.CurrentRow.Cells["单据号"].Value.ToString();
                txtServiceContent.Text = dgvRepairInfo.CurrentRow.Cells["维修内容"].Value.ToString();
            }
        }

        private void chkIsShowDispensing_CheckedChanged(object sender, EventArgs e)
        {
            dgvDispensingInfo.DataSource = m_serverFrockStandingBook.GetSplitCharging(Convert.ToInt32(txtName.Tag),
                txtFrockNumber.Text, chkIsShowDispensing.Checked);
        }

        private void btnDeleteItem_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dgvCheckOutItems.DataSource;

            dtTemp.Rows.Remove(dtTemp.Select("检测项目 = '" + dgvCheckOutItems.CurrentRow.Cells["检测项目"].Value.ToString() + "'")[0]);

            //dgvCheckOutItems.Rows.Remove(dgvCheckOutItems.SelectedRows[0]);
        }

        private void btnAddItem_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dgvCheckOutItems.DataSource;

            DataRow dr = dtTemp.NewRow();

            dr["检测项目"] = txtItemContent.Text;

            dtTemp.Rows.Add(dr);

            dgvCheckOutItems.DataSource = dtTemp;

            txtItemContent.Text = "";
        }

        private void rbTypeCount_CheckedChanged(object sender, EventArgs e)
        {
            if (rbTypeCount.Checked)
            {
                rbTypeTime.Checked = false;
                label20.Text = "次";
            }
            else
            {
                rbTypeTime.Checked = true;
                label20.Text = "月";
            }
        }

        private void btnReferenceInfo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetFrockStandingBook(txtName.Tag == null ? 0 : Convert.ToInt32(txtName.Tag));

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                S_FrockStandingBook lnqTemp = 
                    m_serverFrockStandingBook.GetBookInfo(m_lnqStandingBook == null ? 
                    Convert.ToInt32( form.GetDataItem("物品ID")) : m_lnqStandingBook.GoodsID, 
                    form.GetDataItem("工装编号").ToString());

                View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(lnqTemp.GoodsID));
                View_F_GoodsPlanCost tempParentGoodsLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(lnqTemp.ParentGoodsID));

                if (tempParentGoodsLnq != null)
                {
                    txtParentCode.Text = tempParentGoodsLnq.图号型号;
                    txtParentName.Text = tempParentGoodsLnq.物品名称;
                    txtParentName.Tag = Convert.ToInt32(m_lnqStandingBook.ParentGoodsID);
                    txtParentFrockNumber.Text = m_lnqStandingBook.ParentFrockNumber;

                }

                if (tempGoodsLnq == null)
                {
                    MessageDialog.ShowPromptMessage("系统中无此物品信息");
                }

                txtCode.Text = tempGoodsLnq.图号型号;
                txtName.Text = tempGoodsLnq.物品名称;
                txtName.Tag = Convert.ToInt32(lnqTemp.GoodsID);

                numIdentifyCycle.Value = Convert.ToDecimal(lnqTemp.IdentifyCycle);
                txtApplyToDevice.Text = lnqTemp.ApplyToDevice;
                txtApplyToProcess.Text = lnqTemp.ApplyToProcess;
                txtApplyToProductCode.Text = lnqTemp.ApplyToProductCode;
                txtApplyToProductName.Text = lnqTemp.ApplyToProductName;
                txtDesigner.Text = lnqTemp.Designer;
                cmbApplyToWorkShop.Text = lnqTemp.ApplyToWorkShop;

                if (lnqTemp.IdentifyCycleType != null)
                {
                    rbTypeCount.Checked = (bool)(lnqTemp.IdentifyCycleType == rbTypeCount.Text);
                    rbTypeTime.Checked = (bool)(lnqTemp.IdentifyCycleType == rbTypeTime.Text);
                }
            }
        }

        private void cmbApplyToWorkShop_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbApplyToWorkShop.Text == "机加")
            {
                btnSel.Visible = true;
                txtApplyToProductCode.ReadOnly = true;
                txtApplyToProductName.ReadOnly = true;
            }
            else
            {
                btnSel.Visible = false;
                txtApplyToProductCode.ReadOnly = false;
                txtApplyToProductName.ReadOnly = false;
            }
        }

        private void btnSel_Click(object sender, EventArgs e)
        {
            Dictionary<CE_GoodsAttributeName, string> dic = new Dictionary<CE_GoodsAttributeName, string>();
            dic.Add(CE_GoodsAttributeName.自制件, "True");
            IEnumerable<F_GoodsPlanCost> iEnum = UniversalFunction.GetGoodsInfoList_Attribute(dic);

            DepotManagementDataContext ctx = CommentParameter.DepotDataContext;

            IQueryable iq = (from a in ctx.View_F_GoodsPlanCost
                             where iEnum.Select(k =>k.ID).ToList().Contains(a.序号)
                             select a).AsQueryable<View_F_GoodsPlanCost>();

            DataTable tempTable = GlobalObject.GeneralFunction.ConvertToDataTable(iq);

            List<string> lstKeys = new List<string>();

            lstKeys.Add("序号");

            FormDataTableCheck frm = new FormDataTableCheck(tempTable, _ApplicableGoods, lstKeys);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                _ApplicableGoods = frm._DtResult;

                txtApplyToProductCode.Text = "";
                txtApplyToProductName.Text = "";

                foreach (DataRow dr in _ApplicableGoods.Rows)
                {
                    txtApplyToProductCode.Text += dr["图号型号"].ToString() + "、";
                    txtApplyToProductName.Text += dr["物品名称"].ToString() + "、";
                }

                txtApplyToProductCode.Text = txtApplyToProductCode.Text.Substring(0, txtApplyToProductCode.Text.Length - 1);
                txtApplyToProductName.Text = txtApplyToProductName.Text.Substring(0, txtApplyToProductName.Text.Length - 1);
            }
        }
    }
}
