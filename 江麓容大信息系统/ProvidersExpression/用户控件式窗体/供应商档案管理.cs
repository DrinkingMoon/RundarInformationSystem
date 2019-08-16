using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ProvidersServerModule;
using UniversalControlLibrary;
using GlobalObject;
using ServerModule;

namespace ProvidersExpression
{
    public partial class 供应商档案管理 : Form
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
        /// 查询结果
        /// </summary>
        IQueryResult m_queryResult;

        /// <summary>
        /// 供应商档案服务类
        /// </summary>
        IProvidersBaseServer m_providerServer = ProvidersServerModule.ServerModuleFactory.GetServerModule<IProvidersBaseServer>();

        public 供应商档案管理(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
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
        /// 窗体加载
        /// </summary>
        private void 供应商档案管理_Load(object sender, EventArgs e)
        {
            AuthorityControl(m_authorityFlag);
            RefreshControl();
        }

        /// <summary>
        /// 刷新数据
        /// </summary>
        private void RefreshControl()
        {
            if (!m_providerServer.GetAllBill(out m_queryResult, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            m_queryResult.DataGridView = dataGridView1;

            DataTable dt = m_queryResult.DataCollection.Tables[0];

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);
                       
            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();

            dataGridView1.Columns[0].Frozen = true;
            dataGridView1.Columns[1].Frozen = true;
        }

        private void 新建toolStripButton_Click(object sender, EventArgs e)
        {
            供应商基础信息显示 frm = new 供应商基础信息显示(m_authorityFlag,"");

            frm.ShowDialog();
        }

        /// <summary>
        /// 重绘窗体，让标题居中
        /// </summary>
        private void 供应商档案管理_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 导入toolStripButton_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1);

            if (dtTemp == null)
            {
                //MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            if (!m_providerServer.InsertProvidersInfo(dtTemp, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
            }
        }

        /// <summary>
        /// 检查Excel表的数据
        /// </summary>
        /// <param name="dtcheck">表</param>
        /// <returns>返回是否正确</returns>
        bool CheckTable(DataTable dtcheck)
        {
            if (!dtcheck.Columns.Contains("供应商简码"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【供应商简码】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("供应商全称"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【供应商全称】信息");
                return false;
            }

            if (!dtcheck.Columns.Contains("职务"))
            {
                MessageDialog.ShowPromptMessage("此文件中不包含【职务】信息");
                return false;
            }

            return true;
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshControl();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                供应商基础信息显示 frm = new 供应商基础信息显示(m_authorityFlag, dataGridView1.CurrentRow.Cells["供应商编号"].Value.ToString());

                frm.ShowDialog();
            }

            RefreshControl();
        }

        private void 导出toolStripButton_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
