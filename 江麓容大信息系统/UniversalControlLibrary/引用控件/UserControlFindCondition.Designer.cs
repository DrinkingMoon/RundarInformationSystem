namespace UniversalControlLibrary
{
    partial class UserControlFindCondition
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cmbFindField = new System.Windows.Forms.ComboBox();
            this.cmbOperator = new System.Windows.Forms.ComboBox();
            this.txtFindData = new System.Windows.Forms.TextBox();
            this.cmbRelationSymbol = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dateTime = new System.Windows.Forms.DateTimePicker();
            this.cmbFindData = new System.Windows.Forms.ComboBox();
            this.cmbLeft = new System.Windows.Forms.ComboBox();
            this.cmbRight = new System.Windows.Forms.ComboBox();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.numPrice = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbFindField
            // 
            this.cmbFindField.BackColor = System.Drawing.Color.White;
            this.cmbFindField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFindField.FormattingEnabled = true;
            this.cmbFindField.Location = new System.Drawing.Point(75, 5);
            this.cmbFindField.Name = "cmbFindField";
            this.cmbFindField.Size = new System.Drawing.Size(235, 22);
            this.cmbFindField.TabIndex = 0;
            this.cmbFindField.SelectedIndexChanged += new System.EventHandler(this.comFindField_SelectedIndexChanged);
            this.cmbFindField.MouseDown += new System.Windows.Forms.MouseEventHandler(this.comFindField_MouseDown);
            // 
            // cmbOperator
            // 
            this.cmbOperator.BackColor = System.Drawing.Color.White;
            this.cmbOperator.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOperator.FormattingEnabled = true;
            this.cmbOperator.Items.AddRange(new object[] {
            "=",
            "<>",
            "<",
            "<=",
            ">",
            ">=",
            "包含",
            "是",
            "不是"});
            this.cmbOperator.Location = new System.Drawing.Point(310, 5);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(77, 22);
            this.cmbOperator.TabIndex = 1;
            this.cmbOperator.SelectedIndexChanged += new System.EventHandler(this.cmbOperator_SelectedIndexChanged);
            // 
            // txtFindData
            // 
            this.txtFindData.Location = new System.Drawing.Point(388, 5);
            this.txtFindData.Name = "txtFindData";
            this.txtFindData.Size = new System.Drawing.Size(208, 23);
            this.txtFindData.TabIndex = 2;
            // 
            // cmbRelationSymbol
            // 
            this.cmbRelationSymbol.BackColor = System.Drawing.Color.White;
            this.cmbRelationSymbol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRelationSymbol.FormattingEnabled = true;
            this.cmbRelationSymbol.Items.AddRange(new object[] {
            "and",
            "or"});
            this.cmbRelationSymbol.Location = new System.Drawing.Point(664, 5);
            this.cmbRelationSymbol.Name = "cmbRelationSymbol";
            this.cmbRelationSymbol.Size = new System.Drawing.Size(77, 22);
            this.cmbRelationSymbol.TabIndex = 3;
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.SystemColors.Control;
            this.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFind.Image = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFind.Location = new System.Drawing.Point(597, 5);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(23, 23);
            this.btnFind.TabIndex = 78;
            this.btnFind.UseVisualStyleBackColor = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(741, 3);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(43, 27);
            this.btnDelete.TabIndex = 79;
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dateTime
            // 
            this.dateTime.CalendarMonthBackground = System.Drawing.Color.White;
            this.dateTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.dateTime.Location = new System.Drawing.Point(388, 5);
            this.dateTime.Name = "dateTime";
            this.dateTime.Size = new System.Drawing.Size(208, 23);
            this.dateTime.TabIndex = 80;
            this.dateTime.Visible = false;
            // 
            // cmbFindData
            // 
            this.cmbFindData.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFindData.FormattingEnabled = true;
            this.cmbFindData.Location = new System.Drawing.Point(388, 5);
            this.cmbFindData.Name = "cmbFindData";
            this.cmbFindData.Size = new System.Drawing.Size(208, 22);
            this.cmbFindData.TabIndex = 81;
            this.cmbFindData.Visible = false;
            // 
            // cmbLeft
            // 
            this.cmbLeft.BackColor = System.Drawing.Color.White;
            this.cmbLeft.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeft.FormattingEnabled = true;
            this.cmbLeft.Items.AddRange(new object[] {
            "",
            "(",
            ")"});
            this.cmbLeft.Location = new System.Drawing.Point(29, 5);
            this.cmbLeft.Name = "cmbLeft";
            this.cmbLeft.Size = new System.Drawing.Size(45, 22);
            this.cmbLeft.TabIndex = 82;
            // 
            // cmbRight
            // 
            this.cmbRight.BackColor = System.Drawing.Color.White;
            this.cmbRight.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbRight.FormattingEnabled = true;
            this.cmbRight.Items.AddRange(new object[] {
            "",
            "(",
            ")"});
            this.cmbRight.Location = new System.Drawing.Point(621, 5);
            this.cmbRight.Name = "cmbRight";
            this.cmbRight.Size = new System.Drawing.Size(45, 22);
            this.cmbRight.TabIndex = 83;
            // 
            // numCount
            // 
            this.numCount.Location = new System.Drawing.Point(388, 5);
            this.numCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(209, 23);
            this.numCount.TabIndex = 84;
            // 
            // numPrice
            // 
            this.numPrice.DecimalPlaces = 3;
            this.numPrice.Increment = new decimal(new int[] {
            1,
            0,
            0,
            196608});
            this.numPrice.Location = new System.Drawing.Point(388, 5);
            this.numPrice.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numPrice.Name = "numPrice";
            this.numPrice.Size = new System.Drawing.Size(209, 23);
            this.numPrice.TabIndex = 85;
            // 
            // UserControlFindCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.numPrice);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.cmbRight);
            this.Controls.Add(this.cmbLeft);
            this.Controls.Add(this.cmbFindData);
            this.Controls.Add(this.dateTime);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.cmbRelationSymbol);
            this.Controls.Add(this.txtFindData);
            this.Controls.Add(this.cmbOperator);
            this.Controls.Add(this.cmbFindField);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlFindCondition";
            this.Size = new System.Drawing.Size(966, 32);
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPrice)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbFindField;
        private System.Windows.Forms.ComboBox cmbOperator;
        private System.Windows.Forms.TextBox txtFindData;
        private System.Windows.Forms.ComboBox cmbRelationSymbol;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DateTimePicker dateTime;
        private System.Windows.Forms.ComboBox cmbFindData;
        private System.Windows.Forms.ComboBox cmbLeft;
        private System.Windows.Forms.ComboBox cmbRight;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.NumericUpDown numPrice;
    }
}
