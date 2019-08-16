using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using System.Collections;

namespace Expression
{
    /// <summary>
    /// 产品编码综合查询界面
    /// </summary>
    public partial class 产品编码综合查询 : Form
    {
        /// <summary>
        /// 库房信息服务
        /// </summary>
        IStorageInfo m_serverStorageInfo = ServerModuleFactory.GetServerModule<IStorageInfo>();

        /// <summary>
        /// 当前产品编号
        /// </summary>
        string m_curProductCode = "";

        /// <summary>
        /// 数据定位控件
        /// </summary>
        UserControlDataLocalizer m_dataLocalizer;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 数据库中所有符合查询条件的记录表
        /// </summary>
        DataTable m_allTable;

        /// <summary>
        /// 服务组件
        /// </summary>
        IElectronFileServer m_serverModule = ServerModuleFactory.GetServerModule<IElectronFileServer>();
        
        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 营销服务组件
        /// </summary>
        ISellIn m_sell = ServerModuleFactory.GetServerModule<ISellIn>();

        public 产品编码综合查询(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;
            AuthorityControl(m_authFlag);
            cmbStorage.Items.Add("成品库房");
            cmbStorage.Items.Add("售后库房");
            cmbStorage.Items.Add("下线成品库存");
            cmbStorage.Items.Add("下线售后库存");
            cmbStorage.SelectedIndex = -1;
        }

        void txtProductCode_OnCompleteSearch()
        {
            cmbStorage.Tag = txtProductCode.DataResult["库房ID"].ToString();
            cmbStorage.Text = txtProductCode.DataResult["库房名称"].ToString();
            txtProductCode.Tag = Convert.ToInt32(txtProductCode.DataResult["产品ID"]);
            btnStockCheck.Focus();
        }

        /// <summary>
        /// 权限控制
        /// </summary>
        /// <param name="authorityFlag">权限标志</param>
        void AuthorityControl(PlatformManagement.AuthorityFlag authorityFlag)
        {
            FaceAuthoritySetting.SetEnable(this.Controls, authorityFlag);
        }

        /// <summary>
        /// 查询出厂检验记录
        /// </summary>
        void CheckDeliveryInspection()
        {
            dgvRoutineTest.DataSource = m_sell.GetDeliveryInspectionInfo(
                dgvStockInfo.CurrentRow.Cells["产品编码"].Value.ToString(),
                Convert.ToInt32(dgvStockInfo.CurrentRow.Cells["产品ID"].Value));
        }

        /// <summary>
        /// 查询业务信息
        /// </summary>
        void CheckBusiness()
        {
            dgvOperationInfo.DataSource = m_sell.GetProductCodeOperationInfo(
                dgvStockInfo.CurrentRow.Cells["产品编码"].Value.ToString(),
                Convert.ToInt32(dgvStockInfo.CurrentRow.Cells["产品ID"].Value), cmbStorage.Tag.ToString());
        }

        void CheckOffLineTestInfo()
        {
            Hashtable hsTable = new Hashtable();

            hsTable.Add("@ProductCode", dgvStockInfo.CurrentRow.Cells["产品编码"].Value.ToString());
            hsTable.Add("@ProductType", dgvStockInfo.CurrentRow.Cells["产品型号"].Value.ToString());

            DataTable tempTable =
                GlobalObject.DatabaseServer.QueryInfoPro("CVTFinalInspection_Select", hsTable, out m_err);

            if (tempTable == null || tempTable.Rows.Count == 0)
            {
                return;
            }

            lbOffLineTestInfo.Text = tempTable.Rows[0]["下线试验信息"].ToString();
            lbAuditDate.Text = tempTable.Rows[0]["审核时间"].ToString();
            lbWeigh.Text = tempTable.Rows[0]["称重信息"].ToString();
            lbWeighDate.Text = tempTable.Rows[0]["称重时间"].ToString();
            lbTightness.Text = tempTable.Rows[0]["气密性信息"].ToString();
            lbTightnessDate.Text = tempTable.Rows[0]["检测时间"].ToString();
        }

        /// <summary>
        /// 查询电子档案
        /// </summary>
        void CheckRecord()
        {
            string strProductCode = dgvStockInfo.CurrentRow.Cells["产品型号"].Value.ToString() + " " +
                dgvStockInfo.CurrentRow.Cells["产品编码"].Value.ToString();
            m_curProductCode = strProductCode;
            InitTreeView(strProductCode);
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        void CheckCustomerInfo()
        {
            dgvCustomerInfo.DataSource = m_sell.GetCustomerInfo(
                Convert.ToInt32(dgvStockInfo.CurrentRow.Cells["产品ID"].Value),
                dgvStockInfo.CurrentRow.Cells["产品编码"].Value.ToString());
        }

        /// <summary>
        /// 查询装车信息
        /// </summary>
        void CheckLoadingInfo()
        {
            dgvTruckLoadingInfo.DataSource = m_sell.GetLoadingInfo(
                Convert.ToInt32(dgvStockInfo.CurrentRow.Cells["产品ID"].Value),
                dgvStockInfo.CurrentRow.Cells["产品编码"].Value.ToString());
        }

        /// <summary>
        /// 初始化TreeView1
        /// </summary>
        /// <param name="edition">版本号</param>
        void InitTreeView(string productCode)
        {
            IQueryable<P_ElectronFile> eRecords;
            string error;

            treeView1.Nodes.Clear();

            if (m_serverModule.GetElectronFile(productCode, out eRecords, out error))
            {
                List<string> lstParentCode = (from r in eRecords orderby r.ParentCode 
                                              select r.ParentCode).Distinct().ToList();
                List<P_ElectronFile> electronWords = (from r in eRecords 
                                                      where lstParentCode.Contains(r.GoodsCode) orderby r.GoodsCode 
                                                      select r).ToList();
                if (electronWords.Count == 0)
                {
                    return;
                }

                P_ElectronFile root = electronWords.Find(p => p.ParentCode == "");

                TreeNode rootNode = new TreeNode();

                rootNode.Name = root.GoodsCode;
                rootNode.Text = root.GoodsName;
                rootNode.ToolTipText = root.GoodsName + "," + root.GoodsCode + "," + root.Spec;
                rootNode.Tag = root;
                treeView1.Nodes.Add(rootNode);
                electronWords.Remove(root);

                for (int i = 0; i < electronWords.Count; i++)
                {
                    P_ElectronFile item = electronWords[i];
                    TreeNode parentNode = new TreeNode();

                    parentNode.Name = item.GoodsCode;
                    parentNode.Text = item.GoodsName;
                    parentNode.ToolTipText = item.GoodsName + "," + item.GoodsCode + "," + item.Spec;
                    parentNode.Tag = item;
                    rootNode.Nodes.Add(parentNode);
                    electronWords.Remove(item);

                    i--;
                }

                List<P_ElectronFile> lstEF = 
                    (from r in eRecords 
                     where !lstParentCode.Contains(r.GoodsCode) orderby r.GoodsCode 
                     select r).ToList();

                for (int i = 0; i < rootNode.Nodes.Count; i++)
                {
                    RecursionBuildTreeView(rootNode.Nodes[i], lstEF);
                }

                RecursionBuildTreeView(rootNode, lstEF);
            }

            treeView1.ExpandAll();
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            dgvERecord.DataSource = m_allTable;

            if (m_allTable != null)
            {
                dgvERecord.Columns[0].Visible = false;
            }

            if (m_dataLocalizer == null)
            {
                m_dataLocalizer = new UserControlDataLocalizer(dgvERecord, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dgvERecord.Name, BasicInfo.LoginID));

                panelLocalizer.Controls.Add(m_dataLocalizer);
                m_dataLocalizer.Dock = DockStyle.Bottom;
            }

            dgvERecord.Refresh();
        }

        /// <summary>
        /// 递归生成电子档案的树型结构
        /// </summary>
        /// <param name="parentNode">父总成编码</param>
        /// <param name="eRecords">电子档案信息</param>
        void RecursionBuildTreeView(TreeNode parentNode, List<P_ElectronFile> eRecords)
        {
            for (int i = 0; i < eRecords.Count; i++)
            {
                if (parentNode.Name == eRecords[i].ParentCode)
                {
                    TreeNode node = new TreeNode();

                    node.Name = eRecords[i].GoodsCode;
                    node.Text = eRecords[i].GoodsName;
                    node.ToolTipText = eRecords[i].Spec;
                    node.Tag = eRecords[i];
                    parentNode.Nodes.Add(node);

                    eRecords.RemoveAt(i);
                    i = -1;

                    RecursionBuildTreeView(node, eRecords);
                }
            }
        }

        private void btnEXCEL_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DataTableToExcel(saveFileDialog1, (DataTable)dgvStockInfo.DataSource, null);
        }

        private void btnStockCheck_Click(object sender, EventArgs e)
        {
            string strStorageID = UniversalFunction.GetStorageID(cmbStorage.Text);
            
            if (txtProductCode.Text.Trim() == "")
            {
                if (cmbStorage.Text.Contains("下线"))
                {
                    dgvStockInfo.DataSource = m_sell.GetInsertingCoilStockInfo(cmbStorage.Text);
                }
                else
                {
                    DataTable dt = m_sell.GetStockProductCodeCountInfo(strStorageID);

                    if (cmbStorage.Text.Contains("售后"))
                    {
                        dt.Columns.Add("已返修");
                        dt.Columns.Add("待返修");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dt.Rows[i]["已返修"] = m_sell.GetProductRepairStatusCount(
                                strStorageID, Convert.ToInt32(dt.Rows[i]["物品ID"]), true);
                            dt.Rows[i]["待返修"] = m_sell.GetProductRepairStatusCount(
                                strStorageID, Convert.ToInt32(dt.Rows[i]["物品ID"]), false);
                        }
                    }

                    dgvStockInfo.DataSource = dt;
                }

                dgvStockInfo.Columns["物品ID"].Visible = false;
            }
            else
            {
                dgvStockInfo.DataSource = m_sell.GetStockProductCodeInfo(cmbStorage.Tag.ToString(),
                        Convert.ToInt32(txtProductCode.Tag), "", txtProductCode.Text, "");
                dgvStockInfo.Columns["产品ID"].Visible = false;
                dgvStockInfo.Columns["库房ID"].Visible = false;
            }

            dgvOperationInfo.DataSource = null;
            dgvCustomerInfo.DataSource = null;
            dgvTruckLoadingInfo.DataSource = null;
            dgvERecord.DataSource = null;

            lbOffLineTestInfo.Text = "";
            lbAuditDate.Text = "";
            lbTightness.Text = "";
            lbTightnessDate.Text = "";
            lbWeigh.Text = "";
            lbWeighDate.Text = "";
        }

        /// <summary>
        /// 检测数据
        /// </summary>
        void CheckMessage()
        {
            CheckBusiness();
            CheckRecord();
            CheckCustomerInfo();
            CheckLoadingInfo();
            CheckDeliveryInspection();
            CheckOffLineTestInfo();
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (treeView1.SelectedNode != null)
            {
                P_ElectronFile data = treeView1.SelectedNode.Tag as P_ElectronFile;

                if (!m_serverModule.GetElectronFile(m_curProductCode, data.ParentCode, data.GoodsOnlyCode,
                    data.GoodsCode, data.Spec, out m_allTable, out m_err))
                {
                    MessageDialog.ShowErrorMessage(m_err);
                    return;
                }

                RefreshDataGridView();
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (dgvStockInfo.CurrentRow != null
                && dgvStockInfo.Columns[0].Name != "产品名称")
            {
                CheckMessage(); 
            }

        }

        private void btnFindPurpose_Click(object sender, EventArgs e)
        {
            FormQueryInfo dialog = QueryInfoDialog.GetProductCodeStockSearchMode("");

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                txtProductCode.Text = dialog.GetStringDataItem("箱体编号");
                cmbStorage.Tag = dialog.GetStringDataItem("库房ID").ToString();
                cmbStorage.Text = dialog.GetStringDataItem("库房名称").ToString();
                txtProductCode.Tag = Convert.ToInt32(dialog.GetStringDataItem("产品ID"));
                btnStockCheck.Focus();
            }
        }

        private void btnOperationFind_Click(object sender, EventArgs e)
        {
            产品编码业务查询 form = new 产品编码业务查询();

            form.ShowDialog();
        }

        private void dgvStockInfo_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            if (dgvStockInfo.Columns[0].Name != "产品名称")
            {
                for (int i = 0; i < dgvStockInfo.Rows.Count; i++)
                {
                    if (!(bool)dgvStockInfo.Rows[i].Cells["是否正常"].Value)
                    {
                        dgvStockInfo.Rows[i].DefaultCellStyle.BackColor = Color.Red;
                    }
                }
            }
        }

        private void dgvStockInfo_DoubleClick(object sender, EventArgs e)
        {
            if (dgvStockInfo.Rows.Count == 0)
            {
                return;
            }
            else
            {
                dgvOperationInfo.DataSource = null;
                dgvCustomerInfo.DataSource = null;
                dgvTruckLoadingInfo.DataSource = null;
                dgvERecord.DataSource = null;
                treeView1.Nodes.Clear();

                if (dgvStockInfo.Columns[0].Name == "产品名称")
                {
                    string strStorageID = UniversalFunction.GetStorageID(
                        dgvStockInfo.CurrentRow.Cells["库房名称"].Value.ToString());

                    cmbStorage.Tag = strStorageID;

                    if (cmbStorage.Text.Contains("下线"))
                    {
                        dgvStockInfo.DataSource = m_sell.GetStockProductCodeInfo(strStorageID,
                            Convert.ToInt32(dgvStockInfo.CurrentRow.Cells["物品ID"].Value), "下线", "", "");
                    }
                    else
                    {
                        dgvStockInfo.DataSource = m_sell.GetStockProductCodeInfo(strStorageID,
                            Convert.ToInt32(dgvStockInfo.CurrentRow.Cells["物品ID"].Value), "库房", "", "");
                    }

                    if (cmbStorage.Text.Trim() != "售后库房")
                    {
                        dgvStockInfo.Columns["返修状态"].Visible = false;
                    }

                    dgvStockInfo.Columns["产品ID"].Visible = false;
                    dgvStockInfo.Columns["库房ID"].Visible = false;

                }
                else
                {
                    CheckMessage();
                }
            }
        }

        private void dgvStockInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvStockInfo.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvStockInfo.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvStockInfo.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvERecord_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvERecord.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvERecord.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvERecord.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvOperationInfo_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvOperationInfo.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvOperationInfo.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvOperationInfo.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void dgvRoutineTest_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                dgvRoutineTest.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                dgvRoutineTest.RowHeadersDefaultCellStyle.Font,
                rectangle,
                dgvRoutineTest.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        private void cmbStorage_TextChanged(object sender, EventArgs e)
        {
            cmbStorage.Tag = UniversalFunction.GetStorageID(cmbStorage.Text.ToString().Trim());
        }
    }
}
