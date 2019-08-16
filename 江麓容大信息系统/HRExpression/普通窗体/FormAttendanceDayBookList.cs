using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Service_Peripheral_HR;
using PlatformManagement;
using Expression;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 考勤流水子界面
    /// </summary>
    public partial class FormAttendanceDayBookList : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 考勤流水
        /// </summary>
        string m_dayBookID;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 考勤机导入的人员考勤明细表操作类
        /// </summary>
        IAttendanceMachineServer m_attendanceMachineServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceMachineServer>();

        /// <summary>
        /// 人员考勤流水账操作类
        /// </summary>
        IAttendanceDaybookServer m_dayBookServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceDaybookServer>();

        public FormAttendanceDayBookList(string dayBookID)
        {
            InitializeComponent();

            m_dayBookID = dayBookID;
            RefreshDataGridView();

            cmbResultType.DataSource = m_attendanceMachineServer.GetExceptionType().Select(k => k.TypeName).ToList();
        }

         /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshDataGridView()
        {
            if (!m_dayBookServer.GetAllDayBook(out m_queryResult, out error))
            {
                MessageDialog.ShowPromptMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            DataTable dtTemp = dt.Clone();

            DataRow[] dr = dt.Select("流水号=" + m_dayBookID);

            for (int i = 0; i < dr.Length; i++)
            {
                dtTemp.ImportRow(dr[i]);
            }

            if (dtTemp.Rows.Count > 0)
            {
                dataGridView1.DataSource = dtTemp;
            }

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

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtWrokID.Text = dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString();
            txtName.Text = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();
            txtBillNo.Text = dataGridView1.CurrentRow.Cells["关联单号"].Value.ToString();
            txtPunchTime.Text = dataGridView1.CurrentRow.Cells["打卡时间"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            numHours.Value = dataGridView1.CurrentRow.Cells["小时数"].Value==DBNull.Value?0:Convert.ToDecimal(dataGridView1.CurrentRow.Cells["小时数"].Value);
            cmbResultType.Text = dataGridView1.CurrentRow.Cells["考勤结论类别"].Value.ToString();
            cbIsSubsidize.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否算餐补"].Value);
        }
    }
}
