/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlBargainInfo.cs
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
    /// 合同信息组件
    /// </summary>
    public partial class UserControlBargainInfo : Form
    {
        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 单据号服务组件
        /// </summary>
        IAssignBillNoServer m_getbillno = ServerModuleFactory.GetServerModule<IAssignBillNoServer>();
        
        /// <summary>
        /// 查找到的符合条件的订单物品信息表
        /// </summary>
        IQueryable<View_B_OrderFormGoods> m_findOrderFormGoods;

        /// <summary>
        /// 查找到的符合条件的订单物品信息表
        /// </summary>
        IQueryable<View_B_OrderFormInfo> m_findOrderFormInfo;

        /// <summary>
        /// 订单服务组件
        /// </summary>
        IOrderFormInfoServer m_orderFormServer = ServerModuleFactory.GetServerModule<IOrderFormInfoServer>();

        /// <summary>
        /// 订单物品服务组件
        /// </summary>
        IOrderFormGoodsServer m_serverOrderFormGoods = ServerModuleFactory.GetServerModule<IOrderFormGoodsServer>();

        /// <summary>
        /// 零星采购单服务组件
        /// </summary>
        IMinorPurchaseBillServer m_minorBillServer =
            ServerModule.ServerModuleFactory.GetServerModule<IMinorPurchaseBillServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 查找到的符合条件的合同信息表
        /// </summary>
        IQueryResult m_findBargainInfo;

        /// <summary>
        /// 查找到的符合条件的合同物品信息表
        /// </summary>
        IQueryable<View_B_BargainGoods> m_findBargainGoods;

        /// <summary>
        /// 合同服务组件
        /// </summary>
        IBargainInfoServer m_bargainInfoServer = ServerModuleFactory.GetServerModule<IBargainInfoServer>();

        /// <summary>
        /// 合同物品服务组件
        /// </summary>
        IBargainGoodsServer m_bargainGoodsServer = ServerModuleFactory.GetServerModule<IBargainGoodsServer>();

        /// <summary>
        /// 供应商窗体
        /// </summary>
        FormQueryInfo m_formProvider;

        /// <summary>
        /// 用户窗体
        /// </summary>
        FormPersonnel m_formUser;

        /// <summary>
        /// 合同数据定位控件
        /// </summary>
        UserControlDataLocalizer m_bargainLocalizer;

        /// <summary>
        /// 合同物品数据定位控件
        /// </summary>
        UserControlDataLocalizer m_bargainGoodsLocalizer;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 旧合同号
        /// </summary>
        string m_oldBargainNumber;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        public UserControlBargainInfo(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();

            m_authFlag = nodeInfo.Authority;

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.采购员.ToString()))
            {
                txtBuyer.ReadOnly = true;
                txtBuyer.Tag = BasicInfo.LoginID;
            }


            m_billMessageServer.BillType = CE_BillTypeEnum.合同管理.ToString();
        }

        private void UserControlBargainInfo_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlBargainInfo_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStripBargain, m_authFlag);
            FaceAuthoritySetting.SetVisibly(toolStripGoods, m_authFlag);

            DataTable dt = m_serverBasicGoods.GetDistinctGoodsName();

            txtName.DataSource = dt;
            txtName.DisplayMember = "GoodsName";
            txtName.Text = "";

            dt = m_serverBasicGoods.GetDistinctGoodsCode();
            txtCode.DataSource = dt;
            txtCode.DisplayMember = "GoodsCode";
            txtCode.Text = "";

            this.txtName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtName.AutoCompleteSource = AutoCompleteSource.ListItems;


            this.txtSpec.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtSpec.AutoCompleteSource = AutoCompleteSource.ListItems;

            this.txtCode.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.txtCode.AutoCompleteSource = AutoCompleteSource.ListItems;

            if (BasicInfo.DeptCode == "XZ")
            {
                txtBargainNumber.ReadOnly = true;
                chkIsLx.Checked = true;
                ckbIsVirtualBargain.Checked = true;
                chkIsLx.Enabled = false;
                ckbIsVirtualBargain.Enabled = false;
            }

            // 如果合同信息操作工具栏不可见
            if (!toolStripBargain.Visible)
            {
                groupBoxBargain.Height = groupBoxBargain.Height - toolStripBargain.Height;
                groupBoxBargainGoods.Top -= toolStripBargain.Height;
                panelPara.Height -= toolStripBargain.Height;
            }

            if (!toolStripGoods.Visible)
            {
                groupBoxBargainGoods.Height = groupBoxBargainGoods.Height - toolStripBargain.Height;
                panelPara.Height -= toolStripBargain.Height;
            }

            m_bargainInfoServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);
            
            btnRefreshBargain_Click(sender, e);
            btnRefreshDataGridViewGoods_Click(sender, e);

            // 添加数据定位控件
            if (m_bargainLocalizer == null)
            {
                m_bargainLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                groupBoxBargain.Controls.Add(m_bargainLocalizer);
                m_bargainLocalizer.Dock = DockStyle.Bottom;
                m_bargainLocalizer.Visible = true;
            }

            if (m_bargainGoodsLocalizer == null)
            {
                m_bargainGoodsLocalizer = new UserControlDataLocalizer(dataGridViewGoods, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridViewGoods.Name, BasicInfo.LoginID));
                groupBoxBargainGoods.Controls.Add(m_bargainGoodsLocalizer);
                m_bargainGoodsLocalizer.Dock = DockStyle.Bottom;
                m_bargainGoodsLocalizer.Visible = true;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="tableName">表名</param>
        /// <param name="rowIndex">定位行</param>
        void PositioningRecord(string tableName, int rowIndex)
        {
            if (tableName == "合同信息")
            {
                if (rowIndex >= dataGridView1.Rows.Count)
                {
                    return;
                }

                string strColName = "";

                foreach (DataGridViewColumn col in dataGridView1.Columns)
                {
                    if (col.Visible)
                    {
                        strColName = col.Name;
                        break;
                    }
                }

                dataGridView1.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridView1.CurrentCell = dataGridView1.Rows[rowIndex].Cells[strColName];
            }
            else if (tableName == "合同物品")
            {
                if (rowIndex >= dataGridViewGoods.Rows.Count)
                {
                    return;
                }

                string strColName = "";

                foreach (DataGridViewColumn col in dataGridViewGoods.Columns)
                {
                    if (col.Visible)
                    {
                        strColName = col.Name;
                        break;
                    }
                }

                dataGridViewGoods.FirstDisplayedScrollingRowIndex = rowIndex;
                dataGridViewGoods.CurrentCell = dataGridViewGoods.Rows[rowIndex].Cells[strColName];
            }
        }

        /// <summary>
        /// 定位合同记录
        /// </summary>
        /// <param name="bargainNo">定位用的合同号</param>
        void PositioningRecord(string bargainNo)
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
                if ((string)dataGridView1.Rows[i].Cells["合同号"].Value == bargainNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 定位物品记录
        /// </summary>
        /// <param name="bargainNo">定位用的合同号</param>
        /// <param name="goodsNo">物品ID</param>
        /// <param name="spec">规格</param>
        void PositioningRecord(string bargainNo, string goodsNo)
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
                if ((string)dataGridViewGoods.Rows[i].Cells["合同号"].Value == bargainNo &&
                    (string)dataGridViewGoods.Rows[i].Cells["物品ID"].Value.ToString() == goodsNo)
                {
                    dataGridViewGoods.FirstDisplayedScrollingRowIndex = i;
                    dataGridViewGoods.CurrentCell = dataGridViewGoods.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新合同信息DataGridView
        /// </summary>
        /// <param name="findBargainInfo">数据集</param>
        void RefreshBargainDataGridView(IQueryResult findBargainInfo)
        {
            if (findBargainInfo == null)
            {
                dataGridView1.DataSource = null;
                return;
            }

            if (findBargainInfo.DataCollection == null || findBargainInfo.DataCollection.Tables.Count == 0)
            {
                return;
            }

            DataTable dataSource = findBargainInfo.DataCollection.Tables[0];

            if (BasicInfo.DeptCode == "CG")
            {
                for (int i = 0; i < dataSource.Rows.Count; i++)
                {
                    if (dataSource.Rows[i].Field<string>("合同号").ToString().Substring(0, 2) == "XZ")
                    {
                        dataSource.Rows.RemoveAt(i--);
                    }
                }
            }

            this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            dataGridView1.DataSource = dataSource;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

            if (findBargainInfo != null)
            {
                dataGridView1.Columns["权限控制用登录名"].Visible = false;
            }

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

 

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

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

            dataGridViewGoods.Refresh();
        }

        /// <summary>
        /// 查找图号型号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFindCode_Click(object sender, EventArgs e)
        {
            //this.txtCode.TextChanged -= new EventHandler(txtCode_TextChanged); // 2013-06-17 夏石友屏蔽
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog(true);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtCode.Text = form.GetDataItem("图号型号").ToString().Trim();
                txtName.Text = form.GetDataItem("物品名称").ToString().Trim();
                txtSpec.Text = form.GetDataItem("规格").ToString().Trim();
                txtCode.Tag = form.GetDataItem("序号").ToString().Trim();
            }
            //this.txtCode.TextChanged += new EventHandler(txtCode_TextChanged); // 2013-06-17 夏石友屏蔽
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
            m_formUser = new FormPersonnel(txtBuyer, BasicInfo.DeptCode , "姓名");
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
            if (!CheckBargainDataItem())
            {
                return;
            }

            B_BargainInfo bi = new B_BargainInfo();
            bi.BargainNumber = txtBargainNumber.Text;

            if (txtBuyer.DataResult == null)
            {
                bi.Buyer = txtBuyer.Tag as string;
            }
            else
            {
                bi.Buyer = txtBuyer.DataResult["工号"].ToString();
            }
            
            bi.Cess = numCess.Value;
            bi.Date = dateTimePicker1.Value;
            bi.InputPerson = BasicInfo.LoginName;
            bi.LaisonMode = txtLaisonMode.Text;
            bi.Provider = txtProvider.Text;
            bi.ProviderLinkman = txtProviderLinkman.Text;
            bi.IsOverseas = chkIsOverseas.Checked;
            bi.IsConsignOut = chkIsConsignOut.Checked;
            bi.Remark = txtBillRemark.Text;
            bi.AuditDate = bi.BargainNumber.Contains("AutoH") == true ? (DateTime?)ServerTime.Time : null;
            bi.MinorPurchaseBillNo = txtMinorPurchaseBillNo.Text;

            if (!m_bargainInfoServer.AddBargainInfo(bi, out m_findBargainInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            if (bi.AuditDate == null)
            {
                m_billMessageServer.DestroyMessage(bi.BargainNumber);
                m_billMessageServer.SendNewFlowMessage(bi.BargainNumber, string.Format("【合同号】：{0} 【供应商】：{1}  ※※※ 等待【会计】处理", bi.BargainNumber, bi.Provider),
                    BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.会计.ToString());
            }

            ClearControl();
            RefreshBargainDataGridView(m_findBargainInfo);
            PositioningRecord(bi.BargainNumber);
            RefreshBargainControl();

            btnAdd.Enabled = false;
            chkIsOverseas.Enabled = false;
            chkIsConsignOut.Enabled = false;
            chkIsLx.Enabled = false;
            ckbIsVirtualBargain.Enabled = false;
        }

        /// <summary>
        /// 添加物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddGoods_Click(object sender, EventArgs e)
        {
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

            if (!m_bargainGoodsServer.IsOperator(Convert.ToInt32(txtCode.Tag), txtBargainNumber.Text))
            {
                MessageDialog.ShowPromptMessage("此物品已有业务，不能被操作");
                return;
            }

            B_BargainGoods bg = new B_BargainGoods();

            bg.BargainNumber = txtBargainNumber.Text;
            bg.Amount = Convert.ToInt32(numAmount.Value);
            bg.GoodsID = Convert.ToInt32(txtCode.Tag);
            bg.Remark = txtRemark.Text;
            bg.UnitPrice = numUnitPrice.Value;

            if (!m_bargainGoodsServer.AddBargainGoods(bg, out m_findBargainGoods, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshGoodsDataGridView(m_findBargainGoods);
            ClearGoodsControl();
        }

        /// <summary>
        /// 检测是否允许添加/修改合同信息
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        bool CheckBargainDataItem()
        {
            if (txtBargainNumber.Text == "")
            {
                txtBargainNumber.Focus();
                MessageDialog.ShowPromptMessage("合同号不能为空!");
                return false;
            }

            if (txtProvider.Text == "")
            {
                txtProvider.Focus();
                MessageDialog.ShowPromptMessage("供货单位不能为空!");
                return false;
            }

            if (txtBuyer.Text == "")
            {
                MessageDialog.ShowPromptMessage("采购员不能为空!");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检测是否允许添加/修改物品信息
        /// </summary>
        /// <returns>通过返回True，否则返回False</returns>
        bool CheckGoodsDataItem()
        {
            if (txtBargainNumber.Text == "")
            {
                txtBargainNumber.Focus();
                MessageDialog.ShowPromptMessage("合同号不能为空!");
                return false;
            }

            if (txtName.Text == "")
            {
                txtName.Focus();
                MessageDialog.ShowPromptMessage("物品名称不能为空!");
                return false;
            }

            //因为机加车间需要普通入库自制工装 单价可以为0， Modify by cjb on 2012.5.15
            if (numUnitPrice.Value == 0)
            {
                if (MessageBox.Show("此物品单价为0是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                {
                    numUnitPrice.Focus();
                    return false;
                }
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

            if (m_orderFormServer.FindBargainCount(txtBargainNumber.Text))
            {
                MessageDialog.ShowPromptMessage("请先删除该合同的关联订单，再做此操作!");
                return;
            }

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }

            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["合同录入员"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("只有此单据编制人才可执行删除操作！");
            }

            string bargainNumber = dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString();
            View_B_BargainInfo bargainInfo = m_bargainInfoServer.GetBargainInfo(bargainNumber);

            if (BasicInfo.LoginName != bargainInfo.合同录入员)
            {
                MessageDialog.ShowPromptMessage(string.Format("您无法删除由 {0} 录入的信息！", bargainInfo.合同录入员));
                return;
            }

            if (MessageBox.Show("您是否确定要删除合同号为" + txtBargainNumber.Text + "的合同信息?", "消息", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int rowIndex = dataGridView1.SelectedRows[0].Index;

            if (!m_bargainInfoServer.DeleteBargainInfo(txtBargainNumber.Text, out m_findBargainInfo, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                m_billMessageServer.DestroyMessage(txtBargainNumber.Text);
            }

            ClearControl();
            
            if (m_bargainInfoServer.GetAllBargainInfo(out m_findBargainInfo, out m_err))
            {
                RefreshBargainDataGridView(m_findBargainInfo);
                PositioningRecord("合同信息", rowIndex);
                RefreshBargainControl();
            }

            btnNewInfo.Enabled = true;
        }

        /// <summary>
        /// 删除物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDeleteGoods_Click(object sender, EventArgs e)
        {
            int n = dataGridViewGoods.SelectedRows.Count;

            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["合同录入员"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("只有此单据编制人才可执行删除操作！");
            }

            if (n == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的数据行!");
                return;
            }

            string msg = string.Format("您是否确定要删除名称：{0}，规格：{1}的物品信息?", 
                dataGridViewGoods.SelectedRows[0].Cells["物品名称"].Value, dataGridViewGoods.SelectedRows[0].Cells["规格"].Value);

            if (MessageBox.Show(msg, "消息", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            DataGridViewRow row = dataGridViewGoods.SelectedRows[0];
            string bargainNumber = row.Cells["合同号"].Value.ToString();
            View_B_BargainInfo bargainInfo = m_bargainInfoServer.GetBargainInfo(bargainNumber);

            if (BasicInfo.LoginName != bargainInfo.合同录入员)
            {
                MessageDialog.ShowPromptMessage(string.Format("您无法删除由 {0} 录入的信息！", bargainInfo.合同录入员));
                return;
            }

            if (!m_bargainGoodsServer.IsOperator(Convert.ToInt32(dataGridViewGoods.CurrentRow.Cells["物品ID"].Value),
                dataGridView1.CurrentRow.Cells["合同号"].Value.ToString()))
            {
                MessageDialog.ShowPromptMessage("此物品已有业务，不能被操作");
                return;
            }

            if (!m_bargainGoodsServer.DeleteBargainGoods(dataGridView1.CurrentRow.Cells["合同号"].Value.ToString(), 
                Convert.ToInt32(dataGridViewGoods.CurrentRow.Cells["物品ID"].Value), 
                out m_findBargainGoods, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }

            ClearGoodsControl();
            RefreshGoodsDataGridView(m_findBargainGoods);

            if (m_findBargainGoods.Count() == 0)
            {
                dataGridView1.CurrentRow.Cells["关联零星采购单"].Value = null;
                txtMinorPurchaseBillNo.Text = null;
            }

            PositioningRecord("合同物品", 0);
            RefreshGoodsControl();
        }

        /// <summary>
        /// 重置右面板参数
        /// </summary>
        void ClearControl()
        {
            txtProvider.Text = "";
            txtBuyer.Text = "";
            txtBargainNumber.Text = "";
            txtLaisonMode.Text = "";
            txtProviderLinkman.Text = "";
            numCess.Value = 0;

            chkIsOverseas.Enabled = true;
            chkIsConsignOut.Enabled = true;
            chkIsLx.Enabled = true;
            ckbIsVirtualBargain.Enabled = true;

            chkIsLx.Checked = false;
            ckbIsVirtualBargain.Checked = false;
            chkIsConsignOut.Checked = false;
            chkIsOverseas.Checked = false;

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
            numUnitPrice.Value = 0;
            numAmount.Value = 0;

            btnAddGoods.Enabled = true;
            零星采购toolStripButton.Enabled = true;
        }

        /// <summary>
        /// 修改合同信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckBargainDataItem())
            {
                return;
            }

            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["审核状态"].Value.ToString() == "已审核")
            {
                MessageDialog.ShowPromptMessage("合同已审核，不能进行修改");
                return;
            }

            if (m_orderFormServer.FindBargainCount(txtBargainNumber.Text))
            {
                MessageDialog.ShowPromptMessage("请先删除该合同的关联订单，再做此操作!");
                return;
            }

            B_BargainInfo bi = new B_BargainInfo();
            bi.BargainNumber = txtBargainNumber.Text;

            if (txtBuyer.DataResult == null)
            {
                bi.Buyer = txtBuyer.Tag as string;
            }
            else
            {
                bi.Buyer = txtBuyer.DataResult["工号"].ToString();
            }

            bi.Cess = numCess.Value;
            bi.Date = dateTimePicker1.Value;
            bi.InputPerson = BasicInfo.LoginName;
            bi.LaisonMode = txtLaisonMode.Text;
            bi.Provider = txtProvider.Text;
            bi.ProviderLinkman = txtProviderLinkman.Text;
            bi.Remark = txtBillRemark.Text;
            bi.AuditDate = bi.BargainNumber.Contains("AutoH") == true ? (DateTime?)ServerTime.Time : null;

            if (!m_bargainInfoServer.UpdateBargainInfo(m_oldBargainNumber, bi, out m_findBargainInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshBargainDataGridView(m_findBargainInfo);
            PositioningRecord(bi.BargainNumber);
            RefreshBargainControl();
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

            if (dataGridView1.CurrentRow.Cells["关联零星采购单"].Value.ToString().Trim().Length != 0)
            {
                B_MinorPurchaseList tempLnq = 
                    m_minorBillServer.GetListSingle(dataGridView1.CurrentRow.Cells["关联零星采购单"].Value.ToString(), 
                    txtCode.Text,txtName.Text,txtSpec.Text);

                if (Convert.ToDecimal( tempLnq.Count) < numAmount.Value)
                {
                    MessageDialog.ShowPromptMessage("采购数量不能大于申请数量");
                    return;
                }
            }

            if (dataGridViewGoods.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择需要修改的数据行!");
                return;
            }

            if (dataGridViewGoods.CurrentRow.Cells["图号型号"].Value.ToString() != txtCode.Text
                || dataGridViewGoods.CurrentRow.Cells["物品名称"].Value.ToString() != txtName.Text
                || dataGridViewGoods.CurrentRow.Cells["规格"].Value.ToString() != txtSpec.Text)
            {
                if (!m_bargainGoodsServer.IsOperator(Convert.ToInt32(dataGridViewGoods.CurrentRow.Cells["物品ID"].Value), 
                    txtBargainNumber.Text))
                {
                    MessageDialog.ShowPromptMessage("此物品已有业务，不能被操作");
                    return;
                }
            }

            B_BargainGoods bg = new B_BargainGoods();

            bg.BargainNumber = dataGridView1.CurrentRow.Cells["合同号"].Value.ToString();
            bg.Amount = numAmount.Value;
            bg.GoodsID = Convert.ToInt32(txtCode.Tag);
            bg.UnitPrice = numUnitPrice.Value;
            bg.Remark = txtRemark.Text;

            if (!m_bargainGoodsServer.UpdateBargainGoods(Convert.ToInt32(dataGridViewGoods.CurrentRow.Cells["物品ID"].Value), 
                bg, out m_findBargainGoods, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshGoodsDataGridView(m_findBargainGoods);
            PositioningRecord(bg.BargainNumber, bg.GoodsID.ToString());
            RefreshGoodsControl();
        }

        private void dataGridView1_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            dataGridView1.ClearSelection();
            dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[strColName];
        }

        /// <summary>
        /// 控制按钮可操作性
        /// </summary>
        private void EnabledButton()
        {
            if (!m_bargainInfoServer.IsAudited(dataGridView1.CurrentRow.Cells["合同号"].Value.ToString()))
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

        /// <summary>
        /// 点击dataGridView1单元格事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
            {
                return;
            }

            RefreshBargainControl();

            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            btnRefreshDataGridViewGoods_Click(sender, e);

            if (dataGridView1.SelectedRows[0].Cells["合同号"].Value.ToString().Contains("AutoH"))
            {
                ckbIsVirtualBargain.Checked = true;
                txtBargainNumber.ReadOnly = true;
            }
            else
            {
                ckbIsVirtualBargain.Checked = false;
                txtBargainNumber.ReadOnly = false;
            }

            RefreshGoodsControl();
        }

        /// <summary>
        /// 刷新合同控件
        /// </summary>
        void RefreshBargainControl()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.SelectedRows[0];

            chkIsOverseas.Checked = (bool)row.Cells["是否海外合同"].Value;
            chkIsConsignOut.Checked = (bool)row.Cells["是否委外合同"].Value;
            ckbIsVirtualBargain.Checked = row.Cells["合同号"].Value.ToString().Contains("AutoH");
            txtBargainNumber.Text = row.Cells["合同号"].Value.ToString();
            txtProvider.Text = row.Cells["供货单位"].Value.ToString();
            txtProviderLinkman.Text = row.Cells["供应商联系人"].Value.ToString();
            txtLaisonMode.Text = row.Cells["联系方式"].Value.ToString();
            txtBuyer.Text = row.Cells["采购员"].Value.ToString();
            txtMinorPurchaseBillNo.Text = row.Cells["关联零星采购单"].Value.ToString();

            if (txtMinorPurchaseBillNo.Text.Trim().Length != 0)
            {
                btnAddGoods.Visible = false;
                btnNewGoods.Visible = false;
                btnFindCode.Visible = false;
            }
            else
            {
                btnAddGoods.Visible = true;
                btnNewGoods.Visible = true;
                btnFindCode.Visible = true;
            }
 
            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(txtBargainNumber.Text))
            {
                txtBuyer.Tag = m_bargainInfoServer.GetBuyerCode(txtBargainNumber.Text);
            }

            dateTimePicker1.Value = (DateTime)row.Cells["日期"].Value;

            numCess.Value = Convert.ToDecimal(row.Cells["税率"].Value);

            m_oldBargainNumber = txtBargainNumber.Text;
        }

        /// <summary>
        /// 刷新物品控件
        /// </summary>
        void RefreshGoodsControl()
        {
            ClearGoodsControl();

            if (dataGridViewGoods.SelectedRows.Count == 0)
            {
                return;
            }

            DataGridViewRow row = dataGridViewGoods.SelectedRows[0];

            txtBargainNumber.Text = row.Cells["合同号"].Value.ToString().Trim();
            txtCode.Tag = (int)row.Cells["物品ID"].Value;
            txtCode.Text = row.Cells["图号型号"].Value.ToString().Trim();
            txtName.Text = row.Cells["物品名称"].Value.ToString().Trim();
            txtRemark.Text = row.Cells["备注"].Value == null ? "" : row.Cells["备注"].Value.ToString().Trim();
            numAmount.Value = Convert.ToDecimal(row.Cells["数量"].Value);
            numUnitPrice.Value = Convert.ToDecimal(row.Cells["单价"].Value);

            if (row.Cells["规格"].Value != null)
            {
                txtSpec.Text = row.Cells["规格"].Value.ToString();
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridViewGoods_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshGoodsControl();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            FormNormalChoseDateRange form = new FormNormalChoseDateRange();
            form.ShowDialog();

            报表_合同粗概 report = new 报表_合同粗概(form.BeginTime, form.EndTime);
            report.ShowDialog();
        }

        private void btnSearches_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "合同信息综合查询";
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

        private void btnSetFilter_Click(object sender, EventArgs e)
        {
            FormFilterCondition form = new FormFilterCondition(labelTitle.Text, m_findField, labelTitle.Text);
            form.ShowDialog();

            m_bargainInfoServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text);
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
        /// 检查指定合同的订单中是否存在指定物品
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="code">图号</param>
        /// <param name="name">名称</param>
        /// <param name="spec">规格</param>
        /// <returns>存在返回true</returns>
        private bool IsExsistGoodsInOrderForm(string bargainNumber, string code, string name, string spec)
        {
            IQueryable<View_B_OrderFormGoods> orderFormGoodsGroup = m_serverOrderFormGoods.GetOrderFormGoods(bargainNumber);

            if (orderFormGoodsGroup == null || orderFormGoodsGroup.Count() == 0)
            {
                return false;
            }

            return orderFormGoodsGroup.ToList().FindIndex(p => p.图号型号 == code && p.物品名称 == name && p.规格 == spec) > -1;
        }

        private void 自动生成订单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择一条合同信息后再进行此操作！");
                return;
            }

            View_B_BargainInfo bargainInfo = m_bargainInfoServer.GetBargainInfo(txtBargainNumber.Text);

            if (BasicInfo.LoginName != bargainInfo.合同录入员)
            {
                MessageDialog.ShowPromptMessage(string.Format("您无法将 {0} 录入的合同生成订单信息！", bargainInfo.合同录入员));
                return;
            }

            string bargainNumber = txtBargainNumber.Text;
            IQueryable<B_BargainGoods> bargainGoods = m_bargainGoodsServer.GetBargainGoodsInfo(bargainNumber);

            if (m_orderFormServer.GetOrderFormCollection(bargainNumber).Count() != 0 && bargainNumber.Contains("Auto"))
            {
                MessageDialog.ShowPromptMessage("此合同下已存在订单，不能再重复生成！");
                return;
            }

            if (bargainGoods == null || bargainGoods.Count() == 0)
            {
                MessageDialog.ShowPromptMessage("此合同还没有物品信息，不能生成订单！");
                return;
            }

            IQueryable<View_B_OrderFormInfo> ds = m_orderFormServer.GetOrderFormCollection(bargainNumber);

            if (ds != null && ds.ToList().Count > 0)
            {
                if (MessageDialog.ShowEnquiryMessage("此合同已生成过 "+ ds.ToList().Count + "条【订单】，是否继续生成 ?" ) == DialogResult.No)
                {
                    return;
                }
            }

            string orderFormNumber = GenerateOrderFormNumber();

            if (AddOrderInfo(bargainNumber, orderFormNumber) && AddOrderGoods(bargainGoods, orderFormNumber))
            {
                MessageBox.Show("订单成功生成！","提示");
            }
        }

        /// <summary>
        /// 生成订单号
        /// </summary>
        /// <returns>返回生成的订单号</returns>
        private string GenerateOrderFormNumber()
        {
            string bargainNumber = dataGridView1.CurrentRow.Cells["合同号"].Value.ToString();
            string prefix = null;

            if (bargainNumber.Length > 4)
            {
                prefix = dataGridView1.CurrentRow.Cells["合同号"].Value.ToString().Substring(0, 4);
            }

            string orderFormNumber = "";

            if (BasicInfo.DeptCode == "XZ")
            {
                orderFormNumber = m_getbillno.GetOrderFormNumber("XZ");
            }
            else
            {
                if (prefix == "Auto" || chkIsLx.Checked)
                {
                    orderFormNumber = m_getbillno.GetOrderFormNumber("LX");
                }
                else
                {
                    orderFormNumber = m_getbillno.GetOrderFormNumber("CP");
                }
            }

            return orderFormNumber;
        }

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="bargainNumber">合同号</param>
        /// <param name="orderNumber">订单号</param>
        /// <returns>操作成功返回True，否则返回False</returns>
        private bool AddOrderInfo(string bargainNumber,string orderNumber)
        {
            B_OrderFormInfo info = new B_OrderFormInfo();

            info.TypeID = 1;
            info.OrderFormNumber = orderNumber;
            info.BargainNumber = dataGridView1.CurrentRow.Cells["合同号"].Value.ToString();
            info.Buyer = m_bargainInfoServer.GetBuyerCode(bargainNumber); 
            info.CreateDate = ServerTime.Time;
            info.InputPerson = BasicInfo.LoginName;
            info.Provider = dataGridView1.CurrentRow.Cells["供货单位"].Value.ToString();
            info.ProviderPhone = dataGridView1.CurrentRow.Cells["联系方式"].Value.ToString();
            info.ProviderLinkman = dataGridView1.CurrentRow.Cells["供应商联系人"].Value.ToString();
            info.ProviderFax = "";
            info.ProviderEmail = "";
            info.Remark = "";

            if (!m_orderFormServer.AddOrderFormInfo(BasicInfo.ListRoles, BasicInfo.LoginID, info, out m_findOrderFormInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 添加订单物品
        /// </summary>
        /// <param name="Ordergoods">订单物品列表</param>
        /// <param name="OrderNumber">订单号</param>
        /// <returns>成功返回True，否则返回False</returns>
        private bool AddOrderGoods(IQueryable<B_BargainGoods> ordergoods, string orderNumber)
        {
            List<B_BargainGoods> listGoods = ordergoods.ToList();

            foreach (B_BargainGoods item in listGoods)
            {
                B_OrderFormGoods goods = new B_OrderFormGoods();

                goods.ID = 1;
                goods.OrderFormNumber = orderNumber;
                goods.Amount = item.Amount;
                goods.GoodsID = (int)item.GoodsID;
                goods.CreateDate = ServerTime.Time;
                goods.ArrivalDate = ServerTime.Time; //Convert.ToDateTime("2999-12-31 00:00:00");
                goods.Remark = "";

                if (!m_serverOrderFormGoods.AddOrderFormGoods(BasicInfo.ListRoles, BasicInfo.LoginID, goods,
                    out m_findOrderFormGoods, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 清空信息
        /// </summary>
        private void ClearForm()
        {
            txtBuyer.Text = "";
            txtCode.Text = "";
            txtLaisonMode.Text = "";
            txtName.Text = "";
            txtProvider.Text = "";
            txtProviderLinkman.Text = "";
            txtSpec.Text = "";
            numAmount.Value = 0;
        }

        private void btnRefreshBargain_Click(object sender, EventArgs e)
        {
            if (!m_bargainInfoServer.GetAllBargainInfo(out m_findBargainInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshBargainDataGridView(m_findBargainInfo);
            RefreshBargainControl();
        }

        private void btnRefreshDataGridViewGoods_Click(object sender, EventArgs e)
        {
            IQueryable<View_B_BargainGoods> bargainGoods;

            if (!m_bargainGoodsServer.GetBargainGoods(txtBargainNumber.Text, out bargainGoods, out m_err))
            {
                if (!m_err.Contains("没有找到"))
                    MessageDialog.ShowErrorMessage(m_err);
            }

            RefreshGoodsDataGridView(bargainGoods);
        }

        /// <summary>
        /// 新建信息
        /// </summary>
        private void NewInfo()
        {
            ClearControl();

            txtBuyer.Tag = BasicInfo.LoginID;
            txtBuyer.Text = BasicInfo.LoginName;
        }

        private void btnNewInfo_Click(object sender, EventArgs e)
        {
            NewInfo();
            dateTimePicker1.Value = ServerTime.Time;
            btnAdd.Enabled = true;
        }

        private void btnNewGoods_Click(object sender, EventArgs e)
        {
            ClearGoodsControl();

            DataTable dt = m_serverBasicGoods.GetDistinctGoodsName();

            txtName.DataSource = dt;
            txtName.DisplayMember = "GoodsName";
            txtName.Text = "";

            dt = m_serverBasicGoods.GetDistinctGoodsCode();
            txtCode.DataSource = dt;
            txtCode.DisplayMember = "GoodsCode";
            txtCode.Text = "";
        }
        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSh_Click(object sender, EventArgs e)
        {
            if (m_bargainInfoServer.AuditingBargainInfo(BasicInfo.LoginName ,txtBargainNumber.Text,out m_err))
            {
                List<string> lstRoles = new List<string>();

                lstRoles.Add(CE_RoleEnum.会计.ToString());
                m_billMessageServer.EndFlowMessage(txtBargainNumber.Text,
                    string.Format("{0}号合同已完成", txtBargainNumber.Text), lstRoles, null);
                MessageBox.Show("审核成功", "提示");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
            }
        }

        #region 2013-06-17 夏石友屏蔽, 合同中只有选择物品的功能不再需要此事件了
        //private void txtCode_TextChanged(object sender, EventArgs e)
        //{
        //    //if (txtCode.Text.Trim() != "" && txtCode.Text.Length < 2)
        //    //{
        //    //    MessageBox.Show("如果为【产品类】零件，请选择图号型号，若无此图号型号，请联系产品开发部门添加此图号型号", "提示");
        //    //    return;
        //    //}
        //}

        //private void txtName_Leave(object sender, EventArgs e)
        //{
        //    if (txtName.Text.Trim().Length > 0)
        //    {
        //        txtSpec.DataSource = null;

        //        DataTable Dt = m_serverBasicGoods.GetGoodsTable(txtCode.Text.Trim(),
        //            txtName.Text.Trim(), "", "Spec");

        //        txtSpec.DataSource = Dt;
        //        txtSpec.DisplayMember = "Spec";

        //        if (Dt.Rows.Count > 0)
        //        {
        //            txtSpec.Text = Dt.Rows[0][0].ToString();
        //        }

        //        string strCode = txtCode.Text;
        //        txtCode.DataSource = null;

        //        Dt = m_serverBasicGoods.GetGoodsTable("",
        //            txtName.Text.Trim(), "", "GoodsCode");

        //        txtCode.DataSource = Dt;
        //        txtCode.DisplayMember = "GoodsCode";

        //        if (strCode.Trim().Length > 0)
        //        {
        //            txtCode.Text = strCode;
        //        }
        //        else
        //        {
        //            if (Dt.Rows.Count > 0)
        //            {
        //                txtCode.Text = Dt.Rows[0][0].ToString();
        //            }
        //        }
        //    }
        //    else
        //    {
        //        string strCode = txtCode.Text;

        //        DataTable Dt = m_serverBasicGoods.GetDistinctGoodsCode();

        //        txtCode.DataSource = Dt;
        //        txtCode.DisplayMember = "GoodsCode";
        //        txtCode.Text = strCode;
        //    }
        //}

        //private void txtCode_Leave(object sender, EventArgs e)
        //{
        //    if (txtCode.Text.Trim().Length > 0)
        //    {
        //        txtSpec.DataSource = null;

        //        DataTable Dt = m_serverBasicGoods.GetGoodsTable(txtCode.Text.Trim(),
        //            txtName.Text.Trim(), "", "Spec");

        //        txtSpec.DataSource = Dt;
        //        txtSpec.DisplayMember = "Spec";

        //        if (Dt.Rows.Count > 0)
        //        {
        //            txtSpec.Text = Dt.Rows[0][0].ToString();
        //        }


        //        string strName = txtName.Text;
        //        txtName.DataSource = null;

        //        Dt = m_serverBasicGoods.GetGoodsTable(txtCode.Text.Trim(),
        //            "", "", "GoodsName");

        //        txtName.DataSource = Dt;
        //        txtName.DisplayMember = "GoodsName";

        //        if (Dt.Rows.Count > 0)
        //        {
        //            if (strName.Trim().Length > 0)
        //            {
        //                txtName.Text = strName;
        //            }
        //            else
        //            {
        //                txtName.Text = Dt.Rows[0][0].ToString();
        //            }

        //        }
        //        else
        //        {
        //            txtName.Text = "";
        //        }
        //    }
        //    else
        //    {
        //        string strName = txtName.Text;
        //        DataTable Dt = m_serverBasicGoods.GetDistinctGoodsName();

        //        txtName.DataSource = Dt;
        //        txtName.DisplayMember = "GoodsName";
        //        txtName.Text = strName;
        //    }
        //}

        #endregion

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView dv = sender as DataGridView;

            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dv.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dv.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dv.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void 零星采购toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dt = m_minorBillServer.GetFinishListInfo(txtMinorPurchaseBillNo.Text);
            FormDataTableCheck frm = new FormDataTableCheck(dt);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                string bargainNumber = txtBargainNumber.Text;

                if (dataGridViewGoods.CurrentRow != null)
                {
                    if (!m_bargainGoodsServer.IsOperator(Convert.ToInt32(dataGridViewGoods.CurrentRow.Cells["物品ID"].Value),
                        dataGridView1.CurrentRow.Cells["合同号"].Value.ToString()))
                    {
                        MessageDialog.ShowPromptMessage("此物品已有业务，不能被操作");
                        return;
                    }
                }

                m_bargainGoodsServer.OperatorMinorPurchase(bargainNumber, txtMinorPurchaseBillNo.Text);

                foreach (DataRow dr in frm._DtResult.Rows)
                {
                    B_BargainGoods bg = new B_BargainGoods();

                    bg.BargainNumber = txtBargainNumber.Text;
                    bg.Amount = Convert.ToInt32(dr["申请数量"]);
                    bg.GoodsID = Convert.ToInt32(dr["物品ID"]);
                    bg.Remark = txtRemark.Text;
                    bg.UnitPrice = Convert.ToDecimal(dr["预算价格"].ToString());

                    if (!m_bargainGoodsServer.AddBargainGoods(bg, out m_findBargainGoods, out m_err))
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }

                    m_bargainInfoServer.GetAllBargainInfo(out m_findBargainInfo, out m_err);
                    RefreshBargainDataGridView(m_findBargainInfo);
                    PositioningRecord(bargainNumber);
                    RefreshGoodsDataGridView(m_findBargainGoods);
                    ClearGoodsControl();
                }
            }
        }

        DataTable frm_OnFormDataTableCheckFind(DateTime startTime, DateTime endTime)
        {
            return m_minorBillServer.GetBargainRelate(startTime, endTime);
        }

        private void txtMinorPurchaseBillNo_TextChanged(object sender, EventArgs e)
        {
            if (txtMinorPurchaseBillNo.Text == null || txtMinorPurchaseBillNo.Text.Trim().Length == 0)
            {
                btnAddGoods.Visible = true;
                btnNewGoods.Visible = true;
                btnFindCode.Visible = true;
                chkIsLx.Checked = false;
            }
            else
            {
                btnAddGoods.Visible = false;
                btnNewGoods.Visible = false;
                btnFindCode.Visible = false;
                chkIsLx.Checked = true;
            }
        }

        private void chkIsLx_Click(object sender, EventArgs e)
        {
            if (chkIsLx.Checked)
            {
                if (txtBargainNumber.Text.Trim() != "")
                {
                    FormDataTableCheck frm = new FormDataTableCheck(m_minorBillServer.GetBargainRelate(ServerTime.Time.AddMonths(-1).Date, ServerTime.Time.AddDays(1).Date));
                    frm.OnFormDataTableCheckFind += new DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);

                    frm._BlDateTimeControlShow = true;
                    frm._BlIsCheckBox = false;

                    if (frm.ShowDialog() == DialogResult.OK)
                    {
                        txtMinorPurchaseBillNo.Text = frm._DtResult.Rows[0]["单据号"].ToString();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请填写合同号！");
                }
            }
            else
            {
                txtMinorPurchaseBillNo.Text = "";
            }
        }

        private void ckbIsVirtualBargain_Click(object sender, EventArgs e)
        {
            if (ckbIsVirtualBargain.Checked)
            {
                string bargainNumber = "";
                NewInfo();
                ckbIsVirtualBargain.Checked = true;

                if (BasicInfo.DeptCode.Substring(0, 2).ToString() == "XZ")
                {
                    bargainNumber = m_getbillno.GetBargainNumber("XZ");
                }
                else if (BasicInfo.DeptCode.Substring(0, 2).ToString() == "ZZ")
                {
                    bargainNumber = m_getbillno.GetBargainNumber("ZZ");
                }
                else //if (BasicInfo.DeptCode.Substring(0,2).ToString() == "CG")
                {
                    bargainNumber = m_getbillno.GetBargainNumber("");
                }
                txtBargainNumber.Text = bargainNumber;
                txtBargainNumber.ReadOnly = true;
            }
            else
            {
                txtBargainNumber.Text = "";
                txtBargainNumber.ReadOnly = false;
            }
        }

        private void btnDisable_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            m_bargainInfoServer.DisableInfo(dataGridView1.CurrentRow.Cells["合同号"].Value.ToString());
            MessageDialog.ShowPromptMessage("【合同号】：" + dataGridView1.CurrentRow.Cells["合同号"].Value.ToString() + " 禁用成功");
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (Convert.ToBoolean(dgvr.Cells["禁用"].Value))
                {
                    dgvr.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void dataGridViewGoods_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridViewGoods.Rows.Count == 0)
            {
                return;
            }

            foreach (DataGridViewRow dgvr in dataGridViewGoods.Rows)
            {
                if (UniversalFunction.GetGoodsInfo(Convert.ToInt32(dgvr.Cells["物品ID"].Value)).禁用)
                {
                    dgvr.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }
    }
}
