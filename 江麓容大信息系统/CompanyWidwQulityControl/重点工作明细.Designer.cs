namespace Form_Peripheral_CompanyQuality
{
    partial class 重点工作明细
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbMonth = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYear = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.gbTaskName = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnKeyPoint = new System.Windows.Forms.Button();
            this.txtDutyUser = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.txtExpectedGoal = new UniversalControlLibrary.TextBoxShow();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTaskDescription = new UniversalControlLibrary.TextBoxShow();
            this.label3 = new System.Windows.Forms.Label();
            this.customGroupBox3 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnSetKeyPoint = new System.Windows.Forms.Button();
            this.txtNextPlan = new UniversalControlLibrary.TextBoxShow();
            this.label11 = new System.Windows.Forms.Label();
            this.rbDelay = new System.Windows.Forms.RadioButton();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.txtProgressContent = new UniversalControlLibrary.TextBoxShow();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.gbTaskName.SuspendLayout();
            this.customGroupBox2.SuspendLayout();
            this.customGroupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbMonth);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.cmbYear);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtBillNo);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(873, 44);
            this.panel1.TabIndex = 0;
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
            this.cmbMonth.Location = new System.Drawing.Point(586, 12);
            this.cmbMonth.MaxYear = 0;
            this.cmbMonth.MinYear = 0;
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(121, 20);
            this.cmbMonth.TabIndex = 362;
            this.cmbMonth.SelectedIndexChanged += new System.EventHandler(this.cmbYearMonth_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(539, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 361;
            this.label2.Text = "月份：";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(386, 12);
            this.cmbYear.MaxYear = 0;
            this.cmbYear.MinYear = 0;
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(121, 20);
            this.cmbYear.TabIndex = 360;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYearMonth_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(339, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 359;
            this.label1.Text = "年份：";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(100, 12);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(202, 21);
            this.txtBillNo.TabIndex = 357;
            this.txtBillNo.Text = "CMB201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(25, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 358;
            this.label4.Text = "业务编号：";
            // 
            // gbTaskName
            // 
            this.gbTaskName.Controls.Add(this.flowLayoutPanel1);
            this.gbTaskName.Dock = System.Windows.Forms.DockStyle.Top;
            this.gbTaskName.Location = new System.Drawing.Point(0, 44);
            this.gbTaskName.Name = "gbTaskName";
            this.gbTaskName.Size = new System.Drawing.Size(873, 107);
            this.gbTaskName.TabIndex = 1;
            this.gbTaskName.TabStop = false;
            this.gbTaskName.Text = "重点工作";
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 17);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(867, 87);
            this.flowLayoutPanel1.TabIndex = 1;
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.btnKeyPoint);
            this.customGroupBox2.Controls.Add(this.txtDutyUser);
            this.customGroupBox2.Controls.Add(this.label8);
            this.customGroupBox2.Controls.Add(this.label7);
            this.customGroupBox2.Controls.Add(this.dtpEndDate);
            this.customGroupBox2.Controls.Add(this.label6);
            this.customGroupBox2.Controls.Add(this.dtpStartDate);
            this.customGroupBox2.Controls.Add(this.txtExpectedGoal);
            this.customGroupBox2.Controls.Add(this.label5);
            this.customGroupBox2.Controls.Add(this.txtTaskDescription);
            this.customGroupBox2.Controls.Add(this.label3);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 151);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(873, 183);
            this.customGroupBox2.TabIndex = 2;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "重点工作内容";
            // 
            // btnKeyPoint
            // 
            this.btnKeyPoint.Location = new System.Drawing.Point(719, 151);
            this.btnKeyPoint.Name = "btnKeyPoint";
            this.btnKeyPoint.Size = new System.Drawing.Size(142, 23);
            this.btnKeyPoint.TabIndex = 369;
            this.btnKeyPoint.Text = "查看关键节点";
            this.btnKeyPoint.UseVisualStyleBackColor = true;
            this.btnKeyPoint.Click += new System.EventHandler(this.btnKeyPoint_Click);
            // 
            // txtDutyUser
            // 
            this.txtDutyUser.Location = new System.Drawing.Point(556, 151);
            this.txtDutyUser.Name = "txtDutyUser";
            this.txtDutyUser.ReadOnly = true;
            this.txtDutyUser.Size = new System.Drawing.Size(121, 21);
            this.txtDutyUser.TabIndex = 368;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(497, 155);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 367;
            this.label8.Text = "责任人：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(258, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 366;
            this.label7.Text = "完成时间：";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Enabled = false;
            this.dtpEndDate.Location = new System.Drawing.Point(329, 152);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(139, 21);
            this.dtpEndDate.TabIndex = 365;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(25, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 364;
            this.label6.Text = "启动时间：";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Enabled = false;
            this.dtpStartDate.Location = new System.Drawing.Point(100, 152);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(139, 21);
            this.dtpStartDate.TabIndex = 363;
            // 
            // txtExpectedGoal
            // 
            this.txtExpectedGoal.DataResult = null;
            this.txtExpectedGoal.DataTableResult = null;
            this.txtExpectedGoal.EditingControlDataGridView = null;
            this.txtExpectedGoal.EditingControlFormattedValue = "";
            this.txtExpectedGoal.EditingControlRowIndex = 0;
            this.txtExpectedGoal.EditingControlValueChanged = true;
            this.txtExpectedGoal.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtExpectedGoal.IsMultiSelect = false;
            this.txtExpectedGoal.Location = new System.Drawing.Point(513, 18);
            this.txtExpectedGoal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExpectedGoal.Multiline = true;
            this.txtExpectedGoal.Name = "txtExpectedGoal";
            this.txtExpectedGoal.ReadOnly = true;
            this.txtExpectedGoal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtExpectedGoal.ShowResultForm = false;
            this.txtExpectedGoal.Size = new System.Drawing.Size(348, 123);
            this.txtExpectedGoal.StrEndSql = null;
            this.txtExpectedGoal.TabIndex = 362;
            this.txtExpectedGoal.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(442, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 361;
            this.label5.Text = "预期目标：";
            // 
            // txtTaskDescription
            // 
            this.txtTaskDescription.DataResult = null;
            this.txtTaskDescription.DataTableResult = null;
            this.txtTaskDescription.EditingControlDataGridView = null;
            this.txtTaskDescription.EditingControlFormattedValue = "";
            this.txtTaskDescription.EditingControlRowIndex = 0;
            this.txtTaskDescription.EditingControlValueChanged = true;
            this.txtTaskDescription.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtTaskDescription.IsMultiSelect = false;
            this.txtTaskDescription.Location = new System.Drawing.Point(100, 18);
            this.txtTaskDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTaskDescription.Multiline = true;
            this.txtTaskDescription.Name = "txtTaskDescription";
            this.txtTaskDescription.ReadOnly = true;
            this.txtTaskDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTaskDescription.ShowResultForm = false;
            this.txtTaskDescription.Size = new System.Drawing.Size(324, 123);
            this.txtTaskDescription.StrEndSql = null;
            this.txtTaskDescription.TabIndex = 360;
            this.txtTaskDescription.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(25, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 359;
            this.label3.Text = "工作描述：";
            // 
            // customGroupBox3
            // 
            this.customGroupBox3.Controls.Add(this.btnSetKeyPoint);
            this.customGroupBox3.Controls.Add(this.txtNextPlan);
            this.customGroupBox3.Controls.Add(this.label11);
            this.customGroupBox3.Controls.Add(this.rbDelay);
            this.customGroupBox3.Controls.Add(this.rbNo);
            this.customGroupBox3.Controls.Add(this.rbYes);
            this.customGroupBox3.Controls.Add(this.label10);
            this.customGroupBox3.Controls.Add(this.txtProgressContent);
            this.customGroupBox3.Controls.Add(this.label9);
            this.customGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox3.Location = new System.Drawing.Point(0, 334);
            this.customGroupBox3.Name = "customGroupBox3";
            this.customGroupBox3.Size = new System.Drawing.Size(873, 260);
            this.customGroupBox3.TabIndex = 3;
            this.customGroupBox3.TabStop = false;
            this.customGroupBox3.Text = "进展情况";
            // 
            // btnSetKeyPoint
            // 
            this.btnSetKeyPoint.Location = new System.Drawing.Point(535, 21);
            this.btnSetKeyPoint.Name = "btnSetKeyPoint";
            this.btnSetKeyPoint.Size = new System.Drawing.Size(142, 23);
            this.btnSetKeyPoint.TabIndex = 370;
            this.btnSetKeyPoint.Text = "评价关键节点";
            this.btnSetKeyPoint.UseVisualStyleBackColor = true;
            this.btnSetKeyPoint.Click += new System.EventHandler(this.btnSetKeyPoint_Click);
            // 
            // txtNextPlan
            // 
            this.txtNextPlan.DataResult = null;
            this.txtNextPlan.DataTableResult = null;
            this.txtNextPlan.EditingControlDataGridView = null;
            this.txtNextPlan.EditingControlFormattedValue = "";
            this.txtNextPlan.EditingControlRowIndex = 0;
            this.txtNextPlan.EditingControlValueChanged = true;
            this.txtNextPlan.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtNextPlan.IsMultiSelect = false;
            this.txtNextPlan.Location = new System.Drawing.Point(100, 156);
            this.txtNextPlan.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNextPlan.Multiline = true;
            this.txtNextPlan.Name = "txtNextPlan";
            this.txtNextPlan.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtNextPlan.ShowResultForm = false;
            this.txtNextPlan.Size = new System.Drawing.Size(752, 93);
            this.txtNextPlan.StrEndSql = null;
            this.txtNextPlan.TabIndex = 367;
            this.txtNextPlan.TabStop = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(25, 196);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 366;
            this.label11.Text = "下月计划：";
            // 
            // rbDelay
            // 
            this.rbDelay.AutoSize = true;
            this.rbDelay.ForeColor = System.Drawing.Color.Black;
            this.rbDelay.Location = new System.Drawing.Point(329, 24);
            this.rbDelay.Name = "rbDelay";
            this.rbDelay.Size = new System.Drawing.Size(83, 16);
            this.rbDelay.TabIndex = 365;
            this.rbDelay.TabStop = true;
            this.rbDelay.Text = "延期（△）";
            this.rbDelay.UseVisualStyleBackColor = true;
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.ForeColor = System.Drawing.Color.Black;
            this.rbNo.Location = new System.Drawing.Point(207, 24);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(95, 16);
            this.rbNo.TabIndex = 364;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "未完成（×）";
            this.rbNo.UseVisualStyleBackColor = true;
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.ForeColor = System.Drawing.Color.Black;
            this.rbYes.Location = new System.Drawing.Point(100, 24);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(83, 16);
            this.rbYes.TabIndex = 363;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "完成（○）";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(25, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 362;
            this.label10.Text = "评    价：";
            // 
            // txtProgressContent
            // 
            this.txtProgressContent.DataResult = null;
            this.txtProgressContent.DataTableResult = null;
            this.txtProgressContent.EditingControlDataGridView = null;
            this.txtProgressContent.EditingControlFormattedValue = "";
            this.txtProgressContent.EditingControlRowIndex = 0;
            this.txtProgressContent.EditingControlValueChanged = true;
            this.txtProgressContent.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtProgressContent.IsMultiSelect = false;
            this.txtProgressContent.Location = new System.Drawing.Point(100, 51);
            this.txtProgressContent.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProgressContent.Multiline = true;
            this.txtProgressContent.Name = "txtProgressContent";
            this.txtProgressContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProgressContent.ShowResultForm = false;
            this.txtProgressContent.Size = new System.Drawing.Size(752, 93);
            this.txtProgressContent.StrEndSql = null;
            this.txtProgressContent.TabIndex = 361;
            this.txtProgressContent.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(25, 91);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 360;
            this.label9.Text = "月度进展：";
            // 
            // 重点工作明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 594);
            this.Controls.Add(this.customGroupBox3);
            this.Controls.Add(this.customGroupBox2);
            this.Controls.Add(this.gbTaskName);
            this.Controls.Add(this.panel1);
            this.Name = "重点工作明细";
            this.Text = "重点工作明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.重点工作明细_PanelGetDataInfo);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gbTaskName.ResumeLayout(false);
            this.customGroupBox2.ResumeLayout(false);
            this.customGroupBox2.PerformLayout();
            this.customGroupBox3.ResumeLayout(false);
            this.customGroupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private UniversalControlLibrary.CustomComboBox cmbMonth;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.CustomComboBox cmbYear;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.CustomGroupBox gbTaskName;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private UniversalControlLibrary.TextBoxShow txtExpectedGoal;
        private System.Windows.Forms.Label label5;
        private UniversalControlLibrary.TextBoxShow txtTaskDescription;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnKeyPoint;
        private System.Windows.Forms.TextBox txtDutyUser;
        private System.Windows.Forms.Label label8;
        private UniversalControlLibrary.CustomGroupBox customGroupBox3;
        private System.Windows.Forms.Label label10;
        private UniversalControlLibrary.TextBoxShow txtProgressContent;
        private System.Windows.Forms.Label label9;
        private UniversalControlLibrary.TextBoxShow txtNextPlan;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.RadioButton rbDelay;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYes;
        private System.Windows.Forms.Button btnSetKeyPoint;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;

    }
}