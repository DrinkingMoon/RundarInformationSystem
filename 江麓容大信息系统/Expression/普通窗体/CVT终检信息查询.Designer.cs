namespace Expression
{
    partial class CVT终检信息查询
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.箱号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.下线试验信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.审核时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.称重信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.称重时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.气密性信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.检测时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.终检人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.btnOutPut = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.customPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.Title = "将查询结果保存成 EXCEL 文件";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.型号,
            this.箱号,
            this.下线试验信息,
            this.审核时间,
            this.称重信息,
            this.称重时间,
            this.气密性信息,
            this.检测时间,
            this.终检人});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle3;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 96);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(956, 394);
            this.customDataGridView1.TabIndex = 2;
            // 
            // 型号
            // 
            this.型号.DataPropertyName = "型号";
            this.型号.HeaderText = "型号";
            this.型号.Name = "型号";
            this.型号.ReadOnly = true;
            // 
            // 箱号
            // 
            this.箱号.DataPropertyName = "箱号";
            this.箱号.HeaderText = "箱号";
            this.箱号.Name = "箱号";
            this.箱号.ReadOnly = true;
            // 
            // 下线试验信息
            // 
            this.下线试验信息.DataPropertyName = "下线试验信息";
            this.下线试验信息.HeaderText = "下线试验信息";
            this.下线试验信息.Name = "下线试验信息";
            this.下线试验信息.ReadOnly = true;
            // 
            // 审核时间
            // 
            this.审核时间.DataPropertyName = "审核时间";
            this.审核时间.HeaderText = "审核时间";
            this.审核时间.Name = "审核时间";
            this.审核时间.ReadOnly = true;
            // 
            // 称重信息
            // 
            this.称重信息.DataPropertyName = "称重信息";
            this.称重信息.HeaderText = "称重信息";
            this.称重信息.Name = "称重信息";
            this.称重信息.ReadOnly = true;
            // 
            // 称重时间
            // 
            this.称重时间.DataPropertyName = "称重时间";
            this.称重时间.HeaderText = "称重时间";
            this.称重时间.Name = "称重时间";
            this.称重时间.ReadOnly = true;
            // 
            // 气密性信息
            // 
            this.气密性信息.DataPropertyName = "气密性信息";
            this.气密性信息.HeaderText = "气密性信息";
            this.气密性信息.Name = "气密性信息";
            this.气密性信息.ReadOnly = true;
            // 
            // 检测时间
            // 
            this.检测时间.DataPropertyName = "检测时间";
            this.检测时间.HeaderText = "检测时间";
            this.检测时间.Name = "检测时间";
            this.检测时间.ReadOnly = true;
            // 
            // 终检人
            // 
            this.终检人.DataPropertyName = "终检人";
            this.终检人.HeaderText = "终检人";
            this.终检人.Name = "终检人";
            this.终检人.ReadOnly = true;
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.tabControl1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(956, 96);
            this.customPanel1.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(956, 96);
            this.tabControl1.TabIndex = 804;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(948, 70);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "箱号查询";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(948, 70);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "日期查询";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtProductCode);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbProductType);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(942, 64);
            this.groupBox2.TabIndex = 804;
            this.groupBox2.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(778, 27);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 807;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(684, 27);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 804;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 803;
            this.label2.Text = "箱    号";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(419, 28);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(241, 21);
            this.txtProductCode.TabIndex = 802;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(89, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 801;
            this.label1.Text = "型    号";
            // 
            // cmbProductType
            // 
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Location = new System.Drawing.Point(148, 28);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(182, 20);
            this.cmbProductType.TabIndex = 806;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpEnd);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpStart);
            this.groupBox1.Controls.Add(this.btnOutPut);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(942, 64);
            this.groupBox1.TabIndex = 803;
            this.groupBox1.TabStop = false;
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(627, 25);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(75, 23);
            this.btnSelect.TabIndex = 810;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(377, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 809;
            this.label4.Text = "到";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(417, 26);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(182, 21);
            this.dtpEnd.TabIndex = 808;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(144, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 12);
            this.label3.TabIndex = 807;
            this.label3.Text = "从";
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(177, 26);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(182, 21);
            this.dtpStart.TabIndex = 806;
            // 
            // btnOutPut
            // 
            this.btnOutPut.Location = new System.Drawing.Point(724, 25);
            this.btnOutPut.Name = "btnOutPut";
            this.btnOutPut.Size = new System.Drawing.Size(75, 23);
            this.btnOutPut.TabIndex = 805;
            this.btnOutPut.Text = "导出";
            this.btnOutPut.UseVisualStyleBackColor = true;
            this.btnOutPut.Click += new System.EventHandler(this.btnOutPut_Click);
            // 
            // CVT终检信息查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 490);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customPanel1);
            this.Name = "CVT终检信息查询";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CVT终检信息查询";
            this.Load += new System.EventHandler(this.CVT终检信息查询_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customPanel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 箱号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 下线试验信息;
        private System.Windows.Forms.DataGridViewTextBoxColumn 审核时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 称重信息;
        private System.Windows.Forms.DataGridViewTextBoxColumn 称重时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 气密性信息;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检测时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 终检人;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Button btnOutPut;

    }
}