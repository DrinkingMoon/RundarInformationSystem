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
    public partial class 销售订单评审 : Form
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
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 销售合同/订单评审服务类
        /// </summary>
        ISalesOrderServer m_salesOrderServer = ServerModuleFactory.GetServerModule<ISalesOrderServer>();

        public 销售订单评审(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = { "全部", "等待评审","等待评审结果", "已完成" };

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            RefreshDataGridView();
        }

        private void 销售订单评审_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void 销售订单评审_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="m">窗体消息</param>
        protected override void DefWndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)m.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "营销");

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
        /// 刷新数据
        /// </summary>
        void RefreshDataGridView()
        {
            DataTable dt = m_salesOrderServer.GetAllBillInfo(checkBillDateAndStatus1.cmbBillStatus.Text,
                checkBillDateAndStatus1.dtpStartTime.Value,checkBillDateAndStatus1.dtpEndTime.Value);

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
               UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            btnRefresh_Click(null, null);
        }

        private void 新建toolStripButton_Click(object sender, EventArgs e)
        {
            销售订单零件明细 frm = new 销售订单零件明细();

            frm.ShowDialog();
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                销售订单零件明细 frm = new 销售订单零件明细(billNo);

                frm.ShowDialog();
                RefreshDataGridView();
                PositioningRecord(billNo);
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一条记录！");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                string billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "已完成")
                {
                    if (!m_salesOrderServer.DeleteBill(billNo, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");

                        RefreshDataGridView();
                    }
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一条记录！");
            }
        }
    }
}
