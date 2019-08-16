/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  FormStore.cs
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
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 货物库存界面
    /// </summary>
    public partial class FormStore : Form
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
        /// 物品名称
        /// </summary>
        TextBox m_textName;

        /// <summary>
        /// 图号型号
        /// </summary>
        TextBox m_textCode;

        /// <summary>
        /// 供应商
        /// </summary>
        TextBox m_textProvider;

        /// <summary>
        /// 批次号
        /// </summary>
        TextBox m_textBatchNo;

        /// <summary>
        /// 单位
        /// </summary>
        TextBox m_textUnit;

        /// <summary>
        /// 数量
        /// </summary>
        NumericUpDown m_numCount;

        /// <summary>
        /// 规格
        /// </summary>
        TextBox m_textSpec;

        /// <summary>
        /// 仓库
        /// </summary>
        TextBox m_textDepot;

        /// <summary>
        /// 货架
        /// </summary>
        TextBox m_textShelf;
        
        /// <summary>
        /// 列
        /// </summary>
        TextBox m_textColumn;

        /// <summary>
        /// 层
        /// </summary>
        TextBox m_textLayer;

        /// <summary>
        /// 查询条件
        /// </summary>
        string m_conditionGoodsCode;

        /// <summary>
        /// 查询条件
        /// </summary>
        string m_conditionSpec;

        public FormStore(TextBox textName, TextBox textCode, TextBox textProvider, TextBox textBatchNo, TextBox textUnit, 
            NumericUpDown numCount, TextBox textSpec, string conditionGoodsCode, string conditionSpec, TextBox textDepot, 
            TextBox textShelf, TextBox textColumn, TextBox textLayer)
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | 
                ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
            
            m_textName = textName;
            m_textCode = textCode;
            m_textProvider = textProvider;
            m_textBatchNo = textBatchNo;
            m_textUnit = textUnit;
            m_numCount = numCount;
            m_textSpec = textSpec;
            m_conditionGoodsCode = conditionGoodsCode;

            if (conditionSpec != null && conditionSpec != "")
            {
                m_conditionSpec = conditionSpec;
            }
            else
            {
                m_conditionSpec = null;
            }

            m_textDepot = textDepot;
            m_textShelf = textShelf;
            m_textColumn = textColumn;
            m_textLayer = textLayer;
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStore_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);

            if (dataGridView1.Columns.Count != 0) 
            {
                dataGridView1.Columns[0].Visible = false;
            }
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FormStore_Load(object sender, EventArgs e)
        {
            InitTreeView();

            m_selectDepotCode = null;

            if (m_conditionGoodsCode != null)
            {
                DataTable table;

                if (!m_storeServer.GetSomeGoodsCodeStore(m_conditionGoodsCode, m_conditionSpec, out table, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                dataGridView1.DataSource = table;
                dataGridView1.Refresh();

                treeView.Enabled = false;
            }
            else
            {
                if (!m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                RefreshDataGridView(m_findStore);

                treeView.Enabled = true;
            }

            for (int i = 0; i < dataGridView1.Columns.Count; i++)
            {
                //dataGridView1.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            dataGridView1.Columns[0].Visible = false;
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

            // 获取所有部门信息
            if (!m_depotServer.GetAllMaterialType(out depotInfo, out m_err))
            {
                return;
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

            TreeNode newNode = new TreeNode();
            newNode.Name = "其他";
            newNode.Text = "其他";

            treeView.Nodes[0].Nodes.Add(newNode);

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
        /// 双击右边树结构中任一节点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (m_conditionGoodsCode == null)
            {
                if (e.Node == null)
                {
                    MessageDialog.ShowPromptMessage("您当前所选仓库为空!");
                }

                m_selectDepotCode = e.Node.Name;

                if (e.Node.Name == "仓库")
                {
                    m_selectDepotCode = null;
                }

                if (m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
                {
                    RefreshDataGridView(m_findStore);
                }

                RefreshDataGridView(m_findStore);
            }
        }

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore">库存信息</param>
        void RefreshDataGridView(IQueryable findStore)
        {
            dataGridView1.DataSource = findStore;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 选定物品
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSelect_Click(object sender, EventArgs e)
        {
            if (m_textCode != null && dataGridView1.CurrentRow.Cells[1].Value != null)
            {
                m_textCode.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString();
            }
            else if (m_textCode != null && dataGridView1.CurrentRow.Cells[1].Value == null)
            {
                m_textCode.Text = "";
            }

            if (m_textName != null && dataGridView1.CurrentRow.Cells[2].Value != null)
            {
                m_textName.Text = dataGridView1.CurrentRow.Cells[2].Value.ToString();
            }
            else if (m_textName != null && dataGridView1.CurrentRow.Cells[2].Value == null)
            {
                m_textName.Text = "";
            }

            if (m_textSpec != null && dataGridView1.CurrentRow.Cells[3].Value != null)
            {
                m_textSpec.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
            }
            else if (m_textSpec != null && dataGridView1.CurrentRow.Cells[3].Value == null)
            {
                m_textSpec.Text = "";
            }

            if (m_textProvider != null && dataGridView1.CurrentRow.Cells[4].Value != null)
            {
                m_textProvider.Text = dataGridView1.CurrentRow.Cells[4].Value.ToString();
            }
            else if (m_textProvider != null && dataGridView1.CurrentRow.Cells[4].Value == null)
            {
                m_textProvider.Text = "";
            }

            if (m_textBatchNo != null && dataGridView1.CurrentRow.Cells[5].Value != null)
            {
                m_textBatchNo.Text = dataGridView1.CurrentRow.Cells[5].Value.ToString();
            }
            else if (m_textBatchNo != null && dataGridView1.CurrentRow.Cells[5].Value == null)
            {
                m_textBatchNo.Text = "";
            }

            if (m_textUnit != null && dataGridView1.CurrentRow.Cells[6].Value != null)
            {
                m_textUnit.Text = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            }
            else if (m_textUnit != null && dataGridView1.CurrentRow.Cells[6].Value == null)
            {
                m_textUnit.Text = "";
            }

            if (m_numCount != null && dataGridView1.CurrentRow.Cells[7].Value != null)
            {
                m_numCount.Maximum = Convert.ToDecimal(dataGridView1.CurrentRow.Cells[7].Value);
            }
            else if (m_numCount != null && dataGridView1.CurrentRow.Cells[7].Value == null)
            {
                m_numCount.Text = "";
            }

            if (m_textDepot != null && dataGridView1.CurrentRow.Cells[8].Value != null)
            {
                m_textDepot.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
            }
            else if (m_textDepot != null && dataGridView1.CurrentRow.Cells[8].Value == null)
            {
                m_textDepot.Text = "";
            }

            if (m_textShelf != null && dataGridView1.CurrentRow.Cells[9].Value != null)
            {
                m_textShelf.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
            }
            else if (m_textShelf != null && dataGridView1.CurrentRow.Cells[9].Value == null)
            {
                m_textShelf.Text = "";
            }

            if (m_textColumn != null && dataGridView1.CurrentRow.Cells[10].Value != null)
            {
                m_textColumn.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
            }
            else if (m_textColumn != null && dataGridView1.CurrentRow.Cells[10].Value == null)
            {
                m_textColumn.Text = "";
            }

            if (m_textLayer != null && dataGridView1.CurrentRow.Cells[11].Value != null)
            {
                m_textLayer.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
            }
            else if (m_textLayer != null && dataGridView1.CurrentRow.Cells[11].Value == null)
            {
                m_textLayer.Text = "";
            }

            this.Close();
        }

    }
}
