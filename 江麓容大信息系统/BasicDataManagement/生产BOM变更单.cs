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
    public partial class 生产BOM变更单 : CustomMainForm
    {
        IPBOMChangeService _servicePBOMChange = Service_Project_Design.ServerModuleFactory.GetServerModule<IPBOMChangeService>();
        IFlowServer _serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 生产BOM变更单()
            : base(typeof(生产BOM变更单明细), GlobalObject.CE_BillTypeEnum.生产BOM变更单, 
            Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMChangeService>())
        {
            InitializeComponent();
        }

        private bool 生产BOM变更单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Bus_PBOM_Change lnqBillInfo = form.ResultInfo as Bus_PBOM_Change;
                List<View_Bus_PBOM_Change_Detail> detailInfo = form.ResultList[0] as List<View_Bus_PBOM_Change_Detail>;

                this.OperationType = form.FlowOperationType;
                this.BillNo = lnqBillInfo.BillNo;
                string billStatus = _serviceFlow.GetNowBillStatus(this.BillNo);

                if (billStatus == "" || billStatus == CE_CommonBillStatus.新建单据.ToString())
                {
                    _servicePBOMChange.SaveInfo(lnqBillInfo, detailInfo);
                }

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        this.NotifyPersonnel = form.FlowInfo_NotifyInfo;
                        _servicePBOMChange.OperatarUnFlowBusiness(lnqBillInfo.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                    case CE_FlowOperationType.回退:
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!_servicePBOMChange.IsExist(lnqBillInfo.BillNo))
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
