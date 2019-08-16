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
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    public partial class 报表_出差申请单 : Form, IBillReportInfo
    {

        string m_billID = "";

        string m_billName = "";

        /// <summary>
        /// 单据编号
        /// </summary>
        int m_billNo;

        public 报表_出差申请单(int billNo)
        {
            InitializeComponent();
            m_billNo = billNo;
            InitData();
        }

        private void InitData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                    conn.Open();

                    string sql = "select * from DepotManagement.dbo.View_HR_OnBusinessBill where 编号='" + m_billNo + "'";

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = conn;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet.View_HR_OnBusinessBill);

                    ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_HR_OnBusinessBill",
                        this.DepotManagementDataSet.View_HR_OnBusinessBill);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    sql = "select * from DepotManagement.dbo.HR_OnBusinessSchedule where BillID='" + m_billNo + "'";
                    command = new SqlCommand(sql);
                    command.Connection = conn;

                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet.HR_OnBusinessSchedule);

                    rds = new ReportDataSource("DepotManagementDataSet_HR_OnBusinessSchedule", this.DepotManagementDataSet.HR_OnBusinessSchedule);
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    sql = "select * from DepotManagement.dbo.View_HR_OnBusinessPersonnel where 单据号='" + m_billNo + "'";
                    command = new SqlCommand(sql);
                    command.Connection = conn;

                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet.View_HR_OnBusinessPersonnel);

                    rds = new ReportDataSource("DepotManagementDataSet_View_HR_OnBusinessPersonnel", this.DepotManagementDataSet.View_HR_OnBusinessPersonnel);
                    reportViewer1.LocalReport.DataSources.Add(rds);
                }

                this.reportViewer1.RefreshReport();
            }
            catch (Exception)
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                    conn.Open();

                    string sql = "select * from DepotManagement.dbo.View_HR_OnBusinessBill where 编号='" + m_billNo + "'";

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = conn;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet.View_HR_OnBusinessBill);

                    ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_HR_OnBusinessBill",
                        this.DepotManagementDataSet.View_HR_OnBusinessBill);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    sql = "select * from DepotManagement.dbo.HR_OnBusinessSchedule where BillID='" + m_billNo + "'";
                    command = new SqlCommand(sql);
                    command.Connection = conn;

                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet.HR_OnBusinessSchedule);

                    rds = new ReportDataSource("DepotManagementDataSet_HR_OnBusinessSchedule", this.DepotManagementDataSet.HR_OnBusinessSchedule);
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    sql = "select * from DepotManagement.dbo.View_HR_OnBusinessPersonnel where 单据号='" + m_billNo + "'";
                    command = new SqlCommand(sql);
                    command.Connection = conn;

                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet.View_HR_OnBusinessPersonnel);

                    rds = new ReportDataSource("DepotManagementDataSet_View_HR_OnBusinessPersonnel", this.DepotManagementDataSet.View_HR_OnBusinessPersonnel);
                    reportViewer1.LocalReport.DataSources.Add(rds);
                }

                this.reportViewer1.RefreshReport();
            }
        }

        private void 报表_出差申请单_Load(object sender, EventArgs e)
        {

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
