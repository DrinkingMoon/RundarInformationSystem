namespace Form_Economic_Purchase
{
    partial class 发票信息编辑窗体
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
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.发票号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.发票日期 = new UniversalControlLibrary.DataGridViewDateTimePickColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.发票号,
            this.发票日期,
            this.单据号});
            this.customDataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(412, 319);
            this.customDataGridView1.TabIndex = 3;
            // 
            // 发票号
            // 
            this.发票号.DataPropertyName = "InvoiceNo";
            this.发票号.HeaderText = "发票号";
            this.发票号.Name = "发票号";
            this.发票号.Width = 250;
            // 
            // 发票日期
            // 
            this.发票日期.DataPropertyName = "InvoiceDate";
            this.发票日期.HeaderText = "发票日期";
            this.发票日期.Name = "发票日期";
            this.发票日期.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.发票日期.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "BillNo";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.ReadOnly = true;
            this.单据号.Visible = false;
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
            // 发票信息编辑窗体
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(412, 319);
            this.Controls.Add(this.customDataGridView1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "发票信息编辑窗体";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发票信息编辑窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.发票信息编辑窗体_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 发票号;
        private UniversalControlLibrary.DataGridViewDateTimePickColumn 发票日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
    }
}