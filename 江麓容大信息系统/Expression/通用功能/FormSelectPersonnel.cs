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

namespace Expression
{
    /// <summary>
    /// 用于选择所需的业务用户的窗体
    /// </summary>
    public partial class FormSelectPersonnel : Form
    {
        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_personnelInfo = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 用户列表
        /// </summary>
        List<View_HR_Personnel> m_lstAllUser;

        /// <summary>
        /// 选择的用户
        /// </summary>
        List<View_HR_Personnel> m_selectedUser;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 获取或设置部门编码
        /// </summary>
        internal string DeptCode
        {
            get;
            set;
        }

        /// <summary>
        /// 所有用户
        /// </summary>
        internal List<View_HR_Personnel> AllUser
        {
            get { return m_lstAllUser; }
            set { m_lstAllUser = value; }
        }

        /// <summary>
        /// 获取或设置选择的用户
        /// </summary>
        internal List<View_HR_Personnel> SelectedUser
        {
            get { return m_selectedUser; }
            set { m_selectedUser = value; }
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        public FormSelectPersonnel()
        {
            InitializeComponent();
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
                if (AllUser == null)
                {
                    if (DeptCode == null)
                        AllUser = m_personnelInfo.GetAllInfo().ToList();
                    else
                        AllUser = m_personnelInfo.GetAllInfo(DeptCode).ToList();
                }

                DataGridViewCheckBoxColumn column = new DataGridViewCheckBoxColumn();

                column.Visible = true;
                column.Name = "选中";
                column.HeaderText = "选中";
                column.ReadOnly = false;

                dataGridView.Columns.Add(column);

                dataGridView.Columns.Add("工号", "工号");
                dataGridView.Columns.Add("姓名", "姓名");
                dataGridView.Columns.Add("部门名称", "部门名称");
                dataGridView.Columns.Add("职位", "职位");
                dataGridView.Columns.Add("备注", "备注");

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

                dataGridView.Rows.Clear();
                foreach (var item in AllUser)
                {
                    selectedFlag = false;

                    if (SelectedUser != null && count < SelectedUser.Count)
                    {
                        if (SelectedUser.FindIndex(c => c.工号 == item.工号) >= 0)
                        {
                            selectedFlag = true;
                            count++;
                        }
                    }

                    dataGridView.Rows.Add(new object[] { selectedFlag, item.工号, item.姓名, item.部门名称, item.职位, item.备注 });
                }

                if (m_dataLocalizer == null)
                {
                    m_dataLocalizer = new UserControlDataLocalizer(dataGridView, this.Name, 
                        UniversalFunction.SelectHideFields(this.Name, dataGridView.Name, BasicInfo.LoginID));

                    panelTop.Controls.Add(m_dataLocalizer);

                    m_dataLocalizer.Dock = DockStyle.Bottom;
                }
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
            m_selectedUser = new List<View_HR_Personnel>();

            foreach (DataGridViewRow item in dataGridView.Rows)
            {
                if ((bool)item.Cells[0].Value)
                {
                    View_HR_Personnel user = (from r in m_lstAllUser where r.工号 == item.Cells[1].Value.ToString() select r).Single();
                    m_selectedUser.Add(user);
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
