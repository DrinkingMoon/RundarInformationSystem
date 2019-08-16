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
using Service_Manufacture_Storage;
using FlowControlService;

namespace Form_Manufacture_Storage
{
    public partial class 入库申请单 : CustomMainForm
    {
        public 入库申请单()
            : base(typeof(入库申请单明细), GlobalObject.CE_BillTypeEnum.入库申请单, 
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IRequisitionService_InPut>())
        {
            InitializeComponent();
        }

        private bool 入库申请单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IRequisitionService_InPut serviceRequistion =
                Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IRequisitionService_InPut>();

            try
            {
                List<View_Business_WarehouseInPut_RequisitionDetail> detailInfo =
                    form.ResultInfo as List<View_Business_WarehouseInPut_RequisitionDetail>;
                Business_WarehouseInPut_Requisition lnqRequisition = form.ResultList[0] as Business_WarehouseInPut_Requisition;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqRequisition.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        serviceRequistion.SaveInfo(lnqRequisition, detailInfo);
                        break;
                    case CE_FlowOperationType.暂存:
                        serviceRequistion.SaveInfo(lnqRequisition, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!serviceRequistion.IsExist(lnqRequisition.BillNo))
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
