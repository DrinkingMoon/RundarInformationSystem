using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlAttendanceSummary
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlAttendanceSummary));
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.刷新toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.综合查询toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.导出toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.餐补toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.导出加班toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbMonthDept = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.dtpStarDate = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbYearDept = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numYear = new System.Windows.Forms.NumericUpDown();
            this.btnYearOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btn变更允许调休小时数 = new System.Windows.Forms.ToolStripButton();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).BeginInit();
            this.panel2.SuspendLayout();
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
            this.panel3.Size = new System.Drawing.Size(991, 62);
            this.panel3.TabIndex = 56;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(412, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "人员考勤统计";
            // 
            // 刷新toolStripButton
            // 
            this.刷新toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("刷新toolStripButton.Image")));
            this.刷新toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton.Name = "刷新toolStripButton";
            this.刷新toolStripButton.Size = new System.Drawing.Size(76, 22);
            this.刷新toolStripButton.Tag = "View";
            this.刷新toolStripButton.Text = "刷新数据";
            this.刷新toolStripButton.Click += new System.EventHandler(this.刷新toolStripButton_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新toolStripButton,
            this.toolStripSeparator1,
            this.综合查询toolStripButton,
            this.toolStripSeparator2,
            this.导出toolStripButton,
            this.toolStripSeparator3,
            this.餐补toolStripButton1,
            this.toolStripSeparator4,
            this.导出加班toolStripButton,
            this.btn变更允许调休小时数});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(991, 25);
            this.toolStrip1.TabIndex = 55;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 综合查询toolStripButton
            // 
            this.综合查询toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("综合查询toolStripButton.Image")));
            this.综合查询toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.综合查询toolStripButton.Name = "综合查询toolStripButton";
            this.综合查询toolStripButton.Size = new System.Drawing.Size(76, 22);
            this.综合查询toolStripButton.Text = "综合查询";
            this.综合查询toolStripButton.Click += new System.EventHandler(this.综合查询toolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出toolStripButton
            // 
            this.导出toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("导出toolStripButton.Image")));
            this.导出toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出toolStripButton.Name = "导出toolStripButton";
            this.导出toolStripButton.Size = new System.Drawing.Size(100, 22);
            this.导出toolStripButton.Text = "导出汇总统计";
            this.导出toolStripButton.Click += new System.EventHandler(this.导出toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // 餐补toolStripButton1
            // 
            this.餐补toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("餐补toolStripButton1.Image")));
            this.餐补toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.餐补toolStripButton1.Name = "餐补toolStripButton1";
            this.餐补toolStripButton1.Size = new System.Drawing.Size(100, 22);
            this.餐补toolStripButton1.Text = "导出餐补统计";
            this.餐补toolStripButton1.Click += new System.EventHandler(this.餐补toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出加班toolStripButton
            // 
            this.导出加班toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("导出加班toolStripButton.Image")));
            this.导出加班toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出加班toolStripButton.Name = "导出加班toolStripButton";
            this.导出加班toolStripButton.Size = new System.Drawing.Size(124, 22);
            this.导出加班toolStripButton.Text = "导出加班请假统计";
            this.导出加班toolStripButton.Click += new System.EventHandler(this.导出考勤toolStripButton_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 87);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(991, 60);
            this.panel1.TabIndex = 57;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbMonthDept);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnOK);
            this.groupBox2.Controls.Add(this.dtpEndDate);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.dtpStarDate);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(377, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(614, 60);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "月份筛选";
            // 
            // cmbMonthDept
            // 
            this.cmbMonthDept.FormattingEnabled = true;
            this.cmbMonthDept.Location = new System.Drawing.Point(364, 26);
            this.cmbMonthDept.Name = "cmbMonthDept";
            this.cmbMonthDept.Size = new System.Drawing.Size(129, 21);
            this.cmbMonthDept.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(306, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "部 门";
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(512, 24);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "确  定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.CustomFormat = "yyyy-MM";
            this.dtpEndDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndDate.Location = new System.Drawing.Point(203, 25);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(90, 23);
            this.dtpEndDate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "到";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "日  期";
            // 
            // dtpStarDate
            // 
            this.dtpStarDate.CustomFormat = "yyyy-MM";
            this.dtpStarDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStarDate.Location = new System.Drawing.Point(63, 25);
            this.dtpStarDate.Name = "dtpStarDate";
            this.dtpStarDate.Size = new System.Drawing.Size(93, 23);
            this.dtpStarDate.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbYearDept);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numYear);
            this.groupBox1.Controls.Add(this.btnYearOK);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(377, 60);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "年份筛选";
            // 
            // cmbYearDept
            // 
            this.cmbYearDept.FormattingEnabled = true;
            this.cmbYearDept.Location = new System.Drawing.Point(178, 26);
            this.cmbYearDept.Name = "cmbYearDept";
            this.cmbYearDept.Size = new System.Drawing.Size(112, 21);
            this.cmbYearDept.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(130, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(42, 14);
            this.label5.TabIndex = 7;
            this.label5.Text = "部 门";
            // 
            // numYear
            // 
            this.numYear.Location = new System.Drawing.Point(59, 27);
            this.numYear.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numYear.Minimum = new decimal(new int[] {
            2003,
            0,
            0,
            0});
            this.numYear.Name = "numYear";
            this.numYear.Size = new System.Drawing.Size(65, 23);
            this.numYear.TabIndex = 3;
            this.numYear.Value = new decimal(new int[] {
            2012,
            0,
            0,
            0});
            // 
            // btnYearOK
            // 
            this.btnYearOK.Location = new System.Drawing.Point(296, 24);
            this.btnYearOK.Name = "btnYearOK";
            this.btnYearOK.Size = new System.Drawing.Size(75, 23);
            this.btnYearOK.TabIndex = 2;
            this.btnYearOK.Text = "确  定";
            this.btnYearOK.UseVisualStyleBackColor = true;
            this.btnYearOK.Click += new System.EventHandler(this.btnYearOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "年  份";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 147);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(991, 383);
            this.panel2.TabIndex = 58;
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
            this.dataGridView1.Size = new System.Drawing.Size(991, 348);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(991, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.Filter = "XLS 文件|*.xls";
            // 
            // btn变更允许调休小时数
            // 
            this.btn变更允许调休小时数.Image = ((System.Drawing.Image)(resources.GetObject("btn变更允许调休小时数.Image")));
            this.btn变更允许调休小时数.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn变更允许调休小时数.Name = "btn变更允许调休小时数";
            this.btn变更允许调休小时数.Size = new System.Drawing.Size(184, 22);
            this.btn变更允许调休小时数.Text = "批量变更员工允许调休小时数";
            this.btn变更允许调休小时数.Click += new System.EventHandler(this.btn变更允许调休小时数_Click);
            // 
            // UserControlAttendanceSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(991, 530);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlAttendanceSummary";
            this.Load += new System.EventHandler(this.UserControlAttendanceSummary_Load);
            this.Resize += new System.EventHandler(this.UserControlAttendanceMachineHistory_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numYear)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnYearOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpStarDate;
        private System.Windows.Forms.NumericUpDown numYear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 综合查询toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 导出toolStripButton;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 餐补toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 导出加班toolStripButton;
        private System.Windows.Forms.ComboBox cmbMonthDept;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbYearDept;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripButton btn变更允许调休小时数;
    }
}
