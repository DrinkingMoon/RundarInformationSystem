using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Manufacture_Storage;
using FlowControlService;

namespace Form_Manufacture_Storage
{
    public partial class 发料清单编制主界面 : CustomMainForm
    {
        /// <summary>
        /// 发料清单服务组件
        /// </summary>
        Service_Manufacture_Storage.IProductOrder m_findProductOrder = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 发料清单编制主界面()
            : base(typeof(发料清单编制明细界面), GlobalObject.CE_BillTypeEnum.发料清单,
                Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IProductOrder>())
        {
            InitializeComponent();
            this.Form_CommonProcessSubmit += new FormCommonProcess.FormSubmit(发料清单编制主界面_Form_CommonProcessSubmit);
        }

        bool 发料清单编制主界面_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                List<View_S_DebitSchedule> listInfo = form.ResultInfo as List<View_S_DebitSchedule>;

                CE_FlowOperationType flowOperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[0].ToString());
                string edition = form.ResultList[1].ToString();
                CE_DebitScheduleApplicable applicable = GeneralFunction.StringConvertToEnum<CE_DebitScheduleApplicable>(form.ResultList[2].ToString());

                switch (flowOperationType)
                {
                    case CE_FlowOperationType.提交:
                        m_findProductOrder.SaveInfo(listInfo, form.FlowInfo_BillNo, edition, applicable);
                        m_findProductOrder.OperationInfo(listInfo, form.FlowInfo_BillNo, edition, applicable);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_findProductOrder.SaveInfo(listInfo, form.FlowInfo_BillNo, edition, applicable);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_findProductOrder.IsExist(form.FlowInfo_BillNo))
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
