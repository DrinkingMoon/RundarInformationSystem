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
    /// 营销退库明细界面
    /// </summary>
    public partial class 营销退库明细单 : Form
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
        /// 营销退库服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 部门服务
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
        /// 退库单据
        /// </summary>
        DataRow m_drZdCK;

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

        /// <summary>
        /// 构造函数
        /// </summary>
        public 营销退库明细单(int billID, AuthorityFlag authFlag)
        {
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.营销退库单, m_findSellIn);

            m_billMessageServer.BillType = "营销退库单";

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
        private void 营销退库明细单_Load(object sender, EventArgs e)
        {
            lbUserName.Text = BasicInfo.LoginName;

            DataTable dt = m_findSellIn.GetBill(m_strDJH, m_intDJID);
            m_drZdCK = dt.NewRow();

            if (m_intDJID != 0)
            {
                m_strDJZTFlag = dt.Rows[0]["DJZT_Flag"].ToString();

                View_HR_Personnel lnqPersonnel = m_findPersonnel.GetPersonnelInfo(dt.Rows[0]["LRRY"].ToString());

                lbUserName.Text = lnqPersonnel.姓名.ToString();
                lbUserName.Tag = lnqPersonnel.工号.ToString();
                lbKS.Text = lnqPersonnel.部门名称.ToString();
                lbKS.Tag = lnqPersonnel.部门编码.ToString();
                m_dtMxCK = m_findSellIn.GetList(m_intDJID);
                txtSellID.Text = dt.Rows[0]["DJH"].ToString();
                tbsClient.Tag = dt.Rows[0]["ObjectDept"].ToString();
                tbsClient.Text = m_findClientServer.GetClientName(dt.Rows[0]["ObjectDept"].ToString());
                txtPrice.Text = dt.Rows[0]["Price"].ToString();
                cmbTKFS.Text = dt.Rows[0]["YWFS"].ToString();
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
                cmbTKFS.Enabled = false;
                tbsClient.Enabled = false;
                cmbStorage.Enabled = false;

                if (UniversalFunction.CheckStorageAndPersonnel(dt.Rows[0]["StorageID"].ToString()) == false)
                {
                    btnAffirm.Visible = false;
                }
            }

            dgv_Main.DataSource = m_dtMxCK;
            m_strDJH = txtSellID.Text.Trim();
        }

        void tbsGoods_OnCompleteSearch()
        {
            tbsGoods.Text = tbsGoods.DataResult["物品名称"].ToString();
            txtGoodsCode.Text = tbsGoods.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = tbsGoods.DataResult["物品类别"].ToString();
            txtSpce.Text = tbsGoods.DataResult["规格"].ToString();
            tbsGoods.Tag = tbsGoods.DataResult["序号"].ToString();

            numCount.Focus();
        }

        void txtBatchNo_OnCompleteSearch()
        {
            txtBatchNo.Text = txtBatchNo.DataResult["批次号"].ToString();
            txtSpce.Tag = txtBatchNo.DataResult["供应商编码"].ToString();
            lbUnit.Text = txtBatchNo.DataResult["单位"].ToString();
            lbStock.Text = txtBatchNo.DataResult["库存数量"].ToString();
            numUnitPrice.Value = Convert.ToDecimal(txtBatchNo.DataResult["单价"]) == 0 ?
                m_serverStore.GetGoodsUnitPrice(Convert.ToInt32(tbsGoods.Tag), 
                txtBatchNo.Text.ToString(), UniversalFunction.GetStorageID(cmbStorage.Text)) : 
                Convert.ToDecimal( txtBatchNo.DataResult["单价"]);
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtMxCK.Columns.Add("CPID");
            m_dtMxCK.Columns.Add("GoodsCode");
            m_dtMxCK.Columns.Add("GoodsName");
            m_dtMxCK.Columns.Add("Spec");
            m_dtMxCK.Columns.Add("Depot");
            m_dtMxCK.Columns.Add("BatchNo");
            m_dtMxCK.Columns.Add("UnitPrice");
            m_dtMxCK.Columns.Add("Count");
            m_dtMxCK.Columns.Add("Unit");
            m_dtMxCK.Columns.Add("Price");
            m_dtMxCK.Columns.Add("Remark");
            m_dtMxCK.Columns.Add("Provider");
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
        /// <returns>检测通过返回True，否则返回False</returns>
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
            if (numCount.Value == 0)
            {
                MessageBox.Show("数量不能为0", "提示");
                numCount.Focus();
                return false;
            }

            if (cmbTKFS.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择退库方式");
                cmbTKFS.Focus();
                return false;
            }

            if (tbsGoods.Text.Trim() == ""
                || tbsGoods.Tag == null
                || tbsGoods.Tag.ToString() == ""
                || tbsGoods.Tag.ToString() == "-1")
            {
                MessageBox.Show("请选择产品", "提示");
                tbsGoods.Focus();
                return false;
            }

            if (lbStock.Text == "")
            {
                MessageBox.Show("请选择批次号", "提示");
                numCount.Focus();
                return false;
            }

            //if (txtUnitPrice.Text == "" )
            //{
            //    MessageBox.Show("金额不能为0", "提示");
            //    txtUnitPrice.Focus();
            //    return false;
            //}

            if (cmbStorage.Text == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
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

                if (tbsClient.Tag == null)
                {
                    MessageBox.Show("请选择退库客户", "提示");
                    tbsClient.Focus();
                    return;
                }

                if (cmbTKFS.Text == "")
                {
                    MessageBox.Show("请选择退库方式", "提示");
                    cmbTKFS.Focus();
                    return;
                }


                DataTable dt = (DataTable)dgv_Main.DataSource;

                m_drZdCK["ID"] = m_intDJID.ToString();
                m_drZdCK["DJH"] = txtSellID.Text;
                m_drZdCK["ObjectDept"] = tbsClient.Tag.ToString();
                m_drZdCK["LRRY"] = BasicInfo.LoginID;
                m_drZdCK["Date"] = ServerTime.Time.ToString();
                m_drZdCK["KFRY"] = "";
                m_drZdCK["Price"] = Convert.ToDecimal(txtPrice.Text);
                m_drZdCK["SHRY"] = "";
                m_drZdCK["Remark"] = txtRemarkAll.Text;
                m_drZdCK["YWFS"] = cmbTKFS.Text;
                m_drZdCK["JYRY"] = "";
                m_drZdCK["StorageID"] = UniversalFunction.GetStorageID(cmbStorage.Text);
                m_drZdCK["LRKS"] = BasicInfo.DeptCode;

                if (m_findSellIn.UpdateBill(dt, m_drZdCK, CE_MarketingType.退库.ToString(), out m_err))
                {
                    MessageBox.Show("保存成功", "提示");

                    m_billMessageServer.DestroyMessage(txtSellID.Text);
                    m_billMessageServer.SendNewFlowMessage(txtSellID.Text, string.Format("【退库方式】：{0}  【库房】：{1}   ※※※ 等待【主管】处理", cmbTKFS.Text, cmbStorage.Text), 
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

            if (m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["CPID"].ToString() != "")
            {
                tbsGoods.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsName"].ToString();
                tbsGoods.Tag = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["CPID"].ToString();
                numCount.Value = Convert.ToDecimal( m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Count"]);
                txtSpce.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Spec"].ToString();
                txtRemark.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Remark"].ToString();
                numUnitPrice.Value = Convert.ToDecimal( m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["UnitPrice"]);
                txtBatchNo.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString();
                lbUnit.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Unit"].ToString();
                txtGoodsCode.Text = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsCode"].ToString();
                txtGoodsCode.Tag = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Depot"].ToString();
                txtSpce.Tag = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString();
                lbStock.Text = m_serverStore.GetStockCount(Convert.ToInt32(m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["CPID"].ToString()),
                                                          m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString(),
                                                          m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Provider"].ToString(),
                                                          UniversalFunction.GetStorageID(cmbStorage.Text));
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

                if (m_findSellIn.AffrimBill(m_intDJID, CE_MarketingType.退库, (DataTable)dgv_Main.DataSource, out m_err))
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
                        string.Format("{0} 号营销退库单已经处理完毕", txtSellID.Text),
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
                    if (m_findSellIn.AuditingBill(m_intDJID,txtRemarkAll.Text, out m_err))
                    {
                        MessageBox.Show("审核成功!", "提示");
                        m_billMessageServer.PassFlowMessage(txtSellID.Text, string.Format("【退库方式】：{0}  【库房】：{1}   ※※※ 等待【质检员】处理", cmbTKFS.Text, cmbStorage.Text),
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

        private void dgv_Main_DoubleClick(object sender, EventArgs e)
        {
            if (dgv_Main.CurrentRow == null)
            {
                return;
            }

            switch (UniversalFunction.GetGoodsType(Convert.ToInt32(dgv_Main.CurrentRow.Cells["CPID"].Value),
                UniversalFunction.GetStorageID(cmbStorage.Text)))
            {
                case CE_GoodsType.CVT:
                case CE_GoodsType.TCU:
                    BarCodeInfo tempInfo = new BarCodeInfo();

                    tempInfo.BatchNo = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["BatchNo"].ToString();
                    tempInfo.Count = Convert.ToDecimal(m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Count"]);
                    tempInfo.GoodsCode = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsCode"].ToString();
                    tempInfo.GoodsID = Convert.ToInt32(m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["CPID"]);
                    tempInfo.GoodsName = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["GoodsName"].ToString();
                    tempInfo.Remark = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Remark"].ToString();
                    tempInfo.Spec = m_dtMxCK.Rows[dgv_Main.CurrentRow.Index]["Spec"].ToString();

                    Dictionary<string, string> tempDic = new Dictionary<string, string>();

                    tempDic.Add(UniversalFunction.GetStorageID(cmbStorage.Text), CE_MarketingType.退库.ToString());

                    产品编号 formCode = new 产品编号(tempInfo, CE_BusinessType.库房业务, txtSellID.Text,
                        !(m_strDJZTFlag != "已检验" && m_strDJZTFlag != ""), tempDic);
                    formCode.ShowDialog();
                    break;
                case CE_GoodsType.工装:
                    break;
                case CE_GoodsType.量检具:
                    量检具编号录入窗体 form = new 量检具编号录入窗体(txtSellID.Text,
                        Convert.ToInt32(dgv_Main.CurrentRow.Cells["CPID"].Value), Convert.ToDecimal(dgv_Main.CurrentRow.Cells["Count"].Value), CE_BusinessBillType.营销退库,
                        m_strDJZTFlag == "已检验" ? true : false);

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

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if (m_strDJZTFlag == "已审核")
            {
                if (m_findSellIn.ExamineBill(txtSellID.Text, txtRemarkAll.Text, out m_err))
                {
                    MessageBox.Show("检验通过！", "提示");

                    m_billMessageServer.PassFlowMessage(txtSellID.Text, string.Format("【退库方式】：{0}  【库房】：{1}   ※※※ 等待【仓管】处理", cmbTKFS.Text, cmbStorage.Text),
                            m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString(), true);
                    this.Close();
                }
            }
            else
            {
                MessageBox.Show("请重新确认单据状态", "提示");
            }
        }

        /// <summary>
        /// 完成部门信息检索
        /// </summary>
        private void tbsClient_OnCompleteSearch()
        {
            tbsClient.Tag = tbsClient.DataResult["客户编码"].ToString();
        }

        private void tbsGoods_Enter(object sender, EventArgs e)
        {
            string strSql = "";
            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);

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
            string strSql = " and 物品ID = " + Convert.ToInt32(tbsGoods.Tag);

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

        private void 营销退库明细单_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void numCount_Click(object sender, EventArgs e)
        {
            numCount.Focus();
            numCount.Select(0, numCount.Text.Length);
        }
    }
}
