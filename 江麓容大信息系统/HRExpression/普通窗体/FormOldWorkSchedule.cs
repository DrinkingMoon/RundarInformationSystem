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
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 引用旧排班信息界面
    /// </summary>
    public partial class FormOldWorkSchedule : Form
    {
        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 单号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 获取或设置单号
        /// </summary>
        public string BillNo
        {
            get { return m_billNo; }
            set { m_billNo = value; }
        }

        /// <summary>
        /// 选定状态
        /// </summary>
        bool flag = false;

        /// <summary>
        /// 获取或设置选定状态
        /// </summary>
        public bool Flag
        {
            get { return flag; }
            set { flag = value; }
        }

        /// 排班信息操作类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        public FormOldWorkSchedule()
        {
            InitializeComponent();
            RefreshDataGridView();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_workSchedulingServer.GetWorkScheduling();

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["员工编号"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
               this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }


        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            m_billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            flag = true;

            this.Close();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void FormOldWorkSchedule_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 选定ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            flag = true;

            this.Close();
        }
    }
}
