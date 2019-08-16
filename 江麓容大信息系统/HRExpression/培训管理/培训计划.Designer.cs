namespace Form_Peripheral_HR
{
    partial class 培训计划
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgv_Course = new UniversalControlLibrary.CustomDataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.计划单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.年份 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.计划类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.课程ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.月份 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.课程名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.申请部门 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.所属部门 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.课程类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.评估方式 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.外训 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.预计课时 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.推荐讲师 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.预计经费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControl_Course = new UniversalControlLibrary.UserControlDataLocalizer();
            this.customGroupBox3 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgv_User = new UniversalControlLibrary.CustomDataGridView();
            this.选 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.汇总ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customContextMenuStrip_Select1 = new UniversalControlLibrary.CustomContextMenuStrip_Select(this.components);
            this.userControl_User = new UniversalControlLibrary.UserControlDataLocalizer();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCollect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPlanBillNo = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbPlanType = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.customGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Course)).BeginInit();
            this.customGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_User)).BeginInit();
            this.customGroupBox1.SuspendLayout();
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
            this.panel1.Size = new System.Drawing.Size(1034, 67);
            this.panel1.TabIndex = 26;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(457, 20);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "培训计划";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 179);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.customGroupBox2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.customGroupBox3);
            this.splitContainer1.Size = new System.Drawing.Size(1034, 550);
            this.splitContainer1.SplitterDistance = 514;
            this.splitContainer1.TabIndex = 29;
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.dgv_Course);
            this.customGroupBox2.Controls.Add(this.userControl_Course);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(514, 550);
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
            this.ID,
            this.计划单号,
            this.年份,
            this.计划类型,
            this.课程ID,
            this.月份,
            this.课程名,
            this.申请部门,
            this.所属部门,
            this.课程类型,
            this.评估方式,
            this.外训,
            this.预计课时,
            this.推荐讲师,
            this.预计经费});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Course.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv_Course.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Course.Location = new System.Drawing.Point(3, 49);
            this.dgv_Course.Name = "dgv_Course";
            this.dgv_Course.RowTemplate.Height = 23;
            this.dgv_Course.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Course.Size = new System.Drawing.Size(508, 498);
            this.dgv_Course.TabIndex = 5;
            this.dgv_Course.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Course_CellEnter);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // 计划单号
            // 
            this.计划单号.DataPropertyName = "计划单号";
            this.计划单号.HeaderText = "计划单号";
            this.计划单号.Name = "计划单号";
            this.计划单号.Visible = false;
            // 
            // 年份
            // 
            this.年份.DataPropertyName = "年份";
            this.年份.HeaderText = "年份";
            this.年份.Name = "年份";
            this.年份.Visible = false;
            // 
            // 计划类型
            // 
            this.计划类型.DataPropertyName = "计划类型";
            this.计划类型.HeaderText = "计划类型";
            this.计划类型.Name = "计划类型";
            this.计划类型.Visible = false;
            // 
            // 课程ID
            // 
            this.课程ID.DataPropertyName = "课程ID";
            this.课程ID.HeaderText = "课程ID";
            this.课程ID.Name = "课程ID";
            this.课程ID.Visible = false;
            // 
            // 月份
            // 
            this.月份.DataPropertyName = "月份";
            this.月份.HeaderText = "月份";
            this.月份.Name = "月份";
            this.月份.Width = 60;
            // 
            // 课程名
            // 
            this.课程名.DataPropertyName = "课程名";
            this.课程名.HeaderText = "课程名";
            this.课程名.Name = "课程名";
            this.课程名.ReadOnly = true;
            // 
            // 申请部门
            // 
            this.申请部门.DataPropertyName = "申请部门";
            this.申请部门.HeaderText = "申请部门";
            this.申请部门.Name = "申请部门";
            this.申请部门.ReadOnly = true;
            // 
            // 所属部门
            // 
            this.所属部门.DataPropertyName = "所属部门";
            this.所属部门.HeaderText = "所属部门";
            this.所属部门.Name = "所属部门";
            this.所属部门.ReadOnly = true;
            this.所属部门.Visible = false;
            // 
            // 课程类型
            // 
            this.课程类型.DataPropertyName = "课程类型";
            this.课程类型.HeaderText = "课程类型";
            this.课程类型.Name = "课程类型";
            this.课程类型.ReadOnly = true;
            // 
            // 评估方式
            // 
            this.评估方式.DataPropertyName = "评估方式";
            this.评估方式.HeaderText = "评估方式";
            this.评估方式.Name = "评估方式";
            this.评估方式.ReadOnly = true;
            // 
            // 外训
            // 
            this.外训.DataPropertyName = "外训";
            this.外训.HeaderText = "外训";
            this.外训.Name = "外训";
            this.外训.ReadOnly = true;
            // 
            // 预计课时
            // 
            this.预计课时.DataPropertyName = "预计课时";
            this.预计课时.HeaderText = "预计课时";
            this.预计课时.Name = "预计课时";
            this.预计课时.ReadOnly = true;
            this.预计课时.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.预计课时.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 推荐讲师
            // 
            this.推荐讲师.DataPropertyName = "推荐讲师";
            this.推荐讲师.HeaderText = "推荐讲师";
            this.推荐讲师.Name = "推荐讲师";
            this.推荐讲师.ReadOnly = true;
            // 
            // 预计经费
            // 
            this.预计经费.DataPropertyName = "预计经费";
            this.预计经费.HeaderText = "预计经费";
            this.预计经费.Name = "预计经费";
            this.预计经费.ReadOnly = true;
            // 
            // userControl_Course
            // 
            this.userControl_Course.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl_Course.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControl_Course.Location = new System.Drawing.Point(3, 17);
            this.userControl_Course.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControl_Course.Name = "userControl_Course";
            this.userControl_Course.OnlyLocalize = false;
            this.userControl_Course.Size = new System.Drawing.Size(508, 32);
            this.userControl_Course.StartIndex = 0;
            this.userControl_Course.TabIndex = 4;
            // 
            // customGroupBox3
            // 
            this.customGroupBox3.Controls.Add(this.dgv_User);
            this.customGroupBox3.Controls.Add(this.userControl_User);
            this.customGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox3.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox3.Name = "customGroupBox3";
            this.customGroupBox3.Size = new System.Drawing.Size(516, 550);
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
            this.选,
            this.汇总ID});
            this.dgv_User.ContextMenuStrip = this.customContextMenuStrip_Select1;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_User.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgv_User.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_User.Location = new System.Drawing.Point(3, 49);
            this.dgv_User.Name = "dgv_User";
            this.dgv_User.ReadOnly = true;
            this.dgv_User.RowTemplate.Height = 23;
            this.dgv_User.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_User.Size = new System.Drawing.Size(510, 498);
            this.dgv_User.TabIndex = 6;
            this.dgv_User.Leave += new System.EventHandler(this.dgv_User_Leave);
            this.dgv_User.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_User_CellClick);
            // 
            // 选
            // 
            this.选.HeaderText = "选";
            this.选.Name = "选";
            this.选.ReadOnly = true;
            this.选.Width = 80;
            // 
            // 汇总ID
            // 
            this.汇总ID.DataPropertyName = "汇总ID";
            this.汇总ID.HeaderText = "汇总ID";
            this.汇总ID.Name = "汇总ID";
            this.汇总ID.ReadOnly = true;
            this.汇总ID.Visible = false;
            // 
            // customContextMenuStrip_Select1
            // 
            this.customContextMenuStrip_Select1.Name = "customContextMenuStrip_Select1";
            this.customContextMenuStrip_Select1.Size = new System.Drawing.Size(137, 136);
            // 
            // userControl_User
            // 
            this.userControl_User.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControl_User.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControl_User.Location = new System.Drawing.Point(3, 17);
            this.userControl_User.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControl_User.Name = "userControl_User";
            this.userControl_User.OnlyLocalize = false;
            this.userControl_User.Size = new System.Drawing.Size(510, 32);
            this.userControl_User.StartIndex = 0;
            this.userControl_User.TabIndex = 5;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.btnSelect);
            this.customGroupBox1.Controls.Add(this.btnSave);
            this.customGroupBox1.Controls.Add(this.btnCollect);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.cmbPlanBillNo);
            this.customGroupBox1.Controls.Add(this.label14);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.cmbYear);
            this.customGroupBox1.Controls.Add(this.label13);
            this.customGroupBox1.Controls.Add(this.cmbPlanType);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 67);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(1034, 112);
            this.customGroupBox1.TabIndex = 27;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "信息录入区";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(761, 29);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(97, 23);
            this.btnSelect.TabIndex = 447;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(761, 70);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(97, 23);
            this.btnSave.TabIndex = 446;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCollect
            // 
            this.btnCollect.Location = new System.Drawing.Point(469, 70);
            this.btnCollect.Name = "btnCollect";
            this.btnCollect.Size = new System.Drawing.Size(217, 23);
            this.btnCollect.TabIndex = 445;
            this.btnCollect.Text = "汇总年度计划";
            this.btnCollect.UseVisualStyleBackColor = true;
            this.btnCollect.Click += new System.EventHandler(this.btnCollect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(467, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 444;
            this.label2.Text = "计 划 名：";
            // 
            // cmbPlanBillNo
            // 
            this.cmbPlanBillNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlanBillNo.FormattingEnabled = true;
            this.cmbPlanBillNo.Location = new System.Drawing.Point(535, 30);
            this.cmbPlanBillNo.Name = "cmbPlanBillNo";
            this.cmbPlanBillNo.Size = new System.Drawing.Size(151, 20);
            this.cmbPlanBillNo.TabIndex = 443;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Enabled = false;
            this.label14.Location = new System.Drawing.Point(372, 75);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 12);
            this.label14.TabIndex = 442;
            this.label14.Text = "年";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(176, 75);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 441;
            this.label1.Text = "计划年份：";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(244, 71);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(121, 20);
            this.cmbYear.TabIndex = 440;
            this.cmbYear.SelectedIndexChanged += new System.EventHandler(this.cmbYear_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(176, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(65, 12);
            this.label13.TabIndex = 439;
            this.label13.Text = "计划类型：";
            // 
            // cmbPlanType
            // 
            this.cmbPlanType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPlanType.FormattingEnabled = true;
            this.cmbPlanType.Items.AddRange(new object[] {
            "年度培训计划",
            "临时培训计划"});
            this.cmbPlanType.Location = new System.Drawing.Point(244, 30);
            this.cmbPlanType.Name = "cmbPlanType";
            this.cmbPlanType.Size = new System.Drawing.Size(145, 20);
            this.cmbPlanType.TabIndex = 438;
            this.cmbPlanType.SelectedIndexChanged += new System.EventHandler(this.cmbPlanType_SelectedIndexChanged);
            // 
            // 培训计划
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 729);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.customGroupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "培训计划";
            this.Text = "培训计划";
            this.Load += new System.EventHandler(this.培训计划_Load);
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
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private UniversalControlLibrary.CustomDataGridView dgv_Course;
        private UniversalControlLibrary.UserControlDataLocalizer userControl_Course;
        private UniversalControlLibrary.CustomGroupBox customGroupBox3;
        private UniversalControlLibrary.CustomDataGridView dgv_User;
        private UniversalControlLibrary.UserControlDataLocalizer userControl_User;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbPlanType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCollect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPlanBillNo;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选;
        private System.Windows.Forms.DataGridViewTextBoxColumn 汇总ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 计划单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 年份;
        private System.Windows.Forms.DataGridViewTextBoxColumn 计划类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 月份;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 申请部门;
        private System.Windows.Forms.DataGridViewTextBoxColumn 所属部门;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 评估方式;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 外训;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预计课时;
        private System.Windows.Forms.DataGridViewTextBoxColumn 推荐讲师;
        private System.Windows.Forms.DataGridViewTextBoxColumn 预计经费;
        private UniversalControlLibrary.CustomContextMenuStrip_Select customContextMenuStrip_Select1;
    }
}