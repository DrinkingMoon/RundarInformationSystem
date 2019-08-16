namespace Form_Peripheral_HR
{
    partial class 异常单据业务操作
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.num_Hours = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rb_Operation_Delete = new System.Windows.Forms.RadioButton();
            this.rb_Operation_Modify = new System.Windows.Forms.RadioButton();
            this.rb_Operation_Add = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rb_BillType_OverTime = new System.Windows.Forms.RadioButton();
            this.rb_BillType_Leave = new System.Windows.Forms.RadioButton();
            this.btSubmit = new System.Windows.Forms.Button();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtp_EndTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_BeginTime = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_BusinessType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Hours)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 61);
            this.panel1.TabIndex = 96;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(390, 17);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "异常单据业务操作";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.num_Hours);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.panel3);
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.btSubmit);
            this.groupBox1.Controls.Add(this.txtContent);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtp_EndTime);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtp_BeginTime);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmb_BusinessType);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1008, 145);
            this.groupBox1.TabIndex = 97;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据信息";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(862, 68);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 20;
            this.label8.Text = "小时";
            // 
            // num_Hours
            // 
            this.num_Hours.DecimalPlaces = 1;
            this.num_Hours.Location = new System.Drawing.Point(771, 64);
            this.num_Hours.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.num_Hours.Name = "num_Hours";
            this.num_Hours.Size = new System.Drawing.Size(85, 21);
            this.num_Hours.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(697, 68);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 18;
            this.label7.Text = "小 时 数";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rb_Operation_Delete);
            this.panel3.Controls.Add(this.rb_Operation_Modify);
            this.panel3.Controls.Add(this.rb_Operation_Add);
            this.panel3.Location = new System.Drawing.Point(537, 15);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(200, 34);
            this.panel3.TabIndex = 17;
            // 
            // rb_Operation_Delete
            // 
            this.rb_Operation_Delete.AutoSize = true;
            this.rb_Operation_Delete.Location = new System.Drawing.Point(149, 9);
            this.rb_Operation_Delete.Name = "rb_Operation_Delete";
            this.rb_Operation_Delete.Size = new System.Drawing.Size(47, 16);
            this.rb_Operation_Delete.TabIndex = 6;
            this.rb_Operation_Delete.TabStop = true;
            this.rb_Operation_Delete.Tag = "删除";
            this.rb_Operation_Delete.Text = "删单";
            this.rb_Operation_Delete.UseVisualStyleBackColor = true;
            // 
            // rb_Operation_Modify
            // 
            this.rb_Operation_Modify.AutoSize = true;
            this.rb_Operation_Modify.Location = new System.Drawing.Point(76, 9);
            this.rb_Operation_Modify.Name = "rb_Operation_Modify";
            this.rb_Operation_Modify.Size = new System.Drawing.Size(47, 16);
            this.rb_Operation_Modify.TabIndex = 5;
            this.rb_Operation_Modify.TabStop = true;
            this.rb_Operation_Modify.Tag = "修改";
            this.rb_Operation_Modify.Text = "改单";
            this.rb_Operation_Modify.UseVisualStyleBackColor = true;
            // 
            // rb_Operation_Add
            // 
            this.rb_Operation_Add.AutoSize = true;
            this.rb_Operation_Add.Location = new System.Drawing.Point(3, 9);
            this.rb_Operation_Add.Name = "rb_Operation_Add";
            this.rb_Operation_Add.Size = new System.Drawing.Size(47, 16);
            this.rb_Operation_Add.TabIndex = 4;
            this.rb_Operation_Add.TabStop = true;
            this.rb_Operation_Add.Tag = "添加";
            this.rb_Operation_Add.Text = "补单";
            this.rb_Operation_Add.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rb_BillType_OverTime);
            this.panel2.Controls.Add(this.rb_BillType_Leave);
            this.panel2.Location = new System.Drawing.Point(206, 15);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 34);
            this.panel2.TabIndex = 16;
            // 
            // rb_BillType_OverTime
            // 
            this.rb_BillType_OverTime.AutoSize = true;
            this.rb_BillType_OverTime.Location = new System.Drawing.Point(121, 9);
            this.rb_BillType_OverTime.Name = "rb_BillType_OverTime";
            this.rb_BillType_OverTime.Size = new System.Drawing.Size(59, 16);
            this.rb_BillType_OverTime.TabIndex = 8;
            this.rb_BillType_OverTime.TabStop = true;
            this.rb_BillType_OverTime.Tag = "加班";
            this.rb_BillType_OverTime.Text = "加班单";
            this.rb_BillType_OverTime.UseVisualStyleBackColor = true;
            // 
            // rb_BillType_Leave
            // 
            this.rb_BillType_Leave.AutoSize = true;
            this.rb_BillType_Leave.Location = new System.Drawing.Point(20, 9);
            this.rb_BillType_Leave.Name = "rb_BillType_Leave";
            this.rb_BillType_Leave.Size = new System.Drawing.Size(59, 16);
            this.rb_BillType_Leave.TabIndex = 7;
            this.rb_BillType_Leave.TabStop = true;
            this.rb_BillType_Leave.Tag = "请假";
            this.rb_BillType_Leave.Text = "请假单";
            this.rb_BillType_Leave.UseVisualStyleBackColor = true;
            // 
            // btSubmit
            // 
            this.btSubmit.Location = new System.Drawing.Point(770, 21);
            this.btSubmit.Name = "btSubmit";
            this.btSubmit.Size = new System.Drawing.Size(121, 23);
            this.btSubmit.TabIndex = 15;
            this.btSubmit.Text = "提交";
            this.btSubmit.UseVisualStyleBackColor = true;
            this.btSubmit.Click += new System.EventHandler(this.btSubmit_Click);
            // 
            // txtContent
            // 
            this.txtContent.Location = new System.Drawing.Point(537, 98);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtContent.Size = new System.Drawing.Size(354, 34);
            this.txtContent.TabIndex = 14;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(460, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "执行内容";
            // 
            // dtp_EndTime
            // 
            this.dtp_EndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_EndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_EndTime.Location = new System.Drawing.Point(206, 105);
            this.dtp_EndTime.Name = "dtp_EndTime";
            this.dtp_EndTime.Size = new System.Drawing.Size(200, 21);
            this.dtp_EndTime.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(118, 109);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 11;
            this.label5.Text = "执行结束时间";
            // 
            // dtp_BeginTime
            // 
            this.dtp_BeginTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtp_BeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_BeginTime.Location = new System.Drawing.Point(206, 64);
            this.dtp_BeginTime.Name = "dtp_BeginTime";
            this.dtp_BeginTime.Size = new System.Drawing.Size(200, 21);
            this.dtp_BeginTime.TabIndex = 10;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(118, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 9;
            this.label4.Text = "执行开始时间";
            // 
            // cmb_BusinessType
            // 
            this.cmb_BusinessType.FormattingEnabled = true;
            this.cmb_BusinessType.Location = new System.Drawing.Point(537, 64);
            this.cmb_BusinessType.Name = "cmb_BusinessType";
            this.cmb_BusinessType.Size = new System.Drawing.Size(121, 20);
            this.cmb_BusinessType.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(460, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "业务类型";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(118, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "单据类型";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(460, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "操作类型";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 238);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(1008, 197);
            this.customDataGridView1.TabIndex = 99;
            this.customDataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 206);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1008, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 98;
            // 
            // 异常单据业务操作
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 435);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "异常单据业务操作";
            this.Text = "异常单据业务操作";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_Hours)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtp_EndTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp_BeginTime;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_BusinessType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btSubmit;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rb_Operation_Delete;
        private System.Windows.Forms.RadioButton rb_Operation_Modify;
        private System.Windows.Forms.RadioButton rb_Operation_Add;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton rb_BillType_OverTime;
        private System.Windows.Forms.RadioButton rb_BillType_Leave;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown num_Hours;
        private System.Windows.Forms.Label label7;
    }
}