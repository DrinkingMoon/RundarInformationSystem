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
using Service_Quality_File;
using Expression;
using System.IO;
using UniversalControlLibrary;

namespace Form_Quality_File
{
    public partial class 文件修订废止申请单明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillNo = "";

        public string StrBillNo
        {
            get { return m_strBillNo; }
            set { m_strBillNo = value; }
        }

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// LINQ数据集
        /// </summary>
        FM_RevisedAbolishedBill m_lnqRevisedAbolishedBill = new FM_RevisedAbolishedBill();

        /// <summary>
        /// 文件修订废止申请单服务组件
        /// </summary>
        ISystemFileRevisedAbolishedBill m_serverRevisedAbolishedBill = 
            Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileRevisedAbolishedBill>();

        /// <summary>
        /// 文件基础信息服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 回退流程
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lbBillStatus.Text != "单据已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.文件修订废止申请单, txtBillNo.Text, lbBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您确定要回退此单据吗？") == DialogResult.Yes)
                    {
                        GetData();

                        if (m_serverRevisedAbolishedBill.ReturnBill(form.StrBillID,
                            form.StrBillStatus, m_lnqRevisedAbolishedBill, out m_strErr, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");

                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strErr);
                        }
                    }

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        /// <summary>
        /// 流程控制
        /// </summary>
        void FlowControl()
        {
            bool visible = UniversalFunction.IsOperator(m_strBillNo);

            switch (lbBillStatus.Text)
            {
                case "新建单据":
                    btnAdd.Visible = true;
                    break;
                case "等待审核":
                    btnAudit.Visible = visible;
                    break;
                case "等待批准":
                    btnApprove.Visible = visible;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            lbAuditor.Text = "";
            lbAuditorTime.Text = "";
            lbApprover.Text = "";
            lbApproverTime.Text = "";
            lbPropoer.Text = "";
            lbPropoerTime.Text = "";
            lbBillStatus.Text = "";
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetData()
        {
            m_lnqRevisedAbolishedBill = new FM_RevisedAbolishedBill();

            m_lnqRevisedAbolishedBill.FileID = Convert.ToInt32(txtFileNo.Tag);
            m_lnqRevisedAbolishedBill.AuditorAdvise = txtAuditorAdvise.Text;
            m_lnqRevisedAbolishedBill.ApproverAdvise = txtApproverAdvise.Text;
            m_lnqRevisedAbolishedBill.AuditorAdvise = txtAuditorAdvise.Text;
            m_lnqRevisedAbolishedBill.ProposeContent = txtProposeContent.Text;
            m_lnqRevisedAbolishedBill.BillNo = txtBillNo.Text;
            m_lnqRevisedAbolishedBill.BillStatus = lbBillStatus.Text;
            m_lnqRevisedAbolishedBill.OperationFlag = radioButton2.Checked;
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowData()
        {
            FM_FileList lnqTemp = m_serverFileBasicInfo.GetFile(m_lnqRevisedAbolishedBill.FileID);

            txtFileNo.Text = lnqTemp.FileNo;
            txtFileName.Text = lnqTemp.FileName;
            txtVersion.Text = lnqTemp.Version;
            txtFileNo.Tag = lnqTemp.FileID;
            txtProposeContent.Text = m_lnqRevisedAbolishedBill.ProposeContent;
            txtAuditorAdvise.Text = m_lnqRevisedAbolishedBill.AuditorAdvise;
            txtApproverAdvise.Text = m_lnqRevisedAbolishedBill.ApproverAdvise;
            txtBillNo.Text = m_lnqRevisedAbolishedBill.BillNo;

            lbApprover.Text = m_lnqRevisedAbolishedBill.Approver;
            lbApproverTime.Text = m_lnqRevisedAbolishedBill.ApproverTime.ToString();
            lbAuditor.Text = m_lnqRevisedAbolishedBill.Auditor;
            lbAuditorTime.Text = m_lnqRevisedAbolishedBill.AuditorTime.ToString();
            lbPropoer.Text = m_lnqRevisedAbolishedBill.Propose;
            lbPropoerTime.Text = m_lnqRevisedAbolishedBill.ProposeTime.ToString();
            lbBillStatus.Text = m_lnqRevisedAbolishedBill.BillStatus;

            if (m_lnqRevisedAbolishedBill.OperationFlag)
            {
                radioButton2.Checked = true;
                radioButton1.Checked = false;
            }
            else
            {
                radioButton2.Checked = false;
                radioButton1.Checked = true;
            }
        }

        public 文件修订废止申请单明细(string billNo)
        {
            InitializeComponent();

            m_strBillNo = billNo;
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.文件修订废止申请单.ToString(), m_serverRevisedAbolishedBill);
            m_billMessageServer.BillType = CE_BillTypeEnum.文件修订废止申请单.ToString();
        }

        private void 文件修订废止申请单明细_Load(object sender, EventArgs e)
        {
            ClearData();

            if (m_strBillNo == null)
            {
                txtBillNo.Text = m_billNoControl.GetNewBillNo();
                lbBillStatus.Text = "新建单据";
            }
            else
            {
                m_lnqRevisedAbolishedBill = m_serverRevisedAbolishedBill.GetInfo(m_strBillNo);
                ShowData();
            }

            FlowControl();
        }

        private void 文件修订废止申请单明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            GetData();

            if (m_lnqRevisedAbolishedBill.BillStatus == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            MessageDialog.ShowPromptMessage("请选择下一步流程操作人员");

            FormSelectPersonnel2 frm = new FormSelectPersonnel2();

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (!m_serverRevisedAbolishedBill.AddInfo(m_lnqRevisedAbolishedBill, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {

                m_strBillNo = txtBillNo.Text;

                BillFlowMessage_ReceivedUserType userType =
                    GlobalObject.GeneralFunction.StringConvertToEnum<BillFlowMessage_ReceivedUserType>(frm.SelectedNotifyPersonnelInfo.UserType);

                List<string> notifyList = new List<string>();

                foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                {
                    switch (userType)
                    {
                        case BillFlowMessage_ReceivedUserType.用户:
                            notifyList.Add(pbi.工号);
                            break;
                        case BillFlowMessage_ReceivedUserType.角色:
                            notifyList.Add(pbi.角色);
                            break;
                        default:
                            break;
                    }
                }

                m_billMessageServer.DestroyMessage(m_lnqRevisedAbolishedBill.BillNo);
                m_billMessageServer.SendNewFlowMessage(m_lnqRevisedAbolishedBill.BillNo,
                    string.Format("{0}号文件修订废止申请单已提交，请审核", m_lnqRevisedAbolishedBill.BillNo),
                    userType, notifyList);

                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {

            GetData();

            if (m_lnqRevisedAbolishedBill.BillStatus != "等待审核")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            MessageDialog.ShowPromptMessage("请选择下一步流程操作人员");

            FormSelectPersonnel2 frm = new FormSelectPersonnel2();

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            if (!m_serverRevisedAbolishedBill.AuditInfo(m_lnqRevisedAbolishedBill, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {

                List<string> list = new List<string>();

                foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                {
                    if (pbi.工号 != null && pbi.工号.Length > 0)
                    {
                        list.Add(pbi.工号);
                    }
                }

                m_billMessageServer.PassFlowMessage(m_lnqRevisedAbolishedBill.BillNo,
                    string.Format("{0}号文件发布流程已审核，请批准", m_lnqRevisedAbolishedBill.BillNo),
                    BillFlowMessage_ReceivedUserType.用户, list);

                MessageDialog.ShowPromptMessage("审核成功");
                this.Close();
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            GetData();

            if (m_lnqRevisedAbolishedBill.BillStatus != "等待批准")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            if (!m_serverRevisedAbolishedBill.ApproveInfo(m_lnqRevisedAbolishedBill, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                m_lnqRevisedAbolishedBill = m_serverRevisedAbolishedBill.GetInfo(m_lnqRevisedAbolishedBill.BillNo);

                List<string> list = new List<string>();

                list.Add(UniversalFunction.GetPersonnelCode(m_lnqRevisedAbolishedBill.Auditor));
                list.Add(UniversalFunction.GetPersonnelCode(m_lnqRevisedAbolishedBill.Propose));

                m_billMessageServer.EndFlowMessage(m_lnqRevisedAbolishedBill.BillNo,
                    string.Format("{0}号文件发布流程已结束", m_lnqRevisedAbolishedBill.BillNo), null, list);
                MessageDialog.ShowPromptMessage("批准成功");

                this.Close();
            }
        }

        private void txtFileNo_Enter(object sender, EventArgs e)
        {
            txtFileNo.StrEndSql = " and 类别ID not in (10,11)";
        }

        private void txtFileNo_OnCompleteSearch()
        {
            txtFileName.Text = txtFileNo.DataResult["文件名称"].ToString();
            txtFileNo.Text = txtFileNo.DataResult["文件编号"].ToString();
            txtVersion.Text = txtFileNo.DataResult["版本号"].ToString();
            txtFileNo.Tag = Convert.ToInt32(txtFileNo.DataResult["文件ID"]);
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            if (lbBillStatus.Text == "等待审核" && btnAudit.Visible == true)
            {
                ReturnBillStatus();
            }
            else if (lbBillStatus.Text == "等待批准" && btnApprove.Visible == true)
            {
                ReturnBillStatus();
            }
        }
    }
}
