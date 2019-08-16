using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Service_Peripheral_HR;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using CommonBusinessModule;

namespace Form_Peripheral_HR
{
    public partial class FormWorkSchedule : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 最高部门
        /// </summary>
        string m_highDept;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// 排班信息操作类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        /// <summary>
        /// 排班表单据号
        /// </summary>
        private int m_billNo;

        /// <summary>
        /// 排班主表信息
        /// </summary>
        private View_HR_WorkScheduling m_workSchedulingInfo;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 排班明细信息
        /// </summary>
        private List<View_HR_WorkSchedulingDetail> m_workSchedulingDetail;

        /// <summary>
        /// 排班表包含的人数
        /// </summary>
        private int m_numberOfPeople;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        /// <param name="billNo">排班单据号，如果为0，则表示为新建方式</param>
        public FormWorkSchedule(AuthorityFlag authorityFlag, int billNo)
        {
            InitializeComponent();
            
            m_billMessageServer.BillType = "排班管理";

            m_billNo = billNo;
            m_authFlag = authorityFlag;

            this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;

            toolStrip1.Visible = true;

            if (m_billNo == 0)
            {
                DateTime time = ServerTime.Time;

                dtpBegin.Value = time;
                dtpEnd.Value = time;
                numYear.Value = time.Year;
                numMonth.Value = time.Month;

                ClearControl();
            }
            else
            {
                m_workSchedulingInfo = m_workSchedulingServer.GetWorkSchedulingByBillNo(m_billNo);

                m_workSchedulingDetail = m_workSchedulingServer.GetWorkSchedulingDetail(m_billNo, out m_numberOfPeople);

                InitDataGridView(m_workSchedulingInfo.排班起始时间, m_workSchedulingInfo.排班结束时间);

                txtBillNo.Text = m_billNo.ToString();
                txtCreaterWorkID.Text = m_workSchedulingInfo.排班人;
                txtCreaterWorkID.Tag = m_workSchedulingInfo.员工编号;
                txtDeptDirector.Text = m_workSchedulingInfo.部门主管;
                txtDeptPrincipal.Text = m_workSchedulingInfo.部门负责人;
                txtRemark.Text = m_workSchedulingInfo.备注;
                txtStatus.Text = m_workSchedulingInfo.单据状态;

                dtpBegin.Value = m_workSchedulingInfo.排班起始时间;
                dtpEnd.Value = m_workSchedulingInfo.排班结束时间;

                if (m_workSchedulingInfo.已审核日期.ToString() != "")
                {
                    dtpCompletion.Value = Convert.ToDateTime(m_workSchedulingInfo.已审核日期);
                }

                if (m_workSchedulingInfo.主管签定时间.ToString() != "")
                {
                    dtpDeptDirectorDate.Value = Convert.ToDateTime(m_workSchedulingInfo.主管签定时间);
                }

                if (m_workSchedulingInfo.负责人签定时间.ToString() != "")
                {
                    dtpDeptPrincipalDate.Value = Convert.ToDateTime(m_workSchedulingInfo.负责人签定时间);
                }

                dtpCreateDate.Value = m_workSchedulingInfo.排班时间;
                dtpEnd.Value = m_workSchedulingInfo.排班结束时间;
                dateTimePendingDate.Value = m_workSchedulingInfo.待审核日期;
                txtScheduleName.Text = m_workSchedulingInfo.排班名称;

                numYear.Value = m_workSchedulingInfo.排班年份;
                numMonth.Value = m_workSchedulingInfo.排班月份;
                
                if (dateTimePendingDate.Value > dtpCompletion.Value)
                {
                    dateTimePendingDate.Enabled = false;
                }
            }

            DataTable dt = m_personnerServer.GetHighestDept(txtCreaterWorkID.Tag.ToString());

            if (dt != null && dt.Rows.Count > 0)
            {
                m_highDept = dt.Rows[0]["deptCode"].ToString();
            }
        }

        private void FormWorkSchedule_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
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
        /// 初始化数据显示控件
        /// </summary>
        /// <param name="beginDate">排班开始时间</param>
        /// <param name="endDate">排班结束时间</param>
        void InitDataGridView(DateTime beginDate, DateTime endDate)
        {
            if (dataGridView1.Columns.Count > 0 || beginDate == endDate)
            {
                return;
            }

            dataGridView1.Columns.Add("工号", "工号");
            dataGridView1.Columns.Add("姓名", "姓名");

            DataGridViewComboBoxColumn column = new DataGridViewComboBoxColumn();
            int columnWidth = 120;

            column.SortMode = DataGridViewColumnSortMode.NotSortable;
            column.Visible = true;
            column.ReadOnly = false;

            if (beginDate.Month == endDate.Month)
            {
                for (int i = beginDate.Day; i <= endDate.Day; i++)
                {
                    string name = string.Format("{0:D2}-{1:D2}", beginDate.Month, i);

                    DataGridViewComboBoxColumn newColumn = column.Clone() as DataGridViewComboBoxColumn;

                    newColumn.Name = name;
                    newColumn.HeaderText = i.ToString("D2");

                    dataGridView1.Columns.Add(newColumn);
                    dataGridView1.Columns[dataGridView1.Columns.Count - 1].Width = columnWidth;
                }
            }
            else
            {
                // 获取指定日期所在月份包含的天数
                int day1 = GlobalObject.Year.GetDays(beginDate);
                int day2 = GlobalObject.Year.GetDays(endDate);

                for (int i = beginDate.Day; i <= day1; i++)
                {
                    string name = string.Format("{0:D2}-{1:D2}", beginDate.Month, i);

                    DataGridViewComboBoxColumn newColumn = column.Clone() as DataGridViewComboBoxColumn;

                    newColumn.Name = name;
                    newColumn.HeaderText = i.ToString("D2");

                    dataGridView1.Columns.Add(newColumn);
                    dataGridView1.Columns[dataGridView1.Columns.Count - 1].Width = columnWidth;
                }

                for (int i = 1; i <= endDate.Day; i++)
                {
                    string name = string.Format("{0:D2}-{1:D2}", endDate.Month, i);

                    DataGridViewComboBoxColumn newColumn = column.Clone() as DataGridViewComboBoxColumn;

                    newColumn.Name = name;
                    newColumn.HeaderText = i.ToString("D2");

                    dataGridView1.Columns.Add(newColumn);
                    dataGridView1.Columns[dataGridView1.Columns.Count - 1].Width = columnWidth;
                }
            }

            InitWorkDefinitionComboBox();

            RefreshDataGridView();
        }

        /// <summary>
        /// 获取指定时间对应的列索引
        /// </summary>
        /// <param name="dt">要获取索引的时间</param>
        /// <returns>成功返回>=0的索引号，失败返回-1</returns>
        private int GetColumnIndex(DateTime dt)
        {
            for (int i = 2; i < dataGridView1.Columns.Count; i++)
            {
                if (dataGridView1.Columns[i].Name == dt.ToString("MM-dd"))
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            if (m_workSchedulingDetail != null)
            {
                int pos = 0;

                for (int index = 0; index < m_numberOfPeople; index++)
                {
                    string[] rowData = new string[dataGridView1.Columns.Count];
                    string preLineWorkId = m_workSchedulingDetail[pos].员工编号;

                    rowData[0] = m_workSchedulingDetail[pos].员工编号;
                    rowData[1] = m_workSchedulingDetail[pos].员工姓名;

                    if (GetColumnIndex(m_workSchedulingDetail[pos].时间) != -1)
                    {
                        rowData[GetColumnIndex(m_workSchedulingDetail[pos].时间)] = m_workSchedulingDetail[pos].排班;

                        pos++;

                        for (int i = pos; i < m_workSchedulingDetail.Count; i++, pos++)
                        {
                            if (preLineWorkId == m_workSchedulingDetail[i].员工编号)
                            {
                                rowData[GetColumnIndex(m_workSchedulingDetail[i].时间)] = m_workSchedulingDetail[pos].排班;
                            }
                            else
                            {
                                break;
                            }
                        }

                        dataGridView1.Rows.Add(rowData);
                    }

                    dataGridView1.Columns[0].Frozen = true;
                    dataGridView1.Columns[1].Frozen = true;
                }
            }
        }

        /// <summary>
        /// 查找人员
        /// </summary>
        private void btnFindCode(DataGridViewRow row)
        {
            FormPersonnel form = new FormPersonnel(row.Cells["姓名"], "姓名");
            form.ShowDialog();

            row.Cells["工号"].Value = form.UserCode;
            row.Cells["姓名"].Value = form.UserName;            
        }

        /// <summary>
        /// 初始化下拉框
        /// </summary>
        private void InitWorkDefinitionComboBox()
        {
            DataTable dt = m_workSchedulingServer.GetWorkSchedulingDefinition();

            if (dt != null)
            {
                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].CellType.Name == "DataGridViewComboBoxCell")
                    {
                        dataGridView1.Columns[i].DataPropertyName = "定义编码";

                        DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns[i] as DataGridViewComboBoxColumn;
                        dgvComboBoxColumn.DataSource = dt.DefaultView;
                        dgvComboBoxColumn.DisplayMember = "定义名称";
                        dgvComboBoxColumn.ValueMember = "定义编码";
                    }
                }
            }
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void ClearControl()
        {
            txtCreaterWorkID.Text = BasicInfo.LoginName;
            txtCreaterWorkID.Tag = BasicInfo.LoginID;
            txtDeptDirector.Text = "";
            txtDeptPrincipal.Text = "";
            txtRemark.Text = "";
            txtScheduleName.Text = "";
            
            dtpCreateDate.Value = ServerTime.Time;
            dtpDeptDirectorDate.Value = ServerTime.Time;
            dtpDeptPrincipalDate.Value = ServerTime.Time;
            dtpBegin.Value = ServerTime.Time;
            dtpEnd.Value = ServerTime.Time;

            txtBillNo.Text = "系统自动生成";
            txtStatus.Text = "新建单据";
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            ClearControl();
            m_workSchedulingDetail = null;
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
        }

        /// <summary>
        /// 检查控件输入的正确性
        /// </summary>
        /// <returns>正确返回True，否则返回False</returns>
        bool CheckControl()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                if (cells["姓名"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["姓名"];
                    dataGridView1.BeginEdit(true);

                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请选择员工", i + 1));
                    return false;
                }
            }

            if (txtScheduleName.Text.Trim() == "") 
            {
                MessageDialog.ShowPromptMessage("请填写排班名称！");
                return false;
            }

            if (txtRemark.Text.Trim() == "")
            {
                txtRemark.Text = " ";
            }

            return true;
        }

        /// <summary>
        /// 添加人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddPersonnel_Click(object sender, EventArgs e)
        {
            if (dtpEnd.Value > dtpBegin.Value)
            {
                InitDataGridView(dtpBegin.Value, dtpEnd.Value);

                //FormSelectPersonnel frm = new FormSelectPersonnel("员工");
                FormSelectPersonnel2 frm = new FormSelectPersonnel2();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList != null && frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList.Count > 0)
                    {
                        foreach (var item in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                        {
                            bool flag = false;
                            string[] rowData = new string[dataGridView1.Columns.Count];
                            string preLineWorkId = item.工号;

                            for (int i = 0; i < dataGridView1.Rows.Count; i++)
                            {
                                if (dataGridView1.Rows[i].Cells["工号"].Value.ToString() == preLineWorkId)
                                {
                                    flag = true;
                                    break;
                                }
                            }

                            if (!flag)
                            {
                                rowData[0] = item.工号;
                                rowData[1] = item.姓名;

                                dataGridView1.Rows.Add(rowData);
                            }
                        }
                    }
                }

                dataGridView1.Columns[0].Frozen = true;
                dataGridView1.Columns[1].Frozen = true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("请设置好排班表的起止时间后再进行此操作");
                return;
            }
        }

        /// <summary>
        /// 删除人员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeletePersonnel_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您确定要删除选择行？") == DialogResult.No)
            {
                return;
            }

            for (int i = dataGridView1.SelectedRows.Count; i > 0; i--)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[i - 1]);
            }
        }

        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() == "已完成" || txtStatus.Text.Trim() == "等待下次排班")
            {
                MessageDialog.ShowPromptMessage("单据已经完成，不能修改");
                return;
            }

            if (!CheckControl())
            {
                return;
            }

            List<HR_WorkSchedulingDetail> lstPersonnel = new List<HR_WorkSchedulingDetail>();

            int day1 = GlobalObject.Year.GetDays(dtpBegin.Value);
            int days = 0;

            for (int i = dtpBegin.Value.Day; i <= day1; i++)
            {
                days++;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    HR_WorkSchedulingDetail personnel = new HR_WorkSchedulingDetail();
                    DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns[j] as DataGridViewComboBoxColumn;

                    if (dataGridView1.Columns[j].CellType.Name == "DataGridViewComboBoxCell" && cells[j].Value != null)
                    {
                        personnel.WorkID = dataGridView1.Rows[i].Cells["工号"].Value.ToString();
                        personnel.Code = cells[j].Value.ToString();

                        if (dtpBegin.Value.Month == dtpEnd.Value.Month)
                        {
                            if (j - 1 <= days)
                            {
                                personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpBegin.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (j - 1 <= days)
                            {
                                personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpBegin.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            }
                            else
                            {
                                personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpEnd.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            }
                        }

                        lstPersonnel.Add(personnel);
                    }
                }
            }

            HR_WorkScheduling workSchedule = new HR_WorkScheduling();
            bool isDeptDirector = false;

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(m_personnerServer.GetPersonnelInfo(txtCreaterWorkID.Tag.ToString()).Dept, "0");
            bool flag = false;

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                isDeptDirector = true;
                foreach (var item in directorGroup)
                {
                    if (BasicInfo.LoginID == item.员工编号)
                    {
                        flag = true;
                        break;
                    }
                }
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(m_personnerServer.GetPersonnelInfo(txtCreaterWorkID.Tag.ToString()).Dept, "1");
            bool flagPri = false;

            if (directorGroup1 != null && directorGroup1.Count() > 0)
            {
                foreach (var item in directorGroup1)
                {
                    if (BasicInfo.LoginID == item.员工编号)
                    {
                        flagPri = true;
                        break;
                    }
                }
            }

            if (!flag && !flagPri)
            {
                if (isDeptDirector)
                {
                    workSchedule.BillStatus = OverTimeBillStatus.等待主管审核.ToString();
                }
                else
                {
                    workSchedule.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                }
            }
            else if (flag && !flagPri)
            {
                workSchedule.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
            }
            else
            {
                workSchedule.BillStatus = OverTimeBillStatus.已完成.ToString();
            }

            workSchedule.ScheduleName = txtScheduleName.Text;
            workSchedule.CreateDate = dtpCreateDate.Value;
            workSchedule.CreaterWorkID = txtCreaterWorkID.Tag.ToString();
            workSchedule.BeginDate = dtpBegin.Value;
            workSchedule.EndDate = dtpEnd.Value;
            workSchedule.Remark = txtRemark.Text;
            workSchedule.Year = Convert.ToInt32(numYear.Value);
            workSchedule.Month = Convert.ToInt32(numMonth.Value);
            workSchedule.PendingDate = Convert.ToDateTime(dateTimePendingDate.Value.ToShortDateString());

            if (!m_workSchedulingServer.UpdateWorkScheduling(workSchedule, lstPersonnel, Convert.ToInt32(txtBillNo.Text), out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("修改成功！");
            }

            this.Close();
        }

        private void 提交toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != "新建单据")
            {
                MessageDialog.ShowPromptMessage("请确认单据状态");
                return;
            }

            if (m_workSchedulingServer.IsExise(BasicInfo.LoginID, Convert.ToInt32(numMonth.Value.ToString())))
            {
                MessageDialog.ShowPromptMessage(numMonth.Value + "月已经存在排班！");
                return;
            }

            if (!CheckControl())
            {
                return;
            }

            List<HR_WorkSchedulingDetail> lstPersonnel = new List<HR_WorkSchedulingDetail>();

            int day1 = GlobalObject.Year.GetDays(dtpBegin.Value);
            int days = 0;

            for (int i = dtpBegin.Value.Day; i <= day1; i++)
            {
                days++;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                for (int j = 0; j < dataGridView1.Columns.Count; j++)
                {
                    HR_WorkSchedulingDetail personnel = new HR_WorkSchedulingDetail();
                    DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns[j] as DataGridViewComboBoxColumn;

                    if (dataGridView1.Columns[j].CellType.Name == "DataGridViewComboBoxCell" && cells[j].Value != null)
                    {
                        personnel.WorkID = dataGridView1.Rows[i].Cells["工号"].Value.ToString();

                        if (dtpBegin.Value.Month == dtpEnd.Value.Month)
                        {
                            //personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpBegin.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            if (j - 1 <= days)
                            {
                                personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpBegin.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            if (j - 1 <= days)
                            {
                                personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpBegin.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            }
                            else
                            {
                                personnel.Date = Convert.ToDateTime(numYear.Value + "-" + dtpEnd.Value.Month + "-" + dataGridView1.Columns[j].HeaderText);
                            }
                        }

                        personnel.Code = cells[j].Value == DBNull.Value ? "" : cells[j].Value.ToString();
                        lstPersonnel.Add(personnel);
                    }
                }
            }

            HR_WorkScheduling workSchedule = new HR_WorkScheduling();
            bool isDeptDirector = false;

            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(m_personnerServer.GetPersonnelInfo(txtCreaterWorkID.Tag.ToString()).Dept, "0");
            bool flag = false;

            if (directorGroup != null && directorGroup.Count() > 0)
            {
                isDeptDirector = true;

                foreach (var item in directorGroup)
                {
                    if (BasicInfo.LoginID == item.员工编号)
                    {
                        flag = true;

                        break;
                    }
                }
            }

            IQueryable<View_HR_PersonnelArchive> directorGroup1 = m_personnerServer.GetDeptDirector(m_personnerServer.GetPersonnelInfo(txtCreaterWorkID.Tag.ToString()).Dept, "1");
            bool flagPri = false;

            if (directorGroup1 != null && directorGroup1.Count() > 0)
            {
                foreach (var item in directorGroup1)
                {
                    if (BasicInfo.LoginID == item.员工编号)
                    {
                        flagPri = true;

                        break;
                    }
                }
            }

            if (!flag && !flagPri)
            {
                if (isDeptDirector)
                {
                    workSchedule.BillStatus = OverTimeBillStatus.等待主管审核.ToString();
                }
                else
                {
                    workSchedule.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
                }
            }
            else if (flag && !flagPri)
            {
                workSchedule.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
            }

            workSchedule.ScheduleName = txtScheduleName.Text;
            workSchedule.CreateDate = dtpCreateDate.Value;
            workSchedule.CreaterWorkID = txtCreaterWorkID.Tag.ToString();
            workSchedule.BeginDate = dtpBegin.Value;
            workSchedule.EndDate = dtpEnd.Value;
            workSchedule.Remark = txtRemark.Text;
            workSchedule.Year = Convert.ToInt32(numYear.Value);
            workSchedule.Month = Convert.ToInt32(numMonth.Value);
            workSchedule.PendingDate = Convert.ToDateTime(dateTimePendingDate.Value.ToShortDateString());

            txtBillNo.Text = m_workSchedulingServer.AddWorkScheduling(workSchedule, lstPersonnel, out error).ToString();

            if (Convert.ToInt32(txtBillNo.Text) < 0)
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("新增成功！");
                m_billMessageServer.DestroyMessage(txtBillNo.Text);

                if (workSchedule.BillStatus.Equals(OverTimeBillStatus.等待主管审核.ToString()))
                {
                    m_billMessageServer.SendNewFlowMessage(txtBillNo.Text, string.Format("{0}号请假申请单，请主管审核", txtBillNo.Text),
                        BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptDirectorRoleName(
                        m_personnerServer.GetPersonnelViewInfo(txtCreaterWorkID.Tag.ToString()).部门编号).ToList());
                }
                else if (workSchedule.BillStatus == OverTimeBillStatus.等待部门负责人审核.ToString())
                {
                    m_billMessageServer.SendNewFlowMessage(txtBillNo.Text, string.Format("{0}号请假申请单，请部门负责人审核", m_billNo),
                        BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                        m_personnerServer.GetPersonnelViewInfo(txtCreaterWorkID.Tag.ToString()).部门编号).ToList());
                }
            }

            this.Close();
        }

        private void 主管toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != OverTimeBillStatus.等待主管审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态");
                return;
            }

            HR_WorkScheduling workSchedule = new HR_WorkScheduling();

            workSchedule.ID = Convert.ToInt32(txtBillNo.Text);
            workSchedule.DeptDirector = BasicInfo.LoginID;
            workSchedule.DeptDirectorSignatureDate = ServerTime.Time; 
            workSchedule.CompletionDate = Convert.ToDateTime(dateTimePendingDate.Value.ToShortDateString());

            if (Convert.ToDateTime(dateTimePendingDate.Value.ToShortDateString()) < Convert.ToDateTime(dtpEnd.Value.ToShortDateString()))
            {
                workSchedule.BillStatus = "等待下次排班";
            }
            else
            {
                workSchedule.BillStatus = OverTimeBillStatus.等待部门负责人审核.ToString();
            }

            if (!m_workSchedulingServer.UpdateAuditingWorkScheduling(workSchedule, "主管", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("部门主管审核成功！");
                txtStatus.Text = workSchedule.BillStatus;

                if (workSchedule.BillStatus.Equals("等待下次排班"))
                {
                    string msg = string.Format("{0} 号排班信息部门主管审批成功，排班人员可以进行下次排班", txtBillNo.Text);
                    m_billMessageServer.PassFlowMessage(txtBillNo.Text, msg, BillFlowMessage_ReceivedUserType.用户, txtCreaterWorkID.Tag.ToString());
                }
                else
                {
                    string msg = string.Format("{0} 号排班信息部门主管审核成功，请部门负责人审批", txtBillNo.Text);
                    m_billMessageServer.PassFlowMessage(txtBillNo.Text, msg, BillFlowMessage_ReceivedUserType.角色,
                          m_billMessageServer.GetDeptPrincipalRoleName(m_personnerServer.GetPersonnelViewInfo(txtCreaterWorkID.Tag.ToString()).部门编号).ToList());
                }

                this.Close();
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(DataGridViewComboBoxCell))
            {
                e.Cancel = true;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(DataGridViewComboBoxColumn))
            {
                e.Cancel = true;
            }
        }

        private void 负责人toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != OverTimeBillStatus.等待部门负责人审核.ToString())
            {
                MessageDialog.ShowPromptMessage("请确认单据状态");
                return;
            }

            HR_WorkScheduling workSchedule = new HR_WorkScheduling();

            workSchedule.ID = Convert.ToInt32(txtBillNo.Text);
            workSchedule.DeptDirector = BasicInfo.LoginID;
            workSchedule.DeptDirectorSignatureDate = ServerTime.Time;
            workSchedule.BillStatus = OverTimeBillStatus.已完成.ToString();

            if (!m_workSchedulingServer.UpdateAuditingWorkScheduling(workSchedule, "负责人", out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("部门负责人确认成功！");
                txtStatus.Text = workSchedule.BillStatus;

                List<string> noticeRoles = new List<string>();
                List<string> noticeUser = new List<string>();

                noticeUser.Add(txtCreaterWorkID.Tag.ToString());
                m_billMessageServer.EndFlowMessage(txtBillNo.Text, string.Format("{0} 号排班信息已经处理完毕", txtBillNo.Text), noticeRoles, noticeUser);
                this.Close();
            }
        }

        private void 引用toolStripButton_Click(object sender, EventArgs e)
        {
            int num = dataGridView1.Rows.Count;

            FormOldWorkSchedule frm = new FormOldWorkSchedule();
            frm.ShowDialog();

            if (frm.Flag)
            {
                m_billNo = Convert.ToInt32(frm.BillNo);
                m_workSchedulingInfo = m_workSchedulingServer.GetWorkSchedulingByBillNo(m_billNo);
                m_workSchedulingDetail = m_workSchedulingServer.GetWorkSchedulingDetail(m_billNo, out m_numberOfPeople);
                InitDataGridView(m_workSchedulingInfo.排班起始时间, m_workSchedulingInfo.排班结束时间);

                int newNum = dataGridView1.Rows.Count;

                if (num == newNum)
                {
                    MessageDialog.ShowPromptMessage("点击【新建】后，若需要引用原有排班，请先【引用旧排班】！");
                }
            }
        }

        private void 统计toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtBillNo.Text.Trim() != "系统自动生成")
            {
                FormStatistics frm = new FormStatistics(Convert.ToInt32(txtBillNo.Text));
                frm.Show();
            }
        }

        private void 同班toolStripButton_Click(object sender, EventArgs e)
        {
            List<int> lstRow = new List<int>();

            if (dataGridView1.SelectedRows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                {
                    lstRow.Add(dataGridView1.SelectedRows[i].Index);
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    lstRow.Add(i);
                }
            }

            if (lstRow.Count > 0)
            {
                批量排班选择班次 frm = new 批量排班选择班次();
                frm.ShowDialog();

                string code = frm.ScheduleCode;

                for (int i = 0; i < lstRow.Count; i++)
                {
                    DataGridViewCellCollection cells = dataGridView1.Rows[lstRow[i]].Cells;

                    for (int j = 0; j < dataGridView1.Columns.Count; j++)
                    {
                        DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns[j] as DataGridViewComboBoxColumn;

                        if (dataGridView1.Columns[j].CellType.Name == "DataGridViewComboBoxCell")
                        {
                            cells[j].Value = code;
                        }
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请先添加人员！");
            }
        }

        private void 批量排班ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            同班toolStripButton_Click(sender, e);
        }
    }
}
