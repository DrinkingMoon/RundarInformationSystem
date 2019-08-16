using UniversalControlLibrary;
namespace Expression
{
    partial class 工装验证报告单
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(工装验证报告单));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.工艺员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交检验要求ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交验证要求ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交结论ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.检验员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交检验结果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.验证员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.提交验证结果ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.工艺主管操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.最终审核ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.回退单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.打印报告ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出EXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.menuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.工艺员操作ToolStripMenuItem,
            this.检验员操作ToolStripMenuItem,
            this.验证员操作ToolStripMenuItem,
            this.工艺主管操作ToolStripMenuItem,
            this.回退单据ToolStripMenuItem,
            this.刷新ToolStripMenuItem,
            this.打印报告ToolStripMenuItem,
            this.导出EXCELToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1152, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 工艺员操作ToolStripMenuItem
            // 
            this.工艺员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.提交检验要求ToolStripMenuItem,
            this.提交验证要求ToolStripMenuItem,
            this.提交结论ToolStripMenuItem,
            this.toolStripMenuItem4,
            this.删除单据ToolStripMenuItem});
            this.工艺员操作ToolStripMenuItem.Name = "工艺员操作ToolStripMenuItem";
            this.工艺员操作ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.工艺员操作ToolStripMenuItem.Tag = "ADD";
            this.工艺员操作ToolStripMenuItem.Text = "工艺员操作";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.新建单据ToolStripMenuItem.Tag = "ADD";
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 提交检验要求ToolStripMenuItem
            // 
            this.提交检验要求ToolStripMenuItem.Name = "提交检验要求ToolStripMenuItem";
            this.提交检验要求ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交检验要求ToolStripMenuItem.Tag = "ADD";
            this.提交检验要求ToolStripMenuItem.Text = "提交检验要求";
            this.提交检验要求ToolStripMenuItem.Click += new System.EventHandler(this.提交检验要求ToolStripMenuItem_Click);
            // 
            // 提交验证要求ToolStripMenuItem
            // 
            this.提交验证要求ToolStripMenuItem.Name = "提交验证要求ToolStripMenuItem";
            this.提交验证要求ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交验证要求ToolStripMenuItem.Tag = "ADD";
            this.提交验证要求ToolStripMenuItem.Text = "提交验证要求";
            this.提交验证要求ToolStripMenuItem.Visible = false;
            this.提交验证要求ToolStripMenuItem.Click += new System.EventHandler(this.提交验证要求ToolStripMenuItem_Click);
            // 
            // 提交结论ToolStripMenuItem
            // 
            this.提交结论ToolStripMenuItem.Name = "提交结论ToolStripMenuItem";
            this.提交结论ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交结论ToolStripMenuItem.Tag = "ADD";
            this.提交结论ToolStripMenuItem.Text = "提交结论";
            this.提交结论ToolStripMenuItem.Click += new System.EventHandler(this.提交结论ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(149, 6);
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.删除单据ToolStripMenuItem.Tag = "ADD";
            this.删除单据ToolStripMenuItem.Text = "删除单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // 检验员操作ToolStripMenuItem
            // 
            this.检验员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提交检验结果ToolStripMenuItem});
            this.检验员操作ToolStripMenuItem.Name = "检验员操作ToolStripMenuItem";
            this.检验员操作ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.检验员操作ToolStripMenuItem.Tag = "Check_1";
            this.检验员操作ToolStripMenuItem.Text = "检验员操作";
            // 
            // 提交检验结果ToolStripMenuItem
            // 
            this.提交检验结果ToolStripMenuItem.Name = "提交检验结果ToolStripMenuItem";
            this.提交检验结果ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交检验结果ToolStripMenuItem.Tag = "Check_1";
            this.提交检验结果ToolStripMenuItem.Text = "提交检验结果";
            this.提交检验结果ToolStripMenuItem.Click += new System.EventHandler(this.提交检验结果ToolStripMenuItem_Click);
            // 
            // 验证员操作ToolStripMenuItem
            // 
            this.验证员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.提交验证结果ToolStripMenuItem});
            this.验证员操作ToolStripMenuItem.Name = "验证员操作ToolStripMenuItem";
            this.验证员操作ToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.验证员操作ToolStripMenuItem.Tag = "Check_2";
            this.验证员操作ToolStripMenuItem.Text = "验证员操作";
            this.验证员操作ToolStripMenuItem.Visible = false;
            // 
            // 提交验证结果ToolStripMenuItem
            // 
            this.提交验证结果ToolStripMenuItem.Name = "提交验证结果ToolStripMenuItem";
            this.提交验证结果ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.提交验证结果ToolStripMenuItem.Tag = "Check_2";
            this.提交验证结果ToolStripMenuItem.Text = "提交验证结果";
            this.提交验证结果ToolStripMenuItem.Visible = false;
            this.提交验证结果ToolStripMenuItem.Click += new System.EventHandler(this.提交验证结果ToolStripMenuItem_Click);
            // 
            // 工艺主管操作ToolStripMenuItem
            // 
            this.工艺主管操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.最终审核ToolStripMenuItem});
            this.工艺主管操作ToolStripMenuItem.Name = "工艺主管操作ToolStripMenuItem";
            this.工艺主管操作ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.工艺主管操作ToolStripMenuItem.Tag = "Check_3";
            this.工艺主管操作ToolStripMenuItem.Text = "工艺主管操作";
            // 
            // 最终审核ToolStripMenuItem
            // 
            this.最终审核ToolStripMenuItem.Name = "最终审核ToolStripMenuItem";
            this.最终审核ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.最终审核ToolStripMenuItem.Tag = "Check_3";
            this.最终审核ToolStripMenuItem.Text = "最终审核";
            this.最终审核ToolStripMenuItem.Click += new System.EventHandler(this.最终审核ToolStripMenuItem_Click);
            // 
            // 回退单据ToolStripMenuItem
            // 
            this.回退单据ToolStripMenuItem.Name = "回退单据ToolStripMenuItem";
            this.回退单据ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.回退单据ToolStripMenuItem.Tag = "view";
            this.回退单据ToolStripMenuItem.Text = "回退单据";
            this.回退单据ToolStripMenuItem.Click += new System.EventHandler(this.回退单据ToolStripMenuItem_Click);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.刷新ToolStripMenuItem.Tag = "view";
            this.刷新ToolStripMenuItem.Text = "刷新";
            this.刷新ToolStripMenuItem.Click += new System.EventHandler(this.刷新ToolStripMenuItem_Click);
            // 
            // 打印报告ToolStripMenuItem
            // 
            this.打印报告ToolStripMenuItem.Name = "打印报告ToolStripMenuItem";
            this.打印报告ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.打印报告ToolStripMenuItem.Tag = "view";
            this.打印报告ToolStripMenuItem.Text = "打印报告";
            this.打印报告ToolStripMenuItem.Click += new System.EventHandler(this.打印报告ToolStripMenuItem_Click);
            // 
            // 导出EXCELToolStripMenuItem
            // 
            this.导出EXCELToolStripMenuItem.Name = "导出EXCELToolStripMenuItem";
            this.导出EXCELToolStripMenuItem.Size = new System.Drawing.Size(80, 21);
            this.导出EXCELToolStripMenuItem.Tag = "view";
            this.导出EXCELToolStripMenuItem.Text = "导出EXCEL";
            this.导出EXCELToolStripMenuItem.Click += new System.EventHandler(this.导出EXCELToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1152, 49);
            this.panel3.TabIndex = 45;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(463, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "工装验证报告单";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 159);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1152, 578);
            this.dataGridView1.TabIndex = 49;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 124);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1152, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 48;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.ListBillStatus = ((System.Collections.Generic.List<string>)(resources.GetObject("checkBillDateAndStatus1.ListBillStatus")));
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 74);
            this.checkBillDateAndStatus1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBillDateAndStatus1.MultiVisible = false;
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(1152, 50);
            this.checkBillDateAndStatus1.StatusVisible = true;
            this.checkBillDateAndStatus1.TabIndex = 46;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
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
            // 工装验证报告单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 737);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "工装验证报告单";
            this.Load += new System.EventHandler(this.工装验证报告单_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 工艺员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交检验要求ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交验证要求ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交结论ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 检验员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交检验结果ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 验证员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 提交验证结果ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripMenuItem 回退单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 打印报告ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem 导出EXCELToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 工艺主管操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 最终审核ToolStripMenuItem;
    }
}
