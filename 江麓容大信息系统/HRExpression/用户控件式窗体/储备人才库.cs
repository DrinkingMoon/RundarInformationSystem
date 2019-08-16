using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using UniversalControlLibrary;
using Service_Peripheral_HR;
using ServerModule;
using GlobalObject;

namespace Form_Peripheral_HR
{
    public partial class 储备人才库 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 操作权限
        /// </summary>
        PlatformManagement.AuthorityFlag m_authorityFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 储备人才管理类
        /// </summary>
        ITrainEmployeServer m_trainEmployeServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<ITrainEmployeServer>();

        public 储备人才库(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
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

        private void 储备人才库_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void RefreshControl()
        {
            if (!m_trainEmployeServer.GetAllInfo(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryResult.DataGridView = dataGridView1;

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            if (dt != null && dt.Rows.Count > 0)
            {
                dataGridView1.Columns["序号"].Visible = false;
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

            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.Columns[1].Frozen = true;
        }

        /// <summary>
        /// 改变标题文本的距离
        /// </summary>
        private void 储备人才库_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

        }

        private void 新建toolStripButton1_Click(object sender, EventArgs e)
        {
            储备人才库明细 frm = new 储备人才库明细(0);

            frm.ShowDialog();
            RefreshControl();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);

            储备人才库明细 frm = new 储备人才库明细(id);

            frm.ShowDialog();
            RefreshControl();
            PositioningRecord(id.ToString());
        }

        private void 综合查询toolStripButton3_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "查看储备人才库";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);

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
                if (dataGridView1.Rows[i].Cells["序号"].Value.ToString() == billNo)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }
    }
}
