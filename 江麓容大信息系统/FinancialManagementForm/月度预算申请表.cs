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
using FlowControlService;
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    public partial class 月度预算申请表 : CustomMainForm
    {
        public 月度预算申请表()
            : base(typeof(月度预算申请表明细), GlobalObject.CE_BillTypeEnum.月度预算申请表,
            Service_Economic_Financial.ServerModuleFactory.GetServerModule<Service_Economic_Financial.IBudgetMonth>())
        {
            InitializeComponent();
        }

        private bool 月度预算申请表_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IBudgetMonth servcieMonth = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBudgetMonth>();

            try
            {
                this.OperationType = form.FlowOperationType;
                this.BillNo = form.FlowInfo_BillNo;
                Business_Finance_Budget_Month monthInfo = form.ResultInfo as Business_Finance_Budget_Month;

                DataTable detailTable = form.ResultList[0] as DataTable;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        servcieMonth.SaveInfo(detailTable, monthInfo);
                        servcieMonth.OperationBusiness(detailTable, this.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        servcieMonth.SaveInfo(detailTable, monthInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!servcieMonth.IsExist(this.BillNo))
                {
                    MessageDialog.ShowPromptMessage("数据为空，保存失败，如需退出，请直接X掉界面");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void 月度预算申请表_Form_btnPrint(object sender, EventArgs e)
        {
            IBudgetMonth serviceBudgetMonth = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBudgetMonth>();

            if (tabControl1.SelectedTab.Text == "全部")
            {
                MessageDialog.ShowPromptMessage("请选择【已处理】或【待处理】中的记录");
                return;
            }

            string billNo = "";

            if (tabControl1.SelectedTab.Text == "已处理")
            {
                if (dataGridView2.Rows.Count > 0)
                {
                    billNo = dataGridView2.CurrentRow.Cells["业务编号"].Value.ToString();
                }
            }
            else if (tabControl1.SelectedTab.Text == "待处理")
            {
                if (dataGridView1.Rows.Count > 0)
                {
                    billNo = dataGridView1.CurrentRow.Cells["业务编号"].Value.ToString();
                }

            }

            Business_Finance_Budget_Month billInfo = serviceBudgetMonth.GetBillSingleInfo(billNo);
            DataTable tempTable = serviceBudgetMonth.GetDetailInfo(billInfo);

            ExcelHelperP.DataTableToExcel(saveFileDialog1, tempTable, null);
        }
    }
}
