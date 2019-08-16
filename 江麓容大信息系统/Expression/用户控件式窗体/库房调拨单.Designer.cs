namespace Expression
{
    partial class 库房调拨单
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(库房调拨单));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorUpdate = new System.Windows.Forms.ToolStripSeparator();
            this.btnFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorFind = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDJ_ZT = new System.Windows.Forms.ComboBox();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dgv_Show = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DJH = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InstoreRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OutStoreRoom = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DJZT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LRRY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LRRQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SHRY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SHRQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.质量人员 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批准日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWRY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CWRQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KFRY = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.KFRQ = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Show)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparatorAdd,
            this.btnDelete,
            this.toolStripSeparatorDelete,
            this.btnUpdate,
            this.toolStripSeparatorUpdate,
            this.btnFind,
            this.toolStripSeparatorFind,
            this.btnRefresh,
            this.toolStripSeparator2,
            this.btnPrint});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1051, 25);
            this.toolStrip1.TabIndex = 43;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(70, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "新建(N)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 22);
            this.btnDelete.Tag = "Delete";
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
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(69, 22);
            this.btnUpdate.Tag = "update";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparatorUpdate
            // 
            this.toolStripSeparatorUpdate.Name = "toolStripSeparatorUpdate";
            this.toolStripSeparatorUpdate.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(66, 22);
            this.btnFind.Tag = "view";
            this.btnFind.Text = "查看(F)";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // toolStripSeparatorFind
            // 
            this.toolStripSeparatorFind.Name = "toolStripSeparatorFind";
            this.toolStripSeparatorFind.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(68, 22);
            this.btnRefresh.Tag = "view";
            this.btnRefresh.Text = "刷新(R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "";
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(91, 22);
            this.btnPrint.Tag = "Print";
            this.btnPrint.Text = "单据打印(&P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1051, 49);
            this.panel3.TabIndex = 45;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(460, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "库房调拨";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.cmbDJ_ZT);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1051, 55);
            this.groupBox1.TabIndex = 46;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询信息";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(456, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 7;
            this.label3.Text = "查询状态";
            // 
            // cmbDJ_ZT
            // 
            this.cmbDJ_ZT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDJ_ZT.FormattingEnabled = true;
            this.cmbDJ_ZT.Items.AddRange(new object[] {
            "全  部",
            "已保存",
            "已审核",
            "已批准",
            "已确认"});
            this.cmbDJ_ZT.Location = new System.Drawing.Point(537, 20);
            this.cmbDJ_ZT.Name = "cmbDJ_ZT";
            this.cmbDJ_ZT.Size = new System.Drawing.Size(140, 21);
            this.cmbDJ_ZT.TabIndex = 6;
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(712, 20);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(87, 25);
            this.btnQuery.TabIndex = 5;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(223, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 14);
            this.label1.TabIndex = 3;
            this.label1.Text = "从";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(254, 22);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(172, 23);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(43, 22);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(172, 23);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // dgv_Show
            // 
            this.dgv_Show.AllowUserToAddRows = false;
            this.dgv_Show.AllowUserToDeleteRows = false;
            this.dgv_Show.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Show.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.DJH,
            this.InstoreRoom,
            this.OutStoreRoom,
            this.DJZT,
            this.Price,
            this.LRRY,
            this.LRRQ,
            this.SHRY,
            this.SHRQ,
            this.质量人员,
            this.批准日期,
            this.CWRY,
            this.CWRQ,
            this.KFRY,
            this.KFRQ,
            this.Remark});
            this.dgv_Show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Show.Location = new System.Drawing.Point(0, 129);
            this.dgv_Show.Name = "dgv_Show";
            this.dgv_Show.ReadOnly = true;
            this.dgv_Show.RowHeadersWidth = 20;
            this.dgv_Show.RowTemplate.Height = 23;
            this.dgv_Show.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Show.Size = new System.Drawing.Size(1051, 610);
            this.dgv_Show.TabIndex = 47;
            this.dgv_Show.DoubleClick += new System.EventHandler(this.dgv_Show_DoubleClick);
            this.dgv_Show.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgv_Show_RowPostPaint);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // DJH
            // 
            this.DJH.DataPropertyName = "DJH";
            this.DJH.HeaderText = "单据号";
            this.DJH.Name = "DJH";
            this.DJH.ReadOnly = true;
            // 
            // InstoreRoom
            // 
            this.InstoreRoom.DataPropertyName = "InstoreRoom";
            this.InstoreRoom.HeaderText = "调入仓";
            this.InstoreRoom.Name = "InstoreRoom";
            this.InstoreRoom.ReadOnly = true;
            // 
            // OutStoreRoom
            // 
            this.OutStoreRoom.DataPropertyName = "OutStoreRoom";
            this.OutStoreRoom.HeaderText = "调出仓";
            this.OutStoreRoom.Name = "OutStoreRoom";
            this.OutStoreRoom.ReadOnly = true;
            // 
            // DJZT
            // 
            this.DJZT.DataPropertyName = "DJZT";
            this.DJZT.HeaderText = "单据状态";
            this.DJZT.Name = "DJZT";
            this.DJZT.ReadOnly = true;
            // 
            // Price
            // 
            this.Price.DataPropertyName = "Price";
            this.Price.HeaderText = "金额";
            this.Price.Name = "Price";
            this.Price.ReadOnly = true;
            this.Price.Visible = false;
            // 
            // LRRY
            // 
            this.LRRY.DataPropertyName = "LRRY";
            this.LRRY.HeaderText = "编制人员";
            this.LRRY.Name = "LRRY";
            this.LRRY.ReadOnly = true;
            // 
            // LRRQ
            // 
            this.LRRQ.DataPropertyName = "LRRQ";
            this.LRRQ.HeaderText = "编制日期";
            this.LRRQ.Name = "LRRQ";
            this.LRRQ.ReadOnly = true;
            // 
            // SHRY
            // 
            this.SHRY.DataPropertyName = "SHRY";
            this.SHRY.HeaderText = "审核人员";
            this.SHRY.Name = "SHRY";
            this.SHRY.ReadOnly = true;
            // 
            // SHRQ
            // 
            this.SHRQ.DataPropertyName = "SHRQ";
            this.SHRQ.HeaderText = "审核日期";
            this.SHRQ.Name = "SHRQ";
            this.SHRQ.ReadOnly = true;
            // 
            // 质量人员
            // 
            this.质量人员.DataPropertyName = "Checker";
            this.质量人员.HeaderText = "质量人员";
            this.质量人员.Name = "质量人员";
            this.质量人员.ReadOnly = true;
            // 
            // 批准日期
            // 
            this.批准日期.DataPropertyName = "CheckTime";
            this.批准日期.HeaderText = "批准日期";
            this.批准日期.Name = "批准日期";
            this.批准日期.ReadOnly = true;
            // 
            // CWRY
            // 
            this.CWRY.DataPropertyName = "CWRY";
            this.CWRY.HeaderText = "财务人员";
            this.CWRY.Name = "CWRY";
            this.CWRY.ReadOnly = true;
            // 
            // CWRQ
            // 
            this.CWRQ.DataPropertyName = "CWRQ";
            this.CWRQ.HeaderText = "批准日期";
            this.CWRQ.Name = "CWRQ";
            this.CWRQ.ReadOnly = true;
            // 
            // KFRY
            // 
            this.KFRY.DataPropertyName = "KFRY";
            this.KFRY.HeaderText = "库房人员";
            this.KFRY.Name = "KFRY";
            this.KFRY.ReadOnly = true;
            // 
            // KFRQ
            // 
            this.KFRQ.DataPropertyName = "KFRQ";
            this.KFRQ.HeaderText = "确认日期";
            this.KFRQ.Name = "KFRQ";
            this.KFRQ.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.DataPropertyName = "Remark";
            this.Remark.HeaderText = "备注";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // 库房调拨单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 739);
            this.Controls.Add(this.dgv_Show);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "库房调拨单";
            this.Load += new System.EventHandler(this.库房调拨单_Load);
            this.Resize += new System.EventHandler(this.库房调拨单_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Show)).EndInit();
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
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorUpdate;
        private System.Windows.Forms.ToolStripButton btnFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorFind;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDJ_ZT;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.DataGridView dgv_Show;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn DJH;
        private System.Windows.Forms.DataGridViewTextBoxColumn InstoreRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn OutStoreRoom;
        private System.Windows.Forms.DataGridViewTextBoxColumn DJZT;
        private System.Windows.Forms.DataGridViewTextBoxColumn Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn LRRY;
        private System.Windows.Forms.DataGridViewTextBoxColumn LRRQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn SHRY;
        private System.Windows.Forms.DataGridViewTextBoxColumn SHRQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn 质量人员;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批准日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWRY;
        private System.Windows.Forms.DataGridViewTextBoxColumn CWRQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn KFRY;
        private System.Windows.Forms.DataGridViewTextBoxColumn KFRQ;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
    }
}
