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
    public partial class 报表_营销业务三包外总单 : Form, IBillReportInfo
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 单据类型
        /// </summary>
        string m_billName = "";

        public 报表_营销业务三包外总单(string billID, string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            InitData();
        }

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select TOP (1)* from DepotManagement.dbo.View_S_MarketingAll where DJH = '" + m_billID + "'";
                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_MarketingAll);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_MarketingAll",
                    this.DepotManagementDataSet.View_S_MarketingAll);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }

        private void 报表_营销业务三包外总单_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“DepotManagementDataSet1.View_S_MarketingAll”中。您可以根据需要移动或移除它。

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
