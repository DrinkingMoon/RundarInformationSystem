namespace Form_Economic_Purchase
{
    partial class 采购结算单明细
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtAccoutingDocumentNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbInvoicePrice = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbTotalTaxPrice = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lbInPutPrice = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.txtSettlementCompany = new UniversalControlLibrary.TextBoxShow();
            this.label8 = new System.Windows.Forms.Label();
            this.btnInPutBill = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btnInvoice = new System.Windows.Forms.Button();
            this.cmbTaxRate = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbBillType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtInvoiceInfo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbInvoiceType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.入库单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件图号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库单价_不含税 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单件委托材料 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单件加工费 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.委托加工材料 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票单价 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.税额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.价税合计 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.执行合同号_申请单 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.差异说明 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.groupBox2);
            this.customPanel1.Controls.Add(this.panel1);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(846, 552);
            this.customPanel1.TabIndex = 374;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.customDataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 221);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(846, 331);
            this.groupBox2.TabIndex = 17;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "明细";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.入库单号,
            this.零件图号,
            this.零件名称,
            this.规格,
            this.批次号,
            this.入库数量,
            this.入库单价_不含税,
            this.单件委托材料,
            this.单件加工费,
            this.入库金额,
            this.委托加工材料,
            this.发票单价,
            this.税额,
            this.价税合计,
            this.发票金额,
            this.执行合同号_申请单,
            this.差异说明,
            this.物品ID,
            this.单据号,
            this.序号});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 17);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(840, 311);
            this.customDataGridView1.TabIndex = 1;
            this.customDataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.customDataGridView1_RowsAdded);
            this.customDataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellEndEdit);
            this.customDataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.customDataGridView1_RowsRemoved);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtAccoutingDocumentNo);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 186);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(846, 35);
            this.panel1.TabIndex = 16;
            // 
            // txtAccoutingDocumentNo
            // 
            this.txtAccoutingDocumentNo.Location = new System.Drawing.Point(75, 7);
            this.txtAccoutingDocumentNo.Name = "txtAccoutingDocumentNo";
            this.txtAccoutingDocumentNo.Size = new System.Drawing.Size(241, 21);
            this.txtAccoutingDocumentNo.TabIndex = 375;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(13, 12);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 374;
            this.label12.Text = "凭 证 号";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbInvoicePrice);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.lbTotalTaxPrice);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.lbInPutPrice);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.txtSettlementCompany);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.btnInPutBill);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.btnInvoice);
            this.groupBox1.Controls.Add(this.cmbTaxRate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbBillType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtInvoiceInfo);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cmbInvoiceType);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(846, 186);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据";
            // 
            // lbInvoicePrice
            // 
            this.lbInvoicePrice.AutoSize = true;
            this.lbInvoicePrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInvoicePrice.Location = new System.Drawing.Point(600, 158);
            this.lbInvoicePrice.Name = "lbInvoicePrice";
            this.lbInvoicePrice.Size = new System.Drawing.Size(29, 12);
            this.lbInvoicePrice.TabIndex = 378;
            this.lbInvoicePrice.Text = "0.00";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(495, 158);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 12);
            this.label16.TabIndex = 377;
            this.label16.Text = "发票金额合计:";
            // 
            // lbTotalTaxPrice
            // 
            this.lbTotalTaxPrice.AutoSize = true;
            this.lbTotalTaxPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbTotalTaxPrice.Location = new System.Drawing.Point(781, 158);
            this.lbTotalTaxPrice.Name = "lbTotalTaxPrice";
            this.lbTotalTaxPrice.Size = new System.Drawing.Size(29, 12);
            this.lbTotalTaxPrice.TabIndex = 376;
            this.lbTotalTaxPrice.Text = "0.00";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(676, 158);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(71, 12);
            this.label15.TabIndex = 375;
            this.label15.Text = "发票总金额:";
            // 
            // lbInPutPrice
            // 
            this.lbInPutPrice.AutoSize = true;
            this.lbInPutPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInPutPrice.Location = new System.Drawing.Point(405, 158);
            this.lbInPutPrice.Name = "lbInPutPrice";
            this.lbInPutPrice.Size = new System.Drawing.Size(29, 12);
            this.lbInPutPrice.TabIndex = 374;
            this.lbInPutPrice.Text = "0.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(304, 158);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 12);
            this.label9.TabIndex = 373;
            this.label9.Text = "入库总金额:";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(181, 153);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 372;
            this.btnDelete.Text = "删除选中项";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // txtSettlementCompany
            // 
            this.txtSettlementCompany.DataResult = null;
            this.txtSettlementCompany.DataTableResult = null;
            this.txtSettlementCompany.EditingControlDataGridView = null;
            this.txtSettlementCompany.EditingControlFormattedValue = "";
            this.txtSettlementCompany.EditingControlRowIndex = 0;
            this.txtSettlementCompany.EditingControlValueChanged = true;
            this.txtSettlementCompany.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.txtSettlementCompany.IsMultiSelect = false;
            this.txtSettlementCompany.Location = new System.Drawing.Point(75, 57);
            this.txtSettlementCompany.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSettlementCompany.Name = "txtSettlementCompany";
            this.txtSettlementCompany.ShowResultForm = true;
            this.txtSettlementCompany.Size = new System.Drawing.Size(253, 21);
            this.txtSettlementCompany.StrEndSql = null;
            this.txtSettlementCompany.TabIndex = 371;
            this.txtSettlementCompany.TabStop = false;
            this.txtSettlementCompany.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtSettlementCompany_OnCompleteSearch);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(777, 60);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(11, 12);
            this.label8.TabIndex = 370;
            this.label8.Text = "%";
            // 
            // btnInPutBill
            // 
            this.btnInPutBill.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInPutBill.Location = new System.Drawing.Point(111, 153);
            this.btnInPutBill.Name = "btnInPutBill";
            this.btnInPutBill.Size = new System.Drawing.Size(39, 23);
            this.btnInPutBill.TabIndex = 369;
            this.btnInPutBill.Text = "...";
            this.btnInPutBill.UseVisualStyleBackColor = true;
            this.btnInPutBill.Click += new System.EventHandler(this.btnInPutBill_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(13, 158);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 12);
            this.label7.TabIndex = 368;
            this.label7.Text = "选择入库单号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(85, 123);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 367;
            this.label6.Text = "发票号码";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(85, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 366;
            this.label3.Text = "发票日期";
            // 
            // btnInvoice
            // 
            this.btnInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInvoice.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnInvoice.Location = new System.Drawing.Point(794, 104);
            this.btnInvoice.Name = "btnInvoice";
            this.btnInvoice.Size = new System.Drawing.Size(39, 23);
            this.btnInvoice.TabIndex = 365;
            this.btnInvoice.Text = "...";
            this.btnInvoice.UseVisualStyleBackColor = true;
            this.btnInvoice.Click += new System.EventHandler(this.btnInvoice_Click);
            // 
            // cmbTaxRate
            // 
            this.cmbTaxRate.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaxRate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbTaxRate.FormattingEnabled = true;
            this.cmbTaxRate.Location = new System.Drawing.Point(676, 57);
            this.cmbTaxRate.Name = "cmbTaxRate";
            this.cmbTaxRate.Size = new System.Drawing.Size(95, 20);
            this.cmbTaxRate.TabIndex = 364;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(605, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 363;
            this.label2.Text = "税    率";
            // 
            // cmbBillType
            // 
            this.cmbBillType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBillType.FormattingEnabled = true;
            this.cmbBillType.Location = new System.Drawing.Point(555, 20);
            this.cmbBillType.Name = "cmbBillType";
            this.cmbBillType.Size = new System.Drawing.Size(233, 20);
            this.cmbBillType.TabIndex = 361;
            this.cmbBillType.SelectedIndexChanged += new System.EventHandler(this.cmbBillType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 358;
            this.label1.Text = "结算单位";
            // 
            // txtInvoiceInfo
            // 
            this.txtInvoiceInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInvoiceInfo.BackColor = System.Drawing.Color.White;
            this.txtInvoiceInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInvoiceInfo.ForeColor = System.Drawing.Color.Black;
            this.txtInvoiceInfo.Location = new System.Drawing.Point(144, 95);
            this.txtInvoiceInfo.Multiline = true;
            this.txtInvoiceInfo.Name = "txtInvoiceInfo";
            this.txtInvoiceInfo.ReadOnly = true;
            this.txtInvoiceInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInvoiceInfo.Size = new System.Drawing.Size(644, 43);
            this.txtInvoiceInfo.TabIndex = 345;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(13, 109);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 344;
            this.label13.Text = "本次结算";
            // 
            // cmbInvoiceType
            // 
            this.cmbInvoiceType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInvoiceType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbInvoiceType.FormattingEnabled = true;
            this.cmbInvoiceType.Location = new System.Drawing.Point(442, 57);
            this.cmbInvoiceType.Name = "cmbInvoiceType";
            this.cmbInvoiceType.Size = new System.Drawing.Size(112, 20);
            this.cmbInvoiceType.TabIndex = 328;
            this.cmbInvoiceType.SelectedIndexChanged += new System.EventHandler(this.cmbInvoiceType_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(468, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 326;
            this.label11.Text = "单据类别";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(371, 61);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 324;
            this.label10.Text = "发票类别";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(359, 25);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 309;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(275, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 308;
            this.label5.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(75, 21);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(181, 21);
            this.txtBillNo.TabIndex = 306;
            this.txtBillNo.Text = "CJS201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 307;
            this.label4.Text = "单据编号";
            // 
            // 入库单号
            // 
            this.入库单号.DataPropertyName = "入库单号";
            this.入库单号.HeaderText = "入库单号";
            this.入库单号.Name = "入库单号";
            this.入库单号.ReadOnly = true;
            this.入库单号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.入库单号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 零件图号
            // 
            this.零件图号.DataPropertyName = "零件图号";
            this.零件图号.HeaderText = "零件图号";
            this.零件图号.Name = "零件图号";
            this.零件图号.ReadOnly = true;
            // 
            // 零件名称
            // 
            this.零件名称.DataPropertyName = "零件名称";
            this.零件名称.HeaderText = "零件名称";
            this.零件名称.Name = "零件名称";
            this.零件名称.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 批次号
            // 
            this.批次号.DataPropertyName = "批次号";
            this.批次号.HeaderText = "批次号";
            this.批次号.Name = "批次号";
            this.批次号.ReadOnly = true;
            // 
            // 入库数量
            // 
            this.入库数量.DataPropertyName = "入库数量";
            this.入库数量.HeaderText = "入库数量";
            this.入库数量.Name = "入库数量";
            this.入库数量.ReadOnly = true;
            // 
            // 入库单价_不含税
            // 
            this.入库单价_不含税.DataPropertyName = "入库单价";
            this.入库单价_不含税.HeaderText = "入库单价";
            this.入库单价_不含税.Name = "入库单价_不含税";
            this.入库单价_不含税.ReadOnly = true;
            // 
            // 单件委托材料
            // 
            this.单件委托材料.DataPropertyName = "单件委托材料";
            this.单件委托材料.HeaderText = "单件委托材料";
            this.单件委托材料.Name = "单件委托材料";
            this.单件委托材料.ReadOnly = true;
            // 
            // 单件加工费
            // 
            this.单件加工费.DataPropertyName = "单件加工费";
            this.单件加工费.HeaderText = "单件加工费";
            this.单件加工费.Name = "单件加工费";
            this.单件加工费.ReadOnly = true;
            // 
            // 入库金额
            // 
            this.入库金额.DataPropertyName = "入库金额";
            this.入库金额.HeaderText = "入库金额";
            this.入库金额.Name = "入库金额";
            this.入库金额.ReadOnly = true;
            // 
            // 委托加工材料
            // 
            this.委托加工材料.DataPropertyName = "委托加工材料";
            this.委托加工材料.HeaderText = "委托加工材料";
            this.委托加工材料.Name = "委托加工材料";
            this.委托加工材料.ReadOnly = true;
            // 
            // 发票单价
            // 
            this.发票单价.DataPropertyName = "发票单价";
            this.发票单价.DecimalPlaces = 10;
            this.发票单价.HeaderText = "发票单价";
            this.发票单价.Maximum = new decimal(new int[] {
            -1593835520,
            466537709,
            54210,
            0});
            this.发票单价.Name = "发票单价";
            this.发票单价.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.发票单价.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 税额
            // 
            this.税额.DataPropertyName = "税额";
            this.税额.HeaderText = "税额";
            this.税额.Name = "税额";
            this.税额.ReadOnly = true;
            this.税额.Visible = false;
            // 
            // 价税合计
            // 
            this.价税合计.DataPropertyName = "价税合计";
            this.价税合计.HeaderText = "价税合计";
            this.价税合计.Name = "价税合计";
            this.价税合计.ReadOnly = true;
            this.价税合计.Visible = false;
            // 
            // 发票金额
            // 
            this.发票金额.DataPropertyName = "发票金额";
            this.发票金额.HeaderText = "发票金额";
            this.发票金额.Name = "发票金额";
            this.发票金额.ReadOnly = true;
            this.发票金额.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 执行合同号_申请单
            // 
            this.执行合同号_申请单.DataPropertyName = "合同申请单号";
            this.执行合同号_申请单.HeaderText = "合同申请单号";
            this.执行合同号_申请单.Name = "执行合同号_申请单";
            this.执行合同号_申请单.ReadOnly = true;
            // 
            // 差异说明
            // 
            this.差异说明.DataPropertyName = "差异说明";
            this.差异说明.HeaderText = "差异说明";
            this.差异说明.Name = "差异说明";
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.Visible = false;
            // 
            // 序号
            // 
            this.序号.DataPropertyName = "序号";
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.ReadOnly = true;
            this.序号.Visible = false;
            // 
            // 采购结算单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 552);
            this.Controls.Add(this.customPanel1);
            this.Name = "采购结算单明细";
            this.Text = "采购结算单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customPanel1_PanelGetDataInfo);
            this.customPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInvoiceInfo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbInvoiceType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbBillType;
        private System.Windows.Forms.ComboBox cmbTaxRate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnInvoice;
        private System.Windows.Forms.Button btnInPutBill;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private UniversalControlLibrary.TextBoxShow txtSettlementCompany;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.Label lbTotalTaxPrice;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lbInPutPrice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.GroupBox groupBox2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtAccoutingDocumentNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbInvoicePrice;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件图号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库单价_不含税;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单件委托材料;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单件加工费;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 委托加工材料;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 发票单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 税额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 价税合计;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发票金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 执行合同号_申请单;
        private System.Windows.Forms.DataGridViewTextBoxColumn 差异说明;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
    }
}