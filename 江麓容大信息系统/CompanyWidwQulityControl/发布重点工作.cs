using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using Service_Peripheral_CompanyQuality;
using UniversalControlLibrary;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class 发布重点工作 : Form
    {
        IFocalWork _Service_FocalWork = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        public 发布重点工作()
        {
            InitializeComponent();
            txtTaskName.Focus();
        }

        private void txtDutyUser_OnCompleteSearch()
        {
            if (txtDutyUser.DataResult == null)
            {
                return;
            }

            txtDutyUser.Text = txtDutyUser.DataResult["姓名"].ToString();
            txtDutyUser.Tag = txtDutyUser.DataResult["工号"].ToString();
        }

        private void 发布重点工作_Load(object sender, EventArgs e)
        {
            customDataGridView1.DataSource = _Service_FocalWork.GetTable_FocalWork();
        }

        Bus_FocalWork GetInfo()
        {
            Bus_FocalWork result = new Bus_FocalWork();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtDutyUser.Text))
            {
                throw new Exception("请选择【责任人】");
            }
            else
            {
                result.DutyUser = txtDutyUser.Tag.ToString();
            }

            if (GeneralFunction.IsNullOrEmpty(txtTaskName.Text))
            {
                throw new Exception("请填写【重点工作】");
            }
            else
            {
                result.TaskName = txtTaskName.Text;
                result.F_Id = txtTaskName.Tag == null ? Guid.NewGuid().ToString() : txtTaskName.Tag.ToString();
            }

            if (dtpStartDate.Value > dtpEndDate.Value)
            {
                throw new Exception("【启动时间】必须小于【完成时间】");
            }
            else
            {
                result.EndDate = dtpEndDate.Value;
                result.StartDate = dtpStartDate.Value;
            }

            if (GeneralFunction.IsNullOrEmpty(txtExpectedGoal.Text))
            {
                throw new Exception("请填写【预期目标】");
            }
            else
            {
                result.ExpectedGoal = txtExpectedGoal.Text;
            }

            if (GeneralFunction.IsNullOrEmpty(txtTaskDescription.Text))
            {
                throw new Exception("请填写【工作描述】");
            }
            else
            {
                result.TaskDescription = txtTaskDescription.Text;
            }

            return result;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Bus_FocalWork focalWork = GetInfo();
                focalWork.F_Id = Guid.NewGuid().ToString();

                _Service_FocalWork.SaveFocalWork(focalWork);
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_FocalWork();
                ClearContrl();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            try
            {
                _Service_FocalWork.SaveFocalWork(GetInfo());
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_FocalWork();
                ClearContrl();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (customDataGridView1.CurrentRow == null)
                {
                    return;
                }

                _Service_FocalWork.DeleteFocalWork(customDataGridView1.CurrentRow.Cells["F_Id"].Value.ToString());
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_FocalWork();
                ClearContrl();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void ClearContrl()
        {
            txtDutyUser.Text = "";
            txtDutyUser.Tag = null;

            txtTaskName.Text = "";
            txtTaskName.Tag = null;
            txtTaskDescription.Text = "";
            txtExpectedGoal.Text = "";

            dtpEndDate.Value = ServerTime.Time;
            dtpStartDate.Value = ServerTime.Time;
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            txtTaskName.Tag = customDataGridView1.CurrentRow.Cells["F_Id"].Value;

            Bus_FocalWork focalWork =
                _Service_FocalWork.GetSingle_FocalWork(txtTaskName.Tag.ToString());

            View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(focalWork.DutyUser);

            txtDutyUser.Text = personnel.姓名;
            txtDutyUser.Tag = personnel.工号;

            txtTaskDescription.Text = focalWork.TaskDescription;
            txtExpectedGoal.Text = focalWork.ExpectedGoal;
            txtTaskName.Text = focalWork.TaskName;
            dtpStartDate.Value = (DateTime)focalWork.StartDate;
            dtpEndDate.Value = (DateTime)focalWork.EndDate;
        }

        private void customDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            设置关键节点 frm = new 设置关键节点(txtTaskName.Tag.ToString());
            frm.ShowDialog();
        }
    }
}
