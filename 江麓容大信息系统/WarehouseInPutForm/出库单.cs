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
    public partial class 出库单 : CustomMainForm
    {
        public 出库单()
            : base(typeof(出库单明细), GlobalObject.CE_BillTypeEnum.出库单, 
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IOutPutService>())
        {
            InitializeComponent();
        }

        bool 出库单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IOutPutService serviceOutPut = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IOutPutService>();

            try
            {
                List<View_Business_WarehouseOutPut_OutPutDetail> detailInfo =
                    form.ResultInfo as List<View_Business_WarehouseOutPut_OutPutDetail>;
                Business_WarehouseOutPut_OutPut lnqOutPut = form.ResultList[0] as Business_WarehouseOutPut_OutPut;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqOutPut.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        serviceOutPut.SaveInfo(lnqOutPut, detailInfo);
                        serviceOutPut.FinishBill(lnqOutPut.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        serviceOutPut.SaveInfo(lnqOutPut, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!serviceOutPut.IsExist(lnqOutPut.BillNo))
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
