using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlPersonnelArchive
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlPersonnelArchive));
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.导入toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.导出toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.综合查询toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton批量修改部门 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.月报表toolStripButton = new System.Windows.Forms.ToolStripDropDownButton();
            this.导出平均年龄ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出平均学历ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出平均入司年份ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出各部门在职人数ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出当月离职人员分析ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出人员变化情况ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.plChoose = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.cbStatusOut = new System.Windows.Forms.CheckBox();
            this.cbStatusIn = new System.Windows.Forms.CheckBox();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.plChoose.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1145, 50);
            this.panel3.TabIndex = 30;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(485, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "人员档案管理";
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
            this.导入toolStripButton,
            this.toolStripSeparator2,
            this.导出toolStripButton,
            this.toolStripSeparator3,
            this.综合查询toolStripButton3,
            this.toolStripSeparator5,
            this.toolStripButton批量修改部门,
            this.toolStripSeparator6,
            this.月报表toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1145, 25);
            this.toolStrip1.TabIndex = 29;
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
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 导入toolStripButton
            // 
            this.导入toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导入toolStripButton.Name = "导入toolStripButton";
            this.导入toolStripButton.Size = new System.Drawing.Size(87, 22);
            this.导入toolStripButton.Tag = "导出文件";
            this.导入toolStripButton.Text = "导入Excel文件";
            this.导入toolStripButton.Click += new System.EventHandler(this.导入toolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton批量修改部门
            // 
            this.toolStripButton批量修改部门.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton批量修改部门.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton批量修改部门.Image")));
            this.toolStripButton批量修改部门.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton批量修改部门.Name = "toolStripButton批量修改部门";
            this.toolStripButton批量修改部门.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton批量修改部门.Tag = "Add";
            this.toolStripButton批量修改部门.Text = "批量修改部门";
            this.toolStripButton批量修改部门.Click += new System.EventHandler(this.toolStripButton批量修改部门_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // 月报表toolStripButton
            // 
            this.月报表toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.月报表toolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出平均年龄ToolStripMenuItem,
            this.导出平均学历ToolStripMenuItem,
            this.导出平均入司年份ToolStripMenuItem,
            this.导出各部门在职人数ToolStripMenuItem,
            this.导出当月离职人员分析ToolStripMenuItem,
            this.导出人员变化情况ToolStripMenuItem});
            this.月报表toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("月报表toolStripButton.Image")));
            this.月报表toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.月报表toolStripButton.Name = "月报表toolStripButton";
            this.月报表toolStripButton.Size = new System.Drawing.Size(78, 22);
            this.月报表toolStripButton.Tag = "导出文件";
            this.月报表toolStripButton.Text = "人事月报表";
            // 
            // 导出平均年龄ToolStripMenuItem
            // 
            this.导出平均年龄ToolStripMenuItem.Name = "导出平均年龄ToolStripMenuItem";
            this.导出平均年龄ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.导出平均年龄ToolStripMenuItem.Text = "导出平均年龄";
            this.导出平均年龄ToolStripMenuItem.Click += new System.EventHandler(this.导出平均年龄ToolStripMenuItem_Click);
            // 
            // 导出平均学历ToolStripMenuItem
            // 
            this.导出平均学历ToolStripMenuItem.Name = "导出平均学历ToolStripMenuItem";
            this.导出平均学历ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.导出平均学历ToolStripMenuItem.Text = "导出学历人数";
            this.导出平均学历ToolStripMenuItem.Click += new System.EventHandler(this.导出平均学历ToolStripMenuItem_Click);
            // 
            // 导出平均入司年份ToolStripMenuItem
            // 
            this.导出平均入司年份ToolStripMenuItem.Name = "导出平均入司年份ToolStripMenuItem";
            this.导出平均入司年份ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.导出平均入司年份ToolStripMenuItem.Text = "导出平均司龄";
            this.导出平均入司年份ToolStripMenuItem.Click += new System.EventHandler(this.导出平均入司年份ToolStripMenuItem_Click);
            // 
            // 导出各部门在职人数ToolStripMenuItem
            // 
            this.导出各部门在职人数ToolStripMenuItem.Name = "导出各部门在职人数ToolStripMenuItem";
            this.导出各部门在职人数ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.导出各部门在职人数ToolStripMenuItem.Text = "导出各部门在职人数";
            this.导出各部门在职人数ToolStripMenuItem.Click += new System.EventHandler(this.导出各部门在职人数ToolStripMenuItem_Click);
            // 
            // 导出当月离职人员分析ToolStripMenuItem
            // 
            this.导出当月离职人员分析ToolStripMenuItem.Name = "导出当月离职人员分析ToolStripMenuItem";
            this.导出当月离职人员分析ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.导出当月离职人员分析ToolStripMenuItem.Text = "导出当月离职人员分析";
            this.导出当月离职人员分析ToolStripMenuItem.Click += new System.EventHandler(this.导出当月离职人员分析ToolStripMenuItem_Click);
            // 
            // 导出人员变化情况ToolStripMenuItem
            // 
            this.导出人员变化情况ToolStripMenuItem.Name = "导出人员变化情况ToolStripMenuItem";
            this.导出人员变化情况ToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.导出人员变化情况ToolStripMenuItem.Text = "导出各部门人员变化情况";
            this.导出人员变化情况ToolStripMenuItem.Click += new System.EventHandler(this.导出人员变化情况ToolStripMenuItem_Click);
            // 
            // plChoose
            // 
            this.plChoose.Controls.Add(this.btnOK);
            this.plChoose.Controls.Add(this.cbStatusOut);
            this.plChoose.Controls.Add(this.cbStatusIn);
            this.plChoose.Controls.Add(this.userControlDataLocalizer1);
            this.plChoose.Dock = System.Windows.Forms.DockStyle.Top;
            this.plChoose.Location = new System.Drawing.Point(0, 75);
            this.plChoose.Name = "plChoose";
            this.plChoose.Size = new System.Drawing.Size(1145, 35);
            this.plChoose.TabIndex = 35;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(957, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确 定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cbStatusOut
            // 
            this.cbStatusOut.AutoSize = true;
            this.cbStatusOut.Location = new System.Drawing.Point(867, 11);
            this.cbStatusOut.Name = "cbStatusOut";
            this.cbStatusOut.Size = new System.Drawing.Size(61, 18);
            this.cbStatusOut.TabIndex = 3;
            this.cbStatusOut.Text = "离 职";
            this.cbStatusOut.UseVisualStyleBackColor = true;
            // 
            // cbStatusIn
            // 
            this.cbStatusIn.AutoSize = true;
            this.cbStatusIn.Location = new System.Drawing.Point(757, 11);
            this.cbStatusIn.Name = "cbStatusIn";
            this.cbStatusIn.Size = new System.Drawing.Size(61, 18);
            this.cbStatusIn.TabIndex = 2;
            this.cbStatusIn.Text = "在 职";
            this.cbStatusIn.UseVisualStyleBackColor = true;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Left;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(735, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 110);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1145, 507);
            this.panel4.TabIndex = 36;
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
            this.dataGridView1.Size = new System.Drawing.Size(1145, 507);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
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
            // UserControlPersonnelArchive
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1145, 617);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.plChoose);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlPersonnelArchive";
            this.Load += new System.EventHandler(this.UserControlPersonnelArchive_Load);
            this.Resize += new System.EventHandler(this.UserControlPersonnelArchive_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.plChoose.ResumeLayout(false);
            this.plChoose.PerformLayout();
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.Panel plChoose;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 导入toolStripButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 导出toolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton 综合查询toolStripButton3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.CheckBox cbStatusOut;
        private System.Windows.Forms.CheckBox cbStatusIn;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton toolStripButton批量修改部门;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripDropDownButton 月报表toolStripButton;
        private System.Windows.Forms.ToolStripMenuItem 导出平均年龄ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出平均入司年份ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出平均学历ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出各部门在职人数ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出当月离职人员分析ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 导出人员变化情况ToolStripMenuItem;
    }
}
