using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using HRServerModule;
using ServerModule;
using GlobalObject;
using Expression;

namespace HRExpression
{
    /// <summary>
    /// 考勤分析汇总
    /// </summary>
    public partial class UserControlAttendanceMachineHistory : UserControl
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 考勤分析汇总操作接口
        /// </summary>
        IAttendanceSummaryServer m_attendanceServer = ServerModuleFactory.GetServerModule<IAttendanceSummaryServer>();

        public UserControlAttendanceMachineHistory(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
            AuthorityControl(m_authorityFlag);
            numYear.Value = ServerTime.Time.Year;

            RefreshDataGridView();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            if (!m_attendanceServer.GetAllSummary(out m_queryResult, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            DataTable dtTemp = dt.Clone();
            DataRow[] dr = dt.Select("年份='" + numYear.Value + "'");

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            dataGridView1.DataSource = dtTemp;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
            this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlAttendanceMachineHistory_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnYearOK_Click(object sender, EventArgs e)
        {
            if (!m_attendanceServer.GetAllSummary(out m_queryResult, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            DataTable dtTemp = dt.Clone();
            DataRow[] dr = dt.Select("年份='" + numYear.Value + "'");

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            dataGridView1.DataSource = dtTemp;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int starYear = dtpStarDate.Value.Year;
            int starMonth = dtpStarDate.Value.Month;
            int endYear = dtpEndDate.Value.Year;
            int endMonth = dtpEndDate.Value.Month;

            if (!m_attendanceServer.GetAllSummary(out m_queryResult, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            DataTable dtTemp = dt.Clone();
            DataRow[] dr = dt.Select("年份 >= '" + starYear + "' and 年份 <='" + endYear + "' and 月份 >= '" + starMonth + "' and 月份 <= '" + endMonth + "'");

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            dataGridView1.DataSource = dtTemp;
        }

        private void 综合查询toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "考勤统计";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }
    }
}
