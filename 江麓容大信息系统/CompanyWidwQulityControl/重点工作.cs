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
    public partial class 重点工作 : CustomMainForm
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        IFocalWork _Service_FocalWork = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        IFlowServer _Service_Flow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 重点工作(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
            : base(typeof(重点工作明细), GlobalObject.CE_BillTypeEnum.重点工作,
            Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<Service_Peripheral_CompanyQuality.IFocalWork>())
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
        }

        void btnPublish_Click(object sender, System.EventArgs e)
        {
            发布重点工作 frm = new 发布重点工作();
            frm.ShowDialog();
        }

        private bool 重点工作_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Bus_FocalWork_MonthlyProgress lnqBillInfo = form.ResultInfo as Bus_FocalWork_MonthlyProgress;
                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[0].ToString());

                List<Bus_FocalWork_MonthlyProgress_Content> lstContent =
                    form.ResultList[1] as List<Bus_FocalWork_MonthlyProgress_Content>;
                List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyPoint =
                    form.ResultList[2] as List<Bus_FocalWork_MonthlyProgress_KeyPoint>;

                this.BillNo = lnqBillInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        _Service_FocalWork.SaveInfo(lnqBillInfo, lstContent, lstKeyPoint);
                        _Service_FocalWork.OpertionInfo(lnqBillInfo.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        _Service_FocalWork.SaveInfo(lnqBillInfo, lstContent, lstKeyPoint);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!_Service_FocalWork.IsExist(lnqBillInfo.BillNo))
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

        private void 重点工作_Load(object sender, EventArgs e)
        {
            this.ToolStripSeparator_ShowStatus(m_authorityFlag);
        }
    }
}
