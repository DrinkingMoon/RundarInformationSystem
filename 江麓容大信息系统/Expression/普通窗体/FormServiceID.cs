using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 函电信息查询界面
    /// </summary>
    public partial class FormServiceID : Form
    {
        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 服务类
        /// </summary>
        IServiceFeedBack m_serverFeedBack = WebServerModule.ServerModuleFactory.GetServerModule<IServiceFeedBack>();

        /// <summary>
        /// 函电单据号
        /// </summary>
        private string m_strServiceID;

        public string StrServiceID
        {
            get { return m_strServiceID; }
            set { m_strServiceID = value; }
        }

        public FormServiceID()
        {
            InitializeComponent();

            DataTable dt = m_serverFeedBack.GetAfterService("","");
            dataGridView1.DataSource = dt;

            DataBindGirdView(dt);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            m_strServiceID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() ;

            this.Close();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void DataBindGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

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

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(
                    dataGridView1, this.Name, ServerModule.UniversalFunction.SelectHideFields(
                                                           this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }
    }
}
