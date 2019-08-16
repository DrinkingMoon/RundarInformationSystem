namespace UniversalControlLibrary
{
    partial class CheckBillDateAndStatus
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbMultiple = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.btnFind = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.cmbBillStatus = new System.Windows.Forms.ComboBox();
            this.label22 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label21 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbMultiple);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.label25);
            this.groupBox1.Controls.Add(this.cmbBillStatus);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Controls.Add(this.label21);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.dtpStartTime);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(888, 46);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据筛选";
            // 
            // rbMultiple
            // 
            this.rbMultiple.AutoSize = true;
            this.rbMultiple.Location = new System.Drawing.Point(501, 18);
            this.rbMultiple.Name = "rbMultiple";
            this.rbMultiple.Size = new System.Drawing.Size(47, 16);
            this.rbMultiple.TabIndex = 193;
            this.rbMultiple.Text = "多选";
            this.rbMultiple.UseVisualStyleBackColor = true;
            this.rbMultiple.Click += new System.EventHandler(this.RadioButton_Click);
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.Location = new System.Drawing.Point(553, 18);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 192;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "单选";
            this.radioButton1.UseVisualStyleBackColor = true;
            this.radioButton1.Click += new System.EventHandler(this.RadioButton_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(791, 15);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 190;
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(616, 20);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(29, 12);
            this.label25.TabIndex = 189;
            this.label25.Text = "状态";
            // 
            // cmbBillStatus
            // 
            this.cmbBillStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBillStatus.FormattingEnabled = true;
            this.cmbBillStatus.Location = new System.Drawing.Point(651, 16);
            this.cmbBillStatus.Name = "cmbBillStatus";
            this.cmbBillStatus.Size = new System.Drawing.Size(121, 20);
            this.cmbBillStatus.TabIndex = 188;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(255, 20);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(53, 12);
            this.label22.TabIndex = 187;
            this.label22.Text = "截止日期";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(314, 16);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(158, 21);
            this.dtpEndTime.TabIndex = 186;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(232, 20);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(17, 12);
            this.label21.TabIndex = 185;
            this.label21.Text = "到";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 20);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 184;
            this.label19.Text = "起始日期";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(70, 16);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(153, 21);
            this.dtpStartTime.TabIndex = 183;
            // 
            // CheckBillDateAndStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Name = "CheckBillDateAndStatus";
            this.Size = new System.Drawing.Size(888, 46);
            this.Load += new System.EventHandler(this.CheckBillDateAndStatus_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label25;
        public System.Windows.Forms.ComboBox cmbBillStatus;
        private System.Windows.Forms.Label label22;
        public System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label19;
        public System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.RadioButton rbMultiple;
        private System.Windows.Forms.RadioButton radioButton1;
    }
}
