/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormReportCheckOutInDepotBil.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
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

namespace Expression
{
    /// <summary>
    /// 报检入库报表界面
    /// </summary>
    public partial class 报表_报检入库单 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        string m_err = "";

        /// <summary>
        /// 报表标题
        /// </summary>
        string m_reportTitle;

        public 报表_报检入库单(string billID, string billName, string title)
        {
            InitializeComponent();
            m_billID = billID;
            m_billName = billName;
            m_reportTitle = title;

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
                string sql = "select * from dbo.View_S_CheckOutInDepotBill where 入库单号='" + m_billID + "'";
                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_CheckOutInDepotBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_CheckOutInDepotBill",
                    this.DepotManagementDataSet.View_S_CheckOutInDepotBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            // 加载参数
            ReportParameter rp = new ReportParameter("Report_Parameter_Title", m_reportTitle);
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
            this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp, rp1 });
            this.reportViewer1.RefreshReport();

            //if (BasicInfo.ListRoles.Contains(RoleEnum.会计.ToString()))
            //{
            //    reportViewer1.ShowPrintButton = false;
            //    return;
            //}
        }

        /// <summary>
        /// 用户打印报表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
    }
}
