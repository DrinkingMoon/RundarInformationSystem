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
using System.IO;
using FlowControlService;

namespace Form_Project_Design
{
    public partial class 生产BOM表 : Form
    {

        IBOMInfoService _serviceBOMInfo = Service_Project_Design.ServerModuleFactory.GetServerModule<IBOMInfoService>();

        IBomServer _serviceBOM = ServerModule.ServerModuleFactory.GetServerModule<IBomServer>();

        public 生产BOM表()
        {
            InitializeComponent();
        }

        void RefreshInfo()
        {
            if (cmbEdition.Text.Length == 0 || cmbPBOMVersion.Text.Length == 0)
            {
                return;
            }

            customDataGridView1.DataSource = _serviceBOMInfo.GetPBOMLogInfoItems(cmbEdition.Text, cmbPBOMVersion.Text);

            userControlDataLocalizer1.Init(customDataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, customDataGridView1.Name, BasicInfo.LoginID));
        }

        private void 生产BOM表_Load(object sender, EventArgs e)
        {
            List<string> lstAsscembly = _serviceBOM.GetAssemblyTypeList();

            if (lstAsscembly != null)
            {
                cmbEdition.DataSource = lstAsscembly;

                if (cmbEdition.Items.Count > 0)
                {
                    cmbEdition.SelectedIndex = -1;
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            RefreshInfo();
        }

        private void cmbEdition_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbPBOMVersion.Items.Clear();
            cmbPBOMVersion.DataSource = null;

            if (cmbEdition.Text.Length == 0)
            {
                return;
            }

            foreach (decimal item in _serviceBOMInfo.GetDBOMVersionItems(cmbEdition.Text))
            {
                cmbPBOMVersion.Items.Add(item);
            }

            cmbPBOMVersion.SelectedIndex = 0;
        }

        private void btnOutput_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, customDataGridView1);
        }
    }
}
