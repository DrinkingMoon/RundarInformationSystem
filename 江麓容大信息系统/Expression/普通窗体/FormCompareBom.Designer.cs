namespace Expression
{
    partial class FormCompareBom
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
            this.添加物品toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.添加剔除本身表物品toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除选中剔除Bom表的零件行toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.删除选中剔除本身表的零件行toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.导出Bom = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.导出本身 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDeleteSearch = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.cmbSearchName = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.dgvSelfReject = new System.Windows.Forms.DataGridView();
            this.Self选择物品 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.Self物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Self图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Self物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Self规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.dgvBomReject = new System.Windows.Forms.DataGridView();
            this.选择物品 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.剔除模式 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvBomShow = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvSelfShow = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.saveFileDialog2 = new System.Windows.Forms.SaveFileDialog();
            this.panel4 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.clbEdition = new System.Windows.Forms.CheckedListBox();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfReject)).BeginInit();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBomReject)).BeginInit();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBomShow)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfShow)).BeginInit();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加物品toolStripButton,
            this.toolStripSeparator1,
            this.添加剔除本身表物品toolStripButton,
            this.toolStripSeparator2,
            this.删除选中剔除Bom表的零件行toolStripButton,
            this.toolStripSeparator3,
            this.删除选中剔除本身表的零件行toolStripButton,
            this.toolStripSeparator4,
            this.导出Bom,
            this.toolStripSeparator5,
            this.导出本身});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1147, 25);
            this.toolStrip1.TabIndex = 39;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 添加物品toolStripButton
            // 
            this.添加物品toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.添加物品toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加物品toolStripButton.Name = "添加物品toolStripButton";
            this.添加物品toolStripButton.Size = new System.Drawing.Size(127, 22);
            this.添加物品toolStripButton.Text = "添加剔除Bom表物品";
            this.添加物品toolStripButton.Click += new System.EventHandler(this.添加物品toolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 添加剔除本身表物品toolStripButton
            // 
            this.添加剔除本身表物品toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.添加剔除本身表物品toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加剔除本身表物品toolStripButton.Name = "添加剔除本身表物品toolStripButton";
            this.添加剔除本身表物品toolStripButton.Size = new System.Drawing.Size(133, 22);
            this.添加剔除本身表物品toolStripButton.Text = "添加剔除本身表物品";
            this.添加剔除本身表物品toolStripButton.Click += new System.EventHandler(this.添加剔除本身表物品toolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 删除选中剔除Bom表的零件行toolStripButton
            // 
            this.删除选中剔除Bom表的零件行toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.删除选中剔除Bom表的零件行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除选中剔除Bom表的零件行toolStripButton.Name = "删除选中剔除Bom表的零件行toolStripButton";
            this.删除选中剔除Bom表的零件行toolStripButton.Size = new System.Drawing.Size(175, 22);
            this.删除选中剔除Bom表的零件行toolStripButton.Text = "删除选中剔除Bom表的零件行";
            this.删除选中剔除Bom表的零件行toolStripButton.Click += new System.EventHandler(this.删除选中剔除Bom表的零件行toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // 删除选中剔除本身表的零件行toolStripButton
            // 
            this.删除选中剔除本身表的零件行toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.删除选中剔除本身表的零件行toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除选中剔除本身表的零件行toolStripButton.Name = "删除选中剔除本身表的零件行toolStripButton";
            this.删除选中剔除本身表的零件行toolStripButton.Size = new System.Drawing.Size(181, 22);
            this.删除选中剔除本身表的零件行toolStripButton.Text = "删除选中剔除本身表的零件行";
            this.删除选中剔除本身表的零件行toolStripButton.Click += new System.EventHandler(this.删除选中剔除本身表的零件行toolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出Bom
            // 
            this.导出Bom.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.导出Bom.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出Bom.Name = "导出Bom";
            this.导出Bom.Size = new System.Drawing.Size(127, 22);
            this.导出Bom.Text = "导出剔除Bom的零件";
            this.导出Bom.Click += new System.EventHandler(this.导出Bom_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出本身
            // 
            this.导出本身.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.导出本身.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出本身.Name = "导出本身";
            this.导出本身.Size = new System.Drawing.Size(121, 22);
            this.导出本身.Text = "导出本身表的零件";
            this.导出本身.Click += new System.EventHandler(this.导出本身_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.panel4);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1147, 299);
            this.panel1.TabIndex = 40;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDeleteSearch);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.cmbSearchName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.groupBox5);
            this.groupBox1.Controls.Add(this.groupBox4);
            this.groupBox1.Location = new System.Drawing.Point(0, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1187, 233);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "不需要比较的零件";
            // 
            // btnDeleteSearch
            // 
            this.btnDeleteSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSearch.Location = new System.Drawing.Point(1017, 84);
            this.btnDeleteSearch.Name = "btnDeleteSearch";
            this.btnDeleteSearch.Size = new System.Drawing.Size(160, 27);
            this.btnDeleteSearch.TabIndex = 20;
            this.btnDeleteSearch.Text = "删除我的查询";
            this.btnDeleteSearch.UseVisualStyleBackColor = true;
            this.btnDeleteSearch.Click += new System.EventHandler(this.btnDeleteSearch_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(1017, 146);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(160, 23);
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "保存剔除Bom表零件";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // cmbSearchName
            // 
            this.cmbSearchName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchName.FormattingEnabled = true;
            this.cmbSearchName.Location = new System.Drawing.Point(1017, 57);
            this.cmbSearchName.Name = "cmbSearchName";
            this.cmbSearchName.Size = new System.Drawing.Size(160, 21);
            this.cmbSearchName.TabIndex = 19;
            this.cmbSearchName.SelectedValueChanged += new System.EventHandler(this.cmbSearchName_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1017, 37);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 14);
            this.label9.TabIndex = 18;
            this.label9.Text = "选择我的查询：";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(1017, 187);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(160, 23);
            this.btnQuery.TabIndex = 15;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.dgvSelfReject);
            this.groupBox5.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox5.Location = new System.Drawing.Point(611, 19);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(392, 211);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "剔除本身表的零件";
            // 
            // dgvSelfReject
            // 
            this.dgvSelfReject.AllowUserToAddRows = false;
            this.dgvSelfReject.AllowUserToResizeRows = false;
            this.dgvSelfReject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Self选择物品,
            this.Self物品ID,
            this.Self图号型号,
            this.Self物品名称,
            this.Self规格});
            this.dgvSelfReject.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvSelfReject.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSelfReject.Location = new System.Drawing.Point(3, 19);
            this.dgvSelfReject.Name = "dgvSelfReject";
            this.dgvSelfReject.RowHeadersWidth = 20;
            this.dgvSelfReject.RowTemplate.Height = 23;
            this.dgvSelfReject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelfReject.Size = new System.Drawing.Size(383, 189);
            this.dgvSelfReject.TabIndex = 3;
            this.dgvSelfReject.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvSelfReject_CellContentClick);
            // 
            // Self选择物品
            // 
            this.Self选择物品.Frozen = true;
            this.Self选择物品.HeaderText = "选择物品";
            this.Self选择物品.Name = "Self选择物品";
            this.Self选择物品.Width = 80;
            // 
            // Self物品ID
            // 
            this.Self物品ID.Frozen = true;
            this.Self物品ID.HeaderText = "物品ID";
            this.Self物品ID.Name = "Self物品ID";
            this.Self物品ID.ReadOnly = true;
            this.Self物品ID.Visible = false;
            // 
            // Self图号型号
            // 
            this.Self图号型号.HeaderText = "图号型号";
            this.Self图号型号.Name = "Self图号型号";
            this.Self图号型号.ReadOnly = true;
            this.Self图号型号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Self物品名称
            // 
            this.Self物品名称.HeaderText = "物品名称";
            this.Self物品名称.Name = "Self物品名称";
            this.Self物品名称.ReadOnly = true;
            // 
            // Self规格
            // 
            this.Self规格.HeaderText = "规格";
            this.Self规格.Name = "Self规格";
            this.Self规格.ReadOnly = true;
            this.Self规格.Width = 80;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.dgvBomReject);
            this.groupBox4.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox4.Location = new System.Drawing.Point(3, 19);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(608, 211);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "选择剔除Bom表的零件";
            // 
            // dgvBomReject
            // 
            this.dgvBomReject.AllowUserToAddRows = false;
            this.dgvBomReject.AllowUserToResizeRows = false;
            this.dgvBomReject.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选择物品,
            this.物品ID,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.剔除模式});
            this.dgvBomReject.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvBomReject.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvBomReject.Location = new System.Drawing.Point(3, 19);
            this.dgvBomReject.Name = "dgvBomReject";
            this.dgvBomReject.RowHeadersWidth = 40;
            this.dgvBomReject.RowTemplate.Height = 23;
            this.dgvBomReject.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBomReject.Size = new System.Drawing.Size(602, 189);
            this.dgvBomReject.TabIndex = 8;
            this.dgvBomReject.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBomReject_CellContentClick);
            // 
            // 选择物品
            // 
            this.选择物品.Frozen = true;
            this.选择物品.HeaderText = "选择物品";
            this.选择物品.Name = "选择物品";
            this.选择物品.Width = 80;
            // 
            // 物品ID
            // 
            this.物品ID.Frozen = true;
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 图号型号
            // 
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            this.图号型号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.图号型号.Width = 150;
            // 
            // 物品名称
            // 
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            this.规格.Width = 80;
            // 
            // 剔除模式
            // 
            this.剔除模式.HeaderText = "剔除模式";
            this.剔除模式.Items.AddRange(new object[] {
            "仅零件本身",
            "子零件",
            "本身及子零件"});
            this.剔除模式.Name = "剔除模式";
            this.剔除模式.Width = 150;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 324);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1147, 277);
            this.panel2.TabIndex = 41;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvBomShow);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1147, 277);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "BOM表存在此表不存在";
            // 
            // dgvBomShow
            // 
            this.dgvBomShow.AllowUserToAddRows = false;
            this.dgvBomShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBomShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBomShow.Location = new System.Drawing.Point(3, 19);
            this.dgvBomShow.Name = "dgvBomShow";
            this.dgvBomShow.RowTemplate.Height = 23;
            this.dgvBomShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBomShow.Size = new System.Drawing.Size(1141, 255);
            this.dgvBomShow.TabIndex = 0;
            this.dgvBomShow.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvBomShow_RowPostPaint);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 601);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1147, 151);
            this.panel3.TabIndex = 42;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvSelfShow);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1147, 151);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "此表存在BOM表不存在";
            // 
            // dgvSelfShow
            // 
            this.dgvSelfShow.AllowUserToAddRows = false;
            this.dgvSelfShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelfShow.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelfShow.Location = new System.Drawing.Point(3, 19);
            this.dgvSelfShow.Name = "dgvSelfShow";
            this.dgvSelfShow.RowTemplate.Height = 23;
            this.dgvSelfShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelfShow.Size = new System.Drawing.Size(1141, 129);
            this.dgvSelfShow.TabIndex = 0;
            this.dgvSelfShow.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvSelfShow_RowPostPaint);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.Title = "剔除Bom表中不比较的零件";
            // 
            // saveFileDialog2
            // 
            this.saveFileDialog2.FileName = "*.xls";
            this.saveFileDialog2.Filter = "Excel文件|*.xls";
            this.saveFileDialog2.OverwritePrompt = false;
            this.saveFileDialog2.Title = "剔除本表的不比较的零件";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.label1);
            this.panel4.Controls.Add(this.clbEdition);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1187, 42);
            this.panel4.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(8, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 11;
            this.label1.Text = "产品类型：";
            // 
            // clbEdition
            // 
            this.clbEdition.BackColor = System.Drawing.SystemColors.Control;
            this.clbEdition.CheckOnClick = true;
            this.clbEdition.ColumnWidth = 130;
            this.clbEdition.FormattingEnabled = true;
            this.clbEdition.Location = new System.Drawing.Point(97, 0);
            this.clbEdition.MultiColumn = true;
            this.clbEdition.Name = "clbEdition";
            this.clbEdition.Size = new System.Drawing.Size(1077, 40);
            this.clbEdition.TabIndex = 10;
            // 
            // FormCompareBom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1147, 752);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormCompareBom";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "与BOM比较";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfReject)).EndInit();
            this.groupBox4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBomReject)).EndInit();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBomShow)).EndInit();
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelfShow)).EndInit();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton 添加物品toolStripButton;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvBomShow;
        private System.Windows.Forms.DataGridView dgvSelfShow;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDeleteSearch;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ComboBox cmbSearchName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dgvBomReject;
        private System.Windows.Forms.DataGridViewButtonColumn 选择物品;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewComboBoxColumn 剔除模式;
        private System.Windows.Forms.DataGridView dgvSelfReject;
        private System.Windows.Forms.ToolStripButton 添加剔除本身表物品toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 删除选中剔除Bom表的零件行toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 删除选中剔除本身表的零件行toolStripButton;
        private System.Windows.Forms.DataGridViewButtonColumn Self选择物品;
        private System.Windows.Forms.DataGridViewTextBoxColumn Self物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Self图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn Self物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn Self规格;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 导出Bom;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton 导出本身;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox clbEdition;
    }
}