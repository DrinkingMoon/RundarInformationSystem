using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ServerModule;
using Microsoft.Reporting.WinForms;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 报表_CVT出厂检验记录 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        public 报表_CVT出厂检验记录(string billID, string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            InitData();
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

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_Report_DeliveryInspection where 单据号='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_Report_DeliveryInspection);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_Report_DeliveryInspection", 
                    this.DepotManagementDataSet.View_Report_DeliveryInspection);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

            }

            this.reportViewer1.RefreshReport();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }
    }
}
