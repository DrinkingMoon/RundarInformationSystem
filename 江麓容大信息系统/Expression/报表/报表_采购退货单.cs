/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  报表_采购退货单.cs
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
    /// 采购退货单据报表界面
    /// </summary>
    public partial class 报表_采购退货单 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        public 报表_采购退货单(string billID, string billName)
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

                string sql = "select * from DepotManagement.dbo.View_S_MaterialRejectBill where 退货单号='" + m_billID + "'";
                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_MaterialRejectBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_MaterialRejectBill", 
                    this.DepotManagementDataSet.View_S_MaterialRejectBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from DepotManagement.dbo.View_S_MaterialListRejectBill where 退货单号='" + m_billID + "'";
                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_MaterialListRejectBill);

                rds = new ReportDataSource("DepotManagementDataSet_View_S_MaterialListRejectBill", 
                    this.DepotManagementDataSet.View_S_MaterialListRejectBill);

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
    }
}
