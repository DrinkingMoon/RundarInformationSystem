namespace Form_Quality_QC
{
    partial class 不合格品隔离处置单明细
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.补充信息 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.补充人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.补充日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.num_QC_DisqualifiedCount = new System.Windows.Forms.NumericUpDown();
            this.btn_ReportFile_Down = new System.Windows.Forms.Button();
            this.btn_ReportFile_Up = new System.Windows.Forms.Button();
            this.lbReportFile = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.num_QC_ScraptCount = new System.Windows.Forms.NumericUpDown();
            this.label20 = new System.Windows.Forms.Label();
            this.num_QC_ConcessionCount = new System.Windows.Forms.NumericUpDown();
            this.label21 = new System.Windows.Forms.Label();
            this.num_QC_QualifiedCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtProcessMethodRequire = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.rb_ReturnProcess_TH = new System.Windows.Forms.RadioButton();
            this.rb_ReturnProcess_BF = new System.Windows.Forms.RadioButton();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.num_WorkHours = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.num_PH_DisqualifiendCount = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.num_PH_QualifiedCount = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.numGoodsCount = new System.Windows.Forms.NumericUpDown();
            this.txtIsolationReason = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStorageID = new System.Windows.Forms.ComboBox();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.customPanel1.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_DisqualifiedCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_ScraptCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_ConcessionCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_QualifiedCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_WorkHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PH_DisqualifiendCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PH_QualifiedCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoodsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.customGroupBox1);
            this.customPanel1.Controls.Add(this.groupBox3);
            this.customPanel1.Controls.Add(this.groupBox2);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(893, 661);
            this.customPanel1.TabIndex = 0;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.customDataGridView1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 469);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(893, 192);
            this.customGroupBox1.TabIndex = 3;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "补充信息";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.customDataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.补充信息,
            this.补充人,
            this.补充日期});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 17);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(887, 172);
            this.customDataGridView1.TabIndex = 5;
            // 
            // 补充信息
            // 
            this.补充信息.DataPropertyName = "补充信息";
            this.补充信息.HeaderText = "补充信息";
            this.补充信息.Name = "补充信息";
            this.补充信息.ReadOnly = true;
            this.补充信息.Width = 78;
            // 
            // 补充人
            // 
            this.补充人.DataPropertyName = "补充人";
            this.补充人.HeaderText = "补充人";
            this.补充人.Name = "补充人";
            this.补充人.ReadOnly = true;
            this.补充人.Width = 66;
            // 
            // 补充日期
            // 
            this.补充日期.DataPropertyName = "补充日期";
            this.补充日期.HeaderText = "补充日期";
            this.补充日期.Name = "补充日期";
            this.补充日期.ReadOnly = true;
            this.补充日期.Width = 78;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.num_QC_DisqualifiedCount);
            this.groupBox3.Controls.Add(this.btn_ReportFile_Down);
            this.groupBox3.Controls.Add(this.btn_ReportFile_Up);
            this.groupBox3.Controls.Add(this.lbReportFile);
            this.groupBox3.Controls.Add(this.label23);
            this.groupBox3.Controls.Add(this.num_QC_ScraptCount);
            this.groupBox3.Controls.Add(this.label20);
            this.groupBox3.Controls.Add(this.num_QC_ConcessionCount);
            this.groupBox3.Controls.Add(this.label21);
            this.groupBox3.Controls.Add(this.num_QC_QualifiedCount);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 375);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(893, 94);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Tag = "59";
            this.groupBox3.Text = "QC信息";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(37, 66);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 340;
            this.label11.Text = "不合格数";
            // 
            // num_QC_DisqualifiedCount
            // 
            this.num_QC_DisqualifiedCount.Location = new System.Drawing.Point(102, 62);
            this.num_QC_DisqualifiedCount.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_QC_DisqualifiedCount.Name = "num_QC_DisqualifiedCount";
            this.num_QC_DisqualifiedCount.Size = new System.Drawing.Size(120, 21);
            this.num_QC_DisqualifiedCount.TabIndex = 339;
            // 
            // btn_ReportFile_Down
            // 
            this.btn_ReportFile_Down.Location = new System.Drawing.Point(825, 43);
            this.btn_ReportFile_Down.Name = "btn_ReportFile_Down";
            this.btn_ReportFile_Down.Size = new System.Drawing.Size(56, 23);
            this.btn_ReportFile_Down.TabIndex = 338;
            this.btn_ReportFile_Down.Text = "下载";
            this.btn_ReportFile_Down.UseVisualStyleBackColor = true;
            this.btn_ReportFile_Down.Click += new System.EventHandler(this.btn_ReportFile_Down_Click);
            // 
            // btn_ReportFile_Up
            // 
            this.btn_ReportFile_Up.Location = new System.Drawing.Point(753, 43);
            this.btn_ReportFile_Up.Name = "btn_ReportFile_Up";
            this.btn_ReportFile_Up.Size = new System.Drawing.Size(56, 23);
            this.btn_ReportFile_Up.TabIndex = 337;
            this.btn_ReportFile_Up.Text = "上传";
            this.btn_ReportFile_Up.UseVisualStyleBackColor = true;
            this.btn_ReportFile_Up.Click += new System.EventHandler(this.btn_ReportFile_Up_Click);
            // 
            // lbReportFile
            // 
            this.lbReportFile.AutoSize = true;
            this.lbReportFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbReportFile.Location = new System.Drawing.Point(670, 48);
            this.lbReportFile.Name = "lbReportFile";
            this.lbReportFile.Size = new System.Drawing.Size(77, 12);
            this.lbReportFile.TabIndex = 336;
            this.lbReportFile.Text = "检验报告附件";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label23.Location = new System.Drawing.Point(458, 48);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(65, 12);
            this.label23.TabIndex = 335;
            this.label23.Text = "检测报废数";
            // 
            // num_QC_ScraptCount
            // 
            this.num_QC_ScraptCount.Location = new System.Drawing.Point(533, 44);
            this.num_QC_ScraptCount.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_QC_ScraptCount.Name = "num_QC_ScraptCount";
            this.num_QC_ScraptCount.Size = new System.Drawing.Size(120, 21);
            this.num_QC_ScraptCount.TabIndex = 334;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label20.Location = new System.Drawing.Point(239, 48);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(53, 12);
            this.label20.TabIndex = 326;
            this.label20.Text = "让 步 数";
            // 
            // num_QC_ConcessionCount
            // 
            this.num_QC_ConcessionCount.Location = new System.Drawing.Point(308, 44);
            this.num_QC_ConcessionCount.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_QC_ConcessionCount.Name = "num_QC_ConcessionCount";
            this.num_QC_ConcessionCount.Size = new System.Drawing.Size(120, 21);
            this.num_QC_ConcessionCount.TabIndex = 325;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label21.Location = new System.Drawing.Point(37, 30);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(53, 12);
            this.label21.TabIndex = 324;
            this.label21.Text = "合 格 数";
            // 
            // num_QC_QualifiedCount
            // 
            this.num_QC_QualifiedCount.Location = new System.Drawing.Point(102, 26);
            this.num_QC_QualifiedCount.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_QC_QualifiedCount.Name = "num_QC_QualifiedCount";
            this.num_QC_QualifiedCount.Size = new System.Drawing.Size(120, 21);
            this.num_QC_QualifiedCount.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtProcessMethodRequire);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.rb_ReturnProcess_TH);
            this.groupBox2.Controls.Add(this.rb_ReturnProcess_BF);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.num_WorkHours);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.num_PH_DisqualifiendCount);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.num_PH_QualifiedCount);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(893, 132);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Tag = "58";
            this.groupBox2.Text = "处理人信息";
            // 
            // txtProcessMethodRequire
            // 
            this.txtProcessMethodRequire.Location = new System.Drawing.Point(102, 71);
            this.txtProcessMethodRequire.Multiline = true;
            this.txtProcessMethodRequire.Name = "txtProcessMethodRequire";
            this.txtProcessMethodRequire.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProcessMethodRequire.Size = new System.Drawing.Size(551, 41);
            this.txtProcessMethodRequire.TabIndex = 336;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(37, 85);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 335;
            this.label13.Text = "处理方案";
            // 
            // rb_ReturnProcess_TH
            // 
            this.rb_ReturnProcess_TH.AutoSize = true;
            this.rb_ReturnProcess_TH.Location = new System.Drawing.Point(753, 54);
            this.rb_ReturnProcess_TH.Name = "rb_ReturnProcess_TH";
            this.rb_ReturnProcess_TH.Size = new System.Drawing.Size(71, 16);
            this.rb_ReturnProcess_TH.TabIndex = 331;
            this.rb_ReturnProcess_TH.TabStop = true;
            this.rb_ReturnProcess_TH.Text = "退    货";
            this.rb_ReturnProcess_TH.UseVisualStyleBackColor = true;
            // 
            // rb_ReturnProcess_BF
            // 
            this.rb_ReturnProcess_BF.AutoSize = true;
            this.rb_ReturnProcess_BF.Location = new System.Drawing.Point(753, 20);
            this.rb_ReturnProcess_BF.Name = "rb_ReturnProcess_BF";
            this.rb_ReturnProcess_BF.Size = new System.Drawing.Size(71, 16);
            this.rb_ReturnProcess_BF.TabIndex = 330;
            this.rb_ReturnProcess_BF.TabStop = true;
            this.rb_ReturnProcess_BF.Text = "就地报废";
            this.rb_ReturnProcess_BF.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(670, 37);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 329;
            this.label17.Text = "退货处理";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(467, 37);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 328;
            this.label16.Text = "工    时";
            // 
            // num_WorkHours
            // 
            this.num_WorkHours.DecimalPlaces = 2;
            this.num_WorkHours.Location = new System.Drawing.Point(532, 33);
            this.num_WorkHours.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_WorkHours.Name = "num_WorkHours";
            this.num_WorkHours.Size = new System.Drawing.Size(120, 21);
            this.num_WorkHours.TabIndex = 327;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(238, 37);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 326;
            this.label15.Text = "不合格数";
            // 
            // num_PH_DisqualifiendCount
            // 
            this.num_PH_DisqualifiendCount.Location = new System.Drawing.Point(307, 33);
            this.num_PH_DisqualifiendCount.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_PH_DisqualifiendCount.Name = "num_PH_DisqualifiendCount";
            this.num_PH_DisqualifiendCount.Size = new System.Drawing.Size(120, 21);
            this.num_PH_DisqualifiendCount.TabIndex = 325;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(37, 37);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 324;
            this.label14.Text = "合 格 数";
            // 
            // num_PH_QualifiedCount
            // 
            this.num_PH_QualifiedCount.Location = new System.Drawing.Point(102, 33);
            this.num_PH_QualifiedCount.Maximum = new decimal(new int[] {
            1215752192,
            23,
            0,
            0});
            this.num_PH_QualifiedCount.Name = "num_PH_QualifiedCount";
            this.num_PH_QualifiedCount.Size = new System.Drawing.Size(120, 21);
            this.num_PH_QualifiedCount.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.numGoodsCount);
            this.groupBox1.Controls.Add(this.txtIsolationReason);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtBatchNo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtGoodsName);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbStorageID);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(893, 243);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Tag = "57";
            this.groupBox1.Text = "编制人信息";
            // 
            // numGoodsCount
            // 
            this.numGoodsCount.DecimalPlaces = 2;
            this.numGoodsCount.Enabled = false;
            this.numGoodsCount.Location = new System.Drawing.Point(353, 145);
            this.numGoodsCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numGoodsCount.Name = "numGoodsCount";
            this.numGoodsCount.Size = new System.Drawing.Size(181, 21);
            this.numGoodsCount.TabIndex = 335;
            this.numGoodsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtIsolationReason
            // 
            this.txtIsolationReason.Location = new System.Drawing.Point(77, 187);
            this.txtIsolationReason.Multiline = true;
            this.txtIsolationReason.Name = "txtIsolationReason";
            this.txtIsolationReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtIsolationReason.Size = new System.Drawing.Size(782, 47);
            this.txtIsolationReason.TabIndex = 332;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(17, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 44);
            this.label12.TabIndex = 331;
            this.label12.Text = "隔离原因及处理要求";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(540, 149);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(17, 12);
            this.label10.TabIndex = 328;
            this.label10.Text = "件";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(294, 149);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 327;
            this.label9.Text = "隔 离 数";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(594, 149);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 325;
            this.label8.Text = "供 应 商";
            // 
            // txtProvider
            // 
            this.txtProvider.Enabled = false;
            this.txtProvider.Location = new System.Drawing.Point(653, 145);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.Size = new System.Drawing.Size(206, 21);
            this.txtProvider.TabIndex = 324;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(17, 149);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 323;
            this.label3.Text = "批 次 号";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.DataResult = null;
            this.txtBatchNo.DataTableResult = null;
            this.txtBatchNo.EditingControlDataGridView = null;
            this.txtBatchNo.EditingControlFormattedValue = "";
            this.txtBatchNo.EditingControlRowIndex = 0;
            this.txtBatchNo.EditingControlValueChanged = true;
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品批次;
            this.txtBatchNo.IsMultiSelect = false;
            this.txtBatchNo.Location = new System.Drawing.Point(76, 145);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ShowResultForm = true;
            this.txtBatchNo.Size = new System.Drawing.Size(198, 21);
            this.txtBatchNo.StrEndSql = null;
            this.txtBatchNo.TabIndex = 322;
            this.txtBatchNo.TabStop = false;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(594, 109);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 321;
            this.label7.Text = "规    格";
            // 
            // txtSpec
            // 
            this.txtSpec.Enabled = false;
            this.txtSpec.Location = new System.Drawing.Point(652, 105);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(207, 21);
            this.txtSpec.TabIndex = 320;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(294, 109);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 319;
            this.label6.Text = "物品名称";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Enabled = false;
            this.txtGoodsName.Location = new System.Drawing.Point(353, 105);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.Size = new System.Drawing.Size(204, 21);
            this.txtGoodsName.TabIndex = 318;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(17, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 317;
            this.label2.Text = "图号型号";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.DataTableResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品库存合计;
            this.txtGoodsCode.IsMultiSelect = false;
            this.txtGoodsCode.Location = new System.Drawing.Point(76, 105);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(198, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 316;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            this.txtGoodsCode.Enter += new System.EventHandler(this.txtGoodsCode_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(18, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 315;
            this.label1.Text = "所属库房";
            // 
            // cmbStorageID
            // 
            this.cmbStorageID.FormattingEnabled = true;
            this.cmbStorageID.Location = new System.Drawing.Point(77, 64);
            this.cmbStorageID.Name = "cmbStorageID";
            this.cmbStorageID.Size = new System.Drawing.Size(197, 20);
            this.cmbStorageID.TabIndex = 314;
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(391, 32);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 313;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(297, 32);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 312;
            this.label5.Text = "业务状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(77, 28);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(197, 21);
            this.txtBillNo.TabIndex = 310;
            this.txtBillNo.Text = "GLD201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(18, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 311;
            this.label4.Text = "业务编号";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Multiselect = true;
            // 
            // 不合格品隔离处置单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 661);
            this.Controls.Add(this.customPanel1);
            this.Name = "不合格品隔离处置单明细";
            this.Text = "不合格品隔离处置单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.customPanel1.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_DisqualifiedCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_ScraptCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_ConcessionCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_QC_QualifiedCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.num_WorkHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PH_DisqualifiendCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.num_PH_QualifiedCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoodsCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStorageID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtBatchNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtIsolationReason;
        private System.Windows.Forms.RadioButton rb_ReturnProcess_TH;
        private System.Windows.Forms.RadioButton rb_ReturnProcess_BF;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown num_WorkHours;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown num_PH_DisqualifiendCount;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown num_PH_QualifiedCount;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown num_QC_ScraptCount;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.NumericUpDown num_QC_ConcessionCount;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.NumericUpDown num_QC_QualifiedCount;
        private System.Windows.Forms.Button btn_ReportFile_Up;
        private System.Windows.Forms.Label lbReportFile;
        private System.Windows.Forms.Button btn_ReportFile_Down;
        private System.Windows.Forms.NumericUpDown numGoodsCount;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown num_QC_DisqualifiedCount;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox txtProcessMethodRequire;
        private System.Windows.Forms.Label label13;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 补充信息;
        private System.Windows.Forms.DataGridViewTextBoxColumn 补充人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 补充日期;

    }
}