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
    public partial class 到货单 : CustomMainForm
    {
        public 到货单()
            : base(typeof(到货单明细), GlobalObject.CE_BillTypeEnum.到货单,
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IAOGService>())
        {
            InitializeComponent();
        }

        private bool 到货单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IAOGService serviceAOG = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IAOGService>();

            try
            {
                List<View_Business_WarehouseInPut_AOGDetail> detailInfo =
                    form.ResultInfo as List<View_Business_WarehouseInPut_AOGDetail>;
                Business_WarehouseInPut_AOG lnqAOG = form.ResultList[0] as Business_WarehouseInPut_AOG;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqAOG.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        serviceAOG.SaveInfo(lnqAOG, detailInfo);
                        break;
                    case CE_FlowOperationType.暂存:
                        serviceAOG.SaveInfo(lnqAOG, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!serviceAOG.IsExist(lnqAOG.BillNo))
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
