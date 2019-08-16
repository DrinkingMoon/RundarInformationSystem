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
using UniversalControlLibrary;
using Service_Quality_QC;
using FlowControlService;
namespace Form_Quality_QC
{
    public partial class 不合格品处置单补充信息 : Form
    {
        string _BillNo;

        IRejectIsolationService _Service_Isolation = Service_Quality_QC.ServerModuleFactory.GetServerModule<IRejectIsolationService>();

        public 不合格品处置单补充信息(string billNo)
        {
            InitializeComponent();
            _BillNo = billNo;
        }

        private void 不合格品处置单补充信息_Load(object sender, EventArgs e)
        {
            lbText.Text = "为单据号：【" + _BillNo + "】添加补充信息";
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtMessage.Text.Trim().Length == 0)
                {
                    throw new Exception("请填写【补充信息】");
                }

                _Service_Isolation.AddSupplementMessage(_BillNo, txtMessage.Text.Trim());
                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

    }
}
