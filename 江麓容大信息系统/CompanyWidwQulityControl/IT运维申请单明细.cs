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
using Service_Peripheral_CompanyQuality;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class IT运维申请单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据号
        /// </summary>
        private Business_Composite_ComputerMalfunction _LnqBillInfo = new Business_Composite_ComputerMalfunction();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl _BillNoControl;

        /// <summary>
        /// 服务组件
        /// </summary>
        IComputerMalfunction _MainService = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IComputerMalfunction>();

        public IT运维申请单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                _BillNoControl = new BillNumberControl(CE_BillTypeEnum.IT运维申请单.ToString(), _MainService);
                _LnqBillInfo = _MainService.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            IFlowServer serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

            if (_LnqBillInfo != null)
            {
                lbBillStatus.Text = serverFlow.GetNowBillStatus(_LnqBillInfo.BillNo);
                txtBillNo.Text = _LnqBillInfo.BillNo;

                txtAssetsNo.Text = _LnqBillInfo.AssetsNo;
                txtContact.Text = _LnqBillInfo.Contact;
                txtEvaluation.Text = _LnqBillInfo.Evaluation;
                txtMalfunctionApproach.Text = _LnqBillInfo.MalfunctionApproach;
                txtMalfunctionReason.Text = _LnqBillInfo.MalfunctionReason;
                txtMalfunctionSymptom.Text = _LnqBillInfo.MalfunctionSymptom;
                txtMalfunctionAddress.Text = _LnqBillInfo.MalfunctionAddress;

                numElapsedTime.Value = _LnqBillInfo.ElapsedTime == null ? 0 : (decimal)_LnqBillInfo.ElapsedTime;
                numExpenses.Value = _LnqBillInfo.Expenses == null ? 0 : (decimal)_LnqBillInfo.Expenses;
                numSatisfaction.Value = _LnqBillInfo.Satisfaction == null ? 0 : (decimal)_LnqBillInfo.Satisfaction;

                cmbSymptomType.Text = _LnqBillInfo.SymptomType == null ? "" : _LnqBillInfo.SymptomType;

                if (_LnqBillInfo.Solve != null)
                {
                    GlobalObject.GeneralFunction.SetRadioButton(_LnqBillInfo.Solve, customGroupBox3);
                }

                List<CommonProcessInfo> lstTemp = serverFlow.GetFlowData(_LnqBillInfo.BillNo);
                CommonProcessInfo info = lstTemp.OrderBy(k => k.时间).First();
                txtLinkPersonnel.Text = info.人员;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                _LnqBillInfo = new Business_Composite_ComputerMalfunction();

                txtBillNo.Text = this.FlowInfo_BillNo;
                _LnqBillInfo.BillNo = txtBillNo.Text;
                txtLinkPersonnel.Text = BasicInfo.LoginName;
            }
        }

        void GetInfo()
        {
            _LnqBillInfo.MalfunctionAddress = txtMalfunctionAddress.Text;
            _LnqBillInfo.AssetsNo = txtAssetsNo.Text;
            _LnqBillInfo.BillNo = txtBillNo.Text;
            _LnqBillInfo.Contact = txtContact.Text;
            _LnqBillInfo.ElapsedTime = numElapsedTime.Value;
            _LnqBillInfo.Evaluation = txtEvaluation.Text;
            _LnqBillInfo.Expenses = numExpenses.Value;
            _LnqBillInfo.MalfunctionApproach = txtMalfunctionApproach.Text;
            _LnqBillInfo.MalfunctionReason = txtMalfunctionReason.Text;
            _LnqBillInfo.MalfunctionSymptom = txtMalfunctionSymptom.Text;
            _LnqBillInfo.Satisfaction = (int)numSatisfaction.Value;
            _LnqBillInfo.Solve = GlobalObject.GeneralFunction.GetRadioButton(customGroupBox3);
            _LnqBillInfo.SymptomType = cmbSymptomType.Text;
        }

        private bool IT运维申请单明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                GetInfo();
                this.FlowInfo_BillNo = _LnqBillInfo.BillNo;

                this.ResultList = new List<object>();
                this.ResultList.Add(_LnqBillInfo);
                this.ResultList.Add(flowOperationType);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }
    }
}
