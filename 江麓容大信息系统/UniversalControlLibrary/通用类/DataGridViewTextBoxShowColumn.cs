using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;

namespace UniversalControlLibrary
{
    public class DataGridViewTextBoxShowColumn : DataGridViewColumn
    {
        /// <summary>
        /// 检索的类别
        /// </summary>
        private DataRow m_DataResult = null;

        public DataRow DataResult
        {
            get { return m_DataResult; }
            set { m_DataResult = value; }
        }
        public event GlobalObject.DelegateCollection.NonArgumentHandle m_OnCompleteSearch;

        [
            Category("Data"),
            Description("选择获得数据集的类型"),
            RefreshProperties(RefreshProperties.All)
        ]

        /// <summary>
        /// 获取或设置要检索的数据类别
        /// </summary>
        public TextBoxShow.FindType FindItem
        {
            get
            {
                if (this.TextBoxShowCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                return this.TextBoxShowCellTemplate.FindItem;
            }

            set
            {
                if (this.TextBoxShowCellTemplate == null)
                {
                    throw new InvalidOperationException("Operation cannot be completed because this DataGridViewColumn does not have a CellTemplate.");
                }
                // Update the template cell so that subsequent cloned cells use the new value.
                this.TextBoxShowCellTemplate.FindItem = value;
                if (this.DataGridView != null)
                {
                    // Update all the existing DataGridViewNumericUpDownCell cells in the column accordingly.
                    DataGridViewRowCollection dataGridViewRows = this.DataGridView.Rows;
                    int rowCount = dataGridViewRows.Count;
                    for (int rowIndex = 0; rowIndex < rowCount; rowIndex++)
                    {
                        // Be careful not to unshare rows unnecessarily. 
                        // This could have severe performance repercussions.
                        DataGridViewRow dataGridViewRow = dataGridViewRows.SharedRow(rowIndex);
                        DataGridViewTextBoxShowCell dataGridViewCell = dataGridViewRow.Cells[this.Index] as DataGridViewTextBoxShowCell;
                        if (dataGridViewCell != null)
                        {
                            // Call the internal SetDecimalPlaces method instead of the property to avoid invalidation 
                            // of each cell. The whole column is invalidated later in a single operation for better performance.
                            dataGridViewCell.SetFindItem(rowIndex, value);
                        }
                    }
                    this.DataGridView.InvalidateColumn(this.Index);
                    // TODO: Call the grid's autosizing methods to autosize the column, rows, column headers / row headers as needed.
                }
            }
        }

        public DataGridViewTextBoxShowColumn()
            : base(new DataGridViewTextBoxShowCell())
        {

        }

        //internal protected virtual 
        internal protected virtual void OnCompleteSearch()
        {
            m_DataResult = ((DataGridViewTextBoxShowCell)this.DataGridView.CurrentCell).m_DataResult;
            if (m_OnCompleteSearch != null)
            {
                m_OnCompleteSearch();
            }

        }

        DataGridViewTextBoxShowCell TextBoxShowCellTemplate
        {
            get { return (DataGridViewTextBoxShowCell)this.CellTemplate; }
        }

        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                DataGridViewTextBoxShowCell cell = value as DataGridViewTextBoxShowCell;

                if (value != null && cell != null)
                {
                    throw new InvalidCastException("Must be a DataGridViewTextBoxShowCell");
                }

                base.CellTemplate = value;
            }
        }
    }

    public class DataGridViewTextBoxShowCell : DataGridViewTextBoxCell
    {
        TextBoxShow.FindType m_findType;
        public DataRow m_DataResult = null;
        public string m_EndSql = "";
        public string m_StrSql = "";

        /// <summary>
        /// 要显示的字段集
        /// </summary>
        public string[] m_SZstring;

        /// <summary>
        /// 拼音码列名
        /// </summary>
        public string m_strPyColunm;

        /// <summary>
        /// 五笔码列名
        /// </summary>
        public string m_strWbColunm;

        /// <summary>
        /// 图号
        /// </summary>
        public string m_strCodeColunm;

        /// <summary>
        /// 显示在TextBox中的数据值
        /// </summary>
        public string m_strShowMessage;

        public event GlobalObject.DelegateCollection.NonArgumentHandle m_OnCompleteSearch;

        /// <summary>
        /// Returns the current DataGridView EditingControl as a DataGridViewNumericUpDownEditingControl control
        /// </summary>
        private TextBoxShow EditingTextBoxShow
        {
            get
            {
                return this.DataGridView.EditingControl as TextBoxShow;
            }
        }

        /// <summary>
        /// 获取或设置要检索的数据类别
        /// </summary>
        public TextBoxShow.FindType FindItem
        {
            get
            {
                return m_findType;
            }
            set
            {
                SetFindItem(this.RowIndex, value);
            }
        }

        /// <summary>
        /// Utility function that sets a new value for the DecimalPlaces property of the cell. This function is used by
        /// the cell and column DecimalPlaces property. The column uses this method instead of the DecimalPlaces
        /// property for performance reasons. This way the column can invalidate the entire column at once instead of 
        /// invalidating each cell of the column individually. A row index needs to be provided as a parameter because
        /// this cell may be shared among multiple rows.
        /// </summary>
        internal void SetFindItem(int rowIndex, TextBoxShow.FindType value)
        {
            this.m_findType = value;
            if (OwnsEditingNumericUpDown(rowIndex))
            {
                this.EditingTextBoxShow.FindItem = value;
            }
        }

        /// <summary>
        /// Determines whether this cell, at the given row index, shows the grid's editing control or not.
        /// The row index needs to be provided as a parameter because this cell may be shared among multiple rows.
        /// </summary>
        private bool OwnsEditingNumericUpDown(int rowIndex)
        {
            if (rowIndex == -1 || this.DataGridView == null)
            {
                return false;
            }
            TextBoxShow textBoxShowEditingControl =
                this.DataGridView.EditingControl as TextBoxShow;
            return textBoxShowEditingControl != null 
                && rowIndex == ((IDataGridViewEditingControl)textBoxShowEditingControl).EditingControlRowIndex;
        }

        public DataGridViewTextBoxShowCell()
            : base()
        {

        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            
            if (EditingTextBoxShow != null)
            {
                Delegate[] invokeList = GlobalObject.GeneralFunction.GetObjectEventList(EditingTextBoxShow, "OnCompleteSearch");

                if (invokeList != null)
                {
                    foreach (Delegate del in invokeList)
                    {
                        typeof(TextBoxShow).GetEvent("OnCompleteSearch").RemoveEventHandler(EditingTextBoxShow, del);
                    }
                }

                EditingTextBoxShow.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(EditingTextBoxShow_OnCompleteSearch);

                EditingTextBoxShow._sZstring = m_SZstring;

                EditingTextBoxShow.strPyColunm = m_strPyColunm;
                EditingTextBoxShow.strCodeColunm = m_strCodeColunm;
                EditingTextBoxShow.strWbColunm = m_strWbColunm;
                EditingTextBoxShow.strShowMessage = m_strShowMessage;

                EditingTextBoxShow.strSql = m_StrSql;
                EditingTextBoxShow.StrEndSql = m_EndSql;
                EditingTextBoxShow.FindItem = m_findType;

                if (this.Value != null)
                    EditingTextBoxShow.Text = this.Value.ToString();
                else
                    EditingTextBoxShow.Text = string.Empty;
            }
        }

        void EditingTextBoxShow_OnCompleteSearch()
        {
            m_DataResult = EditingTextBoxShow.DataResult;
            ((DataGridViewTextBoxShowColumn)this.OwningColumn).OnCompleteSearch();
        }

        public override object Clone()
        {
            DataGridViewTextBoxShowCell cell = (DataGridViewTextBoxShowCell)base.Clone();

            if (cell != null)
            {
                cell.m_OnCompleteSearch = m_OnCompleteSearch;
                cell.FindItem = this.FindItem;
            }

            return cell;
        }

        public override Type EditType
        {
            get
            {
                return typeof(TextBoxShow);
            }
        }

        public override Type ValueType
        {
            get { return typeof(string); }
        }

        public override object DefaultNewRowValue
        {
            get
            {
                return string.Empty;
            }
        }
    }
}
