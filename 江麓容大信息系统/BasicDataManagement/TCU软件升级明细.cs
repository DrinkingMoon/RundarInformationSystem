using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using Service_Project_Design;
using FlowControlService;

namespace Form_Project_Design
{
    public partial class TCU软件升级明细 : UniversalControlLibrary.CustomFlowForm
    {
        FlowControlService.IFlowServer _SericeFlow = 
            FlowControlService.ServerModuleFactory.GetServerModule<FlowControlService.IFlowServer>();

        BillNumberControl m_billNoControl;

        Business_Project_TCU_SoftwareUpdate _UpdateInfo = new Business_Project_TCU_SoftwareUpdate();

        List<View_Business_Project_TCU_SoftwareUpdate_DID> _ListDID = new List<View_Business_Project_TCU_SoftwareUpdate_DID>();

        ITCUInfoService _ServiceTCU = Service_Project_Design.ServerModuleFactory.GetServerModule<ITCUInfoService>();

        public TCU软件升级明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.TCU软件升级.ToString(), _ServiceTCU);

                _UpdateInfo = _ServiceTCU.GetSingleInfo_TCUSoft(this.FlowInfo_BillNo);
                _ListDID = _ServiceTCU.GetListDIDInfo(this.FlowInfo_BillNo);
                ShowInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        void ShowInfo()
        {
            if (_UpdateInfo == null)
            {
                txtBillNo.Text = this.FlowInfo_BillNo;
                _UpdateInfo = new Business_Project_TCU_SoftwareUpdate();
            }
            else
            {
                lbBillStatus.Text = _SericeFlow.GetNowBillStatus(_UpdateInfo.BillNo);

                txtBillNo.Text = _UpdateInfo.BillNo;

                txtCarModelNo.Text = _UpdateInfo.CarModelNo;
                txtClientVersion.Text = _UpdateInfo.ClientVersion;
                txtDiagnosisVersion.Text = _UpdateInfo.DiagnosisVersion;
                txtHardwareVersion.Text = _UpdateInfo.HardwareVersion;
                txtTechnicalNote.Text = _UpdateInfo.TechnicalNote;
                txtTestResult.Text = _UpdateInfo.TestResult;
                txtUnderVersion.Text = _UpdateInfo.UnderVersion;

                txtUpdateContent.Text = _UpdateInfo.UpdateContent;
                txtUpdateReason.Text = _UpdateInfo.UpdateReason;

                cmbVersionType.Text = _UpdateInfo.Version.Substring(txtCarModelNo.Text.Length, 1);
                txtVersion.Text = _UpdateInfo.Version.Substring((txtCarModelNo.Text + "T").ToString().Length);

                btnDownload_TCUSoft.Tag = _UpdateInfo.ProgramUnique;
                btnDownload_TestReport.Tag = _UpdateInfo.TestReport;

                if (_UpdateInfo.IsPassTest != null)
                {
                    if (((bool)_UpdateInfo.IsPassTest))
                    {
                        rbYes_Pass.Checked = true;
                        rbNo_Pass.Checked = false;
                    }
                    else
                    {
                        rbYes_Pass.Checked = false;
                        rbNo_Pass.Checked = true;
                    }
                }
            }

            if (lbBillStatus.Text == CE_CommonBillStatus.新建单据.ToString())
            {
                btnUpload_TCUSoft.Enabled = true;
                cmbVersionType.Enabled = true;
                btnUpload_TestReport.Enabled = false;
            }
            else
            {
                btnUpload_TCUSoft.Enabled = false;
                cmbVersionType.Enabled = false;
                btnUpload_TestReport.Enabled = true;
            }

            if (lbBillStatus.Text == CE_CommonBillStatus.单据完成.ToString())
            {
                btnUpload_TCUSoft.Enabled = false;
                cmbVersionType.Enabled = false;
                btnUpload_TestReport.Enabled = false;
            }

            if (btnDownload_TCUSoft.Tag == null || btnDownload_TCUSoft.Tag.ToString().Length == 0)
            {
                btnDownload_TCUSoft.Enabled = false;
            }

            if (btnDownload_TestReport.Tag == null || btnDownload_TestReport.Tag.ToString().Length == 0)
            {
                btnDownload_TestReport.Enabled = false;
            }

            RefreshDataGridView(_ListDID);
        }

        void RefreshDataGridView(List<View_Business_Project_TCU_SoftwareUpdate_DID> listInfo)
        {
            if (listInfo != null)
            {
                customDataGridView1.DataSource = new BindingCollection<View_Business_Project_TCU_SoftwareUpdate_DID>(listInfo);
                customDataGridView1.Columns["BillNo"].Visible = false;
            }
        }

        void GetInfo()
        {
            _UpdateInfo.BillNo = txtBillNo.Text;
            _UpdateInfo.CarModelNo = txtCarModelNo.Text;
            _UpdateInfo.ClientVersion = txtClientVersion.Text;
            _UpdateInfo.DiagnosisVersion = txtDiagnosisVersion.Text;
            _UpdateInfo.HardwareVersion = txtHardwareVersion.Text;

            string strTemp = GlobalObject.GeneralFunction.GetRadioButton(groupBox3);

            if (strTemp != null)
            {
                _UpdateInfo.IsPassTest = strTemp == "通过" ? true : false;
            }

            if (btnDownload_TCUSoft.Tag == null)
            {
                throw new Exception("请上传软件文件");
            }

            _UpdateInfo.ProgramUnique = new Guid(btnDownload_TCUSoft.Tag.ToString());
            _UpdateInfo.TechnicalNote = txtTechnicalNote.Text;
            _UpdateInfo.TestReport = btnDownload_TestReport.Tag == null ? null : (Guid?)new Guid(btnDownload_TestReport.Tag.ToString());
            _UpdateInfo.TestResult = txtTestResult.Text;
            _UpdateInfo.UnderVersion = txtUnderVersion.Text;
            _UpdateInfo.UpdateContent = txtUpdateContent.Text;
            _UpdateInfo.UpdateReason = txtUpdateReason.Text;
            _UpdateInfo.Version = txtCarModelNo.Text + cmbVersionType.Text + txtVersion.Text.Trim().ToLower();

            if (customDataGridView1.Rows.Count != 0)
            {
                _ListDID = (customDataGridView1.DataSource as BindingCollection<View_Business_Project_TCU_SoftwareUpdate_DID>).ToList();
            }
        }

        bool DataCheck()
        {
            try
            {
                if (customDataGridView1.Rows.Count == 0)
                {
                    throw new Exception("请录入【DID】信息");
                }

                if (txtCarModelNo.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【车型代号】");
                }

                if (cmbVersionType.Text.Trim().Length == 0)
                {
                    throw new Exception("请选择【版本号】之前的【版本类型】,V或者S");
                }

                if (txtVersion.Text.Trim().Length < 4 || txtVersion.Text.Trim().Length > 5)
                {
                    throw new Exception("【版本号】长度有误");
                }

                if (txtUpdateContent.Text.Trim().Length == 0)
                {
                    throw new Exception("请录入【升级内容】");
                }

                if (txtUpdateReason.Text.Trim().Length == 0)
                {
                    throw new Exception("请录入【升级原因】");
                }

                if (btnDownload_TCUSoft.Tag == null || btnDownload_TCUSoft.Tag.ToString().Trim().Length == 0)
                {
                    throw new Exception("请上传软件文件");
                }

                if (txtTechnicalNote.Text.Trim().Length == 0)
                {
                    throw new Exception("请录入【技术变更单编号】");
                }

                FlowControlService.IFlowServer serviceFlow = 
                    FlowControlService.ServerModuleFactory.GetServerModule<FlowControlService.IFlowServer>();

                Flow_FlowInfo flow = 
                    serviceFlow.GetNowFlowInfo(serviceFlow.GetBusinessTypeID(CE_BillTypeEnum.TCU软件升级, null), txtBillNo.Text);

                if (flow.FlowID == 72)
                {
                    if (btnDownload_TestReport.Tag == null || btnDownload_TestReport.Tag.ToString().Trim().Length == 0)
                    {
                        throw new Exception("请上传【测试报告】");
                    }

                    if (txtTestResult.Text.Trim().Length == 0)
                    {
                        throw new Exception("请填写【测试结果】");
                    }

                    if (GlobalObject.GeneralFunction.GetRadioButton(groupBox3) == null)
                    {
                        throw new Exception("请选择【测试结论】");
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private bool TCU软件升级明细_PanelGetDataInfo(GlobalObject.CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (flowOperationType == CE_FlowOperationType.提交 && !DataCheck())
                {
                    return false;
                }

                GetInfo();
                _ServiceTCU.CheckVersion(_UpdateInfo);

                FlowInfo_BillNo = txtBillNo.Text;

                List<object> listobj = new List<object>();

                listobj.Add((object)flowOperationType);
                listobj.Add((object)_UpdateInfo);
                listobj.Add((object)_ListDID);

                ResultList = listobj;

                this.KeyWords = "【车型代号】：" + _UpdateInfo.CarModelNo + "；【软件版本号】：" + _UpdateInfo.Version;
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void llbCarModelInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (txtCarModelNo.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【车型代号】");
                return;
            }

            TCU_车型信息 frm = new TCU_车型信息(txtCarModelNo.Text.Trim(), false);
            frm.ShowDialog();
        }

        void FileDownLoad(Button bt, Program_Report pr)
        {
            try
            {
                if (bt.Tag == null || bt.Tag.ToString().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("无附件下载");
                    return;
                }

                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (pr == Program_Report.Program)
                    {
                        FileOperationService.File_DownLoad(new Guid(bt.Tag.ToString()),
                            folderBrowserDialog1.SelectedPath + "\\TCU程序(" + txtCarModelNo.Text.ToUpper() + cmbVersionType.Text + txtVersion.Text.Trim().ToLower() + ")",
                            GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));

                        _ServiceTCU.WriteTxtFile(txtBillNo.Text, folderBrowserDialog1.SelectedPath);
                    }
                    else
                    {
                        FileOperationService.File_DownLoad(new Guid(bt.Tag.ToString()),
                            folderBrowserDialog1.SelectedPath + "\\" + txtBillNo.Text + "_测试报告",
                            GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                    }

                    MessageDialog.ShowPromptMessage("下载成功");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void FileUpLoad(Button bt, Program_Report pr)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (bt.Tag != null && bt.Tag.ToString().Length > 0)
                    {
                        UniversalControlLibrary.FileOperationService.File_Delete(new Guid(bt.Tag.ToString()),
                            GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));
                    }

                    Guid guid = Guid.NewGuid();
                    FileOperationService.File_UpLoad(guid, openFileDialog1.FileName,
                            GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));

                    bt.Tag = guid.ToString();
                    _ServiceTCU.UpdateFilePath(txtBillNo.Text, guid, pr);
                    MessageDialog.ShowPromptMessage("上传成功");

                    if (bt.Name == btnDownload_TCUSoft.Name)
                    {
                        toolTip1.SetToolTip(btnUpload_TCUSoft, openFileDialog1.FileName);
                    }
                    else if (bt.Name == btnDownload_TestReport.Name)
                    {
                        toolTip1.SetToolTip(btnUpload_TestReport, openFileDialog1.FileName);
                    }
                }

                if (btnDownload_TCUSoft.Tag != null && btnDownload_TCUSoft.Tag.ToString().Length > 0)
                {
                    btnDownload_TCUSoft.Enabled = true;
                }

                if (btnDownload_TestReport.Tag != null && btnDownload_TestReport.Tag.ToString().Length > 0)
                {
                    btnDownload_TestReport.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnUpload_TCUSoft_Click(object sender, EventArgs e)
        {
            FileUpLoad(btnDownload_TCUSoft, Program_Report.Program);
        }

        private void btnUpload_TestReport_Click(object sender, EventArgs e)
        {
            FileUpLoad(btnDownload_TestReport, Program_Report.Report);
        }

        private void btnDownload_TCUSoft_Click(object sender, EventArgs e)
        {
            FileDownLoad((Button)sender, Program_Report.Program);
        }

        private void btnDownload_TestReport_Click(object sender, EventArgs e)
        {
            FileDownLoad((Button)sender, Program_Report.Report);
        }

        private void btnAdd_DID_Click(object sender, EventArgs e)
        {
            BindingCollection<View_Business_Project_TCU_SoftwareUpdate_DID> tempList =
                customDataGridView1.DataSource as BindingCollection<View_Business_Project_TCU_SoftwareUpdate_DID>;

            View_Business_Project_TCU_SoftwareUpdate_DID tempLnq = new View_Business_Project_TCU_SoftwareUpdate_DID();

            if (txtDataContent.Text.Length > 0 
                && (int)numDataSize.Value != txtDataContent.Text.Length)
            {
                MessageDialog.ShowPromptMessage("【内容】字节数不等于【字节数】,请重新确认后再【添加】");
                return;
            }

            tempLnq.BillNo = txtBillNo.Text;
            tempLnq.DID = txtDID.Text;
            tempLnq.内容 = txtDataContent.Text;
            tempLnq.字节数 = (int)numDataSize.Value;

            tempList.Add(tempLnq);
            RefreshDataGridView(_ListDID);

            txtDataContent.Text = "";
            txtDID.Text = "";
            numDataSize.Value = 0;
        }

        private void btnDelete_DID_Click(object sender, EventArgs e)
        {
            customDataGridView1.Rows.Remove(customDataGridView1.CurrentRow);
        }
    }
}
