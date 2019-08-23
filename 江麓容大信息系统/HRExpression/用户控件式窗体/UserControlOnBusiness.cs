using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using Expression;
using Service_Peripheral_HR;
using GlobalObject;
using ServerModule;
using UniversalControlLibrary;
using System.Collections;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 出差申请主界面
    /// </summary>
    public partial class UserControlOnBusiness : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        /// <summary>
        /// 出差申请表操作类
        /// </summary>
        IOnBusinessBillServer m_onBusinessServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IOnBusinessBillServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public UserControlOnBusiness(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "出差申请单";
            m_authorityFlag = nodeInfo.Authority;

            #region 数据筛选
            string[] strBillStatus = { "全部", OnBusinessBillStatus.新建单据.ToString(),
                                               OnBusinessBillStatus.等待随行人员部门确认.ToString(),
                                               OnBusinessBillStatus.等待部门负责人审核.ToString(),
                                               OnBusinessBillStatus.等待分管领导审批.ToString(),
                                               OnBusinessBillStatus.等待总经理批准.ToString(),
                                               OnBusinessBillStatus.等待销差人确认.ToString(),
                                               OnBusinessBillStatus.等待出差结果说明.ToString(),
                                               OnBusinessBillStatus.已完成.ToString()};

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            m_onBusinessServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
               + checkBillDateAndStatus1.GetSqlString("申请时间", "单据状态");            

            RefreshDataGridView();
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "出差申请单");

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

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "";
            IQueryResult qr = null;
            List<string> lstFindField = new List<string>();

            IQueryable<View_HR_PersonnelArchive> directorGroup3 = m_personnerServer.GetDeptDirector(BasicInfo.DeptCode, "0");
            IQueryable<View_HR_PersonnelArchive> directorGroup = m_personnerServer.GetDeptDirector(BasicInfo.DeptCode, "1");
            IQueryable<View_HR_PersonnelArchive> directorGroup2 = m_personnerServer.GetDeptDirector(BasicInfo.DeptCode, "2");

            authorization = PlatformFactory.GetObject<IAuthorization>();
            businessID = "出差综合查询";
            string[] pare = { BasicInfo.DeptCode };
            qr = authorization.QueryMultParam(businessID, null, pare);

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

        /// <summary>
        /// 改变控件大小
        /// </summary>
        private void UserControlOnBusiness_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="result">结果集</param>
        private void RefreshDataGridView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["员工编号"].Visible = false;
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["出差结果说明"].Visible = false;
                dataGridView1.Columns["大写金额"].Visible = false;
                dataGridView1.Columns["关联部门"].Visible = false;
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

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="result">结果集</param>
        private void RefreshDataGridView()
        {
            m_onBusinessServer.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
               + checkBillDateAndStatus1.GetSqlString("申请时间", "单据状态");

            IQueryResult result;

            if (!m_onBusinessServer.GetAllOnBusinessBill(out result, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            //if (result.DataCollection.Tables[0].Rows.Count == 0)
            //{
            //    if (!m_onBusinessServer.GetAllOnBusinessBillByWorkID(out result, out error))
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
                dataGridView1.Columns["出差结果说明"].Visible = false;
                dataGridView1.Columns["大写金额"].Visible = false;
                dataGridView1.Columns["关联部门"].Visible = false;
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
                    if (dataGridView1.Rows[i].Cells["编号"].Value.ToString() == billNo)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            int billID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value);

            FormSchedule frm = new FormSchedule(m_authorityFlag,billID);
            frm.ShowDialog();

            RefreshDataGridView();
            PositioningRecord(billID.ToString());
        }

        private void 申请toolStripButton1_Click(object sender, EventArgs e)
        {
            FormSchedule frm = new FormSchedule(m_authorityFlag,0);
            frm.ShowDialog();
           
            RefreshDataGridView();
            PositioningRecord(frm.BillNo);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            刷新toolStripButton1_Click(null, null);
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 回退toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() != "已完成")
                {
                    回退单据 form = new 回退单据("出差申请单", dataGridView1.CurrentRow.Cells["编号"].Value.ToString(), dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString());
                    form.ShowDialog();

                    if (!form.BlFlag)
                    {
                        return;
                    }
                    else
                    {
                        if (MessageBox.Show("您确定要回退此单据吗？", "提示", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            if (m_onBusinessServer.ReturnBill(form.StrBillID,
                                form.StrBillStatus, form.Reason, out error))
                            {
                                MessageDialog.ShowPromptMessage("回退成功");
                            }
                            else
                            {
                                MessageDialog.ShowPromptMessage(error);
                            }
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowPromptMessage("请重新确认单据状态");
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选择一条单据操作！");
            }

            RefreshDataGridView();
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

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.人力资源部办公室文员.ToString()))
            {
                if (MessageBox.Show("您是否确定要删除选中行的信息?", "消息",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_onBusinessServer.DeleteOnBusinessBill(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value.ToString()), out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                    }

                    m_billMessageServer.BillType = "出差申请单";
                    m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["编号"].Value.ToString());
                }

                return;
            }

            if (BasicInfo.LoginID == dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString())
            {
                if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == OnBusinessBillStatus.已完成.ToString()
                    || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == OnBusinessBillStatus.等待出差结果说明.ToString()
                    || dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == OnBusinessBillStatus.等待销差人确认.ToString())
                {
                    MessageDialog.ShowPromptMessage("领导已经批准出差，不能删除！");
                    return;
                }

                if (MessageBox.Show("您是否确定要删除选中行的信息?", "消息",
                      MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (!m_onBusinessServer.DeleteOnBusinessBill(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value.ToString()), out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }
                    else
                    {
                        MessageDialog.ShowPromptMessage("删除成功！");
                    }

                    m_billMessageServer.BillType = "出差申请单";
                    m_billMessageServer.DestroyMessage(dataGridView1.CurrentRow.Cells["编号"].Value.ToString());
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("只有【" + dataGridView1.CurrentRow.Cells["申请人姓名"].Value.ToString() + "】本人才可以删除此单据！");
            }

            RefreshDataGridView();
        }

        private void UserControlOnBusiness_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            Hashtable parameters = new Hashtable();

            parameters.Add("@WorkID", BasicInfo.LoginID);
            parameters.Add("@StartDate", checkBillDateAndStatus1.dtpStartTime.Value);
            parameters.Add("@EndDate", checkBillDateAndStatus1.dtpEndTime.Value);

            DataTable dtSoucre = GlobalObject.DatabaseServer.QueryInfoPro("HR_OnBusiness_Select", parameters, out error);
            RefreshDataGridView(dtSoucre);
        }
    }
}
