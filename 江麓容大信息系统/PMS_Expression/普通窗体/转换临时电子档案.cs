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
using UniversalControlLibrary;

namespace Expression
{
    /// <summary>
    /// 将临时电子档案转换成正式电子档案的窗体
    /// </summary>
    public partial class 转换临时电子档案 : Form
    {
        /// <summary>
        /// 电子档案服务组件
        /// </summary>
        IElectronFileServer m_serverModule = ServerModuleFactory.GetServerModule<IElectronFileServer>();

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        public 转换临时电子档案()
        {
            InitializeComponent();

            string[] productCodes = m_serverModule.GetProductCodeOfTempElectronFile(out m_error);

            if (productCodes == null)
            {
                MessageDialog.ShowErrorMessage(m_error);
            }
            else
            {
                this.cmbProductCode.SelectedIndexChanged -= new System.EventHandler(this.cmbProductCode_SelectedIndexChanged);

                cmbProductCode.Items.AddRange(productCodes);

                this.cmbProductCode.SelectedIndexChanged += new System.EventHandler(this.cmbProductCode_SelectedIndexChanged);
            }
        }

        /// <summary>
        /// 初始化TreeView
        /// </summary>
        /// <param name="productCode">产品编码</param>
        void InitTreeView(string productCode)
        {
            string error;

            List<View_P_TempElectronFile> eRecords = m_serverModule.GetTempElectronFile(productCode, out error);

            dataGridView.DataSource = eRecords;

            treeView1.Nodes.Clear();

            if (eRecords != null)
            {
                // 在后续剔除分总成的过程中防止剔除零部件编码为空的零件，如：白锌平垫
                List<string> lst父总成编码 = (from r in eRecords orderby r.父总成编码 
                                         where r.父总成编码 != "" 
                                         select r.父总成编码).Distinct().ToList();
                
                List<View_P_TempElectronFile> electronWords = (from r in eRecords
                                                               where lst父总成编码.Contains(r.零部件编码)
                                                               orderby r.零部件编码
                                                               select r).ToList();

                View_P_TempElectronFile root = electronWords.Find(p => p.父总成编码 == "");

                TreeNode rootNode = new TreeNode();

                rootNode.Name = root.零部件编码;
                rootNode.Text = root.零部件名称;
                rootNode.ToolTipText = root.零部件名称 + "," + root.零部件编码 + "," + root.规格;
                rootNode.Tag = root;

                treeView1.Nodes.Add(rootNode);
                electronWords.Remove(root);

                for (int i = 0; i < electronWords.Count; i++)
                {
                    View_P_TempElectronFile item = electronWords[i];

                    // 剔除零部件编码为空的零件，如：白锌平垫
                    if (item.零部件编码 == "")
                    {
                        continue;
                    }

                    TreeNode parentNode = new TreeNode();

                    parentNode.Name = item.零部件编码;
                    parentNode.Text = item.零部件名称;
                    parentNode.ToolTipText = item.零部件名称 + "," + item.零部件编码 + "," + item.规格;
                    parentNode.Tag = item;
                    rootNode.Nodes.Add(parentNode);
                    electronWords.Remove(item);

                    i--;
                }

                List<View_P_TempElectronFile> lstEF = //eRecords.ToList();
                    (from r in eRecords 
                     where !lst父总成编码.Contains(r.零部件编码) 
                     orderby r.零部件编码 
                     select r).ToList();

                for (int i = 0; i < rootNode.Nodes.Count; i++)
                {
                    RecursionBuildTreeView(rootNode.Nodes[i], lstEF);
                }

                RecursionBuildTreeView(rootNode, lstEF);
            }
            else
            {
                MessageDialog.ShowErrorMessage(error);
                return;
            }

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 递归生成电子档案的树型结构
        /// </summary>
        /// <param name="parentNode">父总成节点</param>
        /// <param name="eRecords">电子档案信息</param>
        void RecursionBuildTreeView(TreeNode parentNode, List<View_P_TempElectronFile> eRecords)
        {
            for (int i = 0; i < eRecords.Count; i++)
            {
                if (parentNode.Name == eRecords[i].父总成编码)
                {
                    TreeNode node = new TreeNode();

                    node.Name = eRecords[i].零部件编码;
                    node.Text = eRecords[i].零部件名称;
                    node.ToolTipText = eRecords[i].规格;
                    node.Tag = eRecords[i];

                    parentNode.Nodes.Add(node);

                    eRecords.RemoveAt(i);
                    i = -1;

                    RecursionBuildTreeView(node, eRecords);
                }
            }
        }

        private void cmbProductCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitTreeView(cmbProductCode.Text);
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dataGridView.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dataGridView.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dataGridView.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }
    }
}
