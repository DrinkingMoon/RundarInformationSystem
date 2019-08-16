namespace Expression
{
    partial class 工具台帐
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpMachineAccount = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView_MachineAccount = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnOutExcel_MachineAccount = new System.Windows.Forms.Button();
            this.btnQuery_MachineAccount = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDepartment_MachineAccount = new UniversalControlLibrary.TextBoxShow();
            this.btnFindCode_MachineAccount = new System.Windows.Forms.Button();
            this.txtSpec_MachineAccount = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGoodsCode_MachineAccount = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGoodsName_MachineAccount = new UniversalControlLibrary.TextBoxShow();
            this.tpDayToDay = new System.Windows.Forms.TabPage();
            this.dataGridView_DayToDay = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.btnOutExcel_DayToDay = new System.Windows.Forms.Button();
            this.btnQuery_DayToDay = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDepartment_DayToDay = new UniversalControlLibrary.TextBoxShow();
            this.btnFindCode_DayToDay = new System.Windows.Forms.Button();
            this.txtSpec_DayToDay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGoodsCode_DayToDay = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtGoodsName_DayToDay = new UniversalControlLibrary.TextBoxShow();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpMachineAccount.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_MachineAccount)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tpDayToDay.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DayToDay)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 50);
            this.panel1.TabIndex = 57;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(448, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "工具台帐";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpMachineAccount);
            this.tabControl1.Controls.Add(this.tpDayToDay);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 50);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1016, 686);
            this.tabControl1.TabIndex = 59;
            // 
            // tpMachineAccount
            // 
            this.tpMachineAccount.Controls.Add(this.groupBox2);
            this.tpMachineAccount.Controls.Add(this.userControlDataLocalizer1);
            this.tpMachineAccount.Controls.Add(this.groupBox3);
            this.tpMachineAccount.Location = new System.Drawing.Point(4, 21);
            this.tpMachineAccount.Name = "tpMachineAccount";
            this.tpMachineAccount.Padding = new System.Windows.Forms.Padding(3);
            this.tpMachineAccount.Size = new System.Drawing.Size(1008, 661);
            this.tpMachineAccount.TabIndex = 0;
            this.tpMachineAccount.Text = "工具账存";
            this.tpMachineAccount.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView_MachineAccount);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 143);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1002, 515);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "数据集";
            // 
            // dataGridView_MachineAccount
            // 
            this.dataGridView_MachineAccount.AllowUserToAddRows = false;
            this.dataGridView_MachineAccount.AllowUserToDeleteRows = false;
            this.dataGridView_MachineAccount.AllowUserToResizeRows = false;
            this.dataGridView_MachineAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_MachineAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_MachineAccount.Location = new System.Drawing.Point(3, 17);
            this.dataGridView_MachineAccount.Name = "dataGridView_MachineAccount";
            this.dataGridView_MachineAccount.ReadOnly = true;
            this.dataGridView_MachineAccount.RowTemplate.Height = 23;
            this.dataGridView_MachineAccount.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_MachineAccount.Size = new System.Drawing.Size(996, 495);
            this.dataGridView_MachineAccount.TabIndex = 1;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(3, 111);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1002, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 2;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnOutExcel_MachineAccount);
            this.groupBox3.Controls.Add(this.btnQuery_MachineAccount);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtDepartment_MachineAccount);
            this.groupBox3.Controls.Add(this.btnFindCode_MachineAccount);
            this.groupBox3.Controls.Add(this.txtSpec_MachineAccount);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtGoodsCode_MachineAccount);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.txtGoodsName_MachineAccount);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(3, 3);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1002, 108);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "过滤条件";
            // 
            // btnOutExcel_MachineAccount
            // 
            this.btnOutExcel_MachineAccount.Location = new System.Drawing.Point(794, 65);
            this.btnOutExcel_MachineAccount.Name = "btnOutExcel_MachineAccount";
            this.btnOutExcel_MachineAccount.Size = new System.Drawing.Size(87, 25);
            this.btnOutExcel_MachineAccount.TabIndex = 201;
            this.btnOutExcel_MachineAccount.Text = "报表导出";
            this.btnOutExcel_MachineAccount.UseVisualStyleBackColor = true;
            this.btnOutExcel_MachineAccount.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // btnQuery_MachineAccount
            // 
            this.btnQuery_MachineAccount.Location = new System.Drawing.Point(687, 65);
            this.btnQuery_MachineAccount.Name = "btnQuery_MachineAccount";
            this.btnQuery_MachineAccount.Size = new System.Drawing.Size(87, 25);
            this.btnQuery_MachineAccount.TabIndex = 200;
            this.btnQuery_MachineAccount.Text = "查询";
            this.btnQuery_MachineAccount.UseVisualStyleBackColor = true;
            this.btnQuery_MachineAccount.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(27, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 199;
            this.label2.Text = "所在部门";
            // 
            // txtDepartment_MachineAccount
            // 
            this.txtDepartment_MachineAccount.FindItem = UniversalControlLibrary.TextBoxShow.FindType.部门与库房;
            this.txtDepartment_MachineAccount.Location = new System.Drawing.Point(96, 67);
            this.txtDepartment_MachineAccount.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtDepartment_MachineAccount.Name = "txtDepartment_MachineAccount";
            this.txtDepartment_MachineAccount.Size = new System.Drawing.Size(194, 21);
            this.txtDepartment_MachineAccount.TabIndex = 198;
            this.txtDepartment_MachineAccount.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtDepartment_OnCompleteSearch);
            // 
            // btnFindCode_MachineAccount
            // 
            this.btnFindCode_MachineAccount.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode_MachineAccount.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindCode_MachineAccount.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode_MachineAccount.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode_MachineAccount.Location = new System.Drawing.Point(293, 26);
            this.btnFindCode_MachineAccount.Name = "btnFindCode_MachineAccount";
            this.btnFindCode_MachineAccount.Size = new System.Drawing.Size(21, 19);
            this.btnFindCode_MachineAccount.TabIndex = 197;
            this.btnFindCode_MachineAccount.UseVisualStyleBackColor = false;
            this.btnFindCode_MachineAccount.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // txtSpec_MachineAccount
            // 
            this.txtSpec_MachineAccount.Location = new System.Drawing.Point(687, 25);
            this.txtSpec_MachineAccount.Name = "txtSpec_MachineAccount";
            this.txtSpec_MachineAccount.Size = new System.Drawing.Size(194, 21);
            this.txtSpec_MachineAccount.TabIndex = 196;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(622, 29);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 195;
            this.label6.Text = "规    格";
            // 
            // txtGoodsCode_MachineAccount
            // 
            this.txtGoodsCode_MachineAccount.Location = new System.Drawing.Point(394, 25);
            this.txtGoodsCode_MachineAccount.Name = "txtGoodsCode_MachineAccount";
            this.txtGoodsCode_MachineAccount.Size = new System.Drawing.Size(194, 21);
            this.txtGoodsCode_MachineAccount.TabIndex = 194;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(330, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 193;
            this.label5.Text = "图号型号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 192;
            this.label1.Text = "物品名称";
            // 
            // txtGoodsName_MachineAccount
            // 
            this.txtGoodsName_MachineAccount.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsName_MachineAccount.Location = new System.Drawing.Point(96, 25);
            this.txtGoodsName_MachineAccount.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtGoodsName_MachineAccount.Name = "txtGoodsName_MachineAccount";
            this.txtGoodsName_MachineAccount.Size = new System.Drawing.Size(194, 21);
            this.txtGoodsName_MachineAccount.TabIndex = 191;
            this.txtGoodsName_MachineAccount.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsName_OnCompleteSearch);
            // 
            // tpDayToDay
            // 
            this.tpDayToDay.Controls.Add(this.dataGridView_DayToDay);
            this.tpDayToDay.Controls.Add(this.groupBox1);
            this.tpDayToDay.Location = new System.Drawing.Point(4, 21);
            this.tpDayToDay.Name = "tpDayToDay";
            this.tpDayToDay.Padding = new System.Windows.Forms.Padding(3);
            this.tpDayToDay.Size = new System.Drawing.Size(1008, 661);
            this.tpDayToDay.TabIndex = 1;
            this.tpDayToDay.Text = "工具流水账";
            this.tpDayToDay.UseVisualStyleBackColor = true;
            // 
            // dataGridView_DayToDay
            // 
            this.dataGridView_DayToDay.AllowUserToAddRows = false;
            this.dataGridView_DayToDay.AllowUserToDeleteRows = false;
            this.dataGridView_DayToDay.AllowUserToResizeRows = false;
            this.dataGridView_DayToDay.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_DayToDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView_DayToDay.Location = new System.Drawing.Point(3, 137);
            this.dataGridView_DayToDay.Name = "dataGridView_DayToDay";
            this.dataGridView_DayToDay.ReadOnly = true;
            this.dataGridView_DayToDay.RowTemplate.Height = 23;
            this.dataGridView_DayToDay.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView_DayToDay.Size = new System.Drawing.Size(1002, 521);
            this.dataGridView_DayToDay.TabIndex = 3;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.DtpEndDate);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.DtpStartDate);
            this.groupBox1.Controls.Add(this.btnOutExcel_DayToDay);
            this.groupBox1.Controls.Add(this.btnQuery_DayToDay);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtDepartment_DayToDay);
            this.groupBox1.Controls.Add(this.btnFindCode_DayToDay);
            this.groupBox1.Controls.Add(this.txtSpec_DayToDay);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtGoodsCode_DayToDay);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtGoodsName_DayToDay);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1002, 134);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询条件";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(93, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(17, 12);
            this.label9.TabIndex = 217;
            this.label9.Text = "从";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(344, 102);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 216;
            this.label10.Text = "到";
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.Location = new System.Drawing.Point(394, 98);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(194, 21);
            this.DtpEndDate.TabIndex = 215;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(27, 102);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 214;
            this.label11.Text = "查询时间";
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.Location = new System.Drawing.Point(114, 98);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(176, 21);
            this.DtpStartDate.TabIndex = 213;
            // 
            // btnOutExcel_DayToDay
            // 
            this.btnOutExcel_DayToDay.Location = new System.Drawing.Point(794, 96);
            this.btnOutExcel_DayToDay.Name = "btnOutExcel_DayToDay";
            this.btnOutExcel_DayToDay.Size = new System.Drawing.Size(87, 25);
            this.btnOutExcel_DayToDay.TabIndex = 212;
            this.btnOutExcel_DayToDay.Text = "报表导出";
            this.btnOutExcel_DayToDay.UseVisualStyleBackColor = true;
            this.btnOutExcel_DayToDay.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // btnQuery_DayToDay
            // 
            this.btnQuery_DayToDay.Location = new System.Drawing.Point(687, 96);
            this.btnQuery_DayToDay.Name = "btnQuery_DayToDay";
            this.btnQuery_DayToDay.Size = new System.Drawing.Size(87, 25);
            this.btnQuery_DayToDay.TabIndex = 211;
            this.btnQuery_DayToDay.Text = "查询";
            this.btnQuery_DayToDay.UseVisualStyleBackColor = true;
            this.btnQuery_DayToDay.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 66);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 210;
            this.label3.Text = "所在部门";
            // 
            // txtDepartment_DayToDay
            // 
            this.txtDepartment_DayToDay.FindItem = UniversalControlLibrary.TextBoxShow.FindType.部门与库房;
            this.txtDepartment_DayToDay.Location = new System.Drawing.Point(96, 62);
            this.txtDepartment_DayToDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtDepartment_DayToDay.Name = "txtDepartment_DayToDay";
            this.txtDepartment_DayToDay.Size = new System.Drawing.Size(194, 21);
            this.txtDepartment_DayToDay.TabIndex = 209;
            this.txtDepartment_DayToDay.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtDepartment_OnCompleteSearch);
            // 
            // btnFindCode_DayToDay
            // 
            this.btnFindCode_DayToDay.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode_DayToDay.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindCode_DayToDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode_DayToDay.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode_DayToDay.Location = new System.Drawing.Point(293, 26);
            this.btnFindCode_DayToDay.Name = "btnFindCode_DayToDay";
            this.btnFindCode_DayToDay.Size = new System.Drawing.Size(21, 19);
            this.btnFindCode_DayToDay.TabIndex = 208;
            this.btnFindCode_DayToDay.UseVisualStyleBackColor = false;
            this.btnFindCode_DayToDay.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // txtSpec_DayToDay
            // 
            this.txtSpec_DayToDay.Location = new System.Drawing.Point(687, 25);
            this.txtSpec_DayToDay.Name = "txtSpec_DayToDay";
            this.txtSpec_DayToDay.Size = new System.Drawing.Size(194, 21);
            this.txtSpec_DayToDay.TabIndex = 207;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(622, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 206;
            this.label4.Text = "规    格";
            // 
            // txtGoodsCode_DayToDay
            // 
            this.txtGoodsCode_DayToDay.Location = new System.Drawing.Point(394, 25);
            this.txtGoodsCode_DayToDay.Name = "txtGoodsCode_DayToDay";
            this.txtGoodsCode_DayToDay.Size = new System.Drawing.Size(194, 21);
            this.txtGoodsCode_DayToDay.TabIndex = 205;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(330, 29);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 204;
            this.label7.Text = "图号型号";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(27, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 203;
            this.label8.Text = "物品名称";
            // 
            // txtGoodsName_DayToDay
            // 
            this.txtGoodsName_DayToDay.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsName_DayToDay.Location = new System.Drawing.Point(96, 25);
            this.txtGoodsName_DayToDay.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtGoodsName_DayToDay.Name = "txtGoodsName_DayToDay";
            this.txtGoodsName_DayToDay.Size = new System.Drawing.Size(194, 21);
            this.txtGoodsName_DayToDay.TabIndex = 202;
            this.txtGoodsName_DayToDay.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsName_OnCompleteSearch);
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
            // 工具台帐
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Name = "工具台帐";
            this.Text = "工具台帐";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tpMachineAccount.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_MachineAccount)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tpDayToDay.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_DayToDay)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpMachineAccount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tpDayToDay;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridView_MachineAccount;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView_DayToDay;
        private System.Windows.Forms.Button btnFindCode_MachineAccount;
        private System.Windows.Forms.TextBox txtSpec_MachineAccount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGoodsCode_MachineAccount;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.TextBoxShow txtGoodsName_MachineAccount;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtDepartment_MachineAccount;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnOutExcel_MachineAccount;
        private System.Windows.Forms.Button btnQuery_MachineAccount;
        private System.Windows.Forms.Button btnOutExcel_DayToDay;
        private System.Windows.Forms.Button btnQuery_DayToDay;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtDepartment_DayToDay;
        private System.Windows.Forms.Button btnFindCode_DayToDay;
        private System.Windows.Forms.TextBox txtSpec_DayToDay;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtGoodsCode_DayToDay;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private UniversalControlLibrary.TextBoxShow txtGoodsName_DayToDay;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
    }
}