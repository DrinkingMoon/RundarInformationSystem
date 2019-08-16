using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;
using Service_Economic_Financial;
using GlobalObject;

namespace Form_Economic_Financial
{
    public partial class 财务基础参数配置界面 : Form
    {
        IBasicParametersSetting _serviceParametersSetting =
            Service_Economic_Financial.ServerModuleFactory.GetServerModule<Service_Economic_Financial.IBasicParametersSetting>();

        public 财务基础参数配置界面()
        {
            InitializeComponent();
            ShowInfo();

            btnPurpose_Add.Click += new EventHandler(Add_Click);
            btnStorage_Add.Click += new EventHandler(Add_Click);
            btnSubjects_Add.Click += new EventHandler(Add_Click);
            btnSubjectsPurpose_Add.Click += new EventHandler(Add_Click);
            btnBudgetProject_Add.Click += new EventHandler(Add_Click);

            btnPurpose_Delete.Click += new EventHandler(Delete_Click);
            btnStorage_Delete.Click += new EventHandler(Delete_Click);
            btnSubjects_Delete.Click += new EventHandler(Delete_Click);
            btnSubjectsPurpose_Delete.Click += new EventHandler(Delete_Click);
            btnbtnBudgetProject_Delete.Click += new EventHandler(Delete_Click);

            btnStorage_Modify.Click += new EventHandler(Modify_Click);
            btnSubjects_Modify.Click += new EventHandler(Modify_Click);
            btnPurpose_Modify.Click += new EventHandler(Modify_Click);
            btnBudgetProject_Modify.Click += new EventHandler(Modify_Click);


            for (int i = 0; i < 20; i++)
            {
                cmbYear.Items.Add((2012 + i).ToString());
            }

            dtpStartTime.Value = ServerTime.Time;
            dtpEndTime.Value = ServerTime.Time;

            cmbYear.SelectedIndex = -1;
            cmbMonth.SelectedIndex = -1;
            cmbOperationType.SelectedIndex = -1;
        }

        void Modify_Click(object sender, EventArgs e)
        {
            OperationInfo(CE_OperatorMode.修改, (Button)sender);
        }

        void Delete_Click(object sender, EventArgs e)
        {
            OperationInfo(CE_OperatorMode.删除, (Button)sender);
        }

        void Add_Click(object sender, EventArgs e)
        {
            OperationInfo(CE_OperatorMode.添加, (Button)sender);
        }

        void ShowInfo()
        {
            string tempString1 = tabControl1.SelectedTab.Text;
            string tempString2 = "";

            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is TabControl)
                {
                    tempString2 = (cl as TabControl).SelectedTab.Text;
                    break;
                }
            }

            if (tempString1 == "月度操作")
            {
                dgv_RunLog.DataSource = _serviceParametersSetting.GetRunLog();
                return;
            }

            CE_CW_BasicParameters accountingType =
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CW_BasicParameters>(tempString1 + "_" + tempString2);

            switch (accountingType)
            {
                case CE_CW_BasicParameters.预算_科目:
                    tvBudgetProject.ExecStoreNodeState();
                    tvBudgetProject.Nodes.Clear();
                    GlobalObject.GeneralFunction.LoadTreeViewDt(tvBudgetProject,
                        _serviceParametersSetting.GetTableInfo(accountingType.ToString(), null),
                        "科目名称", "科目代码", "父级科目代码", "父级科目代码 = 'Root'");
                    tvBudgetProject.SetTreeNodeState();

                    txtBudgetProject_Parent.Text = "";
                    txtBudgetProject_Parent.Tag = null;
                    txtBudgetProject.Text = "";
                    txtBudgetProject.Tag = null;

                    tvBudgetProject.SelectedNode = tvBudgetProject.Nodes[0];
                    tvBudgetProject.Nodes[0].Expand();
                    tvBudgetProject_AfterSelect(tvBudgetProject, null);
                    break;

                case CE_CW_BasicParameters.核算_科目:

                    tvSubjects.ExecStoreNodeState();
                    tvSubjects.Nodes.Clear();
                    GlobalObject.GeneralFunction.LoadTreeViewDt(tvSubjects,
                        _serviceParametersSetting.GetTableInfo(accountingType.ToString(), null),
                        "科目名称", "科目代码", "父级科目代码", "父级科目代码 = 'Root'");
                    tvSubjects.SetTreeNodeState();

                    txtSubjects_Name.Text = "";
                    txtSubjects_Code.Text = "";
                    txtSubjects_Parent.Text = "";
                    txtSubjects_Parent.Tag = null;

                    tvSubjects.SelectedNode = tvSubjects.Nodes[0];
                    tvSubjects.Nodes[0].Expand();
                    treeView_AfterSelect(tvSubjects, null);

                    break;
                case CE_CW_BasicParameters.核算_用途:
                    tvPurpose.ExecStoreNodeState();
                    tvPurpose.Nodes.Clear();
                    GlobalObject.GeneralFunction.LoadTreeViewDt(tvPurpose,
                        _serviceParametersSetting.GetTableInfo(accountingType.ToString(), null),
                        "用途名称", "用途代码", "父级用途代码", "父级用途代码 = 'Root'");
                    tvPurpose.SetTreeNodeState();

                    txtPurpose_Code.Text = "";
                    txtPurpose_Name.Text = "";
                    txtPurpose_Parent.Text = "";
                    txtPurpose_ParentName.Text = "";

                    tvPurpose.SelectedNode = tvPurpose.Nodes[0];
                    tvPurpose.Nodes[0].Expand();
                    treeView_AfterSelect(tvPurpose, null);

                    break;
                case CE_CW_BasicParameters.核算_库房:
                    dgvStorage.DataSource = _serviceParametersSetting.GetTableInfo(accountingType.ToString(), null);

                    txtStorage_Code.Text = "";
                    txtStorage_Name.Text = "";
                    txtStorage_Subjects.Text = "";
                    txtStorage_Subjects.Tag = null;

                    foreach (Control cl in groupBox3.Controls)
                    {
                        if (cl is CheckBox)
                        {
                            ((CheckBox)cl).Checked = false;
                        }
                    }

                    break;
                case CE_CW_BasicParameters.核算_科目与用途关系:

                    tvSubjectsPurpose.ExecStoreNodeState();
                    tvSubjectsPurpose.Nodes.Clear();
                    GlobalObject.GeneralFunction.LoadTreeViewDt(tvSubjectsPurpose,
                        _serviceParametersSetting.GetTableInfo(accountingType.ToString(), null),
                        "科目名称", "科目代码", "父级科目代码", "父级科目代码 = 'Root'");
                    tvSubjectsPurpose.SetTreeNodeState();

                    txtSubjectsPurpose_Code.Text = "";
                    txtSubjectsPurpose_Name.Text = "";
                    txtSubjectsPurpose_Subjects.Text = "";
                    txtSubjectsPurpose_Subjects.Tag = null;

                    tvSubjectsPurpose.SelectedNode = tvSubjectsPurpose.Nodes[0];
                    tvSubjectsPurpose.Nodes[0].Expand();
                    treeView_AfterSelect(tvSubjectsPurpose, null);

                    break;
                default:
                    break;
            }
        }

        void SelectNode(MultiSelectTreeView treeView)
        {
            string tempString1 = tabControl1.SelectedTab.Text;
            string tempString2 = "";

            foreach (Control cl in tabControl1.SelectedTab.Controls)
            {
                if (cl is TabControl)
                {
                    tempString2 = (cl as TabControl).SelectedTab.Text;
                    break;
                }
            }

            CE_CW_BasicParameters accountingType =
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CW_BasicParameters>(tempString1 + "_" + tempString2);

            List<string> lstColName = new List<string>();

            switch (accountingType)
            {
                case CE_CW_BasicParameters.核算_科目:
                    dgvSubjects.DataSource = _serviceParametersSetting.GetTableInfo(accountingType.ToString(), treeView.SelectedNode.Tag.ToString());

                    lstColName.Add("科目代码");
                    SelectDataGridViewRow(treeView.SelectedNode, dgvSubjects, lstColName);
                    dgvSubjects_CellClick(null, null);
                    break;
                case CE_CW_BasicParameters.核算_用途:
                    dgvPurpose.DataSource = _serviceParametersSetting.GetTableInfo(accountingType.ToString(), treeView.SelectedNode.Tag.ToString());

                    lstColName.Add("用途代码");
                    SelectDataGridViewRow(treeView.SelectedNode, dgvPurpose, lstColName);
                    dgvPurpose_CellClick(null, null);
                    break;
                case CE_CW_BasicParameters.核算_库房:
                    break;
                case CE_CW_BasicParameters.核算_科目与用途关系:
                    dgvSubjectsPurpose.DataSource = _serviceParametersSetting.GetTableInfo(accountingType.ToString(), treeView.SelectedNode.Tag.ToString());

                    lstColName.Add("用途代码");
                    lstColName.Add("科目代码");
                    SelectDataGridViewRow(treeView.SelectedNode, dgvSubjectsPurpose, lstColName);
                    dgvSubjectsPurpose_CellClick(null, null);
                    break;
                case CE_CW_BasicParameters.预算_科目:
                    dgvBudgetProject.DataSource = _serviceParametersSetting.GetTableInfo(accountingType.ToString(), treeView.SelectedNode.Tag.ToString());

                    lstColName.Add("科目代码");
                    SelectDataGridViewRow(treeView.SelectedNode, dgvBudgetProject, lstColName);
                    dgvBudgetProject_CellClick(null, null);
                    break;
                default:
                    break;
            }
        }

        void SelectDataGridViewRow(TreeNode node, CustomDataGridView selectGridView, List<string> lstColName)
        {
            for (int i = 0; i < selectGridView.Rows.Count; i++)
            {
                bool flag = true;

                foreach (string colName in lstColName)
                {
                    if ((string)selectGridView.Rows[i].Cells[colName].Value != node.Tag.ToString())
                    {
                        flag = false;
                    }
                }

                if (flag)
                {
                    selectGridView.CurrentCell = selectGridView.Rows[i].Cells[1];
                    selectGridView.FirstDisplayedScrollingRowIndex = i;
                }
            }
        }

        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            ShowInfo();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            SelectNode((MultiSelectTreeView)sender);
        }

        private void txtSubjects_Parent_OnCompleteSearch()
        {
            txtSubjects_Parent.Tag = txtSubjects_Parent.DataResult["科目代码"].ToString();
        }

        private void txtPurpose_Parent_OnCompleteSearch()
        {
            txtPurpose_Parent.Text = txtPurpose_Parent.DataResult["用途代码"].ToString();
            txtPurpose_ParentName.Text = txtPurpose_Parent.DataResult["用途名称"].ToString();
        }

        private void txtStorage_Subjects_OnCompleteSearch()
        {
            txtStorage_Subjects.Tag = txtStorage_Subjects.DataResult["科目代码"].ToString();
        }

        private void txtSubjectsPurpose_Subjects_OnCompleteSearch()
        {
            txtSubjectsPurpose_Subjects.Tag = txtSubjectsPurpose_Subjects.DataResult["科目代码"].ToString();
        }

        private void txtSubjectsPurpose_Code_OnCompleteSearch()
        {
            txtSubjectsPurpose_Code.Text = txtSubjectsPurpose_Code.DataResult["用途代码"].ToString();
            txtSubjectsPurpose_Name.Text = txtSubjectsPurpose_Code.DataResult["用途名称"].ToString();
        }

        void OperationInfo(CE_OperatorMode mode, Control cl)
        {
            try
            {
                if (GlobalObject.GeneralFunction.ParentControlIsExist<TabPage>(cl, tpSubjects.Name))
                {
                    if (txtSubjects_Code.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请录入【科目代码】");
                        return;
                    }

                    if (txtSubjects_Name.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请录入【科目名称】");
                        return;
                    }

                    if (txtSubjects_Parent.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择【父级科目】");
                        return;
                    }

                    Business_Base_FinanceSubjects subjects = new Business_Base_FinanceSubjects();

                    subjects.SubjectsName = txtSubjects_Name.Text;
                    subjects.SubjectsCode = txtSubjects_Code.Text;
                    subjects.ParentCode = txtSubjects_Parent.Tag == null ? "" : txtSubjects_Parent.Tag.ToString();

                    _serviceParametersSetting.Operation_FinanceSubjects(mode, subjects);
                }
                else if (GlobalObject.GeneralFunction.ParentControlIsExist<TabPage>(cl, tpPurpose.Name))
                {
                    if (txtPurpose_Code.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请输入【用途代码】");
                        return;
                    }

                    if (txtPurpose_Name.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请输入【用途名称】");
                        return;
                    }

                    if (txtPurpose_ParentName.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择【父级用途】");
                        return;
                    }

                    if (txtPurpose_Parent.Text.Trim().Length >= txtPurpose_Code.Text.Trim().Length ||
                        txtPurpose_Code.Text.Trim().Substring(0, txtPurpose_Parent.Text.Trim().Length) != txtPurpose_Parent.Text.ToString())
                    {
                        MessageDialog.ShowPromptMessage("【用途代码】中前面字符未包含【父级用途】代码,例如：【父级代码】：99，则【用途代码】应为：9901、 9902等等");
                        return;
                    }

                    BASE_MaterialRequisitionPurpose purpose = new BASE_MaterialRequisitionPurpose();

                    purpose.Inventory = chb_Inventory.Checked;
                    purpose.Code = txtPurpose_Code.Text;
                    purpose.IsDisable = true;
                    purpose.IsEnd = true;
                    purpose.Purpose = txtPurpose_Name.Text;
                    purpose.DestructiveInspection = chb_DestructiveInspection.Checked;
                    purpose.ThreeOutSideFit = chb_ThreeOutSideFit.Checked;
                    purpose.ThreeOutSideRepair = chb_ThreeOutSideRepair.Checked;
                    purpose.ApplicableDepartment = txtApplicableDepartment.Tag == null ? "" : txtApplicableDepartment.Tag.ToString();
                    purpose.RemindWord = txtRemindWord.Text;

                    _serviceParametersSetting.Operation_MaterialRequisitionPurpose(mode, purpose, txtPurpose_Parent.Text.Trim());
                }
                else if (GlobalObject.GeneralFunction.ParentControlIsExist<TabPage>(cl, tpStorage.Name))
                {
                    if (txtStorage_Code.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请录入【库房代码】");
                        return;
                    }

                    if (txtStorage_Name.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请录入【库房名称】");
                        return;
                    }

                    if (txtStorage_Subjects.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择【所属科目】");
                        return;
                    }

                    BASE_Storage storage = new BASE_Storage();

                    storage.Aftermarket = chbAftermarket.Checked;
                    storage.AftermarketParts = chbAftermarketParts.Checked;
                    storage.AssemblyWarehouse = chbAssemblyWarehouse.Checked;
                    storage.FinancialAccountingFlag = chbFinancialAccountingFlag.Checked;
                    storage.PartInPlanCalculation = chbPartInPlanCalculation.Checked;
                    storage.SingleFinancialAccountingFlag = chbSingleFinancialAccountingFlag.Checked;
                    storage.WorkShopCurrentAccount = chbWorkShopCurrentAccount.Checked;
                    storage.ZeroCostFlag = chbZeroCostFlag.Checked;

                    storage.StorageID = txtStorage_Code.Text;
                    storage.StorageName = txtStorage_Name.Text;

                    storage.StorageLv = 1;


                    Business_Base_FinanceRelationInfo_Subjects_Storage storageSubjects =
                        new Business_Base_FinanceRelationInfo_Subjects_Storage();

                    storageSubjects.StorageID = txtStorage_Code.Text;
                    storageSubjects.SubjectsCode = txtStorage_Subjects.Tag == null ? "" : txtStorage_Subjects.Tag.ToString();

                    _serviceParametersSetting.Operation_StorageInfo(mode, storage, storageSubjects);
                }
                else if (GlobalObject.GeneralFunction.ParentControlIsExist<TabPage>(cl, tpSubjectsPurpose.Name))
                {
                    if (txtSubjectsPurpose_Code.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择【领料用途】");
                        return;
                    }

                    if (txtSubjectsPurpose_Subjects.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择【所属科目】");
                        return;
                    }

                    Business_Base_FinanceRelationInfo_Subjects_Purpose purposeSubjects =
                        new Business_Base_FinanceRelationInfo_Subjects_Purpose();

                    purposeSubjects.PurposeCode = txtSubjectsPurpose_Code.Text;
                    purposeSubjects.SubjectsCode = txtSubjectsPurpose_Subjects.Tag == null ?
                        "" : txtSubjectsPurpose_Subjects.Tag.ToString();

                    _serviceParametersSetting.Operation_SubjectsPurpose(mode, purposeSubjects);
                }
                else if (GlobalObject.GeneralFunction.ParentControlIsExist<TabPage>(cl, tpBudgetProject.Name))
                {
                    if (txtBudgetProject.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请录入【科目名称】");
                        return;
                    }

                    if (txtBudgetProject_Parent.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请选择【父级科目】");
                        return;
                    }

                    if (txtBudgetProject_Code.Text.Trim().Length == 0)
                    {
                        MessageDialog.ShowPromptMessage("请录入【科目代码】");
                        return;
                    }

                    Business_Base_Finance_Budget_ProjectItem project = new Business_Base_Finance_Budget_ProjectItem();

                    project.PerentProjectID = txtBudgetProject_Parent.Tag == null ? "" : txtBudgetProject_Parent.Tag.ToString();
                    project.ProjectName = txtBudgetProject.Text;
                    project.ProjectID = txtBudgetProject_Code.Text;

                    _serviceParametersSetting.Operation_BudgetProject(mode, project);
                }

                MessageDialog.ShowPromptMessage("操作成功");
                ShowInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void dgvSubjects_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSubjects.CurrentRow == null)
            {
                return;
            }

            txtSubjects_Code.Text = dgvSubjects.CurrentRow.Cells["科目代码"].Value.ToString();
            txtSubjects_Name.Text = dgvSubjects.CurrentRow.Cells["科目名称"].Value.ToString();
            txtSubjects_Parent.Tag = dgvSubjects.CurrentRow.Cells["父级科目代码"].Value.ToString() == "Root" ?
                "" : dgvSubjects.CurrentRow.Cells["父级科目代码"].Value.ToString();

            Business_Base_FinanceSubjects temp =
                _serviceParametersSetting.GetFinanceSubjectsSingle(dgvSubjects.CurrentRow.Cells["父级科目代码"].Value.ToString());

            txtSubjects_Parent.Text = temp == null ? "核算科目" : temp.SubjectsName;
        }

        private void dgvPurpose_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPurpose.CurrentRow == null)
            {
                return;
            }

            txtPurpose_Code.Text = dgvPurpose.CurrentRow.Cells["用途代码"].Value.ToString();
            txtPurpose_Name.Text = dgvPurpose.CurrentRow.Cells["用途名称"].Value.ToString();
            txtPurpose_Parent.Text = dgvPurpose.CurrentRow.Cells["父级用途代码"].Value.ToString() == "Root" ?
                "" : dgvPurpose.CurrentRow.Cells["父级用途代码"].Value.ToString();

            txtRemindWord.Text =
                dgvPurpose.CurrentRow.Cells["提醒语句"].Value == null ? "" : dgvPurpose.CurrentRow.Cells["提醒语句"].Value.ToString();
            txtApplicableDepartment.Text =
                dgvPurpose.CurrentRow.Cells["适用部门"].Value == null ? "" : dgvPurpose.CurrentRow.Cells["适用部门"].Value.ToString();

            if (txtApplicableDepartment.Text.Trim().Length > 0)
            {
                string strDept = "";
                foreach (string str in txtApplicableDepartment.Text.Trim().Split(',').ToList())
                {
                    strDept += UniversalFunction.GetDeptCode(str) + ",";
                }

                strDept = strDept.Substring(0, strDept.Length - 1);
                txtApplicableDepartment.Tag = strDept;
            }
            else
            {
                txtApplicableDepartment.Tag = null;
            }

            chb_Inventory.Checked =
                dgvPurpose.CurrentRow.Cells["盘点"].Value == DBNull.Value ? false : Convert.ToBoolean(dgvPurpose.CurrentRow.Cells["盘点"].Value);
            chb_DestructiveInspection.Checked =
                dgvPurpose.CurrentRow.Cells["破坏性检测"].Value == DBNull.Value ? false : Convert.ToBoolean(dgvPurpose.CurrentRow.Cells["破坏性检测"].Value);
            chb_ThreeOutSideFit.Checked =
                dgvPurpose.CurrentRow.Cells["三包外装配"].Value == DBNull.Value ? false : Convert.ToBoolean(dgvPurpose.CurrentRow.Cells["三包外装配"].Value);
            chb_ThreeOutSideRepair.Checked =
                dgvPurpose.CurrentRow.Cells["三包外维修"].Value == DBNull.Value ? false : Convert.ToBoolean(dgvPurpose.CurrentRow.Cells["三包外维修"].Value);

            BASE_MaterialRequisitionPurpose temp =
                _serviceParametersSetting.GetMaterialRequisitionPurposeSingle(dgvPurpose.CurrentRow.Cells["父级用途代码"].Value.ToString());

            txtPurpose_ParentName.Text = temp == null ? "领料用途" : temp.Purpose;
        }

        private void dgvSubjectsPurpose_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvSubjectsPurpose.CurrentRow == null)
            {
                return;
            }

            txtSubjectsPurpose_Code.Text = dgvSubjectsPurpose.CurrentRow.Cells["用途代码"].Value.ToString();
            txtSubjectsPurpose_Name.Text = dgvSubjectsPurpose.CurrentRow.Cells["用途名称"].Value.ToString();
            txtSubjectsPurpose_Subjects.Text = dgvSubjectsPurpose.CurrentRow.Cells["科目名称"].Value.ToString();
            txtSubjectsPurpose_Subjects.Tag = dgvSubjectsPurpose.CurrentRow.Cells["科目代码"].Value.ToString();
        }

        private void dgvStorage_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvStorage.CurrentRow == null)
            {
                return;
            }

            txtStorage_Code.Text = dgvStorage.CurrentRow.Cells["库房代码"].Value.ToString();
            txtStorage_Name.Text = dgvStorage.CurrentRow.Cells["库房名称"].Value.ToString();
            txtStorage_Subjects.Tag = dgvStorage.CurrentRow.Cells["所属科目代码"].Value.ToString();
            txtStorage_Subjects.Text = dgvStorage.CurrentRow.Cells["所属科目"].Value.ToString();

            foreach (Control cl in groupBox3.Controls)
            {
                if (cl is CheckBox)
                {
                    foreach (DataGridViewColumn dgvcl in dgvStorage.Columns)
                    {
                        if (dgvcl.Name == ((CheckBox)cl).Text)
                        {
                            ((CheckBox)cl).Checked = (bool)dgvStorage.CurrentRow.Cells[dgvcl.Name].Value;
                            continue;
                        }
                    }
                }
            }
        }

        private void btnSelectDepartment_Click(object sender, EventArgs e)
        {
            IDepartmentServer service = ServerModule.ServerModuleFactory.GetServerModule<IDepartmentServer>();

            DataTable dtTemp = new DataTable();

            dtTemp.Columns.Add("部门编码");

            if (txtApplicableDepartment.Text.Trim().Length > 0)
            {
                foreach (string str in txtApplicableDepartment.Tag.ToString().Split(',').ToList())
                {
                    DataRow dr = dtTemp.NewRow();
                    dr["部门编码"] = str;
                    dtTemp.Rows.Add(dr);
                }
            }

            List<string> lstTemp = new List<string>();

            lstTemp.Add("部门编码");

            FormDataTableCheck frm = new FormDataTableCheck(service.GetDepartment_Finance(), dtTemp, lstTemp);
            frm._BlDateTimeControlShow = false;
            frm._BlIsCheckBox = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                txtApplicableDepartment.Text = "";
                txtApplicableDepartment.Tag = "";

                foreach (DataRow dr in frm._DtResult.Rows)
                {
                    txtApplicableDepartment.Text += dr["部门名称"].ToString() + ",";
                    txtApplicableDepartment.Tag += dr["部门编码"].ToString() + ",";
                }

                if (txtApplicableDepartment.Text.Trim().Length > 0)
                {
                    txtApplicableDepartment.Text =
                        txtApplicableDepartment.Text.Substring(0, txtApplicableDepartment.Text.Length - 1);
                    txtApplicableDepartment.Tag =
                        txtApplicableDepartment.Tag.ToString().Substring(0, txtApplicableDepartment.Tag.ToString().Length - 1);
                }
            }
        }

        private void tvBudgetProject_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void dgvBudgetProject_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvBudgetProject.CurrentRow == null)
            {
                return;
            }

            txtBudgetProject_Code.Text = dgvBudgetProject.CurrentRow.Cells["科目代码"].Value.ToString();
            txtBudgetProject.Text = dgvBudgetProject.CurrentRow.Cells["科目名称"].Value.ToString();
            txtBudgetProject_Parent.Tag = dgvBudgetProject.CurrentRow.Cells["父级科目代码"].Value.ToString() == "Root" ?
                "" : dgvBudgetProject.CurrentRow.Cells["父级科目代码"].Value.ToString();
            txtBudgetProject_Parent.Text = dgvBudgetProject.CurrentRow.Cells["父级科目"].Value.ToString();
        }

        private void txtBudgetProject_Parent_OnCompleteSearch()
        {
            txtBudgetProject_Parent.Tag = txtBudgetProject_Parent.DataResult["科目代码"].ToString();
        }

        private void btnOperation_Click(object sender, EventArgs e)
        {
            try
            {
                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbOperationType.Text))
                {
                    throw new Exception("请选择【操作类型】");
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbYear.Text))
                {
                    throw new Exception("请选择【年份】");
                }

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbOperationType.Text))
                {
                    throw new Exception("请选择【月份】");
                }

                if (dtpEndTime.Value < dtpStartTime.Value)
                {
                    throw new Exception("【起始时间】需要小于【截止时间】");
                }

                if (MessageDialog.ShowEnquiryMessage("即将开始生成【" + cmbOperationType.Text + "】,请再次确认信息正确，点击【是】继续 ") == DialogResult.No)
                {
                    return;
                }

                if (_serviceParametersSetting.GetCount(cmbOperationType.Text, cmbYear.Text + cmbMonth.Text).Count() > 0)
                {
                    if (_serviceParametersSetting.IsExistAccountBill(cmbYear.Text + cmbMonth.Text))
                    {
                        throw new Exception("挂账单据已存在于供应商应付账款业务中，无法重复执行");
                    }

                    if (MessageDialog.ShowEnquiryMessage("【" + cmbYear.Text + cmbMonth.Text + "】的" + cmbOperationType.Text + "已操作完成，重复执行可能会导致【"
                        + cmbYear.Text + cmbMonth.Text + "】的账务出错，是否继续？") == DialogResult.No)
                    {
                        return;
                    }
                }

                this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                this.btnOperation.Enabled = false;

                _serviceParametersSetting.OperationMonthlyBalance(cmbOperationType.Text,
                    cmbYear.Text + cmbMonth.Text, dtpStartTime.Value, dtpEndTime.Value);
                dgv_RunLog.DataSource = _serviceParametersSetting.GetRunLog();

                MessageDialog.ShowPromptMessage("操作完成");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
            finally
            {
                this.btnOperation.Enabled = true;
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
            }
        }

        private void comYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbYear.Text)
                || GlobalObject.GeneralFunction.IsNullOrEmpty(cmbMonth.Text)
                || GlobalObject.GeneralFunction.IsNullOrEmpty(cmbOperationType.Text))
            {
                return;
            }

            List<DateTime?> lstTime = _serviceParametersSetting.GetListTime(cmbOperationType.Text, cmbYear.Text + cmbMonth.Text);

            if (lstTime.Count() > 0 && lstTime[0] != null)
            {
                dtpStartTime.Value = (DateTime)lstTime[0];
                dtpStartTime.Enabled = false;
            }
            else
            {
                dtpStartTime.Enabled = true;
            }

            if (lstTime.Count() > 1 && lstTime[1] != null)
            {
                dtpEndTime.Value = (DateTime)lstTime[1];
                dtpEndTime.Enabled = false;
            }
            else
            {
                dtpEndTime.Enabled = true;
            }

        }
    }
}
