/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormDepotStore.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 关于界面
 * 其它 :
 *----------------------------------------------------------------------------
 * 历史记录:
 *     1. 日期时间: 2009/07/03 08:02:08 作者: 夏石友 当前版本: V1.00
 *        修改说明: 创建
 ******************************************************************************/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using GlobalObject;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 货物库存界面类
    /// </summary>
    public partial class FormDepotStore : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 当前所选仓库编码
        /// </summary>
        string m_selectDepotCode;

        /// <summary>
        /// 排序顺序,True为升序,False为降序
        /// </summary>
        bool m_sequence = true;

        /// <summary>
        /// 查找到的符合条件的库存信息
        /// </summary>
        IQueryable<View_S_Stock> m_findStore;

        /// <summary>
        /// 仓库信息列表
        /// </summary>
        List<MaterialTypeData> m_listDepotInfo = new List<MaterialTypeData>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IMaterialTypeServer m_depotServer = ServerModuleFactory.GetServerModule<IMaterialTypeServer>();

        /// <summary>
        /// 服务组件
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        /// <summary>
        /// 人员对仓库的访问权限列表
        /// </summary>
        List<View_S_DepotForPersonnel> m_depotForPersonnel;

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        public FormDepotStore()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw |
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);

            UpdateStyles();

            m_depotForPersonnel = m_storeServer.GetDepotForPersonnel(BasicInfo.LoginID.ToString()).ToList();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDepotStore_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((panelTitle.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormDepotStore_Load(object sender, EventArgs e)
        {
            InitTreeView();
        }

        /// <summary>
        /// 初始化treeView
        /// </summary>
        void InitTreeView()
        {
            // 清除树节点
            this.treeView.Nodes.Clear();

            // 添加一个根节点
            TreeNode parentNode = new TreeNode();

            parentNode.Name = "仓库";
            parentNode.Text = "仓库";

            treeView.Nodes.Add(parentNode);

            

            List<MaterialTypeData> depotInfo;

            // 获取所有仓库信息
            if (!m_depotServer.GetAllMaterialType(out depotInfo, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            int len = 0;

            for (int i = 0; i < depotInfo.Count; i++)
            {
                len = depotInfo[i].MaterialTypeCode.Length;
                var result = m_depotForPersonnel.Find(p => p.类型ID.Length >= len 
                            && p.类型ID.Substring(0, len) == depotInfo[i].MaterialTypeCode);

                if (result == null)
                {
                    depotInfo.RemoveAt(i--);
                }
            }

            // 克隆一份
            m_listDepotInfo = m_depotServer.Clone(depotInfo);

            // 是否是叶子节点
            bool isLeaf = false;

            for (int i = 0; i < depotInfo.Count; i++)
            {
                TreeNode node = new TreeNode();

                UpdateTreeNode(node, depotInfo[i]);

                isLeaf = (bool)depotInfo[i].IsEnd;

                parentNode.Nodes.Add(node);

                depotInfo.RemoveAt(i);

                i--;

                if (!isLeaf)
                {
                    BuildTree(node, depotInfo);
                }
            }

            treeView.ExpandAll();
        }

        /// <summary>
        /// 更新树节点信息
        /// </summary>
        /// <param name="node">要更新的树节点</param>
        /// <param name="info">更新后的信息</param>
        private void UpdateTreeNode(TreeNode node, MaterialTypeData info)
        {
            node.Name = info.MaterialTypeCode;
            node.Text = string.Format("({0}) {1}", node.Name, info.MaterialTypeName);
            node.Tag = info;
        }

        /// <summary>
        /// 构建树
        /// </summary>
        /// <param name="parentNode">父节点</param>
        /// <param name="lstDepInfo">部门信息列表</param>
        void BuildTree(TreeNode parentNode, List<MaterialTypeData> lstDepotInfo)
        {
            bool isLeaf = false;

            MaterialTypeData parentDepInfo = parentNode.Tag as MaterialTypeData;

            int nodeGrade = Convert.ToInt32(parentDepInfo.MaterialTypeGrade);

            for (int i = 0; i < lstDepotInfo.Count; i++)
            {
                if (nodeGrade + 1 == Convert.ToInt32(lstDepotInfo[i].MaterialTypeGrade))
                {
                    TreeNode node = new TreeNode();

                    UpdateTreeNode(node, lstDepotInfo[i]);

                    isLeaf = (bool)lstDepotInfo[i].IsEnd;

                    parentNode.Nodes.Add(node);

                    lstDepotInfo.RemoveAt(i);

                    i--;

                    if (!isLeaf)
                    {
                        BuildTree(node, lstDepotInfo);
                    }
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore">数据集</param>
        void RefreshDataGridView(IQueryable findStore)
        {
            DataTable dt = GlobalObject.GeneralFunction.ConvertToDataTable(findStore);
            DataTable dataSource = dt.Clone();

            for (int i = 0; i < m_depotForPersonnel.Count; i++)
            {
                DataRow[] dr = dt.Select("仓库 = '" + m_depotForPersonnel[i].类型ID + "'");

                if (dr.Length > 0)
                {
                    for (int a = 0; a <= dr.Length - 1; a++)
                    {
                        dataSource.ImportRow(dr[a]);
                    }
                }
            }

            dataGridView1.DataSource = dataSource;

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            dataGridView1.Columns["序号"].Visible = false;
            dataGridView1.Columns["物品ID"].Visible = false;
            dataGridView1.Columns["单位ID"].Visible = false;

            if (dataGridView1.Rows.Count > 0)
            {
                panelTop.Visible = true;

                if (m_dataLocalizer == null)
                {
                    m_dataLocalizer = new UserControlDataLocalizer(
                        dataGridView1, this.Name, UniversalFunction.SelectHideFields(
                        this.Name, dataGridView1.Name, BasicInfo.LoginID));

                    panelTop.Controls.Add(m_dataLocalizer);
                    m_dataLocalizer.Dock = DockStyle.Bottom;
                }
            }
            else
            {
                panelTop.Visible = false;
            }

            dataGridView1.Refresh();
        }

        /// <summary>
        /// 清除
        /// </summary>
        void ClearContext()
        {
            txtStorage.Text = "";
            txtShelf.Text = "";
            txtColumn.Text = "";
            txtLayer.Text = "";
        }

        /// <summary>
        /// 更新仓库货架编辑框信息
        /// </summary>
        void RefreshShelf()
        {
            ClearContext();

            if (dataGridView1.SelectedRows.Count == 0)
            {
                return;
            }

            if (dataGridView1.SelectedRows[0].Cells["仓库"].Value != null)
            {
                txtStorage.Text = dataGridView1.SelectedRows[0].Cells["仓库"].Value.ToString();
            }

            if (dataGridView1.SelectedRows[0].Cells["货架"].Value != null)
            {
                txtShelf.Text = dataGridView1.SelectedRows[0].Cells["货架"].Value.ToString();
            }

            if (dataGridView1.SelectedRows[0].Cells["列"].Value != null)
            {
                txtColumn.Text = dataGridView1.SelectedRows[0].Cells["列"].Value.ToString();
            }

            if (dataGridView1.SelectedRows[0].Cells["层"].Value != null)
            {
                txtLayer.Text = dataGridView1.SelectedRows[0].Cells["层"].Value.ToString();
            }
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            RefreshShelf();
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node == null)
            {
                MessageDialog.ShowPromptMessage("您当前所选仓库为空!");
            }

            m_selectDepotCode = e.Node.Name;

            if (e.Node.Name == "仓库")
            {
                m_selectDepotCode = treeView.Nodes[0].Nodes[0].Name;
            }

            if (m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
            {
                RefreshDataGridView(m_findStore);
            }

            RefreshDataGridView(m_findStore);

            RefreshShelf();
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

        private void btnUp_Click(object sender, EventArgs e)
        {
            m_sequence = true;

            if (!m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
            }

            RefreshDataGridView(m_findStore);
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            m_sequence = false;

            if (!m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
            }

            RefreshDataGridView(m_findStore);
        }
    }
}
