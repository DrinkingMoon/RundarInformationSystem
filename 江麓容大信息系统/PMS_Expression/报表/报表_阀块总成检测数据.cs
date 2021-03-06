﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Expression;
using Microsoft.Reporting.WinForms;
using PlatformManagement;

namespace Expression
{
    public partial class 报表_阀块总成检测数据 : Form
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="id">数据编号</param>
        public 报表_阀块总成检测数据(AuthorityFlag authFlag, Int64 id)
        {
            InitializeComponent();

            if ((authFlag & AuthorityFlag.Print) != AuthorityFlag.Print)
            {
                reportViewer1.ShowPrintButton = false;
                reportViewer1.ShowExportButton = false;
                reportViewer1.ShowToolBar = false;
            }

            InitData(id);
        }

        private void 报表_阀块总成检测数据_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void InitData(Int64 id)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = string.Format(
                    "select * from DepotManagement.dbo.View_ZPX_ValveTestDataItems where 序号 = {0}", id);

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();

                adapter.Fill(dt);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_ZPX_ValveTestDataItems", dt);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }
    }
}
