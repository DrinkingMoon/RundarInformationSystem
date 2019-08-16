using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;
using ServerModule;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 销售订单零件明细 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 评审部门选中的行
        /// </summary>
        int m_dataGridViewSelectRow;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        //BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 销售合同/订单评审服务类
        /// </summary>
        ISalesOrderServer m_salesOrderServer = ServerModuleFactory.GetServerModule<ISalesOrderServer>();

        /// <summary>
        /// 部门评审数据集
        /// </summary>
        DataTable dt = new DataTable();

        /// <summary>
        /// 营销出库的零件数据集
        /// </summary>
        DataTable m_dtMxCK = new DataTable();

        /// <summary>
        /// 营销出库数据集
        /// </summary>
        DataTable m_dtCK = new DataTable();

        public 销售订单零件明细()
        {
            InitializeComponent();

            InitDepttComboBox();
            m_billMessageServer.BillType = "销售订单评审";
            txtStatus.Text = SalesOrderStatus.新建单据.ToString();
            txtApplicant.Text = BasicInfo.LoginName;
            txtApplicantDate.Text = ServerTime.Time.ToString();
            txtBillNo.Text = "系统自动生成";
            numYear.Value = ServerTime.Time.Year;
            numMonth.Value = ServerTime.Time.Month;

            dgvPartList.DataSource = m_salesOrderServer.GetPartListInfo("");

            dgvPartList.Columns["单据号"].Visible = false;
        }

        public 销售订单零件明细(string billNo)
        {
            InitializeComponent();
            m_billNo = billNo;
            InitDepttComboBox();
            InitControl(billNo);
            m_billMessageServer.BillType = "销售订单评审";

            // 单击进入编辑状态
            this.dgvReview.EditMode = DataGridViewEditMode.EditOnEnter;
            this.dgvReview.AutoGenerateColumns = false;
        }

        private void txtGoodsName_OnCompleteSearch()
        {
            txtGoodsCode.Text = txtGoodsName.DataResult["图号型号"].ToString();
            txtGoodsCode.Tag = txtGoodsName.DataResult["序号"].ToString();
            txtGoodsName.Text = txtGoodsName.DataResult["物品名称"].ToString();
            txtSpce.Text = txtGoodsName.DataResult["规格"].ToString();
            lbUnit.Text = txtGoodsName.DataResult["单位"].ToString();
        }

        private void tbsClientGoodsName_OnCompleteSearch()
        {
            tbsClientGoodsName.Text = tbsClientGoodsName.DataResult["主机厂物品名称"].ToString();
            txtClientGoodsCode.Text = tbsClientGoodsName.DataResult["主机厂图号型号"].ToString();
            txtClientGoodsCode.Tag = tbsClientGoodsName.DataResult["ID"].ToString();
        }

        /// <summary>
        /// 初始化部门下拉框
        /// </summary>
        private void InitDepttComboBox()
        {
            //string sql = @"select DeptCode, DeptName from  HR_Dept where deptCode <> '00' ORDER BY OrderID";
            //DataTable Dt = DatabaseServer.QueryInfo(sql);

            //if (Dt != null)
            //{
            //    DataGridViewComboBoxColumn dgvComboBoxColumn = dgvReview.Columns["部门"] as DataGridViewComboBoxColumn;

            //    dgvComboBoxColumn.DataPropertyName = "DeptCode";
            //    dgvComboBoxColumn.DataSource = Dt.DefaultView;
            //    dgvComboBoxColumn.DisplayMember = "DeptName";
            //    dgvComboBoxColumn.ValueMember = "DeptCode";
            //}
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        void InitControl(string billNo)
        {
            View_YX_SalesOrder salesOrder = m_salesOrderServer.GetBillInfo(billNo);

            if (salesOrder != null)
            {
                txtApplicant.Text = salesOrder.制单人;
                txtApplicant.Tag = UniversalFunction.GetPersonnelCode(salesOrder.制单人);
                txtApplicantDate.Text = salesOrder.制单时间.ToString();
                txtAuditer.Text = salesOrder.营销主管审核;
                txtAuditTime.Text = salesOrder.审核时间.ToString();
                txtBillNo.Text = salesOrder.单据号;
                txtClient.Text = salesOrder.客户名称;
                txtClient.Tag = salesOrder.客户编号;
                txtContractName.Text = salesOrder.合同_订单名称;
                txtDealRequire.Text = salesOrder.协议要求;
                txtResultDate.Text = salesOrder.结果确认时间.ToString();
                txtResultPerson.Text = salesOrder.评审结果确认人;
                txtReviewResult.Text = salesOrder.评审结果;
                txtReviewType.Text = salesOrder.评审类型;
                txtStatus.Text = salesOrder.单据状态;
                numYear.Value = salesOrder.年;
                numMonth.Value = salesOrder.月;
            }

            dgvPartList.DataSource = m_salesOrderServer.GetPartListInfo(billNo);

            dgvPartList.Columns["单据号"].Visible = false;

            IEnumerable<View_YX_SalesOrderReview> reviewView = m_salesOrderServer.GetReviewListInfo(billNo);

            if (reviewView != null && reviewView.Count() > 0)
            {
                dgvReview.Rows.Clear();
                foreach (View_YX_SalesOrderReview item in reviewView)
                {
                    dgvReview.Rows.Add(new object[] {"",item.部门编码,item.部门,item.评审意见, 
                        item.评审确认人,item.评审确认时间});
                }
            }
        }

        private void dgvReview_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dgvReview.Columns[e.ColumnIndex].ValueType == typeof(decimal))
            {
                e.Cancel = true;
            }
            else if (dgvReview.Columns[e.ColumnIndex].ValueType == typeof(int))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// 检测控件输入的正确性
        /// </summary>
        /// <returns>无误返回True，否则返回False</returns>
        bool CheckControl()
        {
            if (txtContractName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入合同/订单名称！");
                return false;
            }

            if (txtDealRequire.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入协议要求！");
                return false;
            }

            if (txtReviewType.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入评审类型！");
                return false;
            }

            if (txtClient.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择客户名称！");
                return false;
            }

            if (numYear.Value < ServerTime.Time.Year || numMonth.Value < ServerTime.Time.Month)
            {
                if (MessageDialog.ShowEnquiryMessage("您确定是" + numYear.Value + "年" + numMonth.Value + "月的订单？") == DialogResult.No)
                {
                    return false;
                }
            }

            if (dgvReview.Rows.Count > 0)
            {
                for (int i = 0; i < dgvReview.Rows.Count; i++)
                {
                    if (dgvReview.Rows[i].Cells["部门"].Value == null || dgvReview.Rows[i].Cells["部门"].Value.ToString() == "")
                    {
                        MessageDialog.ShowPromptMessage("请选择第" + i + 1 + "行的部门！");
                        return false;
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请添加评审部门！");
                return false;
            }

            if (dgvPartList.Rows.Count < 1)
            {
                MessageDialog.ShowPromptMessage("请添加零件信息！");
                return false;
            }
            return true;
        }
        
        /// <summary>
        /// 绘制数据显示控件行号
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvReview_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvReview.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvReview.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvReview.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvPartList_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPartList.Rows.Count > 0)
            {
                txtGoodsCode.Tag = dgvPartList.CurrentRow.Cells["物品ID"].Value.ToString();
                txtGoodsCode.Text = dgvPartList.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Text = dgvPartList.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpce.Text = dgvPartList.CurrentRow.Cells["规格"].Value.ToString();
                txtClientGoodsCode.Text = dgvPartList.CurrentRow.Cells["客户零件代码"].Value.ToString();
                tbsClientGoodsName.Text = dgvPartList.CurrentRow.Cells["客户零件名称"].Value.ToString();
                lbUnit.Text = dgvPartList.CurrentRow.Cells["单位"].Value.ToString();
                numCount.Value = Convert.ToDecimal(dgvPartList.CurrentRow.Cells["要货数量"].Value);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtClientGoodsCode.Text.Trim() == "" && tbsClientGoodsName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择客户零件信息！");
                return;
            }

            if (txtGoodsCode.Text.Trim() == "" && txtGoodsName.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择系统物品名称！");
                return;
            }

            if (numCount.Value < 1)
            {
                MessageDialog.ShowPromptMessage("请输入要货数量！");
                return;
            }

            DataTable dtTemp = (DataTable)dgvPartList.DataSource;

            for (int i = 0; i < dgvPartList.Rows.Count; i++)
            {
                if (txtGoodsCode.Text == dtTemp.Rows[i]["图号型号"].ToString()
                    && txtGoodsName.Text == dtTemp.Rows[i]["物品名称"].ToString()
                    && txtSpce.Text == dtTemp.Rows[i]["规格"].ToString()
                    && txtClientGoodsCode.Text == dtTemp.Rows[i]["客户零件代码"].ToString()
                    && tbsClientGoodsName.Text == dtTemp.Rows[i]["客户零件名称"].ToString())
                {
                    MessageDialog.ShowPromptMessage("不能录入重复物品");
                    return;
                }
            }

            DataRow dr = dtTemp.NewRow();

            dr["图号型号"] = txtGoodsCode.Text;
            dr["物品名称"] = txtGoodsName.Text;
            dr["规格"] = txtSpce.Text;
            dr["客户零件代码"] = txtClientGoodsCode.Text;
            dr["客户零件名称"] = tbsClientGoodsName.Text;
            dr["单位"] = lbUnit.Text;
            dr["要货数量"] = numCount.Value;
            dr["物品ID"] = txtGoodsCode.Tag.ToString();

            dtTemp.Rows.Add(dr);

            dgvPartList.DataSource = dtTemp;
        }

        private void 添加部门ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataGridViewRow dr = new DataGridViewRow();
            dgvReview.Rows.Add(dr);
        }

        private void 提交订单评审ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            YX_SalesOrder salesOrder = new YX_SalesOrder();

            if (txtBillNo.Text == "系统自动生成")
            {
                salesOrder.BillNo = m_salesOrderServer.GetNextBillID();
            }
            else
            {
                salesOrder.BillNo = txtBillNo.Text;
            }

            salesOrder.Applicant = BasicInfo.LoginID;
            salesOrder.ApplicantDate = ServerTime.Time;
            salesOrder.ClientCode = txtClient.Tag.ToString();
            salesOrder.ContractName = txtContractName.Text;
            salesOrder.DealRequire = txtDealRequire.Text;
            salesOrder.Month = Convert.ToInt32(numMonth.Value);
            salesOrder.Year = Convert.ToInt32(numYear.Value);
            salesOrder.ReviewType = txtReviewType.Text;
            salesOrder.Status = SalesOrderStatus.等待审核.ToString();

            DataTable partListDt = (DataTable)dgvPartList.DataSource;

            DataTable reviewDt = new DataTable();

            reviewDt.Columns.Add("DeptCode");

            for (int i = 0; i < dgvReview.Rows.Count; i++)
            {
                DataRow dr = reviewDt.NewRow();

                dr["DeptCode"] = dgvReview.Rows[i].Cells["部门编码"].Value.ToString();

                reviewDt.Rows.Add(dr);
            }            

            if (!m_salesOrderServer.InsertBill(salesOrder, partListDt, reviewDt, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            m_billMessageServer.SendNewFlowMessage(salesOrder.BillNo, string.Format("{0}号销售订单评审,等待主管审核", salesOrder.BillNo),
                   BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));

            this.Close();
        }

        private void dgvReview_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dgvReview.Columns;

            switch (columns[e.ColumnIndex].Name)
            {
                case "选择部门":

                    if (e.ColumnIndex == 0)
                    {
                        FormQueryInfo frm = QueryInfoDialog.GetDepartment();

                        if (frm.ShowDialog() == DialogResult.OK)
                        {
                            dgvReview.CurrentRow.Cells[2].Value = frm.GetStringDataItem("部门名称");
                            dgvReview.CurrentRow.Cells[1].Value = frm.GetStringDataItem("部门代码");
                        }
                    }

                    break;
            }
        }

        private void txtClient_OnCompleteSearch()
        {
            txtClient.Text = txtClient.DataResult["客户名称"].ToString();
            txtClient.Tag = txtClient.DataResult["客户编码"].ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvPartList.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (numCount.Value<1)
                {
                    MessageDialog.ShowPromptMessage("请输入要货数量！");
                    return;
                }

                dgvPartList.CurrentRow.Cells["图号型号"].Value = txtGoodsCode.Text;
                dgvPartList.CurrentRow.Cells["物品名称"].Value = txtGoodsName.Text;
                dgvPartList.CurrentRow.Cells["规格"].Value = txtSpce.Text;
                dgvPartList.CurrentRow.Cells["要货数量"].Value = numCount.Value;
                dgvPartList.CurrentRow.Cells["单位"].Value = lbUnit.Text;
                dgvPartList.CurrentRow.Cells["客户零件名称"].Value = tbsClientGoodsName.Text;
                dgvPartList.CurrentRow.Cells["客户零件代码"].Value = txtClientGoodsCode.Text;
                dgvPartList.CurrentRow.Cells["物品ID"].Value = txtGoodsCode.Tag;
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvPartList.CurrentRow == null)
            {
                return;
            }
            else
            {
                dgvPartList.Rows.Remove(dgvPartList.CurrentRow);
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            if (txtClient.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择客户后再进行此操作！");
                return;
            }

            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            if (CheckTable(dtTemp))
            {
                dtTemp.Columns.Add("物品ID");
                dtTemp.Columns.Add("客户零件代码");
                dtTemp.Columns.Add("客户零件名称");
                DataRow dr = dtTemp.NewRow();

                for (int i = 0; i < dtTemp.Rows.Count; i++)
                {
                    DataRow row=dtTemp.Rows[i];

                    row["物品ID"] = UniversalFunction.GetGoodsID(row["图号型号"].ToString(), row["物品名称"].ToString(), row["规格"].ToString());

                    if (Convert.ToInt32(row["物品ID"]) == 0)
                    {
                        MessageDialog.ShowPromptMessage("第"+i+"行零件有误，请确认！");
                        return;
                    }

                    DataTable dt = m_salesOrderServer.GetCommunicate(txtClient.Tag.ToString(),
                        Convert.ToInt32(row["物品ID"]));

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        row["客户零件代码"] = m_salesOrderServer.GetCommunicate(txtClient.Tag.ToString(),
                        Convert.ToInt32(row["物品ID"])).Rows[0]["CommunicateGoodsCode"];
                        row["客户零件名称"] = m_salesOrderServer.GetCommunicate(txtClient.Tag.ToString(),
                            Convert.ToInt32(row["物品ID"])).Rows[0]["CommunicateGoodsName"];
                    }
                    else
                    {
                        row["客户零件代码"] = "";
                        row["客户零件名称"] = "";
                    }
                }
                
                dgvPartList.DataSource = dtTemp;
            }
        }

        /// <summary>
        /// 检查导入的数据正确性
        /// </summary>
        /// <param name="dtTemp">导入的数据</param>
        /// <returns>数据正确返回true否则返回false</returns>
        private bool CheckTable(DataTable dtTemp)
        {
            if (!dtTemp.Columns.Contains("规格"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【规格】信息");
                return false;
            }

            if (!dtTemp.Columns.Contains("图号型号"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【图号型号】信息");
                return false;
            }

            if (!dtTemp.Columns.Contains("物品名称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【物品名称】信息");
                return false;
            }

            if (!dtTemp.Columns.Contains("要货数量"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【要货数量】信息");
                return false;
            }

            if (!dtTemp.Columns.Contains("单位"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【单位】信息");
                return false;
            }

            return true;
        }

        private void 审核toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == SalesOrderStatus.等待审核.ToString())
            {
                string deptCode = dgvReview.Rows[0].Cells["部门"].Value.ToString(); 

                YX_SalesOrder salesOrder = new YX_SalesOrder();

                salesOrder.BillNo = txtBillNo.Text;
                salesOrder.Status = SalesOrderStatus.等待评审.ToString();
                salesOrder.Auditer = BasicInfo.LoginID;
                salesOrder.AuditDate = ServerTime.Time;

                DataTable reviewDt = new DataTable();

                reviewDt.Columns.Add("DeptCode");

                for (int i = 0; i < dgvReview.Rows.Count; i++)
                {
                    DataRow dr = reviewDt.NewRow();

                    dr["DeptCode"] = dgvReview.Rows[i].Cells["部门编码"].Value.ToString();

                    reviewDt.Rows.Add(dr);
                }

                if (!m_salesOrderServer.OperationInfo(salesOrder, null,null, "", "", salesOrder.Status, "主管审核", out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审核成功！");

                    string msg = string.Format("{0} 号销售订单评审,请评审部门评审", txtBillNo.Text);
                    m_billMessageServer.PassFlowMessage(txtBillNo.Text, msg,
                             BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(deptCode).ToList());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 部门评审toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == SalesOrderStatus.等待评审.ToString())
            {
                bool flag = false;

                for (int i = 0; i < dgvReview.Rows.Count; i++)
                {
                    if (BasicInfo.DeptName == dgvReview.Rows[i].Cells["部门"].Value.ToString())
                    {
                        flag = true;

                        YX_SalesOrder salesOrder = new YX_SalesOrder();

                        salesOrder.BillNo = txtBillNo.Text;

                        if (i == dgvReview.Rows.Count - 1)
                        {
                            salesOrder.Status = SalesOrderStatus.等待确认评审.ToString();
                        }
                        else
                        {
                            salesOrder.Status = SalesOrderStatus.等待评审.ToString();
                        }

                        if (!m_salesOrderServer.OperationInfo(salesOrder, null,null, BasicInfo.DeptCode,
                            dgvReview.Rows[i].Cells["评审意见"].Value.ToString(), salesOrder.Status, "部门评审", out m_error))
                        {
                            MessageDialog.ShowPromptMessage(m_error);
                        }
                        else
                        {
                            MessageDialog.ShowPromptMessage("评审成功！");

                            if (salesOrder.Status == SalesOrderStatus.等待评审.ToString())
                            {
                                string msg = string.Format("{0} 号销售订单评审,请部门评审", m_billNo);
                                m_billMessageServer.PassFlowMessage(m_billNo, msg,
                                         BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(
                                         dgvReview.Rows[i + 1].Cells["部门"].Value.ToString()).ToList());

                            }
                            else
                            {
                                string msg = string.Format("{0} 号销售订单评审,请营销确认评审意见", m_billNo);
                                m_billMessageServer.PassFlowMessage(m_billNo, msg,
                                         BillFlowMessage_ReceivedUserType.用户, txtApplicant.Tag.ToString());
                            }

                            this.Close();
                        }
                    }
                }

                if (!flag)
                {
                    MessageDialog.ShowPromptMessage("请确认评审部门！");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 结果toolStripButton_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == SalesOrderStatus.等待评审结果.ToString())
            {
                if (txtReviewResult.Text.Trim() == "")
                {
                    MessageDialog.ShowPromptMessage("请填写评审结果！");
                    return;
                }

                YX_SalesOrder salesOrder = new YX_SalesOrder();

                salesOrder.BillNo = txtBillNo.Text;
                salesOrder.Status = SalesOrderStatus.等待确认生效.ToString();
                salesOrder.ResultPerson = BasicInfo.LoginID;
                salesOrder.ResultDate = ServerTime.Time;
                salesOrder.ReviewResult = txtReviewResult.Text;

                DataTable partDt = (DataTable)dgvPartList.DataSource;

                if (!m_salesOrderServer.OperationInfo(salesOrder, partDt,null, "", "", salesOrder.Status, "评审结果", out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    List<string> noticeUser = new List<string>();

                    noticeUser.Add(txtApplicant.Tag.ToString());

                    string msg = string.Format("{0} 号销售订单评审等待订单生效生成出库单", txtBillNo.Text);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg,
                                        BillFlowMessage_ReceivedUserType.用户, txtApplicant.Tag.ToString());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 删除评审部门ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_dataGridViewSelectRow > 0)
            {
                dgvReview.Rows.RemoveAt(m_dataGridViewSelectRow - 1);
            }
        }

        private void dgvReview_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            m_dataGridViewSelectRow = dgvReview.CurrentRow.Index + 1;
        }

        private void dgvReview_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvReview.IsCurrentCellDirty)
            {
                dgvReview.CommitEdit(DataGridViewDataErrorContexts.Commit);
            }
        }

        private void 确认评审ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == SalesOrderStatus.等待确认评审.ToString())
            {
                string deptCode = dgvReview.Rows[0].Cells["部门"].Value.ToString();


                YX_SalesOrder salesOrder = new YX_SalesOrder();

                salesOrder.BillNo = txtBillNo.Text;
                salesOrder.Status = SalesOrderStatus.等待评审结果.ToString();
                salesOrder.ResultPerson = BasicInfo.LoginID;
                salesOrder.ResultDate = ServerTime.Time;
                salesOrder.ReviewResult = txtReviewResult.Text;

                DataTable partDt = (DataTable)dgvPartList.DataSource;

                if (!m_salesOrderServer.OperationInfo(salesOrder, partDt, null, "", "", salesOrder.Status, "确认评审", out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    List<string> noticeUser = new List<string>();

                    noticeUser.Add(txtApplicant.Tag.ToString());

                    string msg = string.Format("{0} 号销售订单评审，等待评审结果", txtBillNo.Text);
                    m_billMessageServer.PassFlowMessage(m_billNo, msg,
                                        BillFlowMessage_ReceivedUserType.角色, CE_RoleEnum.营销负责人.ToString());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 回退部门评审ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (txtStatus.Text == SalesOrderStatus.等待确认评审.ToString())
            {
                string deptCode = dgvReview.Rows[0].Cells["部门"].Value.ToString();

                YX_SalesOrder salesOrder = new YX_SalesOrder();

                salesOrder.BillNo = txtBillNo.Text;
                salesOrder.Status = SalesOrderStatus.等待评审.ToString();

                DataTable reviewDt = new DataTable();

                reviewDt.Columns.Add("DeptCode");
                reviewDt.Columns.Add("Opinion");
                reviewDt.Columns.Add("Confirm");
                reviewDt.Columns.Add("ConfirmDate");

                for (int i = 0; i < dgvReview.Rows.Count; i++)
                {
                    DataRow dr = reviewDt.NewRow();

                    dr["DeptCode"] = dgvReview.Rows[i].Cells["部门编码"].Value.ToString();
                    dr["Opinion"] = dgvReview.Rows[i].Cells["评审意见"].Value.ToString();
                    dr["Confirm"] = dgvReview.Rows[i].Cells["评审确认人"].Value.ToString();
                    dr["ConfirmDate"] = dgvReview.Rows[i].Cells["评审确认时间"].Value.ToString();

                    reviewDt.Rows.Add(dr);
                }

                if (!m_salesOrderServer.OperationInfo(salesOrder, (DataTable)dgvPartList.DataSource, reviewDt, 
                    "", "", salesOrder.Status, "主管审核", out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);
                    return;
                }
                else
                {
                    MessageDialog.ShowPromptMessage("审核成功！");

                    string msg = string.Format("{0} 号销售订单评审,被"+BasicInfo.LoginName+"回退，请各部门重新评审！", txtBillNo.Text);
                    m_billMessageServer.PassFlowMessage(txtBillNo.Text, msg,
                             BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetDeptPrincipalRoleName(deptCode).ToList());

                    this.Close();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 生成出库单toolStripButton_Click(object sender, EventArgs e)
        {
            //自动生成营销出库单
            if (txtStatus.Text == SalesOrderStatus.等待确认生效.ToString())
            {
                //ISellIn m_findSellIn = ServerModule.ServerModuleFactory.GetServerModule<ISellIn>();
                //m_billNoControl = new BillNumberControl(BillTypeEnum.营销出库单, m_findSellIn);

                //DataRow row = m_dtCK.NewRow();

                //row["DJH"] = m_billNoControl.GetNewBillNo();
                //row["ObjectDept"] = txtClient.Tag.ToString();
                //row["LRRY"] = BasicInfo.LoginID;
                //row["Date"] = ServerTime.Time.ToString();
                //row["KFRY"] = "";
                //row["Price"] = 0;
                //row["SHRY"] = "";
                //row["Remark"] = "";
                //row["YWFS"] = "销售出库";
                //row["JYRY"] = "";
                //row["StorageID"] = "02";
                //row["LRKS"] = BasicInfo.DeptCode;

                //CreateDateTableStyle();

                //for (int i = 0; i < dgvPartList.Rows.Count; i++)
                //{
                //    DataRow dr = m_dtMxCK.NewRow();

                //    dr["CPID"] = Convert.ToInt32(dgvPartList.Rows[i].Cells["物品ID"].Value);
                //    dr["GoodsCode"] = dgvPartList.Rows[i].Cells["图号型号"].Value.ToString();
                //    dr["GoodsName"] = dgvPartList.Rows[i].Cells["物品名称"].Value.ToString();
                //    dr["Spec"] = dgvPartList.Rows[i].Cells["规格"].Value.ToString();
                //    dr["BatchNo"] = "";
                //    dr["SellUnitPrice"] = 0;
                //    dr["UnitPrice"] = 0;
                //    dr["Count"] = numCount.Value.ToString();
                //    dr["Unit"] = lbUnit.Text;
                //    dr["Price"] = 0;
                //    dr["Remark"] = "";
                //    dr["Provider"] = "SYS_JLRD";

                //    m_dtMxCK.Rows.Add(dr);
                //}

                //if (m_findSellIn.UpdateBill(Dt, row, MarketingType.出库.ToString(), out m_error))
                //{
                //    MessageDialog.ShowPromptMessage("保存成功");

                //    m_billMessageServer.DestroyMessage(row["DJH"].ToString());
                //    m_billMessageServer.SendNewFlowMessage(row["DJH"].ToString(), 
                //        string.Format("{0} 号营销出库单，请主管审核", row["DJH"].ToString()),
                //        BillFlowMessage_ReceivedUserType.角色,
                //        m_billMessageServer.GetDeptDirectorRoleName(BasicInfo.DeptCode).ToList());

                //    this.Close();
                //}
                //else
                //{
                //    MessageDialog.ShowErrorMessage(m_error);
                //    return;
                //}
            }
            else
            {
                MessageDialog.ShowPromptMessage("请确认单据状态！");
            }
        }

        private void 查看历史评审意见ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dgvReview.SelectedRows.Count > 0)
            {
                string deptCode = dgvReview.CurrentRow.Cells["部门编码"].Value.ToString();

                销售订单部门评审意见历史信息 frm = new 销售订单部门评审意见历史信息(txtBillNo.Text.Trim(),deptCode);

                frm.ShowDialog();
            }
        }

        /// <summary>
        /// 创建样式
        /// </summary>
        private void CreateDateTableStyle()
        {
            m_dtCK.Columns.Add("DJH");
            m_dtCK.Columns.Add("ObjectDept");
            m_dtCK.Columns.Add("LRRY");
            m_dtCK.Columns.Add("Date");
            m_dtCK.Columns.Add("KFRY");
            m_dtCK.Columns.Add("Price");
            m_dtCK.Columns.Add("SHRY");
            m_dtCK.Columns.Add("Remark");
            m_dtCK.Columns.Add("YWFS");
            m_dtCK.Columns.Add("JYRY");
            m_dtCK.Columns.Add("StorageID");
            m_dtCK.Columns.Add("LRKS");

            m_dtMxCK.Columns.Add("CPID");
            m_dtMxCK.Columns.Add("GoodsCode");
            m_dtMxCK.Columns.Add("GoodsName");
            m_dtMxCK.Columns.Add("Spec");
            m_dtMxCK.Columns.Add("BatchNo");
            m_dtMxCK.Columns.Add("SellUnitPrice");
            m_dtMxCK.Columns.Add("UnitPrice");
            m_dtMxCK.Columns.Add("Count");
            m_dtMxCK.Columns.Add("Unit");
            m_dtMxCK.Columns.Add("Price");
            m_dtMxCK.Columns.Add("Remark");
            m_dtMxCK.Columns.Add("Provider");
        }
    }
}
