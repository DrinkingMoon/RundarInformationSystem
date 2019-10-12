namespace Form_Project_Design
{
    partial class TCU软件升级明细
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.txtCarModelNo = new UniversalControlLibrary.TextBoxShow();
            this.txtTechnicalNote = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnDownload_TCUSoft = new System.Windows.Forms.Button();
            this.btnUpload_TCUSoft = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtUpdateContent = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtUpdateReason = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDiagnosisVersion = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUnderVersion = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtHardwareVersion = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtClientVersion = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.llbCarModelInfo = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.DID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.字节数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.内容 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.numDataSize = new System.Windows.Forms.NumericUpDown();
            this.btnDelete_DID = new System.Windows.Forms.Button();
            this.btnAdd_DID = new System.Windows.Forms.Button();
            this.txtDataContent = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtDID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.rbNo_Pass = new System.Windows.Forms.RadioButton();
            this.rbYes_Pass = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.btnDownload_TestReport = new System.Windows.Forms.Button();
            this.btnUpload_TestReport = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.txtTestResult = new System.Windows.Forms.TextBox();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.cmbVersionType = new System.Windows.Forms.ComboBox();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataSize)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.customDataGridView1);
            this.splitContainer1.Panel2.Controls.Add(this.groupBox2);
            this.splitContainer1.Size = new System.Drawing.Size(872, 485);
            this.splitContainer1.SplitterDistance = 640;
            this.splitContainer1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbVersionType);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.txtCarModelNo);
            this.groupBox1.Controls.Add(this.txtTechnicalNote);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnDownload_TCUSoft);
            this.groupBox1.Controls.Add(this.btnUpload_TCUSoft);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtUpdateContent);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtUpdateReason);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtDiagnosisVersion);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtUnderVersion);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtHardwareVersion);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtClientVersion);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label69);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label68);
            this.groupBox1.Controls.Add(this.llbCarModelInfo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(640, 485);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "软件升级内容";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(165, 98);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(72, 21);
            this.txtVersion.TabIndex = 432;
            // 
            // txtCarModelNo
            // 
            this.txtCarModelNo.DataResult = null;
            this.txtCarModelNo.DataTableResult = null;
            this.txtCarModelNo.EditingControlDataGridView = null;
            this.txtCarModelNo.EditingControlFormattedValue = "";
            this.txtCarModelNo.EditingControlRowIndex = 0;
            this.txtCarModelNo.EditingControlValueChanged = true;
            this.txtCarModelNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车型代号;
            this.txtCarModelNo.IsMultiSelect = false;
            this.txtCarModelNo.Location = new System.Drawing.Point(126, 59);
            this.txtCarModelNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCarModelNo.Name = "txtCarModelNo";
            this.txtCarModelNo.ShowResultForm = true;
            this.txtCarModelNo.Size = new System.Drawing.Size(179, 21);
            this.txtCarModelNo.StrEndSql = null;
            this.txtCarModelNo.TabIndex = 431;
            this.txtCarModelNo.TabStop = false;
            this.txtCarModelNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtTechnicalNote
            // 
            this.txtTechnicalNote.Location = new System.Drawing.Point(126, 209);
            this.txtTechnicalNote.Name = "txtTechnicalNote";
            this.txtTechnicalNote.Size = new System.Drawing.Size(486, 21);
            this.txtTechnicalNote.TabIndex = 428;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(29, 212);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(89, 12);
            this.label13.TabIndex = 427;
            this.label13.Text = "技术通知单编号";
            // 
            // btnDownload_TCUSoft
            // 
            this.btnDownload_TCUSoft.Location = new System.Drawing.Point(520, 96);
            this.btnDownload_TCUSoft.Name = "btnDownload_TCUSoft";
            this.btnDownload_TCUSoft.Size = new System.Drawing.Size(75, 23);
            this.btnDownload_TCUSoft.TabIndex = 426;
            this.btnDownload_TCUSoft.Text = "下载";
            this.btnDownload_TCUSoft.UseVisualStyleBackColor = true;
            this.btnDownload_TCUSoft.Click += new System.EventHandler(this.btnDownload_TCUSoft_Click);
            // 
            // btnUpload_TCUSoft
            // 
            this.btnUpload_TCUSoft.Location = new System.Drawing.Point(418, 96);
            this.btnUpload_TCUSoft.Name = "btnUpload_TCUSoft";
            this.btnUpload_TCUSoft.Size = new System.Drawing.Size(75, 23);
            this.btnUpload_TCUSoft.TabIndex = 425;
            this.btnUpload_TCUSoft.Text = "上传";
            this.btnUpload_TCUSoft.UseVisualStyleBackColor = true;
            this.btnUpload_TCUSoft.Click += new System.EventHandler(this.btnUpload_TCUSoft_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(329, 101);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(47, 12);
            this.label12.TabIndex = 424;
            this.label12.Text = "TCU软件";
            // 
            // txtUpdateContent
            // 
            this.txtUpdateContent.Location = new System.Drawing.Point(126, 369);
            this.txtUpdateContent.Multiline = true;
            this.txtUpdateContent.Name = "txtUpdateContent";
            this.txtUpdateContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUpdateContent.Size = new System.Drawing.Size(486, 100);
            this.txtUpdateContent.TabIndex = 423;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(29, 413);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 12);
            this.label11.TabIndex = 422;
            this.label11.Text = "升级【内容】";
            // 
            // txtUpdateReason
            // 
            this.txtUpdateReason.Location = new System.Drawing.Point(126, 248);
            this.txtUpdateReason.Multiline = true;
            this.txtUpdateReason.Name = "txtUpdateReason";
            this.txtUpdateReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUpdateReason.Size = new System.Drawing.Size(486, 100);
            this.txtUpdateReason.TabIndex = 421;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(29, 292);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 420;
            this.label10.Text = "升级【原因】";
            // 
            // txtDiagnosisVersion
            // 
            this.txtDiagnosisVersion.Location = new System.Drawing.Point(418, 173);
            this.txtDiagnosisVersion.Name = "txtDiagnosisVersion";
            this.txtDiagnosisVersion.Size = new System.Drawing.Size(194, 21);
            this.txtDiagnosisVersion.TabIndex = 419;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(329, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 418;
            this.label9.Text = "诊断软件版本";
            // 
            // txtUnderVersion
            // 
            this.txtUnderVersion.Location = new System.Drawing.Point(126, 135);
            this.txtUnderVersion.Name = "txtUnderVersion";
            this.txtUnderVersion.Size = new System.Drawing.Size(179, 21);
            this.txtUnderVersion.TabIndex = 417;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(29, 139);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(95, 12);
            this.label8.TabIndex = 416;
            this.label8.Text = "TCU底层软件版本";
            // 
            // txtHardwareVersion
            // 
            this.txtHardwareVersion.Location = new System.Drawing.Point(417, 135);
            this.txtHardwareVersion.Name = "txtHardwareVersion";
            this.txtHardwareVersion.Size = new System.Drawing.Size(195, 21);
            this.txtHardwareVersion.TabIndex = 415;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(329, 138);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(71, 12);
            this.label7.TabIndex = 414;
            this.label7.Text = "TCU硬件版本";
            // 
            // txtClientVersion
            // 
            this.txtClientVersion.Location = new System.Drawing.Point(126, 173);
            this.txtClientVersion.Name = "txtClientVersion";
            this.txtClientVersion.Size = new System.Drawing.Size(179, 21);
            this.txtClientVersion.TabIndex = 413;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(29, 176);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 412;
            this.label6.Text = "客户软件版本";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(29, 101);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 410;
            this.label5.Text = "TCU软件版本";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(435, 27);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(53, 12);
            this.lbBillStatus.TabIndex = 409;
            this.lbBillStatus.Text = "新建单据";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(329, 27);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(53, 12);
            this.label69.TabIndex = 408;
            this.label69.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(126, 23);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(179, 21);
            this.txtBillNo.TabIndex = 407;
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(29, 27);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(53, 12);
            this.label68.TabIndex = 406;
            this.label68.Text = "单 据 号";
            // 
            // llbCarModelInfo
            // 
            this.llbCarModelInfo.AutoSize = true;
            this.llbCarModelInfo.Location = new System.Drawing.Point(329, 63);
            this.llbCarModelInfo.Name = "llbCarModelInfo";
            this.llbCarModelInfo.Size = new System.Drawing.Size(101, 12);
            this.llbCarModelInfo.TabIndex = 6;
            this.llbCarModelInfo.TabStop = true;
            this.llbCarModelInfo.Text = "查看车型详细信息";
            this.llbCarModelInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbCarModelInfo_LinkClicked);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 4;
            this.label2.Text = "车型代号";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DID,
            this.字节数,
            this.内容});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 159);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowHeadersWidth = 20;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(228, 326);
            this.customDataGridView1.TabIndex = 2;
            // 
            // DID
            // 
            this.DID.DataPropertyName = "DID";
            this.DID.HeaderText = "DID";
            this.DID.Name = "DID";
            this.DID.ReadOnly = true;
            this.DID.Width = 40;
            // 
            // 字节数
            // 
            this.字节数.DataPropertyName = "字节数";
            this.字节数.HeaderText = "字节数";
            this.字节数.Name = "字节数";
            this.字节数.ReadOnly = true;
            this.字节数.Width = 70;
            // 
            // 内容
            // 
            this.内容.DataPropertyName = "内容";
            this.内容.HeaderText = "内容";
            this.内容.Name = "内容";
            this.内容.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.numDataSize);
            this.groupBox2.Controls.Add(this.btnDelete_DID);
            this.groupBox2.Controls.Add(this.btnAdd_DID);
            this.groupBox2.Controls.Add(this.txtDataContent);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtDID);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(228, 159);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "DID内容";
            // 
            // numDataSize
            // 
            this.numDataSize.Location = new System.Drawing.Point(64, 54);
            this.numDataSize.Name = "numDataSize";
            this.numDataSize.Size = new System.Drawing.Size(148, 21);
            this.numDataSize.TabIndex = 430;
            this.numDataSize.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // btnDelete_DID
            // 
            this.btnDelete_DID.Location = new System.Drawing.Point(136, 121);
            this.btnDelete_DID.Name = "btnDelete_DID";
            this.btnDelete_DID.Size = new System.Drawing.Size(75, 23);
            this.btnDelete_DID.TabIndex = 13;
            this.btnDelete_DID.Text = "删除";
            this.btnDelete_DID.UseVisualStyleBackColor = true;
            this.btnDelete_DID.Click += new System.EventHandler(this.btnDelete_DID_Click);
            // 
            // btnAdd_DID
            // 
            this.btnAdd_DID.Location = new System.Drawing.Point(33, 121);
            this.btnAdd_DID.Name = "btnAdd_DID";
            this.btnAdd_DID.Size = new System.Drawing.Size(75, 23);
            this.btnAdd_DID.TabIndex = 12;
            this.btnAdd_DID.Text = "添加";
            this.btnAdd_DID.UseVisualStyleBackColor = true;
            this.btnAdd_DID.Click += new System.EventHandler(this.btnAdd_DID_Click);
            // 
            // txtDataContent
            // 
            this.txtDataContent.Location = new System.Drawing.Point(64, 85);
            this.txtDataContent.Name = "txtDataContent";
            this.txtDataContent.Size = new System.Drawing.Size(148, 21);
            this.txtDataContent.TabIndex = 11;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 89);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "内  容：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "字节数：";
            // 
            // txtDID
            // 
            this.txtDID.Location = new System.Drawing.Point(64, 22);
            this.txtDID.Name = "txtDID";
            this.txtDID.Size = new System.Drawing.Size(148, 21);
            this.txtDID.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "D I D ：";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(18, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 424;
            this.label14.Text = "测试结果";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.rbNo_Pass);
            this.groupBox3.Controls.Add(this.rbYes_Pass);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.btnDownload_TestReport);
            this.groupBox3.Controls.Add(this.btnUpload_TestReport);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtTestResult);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 485);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(872, 126);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "测试内容";
            // 
            // rbNo_Pass
            // 
            this.rbNo_Pass.AutoSize = true;
            this.rbNo_Pass.Location = new System.Drawing.Point(794, 83);
            this.rbNo_Pass.Name = "rbNo_Pass";
            this.rbNo_Pass.Size = new System.Drawing.Size(59, 16);
            this.rbNo_Pass.TabIndex = 431;
            this.rbNo_Pass.TabStop = true;
            this.rbNo_Pass.Text = "不通过";
            this.rbNo_Pass.UseVisualStyleBackColor = true;
            // 
            // rbYes_Pass
            // 
            this.rbYes_Pass.AutoSize = true;
            this.rbYes_Pass.Location = new System.Drawing.Point(718, 83);
            this.rbYes_Pass.Name = "rbYes_Pass";
            this.rbYes_Pass.Size = new System.Drawing.Size(47, 16);
            this.rbYes_Pass.TabIndex = 430;
            this.rbYes_Pass.TabStop = true;
            this.rbYes_Pass.Text = "通过";
            this.rbYes_Pass.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(642, 85);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 429;
            this.label16.Text = "测试结论";
            // 
            // btnDownload_TestReport
            // 
            this.btnDownload_TestReport.Location = new System.Drawing.Point(798, 32);
            this.btnDownload_TestReport.Name = "btnDownload_TestReport";
            this.btnDownload_TestReport.Size = new System.Drawing.Size(56, 23);
            this.btnDownload_TestReport.TabIndex = 428;
            this.btnDownload_TestReport.Text = "下载";
            this.btnDownload_TestReport.UseVisualStyleBackColor = true;
            this.btnDownload_TestReport.Click += new System.EventHandler(this.btnDownload_TestReport_Click);
            // 
            // btnUpload_TestReport
            // 
            this.btnUpload_TestReport.Location = new System.Drawing.Point(714, 32);
            this.btnUpload_TestReport.Name = "btnUpload_TestReport";
            this.btnUpload_TestReport.Size = new System.Drawing.Size(56, 23);
            this.btnUpload_TestReport.TabIndex = 427;
            this.btnUpload_TestReport.Text = "上传";
            this.btnUpload_TestReport.UseVisualStyleBackColor = true;
            this.btnUpload_TestReport.Click += new System.EventHandler(this.btnUpload_TestReport_Click);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(642, 37);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 426;
            this.label15.Text = "测试报告";
            // 
            // txtTestResult
            // 
            this.txtTestResult.Location = new System.Drawing.Point(85, 28);
            this.txtTestResult.Multiline = true;
            this.txtTestResult.Name = "txtTestResult";
            this.txtTestResult.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTestResult.Size = new System.Drawing.Size(545, 81);
            this.txtTestResult.TabIndex = 425;
            // 
            // cmbVersionType
            // 
            this.cmbVersionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbVersionType.FormattingEnabled = true;
            this.cmbVersionType.Items.AddRange(new object[] {
            "V",
            "S"});
            this.cmbVersionType.Location = new System.Drawing.Point(126, 98);
            this.cmbVersionType.Name = "cmbVersionType";
            this.cmbVersionType.Size = new System.Drawing.Size(33, 20);
            this.cmbVersionType.TabIndex = 433;
            // 
            // TCU软件升级明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(872, 611);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.splitContainer1);
            this.Name = "TCU软件升级明细";
            this.Text = "TCU软件升级流程明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.TCU软件升级明细_PanelGetDataInfo);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDataSize)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Button btnDelete_DID;
        private System.Windows.Forms.Button btnAdd_DID;
        private System.Windows.Forms.TextBox txtDataContent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtDID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtTechnicalNote;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnDownload_TCUSoft;
        private System.Windows.Forms.Button btnUpload_TCUSoft;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtUpdateContent;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtUpdateReason;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtDiagnosisVersion;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUnderVersion;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtHardwareVersion;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtClientVersion;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.LinkLabel llbCarModelInfo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.RadioButton rbNo_Pass;
        private System.Windows.Forms.RadioButton rbYes_Pass;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button btnDownload_TestReport;
        private System.Windows.Forms.Button btnUpload_TestReport;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtTestResult;
        private System.Windows.Forms.NumericUpDown numDataSize;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private UniversalControlLibrary.TextBoxShow txtCarModelNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 字节数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 内容;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.ComboBox cmbVersionType;
    }
}