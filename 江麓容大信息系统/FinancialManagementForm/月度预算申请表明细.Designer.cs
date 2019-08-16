namespace Form_Economic_Financial
{
    partial class 月度预算申请表明细
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
            this.btnSelect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMonthValue = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYearValue = new UniversalControlLibrary.CustomComboBox(this.components);
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.父级科目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预算科目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.年度预算 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.月度预算 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.差异说明年 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.实际金额 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.差异说明月 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.科目ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.btnSelect);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.cmbMonthValue);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.cmbYearValue);
            this.customGroupBox1.Controls.Add(this.lbBillStatus);
            this.customGroupBox1.Controls.Add(this.label69);
            this.customGroupBox1.Controls.Add(this.txtBillNo);
            this.customGroupBox1.Controls.Add(this.label68);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(873, 104);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "主单信息";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(641, 68);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(140, 23);
            this.btnSelect.TabIndex = 428;
            this.btnSelect.Text = "查看年度预算表";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(358, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 427;
            this.label2.Text = "预算月份";
            // 
            // cmbMonthValue
            // 
            this.cmbMonthValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonthValue.FormattingEnabled = true;
            this.cmbMonthValue.Location = new System.Drawing.Point(417, 69);
            this.cmbMonthValue.MaxYear = 0;
            this.cmbMonthValue.MinYear = 0;
            this.cmbMonthValue.Name = "cmbMonthValue";
            this.cmbMonthValue.Size = new System.Drawing.Size(121, 20);
            this.cmbMonthValue.TabIndex = 426;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 73);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 425;
            this.label1.Text = "预算年份";
            // 
            // cmbYearValue
            // 
            this.cmbYearValue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYearValue.FormattingEnabled = true;
            this.cmbYearValue.Location = new System.Drawing.Point(116, 69);
            this.cmbYearValue.MaxYear = 0;
            this.cmbYearValue.MinYear = 0;
            this.cmbYearValue.Name = "cmbYearValue";
            this.cmbYearValue.Size = new System.Drawing.Size(121, 20);
            this.cmbYearValue.TabIndex = 424;
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(452, 31);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(53, 12);
            this.lbBillStatus.TabIndex = 423;
            this.lbBillStatus.Text = "新建单据";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(361, 31);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(53, 12);
            this.label69.TabIndex = 422;
            this.label69.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(116, 27);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(206, 21);
            this.txtBillNo.TabIndex = 421;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(48, 31);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(53, 12);
            this.label68.TabIndex = 420;
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
            this.年度预算,
            this.月度预算,
            this.差异说明年,
            this.实际金额,
            this.差异说明月,
            this.科目ID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 104);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(873, 526);
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
            this.预算科目.HeaderText = "预算科目";
            this.预算科目.Name = "预算科目";
            this.预算科目.ReadOnly = true;
            // 
            // 年度预算
            // 
            this.年度预算.DataPropertyName = "年度预算";
            this.年度预算.HeaderText = "年度预算";
            this.年度预算.Name = "年度预算";
            this.年度预算.ReadOnly = true;
            // 
            // 月度预算
            // 
            this.月度预算.DataPropertyName = "月度预算";
            this.月度预算.DecimalPlaces = 2;
            this.月度预算.HeaderText = "月度预算";
            this.月度预算.Maximum = new decimal(new int[] {
            1874919424,
            2328306,
            0,
            0});
            this.月度预算.Name = "月度预算";
            // 
            // 差异说明年
            // 
            this.差异说明年.DataPropertyName = "差异说明(年)";
            this.差异说明年.HeaderText = "差异说明(年)";
            this.差异说明年.Name = "差异说明年";
            // 
            // 实际金额
            // 
            this.实际金额.DataPropertyName = "实际金额";
            this.实际金额.DecimalPlaces = 2;
            this.实际金额.HeaderText = "实际金额";
            this.实际金额.Maximum = new decimal(new int[] {
            -1530494976,
            232830,
            0,
            0});
            this.实际金额.Name = "实际金额";
            // 
            // 差异说明月
            // 
            this.差异说明月.DataPropertyName = "差异说明(月)";
            this.差异说明月.HeaderText = "差异说明(月)";
            this.差异说明月.Name = "差异说明月";
            this.差异说明月.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.差异说明月.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 科目ID
            // 
            this.科目ID.DataPropertyName = "科目ID";
            this.科目ID.HeaderText = "科目ID";
            this.科目ID.Name = "科目ID";
            this.科目ID.ReadOnly = true;
            this.科目ID.Visible = false;
            // 
            // 月度预算申请表明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 630);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "月度预算申请表明细";
            this.Text = "月度预算申请表明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.月度预算申请表明细_PanelGetDataInfo);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.CustomComboBox cmbMonthValue;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.CustomComboBox cmbYearValue;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn 父级科目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预算科目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 年度预算;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 月度预算;
        private System.Windows.Forms.DataGridViewTextBoxColumn 差异说明年;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 实际金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 差异说明月;
        private System.Windows.Forms.DataGridViewTextBoxColumn 科目ID;
    }
}