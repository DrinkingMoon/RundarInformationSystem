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
    /// 员工离职申请主界面
    /// </summary>
    public partial class UserControlDimissionBill : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string error;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 操作权限
        /// </summary>
        AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 员工离职数据集
        /// </summary>
        HR_DimissionBill m_Dimission;        

        /// <summary>
        /// 员工离职申请服务类
        /// </summary>
        IDimissionServer m_dimiServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IDimissionServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        public UserControlDimissionBill(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_billMessageServer.BillType = "员工离职申请单";

            m_authorityFlag = nodeInfo.Authority;
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(menuStrip1, authorityFlag);
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

                default:
                    base.DefWndProc(ref message);
                    break;
            }
        }

         /// <summary>
        /// 刷新
        /// </summary>
        void RefreshControl()
        {
            if (!m_dimiServer.GetAllDimission(out m_queryResult, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];
            dataGridView1.DataSource = dt;

            if (dataGridView1.Rows.Count > 0)
            {
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

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UserControlDimissionBill_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的编号</param>
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
                if (billNo != "0" || billNo != "")
                {
                    if (dataGridView1.Rows[i].Cells["员工编号"].Value.ToString() == billNo)
                    {
                        dataGridView1.FirstDisplayedScrollingRowIndex = i;
                        dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlDimissionBill_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            m_Dimission = m_dimiServer.GetAllDimission(Convert.ToInt32(dataGridView1.CurrentRow.Cells["编号"].Value));

            string deptName = dataGridView1.CurrentRow.Cells["部门"].Value.ToString();
            string deptCode = dataGridView1.CurrentRow.Cells["部门编码"].Value.ToString();
            string workPost = dataGridView1.CurrentRow.Cells["岗位"].Value.ToString();
            string name = dataGridView1.CurrentRow.Cells["员工姓名"].Value.ToString();

            FormDimissionList frm = new FormDimissionList(m_authorityFlag,m_Dimission, deptName, deptCode, workPost, name,m_Dimission.ID);
            frm.ShowDialog();

            RefreshControl();
            PositioningRecord(m_Dimission.ID.ToString());
        }

        private void 离职申请ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormDimissionList frm = new FormDimissionList(m_authorityFlag);
            frm.ShowDialog();

            RefreshControl();
            PositioningRecord(frm.BillID.ToString());
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            lblStatus.Text = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();
        }

        private void 刷新数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 综合查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "员工离职申请查询";
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

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }
    }
}
