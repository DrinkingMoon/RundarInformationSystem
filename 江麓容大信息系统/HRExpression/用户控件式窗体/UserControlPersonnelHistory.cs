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
using PlatformManagement;
using GlobalObject;
using Service_Peripheral_HR;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 员工档案变更历史主界面
    /// </summary>
    public partial class UserControlPersonnelHistory : Form
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
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 人员档案变更数据集
        /// </summary>
        View_HR_PersonnelArchiveChange personnelChange;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        public UserControlPersonnelHistory(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
        }

        /// <summary>
        /// 重新窗体消息处理函数
        /// </summary>
        /// <param name="message">窗体消息</param>
        protected override void DefWndProc(ref Message message)
        {
            switch (message.Msg)
            {
                case WndMsgSender.ShowSpecificData:

                    WndMsgData msg = new WndMsgData();
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)message.GetLParam(dataType);

                    // 人员工号
                    string workID = (string)GeneralFunction.IntPtrToClass(msg.ObjectMessage, msg.BytesOfObjectMessage);

                    PositioningRecord(workID);

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
        /// 改变组件大小
        /// </summary>
        private void UserControlPersonnelHistory_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UserControlPersonnelHistory_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="id">定位用的身份证号码</param>
        public void PositioningRecord(string id)
        {
            if (Convert.ToInt32(id) > 0)
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
                    if (dataGridView1.Rows[i].Cells["员工编号"].Value.ToString() == id)
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
        private void RefreshControl()
        {
            if (!m_personnerServer.GetAllChangeBill(out m_queryResult, out error))
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            dataGridView1.DataSource = dt;

            if (dt != null && dt.Rows.Count > 0)
            {
                //dataGridView1.Columns["照片"].Visible = false;
                //dataGridView1.Columns["附件"].Visible = false;
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
        /// 绑定控件
        /// </summary>
        void BindControl()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                personnelChange = m_personnerServer.GetPersonnelChangeInfo(dataGridView1.CurrentRow.Cells["编号"].Value.ToString());
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            BindControl();

            FormPersonnelChangeList frm = new FormPersonnelChangeList(personnelChange);
            frm.ShowDialog();

            RefreshControl();
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void 导出toolStripButton2_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "人员档案变更历史查询";
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
