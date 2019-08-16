using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using PlatformManagement;
using ServerModule;
using GlobalObject;
using UniversalControlLibrary;
using Service_Manufacture_Storage;

namespace Expression
{
    /// <summary>
    /// 发料清单设置界面
    /// </summary>
    public partial class 发料清单设置 : Form
    {
        /// <summary>
        /// BOM表服务组件
        /// </summary>
        ServerModule.IBomServer m_serverBom = ServerModule.ServerModuleFactory.GetServerModule<ServerModule.IBomServer>();

        ///// <summary>
        ///// 数据表
        ///// </summary>
        //DataTable m_dataTable = new DataTable();
        
        /// <summary>
        /// 初始化坐标
        /// </summary>
        private Point m_pt;

        public Point Pt
        {
            get { return m_pt; }
            set { m_pt = value; }
        }

        /// <summary>
        /// 所属总成编码
        /// </summary>
        string m_strEdition;

        /// <summary>
        /// 错误信息
        /// </summary>
        string m_err;

        /// <summary>
        /// 拖动的源数据行索引
        /// </summary>
        int m_intIndexOfItemUnderMouseToDrag = -1;
        
        /// <summary>
        /// 拖动的目标数据行索引
        /// </summary>
        private int m_intIndexOfItemUnderMouseToDrop = -1;
        
        /// <summary>
        /// 拖动中的鼠标所在位置的当前行索引
        /// </summary>
        private int m_intIndexOfItemUnderMouseOver = -1;
        
        /// <summary>
        /// 不启用拖放的鼠标范围
        /// </summary>
        private Rectangle m_dragBoxFromMouseDown = Rectangle.Empty;

        /// <summary>
        /// 发料清单服务组件
        /// </summary>
        Service_Manufacture_Storage.IProductOrder m_findProductOrder = Service_Manufacture_Storage.ServerModuleFactory.GetServerModule<Service_Manufacture_Storage.IProductOrder>();

        public 发料清单设置()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            cmbApplicable.DataSource = GeneralFunction.GetEumnList(typeof(CE_DebitScheduleApplicable));

            tbsEditionName.Focus();
        }

        /// <summary>
        /// 定位记录
        /// </summary>
        /// <param name="msg">定位信息</param>
        void PositioningRecord(DataGridView datagridview, string code, string name, string spec)
        {
            for (int i = 0; i < datagridview.Rows.Count; i++)
            {
                if ((string)datagridview.Rows[i].Cells["零件编码"].Value == code
                    && (string)datagridview.Rows[i].Cells["零件名称"].Value == name
                    && (string)datagridview.Rows[i].Cells["规格"].Value == spec)
                {
                    datagridview.FirstDisplayedScrollingRowIndex = i;
                    datagridview.CurrentCell = datagridview.Rows[i].Cells["零件编码"];
                }
            }
        }

        void txtGoodsName_OnCompleteSearch()
        {
            txtGoodsName.Text = txtGoodsName.DataResult["物品名称"].ToString();
            txtGoodsCode.Text = txtGoodsName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtGoodsName.DataResult["规格"].ToString();
            txtGoodsCode.Tag = txtGoodsName.DataResult["序号"].ToString();

            txtCount.Text = m_serverBom.GetBomCounts(tbsEditionName.Tag.ToString(), txtGoodsCode.Text, 
                txtGoodsName.Text, txtSpec.Text).ToString();
        }

        void tbsEditionName_OnCompleteSearch()
        {
            if (cmbApplicable.Text.Length == 0)
            {
                MessageDialog.ShowPromptMessage("请先选择适用范围");
                tbsEditionName.Tag = null;
                tbsEditionName.Text = "";
                return;
            }

            tbsEditionName.Tag = tbsEditionName.DataResult["图号型号"].ToString();

            m_strEdition = tbsEditionName.DataResult["图号型号"].ToString();

            ShowDgv();
        }

        private void ShowDgv()
        {
            Dgv_Main.DataSource = m_findProductOrder.GetAllData(tbsEditionName.Tag.ToString(), 
                GeneralFunction.StringConvertToEnum<CE_DebitScheduleApplicable>(cmbApplicable.Text)); 
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                if (Convert.ToInt32(txtCount.Text) == 0)
                {
                    MessageDialog.ShowPromptMessage("基数不能为0");
                    return;
                }

                DataTable dtTemp = (DataTable)Dgv_Main.DataSource;

                DataRow dr = dtTemp.NewRow();

                dr["产品编码"] = tbsEditionName.Tag.ToString();
                dr["父级编码"] = txtGoodsCode.Tag.ToString();
                dr["零件编码"] = txtGoodsCode.Text;
                dr["零件名称"] = txtGoodsName.Text;
                dr["规格"] = txtSpec.Text;
                dr["基数"] = Convert.ToInt32(txtCount.Text);
                dr["别名"] = txtByname.Text;
                dr["顺序位置"] = 0;

                dtTemp.Rows.Add(dr);
                Dgv_Main.DataSource = dtTemp;

                PositioningRecord(Dgv_Main, dr["零件编码"].ToString(), dr["零件名称"].ToString(), dr["规格"].ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (txtGoodsCode.Text.Trim().Length == 0)
            {
                MessageDialog.ShowPromptMessage("请录入物品");
                return;
            }

            Dgv_Main.CurrentRow.Cells["产品编码"].Value = tbsEditionName.Tag;
            Dgv_Main.CurrentRow.Cells["父级编码"].Value = txtGoodsCode.Tag;
            Dgv_Main.CurrentRow.Cells["零件编码"].Value = txtGoodsCode.Text;
            Dgv_Main.CurrentRow.Cells["零件名称"].Value = txtGoodsName.Text;
            Dgv_Main.CurrentRow.Cells["规格"].Value = txtSpec.Text;
            Dgv_Main.CurrentRow.Cells["基数"].Value = Convert.ToInt32(txtCount.Text);
            Dgv_Main.CurrentRow.Cells["别名"].Value = txtByname.Text;
            Dgv_Main.CurrentRow.Cells["顺序位置"].Value = 0;

            string code = txtGoodsCode.Text;
            string name = txtGoodsName.Text;
            string spec = txtSpec.Text;

            DataTable dtTemp = (DataTable)Dgv_Main.DataSource;

            if (dtTemp != null)
            {
                dtTemp.AcceptChanges();
            }

            Dgv_Main.DataSource = dtTemp;
            PositioningRecord(Dgv_Main, code, name, spec);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (Dgv_Main.CurrentRow != null)
            {
                Dgv_Main.Rows.RemoveAt(Dgv_Main.CurrentRow.Index);
                DataTable dtTemp = (DataTable)Dgv_Main.DataSource;
                dtTemp.AcceptChanges();
                Dgv_Main.DataSource = dtTemp;
            }
        }

        #region 行拖动

        private void dataGridView_MouseDown(object sender, MouseEventArgs e)
        {
            // 通过鼠标按下的位置获取所在行的信息
            var hitTest = Dgv_Main.HitTest(e.X, e.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell)
                return;

            // 记下拖动源数据行的索引及已鼠标按下坐标为中心的不会开始拖动的范围
            m_intIndexOfItemUnderMouseToDrag = hitTest.RowIndex;

            if (m_intIndexOfItemUnderMouseToDrag > -1)
            {
                Size dragSize = SystemInformation.DragSize;
                m_dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                m_dragBoxFromMouseDown = Rectangle.Empty;

        }

        private void dataGridView_MouseUp(object sender, MouseEventArgs e)
        {
            // 释放鼠标按键时清空变量为默认值
            m_dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dataGridView_MouseMove(object sender, MouseEventArgs e)
        {
            // 不是鼠标左键按下时移动
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;

            // 如果鼠标在不启用拖动的范围内
            if (m_dragBoxFromMouseDown == Rectangle.Empty || m_dragBoxFromMouseDown.Contains(e.X, e.Y))
                return;

            // 如果源数据行索引值不正确
            if (m_intIndexOfItemUnderMouseToDrag < 0)
                return;

            // 开始拖动，第一个参数表示要拖动的数据，可以自定义，一般是源数据行
            var row = Dgv_Main.Rows[m_intIndexOfItemUnderMouseToDrag];
            DragDropEffects dropEffect = Dgv_Main.DoDragDrop(row, DragDropEffects.All);

            //拖动过程结束后清除拖动位置行的红线效果
            OnRowDragOver(-1);
        }

        private void dataGridView_DragOver(object sender, DragEventArgs e)
        {
            // 把屏幕坐标转换成控件坐标
            Point p = Dgv_Main.PointToClient(new Point(e.X, e.Y));

            // 通过鼠标按下的位置获取所在行的信息
            // 如果不是在数据行或者在源数据行上则不能作为拖放的目标
            var hitTest = Dgv_Main.HitTest(p.X, p.Y);

            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == m_intIndexOfItemUnderMouseToDrag)
            {
                e.Effect = DragDropEffects.None;
                OnRowDragOver(-1);
                return;
            }

            // 设置为作为拖放移动的目标
            e.Effect = DragDropEffects.Move;
            // 通知目标行重绘
            OnRowDragOver(hitTest.RowIndex);
        }

        private void dataGridView_DragDrop(object sender, DragEventArgs e)
        {
            DataTable dtTemp = (DataTable)Dgv_Main.DataSource;

            // 把屏幕坐标转换成控件坐标
            Point p = Dgv_Main.PointToClient(new Point(e.X, e.Y));

            // 如果当前位置不是数据行
            // 或者刚好是源数据行的下一行（本示例中假定拖放操作为拖放至目标行的上方）
            // 则不进行任何操作
            var hitTest = Dgv_Main.HitTest(p.X, p.Y);

            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == m_intIndexOfItemUnderMouseToDrag + 1)
                return;

            m_intIndexOfItemUnderMouseToDrop = hitTest.RowIndex;

            // * 执行拖放操作(执行的逻辑按实际需要)

            var tempRow = dtTemp.NewRow();
            tempRow.ItemArray = dtTemp.Rows[m_intIndexOfItemUnderMouseToDrag].ItemArray;
            dtTemp.Rows.RemoveAt(m_intIndexOfItemUnderMouseToDrag);

            if (m_intIndexOfItemUnderMouseToDrag < m_intIndexOfItemUnderMouseToDrop)
                m_intIndexOfItemUnderMouseToDrop--;

            dtTemp.Rows.InsertAt(tempRow, m_intIndexOfItemUnderMouseToDrop);
            dtTemp.AcceptChanges();
            Dgv_Main.DataSource = dtTemp;

            //Dgv_Main.FirstDisplayedScrollingRowIndex = m_intIndexOfItemUnderMouseToDrop;
        }

        private void dataGridView_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            // 如果当前行是鼠标拖放过程的所在行
            if (e.RowIndex == m_intIndexOfItemUnderMouseOver)
                e.Graphics.FillRectangle(Brushes.Red, e.RowBounds.X, e.RowBounds.Y, e.RowBounds.Width, 2);
        }

        private void OnRowDragOver(int rowIndex)
        {
            // 如果和上次导致重绘的行是同一行则无需重绘
            if (m_intIndexOfItemUnderMouseOver == rowIndex)
                return;

            int old = m_intIndexOfItemUnderMouseOver;
            m_intIndexOfItemUnderMouseOver = rowIndex;

            // 去掉原有行的红线
            if (old > -1)
                Dgv_Main.InvalidateRow(old);

            // 绘制新行的红线
            if (rowIndex > -1)
                Dgv_Main.InvalidateRow(rowIndex);

            if (rowIndex == -1)
            {
                Dgv_Main.CurrentCell = Dgv_Main.Rows[m_intIndexOfItemUnderMouseToDrop].Cells["零件编码"];
            }
        }

        #endregion

        private void btnSave_Click(object sender, EventArgs e)
        {
            DataTable dtTemp = (DataTable)Dgv_Main.DataSource;

            for (int i = 0; i < dtTemp.Rows.Count; i++)
            {
                dtTemp.Rows[i]["顺序位置"] = i;
            }

            if (tbsEditionName.Tag.ToString() == "")
            {
                MessageBox.Show("请录入总成名称","提示");
                tbsEditionName.Focus();
                return;
            }

            if (m_findProductOrder.SaveDate(tbsEditionName.Tag.ToString(), dtTemp,
                GlobalObject.GeneralFunction.StringConvertToEnum<CE_DebitScheduleApplicable>(cmbApplicable.Text), out m_err))
            {
                MessageBox.Show("保存成功", "提示");
                ShowDgv();
                return;
            }
            else
            {
                MessageDialog.ShowErrorMessage(m_err);
                return;
            }
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, Dgv_Main);
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            DataTable dtTemp = ExcelHelperP.RenderFromExcel(openFileDialog1.OpenFile());

            if (dtTemp == null)
            {
                MessageDialog.ShowPromptMessage(m_err);
                return;
            }
            else
            {
                if (!dtTemp.Columns.Contains("零件编码") 
                    || !dtTemp.Columns.Contains("零件名称") 
                    || !dtTemp.Columns.Contains("规格")
                    || !dtTemp.Columns.Contains("基数"))
                {
                    MessageDialog.ShowPromptMessage("表中的列未包含【零件编码】，【零件名称】，【规格】，【基数】，请重新核对");
                }

                DataTable sourceTable = ((DataTable)Dgv_Main.DataSource).Clone();

                foreach (DataRow dr in dtTemp.Rows)
                {
                    if (dr["零件编码"].ToString().Trim().Length > 0 
                        || dr["零件名称"].ToString().Trim().Length > 0)
                    {
                        DataRow tempRow = sourceTable.NewRow();

                        tempRow["产品编码"] = tbsEditionName.Tag.ToString();
                        tempRow["父级编码"] = "";
                        tempRow["零件编码"] = dr["零件编码"];
                        tempRow["零件名称"] = dr["零件名称"];
                        tempRow["规格"] = dr["规格"];
                        tempRow["基数"] = dr["基数"];
                        tempRow["别名"] = "";
                        tempRow["顺序位置"] = 0;

                        sourceTable.Rows.Add(tempRow);
                    }
                }

                Dgv_Main.DataSource = sourceTable;
            }
        }

        private void cmbApplicable_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tbsEditionName.Text.Trim().Length == 0)
            {
                return;
            }

            ShowDgv();
        }
    }
}
