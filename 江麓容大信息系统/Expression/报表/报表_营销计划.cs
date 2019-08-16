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
    public partial class 报表_营销计划 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        string m_err = "";

        public 报表_营销计划(string billID, string billName)
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

        /// <summary>
        /// 插入参数
        /// </summary>
        /// <param name="strNy"></param>
        private void InsertMessage(string strNy)
        {
            int intMonth = Convert.ToInt32( strNy.Substring(4, 2));

            int intFristMonth = intMonth;

            int intSecondMonth = 0;

            int intThridMonth = 0;

            if (intFristMonth == 12)
            {
                intSecondMonth = 1;
            }
            else
            {
                intSecondMonth = intMonth + 1;
            }

            if (intSecondMonth == 12)
            {
                intThridMonth = 1;
            }
            else
            {
                intThridMonth = intSecondMonth + 1;
            }

            string strEndYear = "";

            if (intFristMonth >10)
            {
                strEndYear = (Convert.ToInt32(strNy.Substring(0, 4)) + 1).ToString();
            }
            else
            {
                strEndYear = strNy.Substring(0, 4);
            }
            string strLableNy = "（" + strNy.Substring(0, 4) + "年"
                + intFristMonth.ToString() + "月--" + strEndYear + "年"
                + intThridMonth.ToString() + "月）";

            //设置参数   
            ReportParameter FristMonth = new ReportParameter("FristMonth", ReturnChina(intFristMonth));
            ReportParameter SecondMonth = new ReportParameter("SecondMonth", ReturnChina(intSecondMonth));
            ReportParameter ThridMonth = new ReportParameter("ThridMonth", ReturnChina(intThridMonth));
            ReportParameter LableNy = new ReportParameter("LableNy", strLableNy);


            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { FristMonth });
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { SecondMonth });
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { ThridMonth });
            reportViewer1.LocalReport.SetParameters(new ReportParameter[] { LableNy });
                
        }

        private string ReturnChina(int intMonth)
        {
            if (intMonth == 1)
            {
                return "一  月";
            }
            if (intMonth == 2)
            {
                return "二  月";
            }
            if (intMonth == 3)
            {
                return "三  月";
            }
            if (intMonth == 4)
            {
                return "四  月";
            }
            if (intMonth == 5)
            {
                return "五  月";
            }
            if (intMonth == 6)
            {
                return "六  月";
            }
            if (intMonth == 7)
            {
                return "七  月";
            }
            if (intMonth == 8)
            {
                return "八  月";
            }
            if (intMonth == 9)
            {
                return "九  月";
            }
            if (intMonth == 10)
            {
                return "十  月";
            }
            if (intMonth == 11)
            {
                return "十一月";
            }
            if (intMonth == 12)
            {
                return "十二月";
            }
            return "";
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

                string sql = "select * from DepotManagement.dbo.View_S_MarketingPlanBill where 单据号='" + m_billID + "' ";
                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.depotManagementDataSet.View_S_MarketingPlanBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_MarketingPlanBill", 
                    this.depotManagementDataSet.View_S_MarketingPlanBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_S_MarketingPlanList where 单据号='" + m_billID + "' order by 序号";

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.depotManagementDataSet.View_S_MarketingPlanList);

                rds = new ReportDataSource("DepotManagementDataSet_View_S_MarketingPlanList", 
                    this.depotManagementDataSet.View_S_MarketingPlanList);

                reportViewer1.LocalReport.DataSources.Add(rds);

                InsertMessage(this.depotManagementDataSet.View_S_MarketingPlanBill.Rows[0]["单据年月"].ToString());

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

        private void 报表_营销计划_Load(object sender, EventArgs e)
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }
    }
}
