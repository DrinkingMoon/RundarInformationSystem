namespace Expression
{
    partial class 订单核查
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.核查人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置订单信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.审核人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.审核通过ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.发布订单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出ExcleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimePicker2 = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Font = new System.Drawing.Font("宋体", 10F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.核查人操作ToolStripMenuItem,
            this.审核人操作ToolStripMenuItem,
            this.刷新数据ToolStripMenuItem,
            this.导出ExcleToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1089, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 核查人操作ToolStripMenuItem
            // 
            this.核查人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置订单信息ToolStripMenuItem,
            this.toolStripMenuItem2});
            this.核查人操作ToolStripMenuItem.Name = "核查人操作ToolStripMenuItem";
            this.核查人操作ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.核查人操作ToolStripMenuItem.Tag = "ADD";
            this.核查人操作ToolStripMenuItem.Text = "核查人操作";
            // 
            // 设置订单信息ToolStripMenuItem
            // 
            this.设置订单信息ToolStripMenuItem.Name = "设置订单信息ToolStripMenuItem";
            this.设置订单信息ToolStripMenuItem.Size = new System.Drawing.Size(158, 22);
            this.设置订单信息ToolStripMenuItem.Tag = "ADD";
            this.设置订单信息ToolStripMenuItem.Text = "设置订单信息";
            this.设置订单信息ToolStripMenuItem.Click += new System.EventHandler(this.设置订单信息ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(158, 22);
            this.toolStripMenuItem2.Tag = "ADD";
            this.toolStripMenuItem2.Text = "新建单据";
            this.toolStripMenuItem2.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 审核人操作ToolStripMenuItem
            // 
            this.审核人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.审核通过ToolStripMenuItem,
            this.发布订单ToolStripMenuItem});
            this.审核人操作ToolStripMenuItem.Name = "审核人操作ToolStripMenuItem";
            this.审核人操作ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.审核人操作ToolStripMenuItem.Tag = "Auditing";
            this.审核人操作ToolStripMenuItem.Text = "审核人操作";
            // 
            // 审核通过ToolStripMenuItem
            // 
            this.审核通过ToolStripMenuItem.Name = "审核通过ToolStripMenuItem";
            this.审核通过ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.审核通过ToolStripMenuItem.Tag = "Auditing";
            this.审核通过ToolStripMenuItem.Text = "审核通过";
            this.审核通过ToolStripMenuItem.Click += new System.EventHandler(this.审核通过ToolStripMenuItem_Click);
            // 
            // 发布订单ToolStripMenuItem
            // 
            this.发布订单ToolStripMenuItem.Name = "发布订单ToolStripMenuItem";
            this.发布订单ToolStripMenuItem.Size = new System.Drawing.Size(130, 22);
            this.发布订单ToolStripMenuItem.Tag = "Auditing";
            this.发布订单ToolStripMenuItem.Text = "发布订单";
            this.发布订单ToolStripMenuItem.Click += new System.EventHandler(this.发布订单ToolStripMenuItem_Click);
            // 
            // 刷新数据ToolStripMenuItem
            // 
            this.刷新数据ToolStripMenuItem.Name = "刷新数据ToolStripMenuItem";
            this.刷新数据ToolStripMenuItem.Size = new System.Drawing.Size(75, 20);
            this.刷新数据ToolStripMenuItem.Tag = "view";
            this.刷新数据ToolStripMenuItem.Text = "刷新数据";
            this.刷新数据ToolStripMenuItem.Click += new System.EventHandler(this.刷新数据ToolStripMenuItem_Click);
            // 
            // 导出ExcleToolStripMenuItem
            // 
            this.导出ExcleToolStripMenuItem.Name = "导出ExcleToolStripMenuItem";
            this.导出ExcleToolStripMenuItem.Size = new System.Drawing.Size(82, 20);
            this.导出ExcleToolStripMenuItem.Tag = "view";
            this.导出ExcleToolStripMenuItem.Text = "导出Excle";
            this.导出ExcleToolStripMenuItem.Click += new System.EventHandler(this.导出ExcleToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1089, 49);
            this.panel1.TabIndex = 44;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(426, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "订单核查";
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 73);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1089, 98);
            this.panelPara.TabIndex = 45;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.dateTimePicker2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePicker1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1089, 60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询信息";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(932, 23);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(87, 25);
            this.btnFind.TabIndex = 6;
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(657, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "单据状态:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "全    部",
            "等待审核",
            "等待发布",
            "单据已完成"});
            this.comboBox1.Location = new System.Drawing.Point(747, 24);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 21);
            this.comboBox1.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(323, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(70, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "结束时间:";
            // 
            // dateTimePicker2
            // 
            this.dateTimePicker2.Location = new System.Drawing.Point(404, 24);
            this.dateTimePicker2.Name = "dateTimePicker2";
            this.dateTimePicker2.Size = new System.Drawing.Size(180, 23);
            this.dateTimePicker2.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(70, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始时间:";
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(114, 24);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(180, 23);
            this.dateTimePicker1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 171);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1089, 480);
            this.dataGridView1.TabIndex = 47;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
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
            // 订单核查
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1089, 651);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "订单核查";
            this.Load += new System.EventHandler(this.订单核查_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 核查人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 审核人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 审核通过ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 发布订单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出ExcleToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimePicker2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ToolStripMenuItem 设置订单信息ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
    }
}
