using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlDimissionBill
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.离职申请ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.综合查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.plChoose = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.plChoose.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.离职申请ToolStripMenuItem,
            this.刷新数据ToolStripMenuItem,
            this.综合查询ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1015, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 离职申请ToolStripMenuItem
            // 
            this.离职申请ToolStripMenuItem.Name = "离职申请ToolStripMenuItem";
            this.离职申请ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.离职申请ToolStripMenuItem.Tag = "Add";
            this.离职申请ToolStripMenuItem.Text = "离职申请";
            this.离职申请ToolStripMenuItem.Click += new System.EventHandler(this.离职申请ToolStripMenuItem_Click);
            // 
            // 刷新数据ToolStripMenuItem
            // 
            this.刷新数据ToolStripMenuItem.Name = "刷新数据ToolStripMenuItem";
            this.刷新数据ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.刷新数据ToolStripMenuItem.Tag = "View";
            this.刷新数据ToolStripMenuItem.Text = "刷新数据";
            this.刷新数据ToolStripMenuItem.Click += new System.EventHandler(this.刷新数据ToolStripMenuItem_Click);
            // 
            // 综合查询ToolStripMenuItem
            // 
            this.综合查询ToolStripMenuItem.Name = "综合查询ToolStripMenuItem";
            this.综合查询ToolStripMenuItem.Size = new System.Drawing.Size(67, 20);
            this.综合查询ToolStripMenuItem.Tag = "View";
            this.综合查询ToolStripMenuItem.Text = "综合查询";
            this.综合查询ToolStripMenuItem.Click += new System.EventHandler(this.综合查询ToolStripMenuItem_Click);
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(98, 27);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(21, 14);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "无";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "单据状态：";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 24);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1015, 49);
            this.panel3.TabIndex = 43;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(407, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "员工离职申请单";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 108);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1015, 526);
            this.panel2.TabIndex = 45;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1015, 526);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // plChoose
            // 
            this.plChoose.Controls.Add(this.userControlDataLocalizer1);
            this.plChoose.Dock = System.Windows.Forms.DockStyle.Top;
            this.plChoose.Location = new System.Drawing.Point(0, 73);
            this.plChoose.Name = "plChoose";
            this.plChoose.Size = new System.Drawing.Size(1015, 35);
            this.plChoose.TabIndex = 44;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1015, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // UserControlDimissionBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.plChoose);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "UserControlDimissionBill";
            this.Size = new System.Drawing.Size(1015, 634);
            this.Load += new System.EventHandler(this.UserControlDimissionBill_Load);
            this.Resize += new System.EventHandler(this.UserControlDimissionBill_Resize);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.plChoose.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 刷新数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 综合查询ToolStripMenuItem;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 离职申请ToolStripMenuItem;
        private System.Windows.Forms.Panel plChoose;
        private UserControlDataLocalizer userControlDataLocalizer1;

    }
}
