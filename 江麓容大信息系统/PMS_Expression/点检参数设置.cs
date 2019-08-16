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
    public partial class 点检参数设置 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// RB值
        /// </summary>
        string m_strRbValue = "";

        /// <summary>
        /// 服务组件
        /// </summary>
        IInspectionSetInfo m_serverInspection = PMS_ServerFactory.GetServerModule<IInspectionSetInfo>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 适用CVT型号列表
        /// </summary>
        List<string> m_lstCVTType = new List<string>();

        public 点检参数设置(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            RefrshData();
        }

        private void 点检参数设置_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        void RefrshData()
        {
            dataGridView1.DataSource = m_serverInspection.GetContentInfo();
            dataGridView1.Columns["ContentID"].Visible = false;
            dataGridView1.Columns["点检设备或零件"].Width = 180;

            RefrshItemData();
        }

        /// <summary>
        /// 刷新点检项目数据
        /// </summary>
        void RefrshItemData()
        {
            int intContentID = 0;

            if (dataGridView1.CurrentRow == null)
            {
                intContentID = 0;
            }
            else
            {
                intContentID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ContentID"].Value);
            }

            dataGridView2.DataSource = m_serverInspection.GetItemInfo(intContentID);
            dataGridView2.Columns["ItemID"].Visible = false;
            dataGridView2.Columns["ContentID"].Visible = false;
            dataGridView2.Columns["点检项目名称"].Width = 250;
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="id">定位用的id号</param>
        public void PositioningContentRecord(string id)
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
                if (dataGridView1.Rows[i].Cells["ContentID"].Value.ToString() == id)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="id">定位用的id号</param>
        public void PositioningItemRecord(string id)
        {
            string strColName = "";

            foreach (DataGridViewColumn col in dataGridView2.Columns)
            {
                if (col.Visible)
                {
                    strColName = col.Name;
                    break;
                }
            }

            for (int i = 0; i < dataGridView2.Rows.Count; i++)
            {
                if (dataGridView2.Rows[i].Cells["ItemID"].Value.ToString() == id)
                {
                    dataGridView2.FirstDisplayedScrollingRowIndex = i;
                    dataGridView2.CurrentCell = dataGridView2.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        void RadioButtonControl()
        {
            if (rbHourOnce.Checked || rbSquadOnce.Checked)
            {
                numRate.Value = 0;
                numRate.Enabled = true;

            }
            else
            {

                numRate.Value = 0;
                numRate.Enabled = false;
            }
        }

        /// <summary>
        /// 设置RB
        /// </summary>
        /// <param name="modeName"></param>
        void SetRadioButtonValue(string modeName)
        {
            foreach (Control cl in panel4.Controls)
            {
                if (cl is RadioButton && cl.Text == modeName)
                {
                    ((RadioButton)cl).Checked = true;
                    return;
                }
                else if (cl is RadioButton)
                {
                    ((RadioButton)cl).Checked = false;
                }
            }
        }

        /// <summary>
        /// 获得RB值
        /// </summary>
        void GetRadioButtonValue()
        {
            foreach (Control cl in panel4.Controls)
            {
                if (cl is RadioButton && ((RadioButton)cl).Checked)
                {
                    m_strRbValue = cl.Text;
                    return;
                }
            }
        }

        private void btnContentAdd_Click(object sender, EventArgs e)
        {
            if (m_lstCVTType.Count == 0
                && MessageDialog.ShowEnquiryMessage("此记录未选择任何CVT适用型号,是否继续?") == DialogResult.No)
            {
                return;
            }

            ZPX_InspectionContentSet lnqContent = new ZPX_InspectionContentSet();

            lnqContent.InspectionContent = txtInspectionContent.Text;
            lnqContent.WorkBench = txtWorkBench.Text;
            lnqContent.WorkID = BasicInfo.LoginID;
            lnqContent.Date = ServerTime.Time;
            lnqContent.Msrepl_tran_version = Guid.NewGuid();

            if (!m_serverInspection.AddContent(lnqContent, m_lstCVTType, out m_strErr))
            {
                MessageBox.Show(m_strErr);
            }

            RefrshData();

            PositioningContentRecord(m_serverInspection.GetSingleContentInfo(lnqContent).ID.ToString());
        }

        private void btnItemAdd_Click(object sender, EventArgs e)
        {

            ZPX_InspectionItemSet lnqItemSet = new ZPX_InspectionItemSet();

            lnqItemSet.ContentID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ContentID"].Value);
            lnqItemSet.ItemName = txtInspectionItem.Text;
            lnqItemSet.Rate = numRate.Value;
            GetRadioButtonValue();
            lnqItemSet.Mode = m_serverInspection.GetInspectionModeID(m_strRbValue);
            lnqItemSet.MaxValue = numMaxValue.Value;
            lnqItemSet.MinValue = numMinValue.Value;
            lnqItemSet.WorkID = BasicInfo.LoginID;
            lnqItemSet.Date = ServerTime.Time;

            if (!m_serverInspection.AddItem(lnqItemSet, out m_strErr))
            {
                MessageBox.Show(m_strErr);
            }

            RefrshItemData();

            PositioningItemRecord(m_serverInspection.GetSingleItemInfo(lnqItemSet).ID.ToString());
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtWorkBench.Text = dataGridView1.CurrentRow.Cells["工位"].Value.ToString();
                txtInspectionContent.Tag = dataGridView1.CurrentRow.Cells["ContentID"].Value;
                txtInspectionContent.Text = dataGridView1.CurrentRow.Cells["点检设备或零件"].Value.ToString();
                m_lstCVTType = m_serverInspection.GetListForCVTType(Convert.ToInt32(txtInspectionContent.Tag));
                RefrshItemData();
            }
        }

        private void btnContentDelete_Click(object sender, EventArgs e)
        {
            if (MessageDialog.ShowEnquiryMessage("您确定要删除吗？") == DialogResult.Yes)
            {
                if (!m_serverInspection.DeleteContent(Convert.ToInt32(dataGridView1.CurrentRow.Cells["ContentID"].Value), out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功");
                }
            }

            RefrshData();
        }

        private void btnItemDelete_Click(object sender, EventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定要删除吗？") == DialogResult.Yes)
            {
                if (!m_serverInspection.DeleteItem(Convert.ToInt32(dataGridView2.CurrentRow.Cells["ItemID"].Value), out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功");
                }
            }

            RefrshItemData();
        }

        private void dataGridView2_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.CurrentRow == null)
            {
                return;
            }
            else
            {
                txtInspectionItem.Text = dataGridView2.CurrentRow.Cells["点检项目名称"].Value.ToString();
                txtInspectionItem.Tag = dataGridView2.CurrentRow.Cells["ItemID"].Value;
                SetRadioButtonValue(dataGridView2.CurrentRow.Cells["点检类型"].Value.ToString());
                numMinValue.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["最小值"].Value.ToString() == "" ?
                    0 : dataGridView2.CurrentRow.Cells["最小值"].Value);
                numMaxValue.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["最大值"].Value.ToString() == "" ?
                    0 : dataGridView2.CurrentRow.Cells["最大值"].Value);
                numRate.Value = Convert.ToDecimal(dataGridView2.CurrentRow.Cells["频率"].Value.ToString() == "" ? 
                    0 : dataGridView2.CurrentRow.Cells["频率"].Value);
            }
        }

        private void btnContentNew_Click(object sender, EventArgs e)
        {
            txtWorkBench.Text = "";
            txtInspectionContent.Text = "";
            txtInspectionContent.Tag = null;
            m_lstCVTType = new List<string>();
;
            txtInspectionItem.Text = "";
            txtInspectionItem.Tag = null;
            numRate.Value = 0;
        }

        private void btnItemNew_Click(object sender, EventArgs e)
        {
            txtInspectionItem.Text = "";
            txtInspectionItem.Tag = null;
            numRate.Value = 0;
            SetRadioButtonValue("");
        }

        private void btnContentUpdate_Click(object sender, EventArgs e)
        {
            if (m_lstCVTType.Count == 0
                && MessageDialog.ShowEnquiryMessage("此记录未选择任何CVT适用型号,是否继续?") == DialogResult.No)
            {
                return;
            }

            ZPX_InspectionContentSet lnqContent = new ZPX_InspectionContentSet();

            lnqContent.ID = Convert.ToInt32(txtInspectionContent.Tag);
            lnqContent.InspectionContent = txtInspectionContent.Text;
            lnqContent.WorkBench = txtWorkBench.Text;
            lnqContent.Date = ServerTime.Time;
            lnqContent.WorkID = BasicInfo.LoginID;

            if (!m_serverInspection.UpdateContent(lnqContent,m_lstCVTType, out m_strErr))
            {
                MessageBox.Show(m_strErr);
            }

            RefrshData();

            PositioningContentRecord(lnqContent.ID.ToString());

        }

        private void btnItemUpdate_Click(object sender, EventArgs e)
        {

            ZPX_InspectionItemSet lnqItemSet = new ZPX_InspectionItemSet();

            lnqItemSet.ID = Convert.ToInt32(txtInspectionItem.Tag);
            lnqItemSet.ContentID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["ContentID"].Value);
            lnqItemSet.ItemName = txtInspectionItem.Text;
            lnqItemSet.Rate = numRate.Value;
            GetRadioButtonValue();
            lnqItemSet.Mode = m_serverInspection.GetInspectionModeID(m_strRbValue);
            lnqItemSet.MaxValue = numMaxValue.Value;
            lnqItemSet.MinValue = numMinValue.Value;
            lnqItemSet.WorkID = BasicInfo.LoginID;
            lnqItemSet.Date = ServerTime.Time;

            if (!m_serverInspection.UpdateItem(lnqItemSet, out m_strErr))
            {
                MessageBox.Show(m_strErr);
            }

            RefrshItemData();

            PositioningItemRecord(lnqItemSet.ID.ToString());
        }

        private void rbHourOnce_CheckedChanged(object sender, EventArgs e)
        {
            RadioButtonControl();
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

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {

            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView2.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView2.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView2.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void btnCVTType_Click(object sender, EventArgs e)
        {
            UniversalControlLibrary.FormDataCheck frm = 
                new UniversalControlLibrary.FormDataCheck(m_serverInspection.GetCVTType().AsEnumerable().Select(t => t.Field<string>("CVTType")).ToList(), m_lstCVTType);

            frm.ShowDialog();

            if (frm.DialogResult == DialogResult.OK)
            {
                m_lstCVTType = frm.LstResult;
            }
        }
    }
}
