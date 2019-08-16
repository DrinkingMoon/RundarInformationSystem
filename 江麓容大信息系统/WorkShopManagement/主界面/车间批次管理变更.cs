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
using FlowControlService;
using Service_Manufacture_WorkShop;

namespace Form_Manufacture_WorkShop
{
    public partial class 车间批次管理变更 : CustomMainForm
    {

        public 车间批次管理变更()
            : base(typeof(车间批次管理变更明细), GlobalObject.CE_BillTypeEnum.车间批次管理变更,
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IBatchNoChange>())
        {
            InitializeComponent();
        }

        private bool 培训计划申请表_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IBatchNoChange servcieChange = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IBatchNoChange>();

            try
            {
                this.OperationType = form.FlowOperationType;
                this.BillNo = form.FlowInfo_BillNo;

                Business_WorkShop_BatchNoChange changeInfo = form.ResultInfo as Business_WorkShop_BatchNoChange;

                List<View_Business_WorkShop_BatchNoChangeDetail> lstDetail = 
                    form.ResultList[0] as List<View_Business_WorkShop_BatchNoChangeDetail>;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        servcieChange.SaveInfo(changeInfo, lstDetail);
                        servcieChange.OperationBusiness(this.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        servcieChange.SaveInfo(changeInfo, lstDetail);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!servcieChange.IsExist(this.BillNo))
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
