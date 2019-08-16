using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;


namespace Expression
{
    /// <summary>
    /// 用于选择多个用户的窗体
    /// </summary>
    public partial class FormSelectUsers : Form
    {
        /// <summary>
        /// 用户操作数据库接口
        /// </summary>
        IUserManagement m_userDb = PlatformFactory.GetObject<IUserManagement>();

        /// <summary>
        /// 用户列表
        /// </summary>
        List<View_Auth_User> m_lstAllUser;

        /// <summary>
        /// 选择的用户
        /// </summary>
        List<View_Auth_User> m_selectedUsers;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 已经存在的用户
        /// </summary>
        string m_existUsers;

        /// <summary>
        /// 所有用户
        /// </summary>
        internal List<View_Auth_User> AllUsers
        {
            get { return m_lstAllUser; }
            set { m_lstAllUser = value; }
        }

        /// <summary>
        /// 获取或设置选择的用户
        /// </summary>
        internal List<View_Auth_User> SelectedUsers
        {
            get { return m_selectedUsers; }
            set { m_selectedUsers = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="existUsers">已经选中的用户</param>
        public FormSelectUsers(string existUsers)
        {
            InitializeComponent();
            m_existUsers = existUsers;
        }

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormSelectUsers_Load(object sender, EventArgs e)
        {
            try
            {
                if (AllUsers == null)
                {
                    AllUsers = m_userDb.GetAllUser().ToList();
                }

                dataGridView.DataSource = AllUsers;

                foreach (DataGridViewColumn item in dataGridView.Columns)
                {
                    item.ReadOnly = true;
                    item.Width = 120;
                    item.Visible = false;
                }

                dataGridView.Columns["登录名"].Width = 120;
                dataGridView.Columns["登录名"].Visible = true;
                dataGridView.Columns["姓名"].Width = 120;
                dataGridView.Columns["姓名"].Visible = true;
                dataGridView.Columns["部门"].Width = 120;
                dataGridView.Columns["部门"].Visible = true;

                // 添加数据定位控件
                m_dataLocalizer = new UserControlDataLocalizer(
                    dataGridView, this.Name, ServerModule.UniversalFunction.SelectHideFields(
                                                          this.Name, dataGridView.Name, BasicInfo.LoginID));
                panelTop.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
                m_dataLocalizer.Visible = true;

                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                column.Visible = true;
                column.Name = "选中";
                column.HeaderText = "选中";
                column.ReadOnly = false;
                column.Frozen = false;

                dataGridView.Columns.Insert(0, column);
                dataGridView.Columns["选中"].Width = 68;

                if (!GlobalObject.GeneralFunction.IsNullOrEmpty(m_existUsers))
                {
                    string[] users = m_existUsers.Split(new char[] { ',' });

                    for (int i = 0; i < AllUsers.Count; i++)
                    {
                        if (users.Contains(AllUsers[i].登录名))
                        {
                            dataGridView.Rows[i].Cells[0].Value = true;
                            dataGridView.Rows[i].Cells[0].Selected = true;
                        }
                        else
                        {
                            dataGridView.Rows[i].Cells[0].Value = false;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
                return;
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
            m_selectedUsers = new List<View_Auth_User>();

            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if (item.Cells[0].Value != null && (bool)item.Cells[0].Value)
                {
                    m_selectedUsers.Add(m_lstAllUser[index]);
                }

                index++;
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
    }
}
