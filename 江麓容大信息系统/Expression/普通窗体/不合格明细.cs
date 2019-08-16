using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 不合格成品隔离明细操作界面
    /// </summary>
    public partial class 不合格隔离明细 : Form
    {
        /// <summary>
        /// 不合格产品隔离服务类
        /// </summary>
        IQuarantine m_serverQuarantine = ServerModuleFactory.GetServerModule<IQuarantine>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 不合格明细表
        /// </summary>
        private DataTable m_dtList = new DataTable();

        public DataTable DtList
        {
            get { return m_dtList; }
            set { m_dtList = value; }
        }

        /// <summary>
        /// 单据状态
        /// </summary>
        string m_strBillStatus;

        /// <summary>
        /// 箱体的处理状态
        /// </summary>
        bool m_blIsHandle;

        /// <summary>
        /// 处理结果
        /// </summary>
        string m_strHandleAll;

        public 不合格隔离明细(AuthorityFlag m_authFlag)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

            toolStrip1.Visible = true;

            txtBill_ID.Text = m_serverQuarantine.GetBillID().ToString();
            txtLRRY.Text = BasicInfo.LoginName.ToString();
            txtLRBM.Text = BasicInfo.DeptName.ToString();

            DataTable dtStorage = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dtStorage.Rows.Count; i++)
            {
                if (dtStorage.Rows[i]["StorageID"].ToString() == "02" 
                    || dtStorage.Rows[i]["StorageID"].ToString() == "05")
                {
                    cmbStorage.Items.Add(dtStorage.Rows[i]["StorageName"].ToString());
                }
            }

            cmbStorage.SelectedIndex = -1;

            CreateDateTableStyle();
            dgv_Main.DataSource = m_dtList;
        }

        public 不合格隔离明细(string DJH,string Storage, string Quarantine,string LRRY,string LRBM,string Remark,
            string flag, string handle,string disName,string dis, AuthorityFlag m_authFlag)
        {
            InitializeComponent();

            txtDisposeName.Text = BasicInfo.LoginName;

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);

            toolStrip1.Visible = true;
            txtBill_ID.Text = DJH.ToString();
            cmbStorage.Text = Storage.ToString();
            txtQuarantine.Text = Quarantine.ToString();
            txtLRRY.Text = LRRY.ToString();
            txtLRBM.Text = LRBM.ToString();
            txtRemarkAll.Text = Remark.ToString();
            m_strBillStatus = flag;
            m_strHandleAll = handle;

            m_dtList = m_serverQuarantine.GetList(DJH, out m_strErr);
            dgv_Main.DataSource = m_dtList;
            dgv_Main.Columns["Bill_ID"].Visible = false;
            dgv_Main.Columns["Storage"].Visible = false;

            if (m_strBillStatus != "已保存")
            {
                cmbStorage.Enabled = false;
                txtQuarantine.ReadOnly = true;
                txtRemarkAll.ReadOnly = true;
                btnFindCode.Enabled = false;
                
            }

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.成品仓库管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.售后库房管理员.ToString()))
            {
                dgv_Main.Columns["选"].Visible = true;
                contextMenuStrip1.Enabled = true;
                选择ToolStripMenuItem.Enabled = true;
                取消ToolStripMenuItem.Enabled = true;
                全选ToolStripMenuItem.Enabled = true;
                全消ToolStripMenuItem.Enabled = true;
            }

            DataTable dtStorage = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dtStorage.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dtStorage.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.Text = Storage;

            if (disName == "" || disName == null)
            {
                radioButton1.Checked = true;
                btnAffirm.Visible = true;
                txtDisposeName.ReadOnly = true;
                txtDispose.ReadOnly = true;
            }
            else
            {
                rdbRepair.Checked = true;
                txtDisposeName.Text = disName;
                txtDispose.Text = dis;
                btnAffirm.Visible = false;
            }
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtList.Columns.Add("GoodsCode");
            m_dtList.Columns.Add("GoodsName");
            m_dtList.Columns.Add("ProductCode");
            m_dtList.Columns.Add("Storage");
            m_dtList.Columns.Add("GoodsID");
            m_dtList.Columns.Add("Bill_ID");
            m_dtList.Columns.Add("Spec");
            m_dtList.Columns.Add("Depot");
            m_dtList.Columns.Add("BatchNo");
            m_dtList.Columns.Add("UnitPrice");
            m_dtList.Columns.Add("Remark");
        }

        /// <summary>
        /// 窗体清空
        /// </summary>
        private void ClearDate()
        {
            txtSpce.Text = "";
            txtBatchNo.Text = "";
            txtGoodsCode.Text = "";
            txtRemark.Text = "";
            txtUnitPrice.Text = "0";
            txtGoodName.Text = "";
            txtGoodName.Tag = -1;
        }

        /// <summary>
        /// 添加到datagridview
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (CheckForm())
            {
                if (CheckSameGoods())
                {
                    DataRow dr = m_dtList.NewRow();

                    dr["GoodsCode"] = txtGoodsCode.Text;//图形图号
                    dr["GoodsName"] = txtGoodName.Text;//物品名称
                    dr["ProductCode"] = txtBatchNo.Text;
                    dr["Storage"] = cmbStorage.Text;
                    dr["GoodsID"] = txtGoodName.Tag.ToString();
                    dr["Bill_ID"] = txtBill_ID.Text;
                    dr["Spec"] = txtSpce.Text;
                    dr["Depot"] = txtType.Text;//产品类型
                    dr["UnitPrice"] = (Convert.ToDecimal(txtUnitPrice.Text)).ToString();
                    dr["Remark"] = txtRemark.Text;

                    m_dtList.Rows.Add(dr);
                    dgv_Main.DataSource = m_dtList;
                    ClearDate();
                }
            }
        }

        /// <summary>
        /// 检查同种物品
        /// </summary>
        /// <returns>检测通过返回True,否则返回False</returns>
        public bool CheckSameGoods()
        {
            if (m_dtList == null || m_dtList.Rows.Count == 0)
            {
                return true;
            }

            for (int i = 0; i < m_dtList.Rows.Count; i++)
            {

                if (m_dtList.Rows[i]["GoodsCode"].ToString().Trim() == txtGoodsCode.Text.Trim()
                    && m_dtList.Rows[i]["GoodsName"].ToString().Trim() == txtGoodName.Text.Trim()
                    && m_dtList.Rows[i]["ProductCode"].ToString().Trim() == txtBatchNo.Text.Trim()
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
        /// <returns></returns>
        private bool CheckForm()
        {
            if (txtGoodName.Text.Trim() == "" 
                || txtGoodName.Tag.ToString() == ""
                || txtBatchNo.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品信息");
                txtGoodName.Focus();
                return false;
            }

            if (cmbStorage.Text.Trim() == "")
            {
                cmbStorage.Focus();
                MessageDialog.ShowPromptMessage("所属库房不允许为空!");
                return false;
            }

            return true;
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (m_strHandleAll == "未完成" || m_strHandleAll == null)
            {
                DataTable dt = (DataTable)dgv_Main.DataSource;

                if (dt.Rows.Count != 0)
                {
                    if (m_strBillStatus == "已保存"
                        && (!BasicInfo.ListRoles.Contains(CE_RoleEnum.成品仓库管理员.ToString())
                        && !BasicInfo.ListRoles.Contains(CE_RoleEnum.售后库房管理员.ToString())) || m_strBillStatus == null)
                    {
                        dt.Rows.RemoveAt(dgv_Main.CurrentRow.Index);
                        dgv_Main.DataSource = dt;
                    }
                }
                else
                {
                    ClearDate();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("单据已处理完成，不能进行此操作！");
            }
        }

        /// <summary>
        /// 完成物品信息检索
        /// </summary>
        private void txtGoodName_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodName.DataResult["图号型号"].ToString();
            txtType.Text = txtGoodName.DataResult["物品类别"].ToString();
            txtSpce.Text = txtGoodName.DataResult["规格"].ToString();
            txtUnitPrice.Text = txtGoodName.DataResult["单价"].ToString();
            txtGoodName.Tag = txtGoodName.DataResult["序号"].ToString();
            txtGoodName.Text = txtGoodName.DataResult["物品名称"].ToString();
        }

        /// <summary>
        /// 保存信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_strBillStatus == "已解封" || m_strHandleAll == "全部处理" || m_strBillStatus == "已处理")
                {
                    MessageDialog.ShowPromptMessage("单据不能进行此操作！");
                    return;
                }

                if (txtQuarantine.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请填写隔离原因！");
                    return;
                }

                string strDJH = txtBill_ID.Text;
                DataTable dt = (DataTable)dgv_Main.DataSource;

                if (dt.Rows.Count > 0)
                {
                    S_QuarantineBill dataMain = new S_QuarantineBill();

                    dataMain.Bill_ID = strDJH;
                    dataMain.LRRY = txtLRRY.Text;
                    dataMain.LRRQ = ServerTime.Time;
                    dataMain.Remark = txtRemarkAll.Text;
                    dataMain.Storage = cmbStorage.Text;
                    dataMain.Department = txtLRBM.Text;
                    dataMain.Description = txtQuarantine.Text;
                    dataMain.IsHandle = "未完成";

                    bool DataMainOk = m_serverQuarantine.AddBill(dataMain, out m_strErr);

                    if (!DataMainOk)
                    {
                        MessageDialog.ShowErrorMessage(m_strErr);
                        return;
                    }
                    else
                    {
                        if (!m_serverQuarantine.AddList(dt, txtBill_ID.Text, out m_strErr))
                        {
                            MessageDialog.ShowErrorMessage(m_strErr);
                            return;
                        }
                    }

                    MessageDialog.ShowPromptMessage("单据号【 " + strDJH + " 】保存成功！");
                    this.Close();
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请选择产品信息");
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
        /// 仓管解封
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAffirm_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)this.dgv_Main.DataSource;

            if (m_strBillStatus != "已处理")
            {
                MessageDialog.ShowPromptMessage("请等待质管处理后，再进行此操作！");
                return;
            }

            if (dt.Rows.Count > 0)
            {
                int num = 0;

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dt.Rows[i]["IsHandle"].ToString()))
                    {
                        num = 1;
                    }

                    DataTable dtList = m_serverQuarantine.GetOperationStatus(txtBill_ID.Text, dt.Rows[i]["ProductCode"].ToString(), out m_strErr);

                    if (!Convert.ToBoolean(dtList.Rows[0]["IsHandle"].ToString()) && Convert.ToBoolean(dt.Rows[i]["IsHandle"].ToString()))
                    {
                        S_QuarantineList list = new S_QuarantineList();

                        list.Bill_ID = txtBill_ID.Text;
                        list.ProductStockCode = dt.Rows[i]["ProductCode"].ToString();
                        list.GoodID = int.Parse(dt.Rows[i]["GoodsID"].ToString());

                        m_blIsHandle = m_serverQuarantine.UpdateList(list, out m_strErr);

                        if (m_blIsHandle)
                        {
                            m_blIsHandle = m_serverQuarantine.UpdateProductStockStatus(
                                dt.Rows[i]["ProductCode"].ToString(), dt.Rows[i]["GoodsID"].ToString(), true, out m_strErr);
                        }
                    }
                }
                if (num == 0)
                {
                    MessageDialog.ShowPromptMessage("请勾选需要解封的数据！");
                    return;
                }

                if (m_blIsHandle)
                {
                    string str = "";
                    string status = "";

                    DataTable dtHandle = m_serverQuarantine.GetOperationStatus(txtBill_ID.Text, null, out m_strErr);

                    for (int i = 0; i < dtHandle.Rows.Count; i++)
                    {
                        if (dtHandle.Rows[i]["isHandle"].ToString() == "False")
                        {
                            str = "未完成";
                            status = "已处理";
                            break;
                        }
                        else
                        {
                            str = "全部处理";
                            status = "已解封";
                        }
                    }

                    m_blIsHandle = m_serverQuarantine.AuditingBill(txtBill_ID.Text, str, status, out m_strErr);

                    if (m_blIsHandle)
                    {
                        MessageDialog.ShowPromptMessage("解封成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_strErr);
                        return;
                    }
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region dgv点击事件
        private void dgv_Main_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow row = dgv_Main.CurrentRow;

            txtGoodName.Text=row.Cells["GoodsName"].Value.ToString();
            txtBatchNo.Text=row.Cells["ProductCode"].Value.ToString();
            txtSpce.Text=row.Cells["Spec"].Value.ToString();
            txtGoodsCode.Text=row.Cells["GoodsCode"].Value.ToString();
            txtType.Text=row.Cells["Depot"].Value.ToString();
            txtUnitPrice.Text=row.Cells["UnitPrice"].Value.ToString();
            txtRemark.Text = row.Cells["Remark"].Value.ToString();
            cmbStorage.Text = row.Cells["Storage"].Value.ToString();
            txtGoodName.Tag =  Convert.ToInt32( row.Cells["GoodsID"].Value);

            if (e.RowIndex >= 0)
            {
                S_QuarantineList list = new S_QuarantineList();

                list.Bill_ID = txtBill_ID.Text;
                list.ProductStockCode = dgv_Main.Rows[e.RowIndex].Cells["ProductCode"].Value.ToString();
                list.GoodID = int.Parse(dgv_Main.Rows[e.RowIndex].Cells["GoodsID"].Value.ToString());

                DataTable dt = m_serverQuarantine.GetOperationStatus(txtBill_ID.Text,list.ProductStockCode,out m_strErr);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (dt.Rows[i]["IsHandle"].ToString().Equals("False"))
                    {
                        if (e.ColumnIndex == 11)
                        {
                            if (Convert.ToBoolean(dgv_Main.CurrentRow.Cells["选"].Value))
                            {
                                dgv_Main.CurrentRow.Cells["选"].Value = false;
                            }
                            else
                            {
                                dgv_Main.CurrentRow.Cells["选"].Value = true;
                            }
                        }
                    }
                    else
                    {
                           return;
                    }     
                }
            }
        }
        #endregion

        private void txtBatchNo_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtBatchNo.DataResult["图号型号"].ToString();
            txtType.Text = txtBatchNo.DataResult["物品类别"].ToString();
            txtSpce.Text = txtBatchNo.DataResult["规格"].ToString();
            txtUnitPrice.Text = txtBatchNo.DataResult["单价"].ToString();
            txtGoodName.Tag = txtBatchNo.DataResult["序号"].ToString();
            txtBatchNo.Text = txtBatchNo.DataResult["箱体编号"].ToString();
            txtGoodName.Text = txtBatchNo.DataResult["物品名称"].ToString();
        }

        private void txtBatchNo_Enter(object sender, EventArgs e)
        {
            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);
            string strSql = " and storageID = '" + strStorage + "'";

            if (txtGoodName.Text.Trim() != "")
            {
                strSql += " and a.GoodsID='" + txtGoodName.Tag.ToString() + "'";
            }

            txtBatchNo.StrEndSql = strSql;
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (cmbStorage.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择库房！");
                return;
            }

            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);

            DataTable dtGood = (DataTable)dgv_Main.DataSource;

            if (txtGoodName.Text.Trim() != "")
            {
                FormProductStock frm = new FormProductStock(txtGoodName.Tag.ToString(), strStorage);
                frm.ShowDialog();

                dtGood = SetTable(frm.Dt, dtGood);
            }
            else
            {
                FormProductStock frm = new FormProductStock(strStorage);
                frm.ShowDialog();

                dtGood = SetTable(frm.Dt, dtGood);
            }

            dgv_Main.DataSource = dtGood;
        }

        private DataTable SetTable(DataTable dtNew, DataTable dtOld)
        {
            for (int i = 0; i < dtNew.Rows.Count; i++)
            {
                bool blFlag = true;

                if (dtOld != null)
                {
                    for (int k = 0; k < dtOld.Rows.Count; k++)
                    {
                        if (dtOld.Rows[k]["GoodsID"].ToString() == dtNew.Rows[i]["GoodsID"].ToString()
                            && dtOld.Rows[k]["ProductCode"].ToString() == dtNew.Rows[i]["箱体编号"].ToString())
                        {
                            blFlag = false;
                        }
                    }
                }
                else
                {
                    dtOld = new DataTable();
                }

                if (blFlag)
                {
                    DataTable dtGoodPlan = m_serverQuarantine.GetProductCodeInfo(dtNew.Rows[i]["GoodsID"].ToString());

                    string spec = "";
                    string depot = "";
                    string unitPrice = "";

                    foreach (DataRow item in dtGoodPlan.Rows)
                    {
                        spec = item["spec"].ToString();
                        depot = item["GoodsType"].ToString();
                        unitPrice = item["GoodsUnitPrice"].ToString();
                    }

                    DataRow dr = dtOld.NewRow();

                    dr["GoodsCode"] = dtNew.Rows[i]["图号型号"].ToString();//图形图号
                    dr["GoodsName"] = dtNew.Rows[i]["物品名称"].ToString();//物品名称
                    dr["ProductCode"] = dtNew.Rows[i]["箱体编号"].ToString();
                    dr["Storage"] = cmbStorage.Text;
                    dr["GoodsID"] = dtNew.Rows[i]["GoodsID"].ToString();
                    dr["Bill_ID"] = txtBill_ID.Text;
                    dr["Spec"] = spec;
                    dr["Depot"] = depot;//产品类型
                    dr["UnitPrice"] = unitPrice;
                    dr["Remark"] = txtRemark.Text;

                    dtOld.Rows.Add(dr);
                }
            }

            return dtOld;
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgv_Main.SelectedRows.Count - 1; i++)
            {
                dgv_Main.SelectedRows[i].Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dgv_Main.SelectedRows.Count - 1; i++)
            {
                dgv_Main.SelectedRows[i].Cells["选"].Value = Convert.ToBoolean( 
                    m_serverQuarantine.GetOperationStatus(txtBill_ID.Text, 
                    dgv_Main.SelectedRows[i].Cells["ProductCode"].Value.ToString(),
                    out m_strErr).Rows[0][0]);
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv_Main.Rows.Count; i++)
            {
                dgv_Main.Rows[i].Cells["选"].Value = Convert.ToBoolean(
                    m_serverQuarantine.GetOperationStatus(txtBill_ID.Text,
                    dgv_Main.Rows[i].Cells["ProductCode"].Value.ToString(),
                    out m_strErr).Rows[0][0]);
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgv_Main.Rows.Count; i++)
            {
                dgv_Main.Rows[i].Cells["选"].Value = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                txtDispose.ReadOnly = true;
                txtDisposeName.ReadOnly = true;
            }
        }

        private void 质管toolScripButton_Click(object sender, EventArgs e)
        {
            if (radioButton1.Checked || rdbRepair.Checked)
            {
                if (rdbRepair.Checked)
                {
                    if (txtDispose.Text.Trim() == "")
                    {
                        MessageDialog.ShowPromptMessage("请填写处理方案！");
                        return;
                    }

                    m_blIsHandle = m_serverQuarantine.HandleBill(txtBill_ID.Text, txtDisposeName.Text, txtDispose.Text, out m_strErr);

                    if (m_blIsHandle)
                    {
                        DataTable dt = (DataTable)this.dgv_Main.DataSource;

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            S_QuarantineList list = new S_QuarantineList();

                            list.Bill_ID = txtBill_ID.Text;
                            list.ProductStockCode = dt.Rows[i]["ProductCode"].ToString();
                            list.GoodID = int.Parse(dt.Rows[i]["GoodsID"].ToString());

                            m_blIsHandle = m_serverQuarantine.UpdateList(list, out m_strErr);

                            if (m_blIsHandle)
                            {
                                m_blIsHandle = m_serverQuarantine.AuditingBill(txtBill_ID.Text, "全部处理", "已处理", out m_strErr);
                            }

                        }

                        if (m_blIsHandle)
                        {

                            MessageDialog.ShowPromptMessage("质管处理完成！");
                        }
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }
                }
                else if (radioButton1.Checked)
                {
                    m_blIsHandle = m_serverQuarantine.AuditingBill(txtBill_ID.Text, "未完成", "已处理", out m_strErr);
                    MessageDialog.ShowPromptMessage("质管处理完成！");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择判定结果！");
                return;
            }

            this.Close();
        }

        private void rdbRepair_CheckedChanged(object sender, EventArgs e)
        {

            if (rdbRepair.Checked)
            {
                txtDispose.ReadOnly = false;
                txtDisposeName.ReadOnly = false;
            }
        }
    }
}
