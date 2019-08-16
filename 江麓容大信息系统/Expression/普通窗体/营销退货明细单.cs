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
    /// 营销退货明细界面
    /// </summary>
    public partial class 营销退货明细单 : Form
    {
        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;
        /// <summary>
        /// 部门服务
        /// </summary>
        IDepartmentServer m_findDepartmentServer = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 营销退货服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

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
        /// 退货单据
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
        /// 构造函数
        /// </summary>
        /// <param name="intDJ_ID">单据ID</param>
        public 营销退货明细单(int billID, AuthorityFlag authFlag)
        {
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.营销退货单, m_findSellIn);

            m_billMessageServer.BillType = "营销退货单";

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
        }

        /// <summary>
        /// 控制按钮显示
        /// </summary>
        /// <param name="strPersonnelType">显示状态</param>
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
        private void 营销退货明细单_Load(object sender, EventArgs e)
        {
            DataTable dt = m_findSellIn.GetBill(m_strDJH, m_intDJID);
            m_drZdRK = dt.NewRow();

            if (m_intDJID != 0)
            {
                m_strDJZTFlag = dt.Rows[0]["DJZT_Flag"].ToString();

                View_HR_Personnel lnqPersonnel = m_findPersonnel.GetPersonnelInfo(dt.Rows[0]["LRRY"].ToString());

                lbUserName.Text = lnqPersonnel.姓名.ToString();
                lbUserName.Tag = lnqPersonnel.工号.ToString();
                lbKS.Text = lnqPersonnel.部门名称.ToString();
                lbKS.Tag = lnqPersonnel.部门编码.ToString();
                m_dtMxRK = m_findSellIn.GetList(m_intDJID);
                tbsDept.Tag = dt.Rows[0]["ObjectDept"].ToString();

                View_Department linVdepartment = m_findDepartmentServer.GetDepartments(dt.Rows[0]["ObjectDept"].ToString());

                tbsDept.Text = linVdepartment.部门名称;
                txtSellID.Text = dt.Rows[0]["DJH"].ToString();
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                cmbTHFS.Text = dt.Rows[0]["YWFS"].ToString();
                txtRemarkAll.Text = dt.Rows[0]["Remark"].ToString();
                cmbStorage.Text = UniversalFunction.GetStorageName(dt.Rows[0]["StorageID"].ToString());
            }
            else
            {
                m_strDJZTFlag = "已保存";
                lbUserName.Text = BasicInfo.LoginName;
                lbUserName.Tag = BasicInfo.LoginID;
                lbKS.Text = BasicInfo.DeptName;
                lbKS.Tag = BasicInfo.DeptCode;
                txtSellID.Text = m_billNoControl.GetNewBillNo();

                CreateDateTableStyle();
            }

            if (m_strDJZTFlag != "已保存" && m_strDJZTFlag != "")
            {
                cmbTHFS.Enabled = false;
                tbsDept.Enabled = false;
                cmbStorage.Enabled = false;
            }

            dgv_Main.DataSource = m_dtMxRK;
            m_strDJH = txtSellID.Text.Trim();


            if (cmbStorage.Text == "售后库房")
            {
                label11.Visible = true;
                cmbRepairStatus.Visible = true;
                dgv_Main.Columns["RepairStatus"].Visible = true;
            }
            else
            {
                label11.Visible = false;
                cmbRepairStatus.Visible = false;
                dgv_Main.Columns["RepairStatus"].Visible = false;
            }
        }

        void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtSpce.Tag = txtBatchNo.DataResult["供应商编码"].ToString();
            lbUnit.Text = txtBatchNo.DataResult["单位"].ToString();
            lbStock.Text = txtBatchNo.DataResult["库存数量"].ToString();
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
            tbsGoods.Tag = tbsGoods.DataResult["序号"].ToString();
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = tbsGoods.DataResult["物品类别"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            txtSpce.Tag = "SYS_JLRD";
            lbStock.Text = "";
            txtBatchNo.Text = "";
            lbUnit.Text = "";

            numCount.Focus();
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
            m_dtMxRK.Columns.Add("RepairStatus");
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
            lbUnit.Text = "";
            tbsGoods.Text = "";
            tbsGoods.Tag = -1;
            lbStock.Text = "";
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
                    dr["UnitPrice"] = "0";
                    dr["Count"] = numCount.Value;
                    dr["Unit"] = lbUnit.Text;
                    dr["Price"] = Math.Round(numCount.Value * 0, 4);
                    dr["Remark"] = txtRemark.Text;
                    dr["Provider"] = txtSpce.Tag.ToString();
                    dr["RepairStatus"] = cmbRepairStatus.Text;

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
            if (tbsGoods.Text.Trim() == ""
                || tbsGoods.Tag == null
                || tbsGoods.Tag.ToString() == ""
                || tbsGoods.Tag.ToString() == "-1")
            {
                MessageBox.Show("请选择产品", "提示");
                tbsGoods.Focus();
                return false;
            }

            if (numCount.Value == 0)
            {
                MessageBox.Show("数量不能为0", "提示");
                numCount.Focus();
                return false;
            }


            if (cmbTHFS.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择退货方式");
                cmbTHFS.Focus();
                return false;
            }

            if (lbStock.Text == "")
            {
                MessageBox.Show("请选择批次号", "提示");
                numCount.Focus();
                return false;
            }

            //if (txtUnitPrice.Text == "")
            //{
            //    MessageBox.Show("金额不能为0", "提示");
            //    txtUnitPrice.Focus();
            //    return false;
            //}

            if (numCount.Value > Convert.ToDecimal(lbStock.Text))
            {
                MessageBox.Show("退货数量不能大于库存数量", "提示");
                numCount.Focus();
                return false;
            }

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            return true;
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
                if (cmbStorage.Text == "售后库房" && (cmbTHFS.Text == "生产返修退货" || cmbTHFS.Text == "0公里返修退货"))
                {
                    MessageDialog.ShowPromptMessage("退货方式错误");
                    return;
                }

                if (m_strDJZTFlag == "已确认")
                {
                    return;
                }
                else if (tbsDept.Tag == null)
                {
                    MessageBox.Show("请选择退货部门", "提示");
                    tbsDept.Focus();
                    return;
                }
                else if (cmbTHFS.Text == "")
                {
                    MessageBox.Show("请选择退货方式", "提示");
                    cmbTHFS.Focus();
                    return;
                } 
                
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
                m_drZdRK["YWFS"] = cmbTHFS.Text;
                m_drZdRK["JYRY"] = "";
                m_drZdRK["StorageID"] = UniversalFunction.GetStorageID(cmbStorage.Text);
                m_drZdRK["LRKS"] = BasicInfo.DeptCode;

                if (m_findSellIn.UpdateBill(dt, m_drZdRK, CE_MarketingType.退货.ToString(), out m_err))
                {
                    MessageBox.Show("保存成功", "提示");

                    m_billMessageServer.DestroyMessage(txtSellID.Text);
                    m_billMessageServer.SendNewFlowMessage(txtSellID.Text,
                        string.Format("【退货方式】：{0}  【库房】：{1}   ※※※ 等待【主管】处理", cmbTHFS.Text, cmbStorage.Text), BillFlowMessage_ReceivedUserType.角色,
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
                txtBatchNo.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString();
                lbUnit.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Unit"].ToString();
                txtGoodsCode.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["GoodsCode"].ToString();
                txtGoodsCode.Tag = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Depot"].ToString();
                txtSpce.Tag = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString();
                lbStock.Text = m_serverStore.GetStockCount(Convert.ToInt32(m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["CPID"].ToString()),
                                                            m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString(),
                                                            m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString(),
                                                            UniversalFunction.GetStorageID(cmbStorage.Text));
                cmbRepairStatus.Text = m_dtMxRK.Rows[dgv_Main.CurrentRow.Index]["RepairStatus"].ToString();
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
                    if (m_findSellIn.AuditingBill(m_intDJID,  txtRemarkAll.Text, out m_err))
                    {
                        MessageBox.Show("审核成功!", "提示");

                        m_billMessageServer.PassFlowMessage(txtSellID.Text, string.Format("【退货方式】：{0}  【库房】：{1}   ※※※ 等待【质检员】处理", cmbTHFS.Text, cmbStorage.Text),
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
            if (m_intDJID != 0 && m_strDJZTFlag == "已复审")
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

                if (m_findSellIn.AffrimBill(m_intDJID, CE_MarketingType.退货, (DataTable)dgv_Main.DataSource, out m_err))
                {
                    MessageBox.Show("确认完毕！", "提示");

                    #region 发送知会消息

                    List<string> noticeRoles = new List<string>();

                    string strDept = m_findDepartmentServer.GetDeptInfoFromPersonnelInfo(
                    m_findSellIn.GetBill(m_strDJH, m_intDJID).Rows[0]["LRRY"].ToString()).Rows[0]["DepartmentCode"].ToString();

                    noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));
                    noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                    noticeRoles.Add(CE_RoleEnum.下线主管.ToString());
                    noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString());

                    m_billMessageServer.EndFlowMessage(txtSellID.Text,
                        string.Format("{0} 号营销退货单已经处理完毕", txtSellID.Text),
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

            if (tbsDept.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("请填写【收货车间】");
                return;
            }

            WS_WorkShopCode tempWSCode = serverWSBasic.GetWorkShopCodeInfo(tbsDept.Tag.ToString());

            Dictionary<string, string> tempDic = new Dictionary<string, string>();

            tempDic.Add(UniversalFunction.GetStorageID(cmbStorage.Text), CE_MarketingType.退货.ToString());

            if (tempWSCode != null)
            {
                tempType = CE_BusinessType.综合业务;
                tempDic.Add(tempWSCode.WSCode, CE_SubsidiaryOperationType.营销退货.ToString());
            }

            产品编号 form = new 产品编号(tempInfo, tempType, txtSellID.Text,
                !(m_strDJZTFlag != "已保存" && m_strDJZTFlag != ""), tempDic);
            form.ShowDialog();

            if (form.IntCount != 0)
            {
                dgv_Main.CurrentRow.Cells["Count"].Value = form.IntCount;

            }
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            string strSql = " and 库存数量 <> 0  and 物品ID = " + Convert.ToInt32(tbsGoods.Tag);

            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (strStorage == null)
            {
                strSql += " and 库房代码 is null ";
            }
            else
            {
                strSql += " and 库房代码 = '" + strStorage + "'";
            }

            txtBatchNo.StrEndSql = strSql;
        }

        private void 营销退货明细单_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (m_strDJZTFlag == "已审核")
            {
                if (m_findSellIn.ExamineBill(txtSellID.Text, txtRemarkAll.Text, out m_err))
                {
                    IProductListServer serverProductList = ServerModule.ServerModuleFactory.GetServerModule<IProductListServer>();

                    bool flag = false;
                    for (int i = 0; i < dgv_Main.Rows.Count; i++)
                    {
                        if (Convert.ToBoolean(UniversalFunction.GetGoodsAttributeInfo(Convert.ToInt32(dgv_Main.Rows[i].Cells["CPID"].Value),
                            CE_GoodsAttributeName.TCU)))
                        {
                            flag = true;
                            break;
                        }
                    }

                    if (flag)
                    {
                        m_billMessageServer.PassFlowMessage(txtSellID.Text, string.Format("【退货方式】：{0}  【库房】：{1}   ※※※ 等待【TCU车间主管】处理", cmbTHFS.Text, cmbStorage.Text),
                                CE_RoleEnum.TCU主管.ToString(), true);
                    }
                    else
                    {
                        m_billMessageServer.PassFlowMessage(txtSellID.Text, string.Format("【退货方式】：{0}  【库房】：{1}   ※※※ 等待【下线主管】处理", cmbTHFS.Text, cmbStorage.Text),
                                CE_RoleEnum.下线主管.ToString(), true);
                    }

                    MessageBox.Show("检验通过！", "提示");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }
        }

        private void btnReexamine_Click(object sender, EventArgs e)
        {
            if (m_strDJZTFlag == "已检验")
            {
                Service_Manufacture_WorkShop.IWorkShopBasic serviceBasic =
                    Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                WS_WorkShopCode tempLnq = serviceBasic.GetWorkShopCodeInfo(tbsDept.Tag.ToString());

                if (tempLnq == null)
                {
                    MessageDialog.ShowPromptMessage("获取车间信息错误");
                    return;
                }

                if (m_findSellIn.RetrialBill(txtSellID.Text, txtRemarkAll.Text, out m_err))
                {
                    MessageBox.Show("复审通过！", "提示");

                    List<string> tempList =
                        GlobalObject.GeneralFunction.ConvertListTypeToStringList<CE_RoleEnum>(
                        UniversalFunction.GetStoreroomKeeperRoleEnumList(tempLnq.WSCode));

                    m_billMessageServer.PassFlowMessage(txtSellID.Text,
                        string.Format("【退货方式】：{0}  【库房】：{1}   ※※※ 等待【下线车间工作人员】处理", cmbTHFS.Text, cmbStorage.Text), 
                        BillFlowMessage_ReceivedUserType.角色, tempList);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }
        }

        private void cmbStorage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbStorage.Text == "售后库房")
            {
                label11.Visible = true;
                cmbRepairStatus.Visible = true;
                dgv_Main.Columns["RepairStatus"].Visible = true;
            }
            else
            {
                label11.Visible = false;
                cmbRepairStatus.Visible = false;
                dgv_Main.Columns["RepairStatus"].Visible = false;
            }
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

        private void numCount_Click(object sender, EventArgs e)
        {
            numCount.Focus();
            numCount.Select(0, numCount.Text.Length);
        }
    }
}
