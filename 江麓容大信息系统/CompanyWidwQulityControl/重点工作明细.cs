using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using ServerModule;
using GlobalObject;
using Service_Peripheral_CompanyQuality;
using FlowControlService;

namespace Form_Peripheral_CompanyQuality
{
    public partial class 重点工作明细 : CustomFlowForm
    {        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Bus_FocalWork_MonthlyProgress _Lnq_BillInfo = new Bus_FocalWork_MonthlyProgress();

        List<Bus_FocalWork_MonthlyProgress_Content> _List_Content = new List<Bus_FocalWork_MonthlyProgress_Content>();

        List<Bus_FocalWork_MonthlyProgress_KeyPoint> _List_KeyPoint = new List<Bus_FocalWork_MonthlyProgress_KeyPoint>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IFocalWork _Service_FocalWork = Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer _Service_Flow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 重点工作明细()
        {
            InitializeComponent();

            for (int i = 2011; i <= ServerTime.Time.Year; i++)
            {
                cmbYear.Items.Add(i.ToString());
            }
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.重点工作.ToString(), _Service_FocalWork);
                _Lnq_BillInfo = _Service_FocalWork.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            if (_Lnq_BillInfo != null)
            {
                txtBillNo.Text = _Lnq_BillInfo.BillNo;

                cmbMonth.Text = _Lnq_BillInfo.YearMonth.Substring(4, 2);
                cmbYear.Text = _Lnq_BillInfo.YearMonth.Substring(0, 4);

                _List_Content = _Service_FocalWork.GetList_ProgressContent(_Lnq_BillInfo.BillNo);
                _List_KeyPoint = _Service_FocalWork.GetList_ProgressKeyPoint(_Lnq_BillInfo.BillNo);

                cmbMonth.Enabled = false;
                cmbYear.Enabled = false;
                SetFocalWorkRadioButton();

                if (flowLayoutPanel1.Controls.Count > 0)
                {
                    RadioButton rb1 = flowLayoutPanel1.Controls[0] as RadioButton;
                    rb1.Checked = true;
                }
            }
            else
            {
                _Lnq_BillInfo = new Bus_FocalWork_MonthlyProgress();
                txtBillNo.Text = this.FlowInfo_BillNo;
                _Lnq_BillInfo.BillNo = txtBillNo.Text;
                _Lnq_BillInfo.CreateUser = BasicInfo.LoginID;

                cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
                cmbYear.Text = ServerTime.Time.Year.ToString();
            }
        }

        void SetFocalWorkRadioButton()
        {
            if (GeneralFunction.IsNullOrEmpty(cmbYear.Text) || GeneralFunction.IsNullOrEmpty(cmbMonth.Text))
            {
                return;
            }

            if (_List_Content == null || _List_Content.Count == 0)
            {
                _Lnq_BillInfo.YearMonth = cmbYear.Text + cmbMonth.Text;
                _List_Content = _Service_FocalWork.GetList_ProgressContent(_Lnq_BillInfo);
                _List_KeyPoint = _Service_FocalWork.GetList_ProgressKeyPoint(_Lnq_BillInfo, _List_Content);
            }

            flowLayoutPanel1.Controls.Clear();

            foreach (Bus_FocalWork_MonthlyProgress_Content content in _List_Content)
            {
                RadioButton rb = new RadioButton();

                Bus_FocalWork focalWork = _Service_FocalWork.GetSingle_FocalWork(content.FocalWorkId);

                rb.AutoSize = true;
                rb.TabStop = true;
                rb.Name = focalWork.F_Id;
                rb.Text = focalWork.TaskName;
                rb.Tag = content;
                rb.CheckedChanged += new EventHandler(rb_CheckedChanged);

                flowLayoutPanel1.Controls.Add(rb);
            }

            if (flowLayoutPanel1.Controls.Count > 0)
            {
                RadioButton rb1 = flowLayoutPanel1.Controls[0] as RadioButton;
                rb1.Checked = true;
            }
        }

        void rb_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;

            if (rb.Checked)
            {
                Bus_FocalWork_MonthlyProgress_Content content = rb.Tag as Bus_FocalWork_MonthlyProgress_Content;
                Bus_FocalWork focalWork = _Service_FocalWork.GetSingle_FocalWork(content.FocalWorkId);

                txtExpectedGoal.Text = focalWork.ExpectedGoal;
                txtTaskDescription.Text = focalWork.TaskDescription;
                txtDutyUser.Text = UniversalFunction.GetPersonnelInfo(focalWork.DutyUser).姓名;
                dtpEndDate.Value = (DateTime)focalWork.EndDate;
                dtpStartDate.Value = (DateTime)focalWork.StartDate;
                btnKeyPoint.Tag = focalWork.F_Id;

                txtProgressContent.Text = content.ProgressContent;
                txtNextPlan.Text = content.NextPlan;

                List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKey = (from a in _List_KeyPoint
                                                                       where a.FocalWorkId == content.FocalWorkId
                                                                       select a).ToList();

                btnSetKeyPoint.Tag = lstKey;

                if (lstKey == null || lstKey.Count() == 0)
                {
                    btnSetKeyPoint.Visible = false;
                }
                else
                {
                    btnSetKeyPoint.Visible = true;
                }

                if (!GeneralFunction.IsNullOrEmpty(content.Evaluate))
                {
                    switch (content.Evaluate)
                    {
                        case "已完成":
                            rbYes.Checked = true;
                            break;
                        case "未完成":
                            rbNo.Checked = true;
                            break;
                        case "延期":
                            rbDelay.Checked = true;
                            break;
                        default:
                            rbDelay.Checked = false;
                            rbNo.Checked = false;
                            rbYes.Checked = false;
                            break;
                    }
                }
                else
                {
                    rbDelay.Checked = false;
                    rbNo.Checked = false;
                    rbYes.Checked = false;
                }
            }
            else
            {
                Bus_FocalWork_MonthlyProgress_Content content = rb.Tag as Bus_FocalWork_MonthlyProgress_Content;

                content.ProgressContent = txtProgressContent.Text;
                content.NextPlan = txtNextPlan.Text;

                if (rbYes.Checked)
                {
                    content.Evaluate = "已完成";
                }
                else if (rbNo.Checked)
                {
                    content.Evaluate = "未完成";
                }
                else if (rbDelay.Checked)
                {
                    content.Evaluate = "延期";
                }
            }
        }

        private void cmbYearMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            SetFocalWorkRadioButton();
        }

        private void btnKeyPoint_Click(object sender, EventArgs e)
        {
            if (btnKeyPoint.Tag == null)
            {
                return;
            }

            重点工作关键节点 frm = new 重点工作关键节点(btnKeyPoint.Tag.ToString());
            frm.ShowDialog();
        }

        private void btnSetKeyPoint_Click(object sender, EventArgs e)
        {
            if (btnSetKeyPoint.Tag == null)
            {
                return;
            }

            重点工作关键节点 frm = new 重点工作关键节点(btnSetKeyPoint.Tag as List<Bus_FocalWork_MonthlyProgress_KeyPoint>);
            frm.ShowDialog();
            btnSetKeyPoint.Tag = frm._List_KeyPoint;
        }

        private bool 重点工作明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                foreach (Control cl in flowLayoutPanel1.Controls)
                {
                    if (cl is RadioButton)
                    {
                        if (!((RadioButton)cl).Checked)
                        {
                            continue;
                        }

                        RadioButton rb = cl as RadioButton;

                        Bus_FocalWork_MonthlyProgress_Content content = rb.Tag as Bus_FocalWork_MonthlyProgress_Content;

                        content.ProgressContent = txtProgressContent.Text;
                        content.NextPlan = txtNextPlan.Text;

                        if (rbYes.Checked)
                        {
                            content.Evaluate = "已完成";
                        }
                        else if (rbNo.Checked)
                        {
                            content.Evaluate = "未完成";
                        }
                        else if (rbDelay.Checked)
                        {
                            content.Evaluate = "延期";
                        }

                        break;
                    }
                }

                if (flowOperationType == CE_FlowOperationType.提交)
                {
                    foreach (Bus_FocalWork_MonthlyProgress_Content content in _List_Content)
                    {
                        Bus_FocalWork focalWork = _Service_FocalWork.GetSingle_FocalWork(content.FocalWorkId);
                        if (GeneralFunction.IsNullOrEmpty(content.Evaluate))
                        {
                            throw new Exception("【"+ focalWork.TaskName +"】,未进行评价");
                        }

                        if (GeneralFunction.IsNullOrEmpty(content.NextPlan))
                        {
                            throw new Exception("【" + focalWork.TaskName + "】,未填写【下月计划】");
                        }

                        if (GeneralFunction.IsNullOrEmpty(content.ProgressContent))
                        {
                            throw new Exception("【" + focalWork.TaskName + "】,未填写【工作进展】");
                        }

                        List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstTempKey = (from a in _List_KeyPoint
                                                                                   where a.BillNo == _Lnq_BillInfo.BillNo
                                                                                   && a.FocalWorkId == content.FocalWorkId
                                                                                   select a).ToList();

                        foreach (Bus_FocalWork_MonthlyProgress_KeyPoint keyPoint in lstTempKey)
                        {
                            if (GeneralFunction.IsNullOrEmpty(keyPoint.Evaluate))
                            {
                                Bus_FocalWork_KeyPoint tempKey = _Service_FocalWork.GetSingle_KeyPoint(keyPoint.KeyPointId);
                                throw new Exception("【" + focalWork.TaskName + "】中的关键节点【" + tempKey.KeyPointName + "】,未进行评价");
                            }
                        }
                    }
                }

                this.FlowInfo_BillNo = _Lnq_BillInfo.BillNo;
                this.ResultInfo = _Lnq_BillInfo;

                this.ResultList = new List<object>();
                this.ResultList.Add(flowOperationType);
                this.ResultList.Add(_List_Content);
                this.ResultList.Add(_List_KeyPoint);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }
    }
}
