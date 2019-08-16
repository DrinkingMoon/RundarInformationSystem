using UniversalControlLibrary;
namespace Expression
{
    partial class 安全库存
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(安全库存));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btnShowDownSafeCount = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOutExcel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnOutDownSafeCount = new System.Windows.Forms.ToolStripButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelPara = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.numSafeStockCount = new System.Windows.Forms.NumericUpDown();
            this.lbUnit = new System.Windows.Forms.Label();
            this.txtGoodsType = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tbsGoods = new UniversalControlLibrary.TextBoxShow();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpce = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panelTop = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelPara.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSafeStockCount)).BeginInit();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRefresh,
            this.toolStripSeparator9,
            this.btnShowDownSafeCount,
            this.toolStripSeparator7,
            this.btnOutExcel,
            this.toolStripSeparator8,
            this.btnOutDownSafeCount});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1152, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
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
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // btnShowDownSafeCount
            // 
            this.btnShowDownSafeCount.Image = ((System.Drawing.Image)(resources.GetObject("btnShowDownSafeCount.Image")));
            this.btnShowDownSafeCount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowDownSafeCount.Name = "btnShowDownSafeCount";
            this.btnShowDownSafeCount.Size = new System.Drawing.Size(176, 22);
            this.btnShowDownSafeCount.Tag = "View";
            this.btnShowDownSafeCount.Text = "仅显示低于安全库存记录(&R)";
            this.btnShowDownSafeCount.Click += new System.EventHandler(this.btnShowDownSafeCount_Click);
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
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnOutDownSafeCount
            // 
            this.btnOutDownSafeCount.Image = ((System.Drawing.Image)(resources.GetObject("btnOutDownSafeCount.Image")));
            this.btnOutDownSafeCount.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutDownSafeCount.Name = "btnOutDownSafeCount";
            this.btnOutDownSafeCount.Size = new System.Drawing.Size(151, 22);
            this.btnOutDownSafeCount.Tag = "Add";
            this.btnOutDownSafeCount.Text = "导出低于安全库存项(&E)";
            this.btnOutDownSafeCount.Click += new System.EventHandler(this.btnOutDownSafeCount_Click);
            // 
            // panelMain
            // 
            this.panelMain.Controls.Add(this.panelContent);
            this.panelMain.Controls.Add(this.panelTop);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1152, 576);
            this.panelMain.TabIndex = 1;
            // 
            // panelContent
            // 
            this.panelContent.Controls.Add(this.dataGridView1);
            this.panelContent.Controls.Add(this.panelPara);
            this.panelContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContent.Location = new System.Drawing.Point(0, 44);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(1152, 532);
            this.panelContent.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 158);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 21;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1152, 374);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.label2);
            this.panelPara.Controls.Add(this.numSafeStockCount);
            this.panelPara.Controls.Add(this.lbUnit);
            this.panelPara.Controls.Add(this.txtGoodsType);
            this.panelPara.Controls.Add(this.label14);
            this.panelPara.Controls.Add(this.tbsGoods);
            this.panelPara.Controls.Add(this.txtRemark);
            this.panelPara.Controls.Add(this.label1);
            this.panelPara.Controls.Add(this.txtSpce);
            this.panelPara.Controls.Add(this.label9);
            this.panelPara.Controls.Add(this.txtGoodsCode);
            this.panelPara.Controls.Add(this.label7);
            this.panelPara.Controls.Add(this.label5);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 0);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1152, 158);
            this.panelPara.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(368, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 238;
            this.label2.Text = "安全库存";
            // 
            // numSafeStockCount
            // 
            this.numSafeStockCount.DecimalPlaces = 2;
            this.numSafeStockCount.Location = new System.Drawing.Point(449, 51);
            this.numSafeStockCount.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numSafeStockCount.Name = "numSafeStockCount";
            this.numSafeStockCount.Size = new System.Drawing.Size(252, 23);
            this.numSafeStockCount.TabIndex = 237;
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUnit.Location = new System.Drawing.Point(707, 55);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(35, 14);
            this.lbUnit.TabIndex = 235;
            this.lbUnit.Text = "单位";
            // 
            // txtGoodsType
            // 
            this.txtGoodsType.BackColor = System.Drawing.Color.White;
            this.txtGoodsType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGoodsType.ForeColor = System.Drawing.Color.Black;
            this.txtGoodsType.Location = new System.Drawing.Point(111, 51);
            this.txtGoodsType.Name = "txtGoodsType";
            this.txtGoodsType.ReadOnly = true;
            this.txtGoodsType.Size = new System.Drawing.Size(231, 23);
            this.txtGoodsType.TabIndex = 234;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(25, 55);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 14);
            this.label14.TabIndex = 233;
            this.label14.Text = "材料类别";
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
            this.tbsGoods.Location = new System.Drawing.Point(111, 20);
            this.tbsGoods.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.tbsGoods.Name = "tbsGoods";
            this.tbsGoods.ShowResultForm = true;
            this.tbsGoods.Size = new System.Drawing.Size(231, 23);
            this.tbsGoods.StrEndSql = null;
            this.tbsGoods.TabIndex = 221;
            this.tbsGoods.TabStop = false;
            this.tbsGoods.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsGoods_OnCompleteSearch);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemark.ForeColor = System.Drawing.Color.Black;
            this.txtRemark.Location = new System.Drawing.Point(111, 85);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(978, 23);
            this.txtRemark.TabIndex = 220;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(25, 88);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 219;
            this.label1.Text = "备    注";
            // 
            // txtSpce
            // 
            this.txtSpce.BackColor = System.Drawing.Color.White;
            this.txtSpce.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSpce.ForeColor = System.Drawing.Color.Black;
            this.txtSpce.Location = new System.Drawing.Point(823, 20);
            this.txtSpce.Name = "txtSpce";
            this.txtSpce.ReadOnly = true;
            this.txtSpce.Size = new System.Drawing.Size(266, 23);
            this.txtSpce.TabIndex = 213;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(368, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 212;
            this.label9.Text = "图号型号";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.BackColor = System.Drawing.Color.White;
            this.txtGoodsCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGoodsCode.ForeColor = System.Drawing.Color.Black;
            this.txtGoodsCode.Location = new System.Drawing.Point(449, 20);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ReadOnly = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(252, 23);
            this.txtGoodsCode.TabIndex = 211;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(744, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 210;
            this.label7.Text = "规    格";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(26, 24);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 209;
            this.label5.Text = "物品名称";
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1152, 44);
            this.panelTop.TabIndex = 0;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelTitle.Location = new System.Drawing.Point(510, 7);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "安全库存";
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
            // 安全库存
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1152, 601);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "安全库存";
            this.Text = "安全库存";
            this.Load += new System.EventHandler(this.安全库存_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelPara.ResumeLayout(false);
            this.panelPara.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numSafeStockCount)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtSpce;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label1;
        private TextBoxShow tbsGoods;
        private System.Windows.Forms.TextBox txtGoodsType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnOutExcel;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnOutDownSafeCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton btnShowDownSafeCount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown numSafeStockCount;

    }
}
