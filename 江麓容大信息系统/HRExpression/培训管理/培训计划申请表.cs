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
using Service_Peripheral_HR;

namespace Form_Peripheral_HR
{
    public partial class 培训计划申请表 : CustomMainForm
    {
        public 培训计划申请表()
            : base(typeof(培训计划申请表明细), GlobalObject.CE_BillTypeEnum.培训计划申请表,
            Service_Peripheral_HR.ServerModuleFactory.GetServerModule<Service_Peripheral_HR.ITrainSurvey>())
        {
            InitializeComponent();
        }

        private bool 培训计划申请表_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            ITrainSurvey servcieSurvey = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainSurvey>();

            try
            {
                this.OperationType = form.FlowOperationType;
                this.BillNo = form.FlowInfo_BillNo;

                HR_Train_Plan planInfo = form.ResultInfo as HR_Train_Plan;

                List<View_HR_Train_PlanCourse> lstCourseInfo = form.ResultList[0] as List<View_HR_Train_PlanCourse>;
                List<View_HR_Train_PlanUser> lstUser = form.ResultList[1] as List<View_HR_Train_PlanUser>;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        servcieSurvey.SaveInfo(planInfo, lstCourseInfo, lstUser);
                        servcieSurvey.OperationBusiness(this.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        servcieSurvey.SaveInfo(planInfo, lstCourseInfo, lstUser);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!servcieSurvey.IsExist(this.BillNo))
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
