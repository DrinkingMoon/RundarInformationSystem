using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Project_Project;
using FlowControlService;

namespace Form_Project_Project
{
    public partial class 样品确认申请单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_Sample_ConfirmTheApplication m_lnqBillInfo = new Business_Sample_ConfirmTheApplication();

        public Business_Sample_ConfirmTheApplication LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 服务组件
        /// </summary>
        ISampleApplication m_serviceSample = Service_Project_Project.ServerModuleFactory.GetServerModule<ISampleApplication>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 当前流程节点信息
        /// </summary>
        Flow_FlowInfo m_lnqFlowInfo = null;

        public 样品确认申请单明细()
        {
            InitializeComponent();
        }

        public void InitForm()
        {
            #region RadioButton
            rb_ChargeResult_ResultAffrim_DiposeSuggestion_Surplus_BFTH.Checked = false;
            rb_ChargeResult_ResultAffrim_DiposeSuggestion_Surplus_HG.Checked = false;
            rb_ChargeResult_ResultAffrim_GZ.Checked = false;
            rb_ChargeResult_ResultAffrim_InGZ.Checked = false;
            rb_ChargeResult_ResultAffrim_InPPAP.Checked = false;
            rb_ChargeResult_ResultAffrim_PPAP.Checked = false;
            rb_ChargeResult_ResultAffrim_SG.Checked = false;
            rb_ChargeResult_SampleVerify_No.Checked = false;
            rb_ChargeResult_SampleVerify_Yes.Checked = false;
            rb_Inspect_Result_No.Checked = false;
            rb_Inspect_Result_Yes.Checked = false;
            rb_Inspect_SupplierReport_IsExist_No.Checked = false;
            rb_Inspect_SupplierReport_IsExist_Yes.Checked = false;
            rb_QC_PPAPDispose_PL.Checked = false;
            rb_QC_PPAPDispose_PPAP.Checked = false;
            rb_Review_RectificationRequest_IsExist_No.Checked = false;
            rb_Review_RectificationRequest_IsExist_Yes.Checked = false;
            rb_SQE_ProvidorFeedbackBill_IsExist_No.Checked = false;
            rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked = false;
            rb_Store_IsProperPacking_No.Checked = false;
            rb_Store_IsProperPacking_Yes.Checked = false;
            rb_TestResult_Experiment_Result_No.Checked = false;
            rb_TestResult_Experiment_Result_Yes.Checked = false;
            rb_TestResult_ResultAffrim_DiposeSuggestion_Surplus_BFTH.Checked = false;
            rb_TestResult_ResultAffrim_DiposeSuggestion_Surplus_HG.Checked = false;
            rb_TestResult_ResultAffrim_GZ.Checked = false;
            rb_TestResult_ResultAffrim_InGZ.Checked = false;
            rb_TestResult_ResultAffrim_InPPAP.Checked = false;
            rb_TestResult_ResultAffrim_PPAP.Checked = false;
            rb_TestResult_ResultAffrim_SG.Checked = false;
            rb_TestResult_TrialAssembly_Explain_All.Checked = false;
            rb_TestResult_TrialAssembly_Explain_Part.Checked = false;
            rb_TestResult_TrialAssembly_Result_No.Checked = false;
            rb_TestResult_TrialAssembly_Result_Yes.Checked = false;
            #endregion

            #region CheckBox
            chb_ChargeResult_ResultAffrim_PPAP.Checked = false;
            chb_Purchase_BillType_LX.Checked = false;
            chb_Purchase_BillType_MP.Checked = false;
            chb_Purchase_BillType_WW.Checked = false;
            chb_Purchase_BillType_YCL.Checked = false;
            chb_Purchase_IsPay.Checked = false;
            chb_TestResult_ResultAffrim_PPAP.Checked = false;
            #endregion

            #region Num
            num_Finance_RawMaterialCost.Value = 0;
            num_Inspect_GoodsCount_Scarp.Value = 0;
            num_Purchase_GoodsCount_Send.Value = 0;
            num_Purchase_SendSampleTime.Value = 0;
            num_Review_InspectResult_ReWork_DisqualificationCount.Value = 0;
            num_Review_InspectResult_ReWork_QualificationCount.Value = 0;
            num_SQE_SampleDisposeType_DisqualificationCount.Value = 0;
            num_SQE_SampleDisposeType_QualificationCount.Value = 0;
            num_Store_GoodsCount_AOG.Value = 0;
            num_TestResult_TrialAssembly_Explain_PartCount.Value = 0;
            #endregion

            #region TextBox
            txt_ChargeResult_ResultAffrim_DiposeSuggestion_Bined.Text = "";
            txt_Inspect_IrrItemExplain.Text = "";
            txt_Inspect_Packing_IrrCause.Text = "";
            txt_Purchase_BatchNo.Text = "";
            txt_Purchase_Change_BillNo.Text = "";
            txt_Purchase_Change_Reason.Text = "";
            txt_Purchase_GoodsCode.Text = "";
            txt_Purchase_GoodsName.Text = "";
            txt_Purchase_OrderFormNo.Text = "";
            txt_Purchase_Provider.Text = "";
            txt_Purchase_ProviderBatchNo.Text = "";
            txt_Purchase_Spec.Text = "";
            txt_Purchase_Version.Text = "";
            txt_QC_Suggestion.Text = "";
            txt_Review_IrrItem_Concession.Text = "";
            txt_Review_IrrItem_Rectification.Text = "";
            txt_Review_RectificationItem_Explain.Text = "";
            txt_SQE_ProvidorFeedbackBillNo.Text = "";
            txt_Store_Packing_IrrCause.Text = "";
            txt_Store_Put_Area_Affrim.Text = "";
            txt_Store_Put_AreaNo_Affrim.Text = "";
            txt_Store_Put_LayerNo_Affrim.Text = "";
            txt_TestResult_Experiment_ReportNo.Text = "";
            txt_TestResult_ExperimentCVTNo.Text = "";
            txt_TestResult_ResultAffrim_DiposeSuggestion_Bined.Text = "";
            txt_TestResult_TrialAssembly_ResultExplain.Text = "";
            #endregion

            #region ComBox
            cmb_Purchase_Change_Type.SelectedIndex = -1;
            cmb_Purchase_SampleType.SelectedIndex = -1;
            cmb_Purchase_SendSampleReason.SelectedIndex = -1;
            cmb_Purchase_StorageID.SelectedIndex = -1;
            cmb_Review_InspectResult.SelectedIndex = -1;
            #endregion
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_lnqBillInfo = m_serviceSample.GetSingleBillInfo(this.FlowInfo_BillNo);
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.样品确认申请单.ToString(), m_serviceSample);

                string error = "";

                DataTable storageTable =
                    DataSetHelper.SiftDataTable(UniversalFunction.GetStorageTb(), 
                    " StorageID in ('01','03','08','12','13','15','17')", out error);

                cmb_Purchase_StorageID.DataSource = storageTable;
                cmb_Purchase_StorageID.DisplayMember = "StorageName";
                SetInfo();
                SetControl();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        /// <summary>
        /// 设置控件
        /// </summary>
        void SetControl()
        {
            if (m_lnqFlowInfo != null && m_lnqFlowInfo.FlowID == 45)
            {
                btn_Inspect_ReportFile_Up.Enabled = true;
            }
            else
            {
                btn_Inspect_ReportFile_Up.Enabled = false;
            }

            if (lb_Inspect_ReportFile.Tag != null && lb_Inspect_ReportFile.Tag.ToString().Trim().Length > 0)
            {
                btn_Inspect_ReportFile_Down.Enabled = true;
                btn_Inspect_ReportFile_Look.Enabled = true;
            }
            else
            {
                btn_Inspect_ReportFile_Down.Enabled = false;
                btn_Inspect_ReportFile_Look.Enabled = false;
            }

            if (cmb_Purchase_SendSampleReason.Text == "量产品变更")
            {
                label14.Visible = true;
                label15.Visible = true;
                label17.Visible = true;
                cmb_Purchase_Change_Type.Visible = true;
                txt_Purchase_Change_BillNo.Visible = true;
                txt_Purchase_Change_Reason.Visible = true;

                cmb_Purchase_Change_Type.Enabled = true;
                txt_Purchase_Change_BillNo.Enabled = true;
                txt_Purchase_Change_Reason.Enabled = true;
            }
            else
            {

                label14.Visible = false;
                label15.Visible = false;
                label17.Visible = false;
                cmb_Purchase_Change_Type.Visible = false;
                txt_Purchase_Change_BillNo.Visible = false;
                txt_Purchase_Change_Reason.Visible = false;

                cmb_Purchase_Change_Type.Enabled = false;
                txt_Purchase_Change_BillNo.Enabled = false;
                txt_Purchase_Change_Reason.Enabled = false;
            }

            if (cmb_Review_InspectResult.Text == "返工/返修" && cmb_Purchase_SampleType.Text != "PPAP样件")
            {

                label66.Visible = true;
                label26.Visible = true;
                label65.Visible = true;
                num_Review_InspectResult_ReWork_DisqualificationCount.Visible = true;
                num_Review_InspectResult_ReWork_QualificationCount.Visible = true;

                num_Review_InspectResult_ReWork_DisqualificationCount.Enabled = true;
                num_Review_InspectResult_ReWork_QualificationCount.Enabled = true;
            }
            else
            {
                label66.Visible = false;
                label26.Visible = false;
                label65.Visible = false;
                num_Review_InspectResult_ReWork_DisqualificationCount.Visible = false;
                num_Review_InspectResult_ReWork_QualificationCount.Visible = false;

                num_Review_InspectResult_ReWork_DisqualificationCount.Value = 0;
                num_Review_InspectResult_ReWork_QualificationCount.Value = 0;
                num_Review_InspectResult_ReWork_DisqualificationCount.Enabled = false;
                num_Review_InspectResult_ReWork_QualificationCount.Enabled = false;
            }

            if (cmb_Review_InspectResult.Text == "返工/返修" && cmb_Purchase_SampleType.Text == "PPAP样件")
            {
                label67.Visible = true;
                label32.Visible = true;
                label33.Visible = true;
                num_SQE_SampleDisposeType_DisqualificationCount.Visible = true;
                num_SQE_SampleDisposeType_QualificationCount.Visible = true;

                num_SQE_SampleDisposeType_DisqualificationCount.Enabled = true;
                num_SQE_SampleDisposeType_QualificationCount.Enabled = true;
            }
            else
            {
                label67.Visible = false;
                label32.Visible = false;
                label33.Visible = false;
                num_SQE_SampleDisposeType_DisqualificationCount.Visible = false;
                num_SQE_SampleDisposeType_QualificationCount.Visible = false;

                num_SQE_SampleDisposeType_DisqualificationCount.Value = 0;
                num_SQE_SampleDisposeType_QualificationCount.Value = 0;
                num_SQE_SampleDisposeType_DisqualificationCount.Enabled = false;
                num_SQE_SampleDisposeType_QualificationCount.Enabled = false;
            }

            if (!cmb_Purchase_StorageID.Text.Contains("样品"))
            {
                groupBox11.Visible = false;
            }
            else
            {
                groupBox11.Visible = true;
            }

            if (!chb_Purchase_BillType_YCL.Checked)
            {
                groupBox2.Visible = false;
            }
            else
            {
                groupBox2.Visible = true;
            }

            if (chb_Purchase_BillType_MP.Checked || chb_Review_IsPassTestResult.Checked)
            {
                groupBox7.Visible = false;
            }
            else
            {
                groupBox7.Visible = true;
            }

            if (chb_Purchase_BillType_LX.Checked || cmb_Purchase_SampleType.Text.Trim() == "A样/手工样件")
            {
                groupBox6.Visible = false;
            }
            else
            {
                groupBox6.Visible = true;
            }

            if (chb_ChargeResult_ResultAffrim_PPAP.Checked)
            {
                groupBox9.Visible = true;
            }
            else
            {
                groupBox9.Visible = false;
            }
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetInfo()
        {
            m_lnqBillInfo.BillNo = txtBillNo.Text;

            #region 采购
            m_lnqBillInfo.Purchase_BatchNo = txt_Purchase_BatchNo.Text;
            m_lnqBillInfo.Purchase_BillType = "";

            foreach (Control cl in groupBox1.Controls)
            {
                if (cl is CheckBox && ((CheckBox)cl).Checked)
                {
                    m_lnqBillInfo.Purchase_BillType += ((CheckBox)cl).Text + ",";
                }
            }

            m_lnqBillInfo.Purchase_Change_BillNo = txt_Purchase_Change_BillNo.Text;
            m_lnqBillInfo.Purchase_Change_Reason = txt_Purchase_Change_Reason.Text;
            m_lnqBillInfo.Purchase_Change_Type = cmb_Purchase_Change_Type.Text;
            m_lnqBillInfo.Purchase_GoodsCount_Send = num_Purchase_GoodsCount_Send.Value;
            m_lnqBillInfo.Purchase_GoodsID = (int)txt_Purchase_GoodsCode.Tag;
            m_lnqBillInfo.Purchase_IsPay = chb_Purchase_IsPay.Checked;
            m_lnqBillInfo.Purchase_OrderFormNo = txt_Purchase_OrderFormNo.Text;
            m_lnqBillInfo.Purchase_Provider = txt_Purchase_Provider.Text;
            m_lnqBillInfo.Purchase_ProviderBatchNo = txt_Purchase_ProviderBatchNo.Text;
            m_lnqBillInfo.Purchase_SampleType = cmb_Purchase_SampleType.Text;
            m_lnqBillInfo.Purchase_SendSampleReason = cmb_Purchase_SendSampleReason.Text;
            m_lnqBillInfo.Purchase_SendSampleTime = num_Purchase_SendSampleTime.Value;
            m_lnqBillInfo.Purchase_StorageID = UniversalFunction.GetStorageID(cmb_Purchase_StorageID.Text);
            m_lnqBillInfo.Purchase_Version = txt_Purchase_Version.Text;

            #endregion

            #region 库房
            m_lnqBillInfo.Store_GoodsCount_AOG = num_Store_GoodsCount_AOG.Value;
            m_lnqBillInfo.Store_GoodsCount_InDepot = txt_Store_GoodsCount_InDepot.Text.ToString() == "" ? null :
                (decimal?)Convert.ToDecimal(txt_Store_GoodsCount_InDepot.Text.ToString());

            if (rb_Store_IsProperPacking_Yes.Checked)
            {
                m_lnqBillInfo.Store_IsProperPacking = true;
            }
            else if (rb_Store_IsProperPacking_No.Checked)
            {
                m_lnqBillInfo.Store_IsProperPacking = false;
            }

            m_lnqBillInfo.Store_Packing_IrrCause = txt_Store_Packing_IrrCause.Text;
            m_lnqBillInfo.Store_Put_Area = txt_Store_Put_Area_Affrim.Text;
            m_lnqBillInfo.Store_Put_AreaNo = txt_Store_Put_AreaNo_Affrim.Text;
            m_lnqBillInfo.Store_Put_LayerNo = txt_Store_Put_LayerNo_Affrim.Text;

            #endregion

            //财务
            m_lnqBillInfo.Finance_RawMaterialCost = num_Finance_RawMaterialCost.Value;

            #region 检验
            m_lnqBillInfo.Inspect_GoodsCount_Scarp = num_Inspect_GoodsCount_Scarp.Value;
            m_lnqBillInfo.Inspect_IrrItemExplain = txt_Inspect_IrrItemExplain.Text;
            m_lnqBillInfo.Inspect_ReportFile = lb_Inspect_ReportFile.Tag == null ? null : lb_Inspect_ReportFile.Tag.ToString();
            m_lnqBillInfo.Inspect_ReportNo = "";//txt_Inspect_ReportNo.Text;

            if (rb_Inspect_Result_Yes.Checked)
            {
                m_lnqBillInfo.Inspect_Result = true;
            }
            else if (rb_Inspect_Result_No.Checked)
            {
                m_lnqBillInfo.Inspect_Result = false;
            }

            if (rb_Inspect_IsProperPacking_Yes.Checked)
            {
                m_lnqBillInfo.Inspect_IsProperPacking = true;
            }
            else if (rb_Inspect_IsProperPacking_No.Checked)
            {
                m_lnqBillInfo.Inspect_IsProperPacking = false;
            }

            m_lnqBillInfo.Inspect_Packing_IrrCause = txt_Inspect_Packing_IrrCause.Text;

            if (rb_Inspect_SupplierReport_IsExist_Yes.Checked)
            {
                m_lnqBillInfo.Inspect_SupplierReport_IsExist = true;
            }
            else if (rb_Inspect_SupplierReport_IsExist_No.Checked)
            {
                m_lnqBillInfo.Inspect_SupplierReport_IsExist = false;
            }

            #endregion

            #region 评审
            m_lnqBillInfo.Review_InspectResult = cmb_Review_InspectResult.Text;
            m_lnqBillInfo.Review_InspectResult_ReWork_DisqualificationCount = num_Review_InspectResult_ReWork_DisqualificationCount.Value;
            m_lnqBillInfo.Review_InspectResult_ReWork_QualificationCount = num_Review_InspectResult_ReWork_QualificationCount.Value;

            m_lnqBillInfo.Review_IrrItem_Concession = txt_Review_IrrItem_Concession.Text;
            m_lnqBillInfo.Review_IrrItem_Rectification = txt_Review_IrrItem_Rectification.Text;

            m_lnqBillInfo.Review_ItemReview = "";
            foreach (Control cl in groupBox5.Controls)
            {
                if (cl is CheckBox && ((CheckBox)cl).Checked)
                {
                    m_lnqBillInfo.Review_ItemReview += ((CheckBox)cl).Text + ",";
                }
            }

            m_lnqBillInfo.Review_RectificationItem_Explain = txt_Review_RectificationItem_Explain.Text;

            if (chb_Review_IsPassTestResult.Checked)
            {
                m_lnqBillInfo.Review_IsPassTestResult = true;
            }
            else
            {
                m_lnqBillInfo.Review_IsPassTestResult = false;
            }

            if (rb_Review_RectificationRequest_IsExist_Yes.Checked)
            {
                m_lnqBillInfo.Review_RectificationRequest_IsExist = true;
            }
            else if (rb_Review_RectificationRequest_IsExist_No.Checked)
            {
                m_lnqBillInfo.Review_RectificationRequest_IsExist = false;
            }

            #endregion

            #region SQE
            if (rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked)
            {
                m_lnqBillInfo.SQE_ProvidorFeedbackBill_IsExist = true;
            }
            else if (rb_SQE_ProvidorFeedbackBill_IsExist_No.Checked)
            {
                m_lnqBillInfo.SQE_ProvidorFeedbackBill_IsExist = false;
            }

            m_lnqBillInfo.SQE_ProvidorFeedbackBillNo = txt_SQE_ProvidorFeedbackBillNo.Text;
            m_lnqBillInfo.SQE_SampleDisposeType_DisqualificationCount = num_SQE_SampleDisposeType_DisqualificationCount.Value;
            m_lnqBillInfo.SQE_SampleDisposeType_QualificationCount = num_SQE_SampleDisposeType_QualificationCount.Value;

            #endregion 

            #region 试装试验
            m_lnqBillInfo.TestResult_Experiment_ReportNo = txt_TestResult_Experiment_ReportNo.Text;

            if (rb_TestResult_Experiment_Result_Yes.Checked)
            {
                m_lnqBillInfo.TestResult_Experiment_Result = true;
            }
            else if (rb_TestResult_Experiment_Result_No.Checked)
            {
                m_lnqBillInfo.TestResult_Experiment_Result = false;
            }

            m_lnqBillInfo.TestResult_ExperimentCVTNo = txt_TestResult_ExperimentCVTNo.Text;


            m_lnqBillInfo.TestResult_ResultAffrim = "";
            foreach (Control cl in panel5.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    m_lnqBillInfo.TestResult_ResultAffrim = ((RadioButton)cl).Text;
                    break;
                }
            }

            if (chb_TestResult_ResultAffrim_PPAP.Checked)
            {
                m_lnqBillInfo.TestResult_ResultAffrim += "," + chb_TestResult_ResultAffrim_PPAP.Text;
            }

            foreach (Control cl in panel6.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    m_lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Surplus = ((RadioButton)cl).Text;
                    break;
                }
            }

            m_lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Bined = txt_TestResult_ResultAffrim_DiposeSuggestion_Bined.Text;
            m_lnqBillInfo.TestResult_TrialAssembly_Explain_PartCount = num_TestResult_TrialAssembly_Explain_PartCount.Value;

            if (rb_TestResult_TrialAssembly_Result_Yes.Checked)
            {
                m_lnqBillInfo.TestResult_TrialAssembly_Result = true;
            }
            else if (rb_TestResult_TrialAssembly_Result_No.Checked)
            {
                m_lnqBillInfo.TestResult_TrialAssembly_Result = false;
            }

            m_lnqBillInfo.TestResult_TrialAssembly_ResultExplain = txt_TestResult_TrialAssembly_ResultExplain.Text;

            if (rb_TestResult_TrialAssembly_Explain_All.Checked)
            {
                m_lnqBillInfo.TestResult_TrialAssembly_Explain = rb_TestResult_TrialAssembly_Explain_All.Text;
            }
            else if (rb_TestResult_TrialAssembly_Explain_Part.Checked)
            {
                m_lnqBillInfo.TestResult_TrialAssembly_Explain = rb_TestResult_TrialAssembly_Explain_Part.Text;
            }
            #endregion

            #region 开发主管确认

            m_lnqBillInfo.ChargeResult_ResultAffrim = "";
            foreach (Control cl in panel11.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    m_lnqBillInfo.ChargeResult_ResultAffrim = ((RadioButton)cl).Text;
                    break;
                }
            }

            if (chb_ChargeResult_ResultAffrim_PPAP.Checked)
            {
                m_lnqBillInfo.ChargeResult_ResultAffrim = m_lnqBillInfo.ChargeResult_ResultAffrim + "," + chb_ChargeResult_ResultAffrim_PPAP.Text;
            }

            foreach (Control cl in panel10.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    m_lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Surplus = ((RadioButton)cl).Text;
                    break;
                }
            }

            m_lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Bined = txt_ChargeResult_ResultAffrim_DiposeSuggestion_Bined.Text;

            if (rb_ChargeResult_SampleVerify_Yes.Checked)
            {
                m_lnqBillInfo.ChargeResult_SampleVerify = true;
            }
            else if (rb_ChargeResult_SampleVerify_No.Checked)
            {
                m_lnqBillInfo.ChargeResult_SampleVerify = false;
            }

            #endregion

            #region QC
            if (rb_QC_PPAPDispose_PL.Checked)
            {
                m_lnqBillInfo.QC_PPAPDispose = rb_QC_PPAPDispose_PL.Text;
            }
            else if (rb_QC_PPAPDispose_PPAP.Checked)
            {
                m_lnqBillInfo.QC_PPAPDispose = rb_QC_PPAPDispose_PPAP.Text;
            }

            m_lnqBillInfo.QC_Suggestion = txt_QC_Suggestion.Text;
            #endregion
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        void SetInfo()
        {
            InitForm();

            if (m_lnqBillInfo != null)
            {
                List<string> tempList = new List<string>();
                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(m_lnqBillInfo.Purchase_GoodsID);
                m_lnqFlowInfo =
                    m_serverFlow.GetNowFlowInfo(m_serverFlow.GetBusinessTypeID(CE_BillTypeEnum.样品确认申请单, null), m_lnqBillInfo.BillNo);

                lbBillStatus.Text = m_lnqFlowInfo.BusinessStatus;
                txtBillNo.Text = m_lnqBillInfo.BillNo;

                #region 采购

                cmb_Purchase_Change_Type.Text = m_lnqBillInfo.Purchase_Change_Type;
                cmb_Purchase_SendSampleReason.Text = m_lnqBillInfo.Purchase_SendSampleReason;
                cmb_Purchase_SampleType.Text = m_lnqBillInfo.Purchase_SampleType;
                cmb_Purchase_StorageID.Text = UniversalFunction.GetStorageName(m_lnqBillInfo.Purchase_StorageID);

                if (m_lnqBillInfo.Purchase_BillType != null)
                {
                    tempList = m_lnqBillInfo.Purchase_BillType.Split(',').ToList();

                    foreach (Control cl in groupBox1.Controls)
                    {
                        if (cl is CheckBox)
                        {
                            if (tempList.Contains(((CheckBox)cl).Text))
                            {
                                ((CheckBox)cl).Checked = true;
                            }
                        }
                    }
                }

                chb_Purchase_IsPay.Checked = (bool)m_lnqBillInfo.Purchase_IsPay;

                txt_Purchase_BatchNo.Text = m_lnqBillInfo.Purchase_BatchNo;
                txt_Purchase_Change_BillNo.Text = m_lnqBillInfo.Purchase_Change_BillNo;
                txt_Purchase_Change_Reason.Text = m_lnqBillInfo.Purchase_Change_Reason;
                txt_Purchase_GoodsCode.Text = goodsInfo.图号型号;
                txt_Purchase_GoodsName.Text = goodsInfo.物品名称;
                txt_Purchase_GoodsCode.Tag = m_lnqBillInfo.Purchase_GoodsID;
                txt_Purchase_OrderFormNo.Text = m_lnqBillInfo.Purchase_OrderFormNo;
                txt_Purchase_Provider.Text = m_lnqBillInfo.Purchase_Provider;
                txt_Purchase_ProviderBatchNo.Text = m_lnqBillInfo.Purchase_ProviderBatchNo;
                txt_Purchase_Spec.Text = goodsInfo.规格;
                txt_Purchase_Version.Text = m_lnqBillInfo.Purchase_Version;

                num_Purchase_GoodsCount_Send.Value = m_lnqBillInfo.Purchase_GoodsCount_Send;
                num_Purchase_SendSampleTime.Value = m_lnqBillInfo.Purchase_SendSampleTime;

                #endregion

                #region 确认到货
                if (m_lnqBillInfo.Store_IsProperPacking != null)
                {
                    if ((bool)m_lnqBillInfo.Store_IsProperPacking)
                    {
                        rb_Store_IsProperPacking_Yes.Checked = true;
                        rb_Store_IsProperPacking_No.Checked = false;
                    }
                    else
                    {
                        rb_Store_IsProperPacking_Yes.Checked = false;
                        rb_Store_IsProperPacking_No.Checked = true;
                    }
                }

                txt_Store_Packing_IrrCause.Text = m_lnqBillInfo.Store_Packing_IrrCause;

                num_Store_GoodsCount_AOG.Value = m_lnqBillInfo.Store_GoodsCount_AOG == null ? 0 : (decimal)m_lnqBillInfo.Store_GoodsCount_AOG;
                txt_Store_Put_Area_Affrim.Text = m_lnqBillInfo.Store_Put_Area;
                txt_Store_Put_AreaNo_Affrim.Text = m_lnqBillInfo.Store_Put_AreaNo;
                txt_Store_Put_LayerNo_Affrim.Text = m_lnqBillInfo.Store_Put_LayerNo;

                #endregion

                //财务
                num_Finance_RawMaterialCost.Value = 
                    m_lnqBillInfo.Finance_RawMaterialCost == null ? 0 : (decimal)m_lnqBillInfo.Finance_RawMaterialCost;

                #region 质检
                if (m_lnqBillInfo.Inspect_Result != null)
                {
                    if ((bool)m_lnqBillInfo.Inspect_Result)
                    {
                        rb_Inspect_Result_Yes.Checked = true;
                        rb_Inspect_Result_No.Checked = false;
                    }
                    else
                    {
                        rb_Inspect_Result_Yes.Checked = false;
                        rb_Inspect_Result_No.Checked = true;
                    }
                }

                if (m_lnqBillInfo.Inspect_SupplierReport_IsExist != null)
                {
                    if ((bool)m_lnqBillInfo.Inspect_SupplierReport_IsExist)
                    {
                        rb_Inspect_SupplierReport_IsExist_Yes.Checked = true;
                        rb_Inspect_SupplierReport_IsExist_No.Checked = false;
                    }
                    else
                    {
                        rb_Inspect_SupplierReport_IsExist_Yes.Checked = false;
                        rb_Inspect_SupplierReport_IsExist_No.Checked = true;
                    }
                }

                if (m_lnqBillInfo.Inspect_IsProperPacking != null)
                {
                    if ((bool)m_lnqBillInfo.Inspect_IsProperPacking)
                    {
                        rb_Inspect_IsProperPacking_Yes.Checked = true;
                        rb_Inspect_IsProperPacking_No.Checked = false;
                    }
                    else
                    {
                        rb_Inspect_IsProperPacking_Yes.Checked = false;
                        rb_Inspect_IsProperPacking_No.Checked = true;
                    }
                }

                txt_Inspect_Packing_IrrCause.Text = m_lnqBillInfo.Inspect_Packing_IrrCause;
                txt_Inspect_IrrItemExplain.Text = m_lnqBillInfo.Inspect_IrrItemExplain;
                //txt_Inspect_ReportNo.Text = m_lnqBillInfo.Inspect_ReportNo;

                num_Inspect_GoodsCount_Scarp.Value = 
                    m_lnqBillInfo.Inspect_GoodsCount_Scarp == null ? 0 : (decimal)m_lnqBillInfo.Inspect_GoodsCount_Scarp;
                lb_Inspect_ReportFile.Tag = m_lnqBillInfo.Inspect_ReportFile;
                #endregion

                #region 评审

                if (m_lnqBillInfo.Review_IsPassTestResult != null)
                {
                    chb_Review_IsPassTestResult.Checked = Convert.ToBoolean(m_lnqBillInfo.Review_IsPassTestResult);
                }
                else
                {
                    chb_Review_IsPassTestResult.Checked = false;
                }

                txt_Review_IrrItem_Concession.Text = m_lnqBillInfo.Review_IrrItem_Concession;
                txt_Review_IrrItem_Rectification.Text = m_lnqBillInfo.Review_IrrItem_Rectification;
                txt_Review_RectificationItem_Explain.Text = m_lnqBillInfo.Review_RectificationItem_Explain;

                cmb_Review_InspectResult.Text = m_lnqBillInfo.Review_InspectResult;

                num_Review_InspectResult_ReWork_DisqualificationCount.Value =
                    m_lnqBillInfo.Review_InspectResult_ReWork_DisqualificationCount == null ? 0 : (decimal)m_lnqBillInfo.Review_InspectResult_ReWork_DisqualificationCount;
                num_Review_InspectResult_ReWork_QualificationCount.Value =
                    m_lnqBillInfo.Review_InspectResult_ReWork_QualificationCount == null ? 0 : (decimal)m_lnqBillInfo.Review_InspectResult_ReWork_QualificationCount;

                if (m_lnqBillInfo.Review_RectificationRequest_IsExist != null)
                {
                    if ((bool)m_lnqBillInfo.Review_RectificationRequest_IsExist)
                    {
                        rb_Review_RectificationRequest_IsExist_Yes.Checked = true;
                        rb_Review_RectificationRequest_IsExist_No.Checked = false;
                    }
                    else
                    {
                        rb_Review_RectificationRequest_IsExist_Yes.Checked = false;
                        rb_Review_RectificationRequest_IsExist_No.Checked = true;
                    }
                }

                if (m_lnqBillInfo.Review_ItemReview != null)
                {
                    tempList = m_lnqBillInfo.Review_ItemReview.Split(',').ToList();

                    foreach (Control cl in groupBox5.Controls)
                    {
                        if (cl is CheckBox)
                        {
                            if (tempList.Contains(((CheckBox)cl).Text))
                            {
                                ((CheckBox)cl).Checked = true;
                            }
                        }
                    }
                }

                #endregion

                #region SQE

                num_SQE_SampleDisposeType_DisqualificationCount.Value = m_lnqBillInfo.SQE_SampleDisposeType_DisqualificationCount == null ? 0 :
                    (decimal)m_lnqBillInfo.SQE_SampleDisposeType_DisqualificationCount;

                num_SQE_SampleDisposeType_QualificationCount.Value = m_lnqBillInfo.SQE_SampleDisposeType_QualificationCount == null ? 0 :
                    (decimal)m_lnqBillInfo.SQE_SampleDisposeType_QualificationCount;

                if (m_lnqBillInfo.SQE_ProvidorFeedbackBill_IsExist != null)
                {
                    if ((bool)m_lnqBillInfo.SQE_ProvidorFeedbackBill_IsExist)
                    {
                        rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked = true;
                        rb_SQE_ProvidorFeedbackBill_IsExist_No.Checked = false;
                    }
                    else
                    {
                        rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked = false;
                        rb_SQE_ProvidorFeedbackBill_IsExist_No.Checked = true;
                    }
                }

                txt_SQE_ProvidorFeedbackBillNo.Text = m_lnqBillInfo.SQE_ProvidorFeedbackBillNo;
                #endregion

                #region 试装试验
                txt_TestResult_Experiment_ReportNo.Text = m_lnqBillInfo.TestResult_Experiment_ReportNo;
                txt_TestResult_ExperimentCVTNo.Text = m_lnqBillInfo.TestResult_ExperimentCVTNo;
                txt_TestResult_ResultAffrim_DiposeSuggestion_Bined.Text = m_lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Bined;
                txt_TestResult_TrialAssembly_ResultExplain.Text = m_lnqBillInfo.TestResult_TrialAssembly_ResultExplain;

                num_TestResult_TrialAssembly_Explain_PartCount.Value = m_lnqBillInfo.TestResult_TrialAssembly_Explain_PartCount == null ? 0 :
                    (decimal)m_lnqBillInfo.TestResult_TrialAssembly_Explain_PartCount;

                if (m_lnqBillInfo.TestResult_Experiment_Result != null)
                {
                    if ((bool)m_lnqBillInfo.TestResult_Experiment_Result)
                    {
                        rb_TestResult_Experiment_Result_Yes.Checked = true;
                        rb_TestResult_Experiment_Result_No.Checked = false;
                    }
                    else
                    {
                        rb_TestResult_Experiment_Result_Yes.Checked = false;
                        rb_TestResult_Experiment_Result_No.Checked = true;
                    }
                }

                if (m_lnqBillInfo.TestResult_TrialAssembly_Result != null)
                {
                    if ((bool)m_lnqBillInfo.TestResult_TrialAssembly_Result)
                    {
                        rb_TestResult_TrialAssembly_Result_Yes.Checked = true;
                        rb_TestResult_TrialAssembly_Result_No.Checked = false;
                    }
                    else
                    {
                        rb_TestResult_TrialAssembly_Result_Yes.Checked = false;
                        rb_TestResult_TrialAssembly_Result_No.Checked = true;
                    }
                }


                if (m_lnqBillInfo.TestResult_TrialAssembly_Explain != null)
                {
                    if (m_lnqBillInfo.TestResult_TrialAssembly_Explain.ToString() == rb_TestResult_TrialAssembly_Explain_All.Text)
                    {
                        rb_TestResult_TrialAssembly_Explain_All.Checked = true;
                        rb_TestResult_TrialAssembly_Explain_Part.Checked = false;
                    }
                    else if (m_lnqBillInfo.TestResult_TrialAssembly_Explain.ToString() == rb_TestResult_TrialAssembly_Explain_Part.Text)
                    {
                        rb_TestResult_TrialAssembly_Explain_All.Checked = false;
                        rb_TestResult_TrialAssembly_Explain_Part.Checked = true;
                    }
                }

                if (m_lnqBillInfo.TestResult_ResultAffrim != null 
                    && m_lnqBillInfo.TestResult_ResultAffrim.ToString().Contains(chb_TestResult_ResultAffrim_PPAP.Text))
                {
                    chb_TestResult_ResultAffrim_PPAP.Checked = true;
                }
                else
                {
                    chb_TestResult_ResultAffrim_PPAP.Checked = false;
                }

                if (m_lnqBillInfo.TestResult_ResultAffrim != null)
                {
                    foreach (Control cl in panel5.Controls)
                    {
                        if (cl is RadioButton &&
                            m_lnqBillInfo.TestResult_ResultAffrim.ToString().Contains(((RadioButton)cl).Text))
                        {
                            ((RadioButton)cl).Checked = true;
                            break;
                        }
                    }
                }

                if (m_lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Surplus != null)
                {
                    foreach (Control cl in panel6.Controls)
                    {
                        if (cl is RadioButton &&
                            m_lnqBillInfo.TestResult_ResultAffrim_DiposeSuggestion_Surplus.ToString().Contains(((RadioButton)cl).Text))
                        {
                            ((RadioButton)cl).Checked = true;
                            break;
                        }
                    }
                }

                txt_ChargeResult_ResultAffrim_DiposeSuggestion_Bined.Text = m_lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Bined;

                #endregion

                #region 开发主管确认
                if (m_lnqBillInfo.ChargeResult_SampleVerify != null)
                {
                    if ((bool)m_lnqBillInfo.ChargeResult_SampleVerify)
                    {
                        rb_ChargeResult_SampleVerify_Yes.Checked = true;
                        rb_ChargeResult_SampleVerify_No.Checked = false;
                    }
                    else
                    {
                        rb_ChargeResult_SampleVerify_Yes.Checked = false;
                        rb_ChargeResult_SampleVerify_No.Checked = true;
                    }
                }

                if (m_lnqBillInfo.ChargeResult_ResultAffrim != null)
                {
                    foreach (Control cl in panel11.Controls)
                    {
                        if (cl is RadioButton && m_lnqBillInfo.ChargeResult_ResultAffrim.ToString().Contains(((RadioButton)cl).Text))
                        {
                            ((RadioButton)cl).Checked = true;
                            break;
                        }
                    }
                }

                if (m_lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Surplus != null)
                {
                    foreach (Control cl in panel10.Controls)
                    {
                        if (cl is RadioButton && m_lnqBillInfo.ChargeResult_ResultAffrim_DiposeSuggestion_Surplus.ToString().Contains(((RadioButton)cl).Text))
                        {
                            ((RadioButton)cl).Checked = true;
                            break;
                        }
                    }
                }


                if (m_lnqBillInfo.ChargeResult_ResultAffrim != null
                    && m_lnqBillInfo.ChargeResult_ResultAffrim.ToString().Contains(chb_ChargeResult_ResultAffrim_PPAP.Text))
                {
                    chb_ChargeResult_ResultAffrim_PPAP.Checked = true;
                }
                else
                {
                    chb_ChargeResult_ResultAffrim_PPAP.Checked = false;
                }

                #endregion

                #region QC
                if (m_lnqBillInfo.QC_PPAPDispose != null)
                {
                    if (m_lnqBillInfo.QC_PPAPDispose.ToString() == rb_QC_PPAPDispose_PPAP.Text)
                    {
                        rb_QC_PPAPDispose_PPAP.Checked = true;
                        rb_QC_PPAPDispose_PL.Checked = false;
                    }
                    else
                    {
                        rb_QC_PPAPDispose_PPAP.Checked = false;
                        rb_QC_PPAPDispose_PL.Checked = true;
                    }
                }

                txt_QC_Suggestion.Text = m_lnqBillInfo.QC_Suggestion;
                #endregion

                #region 入库
                if (m_lnqFlowInfo.FlowID == 69)
                {
                    if (cmb_Review_InspectResult.Text == "退货" || 
                        cmb_Review_InspectResult.Text == "报废")
                    {
                        txt_Store_GoodsCount_InDepot.Text = "0";
                    }
                    else
                    {
                        decimal scrapReturn;
                        if (cmb_Purchase_SampleType.Text == "PPAP样件")
                        {
                            scrapReturn = num_SQE_SampleDisposeType_DisqualificationCount.Value;
                        }
                        else
                        {
                            scrapReturn = num_Review_InspectResult_ReWork_DisqualificationCount.Value;
                        }

                        txt_Store_GoodsCount_InDepot.Text = 
                            (num_Store_GoodsCount_AOG.Value - scrapReturn).ToString();
                    }
                }
                else
                {
                    txt_Store_GoodsCount_InDepot.Text = m_lnqBillInfo.Store_GoodsCount_InDepot == null ? "" :
                        m_lnqBillInfo.Store_GoodsCount_InDepot.ToString();
                }
                #endregion
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();
                m_lnqBillInfo = new Business_Sample_ConfirmTheApplication();

                txtBillNo.Text = this.FlowInfo_BillNo;

                txt_Purchase_BatchNo.Text = txtBillNo.Text;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.Purchase_BatchNo = txtBillNo.Text;
            }
        }

        /// <summary>
        /// 暂存检测数据
        /// </summary>
        /// <returns>通过返回True否则返回False</returns>
        bool CheckData_Hold()
        {
            if (txt_Purchase_GoodsCode.Tag == null || txt_Purchase_GoodsCode.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【物品】");
                return false;
            }

            if (num_Purchase_GoodsCount_Send.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【样件数量】");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 提交检测数据
        /// </summary>
        /// <returns>通过返回True否则返回False</returns>
        bool CheckData_Submit()
        {
            if (cmb_Purchase_StorageID.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择【所属库房】");
                return false;
            }

            if (cmb_Purchase_SampleType.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择【样件类型】");
                return false;
            }

            if (cmb_Purchase_SendSampleReason.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择【送样原因】");
                return false;
            }

            if (cmb_Purchase_SampleType.Text != "PPAP样件" && !cmb_Purchase_StorageID.Text.Contains("样品"))
            {
                MessageDialog.ShowPromptMessage("非PPAP样件【所属库房】必须为【样品类】库房");
                return false;
            }

            if (cmb_Purchase_SampleType.Text == "PPAP样件" && cmb_Purchase_StorageID.Text.Contains("样品"))
            {
                MessageDialog.ShowPromptMessage("PPAP样件【所属库房】必须为【非样品类】库房");
                return false;
            }

            if (rb_Store_IsProperPacking_No.Checked && txt_Store_Packing_IrrCause.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【样品包装不合格说明】");
                return false;
            }

            if (rb_Inspect_Result_No.Checked && txt_Inspect_IrrItemExplain.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【检验结果不合格序号说明】");
                return false;
            }

            if (rb_Review_RectificationRequest_IsExist_Yes.Checked && txt_Review_RectificationItem_Explain.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【应整改不合格项目说明】");
                return false;
            }

            if (rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked && txt_SQE_ProvidorFeedbackBillNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【供应商质量反馈单号】");
                return false;
            }

            if (rb_TestResult_TrialAssembly_Explain_Part.Checked && num_TestResult_TrialAssembly_Explain_PartCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【试装数量】");
                return false;
            }

            if (txt_Purchase_Provider.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【供应商】");
                return false;
            }

            if (txt_Purchase_ProviderBatchNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【供方批次号】");
                return false;
            }

            if (txt_Purchase_Version.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【版次号】");
                return false;
            }

            if (cmb_Purchase_SendSampleReason.Text == "量产品变更")
            {
                if (cmb_Purchase_Change_Type.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择【变更类型】");
                    return false;
                }

                if (txt_Purchase_Change_Reason.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请填写【主要更改原因说明】");
                    return false;
                }

                if (txt_Purchase_Change_BillNo.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请填写【更改通知单编号】");
                    return false;
                }
            }

            if (m_lnqFlowInfo != null)
            {
                bool rbCheckFlag = false;

                switch (m_lnqFlowInfo.FlowID)
                {
                    case 42:
                        if (true)
                        {
                            
                        }
                        break;

                    case 43:

                        if (num_Finance_RawMaterialCost.Value == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【原材料费】");
                            return false;
                        }

                        break;
                    case 44:

                        if (!rb_Store_IsProperPacking_No.Checked && !rb_Store_IsProperPacking_Yes.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择【样品包装】是否合格");
                            return false;
                        }

                        if (rb_Store_IsProperPacking_No.Checked && txt_Store_Packing_IrrCause.Text.Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【不合格说明】");
                            return false;
                        }

                        if (num_Store_GoodsCount_AOG.Value == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【到货数量】");
                            return false;
                        }

                        if (num_Store_GoodsCount_AOG.Value > num_Purchase_GoodsCount_Send.Value)
                        {
                            MessageDialog.ShowPromptMessage("【到货数量】不能大于【样件数量】");
                            return false;
                        }

                        break;
                    case 45:

                        if (!rb_Inspect_Result_Yes.Checked && !rb_Inspect_Result_No.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择【检验结果】是否合格");
                            return false;
                        }

                        if (rb_Inspect_Result_No.Checked && txt_Inspect_IrrItemExplain.Text.Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【检验结果不合格说明】");
                            return false;
                        }

                        if (!rb_Inspect_IsProperPacking_Yes.Checked && !rb_Inspect_IsProperPacking_No.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择【样品包装】是否合格");
                            return false;
                        }

                        if (rb_Inspect_IsProperPacking_No.Checked && txt_Inspect_Packing_IrrCause.Text.Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【样品包装不合格说明】");
                            return false;
                        }

                        if (!rb_Inspect_SupplierReport_IsExist_Yes.Checked && !rb_Inspect_SupplierReport_IsExist_No.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择是否有【供方检验报告】");
                            return false;
                        }

                        if (lb_Inspect_ReportFile.Tag == null || lb_Inspect_ReportFile.Tag.ToString().Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请上传【检验报告附件】");
                            return false;
                        }
                        break;
                    case 62:
                    case 46:

                        if (cmb_Review_InspectResult.Text.Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请选择【样品检验结果评审】");
                            return false;
                        }

                        if (cmb_Review_InspectResult.Text == "返工/返修" && cmb_Purchase_SampleType.Text != "PPAP样件")
                        {
                            if (MessageDialog.ShowEnquiryMessage("【非PPAP样件】【样品入库数量】以当前【零件工程师】填写的【返工/返修】"+
                                "后结果的【合格数】、【不合格数】信息为准，请确认已填写完整") == DialogResult.No)
                            {
                                return false;
                            }

                            if (num_Review_InspectResult_ReWork_DisqualificationCount.Value + 
                                num_Review_InspectResult_ReWork_QualificationCount.Value
                                 != num_Store_GoodsCount_AOG.Value - num_Inspect_GoodsCount_Scarp.Value)
                            {
                                MessageDialog.ShowPromptMessage("请填写正确的【返工/返修】的【合格数】与【不合格数】，【合格数】 + 【不合格数】 = 【到货数】 - 【检验报废数】");
                                return false;
                            }
                        }

                        if (!rb_Review_RectificationRequest_IsExist_No.Checked && !rb_Review_RectificationRequest_IsExist_Yes.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择是否有【供应商整改要求】");
                            return false;
                        }

                        if (rb_Review_RectificationRequest_IsExist_Yes.Checked && txt_Review_RectificationItem_Explain.Text.Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【应整改不合格项目说明】");
                            return false;
                        }

                        break;
                    case 47:
                        if (cmb_Review_InspectResult.Text == "返工/返修" && cmb_Purchase_SampleType.Text == "PPAP样件")
                        {
                            if (MessageDialog.ShowEnquiryMessage("【PPAP样件】【样品入库数量】以当前【SQE】填写的【返工/返修】" +
                                "后结果的【合格数】、【不合格数】信息为准，请确认已填写完整") == DialogResult.No)
                            {
                                return false;
                            }

                            if (num_SQE_SampleDisposeType_DisqualificationCount.Value + 
                                num_SQE_SampleDisposeType_QualificationCount.Value
                                 != num_Store_GoodsCount_AOG.Value - num_Inspect_GoodsCount_Scarp.Value)
                            {
                                MessageDialog.ShowPromptMessage("请填写正确的【返工/返修】的【合格数】与【不合格数】，【合格数】 + 【不合格数】 = 【到货数】 - 【检验报废数】");
                                return false;
                            }
                        }

                        if (!rb_SQE_ProvidorFeedbackBill_IsExist_No.Checked && !rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择是否有【供应商质量反馈单】");
                            return false;
                        }

                        if (rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked && txt_SQE_ProvidorFeedbackBillNo.Text.Trim().Length == 0)
                        {
                            MessageDialog.ShowPromptMessage("请填写【供应商质量反馈单号】");
                            return false;
                        }

                        break;
                    case 48:

                        if (rb_TestResult_Experiment_Result_No.Checked || rb_TestResult_Experiment_Result_Yes.Checked)
                        {
                            if (txt_TestResult_Experiment_ReportNo.Text.Trim().Length == 0)
                            {
                                MessageDialog.ShowPromptMessage("请填写【试验报告编号】");
                                return false;
                            }

                            if (txt_TestResult_ExperimentCVTNo.Text.Trim().Length == 0)
                            {
                                MessageDialog.ShowPromptMessage("请填写【试验总成编号】");
                                return false;
                            }
                        }

                        if (rb_TestResult_TrialAssembly_Result_Yes.Checked || rb_TestResult_TrialAssembly_Result_No.Checked)
                        {
                            if (!rb_TestResult_TrialAssembly_Explain_All.Checked && !rb_TestResult_TrialAssembly_Explain_Part.Checked)
                            {
                                MessageDialog.ShowPromptMessage("请选择【试装说明】");
                                return false;
                            }

                            if (rb_TestResult_TrialAssembly_Explain_Part.Checked && num_TestResult_TrialAssembly_Explain_PartCount.Value == 0)
                            {
                                MessageDialog.ShowPromptMessage("请填写【试装数量】");
                                return false;
                            }

                            if (txt_TestResult_TrialAssembly_ResultExplain.Text.Trim().Length == 0)
                            {
                                MessageDialog.ShowPromptMessage("请填写【试装结果说明】");
                                return false;
                            }
                        }

                        rbCheckFlag = false;
                        foreach (Control cl in panel5.Controls)
                        {
                            if (cl is RadioButton && ((RadioButton)cl).Checked)
                            {
                                rbCheckFlag = true;
                                break;
                            }
                        }

                        if (!rbCheckFlag)
                        {
                            MessageDialog.ShowPromptMessage("请选择【结果确认】");
                            return false;
                        }

                        rbCheckFlag = false;
                        foreach (Control cl in panel6.Controls)
                        {
                            if (cl is RadioButton && ((RadioButton)cl).Checked)
                            {
                                rbCheckFlag = true;
                                break;
                            }
                        }

                        if (!rbCheckFlag)
                        {
                            MessageDialog.ShowPromptMessage("请选择【剩余样件处理意见】");
                            return false;
                        }

                        break;
                    case 49:


                        rbCheckFlag = false;
                        foreach (Control cl in panel11.Controls)
                        {
                            if (cl is RadioButton && ((RadioButton)cl).Checked)
                            {
                                rbCheckFlag = true;
                                break;
                            }
                        }

                        if (!rbCheckFlag)
                        {
                            MessageDialog.ShowPromptMessage("请选择【结果确认】");
                            return false;
                        }

                        rbCheckFlag = false;
                        foreach (Control cl in panel10.Controls)
                        {
                            if (cl is RadioButton && ((RadioButton)cl).Checked)
                            {
                                rbCheckFlag = true;
                                break;
                            }
                        }

                        if (!rbCheckFlag)
                        {
                            MessageDialog.ShowPromptMessage("请选择【剩余样件处理意见】");
                            return false;
                        }

                        break;
                    case 50:

                        if (!rb_QC_PPAPDispose_PL.Checked && !rb_QC_PPAPDispose_PPAP.Checked)
                        {
                            MessageDialog.ShowPromptMessage("请选择【PPAP样件处理】方式");
                            return false;
                        }

                        break;
                    case 69:
                        if (txt_Store_GoodsCount_InDepot.Text.Trim().Length == 0 || Convert.ToDecimal(txt_Store_GoodsCount_InDepot.Text) == 0)
                        {
                            if (MessageDialog.ShowEnquiryMessage("【入库数量】为0 是否继续入库？") == DialogResult.No)
                            {
                                return false;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }

            return true;
        }

        private bool customPanel1_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (flowOperationType == CE_FlowOperationType.暂存)
                {
                    if (!CheckData_Hold())
                    {
                        return false;
                    }
                }
                else if (flowOperationType == CE_FlowOperationType.提交)
                {
                    if (!CheckData_Hold())
                    {
                        return false;
                    }

                    if (!CheckData_Submit())
                    {
                        return false;
                    }
                }

                GetInfo();

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.FlowInfo_StorageIDOrWorkShopCode = UniversalFunction.GetStorageID(cmb_Purchase_StorageID.Text);
                this.ResultInfo = m_lnqBillInfo;

                this.ResultList = new List<object>();
                this.ResultList.Add(flowOperationType);
                this.KeyWords = "【" + UniversalFunction.GetGoodsInfo(m_lnqBillInfo.Purchase_GoodsID).物品名称 + "】";

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void rb_Store_IsProperPacking_No_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Store_IsProperPacking_No.Checked)
            {
                txt_Store_Packing_IrrCause.Enabled = true;
            }
            else
            {
                txt_Store_Packing_IrrCause.Enabled = false;
                txt_Store_Packing_IrrCause.Text = "";
            }
        }

        private void rb_Inspect_Result_No_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Inspect_Result_No.Checked)
            {
                txt_Inspect_IrrItemExplain.Enabled = true;
            }
            else
            {
                txt_Inspect_IrrItemExplain.Enabled = false;
                txt_Inspect_IrrItemExplain.Text = "";
            }
        }

        private void rb_Review_RectificationRequest_IsExist_Yes_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Review_RectificationRequest_IsExist_Yes.Checked)
            {
                txt_Review_RectificationItem_Explain.Enabled = true;
            }
            else
            {
                txt_Review_RectificationItem_Explain.Enabled = false;
                txt_Review_RectificationItem_Explain.Text = "";
            }
        }

        private void rb_SQE_ProvidorFeedbackBill_IsExist_Yes_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_SQE_ProvidorFeedbackBill_IsExist_Yes.Checked)
            {
                txt_SQE_ProvidorFeedbackBillNo.Enabled = true;
            }
            else
            {
                txt_SQE_ProvidorFeedbackBillNo.Enabled = false;
                txt_SQE_ProvidorFeedbackBillNo.Text = "";
            }
        }

        private void rb_TestResult_TrialAssembly_Explain_Part_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_TestResult_TrialAssembly_Explain_Part.Checked)
            {
                num_TestResult_TrialAssembly_Explain_PartCount.Enabled = true;
            }
            else
            {
                num_TestResult_TrialAssembly_Explain_PartCount.Enabled = false;
                num_TestResult_TrialAssembly_Explain_PartCount.Value = 0;
            }
        }

        private void btn_SelectGoods_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString())
            {
                return;
            }

            FormQueryInfo form;

            if (chb_Purchase_IsPay.Checked)
            {
                if (txt_Purchase_OrderFormNo.Text.Length == 0)
                {
                    txt_Purchase_OrderFormNo.Focus();
                    MessageDialog.ShowPromptMessage(@"请先选择订单/合同号后再进行此操作！");
                    return;
                }

                form = QueryInfoDialog.GetOrderFormGoodsDialog(txt_Purchase_OrderFormNo.Text, true);
            }
            else
            {
                form = QueryInfoDialog.GetPlanCostGoodsDialog(true);
            }

            if (form == null || form.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                txt_Purchase_GoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txt_Purchase_GoodsName.Text = form.GetDataItem("物品名称").ToString();
                txt_Purchase_Spec.Text = form.GetDataItem("规格").ToString();

                View_F_GoodsPlanCost tempGoodsInfo = 
                    UniversalFunction.GetGoodsInfo(txt_Purchase_GoodsCode.Text, txt_Purchase_GoodsName.Text, txt_Purchase_Spec.Text);
                txt_Purchase_GoodsCode.Tag = tempGoodsInfo.序号;

                Dictionary<CE_GoodsAttributeName, object> dicAttribute = 
                    UniversalFunction.GetGoodsInfList_Attribute_AttchedInfoList(tempGoodsInfo.序号);

                if (dicAttribute.Keys.Contains(CE_GoodsAttributeName.毛坯) &&  Convert.ToBoolean( dicAttribute[CE_GoodsAttributeName.毛坯]))
                {
                    chb_Purchase_BillType_MP.Checked = true;
                }

                IBomServer bomService = PMS_ServerFactory.GetServerModule<IBomServer>();
                DataRow dr = bomService.GetBomInfo(txt_Purchase_GoodsCode.Text.Trim(), txt_Purchase_GoodsName.Text.Trim());

                if (dr == null)
                {
                    txt_Purchase_Version.Text = "";
                }
                else
                {
                    txt_Purchase_Version.Text = dr["Version"].ToString();
                }
            }
        }

        private void chb_Purchase_IsPay_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_Purchase_IsPay.Checked)
            {
                txt_Purchase_OrderFormNo.Enabled = true;
                btn_Purchase_OrderFormNo.Enabled = true;
                txt_Purchase_GoodsCode.Text = "";
                txt_Purchase_GoodsName.Text = "";
                txt_Purchase_Spec.Text = "";
                txt_Purchase_GoodsCode.Tag = null;
                btn_Purchase_Provider.Enabled = false;
            }
            else
            {
                txt_Purchase_OrderFormNo.Enabled = false;
                btn_Purchase_OrderFormNo.Enabled = false;
                btn_Purchase_Provider.Enabled = true;
            }

            txt_Purchase_Provider.Text = "";
            txt_Purchase_OrderFormNo.Text = "";
        }

        private void btn_Purchase_OrderFormNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetOrderFormInfoDialog(CE_BillTypeEnum.样品确认申请单);

            if (DialogResult.OK == form.ShowDialog())
            {
                txt_Purchase_GoodsCode.Text = "";
                txt_Purchase_GoodsName.Text = "";
                txt_Purchase_Spec.Text = "";
                txt_Purchase_Version.Text = "";
                txt_Purchase_OrderFormNo.Text = form.GetDataItem("订单号").ToString();
                txt_Purchase_Provider.Text = form.GetDataItem("供货单位").ToString();

                IOrderFormInfoServer orderFormService = 
                    ServerModule.ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

                IBargainInfoServer bargainInfoService =
                    ServerModule.ServerModuleFactory.GetServerModule<IBargainInfoServer>();

                View_B_OrderFormInfo lnqOrderForm = orderFormService.GetOrderFormInfo(txt_Purchase_OrderFormNo.Text);
                //View_B_BargainInfo lnqBargain = bargainInfoService.GetBargainInfo(lnqOrderForm.合同号);

                //chb_Purchase_BillType_WW.Checked = lnqBargain.是否委外合同;
            }
        }

        private void btn_Purchase_Provider_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetProviderInfoDialog();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txt_Purchase_Provider.Text = form.GetStringDataItem("供应商编码");
            }
        }

        private void chb_ChargeResult_ResultAffrim_PPAP_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_ChargeResult_ResultAffrim_PPAP.Checked)
            {
                groupBox9.Visible = true;
            }
            else
            {
                groupBox9.Visible = false;
            }
        }

        private void rb_Inspect_IsProperPacking_No_CheckedChanged(object sender, EventArgs e)
        {
            if (rb_Inspect_IsProperPacking_No.Checked)
            {
                txt_Inspect_Packing_IrrCause.Enabled = true;
            }
            else
            {
                txt_Inspect_Packing_IrrCause.Enabled = false;
                txt_Inspect_Packing_IrrCause.Text = "";
            }
        }

        private void cmb_Purchase_SendSampleReason_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Purchase_SendSampleReason.Text == "量产品变更")
            {
                label14.Visible = true;
                label15.Visible = true;
                label17.Visible = true;
                cmb_Purchase_Change_Type.Visible = true;
                txt_Purchase_Change_BillNo.Visible = true;
                txt_Purchase_Change_Reason.Visible = true;

                cmb_Purchase_Change_Type.Enabled = true;
                txt_Purchase_Change_BillNo.Enabled = true;
                txt_Purchase_Change_Reason.Enabled = true;
            }
            else
            {

                label14.Visible = false;
                label15.Visible = false;
                label17.Visible = false;
                cmb_Purchase_Change_Type.Visible = false;
                txt_Purchase_Change_BillNo.Visible = false;
                txt_Purchase_Change_Reason.Visible = false;

                cmb_Purchase_Change_Type.Enabled = false;
                txt_Purchase_Change_BillNo.Enabled = false;
                txt_Purchase_Change_Reason.Enabled = false;
            }
        }

        private void chb_Purchase_BillType_MP_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_Purchase_BillType_MP.Checked || chb_Review_IsPassTestResult.Checked)
            {
                groupBox7.Visible = false;
            }
            else
            {
                groupBox7.Visible = true;
            }
        }

        private void chb_Purchase_BillType_LX_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_Purchase_BillType_LX.Checked || cmb_Purchase_SampleType.Text.Trim() == "A样/手工样件")
            {
                groupBox6.Visible = false;
            }
            else
            {
                groupBox6.Visible = true;
            }
        }

        private void chb_Purchase_BillType_YCL_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_Purchase_BillType_YCL.Checked)
            {
                chb_Purchase_BillType_WW.Checked = true;
                groupBox2.Visible = true;
            }
            else
            {
                groupBox2.Visible = false;
            }
        }

        private void btn_Inspect_ReportFile_Up_Click(object sender, EventArgs e)
        {
            try
            {
                string strFilePath = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (openFileDialog1.FileNames.Count() > 2)
                    {
                        MessageDialog.ShowPromptMessage("上传文件数量不能大于2");
                        return;
                    }

                    if (lb_Inspect_ReportFile.Tag != null && lb_Inspect_ReportFile.Tag.ToString().Length > 0)
                    {
                        foreach (string fileItem in lb_Inspect_ReportFile.Tag.ToString().Split(','))
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        }
                    }

                    foreach (string filePath in openFileDialog1.FileNames)
                    {
                        Guid guid = Guid.NewGuid();
                        FileOperationService.File_UpLoad(guid, filePath,
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        strFilePath += guid.ToString() + ",";
                    }

                    lb_Inspect_ReportFile.Tag = strFilePath.Substring(0, strFilePath.Length - 1);
                    m_serviceSample.UpdateFilePath(txtBillNo.Text, lb_Inspect_ReportFile.Tag.ToString());
                    MessageDialog.ShowPromptMessage("上传成功");
                    btn_Inspect_ReportFile_Down.Enabled = true;
                    btn_Inspect_ReportFile_Look.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btn_Inspect_ReportFile_Down_Click(object sender, EventArgs e)
        {
            if (lb_Inspect_ReportFile.Tag == null || lb_Inspect_ReportFile.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("无附件下载");
                return;
            }


            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                string[] tempArray = lb_Inspect_ReportFile.Tag.ToString().Split(',');

                for (int i = 0; i < tempArray.Length; i++)
                {
                    FileOperationService.File_DownLoad(new Guid(tempArray[i]),
                        folderBrowserDialog1.SelectedPath + "\\" + txtBillNo.Text + "_" + i.ToString(),
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                }

                MessageDialog.ShowPromptMessage("下载成功");
            }
        }

        private void cmb_Review_InspectResult_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmb_Review_InspectResult.Text == "返工/返修" && cmb_Purchase_SampleType.Text != "PPAP样件")
            {

                label66.Visible = true;
                label26.Visible = true;
                label65.Visible = true;
                num_Review_InspectResult_ReWork_DisqualificationCount.Visible = true;
                num_Review_InspectResult_ReWork_QualificationCount.Visible = true;

                num_Review_InspectResult_ReWork_DisqualificationCount.Enabled = true;
                num_Review_InspectResult_ReWork_QualificationCount.Enabled = true;
            }
            else
            {
                label66.Visible = false;
                label26.Visible = false;
                label65.Visible = false;
                num_Review_InspectResult_ReWork_DisqualificationCount.Visible = false;
                num_Review_InspectResult_ReWork_QualificationCount.Visible = false;

                num_Review_InspectResult_ReWork_DisqualificationCount.Value = 0;
                num_Review_InspectResult_ReWork_QualificationCount.Value = 0;
                num_Review_InspectResult_ReWork_DisqualificationCount.Enabled = false;
                num_Review_InspectResult_ReWork_QualificationCount.Enabled = false;
            }

            if (cmb_Review_InspectResult.Text == "返工/返修" && cmb_Purchase_SampleType.Text == "PPAP样件")
            {
                label67.Visible = true;
                label32.Visible = true;
                label33.Visible = true;
                num_SQE_SampleDisposeType_DisqualificationCount.Visible = true;
                num_SQE_SampleDisposeType_QualificationCount.Visible = true;

                num_SQE_SampleDisposeType_DisqualificationCount.Enabled = true;
                num_SQE_SampleDisposeType_QualificationCount.Enabled = true;
            }
            else
            {
                label67.Visible = false;
                label32.Visible = false;
                label33.Visible = false;
                num_SQE_SampleDisposeType_DisqualificationCount.Visible = false;
                num_SQE_SampleDisposeType_QualificationCount.Visible = false;

                num_SQE_SampleDisposeType_DisqualificationCount.Value = 0;
                num_SQE_SampleDisposeType_QualificationCount.Value = 0;
                num_SQE_SampleDisposeType_DisqualificationCount.Enabled = false;
                num_SQE_SampleDisposeType_QualificationCount.Enabled = false;
            }

            //if (cmb_Review_InspectResult.Text == "退货")
            //{
            //    chb_Review_IsPassTestResult.Checked = true;
            //}
            //else
            //{
            //    chb_Review_IsPassTestResult.Checked = false;
            //}
        }

        private void btn_Inspect_ReportFile_Look_Click(object sender, EventArgs e)
        {
            if (lb_Inspect_ReportFile.Tag == null || lb_Inspect_ReportFile.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("无附件查看");
                return;
            }

            string[] tempArray = lb_Inspect_ReportFile.Tag.ToString().Split(',');

            for (int i = 0; i < tempArray.Length; i++)
            {
                FileOperationService.File_Look(new Guid(tempArray[i]),
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
            }
        }

        private void chb_Review_IsPassTestResult_CheckedChanged(object sender, EventArgs e)
        {
            if (chb_Purchase_BillType_MP.Checked || chb_Review_IsPassTestResult.Checked)
            {
                groupBox7.Visible = false;
            }
            else
            {
                groupBox7.Visible = true;
            }
        }

        private void cmb_Purchase_SampleType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chb_Purchase_BillType_LX.Checked || cmb_Purchase_SampleType.Text.Trim() == "A样/手工样件")
            {
                groupBox6.Visible = false;
            }
            else
            {
                groupBox6.Visible = true;
            }
        }

        private void cmb_Purchase_StorageID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!cmb_Purchase_StorageID.Text.Contains("样品"))
            {
                groupBox11.Visible = false;
            }
            else
            {
                groupBox11.Visible = true;
            }
        }
    }
}
