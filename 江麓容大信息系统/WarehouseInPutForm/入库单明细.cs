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
    public partial class 入库单明细 : CustomFlowForm
    {
        /// <summary>
        /// 编号服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModule.ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_WarehouseInPut_InPut m_lnqBillInfo = new Business_WarehouseInPut_InPut();

        public Business_WarehouseInPut_InPut LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_WarehouseInPut_InPutDetail> m_listViewDetail = new List<View_Business_WarehouseInPut_InPutDetail>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IInPutService m_serviceInPut = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IInPutService>();

        /// <summary>
        /// 入库申请单服务组件
        /// </summary>
        IRequisitionService_InPut m_serviceRequistion = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IRequisitionService_InPut>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        public 入库单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.入库单.ToString(), m_serviceInPut);
                m_lnqBillInfo = m_serviceInPut.GetSingleBillInfo(this.FlowInfo_BillNo);


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
                ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["图号型号"]).m_EndSql =
                    " and 序号 in ( select GoodsID from Business_WarehouseInPut_RequisitionDetail where BillNo = '"
                    + drTemp["单据号"].ToString() + "')";
                ((DataGridViewTextBoxShowCell)customDataGridView1.CurrentRow.Cells["批次号"]).ReadOnly = true;
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

                DataTable tempTable = m_serviceRequistion.GetListViewDetial(customDataGridView1.CurrentRow.Cells["关联业务"].Value.ToString(), 
                    Convert.ToInt32(drTemp["序号"]), null, null);

                if (tempTable != null && tempTable.Rows.Count > 0)
                {
                    customDataGridView1.CurrentRow.Cells["供应商"].Value = tempTable.Rows[0]["供应商"].ToString();
                    customDataGridView1.CurrentRow.Cells["数量"].Value = Convert.ToDecimal(tempTable.Rows[0]["数量"]);
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

        void SetInfo()
        {
            List<string> listType = GlobalObject.GeneralFunction.GetEumnList(typeof(CE_InPutBusinessType));

            cmbBillType.DataSource = listType;
            cmbBillType.SelectedIndex = -1;

            DataTable dt = UniversalFunction.GetStorageTb();

            cmbStorage.DataSource = dt;
            cmbStorage.DisplayMember = "StorageName";
            cmbStorage.ValueMember = "StorageID";
            cmbStorage.SelectedIndex = -1;

            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtRemark.Text = m_lnqBillInfo.Remark;
                txtApplyingDepartment.Tag = m_lnqBillInfo.ApplyingDepartment;
                txtApplyingDepartment.Text = UniversalFunction.GetDeptName(m_lnqBillInfo.ApplyingDepartment);


                cmbTypeDetail.Text = m_lnqBillInfo.BillTypeDetail;

                cmbBillType.Text = m_lnqBillInfo.BillType;
                cmbStorage.SelectedValue = m_lnqBillInfo.StorageID;
                cmbStorage.Text = UniversalFunction.GetStorageName(m_lnqBillInfo.StorageID);
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_WarehouseInPut_InPut();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }
            ;
            m_listViewDetail = m_serviceInPut.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail);
        }

        void RefreshDataGridView(List<View_Business_WarehouseInPut_InPutDetail> source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_WarehouseInPut_InPutDetail item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.关联业务, item.图号型号, item.物品名称, item.规格, item.批次号,
                        item.供应商,item.数量, item.单位, item.单价,item.备注, item.物品ID, item.单据号});
                }

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    ((DataGridViewTextBoxShowCell)dgvr.Cells["关联业务"]).m_EndSql =
                        " and 单据类型 = '" + cmbBillType.Text + "' and 申请部门编码 = '" + txtApplyingDepartment.Tag.ToString() + "'";

                    if (dgvr.Cells["关联业务"].Value.ToString().Trim().Length > 0)
                    {
                        ((DataGridViewTextBoxShowCell)dgvr.Cells["图号型号"]).m_EndSql =
                            " and 序号 in ( select GoodsID from Business_WarehouseInPut_RequisitionDetail where BillNo = '" 
                            + dgvr.Cells["关联业务"].Value.ToString() + "')";
                    }

                    ((DataGridViewTextBoxShowCell)dgvr.Cells["批次号"]).m_EndSql =
                        " and 物品ID = " + (int)dgvr.Cells["物品ID"].Value;
                }

                List<Business_WarehouseInPut_Requisition> listRequisition =
                    m_serviceRequistion.GetListBillInfo(source.Select(k => k.关联业务).Distinct().ToList());

                cmbTypeDetail.DataSource = listRequisition.Select(k => k.BillTypeDetail).Distinct().ToList();
            }
        }

        bool CheckData()
        {
            if (txtApplyingDepartment.Tag == null || txtApplyingDepartment.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择申请部门");
                return false;
            }

            if (cmbStorage.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择入库库房");
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
                if (txtApplyingDepartment.Tag == null || txtApplyingDepartment.Tag.ToString().Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择申请部门");
                    return;
                }

                if (cmbStorage.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请选择入库库房");
                    return;
                }
                DataGridViewRow dr = new DataGridViewRow();
                customDataGridView1.Rows.Add(dr);
                cmbBillType.Enabled = customDataGridView1.Rows.Count > 0 ? false : true;

                ((DataGridViewTextBoxShowCell)customDataGridView1.Rows[customDataGridView1.Rows.Count - 1].Cells["关联业务"]).m_EndSql =
                    " and 单据类型 = '" + cmbBillType.Text + "' and 申请部门编码 = '" + txtApplyingDepartment.Tag.ToString() + "'";

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

        private void btnBenchCreate_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbBillType.Text.Trim().Length > 0)
                {
                    DataTable dataSource = m_serviceInPut.GetReferenceInfo(cmbBillType.Text,
                        txtApplyingDepartment.Tag == null ? null : txtApplyingDepartment.Tag.ToString(), chbIsRepeat.Checked);
                    FormDataTableCheck frm = new FormDataTableCheck(dataSource);
                    frm._BlDateTimeControlShow = false;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        DataTable tempTable = DataSetHelper.SelectDistinct("", frm._DtResult, "申请部门");

                        if (tempTable.Rows.Count > 1)
                        {
                            MessageDialog.ShowPromptMessage("选择的业务编号的申请部门不一致");
                            return;
                        }
                        else
                        {
                            txtApplyingDepartment.Text = tempTable.Rows[0][0].ToString();
                            txtApplyingDepartment.Tag = UniversalFunction.GetDeptCode(tempTable.Rows[0][0].ToString());
                        }

                        tempTable = DataSetHelper.SelectDistinct("", frm._DtResult, "业务类型明细");

                        if (tempTable.Rows.Count > 1)
                        {
                            MessageDialog.ShowPromptMessage("选择的业务编号的业务类型明细不一致");
                            return;
                        }
                        else
                        {
                            cmbTypeDetail.Text = tempTable.Rows[0][0].ToString();
                        }

                        List<string> listTemp = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "单据号");
                        m_listViewDetail = m_serviceInPut.GetReferenceListViewDetail(txtBillNo.Text, listTemp, chbIsRepeat.Checked);

                        if (m_listViewDetail != null)
                        {
                            List<Business_WarehouseInPut_Requisition> listRequisition =
                                m_serviceRequistion.GetListBillInfo(m_listViewDetail.Select(k => k.关联业务).Distinct().ToList());

                            cmbTypeDetail.DataSource = listRequisition.Select(k => k.BillTypeDetail).Distinct().ToList();
                            m_serverProductCode.InsertChangeProductCodesBillNo(listRequisition.Select(k => k.BillNo).ToList(), txtBillNo.Text);

                            customDataGridView1.Rows.Clear();
                            RefreshDataGridView(m_listViewDetail);
                        }
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

                m_lnqBillInfo = new Business_WarehouseInPut_InPut();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.BillType = cmbBillType.Text;
                m_lnqBillInfo.BillTypeDetail = cmbTypeDetail.Text;
                m_lnqBillInfo.ApplyingDepartment = txtApplyingDepartment.Tag.ToString();
                m_lnqBillInfo.StorageID = cmbStorage.SelectedValue.ToString();
                m_lnqBillInfo.Remark = txtRemark.Text;

                List<View_Business_WarehouseInPut_InPutDetail> listTemp = new List<View_Business_WarehouseInPut_InPutDetail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_WarehouseInPut_InPutDetail detailTemp = new View_Business_WarehouseInPut_InPutDetail();

                    detailTemp.备注 = dgvr.Cells["备注"].Value == null ? "" : dgvr.Cells["备注"].Value.ToString();
                    detailTemp.单据号 = txtBillNo.Text;
                    detailTemp.单位 = dgvr.Cells["单位"].Value == null ? "" : dgvr.Cells["单位"].Value.ToString();
                    detailTemp.供应商 = dgvr.Cells["供应商"].Value == null ? "" : dgvr.Cells["供应商"].Value.ToString();
                    detailTemp.关联业务 = dgvr.Cells["关联业务"].Value == null ? "" : dgvr.Cells["关联业务"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.批次号 = dgvr.Cells["批次号"].Value == null ? "" : dgvr.Cells["批次号"].Value.ToString();
                    detailTemp.数量 = Convert.ToDecimal(dgvr.Cells["数量"].Value);
                    detailTemp.图号型号 = dgvr.Cells["图号型号"].Value == null ? "" : dgvr.Cells["图号型号"].Value.ToString();
                    detailTemp.物品ID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    detailTemp.物品名称 = dgvr.Cells["物品名称"].Value == null ? "" : dgvr.Cells["物品名称"].Value.ToString();

                    if (detailTemp.供应商 == "")
                    {
                        throw new Exception(UniversalFunction.GetGoodsMessage(detailTemp.物品ID) 
                            + "批次号：【" + detailTemp.批次号 + "】,供应商为空，请选择供应商");
                    }

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
                            if (!m_serverProductCode.IsFitCount(detailTemp.物品ID, Convert.ToInt32(detailTemp.数量), detailTemp.单据号))
                            {
                                MessageBox.Show("请对产品设置流水号,并保证产品数量与流水号数一致", "提示");
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

                CE_InPutBusinessType inPutType = GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(cmbBillType.Text);
                CE_MarketingType marketType = GlobalObject.EnumOperation.InPutBusinessTypeConvertToMarketingType(inPutType);
                CE_SubsidiaryOperationType subsdiaryType = GlobalObject.EnumOperation.InPutBusinessTypeConvertToSubsidiaryOperationType(inPutType);

                tempDic.Add(UniversalFunction.GetStorageID(cmbStorage.Text), marketType.ToString());

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
