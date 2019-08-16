using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{

    public class DataGridViewTextBoxButtonColumn : DataGridViewColumn
    {
        public DataGridViewTextBoxButtonColumn()
            : base(new DataGridViewTextBoxButtonCell())
        {
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(DataGridViewTextBoxButtonCell)))
                {
                    throw new InvalidCastException("Must be a DataGridViewTextBoxButtonCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    partial class DataGridViewTextBoxButtonEditingControl : UserControl, IDataGridViewEditingControl
    {
        //private static readonly DataGridViewContentAlignment anyCenter;
        //private static readonly DataGridViewContentAlignment anyRight;
        //private static readonly DataGridViewContentAlignment anyTop;

        private DataGridView dataGridView;
        private int rowIndex;
        private bool valueChanged;
        private bool repositionOnValueChanged;

        public DataGridViewTextBoxButtonEditingControl()
        {
            InitializeComponent();
        }

        #region Implement the IDataGridViewEditingControl's Method and Property
        /// <summary>
        /// Implements the IDataGridViewEditingControl.EditingControlDataGridView property
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
        /// Implements the IDataGridViewEditingControl.EditingControlFormattedValue.Property
        /// </summary>
        public object EditingControlFormattedValue
        {
            get
            {
                return Value;
            }
            set
            {
                this.Value = (string)value;
            }
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.EditingCotrolRowIndex
        /// </summary>
        public int EditingControlRowIndex
        {
            get { return this.rowIndex; }
            set { this.rowIndex = value; }
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.EditingControlValueChanged
        /// </summary>
        public bool EditingControlValueChanged
        {
            get { return this.valueChanged; }
            set { this.valueChanged = value; }
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.EditingPanelCursor
        /// </summary>
        public Cursor EditingPanelCursor
        {
            get { return Cursors.Default; }
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.RepositionEditingControlOnValueChange
        /// </summary>
        public bool RepositionEditingControlOnValueChange
        {
            get { return this.repositionOnValueChanged; }
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.ApplyCellStyleToEditingControl
        /// </summary>
        /// <param name="dataGridViewCellStyle"></param>
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            this.txtEdit.Font = dataGridViewCellStyle.Font;
            if (dataGridViewCellStyle.BackColor.A < 0xff)
            {
                Color color = Color.FromArgb(0xff, dataGridViewCellStyle.BackColor);
                this.txtEdit.BackColor = color;
                this.dataGridView.EditingPanel.BackColor = color;
            }
            else
            {
                this.txtEdit.BackColor = dataGridViewCellStyle.BackColor;
            }
            this.txtEdit.ForeColor = dataGridViewCellStyle.ForeColor;
            if (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True)
            {
                this.txtEdit.WordWrap = true;
            }
            this.repositionOnValueChanged = (dataGridViewCellStyle.WrapMode == DataGridViewTriState.True) 
                && ((dataGridViewCellStyle.Alignment) == DataGridViewContentAlignment.NotSet);
                //&& ((dataGridViewCellStyle.Alignment & anyTop) == DataGridViewContentAlignment.NotSet);
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.EditingControlWantsInputKey
        /// </summary>
        /// <param name="keyData"></param>
        /// <param name="dataGridViewWantsInputKey"></param>
        /// <returns></returns>
        public bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            switch ((keyData & Keys.KeyCode))
            {
                case Keys.Prior:
                case Keys.Next:
                    if (!this.valueChanged)
                    {
                        break;
                    }
                    return true;

                case Keys.End:
                case Keys.Home:
                    if (this.txtEdit.SelectionLength == this.txtEdit.Text.Length)
                    {
                        break;
                    }
                    return true;

                case Keys.Left:
                    if (((this.txtEdit.RightToLeft != RightToLeft.No) || ((this.txtEdit.SelectionLength == 0))) && ((this.txtEdit.RightToLeft != RightToLeft.Yes) || ((this.txtEdit.SelectionLength == 0))))
                    {
                        break;
                    }
                    return true;

                case Keys.Up:
                    if ((this.txtEdit.Text.IndexOf("/r/n") < 0) || ((this.txtEdit.SelectionLength) < this.txtEdit.Text.IndexOf("/r/n")))
                    {
                        break;
                    }
                    return true;

                case Keys.Right:
                    if (((this.txtEdit.RightToLeft != RightToLeft.No) || ((this.txtEdit.SelectionLength == 0))) && ((this.txtEdit.RightToLeft != RightToLeft.Yes) || ((this.txtEdit.SelectionLength == 0))))
                    {
                        break;
                    }
                    return true;

                case Keys.Down:
                    {
                        int startIndex = this.txtEdit.SelectionLength;
                        if (this.Text.IndexOf("/r/n", startIndex) == -1)
                        {
                            break;
                        }
                        return true;
                    }
                case Keys.Delete:
                    if ((this.txtEdit.SelectionLength <= 0))
                    {
                        break;
                    }
                    return true;

                case Keys.Return:
                    if ((((keyData & (Keys.Alt | Keys.Control | Keys.Shift)) == Keys.Shift) && this.txtEdit.Multiline))
                    {
                        return true;
                    }
                    break;
            }
            return !dataGridViewWantsInputKey;
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.GetEditingControlFormattedValue
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return EditingControlFormattedValue;
        }
        /// <summary>
        /// Implement the IDataGridViewEditingControl.PrepareEditingControlForEdit
        /// </summary>
        /// <param name="selectAll"></param>
        public void PrepareEditingControlForEdit(bool selectAll)
        {

        }
        //private static HorizontalAlignment TranslateAlignment(DataGridViewContentAlignment align)
        //{
        //    if ((align & anyRight) != DataGridViewContentAlignment.NotSet)
        //    {
        //        return HorizontalAlignment.Right;
        //    }
        //    if ((align & anyCenter) != DataGridViewContentAlignment.NotSet)
        //    {
        //        return HorizontalAlignment.Center;
        //    }
        //    return HorizontalAlignment.Left;
        //}
        #endregion

        public string Value
        {
            get { return this.txtEdit.Text; }
            set { this.txtEdit.Text = value; }
        }

        public int MaxLength
        {
            get { return this.txtEdit.MaxLength; }
            set { this.txtEdit.MaxLength = value; }
        }

        public new BorderStyle BorderStyle
        {
            get { return this.txtEdit.BorderStyle; }
            set { this.txtEdit.BorderStyle = value; }
        }

        /// <summary>
        /// 设置对话框的显示位置
        /// </summary>
        private Point SetDialogPosition()
        {
            Point pt = new Point();
            pt = this.Location;
            pt = PointToScreen(pt);
            pt.X -= this.Location.X;
            pt.Y -= this.Location.Y - this.Height - 2;

            return pt;
        }

        private void btn_Click(object sender, EventArgs e)
        {
            EditForm frm = new EditForm(Value, SetDialogPosition());
            frm.ShowDialog();
            if (!GlobalObject.GeneralFunction.IsNullOrEmpty(frm.TextString))
                Value = frm.TextString;
            this.valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }

        private void txtEdit_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEdit_Leave(object sender, EventArgs e)
        {
            Value = txtEdit.Text;
            this.valueChanged = true;
            this.EditingControlDataGridView.NotifyCurrentCellDirty(true);
        }
    }

    public class DataGridViewTextBoxButtonCell : DataGridViewTextBoxCell
    {
        public DataGridViewTextBoxButtonCell()
            : base()
        {
        }

        public override void InitializeEditingControl(int rowIndex, object initialFormattedValue, DataGridViewCellStyle dataGridViewCellStyle)
        {
            base.InitializeEditingControl(rowIndex, initialFormattedValue, dataGridViewCellStyle);
            DataGridViewTextBoxButtonEditingControl ctl = DataGridView.EditingControl as DataGridViewTextBoxButtonEditingControl;
            if (this.Value != null)
                ctl.Value = this.Value.ToString();
            else
                ctl.Value = string.Empty;
        }

        public override Type EditType
        {
            get
            {
                return typeof(DataGridViewTextBoxButtonEditingControl);

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
