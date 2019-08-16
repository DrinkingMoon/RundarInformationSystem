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
    public partial class IT运维申请单 : CustomMainForm
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IComputerMalfunction m_mainService =
            Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IComputerMalfunction>();

        public IT运维申请单()
            : base(typeof(IT运维申请单明细), GlobalObject.CE_BillTypeEnum.IT运维申请单,
            Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IComputerMalfunction>())
        {
            InitializeComponent();
        }

        private bool IT运维申请单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Business_Composite_ComputerMalfunction lnqSaveInfo = form.ResultList[0] as Business_Composite_ComputerMalfunction;
                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqSaveInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                    case CE_FlowOperationType.暂存:
                        m_mainService.SaveInfo(lnqSaveInfo);
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
    }
}
