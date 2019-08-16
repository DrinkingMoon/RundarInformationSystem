using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Expression;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 部门调动申请主界面
    /// </summary>
    public partial class UserControlPostChange : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag = AuthorityFlag.View;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 岗位调动服务类
        /// </summary>
        IPostChangeServer m_PostChangeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPostChangeServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public UserControlPostChange(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();
        }

        public UserControlPostChange(AuthorityFlag authority,string workID)
        {
            InitializeComponent();
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="message">窗体消息</param>
        protected override void DefWndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WndMsgSender.ShowSpecificData: // 显示特定数据的消息

                    WndMsgData msg1 = new WndMsgData();
                    Type dataType1 = msg1.GetType();

                    msg1 = (WndMsgData)message.GetLParam(dataType1);

                    // 人员工号
                    string workID = (string)GeneralFunction.IntPtrToClass(msg1.ObjectMessage, msg1.BytesOfObjectMessage);

                    PositioningRecord(workID);

                    break;

                case WndMsgSender.WarningNotice:    // 系统预警消息

                    WndMsgData msg2 = new WndMsgData();
                    Type dataType2 = msg2.GetType();

                    msg2 = (WndMsgData)message.GetLParam(dataType2);

                    // 预警数据
                    List<string> lstData = (List<string>)GeneralFunction.IntPtrToClass(msg2.ObjectMessage, msg2.BytesOfObjectMessage);

                    PositioningRecord(lstData[0]);

                    break;

                case WndMsgSender.PositioningMsg:
                    WndMsgData msg = new WndMsgData();      //这是创建自定义信息的结构
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)message.GetLParam(dataType);//这里获取的就是作为LParam参数发送来的信息的结构

                    DataTable dtMessage = UniversalFunction.PositioningOneRecord(msg.MessageContent, "部门调动申请单");

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
            toolStrip1.Visible = true;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="id">单据号</param>
        public void PositioningRecord(string id)
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
                if (id != "0" || id != "")
                {
                    if (dataGridView1.Rows[i].Cells["编号"].Value.ToString() == id)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            if (!m_PostChangeServer.GetAllPostChange(out m_queryResult, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            dataGridView1.DataSource = dt;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["原部门编号"].Visible = false;
                dataGridView1.Columns["申请部门编号"].Visible = false;
                dataGridView1.Columns["调动原因"].Visible = false;
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
        /// 窗体加载
        /// </summary>
        private void UserControlPostChange_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlPostChange_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            部门调动申请明细 frm = new 部门调动申请明细(m_authorityFlag,"0");

            frm.ShowDialog();
            RefreshControl();
        }

        private void 删除toolStripButton6_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择要删除的行！");
                return;
            }

            if (dataGridView1.SelectedRows.Count > 1)
            {
                MessageDialog.ShowPromptMessage("只能进行单行操作，不能多行删除！");
                return;
            }

            if (BasicInfo.LoginID != dataGridView1.CurrentRow.Cells["员工编号"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("只要申请人本人才能删除此单！");
                return;
            }

            if (dataGridView1.CurrentRow.Cells["总经理"].Value != null 
                && dataGridView1.CurrentRow.Cells["总经理"].Value.ToString() != "")
            {
                MessageDialog.ShowPromptMessage("总经理已批准,不能删除申请单");
                return;
            }

            int billNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value);

            if (MessageBox.Show("您是否确定要删除选中行的信息?", "消息",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                if (!m_PostChangeServer.DeletePostChange(billNo, out error))
                {
                    MessageDialog.ShowPromptMessage(error);
                    return;
                }
            }

            m_billMessageServer.BillType = "岗位调动";
            m_billMessageServer.DestroyMessage(billNo.ToString());
            RefreshControl();
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "工作岗位调动申请";
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

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string billNo = dataGridView1.CurrentRow.Cells["编号"].Value.ToString();

            部门调动申请明细 frm = new 部门调动申请明细(m_authorityFlag, billNo);

            frm.ShowDialog();

            RefreshControl();
            PositioningRecord(billNo);
        }
    }
}
