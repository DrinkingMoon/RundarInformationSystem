namespace Expression
{
    partial class 供应质量信息反馈单
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
            this.qC操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQE操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.sQE意见提交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.验证结果提交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.qE操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.确认结果提交ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.回退单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出EXCEL表单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelPara = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.btnFind = new System.Windows.Forms.Button();
            this.label25 = new System.Windows.Forms.Label();
            this.cmbBillStatus = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.qC操作ToolStripMenuItem,
            this.sQE操作ToolStripMenuItem,
            this.qE操作ToolStripMenuItem,
            this.回退单据ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.导出EXCEL表单ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(895, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // qC操作ToolStripMenuItem
            // 
            this.qC操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提交单据ToolStripMenuItem});
            this.qC操作ToolStripMenuItem.Name = "qC操作ToolStripMenuItem";
            this.qC操作ToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.qC操作ToolStripMenuItem.Tag = "ADD";
            this.qC操作ToolStripMenuItem.Text = "QC操作";
            // 
            // 提交单据ToolStripMenuItem
            // 
            this.提交单据ToolStripMenuItem.Name = "提交单据ToolStripMenuItem";
            this.提交单据ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.提交单据ToolStripMenuItem.Tag = "ADD";
            this.提交单据ToolStripMenuItem.Text = "提交单据";
            this.提交单据ToolStripMenuItem.Click += new System.EventHandler(this.提交单据ToolStripMenuItem_Click);
            // 
            // sQE操作ToolStripMenuItem
            // 
            this.sQE操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.提交新建单据ToolStripMenuItem,
            this.toolStripSeparator1,
            this.sQE意见提交ToolStripMenuItem,
            this.验证结果提交ToolStripMenuItem,
            this.toolStripSeparator2,
            this.删除单据ToolStripMenuItem});
            this.sQE操作ToolStripMenuItem.Name = "sQE操作ToolStripMenuItem";
            this.sQE操作ToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.sQE操作ToolStripMenuItem.Tag = "Auditing";
            this.sQE操作ToolStripMenuItem.Text = "STA操作";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.新建单据ToolStripMenuItem.Tag = "Auditing";
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 提交新建单据ToolStripMenuItem
            // 
            this.提交新建单据ToolStripMenuItem.Name = "提交新建单据ToolStripMenuItem";
            this.提交新建单据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交新建单据ToolStripMenuItem.Tag = "Auditing";
            this.提交新建单据ToolStripMenuItem.Text = "提交新建单据";
            this.提交新建单据ToolStripMenuItem.Click += new System.EventHandler(this.提交新建单据ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(139, 6);
            this.toolStripSeparator1.Tag = "ADD";
            // 
            // sQE意见提交ToolStripMenuItem
            // 
            this.sQE意见提交ToolStripMenuItem.Name = "sQE意见提交ToolStripMenuItem";
            this.sQE意见提交ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.sQE意见提交ToolStripMenuItem.Tag = "Auditing";
            this.sQE意见提交ToolStripMenuItem.Text = "STA意见提交";
            this.sQE意见提交ToolStripMenuItem.Click += new System.EventHandler(this.sQE意见提交ToolStripMenuItem_Click);
            // 
            // 验证结果提交ToolStripMenuItem
            // 
            this.验证结果提交ToolStripMenuItem.Name = "验证结果提交ToolStripMenuItem";
            this.验证结果提交ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.验证结果提交ToolStripMenuItem.Tag = "Auditing";
            this.验证结果提交ToolStripMenuItem.Text = "验证结果提交";
            this.验证结果提交ToolStripMenuItem.Click += new System.EventHandler(this.验证结果提交ToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(139, 6);
            this.toolStripSeparator2.Tag = "ADD";
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.删除单据ToolStripMenuItem.Tag = "Auditing";
            this.删除单据ToolStripMenuItem.Text = "报废单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // qE操作ToolStripMenuItem
            // 
            this.qE操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.确认结果提交ToolStripMenuItem});
            this.qE操作ToolStripMenuItem.Name = "qE操作ToolStripMenuItem";
            this.qE操作ToolStripMenuItem.Size = new System.Drawing.Size(53, 20);
            this.qE操作ToolStripMenuItem.Tag = "Authorize";
            this.qE操作ToolStripMenuItem.Text = "QE操作";
            // 
            // 确认结果提交ToolStripMenuItem
            // 
            this.确认结果提交ToolStripMenuItem.Name = "确认结果提交ToolStripMenuItem";
            this.确认结果提交ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.确认结果提交ToolStripMenuItem.Tag = "Authorize";
            this.确认结果提交ToolStripMenuItem.Text = "确认结果提交";
            this.确认结果提交ToolStripMenuItem.Click += new System.EventHandler(this.确认结果提交ToolStripMenuItem_Click);
            // 
            // 回退单据ToolStripMenuItem
            // 
            this.回退单据ToolStripMenuItem.Name = "回退单据ToolStripMenuItem";
            this.回退单据ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.回退单据ToolStripMenuItem.Tag = "view";
            this.回退单据ToolStripMenuItem.Text = "回退单据";
            this.回退单据ToolStripMenuItem.Click += new System.EventHandler(this.回退单据ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.刷新ToolStripMenuItem.Tag = "view";
            this.刷新ToolStripMenuItem.Text = "刷新数据";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 导出EXCEL表单ToolStripMenuItem
            // 
            this.导出EXCEL表单ToolStripMenuItem.Name = "导出EXCEL表单ToolStripMenuItem";
            this.导出EXCEL表单ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.导出EXCEL表单ToolStripMenuItem.Tag = "view";
            this.导出EXCEL表单ToolStripMenuItem.Text = "查找";
            this.导出EXCEL表单ToolStripMenuItem.Click += new System.EventHandler(this.导出EXCEL表单ToolStripMenuItem_Click);
            // 
            // panelPara
            // 
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 119);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(895, 33);
            this.panelPara.TabIndex = 47;
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
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 24);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(895, 49);
            this.panel2.TabIndex = 49;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(303, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(255, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "供应质量信息反馈单";
            // 
            // groupBox6
            // 
            this.groupBox6.Controls.Add(this.btnFind);
            this.groupBox6.Controls.Add(this.label25);
            this.groupBox6.Controls.Add(this.cmbBillStatus);
            this.groupBox6.Controls.Add(this.label5);
            this.groupBox6.Controls.Add(this.dtpEndTime);
            this.groupBox6.Controls.Add(this.label6);
            this.groupBox6.Controls.Add(this.label7);
            this.groupBox6.Controls.Add(this.dtpStartTime);
            this.groupBox6.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox6.Location = new System.Drawing.Point(0, 73);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(895, 46);
            this.groupBox6.TabIndex = 166;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "数据筛选";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(797, 13);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(87, 25);
            this.btnFind.TabIndex = 182;
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(569, 18);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(63, 14);
            this.label25.TabIndex = 181;
            this.label25.Text = "单据状态";
            // 
            // cmbBillStatus
            // 
            this.cmbBillStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBillStatus.FormattingEnabled = true;
            this.cmbBillStatus.Items.AddRange(new object[] {
            "全  部",
            "等待STA意见",
            "等待STA验证",
            "等待质管部确认",
            "单据已完成",
            "单据已报废"});
            this.cmbBillStatus.Location = new System.Drawing.Point(638, 14);
            this.cmbBillStatus.Name = "cmbBillStatus";
            this.cmbBillStatus.Size = new System.Drawing.Size(140, 21);
            this.cmbBillStatus.TabIndex = 180;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(300, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 179;
            this.label5.Text = "截止日期";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(369, 14);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(184, 23);
            this.dtpEndTime.TabIndex = 178;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(273, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(21, 14);
            this.label6.TabIndex = 177;
            this.label6.Text = "到";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 176;
            this.label7.Text = "起始日期";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(83, 14);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(178, 23);
            this.dtpStartTime.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 152);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(895, 594);
            this.dataGridView1.TabIndex = 167;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // 供应质量信息反馈单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 746);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "供应质量信息反馈单";
            this.Load += new System.EventHandler(this.供应质量信息反馈单_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem qC操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQE操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQE意见提交ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem qE操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 验证结果提交ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 确认结果提交ToolStripMenuItem;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出EXCEL表单ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem 回退单据ToolStripMenuItem;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.ComboBox cmbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
    }
}
