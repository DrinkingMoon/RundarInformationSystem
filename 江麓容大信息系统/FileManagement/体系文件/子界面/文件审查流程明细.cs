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
using System.Diagnostics;
using System.Threading;
using UniversalControlLibrary;
using Form_Quality_File.Properties;

namespace Form_Quality_File
{
    public partial class 文件审查流程明细 : Form
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
        /// 文件基础信息服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasicInfo = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 文件审查流程服务组件
        /// </summary>
        ISystemFileReviewProcess m_serverReviewProcess = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileReviewProcess>();

        /// <summary>
        /// LNQ数据集
        /// </summary>
        FM_ReviewProcess m_lnqReviewProcess = new FM_ReviewProcess();

        /// <summary>
        /// 指定人LNQ数据集
        /// </summary>
        FM_ReviewProcessPointListInfo m_lnqPointInfo = new FM_ReviewProcessPointListInfo();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 相关确认人员信息列表
        /// </summary>
        List<FM_ReviewProcessPointListInfo> m_listPointInfo = new List<FM_ReviewProcessPointListInfo>();

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        public 文件审查流程明细(string sdbNo)
        {
            InitializeComponent();
            m_strSDBNo = sdbNo;

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.文件审查流程.ToString(), m_serverReviewProcess);
            m_billMessageServer.BillType = CE_BillTypeEnum.文件审查流程.ToString();
        }

        private void 文件审查流程明细_Load(object sender, EventArgs e)
        {
            ClearData();

            if (m_strSDBNo == null)
            {
                txtSDBNo.Text = m_billNoControl.GetNewBillNo();
                lbSDBStatus.Text = "新建流程";
            }
            else
            {
                m_lnqReviewProcess = m_serverReviewProcess.GetInfo(m_strSDBNo);
                ShowData();
                m_listPointInfo = m_serverReviewProcess.GetListInfo(m_strSDBNo.ToString());
            }

            FlowControl();

            PositioningRecord(BasicInfo.LoginID);
        }

        private void 文件审查流程明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        /// <summary>
        /// 回退流程
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lbSDBStatus.Text != "流程已结束")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.文件审查流程, txtSDBNo.Text, lbSDBStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您确定要回退此单据吗？") == DialogResult.Yes)
                    {
                        GetData();

                        if (m_serverReviewProcess.ReturnBill(form.StrBillID,
                            form.StrBillStatus, m_lnqReviewProcess, out m_strErr, form.Reason))
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
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位用的信息</param>
        void PositioningRecord(string msg)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["工号"].Value == msg)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
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
                case "等待主管审核":
                    llbAuditorUpLoad.Visible = visible;
                    btnAudit.Visible = visible;
                    break;
                case "等待相关确认":
                    btnPointAffirm.Visible = visible;
                    break;
                case "等待判定":
                    llbJudgeUpLoad.Visible = visible;
                    btnJudge.Visible = visible;
                    break;
                default:
                    break;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.体系工程师.ToString()) && lbSDBStatus.Text == "等待相关确认")
            {
                llbJudgeUpLoad.Visible = true;
                btnJudge.Visible = true;
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
            lbJudge.Text = "";
            lbJudgeTime.Text = "";
            lbPointDate.Text = "";
            lbPointPersonnel.Text = "";
            lbPropoer.Text = "";
            lbPropoerTime.Text = "";
            lbSDBStatus.Text = "";

            llbAuditorDownLoad.Text = "";
            llbJudgeDownLoad.Text = "";
            llbPointDownLoad.Text = "";
            llbProposerDownLoad.Text = "";

            llbAuditorDownLoad.Tag = "";
            llbJudgeDownLoad.Tag = "";
            llbPointDownLoad.Tag = "";
            llbProposerDownLoad.Tag = "";
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetData()
        {
            m_lnqReviewProcess = new FM_ReviewProcess();

            m_lnqReviewProcess.AuditorAdvise = txtAuditorAdvise.Text;
            m_lnqReviewProcess.FileName = txtFileName.Text;
            m_lnqReviewProcess.FileNo = txtFileNo.Text;
            m_lnqReviewProcess.JudgeAdvise = txtJudgeAdvise.Text;
            m_lnqReviewProcess.Remark = txtRemark.Text;
            m_lnqReviewProcess.SDBNo = txtSDBNo.Text;
            m_lnqReviewProcess.SDBStatus = lbSDBStatus.Text;

            if (llbProposerUpLoad.Tag != null)
            {
                m_lnqReviewProcess.FileUnique = new Guid(llbProposerUpLoad.Tag.ToString());
            }

            if (llbAuditorUpLoad.Tag != null)
            {
                m_lnqReviewProcess.AuditorFileUnique = new Guid(llbAuditorUpLoad.Tag.ToString());
            }

            if (llbJudgeUpLoad.Tag != null)
            {
                m_lnqReviewProcess.JudgeFileUnique = new Guid(llbJudgeUpLoad.Tag.ToString());
            }
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowData()
        {
            txtRemark.Text = m_lnqReviewProcess.Remark;
            txtJudgeAdvise.Text = m_lnqReviewProcess.JudgeAdvise;
            txtFileNo.Text = m_lnqReviewProcess.FileNo;
            txtFileName.Text = m_lnqReviewProcess.FileName;
            txtAuditorAdvise.Text = m_lnqReviewProcess.AuditorAdvise;
            txtSDBNo.Text = m_lnqReviewProcess.SDBNo;

            lbAuditor.Text = m_lnqReviewProcess.Auditor;
            lbAuditorTime.Text = m_lnqReviewProcess.AuditorTime.ToString();
            lbJudge.Text = m_lnqReviewProcess.Judge;
            lbJudgeTime.Text = m_lnqReviewProcess.JudgeTime.ToString();
            lbPropoer.Text = m_lnqReviewProcess.Propoer;
            lbPropoerTime.Text = m_lnqReviewProcess.PropoerTime.ToString();
            lbSDBStatus.Text = m_lnqReviewProcess.SDBStatus;

            llbAuditorDownLoad.Text = m_lnqReviewProcess.AuditorFileUnique == null ? "" : lbAuditor.Text + "的文件";
            llbJudgeDownLoad.Text = m_lnqReviewProcess.JudgeFileUnique == null ? "" : lbJudge.Text + "的文件";
            llbProposerDownLoad.Text = m_lnqReviewProcess == null ? "" : lbPropoer.Text + "的文件";

            llbAuditorDownLoad.Tag = m_lnqReviewProcess.AuditorFileUnique;
            llbJudgeDownLoad.Tag = m_lnqReviewProcess.JudgeFileUnique;
            llbProposerDownLoad.Tag = m_lnqReviewProcess.FileUnique;

            dataGridView1.DataSource = m_serverReviewProcess.GetListInfoTable(txtSDBNo.Text);
        }

        /// <summary>
        /// 按条件上传文件
        /// </summary>
        /// <param name="linklb">LinkLabel空间</param>
        /// <param name="guid">序号</param>
        /// <param name="ftpServerPath">文件路径</param>
        /// <param name="fileType">文件类型</param>
        /// <param name="mode">操作模式</param>
        void ConditionUpdateFile(LinkLabel linklb, ref Guid guid, string ftpServerPath, string fileType, CE_OperatorMode mode)
        {
            if (linklb.Tag == null || linklb.Tag.ToString().Trim().Length == 0)
            {
                guid = Guid.NewGuid();
                m_serverFileBasicInfo.AddFile(guid, ftpServerPath + guid.ToString(), fileType);
            }
            else
            {
                guid = new Guid(linklb.Tag.ToString());

                if (mode == CE_OperatorMode.添加)
                {
                    m_serverFileBasicInfo.UpdateFile(guid, fileType);
                }

            }
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
                            ConditionUpdateFile(llbProposerDownLoad, ref guid, strFtpServerPath, fileType, CE_OperatorMode.添加);
                            break;
                        case "Auditor":
                            ConditionUpdateFile(llbAuditorDownLoad, ref guid, strFtpServerPath, fileType, CE_OperatorMode.添加);
                            break;
                        case "Point":
                            ConditionUpdateFile(llbPointDownLoad, ref guid, strFtpServerPath, fileType, CE_OperatorMode.添加);
                            break;
                        case "Judge":
                            ConditionUpdateFile(llbJudgeDownLoad, ref guid, strFtpServerPath, fileType, CE_OperatorMode.添加);
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
            try
            {
                FM_FilePath lnqTemp = m_serverFileBasicInfo.GetFilePathInfo(new Guid(guid));

                操作方式 frm = new 操作方式(ProcessType.审查);
                frm.ShowDialog();

                if (frm.OperatorFlag == CE_FileOperatorType.在线阅读)
                {
                    FileOperationService.File_Look(new Guid(guid),
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                }
                else if (frm.OperatorFlag == CE_FileOperatorType.下载)
                {
                    saveFileDialog1.Filter = "All files (*.*)|*.*";
                    saveFileDialog1.FileName = (txtFileName.Text + lnqTemp.FileType).Replace("/","-");

                    if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                    {
                        BackgroundWorker worker = BackgroundWorkerTools.GetWorker("下载文件");
                        worker.RunWorkerAsync();

                        m_serverFTP.Download(lnqTemp.FilePath, saveFileDialog1.FileName);

                        worker.CancelAsync();

                        if (GetError())
                        {
                            MessageDialog.ShowPromptMessage("下载成功");
                        }
                    }
                }
                else if (frm.OperatorFlag == CE_FileOperatorType.在线编辑)
                {
                    string filePath = "C:\\temp" + lnqTemp.FileType;

                    m_serverFTP.Download(lnqTemp.FilePath, filePath);
                    System.Diagnostics.Process myProcess = new System.Diagnostics.Process();

                    //设置启动进程的初始目录 
                    myProcess.StartInfo.WorkingDirectory = Application.StartupPath;
                    //设置启动进程的应用程序或文档名 
                    myProcess.StartInfo.FileName = filePath;
                    //设置启动进程的参数 
                    myProcess.StartInfo.Arguments = "";

                    myProcess.Start();
                    myProcess.WaitForExit();

                    bool flag = false;
                    Guid guidTemp = new Guid();
                    string strFtpServerPath = "/" + ServerTime.Time.Year.ToString() + "/" + ServerTime.Time.Month.ToString() + "/";

                    if (!GlobalObject.FileTypeRecognition.IsWordDocument(filePath))
                    {
                        throw new Exception("此文件非正常WORD文件，可能由于文件格式无法识别或者文件加密造成无法上传");
                    }

                    if (btnAdd.Visible)
                    {
                        ConditionUpdateFile(llbProposerDownLoad, ref guidTemp, strFtpServerPath, lnqTemp.FileType, CE_OperatorMode.修改);

                        flag = true;
                        llbProposerUpLoad.Tag = guidTemp;
                        llbProposerDownLoad.Tag = guidTemp;
                        llbProposerDownLoad.Text = BasicInfo.LoginName + "的文件";
                    }
                    else if (btnAudit.Visible)
                    {

                        ConditionUpdateFile(llbAuditorDownLoad, ref guidTemp, strFtpServerPath, lnqTemp.FileType, CE_OperatorMode.修改);

                        flag = true;
                        llbAuditorUpLoad.Tag = guidTemp;
                        llbAuditorDownLoad.Tag = guidTemp;
                        llbAuditorDownLoad.Text = BasicInfo.LoginName + "的文件";
                    }
                    else if (btnPointAffirm.Visible)
                    {
                        ConditionUpdateFile(llbPointDownLoad, ref guidTemp, strFtpServerPath, lnqTemp.FileType, CE_OperatorMode.修改);

                        flag = true;
                        m_serverReviewProcess.PointUpLoadFile(guidTemp, m_strSDBNo);

                        dataGridView1.DataSource = m_serverReviewProcess.GetListInfoTable(txtSDBNo.Text);
                        PositioningRecord(BasicInfo.LoginID);
                    }
                    else if (btnJudge.Visible)
                    {
                        ConditionUpdateFile(llbJudgeDownLoad, ref guidTemp, strFtpServerPath, lnqTemp.FileType, CE_OperatorMode.修改);

                        flag = true;
                        llbJudgeUpLoad.Tag = guidTemp;
                        llbJudgeDownLoad.Tag = guidTemp;
                        llbJudgeDownLoad.Text = BasicInfo.LoginName + "的文件";
                    }

                    if (flag)
                    {
                        CursorControl.SetWaitCursor(this);
                        m_serverFileBasicInfo.FileUpLoad(filePath, strFtpServerPath, guidTemp.ToString(), lnqTemp.FileType);
                        this.Cursor = System.Windows.Forms.Cursors.Arrow;
                    }

                    File.Delete(filePath);
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
                return;
            }

        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns></returns>
        bool CheckData()
        {
            if (llbProposerDownLoad.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请上传文件");
                return false;
            }
            else if (txtFileName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请录入文件名称");
                return false;
            }

            return true;
        }

        List<string> GetChoosePepoleList()
        {
            List<string> list = new List<string>();
            FormSelectPersonnel2 frm = new FormSelectPersonnel2();

            if (frm.ShowDialog() != DialogResult.OK)
            {
                return null;
            }

            foreach (PersonnelBasicInfo pbi in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
            {
                if (pbi.工号 != null && pbi.工号.Length > 0)
                {
                    list.Add(pbi.工号);
                }
            }

            return list;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (llbProposerDownLoad.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请上传申请文件");
                return;
            }

            GetData();

            if (m_lnqReviewProcess.SDBStatus == "流程已结束")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            CheckData();

            List<string> listAudit = new List<string>();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.体系工程师.ToString()))
            {
                MessageDialog.ShowPromptMessage("请选择审核人");
                listAudit = GetChoosePepoleList();
                if (listAudit == null)
                {
                    return;
                }
            }

            MessageDialog.ShowPromptMessage("请选择相关确认人");
            List<string> list = GetChoosePepoleList();
            if (list == null)
            {
                return;
            }

            if (!m_serverReviewProcess.AddProcess(m_lnqReviewProcess, list, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                m_strSDBNo = txtSDBNo.Text;
                MessageDialog.ShowPromptMessage("提交成功");
                m_billMessageServer.DestroyMessage(m_lnqReviewProcess.SDBNo);

                if (listAudit.Count() > 0)
                {
                    m_billMessageServer.SendNewFlowMessage(m_lnqReviewProcess.SDBNo,
                        string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", m_lnqReviewProcess.FileNo, m_lnqReviewProcess.FileName),
                        BillFlowMessage_ReceivedUserType.用户, listAudit);
                }
                else
                {
                    m_billMessageServer.SendNewFlowMessage(m_lnqReviewProcess.SDBNo,
                        string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", m_lnqReviewProcess.FileNo, m_lnqReviewProcess.FileName),
                        BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));
                }

                this.Close();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            GetData();

            if (m_lnqReviewProcess.SDBStatus != "等待主管审核")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            if (!m_serverReviewProcess.AuditProcess(m_lnqReviewProcess, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");

                List<string> listPersonnel = new List<string>();

                foreach (FM_ReviewProcessPointListInfo item in m_listPointInfo)
                {
                    listPersonnel.Add(item.PointPersonnel);
                }

                m_billMessageServer.PassFlowMessage(m_lnqReviewProcess.SDBNo,
                    string.Format("【文件编号】：{0} 【文件名】：{1} ※※※ 等待处理", m_lnqReviewProcess.FileNo, m_lnqReviewProcess.FileName),
                    BillFlowMessage_ReceivedUserType.用户,listPersonnel);

                this.Close();
            }
        }

        private void btnJudge_Click(object sender, EventArgs e)
        {
            GetData();

            if (m_lnqReviewProcess.SDBStatus == "流程已结束" || m_lnqReviewProcess.SDBStatus == "新建流程")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            if (!m_serverReviewProcess.JudgeProcess(m_lnqReviewProcess, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");

                List<string> listPersonnel = new List<string>();

                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqReviewProcess.Propoer));
                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqReviewProcess.Auditor));

                foreach (FM_ReviewProcessPointListInfo item in m_listPointInfo)
                {
                    listPersonnel.Add(item.PointPersonnel);
                }

                m_billMessageServer.EndFlowMessage(m_lnqReviewProcess.SDBNo,
                    string.Format("{0}号文件审查流程已结束", m_lnqReviewProcess.SDBNo), null, listPersonnel);
                m_billNoControl.UseBill(m_lnqReviewProcess.SDBNo);

                this.Close();
            }
        }

        private void btnPointAffirm_Click(object sender, EventArgs e)
        {
            if (m_lnqReviewProcess.SDBStatus != "等待相关确认")
            {
                MessageDialog.ShowPromptMessage("流程状态错误");
                return;
            }

            if (!m_serverReviewProcess.PointAffirmProcess(m_strSDBNo, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtAdvise.Text = dataGridView1.CurrentRow.Cells["意见"].Value.ToString();
            lbPointPersonnel.Text = dataGridView1.CurrentRow.Cells["指定人"].Value.ToString();
            lbPointDate.Text = dataGridView1.CurrentRow.Cells["操作日期"].Value.ToString();

            llbPointDownLoad.Text = dataGridView1.CurrentRow.Cells["文件唯一编码"].Value.ToString().Trim().Length == 0 ? "" : lbPointPersonnel.Text + "的文件";
            llbPointDownLoad.Tag = dataGridView1.CurrentRow.Cells["文件唯一编码"].Value.ToString();

            if (lbSDBStatus.Text == "等待相关确认" && dataGridView1.CurrentRow.Cells["工号"].Value.ToString() == BasicInfo.LoginID)
            {
                llbPointUpLoad.Visible = true;
            }
            else
            {
                llbPointUpLoad.Visible = false;
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

        private void llbAuditorUpLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strTag = Update("Auditor");

            if (strTag != null)
            {
                llbAuditorUpLoad.Tag = strTag;
                llbAuditorDownLoad.Tag = strTag;
                llbAuditorDownLoad.Text = BasicInfo.LoginName + "的文件";
            }
        }

        private void llbPointUpLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strTag = Update("Point");

            if (strTag != null)
            {
                m_serverReviewProcess.PointUpLoadFile(new Guid(strTag), m_strSDBNo);
                dataGridView1.DataSource = m_serverReviewProcess.GetListInfoTable(txtSDBNo.Text);
                PositioningRecord(BasicInfo.LoginID);
            }
        }

        private void llbJudgeUpLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strTag = Update("Judge");

            if (strTag != null)
            {
                llbJudgeUpLoad.Tag = strTag;
                llbJudgeDownLoad.Tag = strTag;
                llbJudgeDownLoad.Text = BasicInfo.LoginName + "的文件";
            }
        }

        private void llbProposerDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (llbProposerDownLoad.Tag.ToString().Length != 0)
            {
                ShowOrDownLoad(llbProposerDownLoad.Tag.ToString());
            }

            //List<FileOperatorType> list = new List<FileOperatorType>();
            //list.Add(FileOperatorType.在线编辑);
            //list.Add(FileOperatorType.在线阅读);

            //文件操作方式 frm = new 文件操作方式(list, "", "/质量数据库/2014/1/0106fb77-ec0c-4f8f-b389-c7b209fe0834", "doc");
            //frm.Show();
        }

        private void llbAuditorDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (llbAuditorDownLoad.Tag.ToString().Length != 0)
            {
                ShowOrDownLoad(llbAuditorDownLoad.Tag.ToString());
            }
        }

        private void llbPointDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (llbPointDownLoad.Tag.ToString().Length != 0)
            {
                ShowOrDownLoad(llbPointDownLoad.Tag.ToString());
            }
        }

        private void llbJudgeDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (llbJudgeDownLoad.Tag.ToString().Length != 0)
            {
                ShowOrDownLoad(llbJudgeDownLoad.Tag.ToString());
            }
        }

        private void btnSaveAdvise_Click(object sender, EventArgs e)
        {
            m_serverReviewProcess.PointAdvise(txtAdvise.Text, m_strSDBNo);

            dataGridView1.DataSource = m_serverReviewProcess.GetListInfoTable(txtSDBNo.Text);
            PositioningRecord(BasicInfo.LoginID);
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            if (lbSDBStatus.Text == "等待主管审核" && btnAudit.Visible == true)
            {
               ReturnBillStatus();
            }
        }
    }
}
