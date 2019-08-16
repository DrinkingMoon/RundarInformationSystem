/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  报表_合同粗概.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/10/25
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

namespace Expression
{
    /// <summary>
    /// 报废单据报表界面
    /// </summary>
    public partial class 报表_合同粗概 : Form
    {
        public 报表_合同粗概(DateTime beginTime, DateTime endTime)
        {
            InitializeComponent();

            InitData(beginTime, endTime);
        }

        private void InitData(DateTime beginTime, DateTime endTime)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = string.Format(
                    "select * from DepotManagement.dbo.View_B_BargainInfo where 日期 >= '{0}' and 日期 <= '{1}'", beginTime, endTime);

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_B_BargainInfo);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_B_BargainInfo", 
                    this.DepotManagementDataSet.View_B_BargainInfo);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }
    }
}
