using UniversalControlLibrary;
namespace Expression
{
    partial class FormReplaceAccessory
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
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dtpGiveOutDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtNewGoodsID = new System.Windows.Forms.TextBox();
            this.txtOldGoodsID = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtNewCvtID = new System.Windows.Forms.TextBox();
            this.txtOldCvtID = new System.Windows.Forms.TextBox();
            this.txtOldCode = new TextBoxShow();
            this.txtCount = new System.Windows.Forms.NumericUpDown();
            this.txtNewCode = new TextBoxShow();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtGoodsSpec = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpBackTime = new System.Windows.Forms.DateTimePicker();
            this.label23 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Service = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldGoodsCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldGoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldGoods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldCvtID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BackTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GiveOutDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewGoodsCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewGoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewSpec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewGoods = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewCvtID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCount)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.toolStripSeparator5,
            this.btnClose,
            this.toolStripSeparator3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(803, 25);
            this.toolStrip1.TabIndex = 9;
            this.toolStrip1.TabStop = true;
            this.toolStrip1.Tag = "view";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(49, 22);
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator5.Tag = "";
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(49, 22);
            this.btnClose.Text = "取消";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dtpGiveOutDate);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.txtNewGoodsID);
            this.panel1.Controls.Add(this.txtOldGoodsID);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.txtNewCvtID);
            this.panel1.Controls.Add(this.txtOldCvtID);
            this.panel1.Controls.Add(this.txtOldCode);
            this.panel1.Controls.Add(this.txtCount);
            this.panel1.Controls.Add(this.txtNewCode);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.txtGoodsSpec);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtUnit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.dtpBackTime);
            this.panel1.Controls.Add(this.label23);
            this.panel1.Controls.Add(this.txtGoodsName);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(803, 126);
            this.panel1.TabIndex = 10;
            // 
            // dtpGiveOutDate
            // 
            this.dtpGiveOutDate.Checked = false;
            this.dtpGiveOutDate.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpGiveOutDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpGiveOutDate.Location = new System.Drawing.Point(382, 73);
            this.dtpGiveOutDate.Name = "dtpGiveOutDate";
            this.dtpGiveOutDate.ShowCheckBox = true;
            this.dtpGiveOutDate.Size = new System.Drawing.Size(161, 21);
            this.dtpGiveOutDate.TabIndex = 106;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(299, 77);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 105;
            this.label11.Text = "旧件发出时间";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(478, 48);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 104;
            this.label9.Text = "更新件编号";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(478, 15);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 103;
            this.label10.Text = "返回件编号";
            // 
            // txtNewGoodsID
            // 
            this.txtNewGoodsID.Location = new System.Drawing.Point(545, 44);
            this.txtNewGoodsID.Name = "txtNewGoodsID";
            this.txtNewGoodsID.Size = new System.Drawing.Size(90, 21);
            this.txtNewGoodsID.TabIndex = 102;
            // 
            // txtOldGoodsID
            // 
            this.txtOldGoodsID.Location = new System.Drawing.Point(545, 11);
            this.txtOldGoodsID.Name = "txtOldGoodsID";
            this.txtOldGoodsID.Size = new System.Drawing.Size(90, 21);
            this.txtOldGoodsID.TabIndex = 101;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(640, 47);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 96;
            this.label8.Text = "总成编号";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(640, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 95;
            this.label7.Text = "总成编号";
            // 
            // txtNewCvtID
            // 
            this.txtNewCvtID.Location = new System.Drawing.Point(696, 43);
            this.txtNewCvtID.Name = "txtNewCvtID";
            this.txtNewCvtID.Size = new System.Drawing.Size(100, 21);
            this.txtNewCvtID.TabIndex = 94;
            // 
            // txtOldCvtID
            // 
            this.txtOldCvtID.Location = new System.Drawing.Point(696, 10);
            this.txtOldCvtID.Name = "txtOldCvtID";
            this.txtOldCvtID.Size = new System.Drawing.Size(100, 21);
            this.txtOldCvtID.TabIndex = 93;
            // 
            // txtOldCode
            // 
            this.txtOldCode.BackColor = System.Drawing.Color.White;
            this.txtOldCode.FindItem = TextBoxShow.FindType.所有物品;
            this.txtOldCode.Location = new System.Drawing.Point(78, 10);
            this.txtOldCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtOldCode.Name = "txtOldCode";
            this.txtOldCode.ReadOnly = true;
            this.txtOldCode.Size = new System.Drawing.Size(102, 21);
            this.txtOldCode.TabIndex = 92;
            this.txtOldCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtOldCode_OnCompleteSearch);
            // 
            // txtCount
            // 
            this.txtCount.BackColor = System.Drawing.Color.White;
            this.txtCount.Location = new System.Drawing.Point(78, 75);
            this.txtCount.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.txtCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(60, 21);
            this.txtCount.TabIndex = 91;
            this.txtCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtNewCode
            // 
            this.txtNewCode.BackColor = System.Drawing.Color.White;
            this.txtNewCode.FindItem = TextBoxShow.FindType.所有物品;
            this.txtNewCode.Location = new System.Drawing.Point(78, 43);
            this.txtNewCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtNewCode.Name = "txtNewCode";
            this.txtNewCode.ReadOnly = true;
            this.txtNewCode.Size = new System.Drawing.Size(102, 21);
            this.txtNewCode.TabIndex = 90;
            this.txtNewCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtNewCode_OnCompleteSearch);
            // 
            // textBox1
            // 
            this.textBox1.BackColor = System.Drawing.Color.White;
            this.textBox1.Location = new System.Drawing.Point(386, 43);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(89, 21);
            this.textBox1.TabIndex = 88;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(355, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 12);
            this.label4.TabIndex = 87;
            this.label4.Text = "规格";
            // 
            // textBox2
            // 
            this.textBox2.BackColor = System.Drawing.Color.White;
            this.textBox2.Location = new System.Drawing.Point(250, 43);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(101, 21);
            this.textBox2.TabIndex = 85;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(182, 47);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 84;
            this.label5.Text = "更新件名称";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 47);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 82;
            this.label6.Text = "更新件图型号";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(599, 97);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 23);
            this.btnDelete.TabIndex = 81;
            this.btnDelete.Text = "删 除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(505, 97);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(77, 23);
            this.btnAdd.TabIndex = 80;
            this.btnAdd.Text = "确 认";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtGoodsSpec
            // 
            this.txtGoodsSpec.BackColor = System.Drawing.Color.White;
            this.txtGoodsSpec.Location = new System.Drawing.Point(388, 10);
            this.txtGoodsSpec.Name = "txtGoodsSpec";
            this.txtGoodsSpec.ReadOnly = true;
            this.txtGoodsSpec.Size = new System.Drawing.Size(87, 21);
            this.txtGoodsSpec.TabIndex = 79;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(355, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 78;
            this.label3.Text = "规格";
            // 
            // txtUnit
            // 
            this.txtUnit.Location = new System.Drawing.Point(192, 74);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.Size = new System.Drawing.Size(54, 21);
            this.txtUnit.TabIndex = 77;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(146, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 76;
            this.label2.Text = "单  位";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(5, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 74;
            this.label1.Text = "数    量";
            // 
            // dtpBackTime
            // 
            this.dtpBackTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpBackTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBackTime.Location = new System.Drawing.Point(642, 73);
            this.dtpBackTime.Name = "dtpBackTime";
            this.dtpBackTime.Size = new System.Drawing.Size(154, 21);
            this.dtpBackTime.TabIndex = 73;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.ForeColor = System.Drawing.Color.Blue;
            this.label23.Location = new System.Drawing.Point(559, 77);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(77, 12);
            this.label23.TabIndex = 72;
            this.label23.Text = "旧件返回日期";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.BackColor = System.Drawing.Color.White;
            this.txtGoodsName.Location = new System.Drawing.Point(250, 10);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ReadOnly = true;
            this.txtGoodsName.Size = new System.Drawing.Size(101, 21);
            this.txtGoodsName.TabIndex = 70;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(181, 14);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 69;
            this.label16.Text = "返回件名称";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(3, 14);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 12);
            this.label17.TabIndex = 67;
            this.label17.Text = "返回件图型号";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 151);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(803, 294);
            this.panel2.TabIndex = 11;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Service,
            this.OldGoodsCode,
            this.OldGoodsName,
            this.OldSpec,
            this.OldGoods,
            this.OldCvtID,
            this.BackTime,
            this.GiveOutDate,
            this.NewGoodsCode,
            this.NewGoodsName,
            this.NewSpec,
            this.NewGoods,
            this.NewCvtID,
            this.Count,
            this.Unit});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 5;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(803, 294);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // Service
            // 
            this.Service.DataPropertyName = "ServiceID";
            this.Service.HeaderText = "关联单号";
            this.Service.Name = "Service";
            this.Service.ReadOnly = true;
            // 
            // OldGoodsCode
            // 
            this.OldGoodsCode.DataPropertyName = "OldGoodsCode";
            this.OldGoodsCode.HeaderText = "返回件图号";
            this.OldGoodsCode.Name = "OldGoodsCode";
            this.OldGoodsCode.ReadOnly = true;
            // 
            // OldGoodsName
            // 
            this.OldGoodsName.DataPropertyName = "OldGoodsName";
            this.OldGoodsName.HeaderText = "返回件名称";
            this.OldGoodsName.Name = "OldGoodsName";
            this.OldGoodsName.ReadOnly = true;
            // 
            // OldSpec
            // 
            this.OldSpec.DataPropertyName = "OldSpec";
            this.OldSpec.HeaderText = "返回件规格";
            this.OldSpec.Name = "OldSpec";
            this.OldSpec.ReadOnly = true;
            // 
            // OldGoods
            // 
            this.OldGoods.DataPropertyName = "OldGoodsID";
            this.OldGoods.HeaderText = "返回件编号";
            this.OldGoods.Name = "OldGoods";
            this.OldGoods.ReadOnly = true;
            // 
            // OldCvtID
            // 
            this.OldCvtID.DataPropertyName = "OldCvtID";
            this.OldCvtID.HeaderText = "总成编号";
            this.OldCvtID.Name = "OldCvtID";
            this.OldCvtID.ReadOnly = true;
            // 
            // BackTime
            // 
            this.BackTime.DataPropertyName = "BackTime";
            this.BackTime.HeaderText = "返回日期";
            this.BackTime.Name = "BackTime";
            this.BackTime.ReadOnly = true;
            // 
            // GiveOutDate
            // 
            this.GiveOutDate.HeaderText = "旧件发出时间";
            this.GiveOutDate.Name = "GiveOutDate";
            this.GiveOutDate.ReadOnly = true;
            // 
            // NewGoodsCode
            // 
            this.NewGoodsCode.DataPropertyName = "NewGoodsCode";
            this.NewGoodsCode.HeaderText = "更新件图号";
            this.NewGoodsCode.Name = "NewGoodsCode";
            this.NewGoodsCode.ReadOnly = true;
            // 
            // NewGoodsName
            // 
            this.NewGoodsName.DataPropertyName = "NewGoodsName";
            this.NewGoodsName.HeaderText = "更新件名称";
            this.NewGoodsName.Name = "NewGoodsName";
            this.NewGoodsName.ReadOnly = true;
            // 
            // NewSpec
            // 
            this.NewSpec.DataPropertyName = "NewSpec";
            this.NewSpec.HeaderText = "更新件规格";
            this.NewSpec.Name = "NewSpec";
            this.NewSpec.ReadOnly = true;
            // 
            // NewGoods
            // 
            this.NewGoods.DataPropertyName = "NewGoodsID";
            this.NewGoods.HeaderText = "更新件编号";
            this.NewGoods.Name = "NewGoods";
            this.NewGoods.ReadOnly = true;
            // 
            // NewCvtID
            // 
            this.NewCvtID.DataPropertyName = "NewCvtID";
            this.NewCvtID.HeaderText = "更新件总成编号";
            this.NewCvtID.Name = "NewCvtID";
            this.NewCvtID.ReadOnly = true;
            // 
            // Count
            // 
            this.Count.DataPropertyName = "Count";
            this.Count.HeaderText = "数量";
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            // 
            // Unit
            // 
            this.Unit.DataPropertyName = "Unit";
            this.Unit.HeaderText = "单位";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            // 
            // FormReplaceAccessory
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(803, 445);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormReplaceAccessory";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "返回件与更新件";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCount)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox txtGoodsSpec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpBackTime;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private TextBoxShow txtNewCode;
        private System.Windows.Forms.NumericUpDown txtCount;
        private TextBoxShow txtOldCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtNewCvtID;
        private System.Windows.Forms.TextBox txtOldCvtID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtNewGoodsID;
        private System.Windows.Forms.TextBox txtOldGoodsID;
        private System.Windows.Forms.DateTimePicker dtpGiveOutDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridViewTextBoxColumn Service;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldGoodsCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldGoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldGoods;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldCvtID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BackTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn GiveOutDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewGoodsCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewGoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewSpec;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewGoods;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewCvtID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;


    }
}