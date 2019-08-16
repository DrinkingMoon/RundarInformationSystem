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

namespace Expression
{
    /// <summary>
    /// 产品型号变更单界面
    /// </summary>
    public partial class 产品型号变更单 : Form
    {
        /// <summary>
        /// 营销产品服务组件
        /// </summary>
        IBomServer m_serviceBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务
        /// </summary>
        IProductPlan m_serverPdPlan = ServerModuleFactory.GetServerModule<IProductPlan>();

        /// <summary>
        /// 明细TB
        /// </summary>
        DataTable m_dtList = new DataTable();

        /// <summary>
        /// 服务LNQ
        /// </summary>
        P_ProductChangeBill m_lnqPdPlan = new P_ProductChangeBill();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductChange m_serverProduct = ServerModuleFactory.GetServerModule<IProductChange>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 产品型号变更单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "产品变更单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.产品变更单, m_serverProduct);

            m_authFlag = nodeInfo.Authority;

            ClearDate();
            RefreshDataGirdView(m_serverProduct.GetAllBill());
            cmbBeforeType.DataSource = m_serviceBom.GetAssemblyTypeList();
            cmbAfterType.DataSource = m_serviceBom.GetAssemblyTypeList();

            dataGridView2.Columns["单据号"].Visible = false;
            dataGridView2.Columns["变更前物品ID"].Visible = false;
            dataGridView2.Columns["变更后物品ID"].Visible = false;
        }

        private void 产品型号变更单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "产品变更单");

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

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDate();
            txtDJH.Text = m_billNoControl.GetNewBillNo();
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearDate()
        {
            txtRemark.Text = "";
            txtBillRemark.Text = "";
            cmbAfterType.SelectedIndex = -1;
            cmbBeforeType.SelectedIndex = -1;
            cmbBillType.SelectedIndex = -1;
            txtDJH.Text = "";
            dataGridView2.DataSource = m_serverProduct.GetList("");
            txtRemark.Text = "";
            lb_BZRQ.Text = "";
            lb_PZR.Text = "";
            lb_PZRQ.Text = "";
            lb_SHR.Text = "";
            lb_SHRQ.Text = "";
            lbdw.Text = "台";
            lb_BZR.Text = "";
            lbDJZT.Text = "";
            numCount.Value = 0;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
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
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        private void GetMessage()
        {
            m_dtList = (DataTable)dataGridView2.DataSource;

            m_lnqPdPlan.DJH = txtDJH.Text;
            m_lnqPdPlan.DJZT = lbDJZT.Text;
            m_lnqPdPlan.LRRY = UniversalFunction.GetPersonnelCode(lb_BZR.Text);
            m_lnqPdPlan.LRRQ = lb_BZRQ.Text == "" ? ServerTime.Time : Convert.ToDateTime(lb_BZRQ.Text);
            m_lnqPdPlan.SHRY = UniversalFunction.GetPersonnelCode(lb_SHR.Text);
            m_lnqPdPlan.SHRQ = lb_SHRQ.Text == "" ? ServerTime.Time : Convert.ToDateTime(lb_SHRQ.Text);
            m_lnqPdPlan.PZRY = UniversalFunction.GetPersonnelCode(lb_PZR.Text);
            m_lnqPdPlan.PZRQ = lb_PZRQ.Text == "" ? ServerTime.Time : Convert.ToDateTime(lb_PZRQ.Text);
            m_lnqPdPlan.Remark = txtBillRemark.Text;
            m_lnqPdPlan.DJLX = cmbBillType.Text;
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (txtDJH.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("单据号为空，请新建单据！");
                return;
            }

            if (cmbBillType.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择单据类型");
                return;
            }

            GetMessage();

            m_lnqPdPlan.DJH = txtDJH.Text;

            if (m_serverProduct.AddBill(m_lnqPdPlan,m_dtList,out m_err))
            {
                MessageDialog.ShowPromptMessage("提交成功！");

                m_msgPromulgator.DestroyMessage(txtDJH.Text);
                m_msgPromulgator.SendNewFlowMessage(txtDJH.Text,
                    string.Format("{0} 号产品型号变更单，请主管审核", txtDJH.Text),
                    m_lnqPdPlan.DJLX == "制造部" ? CE_RoleEnum.制造负责人 : CE_RoleEnum.营销负责人);

            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGirdView(m_serverProduct.GetAllBill());
            PositioningRecord(m_lnqPdPlan.DJH);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                dataGridView2.DataSource = null;
                return;
            }
            else
            {
                lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                txtDJH.Text = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                lb_BZR.Text = dataGridView1.CurrentRow.Cells["录入人"].Value.ToString();
                lb_BZRQ.Text = dataGridView1.CurrentRow.Cells["录入时间"].Value.ToString();
                lb_SHR.Text = dataGridView1.CurrentRow.Cells["审核人"].Value.ToString();
                lb_SHRQ.Text = dataGridView1.CurrentRow.Cells["审核时间"].Value.ToString();
                lb_PZR.Text = dataGridView1.CurrentRow.Cells["批准人"].Value.ToString();
                lb_PZRQ.Text = dataGridView1.CurrentRow.Cells["批准时间"].Value.ToString();
                txtBillRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                cmbBillType.Text = dataGridView1.CurrentRow.Cells["单据类别"].Value.ToString();

                dataGridView2.DataSource = m_serverProduct.GetList(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

                dataGridView2.Columns["单据号"].Visible = false;
                dataGridView2.Columns["变更前物品ID"].Visible = false;
                dataGridView2.Columns["变更后物品ID"].Visible = false;
            }
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Rows.Count == 0)
            {
                return;
            }
            else
            {
                cmbBeforeType.Text = dataGridView2.CurrentRow.Cells["变更前型号"].Value.ToString();
                cmbBeforeType.Tag = Convert.ToInt32(dataGridView2.CurrentRow.Cells["变更前物品ID"].Value);
                cmbAfterType.Text = dataGridView2.CurrentRow.Cells["变更后型号"].Value.ToString();
                cmbAfterType.Tag = Convert.ToInt32(dataGridView2.CurrentRow.Cells["变更后物品ID"].Value);
                numCount.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["数量"].Value.ToString());
                txtRemark.Text = dataGridView2.CurrentRow.Cells["备注"].Value.ToString();
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (numCount.Value <= 0)
            {
                MessageDialog.ShowPromptMessage("请填写数量！");
                return;
            }

            DataTable dt = (DataTable)dataGridView2.DataSource == null ? m_serverProduct.GetList("") : (DataTable)dataGridView2.DataSource;
            DataRow dr = dt.NewRow();

            dr["变更前型号"] = cmbBeforeType.Text;
            dr["变更前物品ID"] = Convert.ToInt32(cmbBeforeType.Tag);
            dr["变更后型号"] = cmbAfterType.Text;
            dr["变更后物品ID"] = Convert.ToInt32(cmbAfterType.Tag);
            dr["数量"] = numCount.Value;
            dr["单据号"] = txtDJH.Text;
            dr["备注"] = txtRemark.Text;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["变更前物品ID"].ToString() == dr["变更前物品ID"].ToString())
                {
                    MessageDialog.ShowErrorMessage("不能录入同种物品！");
                    return;
                }
            }

            dt.Rows.Add(dr);
            dataGridView2.DataSource = dt;
            dataGridView2.Columns["单据号"].Visible = false;
            dataGridView2.Columns["变更前物品ID"].Visible = false;
            dataGridView2.Columns["变更后物品ID"].Visible = false;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView2.DataSource;

            foreach (DataGridViewRow dr in dataGridView2.SelectedRows)
            {
                if (dr.Selected)
                {
                    dt.Rows.RemoveAt(dr.Index);
                }
            }

            dataGridView2.DataSource = dt;
            dataGridView2.Columns["单据号"].Visible = false;
            dataGridView2.Columns["变更前物品ID"].Visible = false;
            dataGridView2.Columns["变更后物品ID"].Visible = false;
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["录入人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("单据已完成,不能进行删除！");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
            {
                return;
            }

            if (m_serverProduct.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
            {
                m_billNoControl.CancelBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                MessageDialog.ShowPromptMessage("删除成功");
                m_msgPromulgator.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            ClearDate();
            RefreshDataGirdView(m_serverProduct.GetAllBill());
        }

        private void 部门主管审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待主管审核")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态！");
                return;
            }

            bool b = m_serverProduct.AuditingBill(txtDJH.Text,out m_err);

            if (b)
            {
                MessageDialog.ShowPromptMessage("【" + txtDJH.Text + "】号单据审核成功！");

                m_msgPromulgator.PassFlowMessage(txtDJH.Text, string.Format("{0} 号产品型号变更单，请质管批准", txtDJH.Text), 
                    CE_RoleEnum.质量工程师.ToString(), true);

                RefreshDataGirdView(m_serverProduct.GetAllBill());
                PositioningRecord(m_lnqPdPlan.DJH);
            }
        }

        private void 批准变更ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "等待质管批准")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态！");
                return;
            }

            bool b = m_serverProduct.AuthorizeBill(txtDJH.Text,out m_err);

            if (b)
            {
                MessageDialog.ShowPromptMessage("【" + txtDJH.Text + "】号单据批准变更！");

                #region 发送知会消息

                List<string> noticeRoles = new List<string>();
                noticeRoles.Add(cmbBillType.Text == "制造部" ? CE_RoleEnum.制造负责人.ToString() : CE_RoleEnum.营销负责人.ToString());
                noticeRoles.Add(CE_RoleEnum.采购账务管理员.ToString());
                noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());

                m_msgPromulgator.EndFlowMessage(txtDJH.Text,
                    string.Format("{0} 号营销出库单已经处理完毕", txtDJH.Text),
                    noticeRoles, null);

                #endregion 发送知会消息

                RefreshDataGirdView(m_serverProduct.GetAllBill());
                PositioningRecord(m_lnqPdPlan.DJH);
            }
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverProduct.GetAllBill());
            PositioningRecord(m_lnqPdPlan.DJH);
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

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView2.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView2.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView2.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
