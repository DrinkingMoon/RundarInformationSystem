namespace Expression
{
    partial class 工位管辖划分
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvPurposeAuthority = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnFindCode = new System.Windows.Forms.Button();
            this.txtWorkID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.btnAddPurposeAuthority = new System.Windows.Forms.ToolStripButton();
            this.btnDeletePurposeAuthority = new System.Windows.Forms.ToolStripButton();
            this.cmbPurpose = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgvPurpose = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtWorkbenchRemark = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStrip4 = new System.Windows.Forms.ToolStrip();
            this.btnAddWorkbench = new System.Windows.Forms.ToolStripButton();
            this.btnUpdateWorkbench = new System.Windows.Forms.ToolStripButton();
            this.dgvWorkbench = new System.Windows.Forms.DataGridView();
            this.txtWorkbench = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.toolStrip3 = new System.Windows.Forms.ToolStrip();
            this.btnAddPurpose = new System.Windows.Forms.ToolStripButton();
            this.btnUpdatePurpose = new System.Windows.Forms.ToolStripButton();
            this.txtPurposeName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvWorkbenchPurpose = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbWorkbench = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUpdateWorkbenchPurpose = new System.Windows.Forms.ToolStripButton();
            this.cmbWorkbenchPurpose = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurposeAuthority)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurpose)).BeginInit();
            this.panelTop.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.toolStrip4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkbench)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.toolStrip3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkbenchPurpose)).BeginInit();
            this.panel2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox1.Controls.Add(this.dgvPurposeAuthority);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Location = new System.Drawing.Point(0, 306);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(518, 428);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "为人员分配装配用途（行使管理权限）";
            // 
            // dgvPurposeAuthority
            // 
            this.dgvPurposeAuthority.AllowUserToAddRows = false;
            this.dgvPurposeAuthority.AllowUserToDeleteRows = false;
            this.dgvPurposeAuthority.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvPurposeAuthority.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurposeAuthority.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPurposeAuthority.Location = new System.Drawing.Point(3, 105);
            this.dgvPurposeAuthority.MultiSelect = false;
            this.dgvPurposeAuthority.Name = "dgvPurposeAuthority";
            this.dgvPurposeAuthority.ReadOnly = true;
            this.dgvPurposeAuthority.RowTemplate.Height = 25;
            this.dgvPurposeAuthority.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurposeAuthority.Size = new System.Drawing.Size(512, 320);
            this.dgvPurposeAuthority.TabIndex = 6;
            this.dgvPurposeAuthority.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dgvPurposeAuthority.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurposeAuthority_CellClick);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnFindCode);
            this.panel1.Controls.Add(this.txtWorkID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.toolStrip2);
            this.panel1.Controls.Add(this.cmbPurpose);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(512, 86);
            this.panel1.TabIndex = 5;
            // 
            // btnFindCode
            // 
            this.btnFindCode.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode.Location = new System.Drawing.Point(150, 13);
            this.btnFindCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFindCode.Name = "btnFindCode";
            this.btnFindCode.Size = new System.Drawing.Size(21, 17);
            this.btnFindCode.TabIndex = 170;
            this.btnFindCode.UseVisualStyleBackColor = false;
            this.btnFindCode.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // txtWorkID
            // 
            this.txtWorkID.Location = new System.Drawing.Point(54, 10);
            this.txtWorkID.Name = "txtWorkID";
            this.txtWorkID.ReadOnly = true;
            this.txtWorkID.Size = new System.Drawing.Size(90, 23);
            this.txtWorkID.TabIndex = 10;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "工号";
            // 
            // toolStrip2
            // 
            this.toolStrip2.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddPurposeAuthority,
            this.btnDeletePurposeAuthority});
            this.toolStrip2.Location = new System.Drawing.Point(16, 47);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(146, 25);
            this.toolStrip2.TabIndex = 4;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // btnAddPurposeAuthority
            // 
            this.btnAddPurposeAuthority.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAddPurposeAuthority.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPurposeAuthority.Name = "btnAddPurposeAuthority";
            this.btnAddPurposeAuthority.Size = new System.Drawing.Size(67, 22);
            this.btnAddPurposeAuthority.Tag = "Add";
            this.btnAddPurposeAuthority.Text = "添加(&A)";
            this.btnAddPurposeAuthority.Click += new System.EventHandler(this.btnAddPurposeAuthority_Click);
            // 
            // btnDeletePurposeAuthority
            // 
            this.btnDeletePurposeAuthority.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDeletePurposeAuthority.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeletePurposeAuthority.Name = "btnDeletePurposeAuthority";
            this.btnDeletePurposeAuthority.Size = new System.Drawing.Size(67, 22);
            this.btnDeletePurposeAuthority.Tag = "delete";
            this.btnDeletePurposeAuthority.Text = "删除(&D)";
            this.btnDeletePurposeAuthority.Click += new System.EventHandler(this.btnDeletePurposeAuthority_Click);
            // 
            // cmbPurpose
            // 
            this.cmbPurpose.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPurpose.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbPurpose.FormattingEnabled = true;
            this.cmbPurpose.Location = new System.Drawing.Point(276, 11);
            this.cmbPurpose.Name = "cmbPurpose";
            this.cmbPurpose.Size = new System.Drawing.Size(206, 22);
            this.cmbPurpose.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(207, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "管辖用途";
            // 
            // dgvPurpose
            // 
            this.dgvPurpose.AllowUserToAddRows = false;
            this.dgvPurpose.AllowUserToDeleteRows = false;
            this.dgvPurpose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPurpose.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.dgvPurpose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPurpose.Location = new System.Drawing.Point(15, 58);
            this.dgvPurpose.MultiSelect = false;
            this.dgvPurpose.Name = "dgvPurpose";
            this.dgvPurpose.ReadOnly = true;
            this.dgvPurpose.RowTemplate.Height = 25;
            this.dgvPurpose.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPurpose.Size = new System.Drawing.Size(488, 239);
            this.dgvPurpose.TabIndex = 3;
            this.dgvPurpose.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dgvPurpose.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPurpose_CellClick);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.groupBox3);
            this.panelTop.Controls.Add(this.groupBox2);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1016, 306);
            this.panelTop.TabIndex = 4;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.txtWorkbenchRemark);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.toolStrip4);
            this.groupBox3.Controls.Add(this.dgvWorkbench);
            this.groupBox3.Controls.Add(this.txtWorkbench);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(524, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(492, 306);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "工位管理";
            // 
            // txtWorkbenchRemark
            // 
            this.txtWorkbenchRemark.Location = new System.Drawing.Point(205, 25);
            this.txtWorkbenchRemark.Name = "txtWorkbenchRemark";
            this.txtWorkbenchRemark.Size = new System.Drawing.Size(133, 23);
            this.txtWorkbenchRemark.TabIndex = 12;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(164, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 14);
            this.label5.TabIndex = 11;
            this.label5.Text = "备注";
            // 
            // toolStrip4
            // 
            this.toolStrip4.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip4.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddWorkbench,
            this.btnUpdateWorkbench});
            this.toolStrip4.Location = new System.Drawing.Point(344, 23);
            this.toolStrip4.Name = "toolStrip4";
            this.toolStrip4.Size = new System.Drawing.Size(146, 25);
            this.toolStrip4.TabIndex = 10;
            this.toolStrip4.Text = "toolStrip4";
            // 
            // btnAddWorkbench
            // 
            this.btnAddWorkbench.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAddWorkbench.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddWorkbench.Name = "btnAddWorkbench";
            this.btnAddWorkbench.Size = new System.Drawing.Size(67, 22);
            this.btnAddWorkbench.Tag = "Add";
            this.btnAddWorkbench.Text = "添加(&A)";
            this.btnAddWorkbench.Click += new System.EventHandler(this.btnAddWorkbench_Click);
            // 
            // btnUpdateWorkbench
            // 
            this.btnUpdateWorkbench.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdateWorkbench.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdateWorkbench.Name = "btnUpdateWorkbench";
            this.btnUpdateWorkbench.Size = new System.Drawing.Size(67, 22);
            this.btnUpdateWorkbench.Tag = "update";
            this.btnUpdateWorkbench.Text = "修改(&U)";
            this.btnUpdateWorkbench.Click += new System.EventHandler(this.btnUpdateWorkbench_Click);
            // 
            // dgvWorkbench
            // 
            this.dgvWorkbench.AllowUserToAddRows = false;
            this.dgvWorkbench.AllowUserToDeleteRows = false;
            this.dgvWorkbench.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvWorkbench.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkbench.Location = new System.Drawing.Point(13, 58);
            this.dgvWorkbench.MultiSelect = false;
            this.dgvWorkbench.Name = "dgvWorkbench";
            this.dgvWorkbench.ReadOnly = true;
            this.dgvWorkbench.RowTemplate.Height = 25;
            this.dgvWorkbench.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkbench.Size = new System.Drawing.Size(467, 239);
            this.dgvWorkbench.TabIndex = 7;
            this.dgvWorkbench.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dgvWorkbench.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkbench_CellClick);
            // 
            // txtWorkbench
            // 
            this.txtWorkbench.Location = new System.Drawing.Point(64, 25);
            this.txtWorkbench.Name = "txtWorkbench";
            this.txtWorkbench.Size = new System.Drawing.Size(90, 23);
            this.txtWorkbench.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 8;
            this.label4.Text = "工位号";
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.groupBox2.Controls.Add(this.toolStrip3);
            this.groupBox2.Controls.Add(this.dgvPurpose);
            this.groupBox2.Controls.Add(this.txtPurposeName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(518, 306);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "装配用途管理";
            // 
            // toolStrip3
            // 
            this.toolStrip3.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddPurpose,
            this.btnUpdatePurpose});
            this.toolStrip3.Location = new System.Drawing.Point(299, 23);
            this.toolStrip3.Name = "toolStrip3";
            this.toolStrip3.Size = new System.Drawing.Size(146, 25);
            this.toolStrip3.TabIndex = 6;
            this.toolStrip3.Text = "toolStrip3";
            // 
            // btnAddPurpose
            // 
            this.btnAddPurpose.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAddPurpose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPurpose.Name = "btnAddPurpose";
            this.btnAddPurpose.Size = new System.Drawing.Size(67, 22);
            this.btnAddPurpose.Tag = "Add";
            this.btnAddPurpose.Text = "添加(&A)";
            this.btnAddPurpose.Click += new System.EventHandler(this.btnAddPurpose_Click);
            // 
            // btnUpdatePurpose
            // 
            this.btnUpdatePurpose.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdatePurpose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdatePurpose.Name = "btnUpdatePurpose";
            this.btnUpdatePurpose.Size = new System.Drawing.Size(67, 22);
            this.btnUpdatePurpose.Tag = "update";
            this.btnUpdatePurpose.Text = "修改(&U)";
            this.btnUpdatePurpose.Click += new System.EventHandler(this.btnUpdatePurpose_Click);
            // 
            // txtPurposeName
            // 
            this.txtPurposeName.Location = new System.Drawing.Point(81, 25);
            this.txtPurposeName.Name = "txtPurposeName";
            this.txtPurposeName.Size = new System.Drawing.Size(206, 23);
            this.txtPurposeName.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "用途名称";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.dgvWorkbenchPurpose);
            this.groupBox4.Controls.Add(this.panel2);
            this.groupBox4.Location = new System.Drawing.Point(524, 306);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(494, 428);
            this.groupBox4.TabIndex = 5;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "为工位分配装配用途";
            // 
            // dgvWorkbenchPurpose
            // 
            this.dgvWorkbenchPurpose.AllowUserToAddRows = false;
            this.dgvWorkbenchPurpose.AllowUserToDeleteRows = false;
            this.dgvWorkbenchPurpose.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvWorkbenchPurpose.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWorkbenchPurpose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWorkbenchPurpose.Location = new System.Drawing.Point(3, 105);
            this.dgvWorkbenchPurpose.MultiSelect = false;
            this.dgvWorkbenchPurpose.Name = "dgvWorkbenchPurpose";
            this.dgvWorkbenchPurpose.ReadOnly = true;
            this.dgvWorkbenchPurpose.RowTemplate.Height = 25;
            this.dgvWorkbenchPurpose.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvWorkbenchPurpose.Size = new System.Drawing.Size(488, 320);
            this.dgvWorkbenchPurpose.TabIndex = 6;
            this.dgvWorkbenchPurpose.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.dgvWorkbenchPurpose.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWorkbenchPurpose_CellClick);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbWorkbench);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.toolStrip1);
            this.panel2.Controls.Add(this.cmbWorkbenchPurpose);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(488, 86);
            this.panel2.TabIndex = 5;
            // 
            // cmbWorkbench
            // 
            this.cmbWorkbench.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbWorkbench.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbWorkbench.FormattingEnabled = true;
            this.cmbWorkbench.Location = new System.Drawing.Point(54, 11);
            this.cmbWorkbench.Name = "cmbWorkbench";
            this.cmbWorkbench.Size = new System.Drawing.Size(107, 22);
            this.cmbWorkbench.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 15);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 14);
            this.label6.TabIndex = 0;
            this.label6.Text = "工位";
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUpdateWorkbenchPurpose});
            this.toolStrip1.Location = new System.Drawing.Point(16, 47);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(79, 25);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUpdateWorkbenchPurpose
            // 
            this.btnUpdateWorkbenchPurpose.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdateWorkbenchPurpose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdateWorkbenchPurpose.Name = "btnUpdateWorkbenchPurpose";
            this.btnUpdateWorkbenchPurpose.Size = new System.Drawing.Size(67, 22);
            this.btnUpdateWorkbenchPurpose.Tag = "update";
            this.btnUpdateWorkbenchPurpose.Text = "修改(&U)";
            this.btnUpdateWorkbenchPurpose.Click += new System.EventHandler(this.btnUpdateWorkbenchPurpose_Click);
            // 
            // cmbWorkbenchPurpose
            // 
            this.cmbWorkbenchPurpose.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbWorkbenchPurpose.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbWorkbenchPurpose.FormattingEnabled = true;
            this.cmbWorkbenchPurpose.Location = new System.Drawing.Point(276, 11);
            this.cmbWorkbenchPurpose.Name = "cmbWorkbenchPurpose";
            this.cmbWorkbenchPurpose.Size = new System.Drawing.Size(206, 22);
            this.cmbWorkbenchPurpose.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(207, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 2;
            this.label7.Text = "工位用途";
            // 
            // 工位管辖划分
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelTop);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "工位管辖划分";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "工位管辖划分管理";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurposeAuthority)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPurpose)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.toolStrip4.ResumeLayout(false);
            this.toolStrip4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkbench)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.toolStrip3.ResumeLayout(false);
            this.toolStrip3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWorkbenchPurpose)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripButton btnAddPurposeAuthority;
        private System.Windows.Forms.ComboBox cmbPurpose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton btnDeletePurposeAuthority;
        private System.Windows.Forms.DataGridView dgvPurpose;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ToolStrip toolStrip3;
        private System.Windows.Forms.ToolStripButton btnAddPurpose;
        private System.Windows.Forms.ToolStripButton btnUpdatePurpose;
        private System.Windows.Forms.TextBox txtPurposeName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStrip toolStrip4;
        private System.Windows.Forms.ToolStripButton btnAddWorkbench;
        private System.Windows.Forms.ToolStripButton btnUpdateWorkbench;
        private System.Windows.Forms.DataGridView dgvWorkbench;
        private System.Windows.Forms.TextBox txtWorkbench;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dgvPurposeAuthority;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtWorkbenchRemark;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtWorkID;
        private System.Windows.Forms.Button btnFindCode;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvWorkbenchPurpose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbWorkbench;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnUpdateWorkbenchPurpose;
        private System.Windows.Forms.ComboBox cmbWorkbenchPurpose;
        private System.Windows.Forms.Label label7;
    }
}