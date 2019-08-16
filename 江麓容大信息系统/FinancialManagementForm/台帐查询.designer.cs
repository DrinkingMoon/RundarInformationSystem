using UniversalControlLibrary;
namespace Form_Economic_Financial
{
    partial class 台帐查询
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(台帐查询));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.台账查询 = new System.Windows.Forms.TabPage();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnFindCode = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbStroage = new System.Windows.Forms.ComboBox();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.label7 = new System.Windows.Forms.Label();
            this.btnOutExcel = new System.Windows.Forms.Button();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnQuery = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.DtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.DtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.tbsGoods = new UniversalControlLibrary.TextBoxShow();
            this.数量收发存 = new System.Windows.Forms.TabPage();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.rbSelectUnpay = new System.Windows.Forms.RadioButton();
            this.rbSelectPay = new System.Windows.Forms.RadioButton();
            this.rbSelectAll = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.rbDG = new System.Windows.Forms.RadioButton();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbZW = new System.Windows.Forms.RadioButton();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.物流账务查询 = new System.Windows.Forms.TabPage();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgvlogistics = new UniversalControlLibrary.CustomDataGridView();
            this.dtp_logistics_startTime = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.dtp_logistics_endTime = new System.Windows.Forms.DateTimePicker();
            this.btn_logistics_Output = new System.Windows.Forms.Button();
            this.btn_logistics_Select = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.cmb_logistics_Storage = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.台账查询.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.数量收发存.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.物流账务查询.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvlogistics)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 53);
            this.panel1.TabIndex = 25;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(444, 13);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "台帐查询";
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
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.台账查询);
            this.tabControl1.Controls.Add(this.数量收发存);
            this.tabControl1.Controls.Add(this.物流账务查询);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 53);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 676);
            this.tabControl1.TabIndex = 26;
            // 
            // 台账查询
            // 
            this.台账查询.Controls.Add(this.dataGridView1);
            this.台账查询.Controls.Add(this.groupBox1);
            this.台账查询.Location = new System.Drawing.Point(4, 23);
            this.台账查询.Name = "台账查询";
            this.台账查询.Padding = new System.Windows.Forms.Padding(3);
            this.台账查询.Size = new System.Drawing.Size(1000, 649);
            this.台账查询.TabIndex = 0;
            this.台账查询.Text = "台账查询";
            this.台账查询.UseVisualStyleBackColor = true;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 161);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(994, 485);
            this.dataGridView1.TabIndex = 29;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.btnFindCode);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmbStroage);
            this.groupBox1.Controls.Add(this.txtBatchNo);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.btnOutExcel);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnQuery);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.DtpEndDate);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.DtpStartDate);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbsGoods);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(994, 158);
            this.groupBox1.TabIndex = 28;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询数据";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(786, 67);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(49, 14);
            this.label9.TabIndex = 191;
            this.label9.Text = "无消息";
            // 
            // btnFindCode
            // 
            this.btnFindCode.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindCode.BackgroundImage")));
            this.btnFindCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode.Location = new System.Drawing.Point(296, 21);
            this.btnFindCode.Name = "btnFindCode";
            this.btnFindCode.Size = new System.Drawing.Size(21, 19);
            this.btnFindCode.TabIndex = 190;
            this.btnFindCode.UseVisualStyleBackColor = false;
            this.btnFindCode.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(17, 67);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 170;
            this.label8.Text = "所属库房";
            // 
            // cmbStroage
            // 
            this.cmbStroage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStroage.FormattingEnabled = true;
            this.cmbStroage.Location = new System.Drawing.Point(98, 64);
            this.cmbStroage.Name = "cmbStroage";
            this.cmbStroage.Size = new System.Drawing.Size(194, 21);
            this.cmbStroage.TabIndex = 169;
            this.cmbStroage.SelectedIndexChanged += new System.EventHandler(this.cmbStroage_SelectedIndexChanged);
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.DataResult = null;
            this.txtBatchNo.DataTableResult = null;
            this.txtBatchNo.EditingControlDataGridView = null;
            this.txtBatchNo.EditingControlFormattedValue = "";
            this.txtBatchNo.EditingControlRowIndex = 0;
            this.txtBatchNo.EditingControlValueChanged = false;
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品批次;
            this.txtBatchNo.IsMultiSelect = false;
            this.txtBatchNo.Location = new System.Drawing.Point(418, 63);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ShowResultForm = true;
            this.txtBatchNo.Size = new System.Drawing.Size(194, 23);
            this.txtBatchNo.StrEndSql = null;
            this.txtBatchNo.TabIndex = 168;
            this.txtBatchNo.TabStop = false;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(347, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 166;
            this.label7.Text = "批 次 号";
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Location = new System.Drawing.Point(817, 108);
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(87, 25);
            this.btnOutExcel.TabIndex = 165;
            this.btnOutExcel.Text = "报表导出";
            this.btnOutExcel.UseVisualStyleBackColor = true;
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(728, 19);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(194, 23);
            this.txtSpec.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(652, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "规    格";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.Location = new System.Drawing.Point(419, 19);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.Size = new System.Drawing.Size(194, 23);
            this.txtGoodsCode.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(347, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "图号型号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(93, 113);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "从";
            // 
            // btnQuery
            // 
            this.btnQuery.Location = new System.Drawing.Point(700, 108);
            this.btnQuery.Name = "btnQuery";
            this.btnQuery.Size = new System.Drawing.Size(87, 25);
            this.btnQuery.TabIndex = 6;
            this.btnQuery.Text = "查询";
            this.btnQuery.UseVisualStyleBackColor = true;
            this.btnQuery.Click += new System.EventHandler(this.btnQuery_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(347, 113);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "到";
            // 
            // DtpEndDate
            // 
            this.DtpEndDate.Location = new System.Drawing.Point(416, 109);
            this.DtpEndDate.Name = "DtpEndDate";
            this.DtpEndDate.Size = new System.Drawing.Size(194, 23);
            this.DtpEndDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 3;
            this.label2.Text = "查询时间";
            // 
            // DtpStartDate
            // 
            this.DtpStartDate.Location = new System.Drawing.Point(114, 109);
            this.DtpStartDate.Name = "DtpStartDate";
            this.DtpStartDate.Size = new System.Drawing.Size(176, 23);
            this.DtpStartDate.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "物品名称";
            // 
            // tbsGoods
            // 
            this.tbsGoods.DataResult = null;
            this.tbsGoods.DataTableResult = null;
            this.tbsGoods.EditingControlDataGridView = null;
            this.tbsGoods.EditingControlFormattedValue = "";
            this.tbsGoods.EditingControlRowIndex = 0;
            this.tbsGoods.EditingControlValueChanged = false;
            this.tbsGoods.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.tbsGoods.IsMultiSelect = false;
            this.tbsGoods.Location = new System.Drawing.Point(99, 19);
            this.tbsGoods.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tbsGoods.Name = "tbsGoods";
            this.tbsGoods.ShowResultForm = true;
            this.tbsGoods.Size = new System.Drawing.Size(194, 23);
            this.tbsGoods.StrEndSql = null;
            this.tbsGoods.TabIndex = 0;
            this.tbsGoods.TabStop = false;
            this.tbsGoods.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsGoods_OnCompleteSearch);
            // 
            // 数量收发存
            // 
            this.数量收发存.Controls.Add(this.customDataGridView1);
            this.数量收发存.Controls.Add(this.groupBox2);
            this.数量收发存.Location = new System.Drawing.Point(4, 23);
            this.数量收发存.Name = "数量收发存";
            this.数量收发存.Padding = new System.Windows.Forms.Padding(3);
            this.数量收发存.Size = new System.Drawing.Size(1000, 649);
            this.数量收发存.TabIndex = 1;
            this.数量收发存.Text = "数量收发存";
            this.数量收发存.UseVisualStyleBackColor = true;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 116);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(994, 530);
            this.customDataGridView1.TabIndex = 2;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.btnOutput);
            this.groupBox2.Controls.Add(this.btnSelect);
            this.groupBox2.Controls.Add(this.cmbMonth);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.cmbYear);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(994, 113);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.rbSelectUnpay);
            this.panel3.Controls.Add(this.rbSelectPay);
            this.panel3.Controls.Add(this.rbSelectAll);
            this.panel3.Location = new System.Drawing.Point(141, 67);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(380, 32);
            this.panel3.TabIndex = 16;
            // 
            // rbSelectUnpay
            // 
            this.rbSelectUnpay.AutoSize = true;
            this.rbSelectUnpay.Location = new System.Drawing.Point(216, 6);
            this.rbSelectUnpay.Name = "rbSelectUnpay";
            this.rbSelectUnpay.Size = new System.Drawing.Size(81, 18);
            this.rbSelectUnpay.TabIndex = 16;
            this.rbSelectUnpay.Text = "仅不付款";
            this.rbSelectUnpay.UseVisualStyleBackColor = true;
            // 
            // rbSelectPay
            // 
            this.rbSelectPay.AutoSize = true;
            this.rbSelectPay.Location = new System.Drawing.Point(96, 7);
            this.rbSelectPay.Name = "rbSelectPay";
            this.rbSelectPay.Size = new System.Drawing.Size(81, 18);
            this.rbSelectPay.TabIndex = 14;
            this.rbSelectPay.Text = "去不付款";
            this.rbSelectPay.UseVisualStyleBackColor = true;
            // 
            // rbSelectAll
            // 
            this.rbSelectAll.AutoSize = true;
            this.rbSelectAll.Checked = true;
            this.rbSelectAll.Location = new System.Drawing.Point(4, 7);
            this.rbSelectAll.Name = "rbSelectAll";
            this.rbSelectAll.Size = new System.Drawing.Size(53, 18);
            this.rbSelectAll.TabIndex = 15;
            this.rbSelectAll.TabStop = true;
            this.rbSelectAll.Text = "全部";
            this.rbSelectAll.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbStorage);
            this.panel2.Controls.Add(this.rbDG);
            this.panel2.Controls.Add(this.rbAll);
            this.panel2.Controls.Add(this.rbZW);
            this.panel2.Location = new System.Drawing.Point(494, 21);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(464, 25);
            this.panel2.TabIndex = 15;
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.Enabled = false;
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(323, 2);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(130, 21);
            this.cmbStorage.TabIndex = 13;
            // 
            // rbDG
            // 
            this.rbDG.AutoSize = true;
            this.rbDG.Location = new System.Drawing.Point(242, 3);
            this.rbDG.Name = "rbDG";
            this.rbDG.Size = new System.Drawing.Size(81, 18);
            this.rbDG.TabIndex = 12;
            this.rbDG.Text = "单个库房";
            this.rbDG.UseVisualStyleBackColor = true;
            this.rbDG.CheckedChanged += new System.EventHandler(this.rbDG_CheckedChanged);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(22, 3);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(81, 18);
            this.rbAll.TabIndex = 11;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "全部库房";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // rbZW
            // 
            this.rbZW.AutoSize = true;
            this.rbZW.Location = new System.Drawing.Point(132, 3);
            this.rbZW.Name = "rbZW";
            this.rbZW.Size = new System.Drawing.Size(81, 18);
            this.rbZW.TabIndex = 10;
            this.rbZW.Text = "账务库房";
            this.rbZW.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(58, 76);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 14;
            this.label13.Text = "查询模式：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(411, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 14);
            this.label12.TabIndex = 10;
            this.label12.Text = "所属库房：";
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(841, 72);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(96, 23);
            this.btnOutput.TabIndex = 5;
            this.btnOutput.Text = "导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(707, 72);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(100, 23);
            this.btnSelect.TabIndex = 4;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // cmbMonth
            // 
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbMonth.Location = new System.Drawing.Point(282, 23);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(40, 21);
            this.cmbMonth.TabIndex = 3;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(229, 26);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(49, 14);
            this.label11.TabIndex = 2;
            this.label11.Text = "月份：";
            // 
            // cmbYear
            // 
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(110, 23);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(80, 21);
            this.cmbYear.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(57, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(49, 14);
            this.label10.TabIndex = 0;
            this.label10.Text = "年份：";
            // 
            // 物流账务查询
            // 
            this.物流账务查询.Controls.Add(this.dgvlogistics);
            this.物流账务查询.Controls.Add(this.customGroupBox1);
            this.物流账务查询.Location = new System.Drawing.Point(4, 23);
            this.物流账务查询.Name = "物流账务查询";
            this.物流账务查询.Size = new System.Drawing.Size(1000, 649);
            this.物流账务查询.TabIndex = 2;
            this.物流账务查询.Text = "物流账务查询";
            this.物流账务查询.UseVisualStyleBackColor = true;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.BackColor = System.Drawing.Color.White;
            this.customGroupBox1.Controls.Add(this.label16);
            this.customGroupBox1.Controls.Add(this.cmb_logistics_Storage);
            this.customGroupBox1.Controls.Add(this.btn_logistics_Output);
            this.customGroupBox1.Controls.Add(this.btn_logistics_Select);
            this.customGroupBox1.Controls.Add(this.label15);
            this.customGroupBox1.Controls.Add(this.dtp_logistics_endTime);
            this.customGroupBox1.Controls.Add(this.label14);
            this.customGroupBox1.Controls.Add(this.dtp_logistics_startTime);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(1000, 73);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "查询条件";
            // 
            // dgvlogistics
            // 
            this.dgvlogistics.AllowUserToAddRows = false;
            this.dgvlogistics.AllowUserToDeleteRows = false;
            this.dgvlogistics.AllowUserToResizeRows = false;
            this.dgvlogistics.AutoCreateFilters = true;
            this.dgvlogistics.BaseFilter = "";
            this.dgvlogistics.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvlogistics.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvlogistics.Location = new System.Drawing.Point(0, 73);
            this.dgvlogistics.Name = "dgvlogistics";
            this.dgvlogistics.RowTemplate.Height = 23;
            this.dgvlogistics.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvlogistics.Size = new System.Drawing.Size(1000, 576);
            this.dgvlogistics.TabIndex = 1;
            // 
            // dtp_logistics_startTime
            // 
            this.dtp_logistics_startTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtp_logistics_startTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_logistics_startTime.Location = new System.Drawing.Point(89, 26);
            this.dtp_logistics_startTime.Name = "dtp_logistics_startTime";
            this.dtp_logistics_startTime.Size = new System.Drawing.Size(180, 23);
            this.dtp_logistics_startTime.TabIndex = 0;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(20, 30);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 14);
            this.label14.TabIndex = 1;
            this.label14.Text = "起始时间";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(281, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 14);
            this.label15.TabIndex = 3;
            this.label15.Text = "截止时间";
            // 
            // dtp_logistics_endTime
            // 
            this.dtp_logistics_endTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtp_logistics_endTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtp_logistics_endTime.Location = new System.Drawing.Point(350, 26);
            this.dtp_logistics_endTime.Name = "dtp_logistics_endTime";
            this.dtp_logistics_endTime.Size = new System.Drawing.Size(180, 23);
            this.dtp_logistics_endTime.TabIndex = 2;
            // 
            // btn_logistics_Output
            // 
            this.btn_logistics_Output.Location = new System.Drawing.Point(912, 26);
            this.btn_logistics_Output.Name = "btn_logistics_Output";
            this.btn_logistics_Output.Size = new System.Drawing.Size(80, 23);
            this.btn_logistics_Output.TabIndex = 7;
            this.btn_logistics_Output.Text = "导出";
            this.btn_logistics_Output.UseVisualStyleBackColor = true;
            this.btn_logistics_Output.Click += new System.EventHandler(this.btn_logistics_Output_Click);
            // 
            // btn_logistics_Select
            // 
            this.btn_logistics_Select.Location = new System.Drawing.Point(807, 26);
            this.btn_logistics_Select.Name = "btn_logistics_Select";
            this.btn_logistics_Select.Size = new System.Drawing.Size(80, 23);
            this.btn_logistics_Select.TabIndex = 6;
            this.btn_logistics_Select.Text = "查询";
            this.btn_logistics_Select.UseVisualStyleBackColor = true;
            this.btn_logistics_Select.Click += new System.EventHandler(this.btn_logistics_Select_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(548, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 14);
            this.label16.TabIndex = 172;
            this.label16.Text = "所属库房";
            // 
            // cmb_logistics_Storage
            // 
            this.cmb_logistics_Storage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmb_logistics_Storage.FormattingEnabled = true;
            this.cmb_logistics_Storage.Location = new System.Drawing.Point(617, 27);
            this.cmb_logistics_Storage.Name = "cmb_logistics_Storage";
            this.cmb_logistics_Storage.Size = new System.Drawing.Size(169, 21);
            this.cmb_logistics_Storage.TabIndex = 171;
            // 
            // 台帐查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "台帐查询";
            this.Resize += new System.EventHandler(this.台帐查询_Resize);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.台账查询.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.数量收发存.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.物流账务查询.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvlogistics)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 台账查询;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnFindCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbStroage;
        private TextBoxShow txtBatchNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnOutExcel;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnQuery;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker DtpEndDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker DtpStartDate;
        private System.Windows.Forms.Label label1;
        private TextBoxShow tbsGoods;
        private System.Windows.Forms.TabPage 数量收发存;
        private System.Windows.Forms.GroupBox groupBox2;
        private CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.RadioButton rbDG;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.RadioButton rbZW;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton rbSelectUnpay;
        private System.Windows.Forms.RadioButton rbSelectPay;
        private System.Windows.Forms.RadioButton rbSelectAll;
        private System.Windows.Forms.TabPage 物流账务查询;
        private CustomDataGridView dgvlogistics;
        private CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Button btn_logistics_Output;
        private System.Windows.Forms.Button btn_logistics_Select;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.DateTimePicker dtp_logistics_endTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtp_logistics_startTime;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox cmb_logistics_Storage;
    }
}
