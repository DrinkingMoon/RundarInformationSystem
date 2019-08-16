﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using WebServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 售后函电界面
    /// </summary>
    public partial class 售后函电 : Form
    {
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
        /// 单据消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 服务类
        /// </summary>
        IServiceFeedBack m_serverFeedBack = WebServerModule.ServerModuleFactory.GetServerModule<IServiceFeedBack>();

        /// <summary>
        /// 单据号
        /// </summary>
        string m_strBillID;

        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        public 售后函电(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = "售后函电处理单";

            m_authFlag = nodeInfo.Authority;

            //dtpStartTime.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            //dtpEndTime.Value = ServerTime.Time.AddDays(1);
            //DataTable Dt = m_serverFeedBack.GetAfterService(dtpStartTime.Value.ToString(), dtpEndTime.Value.ToString());

            #region 数据筛选
            string[] strBillStatus = { "全部", "等待接单", "等待审核", "等待回访", "处理完成" };

            checkBillDateAndStatus1.InsertComBox(strBillStatus);

            checkBillDateAndStatus1.OnCompleteSearch +=
                new GlobalObject.DelegateCollection.NonArgumentHandle(checkBillDateAndStatus1_OnCompleteSearch);

            checkBillDateAndStatus1.dtpStartTime.Value = ServerTime.Time.AddDays(1).AddMonths(-1);
            checkBillDateAndStatus1.dtpEndTime.Value = ServerTime.Time.AddDays(1);

            #endregion

            DataBindGirdView();
        }

        private void 售后函电_Load(object sender, EventArgs e)
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
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
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

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "售后函电处理单");

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

        private void 售后函电_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void DataBindGirdView()
        {
            m_serverFeedBack.QueryResultFilter = QueryFilterControl.GetFilterString(labelTitle.Text)
                      + checkBillDateAndStatus1.GetSqlString("函电录入时间", "单据状态");

            IQueryResult result;

            if (!m_serverFeedBack.GetAllBill(out result, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            dataGridView1.DataSource = result.DataCollection.Tables[0];

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["接函电人编号"].Visible = false;
                dataGridView1.Columns["录入人编号"].Visible = false;
                dataGridView1.Columns["处理人编号"].Visible = false;
                dataGridView1.Columns["审核人编号"].Visible = false;
                dataGridView1.Columns["回访人编号"].Visible = false;
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
            string strDJH = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

            FormAfterService frm = new FormAfterService(strDJH, m_authFlag);
            frm.ShowDialog();
            DataBindGirdView();
            PositioningRecord(strDJH);
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            DataBindGirdView();
        }

        private void 刷新toolStripButton2_Click(object sender, EventArgs e)
        {
            DataBindGirdView();
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            FormAfterService frm = new FormAfterService(m_authFlag);
            frm.ShowDialog();
            DataBindGirdView();
            PositioningRecord(frm.LnqAfterService.ServiceID);
        }

        private void 导出toolStripButton1_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            m_strBillID = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["函电录入人"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要删除的记录");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString() == "")
            {
                MessageDialog.ShowPromptMessage("此单据状态下不能进行删除~！");
                return;
            }

            if (MessageBox.Show("您是否确定要删除单据号为【" + dataGridView1.CurrentRow.Cells["单据号"].Value.ToString() + "】",
                "提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                try
                {
                    if (m_serverFeedBack.DeleteByBillNo(m_strBillID, out error))
                    {
                        MessageBox.Show("删除成功", "提示");

                        m_msgPromulgator.DestroyMessage(dataGridView1.CurrentRow.Cells["单据号"].Value.ToString());
                        刷新toolStripButton2_Click(sender, e);
                    }
                }
                catch (Exception ex)
                {
                    MessageDialog.ShowPromptMessage(ex.Message);
                }
            }
        }

        private void 综合toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "售后函电处理单";
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

        private void checkBillDateAndStatus1_OnCompleteSearch()
        {
            刷新toolStripButton2_Click(null,null);
        }
    }
}
