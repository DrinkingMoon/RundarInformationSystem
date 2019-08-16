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
    public partial class 判定报告 : CustomMainForm
    {
        public 判定报告()
            : base(typeof(判定报告明细), GlobalObject.CE_BillTypeEnum.判定报告,
            Service_Quality_QC.ServerModuleFactory.GetServerModule<IJudgeReport>())
        {
            InitializeComponent();
        }

        private bool 判定报告_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IJudgeReport m_serviceJudgeReport = Service_Quality_QC.ServerModuleFactory.GetServerModule<IJudgeReport>();

            try
            {
                Business_InspectionJudge_JudgeReport lnqJudgeReport =
                    form.ResultList[0] as Business_InspectionJudge_JudgeReport;

                this.OperationType =
                    GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());

                List<View_Business_InspectionJudge_JudgeReportDetail> detailInfo =
                    form.ResultList[2] as List<View_Business_InspectionJudge_JudgeReportDetail>;

                List<View_Business_InspectionJudge_JudgeReport_Item> itemInfo =
                    form.ResultList[3] as List<View_Business_InspectionJudge_JudgeReport_Item>;

                this.BillNo = lnqJudgeReport.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        m_serviceJudgeReport.SaveInfo(lnqJudgeReport, itemInfo, detailInfo);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceJudgeReport.SaveInfo(lnqJudgeReport, itemInfo, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceJudgeReport.IsExist(lnqJudgeReport.BillNo))
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
