using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 技术变更处置单 : Form
    {
        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error = "";

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 技术变更服务组件
        /// </summary>
        ITechnologyAlteration m_technologyServer = ServerModuleFactory.GetServerModule<ITechnologyAlteration>();

        public 技术变更处置单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            m_msgPromulgator.BillType = "技术变更单";
            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.技术变更单, m_technologyServer);

            string[] strBillStatus = { "全部", 
                                     "等待批准",
                                     "等待财务审核",
                                     "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);
            RefreshDataGirdView();
        }

        private void 技术变更处置单_Load(object sender, EventArgs e)
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "技术变更单");

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
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void 技术变更处置单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGirdView();
        }

        /// <summary>
        /// 数据刷新
        /// </summary>
        private void RefreshDataGirdView()
        {
            dataGridView1.DataSource = m_technologyServer.GetAllBill(checkBillDateAndStatus1.cmbBillStatus.Text, 
                checkBillDateAndStatus1.dtpStartTime.Value, checkBillDateAndStatus1.dtpEndTime.Value);

            dataGridView1.Columns["序号"].Visible = false;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                DataGridViewRow row = dataGridView1.CurrentRow;

                if (BasicInfo.LoginName != row.Cells["申请人"].Value.ToString())
                {
                    MessageDialog.ShowPromptMessage("您不是此记录的申请人无法进行此操作");
                    return;
                }

                if (row.Cells["单据状态"].Value.ToString() == "已完成")
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                    return;
                }
                else
                {
                    if (!m_technologyServer.DeleteBill(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString(), out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");

                        RefreshDataGirdView();
                        PositioningRecord(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                    }
                }
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            技术变更处置明细 frm = new 技术变更处置明细("", m_authFlag);
            frm.ShowDialog();

            RefreshDataGirdView();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGirdView();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            技术变更处置明细 frm = new 技术变更处置明细(billNo, m_authFlag);
            frm.ShowDialog();

            RefreshDataGirdView();
            PositioningRecord(billNo);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lbDJZT.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
        }
    }
}
