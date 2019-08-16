using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using PlatformManagement;
using Service_Peripheral_HR;
using ServerModule;
using GlobalObject;
using Expression;
using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    /// <summary>
    /// 人员简历主界面
    /// </summary>
    public partial class UserControlResume : Form
    {
        /// <summary>
        /// 人员简历数据集
        /// </summary>
        HR_Resume m_resume;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 人员简历管理类
        /// </summary>
        IResumeServer m_resumeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IResumeServer>();

        public UserControlResume(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authorityFlag = nodeInfo.Authority;
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
                // 要求在界面上显示特定的数据内容
                case WndMsgSender.ShowSpecificData:

                    WndMsgData msg = new WndMsgData();
                    Type dataType = msg.GetType();

                    msg = (WndMsgData)message.GetLParam(dataType);

                    // 身份证号码
                    string id = (string)GeneralFunction.IntPtrToClass(msg.ObjectMessage, msg.BytesOfObjectMessage);

                    // 在界面上显示特定的数据内容
                    PositioningRecord(id);

                    break;

                default:
                    base.DefWndProc(ref message);
                    break;
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="id">定位用的身份证号码</param>
        public void PositioningRecord(string id)
        {
            if (id !="")
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
                    if (dataGridView1.Rows[i].Cells["身份证"].Value.ToString() == id)
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
            RefreshControl(m_resumeServer.GetResume(null, null));
        }

        /// <summary>
        /// 在界面上显示特定的数据内容
        /// </summary>
        /// <param name="id">身份证号码（显示包含指定身份证号码的数据）</param>
        public void ShowSpecificData(string id)
        {
            RefreshControl(m_resumeServer.GetResume(null, null));
        }

        /// <summary>
        /// 刷新数据控件
        /// </summary>
        /// <param name="dataSource">数据源</param>
        void RefreshControl(DataTable dataSource)
        {
            dataGridView1.DataSource = dataSource;

            if (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Columns["编号"].Visible = false;
                dataGridView1.Columns["照片"].Visible = false;
                dataGridView1.Columns["附件"].Visible = false;
                dataGridView1.Columns["第三方关系"].Visible = false;

            }
            dataGridView1.Refresh();

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
        }

        /// <summary>
        /// 窗体加载
        /// </summary>
        private void UserControlResume_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        private void UserControlResume_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 刷新toolStripButton1_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        /// <summary>
        /// 绑定控件
        /// </summary>
        void BindControl()
        {
            m_resume = m_resumeServer.GetResumelInfo(dataGridView1.CurrentRow.Cells["身份证"].Value.ToString());
        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            FormResumeList frm = new FormResumeList(m_authorityFlag, null, "新建");
            frm.ShowDialog();

            RefreshControl();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            BindControl();

            FormResumeList frm = new FormResumeList(m_authorityFlag,m_resume, "查看");
            frm.ShowDialog();

            RefreshControl();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
