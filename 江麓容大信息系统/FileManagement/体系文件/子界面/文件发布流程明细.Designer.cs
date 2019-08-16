using UniversalControlLibrary;
namespace Form_Quality_File
{
    partial class 文件发布流程明细
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(文件发布流程明细));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnAudit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnApprove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReback = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtReplaceFileNo = new UniversalControlLibrary.TextBoxShow();
            this.txtRepalceVersion = new System.Windows.Forms.TextBox();
            this.txtReplaceFileName = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.txtSortName = new UniversalControlLibrary.TextBoxShow();
            this.txtDepartment = new UniversalControlLibrary.TextBoxShow();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbPropoerTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbPropoer = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbSDBStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.llbProposerDownLoad = new System.Windows.Forms.LinkLabel();
            this.llbProposerUpLoad = new System.Windows.Forms.LinkLabel();
            this.txtFileNo = new System.Windows.Forms.TextBox();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSDBNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAuditorAdvise = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lbAuditorTime = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbAuditor = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtApproverAdvise = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbApproverTime = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbApprover = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparatorAdd,
            this.btnAudit,
            this.toolStripSeparatorDelete,
            this.btnApprove,
            this.toolStripSeparator4,
            this.btnReback});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(955, 25);
            this.toolStrip1.TabIndex = 48;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(115, 22);
            this.btnAdd.Tag = "";
            this.btnAdd.Text = "提交发布申请(&P)";
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAudit
            // 
            this.btnAudit.Image = ((System.Drawing.Image)(resources.GetObject("btnAudit.Image")));
            this.btnAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAudit.Name = "btnAudit";
            this.btnAudit.Size = new System.Drawing.Size(116, 22);
            this.btnAudit.Tag = "";
            this.btnAudit.Text = "提交审核信息(&A)";
            this.btnAudit.Visible = false;
            this.btnAudit.Click += new System.EventHandler(this.btnAudit_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnApprove
            // 
            this.btnApprove.Image = ((System.Drawing.Image)(resources.GetObject("btnApprove.Image")));
            this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(114, 22);
            this.btnApprove.Tag = "";
            this.btnApprove.Text = "提交批准信息(&F)";
            this.btnApprove.Visible = false;
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnReback
            // 
            this.btnReback.Image = ((System.Drawing.Image)(resources.GetObject("btnReback.Image")));
            this.btnReback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReback.Name = "btnReback";
            this.btnReback.Size = new System.Drawing.Size(92, 22);
            this.btnReback.Tag = "view";
            this.btnReback.Text = "回退单据(&R)";
            this.btnReback.Click += new System.EventHandler(this.btnReback_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(955, 61);
            this.panel2.TabIndex = 61;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(343, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "文件发布明细信息";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.txtReplaceFileNo);
            this.groupBox1.Controls.Add(this.txtRepalceVersion);
            this.groupBox1.Controls.Add(this.txtReplaceFileName);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtSortName);
            this.groupBox1.Controls.Add(this.txtDepartment);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbPropoerTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbPropoer);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lbSDBStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.llbProposerDownLoad);
            this.groupBox1.Controls.Add(this.llbProposerUpLoad);
            this.groupBox1.Controls.Add(this.txtFileNo);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtSDBNo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(955, 234);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "申请人填写区";
            // 
            // txtReplaceFileNo
            // 
            this.txtReplaceFileNo.DataResult = null;
            this.txtReplaceFileNo.DataTableResult = null;
            this.txtReplaceFileNo.EditingControlDataGridView = null;
            this.txtReplaceFileNo.EditingControlFormattedValue = "";
            this.txtReplaceFileNo.EditingControlRowIndex = 0;
            this.txtReplaceFileNo.EditingControlValueChanged = false;
            this.txtReplaceFileNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.体系文件;
            this.txtReplaceFileNo.IsMultiSelect = false;
            this.txtReplaceFileNo.Location = new System.Drawing.Point(110, 66);
            this.txtReplaceFileNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReplaceFileNo.Name = "txtReplaceFileNo";
            this.txtReplaceFileNo.ShowResultForm = true;
            this.txtReplaceFileNo.Size = new System.Drawing.Size(194, 21);
            this.txtReplaceFileNo.StrEndSql = null;
            this.txtReplaceFileNo.TabIndex = 188;
            this.txtReplaceFileNo.TabStop = false;
            this.txtReplaceFileNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtReplaceFileNo_OnCompleteSearch);
            this.txtReplaceFileNo.Enter += new System.EventHandler(this.txtReplaceFileNo_Enter);
            // 
            // txtRepalceVersion
            // 
            this.txtRepalceVersion.Enabled = false;
            this.txtRepalceVersion.Location = new System.Drawing.Point(855, 65);
            this.txtRepalceVersion.Name = "txtRepalceVersion";
            this.txtRepalceVersion.Size = new System.Drawing.Size(80, 21);
            this.txtRepalceVersion.TabIndex = 187;
            // 
            // txtReplaceFileName
            // 
            this.txtReplaceFileName.Enabled = false;
            this.txtReplaceFileName.Location = new System.Drawing.Point(411, 65);
            this.txtReplaceFileName.Name = "txtReplaceFileName";
            this.txtReplaceFileName.Size = new System.Drawing.Size(324, 21);
            this.txtReplaceFileName.TabIndex = 186;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(760, 69);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(89, 12);
            this.label18.TabIndex = 184;
            this.label18.Text = "替换文件版本号";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(319, 69);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(77, 12);
            this.label15.TabIndex = 183;
            this.label15.Text = "替换文件名称";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 182;
            this.label9.Text = "替换文件编号";
            // 
            // txtSortName
            // 
            this.txtSortName.DataResult = null;
            this.txtSortName.DataTableResult = null;
            this.txtSortName.EditingControlDataGridView = null;
            this.txtSortName.EditingControlFormattedValue = "";
            this.txtSortName.EditingControlRowIndex = 0;
            this.txtSortName.EditingControlValueChanged = false;
            this.txtSortName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.体系文件类别;
            this.txtSortName.IsMultiSelect = false;
            this.txtSortName.Location = new System.Drawing.Point(110, 139);
            this.txtSortName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSortName.Name = "txtSortName";
            this.txtSortName.ShowResultForm = true;
            this.txtSortName.Size = new System.Drawing.Size(194, 21);
            this.txtSortName.StrEndSql = null;
            this.txtSortName.TabIndex = 181;
            this.txtSortName.TabStop = false;
            this.txtSortName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtSortName_OnCompleteSearch);
            // 
            // txtDepartment
            // 
            this.txtDepartment.DataResult = null;
            this.txtDepartment.DataTableResult = null;
            this.txtDepartment.EditingControlDataGridView = null;
            this.txtDepartment.EditingControlFormattedValue = "";
            this.txtDepartment.EditingControlRowIndex = 0;
            this.txtDepartment.EditingControlValueChanged = false;
            this.txtDepartment.FindItem = UniversalControlLibrary.TextBoxShow.FindType.部门;
            this.txtDepartment.IsMultiSelect = false;
            this.txtDepartment.Location = new System.Drawing.Point(411, 139);
            this.txtDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDepartment.Name = "txtDepartment";
            this.txtDepartment.ShowResultForm = true;
            this.txtDepartment.Size = new System.Drawing.Size(165, 21);
            this.txtDepartment.StrEndSql = null;
            this.txtDepartment.TabIndex = 180;
            this.txtDepartment.TabStop = false;
            this.txtDepartment.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtDepartment_OnCompleteSearch);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(319, 142);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 179;
            this.label6.Text = "归口部门";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 142);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 178;
            this.label4.Text = "文件类别";
            // 
            // lbPropoerTime
            // 
            this.lbPropoerTime.AutoSize = true;
            this.lbPropoerTime.Location = new System.Drawing.Point(835, 204);
            this.lbPropoerTime.Name = "lbPropoerTime";
            this.lbPropoerTime.Size = new System.Drawing.Size(71, 12);
            this.lbPropoerTime.TabIndex = 176;
            this.lbPropoerTime.Text = "PropoerTime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(760, 204);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 175;
            this.label10.Text = "申请日期";
            // 
            // lbPropoer
            // 
            this.lbPropoer.AutoSize = true;
            this.lbPropoer.Location = new System.Drawing.Point(835, 175);
            this.lbPropoer.Name = "lbPropoer";
            this.lbPropoer.Size = new System.Drawing.Size(47, 12);
            this.lbPropoer.TabIndex = 174;
            this.lbPropoer.Text = "Propoer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(760, 175);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 173;
            this.label8.Text = "申 请 人";
            // 
            // lbSDBStatus
            // 
            this.lbSDBStatus.AutoSize = true;
            this.lbSDBStatus.ForeColor = System.Drawing.Color.Red;
            this.lbSDBStatus.Location = new System.Drawing.Point(451, 34);
            this.lbSDBStatus.Name = "lbSDBStatus";
            this.lbSDBStatus.Size = new System.Drawing.Size(59, 12);
            this.lbSDBStatus.TabIndex = 172;
            this.lbSDBStatus.Text = "SDBStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(319, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 171;
            this.label5.Text = "流程状态";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(411, 176);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(324, 41);
            this.txtRemark.TabIndex = 170;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(319, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 169;
            this.label3.Text = "备    注";
            // 
            // llbProposerDownLoad
            // 
            this.llbProposerDownLoad.AutoSize = true;
            this.llbProposerDownLoad.Location = new System.Drawing.Point(191, 190);
            this.llbProposerDownLoad.Name = "llbProposerDownLoad";
            this.llbProposerDownLoad.Size = new System.Drawing.Size(53, 12);
            this.llbProposerDownLoad.TabIndex = 168;
            this.llbProposerDownLoad.TabStop = true;
            this.llbProposerDownLoad.Text = "下载文件";
            this.llbProposerDownLoad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbProposerDownLoad_LinkClicked);
            // 
            // llbProposerUpLoad
            // 
            this.llbProposerUpLoad.AutoSize = true;
            this.llbProposerUpLoad.Location = new System.Drawing.Point(108, 190);
            this.llbProposerUpLoad.Name = "llbProposerUpLoad";
            this.llbProposerUpLoad.Size = new System.Drawing.Size(53, 12);
            this.llbProposerUpLoad.TabIndex = 167;
            this.llbProposerUpLoad.TabStop = true;
            this.llbProposerUpLoad.Text = "上传文件";
            this.llbProposerUpLoad.Visible = false;
            this.llbProposerUpLoad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbProposerUpLoad_LinkClicked);
            // 
            // txtFileNo
            // 
            this.txtFileNo.Location = new System.Drawing.Point(110, 102);
            this.txtFileNo.Name = "txtFileNo";
            this.txtFileNo.Size = new System.Drawing.Size(194, 21);
            this.txtFileNo.TabIndex = 0;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(411, 102);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(324, 21);
            this.txtFileName.TabIndex = 163;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(319, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 162;
            this.label1.Text = "文件名称";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(19, 105);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 161;
            this.label13.Text = "文件编号";
            // 
            // txtSDBNo
            // 
            this.txtSDBNo.BackColor = System.Drawing.Color.White;
            this.txtSDBNo.ForeColor = System.Drawing.Color.Red;
            this.txtSDBNo.Location = new System.Drawing.Point(110, 31);
            this.txtSDBNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSDBNo.Name = "txtSDBNo";
            this.txtSDBNo.ReadOnly = true;
            this.txtSDBNo.Size = new System.Drawing.Size(194, 21);
            this.txtSDBNo.TabIndex = 159;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 160;
            this.label2.Text = "流程编号";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtAuditorAdvise);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.lbAuditorTime);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.lbAuditor);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 320);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(955, 95);
            this.groupBox2.TabIndex = 63;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "审核人填写区";
            // 
            // txtAuditorAdvise
            // 
            this.txtAuditorAdvise.Location = new System.Drawing.Point(103, 22);
            this.txtAuditorAdvise.Multiline = true;
            this.txtAuditorAdvise.Name = "txtAuditorAdvise";
            this.txtAuditorAdvise.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAuditorAdvise.Size = new System.Drawing.Size(632, 59);
            this.txtAuditorAdvise.TabIndex = 186;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(19, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 185;
            this.label11.Text = "意    见";
            // 
            // lbAuditorTime
            // 
            this.lbAuditorTime.AutoSize = true;
            this.lbAuditorTime.Location = new System.Drawing.Point(835, 59);
            this.lbAuditorTime.Name = "lbAuditorTime";
            this.lbAuditorTime.Size = new System.Drawing.Size(71, 12);
            this.lbAuditorTime.TabIndex = 184;
            this.lbAuditorTime.Text = "AuditorTime";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(760, 59);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 183;
            this.label14.Text = "审核日期";
            // 
            // lbAuditor
            // 
            this.lbAuditor.AutoSize = true;
            this.lbAuditor.Location = new System.Drawing.Point(835, 30);
            this.lbAuditor.Name = "lbAuditor";
            this.lbAuditor.Size = new System.Drawing.Size(47, 12);
            this.lbAuditor.TabIndex = 182;
            this.lbAuditor.Text = "Auditor";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(760, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 181;
            this.label16.Text = "审 核 人";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtApproverAdvise);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.lbApproverTime);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.lbApprover);
            this.groupBox3.Controls.Add(this.label17);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox3.Location = new System.Drawing.Point(0, 415);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(955, 95);
            this.groupBox3.TabIndex = 64;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "批准人填写区";
            // 
            // txtApproverAdvise
            // 
            this.txtApproverAdvise.Location = new System.Drawing.Point(103, 20);
            this.txtApproverAdvise.Multiline = true;
            this.txtApproverAdvise.Name = "txtApproverAdvise";
            this.txtApproverAdvise.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtApproverAdvise.Size = new System.Drawing.Size(632, 59);
            this.txtApproverAdvise.TabIndex = 186;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 185;
            this.label7.Text = "意    见";
            // 
            // lbApproverTime
            // 
            this.lbApproverTime.AutoSize = true;
            this.lbApproverTime.Location = new System.Drawing.Point(835, 56);
            this.lbApproverTime.Name = "lbApproverTime";
            this.lbApproverTime.Size = new System.Drawing.Size(77, 12);
            this.lbApproverTime.TabIndex = 184;
            this.lbApproverTime.Text = "ApproverTime";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(760, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 183;
            this.label12.Text = "批准日期";
            // 
            // lbApprover
            // 
            this.lbApprover.AutoSize = true;
            this.lbApprover.Location = new System.Drawing.Point(835, 27);
            this.lbApprover.Name = "lbApprover";
            this.lbApprover.Size = new System.Drawing.Size(53, 12);
            this.lbApprover.TabIndex = 182;
            this.lbApprover.Text = "Approver";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(760, 27);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 181;
            this.label17.Text = "批 准 人";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(855, 102);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(80, 21);
            this.txtVersion.TabIndex = 190;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(760, 106);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(89, 12);
            this.label19.TabIndex = 189;
            this.label19.Text = "当前文件版本号";
            // 
            // 文件发布流程明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(955, 516);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "文件发布流程明细";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件发布流程明细";
            this.Load += new System.EventHandler(this.文件发布流程明细_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.文件发布流程明细_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnAudit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnApprove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnReback;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lbPropoerTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbPropoer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbSDBStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel llbProposerDownLoad;
        private System.Windows.Forms.LinkLabel llbProposerUpLoad;
        private System.Windows.Forms.TextBox txtFileNo;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSDBNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private TextBoxShow txtDepartment;
        private System.Windows.Forms.Label label6;
        private TextBoxShow txtSortName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAuditorAdvise;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbAuditorTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbAuditor;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtApproverAdvise;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbApproverTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbApprover;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtRepalceVersion;
        private System.Windows.Forms.TextBox txtReplaceFileName;
        private TextBoxShow txtReplaceFileNo;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label19;
    }
}