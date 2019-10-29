namespace Expression
{
    partial class FormMaterialListReturnedInTheDepot
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMaterialListReturnedInTheDepot));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnBatchCreate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnInputExcel = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnProvider = new System.Windows.Forms.Button();
            this.btnBatchNo = new System.Windows.Forms.Button();
            this.cmbProductStatus = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnFindDepot = new System.Windows.Forms.Button();
            this.txtProviderBatchNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRecordRow = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.numReturnedCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMaterialType = new System.Windows.Forms.TextBox();
            this.txtLayer = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.txtColumn = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtShelf = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.btnFindCode = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReturnedCount)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnAdd,
            this.toolStripSeparator2,
            this.btnDelete,
            this.toolStripSeparator7,
            this.btnDeleteAll,
            this.toolStripSeparatorDelete,
            this.btnUpdate,
            this.toolStripSeparator1,
            this.btnBatchCreate,
            this.toolStripSeparator3,
            this.btnInputExcel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1057, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(70, 22);
            this.btnNew.Tag = "ADD";
            this.btnNew.Text = "新建(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 22);
            this.btnAdd.Tag = "更新";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "更新";
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 22);
            this.btnDelete.Tag = "更新";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteAll.Image")));
            this.btnDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(76, 22);
            this.btnDeleteAll.Text = "删除所有";
            this.btnDeleteAll.ToolTipText = "删除此领料单所有清单信息";
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorDelete.Tag = "更新";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(69, 22);
            this.btnUpdate.Tag = "更新";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "更新";
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Image = ((System.Drawing.Image)(resources.GetObject("btnBatchCreate.Image")));
            this.btnBatchCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(139, 22);
            this.btnBatchCreate.Tag = "更新";
            this.btnBatchCreate.Text = "由领料单批量生成(&P)";
            this.btnBatchCreate.Click += new System.EventHandler(this.btnBatchCreate_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "更新";
            // 
            // btnInputExcel
            // 
            this.btnInputExcel.Image = global::Expression.Properties.Resources.Excel;
            this.btnInputExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInputExcel.Name = "btnInputExcel";
            this.btnInputExcel.Size = new System.Drawing.Size(93, 22);
            this.btnInputExcel.Tag = "更新";
            this.btnInputExcel.Text = "Excel导入(&I)";
            this.btnInputExcel.Click += new System.EventHandler(this.btnInputExcel_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 171);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1041, 368);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1049, 40);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(8, 551);
            this.panelRight.TabIndex = 39;
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1057, 591);
            this.panelMain.TabIndex = 39;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panelTop);
            this.panelCenter.Controls.Add(this.panelBottom);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(8, 40);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1041, 551);
            this.panelCenter.TabIndex = 0;
            // 
            // panelTop
            // 
            this.panelTop.AutoScroll = true;
            this.panelTop.Controls.Add(this.btnProvider);
            this.panelTop.Controls.Add(this.btnBatchNo);
            this.panelTop.Controls.Add(this.cmbProductStatus);
            this.panelTop.Controls.Add(this.label11);
            this.panelTop.Controls.Add(this.btnFindDepot);
            this.panelTop.Controls.Add(this.txtProviderBatchNo);
            this.panelTop.Controls.Add(this.label5);
            this.panelTop.Controls.Add(this.txtRemark);
            this.panelTop.Controls.Add(this.label6);
            this.panelTop.Controls.Add(this.lblAmount);
            this.panelTop.Controls.Add(this.label8);
            this.panelTop.Controls.Add(this.lblRecordRow);
            this.panelTop.Controls.Add(this.label4);
            this.panelTop.Controls.Add(this.txtUnit);
            this.panelTop.Controls.Add(this.numReturnedCount);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.txtSpec);
            this.panelTop.Controls.Add(this.label7);
            this.panelTop.Controls.Add(this.txtProvider);
            this.panelTop.Controls.Add(this.label26);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.txtMaterialType);
            this.panelTop.Controls.Add(this.txtLayer);
            this.panelTop.Controls.Add(this.label30);
            this.panelTop.Controls.Add(this.txtColumn);
            this.panelTop.Controls.Add(this.label29);
            this.panelTop.Controls.Add(this.txtShelf);
            this.panelTop.Controls.Add(this.label28);
            this.panelTop.Controls.Add(this.label27);
            this.panelTop.Controls.Add(this.btnFindCode);
            this.panelTop.Controls.Add(this.label13);
            this.panelTop.Controls.Add(this.txtName);
            this.panelTop.Controls.Add(this.label12);
            this.panelTop.Controls.Add(this.txtBatchNo);
            this.panelTop.Controls.Add(this.txtCode);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1041, 171);
            this.panelTop.TabIndex = 31;
            // 
            // btnProvider
            // 
            this.btnProvider.BackColor = System.Drawing.Color.Transparent;
            this.btnProvider.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnProvider.BackgroundImage")));
            this.btnProvider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnProvider.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnProvider.Location = new System.Drawing.Point(1012, 11);
            this.btnProvider.Name = "btnProvider";
            this.btnProvider.Size = new System.Drawing.Size(21, 21);
            this.btnProvider.TabIndex = 233;
            this.btnProvider.UseVisualStyleBackColor = false;
            this.btnProvider.Click += new System.EventHandler(this.btnProvider_Click);
            // 
            // btnBatchNo
            // 
            this.btnBatchNo.BackColor = System.Drawing.Color.Transparent;
            this.btnBatchNo.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBatchNo.BackgroundImage")));
            this.btnBatchNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnBatchNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnBatchNo.Location = new System.Drawing.Point(275, 40);
            this.btnBatchNo.Name = "btnBatchNo";
            this.btnBatchNo.Size = new System.Drawing.Size(21, 21);
            this.btnBatchNo.TabIndex = 232;
            this.btnBatchNo.UseVisualStyleBackColor = false;
            this.btnBatchNo.Click += new System.EventHandler(this.btnBatchNo_Click);
            // 
            // cmbProductStatus
            // 
            this.cmbProductStatus.FormattingEnabled = true;
            this.cmbProductStatus.Items.AddRange(new object[] {
            "已返修",
            "待返修"});
            this.cmbProductStatus.Location = new System.Drawing.Point(933, 72);
            this.cmbProductStatus.Name = "cmbProductStatus";
            this.cmbProductStatus.Size = new System.Drawing.Size(100, 22);
            this.cmbProductStatus.TabIndex = 230;
            this.cmbProductStatus.Visible = false;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(864, 76);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 14);
            this.label11.TabIndex = 229;
            this.label11.Text = "产品状态";
            this.label11.Visible = false;
            // 
            // btnFindDepot
            // 
            this.btnFindDepot.BackColor = System.Drawing.Color.Transparent;
            this.btnFindDepot.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindDepot.BackgroundImage")));
            this.btnFindDepot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindDepot.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindDepot.Location = new System.Drawing.Point(275, 73);
            this.btnFindDepot.Name = "btnFindDepot";
            this.btnFindDepot.Size = new System.Drawing.Size(21, 21);
            this.btnFindDepot.TabIndex = 228;
            this.btnFindDepot.UseVisualStyleBackColor = false;
            this.btnFindDepot.Click += new System.EventHandler(this.btnFindDepot_Click);
            // 
            // txtProviderBatchNo
            // 
            this.txtProviderBatchNo.BackColor = System.Drawing.Color.White;
            this.txtProviderBatchNo.Location = new System.Drawing.Point(368, 41);
            this.txtProviderBatchNo.Name = "txtProviderBatchNo";
            this.txtProviderBatchNo.ReadOnly = true;
            this.txtProviderBatchNo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProviderBatchNo.Size = new System.Drawing.Size(172, 23);
            this.txtProviderBatchNo.TabIndex = 226;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(301, 45);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 225;
            this.label5.Text = "供方批次";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Location = new System.Drawing.Point(368, 101);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(665, 23);
            this.txtRemark.TabIndex = 222;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(301, 105);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 223;
            this.label6.Text = "备    注";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(88, 105);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(14, 14);
            this.lblAmount.TabIndex = 221;
            this.lblAmount.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 105);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 14);
            this.label8.TabIndex = 220;
            this.label8.Text = "总记录数：";
            // 
            // lblRecordRow
            // 
            this.lblRecordRow.AutoSize = true;
            this.lblRecordRow.Location = new System.Drawing.Point(206, 105);
            this.lblRecordRow.Name = "lblRecordRow";
            this.lblRecordRow.Size = new System.Drawing.Size(14, 14);
            this.lblRecordRow.TabIndex = 219;
            this.lblRecordRow.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(120, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 218;
            this.label4.Text = "当前记录行：";
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.White;
            this.txtUnit.Location = new System.Drawing.Point(617, 40);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUnit.Size = new System.Drawing.Size(172, 23);
            this.txtUnit.TabIndex = 217;
            // 
            // numReturnedCount
            // 
            this.numReturnedCount.BackColor = System.Drawing.Color.White;
            this.numReturnedCount.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.numReturnedCount.DecimalPlaces = 2;
            this.numReturnedCount.Location = new System.Drawing.Point(861, 40);
            this.numReturnedCount.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numReturnedCount.Name = "numReturnedCount";
            this.numReturnedCount.Size = new System.Drawing.Size(172, 23);
            this.numReturnedCount.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(794, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 215;
            this.label2.Text = "退 库 数";
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Location = new System.Drawing.Point(617, 10);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(172, 23);
            this.txtSpec.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(550, 14);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 209;
            this.label7.Text = "规    格";
            // 
            // txtProvider
            // 
            this.txtProvider.BackColor = System.Drawing.Color.White;
            this.txtProvider.Location = new System.Drawing.Point(861, 10);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(145, 23);
            this.txtProvider.TabIndex = 4;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(6, 44);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(70, 14);
            this.label26.TabIndex = 197;
            this.label26.Text = "批次/批号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(794, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 199;
            this.label3.Text = "供 应 商";
            // 
            // txtMaterialType
            // 
            this.txtMaterialType.BackColor = System.Drawing.Color.White;
            this.txtMaterialType.Location = new System.Drawing.Point(81, 72);
            this.txtMaterialType.Name = "txtMaterialType";
            this.txtMaterialType.ReadOnly = true;
            this.txtMaterialType.Size = new System.Drawing.Size(192, 23);
            this.txtMaterialType.TabIndex = 9;
            // 
            // txtLayer
            // 
            this.txtLayer.BackColor = System.Drawing.Color.White;
            this.txtLayer.Location = new System.Drawing.Point(747, 72);
            this.txtLayer.Name = "txtLayer";
            this.txtLayer.ReadOnly = true;
            this.txtLayer.Size = new System.Drawing.Size(110, 23);
            this.txtLayer.TabIndex = 12;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(680, 76);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(63, 14);
            this.label30.TabIndex = 207;
            this.label30.Text = "层    号";
            // 
            // txtColumn
            // 
            this.txtColumn.BackColor = System.Drawing.Color.White;
            this.txtColumn.Location = new System.Drawing.Point(561, 72);
            this.txtColumn.Name = "txtColumn";
            this.txtColumn.ReadOnly = true;
            this.txtColumn.Size = new System.Drawing.Size(110, 23);
            this.txtColumn.TabIndex = 11;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(494, 76);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(63, 14);
            this.label29.TabIndex = 205;
            this.label29.Text = "区    号";
            // 
            // txtShelf
            // 
            this.txtShelf.BackColor = System.Drawing.Color.White;
            this.txtShelf.Location = new System.Drawing.Point(368, 72);
            this.txtShelf.Name = "txtShelf";
            this.txtShelf.ReadOnly = true;
            this.txtShelf.Size = new System.Drawing.Size(110, 23);
            this.txtShelf.TabIndex = 10;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(301, 76);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(63, 14);
            this.label28.TabIndex = 203;
            this.label28.Text = "区    域";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(6, 76);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(63, 14);
            this.label27.TabIndex = 202;
            this.label27.Text = "材料类别";
            // 
            // btnFindCode
            // 
            this.btnFindCode.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindCode.BackgroundImage")));
            this.btnFindCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode.Location = new System.Drawing.Point(275, 11);
            this.btnFindCode.Name = "btnFindCode";
            this.btnFindCode.Size = new System.Drawing.Size(21, 21);
            this.btnFindCode.TabIndex = 1;
            this.btnFindCode.UseVisualStyleBackColor = false;
            this.btnFindCode.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(550, 44);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 14);
            this.label13.TabIndex = 196;
            this.label13.Text = "单    位";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(368, 10);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtName.Size = new System.Drawing.Size(172, 23);
            this.txtName.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(6, 14);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 14);
            this.label12.TabIndex = 194;
            this.label12.Text = "图号/型号";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.BackColor = System.Drawing.Color.White;
            this.txtBatchNo.Location = new System.Drawing.Point(81, 39);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ReadOnly = true;
            this.txtBatchNo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBatchNo.Size = new System.Drawing.Size(192, 23);
            this.txtBatchNo.TabIndex = 0;
            this.txtBatchNo.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtBatchNo_KeyPress);
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(81, 10);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(192, 23);
            this.txtCode.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(301, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 192;
            this.label1.Text = "物品名称";
            // 
            // panelBottom
            // 
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 539);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1041, 12);
            this.panelBottom.TabIndex = 27;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 40);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(8, 551);
            this.panelLeft.TabIndex = 38;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1057, 40);
            this.panel1.TabIndex = 24;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(414, 7);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "领料退库物品清单";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // FormMaterialListReturnedInTheDepot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 616);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMaterialListReturnedInTheDepot";
            this.Text = "选择领料退库单物品";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.FormMaterialListReturnedInTheDepot_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelMain.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numReturnedCount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMaterialType;
        private System.Windows.Forms.TextBox txtLayer;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox txtColumn;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtShelf;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Button btnFindCode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numReturnedCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRecordRow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripButton btnDeleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProviderBatchNo;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnFindDepot;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.ComboBox cmbProductStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripButton btnBatchCreate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Button btnBatchNo;
        private System.Windows.Forms.Button btnProvider;
        private System.Windows.Forms.ToolStripButton btnInputExcel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}