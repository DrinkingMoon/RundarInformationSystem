using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using GlobalObject;
using PlatformManagement;
using Service_Quality_File;
using UniversalControlLibrary;
using Form_Quality_File.Properties;
using System.IO;

namespace Form_Quality_File
{
    public partial class 制度发布流程明细 : CustomFlowForm
    {
        /// <summary>
        /// 制度发布流程服务组件
        /// </summary>
        IInstitutionProcess m_serverInstitution = Service_Quality_File.ServerModuleFactory.GetServerModule<IInstitutionProcess>();

        /// <summary>
        /// 文件基础服务组件
        /// </summary>
        ISystemFileBasicInfo m_serverFileBasic = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileBasicInfo>();

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
        private FM_InstitutionProcess m_lnqInstitution = new FM_InstitutionProcess();

        public FM_InstitutionProcess LnqInstitution
        {
            get { return m_lnqInstitution; }
            set { m_lnqInstitution = value; }
        }

        /// <summary>
        /// FTP服务组件
        /// </summary>
        FileServiceSocket m_serverFTP = new FileServiceSocket(GlobalObject.GlobalParameter.FTPServerIP,
            GlobalObject.GlobalParameter.FTPServerAdvancedUser,
            GlobalObject.GlobalParameter.FTPServerAdvancedPassword);

        public 制度发布流程明细()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 清空信息
        /// </summary>
        void ClearInfo()
        {
            txtSDBNo.Text = "";
            lbSDBStatus.Text = "";
            lbPropoer.Text = "";
            lbPropoerTime.Text = "";
            txtFileName.Text = "";
            txtFileNo.Text = "";
            txtFileSort.Text = "";
            txtFileSort.Tag = null;
            txtRemark.Text = "";
            llbFileDownLoad.Tag = null;
            txtReplaceFileNo.Tag = null;
            txtReplaceFileNo.Text = "";
            txtReplaceFileName.Text = "";
            txtRepalceVersion.Text = "";
            txtDepartment.Text = "";
            txtDepartment.Tag = null;
            radioButton1.Checked = true;
            radioButton2.Checked = false;
            txtReplaceFileNo.Enabled = false;
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            if (m_lnqInstitution != null)
            {
                txtSDBNo.Text = m_lnqInstitution.BillNo;
                txtFileName.Text = m_lnqInstitution.FileName;
                txtFileNo.Text = m_lnqInstitution.FileNo;
                txtReplaceFileNo.Tag = m_lnqInstitution.ReplaceFileID;

                if (txtReplaceFileNo.Tag != null)
                {
                    FM_FileList fileInfo = m_serverFileBasic.GetFile((int)txtReplaceFileNo.Tag);

                    txtReplaceFileNo.Text = fileInfo.FileNo;
                    txtReplaceFileName.Text = fileInfo.FileName;
                    txtRepalceVersion.Text = fileInfo.Version;
                }

                txtDepartment.Tag = m_lnqInstitution.Department;
                txtDepartment.Text = txtDepartment.Tag == null ? "" : UniversalFunction.GetDeptName(txtDepartment.Tag.ToString());

                FM_FileSort sort = m_serverFileBasic.SortInfo(m_lnqInstitution.SortID);

                txtFileSort.Text = sort.SortName;
                txtFileSort.Tag = sort.SortID;

                txtRemark.Text = m_lnqInstitution.Introductions;

                lbPropoer.Text = m_lnqInstitution.Propoer;
                lbPropoerTime.Text = m_lnqInstitution.PropoerTime.ToString();
                lbSDBStatus.Text = m_lnqInstitution.BillStatus;
                llbFileDownLoad.Text = lbPropoer.Text + "的文件";
                llbFileDownLoad.Tag = m_lnqInstitution.FileUnique;

                if (m_lnqInstitution.OperationMode == radioButton1.Text)
                {
                    radioButton1.Checked = true;
                    radioButton2.Checked = false;
                }
                else
                {
                    radioButton1.Checked = false;
                    radioButton2.Checked = true;
                }
            }
            else
            {
                lbSDBStatus.Text = InstitutionBillStatus.新建流程.ToString();
                lbPropoer.Text = BasicInfo.LoginName;
                lbPropoerTime.Text = ServerTime.Time.ToString();
                txtSDBNo.Text = m_billNoControl.GetNewBillNo();
                llbFileDownLoad.Text = "";
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
        /// 上传文件
        /// </summary>
        string UpdateFile()
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

                    ConditionUpdateFile(llbFileDownLoad, ref guid, strFtpServerPath, fileType, CE_OperatorMode.添加);

                    CursorControl.SetWaitCursor(this);
                    m_serverFileBasic.FileUpLoad(openFileDialog1.FileName, strFtpServerPath, guid.ToString(), fileType);

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
                m_serverFileBasic.AddFile(guid, ftpServerPath + guid.ToString(), fileType);
            }
            else
            {
                guid = new Guid(linklb.Tag.ToString());

                if (mode == CE_OperatorMode.添加)
                {
                    m_serverFileBasic.UpdateFile(guid, fileType);
                }

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
                FM_FilePath lnqTemp = m_serverFileBasic.GetFilePathInfo(new Guid(guid));

                操作方式 frm = new 操作方式(ProcessType.发布);
                frm.ShowDialog();

                if (frm.OperatorFlag == CE_FileOperatorType.在线阅读)
                {
                    FileOperationService.File_Look(new Guid(guid),
                            GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                }
                else if (frm.OperatorFlag == CE_FileOperatorType.下载)
                {
                    saveFileDialog1.Filter = "All files (*.*)|*.*";
                    saveFileDialog1.FileName = (txtFileName.Text + lnqTemp.FileType).Replace("/", "-");

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
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
                return;
            }
        }

        private void llbFileDownLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (llbFileDownLoad.Tag.ToString().Length != 0)
            {
                ShowOrDownLoad(llbFileDownLoad.Tag.ToString());
            }
        }

        private void txtFileSort_OnCompleteSearch()
        {
            txtFileSort.Tag = Convert.ToInt32(txtFileSort.DataResult["文件类别ID"]);
        }

        bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            m_lnqInstitution = new FM_InstitutionProcess();

            if (llbFileUpLoad.Tag != null)
            {
                m_lnqInstitution.FileUnique = new Guid(llbFileUpLoad.Tag.ToString());
            }

            m_lnqInstitution.Introductions = txtRemark.Text;
            m_lnqInstitution.TypeCode = UniversalFunction.GetBillPrefix(CE_BillTypeEnum.制度发布流程);
            m_lnqInstitution.FileName = txtFileName.Text;
            m_lnqInstitution.FileNo = txtFileNo.Text;
            m_lnqInstitution.BillNo = txtSDBNo.Text;
            m_lnqInstitution.BillStatus = lbSDBStatus.Text;
            m_lnqInstitution.SortID = txtFileSort.Tag == null ? 0 : Convert.ToInt32(txtFileSort.Tag);
            m_lnqInstitution.Department = txtDepartment.Tag.ToString();
            m_lnqInstitution.OperationMode = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;
            m_lnqInstitution.ReplaceFileID = (int?)txtReplaceFileNo.Tag;

            ResultInfo = m_lnqInstitution;
            return true;
        }

        private void txtFileSort_Enter(object sender, EventArgs e)
        {
            txtFileSort.StrEndSql = " and 文件类型ID = " + (int)CE_FileType.制度文件 + " and 文件类别ID not in (select ParentID from FM_FileSort)";
        }

        private void llbFileUpLoad_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string strTag = UpdateFile();

            if (strTag != null)
            {
                llbFileUpLoad.Tag = strTag;
                llbFileDownLoad.Tag = strTag;
                llbFileDownLoad.Text = BasicInfo.LoginName + "的文件";
            }
        }

        private void txtDepartment_OnCompleteSearch()
        {
            txtDepartment.Tag = txtDepartment.DataResult["部门编码"].ToString();
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
            txtReplaceFileNo.StrEndSql = " and 文件ID in (select FileID from FM_InstitutionProcess where TypeCode = '" 
                + UniversalFunction.GetBillPrefix(CE_BillTypeEnum.制度修订废弃申请流程) + "' and BillStatus = '"
                + InstitutionBillStatus.流程已结束.ToString() + "' and OperationMode = '" + radioButton2.Text + "') and 类别ID <> 29";
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                txtReplaceFileNo.Enabled = false;
                txtReplaceFileNo.Tag = null;
                txtReplaceFileNo.Text = "";
                txtReplaceFileName.Text = "";
                txtRepalceVersion.Text = "";
            }
            else
            {
                txtReplaceFileNo.Enabled = true;
            }
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.制度发布流程.ToString(), m_serverInstitution);
                m_billMessageServer.BillType = CE_BillTypeEnum.制度发布流程.ToString();
                m_lnqInstitution = m_serverInstitution.GetSingleBill(this.FlowInfo_BillNo);
                ClearInfo();
                ShowInfo();

                m_lnqInstitution = new FM_InstitutionProcess();
                m_lnqInstitution.BillNo = txtSDBNo.Text;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }
    }
}
