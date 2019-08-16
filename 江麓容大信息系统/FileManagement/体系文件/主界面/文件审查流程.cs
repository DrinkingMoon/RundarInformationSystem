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
    public partial class 文件审查流程 : Form
    {
        /// <summary>
        /// 文件审查流程服务组件
        /// </summary>
        ISystemFileReviewProcess m_serverReviewProcess = Service_Quality_File.ServerModuleFactory.GetServerModule<ISystemFileReviewProcess>();

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

        public 文件审查流程()
        {
            InitializeComponent();

            string[] strBillStatus = { "全部", 
                                     "新建流程",
                                     "等待主管审核",
                                     "等待相关确认",
                                     "等待判定",
                                     "流程已结束"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.文件审查流程.ToString(), m_serverReviewProcess);
            m_billMessageServer.BillType = CE_BillTypeEnum.文件审查流程.ToString();
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, CE_BillTypeEnum.文件审查流程.ToString());

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
            m_serverReviewProcess.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                + checkBillDateAndStatus1.GetSqlString("申请日期", "流程状态");

            dataGridView1.DataSource = m_serverReviewProcess.GetAllInfo();

            userControlDataLocalizer1.Init(dataGridView1,this.Name,
                UniversalFunction.SelectHideFields(this.Name,dataGridView1.Name,BasicInfo.LoginID));
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
                if ((string)dataGridView1.Rows[i].Cells["流程编号"].Value == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            文件审查流程明细 frm = new 文件审查流程明细(null);
            frm.ShowDialog();
            RefreshData();
            PositioningRecord(frm.StrSDBNo);
        }

        private void 文件审查流程_Load(object sender, EventArgs e)
        {
            checkBillDateAndStatus1.InitDateTime();
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

            文件审查流程明细 frm = new 文件审查流程明细(dataGridView1.CurrentRow.Cells["流程编号"].Value.ToString());
            frm.ShowDialog();

            RefreshData();
            PositioningRecord(frm.StrSDBNo);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

            if (dataGridView1.CurrentRow == null)
            {
                return;
            }

            文件审查流程明细 frm = new 文件审查流程明细(dataGridView1.CurrentRow.Cells["流程编号"].Value.ToString());
            frm.ShowDialog();

            RefreshData();
            PositioningRecord(frm.StrSDBNo);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow.Cells["流程状态"].Value.ToString() == "流程已结束")
            {
                MessageDialog.ShowPromptMessage("流程状态已结束，不能删除");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() != BasicInfo.LoginName)
            {
                MessageDialog.ShowPromptMessage("必须由申请人本人才能删除此流程");
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("是否要删除此流程?") == DialogResult.Yes)
            {
                string strSDBNo = dataGridView1.CurrentRow.Cells["流程编号"].Value.ToString();

                if (!m_serverReviewProcess.DeleteProcess(strSDBNo, out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                    return;
                }
                else
                {
                    m_billMessageServer.DestroyMessage(strSDBNo);
                    m_billNoControl.CancelBill(strSDBNo);
                    MessageDialog.ShowPromptMessage("流程删除成功");
                }
            }

            RefreshData();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
