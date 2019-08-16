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
    public partial class 纠正预防措施报告 : CustomMainForm
    {
        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _serviceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 纠正预防措施报告()
            : base(typeof(纠正预防措施报告明细), GlobalObject.CE_BillTypeEnum.纠正预防措施报告,
            Service_Quality_QC.ServerModuleFactory.GetServerModule<IEightDReport>())
        {
            InitializeComponent();
        }

        void CheckData(Bus_Quality_8DReport reportInfo)
        {
            Flow_FlowInfo info =
                _serviceFlow.GetNowFlowInfo(_serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.纠正预防措施报告, null),
                reportInfo.BillNo);

            switch (info.FlowID)
            {
                case 1109:
                    if (GeneralFunction.IsNullOrEmpty(reportInfo.Theme))
                    {
                        throw new Exception("请填写【主题】");
                    }

                    if (GeneralFunction.IsNullOrEmpty(reportInfo.GoodsInfo))
                    {
                        throw new Exception("请填写【名称及图号】");
                    }

                    if (GeneralFunction.IsNullOrEmpty(reportInfo.HappenSite))
                    {
                        throw new Exception("请填写【发生地点】");
                    }

                    if (reportInfo.HappenDate == null)
                    {
                        throw new Exception("请选择【发生时间】");
                    }

                    if (GeneralFunction.IsNullOrEmpty(reportInfo.HappenSite))
                    {
                        throw new Exception("请填写【不良数量/不良率】");
                    }

                    if (GeneralFunction.IsNullOrEmpty(reportInfo.HappenSite))
                    {
                        throw new Exception("请填写【涉及数量】");
                    }

                    if (GeneralFunction.IsNullOrEmpty(reportInfo.D1_DescribePhenomenon))
                    {
                        throw new Exception("请填写【D1描述现象】");
                    }

                    break;
                case 1112:

                    if (reportInfo.Bus_Quality_8DReport_D2_Crew == null 
                        || reportInfo.Bus_Quality_8DReport_D2_Crew.ToList().Count() == 0)
                    {
                        throw new Exception("请选择并添加【D2改善小组组员】");
                    }

                    if (GeneralFunction.IsNullOrEmpty(reportInfo.D5_ReasonAnalysis))
                    {
                        throw new Exception("请填写并添加【D5根本原因分析】");
                    }

                    if (reportInfo.Bus_Quality_8DReport_D5_Reason == null
                        || reportInfo.Bus_Quality_8DReport_D5_Reason.ToList().Where(k => k.ReasonType == "产生原因").Count() == 0)
                    {
                        throw new Exception("请填写并添加【D5产生原因】");
                    }

                    if (reportInfo.Bus_Quality_8DReport_D6_Countermeasure == null
                        || reportInfo.Bus_Quality_8DReport_D6_Countermeasure.ToList().Where(k => k.CountermeasureType == "产生对策").Count() == 0)
                    {
                        throw new Exception("请填写并添加【D6产生对策】");
                    }

                    if (reportInfo.Bus_Quality_8DReport_D7_Prevent == null
                        || reportInfo.Bus_Quality_8DReport_D7_Prevent.ToList().Count() == 0)
                    {
                        throw new Exception("请勾选并填写【D7预防措施】");
                    }

                    break;

                case 1114:


                    if (reportInfo.Bus_Quality_8DReport_D8_Verify == null
                        || reportInfo.Bus_Quality_8DReport_D8_Verify.ToList().Count() == 0)
                    {
                        throw new Exception("请填写并添加【D8效果确认】");
                    }
                    break;
                default:
                    break;
            }
        }

        private bool 纠正预防措施报告_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IEightDReport serviceEightD =
                   Service_Quality_QC.ServerModuleFactory.GetServerModule<IEightDReport>();
            try
            {
                Bus_Quality_8DReport reportInfo = form.ResultInfo as Bus_Quality_8DReport;
                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[0].ToString());
                this.BillNo = reportInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        CheckData(reportInfo);
                        serviceEightD.SaveInfo(reportInfo);
                        break;
                    case CE_FlowOperationType.暂存:
                        serviceEightD.SaveInfo(reportInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!serviceEightD.IsExist(reportInfo.BillNo))
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
