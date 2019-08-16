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
    public partial class 重点工作详细信息 : Form
    {
        Bus_FocalWork _Lnq_FocalWork = new Bus_FocalWork();
        DataTable _Table_KeyPoint = new DataTable();
        IFocalWork _Service_FocalWork = 
            Service_Peripheral_CompanyQuality.ServerModuleFactory.GetServerModule<IFocalWork>();

        string _YearMonth = null;

        public 重点工作详细信息(string keyValue, string yearMonth)
        {
            InitializeComponent();

            _Lnq_FocalWork = _Service_FocalWork.GetSingle_FocalWork(keyValue);
            _Table_KeyPoint = _Service_FocalWork.GetTable_KeyPoint(keyValue);
            _YearMonth = yearMonth;
        }

        private void 重点工作详细信息_Load(object sender, EventArgs e)
        {
            if (_Lnq_FocalWork == null)
            {
                return;
            }

            labelTitle.Text = _Lnq_FocalWork.TaskName;

            txtTaskDescription.Text = _Lnq_FocalWork.TaskDescription;
            txtExpectedGoal.Text = _Lnq_FocalWork.ExpectedGoal;
            dtpStartDate.Value = (DateTime)_Lnq_FocalWork.StartDate;
            dtpEndDate.Value = (DateTime)_Lnq_FocalWork.EndDate;
            txtDutyUser.Text = UniversalFunction.GetPersonnelInfo(_Lnq_FocalWork.DutyUser).姓名;

            lbStatus.Text = _Lnq_FocalWork.TaskStatus;

            if (lbStatus.Text == "延期")
            {
                lbStatus.ForeColor = Color.Yellow;
            }
            else if (lbStatus.Text == "待启动")
            {
                lbStatus.ForeColor = Color.Black;
            }
            else if(lbStatus.Text == "已完成")
            {
                lbStatus.ForeColor = Color.Black;
            }

            DateTime startDate = Convert.ToDateTime(dtpStartDate.Value.Year.ToString() + "-" + dtpStartDate.Value.Month.ToString("D2") + "-1");
            DateTime endDate = lbStatus.Text == "已完成" ? _Service_FocalWork.GetEndDate(_Lnq_FocalWork.F_Id) :
                Convert.ToDateTime(ServerTime.Time.Year.ToString() + "-" + ServerTime.Time.Month.ToString("D2") + "-1");

            while (startDate <= endDate)
            {
                Label lbKeyPoint = new Label();
                lbKeyPoint.AutoSize = true;
                lbKeyPoint.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));

                Bus_FocalWork_MonthlyProgress_Content content = _Service_FocalWork.GetSingle_Content(_Lnq_FocalWork.F_Id,
                    startDate.Year.ToString() + startDate.Month.ToString("D2"));

                lbKeyPoint.Tag = content;
                lbKeyPoint.Text = startDate.Year.ToString() + "年" + startDate.Month.ToString("D2") + "月\r\n    ";

                if (content == null || GeneralFunction.IsNullOrEmpty(content.Evaluate))
                {
                    lbKeyPoint.Text += "？";
                }
                else
                {
                    switch (content.Evaluate)
                    {
                        case "已完成":
                            lbKeyPoint.Text += "○";
                            lbKeyPoint.BackColor = Color.Green;
                            break;
                        case "未完成":
                            lbKeyPoint.Text += "×";
                            lbKeyPoint.BackColor = Color.Red;
                            break;
                        case "延期":
                            lbKeyPoint.Text += "△";
                            lbKeyPoint.BackColor = Color.Yellow;
                            break;
                        default:
                            break;
                    }
                }

                lbKeyPoint.Click += new EventHandler(lbKeyPoint_Click);

                flowLayoutPanel1.Controls.Add(lbKeyPoint);

                if (startDate < endDate)
                {
                    Label lbConnect = new Label();
                    lbConnect.AutoSize = true;
                    lbConnect.Text = "——→";
                    lbConnect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
                    flowLayoutPanel1.Controls.Add(lbConnect);
                }

                startDate = startDate.AddMonths(1);
            }

            if (!GeneralFunction.IsNullOrEmpty(_YearMonth))
            {
                gbTaskName.Visible = false;

                foreach (Control cl in flowLayoutPanel1.Controls)
                {
                    Label lb = cl as Label;

                    if (lb.Text.Length > 8)
                    {
                        if (lb.Text.Substring(0, 4) == _YearMonth.Substring(0, 4) 
                            && lb.Text.Substring(5, 2) == _YearMonth.Substring(4, 2))
                        {
                            lbKeyPoint_Click(lb, null);
                            break;
                        }
                    }
                }
            }
        }

        void lbKeyPoint_Click(object sender, EventArgs e)
        {
            if (((Label)sender).Tag == null)
            {
                rbDelay.Checked = false;
                rbNo.Checked = false;
                rbYes.Checked = false;
                txtProgressContent.Text = "";
                txtNextPlan.Text = "";
                btnConnectKeyPoint.Tag = null;
                btnConnectKeyPoint.Visible = false;
                return;
            }

            Bus_FocalWork_MonthlyProgress_Content content = ((Label)sender).Tag as Bus_FocalWork_MonthlyProgress_Content;

            txtProgressContent.Text = content.ProgressContent;
            txtNextPlan.Text = content.NextPlan;

            List<Bus_FocalWork_MonthlyProgress_KeyPoint> lstKeyTemp = (from a in _Service_FocalWork.GetList_ProgressKeyPoint(content.BillNo)
                                                                       where a.FocalWorkId == content.FocalWorkId
                                                                       select a).ToList();
            btnConnectKeyPoint.Tag = lstKeyTemp;
            if (lstKeyTemp == null || lstKeyTemp.Count() == 0)
            {
                btnConnectKeyPoint.Visible = false;
            }
            else
            {
                btnConnectKeyPoint.Visible = true;
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
        }

        private void btnKeyPoint_Click(object sender, EventArgs e)
        {
            if (_Lnq_FocalWork.F_Id == null)
            {
                MessageDialog.ShowPromptMessage("无【关键节点】");
                return;
            }

            重点工作关键节点 frm = new 重点工作关键节点(_Lnq_FocalWork.F_Id);
            frm.ShowDialog();
        }

        private void btnConnectKeyPoint_Click(object sender, EventArgs e)
        {
            if (btnConnectKeyPoint.Tag == null)
            {
                MessageDialog.ShowPromptMessage("当前节点无【相关联的关键节点】");
                return;
            }

            重点工作关键节点 frm = new 重点工作关键节点(btnConnectKeyPoint.Tag as List<Bus_FocalWork_MonthlyProgress_KeyPoint>);
            frm.ShowDialog();
        }
    }
}
