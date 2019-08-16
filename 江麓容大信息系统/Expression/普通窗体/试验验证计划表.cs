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
    /// 试验验证计划表界面
    /// </summary>
    public partial class 试验验证计划表 : Form
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
        /// 列表信息
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 质量问题整改处置服务组件
        /// </summary>
        IQualityProblemRectificationDisposalBill m_serverQualityProblem = 
            ServerModuleFactory.GetServerModule<IQualityProblemRectificationDisposalBill>();

        /// <summary>
        /// 数据集
        /// </summary>
        ZL_ExperimentsSchedule m_lnqExperiment = new ZL_ExperimentsSchedule();

        public 试验验证计划表(string billID)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "质量问题整改处置单";

            ClearMessage();

            m_lnqExperiment = m_serverQualityProblem.GetExperimentsMessage(billID);

            lbBill_ID.Text = m_lnqExperiment.Bill_ID;

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
        }

        /// <summary>
        /// 设置信息
        /// </summary>
        void SetMessage()
        {
            txtTestCode.Text = m_lnqExperiment.TestCode;
            txtTestObjective.Text = m_lnqExperiment.TestObjective;
            txtTestPrincipal.Text = m_lnqExperiment.TestPrincipal;
            txtTestProgram.Text = m_lnqExperiment.TestProgram;

            dtpTestTime.Value = m_lnqExperiment.TestTime == null ? ServerTime.Time : Convert.ToDateTime(m_lnqExperiment.TestTime);

            lbAuditPersonnel.Text = m_lnqExperiment.AuditPersonnel;
            lbAuditTime.Text = m_lnqExperiment.AuditTime.ToString();
            lbBill_ID.Text = m_lnqExperiment.Bill_ID;
            lbPlanProducer.Text = m_lnqExperiment.PlanProducer;
            lbPlanTime.Text = m_lnqExperiment.PlanTime.ToString();

            dataGridView1.DataSource = m_serverQualityProblem.GetExperimentsScheduleList(m_lnqExperiment.Bill_ID);

            dataGridView1.Columns["试验步骤"].Width = 80;
            dataGridView1.Columns["具体步骤"].Width = 510;
            dataGridView1.Columns["完成日期"].Width = 120;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqExperiment.AuditPersonnel = lbAuditPersonnel.Text;
            m_lnqExperiment.AuditTime = lbAuditTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbAuditTime.Text);
            m_lnqExperiment.Bill_ID = lbBill_ID.Text;
            m_lnqExperiment.PlanProducer = lbPlanProducer.Text;
            m_lnqExperiment.PlanTime = lbPlanTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbPlanTime.Text);
            m_lnqExperiment.TestCode = txtTestCode.Text;
            m_lnqExperiment.TestObjective = txtTestObjective.Text;
            m_lnqExperiment.TestPrincipal = txtTestPrincipal.Text;
            m_lnqExperiment.TestProgram = txtTestProgram.Text;
            m_lnqExperiment.TestTime = dtpTestTime.Value;

            m_dtList = (DataTable)dataGridView1.DataSource;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetMessage();

            if (!m_serverQualityProblem.SaveExperimentsScheduleInfo(m_lnqExperiment,m_dtList,out m_strErr))
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

                m_billMessageServer.DestroyMessage(m_lnqExperiment.Bill_ID);
                m_billMessageServer.SendNewFlowMessage(m_lnqExperiment.Bill_ID, string.Format("{0} 号试验验证计划表，请部门主管审核", m_lnqExperiment.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户, strList);

                this.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["试验步骤"].ToString() == nudStepNumber.Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加同一步骤");
                    return;
                }
            }

            DataRow dr = dt.NewRow();

            dr["试验步骤"] = nudStepNumber.Value.ToString();
            dr["具体步骤"] = txtSpecificStep.Text;
            dr["责任人"] = txtPrinicipal.Text;
            dr["完成日期"] = dtpFinishTime.Value.ToString();

            dt.Rows.Add(dr);

            dataGridView1.DataSource = dt;

            nudStepNumber.Value = nudStepNumber.Value + 1;

            txtPrinicipal.Text = "";
            txtSpecificStep.Text = "";
            
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            int intIndex = dataGridView1.CurrentRow.Index;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != dataGridView1.CurrentRow.Index
                    && dataGridView1.Rows[i].Cells["试验步骤"].Value.ToString() == nudStepNumber.Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加同一步骤");
                    return;
                }
            }

            dataGridView1.Rows[intIndex].Cells["试验步骤"].Value = nudStepNumber.Value.ToString();
            dataGridView1.Rows[intIndex].Cells["具体步骤"].Value = txtSpecificStep.Text;
            dataGridView1.Rows[intIndex].Cells["责任人"].Value = txtPrinicipal.Text;
            dataGridView1.Rows[intIndex].Cells["完成日期"].Value = dtpFinishTime.Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            dt.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            dataGridView1.DataSource = dt;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                nudStepNumber.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["试验步骤"].Value);
                txtPrinicipal.Text = dataGridView1.CurrentRow.Cells["责任人"].Value.ToString();
                txtSpecificStep.Text = dataGridView1.CurrentRow.Cells["具体步骤"].Value.ToString();
                dtpFinishTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["完成日期"].Value);
            }
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {

            if (!m_serverQualityProblem.AuditingSundrySchedule("试验验证计划表", m_lnqExperiment.Bill_ID, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("审核成功");

                List<string> strList = new List<string>();

                strList.Add(CE_RoleEnum.质量工程师.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqExperiment.Bill_ID,
                    string.Format("{0} 号试验验证计划表已经处理完毕", m_lnqExperiment.Bill_ID),
                    strList, null);

                this.Close();
            }
        }
    }
}
