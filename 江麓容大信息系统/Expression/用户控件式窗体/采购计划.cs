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
using System.Threading;
using UniversalControlLibrary;
using Expression.Properties;

namespace Expression
{
    /// <summary>
    /// 采购计划界面
    /// </summary>
    public partial class 采购计划 : Form
    {
        /// <summary>
        /// Bom表服务组件
        /// </summary>
        IBomServer m_serverBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IPurcharsingPlan m_serverPurPlan = ServerModuleFactory.GetServerModule<IPurcharsingPlan>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 已下订单数
        /// </summary>
        decimal m_dcmOrderFormCount = 0;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgatorForMarketing = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgatorForProduct = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 采购计划(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "采购计划";

            m_msgPromulgatorForMarketing.BillType = "营销要货计划";

            m_msgPromulgatorForProduct.BillType = "生产计划";

            m_authFlag = nodeInfo.Authority;

            InsertCombox();

            ClearDate();
        }

        private void 采购计划_Load(object sender, EventArgs e)
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
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "采购计划");

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_msgPromulgator.DestroyMessage(msg.MessageContent);
                        MessageDialog.ShowPromptMessage("未找到相关记录");
                    }
                    else
                    {
                        cmbYear.Text = msg.MessageContent.ToString().Substring(0, 4);
                        cmbMonth.Text = msg.MessageContent.ToString().Substring(4, 2);
                        btnFind_Click(null, null);
                        //dataGridView1.DataSource = dtMessage;
                    }

                    break;

                default:
                    base.DefWndProc(ref m);
                    break;
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
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 采购计划_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            if (source == null)
            {
                MessageDialog.ShowPromptMessage("此年月份无采购计划");
                return;
            }

            dataGridView1.Columns.Clear();

            dataGridView1.DataSource = source;

            foreach (DataGridViewColumn col in dataGridView1.Columns)
            {
                if (col.HeaderText.Contains("第一"))
                {
                    col.HeaderText = col.HeaderText.Replace("第一", UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 0).ToString());
                }
                else if (col.HeaderText.Contains("第二"))
                {
                    col.HeaderText = col.HeaderText.Replace("第二", UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 1).ToString());
                }
                else if (col.HeaderText.Contains("第三"))
                {
                    col.HeaderText = col.HeaderText.Replace("第三", UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 2).ToString());
                }
                else if (col.HeaderText == "物品ID")
                {
                    col.Visible = false;
                }
            }

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

        }

        /// <summary>
        /// COMBOX 插入数据
        /// </summary>
        void InsertCombox()
        {

            for (int i = 2011; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
            }

        }

        /// <summary>
        /// 清空数据
        /// </summary>
        private void ClearDate()
        {
            dataGridView1.DataSource = m_serverPurPlan.GetList("");
            lb_BZR.Text = "";
            lb_BZRQ.Text = "";
            lb_PZR.Text = "";
            lb_PZRQ.Text = "";
            lb_SHR.Text = "";
            lb_SHRQ.Text = "";
            lbDJZT.Text = "";
        }

        /// <summary>
        /// 显示数据
        /// </summary>
        /// <param name="i">显示方式 0：返回查询信息 1：返回最新信息</param>
        void ShowDate(int i)
        {

            string strNy = cmbYear.Text.Trim() + cmbMonth.Text.Trim();
            DataRow dr = m_serverPurPlan.GetBill(strNy);

            if (dr == null || i != 0)
            {
                lbDJZT.Text = "新建计划";
            }
            else
            {
                lbDJZT.Text = dr["DJZT"].ToString();
                lb_BZR.Text = dr["BZR"].ToString();
                lb_BZRQ.Text = dr["BZRQ"].ToString();
                lb_PZR.Text = dr["PZR"].ToString();
                lb_PZRQ.Text = dr["PZRQ"].ToString();
                lb_SHR.Text = dr["SHR"].ToString();
                lb_SHRQ.Text = dr["SHRQ"].ToString();
            }

            DataTable dt = new DataTable();

            if (i == 0)
            {
                dt = m_serverPurPlan.GetList(strNy);
            }
            else
            {
                if (!m_serverPurPlan.IsFinish(strNy, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    ClearDate();
                    return;
                }

                CursorControl.SetWaitCursor(this);
                dt = m_serverPurPlan.GetNewList(strNy, out m_err);
                this.Cursor = System.Windows.Forms.Cursors.Arrow;
                if (dt == null)
                {
                    MessageDialog.ShowPromptMessage(m_err);
                    ClearDate();
                    return;
                }
            }

            if (dt != null && dt.Rows.Count > 0)
            {
                lbPrice.Text = Convert.ToDecimal(dt.Compute("sum([订货金额])", "")).ToString();
            }

            RefreshDataGirdView(dt);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (cmbMonth.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择月份");
                return;
            }

            if (cmbYear.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择年份");
                return;
            }

            ClearDate();
            ShowDate(0);
        }

        private void 保存采购计划ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text != "单据已完成")
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;

                string strNy = cmbYear.Text.Trim() + cmbMonth.Text.Trim();
                dt = m_serverPurPlan.GetOrderGoodsCount(dt);
                dt = m_serverPurPlan.GetOrderGoodsPrice(dt);

                if (!m_serverPurPlan.UpdateBill(dt, strNy, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("保存成功");

                    m_msgPromulgator.DestroyMessage(strNy);
                    //m_msgPromulgator.SendNewFlowMessage(strNy,
                    //    string.Format("{0}号采购计划已提交，请等待采购主管审核", strNy),
                    //    BillFlowMessage_ReceivedUserType.角色, RoleEnum.生产管理部负责人.ToString());

                    #region 消息流

                    DataTable dtMarketing = new DataTable();
                    DataTable dtProduct = new DataTable();

                    m_serverPurPlan.GetMarketingPlanAndProductPlan(strNy, out dtMarketing, out dtProduct);

                    if (dtMarketing != null)
                    {
                        List<string> noticeRoles = new List<string>();

                        noticeRoles.Add(CE_RoleEnum.营销普通人员.ToString());
                        noticeRoles.Add(CE_RoleEnum.营销负责人.ToString());
                        noticeRoles.Add(CE_RoleEnum.营销分管领导.ToString());

                        for (int i = 0; i < dtMarketing.Rows.Count; i++)
                        {

                            m_msgPromulgatorForMarketing.EndFlowMessage(
                                dtMarketing.Rows[i]["DJH"].ToString(),
                                string.Format("{0} 号营销要货计划已经处理完毕",
                                dtMarketing.Rows[i]["DJH"].ToString()), noticeRoles, null);
                        }
                    }

                    if (dtProduct != null)
                    {
                        List<string> noticeRoles = new List<string>();

                        noticeRoles.Add(CE_RoleEnum.制造部办公室文员.ToString());
                        noticeRoles.Add(CE_RoleEnum.制造负责人.ToString());
                        noticeRoles.Add(CE_RoleEnum.制造分管领导.ToString());

                        for (int i = 0; i < dtProduct.Rows.Count; i++)
                        {
                            m_msgPromulgatorForMarketing.EndFlowMessage(
                                dtProduct.Rows[i]["DJH"].ToString(),
                                string.Format("{0} 号生产计划已经处理完毕",
                                dtProduct.Rows[i]["DJH"].ToString()), noticeRoles, null);
                        }
                    }

                    #endregion

                    ShowDate(0);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认计划状态");
            }
        }

        private void 导出EXCELToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 批准通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待领导批准")
            {
                UpdateProductBill("等待领导批准");

                #region 发送知会消息

                string strNy = cmbYear.Text.Trim() + cmbMonth.Text.Trim();

                List<string> noticeRoles = new List<string>();
                noticeRoles.Add(CE_RoleEnum.采购账务管理员.ToString());
                noticeRoles.Add(CE_RoleEnum.采购主管.ToString());
                noticeRoles.Add(CE_RoleEnum.采购分管领导.ToString());

                m_msgPromulgator.EndFlowMessage(strNy,
                    string.Format("{0} 号采购计划单已处理完毕", strNy),
                    noticeRoles, null);
                #endregion

            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 部门主管审核ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待主管审核")
            {
                UpdateProductBill("等待主管审核");

                string strNy = cmbYear.Text.Trim() + cmbMonth.Text.Trim();

                m_msgPromulgator.PassFlowMessage(strNy,
                    string.Format("{0}号采购计划已审核，请采购分管领导批准",
                    strNy), CE_RoleEnum.生产管理分管领导.ToString(), true);


            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        /// <summary>
        /// 修改数据库
        /// </summary>
        private void UpdateProductBill(string strDJZT)
        {
            string strNy = cmbYear.Text.Trim() + cmbMonth.Text.Trim();

            if (m_serverPurPlan.UpdateBill(strNy, strDJZT, out m_err))
            {
                MessageDialog.ShowPromptMessage("操作成功！");
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            ShowDate(0);
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (lbDJZT.Text == "等待主管审核")
            //{
            //    if (e.ColumnIndex == 18)
            //    {
            //        dataGridView1.ReadOnly = false;

            //    }
            //    else
            //    {
            //        dataGridView1.ReadOnly = true;
            //    }
            //}
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["已下订单数"].Value.ToString() == "")
            {
                dataGridView1.CurrentRow.Cells["已下订单数"].Value = 0;
            }

            dataGridView1.CurrentRow.Cells["计算已下订单数"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算已下订单数"].Value) - m_dcmOrderFormCount +
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["已下订单数"].Value);

            DataTable dtJumblyGoods = m_serverBom.GetJumblyGoods(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value));

            for (int i = 0; i < dtJumblyGoods.Rows.Count; i++)
            {
                for (int k = 0; k < dataGridView1.Rows.Count; k++)
                {
                    if (Convert.ToInt32(dtJumblyGoods.Rows[i]["JumblyGoodsID"]) 
                        == Convert.ToInt32( dataGridView1.Rows[k].Cells["物品ID"].Value))
                    {
                        dataGridView1.Rows[k].Cells["计算已下订单数"].Value =
                            Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算已下订单数"].Value);


                        dataGridView1.Rows[k].Cells["第一月订货总数"].Value =
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["第一月计划数"].Value) +
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["计算安全库存数"].Value) -
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["计算库存数"].Value) -
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["计算待检数"].Value) -
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["计算已下订单数"].Value);

                        dataGridView1.Rows[k].Cells["第二月订货总数"].Value =
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["第一月订货总数"].Value) +
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["第二月计划数"].Value);

                        if (Convert.ToDecimal(dataGridView1.Rows[k].Cells["第二月订货总数"].Value) > 0)
                        {
                            dataGridView1.Rows[k].Cells["第三月订货总数"].Value =
                                Convert.ToDecimal(dataGridView1.Rows[k].Cells["第三月计划数"].Value);
                        }
                        else
                        {
                            dataGridView1.Rows[k].Cells["第三月订货总数"].Value =
                                Convert.ToDecimal(dataGridView1.Rows[k].Cells["第二月订货总数"].Value) +
                                Convert.ToDecimal(dataGridView1.Rows[k].Cells["第三月计划数"].Value);
                        }


                        dataGridView1.Rows[k].Cells["第一月订货数"].Value =
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["第一月订货总数"].Value) *
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["采购份额"].Value) *
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["装配采购份额"].Value);

                        dataGridView1.Rows[k].Cells["第二月订货数"].Value =
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["第二月订货总数"].Value) *
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["采购份额"].Value) *
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["装配采购份额"].Value);

                        dataGridView1.Rows[k].Cells["第三月订货数"].Value =
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["第三月订货总数"].Value) *
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["采购份额"].Value) *
                            Convert.ToDecimal(dataGridView1.Rows[k].Cells["装配采购份额"].Value);
                    }
                }
            }

            dataGridView1.CurrentRow.Cells["第一月订货总数"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第一月计划数"].Value) +
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算安全库存数"].Value) -
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算库存数"].Value) -
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算待检数"].Value) -
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["计算已下订单数"].Value);

            dataGridView1.CurrentRow.Cells["第二月订货总数"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第一月订货总数"].Value) +
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第二月计划数"].Value);

            if (Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第二月订货总数"].Value) > 0)
            {
                dataGridView1.CurrentRow.Cells["第三月订货总数"].Value =
                    Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第三月计划数"].Value);
            }
            else
            {
                dataGridView1.CurrentRow.Cells["第三月订货总数"].Value =
                    Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第二月订货总数"].Value) +
                    Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第三月计划数"].Value);
            }


            dataGridView1.CurrentRow.Cells["第一月订货数"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第一月订货总数"].Value) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["采购份额"].Value) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["装配采购份额"].Value);

            dataGridView1.CurrentRow.Cells["第二月订货数"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第二月订货总数"].Value) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["采购份额"].Value) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["装配采购份额"].Value);

            dataGridView1.CurrentRow.Cells["第三月订货数"].Value =
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["第三月订货总数"].Value) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["采购份额"].Value) *
                Convert.ToDecimal(dataGridView1.CurrentRow.Cells["装配采购份额"].Value);
        }

        /// <summary>
        /// 获取日期是否在查询范围内
        /// </summary>
        /// <returns>通过返回True，不通过返回False</returns>
        bool GetTime()
        {
            if (cmbYear.Text == "" || cmbMonth.Text == "")
            {
                return true;
            }

            int intEndYear = Convert.ToInt32(cmbYear.Text);
            int intEndMonth = Convert.ToInt32(cmbMonth.Text);
            int intStartYear;
            int intStartMonth;

            if (intEndMonth == 1)
            {
                intStartMonth = 12;
                intStartYear = intEndYear - 1;
            }
            else
            {
                intStartMonth = intEndMonth - 1;
                intStartYear = intEndYear;
            }

            DateTime dtStart = Convert.ToDateTime(intStartYear.ToString() + "-"
                + intStartMonth.ToString() + "-" + Convert.ToInt32(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日]) + " 00:00:00");
            DateTime dtEnd = Convert.ToDateTime(intEndYear.ToString() + "-"
                + intEndMonth.ToString() + "-" + Convert.ToInt32(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.月结日]) + " 00:00:00");

            return m_serverPurPlan.IsAllowCreate(dtStart, dtEnd, cmbYear.Text + cmbMonth.Text);
        }

        private void cmbYear_SelectedValueChanged(object sender, EventArgs e)
        {
            btnGetNewInfo.Enabled = GetTime();
        }

        private void cmbMonth_SelectedValueChanged(object sender, EventArgs e)
        {
            btnGetNewInfo.Enabled = GetTime();
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            m_dcmOrderFormCount = Convert.ToDecimal( dataGridView1.CurrentRow.Cells["已下订单数"].Value);
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

        private void btnGetNewInfo_Click(object sender, EventArgs e)
        {
            if (cmbMonth.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择月份");
                return;
            }

            if (cmbYear.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择年份");
                return;
            }

            if (!m_serverPurPlan.IsQualified(cmbYear.Text + cmbMonth.Text, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }

            ClearDate();
            ShowDate(1);
        }

        private void 采购计划公式编写ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            采购计划计算 form = new 采购计划计算(dataGridView1.Columns);
            form.ShowDialog();
        }

        private void 确认采购计划ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待确认")
            {
                UpdateProductBill("等待确认");

                #region 发送知会消息

                string strNy = cmbYear.Text.Trim() + cmbMonth.Text.Trim();

                List<string> noticeRoles = new List<string>();
                noticeRoles.Add(CE_RoleEnum.采购账务管理员.ToString());
                noticeRoles.Add(CE_RoleEnum.采购主管.ToString());
                noticeRoles.Add(CE_RoleEnum.采购分管领导.ToString());

                m_msgPromulgator.EndFlowMessage(strNy,
                    string.Format("{0} 号采购计划单已处理完毕", strNy),
                    noticeRoles, null);
                #endregion

            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }
    }
}
