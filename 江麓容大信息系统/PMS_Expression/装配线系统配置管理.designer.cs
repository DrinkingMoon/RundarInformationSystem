namespace Expression
{
    partial class 装配线系统配置管理
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panelPara = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btn工位管辖划分 = new System.Windows.Forms.ToolStripButton();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.参数名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.参数值范围 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备注 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Controls.Add(this.toolStrip1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(978, 674);
            this.panelMain.TabIndex = 29;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(970, 74);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(8, 600);
            this.panelRight.TabIndex = 39;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.customDataGridView1);
            this.panelCenter.Controls.Add(this.panelPara);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(8, 74);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(970, 600);
            this.panelCenter.TabIndex = 0;
            // 
            // panelPara
            // 
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 0);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(970, 61);
            this.panelPara.TabIndex = 42;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 592);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 8);
            this.panel2.TabIndex = 27;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 74);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(8, 600);
            this.panelLeft.TabIndex = 38;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(978, 49);
            this.panel1.TabIndex = 24;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(370, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(255, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "装配线系统配置管理";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btn工位管辖划分});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(978, 25);
            this.toolStrip1.TabIndex = 37;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btn工位管辖划分
            // 
            this.btn工位管辖划分.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn工位管辖划分.Name = "btn工位管辖划分";
            this.btn工位管辖划分.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btn工位管辖划分.Size = new System.Drawing.Size(84, 22);
            this.btn工位管辖划分.Text = "工位管辖划分";
            this.btn工位管辖划分.Click += new System.EventHandler(this.工位管辖划分_Click);
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.参数名称,
            this.参数值1,
            this.参数值2,
            this.参数值3,
            this.参数值4,
            this.参数值5,
            this.参数值6,
            this.参数值范围,
            this.备注,
            this.ID});
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 61);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(970, 531);
            this.customDataGridView1.TabIndex = 44;
            this.customDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellDoubleClick);
            // 
            // 参数名称
            // 
            this.参数名称.DataPropertyName = "DataType";
            this.参数名称.HeaderText = "参数名称";
            this.参数名称.Name = "参数名称";
            this.参数名称.ReadOnly = true;
            // 
            // 参数值1
            // 
            this.参数值1.DataPropertyName = "Value1";
            this.参数值1.HeaderText = "参数值1";
            this.参数值1.Name = "参数值1";
            this.参数值1.ReadOnly = true;
            // 
            // 参数值2
            // 
            this.参数值2.DataPropertyName = "Value2";
            this.参数值2.HeaderText = "参数值2";
            this.参数值2.Name = "参数值2";
            this.参数值2.ReadOnly = true;
            // 
            // 参数值3
            // 
            this.参数值3.DataPropertyName = "Value3";
            this.参数值3.HeaderText = "参数值3";
            this.参数值3.Name = "参数值3";
            this.参数值3.ReadOnly = true;
            // 
            // 参数值4
            // 
            this.参数值4.DataPropertyName = "Value4";
            this.参数值4.HeaderText = "参数值4";
            this.参数值4.Name = "参数值4";
            this.参数值4.ReadOnly = true;
            // 
            // 参数值5
            // 
            this.参数值5.DataPropertyName = "Value5";
            this.参数值5.HeaderText = "参数值5";
            this.参数值5.Name = "参数值5";
            this.参数值5.ReadOnly = true;
            // 
            // 参数值6
            // 
            this.参数值6.DataPropertyName = "Value6";
            this.参数值6.HeaderText = "参数值6";
            this.参数值6.Name = "参数值6";
            this.参数值6.ReadOnly = true;
            // 
            // 参数值范围
            // 
            this.参数值范围.DataPropertyName = "RangeOfValues";
            this.参数值范围.HeaderText = "参数值范围";
            this.参数值范围.Name = "参数值范围";
            this.参数值范围.ReadOnly = true;
            // 
            // 备注
            // 
            this.备注.DataPropertyName = "Remark";
            this.备注.HeaderText = "备注";
            this.备注.Name = "备注";
            this.备注.ReadOnly = true;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // 装配线系统配置管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(978, 674);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "装配线系统配置管理";
            this.Load += new System.EventHandler(this.装配线系统配置管理_Load);
            this.Resize += new System.EventHandler(this.装配线系统配置管理_Resize);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panelCenter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btn工位管辖划分;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值4;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值5;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值6;
        private System.Windows.Forms.DataGridViewTextBoxColumn 参数值范围;
        private System.Windows.Forms.DataGridViewTextBoxColumn 备注;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    }
}
