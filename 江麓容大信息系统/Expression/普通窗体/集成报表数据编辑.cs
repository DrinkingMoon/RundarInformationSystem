using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 集成报表数据编辑 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 是否保存状态
        /// </summary>
        private bool m_blIsSave = false;

        public bool BlIsSave
        {
            get { return m_blIsSave; }
            set { m_blIsSave = value; }
        }

        /// <summary>
        /// 报表服务组件
        /// </summary>
        IReport m_serverReport = ServerModuleFactory.GetServerModule<IReport>();

        /// <summary>
        /// 明细表
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// LINQ数据集
        /// </summary>
        private BASE_IntegrationReport m_lnqReport = new BASE_IntegrationReport();

        public BASE_IntegrationReport LnqReport
        {
            get { return m_lnqReport; }
            set { m_lnqReport = value; }
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="reportCode">报表编号</param>
        /// <param name="flag">状态 0 ：添加 1：编辑</param>
        public 集成报表数据编辑(string reportCode,int flag)
        {
            InitializeComponent();

            if (flag == 0)
            {
                m_lnqReport.ID = 0;
                m_lnqReport.ReportCode = m_serverReport.GetNewReportCode(reportCode);
            }
            else if (flag == 1)
            {
                m_lnqReport = m_serverReport.GetReportInfo(reportCode);
            }

            cmbFindType.DataSource = GlobalObject.GeneralFunction.GetEumnList(typeof(Report_FindType));

            ShowInfo();

        }

        /// <summary>
        /// 显示数据
        /// </summary>
        void ShowInfo()
        {
            txtProcedureName.Text = m_lnqReport.ProcedureName;
            txtPrintName.Text = m_lnqReport.PrintName;
            txtReportCode.Text = m_lnqReport.ReportCode;
            txtReportName.Text = m_lnqReport.ReportName;
            cmbFindType.Text = m_lnqReport.FindType;

            dataGridView1.DataSource = m_serverReport.GetReportInfoList(m_lnqReport.ReportCode);
        }

        /// <summary>
        /// 获得数据
        /// </summary>
        void GetInfo()
        {
            m_lnqReport.PrintName = txtPrintName.Text;
            m_lnqReport.ReportName = txtReportName.Text;
            m_lnqReport.ReportCode = txtReportCode.Text;
            m_lnqReport.ProcedureName = txtProcedureName.Text;
            m_lnqReport.FindType = cmbFindType.Text;
            
            m_dtList = (DataTable)dataGridView1.DataSource;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtReportCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("报表编号不能为空");
                return ;
            }

            if (txtReportName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("报表名称不能为空");
                return ;
            }

            GetInfo();

            if (!m_serverReport.SaveReportInfo(m_lnqReport, m_dtList, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功！");
                m_blIsSave = true;
                this.Close();
            }
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            txtFieldName.Text = "";
            txtFieldFormat.Text = "";
            txtParameterName.Text = "";
            cmbParameterType.SelectedIndex = -1;
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns>通过返回True，不通过返回False</returns>
        bool CheckData()
        {
            if (txtFieldName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("查询字段不能为空");
                return false;
            }

            if (txtParameterName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("参数名不能为空");
                return false;
            }

            if (cmbParameterType.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("参数类型不能为空");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                if (dtTemp.Rows[i]["查询字段"].ToString() == txtFieldName.Text 
                    || dtTemp.Rows[i]["参数名"].ToString() == txtParameterName.Text)
                {
                    MessageDialog.ShowPromptMessage("查询字段或参数名不能重复");
                    return;
                }
            }

            DataRow dr = dtTemp.NewRow();

            dr["查询字段"] = txtFieldName.Text;
            dr["参数名"] = txtParameterName.Text;
            dr["参数类型"] = cmbParameterType.Text;
            dr["查询字段格式"] = txtFieldFormat.Text;

            dtTemp.Rows.Add(dr);

            dataGridView1.DataSource = dtTemp;

            ClearData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)dataGridView1.DataSource;

            dtTemp.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            dataGridView1.DataSource = dtTemp;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckData())
            {
                return;
            }

            int intIndex = dataGridView1.CurrentRow.Index;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (i != dataGridView1.CurrentRow.Index
                    && dataGridView1.Rows[i].Cells["查询字段"].Value.ToString() == txtFieldName.Text
                    && dataGridView1.Rows[i].Cells["参数名"].Value.ToString() == txtParameterName.Text)
                {
                    MessageDialog.ShowPromptMessage("查询字段或参数名不能重复");
                    return;
                }
            }

            dataGridView1.Rows[intIndex].Cells["查询字段"].Value = txtFieldName.Text;
            dataGridView1.Rows[intIndex].Cells["参数名"].Value = txtParameterName.Text;
            dataGridView1.Rows[intIndex].Cells["参数类型"].Value = cmbParameterType.Text;
            dataGridView1.Rows[intIndex].Cells["查询字段格式"].Value = txtFieldFormat.Text;

            ClearData();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtFieldName.Text = dataGridView1.CurrentRow.Cells["查询字段"].Value.ToString();
                txtParameterName.Text = dataGridView1.CurrentRow.Cells["参数名"].Value.ToString();
                cmbParameterType.Text = dataGridView1.CurrentRow.Cells["参数类型"].Value.ToString();
                txtFieldFormat.Text = dataGridView1.CurrentRow.Cells["查询字段格式"].Value.ToString();
            }
        }

        private void txtReportCode_TextChanged(object sender, EventArgs e)
        {
            if (txtReportCode.Text.Trim().Length < m_lnqReport.ReportCode.Length)
            {
                txtReportCode.Text = m_lnqReport.ReportCode;
            }
        }
    }
}
