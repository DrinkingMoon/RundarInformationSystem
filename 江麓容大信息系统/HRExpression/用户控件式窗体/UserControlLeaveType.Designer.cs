using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlLeaveType
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLeaveType));
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.提交toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.修改toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.删除toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.cbNeedAnnex = new System.Windows.Forms.CheckBox();
            this.cmbParentTypeCode = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.numMinHours = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.cbIncludeHoliday = new System.Windows.Forms.CheckBox();
            this.cbPaidLeave = new System.Windows.Forms.CheckBox();
            this.cmbLeaveMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.numMaxTimes = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numMaxHours = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTypeName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTypeCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.chbIsDelete = new System.Windows.Forms.CheckBox();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTimes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxHours)).BeginInit();
            this.panel2.SuspendLayout();
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
            this.panel3.Size = new System.Drawing.Size(1028, 49);
            this.panel3.TabIndex = 39;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(408, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "请假类别管理";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripButton1,
            this.提交toolStripButton1,
            this.修改toolStripButton1,
            this.删除toolStripButton6,
            this.刷新toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1028, 25);
            this.toolStrip1.TabIndex = 38;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripButton1
            // 
            this.新建toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("新建toolStripButton1.Image")));
            this.新建toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripButton1.Name = "新建toolStripButton1";
            this.新建toolStripButton1.Size = new System.Drawing.Size(55, 22);
            this.新建toolStripButton1.Tag = "Add";
            this.新建toolStripButton1.Text = "新 建";
            this.新建toolStripButton1.Click += new System.EventHandler(this.新建toolStripButton1_Click);
            // 
            // 提交toolStripButton1
            // 
            this.提交toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("提交toolStripButton1.Image")));
            this.提交toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.提交toolStripButton1.Name = "提交toolStripButton1";
            this.提交toolStripButton1.Size = new System.Drawing.Size(55, 22);
            this.提交toolStripButton1.Tag = "Add";
            this.提交toolStripButton1.Text = "添 加";
            this.提交toolStripButton1.Click += new System.EventHandler(this.提交toolStripButton1_Click);
            // 
            // 修改toolStripButton1
            // 
            this.修改toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("修改toolStripButton1.Image")));
            this.修改toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改toolStripButton1.Name = "修改toolStripButton1";
            this.修改toolStripButton1.Size = new System.Drawing.Size(55, 22);
            this.修改toolStripButton1.Tag = "update";
            this.修改toolStripButton1.Text = "修 改";
            this.修改toolStripButton1.Click += new System.EventHandler(this.修改toolStripButton1_Click);
            // 
            // 删除toolStripButton6
            // 
            this.删除toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("删除toolStripButton6.Image")));
            this.删除toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除toolStripButton6.Name = "删除toolStripButton6";
            this.删除toolStripButton6.Size = new System.Drawing.Size(55, 22);
            this.删除toolStripButton6.Tag = "delete";
            this.删除toolStripButton6.Text = "删 除";
            this.删除toolStripButton6.Click += new System.EventHandler(this.删除toolStripButton6_Click);
            // 
            // 刷新toolStripButton1
            // 
            this.刷新toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("刷新toolStripButton1.Image")));
            this.刷新toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton1.Name = "刷新toolStripButton1";
            this.刷新toolStripButton1.Size = new System.Drawing.Size(73, 22);
            this.刷新toolStripButton1.Tag = "View";
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.chbIsDelete);
            this.panel1.Controls.Add(this.cbNeedAnnex);
            this.panel1.Controls.Add(this.cmbParentTypeCode);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.numMinHours);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.cbIncludeHoliday);
            this.panel1.Controls.Add(this.cbPaidLeave);
            this.panel1.Controls.Add(this.cmbLeaveMode);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.numMaxTimes);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.numMaxHours);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtTypeName);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtTypeCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 74);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1028, 147);
            this.panel1.TabIndex = 40;
            // 
            // cbNeedAnnex
            // 
            this.cbNeedAnnex.AutoSize = true;
            this.cbNeedAnnex.ForeColor = System.Drawing.Color.Blue;
            this.cbNeedAnnex.Location = new System.Drawing.Point(742, 123);
            this.cbNeedAnnex.Name = "cbNeedAnnex";
            this.cbNeedAnnex.Size = new System.Drawing.Size(96, 18);
            this.cbNeedAnnex.TabIndex = 21;
            this.cbNeedAnnex.Text = "需附件证明";
            this.cbNeedAnnex.UseVisualStyleBackColor = true;
            // 
            // cmbParentTypeCode
            // 
            this.cmbParentTypeCode.FormattingEnabled = true;
            this.cmbParentTypeCode.Location = new System.Drawing.Point(102, 51);
            this.cmbParentTypeCode.Name = "cmbParentTypeCode";
            this.cmbParentTypeCode.Size = new System.Drawing.Size(121, 21);
            this.cmbParentTypeCode.TabIndex = 20;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(33, 54);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 14);
            this.label11.TabIndex = 19;
            this.label11.Text = "父级类别";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(102, 81);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(620, 54);
            this.txtRemark.TabIndex = 18;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(47, 90);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 14);
            this.label10.TabIndex = 17;
            this.label10.Text = "备  注";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F);
            this.label8.Location = new System.Drawing.Point(954, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 16;
            this.label8.Text = "(小时)";
            // 
            // numMinHours
            // 
            this.numMinHours.DecimalPlaces = 1;
            this.numMinHours.Location = new System.Drawing.Point(864, 12);
            this.numMinHours.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMinHours.Name = "numMinHours";
            this.numMinHours.Size = new System.Drawing.Size(89, 23);
            this.numMinHours.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(739, 16);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(119, 14);
            this.label9.TabIndex = 14;
            this.label9.Text = "允许的最小小时数";
            // 
            // cbIncludeHoliday
            // 
            this.cbIncludeHoliday.AutoSize = true;
            this.cbIncludeHoliday.ForeColor = System.Drawing.Color.Blue;
            this.cbIncludeHoliday.Location = new System.Drawing.Point(742, 52);
            this.cbIncludeHoliday.Name = "cbIncludeHoliday";
            this.cbIncludeHoliday.Size = new System.Drawing.Size(124, 18);
            this.cbIncludeHoliday.TabIndex = 13;
            this.cbIncludeHoliday.Text = "是否包含休息日";
            this.cbIncludeHoliday.UseVisualStyleBackColor = true;
            // 
            // cbPaidLeave
            // 
            this.cbPaidLeave.AutoSize = true;
            this.cbPaidLeave.ForeColor = System.Drawing.Color.Blue;
            this.cbPaidLeave.Location = new System.Drawing.Point(742, 90);
            this.cbPaidLeave.Name = "cbPaidLeave";
            this.cbPaidLeave.Size = new System.Drawing.Size(138, 18);
            this.cbPaidLeave.TabIndex = 12;
            this.cbPaidLeave.Text = "是否带薪假的标志";
            this.cbPaidLeave.UseVisualStyleBackColor = true;
            // 
            // cmbLeaveMode
            // 
            this.cmbLeaveMode.FormattingEnabled = true;
            this.cmbLeaveMode.Items.AddRange(new object[] {
            "任意",
            "月度假",
            "年度假"});
            this.cmbLeaveMode.Location = new System.Drawing.Point(333, 51);
            this.cmbLeaveMode.Name = "cmbLeaveMode";
            this.cmbLeaveMode.Size = new System.Drawing.Size(117, 21);
            this.cmbLeaveMode.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Blue;
            this.label7.Location = new System.Drawing.Point(264, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 10;
            this.label7.Text = "请假模式";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F);
            this.label6.Location = new System.Drawing.Point(628, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "(次)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F);
            this.label5.Location = new System.Drawing.Point(681, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "(小时)";
            // 
            // numMaxTimes
            // 
            this.numMaxTimes.Location = new System.Drawing.Point(536, 50);
            this.numMaxTimes.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numMaxTimes.Name = "numMaxTimes";
            this.numMaxTimes.Size = new System.Drawing.Size(89, 23);
            this.numMaxTimes.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(467, 54);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "最多次数";
            // 
            // numMaxHours
            // 
            this.numMaxHours.DecimalPlaces = 1;
            this.numMaxHours.Location = new System.Drawing.Point(591, 12);
            this.numMaxHours.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numMaxHours.Name = "numMaxHours";
            this.numMaxHours.Size = new System.Drawing.Size(89, 23);
            this.numMaxHours.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(467, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "允许的最多小时数";
            // 
            // txtTypeName
            // 
            this.txtTypeName.Location = new System.Drawing.Point(333, 12);
            this.txtTypeName.Name = "txtTypeName";
            this.txtTypeName.Size = new System.Drawing.Size(117, 23);
            this.txtTypeName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(236, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "请假类别名称";
            // 
            // txtTypeCode
            // 
            this.txtTypeCode.Location = new System.Drawing.Point(103, 12);
            this.txtTypeCode.Name = "txtTypeCode";
            this.txtTypeCode.Size = new System.Drawing.Size(120, 23);
            this.txtTypeCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(5, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "请假类别编号";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 221);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1028, 35);
            this.panel2.TabIndex = 43;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1028, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 256);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1028, 271);
            this.panel4.TabIndex = 44;
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
            this.dataGridView1.Size = new System.Drawing.Size(1028, 271);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // chbIsDelete
            // 
            this.chbIsDelete.AutoSize = true;
            this.chbIsDelete.ForeColor = System.Drawing.Color.Red;
            this.chbIsDelete.Location = new System.Drawing.Point(922, 50);
            this.chbIsDelete.Name = "chbIsDelete";
            this.chbIsDelete.Size = new System.Drawing.Size(54, 18);
            this.chbIsDelete.TabIndex = 22;
            this.chbIsDelete.Text = "禁用";
            this.chbIsDelete.UseVisualStyleBackColor = true;
            // 
            // UserControlLeaveType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1028, 527);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "UserControlLeaveType";
            this.Load += new System.EventHandler(this.UserControlLeaveType_Load);
            this.Resize += new System.EventHandler(this.UserControlLeaveType_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxTimes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxHours)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripButton 提交toolStripButton1;
        private System.Windows.Forms.ToolStripButton 修改toolStripButton1;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton6;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtTypeCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMaxHours;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTypeName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numMaxTimes;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numMinHours;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox cbIncludeHoliday;
        private System.Windows.Forms.CheckBox cbPaidLeave;
        private System.Windows.Forms.ComboBox cmbLeaveMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbParentTypeCode;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox cbNeedAnnex;
        private System.Windows.Forms.CheckBox chbIsDelete;
    }
}
