using UniversalControlLibrary;
namespace Expression
{
    partial class 非产品件检验单
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.申请人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.要求人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交检验要求ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交验证要求ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检验人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交检验结果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交验证结果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.判定人操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交最终判定ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.申请人操作ToolStripMenuItem,
            this.要求人操作ToolStripMenuItem,
            this.检验人操作ToolStripMenuItem,
            this.判定人操作ToolStripMenuItem,
            this.刷新ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(984, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 申请人操作ToolStripMenuItem
            // 
            this.申请人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.提交单据ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.删除单据ToolStripMenuItem});
            this.申请人操作ToolStripMenuItem.Name = "申请人操作ToolStripMenuItem";
            this.申请人操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.申请人操作ToolStripMenuItem.Text = "申请人操作";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 提交单据ToolStripMenuItem
            // 
            this.提交单据ToolStripMenuItem.Name = "提交单据ToolStripMenuItem";
            this.提交单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交单据ToolStripMenuItem.Text = "提交单据";
            this.提交单据ToolStripMenuItem.Click += new System.EventHandler(this.提交单据ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除单据ToolStripMenuItem.Text = "删除单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // 要求人操作ToolStripMenuItem
            // 
            this.要求人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提交检验要求ToolStripMenuItem,
            this.提交验证要求ToolStripMenuItem});
            this.要求人操作ToolStripMenuItem.Name = "要求人操作ToolStripMenuItem";
            this.要求人操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.要求人操作ToolStripMenuItem.Text = "要求人操作";
            // 
            // 提交检验要求ToolStripMenuItem
            // 
            this.提交检验要求ToolStripMenuItem.Name = "提交检验要求ToolStripMenuItem";
            this.提交检验要求ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交检验要求ToolStripMenuItem.Text = "提交检验要求";
            this.提交检验要求ToolStripMenuItem.Click += new System.EventHandler(this.提交检验要求ToolStripMenuItem_Click);
            // 
            // 提交验证要求ToolStripMenuItem
            // 
            this.提交验证要求ToolStripMenuItem.Name = "提交验证要求ToolStripMenuItem";
            this.提交验证要求ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交验证要求ToolStripMenuItem.Text = "提交验证要求";
            this.提交验证要求ToolStripMenuItem.Click += new System.EventHandler(this.提交验证要求ToolStripMenuItem_Click);
            // 
            // 检验人操作ToolStripMenuItem
            // 
            this.检验人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提交检验结果ToolStripMenuItem,
            this.提交验证结果ToolStripMenuItem});
            this.检验人操作ToolStripMenuItem.Name = "检验人操作ToolStripMenuItem";
            this.检验人操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.检验人操作ToolStripMenuItem.Text = "检验人操作";
            // 
            // 提交检验结果ToolStripMenuItem
            // 
            this.提交检验结果ToolStripMenuItem.Name = "提交检验结果ToolStripMenuItem";
            this.提交检验结果ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交检验结果ToolStripMenuItem.Text = "提交检验结果";
            this.提交检验结果ToolStripMenuItem.Click += new System.EventHandler(this.提交检验结果ToolStripMenuItem_Click);
            // 
            // 提交验证结果ToolStripMenuItem
            // 
            this.提交验证结果ToolStripMenuItem.Name = "提交验证结果ToolStripMenuItem";
            this.提交验证结果ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交验证结果ToolStripMenuItem.Text = "提交验证结果";
            this.提交验证结果ToolStripMenuItem.Click += new System.EventHandler(this.提交验证结果ToolStripMenuItem_Click);
            // 
            // 判定人操作ToolStripMenuItem
            // 
            this.判定人操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提交最终判定ToolStripMenuItem});
            this.判定人操作ToolStripMenuItem.Name = "判定人操作ToolStripMenuItem";
            this.判定人操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.判定人操作ToolStripMenuItem.Text = "判定人操作";
            // 
            // 提交最终判定ToolStripMenuItem
            // 
            this.提交最终判定ToolStripMenuItem.Name = "提交最终判定ToolStripMenuItem";
            this.提交最终判定ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.提交最终判定ToolStripMenuItem.Text = "提交最终判定";
            this.提交最终判定ToolStripMenuItem.Click += new System.EventHandler(this.提交最终判定ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(984, 45);
            this.panel1.TabIndex = 45;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(392, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "非产品件检验单";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 147);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(984, 590);
            this.dataGridView1.TabIndex = 49;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 115);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(984, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 47;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 69);
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(984, 46);
            this.checkBillDateAndStatus1.TabIndex = 46;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
            // 
            // 非产品件检验单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 737);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "非产品件检验单";
            this.Text = "非产品件检验单";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 申请人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检验人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交检验结果ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 判定人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交最终判定ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 要求人操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交检验要求ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交验证要求ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交验证结果ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
    }
}