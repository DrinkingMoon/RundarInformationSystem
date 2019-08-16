using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Globalization;


namespace UniversalControlLibrary {

    /// <summary>
    /// Is the standard implementation of DgvBaseFilterHost
    /// </summary>
    [ToolboxItem(false)]
    public partial class DgvFilterHost : DgvBaseFilterHost {

        /// <summary>
        /// Initializes a new instance of the <see cref="DgvFilterHost"/> class.
        /// </summary>
        public DgvFilterHost() {
            InitializeComponent();
            this.CurrentColumnFilterChanged += new EventHandler(DgvFilterHost_CurrentColumnFilterChanged);
            //不用点击非弹出框就关闭
            this.Popup.AutoClose = false;
        }

        void DgvFilterHost_CurrentColumnFilterChanged(object sender, EventArgs e) {
            lblColumnName.Text = CurrentColumnFilter.OriginalDataGridViewColumnHeaderText;
        }

        /// <summary>
        /// Return the effective area to which the <i>column filters</i> will be added.
        /// </summary>
        /// <value></value>
        public override Control FilterClientArea {
            get {
                return this.panelFilterArea;
            }
        }

        public override ComboBox ComboBoxColumns
        {
            get
            {
                return this.comboBox_Cols;
            }
            set
            {
                this.comboBox_Cols = value;
            }
        }

        public override Panel PanelFilterText
        {
            get
            {
                return this.panelFilterText;
            }
            set
            {
                this.panelFilterText = value;
            }
        }

        private void tsOK_Click(object sender, EventArgs e) {
            //FilterManager.ActivateFilter(true);
            DataGridView.RebuildFilter();
            this.Popup.Close();
        }

        private void tsRemove_Click(object sender, EventArgs e) {
            DataGridView.ActivateFilter(false);
            this.Popup.Close();
        }

        private void tsRemoveAll_Click(object sender, EventArgs e) {
            DataGridView.ActivateAllFilters(false);
            this.Popup.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Popup.Close();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            //0.激活当前过滤列并过滤
            DataGridView.ActivateFilter(true);

            //1.重新绑定过滤条件
            DataGridView.SetFilterExpression();
        }

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            this.Popup.Mouse_Down(e);
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            this.Popup.Mouse_Move(e);
        }
    }
}
