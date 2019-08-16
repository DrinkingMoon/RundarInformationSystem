/******************************************************************************
 * 版权所有 (c) 2006-2010, 小康工业集团容大有限责任公司
 *
 * 文件名称:  UserControlDepot.cs
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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 材料类别组件
    /// </summary>
    public partial class UserControlDepot : Form
    {
        /// <summary>
        /// 材料类别编码级数(5 级：** ** **)
        /// </summary>
        const int m_CODEGRADE = 5;

        /// <summary>
        /// 材料类别编码对应不同的级数所应具有的字符数量
        /// </summary>
        readonly int[] m_CodeChars = { 2, 4, 6, 8, 10 };

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 要从材料类别信息列表中检索的材料类别编码
        /// </summary>
        string m_findDepotCode = "";

        /// <summary>
        /// 部门信息列表
        /// </summary>
        List<MaterialTypeData> m_listDepotInfo = new List<MaterialTypeData>();

        /// <summary>
        /// 物品类别服务组件
        /// </summary>
        IMaterialTypeServer m_depotServer = ServerModuleFactory.GetServerModule<IMaterialTypeServer>();

        /// <summary>
        /// 当前树节点
        /// </summary>
        TreeNode CurrentTreeNode
        {
            get { return treeView.SelectedNode; }
        }

        /// <summary>
        /// 当前材料类别信息
        /// </summary>
        MaterialTypeData CurrentDepotInfo
        {
            get
            {
                if (CurrentTreeNode != null)
                {
                    MaterialTypeData info = CurrentTreeNode.Tag as MaterialTypeData;
                    return info;
                }

                return null;
            }
        }

        public UserControlDepot()
        {
            InitializeComponent();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint | ControlStyles.OptimizedDoubleBuffer, true);
            UpdateStyles();
        }

        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlDepot_Load(object sender, EventArgs e)
        {
            InitCodeRule();

            InitTreeView();

            InitSetting(false);
        }

        /// <summary>
        /// 初始化编码规则
        /// </summary>
        void InitCodeRule()
        {
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < m_CodeChars.Length; i++)
            {
                int chars = m_CodeChars[i];

                if (i > 0)
                {
                    chars = m_CodeChars[i] - m_CodeChars[i - 1];
                }

                for (int j = 0; j < chars; j++)
                {
                    sb.Append('*');
                }

                sb.Append(' ');
            }

            labCodeRule.Text = sb.ToString();
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

            parentNode.Name = "材料类别";
            parentNode.Text = "材料类别";

            treeView.Nodes.Add(parentNode);

            List<MaterialTypeData> depotInfo;

            // 获取所有部门信息
            if (m_depotServer.GetAllMaterialType(out depotInfo, out m_err))
            {
                btnUpdate.Enabled = true;
            }
            else
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

            treeView.ExpandAll();
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
        /// 初始化设置
        /// </summary>
        /// <param name="enable">是否允许初始化设置标志</param>
        void InitSetting(bool enable)
        {
            btnDelete.Enabled = enable;
            btnUpdate.Enabled = enable;

            if (!enable)
            {
                txtCode.Text = "";
                txtName.Text = "";

                btnUpdate.Checked = false;
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (CurrentDepotInfo.MaterialTypeGrade == 0)
            {
                MessageDialog.ShowPromptMessage("1级材料类别目录不允许更改!");
                return;
            }

            if (CurrentTreeNode == null || CurrentDepotInfo == null)
            {
                MessageDialog.ShowPromptMessage("请选中要修改的材料类别后再操作");
                return;
            }

            if (CurrentDepotInfo.MaterialTypeCode.CompareTo(txtCode.Text) != 0)
            {
                MessageDialog.ShowPromptMessage("修改时不允许改动材料类别编码");
                txtCode.Text = CurrentDepotInfo.MaterialTypeCode;
                return;
            }

            if (!UpdataDepot(out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
            }
        }

        /// <summary>
        /// 更新材料类别信息
        /// </summary>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>成功返回true</returns>
        bool UpdataDepot(out string err)
        {
            MaterialTypeData info = GetDepotInfoFromFace();

            info.MaterialTypeCode = CurrentDepotInfo.MaterialTypeCode;
            info.MaterialTypeGrade = CurrentDepotInfo.MaterialTypeGrade;
            info.IsEnd = CurrentDepotInfo.IsEnd;

            if (m_depotServer.UpdataMaterialType(info, out err))
            {
                UpdateTreeNode(CurrentTreeNode, info);

                // 从信息列表中更新元素
                UpdateItemFromDepotList(info.MaterialTypeCode, info);

                return true;
            }

            return false;
        }

        /// <summary>
        /// 从界面获取材料类别信息
        /// </summary>
        /// <returns>获取到的材料类别信息</returns>
        private MaterialTypeData GetDepotInfoFromFace()
        {
            MaterialTypeData info = new MaterialTypeData();

            info.MaterialTypeCode = txtCode.Text;
            info.MaterialTypeName = txtName.Text;
            info.MaterialTypeGrade = info.MaterialTypeCode.Length / 2 - 1;

            return info;
        }

        /// <summary>
        /// 从材料类别列表中更新指定材料类别
        /// </summary>
        /// <param name="depCode">要更新材料类别的编码</param>
        /// <param name="info">更新后的信息</param>
        private void UpdateItemFromDepotList(string depotCode, MaterialTypeData info)
        {
            int index = -1;

            m_findDepotCode = depotCode;
            index = m_listDepotInfo.FindIndex(this.FindDepotListData);
            m_listDepotInfo[index] = info;
        }

        /// <summary>
        /// 查找符合条件的部门信息列表数据
        /// </summary>
        /// <param name="info">检索的信息</param>
        /// <returns>当前检索项目符合条件返回true, 其它为false</returns>
        private bool FindDepotListData(MaterialTypeData info)
        {
            if (info.MaterialTypeCode.CompareTo(m_findDepotCode) == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckInputItem(out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            if (!CheckDepotCode(txtCode.Text, out m_err))
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            if (IsExistDepotCode(txtCode.Text))
            {
                txtCode.Focus();
                MessageDialog.ShowErrorMessage("部门编码已经存在, 不允许添加相同的编码");
                return;
            }

            if (!CheckDepotName(txtName.Text, out m_err))
            {
                txtName.Focus();
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }

            // 从界面上获取材料类别信息
            MaterialTypeData info = GetDepotInfoFromFace();

            // 添加的节点必然是叶子节点
            info.IsEnd = true;

            TreeNode parentNode = GetParentNode(info);

            if (parentNode != null)
            {
                MaterialTypeData parentDepotInfo = parentNode.Tag as MaterialTypeData;

                parentDepotInfo = parentDepotInfo.Clone() as MaterialTypeData;

                // 修改父节点的IsEnd属性
                parentDepotInfo.IsEnd = false;

                if (!m_depotServer.AddMaterialType(info, parentDepotInfo, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                // 更新树节点的绑定数据
                parentNode.Tag = parentDepotInfo;

                // 从部门列表中更新父节点信息
                UpdateItemFromDepotList(parentDepotInfo.MaterialTypeCode, parentDepotInfo);
            }
            else
            {
                if (!m_depotServer.AddMaterialType(info, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
            }

            AddTreeNode(info);
            m_listDepotInfo.Add(info);
            InitSetting(false);
            this.treeView.ExpandAll();
        }

        /// <summary>
        /// 检查编辑框的内容是否符合要求
        /// </summary>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>符合要求返回true</returns>
        private bool CheckInputItem(out string err)
        {
            err = null;

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtCode.Text))
            {
                err = "材料类别编码不能为空";
                txtCode.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtName.Text))
            {
                err = "材料类别名称不能为空";
                txtName.Focus();
                return false;
            }

            return true;
        }

        /// <summary>
        /// 检查材料类别编码是否符合要求
        /// </summary>
        /// <param name="depCode">材料类别编码</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>符合要求返回true</returns>
        private bool CheckDepotCode(string depotCode, out string err)
        {
            err = null;

            // 判断输入的材料类别编码长度是否正确
            bool correctLength = false;

            foreach (int codeLen in m_CodeChars)
            {
                if (depotCode.Length == codeLen)
                {
                    correctLength = true;
                    break;
                }
            }

            if (!correctLength)
            {
                StringBuilder sb = new StringBuilder("材料类别编码不符合编码规则, ");

                for (int i = 0; i < m_CodeChars.Length; i++)
                {
                    sb.Append(string.Format("[{0}]级材料类别为[{1}]个字符 ", i + 1, m_CodeChars[i]));
                }

                err = sb.ToString();
                return false;
            }

            if (depotCode.Length == m_CodeChars[0])
            {
                return true;
            }

            // 上级材料类别编码
            string upperDepotCode = depotCode.Substring(0, depotCode.Length - 2);

            if (m_listDepotInfo == null)
            {
                err = string.Format("不存在上级材料类别[{0}], 不允许添加下级材料类别[{1}]", upperDepotCode, depotCode);
                return false;
            }

            // 是否找到上级材料类别
            bool bFindUpperDepot = false;

            foreach (MaterialTypeData info in m_listDepotInfo)
            {
                if (info.MaterialTypeCode.CompareTo(upperDepotCode) == 0)
                {
                    bFindUpperDepot = true;
                    break;
                }
            }

            if (!bFindUpperDepot)
            {
                err = string.Format("不存在上级材料类别[{0}], 不允许添加下级材料类别[{1}]", upperDepotCode, depotCode);
            }

            return bFindUpperDepot;
        }

        /// <summary>
        /// 材料类别编码是否已经存在
        /// </summary>
        /// <param name="depCode">要检查的材料类别编码</param>
        /// <returns>存在返回true</returns>
        bool IsExistDepotCode(string depotCode)
        {
            bool bFindDepot = false;

            foreach (MaterialTypeData info in m_listDepotInfo)
            {
                if (info.MaterialTypeCode.CompareTo(depotCode) == 0)
                {
                    bFindDepot = true;
                    break;
                }
            }

            return bFindDepot;
        }

        /// <summary>
        /// 检查材料类别名称是否符合要求
        /// </summary>
        /// <param name="depName">材料类别名称</param>
        /// <param name="err">错误信息, 没有错误该值为null</param>
        /// <returns>符合要求返回true</returns>
        private bool CheckDepotName(string depotName, out string err)
        {
            err = null;

            if (m_listDepotInfo == null)
            {
                return true;
            }

            foreach (MaterialTypeData info in m_listDepotInfo)
            {
                if (info.MaterialTypeName.CompareTo(depotName) == 0)
                {
                    err = "已经存在同名的材料类别";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 获取指定材料类别信息在树中的父节点
        /// </summary>
        /// <param name="info">要获取父节点的材料类别信息</param>
        /// <returns>成功返回父节点, 失败返回null</returns>
        private TreeNode GetParentNode(MaterialTypeData info)
        {
            if (info.MaterialTypeCode.Length == m_CodeChars[0])
            {
                return null;
            }

            string upperDepotCode = info.MaterialTypeCode.Substring(0, info.MaterialTypeCode.Length - 2);
            TreeNode parentNode = treeView.Nodes.Find(upperDepotCode, true)[0];

            return parentNode;
        }

        /// <summary>
        /// 将材料类别信息添加到一个新的树节点
        /// </summary>
        /// <param name="info">材料类别信息类</param>
        private void AddTreeNode(MaterialTypeData info)
        {
            TreeNode parentNode = null;

            if (info.MaterialTypeGrade == 0)
            {
                parentNode = treeView.Nodes[0];
            }
            else
            {
                parentNode = GetParentNode(info);
            }

            if (parentNode != null)
            {
                TreeNode childNode = new TreeNode();

                UpdateTreeNode(childNode, info);

                parentNode.Nodes.Add(childNode);
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (CurrentDepotInfo.MaterialTypeGrade == 0)
            {
                MessageDialog.ShowPromptMessage("1级材料类别目录不允许删除!");
                return;
            }

            if (CurrentTreeNode == null || CurrentDepotInfo == null)
            {
                MessageDialog.ShowPromptMessage("请选中要删除的材料类别后再操作");
                return;
            }

            if (CurrentTreeNode.Nodes.Count > 0)
            {
                MessageDialog.ShowPromptMessage(string.Format("材料类别[{0}]下还存在子材料类别, 只能删除不存在子材料类别的节点", 
                    CurrentDepotInfo.MaterialTypeCode));
                return;
            }

            if (DialogResult.No == MessageDialog.ShowEnquiryMessage(string.Format("您是否确定要删除部门[({0}){1}]", 
                CurrentDepotInfo.MaterialTypeCode, CurrentDepotInfo.MaterialTypeName)))
            {
                return;
            }

            MaterialTypeData parentDepotInfo = CurrentTreeNode.Parent.Tag as MaterialTypeData;

            if (CurrentTreeNode.Parent.Nodes.Count == 1 && CurrentTreeNode.Parent.Name != "材料类别")
            {
                parentDepotInfo = parentDepotInfo.Clone() as MaterialTypeData;

                // 修改父节点的IsEnd属性
                parentDepotInfo.IsEnd = true;

                if (!m_depotServer.DeleteMaterialType(CurrentDepotInfo.MaterialTypeCode, parentDepotInfo, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                CurrentTreeNode.Parent.Tag = parentDepotInfo;
            }
            else
            {
                if (!m_depotServer.DeleteMaterialType(CurrentDepotInfo.MaterialTypeCode, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }
            }

            // 从信息列表中移除元素
            m_findDepotCode = CurrentDepotInfo.MaterialTypeCode;
            int index = m_listDepotInfo.FindIndex(this.FindDepotListData);
            m_listDepotInfo.RemoveAt(index);

            CurrentTreeNode.Parent.Nodes.Remove(CurrentTreeNode);
        }

        /// <summary>
        /// 更新按钮检查状态发生变化时触发该事件
        /// </summary>
        /// <param name="sender">触发此事件的对象</param>
        /// <param name="e">事件参数</param>
        private void btnUpdate_CheckedChanged(object sender, EventArgs e)
        {
            if (btnUpdate.Checked)
            {
                txtCode.Enabled = false;
            }
        }

        /// <summary>
        /// 树节点选中后触发该事件
        /// </summary>
        /// <param name="sender">触发此事件的对象</param>
        /// <param name="e">事件参数</param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode selNode = treeView.SelectedNode;

            // 是根节点
            if (selNode == treeView.Nodes[0])
            {
                InitSetting(false);
                return;
            }

            InitSetting(true);

            MaterialTypeData info = selNode.Tag as MaterialTypeData;

            txtCode.Text = info.MaterialTypeCode;
            txtName.Text = info.MaterialTypeName;
        }

        /// <summary>
        /// 改变组件大小
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UserControlDepot_Resize(object sender, EventArgs e)
        {
            panelLeft.Width = (this.Width - panelCenter.Width) / 2;
            panelRight.Width = this.Width - panelCenter.Width - panelLeft.Width;
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }
    }
}
