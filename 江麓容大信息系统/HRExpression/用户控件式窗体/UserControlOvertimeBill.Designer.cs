using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlOvertimeBill
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlOvertimeBill));
            this.panel3 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.综合查询toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.人力批量处理toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.label2);
            this.panel3.Controls.Add(this.lblStatus);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1003, 54);
            this.panel3.TabIndex = 167;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Right;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(646, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(357, 28);
            this.label2.TabIndex = 3;
            this.label2.Text = "在公司内加班的人员必须在加班开始和结束后到门卫打卡\r\n(平常加班打上班卡和加班结束卡)否则不计加班。";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(80, 29);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(21, 14);
            this.lblStatus.TabIndex = 2;
            this.lblStatus.Text = "无";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "单据状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(399, 13);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(147, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "加班申请单";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripDropDownButton1,
            this.toolStripSeparator1,
            this.刷新toolStripButton1,
            this.toolStripSeparator4,
            this.综合查询toolStripButton3,
            this.toolStripSeparator2,
            this.人力批量处理toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1003, 25);
            this.toolStrip1.TabIndex = 166;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripDropDownButton1
            // 
            this.新建toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.新建toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.toolStripSeparator8,
            this.删除单据ToolStripMenuItem});
            this.新建toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("新建toolStripDropDownButton1.Image")));
            this.新建toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripDropDownButton1.Name = "新建toolStripDropDownButton1";
            this.新建toolStripDropDownButton1.Size = new System.Drawing.Size(69, 22);
            this.新建toolStripDropDownButton1.Tag = "Add";
            this.新建toolStripDropDownButton1.Text = "申请单据";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.新建单据ToolStripMenuItem.Tag = "Add";
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(121, 6);
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除单据ToolStripMenuItem.Tag = "delete";
            this.删除单据ToolStripMenuItem.Text = "删除单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "Add";
            // 
            // 刷新toolStripButton1
            // 
            this.刷新toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton1.Name = "刷新toolStripButton1";
            this.刷新toolStripButton1.Size = new System.Drawing.Size(60, 22);
            this.刷新toolStripButton1.Tag = "View";
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 综合查询toolStripButton3
            // 
            this.综合查询toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.综合查询toolStripButton3.Name = "综合查询toolStripButton3";
            this.综合查询toolStripButton3.Size = new System.Drawing.Size(60, 22);
            this.综合查询toolStripButton3.Tag = "View";
            this.综合查询toolStripButton3.Text = "综合查询";
            this.综合查询toolStripButton3.Click += new System.EventHandler(this.综合查询toolStripButton3_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 人力批量处理toolStripButton3
            // 
            this.人力批量处理toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.人力批量处理toolStripButton3.Name = "人力批量处理toolStripButton3";
            this.人力批量处理toolStripButton3.Size = new System.Drawing.Size(84, 22);
            this.人力批量处理toolStripButton3.Tag = "Retrial_2";
            this.人力批量处理toolStripButton3.Text = "人力批量处理";
            this.人力批量处理toolStripButton3.Click += new System.EventHandler(this.人力批量处理toolStripButton3_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Controls.Add(this.userControlDataLocalizer1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 124);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1003, 444);
            this.panel1.TabIndex = 170;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 35);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1003, 409);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
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
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1003, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 2;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.ListBillStatus = ((System.Collections.Generic.List<string>)(resources.GetObject("checkBillDateAndStatus1.ListBillStatus")));
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 79);
            this.checkBillDateAndStatus1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBillDateAndStatus1.MultiVisible = true;
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(1003, 45);
            this.checkBillDateAndStatus1.StatusVisible = true;
            this.checkBillDateAndStatus1.TabIndex = 168;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
            // 
            // UserControlOvertimeBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1003, 568);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlOvertimeBill";
            this.Load += new System.EventHandler(this.UserControlOvertimeBill_Load);
            this.Resize += new System.EventHandler(this.UserControlOvertimeBill_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UniversalControlLibrary.CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 综合查询toolStripButton3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton 新建toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 人力批量处理toolStripButton3;

    }
}
