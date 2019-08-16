using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;
using PlatformManagement;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 产品不合格隔离界面
    /// </summary>
    public partial class 产品不合格隔离 : Form
    {
        /// <summary>
        /// 不合格产品隔离服务类
        /// </summary>
        IQuarantine m_serverQuarantine = ServerModuleFactory.GetServerModule<IQuarantine>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 权限组件
        /// </summary>
        AuthorityFlag m_authFlag;

        public 产品不合格隔离(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            cmbDJ_ZT.SelectedIndex = 0;
            dateTimeStart.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dateTimeEnd.Value = ServerTime.Time.AddDays(1);

            DataGridViewBind();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetVisibly(toolStrip1, authorityFlag);
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            DataGridViewBind();
        }

        /// <summary>
        /// 绑定GridView
        /// </summary>
        public void DataGridViewBind()
        {
            DataTable dt = m_serverQuarantine.GetAllBill(dateTimeStart.Value.ToShortDateString(),
                           dateTimeEnd.Value.ToShortDateString(),cmbDJ_ZT.Text,out m_strErr);
            
            if (dt == null)
            {
                MessageDialog.ShowErrorMessage(m_strErr);
                return;
            }

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["单据号"].Width = 120;
            dataGridView1.Columns["ID"].Visible = false;
            dataGridView1.Columns["删除状态"].Visible = false;
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

        private void 产品不合格隔离_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authFlag);

            cmbDJ_ZT.SelectedIndex = 0;
            dateTimeStart.Value = new DateTime(ServerTime.Time.Year, ServerTime.Time.Month, 1);
            dateTimeEnd.Value = ServerTime.Time.AddDays(1);

            DataGridViewBind();
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            DataGridViewRow row = dataGridView1.CurrentRow;

            string DJH = row.Cells["单据号"].Value.ToString();
            string Storage = row.Cells["库房名称"].Value.ToString();
            string Quarantine = row.Cells["隔离原因"].Value.ToString();
            string LRRY = row.Cells["编制人员"].Value.ToString();
            string LRBM = row.Cells["编制部门"].Value.ToString();
            string Remark = row.Cells["备注"].Value.ToString();
            string flag = row.Cells["单据状态"].Value.ToString();
            string handle = row.Cells["处理结果"].Value.ToString();
            string disposeName = row.Cells["处理人"].Value.ToString();
            string dispose = row.Cells["处理方案"].Value.ToString();

            不合格隔离明细 frm = new 不合格隔离明细(DJH, Storage, Quarantine, LRRY, LRBM, Remark, 
                                                    flag, handle, disposeName, dispose, m_authFlag);
            frm.ShowDialog();

            DataGridViewBind();
            PositioningRecord(DJH);
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            不合格隔离明细 frm = new 不合格隔离明细(m_authFlag);
            frm.ShowDialog();

            DataGridViewBind();

            if (dataGridView1.Rows.Count > 0)
            {
                PositioningRecord(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["单据号"].Value.ToString());
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (BasicInfo.LoginName != dataGridView1.CurrentRow.Cells["编制人员"].Value.ToString())
            {
                MessageDialog.ShowPromptMessage("您不是此记录的编制者无法进行此操作");
                return;
            }

            string handle = dataGridView1.CurrentRow.Cells["单据状态"].Value.ToString();

            if (!handle.Equals("已解封"))
            {
                string djh = dataGridView1.CurrentRow.Cells["单据号"].Value.ToString();

                if (MessageDialog.ShowEnquiryMessage("是否要删除【" + djh + "】号单据?") == DialogResult.No)
                {
                    return;
                }

                bool b = m_serverQuarantine.DeleteBill(djh, out m_strErr);

                if (b)
                {
                    DataTable dt = m_serverQuarantine.GetList(djh, out m_strErr);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        string Code = dt.Rows[i]["ProductStockCode"].ToString();
                        string GoodID = dt.Rows[i]["GoodID"].ToString();

                        bool delete = m_serverQuarantine.UpdateProductStockStatus(Code, GoodID, true, out m_strErr);

                        if (delete)
                        {
                            MessageDialog.ShowPromptMessage("单据删除成功！");
                        }
                    }
                }
                else
                {
                    MessageDialog.ShowErrorMessage(m_strErr);
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("不能进行此操作！");
            }

            DataGridViewBind();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            FormSeeInsulate frm = new FormSeeInsulate(m_authFlag);

            frm.ShowDialog();

            DataGridViewBind();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DataGridViewBind();
        }

        private void dataGridView1_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView1.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView1.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView1.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
