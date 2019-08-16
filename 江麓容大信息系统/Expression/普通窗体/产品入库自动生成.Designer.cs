namespace Expression
{
    partial class 产品入库自动生成
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(产品入库自动生成));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnCreate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbProduct = new System.Windows.Forms.ComboBox();
            this.lbConnectInfo = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbRKFS = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.dataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.产品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.箱体编号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库方式 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库库房 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.库房ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCreate,
            this.toolStripSeparator2,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(584, 25);
            this.toolStrip1.TabIndex = 48;
            this.toolStrip1.Text = "toolStrip2";
            // 
            // btnCreate
            // 
            this.btnCreate.Image = ((System.Drawing.Image)(resources.GetObject("btnCreate.Image")));
            this.btnCreate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCreate.Size = new System.Drawing.Size(92, 22);
            this.btnCreate.Text = "自动生成(&C)";
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(68, 22);
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbProduct);
            this.groupBox1.Controls.Add(this.lbConnectInfo);
            this.groupBox1.Controls.Add(this.txtProductCode);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.cmbRKFS);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbStorage);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(584, 129);
            this.groupBox1.TabIndex = 49;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息填写区";
            // 
            // cmbProduct
            // 
            this.cmbProduct.FormattingEnabled = true;
            this.cmbProduct.Location = new System.Drawing.Point(323, 30);
            this.cmbProduct.Name = "cmbProduct";
            this.cmbProduct.Size = new System.Drawing.Size(156, 20);
            this.cmbProduct.TabIndex = 216;
            this.cmbProduct.SelectedIndexChanged += new System.EventHandler(this.cmbProduct_SelectedIndexChanged);
            // 
            // lbConnectInfo
            // 
            this.lbConnectInfo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbConnectInfo.ForeColor = System.Drawing.Color.Red;
            this.lbConnectInfo.Location = new System.Drawing.Point(17, 100);
            this.lbConnectInfo.Name = "lbConnectInfo";
            this.lbConnectInfo.Size = new System.Drawing.Size(464, 19);
            this.lbConnectInfo.TabIndex = 215;
            this.lbConnectInfo.Text = "客户端连接信息显示区";
            this.lbConnectInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(80, 64);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(156, 21);
            this.txtProductCode.TabIndex = 214;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 68);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 213;
            this.label12.Text = "箱体编码";
            // 
            // cmbRKFS
            // 
            this.cmbRKFS.FormattingEnabled = true;
            this.cmbRKFS.Items.AddRange(new object[] {
            "未知",
            "生产入库",
            "生产返修入库",
            "0公里返修入库",
            "售后返修入库",
            "三包配件入库",
            "批量生产入库",
            "0公里批量返修入库"});
            this.cmbRKFS.Location = new System.Drawing.Point(323, 64);
            this.cmbRKFS.Name = "cmbRKFS";
            this.cmbRKFS.Size = new System.Drawing.Size(156, 20);
            this.cmbRKFS.TabIndex = 212;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(264, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 211;
            this.label6.Text = "入库方式";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(264, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 209;
            this.label5.Text = "产品名称";
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(80, 30);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(156, 20);
            this.cmbStorage.TabIndex = 208;
            this.cmbStorage.SelectedIndexChanged += new System.EventHandler(this.cmbStorage_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(18, 34);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 207;
            this.label13.Text = "入库库房";
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(498, 29);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(69, 23);
            this.btnAdd.TabIndex = 38;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(498, 62);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 23);
            this.btnDelete.TabIndex = 37;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoCreateFilters = true;
            this.dataGridView1.BaseFilter = "";
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.产品名称,
            this.箱体编号,
            this.入库方式,
            this.入库库房,
            this.库房ID,
            this.物品ID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 154);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 40;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(584, 362);
            this.dataGridView1.TabIndex = 50;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // 产品名称
            // 
            this.产品名称.DataPropertyName = "产品名称";
            this.产品名称.HeaderText = "产品名称";
            this.产品名称.Name = "产品名称";
            this.产品名称.ReadOnly = true;
            // 
            // 箱体编号
            // 
            this.箱体编号.DataPropertyName = "箱体编号";
            this.箱体编号.HeaderText = "箱体编号";
            this.箱体编号.Name = "箱体编号";
            this.箱体编号.ReadOnly = true;
            // 
            // 入库方式
            // 
            this.入库方式.DataPropertyName = "入库方式";
            this.入库方式.HeaderText = "入库方式";
            this.入库方式.Name = "入库方式";
            this.入库方式.ReadOnly = true;
            // 
            // 入库库房
            // 
            this.入库库房.DataPropertyName = "入库库房";
            this.入库库房.HeaderText = "入库库房";
            this.入库库房.Name = "入库库房";
            this.入库库房.ReadOnly = true;
            // 
            // 库房ID
            // 
            this.库房ID.DataPropertyName = "库房ID";
            this.库房ID.HeaderText = "库房ID";
            this.库房ID.Name = "库房ID";
            this.库房ID.ReadOnly = true;
            this.库房ID.Visible = false;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 产品入库自动生成
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 516);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "产品入库自动生成";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "产品入库自动生成";
            this.Load += new System.EventHandler(this.产品入库自动生成_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.产品入库自动生成_FormClosing);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnCreate;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private UniversalControlLibrary.CustomDataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbRKFS;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbConnectInfo;
        private System.Windows.Forms.ComboBox cmbProduct;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 箱体编号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库方式;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库库房;
        private System.Windows.Forms.DataGridViewTextBoxColumn 库房ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
    }
}