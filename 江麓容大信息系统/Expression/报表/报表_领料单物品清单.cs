/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormReportProductFactDetailFetchBill.cs
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
using Microsoft.Reporting.WinForms;
using GlobalObject;

namespace Expression
{
    /// <summary>
    /// 实际详细领料报表界面
    /// </summary>
    public partial class 报表_领料单物品清单 : Form
    {
        /// <summary>
        /// 领料单号
        /// </summary>
        string m_billID = "";

        /// <summary>
        /// 报表标题
        /// </summary>
        string m_reportTitle;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="billID">单据号</param>
        /// <param name="title">报表头</param>
        public 报表_领料单物品清单(string billID, string title)
        {
            InitializeComponent();

            m_billID = billID;
            m_reportTitle = title;

            InitData();
        }

        /// <summary>
        /// 返回本地报表对象
        /// </summary>
        /// <returns></returns>
        public LocalReport GetLocalReport()
        {
            return this.reportViewer1.LocalReport;
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

                // 在库存表中同一样物品入了几个不同的仓库时此处会报错
                string sql = "select * from DepotManagement.dbo.View_S_MaterialRequisitionGoods "+
                             " where 领料单号='" + m_billID + "' order by 显示位置";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_MaterialRequisitionGoods);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_MaterialRequisitionGoods", 
                                                            this.DepotManagementDataSet.View_S_MaterialRequisitionGoods);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_S_MaterialRequisition where 领料单号='" + m_billID + "'";
                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_MaterialRequisition);

                rds = new ReportDataSource("DepotManagementDataSet_View_S_MaterialRequisition", 
                                            this.DepotManagementDataSet.View_S_MaterialRequisition);

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
        }

        private void FormReportProductFactDetailFetchBill_Load(object sender, EventArgs e)
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }

        private void reportViewer1_Print(object sender, CancelEventArgs e)
        {
            //string error = null;
            //if (VirtualPrint.IsVirtualPrint(out error))
            //{
            //    e.Cancel = true;
            //    MessageDialog.ShowPromptMessage(error);
            //}
        }
    }
}
