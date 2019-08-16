/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormReportCheckOutInDepotBil1.cs
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
using GlobalObject;

namespace Expression
{
    /// <summary>
    /// 报检入库报表界面
    /// </summary>
    public partial class 报表_报检入库单_含金额 : Form
    {
        string m_billID = "";

        string m_billName = "";

        public 报表_报检入库单_含金额(string billID, string billName)
        {
            InitializeComponent();
            m_billID = billID;
            m_billName = billName;
        }

        private void FormReportCheckOutInDepotBil1_Load(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from dbo.View_S_CheckOutInDepotBill where 入库单号='" + m_billID + "' and 仓管签名 is not null";
                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.DepotManagementDataSet.View_S_CheckOutInDepotBill);
            }

            this.reportViewer1.RefreshReport();

            //if (!m_printServer.AddPrintInfo(m_billID, m_billName, BasicInfo.LoginID, BasicInfo.LoginName, BasicInfo.DeptName, ServerTime.Time.ToString().Substring(0, 10), out m_err))
            //{
            //    MessageDialog.ShowPromptMessage(m_err);
            //    reportViewer1.ShowPrintButton = false;
            //}
        }
    }
}
