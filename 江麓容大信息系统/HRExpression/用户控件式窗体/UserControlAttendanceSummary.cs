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
using ServerModule;
using GlobalObject;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 考勤分析汇总
    /// </summary>
    public partial class UserControlAttendanceSummary : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

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
        /// 部门信息服务组件
        /// </summary>
        IOrganizationServer m_departmentServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOrganizationServer>();

        /// <summary>
        /// 考勤分析汇总操作接口
        /// </summary>
        IAttendanceSummaryServer m_attendanceServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSummaryServer>();

        public UserControlAttendanceSummary(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
            numYear.Value = ServerTime.Time.Year;

            RefreshDataGridView();

            IQueryable<View_HR_Dept> m_findDepartment;

            if (!m_departmentServer.GetAllDeptInfo(out m_findDepartment, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            List<View_HR_Dept> deptInfo = m_findDepartment.ToList().FindAll(t => t.父级编码 == "");

            for (int i = 0; i < deptInfo.Count; i++)
            {
                if (deptInfo[i].部门名称 != "")
                {
                    cmbYearDept.Items.Add(deptInfo[i].部门名称);
                    cmbMonthDept.Items.Add(deptInfo[i].部门名称);
                }
            }

            cmbMonthDept.Items.Add("行政管理部");
            cmbYearDept.Items.Add("行政管理部");
            cmbYearDept.Items.Add("全部");
            cmbMonthDept.Items.Add("全部");

            cmbMonthDept.SelectedIndex = cmbMonthDept.Items.Count - 1;
            cmbYearDept.SelectedIndex = cmbYearDept.Items.Count - 1;
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
            if (!m_attendanceServer.GetAllSummary(out m_queryResult, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
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

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["序号"].Visible = false;
            }

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

            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.Columns[1].Frozen = true;
            dataGridView1.Columns[2].Frozen = true;
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
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
            if (!m_attendanceServer.GetAllSummary(out m_queryResult, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            DataTable dtTemp = dt.Clone();
            DataRow[] dr = dt.Select("年份='" + numYear.Value + "'");

            if (cmbYearDept.Text != "全部")
            {
                dr = dt.Select("年份='" + numYear.Value + "' and 部门编码 like '%" +
                    m_departmentServer.GetDeptCode(cmbYearDept.Text) + "%'");
            }

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            dataGridView1.DataSource = dtTemp;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["序号"].Visible = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            int starYear = dtpStarDate.Value.Year;
            int starMonth = dtpStarDate.Value.Month;
            int endYear = dtpEndDate.Value.Year;
            int endMonth = dtpEndDate.Value.Month;

            if (!m_attendanceServer.GetAllSummary(out m_queryResult, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            DataTable dtTemp = dt.Clone();

            DataRow[] dr = dt.Select("(年份 >= '" + starYear + "' and 年份 <='" + endYear + "') and (月份 >= '" + starMonth + "' and 月份 <= '" + endMonth + "')");

            if (cmbMonthDept.Text != "全部")
            {
                dr = dt.Select("(年份 >= '" + starYear + "' and 年份 <='" + endYear + "') and (月份 >= '" + starMonth + "' and 月份 <= '" 
                    + endMonth + "') and 部门编码 like '%" + m_departmentServer.GetDeptCode(cmbMonthDept.Text) + "%'");
            }

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            dataGridView1.DataSource = dtTemp;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["序号"].Visible = false;
            }
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

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dt = GetFormatTable();

            dataGridView1.DataSource = dt;

            string[] hide = { "记录员", "记录时间", "备注", "部门编码", "全勤奖金" };

            ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, hide);

            RefreshDataGridView();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[e.RowIndex]);
            form.ShowDialog();
        }

        private void UserControlAttendanceSummary_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        private void 餐补toolStripButton1_Click(object sender, EventArgs e)
        {
            DataTable dt = GetFormatTable();

            string[] hide = { "迟到情况描述", "早退情况描述","加班总小时数","平时加班总小时数", 
                      "周末加班总小时数", "法定节假日加班小时数", "加班补助总小时数", "加班调休小时数", "周末加班补助小时数", 
                      "平时加班补助小时数", "周末加班调休小时数", "平时加班调休小时数", "未打卡次数", "旷工小时数", "事假次数", 
                      "事假小时数",  "病假次数", "病假小时数","带薪病假次数", "带薪病假小时数",  "出差次数", "出差小时数","调休涉及天数","允许调休小时数", 
                      "调休次数", "调休小时数","年假次数", "年假小时数","婚假次数", "婚假小时数","产假次数", "产假小时数","清除加班小时数","年假剩余小时数",
                      "丧假次数", "丧假小时数","考试假次数", "考试假小时数","其他假次数", "其他假小时数","记录员", "记录时间", "备注", "部门编码",
                      "全勤奖金"};

            ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, hide);

            RefreshDataGridView();
        }

        /// <summary>
        /// 2013-05-09 20:00 夏石友 将导出的数据0值清除
        /// </summary>
        /// <returns>返回DataTable</returns>
        DataTable GetFormatTable()
        {
            DataTable sur = dataGridView1.DataSource as DataTable;
            DataTable dt = sur.Clone();

            for (int j = 0; j < dt.Columns.Count; j++)
            {
                dt.Columns[j].DataType = typeof(string);
            }

            for (int i = 0; i < sur.Rows.Count; i++)
            {
                dt.ImportRow(sur.Rows[i]);

                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    if (dt.Rows[i][j].ToString() == "0")
                    {
                        dt.Rows[i][j] = "";
                    }
                }
            }

            dt.DefaultView.Sort = "部门";
            dt.DefaultView.ToTable().DefaultView.Sort = "月份";

            return dt.DefaultView.ToTable();
        }

        private void 导出考勤toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dt = GetFormatTable();

            string[] hide = {"迟到次数", "迟到情况描述", "早退次数", "早退情况描述", "加班次数", "加班总小时数", "平时加班总小时数",
                                        "周末加班总小时数", "加班补助总小时数", "加班调休小时数", "未打卡次数", "旷工次数", "旷工小时数", "事假次数", 
                                        "事假涉及天数", "病假次数", "病假小时数", "病假涉及天数", "带薪病假次数", "带薪病假涉及天数", "出差次数", 
                                        "出差小时数", "出差假涉及天数","清除加班小时数", "调休次数", "调休小时数", "调休涉及天数", "年假次数", 
                                        "年假小时数", "年假涉及天数", "年假剩余小时数", "婚假次数", "婚假小时数", "婚假涉及天数", "产假次数", "产假小时数",
                                        "产假涉及天数", "丧假次数", "丧假小时数", "丧假涉及天数", "考试假次数", "考试假小时数", "考试假涉及天数", 
                                        "其他假次数", "其他假小时数", "其他假涉及天数", "扣餐补天数", "应考勤天数", "全勤奖金", "加班餐补", "出勤餐补", 
                                        "记录员", "记录时间", "部门编码","备注"};

            ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, hide);

            RefreshDataGridView();
        }

        private void btn变更允许调休小时数_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog dialog = new OpenFileDialog();
                DataTable dt = ExcelHelperP.RenderFromExcel(dialog);

                if (dt != null)
                {
                    List<HR_AttendanceSummary> lstSummary = new List<HR_AttendanceSummary>();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if ((dt.Rows[i]["工号"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dt.Rows[i]["工号"].ToString()))
                            || (dt.Rows[i]["年份"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dt.Rows[i]["年份"].ToString()))
                            || (dt.Rows[i]["月份"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dt.Rows[i]["月份"].ToString()))
                            || (dt.Rows[i]["允许调休小时数"] == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dt.Rows[i]["允许调休小时数"].ToString())))
                        {
                            throw new Exception("第"+ (i + 2).ToString() + "行记录存在【空数据】");
                        }

                        if (UniversalFunction.GetPersonnelInfo(dt.Rows[i]["工号"].ToString()) == null)
                        {
                            throw new Exception("【工号】：" + dt.Rows[i]["工号"].ToString() + "， 不存在");
                        }

                        HR_AttendanceSummary summary = new HR_AttendanceSummary();

                        summary.WorkID = dt.Rows[i]["工号"].ToString();
                        summary.Year = Convert.ToInt32(dt.Rows[i]["年份"]);
                        summary.Month = Convert.ToInt32(dt.Rows[i]["月份"]);
                        summary.AllowMobileVacationHours = Convert.ToDouble(dt.Rows[i]["允许调休小时数"]);

                        lstSummary.Add(summary);
                    }

                    m_attendanceServer.UpdateAllowMobileHours(lstSummary);

                    MessageDialog.ShowPromptMessage("修改【允许调休小时数】成功");
                }
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
            }
        }
    }
}
