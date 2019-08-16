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
    public partial class 供应商应付账款 : CustomMainForm
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 服务组件
        /// </summary>
        IAccountOperation _Service_Account = Service_Economic_Purchase.ServerModuleFactory.GetServerModule<IAccountOperation>();

        IFlowServer _Service_Flow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 供应商应付账款(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
            : base(typeof(供应商应付账款明细), GlobalObject.CE_BillTypeEnum.供应商应付账款,
            Service_Economic_Purchase.ServerModuleFactory.GetServerModule<Service_Economic_Purchase.IAccountOperation>())
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
        }

        private bool 供应商应付账款_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Bus_PurchasingMG_AccountBill lnqBillInfo = form.ResultInfo as Bus_PurchasingMG_AccountBill;

                List<Bus_PurchasingMG_AccountBill_Invoice> invoiceInfo =
                    form.ResultList[0] as List<Bus_PurchasingMG_AccountBill_Invoice>;
                List<View_Bus_PurchasingMG_AccountBill_Detail> detailInfo =
                    form.ResultList[1] as List<View_Bus_PurchasingMG_AccountBill_Detail>;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[2].ToString());
                this.BillNo = lnqBillInfo.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:

                        string billStatus = _Service_Flow.GetNextBillStatus(this.BillNo);

                        if (billStatus == CE_CommonBillStatus.单据完成.ToString())
                        {
                            string operationUser = 
                                _Service_Flow.GetBusinessOperationInfo(this.BillNo).
                                Where(k => k.FlowID == 103).
                                OrderByDescending(k => k.OperationTime).
                                First().OperationPersonnel;

                            if (operationUser == BasicInfo.LoginID)
                            {
                                throw new Exception("【新建人】与【审核人】不能为同一个人");
                            }
                        }

                        _Service_Account.SaveInfo(lnqBillInfo, invoiceInfo, detailInfo);
                        _Service_Account.OperatarUnFlowBusiness(lnqBillInfo.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        _Service_Account.SaveInfo(lnqBillInfo, invoiceInfo, detailInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!_Service_Account.IsExist(lnqBillInfo.BillNo))
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

        void btnReAuditing_Click(object sender, System.EventArgs e)
        {
            try
            {
                if (tabControl1.SelectedTab.Text != "全部")
                {
                    throw new Exception("请选择【全部选项卡】");
                }

                if (this.dataGridView3.CurrentRow == null)
                {
                    throw new Exception("请选择需要删除的业务单据");
                }

                string billNo = this.dataGridView3.CurrentRow.Cells["业务编号"].Value.ToString();

                if (MessageDialog.ShowEnquiryMessage("【强制删除】可能会导致账务错误， 是否仍要删除【" + billNo + "】单据？") == DialogResult.No)
                {
                    return;
                }

                _Service_Account.DeleteInfo_Force(billNo);
                MessageDialog.ShowPromptMessage("【强制删除】成功");
                //foreach (Control cl in tabControl1.SelectedTab.Controls)
                //{
                //    if (cl is CustomDataGridView)
                //    {
                //        if (((CustomDataGridView)cl).CurrentRow == null)
                //        {
                //            throw new Exception("请选择需要设置账单日期的单据");
                //        }

                //        string billNo = ((CustomDataGridView)cl).CurrentRow.Cells["业务编号"].Value.ToString();

                //        FormDateTime frm = new FormDateTime("设置账单日期");
                //        if (frm.ShowDialog() == DialogResult.OK)
                //        {
                //            _Service_Account.SetFinanceTime(billNo, frm.SelectedTime);
                //            MessageDialog.ShowPromptMessage("设置成功");
                //            return;
                //        }
                //        else
                //        {
                //            return;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 供应商应付账款_Load(object sender, EventArgs e)
        {
            this.ToolStripSeparator_ShowStatus(m_authorityFlag);
        }
    }
}
