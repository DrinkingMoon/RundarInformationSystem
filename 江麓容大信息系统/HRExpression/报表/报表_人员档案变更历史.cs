using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace Form_Peripheral_HR
{
    public partial class 报表_人员档案变更历史 : Form
    {
        /// <summary>
        /// 变更编号
        /// </summary>
        int changeID;

        public 报表_人员档案变更历史(int id)
        {
            InitializeComponent();

            changeID = id;
            InitData();
        }

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_HR_PersonnelArchiveChange where 编号='" + changeID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_HR_PersonnelArchiveChange);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_HR_PersonnelArchiveChange",
                    this.DepotManagementDataSet.View_HR_PersonnelArchiveChange);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }

        private void 报表_人员档案变更历史_Load(object sender, EventArgs e)
        {
            //View_HR_PersonnelArchiveChangeBindingSource
        }
    }
}
