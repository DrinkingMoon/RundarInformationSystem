using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Reflection;

namespace UniversalControlLibrary
{
    public partial class CustomDataGridView : DataGridView
    {
        System.ComponentModel.ComponentResourceManager resources =
            new System.ComponentModel.ComponentResourceManager(typeof(UniversalControlLibrary.Properties.Resources));

        #region PRIVATE FIELDS

        private DgvBaseFilterHost mFilterHost;                          // The host UserControl to popup
        private DataView _DataSourceView = new DataView();              // The DataView to which the DataGridView is bound
        private BindingSource _DataSourceBinding = new BindingSource(); // The BindingSource, if any, to which the DataGridView is bound

        private string mBaseFilter = "";            // Developer provided filter expression
        private int mCurrentColumnIndex = -1;       // Column Index of currently visibile filter

        private List<DgvBaseColumnFilter> mColumnFilterList;    // List of ColumnFilter objects
        private bool mAutoCreateFilters = true;
        private bool mFilterIsActive = false;
        private static Image mFilterPicture; //The funnel picture
        SolidBrush solidBrush;
        List<DataGridViewColumn> mDesignColumnsList = new List<DataGridViewColumn>();

        #endregion


        #region EVENTS

        /// <summary>
        /// Occurs when a <i>column filter</i> instance for a column is about to be automatically created.
        /// </summary>
        /// <remarks>
        /// Using this event you can set the <see cref="ColumnFilterEventArgs.ColumnFilter"/> 
        /// property to force the <see cref="DgvBaseColumnFilter"/> specialization to use for the 
        /// column. 
        /// This event is raised only if <see cref="DgvFilterManager.AutoCreateFilters"/> is true.
        /// </remarks>
        public event ColumnFilterEventHandler ColumnFilterAdding;

        /// <summary>
        /// Occurs when a <i>column filter</i> instance is created.
        /// This event is raised only if <see cref="DgvFilterManager.AutoCreateFilters"/> is true.
        /// </summary>
        public event ColumnFilterEventHandler ColumnFilterAdded;

        #endregion


        public CustomDataGridView()
        {
            InitializeComponent();
            solidBrush = new SolidBrush(System.Drawing.Color.Red);
            mColumnFilterList = new List<DgvBaseColumnFilter>(this.Columns.Count);

            this._DataSourceBinding.DataSourceChanged += new EventHandler(_DataSourceBinding_DataSourceChanged);
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        }


        #region PROPERTIES

        public BindingSource DataSourceBinding
        {
            get { return _DataSourceBinding; }
            set { _DataSourceBinding = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the manager must create <i>column filters</i>.
        /// </summary>
        /// <value><c>true</c> by default.</value>
        public bool AutoCreateFilters
        {
            get { return mAutoCreateFilters; }
            set { mAutoCreateFilters = value; }
        }

        /// <summary>
        /// Gets a funnel picture.
        /// </summary>
        public static Image FunnelPicture
        {
            get
            {
                if (mFilterPicture == null)
                {
                    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DgvFilterHost));
                    mFilterPicture = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
                }
                return mFilterPicture;
            }
        }

        /// <summary>
        /// Gets and sets the <i>filter host</i> to use. 
        /// </summary>
        /// <remarks>
        /// The default <i>filter host</i> is an instance of <see cref="DgvFilterHost"/>
        /// </remarks>
        public DgvBaseFilterHost FilterHost
        {
            get
            {
                if (mFilterHost == null)
                {
                    // If not provided, use the default FilterHost
                    FilterHost = new DgvFilterHost();

                }
                return mFilterHost;
            }
            set
            {
                mFilterHost = value;
                // initialize FilterManager to this object
                mFilterHost.DataGridView = this;
                mFilterHost.Popup.Closed += new ToolStripDropDownClosedEventHandler(Popup_Closed);
                mFilterHost.ComboBoxColumns.SelectedValueChanged += new EventHandler(ComboBoxColumns_SelectedValueChanged);
            }
        }

        /// <summary>
        /// Gets and sets developer provided filter expression. This expression
        /// will be "merged" with end-user created filters.
        /// </summary>
        /// <value>The base filter.</value>
        public string BaseFilter
        {
            get { return mBaseFilter; }
            set { mBaseFilter = value; RebuildFilter(); }
        }

        /// <summary>
        /// Gets or sets the <i>column filter</i> control related to the ColumnIndex
        /// </summary>
        /// <param name="ColumnIndex">The index of the <b>DataGridView</b> column</param>
        /// <returns>the <b>DgvBaseColumnFilter</b> related to the <b>DataGridView</b> column</returns>
        /// <remarks>
        /// This indexer allow you to get and set the <i>column filter</i> instance for the column. 
        /// You can set one of the standard <i>column filter</i> implementation or an instance 
        /// of your own <b>DgvBaseFilterColumn</b> specialization.
        /// </remarks>
        public DgvBaseColumnFilter this[int ColumnIndex]
        {
            get { return mColumnFilterList[ColumnIndex]; }
            set
            {
                mColumnFilterList[ColumnIndex] = value;
                value.Init(this, _DataSourceView, ColumnIndex);
            }
        }

        /// <summary>
        /// Gets or sets the <i>column filter</i> control related to the ColumnName
        /// </summary>
        /// <param name="ColumnName">The name of the <b>DataGridView</b> column</param>
        /// <returns>the DgvBaseColumnFilter related to the <b>DataGridView</b> column</returns>
        /// <remarks>
        /// This indexer allow you to get and set the <i>column filter</i> instance for the column. 
        /// You can set one of the standard <i>column filter</i> implementation or an instance 
        /// of your own <b>DgvBaseFilterColumn</b> specialization.
        /// </remarks>
        public DgvBaseColumnFilter this[string ColumnName]
        {
            get { return mColumnFilterList[this.Columns[ColumnName].Index]; }
            set
            {
                this[this.Columns[ColumnName].Index] = value;
            }
        }

        #endregion


        #region DATAGRIDVIEW EVENT HANDLERS

        void _DataSourceBinding_DataSourceChanged(object sender, EventArgs e)
        {
            _DataSourceView = TransDataSource(_DataSourceBinding.DataSource);
            this.DataSource = _DataSourceView;
        }

        protected override void OnRowPostPaint(DataGridViewRowPostPaintEventArgs e)
        {
            Rectangle rectangle = new Rectangle(e.RowBounds.Location.X,
                e.RowBounds.Location.Y,
                this.RowHeadersWidth - 4,
                e.RowBounds.Height);

            TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(),
                this.RowHeadersDefaultCellStyle.Font,
                rectangle,
                this.RowHeadersDefaultCellStyle.ForeColor,
                TextFormatFlags.VerticalCenter | TextFormatFlags.Right);
        }

        protected override void OnCellBeginEdit(DataGridViewCellCancelEventArgs e)
        {
            Type type = this.Rows[e.RowIndex].Cells[e.ColumnIndex].GetType();
        }

        protected override void OnColumnRemoved(DataGridViewColumnEventArgs e)
        {
            try
            {
                if (mColumnFilterList == null)
                {
                    return;
                }

                mColumnFilterList.RemoveAll(k => k == null);
                DgvBaseColumnFilter dgvBaseCol = mColumnFilterList.Find(k => k.DataGridViewColumn == e.Column);

                if (dgvBaseCol != null)
                {
                    mColumnFilterList.Remove(dgvBaseCol);
                }
            }
            catch (Exception)
            {

            }
        }

        protected override void OnColumnAdded(DataGridViewColumnEventArgs e)
        {
            try
            {
                if (_DataSourceView.Table == null)
                {
                    return;
                }

                CreateColumnFilter(e.Column);
            }
            catch (Exception)
            {

            }
        }

        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex != -1) return; //skip if it is not the header row

                //Cell Origin
                if (e.RowIndex == -1 && e.ColumnIndex == -1 && mFilterIsActive)
                {
                    OnFilteredGridPaint(this, e);
                    return;
                }

                if (FilterHost.Popup.Visible)
                {
                    OnHighlightedColumnPaint(this, e);
                }

                if (e.ColumnIndex == -1) return;
                if (mColumnFilterList.Count > 0
                    && mColumnFilterList[e.ColumnIndex] != null
                    && mColumnFilterList[e.ColumnIndex].Active)
                {
                    OnFilteredColumnPaint(this, e);
                }
            }
            catch (Exception)
            {

            }
        }

        public void InitDataSourceBinding()
        {
            try
            {

                if (_DataSourceBinding == null)
                {
                    _DataSourceBinding = new BindingSource();
                }

                if (_DataSourceBinding.DataSource == null)
                {
                    DataTable tempTable = new DataTable();

                    if (mColumnFilterList.Count == 0)
                    {
                        foreach (DataGridViewColumn dgvc in this.Columns)
                        {
                            CreateColumnFilter(dgvc);
                        }
                    }

                    foreach (DgvBaseColumnFilter dbcf in mColumnFilterList)
                    {
                        tempTable.Columns.Add(GlobalObject.GeneralFunction.IsNullOrEmpty(dbcf.DataGridViewColumn.DataPropertyName) ?
                            dbcf.DataGridViewColumn.Name : dbcf.DataGridViewColumn.DataPropertyName);
                    }

                    foreach (DataGridViewRow dgvr in this.Rows)
                    {
                        DataRow dr = tempTable.NewRow();

                        foreach (DataGridViewColumn dgvc in this.Columns)
                        {
                            if (GlobalObject.GeneralFunction.IsNullOrEmpty(dgvc.DataPropertyName))
                            {
                                dr[dgvc.Name] = dgvr.Cells[dgvc.Index].Value;
                            }
                            else
                            {
                                dr[dgvc.DataPropertyName] = dgvr.Cells[dgvc.Index].Value;
                            }
                        }

                        tempTable.Rows.Add(dr);
                    }

                    _DataSourceBinding.DataSource = tempTable;
                }
            }
            catch (Exception)
            {

            }
        }

        public DataTable GetDataSourceToDataTable()
        {
            if (DataSource == null)
            {
                InitDataSourceBinding();
                return _DataSourceView.Table;
            }
            else
            {
                DataView dv = TransDataSource(this.DataSource);
                return dv.Table;
            }
        }

        //菜单单击排序事件
        protected virtual void mMenuStripSort_Click(object sender, EventArgs e)
        {
            var oToolScript = (((MyToolStripMenuItem)sender));
            this.Sort(this.Columns[oToolScript.DgvBaseColumn.DataGridViewColumn.Name],
                oToolScript.SortDirection);
        }

        //菜单单击删除全部过滤条件事件
        protected virtual void mMenuStripFilterDelete_Click(object sender, EventArgs e)
        {
            ActivateAllFilters(false);

            if (!mFilterHost.IsDisposed)
            {
                this.mFilterHost.Popup.Close();
            }
        }

        //菜单单击过滤事件
        protected virtual void mMenuStripFilter_Click(object sender, EventArgs e)
        {
            var oToolScript = (((MyToolStripMenuItem)sender));

            if (mColumnFilterList[oToolScript.DgvBaseColumn.DataGridViewColumn.Index] == null) return; // non-data column

            SetCurrentFilterIndex(oToolScript.DgvBaseColumn.DataGridViewColumn.Index);
            FilterHost.Popup.Show(Control.MousePosition); // show the filterhost popup near the column
            FilterHost.Popup.Focus();

            FilterHost.ComboBoxColumns.SelectedIndex = oToolScript.DgvBaseColumn.DataGridViewColumn.Index;

            //显示过滤表达式
            SetFilterExpression();
        }

        //删除过滤条件图标的点击事件
        private void oPicBox_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //1.根据过来字段找到过滤字段索引
                var oPicBox = (PictureBox)sender;
                var iFilterIndex = GetFilterIndexByFilterName(oPicBox.Tag.ToString());
                if (iFilterIndex == -1) return;
                //2.设置当前过滤索引
                SetCurrentFilterIndex(iFilterIndex);

                //3.将当前过滤列的Active设为false
                ActivateFilter(false);

                //4.设置过滤表达式
                SetFilterExpression();
            }
        }

        //选择列的选择事件
        protected virtual void ComboBoxColumns_SelectedValueChanged(object sender, EventArgs e)
        {
            ComboBox dgvFilterHostCom = sender as ComboBox;

            if (dgvFilterHostCom.DataSource == null)
            {
                return;
            }

            DgvBaseColumnFilter baseColumn = new DgvBaseColumnFilter();

            if (dgvFilterHostCom.SelectedValue is KeyValuePair<DgvBaseColumnFilter, string>)
            {
                baseColumn = ((KeyValuePair<DgvBaseColumnFilter, string>)dgvFilterHostCom.SelectedValue).Key;
            }
            else if (dgvFilterHostCom.SelectedValue is DgvBaseColumnFilter)
            {
                baseColumn = dgvFilterHostCom.SelectedValue as DgvBaseColumnFilter;
            }

            var oSelectedValue = baseColumn.DataGridViewColumn.DataPropertyName;
            var iFilterIndex = GetFilterIndexByFilterName(oSelectedValue);
            if (iFilterIndex == -1) return;
            SetCurrentFilterIndex(iFilterIndex);
        }

        //根据过滤列的名称得到过滤列的列索引
        private int GetFilterIndexByFilterName(string strFilterName)
        {
            int iRes = -1;
            var lstCols = this.Columns;
            foreach (DataGridViewColumn oCol in lstCols)
            {
                if (oCol.DataPropertyName == strFilterName)
                {
                    if (mColumnFilterList[oCol.Index] == null) return -1; // non-data column
                    iRes = oCol.Index;
                    break;
                }
            }

            return iRes;
        }

        //设置当前过滤索引
        private void SetCurrentFilterIndex(int iIndex)
        {
            int OldColumnIndex = mCurrentColumnIndex;
            mCurrentColumnIndex = iIndex;
            FilterHost.CurrentColumnFilter = mColumnFilterList[iIndex];
            try
            {
                //use "try" because old column could have been removed
                this.InvalidateCell(OldColumnIndex, -1);
            }
            catch { }
        }

        //显示过滤表达式
        public void SetFilterExpression()
        {
            string strRowFilterText = GlobalObject.GeneralFunction.IsNullOrEmpty(_DataSourceView.RowFilter) ? "" :
                    _DataSourceView.RowFilter.Contains("1=1  AND") ?
                    _DataSourceView.RowFilter.Replace("1=1  AND", string.Empty) : _DataSourceView.RowFilter;

            var arrFilterText = strRowFilterText.Split(new string[] { " AND " }, StringSplitOptions.RemoveEmptyEntries);
            var y = 0;
            //0.先清空表达式再重新绘制控件
            FilterHost.PanelFilterText.Controls.Clear();
            foreach (var strText in arrFilterText)
            {
                //1.新增panel
                var oPanelText = new Panel();
                oPanelText.Name = "oPanelText" + y;
                oPanelText.Location = new System.Drawing.Point(0, y);
                oPanelText.Size = new System.Drawing.Size(450, 25);

                //2.向panel里面新增显示表达式的label
                var oLabelText = new Label();
                oLabelText.Cursor = Cursors.Hand;
                oLabelText.ForeColor = Color.BlueViolet;
                oLabelText.AutoSize = true;
                oLabelText.Location = new System.Drawing.Point(10, 0);
                oLabelText.Name = "label_filtertext" + y;
                oLabelText.Text = strText.Trim().Trim(new char[2] { '(', ')' });
                oLabelText.Size = new System.Drawing.Size(oLabelText.Text.Length * 8, 25);

                //3.向panel里面新增删除图标
                var oPicBox = new PictureBox();
                oPicBox.Image = ((System.Drawing.Image)(resources.GetObject("Delete_16x161")));
                oPicBox.Cursor = Cursors.Hand;
                oPicBox.Location = new Point(oLabelText.Width + 10, 0);
                oPicBox.Size = new System.Drawing.Size(20, 20);
                var otooltip = new System.Windows.Forms.ToolTip();
                otooltip.SetToolTip(oPicBox, "删除过滤条件");
                oPicBox.MouseClick += oPicBox_MouseClick;
                oPicBox.Tag = oLabelText.Text.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                oPanelText.Controls.Add(oPicBox);
                oPanelText.Controls.Add(oLabelText);
                y = y + 30;
                FilterHost.PanelFilterText.Controls.Add(oPanelText);
            }
        }

        /// <summary>
        /// Paints a funnel icon in the cell origin when some column is filtered.
        /// </summary>
        /// <param name="sender">The sender</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellPaintingEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Override this method to provide your own painting
        /// </remarks>
        protected virtual void OnFilteredGridPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
            e.Paint(e.CellBounds, e.PaintParts & ~DataGridViewPaintParts.Background);
            Rectangle r = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 3, e.CellBounds.Height - 4);
            e.Graphics.DrawImage(FunnelPicture, (e.CellBounds.Width - FunnelPicture.Width) / 2, (e.CellBounds.Height - FunnelPicture.Height) / 2, FunnelPicture.Width, FunnelPicture.Height);
            e.Graphics.DrawRectangle(Pens.Black, r);

            e.Handled = true;
        }

        /// <summary>
        /// Performs customized column header painting when the popup is visibile. 
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellPaintingEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Override this method to provide your own painting
        /// </remarks>
        protected virtual void OnHighlightedColumnPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex != mCurrentColumnIndex || e.RowIndex != -1) return;
            e.Paint(e.CellBounds, DataGridViewPaintParts.All);
            Rectangle r = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 3, e.CellBounds.Height - 4);
            e.Graphics.DrawRectangle(Pens.Yellow, r);
            e.Handled = true;
        }

        /// <summary>
        /// Performs customized column header painting when the column is filtered.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="System.Windows.Forms.DataGridViewCellPaintingEventArgs"/> instance containing the event data.</param>
        /// <remarks>
        /// Override this method to provide your own painting
        /// </remarks>
        protected virtual void OnFilteredColumnPaint(object sender, DataGridViewCellPaintingEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.White, e.CellBounds);
            e.Paint(e.CellBounds, e.PaintParts & ~DataGridViewPaintParts.Background);
            Rectangle r = new Rectangle(e.CellBounds.X + 1, e.CellBounds.Y + 1, e.CellBounds.Width - 3, e.CellBounds.Height - 4);
            e.Graphics.DrawRectangle(Pens.Black, r);
            e.Handled = true;
        }

        #endregion


        #region FILTERHOST MANAGING

        //Forces column header repaint when popup is closed, cleaning customized painting performed by OnHighlightedColumnPaint
        private void Popup_Closed(object sender, ToolStripDropDownClosedEventArgs e)
        {
            this.InvalidateCell(mCurrentColumnIndex, -1);   // Force header repaint (to hide the selection yellow frame)
        }

        #endregion


        #region COLUMN FILTERS MANAGING

        /// <summary>
        /// Activates / Deactivates the filter for the column specified by ColumnIndex.
        /// </summary>
        /// <param name="Active">The active state to set</param>
        /// <param name="ColumnIndex">Index of the column.</param>
        public void ActivateFilter(bool Active, int ColumnIndex)
        {
            this[ColumnIndex].Active = Active;
            RebuildFilter();
        }

        /// <summary>
        /// Activates / Deactivates the filter for the column specified by ColumnName.
        /// </summary>
        /// <param name="Active">The active state to set</param>
        /// <param name="ColumnName">Name of the column.</param>
        public void ActivateFilter(bool Active, string ColumnName)
        {
            this[ColumnName].Active = Active;
            RebuildFilter();
        }

        /// <summary>
        /// Activates / Deactivates the filter for the current, that is last right-clicked, column.
        /// </summary>
        /// <param name="Active">The active state to set</param>
        public void ActivateFilter(bool Active)
        {
            if (mCurrentColumnIndex == -1) return;
            this[mCurrentColumnIndex].Active = Active;
            if (Active) this[mCurrentColumnIndex].FilterExpressionBuild();
            RebuildFilter();
        }

        /// <summary>
        /// Activates / Deactivates all filters.
        /// </summary>
        /// <param name="Active">The active state to set</param>
        public void ActivateAllFilters(bool Active)
        {
            foreach (DgvBaseColumnFilter CF in mColumnFilterList)
            {
                if (CF == null) continue;
                CF.Active = Active;
                if (Active) CF.FilterExpressionBuild();
            }
            RebuildFilter();
        }

        /// <summary>
        /// Rebuilds the whole filter expression.
        /// </summary>
        /// <remarks>
        /// The whole filter expression is the conjunction of each <i>column filter</i> and the <see cref="BaseFilter"/>. 
        /// Call this method to refresh and apply the whole filter expression.
        /// </remarks>
        public void RebuildFilter()
        {
            mFilterIsActive = false;
            string Filter = "";
            foreach (DgvBaseColumnFilter CF in mColumnFilterList)
            {
                if (CF == null) continue;
                if (CF.Active && CF.FilterExpression != "")
                {
                    Filter += " AND (" + CF.FilterExpression + ")";
                    CF.DataGridViewColumn.HeaderText = CF.FilterCaption;
                }
                else
                {
                    CF.DataGridViewColumn.HeaderText = CF.OriginalDataGridViewColumnHeaderText;
                }
            }

            if (Filter != "")
            {
                mFilterIsActive = true;
                Filter = (mBaseFilter == "") ? "1=1 " + Filter : mBaseFilter + " " + Filter;
            }
            else
            {
                Filter = mBaseFilter;
            }

            if (_DataSourceView.RowFilter != Filter)
            {
                _DataSourceView.RowFilter = Filter;
            }
        }

        #endregion


        #region HELPERS

        // Checks if the DataGridView is data bound and the data source finally resolves to a DataView.
        public DataView TransDataSource(object dataSource)
        {
            DataView result = new DataView();

            if (dataSource != null)
            {
                if (dataSource is BindingSource)
                {
                    if (((BindingSource)dataSource).DataSource is DataTable)
                    {
                        result = new DataView(((BindingSource)dataSource).DataSource as DataTable);
                    }
                }
                else if (dataSource is DataSet)
                {
                    result = new DataView(((DataSet)dataSource).Tables[this.DataMember]);
                }
                else if (dataSource is DataTable)
                {
                    result = new DataView(dataSource as DataTable);
                }
                else if (dataSource is DataView)
                {
                    result = dataSource as DataView;
                }
                else if (dataSource is IList)
                {
                    result = new DataView(ConvertToDataTable(dataSource as IList));
                }
            }

            return result;
        }

        /// <summary>
        /// 将LINQ的查询结果转换为DataTable最简单的实现方法
        /// </summary>
        /// <param name="queryable">要转换的结果集</param>
        /// <returns>转换后的表</returns>
        DataTable ConvertToDataTable(IList queryable)
        {
            DataTable dt = new DataTable();

            var props = queryable[0].GetType().GetProperties();

            foreach (System.Reflection.PropertyInfo pi in props)
            {
                if (pi.PropertyType.Name.Contains("EntitySet"))
                {
                    continue;
                }

                string ddd = pi.PropertyType.ToString();

                if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Int32]"))
                    || (pi.PropertyType == System.Type.GetType("System.Int32")))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Int32"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Int64]"))
                    || pi.PropertyType == System.Type.GetType("System.Int64"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Int64"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Int16]"))
                    || pi.PropertyType == System.Type.GetType("System.Int16"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Int16"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Byte]"))
                    || pi.PropertyType == System.Type.GetType("System.Byte"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Byte"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Guid]"))
                    || pi.PropertyType == System.Type.GetType("System.Guid"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Guid"));
                }
                else if (pi.PropertyType == System.Type.GetType("System.String"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.String"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Boolean]"))
                    || (pi.PropertyType == System.Type.GetType("System.Boolean")))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Boolean"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.DateTime]"))
                    || (pi.PropertyType == System.Type.GetType("System.DateTime")))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.DateTime"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Double]"))
                    || pi.PropertyType == System.Type.GetType("System.Double"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Double"));
                }
                else if ((pi.PropertyType == System.Type.GetType("System.Nullable`1[System.Single]"))
                    || pi.PropertyType == System.Type.GetType("System.Single"))
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Single"));
                }
                else if (pi.PropertyType.FullName == "System.Data.Linq.Binary")
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Data.Linq.Binary"));
                }
                else if (pi.PropertyType.FullName == "System.Data.Linq.XElement")
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Data.Linq.XElement"));
                }
                else if (pi.PropertyType.FullName == "System.Object")//sql_variant_null
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Object"));
                }
                else
                {
                    dt.Columns.Add(pi.Name, System.Type.GetType("System.Decimal"));
                }

            }
            foreach (var item in queryable)
            {
                DataRow dr = dt.NewRow();

                foreach (System.Reflection.PropertyInfo pi in props)
                {
                    if (pi.GetValue(item, null) != null
                        && pi.GetValue(item, null) != null
                        && dt.Columns.Contains(pi.Name))
                    {
                        dr[pi.Name] = pi.GetValue(item, null);
                    }
                }

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private void CreateColumnFilter(DataGridViewColumn c)
        {
            if (!mAutoCreateFilters) return;
            //Raise the event about column filter creation

            DgvBaseColumnFilter baseColFilter = new DgvBaseColumnFilter();

            switch (c.GetType().Name)
            {
                case "DataGridViewComboBoxColumn":
                    baseColFilter = new DgvComboBoxColumnFilter();
                    break;
                case "DataGridViewCheckBoxColumn":
                    baseColFilter = new DgvCheckBoxColumnFilter();
                    break;
                case "DataGridViewTextBoxColumn":
                    baseColFilter = new DgvTextBoxColumnFilter();

                    if (_DataSourceView.Table != null
                        && _DataSourceView.Table.Columns[c.DataPropertyName].DataType == typeof(DateTime))
                    {
                        baseColFilter = new DgvDateColumnFilter();
                    }
                    break;
            }

            ColumnFilterEventArgs e = new ColumnFilterEventArgs(c, baseColFilter);
            if (ColumnFilterAdding != null) ColumnFilterAdding(this, e);
            mColumnFilterList.Insert(c.Index, e.ColumnFilter);

            if (e.ColumnFilter != null)// == null when non-data column
            {
                if (ColumnFilterAdded != null)
                {
                    ColumnFilterAdded(this, e);
                }

                e.ColumnFilter.Init(this, _DataSourceView, c.Index);

                ContextMenuStrip oMenuStrip = new ContextMenuStrip();
                MyToolStripMenuItem oToolAscding, oToolDescending, oToolFilter, oToolFilterDelete;

                oToolAscding =
                    new MyToolStripMenuItem("升序", ((System.Drawing.Image)(resources.GetObject("Sort_Ascending"))),
                        new EventHandler(mMenuStripSort_Click));
                oToolAscding.Name = "toolscriptmenuitemasc";
                oToolAscding.DgvBaseColumn = e.ColumnFilter;
                oToolAscding.Size = new System.Drawing.Size(213, 22);
                oToolAscding.SortDirection = ListSortDirection.Ascending;
                oMenuStrip.Items.Add(oToolAscding);

                oToolDescending =
                    new MyToolStripMenuItem("降序", ((System.Drawing.Image)(resources.GetObject("Sort_Descending"))),
                        new EventHandler(mMenuStripSort_Click));
                oToolDescending.Name = "toolscriptmenuitemdesc";
                oToolDescending.DgvBaseColumn = e.ColumnFilter;
                oToolDescending.Size = new System.Drawing.Size(213, 22);
                oToolDescending.SortDirection = ListSortDirection.Descending;
                oMenuStrip.Items.Add(oToolDescending);

                oToolFilter =
                    new MyToolStripMenuItem("过滤", ((System.Drawing.Image)(resources.GetObject("Filter_Add"))),
                        new EventHandler(mMenuStripFilter_Click));
                oToolFilter.Name = "toolscriptmenuitemFilter";
                oToolFilter.DgvBaseColumn = e.ColumnFilter;
                oMenuStrip.Items.Add(oToolFilter);

                oToolFilterDelete =
                    new MyToolStripMenuItem("取消过滤", ((System.Drawing.Image)(resources.GetObject("Filter_Delete"))),
                        new EventHandler(mMenuStripFilterDelete_Click));
                oToolFilterDelete.Name = "toolscriptmenuitemFilterDelete";
                oToolFilterDelete.DgvBaseColumn = e.ColumnFilter;
                oMenuStrip.Items.Add(oToolFilterDelete);

                c.HeaderCell.ContextMenuStrip = oMenuStrip;
            }

            if (mColumnFilterList == null)
            {
                FilterHost.ComboBoxColumns.DataSource = null;
                return;
            }

            Dictionary<DgvBaseColumnFilter, string> dicCombox = new Dictionary<DgvBaseColumnFilter, string>();

            foreach (DgvBaseColumnFilter oCol in mColumnFilterList)
            {
                dicCombox.Add(oCol, oCol.OriginalDataGridViewColumnHeaderText);
            }

            BindingSource bs = new BindingSource();

            bs.DataSource = dicCombox;

            FilterHost.ComboBoxColumns.DataSource = bs;
            FilterHost.ComboBoxColumns.ValueMember = "Key";
            FilterHost.ComboBoxColumns.DisplayMember = "Value";
        }

        #endregion
    }
}
