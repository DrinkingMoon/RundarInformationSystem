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
    public partial class 到货单明细 : CustomFlowForm
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据号
        /// </summary>
        private Business_WarehouseInPut_AOG m_lnqBillInfo = new Business_WarehouseInPut_AOG();

        public Business_WarehouseInPut_AOG LnqBillInfo
        {
            get { return m_lnqBillInfo; }
            set { m_lnqBillInfo = value; }
        }

        /// <summary>
        /// 明细视图数据集合
        /// </summary>
        List<View_Business_WarehouseInPut_AOGDetail> m_listViewDetail = new List<View_Business_WarehouseInPut_AOGDetail>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IAOGService m_serviceAOG = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IAOGService>();

        /// <summary>
        /// 流程服务组件
        /// </summary>
        IFlowServer m_serverFlow = FlowControlService.ServerModuleFactory.GetServerModule<IFlowServer>();

        /// <summary>
        /// 入库申请单服务组件
        /// </summary>
        IRequisitionService_InPut m_serviceRequistion = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<IRequisitionService_InPut>();

        public 到货单明细()
        {
            InitializeComponent();
        }

        public override void LoadFormInfo()
        {
            try
            {
                m_billNoControl = new BillNumberControl(CE_BillTypeEnum.到货单.ToString(), m_serviceAOG);
                m_lnqBillInfo = m_serviceAOG.GetSingleBillInfo(this.FlowInfo_BillNo);
                SetInfo();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
            }
        }

        void SetInfo()
        {
            if (m_lnqBillInfo != null)
            {
                lbBillStatus.Text = m_serverFlow.GetNowBillStatus(m_lnqBillInfo.BillNo);

                txtBillNo.Text = m_lnqBillInfo.BillNo;
                txtRemark.Text = m_lnqBillInfo.Remark;
            }
            else
            {
                lbBillStatus.Text = CE_CommonBillStatus.新建单据.ToString();

                m_lnqBillInfo = new Business_WarehouseInPut_AOG();

                txtBillNo.Text = this.FlowInfo_BillNo;
                m_lnqBillInfo.BillNo = txtBillNo.Text;
            }

            m_listViewDetail = m_serviceAOG.GetListViewDetailInfo(m_lnqBillInfo.BillNo);
            RefreshDataGridView(m_listViewDetail);
        }

        void RefreshDataGridView(List<View_Business_WarehouseInPut_AOGDetail> source)
        {
            if (source != null)
            {
                customDataGridView1.Rows.Clear();
                foreach (View_Business_WarehouseInPut_AOGDetail item in source)
                {
                    customDataGridView1.Rows.Add(new object[] { item.关联业务, item.图号型号, item.物品名称, item.规格, item.批次号,
                        item.供应商, item.数量, item.单位, item.备注, item.物品ID, item.单据号});
                }
            }
        }

        bool CheckData()
        {
            if (customDataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请添加明细");
                return false;
            }

            return true;
        }

        private bool customForm_PanelGetDateInfo(CE_FlowOperationType flowOperationType)
        {
            try
            {
                if (!CheckData())
                {
                    return false;
                }

                m_lnqBillInfo = new Business_WarehouseInPut_AOG();

                m_lnqBillInfo.BillNo = txtBillNo.Text;
                m_lnqBillInfo.Remark = txtRemark.Text;

                List<View_Business_WarehouseInPut_AOGDetail> listTemp = new List<View_Business_WarehouseInPut_AOGDetail>();

                foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
                {
                    View_Business_WarehouseInPut_AOGDetail detailTemp = new View_Business_WarehouseInPut_AOGDetail();

                    detailTemp.关联业务 = dgvr.Cells["关联业务"].Value == null ? "" : dgvr.Cells["关联业务"].Value.ToString();
                    detailTemp.备注 = dgvr.Cells["备注"].Value == null ? "" : dgvr.Cells["备注"].Value.ToString();
                    detailTemp.单据号 = txtBillNo.Text;
                    detailTemp.单位 = dgvr.Cells["单位"].Value == null ? "" : dgvr.Cells["单位"].Value.ToString();
                    detailTemp.供应商 = dgvr.Cells["供应商"].Value == null ? "" : dgvr.Cells["供应商"].Value.ToString();
                    detailTemp.规格 = dgvr.Cells["规格"].Value == null ? "" : dgvr.Cells["规格"].Value.ToString();
                    detailTemp.批次号 = dgvr.Cells["批次号"].Value == null ? "" : dgvr.Cells["批次号"].Value.ToString();
                    detailTemp.数量 = Convert.ToDecimal(dgvr.Cells["数量"].Value);
                    detailTemp.图号型号 = dgvr.Cells["图号型号"].Value == null ? "" : dgvr.Cells["图号型号"].Value.ToString();
                    detailTemp.物品ID = Convert.ToInt32(dgvr.Cells["物品ID"].Value);
                    detailTemp.物品名称 = dgvr.Cells["物品名称"].Value == null ? "" : dgvr.Cells["物品名称"].Value.ToString();

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

        private void btnSelect_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = new FormQueryInfo(m_serviceAOG.GetReferenceInfo(chbIsRepeat.Checked));

            if (DialogResult.OK == form.ShowDialog())
            {
                string billRelate = form.GetDataItem("单据号").ToString();

                DataTable tempTable = m_serviceRequistion.GetListViewDetial(billRelate, null, null, null);

                if (tempTable != null)
                {
                    List<View_Business_WarehouseInPut_AOGDetail> lstTemp = new List<View_Business_WarehouseInPut_AOGDetail>();

                    for (int i = 0; i < tempTable.Rows.Count; i++)
                    {
                        View_Business_WarehouseInPut_AOGDetail lnqTemp = new View_Business_WarehouseInPut_AOGDetail();

                        lnqTemp.关联业务 = billRelate;
                        lnqTemp.单据号 = txtBillNo.Text;
                        lnqTemp.单位 = tempTable.Rows[i]["单位"].ToString();
                        lnqTemp.供应商 = tempTable.Rows[i]["供应商"].ToString();
                        lnqTemp.规格 = tempTable.Rows[i]["规格"].ToString();
                        lnqTemp.数量 = Convert.ToDecimal(tempTable.Rows[i]["数量"]);
                        lnqTemp.图号型号 = tempTable.Rows[i]["图号型号"].ToString();
                        lnqTemp.物品ID = Convert.ToInt32(tempTable.Rows[i]["物品ID"]);
                        lnqTemp.物品名称 = tempTable.Rows[i]["物品名称"].ToString();

                        switch (GlobalObject.GeneralFunction.StringConvertToEnum<CE_InPutBusinessType>(tempTable.Rows[0]["BillType"].ToString()))
                        {
                            case CE_InPutBusinessType.生产采购:
                            case CE_InPutBusinessType.普通采购:
                            case CE_InPutBusinessType.委外采购:
                            case CE_InPutBusinessType.样品采购:
                                lnqTemp.批次号 = txtBillNo.Text + i.ToString("D3");
                                break;
                            case CE_InPutBusinessType.领料退库:
                                if (tempTable.Rows[i]["批次号"] == null || tempTable.Rows[i]["批次号"].ToString().Trim().Length == 0 )
                                {
                                    lnqTemp.批次号 = txtBillNo.Text + i.ToString("D3");
                                }
                                break;
                            default:
                                if (tempTable.Rows[i]["批次号"] != null && tempTable.Rows[i]["批次号"].ToString().Trim().Length > 0)
                                {
                                    lnqTemp.批次号 = tempTable.Rows[i]["批次号"].ToString();
                                }
                                break;
                        }

                        lstTemp.Add(lnqTemp);
                    }

                    customDataGridView1.Rows.Clear();
                    RefreshDataGridView(lstTemp);
                }
            }
        }
    }
}
