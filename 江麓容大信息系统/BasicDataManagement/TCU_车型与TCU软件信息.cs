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

namespace Form_Project_Design
{
    public partial class TCU_车型与TCU软件信息 : Form
    {
        ITCUInfoService _TCUInfoService = Service_Project_Design.ServerModuleFactory.GetServerModule<ITCUInfoService>();

        public TCU_车型与TCU软件信息()
        {
            InitializeComponent();
        }

        private void 车型与TCU软件信息_Load(object sender, EventArgs e)
        {
            ShowInfo();
            RefreshDataGridView(null);
        }

        void ShowInfo()
        {
            GlobalObject.GeneralFunction.LoadTreeViewDt(multiSelectTreeView1,
                _TCUInfoService.GetMotorFactoryTreeInfo(), "ShowName", "ValueCode", "FartherCode", " FartherCode = '0'");
            multiSelectTreeView1.ExpandAll();
        }

        void RefreshDataGridView(string code)
        {
            DataTable dtSource = _TCUInfoService.GetCarModelInfo_Code(code);

            customDataGridView1.DataSource = dtSource;
            userControlDataLocalizer1.Init(customDataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, customDataGridView1.Name, BasicInfo.LoginID));
        }

        private void multiSelectTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            RefreshDataGridView(multiSelectTreeView1.SelectedNode.Tag.ToString());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            TCU_车型信息 frm = new TCU_车型信息(null, true);
            frm.ShowDialog();

            RefreshDataGridView(multiSelectTreeView1.SelectedNode == null ? 
                null : multiSelectTreeView1.SelectedNode.Tag.ToString());
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的记录行");
                return;
            }

            TCU_车型信息 frm = 
                new TCU_车型信息(customDataGridView1.CurrentRow.Cells["车型代号"].Value.ToString(), true);
            frm.ShowDialog();

            RefreshDataGridView(multiSelectTreeView1.SelectedNode == null ?
                null : multiSelectTreeView1.SelectedNode.Tag.ToString());
        }

        private void customDataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            TCU_车型信息 frm =
                new TCU_车型信息(customDataGridView1.CurrentRow.Cells["车型代号"].Value.ToString(), false);
            frm.ShowDialog();

        }
    }
}
