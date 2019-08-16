/******************************************************************************
 *
 * 文件名称:  报表_零星采购单.cs
 * 作者    :  邱瑶       日期: 2013/12/11
 * 开发平台:  vs2008(c#)
 * 用于    :  生产线管理信息系统
 ******************************************************************************/

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

namespace Expression
{
    public partial class 报表_零星采购单 : Form, IBillReportInfo
    {
        /// <summary>
        /// 单据名
        /// </summary>
        string m_billName = "";

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public 报表_零星采购单(string billNo)
        {
            InitializeComponent();

            m_billID = billNo;

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

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                    conn.Open();

                    string sql = "select * from DepotManagement.dbo.View_B_MinorPurchaseBill where 单据号 =" + m_billID;

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = conn;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet1.View_B_MinorPurchaseBill);

                    ReportDataSource rds = new ReportDataSource("DepotManagementDataSet1_View_B_MinorPurchaseBill",
                        this.DepotManagementDataSet1.View_B_MinorPurchaseBill);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    sql = "select * from DepotManagement.dbo.View_B_MinorPurchaseList where 编号 = " + m_billID;

                    command = new SqlCommand(sql);
                    command.Connection = conn;

                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet1.View_B_MinorPurchaseList);

                    rds = new ReportDataSource("DepotManagementDataSet1_View_B_MinorPurchaseList",
                        this.DepotManagementDataSet1.View_B_MinorPurchaseList);

                    reportViewer1.LocalReport.DataSources.Add(rds);
                }
            }
            catch (Exception)
            {
                SqlConnection conn = new SqlConnection();

                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_B_MinorPurchaseBill where 单据号 =" + m_billID;

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_B_MinorPurchaseBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet1_View_B_MinorPurchaseBill",
                    this.DepotManagementDataSet1.View_B_MinorPurchaseBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_B_MinorPurchaseList where 编号 = " + m_billID;

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_B_MinorPurchaseList);

                rds = new ReportDataSource("DepotManagementDataSet1_View_B_MinorPurchaseList",
                    this.DepotManagementDataSet1.View_S_MarketintPartList);

                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }

        private void reportViewer1_Print(object sender, CancelEventArgs e)
        {
            if (VirtualPrint.IsVirtualPrint(out m_err))
            {
                e.Cancel = true;
                MessageDialog.ShowPromptMessage(m_err);
            }
        }
    }
}
