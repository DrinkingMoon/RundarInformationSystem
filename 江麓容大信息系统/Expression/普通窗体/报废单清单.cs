using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ServerModule;
using GlobalObject;
using System.Collections;
using PlatformManagement;
using UniversalControlLibrary;
using CommonBusinessModule;

namespace Expression
{
    public partial class 报废单清单 : Form
    {
        /// <summary>
        /// 车间耗用服务组件
        /// </summary>
        Service_Manufacture_WorkShop.IMaterialsTransfer m_serverMaterials =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IMaterialsTransfer>();

        /// <summary>
        /// 箱体编码服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_serverElectronFile = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 数据内容是否更新的标志
        /// </summary>
        bool m_updateFlag = false;

        /// <summary>
        /// 获取数据内容是否更新的标志
        /// </summary>
        public bool UpdateFlag
        {
            get { return m_updateFlag; }
        }

        /// <summary>
        /// 数据内容是否已经保存的标志
        /// </summary>
        bool m_saveFlag = false;

        /// <summary>
        /// 单据编号
        /// </summary>
        string m_billNo;
        
        /// <summary>
        /// 获取单据编号
        /// </summary>
        public string BillNo
        {
            get { return m_billNo; }
        }

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_planCoseServer = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 营销产品服务组件
        /// </summary>
        ISellIn m_serverSellIn = ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 报废单服务组件m_scrapBillServer
        /// </summary>
        IScrapBillServer m_billServer = ServerModuleFactory.GetServerModule<IScrapBillServer>();

        /// <summary>
        /// 报废物品服务组件m_scrapGoodsServer
        /// </summary>
        IScrapGoodsServer m_goodsServer = ServerModuleFactory.GetServerModule<IScrapGoodsServer>();

        /// <summary>
        /// 人员信息服务
        /// </summary>
        IPersonnelInfoServer m_personnelServer = ServerModuleFactory.GetServerModule<IPersonnelInfoServer>();

        /// <summary>
        /// 库存服务
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 报废单信息
        /// </summary>
        View_S_ScrapBill m_billInfo;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 报废单单据界面
        /// </summary>
        报废单 m_billForm;

        /// <summary>
        /// 权限控制标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 合格供应商服务组件
        /// </summary>
        IAccessoryDutyInfoManageServer m_serverAccessoryDuty = ServerModuleFactory.GetServerModule<IAccessoryDutyInfoManageServer>();

        /// <summary>
        /// 车间管理基础服务
        /// </summary>
        Service_Manufacture_WorkShop.IWorkShopBasic m_serverWSBasic =
            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

        /// <summary>
        /// 车间管理信息
        /// </summary>
        WS_WorkShopCode m_lnqWSCode = new WS_WorkShopCode();

        public 报废单清单(AuthorityFlag authFlag, 报废单 billForm, string billNo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = CE_BillTypeEnum.报废单.ToString();

            m_billForm = billForm;

            m_billNoControl = new BillNumberControl("报废单", m_billServer);

            // 单击进入编辑状态
            this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.AutoGenerateColumns = false;

            dataGridView1.Columns["损失金额"].ValueType = typeof(decimal);
            dataGridView1.Columns["报废数量"].ValueType = typeof(decimal);
            dataGridView1.Columns["工时"].ValueType = typeof(decimal);

            InitWastrelTypeComboBox();
            InitDepartmentComboBox();

            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(billNo))
            {
                m_billInfo = m_billServer.GetBillView(billNo);
                txtPurpose.Text = m_billInfo.单据报废类型;
            }
            else
            {
                txtPurpose.Text = "";
            }

            m_lnqWSCode = m_billInfo == null ?
                m_serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID) :
                m_serverWSBasic.GetPersonnelWorkShop(m_billInfo.申请人编码);

            if (!Convert.ToBoolean(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.开启车间管理模块]))
            {
                dataGridView1.Columns["选批号"].Visible = false;
                dataGridView1.Columns["批次号"].ReadOnly = false;
            }
            else
            {
                dataGridView1.Columns["选批号"].Visible = m_lnqWSCode == null ? false : true;
                dataGridView1.Columns["批次号"].ReadOnly = m_lnqWSCode == null ? false : true;
            }

            m_authFlag = authFlag;
        }

        private void 报废单清单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(AuthorityFlag authorityFlag)
        {
            toolStrip.Visible = true;
            FaceAuthoritySetting.SetVisibly(toolStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);

            if (lblBillStatus.Text == ScrapBillStatus.等待质检批准.ToString() 
                && (authorityFlag & AuthorityFlag.Authorize) != AuthorityFlag.Nothing)
            {
                txtChecker.ReadOnly = false;
            }

            if (lblBillStatus.Text == ScrapBillStatus.新建单据.ToString() 
                && (authorityFlag & AuthorityFlag.Add) != AuthorityFlag.Nothing)
            {
                txtNotifyChecker.ReadOnly = false;
                txtRemark.ReadOnly = false;
            }

            if (m_billInfo != null)
            {
                if (m_billInfo.申请人编码 != BasicInfo.LoginID)
                {
                    btnNew.Enabled = false;
                    btnDelete.Enabled = false;
                    btnDeleteBill.Enabled = false;
                    提交.Enabled = false;
                    btnSave.Enabled = false;
                }

                if (m_billInfo.单据状态 == ScrapBillStatus.已完成.ToString())
                {
                    EnableButton(false);
                    btnNewBill.Enabled = true;
                }
            }

            toolStrip.Visible = true;
        }

        /// <summary>
        /// 初始化报废类别下拉框
        /// </summary>
        private void InitWastrelTypeComboBox()
        {
            string sql = @"select ID, DeclareWastrelType from S_DeclareWastrelType where ID <> 8";
            DataTable dt = DatabaseServer.QueryInfo(sql);

            if (dt != null)
            {
                DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns["报废类别"] as DataGridViewComboBoxColumn;
                dgvComboBoxColumn.DataPropertyName = "ID";
                dgvComboBoxColumn.DataSource = dt.DefaultView;//必须在设置dataGridView1的DataSource的属性前设置
                dgvComboBoxColumn.DisplayMember = "DeclareWastrelType";
                dgvComboBoxColumn.ValueMember = "ID";
            }
        }

        /// <summary>
        /// 初始化责任部门下拉框
        /// </summary>
        private void InitDepartmentComboBox()
        {
            string sql = @"select DepartmentCode, DepartmentName from Department_ZR";
            DataTable dt = DatabaseServer.QueryInfo(sql);

            if (dt != null)
            {
                DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns["责任部门"] as DataGridViewComboBoxColumn;
                dgvComboBoxColumn.DataPropertyName = "DepartmentCode";
                dgvComboBoxColumn.DataSource = dt.DefaultView;//必须在设置dataGridView1的DataSource的属性前设置
                dgvComboBoxColumn.DisplayMember = "DepartmentName";
                dgvComboBoxColumn.ValueMember = "DepartmentCode";
            }
        }

        /// <summary>
        /// 初始化供应商下拉框
        /// </summary>
        /// <param name="goodsID">物品ID</param>
        private void InitProviderComboBox(int goodsID)
        {
            string sql = @"select Provider from S_Stock";

            if (goodsID != 0)
            {
                sql += string.Format(" where GoodsID = {0}", goodsID);
            }

            DataTable dt = DatabaseServer.QueryInfo(sql);

            if (dt != null)
            {
                DataGridViewComboBoxColumn dgvComboBoxColumn = dataGridView1.Columns["TEST"] as DataGridViewComboBoxColumn;
                dgvComboBoxColumn.DataPropertyName = "Provider";
                dgvComboBoxColumn.DataSource = dt.DefaultView;//必须在设置dataGridView1的DataSource的属性前设置
                dgvComboBoxColumn.DisplayMember = "Provider";
                dgvComboBoxColumn.ValueMember = "Provider";
            }
        }

        /// <summary>
        /// 初始化窗体
        /// </summary>
        private void InitForm()
        {
            if (m_billInfo == null)
            {
                lblBillStatus.Text = ScrapBillStatus.新建单据.ToString();
                return;
            }

            lblBillStatus.Text = m_billInfo.单据状态;
            lblBillNo.Text = m_billInfo.报废单号;
            lblBillTime.Text = m_billInfo.报废时间.ToString();
            txtDeclarePersonnel.Text = m_billInfo.申请人签名;
            txtDeclareDepartment.Text = m_billInfo.申请部门;
            txtDeclareDepartment.Tag = m_billInfo.申请部门编码;
            txtPurpose.Text = m_billInfo.单据报废类型;


            if (m_billInfo.部门主管签名 != null)
                txtDepartmentDirector.Text = m_billInfo.部门主管签名;

            if (m_billInfo.知会检验名姓名 != null)
            {
                txtNotifyChecker.Tag = m_billInfo.知会检验名编码;
                txtNotifyChecker.Text = m_billInfo.知会检验名姓名;
            }

            if (m_billInfo.检验员 != null)
            {
                txtChecker.Text = m_billInfo.检验员;
            }

            if (m_billInfo.部门主管签名 != null)
            {
                txtDepartmentDirector.Text = m_billInfo.部门主管签名;
            }

            if (m_billInfo.审批人签名 != null)
            {
                txtSanction.Text = m_billInfo.审批人签名;
            }

            if (m_billInfo.仓管签名 != null)
            {
                txtDepotManager.Text = m_billInfo.仓管签名;
            }

            if (m_billInfo.批准时间 != null)
            {
                dateTimePicker批准.Value = (DateTime)m_billInfo.批准时间;
            }

            if (m_billInfo.仓库确认时间 != null)
            {
                lblDepotTime.Text = m_billInfo.仓库确认时间.ToString();
            }

            if (m_billInfo.备注 != null)
            {
                txtRemark.Text = m_billInfo.备注;
            }

            IEnumerable<View_S_ScrapGoods> goodsGroup = m_goodsServer.GetGoods(m_billInfo.报废单号);

            if (goodsGroup != null && goodsGroup.Count() > 0)
            {
                dataGridView1.Rows.Clear();
                foreach (var goods in goodsGroup)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    dataGridView1.Rows.Add(new object[] { "", goods.物品ID, goods.图号型号, goods.物品名称, goods.规格, goods.版次号,
                        goods.报废数量,goods.报废类别, goods.报废原因, goods.责任部门,goods.责任类型, goods.索赔类别, goods.CVT编号, goods.供应商,
                        goods.责任供应商, "",goods.批次号, goods.报废方式, goods.备注, goods.损失金额, goods.工时, goods.序号});
                }

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                         this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                         this.dataGridView1_ColumnWidthChanged);
                this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(
                                                       this.dataGridView1_CellValueChanged);
            }
        }

        /// <summary>
        /// 绘制数据显示控件行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 查找物品批次号
        /// </summary>
        /// <param name="row">选择行</param>
        private void btnFindBatchNo(DataGridViewRow row)
        {
            FormQueryInfo form = QueryInfoDialog.GetWorkShopBatchNoInfo((int)row.Cells["物品ID"].Value, m_lnqWSCode.WSCode);

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["物品ID"].Value != null &&
                        dataGridView1.Rows[i].Cells["物品ID"].Value == row.Cells["物品ID"].Value &&
                        dataGridView1.Rows[i].Cells["批次号"].Value != null &&
                        dataGridView1.Rows[i].Cells["批次号"].Value == form.GetDataItem("批次号"))
                    {
                        MessageDialog.ShowPromptMessage("此物品有重复记录");
                        return;
                    }
                }

                if (form.GetDataItem("批次号").ToString().Trim().Length == 0)
                {
                    row.Cells["批次号"].Value = "无批次";
                }
                else
                {
                    row.Cells["批次号"].Value = (string)form.GetDataItem("批次号");
                }
            }
        }

        /// <summary>
        /// 查找物品
        /// </summary>
        /// <param name="row">选择行</param>
        private void btnFindCode(DataGridViewRow row)
        {
            FormQueryInfo form = QueryInfoDialog.GetPlanCostGoodsDialog(); 

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["物品ID"].Value != null &&
                        Convert.ToInt32(dataGridView1.Rows[i].Cells["物品ID"].Value) == (int)form.GetDataItem("序号"))
                    {
                        if (MessageBox.Show("此物品有重复记录是否继续？", "提示", MessageBoxButtons.YesNo) == DialogResult.No)
                        {
                            return;
                        }
                    }
                }

                row.Cells["图号型号"].Value = (string)form.GetDataItem("图号型号");
                row.Cells["物品ID"].Value = (int)form.GetDataItem("序号");
                row.Cells["物品名称"].Value = (string)form.GetDataItem("物品名称");
                row.Cells["规格"].Value = (string)form.GetDataItem("规格");
                row.Cells["供应商"].Value = "";
                row.Cells["责任供应商"].Value = "";
            }
        }

        /// <summary>
        /// 查找供应商
        /// </summary>
        /// <param name="row">选择行</param>
        /// <param name="colName">列名</param>
        private void btnFindProvider(DataGridViewRow row, string colName)
        {
            if (row.Cells["物品ID"].Value == null)
                return;

            FormQueryInfo form = QueryInfoDialog.GetDistinctProviderInfoDialog((int)row.Cells["物品ID"].Value, 
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_ScrapProviderType>(colName));

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                if (colName == "供应商")
                {
                    string strProvider = (string)form.GetDataItem("供应商编码");

                    B_AccessoryDutyInfo lnqAcc = new B_AccessoryDutyInfo();

                    lnqAcc.GoodsID = (int)row.Cells["物品ID"].Value;
                    lnqAcc.ProviderA = strProvider;

                    if (!m_serverAccessoryDuty.IsSafeProvider(lnqAcc, out m_error))
                    {
                        MessageDialog.ShowPromptMessage("此物品与供应商未通过合格供应商审核");
                    }

                    row.Cells["供应商"].Value = strProvider;

                }
                else
                {
                    row.Cells["责任供应商"].Value = (string)form.GetDataItem("供应商编码");
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dataGridView1.Columns;

            switch (columns[e.ColumnIndex].Name)
            {
                case "选择物品":
                    btnFindCode(dataGridView1.Rows[e.RowIndex]);
                    break;
                case "选批号":
                    btnFindBatchNo(dataGridView1.Rows[e.RowIndex]);
                    break;
            }
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            DataGridViewColumnCollection columns = this.dataGridView1.Columns;

            switch (columns[this.dataGridView1.CurrentCell.ColumnIndex].Name)
            {
                case "报废数量":
                    break;
                case "损失金额":
                    break;
                case "工时":

                    if (e.Control is TextBox)
                    {
                        TextBox tb = e.Control as TextBox;
                        tb.KeyPress -= new KeyPressEventHandler(tb_KeyPress);
                        tb.KeyPress += new KeyPressEventHandler(tb_KeyPress);
                    }

                    break;
            }
        }

        void tb_KeyPress(object sender, KeyPressEventArgs e)
        {
            DataGridViewColumnCollection columns = this.dataGridView1.Columns;
            string name = columns[this.dataGridView1.CurrentCell.ColumnIndex].Name;

            if (name != "报废数量" && name != "损失金额" && name != "工时")
            {
                return;
            }

            if (!(char.IsDigit(e.KeyChar)))
            {
                Keys key = (Keys)e.KeyChar;

                if (!(key == Keys.Back || key == Keys.Delete))
                {
                    e.Handled = true;
                }
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(decimal))
            {
                e.Cancel = true;
            }
            else if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(int))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 清除窗体上的控件残留信息
        /// </summary>
        void ClearForm()
        {
            lblBillStatus.Text = "";
            lblBillNo.Text = "";
            lblBillTime.Text = "";
            lblDepotTime.Text = "";

            dateTimePicker批准.Enabled = false;
            txtNotifyChecker.Text = "";
            txtNotifyChecker.ReadOnly = false;
            txtRemark.Text = "";
            txtRemark.ReadOnly = false;

            txtChecker.Text = "";
            txtDeclarePersonnel.Text = "";
            txtDeclareDepartment.Text = "";
            txtDepartmentDirector.Text = "";
            txtSanction.Text = "";
            txtDepotManager.Text = "";

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.Clear();
            }
        }

        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        bool CheckSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择至少一条记录后再进行此操作");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查用户对指定行记录是否有操作许可
        /// </summary>
        /// <param name="row">要操作的行记录</param>
        /// <returns>允许返回true</returns>
        bool CheckUserOperation()
        {
            if (m_billInfo == null || m_billInfo.申请人编码 == BasicInfo.LoginID)
            {
                return true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return false;
            }
        }

        /// <summary>
        /// 检测申请人数据项内容是否正确
        /// </summary>
        /// <returns>返回检测结果</returns>
        bool CheckDataItem()
        {
            if (lblBillNo.Text == "")
            {
                MessageDialog.ShowPromptMessage("您还没有新建单据, 没有可用的单据编号！");
                return false;
            }

            if (txtNotifyChecker.Text == "")
            {
                MessageDialog.ShowPromptMessage("要知会的检验员不能为空！");
                return false;
            }

            //张风堂要求修改备注为不强制录入，Modify by cjb at 2012.4.27

            //if (txtRemark.Text.Trim().Length == 0)
            //{
            //    txtRemark.Focus();
            //    MessageDialog.ShowPromptMessage("请在备注栏输入对应纸质单据单号");
            //    return false;
            //}

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                if (cells["物品名称"].Value == null)
                {
                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请选择报废物品", i + 1));
                    return false;
                }

                if (cells["报废数量"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["报废数量"].Value.ToString()) 
                    || Convert.ToDecimal( cells["报废数量"].Value) == 0)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["报废数量"];
                    dataGridView1.BeginEdit(true);
                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 报废数量必须 > 0", i + 1));

                    return false;
                }

                if (cells["报废类别"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["报废类别"];
                    dataGridView1.BeginEdit(true);
                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请选择报废类别", i + 1));

                    return false;
                }

                if (cells["报废原因"].Value == null || cells["报废原因"].Value.ToString().Trim().Length == 0)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["报废原因"];
                    dataGridView1.BeginEdit(true);
                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请输入报废原因", i + 1));

                    return false;
                }

                if (cells["批次号"].Value == null || cells["批次号"].Value.ToString().Trim().Length == 0)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["批次号"];
                    dataGridView1.BeginEdit(true);
                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请输入批次号，如果没有批次请输入“无批次”", i + 1));

                    return false;
                }

                #region 2012.3.6 夏石友，说明：与周妙沟通，责任类型为必选项
                if (cells["责任类型"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["责任类型"];
                    MessageDialog.ShowPromptMessage(
                        string.Format("第 {0} 行, 责任类型为必选项，点击责任类型单元格可以进行“责任类型”选择", i + 1));
                    return false;
                }
                #endregion

                if (cells["责任部门"].Value == null)
                {
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["责任部门"];
                    dataGridView1.BeginEdit(true);
                    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 请输入责任部门", i + 1));

                    return false;
                }
                else
                {
                    if (cells["责任部门"].Value.ToString() == "GYS")
                    {
                        if (cells["供应商"].Value == null || cells["供应商"].Value.ToString().Trim().Length == 0)
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["供应商"];
                            MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 当责任部门选择为“供应商”时必须选择物品供应商，点击供应商单元格可以进行“供应商”选择", i + 1));

                            return false;
                        }

                        if (cells["责任供应商"].Value == null || cells["责任供应商"].Value.ToString().Trim().Length == 0)
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["责任供应商"];
                            MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 当责任部门选择为“供应商”时必须选择责任供应商，点击责任供应商单元格可以进行“供应商”选择", i + 1));

                            return false;
                        }

                        if (cells["责任类型"].Value == null)
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["责任类型"];
                            MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 当责任部门选择为“供应商”时必须选择责任类型，点击责任类型单元格可以进行“责任类型”选择", i + 1));

                            return false;
                        }
                    }
                    else
                    {
                        #region 2012.3.6 夏石友 因为责任部门为质管部的也有可能选择“售后不良，故而屏蔽以下代码
                        //#region 2012.3.5 夏石友 责任部门不是“供应商”时不允许选择“在线不良”、“售后不良”；修改说明：与刘涛沟通

                        //if ((cells["责任类型"].Value.ToString() == "在线不良" || cells["责任类型"].Value.ToString() == "售后不良"))
                        //{
                        //    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["责任类型"];
                        //    MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 当责任部门不是选择为“供应商”时不允许选择责任类型(【在线不良】或者【售后不良】)，点击责任类型单元格可以进行“责任类型”选择", i + 1));
                        //    return false;
                        //}     

                        //#endregion
                        #endregion

                        if (cells["责任部门"].Value.ToString() == "ZZ")
                        {
                            if (cells["责任类型"].Value == null || (cells["责任类型"].Value.ToString() != "生 产 废" 
                                && cells["责任类型"].Value.ToString() != "工 艺 废"))
                            {
                                dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["责任类型"];
                                MessageDialog.ShowPromptMessage(string.Format("第 {0} 行, 当责任部门选择为“制造部”时必须选择责任类型(【生 产 废】或者【工 艺 废】)，点击责任类型单元格可以进行“责任类型”选择", i + 1));

                                return false;
                            }
                        }
                    }

                    if (cells["备注"].Value == null || cells["备注"].Value.ToString().Trim().Length == 0)
                    {
                        if (cells["责任部门"].Value.ToString() == "KH")
                        {
                            dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells["备注"];
                            dataGridView1.BeginEdit(true);
                            MessageDialog.ShowPromptMessage(
                                string.Format("第 {0} 行, 当责任部门选择为“客户”时必须在备注栏注明客户信息", i + 1));

                            return false;
                        }
                    }
                }
            }

            return true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
            {
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要删除选择行（只要不点击保存此操作不生效）？") == DialogResult.No)
            {
                return;
            }

            for (int i = dataGridView1.SelectedRows.Count; i > 0; i--)
            {
                dataGridView1.Rows.Remove(dataGridView1.SelectedRows[i-1]);
            }

            m_updateFlag = true;
            m_saveFlag = false;
        }

        /// <summary>
        /// 使能或禁用按钮
        /// </summary>
        /// <param name="enable">使能或禁用标志</param>
        private void EnableButton(bool enable)
        {
            btnNew.Enabled = enable;
            btnSave.Enabled = enable;
            btnDelete.Enabled = enable;
            btnDeleteBill.Enabled = enable;
            提交.Enabled = enable;
            审核.Enabled = enable;
            批准.Enabled = enable;
            确认报废.Enabled = enable;
            SQE处理.Enabled = enable;
            查找引用toolStripButton3.Enabled = enable;
            btnExcelInput.Enabled = enable;
            btnMaterialsTransfer.Enabled = enable;
            填写变速箱号toolStripButton3.Enabled = enable;
            引用Bom_toolStripButton4.Enabled = enable;
            回退单据.Enabled = enable;
        }

        /// <summary>
        /// 新建单据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewBill_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != "" && lblBillStatus.Text != "新建单据" && dataGridView1.Rows.Count > 0 && m_updateFlag && !m_saveFlag)
            {
                if (MessageDialog.ShowEnquiryMessage("是否保存您刚才所作的更改？") == DialogResult.Yes)
                {
                    if (!SaveData(true))
                    {
                        return;
                    }
                }
                else
                {
                    m_billNoControl.CancelBill();
                }
            }

            m_billInfo = null;
            ClearForm();

            m_saveFlag = false;

            EnableButton(true);
            审核.Enabled = false;
            批准.Enabled = false;
            确认报废.Enabled = false;
            lblBillNo.Text = m_billNoControl.GetNewBillNo();
            lblBillTime.Text = ServerModule.ServerTime.Time.ToString();

            txtDeclarePersonnel.Text = BasicInfo.LoginName;
            txtDeclareDepartment.Text = BasicInfo.DeptName;
            lblBillStatus.Text = ScrapBillStatus.新建单据.ToString();

            AuthorityControl(m_authFlag);
            txtNotifyChecker.Focus();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (lblBillNo.Text == "")
            {
                MessageDialog.ShowPromptMessage("请新建单据后再进行此操作");
                return;
            }

            DataGridViewRow dr = new DataGridViewRow();
            dataGridView1.Rows.Add(dr);

            m_updateFlag = true;
        }

        private void btnDeleteBill_Click(object sender, EventArgs e)
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(lblBillNo.Text) || !m_billServer.IsExist(lblBillNo.Text))
            {
                MessageDialog.ShowPromptMessage("您当前的状态没有单据可以删除！");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要删除本单据，此操作立即生效？") == DialogResult.No)
            {
                return;
            }

            m_billForm.DeleteBill(lblBillNo.Text, lblBillStatus.Text);
            dataGridView1.Rows.Clear();

            EnableButton(false);

            ClearForm();

            m_updateFlag = true;
        }

        private void 报废单清单_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (lblBillStatus.Text != "" && lblBillStatus.Text != "新建单据" &&
                lblBillStatus.Text != "已完成"
                && dataGridView1.Rows.Count > 0 && m_updateFlag && !m_saveFlag)
            {
                if (MessageDialog.ShowEnquiryMessage("您希望保存您做的修改吗？") == DialogResult.Yes)
                {
                    if (!SaveData(true))
                    {
                        e.Cancel = true;
                        return;
                    }
                }
            }

            m_billNoControl.CancelBill();
        }

        private void 提交_Click(object sender, EventArgs e)
        {
            try
            {
                if (lblBillStatus.Text != ScrapBillStatus.新建单据.ToString())
                {
                    MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法提交");
                    return;
                }

                if (!CheckUserOperation())
                    return;

                txtNotifyChecker.Focus();

                if (dataGridView1.Rows.Count == 0)
                {
                    MessageDialog.ShowPromptMessage("您还未添加报废物品明细，无法提交");
                    return;
                }

                if (!CheckDataItem())
                    return;

                if (!m_goodsServer.IsExist(lblBillNo.Text))
                {
                    SaveData(false);
                }

                if (!m_billServer.SubmitNewBill(lblBillNo.Text, out m_error))
                {
                    MessageDialog.ShowErrorMessage(m_error);
                    return;
                }

                m_billMessageServer.DestroyMessage(lblBillNo.Text);

                m_billMessageServer.SendNewFlowMessage(lblBillNo.Text, string.Format("【报废类型】：{0}  【申请人】：{1}  ※※※ 等待【上级领导】处理",
                    txtPurpose.Text, m_billServer.GetBill(lblBillNo.Text).FillInPersonnel),
                    BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

                m_updateFlag = true;
                m_saveFlag = true;

                lblBillStatus.Text = ScrapBillStatus.等待主管审核.ToString();

                txtDepartmentDirector.Text = BasicInfo.LoginName;

                提交.Enabled = false;

                MessageDialog.ShowPromptMessage("成功提交,等待主管审核!");
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void 审核_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != ScrapBillStatus.等待主管审核.ToString())
            {
                MessageDialog.ShowPromptMessage("单据状态不符合审核条件");
                return;
            }

            if (!m_personnelServer.IsUserDirector(m_billInfo.申请人编码, BasicInfo.LoginID))
            {
                MessageDialog.ShowPromptMessage("请选择您下属人员提交的记录后再进行此操作");
                return;
            }

            if (!m_billServer.DirectorAuthorizeBill(lblBillNo.Text, BasicInfo.LoginName, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_updateFlag = true;
            m_saveFlag = true;
            lblBillStatus.Text = ScrapBillStatus.等待质检批准.ToString();

            MessageDialog.ShowPromptMessage("成功审核,等待质检批准!");

            string msg = string.Format("【报废类型】：{0}  【申请人】：{1}  ※※※ 等待【质量工程师】处理",
                txtPurpose.Text, m_billServer.GetBill(lblBillNo.Text).FillInPersonnel);

            m_billMessageServer.PassFlowMessage(lblBillNo.Text, msg, CE_RoleEnum.质量工程师.ToString(), true);

            txtDepartmentDirector.Text = BasicInfo.LoginName;
            txtChecker.ReadOnly = false;
            txtNotifyChecker.ReadOnly = true;
            txtRemark.ReadOnly = true;

            审核.Enabled = false;
        }

        private void 批准_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != ScrapBillStatus.等待质检批准.ToString())
            {
                MessageDialog.ShowPromptMessage("单据状态不符合质检批准条件");
                return;
            }

            if (txtChecker.Text == "")
            {
                txtChecker.Focus();
                MessageDialog.ShowPromptMessage("检验员不能为空");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("默认工时为0，您确定已经录入工时了吗？") == DialogResult.No)
            {
                return;
            }

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                if (dgvr.Cells["责任部门"].Value != null && dgvr.Cells["责任部门"].Value.ToString() == "GYS" &
                    (dgvr.Cells["索赔类别"].Value == null || GeneralFunction.IsNullOrEmpty(dgvr.Cells["索赔类别"].Value.ToString())))
                {
                    MessageDialog.ShowPromptMessage("【责任部门】：供应商，请填写【索赔类别】");
                    return;
                }
            }

            string billNo = lblBillNo.Text;
            bool blflg = false;

            S_ScrapBill billInfo = new S_ScrapBill();

            billInfo.Checker = txtChecker.Text;
            billInfo.Sanction = BasicInfo.LoginName;

            // 存在责任部门为供应商又没有确定报废方式时由SQE处理此单据
            if (GetListGoods().Where(k => k.责任部门 == "GYS").Count() > 0)
            {
                billInfo.BillStatus = ScrapBillStatus.等待SQE处理意见.ToString();
                blflg = true;
            }
            else
            {
                billInfo.BillStatus = ScrapBillStatus.等待仓管确认.ToString();
            }

            if (!m_billServer.SubmitQualityInfo(billNo, billInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            List<View_S_ScrapGoods> lstGoods = new List<View_S_ScrapGoods>(dataGridView1.Rows.Count);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_ScrapGoods goods = new View_S_ScrapGoods();
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                goods.序号 = (long)cells["序号"].Value;
                goods.报废原因 = (string)cells["报废原因"].Value;
                goods.责任类型 = (string)cells["责任类型"].Value;
                goods.责任供应商 = (string)cells["责任供应商"].Value;
                goods.供应商 = (string)cells["供应商"].Value;
                goods.索赔类别 = (string)cells["索赔类别"].Value;

                if (cells["工时"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(cells["工时"].Value.ToString()))
                {
                    goods.工时 = 0;
                }
                else
                {
                    goods.工时 = (decimal)cells["工时"].Value;
                }
                
                lstGoods.Add(goods);
            }

            if (!m_goodsServer.UpdateGoods(lstGoods, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_updateFlag = true;
            m_saveFlag = true;
            lblBillStatus.Text = billInfo.BillStatus;

            dateTimePicker批准.Value = ServerTime.Time;
            txtSanction.Text = billInfo.Sanction;

            if (blflg)
            {
                MessageDialog.ShowPromptMessage("已经批准,等待SQE处理意见!");
                string msg = string.Format("【报废类型】：{0}  【申请人】：{1}  ※※※ 等待【SQE】处理",
                txtPurpose.Text, m_billServer.GetBill(lblBillNo.Text).FillInPersonnel);
                m_billMessageServer.PassFlowMessage(billNo, msg, CE_RoleEnum.SQE组员.ToString(), true);
            }
            else
            {
                MessageDialog.ShowPromptMessage("已经批准,等待仓管确认!");
                string msg = string.Format("【报废类型】：{0}  【申请人】：{1}  ※※※ 等待【仓管员】处理",
                txtPurpose.Text, m_billServer.GetBill(lblBillNo.Text).FillInPersonnel);
                m_billMessageServer.PassFlowMessage(billNo, msg,
                        m_billMessageServer.GetRoleStringForStorage("01").ToString(), true);
            }

            批准.Enabled = false;
        }

        private void 确认报废_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != ScrapBillStatus.等待仓管确认.ToString())
            {
                MessageDialog.ShowPromptMessage("单据状态不符合确认报废条件");
                return;
            }

            string billNo = lblBillNo.Text;

            if (!m_billServer.FinishBill(billNo, BasicInfo.LoginName, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_updateFlag = true;
            m_saveFlag = true;
            lblBillStatus.Text = ScrapBillStatus.已完成.ToString();
            确认报废.Enabled = false;

            lblDepotTime.Text = ServerTime.Time.ToString();
            txtDepotManager.Text = BasicInfo.LoginName;

            #region 发送知会消息

            string content = string.Format("{0} 号报废单已经处理完毕", billNo);

            #region 2013.04.17 夏石友
            
            //List<string> noticeRoles = new List<string>();

            //noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(
            //    m_billServer.GetBill(billNo).DeclareDepartment.Substring(0, 2)));
            //noticeRoles.Add(RoleEnum.质量总监.ToString());
            //noticeRoles.Add(RoleEnum.质控主管.ToString());
            //noticeRoles.Add(RoleEnum.质量工程师.ToString());
            //noticeRoles.Add(RoleEnum.SQE组长.ToString());
            //noticeRoles.Add(RoleEnum.SQE组员.ToString());
            //noticeRoles.Add(RoleEnum.财务主管.ToString());
            //noticeRoles.Add(RoleEnum.会计.ToString());

            //m_billMessageServer.EndFlowMessage(billNo, string.Format("{0} 号报废单已经处理完毕", billNo), noticeRoles, null);

            m_billMessageServer.EndFlowMessage(billNo, 
                string.Format("{0} 号报废单已经处理完毕", billNo), null, 
                new List<string>( new string[] { m_billInfo.申请人编码 } ));

            #endregion

            #endregion 发送知会消息
        }

        private void btnFindNotifyChecker_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtNotifyChecker, "ZK", "姓名");

            if (form.ShowDialog() == DialogResult.OK)
                txtNotifyChecker.Tag = form.GetStringDataItem("工号");
        }

        private void btnFindChecker_Click(object sender, EventArgs e)
        {
            FormPersonnel form = new FormPersonnel(txtChecker, "ZK", "姓名");

            if (form.ShowDialog() == DialogResult.OK)
                txtChecker.Tag = form.GetStringDataItem("工号");
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (lblBillStatus.Text != ScrapBillStatus.新建单据.ToString())
            {
                MessageDialog.ShowPromptMessage("您现在不是处于新建单据状态，无法进行此操作");
                return;
            }

            if (!CheckUserOperation())
                return;

            if (!CheckDataItem())
                return;

            S_ScrapBill bill = new S_ScrapBill();

            bill.Bill_ID = lblBillNo.Text;
            bill.Bill_Time = ServerModule.ServerTime.Time;
            bill.NotifyChecker = txtNotifyChecker.Text;
            bill.Remark = txtRemark.Text;

            if (!m_billServer.UpdateBill(bill, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="showSucessedInfo">是否显示成功保存的消息</param>
        /// <returns>成功返回true</returns>
        private bool SaveData(bool showSucessedInfo)
        {
            if (!m_updateFlag)
            {
                MessageDialog.ShowPromptMessage("您还未进行任何变更, 无需保存！");
                return false;
            }

            if (dataGridView1.Rows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("您还未添加报废物品明细, 无法保存！");
                return false;
            }

            if (txtPurpose.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择单据报废类型");
                return false;
            }

            if (showSucessedInfo && lblBillStatus.Text != ScrapBillStatus.新建单据.ToString() 
                && MessageDialog.ShowEnquiryMessage("保存后单据将设置为新建状态, 是否继续？") == DialogResult.No)
                return false;

            // 使最后更新的单元格数据生效
            dataGridView1.CurrentCell = null;

            if (!CheckDataItem())
                return false;

            lblBillStatus.Text = ScrapBillStatus.新建单据.ToString();

            S_ScrapBill bill = new S_ScrapBill();

            bill.Bill_ID = lblBillNo.Text;
            bill.Bill_Time = Convert.ToDateTime(lblBillTime.Text);
            bill.BillStatus = lblBillStatus.Text;
            bill.DeclareDepartment = BasicInfo.DeptCode;
            bill.FillInPersonnel = BasicInfo.LoginName;
            bill.FillInPersonnelCode = BasicInfo.LoginID;
            bill.ProductType = txtPurpose.Text;

            if (txtNotifyChecker.DataResult == null)
                bill.NotifyChecker = txtNotifyChecker.Tag as string;
            else
                bill.NotifyChecker = txtNotifyChecker.DataResult["工号"].ToString();

            bill.Remark = txtRemark.Text;

            List<View_S_ScrapGoods> lstGoods = new List<View_S_ScrapGoods>(dataGridView1.Rows.Count);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_ScrapGoods goods = new View_S_ScrapGoods();
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                goods.报废单号 = bill.Bill_ID;
                goods.物品ID = (int)cells["物品ID"].Value;

                goods.供应商 = cells["供应商"].Value.ToString();
                goods.责任供应商 = cells["责任供应商"].Value.ToString();

                goods.报废数量 =  Convert.ToDecimal(cells["报废数量"].Value);
                goods.报废类别 = (int)(cells["报废类别"].Value);

                goods.报废原因 = cells["报废原因"].Value.ToString();
                goods.责任部门 = (string)(cells["责任部门"].Value);
                goods.责任类型 = (string)(cells["责任类型"].Value);
                goods.报废方式 = (string)(cells["报废方式"].Value);
                goods.索赔类别 = (string)(cells["索赔类别"].Value);

                if (cells["版次号"].Value != null)
                    goods.版次号 = cells["版次号"].Value.ToString();

                if (cells["批次号"].Value != null)
                    goods.批次号 = cells["批次号"].Value.ToString();

                if (cells["备注"].Value != null)
                {
                    goods.备注 = cells["备注"].Value.ToString();
                }

                if (cells["变速箱号"].Value != null)
                    goods.CVT编号 = cells["变速箱号"].Value.ToString();

                if (cells["损失金额"].Value == null || cells["损失金额"].Value.ToString().Trim()=="")
                {
                    goods.损失金额 = 0;
                    goods.单价 = 0;
                }
                else
                {
                    goods.损失金额 = (decimal)cells["损失金额"].Value;
                    goods.单价 = goods.损失金额 / goods.报废数量;
                }

                if (cells["工时"].Value != null)
                    goods.工时 = Convert.ToDecimal(cells["工时"].Value);

                lstGoods.Add(goods);
            }

            if (lstGoods.Where(k => k.责任部门 == "GYS").Count() > 0)
            {
                if (lstGoods.Where(k => k.责任部门 != "GYS").Count() > 0)
                {
                    MessageDialog.ShowErrorMessage("如存在【责任部门】为供应商的记录，请保证所有记录的【责任部门】均为供应商");
                    return false;
                }

                // 2019.3.26 夏石友，为解决报废期初物品特屏蔽以下代码 <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
                //    using (DepotManagementDataContext ctx = CommentParameter.DepotDataContext)
                //    {
                //        foreach (View_S_ScrapGoods item in lstGoods)
                //        {
                //            if (item.供应商 == item.责任供应商 
                //                && m_billServer.GetOrderForm(ctx, item.物品ID, item.批次号, item.供应商) == null)
                //            {
                //                MessageDialog.ShowErrorMessage("【物品ID】:" + item.物品ID.ToString() + " 【批次号】:" + item.批次号 + " 【供应商】:" + item.供应商 + " 无对应的入库记录，无法报废");
                //                return false;
                //            }
                //        }
                //    }
                // 2019.3.26 夏石友，为解决报废期初物品特屏蔽以下代码 >>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>
            }            

            if (!m_goodsServer.IsInfoAccordance(lblBillNo.Text, lstGoods, out m_error))
            {
                MessageDialog.ShowPromptMessage("此单据已被领料单关联，不能修改之前已保存的物品名称与数量!");
                return false;
            }

            if (!m_goodsServer.SaveGoods(lblBillNo.Text, bill, lstGoods,out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return false;
            }

            // 销毁原消息
            m_billMessageServer.DestroyMessage(lblBillNo.Text);

            m_billNo = lblBillNo.Text;
            m_billInfo = m_billServer.GetBillView(m_billNo);
            m_saveFlag = true;
            提交.Enabled = true;
            审核.Enabled = false;
            批准.Enabled = false;
            确认报废.Enabled = false;

            if (showSucessedInfo)
            {
                MessageDialog.ShowPromptMessage("成功保存信息！");
            }

            return true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveData(true);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            m_updateFlag = true;
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            if (dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].IsInEditMode)
            {
                return;
            }

            FormViewData form = new FormViewData(dataGridView1.Columns, dataGridView1.Rows[e.RowIndex]);
            form.ShowDialog();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            变速箱号.ReadOnly = true;

            if (e.RowIndex < 0 || e.ColumnIndex < 0 || !dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].ReadOnly)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dataGridView1.Columns;

            switch (columns[e.ColumnIndex].Name)
            {
                case "供应商":
                    btnFindProvider(dataGridView1.Rows[e.RowIndex], columns[e.ColumnIndex].Name);
                    break;
                case "责任供应商":
                    btnFindProvider(dataGridView1.Rows[e.RowIndex], columns[e.ColumnIndex].Name);
                    break;
                case "变速箱号":
                    if (UniversalFunction.IsProduct(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value)))
                    {
                        BarCodeInfo tempInfo = new BarCodeInfo();

                        tempInfo.BatchNo = "";
                        tempInfo.Count = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["报废数量"].Value);
                        tempInfo.GoodsCode = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                        tempInfo.GoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);
                        tempInfo.GoodsName = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                        tempInfo.Remark = dataGridView1.CurrentRow.Cells["备注"].Value == null ? "" :
                            dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                        tempInfo.Spec = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();

                        CE_BusinessType tempType = CE_BusinessType.库房业务;

                        Service_Manufacture_WorkShop.IWorkShopBasic serverWSBasic =
                            Service_Manufacture_WorkShop.ServerModuleFactory.GetServerModule<Service_Manufacture_WorkShop.IWorkShopBasic>();

                        WS_WorkShopCode tempWSCode = serverWSBasic.GetPersonnelWorkShop(BasicInfo.LoginID);

                        Dictionary<string, string> tempDic = new Dictionary<string, string>();

                        tempDic.Add("05", CE_MarketingType.库房报废.ToString());

                        if (tempWSCode != null)
                        {
                            tempType = CE_BusinessType.综合业务;
                            tempDic.Add(tempWSCode.WSCode, CE_SubsidiaryOperationType.报废.ToString());
                        }

                        产品编号 form = new 产品编号(tempInfo, tempType, lblBillNo.Text, btnDelete.Enabled, tempDic);

                        form.ShowDialog();

                        dataGridView1.Rows[e.RowIndex].Cells["变速箱号"].Value = "";

                        for (int i = 0; i < form.DtProductCodes.Rows.Count; i++)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells["变速箱号"].Value +=
                                form.DtProductCodes.Rows[i]["ProductCode"].ToString() + "/";
                        }

                        if (dataGridView1.Rows[e.RowIndex].Cells["变速箱号"].Value.ToString().Length > 0)
                        {
                            dataGridView1.Rows[e.RowIndex].Cells["变速箱号"].Value =
                                dataGridView1.Rows[e.RowIndex].Cells["变速箱号"].Value.ToString().Substring(0,
                                dataGridView1.Rows[e.RowIndex].Cells["变速箱号"].Value.ToString().Length - 1);
                        }
                    }
                    else
                    {
                        变速箱号.ReadOnly = false;
                    }

                    break;
            }
        }

        private void 回退单据_Click(object sender, EventArgs e)
        {
            if (审核.Enabled && lblBillStatus.Text == "等待主管审核")
            {
                ReturnBillStatus();
            }
            else if (批准.Enabled && lblBillStatus.Text == "等待质检批准")
            {
                ReturnBillStatus();
            }
            else if (确认报废.Enabled && lblBillStatus.Text == "等待仓管确认")
            {
                ReturnBillStatus();
            }
            else if (SQE处理.Enabled && lblBillStatus.Text == "等待SQE处理意见")
            {
                ReturnBillStatus();
            }
        }

        /// <summary>
        /// 回退单据
        /// </summary>
        private void ReturnBillStatus()
        {
            if (lblBillStatus.Text != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.报废单, lblBillNo.Text, lblBillStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_billServer.ReturnBill(form.StrBillID,
                            form.StrBillStatus, out m_error, form.Reason))
                        {
                            MessageDialog.ShowPromptMessage("回退成功");
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                        }
                    }

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void SQE处理_Click(object sender, EventArgs e)
        {
            dateTimePicker批准.Focus();

            if (lblBillStatus.Text != ScrapBillStatus.等待SQE处理意见.ToString())
            {
                MessageDialog.ShowPromptMessage("单据状态不符合审核条件");
                return;
            }

            foreach (DataGridViewRow dgvr in this.dataGridView1.Rows)
            {
                if (dgvr.Cells["报废方式"].Value == null || GlobalObject.GeneralFunction.IsNullOrEmpty(dgvr.Cells["报废方式"].Value.ToString()))
                {
                    MessageDialog.ShowPromptMessage("请对责任部门为供应商的物品的报废方式进行选择！");
                    return;
                }
            }

            //if (GetListGoods().Where(k => k.责任部门 == "GYS").Count() > 0)
            //{
            //    MessageDialog.ShowPromptMessage("请对责任部门为供应商的物品的报废方式进行选择！");
            //    return;
            //}

            if (!m_billServer.SubmitSQEMessage(lblBillNo.Text, GetListGoods(), out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            m_updateFlag = true;
            m_saveFlag = true;
            lblBillStatus.Text = ScrapBillStatus.等待仓管确认.ToString();

            MessageDialog.ShowPromptMessage("成功处理,等待仓管确认!");

            string msg = string.Format("【报废类型】：{0}  【申请人】：{1}  ※※※ 等待【仓管员】处理",
                txtPurpose.Text, m_billServer.GetBill(lblBillNo.Text).FillInPersonnel);
            m_billMessageServer.PassFlowMessage(lblBillNo.Text, msg,
                        m_billMessageServer.GetRoleStringForStorage("01").ToString(), true);

            txtChecker.ReadOnly = false;
            txtNotifyChecker.ReadOnly = false;
            txtRemark.ReadOnly = false;

            SQE处理.Enabled = false;
        }

        /// <summary>
        /// 获得网格的LNQ 的LIST
        /// </summary>
        /// <returns></returns>
        public List<View_S_ScrapGoods> GetListGoods()
        {
            List<View_S_ScrapGoods> lstGoods = new List<View_S_ScrapGoods>(dataGridView1.Rows.Count);

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                View_S_ScrapGoods goods = new View_S_ScrapGoods();
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                goods.报废单号 = lblBillNo.Text;
                goods.物品ID = (int)cells["物品ID"].Value;

                goods.供应商 = cells["供应商"].Value.ToString();
                goods.责任供应商 = cells["责任供应商"].Value.ToString();

                goods.报废数量 = (decimal)(cells["报废数量"].Value);
                goods.报废类别 = (int)(cells["报废类别"].Value);

                goods.报废原因 = cells["报废原因"].Value.ToString();
                goods.责任部门 = (string)(cells["责任部门"].Value);
                goods.责任类型 = (string)(cells["责任类型"].Value);
                goods.报废方式 = (string)(cells["报废方式"].Value);

                if (cells["批次号"].Value != null)
                    goods.批次号 = cells["批次号"].Value.ToString();

                if (cells["备注"].Value != null)
                {
                    goods.备注 = cells["备注"].Value.ToString();
                }

                if (cells["变速箱号"].Value != null)
                    goods.CVT编号 = cells["变速箱号"].Value.ToString();

                if (cells["损失金额"].Value == null)
                {
                    goods.损失金额 = 0;
                    goods.单价 = 0;
                }
                else
                {
                    goods.损失金额 = (decimal)cells["损失金额"].Value;
                    goods.单价 = goods.损失金额 / goods.报废数量;
                }

                if (cells["工时"].Value != null)
                {
                    goods.工时 = (decimal)cells["工时"].Value;
                }

                goods.报废方式 = (string)cells["报废方式"].Value;

                lstGoods.Add(goods);
            }
            return lstGoods;
        }

        private void 报废单清单_Load(object sender, EventArgs e)
        {
            InitForm();

            AuthorityControl(m_authFlag);

            if (m_billInfo == null)
            {
                EnableButton(false);

                btnNewBill.Enabled = true;
            }
        }

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            领料用途 form = new 领料用途();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPurpose.Tag = form.SelectedData.Code;
                txtPurpose.Text = form.SelectedData.Purpose;
            }
        }

        #region 邱瑶 说明：引用已有报废单的报废物品清单，只有下线人员可操作2012-03-05

        private void 查找引用toolStripButton3_Click(object sender, EventArgs e)
        {
            FormDataTableCheck frm =
                new FormDataTableCheck(m_billServer.GetScrapBill(new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1), ServerTime.Time.AddDays(1)));
            frm._BlDateTimeControlShow = true;
            frm._BlIsCheckBox = false;
            frm.OnFormDataTableCheckFind +=new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind1);

            frm.ShowDialog();

            if (frm._DtResult == null || frm._DtResult.Rows.Count == 0)
            {
                return;
            }

            string billID = frm._DtResult.Rows[0]["报废单号"].ToString();

            if (billID != null)
            {
                m_billNoControl = new BillNumberControl("报废单", m_billServer);

                dataGridView1.Rows.Clear();

                // 单击进入编辑状态
                this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
                this.dataGridView1.AutoGenerateColumns = false;

                dataGridView1.Columns["报废数量"].ValueType = typeof(decimal);
                dataGridView1.Columns["工时"].ValueType = typeof(decimal);

                InitWastrelTypeComboBox();
                InitDepartmentComboBox();
                m_billInfo = m_billServer.GetBillView(billID);

                IEnumerable<View_S_ScrapGoods> goodsGroup = m_goodsServer.GetGoods(m_billInfo.报废单号);

                if (goodsGroup != null && goodsGroup.Count() > 0)
                {
                    foreach (var goods in goodsGroup)
                    {
                        DataGridViewRow row = new DataGridViewRow();
                        dataGridView1.Rows.Add(new object[] { "", goods.物品ID, goods.图号型号, goods.物品名称,
                            goods.规格, goods.版次号, goods.报废数量, goods.报废类别, goods.报废原因, goods.责任部门,
                            goods.责任类型,"","","","","", goods.批次号,"","","",goods.工时 });//所引用的字段
                    }

                    for (int i = 0; i < dataGridView1.Columns.Count; i++)
                    {
                        dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                             this.dataGridView1_ColumnWidthChanged);

                    ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                    this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                             this.dataGridView1_ColumnWidthChanged);

                    this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(
                                                           this.dataGridView1_CellValueChanged);
                }
            }
        }
        #endregion

        #region 邱瑶 说明：批量设置变速箱编号，省去一个个填写2012-03-05
        private void 填写变速箱号toolStripButton3_Click(object sender, EventArgs e)
        {
            填写变速箱编号 frm = new 填写变速箱编号();
            frm.ShowDialog();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;
                cells["变速箱号"].Value = frm.StrCVTID;
            }
        }
        #endregion

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow != null 
                && dataGridView1.CurrentRow.Cells["责任部门"].Value != null 
                && dataGridView1.CurrentRow.Cells["责任部门"].Value.ToString() != "GYS")
            {
                dataGridView1.CurrentRow.Cells["责任供应商"].Value = "";
            }
        }

        #region 邱瑶 说明：引用装配Bom，省去逐步添加物品的麻烦 2012-07-10
        private void 引用Bom_toolStripButton4_Click(object sender, EventArgs e)
        {
            int a = 0;

            try
            {

                //引用装配Bom
                引用装配Bom frmBom = new 引用装配Bom();
                frmBom.ShowDialog();

                if (frmBom.BlIsSelectProductType)
                {
                    this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
                    this.dataGridView1.AutoGenerateColumns = false;

                    dataGridView1.Columns["报废数量"].ValueType = typeof(decimal);
                    dataGridView1.Columns["工时"].ValueType = typeof(decimal);

                    InitWastrelTypeComboBox();
                    InitDepartmentComboBox();

                    if (frmBom.dataGridView1.Rows.Count > 0)
                    {
                        for (int i = 0; i < frmBom.dataGridView1.Rows.Count; i++)
                        {
                            DataGridViewRow row = new DataGridViewRow();
                            a = i;
                            View_F_GoodsPlanCost goodsInfo = m_planCoseServer.GetGoodsInfo(frmBom.dataGridView1.Rows[i].Cells["零件编码"].Value.ToString(),
                                frmBom.dataGridView1.Rows[i].Cells["规格"].Value.ToString(), out m_error);
                            this.dataGridView1.Rows.Add(new object[] { 
                                "",goodsInfo.序号, goodsInfo.图号型号, goodsInfo.物品名称, goodsInfo.规格,
                                "","","","","","","","","","","","","","",0 });
                        }

                        for (int i = 0; i < dataGridView1.Columns.Count; i++)
                        {
                            dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                        }

                        this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                                 this.dataGridView1_ColumnWidthChanged);

                        ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                        this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                                 this.dataGridView1_ColumnWidthChanged);

                        this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(
                                                               this.dataGridView1_CellValueChanged);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        private void btnSetBatchNoAndProvider_Click(object sender, EventArgs e)
        {
            if (m_lnqWSCode == null)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                    DataTable dtElectron = m_serverElectronFile.GetProductElectronFile(cells["变速箱号"].Value.ToString(),
                        cells["图号型号"].Value.ToString(), cells["物品名称"].Value.ToString(),
                        cells["规格"].Value.ToString());

                    if (dtElectron == null)
                    {
                        cells["批次号"].Value = "无批次";
                    }
                    else
                    {
                        cells["批次号"].Value = dtElectron.Rows[0]["BatchNo"].ToString();
                        cells["供应商"].Value = dtElectron.Rows[0]["Provider"].ToString();
                    }
                }
            }
        }

        private void btnMaterialsTransfer_Click(object sender, EventArgs e)
        {
            if (m_lnqWSCode == null)
            {
                return;
            }

            FormDataTableCheck frm =
                new FormDataTableCheck(m_serverMaterials.GetMaterialsTransferInfo(m_lnqWSCode.WSCode));

            frm.OnFormDataTableCheckFind += new GlobalObject.DelegateCollection.FormDataTableCheckFindDelegate(frm_OnFormDataTableCheckFind);
            frm._BlDateTimeControlShow = true;

            if (frm.ShowDialog() == DialogResult.OK)
            {
                List<string> listRequisition = DataSetHelper.ColumnsToList_Distinct(frm._DtResult, "单据号");

                DataTable tempTable =
                    m_serverMaterials.SumMaterialsTransferGoods(listRequisition,
                    (int)CE_SubsidiaryOperationType.物料转换后,(int)CE_SubsidiaryOperationType.报废,
                    m_lnqWSCode.WSCode);

                foreach (DataRow dr in tempTable.Rows)
                {
                    DataGridViewRow row = new DataGridViewRow();

                    this.dataGridView1.Rows.Add(new object[] { "", Convert.ToInt32(dr["物品ID"]), 
                        dr["图号型号"].ToString(), dr["物品名称"].ToString(), dr["规格"].ToString(), "", Convert.ToDecimal(dr["数量"]),
                        "", "", "","","","","","",dr["批次号"].ToString().Trim().Length == 0 ? "无批次":dr["批次号"].ToString(),"","","",0 });
                }

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
                }

                this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                         this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                         this.dataGridView1_ColumnWidthChanged);

                this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(
                                                       this.dataGridView1_CellValueChanged);
            }
        }

        DataTable frm_OnFormDataTableCheckFind1(DateTime startTime, DateTime endTime)
        {
            return m_billServer.GetScrapBill(startTime , endTime);
        }

        DataTable frm_OnFormDataTableCheckFind(DateTime startTime, DateTime endTime)
        {
            return m_serverMaterials.GetMaterialsTransferInfo(m_lnqWSCode.WSCode, startTime, endTime);
        }

        private void btnExcelInput_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

                if (dtTemp == null)
                {
                    return;
                }

                if (!dtTemp.Columns.Contains("图号型号"))
                {
                    throw new Exception("文件无【图号型号】列名");
                }

                if (!dtTemp.Columns.Contains("物品名称"))
                {
                    throw new Exception("文件无【物品名称】列名");
                }

                if (!dtTemp.Columns.Contains("规格"))
                {
                    throw new Exception("文件无【规格】列名");
                }

                if (!dtTemp.Columns.Contains("版次号"))
                {
                    throw new Exception("文件无【版次号】列名");
                }

                if (!dtTemp.Columns.Contains("报废数量"))
                {
                    throw new Exception("文件无【报废数量】列名");
                }

                if (!dtTemp.Columns.Contains("报废类别"))
                {
                    throw new Exception("文件无【报废类别】列名");
                }

                if (!dtTemp.Columns.Contains("报废原因"))
                {
                    throw new Exception("文件无【报废原因】列名");
                }

                if (!dtTemp.Columns.Contains("责任部门"))
                {
                    throw new Exception("文件无【责任部门】列名");
                }

                if (!dtTemp.Columns.Contains("责任类型"))
                {
                    throw new Exception("文件无【责任类型】列名");
                }

                if (!dtTemp.Columns.Contains("变速箱号"))
                {
                    throw new Exception("文件无【变速箱号】列名");
                }

                if (!dtTemp.Columns.Contains("物品供应商"))
                {
                    throw new Exception("文件无【物品供应商】列名");
                }

                if (!dtTemp.Columns.Contains("责任单位"))
                {
                    throw new Exception("文件无【责任单位】列名");
                }

                if (!dtTemp.Columns.Contains("批次号"))
                {
                    throw new Exception("文件无【批次号】列名");
                }

                if (!dtTemp.Columns.Contains("备注"))
                {
                    throw new Exception("文件无【备注】列名");
                }

                if (!dtTemp.Columns.Contains("报废损失"))
                {
                    throw new Exception("文件无【报废损失】列名");
                }

                if (!dtTemp.Columns.Contains("工时"))
                {
                    throw new Exception("文件无【工时】列名");
                }

                if (dtTemp == null || dtTemp.Rows.Count == 0)
                {
                    return;
                }

                string sql = @"select ID, DeclareWastrelType from S_DeclareWastrelType where ID <> 8";
                DataTable dtWastrelType = DatabaseServer.QueryInfo(sql);

                sql = @"select DepartmentCode, DepartmentName from Department_ZR";
                DataTable dtDepartment = DatabaseServer.QueryInfo(sql);

                string[] strArray = { "在线不良", "售后不良", "连 带 废", "在线挑选", "批量返修", "生 产 废", "工 艺 废", 
                                        "返 修 废", "调 试 废", "开 发 废", "质量验证", "设计变更", "设备故障废" };

                this.dataGridView1.Rows.Clear();

                // 单击进入编辑状态
                this.dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
                this.dataGridView1.AutoGenerateColumns = false;

                dataGridView1.Columns["报废数量"].ValueType = typeof(decimal);
                dataGridView1.Columns["工时"].ValueType = typeof(decimal);

                InitWastrelTypeComboBox();
                InitDepartmentComboBox();

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    if (GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["图号型号"])
                        && GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["物品名称"])
                        && GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["规格"]))
                    {
                        continue;
                    }

                    View_F_GoodsPlanCost goodsInfo =
                        UniversalFunction.GetGoodsInfo(dtTemp.Rows[i]["图号型号"].ToString(),
                        dtTemp.Rows[i]["物品名称"].ToString(),
                        dtTemp.Rows[i]["规格"].ToString());

                    if (!GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["物品供应商"])
                        && UniversalFunction.GetProviderInfo(dtTemp.Rows[i]["物品供应商"].ToString()) == null)
                    {
                        throw new Exception("第【" + (i + 2).ToString() + "】行，物品供应商错误");
                    }

                    if (!GlobalObject.GeneralFunction.IsNullOrEmptyObject(dtTemp.Rows[i]["责任单位"])
                        && UniversalFunction.GetProviderInfo(dtTemp.Rows[i]["责任单位"].ToString()) == null)
                    {
                        throw new Exception("第【" + (i + 2).ToString() + "】行，责任单位错误");
                    }

                    if (goodsInfo == null)
                    {
                        throw new Exception("第【" + (i + 2).ToString() + "】行，图号型号，物品名称，规格错误");
                    }

                    if (dtWastrelType.Select("DeclareWastrelType = '" + dtTemp.Rows[i]["报废类别"].ToString() + "'").Count() == 0)
                    {
                        throw new Exception("第【" + (i + 2).ToString() + "】行，报废类别错误");
                    }
                    else
                    {
                        dtTemp.Rows[i]["报废类别"] = dtWastrelType.Select("DeclareWastrelType = '" + dtTemp.Rows[i]["报废类别"].ToString() + "'")[0][0];
                    }

                    if (dtDepartment.Select("DepartmentName = '" + dtTemp.Rows[i]["责任部门"].ToString() + "'").Count() == 0)
                    {
                        throw new Exception("第【" + (i + 2).ToString() + "】行，责任部门错误");
                    }
                    else
                    {
                        dtTemp.Rows[i]["责任部门"] = dtDepartment.Select("DepartmentName = '" + dtTemp.Rows[i]["责任部门"].ToString() + "'")[0][0];
                    }

                    if (!strArray.Contains(dtTemp.Rows[i]["责任类型"].ToString()))
                    {
                        throw new Exception("第【" + (i + 2).ToString() + "】行，责任类型错误");
                    }

                    DataGridViewRow row = new DataGridViewRow();

                    dataGridView1.Rows.Add(new object[] { "", goodsInfo.序号, goodsInfo.图号型号, goodsInfo.物品名称,
                            goodsInfo.规格, dtTemp.Rows[i]["版次号"].ToString(), dtTemp.Rows[i]["报废数量"].ToString(), Convert.ToInt32( dtTemp.Rows[i]["报废类别"]),
                            dtTemp.Rows[i]["报废原因"].ToString(), dtTemp.Rows[i]["责任部门"].ToString(), dtTemp.Rows[i]["责任类型"].ToString(),"",
                            dtTemp.Rows[i]["变速箱号"].ToString(), dtTemp.Rows[i]["物品供应商"].ToString(), dtTemp.Rows[i]["责任单位"].ToString(),
                            "", dtTemp.Rows[i]["批次号"].ToString(), "", dtTemp.Rows[i]["备注"].ToString(),
                            Convert.ToDecimal(dtTemp.Rows[i]["报废损失"].ToString()), Convert.ToDecimal( dtTemp.Rows[i]["工时"].ToString())});


                    for (int k = 0; k < dataGridView1.Columns.Count; k++)
                    {
                        dataGridView1.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
                    }

                    this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                             this.dataGridView1_ColumnWidthChanged);

                    ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                    this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                                                             this.dataGridView1_ColumnWidthChanged);

                    this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(
                                                           this.dataGridView1_CellValueChanged);

                    m_updateFlag = true;
                }
                
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                return;
            }
        }
    }
}
