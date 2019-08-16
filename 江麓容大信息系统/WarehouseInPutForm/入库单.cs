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
    public partial class 入库单 : CustomMainForm
    {
        /// <summary>
        /// 服务组件
        /// </summary>

        public 入库单()
            : base(typeof(入库单明细), GlobalObject.CE_BillTypeEnum.入库单,
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IInPutService>())
        {
            InitializeComponent();
        }

        private bool 入库单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IInPutService serviceInPut = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IInPutService>();

            try
            {
                List<View_Business_WarehouseInPut_InPutDetail> detailInfo =
                    form.ResultInfo as List<View_Business_WarehouseInPut_InPutDetail>;
                Business_WarehouseInPut_InPut lnqInPut = form.ResultList[0] as Business_WarehouseInPut_InPut;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqInPut.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        serviceInPut.SaveInfo(lnqInPut, detailInfo);
                        serviceInPut.FinishBill(lnqInPut.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        serviceInPut.SaveInfo(lnqInPut, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!serviceInPut.IsExist(lnqInPut.BillNo))
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
