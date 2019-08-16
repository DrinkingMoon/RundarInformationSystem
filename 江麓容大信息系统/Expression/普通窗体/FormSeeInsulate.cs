using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
    /// 成品隔离与解封查询界面
    /// </summary>
    public partial class FormSeeInsulate : Form
    {
        /// <summary>
        /// 不合格产品隔离服务类
        /// </summary>
        IQuarantine m_serverQuarantine = ServerModuleFactory.GetServerModule<IQuarantine>();

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 箱体的处理状态
        /// </summary>
        bool m_blIsHandle;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr;

        public FormSeeInsulate(AuthorityFlag m_authFlag)
        {
            InitializeComponent();

            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
            FaceAuthoritySetting.SetVisibly(this.toolStrip1, m_authFlag);
            DataBind();

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.成品仓库管理员.ToString())
                || BasicInfo.ListRoles.Contains(CE_RoleEnum.售后库房管理员.ToString()))
            {
                dataGridView1.Columns["选"].Visible = true;
                dataGridView1.Columns["选"].ReadOnly = false;
            }

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(
                    dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                                                           this.Name, dataGridView1.Name, BasicInfo.LoginID));
                m_dataLocalizer.OnlyLocalize = true;
                panel1.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            toolStrip1.Visible = true;
        }

        /// <summary>
        /// 绑定数据集
        /// </summary>
        void DataBind()
        {
            DataTable dt = m_serverQuarantine.GetInsulateGoodsInfo();

            if (dt != null)
            {
                dataGridView1.DataSource = dt;
                dataGridView1.Columns["GoodsID"].Visible = false;
                dataGridView1.Columns["箱体编号"].ReadOnly = true;
                dataGridView1.Columns["图号型号"].ReadOnly = true;
                dataGridView1.Columns["物品名称"].ReadOnly = true;
                dataGridView1.Columns["总成状态"].ReadOnly = true;
                dataGridView1.Columns["库房名称"].ReadOnly = true;
            }
            else
            {
                MessageDialog.ShowPromptMessage("没有隔离产品！");
            }
        }

        private void 选择ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = true;
            }
        }

        private void 取消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i <= dataGridView1.SelectedRows.Count - 1; i++)
            {
                dataGridView1.SelectedRows[i].Cells["选"].Value = false;
            }
        }

        private void 全消ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = false;
            }
        }

        private void 全选ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                dataGridView1.Rows[i].Cells["选"].Value = true;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            string Bill_ID = "";
            DataTable dt = (DataTable)this.dataGridView1.DataSource;

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    DataRow drStockCode = m_serverQuarantine.GetBillID(dt.Rows[i]["箱体编号"].ToString());
                    Bill_ID = drStockCode[0].ToString();
                    DataTable dtList = m_serverQuarantine.GetOperationStatus(Bill_ID, dt.Rows[i]["箱体编号"].ToString(), out m_strErr);

                    if (!Convert.ToBoolean(dtList.Rows[0]["IsHandle"].ToString()))
                    {
                        S_QuarantineList list = new S_QuarantineList();

                        list.Bill_ID = Bill_ID;
                        list.ProductStockCode = dt.Rows[i]["箱体编号"].ToString();
                        list.GoodID = int.Parse(dt.Rows[i]["GoodsID"].ToString());

                        m_blIsHandle = m_serverQuarantine.UpdateList(list, out m_strErr);

                        if (m_blIsHandle)
                        {
                            m_blIsHandle = m_serverQuarantine.UpdateProductStockStatus(
                                dt.Rows[i]["箱体编号"].ToString(), dt.Rows[i]["GoodsID"].ToString(), true, out m_strErr);
                        }
                    }
                }

                if (m_blIsHandle)
                {
                    string str = "";
                    DataTable dtHandle = m_serverQuarantine.GetOperationStatus(Bill_ID, null, out m_strErr);

                    for (int i = 0; i < dtHandle.Rows.Count; i++)
                    {
                        string str2 = dtHandle.Rows[i]["isHandle"].ToString();

                        if (dtHandle.Rows[i]["isHandle"].ToString() == "False")
                        {
                            str = "未完成";
                            break;
                        }
                        else
                        {
                            str = "全部处理";
                        }
                    }

                    m_blIsHandle = m_serverQuarantine.AuditingBill(Bill_ID, str, "已解封", out m_strErr);

                    if (m_blIsHandle)
                    {
                        MessageDialog.ShowPromptMessage("解封成功！");
                        this.Close();
                    }
                    else
                    {
                        MessageDialog.ShowErrorMessage(m_strErr);
                        return;
                    }
                }
            }
        }
    }
}
