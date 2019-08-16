namespace Expression
{
    partial class 还货清单
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
            this.btnPropose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnAudit = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbStockCount = new System.Windows.Forms.Label();
            this.lbKCDW = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.txtCode = new UniversalControlLibrary.TextBoxShow();
            this.lbHYDW = new System.Windows.Forms.Label();
            this.numOperationCount = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOperationCount)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPropose,
            this.toolStripSeparatorAdd,
            this.btnAudit});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(882, 25);
            this.toolStrip1.TabIndex = 76;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPropose
            // 
            this.btnPropose.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnPropose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPropose.Name = "btnPropose";
            this.btnPropose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPropose.Size = new System.Drawing.Size(67, 22);
            this.btnPropose.Tag = "";
            this.btnPropose.Text = "保存(&S)";
            this.btnPropose.Click += new System.EventHandler(this.btnPropose_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAudit
            // 
            this.btnAudit.Image = global::UniversalControlLibrary.Properties.Resources.cancle;
            this.btnAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAudit.Name = "btnAudit";
            this.btnAudit.Size = new System.Drawing.Size(67, 22);
            this.btnAudit.Tag = "";
            this.btnAudit.Text = "关闭(&C)";
            this.btnAudit.Click += new System.EventHandler(this.btnAudit_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnModify);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.lbStockCount);
            this.groupBox1.Controls.Add(this.lbKCDW);
            this.groupBox1.Controls.Add(this.label19);
            this.groupBox1.Controls.Add(this.txtBatchNo);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.lbHYDW);
            this.groupBox1.Controls.Add(this.numOperationCount);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(882, 116);
            this.groupBox1.TabIndex = 77;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置记录区";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(366, 83);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(205, 21);
            this.txtRemark.TabIndex = 42;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(295, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 41;
            this.label1.Text = "备    注";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(787, 82);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 40;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(689, 82);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 23);
            this.btnModify.TabIndex = 39;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(591, 82);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 38;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbStockCount
            // 
            this.lbStockCount.AutoSize = true;
            this.lbStockCount.Location = new System.Drawing.Point(656, 55);
            this.lbStockCount.Name = "lbStockCount";
            this.lbStockCount.Size = new System.Drawing.Size(77, 12);
            this.lbStockCount.TabIndex = 37;
            this.lbStockCount.Text = "lbStockCount";
            // 
            // lbKCDW
            // 
            this.lbKCDW.AutoSize = true;
            this.lbKCDW.Location = new System.Drawing.Point(785, 55);
            this.lbKCDW.Name = "lbKCDW";
            this.lbKCDW.Size = new System.Drawing.Size(29, 12);
            this.lbKCDW.TabIndex = 36;
            this.lbKCDW.Text = "单位";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(589, 55);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(53, 12);
            this.label19.TabIndex = 35;
            this.label19.Text = "库存数量";
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间借贷物品批次号;
            this.txtBatchNo.Location = new System.Drawing.Point(72, 51);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(205, 21);
            this.txtBatchNo.TabIndex = 34;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // txtCode
            // 
            this.txtCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间借贷物品;
            this.txtCode.Location = new System.Drawing.Point(72, 20);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(205, 21);
            this.txtCode.TabIndex = 33;
            this.txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtCode_OnCompleteSearch);
            this.txtCode.Enter += new System.EventHandler(this.txtCode_Enter);
            // 
            // lbHYDW
            // 
            this.lbHYDW.AutoSize = true;
            this.lbHYDW.Location = new System.Drawing.Point(542, 55);
            this.lbHYDW.Name = "lbHYDW";
            this.lbHYDW.Size = new System.Drawing.Size(29, 12);
            this.lbHYDW.TabIndex = 32;
            this.lbHYDW.Text = "单位";
            // 
            // numOperationCount
            // 
            this.numOperationCount.DecimalPlaces = 3;
            this.numOperationCount.Location = new System.Drawing.Point(366, 51);
            this.numOperationCount.Maximum = new decimal(new int[] {
            -159383552,
            46653770,
            5421,
            0});
            this.numOperationCount.Name = "numOperationCount";
            this.numOperationCount.Size = new System.Drawing.Size(170, 21);
            this.numOperationCount.TabIndex = 31;
            this.numOperationCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(295, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 30;
            this.label16.Text = "数    量";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(13, 55);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 29;
            this.label15.Text = "批 次 号";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(658, 20);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(205, 21);
            this.txtSpec.TabIndex = 28;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(589, 24);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 27;
            this.label14.Text = "规    格";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(366, 20);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(205, 21);
            this.txtName.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(295, 24);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 25;
            this.label13.Text = "物品名称";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(13, 24);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 24;
            this.label11.Text = "图号型号";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridView1);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 141);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(882, 347);
            this.groupBox2.TabIndex = 79;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "还货清单列表";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 17);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(876, 327);
            this.dataGridView1.TabIndex = 1;
            // 
            // txtProvider
            // 
            this.txtProvider.Location = new System.Drawing.Point(72, 83);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(205, 21);
            this.txtProvider.TabIndex = 44;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 87);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 43;
            this.label2.Text = "供 应 商";
            // 
            // 还货清单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(882, 488);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "还货清单";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "还货清单";
            this.Load += new System.EventHandler(this.还货清单_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numOperationCount)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPropose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lbStockCount;
        private System.Windows.Forms.Label lbKCDW;
        private System.Windows.Forms.Label label19;
        private UniversalControlLibrary.TextBoxShow txtBatchNo;
        private UniversalControlLibrary.TextBoxShow txtCode;
        private System.Windows.Forms.Label lbHYDW;
        private System.Windows.Forms.NumericUpDown numOperationCount;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnAudit;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label2;

    }
}