using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 材料类别树
    /// </summary>
    static class MaterialsTypeTree
    {
        /// <summary>
        /// 构建材料类别树
        /// </summary>
        /// <param name="treeView">要构建的树</param>
        /// <returns>成功返回true</returns>
        static public bool BuildTree(TreeView treeView)
        {
            // 仓库信息服务
            IMaterialTypeServer depotServer = ServerModuleFactory.GetServerModule<IMaterialTypeServer>();

            try
            {
                IQueryable<View_S_Depot> depot = null;
                string error = null;

                if (depotServer.GetAllMaterialType(out depot, out error))
                {
                    InitTreeView(treeView, depot.ToList());
                }

                return true;
            }
            catch (Exception err)
            {
                MessageDialog.ShowErrorMessage(err.Message);
                return false;
            }
        }

        static void InitTreeView(TreeView treeView, List<View_S_Depot> lstDepotInfo)
        {
            TreeNode rootNode = GenerateNode(lstDepotInfo[0]);
            rootNode.Text = "材料类别";

            treeView.Nodes.Add(rootNode);
            lstDepotInfo.RemoveAt(0);

            BuildTreeView(rootNode, lstDepotInfo);
            treeView.ExpandAll();
        }

        /// <summary>
        /// 生成树节点
        /// </summary>
        /// <param name="depotInfo">仓库信息</param>
        /// <returns>返回生成的树节点</returns>
        static TreeNode GenerateNode(View_S_Depot depotInfo)
        {
            TreeNode node = new TreeNode();
            node.Name = depotInfo.仓库编码;
            node.Text = depotInfo.仓库名称;
            node.Tag = depotInfo;

            return node;
        }

        static void BuildTreeView(TreeNode parentNode, List<View_S_Depot> lstDepotInfo)
        {
            for (int i = 0; i < lstDepotInfo.Count; i++)
            {
                if (parentNode.Name.Length + 2 != lstDepotInfo[i].仓库编码.Length)
                    break;

                TreeNode node = GenerateNode(lstDepotInfo[i]);
                parentNode.Nodes.Add(node);

                lstDepotInfo.RemoveAt(i);
                i--;

                if (!(node.Tag as View_S_Depot).是否末级)
                {
                    BuildTreeView(node, lstDepotInfo);
                }
            }
        }
    }
}
