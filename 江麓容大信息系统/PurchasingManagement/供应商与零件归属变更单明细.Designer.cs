namespace Form_Economic_Purchase
{
    partial class 供应商与零件归属变更单明细
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件类型 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商等级 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.难度等级 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.说明 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cmbProviderLV = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPartType = new System.Windows.Forms.TextBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.txtExplain = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cmbDiffcultyLV = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProvider = new UniversalControlLibrary.TextBoxShow();
            this.label9 = new System.Windows.Forms.Label();
            this.txtChangeReason = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbChangeType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.customDataGridView1);
            this.customPanel1.Controls.Add(this.userControlDataLocalizer1);
            this.customPanel1.Controls.Add(this.groupBox2);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(892, 552);
            this.customPanel1.TabIndex = 0;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.单据号,
            this.零件类型,
            this.物品ID,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.供应商等级,
            this.难度等级,
            this.说明});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 280);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.Padding = new System.Windows.Forms.Padding(0, 6, 0, 6);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.customDataGridView1.RowsDefaultCellStyle = dataGridViewCellStyle2;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(892, 272);
            this.customDataGridView1.TabIndex = 16;
            this.customDataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellEnter);
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.ReadOnly = true;
            this.单据号.Visible = false;
            // 
            // 零件类型
            // 
            this.零件类型.DataPropertyName = "零件类型";
            this.零件类型.HeaderText = "零件类型";
            this.零件类型.Name = "零件类型";
            this.零件类型.ReadOnly = true;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "物品名称";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 供应商等级
            // 
            this.供应商等级.DataPropertyName = "供应商等级";
            this.供应商等级.HeaderText = "供应商等级";
            this.供应商等级.Name = "供应商等级";
            this.供应商等级.ReadOnly = true;
            // 
            // 难度等级
            // 
            this.难度等级.DataPropertyName = "难度等级";
            this.难度等级.HeaderText = "难度等级";
            this.难度等级.Name = "难度等级";
            this.难度等级.ReadOnly = true;
            // 
            // 说明
            // 
            this.说明.DataPropertyName = "说明";
            this.说明.HeaderText = "说明";
            this.说明.Name = "说明";
            this.说明.ReadOnly = true;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 248);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = true;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(892, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 15;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmbProviderLV);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtPartType);
            this.groupBox2.Controls.Add(this.btnAdd);
            this.groupBox2.Controls.Add(this.btnDelete);
            this.groupBox2.Controls.Add(this.btnModify);
            this.groupBox2.Controls.Add(this.txtExplain);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.cmbDiffcultyLV);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.txtGoodsCode);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtSpec);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtGoodsName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 122);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(892, 126);
            this.groupBox2.TabIndex = 14;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "明细";
            // 
            // cmbProviderLV
            // 
            this.cmbProviderLV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProviderLV.FormattingEnabled = true;
            this.cmbProviderLV.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.cmbProviderLV.Location = new System.Drawing.Point(84, 91);
            this.cmbProviderLV.Name = "cmbProviderLV";
            this.cmbProviderLV.Size = new System.Drawing.Size(155, 20);
            this.cmbProviderLV.TabIndex = 335;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(13, 95);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 334;
            this.label10.Text = "供应商等级";
            // 
            // txtPartType
            // 
            this.txtPartType.Location = new System.Drawing.Point(84, 24);
            this.txtPartType.Name = "txtPartType";
            this.txtPartType.Size = new System.Drawing.Size(219, 21);
            this.txtPartType.TabIndex = 333;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(611, 23);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 332;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(799, 23);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 331;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(705, 23);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 330;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // txtExplain
            // 
            this.txtExplain.Location = new System.Drawing.Point(668, 91);
            this.txtExplain.Name = "txtExplain";
            this.txtExplain.Size = new System.Drawing.Size(206, 21);
            this.txtExplain.TabIndex = 329;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(609, 95);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 328;
            this.label13.Text = "说    明";
            // 
            // cmbDiffcultyLV
            // 
            this.cmbDiffcultyLV.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiffcultyLV.FormattingEnabled = true;
            this.cmbDiffcultyLV.Items.AddRange(new object[] {
            "A",
            "B",
            "C",
            "D"});
            this.cmbDiffcultyLV.Location = new System.Drawing.Point(386, 91);
            this.cmbDiffcultyLV.Name = "cmbDiffcultyLV";
            this.cmbDiffcultyLV.Size = new System.Drawing.Size(155, 20);
            this.cmbDiffcultyLV.TabIndex = 327;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(327, 95);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 326;
            this.label12.Text = "难度等级";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.DataTableResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsCode.IsMultiSelect = false;
            this.txtGoodsCode.Location = new System.Drawing.Point(84, 58);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(219, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 319;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            this.txtGoodsCode.Enter += new System.EventHandler(this.txtGoodsCode_Enter);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(13, 62);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 318;
            this.label8.Text = "图号型号";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(609, 62);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 317;
            this.label7.Text = "规    格";
            // 
            // txtSpec
            // 
            this.txtSpec.Enabled = false;
            this.txtSpec.Location = new System.Drawing.Point(668, 58);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(206, 21);
            this.txtSpec.TabIndex = 316;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(327, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 315;
            this.label6.Text = "物品名称";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Enabled = false;
            this.txtGoodsName.Location = new System.Drawing.Point(386, 58);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.Size = new System.Drawing.Size(206, 21);
            this.txtGoodsName.TabIndex = 314;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 312;
            this.label3.Text = "零件类别";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtChangeReason);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbChangeType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(892, 122);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "单据";
            // 
            // txtProvider
            // 
            this.txtProvider.DataResult = null;
            this.txtProvider.DataTableResult = null;
            this.txtProvider.EditingControlDataGridView = null;
            this.txtProvider.EditingControlFormattedValue = "";
            this.txtProvider.EditingControlRowIndex = 0;
            this.txtProvider.EditingControlValueChanged = true;
            this.txtProvider.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.txtProvider.IsMultiSelect = false;
            this.txtProvider.Location = new System.Drawing.Point(668, 69);
            this.txtProvider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ShowResultForm = true;
            this.txtProvider.Size = new System.Drawing.Size(206, 21);
            this.txtProvider.StrEndSql = null;
            this.txtProvider.TabIndex = 315;
            this.txtProvider.TabStop = false;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(609, 74);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 314;
            this.label9.Text = "供 应 商";
            // 
            // txtChangeReason
            // 
            this.txtChangeReason.Location = new System.Drawing.Point(75, 55);
            this.txtChangeReason.Multiline = true;
            this.txtChangeReason.Name = "txtChangeReason";
            this.txtChangeReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtChangeReason.Size = new System.Drawing.Size(517, 47);
            this.txtChangeReason.TabIndex = 313;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 312;
            this.label2.Text = "变更原因";
            // 
            // cmbChangeType
            // 
            this.cmbChangeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChangeType.FormattingEnabled = true;
            this.cmbChangeType.Items.AddRange(new object[] {
            "准入",
            "等级变更",
            "淘汰"});
            this.cmbChangeType.Location = new System.Drawing.Point(668, 20);
            this.cmbChangeType.Name = "cmbChangeType";
            this.cmbChangeType.Size = new System.Drawing.Size(206, 20);
            this.cmbChangeType.TabIndex = 311;
            this.cmbChangeType.SelectedIndexChanged += new System.EventHandler(this.cmbChangeType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(609, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 310;
            this.label1.Text = "变更类型";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(403, 24);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 309;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(319, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 308;
            this.label5.Text = "业务状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(75, 21);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(219, 21);
            this.txtBillNo.TabIndex = 306;
            this.txtBillNo.Text = "PPC201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 307;
            this.label4.Text = "业务编号";
            // 
            // 供应商与零件归属变更单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 552);
            this.Controls.Add(this.customPanel1);
            this.Name = "供应商与零件归属变更单明细";
            this.Text = "供应商与零件归属变更单明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtChangeReason;
        private System.Windows.Forms.ComboBox cmbDiffcultyLV;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.TextBox txtExplain;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtPartType;
        private UniversalControlLibrary.TextBoxShow txtProvider;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbProviderLV;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbChangeType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件类型;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商等级;
        private System.Windows.Forms.DataGridViewTextBoxColumn 难度等级;
        private System.Windows.Forms.DataGridViewTextBoxColumn 说明;

    }
}