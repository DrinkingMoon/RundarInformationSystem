namespace Form_Quality_QC
{
    partial class 检验报告明细
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(检验报告明细));
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.检验项目 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.检验结果 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbIsRepeat = new System.Windows.Forms.CheckBox();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtChecker = new UniversalControlLibrary.TextBoxShow();
            this.cmbGoodsType2 = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.chbIsQualified = new System.Windows.Forms.CheckBox();
            this.cmbGoodsImportance = new System.Windows.Forms.ComboBox();
            this.cmbGoodsType1 = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpCheckDate = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lbUnit = new System.Windows.Forms.Label();
            this.numGoodsCount = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtInspectionExplain = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtBillRelate = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtInspectionReportNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoodsCount)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.customDataGridView1);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(812, 433);
            this.customPanel1.TabIndex = 1;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.检验项目,
            this.检验结果,
            this.单据号});
            this.customDataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 309);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(812, 124);
            this.customDataGridView1.TabIndex = 21;
            // 
            // 检验项目
            // 
            this.检验项目.DataPropertyName = "检验项目";
            this.检验项目.HeaderText = "检验项目";
            this.检验项目.Name = "检验项目";
            this.检验项目.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.检验项目.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.检验项目.Width = 350;
            // 
            // 检验结果
            // 
            this.检验结果.DataPropertyName = "检验结果";
            this.检验结果.HeaderText = "检验结果";
            this.检验结果.Name = "检验结果";
            this.检验结果.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.检验结果.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.检验结果.Width = 400;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.ReadOnly = true;
            this.单据号.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbIsRepeat);
            this.groupBox1.Controls.Add(this.txtBatchNo);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtChecker);
            this.groupBox1.Controls.Add(this.cmbGoodsType2);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.chbIsQualified);
            this.groupBox1.Controls.Add(this.cmbGoodsImportance);
            this.groupBox1.Controls.Add(this.cmbGoodsType1);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.dtpCheckDate);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.lbUnit);
            this.groupBox1.Controls.Add(this.numGoodsCount);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtGoodsName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtInspectionExplain);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.txtBillRelate);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtInspectionReportNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(812, 309);
            this.groupBox1.TabIndex = 20;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据";
            // 
            // chbIsRepeat
            // 
            this.chbIsRepeat.AutoSize = true;
            this.chbIsRepeat.Location = new System.Drawing.Point(550, 59);
            this.chbIsRepeat.Name = "chbIsRepeat";
            this.chbIsRepeat.Size = new System.Drawing.Size(72, 16);
            this.chbIsRepeat.TabIndex = 375;
            this.chbIsRepeat.Text = "重复引用";
            this.chbIsRepeat.UseVisualStyleBackColor = true;
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.DataResult = null;
            this.txtBatchNo.EditingControlDataGridView = null;
            this.txtBatchNo.EditingControlFormattedValue = "";
            this.txtBatchNo.EditingControlRowIndex = 0;
            this.txtBatchNo.EditingControlValueChanged = true;
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品批次;
            this.txtBatchNo.Location = new System.Drawing.Point(93, 134);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ShowResultForm = true;
            this.txtBatchNo.Size = new System.Drawing.Size(181, 21);
            this.txtBatchNo.StrEndSql = null;
            this.txtBatchNo.TabIndex = 374;
            this.txtBatchNo.TabStop = false;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsCode.Location = new System.Drawing.Point(93, 94);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(181, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 373;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(93, 22);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(181, 21);
            this.txtBillNo.TabIndex = 371;
            this.txtBillNo.Text = "IRB201411000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(13, 26);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 372;
            this.label16.Text = "业务单号";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(386, 26);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 370;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(288, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 369;
            this.label9.Text = "业务状态";
            // 
            // txtChecker
            // 
            this.txtChecker.DataResult = null;
            this.txtChecker.EditingControlDataGridView = null;
            this.txtChecker.EditingControlFormattedValue = "";
            this.txtChecker.EditingControlRowIndex = 0;
            this.txtChecker.EditingControlValueChanged = true;
            this.txtChecker.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtChecker.Location = new System.Drawing.Point(93, 208);
            this.txtChecker.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtChecker.Name = "txtChecker";
            this.txtChecker.ShowResultForm = true;
            this.txtChecker.Size = new System.Drawing.Size(181, 21);
            this.txtChecker.StrEndSql = null;
            this.txtChecker.TabIndex = 368;
            this.txtChecker.TabStop = false;
            this.txtChecker.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtChecker_OnCompleteSearch);
            this.txtChecker.Enter += new System.EventHandler(this.txtChecker_Enter);
            // 
            // cmbGoodsType2
            // 
            this.cmbGoodsType2.FormattingEnabled = true;
            this.cmbGoodsType2.Items.AddRange(new object[] {
            "样品",
            "试生产",
            "生产"});
            this.cmbGoodsType2.Location = new System.Drawing.Point(351, 171);
            this.cmbGoodsType2.Name = "cmbGoodsType2";
            this.cmbGoodsType2.Size = new System.Drawing.Size(181, 20);
            this.cmbGoodsType2.TabIndex = 367;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(288, 175);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(59, 12);
            this.label15.TabIndex = 366;
            this.label15.Text = "产品状态2";
            // 
            // chbIsQualified
            // 
            this.chbIsQualified.AutoSize = true;
            this.chbIsQualified.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chbIsQualified.Location = new System.Drawing.Point(550, 261);
            this.chbIsQualified.Name = "chbIsQualified";
            this.chbIsQualified.Size = new System.Drawing.Size(61, 20);
            this.chbIsQualified.TabIndex = 365;
            this.chbIsQualified.Text = "合格";
            this.chbIsQualified.UseVisualStyleBackColor = true;
            // 
            // cmbGoodsImportance
            // 
            this.cmbGoodsImportance.FormattingEnabled = true;
            this.cmbGoodsImportance.Items.AddRange(new object[] {
            "关键",
            "重要",
            "一般"});
            this.cmbGoodsImportance.Location = new System.Drawing.Point(619, 171);
            this.cmbGoodsImportance.Name = "cmbGoodsImportance";
            this.cmbGoodsImportance.Size = new System.Drawing.Size(181, 20);
            this.cmbGoodsImportance.TabIndex = 364;
            // 
            // cmbGoodsType1
            // 
            this.cmbGoodsType1.FormattingEnabled = true;
            this.cmbGoodsType1.Items.AddRange(new object[] {
            "新开发",
            "设计变更",
            "工程变更"});
            this.cmbGoodsType1.Location = new System.Drawing.Point(93, 171);
            this.cmbGoodsType1.Name = "cmbGoodsType1";
            this.cmbGoodsType1.Size = new System.Drawing.Size(181, 20);
            this.cmbGoodsType1.TabIndex = 363;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(548, 175);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 359;
            this.label14.Text = "零件重要度";
            // 
            // dtpCheckDate
            // 
            this.dtpCheckDate.Location = new System.Drawing.Point(351, 208);
            this.dtpCheckDate.Name = "dtpCheckDate";
            this.dtpCheckDate.Size = new System.Drawing.Size(181, 21);
            this.dtpCheckDate.TabIndex = 358;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(288, 212);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 357;
            this.label13.Text = "检验日期";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(13, 212);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 355;
            this.label11.Text = "检 验 员";
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUnit.Location = new System.Drawing.Point(771, 137);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(29, 12);
            this.lbUnit.TabIndex = 354;
            this.lbUnit.Text = "单位";
            // 
            // numGoodsCount
            // 
            this.numGoodsCount.DecimalPlaces = 3;
            this.numGoodsCount.Location = new System.Drawing.Point(619, 133);
            this.numGoodsCount.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numGoodsCount.Name = "numGoodsCount";
            this.numGoodsCount.Size = new System.Drawing.Size(146, 21);
            this.numGoodsCount.TabIndex = 353;
            this.numGoodsCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(548, 137);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 352;
            this.label8.Text = "数    量";
            // 
            // txtProvider
            // 
            this.txtProvider.Location = new System.Drawing.Point(351, 133);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(181, 21);
            this.txtProvider.TabIndex = 351;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(288, 137);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 350;
            this.label7.Text = "供 应 商";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(13, 137);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 348;
            this.label6.Text = "批 次 号";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(619, 95);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(181, 21);
            this.txtSpec.TabIndex = 347;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(548, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 346;
            this.label5.Text = "规    格";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Location = new System.Drawing.Point(351, 95);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ReadOnly = true;
            this.txtGoodsName.Size = new System.Drawing.Size(181, 21);
            this.txtGoodsName.TabIndex = 345;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(288, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 344;
            this.label3.Text = "物品名称";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 99);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 342;
            this.label2.Text = "图号型号";
            // 
            // txtInspectionExplain
            // 
            this.txtInspectionExplain.BackColor = System.Drawing.Color.White;
            this.txtInspectionExplain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInspectionExplain.ForeColor = System.Drawing.Color.Black;
            this.txtInspectionExplain.Location = new System.Drawing.Point(93, 246);
            this.txtInspectionExplain.Multiline = true;
            this.txtInspectionExplain.Name = "txtInspectionExplain";
            this.txtInspectionExplain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtInspectionExplain.Size = new System.Drawing.Size(439, 51);
            this.txtInspectionExplain.TabIndex = 341;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(13, 261);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 340;
            this.label10.Text = "检验结论";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 175);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 12);
            this.label1.TabIndex = 336;
            this.label1.Text = "产品状态1";
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.Transparent;
            this.btnSelect.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSelect.Image")));
            this.btnSelect.Location = new System.Drawing.Point(511, 58);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(21, 19);
            this.btnSelect.TabIndex = 335;
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtBillRelate
            // 
            this.txtBillRelate.BackColor = System.Drawing.Color.White;
            this.txtBillRelate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillRelate.ForeColor = System.Drawing.Color.Black;
            this.txtBillRelate.Location = new System.Drawing.Point(351, 57);
            this.txtBillRelate.Name = "txtBillRelate";
            this.txtBillRelate.Size = new System.Drawing.Size(154, 21);
            this.txtBillRelate.TabIndex = 332;
            this.txtBillRelate.TextChanged += new System.EventHandler(this.txtBillRelate_TextChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(288, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 331;
            this.label12.Text = "关联业务";
            // 
            // txtInspectionReportNo
            // 
            this.txtInspectionReportNo.BackColor = System.Drawing.Color.White;
            this.txtInspectionReportNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtInspectionReportNo.ForeColor = System.Drawing.Color.Black;
            this.txtInspectionReportNo.Location = new System.Drawing.Point(93, 57);
            this.txtInspectionReportNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtInspectionReportNo.Name = "txtInspectionReportNo";
            this.txtInspectionReportNo.Size = new System.Drawing.Size(181, 21);
            this.txtInspectionReportNo.TabIndex = 306;
            this.txtInspectionReportNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 307;
            this.label4.Text = "检验报告编号";
            // 
            // 检验报告明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(812, 433);
            this.Controls.Add(this.customPanel1);
            this.Name = "检验报告明细";
            this.Text = "检验单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.contextMenuStrip1.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numGoodsCount)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtBillRelate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtInspectionReportNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.NumericUpDown numGoodsCount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtInspectionExplain;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpCheckDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.CheckBox chbIsQualified;
        private System.Windows.Forms.ComboBox cmbGoodsImportance;
        private System.Windows.Forms.ComboBox cmbGoodsType1;
        private System.Windows.Forms.ComboBox cmbGoodsType2;
        private System.Windows.Forms.Label label15;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private UniversalControlLibrary.TextBoxShow txtChecker;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label9;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 检验项目;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 检验结果;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label16;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private UniversalControlLibrary.TextBoxShow txtBatchNo;
        private System.Windows.Forms.CheckBox chbIsRepeat;
    }
}