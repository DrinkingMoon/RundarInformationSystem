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
    /// 库房报缺查询界面
    /// </summary>
    public partial class 库房报缺查询 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 服务
        /// </summary>
        IStockLack m_serverLack = ServerModuleFactory.GetServerModule<IStockLack>();

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 所有数据列表
        /// </summary>
        DataTable m_dtAllInfo = new DataTable();

        /// <summary>
        /// 服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        public 库房报缺查询(PlatformManagement.FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            dtp_End.Value = ServerTime.Time.AddDays(1);
            m_authFlag = nodeInfo.Authority;
           
            cmbStorage.Items.Add("全部库房");
            DataTable dt = UniversalFunction.GetStorageTb();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                cmbStorage.Items.Add(dt.Rows[i]["StorageName"].ToString());
            }

            cmbStorage.SelectedIndex = 0;

            IBomServer bomService = ServerModuleFactory.GetServerModule<IBomServer>();
            List<string> tempList = bomService.GetAssemblyTypeList();

            cmbCVTType.DataSource = tempList;
            cmbCVT_Plan.DataSource = tempList;

            cmbCustomTemplates.DataSource = m_serverLack.GetCustomTemplatesMain();

            cmbCustomTemplates.ValueMember = "ID";
            cmbCustomTemplates.DisplayMember = "模板名称";
        }
        
        private void 库房报缺查询_Load(object sender, EventArgs e)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, m_authFlag);
        }

        private void dataGridView1_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewRow dr = dataGridView1.Rows[i];

                if (Convert.ToDecimal(dataGridView1.Rows[i].Cells["报缺数"].Value) < 0)
                {
                    dataGridView1.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                }
            }
        }

        private void 查询ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strStorageID = "";

            if (cmbStorage.Text == "全部库房")
            {
                strStorageID = "00";
            }
            else
            {
                strStorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            }

            switch (tabControl1.SelectedIndex)
            {
                case 0:
                    if (dtp_Start.Value.Date < ServerTime.Time.Date)
                    {
                        MessageDialog.ShowErrorMessage("查询时间不能小于当前时间！");
                        return;
                    }

                    if (dtp_Start.Value.Date > dtp_End.Value.Date)
                    {
                        MessageDialog.ShowErrorMessage("开始时间不能大于结束时间");
                        return;
                    }
                    m_dtAllInfo = m_serverLack.ReportWanting(
                        dtp_Start.Value.ToShortDateString(), dtp_End.Value.ToShortDateString(),
                         strStorageID, "ReportWanting", out m_err);
                    break;
                case 1:
                    m_serverLack.ClearTempTable();
                    CreatTempTable();
                    m_dtAllInfo = m_serverLack.ReportWanting(
                        dtp_Start.Value.ToShortDateString(), dtp_End.Value.ToShortDateString(),
                         strStorageID, "ReportWantingElse", out m_err);
                    break;
                case 2:
                    List<string> listTemp = new List<string>();

                    foreach (TreeNode item in treeView1.Nodes)
                    {
                        GetTreeViewValue(item, ref listTemp);
                    }

                    m_serverLack.SetSingleBom(listTemp);

                    m_dtAllInfo = m_serverLack.ReportWanting(numSingleCount.Value.ToString(), "",
                         strStorageID, "ReportWantingSingle", out m_err);
                    break;
                case 3:
                    m_dtAllInfo = m_serverLack.ReportWanting(numCustom.Value.ToString(), cmbCustomTemplates.SelectedValue.ToString(),
                         strStorageID, "ReportWantingCustom", out m_err);
                    break;
                default:
                    break;
            }

            DataTable tempTable = m_dtAllInfo;

            if (chbIsZero.Checked)
            {
                tempTable = GlobalObject.DataSetHelper.SiftDataTable(tempTable, "计划数 > 0", out m_err);
            }

            dataGridView1.DataSource = tempTable;

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        void GetTreeViewValue(TreeNode node, ref List<string> listInfo)
        {
            if (node.Nodes.Count == 0)
            {
                listInfo.Add(node.Tag.ToString() + "-" + node.Parent.Tag.ToString());
            }
            else
            {
                if (!node.IsExpanded)
                {
                    listInfo.Add(node.Tag.ToString() + "-" + node.Parent.Tag.ToString());
                }
                else
                {
                    foreach (TreeNode item in node.Nodes)
                    {
                        GetTreeViewValue(item, ref listInfo);
                    }
                }
            }
        }

        /// <summary>
        /// 创建临时表
        /// </summary>
        void CreatTempTable()
        {
            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
            {
                m_serverLack.AddTempTable(dgvr.Cells["总成型号"].Value.ToString(),
                    Convert.ToDecimal( dgvr.Cells["数量"].Value));
            }
        }

        private void 导出全部项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void 导出报缺项ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                DataTable dt = ((DataTable)dataGridView1.DataSource).Clone();
                DataRow[] dr = ((DataTable)dataGridView1.DataSource).Select("报缺数 < 0");

                if (dr.Length > 0)
                {
                    for (int a = 0; a <= dr.Length - 1; a++)
                    {
                        dt.ImportRow(dr[a]);
                    }
                }

                string[] hideColumns = { "物品ID" };

                ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, hideColumns);
            }
        }

        private void chbIsZero_CheckedChanged(object sender, EventArgs e)
        {
            DataTable tempTable = m_dtAllInfo;

            if (chbIsZero.Checked)
            {
                tempTable = GlobalObject.DataSetHelper.SiftDataTable(tempTable, "计划数 > 0", out m_err);
            }

            dataGridView1.DataSource = tempTable;
        }

        private void btnEditCustomTemplates_Click(object sender, EventArgs e)
        {
            库房报缺_自定义 frm = new 库房报缺_自定义();
            frm.ShowDialog();

            cmbCustomTemplates.DataSource = m_serverLack.GetCustomTemplatesMain();

            cmbCustomTemplates.ValueMember = "ID";
            cmbCustomTemplates.DisplayMember = "模板名称";
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2 && cmbCVTType.Text.Trim().Length > 0)
            {
                this.treeView1.Nodes.Clear();
                GlobalObject.GeneralFunction.LoadTreeViewDt(this.treeView1,
                    m_serverLack.GetBomTable(cmbCVTType.Text), "GoodsName", "GoodsID", "ParentID", "ParentID = '0'");
                treeView1.ExpandAll();
                splitContainer1.Panel1Collapsed = false;
            }
            else
            {
                splitContainer1.Panel1Collapsed = true;
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (cmbCVT_Plan.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请选择【总成型号】");
                return;
            }

            foreach (DataGridViewRow dgvr in customDataGridView1.Rows)
            {
                if (dgvr.Cells["总成型号"].Value.ToString() == cmbCVT_Plan.Text)
                {
                    MessageDialog.ShowPromptMessage("不能添加重复【总成型号】");
                    return;
                }
            }

            customDataGridView1.Rows.Add(new object[] { cmbCVT_Plan.Text, numCount_Plan.Value });
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            customDataGridView1.Rows.Remove(customDataGridView1.CurrentRow);
        }

        private void cmbCVTType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 2 && cmbCVTType.Text.Trim().Length > 0)
            {
                this.treeView1.Nodes.Clear();
                GlobalObject.GeneralFunction.LoadTreeViewDt(this.treeView1,
                    m_serverLack.GetBomTable(cmbCVTType.Text), "GoodsName", "GoodsID", "ParentID", "ParentID = '0'");
                treeView1.ExpandAll();
                splitContainer1.Panel1Collapsed = false;
            }
        }
    }
}
