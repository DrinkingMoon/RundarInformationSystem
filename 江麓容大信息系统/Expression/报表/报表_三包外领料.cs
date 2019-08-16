using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;

namespace Expression
{
    public partial class 报表_三包外领料 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        //string m_err = "";

        public 报表_三包外领料(string billID, string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            InitData();
        }

        private void 报表_三包外领料_Load(object sender, EventArgs e)
        {

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

                //string sql = "select * from DepotManagement.dbo.View_S_MaterialRequisitionGoods where 领料单号='" + m_billID + "' order by 显示位置";
                //SqlCommand command = new SqlCommand(sql);
                //command.Connection = conn;
                //SqlDataAdapter adapter = new SqlDataAdapter(command);
                //adapter.Fill(this.DepotManagementDataSet.View_S_MaterialRequisitionGoods);

                //ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_MaterialRequisitionGoods", this.DepotManagementDataSet.View_S_MaterialRequisitionGoods);
                reportViewer1.LocalReport.DataSources.Clear();
                //reportViewer1.LocalReport.DataSources.Add(rds);

                string sql = "select * from DepotManagement.dbo.View_S_MaterialRequisition where 领料单号='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_MaterialRequisition);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_MaterialRequisition",
                    this.DepotManagementDataSet.View_S_MaterialRequisition);

                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
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
