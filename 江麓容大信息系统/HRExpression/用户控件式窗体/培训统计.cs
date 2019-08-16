using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using UniversalControlLibrary;
using ServerModule;
using Service_Peripheral_HR;
using GlobalObject;

namespace Form_Peripheral_HR
{
    public partial class 培训统计 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 所参加部门编码
        /// </summary>
        string m_deptCode;

        /// <summary>
        /// 中午休息小时数
        /// </summary>
        double m_restHours;

        /// <summary>
        /// 下午上班打卡时间
        /// </summary>
        string m_punchInAfternoon;

        /// <summary>
        /// 下午下班打卡时间
        /// </summary>
        string m_punchInAfternoonEnd;

        /// <summary>
        /// 上午下班打卡时间
        /// </summary>
        string m_punchInMorningEnd;

        /// <summary>
        /// 上午上班打卡时间
        /// </summary>
        string m_punchInMorning;

        /// <summary>
        /// 小时数
        /// </summary>
        //double m_hours = 0;

        /// <summary>
        /// 所选中的人员
        /// </summary>
        List<View_SelectPersonnel> m_lstPerson = new List<View_SelectPersonnel>();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 培训统计服务类
        /// </summary>
        ICultivateServer m_cultivateServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ICultivateServer>();

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceSchemeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        public 培训统计(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
            DateTime time = ServerTime.Time;
            dtpQueryStartTime.Value = time.AddDays(1).AddMonths(-1);
            dtpQueryEndTime.Value = time.AddDays(1);
            dtpStartTime.Value = time;
            dtpEndTime.Value = time;

            RefreshDataGridView();

            //获取考勤作息时间
            IQueryable<HR_AttendanceScheme> dtScheme = m_attendanceSchemeServer.GetLinqResult();

            string punchInMorning = dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheMorning.Value.Minute.ToString();

            string punchInMorningEnd = dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheMorning.Value.Minute.ToString();

            string punchInAfternoonEnd = dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().EndTimeInTheAfternoon.Value.Minute.ToString();

            string punchInAfternoon = dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Hour.ToString() +
                ":" + dtScheme.Take(1).Single().BeginTimeInTheAfternoon.Value.Minute.ToString();

            double restHours = (Convert.ToDateTime(punchInAfternoon) - Convert.ToDateTime(punchInMorningEnd)).Hours;
            restHours += Convert.ToDouble((Convert.ToDateTime(punchInAfternoon) - Convert.ToDateTime(punchInMorningEnd)).Minutes) / 60;

            m_punchInMorning = punchInMorning;
            m_punchInMorningEnd = punchInMorningEnd;
            m_punchInAfternoonEnd = punchInAfternoonEnd;
            m_punchInAfternoon = punchInAfternoon;
            m_restHours = restHours;
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void 培训统计_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        /// <summary>
        /// 改变控件大小
        /// </summary>
        private void 培训统计_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            m_cultivateServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                      + "培训开始时间>='" + dtpQueryStartTime.Value.ToString() + "' and 培训开始时间<='" + dtpQueryEndTime.Value.ToString() + "'";

            IQueryResult result;

            if (!m_cultivateServer.GetAllBill(out result,out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

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

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>控件输入正确返回true，否则返回false</returns>
        bool CheckControl()
        {
            if (txtCourseName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入课件名称！");
                return false;
            }

            if (cmbCultivateType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择培训类别！");
                return false;
            }

            if (txtCultivateLecturer.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入培训讲师！");
                return false;
            } 

            if (dtpStartTime.Value >= dtpEndTime.Value)
            {
                MessageDialog.ShowPromptMessage("请输入正确培训起止时间！");
                return false;
            }
            
            if (txtDept.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择参加部门！");
                return false;
            }

            if (txtPerson.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择参加人员！");
                return false;
            }

            return true;
        }

        private void 提交toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            HR_CultivateStatistics cultivate = new HR_CultivateStatistics();

            cultivate.CourseName = txtCourseName.Text.Trim();
            cultivate.CultivateLecturer = txtCultivateLecturer.Text.Trim();
            cultivate.CultivateType = cmbCultivateType.Text;
            cultivate.Dept = txtDept.Text;
            cultivate.EndTime = dtpEndTime.Value;
            cultivate.IsCourseware = cbIsCourseware.Checked;
            //cultivate.Person = txtPerson.Text;
            cultivate.Recorder = BasicInfo.LoginID;
            cultivate.RecordTime = ServerTime.Time;
            cultivate.StartTime = dtpStartTime.Value;
            cultivate.IsWorkTime = cbIsWorkTime.Checked;
            cultivate.SumHours = Convert.ToDecimal(txtSumHours.Text);

            if (!m_cultivateServer.InsertBill(cultivate, m_lstPerson, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功！");
            }

            RefreshDataGridView();
        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {
            double tempHours = (dtpEndTime.Value - dtpStartTime.Value).Hours;

            if ((dtpEndTime.Value - dtpStartTime.Value).Minutes >= 0 
                && (dtpEndTime.Value.Minute != dtpStartTime.Value.Minute))
            {
                tempHours = tempHours + Convert.ToDouble(((dtpEndTime.Value - dtpStartTime.Value).Minutes)) / 60;
            }
            else if ((dtpEndTime.Value - dtpStartTime.Value).Minutes < 0
                && (dtpEndTime.Value.Minute != dtpStartTime.Value.Minute))
            {
                tempHours = tempHours - 1;
                tempHours = tempHours + Convert.ToDouble(((dtpEndTime.Value - dtpStartTime.Value).Minutes)) / 60;
            }

            txtSumHours.Text = Math.Round(tempHours, 1, MidpointRounding.AwayFromZero).ToString();
        }

        private void dtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            double tempHours = (dtpEndTime.Value - dtpStartTime.Value).Hours;

            if ((dtpEndTime.Value - dtpStartTime.Value).Minutes >= 0
                && (dtpEndTime.Value.Minute != dtpStartTime.Value.Minute))
            {
                tempHours = tempHours + Convert.ToDouble((dtpEndTime.Value - dtpStartTime.Value).Minutes) / 60;
            }
            else if ((dtpEndTime.Value - dtpStartTime.Value).Minutes < 0
                && (dtpEndTime.Value.Minute != dtpStartTime.Value.Minute))
            {
                tempHours = tempHours - 1;
                tempHours = tempHours + Convert.ToDouble(((dtpEndTime.Value - dtpStartTime.Value).Minutes)) / 60;
            }

            if (Convert.ToDateTime(dtpStartTime.Value.ToShortTimeString()) < Convert.ToDateTime(m_punchInMorningEnd)
                    && Convert.ToDateTime(dtpEndTime.Value.ToShortTimeString()) > Convert.ToDateTime(m_punchInAfternoon))
            {
                tempHours = tempHours - m_restHours;
            }

            txtSumHours.Text = Math.Round(tempHours, 1, MidpointRounding.AwayFromZero).ToString();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtCourseName.Text = dataGridView1.CurrentRow.Cells["课件名称"].Value.ToString();
            txtCultivateLecturer.Text = dataGridView1.CurrentRow.Cells["培训讲师"].Value.ToString();
            txtDept.Text = dataGridView1.CurrentRow.Cells["参加部门"].Value.ToString();
            txtPerson.Text = dataGridView1.CurrentRow.Cells["参加人员"].Value.ToString();
            txtSumHours.Text = dataGridView1.CurrentRow.Cells["培训小时"].Value.ToString();
            dtpStartTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["培训开始时间"].Value);
            dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["培训终止时间"].Value);
            cmbCultivateType.Text = dataGridView1.CurrentRow.Cells["培训类别"].Value.ToString();
            cbIsCourseware.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["有无课件"].Value);
            cbIsWorkTime.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否工作时间"].Value);

            if (txtPerson.Text.Trim()!="")
            {
                string[] person = txtPerson.Text.Split('，');
                m_lstPerson = new List<View_SelectPersonnel>();

                for (int i = 0; i < person.Length; i++)
                {
                    View_SelectPersonnel lstPerson = new View_SelectPersonnel();

                    lstPerson.员工姓名 = person[i];

                    m_lstPerson.Add(lstPerson);
                }
            }

            string[] dept = txtDept.Text.Split(';');
            m_deptCode = "";

            for (int i = 0; i < dept.Length; i++)
            {
                m_deptCode += UniversalFunction.GetDeptCode(dept[i]) + ";";
            }
        }

        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["记录人员"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("只有记录人才能修改！");
                return;
            }

            if (!CheckControl())
            {
                return;
            }

            HR_CultivateStatistics cultivate = new HR_CultivateStatistics();

            cultivate.CourseName = txtCourseName.Text.Trim();
            cultivate.CultivateLecturer = txtCultivateLecturer.Text.Trim();
            cultivate.CultivateType = cmbCultivateType.Text;
            cultivate.Dept = txtDept.Text;
            cultivate.EndTime = dtpEndTime.Value;
            cultivate.IsCourseware = cbIsCourseware.Checked;
            cultivate.Recorder = BasicInfo.LoginID;
            cultivate.RecordTime = ServerTime.Time;
            cultivate.StartTime = dtpStartTime.Value;
            cultivate.SumHours = Convert.ToDecimal(txtSumHours.Text);
            cultivate.IsWorkTime = cbIsWorkTime.Checked;

            if (!m_cultivateServer.UpdatetBill((int)(dataGridView1.CurrentRow.Cells["序号"].Value), cultivate,m_lstPerson, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功！");
                RefreshDataGridView();
            }
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中行吗？") == DialogResult.Yes)
                {
                    if (Convert.ToDateTime(dataGridView1.CurrentRow.Cells["培训开始时间"].Value).Month != ServerTime.Time.Month)
                    {
                        MessageDialog.ShowPromptMessage("只能删除本月培训的数据！");
                        return;
                    }

                    if (!m_cultivateServer.DeleteBill(Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                        RefreshDataGridView();
                    }
                }
            }
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 综合查询toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "培训综合统计查询";
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

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("员工");

            txtCultivateLecturer.Text = "";

            if (form.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < form.SelectedUser.Count; i++)
                {
                    txtCultivateLecturer.Text += form.SelectedUser[i].员工姓名 + ";";
                }

                txtCultivateLecturer.Text = txtCultivateLecturer.Text.Substring(0, txtCultivateLecturer.Text.Length - 1);
            }
        }

        private void btnDept_Click(object sender, EventArgs e)
        {
            FormSelectPersonnel form = new FormSelectPersonnel("部门");

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtDept.Text = "";
                m_deptCode = "";

                for (int i = 0; i < form.SelectedDept.Count; i++)
                {
                    txtDept.Text += form.SelectedDept[i].部门名称 + ";";
                    m_deptCode += form.SelectedDept[i].部门代码 + ";";
                }

                txtDept.Text = txtDept.Text.Substring(0, txtDept.Text.Length - 1);
                btnPerson_Click(sender, e);
            }
        }

        private void btnPerson_Click(object sender, EventArgs e)
        {
            if (txtDept.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请先选择参与部门！");
                return;
            }

            FormSelectPersonnel form = new FormSelectPersonnel("员工");
            
            form.DeptCode = m_deptCode;

            if (txtPerson.Text.Trim() != "")
            {
                string[] person = txtPerson.Text.Split('，');

                List<View_SelectPersonnel> list = new List<View_SelectPersonnel>();

                for (int i = 0; i < person.Length; i++)
                {
                    if (person[i] != "")
                    {
                        View_SelectPersonnel personnel = new View_SelectPersonnel();

                        personnel.员工编号 = UniversalFunction.GetPersonnelCode(person[i]);
                        list.Add(personnel);
                    }
                }

                form.SelectedUser = list;
            }            

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPerson.Text = "";
                m_lstPerson = form.SelectedUser;

                for (int i = 0; i < form.SelectedUser.Count; i++)
                {
                    txtPerson.Text += form.SelectedUser[i].员工姓名 + ";";
                }

                txtPerson.Text = txtPerson.Text.Substring(0, txtPerson.Text.Length - 1);
            }
        }

        private void 新建toolStripButton_Click(object sender, EventArgs e)
        {
            txtCourseName.Text = "";
            txtCultivateLecturer.Text = "";
            txtDept.Text = "";
            txtPerson.Text = "";
            txtSumHours.Text = "";
            cmbCultivateType.SelectedIndex = -1;
            cbIsCourseware.Checked = false;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            txtCourseName.Text = dataGridView1.CurrentRow.Cells["课件名称"].Value.ToString();
            txtCultivateLecturer.Text = dataGridView1.CurrentRow.Cells["培训讲师"].Value.ToString();
            txtDept.Text = dataGridView1.CurrentRow.Cells["参加部门"].Value.ToString();
            txtPerson.Text = dataGridView1.CurrentRow.Cells["参加人员"].Value.ToString();
            txtSumHours.Text = dataGridView1.CurrentRow.Cells["培训小时"].Value.ToString();
            dtpStartTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["培训开始时间"].Value);
            dtpEndTime.Value = Convert.ToDateTime(dataGridView1.CurrentRow.Cells["培训终止时间"].Value);
            cmbCultivateType.Text = dataGridView1.CurrentRow.Cells["培训类别"].Value.ToString();
            cbIsCourseware.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["有无课件"].Value);
            cbIsWorkTime.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否工作时间"].Value);

            if (txtPerson.Text.Trim() != "")
            {
                string[] person = txtPerson.Text.Split('，');
                m_lstPerson = new List<View_SelectPersonnel>();

                for (int i = 0; i < person.Length; i++)
                {
                    View_SelectPersonnel lstPerson = new View_SelectPersonnel();

                    lstPerson.员工姓名 = person[i];

                    m_lstPerson.Add(lstPerson);
                }
            }

            string[] dept = txtDept.Text.Split(';');
            m_deptCode = "";

            for (int i = 0; i < dept.Length; i++)
            {
                m_deptCode += UniversalFunction.GetDeptCode(dept[i]) + ";";
            }
        }

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }
    }
}
