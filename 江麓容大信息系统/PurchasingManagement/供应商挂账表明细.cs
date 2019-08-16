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
    public partial class 供应商挂账表明细 : Form
    {
        IAccountOperation _Service_Account = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IAccountOperation>();

        string _YearMonth;

        string _Provider;

        public 供应商挂账表明细(string yearMonth, string provider, string billStatus)
        {
            InitializeComponent();

            _YearMonth = yearMonth;
            _Provider = provider;

            customDataGridView1.DataSource = _Service_Account.GetTable_Detail(provider, yearMonth);

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()) && billStatus == SelectType.待审核.ToString())
            {
                btnOK.Text = "审核";
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()) && billStatus == SelectType.待复核.ToString())
            {
                btnOK.Text = "复核";
            }
            else if ((BasicInfo.ListRoles.Contains(CE_RoleEnum.采购账务管理员.ToString()) || BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
                && (billStatus == SelectType.可打印.ToString() || billStatus == SelectType.已打印.ToString()))
            {
                btnOK.Text = "打印";
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                SelectType selectType;

                switch (btnOK.Text)
                {
                    case "审核":
                        selectType = SelectType.待审核;
                        break;
                    case "复核":
                        selectType = SelectType.待复核;
                        break;
                    case "打印":
                        selectType = SelectType.可打印;
                        break;
                    default:
                        selectType = SelectType.全部;
                        break;
                }

                if (selectType != SelectType.全部)
                {
                    if (selectType == SelectType.可打印)
                    {
                        BaseModule_Economic.报表.供应商挂账单 frm =
                            new BaseModule_Economic.报表.供应商挂账单(_YearMonth, _Provider);
                        frm.ShowDialog();
                    }

                    _Service_Account.SubmitList(selectType, _Provider, _YearMonth);
                    MessageDialog.ShowPromptMessage("操作成功");
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }
    }
}
