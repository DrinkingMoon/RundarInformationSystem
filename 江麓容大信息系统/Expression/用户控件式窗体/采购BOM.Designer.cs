using UniversalControlLibrary;
namespace Expression
{
    partial class 采购BOM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(采购BOM));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnModify = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCreateInfo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnInExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOutExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOutExcelInfo = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbUnit = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.numSafeStockCount = new System.Windows.Forms.NumericUpDown();
            this.btnSetUsage = new System.Windows.Forms.Button();
            this.tbsGoods = new UniversalControlLibrary.TextBoxShow();
            this.txtSpce = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSafeStockCount)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.toolStripSeparator2,
            this.btnAdd,
            this.toolStripSeparator4,
            this.btnModify,
            this.toolStripSeparator1,
            this.btnDelete,
            this.toolStripSeparator5,
            this.btnCreateInfo,
            this.toolStripSeparator6,
            this.btnRefresh,
            this.toolStripSeparator8,
            this.btnInExcel,
            this.toolStripSeparator7,
            this.btnOutExcel,
            this.toolStripSeparator3,
            this.btnOutExcelInfo});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(70, 22);
            this.btnNew.Tag = "Add";
            this.btnNew.Text = "新建(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnModify
            // 
            this.btnModify.Image = ((System.Drawing.Image)(resources.GetObject("btnModify.Image")));
            this.btnModify.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(72, 22);
            this.btnModify.Tag = "Add";
            this.btnModify.Text = "修改(&M)";
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 22);
            this.btnDelete.Tag = "Add";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnCreateInfo
            // 
            this.btnCreateInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateInfo.Image")));
            this.btnCreateInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateInfo.Name = "btnCreateInfo";
            this.btnCreateInfo.Size = new System.Drawing.Size(200, 22);
            this.btnCreateInfo.Tag = "Add";
            this.btnCreateInfo.Text = "由总成数重新初始化安全库存(C)";
            this.btnCreateInfo.Click += new System.EventHandler(this.btnCreateInfo_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(68, 22);
            this.btnRefresh.Tag = "View";
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnInExcel
            // 
            this.btnInExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnInExcel.Image")));
            this.btnInExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInExcel.Name = "btnInExcel";
            this.btnInExcel.Size = new System.Drawing.Size(64, 22);
            this.btnInExcel.Tag = "Add";
            this.btnInExcel.Text = "导入(&I)";
            this.btnInExcel.Click += new System.EventHandler(this.btnInExcel_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Image = ((System.Drawing.Image)(resources.GetObject("btnOutExcel.Image")));
            this.btnOutExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(67, 22);
            this.btnOutExcel.Tag = "View";
            this.btnOutExcel.Text = "导出(&E)";
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOutExcelInfo
            // 
            this.btnOutExcelInfo.Image = ((System.Drawing.Image)(resources.GetObject("btnOutExcelInfo.Image")));
            this.btnOutExcelInfo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutExcelInfo.Name = "btnOutExcelInfo";
            this.btnOutExcelInfo.Size = new System.Drawing.Size(142, 22);
            this.btnOutExcelInfo.Tag = "Add";
            this.btnOutExcelInfo.Text = "导出零件综合信息(&O)";
            this.btnOutExcelInfo.Click += new System.EventHandler(this.btnOutExcelInfo_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbUnit);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numSafeStockCount);
            this.groupBox1.Controls.Add(this.btnSetUsage);
            this.groupBox1.Controls.Add(this.tbsGoods);
            this.groupBox1.Controls.Add(this.txtSpce);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtGoodsCode);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1016, 109);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "物料信息";
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUnit.Location = new System.Drawing.Point(276, 71);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(35, 14);
            this.lbUnit.TabIndex = 241;
            this.lbUnit.Text = "单位";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(15, 71);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 240;
            this.label2.Text = "安全库存";
            // 
            // numSafeStockCount
            // 
            this.numSafeStockCount.DecimalPlaces = 2;
            this.numSafeStockCount.Location = new System.Drawing.Point(87, 67);
            this.numSafeStockCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numSafeStockCount.Name = "numSafeStockCount";
            this.numSafeStockCount.Size = new System.Drawing.Size(181, 23);
            this.numSafeStockCount.TabIndex = 239;
            // 
            // btnSetUsage
            // 
            this.btnSetUsage.Location = new System.Drawing.Point(770, 65);
            this.btnSetUsage.Name = "btnSetUsage";
            this.btnSetUsage.Size = new System.Drawing.Size(225, 27);
            this.btnSetUsage.TabIndex = 228;
            this.btnSetUsage.Text = "设置采购基数";
            this.btnSetUsage.UseVisualStyleBackColor = true;
            this.btnSetUsage.Click += new System.EventHandler(this.btnSetUsage_Click);
            // 
            // tbsGoods
            // 
            this.tbsGoods.DataResult = null;
            this.tbsGoods.DataTableResult = null;
            this.tbsGoods.EditingControlDataGridView = null;
            this.tbsGoods.EditingControlFormattedValue = "";
            this.tbsGoods.EditingControlRowIndex = 0;
            this.tbsGoods.EditingControlValueChanged = false;
            this.tbsGoods.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.tbsGoods.IsMultiSelect = false;
            this.tbsGoods.Location = new System.Drawing.Point(87, 27);
            this.tbsGoods.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.tbsGoods.Name = "tbsGoods";
            this.tbsGoods.ShowResultForm = true;
            this.tbsGoods.Size = new System.Drawing.Size(224, 23);
            this.tbsGoods.StrEndSql = null;
            this.tbsGoods.TabIndex = 227;
            this.tbsGoods.TabStop = false;
            this.tbsGoods.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsGoods_OnCompleteSearch);
            // 
            // txtSpce
            // 
            this.txtSpce.BackColor = System.Drawing.Color.White;
            this.txtSpce.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSpce.ForeColor = System.Drawing.Color.Black;
            this.txtSpce.Location = new System.Drawing.Point(770, 27);
            this.txtSpce.Name = "txtSpce";
            this.txtSpce.ReadOnly = true;
            this.txtSpce.Size = new System.Drawing.Size(225, 23);
            this.txtSpce.TabIndex = 226;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(338, 31);
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
            this.txtGoodsCode.Location = new System.Drawing.Point(407, 27);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ReadOnly = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(241, 23);
            this.txtGoodsCode.TabIndex = 224;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(677, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 223;
            this.label7.Text = "规    格";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(15, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 222;
            this.label5.Text = "物品名称";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 25);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1016, 49);
            this.panel2.TabIndex = 242;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(454, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(108, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "采购BOM";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 220);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 639);
            this.dataGridView1.TabIndex = 243;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 183);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1016, 37);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 2;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // 采购BOM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 859);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "采购BOM";
            this.Text = "采购BOM";
            this.Load += new System.EventHandler(this.CBOM客户物料清单_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSafeStockCount)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnModify;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private TextBoxShow tbsGoods;
        private System.Windows.Forms.TextBox txtSpce;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnCreateInfo;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Button btnSetUsage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numSafeStockCount;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnOutExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnOutExcelInfo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnInExcel;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}