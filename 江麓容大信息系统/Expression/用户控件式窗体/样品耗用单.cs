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
using GlobalObject;
using UniversalControlLibrary;
using Expressio;

namespace Expression
{
    /// <summary>
    /// 样品耗用单界面
    /// </summary>
    public partial class 样品耗用单 : Form
    {
        /// <summary>
        /// 部门服务组件
        /// </summary>
        IDepartmentServer m_serverDepartment = ServerModuleFactory.GetServerModule<IDepartmentServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = 
            BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务
        /// </summary>
        IMusterUse m_serverUse = ServerModuleFactory.GetServerModule<IMusterUse>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据集
        /// </summary>
        S_MusterUseBill m_lnqBill = new S_MusterUseBill();

        /// <summary>
        /// 修改标志
        /// </summary>
        bool m_editFlag = false;

        public 样品耗用单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "样品耗用单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.样品耗用单, m_serverUse);

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
            RefreshDataGirdView(m_serverUse.GetAllBill());

            m_authFlag = nodeInfo.Authority;
        }

        void tbsBatchNo_OnCompleteSearch()
        {
            tbsBatchNo.Text = tbsBatchNo.DataResult["批次号"].ToString();
            txtVersion.Text = tbsBatchNo.DataResult["版次号"].ToString();
            txtStockCount.Text = m_serverUse.GetStockCount(Convert.ToInt32( tbsCode.Tag), tbsBatchNo.Text).ToString();
        }

        void tbsCode_OnCompleteSearch()
        {
            tbsCode.Text = tbsCode.DataResult["图号型号"].ToString();
            tbsCode.Tag = Convert.ToInt32(tbsCode.DataResult["物品ID"].ToString());
            txtName.Text = tbsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = tbsCode.DataResult["规格"].ToString();
            lbdw.Text = tbsCode.DataResult["单位"].ToString();
            lbKCDW.Text = tbsCode.DataResult["单位"].ToString();
            tbsBatchNo.Text = "";
            txtStockCount.Text = "";
            numCount.Value = 0;
            txtRemark.Text = "";
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                //接收自定义消息,放弃未提交的单据号
                case WndMsgSender.CloseMsg:
                    // 放弃未使用的单据号
                    m_billNoControl.CancelBill();
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "样品耗用单");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_msgPromulgator.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        dataGridView1.DataSource = dtMessage;
                        dataGridView1.Rows[0].Selected = true;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
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
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];

                    dataGridView1_CellClick(null, null);

                    break;
                }
            }
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            ClearInfo();
            dataGridView1.DataSource = source;

            dataGridView1.Columns["单据号"].Width = 120;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        void GetMessage()
        {
            m_lnqBill = new S_MusterUseBill();
            m_lnqBill.DJH = txtDJH.Text;
            m_lnqBill.DJZT = lbDJZT.Text;
            m_lnqBill.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
        }

        /// <summary>
        /// 变更单据状态
        /// </summary>
        void UpdateBill()
        {
            GetMessage();

            if (m_lnqBill.DJZT == "单据已完成" || m_lnqBill.DJZT == "单据已报废")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
            else
            {
                if (!m_serverUse.UpdateBill(m_lnqBill,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    return;
                }
                else
                {
                    if (lbDJZT.Text == "等待确认出库")
                    {
                        m_billNoControl.UseBill(m_lnqBill.DJH);
                    }

                    MessageDialog.ShowPromptMessage("提交成功");
                }
            }

            RefreshDataGirdView(m_serverUse.GetAllBill());
            PositioningRecord(m_lnqBill.DJH);
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            txtDJH.Text = m_billNoControl.GetNewBillNo();
            lbDJZT.Text = "新建单据";
            txtName.Text = "";
            numCount.Value = 0;
            tbsCode.Text = "";
            tbsCode.Tag = -1;
            txtSpec.Text = "";
            tbsBatchNo.Text = "";
            txtStockCount.Text = "";
            txtVersion.Text = "";
            txtRemark.Text = "";
            txtPurpose.Tag = -1;
            txtPurpose.Text = "";
            dataGridView2.DataSource = m_serverUse.GetList(txtDJH.Text);
            btnAdd.Enabled = true;
            btnDelete.Enabled = true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count < 1)
            {
                return;
            }
            else
            {
                tbsCode.Text = dataGridView2.CurrentRow.Cells["图号型号"].Value.ToString();
                tbsCode.Tag = Convert.ToInt32(dataGridView2.CurrentRow.Cells["物品ID"].Value.ToString());
                txtName.Text = dataGridView2.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView2.CurrentRow.Cells["规格"].Value.ToString();
                numCount.Value = Convert.ToDecimal( dataGridView2.CurrentRow.Cells["数量"].Value);
                tbsBatchNo.Text = dataGridView2.CurrentRow.Cells["批次号"].Value.ToString();
                lbdw.Text = dataGridView2.CurrentRow.Cells["单位"].Value.ToString();
                lbKCDW.Text = dataGridView2.CurrentRow.Cells["单位"].Value.ToString();
                txtStockCount.Text = m_serverUse.GetStockCount(Convert.ToInt32(tbsCode.Tag), tbsBatchNo.Text).ToString();
                txtVersion.Text = dataGridView2.CurrentRow.Cells["版次号"].Value.ToString();
                txtRemark.Text = dataGridView2.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_lnqBill = m_serverUse.GetBill(txtDJH.Text);

            if (m_lnqBill != null)
            {
                if (m_lnqBill.DJZT == "单据已完成" || m_lnqBill.DJZT == "单据已报废")
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                    return;
                }
            }

            if (txtPurpose.Text.Trim() == "" 
                || txtPurpose.Tag == null 
                || txtPurpose.Tag.ToString() == ""
                || txtPurpose.Tag.ToString() == "-1")
            {
                MessageDialog.ShowPromptMessage("请选择用途");
                return;
            }

            if (lbDJZT.Text != "新建单据" && dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("您不是此单据的编制人，请重新确认");
                return;
            }

            if (!m_serverUse.SaveBill(txtDJH.Text, UniversalFunction.GetStorageID(cmbStorage.Text), txtPurpose.Tag.ToString(),
                (DataTable)dataGridView2.DataSource, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");
                m_editFlag = false;
                m_msgPromulgator.DestroyMessage(txtDJH.Text);
                m_msgPromulgator.SendNewFlowMessage(txtDJH.Text,
                    string.Format("{0} 号样品耗用单，请主管审核", txtDJH.Text), BillFlowMessage_ReceivedUserType.角色,
                    m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));
            }

            m_lnqBill = m_serverUse.GetBill(txtDJH.Text);

            RefreshDataGirdView(m_serverUse.GetAllBill());
            PositioningRecord(m_lnqBill.DJH);
        }

        private void 部门主管审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待主管审核")
            {
                UpdateBill();
                m_msgPromulgator.PassFlowMessage(m_lnqBill.DJH,
                    string.Format("{0} 号样品耗用单，请仓管确认", m_lnqBill.DJH), CE_RoleEnum.样品库管理员.ToString(), true);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void 批准通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待确认出库")
            {
                UpdateBill();

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();


                string strDept = m_serverDepartment.GetDeptInfoFromPersonnelInfo(
                    m_serverUse.GetBill(m_lnqBill.DJH).SQR).Rows[0]["DepartmentCode"].ToString();
                noticeRoles.AddRange(m_billMessageServer.GetDeptDirectorRoleName(strDept));

                noticeRoles.Add(m_billMessageServer.GetRoleStringForStorage(cmbStorage.Text).ToString());

                m_msgPromulgator.EndFlowMessage(m_lnqBill.DJH,
                    string.Format("{0} 号样品耗用单已经处理完毕", m_lnqBill.DJH),
                    noticeRoles, null);

                #endregion 发送知会消息

            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView2.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (tbsCode.Tag.ToString() == dt.Rows[i]["物品ID"].ToString()
                    && tbsBatchNo.Text == dt.Rows[i]["批次号"].ToString())
                {
                    MessageDialog.ShowPromptMessage("不同添加同种物品同批次");
                    return;
                }
            }

            if (tbsBatchNo.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("批次不能为空");
                return;
            }
            else if (txtStockCount.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("库存数不能为空");
                return;

            }
            else if (txtRemark.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("备注不能为空,请详细填写使用信息");
                return;
            }
            else if (numCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("数量必须大于0");
                return;
            }
            else if (numCount.Value > Convert.ToDecimal( txtStockCount.Text))
            {
                MessageDialog.ShowPromptMessage("耗用数必须小于库存数");
                return;
            }

            DataRow dr = dt.NewRow();

            dr["物品ID"] = Convert.ToInt32(tbsCode.Tag.ToString());
            dr["物品名称"] = txtName.Text;
            dr["图号型号"] = tbsCode.Text;
            dr["规格"] = txtSpec.Text;
            dr["数量"] = numCount.Value;
            dr["单据号"] = txtDJH.Text;
            dr["单位"] = lbdw.Text;
            dr["批次号"] = tbsBatchNo.Text;
            dr["库存数"] = Convert.ToDecimal( txtStockCount.Text);
            dr["版次号"] = txtVersion.Text;
            dr["备注"] = txtRemark.Text;

            dt.Rows.Add(dr);

            m_editFlag = true;
            dataGridView2.DataSource = dt;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                return;
            }

            DataTable dt = (DataTable)dataGridView2.DataSource;
            dt.Rows.RemoveAt(dataGridView2.CurrentRow.Index);
            m_editFlag = true;
            dataGridView2.DataSource = dt;
        }

        private void tbsBatchNo_Enter(object sender, EventArgs e)
        {
            tbsBatchNo.StrEndSql = " and a.GoodsID = " + Convert.ToInt32(tbsCode.Tag.ToString());
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["申请人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (lbDJZT.Text != "单据已完成")
            {
                if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
                {
                    return;
                }

                if (!m_serverUse.ScarpBill(txtDJH.Text,out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    m_billNoControl.CancelBill(txtDJH.Text);
                    MessageDialog.ShowPromptMessage("报废成功！");
                    m_msgPromulgator.DestroyMessage(txtDJH.Text);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            RefreshDataGirdView(m_serverUse.GetAllBill());
            PositioningRecord(dataGridView1.Rows[0].Cells["单据号"].Value.ToString());
        }

        void ClearInfo()
        {
            txtDJH.Text = "";
            txtName.Text = "";
            numCount.Value = 0;
            tbsCode.Text = "";
            tbsCode.Tag = -1;
            txtSpec.Text = "";
            tbsBatchNo.Text = "";
            txtStockCount.Text = "";
            txtVersion.Text = "";
            txtRemark.Text = "";

            dataGridView2.DataSource = null;
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverUse.GetAllBill());
            PositioningRecord(m_lnqBill.DJH);
        }

        private void tbsCode_Enter(object sender, EventArgs e)
        {
            string strSql = "";
            string strStorage = UniversalFunction.GetStorageID(cmbStorage.Text);

            if (strStorage == null)
            {
                strSql += " and 库房代码  is null ";
            }
            else
            {
                strSql += " and 库房代码  = '" + strStorage + "'";
            }

            tbsCode.StrEndSql = strSql;
        }

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "样品耗用综合查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
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

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            领料用途 form = new 领料用途();

            if (form.ShowDialog() == DialogResult.OK)
            {
                txtPurpose.Tag = form.SelectedData.Code;
                txtPurpose.Text = form.SelectedData.Purpose;
            }
        }

        private void 样品耗用单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
            编制人操作ToolStripMenuItem.Visible = false;
        }

        private void 打印单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IBillReportInfo report = new 报表_样品耗用单(txtDJH.Text, "样品耗用单");

            PrintReportBill print = new PrintReportBill(21.8, 9.31, report);
            print.DirectPrintReport();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count < 1)
            {
                return;
            }
            else
            {

                if (txtDJH.Text != dataGridView1.CurrentRow.Cells["单据号"].Value.ToString()
                    && lbDJZT.Text != "单据已完成" && m_editFlag)
                {
                    m_editFlag = false;

                    if (MessageDialog.ShowEnquiryMessage("是否要保存您已做的操作?") == DialogResult.Yes)
                    {
                        string tempBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                        m_lnqBill = m_serverUse.GetBill(txtDJH.Text);

                        if (m_lnqBill != null)
                        {
                            if (m_lnqBill.DJZT == "单据已完成" || m_lnqBill.DJZT == "单据已报废")
                            {
                                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                                return;
                            }
                        }

                        if (txtPurpose.Text.Trim() == ""
                            || txtPurpose.Tag == null
                            || txtPurpose.Tag.ToString() == ""
                            || txtPurpose.Tag.ToString() == "-1")
                        {
                            MessageDialog.ShowPromptMessage("请选择用途");
                            return;
                        }

                        if (!m_serverUse.SaveBill(txtDJH.Text, UniversalFunction.GetStorageID(cmbStorage.Text), txtPurpose.Tag.ToString(),
                            (DataTable)dataGridView2.DataSource, out m_err))
                        {
                            MessageDialog.ShowPromptMessage(m_err);
                            return;
                        }
                        else
                        {
                            m_msgPromulgator.DestroyMessage(txtDJH.Text);
                            m_msgPromulgator.SendNewFlowMessage(txtDJH.Text,
                                string.Format("{0} 号样品耗用单，请主管审核", txtDJH.Text), BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));
                        }

                        RefreshDataGirdView(m_serverUse.GetAllBill());
                        m_lnqBill = m_serverUse.GetBill(txtDJH.Text);
                        PositioningRecord(tempBillNo);
                    }
                }
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成")
                {
                    btnAdd.Enabled = false;
                    btnDelete.Enabled = false;
                }
                else
                {
                    if (dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
                    {
                        btnAdd.Enabled = false;
                        btnDelete.Enabled = false;
                    }
                    else
                    {
                        btnAdd.Enabled = true;
                        btnDelete.Enabled = true;
                    }
                }

                txtDJH.Text = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                dataGridView2.DataSource = m_serverUse.GetList(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                cmbStorage.Text = dataGridView1.CurrentRow.Cells["库房名称"].Value.ToString();
                txtPurpose.Text = dataGridView1.CurrentRow.Cells["用途"].Value.ToString();
                txtPurpose.Tag = dataGridView1.CurrentRow.Cells["用途编码"].Value.ToString();
            }
        }

        private void 打印清单ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            报表_样品耗用清单 report = new 报表_样品耗用清单(txtDJH.Text, "样品耗用单");
            report.ShowDialog();
        }
    }
}
