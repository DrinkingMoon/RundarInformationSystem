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

namespace Expression
{
    /// <summary>
    /// 营销入库明细界面
    /// </summary>
    public partial class 营销入库明细单 : Form
    {
        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 营销入库服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 部门服务
        /// </summary>
        IDepartmentServer m_findDepartmentServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_findPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 单据明细表
        /// </summary>
        DataTable m_dtMxRK = new DataTable();

        /// <summary>
        /// 单据状态
        /// </summary>
        string m_strDJZTFlag;

        /// <summary>
        /// 入库单据
        /// </summary>
        DataRow m_drZdRK;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 单据ID
        /// </summary>
        int m_intDJID = 0;

        /// <summary>
        /// 单据号
        /// </summary>
        private string m_strDJH;

        public string StrDJH
        {
            get { return m_strDJH; }
            set { m_strDJH = value; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        //string m_strErr;

        /// <summary>
        /// 车间管理基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 车间管理信息
        /// </summary>
        WS_WorkShopCode m_lnqWSCode = new WS_WorkShopCode();

        /// <summary>
        /// 构造函数
        /// </summary>
        public 营销入库明细单(int billID, AuthorityFlag authFlag)
        {
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.营销入库单, m_findSellIn);

            m_billMessageServer.BillType = "营销入库单";

            m_intDJID = billID;
            InitializeComponent();

            FaceAuthoritySetting.SetEnable(this.Controls, authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip, authFlag);

            this.toolStrip.Visible = true;
            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
            cmbType.DataSource = m_serverBom.GetAssemblyTypeList();
                //车型代号 UniversalFunction.GetTCUCarModelInfo().Select(k => k.CarModelNo).ToList();
            cmbType.SelectedIndex = -1;
        }

        /// <summary>
        /// 设置版本号
        /// </summary>
        /// <param name="version">TCU版本号</param>
        void SetTypeAndVersion(string version)
        {
            if (version.Trim().Length == 0)
            {
                cmbType.SelectedIndex = -1;
                numVersion.Value = 0;
                return;
            }

            string[] arrayData = version.Split(new char[] { ' ' });

            if (arrayData == null || arrayData.Length != 2)
            {
                MessageDialog.ShowErrorMessage(string.Format("[{0}] 版本格式不正确", version));
                return;
            }

            string productType = arrayData[0];

            numVersion.Value = Convert.ToDecimal(arrayData[1]);
        }

        /// <summary>
        /// 控制按钮显示
        /// </summary>
        /// <param name="strPersonnelType">显示方式</param>
        public void ContrlForm(string strPersonnelType)
        {
            switch (strPersonnelType)
            {
                case "编制人":
                    btnAffirm.Visible = false;
                    btnSh.Visible = false;
                    break;
                case "审核人":
                    btnAdd.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnAffirm.Visible = false;
                    break;
                case "仓管员":
                    btnAdd.Visible = false;
                    btnUpdate.Visible = false;
                    btnDelete.Visible = false;
                    btnSave.Visible = false;
                    btnSh.Visible = false;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 界面初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 营销入库明细单_Load(object sender, EventArgs e)
        {
            DataTable dt = m_findSellIn.GetBill(m_strDJH, m_intDJID);

            m_drZdRK = dt.NewRow();

            if (m_intDJID != 0)
            {
                View_HR_Personnel lnqPersonnel = m_findPersonnel.GetPersonnelInfo(dt.Rows[0]["LRRY"].ToString());

                m_strDJZTFlag = dt.Rows[0]["DJZT_Flag"].ToString();
                lbUserName.Text = lnqPersonnel.姓名.ToString();
                lbUserName.Tag = lnqPersonnel.工号.ToString();
                lbKS.Text = lnqPersonnel.部门名称.ToString();
                lbKS.Tag = lnqPersonnel.部门编码.ToString();
                m_dtMxRK = m_findSellIn.GetList(m_intDJID);
                tbsDept.Tag = dt.Rows[0]["ObjectDept"].ToString();
                txtSellID.Text = dt.Rows[0]["DJH"].ToString();

                View_Department linVdepartment = m_findDepartmentServer.GetDepartments(dt.Rows[0]["ObjectDept"].ToString());

                tbsDept.Text = linVdepartment.部门名称;
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                txtRemarkAll.Text = dt.Rows[0]["Remark"].ToString();
                cmbRKFS.Text = dt.Rows[0]["YWFS"].ToString();
                cmbStorage.Text = UniversalFunction.GetStorageName(dt.Rows[0]["StorageID"].ToString());
                m_lnqWSCode = m_serverWSBasic.GetWorkShopCodeInfo(tbsDept.Tag.ToString());
            }
            else
            {
                m_strDJZTFlag = "已保存";
                lbUserName.Text = BasicInfo.LoginName;
                lbUserName.Tag = BasicInfo.LoginID;
                lbKS.Text = BasicInfo.DeptName;
                lbKS.Tag = BasicInfo.DeptCode;

                WS_WorkShopCode tempWorkCodeInfo = m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID);

                if (tempWorkCodeInfo != null)
                {
                    tbsDept.Text = tempWorkCodeInfo.WSName;
                    tbsDept.Tag = tempWorkCodeInfo.DeptCode;
                }

                txtSellID.Text = m_billNoControl.GetNewBillNo();

                CreateDateTableStyle();
            }

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                txtBatchNo.Enabled = false;
            }

            if (m_strDJZTFlag != "已保存" && m_strDJZTFlag != "")
            {
                cmbRKFS.Enabled = false;
                tbsDept.Enabled = false;
                cmbStorage.Enabled = false;

                if (UniversalFunction.CheckStorageAndPersonnel(dt.Rows[0]["StorageID"].ToString()) == false)
                {
                    btnAffirm.Visible = false;
                }
            }

            OperationbtnCheckIsVisible(m_dtMxRK);

            //DataRow drDept = m_serverDepartment.GetPersonnelAffiliatedTopFunction(lbUserName.Text).Rows[0];

            //tbsDept.Enabled = false;
            //tbsDept.Text = drDept["DepartmentName"].ToString();
            //tbsDept.Tag = drDept["DepartmentCode"].ToString();
            dgv_Main.DataSource = m_dtMxRK;

            m_strDJH = txtSellID.Text.Trim();
        }

        private void OperationbtnCheckIsVisible(DataTable dtmessage)
        {
            for (int i = 0; i < dtmessage.Rows.Count; i++)
            {
                if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(dtmessage.Rows[i]["CPID"].ToString()),
                    GlobalObject.CE_GoodsAttributeName.CVT)))
                {
                    btnCheck.Visible = false;
                }
            }
        }

        /// <summary>
        /// 完成部门信息检索
        /// </summary>
        private void tbsDept_OnCompleteSearch()
        {
            tbsDept.Tag = tbsDept.DataResult["部门编码"].ToString();
        }

        /// <summary>
        /// 完成物品信息检索
        /// </summary>
        private void tbsGoods_OnCompleteSearch()
        {
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = tbsGoods.DataResult["物品类别"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            tbsGoods.Tag = tbsGoods.DataResult["序号"].ToString();
            lbUnit.Text = tbsGoods.DataResult["单位"].ToString();
            txtSpce.Tag = "SYS_JLRD";

            if (tbsDept.Tag != null)
            {
                m_lnqWSCode = m_serverWSBasic.GetWorkShopCodeInfo(tbsDept.Tag.ToString());
                txtBatchNo.Enabled = m_lnqWSCode == null ? false : true;
                numCount.Focus();
            }
            else
            {
                tbsDept.Focus();
            }
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtMxRK.Columns.Add("CPID");
            m_dtMxRK.Columns.Add("GoodsCode");
            m_dtMxRK.Columns.Add("GoodsName");
            m_dtMxRK.Columns.Add("Spec");
            m_dtMxRK.Columns.Add("Depot");
            m_dtMxRK.Columns.Add("BatchNo");
            m_dtMxRK.Columns.Add("UnitPrice");
            m_dtMxRK.Columns.Add("Count");
            m_dtMxRK.Columns.Add("Unit");
            m_dtMxRK.Columns.Add("Price");
            m_dtMxRK.Columns.Add("Remark");
            m_dtMxRK.Columns.Add("Provider");
            m_dtMxRK.Columns.Add("Version");
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            if (!m_findSellIn.DeleteProductCodeInfo(m_strDJH, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            this.Close();
        }

        /// <summary>
        /// 窗体清空
        /// </summary>
        private void ClearDate()
        {
            txtSpce.Text = "";
            numCount.Value = 0;
            txtBatchNo.Text = "";
            txtGoodsCode.Text = "";
            txtRemark.Text = "";
            numUnitPrice.Value = 0;
            lbUnit.Text = "";
            tbsGoods.Text = "";
            tbsGoods.Tag = -1;
            cmbType.SelectedIndex = -1;
            numVersion.Value = 0;
        }

        /// <summary>
        /// 添加事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckForm())
            {
                if (CheckSameGoods())
                {
                    DataRow dr = m_dtMxRK.NewRow();

                    dr["CPID"] = Convert.ToInt32(tbsGoods.Tag);
                    dr["GoodsCode"] = txtGoodsCode.Text;
                    dr["GoodsName"] = tbsGoods.Text;
                    dr["Spec"] = txtSpce.Text;
                    dr["Depot"] = txtGoodsCode.Tag.ToString();
                    dr["BatchNo"] = txtBatchNo.Text;
                    dr["UnitPrice"] = numUnitPrice.Value;
                    dr["Count"] = numCount.Value;
                    dr["Unit"] = lbUnit.Text;
                    dr["Price"] = Math.Round(numCount.Value * numUnitPrice.Value, 4);
                    dr["Remark"] = txtRemark.Text;
                    dr["Provider"] = txtSpce.Tag.ToString();

                    IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

                    if (Convert.ToBoolean( UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(tbsGoods.Tag), CE_GoodsAttributeName.TCU)))
                    {
                        dr["Version"] = cmbType.Text.Trim() + " " + numVersion.Value.ToString();
                    }
                    else
                    {
                        dr["Version"] = "";
                    }

                    m_dtMxRK.Rows.Add(dr);

                    dgv_Main.DataSource = m_dtMxRK;

                    if (m_dtMxRK.Rows.Count > 0)
                    {
                        txtPrice.Text = (Convert.ToDecimal(txtPrice.Text) + Convert.ToDecimal(dr["Price"])).ToString();
                    }
                    else
                    {
                        txtPrice.Text = dr["Prcie"].ToString();
                    }

                    ClearDate();
                }
            }
        }

        /// <summary>
        /// 检查同种物品
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckSameGoods()
        {
            if (m_dtMxRK == null || m_dtMxRK.Rows.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < m_dtMxRK.Rows.Count; i++)
            {
                if (m_dtMxRK.Rows[i]["GoodsCode"].ToString().Trim() == txtGoodsCode.Text.Trim()
                    && m_dtMxRK.Rows[i]["GoodsName"].ToString().Trim() == tbsGoods.Text.Trim()
                    && m_dtMxRK.Rows[i]["Spec"].ToString().Trim() == txtSpce.Text.Trim()
                    && m_dtMxRK.Rows[i]["Depot"].ToString().Trim() == txtGoodsCode.Tag.ToString().Trim()
                    && m_dtMxRK.Rows[i]["Provider"].ToString().Trim() == txtSpce.Tag.ToString().Trim()
                    && m_dtMxRK.Rows[i]["BatchNo"].ToString().Trim() == txtBatchNo.Text.Trim()
                    )
                {
                    MessageBox.Show("不能添加相同产品", "提示");
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 窗体检查
        /// </summary>
        /// <returns>检测通过返回True，否则返回False</returns>
        private bool CheckForm()
        {

            if (tbsGoods.Text.Trim() == "" ||
                tbsGoods.Tag == null || tbsGoods.Tag.ToString() == ""
                || tbsGoods.Tag.ToString() == "-1")
            {
                MessageBox.Show("请选择产品", "提示");
                tbsGoods.Focus();
                return false;
            }

            if (cmbRKFS.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择入库方式");
                cmbRKFS.Focus();
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

            if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32( tbsGoods.Tag.ToString()), CE_GoodsAttributeName.TCU))
                && (cmbType.Text.Trim() == "" || numVersion.Value == 0))
            {
                cmbType.Focus();
                MessageDialog.ShowPromptMessage("请选择所属型号及版本号");
                return false;
            }

            return true;
        }

        /// <summary>
        /// txtUnitPrice的KeyPress事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnitPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// txtCount的KeyPress事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 8 && !Char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// 更新事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            txtPrice.Text = (Convert.ToDecimal(txtPrice.Text) - Convert.ToDecimal(
                m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Price"])).ToString();
            m_dtMxRK.Rows.RemoveAt(dgv_Main.CurrentRow.Index);
            btnAdd_Click(sender, e);
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_dtMxRK.Rows.Count != 0)
            {
                txtPrice.Text = (Convert.ToDecimal(txtPrice.Text) - Convert.ToDecimal(
                    m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Price"])).ToString();

                m_dtMxRK.Rows.RemoveAt(dgv_Main.CurrentRow.Index);

                dgv_Main.DataSource = m_dtMxRK;
            }
            else
            {
                ClearDate();
            }
        }

        /// <summary>
        /// 保存事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbStorage.Text == "售后库房" && (cmbRKFS.Text == "生产入库" || cmbRKFS.Text == "生产返修入库"))
                {
                    MessageDialog.ShowPromptMessage("入库方式错误");
                    return;
                }

                if (m_strDJZTFlag == "已确认")
                {
                    return;
                }
                else if (tbsDept.Tag == null)
                {
                    MessageBox.Show("请选择入库部门", "提示");
                    tbsDept.Focus();
                    return;
                }
                else if (cmbRKFS.Text == "")
                {
                    MessageBox.Show("请选择入库方式", "提示");
                    cmbRKFS.Focus();
                    return;
                }

                DataTable dtTemp = (DataTable)dgv_Main.DataSource;

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (!m_serverProductCode.IsFitCount(Convert.ToInt32(dtTemp.Rows[i]["CPID"]),
                        (int)Convert.ToDecimal(dtTemp.Rows[i]["Count"]), m_strDJH))
                    {
                        MessageBox.Show("请对产品设置流水号,并保证产品数量与流水号数一致", "提示");
                        return;
                    }
                }

                //由于错误录入入库方式，而导致无法重新进行生产入库，经与李剑飞、财务李姝姝沟通后，禁用此功能 Modify by cjb 2019.2.21
                //if (cmbRKFS.Text == "生产入库")
                //{
                //    m_serverProductCode.IsExistProductStock(txtSellID.Text);
                //}

                DataTable dt = (DataTable)dgv_Main.DataSource;
                m_drZdRK["ID"] = m_intDJID.ToString();
                m_drZdRK["DJH"] = txtSellID.Text;
                m_drZdRK["ObjectDept"] = tbsDept.Tag.ToString();
                m_drZdRK["LRRY"] = BasicInfo.LoginID;
                m_drZdRK["Date"] = ServerTime.Time.ToString();
                m_drZdRK["KFRY"] = "";
                m_drZdRK["Price"] = Convert.ToDecimal(txtPrice.Text);
                m_drZdRK["SHRY"] = "";
                m_drZdRK["Remark"] = txtRemarkAll.Text;
                m_drZdRK["YWFS"] = cmbRKFS.Text;
                m_drZdRK["JYRY"] = "";
                m_drZdRK["StorageID"] = UniversalFunction.GetStorageID(cmbStorage.Text);
                m_drZdRK["LRKS"] = BasicInfo.DeptCode;

                if (m_findSellIn.UpdateBill(dt, m_drZdRK, CE_MarketingType.入库.ToString(), out m_err))
                {
                    MessageBox.Show("保存成功", "提示");

                    m_billMessageServer.DestroyMessage(txtSellID.Text);
                    m_billMessageServer.SendNewFlowMessage(txtSellID.Text, string.Format("【入库方式】：{0}  【库房】：{1}   ※※※ 等待【主管】处理", cmbRKFS.Text, cmbStorage.Text),
                        BillFlowMessage_ReceivedUserType.角色,
                        m_billMessageServer.GetDeptDirectorRoleName(BasicInfo.DeptCode).ToList());
                    m_intDJID = 1;
                    this.Close();
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgv_Main_Click(object sender, EventArgs e)
        {
            if (dgv_Main.CurrentRow == null)
            {
                return;
            }

            if (m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["CPID"].ToString() != "")
            {
                tbsGoods.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["GoodsName"].ToString();
                tbsGoods.Tag = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["CPID"].ToString();
                numCount.Value = Convert.ToDecimal( m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Count"]);
                txtSpce.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Spec"].ToString();
                txtRemark.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Remark"].ToString();
                numUnitPrice.Value = Convert.ToDecimal( m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["UnitPrice"]);
                txtBatchNo.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString();
                lbUnit.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Unit"].ToString();
                txtGoodsCode.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["GoodsCode"].ToString();
                txtGoodsCode.Tag = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Depot"].ToString();
                txtSpce.Tag = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString();

                SetTypeAndVersion(m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Version"].ToString());
            }
        }

        /// <summary>
        /// 审核事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSh_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_strDJZTFlag == "已保存")
                {
                    if (m_findSellIn.AuditingBill(m_intDJID, txtRemarkAll.Text, out m_err))
                    {
                        MessageBox.Show("审核成功!", "提示");

                        m_billMessageServer.PassFlowMessage(txtSellID.Text, 
                            string.Format("【入库方式】：{0}  【库房】：{1}   ※※※ 等待【质检员】处理", cmbRKFS.Text, cmbStorage.Text),
                                CE_RoleEnum.质量工程师.ToString(), true);
                        this.Close();
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("请重新确认单据状态!", "提示");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowErrorMessage(ex.Message);
                return;
            }
        }

        /// <summary>
        /// 仓管确认事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAffirm_Click(object sender, EventArgs e)
        {
            if (m_intDJID != 0 && m_strDJZTFlag == "已检验")
            {
                DataTable dtTemp = (DataTable)dgv_Main.DataSource;

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (!m_serverProductCode.IsFitCount(Convert.ToInt32(dtTemp.Rows[i]["CPID"]),
                        Convert.ToInt32(dtTemp.Rows[i]["Count"]), m_strDJH))
                    {
                        MessageBox.Show("请对产品设置流水号,并保证产品数量与流水号数一致", "提示");
                        return;
                    }
                }

                if (m_findSellIn.IsExistAbnomalProductCode(txtSellID.Text.Trim()))
                {
                    MessageDialog.ShowPromptMessage("此单据存在异常箱号，无法入库，请通过【特殊放行】");
                    return;
                }

                if (m_findSellIn.AffrimBill(m_intDJID, CE_MarketingType.入库, (DataTable)dgv_Main.DataSource, out m_err))
                {
                    MessageBox.Show("确认完毕！", "提示");


                    #region 发送知会消息

                    List<string> noticeRoles = new List<string>();

                    string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    m_findSellIn.GetBill(m_strDJH, m_intDJID).Rows[0]["LRRY"].ToString()).Rows[0]["DepartmentCode"].ToString();

                    noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));
                    noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                    noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString());

                    m_billMessageServer.EndFlowMessage(txtSellID.Text,
                        string.Format("{0} 号营销入库单已经处理完毕", txtSellID.Text),
                        noticeRoles, null);

                    #endregion 发送知会消息

                    m_billNoControl.UseBill(txtSellID.Text);
                    this.Close();
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
            }
            else if (m_intDJID != 0)
            {
                MessageBox.Show("请重新确认单据状态!", "提示");
                return;
            }
            else
            {
                MessageBox.Show("单据ID无效，请重新确认!", "提示");
                return;
            }
        }

        private void dgv_Main_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_Main.CurrentRow == null)
            {
                return;
            }

            BarCodeInfo tempInfo = new BarCodeInfo();

            tempInfo.BatchNo = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString();
            tempInfo.Count = Convert.ToDecimal(m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Count"]);
            tempInfo.GoodsCode = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["GoodsCode"].ToString();
            tempInfo.GoodsID = Convert.ToInt32(m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["CPID"]);
            tempInfo.GoodsName = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["GoodsName"].ToString();
            tempInfo.Remark = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Remark"].ToString();
            tempInfo.Spec = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Spec"].ToString();

            CE_BusinessType tempType = CE_BusinessType.库房业务;

            Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

            if (tbsDept.Text.Trim().Length == 0 || tbsDept.Tag == null)
            {
                throw new Exception("请选择【出货车间】");
            }

            WS_WorkShopCode tempWSCode = serverWSBasic.GetWorkShopCodeInfo(tbsDept.Tag.ToString());

            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            tempDic.Add(UniversalFunction.GetStorageID(cmbStorage.Text), CE_MarketingType.入库.ToString());

            if (tempWSCode != null)
            {
                tempType = CE_BusinessType.综合业务;
                tempDic.Add(tempWSCode.WSCode, CE_SubsidiaryOperationType.营销入库.ToString());
            }

            产品编号 form = new 产品编号(tempInfo, tempType, txtSellID.Text,
                !(m_strDJZTFlag != "已保存" && m_strDJZTFlag != ""), tempDic);

            form.BlAfterServer = cmbRKFS.Text.Contains("售后返修");

            form.ShowDialog();

            if (form.IntCount != 0)
            {
                dgv_Main.CurrentRow.Cells["Count"].Value = form.IntCount;
            }
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (m_strDJZTFlag == "已审核")
            {
                if (m_findSellIn.ExamineBill(txtSellID.Text, txtRemarkAll.Text, out m_err))
                {
                    MessageBox.Show("检验通过！", "提示");

                    m_billMessageServer.PassFlowMessage(txtSellID.Text, string.Format("【入库方式】：{0}  【库房】：{1}   ※※※ 等待【仓管】处理", cmbRKFS.Text, cmbStorage.Text),
                            m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }
        }

        private void 营销入库明细单_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void rdbProduct_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbProduct.Checked)
            {
                tbsGoods.FindItem = TextBoxShow.FindType.营销物品;
            }
            else
            {
                tbsGoods.FindItem = TextBoxShow.FindType.所有物品;
            }
        }

        private void tbsDept_Enter(object sender, EventArgs e)
        {
            tbsDept.StrEndSql = " and DeptCode in (select DeptCode from WS_WorkShopCode)";
        }

        private void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            txtBatchNo.StrEndSql = " and 物品ID = " 
                + Convert.ToInt32(tbsGoods.Tag) + " and 车间代码 = '"
                + m_lnqWSCode.WSCode + "'";
        }

        private void numCount_Click(object sender, EventArgs e)
        {
            numCount.Focus();
            numCount.Select(0, numCount.Text.Length);
        }
    }
}
