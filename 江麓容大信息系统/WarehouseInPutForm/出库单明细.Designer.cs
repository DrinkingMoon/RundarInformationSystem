namespace Form_Manufacture_Storage
{
    partial class 出库单明细
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.关联业务 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.图号型号 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批次号 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.供应商 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.数量 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备注 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbTypeDetail = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbIsRepeat = new System.Windows.Forms.CheckBox();
            this.txtApplyingDepartment = new UniversalControlLibrary.TextBoxShow();
            this.label14 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnBatchCreate = new System.Windows.Forms.Button();
            this.cmbBillType = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.customDataGridView1);
            this.customPanel1.Controls.Add(this.userControlDataLocalizer1);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(919, 552);
            this.customPanel1.TabIndex = 0;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.关联业务,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.批次号,
            this.供应商,
            this.数量,
            this.单位,
            this.备注,
            this.物品ID,
            this.单据号});
            this.customDataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 189);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(919, 363);
            this.customDataGridView1.TabIndex = 18;
            this.customDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellDoubleClick);
            // 
            // 关联业务
            // 
            this.关联业务.DataPropertyName = "关联业务";
            this.关联业务.DataResult = null;
            this.关联业务.FindItem = UniversalControlLibrary.TextBoxShow.FindType.出库申请单;
            this.关联业务.HeaderText = "关联业务";
            this.关联业务.Name = "关联业务";
            this.关联业务.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.关联业务.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.DataResult = null;
            this.图号型号.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
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
            // 批次号
            // 
            this.批次号.DataPropertyName = "批次号";
            this.批次号.DataResult = null;
            this.批次号.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品批次;
            this.批次号.HeaderText = "批次号";
            this.批次号.Name = "批次号";
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.DataResult = null;
            this.供应商.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            // 
            // 数量
            // 
            this.数量.DataPropertyName = "数量";
            this.数量.DecimalPlaces = 2;
            this.数量.HeaderText = "数量";
            this.数量.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.数量.Name = "数量";
            // 
            // 单位
            // 
            this.单位.DataPropertyName = "单位";
            this.单位.HeaderText = "单位";
            this.单位.Name = "单位";
            this.单位.ReadOnly = true;
            this.单位.Width = 40;
            // 
            // 备注
            // 
            this.备注.DataPropertyName = "备注";
            this.备注.HeaderText = "备注";
            this.备注.Name = "备注";
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
            this.单据号.ReadOnly = true;
            this.单据号.Visible = false;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 157);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(919, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 16;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbTypeDetail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chbIsRepeat);
            this.groupBox1.Controls.Add(this.txtApplyingDepartment);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.cmbStorage);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.btnBatchCreate);
            this.groupBox1.Controls.Add(this.cmbBillType);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(919, 157);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据";
            // 
            // cmbTypeDetail
            // 
            this.cmbTypeDetail.FormattingEnabled = true;
            this.cmbTypeDetail.Location = new System.Drawing.Point(475, 89);
            this.cmbTypeDetail.Name = "cmbTypeDetail";
            this.cmbTypeDetail.Size = new System.Drawing.Size(166, 20);
            this.cmbTypeDetail.TabIndex = 362;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(388, 94);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 361;
            this.label1.Text = "业务类型明细";
            // 
            // chbIsRepeat
            // 
            this.chbIsRepeat.AutoSize = true;
            this.chbIsRepeat.Location = new System.Drawing.Point(293, 92);
            this.chbIsRepeat.Name = "chbIsRepeat";
            this.chbIsRepeat.Size = new System.Drawing.Size(72, 16);
            this.chbIsRepeat.TabIndex = 356;
            this.chbIsRepeat.Text = "重复引用";
            this.chbIsRepeat.UseVisualStyleBackColor = true;
            // 
            // txtApplyingDepartment
            // 
            this.txtApplyingDepartment.DataResult = null;
            this.txtApplyingDepartment.EditingControlDataGridView = null;
            this.txtApplyingDepartment.EditingControlFormattedValue = "";
            this.txtApplyingDepartment.EditingControlRowIndex = 0;
            this.txtApplyingDepartment.EditingControlValueChanged = true;
            this.txtApplyingDepartment.FindItem = UniversalControlLibrary.TextBoxShow.FindType.部门;
            this.txtApplyingDepartment.Location = new System.Drawing.Point(475, 57);
            this.txtApplyingDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtApplyingDepartment.Name = "txtApplyingDepartment";
            this.txtApplyingDepartment.ShowResultForm = true;
            this.txtApplyingDepartment.Size = new System.Drawing.Size(166, 21);
            this.txtApplyingDepartment.StrEndSql = null;
            this.txtApplyingDepartment.TabIndex = 347;
            this.txtApplyingDepartment.TabStop = false;
            this.txtApplyingDepartment.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtApplyingDepartment_OnCompleteSearch);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(388, 61);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 338;
            this.label14.Text = "申请部门";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemark.ForeColor = System.Drawing.Color.Black;
            this.txtRemark.Location = new System.Drawing.Point(85, 123);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(814, 25);
            this.txtRemark.TabIndex = 337;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(12, 129);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 336;
            this.label13.Text = "备    注";
            // 
            // cmbStorage
            // 
            this.cmbStorage.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(85, 57);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(192, 20);
            this.cmbStorage.TabIndex = 335;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(13, 61);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 334;
            this.label11.Text = "出库库房";
            // 
            // btnBatchCreate
            // 
            this.btnBatchCreate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBatchCreate.Location = new System.Drawing.Point(200, 86);
            this.btnBatchCreate.Name = "btnBatchCreate";
            this.btnBatchCreate.Size = new System.Drawing.Size(77, 26);
            this.btnBatchCreate.TabIndex = 333;
            this.btnBatchCreate.Text = "批量引用";
            this.btnBatchCreate.UseVisualStyleBackColor = true;
            this.btnBatchCreate.Click += new System.EventHandler(this.btnBenchCreate_Click);
            // 
            // cmbBillType
            // 
            this.cmbBillType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBillType.FormattingEnabled = true;
            this.cmbBillType.Location = new System.Drawing.Point(85, 90);
            this.cmbBillType.Name = "cmbBillType";
            this.cmbBillType.Size = new System.Drawing.Size(109, 20);
            this.cmbBillType.TabIndex = 328;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(13, 94);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 324;
            this.label10.Text = "业务类型";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(484, 27);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 309;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(388, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 308;
            this.label5.Text = "业务状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(85, 23);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(192, 21);
            this.txtBillNo.TabIndex = 306;
            this.txtBillNo.Text = "IPB201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 307;
            this.label4.Text = "业务编号";
            // 
            // 出库单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(919, 552);
            this.Controls.Add(this.customPanel1);
            this.Name = "出库单明细";
            this.Text = "出库单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.contextMenuStrip1.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnBatchCreate;
        private System.Windows.Forms.ComboBox cmbBillType;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private UniversalControlLibrary.TextBoxShow txtApplyingDepartment;
        private System.Windows.Forms.CheckBox chbIsRepeat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbTypeDetail;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 关联业务;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 批次号;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 供应商;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 备注;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;

    }
}