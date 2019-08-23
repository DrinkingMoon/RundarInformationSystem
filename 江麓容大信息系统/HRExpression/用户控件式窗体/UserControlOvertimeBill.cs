using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using ServerModule;
using PlatformManagement;
using Service_Peripheral_HR;
using GlobalObject;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 加班申请界面
    /// </summary>
    public partial class UserControlOvertimeBill : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 单据编号
        /// </summary>
        string m_billNo;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 加班申请操作类
        /// </summary>
        IOverTimeBillServer m_overTimeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOverTimeBillServer>();

        public UserControlOvertimeBill(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "加班申请单";

            m_authorityFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = { "全部", OverTimeBillStatus.新建单据.ToString(),
                                               OverTimeBillStatus.等待主管审核.ToString(),
                                               OverTimeBillStatus.等待部门负责人审核.ToString(),
                                               OverTimeBillStatus.等待分管领导审批.ToString(),
                                               OverTimeBillStatus.已完成.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            RefreshDataGridView();
        }

        private void UserControlOvertimeBill_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "加班申请单");

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
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string billNo)
        {
            if (Convert.ToInt32(billNo) > 0)
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
        }

        /// <summary>
        /// 改变控件大小
        /// </summary>
        private void UserControlOvertimeBill_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            m_overTimeServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                      + checkBillDateAndStatus1.GetSqlString("申请时间", "单据状态");

            IQueryResult result;

            if (!m_overTimeServer.GetAllOverTimeBill(out result, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            //if (result.DataCollection.Tables[0].Rows.Count == 0)
            //{
            //    if (!m_overTimeServer.GetAllOverTimeBillByWorkID(out result, out error))
            //    {
            //        MessageDialog.ShowErrorMessage(error);
            //        return;
            //    }
            //}

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["员工编号"].Visible = false;
                dataGridView1.Columns["部门编码"].Visible = false;
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

            //dataGridView1.Refresh();
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            刷新toolStripButton1_Click(null,null);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 删除单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginID == dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString())
            {
                if (dataGridView1.SelectedRows.Count != 1)
                {
                    MessageDialog.ShowPromptMessage("请选择需要删除的一行记录！");
                    return;
                }

                if (MessageBox.Show("您是否确定要删除选中行的信息?", "消息",
                       MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (lblStatus.Text.Trim() != OverTimeBillStatus.已完成.ToString())
                    {
                        m_billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                        if (!m_overTimeServer.DeleteOverTimeBill(Convert.ToInt32(m_billNo), out error))
                        {
                            MessageDialog.ShowPromptMessage(error);
                            return;
                        }

                        m_billMessageServer.BillType = "加班申请单";
                        m_billMessageServer.DestroyMessage(m_billNo);
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("单据已完成，不能删除");
                        return;
                    }
                }
            }
            else 
            {
                MessageDialog.ShowPromptMessage("只有【" + dataGridView1.CurrentRow.Cells["申请人"].Value.ToString() + "】本人才可以删除此单据");
            }

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dataGridView1.Rows[i].Cells["单据状态"].Value.ToString() != "已完成")
                    {
                        dataGridView1.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                    }
                }
            }
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lblStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            m_billNo = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
            FormOverTimeList frm = new FormOverTimeList(m_authorityFlag, Convert.ToInt32(m_billNo));
            frm.ShowDialog();

            RefreshDataGridView();
            PositioningRecord(m_billNo);
        }

        private void 新建单据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormOverTimeList frm = new FormOverTimeList(m_authorityFlag, 0);
            frm.ShowDialog();

            RefreshDataGridView();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "加班申请综合查询";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 人力批量处理toolStripButton3_Click(object sender, EventArgs e)
        {
            人力批量处理加班单 frm = new 人力批量处理加班单();
            frm.ShowDialog();
            RefreshDataGridView();
        }
    }
}
