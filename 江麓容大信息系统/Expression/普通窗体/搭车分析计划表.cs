using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 搭车分析计划表
    /// </summary>
    public partial class 搭车分析计划表 : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 质量问题整改处置服务组件
        /// </summary>
        IQualityProblemRectificationDisposalBill m_serverQualityProblem =
            ServerModuleFactory.GetServerModule<IQualityProblemRectificationDisposalBill>();

        /// <summary>
        /// 数据集
        /// </summary>
        ZL_AssemblingAnalysisSchedule m_lnqAssemblingAnalysis = new ZL_AssemblingAnalysisSchedule();

        public 搭车分析计划表(string billID)
        {
            InitializeComponent();

            m_billMessageServer.BillType = CE_BillTypeEnum.质量问题整改处置单.ToString();

            ClearMessage();

            m_lnqAssemblingAnalysis = m_serverQualityProblem.GetAssemblingAnalysisMessage(billID);

            lbBill_ID.Text = m_lnqAssemblingAnalysis.Bill_ID;

            SetMessage();

            if (lbPlanProducer.Text != "")
            {
                string strDepartment = m_serverDepartment.GetDeptInfoFromPersonnelInfo(lbPlanProducer.Text).Rows[0]["DepartmentCode"].ToString();

                IQueryable<View_HR_Personnel> iqList = m_serverPersonnel.GetDeptDirector(strDepartment);

                foreach (var item in iqList)
                {
                    if (item.工号 == BasicInfo.LoginID)
                    {
                        btnAuditing.Visible = true;
                        toolStripSeparator2.Visible = true;
                    }
                }
            }

            if (txtPrincipal.Text == BasicInfo.LoginName)
            {
                btnSaveAnalysisResult.Visible = true;
                toolStripSeparator4.Visible = true;
            }

            if (lbAnalysisPersonnel.Text != "")
            {
                string strDepartment = m_serverDepartment.GetDeptInfoFromPersonnelInfo(lbAnalysisPersonnel.Text).Rows[0]["DepartmentCode"].ToString();

                IQueryable<View_HR_Personnel> iqList = m_serverPersonnel.GetDeptDirector(strDepartment);

                foreach (var item in iqList)
                {
                    if (item.工号 == BasicInfo.LoginID)
                    {
                        btnAuditingAnalysisResult.Visible = true;
                        toolStripSeparator3.Visible = true;
                    }
                }
            }
        }


        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearMessage()
        {
            lbAuditPersonnel.Text = "";
            lbAuditTime.Text = "";
            lbPlanProducer.Text = "";
            lbPlanTime.Text = "";
            lbAnalysisAuditPersonnel.Text = "";
            lbAnalysisAuditTime.Text = "";
            lbAnalysisPersonnel.Text = "";
            lbAnalysisTime.Text = "";
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        void SetMessage()
        {
            txtAnalysisDepartment.Text = m_lnqAssemblingAnalysis.AnalysisDepartment;
            txtAnalysisResult.Text = m_lnqAssemblingAnalysis.AnalysisResult;
            txtAssemblingClaim.Text = m_lnqAssemblingAnalysis.AssemblingClaim;
            txtFaultCode.Text = m_lnqAssemblingAnalysis.FaultCode;
            txtFaultDataReplay.Text = m_lnqAssemblingAnalysis.FaultDataReplay;
            txtFeedbackNumber.Text = m_lnqAssemblingAnalysis.FeedbackNumber;
            txtMileage.Text = m_lnqAssemblingAnalysis.Mileage;
            txtPrincipal.Text = m_lnqAssemblingAnalysis.Principal;
            txtProductCode.Text = m_lnqAssemblingAnalysis.ProductCode;
            txtProductType.Text = m_lnqAssemblingAnalysis.ProductType;
            txtSpecificFault.Text = m_lnqAssemblingAnalysis.SpecificFault;
            txtTestBedResults.Text = m_lnqAssemblingAnalysis.TestBedResults;

            dtpFinishTimeClaim.Value = m_lnqAssemblingAnalysis.FinishTimeClaim == null ? ServerTime.Time : Convert.ToDateTime(m_lnqAssemblingAnalysis.FinishTimeClaim);

            lbAnalysisAuditPersonnel.Text = m_lnqAssemblingAnalysis.AnalysisAuditPersonnel;
            lbAnalysisAuditTime.Text = m_lnqAssemblingAnalysis.AnalysisAuditTime.ToString();
            lbAnalysisPersonnel.Text = m_lnqAssemblingAnalysis.AnalysisPersonnel;
            lbAnalysisTime.Text = m_lnqAssemblingAnalysis.AnalysisTime.ToString();
            lbAuditPersonnel.Text = m_lnqAssemblingAnalysis.AuditPersonnel;
            lbAuditTime.Text = m_lnqAssemblingAnalysis.AuditTime.ToString();
            lbPlanProducer.Text = m_lnqAssemblingAnalysis.PlanProducer;
            lbPlanTime.Text = m_lnqAssemblingAnalysis.PlanTime.ToString();
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqAssemblingAnalysis.AnalysisAuditPersonnel = lbAnalysisAuditPersonnel.Text;
            m_lnqAssemblingAnalysis.AnalysisAuditTime = lbAnalysisAuditTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbAnalysisAuditTime.Text);
            m_lnqAssemblingAnalysis.AnalysisDepartment = txtAnalysisDepartment.Text;
            m_lnqAssemblingAnalysis.AnalysisPersonnel = lbAnalysisPersonnel.Text;
            m_lnqAssemblingAnalysis.AnalysisResult = txtAnalysisResult.Text;
            m_lnqAssemblingAnalysis.AnalysisTime = lbAnalysisTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbAnalysisTime.Text);
            m_lnqAssemblingAnalysis.AssemblingClaim = txtAssemblingClaim.Text;
            m_lnqAssemblingAnalysis.AuditPersonnel = lbAuditPersonnel.Text;
            m_lnqAssemblingAnalysis.AuditTime = lbAuditTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbAuditTime.Text);
            m_lnqAssemblingAnalysis.FaultCode = txtFaultCode.Text;
            m_lnqAssemblingAnalysis.FaultDataReplay = txtFaultDataReplay.Text;
            m_lnqAssemblingAnalysis.FeedbackNumber = txtFeedbackNumber.Text;
            m_lnqAssemblingAnalysis.FinishTimeClaim = dtpFinishTimeClaim.Value;
            m_lnqAssemblingAnalysis.Mileage = txtMileage.Text;
            m_lnqAssemblingAnalysis.PlanProducer = lbPlanProducer.Text;
            m_lnqAssemblingAnalysis.PlanTime = lbPlanTime.Text == null ? ServerTime.Time : Convert.ToDateTime(lbPlanTime.Text);
            m_lnqAssemblingAnalysis.Principal = txtPrincipal.Text;
            m_lnqAssemblingAnalysis.ProductCode = txtProductCode.Text;
            m_lnqAssemblingAnalysis.ProductType = txtProductType.Text;
            m_lnqAssemblingAnalysis.SpecificFault = txtSpecificFault.Text;
            m_lnqAssemblingAnalysis.TestBedResults = txtTestBedResults.Text;

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverQualityProblem.SaveAssemblingAnalysisScheduleInfo(m_lnqAssemblingAnalysis,  out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");

                List<string> strList = new List<string>();

                string strDepartment = m_serverDepartment.GetDeptInfoFromPersonnelInfo(BasicInfo.LoginID).Rows[0]["DepartmentCode"].ToString();

                IQueryable<View_HR_Personnel> iqList = m_serverPersonnel.GetDeptDirector(strDepartment);

                foreach (var item in iqList)
                {
                    strList.Add(item.工号);
                }

                m_billMessageServer.DestroyMessage(m_lnqAssemblingAnalysis.Bill_ID);
                m_billMessageServer.SendNewFlowMessage(m_lnqAssemblingAnalysis.Bill_ID, string.Format("{0} 号搭车分析计划表，请部门主管审核", m_lnqAssemblingAnalysis.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户, strList);

                this.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {
            if (!m_serverQualityProblem.AuditingSundrySchedule("搭车分析计划表", m_lnqAssemblingAnalysis.Bill_ID, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("审核成功");

                IQueryable<View_HR_Personnel> listPersonnel = m_serverPersonnel.GetPersonnelViewInfo(ServerModule.PersonnelDefiniens.ParameterType.姓名, txtPrincipal.Text);

                m_billMessageServer.PassFlowMessage(m_lnqAssemblingAnalysis.Bill_ID, string.Format("{0} 号搭车分析计划表，请责任人提交分析结果", m_lnqAssemblingAnalysis.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户, listPersonnel.First().工号);

                this.Close();
            }
        }

        private void btnSaveAnalysisResult_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverQualityProblem.SaveAnalysisResult(m_lnqAssemblingAnalysis, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交分析结果成功");

                List<string> strList = new List<string>();

                string strDepartment = m_serverDepartment.GetDeptInfoFromPersonnelInfo(BasicInfo.LoginID).Rows[0]["DepartmentCode"].ToString();

                IQueryable<View_HR_Personnel> iqList = m_serverPersonnel.GetDeptDirector(strDepartment);

                foreach (var item in iqList)
                {
                    strList.Add(item.工号);
                }

                m_billMessageServer.PassFlowMessage(m_lnqAssemblingAnalysis.Bill_ID, string.Format("{0} 号搭车分析计划表，请部门主管审核", m_lnqAssemblingAnalysis.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户, strList);

                this.Close();
            }
        }

        private void btnAuditingAnalysisResult_Click(object sender, EventArgs e)
        {
            if (!m_serverQualityProblem.AuditingAnalysisResult(m_lnqAssemblingAnalysis.Bill_ID, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("审核分析结果成功");

                List<string> strList = new List<string>();

                strList.Add(CE_RoleEnum.质量工程师.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqAssemblingAnalysis.Bill_ID,
                    string.Format("{0} 号搭车分析计划表已经处理完毕", m_lnqAssemblingAnalysis.Bill_ID),
                    strList, null);

                this.Close();
            }
        }
    }
}
