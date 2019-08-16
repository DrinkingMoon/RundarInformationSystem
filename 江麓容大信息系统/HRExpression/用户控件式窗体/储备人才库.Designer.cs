namespace Form_Peripheral_HR
{
    partial class 储备人才库
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.导出toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.综合查询toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.plChoose = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.plChoose.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 9F);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripButton1,
            this.toolStripSeparator4,
            this.刷新toolStripButton1,
            this.toolStripSeparator1,
            this.导出toolStripButton,
            this.toolStripSeparator3,
            this.综合查询toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1162, 25);
            this.toolStrip1.TabIndex = 30;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripButton1
            // 
            this.新建toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripButton1.Name = "新建toolStripButton1";
            this.新建toolStripButton1.Size = new System.Drawing.Size(39, 22);
            this.新建toolStripButton1.Tag = "Add";
            this.新建toolStripButton1.Text = "新 建";
            this.新建toolStripButton1.Click += new System.EventHandler(this.新建toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 刷新toolStripButton1
            // 
            this.刷新toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton1.Name = "刷新toolStripButton1";
            this.刷新toolStripButton1.Size = new System.Drawing.Size(57, 22);
            this.刷新toolStripButton1.Tag = "Add";
            this.刷新toolStripButton1.Text = "刷新数据";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出toolStripButton
            // 
            this.导出toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出toolStripButton.Name = "导出toolStripButton";
            this.导出toolStripButton.Size = new System.Drawing.Size(87, 22);
            this.导出toolStripButton.Tag = "导出文件";
            this.导出toolStripButton.Text = "导出Excel文件";
            this.导出toolStripButton.Click += new System.EventHandler(this.导出toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // 综合查询toolStripButton3
            // 
            this.综合查询toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.综合查询toolStripButton3.Name = "综合查询toolStripButton3";
            this.综合查询toolStripButton3.Size = new System.Drawing.Size(57, 22);
            this.综合查询toolStripButton3.Tag = "导出文件";
            this.综合查询toolStripButton3.Text = "综合查询";
            this.综合查询toolStripButton3.Click += new System.EventHandler(this.综合查询toolStripButton3_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1162, 54);
            this.panel3.TabIndex = 31;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(467, 13);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "储备人才信息管理";
            // 
            // plChoose
            // 
            this.plChoose.Controls.Add(this.userControlDataLocalizer1);
            this.plChoose.Dock = System.Windows.Forms.DockStyle.Top;
            this.plChoose.Location = new System.Drawing.Point(0, 79);
            this.plChoose.Name = "plChoose";
            this.plChoose.Size = new System.Drawing.Size(1162, 38);
            this.plChoose.TabIndex = 36;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 117);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1162, 494);
            this.dataGridView1.TabIndex = 37;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1162, 38);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // 储备人才库
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1162, 611);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.plChoose);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "储备人才库";
            this.Text = "储备人才库";
            this.Load += new System.EventHandler(this.储备人才库_Load);
            this.Resize += new System.EventHandler(this.储备人才库_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.plChoose.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 导出toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 综合查询toolStripButton3;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel plChoose;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}