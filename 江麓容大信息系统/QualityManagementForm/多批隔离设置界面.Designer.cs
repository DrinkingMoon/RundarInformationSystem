namespace Form_Quality_QC
{
    partial class 多批隔离设置界面
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(多批隔离设置界面));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnExit = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.numGoodsCount = new System.Windows.Forms.NumericUpDown();
            this.txtIsolationReason = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStorageID = new System.Windows.Forms.ComboBox();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.隔离原因及处理要求 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.所属库房 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.库房代码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoodsCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.toolStripSeparatorAdd,
            this.btnExit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(874, 25);
            this.toolStrip1.TabIndex = 83;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSave.Size = new System.Drawing.Size(67, 22);
            this.btnSave.Tag = "Add";
            this.btnSave.Text = "提交(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExit
            // 
            this.btnExit.Image = ((System.Drawing.Image)(resources.GetObject("btnExit.Image")));
            this.btnExit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(67, 22);
            this.btnExit.Tag = "view";
            this.btnExit.Text = "退出(&E)";
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.numGoodsCount);
            this.groupBox1.Controls.Add(this.txtIsolationReason);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBatchNo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtGoodsName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbStorageID);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(874, 213);
            this.groupBox1.TabIndex = 84;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "57";
            this.groupBox1.Text = "编制人信息";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(780, 25);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 337;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(675, 25);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 336;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // numGoodsCount
            // 
            this.numGoodsCount.DecimalPlaces = 2;
            this.numGoodsCount.Enabled = false;
            this.numGoodsCount.Location = new System.Drawing.Point(349, 109);
            this.numGoodsCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numGoodsCount.Name = "numGoodsCount";
            this.numGoodsCount.Size = new System.Drawing.Size(181, 21);
            this.numGoodsCount.TabIndex = 335;
            this.numGoodsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIsolationReason
            // 
            this.txtIsolationReason.Location = new System.Drawing.Point(349, 151);
            this.txtIsolationReason.Multiline = true;
            this.txtIsolationReason.Name = "txtIsolationReason";
            this.txtIsolationReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtIsolationReason.Size = new System.Drawing.Size(506, 47);
            this.txtIsolationReason.TabIndex = 332;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(290, 151);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 44);
            this.label12.TabIndex = 331;
            this.label12.Text = "隔离原因及处理要求";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(536, 113);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 328;
            this.label10.Text = "件";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(290, 113);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 327;
            this.label9.Text = "隔 离 数";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(590, 113);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 325;
            this.label8.Text = "供 应 商";
            // 
            // txtProvider
            // 
            this.txtProvider.Enabled = false;
            this.txtProvider.Location = new System.Drawing.Point(649, 109);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.Size = new System.Drawing.Size(206, 21);
            this.txtProvider.TabIndex = 324;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 323;
            this.label3.Text = "批 次 号";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.DataResult = null;
            this.txtBatchNo.DataTableResult = null;
            this.txtBatchNo.EditingControlDataGridView = null;
            this.txtBatchNo.EditingControlFormattedValue = "";
            this.txtBatchNo.EditingControlRowIndex = 0;
            this.txtBatchNo.EditingControlValueChanged = true;
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品批次;
            this.txtBatchNo.IsMultiSelect = true;
            this.txtBatchNo.Location = new System.Drawing.Point(72, 109);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Multiline = true;
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBatchNo.ShowResultForm = true;
            this.txtBatchNo.Size = new System.Drawing.Size(198, 86);
            this.txtBatchNo.StrEndSql = null;
            this.txtBatchNo.TabIndex = 322;
            this.txtBatchNo.TabStop = false;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(590, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 321;
            this.label7.Text = "规    格";
            // 
            // txtSpec
            // 
            this.txtSpec.Enabled = false;
            this.txtSpec.Location = new System.Drawing.Point(648, 69);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(207, 21);
            this.txtSpec.TabIndex = 320;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(290, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 319;
            this.label6.Text = "物品名称";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Enabled = false;
            this.txtGoodsName.Location = new System.Drawing.Point(349, 69);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.Size = new System.Drawing.Size(204, 21);
            this.txtGoodsName.TabIndex = 318;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 317;
            this.label2.Text = "图号型号";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.DataTableResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品库存合计;
            this.txtGoodsCode.IsMultiSelect = false;
            this.txtGoodsCode.Location = new System.Drawing.Point(72, 69);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(198, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 316;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            this.txtGoodsCode.Enter += new System.EventHandler(this.txtGoodsCode_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(14, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 315;
            this.label1.Text = "所属库房";
            // 
            // cmbStorageID
            // 
            this.cmbStorageID.FormattingEnabled = true;
            this.cmbStorageID.Location = new System.Drawing.Point(73, 28);
            this.cmbStorageID.Name = "cmbStorageID";
            this.cmbStorageID.Size = new System.Drawing.Size(197, 20);
            this.cmbStorageID.TabIndex = 314;
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
            this.批次号,
            this.供应商,
            this.隔离原因及处理要求,
            this.所属库房,
            this.库房代码,
            this.物品ID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 238);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(874, 201);
            this.customDataGridView1.TabIndex = 86;
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
            // 数量
            // 
            this.数量.DataPropertyName = "数量";
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.ReadOnly = true;
            this.数量.Width = 75;
            // 
            // 批次号
            // 
            this.批次号.DataPropertyName = "批次号";
            this.批次号.HeaderText = "批次号";
            this.批次号.Name = "批次号";
            this.批次号.ReadOnly = true;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.ReadOnly = true;
            this.供应商.Width = 75;
            // 
            // 隔离原因及处理要求
            // 
            this.隔离原因及处理要求.HeaderText = "隔离原因及处理要求";
            this.隔离原因及处理要求.Name = "隔离原因及处理要求";
            this.隔离原因及处理要求.ReadOnly = true;
            this.隔离原因及处理要求.Width = 200;
            // 
            // 所属库房
            // 
            this.所属库房.DataPropertyName = "所属库房";
            this.所属库房.HeaderText = "所属库房";
            this.所属库房.Name = "所属库房";
            this.所属库房.ReadOnly = true;
            this.所属库房.Width = 80;
            // 
            // 库房代码
            // 
            this.库房代码.HeaderText = "库房代码";
            this.库房代码.Name = "库房代码";
            this.库房代码.ReadOnly = true;
            this.库房代码.Visible = false;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 多批隔离设置界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(874, 439);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "多批隔离设置界面";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "多批隔离设置界面";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoodsCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnExit;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.NumericUpDown numGoodsCount;
        private System.Windows.Forms.TextBox txtIsolationReason;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtBatchNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStorageID;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.DataGridViewTextBoxColumn 隔离原因及处理要求;
        private System.Windows.Forms.DataGridViewTextBoxColumn 所属库房;
        private System.Windows.Forms.DataGridViewTextBoxColumn 库房代码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
    }
}