using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using PlatformManagement;
using Service_Manufacture_WorkShop;
using Expression;
using UniversalControlLibrary;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    enum ToLeadType
    {
        领料单导入,
        拆卸单导入, 
        装配BOM选择性导入,
        组装单导入,
        组装返修电子档案导入,
        拆卸电子档案导入,
        返修匹配
    }

    public partial class 车间物料转换单明细 : Form
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
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strBillNo = "";

        public string StrBillNo
        {
            get { return m_strBillNo; }
            set { m_strBillNo = value; }
        }

        /// <summary>
        /// 单据LINQ数据集
        /// </summary>
        WS_MaterialsTransfer m_lnqBill = new WS_MaterialsTransfer();

        /// <summary>
        /// 明细信息
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 车间库存管理服务组件
        /// </summary>
        IWorkShopStock m_serverStock = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopStock>();

        /// <summary>
        /// 车间物料转换服务组件
        /// </summary>
        IMaterialsTransfer m_serverMaterials = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IMaterialsTransfer>();

        /// <summary>
        /// 车间管理基础信息服务组件
        /// </summary>
        IWorkShopBasic m_serverWSBasic = Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<IWorkShopBasic>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IBomServer m_bomService = ServerModule.ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 人员管理组件
        /// </summary>
        IPersonnelInfoServer m_serverPersonnel = ServerModule.ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 转换方式枚举
        /// </summary>
        MaterialsTransferConvertType m_selectType;

        public 车间物料转换单明细(string billNo)
        {
            InitializeComponent();

            m_strBillNo = billNo;

            m_lnqBill = m_serverMaterials.GetBillSingle(m_strBillNo);
            m_dtList = m_serverMaterials.GetListInfo(m_strBillNo);

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.车间物料转换单.ToString(), m_serverMaterials);
            m_billMessageServer.BillType = CE_BillTypeEnum.车间物料转换单.ToString();

            cmbConvertType.DataSource = GlobalObject.GeneralFunction.GetEumnList(typeof(MaterialsTransferConvertType));

            cmbWSCode.DataSource = m_serverWSBasic.GetWorkShopBasicInfo();

            cmbWSCode.DisplayMember = "车间名称";
            cmbWSCode.ValueMember = "车间编码";
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(DataGridView datagridview,int goodsID, string batchNo)
        {
            for (int i = 0; i < datagridview.Rows.Count; i++)
            {
                if ((int)datagridview.Rows[i].Cells["物品ID"].Value == goodsID
                    && (string)datagridview.Rows[i].Cells["批次号"].Value == batchNo)
                {
                    datagridview.FirstDisplayedScrollingRowIndex = i;
                    datagridview.CurrentCell = datagridview.Rows[i].Cells["批次号"];
                }
            }
        }

        /// <summary>
        /// 获取信息
        /// </summary>
        void GetInfo()
        {
            m_lnqBill = new WS_MaterialsTransfer();

            m_lnqBill.BillNo = txtBillNo.Text;
            m_lnqBill.BillStatus = lbBillStatus.Text;
            m_lnqBill.Remark = txtBillRemark.Text;
            m_lnqBill.WSCode = cmbWSCode.SelectedValue.ToString();
            m_lnqBill.ConvertType = cmbConvertType.Text;

            m_dtList = GlobalObject.DataSetHelper.SelectUnionAll((DataTable)dataGridViewBefore.DataSource, 
                (DataTable)dataGridViewAfter.DataSource);
            
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        void ShowInfo()
        {
            if (m_lnqBill == null)
            {
                lbPropose.Text = BasicInfo.LoginName;
                lbProposerDate.Text = ServerTime.Time.ToShortDateString();
                return;
            }

            txtBillNo.Text = m_lnqBill.BillNo;
            txtBillRemark.Text = m_lnqBill.Remark;
            cmbConvertType.Text = m_lnqBill.ConvertType;

            WS_WorkShopCode temp = m_serverWSBasic.GetWorkShopCodeInfo(m_lnqBill.WSCode);

            if (temp != null)
            {
                cmbWSCode.Text = temp.WSName;
                cmbWSCode.SelectedValue = temp.WSCode;
            }

            lbBillStatus.Text = m_lnqBill.BillStatus;
            lbAffirm.Text = m_lnqBill.Affirm == null ? "" : m_lnqBill.Affirm;
            lbAffirmDate.Text = m_lnqBill.AffirmDate == null ? "" : m_lnqBill.AffirmDate.ToString();
            lbAudit.Text = m_lnqBill.Audit == null ? "" : m_lnqBill.Audit;
            lbAuditDate.Text = m_lnqBill.AuditDate == null ? "" : m_lnqBill.AuditDate.ToString();
            lbPropose.Text = m_lnqBill.Proposer == null ? "" : m_lnqBill.Proposer;
            lbProposerDate.Text = m_lnqBill.ProposerDate.ToString() == "0001-1-1 0:00:00" ? "" : m_lnqBill.ProposerDate.ToString();

            BandingData(userControlDataLocalizerBefore, dataGridViewBefore, GlobalObject.DataSetHelper.SiftDataTable(m_dtList, "操作类型 = " +
                (int)CE_SubsidiaryOperationType.物料转换前, out m_strError));
            BandingData(userControlDataLocalizerAfter, dataGridViewAfter, GlobalObject.DataSetHelper.SiftDataTable(m_dtList, "操作类型 = " +
                (int)CE_SubsidiaryOperationType.物料转换后, out m_strError));

        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearData()
        {
            lbAffirm.Text = "";
            lbAffirmDate.Text = "";
            lbAudit.Text = "";
            lbAuditDate.Text = "";
            lbBillStatus.Text = "";
            lbPropose.Text = "";
            lbProposerDate.Text = "";
            lbHYDW.Text = "单位";
            lbKCDW.Text = "单位";
            lbStockCount.Text = "";

            txtBatchNo.Text = "";
            txtBillNo.Text = "";
            txtBillRemark.Text = "";
            txtCode.Text = "";
            txtListRemark.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";

            DataTable tempDataTableBefore = m_dtList.Clone();
            DataTable tempDataTableAfter = m_dtList.Clone();
            BandingData(userControlDataLocalizerBefore, dataGridViewBefore, tempDataTableBefore);
            BandingData(userControlDataLocalizerAfter, dataGridViewAfter, tempDataTableAfter);
        }

        /// <summary>
        /// 明细信息清空
        /// </summary>
        void ListClear()
        {
            txtCode.Text = "";
            txtCode.Tag = null;
            txtName.Text = "";
            txtSpec.Text = "";

            txtBatchNo.Text = "";
            numOperationCount.Value = 0;
            lbStockCount.Text = "";
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckData()
        {
            m_dtList = GlobalObject.DataSetHelper.SelectUnionAll((DataTable)dataGridViewBefore.DataSource,
                (DataTable)dataGridViewAfter.DataSource);

            if (cmbWSCode.SelectedValue == null || cmbWSCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择所属车间");
                return false;
            }
            else if (dataGridViewBefore.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("转换前物品明细无记录，请录入");
                return false;
            }
            else if (dataGridViewAfter.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("转换后物品明细无记录，请录入");
                return false;
            }
            //else if (m_dtList == null || m_dtList.Rows.Count == 0)
            //{
            //    MessageDialog.ShowPromptMessage("请录入物品");.
            //    return false;
            //}
            //else if (m_dtList.Select("操作类型 = "+ (int)SubsidiaryOperationType.物料转换后).Length == 0)
            //{
            //    MessageDialog.ShowPromptMessage("转换后物品明细无记录，请录入");
            //    return false;
            //}
            //else if (m_dtList.Select("操作类型 = " + (int)SubsidiaryOperationType.物料转换前).Length == 0)
            //{
            //    MessageDialog.ShowPromptMessage("转换前物品明细无记录，请录入");
            //    return false;
            //}
            else if (txtBillNo.Text == null || txtBillNo.Text == "")
            {
                MessageDialog.ShowPromptMessage("请确认单据号");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 设置批次号
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        /// <param name="batchNo">批次号</param>
        void SetBatchNo(int goodsID, ref string batchNo)
        {
            MaterialsTransferConvertType tempType =
                       GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(cmbConvertType.Text);

            switch (tempType)
            {
                case MaterialsTransferConvertType.组装:
                    batchNo = CE_BatchNoPrefix.ZRD.ToString() + txtBillNo.Text.Substring(4);
                    break;
                case MaterialsTransferConvertType.拆卸:
                    if (batchNo.Trim() == "")
                    {
                        batchNo = CE_BatchNoPrefix.ZT.ToString() + ServerTime.Time.Year.ToString() + goodsID.ToString("D6");
                    }
                    break;
                case MaterialsTransferConvertType.维修:
                    batchNo = CE_BatchNoPrefix.WT.ToString() + ServerTime.Time.Year.ToString() + goodsID.ToString("D6");
                    break;
                case MaterialsTransferConvertType.加工:
                    batchNo = CE_BatchNoPrefix.ZRD.ToString() + txtBillNo.Text.Substring(4);
                    break;
                default:
                    break;
            }

            if (UniversalFunction.IsProduct(goodsID))
            {
                batchNo = "";
            }
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="dataGridView">数据集</param>
        void AddGoods(UserControlDataLocalizer userControlDataLocalizer, DataGridView dataGridView)
        {
            if (txtCode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请录入物品");
                return;
            }

            DataTable dtTemp = (DataTable)dataGridView.DataSource;

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtCode.Text;
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["物品ID"] = txtCode.Tag;
            dr["批次号"] = txtBatchNo.Text;
            dr["数量"] = numOperationCount.Value;
            dr["备注"] = txtListRemark.Text;
            dr["单位"] = lbHYDW.Text;

            if (dataGridView == dataGridViewAfter)
            {
                string batchNo = dr["批次号"].ToString();
                SetBatchNo(Convert.ToInt32(dr["物品ID"]), ref batchNo);

                dr["批次号"] = batchNo;
                dr["操作类型"] = (int)CE_SubsidiaryOperationType.物料转换后;
            }
            else if (dataGridView == dataGridViewBefore)
            {
                dr["操作类型"] = (int)CE_SubsidiaryOperationType.物料转换前;
            }
            else
            {
                MessageDialog.ShowPromptMessage("数据错误，请重新核实");
                return;
            }

            int goodsID = (int)txtCode.Tag;
            string tempbatchNo = txtBatchNo.Text;

            dtTemp.Rows.Add(dr);
            dtTemp.AcceptChanges();
            BandingData(userControlDataLocalizer, dataGridView, dtTemp);
            ListClear();
            PositioningRecord(dataGridView, goodsID, tempbatchNo);
        }

        /// <summary>
        /// 修改物品
        /// </summary>
        /// <param name="dataGridView">数据集</param>
        void ModifyGoods(UserControlDataLocalizer userControlDataLocalizer, DataGridView dataGridView)
        {
            if (txtCode.Tag == null)
            {
                MessageDialog.ShowPromptMessage("请录入物品");
                return;
            }

            dataGridView.CurrentRow.Cells["图号型号"].Value = txtCode.Text;
            dataGridView.CurrentRow.Cells["物品名称"].Value = txtName.Text;
            dataGridView.CurrentRow.Cells["规格"].Value = txtSpec.Text;
            dataGridView.CurrentRow.Cells["物品ID"].Value = txtCode.Tag;
            dataGridView.CurrentRow.Cells["批次号"].Value = txtBatchNo.Text;
            dataGridView.CurrentRow.Cells["数量"].Value = numOperationCount.Value;
            dataGridView.CurrentRow.Cells["备注"].Value = txtListRemark.Text;
            dataGridView.CurrentRow.Cells["单位"].Value = lbHYDW.Text;

            if (dataGridView == dataGridViewAfter)
            {
                string batchNo = dataGridView.CurrentRow.Cells["批次号"].Value.ToString();
                SetBatchNo(Convert.ToInt32(dataGridView.CurrentRow.Cells["物品ID"].Value), ref batchNo);

                dataGridView.CurrentRow.Cells["批次号"].Value = batchNo;
                dataGridView.CurrentRow.Cells["操作类型"].Value = (int)CE_SubsidiaryOperationType.物料转换后;
            }
            else if (dataGridView == dataGridViewBefore)
            {
                dataGridView.CurrentRow.Cells["操作类型"].Value = (int)CE_SubsidiaryOperationType.物料转换前;
            }
            else
            {
                MessageDialog.ShowPromptMessage("数据错误，请重新核实");
                return;
            }

            int goodsID = (int)dataGridView.CurrentRow.Cells["物品ID"].Value;
            string tempbatchNo = (string)dataGridView.CurrentRow.Cells["批次号"].Value;

            DataTable dtTemp = (DataTable)dataGridView.DataSource;
            dtTemp.AcceptChanges();
            BandingData(userControlDataLocalizer, dataGridView, dtTemp);
            PositioningRecord(dataGridView, goodsID, tempbatchNo);
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="dataGridView">数据集</param>
        void DeleteGoods(UserControlDataLocalizer userControlDataLocalizer, DataGridView dataGridView)
        {
            foreach (DataGridViewRow dgvr in dataGridView.SelectedRows)
            {
                dataGridView.Rows.RemoveAt(dgvr.Index);
            }

            DataTable dtTemp = (DataTable)dataGridView.DataSource;
            dtTemp.AcceptChanges();
            BandingData(userControlDataLocalizer, dataGridView, dtTemp);
            ListClear();
        }

        /// <summary>
        /// 选择物品
        /// </summary>
        /// <param name="dataGridView">数据集</param>
        void ClickGoods(DataGridView dataGridView)
        {
            if (dataGridView.CurrentRow == null)
            {
                return;
            }

            txtCode.Tag = dataGridView.CurrentRow.Cells["物品ID"].Value;

            txtBatchNo.Text = dataGridView.CurrentRow.Cells["批次号"].Value.ToString();
            txtCode.Text = dataGridView.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridView.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridView.CurrentRow.Cells["规格"].Value.ToString();
            txtListRemark.Text = dataGridView.CurrentRow.Cells["备注"].Value.ToString();
            numOperationCount.Value = Convert.ToDecimal(dataGridView.CurrentRow.Cells["数量"].Value);
            lbHYDW.Text = dataGridView.CurrentRow.Cells["单位"].Value.ToString();
            lbKCDW.Text = dataGridView.CurrentRow.Cells["单位"].Value.ToString();

            WS_WorkShopStock tempStock = m_serverStock.GetStockSingleInfo(cmbWSCode.SelectedValue.ToString(),
                Convert.ToInt32(txtCode.Tag), txtBatchNo.Text);

            lbStockCount.Text = tempStock == null ? "0" : tempStock.StockCount.ToString();
        }

        /// <summary>
        /// 流程控制
        /// </summary>
        void FlowControl()
        {
            bool visible = UniversalFunction.IsOperator(txtBillNo.Text);

            switch (GlobalObject.GeneralFunction.StringConvertToEnum<MaterialsTransferStatus>(lbBillStatus.Text))
            {
                case MaterialsTransferStatus.新建单据:
                    btnSave.Visible = BasicInfo.LoginName == lbPropose.Text ? true : false;
                    btnPropose.Visible = true;
                    break;
                case MaterialsTransferStatus.等待审核:

                    if (BasicInfo.LoginName == lbPropose.Text)
                    {
                        btnPropose.Visible = true;
                    }

                    btnAudit.Visible = visible;
                    groupBox1.Enabled = false;
                    break;
                case MaterialsTransferStatus.等待确认:

                    if (BasicInfo.LoginName == lbPropose.Text)
                    {
                        btnPropose.Visible = true;
                    }

                    btnAffirm.Visible = visible;
                    groupBox1.Enabled = false;
                    break;
                case MaterialsTransferStatus.单据已完成:
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 财务检测
        /// </summary>
        /// <returns>通过返回True,失败返回False</returns>
        void CheckInfo(out string error)
        {
            error = "";

            try
            {
                MaterialsTransferConvertType convertType = 
                    GlobalObject.GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(m_lnqBill.ConvertType);

                switch (convertType)
                {
                    case MaterialsTransferConvertType.组装:
                    case MaterialsTransferConvertType.拆卸:
                        m_serverMaterials.CheckBom(m_dtList, m_lnqBill.WSCode, convertType);
                        break;
                    case MaterialsTransferConvertType.维修:
                        m_serverMaterials.CheckRepair(m_dtList);
                        break;
                    case MaterialsTransferConvertType.加工:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }
        }

        /// <summary>
        /// 匹配数量
        /// </summary>
        /// <param name="datagridview"></param>
        /// <param name="ratio"></param>
        void MatchCount(DataGridView datagridview, decimal ratio)
        {
            foreach (DataGridViewRow dr in datagridview.Rows)
            {
                dr.Cells["数量"].Value = Math.Round(Convert.ToDecimal(dr.Cells["数量"].Value) * ratio, 0);
            }

            DataTable dtTemp = (DataTable)datagridview.DataSource;
            dtTemp.AcceptChanges();
            datagridview.DataSource = dtTemp;
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (GeneralFunction.StringConvertToEnum<MaterialsTransferStatus>(m_lnqBill.BillStatus) != MaterialsTransferStatus.单据已完成)
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.车间物料转换单, m_lnqBill.BillNo, m_lnqBill.BillStatus);

                if (form.ShowDialog() == DialogResult.OK)
                {

                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_serverMaterials.ReturnBill(m_lnqBill.BillNo,
                            GeneralFunction.StringConvertToEnum<MaterialsTransferStatus>(m_lnqBill.BillStatus),
                            out m_strError, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_strError);
                        }
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        /// <summary>
        /// 绑定DataGridView
        /// </summary>
        /// <param name="userControlDataLocalizer">UserControlDataLocalizer</param>
        /// <param name="dgrv">DataGridView</param>
        /// <param name="source">数据源</param>
        void BandingData(UserControlDataLocalizer userControlDataLocalizer, DataGridView dgrv, DataTable source)
        {
            if (!source.Columns.Contains("异常"))
            {
                source.Columns.Add("异常", typeof(string));
            }

            source.Columns["异常"].DefaultValue = "否";

            dgrv.DataSource = source;

            foreach (DataGridViewRow item in dgrv.Rows)
            {
                if (item.DefaultCellStyle.BackColor == Color.Red)
                {
                    item.Cells["异常"].Value = "是";
                }
            }

            dgrv.Columns["单据号"].Visible = false;

            userControlDataLocalizer.Init(dgrv, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dgrv.Name, BasicInfo.LoginID));
        }

        private void 车间物料转换单明细_Load(object sender, EventArgs e)
        {
            ClearData();
            ShowInfo();

            if (m_strBillNo == null)
            {
                m_strBillNo = m_billNoControl.GetNewBillNo();
                txtBillNo.Text = m_strBillNo;
                lbBillStatus.Text = MaterialsTransferStatus.新建单据.ToString();
                cmbConvertType.SelectedIndex = -1;
                cmbWSCode.SelectedIndex = -1;

                if (BasicInfo.DeptCode.Length >= 4)
                {
                    WS_WorkShopCode tempLnq = m_serverWSBasic.GetWorkShopCodeInfo(BasicInfo.DeptCode.Substring(0, 4));

                    if (tempLnq != null)
                    {
                        cmbWSCode.Text = tempLnq.WSName;
                        cmbWSCode.SelectedValue = tempLnq.WSCode;
                    }
                }
            }

            dataGridViewAfter.Columns["单据号"].Visible = false;
            dataGridViewAfter.Columns["操作类型"].Visible = false;
            dataGridViewBefore.Columns["单据号"].Visible = false;
            dataGridViewBefore.Columns["操作类型"].Visible = false;

            FlowControl();
        }

        private void 车间物料转换单明细_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void btnPropose_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            string error = "";

            CheckInfo(out error);

            if (error.Length > 0)
            {
                FormLargeMessage form = new FormLargeMessage(error, Color.Red);
                form.ShowDialog();

                if (MessageDialog.ShowEnquiryMessage("是否继续提交?") == DialogResult.No)
                {
                    return;
                }
            }

            if (!m_serverMaterials.ProposeBill(m_lnqBill, m_dtList, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                m_billMessageServer.DestroyMessage(m_lnqBill.BillNo);
                m_billMessageServer.SendNewFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号车间物料转换单已提交，请等待上级审核", m_lnqBill.BillNo),
                    BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                MessageDialog.ShowPromptMessage("提交成功");
                this.Close();
            }
        }

        private void btnAudit_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            if (!m_serverMaterials.AuditBill(m_lnqBill.BillNo, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                List<string> tempList =
                    GlobalObject.GeneralFunction.ConvertListTypeToStringList<CE_RoleEnum>(
                    UniversalFunction.GetStoreroomKeeperRoleEnumList(cmbWSCode.SelectedValue.ToString()));

                m_billMessageServer.PassFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号车间物料转换单已审核，请相关物料管理员确认", m_lnqBill.BillNo),
                    BillFlowMessage_ReceivedUserType.角色, tempList);

                MessageDialog.ShowPromptMessage("审核成功");
                this.Close();
            }
        }

        private void btnAffirm_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            string error = "";

            CheckInfo(out error);

            if (!m_serverMaterials.AffirmBill(m_lnqBill.BillNo, m_dtList, error,  out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                List<string> listPersonnel = new List<string>();

                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqBill.Proposer));
                listPersonnel.Add(UniversalFunction.GetPersonnelCode(m_lnqBill.Audit));

                m_billMessageServer.EndFlowMessage(m_lnqBill.BillNo,
                    string.Format("{0}号车间物料转换单已完成", m_lnqBill.BillNo), null, listPersonnel);
                m_billNoControl.UseBill(m_lnqBill.BillNo);

                MessageDialog.ShowPromptMessage("确认成功");
                this.Close();
            }
        }

        private void txtCode_OnCompleteSearch()
        {
            ListClear();

            txtCode.Text = txtCode.DataResult["图号型号"].ToString();
            txtName.Text = txtCode.DataResult["物品名称"].ToString();
            txtSpec.Text = txtCode.DataResult["规格"].ToString();

            lbKCDW.Text = txtCode.DataResult["单位"].ToString();
            lbHYDW.Text = txtCode.DataResult["单位"].ToString();

            txtCode.Tag = txtCode.DataResult["序号"];
            lbStockCount.Text = "0";
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();

            WS_WorkShopStock tempStock = m_serverStock.GetStockSingleInfo(cmbWSCode.SelectedValue.ToString(),
                Convert.ToInt32(txtCode.Tag),
                txtBatchNo.DataResult["批次号"].ToString());

            lbStockCount.Text = tempStock == null ? "0" : tempStock.StockCount.ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtCode.Tag) + " and 车间代码 = '"
                + cmbWSCode.SelectedValue.ToString() +"'";
        }

        private void dataGridViewBefore_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClickGoods(dataGridViewBefore);
        }

        private void btn_Add_Before_Click(object sender, EventArgs e)
        {
            AddGoods(userControlDataLocalizerBefore, dataGridViewBefore);
        }

        private void btn_Modify_Before_Click(object sender, EventArgs e)
        {
            ModifyGoods(userControlDataLocalizerBefore, dataGridViewBefore);
        }

        private void btn_Delete_Before_Click(object sender, EventArgs e)
        {
            DeleteGoods(userControlDataLocalizerBefore, dataGridViewBefore);
        }

        private void dataGridViewAfter_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            ClickGoods(dataGridViewAfter);
        }

        private void btn_Add_After_Click(object sender, EventArgs e)
        {
            AddGoods(userControlDataLocalizerAfter, dataGridViewAfter);
        }

        private void btn_Modify_After_Click(object sender, EventArgs e)
        {
            ModifyGoods(userControlDataLocalizerAfter, dataGridViewAfter);
        }

        private void btn_Delete_After_Click(object sender, EventArgs e)
        {
            DeleteGoods(userControlDataLocalizerAfter, dataGridViewAfter);
        }

        private void dataGridViewBefore_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridViewBefore.Rows.Count > 0)
            {
                cmbWSCode.Enabled = false;
                cmbConvertType.Enabled = false; 
                lbSumBefore.Text = ((DataTable)dataGridViewBefore.DataSource).Compute("SUM(数量)", "").ToString();
            }
            else
            {
                cmbWSCode.Enabled = true;
                cmbConvertType.Enabled = true;
                lbSumBefore.Text = "0";
            }

            if (lbBillStatus.Text != MaterialsTransferStatus.单据已完成.ToString())
            {
                foreach (DataGridViewRow dgvr in dataGridViewBefore.Rows)
                {
                    WS_WorkShopStock tempStock = m_serverStock.GetStockSingleInfo(cmbWSCode.SelectedValue.ToString(),
                        (int)dgvr.Cells["物品ID"].Value,
                        dgvr.Cells["批次号"].Value.ToString());

                    if (tempStock == null || tempStock.StockCount < (decimal)dgvr.Cells["数量"].Value)
                    {
                        dgvr.DefaultCellStyle.BackColor = Color.Red;
                    }
                    else
                    {
                        dgvr.DefaultCellStyle.BackColor = Control.DefaultBackColor; 
                    }
                }
            }
        }

        private void dataGridViewAfter_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridViewBefore.Rows.Count > 0)
            {
                cmbWSCode.Enabled = false;
                cmbConvertType.Enabled = false;
                lbSumAfter.Text = ((DataTable)dataGridViewAfter.DataSource).Compute("SUM(数量)", "").ToString();
            }
            else
            {
                cmbWSCode.Enabled = true;
                cmbConvertType.Enabled = true;
                lbSumAfter.Text = "0";
            }
        }

        private void dataGridViewBefore_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewBefore.CurrentRow == null)
            {
                return;
            }

            int intGoodsID = (int)dataGridViewBefore.CurrentRow.Cells["物品ID"].Value;

            BarCodeInfo tempInfo = new BarCodeInfo();

            tempInfo.BatchNo = dataGridViewBefore.CurrentRow.Cells["批次号"].Value.ToString();
            tempInfo.Count = (decimal)dataGridViewBefore.CurrentRow.Cells["数量"].Value;
            tempInfo.GoodsCode = dataGridViewBefore.CurrentRow.Cells["图号型号"].Value.ToString();
            tempInfo.GoodsID = intGoodsID;
            tempInfo.GoodsName = dataGridViewBefore.CurrentRow.Cells["物品名称"].Value.ToString();
            tempInfo.Remark = dataGridViewBefore.CurrentRow.Cells["备注"].Value.ToString();
            tempInfo.Spec = dataGridViewBefore.CurrentRow.Cells["规格"].Value.ToString();

            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            tempDic.Add(cmbWSCode.SelectedValue.ToString(), CE_SubsidiaryOperationType.物料转换前.ToString());

            产品编号 form = new 产品编号(tempInfo, CE_BusinessType.车间业务, m_strBillNo,
                lbPropose.Text == BasicInfo.LoginName, tempDic);

            form.ShowDialog();
        }

        private void dataGridViewAfter_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridViewAfter.CurrentRow == null)
            {
                return;
            }

            int intGoodsID = (int)dataGridViewAfter.CurrentRow.Cells["物品ID"].Value;

            BarCodeInfo tempInfo = new BarCodeInfo();

            tempInfo.BatchNo = dataGridViewAfter.CurrentRow.Cells["批次号"].Value.ToString();
            tempInfo.Count = (decimal)dataGridViewAfter.CurrentRow.Cells["数量"].Value;
            tempInfo.GoodsCode = dataGridViewAfter.CurrentRow.Cells["图号型号"].Value.ToString();
            tempInfo.GoodsID = intGoodsID;
            tempInfo.GoodsName = dataGridViewAfter.CurrentRow.Cells["物品名称"].Value.ToString();
            tempInfo.Remark = dataGridViewAfter.CurrentRow.Cells["备注"].Value.ToString();
            tempInfo.Spec = dataGridViewAfter.CurrentRow.Cells["规格"].Value.ToString();

            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            tempDic.Add(cmbWSCode.SelectedValue.ToString(), CE_SubsidiaryOperationType.物料转换后.ToString());

            产品编号 form = new 产品编号(tempInfo, CE_BusinessType.车间业务, m_strBillNo,
                lbPropose.Text == BasicInfo.LoginName, tempDic);

            form.ShowDialog();
        }

        private void btnMatching_Click(object sender, EventArgs e)
        {
            if (dataGridViewAfter.Rows.Count > 0 && dataGridViewBefore.Rows.Count > 0)
            {
                批量修改 frm = new 批量修改();

                frm.ShowDialog();

                if (frm.DialogResult == DialogResult.OK)
                {
                    if (frm.EnumOperationType == CE_SubsidiaryOperationType.物料转换前)
                    {
                        MatchCount(dataGridViewBefore, frm.DecRatio);
                    }
                    else if (frm.EnumOperationType == CE_SubsidiaryOperationType.物料转换后)
                    {
                        MatchCount(dataGridViewAfter, frm.DecRatio);
                    }
                }
            }
        }

        private void btnReback_Click(object sender, EventArgs e)
        {
            switch (GeneralFunction.StringConvertToEnum<MaterialsTransferStatus>(m_lnqBill.BillStatus))
            {
                case MaterialsTransferStatus.等待审核:

                    if (btnAudit.Visible)
                    {
                        ReturnBillStatus();
                    }
                    break;
                case MaterialsTransferStatus.等待确认:

                    if (btnAffirm.Visible)
                    {
                        ReturnBillStatus();
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnBeforeToLead_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();

            if (cmbConvertType.Text != "")
            {
                tempList.Add(ToLeadType.领料单导入.ToString());
                tempList.Add(ToLeadType.拆卸单导入.ToString());
                tempList.Add(ToLeadType.组装返修电子档案导入.ToString());
            }

            FormDataRadio frm = new FormDataRadio(tempList, true);
            frm.OnFormDataRodioDelegate += new GlobalObject.DelegateCollection.FormDataRodioDelegate(frmAll_OnFormDataRodioDelegate);
            frm.ShowDialog();
        }

        private void btnAfterToLead_Click(object sender, EventArgs e)
        {
            List<string> tempList = new List<string>();

            if (cmbConvertType.Text != "")
            {
                tempList.Add(ToLeadType.装配BOM选择性导入.ToString());
                tempList.Add(ToLeadType.组装单导入.ToString());
                tempList.Add(ToLeadType.拆卸电子档案导入.ToString());
                tempList.Add(ToLeadType.返修匹配.ToString());
            }

            FormDataRadio frm = new FormDataRadio(tempList, true);
            frm.OnFormDataRodioDelegate += new GlobalObject.DelegateCollection.FormDataRodioDelegate(frmAll_OnFormDataRodioDelegate);
            frm.ShowDialog();
        }

        DialogResult frmAll_OnFormDataRodioDelegate(string info)
        {
            ToLeadType type = GlobalObject.GeneralFunction.StringConvertToEnum<ToLeadType>(info);

            DataTable tempTable = new DataTable();

            switch (type)
            {
                case ToLeadType.领料单导入:
                    MaterialsTransferConvertType selectType = GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(cmbConvertType.Text);

                    if (selectType == MaterialsTransferConvertType.拆卸)
                    {
                        return DialogResult.Cancel;
                    }

                    if (cmbWSCode.SelectedValue == null || cmbWSCode.SelectedValue.ToString() == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择所属车间");
                        return DialogResult.Cancel;
                    }

                    tempTable = m_serverMaterials.GetRequisitionInfo(cmbWSCode.SelectedValue.ToString());
                    FormDataTableCheck frm = new FormDataTableCheck(tempTable);
                    frm._BlDateTimeControlShow = true;
                    frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);
                    frm.OnFormDataTableCheck += new GlobalObject.DelegateCollection.FormDataTableCheckDelegate(frmBeforeRequisition_OnFormDataTableCheck);
                    return frm.ShowDialog();

                case ToLeadType.拆卸单导入:
                    tempTable = m_serverMaterials.GetMaterialsTransferInfo(cmbWSCode.SelectedValue.ToString());
                    frm = new FormDataTableCheck(tempTable);
                    frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);
                    frm.OnFormDataTableCheck += new GlobalObject.DelegateCollection.FormDataTableCheckDelegate(frmBeforeDisassembly_OnFormDataTableCheck);
                    return frm.ShowDialog();

                case ToLeadType.拆卸电子档案导入:
                case ToLeadType.组装返修电子档案导入:

                    int goodsID = 0;
                    MaterialsTransferConvertType convertType = 
                        GlobalObject.GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(cmbConvertType.Text);
                    CE_SubsidiaryOperationType subType = CE_SubsidiaryOperationType.未知;
                    DataTable tempTable1 = new DataTable();

                    DateTime dtStart = new DateTime();
                    DateTime dtEnd = new DateTime();

                    switch (convertType)
                    {
                        case MaterialsTransferConvertType.组装:
                        case MaterialsTransferConvertType.维修:
                            subType = CE_SubsidiaryOperationType.物料转换后;

                            tempTable1 = (DataTable)dataGridViewAfter.DataSource;

                            if (tempTable1 == null || tempTable1.Rows.Count == 0)
                            {
                                MessageDialog.ShowPromptMessage("请录入转换前物品信息");
                                return DialogResult.Cancel;
                            }

                            if (convertType == MaterialsTransferConvertType.维修)
                            {
                                FormNormalChoseDateRange frmDate = new FormNormalChoseDateRange();
                                frmDate.ShowDialog();
                                dtStart = frmDate.BeginTime;
                                dtEnd = frmDate.EndTime;
                            }

                            break;
                        case MaterialsTransferConvertType.拆卸:

                            if (type == ToLeadType.拆卸电子档案导入)
                            {
                                subType = CE_SubsidiaryOperationType.物料转换前;

                                tempTable1 = (DataTable)dataGridViewBefore.DataSource;

                                if (tempTable1 == null || tempTable1.Rows.Count == 0)
                                {
                                    MessageDialog.ShowPromptMessage("请录入转换前物品信息");
                                    return DialogResult.Cancel;
                                }
                            }
                            else
                            {
                                return DialogResult.Cancel;
                            }
                            break;
                        case MaterialsTransferConvertType.加工:
                            return DialogResult.Cancel;
                        default:
                            subType = CE_SubsidiaryOperationType.未知;
                            break;
                    }

                    goodsID = m_serverMaterials.GetASSYGoodsID(tempTable1);

                    if (goodsID == 0)
                    {
                        MessageDialog.ShowPromptMessage("未提供正确信息，无法导入总成相关信息");
                        return DialogResult.Cancel;
                    }

                    tempTable = m_serverMaterials.GetElectronFileInfo(txtBillNo.Text, goodsID, subType, convertType,
                        cmbWSCode.SelectedValue.ToString(), dtStart, dtEnd);

                    if (tempTable.Rows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("此单据的总成无相关信息");
                        return DialogResult.Cancel;
                    }

                    return InsertDataToBefore(tempTable, convertType);

                case ToLeadType.装配BOM选择性导入:
                    MaterialsTransferConvertType m_selectType = GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(cmbConvertType.Text);

                    if (m_selectType != MaterialsTransferConvertType.拆卸)
                    {
                        return DialogResult.Cancel;
                    }

                    if (dataGridViewBefore.Rows.Count != 1)
                    {
                        return DialogResult.Cancel;
                    }

                    List<string> tempList = m_bomService.GetAssemblyTypeList();

                    if (tempList.Count != 0)
                    {
                        FormDataRadio frmFDR = new FormDataRadio(tempList, true);
                        frmFDR.OnFormDataRodioDelegate += new GlobalObject.DelegateCollection.FormDataRodioDelegate(frmAfterBom_OnFormDataRodioDelegate);
                        return frmFDR.ShowDialog();
                    }
                    break;

                case ToLeadType.组装单导入:
                    tempTable = m_serverMaterials.GetMaterialsTransferInfo(cmbWSCode.SelectedValue.ToString());
                    frm = new FormDataTableCheck(tempTable);
                    frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);
                    frm.OnFormDataTableCheck += new GlobalObject.DelegateCollection.FormDataTableCheckDelegate(frmAfterAssembly_OnFormDataTableCheck);
                    return frm.ShowDialog();

                case ToLeadType.返修匹配:

                    int goodsBefore = m_serverMaterials.GetASSYGoodsID((DataTable)dataGridViewBefore.DataSource);
                    int goodsAfter = m_serverMaterials.GetASSYGoodsID((DataTable)dataGridViewAfter.DataSource);

                    if (goodsBefore == 0)
                    {
                        MessageDialog.ShowPromptMessage("在转换前明细中为找到分总成或者总成信息");
                        return DialogResult.Cancel;
                    }

                    if (goodsAfter == 0)
                    {
                        MessageDialog.ShowPromptMessage("在转换后明细中为找到分总成或者总成信息");
                        return DialogResult.Cancel;
                    }

                    tempTable = m_serverMaterials.GetRepairMatch((DataTable)dataGridViewBefore.DataSource, goodsBefore, goodsAfter, 
                        txtBillNo.Text);

                    BandingData(userControlDataLocalizerAfter, dataGridViewAfter, 
                        m_serverMaterials.SumTable((DataTable)dataGridViewAfter.DataSource, tempTable,
                        (int)CE_SubsidiaryOperationType.物料转换后));

                    return DialogResult.OK;
                default:
                    break;
            }

            return DialogResult.Cancel;
        }

        DataTable frm_OnFormDataTableCheckFind(DateTime startTime, DateTime endTime)
        {
            return m_serverMaterials.GetRequisitionInfo(cmbWSCode.SelectedValue.ToString(), startTime, endTime);
        }

        DialogResult frmBeforeRequisition_OnFormDataTableCheck(DataTable tableInfo)
        {
            List<string> tempList = new List<string>();

            foreach (DataRow dr in tableInfo.Rows)
            {
                tempList.Add(dr["单号"].ToString());
            }

            BandingData(userControlDataLocalizerBefore, dataGridViewBefore, m_serverMaterials.SumTable((DataTable)dataGridViewBefore.DataSource,
                m_serverMaterials.SumRequisitionGoods(tempList), (int)CE_SubsidiaryOperationType.物料转换前));

            return DialogResult.OK;
        }

        DialogResult frmBeforeDisassembly_OnFormDataTableCheck(DataTable tableInfo)
        {
            List<string> tempList = new List<string>();

            foreach (DataRow dr in tableInfo.Rows)
            {
                tempList.Add(dr["单据号"].ToString());
            }

            BandingData(userControlDataLocalizerBefore, dataGridViewBefore, m_serverMaterials.SumTable((DataTable)dataGridViewBefore.DataSource,
                m_serverMaterials.SumMaterialsTransferGoods(tempList,
                (int)CE_SubsidiaryOperationType.物料转换后, (int)CE_SubsidiaryOperationType.物料转换前,
                cmbWSCode.SelectedValue.ToString()), (int)CE_SubsidiaryOperationType.物料转换前));

            return DialogResult.OK;
        }

        DialogResult frmAfterBom_OnFormDataRodioDelegate(string info)
        {
            FormDataTableCheck frmTable =
                new FormDataTableCheck(m_serverMaterials.GetAssemblingBomInfo(info,
                    (int)dataGridViewBefore.Rows[0].Cells["物品ID"].Value));
            frmTable.OnFormDataTableCheck += new GlobalObject.DelegateCollection.FormDataTableCheckDelegate(frmAfterBomTable_OnFormDataTableCheck);
            return frmTable.ShowDialog();
        }

        DialogResult frmAfterBomTable_OnFormDataTableCheck(DataTable info)
        {
            DataTable tempTable = ((DataTable)dataGridViewAfter.DataSource).Clone();

            foreach (DataRow dr in info.Rows)
            {
                DataRow drNew = tempTable.NewRow();

                drNew["单据号"] = "";
                drNew["图号型号"] = dr["图号型号"];
                drNew["物品名称"] = dr["物品名称"];
                drNew["规格"] = dr["规格"];

                string batchNoPrefix = m_selectType == MaterialsTransferConvertType.拆卸 ? CE_BatchNoPrefix.ZT.ToString() : CE_BatchNoPrefix.FT.ToString();

                drNew["批次号"] = batchNoPrefix + ServerTime.Time.Year.ToString() + ((int)dr["物品ID"]).ToString("D6");
                drNew["数量"] = (decimal)dr["基数"] * (decimal)dataGridViewBefore.Rows[0].Cells["数量"].Value;
                drNew["单位"] = dr["单位"];
                drNew["操作类型"] = (int)CE_SubsidiaryOperationType.物料转换后;
                drNew["备注"] = "";
                drNew["物品ID"] = dr["物品ID"];

                tempTable.Rows.Add(drNew);
            }

            BandingData(userControlDataLocalizerAfter, dataGridViewAfter, 
                m_serverMaterials.SumTable((DataTable)dataGridViewAfter.DataSource, tempTable, 
                (int)CE_SubsidiaryOperationType.物料转换后));

            return DialogResult.OK;
        }

        DialogResult frmAfterAssembly_OnFormDataTableCheck(DataTable info)
        {
            List<string> tempList = new List<string>();

            foreach (DataRow dr in info.Rows)
            {
                tempList.Add(dr["单据号"].ToString());
            }

            BandingData(userControlDataLocalizerAfter, dataGridViewAfter, 
                m_serverMaterials.SumTable((DataTable)dataGridViewAfter.DataSource,
                m_serverMaterials.SumMaterialsTransferGoods(tempList,
                (int)CE_SubsidiaryOperationType.物料转换前, (int)CE_SubsidiaryOperationType.物料转换后,
                cmbWSCode.SelectedValue.ToString()), (int)CE_SubsidiaryOperationType.物料转换后));

            return DialogResult.OK;
        }

        DialogResult InsertDataToBefore(DataTable insertTable, MaterialsTransferConvertType convertType)
        {
            DataTable tempTable = new DataTable();

            switch (convertType)
            {
                case MaterialsTransferConvertType.组装:
                case MaterialsTransferConvertType.维修:
                    BandingData(userControlDataLocalizerBefore, dataGridViewBefore, m_serverMaterials.SumTable((DataTable)dataGridViewBefore.DataSource, insertTable,
                        (int)CE_SubsidiaryOperationType.物料转换前));
                    break;
                case MaterialsTransferConvertType.拆卸:
                    BandingData(userControlDataLocalizerAfter, dataGridViewAfter, m_serverMaterials.SumTable((DataTable)dataGridViewAfter.DataSource, insertTable, 
                        (int)CE_SubsidiaryOperationType.物料转换后));
                    break;
                case MaterialsTransferConvertType.加工:
                    break;
                default:
                    break;
            }

            return DialogResult.OK;
        } 

        private void cmbConvertType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbConvertType.Text != "")
            {
                m_selectType = GeneralFunction.StringConvertToEnum<MaterialsTransferConvertType>(cmbConvertType.Text);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            if (!m_serverMaterials.SaveBill(m_lnqBill, m_dtList, out m_strError))
            {
                MessageDialog.ShowPromptMessage(m_strError);
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");
                this.Close();
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            GetInfo();

            if (!CheckData())
            {
                return;
            }

            string error = "";

            CheckInfo(out error);

            if (error.Length == 0)
            {
                MessageDialog.ShowPromptMessage("检测通过");
            }
            else
            {
                FormLargeMessage form = new FormLargeMessage(error, Color.Red);
                form.ShowDialog();
            }
        }
    }
}
