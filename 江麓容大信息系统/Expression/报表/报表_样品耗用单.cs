/******************************************************************************
 *
 * 文件名称:  报表_样品耗用单.cs
 * 作者    :  邱瑶       日期: 2013/12/23
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
using UniversalControlLibrary;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using GlobalObject;
using ServerModule;

namespace Expression
{
    public partial class 报表_样品耗用单 : Form, IBillReportInfo
    {
        /// <summary>
        /// 单据号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 单据名称
        /// </summary>
        string m_billName = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err = "";

        public 报表_样品耗用单(string billID,string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            InitData();
        }

        private void 报表_样品耗用单_Load(object sender, EventArgs e)
        {
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

                string sql = "select * from DepotManagement.dbo.View_S_MusterUseBill where " +
                             " 单据号='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_S_MusterUseBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet1_View_S_MusterUseBill",
                    this.DepotManagementDataSet1.View_S_MusterUseBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_S_MusterUseList where 单据号='" + m_billID + "'";

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_S_MusterUseList);

                rds = new ReportDataSource("DepotManagementDataSet1_View_S_MusterUseList",
                    this.DepotManagementDataSet1.View_S_MusterUseList);

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
