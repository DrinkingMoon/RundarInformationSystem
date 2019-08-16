using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WebServerModule;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 售后服务质量反馈单界面
    /// </summary>
    public partial class 售后服务质量反馈单 : Form
    {
        /// <summary>
        /// 数据集
        /// </summary>
        S_ServiceFeedBack m_lnqMess = new S_ServiceFeedBack();

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 服务类
        /// </summary>
        IServiceFeedBack m_serverFeedBack = WebServerModule.ServerModuleFactory.GetServerModule<IServiceFeedBack>();

        /// <summary>
        /// 反馈单号
        /// </summary>
        string m_strDJH;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public 售后服务质量反馈单(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "售后质量信息反馈单";

            m_authFlag = nodeInfo.Authority;
            string[] strBillStatus = { "全部", "等待确认返回时间", "等待主管审核", "等待质管确认", "等待责任部门确认", "等待责任人确认", "等待质管检查", "单据完成" };

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);
            
            DataBindGirdView();
        }

        private void 售后服务质量反馈单_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);
        }

        private void 售后服务质量反馈单_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {            
            DataBindGirdView();
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "售后质量信息反馈单");

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
                if ((string)dataGridView1.Rows[i].Cells["反馈单号"].Value == billNo)
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
        /// <param name="source">数据集</param>
        void DataBindGirdView()
        {
            m_serverFeedBack.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                     + checkBillDateAndStatus1.GetSqlString("反馈日期", "单据状态");

            IQueryResult result;

            if (!m_serverFeedBack.GetAllBillFeedBack(out result, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["反馈人编号"].Visible = false;
                dataGridView1.Columns["责任人编号"].Visible = false;
                dataGridView1.Columns["营销售后审核人编号"].Visible = false;
                dataGridView1.Columns["质管意见提出人编号"].Visible = false;
                dataGridView1.Columns["责任部门主管编号"].Visible = false;
                dataGridView1.Columns["部门编码"].Visible = false;
                dataGridView1.Columns["质管检查人编号"].Visible = false;

            }

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

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            m_strDJH = dataGridView1.CurrentRow.Cells["反馈单号"].Value.ToString();
            string ServiceDJH = dataGridView1.CurrentRow.Cells["关联号"].Value.ToString();

            if (m_strDJH != "" || m_strDJH != null)
            {
                售后服务质量反馈单明细 form = new 售后服务质量反馈单明细(m_authFlag, m_strDJH, ServiceDJH);
                form.BusinessView = dataGridView1;
                form.BusinessView_Row = dataGridView1.CurrentRow;
                form.ShowDialog();
            }

            DataBindGirdView();
            PositioningRecord(m_strDJH);
        }

        private void 刷新toolStripButton2_Click(object sender, EventArgs e)
        {
            DataBindGirdView();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            售后服务质量反馈单明细 form = new 售后服务质量反馈单明细(m_authFlag);
            form.ShowDialog();

            DataBindGirdView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 综合toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "售后质量反馈查询";
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

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            刷新toolStripButton2_Click(null, null);
        }
    }
}
