namespace Form_Manufacture_WorkShop
{
    partial class 车间异常明细信息处理
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDispose = new System.Windows.Forms.ToolStripButton();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Modify = new System.Windows.Forms.Button();
            this.btn_Add = new System.Windows.Forms.Button();
            this.lbStockCount = new System.Windows.Forms.Label();
            this.lbKCDW = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.lbHYDW = new System.Windows.Forms.Label();
            this.numOperationCount = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtCode = new UniversalControlLibrary.TextBoxShow();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.toolStrip2.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOperationCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1176, 61);
            this.panel1.TabIndex = 83;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(447, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(282, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "车间异常明细信息处理";
            // 
            // toolStrip2
            // 
            this.toolStrip2.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator2,
            this.btnDispose});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(1176, 25);
            this.toolStrip2.TabIndex = 84;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDispose
            // 
            this.btnDispose.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnDispose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDispose.Name = "btnDispose";
            this.btnDispose.Size = new System.Drawing.Size(115, 22);
            this.btnDispose.Tag = "view";
            this.btnDispose.Text = "处理异常信息(&D)";
            this.btnDispose.Click += new System.EventHandler(this.btnDispose_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 86);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.dataGridView1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1176, 650);
            this.splitContainer1.SplitterDistance = 598;
            this.splitContainer1.TabIndex = 86;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(598, 650);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btn_Delete);
            this.splitContainer2.Panel1.Controls.Add(this.btn_Modify);
            this.splitContainer2.Panel1.Controls.Add(this.btn_Add);
            this.splitContainer2.Panel1.Controls.Add(this.lbStockCount);
            this.splitContainer2.Panel1.Controls.Add(this.lbKCDW);
            this.splitContainer2.Panel1.Controls.Add(this.label19);
            this.splitContainer2.Panel1.Controls.Add(this.lbHYDW);
            this.splitContainer2.Panel1.Controls.Add(this.numOperationCount);
            this.splitContainer2.Panel1.Controls.Add(this.label16);
            this.splitContainer2.Panel1.Controls.Add(this.txtBatchNo);
            this.splitContainer2.Panel1.Controls.Add(this.label15);
            this.splitContainer2.Panel1.Controls.Add(this.txtSpec);
            this.splitContainer2.Panel1.Controls.Add(this.label14);
            this.splitContainer2.Panel1.Controls.Add(this.txtCode);
            this.splitContainer2.Panel1.Controls.Add(this.txtName);
            this.splitContainer2.Panel1.Controls.Add(this.label13);
            this.splitContainer2.Panel1.Controls.Add(this.label11);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.dataGridView2);
            this.splitContainer2.Size = new System.Drawing.Size(574, 650);
            this.splitContainer2.SplitterDistance = 162;
            this.splitContainer2.TabIndex = 0;
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(485, 127);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(75, 23);
            this.btn_Delete.TabIndex = 36;
            this.btn_Delete.Text = "删除";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Modify
            // 
            this.btn_Modify.Location = new System.Drawing.Point(392, 127);
            this.btn_Modify.Name = "btn_Modify";
            this.btn_Modify.Size = new System.Drawing.Size(75, 23);
            this.btn_Modify.TabIndex = 35;
            this.btn_Modify.Text = "修改";
            this.btn_Modify.UseVisualStyleBackColor = true;
            this.btn_Modify.Click += new System.EventHandler(this.btn_Modify_Click);
            // 
            // btn_Add
            // 
            this.btn_Add.Location = new System.Drawing.Point(299, 127);
            this.btn_Add.Name = "btn_Add";
            this.btn_Add.Size = new System.Drawing.Size(75, 23);
            this.btn_Add.TabIndex = 34;
            this.btn_Add.Text = "添加";
            this.btn_Add.UseVisualStyleBackColor = true;
            this.btn_Add.Click += new System.EventHandler(this.btn_Add_Click);
            // 
            // lbStockCount
            // 
            this.lbStockCount.AutoSize = true;
            this.lbStockCount.Location = new System.Drawing.Point(390, 85);
            this.lbStockCount.Name = "lbStockCount";
            this.lbStockCount.Size = new System.Drawing.Size(77, 12);
            this.lbStockCount.TabIndex = 33;
            this.lbStockCount.Text = "lbStockCount";
            // 
            // lbKCDW
            // 
            this.lbKCDW.AutoSize = true;
            this.lbKCDW.Location = new System.Drawing.Point(508, 85);
            this.lbKCDW.Name = "lbKCDW";
            this.lbKCDW.Size = new System.Drawing.Size(29, 12);
            this.lbKCDW.TabIndex = 32;
            this.lbKCDW.Text = "单位";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(297, 85);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 31;
            this.label19.Text = "库存数量";
            // 
            // lbHYDW
            // 
            this.lbHYDW.AutoSize = true;
            this.lbHYDW.Location = new System.Drawing.Point(249, 85);
            this.lbHYDW.Name = "lbHYDW";
            this.lbHYDW.Size = new System.Drawing.Size(29, 12);
            this.lbHYDW.TabIndex = 30;
            this.lbHYDW.Text = "单位";
            // 
            // numOperationCount
            // 
            this.numOperationCount.Location = new System.Drawing.Point(73, 81);
            this.numOperationCount.Maximum = new decimal(new int[] {
            -159383552,
            46653770,
            5421,
            0});
            this.numOperationCount.Name = "numOperationCount";
            this.numOperationCount.Size = new System.Drawing.Size(170, 21);
            this.numOperationCount.TabIndex = 29;
            this.numOperationCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(13, 85);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 28;
            this.label16.Text = "操作数量";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间耗用物品批次号;
            this.txtBatchNo.Location = new System.Drawing.Point(73, 47);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(205, 21);
            this.txtBatchNo.TabIndex = 27;
            this.txtBatchNo.TextChanged += new System.EventHandler(this.txtBatchNo_TextChanged);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 51);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 26;
            this.label15.Text = "批 次 号";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(356, 47);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(205, 21);
            this.txtSpec.TabIndex = 25;
            this.txtSpec.TextChanged += new System.EventHandler(this.txtSpec_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(297, 51);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 24;
            this.label14.Text = "规    格";
            // 
            // txtCode
            // 
            this.txtCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间耗用物品;
            this.txtCode.Location = new System.Drawing.Point(73, 13);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(205, 21);
            this.txtCode.TabIndex = 23;
            this.txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtCode_OnCompleteSearch);
            this.txtCode.TextChanged += new System.EventHandler(this.txtCode_TextChanged);
            this.txtCode.Enter += new System.EventHandler(this.txtCode_Enter);
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(356, 13);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(205, 21);
            this.txtName.TabIndex = 22;
            this.txtName.TextChanged += new System.EventHandler(this.txtName_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(297, 17);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 21;
            this.label13.Text = "物品名称";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 17);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 20;
            this.label11.Text = "图号型号";
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(574, 484);
            this.dataGridView2.TabIndex = 2;
            this.dataGridView2.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView2_CellEnter);
            // 
            // 车间异常明细信息处理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1176, 736);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip2);
            this.Name = "车间异常明细信息处理";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "车间异常明细信息处理";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.numOperationCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnDispose;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Modify;
        private System.Windows.Forms.Button btn_Add;
        private System.Windows.Forms.Label lbStockCount;
        private System.Windows.Forms.Label lbKCDW;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label lbHYDW;
        private System.Windows.Forms.NumericUpDown numOperationCount;
        private System.Windows.Forms.Label label16;
        private UniversalControlLibrary.TextBoxShow txtBatchNo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label14;
        private UniversalControlLibrary.TextBoxShow txtCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DataGridView dataGridView2;
    }
}