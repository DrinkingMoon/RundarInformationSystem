namespace Expression
{
    partial class 对应的隔离单
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.隔离单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供货单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供方批次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.退货数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(600, 46);
            this.panel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(513, 12);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "提交";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择ToolStripMenuItem,
            this.全选ToolStripMenuItem,
            this.取消ToolStripMenuItem,
            this.全消ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 92);
            // 
            // 选择ToolStripMenuItem
            // 
            this.选择ToolStripMenuItem.Name = "选择ToolStripMenuItem";
            this.选择ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.选择ToolStripMenuItem.Text = "选择";
            this.选择ToolStripMenuItem.Click += new System.EventHandler(this.选择ToolStripMenuItem_Click);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.全选ToolStripMenuItem.Text = "全部选中";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 取消ToolStripMenuItem
            // 
            this.取消ToolStripMenuItem.Name = "取消ToolStripMenuItem";
            this.取消ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.取消ToolStripMenuItem.Text = "取消";
            this.取消ToolStripMenuItem.Click += new System.EventHandler(this.取消ToolStripMenuItem_Click);
            // 
            // 全消ToolStripMenuItem
            // 
            this.全消ToolStripMenuItem.Name = "全消ToolStripMenuItem";
            this.全消ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.全消ToolStripMenuItem.Text = "全部取消";
            this.全消ToolStripMenuItem.Click += new System.EventHandler(this.全消ToolStripMenuItem_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sel,
            this.隔离单号,
            this.物品ID,
            this.物品名称,
            this.图号型号,
            this.规格,
            this.供货单位,
            this.批次号,
            this.供方批次号,
            this.退货数});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 46);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(600, 389);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // Sel
            // 
            this.Sel.DataPropertyName = "Sel";
            this.Sel.FalseValue = ((short)(0));
            this.Sel.HeaderText = "选";
            this.Sel.IndeterminateValue = ((short)(0));
            this.Sel.Name = "Sel";
            this.Sel.ReadOnly = true;
            this.Sel.TrueValue = ((short)(1));
            this.Sel.Width = 20;
            // 
            // 隔离单号
            // 
            this.隔离单号.DataPropertyName = "隔离单号";
            this.隔离单号.HeaderText = "隔离单号";
            this.隔离单号.Name = "隔离单号";
            this.隔离单号.ReadOnly = true;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "物品名称";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 供货单位
            // 
            this.供货单位.DataPropertyName = "供货单位";
            this.供货单位.HeaderText = "供货单位";
            this.供货单位.Name = "供货单位";
            this.供货单位.ReadOnly = true;
            // 
            // 批次号
            // 
            this.批次号.DataPropertyName = "批次号";
            this.批次号.HeaderText = "批次号";
            this.批次号.Name = "批次号";
            this.批次号.ReadOnly = true;
            // 
            // 供方批次号
            // 
            this.供方批次号.DataPropertyName = "供方批次号";
            this.供方批次号.HeaderText = "供方批次号";
            this.供方批次号.Name = "供方批次号";
            this.供方批次号.ReadOnly = true;
            // 
            // 退货数
            // 
            this.退货数.DataPropertyName = "退货数";
            this.退货数.HeaderText = "退货数";
            this.退货数.Name = "退货数";
            this.退货数.ReadOnly = true;
            // 
            // 对应的隔离单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(600, 435);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Name = "对应的隔离单";
            this.Text = "对应的隔离单";
            this.panel1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全消ToolStripMenuItem;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Sel;
        private System.Windows.Forms.DataGridViewTextBoxColumn 隔离单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供货单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供方批次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 退货数;
    }
}