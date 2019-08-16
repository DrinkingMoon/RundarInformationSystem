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
    public partial class 新品开发计划表 : Form
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
        ZL_NewProductDevelopmentSchedule m_lnqNewProductDevelopment = new ZL_NewProductDevelopmentSchedule();

        public 新品开发计划表(string billID)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "质量问题整改处置单";

            ClearMessage();

            m_lnqNewProductDevelopment = m_serverQualityProblem.GetNewProductDevelopmentMessage(billID);

            lbBill_ID.Text = m_lnqNewProductDevelopment.Bill_ID;

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
            txtGoodsCode.Text = m_lnqNewProductDevelopment.GoodsCode;
            txtGoodsName.Text = m_lnqNewProductDevelopment.GoodsName;
            txtDevelopmentReason.Text = m_lnqNewProductDevelopment.DevelopmentReason;

            lbAuditPersonnel.Text = m_lnqNewProductDevelopment.AuditPersonnel;
            lbAuditTime.Text = m_lnqNewProductDevelopment.AuditTime.ToString();
            lbBill_ID.Text = m_lnqNewProductDevelopment.Bill_ID;
            lbPlanProducer.Text = m_lnqNewProductDevelopment.PlanProducer;
            lbPlanTime.Text = m_lnqNewProductDevelopment.PlanTime.ToString();

            dataGridView1.DataSource = m_serverQualityProblem.GetNewProductDevelopmentScheduleList(m_lnqNewProductDevelopment.Bill_ID);

            dataGridView1.Columns["开发过程"].Width = 80;
            dataGridView1.Columns["工作事项"].Width = 510;
            dataGridView1.Columns["完成日期"].Width = 120;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqNewProductDevelopment.DevelopmentReason = txtDevelopmentReason.Text;
            m_lnqNewProductDevelopment.GoodsCode = txtGoodsCode.Text;
            m_lnqNewProductDevelopment.GoodsName = txtGoodsName.Text;

            m_lnqNewProductDevelopment.AuditPersonnel = lbAuditPersonnel.Text;
            m_lnqNewProductDevelopment.AuditTime = lbAuditTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbAuditTime.Text);
            m_lnqNewProductDevelopment.Bill_ID = lbBill_ID.Text;
            m_lnqNewProductDevelopment.PlanProducer = lbPlanProducer.Text;
            m_lnqNewProductDevelopment.PlanTime = lbPlanTime.Text == "" ? ServerTime.Time : Convert.ToDateTime(lbPlanTime.Text);

            m_dtList = (DataTable)dataGridView1.DataSource;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            GetMessage();

            if (!m_serverQualityProblem.SaveNewProductDevelopmentScheduleInfo(m_lnqNewProductDevelopment, m_dtList, out m_strErr))
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

                m_billMessageServer.DestroyMessage(m_lnqNewProductDevelopment.Bill_ID);
                m_billMessageServer.SendNewFlowMessage(m_lnqNewProductDevelopment.Bill_ID, string.Format("{0} 号新品开发计划表，请部门主管审核", m_lnqNewProductDevelopment.Bill_ID),
                    BillFlowMessage_ReceivedUserType.用户, strList);

                this.Close();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["开发过程"].ToString() == nudDevelopmentStepNumber.Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加同一个开发步骤");
                    return;
                }
            }

            DataRow dr = dt.NewRow();

            dr["开发过程"] = nudDevelopmentStepNumber.Value.ToString();
            dr["工作事项"] = txtAgenda.Text;
            dr["责任人"] = txtPrinicipal.Text;
            dr["完成日期"] = dtpFinishTime.Value.ToString();

            dt.Rows.Add(dr);

            dataGridView1.DataSource = dt;

            nudDevelopmentStepNumber.Value = nudDevelopmentStepNumber.Value + 1;

            txtPrinicipal.Text = "";
            txtAgenda.Text = "";
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int intIndex = dataGridView1.CurrentRow.Index;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != dataGridView1.CurrentRow.Index
                    && dataGridView1.Rows[i].Cells["开发过程"].Value.ToString() == nudDevelopmentStepNumber.Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("不能添加同一个开发步骤");
                    return;
                }
            }

            dataGridView1.Rows[intIndex].Cells["开发过程"].Value = nudDevelopmentStepNumber.Value.ToString();
            dataGridView1.Rows[intIndex].Cells["工作事项"].Value = txtAgenda.Text;
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
                nudDevelopmentStepNumber.Value = Convert.ToInt32(dataGridView1.CurrentRow.Cells["开发过程"].Value);
                txtPrinicipal.Text = dataGridView1.CurrentRow.Cells["责任人"].Value.ToString();
                txtAgenda.Text = dataGridView1.CurrentRow.Cells["工作事项"].Value.ToString();
                dtpFinishTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["完成日期"].Value);
            }
        }

        private void btnAuditing_Click(object sender, EventArgs e)
        {
            if (!m_serverQualityProblem.AuditingSundrySchedule("新品开发计划表", m_lnqNewProductDevelopment.Bill_ID, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("审核成功");

                List<string> strList = new List<string>();

                strList.Add(CE_RoleEnum.质量工程师.ToString());

                m_billMessageServer.EndFlowMessage(m_lnqNewProductDevelopment.Bill_ID,
                    string.Format("{0} 号新品开发计划表已经处理完毕", m_lnqNewProductDevelopment.Bill_ID),
                    strList, null);

                this.Close();
            }
        }
    }
}
