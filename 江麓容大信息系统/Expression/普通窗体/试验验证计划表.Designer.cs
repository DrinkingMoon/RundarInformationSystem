using UniversalControlLibrary;
namespace Expression
{
    partial class 试验验证计划表
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbBill_ID = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbAuditTime = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbAuditPersonnel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbPlanTime = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lbPlanProducer = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dtpTestTime = new System.Windows.Forms.DateTimePicker();
            this.label16 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtTestProgram = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTestCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTestObjective = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtSpecificStep = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.dtpFinishTime = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.nudStepNumber = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.btnAuditing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.txtPrinicipal = new TextBoxShow();
            this.txtTestPrincipal = new TextBoxShow();
            this.panel2.SuspendLayout();
            this.toolStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepNumber)).BeginInit();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.lbBill_ID);
            this.panel2.Controls.Add(this.label30);
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(860, 39);
            this.panel2.TabIndex = 69;
            // 
            // lbBill_ID
            // 
            this.lbBill_ID.AutoSize = true;
            this.lbBill_ID.ForeColor = System.Drawing.Color.Red;
            this.lbBill_ID.Location = new System.Drawing.Point(72, 13);
            this.lbBill_ID.Name = "lbBill_ID";
            this.lbBill_ID.Size = new System.Drawing.Size(47, 12);
            this.lbBill_ID.TabIndex = 22;
            this.lbBill_ID.Text = "Bill_ID";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(19, 13);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(47, 12);
            this.label30.TabIndex = 21;
            this.label30.Text = "单据号:";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(330, 6);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "试验验证计划表";
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.toolStripSeparator2,
            this.btnAuditing,
            this.toolStripSeparator1,
            this.btnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(860, 25);
            this.toolStrip.TabIndex = 68;
            this.toolStrip.TabStop = true;
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(67, 22);
            this.btnSave.Tag = "Process_1";
            this.btnSave.Text = "保存(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 22);
            this.btnClose.Tag = "Process_1";
            this.btnClose.Text = "退出(&Q)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbAuditTime);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lbAuditPersonnel);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lbPlanTime);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.lbPlanProducer);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.dtpTestTime);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.txtTestPrincipal);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtTestProgram);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtTestCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtTestObjective);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 64);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(860, 200);
            this.groupBox1.TabIndex = 70;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "试验计划";
            // 
            // lbAuditTime
            // 
            this.lbAuditTime.AutoSize = true;
            this.lbAuditTime.Location = new System.Drawing.Point(713, 177);
            this.lbAuditTime.Name = "lbAuditTime";
            this.lbAuditTime.Size = new System.Drawing.Size(59, 12);
            this.lbAuditTime.TabIndex = 73;
            this.lbAuditTime.Text = "AuditTime";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(660, 177);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(35, 12);
            this.label7.TabIndex = 72;
            this.label7.Text = "时间:";
            // 
            // lbAuditPersonnel
            // 
            this.lbAuditPersonnel.AutoSize = true;
            this.lbAuditPersonnel.Location = new System.Drawing.Point(535, 177);
            this.lbAuditPersonnel.Name = "lbAuditPersonnel";
            this.lbAuditPersonnel.Size = new System.Drawing.Size(89, 12);
            this.lbAuditPersonnel.TabIndex = 71;
            this.lbAuditPersonnel.Text = "AuditPersonnel";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(464, 177);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 70;
            this.label9.Text = "审核人:";
            // 
            // lbPlanTime
            // 
            this.lbPlanTime.AutoSize = true;
            this.lbPlanTime.Location = new System.Drawing.Point(321, 177);
            this.lbPlanTime.Name = "lbPlanTime";
            this.lbPlanTime.Size = new System.Drawing.Size(53, 12);
            this.lbPlanTime.TabIndex = 69;
            this.lbPlanTime.Text = "PlanTime";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(268, 177);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 12);
            this.label6.TabIndex = 68;
            this.label6.Text = "时间:";
            // 
            // lbPlanProducer
            // 
            this.lbPlanProducer.AutoSize = true;
            this.lbPlanProducer.Location = new System.Drawing.Point(169, 177);
            this.lbPlanProducer.Name = "lbPlanProducer";
            this.lbPlanProducer.Size = new System.Drawing.Size(77, 12);
            this.lbPlanProducer.TabIndex = 67;
            this.lbPlanProducer.Text = "PlanProducer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(72, 177);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 12);
            this.label4.TabIndex = 66;
            this.label4.Text = "计划作成人:";
            // 
            // dtpTestTime
            // 
            this.dtpTestTime.Location = new System.Drawing.Point(688, 19);
            this.dtpTestTime.Name = "dtpTestTime";
            this.dtpTestTime.Size = new System.Drawing.Size(160, 21);
            this.dtpTestTime.TabIndex = 65;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(631, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 64;
            this.label16.Text = "试验时间";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(439, 23);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(65, 12);
            this.label14.TabIndex = 62;
            this.label14.Text = "试验负责人";
            // 
            // txtTestProgram
            // 
            this.txtTestProgram.Location = new System.Drawing.Point(74, 109);
            this.txtTestProgram.Multiline = true;
            this.txtTestProgram.Name = "txtTestProgram";
            this.txtTestProgram.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTestProgram.Size = new System.Drawing.Size(774, 55);
            this.txtTestProgram.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "试验方案";
            // 
            // txtTestCode
            // 
            this.txtTestCode.Location = new System.Drawing.Point(74, 19);
            this.txtTestCode.Name = "txtTestCode";
            this.txtTestCode.Size = new System.Drawing.Size(359, 21);
            this.txtTestCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 23);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "试验名称";
            // 
            // txtTestObjective
            // 
            this.txtTestObjective.Location = new System.Drawing.Point(74, 59);
            this.txtTestObjective.Multiline = true;
            this.txtTestObjective.Name = "txtTestObjective";
            this.txtTestObjective.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTestObjective.Size = new System.Drawing.Size(774, 36);
            this.txtTestObjective.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 71);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "试验目的";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Controls.Add(this.panel1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 264);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(860, 358);
            this.groupBox2.TabIndex = 71;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "试验过程";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 116);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(854, 239);
            this.dataGridView1.TabIndex = 51;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnUpdate);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.txtSpecificStep);
            this.panel1.Controls.Add(this.label11);
            this.panel1.Controls.Add(this.dtpFinishTime);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtPrinicipal);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.nudStepNumber);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(854, 99);
            this.panel1.TabIndex = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(770, 13);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 74;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(672, 13);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(75, 23);
            this.btnUpdate.TabIndex = 73;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(574, 12);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 72;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtSpecificStep
            // 
            this.txtSpecificStep.Location = new System.Drawing.Point(71, 48);
            this.txtSpecificStep.Multiline = true;
            this.txtSpecificStep.Name = "txtSpecificStep";
            this.txtSpecificStep.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtSpecificStep.Size = new System.Drawing.Size(774, 36);
            this.txtSpecificStep.TabIndex = 71;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 60);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 70;
            this.label11.Text = "具体步骤";
            // 
            // dtpFinishTime
            // 
            this.dtpFinishTime.Location = new System.Drawing.Point(387, 13);
            this.dtpFinishTime.Name = "dtpFinishTime";
            this.dtpFinishTime.Size = new System.Drawing.Size(160, 21);
            this.dtpFinishTime.TabIndex = 69;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(328, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 68;
            this.label8.Text = "完成时间";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(154, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(41, 12);
            this.label10.TabIndex = 66;
            this.label10.Text = "责任人";
            // 
            // nudStepNumber
            // 
            this.nudStepNumber.Location = new System.Drawing.Point(71, 13);
            this.nudStepNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudStepNumber.Name = "nudStepNumber";
            this.nudStepNumber.Size = new System.Drawing.Size(60, 21);
            this.nudStepNumber.TabIndex = 4;
            this.nudStepNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.nudStepNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "试验步骤";
            // 
            // btnAuditing
            // 
            this.btnAuditing.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAuditing.Image = global::UniversalControlLibrary.Properties.Resources.审核6;
            this.btnAuditing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAuditing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAuditing.Name = "btnAuditing";
            this.btnAuditing.Size = new System.Drawing.Size(67, 22);
            this.btnAuditing.Tag = "Process_1";
            this.btnAuditing.Text = "审核(&S)";
            this.btnAuditing.Visible = false;
            this.btnAuditing.Click += new System.EventHandler(this.btnAuditing_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Visible = false;
            // 
            // txtPrinicipal
            // 
            this.txtPrinicipal.FindItem = TextBoxShow.FindType.人员;
            this.txtPrinicipal.Location = new System.Drawing.Point(201, 13);
            this.txtPrinicipal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPrinicipal.Name = "txtPrinicipal";
            this.txtPrinicipal.Size = new System.Drawing.Size(113, 21);
            this.txtPrinicipal.TabIndex = 67;
            // 
            // txtTestPrincipal
            // 
            this.txtTestPrincipal.FindItem = TextBoxShow.FindType.人员;
            this.txtTestPrincipal.Location = new System.Drawing.Point(508, 19);
            this.txtTestPrincipal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTestPrincipal.Name = "txtTestPrincipal";
            this.txtTestPrincipal.Size = new System.Drawing.Size(113, 21);
            this.txtTestPrincipal.TabIndex = 63;
            // 
            // 试验验证计划表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 622);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip);
            this.Name = "试验验证计划表";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "试验验证计划表";
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStepNumber)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtTestProgram;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTestCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTestObjective;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpTestTime;
        private System.Windows.Forms.Label label16;
        private TextBoxShow txtTestPrincipal;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbAuditTime;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbAuditPersonnel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbPlanTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbPlanProducer;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtSpecificStep;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpFinishTime;
        private System.Windows.Forms.Label label8;
        private TextBoxShow txtPrinicipal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown nudStepNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lbBill_ID;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnAuditing;
    }
}