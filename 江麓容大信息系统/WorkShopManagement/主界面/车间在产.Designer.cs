namespace Form_Manufacture_WorkShop
{
    partial class 车间在产
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.BOM = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtSpec_BOM = new UniversalControlLibrary.TextBoxShow();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGoodsName_BOM = new UniversalControlLibrary.TextBoxShow();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGoodsCode_BOM = new UniversalControlLibrary.TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnInput_BOM = new System.Windows.Forms.Button();
            this.btnOutput_BOM = new System.Windows.Forms.Button();
            this.btnSelect_BOM = new System.Windows.Forms.Button();
            this.在产报表 = new System.Windows.Forms.TabPage();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnResolve_InProduct = new System.Windows.Forms.Button();
            this.btnSupplementary_InProduct = new System.Windows.Forms.Button();
            this.btnCreate_InProduct = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.btnOutput_InProduct = new System.Windows.Forms.Button();
            this.btnSelect_InProduct = new System.Windows.Forms.Button();
            this.tabControlMain = new System.Windows.Forms.TabControl();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.tcInProduct = new System.Windows.Forms.TabControl();
            this.车间在产报表 = new System.Windows.Forms.TabPage();
            this.dgvInProduct = new UniversalControlLibrary.CustomDataGridView();
            this.ucdlInProduct = new UniversalControlLibrary.UserControlDataLocalizer();
            this.盘点辅助报表 = new System.Windows.Forms.TabPage();
            this.dgvSupplementary = new UniversalControlLibrary.CustomDataGridView();
            this.ucdlSupplementary = new UniversalControlLibrary.UserControlDataLocalizer();
            this.零件供应商耗用报表 = new System.Windows.Forms.TabPage();
            this.dgvResolve = new UniversalControlLibrary.CustomDataGridView();
            this.ucdlResolve = new UniversalControlLibrary.UserControlDataLocalizer();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgvbom = new UniversalControlLibrary.CustomDataGridView();
            this.usdlbom = new UniversalControlLibrary.UserControlDataLocalizer();
            this.物品ID2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.年月2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.年月1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.年月3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.总成ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.BOM.SuspendLayout();
            this.panel2.SuspendLayout();
            this.在产报表.SuspendLayout();
            this.panel3.SuspendLayout();
            this.tabControlMain.SuspendLayout();
            this.customGroupBox1.SuspendLayout();
            this.tcInProduct.SuspendLayout();
            this.车间在产报表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvInProduct)).BeginInit();
            this.盘点辅助报表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplementary)).BeginInit();
            this.零件供应商耗用报表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvResolve)).BeginInit();
            this.customGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvbom)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 49);
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
            this.labelTitle.Text = "车间在产";
            // 
            // BOM
            // 
            this.BOM.Controls.Add(this.dgvbom);
            this.BOM.Controls.Add(this.usdlbom);
            this.BOM.Controls.Add(this.customGroupBox2);
            this.BOM.Location = new System.Drawing.Point(4, 22);
            this.BOM.Name = "BOM";
            this.BOM.Size = new System.Drawing.Size(1000, 605);
            this.BOM.TabIndex = 2;
            this.BOM.Text = "BOM";
            this.BOM.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtSpec_BOM);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtGoodsName_BOM);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtGoodsCode_BOM);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.btnInput_BOM);
            this.panel2.Controls.Add(this.btnOutput_BOM);
            this.panel2.Controls.Add(this.btnSelect_BOM);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 17);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(994, 109);
            this.panel2.TabIndex = 0;
            // 
            // txtSpec_BOM
            // 
            this.txtSpec_BOM.DataResult = null;
            this.txtSpec_BOM.DataTableResult = null;
            this.txtSpec_BOM.EditingControlDataGridView = null;
            this.txtSpec_BOM.EditingControlFormattedValue = "";
            this.txtSpec_BOM.EditingControlRowIndex = 0;
            this.txtSpec_BOM.EditingControlValueChanged = true;
            this.txtSpec_BOM.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtSpec_BOM.IsMultiSelect = false;
            this.txtSpec_BOM.Location = new System.Drawing.Point(325, 76);
            this.txtSpec_BOM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSpec_BOM.Name = "txtSpec_BOM";
            this.txtSpec_BOM.ReadOnly = true;
            this.txtSpec_BOM.ShowResultForm = false;
            this.txtSpec_BOM.Size = new System.Drawing.Size(175, 21);
            this.txtSpec_BOM.StrEndSql = null;
            this.txtSpec_BOM.TabIndex = 11;
            this.txtSpec_BOM.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(254, 81);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 10;
            this.label4.Text = "规    格";
            // 
            // txtGoodsName_BOM
            // 
            this.txtGoodsName_BOM.DataResult = null;
            this.txtGoodsName_BOM.DataTableResult = null;
            this.txtGoodsName_BOM.EditingControlDataGridView = null;
            this.txtGoodsName_BOM.EditingControlFormattedValue = "";
            this.txtGoodsName_BOM.EditingControlRowIndex = 0;
            this.txtGoodsName_BOM.EditingControlValueChanged = true;
            this.txtGoodsName_BOM.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtGoodsName_BOM.IsMultiSelect = false;
            this.txtGoodsName_BOM.Location = new System.Drawing.Point(325, 42);
            this.txtGoodsName_BOM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsName_BOM.Name = "txtGoodsName_BOM";
            this.txtGoodsName_BOM.ReadOnly = true;
            this.txtGoodsName_BOM.ShowResultForm = false;
            this.txtGoodsName_BOM.Size = new System.Drawing.Size(175, 21);
            this.txtGoodsName_BOM.StrEndSql = null;
            this.txtGoodsName_BOM.TabIndex = 9;
            this.txtGoodsName_BOM.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(254, 47);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 8;
            this.label3.Text = "名    称";
            // 
            // txtGoodsCode_BOM
            // 
            this.txtGoodsCode_BOM.DataResult = null;
            this.txtGoodsCode_BOM.DataTableResult = null;
            this.txtGoodsCode_BOM.EditingControlDataGridView = null;
            this.txtGoodsCode_BOM.EditingControlFormattedValue = "";
            this.txtGoodsCode_BOM.EditingControlRowIndex = 0;
            this.txtGoodsCode_BOM.EditingControlValueChanged = true;
            this.txtGoodsCode_BOM.FindItem = UniversalControlLibrary.TextBoxShow.FindType.营销物品;
            this.txtGoodsCode_BOM.IsMultiSelect = false;
            this.txtGoodsCode_BOM.Location = new System.Drawing.Point(325, 11);
            this.txtGoodsCode_BOM.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode_BOM.Name = "txtGoodsCode_BOM";
            this.txtGoodsCode_BOM.ShowResultForm = true;
            this.txtGoodsCode_BOM.Size = new System.Drawing.Size(175, 21);
            this.txtGoodsCode_BOM.StrEndSql = null;
            this.txtGoodsCode_BOM.TabIndex = 7;
            this.txtGoodsCode_BOM.TabStop = false;
            this.txtGoodsCode_BOM.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_BOM_OnCompleteSearch);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(254, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 6;
            this.label2.Text = "图号型号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(148, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 12);
            this.label1.TabIndex = 5;
            this.label1.Text = "总成/分总成：";
            // 
            // btnInput_BOM
            // 
            this.btnInput_BOM.Location = new System.Drawing.Point(772, 40);
            this.btnInput_BOM.Name = "btnInput_BOM";
            this.btnInput_BOM.Size = new System.Drawing.Size(75, 23);
            this.btnInput_BOM.TabIndex = 4;
            this.btnInput_BOM.Text = "导入";
            this.btnInput_BOM.UseVisualStyleBackColor = true;
            this.btnInput_BOM.Click += new System.EventHandler(this.btnInput_BOM_Click);
            // 
            // btnOutput_BOM
            // 
            this.btnOutput_BOM.Location = new System.Drawing.Point(657, 40);
            this.btnOutput_BOM.Name = "btnOutput_BOM";
            this.btnOutput_BOM.Size = new System.Drawing.Size(75, 23);
            this.btnOutput_BOM.TabIndex = 3;
            this.btnOutput_BOM.Text = "导出";
            this.btnOutput_BOM.UseVisualStyleBackColor = true;
            this.btnOutput_BOM.Click += new System.EventHandler(this.btnOutput_BOM_Click);
            // 
            // btnSelect_BOM
            // 
            this.btnSelect_BOM.Location = new System.Drawing.Point(542, 40);
            this.btnSelect_BOM.Name = "btnSelect_BOM";
            this.btnSelect_BOM.Size = new System.Drawing.Size(75, 23);
            this.btnSelect_BOM.TabIndex = 2;
            this.btnSelect_BOM.Text = "查询";
            this.btnSelect_BOM.UseVisualStyleBackColor = true;
            this.btnSelect_BOM.Click += new System.EventHandler(this.btnSelect_BOM_Click);
            // 
            // 在产报表
            // 
            this.在产报表.Controls.Add(this.tcInProduct);
            this.在产报表.Controls.Add(this.customGroupBox1);
            this.在产报表.Location = new System.Drawing.Point(4, 22);
            this.在产报表.Name = "在产报表";
            this.在产报表.Padding = new System.Windows.Forms.Padding(3);
            this.在产报表.Size = new System.Drawing.Size(1000, 605);
            this.在产报表.TabIndex = 0;
            this.在产报表.Text = "在产报表";
            this.在产报表.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnResolve_InProduct);
            this.panel3.Controls.Add(this.btnSupplementary_InProduct);
            this.panel3.Controls.Add(this.btnCreate_InProduct);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.cmbMonth);
            this.panel3.Controls.Add(this.label6);
            this.panel3.Controls.Add(this.cmbYear);
            this.panel3.Controls.Add(this.btnOutput_InProduct);
            this.panel3.Controls.Add(this.btnSelect_InProduct);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 17);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(988, 107);
            this.panel3.TabIndex = 1;
            // 
            // btnResolve_InProduct
            // 
            this.btnResolve_InProduct.Location = new System.Drawing.Point(672, 20);
            this.btnResolve_InProduct.Name = "btnResolve_InProduct";
            this.btnResolve_InProduct.Size = new System.Drawing.Size(185, 23);
            this.btnResolve_InProduct.TabIndex = 175;
            this.btnResolve_InProduct.Text = "导入零件供应商耗用明细表";
            this.btnResolve_InProduct.UseVisualStyleBackColor = true;
            this.btnResolve_InProduct.Click += new System.EventHandler(this.btnResolve_InProduct_Click);
            // 
            // btnSupplementary_InProduct
            // 
            this.btnSupplementary_InProduct.Location = new System.Drawing.Point(402, 20);
            this.btnSupplementary_InProduct.Name = "btnSupplementary_InProduct";
            this.btnSupplementary_InProduct.Size = new System.Drawing.Size(185, 23);
            this.btnSupplementary_InProduct.TabIndex = 174;
            this.btnSupplementary_InProduct.Text = "导入实盘数";
            this.btnSupplementary_InProduct.UseVisualStyleBackColor = true;
            this.btnSupplementary_InProduct.Click += new System.EventHandler(this.btnSupplementary_InProduct_Click);
            // 
            // btnCreate_InProduct
            // 
            this.btnCreate_InProduct.Location = new System.Drawing.Point(132, 20);
            this.btnCreate_InProduct.Name = "btnCreate_InProduct";
            this.btnCreate_InProduct.Size = new System.Drawing.Size(185, 23);
            this.btnCreate_InProduct.TabIndex = 173;
            this.btnCreate_InProduct.Text = "生成在产报表";
            this.btnCreate_InProduct.UseVisualStyleBackColor = true;
            this.btnCreate_InProduct.Click += new System.EventHandler(this.btnCreate_InProduct_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(386, 69);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 172;
            this.label5.Text = "月份";
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
            this.cmbMonth.Location = new System.Drawing.Point(427, 65);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(76, 20);
            this.cmbMonth.TabIndex = 171;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(248, 69);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 12);
            this.label6.TabIndex = 170;
            this.label6.Text = "年份";
            // 
            // cmbYear
            // 
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(289, 65);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(76, 20);
            this.cmbYear.TabIndex = 169;
            // 
            // btnOutput_InProduct
            // 
            this.btnOutput_InProduct.Location = new System.Drawing.Point(653, 64);
            this.btnOutput_InProduct.Name = "btnOutput_InProduct";
            this.btnOutput_InProduct.Size = new System.Drawing.Size(75, 23);
            this.btnOutput_InProduct.TabIndex = 3;
            this.btnOutput_InProduct.Text = "导出";
            this.btnOutput_InProduct.UseVisualStyleBackColor = true;
            this.btnOutput_InProduct.Click += new System.EventHandler(this.btnOutput_InProduct_Click);
            // 
            // btnSelect_InProduct
            // 
            this.btnSelect_InProduct.Location = new System.Drawing.Point(555, 64);
            this.btnSelect_InProduct.Name = "btnSelect_InProduct";
            this.btnSelect_InProduct.Size = new System.Drawing.Size(75, 23);
            this.btnSelect_InProduct.TabIndex = 2;
            this.btnSelect_InProduct.Text = "查询";
            this.btnSelect_InProduct.UseVisualStyleBackColor = true;
            this.btnSelect_InProduct.Click += new System.EventHandler(this.btnSelect_InProduct_Click);
            // 
            // tabControlMain
            // 
            this.tabControlMain.Controls.Add(this.在产报表);
            this.tabControlMain.Controls.Add(this.BOM);
            this.tabControlMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControlMain.Location = new System.Drawing.Point(0, 49);
            this.tabControlMain.Name = "tabControlMain";
            this.tabControlMain.SelectedIndex = 0;
            this.tabControlMain.Size = new System.Drawing.Size(1008, 631);
            this.tabControlMain.TabIndex = 26;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.BackColor = System.Drawing.Color.White;
            this.customGroupBox1.Controls.Add(this.panel3);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(3, 3);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(994, 127);
            this.customGroupBox1.TabIndex = 176;
            this.customGroupBox1.TabStop = false;
            // 
            // tcInProduct
            // 
            this.tcInProduct.Controls.Add(this.车间在产报表);
            this.tcInProduct.Controls.Add(this.盘点辅助报表);
            this.tcInProduct.Controls.Add(this.零件供应商耗用报表);
            this.tcInProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcInProduct.Location = new System.Drawing.Point(3, 130);
            this.tcInProduct.Name = "tcInProduct";
            this.tcInProduct.SelectedIndex = 0;
            this.tcInProduct.Size = new System.Drawing.Size(994, 472);
            this.tcInProduct.TabIndex = 177;
            this.tcInProduct.SelectedIndexChanged += new System.EventHandler(this.tcInProduct_SelectedIndexChanged);
            // 
            // 车间在产报表
            // 
            this.车间在产报表.Controls.Add(this.dgvInProduct);
            this.车间在产报表.Controls.Add(this.ucdlInProduct);
            this.车间在产报表.Location = new System.Drawing.Point(4, 22);
            this.车间在产报表.Name = "车间在产报表";
            this.车间在产报表.Padding = new System.Windows.Forms.Padding(3);
            this.车间在产报表.Size = new System.Drawing.Size(986, 446);
            this.车间在产报表.TabIndex = 0;
            this.车间在产报表.Text = "车间在产报表";
            this.车间在产报表.UseVisualStyleBackColor = true;
            // 
            // dgvInProduct
            // 
            this.dgvInProduct.AllowUserToAddRows = false;
            this.dgvInProduct.AllowUserToDeleteRows = false;
            this.dgvInProduct.AllowUserToResizeRows = false;
            this.dgvInProduct.AutoCreateFilters = true;
            this.dgvInProduct.BaseFilter = "";
            this.dgvInProduct.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvInProduct.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.物品ID1,
            this.年月1});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvInProduct.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvInProduct.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvInProduct.Location = new System.Drawing.Point(3, 35);
            this.dgvInProduct.Name = "dgvInProduct";
            this.dgvInProduct.RowTemplate.Height = 23;
            this.dgvInProduct.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvInProduct.Size = new System.Drawing.Size(980, 408);
            this.dgvInProduct.TabIndex = 36;
            // 
            // ucdlInProduct
            // 
            this.ucdlInProduct.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucdlInProduct.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucdlInProduct.Location = new System.Drawing.Point(3, 3);
            this.ucdlInProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucdlInProduct.Name = "ucdlInProduct";
            this.ucdlInProduct.OnlyLocalize = false;
            this.ucdlInProduct.Size = new System.Drawing.Size(980, 32);
            this.ucdlInProduct.StartIndex = 0;
            this.ucdlInProduct.TabIndex = 35;
            // 
            // 盘点辅助报表
            // 
            this.盘点辅助报表.Controls.Add(this.dgvSupplementary);
            this.盘点辅助报表.Controls.Add(this.ucdlSupplementary);
            this.盘点辅助报表.Location = new System.Drawing.Point(4, 22);
            this.盘点辅助报表.Name = "盘点辅助报表";
            this.盘点辅助报表.Padding = new System.Windows.Forms.Padding(3);
            this.盘点辅助报表.Size = new System.Drawing.Size(986, 446);
            this.盘点辅助报表.TabIndex = 1;
            this.盘点辅助报表.Text = "盘点辅助报表";
            this.盘点辅助报表.UseVisualStyleBackColor = true;
            // 
            // dgvSupplementary
            // 
            this.dgvSupplementary.AllowUserToAddRows = false;
            this.dgvSupplementary.AllowUserToDeleteRows = false;
            this.dgvSupplementary.AllowUserToResizeRows = false;
            this.dgvSupplementary.AutoCreateFilters = true;
            this.dgvSupplementary.BaseFilter = "";
            this.dgvSupplementary.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSupplementary.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.物品ID2,
            this.年月2});
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvSupplementary.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvSupplementary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSupplementary.Location = new System.Drawing.Point(3, 35);
            this.dgvSupplementary.Name = "dgvSupplementary";
            this.dgvSupplementary.RowTemplate.Height = 23;
            this.dgvSupplementary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSupplementary.Size = new System.Drawing.Size(980, 408);
            this.dgvSupplementary.TabIndex = 36;
            // 
            // ucdlSupplementary
            // 
            this.ucdlSupplementary.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucdlSupplementary.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucdlSupplementary.Location = new System.Drawing.Point(3, 3);
            this.ucdlSupplementary.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucdlSupplementary.Name = "ucdlSupplementary";
            this.ucdlSupplementary.OnlyLocalize = false;
            this.ucdlSupplementary.Size = new System.Drawing.Size(980, 32);
            this.ucdlSupplementary.StartIndex = 0;
            this.ucdlSupplementary.TabIndex = 35;
            // 
            // 零件供应商耗用报表
            // 
            this.零件供应商耗用报表.Controls.Add(this.dgvResolve);
            this.零件供应商耗用报表.Controls.Add(this.ucdlResolve);
            this.零件供应商耗用报表.Location = new System.Drawing.Point(4, 22);
            this.零件供应商耗用报表.Name = "零件供应商耗用报表";
            this.零件供应商耗用报表.Size = new System.Drawing.Size(986, 446);
            this.零件供应商耗用报表.TabIndex = 3;
            this.零件供应商耗用报表.Text = "零件供应商耗用报表";
            this.零件供应商耗用报表.UseVisualStyleBackColor = true;
            // 
            // dgvResolve
            // 
            this.dgvResolve.AllowUserToAddRows = false;
            this.dgvResolve.AllowUserToDeleteRows = false;
            this.dgvResolve.AllowUserToResizeRows = false;
            this.dgvResolve.AutoCreateFilters = true;
            this.dgvResolve.BaseFilter = "";
            this.dgvResolve.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvResolve.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.物品ID3,
            this.年月3});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvResolve.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvResolve.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvResolve.Location = new System.Drawing.Point(0, 32);
            this.dgvResolve.Name = "dgvResolve";
            this.dgvResolve.RowTemplate.Height = 23;
            this.dgvResolve.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvResolve.Size = new System.Drawing.Size(986, 414);
            this.dgvResolve.TabIndex = 36;
            // 
            // ucdlResolve
            // 
            this.ucdlResolve.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucdlResolve.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucdlResolve.Location = new System.Drawing.Point(0, 0);
            this.ucdlResolve.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucdlResolve.Name = "ucdlResolve";
            this.ucdlResolve.OnlyLocalize = false;
            this.ucdlResolve.Size = new System.Drawing.Size(986, 32);
            this.ucdlResolve.StartIndex = 0;
            this.ucdlResolve.TabIndex = 35;
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.BackColor = System.Drawing.Color.White;
            this.customGroupBox2.Controls.Add(this.panel2);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(1000, 129);
            this.customGroupBox2.TabIndex = 1;
            this.customGroupBox2.TabStop = false;
            // 
            // dgvbom
            // 
            this.dgvbom.AllowUserToAddRows = false;
            this.dgvbom.AllowUserToDeleteRows = false;
            this.dgvbom.AllowUserToResizeRows = false;
            this.dgvbom.AutoCreateFilters = true;
            this.dgvbom.BaseFilter = "";
            this.dgvbom.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvbom.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.总成ID,
            this.物品ID4});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvbom.DefaultCellStyle = dataGridViewCellStyle1;
            this.dgvbom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvbom.Location = new System.Drawing.Point(0, 161);
            this.dgvbom.Name = "dgvbom";
            this.dgvbom.RowTemplate.Height = 23;
            this.dgvbom.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvbom.Size = new System.Drawing.Size(1000, 444);
            this.dgvbom.TabIndex = 36;
            // 
            // usdlbom
            // 
            this.usdlbom.Dock = System.Windows.Forms.DockStyle.Top;
            this.usdlbom.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.usdlbom.Location = new System.Drawing.Point(0, 129);
            this.usdlbom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usdlbom.Name = "usdlbom";
            this.usdlbom.OnlyLocalize = false;
            this.usdlbom.Size = new System.Drawing.Size(1000, 32);
            this.usdlbom.StartIndex = 0;
            this.usdlbom.TabIndex = 35;
            // 
            // 物品ID2
            // 
            this.物品ID2.DataPropertyName = "物品ID";
            this.物品ID2.HeaderText = "物品ID2";
            this.物品ID2.Name = "物品ID2";
            this.物品ID2.Visible = false;
            // 
            // 年月2
            // 
            this.年月2.DataPropertyName = "年月";
            this.年月2.HeaderText = "年月2";
            this.年月2.Name = "年月2";
            this.年月2.Visible = false;
            // 
            // 物品ID1
            // 
            this.物品ID1.DataPropertyName = "物品ID";
            this.物品ID1.HeaderText = "物品ID1";
            this.物品ID1.Name = "物品ID1";
            this.物品ID1.Visible = false;
            // 
            // 年月1
            // 
            this.年月1.DataPropertyName = "年月";
            this.年月1.HeaderText = "年月1";
            this.年月1.Name = "年月1";
            this.年月1.Visible = false;
            // 
            // 物品ID3
            // 
            this.物品ID3.DataPropertyName = "物品ID";
            this.物品ID3.HeaderText = "物品ID3";
            this.物品ID3.Name = "物品ID3";
            this.物品ID3.Visible = false;
            // 
            // 年月3
            // 
            this.年月3.DataPropertyName = "年月";
            this.年月3.HeaderText = "年月3";
            this.年月3.Name = "年月3";
            this.年月3.Visible = false;
            // 
            // 总成ID
            // 
            this.总成ID.DataPropertyName = "总成ID";
            this.总成ID.HeaderText = "总成ID";
            this.总成ID.Name = "总成ID";
            this.总成ID.Visible = false;
            // 
            // 物品ID4
            // 
            this.物品ID4.DataPropertyName = "物品ID";
            this.物品ID4.HeaderText = "物品ID4";
            this.物品ID4.Name = "物品ID4";
            this.物品ID4.Visible = false;
            // 
            // 车间在产
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 680);
            this.Controls.Add(this.tabControlMain);
            this.Controls.Add(this.panel1);
            this.Name = "车间在产";
            this.Text = "车间在产";
            this.Load += new System.EventHandler(this.车间在产_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.BOM.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.在产报表.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.tabControlMain.ResumeLayout(false);
            this.customGroupBox1.ResumeLayout(false);
            this.tcInProduct.ResumeLayout(false);
            this.车间在产报表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvInProduct)).EndInit();
            this.盘点辅助报表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvSupplementary)).EndInit();
            this.零件供应商耗用报表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvResolve)).EndInit();
            this.customGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvbom)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TabPage BOM;
        private System.Windows.Forms.Panel panel2;
        private UniversalControlLibrary.TextBoxShow txtSpec_BOM;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.TextBoxShow txtGoodsName_BOM;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode_BOM;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnInput_BOM;
        private System.Windows.Forms.Button btnOutput_BOM;
        private System.Windows.Forms.Button btnSelect_BOM;
        private System.Windows.Forms.TabPage 在产报表;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnResolve_InProduct;
        private System.Windows.Forms.Button btnSupplementary_InProduct;
        private System.Windows.Forms.Button btnCreate_InProduct;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnOutput_InProduct;
        private System.Windows.Forms.Button btnSelect_InProduct;
        private System.Windows.Forms.TabControl tabControlMain;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private UniversalControlLibrary.CustomDataGridView dgvbom;
        private UniversalControlLibrary.UserControlDataLocalizer usdlbom;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private System.Windows.Forms.TabControl tcInProduct;
        private System.Windows.Forms.TabPage 车间在产报表;
        private UniversalControlLibrary.CustomDataGridView dgvInProduct;
        private UniversalControlLibrary.UserControlDataLocalizer ucdlInProduct;
        private System.Windows.Forms.TabPage 盘点辅助报表;
        private UniversalControlLibrary.CustomDataGridView dgvSupplementary;
        private UniversalControlLibrary.UserControlDataLocalizer ucdlSupplementary;
        private System.Windows.Forms.TabPage 零件供应商耗用报表;
        private UniversalControlLibrary.CustomDataGridView dgvResolve;
        private UniversalControlLibrary.UserControlDataLocalizer ucdlResolve;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 年月1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 年月2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 年月3;
        private System.Windows.Forms.DataGridViewTextBoxColumn 总成ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID4;
    }
}