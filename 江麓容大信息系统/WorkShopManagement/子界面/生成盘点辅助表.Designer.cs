namespace Form_Manufacture_WorkShop
{
    partial class 生成盘点辅助表
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnCreate = new System.Windows.Forms.Button();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.numProductCount = new System.Windows.Forms.NumericUpDown();
            this.txtSpec = new UniversalControlLibrary.TextBoxShow();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGoodsName = new UniversalControlLibrary.TextBoxShow();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.产品图号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProductCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnCreate);
            this.panel1.Controls.Add(this.dtpEndTime);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.cmbMonth);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.cmbYear);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(677, 53);
            this.panel1.TabIndex = 0;
            // 
            // btnCreate
            // 
            this.btnCreate.Location = new System.Drawing.Point(564, 16);
            this.btnCreate.Name = "btnCreate";
            this.btnCreate.Size = new System.Drawing.Size(75, 23);
            this.btnCreate.TabIndex = 191;
            this.btnCreate.Text = "生成";
            this.btnCreate.UseVisualStyleBackColor = true;
            this.btnCreate.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(386, 17);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(153, 21);
            this.dtpEndTime.TabIndex = 190;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(317, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 189;
            this.label1.Text = "截止日期";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(157, 21);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 188;
            this.label8.Text = "月份";
            // 
            // cmbMonth
            // 
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbMonth.Location = new System.Drawing.Point(198, 17);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(76, 20);
            this.cmbMonth.TabIndex = 187;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(19, 21);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(29, 12);
            this.label10.TabIndex = 186;
            this.label10.Text = "年份";
            // 
            // cmbYear
            // 
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(60, 17);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(76, 20);
            this.cmbYear.TabIndex = 185;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnDelete);
            this.panel2.Controls.Add(this.btnAdd);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.numProductCount);
            this.panel2.Controls.Add(this.txtSpec);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtGoodsName);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.txtGoodsCode);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(0, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(218, 259);
            this.panel2.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(123, 223);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 197;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(23, 223);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 192;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 164);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 196;
            this.label5.Text = "产品数量";
            // 
            // numProductCount
            // 
            this.numProductCount.Location = new System.Drawing.Point(23, 184);
            this.numProductCount.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.numProductCount.Name = "numProductCount";
            this.numProductCount.Size = new System.Drawing.Size(120, 21);
            this.numProductCount.TabIndex = 192;
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
            this.txtSpec.Location = new System.Drawing.Point(23, 131);
            this.txtSpec.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.ShowResultForm = false;
            this.txtSpec.Size = new System.Drawing.Size(175, 21);
            this.txtSpec.StrEndSql = null;
            this.txtSpec.TabIndex = 195;
            this.txtSpec.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 194;
            this.label4.Text = "产品规格";
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
            this.txtGoodsName.Location = new System.Drawing.Point(23, 83);
            this.txtGoodsName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ReadOnly = true;
            this.txtGoodsName.ShowResultForm = false;
            this.txtGoodsName.Size = new System.Drawing.Size(175, 21);
            this.txtGoodsName.StrEndSql = null;
            this.txtGoodsName.TabIndex = 193;
            this.txtGoodsName.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 192;
            this.label3.Text = "产品名称";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.DataTableResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.营销物品;
            this.txtGoodsCode.IsMultiSelect = false;
            this.txtGoodsCode.Location = new System.Drawing.Point(23, 33);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(175, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 191;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 190;
            this.label2.Text = "产品图号";
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
            this.产品图号,
            this.产品名称,
            this.产品规格,
            this.产品数量,
            this.物品ID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(218, 53);
            this.customDataGridView1.MultiSelect = false;
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(459, 259);
            this.customDataGridView1.TabIndex = 3;
            // 
            // 产品图号
            // 
            this.产品图号.HeaderText = "产品图号";
            this.产品图号.Name = "产品图号";
            this.产品图号.ReadOnly = true;
            // 
            // 产品名称
            // 
            this.产品名称.HeaderText = "产品名称";
            this.产品名称.Name = "产品名称";
            this.产品名称.ReadOnly = true;
            // 
            // 产品规格
            // 
            this.产品规格.HeaderText = "产品规格";
            this.产品规格.Name = "产品规格";
            this.产品规格.ReadOnly = true;
            // 
            // 产品数量
            // 
            this.产品数量.HeaderText = "产品数量";
            this.产品数量.Name = "产品数量";
            this.产品数量.ReadOnly = true;
            // 
            // 物品ID
            // 
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // 生成盘点辅助表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 312);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "生成盘点辅助表";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "生成盘点辅助表";
            this.Load += new System.EventHandler(this.生成盘点辅助表_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProductCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Panel panel2;
        private UniversalControlLibrary.TextBoxShow txtGoodsName;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numProductCount;
        private UniversalControlLibrary.TextBoxShow txtSpec;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnCreate;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品图号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
    }
}