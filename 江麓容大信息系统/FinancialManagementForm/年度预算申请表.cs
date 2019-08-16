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
    public partial class 年度预算申请表 : CustomMainForm
    {
        public 年度预算申请表()
            : base(typeof(年度预算申请表明细), GlobalObject.CE_BillTypeEnum.年度预算申请表,
            Service_Economic_Financial.ServerModuleFactory.GetServerModule<Service_Economic_Financial.IBudgetYear>())
        {
            InitializeComponent();
        }

        private bool 年度预算申请表_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IBudgetYear servcieYear = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IBudgetYear>();

            try
            {
                this.OperationType = form.FlowOperationType;
                this.BillNo = form.FlowInfo_BillNo;
                Business_Finance_Budget_Year yearInfo = form.ResultInfo as Business_Finance_Budget_Year;

                DataTable detailTable = form.ResultList[0] as DataTable;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        servcieYear.SaveInfo(detailTable, yearInfo);
                        servcieYear.OperationBusiness(this.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        servcieYear.SaveInfo(detailTable, yearInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!servcieYear.IsExist(this.BillNo))
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
    }
}
