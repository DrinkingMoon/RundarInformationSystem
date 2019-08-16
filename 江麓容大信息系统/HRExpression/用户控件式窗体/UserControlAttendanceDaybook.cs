using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Service_Peripheral_HR;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using System.Threading;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 人员考勤流水主界面
    /// </summary>
    public partial class UserControlAttendanceDaybook : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 考勤分析汇总操作接口
        /// </summary>
        IAttendanceSummaryServer m_attendanceServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSummaryServer>();

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceSchemeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        /// <summary>
        /// 人员考勤流水账操作类
        /// </summary>
        IAttendanceDaybookServer m_dayBookServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceDaybookServer>();

        public UserControlAttendanceDaybook(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;

            DateTime dtStart, dtEnd;

            ServerTime.GetMonthlyBalance(ServerTime.Time, out dtStart, out dtEnd);

            dtpSelectStar.Value = dtStart;
            dtpSelectEnd.Value = dtEnd.AddDays(-1);

            RefreshDataGridView();
        }

        private void UserControlAttendanceDaybook_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if (dataGridView1.Rows[i].Cells["流水号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            DataTable dt = m_dayBookServer.GetDayBookView(dtpSelectStar.Value.ToString(), dtpSelectEnd.Value.ToString());
            dataGridView1.DataSource = dt;

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

        private void UserControlAttendanceDaybook_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //string dayBookID = dataGridView1.CurrentRow.Cells["流水号"].Value.ToString();

            //FormAttendanceDayBookList frm = new FormAttendanceDayBookList(dayBookID);
            //frm.ShowDialog();

            //PositioningRecord(dayBookID);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            //Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
            //    e.RowBounds.Location.Y,
            //    dataGridView1.RowHeadersWidth - 4,
            //    e.RowBounds.Height);

            //TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
            //    dataGridView1.RowHeadersDefaultCellStyle.Font,
            //    rectangle,
            //    dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
            //    TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 综合查询toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "人员考勤流水账";
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

        private void btnOK_Click(object sender, EventArgs e)
        {
            //if (ServerTime.Time.Day >= 25 && dtpBeginDate.Value.Month < ServerTime.Time.Month)
            //{
            //    MessageDialog.ShowPromptMessage("每月25号之后不允许对上个月的考勤再做统计！");
            //    return;
            //}

            if (MessageBox.Show("节假日、异常登记等所有信息是否都已经处理完成？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                DataTable dtResult = m_dayBookServer.GetDayBookViewByDate(dtpBeginDate.Value.ToString(), dtpEndDate.Value.ToString());

                try
                {
                    for (int j = 0; j < dtResult.Rows.Count; j++)    //循环在职需要考勤的员工
                    {
                        string workID = dtResult.Rows[j]["员工编号"].ToString();

                        HR_AttendanceSetting attendanceSet = m_attendanceSchemeServer.GetAttendanceSettingByWorkID(workID);
                        string[] schemeCode = attendanceSet.SchemeCode.Split(' ');
                        string mode = m_attendanceSchemeServer.GetAttendanceSchemeByCode(schemeCode[0]).AttendanceMode;

                        DateTime starTime, endTime;

                        starTime = dtpBeginDate.Value;
                        endTime = dtpEndDate.Value;

                        if (mode.Contains("非自然"))
                        {
                            ServerTime.GetMonthlyBalance(dtpBeginDate.Value, out starTime, out endTime);
                            endTime = endTime.AddDays(-1);
                        }

                        if (!m_attendanceServer.AddAttendanceSummary(workID, starTime, endTime, out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                    }

                    if (!m_attendanceServer.AddAttendanceSummaryByAllowOverTime(out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }

                    MessageDialog.ShowPromptMessage("人员考勤统计已经完成");
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else
            {
                return;
            }
        }

        private void btnSelectOK_Click(object sender, EventArgs e)
        {
            DataTable dt = m_dayBookServer.GetDayBookView(dtpSelectStar.Value.ToString(), dtpSelectEnd.Value.ToString());
            dataGridView1.DataSource = dt;
        }

        private void 统计toolStripButton_Click(object sender, EventArgs e)
        {
            btnOK_Click(sender, e);
        }
    }
}
