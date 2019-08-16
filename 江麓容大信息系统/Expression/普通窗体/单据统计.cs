using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 单据统计界面
    /// </summary>
    public partial class 单据统计 : Form
    {
        /// <summary>
        /// 领料单服务
        /// </summary>
        IMaterialRequisitionServer m_billServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 业务名称
        /// </summary>
        string m_strBusiness;

        public 单据统计(string strbusinessname)
        {
            InitializeComponent();

            m_strBusiness = strbusinessname;

            dateTime_startTime.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dateTime_endTime.Value = ServerTime.Time;
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DataTable dt = GetStatisticalData(m_strBusiness, out m_strErr);
            dgvStatistical.DataSource = dt;

            if (dt != null)
                dgvStatistical.Columns["申请部门编码"].Visible = false; // 隐藏部门编码
        }

        private void dgvStatistical_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvStatistical.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvStatistical.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvStatistical.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        /// <summary>
        /// 统计数据
        /// </summary>
        /// <param name="strBusinessName">统计的数据集名称</param>
        /// <param name="error">错误信息</param>
        /// <returns>返回Table数据集</returns>
        DataTable GetStatisticalData(string strBusinessName, out string error)
        {
            error = null;

            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();

            string[] pare = { dateTime_startTime.Value.Date.ToString(),
                                dateTime_endTime.Value.Date.ToString() };
            IQueryResult result = authorization.QueryMultParam(strBusinessName,null, pare);

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(result.Error))
            {
                if (result.DataCollection == null || result.DataCollection.Tables.Count == 0)
                {
                    return null;
                }

                DataTable dt = result.DataCollection.Tables[0];
                return dt;
            }
            else
            {
                error = result.Error;
                return null;
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dgvStatistical);
        }

        private void btnApplicantSelect_Click(object sender, EventArgs e)
        {
            if (m_strBusiness.Contains("领料单"))
            {
                DataTable dt = GetStatisticalData("领料单编制人统计", out m_strErr);
                dgvStatistical.DataSource = dt;
            }
            else if (m_strBusiness.Contains("领料退库"))
            {
                DataTable dt = GetStatisticalData("领料退库单编制人统计", out m_strErr);
                dgvStatistical.DataSource = dt;
            }
            else if (m_strBusiness.Contains("报废单"))
            {
                DataTable dt = GetStatisticalData("报废单编制人统计", out m_strErr);
                dgvStatistical.DataSource = dt;
            }

            if (dgvStatistical.Rows.Count > 0)
            {
                dgvStatistical.Columns["申请部门编码"].Visible = false; // 隐藏部门部门
                dgvStatistical.Columns["员工编号"].Visible = false;
            }
        }
    }
}
