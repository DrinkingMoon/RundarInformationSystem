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
    public partial class 设置关键节点 : Form
    {
        IFocalWork _Service_FocalWork = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        public 设置关键节点(string focalWorkId)
        {
            InitializeComponent();

            customGroupBox2.Tag = focalWorkId;
            txtKeyPointName.Focus();
        }

        private void 设置关键节点_Load(object sender, EventArgs e)
        {
            customDataGridView1.DataSource = _Service_FocalWork.GetTable_KeyPoint(customGroupBox2.Tag.ToString());
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

        Bus_FocalWork_KeyPoint GetInfo()
        {
            Bus_FocalWork_KeyPoint result = new Bus_FocalWork_KeyPoint();

            result.FocalWorkId = customGroupBox2.Tag.ToString();

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtDutyUser.Text))
            {
                throw new Exception("请选择【责任人】");
            }
            else
            {
                result.DutyUser = txtDutyUser.Tag.ToString();
            }

            if (GeneralFunction.IsNullOrEmpty(txtKeyPointName.Text))
            {
                throw new Exception("请填写【关键节点】");
            }
            else
            {
                result.KeyPointName = txtKeyPointName.Text;
                result.F_Id = txtKeyPointName.Tag == null ? Guid.NewGuid().ToString() : txtKeyPointName.Tag.ToString();
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

            return result;
        }

        void ClearContrl()
        {
            txtDutyUser.Text = "";
            txtDutyUser.Tag = null;

            txtKeyPointName.Text = "";
            txtKeyPointName.Tag = null;

            dtpEndDate.Value = ServerTime.Time;
            dtpStartDate.Value = ServerTime.Time;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Bus_FocalWork_KeyPoint keyPoint = GetInfo();
                keyPoint.F_Id = Guid.NewGuid().ToString();

                _Service_FocalWork.SaveKeyPoint(keyPoint);
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_KeyPoint(customGroupBox2.Tag.ToString());
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
                _Service_FocalWork.SaveKeyPoint(GetInfo());
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_KeyPoint(customGroupBox2.Tag.ToString());
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

                _Service_FocalWork.DeleteKeyPoint(customDataGridView1.CurrentRow.Cells["F_Id"].Value.ToString());
                customDataGridView1.DataSource = _Service_FocalWork.GetTable_KeyPoint(customGroupBox2.Tag.ToString());
                ClearContrl();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void customDataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            txtKeyPointName.Tag = customDataGridView1.CurrentRow.Cells["F_Id"].Value;

            Bus_FocalWork_KeyPoint keyPoint =
                _Service_FocalWork.GetSingle_KeyPoint(txtKeyPointName.Tag.ToString());

            View_HR_Personnel personnel = UniversalFunction.GetPersonnelInfo(keyPoint.DutyUser);

            txtDutyUser.Text = personnel.姓名;
            txtDutyUser.Tag = personnel.工号;

            txtKeyPointName.Text = keyPoint.KeyPointName;
            dtpStartDate.Value = (DateTime)keyPoint.StartDate;
            dtpEndDate.Value = (DateTime)keyPoint.EndDate;
        }
    }
}
