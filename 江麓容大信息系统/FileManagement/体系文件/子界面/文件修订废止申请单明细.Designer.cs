using UniversalControlLibrary;
namespace Form_Quality_File
{
    partial class 文件修订废止申请单明细
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtApproverAdvise = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.lbApproverTime = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.lbApprover = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtAuditorAdvise = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.lbAuditorTime = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.lbAuditor = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFileNo = new TextBoxShow();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lbPropoerTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbPropoer = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProposeContent = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
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
            this.toolStrip1.Size = new System.Drawing.Size(957, 25);
            this.toolStrip1.TabIndex = 49;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.提交;
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
            this.btnAudit.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAudit.Name = "btnAudit";
            this.btnAudit.Size = new System.Drawing.Size(115, 22);
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
            this.btnApprove.Image = global::UniversalControlLibrary.Properties.Resources.审核6;
            this.btnApprove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApprove.Name = "btnApprove";
            this.btnApprove.Size = new System.Drawing.Size(115, 22);
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
            this.btnReback.Image = global::UniversalControlLibrary.Properties.Resources.回退;
            this.btnReback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReback.Name = "btnReback";
            this.btnReback.Size = new System.Drawing.Size(91, 22);
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
            this.panel2.Size = new System.Drawing.Size(957, 61);
            this.panel2.TabIndex = 62;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(311, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(309, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "文件修订废止申请单明细";
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
            this.groupBox3.Location = new System.Drawing.Point(0, 355);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(957, 95);
            this.groupBox3.TabIndex = 67;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "批准人填写区";
            // 
            // txtApproverAdvise
            // 
            this.txtApproverAdvise.Location = new System.Drawing.Point(110, 20);
            this.txtApproverAdvise.Multiline = true;
            this.txtApproverAdvise.Name = "txtApproverAdvise";
            this.txtApproverAdvise.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtApproverAdvise.Size = new System.Drawing.Size(625, 59);
            this.txtApproverAdvise.TabIndex = 186;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 44);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 185;
            this.label7.Text = "批准意见";
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
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtAuditorAdvise);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.lbAuditorTime);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.lbAuditor);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 260);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(957, 95);
            this.groupBox2.TabIndex = 66;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "审核人填写区";
            // 
            // txtAuditorAdvise
            // 
            this.txtAuditorAdvise.Location = new System.Drawing.Point(110, 22);
            this.txtAuditorAdvise.Multiline = true;
            this.txtAuditorAdvise.Name = "txtAuditorAdvise";
            this.txtAuditorAdvise.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAuditorAdvise.Size = new System.Drawing.Size(625, 59);
            this.txtAuditorAdvise.TabIndex = 186;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(21, 47);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 185;
            this.label11.Text = "审核意见";
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
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFileNo);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.lbPropoerTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbPropoer);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtProposeContent);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(957, 174);
            this.groupBox1.TabIndex = 65;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "申请人填写区";
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ForeColor = System.Drawing.Color.MediumBlue;
            this.radioButton2.Location = new System.Drawing.Point(835, 32);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(47, 16);
            this.radioButton2.TabIndex = 182;
            this.radioButton2.Text = "废止";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Checked = true;
            this.radioButton1.ForeColor = System.Drawing.Color.MediumBlue;
            this.radioButton1.Location = new System.Drawing.Point(762, 32);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(47, 16);
            this.radioButton1.TabIndex = 181;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "修订";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.MediumBlue;
            this.label6.Location = new System.Drawing.Point(682, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 180;
            this.label6.Text = "操作模式";
            // 
            // txtFileNo
            // 
            this.txtFileNo.FindItem = TextBoxShow.FindType.体系文件;
            this.txtFileNo.Location = new System.Drawing.Point(110, 64);
            this.txtFileNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileNo.Name = "txtFileNo";
            this.txtFileNo.Size = new System.Drawing.Size(194, 21);
            this.txtFileNo.TabIndex = 179;
            this.txtFileNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtFileNo_OnCompleteSearch);
            this.txtFileNo.Enter += new System.EventHandler(this.txtFileNo_Enter);
            // 
            // txtVersion
            // 
            this.txtVersion.Enabled = false;
            this.txtVersion.Location = new System.Drawing.Point(837, 64);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(108, 21);
            this.txtVersion.TabIndex = 178;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(760, 68);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 177;
            this.label4.Text = "版 本 号";
            // 
            // lbPropoerTime
            // 
            this.lbPropoerTime.AutoSize = true;
            this.lbPropoerTime.Location = new System.Drawing.Point(835, 147);
            this.lbPropoerTime.Name = "lbPropoerTime";
            this.lbPropoerTime.Size = new System.Drawing.Size(71, 12);
            this.lbPropoerTime.TabIndex = 176;
            this.lbPropoerTime.Text = "PropoerTime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(760, 147);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 175;
            this.label10.Text = "申请日期";
            // 
            // lbPropoer
            // 
            this.lbPropoer.AutoSize = true;
            this.lbPropoer.Location = new System.Drawing.Point(835, 118);
            this.lbPropoer.Name = "lbPropoer";
            this.lbPropoer.Size = new System.Drawing.Size(47, 12);
            this.lbPropoer.TabIndex = 174;
            this.lbPropoer.Text = "Propoer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(760, 118);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 173;
            this.label8.Text = "申 请 人";
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(451, 34);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 172;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(319, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 171;
            this.label5.Text = "单据状态";
            // 
            // txtProposeContent
            // 
            this.txtProposeContent.Location = new System.Drawing.Point(110, 98);
            this.txtProposeContent.Multiline = true;
            this.txtProposeContent.Name = "txtProposeContent";
            this.txtProposeContent.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProposeContent.Size = new System.Drawing.Size(625, 63);
            this.txtProposeContent.TabIndex = 170;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(21, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 46);
            this.label3.TabIndex = 169;
            this.label3.Text = "申请理由及建议修改内容";
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(411, 64);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(324, 21);
            this.txtFileName.TabIndex = 163;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(319, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 162;
            this.label1.Text = "文件名称";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(21, 68);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 161;
            this.label13.Text = "文件编号";
            // 
            // txtBillNo
            // 
            this.txtBillNo.BackColor = System.Drawing.Color.White;
            this.txtBillNo.ForeColor = System.Drawing.Color.Red;
            this.txtBillNo.Location = new System.Drawing.Point(110, 30);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(194, 21);
            this.txtBillNo.TabIndex = 159;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 160;
            this.label2.Text = "单 据 号";
            // 
            // 文件修订废止申请单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(957, 455);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Name = "文件修订废止申请单明细";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "文件修订废止申请单明细";
            this.Load += new System.EventHandler(this.文件修订废止申请单明细_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.文件修订废止申请单明细_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtApproverAdvise;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbApproverTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbApprover;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtAuditorAdvise;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lbAuditorTime;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbAuditor;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.GroupBox groupBox1;
        private TextBoxShow txtFileNo;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbPropoerTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbPropoer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProposeContent;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.Label label6;
    }
}