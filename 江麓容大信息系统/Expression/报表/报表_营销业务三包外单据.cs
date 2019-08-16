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
using ServerModule;
using UniversalControlLibrary;
using GlobalObject;

namespace Expression
{
    public partial class 报表_营销业务三包外单据 : Form,IBillReportInfo
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 单据类型
        /// </summary>
        string m_billName = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error = "";

        public 报表_营销业务三包外单据(string billID, string billName)
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

                string sql = "select * from DepotManagement.dbo.View_S_MarketingAll where DJH = '" + m_billID + "'";
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

        private void 报表_营销业务三包外单据_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“DepotManagementDataSet.View_S_MarketingAll”中。您可以根据需要移动或移除它。
           

        }

        private void reportViewer1_Print(object sender, CancelEventArgs e)
        {
            if (VirtualPrint.IsVirtualPrint(out m_error))
            {
                e.Cancel = true;
                MessageDialog.ShowPromptMessage(m_error);
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

                if (printManagement.IsExist(printInfo, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }
                else if (!printManagement.AddPrintInfo(printInfo, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                }

                reportViewer1.ShowPrintButton = false;
            }
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
