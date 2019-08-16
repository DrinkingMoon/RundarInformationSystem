using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using Service_Peripheral_HR;
using Expression;
using UniversalControlLibrary;
using GlobalObject;

namespace CommonBusinessModule
{
    /// <summary>
    /// 查询职员信息用的界面
    /// </summary>
    public partial class FormPersonnel : Form
    {
        /// <summary>
        /// 查询到的职员信息
        /// </summary>
        IQueryable<View_SelectPersonnel> m_findPersonnel;

        /// <summary>
        /// 职员服务组件
        /// </summary>
        IPersonnelArchiveServer m_personnelServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 数据字典
        /// </summary>
        Dictionary<string, object> m_dicData = new Dictionary<string, object>();

        /// <summary>
        /// 编辑框
        /// </summary>
        TextBox m_textBox;

        /// <summary>
        /// 编辑框
        /// </summary>
        DataGridViewCell m_gridViewCell;

        /// <summary>
        /// 编辑框
        /// </summary>
        ComboBox m_cmBox;

        /// <summary>
        /// 数据名
        /// </summary>
        string m_dataName;

        /// <summary>
        /// 用户编码
        /// </summary>
        string m_userCode;

        /// <summary>
        /// 用户职位
        /// </summary>
        string m_userWrokPost;

        /// <summary>
        /// 部门编码或部门名称
        /// </summary>
        string m_dept;

        /// <summary>
        /// 用户姓名
        /// </summary>
        string m_userName;

        /// <summary>
        /// 获取用户编码
        /// </summary>
        public string UserCode
        {
            get { return m_userCode; }
        }

        /// <summary>
        /// 获取用户姓名
        /// </summary>
        public string UserName
        {
            get { return m_userName; }
        }

        /// <summary>
        /// 获取用户职位
        /// </summary>
        public string UserWorkPost
        {
            get { return m_userWrokPost; }
        }

        /// <summary>
        /// 获取数据项
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回获取到的数据值</returns>
        public Object this[String name]
        {
            get
            {
                if (m_dicData != null && m_dicData.Count() > 0 && m_dicData.ContainsKey(name))
                {
                    return m_dicData[name];
                }

                return null;
            }
        }

        /// <summary>
        /// 获取字符串类型数据
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回获取到的数据值</returns>
        public string GetStringDataItem(string name)
        {
            if (m_dicData.ContainsKey(name))
            {
                return (string)m_dicData[name];
            }

            return null;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="textBox">将选定值写入的编辑框</param>
        public FormPersonnel(TextBox textBox)
        {
            InitializeComponent();

            m_textBox = textBox;
            m_dataName = "员工姓名";
        }

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="textBox">将选定值写入的编辑框</param>
        /// <param name="dataName">要写入的数据名称, 登录名或姓名</param>
        public FormPersonnel(TextBox textBox, string dataName)
        {
            InitializeComponent();

            m_textBox = textBox;
            m_dataName = dataName;
        }

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="gridViewCell">需要的值</param>
        /// <param name="dataName">要写入的数据名称, 登录名或姓名</param>
        public FormPersonnel(DataGridViewCell gridViewCell, string dataName)
        {
            InitializeComponent();

            m_gridViewCell = gridViewCell;
            m_dataName = dataName;
        }

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="cmbox">将选定值写入的编辑框</param>
        /// <param name="dataName">要写入的数据名称, 登录名或姓名</param>
        public FormPersonnel(ComboBox cmbox, string dataName)
        {
            InitializeComponent();

            m_cmBox = cmbox;
            m_dataName = dataName;
        }

        /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="textBox">将选定值写入的编辑框</param>
        /// <param name="dept">部门编码或部门名称</param>
        /// <param name="dataName">要写入的数据名称, 登录名或姓名</param>
        public FormPersonnel(TextBox textBox, string dept, string dataName)
        {
            InitializeComponent();

            m_textBox = textBox;
            m_dataName = dataName;
            m_dept = dept;
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormUser_Load(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(m_dept))
            {
                m_findPersonnel = m_personnelServer.GetAllInfo();
            }
            else
            {
                m_findPersonnel = m_personnelServer.GetPersonnelViewInfo(Service_Peripheral_HR.PersonnelDefiniens.ParameterType.部门, m_dept);
            }

            if (m_findPersonnel == null || m_findPersonnel.Count() == 0)
            {
                return;
            }

            RefreshDataGridView(m_findPersonnel);

            // 显示列
            string[] showColumns = new string[] { "员工编号", "员工姓名", "部门","职位" };

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = 100;
                cmbFindItem.Items.Add(dataGridView1.Columns[i].Name);

                if (!showColumns.Contains(dataGridView1.Columns[i].Name))
                    dataGridView1.Columns[i].Visible = false;
            }

            cmbFindItem.SelectedIndex = 0;
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore">结果集</param>
        void RefreshDataGridView(IQueryable<View_SelectPersonnel> findUser)
        {
            dataGridView1.DataSource = new BindingList<View_SelectPersonnel>(findUser.ToList());
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 选定人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 选定人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            m_userName = dataGridView1.CurrentRow.Cells["员工姓名"].Value as string;
            m_userCode = dataGridView1.CurrentRow.Cells["员工编号"].Value as string;
            m_userWrokPost = dataGridView1.CurrentRow.Cells["岗位"].Value as string;

            if (m_textBox != null)
            {
                m_textBox.Text = m_userName;
                m_textBox.Tag = m_userCode;
            }

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                m_dicData.Clear();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    m_dicData.Add(dataGridView1.Columns[i].Name, row.Cells[i].Value);
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            选定人员ToolStripMenuItem.PerformClick();
        }

        private void txtContext_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = FuzzyFindDataTableRecord.FindRecord(m_findPersonnel, cmbFindItem.Text, txtContext.Text);
        }
    }
}
