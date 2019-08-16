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
using Service_Peripheral_HR;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 用于选择所需的业务用户的窗体
    /// </summary>
    public partial class FormSelectPersonnel : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 选中的数据行数
        /// </summary>
        string m_count;

        /// <summary>
        /// 类型
        /// </summary>
        string m_type;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnelInfo = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 部门管理类
        /// </summary>
        IOrganizationServer m_deptServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 查找到的符合条件的部门信息
        /// </summary>
        IQueryable<View_HR_Dept> m_findDepartment;

        /// <summary>
        /// 用户列表
        /// </summary>
        List<View_SelectPersonnel> m_lstAllUser;

        /// <summary>
        /// 选择的用户
        /// </summary>
        List<View_SelectPersonnel> m_selectedUser;

        /// <summary>
        /// 部门列表
        /// </summary>
        List<View_HR_Dept> m_lstAllDept;

        /// <summary>
        /// 选择的部门
        /// </summary>
        List<View_HR_Dept> m_selectedDept;

        /// <summary>
        /// 所有部门
        /// </summary>
        internal List<View_HR_Dept> AllDept
        {
            get { return m_lstAllDept; }
            set { m_lstAllDept = value; }
        }

        /// <summary>
        /// 获取或设置选择的部门
        /// </summary>
        internal List<View_HR_Dept> SelectedDept
        {
            get { return m_selectedDept; }
            set { m_selectedDept = value; }
        }

        /// <summary>
        /// 获取或设置部门编码
        /// </summary>
        public string DeptCode
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置行数
        /// </summary>
        internal string Count
        {
            get { return m_count; }
            set { m_count = value; }
        }

        /// <summary>
        /// 所有用户
        /// </summary>
        internal List<View_SelectPersonnel> AllUser
        {
            get { return m_lstAllUser; }
            set { m_lstAllUser = value; }
        }

        /// <summary>
        /// 获取或设置选择的用户
        /// </summary>
        public List<View_SelectPersonnel> SelectedUser
        {
            get { return m_selectedUser; }
            set { m_selectedUser = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">类型（员工或部门）</param>
        public FormSelectPersonnel(string type)
        {
            InitializeComponent();

            m_type = type;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSelectUser_Load(object sender, EventArgs e)
        {
            try
            {
                if (m_type == "员工")
                {
                    if (AllUser == null)
                    {
                        if (DeptCode == null)
                        {
                            AllUser = m_personnelInfo.GetAllInfo().ToList();
                        }
                        else
                        {
                            string[] dept = DeptCode.Split(';');

                            if (dept.Length > 0)
                            {
                                string sql = "";

                                for (int i = 0; i < dept.Length; i++)
                                {
                                    if (i == 0)
                                    {
                                        sql += " and 部门编号 in (select DeptCode from fun_get_BelongDept('" + dept[i] + "'))";
                                    }
                                    else if (dept[i] != "")
                                    {
                                        sql += " or 部门编号 in (select DeptCode from fun_get_BelongDept('" + dept[i] + "'))";
                                    }
                                }

                                AllUser = m_personnelInfo.GetPersonByDept(sql).ToList();
                            }
                            else
                            {
                                AllUser = m_personnelInfo.GetAllInfo(DeptCode).ToList();
                            }
                        }
                    }

                    DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                    column.Visible = true;
                    column.Name = "选中";
                    column.HeaderText = "选中";
                    column.ReadOnly = false;

                    dataGridView.Columns.Add(column);

                    dataGridView.Columns.Add("员工姓名", "员工姓名");
                    dataGridView.Columns.Add("员工编号", "员工编号");
                    dataGridView.Columns.Add("部门", "部门");

                    foreach (DataGridViewColumn item in dataGridView.Columns)
                    {
                        if (item.Name != "选中")
                        {
                            item.ReadOnly = true;
                            item.Width = item.HeaderText.Length * (int)this.Font.Size + 100;
                        }
                        else
                        {
                            item.Width = 68;
                            item.ReadOnly = false;
                            item.Frozen = false;
                        }
                    }

                    bool selectedFlag = false;
                    int count = 0;

                    foreach (var item in AllUser)
                    {
                        selectedFlag = false;

                        if (SelectedUser != null && count < SelectedUser.Count)
                        {
                            if (SelectedUser.FindIndex(c => c.员工编号 == item.员工编号) >= 0)
                            {
                                selectedFlag = true;
                                count++;
                            }
                        }

                        dataGridView.Rows.Add(new object[] { selectedFlag, item.员工姓名, item.员工编号, item.部门 });
                    }
                }

                if (m_type == "部门")
                {
                    if (AllDept == null)
                    {
                        if (!m_deptServer.GetAllDeptInfo(out m_findDepartment, out error))
                        {
                            MessageDialog.ShowErrorMessage(error);
                            return;
                        }

                        AllDept = m_findDepartment.ToList();
                    }

                    DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                    column.Visible = true;
                    column.Name = "选中";
                    column.HeaderText = "选中";
                    column.ReadOnly = false;

                    dataGridView.Columns.Add(column);

                    dataGridView.Columns.Add("部门代码", "部门代码");
                    dataGridView.Columns.Add("部门名称", "部门名称");
                    dataGridView.Columns.Add("部门类型", "部门类型");

                    foreach (DataGridViewColumn item in dataGridView.Columns)
                    {
                        if (item.Name != "选中")
                        {
                            item.ReadOnly = true;
                            item.Width = item.HeaderText.Length * (int)this.Font.Size + 100;
                        }
                        else
                        {
                            item.Width = 68;
                            item.ReadOnly = false;
                            item.Frozen = false;
                        }
                    }

                    bool selectedFlag = false;
                    int count = 0;

                    foreach (var item in AllDept)
                    {
                        selectedFlag = false;

                        if (SelectedDept != null && count < SelectedDept.Count)
                        {
                            if (SelectedDept.FindIndex(c => c.部门代码 == item.部门代码) >= 0)
                            {
                                selectedFlag = true;
                                count++;
                            }
                        }

                        dataGridView.Rows.Add(new object[] { selectedFlag, item.部门代码, item.部门名称, item.部门类型 });
                    }
                }

                userControlDataLocalizer1.Init(dataGridView, this.Name,
                        UniversalFunction.SelectHideFields(this.Name, dataGridView.Name, BasicInfo.LoginID));

                lblSumCount.Text = dataGridView.Rows.Count.ToString();
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
            }
        }

        /// <summary>
        /// 点击选择按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectField_Click(object sender, EventArgs e)
        {
            int index = 0;

            if (AllUser != null)
            {
                m_selectedUser = new List<View_SelectPersonnel>();

                foreach (DataGridViewRow item in dataGridView.Rows)
                {
                    if ((bool)item.Cells["选中"].Value)
                    {
                        View_SelectPersonnel user = (from r in m_lstAllUser
                                                     where r.员工编号 == item.Cells["员工编号"].Value.ToString()
                                                         select r).Single();
                        m_selectedUser.Add(user);
                    }

                    index++;
                }

                if (m_selectedUser.Count() == 0)
                {
                    MessageDialog.ShowPromptMessage("请在【选中】框内勾选人员");
                    return;
                }
            }
            else if (AllDept != null)
            {
                m_selectedDept = new List<View_HR_Dept>();

                foreach (DataGridViewRow item in dataGridView.Rows)
                {
                    if ((bool)item.Cells["选中"].Value)
                    {
                        View_HR_Dept user = (from r in m_lstAllDept
                                             where r.部门代码 == item.Cells["部门代码"].Value.ToString()
                                                         select r).Single();
                        m_selectedDept.Add(user);
                    }

                    index++;
                }

                if (m_selectedDept.Count == index)
                {
                    m_count = "全部";
                }
            }

            this.DialogResult = DialogResult.OK;
        }
        
        /// <summary>
        /// 点击关闭按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// 用于确定数据控件中的检查框值已经更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dataGridView.IsCurrentCellDirty)
            {
                dataGridView.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        /// <summary>
        /// 根据参数选中界面上的数据
        /// </summary>
        /// <param name="selectedFlag">为真则选中所有数据，为假则清除所有选择</param>
        private void SelectDataRow(bool selectedFlag)
        {
            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if (item.Visible)
                {
                    item.Cells["选中"].Value = selectedFlag;
                }
            }

            if (dataGridView.CurrentRow != null)
                dataGridView.CurrentCell = dataGridView.CurrentRow.Cells[1];
        }

        /// <summary>
        /// 根据参数显示界面上的数据
        /// </summary>
        /// <param name="selectedFlag">为真则显示所有选中数据，为假则显示所有未选中数据</param>
        private void ShowSelectedDataRow(bool selectedFlag)
        {
            int count = 0;

            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if ((bool)item.Cells["选中"].Value)
                {
                    item.Visible = selectedFlag;
                    count += 1;
                }
                else
                {
                    item.Visible = !selectedFlag;
                }
            }

            if (selectedFlag)
            {
                lblSumCount.Text = count.ToString();
            }
            else
            {
                lblSumCount.Text = (dataGridView.Rows.Count - count).ToString();
            }
        }

        /// <summary>
        /// 选择所有
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            SelectDataRow(true);
        }

        /// <summary>
        /// 清除所有选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClearAll_Click(object sender, EventArgs e)
        {
            SelectDataRow(false);
        }

        /// <summary>
        /// 显示所有选中用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowSelectedData_Click(object sender, EventArgs e)
        {
            ShowSelectedDataRow(true);
        }

        /// <summary>
        /// 显示所有未选中用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnShowUnselectedData_Click(object sender, EventArgs e)
        {
            ShowSelectedDataRow(false);
        }
    }
}
