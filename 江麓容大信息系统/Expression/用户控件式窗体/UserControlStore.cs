/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlStore.cs
 * 作者    :  夏石友    版本: v1.00    日期: 2009/06/15
 * 开发平台:  vs2005(c#)
 * 用于    :  生产线管理信息系统
 *----------------------------------------------------------------------------
 * 描述 : 供应商信息界面
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
    /// 库存组件
    /// </summary>
    public partial class UserControlStore : Form
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
        /// 材料类别服务组件
        /// </summary>
        IMaterialTypeServer m_depotServer = ServerModuleFactory.GetServerModule<IMaterialTypeServer>();

        /// <summary>
        /// 库存管理服务组件
        /// </summary>
        IStoreServer m_storeServer = ServerModuleFactory.GetServerModule<IStoreServer>();

        public UserControlStore()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlStore_Resize(object sender, EventArgs e)
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
        private void UserControlStore_Load(object sender, EventArgs e)
        {
            InitTreeView();

            m_selectDepotCode = treeView.Nodes[0].Nodes[0].Name;

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.营销仓库管理员.ToString()))
            {
                if (!m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.制造仓库管理员.ToString()))
            {
                if (!m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
            }

            RefreshDataGridView(m_findStore);

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns[0].Visible = false;
            }

            RefreshShelf();
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

            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.营销仓库管理员.ToString()))
            {
                for (int i = 0; i < depotInfo.Count; i++)
                {
                    if (depotInfo[i].MaterialTypeCode.Substring(0, 2) != "YX")
                    {
                        depotInfo.RemoveAt(i);
                        i--;
                    }
                }
            }
            else if (BasicInfo.ListRoles.Contains(CE_RoleEnum.制造仓库管理员.ToString()))
            {
                for (int i = 0; i < depotInfo.Count; i++)
                {
                    if (depotInfo[i].MaterialTypeCode.Substring(0, 2) == "YX")
                    {
                        depotInfo.RemoveAt(i);
                        i--;
                    }
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

        /// <summary>
        /// 刷新DataGridView
        /// </summary>
        /// <param name="findStore"></param>
        void RefreshDataGridView(IQueryable findStore)
        {
            dataGridView1.DataSource = findStore;
            dataGridView1.Refresh();
        }

        /// <summary>
        /// 点击DataGridView
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            RefreshShelf();
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

            if (dataGridView1.CurrentRow != null)
            {
                if (dataGridView1.CurrentRow.Cells[8].Value != null)
                {
                    txtStorage.Text = dataGridView1.CurrentRow.Cells[8].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells[9].Value != null)
                {
                    txtShelf.Text = dataGridView1.CurrentRow.Cells[9].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells[10].Value != null)
                {
                    txtColumn.Text = dataGridView1.CurrentRow.Cells[10].Value.ToString();
                }

                if (dataGridView1.CurrentRow.Cells[11].Value != null)
                {
                    txtLayer.Text = dataGridView1.CurrentRow.Cells[11].Value.ToString();
                }
            }
            else if (dataGridView1.Rows.Count > 0)
            {
                if (dataGridView1.Rows[0].Cells[8].Value != null)
                {
                    txtStorage.Text = dataGridView1.Rows[0].Cells[8].Value.ToString();
                }
                
                if (dataGridView1.Rows[0].Cells[9].Value != null)
                {
                    txtShelf.Text = dataGridView1.Rows[0].Cells[9].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells[10].Value != null)
                {
                    txtColumn.Text = dataGridView1.Rows[0].Cells[10].Value.ToString();
                }

                if (dataGridView1.Rows[0].Cells[11].Value != null)
                {
                    txtLayer.Text = dataGridView1.Rows[0].Cells[11].Value.ToString();
                }
            }
        }

        /// <summary>
        /// 升序排列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUp_Click(object sender, EventArgs e)
        {
            m_sequence = true;

            if (!m_storeServer.GetAllStore(m_selectDepotCode, m_sequence, out m_findStore, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
            }

            RefreshDataGridView(m_findStore);
        }

        /// <summary>
        /// 降序排列
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
