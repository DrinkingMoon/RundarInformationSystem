using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using ServerModule;
using Expression;
using Service_Peripheral_HR;
using System.Collections;
using System.Threading;
using System.Diagnostics;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 考勤机导入的人员考勤明细表界面
    /// </summary>
    public partial class UserControlAttendanceMachine : Form
    {
        #region 声明变量
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

        /// 排班信息操作类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        /// <summary>
        /// 考勤机导入的人员考勤明细表操作类
        /// </summary>
        IAttendanceMachineServer m_attendanceMachineServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceMachineServer>();

        /// <summary>
        /// 考勤方案操作类
        /// </summary>
        IAttendanceSchemeServer m_attendanceSchemeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceSchemeServer>();

        #endregion

        public UserControlAttendanceMachine(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
            RefreshDataGridView();
        }

        private void UserControlAttendanceMachine_Load(object sender, EventArgs e)
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
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_attendanceMachineServer.GetAllInfo();

            dataGridView1.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["单据号"].Visible = false;
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
        }

        private void 导入toolStripButton_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                if (CheckTable(dtTemp))
                {
                    #region 分析排班人员的打卡时间

                    List<HR_AttendanceMachineDataList> lstMachineDate = new List<HR_AttendanceMachineDataList>();

                    for (int i = 0; i < dtTemp.Rows.Count; i++)
                    {
                        string workID = "";
                        string recordTime = "";
                        string beforTime = "";
                        string afterTime = "";

                        if (dtTemp.Columns.Contains("工号"))
                        {
                            workID = dtTemp.Rows[i]["工号"].ToString();

                            if (workID == "")
                            {
                                continue;
                            }
                        }
                        else
                        {
                            if (GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["日期"])
                                || GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["时间"]))
                            {
                                continue;
                            }

                            if (dtTemp.Rows[i]["登记号码"].ToString() == "")
                            {
                                continue;
                            }

                            string str = m_attendanceMachineServer.GetCardIDWorkIDMapping(dtTemp.Rows[i]["登记号码"].ToString());

                            if (str.Equals("0"))
                            {
                                //MessageDialog.ShowPromptMessage("【" + dtTemp.Rows[i]["登记号码"].ToString() + "】登记号码没有在映射表中！");
                                //return;
                                continue;
                            }

                            workID = str;
                        }

                        DateTime date = Convert.ToDateTime(dtTemp.Rows[i]["日期"].ToString());
                        DateTime beforTimeTemp = date.AddDays(-1);
                        DateTime afterTimeTemp = Convert.ToDateTime(dtTemp.Rows[i]["日期"].ToString()).AddDays(1);
                        string[] recordTimeTemp = dtTemp.Rows[i]["时间"].ToString().Split(' ');

                        HR_AttendanceSetting attendanceSet = m_attendanceSchemeServer.GetAttendanceSettingByWorkID(workID);
                        string[] schemeCode = attendanceSet.SchemeCode.Split(' ');
                        string mode = m_attendanceSchemeServer.GetAttendanceSchemeByCode(schemeCode[0]).AttendanceMode;

                        if (mode.Contains("自然月排班考勤"))
                        {
                            DataTable dtWorkSchedule = m_workSchedulingServer.GetWorkSchedulingByWorkIDAndDate(workID, Convert.ToDateTime(dtTemp.Rows[i]["日期"].ToString()));

                            if (dtWorkSchedule != null && dtWorkSchedule.Rows.Count > 0)
                            {
                                if (!Convert.ToBoolean(dtWorkSchedule.Rows[0]["isholiday"].ToString()))
                                {
                                    string beginTime = Convert.ToDateTime(dtWorkSchedule.Rows[0]["beginTime"].ToString()).ToShortTimeString();
                                    string punchInBegin = Convert.ToDateTime(dtWorkSchedule.Rows[0]["punchInBeginTime"].ToString()).ToShortTimeString();
                                    string punchInEnd = Convert.ToDateTime(dtWorkSchedule.Rows[0]["punchInEndTime"].ToString()).ToShortTimeString();
                                    string punchOutBegin = Convert.ToDateTime(dtWorkSchedule.Rows[0]["punchOutBeginTime"].ToString()).ToShortTimeString();
                                    string punchOutEnd = Convert.ToDateTime(dtWorkSchedule.Rows[0]["punchOutEndTime"].ToString()).ToShortTimeString();

                                    if ((Convert.ToDateTime(beginTime) - Convert.ToDateTime(punchInBegin)).Hours < 0)
                                    {
                                        if (i != dtTemp.Rows.Count - 1)
                                        {
                                            string str = dtTemp.Rows[i + 1]["时间"].ToString();

                                            if (!str.Equals("0"))
                                            {
                                                string[] timeTemp = str.Split(' ');

                                                if (recordTimeTemp.Length == 0)
                                                {
                                                    beforTime = afterTimeTemp.Year + "年" + afterTimeTemp.Month + "月" + afterTimeTemp.Day + "日" + timeTemp[0];
                                                    afterTime = timeTemp[1];
                                                }
                                                else
                                                {
                                                    afterTime = timeTemp[0];
                                                }
                                            }
                                        }
                                    }

                                    if (Convert.ToInt32(dtWorkSchedule.Rows[0]["endOffSetDays"].ToString()) > 0)
                                    {
                                        afterTimeTemp = Convert.ToDateTime(dtTemp.Rows[i]["日期"].ToString()).AddDays(
                                            Convert.ToInt32(dtWorkSchedule.Rows[0]["endOffSetDays"].ToString()));

                                        string str = dtTemp.Rows[i + Convert.ToInt32(dtWorkSchedule.Rows[0]["endOffSetDays"].ToString())]["时间"].ToString();

                                        if (!str.Equals("0"))
                                        {
                                            string[] timeTemp = str.Split(' ');

                                            if (timeTemp[0] != "")
                                            {
                                                if (Convert.ToDateTime(timeTemp[0]) < Convert.ToDateTime(punchOutEnd))
                                                {
                                                    afterTime = timeTemp[0];
                                                }
                                            }
                                        }
                                    }

                                    if (beforTime != "")
                                    {
                                        if (!recordTime.Contains(beforTime))
                                        {
                                            recordTime = beforTime + " ";
                                        }
                                    }

                                    for (int j = 0; j < recordTimeTemp.Length; j++)
                                    {
                                        if (recordTimeTemp[j] != "")
                                        {
                                            if (j != 0)
                                            {
                                                if (recordTimeTemp[j - 1] == recordTimeTemp[j])
                                                {
                                                    continue;
                                                }

                                                if (Convert.ToDateTime(recordTimeTemp[j]).Hour - Convert.ToDateTime(recordTimeTemp[j - 1]).Hour == 0
                                                    &&
                                                    Convert.ToDateTime(recordTimeTemp[j]).Minute - Convert.ToDateTime(recordTimeTemp[j - 1]).Minute < 5)
                                                {
                                                    continue;
                                                }
                                            }

                                            recordTime += date.Year + "年" + date.Month + "月" + date.Day + "日" + recordTimeTemp[j] + " ";
                                        }
                                    }

                                    if (afterTime != "")
                                    {
                                        string strAfter = afterTimeTemp.Year + "年" + afterTimeTemp.Month + "月" + afterTimeTemp.Day + "日" + afterTime;

                                        if (!recordTime.Contains(strAfter))
                                        {
                                            recordTime += afterTimeTemp.Year + "年" + afterTimeTemp.Month + "月" + afterTimeTemp.Day + "日" + afterTime + " ";
                                        }
                                    }
                                }
                                else
                                {
                                    for (int j = 0; j < recordTimeTemp.Length; j++)
                                    {
                                        if (recordTimeTemp[j] != "")
                                        {
                                            if (j != 0)
                                            {
                                                if (recordTimeTemp[j - 1] == recordTimeTemp[j])
                                                {
                                                    continue;
                                                }

                                                if (Convert.ToDateTime(recordTimeTemp[j]).Hour - Convert.ToDateTime(recordTimeTemp[j - 1]).Hour == 0
                                                    &&
                                                    Convert.ToDateTime(recordTimeTemp[j]).Minute - Convert.ToDateTime(recordTimeTemp[j - 1]).Minute < 5)
                                                {
                                                    continue;
                                                }
                                            }

                                            recordTime += date.Year + "年" + date.Month + "月" + date.Day + "日" + recordTimeTemp[j] + " ";
                                        }
                                    }
                                }
                            }
                            else
                            {
                                for (int j = 0; j < recordTimeTemp.Length; j++)
                                {
                                    if (recordTimeTemp[j] != "")
                                    {
                                        if (j != 0)
                                        {
                                            if (recordTimeTemp[j - 1] == recordTimeTemp[j])
                                            {
                                                continue;
                                            }

                                            if (Convert.ToDateTime(recordTimeTemp[j]).Hour - Convert.ToDateTime(recordTimeTemp[j - 1]).Hour == 0
                                                && Convert.ToDateTime(recordTimeTemp[j]).Minute - Convert.ToDateTime(recordTimeTemp[j - 1]).Minute < 5)
                                            {
                                                continue;
                                            }
                                        }

                                        recordTime += date.Year + "年" + date.Month + "月" + date.Day + "日" + recordTimeTemp[j] + " ";
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < recordTimeTemp.Length; j++)
                            {
                                if (recordTimeTemp[j] != "")
                                {
                                    if (j != 0)
                                    {
                                        if (recordTimeTemp[j - 1] == recordTimeTemp[j])
                                        {
                                            continue;
                                        }

                                        if (Convert.ToDateTime(recordTimeTemp[j]).Hour - Convert.ToDateTime(recordTimeTemp[j - 1]).Hour == 0
                                            && Convert.ToDateTime(recordTimeTemp[j]).Minute - Convert.ToDateTime(recordTimeTemp[j - 1]).Minute < 5)
                                        {
                                            continue;
                                        }
                                    }

                                    recordTime += date.Year + "年" + date.Month + "月" + date.Day + "日" + recordTimeTemp[j] + " ";
                                }
                            }
                        }

                        HR_AttendanceMachineDataList machineDataList = new HR_AttendanceMachineDataList();

                        machineDataList.Date = date;
                        machineDataList.RecordTime = recordTime;
                        machineDataList.WorkID = workID;
                        machineDataList.Remark = "";

                        lstMachineDate.Add(machineDataList);
                    }
                    #endregion

                    m_attendanceMachineServer.InsertAttendanceMachine(lstMachineDate);
                    MessageDialog.ShowPromptMessage("导入成功!");
                }

                RefreshDataGridView();

                if (dataGridView1.Rows.Count > 0)
                {
                    dataGridView1.Columns["单据号"].Visible = false;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        /// <summary>
        /// 检查Excel表的数据
        /// </summary>
        /// <param name="dtcheck">表</param>
        /// <returns>返回是否正确</returns>
        private bool CheckTable(DataTable dtcheck)
        {
            if (!dtcheck.Columns.Contains("工号") && !dtcheck.Columns.Contains("登记号码"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【工号】或者是【登记号码】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("时间"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【时间】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("日期"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【日期】信息");
                return false;
            }

            return true;
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlAttendanceMachine_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void btnOK_1_Click(object sender, EventArgs e)
        {
            try
            {
                if (MessageBox.Show("节假日、集体异常、员工方案等所有信息是否都已经设置完成？", "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    IAttendanceAnalysis serviceAnalysis = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceAnalysis>();
                    serviceAnalysis.Analysis_Main(dtpBeginDate.Value.Date, dtpEndDate.Value.Date, null);
                    MessageDialog.ShowPromptMessage("分析完成");
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                if (MessageBox.Show("是否要删除选中的行？", "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
                    {
                        if (!m_attendanceMachineServer.DeleteAttendanceMachineByID(
                            dataGridView1.SelectedRows[i].Cells["单据号"].Value.ToString(), out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                            return;
                        }
                    }

                    RefreshDataGridView();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的一行记录！");
            }
        }

        private void btnSelectAnalyze_Click(object sender, EventArgs e)
        {
            string workID = "";

            try
            {
                IAttendanceAnalysis serviceAnalysis = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceAnalysis>();
                FormSelectPersonnel2 frm = new FormSelectPersonnel2();

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    if (frm.SelectedNotifyPersonnelInfo.UserType == BillFlowMessage_ReceivedUserType.用户.ToString())
                    {
                        foreach (PersonnelBasicInfo personnelInfo in frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList)
                        {
                            workID = personnelInfo.工号;
                            serviceAnalysis.Analysis_Main(dtpBeginDate.Value.Date, dtpEndDate.Value.Date, personnelInfo.工号);
                        }

                        MessageDialog.ShowPromptMessage("分析完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message + " 工号：" + workID);
            }
        }

    }
}
