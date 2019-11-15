using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public class DataGridViewDateTimePickColumn : DataGridViewColumn
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DataGridViewDateTimePickColumn()
            : base(new DataGridViewDateTimePickCell())
        {
        }

        /// <summary>
        /// 获取或设置用于创建新单元格的模板。
        /// </summary>
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                // Ensure that the cell used for the template is a CalendarCell.
                if (value != null &&
                    !value.GetType().IsAssignableFrom(typeof(DataGridViewDateTimePickCell)))
                {
                    throw new InvalidCastException("Must be a CalendarCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    /// <summary>
    /// DateTimePicker单元格控件
    /// </summary>
    class DataGridViewDateTimePickerEditingControl : DateTimePicker, IDataGridViewEditingControl
    {
        DataGridView dataGridView;
        private bool valueChanged = false;
        int rowIndex;

        /// <summary>
        /// 初始化
        /// </summary>
        public DataGridViewDateTimePickerEditingControl()
        {
            this.TabStop = false;
            this.CustomFormat = "yyyy-MM-dd HH:mm";
            this.Format = DateTimePickerFormat.Custom;
        }

        /// <summary>
        /// 重写数据变化方法
        /// </summary>
        /// <param name="eventargs"></param>
        protected override void OnValueChanged(EventArgs eventargs)
        {
            // Notify the DataGridView that the contents of the cell
            // have changed.
            valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
            base.OnValueChanged(eventargs);
        }

        #region IDataGridViewEditingControl 成员

        /// <summary>
        /// 获取或设置编辑器正在修改的单元格的格式化值。
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return this.Value.ToShortDateString();
            }
            set
            {
                if (value is String)
                {
                    this.Value = DateTime.Parse((String)value);
                }
            }
        }

        /// <summary>
        /// 检索单元格的格式化值。
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object GetEditingControlFormattedValue(
            DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }

        /// <summary>
        /// 更改控件的用户界面 (UI)，使之与指定单元格样式一致。
        /// </summary>
        /// <param name="dataGridViewCellStyle"></param>
        public void ApplyCellStyleToEditingControl(
            DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.Font = dataGridViewCellStyle.Font;
            this.CalendarForeColor = dataGridViewCellStyle.ForeColor;
            this.CalendarMonthBackground = dataGridViewCellStyle.BackColor;
        }

        /// <summary>
        /// 获取或设置该承载单元格的父行的索引。
        /// </summary>
        public int EditingControlRowIndex
        {
            get
            {
                return rowIndex;
            }
            set
            {
                rowIndex = value;
            }
        }

        /// <summary>
        /// 确定指定的键是应由编辑控件处理的常规输入键，还是应由 System.Windows.Forms.DataGridView 处理的特殊键。
        /// </summary>
        /// <param name="key"></param>
        /// <param name="dataGridViewWantsInputKey"></param>
        /// <returns></returns>
        public bool EditingControlWantsInputKey(
            Keys key, bool dataGridViewWantsInputKey)
        {
            // Let the DateTimePicker handle the keys listed.
            switch (key & Keys.KeyCode)
            {
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                case Keys.Right:
                case Keys.Home:
                case Keys.End:
                case Keys.PageDown:
                case Keys.PageUp:
                    return true;
                default:
                    return !dataGridViewWantsInputKey;
            }
        }

        /// <summary>
        /// 准备当前选中的单元格以进行编辑。
        /// </summary>
        /// <param name="selectAll"></param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {
            // No preparation needs to be done.
        }

        /// <summary>
        /// 获取或设置一个值，该值指示每当值更改时，是否需要重新定位单元格的内容。
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 获取或设置包含单元格的 System.Windows.Forms.DataGridView。
        /// </summary>
        public DataGridView EditingControlDataGridView
        {
            get
            {
                return dataGridView;
            }
            set
            {
                dataGridView = value;
            }
        }

        /// <summary>
        /// 获取或设置一个值，该值指示编辑控件的值是否与承载单元格的值不同。
        /// </summary>
        public bool EditingControlValueChanged
        {
            get
            {
                return valueChanged;
            }
            set
            {
                valueChanged = value;
            }
        }

        /// <summary>
        /// 获取当鼠标指针位于 System.Windows.Forms.DataGridView.EditingPanel 上方但不位于编辑控件上方时所使用的光标。
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get
            {
                return base.Cursor;
            }
        }

        #endregion
    }

    /// <summary>
    /// 日期选择DataGridViewTextBoxCell
    /// </summary>
    public class DataGridViewDateTimePickCell : DataGridViewTextBoxCell
    {
        /// <summary>
        /// 初始化
        /// </summary>
        public DataGridViewDateTimePickCell()
            : base()
        {
            // Use the short date format.
            this.Style.Format = "d";
        }

        /// <summary>
        /// 附加并初始化寄宿的编辑控件。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <param name="initialFormattedValue"></param>
        /// <param name="dataGridViewCellStyle"></param>
        public override void InitializeEditingControl(int rowIndex, object
            initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            // Set the value of the editing control to the current cell value.
            base.InitializeEditingControl(rowIndex, initialFormattedValue,
                dataGridViewCellStyle);
            DataGridViewDateTimePickerEditingControl ctl =
                DataGridView.EditingControl as DataGridViewDateTimePickerEditingControl;

            if (this.Value != null
                && this.Value.ToString().Trim().Length != 0)
            {
                ctl.Value = (DateTime)this.Value;
            }
            else
            {
                ctl.Value = DateTime.Now;
            }
        }

        /// <summary>
        /// 编辑时的类型
        /// </summary>
        public override Type EditType
        {
            get
            {
                // Return the type of the editing contol that CalendarCell uses.
                return typeof(DataGridViewDateTimePickerEditingControl);
            }
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public override Type ValueType
        {
            get
            {
                // Return the type of the value that CalendarCell contains.
                return typeof(DateTime);
            }
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public override object DefaultNewRowValue
        {
            get
            {
                // Use the current date and time as the default value.
                return DateTime.Now;
            }
        }
    }
}
