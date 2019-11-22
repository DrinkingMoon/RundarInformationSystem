namespace Form_Project_Design
{
    partial class 生产BOM变更单明细
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFileCode = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lbDownFile = new System.Windows.Forms.Label();
            this.lbUpFile = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridViewStruct = new UniversalControlLibrary.CustomDataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridViewEdtion = new UniversalControlLibrary.CustomDataGridView();
            this.总成型号_ZC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.设计BOM版本_ZC = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cmbDBOMVersion = new System.Windows.Forms.ComboBox();
            this.cmbEdition = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnParentDelete = new System.Windows.Forms.Button();
            this.btnParentAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.选 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.父级图号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件图号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件类别 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.基数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.选配 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.台套领料 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.采购 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.生效版次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.生效日期 = new UniversalControlLibrary.DataGridViewDateTimePickColumn();
            this.失效版次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.父级物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.总成型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.设计BOM版本 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStruct)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEdtion)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtReason);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFileCode);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.lbDownFile);
            this.groupBox1.Controls.Add(this.lbUpFile);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(873, 112);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据总信息";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.SystemColors.Window;
            this.txtReason.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReason.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtReason.Location = new System.Drawing.Point(352, 62);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtReason.Size = new System.Drawing.Size(454, 39);
            this.txtReason.TabIndex = 321;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(277, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 320;
            this.label6.Text = "变更原因";
            // 
            // txtFileCode
            // 
            this.txtFileCode.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFileCode.Location = new System.Drawing.Point(87, 26);
            this.txtFileCode.Name = "txtFileCode";
            this.txtFileCode.Size = new System.Drawing.Size(171, 21);
            this.txtFileCode.TabIndex = 319;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.ForeColor = System.Drawing.Color.Blue;
            this.label15.Location = new System.Drawing.Point(18, 30);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 318;
            this.label15.Text = "技术单号";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(18, 75);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 317;
            this.label13.Text = "文件操作";
            // 
            // lbDownFile
            // 
            this.lbDownFile.AutoSize = true;
            this.lbDownFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbDownFile.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbDownFile.Location = new System.Drawing.Point(159, 75);
            this.lbDownFile.Name = "lbDownFile";
            this.lbDownFile.Size = new System.Drawing.Size(53, 12);
            this.lbDownFile.TabIndex = 315;
            this.lbDownFile.Text = "下载文件";
            this.lbDownFile.Click += new System.EventHandler(this.lbDownFile_Click);
            // 
            // lbUpFile
            // 
            this.lbUpFile.AutoSize = true;
            this.lbUpFile.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUpFile.ForeColor = System.Drawing.SystemColors.Highlight;
            this.lbUpFile.Location = new System.Drawing.Point(85, 75);
            this.lbUpFile.Name = "lbUpFile";
            this.lbUpFile.Size = new System.Drawing.Size(53, 12);
            this.lbUpFile.TabIndex = 314;
            this.lbUpFile.Text = "上传文件";
            this.lbUpFile.Click += new System.EventHandler(this.lbUpFile_Click);
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(652, 30);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 313;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(548, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 312;
            this.label5.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(352, 26);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(171, 21);
            this.txtBillNo.TabIndex = 310;
            this.txtBillNo.Text = "PGD201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(277, 30);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 311;
            this.label4.Text = "单据编号";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Controls.Add(this.groupBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 112);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(873, 518);
            this.panel1.TabIndex = 3;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridViewStruct);
            this.panel3.Controls.Add(this.userControlDataLocalizer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(233, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(640, 518);
            this.panel3.TabIndex = 6;
            // 
            // dataGridViewStruct
            // 
            this.dataGridViewStruct.AllowUserToAddRows = false;
            this.dataGridViewStruct.AllowUserToDeleteRows = false;
            this.dataGridViewStruct.AllowUserToResizeRows = false;
            this.dataGridViewStruct.AutoCreateFilters = true;
            this.dataGridViewStruct.BaseFilter = "";
            this.dataGridViewStruct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewStruct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选,
            this.父级图号,
            this.零件图号,
            this.零件名称,
            this.零件规格,
            this.零件类别,
            this.基数,
            this.选配,
            this.台套领料,
            this.采购,
            this.生效版次号,
            this.生效日期,
            this.失效版次号,
            this.单据号,
            this.父级物品ID,
            this.物品ID,
            this.总成型号,
            this.设计BOM版本,
            this.ID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewStruct.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridViewStruct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewStruct.Location = new System.Drawing.Point(0, 32);
            this.dataGridViewStruct.Name = "dataGridViewStruct";
            this.dataGridViewStruct.RowHeadersWidth = 21;
            this.dataGridViewStruct.RowTemplate.Height = 23;
            this.dataGridViewStruct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewStruct.Size = new System.Drawing.Size(640, 486);
            this.dataGridViewStruct.TabIndex = 6;
            this.dataGridViewStruct.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewStruct_CellClick);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = true;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(640, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridViewEdtion);
            this.groupBox3.Controls.Add(this.panel2);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(233, 518);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "总成信息";
            // 
            // dataGridViewEdtion
            // 
            this.dataGridViewEdtion.AllowUserToAddRows = false;
            this.dataGridViewEdtion.AllowUserToDeleteRows = false;
            this.dataGridViewEdtion.AllowUserToResizeRows = false;
            this.dataGridViewEdtion.AutoCreateFilters = true;
            this.dataGridViewEdtion.BaseFilter = "";
            this.dataGridViewEdtion.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEdtion.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.总成型号_ZC,
            this.设计BOM版本_ZC});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridViewEdtion.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridViewEdtion.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEdtion.Location = new System.Drawing.Point(3, 132);
            this.dataGridViewEdtion.MultiSelect = false;
            this.dataGridViewEdtion.Name = "dataGridViewEdtion";
            this.dataGridViewEdtion.ReadOnly = true;
            this.dataGridViewEdtion.RowHeadersWidth = 21;
            this.dataGridViewEdtion.RowTemplate.Height = 23;
            this.dataGridViewEdtion.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewEdtion.Size = new System.Drawing.Size(227, 383);
            this.dataGridViewEdtion.TabIndex = 4;
            this.dataGridViewEdtion.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewEdtion_CellEnter);
            // 
            // 总成型号_ZC
            // 
            this.总成型号_ZC.DataPropertyName = "总成型号";
            this.总成型号_ZC.HeaderText = "总成型号";
            this.总成型号_ZC.Name = "总成型号_ZC";
            this.总成型号_ZC.ReadOnly = true;
            // 
            // 设计BOM版本_ZC
            // 
            this.设计BOM版本_ZC.DataPropertyName = "设计BOM版本";
            this.设计BOM版本_ZC.HeaderText = "设计BOM版本";
            this.设计BOM版本_ZC.Name = "设计BOM版本_ZC";
            this.设计BOM版本_ZC.ReadOnly = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.cmbDBOMVersion);
            this.panel2.Controls.Add(this.cmbEdition);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnParentDelete);
            this.panel2.Controls.Add(this.btnParentAdd);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(227, 115);
            this.panel2.TabIndex = 2;
            // 
            // cmbDBOMVersion
            // 
            this.cmbDBOMVersion.BackColor = System.Drawing.Color.White;
            this.cmbDBOMVersion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDBOMVersion.FormattingEnabled = true;
            this.cmbDBOMVersion.Location = new System.Drawing.Point(94, 46);
            this.cmbDBOMVersion.Name = "cmbDBOMVersion";
            this.cmbDBOMVersion.Size = new System.Drawing.Size(115, 20);
            this.cmbDBOMVersion.TabIndex = 268;
            // 
            // cmbEdition
            // 
            this.cmbEdition.BackColor = System.Drawing.Color.White;
            this.cmbEdition.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEdition.FormattingEnabled = true;
            this.cmbEdition.Location = new System.Drawing.Point(69, 13);
            this.cmbEdition.Name = "cmbEdition";
            this.cmbEdition.Size = new System.Drawing.Size(140, 20);
            this.cmbEdition.TabIndex = 267;
            this.cmbEdition.SelectedIndexChanged += new System.EventHandler(this.cmbEdition_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(8, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 12);
            this.label1.TabIndex = 266;
            this.label1.Text = "设计BOM版本";
            // 
            // btnParentDelete
            // 
            this.btnParentDelete.Location = new System.Drawing.Point(130, 76);
            this.btnParentDelete.Name = "btnParentDelete";
            this.btnParentDelete.Size = new System.Drawing.Size(70, 23);
            this.btnParentDelete.TabIndex = 264;
            this.btnParentDelete.Text = "删除";
            this.btnParentDelete.UseVisualStyleBackColor = true;
            this.btnParentDelete.Click += new System.EventHandler(this.btnParentDelete_Click);
            // 
            // btnParentAdd
            // 
            this.btnParentAdd.Location = new System.Drawing.Point(44, 76);
            this.btnParentAdd.Name = "btnParentAdd";
            this.btnParentAdd.Size = new System.Drawing.Size(70, 23);
            this.btnParentAdd.TabIndex = 263;
            this.btnParentAdd.Text = "添加";
            this.btnParentAdd.UseVisualStyleBackColor = true;
            this.btnParentAdd.Click += new System.EventHandler(this.btnParentAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(8, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 249;
            this.label3.Text = "总成型号";
            // 
            // 选
            // 
            this.选.DataPropertyName = "选";
            this.选.HeaderText = "选";
            this.选.Name = "选";
            this.选.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.选.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.选.Width = 40;
            // 
            // 父级图号
            // 
            this.父级图号.DataPropertyName = "父级图号";
            this.父级图号.HeaderText = "父级图号";
            this.父级图号.Name = "父级图号";
            this.父级图号.ReadOnly = true;
            this.父级图号.Width = 120;
            // 
            // 零件图号
            // 
            this.零件图号.DataPropertyName = "零件图号";
            this.零件图号.HeaderText = "零件图号";
            this.零件图号.Name = "零件图号";
            this.零件图号.ReadOnly = true;
            this.零件图号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.零件图号.Width = 120;
            // 
            // 零件名称
            // 
            this.零件名称.DataPropertyName = "零件名称";
            this.零件名称.HeaderText = "零件名称";
            this.零件名称.Name = "零件名称";
            this.零件名称.ReadOnly = true;
            this.零件名称.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.零件名称.Width = 160;
            // 
            // 零件规格
            // 
            this.零件规格.DataPropertyName = "零件规格";
            this.零件规格.HeaderText = "零件规格";
            this.零件规格.Name = "零件规格";
            this.零件规格.ReadOnly = true;
            this.零件规格.Width = 80;
            // 
            // 零件类别
            // 
            this.零件类别.DataPropertyName = "零件类别";
            this.零件类别.HeaderText = "零件类别";
            this.零件类别.Items.AddRange(new object[] {
            "成品",
            "自制件",
            "半成品",
            "毛坯"});
            this.零件类别.Name = "零件类别";
            this.零件类别.Width = 80;
            // 
            // 基数
            // 
            this.基数.DataPropertyName = "基数";
            this.基数.HeaderText = "基数";
            this.基数.Name = "基数";
            this.基数.ReadOnly = true;
            this.基数.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.基数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.基数.Width = 60;
            // 
            // 选配
            // 
            this.选配.DataPropertyName = "选配";
            this.选配.HeaderText = "选配";
            this.选配.Name = "选配";
            this.选配.Width = 40;
            // 
            // 台套领料
            // 
            this.台套领料.DataPropertyName = "台套领料";
            this.台套领料.HeaderText = "台套领料";
            this.台套领料.Name = "台套领料";
            this.台套领料.Width = 80;
            // 
            // 采购
            // 
            this.采购.DataPropertyName = "采购";
            this.采购.HeaderText = "采购";
            this.采购.Name = "采购";
            this.采购.Width = 40;
            // 
            // 生效版次号
            // 
            this.生效版次号.DataPropertyName = "生效版次号";
            this.生效版次号.HeaderText = "生效版次号";
            this.生效版次号.Name = "生效版次号";
            this.生效版次号.ReadOnly = true;
            this.生效版次号.Width = 90;
            // 
            // 生效日期
            // 
            this.生效日期.DataPropertyName = "生效日期";
            this.生效日期.HeaderText = "生效日期";
            this.生效日期.Name = "生效日期";
            this.生效日期.Width = 150;
            // 
            // 失效版次号
            // 
            this.失效版次号.DataPropertyName = "失效版次号";
            this.失效版次号.HeaderText = "失效版次号";
            this.失效版次号.Name = "失效版次号";
            this.失效版次号.Width = 90;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.Visible = false;
            // 
            // 父级物品ID
            // 
            this.父级物品ID.DataPropertyName = "父级物品ID";
            this.父级物品ID.HeaderText = "父级物品ID";
            this.父级物品ID.Name = "父级物品ID";
            this.父级物品ID.Visible = false;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.Visible = false;
            // 
            // 总成型号
            // 
            this.总成型号.DataPropertyName = "总成型号";
            this.总成型号.HeaderText = "总成型号";
            this.总成型号.Name = "总成型号";
            this.总成型号.Visible = false;
            // 
            // 设计BOM版本
            // 
            this.设计BOM版本.DataPropertyName = "设计BOM版本";
            this.设计BOM版本.HeaderText = "设计BOM版本";
            this.设计BOM版本.Name = "设计BOM版本";
            this.设计BOM版本.Visible = false;
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.Visible = false;
            // 
            // 生产BOM变更单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 630);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.groupBox1);
            this.Name = "生产BOM变更单明细";
            this.Text = "生产BOM变更单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.生产BOM变更单明细_PanelGetDataInfo);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewStruct)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEdtion)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFileCode;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lbDownFile;
        private System.Windows.Forms.Label lbUpFile;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox3;
        private UniversalControlLibrary.CustomDataGridView dataGridViewEdtion;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btnParentDelete;
        private System.Windows.Forms.Button btnParentAdd;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private UniversalControlLibrary.CustomDataGridView dataGridViewStruct;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 总成型号_ZC;
        private System.Windows.Forms.DataGridViewTextBoxColumn 设计BOM版本_ZC;
        private System.Windows.Forms.ComboBox cmbEdition;
        private System.Windows.Forms.ComboBox cmbDBOMVersion;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选;
        private System.Windows.Forms.DataGridViewTextBoxColumn 父级图号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件图号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件规格;
        private System.Windows.Forms.DataGridViewComboBoxColumn 零件类别;
        private System.Windows.Forms.DataGridViewTextBoxColumn 基数;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选配;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 台套领料;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 采购;
        private System.Windows.Forms.DataGridViewTextBoxColumn 生效版次号;
        private UniversalControlLibrary.DataGridViewDateTimePickColumn 生效日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 失效版次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 父级物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 总成型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 设计BOM版本;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
    }
}