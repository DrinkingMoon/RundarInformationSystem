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
    public partial class 整台份请领单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_WarehouseOutPut_WholeMachineRequisition m_lnqBillInfo = new Business_WarehouseOutPut_WholeMachineRequisition();

        public Business_WarehouseOutPut_WholeMachineRequisition LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> m_listViewDetail = 
            new List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail>();

        /// <summary>
        /// 库房顺序视图数据集合
        /// </summary>
        List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> m_listStorageID = 
            new List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IWholeMachineRequisitionService m_serviceWholeMachine = 
            Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IWholeMachineRequisitionService>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModule.ServerModuleFactory.GetServerModule<IProductInfoServer>();

        public 整台份请领单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.整台份请领单.ToString(), m_serviceWholeMachine);
                m_lnqBillInfo = m_serviceWholeMachine.GetSingleBillInfo(this.FlowInfo_BillNo);

                this.图号型号.m_OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(图号型号_m_OnCompleteSearch);
                this.库房名称.m_OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(库房名称_m_OnCompleteSearch);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void 库房名称_m_OnCompleteSearch()
        {
            DataRow drTemp = this.库房名称.DataResult;

            if (drTemp != null)
            {
                customDataGridView2.CurrentRow.Cells["库房名称"].Value = drTemp["库房名称"];
                customDataGridView2.CurrentRow.Cells["库房代码"].Value = drTemp["库房编码"];
                customDataGridView2.CurrentRow.Cells["库房顺序"].Value = customDataGridView1.Rows.Count;
                customDataGridView2.CurrentRow.Cells["单据号1"].Value = txtBillNo.Text;
            }
        }

        void 图号型号_m_OnCompleteSearch()
        {
            DataRow drTemp = this.图号型号.DataResult;

            if (drTemp != null)
            {
                customDataGridView1.CurrentRow.Cells["图号型号"].Value = drTemp["图号型号"];
                customDataGridView1.CurrentRow.Cells["物品名称"].Value = drTemp["物品名称"];
                customDataGridView1.CurrentRow.Cells["规格"].Value = drTemp["规格"];
                customDataGridView1.CurrentRow.Cells["物品ID"].Value = drTemp["序号"];
                customDataGridView1.CurrentRow.Cells["单位"].Value = drTemp["单位"];
            }
        }

        void SetInfo()
        {
            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtRemark.Text = m_lnqBillInfo.Remark;
                txtProductType.Text = m_lnqBillInfo.ProductType;
                numRequestCount.Value = m_lnqBillInfo.MachineCount;
                txtPurpose.Text = m_lnqBillInfo.BillTypeDetail;

                if (lbBillStatus.Text != CE_CommonBillStatus.新建单据.ToString())
                {
                    this.groupBox1.Enabled = false;
                }

                if (lbBillStatus.Text == CE_CommonBillStatus.等待审核.ToString())
                {
                    this.添加ToolStripMenuItem1.Visible = false;
                    this.添加ToolStripMenuItem2.Visible = false;
                    this.删除ToolStripMenuItem1.Visible = false;
                    this.删除ToolStripMenuItem2.Visible = false;

                    this.customDataGridView1.ReadOnly = true;
                    this.customDataGridView2.ReadOnly = true;
                }
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_WarehouseOutPut_WholeMachineRequisition();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }

            m_listViewDetail = m_serviceWholeMachine.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            m_listStorageID = m_serviceWholeMachine.GetListViewStorageIDInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail, m_listStorageID);
        }

        void RefreshDataGridView(List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> source,
            List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> storageList)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_WarehouseOutPut_WholeMachineRequisitionDetail item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.图号型号, item.物品名称, item.规格, item.数量, item.单位, item.基数, item.物品ID, item.单据号});
                }

                userControlDataLocalizer1.Init(customDataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, customDataGridView1.Name, BasicInfo.LoginID));
            }

            if(storageList != null)
            {
                customDataGridView2.Rows.Clear();
                foreach (View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID item1 in storageList)
                {
                    customDataGridView2.Rows.Add(new object[] { item1.库房名称, item1.库房代码, item1.库房顺序, item1.单据号 });
                }
            }
        }

        bool CheckData()
        {
            if (txtPurpose.Text.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【用途】");
                return false;
            }

            if (txtProductType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【产品类型】");
                return false;
            }

            if (txtRemark.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【备注】");
                return false;
            }

            if (numRequestCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请领台份【数量】要大于0");
                return false;
            }

            if (customDataGridView2.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加库房");
                return false;
            }

            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return false;
            }

            return true;
        }

        private void 添加ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            customDataGridView1.Rows.Add(dr);
        }

        private void 添加ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            customDataGridView2.Rows.Add(dr);
        }

        private void 删除ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (customDataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in customDataGridView1.SelectedRows)
                {
                    customDataGridView1.Rows.Remove(dr);
                }
            }
        }

        private void 删除ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (customDataGridView2.Rows.Count > 0)
            {
                foreach (DataGridViewRow dr in customDataGridView2.SelectedRows)
                {
                    customDataGridView2.Rows.Remove(dr);
                }
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

                m_lnqBillInfo = new Business_WarehouseOutPut_WholeMachineRequisition();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.BillTypeDetail = txtPurpose.Text;
                m_lnqBillInfo.ProductType = txtProductType.Text.ToString();
                m_lnqBillInfo.MachineCount = numRequestCount.Value;
                m_lnqBillInfo.Remark = txtRemark.Text;
                m_lnqBillInfo.IncludeAfterSupplement = chbIncludeAfterSupplement.Checked;

                List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> listDetail = 
                    new List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_WarehouseOutPut_WholeMachineRequisitionDetail detailTemp = 
                        new View_Business_WarehouseOutPut_WholeMachineRequisitionDetail();

                    detailTemp.单据号 = txtBillNo.Text;
                    detailTemp.单位 = dgvr.Cells["单位"].Value == null ? "" : dgvr.Cells["单位"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.数量 = Convert.ToDecimal(dgvr.Cells["数量"].Value);
                    detailTemp.基数 = Convert.ToDecimal(dgvr.Cells["基数"].Value);
                    detailTemp.图号型号 = dgvr.Cells["图号型号"].Value == null ? "" : dgvr.Cells["图号型号"].Value.ToString();
                    detailTemp.物品ID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    detailTemp.物品名称 = dgvr.Cells["物品名称"].Value == null ? "" : dgvr.Cells["物品名称"].Value.ToString();

                    listDetail.Add(detailTemp);
                }

                List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID> listStorage =
                    new List<View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID>();

                foreach (DataGridViewRow dgvr in customDataGridView2.Rows)
                {
                    View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID storageTemp =
                        new View_Business_WarehouseOutPut_WholeMachineRequisition_StorageID();

                    storageTemp.单据号 = txtBillNo.Text;
                    storageTemp.库房名称 = dgvr.Cells["库房名称"].Value == null ? "" : dgvr.Cells["库房名称"].Value.ToString();
                    storageTemp.库房代码 = dgvr.Cells["库房代码"].Value == null ? "" : dgvr.Cells["库房代码"].Value.ToString();
                    storageTemp.库房顺序 = dgvr.Cells["库房顺序"].Value == null ? 0 : Convert.ToInt32(dgvr.Cells["库房顺序"].Value.ToString());

                    listStorage.Add(storageTemp);
                }

                this.FlowInfo_BillNo = txtBillNo.Text;

                this.ResultList = new List<object>();

                this.ResultList.Add(m_lnqBillInfo);
                this.ResultList.Add(listDetail);
                this.ResultList.Add(listStorage);
                this.ResultList.Add(flowOperationType);

                return true;
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return false;
            }
        }

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            领料用途 form = new 领料用途();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPurpose.Tag = form.SelectedData.Code;
                txtPurpose.Text = form.SelectedData.Purpose;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (txtProductType.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage(string.Format("请选择产品类型"));
                return;
            }

            //if (!txtPurpose.Text.Contains(txtProductType.Text))
            //{
            //    txtProductType.Focus();
            //    MessageDialog.ShowPromptMessage("请选择与用途匹配的产品类型");
            //    return;
            //}

            if (numRequestCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage(string.Format("整台份数量不能等于0"));
                return;

            }

            List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail> source =
                new List<View_Business_WarehouseOutPut_WholeMachineRequisitionDetail>();

            Service_Project_Design.IBOMInfoService service = 
                Service_Project_Design.ServerModuleFactory.GetServerModule<Service_Project_Design.IBOMInfoService>();

            bool isCVT = Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(txtProductType.Tag), CE_GoodsAttributeName.CVT));
            bool isTCU = Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(txtProductType.Tag), CE_GoodsAttributeName.TCU));

            DataTable tablePBOM = service.GetPBOMItems(isCVT || isTCU ? txtProductType.Text : txtProductType.Tag.ToString());

            if (tablePBOM.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage(string.Format("没有找到{0}型号的生产BOM，无法进行此操作！", txtProductType.Text));
                return;
            }

            foreach (DataRow dr in tablePBOM.Rows)
            {
                View_Business_WarehouseOutPut_WholeMachineRequisitionDetail tempView =
                    new View_Business_WarehouseOutPut_WholeMachineRequisitionDetail();

                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(Convert.ToInt32(dr["GoodsID"]));

                tempView.单据号 = txtBillNo.Text;
                tempView.单位 = goodsInfo.单位;
                tempView.规格 = goodsInfo.规格;
                tempView.基数 = Convert.ToDecimal(dr["Usage"]);
                tempView.数量 = Convert.ToDecimal(dr["Usage"]) * numRequestCount.Value;
                tempView.图号型号 = goodsInfo.图号型号;
                tempView.物品ID = Convert.ToInt32(dr["GoodsID"]);
                tempView.物品名称 = goodsInfo.物品名称;

                source.Add(tempView);
            }

            //Service_Manufacture_Storage.IProductOrder server = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();
            //List<BASE_ProductOrder> mrGoodsOrder =
            //    server.GetAllDataList(chbIncludeAfterSupplement.Checked ? FetchGoodsType.整台领料 : FetchGoodsType.整台领料不含后补充,
            //    txtProductType.Text, CE_DebitScheduleApplicable.正常装配, true);

            //if (mrGoodsOrder.Count == 0)
            //{
            //    MessageDialog.ShowPromptMessage(string.Format("没有找到{0}整台份的领料排序规则，无法进行此操作！", txtProductType.Text));
            //    return;
            //}

            //foreach (BASE_ProductOrder item1 in mrGoodsOrder)
            //{
            //    View_Business_WarehouseOutPut_WholeMachineRequisitionDetail tempView = 
            //        new View_Business_WarehouseOutPut_WholeMachineRequisitionDetail();

            //    View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(item1.GoodsID);

            //    tempView.单据号 = txtBillNo.Text;
            //    tempView.单位 = goodsInfo.单位;
            //    tempView.规格 = goodsInfo.规格;
            //    tempView.基数 = item1.Redices;
            //    tempView.数量 = item1.Redices * numRequestCount.Value;
            //    tempView.图号型号 = goodsInfo.图号型号;
            //    tempView.物品ID = item1.GoodsID;
            //    tempView.物品名称 = goodsInfo.物品名称;


            //    source.Add(tempView);
            //}
            
            RefreshDataGridView(source, null);

            txtProductType.Enabled = false;
            numRequestCount.Enabled = false;
            txtPurpose.Enabled = false;
            btnCreate.Enabled = false;
            chbIncludeAfterSupplement.Enabled = false;
        }

        private void txtProductType_OnCompleteSearch()
        {
            if (txtProductType.DataResult == null)
            {
                return;
            }

            txtProductType.Text = txtProductType.DataResult["图号型号"].ToString();
            txtProductType.Tag = txtProductType.DataResult["序号"];
        }

        private void txtProductType_Enter(object sender, EventArgs e)
        {
            txtProductType.StrEndSql = " and 序号 in (select distinct GoodsID from F_GoodsAttributeRecord "+
                " where AttributeID in (" + (int)CE_GoodsAttributeName.CVT + ", " + (int)CE_GoodsAttributeName.TCU + ", " + (int)CE_GoodsAttributeName.部件 + ") and  AttributeValue = '" + bool.TrueString + "') ";
        }
    }
}
