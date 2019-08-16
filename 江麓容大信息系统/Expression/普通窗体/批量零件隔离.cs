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
    /// <summary>
    /// 批量隔离产品界面
    /// </summary>
    public partial class 批量零件隔离 : Form
    {
        /// <summary>
        /// 库房信息服务组件
        /// </summary>
        IStoreServer m_serverStore = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 单据类消息发布器
        /// </summary>
        IBillMessagePromulgatorServer m_billMessageServer = BasicServerFactory.GetServerModule<IBillMessagePromulgatorServer>();

        /// <summary>
        /// 单据编号控制类
        /// </summary>
        BillNumberControl m_billNoControl;

        /// <summary>
        /// 数据集
        /// </summary>
        S_IsolationManageBill m_lnqIslation = new S_IsolationManageBill();

        /// <summary>
        /// 处理部门编码
        /// </summary>
        string m_strCLBM = "";

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 服务类
        /// </summary>
        IIsolationManageBill m_serverIsolation = ServerModuleFactory.GetServerModule<IIsolationManageBill>();

        public 批量零件隔离()
        {
            InitializeComponent();

            m_billMessageServer.BillType = "不合格品隔离处置单";

            m_billNoControl = new BillNumberControl(CE_BillTypeEnum.不合格品隔离处置单, m_serverIsolation);

            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = -1;
        }

        void txtName_OnCompleteSearch()
        {
            txtName.Text = txtName.DataResult["物品名称"].ToString();
            txtName.Tag = txtName.DataResult["序号"].ToString();
            txtCode.Text = txtName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtName.DataResult["规格"].ToString();
        }

        private void btOut_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btFind_Click(object sender, EventArgs e)
        {

            if (txtName.Tag == null || txtName.Tag.ToString() == "" || txtName.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择物品名称");
                return;
            }
            else if (cmbStorage.Text == "")
            {
                MessageDialog.ShowPromptMessage("请选择所属库房");
                return;
            }

            DataTable dt = m_serverStore.GetGoodsStockInfo(Convert.ToInt32(txtName.Tag),
                UniversalFunction.GetStorageID(cmbStorage.Text),0);


            dataGridView1.DataSource = dt;
        }

        /// <summary>
        /// 数据填充
        /// </summary>
        void GetMessage()
        {
            m_lnqIslation.DJZT = "新建单据";
            m_lnqIslation.GoodsID = Convert.ToInt32(txtName.Tag);
            m_lnqIslation.StorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            m_lnqIslation.IsolateReason = txtReason.Text;
            m_lnqIslation.IsolateMeansAndAsk = txtMeansAndAsk.Text;
            m_lnqIslation.CLBM = m_strCLBM;
        }

        private void btCreat_Click(object sender, EventArgs e)
        {
            if (txtReason.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写隔离原因");
                return;
            }
            else if (txtMeansAndAsk.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写处理方法和要求");
                return;
            }


            不合格品处理部门 form = new 不合格品处理部门();
            form.ShowDialog();

            if (!form.BlFlag)
            {
                MessageDialog.ShowPromptMessage("请选择要求的处理部门");
                return;
            }
            else
            {
                m_strCLBM = form.StrCLBM;
            }

            GetMessage();

            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if (dt.Rows[i]["选"].ToString() != "0")
                {
                    m_lnqIslation.DJH = m_billNoControl.GetNewBillNo();
                    m_lnqIslation.BatchNo = dt.Rows[i]["批次号"].ToString();
                    m_lnqIslation.Amount = Convert.ToDecimal(dt.Rows[i]["库存数量"]);
                    m_lnqIslation.Provider = dt.Rows[i]["供货单位"].ToString();

                    if (m_serverIsolation.UpdateBill(m_lnqIslation, false, out m_err))
                    {
                        m_billMessageServer.DestroyMessage(m_lnqIslation.DJH);
                        m_billMessageServer.SendNewFlowMessage(m_lnqIslation.DJH,
                            string.Format("{0}号不合格品隔离处置单已提交，请等待主管审核",m_lnqIslation.DJH),
                            BillFlowMessage_ReceivedUserType.角色, m_billMessageServer.GetSuperior(CE_RoleStyleType.上级领导, BasicInfo.LoginID));
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_err);
                        return;
                    }
                }
            }

            this.Close();

        }

        private void 全部选中ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = 1;
                dt.Rows[i]["选"] = 1;
            }

            dataGridView1.DataSource = dt;
        }

        private void 全部取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)dataGridView1.DataSource;

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = 0;
                dt.Rows[i]["选"] = 0;
            }

            dataGridView1.DataSource = dt;
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
