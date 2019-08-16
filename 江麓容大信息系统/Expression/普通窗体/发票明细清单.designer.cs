namespace Expression
{
    partial class 发票明细清单
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnOutExcel = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripLabel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbSumTax = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbSumPrice = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.btnFindProvider = new System.Windows.Forms.Button();
            this.txtOrderNumber = new System.Windows.Forms.TextBox();
            this.txtProvide = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dgv_InvoiceShow = new System.Windows.Forms.DataGridView();
            this.Bill_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxRat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BeforTax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Provider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PZH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.删除此记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btn_find = new System.Windows.Forms.Button();
            this.dtp_Start = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dtp_End = new System.Windows.Forms.DateTimePicker();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgv_OrderGoods = new System.Windows.Forms.DataGridView();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.含税价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.类别 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip3 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加记录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgv_OrderInfo = new System.Windows.Forms.DataGridView();
            this.订单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InvoiceShow)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OrderGoods)).BeginInit();
            this.contextMenuStrip3.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OrderInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnOutExcel,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 39;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSave.Size = new System.Drawing.Size(67, 22);
            this.btnSave.Tag = "ADD";
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnOutExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(121, 22);
            this.btnOutExcel.Tag = "view";
            this.btnOutExcel.Text = "导成Excel文件(&O)";
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 22);
            this.btnClose.Tag = "view";
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbSumTax);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.lbSumPrice);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.btnFindProvider);
            this.panel2.Controls.Add(this.txtOrderNumber);
            this.panel2.Controls.Add(this.txtProvide);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.dgv_InvoiceShow);
            this.panel2.Controls.Add(this.btn_find);
            this.panel2.Controls.Add(this.dtp_Start);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.dtp_End);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1016, 357);
            this.panel2.TabIndex = 41;
            // 
            // lbSumTax
            // 
            this.lbSumTax.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSumTax.AutoSize = true;
            this.lbSumTax.Location = new System.Drawing.Point(893, 302);
            this.lbSumTax.Name = "lbSumTax";
            this.lbSumTax.Size = new System.Drawing.Size(0, 14);
            this.lbSumTax.TabIndex = 201;
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(824, 302);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 200;
            this.label7.Text = "发票税额";
            // 
            // lbSumPrice
            // 
            this.lbSumPrice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lbSumPrice.AutoSize = true;
            this.lbSumPrice.Location = new System.Drawing.Point(893, 263);
            this.lbSumPrice.Name = "lbSumPrice";
            this.lbSumPrice.Size = new System.Drawing.Size(0, 14);
            this.lbSumPrice.TabIndex = 199;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(823, 262);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 198;
            this.label5.Text = "发票金额";
            // 
            // btnFindProvider
            // 
            this.btnFindProvider.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFindProvider.BackColor = System.Drawing.Color.Transparent;
            this.btnFindProvider.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindProvider.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindProvider.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindProvider.Location = new System.Drawing.Point(985, 100);
            this.btnFindProvider.Name = "btnFindProvider";
            this.btnFindProvider.Size = new System.Drawing.Size(24, 25);
            this.btnFindProvider.TabIndex = 197;
            this.btnFindProvider.UseVisualStyleBackColor = false;
            this.btnFindProvider.Click += new System.EventHandler(this.btnFindProvider_Click);
            // 
            // txtOrderNumber
            // 
            this.txtOrderNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOrderNumber.Location = new System.Drawing.Point(883, 148);
            this.txtOrderNumber.Name = "txtOrderNumber";
            this.txtOrderNumber.Size = new System.Drawing.Size(126, 23);
            this.txtOrderNumber.TabIndex = 51;
            // 
            // txtProvide
            // 
            this.txtProvide.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProvide.Location = new System.Drawing.Point(883, 102);
            this.txtProvide.Name = "txtProvide";
            this.txtProvide.Size = new System.Drawing.Size(95, 23);
            this.txtProvide.TabIndex = 50;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(823, 152);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 49;
            this.label4.Text = "订 单 号";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(823, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 48;
            this.label3.Text = "供 应 商";
            // 
            // dgv_InvoiceShow
            // 
            this.dgv_InvoiceShow.AllowUserToAddRows = false;
            this.dgv_InvoiceShow.AllowUserToDeleteRows = false;
            this.dgv_InvoiceShow.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgv_InvoiceShow.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_InvoiceShow.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Bill_ID,
            this.GoodsCode,
            this.GoodsName,
            this.Spec,
            this.Count,
            this.Unit,
            this.UnitPrice,
            this.Price,
            this.TaxRat,
            this.Tax,
            this.BeforTax,
            this.BatchNo,
            this.InvoiceType,
            this.OrderNumber,
            this.ID,
            this.InvoiceCode,
            this.Provider,
            this.Depot,
            this.Date,
            this.PZH});
            this.dgv_InvoiceShow.ContextMenuStrip = this.contextMenuStrip1;
            this.dgv_InvoiceShow.Location = new System.Drawing.Point(0, 0);
            this.dgv_InvoiceShow.Name = "dgv_InvoiceShow";
            this.dgv_InvoiceShow.RowHeadersWidth = 15;
            this.dgv_InvoiceShow.RowTemplate.Height = 23;
            this.dgv_InvoiceShow.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_InvoiceShow.Size = new System.Drawing.Size(815, 351);
            this.dgv_InvoiceShow.TabIndex = 0;
            this.dgv_InvoiceShow.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_InvoiceShow_CellEndEdit);
            this.dgv_InvoiceShow.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_InvoiceShow_DataBindingComplete);
            // 
            // Bill_ID
            // 
            this.Bill_ID.DataPropertyName = "Bill_ID";
            this.Bill_ID.HeaderText = "入库单号";
            this.Bill_ID.Name = "Bill_ID";
            this.Bill_ID.ReadOnly = true;
            this.Bill_ID.Width = 120;
            // 
            // GoodsCode
            // 
            this.GoodsCode.DataPropertyName = "GoodsCode";
            this.GoodsCode.HeaderText = "图型型号";
            this.GoodsCode.Name = "GoodsCode";
            this.GoodsCode.ReadOnly = true;
            // 
            // GoodsName
            // 
            this.GoodsName.DataPropertyName = "GoodsName";
            this.GoodsName.HeaderText = "物品名称";
            this.GoodsName.Name = "GoodsName";
            this.GoodsName.ReadOnly = true;
            // 
            // Spec
            // 
            this.Spec.DataPropertyName = "Spec";
            this.Spec.HeaderText = "规格";
            this.Spec.Name = "Spec";
            this.Spec.ReadOnly = true;
            this.Spec.Width = 140;
            // 
            // Count
            // 
            this.Count.DataPropertyName = "Count";
            this.Count.HeaderText = "数量";
            this.Count.Name = "Count";
            this.Count.ReadOnly = true;
            this.Count.Width = 60;
            // 
            // Unit
            // 
            this.Unit.DataPropertyName = "Unit";
            this.Unit.HeaderText = "单位";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Width = 30;
            // 
            // UnitPrice
            // 
            this.UnitPrice.DataPropertyName = "UnitPrice";
            this.UnitPrice.HeaderText = "单价";
            this.UnitPrice.Name = "UnitPrice";
            this.UnitPrice.ReadOnly = true;
            this.UnitPrice.Width = 60;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "金额";
            this.Price.Name = "Price";
            this.Price.Width = 80;
            // 
            // TaxRat
            // 
            this.TaxRat.DataPropertyName = "TaxRat";
            this.TaxRat.HeaderText = "税率(%)";
            this.TaxRat.Name = "TaxRat";
            this.TaxRat.ReadOnly = true;
            this.TaxRat.Width = 30;
            // 
            // Tax
            // 
            this.Tax.DataPropertyName = "Tax";
            this.Tax.HeaderText = "税额";
            this.Tax.Name = "Tax";
            this.Tax.Width = 80;
            // 
            // BeforTax
            // 
            this.BeforTax.DataPropertyName = "BeforTax";
            this.BeforTax.HeaderText = "含税价";
            this.BeforTax.Name = "BeforTax";
            this.BeforTax.ReadOnly = true;
            // 
            // BatchNo
            // 
            this.BatchNo.DataPropertyName = "BatchNo";
            this.BatchNo.HeaderText = "批次号";
            this.BatchNo.Name = "BatchNo";
            this.BatchNo.ReadOnly = true;
            // 
            // InvoiceType
            // 
            this.InvoiceType.DataPropertyName = "InvoiceType";
            this.InvoiceType.HeaderText = "发票类型";
            this.InvoiceType.Name = "InvoiceType";
            this.InvoiceType.ReadOnly = true;
            this.InvoiceType.Visible = false;
            // 
            // OrderNumber
            // 
            this.OrderNumber.DataPropertyName = "OrderNumber";
            this.OrderNumber.HeaderText = "订单号";
            this.OrderNumber.Name = "OrderNumber";
            this.OrderNumber.ReadOnly = true;
            this.OrderNumber.Visible = false;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // InvoiceCode
            // 
            this.InvoiceCode.DataPropertyName = "InvoiceCode";
            this.InvoiceCode.HeaderText = "发票号";
            this.InvoiceCode.Name = "InvoiceCode";
            this.InvoiceCode.Visible = false;
            // 
            // Provider
            // 
            this.Provider.DataPropertyName = "Provider";
            this.Provider.HeaderText = "供应商";
            this.Provider.Name = "Provider";
            this.Provider.Visible = false;
            // 
            // Depot
            // 
            this.Depot.DataPropertyName = "Depot";
            this.Depot.HeaderText = "类别";
            this.Depot.Name = "Depot";
            this.Depot.Visible = false;
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Date";
            this.Date.HeaderText = "date";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            this.Date.Visible = false;
            // 
            // PZH
            // 
            this.PZH.DataPropertyName = "PZH";
            this.PZH.HeaderText = "PZH";
            this.PZH.Name = "PZH";
            this.PZH.ReadOnly = true;
            this.PZH.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除此记录ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 26);
            // 
            // 删除此记录ToolStripMenuItem
            // 
            this.删除此记录ToolStripMenuItem.Name = "删除此记录ToolStripMenuItem";
            this.删除此记录ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.删除此记录ToolStripMenuItem.Text = "删除";
            this.删除此记录ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // btn_find
            // 
            this.btn_find.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btn_find.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_find.Location = new System.Drawing.Point(883, 205);
            this.btn_find.Name = "btn_find";
            this.btn_find.Size = new System.Drawing.Size(125, 27);
            this.btn_find.TabIndex = 47;
            this.btn_find.Text = "查询";
            this.btn_find.UseVisualStyleBackColor = true;
            this.btn_find.Click += new System.EventHandler(this.btn_find_Click);
            // 
            // dtp_Start
            // 
            this.dtp_Start.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_Start.Location = new System.Drawing.Point(883, 16);
            this.dtp_Start.Name = "dtp_Start";
            this.dtp_Start.Size = new System.Drawing.Size(126, 23);
            this.dtp_Start.TabIndex = 43;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(861, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 46;
            this.label2.Text = "至";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(863, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 12);
            this.label1.TabIndex = 44;
            this.label1.Text = "从";
            // 
            // dtp_End
            // 
            this.dtp_End.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dtp_End.Location = new System.Drawing.Point(883, 61);
            this.dtp_End.Name = "dtp_End";
            this.dtp_End.Size = new System.Drawing.Size(126, 23);
            this.dtp_End.TabIndex = 45;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 382);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1016, 354);
            this.panel3.TabIndex = 42;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgv_OrderGoods);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(440, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(576, 354);
            this.panel5.TabIndex = 3;
            // 
            // dgv_OrderGoods
            // 
            this.dgv_OrderGoods.AllowUserToAddRows = false;
            this.dgv_OrderGoods.AllowUserToDeleteRows = false;
            this.dgv_OrderGoods.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_OrderGoods.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.图号型号,
            this.物品名称,
            this.规格,
            this.单价,
            this.数量,
            this.单位,
            this.含税价,
            this.类别,
            this.批次号});
            this.dgv_OrderGoods.ContextMenuStrip = this.contextMenuStrip3;
            this.dgv_OrderGoods.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_OrderGoods.Location = new System.Drawing.Point(0, 0);
            this.dgv_OrderGoods.Name = "dgv_OrderGoods";
            this.dgv_OrderGoods.ReadOnly = true;
            this.dgv_OrderGoods.RowHeadersWidth = 15;
            this.dgv_OrderGoods.RowTemplate.Height = 23;
            this.dgv_OrderGoods.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_OrderGoods.Size = new System.Drawing.Size(576, 354);
            this.dgv_OrderGoods.TabIndex = 0;
            this.dgv_OrderGoods.DoubleClick += new System.EventHandler(this.dgv_OrderGoods_DoubleClick);
            this.dgv_OrderGoods.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgv_OrderGoods_DataBindingComplete);
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "物品名称";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            this.物品名称.Width = 110;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 单价
            // 
            this.单价.DataPropertyName = "单价";
            this.单价.HeaderText = "单价";
            this.单价.Name = "单价";
            this.单价.ReadOnly = true;
            // 
            // 数量
            // 
            this.数量.DataPropertyName = "数量";
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.ReadOnly = true;
            this.数量.Width = 60;
            // 
            // 单位
            // 
            this.单位.DataPropertyName = "单位";
            this.单位.HeaderText = "单位";
            this.单位.Name = "单位";
            this.单位.ReadOnly = true;
            this.单位.Width = 40;
            // 
            // 含税价
            // 
            this.含税价.DataPropertyName = "含税价";
            this.含税价.HeaderText = "含税价";
            this.含税价.Name = "含税价";
            this.含税价.ReadOnly = true;
            // 
            // 类别
            // 
            this.类别.DataPropertyName = "类别";
            this.类别.HeaderText = "类别";
            this.类别.Name = "类别";
            this.类别.ReadOnly = true;
            this.类别.Visible = false;
            // 
            // 批次号
            // 
            this.批次号.DataPropertyName = "批次号";
            this.批次号.HeaderText = "批次号";
            this.批次号.Name = "批次号";
            this.批次号.ReadOnly = true;
            // 
            // contextMenuStrip3
            // 
            this.contextMenuStrip3.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加记录ToolStripMenuItem});
            this.contextMenuStrip3.Name = "contextMenuStrip3";
            this.contextMenuStrip3.Size = new System.Drawing.Size(95, 26);
            // 
            // 添加记录ToolStripMenuItem
            // 
            this.添加记录ToolStripMenuItem.Name = "添加记录ToolStripMenuItem";
            this.添加记录ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.添加记录ToolStripMenuItem.Text = "添加";
            this.添加记录ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(437, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 354);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgv_OrderInfo);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(437, 354);
            this.panel4.TabIndex = 0;
            // 
            // dgv_OrderInfo
            // 
            this.dgv_OrderInfo.AllowUserToAddRows = false;
            this.dgv_OrderInfo.AllowUserToDeleteRows = false;
            this.dgv_OrderInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_OrderInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.订单号,
            this.入库单号,
            this.供应商名称,
            this.日期,
            this.供应商});
            this.dgv_OrderInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_OrderInfo.Location = new System.Drawing.Point(0, 0);
            this.dgv_OrderInfo.Name = "dgv_OrderInfo";
            this.dgv_OrderInfo.ReadOnly = true;
            this.dgv_OrderInfo.RowHeadersWidth = 15;
            this.dgv_OrderInfo.RowTemplate.Height = 23;
            this.dgv_OrderInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_OrderInfo.Size = new System.Drawing.Size(437, 354);
            this.dgv_OrderInfo.TabIndex = 0;
            this.dgv_OrderInfo.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_OrderInfo_CellEnter);
            // 
            // 订单号
            // 
            this.订单号.DataPropertyName = "订单号";
            this.订单号.HeaderText = "订单号";
            this.订单号.Name = "订单号";
            this.订单号.ReadOnly = true;
            // 
            // 入库单号
            // 
            this.入库单号.DataPropertyName = "入库单号";
            this.入库单号.HeaderText = "入库单号";
            this.入库单号.Name = "入库单号";
            this.入库单号.ReadOnly = true;
            // 
            // 供应商名称
            // 
            this.供应商名称.DataPropertyName = "ProviderName";
            this.供应商名称.HeaderText = "供应商名称";
            this.供应商名称.Name = "供应商名称";
            this.供应商名称.ReadOnly = true;
            // 
            // 日期
            // 
            this.日期.DataPropertyName = "日期";
            this.日期.HeaderText = "日期";
            this.日期.Name = "日期";
            this.日期.ReadOnly = true;
            this.日期.Width = 120;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.ReadOnly = true;
            this.供应商.Visible = false;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.Title = "将查询结果保存成 EXCEL 文件";
            // 
            // 发票明细清单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "发票明细清单";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发票明细清单";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.发票明细清单_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_InvoiceShow)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OrderGoods)).EndInit();
            this.contextMenuStrip3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_OrderInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgv_OrderGoods;
        private System.Windows.Forms.DataGridView dgv_OrderInfo;
        private System.Windows.Forms.DataGridView dgv_InvoiceShow;
        private System.Windows.Forms.DateTimePicker dtp_Start;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtp_End;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_find;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 删除此记录ToolStripMenuItem;
        private System.Windows.Forms.TextBox txtOrderNumber;
        private System.Windows.Forms.TextBox txtProvide;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripLabel btnClose;
        private System.Windows.Forms.Button btnFindProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip3;
        private System.Windows.Forms.ToolStripMenuItem 添加记录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton btnOutExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbSumTax;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbSumPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bill_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxRat;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn BeforTax;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Provider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn PZH;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 含税价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 类别;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批次号;
    }
}