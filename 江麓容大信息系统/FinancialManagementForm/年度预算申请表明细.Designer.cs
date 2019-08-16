namespace Form_Economic_Financial
{
    partial class 年度预算申请表明细
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYearValue = new UniversalControlLibrary.CustomComboBox(this.components);
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.父级科目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算科目 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.一月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.二月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.三月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.四月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.五月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.六月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.七月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.八月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.九月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.十月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.十一月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.十二月 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.合计 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customContextMenuStrip_Edit1 = new UniversalControlLibrary.CustomContextMenuStrip_Edit(this.components);
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.cmbYearValue);
            this.customGroupBox1.Controls.Add(this.lbBillStatus);
            this.customGroupBox1.Controls.Add(this.label69);
            this.customGroupBox1.Controls.Add(this.txtBillNo);
            this.customGroupBox1.Controls.Add(this.label68);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(873, 62);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "主表信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(592, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 419;
            this.label1.Text = "预算年份";
            // 
            // cmbYearValue
            // 
            this.cmbYearValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYearValue.FormattingEnabled = true;
            this.cmbYearValue.Location = new System.Drawing.Point(651, 24);
            this.cmbYearValue.MaxYear = 0;
            this.cmbYearValue.MinYear = 0;
            this.cmbYearValue.Name = "cmbYearValue";
            this.cmbYearValue.Size = new System.Drawing.Size(121, 20);
            this.cmbYearValue.TabIndex = 418;
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(437, 28);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(53, 12);
            this.lbBillStatus.TabIndex = 417;
            this.lbBillStatus.Text = "新建单据";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(346, 28);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(53, 12);
            this.label69.TabIndex = 416;
            this.label69.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(101, 24);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(206, 21);
            this.txtBillNo.TabIndex = 415;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(33, 28);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(53, 12);
            this.label68.TabIndex = 414;
            this.label68.Text = "单 据 号";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.父级科目,
            this.预算科目,
            this.一月,
            this.二月,
            this.三月,
            this.四月,
            this.五月,
            this.六月,
            this.七月,
            this.八月,
            this.九月,
            this.十月,
            this.十一月,
            this.十二月,
            this.合计,
            this.科目ID});
            this.customDataGridView1.ContextMenuStrip = this.customContextMenuStrip_Edit1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 62);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(873, 568);
            this.customDataGridView1.TabIndex = 2;
            this.customDataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellEndEdit);
            // 
            // 父级科目
            // 
            this.父级科目.DataPropertyName = "父级科目";
            this.父级科目.HeaderText = "父级科目";
            this.父级科目.Name = "父级科目";
            this.父级科目.ReadOnly = true;
            // 
            // 预算科目
            // 
            this.预算科目.DataPropertyName = "预算科目";
            this.预算科目.DataResult = null;
            this.预算科目.FindItem = UniversalControlLibrary.TextBoxShow.FindType.预算科目;
            this.预算科目.HeaderText = "预算科目";
            this.预算科目.Name = "预算科目";
            // 
            // 一月
            // 
            this.一月.DataPropertyName = "1月";
            this.一月.DecimalPlaces = 2;
            this.一月.HeaderText = "1月";
            this.一月.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.一月.Name = "一月";
            // 
            // 二月
            // 
            this.二月.DataPropertyName = "2月";
            this.二月.DecimalPlaces = 2;
            this.二月.HeaderText = "2月";
            this.二月.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.二月.Name = "二月";
            // 
            // 三月
            // 
            this.三月.DataPropertyName = "3月";
            this.三月.DecimalPlaces = 2;
            this.三月.HeaderText = "3月";
            this.三月.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.三月.Name = "三月";
            // 
            // 四月
            // 
            this.四月.DataPropertyName = "4月";
            this.四月.DecimalPlaces = 2;
            this.四月.HeaderText = "4月";
            this.四月.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.四月.Name = "四月";
            // 
            // 五月
            // 
            this.五月.DataPropertyName = "5月";
            this.五月.DecimalPlaces = 2;
            this.五月.HeaderText = "5月";
            this.五月.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.五月.Name = "五月";
            // 
            // 六月
            // 
            this.六月.DataPropertyName = "6月";
            this.六月.DecimalPlaces = 2;
            this.六月.HeaderText = "6月";
            this.六月.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.六月.Name = "六月";
            // 
            // 七月
            // 
            this.七月.DataPropertyName = "7月";
            this.七月.DecimalPlaces = 2;
            this.七月.HeaderText = "7月";
            this.七月.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.七月.Name = "七月";
            // 
            // 八月
            // 
            this.八月.DataPropertyName = "8月";
            this.八月.DecimalPlaces = 2;
            this.八月.HeaderText = "8月";
            this.八月.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.八月.Name = "八月";
            // 
            // 九月
            // 
            this.九月.DataPropertyName = "9月";
            this.九月.DecimalPlaces = 2;
            this.九月.HeaderText = "9月";
            this.九月.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.九月.Name = "九月";
            // 
            // 十月
            // 
            this.十月.DataPropertyName = "10月";
            this.十月.DecimalPlaces = 2;
            this.十月.HeaderText = "10月";
            this.十月.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.十月.Name = "十月";
            // 
            // 十一月
            // 
            this.十一月.DataPropertyName = "11月";
            this.十一月.DecimalPlaces = 2;
            this.十一月.HeaderText = "11月";
            this.十一月.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.十一月.Name = "十一月";
            // 
            // 十二月
            // 
            this.十二月.DataPropertyName = "12月";
            this.十二月.DecimalPlaces = 2;
            this.十二月.HeaderText = "12月";
            this.十二月.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.十二月.Name = "十二月";
            // 
            // 合计
            // 
            this.合计.DataPropertyName = "合计";
            this.合计.HeaderText = "合计";
            this.合计.Name = "合计";
            this.合计.ReadOnly = true;
            // 
            // 科目ID
            // 
            this.科目ID.DataPropertyName = "科目ID";
            this.科目ID.HeaderText = "科目ID";
            this.科目ID.Name = "科目ID";
            this.科目ID.Visible = false;
            // 
            // customContextMenuStrip_Edit1
            // 
            this.customContextMenuStrip_Edit1.IsAddFirstRow = false;
            this.customContextMenuStrip_Edit1.Name = "customContextMenuStrip_Edit1";
            this.customContextMenuStrip_Edit1.Size = new System.Drawing.Size(153, 114);
            this.customContextMenuStrip_Edit1._InputEvent += new GlobalObject.DelegateCollection.DataTableHandle(this.customContextMenuStrip_Edit1__InputEvent);
            this.customContextMenuStrip_Edit1._DeleteEvent += new GlobalObject.DelegateCollection.DataGridViewEditRow(this.customContextMenuStrip_Edit1__DeleteEvent);
            // 
            // 年度预算申请表明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 630);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "年度预算申请表明细";
            this.Text = "年度预算申请单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.年度预算申请表明细_PanelGetDataInfo);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label68;
        private UniversalControlLibrary.CustomComboBox cmbYearValue;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.CustomContextMenuStrip_Edit customContextMenuStrip_Edit1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 父级科目;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 预算科目;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 一月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 二月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 三月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 四月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 五月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 六月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 七月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 八月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 九月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 十月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 十一月;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 十二月;
        private System.Windows.Forms.DataGridViewTextBoxColumn 合计;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目ID;
    }
}