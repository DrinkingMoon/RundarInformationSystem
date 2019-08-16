namespace Expression
{
    partial class 零星采购申请统计报表
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.numMonth = new System.Windows.Forms.NumericUpDown();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numEndMonth = new System.Windows.Forms.NumericUpDown();
            this.numEndYear = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbsDeptCode = new UniversalControlLibrary.TextBoxShow();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndYear)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(607, 43);
            this.panel1.TabIndex = 45;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(162, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(282, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "零星采购申请统计报表";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 43);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(607, 527);
            this.panel2.TabIndex = 46;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 123);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(607, 404);
            this.dataGridView1.TabIndex = 8;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 91);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(607, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 7;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(607, 91);
            this.panel3.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbsDeptCode);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.numEndMonth);
            this.groupBox1.Controls.Add(this.numEndYear);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numMonth);
            this.groupBox1.Controls.Add(this.numYear);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(607, 91);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据筛选";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(463, 34);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(75, 23);
            this.btnQuery.TabIndex = 7;
            this.btnQuery.Text = "查  询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(263, 38);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "部  门";
            // 
            // numMonth
            // 
            this.numMonth.Location = new System.Drawing.Point(184, 21);
            this.numMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numMonth.Name = "numMonth";
            this.numMonth.Size = new System.Drawing.Size(50, 23);
            this.numMonth.TabIndex = 3;
            this.numMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(84, 21);
            this.numYear.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(67, 23);
            this.numYear.TabIndex = 2;
            this.numYear.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(240, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "月";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(157, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "年";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "开始日期";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 54);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "截止日期";
            // 
            // numEndMonth
            // 
            this.numEndMonth.Location = new System.Drawing.Point(184, 50);
            this.numEndMonth.Maximum = new decimal(new int[] {
            12,
            0,
            0,
            0});
            this.numEndMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEndMonth.Name = "numEndMonth";
            this.numEndMonth.Size = new System.Drawing.Size(50, 23);
            this.numEndMonth.TabIndex = 12;
            this.numEndMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numEndYear
            // 
            this.numEndYear.Location = new System.Drawing.Point(84, 50);
            this.numEndYear.Maximum = new decimal(new int[] {
            20000,
            0,
            0,
            0});
            this.numEndYear.Minimum = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            this.numEndYear.Name = "numEndYear";
            this.numEndYear.Size = new System.Drawing.Size(67, 23);
            this.numEndYear.TabIndex = 11;
            this.numEndYear.Value = new decimal(new int[] {
            2014,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(240, 54);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 14);
            this.label6.TabIndex = 10;
            this.label6.Text = "月";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(157, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(21, 14);
            this.label7.TabIndex = 9;
            this.label7.Text = "年";
            // 
            // tbsDeptCode
            // 
            this.tbsDeptCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.部门;
            this.tbsDeptCode.Location = new System.Drawing.Point(318, 35);
            this.tbsDeptCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbsDeptCode.Name = "tbsDeptCode";
            this.tbsDeptCode.Size = new System.Drawing.Size(126, 23);
            this.tbsDeptCode.TabIndex = 123;
            this.tbsDeptCode.Text = " ";
            // 
            // 零星采购申请统计报表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 570);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "零星采购申请统计报表";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "零星采购申请统计报表";
            this.Resize += new System.EventHandler(this.零星采购申请统计报表_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndYear)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMonth;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numEndMonth;
        private System.Windows.Forms.NumericUpDown numEndYear;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private UniversalControlLibrary.TextBoxShow tbsDeptCode;
    }
}