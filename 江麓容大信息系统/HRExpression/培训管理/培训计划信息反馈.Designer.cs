namespace Form_Peripheral_HR
{
    partial class 培训计划信息反馈
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(培训计划信息反馈));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgv_Course = new UniversalControlLibrary.CustomDataGridView();
            this.课程ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox3 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgv_User = new UniversalControlLibrary.CustomDataGridView();
            this.FeedbackID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnSetUser = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtClassHour = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.chbIsOutSide = new System.Windows.Forms.CheckBox();
            this.numFund = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtLecturer = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.btnCourse = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCourse = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.cmbYearValue = new UniversalControlLibrary.CustomComboBox(this.components);
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.customGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Course)).BeginInit();
            this.customGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_User)).BeginInit();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFund)).BeginInit();
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
            this.panel1.Size = new System.Drawing.Size(961, 67);
            this.panel1.TabIndex = 27;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(366, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "培训计划信息反馈";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 264);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.customGroupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.customGroupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(961, 200);
            this.splitContainer1.SplitterDistance = 477;
            this.splitContainer1.TabIndex = 30;
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.dgv_Course);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(477, 200);
            this.customGroupBox2.TabIndex = 0;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "课程";
            // 
            // dgv_Course
            // 
            this.dgv_Course.AllowUserToAddRows = false;
            this.dgv_Course.AllowUserToDeleteRows = false;
            this.dgv_Course.AllowUserToResizeRows = false;
            this.dgv_Course.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Course.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.课程ID,
            this.ID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Course.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgv_Course.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Course.Location = new System.Drawing.Point(3, 17);
            this.dgv_Course.Name = "dgv_Course";
            this.dgv_Course.ReadOnly = true;
            this.dgv_Course.RowTemplate.Height = 23;
            this.dgv_Course.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Course.Size = new System.Drawing.Size(471, 180);
            this.dgv_Course.TabIndex = 5;
            this.dgv_Course.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Course_CellEnter);
            // 
            // 课程ID
            // 
            this.课程ID.DataPropertyName = "课程ID";
            this.课程ID.HeaderText = "课程ID";
            this.课程ID.Name = "课程ID";
            this.课程ID.ReadOnly = true;
            this.课程ID.Visible = false;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // customGroupBox3
            // 
            this.customGroupBox3.Controls.Add(this.dgv_User);
            this.customGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox3.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox3.Name = "customGroupBox3";
            this.customGroupBox3.Size = new System.Drawing.Size(480, 200);
            this.customGroupBox3.TabIndex = 0;
            this.customGroupBox3.TabStop = false;
            this.customGroupBox3.Text = "人员";
            // 
            // dgv_User
            // 
            this.dgv_User.AllowUserToAddRows = false;
            this.dgv_User.AllowUserToDeleteRows = false;
            this.dgv_User.AllowUserToResizeRows = false;
            this.dgv_User.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_User.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.FeedbackID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_User.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_User.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_User.Location = new System.Drawing.Point(3, 17);
            this.dgv_User.Name = "dgv_User";
            this.dgv_User.ReadOnly = true;
            this.dgv_User.RowTemplate.Height = 23;
            this.dgv_User.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_User.Size = new System.Drawing.Size(474, 180);
            this.dgv_User.TabIndex = 6;
            // 
            // FeedbackID
            // 
            this.FeedbackID.DataPropertyName = "FeedbackID";
            this.FeedbackID.HeaderText = "FeedbackID";
            this.FeedbackID.Name = "FeedbackID";
            this.FeedbackID.ReadOnly = true;
            this.FeedbackID.Visible = false;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.btnSetUser);
            this.customGroupBox1.Controls.Add(this.btnDelete);
            this.customGroupBox1.Controls.Add(this.label9);
            this.customGroupBox1.Controls.Add(this.label8);
            this.customGroupBox1.Controls.Add(this.txtClassHour);
            this.customGroupBox1.Controls.Add(this.label4);
            this.customGroupBox1.Controls.Add(this.chbIsOutSide);
            this.customGroupBox1.Controls.Add(this.numFund);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.txtLecturer);
            this.customGroupBox1.Controls.Add(this.label7);
            this.customGroupBox1.Controls.Add(this.dtpEndTime);
            this.customGroupBox1.Controls.Add(this.label6);
            this.customGroupBox1.Controls.Add(this.dtpStartTime);
            this.customGroupBox1.Controls.Add(this.label5);
            this.customGroupBox1.Controls.Add(this.btnCourse);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.txtCourse);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.btnAdd);
            this.customGroupBox1.Controls.Add(this.cmbYearValue);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 113);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(961, 151);
            this.customGroupBox1.TabIndex = 29;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "信息录入区";
            // 
            // btnSetUser
            // 
            this.btnSetUser.Location = new System.Drawing.Point(687, 71);
            this.btnSetUser.Name = "btnSetUser";
            this.btnSetUser.Size = new System.Drawing.Size(181, 23);
            this.btnSetUser.TabIndex = 151;
            this.btnSetUser.Text = "选择参加培训人员";
            this.btnSetUser.UseVisualStyleBackColor = true;
            this.btnSetUser.Click += new System.EventHandler(this.btnSetUser_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(793, 112);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 150;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(874, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 149;
            this.label9.Text = "小时";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(685, 37);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 148;
            this.label8.Text = "预计课时";
            // 
            // txtClassHour
            // 
            this.txtClassHour.Enabled = false;
            this.txtClassHour.Location = new System.Drawing.Point(760, 33);
            this.txtClassHour.Name = "txtClassHour";
            this.txtClassHour.Size = new System.Drawing.Size(108, 21);
            this.txtClassHour.TabIndex = 147;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(579, 117);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(17, 12);
            this.label4.TabIndex = 146;
            this.label4.Text = "元";
            // 
            // chbIsOutSide
            // 
            this.chbIsOutSide.AutoSize = true;
            this.chbIsOutSide.Enabled = false;
            this.chbIsOutSide.Location = new System.Drawing.Point(618, 35);
            this.chbIsOutSide.Name = "chbIsOutSide";
            this.chbIsOutSide.Size = new System.Drawing.Size(48, 16);
            this.chbIsOutSide.TabIndex = 145;
            this.chbIsOutSide.Text = "外训";
            this.chbIsOutSide.UseVisualStyleBackColor = true;
            // 
            // numFund
            // 
            this.numFund.Location = new System.Drawing.Point(454, 113);
            this.numFund.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numFund.Name = "numFund";
            this.numFund.Size = new System.Drawing.Size(120, 21);
            this.numFund.TabIndex = 144;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(359, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 143;
            this.label3.Text = "培 训 经 费";
            // 
            // txtLecturer
            // 
            this.txtLecturer.Location = new System.Drawing.Point(154, 113);
            this.txtLecturer.Name = "txtLecturer";
            this.txtLecturer.Size = new System.Drawing.Size(148, 21);
            this.txtLecturer.TabIndex = 142;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(59, 117);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 141;
            this.label7.Text = "培 训 讲 师";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(454, 72);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(148, 21);
            this.dtpEndTime.TabIndex = 140;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(359, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 139;
            this.label6.Text = "培训终止时间";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpStartTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStartTime.Location = new System.Drawing.Point(154, 72);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(148, 21);
            this.dtpStartTime.TabIndex = 138;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(57, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 137;
            this.label5.Text = "培训开始时间";
            // 
            // btnCourse
            // 
            this.btnCourse.BackColor = System.Drawing.Color.Transparent;
            this.btnCourse.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnCourse.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnCourse.Image = ((System.Drawing.Image)(resources.GetObject("btnCourse.Image")));
            this.btnCourse.Location = new System.Drawing.Point(581, 34);
            this.btnCourse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCourse.Name = "btnCourse";
            this.btnCourse.Size = new System.Drawing.Size(21, 19);
            this.btnCourse.TabIndex = 136;
            this.btnCourse.Tag = "";
            this.btnCourse.UseVisualStyleBackColor = false;
            this.btnCourse.Click += new System.EventHandler(this.btnCourse_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(360, 37);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "课 程 名";
            // 
            // txtCourse
            // 
            this.txtCourse.Location = new System.Drawing.Point(454, 33);
            this.txtCourse.Name = "txtCourse";
            this.txtCourse.ReadOnly = true;
            this.txtCourse.Size = new System.Drawing.Size(121, 21);
            this.txtCourse.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "培 训 年 份";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(687, 112);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // cmbYearValue
            // 
            this.cmbYearValue.FormattingEnabled = true;
            this.cmbYearValue.Location = new System.Drawing.Point(153, 33);
            this.cmbYearValue.MaxYear = 2025;
            this.cmbYearValue.MinYear = 2015;
            this.cmbYearValue.Name = "cmbYearValue";
            this.cmbYearValue.Size = new System.Drawing.Size(121, 20);
            this.cmbYearValue.TabIndex = 0;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.ListBillStatus = ((System.Collections.Generic.List<string>)(resources.GetObject("checkBillDateAndStatus1.ListBillStatus")));
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 67);
            this.checkBillDateAndStatus1.MultiVisible = false;
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(961, 46);
            this.checkBillDateAndStatus1.StatusVisible = false;
            this.checkBillDateAndStatus1.TabIndex = 28;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
            // 
            // 培训计划信息反馈
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 464);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.customGroupBox1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.panel1);
            this.Name = "培训计划信息反馈";
            this.Text = "培训计划信息反馈";
            this.Load += new System.EventHandler(this.培训计划信息反馈_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.customGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Course)).EndInit();
            this.customGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_User)).EndInit();
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numFund)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private UniversalControlLibrary.CheckBillDateAndStatus checkBillDateAndStatus1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private UniversalControlLibrary.CustomDataGridView dgv_Course;
        private UniversalControlLibrary.CustomGroupBox customGroupBox3;
        private UniversalControlLibrary.CustomDataGridView dgv_User;
        private UniversalControlLibrary.CustomComboBox cmbYearValue;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCourse;
        private System.Windows.Forms.Button btnCourse;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numFund;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtLecturer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbIsOutSide;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtClassHour;
        private System.Windows.Forms.Button btnSetUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn FeedbackID;
    }
}