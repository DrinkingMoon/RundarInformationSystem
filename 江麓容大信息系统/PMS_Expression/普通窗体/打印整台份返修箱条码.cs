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
using System.Collections;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 打印整台份返修箱条码 : Form
    {
        /// <summary>
        /// 返修零件信息服务
        /// </summary>
        IReparativePartInfoServer m_reparativePartInfoServer = ServerModuleFactory.GetServerModule<IReparativePartInfoServer>();

        /// <summary>
        /// 要打印的数据
        /// </summary>        
        public List<View_ZPX_ReparativeBarcode> PrintData
        {
            get
            {
                if (dataGridView1.Rows.Count == 0)
                {
                    return null;
                }

                List<View_ZPX_ReparativeBarcode> lstData = new List<View_ZPX_ReparativeBarcode>();

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    View_ZPX_ReparativeBarcode data = (dataGridView1.Rows[i].Tag as StateData<View_ZPX_ReparativeBarcode>).Data;

                    data.供货单位 = dataGridView1.Rows[i].Cells["供货单位"].Value.ToString();
                    data.数量 = Convert.ToInt32(dataGridView1.Rows[i].Cells["数量"].Value.ToString());

                    lstData.Add(data);
                }

                return lstData;
            }
        }

        /// <summary>
        /// 数据列表
        /// </summary>
        List<StateData<View_ZPX_ReparativeBarcode>> m_lstData = new List<StateData<View_ZPX_ReparativeBarcode>>();

        /// <summary>
        /// 是否打印
        /// </summary>
        public bool blPrintFlag = false;

        /// <summary>
        /// 产品编码
        /// </summary>
        public string m_edition;

        /// <summary>
        /// 条形码在装配多批次管理中的用途编号
        /// </summary>
        public int PurposeID
        {
            get { return Convert.ToInt32(cmbPurpose.SelectedValue); }
        }

        public 打印整台份返修箱条码()
        {
            InitializeComponent();

            dataGridView1.Columns["数量"].ValueType = typeof(int);

            cmbPurpose.DataSource = ServerModuleFactory.GetServerModule<IMultiBatchPartServer>().GetPersonnelPurpose();
            cmbPurpose.DisplayMember = "装配用途名称";
            cmbPurpose.ValueMember = "装配用途编号";
        }

        void txtEdition_OnCompleteSearch()
        {
            m_edition = txtEdition.DataResult["零部件编码"].ToString();

            IQueryable<View_ZPX_ReparativeBarcode> result = m_reparativePartInfoServer.GetData(m_edition);

            if (result.Count() == 0)
            {
                dataGridView1.Rows.Clear();
                MessageDialog.ShowPromptMessage("没有找到指定数据");
            }
            else
            {
                dataGridView1.Rows.Clear();

                int index = 0;

                foreach (var item in result)
                {
                    dataGridView1.Rows.Add(new object[] {
                        item.序号, item.产品编码, item.零部件编码, item.零部件名称, item.规格, item.数量, item.供货单位, item.位置 });

                    StateData<View_ZPX_ReparativeBarcode> part = new StateData<View_ZPX_ReparativeBarcode>(item);

                    dataGridView1.Rows[index++].Tag = part;
                }

                this.dataGridView1.ColumnWidthChanged -= new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);

                ColumnWidthControl.SetDataGridView(labelTitle.Text, dataGridView1);

                this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(
                    this.dataGridView1_ColumnWidthChanged);

                m_dataLocalizer.Init(dataGridView1, this.Name, 
                    UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
            }
        }

        void txtGoodsName_OnCompleteSearch()
        {
            txtGoodsName.Text = txtGoodsName.DataResult["物品名称"].ToString();
            txtGoodsCode.Text = txtGoodsName.DataResult["图号型号"].ToString();
            txtSpec.Text = txtGoodsName.DataResult["规格"].ToString();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0)
            {
                return;
            }

            if (MessageDialog.ShowEnquiryMessage("您确定选择的用途是【" + cmbPurpose.Text + "】吗？") == DialogResult.No)
            {
                MessageDialog.ShowPromptMessage("打印条形码前请选择好在多批次管理中的具体用途后再进行此操作");
                cmbPurpose.Focus();
                return;
            }

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                if ((int)dataGridView1.Rows[i].Cells["数量"].Value == 0)
                {
                    MessageDialog.ShowErrorMessage(string.Format("第 {0} 行零件数量=0, 请设置好数量后再进行此操作",
                        dataGridView1.Rows[i].Index + 1));

                    return;
                }
            }

            blPrintFlag = true;
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (txtEdition.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("父总成编码不能为空");
                return;
            }

            if (txtGoodsCode.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("零部件编码不能为空");
                return;
            }

            View_ZPX_ReparativeBarcode item = new View_ZPX_ReparativeBarcode();

            item.序号 = 0;
            item.产品编码 = m_edition;
            item.零部件编码 = txtGoodsCode.Text;
            item.零部件名称 = txtGoodsName.Text;
            item.规格 = txtSpec.Text;
            item.数量 = 1;
            item.供货单位 = "";
            item.位置 = 1;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                StateData<View_ZPX_ReparativeBarcode> rowData = (StateData<View_ZPX_ReparativeBarcode>)dataGridView1.Rows[i].Tag;

                if (item.零部件编码 == rowData.Data.零部件编码 && item.规格 == rowData.Data.规格)
                {
                    MessageDialog.ShowErrorMessage(string.Format("{0},{1},规格【{2}】的零件已经存在不允许重复添加",
                        item.零部件编码, item.零部件名称, item.规格));

                    return;
                }
            }

            if (dataGridView1.Rows.Count > 0)
            {
                item.位置 += Convert.ToInt32(dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["位置"].Value);
            }

            dataGridView1.Rows.Add(new object[] {
                        item.序号, item.产品编码, item.零部件编码, item.零部件名称, item.规格, item.数量, item.供货单位, item.位置 });

            StateData<View_ZPX_ReparativeBarcode> data = new StateData<View_ZPX_ReparativeBarcode>(item);
            data.DataStatus = StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Add;

            dataGridView1.Rows[dataGridView1.Rows.Count - 1].Tag = data;

            m_lstData.Add(data);
        }

        /// <summary>
        /// 点击供货单位单元格更新供货单位数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
            {
                return;
            }

            DataGridViewColumnCollection columns = this.dataGridView1.Columns;

            if (columns[e.ColumnIndex].Name == "供货单位")
            {
                FormQueryInfo form = QueryInfoDialog.GetProviderFromGoods(
                    dataGridView1.Rows[e.RowIndex].Cells["零部件编码"].Value.ToString(),
                    dataGridView1.Rows[e.RowIndex].Cells["规格"].Value.ToString());

                if (DialogResult.OK == form.ShowDialog() &&
                    MessageDialog.ShowEnquiryMessage("是否保存该供应商信息？") == DialogResult.Yes)
                {
                    string provider = form["供货单位"];

                    dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = provider;

                    StateData<View_ZPX_ReparativeBarcode> data = dataGridView1.Rows[e.RowIndex].Tag as StateData<View_ZPX_ReparativeBarcode>;
                    data.Data.供货单位 = provider;

                    int index = GetDataIndex(data);

                    if (index == -1)
                    {
                        data.DataStatus = StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Update;
                        m_lstData.Add(data);
                    }
                }
            }
        }

        /// <summary>
        /// 获取数据在变更列表中的索引
        /// </summary>
        /// <param name="data">查询的数据</param>
        /// <returns>失败返回-1, 成功返回获取到的索引</returns>
        private int GetDataIndex(StateData<View_ZPX_ReparativeBarcode> data)
        {
            int index = m_lstData.FindIndex(r =>
                r.Data.零部件编码 == data.Data.零部件编码 &&
                r.Data.规格 == data.Data.规格);

            return index;
        }

        private void dataGridView1_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
        {
            ColumnWidthControl.SaveColumnParams(labelTitle.Text, e.Column);
        }

        private void 打印整台份返修箱条码_Resize(object sender, EventArgs e)
        {
            labelTitle.Location = new Point((this.Width - labelTitle.Width) / 2, labelTitle.Location.Y);
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

        /// <summary>
        /// 检查是否正确选择操作的记录行
        /// </summary>
        /// <returns>正确返回true</returns>
        bool CheckSelectedRow()
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageDialog.ShowPromptMessage("请选择记录后再进行此操作");
                return false;
            }

            return true;
        }

        private void 只打印选择行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectRecords(true);
            btnPrint.PerformClick();
        }

        /// <summary>
        /// 选择记录
        /// </summary>
        /// <param name="onlySelectedRows">
        /// 是否仅当前界面上被选中的记录行
        /// true ：仅保留当前界面上被选中的记录行
        /// false：删除当前界面上被选中的记录行
        /// </param>
        private void SelectRecords(bool onlySelectedRows)
        {
            if (!CheckSelectedRow())
                return;

            Dictionary<long, int> dicID = new Dictionary<long, int>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dicID.Add((long)dataGridView1.SelectedRows[i].Cells[0].Value, 0);
            }

            if (onlySelectedRows)
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (!dicID.ContainsKey(Convert.ToInt64(dataGridView1.Rows[i].Cells[0].Value)))
                    {
                        dataGridView1.Rows.RemoveAt(i--);
                    }
                }
            }
            else
            {
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    if (dicID.ContainsKey(Convert.ToInt64(dataGridView1.Rows[i].Cells[0].Value)))
                    {
                        dataGridView1.Rows.RemoveAt(i--);
                    }
                }
            }
        }

        private void 删除选择行记录仅本次打印有效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SelectRecords(false);
        }

        private void 移除所有选择记录供应商永久有效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].Cells["供货单位"].Value = "";

                StateData<View_ZPX_ReparativeBarcode> data = dataGridView1.SelectedRows[i].Tag as StateData<View_ZPX_ReparativeBarcode>;
                data.Data.供货单位 = "";
                data.DataStatus = StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Update;
            }
        }

        private void 移除选择记录供应商_本次有效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            Dictionary<long, int> dicID = new Dictionary<long, int>();

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                dataGridView1.SelectedRows[i].Cells["供货单位"].Value = "";
            }
        }

        private void txtEdition_TextChanged(object sender, EventArgs e)
        {
            if (txtEdition.Text != "")
            {
                txtGoodsName.ReadOnly = false;
            }
            else
            {
                txtGoodsName.ReadOnly = true;
            }
        }

        private void 删除选择行记录永远有效ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            int[] arrayIndex = new int[dataGridView1.SelectedRows.Count];

            for (int i = 0; i < dataGridView1.SelectedRows.Count; i++)
            {
                arrayIndex[i] = dataGridView1.SelectedRows[i].Index;
            }

            for (int i = 0; i < arrayIndex.Length; i++)
            {
                StateData<View_ZPX_ReparativeBarcode> data = 
                    dataGridView1.Rows[arrayIndex[i]].Tag as StateData<View_ZPX_ReparativeBarcode>;

                data.DataStatus = StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Delete;

                int index = GetDataIndex(data);

                if (index != -1)
                {
                    if (m_lstData[index].DataStatus == StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Add)
                        m_lstData.RemoveAt(index);
                }
                else
                {
                    m_lstData.Add(data);
                }

                dataGridView1.Rows.Remove(dataGridView1.Rows[arrayIndex[i]]);
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].ValueType == typeof(int))
            {
                e.Cancel = true;
            }
        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && dataGridView1.Columns[e.ColumnIndex].Name == "数量")
            {
                StateData<View_ZPX_ReparativeBarcode> data = (StateData<View_ZPX_ReparativeBarcode>)dataGridView1.Rows[e.RowIndex].Tag;
                data.Data.数量 = (int)dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                int index = GetDataIndex(data);

                if (index == -1)
                {
                    data.DataStatus = StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Update;
                    m_lstData.Add(data);
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (m_lstData.Count > 0)
            {
                string error;

                if (!m_reparativePartInfoServer.Update(m_lstData, out error))
                {
                    MessageDialog.ShowErrorMessage(error);
                }
                else
                {
                    m_lstData.Clear();
                    txtEdition_OnCompleteSearch();
                }
            }
        }

        /// <summary>
        /// 设置指定行数据
        /// </summary>
        /// <param name="rowIndex">指定行索引</param>
        /// <param name="rowData">数据</param>
        private void SetRowData(int rowIndex, StateData<View_ZPX_ReparativeBarcode> rowData)
        {
            dataGridView1.Rows[rowIndex].Cells["序号"].Value = rowData.Data.序号;
            dataGridView1.Rows[rowIndex].Cells["产品编码"].Value = rowData.Data.产品编码;
            dataGridView1.Rows[rowIndex].Cells["零部件编码"].Value = rowData.Data.零部件编码;
            dataGridView1.Rows[rowIndex].Cells["零部件名称"].Value = rowData.Data.零部件名称;
            dataGridView1.Rows[rowIndex].Cells["规格"].Value = rowData.Data.规格;
            dataGridView1.Rows[rowIndex].Cells["数量"].Value = rowData.Data.数量;
            dataGridView1.Rows[rowIndex].Cells["供货单位"].Value = rowData.Data.供货单位;
            dataGridView1.Rows[rowIndex].Cells["位置"].Value = rowData.Data.位置;

            dataGridView1.Rows[rowIndex].Tag = rowData;
        }

        /// <summary>
        /// 移动行的位置
        /// </summary>
        /// <param name="surRowIndex">源行索引</param>
        /// <param name="desRowIndex">目标行索引</param>
        private void MoveRow(int surRowIndex, int desRowIndex)
        {
            if (dataGridView1.Rows.Count <= desRowIndex || desRowIndex < 0)
            {
                MessageDialog.ShowErrorMessage("目标行索引不存在，无法移动到指定行, 出错索引号：" + desRowIndex.ToString());
                return;
            }
            else if (surRowIndex == desRowIndex)
            {
                MessageDialog.ShowErrorMessage(
                    "目标行索引不能和源行索引相同，出错索引号：" + surRowIndex.ToString());

                return;
            }

            StateData<View_ZPX_ReparativeBarcode> curData = (StateData<View_ZPX_ReparativeBarcode>)dataGridView1.Rows[surRowIndex].Tag;
            StateData<View_ZPX_ReparativeBarcode> desRowData = (StateData<View_ZPX_ReparativeBarcode>)dataGridView1.Rows[desRowIndex].Tag;

            int indexOffset = desRowIndex < surRowIndex ? 1 : 0;

            if (desRowIndex < surRowIndex)
            {
                dataGridView1.Rows.InsertCopy(surRowIndex, desRowIndex);
                SetRowData(desRowIndex, curData);
            }
            else
            {
                dataGridView1.Rows.InsertCopy(surRowIndex, desRowIndex + 1);
                SetRowData(desRowIndex + 1, curData);
            }

            dataGridView1.Rows.RemoveAt(surRowIndex + indexOffset);

            dataGridView1.ClearSelection();
            dataGridView1.Rows[desRowIndex].Selected = true;

            for (int i = 0; i < dataGridView1.Rows.Count; i++)
            {
                StateData<View_ZPX_ReparativeBarcode> rowData = (StateData<View_ZPX_ReparativeBarcode>)dataGridView1.Rows[i].Tag;

                rowData.Data.位置 = i;

                dataGridView1.Rows[i].Cells["位置"].Value = i;

                if (GetDataIndex(rowData) == -1)
                {
                    rowData.DataStatus = StateData<View_ZPX_ReparativeBarcode>.DataStatusEnum.Update;
                    m_lstData.Add(rowData);
                }
            }
        }

        /// <summary>
        /// 将当前选中行数据批量移动到指定行位置
        /// </summary>
        /// <param name="desPosition">目标位置</param>
        private void BatchMoveRow(int desPosition)
        {
            if (!CheckSelectedRow())
                return;

            if (desPosition == 0 && IsFirstRow())
                return;
            else if (desPosition == dataGridView1.Rows.Count - 1 && IsLastRow())
                return;

            List<int> lstIndex = GetSortedIndexOfSelectedRows();

            if (desPosition == lstIndex[0])
                return;

            if (lstIndex[0] > desPosition)
            {
                foreach (int element in lstIndex)
                {
                    MoveRow(element, desPosition++);
                }
            }
            else if (desPosition == dataGridView1.Rows.Count - 1)
            {
                for (int i = 0; i < lstIndex.Count; i++)
                {
                    MoveRow(lstIndex[0], desPosition);
                }
            }
            else
            {
                for (int i = 0; i < lstIndex.Count; i++)
                {
                    MoveRow(lstIndex[lstIndex.Count - 1 - i], desPosition + lstIndex.Count - 1 - i);
                }
            }
        }

        /// <summary>
        /// 判断是否第一行
        /// </summary>
        /// <returns>是则返回true</returns>
        private bool IsFirstRow()
        {
            return GetMinIndex(dataGridView1.SelectedRows) == 0;
        }

        /// <summary>
        /// 判断是否最后一行
        /// </summary>
        /// <returns>是则返回true</returns>
        private bool IsLastRow()
        {
            return (GetMaxIndex(dataGridView1.SelectedRows) == dataGridView1.Rows.Count - 1);
        }

        /// <summary>
        /// 以从小到大排序的方式获取选择行索引
        /// </summary>
        /// <returns>返回获取到的索引</returns>
        private List<int> GetSortedIndexOfSelectedRows()
        {
            System.Diagnostics.Debug.Assert(
                dataGridView1.SelectedRows.Count != 0,
                "没有选择行，不允许进行此操作");

            SortedList<int, int> sortedlstIndex = new SortedList<int, int>();
            List<int> lstIndex = new List<int>();

            foreach(DataGridViewRow row in dataGridView1.SelectedRows)
            {
                sortedlstIndex.Add(row.Index, 0);
            }

            foreach (KeyValuePair<int,int> item in sortedlstIndex)
            {
                lstIndex.Add(item.Key);
            }

            return lstIndex;
        }

        /// <summary>
        /// 获取最小索引号
        /// </summary>
        /// <param name="rows">行集</param>
        /// <returns>返回获取到的最小索引值</returns>
        private int GetMinIndex(DataGridViewSelectedRowCollection rows)
        {
            if (rows == null || rows.Count == 0)
            {
                return 0;
            }

            return rows[0].Index < rows[rows.Count - 1].Index ?
                rows[0].Index : rows[rows.Count - 1].Index;
        }

        /// <summary>
        /// 获取最大索引号
        /// </summary>
        /// <param name="rows">行集</param>
        /// <returns>返回获取到的最大索引值</returns>
        private int GetMaxIndex(DataGridViewSelectedRowCollection rows)
        {
            if (rows == null || rows.Count == 0)
            {
                return 0;
            }

            return rows[0].Index > rows[rows.Count - 1].Index ?
                rows[0].Index : rows[rows.Count - 1].Index;
        }

        private void 向上移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (IsFirstRow())
                return;

            int minIndex = GetMinIndex(dataGridView1.SelectedRows);

            int maxIndex = GetMaxIndex(dataGridView1.SelectedRows);
            int count = dataGridView1.SelectedRows.Count;

            MoveRow(minIndex - 1, maxIndex);

            dataGridView1.ClearSelection();

            for (int i = 0; i < count; i++)
            {
                dataGridView1.Rows[minIndex - 1 + i].Selected = true;
            }
        }

        private void 向下移动ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (IsLastRow())
                return;

            int maxIndex = GetMaxIndex(dataGridView1.SelectedRows);

            int minIndex = GetMinIndex(dataGridView1.SelectedRows);
            int count = dataGridView1.SelectedRows.Count;

            MoveRow(maxIndex + 1, minIndex);
            
            dataGridView1.ClearSelection();

            for (int i = 0; i < count; i++)
            {
                dataGridView1.Rows[minIndex + 1 + i].Selected = true;
            }
        }

        private void 移动到第一个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (IsFirstRow())
                return;

            int minIndex = GetMinIndex(dataGridView1.SelectedRows);
            int count = dataGridView1.SelectedRows.Count;

            BatchMoveRow(0);

            dataGridView1.ClearSelection();

            for (int i = 0; i < count; i++)
            {
                dataGridView1.Rows[i].Selected = true;
            }
        }

        private void 移动到最后ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!CheckSelectedRow())
                return;

            if (IsLastRow())
                return;

            int maxIndex = GetMaxIndex(dataGridView1.SelectedRows);

            int count = dataGridView1.SelectedRows.Count;

            BatchMoveRow(dataGridView1.Rows.Count - 1);

            dataGridView1.ClearSelection();

            for (int i = 0; i < count; i++)
            {
                dataGridView1.Rows[dataGridView1.Rows.Count - 1 - i].Selected = true;
            }
        }

        private void 移动到指定行ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int pos = 0;

            try
            {
                pos = Convert.ToInt32(InputBox.ShowDialog("数据输入", "指定行号：", "0")) - 1;
            }
            catch (Exception exce)
            {
                MessageDialog.ShowErrorMessage(exce.Message);
                return;
            }

            int minIndex = GetMinIndex(dataGridView1.SelectedRows);
            int maxIndex = GetMaxIndex(dataGridView1.SelectedRows);
            int count = dataGridView1.SelectedRows.Count;

            pos = pos == -1 ? 0 : pos;

            if (pos + count >= dataGridView1.Rows.Count)
            {
                pos = dataGridView1.Rows.Count - count - 1;
            }

            if (pos == 0 && minIndex == 0)
            {
                return;
            }
            else if (pos + count >= dataGridView1.Rows.Count)
            {
                return;
            }

            if (pos == 0)
            {
                移动到第一个ToolStripMenuItem_Click(sender, e);
                return;
            }
            else if (pos + count == dataGridView1.Rows.Count - 1)
            {
                移动到最后ToolStripMenuItem_Click(sender, e);
                return;
            }
            else
            {
                BatchMoveRow(pos);
            }

            dataGridView1.ClearSelection();

            for (int i = 0; i < count; i++)
            {
                dataGridView1.Rows[pos + i].Selected = true;
            }
        }
    }
}
