namespace Form_Peripheral_CompanyQuality
{
    partial class 重点工作查询
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnSelectDetail = new System.Windows.Forms.Button();
            this.txtTaskName = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.customDataGridView2 = new UniversalControlLibrary.CustomDataGridView();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnSelect = new System.Windows.Forms.Button();
            this.cmbMonth = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.cmbYear = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.F_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.重点工作 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F_Id1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.重点工作1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.customGroupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView2)).BeginInit();
            this.customGroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1006, 49);
            this.panel1.TabIndex = 55;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(416, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "重点工作查询";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 49);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1006, 574);
            this.tabControl1.TabIndex = 56;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.customDataGridView1);
            this.tabPage1.Controls.Add(this.customGroupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(998, 548);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "单工作查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.F_Id1,
            this.重点工作1});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 93);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(992, 452);
            this.customDataGridView1.TabIndex = 2;
            this.customDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellDoubleClick);
            this.customDataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellClick);
            this.customDataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.customDataGridView1_DataBindingComplete);
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.BackColor = System.Drawing.Color.White;
            this.customGroupBox1.Controls.Add(this.btnSelectDetail);
            this.customGroupBox1.Controls.Add(this.txtTaskName);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(992, 90);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // btnSelectDetail
            // 
            this.btnSelectDetail.Location = new System.Drawing.Point(782, 34);
            this.btnSelectDetail.Name = "btnSelectDetail";
            this.btnSelectDetail.Size = new System.Drawing.Size(114, 23);
            this.btnSelectDetail.TabIndex = 2;
            this.btnSelectDetail.Text = "查看详细信息";
            this.btnSelectDetail.UseVisualStyleBackColor = true;
            this.btnSelectDetail.Click += new System.EventHandler(this.btnSelectDetail_Click);
            // 
            // txtTaskName
            // 
            this.txtTaskName.DataResult = null;
            this.txtTaskName.DataTableResult = null;
            this.txtTaskName.EditingControlDataGridView = null;
            this.txtTaskName.EditingControlFormattedValue = "";
            this.txtTaskName.EditingControlRowIndex = 0;
            this.txtTaskName.EditingControlValueChanged = true;
            this.txtTaskName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtTaskName.IsMultiSelect = false;
            this.txtTaskName.Location = new System.Drawing.Point(178, 35);
            this.txtTaskName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.ReadOnly = true;
            this.txtTaskName.ShowResultForm = false;
            this.txtTaskName.Size = new System.Drawing.Size(515, 21);
            this.txtTaskName.StrEndSql = null;
            this.txtTaskName.TabIndex = 1;
            this.txtTaskName.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(97, 39);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "重点工作：";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.customDataGridView2);
            this.tabPage2.Controls.Add(this.customGroupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(998, 548);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "月度查询";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // customDataGridView2
            // 
            this.customDataGridView2.AllowUserToAddRows = false;
            this.customDataGridView2.AllowUserToDeleteRows = false;
            this.customDataGridView2.AllowUserToResizeRows = false;
            this.customDataGridView2.AutoCreateFilters = true;
            this.customDataGridView2.BaseFilter = "";
            this.customDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.F_Id,
            this.重点工作});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView2.DefaultCellStyle = dataGridViewCellStyle2;
            this.customDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView2.Location = new System.Drawing.Point(3, 93);
            this.customDataGridView2.Name = "customDataGridView2";
            this.customDataGridView2.ReadOnly = true;
            this.customDataGridView2.RowTemplate.Height = 23;
            this.customDataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView2.Size = new System.Drawing.Size(992, 452);
            this.customDataGridView2.TabIndex = 2;
            this.customDataGridView2.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView2_CellDoubleClick);
            this.customDataGridView2.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.customDataGridView2_DataBindingComplete);
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.BackColor = System.Drawing.Color.White;
            this.customGroupBox2.Controls.Add(this.btnSelect);
            this.customGroupBox2.Controls.Add(this.cmbMonth);
            this.customGroupBox2.Controls.Add(this.label3);
            this.customGroupBox2.Controls.Add(this.cmbYear);
            this.customGroupBox2.Controls.Add(this.label2);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox2.Location = new System.Drawing.Point(3, 3);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(992, 90);
            this.customGroupBox2.TabIndex = 0;
            this.customGroupBox2.TabStop = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(675, 34);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(114, 23);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "查找";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbMonth.Location = new System.Drawing.Point(496, 36);
            this.cmbMonth.MaxYear = 0;
            this.cmbMonth.MinYear = 0;
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(121, 20);
            this.cmbMonth.TabIndex = 3;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbYearMonth_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(427, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "查询月份";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(272, 36);
            this.cmbYear.MaxYear = 0;
            this.cmbYear.MinYear = 0;
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(121, 20);
            this.cmbYear.TabIndex = 1;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYearMonth_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(203, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "查询年份";
            // 
            // F_Id
            // 
            this.F_Id.DataPropertyName = "F_Id";
            this.F_Id.HeaderText = "F_Id";
            this.F_Id.Name = "F_Id";
            this.F_Id.ReadOnly = true;
            this.F_Id.Visible = false;
            // 
            // 重点工作
            // 
            this.重点工作.DataPropertyName = "重点工作";
            this.重点工作.HeaderText = "重点工作";
            this.重点工作.Name = "重点工作";
            this.重点工作.ReadOnly = true;
            this.重点工作.Width = 300;
            // 
            // F_Id1
            // 
            this.F_Id1.DataPropertyName = "F_Id";
            this.F_Id1.HeaderText = "F_Id";
            this.F_Id1.Name = "F_Id1";
            this.F_Id1.ReadOnly = true;
            this.F_Id1.Visible = false;
            // 
            // 重点工作1
            // 
            this.重点工作1.DataPropertyName = "重点工作";
            this.重点工作1.HeaderText = "重点工作";
            this.重点工作1.Name = "重点工作1";
            this.重点工作1.ReadOnly = true;
            this.重点工作1.Width = 300;
            // 
            // 重点工作查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 623);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "重点工作查询";
            this.Text = "重点工作查询";
            this.Load += new System.EventHandler(this.重点工作查询_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView2)).EndInit();
            this.customGroupBox2.ResumeLayout(false);
            this.customGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Button btnSelectDetail;
        private UniversalControlLibrary.TextBoxShow txtTaskName;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView2;
        private System.Windows.Forms.Button btnSelect;
        private UniversalControlLibrary.CustomComboBox cmbMonth;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.CustomComboBox cmbYear;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn 重点工作;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 重点工作1;
    }
}