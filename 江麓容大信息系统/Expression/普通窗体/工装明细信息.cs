using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using PlatformManagement;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 工装明细信息 : Form
    {
        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 物品ID
        /// </summary>
        int m_intGoodsID = 0;

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_msgPromulgator = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        /// <summary>
        /// 服务组件
        /// </summary>
        IFrockStandingBook m_serverFrockStandingBook = PMS_ServerFactory.GetServerModule<IFrockStandingBook>();

        public 工装明细信息(int goodsID, AuthorityFlag authFlag)
        {
            InitializeComponent();

            m_msgPromulgator.BillType = CE_BillTypeEnum.工装台帐.ToString();

            m_authFlag = authFlag;

            m_intGoodsID = goodsID;

            FaceAuthoritySetting.SetEnable(this.Controls, authFlag);
            FaceAuthoritySetting.SetVisibly(this.menuStrip1, authFlag);

            menuStrip1.Visible = true;

            dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="frocknumber">单号</param>
        /// <param name="gooodsid">物品ID</param>
        void PositioningRecord(string frocknumber, int gooodsid)
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
                if ((string)dataGridView1.Rows[i].Cells["工装编号"].Value == frocknumber
                    && (int)dataGridView1.Rows[i].Cells["物品ID"].Value == gooodsid)
                {
                    dataGridView1.FirstDisplayedScrollingRowIndex = i;
                    dataGridView1.CurrentCell = dataGridView1.Rows[i].Cells[strColName];
                    break;
                }
            }
        }

        private void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            int intGoodsID = Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value);

            string strFrockNumber = dataGridView1.CurrentRow.Cells["工装编号"].Value.ToString();

            工装总成信息 form = new 工装总成信息(intGoodsID, strFrockNumber, true, m_authFlag, false);
            form.ShowDialog();

            dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
            PositioningRecord(strFrockNumber, intGoodsID);
        }

        private void 新建ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            工装总成信息 form = new 工装总成信息(0, "", true, m_authFlag, true);
            form.ShowDialog();

            dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
        }

        private void 刷新ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentRow == null)
            {
                return;
            }
            else
            {

                if (MessageDialog.ShowEnquiryMessage("您是否要删除此工装信息") == DialogResult.No)
                {
                    return;
                }

                if (!m_serverFrockStandingBook.DeleteFrockStandingBook(Convert.ToInt32(dataGridView1.CurrentRow.Cells["物品ID"].Value),
                    dataGridView1.CurrentRow.Cells["工装编号"].Value.ToString(), out m_strErr))
                {
                    MessageDialog.ShowPromptMessage(m_strErr);
                }
                else
                {
                    MessageDialog.ShowPromptMessage("删除成功");
                    dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
                }
            }
        }

        private void chkIsShowInStock_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void chkIsShowDispensingScrapInfo_CheckedChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = m_serverFrockStandingBook.GetAllTable(chkIsShowDispensingScrapInfo.Checked, chkIsShowInStock.Checked, m_intGoodsID);
            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
