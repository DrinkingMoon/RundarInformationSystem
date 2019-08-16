using UniversalControlLibrary;
namespace Expression
{
    partial class 产品编码综合查询
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(产品编码综合查询));
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvStockInfo = new System.Windows.Forms.DataGridView();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpRecord = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.dgvERecord = new System.Windows.Forms.DataGridView();
            this.panelLocalizer = new System.Windows.Forms.Panel();
            this.tpBusiness = new System.Windows.Forms.TabPage();
            this.dgvOperationInfo = new System.Windows.Forms.DataGridView();
            this.tpLoadingInfo = new System.Windows.Forms.TabPage();
            this.dgvTruckLoadingInfo = new System.Windows.Forms.DataGridView();
            this.tpCustomerInfo = new System.Windows.Forms.TabPage();
            this.dgvCustomerInfo = new System.Windows.Forms.DataGridView();
            this.tpDeliveryInspection = new System.Windows.Forms.TabPage();
            this.dgvRoutineTest = new System.Windows.Forms.DataGridView();
            this.检测项目 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.技术要求 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.检测情况 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.判定 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tpOffLineTestInfo = new System.Windows.Forms.TabPage();
            this.lbTightnessDate = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbTightness = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbWeighDate = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbWeigh = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbAuditDate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbOffLineTestInfo = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnOperationFind = new System.Windows.Forms.Button();
            this.btnFindPurpose = new System.Windows.Forms.Button();
            this.txtProductCode = new UniversalControlLibrary.TextBoxShow();
            this.btnEXCEL = new System.Windows.Forms.Button();
            this.btnStockCheck = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockInfo)).BeginInit();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tpRecord.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvERecord)).BeginInit();
            this.tpBusiness.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperationInfo)).BeginInit();
            this.tpLoadingInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvTruckLoadingInfo)).BeginInit();
            this.tpCustomerInfo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerInfo)).BeginInit();
            this.tpDeliveryInspection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutineTest)).BeginInit();
            this.tpOffLineTestInfo.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(981, 62);
            this.panel1.TabIndex = 56;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(376, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "产品编码综合查询";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvStockInfo);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(981, 249);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "库存状态";
            // 
            // dgvStockInfo
            // 
            this.dgvStockInfo.AllowUserToAddRows = false;
            this.dgvStockInfo.AllowUserToDeleteRows = false;
            this.dgvStockInfo.AllowUserToResizeRows = false;
            this.dgvStockInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStockInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStockInfo.Location = new System.Drawing.Point(3, 19);
            this.dgvStockInfo.Name = "dgvStockInfo";
            this.dgvStockInfo.ReadOnly = true;
            this.dgvStockInfo.RowHeadersWidth = 21;
            this.dgvStockInfo.RowTemplate.Height = 23;
            this.dgvStockInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStockInfo.Size = new System.Drawing.Size(975, 227);
            this.dgvStockInfo.TabIndex = 0;
            this.dgvStockInfo.DoubleClick += new System.EventHandler(this.dgvStockInfo_DoubleClick);
            this.dgvStockInfo.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvStockInfo_RowPostPaint);
            this.dgvStockInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvStockInfo_DataBindingComplete);
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.panel2);
            this.panel3.Controls.Add(this.splitter1);
            this.panel3.Controls.Add(this.groupBox2);
            this.panel3.Controls.Add(this.groupBox1);
            this.panel3.Controls.Add(this.panel1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(981, 800);
            this.panel3.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.tabControl1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 372);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(981, 428);
            this.panel2.TabIndex = 67;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpRecord);
            this.tabControl1.Controls.Add(this.tpBusiness);
            this.tabControl1.Controls.Add(this.tpLoadingInfo);
            this.tabControl1.Controls.Add(this.tpCustomerInfo);
            this.tabControl1.Controls.Add(this.tpDeliveryInspection);
            this.tabControl1.Controls.Add(this.tpOffLineTestInfo);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(981, 428);
            this.tabControl1.TabIndex = 64;
            // 
            // tpRecord
            // 
            this.tpRecord.Controls.Add(this.splitContainer1);
            this.tpRecord.Location = new System.Drawing.Point(4, 23);
            this.tpRecord.Name = "tpRecord";
            this.tpRecord.Size = new System.Drawing.Size(973, 401);
            this.tpRecord.TabIndex = 4;
            this.tpRecord.Text = "电子档案";
            this.tpRecord.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dgvERecord);
            this.splitContainer1.Panel2.Controls.Add(this.panelLocalizer);
            this.splitContainer1.Size = new System.Drawing.Size(973, 401);
            this.splitContainer1.SplitterDistance = 218;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ItemHeight = 20;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(218, 401);
            this.treeView1.TabIndex = 32;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // dgvERecord
            // 
            this.dgvERecord.AllowUserToAddRows = false;
            this.dgvERecord.AllowUserToDeleteRows = false;
            this.dgvERecord.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvERecord.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvERecord.Location = new System.Drawing.Point(0, 33);
            this.dgvERecord.Name = "dgvERecord";
            this.dgvERecord.ReadOnly = true;
            this.dgvERecord.RowHeadersWidth = 21;
            this.dgvERecord.RowTemplate.Height = 23;
            this.dgvERecord.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvERecord.Size = new System.Drawing.Size(750, 368);
            this.dgvERecord.TabIndex = 170;
            this.dgvERecord.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvERecord_RowPostPaint);
            // 
            // panelLocalizer
            // 
            this.panelLocalizer.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelLocalizer.Location = new System.Drawing.Point(0, 0);
            this.panelLocalizer.Name = "panelLocalizer";
            this.panelLocalizer.Size = new System.Drawing.Size(750, 33);
            this.panelLocalizer.TabIndex = 169;
            // 
            // tpBusiness
            // 
            this.tpBusiness.Controls.Add(this.dgvOperationInfo);
            this.tpBusiness.Location = new System.Drawing.Point(4, 23);
            this.tpBusiness.Name = "tpBusiness";
            this.tpBusiness.Padding = new System.Windows.Forms.Padding(3);
            this.tpBusiness.Size = new System.Drawing.Size(973, 401);
            this.tpBusiness.TabIndex = 0;
            this.tpBusiness.Text = "业务信息";
            this.tpBusiness.UseVisualStyleBackColor = true;
            // 
            // dgvOperationInfo
            // 
            this.dgvOperationInfo.AllowUserToAddRows = false;
            this.dgvOperationInfo.AllowUserToDeleteRows = false;
            this.dgvOperationInfo.AllowUserToResizeRows = false;
            this.dgvOperationInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOperationInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOperationInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvOperationInfo.Name = "dgvOperationInfo";
            this.dgvOperationInfo.ReadOnly = true;
            this.dgvOperationInfo.RowTemplate.Height = 23;
            this.dgvOperationInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOperationInfo.Size = new System.Drawing.Size(967, 395);
            this.dgvOperationInfo.TabIndex = 64;
            this.dgvOperationInfo.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvOperationInfo_RowPostPaint);
            // 
            // tpLoadingInfo
            // 
            this.tpLoadingInfo.Controls.Add(this.dgvTruckLoadingInfo);
            this.tpLoadingInfo.Location = new System.Drawing.Point(4, 23);
            this.tpLoadingInfo.Name = "tpLoadingInfo";
            this.tpLoadingInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpLoadingInfo.Size = new System.Drawing.Size(973, 401);
            this.tpLoadingInfo.TabIndex = 1;
            this.tpLoadingInfo.Text = "装车信息";
            this.tpLoadingInfo.UseVisualStyleBackColor = true;
            // 
            // dgvTruckLoadingInfo
            // 
            this.dgvTruckLoadingInfo.AllowUserToAddRows = false;
            this.dgvTruckLoadingInfo.AllowUserToDeleteRows = false;
            this.dgvTruckLoadingInfo.AllowUserToResizeRows = false;
            this.dgvTruckLoadingInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvTruckLoadingInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvTruckLoadingInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvTruckLoadingInfo.Name = "dgvTruckLoadingInfo";
            this.dgvTruckLoadingInfo.ReadOnly = true;
            this.dgvTruckLoadingInfo.RowTemplate.Height = 23;
            this.dgvTruckLoadingInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvTruckLoadingInfo.Size = new System.Drawing.Size(967, 395);
            this.dgvTruckLoadingInfo.TabIndex = 1;
            // 
            // tpCustomerInfo
            // 
            this.tpCustomerInfo.Controls.Add(this.dgvCustomerInfo);
            this.tpCustomerInfo.Location = new System.Drawing.Point(4, 23);
            this.tpCustomerInfo.Name = "tpCustomerInfo";
            this.tpCustomerInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpCustomerInfo.Size = new System.Drawing.Size(973, 401);
            this.tpCustomerInfo.TabIndex = 2;
            this.tpCustomerInfo.Text = "客户信息";
            this.tpCustomerInfo.UseVisualStyleBackColor = true;
            // 
            // dgvCustomerInfo
            // 
            this.dgvCustomerInfo.AllowUserToAddRows = false;
            this.dgvCustomerInfo.AllowUserToDeleteRows = false;
            this.dgvCustomerInfo.AllowUserToResizeRows = false;
            this.dgvCustomerInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCustomerInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCustomerInfo.Location = new System.Drawing.Point(3, 3);
            this.dgvCustomerInfo.Name = "dgvCustomerInfo";
            this.dgvCustomerInfo.ReadOnly = true;
            this.dgvCustomerInfo.RowTemplate.Height = 23;
            this.dgvCustomerInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvCustomerInfo.Size = new System.Drawing.Size(967, 395);
            this.dgvCustomerInfo.TabIndex = 1;
            // 
            // tpDeliveryInspection
            // 
            this.tpDeliveryInspection.Controls.Add(this.dgvRoutineTest);
            this.tpDeliveryInspection.Location = new System.Drawing.Point(4, 23);
            this.tpDeliveryInspection.Name = "tpDeliveryInspection";
            this.tpDeliveryInspection.Size = new System.Drawing.Size(973, 401);
            this.tpDeliveryInspection.TabIndex = 5;
            this.tpDeliveryInspection.Text = "出厂检验信息";
            this.tpDeliveryInspection.UseVisualStyleBackColor = true;
            // 
            // dgvRoutineTest
            // 
            this.dgvRoutineTest.AllowUserToAddRows = false;
            this.dgvRoutineTest.AllowUserToDeleteRows = false;
            this.dgvRoutineTest.AllowUserToResizeRows = false;
            this.dgvRoutineTest.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRoutineTest.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.检测项目,
            this.技术要求,
            this.检测情况,
            this.判定});
            this.dgvRoutineTest.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRoutineTest.Location = new System.Drawing.Point(0, 0);
            this.dgvRoutineTest.Name = "dgvRoutineTest";
            this.dgvRoutineTest.ReadOnly = true;
            this.dgvRoutineTest.RowTemplate.Height = 23;
            this.dgvRoutineTest.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvRoutineTest.Size = new System.Drawing.Size(973, 401);
            this.dgvRoutineTest.TabIndex = 1;
            this.dgvRoutineTest.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvRoutineTest_RowPostPaint);
            // 
            // 检测项目
            // 
            this.检测项目.DataPropertyName = "检测项目";
            this.检测项目.HeaderText = "检测项目";
            this.检测项目.Name = "检测项目";
            this.检测项目.ReadOnly = true;
            this.检测项目.Width = 200;
            // 
            // 技术要求
            // 
            this.技术要求.DataPropertyName = "技术要求";
            this.技术要求.HeaderText = "技术要求";
            this.技术要求.Name = "技术要求";
            this.技术要求.ReadOnly = true;
            this.技术要求.Width = 600;
            // 
            // 检测情况
            // 
            this.检测情况.DataPropertyName = "检测情况";
            this.检测情况.HeaderText = "检测情况";
            this.检测情况.Name = "检测情况";
            this.检测情况.ReadOnly = true;
            // 
            // 判定
            // 
            this.判定.DataPropertyName = "判定";
            this.判定.HeaderText = "判定";
            this.判定.Name = "判定";
            this.判定.ReadOnly = true;
            // 
            // tpOffLineTestInfo
            // 
            this.tpOffLineTestInfo.Controls.Add(this.lbTightnessDate);
            this.tpOffLineTestInfo.Controls.Add(this.label12);
            this.tpOffLineTestInfo.Controls.Add(this.lbTightness);
            this.tpOffLineTestInfo.Controls.Add(this.label14);
            this.tpOffLineTestInfo.Controls.Add(this.lbWeighDate);
            this.tpOffLineTestInfo.Controls.Add(this.label8);
            this.tpOffLineTestInfo.Controls.Add(this.lbWeigh);
            this.tpOffLineTestInfo.Controls.Add(this.label10);
            this.tpOffLineTestInfo.Controls.Add(this.lbAuditDate);
            this.tpOffLineTestInfo.Controls.Add(this.label6);
            this.tpOffLineTestInfo.Controls.Add(this.lbOffLineTestInfo);
            this.tpOffLineTestInfo.Controls.Add(this.label3);
            this.tpOffLineTestInfo.Location = new System.Drawing.Point(4, 23);
            this.tpOffLineTestInfo.Name = "tpOffLineTestInfo";
            this.tpOffLineTestInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tpOffLineTestInfo.Size = new System.Drawing.Size(973, 401);
            this.tpOffLineTestInfo.TabIndex = 6;
            this.tpOffLineTestInfo.Text = "下线试验信息";
            this.tpOffLineTestInfo.UseVisualStyleBackColor = true;
            // 
            // lbTightnessDate
            // 
            this.lbTightnessDate.AutoSize = true;
            this.lbTightnessDate.Location = new System.Drawing.Point(658, 196);
            this.lbTightnessDate.Name = "lbTightnessDate";
            this.lbTightnessDate.Size = new System.Drawing.Size(63, 14);
            this.lbTightnessDate.TabIndex = 11;
            this.lbTightnessDate.Text = "检测时间";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(540, 196);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 14);
            this.label12.TabIndex = 10;
            this.label12.Text = "检测时间:";
            // 
            // lbTightness
            // 
            this.lbTightness.AutoSize = true;
            this.lbTightness.Location = new System.Drawing.Point(360, 196);
            this.lbTightness.Name = "lbTightness";
            this.lbTightness.Size = new System.Drawing.Size(77, 14);
            this.lbTightness.TabIndex = 9;
            this.lbTightness.Text = "气密性信息";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(256, 196);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(84, 14);
            this.label14.TabIndex = 8;
            this.label14.Text = "气密性信息:";
            // 
            // lbWeighDate
            // 
            this.lbWeighDate.AutoSize = true;
            this.lbWeighDate.Location = new System.Drawing.Point(658, 125);
            this.lbWeighDate.Name = "lbWeighDate";
            this.lbWeighDate.Size = new System.Drawing.Size(63, 14);
            this.lbWeighDate.TabIndex = 7;
            this.lbWeighDate.Text = "称重时间";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(540, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 14);
            this.label8.TabIndex = 6;
            this.label8.Text = "称重时间:";
            // 
            // lbWeigh
            // 
            this.lbWeigh.AutoSize = true;
            this.lbWeigh.Location = new System.Drawing.Point(360, 125);
            this.lbWeigh.Name = "lbWeigh";
            this.lbWeigh.Size = new System.Drawing.Size(63, 14);
            this.lbWeigh.TabIndex = 5;
            this.lbWeigh.Text = "称重信息";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(270, 125);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 14);
            this.label10.TabIndex = 4;
            this.label10.Text = "称重信息:";
            // 
            // lbAuditDate
            // 
            this.lbAuditDate.AutoSize = true;
            this.lbAuditDate.Location = new System.Drawing.Point(658, 60);
            this.lbAuditDate.Name = "lbAuditDate";
            this.lbAuditDate.Size = new System.Drawing.Size(63, 14);
            this.lbAuditDate.TabIndex = 3;
            this.lbAuditDate.Text = "审核时间";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(540, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(70, 14);
            this.label6.TabIndex = 2;
            this.label6.Text = "审核时间:";
            // 
            // lbOffLineTestInfo
            // 
            this.lbOffLineTestInfo.AutoSize = true;
            this.lbOffLineTestInfo.Location = new System.Drawing.Point(360, 60);
            this.lbOffLineTestInfo.Name = "lbOffLineTestInfo";
            this.lbOffLineTestInfo.Size = new System.Drawing.Size(91, 14);
            this.lbOffLineTestInfo.TabIndex = 1;
            this.lbOffLineTestInfo.Text = "下线试验信息";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(242, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 14);
            this.label3.TabIndex = 0;
            this.label3.Text = "下线试验信息:";
            // 
            // splitter1
            // 
            this.splitter1.BackColor = System.Drawing.Color.Silver;
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitter1.Location = new System.Drawing.Point(0, 367);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(981, 5);
            this.splitter1.TabIndex = 66;
            this.splitter1.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnOperationFind);
            this.groupBox1.Controls.Add(this.btnFindPurpose);
            this.groupBox1.Controls.Add(this.txtProductCode);
            this.groupBox1.Controls.Add(this.btnEXCEL);
            this.groupBox1.Controls.Add(this.btnStockCheck);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbStorage);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 62);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(981, 56);
            this.groupBox1.TabIndex = 65;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "查询信息";
            // 
            // btnOperationFind
            // 
            this.btnOperationFind.Location = new System.Drawing.Point(739, 20);
            this.btnOperationFind.Name = "btnOperationFind";
            this.btnOperationFind.Size = new System.Drawing.Size(111, 25);
            this.btnOperationFind.TabIndex = 225;
            this.btnOperationFind.Tag = "view";
            this.btnOperationFind.Text = "编码业务查询";
            this.btnOperationFind.UseVisualStyleBackColor = true;
            this.btnOperationFind.Click += new System.EventHandler(this.btnOperationFind_Click);
            // 
            // btnFindPurpose
            // 
            this.btnFindPurpose.BackColor = System.Drawing.Color.Transparent;
            this.btnFindPurpose.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindPurpose.BackgroundImage")));
            this.btnFindPurpose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindPurpose.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindPurpose.Location = new System.Drawing.Point(567, 21);
            this.btnFindPurpose.Name = "btnFindPurpose";
            this.btnFindPurpose.Size = new System.Drawing.Size(24, 23);
            this.btnFindPurpose.TabIndex = 224;
            this.btnFindPurpose.UseVisualStyleBackColor = false;
            this.btnFindPurpose.Click += new System.EventHandler(this.btnFindPurpose_Click);
            // 
            // txtProductCode
            // 
            this.txtProductCode.DataResult = null;
            this.txtProductCode.DataTableResult = null;
            this.txtProductCode.EditingControlDataGridView = null;
            this.txtProductCode.EditingControlFormattedValue = "";
            this.txtProductCode.EditingControlRowIndex = 0;
            this.txtProductCode.EditingControlValueChanged = false;
            this.txtProductCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.总成编码库存;
            this.txtProductCode.IsMultiSelect = false;
            this.txtProductCode.Location = new System.Drawing.Point(381, 21);
            this.txtProductCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.ShowResultForm = true;
            this.txtProductCode.Size = new System.Drawing.Size(179, 23);
            this.txtProductCode.StrEndSql = null;
            this.txtProductCode.TabIndex = 7;
            this.txtProductCode.TabStop = false;
            this.txtProductCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProductCode_OnCompleteSearch);
            // 
            // btnEXCEL
            // 
            this.btnEXCEL.Location = new System.Drawing.Point(873, 20);
            this.btnEXCEL.Name = "btnEXCEL";
            this.btnEXCEL.Size = new System.Drawing.Size(87, 25);
            this.btnEXCEL.TabIndex = 6;
            this.btnEXCEL.Tag = "view";
            this.btnEXCEL.Text = "导出EXCEL";
            this.btnEXCEL.UseVisualStyleBackColor = true;
            this.btnEXCEL.Click += new System.EventHandler(this.btnEXCEL_Click);
            // 
            // btnStockCheck
            // 
            this.btnStockCheck.Location = new System.Drawing.Point(629, 20);
            this.btnStockCheck.Name = "btnStockCheck";
            this.btnStockCheck.Size = new System.Drawing.Size(87, 25);
            this.btnStockCheck.TabIndex = 4;
            this.btnStockCheck.Tag = "view";
            this.btnStockCheck.Text = "查  询";
            this.btnStockCheck.UseVisualStyleBackColor = true;
            this.btnStockCheck.Click += new System.EventHandler(this.btnStockCheck_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(312, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "箱体编号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "库房名称";
            // 
            // cmbStorage
            // 
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(110, 22);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(179, 21);
            this.cmbStorage.TabIndex = 0;
            this.cmbStorage.TextChanged += new System.EventHandler(this.cmbStorage_TextChanged);
            // 
            // 产品编码综合查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(998, 570);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "产品编码综合查询";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStockInfo)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tpRecord.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvERecord)).EndInit();
            this.tpBusiness.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOperationInfo)).EndInit();
            this.tpLoadingInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvTruckLoadingInfo)).EndInit();
            this.tpCustomerInfo.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomerInfo)).EndInit();
            this.tpDeliveryInspection.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRoutineTest)).EndInit();
            this.tpOffLineTestInfo.ResumeLayout(false);
            this.tpOffLineTestInfo.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvStockInfo;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tpRecord;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.DataGridView dgvERecord;
        private System.Windows.Forms.Panel panelLocalizer;
        private System.Windows.Forms.TabPage tpBusiness;
        private System.Windows.Forms.DataGridView dgvOperationInfo;
        private System.Windows.Forms.TabPage tpLoadingInfo;
        private System.Windows.Forms.DataGridView dgvTruckLoadingInfo;
        private System.Windows.Forms.TabPage tpCustomerInfo;
        private System.Windows.Forms.DataGridView dgvCustomerInfo;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnEXCEL;
        private System.Windows.Forms.Button btnStockCheck;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStorage;
        private TextBoxShow txtProductCode;
        private System.Windows.Forms.TabPage tpDeliveryInspection;
        private System.Windows.Forms.DataGridView dgvRoutineTest;
        private System.Windows.Forms.Button btnFindPurpose;
        private System.Windows.Forms.Button btnOperationFind;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检测项目;
        private System.Windows.Forms.DataGridViewTextBoxColumn 技术要求;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检测情况;
        private System.Windows.Forms.DataGridViewTextBoxColumn 判定;
        private System.Windows.Forms.TabPage tpOffLineTestInfo;
        private System.Windows.Forms.Label lbOffLineTestInfo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbTightnessDate;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbTightness;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbWeighDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbWeigh;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbAuditDate;
        private System.Windows.Forms.Label label6;
    }
}
