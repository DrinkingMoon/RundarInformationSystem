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
using Service_Economic_Purchase;
using FlowControlService;


namespace Form_Economic_Purchase
{
    public partial class 供应商挂账表 : Form
    {
        IAccountOperation _Service_AccountOperation = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IAccountOperation>();

        List<Bus_PurchasingMG_Account> _List_Account = new List<Bus_PurchasingMG_Account>();

        public 供应商挂账表()
        {
            InitializeComponent();

            for (int i = 0; i < 20; i++)
            {
                cmbYear.Items.Add((2012 + i).ToString());
            }

            List<string> lstStatus = GlobalObject.GeneralFunction.GetEumnList(typeof(SelectType));

            cmbStatus.DataSource = lstStatus;

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
            cmbStatus.Text = SelectType.全部.ToString();
        }

        void DataGridShow()
        {
            customDataGridView1.DataSource = _Service_AccountOperation.GetTable(cmbYear.Text + cmbMonth.Text, 
                GeneralFunction.StringConvertToEnum<SelectType>(cmbStatus.Text));

            userControlDataLocalizer1.Init(this.customDataGridView1, this.Name, null);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            DataGridShow();

            SelectType slType = GeneralFunction.StringConvertToEnum<SelectType>(cmbStatus.Text);

            if (slType == SelectType.可打印 || slType == SelectType.已打印)
            {
                btnPrint.Visible = true;
            }
            else
            {
                btnPrint.Visible = false;
            }
        }

        private void customDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (customDataGridView1.CurrentRow == null)
            {
                return;
            }

            供应商挂账表明细 frm = new 供应商挂账表明细(customDataGridView1.CurrentRow.Cells["挂账年月"].Value.ToString(),
                customDataGridView1.CurrentRow.Cells["供应商简码"].Value.ToString(),
                customDataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());

            if (frm.ShowDialog() == DialogResult.OK)
            {
                DataGridShow();
            }
        }

        private void 供应商挂账表_Load(object sender, EventArgs e)
        {
            string[] fileNameAccount = new string[] { "挂账年月", "图号型号", "物品名称", "规格", "供应商", 
                "挂账方式", "协议单价", "税率", "单位", "上月未挂", "本月入库", "本月应挂" , "本月未挂",
                "转结数量", "到票数量", "到票金额", "审核人", "审核时间", "复审人", "复审时间", "最后打印人", "最后打印时间"};

            FormConditionFind fcfAccount = new FormConditionFind(this.挂账表查询, fileNameAccount, this.挂账表查询.Name, this.Name + this.挂账表查询.Name);

            fcfAccount.TopLevel = false;
            fcfAccount.Dock = DockStyle.Fill;
            fcfAccount.FormBorderStyle = FormBorderStyle.None;
            fcfAccount.Show();

            fcfAccount.Parent = this.挂账表查询;

            string[] fileNameAccountDetail = new string[] { "挂账年月", "供应商", "单据号", "图号型号", "物品名称", "规格", "批次号", "数量" };

            FormConditionFind fcfAccountDetail = new FormConditionFind(this.挂账表明细查询, fileNameAccountDetail, this.挂账表明细查询.Name, this.Name + this.挂账表明细查询.Name);

            fcfAccountDetail.TopLevel = false;
            fcfAccountDetail.Dock = DockStyle.Fill;
            fcfAccountDetail.FormBorderStyle = FormBorderStyle.None;
            fcfAccountDetail.Show();

            fcfAccountDetail.Parent = this.挂账表明细查询;
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.SelectedRows)
            {
                BaseModule_Economic.报表.供应商挂账单 frm =
                    new BaseModule_Economic.报表.供应商挂账单(dgvr.Cells["挂账年月"].Value.ToString(), dgvr.Cells["供应商简码"].Value.ToString());
                frm.ShowDialog();
            }

            btnSelect_Click(null, null);
        }
    }
}
