namespace Expression
{
    partial class UserControlBarCodeManage
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
            this.labelTitle = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSearch = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnManageProductBarcode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrintBarCode = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrintCVTBarcodes = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrintBoardCode = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelCenter.SuspendLayout();
            this.panel1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(420, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(147, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "条形码管理";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(986, 513);
            this.dataGridView1.TabIndex = 31;
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 103);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(986, 524);
            this.panelCenter.TabIndex = 56;
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 513);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(986, 11);
            this.panel2.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(986, 50);
            this.panel1.TabIndex = 57;
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.Control;
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 75);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(986, 28);
            this.panelTop.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSearch,
            this.toolStripSeparator2,
            this.btnManageProductBarcode,
            this.toolStripSeparator3,
            this.btnPrintBarCode,
            this.toolStripSeparator1,
            this.btnPrintCVTBarcodes,
            this.toolStripSeparator4,
            this.btnPrintBoardCode});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(986, 25);
            this.toolStrip1.TabIndex = 38;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSearch
            // 
            this.btnSearch.Image = global::UniversalControlLibrary.Properties.Resources.Search;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(85, 22);
            this.btnSearch.Text = "条形码查询";
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnManageProductBarcode
            // 
            this.btnManageProductBarcode.Image = global::UniversalControlLibrary.Properties.Resources.Report;
            this.btnManageProductBarcode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnManageProductBarcode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManageProductBarcode.Name = "btnManageProductBarcode";
            this.btnManageProductBarcode.Size = new System.Drawing.Size(157, 22);
            this.btnManageProductBarcode.Tag = "view";
            this.btnManageProductBarcode.Text = "产品总成条形码打印管理";
            this.btnManageProductBarcode.Click += new System.EventHandler(this.btnManageProductBarcode_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPrintBarCode
            // 
            this.btnPrintBarCode.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.btnPrintBarCode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintBarCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrintBarCode.Name = "btnPrintBarCode";
            this.btnPrintBarCode.Size = new System.Drawing.Size(145, 22);
            this.btnPrintBarCode.Tag = "Print";
            this.btnPrintBarCode.Text = "打印选择的物品条形码";
            this.btnPrintBarCode.Click += new System.EventHandler(this.btnPrintBarCode_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPrintCVTBarcodes
            // 
            this.btnPrintCVTBarcodes.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.btnPrintCVTBarcodes.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintCVTBarcodes.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrintCVTBarcodes.Name = "btnPrintCVTBarcodes";
            this.btnPrintCVTBarcodes.Size = new System.Drawing.Size(193, 22);
            this.btnPrintCVTBarcodes.Tag = "审核";
            this.btnPrintCVTBarcodes.Text = "打印总成回收零件装配用条形码";
            this.btnPrintCVTBarcodes.ToolTipText = "同整台份领料，不包含零星领料零件";
            this.btnPrintCVTBarcodes.Click += new System.EventHandler(this.btnPrintCVTBarcodes_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPrintBoardCode
            // 
            this.btnPrintBoardCode.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.btnPrintBoardCode.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnPrintBoardCode.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrintBoardCode.Name = "btnPrintBoardCode";
            this.btnPrintBoardCode.Size = new System.Drawing.Size(109, 22);
            this.btnPrintBoardCode.Tag = "view";
            this.btnPrintBoardCode.Text = "打印看板条形码";
            this.btnPrintBoardCode.Click += new System.EventHandler(this.btnPrintBoardCode_Click);
            // 
            // UserControlBarCodeManage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(986, 627);
            this.Controls.Add(this.panelCenter);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlBarCodeManage";
            this.Load += new System.EventHandler(this.UserControlBarCodeManage_Load);
            this.Resize += new System.EventHandler(this.UserControlBarCodeManage_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelCenter.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPrintBarCode;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnPrintCVTBarcodes;
        private System.Windows.Forms.ToolStripButton btnSearch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnManageProductBarcode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnPrintBoardCode;
    }
}
