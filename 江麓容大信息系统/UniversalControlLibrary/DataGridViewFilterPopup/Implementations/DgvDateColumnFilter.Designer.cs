namespace UniversalControlLibrary {
    partial class DgvDateColumnFilter {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            this.comboBoxOperator = new System.Windows.Forms.ComboBox();
            this.dateTimePickerValue = new System.Windows.Forms.DateTimePicker();
            this.SuspendLayout();
            // 
            // comboBoxOperator
            // 
            this.comboBoxOperator.FormattingEnabled = true;
            this.comboBoxOperator.Location = new System.Drawing.Point(3, 4);
            this.comboBoxOperator.Name = "comboBoxOperator";
            this.comboBoxOperator.Size = new System.Drawing.Size(65, 20);
            this.comboBoxOperator.TabIndex = 0;
            // 
            // dateTimePickerValue
            // 
            this.dateTimePickerValue.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dateTimePickerValue.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePickerValue.Location = new System.Drawing.Point(74, 3);
            this.dateTimePickerValue.Name = "dateTimePickerValue";
            this.dateTimePickerValue.Size = new System.Drawing.Size(153, 21);
            this.dateTimePickerValue.TabIndex = 1;
            // 
            // DgvDateColumnFilter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.dateTimePickerValue);
            this.Controls.Add(this.comboBoxOperator);
            this.Name = "DgvDateColumnFilter";
            this.Size = new System.Drawing.Size(230, 26);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox comboBoxOperator;
        private System.Windows.Forms.DateTimePicker dateTimePickerValue;
    }
}
