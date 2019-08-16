using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;
using System;

namespace Expression
{
    public partial class 基础物品信息设置界面 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 物品ID
        /// </summary>
        private int? m_goodsID = null;

        public int? GoodsID
        {
            get { return m_goodsID; }
            set { m_goodsID = value; }
        }

        /// <summary>
        /// 属性记录对象
        /// </summary>
        List<F_GoodsAttributeRecord> m_lstRecord = new List<F_GoodsAttributeRecord>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 物品信息
        /// </summary>
        F_GoodsPlanCost m_goodsInfo = new F_GoodsPlanCost();

        /// <summary>
        /// 替换件信息列表
        /// </summary>
        List<View_F_GoodsReplaceInfo> m_lstReplaceInfo = new List<View_F_GoodsReplaceInfo>();

        List<F_ProductWaterCode> m_lstWaterCode = new List<F_ProductWaterCode>();

        /// <summary>
        /// 毛坯对应成品信息列表
        /// </summary>
        List<View_F_GoodsBlankToProduct> m_lstBlankToProductInfo = new List<View_F_GoodsBlankToProduct>();

        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStock = ServerModule.ServerModuleFactory.GetServerModule<IStoreServer>();

        public 基础物品信息设置界面(int? goodsID, bool isSave)
        {
            InitializeComponent();

            m_goodsID = goodsID;

            if (goodsID == null)
            {
                MessageDialog.ShowPromptMessage("【物品信息】异常,请退出当前界面，再试");
                this.Close();
            }
            else
            {
                lbStock.Text = m_serverStock.GetGoodsStock((int)goodsID).ToString();
                m_goodsInfo = m_serverGoods.GetGoodsInfo((int)goodsID);
                m_lstRecord = m_serverGoods.GetGoodsAttirbuteRecordList((int)m_goodsID);
            }

            StapleInfo.InitUnitComboBox(cmbUnit);

            dataGridViewBlankToProduct.DataSource = new BindingList<View_F_GoodsBlankToProduct>(m_lstBlankToProductInfo);
            dataGridViewReplace.DataSource = new BindingList<View_F_GoodsReplaceInfo>(m_lstReplaceInfo);
            dataGridViewWaterCode.DataSource = new BindingList<F_ProductWaterCode>(m_lstWaterCode);

            ShowInfo(m_goodsInfo, m_lstRecord);

            if (isSave)
            {
                ShowRightControl();
            }
            else
            {
                btnSave.Visible = isSave;
            }
        }

        /// <summary>
        /// 控件控制
        /// </summary>
        void ShowRightControl()
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.基础物品管理员_生管.ToString()))
            {
                pl_SG.Enabled = true;

                txtCode.Enabled = true;
                txtName.Enabled = true;
                txtSpec.Enabled = true;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.基础物品管理员_财务.ToString()))
            {
                pl_CW.Enabled = true;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.基础物品管理员_采购.ToString()))
            {
                pl_CG.Enabled = true;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.基础物品管理员_工艺.ToString()))
            {
                pl_GY.Enabled = true;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.基础物品管理员_技术.ToString()))
            {
                pl_JS.Enabled = true;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.基础物品管理员_质量.ToString()))
            {
                pl_ZK.Enabled = true;
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
            {
                pl_Sys.Enabled = true;
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="listRecord">信息列表</param>
        void ShowInfo(F_GoodsPlanCost goodsInfo, List<F_GoodsAttributeRecord> listRecord)
        {
            if (goodsInfo == null)
            {
                return;
            }

            View_F_GoodsPlanCost goodsView = UniversalFunction.GetGoodsInfo(goodsInfo.ID);

            txtCode.Tag = m_goodsID;
            txtCode.Text = goodsView.图号型号;
            txtName.Text = goodsView.物品名称;
            txtSpec.Text = goodsView.规格;
            chbIsDisable.Checked = m_goodsInfo.IsDisable;
            txtDepot.Text = goodsView.物品类别名称;
            txtDepot.Tag = goodsView.物品类别;
            cmbUnit.Text = goodsView.单位;
            cmbUnit.Tag = goodsView.单位ID;

            if (listRecord == null)
            {
                return;
            }

            foreach (F_GoodsAttributeRecord record in listRecord)
            {
                if (record.AttributeValue == null)
                {
                    continue;
                }

                F_GoodsAttribute attribute = m_serverGoods.GetGoodsAttirbute(record.AttributeID);
                CE_GoodsAttributeName attributeName = 
                    GlobalObject.GeneralFunction.StringConvertToEnum<CE_GoodsAttributeName>(attribute.AttributeName);

                bool valuesFlag = false;
                Boolean.TryParse(record.AttributeValue, out valuesFlag);

                switch (attributeName)
                {
                    case CE_GoodsAttributeName.生产耗用:
                        chbProductionUse.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.防锈期:
                        chbRustLife.Checked = true;
                        numRustLife.Value = Convert.ToDecimal(record.AttributeValue);
                        break;
                    case CE_GoodsAttributeName.保质期:
                        chbShelfLife.Checked = true;
                        break;
                    case CE_GoodsAttributeName.安全库存:
                        chbSafeStock.Checked = true;
                        numSafeStock.Value = Convert.ToDecimal(record.AttributeValue);
                        break;
                    case CE_GoodsAttributeName.最高库存:
                        chbTopStock.Checked = true;
                        numTopStock.Value = Convert.ToDecimal(record.AttributeValue);
                        break;
                    case CE_GoodsAttributeName.毛坯:
                        chbBlank.Checked = valuesFlag;
                        m_lstBlankToProductInfo = m_serverGoods.GetBlankToProductListInfo(record.AttributeRecordID);
                        dataGridViewBlankToProduct.DataSource = new BindingList<View_F_GoodsBlankToProduct>(m_lstBlankToProductInfo);
                        chbBlank_CheckedChanged(null, null);
                        break;
                    case CE_GoodsAttributeName.缺料计算考虑毛坯:
                        chbStarvingBlank.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.物料价值ABC分类:
                        cmbGoodsValueABC.Text = record.AttributeValue;
                        break;
                    case CE_GoodsAttributeName.技术等级ABC分类:
                        chbGoodsTechnologyABC.Checked = true;
                        cmbGoodsTechnologyABC.Text = record.AttributeValue;
                        break;
                    case CE_GoodsAttributeName.来料须依据检验结果入库:
                        chbNeedDetection.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.采购件:
                        chbPurchaseGoods.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.自制件:
                        chbSelfmade.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.委外加工:
                        chbOutsourceGoods.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.部件:
                        chbAssemblyUnit.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.零件:
                        chbPart.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.选配件:
                        chbOption.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.替换件:
                        chbReplace.Checked = valuesFlag;
                        m_lstReplaceInfo = m_serverGoods.GetReplaceListInfo(record.AttributeRecordID);
                        dataGridViewReplace.DataSource = new BindingList<View_F_GoodsReplaceInfo>(m_lstReplaceInfo);
                        break;
                    case CE_GoodsAttributeName.CVT:
                        chbCVT.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.TCU:
                        chbTCU.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.虚拟件:
                        chbVirtualPart.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.领用上限:
                        break;
                    case CE_GoodsAttributeName.整包发料:
                        chbPackageSending.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.刀具寿命:
                        num_CutterLife.Value = Convert.ToDecimal( record.AttributeValue);
                        break;
                    case CE_GoodsAttributeName.流水码:
                        chbWaterCode.Checked = valuesFlag;
                        m_lstWaterCode = m_serverGoods.GetWaterCodeListInfo(record.AttributeRecordID);
                        dataGridViewWaterCode.DataSource = new BindingList<F_ProductWaterCode>(m_lstWaterCode);
                        break;
                    case CE_GoodsAttributeName.厂商编码:
                        txtCSCODE.Text = record.AttributeValue == null ? "" : record.AttributeValue.ToString();
                        break;
                    case CE_GoodsAttributeName.停产:
                        chbIsEol.Checked = valuesFlag;
                        break;
                    case CE_GoodsAttributeName.装箱数:
                        num_PCS.Value = Convert.ToDecimal(record.AttributeValue);
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 收集信息
        /// </summary>
        void GetInfo()
        {
            m_lstRecord = new List<F_GoodsAttributeRecord>();
            F_GoodsAttributeRecord tempRecord = new F_GoodsAttributeRecord();
            m_goodsInfo = new F_GoodsPlanCost();
            string TrueStr = System.Boolean.TrueString;

            m_goodsInfo.ID = m_goodsID == null ? 0 : (int)m_goodsID;
            m_goodsInfo.GoodsName = txtName.Text;
            m_goodsInfo.GoodsCode = txtCode.Text;
            m_goodsInfo.Spec = txtSpec.Text;
            m_goodsInfo.UnitID = Convert.ToInt32(cmbUnit.SelectedValue);
            m_goodsInfo.GoodsType = txtDepot.Tag.ToString();
            m_goodsInfo.IsDisable = chbIsDisable.Checked;


            tempRecord = new F_GoodsAttributeRecord();

            tempRecord.AttributeID = (int)CE_GoodsAttributeName.物料价值ABC分类;
            tempRecord.AttributeValue = cmbGoodsValueABC.Text;

            m_lstRecord.Add(tempRecord);

            if (chbIsEol.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.停产;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbAssemblyUnit.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.部件;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbBlank.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.毛坯;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);

                m_lstBlankToProductInfo = ((BindingList<View_F_GoodsBlankToProduct>)dataGridViewBlankToProduct.DataSource).ToList();
            }

            if (chbCVT.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.CVT;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbGoodsTechnologyABC.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.技术等级ABC分类;
                tempRecord.AttributeValue = cmbGoodsTechnologyABC.Text;

                m_lstRecord.Add(tempRecord);
            }

            if (chbNeedDetection.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.来料须依据检验结果入库;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbOption.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.选配件;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbOutsourceGoods.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.委外加工;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbPart.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.零件;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbProductionUse.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.生产耗用;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbPackageSending.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.整包发料;
                tempRecord.AttributeValue = "B_GoodsLeastPackAndStock";
                tempRecord.IsHavingChildTable = true;

                m_lstRecord.Add(tempRecord);
            }

            if (chbPurchaseGoods.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.采购件;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbReplace.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.替换件;
                tempRecord.AttributeValue = "F_GoodsReplaceInfo";
                tempRecord.IsHavingChildTable = true;

                m_lstRecord.Add(tempRecord);
                m_lstReplaceInfo = ((BindingList<View_F_GoodsReplaceInfo>)dataGridViewReplace.DataSource).ToList();
            }

            if (chbRustLife.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.防锈期;
                tempRecord.AttributeValue = numRustLife.Value.ToString();

                m_lstRecord.Add(tempRecord);
            }

            if (chbSafeStock.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.安全库存;
                tempRecord.AttributeValue = numSafeStock.Value.ToString();

                m_lstRecord.Add(tempRecord);
            }

            if (chbSelfmade.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.自制件;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbShelfLife.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.保质期;
                tempRecord.AttributeValue = "KF_GoodsShelfLife";
                tempRecord.IsHavingChildTable = true;

                m_lstRecord.Add(tempRecord);
            }

            if (chbBlank.Checked && chbStarvingBlank.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.缺料计算考虑毛坯;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbTCU.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.TCU;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (chbTopStock.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.最高库存;
                tempRecord.AttributeValue = numTopStock.Value.ToString();

                m_lstRecord.Add(tempRecord);
            }

            if (chbVirtualPart.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.虚拟件;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
            }

            if (num_CutterLife.Value > 0)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.刀具寿命;
                tempRecord.AttributeValue = num_CutterLife.Value.ToString();

                m_lstRecord.Add(tempRecord);
            }

            if (num_PCS.Value > 0)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.装箱数;
                tempRecord.AttributeValue = num_PCS.Value.ToString();

                m_lstRecord.Add(tempRecord);

            }

            if (chbWaterCode.Checked)
            {
                tempRecord = new F_GoodsAttributeRecord();

                tempRecord.AttributeID = (int)CE_GoodsAttributeName.流水码;
                tempRecord.AttributeValue = TrueStr;

                m_lstRecord.Add(tempRecord);
                m_lstWaterCode = ((BindingList<F_ProductWaterCode>)dataGridViewWaterCode.DataSource).ToList();

                if (txtCSCODE.Text.Trim().Length > 0)
                {
                    tempRecord = new F_GoodsAttributeRecord();

                    tempRecord.AttributeID = (int)CE_GoodsAttributeName.厂商编码;
                    tempRecord.AttributeValue = txtCSCODE.Text;

                    m_lstRecord.Add(tempRecord);
                }
            }
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>有误返回False,否则返回True</returns>
        bool CheckInfo()
        {
            if (txtName.Text.Trim().Length == 0)
            {
                MessageDialog.ShowErrorMessage("请录入物品名称");
                return false;
            }

            if (txtDepot.Text.Trim().Length == 0 || txtDepot.Tag == null || txtDepot.Tag.ToString().Trim().Length == 0)
            {
                MessageDialog.ShowErrorMessage("请选择物品类别");
                return false;
            }

            if (cmbUnit.Text.Trim().Length == 0)
            {
                MessageDialog.ShowErrorMessage("请选择单位");
                return false;
            }

            return true;
        }

        private void btnDepot_Click(object sender, EventArgs e)
        {
            FormDepotType form = new FormDepotType();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtDepot.Tag = form.SelectedDepotType.仓库编码;
                txtDepot.Text = form.SelectedDepotType.仓库名称;
            }
        }

        private void chbSafeStock_CheckedChanged(object sender, EventArgs e)
        {
            numSafeStock.Enabled = chbSafeStock.Checked;
        }

        private void chbTopStock_CheckedChanged(object sender, EventArgs e)
        {
            numTopStock.Enabled = chbTopStock.Checked;
        }

        private void chbRustLife_CheckedChanged(object sender, EventArgs e)
        {
            numRustLife.Enabled = chbRustLife.Checked;
        }

        private void chbGoodsTechnologyABC_CheckedChanged(object sender, EventArgs e)
        {
            cmbGoodsTechnologyABC.Enabled = chbGoodsTechnologyABC.Checked;
        }

        #region Replace
        private void chbReplace_CheckedChanged(object sender, EventArgs e)
        {
            btnAdd_Replace.Enabled = chbReplace.Checked;
            btnDelete_Replace.Enabled = chbReplace.Checked;
            btnUpdate_Replace.Enabled = chbReplace.Checked;
        }

        private void txtReplaceCode_OnCompleteSearch()
        {
            if (txtGoodsCode_Replace.DataResult == null)
            {
                return;
            }

            txtGoodsCode_Replace.Text = txtGoodsCode_Replace.DataResult["图号型号"].ToString();
            txtGoodsCode_Replace.Tag = txtGoodsCode_Replace.DataResult["序号"];
            txtGoodsName_Replace.Text = txtGoodsCode_Replace.DataResult["物品名称"].ToString();
            txtSpec_Replace.Text = txtGoodsCode_Replace.DataResult["规格"].ToString();
        }

        private void btnAdd_Replace_Click(object sender, EventArgs e)
        {
            if (m_lstReplaceInfo.Where(k => k.物品ID == Convert.ToInt32(txtGoodsCode_Replace.Tag)).Count() == 0)
            {
                View_F_GoodsReplaceInfo replaceTemp = new View_F_GoodsReplaceInfo();
                replaceTemp.替换比率 = numRate_Replace.Value;
                replaceTemp.图号型号 = txtGoodsCode_Replace.Text;
                replaceTemp.规格 = txtSpec_Replace.Text;
                replaceTemp.物品ID = Convert.ToInt32(txtGoodsCode_Replace.Tag);
                replaceTemp.物品名称 = txtGoodsName_Replace.Text;

                m_lstReplaceInfo.Add(replaceTemp);

                dataGridViewReplace.DataSource = new BindingList<View_F_GoodsReplaceInfo>(m_lstReplaceInfo);
            }
        }

        private void btnUpdate_Replace_Click(object sender, EventArgs e)
        {
            if (dataGridViewReplace.CurrentRow == null)
            {
                return;
            }

            foreach (View_F_GoodsReplaceInfo item in m_lstReplaceInfo)
            {
                if (item.物品ID == Convert.ToInt32(dataGridViewReplace.CurrentRow.Cells["物品ID"].Value))
                {
                    item.替换比率 = numRate_Replace.Value;
                    item.图号型号 = txtGoodsCode_Replace.Text;
                    item.规格 = txtSpec_Replace.Text;
                    item.物品ID = Convert.ToInt32(txtGoodsCode_Replace.Tag);
                    item.物品名称 = txtGoodsName_Replace.Text;
                }
            }

            dataGridViewReplace.DataSource = new BindingList<View_F_GoodsReplaceInfo>(m_lstReplaceInfo);
        }

        private void btnDelete_Replace_Click(object sender, EventArgs e)
        {
            m_lstReplaceInfo.RemoveAll(t => t.物品ID == Convert.ToInt32(dataGridViewReplace.CurrentRow.Cells["物品ID"].Value));

            dataGridViewReplace.DataSource = new BindingList<View_F_GoodsReplaceInfo>(m_lstReplaceInfo);
        }

        private void dataGridViewReplace_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewReplace.CurrentRow == null)
            {
                return;
            }

            txtGoodsCode_Replace.Tag = Convert.ToInt32(dataGridViewReplace.CurrentRow.Cells["物品ID"].Value);
            txtGoodsCode_Replace.Text = dataGridViewReplace.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsName_Replace.Text = dataGridViewReplace.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec_Replace.Text = dataGridViewReplace.CurrentRow.Cells["规格"].Value.ToString();
            numRate_Replace.Value = Convert.ToDecimal(dataGridViewReplace.CurrentRow.Cells["替换比率"].Value);
        }

        #endregion

        #region BlankToProduct

        private void chbBlank_CheckedChanged(object sender, EventArgs e)
        {
            chbStarvingBlank.Enabled = chbBlank.Checked;
            btnAdd_Blank.Enabled = chbBlank.Checked;
            btnDelete_Blank.Enabled = chbBlank.Checked;
            btnUpdate_Blank.Enabled = chbBlank.Checked;
        }

        private void txtGoodsCode_Blank_OnCompleteSearch()
        {
            if (txtGoodsCode_Blank.DataResult == null)
            {
                return;
            }

            txtGoodsCode_Blank.Text = txtGoodsCode_Blank.DataResult["图号型号"].ToString();
            txtGoodsCode_Blank.Tag = txtGoodsCode_Blank.DataResult["序号"];
            txtGoodsName_Blank.Text = txtGoodsCode_Blank.DataResult["物品名称"].ToString();
            txtSpec_Blank.Text = txtGoodsCode_Blank.DataResult["规格"].ToString();
        }

        private void btnAdd_Blank_Click(object sender, EventArgs e)
        {
            if (m_lstBlankToProductInfo.Where(k => k.物品ID == Convert.ToInt32(txtGoodsCode_Blank.Tag)).Count() == 0)
            {
                View_F_GoodsBlankToProduct replaceTemp = new View_F_GoodsBlankToProduct();

                replaceTemp.图号型号 = txtGoodsCode_Blank.Text;
                replaceTemp.规格 = txtSpec_Blank.Text;
                replaceTemp.物品ID = Convert.ToInt32(txtGoodsCode_Blank.Tag);
                replaceTemp.物品名称 = txtGoodsName_Blank.Text;

                m_lstBlankToProductInfo.Add(replaceTemp);
                dataGridViewBlankToProduct.DataSource = new BindingList<View_F_GoodsBlankToProduct>(m_lstBlankToProductInfo);
            }
        }

        private void btnUpdate_Blank_Click(object sender, EventArgs e)
        {
            if (dataGridViewBlankToProduct.CurrentRow == null)
            {
                return;
            }

            foreach (View_F_GoodsBlankToProduct item in m_lstBlankToProductInfo)
            {
                if (item.物品ID == Convert.ToInt32(dataGridViewBlankToProduct.CurrentRow.Cells["物品ID1"].Value))
                {
                    item.图号型号 = txtGoodsCode_Blank.Text;
                    item.规格 = txtSpec_Blank.Text;
                    item.物品ID = Convert.ToInt32(txtGoodsCode_Blank.Tag);
                    item.物品名称 = txtGoodsName_Blank.Text;
                }
            }

            dataGridViewBlankToProduct.DataSource = new BindingList<View_F_GoodsBlankToProduct>(m_lstBlankToProductInfo);
        }

        private void btnDelete_Blank_Click(object sender, EventArgs e)
        {
            m_lstBlankToProductInfo.RemoveAll(t => t.物品ID == Convert.ToInt32(dataGridViewBlankToProduct.CurrentRow.Cells["物品ID1"].Value));
            dataGridViewBlankToProduct.DataSource = 
                new BindingList<View_F_GoodsBlankToProduct>(m_lstBlankToProductInfo);
        }

        private void dataGridViewBlankToProduct_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewBlankToProduct.CurrentRow != null)
            {
                int goodsID = Convert.ToInt32(dataGridViewBlankToProduct.CurrentRow.Cells["物品ID1"].Value);
                View_F_GoodsPlanCost goodsInfo = UniversalFunction.GetGoodsInfo(goodsID);

                txtGoodsCode_Blank.Text = goodsInfo.图号型号;
                txtGoodsName_Blank.Text = goodsInfo.物品名称;
                txtSpec_Blank.Text = goodsInfo.规格;
                txtGoodsCode_Blank.Tag = goodsID;
            }
        }

        #endregion

        #region WaterCode

        private void chbWaterCode_CheckedChanged(object sender, EventArgs e)
        {
            btnAdd_WaterCode.Enabled = chbWaterCode.Checked;
            btnDelete_WaterCode.Enabled = chbWaterCode.Checked;
            btnUpdate_WaterCode.Enabled = chbWaterCode.Checked;
        }

        private void btnAdd_WaterCode_Click(object sender, EventArgs e)
        {
            if (m_lstWaterCode.Where(k => k.BarcodeRole == txtBarcodeRole.Text && k.SteelSealRole == txtSteelSealRole.Text).Count() == 0)
            {
                F_ProductWaterCode temp = new F_ProductWaterCode();
                temp.BarcodeExample = txtBarcodeExample.Text;
                temp.BarcodeRole = txtBarcodeRole.Text;
                temp.SteelSealExample = txtSteelSealExample.Text;
                temp.SteelSealRole = txtSteelSealRole.Text;

                m_lstWaterCode.Add(temp);

                dataGridViewWaterCode.DataSource = new BindingList<F_ProductWaterCode>(m_lstWaterCode);
            }
        }

        private void btnUpdate_WaterCode_Click(object sender, EventArgs e)
        {
            if (dataGridViewWaterCode.CurrentRow == null)
            {
                return;
            }

            foreach (F_ProductWaterCode item in m_lstWaterCode)
            {
                if (item.BarcodeRole == dataGridViewWaterCode.CurrentRow.Cells["出厂条形码正则表达式"].Value.ToString()
                    && item.SteelSealRole == dataGridViewWaterCode.CurrentRow.Cells["箱体编码正则表达式"].Value.ToString())
                {
                    item.BarcodeExample = txtBarcodeExample.Text;
                    item.BarcodeRole = txtBarcodeRole.Text;
                    item.SteelSealExample = txtSteelSealExample.Text;
                    item.SteelSealRole = txtSteelSealRole.Text;
                }
            }

            dataGridViewWaterCode.DataSource = new BindingList<F_ProductWaterCode>(m_lstWaterCode);
        }

        private void btnDelete_WaterCode_Click(object sender, EventArgs e)
        {
            m_lstWaterCode.RemoveAll(t => t.BarcodeRole == dataGridViewWaterCode.CurrentRow.Cells["出厂条形码正则表达式"].Value.ToString()
                && t.SteelSealRole == dataGridViewWaterCode.CurrentRow.Cells["箱体编码正则表达式"].Value.ToString());

            dataGridViewWaterCode.DataSource = new BindingList<F_ProductWaterCode>(m_lstWaterCode);
        }

        private void dataGridViewWaterCode_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridViewWaterCode.CurrentRow != null)
            {
                txtBarcodeRole.Text = dataGridViewWaterCode.CurrentRow.Cells["出厂条形码正则表达式"].Value.ToString();
                txtBarcodeExample.Text = dataGridViewWaterCode.CurrentRow.Cells["出厂条形码示例"].Value.ToString();
                txtSteelSealRole.Text = dataGridViewWaterCode.CurrentRow.Cells["箱体编码正则表达式"].Value.ToString();
                txtSteelSealExample.Text = dataGridViewWaterCode.CurrentRow.Cells["箱体编码示例"].Value.ToString();
            }
        }
        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckInfo())
            {
                return;
            }

            GetInfo();

            if (!m_serverGoods.EditGoodsInfo(m_goodsInfo, m_lstRecord, m_lstReplaceInfo, m_lstBlankToProductInfo, m_lstWaterCode, out m_strErr))
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("保存成功");
                m_goodsID = m_serverGoods.GetGoodsID(m_goodsInfo.GoodsCode, m_goodsInfo.GoodsName, m_goodsInfo.Spec);
                this.Close();
            }
        }

        private void chbCVT_CheckedChanged(object sender, EventArgs e)
        {
            if (chbCVT.Checked || chbTCU.Checked)
            {
                chbIsEol.Enabled = true;
            }
            else
            {
                chbIsEol.Enabled = false;
                chbIsEol.Checked = false;
            }
        }

        private void chbTCU_CheckedChanged(object sender, EventArgs e)
        {
            if (chbTCU.Checked || chbCVT.Checked)
            {
                chbIsEol.Enabled = true;
            }
            else
            {
                chbIsEol.Enabled = false;
                chbIsEol.Checked = false;
            }
        }

    }
}
