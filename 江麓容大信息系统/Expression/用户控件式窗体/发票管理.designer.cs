using UniversalControlLibrary;
namespace Expression
{
    partial class 发票管理
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btn_find = new System.Windows.Forms.Button();
            this.dtp_Start = new System.Windows.Forms.DateTimePicker();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtp_End = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProvider = new UniversalControlLibrary.TextBoxShow();
            this.label6 = new System.Windows.Forms.Label();
            this.txtPZH = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbInvoiceType = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInvoice = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.dgv_Mx = new System.Windows.Forms.DataGridView();
            this.GoodsCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Provider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Depot = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Count = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UnitPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OrderNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bill_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.befortax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.taxrat = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tax = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datetime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceCode_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PZH_1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dgv_Main = new System.Windows.Forms.DataGridView();
            this.InvoiceCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PZH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProviderName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxRatMe = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TaxPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SumPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InvoiceType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Mx)).BeginInit();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.toolStripSeparator1,
            this.btnAdd,
            this.toolStripSeparatorAdd,
            this.btnDelete,
            this.toolStripSeparatorDelete,
            this.btnUpdate});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 38;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.Image = global::UniversalControlLibrary.Properties.Resources.File2;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnNew.Size = new System.Drawing.Size(67, 22);
            this.btnNew.Tag = "ADD";
            this.btnNew.Text = "新建(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(67, 22);
            this.btnAdd.Tag = "ADD";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 22);
            this.btnDelete.Tag = "ADD";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(49, 22);
            this.btnUpdate.Tag = "ADD";
            this.btnUpdate.Text = "修改";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 37);
            this.panel1.TabIndex = 39;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(436, 6);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "发票管理";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox2);
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 62);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(984, 73);
            this.panel2.TabIndex = 40;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btn_find);
            this.groupBox2.Controls.Add(this.dtp_Start);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.dtp_End);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(504, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(480, 73);
            this.groupBox2.TabIndex = 207;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息查询";
            // 
            // btn_find
            // 
            this.btn_find.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btn_find.Location = new System.Drawing.Point(240, 27);
            this.btn_find.Name = "btn_find";
            this.btn_find.Size = new System.Drawing.Size(87, 25);
            this.btn_find.TabIndex = 210;
            this.btn_find.Text = "查询";
            this.btn_find.UseVisualStyleBackColor = true;
            this.btn_find.Click += new System.EventHandler(this.btn_find_Click);
            // 
            // dtp_Start
            // 
            this.dtp_Start.Location = new System.Drawing.Point(33, 15);
            this.dtp_Start.Name = "dtp_Start";
            this.dtp_Start.Size = new System.Drawing.Size(182, 23);
            this.dtp_Start.TabIndex = 206;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(5, 49);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 209;
            this.label4.Text = "至";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(7, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(17, 12);
            this.label5.TabIndex = 207;
            this.label5.Text = "从";
            // 
            // dtp_End
            // 
            this.dtp_End.Location = new System.Drawing.Point(33, 45);
            this.dtp_End.Name = "dtp_End";
            this.dtp_End.Size = new System.Drawing.Size(182, 23);
            this.dtp_End.TabIndex = 208;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtPZH);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbInvoiceType);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtInvoice);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(504, 73);
            this.groupBox1.TabIndex = 206;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息设置";
            // 
            // txtProvider
            // 
            this.txtProvider.EditingControlDataGridView = null;
            this.txtProvider.EditingControlFormattedValue = "";
            this.txtProvider.EditingControlRowIndex = 0;
            this.txtProvider.EditingControlValueChanged = false;
            this.txtProvider.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.txtProvider.Location = new System.Drawing.Point(308, 45);
            this.txtProvider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ShowResultForm = true;
            this.txtProvider.Size = new System.Drawing.Size(164, 23);
            this.txtProvider.TabIndex = 210;
            this.txtProvider.TabStop = false;
            this.txtProvider.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProvider_OnCompleteSearch);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(242, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 209;
            this.label6.Text = "凭 证 号";
            // 
            // txtPZH
            // 
            this.txtPZH.Location = new System.Drawing.Point(308, 15);
            this.txtPZH.Name = "txtPZH";
            this.txtPZH.Size = new System.Drawing.Size(163, 23);
            this.txtPZH.TabIndex = 208;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(8, 49);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 207;
            this.label3.Text = "发票类型";
            // 
            // cmbInvoiceType
            // 
            this.cmbInvoiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInvoiceType.FormattingEnabled = true;
            this.cmbInvoiceType.Items.AddRange(new object[] {
            "普通发票",
            "专用发票"});
            this.cmbInvoiceType.Location = new System.Drawing.Point(74, 46);
            this.cmbInvoiceType.Name = "cmbInvoiceType";
            this.cmbInvoiceType.Size = new System.Drawing.Size(163, 21);
            this.cmbInvoiceType.TabIndex = 206;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(242, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 204;
            this.label2.Text = "供 应 商";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 202;
            this.label1.Text = "发 票 号";
            // 
            // txtInvoice
            // 
            this.txtInvoice.Location = new System.Drawing.Point(74, 15);
            this.txtInvoice.Name = "txtInvoice";
            this.txtInvoice.Size = new System.Drawing.Size(163, 23);
            this.txtInvoice.TabIndex = 201;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.panel5);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.panel4);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 135);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(984, 396);
            this.panel3.TabIndex = 41;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.dgv_Mx);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(802, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(182, 396);
            this.panel5.TabIndex = 2;
            // 
            // dgv_Mx
            // 
            this.dgv_Mx.AllowUserToAddRows = false;
            this.dgv_Mx.AllowUserToDeleteRows = false;
            this.dgv_Mx.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Mx.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GoodsCode,
            this.GoodsName,
            this.Spec,
            this.Provider,
            this.Depot,
            this.Count,
            this.Unit,
            this.UnitPrice,
            this.金额,
            this.OrderNumber,
            this.Bill_ID,
            this.BatchNo,
            this.Type,
            this.befortax,
            this.taxrat,
            this.tax,
            this.datetime,
            this.ID,
            this.InvoiceCode_1,
            this.GoodsID,
            this.PZH_1});
            this.dgv_Mx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Mx.Location = new System.Drawing.Point(0, 0);
            this.dgv_Mx.Name = "dgv_Mx";
            this.dgv_Mx.ReadOnly = true;
            this.dgv_Mx.RowHeadersWidth = 15;
            this.dgv_Mx.RowTemplate.Height = 23;
            this.dgv_Mx.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Mx.Size = new System.Drawing.Size(182, 396);
            this.dgv_Mx.TabIndex = 0;
            // 
            // GoodsCode
            // 
            this.GoodsCode.DataPropertyName = "GoodsCode";
            this.GoodsCode.HeaderText = "图号型号";
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
            // 
            // Provider
            // 
            this.Provider.DataPropertyName = "Provider";
            this.Provider.HeaderText = "供应商";
            this.Provider.Name = "Provider";
            this.Provider.ReadOnly = true;
            this.Provider.Visible = false;
            // 
            // Depot
            // 
            this.Depot.DataPropertyName = "Depot";
            this.Depot.HeaderText = "类别";
            this.Depot.Name = "Depot";
            this.Depot.ReadOnly = true;
            this.Depot.Visible = false;
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
            this.Unit.Width = 20;
            // 
            // UnitPrice
            // 
            this.UnitPrice.DataPropertyName = "UnitPrice";
            this.UnitPrice.HeaderText = "单价";
            this.UnitPrice.Name = "UnitPrice";
            this.UnitPrice.ReadOnly = true;
            this.UnitPrice.Width = 75;
            // 
            // 金额
            // 
            this.金额.DataPropertyName = "Price";
            this.金额.HeaderText = "金额";
            this.金额.Name = "金额";
            this.金额.ReadOnly = true;
            // 
            // OrderNumber
            // 
            this.OrderNumber.DataPropertyName = "OrderNumber";
            this.OrderNumber.HeaderText = "订单号";
            this.OrderNumber.Name = "OrderNumber";
            this.OrderNumber.ReadOnly = true;
            this.OrderNumber.Visible = false;
            // 
            // Bill_ID
            // 
            this.Bill_ID.DataPropertyName = "Bill_ID";
            this.Bill_ID.HeaderText = "入库单号";
            this.Bill_ID.Name = "Bill_ID";
            this.Bill_ID.ReadOnly = true;
            // 
            // BatchNo
            // 
            this.BatchNo.DataPropertyName = "BatchNo";
            this.BatchNo.HeaderText = "批次号";
            this.BatchNo.Name = "BatchNo";
            this.BatchNo.ReadOnly = true;
            // 
            // Type
            // 
            this.Type.DataPropertyName = "InvoiceType";
            this.Type.HeaderText = "发票类型";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Visible = false;
            // 
            // befortax
            // 
            this.befortax.DataPropertyName = "befortax";
            this.befortax.HeaderText = "befortax";
            this.befortax.Name = "befortax";
            this.befortax.ReadOnly = true;
            this.befortax.Visible = false;
            // 
            // taxrat
            // 
            this.taxrat.DataPropertyName = "taxrat";
            this.taxrat.HeaderText = "taxrat";
            this.taxrat.Name = "taxrat";
            this.taxrat.ReadOnly = true;
            this.taxrat.Visible = false;
            // 
            // tax
            // 
            this.tax.DataPropertyName = "tax";
            this.tax.HeaderText = "tax";
            this.tax.Name = "tax";
            this.tax.ReadOnly = true;
            this.tax.Visible = false;
            // 
            // datetime
            // 
            this.datetime.DataPropertyName = "date";
            this.datetime.HeaderText = "datetime";
            this.datetime.Name = "datetime";
            this.datetime.ReadOnly = true;
            this.datetime.Visible = false;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // InvoiceCode_1
            // 
            this.InvoiceCode_1.DataPropertyName = "InvoiceCode";
            this.InvoiceCode_1.HeaderText = "InvoiceCode";
            this.InvoiceCode_1.Name = "InvoiceCode_1";
            this.InvoiceCode_1.ReadOnly = true;
            this.InvoiceCode_1.Visible = false;
            // 
            // GoodsID
            // 
            this.GoodsID.DataPropertyName = "GoodsID";
            this.GoodsID.HeaderText = "GoodsID";
            this.GoodsID.Name = "GoodsID";
            this.GoodsID.ReadOnly = true;
            this.GoodsID.Visible = false;
            // 
            // PZH_1
            // 
            this.PZH_1.DataPropertyName = "PZH";
            this.PZH_1.HeaderText = "PZH";
            this.PZH_1.Name = "PZH_1";
            this.PZH_1.ReadOnly = true;
            this.PZH_1.Visible = false;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(799, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 396);
            this.splitter1.TabIndex = 1;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dgv_Main);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(799, 396);
            this.panel4.TabIndex = 0;
            // 
            // dgv_Main
            // 
            this.dgv_Main.AllowUserToAddRows = false;
            this.dgv_Main.AllowUserToDeleteRows = false;
            this.dgv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.InvoiceCode,
            this.PZH,
            this.ProviderName,
            this.Price,
            this.TaxRatMe,
            this.TaxPrice,
            this.SumPrice,
            this.InvoiceType,
            this.Date,
            this.供应商});
            this.dgv_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Main.Location = new System.Drawing.Point(0, 0);
            this.dgv_Main.Name = "dgv_Main";
            this.dgv_Main.ReadOnly = true;
            this.dgv_Main.RowHeadersWidth = 52;
            this.dgv_Main.RowTemplate.Height = 23;
            this.dgv_Main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Main.Size = new System.Drawing.Size(799, 396);
            this.dgv_Main.TabIndex = 1;
            this.dgv_Main.DoubleClick += new System.EventHandler(this.dgv_Main_DoubleClick);
            this.dgv_Main.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_Main_RowPostPaint);
            this.dgv_Main.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Main_CellEnter);
            // 
            // InvoiceCode
            // 
            this.InvoiceCode.DataPropertyName = "InvoiceCode";
            this.InvoiceCode.HeaderText = "发票号";
            this.InvoiceCode.Name = "InvoiceCode";
            this.InvoiceCode.ReadOnly = true;
            // 
            // PZH
            // 
            this.PZH.DataPropertyName = "PZH";
            this.PZH.HeaderText = "凭证号";
            this.PZH.Name = "PZH";
            this.PZH.ReadOnly = true;
            // 
            // ProviderName
            // 
            this.ProviderName.DataPropertyName = "ProviderName";
            this.ProviderName.HeaderText = "供应商名称";
            this.ProviderName.Name = "ProviderName";
            this.ProviderName.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "金额";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            // 
            // TaxRatMe
            // 
            this.TaxRatMe.DataPropertyName = "TaxRat";
            this.TaxRatMe.HeaderText = "税率";
            this.TaxRatMe.Name = "TaxRatMe";
            this.TaxRatMe.ReadOnly = true;
            // 
            // TaxPrice
            // 
            this.TaxPrice.DataPropertyName = "Tax";
            this.TaxPrice.HeaderText = "税额";
            this.TaxPrice.Name = "TaxPrice";
            this.TaxPrice.ReadOnly = true;
            // 
            // SumPrice
            // 
            this.SumPrice.DataPropertyName = "SumPrice";
            this.SumPrice.HeaderText = "合计金额";
            this.SumPrice.Name = "SumPrice";
            this.SumPrice.ReadOnly = true;
            // 
            // InvoiceType
            // 
            this.InvoiceType.DataPropertyName = "InvoiceType";
            this.InvoiceType.HeaderText = "发票类型";
            this.InvoiceType.Name = "InvoiceType";
            this.InvoiceType.ReadOnly = true;
            this.InvoiceType.Width = 80;
            // 
            // Date
            // 
            this.Date.DataPropertyName = "Date";
            this.Date.HeaderText = "日期";
            this.Date.Name = "Date";
            this.Date.ReadOnly = true;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "Provider";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.ReadOnly = true;
            this.供应商.Visible = false;
            // 
            // 发票管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 531);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "发票管理";
            this.Load += new System.EventHandler(this.发票管理_Load);
            this.Resize += new System.EventHandler(this.发票管理_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel5.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Mx)).EndInit();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Main)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.DataGridView dgv_Mx;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dgv_Main;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbInvoiceType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInvoice;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btn_find;
        private System.Windows.Forms.DateTimePicker dtp_Start;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtp_End;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtPZH;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn PZH;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProviderName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxRatMe;
        private System.Windows.Forms.DataGridViewTextBoxColumn TaxPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn SumPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceType;
        private System.Windows.Forms.DataGridViewTextBoxColumn Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private TextBoxShow txtProvider;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Provider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Depot;
        private System.Windows.Forms.DataGridViewTextBoxColumn Count;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn UnitPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn 金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn OrderNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bill_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn befortax;
        private System.Windows.Forms.DataGridViewTextBoxColumn taxrat;
        private System.Windows.Forms.DataGridViewTextBoxColumn tax;
        private System.Windows.Forms.DataGridViewTextBoxColumn datetime;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn InvoiceCode_1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsID;
        private System.Windows.Forms.DataGridViewTextBoxColumn PZH_1;
    }
}
