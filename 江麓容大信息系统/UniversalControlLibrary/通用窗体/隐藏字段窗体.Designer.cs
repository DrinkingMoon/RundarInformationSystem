namespace UniversalControlLibrary
{
    partial class 隐藏字段窗体
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全部选中ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部取消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.字段名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(268, 392);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "设置隐藏字段";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选,
            this.字段名称});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 21;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(262, 372);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全部选中ToolStripMenuItem,
            this.全部取消ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 48);
            // 
            // 全部选中ToolStripMenuItem
            // 
            this.全部选中ToolStripMenuItem.Name = "全部选中ToolStripMenuItem";
            this.全部选中ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.全部选中ToolStripMenuItem.Text = "全部选中";
            this.全部选中ToolStripMenuItem.Click += new System.EventHandler(this.全部选中ToolStripMenuItem_Click);
            // 
            // 全部取消ToolStripMenuItem
            // 
            this.全部取消ToolStripMenuItem.Name = "全部取消ToolStripMenuItem";
            this.全部取消ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.全部取消ToolStripMenuItem.Text = "全部取消";
            this.全部取消ToolStripMenuItem.Click += new System.EventHandler(this.全部取消ToolStripMenuItem_Click);
            // 
            // 选
            // 
            this.选.DataPropertyName = "选";
            this.选.HeaderText = "选";
            this.选.Name = "选";
            this.选.ReadOnly = true;
            this.选.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.选.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.选.Width = 20;
            // 
            // 字段名称
            // 
            this.字段名称.DataPropertyName = "FieldName";
            this.字段名称.HeaderText = "字段名称";
            this.字段名称.Name = "字段名称";
            this.字段名称.ReadOnly = true;
            this.字段名称.Width = 150;
            // 
            // 隐藏字段窗体
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(268, 392);
            this.Controls.Add(this.groupBox2);
            this.Name = "隐藏字段窗体";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "隐藏字段窗体";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.隐藏字段窗体_FormClosing);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 全部选中ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部取消ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选;
        private System.Windows.Forms.DataGridViewTextBoxColumn 字段名称;

    }
}