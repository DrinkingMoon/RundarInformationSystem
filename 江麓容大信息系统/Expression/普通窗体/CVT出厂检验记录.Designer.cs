namespace Expression
{
    partial class CVT出厂检验记录
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnOutExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBill_ID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.rbFinalNo = new System.Windows.Forms.RadioButton();
            this.rbFinalYes = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpFinalDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.lbFinalPersonnel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpCheck = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.lbPersonnel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.检测项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.技术要求 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.判定 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.不合格情况 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnOutExcel,
            this.toolStripSeparatorAdd,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(961, 25);
            this.toolStrip1.TabIndex = 46;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnOutExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(91, 22);
            this.btnOutExcel.Tag = "view";
            this.btnOutExcel.Text = "下载文件(&O)";
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(67, 22);
            this.btnClose.Tag = "view";
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProductType);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtProductCode);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtBill_ID);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(961, 51);
            this.groupBox1.TabIndex = 47;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "编号信息";
            // 
            // txtProductType
            // 
            this.txtProductType.Enabled = false;
            this.txtProductType.Location = new System.Drawing.Point(429, 20);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(150, 21);
            this.txtProductType.TabIndex = 227;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(691, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 219;
            this.label7.Text = "产品编码：";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Enabled = false;
            this.txtProductCode.Location = new System.Drawing.Point(765, 20);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(150, 21);
            this.txtProductCode.TabIndex = 218;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(343, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 216;
            this.label13.Text = "所属总成型号：";
            // 
            // txtBill_ID
            // 
            this.txtBill_ID.BackColor = System.Drawing.Color.White;
            this.txtBill_ID.ForeColor = System.Drawing.Color.Red;
            this.txtBill_ID.Location = new System.Drawing.Point(100, 20);
            this.txtBill_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBill_ID.Name = "txtBill_ID";
            this.txtBill_ID.ReadOnly = true;
            this.txtBill_ID.Size = new System.Drawing.Size(157, 21);
            this.txtBill_ID.TabIndex = 159;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 160;
            this.label6.Text = "单据号：";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.groupBox4);
            this.groupBox3.Controls.Add(this.dtpFinalDate);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.lbFinalPersonnel);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.dtpCheck);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.lbPersonnel);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.txtRemark);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.rbNo);
            this.groupBox3.Controls.Add(this.rbYes);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.groupBox3.Location = new System.Drawing.Point(0, 655);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(961, 143);
            this.groupBox3.TabIndex = 49;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "检测结果";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.rbFinalNo);
            this.groupBox4.Controls.Add(this.rbFinalYes);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Location = new System.Drawing.Point(412, 11);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(503, 36);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            // 
            // rbFinalNo
            // 
            this.rbFinalNo.AutoSize = true;
            this.rbFinalNo.Enabled = false;
            this.rbFinalNo.Location = new System.Drawing.Point(320, 16);
            this.rbFinalNo.Name = "rbFinalNo";
            this.rbFinalNo.Size = new System.Drawing.Size(59, 16);
            this.rbFinalNo.TabIndex = 14;
            this.rbFinalNo.TabStop = true;
            this.rbFinalNo.Text = "不合格";
            this.rbFinalNo.UseVisualStyleBackColor = true;
            // 
            // rbFinalYes
            // 
            this.rbFinalYes.AutoSize = true;
            this.rbFinalYes.Enabled = false;
            this.rbFinalYes.Location = new System.Drawing.Point(196, 16);
            this.rbFinalYes.Name = "rbFinalYes";
            this.rbFinalYes.Size = new System.Drawing.Size(47, 16);
            this.rbFinalYes.TabIndex = 13;
            this.rbFinalYes.TabStop = true;
            this.rbFinalYes.Text = "合格";
            this.rbFinalYes.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(104, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 12;
            this.label4.Text = "最终判定：";
            // 
            // dtpFinalDate
            // 
            this.dtpFinalDate.Location = new System.Drawing.Point(765, 112);
            this.dtpFinalDate.Name = "dtpFinalDate";
            this.dtpFinalDate.Size = new System.Drawing.Size(150, 21);
            this.dtpFinalDate.TabIndex = 15;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(699, 116);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "判定时间：";
            // 
            // lbFinalPersonnel
            // 
            this.lbFinalPersonnel.AutoSize = true;
            this.lbFinalPersonnel.Location = new System.Drawing.Point(604, 116);
            this.lbFinalPersonnel.Name = "lbFinalPersonnel";
            this.lbFinalPersonnel.Size = new System.Drawing.Size(41, 12);
            this.lbFinalPersonnel.TabIndex = 13;
            this.lbFinalPersonnel.Text = "判定人";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(527, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 12;
            this.label9.Text = "判定人：";
            // 
            // dtpCheck
            // 
            this.dtpCheck.Location = new System.Drawing.Point(299, 112);
            this.dtpCheck.Name = "dtpCheck";
            this.dtpCheck.Size = new System.Drawing.Size(157, 21);
            this.dtpCheck.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(233, 116);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 7;
            this.label5.Text = "检验时间：";
            // 
            // lbPersonnel
            // 
            this.lbPersonnel.AutoSize = true;
            this.lbPersonnel.Location = new System.Drawing.Point(103, 116);
            this.lbPersonnel.Name = "lbPersonnel";
            this.lbPersonnel.Size = new System.Drawing.Size(65, 12);
            this.lbPersonnel.TabIndex = 6;
            this.lbPersonnel.Text = "检验员名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "检验员：";
            // 
            // txtRemark
            // 
            this.txtRemark.Enabled = false;
            this.txtRemark.Location = new System.Drawing.Point(103, 55);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(812, 41);
            this.txtRemark.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 3;
            this.label2.Text = "备  注：";
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Enabled = false;
            this.rbNo.Location = new System.Drawing.Point(235, 27);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(59, 16);
            this.rbNo.TabIndex = 2;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "不合格";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Enabled = false;
            this.rbYes.Location = new System.Drawing.Point(111, 27);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(47, 16);
            this.rbYes.TabIndex = 1;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "合格";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "结  论：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 76);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(961, 579);
            this.groupBox2.TabIndex = 50;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "检测项目";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.检测项目,
            this.技术要求,
            this.判定,
            this.不合格情况,
            this.单据号,
            this.序号});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(955, 559);
            this.dataGridView1.TabIndex = 1;
            // 
            // 检测项目
            // 
            this.检测项目.DataPropertyName = "检测项目";
            this.检测项目.HeaderText = "检测项目";
            this.检测项目.Name = "检测项目";
            this.检测项目.ReadOnly = true;
            this.检测项目.Width = 300;
            // 
            // 技术要求
            // 
            this.技术要求.DataPropertyName = "技术要求";
            this.技术要求.HeaderText = "技术要求";
            this.技术要求.Name = "技术要求";
            this.技术要求.ReadOnly = true;
            this.技术要求.Width = 300;
            // 
            // 判定
            // 
            this.判定.DataPropertyName = "判定";
            this.判定.HeaderText = "判定";
            this.判定.Name = "判定";
            this.判定.ReadOnly = true;
            this.判定.Width = 60;
            // 
            // 不合格情况
            // 
            this.不合格情况.DataPropertyName = "不合格情况";
            this.不合格情况.HeaderText = "不合格情况";
            this.不合格情况.Name = "不合格情况";
            this.不合格情况.ReadOnly = true;
            this.不合格情况.Width = 200;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.ReadOnly = true;
            this.单据号.Visible = false;
            // 
            // 序号
            // 
            this.序号.DataPropertyName = "序号";
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.ReadOnly = true;
            this.序号.Visible = false;
            // 
            // CVT出厂检验记录
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 798);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "CVT出厂检验记录";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CVT出厂检验记录表";
            this.Load += new System.EventHandler(this.CVT出厂检验记录_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnOutExcel;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYes;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbPersonnel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpCheck;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBill_ID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.Label lbFinalPersonnel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpFinalDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbFinalNo;
        private System.Windows.Forms.RadioButton rbFinalYes;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.CustomDataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检测项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 技术要求;
        private System.Windows.Forms.DataGridViewTextBoxColumn 判定;
        private System.Windows.Forms.DataGridViewTextBoxColumn 不合格情况;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
    }
}