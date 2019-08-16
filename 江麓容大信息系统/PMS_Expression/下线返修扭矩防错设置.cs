/******************************************************************************
 *
 * 文件名称:  下线返修扭矩防错设置.cs
 * 作者    :  邱瑶       日期: 2014/07/7
 * 开发平台:  vs2008(c#)
 * 用于    :  装配线管理信息系统
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
using UniversalControlLibrary;
using PlatformManagement;
using GlobalObject;

namespace Expression
{
    public partial class 下线返修扭矩防错设置 : Form
    {
        /// <summary>
        /// 错误信息
        /// </summary>
        string m_error;

        /// <summary>
        /// 权限标志
        /// </summary>
        AuthorityFlag m_authFlag;

        /// <summary>
        /// 可供查找的所有字段
        /// </summary>
        string[] m_findField = null;

        /// <summary>
        /// 产品信息服务组件
        /// </summary>
        IProductInfoServer m_productInfoServer = ServerModuleFactory.GetServerModule<IProductInfoServer>();

        /// <summary>
        /// 下线防错数据服务类
        /// </summary>
        IOfflineFailSafeServer m_offlineFailServer = ServerModuleFactory.GetServerModule<IOfflineFailSafeServer>();

        /// <summary>
        /// 产品信息
        /// </summary>
        IQueryable<View_P_ProductInfo> m_productInfo;

        /// <summary>
        /// 下线返修扭矩防错信息
        /// </summary>
        List<View_ZPX_OfflineFailsafe> m_list;

        /// <summary>
        /// 记录操作日志服务类
        /// </summary>
        ISystemLogServer m_sysLogServer = ServerModuleFactory.GetServerModule<ISystemLogServer>();

        /// <summary>
        /// 选择的行索引
        /// </summary>
        int m_selectionIdx = -1;

        public 下线返修扭矩防错设置(FunctionTreeNodeInfo nodeInfo)
        {
            InitializeComponent();

            m_authFlag = nodeInfo.Authority;

            #region 获取所有产品编码(产品类型)信息

            if (!m_productInfoServer.GetAllProductInfo(out m_productInfo, out m_error))
            {
                MessageDialog.ShowErrorMessage(m_error);
                return;
            }

            if (m_productInfo != null)
            {
                foreach (var item in m_productInfo)
                {
                    cmbProductType.Items.Add(item.产品类型编码);
                    cmbQuery.Items.Add(item.产品类型编码);
                }

                cmbQuery.Items.Add("全部");
                cmbProductType.SelectedIndex = 0;
                cmbQuery.SelectedIndex = 0;
            }

            #endregion

            DataTable dt = m_offlineFailServer.GetPhase();

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbPhase.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbPhase.Items.Add(dt.Rows[i]["Phase"].ToString());
                }
            }

            RefreshDataGridView();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtGoodsCode.Text = "";
            txtGoodsName.Text = "";
            txtSpec.Text = "";
            txtMessage.Text = "";
            cmbProductType.Text = "";
            txtPositionMessage.Text = "";
            cmbPhase.SelectedIndex = -1;
            cmbType.SelectedIndex = -1;
            cbIsDistinguish.Checked = false;
            numNumCount.Value = 0;
            numTorque.Value = 0;
        }

        /// <summary>
        /// 刷新
        /// </summary>
        void RefreshDataGridView()
        {
            m_list = m_offlineFailServer.GetBillByProductType(cmbQuery.Text);
            dataGridView1.DataSource = GlobalObject.GeneralFunction.ConvertToDataTable(m_list);

            this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                this.dataGridView1_ColumnWidthChanged);

            // 添加查询用的列
            if (m_findField == null)
            {
                List<string> lstColumnName = new List<string>();

                for (int i = 0; i < dataGridView1.Columns.Count; i++)
                {
                    if (dataGridView1.Columns[i].Visible)
                    {
                        lstColumnName.Add(dataGridView1.Columns[i].Name);
                    }
                }

                m_findField = lstColumnName.ToArray();
            }

            userControlDataLocalizer1.Init(dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));

            dataGridView1.Refresh();
        }

        private void cmbProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbProductType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择产品型号");
                return;
            }

            cmbParentName.Items.AddRange(
                    ServerModuleFactory.GetServerModule<IAssemblingBom>().GetParentNames());
        }

        private void 下线返修扭矩防错设置_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
        }

        private void 设置阶段toolStripButton_Click(object sender, EventArgs e)
        {
            设置阶段 frm = new 设置阶段();

            frm.ShowDialog();

            DataTable dt = m_offlineFailServer.GetPhase();

            if (dt != null && dt.Rows.Count > 0)
            {
                cmbPhase.Items.Clear();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    cmbPhase.Items.Add(dt.Rows[i]["Phase"].ToString());
                }
            }
        }

        /// <summary>
        /// 检查控件
        /// </summary>
        /// <returns>正确返回true否则返回false</returns>
        bool CheckControl()
        {
            if (cmbType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择返修类型！");
                return false;
            }

            if (cmbProductType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择产品型号！");
                return false;
            }

            if (cmbType.SelectedIndex == 0)
            {
                if (cmbPhase.SelectedIndex == -1)
                {
                    MessageDialog.ShowPromptMessage("请选择阶段！");
                    return false;
                }
            }
            else if (cmbType.SelectedIndex == 1)
            {
                if (cmbParentName.SelectedIndex == -1)
                {
                    MessageDialog.ShowPromptMessage("请选择分总成！");
                    return false;
                }
            }            

            if (txtGoodsCode.Text.Trim() == ""
                && txtGoodsName.Text.Trim() == "" && txtSpec.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请选择零件信息！");
                return false;
            }

            if (numTorque.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请输入正确的扭矩大小！");
                return false;
            }

            if (txtMessage.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入提示信息！");
                return false;
            }

            if (txtPositionMessage.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请输入作用位置！");
                return false;
            }

            if (numNumCount.Value == 0)
            {
                MessageDialog.ShowPromptMessage("请输入数量！");
                return false;
            }

            return true;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            ZPX_OfflineFailsafe failSafe = new ZPX_OfflineFailsafe();

            failSafe.GoodsCode = txtGoodsCode.Text;
            failSafe.GoodsName = txtGoodsName.Text;
            failSafe.IsDistinguish = cbIsDistinguish.Checked;
            failSafe.Message = txtMessage.Text;
            failSafe.NumCount = (int)numNumCount.Value;

            if (cmbType.SelectedIndex == 0)
            {
                failSafe.Phase = cmbPhase.Text;
                failSafe.SourceProductType = "";
            }
            else
            {
                failSafe.SourceProductType = cmbParentName.Text;
                failSafe.Phase = "";
            }

            failSafe.PositionMessage = txtPositionMessage.Text;
            failSafe.ProductType = cmbProductType.Text;
            failSafe.Recorder = BasicInfo.LoginID;
            failSafe.RecordTime = ServerTime.Time;
            failSafe.Spec = txtSpec.Text;
            failSafe.Torque = (int)numTorque.Value;
            failSafe.Type = cmbType.Text;

            int orderNo = m_offlineFailServer.GetOrderNoMax(cmbType.Text, cmbProductType.Text, failSafe.SourceProductType, failSafe.Phase);

            failSafe.OrderNo = ++orderNo;

            if (!m_offlineFailServer.InsertFailsafe(failSafe, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("添加成功！");
            m_sysLogServer.RecordLog<ZPX_OfflineFailsafe>(CE_OperatorMode.添加, failSafe, failSafe);
            RefreshDataGridView();
        }

        private void 刷新toolStripButton_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                cmbParentName.Text = dataGridView1.CurrentRow.Cells["分装总成"].Value.ToString();
                cmbPhase.Text = dataGridView1.CurrentRow.Cells["阶段"].Value.ToString();
                cmbProductType.Text = dataGridView1.CurrentRow.Cells["产品型号"].Value.ToString();
                cmbType.Text = dataGridView1.CurrentRow.Cells["返修类型"].Value.ToString();
                txtGoodsCode.Text = dataGridView1.CurrentRow.Cells["图号型号"].Value.ToString();
                txtGoodsName.Text = dataGridView1.CurrentRow.Cells["物品名称"].Value.ToString();
                txtMessage.Text = dataGridView1.CurrentRow.Cells["提示"].Value.ToString();
                txtPositionMessage.Text = dataGridView1.CurrentRow.Cells["位置信息"].Value.ToString();
                txtSpec.Text = dataGridView1.CurrentRow.Cells["规格"].Value.ToString();
                cbIsDistinguish.Checked = Convert.ToBoolean(dataGridView1.CurrentRow.Cells["是否手动判别"].Value);
                numNumCount.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["数量"].Value);
                numTorque.Value = Convert.ToDecimal(dataGridView1.CurrentRow.Cells["扭矩大小"].Value);
            }
        }

        private void 删除toolStripButton_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 1)
            {
                if (MessageDialog.ShowEnquiryMessage("确定删除选中的信息吗？") == DialogResult.Yes)
                {
                    ZPX_OfflineFailsafe failsafe = new ZPX_OfflineFailsafe();

                    failsafe.GoodsCode = txtGoodsCode.Text.Trim();
                    failsafe.GoodsName = txtGoodsName.Text.Trim();
                    failsafe.Spec = txtSpec.Text.Trim();
                    failsafe.Type = cmbType.Text;
                    failsafe.ProductType = cmbProductType.Text;
                    failsafe.Phase = dataGridView1.CurrentRow.Cells["阶段"].Value.ToString();
                    failsafe.SourceProductType = dataGridView1.CurrentRow.Cells["分装总成"].Value.ToString();
                    failsafe.PositionMessage = dataGridView1.CurrentRow.Cells["位置信息"].Value.ToString();

                    if (!m_offlineFailServer.DeleteFailSafe(failsafe.ProductType,failsafe.SourceProductType, 
                        failsafe.GoodsCode,failsafe.GoodsName, failsafe.Spec,failsafe.Phase,failsafe.PositionMessage, out m_error))
                    {
                        MessageDialog.ShowPromptMessage(m_error);
                        return;
                    }

                    m_sysLogServer.RecordLog<ZPX_OfflineFailsafe>(CE_OperatorMode.删除,failsafe,failsafe);
                    RefreshDataGridView();
                }
            }
            else
            {
                MessageDialog.ShowPromptMessage("请选中一行数据再进行操作！");
            }
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }

        private void cmbType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbType.SelectedIndex == 0)
            {
                cmbParentName.Enabled = false;
                cmbPhase.Enabled = true;
            }
            else
            {
                cmbParentName.Enabled = true;
                cmbPhase.Enabled = false;
            }
        }

        private void cmbProductType_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (cmbProductType.SelectedIndex == -1)
            {
                MessageDialog.ShowPromptMessage("请选择产品型号");
                return;
            }

            cmbParentName.Items.Clear();
            cmbParentName.Items.AddRange(
                    ServerModuleFactory.GetServerModule<IAssemblingBom>().GetParentNames(cmbProductType.Text));
        }

        private void btnFindCode_Click(object sender, EventArgs e)
        {
            FormQueryInfo form = QueryInfoDialog.GetAllGoodsInfo();

            if (form != null && form.ShowDialog() == DialogResult.OK)
            {
                txtGoodsCode.Text = form.GetDataItem("图号型号").ToString();
                txtGoodsName.Text = form.GetDataItem("物品名称").ToString();
                txtSpec.Text = form.GetDataItem("规格").ToString();
            }
        }

        private void dataGridView1_DragDrop(object sender, DragEventArgs e)
        {
            int idx = GetRowFromPoint(e.X, e.Y);

            if (idx < 0 || m_selectionIdx == idx)
                return;

            if (e.Data.GetDataPresent(typeof(DataGridViewRow)))
            {
                DataGridViewRow row = (DataGridViewRow)e.Data.GetData(typeof(DataGridViewRow));
                DataTable dt = (DataTable)dataGridView1.DataSource;

                var tempRow = dt.NewRow();

                tempRow.ItemArray = dt.Rows[m_selectionIdx].ItemArray;

                this.dataGridView1.CellEnter -= new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                dt.Rows.RemoveAt(m_selectionIdx);

                dt.Rows.InsertAt(tempRow, idx);

                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);

                m_selectionIdx = idx;

                dataGridView1.Rows[m_selectionIdx].Selected = true;

                int visibleColumn = StapleInfo.GetVisibleColumn(dataGridView1);

                dataGridView1.CurrentCell = dataGridView1.Rows[m_selectionIdx].Cells[visibleColumn];
            }
        }

        /// <summary>
        /// 根据鼠标按键被释放时的鼠标位置计算行序号
        /// </summary>
        /// <param name="x">鼠标x轴</param>
        /// <param name="y">鼠标y轴</param>
        /// <returns>返回获取的行号</returns>
        private int GetRowFromPoint(int x, int y)
        {
            for (int i = 0; i < dataGridView1.RowCount; i++)
            {
                Rectangle rec = dataGridView1.GetRowDisplayRectangle(i, false);

                if (dataGridView1.RectangleToScreen(rec).Contains(x, y))
                    return i;
            }

            return -1;
        }

        private void dataGridView1_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
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

        private void dataGridView1_CellMouseMove(object sender, DataGridViewCellMouseEventArgs e)
        {
            if ((e.Clicks < 2) && (e.Button == MouseButtons.Left))
            {
                if ((e.ColumnIndex == -1) && (e.RowIndex > -1))
                {
                    m_selectionIdx = e.RowIndex;
                    dataGridView1.DoDragDrop(dataGridView1.Rows[e.RowIndex], DragDropEffects.Move);
                }
            }
        }

        private void 保存顺序toolStripButton_Click(object sender, EventArgs e)
        {
            List<View_ZPX_OfflineFailsafe> lstData = new List<View_ZPX_OfflineFailsafe>();

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                DataGridViewCellCollection cells = dataGridView1.Rows[i].Cells;

                lstData.Add(m_list.Find(p =>
                    p.产品型号 == cells["产品型号"].Value.ToString() &&
                    p.返修类型 == cells["返修类型"].Value.ToString() &&
                    p.分装总成 == cells["分装总成"].Value.ToString() &&
                    p.规格 == cells["规格"].Value.ToString() &&
                    p.阶段 == cells["阶段"].Value.ToString() &&
                    p.扭矩大小 == (int)(cells["扭矩大小"].Value) &&
                    p.数量 == (int)(cells["数量"].Value) &&
                    p.是否手动判别 == (bool)(cells["是否手动判别"].Value) &&
                    p.图号型号 == cells["图号型号"].Value.ToString() &&
                    p.物品名称 == cells["物品名称"].Value.ToString() &&
                    p.位置信息==cells["位置信息"].Value.ToString()));
            }

            if (lstData.Count > 0)
            {
                lstData = lstData.OrderBy(p => p.产品型号).ToList();

                try
                {
                    m_offlineFailServer.SaveOrderNo(lstData);

                    RefreshDataGridView();
                }
                catch (Exception exce)
                {
                    MessageDialog.ShowErrorMessage(exce.Message);
                }
            }
        }

        private void btnUp_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;

            if (index > 0)
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;
                var tempRow = dt.NewRow();

                tempRow.ItemArray = dt.Rows[index].ItemArray;

                dt.Rows.RemoveAt(index);
                dt.Rows.InsertAt(tempRow, index - 1);

                dataGridView1.DataSource = dt;
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
                m_selectionIdx = index - 1;

                dataGridView1.Rows[m_selectionIdx].Selected = true;

                int visibleColumn = StapleInfo.GetVisibleColumn(dataGridView1);

                dataGridView1.CurrentCell = dataGridView1.Rows[m_selectionIdx].Cells[visibleColumn];
                
            }
        }

        private void btnSearchProduct_Click(object sender, EventArgs e)
        {
            RefreshDataGridView();
        }

        private void 日志toolStripButton_Click(object sender, EventArgs e)
        {
            IAuthorization authorization = PlatformFactory.GetObject<IAuthorization>();
            string businessID = "下线返修扭矩防错设置";
            IQueryResult qr = authorization.Query(businessID, null, null, 0);
            List<string> lstFindField = new List<string>();
            DataColumnCollection columns = qr.DataCollection.Tables[0].Columns;

            if (qr.Succeeded && columns.Count > 0)
            {
                for (int i = 0; i < columns.Count; i++)
                {
                    lstFindField.Add(columns[i].ColumnName);
                }
            }

            FormConditionFind formFindCondition = new FormConditionFind(this, lstFindField.ToArray(), businessID, labelTitle.Text);
            formFindCondition.ShowDialog();
        }

        private void 修改toolStripButton_Click(object sender, EventArgs e)
        {
            if (!CheckControl())
            {
                return;
            }

            DataGridViewRow row = dataGridView1.CurrentRow;

            ZPX_OfflineFailsafe oldFailsafe = new ZPX_OfflineFailsafe();

            oldFailsafe.GoodsCode = row.Cells["图号型号"].Value.ToString();
            oldFailsafe.GoodsName = row.Cells["物品名称"].Value.ToString();
            oldFailsafe.IsDistinguish = Convert.ToBoolean(row.Cells["是否手动判别"].Value);
            oldFailsafe.Message = row.Cells["提示"].Value.ToString();
            oldFailsafe.NumCount = Convert.ToInt32(row.Cells["数量"].Value);
            oldFailsafe.PositionMessage = row.Cells["位置信息"].Value.ToString();
            oldFailsafe.ProductType = row.Cells["产品型号"].Value.ToString();
            oldFailsafe.Recorder = BasicInfo.LoginID;
            oldFailsafe.RecordTime = ServerTime.Time;
            oldFailsafe.Spec = row.Cells["规格"].Value.ToString();
            oldFailsafe.Torque = Convert.ToInt32(row.Cells["扭矩大小"].Value);
            oldFailsafe.Type = row.Cells["返修类型"].Value.ToString();
            oldFailsafe.Phase = row.Cells["阶段"].Value == DBNull.Value ? null :
                row.Cells["阶段"].Value.ToString();
            oldFailsafe.SourceProductType = row.Cells["分装总成"].Value == DBNull.Value ? null :
                row.Cells["分装总成"].Value.ToString();

            ZPX_OfflineFailsafe failSafe = new ZPX_OfflineFailsafe();

            failSafe.GoodsCode = txtGoodsCode.Text;
            failSafe.GoodsName = txtGoodsName.Text;
            failSafe.IsDistinguish = cbIsDistinguish.Checked;
            failSafe.Message = txtMessage.Text;
            failSafe.NumCount = (int)numNumCount.Value;

            if (cmbType.SelectedIndex == 0)
            {
                failSafe.Phase = cmbPhase.Text;
                failSafe.SourceProductType = "";
            }
            else
            {
                failSafe.SourceProductType = cmbParentName.Text;
                failSafe.Phase = "";
            }

            failSafe.PositionMessage = txtPositionMessage.Text;
            failSafe.ProductType = cmbProductType.Text;
            failSafe.Recorder = BasicInfo.LoginID;
            failSafe.RecordTime = ServerTime.Time;
            failSafe.Spec = txtSpec.Text;
            failSafe.Torque = (int)numTorque.Value;
            failSafe.Type = cmbType.Text;

            failSafe.OrderNo = Convert.ToInt32(dataGridView1.CurrentRow.Cells["防错顺序"].Value);

            if (!m_offlineFailServer.UpdateFailsafe(Convert.ToInt32(dataGridView1.CurrentRow.Cells["序号"].Value),failSafe, out m_error))
            {
                MessageDialog.ShowPromptMessage(m_error);
                return;
            }

            MessageDialog.ShowPromptMessage("修改成功！");
            m_sysLogServer.RecordLog<ZPX_OfflineFailsafe>(CE_OperatorMode.修改, failSafe, oldFailsafe);
            RefreshDataGridView();
        }

        private void 复制toolStripButton_Click(object sender, EventArgs e)
        {
            下线返修复制产品型号 frm = new 下线返修复制产品型号();

            frm.ShowDialog();
        }

        private void btnDown_Click(object sender, EventArgs e)
        {
            int index = dataGridView1.CurrentRow.Index;

            if (index < dataGridView1.Rows.Count)
            {
                DataTable dt = (DataTable)dataGridView1.DataSource;
                var tempRow = dt.NewRow();

                tempRow.ItemArray = dt.Rows[index].ItemArray;

                dt.Rows.RemoveAt(index);
                dt.Rows.InsertAt(tempRow, index + 1);

                dataGridView1.DataSource = dt;
                this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
                m_selectionIdx = index + 1;

                dataGridView1.Rows[m_selectionIdx].Selected = true;

                int visibleColumn = StapleInfo.GetVisibleColumn(dataGridView1);

                dataGridView1.CurrentCell = dataGridView1.Rows[m_selectionIdx].Cells[visibleColumn];
            }
        }
    }
}
