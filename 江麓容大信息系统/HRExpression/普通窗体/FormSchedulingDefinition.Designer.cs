using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class FormSchedulingDefinition
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSchedulingDefinition));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.添加toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.修改toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.删除toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.numDays = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.dtpPunchOutEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpPunchInBeginTime = new System.Windows.Forms.DateTimePicker();
            this.dtpPunchOutBeginTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpPunchInEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpBeginTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cbIsHoliday = new System.Windows.Forms.CheckBox();
            this.txtName = new System.Windows.Forms.TextBox();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripButton1,
            this.toolStripSeparator1,
            this.添加toolStripButton1,
            this.toolStripSeparator2,
            this.修改toolStripButton2,
            this.toolStripSeparator3,
            this.删除toolStripButton1,
            this.toolStripSeparator4,
            this.刷新toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(829, 25);
            this.toolStrip1.TabIndex = 50;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripButton1
            // 
            this.新建toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.新建toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("新建toolStripButton1.Image")));
            this.新建toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripButton1.Name = "新建toolStripButton1";
            this.新建toolStripButton1.Size = new System.Drawing.Size(38, 22);
            this.新建toolStripButton1.Text = "新 建";
            this.新建toolStripButton1.Click += new System.EventHandler(this.新建toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "View";
            // 
            // 添加toolStripButton1
            // 
            this.添加toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加toolStripButton1.Name = "添加toolStripButton1";
            this.添加toolStripButton1.Size = new System.Drawing.Size(38, 22);
            this.添加toolStripButton1.Tag = "Add";
            this.添加toolStripButton1.Text = "添 加";
            this.添加toolStripButton1.Click += new System.EventHandler(this.添加toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "Add";
            // 
            // 修改toolStripButton2
            // 
            this.修改toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改toolStripButton2.Name = "修改toolStripButton2";
            this.修改toolStripButton2.Size = new System.Drawing.Size(38, 22);
            this.修改toolStripButton2.Tag = "update";
            this.修改toolStripButton2.Text = "修 改";
            this.修改toolStripButton2.Click += new System.EventHandler(this.修改toolStripButton2_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "Add";
            // 
            // 删除toolStripButton1
            // 
            this.删除toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除toolStripButton1.Name = "删除toolStripButton1";
            this.删除toolStripButton1.Size = new System.Drawing.Size(38, 22);
            this.删除toolStripButton1.Tag = "delete";
            this.删除toolStripButton1.Text = "删 除";
            this.删除toolStripButton1.Click += new System.EventHandler(this.删除toolStripButton1_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Tag = "View";
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
            // panel1
            // 
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.numDays);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.dtpPunchOutEndTime);
            this.panel1.Controls.Add(this.dtpPunchInBeginTime);
            this.panel1.Controls.Add(this.dtpPunchOutBeginTime);
            this.panel1.Controls.Add(this.dtpEndTime);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.dtpPunchInEndTime);
            this.panel1.Controls.Add(this.dtpBeginTime);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cbIsHoliday);
            this.panel1.Controls.Add(this.txtName);
            this.panel1.Controls.Add(this.txtCode);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(829, 137);
            this.panel1.TabIndex = 51;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(789, 104);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(35, 14);
            this.label11.TabIndex = 21;
            this.label11.Text = "(天)";
            // 
            // numDays
            // 
            this.numDays.Location = new System.Drawing.Point(674, 100);
            this.numDays.Name = "numDays";
            this.numDays.Size = new System.Drawing.Size(109, 23);
            this.numDays.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(562, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 19;
            this.label10.Text = "偏移天数";
            // 
            // dtpPunchOutEndTime
            // 
            this.dtpPunchOutEndTime.CustomFormat = "HH:mm";
            this.dtpPunchOutEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPunchOutEndTime.Location = new System.Drawing.Point(674, 68);
            this.dtpPunchOutEndTime.Name = "dtpPunchOutEndTime";
            this.dtpPunchOutEndTime.Size = new System.Drawing.Size(146, 23);
            this.dtpPunchOutEndTime.TabIndex = 18;
            // 
            // dtpPunchInBeginTime
            // 
            this.dtpPunchInBeginTime.CustomFormat = "HH:mm";
            this.dtpPunchInBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPunchInBeginTime.Location = new System.Drawing.Point(674, 34);
            this.dtpPunchInBeginTime.Name = "dtpPunchInBeginTime";
            this.dtpPunchInBeginTime.Size = new System.Drawing.Size(146, 23);
            this.dtpPunchInBeginTime.TabIndex = 17;
            // 
            // dtpPunchOutBeginTime
            // 
            this.dtpPunchOutBeginTime.CustomFormat = "HH:mm";
            this.dtpPunchOutBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPunchOutBeginTime.Location = new System.Drawing.Point(398, 68);
            this.dtpPunchOutBeginTime.Name = "dtpPunchOutBeginTime";
            this.dtpPunchOutBeginTime.Size = new System.Drawing.Size(146, 23);
            this.dtpPunchOutBeginTime.TabIndex = 16;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "HH:mm";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(398, 36);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(146, 23);
            this.dtpEndTime.TabIndex = 15;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(128, 100);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(416, 23);
            this.txtRemark.TabIndex = 14;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Blue;
            this.label9.Location = new System.Drawing.Point(61, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 13;
            this.label9.Text = "备    注";
            // 
            // dtpPunchInEndTime
            // 
            this.dtpPunchInEndTime.CustomFormat = "HH:mm";
            this.dtpPunchInEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpPunchInEndTime.Location = new System.Drawing.Point(128, 68);
            this.dtpPunchInEndTime.Name = "dtpPunchInEndTime";
            this.dtpPunchInEndTime.Size = new System.Drawing.Size(149, 23);
            this.dtpPunchInEndTime.TabIndex = 12;
            // 
            // dtpBeginTime
            // 
            this.dtpBeginTime.CustomFormat = "HH:mm";
            this.dtpBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBeginTime.Location = new System.Drawing.Point(128, 34);
            this.dtpBeginTime.Name = "dtpBeginTime";
            this.dtpBeginTime.Size = new System.Drawing.Size(149, 23);
            this.dtpBeginTime.TabIndex = 11;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(549, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(119, 14);
            this.label8.TabIndex = 10;
            this.label8.Text = "下班打卡截止时间";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(276, 73);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(119, 14);
            this.label7.TabIndex = 9;
            this.label7.Text = "下班打卡起始时间";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(119, 14);
            this.label6.TabIndex = 8;
            this.label6.Text = "上班打卡截止时间";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(549, 39);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(119, 14);
            this.label5.TabIndex = 7;
            this.label5.Text = "上班打卡起始时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(304, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "上班结束时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "开始上班时间";
            // 
            // cbIsHoliday
            // 
            this.cbIsHoliday.AutoSize = true;
            this.cbIsHoliday.ForeColor = System.Drawing.Color.Blue;
            this.cbIsHoliday.Location = new System.Drawing.Point(565, 10);
            this.cbIsHoliday.Name = "cbIsHoliday";
            this.cbIsHoliday.Size = new System.Drawing.Size(103, 18);
            this.cbIsHoliday.TabIndex = 4;
            this.cbIsHoliday.Text = "是 否 休 假";
            this.cbIsHoliday.UseVisualStyleBackColor = true;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(398, 7);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(146, 23);
            this.txtName.TabIndex = 3;
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(128, 3);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(149, 23);
            this.txtCode.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(318, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "排班定义名";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(33, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "排班定义编码";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 162);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(829, 359);
            this.panel2.TabIndex = 52;
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
            this.dataGridView1.Size = new System.Drawing.Size(829, 324);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(829, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // FormSchedulingDefinition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(829, 521);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormSchedulingDefinition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "排班定义";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDays)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 添加toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 修改toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DateTimePicker dtpBeginTime;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox cbIsHoliday;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dtpPunchOutEndTime;
        private System.Windows.Forms.DateTimePicker dtpPunchInBeginTime;
        private System.Windows.Forms.DateTimePicker dtpPunchOutBeginTime;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpPunchInEndTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numDays;
    }
}