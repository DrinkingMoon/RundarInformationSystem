namespace Expression
{
    partial class 电子档案录入
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnAllFind = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.txtProductOnlyCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ParentCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Counts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckDatas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FactDatas = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FittingPersonnel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FittingTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmendPersonnel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmendTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkBench = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAllFind);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.btnSave);
            this.groupBox1.Controls.Add(this.txtProductOnlyCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmbProductType);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1016, 66);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息录入";
            // 
            // btnAllFind
            // 
            this.btnAllFind.Location = new System.Drawing.Point(821, 27);
            this.btnAllFind.Name = "btnAllFind";
            this.btnAllFind.Size = new System.Drawing.Size(78, 23);
            this.btnAllFind.TabIndex = 6;
            this.btnAllFind.Text = "全部查询";
            this.btnAllFind.UseVisualStyleBackColor = true;
            this.btnAllFind.Click += new System.EventHandler(this.btnAllFind_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(514, 28);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 5;
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(616, 28);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtProductOnlyCode
            // 
            this.txtProductOnlyCode.Location = new System.Drawing.Point(294, 29);
            this.txtProductOnlyCode.Name = "txtProductOnlyCode";
            this.txtProductOnlyCode.Size = new System.Drawing.Size(147, 21);
            this.txtProductOnlyCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(235, 33);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "产品编号";
            // 
            // cmbProductType
            // 
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Items.AddRange(new object[] {
            "RDC15-FB"});
            this.cmbProductType.Location = new System.Drawing.Point(85, 29);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(121, 20);
            this.cmbProductType.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "产品型号";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.treeView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox2.Location = new System.Drawing.Point(0, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(225, 550);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "总成树";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.ItemHeight = 16;
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(219, 530);
            this.treeView1.TabIndex = 0;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(225, 66);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 550);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(228, 66);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(788, 550);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "零件信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.ProductCode,
            this.ParentCode,
            this.GoodsCode,
            this.GoodsName,
            this.Spec,
            this.BatchNo,
            this.Counts,
            this.CheckDatas,
            this.FactDatas,
            this.FittingPersonnel,
            this.FittingTime,
            this.AmendPersonnel,
            this.AmendTime,
            this.WorkBench,
            this.Remark});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(782, 530);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "ID";
            this.ID.Name = "ID";
            this.ID.ReadOnly = true;
            this.ID.Visible = false;
            // 
            // ProductCode
            // 
            this.ProductCode.DataPropertyName = "ProductCode";
            this.ProductCode.HeaderText = "ProductCode";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.ReadOnly = true;
            this.ProductCode.Visible = false;
            // 
            // ParentCode
            // 
            this.ParentCode.DataPropertyName = "ParentCode";
            this.ParentCode.HeaderText = "ParentCode";
            this.ParentCode.Name = "ParentCode";
            this.ParentCode.ReadOnly = true;
            this.ParentCode.Visible = false;
            // 
            // GoodsCode
            // 
            this.GoodsCode.DataPropertyName = "GoodsCode";
            this.GoodsCode.HeaderText = "图号型号";
            this.GoodsCode.Name = "GoodsCode";
            this.GoodsCode.ReadOnly = true;
            // 
            // GoodsName
            // 
            this.GoodsName.DataPropertyName = "GoodsName";
            this.GoodsName.HeaderText = "物品名称";
            this.GoodsName.Name = "GoodsName";
            this.GoodsName.ReadOnly = true;
            // 
            // Spec
            // 
            this.Spec.DataPropertyName = "Spec";
            this.Spec.HeaderText = "规格";
            this.Spec.Name = "Spec";
            this.Spec.ReadOnly = true;
            // 
            // BatchNo
            // 
            this.BatchNo.DataPropertyName = "BatchNo";
            this.BatchNo.HeaderText = "批次号";
            this.BatchNo.Name = "BatchNo";
            // 
            // Counts
            // 
            this.Counts.DataPropertyName = "Counts";
            this.Counts.HeaderText = "数量";
            this.Counts.Name = "Counts";
            // 
            // CheckDatas
            // 
            this.CheckDatas.DataPropertyName = "CheckDatas";
            this.CheckDatas.HeaderText = "检查数据";
            this.CheckDatas.Name = "CheckDatas";
            // 
            // FactDatas
            // 
            this.FactDatas.DataPropertyName = "FactDatas";
            this.FactDatas.HeaderText = "实际数据";
            this.FactDatas.Name = "FactDatas";
            // 
            // FittingPersonnel
            // 
            this.FittingPersonnel.DataPropertyName = "FittingPersonnel";
            this.FittingPersonnel.HeaderText = "操作人员";
            this.FittingPersonnel.Name = "FittingPersonnel";
            // 
            // FittingTime
            // 
            this.FittingTime.DataPropertyName = "FittingTime";
            this.FittingTime.HeaderText = "操作时间";
            this.FittingTime.Name = "FittingTime";
            // 
            // AmendPersonnel
            // 
            this.AmendPersonnel.DataPropertyName = "AmendPersonnel";
            this.AmendPersonnel.HeaderText = "检验人员";
            this.AmendPersonnel.Name = "AmendPersonnel";
            // 
            // AmendTime
            // 
            this.AmendTime.DataPropertyName = "AmendTime";
            this.AmendTime.HeaderText = "检验时间";
            this.AmendTime.Name = "AmendTime";
            // 
            // WorkBench
            // 
            this.WorkBench.DataPropertyName = "WorkBench";
            this.WorkBench.HeaderText = "工位";
            this.WorkBench.Name = "WorkBench";
            // 
            // Remark
            // 
            this.Remark.DataPropertyName = "Remark";
            this.Remark.HeaderText = "备注";
            this.Remark.Name = "Remark";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(715, 27);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // 电子档案录入
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 616);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "电子档案录入";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "电子档案录入";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.电子档案录入_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.TextBox txtProductOnlyCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ParentCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Counts;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckDatas;
        private System.Windows.Forms.DataGridViewTextBoxColumn FactDatas;
        private System.Windows.Forms.DataGridViewTextBoxColumn FittingPersonnel;
        private System.Windows.Forms.DataGridViewTextBoxColumn FittingTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmendPersonnel;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmendTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkBench;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnAllFind;
        private System.Windows.Forms.Button btnDelete;
    }
}