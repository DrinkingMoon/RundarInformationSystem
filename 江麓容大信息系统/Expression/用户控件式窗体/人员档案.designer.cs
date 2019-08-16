namespace Expression
{
    partial class UserControlPersonnel
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("部门分类");
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnPosition = new System.Windows.Forms.ToolStripButton();
            this.panel11 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbUserStatus = new System.Windows.Forms.ComboBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbUseStatus = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbPositionStatus = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbDepot = new System.Windows.Forms.ComboBox();
            this.cmbWorkPost = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.ckbDelete = new System.Windows.Forms.CheckBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ckbOnTheJob = new System.Windows.Forms.CheckBox();
            this.ckbUser = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtPersonnelName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtPersonnelCode = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.工号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.姓名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.部门编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.部门名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.职位编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.职位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.是否操作用户 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.是否在职 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.电子邮件 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.手机号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备注 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DeleteFlag = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelTop = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.toolStrip1.SuspendLayout();
            this.panel11.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnUpdate,
            this.btnPosition});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1063, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(67, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(67, 22);
            this.btnUpdate.Tag = "ADD";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnPosition
            // 
            this.btnPosition.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnPosition.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(115, 22);
            this.btnPosition.Tag = "ADD";
            this.btnPosition.Text = "新增修改职位(&Z)";
            this.btnPosition.Click += new System.EventHandler(this.btnPosition_Click);
            // 
            // panel11
            // 
            this.panel11.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel11.Controls.Add(this.groupBox3);
            this.panel11.Controls.Add(this.panel1);
            this.panel11.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel11.Location = new System.Drawing.Point(0, 25);
            this.panel11.Name = "panel11";
            this.panel11.Size = new System.Drawing.Size(1063, 107);
            this.panel11.TabIndex = 25;
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.cmbUserStatus);
            this.groupBox3.Controls.Add(this.btnFind);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cmbUseStatus);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.cmbPositionStatus);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 42);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1063, 65);
            this.groupBox3.TabIndex = 53;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "人员状态筛选";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(550, 32);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 14);
            this.label10.TabIndex = 17;
            this.label10.Text = "用户状态：";
            // 
            // cmbUserStatus
            // 
            this.cmbUserStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserStatus.FormattingEnabled = true;
            this.cmbUserStatus.Items.AddRange(new object[] {
            "全    部",
            "操作用户",
            "非操作用户"});
            this.cmbUserStatus.Location = new System.Drawing.Point(645, 29);
            this.cmbUserStatus.Name = "cmbUserStatus";
            this.cmbUserStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbUserStatus.TabIndex = 16;
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(850, 29);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(94, 21);
            this.btnFind.TabIndex = 15;
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(286, 32);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 14);
            this.label9.TabIndex = 14;
            this.label9.Text = "使用状态：";
            // 
            // cmbUseStatus
            // 
            this.cmbUseStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUseStatus.FormattingEnabled = true;
            this.cmbUseStatus.Items.AddRange(new object[] {
            "全  部",
            "在使用",
            "已停用"});
            this.cmbUseStatus.Location = new System.Drawing.Point(381, 29);
            this.cmbUseStatus.Name = "cmbUseStatus";
            this.cmbUseStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbUseStatus.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(22, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 14);
            this.label8.TabIndex = 12;
            this.label8.Text = "职位状态：";
            // 
            // cmbPositionStatus
            // 
            this.cmbPositionStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPositionStatus.FormattingEnabled = true;
            this.cmbPositionStatus.Items.AddRange(new object[] {
            "在职",
            "全部",
            "离职"});
            this.cmbPositionStatus.Location = new System.Drawing.Point(117, 29);
            this.cmbPositionStatus.Name = "cmbPositionStatus";
            this.cmbPositionStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbPositionStatus.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1063, 42);
            this.panel1.TabIndex = 51;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(471, 8);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 14;
            this.labelTitle.Text = "人员档案";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbDepot);
            this.groupBox2.Controls.Add(this.cmbWorkPost);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.ckbDelete);
            this.groupBox2.Controls.Add(this.txtRemark);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtEmail);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtPhone);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.ckbOnTheJob);
            this.groupBox2.Controls.Add(this.ckbUser);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtPersonnelName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtPersonnelCode);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox2.Location = new System.Drawing.Point(840, 132);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(223, 640);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息操作";
            // 
            // cmbDepot
            // 
            this.cmbDepot.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbDepot.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbDepot.FormattingEnabled = true;
            this.cmbDepot.Location = new System.Drawing.Point(85, 149);
            this.cmbDepot.Name = "cmbDepot";
            this.cmbDepot.Size = new System.Drawing.Size(133, 21);
            this.cmbDepot.TabIndex = 3;
            // 
            // cmbWorkPost
            // 
            this.cmbWorkPost.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbWorkPost.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbWorkPost.FormattingEnabled = true;
            this.cmbWorkPost.Location = new System.Drawing.Point(85, 110);
            this.cmbWorkPost.Name = "cmbWorkPost";
            this.cmbWorkPost.Size = new System.Drawing.Size(133, 21);
            this.cmbWorkPost.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(8, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 18;
            this.label7.Text = "员工职位";
            // 
            // ckbDelete
            // 
            this.ckbDelete.AutoSize = true;
            this.ckbDelete.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckbDelete.ForeColor = System.Drawing.Color.Red;
            this.ckbDelete.Location = new System.Drawing.Point(166, 344);
            this.ckbDelete.Name = "ckbDelete";
            this.ckbDelete.Size = new System.Drawing.Size(54, 18);
            this.ckbDelete.TabIndex = 10;
            this.ckbDelete.Text = "停用";
            this.ckbDelete.UseVisualStyleBackColor = true;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(85, 270);
            this.txtRemark.MaxLength = 25;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(134, 23);
            this.txtRemark.TabIndex = 6;
            this.txtRemark.Tag = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(8, 274);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 15;
            this.label6.Text = "备    注";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(84, 231);
            this.txtEmail.MaxLength = 25;
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(134, 23);
            this.txtEmail.TabIndex = 5;
            this.txtEmail.Tag = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(19, 234);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 14);
            this.label5.TabIndex = 13;
            this.label5.Text = "Email";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(84, 192);
            this.txtPhone.MaxLength = 25;
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(134, 23);
            this.txtPhone.TabIndex = 4;
            this.txtPhone.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(7, 196);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 11;
            this.label4.Text = "联系电话";
            // 
            // ckbOnTheJob
            // 
            this.ckbOnTheJob.AutoSize = true;
            this.ckbOnTheJob.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckbOnTheJob.Location = new System.Drawing.Point(84, 313);
            this.ckbOnTheJob.Name = "ckbOnTheJob";
            this.ckbOnTheJob.Size = new System.Drawing.Size(54, 18);
            this.ckbOnTheJob.TabIndex = 8;
            this.ckbOnTheJob.Text = "在职";
            this.ckbOnTheJob.UseVisualStyleBackColor = true;
            this.ckbOnTheJob.CheckedChanged += new System.EventHandler(this.ckbOnTheJob_CheckedChanged);
            // 
            // ckbUser
            // 
            this.ckbUser.AutoSize = true;
            this.ckbUser.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ckbUser.Location = new System.Drawing.Point(85, 344);
            this.ckbUser.Name = "ckbUser";
            this.ckbUser.Size = new System.Drawing.Size(82, 18);
            this.ckbUser.TabIndex = 9;
            this.ckbUser.Text = "操作用户";
            this.ckbUser.UseVisualStyleBackColor = true;
            this.ckbUser.CheckedChanged += new System.EventHandler(this.ckbUser_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(7, 155);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "所属部门";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(7, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "员工姓名";
            // 
            // txtPersonnelName
            // 
            this.txtPersonnelName.Location = new System.Drawing.Point(84, 72);
            this.txtPersonnelName.MaxLength = 25;
            this.txtPersonnelName.Name = "txtPersonnelName";
            this.txtPersonnelName.Size = new System.Drawing.Size(134, 23);
            this.txtPersonnelName.TabIndex = 1;
            this.txtPersonnelName.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(7, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "人员工号";
            // 
            // txtPersonnelCode
            // 
            this.txtPersonnelCode.Location = new System.Drawing.Point(84, 37);
            this.txtPersonnelCode.MaxLength = 25;
            this.txtPersonnelCode.Name = "txtPersonnelCode";
            this.txtPersonnelCode.Size = new System.Drawing.Size(134, 23);
            this.txtPersonnelCode.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 132);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(840, 640);
            this.groupBox1.TabIndex = 30;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息显示";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.panelTop);
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.treeView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(834, 618);
            this.panel2.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.ColumnHeader;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.工号,
            this.姓名,
            this.部门编码,
            this.部门名称,
            this.职位编码,
            this.职位,
            this.是否操作用户,
            this.是否在职,
            this.电子邮件,
            this.手机号,
            this.备注,
            this.DeleteFlag});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dataGridView1.Location = new System.Drawing.Point(272, 28);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 50;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(562, 590);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // 工号
            // 
            this.工号.DataPropertyName = "工号";
            this.工号.HeaderText = "工号";
            this.工号.Name = "工号";
            this.工号.Width = 60;
            // 
            // 姓名
            // 
            this.姓名.DataPropertyName = "姓名";
            this.姓名.HeaderText = "姓名";
            this.姓名.Name = "姓名";
            this.姓名.Width = 60;
            // 
            // 部门编码
            // 
            this.部门编码.DataPropertyName = "部门编码";
            this.部门编码.HeaderText = "部门编码";
            this.部门编码.Name = "部门编码";
            this.部门编码.Visible = false;
            this.部门编码.Width = 78;
            // 
            // 部门名称
            // 
            this.部门名称.DataPropertyName = "部门名称";
            this.部门名称.HeaderText = "部门名称";
            this.部门名称.Name = "部门名称";
            this.部门名称.Width = 88;
            // 
            // 职位编码
            // 
            this.职位编码.DataPropertyName = "职位编码";
            this.职位编码.HeaderText = "职位编码";
            this.职位编码.Name = "职位编码";
            this.职位编码.Visible = false;
            this.职位编码.Width = 78;
            // 
            // 职位
            // 
            this.职位.DataPropertyName = "职位";
            this.职位.HeaderText = "职位";
            this.职位.Name = "职位";
            this.职位.Width = 60;
            // 
            // 是否操作用户
            // 
            this.是否操作用户.DataPropertyName = "是否操作用户";
            this.是否操作用户.FalseValue = "false";
            this.是否操作用户.HeaderText = "是否操作用户";
            this.是否操作用户.IndeterminateValue = "true";
            this.是否操作用户.Name = "是否操作用户";
            this.是否操作用户.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.是否操作用户.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.是否操作用户.TrueValue = "true";
            this.是否操作用户.Width = 116;
            // 
            // 是否在职
            // 
            this.是否在职.DataPropertyName = "是否在职";
            this.是否在职.FalseValue = "false";
            this.是否在职.HeaderText = "是否在职";
            this.是否在职.IndeterminateValue = "true";
            this.是否在职.Name = "是否在职";
            this.是否在职.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.是否在职.TrueValue = "true";
            this.是否在职.Width = 88;
            // 
            // 电子邮件
            // 
            this.电子邮件.DataPropertyName = "电子邮件";
            this.电子邮件.HeaderText = "电子邮件";
            this.电子邮件.Name = "电子邮件";
            this.电子邮件.Width = 88;
            // 
            // 手机号
            // 
            this.手机号.DataPropertyName = "手机号";
            this.手机号.HeaderText = "手机号";
            this.手机号.Name = "手机号";
            this.手机号.Width = 74;
            // 
            // 备注
            // 
            this.备注.DataPropertyName = "备注";
            this.备注.HeaderText = "备注";
            this.备注.Name = "备注";
            this.备注.Width = 60;
            // 
            // DeleteFlag
            // 
            this.DeleteFlag.DataPropertyName = "DeleteFlag";
            this.DeleteFlag.HeaderText = "DeleteFlag";
            this.DeleteFlag.Name = "DeleteFlag";
            this.DeleteFlag.Visible = false;
            this.DeleteFlag.Width = 90;
            // 
            // panelTop
            // 
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(272, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(562, 28);
            this.panelTop.TabIndex = 11;
            this.panelTop.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(269, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 618);
            this.splitter1.TabIndex = 10;
            this.splitter1.TabStop = false;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            treeNode2.Name = "部门分类";
            treeNode2.Text = "部门分类";
            this.treeView1.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.treeView1.Size = new System.Drawing.Size(269, 618);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // UserControlPersonnel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel11);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlPersonnel";
            this.Size = new System.Drawing.Size(1063, 772);
            this.Load += new System.EventHandler(this.UserControlPersonnel_Load);
            this.Resize += new System.EventHandler(this.UserControlPersonnel_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel11.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.Panel panel11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox ckbDelete;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox ckbOnTheJob;
        private System.Windows.Forms.CheckBox ckbUser;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPersonnelName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtPersonnelCode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbWorkPost;
        private System.Windows.Forms.ComboBox cmbDepot;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.DataGridViewTextBoxColumn 工号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 姓名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 部门编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 部门名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 职位编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 职位;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 是否操作用户;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 是否在职;
        private System.Windows.Forms.DataGridViewTextBoxColumn 电子邮件;
        private System.Windows.Forms.DataGridViewTextBoxColumn 手机号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备注;
        private System.Windows.Forms.DataGridViewTextBoxColumn DeleteFlag;
        private System.Windows.Forms.ToolStripButton btnPosition;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbUseStatus;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbPositionStatus;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbUserStatus;
    }
}
