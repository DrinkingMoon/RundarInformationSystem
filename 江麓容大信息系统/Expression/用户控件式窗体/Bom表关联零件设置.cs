using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// BOM表关联零件设置界面
    /// </summary>
    public partial class Bom表关联零件设置 : Form
    {
        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 数据集
        /// </summary>
        P_JumblyBomGoods m_lnqJumbly = new P_JumblyBomGoods();

        /// <summary>
        /// 服务组件
        /// </summary>
        IBomServer m_bomServer = ServerModuleFactory.GetServerModule<IBomServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        public Bom表关联零件设置()
        {
            InitializeComponent();
            RefreshDataGirdView(m_bomServer.GetJumblyTable());
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
            txtName.Tag = Convert.ToInt32(txtName.DataResult["序号"].ToString());
        }

        void txtBomGoodsName_OnCompleteSearch()
        {
            txtBomGoodsName.Text = txtBomGoodsName.DataResult["零部件名称"].ToString();
            txtBomGoodsCode.Text = txtBomGoodsName.DataResult["零部件编码"].ToString();
            txtBomSpec.Text = txtBomGoodsName.DataResult["规格"].ToString();

        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="billNo">定位用的单据号</param>
        void PositioningRecord(string strbomCode,string strbomName, string strSpec,int intGoodsID)
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
                if ((string)dataGridView1.Rows[i].Cells["BOM图号型号"].Value == strbomCode
                    && (string)dataGridView1.Rows[i].Cells["BOM物品名称"].Value == strbomName
                    && (string)dataGridView1.Rows[i].Cells["BOM规格"].Value == strSpec
                    && (int)dataGridView1.Rows[i].Cells["零件物品ID"].Value == intGoodsID)
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
        void RefreshDataGirdView(DataTable source)
        {
            dataGridView1.DataSource = source;

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dataGridView1, this.Name, 
                                  UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
                panelPara.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }
        }

        /// <summary>
        /// 获得数据集信息
        /// </summary>
        void GetMessage()
        {
            m_lnqJumbly.BomGoodsCode = txtBomGoodsCode.Text;
            m_lnqJumbly.BomGoodsName = txtBomGoodsName.Text;
            m_lnqJumbly.BomSpec = txtBomSpec.Text;
            m_lnqJumbly.IsJumbly = chbIsJumbly.Checked;
            m_lnqJumbly.IsStock = chbIsStock.Checked;
            m_lnqJumbly.JumblyGoodsID = Convert.ToInt32(txtName.Tag);
            m_lnqJumbly.Quato = numericUpDown1.Value;
            m_lnqJumbly.IsMath = chbIsMath.Checked;
            m_lnqJumbly.Remark = txtRemark.Text;
        }

        /// <summary>
        /// 清空数据
        /// </summary>
        void ClearDate()
        {
            txtBomGoodsCode.Text = "";
            txtBomGoodsName.Text = "";
            txtBomSpec.Text = "";
            txtCode.Text = "";
            txtName.Text = "";
            txtSpec.Text = "";
            chbIsJumbly.Checked = false;
            chbIsStock.Checked = false;
            chbIsMath.Checked = false;
            txtName.Tag = -1;
        }

        /// <summary>
        /// 检查窗体信息
        /// </summary>
        /// <returns>检测通过返回True,检测失败返回False</returns>
        bool CheckForm()
        {
            if (txtBomGoodsName.Text == "" && txtBomGoodsCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择对应的BOM表的零件");
                return false;
            }

            if (txtName.Text == "" && txtCode.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择需要设置的物品信息");
                return false; ;
            }

            return true;
        }

        /// <summary>
        /// 检查相同记录数
        /// </summary>
        /// <returns>不相同返回True,相同返回False</returns>
        bool SameGoods()
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["BOM图号型号"].ToString() == m_lnqJumbly.BomGoodsCode
                    && dt.Rows[i]["BOM物品名称"].ToString() == m_lnqJumbly.BomGoodsName
                    && dt.Rows[i]["BOM规格"].ToString() == m_lnqJumbly.BomSpec
                    && Convert.ToInt32(dt.Rows[i]["零件物品ID"]) == Convert.ToInt32(m_lnqJumbly.JumblyGoodsID)
                    && (m_lnqJumbly.ID == 0 || Convert.ToInt32(dt.Rows[i]["序号"]) != Convert.ToInt32(m_lnqJumbly.ID)))
                {
                    return false;
                }
            }

            return true;
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                dataGridView1.DataSource = null;
                return;
            }
            else
            {
                txtBomGoodsCode.Text = dataGridView1.CurrentRow.Cells["BOM图号型号"].Value.ToString();
                txtBomGoodsName.Text = dataGridView1.CurrentRow.Cells["BOM物品名称"].Value.ToString();
                txtBomSpec.Text = dataGridView1.CurrentRow.Cells["BOM规格"].Value.ToString();
                txtCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                chbIsJumbly.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否混装"].Value);
                chbIsStock.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否采购"].Value);
                chbIsMath.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否合计库存"].Value);
                txtName.Tag = Convert.ToInt32(dataGridView1.CurrentRow.Cells["零件物品ID"].Value);
                numericUpDown1.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["配额"].Value);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckForm())
            {
                return;
            }

            GetMessage();

            if (!SameGoods())
            {
                MessageDialog.ShowPromptMessage("不能添加同种物品");
                return;
            }

            if (!m_bomServer.AddJumbly(m_lnqJumbly, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("添加成功");
            }

            RefreshDataGirdView(m_bomServer.GetJumblyTable());

            PositioningRecord(m_lnqJumbly.BomGoodsCode, m_lnqJumbly.BomGoodsName,
                m_lnqJumbly.BomSpec, Convert.ToInt32( m_lnqJumbly.JumblyGoodsID));

            ClearDate();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (!CheckForm())
            {
                return;
            }

            GetMessage();
            m_lnqJumbly.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);

            if (!SameGoods())
            {
                MessageDialog.ShowPromptMessage("不能添加同种物品");
                return;
            }

            if (!m_bomServer.UpdateJumbly(m_lnqJumbly, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("更新成功");
            }

            RefreshDataGirdView(m_bomServer.GetJumblyTable());
            PositioningRecord(m_lnqJumbly.BomGoodsCode, m_lnqJumbly.BomGoodsName,
                m_lnqJumbly.BomSpec, Convert.ToInt32( m_lnqJumbly.JumblyGoodsID));

            ClearDate();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (!CheckForm())
            {
                return;
            }

            GetMessage();
            m_lnqJumbly.ID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value);

            if (!m_bomServer.DeleteJumbly(m_lnqJumbly, out m_err))
            {
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {
                MessageDialog.ShowPromptMessage("删除成功");
            }

            RefreshDataGirdView(m_bomServer.GetJumblyTable());
            ClearDate();
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
