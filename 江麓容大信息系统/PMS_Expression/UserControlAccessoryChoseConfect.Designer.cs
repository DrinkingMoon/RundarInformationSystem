namespace Expression
{
    partial class UserControlAccessoryChoseConfect
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlAccessoryChoseConfect));
            this.panelPara = new System.Windows.Forms.Panel();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFindCode = new System.Windows.Forms.Button();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtChoseConfect = new System.Windows.Forms.TextBox();
            this.numMax = new System.Windows.Forms.NumericUpDown();
            this.numMin = new System.Windows.Forms.NumericUpDown();
            this.lbChoseData = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lbRangeData = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.toolStripSeparatorUpdate = new System.Windows.Forms.ToolStripSeparator();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.btnChoseControlManage = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelPara.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).BeginInit();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelCenter.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelPara
            // 
            this.panelPara.BackColor = System.Drawing.SystemColors.Control;
            this.panelPara.Controls.Add(this.cmbProductType);
            this.panelPara.Controls.Add(this.label1);
            this.panelPara.Controls.Add(this.btnFindCode);
            this.panelPara.Controls.Add(this.txtCode);
            this.panelPara.Controls.Add(this.txtChoseConfect);
            this.panelPara.Controls.Add(this.numMax);
            this.panelPara.Controls.Add(this.numMin);
            this.panelPara.Controls.Add(this.lbChoseData);
            this.panelPara.Controls.Add(this.label4);
            this.panelPara.Controls.Add(this.lbRangeData);
            this.panelPara.Controls.Add(this.label12);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 0);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1016, 99);
            this.panelPara.TabIndex = 16;
            // 
            // cmbProductType
            // 
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Location = new System.Drawing.Point(691, 15);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(151, 21);
            this.cmbProductType.TabIndex = 172;
            this.cmbProductType.SelectedIndexChanged += new System.EventHandler(this.cmbProductType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(591, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 171;
            this.label1.Text = "产品类型";
            // 
            // btnFindCode
            // 
            this.btnFindCode.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnFindCode.BackgroundImage")));
            this.btnFindCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode.Location = new System.Drawing.Point(457, 16);
            this.btnFindCode.Name = "btnFindCode";
            this.btnFindCode.Size = new System.Drawing.Size(21, 19);
            this.btnFindCode.TabIndex = 170;
            this.btnFindCode.UseVisualStyleBackColor = false;
            this.btnFindCode.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(248, 14);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(203, 23);
            this.txtCode.TabIndex = 169;
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            // 
            // txtChoseConfect
            // 
            this.txtChoseConfect.Location = new System.Drawing.Point(691, 56);
            this.txtChoseConfect.Name = "txtChoseConfect";
            this.txtChoseConfect.Size = new System.Drawing.Size(151, 23);
            this.txtChoseConfect.TabIndex = 168;
            // 
            // numMax
            // 
            this.numMax.DecimalPlaces = 7;
            this.numMax.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numMax.Location = new System.Drawing.Point(381, 56);
            this.numMax.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numMax.Name = "numMax";
            this.numMax.Size = new System.Drawing.Size(97, 23);
            this.numMax.TabIndex = 167;
            // 
            // numMin
            // 
            this.numMin.DecimalPlaces = 7;
            this.numMin.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numMin.Location = new System.Drawing.Point(248, 56);
            this.numMin.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numMin.Name = "numMin";
            this.numMin.Size = new System.Drawing.Size(97, 23);
            this.numMin.TabIndex = 166;
            // 
            // lbChoseData
            // 
            this.lbChoseData.AutoSize = true;
            this.lbChoseData.Location = new System.Drawing.Point(591, 60);
            this.lbChoseData.Name = "lbChoseData";
            this.lbChoseData.Size = new System.Drawing.Size(35, 14);
            this.lbChoseData.TabIndex = 165;
            this.lbChoseData.Text = "选配";
            this.lbChoseData.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(354, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 164;
            this.label4.Text = "至";
            // 
            // lbRangeData
            // 
            this.lbRangeData.AutoSize = true;
            this.lbRangeData.Location = new System.Drawing.Point(151, 60);
            this.lbRangeData.Name = "lbRangeData";
            this.lbRangeData.Size = new System.Drawing.Size(35, 14);
            this.lbRangeData.TabIndex = 163;
            this.lbRangeData.Text = "范围";
            this.lbRangeData.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(151, 18);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(70, 14);
            this.label12.TabIndex = 46;
            this.label12.Text = "图号/型号";
            // 
            // toolStripSeparatorUpdate
            // 
            this.toolStripSeparatorUpdate.Name = "toolStripSeparatorUpdate";
            this.toolStripSeparatorUpdate.Size = new System.Drawing.Size(6, 25);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 50);
            this.panel1.TabIndex = 24;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(421, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "零件选配信息";
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparatorAdd,
            this.btnUpdate,
            this.toolStripSeparatorDelete,
            this.btnDelete,
            this.toolStripSeparatorUpdate,
            this.btnChoseControlManage});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 37;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(68, 22);
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 22);
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(69, 22);
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnChoseControlManage
            // 
            this.btnChoseControlManage.Image = ((System.Drawing.Image)(resources.GetObject("btnChoseControlManage.Image")));
            this.btnChoseControlManage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnChoseControlManage.Name = "btnChoseControlManage";
            this.btnChoseControlManage.Size = new System.Drawing.Size(112, 22);
            this.btnChoseControlManage.Tag = "Add";
            this.btnChoseControlManage.Text = "选配表表头设计";
            this.btnChoseControlManage.Click += new System.EventHandler(this.btnChoseControlManage_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 99);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 442);
            this.dataGridView1.TabIndex = 30;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Controls.Add(this.panelPara);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 75);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1016, 552);
            this.panelCenter.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 541);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1016, 11);
            this.panel2.TabIndex = 27;
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Controls.Add(this.toolStrip1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1016, 627);
            this.panelMain.TabIndex = 34;
            // 
            // UserControlAccessoryChoseConfect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 627);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlAccessoryChoseConfect";
            this.Load += new System.EventHandler(this.UserControlAccessoryChoseConfect_Load);
            this.panelPara.ResumeLayout(false);
            this.panelPara.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMax)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMin)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelCenter.ResumeLayout(false);
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorUpdate;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.ToolStripButton btnChoseControlManage;
        private System.Windows.Forms.TextBox txtChoseConfect;
        private System.Windows.Forms.NumericUpDown numMax;
        private System.Windows.Forms.NumericUpDown numMin;
        private System.Windows.Forms.Label lbChoseData;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lbRangeData;
        private System.Windows.Forms.Button btnFindCode;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.Label label1;
    }
}
