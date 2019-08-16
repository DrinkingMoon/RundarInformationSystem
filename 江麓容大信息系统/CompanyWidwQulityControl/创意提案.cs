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
using Service_Peripheral_CompanyQuality;
using UniversalControlLibrary;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class 创意提案 : CustomMainForm
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        ICreativeePersentation m_mainService = 
            Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<ICreativeePersentation>();

        public 创意提案(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
            : base(typeof(创意提案明细), GlobalObject.CE_BillTypeEnum.创意提案, 
            Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<ICreativeePersentation>())
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
        }

        void btnDirectAdd_Click(object sender, System.EventArgs e)
        {
            创意提案明细 frmDetail = new 创意提案明细();
            frmDetail.IsDirectAdd = true;
            FormCommonProcess frm = new FormCommonProcess(CE_BillTypeEnum.创意提案, null, frmDetail, CE_OperatorMode.添加);
            frm.CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);
            OperationType = CE_FlowOperationType.未知;

            if (frm.ShowDialog() != DialogResult.OK)
            {
                BillNumberControl m_billNoControl = new BillNumberControl(CE_BillTypeEnum.创意提案.ToString(), m_mainService);
                m_billNoControl.CancelBill(frmDetail.LnqBillInfo.BillNo);
            }
            else
            {
                SendMessage();
                m_mainService.DirectAdd(frmDetail.LnqBillInfo.BillNo);
            }
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Business_CWQC_CreativePersentation lnqSaveInfo = form.ResultList[0] as Business_CWQC_CreativePersentation;
                Business_CWQC_CreativePersentation_EffectValue lnqEffectValue = form.ResultList[2] as Business_CWQC_CreativePersentation_EffectValue;
                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqSaveInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        m_mainService.SaveInfo(lnqSaveInfo, lnqEffectValue);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_mainService.SaveInfo(lnqSaveInfo, lnqEffectValue);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_mainService.IsExist(lnqSaveInfo.BillNo))
                {
                    MessageDialog.ShowPromptMessage("数据为空，保存失败，如需退出，请直接X掉界面");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void 创意提案_Load(object sender, EventArgs e)
        {
            this.ToolStripSeparator_ShowStatus(m_authorityFlag);
        }
    }
}
