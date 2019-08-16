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
using System.Reflection;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 装配多批次零件管理 : Form
    {
        /// <summary>
        /// 多批次零件服务
        /// </summary>
        IMultiBatchPartServer m_mbpServer = PMS_ServerFactory.GetServerModule<IMultiBatchPartServer>();

        /// <summary>
        /// 数据查询结果
        /// </summary>
        IEnumerable<View_ZPX_MultiBatchPart> m_dataResult = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 查询列名
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 是否显示装配多批次用途提示
        /// </summary>
        bool m_showPurposePrompt = true;

        /// <summary>
        /// 产品信息服务
        /// </summary>
        private IProductInfoServer m_productInfoServer = PMS_ServerFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 变速箱箱号
        /// </summary>
        string m_productNumber;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public 装配多批次零件管理(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            winFormPage1.PageSize = 2000;
            winFormPage1.RefreshData = new GlobalObject.DelegateCollection.NonArgumentHandle(this.GoToPage);

            cmbPurpose.DataSource = m_mbpServer.GetPersonnelPurpose().OrderByDescending(k => k.装配用途编号);
            cmbPurpose.DisplayMember = "装配用途名称";
            cmbPurpose.ValueMember = "装配用途编号";

            IQueryable<View_P_ProductInfo> productInfo = null;

            if (!m_productInfoServer.GetAllProductInfo(out productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }
            else
            {
                productInfo = from r in productInfo
                              where !r.产品类型名称.Contains("返修")
                              select r;

                cmbProductCode.DataSource = productInfo;
                cmbProductCode.DisplayMember = "产品类型编码";
                cmbProductCode.ValueMember = "产品类型编码";
            }
        }

        private void 装配多批次零件管理_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
            TempPermission();
        }

        /// <summary>
        /// 用途发生变化时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbPurpose_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPurpose.SelectedValue as View_ZPX_PersonnelAuthority != null)
                return;

            // 3:"下线车间在线返修", 4: "下线车间再制造"
            int purposeID = (int)cmbPurpose.SelectedValue;

            if (purposeID != 4 && purposeID != 5)
            {
                cmbProductCode.SelectedIndex = -1;
                cmbProductCode.Enabled = false;

                txtProductNumber.Text = "";
                txtProductNumber.Enabled = false;

                是否一次性物品.Enabled = false;
            }
            else
            {
                cmbProductCode.Enabled = true;
                txtProductNumber.Enabled = true;
                是否一次性物品.Enabled = true;
            }
        }

        /// <summary>
        /// 跳转页
        /// </summary>
        void GoToPage()
        {
            if (m_dataResult == null)
                return;

            IEnumerable<View_ZPX_MultiBatchPart> result = m_mbpServer.GetEnumerable<View_ZPX_MultiBatchPart>(
                m_dataResult, null, null, winFormPage1.PageSize, winFormPage1.PageIndex);

            dataGridView1.DataSource = GlobalObject.GeneralFunction.ConvertToDataTable<View_ZPX_MultiBatchPart>(result);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void 装配多批次零件管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 新建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNew_Click(object sender, EventArgs e)
        {
            numBarCode.Value = 1;
            numCount.Value = 0;
            txtRemark.Text = "";
        }

        /// <summary>
        /// 检查界面上的数据是否有效，为添加、更新、删除操作作准备
        /// </summary>
        /// <returns>有效返回true</returns>
        private bool CheckData()
        {
            m_productNumber = "";

            if (m_showPurposePrompt)
            {
                m_showPurposePrompt = false;

                if (MessageDialog.ShowEnquiryMessage("您确定选择的用途是【" + cmbPurpose.Text + "】吗？") == DialogResult.No)
                {
                    MessageDialog.ShowPromptMessage("请选择好具体用途后再进行此操作");
                    cmbPurpose.Focus();
                    return false;
                }
            }

            if (cmbPurpose.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择好具体用途后再进行此操作");
                cmbPurpose.Focus();
                return false;
            }

            // 4:"下线车间在线返修", 5: "下线车间再制造"
            int purposeID = (int)(cmbPurpose.SelectedValue);

            if ((purposeID == 4 || purposeID == 5))
            {
                if (cmbProductCode.Text == "")
                {
                    MessageDialog.ShowPromptMessage("请选择产品型号后再进行此操作");

                    cmbProductCode.Focus();
                    return false;
                }
                else
                {
                    if (DialogResult.No == MessageDialog.ShowEnquiryMessage(
                        "您确定选择当前的产品型号吗，选择的产品型号直接决定了领料单中的一次性物品是否能导入到多批次管理？"))
                    {
                        return false;
                    }
                }

                if (!是否一次性物品.Checked)
                {
                    if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtProductNumber.Text.Trim()))
                    {
                        MessageDialog.ShowPromptMessage("请录入产品编号后再进行此操作");

                        txtProductNumber.Focus();
                        return false;
                    }

                    // 校验箱号录入是否正确
                    m_productNumber = cmbProductCode.Text + " " + txtProductNumber.Text.Trim();

                    if (!SCM_Level02_ServerFactory.GetServerModule<IProductCodeServer>().VerifyProductCodesInfo(
                        cmbProductCode.Text, txtProductNumber.Text, GlobalObject.CE_BarCodeType.内部钢印码, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            if (!m_mbpServer.Add(string.Format("{0}({1})", BasicInfo.LoginID, BasicInfo.LoginName),
                Convert.ToInt32(cmbPurpose.SelectedValue), Convert.ToInt32(numBarCode.Value), m_productNumber,
                Convert.ToInt32(numCount.Value), out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            SearchData(Convert.ToInt32(numBarCode.Value));
        }

        /// <summary>
        /// 按日期检索数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData(0);
        }

        /// <summary>
        /// 检索数据(如果条形码 > 0 则使用条形码检索模式)
        /// </summary>
        /// <param name="barCode">要检索的条形码</param>
        void SearchData(int barCode)
        {
            if (barCode < 1)
            {
                m_dataResult = m_mbpServer.GetData(dateTimePickerST.Value, dateTimePickerET.Value);
            }
            else
            {
                m_dataResult = m_mbpServer.GetData(barCode);
            }

            winFormPage1.Count = m_dataResult.Count();

            if (m_dataResult.Count() != 0)
            {
                winFormPage1.PageIndex = 1;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有查找到所需的数据");

                if (dataGridView1.Rows.Count == 0)
                    return;
                else
                    btnNew_Click(null, null);
            }

            IEnumerable<View_ZPX_MultiBatchPart> result = m_mbpServer.GetEnumerable<View_ZPX_MultiBatchPart>(
                m_dataResult, null, null, winFormPage1.PageSize, 1);

            RefreshDataGridView(GlobalObject.GeneralFunction.ConvertToDataTable<View_ZPX_MultiBatchPart>(result));
        }

        /// <summary>
        /// 刷新数据控件
        /// </summary>
        /// <param name="dt"></param>
        private void RefreshDataGridView(DataTable dt)
        {
            dataGridView1.DataSource = dt;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(this.labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            numBarCode.Value = (int)dataGridView1.SelectedRows[0].Cells["条形码"].Value;
            numCount.Value = (int)dataGridView1.SelectedRows[0].Cells["数量"].Value;
            txtRemark.Text = (string)dataGridView1.SelectedRows[0].Cells["备注"].Value;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            #region 夏石友，2017-01-15，根据信息化系统变更处置申请单变更，CZ2017010000019
            //if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.质检员_装配.ToString()))
            if ((m_authFlag & AuthorityFlag.Delete) != AuthorityFlag.Delete)
            #endregion 夏石友，2017-01-15，根据信息化系统变更处置申请单变更，CZ2017010000019
            {
                if (numCount.Value > Convert.ToDecimal(dataGridView1.SelectedRows[0].Cells["数量"].Value))
                {
                    //MessageDialog.ShowPromptMessage("非【质检员_装配】角色，【修改数量】不能大于【原有数量】");
                    MessageDialog.ShowPromptMessage("无足够的权限，【修改数量】不能大于【原有数量】");
                    return;
                }
            }

            if (!m_mbpServer.Update(string.Format("{0}({1})", BasicInfo.LoginID, BasicInfo.LoginName),
                Convert.ToInt32(cmbPurpose.SelectedValue), Convert.ToInt32(numBarCode.Value), m_productNumber,
                Convert.ToInt32(numCount.Value), out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            SearchData(Convert.ToInt32(numBarCode.Value));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要删除行后再进行此操作");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要删除选择的记录吗？") == DialogResult.No)
                return;

            int barCodeId = 0;

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.SelectedRows[i].Cells;
                barCodeId = (int)cells["条形码"].Value;

                if (!m_mbpServer.Delete(Convert.ToInt32(cmbPurpose.SelectedValue), barCodeId, m_productNumber, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }
            }

            SearchData(0);
        }

        /// <summary>
        /// 从领料单导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadDataFromMRB_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            FormQueryInfo dialog = QueryInfoDialog.GetMaterialRequisitionBillDialog(BasicInfo.LoginID, CE_BillTypeEnum.领料单);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string billNo = dialog.GetStringDataItem("领料单号");

                if (MessageDialog.ShowEnquiryMessage("您确定要导入 " + billNo
                    + " 领料单的信息吗？此过程需要一段时间，是否继续？") == DialogResult.No)
                {
                    return;
                }

                // 领料单服务
                IMaterialRequisitionServer billServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionServer>();

                // 领料单物品清单服务
                IMaterialRequisitionGoodsServer goodsServer = ServerModuleFactory.GetServerModule<IMaterialRequisitionGoodsServer>();

                View_S_MaterialRequisition bill = billServer.GetBillView(billNo);

                List<View_S_MaterialRequisitionGoods> lstMRGoods = (from r in goodsServer.GetGoods(billNo)
                                                                    orderby r.显示位置
                                                                    select r).ToList();

                if (lstMRGoods.Count > 0)
                {
                    if (是否一次性物品.Checked && GlobalObject.GeneralFunction.IsNullOrEmpty(m_productNumber))
                    {
                        m_productNumber = cmbProductCode.Text;
                    }

                    List<StorageGoods> lstGoods = new List<StorageGoods>(lstMRGoods.Count);

                    foreach (var item in lstMRGoods)
                    {
                        StorageGoods goods = new StorageGoods();

                        goods.GoodsCode = item.图号型号;
                        goods.GoodsName = item.物品名称;
                        goods.Spec = item.规格;
                        goods.Provider = item.供应商编码;
                        goods.BatchNo = item.批次号;
                        goods.Quantity = item.实领数;
                        goods.StorageID = item.StorageID;

                        lstGoods.Add(goods);
                    }

                    if (!m_mbpServer.AddFromBill(BasicInfo.LoginID, Convert.ToInt32(
                        cmbPurpose.SelectedValue), m_productNumber, billNo, lstGoods, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("操作成功");

                        SearchData(0);
                    }
                }
            }
        }

        private void btnLoadDataFromMRRB_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            FormQueryInfo dialog = QueryInfoDialog.GetMaterialRequisitionBillDialog(BasicInfo.LoginID, CE_BillTypeEnum.领料退库单);

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string billNo = dialog.GetStringDataItem("退库单号");

                if (MessageDialog.ShowEnquiryMessage("您确定要导入 " + billNo
                    + " 领料单的信息吗？此过程需要一段时间，是否继续？") == DialogResult.No)
                {
                    return;
                }

                IMaterialReturnedInTheDepot billServer = ServerModuleFactory.GetServerModule<IMaterialReturnedInTheDepot>();
                IMaterialListReturnedInTheDepot goodsServer = ServerModuleFactory.GetServerModule<IMaterialListReturnedInTheDepot>();

                View_S_MaterialReturnedInTheDepot bill = billServer.GetBillView(billNo);
                List<View_S_MaterialListReturnedInTheDepot> lstMRGoods = (from r in goodsServer.GetGoods(billNo) select r).ToList();

                if (lstMRGoods.Count > 0)
                {
                    if (是否一次性物品.Checked && GlobalObject.GeneralFunction.IsNullOrEmpty(m_productNumber))
                    {
                        m_productNumber = cmbProductCode.Text;
                    }

                    List<StorageGoods> lstGoods = new List<StorageGoods>(lstMRGoods.Count);

                    foreach (var item in lstMRGoods)
                    {
                        StorageGoods goods = new StorageGoods();

                        goods.GoodsCode = item.图号型号;
                        goods.GoodsName = item.物品名称;
                        goods.Spec = item.规格;
                        goods.Provider = item.供应商;
                        goods.BatchNo = item.批次号;
                        goods.Quantity = -(decimal)item.退库数;
                        goods.StorageID = bill.库房代码;

                        lstGoods.Add(goods);
                    }

                    if (!m_mbpServer.AddFromBill(BasicInfo.LoginID, Convert.ToInt32(
                        cmbPurpose.SelectedValue), m_productNumber, billNo, lstGoods, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("操作成功");

                        SearchData(0);
                    }
                }
            }
        }

        private void btnAdvSearch_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "装配线多批次管理查询";
            IQueryResult qr = null;

            if (m_lstFindField.Count == 0)
            {
                qr = authorization.Query(businessID, null, null, 0);

                DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

                if (qr.Succeeded && columns.Count > 0)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        m_lstFindField.Add(columns[i].ColumnName);
                    }
                }
            }

            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
                return;

            qr = authorization.Query(businessID, null, formFindCondition.SearchSQL, -1);

            dataGridView1.DataSource = qr.DataCollection.Tables[0];
            dataGridView1.Refresh();

            m_dataResult = null;
            this.winFormPage1.Count = 0;
        }

        /// <summary>
        /// 获取指定条形码的数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearchBarcode_Click(object sender, EventArgs e)
        {
            SearchData(Convert.ToInt32(numBarCode.Value));
            dataGridView1_CellClick(null, null);
        }

        private void 是否一次性物品_CheckedChanged(object sender, EventArgs e)
        {
            if (是否一次性物品.Checked)
            {
                if (MessageDialog.ShowEnquiryMessage("如果是从领料单中导入数据且领料单中包含非一次性物料则不能勾选此选项，是否继续此操作？") != DialogResult.Yes)
                {
                    this.是否一次性物品.CheckedChanged -= new System.EventHandler(this.是否一次性物品_CheckedChanged);

                    是否一次性物品.Checked = !是否一次性物品.Checked;

                    this.是否一次性物品.CheckedChanged += new System.EventHandler(this.是否一次性物品_CheckedChanged);
                }
            }
        }

        /// <summary>
        /// 从营销出库单导入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnLoadDataFromOutboundOrder_Click(object sender, EventArgs e)
        {
            if (!CheckData())
                return;

            FormQueryInfo dialog = QueryInfoDialog.GetOutboundBillDialog();

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string billNo = dialog.GetStringDataItem("单据号");

                if (MessageDialog.ShowEnquiryMessage("您确定要导入 " + billNo
                    + " 营销出库单的信息吗？此过程需要一段时间，是否继续？") == DialogResult.No)
                {
                    return;
                }

                // 营销出库单服务
                ISellIn billServer = ServerModuleFactory.GetServerModule<ISellIn>();

                DataTable bill = billServer.GetBill(billNo, 0);

                string storageID = bill.Rows[0]["StorageID"].ToString();

                DataTable list = billServer.GetList((int)bill.Rows[0]["ID"]);

                if (list.Rows.Count > 0)
                {
                    List<StorageGoods> lstGoods = new List<StorageGoods>(list.Rows.Count);

                    for (int i = 0; i < list.Rows.Count; i++)
                    {
                        StorageGoods goods = new StorageGoods();

                        goods.GoodsCode = list.Rows[i]["GoodsCode"].ToString();
                        goods.GoodsName = list.Rows[i]["GoodsName"].ToString();
                        goods.Spec = list.Rows[i]["Spec"].ToString();
                        goods.Provider = list.Rows[i]["Provider"].ToString();
                        goods.BatchNo = list.Rows[i]["BatchNo"].ToString();
                        goods.Quantity = (decimal)list.Rows[i]["Count"];
                        goods.StorageID = storageID;

                        lstGoods.Add(goods);
                    }

                    if (是否一次性物品.Checked && GlobalObject.GeneralFunction.IsNullOrEmpty(m_productNumber))
                    {
                        m_productNumber = cmbProductCode.Text;
                    }

                    if (!m_mbpServer.AddFromBill(BasicInfo.LoginID, Convert.ToInt32(
                        cmbPurpose.SelectedValue), m_productNumber, billNo, lstGoods, out m_error))
                    {
                        MessageDialog.ShowErrorMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("操作成功");

                        SearchData(0);
                    }
                }
            }
        }

        /// <summary>
        /// 删除用途中的所有多批次信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnBatchDelete_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(cmbPurpose.Text))
            {
                MessageDialog.ShowPromptMessage("请选择要删除的用途");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage(
                string.Format("您真的要删除用途 {0} 中的所有多批次信息吗？", cmbPurpose.Text)) == DialogResult.Yes)
            {
                if (!m_mbpServer.Delete(Convert.ToInt32(cmbPurpose.SelectedValue), out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("操作成功");

                    SearchData(0);
                }
            }
        }

        void TempPermission()
        {
            #region 夏石友，2017-01-15，根据信息化系统变更处置申请单变更，CZ2017010000019
            //if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.质检员_装配.ToString()))
            if ((m_authFlag & AuthorityFlag.Delete) != AuthorityFlag.Delete)
            #endregion 夏石友，2017-01-15，根据信息化系统变更处置申请单变更，CZ2017010000019
            {
                btnAdd.Visible = false;
                btnDelete.Visible = false;
                btnBatchDelete.Visible = false;

                //cmbProductCode.Enabled = false;
                //cmbPurpose.Enabled = false;
                //txtProductNumber.Enabled = false;
                //numBarCode.Enabled = false;
                //是否一次性物品.Enabled = false;
            }
        }
    }
}
