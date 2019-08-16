using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using System.Data.SqlClient;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 报表_人员档案 : Form, IBillReportInfo
    {
        /// <summary>
        /// 员工编号
        /// </summary>
        string workID;

        string m_billID = "";

        string m_billName = "";

        public 报表_人员档案(string loginID)
        {
            InitializeComponent();

            workID = loginID;

            InitData();
        }

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_HR_PersonnelArchive where 员工编号='" + workID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_HR_PersonnelArchive);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_HR_PersonnelArchive",
                    this.DepotManagementDataSet.View_HR_PersonnelArchive);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.HR_PersonnelArchiveList where WorkID='" + workID + "'";
                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.HR_PersonnelArchiveList);

                rds = new ReportDataSource("DepotManagementDataSet_HR_PersonnelArchiveList", this.DepotManagementDataSet.HR_PersonnelArchiveList);
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }

        private void 报表_人员档案_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“DepotManagementDataSet.View_HR_PersonnelArchive”中。您可以根据需要移动或移除它。

        }

        #region IBillReportInfo 成员

        /// <summary>
        /// 获取单据类型, 如：领料单等
        /// </summary>
        public string BillType
        {
            get
            {
                return m_billName;
            }
        }

        /// <summary>
        /// 获取单据编号
        /// </summary>
        public string BillNo
        {
            get
            {
                return m_billID;
            }
        }

        /// <summary>
        /// 获取本地报表对象
        /// </summary>
        public LocalReport LocalReportObject
        {
            get
            {
                return reportViewer1.LocalReport;
            }
        }

        #endregion
    }
}
