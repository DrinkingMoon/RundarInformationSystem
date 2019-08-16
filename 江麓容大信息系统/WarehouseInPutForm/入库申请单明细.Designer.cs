namespace Form_Manufacture_Storage
{
    partial class 入库申请单明细
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(入库申请单明细));
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
            this.检验报告 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.备注 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnTypeDetail = new System.Windows.Forms.Button();
            this.txtTypeDetail = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chbIsRepeat = new System.Windows.Forms.CheckBox();
            this.txtApplyingDepartment = new UniversalControlLibrary.TextBoxShow();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.chbIsConfirmArrival = new System.Windows.Forms.CheckBox();
            this.btnBenchCreate = new System.Windows.Forms.Button();
            this.cmbBillType = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
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
            this.customPanel1.Size = new System.Drawing.Size(893, 552);
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
            this.检验报告,
            this.备注,
            this.物品ID,
            this.单据号});
            this.customDataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 156);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(893, 396);
            this.customDataGridView1.TabIndex = 13;
            this.customDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellDoubleClick);
            // 
            // 关联业务
            // 
            this.关联业务.DataPropertyName = "关联业务";
            this.关联业务.DataResult = null;
            this.关联业务.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
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
            // 检验报告
            // 
            this.检验报告.DataPropertyName = "检验报告";
            this.检验报告.HeaderText = "检验报告";
            this.检验报告.Name = "检验报告";
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
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 124);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = true;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(893, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 12;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnTypeDetail);
            this.groupBox1.Controls.Add(this.txtTypeDetail);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.chbIsRepeat);
            this.groupBox1.Controls.Add(this.txtApplyingDepartment);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.chbIsConfirmArrival);
            this.groupBox1.Controls.Add(this.btnBenchCreate);
            this.groupBox1.Controls.Add(this.cmbBillType);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(893, 124);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据";
            // 
            // btnTypeDetail
            // 
            this.btnTypeDetail.BackColor = System.Drawing.Color.Transparent;
            this.btnTypeDetail.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnTypeDetail.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnTypeDetail.Image = ((System.Drawing.Image)(resources.GetObject("btnTypeDetail.Image")));
            this.btnTypeDetail.Location = new System.Drawing.Point(700, 57);
            this.btnTypeDetail.Name = "btnTypeDetail";
            this.btnTypeDetail.Size = new System.Drawing.Size(21, 19);
            this.btnTypeDetail.TabIndex = 360;
            this.btnTypeDetail.UseVisualStyleBackColor = false;
            this.btnTypeDetail.Click += new System.EventHandler(this.btnTypeDetail_Click);
            // 
            // txtTypeDetail
            // 
            this.txtTypeDetail.BackColor = System.Drawing.Color.White;
            this.txtTypeDetail.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtTypeDetail.Location = new System.Drawing.Point(555, 56);
            this.txtTypeDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTypeDetail.Name = "txtTypeDetail";
            this.txtTypeDetail.ReadOnly = true;
            this.txtTypeDetail.Size = new System.Drawing.Size(139, 21);
            this.txtTypeDetail.TabIndex = 359;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(468, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 358;
            this.label1.Text = "业务类型明细";
            // 
            // chbIsRepeat
            // 
            this.chbIsRepeat.AutoSize = true;
            this.chbIsRepeat.Location = new System.Drawing.Point(277, 58);
            this.chbIsRepeat.Name = "chbIsRepeat";
            this.chbIsRepeat.Size = new System.Drawing.Size(72, 16);
            this.chbIsRepeat.TabIndex = 357;
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
            this.txtApplyingDepartment.Location = new System.Drawing.Point(555, 21);
            this.txtApplyingDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtApplyingDepartment.Name = "txtApplyingDepartment";
            this.txtApplyingDepartment.ShowResultForm = true;
            this.txtApplyingDepartment.Size = new System.Drawing.Size(166, 21);
            this.txtApplyingDepartment.StrEndSql = null;
            this.txtApplyingDepartment.TabIndex = 346;
            this.txtApplyingDepartment.TabStop = false;
            this.txtApplyingDepartment.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtApplyingDepartment_OnCompleteSearch);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemark.ForeColor = System.Drawing.Color.Black;
            this.txtRemark.Location = new System.Drawing.Point(75, 92);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(784, 23);
            this.txtRemark.TabIndex = 345;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(12, 97);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 344;
            this.label13.Text = "备    注";
            // 
            // chbIsConfirmArrival
            // 
            this.chbIsConfirmArrival.AutoSize = true;
            this.chbIsConfirmArrival.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chbIsConfirmArrival.Location = new System.Drawing.Point(361, 58);
            this.chbIsConfirmArrival.Name = "chbIsConfirmArrival";
            this.chbIsConfirmArrival.Size = new System.Drawing.Size(96, 16);
            this.chbIsConfirmArrival.TabIndex = 333;
            this.chbIsConfirmArrival.Text = "需要确认到货";
            this.chbIsConfirmArrival.UseVisualStyleBackColor = true;
            // 
            // btnBenchCreate
            // 
            this.btnBenchCreate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBenchCreate.Location = new System.Drawing.Point(193, 53);
            this.btnBenchCreate.Name = "btnBenchCreate";
            this.btnBenchCreate.Size = new System.Drawing.Size(63, 26);
            this.btnBenchCreate.TabIndex = 329;
            this.btnBenchCreate.Text = "批量引用";
            this.btnBenchCreate.UseVisualStyleBackColor = true;
            this.btnBenchCreate.Click += new System.EventHandler(this.btnBenchCreate_Click);
            // 
            // cmbBillType
            // 
            this.cmbBillType.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbBillType.FormattingEnabled = true;
            this.cmbBillType.Location = new System.Drawing.Point(75, 56);
            this.cmbBillType.Name = "cmbBillType";
            this.cmbBillType.Size = new System.Drawing.Size(112, 20);
            this.cmbBillType.TabIndex = 328;
            this.cmbBillType.SelectedIndexChanged += new System.EventHandler(this.cmbBillType_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(468, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 326;
            this.label11.Text = "申请部门";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(13, 60);
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
            this.label5.Text = "业务状态";
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
            this.txtBillNo.Text = "IPA201412000001";
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
            this.label4.Text = "业务编号";
            // 
            // 入库申请单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 552);
            this.Controls.Add(this.customPanel1);
            this.Name = "入库申请单明细";
            this.Text = "入库申请单明细";
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
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 关联业务;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 批次号;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 供应商;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 检验报告;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 备注;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private UniversalControlLibrary.TextBoxShow txtApplyingDepartment;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chbIsConfirmArrival;
        private System.Windows.Forms.Button btnBenchCreate;
        private System.Windows.Forms.ComboBox cmbBillType;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chbIsRepeat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtTypeDetail;
        private System.Windows.Forms.Button btnTypeDetail;

    }
}