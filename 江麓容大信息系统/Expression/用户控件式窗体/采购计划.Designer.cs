namespace Expression
{
    partial class 采购计划
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
            this.采购计划公式编写ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.保存采购计划ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出EXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbPrice = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbDJZT = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnGetNewInfo = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.lb_PZRQ = new System.Windows.Forms.Label();
            this.lb_PZR = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lb_SHRQ = new System.Windows.Forms.Label();
            this.lb_SHR = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lb_BZRQ = new System.Windows.Forms.Label();
            this.lb_BZR = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.确认采购计划ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.导出EXCELToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip.TabIndex = 51;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 编制人操作ToolStripMenuItem
            // 
            this.编制人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.采购计划公式编写ToolStripMenuItem,
            this.保存采购计划ToolStripMenuItem,
            this.确认采购计划ToolStripMenuItem});
            this.编制人操作ToolStripMenuItem.Name = "编制人操作ToolStripMenuItem";
            this.编制人操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.编制人操作ToolStripMenuItem.Tag = "Add";
            this.编制人操作ToolStripMenuItem.Text = "编制人操作";
            // 
            // 采购计划公式编写ToolStripMenuItem
            // 
            this.采购计划公式编写ToolStripMenuItem.Name = "采购计划公式编写ToolStripMenuItem";
            this.采购计划公式编写ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.采购计划公式编写ToolStripMenuItem.Tag = "ADD";
            this.采购计划公式编写ToolStripMenuItem.Text = "采购计划公式编写";
            this.采购计划公式编写ToolStripMenuItem.Click += new System.EventHandler(this.采购计划公式编写ToolStripMenuItem_Click);
            // 
            // 保存采购计划ToolStripMenuItem
            // 
            this.保存采购计划ToolStripMenuItem.Name = "保存采购计划ToolStripMenuItem";
            this.保存采购计划ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.保存采购计划ToolStripMenuItem.Tag = "ADD";
            this.保存采购计划ToolStripMenuItem.Text = "保存采购计划";
            this.保存采购计划ToolStripMenuItem.Click += new System.EventHandler(this.保存采购计划ToolStripMenuItem_Click);
            // 
            // 导出EXCELToolStripMenuItem
            // 
            this.导出EXCELToolStripMenuItem.Name = "导出EXCELToolStripMenuItem";
            this.导出EXCELToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.导出EXCELToolStripMenuItem.Tag = "view";
            this.导出EXCELToolStripMenuItem.Text = "导出EXCEL";
            this.导出EXCELToolStripMenuItem.Click += new System.EventHandler(this.导出EXCELToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.lbPrice);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.lbDJZT);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1051, 49);
            this.panel1.TabIndex = 52;
            // 
            // lbPrice
            // 
            this.lbPrice.AutoSize = true;
            this.lbPrice.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbPrice.Location = new System.Drawing.Point(854, 20);
            this.lbPrice.Name = "lbPrice";
            this.lbPrice.Size = new System.Drawing.Size(35, 14);
            this.lbPrice.TabIndex = 15;
            this.lbPrice.Text = "0.00";
            this.lbPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(755, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 14;
            this.label6.Text = "订货总金额：";
            // 
            // lbDJZT
            // 
            this.lbDJZT.AutoSize = true;
            this.lbDJZT.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDJZT.ForeColor = System.Drawing.Color.Red;
            this.lbDJZT.Location = new System.Drawing.Point(110, 20);
            this.lbDJZT.Name = "lbDJZT";
            this.lbDJZT.Size = new System.Drawing.Size(35, 14);
            this.lbDJZT.TabIndex = 13;
            this.lbDJZT.Text = "DJZT";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label32.Location = new System.Drawing.Point(21, 20);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(77, 14);
            this.label32.TabIndex = 12;
            this.label32.Text = "单据状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(460, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "采购计划";
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 73);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1051, 125);
            this.panelPara.TabIndex = 54;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGetNewInfo);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.lb_PZRQ);
            this.groupBox1.Controls.Add(this.lb_PZR);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lb_SHRQ);
            this.groupBox1.Controls.Add(this.lb_SHR);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lb_BZRQ);
            this.groupBox1.Controls.Add(this.lb_BZR);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbMonth);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbYear);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1051, 83);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主表信息";
            // 
            // btnGetNewInfo
            // 
            this.btnGetNewInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetNewInfo.Location = new System.Drawing.Point(27, 52);
            this.btnGetNewInfo.Name = "btnGetNewInfo";
            this.btnGetNewInfo.Size = new System.Drawing.Size(161, 27);
            this.btnGetNewInfo.TabIndex = 225;
            this.btnGetNewInfo.Text = "获取最新采购计划";
            this.btnGetNewInfo.UseVisualStyleBackColor = true;
            this.btnGetNewInfo.Click += new System.EventHandler(this.btnGetNewInfo_Click);
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnFind.Location = new System.Drawing.Point(206, 52);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(87, 27);
            this.btnFind.TabIndex = 224;
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // lb_PZRQ
            // 
            this.lb_PZRQ.AutoSize = true;
            this.lb_PZRQ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_PZRQ.Location = new System.Drawing.Point(955, 53);
            this.lb_PZRQ.Name = "lb_PZRQ";
            this.lb_PZRQ.Size = new System.Drawing.Size(35, 14);
            this.lb_PZRQ.TabIndex = 223;
            this.lb_PZRQ.Text = "PZRQ";
            this.lb_PZRQ.Visible = false;
            // 
            // lb_PZR
            // 
            this.lb_PZR.AutoSize = true;
            this.lb_PZR.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_PZR.Location = new System.Drawing.Point(955, 22);
            this.lb_PZR.Name = "lb_PZR";
            this.lb_PZR.Size = new System.Drawing.Size(28, 14);
            this.lb_PZR.TabIndex = 222;
            this.lb_PZR.Text = "PZR";
            this.lb_PZR.Visible = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(871, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 221;
            this.label12.Text = "批准日期";
            this.label12.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(871, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 220;
            this.label3.Text = "批 准 人";
            this.label3.Visible = false;
            // 
            // lb_SHRQ
            // 
            this.lb_SHRQ.AutoSize = true;
            this.lb_SHRQ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_SHRQ.Location = new System.Drawing.Point(701, 53);
            this.lb_SHRQ.Name = "lb_SHRQ";
            this.lb_SHRQ.Size = new System.Drawing.Size(35, 14);
            this.lb_SHRQ.TabIndex = 219;
            this.lb_SHRQ.Text = "SHRQ";
            this.lb_SHRQ.Visible = false;
            // 
            // lb_SHR
            // 
            this.lb_SHR.AutoSize = true;
            this.lb_SHR.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_SHR.Location = new System.Drawing.Point(701, 22);
            this.lb_SHR.Name = "lb_SHR";
            this.lb_SHR.Size = new System.Drawing.Size(28, 14);
            this.lb_SHR.TabIndex = 218;
            this.lb_SHR.Text = "SHR";
            this.lb_SHR.Visible = false;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(616, 53);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 217;
            this.label8.Text = "审核日期";
            this.label8.Visible = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(616, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 216;
            this.label9.Text = "审 核 人";
            this.label9.Visible = false;
            // 
            // lb_BZRQ
            // 
            this.lb_BZRQ.AutoSize = true;
            this.lb_BZRQ.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_BZRQ.Location = new System.Drawing.Point(456, 53);
            this.lb_BZRQ.Name = "lb_BZRQ";
            this.lb_BZRQ.Size = new System.Drawing.Size(35, 14);
            this.lb_BZRQ.TabIndex = 215;
            this.lb_BZRQ.Text = "BZRQ";
            // 
            // lb_BZR
            // 
            this.lb_BZR.AutoSize = true;
            this.lb_BZR.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lb_BZR.Location = new System.Drawing.Point(456, 22);
            this.lb_BZR.Name = "lb_BZR";
            this.lb_BZR.Size = new System.Drawing.Size(28, 14);
            this.lb_BZR.TabIndex = 214;
            this.lb_BZR.Text = "BZR";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(369, 53);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 213;
            this.label4.Text = "编制日期";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(369, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 212;
            this.label5.Text = "编 制 人";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label2.Location = new System.Drawing.Point(296, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 211;
            this.label2.Text = "月";
            // 
            // cmbMonth
            // 
            this.cmbMonth.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Location = new System.Drawing.Point(232, 20);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(61, 21);
            this.cmbMonth.TabIndex = 210;
            this.cmbMonth.SelectedValueChanged += new System.EventHandler(this.cmbMonth_SelectedValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(202, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 14);
            this.label1.TabIndex = 209;
            this.label1.Text = "年";
            // 
            // cmbYear
            // 
            this.cmbYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(113, 20);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(84, 21);
            this.cmbYear.TabIndex = 208;
            this.cmbYear.SelectedValueChanged += new System.EventHandler(this.cmbYear_SelectedValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(23, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(70, 14);
            this.label13.TabIndex = 207;
            this.label13.Text = "查询年月:";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 198);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1051, 635);
            this.dataGridView1.TabIndex = 55;
            this.dataGridView1.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dataGridView1_CellBeginEdit);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
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
            // 确认采购计划ToolStripMenuItem
            // 
            this.确认采购计划ToolStripMenuItem.Name = "确认采购计划ToolStripMenuItem";
            this.确认采购计划ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.确认采购计划ToolStripMenuItem.Tag = "ADD";
            this.确认采购计划ToolStripMenuItem.Text = "确认采购计划";
            this.确认采购计划ToolStripMenuItem.Click += new System.EventHandler(this.确认采购计划ToolStripMenuItem_Click);
            // 
            // 采购计划
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 833);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "采购计划";
            this.Load += new System.EventHandler(this.采购计划_Load);
            this.Resize += new System.EventHandler(this.采购计划_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
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

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 编制人操作ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbDJZT;
        private System.Windows.Forms.Label label32;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.ToolStripMenuItem 导出EXCELToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label lb_PZRQ;
        private System.Windows.Forms.Label lb_PZR;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lb_SHRQ;
        private System.Windows.Forms.Label lb_SHR;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lb_BZRQ;
        private System.Windows.Forms.Label lb_BZR;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripMenuItem 保存采购计划ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnGetNewInfo;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbPrice;
        private System.Windows.Forms.ToolStripMenuItem 采购计划公式编写ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 确认采购计划ToolStripMenuItem;
    }
}
