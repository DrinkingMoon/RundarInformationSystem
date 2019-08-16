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
    /// 工装验证报告明细界面
    /// </summary>
    public partial class 工装验证报告 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 基础物品服务组件
        /// </summary>
        IBasicGoodsServer m_serverBasicGoods = ServerModuleFactory.GetServerModule<IBasicGoodsServer>();

        /// <summary>
        /// 数据集
        /// </summary>
        private S_FrockProvingReport m_lnqFrock;

        public S_FrockProvingReport LnqFrock
        {
            get { return m_lnqFrock; }
            set { m_lnqFrock = value; }
        }

        /// <summary>
        /// 验证和检验内容表
        /// </summary>
        private DataTable m_dtAttached;

        public DataTable DtAttached
        {
            get { return m_dtAttached; }
            set { m_dtAttached = value; }
        }

        /// <summary>
        /// 工装台帐服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IFrockProvingReport m_serverFrock = ServerModuleFactory.GetServerModule<IFrockProvingReport>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        public 工装验证报告(string djh)
        {
            InitializeComponent();

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.工装验证报告单, m_serverFrock);

            if (djh == "")
            {
                lblBillNo.Text = m_billNoControl.GetNewBillNo();
                lblBillStatus.Text = "新建单据";
                chbIsInStock.Visible = false;
            }
            else
            {
                lblBillNo.Text = djh;
                cmbBillType.Enabled = false;
            }

            m_lnqFrock = m_serverFrock.GetBill(lblBillNo.Text);
            dataGridView1.DataSource = m_serverFrock.GetAttachedTable(lblBillNo.Text, "检验");
            dataGridView2.DataSource = m_serverFrock.GetAttachedTable(lblBillNo.Text, "验证");

            ShowMessage();
            ControlWidget(lblBillStatus.Text);

            if (cmbBillType.Text != "入库检验")
            {
                chbIsInStock.Visible = false;
            }

            if (m_lnqFrock != null)
            {
                DataRow drInfo = m_serverFrockStandingBook.GetInDepotBillInfo(m_lnqFrock.FrockNumber);

                if (drInfo != null)
                {
                    txtProposer.Text = drInfo["申请人"].ToString();
                    txtProposer.Tag = drInfo["申请人工号"].ToString();
                    txtProviderCode.Text = drInfo["供应商编码"].ToString();
                    txtProviderName.Text = drInfo["供应商简称"].ToString();
                    txtDesigner.Text = drInfo["设计人"].ToString();
                    txtDesigner.Tag = drInfo["设计人工号"].ToString();
                    txtConnectNumber.Text = drInfo["关联单号"].ToString();
                }
            }

            if (lblBillStatus.Text == "单据已完成")
            {
                toolStrip1.Visible = false;
            }

        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"].ToString());
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtCode.Tag = Convert.ToInt32(txtName.DataResult["序号"].ToString());
        }

        /// <summary>
        /// 检测数据是否完整
        /// </summary>
        /// <param name="billstatus">单据状态</param>
        /// <returns>检测通过返回True，否则返回False</returns>
        bool CheckMessage(string billstatus)
        {
            switch (billstatus)
            {
                case "新建单据":

                    if (txtCode.Tag.ToString() == "" || txtCode.Tag.ToString() == "0" || txtName.Text.Trim() == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择需要检验的工装");
                        return false;
                    }

                    if (txtFrockNumber.Text == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择工装的编码");
                        return false;
                    }

                    if (((DataTable)dataGridView1.DataSource).Rows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("请添加检验要求");
                        return false;
                    }

                    if (cmbBillType.Text == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择报告类别");
                        return false;
                    }

                    break;
                case "等待检验要求":

                    if (((DataTable)dataGridView1.DataSource).Rows.Count == 0)
                    {
                        MessageDialog.ShowPromptMessage("请添加检验要求");
                        return false;
                    }

                    break;

                case "等待检验":

                    foreach (DataGridViewRow item in dataGridView1.Rows)
                    {
                        if (item.Cells["检验内容"].Value.ToString().Trim() == "")
                        {
                            MessageDialog.ShowPromptMessage("请完整填写检验内容");
                            return false;
                        }
                    }

                    //if (txtExamineVerdict.Text.Trim() == "")
                    //{
                    //    MessageDialog.ShowPromptMessage("请填写检验说明");
                    //    return false;
                    //}

                    break;

                    //可直接提交结论，无需验证，王永红 Modify by cjb on 2014.11.11
                //case "等待验证要求":

                //    if (txtFinalVerdict.Text.Trim() == "")
                //    {
                //        if (((DataTable)dataGridView2.DataSource).Rows.Count == 0)
                //        {
                //            MessageDialog.ShowPromptMessage("请添加验证要求");
                //            return false;
                //        }
                //    }

                //    break;

                //case "等待验证":

                //    foreach (DataGridViewRow item in dataGridView2.Rows)
                //    {
                //        if (item.Cells["验证内容"].Value.ToString().Trim() == "")
                //        {
                //            MessageDialog.ShowPromptMessage("请完整填写检验内容");
                //            return false;
                //        }
                //    }

                //    if (txtProvingVerdict.Text.Trim() == "")
                //    {
                //        MessageDialog.ShowPromptMessage("请填写验证结论");
                //        return false;
                //    }

                //    break;
                case "等待结论":

                    if (!rbRK.Checked && !rbTH.Checked)
                    {
                        MessageDialog.ShowPromptMessage("请选择【入库】或者【退货】");
                        return false;
                    }

                    //if (!chbIsInStock.Checked)
                    //{
                    //    if (MessageDialog.ShowEnquiryMessage("请注意：【入库且能正常使用】选项未被勾选，系统将判定【此工装】不合格并且不予入库，是否继续？") == DialogResult.No)
                    //    {
                    //        return false;
                    //    }
                    //}

                    break;
                default:

                    break;
            }

            return true;
        }

        /// <summary>
        /// 控制控件
        /// </summary>
        /// <param name="billstatus">单据状态</param>
        void ControlWidget(string billstatus)
        {
            if (billstatus == "新建单据")
            {
                btExamineAdd.Enabled = true;
                btExamineDelete.Enabled = true;
            }
            else
            {
                txtName.ShowResultForm = false;
                txtFrockNumber.ShowResultForm = false;

                switch (billstatus)
                {
                    case "等待检验要求":
                        btExamineAdd.Enabled = true;
                        btExamineDelete.Enabled = true;
                        break;
                    case "等待检验":
                        btExamineUpdate.Enabled = true;
                        break;
                    case "等待验证要求":
                        btProvingAdd.Enabled = true;
                        btProvingDelete.Enabled = true;
                        break;
                    case "等待验证":
                        btProvingUpdate.Enabled = true;
                        break;
                    default:
                        break;
                }
            }
        }

        /// <summary>
        /// 显示所有信息
        /// </summary>
        void ShowMessage()
        {
            if (m_lnqFrock != null)
            {
                View_F_GoodsPlanCost tempGoodsLnq = UniversalFunction.GetGoodsInfo(m_lnqFrock.GoodsID);

                if (tempGoodsLnq == null)
                {
                    MessageDialog.ShowPromptMessage("找不到此物品信息或者此物品信息错误");
                    return;
                }

                txtCode.Text = tempGoodsLnq.图号型号;
                txtCode.Tag = m_lnqFrock.GoodsID;
                txtName.Text = tempGoodsLnq.物品名称;
                txtFrockNumber.Text = m_lnqFrock.FrockNumber;
                txtConnectNumber.Text = m_lnqFrock.ConnectBillNumber;
                //txtExamineVerdict.Text = m_lnqFrock.ExamineVerdict;
                txtFinalVerdict.Text = m_lnqFrock.FinalVerdict;
                //txtProvingVerdict.Text = m_lnqFrock.ProvingVerdict;
                cmbBillType.Text = m_lnqFrock.BillType;

                if (m_lnqFrock.IsInStock != null)
                {
                    if ((bool)m_lnqFrock.IsInStock)
                    {
                        rbRK.Checked = true;
                        rbTH.Checked = false;
                    }
                    else
                    {
                        rbRK.Checked = false;
                        rbTH.Checked = true;
                    }
                }
                else
                {
                    rbRK.Checked = false;
                    rbTH.Checked = false;
                }

                //if (m_lnqFrock.IsInStock != null)
                //{
                //    chbIsInStock.Checked = (bool)m_lnqFrock.IsInStock;
                //}

                lbGYRY.Text = m_lnqFrock.GYRY;
                lbGYRQ.Text = m_lnqFrock.GYRQ.ToString();
                lbJYRY.Text = m_lnqFrock.JYRY;
                lbJYRQ.Text = m_lnqFrock.JYRQ.ToString();
                lbYZRY.Text = m_lnqFrock.YZRY;
                lbYZRQ.Text = m_lnqFrock.YZRQ.ToString();

                lblBillTime.Text = m_lnqFrock.BZRQ.ToString();
                lblBillStatus.Text = m_lnqFrock.DJZT;
                lblBillNo.Text = m_lnqFrock.DJH;

                //chbIsExamineQualified.Checked = m_lnqFrock.IsExamineQualified == null ? false : (bool)m_lnqFrock.IsExamineQualified;
                //chbIsProvingQualified.Checked = m_lnqFrock.IsProvingQualified == null ? false : (bool)m_lnqFrock.IsProvingQualified;
            }
        }

        /// <summary>
        /// 获得所有信息
        /// </summary>
        void GetMessage()
        {
            m_lnqFrock = new S_FrockProvingReport();

            m_dtAttached = ((DataTable)dataGridView1.DataSource).Clone();

            foreach (DataGridViewRow dgvr in dataGridView1.Rows)
            {
                DataRow dr = m_dtAttached.NewRow();

                dr["AttachedType"] = dgvr.Cells["AttachedType"].Value.ToString();
                dr["AskContent"] = dgvr.Cells["检验要求"].Value.ToString();
                dr["AnswerContent"] = dgvr.Cells["检验内容"].Value.ToString();

                m_dtAttached.Rows.Add(dr);
            }

            //m_dtAttached = (DataTable)dataGridView1.DataSource;

            //DataTable dtTemp = (DataTable)dataGridView2.DataSource;

            //for (int i = 0; i < dtTemp.Rows.Count; i++)
            //{
            //    DataRow dr = m_dtAttached.NewRow();

            //    dr["AttachedType"] = dtTemp.Rows[i]["AttachedType"].ToString();
            //    dr["AskContent"] = dtTemp.Rows[i]["AskContent"].ToString();
            //    dr["AnswerContent"] = dtTemp.Rows[i]["AnswerContent"].ToString();

            //    m_dtAttached.Rows.Add(dr);
            //}

            m_lnqFrock.DJH = lblBillNo.Text;
            m_lnqFrock.DJZT = lblBillStatus.Text;
            //m_lnqFrock.ExamineVerdict = txtExamineVerdict.Text;
            m_lnqFrock.FinalVerdict = txtFinalVerdict.Text;
            m_lnqFrock.FrockNumber = txtFrockNumber.Text;
            m_lnqFrock.GoodsID = Convert.ToInt32(txtCode.Tag);
            //m_lnqFrock.IsExamineQualified = chbIsExamineQualified.Checked;
            //m_lnqFrock.IsProvingQualified = chbIsProvingQualified.Checked;
            m_lnqFrock.IsInStock = rbRK.Checked;
                //chbIsInStock.Checked;
            //m_lnqFrock.ProvingVerdict = txtProvingVerdict.Text;
            m_lnqFrock.ConnectBillNumber = txtConnectNumber.Text;
            m_lnqFrock.BillType = cmbBillType.Text;

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

        private void btExamineAdd_Click(object sender, EventArgs e)
        {
            DataTable dtTempExamine = (DataTable)dataGridView1.DataSource;
            DataRow dr = dtTempExamine.NewRow();
            dr["AskContent"] = txtExamineAsk.Text;
            dr["AttachedType"] = "检验";
            dtTempExamine.Rows.Add(dr);

            dataGridView1.DataSource = dtTempExamine;

            txtExamineAsk.Text = "";
        }

        private void btExamineDelete_Click(object sender, EventArgs e)
        {
            DataTable dtTempExamine = (DataTable)dataGridView1.DataSource;

            dtTempExamine.Rows.RemoveAt(dataGridView1.CurrentRow.Index);

            dataGridView1.DataSource = dtTempExamine;
        }

        private void btExamineUpdate_Click(object sender, EventArgs e)
        {
            DataTable dtTempExamine = (DataTable)dataGridView1.DataSource;

            dtTempExamine.Rows[dataGridView1.CurrentRow.Index]["AnswerContent"] = txtExamineAnswer.Text;

            dataGridView1.DataSource = dtTempExamine;
        }

        private void btProvingAdd_Click(object sender, EventArgs e)
        {
            DataTable dtTempProving = (DataTable)dataGridView2.DataSource;

            DataRow dr = dtTempProving.NewRow();

            dr["AskContent"] = txtProvingAsk.Text;
            dr["AttachedType"] = "验证";

            dtTempProving.Rows.Add(dr);

            dataGridView2.DataSource = dtTempProving;

            txtProvingAsk.Text = "";
        }

        private void btProvingDelete_Click(object sender, EventArgs e)
        {
            DataTable dtTempProving = (DataTable)dataGridView2.DataSource;

            dtTempProving.Rows.RemoveAt(dataGridView2.CurrentRow.Index);

            dataGridView2.DataSource = dtTempProving;
        }

        private void btProvingUpdate_Click(object sender, EventArgs e)
        {
            DataTable dtTempProving = (DataTable)dataGridView2.DataSource;

            dtTempProving.Rows[dataGridView2.CurrentRow.Index]["AnswerContent"] = txtProvingAnswer.Text;

            dataGridView2.DataSource = dtTempProving;
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow != null)
            {
                txtExamineAnswer.Text = dataGridView1.CurrentRow.Cells["检验内容"].Value.ToString();
                txtExamineAsk.Text = dataGridView1.CurrentRow.Cells["检验要求"].Value.ToString();
            }


        }

        private void dataGridView2_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow != null)
            {
                txtProvingAnswer.Text = dataGridView2.CurrentRow.Cells["验证内容"].Value.ToString();
                txtProvingAsk.Text = dataGridView2.CurrentRow.Cells["验证要求"].Value.ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!CheckMessage(lblBillStatus.Text))
            {
                return;
            }

            GetMessage();

            if (!m_serverFrock.SaveInfo(m_lnqFrock, m_dtAttached, out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
                return;
            }

            this.Close();
        }

        private void 提交_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 工装验证报告_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_billNoControl.CancelBill();
        }

        private void txtFrockNumber_Enter(object sender, EventArgs e)
        {
            txtFrockNumber.StrEndSql = " and 物品ID = " + Convert.ToInt32(txtName.Tag);
        }

        private void btnReferenceInfo_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetFrockProvingReport(txtCode.Tag == null ? 0 : Convert.ToInt32(txtCode.Tag));

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                string strbillNo = form.GetDataItem("单据号").ToString();

                if (((DataTable)dataGridView1.DataSource).Rows.Count == 0)
                {
                    DataTable dtTemp = m_serverFrock.GetAttachedTable(strbillNo, "检验");

                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        dr["AnswerContent"] = "";
                    }

                    dataGridView1.DataSource = dtTemp;
                }

                if (((DataTable)dataGridView2.DataSource).Rows.Count == 0)
                {
                    DataTable dtTemp = m_serverFrock.GetAttachedTable(strbillNo, "验证");

                    foreach (DataRow dr in dtTemp.Rows)
                    {
                        dr["AnswerContent"] = "";
                    }

                    dataGridView2.DataSource = dtTemp;
                }
            }
        }

        private void cmbBillType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lblBillStatus.Text == "新建单据" && dataGridView1.Rows.Count == 0 && txtFrockNumber.Text.Trim().Length != 0)
            {
                DataTable tempTable = m_serverFrockStandingBook.GetCheckItemsContent(txtFrockNumber.Text);

                if (tempTable != null && tempTable.Rows.Count > 0)
                {
                    DataTable tempTable1 = (DataTable)dataGridView1.DataSource;

                    foreach (DataRow dr in tempTable.Rows)
                    {
                        DataRow tempRow = tempTable1.NewRow();
                        tempRow["AskContent"] = dr["检测项目"];
                        tempRow["AttachedType"] = "检验";

                        tempTable1.Rows.Add(tempRow);
                    }

                    dataGridView1.DataSource = tempTable1;
                }
            }
        }

        private void txtFrockNumber_TextChanged(object sender, EventArgs e)
        {
            DataRow drInfo = m_serverFrockStandingBook.GetInDepotBillInfo(txtFrockNumber.Text);

            if (drInfo != null)
            {
                txtProposer.Text = drInfo["申请人"].ToString();
                txtProposer.Tag = drInfo["申请人工号"].ToString();
                txtProviderCode.Text = drInfo["供应商编码"].ToString();
                txtProviderName.Text = drInfo["供应商简称"].ToString();
                txtDesigner.Text = drInfo["设计人"].ToString();
                txtDesigner.Tag = drInfo["设计人工号"].ToString();
                txtConnectNumber.Text = drInfo["关联单号"].ToString();
            }
        }
    }
}
