namespace Form_Economic_Purchase
{
    partial class 价格清单
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dtpEnd = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label7 = new System.Windows.Forms.Label();
            this.btnSelect = new System.Windows.Forms.Button();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnInput = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.numRate = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numUnitPrice = new System.Windows.Forms.NumericUpDown();
            this.txtProvider = new UniversalControlLibrary.TextBoxShow();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpec = new UniversalControlLibrary.TextBoxShow();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGoodsName = new UniversalControlLibrary.TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.GoodsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.单价 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.有效期开始时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.有效期结束时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.税率 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.记录时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.记录员 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 157);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1008, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.dtpEnd);
            this.customGroupBox1.Controls.Add(this.label8);
            this.customGroupBox1.Controls.Add(this.dtpStart);
            this.customGroupBox1.Controls.Add(this.label7);
            this.customGroupBox1.Controls.Add(this.btnSelect);
            this.customGroupBox1.Controls.Add(this.btnOutput);
            this.customGroupBox1.Controls.Add(this.btnInput);
            this.customGroupBox1.Controls.Add(this.btnSave);
            this.customGroupBox1.Controls.Add(this.label6);
            this.customGroupBox1.Controls.Add(this.numRate);
            this.customGroupBox1.Controls.Add(this.label5);
            this.customGroupBox1.Controls.Add(this.numUnitPrice);
            this.customGroupBox1.Controls.Add(this.txtProvider);
            this.customGroupBox1.Controls.Add(this.label4);
            this.customGroupBox1.Controls.Add(this.txtSpec);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.txtGoodsName);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.txtGoodsCode);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(1008, 157);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "信息";
            // 
            // dtpEnd
            // 
            this.dtpEnd.Location = new System.Drawing.Point(417, 111);
            this.dtpEnd.Name = "dtpEnd";
            this.dtpEnd.Size = new System.Drawing.Size(139, 21);
            this.dtpEnd.TabIndex = 19;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(322, 115);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 12);
            this.label8.TabIndex = 18;
            this.label8.Text = "有效期结束时间";
            // 
            // dtpStart
            // 
            this.dtpStart.Location = new System.Drawing.Point(129, 111);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.Size = new System.Drawing.Size(139, 21);
            this.dtpStart.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(34, 115);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 12);
            this.label7.TabIndex = 16;
            this.label7.Text = "有效期开始时间";
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(803, 67);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(193, 23);
            this.btnSelect.TabIndex = 15;
            this.btnSelect.Text = "查看历史价格";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(929, 110);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(66, 23);
            this.btnOutput.TabIndex = 14;
            this.btnOutput.Text = "导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnInput
            // 
            this.btnInput.Location = new System.Drawing.Point(805, 110);
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(85, 23);
            this.btnInput.TabIndex = 13;
            this.btnInput.Text = "导入并保存";
            this.btnInput.UseVisualStyleBackColor = true;
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(697, 110);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(69, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(585, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 11;
            this.label6.Text = "税    率";
            // 
            // numRate
            // 
            this.numRate.Location = new System.Drawing.Point(644, 68);
            this.numRate.Name = "numRate";
            this.numRate.Size = new System.Drawing.Size(72, 21);
            this.numRate.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(322, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 12);
            this.label5.TabIndex = 9;
            this.label5.Text = "单价（含税）";
            // 
            // numUnitPrice
            // 
            this.numUnitPrice.DecimalPlaces = 4;
            this.numUnitPrice.Location = new System.Drawing.Point(417, 68);
            this.numUnitPrice.Maximum = new decimal(new int[] {
            -727379968,
            232,
            0,
            0});
            this.numUnitPrice.Name = "numUnitPrice";
            this.numUnitPrice.Size = new System.Drawing.Size(139, 21);
            this.numUnitPrice.TabIndex = 8;
            // 
            // txtProvider
            // 
            this.txtProvider.DataResult = null;
            this.txtProvider.DataTableResult = null;
            this.txtProvider.EditingControlDataGridView = null;
            this.txtProvider.EditingControlFormattedValue = "";
            this.txtProvider.EditingControlRowIndex = 0;
            this.txtProvider.EditingControlValueChanged = true;
            this.txtProvider.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.txtProvider.IsMultiSelect = false;
            this.txtProvider.Location = new System.Drawing.Point(93, 68);
            this.txtProvider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ShowResultForm = true;
            this.txtProvider.Size = new System.Drawing.Size(175, 21);
            this.txtProvider.StrEndSql = null;
            this.txtProvider.TabIndex = 7;
            this.txtProvider.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(34, 72);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "供 应 商";
            // 
            // txtSpec
            // 
            this.txtSpec.DataResult = null;
            this.txtSpec.DataTableResult = null;
            this.txtSpec.EditingControlDataGridView = null;
            this.txtSpec.EditingControlFormattedValue = "";
            this.txtSpec.EditingControlRowIndex = 0;
            this.txtSpec.EditingControlValueChanged = true;
            this.txtSpec.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtSpec.IsMultiSelect = false;
            this.txtSpec.Location = new System.Drawing.Point(644, 27);
            this.txtSpec.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ShowResultForm = false;
            this.txtSpec.Size = new System.Drawing.Size(175, 21);
            this.txtSpec.StrEndSql = null;
            this.txtSpec.TabIndex = 5;
            this.txtSpec.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(585, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "规   格";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.DataResult = null;
            this.txtGoodsName.DataTableResult = null;
            this.txtGoodsName.EditingControlDataGridView = null;
            this.txtGoodsName.EditingControlFormattedValue = "";
            this.txtGoodsName.EditingControlRowIndex = 0;
            this.txtGoodsName.EditingControlValueChanged = true;
            this.txtGoodsName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtGoodsName.IsMultiSelect = false;
            this.txtGoodsName.Location = new System.Drawing.Point(381, 27);
            this.txtGoodsName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ShowResultForm = false;
            this.txtGoodsName.Size = new System.Drawing.Size(175, 21);
            this.txtGoodsName.StrEndSql = null;
            this.txtGoodsName.TabIndex = 3;
            this.txtGoodsName.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(322, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "物品名称";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.DataTableResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsCode.IsMultiSelect = false;
            this.txtGoodsCode.Location = new System.Drawing.Point(93, 27);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(175, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 1;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            this.txtGoodsCode.Enter += new System.EventHandler(this.txtGoodsCode_Enter);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(34, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "图号型号";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GoodsID,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.供应商,
            this.单价,
            this.有效期开始时间,
            this.有效期结束时间,
            this.税率,
            this.记录时间,
            this.记录员});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 189);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(1008, 540);
            this.customDataGridView1.TabIndex = 3;
            this.customDataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellClick);
            // 
            // GoodsID
            // 
            this.GoodsID.DataPropertyName = "GoodsID";
            this.GoodsID.HeaderText = "GoodsID";
            this.GoodsID.Name = "GoodsID";
            this.GoodsID.ReadOnly = true;
            this.GoodsID.Visible = false;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "GoodsCode";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "GoodsName";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "Spec";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "Provider";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.ReadOnly = true;
            // 
            // 单价
            // 
            this.单价.DataPropertyName = "UnitPrice";
            this.单价.HeaderText = "单价";
            this.单价.Name = "单价";
            this.单价.ReadOnly = true;
            // 
            // 有效期开始时间
            // 
            this.有效期开始时间.DataPropertyName = "ValidityStart";
            this.有效期开始时间.HeaderText = "有效期开始时间";
            this.有效期开始时间.Name = "有效期开始时间";
            this.有效期开始时间.ReadOnly = true;
            // 
            // 有效期结束时间
            // 
            this.有效期结束时间.DataPropertyName = "ValidityEnd";
            this.有效期结束时间.HeaderText = "有效期结束时间";
            this.有效期结束时间.Name = "有效期结束时间";
            this.有效期结束时间.ReadOnly = true;
            // 
            // 税率
            // 
            this.税率.DataPropertyName = "Rate";
            this.税率.HeaderText = "税率";
            this.税率.Name = "税率";
            this.税率.ReadOnly = true;
            // 
            // 记录时间
            // 
            this.记录时间.DataPropertyName = "RecordTime";
            this.记录时间.HeaderText = "记录时间";
            this.记录时间.Name = "记录时间";
            this.记录时间.ReadOnly = true;
            // 
            // 记录员
            // 
            this.记录员.DataPropertyName = "Name";
            this.记录员.HeaderText = "记录员";
            this.记录员.Name = "记录员";
            this.记录员.ReadOnly = true;
            // 
            // 价格清单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "价格清单";
            this.Text = "价格清单";
            this.Load += new System.EventHandler(this.价格清单_Load);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numRate;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numUnitPrice;
        private UniversalControlLibrary.TextBoxShow txtProvider;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.TextBoxShow txtSpec;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtGoodsName;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnInput;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEnd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单价;
        private System.Windows.Forms.DataGridViewTextBoxColumn 有效期开始时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 有效期结束时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 税率;
        private System.Windows.Forms.DataGridViewTextBoxColumn 记录时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 记录员;
    }
}