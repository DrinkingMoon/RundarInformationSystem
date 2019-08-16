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
    public partial class 报表_盘点单 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        string m_err = "";

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
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_S_ReportForStorageCheck where DJH = '" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_ReportForStorageCheck);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_ReportForStorageCheck", 
                                                            this.DepotManagementDataSet.View_S_ReportForStorageCheck);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                //sql = "select * from DepotManagement.dbo.View_S_ReportForStorageCheck where DJH ='" + m_billID + "'";
                //command = new SqlCommand(sql);
                //command.Connection = conn;
                //adapter = new SqlDataAdapter(command);
                //adapter.Fill(this.DepotManagementDataSet.View_S_ReportForStorageCheck);

                //rds = new ReportDataSource("depotManagementDataSet_View_S_ReportForStorageCheck", this.DepotManagementDataSet.View_S_ReportForStorageCheck);
                //reportViewer1.LocalReport.DataSources.Add(rds);
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
            else
            {
                IPrintManagement printManagement = BasicServerFactory.GetServerModule<IPrintManagement>();

                S_PrintBillTable printInfo = new S_PrintBillTable();

                printInfo.Bill_ID = m_billID;
                printInfo.Bill_Name = m_billName;
                printInfo.PrintDateTime = ServerModule.ServerTime.Time;
                printInfo.PrintFlag = true;
                printInfo.PrintPersonnelCode = BasicInfo.LoginID;
                printInfo.PrintPersonnelName = BasicInfo.LoginName;
                printInfo.PrintPersonnelDepartment = BasicInfo.DeptName;

                if (printManagement.IsExist(printInfo, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else if (!printManagement.AddPrintInfo(printInfo, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }

                reportViewer1.ShowPrintButton = false;
            }
        }

        public 报表_盘点单(string billID, string billName)
        {
            InitializeComponent();
            m_billID = billID;
            m_billName = billName;

            InitData();
        }

        private void 报表_盘点单_Load(object sender, EventArgs e)
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }
    }
}
