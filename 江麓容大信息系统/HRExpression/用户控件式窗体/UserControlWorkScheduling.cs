using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using GlobalObject;
using ServerModule;
using Expression;
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 人员排班管理界面
    /// </summary>
    public partial class UserControlWorkScheduling : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 单据号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// 排班信息操作类
        /// </summary>
        IWorkSchedulingServer m_workSchedulingServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IWorkSchedulingServer>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="nodeInfo">功能树节点信息</param>
        public UserControlWorkScheduling(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "排班管理";

            m_authorityFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = {"全部",  "新建单据",
                                               "等待主管审核",
                                               "等待下次排班",
                                               "已完成"};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);
            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            RefreshDataGridView();
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
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="message">窗体消息</param>
        protected override void DefWndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WndMsgSender.CloseMsg:
                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)message.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "排班信息表");

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
                    base.DefWndProc(ref message);
                    break;
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlWorkSchedulingDefinition_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
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
                if (dataGridView1.Rows[i].Cells["单据号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            m_workSchedulingServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                      + checkBillDateAndStatus1.GetSqlString("排班时间", "单据状态");

            IQueryResult result;

            if (!m_workSchedulingServer.GetAllWorkScheduling(out result, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["员工编号"].Visible = false;
            }

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
               this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
            if (m_findField == null)
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

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        private void 设置考勤toolStripButton1_Click(object sender, EventArgs e)
        {
            FormSchedulingDefinition frm = new FormSchedulingDefinition();

            frm.ShowDialog();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            FormWorkSchedule frm = new FormWorkSchedule(m_authorityFlag, 0);
            frm.ShowDialog();

            RefreshDataGridView();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            FormWorkSchedule frm = new FormWorkSchedule(m_authorityFlag, Convert.ToInt32(m_billNo));
            frm.ShowDialog();

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginID == dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString())
            {
                if (dataGridView1.SelectedRows.Count != 1)
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的数据行");
                    return;
                }

                if (MessageBox.Show("您是否确定要删除选中行的信息?", "消息",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_workSchedulingServer.DeleteWorkScheduling(Convert.ToInt32(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString()), out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");

                        m_billMessageServer.BillType = "排班管理";
                        m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                    }
                }
            }

            RefreshDataGridView();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lblStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            RefreshDataGridView();
        }

        private void UserControlWorkScheduling_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }
    }
}
