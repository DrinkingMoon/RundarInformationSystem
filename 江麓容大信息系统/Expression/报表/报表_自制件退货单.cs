using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using GlobalObject;
using ServerModule;
using Microsoft.Reporting.WinForms;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 报表_自制件退货单 : Form,IBillReportInfo
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 单据名称
        /// </summary>
        string m_billName = "";

        public 报表_自制件退货单(string billID, string billName)
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

                string sql = "select * from DepotManagement.dbo.View_S_HomemadeRejectBill where 退货单号='" + m_billID + "'";
                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_HomemadeRejectBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_HomemadeRejectBill", 
                    this.DepotManagementDataSet.View_S_HomemadeRejectBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_S_HomemadeRejectList where 退货单号='" + m_billID + "'";
                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_HomemadeRejectList);

                rds = new ReportDataSource("DepotManagementDataSet_View_S_HomemadeRejectList", this.DepotManagementDataSet.View_S_HomemadeRejectList);

                reportViewer1.LocalReport.DataSources.Add(rds);
            }


            string tempCompany = "";

            switch (GlobalParameter.SystemName)
            {
                case CE_SystemName.泸州容大:
                    tempCompany = "泸州容大智能变速器有限公司";
                    break;
                case CE_SystemName.湖南容大:
                    tempCompany = "湖南容大智能变速器股份有限公司";
                    break;
                default:
                    break;
            }

            ReportParameter rp1 = new ReportParameter("SystemName", tempCompany);
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp1 });
            this.reportViewer1.RefreshReport();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }

        private void 报表_自制件退货单_Load(object sender, EventArgs e)
        {

        }
    }
}
