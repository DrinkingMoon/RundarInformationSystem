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
    /// 营销要货计划界面
    /// </summary>
    public partial class 营销要货计划 : Form
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 服务
        /// </summary>
        IMarketingPlan m_serverMkPlan = ServerModuleFactory.GetServerModule<IMarketingPlan>();

        /// <summary>
        /// 数据集
        /// </summary>
        S_MarketingPlanBill m_lnqMkBill = new S_MarketingPlanBill();

        /// <summary>
        /// 明细TB
        /// </summary>
        DataTable m_dtMx = new DataTable();

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
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 营销要货计划(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "营销要货计划";

            m_authFlag = nodeInfo.Authority;

            InsertCombox();

            ClearDate();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.营销要货计划, m_serverMkPlan);

            RefreshDataGirdView(m_serverMkPlan.GetAllBill());

            cmbBillType.SelectedIndex = 0;
        }

        private void 营销要货计划_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        void tbsCode_OnCompleteSearch()
        {
            tbsCode.Tag = Convert.ToInt32(tbsCode.DataResult["序号"]);
            tbsCode.Text = tbsCode.DataResult["图号型号"].ToString();
            txtName.Text = tbsCode.DataResult["物品名称"].ToString();
            txtSpec.Text = tbsCode.DataResult["规格"].ToString();
            txtCSCode.Text = tbsCode.DataResult["厂商编码"].ToString();
            lbdw1.Text = tbsCode.DataResult["单位"].ToString();
            lbdw2.Text = tbsCode.DataResult["单位"].ToString();
            lbdw3.Text = tbsCode.DataResult["单位"].ToString();

            numFirstMonthCount.Focus();
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "营销要货计划");

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
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            dataGridView1.Columns["文件编号"].Visible = false;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// COMBOX 插入数据
        /// </summary>
        void InsertCombox()
        {
            for (int i = 2010; i < 2050; i++)
            {
                cmbYear.Items.Add(i);
            }

            for (int f = 1; f <= 12; f++)
            {
                cmbMonth.Items.Add(f.ToString("D2"));
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
        /// 清空数据
        /// </summary>
        private void ClearDate()
        {
            txtDJH.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            tbsCode.Text = "";
            txtCSCode.Text = "";
            tbsCode.Tag = -1;
            lbUpLoadFile.Tag = null;
            dataGridView2.DataSource = m_serverMkPlan.GetList("");
            SetToolTipText();
            lbdw1.Text = "";
            lbdw2.Text = "";
            lbdw3.Text = "";
            lbDJZT.Text = "";
            numFirstMonthCount.Value = 0;
            txtFirstMonthSumCont.Text = "0";
            cmbMonth.SelectedIndex = -1;
            cmbYear.SelectedIndex = -1;
            numSecondMonthCount.Value = 0;
            txtSecondMonthSumCount.Text = "0";
            numThirdMonthCount.Value = 0;
            txtThirdMonthSumCount.Text = "0";
            cmbBillType.SelectedIndex = 0;
            lbUpLoadFile.Visible = true;
            lbLookFile.Visible = false;
        }

        /// <summary>
        /// 获得信息
        /// </summary>
        private void GetMessage()
        {
            if (txtDJH.Text == "")
            {
                MessageDialog.ShowPromptMessage("请对应单据");
                return;
            }

            m_dtMx = (DataTable)dataGridView2.DataSource;
            m_lnqMkBill.DJH = txtDJH.Text;
            m_lnqMkBill.BillType = cmbBillType.Text;
            m_lnqMkBill.YearAndMonth = cmbYear.Text + cmbMonth.Text;
            m_lnqMkBill.FirstMonthSumCount = Convert.ToDecimal(txtFirstMonthSumCont.Text);
            m_lnqMkBill.SecondMonthSumCount = Convert.ToDecimal(txtSecondMonthSumCount.Text);
            m_lnqMkBill.ThirdMonthSumCount = Convert.ToDecimal(txtThirdMonthSumCount.Text);
            m_lnqMkBill.DJZT = lbDJZT.Text;
            m_lnqMkBill.Remark = txtBillRemark.Text;
            m_lnqMkBill.FileNo = lbUpLoadFile.Tag.ToString();
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearDate();
            txtDJH.Text = m_billNoControl.GetNewBillNo();
            lbDJZT.Text = "新建单据";
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
                cmbBillType.Text = dataGridView1.CurrentRow.Cells["单据类型"].Value.ToString();
                cmbYear.Text = dataGridView1.CurrentRow.Cells["单据年月"].Value.ToString().Substring(0, 4);
                cmbMonth.Text = dataGridView1.CurrentRow.Cells["单据年月"].Value.ToString().Substring(4, 2);

                txtFirstMonthSumCont.Text = dataGridView1.CurrentRow.Cells["第一个月计划总数"].Value.ToString();
                txtSecondMonthSumCount.Text = dataGridView1.CurrentRow.Cells["第二个月计划总数"].Value.ToString();
                txtThirdMonthSumCount.Text = dataGridView1.CurrentRow.Cells["第三个月计划总数"].Value.ToString();
                dataGridView2.DataSource = m_serverMkPlan.GetList(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());

                SetToolTipText();

                txtBillRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                lbUpLoadFile.Tag = dataGridView1.CurrentRow.Cells["文件编号"].Value.ToString();

                if (lbUpLoadFile.Tag == null || lbUpLoadFile.Tag.ToString().Length == 0)
                {
                    lbLookFile.Visible = false;
                }
                else
                {
                    lbLookFile.Visible = true;
                }
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
                tbsCode.Text = dataGridView2.CurrentRow.Cells["图号型号"].Value.ToString();
                tbsCode.Tag = Convert.ToInt32(dataGridView2.CurrentRow.Cells["物品ID"].Value);
                txtName.Text = dataGridView2.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView2.CurrentRow.Cells["规格"].Value.ToString();
                txtCSCode.Text = dataGridView2.CurrentRow.Cells["厂商编码"].Value.ToString();

                numFirstMonthCount.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells[4].Value);
                numSecondMonthCount.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells[5].Value);
                numThirdMonthCount.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells[6].Value);

                lbdw1.Text = dataGridView2.CurrentRow.Cells["单位"].Value.ToString();
                lbdw2.Text = dataGridView2.CurrentRow.Cells["单位"].Value.ToString();
                lbdw3.Text = dataGridView2.CurrentRow.Cells["单位"].Value.ToString();

                txtListRemark.Text = dataGridView2.CurrentRow.Cells["备注"].Value.ToString();
                SetToolTipText();
            }
        }

        /// <summary>
        /// 设置DataGirdView中的单元格ToolTip
        /// </summary>
        void SetToolTipText()
        {
            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                dataGridView2.Rows[i].Cells[5].ToolTipText = m_serverMkPlan.GetCellToolTipString(txtDJH.Text,
                    Convert.ToInt32(dataGridView2.Rows[i].Cells["物品ID"].Value), 1);
                dataGridView2.Rows[i].Cells[6].ToolTipText = m_serverMkPlan.GetCellToolTipString(txtDJH.Text,
                    Convert.ToInt32(dataGridView2.Rows[i].Cells["物品ID"].Value), 2);
                dataGridView2.Rows[i].Cells[7].ToolTipText = m_serverMkPlan.GetCellToolTipString(txtDJH.Text,
                    Convert.ToInt32(dataGridView2.Rows[i].Cells["物品ID"].Value), 3);
            }
        }

        /// <summary>
        /// 修改数据库
        /// </summary>
        /// <param name="strDJZT">单据状态</param>
        private void UpdateProductBill(string strDJZT)
        {
            DataTable dt = ((DataTable)dataGridView1.DataSource).Clone();

            foreach (DataGridViewRow dr in dataGridView1.SelectedRows)
            {
                if (dr.Selected && dr.Cells["单据状态"].Value.ToString() == strDJZT)
                {
                    DataRow NewDr = dt.NewRow();

                    NewDr["单据号"] = dr.Cells["单据号"].Value.ToString();
                    dt.Rows.Add(NewDr);
                }
            }

            if (m_serverMkPlan.UpdateBill(strDJZT, dt, out m_err))
            {
                MessageDialog.ShowPromptMessage("操作成功！");

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (strDJZT == "等待主管审核")
                    {
                        m_msgPromulgator.PassFlowMessage(dt.Rows[i]["单据号"].ToString(),
                            string.Format("{0}号营销要货计划部门主管已审核，请等待分管领导批准",
                            dt.Rows[i]["单据号"].ToString()),
                            CE_RoleEnum.营销分管领导.ToString(), true);
                    }
                    else
                    {
                        List<string> strList = new List<string>();

                        strList.Add(CE_RoleEnum.营销负责人.ToString());
                        strList.Add(CE_RoleEnum.采购负责人.ToString());
                        strList.Add(CE_RoleEnum.制造负责人.ToString());

                        m_msgPromulgator.PassFlowMessage(dt.Rows[i]["单据号"].ToString(),
                            string.Format("{0}号营销要货计划分管领导已批准，请各相关部门完成此营销要货计划的相关工作",
                            dt.Rows[i]["单据号"].ToString()), BillFlowMessage_ReceivedUserType.角色, strList);
                    }
                }
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            RefreshDataGirdView(m_serverMkPlan.GetAllBill());
        }

        /// <summary>
        /// 检查数据
        /// </summary>
        /// <returns>通过返回True，否则False</returns>
        private bool CheckDate()
        {
            if (txtDJH.Text == "")
            {
                MessageDialog.ShowErrorMessage("单据号为空，请新建单据！");
                return false;
            }

            if (cmbBillType.Text == "")
            {
                MessageDialog.ShowErrorMessage("单据类型为空，请选择单据类型");
                return false;
            }

            if (lbUpLoadFile.Tag == null || lbUpLoadFile.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请【上传附件】");
                return false;
            }

            return true;
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "新建单据")
            {
                if (!CheckDate())
                {
                    return;
                }

                GetMessage();
                m_lnqMkBill.DJH = txtDJH.Text;

                if (m_serverMkPlan.AddBill(m_lnqMkBill, m_dtMx, out m_err))
                {
                    MessageDialog.ShowPromptMessage("提交成功！");

                    m_msgPromulgator.DestroyMessage(txtDJH.Text);
                    List<string> strList = new List<string>();

                    strList.Add(CE_RoleEnum.营销负责人.ToString());
                    strList.Add(CE_RoleEnum.采购负责人.ToString());
                    strList.Add(CE_RoleEnum.制造负责人.ToString());
                    strList.Add(CE_RoleEnum.生产管理部人员.ToString());

                    m_msgPromulgator.SendNewFlowMessage(m_lnqMkBill.DJH,
                        string.Format("【单据年月】：{0} 【第一月总数】：{1} 【第二月总数】：{2} 【第三月总数】：{3} ※※※ 请【生产管理部人员】处理",
                        m_lnqMkBill.YearAndMonth, m_lnqMkBill.FirstMonthSumCount, m_lnqMkBill.SecondMonthSumCount, m_lnqMkBill.ThirdMonthSumCount),
                        BillFlowMessage_ReceivedUserType.角色, strList);
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                RefreshDataGirdView(m_serverMkPlan.GetAllBill());
                PositioningRecord(m_lnqMkBill.DJH);
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
                PositioningRecord(m_lnqMkBill.DJH);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["编制人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成"
                || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已被变更")
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您是否要删除此单据") == DialogResult.No)
            {
                return;
            }

            if (m_serverMkPlan.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_err))
            {
                string[] pathArray = lbUpLoadFile.Tag.ToString().Split(',');

                for (int i = 0; i < pathArray.Length; i++)
                {
                    UniversalControlLibrary.FileOperationService.File_Delete(new Guid(pathArray[i]),
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                }

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
            RefreshDataGirdView(m_serverMkPlan.GetAllBill());
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

        private void 批准通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lbDJZT.Text == "等待领导批准")
            {
                UpdateProductBill("等待领导批准");
                PositioningRecord(m_lnqMkBill.DJH);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请重新确认单据状态");
            }
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverMkPlan.GetAllBill());
            PositioningRecord(m_lnqMkBill.DJH);
        }

        private void 打印ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已完成"
                && dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "单据已被变更")
            {
                MessageDialog.ShowPromptMessage("请选择已确认的记录后再进行此操作");
                return;
            }

            IBillReportInfo report = new 报表_营销计划(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), labelTitle.Text);
            PrintReportBill print = new PrintReportBill(29.7, 21, report);
            print.DirectPrintReport();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView2.DataSource == null ? m_serverMkPlan.GetList("")
                : (DataTable)dataGridView2.DataSource;
            DataRow dr = dt.NewRow();

            dr["图号型号"] = tbsCode.Text;
            dr["物品ID"] = Convert.ToInt32(tbsCode.Tag);
            dr["物品名称"] = txtName.Text;
            dr["规格"] = txtSpec.Text;
            dr["厂商编码"] = txtCSCode.Text;
            dr["第一个月计划数"] = numFirstMonthCount.Value.ToString();
            dr["第二个月计划数"] = numSecondMonthCount.Value.ToString();
            dr["第三个月计划数"] = numThirdMonthCount.Value.ToString();
            dr["备注"] = txtListRemark.Text;
            dr["单位"] = lbdw1.Text;
            dr["单位"] = lbdw2.Text;
            dr["单位"] = lbdw3.Text;
            dr["单据号"] = txtDJH.Text;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["物品ID"].ToString() == dr["物品ID"].ToString())
                {
                    MessageDialog.ShowErrorMessage("不能录入同种物品！");
                    return;
                }
            }

            dt.Rows.Add(dr);
            dataGridView2.DataSource = dt;
            SetToolTipText();
            txtFirstMonthSumCont.Text = dt.Compute("Sum(第一个月计划数)", "物品名称 <> 'TCU总成'").ToString();
            txtSecondMonthSumCount.Text = dt.Compute("Sum(第二个月计划数)", "物品名称 <> 'TCU总成'").ToString();
            txtThirdMonthSumCount.Text = dt.Compute("Sum(第三个月计划数)", "物品名称 <> 'TCU总成'").ToString();
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
            SetToolTipText();
            txtFirstMonthSumCont.Text = dt.Compute("Sum(第一个月计划数)", "物品名称 <> 'TCU总成'").ToString();
            txtSecondMonthSumCount.Text = dt.Compute("Sum(第二个月计划数)", "物品名称 <> 'TCU总成'").ToString();
            txtThirdMonthSumCount.Text = dt.Compute("Sum(第三个月计划数)", "物品名称 <> 'TCU总成'").ToString();
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (UniversalFunction.StringIsDecimal(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString()))
            {
                if (e.ColumnIndex >= 3 && e.ColumnIndex <= 5)
                {
                    营销要货计划交货期设置 form = new 营销要货计划交货期设置(txtDJH.Text, Convert.ToInt32(tbsCode.Tag),
                        e.ColumnIndex - 2, Convert.ToDecimal(dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex].Value),
                        dataGridView2.Columns[e.ColumnIndex].HeaderText, cmbYear.Text + "年" + cmbMonth.Text + "月", lbDJZT.Text);

                    form.ShowDialog();

                    SetToolTipText();
                }
            }
        }

        private void cmbMonth_TextChanged(object sender, EventArgs e)
        {
            if (UniversalFunction.StringIsDecimal(cmbMonth.Text))
            {
                dataGridView2.Columns[4].HeaderText = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 0).ToString() + "月计划数";
                dataGridView2.Columns[5].HeaderText = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 1).ToString() + "月计划数";
                dataGridView2.Columns[6].HeaderText = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 2).ToString() + "月计划数";

                label12.Text = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 0).ToString() + "月计划总数";
                label13.Text = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 1).ToString() + "月计划总数";
                label16.Text = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 2).ToString() + "月计划总数";

                label7.Text = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 0).ToString() + "月计划数";
                label11.Text = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 1).ToString() + "月计划数";
                label15.Text = UniversalFunction.GetPlanMonth(Convert.ToInt32(cmbMonth.Text), 2).ToString() + "月计划数";
            }
        }

        private void lbUpLoadFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel lbLink = (LinkLabel)sender;
            try
            {
                string strFilePath = "";

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    if (lbLink.Tag != null && lbLink.ToString().Length > 0)
                    {
                        foreach (string fileItem in lbLink.Tag.ToString().Split(','))
                        {
                            UniversalControlLibrary.FileOperationService.File_Delete(new Guid(fileItem),
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        }
                    }

                    foreach (string filePath in openFileDialog1.FileNames)
                    {
                        Guid guid = Guid.NewGuid();
                        FileOperationService.File_UpLoad(guid, filePath,
                                GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
                        strFilePath += guid.ToString() + ",";
                    }

                    lbLink.Tag = strFilePath.Substring(0, strFilePath.Length - 1);
                    lbLookFile.Visible = true;
                    m_serverMkPlan.UpdateFilePath(txtDJH.Text, lbLink.Tag.ToString());
                    MessageDialog.ShowPromptMessage("上传成功");
                    lbLink.Visible = true;
                }
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
            }
        }

        private void lbLookFile_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (lbUpLoadFile.Tag == null || lbUpLoadFile.Tag.ToString().Length == 0)
            {
                MessageDialog.ShowPromptMessage("无附件查看");
                return;
            }

            string[] tempArray = lbUpLoadFile.Tag.ToString().Split(',');

            for (int i = 0; i < tempArray.Length; i++)
            {
                FileOperationService.File_Look(new Guid(tempArray[i]),
                        GlobalObject.GeneralFunction.StringConvertToEnum<CE_CommunicationMode>(BasicInfo.BaseSwitchInfo[(int)GlobalObject.CE_SwitchName.文件传输方式]));
            }
        }
    }
}
