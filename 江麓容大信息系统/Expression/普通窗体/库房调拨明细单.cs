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
    /// 库房调拨明细界面
    /// </summary>
    public partial class 库房调拨明细单 : Form
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 库房信息服务组件
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 库存服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 营销服务组件
        /// </summary>
        ISellIn m_serverSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 营销出库服务组件
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 调拨单服务组件
        /// </summary>
        ICannibalize m_serverCannibalize = ServerModuleFactory.GetServerModule<ICannibalize>();

        /// <summary>
        /// 客户服务组件
        /// </summary>
        IClientServer m_findClientServer = ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 人员服务组件
        /// </summary>
        IPersonnelInfoServer m_findPersonnel = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 单据明细表
        /// </summary>
        DataTable m_dtMxCK = new DataTable();

        /// <summary>
        /// 出库单据
        /// </summary>
        S_CannibalizeBill m_billInfoLnq = new S_CannibalizeBill();

        /// <summary>
        /// 单据状态
        /// </summary>
        string m_strDJZTFlag;

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

        public 库房调拨明细单(int intDJ_ID, AuthorityFlag m_authFlag)
        {
            m_billMessageServer.BillType = CE_BillTypeEnum.库房调拨单.ToString();

            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.库房调拨单, m_serverCannibalize);

            m_intDJID = intDJ_ID;

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip, m_authFlag);

            this.toolStrip.Visible = true;
            DataTable dt = m_serverStorageInfo.GetStorageNameFromPersonnel(BasicInfo.LoginID);

            if (dt.Rows.Count != 0 && m_intDJID == 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbInStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
                }

                cmbInStorage.SelectedIndex = 0;
            }
            else
            {
                dt = UniversalFunction.GetStorageTb();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbInStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
                }

                cmbInStorage.SelectedIndex = 0;
            }

            dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbOutStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbOutStorage.SelectedIndex = -1;
        }

        private void 库房调拨明细单_Load(object sender, EventArgs e)
        {
            S_CannibalizeBill tempBillInfo = new S_CannibalizeBill();

            if (m_intDJID != 0)
            {
                tempBillInfo = m_serverCannibalize.GetBill(m_intDJID);
                m_strDJZTFlag = tempBillInfo.DJZT;

                View_HR_Personnel lnqPersonnel = m_findPersonnel.GetPersonnelInfo(tempBillInfo.LRRY);

                if (lnqPersonnel.姓名 != BasicInfo.LoginName)
                {
                    btnSave.Visible = false;
                    cmbInStorage.Enabled = false;
                    cmbOutStorage.Enabled = false;
                }

                m_dtMxCK = m_serverCannibalize.GetList(m_intDJID);
                txtSellID.Text = tempBillInfo.DJH;
                txtPrice.Text = tempBillInfo.Price.ToString();
                txtRemarkAll.Text = tempBillInfo.Remark;
                cmbInStorage.Text = UniversalFunction.GetStorageName(tempBillInfo.InStoreRoom);
                cmbOutStorage.Text = UniversalFunction.GetStorageName(tempBillInfo.OutStoreRoom);
                btnAffirm.Visible = UniversalFunction.CheckStorageAndPersonnel(tempBillInfo.OutStoreRoom);
            }
            else
            {
                m_strDJZTFlag = "已保存";
                btnAffirm.Visible = false;
                txtSellID.Text = m_billNoControl.GetNewBillNo();
                CreateDateTableStyle();
            }

            if (m_strDJZTFlag != "已保存" && m_strDJZTFlag != "")
            {
                cmbOutStorage.Enabled = false;
                cmbInStorage.Enabled = false;

                if (!UniversalFunction.CheckStorageAndPersonnel(tempBillInfo.OutStoreRoom) == false)
                {
                    btnSh.Visible = false;
                }
                
            }

            dgv_Main.DataSource = m_dtMxCK;

            m_strDJH = txtSellID.Text.Trim();
        }

        void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtSpce.Tag = txtBatchNo.DataResult["供应商编码"].ToString();
            txtUnitPrice.Text = txtBatchNo.DataResult["单价"].ToString();
            lbUnit.Text = txtBatchNo.DataResult["单位"].ToString();
            lbStock.Text = txtBatchNo.DataResult["库存数量"].ToString();
        }

        void tbsGoods_OnCompleteSearch()
        {
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            tbsGoods.Tag = tbsGoods.DataResult["序号"].ToString();
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = tbsGoods.DataResult["物品类别"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            lbStock.Text = "";
            txtUnitPrice.Text = "0";
            txtBatchNo.Text = "";
            lbUnit.Text = "";
            txtCount.Focus();
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtMxCK.Columns.Add("GoodsID");
            m_dtMxCK.Columns.Add("GoodsCode");
            m_dtMxCK.Columns.Add("GoodsName");
            m_dtMxCK.Columns.Add("Spec");
            m_dtMxCK.Columns.Add("BatchNo");
            m_dtMxCK.Columns.Add("Count");
            m_dtMxCK.Columns.Add("Unit");
            m_dtMxCK.Columns.Add("UnitPrice");
            m_dtMxCK.Columns.Add("Depot");
            m_dtMxCK.Columns.Add("Provider");
            m_dtMxCK.Columns.Add("Price");
            m_dtMxCK.Columns.Add("Remark");
            m_dtMxCK.Columns.Add("RepairStatus");    
        }

        /// <summary>
        /// 关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// 窗体清空
        /// </summary>
        private void ClearDate()
        {
            txtSpce.Text = "";
            txtCount.Text = "0";
            txtBatchNo.Text = "";
            txtGoodsCode.Text = "";
            txtRemark.Text = "";
            txtUnitPrice.Text = "0";
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
                    DataRow dr = m_dtMxCK.NewRow();

                    dr["GoodsID"] = Convert.ToInt32(tbsGoods.Tag);
                    dr["GoodsCode"] = txtGoodsCode.Text;
                    dr["GoodsName"] = tbsGoods.Text;
                    dr["Spec"] = txtSpce.Text;
                    dr["Depot"] = txtGoodsCode.Tag.ToString();
                    dr["BatchNo"] = txtBatchNo.Text;
                    dr["UnitPrice"] = (Convert.ToDecimal(txtUnitPrice.Text)).ToString();
                    dr["Count"] = (Convert.ToDecimal(txtCount.Text)).ToString();
                    dr["Unit"] = lbUnit.Text;
                    dr["Price"] = Math.Round(Convert.ToDecimal(txtCount.Text) * Convert.ToDecimal(txtUnitPrice.Text), 4);
                    dr["Remark"] = txtRemark.Text;
                    dr["Provider"] = txtSpce.Tag.ToString();
                    dr["RepairStatus"] = cmbProductStatus.Text == "已返修" ? 1 : 0;

                    m_dtMxCK.Rows.Add(dr);

                    dgv_Main.DataSource = m_dtMxCK;

                    if (m_dtMxCK.Rows.Count > 0)
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
        /// <returns>检测通过返回True,否则返回False</returns>
        private bool CheckSameGoods()
        {
            if (m_dtMxCK == null || m_dtMxCK.Rows.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < m_dtMxCK.Rows.Count; i++)
            {
                if (m_dtMxCK.Rows[i]["GoodsCode"].ToString().Trim() == txtGoodsCode.Text.Trim()
                    && m_dtMxCK.Rows[i]["GoodsName"].ToString().Trim() == tbsGoods.Text.Trim()
                    && m_dtMxCK.Rows[i]["Spec"].ToString().Trim() == txtSpce.Text.Trim()
                    && m_dtMxCK.Rows[i]["Depot"].ToString().Trim() == txtGoodsCode.Tag.ToString().Trim()
                    && m_dtMxCK.Rows[i]["Provider"].ToString().Trim() == txtSpce.Tag.ToString().Trim()
                    && m_dtMxCK.Rows[i]["BatchNo"].ToString().Trim() == txtBatchNo.Text.Trim()
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
            if (txtCount.Text == "" || txtCount.Text == "0")
            {
                MessageBox.Show("数量不能为0", "提示");
                txtCount.Focus();
                return false;
            }

            if (lbStock.Text == "")
            {
                MessageBox.Show("请选择批次号", "提示");
                txtCount.Focus();
                return false;
            }

            if (tbsGoods.Tag.ToString() == "" || tbsGoods.Tag.ToString() == "-1")
            {
                MessageBox.Show("请选择产品", "提示");
                tbsGoods.Focus();
                return false;
            }

            if (txtUnitPrice.Text == "" || txtUnitPrice.Text == "0")
            {
                MessageBox.Show("金额不能为0", "提示");
                txtUnitPrice.Focus();
                return false;
            }

            if (Convert.ToDecimal(txtCount.Text) > Convert.ToDecimal(lbStock.Text))
            {
                MessageBox.Show("出库数量不能大于库存数量", "提示");
                txtCount.Focus();
                return false;
            }

            if (UniversalFunction.GetStorageID(cmbInStorage.Text) == "05" && cmbProductStatus.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品状态");
                cmbProductStatus.Focus();
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
        /// txtUnitPrice的KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtUnitPrice_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtUnitPrice.Text != "")
            {
                if (e.KeyValue == 13)
                {
                    txtCount.Focus();
                }
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
        /// txtCount的KeyDown事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtCount_KeyDown(object sender, KeyEventArgs e)
        {
            if (txtCount.Text != "")
            {
                if (e.KeyValue == 13)
                {
                    btnAdd.Focus();
                }
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
                m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Price"])).ToString();
            m_dtMxCK.Rows.RemoveAt(dgv_Main.CurrentRow.Index);
            btnAdd_Click(sender, e);
        }

        /// <summary>
        /// 删除事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_dtMxCK.Rows.Count != 0)
            {
                txtPrice.Text = (Convert.ToDecimal(txtPrice.Text) - Convert.ToDecimal(
                    m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Price"])).ToString();
                m_dtMxCK.Rows.RemoveAt(dgv_Main.CurrentRow.Index);
                dgv_Main.DataSource = m_dtMxCK;
            }
            else
            {
                txtPrice.Text = "0";
                ClearDate();
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

            if (m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsID"].ToString() != "")
            {
                tbsGoods.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsName"].ToString();
                tbsGoods.Tag = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsID"].ToString();
                txtCount.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Count"].ToString();
                txtSpce.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Spec"].ToString();
                txtRemark.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Remark"].ToString();
                txtUnitPrice.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["UnitPrice"].ToString();
                txtBatchNo.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString();
                lbUnit.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Unit"].ToString();
                txtGoodsCode.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsCode"].ToString();
                txtGoodsCode.Tag = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Depot"].ToString();
                txtSpce.Tag = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString();

                if (m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["RepairStatus"].ToString() == "")
                {
                    cmbProductStatus.SelectedIndex = -1;
                }
                else
                {
                    cmbProductStatus.Text =
                        Convert.ToInt32(m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["RepairStatus"]) == 1 ? "已返修" : "待返修";
                }

                lbStock.Text = m_serverStore.GetGoodsStockInfo(
                    Convert.ToInt32(m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsID"].ToString()),
                    m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString(),
                    m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString(),
                    UniversalFunction.GetStorageID(cmbOutStorage.Text)).Rows[0]["ExistCount"].ToString();
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
                    if (m_serverCannibalize.AuditingBill(m_intDJID, txtRemarkAll.Text, out m_err))
                    {
                        string msg = string.Format("{0} 号库房调拨单已由主管审核，请质量工程师检测通过", txtSellID.Text);
                        m_billMessageServer.PassFlowMessage(txtSellID.Text, msg, CE_RoleEnum.质量工程师.ToString(), true);
                        MessageBox.Show("审核成功!", "提示");
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
        /// 批准
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (m_strDJZTFlag == "已检测")
            {
                if (m_serverCannibalize.CheckBill(txtSellID.Text, 
                    txtRemarkAll.Text, out m_err))
                {
                    string msg = string.Format("{0} 号库房调拨单已由财务批准，请仓管确认", txtSellID.Text);
                    m_billMessageServer.PassFlowMessage(txtSellID.Text, msg, 
                        m_billMessageServer.GetRoleStringForStorage(cmbInStorage.Text).ToString(), true);
                    MessageBox.Show("财务批准通过！", "提示");
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
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
                if (m_strDJZTFlag == "已确认")
                {
                    return;
                }

                BASE_Storage storageInfo_Out = UniversalFunction.GetStorageInfo(cmbOutStorage.Text);
                BASE_Storage storageInfo_In = UniversalFunction.GetStorageInfo(cmbInStorage.Text);

                if (storageInfo_In.StorageID == storageInfo_Out.StorageID)
                {
                    throw new Exception("【入库库房】与【出库库房】是同一库房，无法操作");
                }

                if (storageInfo_In.ZeroCostFlag != storageInfo_Out.ZeroCostFlag)
                {
                    throw new Exception("【入库库房】与【出库库房】财务结算属性不一致，不能相互调拨");
                }

                DataTable dt = (DataTable)dgv_Main.DataSource;

                m_billInfoLnq.ID = m_intDJID;
                m_billInfoLnq.DJH = txtSellID.Text;
                m_billInfoLnq.LRRY = BasicInfo.LoginID;
                m_billInfoLnq.LRRQ = ServerTime.Time;
                m_billInfoLnq.Price = Convert.ToDecimal(txtPrice.Text);
                m_billInfoLnq.Remark = txtRemarkAll.Text;
                m_billInfoLnq.OutStoreRoom = storageInfo_Out.StorageID;
                m_billInfoLnq.InStoreRoom = storageInfo_In.StorageID;

                m_serverCannibalize.SaveBill(dt, m_billInfoLnq);
                m_billMessageServer.DestroyMessage(txtSellID.Text);
                m_billMessageServer.SendNewFlowMessage(txtSellID.Text, string.Format("{0} 号库房调拨单，请主管审核", txtSellID.Text),
                    BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                MessageBox.Show("保存成功", "提示");
                txtSellID.Tag = null;
                this.Close();

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
            if (m_intDJID != 0 && m_strDJZTFlag == "已批准")
            {
                DataTable dtTemp = (DataTable)dgv_Main.DataSource;

                BASE_Storage storageInfo_Out = UniversalFunction.GetStorageInfo(cmbOutStorage.Text);
                BASE_Storage storageInfo_In = UniversalFunction.GetStorageInfo(cmbInStorage.Text);

                if (storageInfo_In.StorageID == storageInfo_Out.StorageID)
                {
                    throw new Exception("【入库库房】与【出库库房】是同一库房，无法操作");
                }

                if (storageInfo_In.ZeroCostFlag != storageInfo_Out.ZeroCostFlag)
                {
                    throw new Exception("【入库库房】与【出库库房】财务结算属性不一致，不能相互调拨");
                }

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (!m_serverProductCode.IsFitCount(Convert.ToInt32(dtTemp.Rows[i]["GoodsID"]),
                        Convert.ToInt32(dtTemp.Rows[i]["Count"]), m_strDJH))
                    {
                        MessageBox.Show("请对产品设置流水号,并保证产品数量与流水号数一致", "提示");
                        return;
                    }
                }

                if (m_serverCannibalize.AffirmBill(m_intDJID, out m_err))
                {
                    #region 发送知会消息

                    List<string> noticeRoles = new List<string>();
                    string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                        m_serverCannibalize.GetBill(m_intDJID).LRRY).Rows[0]["DepartmentCode"].ToString();
                    noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));
                    noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbOutStorage.Text).ToString());
                    noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbInStorage.Text).ToString());
                    noticeRoles.Add(CE_RoleEnum.财务主管.ToString());
                    noticeRoles.Add(CE_RoleEnum.会计.ToString());

                    m_billMessageServer.EndFlowMessage(txtSellID.Text, 
                        string.Format("{0} 号库房调拨单已经处理完毕", txtSellID.Text),
                        noticeRoles, null);

                    #endregion 发送知会消息

                    MessageBox.Show("确认完毕！", "提示");
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

        private void tbsGoods_Enter(object sender, EventArgs e)
        {
            string strSql = "";
            string strStorage = UniversalFunction.GetStorageID(cmbOutStorage.Text);

            if (strStorage == null)
            {
                strSql += " and 库房代码 is null ";
            }
            else
            {
                strSql += " and 库房代码 = '" + strStorage + "'";
            }

            tbsGoods.StrEndSql = strSql;
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            string strSql = " and 库存数量 <> 0  and 物品状态 <> '隔离' and 物品ID = " + Convert.ToInt32(tbsGoods.Tag);

            string strStorage = UniversalFunction.GetStorageID(cmbOutStorage.Text);

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

        private void dgv_Main_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_Main.CurrentRow == null)
            {
                return;
            }

            switch (UniversalFunction.GetGoodsType(Convert.ToInt32(dgv_Main.CurrentRow.Cells["GoodsID"].Value),
                UniversalFunction.GetStorageID(cmbOutStorage.Text)))
            {
                case CE_GoodsType.CVT:
                case CE_GoodsType.TCU:
                    BarCodeInfo tempInfo = new BarCodeInfo();

                    tempInfo.BatchNo = dgv_Main.CurrentRow.Cells["BatchNo"].Value.ToString();
                    tempInfo.Count = Convert.ToDecimal(dgv_Main.CurrentRow.Cells["Count"].Value);
                    tempInfo.GoodsCode = dgv_Main.CurrentRow.Cells["GoodsCode"].Value.ToString();
                    tempInfo.GoodsID = Convert.ToInt32(dgv_Main.CurrentRow.Cells["GoodsID"].Value);
                    tempInfo.GoodsName = dgv_Main.CurrentRow.Cells["GoodsName"].Value.ToString();
                    tempInfo.Remark = dgv_Main.CurrentRow.Cells["Remark"].Value.ToString();
                    tempInfo.Spec = dgv_Main.CurrentRow.Cells["Spec"].Value.ToString();

                    Dictionary<string, string> tempDic = new Dictionary<string, string>();

                    tempDic.Add(UniversalFunction.GetStorageID(cmbOutStorage.Text), CE_MarketingType.调出.ToString());
                    tempDic.Add(UniversalFunction.GetStorageID(cmbInStorage.Text), CE_MarketingType.调入.ToString());

                    产品编号 formCode = new 产品编号(tempInfo, CE_BusinessType.库房业务, txtSellID.Text,
                        m_strDJZTFlag == "已批准" ? true : false, tempDic);

                    formCode.ShowDialog();

                 break;
                case CE_GoodsType.工装:
                 break;
                case CE_GoodsType.量检具:
                    量检具编号录入窗体 form = new 量检具编号录入窗体(txtSellID.Text,
                        Convert.ToInt32(dgv_Main.CurrentRow.Cells["GoodsID"].Value), 
                        Convert.ToDecimal(dgv_Main.CurrentRow.Cells["Count"].Value), CE_BusinessBillType.库房调出,
                        m_strDJZTFlag == "已批准" ? true : false);

                 form.ShowDialog();
                 break;
                case CE_GoodsType.零件:
                 break;
                case CE_GoodsType.未知物品:
                 break;
                default:
                 break;
            }
        }

        private void 库房调拨明细单_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void cmbInStorage_TextChanged(object sender, EventArgs e)
        {
            if (UniversalFunction.GetStorageID(cmbInStorage.Text) == "05")
            {
                label6.Visible = true;
                cmbProductStatus.Visible = true;
            }
            else
            {
                label6.Visible = false;
                cmbProductStatus.Visible = false;
            }
        }

        private void btnQuality_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_strDJZTFlag == "已审核")
                {
                    if (m_serverCannibalize.QualityBill(m_intDJID, txtRemarkAll.Text, out m_err))
                    {
                        string msg = string.Format("{0} 号库房调拨单已由已检测，请财务批准", txtSellID.Text);

                        m_billMessageServer.PassFlowMessage(txtSellID.Text, msg, CE_RoleEnum.会计.ToString(), true);
                        MessageBox.Show("检测通过!", "提示");
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
    }
}
