using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;
using Service_Economic_Purchase;
using FlowControlService;

namespace Form_Economic_Purchase
{
    public partial class 供应商与零件归属变更单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_PurchasingMG_PartsBelongPriovderChange m_lnqBillInfo = new Business_PurchasingMG_PartsBelongPriovderChange();

        public Business_PurchasingMG_PartsBelongPriovderChange LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> m_listViewDetail = new List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IPartsBelongProviderChangeService m_serviceChangeBill = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IPartsBelongProviderChangeService>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 供应商与零件归属变更单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.供应商与零件归属变更单.ToString(), m_serviceChangeBill);
                m_lnqBillInfo = m_serviceChangeBill.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();

                if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString())
                {
                    groupBox1.Enabled = false;
                    groupBox2.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtProvider.Text = m_lnqBillInfo.Provider;
                txtChangeReason.Text = m_lnqBillInfo.ChangeReason;
                cmbChangeType.Text = m_lnqBillInfo.ChangeType;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_PurchasingMG_PartsBelongPriovderChange();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }

            m_listViewDetail = m_serviceChangeBill.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail);
        }

        void RefreshDataGridView(List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> source)
        {
            if (source != null)
            {
                customDataGridView1.DataSource = new BindingCollection<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail>(source);
            }
        }

        bool CheckDateDetail()
        {
            if (txtPartType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【零件类别】");
                return false;
            }

            if (cmbProviderLV.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【供应商等级】");
                return false;
            }

            if (txtGoodsCode.Tag == null || txtGoodsCode.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【物品】");
                return false;
            }

            if (cmbDiffcultyLV.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【难度等级】");
                return false;
            }

            //if (txtExplain.Text.Trim().Length == 0)
            //{
            //    MessageDialog.ShowPromptMessage("请填写【说明】");
            //    return false;
            //}

            return true;
        }

        bool CheckData()
        {
            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("未添加任何物品信息");
                return false;
            }

            if (txtChangeReason.Text == null || txtChangeReason.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【变更原因】");
                return false;
            }

            if (cmbChangeType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【变更类型】");
                return false;
            }

            if (txtProvider.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【供应商】");
                return false;
            }

            return true;
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (!CheckData())
                {
                    return false;
                }

                m_lnqBillInfo = new Business_PurchasingMG_PartsBelongPriovderChange();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.ChangeReason = txtChangeReason.Text;
                m_lnqBillInfo.ChangeType = cmbChangeType.Text;
                m_lnqBillInfo.Provider = txtProvider.Text;

                List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> listTemp = 
                    new List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail>();

                foreach (DataGridViewRow dr in customDataGridView1.Rows)
                {
                    View_Business_PurchasingMG_PartsBelongPriovderChangeDetail tempInfo = 
                        new View_Business_PurchasingMG_PartsBelongPriovderChangeDetail();

                    tempInfo.单据号 = dr.Cells["单据号"].Value.ToString();
                    
                    tempInfo.规格 = dr.Cells["规格"].Value.ToString();
                    tempInfo.零件类型 = dr.Cells["零件类型"].Value.ToString();
                    tempInfo.供应商等级 = dr.Cells["供应商等级"].Value.ToString();
                    tempInfo.难度等级 = dr.Cells["难度等级"].Value.ToString();
                    tempInfo.说明 = dr.Cells["说明"].Value.ToString();
                    tempInfo.图号型号 = dr.Cells["图号型号"].Value.ToString();
                    tempInfo.物品ID = Convert.ToInt32( dr.Cells["物品ID"].Value);
                    tempInfo.物品名称 = dr.Cells["物品名称"].Value.ToString();

                    listTemp.Add(tempInfo);
                }

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.ResultInfo = listTemp;

                this.ResultList = new List<object>();

                this.ResultList.Add(m_lnqBillInfo);
                this.ResultList.Add(flowOperationType);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckDateDetail())
            {
                return;
            }

            List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> listTemp =
                (customDataGridView1.DataSource as BindingCollection<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail>).ToList();

            View_Business_PurchasingMG_PartsBelongPriovderChangeDetail tempInfo = 
                new View_Business_PurchasingMG_PartsBelongPriovderChangeDetail();

            tempInfo.单据号 = txtBillNo.Text;
            tempInfo.供应商等级 = cmbProviderLV.Text;
            tempInfo.规格 = txtSpec.Text;
            tempInfo.零件类型 = txtPartType.Text;
            tempInfo.难度等级 = cmbDiffcultyLV.Text;
            tempInfo.说明 = txtExplain.Text;
            tempInfo.图号型号 = txtGoodsCode.Text;
            tempInfo.物品ID = Convert.ToInt32( txtGoodsCode.Tag);
            tempInfo.物品名称 = txtGoodsName.Text;

            listTemp.Add(tempInfo);
            RefreshDataGridView(listTemp);
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (!CheckDateDetail())
            {
                return;
            }
            
            customDataGridView1.CurrentRow.Cells["供应商等级"].Value = cmbProviderLV.Text;
            customDataGridView1.CurrentRow.Cells["规格"].Value = txtSpec.Text;
            customDataGridView1.CurrentRow.Cells["零件类型"].Value = txtPartType.Text;
            customDataGridView1.CurrentRow.Cells["难度等级"].Value = cmbDiffcultyLV.Text;
            customDataGridView1.CurrentRow.Cells["说明"].Value = txtExplain.Text;
            customDataGridView1.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;
            customDataGridView1.CurrentRow.Cells["物品ID"].Value = Convert.ToInt32(txtGoodsCode.Tag);
            customDataGridView1.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in customDataGridView1.SelectedRows)
                {
                    customDataGridView1.Rows.Remove(dr);
                }
            }
        }

        private void cmbChangeType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbChangeType.Text == "淘汰")
            {
                txtGoodsCode.StrEndSql = "  and 序号 in (select GoodsID from B_AccessoryDutyInfo)";

                cmbProviderLV.Enabled = false;
                txtPartType.Enabled = false;
                cmbDiffcultyLV.Enabled = false;
            }
            else
            {
                cmbProviderLV.Enabled = true;
                txtPartType.Enabled = true;
                cmbDiffcultyLV.Enabled = true;
            }
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            if (txtGoodsCode.DataResult != null)
            {
                txtGoodsCode.Tag = Convert.ToInt32(txtGoodsCode.DataResult["序号"]);
                txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
                txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();

                B_AccessoryDutyInfo lnqTemp = m_serviceChangeBill.GetDutyInfo(Convert.ToInt32(txtGoodsCode.Tag));

                if (lnqTemp != null)
                {
                    txtExplain.Text = lnqTemp.Remark;
                    cmbProviderLV.Text = m_serviceChangeBill.GetProviderLV(Convert.ToInt32(txtGoodsCode.Tag), txtProvider.Text);
                    txtPartType.Text = lnqTemp.Sort;
                    cmbDiffcultyLV.Text = lnqTemp.Grade;
                }
            }
            else
            {
                txtGoodsCode.Tag = null;
                txtGoodsName.Text = "";
                txtSpec.Text = "";
            }
        }

        private void customDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            txtExplain.Text = customDataGridView1.CurrentRow.Cells["说明"].Value.ToString();
            txtGoodsCode.Text = customDataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsName.Text = customDataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtGoodsCode.Tag = customDataGridView1.CurrentRow.Cells["物品ID"].Value.ToString();
            txtPartType.Text = customDataGridView1.CurrentRow.Cells["零件类型"].Value.ToString();
            txtSpec.Text = customDataGridView1.CurrentRow.Cells["规格"].Value.ToString();

            cmbProviderLV.Text = customDataGridView1.CurrentRow.Cells["供应商等级"].Value.ToString();
            cmbDiffcultyLV.Text = customDataGridView1.CurrentRow.Cells["难度等级"].Value.ToString();
        }

        private void txtGoodsCode_Enter(object sender, EventArgs e)
        {
            txtGoodsCode.StrEndSql += " and 序号 in (select ID from F_GoodsPlanCost where IsDisable = 0)";
        }
    }
}
