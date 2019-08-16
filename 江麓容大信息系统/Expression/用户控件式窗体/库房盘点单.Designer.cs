namespace Expression
{
    partial class 库房盘点单
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
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.编制人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置盘点明细ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.部门主管操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.部门主管审核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.打印单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.分管领导操作ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.分管领导批准ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.分管领导操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批准通过ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.仓管员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.盘点确认ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.lblBillStatus = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtBill_ID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbPDFS = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.编制人操作ToolStripMenuItem,
            this.部门主管操作ToolStripMenuItem,
            this.分管领导操作ToolStripMenuItem1,
            this.分管领导操作ToolStripMenuItem,
            this.仓管员操作ToolStripMenuItem,
            this.刷新数据ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip.TabIndex = 50;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 编制人操作ToolStripMenuItem
            // 
            this.编制人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.设置盘点明细ToolStripMenuItem,
            this.提交单据ToolStripMenuItem,
            this.toolStripMenuItem4,
            this.删除单据ToolStripMenuItem});
            this.编制人操作ToolStripMenuItem.Name = "编制人操作ToolStripMenuItem";
            this.编制人操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.编制人操作ToolStripMenuItem.Tag = "Add";
            this.编制人操作ToolStripMenuItem.Text = "编制人操作";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.新建单据ToolStripMenuItem.Tag = "Add";
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 设置盘点明细ToolStripMenuItem
            // 
            this.设置盘点明细ToolStripMenuItem.Name = "设置盘点明细ToolStripMenuItem";
            this.设置盘点明细ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.设置盘点明细ToolStripMenuItem.Tag = "Add";
            this.设置盘点明细ToolStripMenuItem.Text = "设置盘点明细";
            this.设置盘点明细ToolStripMenuItem.Click += new System.EventHandler(this.设置盘点明细ToolStripMenuItem_Click);
            // 
            // 提交单据ToolStripMenuItem
            // 
            this.提交单据ToolStripMenuItem.Name = "提交单据ToolStripMenuItem";
            this.提交单据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交单据ToolStripMenuItem.Tag = "Add";
            this.提交单据ToolStripMenuItem.Text = "提交单据";
            this.提交单据ToolStripMenuItem.Click += new System.EventHandler(this.提交单据ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(139, 6);
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.删除单据ToolStripMenuItem.Tag = "Add";
            this.删除单据ToolStripMenuItem.Text = "报废单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // 部门主管操作ToolStripMenuItem
            // 
            this.部门主管操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.部门主管审核ToolStripMenuItem,
            this.toolStripSeparator1,
            this.打印单据ToolStripMenuItem});
            this.部门主管操作ToolStripMenuItem.Name = "部门主管操作ToolStripMenuItem";
            this.部门主管操作ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.部门主管操作ToolStripMenuItem.Tag = "Auditing";
            this.部门主管操作ToolStripMenuItem.Text = "部门主管操作";
            // 
            // 部门主管审核ToolStripMenuItem
            // 
            this.部门主管审核ToolStripMenuItem.Name = "部门主管审核ToolStripMenuItem";
            this.部门主管审核ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.部门主管审核ToolStripMenuItem.Tag = "Auditing";
            this.部门主管审核ToolStripMenuItem.Text = "部门主管审核";
            this.部门主管审核ToolStripMenuItem.Click += new System.EventHandler(this.部门主管审核ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // 打印单据ToolStripMenuItem
            // 
            this.打印单据ToolStripMenuItem.Name = "打印单据ToolStripMenuItem";
            this.打印单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打印单据ToolStripMenuItem.Tag = "Auditing";
            this.打印单据ToolStripMenuItem.Text = "打印单据";
            this.打印单据ToolStripMenuItem.Click += new System.EventHandler(this.打印单据ToolStripMenuItem_Click);
            // 
            // 分管领导操作ToolStripMenuItem1
            // 
            this.分管领导操作ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.分管领导批准ToolStripMenuItem});
            this.分管领导操作ToolStripMenuItem1.Name = "分管领导操作ToolStripMenuItem1";
            this.分管领导操作ToolStripMenuItem1.Size = new System.Drawing.Size(77, 20);
            this.分管领导操作ToolStripMenuItem1.Tag = "Retrial_1";
            this.分管领导操作ToolStripMenuItem1.Text = "负责人操作";
            // 
            // 分管领导批准ToolStripMenuItem
            // 
            this.分管领导批准ToolStripMenuItem.Name = "分管领导批准ToolStripMenuItem";
            this.分管领导批准ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.分管领导批准ToolStripMenuItem.Tag = "Retrial_1";
            this.分管领导批准ToolStripMenuItem.Text = "负责人批准";
            this.分管领导批准ToolStripMenuItem.Click += new System.EventHandler(this.分管领导批准ToolStripMenuItem_Click);
            // 
            // 分管领导操作ToolStripMenuItem
            // 
            this.分管领导操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.批准通过ToolStripMenuItem});
            this.分管领导操作ToolStripMenuItem.Name = "分管领导操作ToolStripMenuItem";
            this.分管领导操作ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.分管领导操作ToolStripMenuItem.Tag = "Authorize";
            this.分管领导操作ToolStripMenuItem.Text = "财务操作";
            // 
            // 批准通过ToolStripMenuItem
            // 
            this.批准通过ToolStripMenuItem.Name = "批准通过ToolStripMenuItem";
            this.批准通过ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.批准通过ToolStripMenuItem.Tag = "Authorize";
            this.批准通过ToolStripMenuItem.Text = "财务批准";
            this.批准通过ToolStripMenuItem.Click += new System.EventHandler(this.批准通过ToolStripMenuItem_Click);
            // 
            // 仓管员操作ToolStripMenuItem
            // 
            this.仓管员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.盘点确认ToolStripMenuItem});
            this.仓管员操作ToolStripMenuItem.Name = "仓管员操作ToolStripMenuItem";
            this.仓管员操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.仓管员操作ToolStripMenuItem.Tag = "StockIn";
            this.仓管员操作ToolStripMenuItem.Text = "仓管员操作";
            // 
            // 盘点确认ToolStripMenuItem
            // 
            this.盘点确认ToolStripMenuItem.Name = "盘点确认ToolStripMenuItem";
            this.盘点确认ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.盘点确认ToolStripMenuItem.Tag = "StockIn";
            this.盘点确认ToolStripMenuItem.Text = "盘点确认";
            this.盘点确认ToolStripMenuItem.Click += new System.EventHandler(this.盘点确认ToolStripMenuItem_Click);
            // 
            // 刷新数据ToolStripMenuItem
            // 
            this.刷新数据ToolStripMenuItem.Name = "刷新数据ToolStripMenuItem";
            this.刷新数据ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.刷新数据ToolStripMenuItem.Tag = "view";
            this.刷新数据ToolStripMenuItem.Text = "刷新数据";
            this.刷新数据ToolStripMenuItem.Click += new System.EventHandler(this.刷新数据ToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.label11);
            this.panel3.Controls.Add(this.lblBillStatus);
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1051, 49);
            this.panel3.TabIndex = 51;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(20, 20);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(65, 12);
            this.label11.TabIndex = 194;
            this.label11.Text = "单据状态：";
            // 
            // lblBillStatus
            // 
            this.lblBillStatus.AutoSize = true;
            this.lblBillStatus.Location = new System.Drawing.Point(106, 20);
            this.lblBillStatus.Name = "lblBillStatus";
            this.lblBillStatus.Size = new System.Drawing.Size(0, 14);
            this.lblBillStatus.TabIndex = 193;
            this.lblBillStatus.TextChanged += new System.EventHandler(this.lblBillStatus_TextChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(456, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(147, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "库房盘点单";
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 73);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1051, 88);
            this.panelPara.TabIndex = 52;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtBill_ID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbPDFS);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbStorage);
            this.groupBox1.Controls.Add(this.label36);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1051, 56);
            this.groupBox1.TabIndex = 53;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据信息区";
            // 
            // txtBill_ID
            // 
            this.txtBill_ID.BackColor = System.Drawing.Color.White;
            this.txtBill_ID.ForeColor = System.Drawing.Color.Red;
            this.txtBill_ID.Location = new System.Drawing.Point(141, 22);
            this.txtBill_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBill_ID.Name = "txtBill_ID";
            this.txtBill_ID.ReadOnly = true;
            this.txtBill_ID.Size = new System.Drawing.Size(199, 23);
            this.txtBill_ID.TabIndex = 196;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 197;
            this.label2.Text = "盘点单号";
            // 
            // cmbPDFS
            // 
            this.cmbPDFS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPDFS.FormattingEnabled = true;
            this.cmbPDFS.Items.AddRange(new object[] {
            "全库房盘点",
            "分类别盘点",
            "自定义盘点"});
            this.cmbPDFS.Location = new System.Drawing.Point(854, 22);
            this.cmbPDFS.Name = "cmbPDFS";
            this.cmbPDFS.Size = new System.Drawing.Size(151, 21);
            this.cmbPDFS.TabIndex = 195;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(733, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 194;
            this.label1.Text = "盘点方式";
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(521, 22);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(151, 21);
            this.cmbStorage.TabIndex = 193;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(400, 26);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(63, 14);
            this.label36.TabIndex = 192;
            this.label36.Text = "所属库房";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 161);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1051, 637);
            this.dataGridView1.TabIndex = 54;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // 库房盘点单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 798);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "库房盘点单";
            this.Load += new System.EventHandler(this.库房盘点单_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 编制人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 部门主管操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 部门主管审核ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分管领导操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 批准通过ToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripMenuItem 仓管员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 盘点确认ToolStripMenuItem;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtBill_ID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbPDFS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.Label lblBillStatus;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripMenuItem 设置盘点明细ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 分管领导操作ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 分管领导批准ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 打印单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新数据ToolStripMenuItem;

    }
}
