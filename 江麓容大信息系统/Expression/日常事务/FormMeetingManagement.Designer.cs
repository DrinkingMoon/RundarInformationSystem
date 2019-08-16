using UniversalControlLibrary;
namespace Expression
{
    partial class FormMeetingManagement
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
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkMySelf = new System.Windows.Forms.CheckBox();
            this.chk已撤销会议 = new System.Windows.Forms.CheckBox();
            this.chk已召开会议 = new System.Windows.Forms.CheckBox();
            this.chk已发布未召开会议 = new System.Windows.Forms.CheckBox();
            this.chk所有 = new System.Windows.Forms.CheckBox();
            this.chk待发布会议 = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTimePickerST = new System.Windows.Forms.DateTimePicker();
            this.dateTimePickerET = new System.Windows.Forms.DateTimePicker();
            this.groupBoxCreator = new System.Windows.Forms.GroupBox();
            this.dtpkBeginTime = new System.Windows.Forms.DateTimePicker();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpkEndTime = new System.Windows.Forms.DateTimePicker();
            this.label9 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txt与会资源 = new System.Windows.Forms.TextBox();
            this.txt与会人员 = new System.Windows.Forms.TextBox();
            this.txtDeclarePersonnel = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtHost = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpkCreateTime = new System.Windows.Forms.DateTimePicker();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBillStatus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tlsb单据统计 = new System.Windows.Forms.ToolStripButton();
            this.btnAdvSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefreshData = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.批准 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnView = new System.Windows.Forms.ToolStripButton();
            this.btnSetRealFinishedTime = new System.Windows.Forms.ToolStripButton();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblPersonAmount = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
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
            this.dataGridView1.Location = new System.Drawing.Point(0, 221);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 46;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(960, 429);
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
            this.panelPara.Size = new System.Drawing.Size(960, 221);
            this.panelPara.TabIndex = 16;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(8, 187);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(945, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.chkMySelf);
            this.groupBox2.Controls.Add(this.chk已撤销会议);
            this.groupBox2.Controls.Add(this.chk已召开会议);
            this.groupBox2.Controls.Add(this.chk已发布未召开会议);
            this.groupBox2.Controls.Add(this.chk所有);
            this.groupBox2.Controls.Add(this.chk待发布会议);
            this.groupBox2.Location = new System.Drawing.Point(509, 100);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(444, 82);
            this.groupBox2.TabIndex = 158;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "显示控制";
            // 
            // chkMySelf
            // 
            this.chkMySelf.AutoSize = true;
            this.chkMySelf.Checked = true;
            this.chkMySelf.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkMySelf.ForeColor = System.Drawing.SystemColors.Highlight;
            this.chkMySelf.Location = new System.Drawing.Point(252, 51);
            this.chkMySelf.Name = "chkMySelf";
            this.chkMySelf.Size = new System.Drawing.Size(138, 18);
            this.chkMySelf.TabIndex = 5;
            this.chkMySelf.Text = "与自己有关的会议";
            this.chkMySelf.UseVisualStyleBackColor = true;
            // 
            // chk已撤销会议
            // 
            this.chk已撤销会议.AutoSize = true;
            this.chk已撤销会议.Location = new System.Drawing.Point(108, 51);
            this.chk已撤销会议.Name = "chk已撤销会议";
            this.chk已撤销会议.Size = new System.Drawing.Size(96, 18);
            this.chk已撤销会议.TabIndex = 4;
            this.chk已撤销会议.Text = "已撤销会议";
            this.chk已撤销会议.UseVisualStyleBackColor = true;
            this.chk已撤销会议.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // chk已召开会议
            // 
            this.chk已召开会议.AutoSize = true;
            this.chk已召开会议.Location = new System.Drawing.Point(252, 18);
            this.chk已召开会议.Name = "chk已召开会议";
            this.chk已召开会议.Size = new System.Drawing.Size(96, 18);
            this.chk已召开会议.TabIndex = 2;
            this.chk已召开会议.Text = "已召开会议";
            this.chk已召开会议.UseVisualStyleBackColor = true;
            this.chk已召开会议.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // chk已发布未召开会议
            // 
            this.chk已发布未召开会议.AutoSize = true;
            this.chk已发布未召开会议.Location = new System.Drawing.Point(108, 18);
            this.chk已发布未召开会议.Name = "chk已发布未召开会议";
            this.chk已发布未召开会议.Size = new System.Drawing.Size(138, 18);
            this.chk已发布未召开会议.TabIndex = 1;
            this.chk已发布未召开会议.Text = "已发布未召开会议";
            this.chk已发布未召开会议.UseVisualStyleBackColor = true;
            this.chk已发布未召开会议.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // chk所有
            // 
            this.chk所有.AutoSize = true;
            this.chk所有.Location = new System.Drawing.Point(6, 51);
            this.chk所有.Name = "chk所有";
            this.chk所有.Size = new System.Drawing.Size(82, 18);
            this.chk所有.TabIndex = 3;
            this.chk所有.Text = "所有会议";
            this.chk所有.UseVisualStyleBackColor = true;
            this.chk所有.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // chk待发布会议
            // 
            this.chk待发布会议.AutoSize = true;
            this.chk待发布会议.Location = new System.Drawing.Point(6, 18);
            this.chk待发布会议.Name = "chk待发布会议";
            this.chk待发布会议.Size = new System.Drawing.Size(96, 18);
            this.chk待发布会议.TabIndex = 0;
            this.chk待发布会议.Text = "待发布会议";
            this.chk待发布会议.UseVisualStyleBackColor = true;
            this.chk待发布会议.CheckedChanged += new System.EventHandler(this.chk显示控制_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSearch);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTimePickerST);
            this.groupBox1.Controls.Add(this.dateTimePickerET);
            this.groupBox1.Location = new System.Drawing.Point(8, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(466, 82);
            this.groupBox1.TabIndex = 157;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "会议时间范围";
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Image = global::UniversalControlLibrary.Properties.Resources.Search;
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
            this.groupBoxCreator.Controls.Add(this.dtpkBeginTime);
            this.groupBoxCreator.Controls.Add(this.label14);
            this.groupBoxCreator.Controls.Add(this.dtpkEndTime);
            this.groupBoxCreator.Controls.Add(this.label9);
            this.groupBoxCreator.Controls.Add(this.label3);
            this.groupBoxCreator.Controls.Add(this.label12);
            this.groupBoxCreator.Controls.Add(this.txtTitle);
            this.groupBoxCreator.Controls.Add(this.label4);
            this.groupBoxCreator.Controls.Add(this.txt与会资源);
            this.groupBoxCreator.Controls.Add(this.txt与会人员);
            this.groupBoxCreator.Controls.Add(this.txtDeclarePersonnel);
            this.groupBoxCreator.Controls.Add(this.label2);
            this.groupBoxCreator.Controls.Add(this.label7);
            this.groupBoxCreator.Controls.Add(this.txtID);
            this.groupBoxCreator.Controls.Add(this.label6);
            this.groupBoxCreator.Controls.Add(this.txtHost);
            this.groupBoxCreator.Controls.Add(this.label5);
            this.groupBoxCreator.Controls.Add(this.dtpkCreateTime);
            this.groupBoxCreator.Location = new System.Drawing.Point(8, 5);
            this.groupBoxCreator.Name = "groupBoxCreator";
            this.groupBoxCreator.Size = new System.Drawing.Size(945, 91);
            this.groupBoxCreator.TabIndex = 156;
            this.groupBoxCreator.TabStop = false;
            this.groupBoxCreator.Text = "信息预览";
            // 
            // dtpkBeginTime
            // 
            this.dtpkBeginTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpkBeginTime.Enabled = false;
            this.dtpkBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpkBeginTime.Location = new System.Drawing.Point(86, 64);
            this.dtpkBeginTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpkBeginTime.Name = "dtpkBeginTime";
            this.dtpkBeginTime.Size = new System.Drawing.Size(151, 23);
            this.dtpkBeginTime.TabIndex = 3;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(246, 68);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 14);
            this.label14.TabIndex = 160;
            this.label14.Text = "结束时间";
            // 
            // dtpkEndTime
            // 
            this.dtpkEndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpkEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpkEndTime.Location = new System.Drawing.Point(315, 64);
            this.dtpkEndTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpkEndTime.Name = "dtpkEndTime";
            this.dtpkEndTime.Size = new System.Drawing.Size(151, 23);
            this.dtpkEndTime.TabIndex = 4;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(16, 68);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 107;
            this.label9.Text = "开始时间";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(498, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 156;
            this.label3.Text = "会议资源";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(498, 43);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 103;
            this.label12.Text = "与会人员";
            // 
            // txtTitle
            // 
            this.txtTitle.BackColor = System.Drawing.Color.White;
            this.txtTitle.ForeColor = System.Drawing.Color.Black;
            this.txtTitle.Location = new System.Drawing.Point(86, 39);
            this.txtTitle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.ReadOnly = true;
            this.txtTitle.Size = new System.Drawing.Size(380, 23);
            this.txtTitle.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(17, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 104;
            this.label4.Text = "会议主题";
            // 
            // txt与会资源
            // 
            this.txt与会资源.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt与会资源.BackColor = System.Drawing.Color.White;
            this.txt与会资源.Location = new System.Drawing.Point(567, 64);
            this.txt与会资源.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt与会资源.Name = "txt与会资源";
            this.txt与会资源.ReadOnly = true;
            this.txt与会资源.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txt与会资源.Size = new System.Drawing.Size(354, 23);
            this.txt与会资源.TabIndex = 6;
            this.txt与会资源.MouseEnter += new System.EventHandler(this.txt与会资源_MouseEnter);
            // 
            // txt与会人员
            // 
            this.txt与会人员.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txt与会人员.BackColor = System.Drawing.Color.White;
            this.txt与会人员.Location = new System.Drawing.Point(567, 39);
            this.txt与会人员.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txt与会人员.Name = "txt与会人员";
            this.txt与会人员.ReadOnly = true;
            this.txt与会人员.Size = new System.Drawing.Size(354, 23);
            this.txt与会人员.TabIndex = 102;
            this.txt与会人员.MouseEnter += new System.EventHandler(this.txt与会人员_MouseEnter);
            // 
            // txtDeclarePersonnel
            // 
            this.txtDeclarePersonnel.BackColor = System.Drawing.Color.White;
            this.txtDeclarePersonnel.ForeColor = System.Drawing.Color.Black;
            this.txtDeclarePersonnel.Location = new System.Drawing.Point(567, 14);
            this.txtDeclarePersonnel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDeclarePersonnel.Name = "txtDeclarePersonnel";
            this.txtDeclarePersonnel.ReadOnly = true;
            this.txtDeclarePersonnel.Size = new System.Drawing.Size(139, 23);
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
            this.label2.Text = "会议编号";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Black;
            this.label7.Location = new System.Drawing.Point(714, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 98;
            this.label7.Text = "主 持 人";
            // 
            // txtID
            // 
            this.txtID.BackColor = System.Drawing.Color.White;
            this.txtID.ForeColor = System.Drawing.Color.Red;
            this.txtID.Location = new System.Drawing.Point(86, 14);
            this.txtID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(152, 23);
            this.txtID.TabIndex = 0;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(498, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 96;
            this.label6.Text = "编 制 人";
            // 
            // txtHost
            // 
            this.txtHost.BackColor = System.Drawing.Color.White;
            this.txtHost.ForeColor = System.Drawing.Color.Black;
            this.txtHost.Location = new System.Drawing.Point(783, 14);
            this.txtHost.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtHost.Name = "txtHost";
            this.txtHost.ReadOnly = true;
            this.txtHost.Size = new System.Drawing.Size(138, 23);
            this.txtHost.TabIndex = 99;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(246, 18);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 38;
            this.label5.Text = "编制时间";
            // 
            // dtpkCreateTime
            // 
            this.dtpkCreateTime.Enabled = false;
            this.dtpkCreateTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dtpkCreateTime.Location = new System.Drawing.Point(315, 14);
            this.dtpkCreateTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpkCreateTime.Name = "dtpkCreateTime";
            this.dtpkCreateTime.Size = new System.Drawing.Size(151, 23);
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
            this.panel1.Controls.Add(this.lblPersonAmount);
            this.panel1.Controls.Add(this.label13);
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
            this.label11.Text = "会议状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(448, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "会议管理";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsb单据统计
            // 
            this.tlsb单据统计.Image = global::UniversalControlLibrary.Properties.Resources.高级检索;
            this.tlsb单据统计.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsb单据统计.Name = "tlsb单据统计";
            this.tlsb单据统计.Size = new System.Drawing.Size(76, 22);
            this.tlsb单据统计.Tag = "-view-";
            this.tlsb单据统计.Text = "会议统计";
            this.tlsb单据统计.Visible = false;
            // 
            // btnAdvSearch
            // 
            this.btnAdvSearch.Image = global::UniversalControlLibrary.Properties.Resources.Search;
            this.btnAdvSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdvSearch.Name = "btnAdvSearch";
            this.btnAdvSearch.Size = new System.Drawing.Size(76, 22);
            this.btnAdvSearch.Tag = "view";
            this.btnAdvSearch.Text = "高级检索";
            this.btnAdvSearch.Click += new System.EventHandler(this.btnAdvSearch_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Visible = false;
            // 
            // btnRefreshData
            // 
            this.btnRefreshData.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.btnRefreshData.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnRefreshData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefreshData.Name = "btnRefreshData";
            this.btnRefreshData.Size = new System.Drawing.Size(76, 22);
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
            this.btnDelete,
            this.批准,
            this.toolStripSeparatorDelete,
            this.btnAdvSearch,
            this.toolStripSeparator4,
            this.tlsb单据统计,
            this.toolStripSeparator2,
            this.btnView,
            this.btnRefreshData,
            this.btnSetRealFinishedTime});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.Image = global::UniversalControlLibrary.Properties.Resources.File2;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(70, 22);
            this.btnNew.Tag = "ADD";
            this.btnNew.Text = "新建(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.table;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(69, 22);
            this.btnUpdate.Tag = "update";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 22);
            this.btnDelete.Tag = "delete";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // 批准
            // 
            this.批准.Image = global::UniversalControlLibrary.Properties.Resources.Ok;
            this.批准.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.批准.Name = "批准";
            this.批准.Size = new System.Drawing.Size(67, 22);
            this.批准.Tag = "Authorize";
            this.批准.Text = "批准(&P)";
            this.批准.Visible = false;
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorDelete.Tag = "";
            // 
            // btnView
            // 
            this.btnView.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.btnView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnView.Name = "btnView";
            this.btnView.Size = new System.Drawing.Size(108, 22);
            this.btnView.Tag = "view";
            this.btnView.Text = "查看当前会议信息";
            this.btnView.Click += new System.EventHandler(this.btnView_Click);
            // 
            // btnSetRealFinishedTime
            // 
            this.btnSetRealFinishedTime.Image = global::UniversalControlLibrary.Properties.Resources.match;
            this.btnSetRealFinishedTime.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetRealFinishedTime.Name = "btnSetRealFinishedTime";
            this.btnSetRealFinishedTime.Size = new System.Drawing.Size(148, 22);
            this.btnSetRealFinishedTime.Text = "设置会议实际完成时间";
            this.btnSetRealFinishedTime.Click += new System.EventHandler(this.btnSetRealFinishedTime_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            // 
            // lblPersonAmount
            // 
            this.lblPersonAmount.AutoSize = true;
            this.lblPersonAmount.Location = new System.Drawing.Point(310, 18);
            this.lblPersonAmount.Name = "lblPersonAmount";
            this.lblPersonAmount.Size = new System.Drawing.Size(0, 14);
            this.lblPersonAmount.TabIndex = 193;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(227, 18);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 192;
            this.label13.Text = "会议人数：";
            // 
            // FormMeetingManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 734);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMeetingManagement";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "会议管理";
            this.Resize += new System.EventHandler(this.FormMeetingManagement_Resize);
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
        private System.Windows.Forms.CheckBox chk待发布会议;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTimePickerST;
        private System.Windows.Forms.DateTimePicker dateTimePickerET;
        private System.Windows.Forms.GroupBox groupBoxCreator;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.DateTimePicker dtpkEndTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txt与会资源;
        private System.Windows.Forms.TextBox txt与会人员;
        private System.Windows.Forms.TextBox txtDeclarePersonnel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtHost;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpkCreateTime;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBillStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tlsb单据统计;
        private System.Windows.Forms.ToolStripButton btnAdvSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnRefreshData;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.CheckBox chk已召开会议;
        private System.Windows.Forms.CheckBox chk已发布未召开会议;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DateTimePicker dtpkBeginTime;
        private System.Windows.Forms.CheckBox chk已撤销会议;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton btnSetRealFinishedTime;
        private System.Windows.Forms.ToolStripButton 批准;
        private System.Windows.Forms.ToolStripButton btnView;
        private System.Windows.Forms.CheckBox chkMySelf;
        private System.Windows.Forms.Label lblPersonAmount;
        private System.Windows.Forms.Label label13;
    }
}