/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  报表_库存信息.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2010/10/27
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
    /// 库存报表界面
    /// </summary>
    public partial class 报表_库存信息 : Form
    {
        public 报表_库存信息()
        {
            InitializeComponent();

            InitData();
        }

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_S_Stock order by 材料类别名称";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_Stock);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_Stock", this.DepotManagementDataSet.View_S_Stock);

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

        private void 报表_库存信息_Load(object sender, EventArgs e)
        {
            // TODO: 这行代码将数据加载到表“DepotManagementDataSet.View_S_Stock”中。您可以根据需要移动或移除它。
            //this.View_S_StockTableAdapter.Fill(this.DepotManagementDataSet.View_S_Stock);

        }
    }
}
