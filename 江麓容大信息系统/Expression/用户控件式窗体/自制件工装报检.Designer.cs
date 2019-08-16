using UniversalControlLibrary;
namespace Expression
{
    partial class 自制件工装报检
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
            this.机加人员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.修改单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.仓库管理员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.核实物品清单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.物品入库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打印单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查找ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnRefresh = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBillStatus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.menuStrip.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.机加人员操作ToolStripMenuItem,
            this.仓库管理员操作ToolStripMenuItem,
            this.查找ToolStripMenuItem,
            this.btnRefresh});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(943, 24);
            this.menuStrip.TabIndex = 1;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 机加人员操作ToolStripMenuItem
            // 
            this.机加人员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.提交单据ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.修改单据ToolStripMenuItem,
            this.toolStripSeparator1,
            this.删除单据ToolStripMenuItem});
            this.机加人员操作ToolStripMenuItem.Name = "机加人员操作ToolStripMenuItem";
            this.机加人员操作ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.机加人员操作ToolStripMenuItem.Tag = "ADD";
            this.机加人员操作ToolStripMenuItem.Text = "机加人员操作";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.新建单据ToolStripMenuItem.Tag = "ADD";
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 提交单据ToolStripMenuItem
            // 
            this.提交单据ToolStripMenuItem.Name = "提交单据ToolStripMenuItem";
            this.提交单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交单据ToolStripMenuItem.Tag = "Add";
            this.提交单据ToolStripMenuItem.Text = "提交单据";
            this.提交单据ToolStripMenuItem.Click += new System.EventHandler(this.提交单据ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(149, 6);
            this.toolStripMenuItem3.Tag = "add";
            // 
            // 修改单据ToolStripMenuItem
            // 
            this.修改单据ToolStripMenuItem.Name = "修改单据ToolStripMenuItem";
            this.修改单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.修改单据ToolStripMenuItem.Tag = "update";
            this.修改单据ToolStripMenuItem.Text = "修改单据";
            this.修改单据ToolStripMenuItem.Click += new System.EventHandler(this.修改单据ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            this.toolStripSeparator1.Tag = "add";
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除单据ToolStripMenuItem.Tag = "delete";
            this.删除单据ToolStripMenuItem.Text = "删除单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // 仓库管理员操作ToolStripMenuItem
            // 
            this.仓库管理员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.核实物品清单ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.物品入库ToolStripMenuItem,
            this.打印单据ToolStripMenuItem});
            this.仓库管理员操作ToolStripMenuItem.Name = "仓库管理员操作ToolStripMenuItem";
            this.仓库管理员操作ToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.仓库管理员操作ToolStripMenuItem.Tag = "StockIn";
            this.仓库管理员操作ToolStripMenuItem.Text = "仓库管理员操作";
            // 
            // 核实物品清单ToolStripMenuItem
            // 
            this.核实物品清单ToolStripMenuItem.Name = "核实物品清单ToolStripMenuItem";
            this.核实物品清单ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.核实物品清单ToolStripMenuItem.Tag = "StockIn";
            this.核实物品清单ToolStripMenuItem.Text = "核实物品清单";
            this.核实物品清单ToolStripMenuItem.Click += new System.EventHandler(this.核实物品清单ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(149, 6);
            this.toolStripMenuItem2.Tag = "StockIn";
            // 
            // 物品入库ToolStripMenuItem
            // 
            this.物品入库ToolStripMenuItem.Name = "物品入库ToolStripMenuItem";
            this.物品入库ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.物品入库ToolStripMenuItem.Tag = "StockIn";
            this.物品入库ToolStripMenuItem.Text = "零件入库";
            this.物品入库ToolStripMenuItem.Click += new System.EventHandler(this.物品入库ToolStripMenuItem_Click);
            // 
            // 打印单据ToolStripMenuItem
            // 
            this.打印单据ToolStripMenuItem.Name = "打印单据ToolStripMenuItem";
            this.打印单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.打印单据ToolStripMenuItem.Tag = "StockIn";
            this.打印单据ToolStripMenuItem.Text = "打印单据";
            this.打印单据ToolStripMenuItem.Click += new System.EventHandler(this.打印单据ToolStripMenuItem_Click);
            // 
            // 查找ToolStripMenuItem
            // 
            this.查找ToolStripMenuItem.Name = "查找ToolStripMenuItem";
            this.查找ToolStripMenuItem.Size = new System.Drawing.Size(41, 20);
            this.查找ToolStripMenuItem.Tag = "View";
            this.查找ToolStripMenuItem.Text = "查找";
            this.查找ToolStripMenuItem.Click += new System.EventHandler(this.查找ToolStripMenuItem_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(65, 20);
            this.btnRefresh.Tag = "View";
            this.btnRefresh.Text = "刷新数据";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.lblBillStatus);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(943, 46);
            this.panel1.TabIndex = 25;
            // 
            // lblBillStatus
            // 
            this.lblBillStatus.AutoSize = true;
            this.lblBillStatus.Location = new System.Drawing.Point(127, 23);
            this.lblBillStatus.Name = "lblBillStatus";
            this.lblBillStatus.Size = new System.Drawing.Size(0, 14);
            this.lblBillStatus.TabIndex = 189;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(30, 23);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 14);
            this.label11.TabIndex = 188;
            this.label11.Text = "单据状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(385, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "自制件工装报检";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 150);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(943, 553);
            this.dataGridView1.TabIndex = 165;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 70);
            this.checkBillDateAndStatus1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(943, 80);
            this.checkBillDateAndStatus1.TabIndex = 164;
            // 
            // 自制件工装报检
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(943, 703);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("宋体", 10.5F);
            this.Name = "自制件工装报检";
            this.Load += new System.EventHandler(this.自制件工装报检_Load);
            this.Resize += new System.EventHandler(this.自制件工装报检_Resize);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 机加人员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 修改单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 仓库管理员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 核实物品清单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 物品入库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查找ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem btnRefresh;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBillStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelTitle;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 提交单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印单据ToolStripMenuItem;
    }
}
