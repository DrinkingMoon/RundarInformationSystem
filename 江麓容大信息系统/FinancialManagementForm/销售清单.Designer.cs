using UniversalControlLibrary;
namespace Form_Economic_Financial
{
    partial class 销售清单
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.刷新toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.打印toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.选择打印toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.日志toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton导出 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.选 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel3 = new System.Windows.Forms.Panel();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新toolStripButton,
            this.toolStripSeparator1,
            this.打印toolStripButton,
            this.toolStripSeparator2,
            this.选择打印toolStripButton,
            this.toolStripSeparator3,
            this.日志toolStripButton,
            this.toolStripSeparator4,
            this.toolStripButton导出});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1025, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 刷新toolStripButton
            // 
            this.刷新toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.刷新toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton.Name = "刷新toolStripButton";
            this.刷新toolStripButton.Size = new System.Drawing.Size(73, 22);
            this.刷新toolStripButton.Text = "刷新数据";
            this.刷新toolStripButton.Click += new System.EventHandler(this.刷新toolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 打印toolStripButton
            // 
            this.打印toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.打印toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.打印toolStripButton.Name = "打印toolStripButton";
            this.打印toolStripButton.Size = new System.Drawing.Size(97, 22);
            this.打印toolStripButton.Tag = "View";
            this.打印toolStripButton.Text = "单个单据打印";
            this.打印toolStripButton.Click += new System.EventHandler(this.打印toolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 选择打印toolStripButton
            // 
            this.选择打印toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.选择打印toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.选择打印toolStripButton.Name = "选择打印toolStripButton";
            this.选择打印toolStripButton.Size = new System.Drawing.Size(85, 22);
            this.选择打印toolStripButton.Text = "选择性打印";
            this.选择打印toolStripButton.Click += new System.EventHandler(this.选择打印toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // 日志toolStripButton
            // 
            this.日志toolStripButton.Image = global::UniversalControlLibrary.Properties.Resources.File2;
            this.日志toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.日志toolStripButton.Name = "日志toolStripButton";
            this.日志toolStripButton.Size = new System.Drawing.Size(97, 22);
            this.日志toolStripButton.Text = "查看操作日志";
            this.日志toolStripButton.Click += new System.EventHandler(this.日志toolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton导出
            // 
            this.toolStripButton导出.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.toolStripButton导出.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton导出.Name = "toolStripButton导出";
            this.toolStripButton导出.Size = new System.Drawing.Size(79, 22);
            this.toolStripButton导出.Text = "导出Excel";
            this.toolStripButton导出.Click += new System.EventHandler(this.toolStripButton导出_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1025, 57);
            this.panel1.TabIndex = 53;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(452, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "销售清单";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 82);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1025, 485);
            this.panel2.TabIndex = 171;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 90);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 30;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.Size = new System.Drawing.Size(1025, 395);
            this.dataGridView1.TabIndex = 173;
            this.dataGridView1.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseClick);
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // 选
            // 
            this.选.FillWeight = 25F;
            this.选.HeaderText = "选";
            this.选.Name = "选";
            this.选.ReadOnly = true;
            this.选.Width = 25;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择ToolStripMenuItem,
            this.全消ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // 选择ToolStripMenuItem
            // 
            this.选择ToolStripMenuItem.Name = "选择ToolStripMenuItem";
            this.选择ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.选择ToolStripMenuItem.Text = "选择";
            this.选择ToolStripMenuItem.Click += new System.EventHandler(this.选择ToolStripMenuItem_Click);
            // 
            // 全消ToolStripMenuItem
            // 
            this.全消ToolStripMenuItem.Name = "全消ToolStripMenuItem";
            this.全消ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.全消ToolStripMenuItem.Text = "全消";
            this.全消ToolStripMenuItem.Click += new System.EventHandler(this.全消ToolStripMenuItem_Click);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 55);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1025, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 172;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.checkBillDateAndStatus1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1025, 55);
            this.panel3.TabIndex = 0;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 0);
            this.checkBillDateAndStatus1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(1025, 50);
            this.checkBillDateAndStatus1.TabIndex = 185;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.ShowHelp = true;
            this.saveFileDialog1.Title = "将查询结果保存成 EXCEL 文件";
            // 
            // 销售清单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1025, 567);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "销售清单";
            this.Text = "销售清单";
            this.Load += new System.EventHandler(this.销售清单_Load);
            this.Resize += new System.EventHandler(this.销售清单_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panel2;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.ToolStripButton 打印toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 选择打印toolStripButton;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全消ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 日志toolStripButton;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton toolStripButton导出;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}