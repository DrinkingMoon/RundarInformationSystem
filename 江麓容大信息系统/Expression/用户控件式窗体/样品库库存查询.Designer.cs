namespace Expression
{
    partial class 样品库库存查询
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
            this.查询库存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出EXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelPara = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkIsShowZeroStock = new System.Windows.Forms.CheckBox();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtShelf = new System.Windows.Forms.TextBox();
            this.label28 = new System.Windows.Forms.Label();
            this.txtColumn = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.txtLayer = new System.Windows.Forms.TextBox();
            this.label30 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查询库存ToolStripMenuItem,
            this.导出EXCELToolStripMenuItem,
            this.修改ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(895, 24);
            this.menuStrip.TabIndex = 48;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 查询库存ToolStripMenuItem
            // 
            this.查询库存ToolStripMenuItem.Name = "查询库存ToolStripMenuItem";
            this.查询库存ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.查询库存ToolStripMenuItem.Tag = "view";
            this.查询库存ToolStripMenuItem.Text = "查询库存";
            this.查询库存ToolStripMenuItem.Click += new System.EventHandler(this.查询库存ToolStripMenuItem_Click);
            // 
            // 导出EXCELToolStripMenuItem
            // 
            this.导出EXCELToolStripMenuItem.Name = "导出EXCELToolStripMenuItem";
            this.导出EXCELToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.导出EXCELToolStripMenuItem.Tag = "view";
            this.导出EXCELToolStripMenuItem.Text = "导出EXCEL";
            this.导出EXCELToolStripMenuItem.Click += new System.EventHandler(this.导出EXCELToolStripMenuItem_Click);
            // 
            // 修改ToolStripMenuItem
            // 
            this.修改ToolStripMenuItem.Name = "修改ToolStripMenuItem";
            this.修改ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.修改ToolStripMenuItem.Tag = "Add";
            this.修改ToolStripMenuItem.Text = "修改库存信息";
            this.修改ToolStripMenuItem.Click += new System.EventHandler(this.修改ToolStripMenuItem_Click);
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.panel1);
            this.panelPara.Controls.Add(this.panel2);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 24);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(895, 178);
            this.panelPara.TabIndex = 49;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtBatchNo);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.txtSpec);
            this.panel1.Controls.Add(this.label31);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtShelf);
            this.panel1.Controls.Add(this.label28);
            this.panel1.Controls.Add(this.txtColumn);
            this.panel1.Controls.Add(this.label29);
            this.panel1.Controls.Add(this.txtLayer);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(895, 89);
            this.panel1.TabIndex = 52;
            // 
            // chkIsShowZeroStock
            // 
            this.chkIsShowZeroStock.AutoSize = true;
            this.chkIsShowZeroStock.Checked = true;
            this.chkIsShowZeroStock.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsShowZeroStock.Location = new System.Drawing.Point(52, 18);
            this.chkIsShowZeroStock.Name = "chkIsShowZeroStock";
            this.chkIsShowZeroStock.Size = new System.Drawing.Size(89, 18);
            this.chkIsShowZeroStock.TabIndex = 190;
            this.chkIsShowZeroStock.Text = "显示0库存";
            this.chkIsShowZeroStock.UseVisualStyleBackColor = true;
            this.chkIsShowZeroStock.CheckedChanged += new System.EventHandler(this.chkIsShowZeroStock_CheckedChanged);
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.BackColor = System.Drawing.Color.White;
            this.txtBatchNo.ForeColor = System.Drawing.Color.Red;
            this.txtBatchNo.Location = new System.Drawing.Point(86, 50);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ReadOnly = true;
            this.txtBatchNo.Size = new System.Drawing.Size(170, 23);
            this.txtBatchNo.TabIndex = 188;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(17, 54);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 14);
            this.label13.TabIndex = 189;
            this.label13.Text = "批 次 号";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(10, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 14);
            this.label12.TabIndex = 187;
            this.label12.Text = "图号/型号";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(86, 15);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(247, 23);
            this.txtCode.TabIndex = 182;
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Location = new System.Drawing.Point(689, 15);
            this.txtSpec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(203, 23);
            this.txtSpec.TabIndex = 184;
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(621, 20);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(63, 14);
            this.label31.TabIndex = 186;
            this.label31.Text = "规    格";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(415, 15);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(192, 23);
            this.txtName.TabIndex = 183;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(346, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 185;
            this.label1.Text = "物品名称";
            // 
            // txtShelf
            // 
            this.txtShelf.BackColor = System.Drawing.Color.White;
            this.txtShelf.Location = new System.Drawing.Point(341, 50);
            this.txtShelf.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtShelf.Name = "txtShelf";
            this.txtShelf.Size = new System.Drawing.Size(95, 23);
            this.txtShelf.TabIndex = 112;
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(272, 54);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(63, 14);
            this.label28.TabIndex = 115;
            this.label28.Text = "区    域";
            // 
            // txtColumn
            // 
            this.txtColumn.BackColor = System.Drawing.Color.White;
            this.txtColumn.Location = new System.Drawing.Point(513, 50);
            this.txtColumn.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtColumn.Name = "txtColumn";
            this.txtColumn.Size = new System.Drawing.Size(94, 23);
            this.txtColumn.TabIndex = 113;
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(444, 54);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(63, 14);
            this.label29.TabIndex = 116;
            this.label29.Text = "区    号";
            // 
            // txtLayer
            // 
            this.txtLayer.BackColor = System.Drawing.Color.White;
            this.txtLayer.Location = new System.Drawing.Point(689, 50);
            this.txtLayer.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtLayer.Name = "txtLayer";
            this.txtLayer.Size = new System.Drawing.Size(77, 23);
            this.txtLayer.TabIndex = 114;
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(621, 54);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(63, 14);
            this.label30.TabIndex = 117;
            this.label30.Text = "层    号";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.chkIsShowZeroStock);
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(895, 49);
            this.panel2.TabIndex = 45;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(335, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "样品库库存查询";
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
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 202);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(895, 544);
            this.dataGridView1.TabIndex = 52;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // 样品库库存查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(895, 746);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "样品库库存查询";
            this.Load += new System.EventHandler(this.样品库库存查询_Load);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 查询库存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出EXCELToolStripMenuItem;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtShelf;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.TextBox txtColumn;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox txtLayer;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ToolStripMenuItem 修改ToolStripMenuItem;
        private System.Windows.Forms.CheckBox chkIsShowZeroStock;
    }
}
