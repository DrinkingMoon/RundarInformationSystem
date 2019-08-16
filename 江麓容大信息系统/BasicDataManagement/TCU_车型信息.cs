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
using Service_Project_Design;
using FlowControlService;
using System.Text.RegularExpressions;

namespace Form_Project_Design
{
    public partial class TCU_车型信息 : Form
    {
        ITCUInfoService _ServiceTCU = Service_Project_Design.ServerModuleFactory.GetServerModule<ITCUInfoService>();

        TCU_CarModelInfo _CarModelInfo = new TCU_CarModelInfo();

        object _DetailInfo = null;

        List<TabPage> lstTabPage = new List<TabPage>();

        public TCU_车型信息(string carModelNo, bool isEdit)
        {
            InitializeComponent();
            _CarModelInfo = _ServiceTCU.GetCarModelInfo(carModelNo);

            foreach (TabPage tp in tabControl1.TabPages)
            {
                lstTabPage.Add(tp);
            }
            
            btnSave.Visible = isEdit;
            lbDLLName.Enabled = isEdit;
        }

        private void TCU_车型信息_Load(object sender, EventArgs e)
        {
            foreach (Control cl in plCarModelType.Controls)
            {
                if (cl is RadioButton)
                {
                    ((RadioButton)cl).CheckedChanged += new EventHandler(rbCarModel_CheckedChanged);
                }
            }

            ShowInfo();
        }

        void rbCarModel_CheckedChanged(object sender, EventArgs e)
        {
            if (((RadioButton)sender).Checked)
            {
                foreach (TabPage tp in lstTabPage)
                {
                    if (tp.Name == ((RadioButton)sender).Text)
                    {
                        tabControl1.TabPages.Clear();
                        tabControl1.TabPages.Add(tp);
                    }
                }
            }
        }

        void ShowInfo_Tradition()
        {
            TCU_CarModelInfo_Tradition tradition = _ServiceTCU.GetCarModelInfo_Tradition(_CarModelInfo.CarModelNo);

            if (tradition == null)
            {
                return;
            }

            txtModelName.Text = tradition.ModelName;
            txtCVTModel.Text = tradition.CVTModel;
            txtEngineModel.Text = tradition.EngineModel;
            txtEngineCC.Text = tradition.EngineCC;
            txtUseArea.Text = tradition.UseArea;
            txtEMSProvider.Text = tradition.EMSProvider;
            txtTCUProvider.Text = tradition.TCUProvider;
            txtTireSize.Text = tradition.TireSize;
            txtQRCode_FactoryCode.Text = tradition.QRCode_FactoryCode;
            txtQRCode_PartsCode.Text = tradition.QRCode_PartsCode;
            txtQRCode_PartsType.Text = tradition.QRCode_PartsType;
            txtQRCode_Provider.Text = tradition.QRCode_Provider;

            GlobalObject.GeneralFunction.SetRadioButton(tradition.Diagnostics, plDiagnostics);

            foreach (Control cl in groupBox2.Controls)
            {
                List<string> lstPropertyName = GlobalObject.GeneralFunction.GetItemPropertyName(tradition);

                foreach (string name in lstPropertyName)
                {
                    if (cl.Tag != null && cl.Tag.ToString() == name)
                    {
                        GlobalObject.GeneralFunction.SetRadioButton(GeneralFunction.GetItemValue<TCU_CarModelInfo_Tradition>(tradition, name), cl);
                    }
                }
            }
        }

        void ShowInfo()
        {
            if (_CarModelInfo == null)
            {
                return;
            }

            foreach (TabPage tp in lstTabPage)
            {
                if (tp.Name == _CarModelInfo.CarModelType)
                {
                    tabControl1.TabPages.Clear();
                    tabControl1.TabPages.Add(tp);
                }
            }

            CE_CarModelType type = GlobalObject.GeneralFunction.StringConvertToEnum<CE_CarModelType>(_CarModelInfo.CarModelType);

            switch (type)
            {
                case CE_CarModelType.传统车型:
                    ShowInfo_Tradition();
                    break;
                case CE_CarModelType.新能源车型:
                    break;
                default:
                    break;
            }

            txtCarModelNo.Text = _CarModelInfo.CarModelNo;
            txtFactoryID.Tag = _CarModelInfo.FactoryCode;
            txtFactoryID.Text = _ServiceTCU.GetFactoryInfo(_CarModelInfo.FactoryCode).FactoryShortName;
            chbIsOff.Checked = _CarModelInfo.IsOff;

            if (_CarModelInfo.DLLFileUnique != null)
            {
                lbDLLName.Tag = _CarModelInfo.DLLFileUnique;
                lbDLLName.Text = _CarModelInfo.DLLName;
            }

            GlobalObject.GeneralFunction.SetRadioButton(_CarModelInfo.CarModelType, plCarModelType);

            txtCarModelNo.Enabled = false;
            txtFactoryID.Enabled = false;
            plCarModelType.Enabled = false;
        }

        void GetInfo()
        {
            CheckInfo();

            if (_CarModelInfo == null)
            {
                _CarModelInfo = new TCU_CarModelInfo();

                _CarModelInfo.CarModelNo = txtCarModelNo.Text.ToUpper();
                _CarModelInfo.FactoryCode = txtFactoryID.Tag.ToString();
                _CarModelInfo.CarModelType = GlobalObject.GeneralFunction.GetRadioButton(plCarModelType);
            }

            _CarModelInfo.IsOff = chbIsOff.Checked;
            _CarModelInfo.DLLName = lbDLLName.Text;
            _CarModelInfo.DLLFileUnique = new Guid(lbDLLName.Tag.ToString());

            CE_CarModelType type = GlobalObject.GeneralFunction.StringConvertToEnum<CE_CarModelType>(_CarModelInfo.CarModelType);

            switch (type)
            {
                case CE_CarModelType.传统车型:
                    _DetailInfo = GetInfo_Tradition();
                    break;
                case CE_CarModelType.新能源车型:
                    break;
                default:
                    break;
            }
        }

        object GetInfo_Tradition()
        {
            TCU_CarModelInfo_Tradition result = new TCU_CarModelInfo_Tradition();

            CheckInfo_Tradition();

            result.CarModelNo = txtCarModelNo.Text;
            result.ModelName = txtModelName.Text;
            result.CVTModel = txtCVTModel.Text;
            result.EngineModel = txtEngineModel.Text;
            result.EngineCC = txtEngineCC.Text;
            result.UseArea = txtUseArea.Text;
            result.EMSProvider = txtEMSProvider.Text;
            result.TCUProvider = txtTCUProvider.Text;
            result.TireSize = txtTireSize.Text;
            result.QRCode_FactoryCode = txtQRCode_FactoryCode.Text;
            result.QRCode_PartsCode = txtQRCode_PartsCode.Text;
            result.QRCode_PartsType = txtQRCode_PartsType.Text;
            result.QRCode_Provider = txtQRCode_Provider.Text;
            result.Diagnostics = GlobalObject.GeneralFunction.GetRadioButton(plDiagnostics);

            foreach (Control cl in groupBox2.Controls)
            {
                List<string> lstPropertyName = GlobalObject.GeneralFunction.GetItemPropertyName(result);

                foreach (string name in lstPropertyName)
                {
                    if (cl.Tag != null && cl.Tag.ToString() == name)
                    {
                        GlobalObject.GeneralFunction.SetValue(result, name, GlobalObject.GeneralFunction.GetRadioButton(cl));
                    }
                }
            }

            return result;
        }

        void CheckInfo_Tradition()
        {

        }

        void CheckInfo()
        {
            if (txtFactoryID.Text.Trim().Length == 0 || txtFactoryID.Tag == null)
            {
                throw new Exception("请选择【整车厂】");
            }

            if (txtCarModelNo.Text.Trim().Length == 0)
            {
                throw new Exception("请填写【车型代号】");
            }

            if (GlobalObject.GeneralFunction.GetRadioButton(plCarModelType) == null)
            {
                throw new Exception("请选择【车型类别】");
            }

            if (txtQRCode_FactoryCode.Text.Trim().Length == 0)
            {
                throw new Exception("请填写【二维码】【工厂代号匹配电阻】");
            }

            if (txtQRCode_PartsCode.Text.Trim().Length == 0)
            {
                throw new Exception("请填写【二维码】【容大零件图号】");
            }

            if (txtQRCode_PartsType.Text.Trim().Length == 0)
            {
                throw new Exception("请填写【二维码】【零部件种类状态代码】");
            }

            if (txtQRCode_Provider.Text.Trim().Length == 0)
            {
                throw new Exception("请填写【二维码】【供应商代码】");
            }

            if (lbDLLName.Tag == null || GlobalObject.GeneralFunction.IsNullOrEmpty(lbDLLName.Tag.ToString()))
            {
                throw new Exception("请上传DLL文件");
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                GetInfo();
                _ServiceTCU.SaveCarModelInfo(_CarModelInfo, _DetailInfo);
                MessageDialog.ShowPromptMessage("保存成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void txtFactoryID_OnCompleteSearch()
        {
            if (txtFactoryID.DataResult == null)
            {
                return;
            }

            txtFactoryID.Tag = txtFactoryID.DataResult["编码"].ToString();
        }

        private void btnSelectSoftware_Click(object sender, EventArgs e)
        {
            if (txtCarModelNo.Text.Trim().Length == 0)
            {
                return;
            }

            UniversalControlLibrary.FormDataShow frm = 
                new FormDataShow(_ServiceTCU.GetSoftWareVersionListInfo(txtCarModelNo.Text));
            frm.dataGridView1.DoubleClick += new EventHandler(dataGridView1_DoubleClick);
            frm.ShowDialog();
        }

        void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            DataGridView dgv = (DataGridView)sender;

            if (dgv.CurrentRow == null)
            {
                return;
            }

            TCU软件升级明细 frm = new TCU软件升级明细();
            frm.FlowInfo_BillNo = dgv.CurrentRow.Cells["业务编号"].Value.ToString();
            frm.LoadFormInfo();
            frm.Show();
        }

        private void lbDLLName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    string fileName = openFileDialog1.SafeFileName.Substring(0, openFileDialog1.SafeFileName.LastIndexOf("."));

                    if (!Regex.IsMatch(fileName, @"^"+ txtCarModelNo.Text.Substring(0, 2) +@"\d{3}$"))
                    {
                        throw new Exception("文件名不匹配，无法上传");
                    }

                    Guid guid = Guid.NewGuid();
                    FileOperationService.File_UpLoad(guid, openFileDialog1.FileName,
                            GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)CE_SwitchName.文件传输方式]));

                    MessageDialog.ShowPromptMessage("上传成功");

                    lbDLLName.Tag = guid.ToString();
                    lbDLLName.Text = fileName;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }
    }
}
