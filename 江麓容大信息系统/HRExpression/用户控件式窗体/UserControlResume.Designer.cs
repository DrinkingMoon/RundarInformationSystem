using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlResume
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlResume));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.plChoose = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.导出toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.plChoose.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripButton1,
            this.toolStripSeparator4,
            this.刷新toolStripButton1,
            this.toolStripSeparator1,
            this.导出toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1150, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripButton1
            // 
            this.新建toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripButton1.Name = "新建toolStripButton1";
            this.新建toolStripButton1.Size = new System.Drawing.Size(50, 22);
            this.新建toolStripButton1.Tag = "Add";
            this.新建toolStripButton1.Text = "新建(&N)";
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
            this.刷新toolStripButton1.Size = new System.Drawing.Size(59, 22);
            this.刷新toolStripButton1.Tag = "Add";
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1150, 50);
            this.panel3.TabIndex = 27;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(474, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "人员简历信息";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.plChoose);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 75);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1150, 662);
            this.panel2.TabIndex = 243;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 38);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1150, 624);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // plChoose
            // 
            this.plChoose.Controls.Add(this.userControlDataLocalizer1);
            this.plChoose.Dock = System.Windows.Forms.DockStyle.Top;
            this.plChoose.Location = new System.Drawing.Point(0, 0);
            this.plChoose.Name = "plChoose";
            this.plChoose.Size = new System.Drawing.Size(1150, 38);
            this.plChoose.TabIndex = 1;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1150, 38);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出toolStripButton
            // 
            this.导出toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.导出toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("导出toolStripButton.Image")));
            this.导出toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出toolStripButton.Name = "导出toolStripButton";
            this.导出toolStripButton.Size = new System.Drawing.Size(84, 22);
            this.导出toolStripButton.Tag = "Add";
            this.导出toolStripButton.Text = "导出Excel文件";
            this.导出toolStripButton.Click += new System.EventHandler(this.导出toolStripButton_Click);
            // 
            // UserControlResume
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlResume";
            this.Size = new System.Drawing.Size(1150, 737);
            this.Load += new System.EventHandler(this.UserControlResume_Load);
            this.Resize += new System.EventHandler(this.UserControlResume_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.plChoose.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.Panel plChoose;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 导出toolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
