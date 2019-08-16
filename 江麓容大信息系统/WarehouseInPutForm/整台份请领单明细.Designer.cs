namespace Form_Manufacture_Storage
{
    partial class 整台份请领单明细
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(整台份请领单明细));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStrip2 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.图号型号 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new UniversalControlLibrary.DataGridViewNumericUpDownColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.基数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.customDataGridView2 = new UniversalControlLibrary.CustomDataGridView();
            this.库房名称 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.库房代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.库房顺序 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbIncludeAfterSupplement = new System.Windows.Forms.CheckBox();
            this.txtProductType = new UniversalControlLibrary.TextBoxShow();
            this.btnCreate = new System.Windows.Forms.Button();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.numRequestCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFindPurpose = new System.Windows.Forms.Button();
            this.txtPurpose = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip2.SuspendLayout();
            this.customPanel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRequestCount)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem1,
            this.删除ToolStripMenuItem1});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // 添加ToolStripMenuItem1
            // 
            this.添加ToolStripMenuItem1.Name = "添加ToolStripMenuItem1";
            this.添加ToolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.添加ToolStripMenuItem1.Text = "添加";
            this.添加ToolStripMenuItem1.Click += new System.EventHandler(this.添加ToolStripMenuItem1_Click);
            // 
            // 删除ToolStripMenuItem1
            // 
            this.删除ToolStripMenuItem1.Name = "删除ToolStripMenuItem1";
            this.删除ToolStripMenuItem1.Size = new System.Drawing.Size(94, 22);
            this.删除ToolStripMenuItem1.Text = "删除";
            this.删除ToolStripMenuItem1.Click += new System.EventHandler(this.删除ToolStripMenuItem1_Click);
            // 
            // contextMenuStrip2
            // 
            this.contextMenuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem2,
            this.删除ToolStripMenuItem2});
            this.contextMenuStrip2.Name = "contextMenuStrip1";
            this.contextMenuStrip2.Size = new System.Drawing.Size(95, 48);
            // 
            // 添加ToolStripMenuItem2
            // 
            this.添加ToolStripMenuItem2.Name = "添加ToolStripMenuItem2";
            this.添加ToolStripMenuItem2.Size = new System.Drawing.Size(94, 22);
            this.添加ToolStripMenuItem2.Text = "添加";
            this.添加ToolStripMenuItem2.Click += new System.EventHandler(this.添加ToolStripMenuItem2_Click);
            // 
            // 删除ToolStripMenuItem2
            // 
            this.删除ToolStripMenuItem2.Name = "删除ToolStripMenuItem2";
            this.删除ToolStripMenuItem2.Size = new System.Drawing.Size(94, 22);
            this.删除ToolStripMenuItem2.Text = "删除";
            this.删除ToolStripMenuItem2.Click += new System.EventHandler(this.删除ToolStripMenuItem2_Click);
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.groupBox2);
            this.customPanel1.Controls.Add(this.splitter1);
            this.customPanel1.Controls.Add(this.groupBox3);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(880, 490);
            this.customPanel1.TabIndex = 1;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.customDataGridView1);
            this.groupBox2.Controls.Add(this.userControlDataLocalizer1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 147);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(626, 343);
            this.groupBox2.TabIndex = 10;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "领料清单";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.图号型号,
            this.物品名称,
            this.规格,
            this.数量,
            this.单位,
            this.基数,
            this.物品ID,
            this.单据号});
            this.customDataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 44);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(620, 296);
            this.customDataGridView1.TabIndex = 3;
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
            // 数量
            // 
            this.数量.DataPropertyName = "数量";
            this.数量.HeaderText = "数量";
            this.数量.Maximum = new decimal(new int[] {
            100000000,
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
            this.单位.Width = 60;
            // 
            // 基数
            // 
            this.基数.DataPropertyName = "基数";
            this.基数.HeaderText = "基数";
            this.基数.Name = "基数";
            this.基数.ReadOnly = true;
            this.基数.Width = 40;
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
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(3, 17);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = true;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(620, 27);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 2;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(626, 147);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 343);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.customDataGridView2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox3.Location = new System.Drawing.Point(629, 147);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(251, 343);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "库房优先级";
            // 
            // customDataGridView2
            // 
            this.customDataGridView2.AllowUserToAddRows = false;
            this.customDataGridView2.AllowUserToDeleteRows = false;
            this.customDataGridView2.AllowUserToResizeRows = false;
            this.customDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.库房名称,
            this.库房代码,
            this.库房顺序,
            this.单据号1});
            this.customDataGridView2.ContextMenuStrip = this.contextMenuStrip2;
            this.customDataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView2.Location = new System.Drawing.Point(3, 17);
            this.customDataGridView2.Name = "customDataGridView2";
            this.customDataGridView2.RowTemplate.Height = 23;
            this.customDataGridView2.Size = new System.Drawing.Size(245, 323);
            this.customDataGridView2.TabIndex = 2;
            // 
            // 库房名称
            // 
            this.库房名称.DataResult = null;
            this.库房名称.FindItem = UniversalControlLibrary.TextBoxShow.FindType.库房;
            this.库房名称.HeaderText = "库房名称";
            this.库房名称.Name = "库房名称";
            // 
            // 库房代码
            // 
            this.库房代码.DataPropertyName = "库房代码";
            this.库房代码.HeaderText = "库房代码";
            this.库房代码.Name = "库房代码";
            this.库房代码.ReadOnly = true;
            // 
            // 库房顺序
            // 
            this.库房顺序.DataPropertyName = "库房顺序";
            this.库房顺序.HeaderText = "库房顺序";
            this.库房顺序.Name = "库房顺序";
            this.库房顺序.ReadOnly = true;
            this.库房顺序.Visible = false;
            this.库房顺序.Width = 60;
            // 
            // 单据号1
            // 
            this.单据号1.DataPropertyName = "单据号";
            this.单据号1.HeaderText = "单据号";
            this.单据号1.Name = "单据号1";
            this.单据号1.ReadOnly = true;
            this.单据号1.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbIncludeAfterSupplement);
            this.groupBox1.Controls.Add(this.txtProductType);
            this.groupBox1.Controls.Add(this.btnCreate);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numRequestCount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnFindPurpose);
            this.groupBox1.Controls.Add(this.txtPurpose);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(880, 147);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "概要信息";
            // 
            // chbIncludeAfterSupplement
            // 
            this.chbIncludeAfterSupplement.AutoSize = true;
            this.chbIncludeAfterSupplement.Location = new System.Drawing.Point(732, 54);
            this.chbIncludeAfterSupplement.Name = "chbIncludeAfterSupplement";
            this.chbIncludeAfterSupplement.Size = new System.Drawing.Size(84, 16);
            this.chbIncludeAfterSupplement.TabIndex = 316;
            this.chbIncludeAfterSupplement.Text = "包含后补充";
            this.chbIncludeAfterSupplement.UseVisualStyleBackColor = true;
            // 
            // txtProductType
            // 
            this.txtProductType.DataResult = null;
            this.txtProductType.EditingControlDataGridView = null;
            this.txtProductType.EditingControlFormattedValue = "";
            this.txtProductType.EditingControlRowIndex = 0;
            this.txtProductType.EditingControlValueChanged = true;
            this.txtProductType.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtProductType.Location = new System.Drawing.Point(364, 52);
            this.txtProductType.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.ShowResultForm = true;
            this.txtProductType.Size = new System.Drawing.Size(134, 21);
            this.txtProductType.StrEndSql = null;
            this.txtProductType.TabIndex = 315;
            this.txtProductType.TabStop = false;
            this.txtProductType.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProductType_OnCompleteSearch);
            this.txtProductType.Enter += new System.EventHandler(this.txtProductType_Enter);
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(732, 88);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(84, 36);
            this.btnCreate.TabIndex = 314;
            this.btnCreate.Text = "生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(373, 23);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 313;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(305, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 312;
            this.label5.Text = "业务状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(80, 19);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(204, 21);
            this.txtBillNo.TabIndex = 310;
            this.txtBillNo.Text = "WMD201501000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(17, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 311;
            this.label4.Text = "业务编号";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Location = new System.Drawing.Point(80, 86);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(631, 41);
            this.txtRemark.TabIndex = 231;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(17, 100);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 230;
            this.label2.Text = "备    注";
            // 
            // numRequestCount
            // 
            this.numRequestCount.BackColor = System.Drawing.Color.White;
            this.numRequestCount.Location = new System.Drawing.Point(580, 52);
            this.numRequestCount.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numRequestCount.Name = "numRequestCount";
            this.numRequestCount.Size = new System.Drawing.Size(131, 21);
            this.numRequestCount.TabIndex = 228;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(521, 56);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 229;
            this.label1.Text = "总成数量";
            // 
            // btnFindPurpose
            // 
            this.btnFindPurpose.BackColor = System.Drawing.Color.Transparent;
            this.btnFindPurpose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindPurpose.BackgroundImage")));
            this.btnFindPurpose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindPurpose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindPurpose.Location = new System.Drawing.Point(263, 53);
            this.btnFindPurpose.Name = "btnFindPurpose";
            this.btnFindPurpose.Size = new System.Drawing.Size(21, 19);
            this.btnFindPurpose.TabIndex = 227;
            this.btnFindPurpose.UseVisualStyleBackColor = false;
            this.btnFindPurpose.Click += new System.EventHandler(this.btnFindPurpose_Click);
            // 
            // txtPurpose
            // 
            this.txtPurpose.BackColor = System.Drawing.Color.White;
            this.txtPurpose.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPurpose.Location = new System.Drawing.Point(80, 52);
            this.txtPurpose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtPurpose.Name = "txtPurpose";
            this.txtPurpose.ReadOnly = true;
            this.txtPurpose.Size = new System.Drawing.Size(177, 21);
            this.txtPurpose.TabIndex = 225;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label11.Location = new System.Drawing.Point(17, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 226;
            this.label11.Text = "用    途";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(305, 56);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 224;
            this.label6.Text = "产品类型";
            // 
            // 整台份请领单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 490);
            this.Controls.Add(this.customPanel1);
            this.Name = "整台份请领单明细";
            this.Text = "整台份请领单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.contextMenuStrip1.ResumeLayout(false);
            this.contextMenuStrip2.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRequestCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView2;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip2;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnFindPurpose;
        private System.Windows.Forms.TextBox txtPurpose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numRequestCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 库房名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 库房代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 库房顺序;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号1;
        private System.Windows.Forms.Button btnCreate;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private UniversalControlLibrary.DataGridViewNumericUpDownColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 基数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private UniversalControlLibrary.TextBoxShow txtProductType;
        private System.Windows.Forms.CheckBox chbIncludeAfterSupplement;
    }
}