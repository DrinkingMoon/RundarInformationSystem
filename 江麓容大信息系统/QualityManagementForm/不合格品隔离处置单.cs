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
    public partial class 不合格品隔离处置单 : CustomMainForm
    {
        /// <summary>
        /// 服务组件
        /// </summary>
        IRejectIsolationService m_serviceIsolation = Service_Quality_QC.ServerModuleFactory.GetServerModule<IRejectIsolationService>();

        public 不合格品隔离处置单()
            : base(typeof(不合格品隔离处置单明细), GlobalObject.CE_BillTypeEnum.不合格品隔离处置单,
                Service_Quality_QC.ServerModuleFactory.GetServerModule<IRejectIsolationService>())
        {
            InitializeComponent();
            this.Form_CommonProcessSubmit += new FormCommonProcess.FormSubmit(frm_CommonProcessSubmit);
        }

        private void btnBatchAdd_Click(object sender, EventArgs e)
        {
            多批隔离设置界面 frm = new 多批隔离设置界面();
            frm.ShowDialog();
            this.btnRefresh_Click(sender, e);
        }

        private void btnAddMsg_Click(object sender, System.EventArgs e)
        {
            if (tabControl1.SelectedTab.Text != "已处理")
            {
                throw new Exception("请选择【已处理】选项卡");
            }

            if (this.dataGridView2.CurrentRow == null)
            {
                throw new Exception("请选择需要操作的业务单据");
            }

            不合格品处置单补充信息 frm = new 不合格品处置单补充信息(this.dataGridView2.CurrentRow.Cells["业务编号"].Value.ToString());
            frm.ShowDialog();
        }

        bool frm_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            try
            {
                Business_QualityManagement_Isolation lnqInPut = form.ResultList[0] as Business_QualityManagement_Isolation;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[1].ToString());
                this.BillNo = lnqInPut.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        m_serviceIsolation.SaveInfo(lnqInPut);
                        m_serviceIsolation.FinishBill(lnqInPut.BillNo);
                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceIsolation.SaveInfo(lnqInPut);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceIsolation.IsExist(lnqInPut.BillNo))
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
