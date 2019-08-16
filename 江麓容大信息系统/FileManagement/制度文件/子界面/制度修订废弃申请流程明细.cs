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
    public partial class 制度修订废弃申请流程明细 : CustomFlowForm
    {
        /// <summary>
        /// 制度流程服务组件
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

        public 制度修订废弃申请流程明细()
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
            txtVersion.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            if (m_lnqInstitution != null)
            {
                txtSDBNo.Text = m_lnqInstitution.BillNo;

                FM_FileList fileInfo = m_serverFileBasic.GetFile((int)m_lnqInstitution.FileID);

                txtFileNo.Tag = fileInfo.FileID;
                txtFileName.Text = fileInfo.FileName;
                txtFileNo.Text = fileInfo.FileNo;
                txtVersion.Text = fileInfo.Version;

                txtRemark.Text = m_lnqInstitution.Introductions;

                lbPropoer.Text = m_lnqInstitution.Propoer;
                lbPropoerTime.Text = m_lnqInstitution.PropoerTime.ToString();
                lbSDBStatus.Text = m_lnqInstitution.BillStatus;
            }
            else
            {
                lbSDBStatus.Text = InstitutionBillStatus.新建流程.ToString();
                lbPropoer.Text = BasicInfo.LoginName;
                lbPropoerTime.Text = ServerTime.Time.ToString();
                txtSDBNo.Text = m_billNoControl.GetNewBillNo();
            }
        }

        bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            m_lnqInstitution = new FM_InstitutionProcess();

            m_lnqInstitution.Introductions = txtRemark.Text;
            m_lnqInstitution.TypeCode = UniversalFunction.GetBillPrefix(CE_BillTypeEnum.制度修订废弃申请流程);
            m_lnqInstitution.FileName = txtFileName.Text;
            m_lnqInstitution.FileNo = txtFileNo.Text;
            m_lnqInstitution.BillNo = txtSDBNo.Text;
            m_lnqInstitution.BillStatus = lbSDBStatus.Text;
            m_lnqInstitution.OperationMode = radioButton1.Checked ? radioButton1.Text : radioButton2.Text;
            m_lnqInstitution.FileID = (int?)txtFileNo.Tag;

            FM_FileList fileInfo = m_serverFileBasic.GetFile((int)m_lnqInstitution.FileID);

            m_lnqInstitution.FileUnique = fileInfo.FileUnique;
            m_lnqInstitution.SortID = fileInfo.SortID;

            ResultInfo = m_lnqInstitution;

            return true;
        }

        private void txtFileNo_OnCompleteSearch()
        {
            txtFileNo.Text = txtFileNo.DataResult["文件编号"].ToString();
            txtFileNo.Tag = Convert.ToInt32(txtFileNo.DataResult["文件ID"]);
            txtFileName.Text = txtFileNo.DataResult["文件名称"].ToString();
            txtVersion.Text = txtFileNo.DataResult["版本号"].ToString();
        }

        private void txtFileNo_Enter(object sender, EventArgs e)
        {
            txtFileNo.StrEndSql = " and 类别ID in (select SortID from FM_FileSort where FileType = " 
                + (int)CE_FileType.制度文件 + " and SortID <> 29)";
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.制度修订废弃申请流程.ToString(), m_serverInstitution);
                m_billMessageServer.BillType = CE_BillTypeEnum.制度修订废弃申请流程.ToString();
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
