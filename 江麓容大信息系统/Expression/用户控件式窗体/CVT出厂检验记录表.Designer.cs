using UniversalControlLibrary;
namespace Expression
{
    partial class CVT出厂检验记录表
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CVT出厂检验记录表));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnBatch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnOnly = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnJudge = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnFind = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorFind = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOutExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCVTFinishSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbFinalNo = new System.Windows.Forms.RadioButton();
            this.rbFinalYes = new System.Windows.Forms.RadioButton();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbTestItemName = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.lbDJZT = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtDisqualificationCase = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtAssociatedBillNo = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbTechnicalRequirementsName = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtProductType = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtBill_ID = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.rbNo = new System.Windows.Forms.RadioButton();
            this.rbYes = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.toolStrip1.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnBatch,
            this.toolStripSeparatorAdd,
            this.btnOnly,
            this.toolStripSeparator1,
            this.btnJudge,
            this.toolStripSeparatorDelete,
            this.btnFind,
            this.toolStripSeparatorFind,
            this.btnRefresh,
            this.toolStripSeparator3,
            this.btnOutExcel,
            this.toolStripSeparator2,
            this.btnCVTFinishSelect,
            this.toolStripSeparator4});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(984, 25);
            this.toolStrip1.TabIndex = 45;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnBatch
            // 
            this.btnBatch.Image = ((System.Drawing.Image)(resources.GetObject("btnBatch.Image")));
            this.btnBatch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnBatch.Name = "btnBatch";
            this.btnBatch.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnBatch.Size = new System.Drawing.Size(144, 22);
            this.btnBatch.Tag = "Add";
            this.btnBatch.Text = "批量提交合格信息(&M)";
            this.btnBatch.Click += new System.EventHandler(this.btnBatch_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOnly
            // 
            this.btnOnly.Image = ((System.Drawing.Image)(resources.GetObject("btnOnly.Image")));
            this.btnOnly.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOnly.Name = "btnOnly";
            this.btnOnly.Size = new System.Drawing.Size(94, 22);
            this.btnOnly.Tag = "delete";
            this.btnOnly.Text = "单个提交(&O)";
            this.btnOnly.Click += new System.EventHandler(this.btnOnly_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnJudge
            // 
            this.btnJudge.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnJudge.Image = ((System.Drawing.Image)(resources.GetObject("btnJudge.Image")));
            this.btnJudge.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnJudge.Name = "btnJudge";
            this.btnJudge.Size = new System.Drawing.Size(91, 22);
            this.btnJudge.Tag = "Auditing";
            this.btnJudge.Text = "最终判定(&F)";
            this.btnJudge.Click += new System.EventHandler(this.btnJudge_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnFind
            // 
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(68, 22);
            this.btnFind.Tag = "view";
            this.btnFind.Text = "查看(&C)";
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // toolStripSeparatorFind
            // 
            this.toolStripSeparatorFind.Name = "toolStripSeparatorFind";
            this.toolStripSeparatorFind.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 22);
            this.btnRefresh.Tag = "View";
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOutExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnOutExcel.Image")));
            this.btnOutExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(97, 22);
            this.btnOutExcel.Tag = "view";
            this.btnOutExcel.Text = "导出EXCEL(&E)";
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCVTFinishSelect
            // 
            this.btnCVTFinishSelect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCVTFinishSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnCVTFinishSelect.Image")));
            this.btnCVTFinishSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCVTFinishSelect.Name = "btnCVTFinishSelect";
            this.btnCVTFinishSelect.Size = new System.Drawing.Size(133, 22);
            this.btnCVTFinishSelect.Tag = "view";
            this.btnCVTFinishSelect.Text = "CVT终检信息查询(&V)";
            this.btnCVTFinishSelect.Click += new System.EventHandler(this.btnCVTFinishSelect_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // panelTitle
            // 
            this.panelTitle.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelTitle.Controls.Add(this.labelTitle);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(0, 25);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(984, 53);
            this.panelTitle.TabIndex = 46;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(371, 13);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(243, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "CVT出厂检验记录表";
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox2);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 128);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(984, 288);
            this.panelPara.TabIndex = 49;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbFinalNo);
            this.groupBox2.Controls.Add(this.rbFinalYes);
            this.groupBox2.Controls.Add(this.txtRemark);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.cmbTestItemName);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.lbDJZT);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.txtDisqualificationCase);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txtAssociatedBillNo);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.cmbTechnicalRequirementsName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtProductType);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.txtProductCode);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.txtBill_ID);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.rbNo);
            this.groupBox2.Controls.Add(this.rbYes);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(984, 240);
            this.groupBox2.TabIndex = 49;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "检验信息";
            // 
            // rbFinalNo
            // 
            this.rbFinalNo.AutoSize = true;
            this.rbFinalNo.Location = new System.Drawing.Point(869, 196);
            this.rbFinalNo.Name = "rbFinalNo";
            this.rbFinalNo.Size = new System.Drawing.Size(67, 18);
            this.rbFinalNo.TabIndex = 250;
            this.rbFinalNo.TabStop = true;
            this.rbFinalNo.Text = "不合格";
            this.rbFinalNo.UseVisualStyleBackColor = true;
            // 
            // rbFinalYes
            // 
            this.rbFinalYes.AutoSize = true;
            this.rbFinalYes.Location = new System.Drawing.Point(790, 196);
            this.rbFinalYes.Name = "rbFinalYes";
            this.rbFinalYes.Size = new System.Drawing.Size(53, 18);
            this.rbFinalYes.TabIndex = 249;
            this.rbFinalYes.TabStop = true;
            this.rbFinalYes.Text = "合格";
            this.rbFinalYes.UseVisualStyleBackColor = true;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(120, 183);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(496, 44);
            this.txtRemark.TabIndex = 244;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(653, 198);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(105, 14);
            this.label11.TabIndex = 248;
            this.label11.Text = "最终判定结论：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F);
            this.label1.Location = new System.Drawing.Point(33, 198);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 243;
            this.label1.Text = "备    注：";
            // 
            // cmbTestItemName
            // 
            this.cmbTestItemName.FormattingEnabled = true;
            this.cmbTestItemName.Location = new System.Drawing.Point(736, 58);
            this.cmbTestItemName.Name = "cmbTestItemName";
            this.cmbTestItemName.Size = new System.Drawing.Size(228, 21);
            this.cmbTestItemName.TabIndex = 242;
            this.cmbTestItemName.SelectedIndexChanged += new System.EventHandler(this.cmbTestItemName_SelectedIndexChanged);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10F);
            this.label12.Location = new System.Drawing.Point(653, 61);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(77, 14);
            this.label12.TabIndex = 241;
            this.label12.Text = "检测项目：";
            // 
            // lbDJZT
            // 
            this.lbDJZT.AutoSize = true;
            this.lbDJZT.ForeColor = System.Drawing.Color.Red;
            this.lbDJZT.Location = new System.Drawing.Point(775, 27);
            this.lbDJZT.Name = "lbDJZT";
            this.lbDJZT.Size = new System.Drawing.Size(63, 14);
            this.lbDJZT.TabIndex = 237;
            this.lbDJZT.Text = "单据状态";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(653, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 14);
            this.label10.TabIndex = 236;
            this.label10.Text = "单据状态：";
            // 
            // txtDisqualificationCase
            // 
            this.txtDisqualificationCase.Location = new System.Drawing.Point(436, 123);
            this.txtDisqualificationCase.Multiline = true;
            this.txtDisqualificationCase.Name = "txtDisqualificationCase";
            this.txtDisqualificationCase.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDisqualificationCase.Size = new System.Drawing.Size(528, 44);
            this.txtDisqualificationCase.TabIndex = 233;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10F);
            this.label8.Location = new System.Drawing.Point(329, 138);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 232;
            this.label8.Text = "不合格情况：";
            // 
            // txtAssociatedBillNo
            // 
            this.txtAssociatedBillNo.Enabled = false;
            this.txtAssociatedBillNo.Location = new System.Drawing.Point(436, 22);
            this.txtAssociatedBillNo.Name = "txtAssociatedBillNo";
            this.txtAssociatedBillNo.Size = new System.Drawing.Size(180, 23);
            this.txtAssociatedBillNo.TabIndex = 231;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(343, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 230;
            this.label5.Text = "关联单号：";
            // 
            // cmbTechnicalRequirementsName
            // 
            this.cmbTechnicalRequirementsName.FormattingEnabled = true;
            this.cmbTechnicalRequirementsName.Location = new System.Drawing.Point(437, 90);
            this.cmbTechnicalRequirementsName.Name = "cmbTechnicalRequirementsName";
            this.cmbTechnicalRequirementsName.Size = new System.Drawing.Size(527, 21);
            this.cmbTechnicalRequirementsName.TabIndex = 228;
            this.cmbTechnicalRequirementsName.SelectedIndexChanged += new System.EventHandler(this.cmbTechnicalRequirementsName_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F);
            this.label4.Location = new System.Drawing.Point(329, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 227;
            this.label4.Text = "不合格项目：";
            // 
            // txtProductType
            // 
            this.txtProductType.Enabled = false;
            this.txtProductType.Location = new System.Drawing.Point(121, 72);
            this.txtProductType.Name = "txtProductType";
            this.txtProductType.Size = new System.Drawing.Size(182, 23);
            this.txtProductType.TabIndex = 226;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 135);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 14);
            this.label7.TabIndex = 225;
            this.label7.Text = "产品编码：";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Enabled = false;
            this.txtProductCode.Location = new System.Drawing.Point(120, 130);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(184, 23);
            this.txtProductCode.TabIndex = 224;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10F);
            this.label13.Location = new System.Drawing.Point(33, 77);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 14);
            this.label13.TabIndex = 222;
            this.label13.Text = "产品型号：";
            // 
            // txtBill_ID
            // 
            this.txtBill_ID.BackColor = System.Drawing.Color.White;
            this.txtBill_ID.ForeColor = System.Drawing.Color.Red;
            this.txtBill_ID.Location = new System.Drawing.Point(122, 22);
            this.txtBill_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBill_ID.Name = "txtBill_ID";
            this.txtBill_ID.ReadOnly = true;
            this.txtBill_ID.Size = new System.Drawing.Size(181, 23);
            this.txtBill_ID.TabIndex = 220;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(47, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 221;
            this.label6.Text = "单据号：";
            // 
            // rbNo
            // 
            this.rbNo.AutoSize = true;
            this.rbNo.Location = new System.Drawing.Point(541, 59);
            this.rbNo.Name = "rbNo";
            this.rbNo.Size = new System.Drawing.Size(67, 18);
            this.rbNo.TabIndex = 5;
            this.rbNo.TabStop = true;
            this.rbNo.Text = "不合格";
            this.rbNo.UseVisualStyleBackColor = true;
            this.rbNo.CheckedChanged += new System.EventHandler(this.rbNo_CheckedChanged);
            // 
            // rbYes
            // 
            this.rbYes.AutoSize = true;
            this.rbYes.Location = new System.Drawing.Point(437, 59);
            this.rbYes.Name = "rbYes";
            this.rbYes.Size = new System.Drawing.Size(53, 18);
            this.rbYes.TabIndex = 4;
            this.rbYes.TabStop = true;
            this.rbYes.Text = "合格";
            this.rbYes.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(357, 61);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 3;
            this.label3.Text = "结  论：";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 416);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(984, 330);
            this.dataGridView1.TabIndex = 52;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick_1);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.ListBillStatus = ((System.Collections.Generic.List<string>)(resources.GetObject("checkBillDateAndStatus1.ListBillStatus")));
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 78);
            this.checkBillDateAndStatus1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.checkBillDateAndStatus1.MultiVisible = true;
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(984, 50);
            this.checkBillDateAndStatus1.StatusVisible = true;
            this.checkBillDateAndStatus1.TabIndex = 51;
            // 
            // CVT出厂检验记录表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(984, 746);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.panelTitle);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "CVT出厂检验记录表";
            this.Load += new System.EventHandler(this.CVT出厂检验记录表_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnBatch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnOnly;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnFind;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorFind;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnOutExcel;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbNo;
        private System.Windows.Forms.RadioButton rbYes;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtProductType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtBill_ID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbTechnicalRequirementsName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtAssociatedBillNo;
        private System.Windows.Forms.TextBox txtDisqualificationCase;
        private System.Windows.Forms.Label lbDJZT;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnJudge;
        private System.Windows.Forms.ComboBox cmbTestItemName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.RadioButton rbFinalNo;
        private System.Windows.Forms.RadioButton rbFinalYes;
        private System.Windows.Forms.Label label11;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton btnCVTFinishSelect;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label1;

    }
}
