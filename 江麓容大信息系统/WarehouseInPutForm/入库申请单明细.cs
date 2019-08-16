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
using Service_Manufacture_Storage;
using GlobalObject;
using FlowControlService;

namespace Form_Manufacture_Storage
{
    public partial class 入库申请单明细 : CustomFlowForm
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strError = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_WarehouseInPut_Requisition m_lnqBillInfo = new Business_WarehouseInPut_Requisition();

        public Business_WarehouseInPut_Requisition LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_WarehouseInPut_RequisitionDetail> m_listViewDetail = new List<View_Business_WarehouseInPut_RequisitionDetail>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IRequisitionService_InPut m_serviceRequistion = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IRequisitionService_InPut>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 入库申请单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.入库申请单.ToString(), m_serviceRequistion);
                m_lnqBillInfo = m_serviceRequistion.GetSingleBillInfo(this.FlowInfo_BillNo);

                this.关联业务.m_OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(关联业务_m_OnCompleteSearch);
                this.图号型号.m_OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(图号型号_m_OnCompleteSearch);
                this.批次号.m_OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(批次号_m_OnCompleteSearch);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void 关联业务_m_OnCompleteSearch()
        {
            DataRow drTemp = this.关联业务.DataResult;

            if (drTemp != null)
            {
                switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>( cmbBillType.Text))
                {
                    case CE_InPutBusinessType.生产采购:
                    case CE_InPutBusinessType.普通采购:
                    case CE_InPutBusinessType.委外采购:
                    case CE_InPutBusinessType.样品采购:

                        ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["批次号"]).ReadOnly = true;
                        ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["供应商"]).ReadOnly = true;

                        customDataGridView1.CurrentRow.Cells["供应商"].Value = drTemp["供货单位"];
                        ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["图号型号"]).m_EndSql =
                            " and 序号 in ( select GoodsID from B_OrderFormGoods where OrderFormNumber = '" 
                            + drTemp["订单号"].ToString() + "')";

                        break;
                    default:
                        ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["批次号"]).ReadOnly = false;
                        ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["供应商"]).ReadOnly = false;
                        break;
                }
            }
        }

        void 批次号_m_OnCompleteSearch()
        {
            DataRow drTemp = this.批次号.DataResult;

            if (drTemp != null)
            {
                customDataGridView1.CurrentRow.Cells["批次号"].Value = drTemp["批次号"];
                customDataGridView1.CurrentRow.Cells["供应商"].Value = drTemp["供应商编码"];

                ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["供应商"]).ReadOnly = true;
            }
        }

        void 图号型号_m_OnCompleteSearch()
        {
            DataRow drTemp = this.图号型号.DataResult;

            if (drTemp != null)
            {
                customDataGridView1.CurrentRow.Cells["物品名称"].Value = drTemp["物品名称"];
                customDataGridView1.CurrentRow.Cells["规格"].Value = drTemp["规格"];
                customDataGridView1.CurrentRow.Cells["物品ID"].Value = drTemp["序号"];
                customDataGridView1.CurrentRow.Cells["单位"].Value = drTemp["单位"];

                ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["批次号"]).m_EndSql =
                    " and 物品ID = " + (int)drTemp["序号"];

                IBasicGoodsServer goodsService = SCM_Level01_ServerFactory.GetServerModule<IBasicGoodsServer>();

                F_GoodsAttributeRecord record = goodsService.GetGoodsAttirbuteRecord(Convert.ToInt32(drTemp["序号"]), 
                    Convert.ToInt32(CE_GoodsAttributeName.来料须依据检验结果入库));

                if (record != null)
                {
                    customDataGridView1.CurrentRow.Cells["检验报告"].Value = Convert.ToBoolean(record.AttributeValue);
                }
            }
        }

        void SetInfo()
        {
            List<string> listType = GlobalObject.GeneralFunction.GetEumnList(typeof(CE_InPutBusinessType));

            cmbBillType.DataSource = listType;
            cmbBillType.SelectedIndex = -1;

            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtRemark.Text = m_lnqBillInfo.Remark;
                txtApplyingDepartment.Tag = m_lnqBillInfo.ApplyingDepartment;
                txtApplyingDepartment.Text = UniversalFunction.GetDeptName(m_lnqBillInfo.ApplyingDepartment);

                txtTypeDetail.Text = m_lnqBillInfo.BillTypeDetail;

                cmbBillType.Text = m_lnqBillInfo.BillType;
                chbIsConfirmArrival.Checked = m_lnqBillInfo.IsConfirmArrival;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_WarehouseInPut_Requisition();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;

                txtApplyingDepartment.Tag = BasicInfo.DeptCode;
                txtApplyingDepartment.Text = BasicInfo.DeptName;
            }

            m_listViewDetail = m_serviceRequistion.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail);
        }

        void RefreshDataGridView(List<View_Business_WarehouseInPut_RequisitionDetail> source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_WarehouseInPut_RequisitionDetail item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.关联业务, item.图号型号, item.物品名称, item.规格, item.批次号,
                        item.供应商,item.数量, item.单位, item.检验报告,item.备注, item.物品ID, item.单据号});
                }

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(cmbBillType.Text))
                    {
                        case CE_InPutBusinessType.生产采购:
                        case CE_InPutBusinessType.普通采购:
                        case CE_InPutBusinessType.委外采购:
                        case CE_InPutBusinessType.样品采购:
                            ((DataGridViewTextBoxShowCell)dgvr.Cells["图号型号"]).m_EndSql =
                                " and 序号 in ( select GoodsID from B_OrderFormGoods where OrderFormNumber = '" 
                                + dgvr.Cells["关联业务"].Value.ToString() + "')";
                            break;
                        default:
                            break;
                    }

                    ((DataGridViewTextBoxShowCell)dgvr.Cells["批次号"]).m_EndSql =
                        " and 物品ID = " + (int)dgvr.Cells["物品ID"].Value;
                }
            }
        }

        bool CheckData()
        {
            if (txtApplyingDepartment.Tag == null || txtApplyingDepartment.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择申请部门");
                return false;
            }

            if (cmbBillType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择业务类型");
                return false;
            }

            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return false;
            }

            return true;
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbBillType.Text.Trim().Length > 0)
            {
                DataGridViewRow dr = new DataGridViewRow();
                customDataGridView1.Rows.Add(dr);
                cmbBillType.Enabled = customDataGridView1.Rows.Count > 0 ? false : true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择业务类型");
            }
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in customDataGridView1.SelectedRows)
                {
                    customDataGridView1.Rows.Remove(dr);
                }

                cmbBillType.Enabled = customDataGridView1.Rows.Count > 0 ? false : true;
            }
        }

        private void cmbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBillType.Text.Trim().Length > 0)
            {
                switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(cmbBillType.Text))
                {
                    case CE_InPutBusinessType.生产采购:
                    case CE_InPutBusinessType.普通采购:
                    case CE_InPutBusinessType.委外采购:
                    case CE_InPutBusinessType.样品采购:
                        this.关联业务.ReadOnly = false;
                        this.关联业务.FindItem = TextBoxShow.FindType.订单信息;
                        break;
                    default:
                        this.关联业务.ReadOnly = true;
                        break;
                }
            }
        }

        private void btnBenchCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBillType.Text.Trim().Length > 0)
                {
                    switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(cmbBillType.Text))
                    {
                        case CE_InPutBusinessType.生产采购:
                        case CE_InPutBusinessType.普通采购:
                        case CE_InPutBusinessType.委外采购:
                        case CE_InPutBusinessType.样品采购:

                            IQueryable<View_B_OrderFormInfo> findOrderFormInfo;
                            IOrderFormInfoServer orderFormServer = ServerModule.ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

                            if (!orderFormServer.GetAllOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID, out findOrderFormInfo, out m_strError))
                            {
                                MessageDialog.ShowErrorMessage(m_strError);
                                return;
                            }

                            DataTable dataSource = GlobalObject.GeneralFunction.ConvertToDataTable<View_B_OrderFormInfo>(findOrderFormInfo);
                            FormDataTableCheck frm = new FormDataTableCheck(dataSource);

                            frm._BlDateTimeControlShow = false;

                            if (frm.ShowDialog() == DialogResult.OK)
                            {
                                List<string> listTemp = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "订单号");

                                m_listViewDetail = m_serviceRequistion.GetListViewDetail_OrderForm(txtBillNo.Text, listTemp);
                                RefreshDataGridView(m_listViewDetail);
                            }

                            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                            {
                                ((DataGridViewTextBoxShowCell)dgvr.Cells["图号型号"]).m_EndSql =
                                    " and 序号 in ( select GoodsID from B_OrderFormGoods where OrderFormNumber = '"
                                    + dgvr.Cells["关联业务"].Value.ToString() + "')";
                                ((DataGridViewTextBoxShowCell)dgvr.Cells["批次号"]).ReadOnly = true;
                                ((DataGridViewTextBoxShowCell)dgvr.Cells["供应商"]).ReadOnly = true;
                            }

                            break;
                        default:
                            break;
                    }

                    cmbBillType.Enabled = customDataGridView1.Rows.Count > 0 ? false : true;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请选择【业务类型】");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (!CheckData())
                {
                    return false;
                }

                m_lnqBillInfo = new Business_WarehouseInPut_Requisition();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.BillType = cmbBillType.Text;
                m_lnqBillInfo.BillTypeDetail = txtTypeDetail.Text;
                m_lnqBillInfo.ApplyingDepartment = txtApplyingDepartment.Tag.ToString();
                m_lnqBillInfo.IsConfirmArrival = chbIsConfirmArrival.Checked;
                m_lnqBillInfo.Remark = txtRemark.Text;

                List<View_Business_WarehouseInPut_RequisitionDetail> listTemp = new List<View_Business_WarehouseInPut_RequisitionDetail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_WarehouseInPut_RequisitionDetail detailTemp = new View_Business_WarehouseInPut_RequisitionDetail();

                    detailTemp.备注 = dgvr.Cells["备注"].Value == null ? "" : dgvr.Cells["备注"].Value.ToString();
                    detailTemp.单据号 = txtBillNo.Text;
                    detailTemp.单位 = dgvr.Cells["单位"].Value == null ? "" : dgvr.Cells["单位"].Value.ToString();
                    detailTemp.供应商 = dgvr.Cells["供应商"].Value == null ? "" : dgvr.Cells["供应商"].Value.ToString();
                    detailTemp.关联业务 = dgvr.Cells["关联业务"].Value == null ? "" : dgvr.Cells["关联业务"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.检验报告 = Convert.ToBoolean(dgvr.Cells["检验报告"].Value);
                    detailTemp.批次号 = dgvr.Cells["批次号"].Value == null ? "" : dgvr.Cells["批次号"].Value.ToString();
                    detailTemp.数量 = Convert.ToDecimal(dgvr.Cells["数量"].Value);
                    detailTemp.图号型号 = dgvr.Cells["图号型号"].Value == null ? "" : dgvr.Cells["图号型号"].Value.ToString();
                    detailTemp.物品ID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    detailTemp.物品名称 = dgvr.Cells["物品名称"].Value == null ? "" : dgvr.Cells["物品名称"].Value.ToString();

                    switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(m_lnqBillInfo.BillType))
                    {
                        case CE_InPutBusinessType.生产采购:
                        case CE_InPutBusinessType.普通采购:
                        case CE_InPutBusinessType.委外采购:
                        case CE_InPutBusinessType.样品采购:
                        case CE_InPutBusinessType.自制件入库:
                            break;
                        case CE_InPutBusinessType.领料退库:
                        case CE_InPutBusinessType.营销入库:
                        case CE_InPutBusinessType.营销退库:
                            IProductCodeServer serverProductCode = ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>();
                            if (!serverProductCode.IsFitCount(detailTemp.物品ID, Convert.ToInt32(detailTemp.数量), detailTemp.单据号))
                            {
                                MessageBox.Show("请对CVT/TCU设置流水编号,并保证产品数量与流水编号数一致", "提示");
                                return false;
                            }
                            break;
                        default:
                            break;
                    }

                    listTemp.Add(detailTemp);
                }

                this.FlowInfo_BillNo = txtBillNo.Text;
                this.ResultInfo = listTemp;

                this.ResultList = new List<object>();

                this.ResultList.Add(m_lnqBillInfo);
                this.ResultList.Add(flowOperationType);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void txtApplyingDepartment_OnCompleteSearch()
        {
            txtApplyingDepartment.Tag = txtApplyingDepartment.DataResult["部门编码"];
        }

        private void btnTypeDetail_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBillType.Text.Trim().Length > 0)
                {
                    switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(cmbBillType.Text))
                    {
                        case CE_InPutBusinessType.生产采购:
                        case CE_InPutBusinessType.普通采购:
                        case CE_InPutBusinessType.委外采购:
                        case CE_InPutBusinessType.样品采购:
                        case CE_InPutBusinessType.自制件入库:
                            break;
                        case CE_InPutBusinessType.领料退库:

                            领料用途 form = new 领料用途();

                            if (form.ShowDialog() == DialogResult.OK)
                            {
                                txtTypeDetail.Tag = form.SelectedData.Code;
                                txtTypeDetail.Text = form.SelectedData.Purpose;
                            }

                            break;
                        case CE_InPutBusinessType.营销入库:
                            break;
                        case CE_InPutBusinessType.营销退库:
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请选择【业务类型】");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }

        private void customDataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == -1 && e.RowIndex >= 0 
                && customDataGridView1.CurrentRow.Cells["物品ID"].Value != null 
                && customDataGridView1.CurrentRow.Cells["物品ID"].Value.ToString().Length > 0)
            {
                BarCodeInfo barCode = new BarCodeInfo();

                barCode.BatchNo = customDataGridView1.CurrentRow.Cells["批次号"].Value == null ? "" :
                    customDataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
                barCode.Count = customDataGridView1.CurrentRow.Cells["数量"].Value == null ? 0 :
                    Convert.ToDecimal( customDataGridView1.CurrentRow.Cells["数量"].Value);
                barCode.GoodsID = Convert.ToInt32( customDataGridView1.CurrentRow.Cells["物品ID"].Value);

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(barCode.GoodsID);

                barCode.GoodsName = goodsInfo.物品名称;
                barCode.GoodsCode = goodsInfo.图号型号;
                barCode.Spec = goodsInfo.规格;

                CE_BusinessType tempType = CE_BusinessType.库房业务;

                Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                WS_WorkShopCode tempWSCode = serverWSBasic.GetWorkShopCodeInfo(txtApplyingDepartment.Tag.ToString());

                Dictionary<string, string> tempDic = new Dictionary<string, string>();

                CE_InPutBusinessType inPutType =
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(cmbBillType.Text);
                CE_MarketingType marketType = 
                    GlobalObject.EnumOperation.InPutBusinessTypeConvertToMarketingType(inPutType);
                CE_SubsidiaryOperationType subsdiaryType = 
                    GlobalObject.EnumOperation.InPutBusinessTypeConvertToSubsidiaryOperationType(inPutType);

                tempDic.Add("", marketType.ToString());

                if (tempWSCode != null)
                {
                    tempType = CE_BusinessType.综合业务;
                    tempDic.Add(tempWSCode.WSCode, subsdiaryType.ToString());
                }

                产品编号 frm = new 产品编号(barCode, tempType, txtBillNo.Text, (lbBillStatus.Text != CE_CommonBillStatus.单据完成.ToString()), tempDic);

                if (frm.ShowDialog() == DialogResult.OK)
                {
                    customDataGridView1.Rows[e.RowIndex].Cells["数量"].Value = frm.IntCount;
                }
            }
        }
    }
}
