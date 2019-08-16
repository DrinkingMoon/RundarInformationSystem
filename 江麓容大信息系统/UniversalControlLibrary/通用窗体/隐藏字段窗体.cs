using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 隐藏字段窗体
    /// </summary>
    public partial class 隐藏字段窗体 : Form
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IComprehensiveOperation m_Comprehensive = BasicServerFactory.GetServerModule<IComprehensiveOperation>();

        /// <summary>
        /// 传出数据集
        /// </summary>
        private DataTable m_dtReturn = new DataTable();

        public DataTable DtReturn
        {
            get { return m_dtReturn; }
            set { m_dtReturn = value; }
        }

        /// <summary>
        /// 窗体名称
        /// </summary>
        string m_formName = "";

        /// <summary>
        /// datagridview名称
        /// </summary>
        string m_dataGridViewName = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 隐藏字段窗体()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="formName">窗体名称</param>
        /// <param name="dataGridViewName">数据显示控件名称</param>
        /// <param name="fields">要</param>
        public 隐藏字段窗体(string formName, string dataGridViewName, string[] fields)
        {
            InitializeComponent();

            m_dtReturn.Columns.Add("字段名");
            m_dataGridViewName = dataGridViewName;
            m_formName = formName;

            RefreshData(fields);
        }

        void RefreshData(string[] fields)
        {
            DataTable dt = new DataTable();
            DataTable dtHide = UniversalFunction.SelectHideFields(m_formName, m_dataGridViewName, BasicInfo.LoginID);

            dt.Columns.Add("选");
            dt.Columns.Add("FieldName");

            for (int i = 0; i < fields.Length; i++)
            {
                if (fields[i].ToString().Contains("单价") 
                    || fields[i].ToString().Contains("金额") 
                    || fields[i].ToString().Contains("委外费"))
                {
                    continue;
                }

                DataRow dr = dt.NewRow();

                dr["选"] = true;

                for (int k = 0; k < dtHide.Rows.Count; k++)
                {
                    if (dtHide.Rows[k]["FieldName"].ToString() == fields[i].ToString())
                    {
                        dr["选"] = false;
                    }
                }

                dr["FieldName"] = fields[i].ToString();

                dt.Rows.Add(dr);
            }

            dataGridView1.DataSource = dt;
        }

        private void 隐藏字段窗体_FormClosing(object sender, FormClosingEventArgs e)
        {
            DataTable dtGrid = (DataTable)dataGridView1.DataSource;

            m_Comprehensive.OperationHideFields(m_formName, m_dataGridViewName,
                BasicInfo.LoginID, dtGrid, out m_err);

            for (int i = 0; i < dtGrid.Rows.Count; i++)
            {
                if (dtGrid.Rows[i]["选"].ToString() != "True")
                {
                    DataRow dr = m_dtReturn.NewRow();

                    dr["字段名"] = dtGrid.Rows[i]["FieldName"].ToString();

                    m_dtReturn.Rows.Add(dr);
                }
            }
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell.ColumnIndex == 0)
            {
                if (Convert.ToBoolean(dataGridView1.CurrentRow.Cells["选"].Value))
                {
                    dataGridView1.CurrentRow.Cells["选"].Value = false;
                }
                else
                {
                    dataGridView1.CurrentRow.Cells["选"].Value = true;
                }
            }
        }

        private void 全部选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = true;
            }
        }

        private void 全部取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = false;
            }
        }
    }
}
