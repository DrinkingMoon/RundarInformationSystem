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
using Service_Manufacture_WorkShop;

namespace Form_Manufacture_WorkShop 
{
    public partial class 车间批次管理变更明细 : CustomFlowForm
    {
        IFlowServer _ServiceFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();
        IBatchNoChange _ServiceChange = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IBatchNoChange>();

        public 车间批次管理变更明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                txtBillNo.Text = this.FlowInfo_BillNo;
                lbBillStatus.Text = _ServiceFlow.GetNowBillStatus(this.FlowInfo_BillNo);
                this.图号型号.m_OnCompleteSearch += new DelegateCollection.NonArgumentHandle(图号型号_m_OnCompleteSearch);
                this.管理方式.DataSource = GlobalObject.GeneralFunction.GetEumnList(typeof(CE_WorkShop_BatchNoChangeType));

                Business_WorkShop_BatchNoChange changeInfo = _ServiceChange.GetSingleInfo(this.FlowInfo_BillNo);

                if (changeInfo != null)
                {
                    txtReason.Text = changeInfo.Reason;
                }

                List<View_Business_WorkShop_BatchNoChangeDetail> lstDetail = _ServiceChange.GetListDetail(this.FlowInfo_BillNo);
                RefreshDataGridView(lstDetail);
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        void 图号型号_m_OnCompleteSearch()
        {
            DataRow drTemp = this.图号型号.DataResult;

            if (drTemp != null)
            {
                foreach (DataGridViewRow dr in customDataGridView1.Rows)
                {
                    if (dr.Cells["物品ID"].Value == null)
                    {
                        continue;
                    }

                    if (dr.Cells["物品ID"].Value.ToString() == drTemp["序号"].ToString())
                    {
                        MessageDialog.ShowPromptMessage("存在相同项，不能添加重复零件");
                        customDataGridView1.CurrentRow.Cells["图号型号"].Value = "";
                        customDataGridView1.CurrentRow.Cells["物品名称"].Value = "";
                        customDataGridView1.CurrentRow.Cells["规格"].Value = "";
                        customDataGridView1.CurrentRow.Cells["物品ID"].Value = 0;
                        return;
                    }
                }

                customDataGridView1.CurrentRow.Cells["图号型号"].Value = drTemp["图号型号"];
                customDataGridView1.CurrentRow.Cells["物品名称"].Value = drTemp["物品名称"];
                customDataGridView1.CurrentRow.Cells["规格"].Value = drTemp["规格"];
                customDataGridView1.CurrentRow.Cells["物品ID"].Value = drTemp["序号"];
            }
        }

        void RefreshDataGridView(List<View_Business_WorkShop_BatchNoChangeDetail> source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_WorkShop_BatchNoChangeDetail item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.单据号, item.物品ID, item.图号型号, item.物品名称, item.规格, item.管理方式 });
                }

                userControlDataLocalizer1.Init(customDataGridView1, customDataGridView1.Name, null);
            }
        }

        List<View_Business_WorkShop_BatchNoChangeDetail> GetDetail()
        {
            List<View_Business_WorkShop_BatchNoChangeDetail> result = new List<View_Business_WorkShop_BatchNoChangeDetail>();

            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_WorkShop_BatchNoChangeDetail detail = new View_Business_WorkShop_BatchNoChangeDetail();

                    detail.单据号 = txtBillNo.Text;
                    detail.管理方式 = dgvr.Cells["管理方式"].Value.ToString();
                    detail.规格 = dgvr.Cells["规格"].Value.ToString();
                    detail.图号型号 = dgvr.Cells["图号型号"].Value.ToString();
                    detail.物品ID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    detail.物品名称 = dgvr.Cells["物品名称"].Value.ToString();

                    result.Add(detail);
                }
            }

            return result;
        }

        private bool 车间批次管理变更明细_PanelGetDataInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (txtReason.Text.Trim().Length == 0)
                {
                    throw new Exception("请填写【变更原因】");
                }

                if (customDataGridView1.Rows.Count == 0)
                {
                    throw new Exception("请录入【明细】信息");
                }

                Business_WorkShop_BatchNoChange changeInfo = new Business_WorkShop_BatchNoChange();

                changeInfo.BillNo = txtBillNo.Text;
                changeInfo.Reason = txtReason.Text;

                this.ResultInfo = changeInfo;
                this.FlowOperationType = flowOperationType;

                this.ResultList.Add(GetDetail());
                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void customContextMenuStrip_Edit1__InputEvent(DataTable dtTemp)
        {
            List<View_Business_WorkShop_BatchNoChangeDetail> lstDetail = new List<View_Business_WorkShop_BatchNoChangeDetail>();
            List<string> lstType = GlobalObject.GeneralFunction.GetEumnList(typeof(CE_WorkShop_BatchNoChangeType));

            foreach (DataRow dr in dtTemp.Rows)
            {
                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(dr["图号型号"].ToString().Trim(),
                    dr["物品名称"].ToString().Trim(), dr["规格"].ToString().Trim());

                if (goodsInfo == null)
                {
                    throw new Exception(string.Format("【图号型号】：{0} ，【物品名称】：{1}，【规格】：{2} 获取物品信息失败"));
                }

                if (!lstType.Contains(dr["管理方式"].ToString().Trim()))
                {
                    throw new Exception(UniversalFunction.GetGoodsMessage(goodsInfo.序号) + " 【管理方式】不存在");
                }

                View_Business_WorkShop_BatchNoChangeDetail detail = new View_Business_WorkShop_BatchNoChangeDetail();

                detail.单据号 = txtBillNo.Text;
                detail.管理方式 = dr["管理方式"].ToString().Trim();
                detail.规格 = goodsInfo.规格;
                detail.图号型号 = goodsInfo.图号型号;
                detail.物品ID = goodsInfo.序号;
                detail.物品名称 = goodsInfo.物品名称;

                lstDetail.Add(detail);
            }

            RefreshDataGridView(lstDetail);
        }
    }
}
