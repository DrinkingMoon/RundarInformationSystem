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

namespace Expressio
{
    public partial class 报表_样品耗用清单 : Form
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 单据名称
        /// </summary>
        string m_billName = "";

        public 报表_样品耗用清单(string billID, string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            InitData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_S_MusterUseBill where " +
                             " 单据号='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_S_MusterUseBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet1_View_S_MusterUseBill",
                    this.DepotManagementDataSet1.View_S_MusterUseBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_S_MusterUseList where 单据号='" + m_billID + "'";

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_S_MusterUseList);

                rds = new ReportDataSource("DepotManagementDataSet1_View_S_MusterUseList",
                    this.DepotManagementDataSet1.View_S_MusterUseList);

                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }

        private void 报表_样品耗用清单_Load(object sender, EventArgs e)
        {

        }
    }
}
