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
using Service_Project_Design;
using FlowControlService;


namespace Form_Project_Design
{
    public partial class 设计BOM变更单 : CustomMainForm
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IBOMChangeService m_serviceStatement = Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMChangeService>();

        public 设计BOM变更单()
            : base(typeof(设计BOM变更单明细), GlobalObject.CE_BillTypeEnum.设计BOM变更单, 
            Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMChangeService>())
        {
            InitializeComponent();

            this.Form_CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Business_Base_BomChange lnqBillInfo = form.ResultInfo as Business_Base_BomChange;

                List<View_Business_Base_BomChange_Struct> structInfo =
                    form.ResultList[0] as List<View_Business_Base_BomChange_Struct>;
                List<View_Business_Base_BomChange_PartsLibrary> libraryInfo =
                    form.ResultList[1] as List<View_Business_Base_BomChange_PartsLibrary>;

                this.OperationType = form.FlowOperationType;
                this.BillNo = lnqBillInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        this.NotifyPersonnel = form.FlowInfo_NotifyInfo;
                        m_serviceStatement.SaveInfo(lnqBillInfo, libraryInfo, structInfo);
                        m_serviceStatement.OperatarUnFlowBusiness(lnqBillInfo.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceStatement.SaveInfo(lnqBillInfo, libraryInfo, structInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceStatement.IsExist(lnqBillInfo.BillNo))
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
    }
}
