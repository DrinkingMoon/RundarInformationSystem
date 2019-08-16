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
using Service_Quality_QC;
using FlowControlService;


namespace Form_Quality_QC
{
    public partial class 检验报告 : CustomMainForm
    {
        public 检验报告()
            : base(typeof(检验报告明细), GlobalObject.CE_BillTypeEnum.检验报告,
            Service_Quality_QC.ServerModuleFactory.GetServerModule<IInspectionReportService>())
        {
            InitializeComponent();
        }

        private bool 检验报告_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IInspectionReportService serviceInspectionReport =
                Service_Quality_QC.ServerModuleFactory.GetServerModule<IInspectionReportService>();

            try
            {
                List<View_Business_InspectionJudge_InspectionReport_Item> detailInfo =
                    form.ResultInfo as List<View_Business_InspectionJudge_InspectionReport_Item>;
                Business_InspectionJudge_InspectionReport lnqInspectionReport = form.ResultList[0] as Business_InspectionJudge_InspectionReport;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqInspectionReport.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        serviceInspectionReport.SaveInfo(lnqInspectionReport, detailInfo);
                        break;
                    case CE_FlowOperationType.暂存:
                        serviceInspectionReport.SaveInfo(lnqInspectionReport, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!serviceInspectionReport.IsExist(lnqInspectionReport.BillNo))
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
