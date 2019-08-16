namespace Expression
{
    partial class 打印CVT出厂条形码
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
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.btnPrintSelectedList = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtPrintRemark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.cmbPrintMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numPrintAmount = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.numEndNumber = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numBeginNumber = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.btnFindRule = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.txtBuildRuleID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.打印编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.打印规则编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.打印起始编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.打印结束编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.打印份数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.打印模式 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.条形码构建规则 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.条形码示例 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrintAmount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndNumber)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeginNumber)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.toolStripSeparator1,
            this.btnSh,
            this.toolStripSeparator3,
            this.btnPrint,
            this.btnPrintSelectedList,
            this.toolStripSeparator4,
            this.btnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1028, 25);
            this.toolStrip.TabIndex = 7;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 22);
            this.btnSave.Tag = "Add";
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSh
            // 
            this.btnSh.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSh.Image = global::UniversalControlLibrary.Properties.Resources.审核6;
            this.btnSh.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSh.Name = "btnSh";
            this.btnSh.Size = new System.Drawing.Size(67, 22);
            this.btnSh.Tag = "Authorize";
            this.btnSh.Text = "批准(&P)";
            this.btnSh.Visible = false;
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Visible = false;
            // 
            // btnPrint
            // 
            this.btnPrint.Enabled = false;
            this.btnPrint.Image = global::UniversalControlLibrary.Properties.Resources.Printer2;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(139, 22);
            this.btnPrint.Tag = "Auditing";
            this.btnPrint.Text = "打印所有明细设置(&P)";
            this.btnPrint.ToolTipText = "打印所有明细设置,注意：要先保存再打印(P)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnPrintSelectedList
            // 
            this.btnPrintSelectedList.Enabled = false;
            this.btnPrintSelectedList.Image = global::UniversalControlLibrary.Properties.Resources.Printer2;
            this.btnPrintSelectedList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrintSelectedList.Name = "btnPrintSelectedList";
            this.btnPrintSelectedList.Size = new System.Drawing.Size(97, 22);
            this.btnPrintSelectedList.Tag = "Auditing";
            this.btnPrintSelectedList.Text = "打印选择明细";
            this.btnPrintSelectedList.ToolTipText = "打印选择明细,注意：要先保存再打印(P)";
            this.btnPrintSelectedList.Click += new System.EventHandler(this.btnPrintSelectedList_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator4.Tag = "";
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 22);
            this.btnClose.Tag = "view";
            this.btnClose.Text = "退出(&Q)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtPrintRemark);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1028, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "本次打印主信息";
            // 
            // txtPrintRemark
            // 
            this.txtPrintRemark.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPrintRemark.Location = new System.Drawing.Point(83, 17);
            this.txtPrintRemark.Multiline = true;
            this.txtPrintRemark.Name = "txtPrintRemark";
            this.txtPrintRemark.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtPrintRemark.Size = new System.Drawing.Size(935, 73);
            this.txtPrintRemark.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "打印说明";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnUpdate);
            this.groupBox2.Controls.Add(this.dateTimePicker1);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.cmbPrintMode);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.numPrintAmount);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.numEndNumber);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.numBeginNumber);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.btnFindRule);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtProductType);
            this.groupBox2.Controls.Add(this.txtBuildRuleID);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 125);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1028, 111);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "明细信息";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(804, 19);
            this.btnUpdate.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(89, 35);
            this.btnUpdate.TabIndex = 58;
            this.btnUpdate.Tag = "UPDATE";
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(522, 24);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(130, 23);
            this.dateTimePicker1.TabIndex = 14;
            this.toolTip1.SetToolTip(this.dateTimePicker1, "取年月数据，日数据无效");
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(681, 19);
            this.btnAdd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(89, 35);
            this.btnAdd.TabIndex = 56;
            this.btnAdd.Tag = "ADD";
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(452, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 13;
            this.label8.Text = "产品日期";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(927, 18);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(89, 35);
            this.btnDelete.TabIndex = 57;
            this.btnDelete.Tag = "DELETE";
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // cmbPrintMode
            // 
            this.cmbPrintMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrintMode.FormattingEnabled = true;
            this.cmbPrintMode.Location = new System.Drawing.Point(735, 70);
            this.cmbPrintMode.Name = "cmbPrintMode";
            this.cmbPrintMode.Size = new System.Drawing.Size(281, 22);
            this.cmbPrintMode.TabIndex = 12;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(666, 74);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 11;
            this.label7.Text = "打印模式";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numPrintAmount
            // 
            this.numPrintAmount.Location = new System.Drawing.Point(521, 70);
            this.numPrintAmount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.numPrintAmount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numPrintAmount.Name = "numPrintAmount";
            this.numPrintAmount.Size = new System.Drawing.Size(131, 23);
            this.numPrintAmount.TabIndex = 10;
            this.numPrintAmount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(452, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 9;
            this.label6.Text = "打印份数";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numEndNumber
            // 
            this.numEndNumber.Location = new System.Drawing.Point(295, 70);
            this.numEndNumber.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numEndNumber.Name = "numEndNumber";
            this.numEndNumber.Size = new System.Drawing.Size(143, 23);
            this.numEndNumber.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(225, 62);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 38);
            this.label5.TabIndex = 7;
            this.label5.Text = "打印结束编号";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // numBeginNumber
            // 
            this.numBeginNumber.Location = new System.Drawing.Point(83, 70);
            this.numBeginNumber.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.numBeginNumber.Name = "numBeginNumber";
            this.numBeginNumber.Size = new System.Drawing.Size(102, 23);
            this.numBeginNumber.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(13, 62);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 38);
            this.label4.TabIndex = 5;
            this.label4.Text = "打印起始编号";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnFindRule
            // 
            this.btnFindRule.BackColor = System.Drawing.Color.Transparent;
            this.btnFindRule.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindRule.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindRule.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindRule.Location = new System.Drawing.Point(191, 25);
            this.btnFindRule.Name = "btnFindRule";
            this.btnFindRule.Size = new System.Drawing.Size(21, 21);
            this.btnFindRule.TabIndex = 4;
            this.btnFindRule.UseVisualStyleBackColor = false;
            this.btnFindRule.Click += new System.EventHandler(this.btnFindRule_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(222, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "产品型号";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtProductType
            // 
            this.txtProductType.Location = new System.Drawing.Point(295, 24);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.ReadOnly = true;
            this.txtProductType.Size = new System.Drawing.Size(143, 23);
            this.txtProductType.TabIndex = 2;
            // 
            // txtBuildRuleID
            // 
            this.txtBuildRuleID.Location = new System.Drawing.Point(83, 24);
            this.txtBuildRuleID.Name = "txtBuildRuleID";
            this.txtBuildRuleID.ReadOnly = true;
            this.txtBuildRuleID.Size = new System.Drawing.Size(102, 23);
            this.txtBuildRuleID.TabIndex = 2;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 32);
            this.label2.TabIndex = 1;
            this.label2.Text = "条码规则编号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 236);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1028, 319);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "明细显示";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.打印编号,
            this.打印规则编号,
            this.产品型号,
            this.打印起始编号,
            this.打印结束编号,
            this.打印份数,
            this.打印模式,
            this.产品日期,
            this.条形码构建规则,
            this.条形码示例});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 19);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.RowHeadersWidth = 20;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1022, 297);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // 序号
            // 
            this.序号.DataPropertyName = "序号";
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.ReadOnly = true;
            this.序号.Width = 40;
            // 
            // 打印编号
            // 
            this.打印编号.DataPropertyName = "打印编号";
            this.打印编号.HeaderText = "打印编号";
            this.打印编号.Name = "打印编号";
            this.打印编号.ReadOnly = true;
            this.打印编号.Visible = false;
            // 
            // 打印规则编号
            // 
            this.打印规则编号.DataPropertyName = "打印规则编号";
            this.打印规则编号.HeaderText = "打印规则编号";
            this.打印规则编号.Name = "打印规则编号";
            this.打印规则编号.ReadOnly = true;
            this.打印规则编号.Visible = false;
            // 
            // 产品型号
            // 
            this.产品型号.HeaderText = "产品型号";
            this.产品型号.Name = "产品型号";
            this.产品型号.ReadOnly = true;
            // 
            // 打印起始编号
            // 
            this.打印起始编号.HeaderText = "打印起始编号";
            this.打印起始编号.Name = "打印起始编号";
            this.打印起始编号.ReadOnly = true;
            // 
            // 打印结束编号
            // 
            this.打印结束编号.HeaderText = "打印结束编号";
            this.打印结束编号.Name = "打印结束编号";
            this.打印结束编号.ReadOnly = true;
            // 
            // 打印份数
            // 
            this.打印份数.HeaderText = "打印份数";
            this.打印份数.Name = "打印份数";
            this.打印份数.ReadOnly = true;
            // 
            // 打印模式
            // 
            this.打印模式.HeaderText = "打印模式";
            this.打印模式.Name = "打印模式";
            this.打印模式.ReadOnly = true;
            // 
            // 产品日期
            // 
            this.产品日期.HeaderText = "产品日期";
            this.产品日期.Name = "产品日期";
            this.产品日期.ReadOnly = true;
            // 
            // 条形码构建规则
            // 
            this.条形码构建规则.HeaderText = "条形码构建规则";
            this.条形码构建规则.Name = "条形码构建规则";
            this.条形码构建规则.ReadOnly = true;
            this.条形码构建规则.Width = 150;
            // 
            // 条形码示例
            // 
            this.条形码示例.HeaderText = "条形码示例";
            this.条形码示例.Name = "条形码示例";
            this.条形码示例.ReadOnly = true;
            this.条形码示例.Width = 150;
            // 
            // 打印CVT出厂条形码
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1028, 555);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "打印CVT出厂条形码";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印CVT出厂条形码";
            this.Load += new System.EventHandler(this.打印CVT出厂条形码_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPrintAmount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numEndNumber)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBeginNumber)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnSh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPrintRemark;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.TextBox txtBuildRuleID;
        private System.Windows.Forms.Button btnFindRule;
        private System.Windows.Forms.NumericUpDown numBeginNumber;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numEndNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbPrintMode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numPrintAmount;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 打印编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 打印规则编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 打印起始编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 打印结束编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 打印份数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 打印模式;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 条形码构建规则;
        private System.Windows.Forms.DataGridViewTextBoxColumn 条形码示例;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton btnPrintSelectedList;
    }
}