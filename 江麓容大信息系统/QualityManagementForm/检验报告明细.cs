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
using Service_Quality_QC;
using FlowControlService;

namespace Form_Quality_QC
{
    public partial class 检验报告明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_InspectionJudge_InspectionReport m_lnqBillInfo = new Business_InspectionJudge_InspectionReport();

        public Business_InspectionJudge_InspectionReport LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_InspectionJudge_InspectionReport_Item> m_listViewDetail = new List<View_Business_InspectionJudge_InspectionReport_Item>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IInspectionReportService m_serviceInspectionReport = Service_Quality_QC.ServerModuleFactory.GetServerModule<IInspectionReportService>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 检验报告明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.检验报告.ToString(), m_serviceInspectionReport);
                m_lnqBillInfo = m_serviceInspectionReport.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
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
                txtInspectionReportNo.Text = m_lnqBillInfo.InspectionReportNo;
                txtBillRelate.Text = m_lnqBillInfo.BillRelate;

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(m_lnqBillInfo.GoodsID);
                txtGoodsCode.Text = goodsInfo.图号型号;
                txtGoodsName.Text = goodsInfo.物品名称;
                txtSpec.Text = goodsInfo.规格;
                txtGoodsCode.Tag = goodsInfo.序号;
                lbUnit.Text = goodsInfo.单位;

                txtBatchNo.Text = m_lnqBillInfo.BatchNo;
                txtChecker.Tag = m_lnqBillInfo.Checker;
                txtChecker.Text = UniversalFunction.GetPersonnelName(m_lnqBillInfo.Checker);

                txtInspectionExplain.Text = m_lnqBillInfo.InspectionExplain;
                txtProvider.Text = m_lnqBillInfo.Provider;
                cmbGoodsImportance.Text = m_lnqBillInfo.GoodsImportance;
                cmbGoodsType1.Text = m_lnqBillInfo.GoodsType1;
                cmbGoodsType2.Text = m_lnqBillInfo.GoodsType2;
                numGoodsCount.Value = m_lnqBillInfo.GoodsCount;
                dtpCheckDate.Value = m_lnqBillInfo.CheckDate;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_InspectionJudge_InspectionReport();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }

            m_listViewDetail = m_serviceInspectionReport.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail);
        }

        void RefreshDataGridView(List<View_Business_InspectionJudge_InspectionReport_Item> source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_InspectionJudge_InspectionReport_Item item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.检验项目, item.检验结果, item.单据号 });
                }
            }
        }

        bool CheckData()
        {
            if (txtInspectionReportNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写检验报告编号");
                return false;
            }

            if (txtBillRelate.Text.Trim().Length == 0 || txtGoodsName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要检验的物品");
                return false;
            }

            if (txtChecker.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写检验员");
            }

            if (numGoodsCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("检验数量不能等于0");
            }

            //if (customDataGridView1.Rows.Count == 0)
            //{
            //    MessageDialog.ShowPromptMessage("请添加明细");
            //    return false;
            //}

            return true;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtBillRelate.Text.Trim().Length > 0)
            {
                DataGridViewRow dr = new DataGridViewRow();
                customDataGridView1.Rows.Add(dr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要检验的物品");
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in customDataGridView1.SelectedRows)
                {
                    customDataGridView1.Rows.Remove(dr);
                }
            }
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (!CheckData())
                {
                    return false;
                }

                m_lnqBillInfo = new Business_InspectionJudge_InspectionReport();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.InspectionReportNo = txtInspectionReportNo.Text;
                m_lnqBillInfo.BatchNo = txtBatchNo.Text;
                m_lnqBillInfo.BillRelate = txtBillRelate.Text;
                m_lnqBillInfo.Checker = txtChecker.Tag.ToString();
                m_lnqBillInfo.GoodsCount = numGoodsCount.Value;
                m_lnqBillInfo.GoodsID = Convert.ToInt32(txtGoodsCode.Tag);
                m_lnqBillInfo.GoodsImportance = cmbGoodsImportance.Text;
                m_lnqBillInfo.GoodsType1 = cmbGoodsType1.Text;
                m_lnqBillInfo.GoodsType2 = cmbGoodsType2.Text;
                m_lnqBillInfo.InspectionExplain = txtInspectionExplain.Text;
                m_lnqBillInfo.IsQualified = chbIsQualified.Checked;
                m_lnqBillInfo.Provider = txtProvider.Text;
                m_lnqBillInfo.CheckDate = dtpCheckDate.Value;

                List<View_Business_InspectionJudge_InspectionReport_Item> listTemp = new List<View_Business_InspectionJudge_InspectionReport_Item>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_InspectionJudge_InspectionReport_Item detailTemp = new View_Business_InspectionJudge_InspectionReport_Item();

                    detailTemp.检验项目 = dgvr.Cells["检验项目"].Value == null ? "" : dgvr.Cells["检验项目"].Value.ToString();
                    detailTemp.检验结果 = dgvr.Cells["检验结果"].Value == null ? "" : dgvr.Cells["检验结果"].Value.ToString();
                    detailTemp.单据号 = txtBillNo.Text;

                    listTemp.Add(detailTemp);
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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = new FormQueryInfo(m_serviceInspectionReport.GetReferenceInfo(chbIsRepeat.Checked));

            if (DialogResult.OK == form.ShowDialog())
            {
                txtBillRelate.Text = form.GetDataItem("单据号").ToString();
                txtProvider.Text = form.GetDataItem("供应商").ToString();
                txtBatchNo.Text = form.GetDataItem("批次号").ToString();
                numGoodsCount.Value = Convert.ToDecimal(form.GetDataItem("数量"));

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo((int)form.GetDataItem("物品ID"));

                txtGoodsCode.Tag = goodsInfo.序号;
                txtGoodsCode.Text = goodsInfo.图号型号;
                txtGoodsName.Text = goodsInfo.物品名称;
                txtSpec.Text = goodsInfo.规格;
                lbUnit.Text = goodsInfo.单位;
            }
        }

        private void txtChecker_Enter(object sender, EventArgs e)
        {
            txtChecker.StrEndSql = " and Dept like 'ZK%'";
        }

        private void txtChecker_OnCompleteSearch()
        {
            txtChecker.Text = txtChecker.DataResult["姓名"].ToString();
            txtChecker.Tag = txtChecker.DataResult["工号"].ToString();
        }

        private void txtGoodsCode_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodsCode.DataResult["图号型号"].ToString();
            txtGoodsName.Text = txtGoodsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtGoodsCode.DataResult["规格"].ToString();
            txtGoodsCode.Tag = Convert.ToInt32( txtGoodsCode.DataResult["序号"]);

        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtProvider.Text = txtBatchNo.DataResult["供货单位"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32( txtGoodsCode.Tag);
        }

        private void txtBillRelate_TextChanged(object sender, EventArgs e)
        {
            if (txtBillRelate.Text.Trim().Length == 0)
            {
                txtGoodsCode.ShowResultForm = true;
                txtBatchNo.ShowResultForm = true;
            }
            else
            {
                txtGoodsCode.ShowResultForm = false;
                txtBatchNo.ShowResultForm = false;
            }
        }
    }
}
