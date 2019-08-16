namespace Form_Peripheral_HR
{
    partial class FormPersonnelResume
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
            this.toolStripFuzzyFind = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbFindItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtContext = new System.Windows.Forms.ToolStripTextBox();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选定人员ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.toolStripFuzzyFind.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.toolStripFuzzyFind);
            this.panel1.Controls.Add(this.lblPrompt);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(652, 103);
            this.panel1.TabIndex = 40;
            // 
            // toolStripFuzzyFind
            // 
            this.toolStripFuzzyFind.AutoSize = false;
            this.toolStripFuzzyFind.BackColor = System.Drawing.Color.Transparent;
            this.toolStripFuzzyFind.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripFuzzyFind.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripFuzzyFind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbFindItem,
            this.toolStripLabel2,
            this.txtContext});
            this.toolStripFuzzyFind.Location = new System.Drawing.Point(0, 70);
            this.toolStripFuzzyFind.Name = "toolStripFuzzyFind";
            this.toolStripFuzzyFind.Size = new System.Drawing.Size(652, 33);
            this.toolStripFuzzyFind.TabIndex = 16;
            this.toolStripFuzzyFind.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(63, 30);
            this.toolStripLabel1.Text = "查找条件";
            // 
            // cmbFindItem
            // 
            this.cmbFindItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFindItem.Name = "cmbFindItem";
            this.cmbFindItem.Size = new System.Drawing.Size(186, 33);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(98, 30);
            this.toolStripLabel2.Text = "   全部或部分";
            // 
            // txtContext
            // 
            this.txtContext.Name = "txtContext";
            this.txtContext.Size = new System.Drawing.Size(186, 33);
            this.txtContext.TextChanged += new System.EventHandler(this.txtContext_TextChanged);
            // 
            // lblPrompt
            // 
            this.lblPrompt.AutoSize = true;
            this.lblPrompt.Location = new System.Drawing.Point(3, 50);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(245, 14);
            this.lblPrompt.TabIndex = 11;
            this.lblPrompt.Text = "提示信息：双击记录行或右键完成选择";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(226, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "人员简历信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 103);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(652, 438);
            this.dataGridView1.TabIndex = 41;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选定人员ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // 选定人员ToolStripMenuItem
            // 
            this.选定人员ToolStripMenuItem.Name = "选定人员ToolStripMenuItem";
            this.选定人员ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.选定人员ToolStripMenuItem.Text = "选定人员";
            this.选定人员ToolStripMenuItem.Click += new System.EventHandler(this.选定人员ToolStripMenuItem_Click);
            // 
            // FormPersonnelResume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(652, 541);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "FormPersonnelResume";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "人员简历";
            this.Load += new System.EventHandler(this.FormPersonnelResume_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStripFuzzyFind.ResumeLayout(false);
            this.toolStripFuzzyFind.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStripFuzzyFind;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cmbFindItem;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripTextBox txtContext;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选定人员ToolStripMenuItem;

    }
}