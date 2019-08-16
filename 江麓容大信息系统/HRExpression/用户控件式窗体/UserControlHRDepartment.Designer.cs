namespace Form_Peripheral_HR
{
    partial class UserControlHRDepartment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlHRDepartment));
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.部门类别toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.label13 = new System.Windows.Forms.Label();
            this.txtFatherCode = new System.Windows.Forms.TextBox();
            this.cbIsPermission = new System.Windows.Forms.CheckBox();
            this.cmbLeader = new System.Windows.Forms.ComboBox();
            this.btnLeader = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbDirector = new System.Windows.Forms.ComboBox();
            this.btnDirector = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.numOrder = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbPrincipal = new System.Windows.Forms.ComboBox();
            this.btnChoose = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbDeptType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFax = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTelephone = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panel2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "File2.png");
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
            this.toolStripSeparator1,
            this.部门类别toolStripButton1,
            this.toolStripSeparator2,
            this.刷新toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(818, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(52, 22);
            this.btnAdd.Tag = "ADD";
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
            // 
            // btnUpdate
            // 
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.btnUpdate.Size = new System.Drawing.Size(44, 22);
            this.btnUpdate.Tag = "Update";
            this.btnUpdate.Text = "修  改";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 部门类别toolStripButton1
            // 
            this.部门类别toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.部门类别toolStripButton1.Name = "部门类别toolStripButton1";
            this.部门类别toolStripButton1.Size = new System.Drawing.Size(84, 22);
            this.部门类别toolStripButton1.Text = "部门类别操作";
            this.部门类别toolStripButton1.Click += new System.EventHandler(this.部门类别toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 刷新toolStripButton1
            // 
            this.刷新toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton1.Name = "刷新toolStripButton1";
            this.刷新toolStripButton1.Size = new System.Drawing.Size(60, 22);
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panel2);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Controls.Add(this.toolStrip1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(818, 629);
            this.panelMain.TabIndex = 29;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitContainer1);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 74);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(818, 555);
            this.panel2.TabIndex = 25;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(122, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.label13);
            this.splitContainer1.Panel2.Controls.Add(this.txtFatherCode);
            this.splitContainer1.Panel2.Controls.Add(this.cbIsPermission);
            this.splitContainer1.Panel2.Controls.Add(this.cmbLeader);
            this.splitContainer1.Panel2.Controls.Add(this.btnLeader);
            this.splitContainer1.Panel2.Controls.Add(this.label12);
            this.splitContainer1.Panel2.Controls.Add(this.cmbDirector);
            this.splitContainer1.Panel2.Controls.Add(this.btnDirector);
            this.splitContainer1.Panel2.Controls.Add(this.label11);
            this.splitContainer1.Panel2.Controls.Add(this.numOrder);
            this.splitContainer1.Panel2.Controls.Add(this.label10);
            this.splitContainer1.Panel2.Controls.Add(this.cmbPrincipal);
            this.splitContainer1.Panel2.Controls.Add(this.btnChoose);
            this.splitContainer1.Panel2.Controls.Add(this.label9);
            this.splitContainer1.Panel2.Controls.Add(this.label8);
            this.splitContainer1.Panel2.Controls.Add(this.cmbDeptType);
            this.splitContainer1.Panel2.Controls.Add(this.label7);
            this.splitContainer1.Panel2.Controls.Add(this.txtRemark);
            this.splitContainer1.Panel2.Controls.Add(this.label6);
            this.splitContainer1.Panel2.Controls.Add(this.txtFax);
            this.splitContainer1.Panel2.Controls.Add(this.label5);
            this.splitContainer1.Panel2.Controls.Add(this.txtTelephone);
            this.splitContainer1.Panel2.Controls.Add(this.label4);
            this.splitContainer1.Panel2.Controls.Add(this.label1);
            this.splitContainer1.Panel2.Controls.Add(this.label2);
            this.splitContainer1.Panel2.Controls.Add(this.txtName);
            this.splitContainer1.Panel2.Controls.Add(this.label3);
            this.splitContainer1.Panel2.Controls.Add(this.txtCode);
            this.splitContainer1.Size = new System.Drawing.Size(696, 555);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 1;
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(250, 555);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.Color.Blue;
            this.label13.Location = new System.Drawing.Point(47, 119);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 63;
            this.label13.Text = "父部门编码";
            // 
            // txtFatherCode
            // 
            this.txtFatherCode.Location = new System.Drawing.Point(130, 115);
            this.txtFatherCode.MaxLength = 25;
            this.txtFatherCode.Name = "txtFatherCode";
            this.txtFatherCode.Size = new System.Drawing.Size(150, 23);
            this.txtFatherCode.TabIndex = 64;
            this.txtFatherCode.Tag = "";
            // 
            // cbIsPermission
            // 
            this.cbIsPermission.AutoSize = true;
            this.cbIsPermission.ForeColor = System.Drawing.Color.Blue;
            this.cbIsPermission.Location = new System.Drawing.Point(351, 152);
            this.cbIsPermission.Name = "cbIsPermission";
            this.cbIsPermission.Size = new System.Drawing.Size(82, 18);
            this.cbIsPermission.TabIndex = 62;
            this.cbIsPermission.Tag = "Update";
            this.cbIsPermission.Text = "有审批权";
            this.cbIsPermission.UseVisualStyleBackColor = true;
            // 
            // cmbLeader
            // 
            this.cmbLeader.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLeader.FormattingEnabled = true;
            this.cmbLeader.Location = new System.Drawing.Point(130, 216);
            this.cmbLeader.Name = "cmbLeader";
            this.cmbLeader.Size = new System.Drawing.Size(150, 21);
            this.cmbLeader.TabIndex = 61;
            // 
            // btnLeader
            // 
            this.btnLeader.Location = new System.Drawing.Point(289, 215);
            this.btnLeader.Name = "btnLeader";
            this.btnLeader.Size = new System.Drawing.Size(56, 25);
            this.btnLeader.TabIndex = 60;
            this.btnLeader.Tag = "Update";
            this.btnLeader.Text = "选择";
            this.btnLeader.UseVisualStyleBackColor = true;
            this.btnLeader.Click += new System.EventHandler(this.btnLeader_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.MediumBlue;
            this.label12.Location = new System.Drawing.Point(47, 219);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 59;
            this.label12.Text = "分管领导";
            // 
            // cmbDirector
            // 
            this.cmbDirector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDirector.FormattingEnabled = true;
            this.cmbDirector.Location = new System.Drawing.Point(130, 148);
            this.cmbDirector.Name = "cmbDirector";
            this.cmbDirector.Size = new System.Drawing.Size(150, 21);
            this.cmbDirector.TabIndex = 58;
            // 
            // btnDirector
            // 
            this.btnDirector.Location = new System.Drawing.Point(289, 148);
            this.btnDirector.Name = "btnDirector";
            this.btnDirector.Size = new System.Drawing.Size(56, 25);
            this.btnDirector.TabIndex = 57;
            this.btnDirector.Tag = "Update";
            this.btnDirector.Text = "选择";
            this.btnDirector.UseVisualStyleBackColor = true;
            this.btnDirector.Click += new System.EventHandler(this.btnDirector_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.MediumBlue;
            this.label11.Location = new System.Drawing.Point(47, 153);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 14);
            this.label11.TabIndex = 56;
            this.label11.Text = "主    管";
            // 
            // numOrder
            // 
            this.numOrder.Location = new System.Drawing.Point(130, 282);
            this.numOrder.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numOrder.Name = "numOrder";
            this.numOrder.Size = new System.Drawing.Size(150, 23);
            this.numOrder.TabIndex = 55;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(47, 285);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 54;
            this.label10.Text = "部门序号";
            // 
            // cmbPrincipal
            // 
            this.cmbPrincipal.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrincipal.FormattingEnabled = true;
            this.cmbPrincipal.Location = new System.Drawing.Point(130, 183);
            this.cmbPrincipal.Name = "cmbPrincipal";
            this.cmbPrincipal.Size = new System.Drawing.Size(150, 21);
            this.cmbPrincipal.TabIndex = 53;
            // 
            // btnChoose
            // 
            this.btnChoose.Location = new System.Drawing.Point(289, 182);
            this.btnChoose.Name = "btnChoose";
            this.btnChoose.Size = new System.Drawing.Size(56, 25);
            this.btnChoose.TabIndex = 52;
            this.btnChoose.Tag = "Update";
            this.btnChoose.Text = "选择";
            this.btnChoose.UseVisualStyleBackColor = true;
            this.btnChoose.Click += new System.EventHandler(this.btnChoose_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Red;
            this.label9.Location = new System.Drawing.Point(118, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(175, 14);
            this.label9.TabIndex = 50;
            this.label9.Text = "修改时不允许变更部门编码";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Red;
            this.label8.Location = new System.Drawing.Point(47, 420);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(133, 14);
            this.label8.TabIndex = 49;
            this.label8.Text = "编码规则：** ** **";
            // 
            // cmbDeptType
            // 
            this.cmbDeptType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDeptType.FormattingEnabled = true;
            this.cmbDeptType.Location = new System.Drawing.Point(130, 249);
            this.cmbDeptType.Name = "cmbDeptType";
            this.cmbDeptType.Size = new System.Drawing.Size(150, 21);
            this.cmbDeptType.TabIndex = 40;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 384);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 48;
            this.label7.Text = "备    注";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(130, 381);
            this.txtRemark.MaxLength = 25;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(150, 23);
            this.txtRemark.TabIndex = 43;
            this.txtRemark.Tag = "";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 351);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 47;
            this.label6.Text = "传    真";
            // 
            // txtFax
            // 
            this.txtFax.Location = new System.Drawing.Point(130, 348);
            this.txtFax.MaxLength = 25;
            this.txtFax.Name = "txtFax";
            this.txtFax.Size = new System.Drawing.Size(150, 23);
            this.txtFax.TabIndex = 42;
            this.txtFax.Tag = "";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 318);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 46;
            this.label5.Text = "电    话";
            // 
            // txtTelephone
            // 
            this.txtTelephone.Location = new System.Drawing.Point(130, 315);
            this.txtTelephone.MaxLength = 25;
            this.txtTelephone.Name = "txtTelephone";
            this.txtTelephone.Size = new System.Drawing.Size(150, 23);
            this.txtTelephone.TabIndex = 41;
            this.txtTelephone.Tag = "";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(47, 252);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 45;
            this.label4.Text = "部门类型";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.MediumBlue;
            this.label1.Location = new System.Drawing.Point(47, 186);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 44;
            this.label1.Text = "负 责 人";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(47, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 34;
            this.label2.Text = "部门名称";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(130, 82);
            this.txtName.MaxLength = 25;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(150, 23);
            this.txtName.TabIndex = 36;
            this.txtName.Tag = "";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(47, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 37;
            this.label3.Text = "部门编码";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(130, 27);
            this.txtCode.MaxLength = 25;
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(150, 23);
            this.txtCode.TabIndex = 35;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(122, 555);
            this.panel3.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(818, 49);
            this.panel1.TabIndex = 24;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(343, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "组织机构";
            // 
            // UserControlHRDepartment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(818, 629);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlHRDepartment";
            this.Load += new System.EventHandler(this.UserControlHRDepartment_Load);
            this.Resize += new System.EventHandler(this.UserControlHRDepartment_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOrder)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbDeptType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtFax;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtTelephone;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.ToolStripButton 部门类别toolStripButton1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Button btnChoose;
        private System.Windows.Forms.ComboBox cmbPrincipal;
        private System.Windows.Forms.NumericUpDown numOrder;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbDirector;
        private System.Windows.Forms.Button btnDirector;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox cmbLeader;
        private System.Windows.Forms.Button btnLeader;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cbIsPermission;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtFatherCode;
    }
}
