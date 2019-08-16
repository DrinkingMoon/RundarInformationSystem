using UniversalControlLibrary;
namespace Form_Quality_File
{
    partial class 文件销毁记录表
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbProposerTime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbProposer = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbDestroyTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbDestroyPersonnel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbApproverTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbApprover = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtDestroyWay = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.numCopies = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCoverFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileNo = new TextBoxShow();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnApprove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnDestroy = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopies)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 61);
            this.panel1.TabIndex = 61;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(408, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "文件销毁记录表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbProposerTime);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lbProposer);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.lbDestroyTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbDestroyPersonnel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lbApproverTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbApprover);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtDestroyWay);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.numCopies);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtCoverFile);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtFileNo);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1016, 145);
            this.groupBox1.TabIndex = 62;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息显示";
            // 
            // lbProposerTime
            // 
            this.lbProposerTime.AutoSize = true;
            this.lbProposerTime.Location = new System.Drawing.Point(913, 56);
            this.lbProposerTime.Name = "lbProposerTime";
            this.lbProposerTime.Size = new System.Drawing.Size(77, 12);
            this.lbProposerTime.TabIndex = 213;
            this.lbProposerTime.Text = "ProposerTime";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(844, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 212;
            this.label9.Text = "申请日期";
            // 
            // lbProposer
            // 
            this.lbProposer.AutoSize = true;
            this.lbProposer.Location = new System.Drawing.Point(733, 56);
            this.lbProposer.Name = "lbProposer";
            this.lbProposer.Size = new System.Drawing.Size(53, 12);
            this.lbProposer.TabIndex = 211;
            this.lbProposer.Text = "Proposer";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(657, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 210;
            this.label12.Text = "申 请 人";
            // 
            // lbDestroyTime
            // 
            this.lbDestroyTime.AutoSize = true;
            this.lbDestroyTime.Location = new System.Drawing.Point(913, 116);
            this.lbDestroyTime.Name = "lbDestroyTime";
            this.lbDestroyTime.Size = new System.Drawing.Size(71, 12);
            this.lbDestroyTime.TabIndex = 209;
            this.lbDestroyTime.Text = "DestroyTime";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(844, 116);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 208;
            this.label6.Text = "销毁日期";
            // 
            // lbDestroyPersonnel
            // 
            this.lbDestroyPersonnel.AutoSize = true;
            this.lbDestroyPersonnel.Location = new System.Drawing.Point(733, 116);
            this.lbDestroyPersonnel.Name = "lbDestroyPersonnel";
            this.lbDestroyPersonnel.Size = new System.Drawing.Size(101, 12);
            this.lbDestroyPersonnel.TabIndex = 207;
            this.lbDestroyPersonnel.Text = "DestroyPersonnel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(657, 116);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 206;
            this.label7.Text = "销 毁 人";
            // 
            // lbApproverTime
            // 
            this.lbApproverTime.AutoSize = true;
            this.lbApproverTime.Location = new System.Drawing.Point(913, 86);
            this.lbApproverTime.Name = "lbApproverTime";
            this.lbApproverTime.Size = new System.Drawing.Size(77, 12);
            this.lbApproverTime.TabIndex = 205;
            this.lbApproverTime.Text = "ApproverTime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(844, 86);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 204;
            this.label10.Text = "批准日期";
            // 
            // lbApprover
            // 
            this.lbApprover.AutoSize = true;
            this.lbApprover.Location = new System.Drawing.Point(733, 86);
            this.lbApprover.Name = "lbApprover";
            this.lbApprover.Size = new System.Drawing.Size(53, 12);
            this.lbApprover.TabIndex = 203;
            this.lbApprover.Text = "Approver";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(657, 86);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 202;
            this.label8.Text = "批 准 人";
            // 
            // txtDestroyWay
            // 
            this.txtDestroyWay.Location = new System.Drawing.Point(97, 85);
            this.txtDestroyWay.Multiline = true;
            this.txtDestroyWay.Name = "txtDestroyWay";
            this.txtDestroyWay.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDestroyWay.Size = new System.Drawing.Size(543, 46);
            this.txtDestroyWay.TabIndex = 197;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(33, 102);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 196;
            this.label5.Text = "销毁办法";
            // 
            // numCopies
            // 
            this.numCopies.Location = new System.Drawing.Point(384, 52);
            this.numCopies.Name = "numCopies";
            this.numCopies.Size = new System.Drawing.Size(120, 21);
            this.numCopies.TabIndex = 195;
            this.numCopies.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(320, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 194;
            this.label3.Text = "份    数";
            // 
            // txtCoverFile
            // 
            this.txtCoverFile.Location = new System.Drawing.Point(97, 52);
            this.txtCoverFile.Name = "txtCoverFile";
            this.txtCoverFile.Size = new System.Drawing.Size(204, 21);
            this.txtCoverFile.TabIndex = 193;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 192;
            this.label2.Text = "文件载体";
            // 
            // txtFileNo
            // 
            this.txtFileNo.FindItem = TextBoxShow.FindType.体系文件;
            this.txtFileNo.Location = new System.Drawing.Point(97, 20);
            this.txtFileNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileNo.Name = "txtFileNo";
            this.txtFileNo.Size = new System.Drawing.Size(204, 21);
            this.txtFileNo.TabIndex = 191;
            this.txtFileNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtFileNo_OnCompleteSearch);
            this.txtFileNo.Enter += new System.EventHandler(this.txtFileNo_Enter);
            // 
            // txtVersion
            // 
            this.txtVersion.Enabled = false;
            this.txtVersion.Location = new System.Drawing.Point(727, 20);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(95, 21);
            this.txtVersion.TabIndex = 190;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(657, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 189;
            this.label4.Text = "版 本 号";
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(384, 20);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(256, 21);
            this.txtFileName.TabIndex = 188;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(322, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 187;
            this.label1.Text = "文件名称";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(33, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 186;
            this.label13.Text = "文件编号";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 263);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 473);
            this.dataGridView1.TabIndex = 67;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparator2,
            this.btnUpdate,
            this.toolStripSeparatorAdd,
            this.btnDelete,
            this.toolStripSeparator3,
            this.btnApprove,
            this.toolStripSeparatorDelete,
            this.btnDestroy,
            this.toolStripSeparator5,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnExport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 45;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(67, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(67, 22);
            this.btnUpdate.Tag = "Delete";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
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
            this.btnDelete.Tag = "Delete";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnApprove
            // 
            this.btnApprove.Image = global::UniversalControlLibrary.Properties.Resources.match;
            this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(67, 22);
            this.btnApprove.Tag = "view";
            this.btnApprove.Text = "批准(&S)";
            this.btnApprove.Click += new System.EventHandler(this.btnApprove_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDestroy
            // 
            this.btnDestroy.Image = global::UniversalControlLibrary.Properties.Resources.ReadMe;
            this.btnDestroy.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDestroy.Name = "btnDestroy";
            this.btnDestroy.Size = new System.Drawing.Size(67, 22);
            this.btnDestroy.Tag = "view";
            this.btnDestroy.Text = "销毁(&W)";
            this.btnDestroy.Click += new System.EventHandler(this.btnDestroy_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 22);
            this.btnRefresh.Tag = "view";
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExport
            // 
            this.btnExport.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(97, 22);
            this.btnExport.Tag = "view";
            this.btnExport.Text = "导出EXCEL(&E)";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 231);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1016, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 65;
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
            // 文件销毁记录表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "文件销毁记录表";
            this.Text = "文件销毁记录表";
            this.Load += new System.EventHandler(this.文件销毁记录表_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCopies)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnApprove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnDestroy;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExport;
        private TextBoxShow txtFileNo;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtDestroyWay;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numCopies;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtCoverFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbProposerTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbProposer;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbDestroyTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbDestroyPersonnel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbApproverTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbApprover;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}