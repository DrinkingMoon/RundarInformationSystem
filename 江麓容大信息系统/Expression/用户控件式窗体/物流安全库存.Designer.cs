using UniversalControlLibrary;
namespace Expression
{
    partial class 物流安全库存
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnModify = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOutExcel = new System.Windows.Forms.ToolStripButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numMinValue = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.numMaxValue = new System.Windows.Forms.NumericUpDown();
            this.txtSpce = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.tbsGoods = new TextBoxShow();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.rbNomarl = new System.Windows.Forms.RadioButton();
            this.rbMoreMax = new System.Windows.Forms.RadioButton();
            this.rbLessMin = new System.Windows.Forms.RadioButton();
            this.toolStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparator3,
            this.btnModify,
            this.toolStripSeparator4,
            this.btnDelete,
            this.toolStripSeparator6,
            this.btnRefresh,
            this.toolStripSeparator7,
            this.btnOutExcel});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(67, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnModify
            // 
            this.btnModify.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(67, 22);
            this.btnModify.Tag = "Add";
            this.btnModify.Text = "修改(&M)";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 22);
            this.btnDelete.Tag = "Add";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 22);
            this.btnRefresh.Tag = "View";
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnOutExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(67, 22);
            this.btnOutExcel.Tag = "Add";
            this.btnOutExcel.Text = "导出(&E)";
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 25);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1016, 44);
            this.panelTop.TabIndex = 2;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTitle.Location = new System.Drawing.Point(421, 9);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "物流安全库存";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbLessMin);
            this.groupBox1.Controls.Add(this.rbMoreMax);
            this.groupBox1.Controls.Add(this.rbNomarl);
            this.groupBox1.Controls.Add(this.rbAll);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numMinValue);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.numMaxValue);
            this.groupBox1.Controls.Add(this.tbsGoods);
            this.groupBox1.Controls.Add(this.txtSpce);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 69);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1016, 133);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息录入";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemark.ForeColor = System.Drawing.Color.Black;
            this.txtRemark.Location = new System.Drawing.Point(89, 94);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(890, 23);
            this.txtRemark.TabIndex = 233;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(20, 97);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 232;
            this.label3.Text = "备    注";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(339, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 231;
            this.label2.Text = "下    限";
            // 
            // numMinValue
            // 
            this.numMinValue.DecimalPlaces = 3;
            this.numMinValue.Location = new System.Drawing.Point(408, 59);
            this.numMinValue.Maximum = new decimal(new int[] {
            1316134912,
            2328,
            0,
            0});
            this.numMinValue.Name = "numMinValue";
            this.numMinValue.Size = new System.Drawing.Size(120, 23);
            this.numMinValue.TabIndex = 230;
            this.numMinValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 229;
            this.label1.Text = "上    限";
            // 
            // numMaxValue
            // 
            this.numMaxValue.DecimalPlaces = 3;
            this.numMaxValue.Location = new System.Drawing.Point(89, 59);
            this.numMaxValue.Maximum = new decimal(new int[] {
            276447232,
            23283,
            0,
            0});
            this.numMaxValue.Name = "numMaxValue";
            this.numMaxValue.Size = new System.Drawing.Size(120, 23);
            this.numMaxValue.TabIndex = 228;
            this.numMaxValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtSpce
            // 
            this.txtSpce.BackColor = System.Drawing.Color.White;
            this.txtSpce.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSpce.ForeColor = System.Drawing.Color.Black;
            this.txtSpce.Location = new System.Drawing.Point(756, 26);
            this.txtSpce.Name = "txtSpce";
            this.txtSpce.ReadOnly = true;
            this.txtSpce.Size = new System.Drawing.Size(223, 23);
            this.txtSpce.TabIndex = 226;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(339, 30);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 225;
            this.label9.Text = "图号型号";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.BackColor = System.Drawing.Color.White;
            this.txtGoodsCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGoodsCode.ForeColor = System.Drawing.Color.Black;
            this.txtGoodsCode.Location = new System.Drawing.Point(408, 26);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ReadOnly = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(252, 23);
            this.txtGoodsCode.TabIndex = 224;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(687, 30);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 223;
            this.label7.Text = "规    格";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(19, 30);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 222;
            this.label5.Text = "物品名称";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 234);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 502);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 202);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1016, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 4;
            // 
            // tbsGoods
            // 
            this.tbsGoods.FindItem = TextBoxShow.FindType.所有物品;
            this.tbsGoods.Location = new System.Drawing.Point(89, 26);
            this.tbsGoods.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tbsGoods.Name = "tbsGoods";
            this.tbsGoods.Size = new System.Drawing.Size(208, 23);
            this.tbsGoods.TabIndex = 227;
            this.tbsGoods.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsGoods_OnCompleteSearch);
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(615, 61);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(53, 18);
            this.rbAll.TabIndex = 234;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "全部";
            this.rbAll.UseVisualStyleBackColor = true;
            this.rbAll.CheckedChanged += new System.EventHandler(this.rbAll_CheckedChanged);
            // 
            // rbNomarl
            // 
            this.rbNomarl.AutoSize = true;
            this.rbNomarl.Location = new System.Drawing.Point(700, 61);
            this.rbNomarl.Name = "rbNomarl";
            this.rbNomarl.Size = new System.Drawing.Size(53, 18);
            this.rbNomarl.TabIndex = 235;
            this.rbNomarl.Text = "正常";
            this.rbNomarl.UseVisualStyleBackColor = true;
            this.rbNomarl.CheckedChanged += new System.EventHandler(this.rbNomarl_CheckedChanged);
            // 
            // rbMoreMax
            // 
            this.rbMoreMax.AutoSize = true;
            this.rbMoreMax.Location = new System.Drawing.Point(785, 61);
            this.rbMoreMax.Name = "rbMoreMax";
            this.rbMoreMax.Size = new System.Drawing.Size(81, 18);
            this.rbMoreMax.TabIndex = 236;
            this.rbMoreMax.Text = "超出上限";
            this.rbMoreMax.UseVisualStyleBackColor = true;
            this.rbMoreMax.CheckedChanged += new System.EventHandler(this.rbMoreMax_CheckedChanged);
            // 
            // rbLessMin
            // 
            this.rbLessMin.AutoSize = true;
            this.rbLessMin.Location = new System.Drawing.Point(898, 61);
            this.rbLessMin.Name = "rbLessMin";
            this.rbLessMin.Size = new System.Drawing.Size(81, 18);
            this.rbLessMin.TabIndex = 237;
            this.rbLessMin.Text = "低于下限";
            this.rbLessMin.UseVisualStyleBackColor = true;
            this.rbLessMin.CheckedChanged += new System.EventHandler(this.rbLessMin_CheckedChanged);
            // 
            // 物流安全库存
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.toolStrip1);
            this.Name = "物流安全库存";
            this.Text = "物流安全库存";
            this.Load += new System.EventHandler(this.物流安全库存_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numMinValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numMaxValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnModify;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnOutExcel;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numMinValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numMaxValue;
        private TextBoxShow tbsGoods;
        private System.Windows.Forms.TextBox txtSpce;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.RadioButton rbLessMin;
        private System.Windows.Forms.RadioButton rbMoreMax;
        private System.Windows.Forms.RadioButton rbNomarl;
        private System.Windows.Forms.RadioButton rbAll;
    }
}