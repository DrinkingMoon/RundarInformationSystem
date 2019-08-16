using UniversalControlLibrary;

namespace Form_Peripheral_HR
{
    partial class FormShortMsgManagement
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chk已发送 = new System.Windows.Forms.CheckBox();
            this.chk待发送 = new System.Windows.Forms.CheckBox();
            this.chk所有 = new System.Windows.Forms.CheckBox();
            this.chk暂存 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerST = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerET = new System.Windows.Forms.DateTimePicker();
            this.groupBoxCreator = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtMobileNo = new System.Windows.Forms.TextBox();
            this.dtpkSendTime = new System.Windows.Forms.DateTimePicker();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtReceiver = new System.Windows.Forms.TextBox();
            this.txtDeclarePersonnel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpkCreateTime = new System.Windows.Forms.DateTimePicker();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBillStatus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.btnRefreshData = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnPublish = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.btn撤销 = new System.Windows.Forms.ToolStripButton();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbShortMsgType = new System.Windows.Forms.ComboBox();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelPara.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBoxCreator.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1016, 709);
            this.panelMain.TabIndex = 43;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Controls.Add(this.panelPara);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(28, 50);
            this.panelCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(960, 659);
            this.panelCenter.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 255);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 46;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(960, 395);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 650);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(960, 9);
            this.panel2.TabIndex = 27;
            // 
            // panelPara
            // 
            this.panelPara.BackColor = System.Drawing.SystemColors.Control;
            this.panelPara.Controls.Add(this.userControlDataLocalizer1);
            this.panelPara.Controls.Add(this.groupBox2);
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Controls.Add(this.groupBoxCreator);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 0);
            this.panelPara.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(960, 255);
            this.panelPara.TabIndex = 16;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chk已发送);
            this.groupBox2.Controls.Add(this.chk待发送);
            this.groupBox2.Controls.Add(this.chk所有);
            this.groupBox2.Controls.Add(this.chk暂存);
            this.groupBox2.Location = new System.Drawing.Point(509, 138);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 82);
            this.groupBox2.TabIndex = 158;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "显示控制";
            // 
            // chk已发送
            // 
            this.chk已发送.AutoSize = true;
            this.chk已发送.Location = new System.Drawing.Point(6, 51);
            this.chk已发送.Name = "chk已发送";
            this.chk已发送.Size = new System.Drawing.Size(96, 18);
            this.chk已发送.TabIndex = 4;
            this.chk已发送.Text = "已发送短信";
            this.chk已发送.UseVisualStyleBackColor = true;
            // 
            // chk待发送
            // 
            this.chk待发送.AutoSize = true;
            this.chk待发送.Location = new System.Drawing.Point(108, 18);
            this.chk待发送.Name = "chk待发送";
            this.chk待发送.Size = new System.Drawing.Size(110, 18);
            this.chk待发送.TabIndex = 1;
            this.chk待发送.Text = "待发送的短信";
            this.chk待发送.UseVisualStyleBackColor = true;
            this.chk待发送.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // chk所有
            // 
            this.chk所有.AutoSize = true;
            this.chk所有.Checked = true;
            this.chk所有.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chk所有.Location = new System.Drawing.Point(108, 51);
            this.chk所有.Name = "chk所有";
            this.chk所有.Size = new System.Drawing.Size(82, 18);
            this.chk所有.TabIndex = 3;
            this.chk所有.Text = "所有短信";
            this.chk所有.UseVisualStyleBackColor = true;
            this.chk所有.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // chk暂存
            // 
            this.chk暂存.AutoSize = true;
            this.chk暂存.Location = new System.Drawing.Point(6, 18);
            this.chk暂存.Name = "chk暂存";
            this.chk暂存.Size = new System.Drawing.Size(82, 18);
            this.chk暂存.TabIndex = 0;
            this.chk暂存.Text = "暂存短信";
            this.chk暂存.UseVisualStyleBackColor = true;
            this.chk暂存.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePickerST);
            this.groupBox1.Controls.Add(this.dateTimePickerET);
            this.groupBox1.Location = new System.Drawing.Point(8, 138);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 82);
            this.groupBox1.TabIndex = 157;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "祝福时间范围";
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(271, 24);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(148, 50);
            this.btnSearch.TabIndex = 2;
            this.btnSearch.Text = "检索数据";
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(16, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 2;
            this.label10.Text = "结束日期";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(16, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "起始日期";
            // 
            // dateTimePickerST
            // 
            this.dateTimePickerST.Location = new System.Drawing.Point(86, 24);
            this.dateTimePickerST.Name = "dateTimePickerST";
            this.dateTimePickerST.Size = new System.Drawing.Size(151, 23);
            this.dateTimePickerST.TabIndex = 0;
            // 
            // dateTimePickerET
            // 
            this.dateTimePickerET.Location = new System.Drawing.Point(86, 51);
            this.dateTimePickerET.Name = "dateTimePickerET";
            this.dateTimePickerET.Size = new System.Drawing.Size(151, 23);
            this.dateTimePickerET.TabIndex = 1;
            // 
            // groupBoxCreator
            // 
            this.groupBoxCreator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxCreator.Controls.Add(this.cmbShortMsgType);
            this.groupBoxCreator.Controls.Add(this.label8);
            this.groupBoxCreator.Controls.Add(this.label7);
            this.groupBoxCreator.Controls.Add(this.txtRemark);
            this.groupBoxCreator.Controls.Add(this.label3);
            this.groupBoxCreator.Controls.Add(this.txtMobileNo);
            this.groupBoxCreator.Controls.Add(this.dtpkSendTime);
            this.groupBoxCreator.Controls.Add(this.txtContent);
            this.groupBoxCreator.Controls.Add(this.label4);
            this.groupBoxCreator.Controls.Add(this.label9);
            this.groupBoxCreator.Controls.Add(this.label12);
            this.groupBoxCreator.Controls.Add(this.txtReceiver);
            this.groupBoxCreator.Controls.Add(this.txtDeclarePersonnel);
            this.groupBoxCreator.Controls.Add(this.label2);
            this.groupBoxCreator.Controls.Add(this.txtID);
            this.groupBoxCreator.Controls.Add(this.label6);
            this.groupBoxCreator.Controls.Add(this.label5);
            this.groupBoxCreator.Controls.Add(this.dtpkCreateTime);
            this.groupBoxCreator.Location = new System.Drawing.Point(8, 5);
            this.groupBoxCreator.Name = "groupBoxCreator";
            this.groupBoxCreator.Size = new System.Drawing.Size(945, 127);
            this.groupBoxCreator.TabIndex = 156;
            this.groupBoxCreator.TabStop = false;
            this.groupBoxCreator.Text = "信息预览";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(224, 94);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 14);
            this.label7.TabIndex = 111;
            this.label7.Text = "备注";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtRemark.Location = new System.Drawing.Point(293, 91);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ReadOnly = true;
            this.txtRemark.Size = new System.Drawing.Size(184, 23);
            this.txtRemark.TabIndex = 110;
            // 
            // label3
            // 
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(224, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 35);
            this.label3.TabIndex = 109;
            this.label3.Text = "接收人电话";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtMobileNo
            // 
            this.txtMobileNo.BackColor = System.Drawing.Color.White;
            this.txtMobileNo.Location = new System.Drawing.Point(293, 52);
            this.txtMobileNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMobileNo.Name = "txtMobileNo";
            this.txtMobileNo.Size = new System.Drawing.Size(184, 23);
            this.txtMobileNo.TabIndex = 108;
            // 
            // dtpkSendTime
            // 
            this.dtpkSendTime.Checked = false;
            this.dtpkSendTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpkSendTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpkSendTime.Location = new System.Drawing.Point(733, 14);
            this.dtpkSendTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpkSendTime.Name = "dtpkSendTime";
            this.dtpkSendTime.ShowCheckBox = true;
            this.dtpkSendTime.Size = new System.Drawing.Size(206, 23);
            this.dtpkSendTime.TabIndex = 3;
            // 
            // txtContent
            // 
            this.txtContent.BackColor = System.Drawing.Color.White;
            this.txtContent.ForeColor = System.Drawing.Color.Black;
            this.txtContent.Location = new System.Drawing.Point(565, 46);
            this.txtContent.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(380, 76);
            this.txtContent.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(496, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 104;
            this.label4.Text = "短信内容";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(664, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 107;
            this.label9.Text = "发送时间";
            // 
            // label12
            // 
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(17, 45);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(51, 35);
            this.label12.TabIndex = 103;
            this.label12.Text = "接收人姓名";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtReceiver
            // 
            this.txtReceiver.BackColor = System.Drawing.Color.White;
            this.txtReceiver.Location = new System.Drawing.Point(86, 52);
            this.txtReceiver.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtReceiver.Name = "txtReceiver";
            this.txtReceiver.Size = new System.Drawing.Size(121, 23);
            this.txtReceiver.TabIndex = 102;
            // 
            // txtDeclarePersonnel
            // 
            this.txtDeclarePersonnel.BackColor = System.Drawing.Color.White;
            this.txtDeclarePersonnel.ForeColor = System.Drawing.Color.Black;
            this.txtDeclarePersonnel.Location = new System.Drawing.Point(565, 14);
            this.txtDeclarePersonnel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDeclarePersonnel.Name = "txtDeclarePersonnel";
            this.txtDeclarePersonnel.ReadOnly = true;
            this.txtDeclarePersonnel.Size = new System.Drawing.Size(93, 23);
            this.txtDeclarePersonnel.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(17, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "短信编号";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.ForeColor = System.Drawing.Color.Red;
            this.txtID.Location = new System.Drawing.Point(86, 14);
            this.txtID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(121, 23);
            this.txtID.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(496, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 96;
            this.label6.Text = "编 制 人";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(224, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 38;
            this.label5.Text = "编制时间";
            // 
            // dtpkCreateTime
            // 
            this.dtpkCreateTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpkCreateTime.Enabled = false;
            this.dtpkCreateTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpkCreateTime.Location = new System.Drawing.Point(293, 14);
            this.dtpkCreateTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpkCreateTime.Name = "dtpkCreateTime";
            this.dtpkCreateTime.Size = new System.Drawing.Size(184, 23);
            this.dtpkCreateTime.TabIndex = 1;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(988, 50);
            this.panelRight.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(28, 659);
            this.panelRight.TabIndex = 39;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 50);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(28, 659);
            this.panelLeft.TabIndex = 38;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.lblBillStatus);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 50);
            this.panel1.TabIndex = 24;
            // 
            // lblBillStatus
            // 
            this.lblBillStatus.AutoSize = true;
            this.lblBillStatus.Location = new System.Drawing.Point(126, 18);
            this.lblBillStatus.Name = "lblBillStatus";
            this.lblBillStatus.Size = new System.Drawing.Size(0, 14);
            this.lblBillStatus.TabIndex = 191;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(43, 18);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(77, 14);
            this.label11.TabIndex = 190;
            this.label11.Text = "短信状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(448, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "短信管理";
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefreshData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(60, 22);
            this.btnRefreshData.Tag = "view";
            this.btnRefreshData.Text = "刷新数据";
            this.btnRefreshData.Click += new System.EventHandler(this.btnRefreshData_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnUpdate,
            this.btnPublish,
            this.btn撤销,
            this.toolStripSeparator1,
            this.btnDelete,
            this.toolStripSeparatorDelete,
            this.btnRefreshData});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(54, 22);
            this.btnNew.Tag = "ADD";
            this.btnNew.Text = "新建(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(53, 22);
            this.btnUpdate.Tag = "update";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnPublish
            // 
            this.btnPublish.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(51, 22);
            this.btnPublish.Tag = "update";
            this.btnPublish.Text = "发布(&P)";
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(53, 22);
            this.btnDelete.Tag = "delete";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorDelete.Tag = "";
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // btn撤销
            // 
            this.btn撤销.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btn撤销.Name = "btn撤销";
            this.btn撤销.Size = new System.Drawing.Size(52, 22);
            this.btn撤销.Tag = "update";
            this.btn撤销.Text = "撤销(&R)";
            this.btn撤销.Click += new System.EventHandler(this.btn撤销_Click);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 223);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(960, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(17, 94);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 112;
            this.label8.Text = "短信类别";
            // 
            // cmbShortMsgType
            // 
            this.cmbShortMsgType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbShortMsgType.FormattingEnabled = true;
            this.cmbShortMsgType.Location = new System.Drawing.Point(86, 91);
            this.cmbShortMsgType.Name = "cmbShortMsgType";
            this.cmbShortMsgType.Size = new System.Drawing.Size(121, 22);
            this.cmbShortMsgType.TabIndex = 113;
            // 
            // FormShortMsgManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormShortMsgManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "短信管理";
            this.Resize += new System.EventHandler(this.FormWiseManagement_Resize);
            this.panelMain.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelPara.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBoxCreator.ResumeLayout(false);
            this.groupBoxCreator.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chk所有;
        private System.Windows.Forms.CheckBox chk暂存;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerST;
        private System.Windows.Forms.DateTimePicker dateTimePickerET;
        private System.Windows.Forms.GroupBox groupBoxCreator;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtReceiver;
        private System.Windows.Forms.TextBox txtDeclarePersonnel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpkCreateTime;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBillStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripButton btnRefreshData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.CheckBox chk待发送;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DateTimePicker dtpkSendTime;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton btnPublish;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMobileNo;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.CheckBox chk已发送;
        private System.Windows.Forms.ToolStripButton btn撤销;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ComboBox cmbShortMsgType;
        private System.Windows.Forms.Label label8;
    }
}