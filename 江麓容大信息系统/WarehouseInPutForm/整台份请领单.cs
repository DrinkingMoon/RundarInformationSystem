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
using Service_Manufacture_Storage;
using FlowControlService;

namespace Form_Manufacture_Storage
{
    public partial class 整台份请领单 : CustomMainForm
    {
        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IWholeMachineRequisitionService m_serviceWholeMachine =
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IWholeMachineRequisitionService>();

        public 整台份请领单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
            : base(typeof(整台份请领单明细), GlobalObject.CE_BillTypeEnum.整台份请领单,
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IWholeMachineRequisitionService>())
        {
            InitializeComponent();
            m_authorityFlag = nodeInfo.Authority;
        }

        private void 整台份请领单_Load(object sender, EventArgs e)
        {
            this.ToolStripSeparator_ShowStatus(m_authorityFlag);
        }

        bool 整台份请领单_Form_CommonProcessSubmit(CustomFlowForm form, string advise)
        {
            IWholeMachineRequisitionService m_serviceWholeMachine =
                Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IWholeMachineRequisitionService>();
            try
            {
                Business_WarehouseOutPut_WholeMachineRequisition lnqRequisition =
                    form.ResultList[0] as Business_WarehouseOutPut_WholeMachineRequisition;
                List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> detailInfo =
                    form.ResultList[1] as List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail>;
                List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> storageInfo =
                    form.ResultList[2] as List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID>;

                this.OperationType = GeneralFunction.StringConvertToEnum<CE_FlowOperationType>(form.ResultList[3].ToString());
                this.BillNo = lnqRequisition.BillNo;

                switch (this.OperationType)
                {
                    case CE_FlowOperationType.提交:
                        List<string> listBillNo = new List<string>();
                        m_serviceWholeMachine.SaveInfo(lnqRequisition, detailInfo, storageInfo);
                        m_serviceWholeMachine.AutoFirstMaterialRequisition(lnqRequisition.BillNo, out listBillNo);

                        if (listBillNo != null && listBillNo.Count > 0)
                        {
                            string msg = "";

                            foreach (string billNoTemp in listBillNo)
                            {
                                msg = msg + "【" + billNoTemp + "】,";
                            }

                            msg = msg.Substring(0, msg.Length - 1);

                            MessageDialog.ShowPromptMessage("已生成领料单 " + msg);
                        }

                        break;
                    case CE_FlowOperationType.暂存:
                        m_serviceWholeMachine.SaveInfo(lnqRequisition, detailInfo, storageInfo);
                        break;
                    case CE_FlowOperationType.回退:
                        break;
                    case CE_FlowOperationType.未知:
                        break;
                    default:
                        break;
                }

                if (!m_serviceWholeMachine.IsExist(lnqRequisition.BillNo))
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

        private void btnShortage_Click(object sender, EventArgs e)
        {
            if (this.SelectDataGridView.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择单据");
                return;
            }

            if (this.SelectDataGridView.CurrentRow.Cells["业务状态"].Value.ToString() == CE_CommonBillStatus.单据完成.ToString())
            {
                try
                {
                    List<string> listBillNo = new List<string>();
                    m_serviceWholeMachine.AutoSupplementaryRequisition(this.SelectDataGridView.CurrentRow.Cells["业务编号"].Value.ToString(), out listBillNo);

                    if (listBillNo != null && listBillNo.Count > 0)
                    {
                        string msg = "";

                        foreach (string billNoTemp in listBillNo)
                        {
                            msg = msg + "【" + billNoTemp + "】,";
                        }

                        msg = msg.Substring(0, msg.Length - 1);
                        MessageDialog.ShowPromptMessage("已生成领料单 " + msg);
                    }

                    this.RefreshData(this.tabControl1.SelectedTab);
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowPromptMessage(ex.Message);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("只能操作【业务状态】：【单据完成】的单据");
            }
        }

        private void btnSign_Click(object sender, EventArgs e)
        {
            if (this.SelectDataGridView.CurrentRow == null)
            {
                MessageDialog.ShowPromptMessage("请选择单据");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("此单据将被标记为【已完成全部发料】，您确定要这么做吗？") == DialogResult.Yes)
            {
                m_serviceWholeMachine.SignFinish(this.SelectDataGridView.CurrentRow.Cells["业务编号"].Value.ToString());
                MessageDialog.ShowPromptMessage("已标记为【已完成全部发料】");
                this.RefreshData(this.tabControl1.SelectedTab);
            }
        }

        private void btnSelect_Click(object sender, System.EventArgs e)
        {
            m_serviceWholeMachine.SetStatus(this.SelectDataGridView as DataGridView);
        }
    }
}
