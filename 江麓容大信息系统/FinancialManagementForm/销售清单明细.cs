using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using Service_Peripheral_HR;
using UniversalControlLibrary;
using Service_Economic_Financial;

namespace Form_Economic_Financial
{
    /// <summary>
    /// 销售清单明细
    /// </summary>
    public partial class 销售清单明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 营销出库服务类
        /// </summary>
        ISellIn m_findSellIn = ServerModule.ServerModuleFactory.GetServerModule<ISellIn>();

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerArchiveServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 销售清单服务类
        /// </summary>
        IMarketingPartBillServer m_marketPartBillServer = Service_Economic_Financial.ServerModuleFactory.GetServerModule<IMarketingPartBillServer>();

        /// <summary>
        /// 客户信息服务类
        /// </summary>
        IClientServer m_clientServer = ServerModule.ServerModuleFactory.GetServerModule<IClientServer>();

        /// <summary>
        /// 单据明细list数据集
        /// </summary>
        List<View_S_MarketintPartList> m_listMarketPart = new List<View_S_MarketintPartList>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 最低定价服务组件
        /// </summary>
        ILowestSellPriceServer m_LowestPriceServer = ServerModule.ServerModuleFactory.GetServerModule<ILowestSellPriceServer>();

        public 销售清单明细(PlatformManagement.AuthorityFlag authorityFlag,string billNo)
        {
            InitializeComponent();

            m_authFlag = authorityFlag;
            m_billNo = billNo;
            m_billMessageServer.BillType = "销售清单";

            DataTable dtMain = m_marketPartBillServer.GetDataByBillNo(m_billNo);

            if (dtMain != null)
            {
                txtBillNo.Text = m_billNo;
                txtClient.Text =dtMain.Rows[0]["客户名称"].ToString();
                txtClient.Tag = dtMain.Rows[0]["ClientID"].ToString();
                txtAssociated.Text = dtMain.Rows[0]["营销出库单号"].ToString();
                txtRecorder.Text = dtMain.Rows[0]["销售人员"].ToString();
                txtRecordTime.Text = dtMain.Rows[0]["销售时间"].ToString();
                txtMainRemark.Text = dtMain.Rows[0]["备注"].ToString();
                txtStatus.Text = dtMain.Rows[0]["单据状态"].ToString();                
                cbCarLoad.Checked = Convert.ToBoolean(dtMain.Rows[0]["是否套用整车厂"].ToString());

                if (dtMain.Rows[0]["总金额"].ToString() == "")
                {
                    numAmount.Value = 0;
                }
                else
                {
                    numAmount.Value = Convert.ToDecimal(dtMain.Rows[0]["总金额"].ToString());
                }

                if (dtMain.Rows[0]["价格套用的整车厂"] != null)
                {
                    tbsUseClient.Text = dtMain.Rows[0]["价格套用的整车厂"].ToString();
                    tbsUseClient.Tag = dtMain.Rows[0]["价格套用的整车厂编码"].ToString();
                }

                if (dtMain.Rows[0]["营销审核时间"] != null)
                {
                    txtYXAuditTime.Text = dtMain.Rows[0]["营销审核时间"].ToString();
                }
                else
                {
                    txtYXAuditTime.Text = ServerTime.Time.ToString();
                }

                if (dtMain.Rows[0]["营销审核人"] != null)
                {
                    txtAuditor.Text = dtMain.Rows[0]["营销审核人"].ToString();
                }
                else
                {
                    txtCWAuditor.Text = BasicInfo.LoginName;
                }

                if (dtMain.Rows[0]["审核时间"] != null)
                {
                    txtCWAuditTime.Text = dtMain.Rows[0]["审核时间"].ToString();
                }
                else
                {
                    txtCWAuditTime.Text = ServerTime.Time.ToString();
                }

                if (dtMain.Rows[0]["财务审核人"] != null)
                {
                    txtCWAuditor.Text = dtMain.Rows[0]["财务审核人"].ToString();
                }
                else
                {
                    txtCWAuditor.Text = BasicInfo.LoginName;
                }
            }

            RefreshControl();
            txtClient.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(txtClient_OnCompleteSearch);
            tbsOutCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(tbsOutCode_OnCompleteSearch);

            if (txtStatus.Text == "等待销售人员确认")
            {
                tbsOutCode.Enabled = true;
            }
            else
            {
                tbsOutCode.Enabled = false;
            }
        }

        private void 销售清单明细_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, m_authFlag);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            m_listMarketPart = m_marketPartBillServer.GetListDataByBillNo(m_billNo);
            dataGridView1.DataSource = m_listMarketPart;

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["单据号"].Visible = false;
            dataGridView1.Columns["GoodsID"].Visible = false;
            dataGridView1.Columns["供应商"].Visible = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (numSellUnitPrice.Value < Convert.ToDecimal(txtLowestPrice.Text))
            {
                MessageDialog.ShowPromptMessage("销售单价小于最低定价,请重新填写销售价格！");
                return;
            }

            if (tbsOutCode.Text.Trim() == "" && txtOutName.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择主机厂的零件代码！");
                return;
            }

            for (int i = 0; i < m_listMarketPart.Count; i++)
            {
                if (m_listMarketPart[i].GoodsID == txtGoodsCode.Tag.ToString() 
                    && m_listMarketPart[i].批次号 == txtBatchNo.Text
                    && m_listMarketPart[i].供应商 == txtprovider.Text)
                {
                    m_listMarketPart[i].备注 = m_listMarketPart[i].备注 + " " + txtRemark.Text;
                    m_listMarketPart[i].销售单价 = numSellUnitPrice.Value;
                    m_listMarketPart[i].主机厂代码 = tbsOutCode.Text;
                    m_listMarketPart[i].主机厂物品名称 = txtOutName.Text;
                    m_listMarketPart[i].最低定价 = Convert.ToDecimal(txtLowestPrice.Text);
                    m_listMarketPart[i].供应商 = txtprovider.Text;

                    break;
                }
            }

            dataGridView1.DataSource = m_listMarketPart;
            dataGridView1.Refresh();

            dataGridView1.Columns["GoodsID"].Visible = false;
            dataGridView1.Columns["供应商"].Visible = false;
            dataGridView1.Columns["单据号"].Visible = false;

            ClearDate();
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
            txtLowestPrice.Text = "0";
            lbUnit.Text = "";
            txtGoodsCode.Text = "";
            txtGoodsCode.Tag = -1;
            txtGoodsName.Text = "";
            numSellUnitPrice.Value = 0;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            txtBatchNo.Text = dataGridView1.CurrentRow.Cells["批次号"].Value.ToString();
            txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
            txtGoodsCode.Tag = dataGridView1.CurrentRow.Cells["GoodsID"].Value.ToString();
            txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
            txtLowestPrice.Text = dataGridView1.CurrentRow.Cells["最低定价"].Value.ToString();
            txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
            txtSpce.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
            numCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
            numSellUnitPrice.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["销售单价"].Value);
            lbUnit.Text = dataGridView1.CurrentRow.Cells["单位"].Value.ToString();
            tbsOutCode.Text = dataGridView1.CurrentRow.Cells["主机厂代码"].Value.ToString();
            txtOutName.Text = dataGridView1.CurrentRow.Cells["主机厂物品名称"].Value.ToString();
            txtprovider.Text = dataGridView1.CurrentRow.Cells["供应商"].Value.ToString();

            try
            {
                if (cbCarLoad.Checked && tbsUseClient.Text != "")
                {
                    txtLowestPrice.Text = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                    Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).Price.ToString();
                    numSellUnitPrice.Value = Convert.ToDecimal(txtLowestPrice.Text);

                    txtOutName.Text = m_LowestPriceServer.GetCommunicateInfo(m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                    Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).CommunicateID.ToString(),out m_error).CommunicateGoodsName;

                    tbsOutCode.Text = m_LowestPriceServer.GetCommunicateInfo(m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                    Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).CommunicateID.ToString(), out m_error).CommunicateGoodsCode;
                }
                else if (tbsUseClient.Text != "" && !cbCarLoad.Checked)
                {
                    txtLowestPrice.Text = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                    Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).TerminalPrice.ToString();
                }
            }
            catch (Exception)
            {
                MessageDialog.ShowPromptMessage("最低定价中没有该零件！");

                tbsUseClient.Text = "";
                tbsUseClient.Tag = "";
                cbCarLoad.Checked = false;

                return;
            }            
        }

        private void 销售toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtRecorder.Text == BasicInfo.LoginName
                && m_marketPartBillServer.GetDataByBillNo(m_billNo).Rows[0]["单据状态"].ToString() != "已完成")
            {
                if (txtClient.Text.Trim() != "" && txtClient.Text.Trim() != "全部" && txtClient.Text.Trim() != "其它")
                {
                    S_MarketingPartBill bill = new S_MarketingPartBill();

                    bill.ClientID = txtClient.Tag.ToString();
                    bill.Remark = txtMainRemark.Text;
                    bill.Recorder = BasicInfo.LoginID;
                    bill.RecordTime = ServerTime.Time;
                    bill.Status = "等待主管审核";
                    bill.BillNo = m_billNo;
                    bill.AssociatedBillNo = txtAssociated.Text;

                    if (cbCarLoad.Checked)
                    {
                        bill.IsCarLoad = true;

                        if (tbsUseClient.Text.Trim() == "")
                        {
                            MessageDialog.ShowPromptMessage("请选择套用哪个整车厂的价格！");
                            return;
                        }
                        else
                        {
                            bill.CiteTerminalClient = tbsUseClient.Tag.ToString();

                            for (int i = 0; i < m_listMarketPart.Count; i++)
                            {
                                YX_LowestMarketPrice lowest = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(), Convert.ToInt32(m_listMarketPart[i].GoodsID), out m_error);

                                if (lowest != null)
                                {
                                    m_listMarketPart[i].最低定价 = lowest.Price;
                                    m_listMarketPart[i].销售单价 = lowest.Price;
                                }
                                else
                                {
                                    MessageDialog.ShowPromptMessage("请在最低定价表中设置该整车厂【"
                                             + m_listMarketPart[i].图号型号 + m_listMarketPart[i].物品名称 + "】的最低价格！");
                                    销售toolStripButton.Enabled = false;
                                    cbCarLoad.Checked = false;
                                    tbsUseClient.Text = "";
                                    tbsUseClient.Tag = "";

                                    return;
                                }
                            }
                        }
                    }
                    else
                    {
                        bill.IsCarLoad = false;

                        //什么意思 没搞明白？ by cjb on 2015.10.19
                        if (Convert.ToDecimal(txtLowestPrice.Text) > numSellUnitPrice.Value)
                        {
                            MessageDialog.ShowPromptMessage("请重新填写销售价格！");
                            return;
                        }
                    }

                    if (!m_marketPartBillServer.UpdateData(bill, m_listMarketPart, "销售", out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("确认成功！");

                        if (BasicInfo.DeptCode.Contains("YX"))
                        {
                            m_billMessageServer.PassFlowMessage(m_billNo,
                                string.Format("【客户名称】：{0}    ※※※ 等待【主管】处理", txtClient.Text), BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetDeptDirectorRoleName(BasicInfo.DeptCode).ToList());
                        }
                        else
                        {
                            m_billMessageServer.PassFlowMessage(m_billNo,
                                string.Format("【客户名称】：{0}    ※※※ 等待【负责人】处理", txtClient.Text), BillFlowMessage_ReceivedUserType.角色,
                                m_billMessageServer.GetDeptPrincipalRoleName(BasicInfo.DeptCode).ToList());
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("客户不正确，请重新选择！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("您不是销售人员【" + txtRecorder.Text + "】或单据已完成，不能进行此操作！");
                return;
            }

            this.Close();
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
        }

        private void 财务toolStripButton_Click(object sender, EventArgs e)
        {
            if (m_marketPartBillServer.GetDataByBillNo(m_billNo).Rows[0]["单据状态"].ToString() == "等待财务审核"
                || m_marketPartBillServer.GetDataByBillNo(m_billNo).Rows[0]["单据状态"].ToString() == "已完成")
            {
                if (txtClient.Text.Trim() != "" && txtClient.Text.Trim() != "全部" && txtClient.Text.Trim() != "其它")
                {
                    if (Convert.ToDecimal(txtLowestPrice.Text) > numSellUnitPrice.Value)
                    {
                        MessageDialog.ShowPromptMessage("请重新填写销售价格！");
                        return;
                    }

                    for (int i = 0; i < m_listMarketPart.Count; i++)
                    {
                        for (int j = 0; j < dataGridView1.Rows.Count; j++)
                        {
                            if (m_listMarketPart[i].GoodsID == dataGridView1.Rows[j].Cells["GoodsID"].Value.ToString()
                                && m_listMarketPart[i].批次号 == dataGridView1.Rows[j].Cells["批次号"].Value.ToString()
                                && m_listMarketPart[i].供应商 == dataGridView1.Rows[j].Cells["供应商"].Value.ToString())
                            {
                                m_listMarketPart[i].备注 = m_listMarketPart[i].备注 + " " + txtRemark.Text;
                                m_listMarketPart[i].销售单价 = numSellUnitPrice.Value;
                                m_listMarketPart[i].主机厂代码 = tbsOutCode.Text;
                                m_listMarketPart[i].主机厂物品名称 = txtOutName.Text;
                                m_listMarketPart[i].最低定价 = Convert.ToDecimal(txtLowestPrice.Text);
                                m_listMarketPart[i].供应商 = txtprovider.Text.Trim();
                            }
                        }
                    }

                    S_MarketingPartBill bill = new S_MarketingPartBill();

                    bill.ClientID = txtClient.Tag.ToString();
                    bill.Remark = txtMainRemark.Text;
                    bill.CW_Auditor = BasicInfo.LoginID;
                    bill.CW_AuditTime = ServerTime.Time;
                    bill.Status = "已完成";
                    bill.BillNo = m_billNo;
                    bill.AssociatedBillNo = txtAssociated.Text;

                    if (!m_marketPartBillServer.UpdateData(bill, m_listMarketPart, "财务", out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("审核成功！");
                    }

                    List<string> noticeUser = new List<string>();

                    noticeUser.Add(m_personnerArchiveServer.GetPersonnelViewInfoByName(txtRecorder.Text));

                    m_billMessageServer.EndFlowMessage(m_billNo,
                            string.Format("{0} 号销售清单已经处理完毕", m_billNo),
                            null, noticeUser);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("客户不正确，请重新选择！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }

            this.Close();
        }

        private void 营销主管toolStripButton_Click(object sender, EventArgs e)
        {
            if (m_marketPartBillServer.GetDataByBillNo(m_billNo).Rows[0]["单据状态"].ToString() == "等待主管审核")
            {
                if (txtClient.Text.Trim() != "" && txtClient.Text.Trim() != "全部" && txtClient.Text.Trim() != "其它")
                {
                    S_MarketingPartBill bill = new S_MarketingPartBill();

                    bill.Status = "等待财务审核";
                    bill.BillNo = m_billNo;
                    bill.AssociatedBillNo = txtAssociated.Text;
                    bill.YX_Auditor = BasicInfo.LoginID;
                    bill.YX_AuditTime = ServerTime.Time;                   

                    if (!m_marketPartBillServer.UpdateData(bill, m_listMarketPart, "销售主管", out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("确认成功！");

                        m_billMessageServer.PassFlowMessage(m_billNo,
                            string.Format("【客户名称】：{0}    ※※※ 等待【会计】处理", txtClient.Text), BillFlowMessage_ReceivedUserType.角色,
                            CE_RoleEnum.会计.ToString());

                        this.Close();
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("客户不正确，请重新选择！");
                    return;
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
                return;
            }
        }

        private void 回退toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text.Trim() != "已完成")
            {
                回退单据 form = new 回退单据(CE_BillTypeEnum.销售清单, m_billNo, txtStatus.Text);

                if (form.ShowDialog() == DialogResult.OK)
                {
                    if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        if (m_marketPartBillServer.ReturnBill(form.StrBillID,
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
                if (BasicInfo.DeptCode == "CW")
                {
                    回退单据 form = new 回退单据(CE_BillTypeEnum.销售清单, m_billNo, txtStatus.Text);

                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (m_marketPartBillServer.ReturnBill(form.StrBillID,
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
                    MessageDialog.ShowPromptMessage("请确认单据状态！");
                }
            }
        }

        private void tbsUseClient_OnCompleteSearch()
        {
            tbsUseClient.Text = tbsUseClient.DataResult["主机厂"].ToString();
            tbsUseClient.Tag = tbsUseClient.DataResult["客户编码"].ToString();

            try
            {
                if (cbCarLoad.Checked)
                {
                    for (int i = 0; i < m_listMarketPart.Count; i++)
                    {
                        YX_LowestMarketPrice lowest = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                            Convert.ToInt32(m_listMarketPart[i].GoodsID), out m_error);

                        if (lowest != null)
                        {
                            m_listMarketPart[i].最低定价 = lowest.Price;
                            m_listMarketPart[i].销售单价 = lowest.Price;
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("请在最低定价表中设置该整车厂【" 
                                + m_listMarketPart[i].图号型号 + m_listMarketPart[i].物品名称+"】的最低价格！");
                            cbCarLoad.Checked = false;
                            tbsUseClient.Text = "";
                            tbsUseClient.Tag = "";

                            销售toolStripButton.Enabled = false;

                            return;
                        }
                    }

                    txtLowestPrice.Text = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                    Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).Price.ToString();
                }
                else
                {
                    txtLowestPrice.Text = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                    Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).TerminalPrice.ToString();

                    for (int i = 0; i < m_listMarketPart.Count; i++)
                    {
                        YX_LowestMarketPrice lowest = m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                            Convert.ToInt32(m_listMarketPart[i].GoodsID), out m_error);

                        if (lowest != null)
                        {
                            m_listMarketPart[i].最低定价 = lowest.Price;
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("请在最低定价表中设置该整车厂【" 
                                + m_listMarketPart[i].图号型号 + m_listMarketPart[i].物品名称 + "】的最低价格！");
                            cbCarLoad.Checked = false;
                            tbsUseClient.Text = "";
                            tbsUseClient.Tag = "";

                            销售toolStripButton.Enabled = false;
                            return;
                        }
                    }
                }

                txtOutName.Text = m_LowestPriceServer.GetCommunicateInfo(m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                   Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).CommunicateID.ToString(), out m_error).CommunicateGoodsName;
                tbsOutCode.Text = m_LowestPriceServer.GetCommunicateInfo(m_LowestPriceServer.GetDataByClientCode(tbsUseClient.Tag.ToString(),
                  Convert.ToInt32(txtGoodsCode.Tag.ToString()), out m_error).CommunicateID.ToString(), out m_error).CommunicateGoodsCode;

                dataGridView1.DataSource = m_listMarketPart;
                dataGridView1.Refresh();
            }
            catch (Exception)
            {
                MessageDialog.ShowPromptMessage("最低定价中没有该零件！");

                tbsUseClient.Text = "";
                tbsUseClient.Tag = "";
                cbCarLoad.Checked = false;

                return;
            }
        }

        private void tbsOutCode_OnCompleteSearch()
        {
            tbsOutCode.Text = tbsOutCode.DataResult["主机厂图号型号"].ToString();
            txtOutName.Text = tbsOutCode.DataResult["主机厂物品名称"].ToString();
            txtLowestPrice.Text = tbsOutCode.DataResult["最低定价"].ToString();
            numSellUnitPrice.Value = Convert.ToDecimal(txtLowestPrice.Text);
        }

        private void cbCarLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (cbCarLoad.Checked)
            {
                tbsOutCode.Enabled = false;
            }
            else
            {
                tbsOutCode.Enabled = true;
            }
        }

        private void numSellUnitPrice_ValueChanged(object sender, EventArgs e)
        {
            //if (numSellUnitPrice.Value < Convert.ToDecimal(txtLowestPrice.Text))
            //{
            //    MessageDialog.ShowPromptMessage("销售单价不能低于最低价格！");
            //    numSellUnitPrice.Value = Convert.ToDecimal(txtLowestPrice.Text);
            //    return;
            //}
        }

        private void numSellUnitPrice_Leave(object sender, EventArgs e)
        {
            if (numSellUnitPrice.Value < Convert.ToDecimal(txtLowestPrice.Text))
            {
                MessageDialog.ShowPromptMessage("销售单价不能低于最低价格！");
                numSellUnitPrice.Value = Convert.ToDecimal(txtLowestPrice.Text);
                return;
            }
        }
    }
}
