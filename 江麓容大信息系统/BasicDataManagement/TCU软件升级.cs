using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using FlowControlService;
using Service_Project_Design;

namespace Form_Project_Design
{
    public partial class TCU软件升级 : CustomMainForm
    {
        ITCUInfoService _ServiceTCU = Service_Project_Design.ServerModuleFactory.GetServerModule<ITCUInfoService>();

        public TCU软件升级()
            : base(typeof(TCU软件升级明细), GlobalObject.CE_BillTypeEnum.TCU软件升级,
            Service_Project_Design.ServerModuleFactory.GetServerModule<ITCUInfoService>())
        {
            InitializeComponent();
        }

        private bool TCU软件升级_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            CE_FlowOperationType tempType = 
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[0].ToString());

            switch (tempType)
            {
                case CE_FlowOperationType.提交:

                    _ServiceTCU.SaveInfo_TCUSoft(form.ResultList[1] as Business_Project_TCU_SoftwareUpdate,
                        form.ResultList[2] as List<View_Business_Project_TCU_SoftwareUpdate_DID>);

                    _ServiceTCU.SubmitInfo_TCUSoft(form.FlowInfo_BillNo);
                    break;
                case CE_FlowOperationType.暂存:

                    _ServiceTCU.SaveInfo_TCUSoft(form.ResultList[1] as Business_Project_TCU_SoftwareUpdate,
                        form.ResultList[2] as List<View_Business_Project_TCU_SoftwareUpdate_DID>);
                    break;
                case CE_FlowOperationType.回退:
                    break;
                case CE_FlowOperationType.未知:
                    break;
                default:
                    break;
            }

            if (!_ServiceTCU.IsExist(form.FlowInfo_BillNo))
            {
                MessageDialog.ShowPromptMessage("数据为空，保存失败，如需退出，请直接X掉界面");
                return false;
            }

            return true;
        }
    }
}
