using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 样品确认申请清单界面
    /// </summary>
    public partial class 样品确认申请单清单 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 订单服务组件
        /// </summary>
        IOrderFormInfoServer m_serverOrderFormInfo = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

        /// <summary>
        /// 合同服务组件
        /// </summary>
        IBargainInfoServer m_serverBargainInfo = ServerModuleFactory.GetServerModule<IBargainInfoServer>();

        /// <summary>
        /// BOM表信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 公共数据集
        /// </summary>
        private S_MusterAffirmBill m_lnqMuster = new S_MusterAffirmBill();

        public S_MusterAffirmBill LnqMuster
        {
            get { return m_lnqMuster; }
            set { m_lnqMuster = value; }
        }

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// 确认单服务
        /// </summary>
        IMusterAffirmBill m_serverMuster = ServerModuleFactory.GetServerModule<IMusterAffirmBill>();

        /// <summary>
        /// 供应商查询窗体
        /// </summary>
        FormQueryInfo m_formProvider;

        /// <summary>
        /// 车间管理基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 车间管理信息
        /// </summary>
        WS_WorkShopCode m_lnqWSCode = new WS_WorkShopCode();

        #region 对于RadioButton的赋值变量

        /// <summary>
        /// 零件工程师确认结果
        /// </summary>
        string m_strEngineerAffirmResult = "";

        /// <summary>
        /// 开发部门主管确认结果
        /// </summary>
        string m_strSatarpAffirmResult = "";

        /// <summary>
        /// 零件工程师剩余样品处理方式
        /// </summary>
        string m_strEngineerRemainMusterDispose = "";

        /// <summary>
        /// 开发部门主管剩余样品处理方式
        /// </summary>
        string m_strSatarpRemainMusterDispose = "";

        /// <summary>
        /// SQE对于报废/退货样品的处理方式
        /// </summary>
        string m_strScarpDisposeMind = "";

        #endregion

        public 样品确认申请单清单(string DJH)
        {
            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.样品确认申请单, m_serverMuster);

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
            m_strDJH = DJH;

            if (m_strDJH == "")
            {
                lblBillNo.Text = m_billNoControl.GetNewBillNo();
                lblBillStatus.Text = "新建单据";
            }
            else
            {
                m_lnqMuster = m_serverMuster.GetBill(m_strDJH);

                ShowAllMessage();
                
                panelBuyer.Enabled = lblBillStatus.Text == "新建单据" ? true : false;
                numMusterCount.Enabled = lblBillStatus.Text == "等待仓管确认到货" ? true : false;
                numRawMaterialPrice.Enabled = lblBillStatus.Text == "等待财务确认" ? true : false;

                if (rbSatrapRemainMusterDispose_BF.Checked == true)
                {
                    label25.Text = "报 废 数";
                    label69.Text = "退 货 数";
                }
                else
                {
                    label25.Text = "入 库 数";
                    label69.Text = "退货/报废数";
                }

                if (lblBillStatus.Text == "单据已完成" || lblBillStatus.Text == "等待仓管确认入库" || lblBillStatus.Text == "等待SQE处理")
                {
                    numScrapCount.Value = Convert.ToDecimal(m_lnqMuster.ScrapCount);
                        //m_serverMuster.GetUseCount(Convert.ToInt32(LnqMuster.GoodsID), LnqMuster.BatchNo);
                    numEligibleCount.Value = Convert.ToDecimal(m_lnqMuster.EligbilityCount);
                    numEjectableCount.Value = Convert.ToDecimal(m_lnqMuster.EjectableCount);
                }
                else
                {
                    numScrapCount.Value = m_serverMuster.GetUseCount(Convert.ToInt32(m_lnqMuster.GoodsID), m_lnqMuster.BatchNo);
                    numEligibleCount.Value = m_serverMuster.GetStockCount(Convert.ToInt32(m_lnqMuster.GoodsID), m_lnqMuster.BatchNo);
                    numEjectableCount.Value = numMusterCount.Value - numScrapCount.Value - numEligibleCount.Value;
                }
            }


            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                btnBatchNo.Visible = false;

                if (m_lnqMuster == null || m_lnqMuster.BatchNo == null || m_lnqMuster.BatchNo == "")
                {
                    txtBatchNo.Text = lblBillNo.Text;
                }
                else
                {
                    txtBatchNo.Text = m_lnqMuster.BatchNo;
                }
            }
            else
            {
                m_lnqWSCode = m_lnqMuster.DJH == null ?
                    m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID) :
                    m_serverWSBasic.GetPersonnelWorkShop(m_lnqMuster.SQR);

                btnBatchNo.Visible = m_lnqWSCode == null ? false : true;

                if (m_lnqMuster == null || m_lnqMuster.BatchNo == null || m_lnqMuster.BatchNo == "")
                {
                    txtBatchNo.Text = m_lnqWSCode == null ? lblBillNo.Text : txtBatchNo.Text;
                }
                else
                {
                    txtBatchNo.Text = m_lnqMuster.BatchNo;
                }
            }
        }

        private void 样品确认申请单清单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 获得所有信息
        /// </summary>
        private void GetAllMessage()
        {
            //GetRbMessage();

            m_lnqMuster.IsIncludeRawMaterial = chkIsIncludeRawMaterial.Checked;
            m_lnqMuster.IsBlank = chkIsBlank.Checked;
            m_lnqMuster.IsOutsourcing = chkIsOutsourcing.Checked;
            m_lnqMuster.RawMaterialPrice = numRawMaterialPrice.Value;
            m_lnqMuster.Financer = txtFinancer.Text;
            m_lnqMuster.DJH = lblBillNo.Text;
            m_lnqMuster.DJZT = lblBillStatus.Text;
            m_lnqMuster.SQR = txtSQR.Text;
            m_lnqMuster.SQERQ = dtpSQRQ.Value;
            m_lnqMuster.GoodsID = Convert.ToInt32(txtCode.Tag);
            m_lnqMuster.BatchNo = txtBatchNo.Text;
            m_lnqMuster.Provider = txtProvider.Text;
            m_lnqMuster.GiveMusterCount = Convert.ToInt32(numGiveMusterCount.Value);
            m_lnqMuster.Version = txtVersion.Text.Trim();
            m_lnqMuster.SendCount = numSendCount.Value;
            m_lnqMuster.IsPay = chkPayment.Checked;
            m_lnqMuster.OrderFormNumber = txtOrderForm.Text;
            m_lnqMuster.MusterType = cmbMusterType.Text;
            m_lnqMuster.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            m_lnqMuster.AffirmGoodsPersonnel = txtQRDH.Text;
            m_lnqMuster.MusterCount = numMusterCount.Value;
            m_lnqMuster.JYR = txtJYR.Text;
            m_lnqMuster.Checker = txtChecker.Text;
            m_lnqMuster.CheckScarpCount = numCheckScarpCount.Value;
            m_lnqMuster.CheckResult = txtCheckResult.Text;
            m_lnqMuster.CheckReport = txtCheckReport.Text;
            m_lnqMuster.FeederReport = cmbFeederReport.Text;
            m_lnqMuster.MusterPack = txtMusterPack.Text;
            m_lnqMuster.SQE = txtSQE.Text;
            m_lnqMuster.SQEExplain = txtSQEExplain.Text;
            m_lnqMuster.PSR = txtPSR.Text;
            m_lnqMuster.MusterCareful = cmbMusterCareful.Text;
            m_lnqMuster.MusterCarefulResult = cmbMusterCarefulResult.Text;
            m_lnqMuster.MusterCarefulResultReport = txtMusterCarefulResultReport.Text;
            m_lnqMuster.JLR = txtJLR.Text;
            m_lnqMuster.AffirmResult = txtAffirmResult.Text;
            m_lnqMuster.SYR = txtSYR.Text;
            m_lnqMuster.EngineerMind = txtEngineerMind.Text;
            m_lnqMuster.TestAssemblyNumber = txtTestAssemblyNumber.Text;
            m_lnqMuster.TestResult = txtTestResult.Text;
            m_lnqMuster.EngineerTestMusterCVTDispose = txtEngineerTestMusterCVTAssemblyDispose.Text;
            m_lnqMuster.EngineerAffirmResult = m_strEngineerAffirmResult;
            m_lnqMuster.EngineerRemainMusterDispose = m_strEngineerRemainMusterDispose;
            m_lnqMuster.ZGR = txtZGR.Text;
            m_lnqMuster.SatrapMind = txtSatarpMind.Text;
            m_lnqMuster.SatrapTestMusterCVTDispose = txtSatrapTestMusterCVTAssemblyDispose.Text;
            m_lnqMuster.SatrapAffirmResult = m_strSatarpAffirmResult;
            m_lnqMuster.SatrapRemainMusterDispose = m_strSatarpRemainMusterDispose;
            m_lnqMuster.IsEligbility = cmbIsEligbility.Text == "合格" ? true : false;
            m_lnqMuster.CLR = txtCLR.Text;
            m_lnqMuster.ScrapCount = Convert.ToInt32(numScrapCount.Value);
            m_lnqMuster.EligbilityCount = Convert.ToInt32(numEligibleCount.Value);
            m_lnqMuster.EjectableCount = Convert.ToInt32(numEjectableCount.Value);
            m_lnqMuster.ScrapDisposeMode = m_strScarpDisposeMind;
            m_lnqMuster.Remark = txtRemark.Text;
            m_lnqMuster.ShelfArea = txtShelf.Text;
            m_lnqMuster.ColumnNumber = txtColumn.Text;
            m_lnqMuster.LayerNumber = txtLayer.Text;
            m_lnqMuster.ProviderBatchNo = txtProviderBatchNo.Text;

            m_lnqMuster.CraftMusterCareful = cmbCraftMusterCareful.Text;
            m_lnqMuster.CraftMusterCarefulDate = dtpCraftMusterCarefulDate.Value;
            m_lnqMuster.CraftMusterCarefulPersonnel = txtCraftMusterCarefulPersonnel.Text;
            m_lnqMuster.CraftMusterCarefulResult = cmbCraftMusterCarefulResult.Text;
            m_lnqMuster.CraftMusterCarefulResultReport = txtCraftMusterCarefulResultReport.Text;

        }

        /// <summary>
        /// 显示所有信息
        /// </summary>
        private void ShowAllMessage()
        {
            View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(Convert.ToInt32(m_lnqMuster.GoodsID));

            if (tempGoodsLnq == null)
            {
                MessageDialog.ShowPromptMessage("物品基础表有误");
                return;
            }

            txtChecker.Text = m_lnqMuster.Checker;

            txtCraftMusterCarefulPersonnel.Text = m_lnqMuster.CraftMusterCarefulPersonnel;
            txtCraftMusterCarefulResultReport.Text = m_lnqMuster.CraftMusterCarefulResultReport;
            dtpCraftMusterCarefulDate.Value = m_lnqMuster.CraftMusterCarefulDate == null ? ServerTime.Time : (DateTime)m_lnqMuster.CraftMusterCarefulDate;
            cmbCraftMusterCareful.Text = m_lnqMuster.CraftMusterCareful;
            cmbCraftMusterCarefulResult.Text = m_lnqMuster.CraftMusterCarefulResult;

            txtProviderBatchNo.Text = m_lnqMuster.ProviderBatchNo;
            chkIsOutsourcing.Checked = m_lnqMuster.IsOutsourcing;
            chkIsIncludeRawMaterial.Checked = m_lnqMuster.IsIncludeRawMaterial;
            chkIsBlank.Checked = m_lnqMuster.IsBlank;
            numRawMaterialPrice.Value = m_lnqMuster.RawMaterialPrice;
            txtFinancer.Text = m_lnqMuster.Financer;
            lblBillNo.Text = m_lnqMuster.DJH;
            txtBatchNo.Text = m_lnqMuster.BatchNo;
            lblBillStatus.Text = m_lnqMuster.DJZT;
            lblBillTime.Text = m_lnqMuster.CreatBillTime.Value.ToString();
            txtSQR.Text = m_lnqMuster.SQR;
            dtpSQRQ.Value = m_lnqMuster.SQRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.SQRQ.Value);
            numGiveMusterCount.Value = Convert.ToDecimal(m_lnqMuster.GiveMusterCount);
            txtVersion.Text = m_lnqMuster.Version;
            numSendCount.Value = Convert.ToDecimal(m_lnqMuster.SendCount);
            chkPayment.Checked = Convert.ToBoolean(m_lnqMuster.IsPay);
            txtOrderForm.Text = m_lnqMuster.OrderFormNumber;
            txtProvider.Text = m_lnqMuster.Provider;
            cmbMusterType.Text = m_lnqMuster.MusterType;
            cmbStorage.Text = UniversalFunction.GetStorageName(m_lnqMuster.StorageID);
            txtCode.Text = tempGoodsLnq.图号型号;
            txtName.Text = tempGoodsLnq.物品名称;
            txtSpec.Text = tempGoodsLnq.规格;
            txtCode.Tag = Convert.ToInt32(m_lnqMuster.GoodsID);
            txtQRDH.Text = m_lnqMuster.AffirmGoodsPersonnel;
            dtpQRDHRQ.Value = m_lnqMuster.AffirmGoodsTime == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.AffirmGoodsTime);
            numMusterCount.Value = Convert.ToDecimal(m_lnqMuster.MusterCount);
            txtJYR.Text = m_lnqMuster.JYR;
            dtpJYRQ.Value = m_lnqMuster.JYRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.JYRQ.Value);
            numCheckScarpCount.Value = Convert.ToDecimal(m_lnqMuster.CheckScarpCount);
            txtCheckReport.Text = m_lnqMuster.CheckReport;
            txtCheckResult.Text = m_lnqMuster.CheckResult;
            cmbFeederReport.Text = m_lnqMuster.FeederReport;
            txtMusterPack.Text = m_lnqMuster.MusterPack;
            txtSQE.Text = m_lnqMuster.SQE;
            dtpSQERQ.Value = m_lnqMuster.SQERQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.SQERQ.Value);
            txtSQEExplain.Text = m_lnqMuster.SQEExplain;
            txtPSR.Text = m_lnqMuster.PSR;
            dtpPSRQ.Value = m_lnqMuster.PSRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.PSRQ.Value);
            txtMusterCarefulResultReport.Text = m_lnqMuster.MusterCarefulResultReport;
            cmbMusterCareful.Text = m_lnqMuster.MusterCareful;
            cmbMusterCarefulResult.Text = m_lnqMuster.MusterCarefulResult;
            txtJLR.Text = m_lnqMuster.JLR;
            dtpJLRQ.Value = m_lnqMuster.JLRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.JLRQ.Value);
            txtAffirmResult.Text = m_lnqMuster.AffirmResult;
            txtSYR.Text = m_lnqMuster.SYR;
            dtpSYRQ.Value = m_lnqMuster.SYRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.SYRQ.Value);
            txtEngineerMind.Text = m_lnqMuster.EngineerMind;
            txtEngineerTestMusterCVTAssemblyDispose.Text = m_lnqMuster.EngineerTestMusterCVTDispose;
            txtTestAssemblyNumber.Text = m_lnqMuster.TestAssemblyNumber;
            txtTestResult.Text = m_lnqMuster.TestResult;
            txtZGR.Text = m_lnqMuster.ZGR;
            dtpZGRQ.Value = m_lnqMuster.ZGRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.ZGRQ.Value);
            txtSatarpMind.Text = m_lnqMuster.SatrapMind;
            txtSatrapTestMusterCVTAssemblyDispose.Text = m_lnqMuster.SatrapTestMusterCVTDispose;
            cmbIsEligbility.Text = Convert.ToBoolean(m_lnqMuster.IsEligbility) == true ?
                "合格" : "不合格";
            txtCLR.Text = m_lnqMuster.CLR;
            dtpCLRQ.Value = m_lnqMuster.CLRQ == null ?
                ServerTime.Time : Convert.ToDateTime(m_lnqMuster.CLRQ.Value);
            numEligibleCount.Value = Convert.ToInt32(m_lnqMuster.EligbilityCount);
            numScrapCount.Value = Convert.ToInt32(m_lnqMuster.ScrapCount);
            m_strEngineerAffirmResult = m_lnqMuster.EngineerAffirmResult == null ? "" : m_lnqMuster.EngineerAffirmResult;
            m_strEngineerRemainMusterDispose = m_lnqMuster.EngineerRemainMusterDispose == null ? "" : m_lnqMuster.EngineerRemainMusterDispose;
            m_strSatarpAffirmResult = m_lnqMuster.SatrapAffirmResult == null ? "" : m_lnqMuster.SatrapAffirmResult;
            m_strSatarpRemainMusterDispose = m_lnqMuster.SatrapRemainMusterDispose == null ? "" : m_lnqMuster.SatrapRemainMusterDispose;
            m_strScarpDisposeMind = m_lnqMuster.ScrapDisposeMode == null ? "" : m_lnqMuster.ScrapDisposeMode;
            numEjectableCount.Value = Convert.ToInt32(m_lnqMuster.EjectableCount);
            txtRemark.Text = m_lnqMuster.Remark;
            txtColumn.Text = m_lnqMuster.ColumnNumber;
            txtLayer.Text = m_lnqMuster.LayerNumber;
            txtShelf.Text = m_lnqMuster.ShelfArea;

            SetRbMessage();
        }

        /// <summary>
        /// 获得RB中的信息
        /// </summary>
        private void GetRbMessage()
        {
            if (rbEngineerAffirmResult_CXSY.Checked)
            {
                m_strEngineerAffirmResult = rbEngineerAffirmResult_CXSY.Text;
            }
            else if (rbEngineerAffirmResult_CXTJ.Checked)
            {
                m_strEngineerAffirmResult = rbEngineerAffirmResult_CXTJ.Text;
            }
            else if (rbEngineerAffirmResult_PPAP.Checked)
            {
                m_strEngineerAffirmResult = rbEngineerAffirmResult_PPAP.Text;
            }
            else if (rbEngineerAffirmResult_PL.Checked)
            {
                m_strEngineerAffirmResult = rbEngineerAffirmResult_PL.Text;
            }

            if (rbEngineerRemainMusterDispose_BF.Checked)
            {
                m_strEngineerRemainMusterDispose = rbEngineerRemainMusterDispose_BF.Text;
            }
            else if (rbEngineerRemainMusterDispose_RCPK.Checked)
            {
                m_strEngineerRemainMusterDispose = rbEngineerRemainMusterDispose_RCPK.Text;
            }
            else if (rbEngineerRemainMusterDispose_RYPK.Checked)
            {
                m_strEngineerRemainMusterDispose = rbEngineerRemainMusterDispose_RYPK.Text;
            }

            if (rbSatarpAffirmResult_CXSY.Checked)
            {
                m_strSatarpAffirmResult = rbSatarpAffirmResult_CXSY.Text;
            }
            else if (rbSatarpAffirmResult_CXTJ.Checked)
            {
                m_strSatarpAffirmResult = rbSatarpAffirmResult_CXTJ.Text;
            }
            else if (rbSatarpAffirmResult_PPAP.Checked)
            {
                m_strSatarpAffirmResult = rbSatarpAffirmResult_PPAP.Text;
            }
            else if (rbSatarpAffirmResult_PL.Checked)
            {
                m_strSatarpAffirmResult = rbSatarpAffirmResult_PL.Text;
            }

            if (rbSatrapRemainMusterDispose_BF.Checked)
            {
                m_strSatarpRemainMusterDispose = rbSatrapRemainMusterDispose_BF.Text;
            }
            else if (rbSatrapRemainMusterDispose_RCPK.Checked)
            {
                m_strSatarpRemainMusterDispose = rbSatrapRemainMusterDispose_RCPK.Text;
            }
            else if (rbSatrapRemainMusterDispose_RYPK.Checked)
            {
                m_strSatarpRemainMusterDispose = rbSatrapRemainMusterDispose_RYPK.Text;
            }

            if (rbScrapDisposeMind_TH.Checked)
            {
                m_strScarpDisposeMind = rbScrapDisposeMind_TH.Text;
            }
            else if (rbScrapDisposeMind_YDBF.Checked)
            {
                m_strScarpDisposeMind = rbScrapDisposeMind_YDBF.Text;
            }
        }

        /// <summary>
        /// 根据信息设置RB的CHK的状态
        /// </summary>
        private void SetRbMessage()
        {
            if (m_strSatarpRemainMusterDispose == rbSatrapRemainMusterDispose_RYPK.Text)
            {
                rbSatrapRemainMusterDispose_RYPK.Checked = true;
            }
            else if (m_strSatarpRemainMusterDispose == rbSatrapRemainMusterDispose_RCPK.Text)
            {
                rbSatrapRemainMusterDispose_RCPK.Checked = true;
            }
            else if (m_strSatarpRemainMusterDispose == rbSatrapRemainMusterDispose_BF.Text)
            {
                rbSatrapRemainMusterDispose_BF.Checked = true;
            }

            if (m_strSatarpAffirmResult == rbSatarpAffirmResult_PPAP.Text)
            {
                rbSatarpAffirmResult_PPAP.Checked = true;
            }
            else if (m_strSatarpAffirmResult == rbSatarpAffirmResult_CXTJ.Text)
            {
                rbSatarpAffirmResult_CXTJ.Checked = true;
            }
            else if (m_strSatarpAffirmResult == rbSatarpAffirmResult_CXSY.Text)
            {
                rbSatarpAffirmResult_CXSY.Checked = true;
            }
            else if (m_strSatarpAffirmResult == rbSatarpAffirmResult_PL.Text)
            {
                rbSatarpAffirmResult_PL.Checked = true;
            }

            if (m_strEngineerRemainMusterDispose == rbEngineerRemainMusterDispose_RYPK.Text)
            {
                rbEngineerRemainMusterDispose_RYPK.Checked = true;
            }
            else if (m_strEngineerRemainMusterDispose == rbEngineerRemainMusterDispose_RCPK.Text)
            {
                rbEngineerRemainMusterDispose_RCPK.Checked = true;
            }
            else if (m_strEngineerRemainMusterDispose == rbEngineerRemainMusterDispose_BF.Text)
            {
                rbEngineerRemainMusterDispose_BF.Checked = true;
            }

            if (m_strEngineerAffirmResult == rbEngineerAffirmResult_PPAP.Text)
            {
                rbEngineerAffirmResult_PPAP.Checked = true;
            }
            else if (m_strEngineerAffirmResult == rbEngineerAffirmResult_CXTJ.Text)
            {
                rbEngineerAffirmResult_CXTJ.Checked = true;
            }
            else if (m_strEngineerAffirmResult == rbEngineerAffirmResult_CXSY.Text)
            {
                rbEngineerAffirmResult_CXSY.Checked = true;
            }
            else if (m_strEngineerAffirmResult == rbEngineerAffirmResult_PL.Text)
            {
                rbEngineerAffirmResult_PL.Checked = true;
            }

            if (m_strScarpDisposeMind == rbScrapDisposeMind_TH.Text)
            {
                rbScrapDisposeMind_TH.Checked = true;
            }
            else if (m_strScarpDisposeMind == rbScrapDisposeMind_YDBF.Text)
            {
                rbScrapDisposeMind_YDBF.Checked = true;
            }
        }

        /// <summary>
        /// 检查信息
        /// </summary>
        /// <param name="strFlag">单据状态</param>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckMessage(string strFlag)
        {
            if (strFlag == "新建单据")
            {
                if (cmbMusterType.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择样品类别");
                    return false;
                }
                else if (chkPayment.Checked && txtOrderForm.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择订单号");
                    return false;
                }
                else if (txtCode.Tag == null)
                {
                    MessageDialog.ShowPromptMessage("请选择物品");
                    return false;
                }
                else if (numSendCount.Value == 0)
                {
                    MessageDialog.ShowPromptMessage("送样数量必须大于0");
                    return false;
                }
                else if (txtProvider.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择供应商");
                    return false;
                }
                else if (txtVersion.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写版次号");
                    return false;
                }
                else if (txtProviderBatchNo.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请填写供方批次");
                    return false;
                }
                else if (txtBatchNo.Text.Trim().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("请填写批次号");
                    return false;
                }
            }

            if (strFlag == "等待仓管确认到货")
            {
                if (numMusterCount.Value == 0)
                {
                    MessageDialog.ShowPromptMessage("到货数量必须大于0");
                    return false;
                }
                else if (numMusterCount.Value > numSendCount.Value)
                {
                    MessageDialog.ShowPromptMessage("到货数不能大于送样数");
                    return false;
                }
            }

            if (strFlag == "等待检验")
            {
                if (txtCheckReport.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("检验报告编号不能为空");
                    return false;
                }
                else if (txtCheckResult.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("检验结果不能为空");
                    return false;
                }
                else if (cmbFeederReport.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择供方检验报告有或无");
                    return false;
                }
                else if (txtMusterPack.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写样品包装信息");
                    return false;
                }
            }

            if (strFlag == "等待确认检验信息")
            {
                if (txtSQEExplain.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写SQE说明");
                    return false;
                }
            }

            if (strFlag == "等待工艺工程师评审")
            {
                if (cmbCraftMusterCareful.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择是否进行了评审");
                    return false;
                }
                else if (cmbCraftMusterCarefulResult.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择评审结果");
                    return false;
                }
                else if (txtCraftMusterCarefulResultReport.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写评审结果说明");
                    return false;
                }
            }

            if (strFlag == "等待零件工程师评审")
            {
                if (cmbMusterCareful.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择是否进行了评审");
                    return false;
                }
                else if (cmbMusterCarefulResult.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请选择评审结果");
                    return false;
                }
                else if (txtMusterCarefulResultReport.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写评审结果说明");
                    return false;
                }
            }

            if (strFlag == "等待项目经理确认")
            {
                if (txtAffirmResult.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写确认结果");
                    return false;
                }
            }

            if (strFlag == "等待试验结果")
            {
                if (txtTestAssemblyNumber.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写试装总成编号");
                    return false;
                }
                else if (txtTestResult.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写试装结果，试验结果");
                    return false;
                }
                else if (txtEngineerMind.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写零件工程师的意见");
                    return false;
                }
                else if (m_strEngineerAffirmResult == "")
                {
                    MessageDialog.ShowPromptMessage("请选择样品确认结果");
                    return false;
                }
                else if (m_strEngineerRemainMusterDispose == "")
                {
                    MessageDialog.ShowPromptMessage("请选择剩余样品处理方式");
                    return false;
                }
            }

            if (strFlag == "等待主管确认")
            {
                if (cmbIsEligbility.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择样品是否合格");
                    return false;
                }
                else if (txtSatarpMind.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写主管意见");
                    return false;
                }
                else if (m_strSatarpAffirmResult == "")
                {
                    MessageDialog.ShowPromptMessage("请选择样品确认结果");
                    return false;
                }
                else if (m_strSatarpRemainMusterDispose == "")
                {
                    MessageDialog.ShowPromptMessage("请选择剩余样品处理方式");
                    return false;
                }
            }

            if (strFlag == "等待SQE处理")
            {
                if (numMusterCount.Value - numScrapCount.Value - numEjectableCount.Value < 0)
                {
                    MessageDialog.ShowPromptMessage("【入库数】/【报废数】+【耗用数】+ 【退货数】 = 【样品数】，请重新调整【退货数】");
                    return false;
                }

                if (numEjectableCount.Value > 0 && m_strScarpDisposeMind == "")
                {
                    MessageDialog.ShowPromptMessage("请在退货数大于0的情况下选择处理方式");
                    return false;
                }
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            GetRbMessage();

            if (!CheckMessage(lblBillStatus.Text))
            {
                return;
            }

            GetAllMessage();

            if (!m_serverMuster.SaveInfo(m_lnqMuster, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");
                this.Close();
            }
        }

        private void btnFindOrderForm_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetOrderFormInfoDialog(CE_BillTypeEnum.样品确认申请单);

            if (DialogResult.OK == form.ShowDialog())
            {
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";
                txtVersion.Text = "";
                txtOrderForm.Text = form.GetDataItem("订单号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();

                View_B_OrderFormInfo lnqOrderForm = m_serverOrderFormInfo.GetOrderFormInfo(txtOrderForm.Text);

                View_B_BargainInfo lnqBargain = m_serverBargainInfo.GetBargainInfo(lnqOrderForm.合同号);

                chkIsOutsourcing.Checked = lnqBargain.是否委外合同;
            }
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "新建单据")
            {
                return;
            }

            FormQueryInfo form;

            if (chkPayment.Checked)
            {
                if (txtOrderForm.Text.Length == 0)
                {
                    txtOrderForm.Focus();
                    MessageDialog.ShowPromptMessage(@"请先选择订单/合同号后再进行此操作！");
                    return;
                }

                form = QueryInfoDialog.GetOrderFormGoodsDialog(txtOrderForm.Text, true);
            }
            else
            {
                form = QueryInfoDialog.GetPlanCostGoodsDialog(true);
            }

            if (form == null || form.ShowDialog() != DialogResult.OK)
            {
                return;
            }
            else
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();

                View_F_GoodsPlanCost tempGoodsInfo = UniversalFunction.GetGoodsInfo(txtCode.Text, txtName.Text, txtSpec.Text);
                txtCode.Tag = tempGoodsInfo.序号;

                DataRow dr = m_serverBom.GetBomInfo(txtCode.Text.Trim(), txtName.Text.Trim());

                if (dr == null)
                {
                    txtVersion.Text = "";
                }
                else
                {
                    txtVersion.Text = dr["Version"].ToString();
                }
            }
        }

        private void chkPayment_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPayment.Checked)
            {
                txtOrderForm.Enabled = true;
                btnFindOrderForm.Enabled = true;
                txtCode.Text = "";
                txtName.Text = "";
                txtSpec.Text = "";
                txtCode.Tag = null;
                btnProvider.Enabled = false;
            }
            else
            {
                txtOrderForm.Enabled = false;
                btnFindOrderForm.Enabled = false;
                btnProvider.Enabled = true;
            }

            txtProvider.Text = "";
            txtOrderForm.Text = "";
            txtProvider.Text = "";
        }

        private void btnProvider_Click(object sender, EventArgs e)
        {
            m_formProvider = QueryInfoDialog.GetProviderInfoDialog();

            if (m_formProvider.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Text = m_formProvider.GetStringDataItem("供应商编码");
            }
        }

        private void numEjectableCount_ValueChanged(object sender, EventArgs e)
        {
            decimal dcCount = numMusterCount.Value - numScrapCount.Value - numEjectableCount.Value;

            if (dcCount < 0)
            {
                MessageDialog.ShowPromptMessage("【入库数】/【报废数】+【耗用数】+ 【退货数】 = 【样品数】，请重新调整【退货数】");
                numEligibleCount.Value = 0;
                return;
            }
            else
            {
                numEligibleCount.Value = dcCount;
            }
        }

        private void 样品确认申请单清单_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void chkIsOutsourcing_CheckedChanged(object sender, EventArgs e)
        {
            chkIsIncludeRawMaterial.Enabled = chkIsOutsourcing.Checked;
            chkIsIncludeRawMaterial.Checked = chkIsOutsourcing.Checked;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            m_lnqMuster = m_serverMuster.GetBill(lblBillNo.Text);
            this.Close();
        }

        private void btnBatchNo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetWorkShopBatchNoInfo(Convert.ToInt32(txtCode.Tag), m_lnqWSCode.WSCode);

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtBatchNo.Text = (string)form.GetDataItem("批次号");
            }
        }

        private void txtChecker_Enter(object sender, EventArgs e)
        {
            txtChecker.StrEndSql = " and 部门 = '检查科'";
        }
    }
}
