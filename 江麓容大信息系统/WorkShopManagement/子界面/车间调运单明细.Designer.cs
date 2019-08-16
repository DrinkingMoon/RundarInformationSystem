using UniversalControlLibrary;
namespace Form_Manufacture_WorkShop
{
    partial class 车间调运单明细
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lbStockCount = new System.Windows.Forms.Label();
            this.lbKCDW = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.txtCode = new UniversalControlLibrary.TextBoxShow();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtListRemark = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.lbHYDW = new System.Windows.Forms.Label();
            this.numOperationCount = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbInWSCode = new System.Windows.Forms.ComboBox();
            this.cmbOutWSCode = new System.Windows.Forms.ComboBox();
            this.txtBillRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAffirmDate = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbAffirm = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbAuditDate = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbAudit = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbProposerDate = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbPropose = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPropose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnAudit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnAffirm = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReback = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOperationCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 379);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(959, 289);
            this.dataGridView1.TabIndex = 78;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lbStockCount);
            this.groupBox2.Controls.Add(this.lbKCDW);
            this.groupBox2.Controls.Add(this.label19);
            this.groupBox2.Controls.Add(this.txtBatchNo);
            this.groupBox2.Controls.Add(this.txtCode);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnModify);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.txtListRemark);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.lbHYDW);
            this.groupBox2.Controls.Add(this.numOperationCount);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtSpec);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 241);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(959, 138);
            this.groupBox2.TabIndex = 77;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "明细信息";
            // 
            // lbStockCount
            // 
            this.lbStockCount.AutoSize = true;
            this.lbStockCount.Location = new System.Drawing.Point(733, 67);
            this.lbStockCount.Name = "lbStockCount";
            this.lbStockCount.Size = new System.Drawing.Size(77, 12);
            this.lbStockCount.TabIndex = 23;
            this.lbStockCount.Text = "lbStockCount";
            // 
            // lbKCDW
            // 
            this.lbKCDW.AutoSize = true;
            this.lbKCDW.Location = new System.Drawing.Point(862, 67);
            this.lbKCDW.Name = "lbKCDW";
            this.lbKCDW.Size = new System.Drawing.Size(29, 12);
            this.lbKCDW.TabIndex = 22;
            this.lbKCDW.Text = "单位";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(666, 67);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 21;
            this.label19.Text = "库存数量";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间耗用物品批次号;
            this.txtBatchNo.Location = new System.Drawing.Point(78, 63);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(205, 21);
            this.txtBatchNo.TabIndex = 20;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // txtCode
            // 
            this.txtCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间耗用物品;
            this.txtCode.Location = new System.Drawing.Point(78, 27);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(205, 21);
            this.txtCode.TabIndex = 19;
            this.txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtCode_OnCompleteSearch);
            this.txtCode.Enter += new System.EventHandler(this.txtCode_Enter);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(864, 100);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 18;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(767, 100);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 17;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(670, 100);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 16;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtListRemark
            // 
            this.txtListRemark.Location = new System.Drawing.Point(78, 101);
            this.txtListRemark.Name = "txtListRemark";
            this.txtListRemark.Size = new System.Drawing.Size(537, 21);
            this.txtListRemark.TabIndex = 15;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(19, 105);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(53, 12);
            this.label18.TabIndex = 14;
            this.label18.Text = "备    注";
            // 
            // lbHYDW
            // 
            this.lbHYDW.AutoSize = true;
            this.lbHYDW.Location = new System.Drawing.Point(586, 67);
            this.lbHYDW.Name = "lbHYDW";
            this.lbHYDW.Size = new System.Drawing.Size(29, 12);
            this.lbHYDW.TabIndex = 13;
            this.lbHYDW.Text = "单位";
            // 
            // numOperationCount
            // 
            this.numOperationCount.Location = new System.Drawing.Point(410, 63);
            this.numOperationCount.Maximum = new decimal(new int[] {
            -159383552,
            46653770,
            5421,
            0});
            this.numOperationCount.Name = "numOperationCount";
            this.numOperationCount.Size = new System.Drawing.Size(170, 21);
            this.numOperationCount.TabIndex = 11;
            this.numOperationCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(339, 67);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 10;
            this.label16.Text = "调运数量";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(19, 67);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 8;
            this.label15.Text = "批 次 号";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(735, 27);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(205, 21);
            this.txtSpec.TabIndex = 7;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(666, 31);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 6;
            this.label14.Text = "规    格";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(410, 27);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(205, 21);
            this.txtName.TabIndex = 5;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(339, 31);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 4;
            this.label13.Text = "物品名称";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "图号型号";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbInWSCode);
            this.groupBox1.Controls.Add(this.cmbOutWSCode);
            this.groupBox1.Controls.Add(this.txtBillRemark);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbAffirmDate);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lbAffirm);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.lbAuditDate);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lbAudit);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbProposerDate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbPropose);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(959, 155);
            this.groupBox1.TabIndex = 76;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主单信息";
            // 
            // cmbInWSCode
            // 
            this.cmbInWSCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInWSCode.FormattingEnabled = true;
            this.cmbInWSCode.Location = new System.Drawing.Point(776, 26);
            this.cmbInWSCode.Name = "cmbInWSCode";
            this.cmbInWSCode.Size = new System.Drawing.Size(163, 20);
            this.cmbInWSCode.TabIndex = 23;
            // 
            // cmbOutWSCode
            // 
            this.cmbOutWSCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOutWSCode.FormattingEnabled = true;
            this.cmbOutWSCode.Location = new System.Drawing.Point(524, 26);
            this.cmbOutWSCode.Name = "cmbOutWSCode";
            this.cmbOutWSCode.Size = new System.Drawing.Size(163, 20);
            this.cmbOutWSCode.TabIndex = 22;
            // 
            // txtBillRemark
            // 
            this.txtBillRemark.Location = new System.Drawing.Point(78, 64);
            this.txtBillRemark.Name = "txtBillRemark";
            this.txtBillRemark.Size = new System.Drawing.Size(862, 21);
            this.txtBillRemark.TabIndex = 21;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(19, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 20;
            this.label3.Text = "备    注";
            // 
            // lbAffirmDate
            // 
            this.lbAffirmDate.AutoSize = true;
            this.lbAffirmDate.ForeColor = System.Drawing.Color.Black;
            this.lbAffirmDate.Location = new System.Drawing.Point(692, 126);
            this.lbAffirmDate.Name = "lbAffirmDate";
            this.lbAffirmDate.Size = new System.Drawing.Size(77, 12);
            this.lbAffirmDate.TabIndex = 19;
            this.lbAffirmDate.Text = "lbAffirmDate";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(624, 126);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 18;
            this.label9.Text = "确认时间";
            // 
            // lbAffirm
            // 
            this.lbAffirm.AutoSize = true;
            this.lbAffirm.ForeColor = System.Drawing.Color.Black;
            this.lbAffirm.Location = new System.Drawing.Point(692, 101);
            this.lbAffirm.Name = "lbAffirm";
            this.lbAffirm.Size = new System.Drawing.Size(53, 12);
            this.lbAffirm.TabIndex = 17;
            this.lbAffirm.Text = "lbAffirm";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(624, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 16;
            this.label12.Text = "确 认 人";
            // 
            // lbAuditDate
            // 
            this.lbAuditDate.AutoSize = true;
            this.lbAuditDate.ForeColor = System.Drawing.Color.Black;
            this.lbAuditDate.Location = new System.Drawing.Point(421, 126);
            this.lbAuditDate.Name = "lbAuditDate";
            this.lbAuditDate.Size = new System.Drawing.Size(71, 12);
            this.lbAuditDate.TabIndex = 15;
            this.lbAuditDate.Text = "lbAuditDate";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(353, 126);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 14;
            this.label8.Text = "审核时间";
            // 
            // lbAudit
            // 
            this.lbAudit.AutoSize = true;
            this.lbAudit.ForeColor = System.Drawing.Color.Black;
            this.lbAudit.Location = new System.Drawing.Point(421, 101);
            this.lbAudit.Name = "lbAudit";
            this.lbAudit.Size = new System.Drawing.Size(47, 12);
            this.lbAudit.TabIndex = 13;
            this.lbAudit.Text = "lbAudit";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(353, 101);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 12;
            this.label10.Text = "审 核 人";
            // 
            // lbProposerDate
            // 
            this.lbProposerDate.AutoSize = true;
            this.lbProposerDate.ForeColor = System.Drawing.Color.Black;
            this.lbProposerDate.Location = new System.Drawing.Point(149, 126);
            this.lbProposerDate.Name = "lbProposerDate";
            this.lbProposerDate.Size = new System.Drawing.Size(89, 12);
            this.lbProposerDate.TabIndex = 11;
            this.lbProposerDate.Text = "lbProposerDate";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(81, 126);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "申请时间";
            // 
            // lbPropose
            // 
            this.lbPropose.AutoSize = true;
            this.lbPropose.ForeColor = System.Drawing.Color.Black;
            this.lbPropose.Location = new System.Drawing.Point(149, 100);
            this.lbPropose.Name = "lbPropose";
            this.lbPropose.Size = new System.Drawing.Size(59, 12);
            this.lbPropose.TabIndex = 9;
            this.lbPropose.Text = "lbPropose";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(81, 101);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "申 请 人";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(454, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 6;
            this.label5.Text = "调出车间";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(706, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "调入车间";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(351, 30);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(77, 12);
            this.lbBillStatus.TabIndex = 3;
            this.lbBillStatus.Text = "lbBillStatus";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(78, 26);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(174, 21);
            this.txtBillNo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "单 据 号";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(959, 61);
            this.panel1.TabIndex = 75;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(379, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "车间调运单明细";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPropose,
            this.toolStripSeparatorAdd,
            this.btnAudit,
            this.toolStripSeparatorDelete,
            this.btnAffirm,
            this.toolStripSeparator4,
            this.btnReback});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(959, 25);
            this.toolStrip1.TabIndex = 74;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPropose
            // 
            this.btnPropose.Image = global::UniversalControlLibrary.Properties.Resources.提交;
            this.btnPropose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPropose.Name = "btnPropose";
            this.btnPropose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPropose.Size = new System.Drawing.Size(91, 22);
            this.btnPropose.Tag = "";
            this.btnPropose.Text = "提交申请(&P)";
            this.btnPropose.Visible = false;
            this.btnPropose.Click += new System.EventHandler(this.btnPropose_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAudit
            // 
            this.btnAudit.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAudit.Name = "btnAudit";
            this.btnAudit.Size = new System.Drawing.Size(91, 22);
            this.btnAudit.Tag = "";
            this.btnAudit.Text = "审核单据(&A)";
            this.btnAudit.Visible = false;
            this.btnAudit.Click += new System.EventHandler(this.btnAudit_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAffirm
            // 
            this.btnAffirm.Image = global::UniversalControlLibrary.Properties.Resources.确定亮;
            this.btnAffirm.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAffirm.Name = "btnAffirm";
            this.btnAffirm.Size = new System.Drawing.Size(103, 22);
            this.btnAffirm.Tag = "";
            this.btnAffirm.Text = "管理员确认(&Q)";
            this.btnAffirm.Visible = false;
            this.btnAffirm.Click += new System.EventHandler(this.btnAffirm_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnReback
            // 
            this.btnReback.Image = global::UniversalControlLibrary.Properties.Resources.回退;
            this.btnReback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReback.Name = "btnReback";
            this.btnReback.Size = new System.Drawing.Size(91, 22);
            this.btnReback.Tag = "view";
            this.btnReback.Text = "回退单据(&R)";
            this.btnReback.Click += new System.EventHandler(this.btnReback_Click);
            // 
            // 车间调运单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(959, 668);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "车间调运单明细";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "车间调运单明细";
            this.Load += new System.EventHandler(this.车间调运单明细_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.车间调运单明细_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOperationCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label lbStockCount;
        private System.Windows.Forms.Label lbKCDW;
        private System.Windows.Forms.Label label19;
        private TextBoxShow txtBatchNo;
        private TextBoxShow txtCode;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtListRemark;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label lbHYDW;
        private System.Windows.Forms.NumericUpDown numOperationCount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbInWSCode;
        private System.Windows.Forms.ComboBox cmbOutWSCode;
        private System.Windows.Forms.TextBox txtBillRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbAffirmDate;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbAffirm;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbAuditDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbAudit;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbProposerDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbPropose;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPropose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnAudit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnAffirm;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnReback;
    }
}