namespace Form_Quality_QC
{
    partial class 判定报告明细
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.关联业务 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.批次号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.备注 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.radioButton7 = new System.Windows.Forms.RadioButton();
            this.radioButton8 = new System.Windows.Forms.RadioButton();
            this.txtFinalJudgeExplain = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.customDataGridView2 = new UniversalControlLibrary.CustomDataGridView();
            this.判定项目 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.判定结果 = new UniversalControlLibrary.DataGridViewTextBoxButtonColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbIsRepeat = new System.Windows.Forms.CheckBox();
            this.txtJudgeReportNo = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chbIsFinalJudge = new System.Windows.Forms.CheckBox();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.txtJudgeExplain = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnBenchCreate = new System.Windows.Forms.Button();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.contextMenuStrip1.SuspendLayout();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView2)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(95, 48);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(94, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // customPanel1
            // 
            this.customPanel1.Controls.Add(this.customDataGridView1);
            this.customPanel1.Controls.Add(this.userControlDataLocalizer1);
            this.customPanel1.Controls.Add(this.groupBox2);
            this.customPanel1.Controls.Add(this.customDataGridView2);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(891, 590);
            this.customPanel1.TabIndex = 0;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.关联业务,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.批次号,
            this.供应商,
            this.数量,
            this.单位,
            this.备注,
            this.物品ID,
            this.单据号});
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 425);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(891, 165);
            this.customDataGridView1.TabIndex = 38;
            // 
            // 关联业务
            // 
            this.关联业务.DataPropertyName = "关联业务";
            this.关联业务.HeaderText = "关联业务";
            this.关联业务.Name = "关联业务";
            this.关联业务.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.图号型号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
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
            // 批次号
            // 
            this.批次号.DataPropertyName = "批次号";
            this.批次号.HeaderText = "批次号";
            this.批次号.Name = "批次号";
            this.批次号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.批次号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.供应商.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 数量
            // 
            this.数量.DataPropertyName = "数量";
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.数量.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 单位
            // 
            this.单位.DataPropertyName = "单位";
            this.单位.HeaderText = "单位";
            this.单位.Name = "单位";
            this.单位.ReadOnly = true;
            this.单位.Width = 40;
            // 
            // 备注
            // 
            this.备注.DataPropertyName = "备注";
            this.备注.HeaderText = "备注";
            this.备注.Name = "备注";
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.ReadOnly = true;
            this.单据号.Visible = false;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 393);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(891, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 37;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.radioButton5);
            this.groupBox2.Controls.Add(this.radioButton6);
            this.groupBox2.Controls.Add(this.radioButton7);
            this.groupBox2.Controls.Add(this.radioButton8);
            this.groupBox2.Controls.Add(this.txtFinalJudgeExplain);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 269);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(891, 124);
            this.groupBox2.TabIndex = 36;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "最终判定";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 352;
            this.label3.Text = "说    明";
            // 
            // radioButton5
            // 
            this.radioButton5.AutoSize = true;
            this.radioButton5.Location = new System.Drawing.Point(352, 31);
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.Size = new System.Drawing.Size(71, 16);
            this.radioButton5.TabIndex = 349;
            this.radioButton5.Text = "退    货";
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            this.radioButton6.AutoSize = true;
            this.radioButton6.Location = new System.Drawing.Point(263, 31);
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.Size = new System.Drawing.Size(71, 16);
            this.radioButton6.TabIndex = 348;
            this.radioButton6.Text = "挑选返工";
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // radioButton7
            // 
            this.radioButton7.AutoSize = true;
            this.radioButton7.Location = new System.Drawing.Point(177, 31);
            this.radioButton7.Name = "radioButton7";
            this.radioButton7.Size = new System.Drawing.Size(71, 16);
            this.radioButton7.TabIndex = 347;
            this.radioButton7.Text = "让步接收";
            this.radioButton7.UseVisualStyleBackColor = true;
            // 
            // radioButton8
            // 
            this.radioButton8.AutoSize = true;
            this.radioButton8.Location = new System.Drawing.Point(85, 31);
            this.radioButton8.Name = "radioButton8";
            this.radioButton8.Size = new System.Drawing.Size(71, 16);
            this.radioButton8.TabIndex = 346;
            this.radioButton8.Text = "同意使用";
            this.radioButton8.UseVisualStyleBackColor = true;
            // 
            // txtFinalJudgeExplain
            // 
            this.txtFinalJudgeExplain.BackColor = System.Drawing.Color.White;
            this.txtFinalJudgeExplain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtFinalJudgeExplain.ForeColor = System.Drawing.Color.Black;
            this.txtFinalJudgeExplain.Location = new System.Drawing.Point(85, 65);
            this.txtFinalJudgeExplain.Multiline = true;
            this.txtFinalJudgeExplain.Name = "txtFinalJudgeExplain";
            this.txtFinalJudgeExplain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtFinalJudgeExplain.Size = new System.Drawing.Size(521, 47);
            this.txtFinalJudgeExplain.TabIndex = 345;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 344;
            this.label1.Text = "判定结论";
            // 
            // customDataGridView2
            // 
            this.customDataGridView2.AllowUserToAddRows = false;
            this.customDataGridView2.AllowUserToDeleteRows = false;
            this.customDataGridView2.AllowUserToResizeRows = false;
            this.customDataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView2.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.判定项目,
            this.判定结果,
            this.dataGridViewTextBoxColumn1});
            this.customDataGridView2.ContextMenuStrip = this.contextMenuStrip1;
            this.customDataGridView2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customDataGridView2.Location = new System.Drawing.Point(0, 153);
            this.customDataGridView2.Name = "customDataGridView2";
            this.customDataGridView2.RowTemplate.Height = 23;
            this.customDataGridView2.Size = new System.Drawing.Size(891, 116);
            this.customDataGridView2.TabIndex = 35;
            // 
            // 判定项目
            // 
            this.判定项目.DataPropertyName = "判定项目";
            this.判定项目.HeaderText = "判定项目";
            this.判定项目.Name = "判定项目";
            this.判定项目.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.判定项目.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.判定项目.Width = 350;
            // 
            // 判定结果
            // 
            this.判定结果.DataPropertyName = "判定结果";
            this.判定结果.HeaderText = "判定结果";
            this.判定结果.Name = "判定结果";
            this.判定结果.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.判定结果.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.判定结果.Width = 400;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "单据号";
            this.dataGridViewTextBoxColumn1.HeaderText = "单据号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbIsRepeat);
            this.groupBox1.Controls.Add(this.txtJudgeReportNo);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.chbIsFinalJudge);
            this.groupBox1.Controls.Add(this.radioButton4);
            this.groupBox1.Controls.Add(this.radioButton3);
            this.groupBox1.Controls.Add(this.radioButton2);
            this.groupBox1.Controls.Add(this.radioButton1);
            this.groupBox1.Controls.Add(this.txtJudgeExplain);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnBenchCreate);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBillNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(891, 153);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "判定";
            // 
            // chbIsRepeat
            // 
            this.chbIsRepeat.AutoSize = true;
            this.chbIsRepeat.Location = new System.Drawing.Point(539, 25);
            this.chbIsRepeat.Name = "chbIsRepeat";
            this.chbIsRepeat.Size = new System.Drawing.Size(72, 16);
            this.chbIsRepeat.TabIndex = 354;
            this.chbIsRepeat.Text = "重复引用";
            this.chbIsRepeat.UseVisualStyleBackColor = true;
            // 
            // txtJudgeReportNo
            // 
            this.txtJudgeReportNo.BackColor = System.Drawing.Color.White;
            this.txtJudgeReportNo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJudgeReportNo.ForeColor = System.Drawing.Color.Black;
            this.txtJudgeReportNo.Location = new System.Drawing.Point(717, 23);
            this.txtJudgeReportNo.Name = "txtJudgeReportNo";
            this.txtJudgeReportNo.Size = new System.Drawing.Size(154, 21);
            this.txtJudgeReportNo.TabIndex = 353;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(628, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 352;
            this.label6.Text = "判定报告编号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 351;
            this.label2.Text = "说    明";
            // 
            // chbIsFinalJudge
            // 
            this.chbIsFinalJudge.AutoSize = true;
            this.chbIsFinalJudge.Location = new System.Drawing.Point(630, 109);
            this.chbIsFinalJudge.Name = "chbIsFinalJudge";
            this.chbIsFinalJudge.Size = new System.Drawing.Size(144, 16);
            this.chbIsFinalJudge.TabIndex = 350;
            this.chbIsFinalJudge.Text = "是否需要进行最终判定";
            this.chbIsFinalJudge.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            this.radioButton4.AutoSize = true;
            this.radioButton4.Location = new System.Drawing.Point(352, 64);
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.Size = new System.Drawing.Size(71, 16);
            this.radioButton4.TabIndex = 349;
            this.radioButton4.Text = "退    货";
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            this.radioButton3.AutoSize = true;
            this.radioButton3.Location = new System.Drawing.Point(263, 64);
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.Size = new System.Drawing.Size(71, 16);
            this.radioButton3.TabIndex = 348;
            this.radioButton3.Text = "挑选返工";
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.Location = new System.Drawing.Point(177, 64);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(71, 16);
            this.radioButton2.TabIndex = 347;
            this.radioButton2.Text = "让步接收";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(85, 64);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(71, 16);
            this.radioButton1.TabIndex = 346;
            this.radioButton1.Text = "同意使用";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // txtJudgeExplain
            // 
            this.txtJudgeExplain.BackColor = System.Drawing.Color.White;
            this.txtJudgeExplain.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtJudgeExplain.ForeColor = System.Drawing.Color.Black;
            this.txtJudgeExplain.Location = new System.Drawing.Point(85, 98);
            this.txtJudgeExplain.Multiline = true;
            this.txtJudgeExplain.Name = "txtJudgeExplain";
            this.txtJudgeExplain.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtJudgeExplain.Size = new System.Drawing.Size(521, 43);
            this.txtJudgeExplain.TabIndex = 345;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(13, 66);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 344;
            this.label13.Text = "判定结论";
            // 
            // btnBenchCreate
            // 
            this.btnBenchCreate.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnBenchCreate.Location = new System.Drawing.Point(454, 20);
            this.btnBenchCreate.Name = "btnBenchCreate";
            this.btnBenchCreate.Size = new System.Drawing.Size(69, 26);
            this.btnBenchCreate.TabIndex = 329;
            this.btnBenchCreate.Text = "引用";
            this.btnBenchCreate.UseVisualStyleBackColor = true;
            this.btnBenchCreate.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(350, 27);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 309;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(281, 27);
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
            this.txtBillNo.Location = new System.Drawing.Point(85, 23);
            this.txtBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.ReadOnly = true;
            this.txtBillNo.Size = new System.Drawing.Size(181, 21);
            this.txtBillNo.TabIndex = 306;
            this.txtBillNo.Text = "JRB201412000001";
            this.txtBillNo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(13, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 307;
            this.label4.Text = "业务编号";
            // 
            // 判定报告明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 590);
            this.Controls.Add(this.customPanel1);
            this.Name = "判定报告明细";
            this.Text = "判定报告明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.contextMenuStrip1.ResumeLayout(false);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.RadioButton radioButton7;
        private System.Windows.Forms.RadioButton radioButton8;
        private System.Windows.Forms.TextBox txtFinalJudgeExplain;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView2;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 判定项目;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 判定结果;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.TextBox txtJudgeExplain;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnBenchCreate;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chbIsFinalJudge;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn 关联业务;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 批次号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单位;
        private UniversalControlLibrary.DataGridViewTextBoxButtonColumn 备注;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtJudgeReportNo;
        private System.Windows.Forms.CheckBox chbIsRepeat;

    }
}