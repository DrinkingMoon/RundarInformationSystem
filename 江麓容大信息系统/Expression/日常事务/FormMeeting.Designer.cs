using UniversalControlLibrary;
namespace Expression
{
    partial class FormMeeting
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBillNo = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblBillStatus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.提交 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.撤销 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdImportance = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmd与会人员 = new System.Windows.Forms.ComboBox();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numMinute = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.radio消息框 = new System.Windows.Forms.RadioButton();
            this.radio短信 = new System.Windows.Forms.RadioButton();
            this.radio短信及消息框 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpkBeginTime = new System.Windows.Forms.DateTimePicker();
            this.dtpkEndTime = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.cmd与会资源 = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txt记录员 = new UniversalControlLibrary.TextBoxShow();
            this.txt主持人 = new UniversalControlLibrary.TextBoxShow();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinute)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "会议名称";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(99, 95);
            this.txtTitle.MaxLength = 20;
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(427, 23);
            this.txtTitle.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.lblBillNo);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.lblBillStatus);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(904, 50);
            this.panel1.TabIndex = 26;
            // 
            // lblBillNo
            // 
            this.lblBillNo.AutoSize = true;
            this.lblBillNo.Location = new System.Drawing.Point(111, 8);
            this.lblBillNo.Name = "lblBillNo";
            this.lblBillNo.Size = new System.Drawing.Size(0, 14);
            this.lblBillNo.TabIndex = 196;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(29, 7);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 14);
            this.label10.TabIndex = 194;
            this.label10.Text = "会议编号：";
            // 
            // lblBillStatus
            // 
            this.lblBillStatus.AutoSize = true;
            this.lblBillStatus.Location = new System.Drawing.Point(111, 27);
            this.lblBillStatus.Name = "lblBillStatus";
            this.lblBillStatus.Size = new System.Drawing.Size(0, 14);
            this.lblBillStatus.TabIndex = 193;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(29, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 14);
            this.label11.TabIndex = 192;
            this.label11.Text = "事务状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(392, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "日常会议";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnSave,
            this.toolStripSeparator1,
            this.提交,
            this.toolStripSeparator2,
            this.撤销,
            this.toolStripSeparator3,
            this.btnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(904, 25);
            this.toolStrip.TabIndex = 9;
            this.toolStrip.TabStop = true;
            // 
            // btnNew
            // 
            this.btnNew.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(52, 22);
            this.btnNew.Tag = "Add";
            this.btnNew.Text = "新建";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 22);
            this.btnSave.Tag = "Add";
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 提交
            // 
            this.提交.Image = global::UniversalControlLibrary.Properties.Resources.提交;
            this.提交.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.提交.Name = "提交";
            this.提交.Size = new System.Drawing.Size(52, 22);
            this.提交.Text = "发布";
            this.提交.Click += new System.EventHandler(this.提交_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 撤销
            // 
            this.撤销.Image = global::UniversalControlLibrary.Properties.Resources.cancle;
            this.撤销.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.撤销.Name = "撤销";
            this.撤销.Size = new System.Drawing.Size(52, 22);
            this.撤销.Tag = "add";
            this.撤销.Text = "撤销";
            this.撤销.Click += new System.EventHandler(this.撤销_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(52, 22);
            this.btnClose.Tag = "";
            this.btnClose.Text = "退出";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 254);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "重 要 性";
            // 
            // cmdImportance
            // 
            this.cmdImportance.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmdImportance.FormattingEnabled = true;
            this.cmdImportance.Location = new System.Drawing.Point(98, 250);
            this.cmdImportance.Name = "cmdImportance";
            this.cmdImportance.Size = new System.Drawing.Size(171, 22);
            this.cmdImportance.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(29, 137);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 29;
            this.label3.Text = "主持人员";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(29, 176);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 31;
            this.label4.Text = "与会人员";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(286, 137);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 33;
            this.label5.Text = "记录人员";
            // 
            // cmd与会人员
            // 
            this.cmd与会人员.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmd与会人员.FormattingEnabled = true;
            this.cmd与会人员.Items.AddRange(new object[] {
            "",
            "新建"});
            this.cmd与会人员.Location = new System.Drawing.Point(98, 172);
            this.cmd与会人员.Name = "cmd与会人员";
            this.cmd与会人员.Size = new System.Drawing.Size(428, 22);
            this.cmd与会人员.TabIndex = 5;
            this.cmd与会人员.SelectedIndexChanged += new System.EventHandler(this.cmd与会人员_SelectedIndexChanged);
            this.cmd与会人员.MouseEnter += new System.EventHandler(this.cmd与会人员_MouseEnter);
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.Location = new System.Drawing.Point(99, 287);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.Size = new System.Drawing.Size(793, 152);
            this.txtContent.TabIndex = 8;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.numMinute);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.radio消息框);
            this.groupBox2.Controls.Add(this.radio短信);
            this.groupBox2.Controls.Add(this.radio短信及消息框);
            this.groupBox2.Location = new System.Drawing.Point(563, 168);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(329, 100);
            this.groupBox2.TabIndex = 38;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "提醒方式";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(278, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(35, 14);
            this.label8.TabIndex = 30;
            this.label8.Text = "分钟";
            // 
            // numMinute
            // 
            this.numMinute.Location = new System.Drawing.Point(211, 20);
            this.numMinute.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMinute.Minimum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.numMinute.Name = "numMinute";
            this.numMinute.Size = new System.Drawing.Size(61, 23);
            this.numMinute.TabIndex = 3;
            this.numMinute.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(169, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(35, 14);
            this.label9.TabIndex = 28;
            this.label9.Text = "提前";
            // 
            // radio消息框
            // 
            this.radio消息框.AutoSize = true;
            this.radio消息框.Location = new System.Drawing.Point(15, 70);
            this.radio消息框.Name = "radio消息框";
            this.radio消息框.Size = new System.Drawing.Size(109, 18);
            this.radio消息框.TabIndex = 2;
            this.radio消息框.TabStop = true;
            this.radio消息框.Text = "仅消息框提醒";
            this.radio消息框.UseVisualStyleBackColor = true;
            // 
            // radio短信
            // 
            this.radio短信.AutoSize = true;
            this.radio短信.Location = new System.Drawing.Point(15, 46);
            this.radio短信.Name = "radio短信";
            this.radio短信.Size = new System.Drawing.Size(95, 18);
            this.radio短信.TabIndex = 1;
            this.radio短信.TabStop = true;
            this.radio短信.Text = "仅短信提醒";
            this.radio短信.UseVisualStyleBackColor = true;
            // 
            // radio短信及消息框
            // 
            this.radio短信及消息框.AutoSize = true;
            this.radio短信及消息框.Checked = true;
            this.radio短信及消息框.Location = new System.Drawing.Point(15, 22);
            this.radio短信及消息框.Name = "radio短信及消息框";
            this.radio短信及消息框.Size = new System.Drawing.Size(137, 18);
            this.radio短信及消息框.TabIndex = 0;
            this.radio短信及消息框.TabStop = true;
            this.radio短信及消息框.Text = "短信及消息框提醒";
            this.radio短信及消息框.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(560, 99);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 40;
            this.label6.Text = "开始时间";
            // 
            // dtpkBeginTime
            // 
            this.dtpkBeginTime.Checked = false;
            this.dtpkBeginTime.CustomFormat = "yyyy年MM月dd日 HH:mm";
            this.dtpkBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpkBeginTime.Location = new System.Drawing.Point(635, 94);
            this.dtpkBeginTime.Name = "dtpkBeginTime";
            this.dtpkBeginTime.ShowCheckBox = true;
            this.dtpkBeginTime.Size = new System.Drawing.Size(257, 23);
            this.dtpkBeginTime.TabIndex = 3;
            this.dtpkBeginTime.ValueChanged += new System.EventHandler(this.dtpkBeginTime_ValueChanged);
            // 
            // dtpkEndTime
            // 
            this.dtpkEndTime.Checked = false;
            this.dtpkEndTime.CustomFormat = "yyyy年MM月dd日 HH:mm";
            this.dtpkEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpkEndTime.Location = new System.Drawing.Point(635, 133);
            this.dtpkEndTime.Name = "dtpkEndTime";
            this.dtpkEndTime.ShowCheckBox = true;
            this.dtpkEndTime.Size = new System.Drawing.Size(257, 23);
            this.dtpkEndTime.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(560, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 42;
            this.label7.Text = "结束时间";
            // 
            // cmd与会资源
            // 
            this.cmd与会资源.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmd与会资源.FormattingEnabled = true;
            this.cmd与会资源.Items.AddRange(new object[] {
            "",
            "新建"});
            this.cmd与会资源.Location = new System.Drawing.Point(98, 211);
            this.cmd与会资源.Name = "cmd与会资源";
            this.cmd与会资源.Size = new System.Drawing.Size(428, 22);
            this.cmd与会资源.TabIndex = 6;
            this.cmd与会资源.SelectedIndexChanged += new System.EventHandler(this.cmd与会资源_SelectedIndexChanged);
            this.cmd与会资源.MouseEnter += new System.EventHandler(this.cmd与会资源_MouseEnter);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(29, 215);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 44;
            this.label12.Text = "与会资源";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(29, 290);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 14);
            this.label13.TabIndex = 46;
            this.label13.Text = "会议正文";
            // 
            // txt记录员
            // 
            this.txt记录员.EditingControlDataGridView = null;
            this.txt记录员.EditingControlFormattedValue = "";
            this.txt记录员.EditingControlRowIndex = 0;
            this.txt记录员.EditingControlValueChanged = false;
            this.txt记录员.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txt记录员.Location = new System.Drawing.Point(355, 133);
            this.txt记录员.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt记录员.Name = "txt记录员";
            this.txt记录员.ShowResultForm = true;
            this.txt记录员.Size = new System.Drawing.Size(171, 23);
            this.txt记录员.TabIndex = 2;
            this.txt记录员.TabStop = false;
            // 
            // txt主持人
            // 
            this.txt主持人.EditingControlDataGridView = null;
            this.txt主持人.EditingControlFormattedValue = "";
            this.txt主持人.EditingControlRowIndex = 0;
            this.txt主持人.EditingControlValueChanged = false;
            this.txt主持人.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txt主持人.Location = new System.Drawing.Point(98, 133);
            this.txt主持人.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txt主持人.Name = "txt主持人";
            this.txt主持人.ShowResultForm = true;
            this.txt主持人.Size = new System.Drawing.Size(171, 23);
            this.txt主持人.TabIndex = 1;
            this.txt主持人.TabStop = false;
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // FormMeeting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 451);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.cmd与会资源);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dtpkEndTime);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.dtpkBeginTime);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtContent);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.cmd与会人员);
            this.Controls.Add(this.txt记录员);
            this.Controls.Add(this.txt主持人);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdImportance);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.txtTitle);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMeeting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "会议信息";
            this.Resize += new System.EventHandler(this.FormMeeting_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinute)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBillNo;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblBillStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 提交;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 撤销;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmdImportance;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private TextBoxShow txt主持人;
        private TextBoxShow txt记录员;
        private System.Windows.Forms.ComboBox cmd与会人员;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numMinute;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.RadioButton radio消息框;
        private System.Windows.Forms.RadioButton radio短信及消息框;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpkBeginTime;
        private System.Windows.Forms.DateTimePicker dtpkEndTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmd与会资源;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.RadioButton radio短信;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}