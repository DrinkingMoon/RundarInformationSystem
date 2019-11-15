namespace UniversalControlLibrary
{
    partial class 产品编号
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.txtBoxNo = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProductCode = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnAuditing = new System.Windows.Forms.Button();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BoxNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ZcCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lbConnectInfo = new System.Windows.Forms.Label();
            this.btnPrintSelect = new System.Windows.Forms.Button();
            this.btnPrintAll = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(304, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 455);
            this.panel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnPrintAll);
            this.groupBox1.Controls.Add(this.btnPrintSelect);
            this.groupBox1.Controls.Add(this.btnInput);
            this.groupBox1.Controls.Add(this.lbConnectInfo);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnOutput);
            this.groupBox1.Controls.Add(this.txtBoxNo);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtCount);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtProductCode);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtBatchNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnAuditing);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtGoodsName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(230, 455);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "产品信息";
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(20, 323);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(83, 27);
            this.btnInput.TabIndex = 38;
            this.btnInput.Text = "Excel导入";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(20, 286);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(83, 27);
            this.btnAdd.TabIndex = 36;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(135, 286);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(83, 27);
            this.btnDelete.TabIndex = 35;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(135, 323);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(83, 27);
            this.btnOutput.TabIndex = 34;
            this.btnOutput.Text = "Excel导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // txtBoxNo
            // 
            this.txtBoxNo.Location = new System.Drawing.Point(60, 254);
            this.txtBoxNo.Name = "txtBoxNo";
            this.txtBoxNo.Size = new System.Drawing.Size(158, 21);
            this.txtBoxNo.TabIndex = 33;
            this.txtBoxNo.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBoxNo_KeyDown);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 258);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 32;
            this.label12.Text = "整 箱 号";
            // 
            // txtCount
            // 
            this.txtCount.Enabled = false;
            this.txtCount.Location = new System.Drawing.Point(60, 152);
            this.txtCount.Name = "txtCount";
            this.txtCount.ReadOnly = true;
            this.txtCount.Size = new System.Drawing.Size(158, 21);
            this.txtCount.TabIndex = 31;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 156);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 30;
            this.label7.Text = "数    量";
            // 
            // txtProductCode
            // 
            this.txtProductCode.Location = new System.Drawing.Point(60, 220);
            this.txtProductCode.Name = "txtProductCode";
            this.txtProductCode.Size = new System.Drawing.Size(158, 21);
            this.txtProductCode.TabIndex = 1;
            this.txtProductCode.Tag = "";
            this.txtProductCode.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtProductCode_KeyDown);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(4, 224);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 28;
            this.label6.Text = "产品编号";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(60, 186);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(158, 21);
            this.txtCode.TabIndex = 27;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(4, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 26;
            this.label5.Text = "型    号";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Enabled = false;
            this.txtBatchNo.Location = new System.Drawing.Point(60, 118);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.ReadOnly = true;
            this.txtBatchNo.Size = new System.Drawing.Size(158, 21);
            this.txtBatchNo.TabIndex = 25;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(4, 122);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 24;
            this.label4.Text = "批 次 号";
            // 
            // btnAuditing
            // 
            this.btnAuditing.Location = new System.Drawing.Point(20, 404);
            this.btnAuditing.Name = "btnAuditing";
            this.btnAuditing.Size = new System.Drawing.Size(198, 27);
            this.btnAuditing.TabIndex = 14;
            this.btnAuditing.Text = "提交";
            this.btnAuditing.UseVisualStyleBackColor = true;
            this.btnAuditing.Click += new System.EventHandler(this.btnAuditing_Click);
            // 
            // txtSpec
            // 
            this.txtSpec.Enabled = false;
            this.txtSpec.Location = new System.Drawing.Point(60, 84);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(158, 21);
            this.txtSpec.TabIndex = 23;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 22;
            this.label3.Text = "规    格";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.Enabled = false;
            this.txtGoodsCode.Location = new System.Drawing.Point(60, 50);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ReadOnly = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(158, 21);
            this.txtGoodsCode.TabIndex = 21;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 20;
            this.label2.Text = "图号型号";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Enabled = false;
            this.txtGoodsName.Location = new System.Drawing.Point(60, 16);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ReadOnly = true;
            this.txtGoodsName.Size = new System.Drawing.Size(158, 21);
            this.txtGoodsName.TabIndex = 19;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 18;
            this.label1.Text = "产品名称";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(304, 455);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.ProductCode,
            this.BoxNo,
            this.Code,
            this.ZcCode});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 38;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(304, 455);
            this.dataGridView1.TabIndex = 0;
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Number";
            this.Number.HeaderText = "序号";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            this.Number.Width = 37;
            // 
            // ProductCode
            // 
            this.ProductCode.DataPropertyName = "ProductCode";
            this.ProductCode.HeaderText = "出厂流水号";
            this.ProductCode.Name = "ProductCode";
            this.ProductCode.ReadOnly = true;
            this.ProductCode.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ProductCode.Width = 130;
            // 
            // BoxNo
            // 
            this.BoxNo.DataPropertyName = "BoxNo";
            this.BoxNo.HeaderText = "箱体编码";
            this.BoxNo.Name = "BoxNo";
            this.BoxNo.ReadOnly = true;
            this.BoxNo.Width = 120;
            // 
            // Code
            // 
            this.Code.DataPropertyName = "Code";
            this.Code.HeaderText = "Code";
            this.Code.Name = "Code";
            this.Code.ReadOnly = true;
            this.Code.Visible = false;
            // 
            // ZcCode
            // 
            this.ZcCode.DataPropertyName = "ZcCode";
            this.ZcCode.HeaderText = "ZcCode";
            this.ZcCode.Name = "ZcCode";
            this.ZcCode.ReadOnly = true;
            this.ZcCode.Visible = false;
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
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "Number";
            this.dataGridViewTextBoxColumn1.HeaderText = "序号";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 37;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "ProductCode";
            this.dataGridViewTextBoxColumn2.HeaderText = "出厂流水号";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 130;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "BoxNo";
            this.dataGridViewTextBoxColumn3.HeaderText = "箱体编码";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 120;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "Code";
            this.dataGridViewTextBoxColumn4.HeaderText = "Code";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Visible = false;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "ZcCode";
            this.dataGridViewTextBoxColumn5.HeaderText = "ZcCode";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // lbConnectInfo
            // 
            this.lbConnectInfo.AutoSize = true;
            this.lbConnectInfo.Font = new System.Drawing.Font("宋体", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbConnectInfo.ForeColor = System.Drawing.Color.Red;
            this.lbConnectInfo.Location = new System.Drawing.Point(67, 439);
            this.lbConnectInfo.Name = "lbConnectInfo";
            this.lbConnectInfo.Size = new System.Drawing.Size(105, 10);
            this.lbConnectInfo.TabIndex = 37;
            this.lbConnectInfo.Text = "客户端连接信息显示区";
            this.lbConnectInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // btnPrintSelect
            // 
            this.btnPrintSelect.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrintSelect.Location = new System.Drawing.Point(20, 360);
            this.btnPrintSelect.Name = "btnPrintSelect";
            this.btnPrintSelect.Size = new System.Drawing.Size(83, 34);
            this.btnPrintSelect.TabIndex = 39;
            this.btnPrintSelect.Text = "打印选择箱体编码";
            this.btnPrintSelect.UseVisualStyleBackColor = true;
            this.btnPrintSelect.Click += new System.EventHandler(this.btnPrintSelect_Click);
            // 
            // btnPrintAll
            // 
            this.btnPrintAll.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPrintAll.Location = new System.Drawing.Point(135, 360);
            this.btnPrintAll.Name = "btnPrintAll";
            this.btnPrintAll.Size = new System.Drawing.Size(83, 34);
            this.btnPrintAll.TabIndex = 40;
            this.btnPrintAll.Text = "打印所有箱体编码";
            this.btnPrintAll.UseVisualStyleBackColor = true;
            this.btnPrintAll.Click += new System.EventHandler(this.btnPrintAll_Click);
            // 
            // 产品编号
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(534, 455);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "产品编号";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "产品编号";
            this.Load += new System.EventHandler(this.产品编号_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.产品编号_FormClosing);
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProductCode;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAuditing;
        private System.Windows.Forms.TextBox txtBoxNo;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn BoxNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn ZcCode;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.Button btnPrintSelect;
        private System.Windows.Forms.Label lbConnectInfo;
        private System.Windows.Forms.Button btnPrintAll;
    }
}