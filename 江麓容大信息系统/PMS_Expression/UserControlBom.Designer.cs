namespace Expression
{
    partial class UserControlBom
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
            this.numBasicCount = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorUpdate = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExportExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorSave = new System.Windows.Forms.ToolStripSeparator();
            this.btnCancle = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panelPara = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.cmbAssemblyFlag = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtParentCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuTreeNode = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmbSetAssemblyFlag = new System.Windows.Forms.ToolStripComboBox();
            this.panelTreeTop = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuDataGridView = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.复制数据表数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelSearch = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.numBasicCount)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuTreeNode.SuspendLayout();
            this.panelTreeTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuDataGridView.SuspendLayout();
            this.panelSearch.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // numBasicCount
            // 
            this.numBasicCount.BackColor = System.Drawing.Color.White;
            this.numBasicCount.Location = new System.Drawing.Point(598, 14);
            this.numBasicCount.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.numBasicCount.Name = "numBasicCount";
            this.numBasicCount.Size = new System.Drawing.Size(115, 23);
            this.numBasicCount.TabIndex = 159;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(528, 18);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(63, 14);
            this.label15.TabIndex = 54;
            this.label15.Text = "基    数";
            // 
            // cmbProductType
            // 
            this.cmbProductType.BackColor = System.Drawing.Color.White;
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Location = new System.Drawing.Point(78, 14);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(158, 21);
            this.cmbProductType.TabIndex = 153;
            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(466, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(54, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "BOM";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparatorAdd,
            this.btnDelete,
            this.toolStripSeparatorDelete,
            this.btnUpdate,
            this.toolStripSeparatorUpdate,
            this.btnSave,
            this.toolStripSeparator2,
            this.btnExportExcel,
            this.toolStripSeparatorSave,
            this.btnCancle,
            this.toolStripSeparator1,
            this.btnRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(986, 25);
            this.toolStrip1.TabIndex = 37;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(67, 22);
            this.btnAdd.Tag = "Update";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 22);
            this.btnDelete.Tag = "Update";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(67, 22);
            this.btnUpdate.Tag = "Update";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparatorUpdate
            // 
            this.toolStripSeparatorUpdate.Name = "toolStripSeparatorUpdate";
            this.toolStripSeparatorUpdate.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(49, 22);
            this.btnSave.Tag = "Update";
            this.btnSave.Text = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExportExcel
            // 
            this.btnExportExcel.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnExportExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExportExcel.Name = "btnExportExcel";
            this.btnExportExcel.Size = new System.Drawing.Size(79, 22);
            this.btnExportExcel.Tag = "ExportFile";
            this.btnExportExcel.Text = "EXCEL导出";
            this.btnExportExcel.Click += new System.EventHandler(this.btnExportExcel_Click);
            // 
            // toolStripSeparatorSave
            // 
            this.toolStripSeparatorSave.Name = "toolStripSeparatorSave";
            this.toolStripSeparatorSave.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCancle
            // 
            this.btnCancle.Image = global::UniversalControlLibrary.Properties.Resources.cancle;
            this.btnCancle.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(49, 22);
            this.btnCancle.Tag = "Update";
            this.btnCancle.Text = "取消";
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(49, 22);
            this.btnRefresh.Tag = "View";
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(986, 50);
            this.panel1.TabIndex = 24;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(9, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 152;
            this.label9.Text = "产品类型";
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Location = new System.Drawing.Point(598, 77);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(115, 23);
            this.txtSpec.TabIndex = 150;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(531, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 149;
            this.label7.Text = "规    格";
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 75);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(8, 552);
            this.panelLeft.TabIndex = 38;
            // 
            // panelPara
            // 
            this.panelPara.BackColor = System.Drawing.SystemColors.Control;
            this.panelPara.Controls.Add(this.label5);
            this.panelPara.Controls.Add(this.txtVersion);
            this.panelPara.Controls.Add(this.cmbAssemblyFlag);
            this.panelPara.Controls.Add(this.txtSpec);
            this.panelPara.Controls.Add(this.label7);
            this.panelPara.Controls.Add(this.label6);
            this.panelPara.Controls.Add(this.label1);
            this.panelPara.Controls.Add(this.numBasicCount);
            this.panelPara.Controls.Add(this.txtName);
            this.panelPara.Controls.Add(this.label15);
            this.panelPara.Controls.Add(this.cmbProductType);
            this.panelPara.Controls.Add(this.label9);
            this.panelPara.Controls.Add(this.label22);
            this.panelPara.Controls.Add(this.txtRemark);
            this.panelPara.Controls.Add(this.label12);
            this.panelPara.Controls.Add(this.txtCode);
            this.panelPara.Controls.Add(this.txtParentCode);
            this.panelPara.Controls.Add(this.label2);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 0);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(970, 110);
            this.panelPara.TabIndex = 16;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(754, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 163;
            this.label5.Text = "版 次 号";
            // 
            // txtVersion
            // 
            this.txtVersion.BackColor = System.Drawing.Color.White;
            this.txtVersion.Location = new System.Drawing.Point(820, 77);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtVersion.Size = new System.Drawing.Size(131, 23);
            this.txtVersion.TabIndex = 162;
            // 
            // cmbAssemblyFlag
            // 
            this.cmbAssemblyFlag.BackColor = System.Drawing.Color.White;
            this.cmbAssemblyFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssemblyFlag.FormattingEnabled = true;
            this.cmbAssemblyFlag.Items.AddRange(new object[] {
            "非总成",
            "总成"});
            this.cmbAssemblyFlag.Location = new System.Drawing.Point(355, 14);
            this.cmbAssemblyFlag.Name = "cmbAssemblyFlag";
            this.cmbAssemblyFlag.Size = new System.Drawing.Size(115, 21);
            this.cmbAssemblyFlag.TabIndex = 161;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(286, 18);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 160;
            this.label6.Text = "总成标志";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 81);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 44;
            this.label1.Text = "零件名称";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(78, 77);
            this.txtName.Name = "txtName";
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtName.Size = new System.Drawing.Size(391, 23);
            this.txtName.TabIndex = 47;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(754, 18);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(63, 14);
            this.label22.TabIndex = 67;
            this.label22.Text = "备    注";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Location = new System.Drawing.Point(820, 14);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(131, 23);
            this.txtRemark.TabIndex = 66;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(524, 51);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 14);
            this.label12.TabIndex = 46;
            this.label12.Text = "图号/型号";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(598, 45);
            this.txtCode.Name = "txtCode";
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(353, 23);
            this.txtCode.TabIndex = 45;
            // 
            // txtParentCode
            // 
            this.txtParentCode.BackColor = System.Drawing.Color.White;
            this.txtParentCode.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtParentCode.Location = new System.Drawing.Point(78, 45);
            this.txtParentCode.Name = "txtParentCode";
            this.txtParentCode.Size = new System.Drawing.Size(391, 23);
            this.txtParentCode.TabIndex = 36;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(4, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 37);
            this.label2.TabIndex = 0;
            this.label2.Text = " 父总成\r\n图号/型号";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.splitContainer1);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Controls.Add(this.panelPara);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(8, 75);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(970, 552);
            this.panelCenter.TabIndex = 0;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.Silver;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 110);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            this.splitContainer1.Panel1.Controls.Add(this.panelTreeTop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.dataGridView1);
            this.splitContainer1.Panel2.Controls.Add(this.panelSearch);
            this.splitContainer1.Size = new System.Drawing.Size(970, 431);
            this.splitContainer1.SplitterDistance = 317;
            this.splitContainer1.TabIndex = 28;
            // 
            // treeView1
            // 
            this.treeView1.ContextMenuStrip = this.contextMenuTreeNode;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(0, 52);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(317, 379);
            this.treeView1.TabIndex = 31;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // contextMenuTreeNode
            // 
            this.contextMenuTreeNode.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmbSetAssemblyFlag});
            this.contextMenuTreeNode.Name = "contextMenuTreeNode";
            this.contextMenuTreeNode.Size = new System.Drawing.Size(182, 28);
            // 
            // cmbSetAssemblyFlag
            // 
            this.cmbSetAssemblyFlag.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSetAssemblyFlag.Items.AddRange(new object[] {
            "非总成",
            "总成"});
            this.cmbSetAssemblyFlag.Name = "cmbSetAssemblyFlag";
            this.cmbSetAssemblyFlag.Size = new System.Drawing.Size(121, 20);
            // 
            // panelTreeTop
            // 
            this.panelTreeTop.Controls.Add(this.label3);
            this.panelTreeTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTreeTop.Location = new System.Drawing.Point(0, 0);
            this.panelTreeTop.Name = "panelTreeTop";
            this.panelTreeTop.Size = new System.Drawing.Size(317, 52);
            this.panelTreeTop.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 45;
            this.label3.Text = "BOM 树";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuDataGridView;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 52);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(649, 379);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // contextMenuDataGridView
            // 
            this.contextMenuDataGridView.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.复制数据表数据ToolStripMenuItem});
            this.contextMenuDataGridView.Name = "contextMenuDataGridView";
            this.contextMenuDataGridView.Size = new System.Drawing.Size(155, 26);
            // 
            // 复制数据表数据ToolStripMenuItem
            // 
            this.复制数据表数据ToolStripMenuItem.Name = "复制数据表数据ToolStripMenuItem";
            this.复制数据表数据ToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.复制数据表数据ToolStripMenuItem.Text = "复制数据表数据";
            this.复制数据表数据ToolStripMenuItem.Click += new System.EventHandler(this.复制数据表数据ToolStripMenuItem_Click);
            // 
            // panelSearch
            // 
            this.panelSearch.Controls.Add(this.label4);
            this.panelSearch.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelSearch.Location = new System.Drawing.Point(0, 0);
            this.panelSearch.Name = "panelSearch";
            this.panelSearch.Size = new System.Drawing.Size(649, 52);
            this.panelSearch.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(105, 14);
            this.label4.TabIndex = 45;
            this.label4.Text = "BOM 数据查询区";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 541);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(970, 11);
            this.panel2.TabIndex = 27;
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Controls.Add(this.toolStrip1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(986, 627);
            this.panelMain.TabIndex = 34;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(978, 75);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(8, 552);
            this.panelRight.TabIndex = 39;
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
            // UserControlBom
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 627);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlBom";
            this.Load += new System.EventHandler(this.UserControlBom_Load);
            this.Enter += new System.EventHandler(this.UserControlBom_Enter);
            this.Resize += new System.EventHandler(this.UserControlBom_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.numBasicCount)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.panelPara.PerformLayout();
            this.panelCenter.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuTreeNode.ResumeLayout(false);
            this.panelTreeTop.ResumeLayout(false);
            this.panelTreeTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuDataGridView.ResumeLayout(false);
            this.panelSearch.ResumeLayout(false);
            this.panelSearch.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numBasicCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorUpdate;
        private System.Windows.Forms.ToolStripButton btnCancle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtParentCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.ComboBox cmbAssemblyFlag;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorSave;
        private System.Windows.Forms.ContextMenuStrip contextMenuTreeNode;
        private System.Windows.Forms.ToolStripComboBox cmbSetAssemblyFlag;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panelTreeTop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelSearch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ContextMenuStrip contextMenuDataGridView;
        private System.Windows.Forms.ToolStripMenuItem 复制数据表数据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnExportExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtVersion;
    }
}
