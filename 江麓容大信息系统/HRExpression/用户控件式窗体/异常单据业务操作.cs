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
    public partial class 异常单据业务操作 : Form
    {
        /// <summary>
        /// 请假操作类
        /// </summary>
        ILeaveServer _serviceLeave = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ILeaveServer>();

        IAttendanceAnalysis _serviceAnalysis = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IAttendanceAnalysis>();

        public 异常单据业务操作()
        {
            InitializeComponent();
            AddPublicMethod(panel2);
            AddPublicMethod(panel3);
        }

        string GetMode(Panel panel)
        {
            string mode = "";

            foreach (Control cl in panel.Controls)
            {
                if (cl is RadioButton)
                {
                    if (((RadioButton)cl).Checked)
                    {
                        mode = cl.Tag.ToString();
                    }
                }
            }

            return mode;
        }

        void AddPublicMethod(Panel panel)
        {
            foreach (Control cl in panel.Controls)
            {
                if (cl is RadioButton)
                {
                    ((RadioButton)cl).CheckedChanged += new EventHandler(RadioButton_CheckedChanged);
                }
            }
        }

        void RadioButton_CheckedChanged(object sender, EventArgs e)
        {
            RefreshDataGridView();

            if (((RadioButton)sender).Parent == panel2 && ((RadioButton)sender).Checked)
            {
                AddComboxItems(GeneralFunction.StringConvertToEnum<CE_HR_AttendanceExceptionType>(
                    ((RadioButton)sender).Text.Substring(0, 2)));
            }
        }

        void AddComboxItems(CE_HR_AttendanceExceptionType type)
        {
            cmb_BusinessType.Items.Clear();
            cmb_BusinessType.DataSource = null;

            if (type == CE_HR_AttendanceExceptionType.请假)
            {
                DataTable dt = _serviceLeave.GetLeaveTypeByCode(null);

                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        cmb_BusinessType.Items.Add(dt.Rows[i]["请假类别"].ToString());
                    }
                }
            }
            else if (type == CE_HR_AttendanceExceptionType.加班)
            {
                cmb_BusinessType.Items.Add("加班调休_公司内");
                cmb_BusinessType.Items.Add("加班补助_公司内");
                cmb_BusinessType.Items.Add("加班调休_公司外");
                cmb_BusinessType.Items.Add("加班补助_公司外");
            }

            cmb_BusinessType.SelectedIndex = -1;
        }

        void RefreshDataGridView()
        {
            string type = GetMode(panel2);
            string operationMode = GetMode(panel3);

            if (type.Trim().Length == 0 || operationMode.Trim().Length == 0)
            {
                //MessageDialog.ShowPromptMessage("请选择【单据类型】、【操作类型】");
                return;
            }

            customDataGridView1.DataSource = _serviceAnalysis.GetBusinessInfo_Exception(
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_HR_AttendanceExceptionType>(type),
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_OperatorMode>(operationMode));

            userControlDataLocalizer1.Init(customDataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, customDataGridView1.Name, BasicInfo.LoginID));
        }

        bool CheckData()
        {
            if (dtp_EndTime.Value <= dtp_BeginTime.Value)
            {
                MessageDialog.ShowPromptMessage("【开始时间】需小于【结束时间】");
                return false;
            }

            //if (dtp_EndTime.Value <= _serviceAnalysis.GetLastDateTime())
            //{
            //    MessageDialog.ShowPromptMessage("【结束日期】需小于" + _serviceAnalysis.GetLastDateTime().ToLongDateString());
            //    return false;
            //}

            if (num_Hours.Value == 0 
                ||( GlobalObject.GeneralFunction.GetDecimalPointAfter(num_Hours.Value) != 0.5
                && GlobalObject.GeneralFunction.GetDecimalPointAfter(num_Hours.Value) != 0))
            {
                MessageDialog.ShowPromptMessage("请自行计算【小时数】，要求小数位为 0 或者0.5");
                return false;
            }

            if (cmb_BusinessType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【业务类型】");
                return false;
            }

            if (txtContent.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【执行内容】");
                return false;
            }

            return true;
        }

        void OperationBusiness()
        {
            string type = GetMode(panel2);
            string mode = GetMode(panel3);

            if (type.Trim().Length == 0 || mode.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【单据类型】、【操作类型】");
                return;
            }
            CE_HR_AttendanceExceptionType billType = 
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_HR_AttendanceExceptionType>(type);
            CE_OperatorMode operationMode =
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_OperatorMode>(mode);

            if (!CheckData())
            {
                return;
            }

            List<object> lstInfo = new List<object>();
            List<PersonnelBasicInfo> lstPersonnel = new List<PersonnelBasicInfo>();

            lstInfo.Add(ServerTime.ConvertToDateTime(dtp_BeginTime.Value));
            lstInfo.Add(ServerTime.ConvertToDateTime(dtp_EndTime.Value));
            lstInfo.Add(cmb_BusinessType.Text);
            lstInfo.Add(txtContent.Text.Trim());
            lstInfo.Add(num_Hours.Value);

            if (operationMode == CE_OperatorMode.添加)
            {
                lstInfo.Add(null);

                MessageDialog.ShowPromptMessage(string.Format("请设置需要【{0}】的人员", billType.ToString()));
                FormSelectPersonnel2 frm = new FormSelectPersonnel2();

                if (frm.ShowDialog() != DialogResult.OK)
                {
                    return;
                }
                else
                {
                    if (frm.SelectedNotifyPersonnelInfo.UserType != BillFlowMessage_ReceivedUserType.用户.ToString())
                    {
                        MessageDialog.ShowPromptMessage("请选择【用户】");
                        return;
                    }
                    else
                    {
                        lstPersonnel = frm.SelectedNotifyPersonnelInfo.PersonnelBasicInfoList;
                    }
                }

                _serviceAnalysis.Operation_Exception(billType, operationMode, lstInfo, lstPersonnel);
            }
            else if (operationMode == CE_OperatorMode.修改)
            {
                lstInfo.Add((int)customDataGridView1.CurrentRow.Cells["单据号"].Value);

                if (MessageDialog.ShowEnquiryMessage("你确定要【" + operationMode.ToString() + "】?") == DialogResult.No)
                {
                    return;
                }

                _serviceAnalysis.Operation_Exception(billType, operationMode, lstInfo, lstPersonnel);
            }
            else if (operationMode == CE_OperatorMode.删除)
            {
                if (MessageDialog.ShowEnquiryMessage("你确定要【" + operationMode.ToString() + "】?") == DialogResult.No)
                {
                    return;
                }

                if (customDataGridView1.SelectedRows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择需要【删除】的记录");
                    return;
                }

                foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
                {
                    lstInfo.Add((int)dgvr.Cells["单据号"].Value);
                    lstPersonnel = new List<PersonnelBasicInfo>();
                    PersonnelBasicInfo personnel = new PersonnelBasicInfo();

                    personnel.工号 = dgvr.Cells["执行人"].Value.ToString();
                    lstPersonnel.Add(personnel);

                    _serviceAnalysis.Operation_Exception(billType, operationMode, lstInfo, lstPersonnel);
                }
            }

            MessageDialog.ShowPromptMessage(string.Format("【{0}单{1}成功】", billType.ToString(), operationMode.ToString()));
            RefreshDataGridView();
        }

        private void btSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                OperationBusiness();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void customDataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            if (GlobalObject.GeneralFunction.StringConvertToEnum<CE_OperatorMode>(GetMode(panel3)) == CE_OperatorMode.添加)
            {
                dtp_BeginTime.Value = ServerTime.Time;
                dtp_EndTime.Value = ServerTime.Time;

                cmb_BusinessType.SelectedIndex = -1;
                num_Hours.Value = 0;
                txtContent.Text = "";
            }
            else
            {
                dtp_BeginTime.Value = Convert.ToDateTime(customDataGridView1.CurrentRow.Cells["执行开始时间"].Value);
                dtp_EndTime.Value = Convert.ToDateTime(customDataGridView1.CurrentRow.Cells["执行结束时间"].Value);
                cmb_BusinessType.Text = customDataGridView1.CurrentRow.Cells["业务类型"].Value.ToString();
                txtContent.Text = customDataGridView1.CurrentRow.Cells["执行内容"].Value.ToString();
                num_Hours.Value = Convert.ToDecimal( customDataGridView1.CurrentRow.Cells["小时数"].Value);
            }
        }
    }
}
