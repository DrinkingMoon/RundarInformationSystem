namespace Form_Economic_Purchase
{
    partial class 供应商应付账款明细
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
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dtpFinanceTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.lbInvoicePrice = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lbInPutPrice = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.btnInvoice = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnInputAccountInfo = new System.Windows.Forms.Button();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVoucherNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInvoiceInfo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtSettlementCompany = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.挂账年月 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.协议单价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.税率 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.应付数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.实付数量 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.应付金额 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票金额 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.备注 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.dtpFinanceTime);
            this.customGroupBox1.Controls.Add(this.label6);
            this.customGroupBox1.Controls.Add(this.lbInvoicePrice);
            this.customGroupBox1.Controls.Add(this.label16);
            this.customGroupBox1.Controls.Add(this.lbInPutPrice);
            this.customGroupBox1.Controls.Add(this.label9);
            this.customGroupBox1.Controls.Add(this.btnInvoice);
            this.customGroupBox1.Controls.Add(this.btnDelete);
            this.customGroupBox1.Controls.Add(this.btnInputAccountInfo);
            this.customGroupBox1.Controls.Add(this.txtRemark);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.txtVoucherNo);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.txtInvoiceInfo);
            this.customGroupBox1.Controls.Add(this.label12);
            this.customGroupBox1.Controls.Add(this.txtSettlementCompany);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.lbBillStatus);
            this.customGroupBox1.Controls.Add(this.label5);
            this.customGroupBox1.Controls.Add(this.txtBillNo);
            this.customGroupBox1.Controls.Add(this.label4);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(846, 251);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // dtpFinanceTime
            // 
            this.dtpFinanceTime.Location = new System.Drawing.Point(85, 172);
            this.dtpFinanceTime.Name = "dtpFinanceTime";
            this.dtpFinanceTime.Size = new System.Drawing.Size(181, 21);
            this.dtpFinanceTime.TabIndex = 392;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(23, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 391;
            this.label6.Text = "账单日期";
            // 
            // lbInvoicePrice
            // 
            this.lbInvoicePrice.AutoSize = true;
            this.lbInvoicePrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInvoicePrice.Location = new System.Drawing.Point(772, 176);
            this.lbInvoicePrice.Name = "lbInvoicePrice";
            this.lbInvoicePrice.Size = new System.Drawing.Size(29, 12);
            this.lbInvoicePrice.TabIndex = 390;
            this.lbInvoicePrice.Text = "0.00";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(673, 176);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(83, 12);
            this.label16.TabIndex = 389;
            this.label16.Text = "发票金额合计:";
            // 
            // lbInPutPrice
            // 
            this.lbInPutPrice.AutoSize = true;
            this.lbInPutPrice.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbInPutPrice.Location = new System.Drawing.Point(602, 176);
            this.lbInPutPrice.Name = "lbInPutPrice";
            this.lbInPutPrice.Size = new System.Drawing.Size(29, 12);
            this.lbInPutPrice.TabIndex = 386;
            this.lbInPutPrice.Text = "0.00";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(502, 176);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 12);
            this.label9.TabIndex = 385;
            this.label9.Text = "应付金额合计:";
            // 
            // btnInvoice
            // 
            this.btnInvoice.Location = new System.Drawing.Point(774, 115);
            this.btnInvoice.Name = "btnInvoice";
            this.btnInvoice.Size = new System.Drawing.Size(39, 24);
            this.btnInvoice.TabIndex = 384;
            this.btnInvoice.Text = "……";
            this.btnInvoice.UseVisualStyleBackColor = true;
            this.btnInvoice.Click += new System.EventHandler(this.btnInvoice_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(604, 61);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(85, 23);
            this.btnDelete.TabIndex = 383;
            this.btnDelete.Text = "删除选中项";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnInputAccountInfo
            // 
            this.btnInputAccountInfo.Location = new System.Drawing.Point(454, 61);
            this.btnInputAccountInfo.Name = "btnInputAccountInfo";
            this.btnInputAccountInfo.Size = new System.Drawing.Size(131, 23);
            this.btnInputAccountInfo.TabIndex = 382;
            this.btnInputAccountInfo.Text = "选择需勾稽挂账明细";
            this.btnInputAccountInfo.UseVisualStyleBackColor = true;
            this.btnInputAccountInfo.Click += new System.EventHandler(this.btnInputAccountInfo_Click);
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(85, 213);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(728, 21);
            this.txtRemark.TabIndex = 381;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(23, 216);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 380;
            this.label3.Text = "备    注";
            // 
            // txtVoucherNo
            // 
            this.txtVoucherNo.Location = new System.Drawing.Point(345, 172);
            this.txtVoucherNo.Name = "txtVoucherNo";
            this.txtVoucherNo.Size = new System.Drawing.Size(151, 21);
            this.txtVoucherNo.TabIndex = 379;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(286, 176);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 378;
            this.label2.Text = "凭 证 号";
            // 
            // txtInvoiceInfo
            // 
            this.txtInvoiceInfo.Location = new System.Drawing.Point(85, 101);
            this.txtInvoiceInfo.Multiline = true;
            this.txtInvoiceInfo.Name = "txtInvoiceInfo";
            this.txtInvoiceInfo.ReadOnly = true;
            this.txtInvoiceInfo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInvoiceInfo.Size = new System.Drawing.Size(683, 53);
            this.txtInvoiceInfo.TabIndex = 377;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(23, 121);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 376;
            this.label12.Text = "发 票 号";
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
            this.txtSettlementCompany.Location = new System.Drawing.Point(85, 63);
            this.txtSettlementCompany.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSettlementCompany.Name = "txtSettlementCompany";
            this.txtSettlementCompany.ShowResultForm = true;
            this.txtSettlementCompany.Size = new System.Drawing.Size(340, 21);
            this.txtSettlementCompany.StrEndSql = null;
            this.txtSettlementCompany.TabIndex = 373;
            this.txtSettlementCompany.TabStop = false;
            this.txtSettlementCompany.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtSettlementCompany_OnCompleteSearch);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(23, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 372;
            this.label1.Text = "开票单位";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(363, 30);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 313;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(286, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 312;
            this.label5.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(85, 26);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(181, 21);
            this.txtBillNo.TabIndex = 310;
            this.txtBillNo.Text = "PYF201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(23, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 311;
            this.label4.Text = "单据编号";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.图号型号,
            this.物品名称,
            this.规格,
            this.挂账年月,
            this.协议单价,
            this.税率,
            this.单位,
            this.应付数量,
            this.实付数量,
            this.应付金额,
            this.发票金额,
            this.备注,
            this.单据号,
            this.GoodsID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 251);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(846, 301);
            this.customDataGridView1.TabIndex = 2;
            this.customDataGridView1.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.customDataGridView1_RowsAdded);
            this.customDataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellEndEdit);
            this.customDataGridView1.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.customDataGridView1_RowsRemoved);
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
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 挂账年月
            // 
            this.挂账年月.DataPropertyName = "挂账年月";
            this.挂账年月.HeaderText = "挂账年月";
            this.挂账年月.Name = "挂账年月";
            this.挂账年月.ReadOnly = true;
            // 
            // 协议单价
            // 
            this.协议单价.DataPropertyName = "协议单价";
            this.协议单价.HeaderText = "协议单价";
            this.协议单价.Name = "协议单价";
            this.协议单价.ReadOnly = true;
            // 
            // 税率
            // 
            this.税率.DataPropertyName = "税率";
            this.税率.HeaderText = "税率";
            this.税率.Name = "税率";
            this.税率.Width = 60;
            // 
            // 单位
            // 
            this.单位.DataPropertyName = "单位";
            this.单位.HeaderText = "单位";
            this.单位.Name = "单位";
            this.单位.Width = 60;
            // 
            // 应付数量
            // 
            this.应付数量.DataPropertyName = "应付数量";
            this.应付数量.HeaderText = "应付数量";
            this.应付数量.Name = "应付数量";
            this.应付数量.ReadOnly = true;
            // 
            // 实付数量
            // 
            this.实付数量.DataPropertyName = "实付数量";
            this.实付数量.DecimalPlaces = 2;
            this.实付数量.HeaderText = "实付数量";
            this.实付数量.Maximum = new decimal(new int[] {
            1569325056,
            23283064,
            0,
            0});
            this.实付数量.Minimum = new decimal(new int[] {
            1569325056,
            23283064,
            0,
            -2147483648});
            this.实付数量.Name = "实付数量";
            this.实付数量.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.实付数量.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 应付金额
            // 
            this.应付金额.DataPropertyName = "应付金额";
            this.应付金额.HeaderText = "应付金额";
            this.应付金额.Name = "应付金额";
            this.应付金额.ReadOnly = true;
            // 
            // 发票金额
            // 
            this.发票金额.DataPropertyName = "发票金额";
            this.发票金额.DecimalPlaces = 2;
            this.发票金额.HeaderText = "发票金额";
            this.发票金额.Maximum = new decimal(new int[] {
            -1981284352,
            -1966660860,
            0,
            0});
            this.发票金额.Minimum = new decimal(new int[] {
            -1981284352,
            -1966660860,
            0,
            -2147483648});
            this.发票金额.Name = "发票金额";
            this.发票金额.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.发票金额.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 备注
            // 
            this.备注.DataPropertyName = "备注";
            this.备注.HeaderText = "备注";
            this.备注.Name = "备注";
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.Visible = false;
            // 
            // GoodsID
            // 
            this.GoodsID.DataPropertyName = "GoodsID";
            this.GoodsID.HeaderText = "GoodsID";
            this.GoodsID.Name = "GoodsID";
            this.GoodsID.Visible = false;
            // 
            // 供应商应付账款明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(846, 552);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "供应商应付账款明细";
            this.Text = "供应商应付账款明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.供应商应付账款明细_PanelGetDataInfo);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.TextBoxShow txtSettlementCompany;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtInvoiceInfo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnInputAccountInfo;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVoucherNo;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Button btnInvoice;
        private System.Windows.Forms.Label lbInvoicePrice;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lbInPutPrice;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpFinanceTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 挂账年月;
        private System.Windows.Forms.DataGridViewTextBoxColumn 协议单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 税率;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 应付数量;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 实付数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 应付金额;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 发票金额;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备注;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsID;
    }
}