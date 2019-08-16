using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using PlatformManagement;
using System.Reflection;
using UniversalControlLibrary;


namespace Expression
{
    public partial class 集成报表 : Form
    {
        string[] m_strList = 
        {   
            "材料收发存汇总表", 
            "BOM表物品最新单价",
            "看板发料信息汇总表",
            "库房收发存报表", 
            "成品库房工作表", 
            "售后账存信息",
            "库存资金实时查询",
            "车间收发存汇总表",
            "车间耗用报表",
            "车间工作量统计报表"
        };

        DataTable m_dtComBox = new DataTable();

        DateTimePicker m_dtpShow = new DateTimePicker();

        ComboBox m_cmbShow = new ComboBox();

        TextBoxShow m_txtShow = new TextBoxShow();

        DataTable m_dataSource = new DataTable();

        /// <summary>
        /// 查找条件字段列表
        /// </summary>
        List<string> m_lstFindField = new List<string>();

        /// <summary>
        /// 数据集
        /// </summary>
        BASE_IntegrationReport m_lnqIntegration = new BASE_IntegrationReport();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_strErr = "";

        /// <summary>
        /// NodeTag数组
        /// </summary>
        DataTable m_dtNodeTag = new DataTable();

        /// <summary>
        /// 报表服务组件
        /// </summary>
        IReport m_serverReport = ServerModuleFactory.GetServerModule<IReport>();

        IBomServer m_bomService = ServerModuleFactory.GetServerModule<IBomServer>();

        public 集成报表()
        {
            InitializeComponent();

            DataTable dtTemp = m_serverReport.GetReportTree(null);

            if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.业务系统管理员.ToString()))
            {
                treeView1.ContextMenuStrip = null;

                if (!BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
                {
                    dtTemp = m_serverReport.GetReportTree(BasicInfo.DeptCode);
                }
            }

            GlobalObject.GeneralFunction.LoadTreeViewDt(treeView1, dtTemp,
               "ReportName", "ReportCode", "RootSign", "RootSign = 'Root'");
            treeView1.ExpandAll();

            m_dtpShow.ValueChanged += new EventHandler(m_dtpShow_ValueChanged);
            dataGridView1.Controls.Add(m_dtpShow);
            m_dtpShow.Visible = false;

            m_cmbShow.DropDownStyle = ComboBoxStyle.DropDownList;//设置combox编辑模式
            m_cmbShow.SelectedValueChanged += new EventHandler(m_cmbShow_SelectedValueChanged);
            dataGridView1.Controls.Add(m_cmbShow);
            m_cmbShow.Visible = false;
        }

        void BandCombox()
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex,
                dataGridView1.CurrentCell.RowIndex, false);

            m_cmbShow.Left = rect.Left;
            m_cmbShow.Top = rect.Top;
            m_cmbShow.Width = rect.Width;
            m_cmbShow.Height = rect.Height;
            m_cmbShow.Visible = true;

        }

        void BandDateTimePick()
        {
            Rectangle rect = dataGridView1.GetCellDisplayRectangle(dataGridView1.CurrentCell.ColumnIndex,
               dataGridView1.CurrentCell.RowIndex, false);

            m_dtpShow.Left = rect.Left;
            m_dtpShow.Top = rect.Top;
            m_dtpShow.Width = rect.Width;
            m_dtpShow.Height = rect.Height;
            m_dtpShow.Visible = true;
        }

        void m_dtpShow_ValueChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }

            dataGridView1.CurrentCell.Value = m_dtpShow.Value.ToShortDateString();
        }

        void m_cmbShow_SelectedValueChanged(Object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }

            dataGridView1.CurrentCell.Value = m_cmbShow.Text;
            dataGridView1.CurrentCell.Tag = m_cmbShow.SelectedValue;
        }

        private void dataGridView1_CurrentCellChanged(object sender, EventArgs e)
        {
            if (dataGridView1.CurrentCell == null)
            {
                return;
            }

            if (dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].Cells["参数类型"].Value.ToString() == "DateTime"
                && dataGridView1.CurrentCell.ColumnIndex == 1)
            {
                if (dataGridView1.CurrentCell.Value == null || dataGridView1.CurrentCell.Value.ToString() == "")
                {
                    dataGridView1.CurrentCell.Value = DateTime.Now.ToShortDateString();
                    m_dtpShow.Value = DateTime.Now;
                }
                else
                {
                    m_dtpShow.Value = Convert.ToDateTime(dataGridView1.CurrentCell.Value);
                }

                BandDateTimePick();
            }
            else
            {
                m_dtpShow.Visible = false;
            }

            if (m_strList.Contains(treeView1.SelectedNode.Text)
                && dataGridView1.CurrentCell.ColumnIndex == 1
                && dataGridView1.CurrentCell.RowIndex == 0)
            {
                if (dataGridView1.CurrentCell.Value == null || dataGridView1.CurrentCell.Value.ToString() == "")
                {
                    dataGridView1.CurrentCell.Value = m_cmbShow.Text;
                    dataGridView1.CurrentCell.Tag = m_cmbShow.SelectedValue;
                }
                else
                {
                    m_cmbShow.Text = dataGridView1.CurrentCell.Value.ToString();
                    m_cmbShow.SelectedValue = dataGridView1.CurrentCell.Tag.ToString();
                }

                BandCombox();
            }
            else
            {
                m_cmbShow.Visible = false;
            }
        }

        void SpecialHandling()
        {
            m_cmbShow.ValueMember = null;//设置combox值
            m_cmbShow.DisplayMember = null;//设置combox显示值
            m_cmbShow.DataSource = null;//将数据表绑定到combox中
            m_cmbShow.SelectedValue = null;
            m_cmbShow.Text = null;

            DataTable dt = new DataTable();
            DataRow dr = null;

            switch (treeView1.SelectedNode.Text)
            {
                case "材料收发存汇总表":
                    dt = UniversalFunction.GetStorageTb();
                    dr = dt.NewRow();
                    dr["StorageName"] = "全部库房";
                    dr["StorageID"] = "00";
                    dt.Rows.Add(dr);

                    m_cmbShow.ValueMember = "StorageID";//设置combox值
                    m_cmbShow.DisplayMember = "StorageName";//设置combox显示值
                    m_cmbShow.DataSource = dt;//将数据表绑定到combox中
                    m_cmbShow.SelectedValue = "00";
                    m_cmbShow.Text = "全部库房";
                    break;
                case "库房收发存报表":
                case "售后账存信息":
                    dt = UniversalFunction.GetStorageTb();

                    m_cmbShow.ValueMember = "StorageID";//设置combox值
                    m_cmbShow.DisplayMember = "StorageName";//设置combox显示值
                    m_cmbShow.DataSource = dt;//将数据表绑定到combox中
                    break;
                case "BOM表物品最新单价":
                case "成品库房工作表":
                case "看板发料信息汇总表":
                    m_cmbShow.ValueMember = "GoodsCode";//设置combox值
                    m_cmbShow.DisplayMember = "GoodsCode";//设置combox值
                    m_cmbShow.DataSource = m_bomService.GetAssemblyTypeList();//将数据表绑定到combox中
                    //m_cmbShow.SelectedValue = "RDC15-FB";
                    m_cmbShow.Text = "RDC15-FB";
                    break;
                case "库存资金实时查询":
                    dt = UniversalFunction.GetStorageTb();
                    dr = dt.NewRow();
                    dr["StorageID"] = "";
                    dr["StorageName"] = "所有库房";
                    dt.Rows.Add(dr);

                    m_cmbShow.ValueMember = "StorageID";//设置combox值
                    m_cmbShow.DisplayMember = "StorageName";//设置combox显示值
                    m_cmbShow.DataSource = dt;//将数据表绑定到combox中
                    break;
                case "车间收发存汇总表":
                case "车间耗用报表":
                case "车间工作量统计报表":
                    dt = UniversalFunction.GetWorkShopTb();
                    dr = dt.NewRow();
                    dr["WSCode"] = "";
                    dr["WSName"] = "所有车间";
                    dt.Rows.Add(dr);

                    m_cmbShow.ValueMember = "WSCode";//设置combox值
                    m_cmbShow.DisplayMember = "WSName";//设置combox显示值
                    m_cmbShow.DataSource = dt;//将数据表绑定到combox中\
                    break;
                default:
                    break;
            }
        }

        DataTable SpecialHandlingDate()
        {
            switch (treeView1.SelectedNode.Text)
            {
                case "材料收发存汇总表":
                case "库房收发存报表":
                case "售后账存信息":
                case "车间收发存汇总表":
                case "车间耗用报表":
                case "车间工作量统计报表":
                    DataTable dtTemp = ((DataTable)dataGridView1.DataSource).Copy();
                    dtTemp.Rows[0][1] = dataGridView1.Rows[0].Cells[1].Tag.ToString();
                    return dtTemp;
                default:
                    break;
            }

            return (DataTable)dataGridView1.DataSource;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TreeNode Node = treeView1.SelectedNode;

                if (MessageDialog.ShowEnquiryMessage("是否要删除[" + Node.Text + "],同时会删除此类型的下级所有类型,是否要继续？")
                    == DialogResult.Yes)
                {
                    if (!m_serverReport.DeleteReportInfo(Node.Tag.ToString(),out m_strErr))
                    {
                        MessageDialog.ShowErrorMessage(m_strErr);
                        return;
                    }
                    else
                    {
                        treeView1.Nodes.Remove(Node);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void 编辑ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            集成报表数据编辑 form = new 集成报表数据编辑(treeView1.SelectedNode.Tag.ToString(), 1);

            form.ShowDialog();

            if (form.BlIsSave)
            {
                TreeNode node = treeView1.SelectedNode;

                node.Text = form.LnqReport.ReportName;
                node.Tag = form.LnqReport.ReportCode;

                treeView1.SelectedNode = node;

                treeView1_AfterSelect(sender, null);
            }
        }

        private void 添加ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            集成报表数据编辑 form = new 集成报表数据编辑(treeView1.SelectedNode.Tag.ToString(), 0);

            form.ShowDialog();

            if (form.BlIsSave)
            {
                TreeNode node = treeView1.SelectedNode;
                TreeNode NewNode = new TreeNode();

                NewNode.Text = form.LnqReport.ReportName;
                NewNode.Tag = form.LnqReport.ReportCode;

                node.Nodes.Add(NewNode);
                treeView1.SelectedNode = NewNode;

                treeView1_AfterSelect(sender, null);
            }
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            m_lnqIntegration = m_serverReport.GetReportInfo(treeView1.SelectedNode.Tag.ToString());
            SpecialHandling();
            dataGridView1.DataSource = m_serverReport.GetFindInfo(m_lnqIntegration.ReportCode);
            dataGridView1.Columns["查询内容"].Width = 200;
            dataGridView1.Columns["查询字段"].Width = 150;
            dataGridView1.Columns["查询字段格式"].Width = 200;
            dataGridView1.Columns["参数名"].Visible = false;
            dataGridView1.Columns["参数类型"].Visible = false;

            for (int i = 0; i < this.dataGridView1.Columns.Count; i++)
            {
                this.dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView2.DataSource = null;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            m_dataSource = m_serverReport.QueryInfo(m_lnqIntegration.ReportCode, SpecialHandlingDate(), out m_strErr);

            if (m_dataSource == null)
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                m_lstFindField.Clear();

                DataColumnCollection columns = m_dataSource.Columns;

                if (columns.Count > 0)
                {
                    for (int i = 0; i < columns.Count; i++)
                    {
                        m_lstFindField.Add(columns[i].ColumnName);
                    }
                }

                dataGridView2.DataSource = m_dataSource;
                userControlDataLocalizer1.Init(this.dataGridView2, this.Name,
                    UniversalFunction.SelectHideFields(this.Name, this.dataGridView2.Name, BasicInfo.LoginID));
            }
        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView2);
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                if (m_lnqIntegration.PrintName.ToString().Length == 0)
                {
                    MessageDialog.ShowPromptMessage("此报表未设置关联打印报表名");
                    return;
                }
                
                string strFullName = "";

                foreach (var item in Assembly.Load("Expression").GetTypes())
                {
                    if (item.FullName.Contains(m_lnqIntegration.PrintName.ToString()))
                    {
                        strFullName = item.FullName;
                    }
                }

                object[] args = new object[dataGridView1.Rows.Count];

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    args[i] = GlobalObject.GeneralFunction.ChangeType(dataGridView1.Rows[i].Cells["查询内容"].Value.ToString(),
                        dataGridView1.Rows[i].Cells["参数类型"].Value.ToString());
                }

                // 实例化窗体
                Form form = Assembly.Load("Expression").CreateInstance(strFullName,true,BindingFlags.Default,null,args,null,null) as Form;
                form.Show();

            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage("此报表无效,请重新设置\n" + ex.Message);
                return;
            }

        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == 1)
            {
                dataGridView1.ReadOnly = false;
            }
            else
            {
                dataGridView1.ReadOnly = true;
            }
        }

        private void dataGridView2_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
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

        private void btnSift_Click(object sender, EventArgs e)
        {
            FormFindCondition formFindCondition = new FormFindCondition(m_lstFindField.ToArray());

            if (formFindCondition.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DataTable dtTemp = GlobalObject.DataSetHelper.SiftDataTable(m_dataSource, 
                formFindCondition.SearchSQL, out m_strErr);

            if (dtTemp == null)
            {
                MessageDialog.ShowPromptMessage(m_strErr);
            }
            else
            {
                dataGridView2.DataSource = dtTemp;
            }
        }

        private void dataGridView2_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dataGridView2 == null || dataGridView2.Rows == null)
            {
                return;
            }

            lbRowCount.Text = dataGridView2.Rows.Count.ToString();
        }
    }
}
