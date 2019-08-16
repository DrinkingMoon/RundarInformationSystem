using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class FormAttendanceScheme
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormAttendanceScheme));
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
            this.numEndDateMonth = new System.Windows.Forms.NumericUpDown();
            this.numBeginDateMonth = new System.Windows.Forms.NumericUpDown();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.cbInPublicHoliday = new System.Windows.Forms.CheckBox();
            this.txtSchemeName = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.dtpPunchOutEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpPunchOutBeginTime = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.dtpPunchInEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpPunchInBeginTime = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dtpEndTimeAfternoon = new System.Windows.Forms.DateTimePicker();
            this.dtpBeginTimeAfternoon = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpEndTimeMorning = new System.Windows.Forms.DateTimePicker();
            this.dtpBeginTimeMorning = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbAttendanceMode = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSchemeCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndDateMonth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeginDateMonth)).BeginInit();
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
            this.toolStrip1.Size = new System.Drawing.Size(911, 25);
            this.toolStrip1.TabIndex = 49;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripButton1
            // 
            this.新建toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.新建toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("新建toolStripButton1.Image")));
            this.新建toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripButton1.Name = "新建toolStripButton1";
            this.新建toolStripButton1.Size = new System.Drawing.Size(39, 22);
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
            this.添加toolStripButton1.Size = new System.Drawing.Size(39, 22);
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
            this.修改toolStripButton2.Size = new System.Drawing.Size(39, 22);
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
            this.删除toolStripButton1.Size = new System.Drawing.Size(39, 22);
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
            this.刷新toolStripButton1.Size = new System.Drawing.Size(57, 22);
            this.刷新toolStripButton1.Tag = "Add";
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.Controls.Add(this.numEndDateMonth);
            this.panel1.Controls.Add(this.numBeginDateMonth);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.label17);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.cbInPublicHoliday);
            this.panel1.Controls.Add(this.txtSchemeName);
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.dtpPunchOutEndTime);
            this.panel1.Controls.Add(this.dtpPunchOutBeginTime);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.label12);
            this.panel1.Controls.Add(this.dtpPunchInEndTime);
            this.panel1.Controls.Add(this.dtpPunchInBeginTime);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.dtpEndTimeAfternoon);
            this.panel1.Controls.Add(this.dtpBeginTimeAfternoon);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.dtpEndTimeMorning);
            this.panel1.Controls.Add(this.dtpBeginTimeMorning);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.cmbAttendanceMode);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtSchemeCode);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(911, 235);
            this.panel1.TabIndex = 50;
            // 
            // numEndDateMonth
            // 
            this.numEndDateMonth.Location = new System.Drawing.Point(336, 59);
            this.numEndDateMonth.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numEndDateMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numEndDateMonth.Name = "numEndDateMonth";
            this.numEndDateMonth.Size = new System.Drawing.Size(139, 23);
            this.numEndDateMonth.TabIndex = 33;
            this.numEndDateMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // numBeginDateMonth
            // 
            this.numBeginDateMonth.Location = new System.Drawing.Point(98, 59);
            this.numBeginDateMonth.Maximum = new decimal(new int[] {
            31,
            0,
            0,
            0});
            this.numBeginDateMonth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBeginDateMonth.Name = "numBeginDateMonth";
            this.numBeginDateMonth.Size = new System.Drawing.Size(140, 23);
            this.numBeginDateMonth.TabIndex = 32;
            this.numBeginDateMonth.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(98, 205);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(808, 23);
            this.txtRemark.TabIndex = 31;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.Color.Blue;
            this.label17.Location = new System.Drawing.Point(5, 208);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(63, 14);
            this.label17.TabIndex = 30;
            this.label17.Text = "备    注";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Red;
            this.label16.Location = new System.Drawing.Point(544, 87);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(280, 14);
            this.label16.TabIndex = 29;
            this.label16.Text = "若为是,不能对填加班条的情况重复累计加班";
            this.label16.Visible = false;
            // 
            // cbInPublicHoliday
            // 
            this.cbInPublicHoliday.AutoSize = true;
            this.cbInPublicHoliday.ForeColor = System.Drawing.Color.Blue;
            this.cbInPublicHoliday.Location = new System.Drawing.Point(547, 59);
            this.cbInPublicHoliday.Name = "cbInPublicHoliday";
            this.cbInPublicHoliday.Size = new System.Drawing.Size(208, 18);
            this.cbInPublicHoliday.TabIndex = 28;
            this.cbInPublicHoliday.Text = "法定假日上班是否自动算加班";
            this.cbInPublicHoliday.UseVisualStyleBackColor = true;
            this.cbInPublicHoliday.Visible = false;
            // 
            // txtSchemeName
            // 
            this.txtSchemeName.Location = new System.Drawing.Point(336, 10);
            this.txtSchemeName.Name = "txtSchemeName";
            this.txtSchemeName.Size = new System.Drawing.Size(139, 23);
            this.txtSchemeName.TabIndex = 27;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ForeColor = System.Drawing.Color.Blue;
            this.label15.Location = new System.Drawing.Point(244, 13);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 14);
            this.label15.TabIndex = 26;
            this.label15.Text = "方案名称";
            // 
            // dtpPunchOutEndTime
            // 
            this.dtpPunchOutEndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpPunchOutEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpPunchOutEndTime.Location = new System.Drawing.Point(766, 167);
            this.dtpPunchOutEndTime.Name = "dtpPunchOutEndTime";
            this.dtpPunchOutEndTime.ShowCheckBox = true;
            this.dtpPunchOutEndTime.Size = new System.Drawing.Size(140, 23);
            this.dtpPunchOutEndTime.TabIndex = 25;
            this.dtpPunchOutEndTime.Value = new System.DateTime(2012, 9, 18, 23, 59, 0, 0);
            // 
            // dtpPunchOutBeginTime
            // 
            this.dtpPunchOutBeginTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpPunchOutBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpPunchOutBeginTime.Location = new System.Drawing.Point(547, 167);
            this.dtpPunchOutBeginTime.Name = "dtpPunchOutBeginTime";
            this.dtpPunchOutBeginTime.ShowCheckBox = true;
            this.dtpPunchOutBeginTime.Size = new System.Drawing.Size(140, 23);
            this.dtpPunchOutBeginTime.TabIndex = 24;
            this.dtpPunchOutBeginTime.Value = new System.DateTime(2012, 9, 18, 17, 30, 0, 0);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(697, 167);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 28);
            this.label11.TabIndex = 23;
            this.label11.Text = "下班打卡\r\n截止时间";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(479, 167);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 28);
            this.label12.TabIndex = 22;
            this.label12.Text = "下班打卡\r\n起始时间";
            // 
            // dtpPunchInEndTime
            // 
            this.dtpPunchInEndTime.CustomFormat = "HH:mm";
            this.dtpPunchInEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpPunchInEndTime.Location = new System.Drawing.Point(336, 167);
            this.dtpPunchInEndTime.Name = "dtpPunchInEndTime";
            this.dtpPunchInEndTime.ShowCheckBox = true;
            this.dtpPunchInEndTime.Size = new System.Drawing.Size(139, 23);
            this.dtpPunchInEndTime.TabIndex = 21;
            this.dtpPunchInEndTime.Value = new System.DateTime(2012, 9, 18, 8, 33, 0, 0);
            // 
            // dtpPunchInBeginTime
            // 
            this.dtpPunchInBeginTime.CustomFormat = "HH:mm";
            this.dtpPunchInBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpPunchInBeginTime.Location = new System.Drawing.Point(98, 167);
            this.dtpPunchInBeginTime.Name = "dtpPunchInBeginTime";
            this.dtpPunchInBeginTime.ShowCheckBox = true;
            this.dtpPunchInBeginTime.Size = new System.Drawing.Size(140, 23);
            this.dtpPunchInBeginTime.TabIndex = 20;
            this.dtpPunchInBeginTime.Value = new System.DateTime(2012, 9, 18, 6, 0, 0, 0);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(253, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 28);
            this.label13.TabIndex = 19;
            this.label13.Text = "上班打卡\r\n截止时间";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(5, 167);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 28);
            this.label14.TabIndex = 18;
            this.label14.Text = "上班打卡\r\n起始时间";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(5, 40);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(161, 14);
            this.label10.TabIndex = 17;
            this.label10.Text = "(下列用于非自然月考勤)";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(5, 97);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(147, 14);
            this.label9.TabIndex = 16;
            this.label9.Text = "(下列用于非排班模式)";
            // 
            // dtpEndTimeAfternoon
            // 
            this.dtpEndTimeAfternoon.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEndTimeAfternoon.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTimeAfternoon.Location = new System.Drawing.Point(766, 124);
            this.dtpEndTimeAfternoon.Name = "dtpEndTimeAfternoon";
            this.dtpEndTimeAfternoon.ShowCheckBox = true;
            this.dtpEndTimeAfternoon.Size = new System.Drawing.Size(140, 23);
            this.dtpEndTimeAfternoon.TabIndex = 15;
            this.dtpEndTimeAfternoon.Value = new System.DateTime(2012, 9, 18, 17, 30, 0, 0);
            // 
            // dtpBeginTimeAfternoon
            // 
            this.dtpBeginTimeAfternoon.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpBeginTimeAfternoon.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpBeginTimeAfternoon.Location = new System.Drawing.Point(547, 124);
            this.dtpBeginTimeAfternoon.Name = "dtpBeginTimeAfternoon";
            this.dtpBeginTimeAfternoon.ShowCheckBox = true;
            this.dtpBeginTimeAfternoon.Size = new System.Drawing.Size(140, 23);
            this.dtpBeginTimeAfternoon.TabIndex = 14;
            this.dtpBeginTimeAfternoon.Value = new System.DateTime(2012, 9, 18, 13, 30, 0, 0);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(697, 124);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 28);
            this.label7.TabIndex = 13;
            this.label7.Text = "下午结\r\n束时间";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(479, 124);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(49, 28);
            this.label8.TabIndex = 12;
            this.label8.Text = "下午开\r\n始时间";
            // 
            // dtpEndTimeMorning
            // 
            this.dtpEndTimeMorning.CustomFormat = "HH:mm";
            this.dtpEndTimeMorning.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpEndTimeMorning.Location = new System.Drawing.Point(336, 124);
            this.dtpEndTimeMorning.Name = "dtpEndTimeMorning";
            this.dtpEndTimeMorning.ShowCheckBox = true;
            this.dtpEndTimeMorning.Size = new System.Drawing.Size(139, 23);
            this.dtpEndTimeMorning.TabIndex = 11;
            this.dtpEndTimeMorning.Value = new System.DateTime(2012, 9, 18, 12, 0, 0, 0);
            // 
            // dtpBeginTimeMorning
            // 
            this.dtpBeginTimeMorning.CustomFormat = "HH:mm";
            this.dtpBeginTimeMorning.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.dtpBeginTimeMorning.Location = new System.Drawing.Point(98, 124);
            this.dtpBeginTimeMorning.Name = "dtpBeginTimeMorning";
            this.dtpBeginTimeMorning.ShowCheckBox = true;
            this.dtpBeginTimeMorning.Size = new System.Drawing.Size(140, 23);
            this.dtpBeginTimeMorning.TabIndex = 10;
            this.dtpBeginTimeMorning.Value = new System.DateTime(2012, 9, 18, 8, 30, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(242, 128);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "上午结束时间";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(5, 128);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 8;
            this.label6.Text = "上午开始时间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(242, 64);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 5;
            this.label4.Text = "本月结束时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 64);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "上月开始时间";
            // 
            // cmbAttendanceMode
            // 
            this.cmbAttendanceMode.FormattingEnabled = true;
            this.cmbAttendanceMode.Items.AddRange(new object[] {
            "自然月考勤(每月1号到月底)",
            "非自然月考勤(上月xx号到本月xx号)",
            "自然月排班考勤",
            "不考勤"});
            this.cmbAttendanceMode.Location = new System.Drawing.Point(547, 10);
            this.cmbAttendanceMode.Name = "cmbAttendanceMode";
            this.cmbAttendanceMode.Size = new System.Drawing.Size(359, 21);
            this.cmbAttendanceMode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(479, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "考勤模式";
            // 
            // txtSchemeCode
            // 
            this.txtSchemeCode.Location = new System.Drawing.Point(98, 10);
            this.txtSchemeCode.Name = "txtSchemeCode";
            this.txtSchemeCode.Size = new System.Drawing.Size(140, 23);
            this.txtSchemeCode.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(5, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "方案编码";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 260);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(911, 283);
            this.panel2.TabIndex = 51;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(911, 251);
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
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(911, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // FormAttendanceScheme
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(911, 543);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.MaximizeBox = false;
            this.Name = "FormAttendanceScheme";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置考勤方案";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEndDateMonth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeginDateMonth)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 添加toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 修改toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSchemeCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.ComboBox cmbAttendanceMode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpEndTimeMorning;
        private System.Windows.Forms.DateTimePicker dtpBeginTimeMorning;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DateTimePicker dtpEndTimeAfternoon;
        private System.Windows.Forms.DateTimePicker dtpBeginTimeAfternoon;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpPunchOutEndTime;
        private System.Windows.Forms.DateTimePicker dtpPunchOutBeginTime;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DateTimePicker dtpPunchInEndTime;
        private System.Windows.Forms.DateTimePicker dtpPunchInBeginTime;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox cbInPublicHoliday;
        private System.Windows.Forms.TextBox txtSchemeName;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.NumericUpDown numEndDateMonth;
        private System.Windows.Forms.NumericUpDown numBeginDateMonth;
    }
}