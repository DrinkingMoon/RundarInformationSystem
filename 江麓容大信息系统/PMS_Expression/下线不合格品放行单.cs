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
    public partial class 下线不合格品放行单 : Form
    {
        /// <summary>
        /// 查找条件窗体
        /// </summary>
        FormConditionFind m_formFindCondition;

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductCodeServer m_serverProductCode = ServerModuleFactory.GetServerModule<IProductCodeServer>();

        /// <summary>
        /// LINQ实体集
        /// </summary>
        ZL_ProductReleases m_lnqProductReleases = new ZL_ProductReleases();

        /// <summary>
        /// 下线不合格品放行单服务组件
        /// </summary>
        IProductReleases m_serverReleases = PMS_ServerFactory.GetServerModule<IProductReleases>();

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IBomServer m_serviceBom = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 下线不合格品放行单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "下线不合格品放行单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.下线不合格品放行单, m_serverReleases);

            m_authFlag = nodeInfo.Authority;

            lbDJZT.Text = "";
            
            ClearDate();

            cmbEdition.DataSource = m_serviceBom.GetAssemblyTypeList();
            cmbEdition.SelectedIndex = -1;

            string[] strBillStatus = { "全  部", 
                                     "等待审核", 
                                     "等待批准",
                                     "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            刷新数据ToolStripMenuItem_Click(null, null);
        }

        private void 下线不合格品放行单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearDate()
        {
            lbDJZT.Text = "";
            txtBillNo.Text = "";
            cmbEdition.SelectedIndex = -1;
            txtProductCode.Text = "";
            txtFaultPhenomenon.Text = "";
            txtRemark.Text = "";
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="source">数据集</param>
        void RefreshDataGirdView(DataTable source)
        {
            m_findField = null;

            dataGridView1.DataSource = source;

            userControlDataLocalizer.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            // 添加查询用的列
            if (m_findField == null || m_findField.Count() == 0)
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
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 获得实体集数据
        /// </summary>
        void GetMessage()
        {
            m_lnqProductReleases = new ZL_ProductReleases();

            m_lnqProductReleases.BillNo = txtBillNo.Text;
            m_lnqProductReleases.BillStatus = lbDJZT.Text;
            m_lnqProductReleases.FaultPhenomenon = txtFaultPhenomenon.Text;
            m_lnqProductReleases.ProductCode = txtProductCode.Text;
            m_lnqProductReleases.ProductModel = cmbEdition.Text;
            m_lnqProductReleases.Remark = txtRemark.Text;
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "下线不合格品放行单");

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

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClearDate();
            txtBillNo.Text = "系统自动生成";
            lbDJZT.Text = "新建单据";
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView(m_serverReleases.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text, chbIsShowNew.Checked));
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
                txtBillNo.Text = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
                txtFaultPhenomenon.Text = dataGridView1.CurrentRow.Cells["故障现象"].Value.ToString();
                txtProductCode.Text = dataGridView1.CurrentRow.Cells["产品箱号"].Value.ToString();
                txtRemark.Text = dataGridView1.CurrentRow.Cells["备注"].Value.ToString();
                cmbEdition.Text = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
            }
        }

        private void 报废单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            if (dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("必须为申请人才能删除此单据");
                return;
            }

            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            if (MessageDialog.ShowEnquiryMessage("是否要删除【"+ billNo +"】号单据?") == DialogResult.Yes )
            {
                m_serverReleases.DeleteBill(billNo);
                m_msgPromulgator.DestroyMessage(billNo);
                m_billNoControl.CancelBill(billNo);
            }

            刷新数据ToolStripMenuItem_Click(sender,e);
        }

        private void 提交单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cmbEdition.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择产品型号");
                return;
            }

            if (!m_serverProductCode.VerifyProductCodesInfo(cmbEdition.Text, txtProductCode.Text.Trim(), 
                GlobalObject.CE_BarCodeType.内部钢印码, out m_strErr))
            {
                txtProductCode.Text = "";

                MessageDialog.ShowPromptMessage(m_strErr);
                txtProductCode.Focus();
                return;
            }

            GetMessage();

            if (m_lnqProductReleases.BillStatus != "新建单据")
            {
                MessageDialog.ShowPromptMessage("单据状态不正确");
                return;
            }

            if (!m_serverReleases.SubmitBill(m_lnqProductReleases,out m_strErr))
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                MessageDialog.ShowPromptMessage("提交成功");

                m_msgPromulgator.DestroyMessage(m_lnqProductReleases.BillNo);
                m_msgPromulgator.SendNewFlowMessage(m_lnqProductReleases.BillNo,
                    string.Format("{0} 号下线不合格品放行单，请质量工程师审核", m_lnqProductReleases.BillNo),
                    CE_RoleEnum.质检员_下线);
            }

            刷新数据ToolStripMenuItem_Click(null, null);
            PositioningRecord(m_lnqProductReleases.BillNo);
        }

        private void 审核单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    if (item.Cells["单据状态"].Value.ToString() != "等待审核")
                    {
                        MessageDialog.ShowPromptMessage("【" + item.Cells["单据号"].Value.ToString() + "】号单据状态不正确");
                        return;
                    }
                    else
                    {
                        m_lnqProductReleases.BillNo = item.Cells["单据号"].Value.ToString();

                        if (!m_serverReleases.SubmitBill(m_lnqProductReleases, out m_strErr))
                        {
                            throw new Exception(m_strErr);
                        }
                        else
                        {
                            m_msgPromulgator.PassFlowMessage(m_lnqProductReleases.BillNo,
                                string.Format("{0} 号下线不合格品放行单，请质管部部长批准", m_lnqProductReleases.BillNo),
                                CE_RoleEnum.质控负责人);
                        }
                    }
                }

                MessageDialog.ShowPromptMessage("审核成功");
            }
            catch (Exception ex)
            {
                m_strErr = ex.Message;
                MessageDialog.ShowPromptMessage(m_strErr);
            }

            刷新数据ToolStripMenuItem_Click(null, null);
        }

        private void 确认通过ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in dataGridView1.SelectedRows)
                {
                    if (item.Cells["单据状态"].Value.ToString() != "等待批准")
                    {
                        MessageDialog.ShowPromptMessage("【" + item.Cells["单据号"].Value.ToString() + "】号单据状态不正确");
                        return;
                    }
                    else
                    {
                        m_lnqProductReleases.BillNo = item.Cells["单据号"].Value.ToString();

                        if (!m_serverReleases.SubmitBill(m_lnqProductReleases, out m_strErr))
                        {
                            throw new Exception(m_strErr);
                        }
                        else
                        {
                            List<string> noticeRoles = new List<string>();
                            noticeRoles.Add(CE_RoleEnum.质量工程师.ToString());
                            noticeRoles.Add(CE_RoleEnum.质控负责人.ToString());

                            List<string> noticeUers = new List<string>();
                            noticeRoles.Add(UniversalFunction.GetPersonnelCode(item.Cells["申请人"].Value.ToString()));

                            m_msgPromulgator.EndFlowMessage(m_lnqProductReleases.BillNo,
                                    string.Format("{0} 号下线不合格品放行单已经处理完毕", m_lnqProductReleases.BillNo),
                                    noticeRoles, noticeUers);
                        }
                    }
                }

                MessageDialog.ShowPromptMessage("批准成功");
            }
            catch (Exception ex)
            {
                m_strErr = ex.Message;
                MessageDialog.ShowPromptMessage(m_strErr);
            }

            刷新数据ToolStripMenuItem_Click(null, null);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView(m_serverReleases.GetAllBill(checkBillDateAndStatus1.dtpStartTime.Value,
                checkBillDateAndStatus1.dtpEndTime.Value, checkBillDateAndStatus1.cmbBillStatus.Text, chbIsShowNew.Checked));
        }

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_formFindCondition != null && !m_formFindCondition.SaveFlag)
            {
                m_formFindCondition = null;
            }

            if (m_formFindCondition == null)
            {
                m_formFindCondition = new FormConditionFind(this, m_findField, labelTitle.Text, labelTitle.Text);
            }

            m_formFindCondition.ShowDialog();
        }

        private void chbIsShowNew_CheckedChanged(object sender, EventArgs e)
        {
            刷新数据ToolStripMenuItem_Click(sender, e);
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
    }
}
