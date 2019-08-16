namespace Form_Economic_Purchase
{
    partial class 供应商挂账表
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.操作 = new System.Windows.Forms.TabPage();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.挂账年月 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商简码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据状态 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.挂账表查询 = new System.Windows.Forms.TabPage();
            this.挂账表明细查询 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.操作.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.操作);
            this.tabControl1.Controls.Add(this.挂账表查询);
            this.tabControl1.Controls.Add(this.挂账表明细查询);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 729);
            this.tabControl1.TabIndex = 0;
            // 
            // 操作
            // 
            this.操作.Controls.Add(this.customDataGridView1);
            this.操作.Controls.Add(this.userControlDataLocalizer1);
            this.操作.Controls.Add(this.customGroupBox1);
            this.操作.Location = new System.Drawing.Point(4, 22);
            this.操作.Name = "操作";
            this.操作.Padding = new System.Windows.Forms.Padding(3);
            this.操作.Size = new System.Drawing.Size(1000, 703);
            this.操作.TabIndex = 0;
            this.操作.Text = "操作";
            this.操作.UseVisualStyleBackColor = true;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.挂账年月,
            this.供应商简码,
            this.供应商名称,
            this.单据状态});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 108);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(994, 592);
            this.customDataGridView1.TabIndex = 2;
            this.customDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellDoubleClick);
            // 
            // 挂账年月
            // 
            this.挂账年月.DataPropertyName = "挂账年月";
            this.挂账年月.HeaderText = "挂账年月";
            this.挂账年月.Name = "挂账年月";
            this.挂账年月.Visible = false;
            this.挂账年月.Width = 80;
            // 
            // 供应商简码
            // 
            this.供应商简码.DataPropertyName = "供应商简码";
            this.供应商简码.HeaderText = "供应商简码";
            this.供应商简码.Name = "供应商简码";
            // 
            // 供应商名称
            // 
            this.供应商名称.DataPropertyName = "供应商名称";
            this.供应商名称.HeaderText = "供应商名称";
            this.供应商名称.Name = "供应商名称";
            this.供应商名称.Width = 350;
            // 
            // 单据状态
            // 
            this.单据状态.DataPropertyName = "单据状态";
            this.单据状态.HeaderText = "单据状态";
            this.单据状态.Name = "单据状态";
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(3, 76);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(994, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.BackColor = System.Drawing.Color.White;
            this.customGroupBox1.Controls.Add(this.btnPrint);
            this.customGroupBox1.Controls.Add(this.btnSelect);
            this.customGroupBox1.Controls.Add(this.cmbStatus);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.cmbMonth);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.cmbYear);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(994, 73);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Location = new System.Drawing.Point(892, 29);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 7;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Visible = false;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(787, 29);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 6;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cmbStatus
            // 
            this.cmbStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(583, 30);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(121, 20);
            this.cmbStatus.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(508, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "状    态";
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
            this.cmbMonth.Location = new System.Drawing.Point(341, 30);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(121, 20);
            this.cmbMonth.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(266, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "挂账月份";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(108, 30);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(121, 20);
            this.cmbYear.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "挂账年份";
            // 
            // 挂账表查询
            // 
            this.挂账表查询.Location = new System.Drawing.Point(4, 22);
            this.挂账表查询.Name = "挂账表查询";
            this.挂账表查询.Padding = new System.Windows.Forms.Padding(3);
            this.挂账表查询.Size = new System.Drawing.Size(1000, 703);
            this.挂账表查询.TabIndex = 1;
            this.挂账表查询.Text = "挂账表查询";
            this.挂账表查询.UseVisualStyleBackColor = true;
            // 
            // 挂账表明细查询
            // 
            this.挂账表明细查询.Location = new System.Drawing.Point(4, 22);
            this.挂账表明细查询.Name = "挂账表明细查询";
            this.挂账表明细查询.Size = new System.Drawing.Size(1000, 703);
            this.挂账表明细查询.TabIndex = 2;
            this.挂账表明细查询.Text = "月度入账业务明细查询";
            this.挂账表明细查询.UseVisualStyleBackColor = true;
            // 
            // 供应商挂账表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tabControl1);
            this.Name = "供应商挂账表";
            this.Text = "供应商挂账表";
            this.Load += new System.EventHandler(this.供应商挂账表_Load);
            this.tabControl1.ResumeLayout(false);
            this.操作.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 操作;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.TabPage 挂账表查询;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridViewTextBoxColumn 挂账年月;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商简码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据状态;
        private System.Windows.Forms.TabPage 挂账表明细查询;
        private System.Windows.Forms.Button btnPrint;
    }
}