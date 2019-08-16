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
    public partial class 判定报告明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_InspectionJudge_JudgeReport m_lnqBillInfo = new Business_InspectionJudge_JudgeReport();

        public Business_InspectionJudge_JudgeReport LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_InspectionJudge_JudgeReport_Item> m_listViewItem = new List<View_Business_InspectionJudge_JudgeReport_Item>();

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_InspectionJudge_JudgeReportDetail> m_listViewDetail = new List<View_Business_InspectionJudge_JudgeReportDetail>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IJudgeReport m_serviceJudgeReport = Service_Quality_QC.ServerModuleFactory.GetServerModule<IJudgeReport>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 判定报告明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.判定报告.ToString(), m_serviceJudgeReport);
                m_lnqBillInfo = m_serviceJudgeReport.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();

                switch (lbBillStatus.Text)
                {
                    case "新建单据":
                        groupBox2.Enabled = false;
                        break;
                    case "等待最终判定":
                        groupBox1.Enabled = false;
                        break;
                    case "单据完成":
                        groupBox1.Enabled = false;
                        groupBox2.Enabled = false;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetRadioButton_Judge(string judge)
        {
            if (judge == null || judge.Trim().Length == 0)
            {
                return;
            }

            foreach (Control item in groupBox1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Text == judge)
                    {
                        ((RadioButton)item).Checked = true;
                    }
                }
            }
        }

        void SetRadioButton_FinalJudge(string judge)
        {
            if (judge == null || judge.Trim().Length == 0)
            {
                return;
            }

            foreach (Control item in groupBox2.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Text == judge)
                    {
                        ((RadioButton)item).Checked = true;
                    }
                }
            }
        }

        string GetRadioButton_Judge()
        {
            string result = null;

            foreach (Control item in groupBox1.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        result = ((RadioButton)item).Text;
                    }
                }
            }

            return result;
        }

        string GetRadioButton_FinalJudge()
        {
            string result = null;

            foreach (Control item in groupBox2.Controls)
            {
                if (item is RadioButton)
                {
                    if (((RadioButton)item).Checked)
                    {
                        result = ((RadioButton)item).Text;
                    }
                }
            }

            return result;
        }

        void SetInfo()
        {
            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                SetRadioButton_Judge(m_lnqBillInfo.Judge);
                SetRadioButton_FinalJudge(m_lnqBillInfo.FinalJudge);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtFinalJudgeExplain.Text = m_lnqBillInfo.FinalJudgeExplain;
                txtJudgeExplain.Text = m_lnqBillInfo.JudgeExplain;
                txtJudgeReportNo.Text = m_lnqBillInfo.JudgeReportNo;

                chbIsFinalJudge.Checked = m_lnqBillInfo.IsFinalJudge;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_InspectionJudge_JudgeReport();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }

            m_listViewDetail = m_serviceJudgeReport.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            m_listViewItem = m_serviceJudgeReport.GetListViewItemInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail, m_listViewItem);
        }

        void RefreshDataGridView(List<View_Business_InspectionJudge_JudgeReportDetail> detailSource, 
            List<View_Business_InspectionJudge_JudgeReport_Item> itemSource)
        {
            if (detailSource != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_InspectionJudge_JudgeReportDetail item in detailSource)
                {
                    customDataGridView1.Rows.Add(new object[] { item.关联业务, item.图号型号, item.物品名称, item.规格, 
                        item.批次号, item.供应商, item.数量, item.单位, item.备注, item.物品ID, item.单据号 });
                }
            }

            if (itemSource != null)
            {
                customDataGridView2.Rows.Clear();
                foreach (View_Business_InspectionJudge_JudgeReport_Item item in itemSource)
                {
                    customDataGridView2.Rows.Add(new object[] { item.判定项目, item.判定结果, item.单据号 });
                }
            }
        }

        bool CheckData()
        {
            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("判定物品不能为空,请【引用】需要判定的业务");
                return false;
            }

            if ((lbBillStatus.Text == "新建单据" && m_lnqBillInfo.Judge == null)
                || (lbBillStatus.Text == "等待最终判定" && m_lnqBillInfo.FinalJudge == null))
            {
                MessageDialog.ShowPromptMessage("请选择判定结果");
                return false;
            }

            return true;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            customDataGridView2.Rows.Add(dr);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customDataGridView2.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in customDataGridView2.SelectedRows)
                {
                    customDataGridView2.Rows.Remove(dr);
                }
            }
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {

                m_lnqBillInfo = new Business_InspectionJudge_JudgeReport();
                m_lnqBillInfo.Judge = GetRadioButton_Judge();
                m_lnqBillInfo.FinalJudge = GetRadioButton_FinalJudge();

                if (!CheckData())
                {
                    return false;
                }

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.IsFinalJudge = chbIsFinalJudge.Checked;
                m_lnqBillInfo.JudgeExplain = txtJudgeExplain.Text;
                m_lnqBillInfo.JudgeReportNo = txtJudgeReportNo.Text;
                m_lnqBillInfo.FinalJudgeExplain = txtFinalJudgeExplain.Text;

                List<View_Business_InspectionJudge_JudgeReport_Item> listItemTemp = new List<View_Business_InspectionJudge_JudgeReport_Item>();

                foreach (DataGridViewRow dgvr in customDataGridView2.Rows)
                {
                    View_Business_InspectionJudge_JudgeReport_Item itemTemp = new View_Business_InspectionJudge_JudgeReport_Item();

                    itemTemp.判定项目 = dgvr.Cells["判定项目"].Value == null ? "" : dgvr.Cells["判定项目"].Value.ToString();
                    itemTemp.判定结果 = dgvr.Cells["判定结果"].Value == null ? "" : dgvr.Cells["判定结果"].Value.ToString();
                    itemTemp.单据号 = txtBillNo.Text;

                    listItemTemp.Add(itemTemp);
                }

                List<View_Business_InspectionJudge_JudgeReportDetail> listDetailTemp = new List<View_Business_InspectionJudge_JudgeReportDetail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_InspectionJudge_JudgeReportDetail detailTemp = new View_Business_InspectionJudge_JudgeReportDetail();

                    detailTemp.备注 = dgvr.Cells["备注"].Value == null ? "" : dgvr.Cells["备注"].Value.ToString();
                    detailTemp.单位 = dgvr.Cells["单位"].Value == null ? "" : dgvr.Cells["单位"].Value.ToString();
                    detailTemp.供应商 = dgvr.Cells["供应商"].Value == null ? "" : dgvr.Cells["供应商"].Value.ToString();
                    detailTemp.关联业务 = dgvr.Cells["关联业务"].Value == null ? "" : dgvr.Cells["关联业务"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.批次号 = dgvr.Cells["批次号"].Value == null ? "" : dgvr.Cells["批次号"].Value.ToString();
                    detailTemp.数量 = dgvr.Cells["数量"].Value == null ? 0 : Convert.ToDecimal( dgvr.Cells["数量"].Value);
                    detailTemp.图号型号 = dgvr.Cells["图号型号"].Value == null ? "" : dgvr.Cells["图号型号"].Value.ToString();
                    detailTemp.物品ID = dgvr.Cells["物品ID"].Value == null ? 0 : Convert.ToInt32( dgvr.Cells["物品ID"].Value);
                    detailTemp.物品名称 = dgvr.Cells["物品名称"].Value == null ? "" : dgvr.Cells["物品名称"].Value.ToString();
                    detailTemp.单据号 = txtBillNo.Text;

                    listDetailTemp.Add(detailTemp);
                }

                this.FlowInfo_BillNo = txtBillNo.Text;

                this.ResultList = new List<object>();

                this.ResultList.Add(m_lnqBillInfo);
                this.ResultList.Add(flowOperationType);
                this.ResultList.Add(listDetailTemp);
                this.ResultList.Add(listItemTemp);

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
            FormQueryInfo form = new FormQueryInfo(m_serviceJudgeReport.GetReferenceInfo(chbIsRepeat.Checked));

            if (DialogResult.OK == form.ShowDialog())
            {
                string billNo = form.GetDataItem("业务编号").ToString();
                List<View_Business_InspectionJudge_JudgeReportDetail> detailSource = 
                    new List<View_Business_InspectionJudge_JudgeReportDetail>();

                switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_BillTypeEnum>(form.GetDataItem("业务类别名称").ToString()))
                {
                    case CE_BillTypeEnum.入库申请单:
                        detailSource = m_serviceJudgeReport.GetJudgeReportDetail<View_Business_WarehouseInPut_RequisitionDetail>(billNo, txtBillNo.Text,
                           new BaseModule_Manufacture.CommonClass().GetListViewDetailInfo_Requisition(billNo));
                        break;
                    case CE_BillTypeEnum.到货单:
                        detailSource = m_serviceJudgeReport.GetJudgeReportDetail<View_Business_WarehouseInPut_AOGDetail>(billNo, txtBillNo.Text,
                            new BaseModule_Manufacture.CommonClass().GetListViewDetailInfo_AOG(billNo));
                        break;
                    case CE_BillTypeEnum.检验报告:
                        IInspectionReportService inspectionReportService =
                            Service_Quality_QC.ServerModuleFactory.GetServerModule<IInspectionReportService>();
                        List<Business_InspectionJudge_InspectionReport> tempList = new List<Business_InspectionJudge_InspectionReport>();
                        tempList.Add(inspectionReportService.GetSingleBillInfo(billNo));
                        detailSource = m_serviceJudgeReport.GetJudgeReportDetail<Business_InspectionJudge_InspectionReport>(billNo, txtBillNo.Text, tempList);
                        break;
                    default:
                        return;
                }

                if (detailSource != null)
                {
                    foreach (View_Business_InspectionJudge_JudgeReportDetail item in detailSource)
                    {
                        customDataGridView1.Rows.Add(new object[] { item.关联业务, item.图号型号, item.物品名称, item.规格, 
                        item.批次号, item.供应商, item.数量, item.单位, item.备注, item.物品ID, item.单据号 });
                    }
                }
            }
        }
    }
}
