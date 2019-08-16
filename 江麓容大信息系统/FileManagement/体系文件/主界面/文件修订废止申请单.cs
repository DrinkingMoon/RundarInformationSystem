using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using Expression;
using GlobalObject;
using PlatformManagement;
using Service_Quality_File;
using UniversalControlLibrary;

namespace Form_Quality_File
{
    public partial class 文件修订废止申请单 : Form
    {
        /// <summary>
        /// 文件修订废止服务组件
        /// </summary>
        ISystemFileRevisedAbolishedBill m_serverRevisedAbolished = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileRevisedAbolishedBill>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 文件修订废止申请单()
        {
            InitializeComponent();

            string[] strBillStatus = { "全部", 
                                     "新建单据",
                                     "等待审核",
                                     "等待批准",
                                     "单据已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.文件修订废止申请单.ToString(), m_serverRevisedAbolished);
            m_billMessageServer.BillType = CE_BillTypeEnum.文件修订废止申请单.ToString();
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, CE_BillTypeEnum.文件修订废止申请单.ToString());

                    if (dtMessage.Rows.Count == 0)
                    {
                        m_billMessageServer.DestroyMessage(msg.MessageContent);
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
        /// 刷新数据
        /// </summary>
        void RefreshData()
        {
            dataGridView1.DataSource = m_serverRevisedAbolished.GetAllInfo(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
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

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            文件修订废止申请单明细 frm = new 文件修订废止申请单明细(null);
            frm.ShowDialog();
            RefreshData();
            PositioningRecord(frm.StrBillNo);
        }

        private void 文件修订废止申请单_Load(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshData();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            文件修订废止申请单明细 frm = new 文件修订废止申请单明细(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
            frm.ShowDialog();

            RefreshData();
            PositioningRecord(frm.StrBillNo);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            文件修订废止申请单明细 frm = new 文件修订废止申请单明细(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
            frm.ShowDialog();

            RefreshData();
            PositioningRecord(frm.StrBillNo);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "单据已完成")
            {
                MessageDialog.ShowPromptMessage("此单据已完成，不能删除");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("必须由申请人本人才能删除此流程");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("是否要删除此单据?") == DialogResult.Yes)
            {
                string strBillNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                if (!m_serverRevisedAbolished.DeleteInfo(strBillNo, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    m_billMessageServer.DestroyMessage(strBillNo);
                    m_billNoControl.CancelBill(strBillNo);
                    MessageDialog.ShowPromptMessage("单据删除成功");
                }
            }

            RefreshData();
        }
    }
}
