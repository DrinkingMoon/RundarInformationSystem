namespace Expression
{
    partial class UserControlPurchaseStore
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
            this.components = new System.ComponentModel.Container();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.checkBox8 = new System.Windows.Forms.CheckBox();
            this.checkBox7 = new System.Windows.Forms.CheckBox();
            this.checkBox6 = new System.Windows.Forms.CheckBox();
            this.checkBox5 = new System.Windows.Forms.CheckBox();
            this.checkBox4 = new System.Windows.Forms.CheckBox();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label7 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtProduct = new UniversalControlLibrary.TextBoxShow();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbSumType = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStorage = new UniversalControlLibrary.TextBoxShow();
            this.cmbMonth = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.cmbYear = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.cmbSource = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label8 = new System.Windows.Forms.Label();
            this.rbMx = new System.Windows.Forms.RadioButton();
            this.rbHz = new System.Windows.Forms.RadioButton();
            this.rbZdy = new System.Windows.Forms.RadioButton();
            this.cmbZdy = new UniversalControlLibrary.CustomComboBox(this.components);
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.customGroupBox1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(433, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "库存查询";
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 571);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(986, 11);
            this.panel2.TabIndex = 27;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.customDataGridView1);
            this.panelCenter.Controls.Add(this.customGroupBox1);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 45);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(986, 582);
            this.panelCenter.TabIndex = 59;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 294);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(986, 277);
            this.customDataGridView1.TabIndex = 29;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.BackColor = System.Drawing.Color.White;
            this.customGroupBox1.Controls.Add(this.cmbZdy);
            this.customGroupBox1.Controls.Add(this.rbZdy);
            this.customGroupBox1.Controls.Add(this.rbHz);
            this.customGroupBox1.Controls.Add(this.rbMx);
            this.customGroupBox1.Controls.Add(this.label8);
            this.customGroupBox1.Controls.Add(this.checkBox8);
            this.customGroupBox1.Controls.Add(this.checkBox7);
            this.customGroupBox1.Controls.Add(this.checkBox6);
            this.customGroupBox1.Controls.Add(this.checkBox5);
            this.customGroupBox1.Controls.Add(this.checkBox4);
            this.customGroupBox1.Controls.Add(this.checkBox3);
            this.customGroupBox1.Controls.Add(this.checkBox2);
            this.customGroupBox1.Controls.Add(this.label7);
            this.customGroupBox1.Controls.Add(this.checkBox1);
            this.customGroupBox1.Controls.Add(this.btnOutput);
            this.customGroupBox1.Controls.Add(this.btnSelect);
            this.customGroupBox1.Controls.Add(this.txtProduct);
            this.customGroupBox1.Controls.Add(this.label6);
            this.customGroupBox1.Controls.Add(this.cmbSumType);
            this.customGroupBox1.Controls.Add(this.label5);
            this.customGroupBox1.Controls.Add(this.label4);
            this.customGroupBox1.Controls.Add(this.txtStorage);
            this.customGroupBox1.Controls.Add(this.cmbMonth);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.cmbYear);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.cmbSource);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(986, 294);
            this.customGroupBox1.TabIndex = 28;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "查询条件";
            // 
            // checkBox8
            // 
            this.checkBox8.AutoSize = true;
            this.checkBox8.Location = new System.Drawing.Point(763, 119);
            this.checkBox8.Name = "checkBox8";
            this.checkBox8.Size = new System.Drawing.Size(124, 18);
            this.checkBox8.TabIndex = 24;
            this.checkBox8.Tag = "7";
            this.checkBox8.Text = "仅限于售后备件";
            this.checkBox8.UseVisualStyleBackColor = true;
            // 
            // checkBox7
            // 
            this.checkBox7.AutoSize = true;
            this.checkBox7.Location = new System.Drawing.Point(619, 119);
            this.checkBox7.Name = "checkBox7";
            this.checkBox7.Size = new System.Drawing.Size(124, 18);
            this.checkBox7.TabIndex = 23;
            this.checkBox7.Tag = "6";
            this.checkBox7.Text = "仅限于返修箱用";
            this.checkBox7.UseVisualStyleBackColor = true;
            // 
            // checkBox6
            // 
            this.checkBox6.AutoSize = true;
            this.checkBox6.Location = new System.Drawing.Point(531, 119);
            this.checkBox6.Name = "checkBox6";
            this.checkBox6.Size = new System.Drawing.Size(68, 18);
            this.checkBox6.TabIndex = 22;
            this.checkBox6.Tag = "5";
            this.checkBox6.Text = "待处理";
            this.checkBox6.UseVisualStyleBackColor = true;
            // 
            // checkBox5
            // 
            this.checkBox5.AutoSize = true;
            this.checkBox5.Location = new System.Drawing.Point(383, 119);
            this.checkBox5.Name = "checkBox5";
            this.checkBox5.Size = new System.Drawing.Size(54, 18);
            this.checkBox5.TabIndex = 21;
            this.checkBox5.Tag = "3";
            this.checkBox5.Text = "隔离";
            this.checkBox5.UseVisualStyleBackColor = true;
            // 
            // checkBox4
            // 
            this.checkBox4.AutoSize = true;
            this.checkBox4.Location = new System.Drawing.Point(457, 119);
            this.checkBox4.Name = "checkBox4";
            this.checkBox4.Size = new System.Drawing.Size(54, 18);
            this.checkBox4.TabIndex = 20;
            this.checkBox4.Tag = "4";
            this.checkBox4.Text = "报废";
            this.checkBox4.UseVisualStyleBackColor = true;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(309, 119);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(54, 18);
            this.checkBox3.TabIndex = 19;
            this.checkBox3.Tag = "2";
            this.checkBox3.Text = "样品";
            this.checkBox3.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(207, 119);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(82, 18);
            this.checkBox2.TabIndex = 18;
            this.checkBox2.Tag = "1";
            this.checkBox2.Text = "正在挑选";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(47, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 14);
            this.label7.TabIndex = 17;
            this.label7.Text = "物品状态：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Location = new System.Drawing.Point(133, 119);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(54, 18);
            this.checkBox1.TabIndex = 16;
            this.checkBox1.Tag = "0";
            this.checkBox1.Text = "正常";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(798, 27);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(95, 23);
            this.btnOutput.TabIndex = 15;
            this.btnOutput.Text = "导出Excel";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(681, 27);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(95, 23);
            this.btnSelect.TabIndex = 14;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtProduct
            // 
            this.txtProduct.DataResult = null;
            this.txtProduct.DataTableResult = null;
            this.txtProduct.EditingControlDataGridView = null;
            this.txtProduct.EditingControlFormattedValue = "";
            this.txtProduct.EditingControlRowIndex = 0;
            this.txtProduct.EditingControlValueChanged = true;
            this.txtProduct.Enabled = false;
            this.txtProduct.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtProduct.IsMultiSelect = true;
            this.txtProduct.Location = new System.Drawing.Point(354, 206);
            this.txtProduct.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProduct.Multiline = true;
            this.txtProduct.Name = "txtProduct";
            this.txtProduct.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtProduct.ShowResultForm = true;
            this.txtProduct.Size = new System.Drawing.Size(533, 58);
            this.txtProduct.StrEndSql = null;
            this.txtProduct.TabIndex = 12;
            this.txtProduct.TabStop = false;
            this.txtProduct.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProduct_OnCompleteSearch);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(283, 228);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "产品类型";
            // 
            // cmbSumType
            // 
            this.cmbSumType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSumType.Enabled = false;
            this.cmbSumType.FormattingEnabled = true;
            this.cmbSumType.Items.AddRange(new object[] {
            "库房信息",
            "采购BOM",
            "装配BOM",
            "设计BOM",
            "发料清单"});
            this.cmbSumType.Location = new System.Drawing.Point(133, 225);
            this.cmbSumType.MaxYear = 0;
            this.cmbSumType.MinYear = 0;
            this.cmbSumType.Name = "cmbSumType";
            this.cmbSumType.Size = new System.Drawing.Size(124, 21);
            this.cmbSumType.TabIndex = 10;
            this.cmbSumType.SelectedIndexChanged += new System.EventHandler(this.cmbSumType_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(47, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "汇总方式：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(47, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 14);
            this.label4.TabIndex = 7;
            this.label4.Text = "所属库房：";
            // 
            // txtStorage
            // 
            this.txtStorage.DataResult = null;
            this.txtStorage.DataTableResult = null;
            this.txtStorage.EditingControlDataGridView = null;
            this.txtStorage.EditingControlFormattedValue = "";
            this.txtStorage.EditingControlRowIndex = 0;
            this.txtStorage.EditingControlValueChanged = true;
            this.txtStorage.FindItem = UniversalControlLibrary.TextBoxShow.FindType.库房;
            this.txtStorage.IsMultiSelect = true;
            this.txtStorage.Location = new System.Drawing.Point(133, 72);
            this.txtStorage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtStorage.Name = "txtStorage";
            this.txtStorage.ShowResultForm = true;
            this.txtStorage.Size = new System.Drawing.Size(758, 23);
            this.txtStorage.StrEndSql = null;
            this.txtStorage.TabIndex = 6;
            this.txtStorage.TabStop = false;
            this.txtStorage.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtStorage_OnCompleteSearch);
            // 
            // cmbMonth
            // 
            this.cmbMonth.Enabled = false;
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
            this.cmbMonth.Location = new System.Drawing.Point(455, 28);
            this.cmbMonth.MaxYear = 0;
            this.cmbMonth.MinYear = 0;
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(54, 21);
            this.cmbMonth.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(414, 31);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "月份";
            // 
            // cmbYear
            // 
            this.cmbYear.Enabled = false;
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(324, 28);
            this.cmbYear.MaxYear = 0;
            this.cmbYear.MinYear = 0;
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(75, 21);
            this.cmbYear.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(283, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "年份";
            // 
            // cmbSource
            // 
            this.cmbSource.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSource.FormattingEnabled = true;
            this.cmbSource.Items.AddRange(new object[] {
            "当前库存",
            "月度库存"});
            this.cmbSource.Location = new System.Drawing.Point(133, 28);
            this.cmbSource.MaxYear = 0;
            this.cmbSource.MinYear = 0;
            this.cmbSource.Name = "cmbSource";
            this.cmbSource.Size = new System.Drawing.Size(124, 21);
            this.cmbSource.TabIndex = 1;
            this.cmbSource.SelectedIndexChanged += new System.EventHandler(this.cmbSource_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "数据来源：";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(986, 45);
            this.panel1.TabIndex = 60;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(47, 167);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 14);
            this.label8.TabIndex = 25;
            this.label8.Text = "查询方式：";
            // 
            // rbMx
            // 
            this.rbMx.AutoSize = true;
            this.rbMx.Checked = true;
            this.rbMx.Location = new System.Drawing.Point(133, 165);
            this.rbMx.Name = "rbMx";
            this.rbMx.Size = new System.Drawing.Size(81, 18);
            this.rbMx.TabIndex = 26;
            this.rbMx.TabStop = true;
            this.rbMx.Text = "明细查询";
            this.rbMx.UseVisualStyleBackColor = true;
            // 
            // rbHz
            // 
            this.rbHz.AutoSize = true;
            this.rbHz.Location = new System.Drawing.Point(236, 165);
            this.rbHz.Name = "rbHz";
            this.rbHz.Size = new System.Drawing.Size(81, 18);
            this.rbHz.TabIndex = 27;
            this.rbHz.Text = "汇总查询";
            this.rbHz.UseVisualStyleBackColor = true;
            this.rbHz.CheckedChanged += new System.EventHandler(this.rbHz_CheckedChanged);
            // 
            // rbZdy
            // 
            this.rbZdy.AutoSize = true;
            this.rbZdy.Location = new System.Drawing.Point(339, 165);
            this.rbZdy.Name = "rbZdy";
            this.rbZdy.Size = new System.Drawing.Size(95, 18);
            this.rbZdy.TabIndex = 28;
            this.rbZdy.Text = "自定义匹配";
            this.rbZdy.UseVisualStyleBackColor = true;
            // 
            // cmbZdy
            // 
            this.cmbZdy.FormattingEnabled = true;
            this.cmbZdy.Items.AddRange(new object[] {
            "库房信息",
            "采购BOM",
            "装配BOM",
            "设计BOM",
            "发料清单"});
            this.cmbZdy.Location = new System.Drawing.Point(440, 164);
            this.cmbZdy.MaxYear = 0;
            this.cmbZdy.MinYear = 0;
            this.cmbZdy.Name = "cmbZdy";
            this.cmbZdy.Size = new System.Drawing.Size(208, 21);
            this.cmbZdy.TabIndex = 29;
            // 
            // UserControlPurchaseStore
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 627);
            this.Controls.Add(this.panelCenter);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlPurchaseStore";
            this.Load += new System.EventHandler(this.UserControlPurchaseStore_Load);
            this.panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panel1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private UniversalControlLibrary.CustomComboBox cmbMonth;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.CustomComboBox cmbYear;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.CustomComboBox cmbSource;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnSelect;
        private UniversalControlLibrary.TextBoxShow txtProduct;
        private System.Windows.Forms.Label label6;
        private UniversalControlLibrary.CustomComboBox cmbSumType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.TextBoxShow txtStorage;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox8;
        private System.Windows.Forms.CheckBox checkBox7;
        private System.Windows.Forms.CheckBox checkBox6;
        private System.Windows.Forms.CheckBox checkBox5;
        private System.Windows.Forms.CheckBox checkBox4;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private UniversalControlLibrary.CustomComboBox cmbZdy;
        private System.Windows.Forms.RadioButton rbZdy;
        private System.Windows.Forms.RadioButton rbHz;
        private System.Windows.Forms.RadioButton rbMx;
        private System.Windows.Forms.Label label8;
    }
}
