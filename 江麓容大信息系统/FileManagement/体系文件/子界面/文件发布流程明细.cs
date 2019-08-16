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
using Form_Quality_File.Properties;

namespace Form_Quality_File
{
    public partial class 文件发布流程明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 流程编号
        /// </summary>
        private string m_strSDBNo = "";

        public string StrSDBNo
        {
            get { return m_strSDBNo; }
            set { m_strSDBNo = value; }
        }

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 文件基础信息服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 文件发布流程服务组件
        /// </summary>
        ISystemFileReleaseProcess m_serverReleaseProcess = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileReleaseProcess>();

        /// <summary>
        /// LNQ数据集
        /// </summary>
        FM_ReleaseProcess m_lnqReleaseProcess = new FM_ReleaseProcess();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        /// <summary>
        /// 回退流程
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lbSDBStatus.Text != "流程已结束")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.文件发布流程, txtSDBNo.Text, lbSDBStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您确定要回退此单据吗？") == DialogResult.Yes)
                    {
                        GetData();

                        if (m_serverReleaseProcess.ReturnBill(form.StrBillID,
                            form.StrBillStatus, m_lnqReleaseProcess, out m_strErr, form.Reason))
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
            bool visible = UniversalFunction.IsOperator(m_strSDBNo);

            switch (lbSDBStatus.Text)
            {
                case "新建流程":
                    btnAdd.Visible = true;
                    llbProposerUpLoad.Visible = true;
                    break;
                case "等待审核":
                    btnAudit.Visible = visible;
                    break;
                case "等待批准":
                    btnApprove.Visible = visible;
                    break;
                case "流程已结束":
                    llbProposerDownLoad.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 获得FTP错误信息
        /// </summary>
        bool GetError()
        {
            if (m_serverFTP.Errormessage.Length != 0)
            {
                MessageDialog.ShowPromptMessage(m_serverFTP.Errormessage);
                return false;
            }
            else
            {
                return true;
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
            lbSDBStatus.Text = "";

            llbProposerDownLoad.Text = "";

            llbProposerDownLoad.Tag = "";
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetData()
        {
            m_lnqReleaseProcess = new FM_ReleaseProcess();

            m_lnqReleaseProcess.FileName = txtFileName.Text;
            m_lnqReleaseProcess.FileNo = txtFileNo.Text;

            if (llbProposerUpLoad.Tag != null)
            {
                m_lnqReleaseProcess.FileUnique = new Guid(llbProposerUpLoad.Tag.ToString());
            }

            m_lnqReleaseProcess.SortID = (int)txtSortName.Tag;
            m_lnqReleaseProcess.Department = txtDepartment.Tag.ToString();
            m_lnqReleaseProcess.ApproverAdvise = txtApproverAdvise.Text;
            m_lnqReleaseProcess.AuditorAdvise = txtAuditorAdvise.Text;
            m_lnqReleaseProcess.Remark = txtRemark.Text;
            m_lnqReleaseProcess.SDBNo = txtSDBNo.Text;
            m_lnqReleaseProcess.SDBStatus = lbSDBStatus.Text;
            m_lnqReleaseProcess.Version = txtVersion.Text;

            Nullable<int> nullInt = null;

            m_lnqReleaseProcess.ReplaceFileID = txtReplaceFileNo.Tag == null ? nullInt : Convert.ToInt32(txtReplaceFileNo.Tag);
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowData()
        {
            if (m_lnqReleaseProcess.ReplaceFileID != null)
            {
                FM_FileList lnqTemp = m_serverFileBasicInfo.GetFile(Convert.ToInt32(m_lnqReleaseProcess.ReplaceFileID));

                txtReplaceFileNo.Tag = lnqTemp.FileID;
                txtReplaceFileNo.Text = lnqTemp.FileNo;
                txtReplaceFileName.Text = lnqTemp.FileName;
                txtRepalceVersion.Text = lnqTemp.Version;
            }

            txtRemark.Text = m_lnqReleaseProcess.Remark;
            txtFileNo.Text = m_lnqReleaseProcess.FileNo;
            txtFileName.Text = m_lnqReleaseProcess.FileName;
            txtDepartment.Text = m_serverDepartment.GetDepartmentName(m_lnqReleaseProcess.Department);
            txtDepartment.Tag = m_lnqReleaseProcess.Department;
            txtSortName.Tag = m_lnqReleaseProcess.SortID;
            txtSortName.Text = m_serverFileBasicInfo.SortInfo(Convert.ToInt32(m_lnqReleaseProcess.SortID)).SortName;
            txtAuditorAdvise.Text = m_lnqReleaseProcess.AuditorAdvise;
            txtApproverAdvise.Text = m_lnqReleaseProcess.ApproverAdvise;
            txtSDBNo.Text = m_lnqReleaseProcess.SDBNo;
            txtVersion.Text = m_lnqReleaseProcess.Version;

            lbApprover.Text = m_lnqReleaseProcess.Approver;
            lbApproverTime.Text = m_lnqReleaseProcess.ApproverTime.ToString();
            lbAuditor.Text = m_lnqReleaseProcess.Auditor;
            lbAuditorTime.Text = m_lnqReleaseProcess.AuditorTime.ToString();
            lbPropoer.Text = m_lnqReleaseProcess.Propoer;
            lbPropoerTime.Text = m_lnqReleaseProcess.PropoerTime.ToString();
            lbSDBStatus.Text = m_lnqReleaseProcess.SDBStatus;

            llbProposerDownLoad.Text = m_lnqReleaseProcess == null ? "" : lbPropoer.Text + "的文件";

            llbProposerDownLoad.Tag = m_lnqReleaseProcess.FileUnique.ToString();
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        string Update(string mode)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    Guid guid = new Guid();
                    string fileType = openFileDialog1.FileName.Substring(openFileDialog1.FileName.LastIndexOf("."));
                    string strFtpServerPath = "/" + ServerTime.Time.Year.ToString() + "/" + ServerTime.Time.Month.ToString() + "/";

                    if (!GlobalObject.FileTypeRecognition.IsWordDocument(openFileDialog1.FileName))
                    {
                        throw new Exception("此文件非正常WORD文件，可能由于文件格式无法识别或者文件加密造成无法上传");
                    }

                    switch (mode)
                    {
                        case "Propoer":

                            if (llbProposerDownLoad.Tag == null || llbProposerDownLoad.Tag.ToString().Trim().Length == 0)
                            {
                                guid = Guid.NewGuid();
                                m_serverFileBasicInfo.AddFile(guid, strFtpServerPath + guid.ToString(), fileType);
                            }
                            else
                            {
                                guid = new Guid(llbProposerDownLoad.Tag.ToString());
                                m_serverFileBasicInfo.UpdateFile(guid, fileType);
                            }
                            break;
                        default:
                            break;
                    }

                    CursorControl.SetWaitCursor(this);

                    m_serverFileBasicInfo.FileUpLoad(openFileDialog1.FileName, strFtpServerPath, guid.ToString(), fileType);

                    this.Cursor = System.Windows.Forms.Cursors.Arrow;

                    if (GetError())
                    {
                        MessageDialog.ShowPromptMessage("上传成功");
                        return guid.ToString();
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
                return null;
            }
        }

        /// <summary>
        /// 通用操作方式
        /// </summary>
        /// <param name="guid">唯一编码</param>
        void ShowOrDownLoad(string guid)
        {
            FM_FilePath lnqTemp = m_serverFileBasicInfo.GetFilePathInfo(new Guid(guid));
            操作方式 frm = new 操作方式(ProcessType.发布);
            frm.ShowDialog();

            if (frm.OperatorFlag ==  CE_FileOperatorType.在线阅读)
            {
                FileOperationService.File_Look(new Guid(guid),
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
            }
            else if (frm.OperatorFlag ==  CE_FileOperatorType.下载)
            {
                saveFileDialog1.Filter = "All files (*.*)|*.*";
                saveFileDialog1.FileName = (txtFileName.Text + lnqTemp.FileType).Replace("/", "-");

                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    m_serverFTP.Download(lnqTemp.FilePath, saveFileDialog1.FileName);

                    if (GetError())
                    {
                        MessageDialog.ShowPromptMessage("下载成功");
                    }
                }
            }
        }

        public 文件发布流程明细(string sdbNo)
        {
            InitializeComponent();

            m_strSDBNo = sdbNo;
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.文件发布流程.ToString(), m_serverReleaseProcess);
            m_billMessageServer.BillType = CE_BillTypeEnum.文件发布流程.ToString();
        }

        private void 文件发布流程明细_Load(object sender, EventArgs e)
        {
            ClearData();

            if (m_strSDBNo == null)
            {
                txtSDBNo.Text = m_billNoControl.GetNewBillNo();
                lbSDBStatus.Text = "新建流程";
            }
            else
            {
                m_lnqReleaseProcess = m_serverReleaseProcess.GetInfo(m_strSDBNo);
                ShowData();
            }

            FlowControl();
        }

        private void 文件发布流程明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (llbProposerDownLoad.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请上传申请文件");
                return;
            }

            GetData();

            if (m_lnqReleaseProcess.SDBStatus == "流程已结束")
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

            if (!m_serverReleaseProcess.AddProcess(m_lnqReleaseProcess, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {

                m_strSDBNo = txtSDBNo.Text;

                List<string> list = new List<string>();

                foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                {
                    if (pbi.工号 != null && pbi.工号.Length > 0)
                    {
                        list.Add(pbi.工号);
                    }
                }

                m_billMessageServer.DestroyMessage(m_lnqReleaseProcess.SDBNo);
                m_billMessageServer.SendNewFlowMessage(m_lnqReleaseProcess.SDBNo,
                    string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", m_lnqReleaseProcess.FileNo, m_lnqReleaseProcess.FileName),
                    BillFlowMessage_ReceivedUserType.用户, list);

                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            GetData();

            if (m_lnqReleaseProcess.SDBStatus != "等待审核")
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

            if (!m_serverReleaseProcess.AuditProcess(m_lnqReleaseProcess, out m_strErr))
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

                m_billMessageServer.PassFlowMessage(m_lnqReleaseProcess.SDBNo,
                    string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", m_lnqReleaseProcess.FileNo, m_lnqReleaseProcess.FileName),
                    BillFlowMessage_ReceivedUserType.用户, list);

                MessageDialog.ShowPromptMessage("审核成功");
                this.Close();
            }
        }

        private void btnApprove_Click(object sender, EventArgs e)
        {
            GetData();

            if (m_lnqReleaseProcess.SDBStatus != "等待批准")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            if (!m_serverReleaseProcess.ApproveProcess(m_lnqReleaseProcess, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                m_lnqReleaseProcess = m_serverReleaseProcess.GetInfo(m_lnqReleaseProcess.SDBNo);

                List<string> list = new List<string>();

                list.Add(UniversalFunction.GetPersonnelCode(m_lnqReleaseProcess.Auditor));
                list.Add(UniversalFunction.GetPersonnelCode(m_lnqReleaseProcess.Propoer));

                m_billMessageServer.EndFlowMessage(m_lnqReleaseProcess.SDBNo,
                    string.Format("{0}号文件发布流程已结束", m_lnqReleaseProcess.SDBNo), null, list);
                MessageDialog.ShowPromptMessage("批准成功");

                this.Close();
            }
        }

        private void llbProposerUpLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strTag = Update("Propoer");

            if (strTag != null)
            {
                llbProposerUpLoad.Tag = strTag;
                llbProposerDownLoad.Tag = strTag;
                llbProposerDownLoad.Text = BasicInfo.LoginName + "的文件";
            }
        }

        private void llbProposerDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (llbProposerDownLoad.Tag.ToString().Length != 0)
            {
                ShowOrDownLoad(llbProposerDownLoad.Tag.ToString());
            }
        }

        private void txtDepartment_OnCompleteSearch()
        {
            txtDepartment.Tag = txtDepartment.DataResult["部门编码"].ToString();
        }

        private void txtSortName_OnCompleteSearch()
        {
            txtSortName.Tag = Convert.ToInt32(txtSortName.DataResult["类别ID"]);
        }

        private void txtReplaceFileNo_OnCompleteSearch()
        {
            txtReplaceFileNo.Text = txtReplaceFileNo.DataResult["文件编号"].ToString();
            txtReplaceFileNo.Tag = Convert.ToInt32(txtReplaceFileNo.DataResult["文件ID"]);
            txtReplaceFileName.Text = txtReplaceFileNo.DataResult["文件名称"].ToString();
            txtRepalceVersion.Text = txtReplaceFileNo.DataResult["版本号"].ToString();
        }

        private void txtReplaceFileNo_Enter(object sender, EventArgs e)
        {
            txtReplaceFileNo.StrEndSql = " and 类别ID not in (10,11)";
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            if (lbSDBStatus.Text == "等待审核" && btnAudit.Visible == true)
            {
                ReturnBillStatus();
            }
            else if (lbSDBStatus.Text == "等待批准" && btnApprove.Visible == true)
            {
                ReturnBillStatus();
            }
        }
    }
}
