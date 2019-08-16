/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormReportDeclareWastrelBil.cs
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

namespace Expression
{
    /// <summary>
    /// 单据打印明细报表界面
    /// </summary>
    public partial class 报表_单据打印表 : Form
    {
        /// <summary>
        /// 单据打印起始时间
        /// </summary>
        DateTime m_beginTime;

        /// <summary>
        /// 单据打印结束时间
        /// </summary>
        DateTime m_endTime;

        /// <summary>
        /// 部门信息
        /// </summary>
        string m_dept;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="beginDate">单据打印起始时间</param>
        /// <param name="endDate">单据打印结束时间</param>
        /// <param name="dept">部门信息</param>
        public 报表_单据打印表(DateTime beginDate, DateTime endDate, string dept)
        {
            InitializeComponent();

            m_beginTime = beginDate;
            m_endTime = endDate;
            m_dept = dept;

            InitData();
        }

        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "";

                if (GlobalObject.GeneralFunction.IsNullOrEmpty(m_dept))
                {
                    sql = "select * from DepotManagement.dbo.View_S_PrintBill where 打印时间 >='" + m_beginTime.ToString()
                        + "' and 打印时间 <= '" + m_endTime.ToString() + "' order by 分发部门, 单据类别, 单据编号";
                }
                else
                {
                    sql = "select * from DepotManagement.dbo.View_S_PrintBill where 打印时间 >='" + m_beginTime.ToString()
                        + "' and 打印时间 <= '" + m_endTime.ToString() + "' and 分发部门 = '" + 
                        m_dept + "' order by 分发部门, 单据类别, 单据编号";
                }

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_PrintBill);

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_PrintBill", 
                    this.DepotManagementDataSet.View_S_PrintBill);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }
    }
}
