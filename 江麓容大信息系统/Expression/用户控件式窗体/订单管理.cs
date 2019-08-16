/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlOrderFormInfo.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友   当前版本: V1.00
 *        修改说明: 创建
 *     2. 日期时间: 2010/09/03 08:02:08 作者: 夏石友 当前版本: V1.1
 *        修改说明: 修改成适合财务所需的版本
 ******************************************************************************/
using System;
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
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;
using CommonBusinessModule;

namespace Expression
{
    /// <summary>
    /// 订单信息组件
    /// </summary>
    public partial class UserControlOrderFormInfo : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的订单信息表
        /// </summary>
        IQueryable<View_B_OrderFormInfo> m_findOrderFormInfo;

        /// <summary>
        /// 查找到的符合条件的订单物品信息表
        /// </summary>
        IQueryable<View_B_OrderFormGoods> m_findOrderFormGoods;

        /// <summary>
        /// 订单服务组件
        /// </summary>
        IOrderFormInfoServer m_orderFormServer = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

        /// <summary>
        /// 订单物品服务组件
        /// </summary>
        IOrderFormGoodsServer m_serverOrderFormGoods = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();

        /// <summary>
        /// 供应商窗体
        /// </summary>
        FormQueryInfo m_formProvider;

        /// <summary>
        /// 用户窗体
        /// </summary>
        FormPersonnel m_formUser;

        /// <summary>
        /// 订单数据定位控件
        /// </summary>
        UserControlDataLocalizer m_orderFormLocalizer;

        /// <summary>
        /// 订单物品定位控件
        /// </summary>
        UserControlDataLocalizer m_goodsLocalizer;

        /// <summary>
        /// 旧订单号
        /// </summary>
        string m_oldOrderFormNumber;

        /// <summary>
        /// 订单类型字典(类型名称与ID构成的字典)
        /// </summary>
        Dictionary<string, int> m_dicOrderFormType = new Dictionary<string, int>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public UserControlOrderFormInfo(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_authFlag = nodeInfo.Authority;
        }

        private void UserControlOrderFormInfo_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlOrderFormInfo_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStripOrderForm, m_authFlag);
            FaceAuthoritySetting.SetVisibly(toolStripGoods, m_authFlag);
            
            // 如果订单信息操作工具栏不可见
            if (!toolStripOrderForm.Visible)
            {
                groupBoxOrderFormInfo.Height = groupBoxOrderFormInfo.Height - toolStripOrderForm.Height;
                groupBoxOrderFormGoods.Top -= toolStripOrderForm.Height;
                panelPara.Height -= toolStripOrderForm.Height;
            }

            if (!toolStripGoods.Visible)
            {
                groupBoxOrderFormGoods.Height = groupBoxOrderFormGoods.Height - toolStripOrderForm.Height;
                panelPara.Height -= toolStripOrderForm.Height;
            }

            #region 获取订单类型

            List<B_OrderFormType> lstType = m_orderFormServer.GetOrderFormType();

            m_dicOrderFormType.Clear();

            foreach(var item in lstType)
            {
                cmbOrderFormType.Items.Add(item.TypeName);
                m_dicOrderFormType.Add(item.TypeName, item.ID);
            }

            #endregion 获取订单类型

            btnRefresh_Click(sender, e);
        }

        /// <summary>
        /// 刷新订单信息DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshOrderFormDataGridView()
        {
            string strSql = "select * from View_B_OrderFormInfo order by 订货日期 desc, 订单号 desc ";
            DataTable dataSource = GlobalObject.DatabaseServer.QueryInfo(strSql);

            dataGridView1.DataSource = dataSource;

            // 添加数据定位控件
            if (m_orderFormLocalizer == null)
            {
                m_orderFormLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                groupBoxOrderFormInfo.Controls.Add(m_orderFormLocalizer);
                m_orderFormLocalizer.Dock = DockStyle.Bottom;
                m_orderFormLocalizer.Visible = true;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Columns["权限控制用登录名"].Visible = false;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 刷新物品信息DataGridView
        /// </summary>
        /// <param name="goodsInfo">数据集</param>
        void RefreshGoodsDataGridView(IQueryable goodsInfo)
        {
            dataGridViewGoods.DataSource = goodsInfo;

            if (goodsInfo != null)
            {
                dataGridViewGoods.Columns["序号"].Visible = false;
            }

            if (m_goodsLocalizer == null)
            {
                m_goodsLocalizer = new UserControlDataLocalizer(dataGridViewGoods, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridViewGoods.Name, BasicInfo.LoginID));
                groupBoxOrderFormGoods.Controls.Add(m_goodsLocalizer);
                m_goodsLocalizer.Dock = DockStyle.Bottom;
                m_goodsLocalizer.Visible = true;
            }

            dataGridViewGoods.Refresh();
        }

        /// <summary>
        /// 查找图号型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            if (txtBargainNumber.Text.Length == 0)
            {
                txtBargainNumber.Focus();
                MessageDialog.ShowPromptMessage("请先选择合同号后再进行此操作！");
                return;
            }

            FormQueryInfo form = QueryInfoDialog.GetOrderFormGoodsDialog(txtProvider.Text);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Tag = (int)form.GetDataItem("序号");
                txtCode.Text = form.GetDataItem("图号型号").ToString();
                txtName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
        }

        /// <summary>
        /// 查找供应商
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindProvider_Click(object sender, EventArgs e)
        {
            m_formProvider = QueryInfoDialog.GetProviderInfoDialog();

            if (m_formProvider.ShowDialog() == DialogResult.OK)
            {
                txtProvider.Text = m_formProvider.GetStringDataItem("供应商编码");
            }
        }

        /// <summary>
        /// 查找采购员
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindBuyer_Click(object sender, EventArgs e)
        {
            m_formUser = new FormPersonnel(txtBuyer, "CG", "姓名");
            m_formUser.ShowDialog();

            txtBuyer.Tag = m_formUser.UserCode;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckOrderFormDataItem())
            {
                return;
            }

            txtOrderFormNumber.Text = m_orderFormServer.GetOrderNo(txtProvider.Text);
            txtBargainNumber.Text = "价格清单";

            B_OrderFormInfo info = new B_OrderFormInfo();

            info.TypeID = m_dicOrderFormType[cmbOrderFormType.Text];
            info.OrderFormNumber = txtOrderFormNumber.Text;
            info.BargainNumber = txtBargainNumber.Text;
            info.Buyer = txtBuyer.Tag.ToString();
            info.CreateDate = dateTimeOrder.Value;
            info.InputPerson = BasicInfo.LoginName;
            info.Provider = txtProvider.Text;
            info.ProviderPhone = txtProviderPhone.Text;
            info.ProviderLinkman = txtProviderLinkman.Text;
            info.ProviderFax = txtProviderFax.Text;
            info.ProviderEmail = txtProviderEMail.Text;
            info.Remark = txtRemark.Text;

            if (!m_orderFormServer.AddOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID, info, out m_findOrderFormInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("已添加订单，【订单编号】：" + info.OrderFormNumber);
            }

            ClearControl();
            RefreshOrderFormDataGridView();
            PositioningRecord(info.OrderFormNumber);
            RefreshOrderFormControl();
            txtOrderFormNumber.ReadOnly = true;
        }

        /// <summary>
        /// 判断是否允许往订单中增加此物品
        /// </summary>
        /// <param name="operation">操作方式, 添加或更新</param>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="goods">要增加或更新的物品信息</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>允许返回true, 否则返回false</returns>
        private bool AllowGoods(string operation, string bargainNumber, B_OrderFormGoods goods, out string error)
        {
            IQueryable<View_B_OrderFormInfo> orderFormGroup = m_orderFormServer.GetOrderFormCollection(bargainNumber);
            error = null;

            IQueryable<View_B_OrderFormGoods> goodsGroup = m_serverOrderFormGoods.GetOrderFormGoods(bargainNumber);

            if (goodsGroup == null || goodsGroup.Count() == 0)
                return true;

            List<View_B_OrderFormGoods> lstGoods = goodsGroup.ToList();
            View_B_OrderFormGoods findResult = lstGoods.Find(p => p.物品ID == goods.GoodsID);

            if (findResult == null)
                return true;

            if (operation == "更新" && findResult.订单号 == goods.OrderFormNumber)
            {
                return true;
            }

            error = string.Format("{0} 订单已经存在物品：{1}，{2}，{3}，不允许再重复！",
                findResult.订单号, findResult.图号型号, findResult.物品名称, findResult.规格);

            return false;
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddGoods_Click(object sender, EventArgs e)
        {
            //if (m_orderFormServer.FindInDepotOrderGoodsCount(txtOrderFormNumber.Text.Trim()))
            //{
            //    MessageDialog.ShowPromptMessage("请先删除该订单关联的入库单再做此操作！");
            //    return;
            //}

            if (!CheckGoodsDataItem())
            {
                return;
            }

            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable(dataGridViewGoods.DataSource as IQueryable);

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["图号型号"].ToString() == txtCode.Text
                    && dt.Rows[i]["物品名称"].ToString() == txtName.Text
                    && dt.Rows[i]["规格"].ToString() == txtSpec.Text)
                {
                    MessageDialog.ShowPromptMessage("不能添加同种物品！");
                    return;
                }
            }

            B_OrderFormGoods goods = new B_OrderFormGoods();

            goods.ID = 1;
            goods.OrderFormNumber = txtOrderFormNumber.Text;
            goods.Amount = Convert.ToInt32(numAmount.Value);
            goods.GoodsID = (int)txtCode.Tag;
            goods.CreateDate = dateTimeStartDate.Value;
            goods.ArrivalDate = dateTimeArrivalDate.Value;  // 要求到货日期
            goods.Remark = txtGoodsRemark.Text;

            if (!m_serverOrderFormGoods.AddOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, 
                goods, out m_findOrderFormGoods, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshGoodsDataGridView(m_findOrderFormGoods);
            PositioningRecord(goods.OrderFormNumber,goods.GoodsID.ToString());
            RefreshGoodsControl();
        }

        /// <summary>
        /// 检测是否允许添加/修改订单信息
        /// </summary> 
        bool CheckOrderFormDataItem()
        {
            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("供货单位不能为空!");
                return false;
            }

            if (txtBuyer.Text == "")
            {
                txtBuyer.Focus();
                MessageDialog.ShowPromptMessage("订货员不能为空!");
                return false;
            }

            if (cmbOrderFormType.SelectedIndex < 0)
            {
                cmbOrderFormType.Focus();
                MessageDialog.ShowPromptMessage("订单类型不能为空!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测是否允许添加/修改物品信息
        /// </summary> 
        bool CheckGoodsDataItem()
        {
            if (txtOrderFormNumber.Text == "")
            {
                txtOrderFormNumber.Focus();
                MessageDialog.ShowPromptMessage("订单号不能为空!");
                return false;
            }

            if (txtName.Text == "")
            {
                btnFindCode.Focus();
                MessageDialog.ShowPromptMessage("物品名称不能为空!");
                return false;
            }

            if (numAmount.Value == 0)
            {
                numAmount.Focus();
                MessageDialog.ShowPromptMessage("订货数量不能为0!");
                return false;
            }

            if (dateTimeArrivalDate.Value.Date < dateTimeStartDate.Value.Date)
            {
                dateTimeArrivalDate.Focus();
                MessageDialog.ShowPromptMessage("要求到货日期设置不正确，不能比订货日期早!");
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
            int n = dataGridView1.SelectedRows.Count;

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }

            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["录入人员"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("只有此单据编制人才可执行删除操作！");
            }

            string orderFormNumber = dataGridView1.SelectedRows[0].Cells["订单号"].Value.ToString();
            View_B_OrderFormInfo info = m_orderFormServer.GetOrderFormInfo(orderFormNumber);

            if (BasicInfo.LoginName != info.录入人员)
            {
                MessageDialog.ShowPromptMessage(string.Format("您无法删除由 {0} 录入的信息！", info.录入人员));
                return;
            }

            if (MessageBox.Show("您是否确定要删除订单号为：" + txtOrderFormNumber.Text + "的信息?", "消息", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (m_orderFormServer.FindInDepotOrderGoodsCount(txtOrderFormNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请先删除该订单关联的入库单再做此操作！");
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;

            if (!m_orderFormServer.DeleteOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID, 
                txtOrderFormNumber.Text, out m_findOrderFormInfo, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                ClearControl();
                RefreshOrderFormDataGridView();
                PositioningRecord("订单信息", rowIndex);
                RefreshOrderFormControl();
            }

            txtOrderFormNumber.ReadOnly = true;
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteGoods_Click(object sender, EventArgs e)
        {
            int n = dataGridViewGoods.SelectedRows.Count;

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }

            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["录入人员"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("只有此单据编制人才可执行删除操作！");
            }

            string orderFormNumber = dataGridViewGoods.SelectedRows[0].Cells["订单号"].Value.ToString();
            View_B_OrderFormInfo info = m_orderFormServer.GetOrderFormInfo(orderFormNumber);

            if (BasicInfo.LoginName != info.录入人员)
            {
                MessageDialog.ShowPromptMessage(string.Format("您无法删除由 {0} 录入的信息！", info.录入人员));
                return;
            }

            string msg = string.Format("您是否确定要删除名称：{0}，规格：{1}的物品信息?", 
                dataGridViewGoods.SelectedRows[0].Cells["物品名称"].Value, dataGridViewGoods.SelectedRows[0].Cells["规格"].Value);

            if (MessageBox.Show(msg, "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (m_orderFormServer.FindInDepotOrderGoodsCount(txtOrderFormNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请先删除该订单关联的入库单再做此操作！");
                return;
            }

            int id = (int)dataGridViewGoods.SelectedRows[0].Cells["序号"].Value;
            int rowIndex = dataGridViewGoods.SelectedRows[0].Index;

            if (!m_serverOrderFormGoods.DeleteOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, id, 
                out m_findOrderFormGoods, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }

            RefreshGoodsDataGridView(m_findOrderFormGoods);
            PositioningRecord("订单物品", rowIndex);
            RefreshGoodsControl();
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ClearControl()
        {
            txtOrderFormNumber.Text = "系统自动生成";
            txtBargainNumber.Text = "";
            txtBuyer.Text = "";
            dateTimeOrder.Value = ServerModule.ServerTime.Time;

            txtProvider.Text = "";
            txtProviderLinkman.Text = "";
            txtProviderPhone.Text = "";
            txtProviderFax.Text = "";
            txtProviderEMail.Text = "";
            txtRemark.Text = "";
            txtOrderFormNumber.ReadOnly = false;
            ClearGoodsControl();
        }

        /// <summary>
        /// 重置物品板参数
        /// </summary>
        void ClearGoodsControl()
        {
            txtCode.Tag = null;
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            txtGoodsRemark.Text = "";

            dateTimeStartDate.Value = ServerModule.ServerTime.Time;
            dateTimeArrivalDate.Value = ServerModule.ServerTime.Time;
            numAmount.Value = 0;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (m_orderFormServer.FindInDepotOrderGoodsCount(txtOrderFormNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请先删除该订单关联的入库单再做此操作！");
                return;
            }

            if (!CheckOrderFormDataItem())
            {
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }

            B_OrderFormInfo info = new B_OrderFormInfo();

            info.TypeID = m_dicOrderFormType[cmbOrderFormType.Text];
            info.OrderFormNumber = txtOrderFormNumber.Text;
            info.BargainNumber = txtBargainNumber.Text;
            info.Buyer = txtBuyer.Tag.ToString();
            info.CreateDate = dateTimeOrder.Value;
            info.InputPerson = BasicInfo.LoginName;
            info.Provider = txtProvider.Text;
            info.ProviderPhone = txtProviderPhone.Text;
            info.ProviderLinkman = txtProviderLinkman.Text;
            info.ProviderFax = txtProviderFax.Text;
            info.ProviderEmail = txtProviderEMail.Text;
            info.Remark = txtRemark.Text;

            if (!m_orderFormServer.UpdateOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID,
                txtOrderFormNumber.Text, info, out m_findOrderFormInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshOrderFormDataGridView();
            PositioningRecord(info.OrderFormNumber);
            RefreshOrderFormControl();
            txtOrderFormNumber.ReadOnly = true;
        }

        /// <summary>
        /// 修改物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyGoods_Click(object sender, EventArgs e)
        {
            if (!CheckGoodsDataItem())
            {
                return;
            }

            if (dataGridViewGoods.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }

            if (m_orderFormServer.FindInDepotOrderGoodsCount(txtOrderFormNumber.Text.Trim()))
            {
                MessageDialog.ShowPromptMessage("请先删除该订单关联的入库单再做此操作！");
                return;
            }

            B_OrderFormGoods goods = new B_OrderFormGoods();

            goods.OrderFormNumber = txtOrderFormNumber.Text;
            goods.Amount = Convert.ToInt32(numAmount.Value);
            goods.GoodsID = (int)txtCode.Tag;
            goods.CreateDate = dateTimeStartDate.Value;
            goods.Remark = txtGoodsRemark.Text;
            goods.ArrivalDate = dateTimeArrivalDate.Value;  // 要求到货日期

            int id = (int)dataGridViewGoods.SelectedRows[0].Cells["序号"].Value;

            if (!m_serverOrderFormGoods.UpdateOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, id, 
                goods, out m_findOrderFormGoods, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshGoodsDataGridView(m_findOrderFormGoods);
            PositioningRecord(goods.OrderFormNumber, goods.GoodsID.ToString());
            RefreshGoodsControl();
        }

        /// <summary>
        /// 点击dataGridView1单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshOrderFormControl();
            btnRefreshGoods_Click(sender, e);

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridViewGoods.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            if (dataGridViewGoods.Rows.Count > 0)
            {
                dataGridViewGoods.CurrentCell = dataGridViewGoods.Rows[0].Cells[strColName];
            }

            RefreshGoodsControl();
        }

        /// <summary>
        /// 刷新订单控件
        /// </summary>
        void RefreshOrderFormControl()
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            txtOrderFormNumber.Text = dataGridView1.CurrentRow.Cells["订单号"].Value.ToString();
            txtBargainNumber.Text = dataGridView1.CurrentRow.Cells["合同号"].Value.ToString();
            txtProvider.Text = dataGridView1.CurrentRow.Cells["供货单位"].Value.ToString();
            txtProviderLinkman.Text = dataGridView1.CurrentRow.Cells["供应商联系人"].Value.ToString();
            txtProviderPhone.Text = dataGridView1.CurrentRow.Cells["供应商联系电话"].Value.ToString();
            txtProviderFax.Text = dataGridView1.CurrentRow.Cells["供应商传真号"].Value.ToString();
            txtProviderEMail.Text = dataGridView1.CurrentRow.Cells["供应商电子邮件"].Value.ToString();
            txtBuyer.Text = dataGridView1.CurrentRow.Cells["订货员"].Value.ToString();

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(txtOrderFormNumber.Text))
            {
                txtBuyer.Tag = m_orderFormServer.GetBuyerCode(txtOrderFormNumber.Text);
            }

            cmbOrderFormType.Text = dataGridView1.CurrentRow.Cells["订单类型"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();

            dateTimeOrder.Value = (DateTime)dataGridView1.CurrentRow.Cells["订货日期"].Value;

            m_oldOrderFormNumber = txtOrderFormNumber.Text;
        }

        /// <summary>
        /// 定位订单记录
        /// </summary>
        /// <param name="orderFormNumber">定位用的订单号</param>
        void PositioningRecord(string orderFormNumber)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((string)dataGridView1.Rows[i].Cells["订单号"].Value == orderFormNumber)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">定位行</param>
        void PositioningRecord(string tableName, int rowIndex)
        {
            if (tableName == "订单信息")
            {
                string strColName = "";

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    if (col.Visible)
                    {
                        strColName = col.Name;
                        break;
                    }
                }
                if (rowIndex >= dataGridView1.Rows.Count)
                {
                    return;
                }

                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[strColName];
            }
            else if (tableName == "订单物品")
            {
                string strColName = "";

                foreach (DataGridViewColumn col in dataGridViewGoods.Columns)
                {
                    if (col.Visible)
                    {
                        strColName = col.Name;
                        break;
                    }
                }

                if (rowIndex >= dataGridViewGoods.Rows.Count)
                {
                    return;
                }

                dataGridViewGoods.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridViewGoods.CurrentCell = dataGridViewGoods.Rows[rowIndex].Cells[strColName];
            }
        }

        /// <summary>
        /// 定位物品记录
        /// </summary>
        /// <param name="orderFormNo">定位用的订单号</param>
        /// <param name="goodsNo">物品ID</param>
        /// <param name="spec">规格</param>
        void PositioningRecord(string orderFormNo, string goodsNo)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridViewGoods.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridViewGoods.Rows.Count; i++)
            {
                if ((string)dataGridViewGoods.Rows[i].Cells["订单号"].Value == orderFormNo &&
                    (string)dataGridViewGoods.Rows[i].Cells["物品名称"].Value == goodsNo)
                {
                    dataGridViewGoods.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewGoods.CurrentCell = dataGridViewGoods.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新物品控件
        /// </summary>
        void RefreshGoodsControl()
        {
            ClearGoodsControl();

            if (dataGridViewGoods.CurrentRow == null)
            {
                return;
            }

            txtOrderFormNumber.Text = dataGridViewGoods.CurrentRow.Cells["订单号"].Value.ToString();
            txtCode.Text = dataGridViewGoods.CurrentRow.Cells["图号型号"].Value.ToString();
            txtName.Text = dataGridViewGoods.CurrentRow.Cells["物品名称"].Value.ToString();
            txtSpec.Text = dataGridViewGoods.CurrentRow.Cells["规格"].Value.ToString();
            txtCode.Tag = (int)dataGridViewGoods.CurrentRow.Cells["物品ID"].Value;

            numAmount.Value = Convert.ToDecimal(dataGridViewGoods.CurrentRow.Cells["订货数量"].Value);
            dateTimeStartDate.Value = (DateTime)dataGridViewGoods.CurrentRow.Cells["订货日期"].Value;
            dateTimeArrivalDate.Value = (DateTime)dataGridViewGoods.CurrentRow.Cells["要求到货日期"].Value;

            txtGoodsRemark.Text = dataGridViewGoods.CurrentRow.Cells["备注"].Value.ToString();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridViewGoods_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshGoodsControl();
        }

        private void btnFindBargain_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetBargainInfoDialog();

            if (form != null && DialogResult.OK == form.ShowDialog())
            {
                txtBargainNumber.Text = form.GetDataItem("合同号").ToString();
                txtProvider.Text = form.GetDataItem("供货单位").ToString();
            }
        }

        private void btnSearches_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "订单信息综合查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();

            if (qr.DataCollection == null || qr.DataCollection.Tables.Count == 0)
            {
                return;
            }

            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (!m_orderFormServer.GetAllOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID, out m_findOrderFormInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshOrderFormDataGridView();
            RefreshOrderFormControl();
        }

        private void btnRefreshGoods_Click(object sender, EventArgs e)
        {
            IQueryable<View_B_OrderFormGoods> OrderFormGoods;

            if (!m_serverOrderFormGoods.GetOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, 
                txtOrderFormNumber.Text, out OrderFormGoods, out m_err))
            {
                if (!m_err.Contains("没有找到"))
                    MessageDialog.ShowErrorMessage(m_err);
            }

            RefreshGoodsDataGridView(OrderFormGoods);
        }

        private void btnNewGoods_Click(object sender, EventArgs e)
        {
            ClearGoodsControl();
        }

        private void btnNewInfo_Click(object sender, EventArgs e)
        {
            ClearControl();
        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSh_Click(object sender, EventArgs e)
        {
            if (m_orderFormServer.Auditing(BasicInfo.LoginName, txtOrderFormNumber.Text, out m_err))
            {
                MessageBox.Show("审核成功", "提示");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
            }
        }

        /// <summary>
        /// 控制按钮可操作性
        /// </summary>
        private void EnabledButton()
        {
            if (!m_orderFormServer.CheckDate(dataGridView1.CurrentRow.Cells["订单号"].Value.ToString()))
            {
                btnDelete.Enabled = true;
                btnUpdate.Enabled = true;
                btnDeleteGoods.Enabled = true;
                btnModifyGoods.Enabled = true;
            }
            else
            {
                btnDelete.Enabled = false;
                btnUpdate.Enabled = false;
                btnDeleteGoods.Enabled = false;
                btnModifyGoods.Enabled = false;
            }
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

        private void btnCloseOrderForm_Click(object sender, EventArgs e)
        {
            未到货订单统计 frm = new 未到货订单统计();
            frm.ShowDialog();
        }
    }
}
