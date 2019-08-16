using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using ServerModule;
using GlobalObject;
using Service_Quality_QC;
using FlowControlService;
using CommonBusinessModule;
using PlatformManagement;

namespace Form_Quality_QC
{
    public partial class 纠正预防措施报告明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务组件
        /// </summary>
        IEightDReport _Service_EightDReport = Service_Quality_QC.ServerModuleFactory.GetServerModule<IEightDReport>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _Service_Flow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 单据号
        /// </summary>
        private Bus_Quality_8DReport _Lnq_BillInfo = new Bus_Quality_8DReport();

        /// <summary>
        /// 预防措施类型
        /// </summary>
        string[] _Arrary_PreventType = 
            new string[] { "过程流程图", "FMEA", "控制计划", "作业指导书", "质量控制标准", "检验标准书", "技术图纸", "其他" };

        public 纠正预防措施报告明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.纠正预防措施报告.ToString(), _Service_EightDReport);
                _Lnq_BillInfo = _Service_EightDReport.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            if (_Lnq_BillInfo == null)
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();
                _Lnq_BillInfo = new Bus_Quality_8DReport();

                txtBillNo.Text = this.FlowInfo_BillNo;
                _Lnq_BillInfo.BillNo = txtBillNo.Text;
                _Lnq_BillInfo.F_Id = Guid.NewGuid().ToString();
            }
            else
            {
                lbBillStatus.Text = _Service_Flow.GetNowBillStatus(_Lnq_BillInfo.BillNo);

                txtBillNo.Text = _Lnq_BillInfo.BillNo;
                txtBatchNo.Text = _Lnq_BillInfo.BatchNo;
                txtBadness.Text = _Lnq_BillInfo.Badness;
                txtTheme.Text = _Lnq_BillInfo.Theme;
                txtInvolve.Text = _Lnq_BillInfo.Involve;
                txtHappenSite.Text = _Lnq_BillInfo.HappenSite;
                dtpHappenDate.Value = _Lnq_BillInfo.HappenDate == null ? ServerTime.Time : (DateTime)dtpHappenDate.Value;
                txtGoodsInfo.Text = _Lnq_BillInfo.GoodsInfo;

                //D1
                txtD1_DescribePhenomenon.Text = _Lnq_BillInfo.D1_DescribePhenomenon;

                if (_Lnq_BillInfo.D1_DescribePhenomenon_Accessory != null)
                {
                    llbD1_DescribePhenomenon_Accessory.Tag = _Lnq_BillInfo.D1_DescribePhenomenon_Accessory;
                    llbD1_DescribePhenomenon_Accessory.Enabled = true;
                    llbD1_DescribePhenomenon_Accessory_DownLoad.Enabled = true;
                }

                if (_Lnq_BillInfo.D1_LastHappenTime != null)
                {
                    dtpD1_LastHappenTime.Value = (DateTime)_Lnq_BillInfo.D1_LastHappenTime;
                    PanelRadioButtonSetChecked(plD1_LastHappenTime, "是");
                }

                if (_Lnq_BillInfo.D1_InfluenceElseProduct_Explain != null)
                {
                    txtD1_InfluenceElseProduct_Explain.Text = _Lnq_BillInfo.D1_InfluenceElseProduct_Explain;
                    PanelRadioButtonSetChecked(plD1_InfluenceElseProduct_Explain, "是");
                }

                if (_Lnq_BillInfo.D1_DutyDepartment != null)
                {
                    foreach (Control cl in plD1_DutyDepartment.Controls)
                    {
                        if (cl is RadioButton)
                        {
                            RadioButton rb = cl as RadioButton;
                            if (rb.Text == _Lnq_BillInfo.D1_DutyDepartment)
                            {
                                rb.Checked = true;
                                break;
                            }
                        }
                    }
                }

                //D2
                if (_Lnq_BillInfo.D2_DutyPersonnel != null)
                {
                    txtD2_DutyPersonnel.Text = UniversalFunction.GetPersonnelInfo(_Lnq_BillInfo.D2_DutyPersonnel).姓名;
                    txtD2_DutyPersonnel.Tag = _Lnq_BillInfo.D2_DutyPersonnel;
                }

                foreach (Bus_Quality_8DReport_D2_Crew item in _Lnq_BillInfo.Bus_Quality_8DReport_D2_Crew.ToList())
                {
                    View_HR_Personnel personnelInfo = UniversalFunction.GetPersonnelInfo(item.PersonnelWorkId);
                    dgvD2_Crew.Rows.Add(new object[] { personnelInfo.姓名, personnelInfo.部门名称, personnelInfo.工号 });
                }

                //D3
                if (_Lnq_BillInfo.D3_Reason_Incoming != null)
                {
                    txtD3_Reason_Incoming.Text = _Lnq_BillInfo.D3_Reason_Incoming;
                    PanelRadioButtonSetChecked(plD3_Reason_Incoming, "是");
                }

                if (_Lnq_BillInfo.D3_Reason_Producted != null)
                {
                    txtD3_Reason_Producted.Text = _Lnq_BillInfo.D3_Reason_Producted;
                    PanelRadioButtonSetChecked(plD3_Reason_Producted, "是");
                }

                if (_Lnq_BillInfo.D3_Reason_Production != null)
                {
                    txtD3_Reason_Production.Text = _Lnq_BillInfo.D3_Reason_Production;
                    PanelRadioButtonSetChecked(plD3_Reason_Production, "是");
                }

                if (_Lnq_BillInfo.D3_Reason_Send != null)
                {
                    txtD3_Reason_Send.Text = _Lnq_BillInfo.D3_Reason_Send;
                    PanelRadioButtonSetChecked(plD3_Reason_Send, "是");
                }

                //D4
                if (_Lnq_BillInfo.D4_Else_Measure != null)
                {
                    txtD4_Else_Measure.Text = _Lnq_BillInfo.D4_Else_Measure;
                    txtD4_Else_NG.Text = _Lnq_BillInfo.D4_Else_NG;
                    txtD4_Else_OK.Text = _Lnq_BillInfo.D4_Else_OK;
                    PanelRadioButtonSetChecked(plD4_Else, "是");
                }

                if (_Lnq_BillInfo.D4_FinishClient_Measure != null)
                {
                    txtD4_FinishClient_Measure.Text = _Lnq_BillInfo.D4_FinishClient_Measure;
                    txtD4_FinishClient_NG.Text = _Lnq_BillInfo.D4_FinishClient_NG;
                    txtD4_FinishClient_OK.Text = _Lnq_BillInfo.D4_FinishClient_OK;
                    PanelRadioButtonSetChecked(plD4_FinishClient, "是");
                }

                if (_Lnq_BillInfo.D4_FinishCom_Measure != null)
                {
                    txtD4_FinishCom_Measure.Text = _Lnq_BillInfo.D4_FinishCom_Measure;
                    txtD4_FinishCom_NG.Text = _Lnq_BillInfo.D4_FinishCom_NG;
                    txtD4_FinishCom_OK.Text = _Lnq_BillInfo.D4_FinishCom_OK;
                    PanelRadioButtonSetChecked(plD4_FinishCom, "是");
                }

                if (_Lnq_BillInfo.D4_Loading_Measure != null)
                {
                    txtD4_Loading_Measure.Text = _Lnq_BillInfo.D4_Loading_Measure;
                    txtD4_Loading_NG.Text = _Lnq_BillInfo.D4_Loading_NG;
                    txtD4_Loading_OK.Text = _Lnq_BillInfo.D4_Loading_OK;
                    PanelRadioButtonSetChecked(plD4_Loading, "是");
                }

                if (_Lnq_BillInfo.D4_Semi_Measure != null)
                {
                    txtD4_Semi_Measure.Text = _Lnq_BillInfo.D4_Semi_Measure;
                    txtD4_Semi_NG.Text = _Lnq_BillInfo.D4_Semi_NG;
                    txtD4_Semi_OK.Text = _Lnq_BillInfo.D4_Semi_OK;
                    PanelRadioButtonSetChecked(plD4_Semi, "是");
                }

                txtD4_HowIdentify.Text = _Lnq_BillInfo.D4_HowIdentify;

                //D5
                txtD5_ReasonAnalysis.Text = _Lnq_BillInfo.D5_ReasonAnalysis;


                if (_Lnq_BillInfo.D5_ReasonAnalysis_Accessory != null)
                {
                    llbD5_ReasonAnalysis_Accessory.Tag = _Lnq_BillInfo.D5_ReasonAnalysis_Accessory;
                    llbD5_ReasonAnalysis_Accessory.Enabled = true;
                    llbD5_ReasonAnalysis_Accessory_DownLoad.Enabled = true;
                }

                foreach (Bus_Quality_8DReport_D5_Reason item in _Lnq_BillInfo.Bus_Quality_8DReport_D5_Reason.Where(k =>
                            k.ReasonType == dgvD5_Reason_Out.Parent.Text).ToList())
                {
                    dgvD5_Reason_Out.Rows.Add(new object[] { item.Description });
                }

                foreach (Bus_Quality_8DReport_D5_Reason item in _Lnq_BillInfo.Bus_Quality_8DReport_D5_Reason.Where(k =>
                            k.ReasonType == dgvD5_Reason_Pro.Parent.Text).ToList())
                {
                    dgvD5_Reason_Pro.Rows.Add(new object[] { item.Description });
                }

                //D6

                foreach (Bus_Quality_8DReport_D6_Countermeasure item in _Lnq_BillInfo.Bus_Quality_8DReport_D6_Countermeasure.Where(k =>
                            k.CountermeasureType == dgvD6_Countermeasure_Out.Parent.Text).ToList())
                {
                    dgvD6_Countermeasure_Out.Rows.Add(new object[] { item.Description, item.Duty, item.FinishDate });
                }

                foreach (Bus_Quality_8DReport_D6_Countermeasure item in _Lnq_BillInfo.Bus_Quality_8DReport_D6_Countermeasure.Where(k =>
                            k.CountermeasureType == dgvD6_Countermeasure_Pro.Parent.Text).ToList())
                {
                    dgvD6_Countermeasure_Pro.Rows.Add(new object[] { item.Description, item.Duty, item.FinishDate });
                }

                //D7
                List<Bus_Quality_8DReport_D7_Prevent> lstPrevent = _Lnq_BillInfo.Bus_Quality_8DReport_D7_Prevent.ToList();

                foreach (string item in _Arrary_PreventType)
                {
                    List<Bus_Quality_8DReport_D7_Prevent> lstTemp = lstPrevent.Where(k => k.ItemsType == item).ToList();

                    if (lstTemp.Count() == 1)
                    {
                        dgvD7_Prevent.Rows.Add(new object[] { true, item, lstTemp.Single().Department, lstTemp.Single().OperationTime });
                    }
                    else
                    {
                        dgvD7_Prevent.Rows.Add(new object[] { false, item, "", "" });
                    }
                }

                foreach (DataGridViewRow dgvr in dgvD7_Prevent.Rows)
                {
                    if (Convert.ToBoolean(dgvr.Cells[0].Value))
                    {
                        dgvr.Cells[2].ReadOnly = false;
                        dgvr.Cells[3].ReadOnly = false;
                    }
                    else
                    {
                        dgvr.Cells[2].ReadOnly = true;
                        dgvr.Cells[3].ReadOnly = true;
                        dgvr.Cells[2].Value = null;
                        dgvr.Cells[3].Value = null;
                    }

                    dgvr.Cells[1].ReadOnly = true;
                }

                //D8
                if (_Lnq_BillInfo.D8_Unfulfillment_Explain != null)
                {
                    txtD8_Unfulfillment_Explain.Text = _Lnq_BillInfo.D8_Unfulfillment_Explain;
                    PanelRadioButtonSetChecked(plD8_Unfulfillment_Explain, "否");
                }

                foreach (Bus_Quality_8DReport_D8_Verify item in _Lnq_BillInfo.Bus_Quality_8DReport_D8_Verify.ToList())
                {
                    dgvD8_Verify.Rows.Add(new object[] { item.WayMode, item.Result, item.Duty, item.OperationDate, item.Effect });
                }
            }
        }

        void GetInfo()
        {
            object objValue = null;

            _Lnq_BillInfo.Badness = txtBadness.Text;
            _Lnq_BillInfo.BatchNo = txtBatchNo.Text;
            _Lnq_BillInfo.BillNo = txtBillNo.Text;
            _Lnq_BillInfo.GoodsInfo = txtGoodsInfo.Text;
            _Lnq_BillInfo.HappenDate = dtpHappenDate.Value;
            _Lnq_BillInfo.HappenSite = txtHappenSite.Text;
            _Lnq_BillInfo.Involve = txtInvolve.Text;
            _Lnq_BillInfo.Theme = txtTheme.Text;

            //D1
            _Lnq_BillInfo.D1_DescribePhenomenon = txtD1_DescribePhenomenon.Text;

            objValue = GetControlValue(llbD1_DescribePhenomenon_Accessory);
            _Lnq_BillInfo.D1_DescribePhenomenon_Accessory = objValue == null ? null : objValue.ToString();

            foreach (Control cl in plD1_DutyDepartment.Controls)
            {
                if (cl is RadioButton)
                {
                    RadioButton rb = cl as RadioButton;
                    if (rb.Checked)
                    {
                        _Lnq_BillInfo.D1_DutyDepartment = rb.Text;
                        break;
                    }
                }
            }

            objValue = GetControlValue(txtD1_InfluenceElseProduct_Explain);
            _Lnq_BillInfo.D1_InfluenceElseProduct_Explain = objValue == null ? null : objValue.ToString();

            objValue = GetControlValue(dtpD1_LastHappenTime);
            _Lnq_BillInfo.D1_LastHappenTime = objValue == null ? null : (DateTime?)objValue;


            //D2
            _Lnq_BillInfo.D2_DutyPersonnel = txtD2_DutyPersonnel.Tag == null ? null : txtD2_DutyPersonnel.Tag.ToString();
            _Lnq_BillInfo.Bus_Quality_8DReport_D2_Crew.Clear();
            foreach (DataGridViewRow dgvr in dgvD2_Crew.Rows)
            {
                Bus_Quality_8DReport_D2_Crew temp = new Bus_Quality_8DReport_D2_Crew();

                temp.F_Id = Guid.NewGuid().ToString();
                temp.ReportId = _Lnq_BillInfo.F_Id;
                temp.PersonnelWorkId = dgvr.Cells["D2PersonnelWorkId"].Value.ToString();

                _Lnq_BillInfo.Bus_Quality_8DReport_D2_Crew.Add(temp);
            }

            //D3
            objValue = GetControlValue(txtD3_Reason_Incoming);
            _Lnq_BillInfo.D3_Reason_Incoming = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD3_Reason_Producted);
            _Lnq_BillInfo.D3_Reason_Producted = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD3_Reason_Production);
            _Lnq_BillInfo.D3_Reason_Production = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD3_Reason_Send);
            _Lnq_BillInfo.D3_Reason_Send = objValue == null ? null : objValue.ToString();

            //D4
            objValue = GetControlValue(txtD4_Else_Measure);
            _Lnq_BillInfo.D4_Else_Measure = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Else_NG);
            _Lnq_BillInfo.D4_Else_NG = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Else_OK);
            _Lnq_BillInfo.D4_Else_OK = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_FinishClient_Measure);
            _Lnq_BillInfo.D4_FinishClient_Measure = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_FinishClient_NG);
            _Lnq_BillInfo.D4_FinishClient_NG = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_FinishClient_OK);
            _Lnq_BillInfo.D4_FinishClient_OK = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_FinishCom_Measure);
            _Lnq_BillInfo.D4_FinishCom_Measure = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_FinishCom_NG);
            _Lnq_BillInfo.D4_FinishCom_NG = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_FinishCom_OK);
            _Lnq_BillInfo.D4_FinishCom_OK = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Loading_Measure);
            _Lnq_BillInfo.D4_Loading_Measure = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Loading_NG);
            _Lnq_BillInfo.D4_Loading_NG = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Loading_OK);
            _Lnq_BillInfo.D4_Loading_OK = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Semi_Measure);
            _Lnq_BillInfo.D4_Semi_Measure = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Semi_NG);
            _Lnq_BillInfo.D4_Semi_NG = objValue == null ? null : objValue.ToString();
            objValue = GetControlValue(txtD4_Semi_OK);
            _Lnq_BillInfo.D4_Semi_OK = objValue == null ? null : objValue.ToString();

            _Lnq_BillInfo.D4_HowIdentify = txtD4_HowIdentify.Text;

            //D5
            _Lnq_BillInfo.D5_ReasonAnalysis = txtD5_ReasonAnalysis.Text;
            objValue = GetControlValue(llbD5_ReasonAnalysis_Accessory);
            _Lnq_BillInfo.D5_ReasonAnalysis_Accessory = objValue == null ? null : objValue.ToString();

            _Lnq_BillInfo.Bus_Quality_8DReport_D5_Reason.Clear();
            foreach (DataGridViewRow dgvr in dgvD5_Reason_Out.Rows)
            {
                Bus_Quality_8DReport_D5_Reason tempInfo = new Bus_Quality_8DReport_D5_Reason();

                tempInfo.F_Id = Guid.NewGuid().ToString();
                tempInfo.ReportId = _Lnq_BillInfo.F_Id;
                tempInfo.Description = dgvr.Cells["D5Out_Description"].Value.ToString();
                tempInfo.ReasonType = dgvD5_Reason_Out.Parent.Text;

                _Lnq_BillInfo.Bus_Quality_8DReport_D5_Reason.Add(tempInfo);
            }

            foreach (DataGridViewRow dgvr in dgvD5_Reason_Pro.Rows)
            {
                Bus_Quality_8DReport_D5_Reason tempInfo = new Bus_Quality_8DReport_D5_Reason();

                tempInfo.F_Id = Guid.NewGuid().ToString();
                tempInfo.ReportId = _Lnq_BillInfo.F_Id;
                tempInfo.Description = dgvr.Cells["D5Pro_Description"].Value.ToString();
                tempInfo.ReasonType = dgvD5_Reason_Pro.Parent.Text;

                _Lnq_BillInfo.Bus_Quality_8DReport_D5_Reason.Add(tempInfo);
            }

            //D6
            _Lnq_BillInfo.Bus_Quality_8DReport_D6_Countermeasure.Clear();

            foreach (DataGridViewRow dgvr in dgvD6_Countermeasure_Out.Rows)
            {
                Bus_Quality_8DReport_D6_Countermeasure tempInfo = new Bus_Quality_8DReport_D6_Countermeasure();

                tempInfo.F_Id = Guid.NewGuid().ToString();
                tempInfo.ReportId = _Lnq_BillInfo.F_Id;
                tempInfo.CountermeasureType = dgvD6_Countermeasure_Out.Parent.Text;
                tempInfo.Description = dgvr.Cells["D6Out_Description"].Value.ToString();
                tempInfo.Duty = dgvr.Cells["D6Out_Duty"].Value.ToString();
                tempInfo.FinishDate = Convert.ToDateTime(dgvr.Cells["D6Out_FinishDate"].Value);

                _Lnq_BillInfo.Bus_Quality_8DReport_D6_Countermeasure.Add(tempInfo);
            }

            foreach (DataGridViewRow dgvr in dgvD6_Countermeasure_Pro.Rows)
            {
                Bus_Quality_8DReport_D6_Countermeasure tempInfo = new Bus_Quality_8DReport_D6_Countermeasure();

                tempInfo.F_Id = Guid.NewGuid().ToString();
                tempInfo.ReportId = _Lnq_BillInfo.F_Id;
                tempInfo.CountermeasureType = dgvD6_Countermeasure_Pro.Parent.Text;
                tempInfo.Description = dgvr.Cells["D6Pro_Description"].Value.ToString();
                tempInfo.Duty = dgvr.Cells["D6Pro_Duty"].Value.ToString();
                tempInfo.FinishDate = dgvr.Cells["D6Pro_FinishDate"].Value == null || 
                    GeneralFunction.IsNullOrEmpty(dgvr.Cells["D6Pro_FinishDate"].Value.ToString()) ?
                    null : (DateTime?)dgvr.Cells["D6Pro_FinishDate"].Value;

                _Lnq_BillInfo.Bus_Quality_8DReport_D6_Countermeasure.Add(tempInfo);
            }

            //D7
            _Lnq_BillInfo.Bus_Quality_8DReport_D7_Prevent.Clear();

            foreach (DataGridViewRow dgvr in dgvD7_Prevent.Rows)
            {
                if (dgvr.Cells["D7Select"].Value != null && Convert.ToBoolean(dgvr.Cells["D7Select"].Value))
                {
                    Bus_Quality_8DReport_D7_Prevent tempInfo = new Bus_Quality_8DReport_D7_Prevent();

                    tempInfo.F_Id = Guid.NewGuid().ToString();
                    tempInfo.ReportId = _Lnq_BillInfo.F_Id;
                    tempInfo.Department = dgvr.Cells["D7Department"].Value.ToString();
                    tempInfo.ItemsType = dgvr.Cells["D7ItemsType"].Value.ToString();
                    tempInfo.OperationTime = dgvr.Cells["D7OperationTime"].Value == null || 
                        GeneralFunction.IsNullOrEmpty(dgvr.Cells["D7OperationTime"].Value.ToString()) ?
                        null : (DateTime?)dgvr.Cells["D7OperationTime"].Value;

                    _Lnq_BillInfo.Bus_Quality_8DReport_D7_Prevent.Add(tempInfo);
                }
            }

            //D8
            objValue = GetControlValue(txtD8_Unfulfillment_Explain);
            _Lnq_BillInfo.D8_Unfulfillment_Explain = objValue == null ? null : objValue.ToString();

            _Lnq_BillInfo.Bus_Quality_8DReport_D8_Verify.Clear();

            foreach (DataGridViewRow dgvr in dgvD8_Verify.Rows)
            {
                Bus_Quality_8DReport_D8_Verify tempInfo = new Bus_Quality_8DReport_D8_Verify();

                tempInfo.F_Id = Guid.NewGuid().ToString();
                tempInfo.ReportId = _Lnq_BillInfo.F_Id;
                tempInfo.Duty = dgvr.Cells["D8Duty"].Value.ToString();
                tempInfo.Effect = dgvr.Cells["D8Effect"].Value.ToString();
                tempInfo.OperationDate = dgvr.Cells["D8OperationDate"].Value == null || 
                    GeneralFunction.IsNullOrEmpty(dgvr.Cells["D8OperationDate"].Value.ToString()) ?
                    null : (DateTime?)dgvr.Cells["D8OperationDate"].Value;
                tempInfo.Result = dgvr.Cells["D8Result"].Value.ToString();
                tempInfo.WayMode = dgvr.Cells["D8WayMode"].Value.ToString();

                _Lnq_BillInfo.Bus_Quality_8DReport_D8_Verify.Add(tempInfo);
            }
        }

        object GetControlValue(Control cl)
        {
            if (cl is TextBox)
            {
                TextBox txt = cl as TextBox;
                return txt.Enabled ? txt.Text : null;
            }
            else if (cl is DateTimePicker)
            {
                DateTimePicker dtp = cl as DateTimePicker;
                return dtp.Enabled ? (DateTime?)dtp.Value : null;
            }
            else if (cl is LinkLabel)
            {
                LinkLabel llb = cl as LinkLabel;
                return llb.Tag == null ? null : llb.Tag.ToString();
            }
            else
            {
                return null;
            }
        }

        private bool 纠正预防措施报告明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                GetInfo();

                Flow_FlowInfo info =
                    _Service_Flow.GetNowFlowInfo(_Service_Flow.GetBusinessTypeID(CE_BillTypeEnum.纠正预防措施报告, null),
                    _Lnq_BillInfo.BillNo);

                if (flowOperationType == CE_FlowOperationType.提交)
                {
                    NotifyPersonnelInfo personnelInfo = new NotifyPersonnelInfo();
                    List<PersonnelBasicInfo> lstInfo = new List<PersonnelBasicInfo>();
                    PersonnelBasicInfo infoTemp = new PersonnelBasicInfo();

                    switch (info.FlowID)
                    {
                        case 1109:
                            if (GeneralFunction.IsNullOrEmpty(_Lnq_BillInfo.D1_DutyDepartment))
                            {
                                throw new Exception("请选择【调查改善担当部门】");
                            }
                            break;

                        case 1110:
                        case 1112:
                            IBillMessagePromulgatorServer serviceBillMessage =
                                BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

                            List<string> lstRoleName = serviceBillMessage.GetSuperior(CE_RoleStyleType.负责人,
                                UniversalFunction.GetDeptCode(_Lnq_BillInfo.D1_DutyDepartment));

                            personnelInfo.UserType = BillFlowMessage_ReceivedUserType.角色.ToString();
                            foreach (string item in lstRoleName)
                            {
                                infoTemp = new PersonnelBasicInfo();
                                infoTemp.角色 = item;
                                lstInfo.Add(infoTemp);
                            }

                            personnelInfo.PersonnelBasicInfoList = lstInfo;
                            this.FlowInfo_NotifyInfo = personnelInfo;
                            break;
                        case 1111:

                            if (GeneralFunction.IsNullOrEmpty(_Lnq_BillInfo.D2_DutyPersonnel))
                            {
                                throw new Exception("请指定【改善小组组长】");
                            }

                            personnelInfo.UserType = BillFlowMessage_ReceivedUserType.用户.ToString();
                            infoTemp = new PersonnelBasicInfo();
                            infoTemp.工号 = _Lnq_BillInfo.D2_DutyPersonnel;
                            lstInfo.Add(infoTemp);

                            personnelInfo.PersonnelBasicInfoList = lstInfo;
                            this.FlowInfo_NotifyInfo = personnelInfo;
                            break;
                        default:
                            break;
                    }
                }

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.ResultInfo = _Lnq_BillInfo;

                this.ResultList = new List<object>();
                this.ResultList.Add(flowOperationType);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        void PanelRadioButtonSetChecked(Panel pl, string rbText)
        {
            foreach (Control cl in pl.Controls)
            {
                if (cl is RadioButton)
                {
                    RadioButton rb = cl as RadioButton;

                    if (rb.Text == rbText)
                    {
                        rb.Checked = true;
                    }
                    else
                    {
                        rb.Checked = false;
                    }
                }
            }
        }

        private void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            foreach (Control cl in rb.Parent.Controls)
            {
                if (!(cl is RadioButton) && !(cl is Label))
                {
                    cl.Enabled = rb.Checked;
                }
            }
        }

        private void btnDataGridViewRowAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = sender as Button;

                switch (bt.Name)
                {
                    case "btnD2_Crew_Add":
                        if (GeneralFunction.IsNullOrEmpty(txtD2_Crew.Text))
                        {
                            throw new Exception("请选择【改善小组组员】");
                        }

                        foreach (DataGridViewRow dgvr in dgvD2_Crew.Rows)
                        {
                            if (dgvr.Cells["D2PersonnelWorkId"].Value.ToString() == txtD2_Crew.Tag.ToString())
                            {
                                return;
                            }
                        }

                        View_HR_Personnel personnnelInfo = UniversalFunction.GetPersonnelInfo(txtD2_Crew.Tag.ToString());

                        dgvD2_Crew.Rows.Add(new object[] { personnnelInfo.姓名, personnnelInfo.部门名称, personnnelInfo.工号 });

                        txtD2_Crew.Text = "";
                        txtD2_Crew.Tag = null;
                        break;
                    case "btnD5_Reason_Pro_Add":
                        if (GeneralFunction.IsNullOrEmpty(txtD5_Reason_Pro_Des.Text))
                        {
                            throw new Exception("请填写【产生原因描述】");
                        }

                        dgvD5_Reason_Pro.Rows.Add(new object[] { txtD5_Reason_Pro_Des.Text });

                        txtD5_Reason_Pro_Des.Text = "";
                        break;
                    case "btnD5_Reason_Out_Add":
                        if (GeneralFunction.IsNullOrEmpty(txtD5_Reason_Out_Des.Text))
                        {
                            throw new Exception("请填写【流出原因描述】");
                        }

                        dgvD5_Reason_Out.Rows.Add(new object[] { txtD5_Reason_Out_Des.Text });

                        txtD5_Reason_Out_Des.Text = "";
                        break;
                    case "btnD6_Countermeasure_Pro_Add":
                        if (GeneralFunction.IsNullOrEmpty(txtD6_Countermeasure_Pro_Des.Text))
                        {
                            throw new Exception("请填写【产生原因描述】");
                        }

                        if (GeneralFunction.IsNullOrEmpty(txtD6_Countermeasure_Pro_Duty.Text))
                        {
                            throw new Exception("请填写【负责人/部门】");
                        }

                        dgvD6_Countermeasure_Pro.Rows.Add(new object[] { txtD6_Countermeasure_Pro_Des.Text, txtD6_Countermeasure_Pro_Duty.Text, 
                            dtpD6_Countermeasure_Pro.Value });

                        txtD6_Countermeasure_Pro_Des.Text = "";
                        txtD6_Countermeasure_Pro_Duty.Text = "";
                        dtpD6_Countermeasure_Pro.Value = ServerTime.Time;

                        break;
                    case "btnD6_Countermeasure_Out_Add":
                        if (GeneralFunction.IsNullOrEmpty(txtD6_Countermeasure_Out_Des.Text))
                        {
                            throw new Exception("请填写【流出原因描述】");
                        }

                        if (GeneralFunction.IsNullOrEmpty(txtD6_Countermeasure_Out_Duty.Text))
                        {
                            throw new Exception("请填写【负责人/部门】");
                        }

                        dgvD6_Countermeasure_Out.Rows.Add(new object[] { txtD6_Countermeasure_Out_Des.Text, txtD6_Countermeasure_Out_Duty.Text, 
                            dtpD6_Countermeasure_Out.Value });

                        txtD6_Countermeasure_Out_Des.Text = "";
                        txtD6_Countermeasure_Out_Duty.Text = "";
                        dtpD6_Countermeasure_Out.Value = ServerTime.Time;
                        break;
                    case "btnD8_Verify_Add":
                        if (GeneralFunction.IsNullOrEmpty(txtD8_Verify_WayMode.Text))
                        {
                            throw new Exception("请填写【验证方式】");
                        }

                        if (GeneralFunction.IsNullOrEmpty(txtD8_Verify_Result.Text))
                        {
                            throw new Exception("请填写【验证结果】");
                        }

                        if (GeneralFunction.IsNullOrEmpty(txtD8_Verify_Duty.Text))
                        {
                            throw new Exception("请填写【负责人/部门】");
                        }

                        if (GeneralFunction.IsNullOrEmpty(cmbD8_Verify_Effect.Text))
                        {
                            throw new Exception("请选择【效果】");
                        }

                        dgvD8_Verify.Rows.Add(new object[] { txtD8_Verify_WayMode.Text, txtD8_Verify_Result.Text, txtD8_Verify_Duty.Text,  
                            dtpD8_Verify.Value, cmbD8_Verify_Effect.Text });

                        txtD8_Verify_WayMode.Text = "";
                        txtD8_Verify_Result.Text = "";
                        txtD8_Verify_Duty.Text = "";
                        cmbD8_Verify_Effect.SelectedIndex = -1;
                        dtpD8_Verify.Value = ServerTime.Time;

                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDataGridViewRowDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Button bt = sender as Button;

                if (bt.Name == "btnD2_Crew_Delete")
                {
                    foreach (DataGridViewRow dgvr in dgvD2_Crew.SelectedRows)
                    {
                        dgvD2_Crew.Rows.Remove(dgvr);
                    }
                }
                else
                {
                    Control parentcl = bt.Parent.Parent;
                    CustomDataGridView cdgv = new CustomDataGridView();

                    foreach (Control cl in parentcl.Controls)
                    {
                        if (cl is CustomDataGridView)
                        {
                            cdgv = cl as CustomDataGridView;
                            break;
                        }
                    }

                    foreach (DataGridViewRow dgvr in cdgv.SelectedRows)
                    {
                        cdgv.Rows.Remove(dgvr);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void DataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridView dgvr = sender as DataGridView;

                if (dgvr.CurrentRow == null)
                {
                    return;
                }

                switch (dgvr.Name)
                {
                    case "dgvD2_Crew":
                        txtD2_Crew.Text = dgvr.CurrentRow.Cells["D2PersonnelName"].Value.ToString();
                        txtD2_Crew.Tag = dgvr.CurrentRow.Cells["D2PersonnelWorkId"].Value.ToString();
                        break;
                    case "dgvD5_Reason_Pro":
                        txtD5_Reason_Pro_Des.Text = dgvr.CurrentRow.Cells["D5Pro_Description"].Value.ToString();
                        break;
                    case "dgvD5_Reason_Out":
                        txtD5_Reason_Pro_Des.Text = dgvr.CurrentRow.Cells["D5Out_Description"].Value.ToString();
                        break;
                    case "dgvD6_Countermeasure_Pro":
                        txtD6_Countermeasure_Pro_Des.Text = dgvr.CurrentRow.Cells["D6Pro_Description"].Value.ToString();
                        txtD6_Countermeasure_Pro_Duty.Text = dgvr.CurrentRow.Cells["D6Pro_Duty"].Value.ToString();
                        dtpD6_Countermeasure_Pro.Value = Convert.ToDateTime( dgvr.CurrentRow.Cells["D6Pro_FinishDate"].Value);
                        break;
                    case "dgvD6_Countermeasure_Out":
                        txtD6_Countermeasure_Out_Des.Text = dgvr.CurrentRow.Cells["D6Out_Description"].Value.ToString();
                        txtD6_Countermeasure_Out_Duty.Text = dgvr.CurrentRow.Cells["D6Out_Duty"].Value.ToString();
                        dtpD6_Countermeasure_Out.Value = Convert.ToDateTime(dgvr.CurrentRow.Cells["D6Out_FinishDate"].Value);
                        break;
                    case "dgvD8_Verify":
                        txtD8_Verify_Duty.Text = dgvr.CurrentRow.Cells["D8Duty"].Value.ToString();
                        txtD8_Verify_Result.Text = dgvr.CurrentRow.Cells["D8Result"].Value.ToString();
                        txtD8_Verify_WayMode.Text = dgvr.CurrentRow.Cells["D8WayMode"].Value.ToString();
                        cmbD8_Verify_Effect.Text = dgvr.CurrentRow.Cells["D8Effect"].Value.ToString();
                        dtpD8_Verify.Value = Convert.ToDateTime(dgvr.CurrentRow.Cells["D8OperationDate"].Value);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        List<LinkLabel> GetListLinkLabel(Control clMain)
        {
            LinkLabel lbUpLoad = new LinkLabel();
            LinkLabel lbLook = new LinkLabel();
            LinkLabel lbDownLoad = new LinkLabel();

            foreach (Control cl in clMain.Parent.Controls)
            {
                if (cl is LinkLabel)
                {
                    if (cl.Name.Contains("DownLoad"))
                    {
                        lbDownLoad = cl as LinkLabel;
                    }
                    else if (cl.Name.Contains("UpLoad"))
                    {
                        lbUpLoad = cl as LinkLabel;
                    }
                    else
                    {
                        lbLook = cl as LinkLabel;
                    }
                }
            }

            List<LinkLabel> lstResult = new List<LinkLabel>();

            lstResult.Add(lbUpLoad);
            lstResult.Add(lbLook);
            lstResult.Add(lbDownLoad);

            return lstResult;
        }

        private void LinkLable_UpLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                List<LinkLabel> lstllb = GetListLinkLabel(sender as Control);

                LinkLabel lbUpLoad = lstllb[0];
                LinkLabel lbLook = lstllb[1];
                LinkLabel lbDownLoad = lstllb[2];

                string strFilePath = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (lbLook.Tag != null && lbLook.Tag.ToString().Length > 0)
                    {
                        foreach (string fileItem in lbLook.Tag.ToString().Split(','))
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

                    lbLook.Tag = strFilePath.Substring(0, strFilePath.Length - 1);
                    _Service_EightDReport.UpdateFilePath(txtBillNo.Text, lbLook.Tag.ToString(), lbLook.Name.Contains("D1") ? "D1" : "D5");
                    MessageDialog.ShowPromptMessage("上传成功");
                    lbDownLoad.Enabled = true;
                    lbLook.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void LinkLable_Look_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                List<LinkLabel> lstllb = GetListLinkLabel(sender as Control);

                LinkLabel lbUpLoad = lstllb[0];
                LinkLabel lbLook = lstllb[1];
                LinkLabel lbDownLoad = lstllb[2];

                if (lbLook.Tag == null || lbLook.Tag.ToString().Length == 0)
                {
                    throw new Exception("无附件查看");
                }

                string[] tempArray = lbLook.Tag.ToString().Split(',');

                for (int i = 0; i < tempArray.Length; i++)
                {
                    FileOperationService.File_Look(new Guid(tempArray[i]),
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void LinkLable_DownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                List<LinkLabel> lstllb = GetListLinkLabel(sender as Control);

                LinkLabel lbUpLoad = lstllb[0];
                LinkLabel lbLook = lstllb[1];
                LinkLabel lbDownLoad = lstllb[2];

                if (lbLook.Tag == null || lbLook.Tag.ToString().Length == 0)
                {
                    throw new Exception("无附件下载");
                }

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    string[] tempArray = lbLook.Tag.ToString().Split(',');

                    for (int i = 0; i < tempArray.Length; i++)
                    {
                        FileOperationService.File_DownLoad(new Guid(tempArray[i]),
                            folderBrowserDialog1.SelectedPath + "\\" + txtBillNo.Text + "_" + i.ToString(),
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                    }

                    MessageDialog.ShowPromptMessage("下载成功");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void txtD2_Crew_OnCompleteSearch()
        {
            if (txtD2_Crew.DataResult == null)
            {
                return;
            }

            txtD2_Crew.Text = txtD2_Crew.DataResult["姓名"].ToString();
            txtD2_Crew.Tag = txtD2_Crew.DataResult["工号"].ToString();
        }

        private void txtD2_DutyPersonnel_OnCompleteSearch()
        {
            if (txtD2_DutyPersonnel.DataResult == null)
            {
                return;
            }

            txtD2_DutyPersonnel.Text = txtD2_DutyPersonnel.DataResult["姓名"].ToString();
            txtD2_DutyPersonnel.Tag = txtD2_DutyPersonnel.DataResult["工号"].ToString();
        }

        private void dgvD7_Prevent_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                if (Convert.ToBoolean(dgvD7_Prevent.Rows[e.RowIndex].Cells[e.ColumnIndex].Value))
                {
                    dgvD7_Prevent.Rows[e.RowIndex].Cells[2].ReadOnly = true;
                    dgvD7_Prevent.Rows[e.RowIndex].Cells[3].ReadOnly = true;
                    dgvD7_Prevent.Rows[e.RowIndex].Cells[2].Value = null;
                    dgvD7_Prevent.Rows[e.RowIndex].Cells[3].Value = null;
                }
                else
                {
                    dgvD7_Prevent.Rows[e.RowIndex].Cells[2].ReadOnly = false;
                    dgvD7_Prevent.Rows[e.RowIndex].Cells[3].ReadOnly = false;
                }
            }
        }
    }
}
