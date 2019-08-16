/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormReportFetchGoodsBill.cs
 * 作者    :  曹津彬    版本: v1.00    日期: 2014/3/21
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
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
using ServerModule;
using Microsoft.Reporting.WinForms;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 报表_外部调运单 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        string m_err = "";

        public 报表_外部调运单(string billID, string billName)
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
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_Out_ManeuverList where " +
                             " Bill_ID='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_Out_ManeuverList);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_Out_ManeuverList",
                    this.DepotManagementDataSet.View_Out_ManeuverList);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_Out_ManeuverBill where 单据号='" + m_billID + "'";

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_Out_ManeuverBill);

                rds = new ReportDataSource("DepotManagementDataSet_View_Out_ManeuverBill",
                    this.DepotManagementDataSet.View_Out_ManeuverBill);

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

        private void 报表_外部调运单_Load(object sender, EventArgs e)
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }
    }
}
