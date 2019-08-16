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
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 报表_销售清单 : Form, IBillReportInfo
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
        string m_err = "";

        /// <summary>
        /// 客户名称
        /// </summary>
        string m_client;

        public 报表_销售清单(string billID,string clientName)
        {
            InitializeComponent();

            m_billID = billID;
            m_client = clientName;

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

                    string sql = "select * from DepotManagement.dbo.View_S_MarketingPartBill where 单据号 in ('" + m_billID + "')";

                    SqlCommand command = new SqlCommand(sql);
                    command.Connection = conn;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet1.View_S_MarketingPartBill);

                    ReportDataSource rds = new ReportDataSource("DepotManagementDataSet1_View_S_MarketingPartBill",
                        this.DepotManagementDataSet1.View_S_MarketingPartBill);

                    reportViewer1.LocalReport.DataSources.Clear();
                    reportViewer1.LocalReport.DataSources.Add(rds);

                    sql = "select SUM(数量) as 数量,GoodsID,主机厂物品名称,主机厂代码,销售单价,单位,max(单据号) 单据号," +
                          " max(批次号) as 批次号,max(供应商) 供应商,max(序号) as 序号 from " +
                          " DepotManagement.dbo.View_S_MarketintPartList where 单据号 in " +
                          " ('" + m_billID + "') group by GoodsID,销售单价,主机厂代码,主机厂物品名称,单位" +
                          " order by 主机厂物品名称";

                    command = new SqlCommand(sql);
                    command.Connection = conn;

                    adapter = new SqlDataAdapter(command);
                    adapter.Fill(this.DepotManagementDataSet1.View_S_MarketintPartList);

                    rds = new ReportDataSource("DepotManagementDataSet1_View_S_MarketintPartList",
                        this.DepotManagementDataSet1.View_S_MarketintPartList);

                    reportViewer1.LocalReport.DataSources.Add(rds);
                }

                // 加载参数
                ReportParameter rp = new ReportParameter("Report_Parameter_Client", m_client);

                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                this.reportViewer1.RefreshReport();
            }
            catch (Exception)
            {
                SqlConnection conn = new SqlConnection();

                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select SUM(数量) as 数量,GoodsID,主机厂物品名称,主机厂代码,销售单价,单位,max(单据号) 单据号," +
                          " max(批次号) as 批次号,max(供应商) 供应商,max(序号) as 序号 from " +
                          " DepotManagement.dbo.View_S_MarketintPartList where 单据号 in " +
                          " ('" + m_billID + "') group by GoodsID,销售单价,主机厂代码,主机厂物品名称,单位" +
                          " order by 主机厂物品名称";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet1.View_S_MarketintPartList);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet1_View_S_MarketintPartList",
                        this.DepotManagementDataSet1.View_S_MarketintPartList);
                reportViewer1.LocalReport.DataSources.Add(rds);

                // 加载参数
                ReportParameter rp = new ReportParameter("Report_Parameter_Client", m_client);

                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
                this.reportViewer1.RefreshReport();
            }
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
