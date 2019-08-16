using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using Expression;
using PlatformManagement;
using Service_Peripheral_External;
using UniversalControlLibrary;

namespace Form_Peripheral_External
{
    public partial class 售后配件申请单 : Form
    {
        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 售后配件申请服务组件
        /// </summary>
        IAfterServicePartsApply m_serverAfterService = Service_Peripheral_External.ServerModuleFactory.GetServerModule<IAfterServicePartsApply>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        public 售后配件申请单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.售后配件申请单, m_serverAfterService);

            string[] strBillStatus = { "全部", 
                                     "等待主管审核",
                                     "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            DataRefresh();
        }

        private void 售后配件申请单_Load(object sender, EventArgs e)
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "售后配件申请单");

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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        void DataRefresh()
        {
            dataGridView1.DataSource = m_serverAfterService.GetAllBillInfo(checkBillDateAndStatus1.cmbBillStatus.Text, 
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value);

            userControlDataLocalizer1.Init(dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="info">定位用的信息</param>
        void PositioningRecord(string info)
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
                if ((string)dataGridView1.Rows[i].Cells["单据号"].Value == info)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

            售后配件申请明细 form = new 售后配件申请明细(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), m_authFlag);
            form.ShowDialog();


            DataRefresh();
            PositioningRecord(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            售后配件申请明细 form = new 售后配件申请明细("", m_authFlag);
            form.ShowDialog();

            DataRefresh();

        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                售后配件申请明细 form = new 售后配件申请明细(strBillNo, m_authFlag);
                form.ShowDialog();

                DataRefresh();
                PositioningRecord(strBillNo);
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataRefresh();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            DataRefresh();
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "已完成")
                {
                    MessageDialog.ShowPromptMessage("此单据已完成，无法删除");
                    return;
                }

                if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["申请人"].Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("只有此单据的编制人才能删除单据");
                    return;
                }

                if (MessageDialog.ShowEnquiryMessage("是否要删除【" + dataGridView1.CurrentRow.Cells["单据号"].Value + "】号单据？") == DialogResult.Yes)
                {
                    if (!m_serverAfterService.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_strErr))
                    {
                        MessageDialog.ShowPromptMessage(m_strErr);
                    }
                    else
                    {
                        m_msgPromulgator.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                        m_billNoControl.CancelBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                        MessageDialog.ShowPromptMessage("删除成功");
                    }
                }

                DataRefresh();
            }
        }
    }
}
