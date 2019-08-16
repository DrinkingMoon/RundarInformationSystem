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
using Service_Economic_Purchase;
using Form_Economic_Purchase;
using FlowControlService;

namespace Form_Economic_Purchase
{
    public partial class 供应商与零件归属变更单 : CustomMainForm
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IPartsBelongProviderChangeService m_serviceChangeBill = 
            Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IPartsBelongProviderChangeService>();

        public 供应商与零件归属变更单()
            : base(typeof(供应商与零件归属变更单明细), GlobalObject.CE_BillTypeEnum.供应商与零件归属变更单,
                Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IPartsBelongProviderChangeService>())
        {
            InitializeComponent();
            this.Form_CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail> detailInfo =
                    form.ResultInfo as List<View_Business_PurchasingMG_PartsBelongPriovderChangeDetail>;
                Business_PurchasingMG_PartsBelongPriovderChange lnqInPut = form.ResultList[0] as Business_PurchasingMG_PartsBelongPriovderChange;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqInPut.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        //if (!panel.GetNotifyPersonnel())
                        //{
                        //    return false;
                        //}

                        //NotifyPersonnel = panel.FlowInfo_NotifyInfo;
                        m_serviceChangeBill.SaveInfo(lnqInPut, detailInfo);
                        m_serviceChangeBill.FinishBill(lnqInPut.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceChangeBill.SaveInfo(lnqInPut, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceChangeBill.IsExist(lnqInPut.BillNo))
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
