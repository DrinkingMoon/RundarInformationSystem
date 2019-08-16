using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using Expression;
using UniversalControlLibrary;
using PlatformManagement;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 获取人员简历界面
    /// </summary>
    public partial class FormPersonnelResume : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 储备人才管理类
        /// </summary>
        IResumeServer m_resumeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IResumeServer>();

        /// <summary>
        /// 数据源
        /// </summary>
        DataTable dt;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 数据名
        /// </summary>
        string m_dataName;

        /// <summary>
        /// 用户编码
        /// </summary>
        string m_userCode;

        /// <summary>
        /// 用户姓名
        /// </summary>
        string m_userName;

        /// <summary>
        /// 编辑框
        /// </summary>
        TextBox m_textBox;

        /// <summary>
        /// 数据字典
        /// </summary>
        Dictionary<string, object> m_dicData = new Dictionary<string, object>();

        /// <summary>
        /// 获取用户序号
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
        /// 获取数据项
        /// </summary>
        /// <param name="name">数据名称</param>
        /// <returns>返回获取到的数据值</returns>
        public Object GetDataItem(string name)
        {
            if (m_dicData.ContainsKey(name))
            {
                return m_dicData[name];
            }

            return null;
        }

         /// <summary>
        /// 构建函数
        /// </summary>
        /// <param name="textBox">将选定值写入的编辑框</param>
        /// <param name="dataName">要写入的数据名称, 登录名或姓名</param>
        public FormPersonnelResume(TextBox textBox, string dataName)
        {
            InitializeComponent();

            m_textBox = textBox;
            m_dataName = dataName;
        }

        public FormPersonnelResume()
        {
            InitializeComponent();
        }

         /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore">结果集</param>
        void RefreshDataGridView(DataTable resumeDt)
        {
            dataGridView1.DataSource = resumeDt;
            dataGridView1.Refresh();

        }

        private void FormPersonnelResume_Load(object sender, EventArgs e)
        {
            if (!m_resumeServer.GetAllInfo(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryResult.DataGridView = dataGridView1;

            dt = m_queryResult.DataCollection.Tables[0];

            if (dt.Rows.Count == 0)
            {
                return;
            }

            RefreshDataGridView(dt);

            // 显示列
            string[] showColumns = new string[] { "编号", "简历状态", "姓名", "性别", "出生日期", "毕业院校", "电话", "特长", "记录时间" };

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                dataGridView1.Columns[i].Width = 100;
                cmbFindItem.Items.Add(dataGridView1.Columns[i].Name);

                if (!showColumns.Contains(dataGridView1.Columns[i].Name))
                    dataGridView1.Columns[i].Visible = false;
            }

            cmbFindItem.SelectedIndex = 0;
        }

        private void 选定人员ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //m_gridViewCell.Value = dataGridView1.CurrentRow.Cells[m_dataName].Value.ToString();
            m_userName = dataGridView1.CurrentRow.Cells["姓名"].Value.ToString();
            m_userCode = dataGridView1.CurrentRow.Cells["编号"].Value.ToString();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                DataGridViewRow row = dataGridView1.SelectedRows[0];
                m_dicData.Clear();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    m_dicData.Add(dataGridView1.Columns[i].Name, row.Cells[i].Value);
                }

                this.DialogResult = DialogResult.OK;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            选定人员ToolStripMenuItem.PerformClick();
        }

        private void txtContext_TextChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = FuzzyFindDataTableRecord.FindRecord(dt, cmbFindItem.Text, txtContext.Text);
        }
    }
}
