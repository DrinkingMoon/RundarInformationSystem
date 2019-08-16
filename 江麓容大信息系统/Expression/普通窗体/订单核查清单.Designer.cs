namespace Expression
{
    partial class 订单核查清单
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
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripLabel();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnDateDelete = new System.Windows.Forms.Button();
            this.btnDateAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.dateTimeArrivalDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtStockQuotaCount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtStockQuota = new System.Windows.Forms.TextBox();
            this.txtOrderFormNumber = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.btnFindOrderForm = new System.Windows.Forms.Button();
            this.lbDW = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numAmount = new System.Windows.Forms.NumericUpDown();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFindCode = new System.Windows.Forms.Button();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.txtBargainNumber = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dgvArriveList = new System.Windows.Forms.DataGridView();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.dgvOrderFormList = new System.Windows.Forms.DataGridView();
            this.toolStrip1.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvArriveList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderFormList)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUpdate,
            this.toolStripSeparatorDelete,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1047, 25);
            this.toolStrip1.TabIndex = 41;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(67, 22);
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(63, 22);
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox2);
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 25);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1047, 199);
            this.panelPara.TabIndex = 42;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnDateDelete);
            this.groupBox2.Controls.Add(this.btnDateAdd);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.numCount);
            this.groupBox2.Controls.Add(this.dateTimeArrivalDate);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 108);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1047, 54);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "到货日期填写区";
            // 
            // btnDateDelete
            // 
            this.btnDateDelete.Location = new System.Drawing.Point(767, 17);
            this.btnDateDelete.Name = "btnDateDelete";
            this.btnDateDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDateDelete.TabIndex = 238;
            this.btnDateDelete.Text = "删除";
            this.btnDateDelete.UseVisualStyleBackColor = true;
            this.btnDateDelete.Click += new System.EventHandler(this.btnDateDelete_Click);
            // 
            // btnDateAdd
            // 
            this.btnDateAdd.Location = new System.Drawing.Point(640, 17);
            this.btnDateAdd.Name = "btnDateAdd";
            this.btnDateAdd.Size = new System.Drawing.Size(75, 23);
            this.btnDateAdd.TabIndex = 237;
            this.btnDateAdd.Text = "添加";
            this.btnDateAdd.UseVisualStyleBackColor = true;
            this.btnDateAdd.Click += new System.EventHandler(this.btnDateAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(551, 24);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 236;
            this.label8.Text = "单位";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(330, 24);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 235;
            this.label9.Text = "数    量";
            // 
            // numCount
            // 
            this.numCount.BackColor = System.Drawing.Color.White;
            this.numCount.Location = new System.Drawing.Point(399, 20);
            this.numCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(146, 21);
            this.numCount.TabIndex = 234;
            // 
            // dateTimeArrivalDate
            // 
            this.dateTimeArrivalDate.Location = new System.Drawing.Point(92, 20);
            this.dateTimeArrivalDate.Name = "dateTimeArrivalDate";
            this.dateTimeArrivalDate.Size = new System.Drawing.Size(181, 21);
            this.dateTimeArrivalDate.TabIndex = 231;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(23, 24);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 230;
            this.label6.Text = "到货日期";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.txtStockQuotaCount);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtStockQuota);
            this.groupBox1.Controls.Add(this.txtOrderFormNumber);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.btnFindOrderForm);
            this.groupBox1.Controls.Add(this.lbDW);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.numAmount);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnFindCode);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.txtBargainNumber);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1047, 108);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息填写区";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(848, 88);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 244;
            this.label13.Text = "单位";
            // 
            // txtStockQuotaCount
            // 
            this.txtStockQuotaCount.BackColor = System.Drawing.Color.White;
            this.txtStockQuotaCount.Location = new System.Drawing.Point(707, 81);
            this.txtStockQuotaCount.MaxLength = 50;
            this.txtStockQuotaCount.Name = "txtStockQuotaCount";
            this.txtStockQuotaCount.ReadOnly = true;
            this.txtStockQuotaCount.Size = new System.Drawing.Size(135, 21);
            this.txtStockQuotaCount.TabIndex = 242;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(614, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 243;
            this.label2.Text = "预计采购总数";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(551, 85);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 12);
            this.label11.TabIndex = 241;
            this.label11.Text = "%";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(329, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 240;
            this.label10.Text = "采购份额";
            // 
            // txtStockQuota
            // 
            this.txtStockQuota.BackColor = System.Drawing.Color.White;
            this.txtStockQuota.Location = new System.Drawing.Point(398, 81);
            this.txtStockQuota.MaxLength = 50;
            this.txtStockQuota.Name = "txtStockQuota";
            this.txtStockQuota.ReadOnly = true;
            this.txtStockQuota.Size = new System.Drawing.Size(147, 21);
            this.txtStockQuota.TabIndex = 239;
            // 
            // txtOrderFormNumber
            // 
            this.txtOrderFormNumber.BackColor = System.Drawing.Color.White;
            this.txtOrderFormNumber.Location = new System.Drawing.Point(399, 19);
            this.txtOrderFormNumber.MaxLength = 20;
            this.txtOrderFormNumber.Name = "txtOrderFormNumber";
            this.txtOrderFormNumber.ReadOnly = true;
            this.txtOrderFormNumber.Size = new System.Drawing.Size(190, 21);
            this.txtOrderFormNumber.TabIndex = 235;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(329, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 236;
            this.label3.Text = "订 单 号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(626, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 238;
            this.label5.Text = "供货单位";
            // 
            // txtProvider
            // 
            this.txtProvider.BackColor = System.Drawing.Color.White;
            this.txtProvider.Location = new System.Drawing.Point(707, 19);
            this.txtProvider.MaxLength = 50;
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(135, 21);
            this.txtProvider.TabIndex = 237;
            // 
            // btnFindOrderForm
            // 
            this.btnFindOrderForm.BackColor = System.Drawing.Color.Transparent;
            this.btnFindOrderForm.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindOrderForm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindOrderForm.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindOrderForm.Location = new System.Drawing.Point(290, 19);
            this.btnFindOrderForm.Name = "btnFindOrderForm";
            this.btnFindOrderForm.Size = new System.Drawing.Size(21, 21);
            this.btnFindOrderForm.TabIndex = 234;
            this.btnFindOrderForm.UseVisualStyleBackColor = false;
            this.btnFindOrderForm.Click += new System.EventHandler(this.btnFindOrderForm_Click);
            // 
            // lbDW
            // 
            this.lbDW.AutoSize = true;
            this.lbDW.Location = new System.Drawing.Point(244, 85);
            this.lbDW.Name = "lbDW";
            this.lbDW.Size = new System.Drawing.Size(29, 12);
            this.lbDW.TabIndex = 233;
            this.lbDW.Text = "单位";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(9, 83);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 232;
            this.label4.Text = "实际订货数量";
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.White;
            this.numAmount.Location = new System.Drawing.Point(92, 81);
            this.numAmount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new System.Drawing.Size(146, 21);
            this.numAmount.TabIndex = 231;
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Location = new System.Drawing.Point(707, 50);
            this.txtSpec.MaxLength = 50;
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(135, 21);
            this.txtSpec.TabIndex = 229;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(626, 54);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 230;
            this.label7.Text = "规    格";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(398, 50);
            this.txtName.MaxLength = 50;
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(191, 21);
            this.txtName.TabIndex = 227;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(329, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 228;
            this.label1.Text = "物品名称";
            // 
            // btnFindCode
            // 
            this.btnFindCode.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode.Location = new System.Drawing.Point(290, 50);
            this.btnFindCode.Name = "btnFindCode";
            this.btnFindCode.Size = new System.Drawing.Size(21, 21);
            this.btnFindCode.TabIndex = 226;
            this.btnFindCode.UseVisualStyleBackColor = false;
            this.btnFindCode.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(18, 54);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(59, 12);
            this.label12.TabIndex = 225;
            this.label12.Text = "图号/型号";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(92, 50);
            this.txtCode.MaxLength = 50;
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(192, 21);
            this.txtCode.TabIndex = 224;
            // 
            // txtBargainNumber
            // 
            this.txtBargainNumber.BackColor = System.Drawing.Color.White;
            this.txtBargainNumber.Location = new System.Drawing.Point(92, 19);
            this.txtBargainNumber.MaxLength = 20;
            this.txtBargainNumber.Name = "txtBargainNumber";
            this.txtBargainNumber.ReadOnly = true;
            this.txtBargainNumber.Size = new System.Drawing.Size(192, 21);
            this.txtBargainNumber.TabIndex = 223;
            this.txtBargainNumber.TextChanged += new System.EventHandler(this.txtBargainNumber_TextChanged);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ForeColor = System.Drawing.Color.Black;
            this.label16.Location = new System.Drawing.Point(21, 23);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 222;
            this.label16.Text = "合 同 号";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dgvArriveList);
            this.panel1.Controls.Add(this.splitter1);
            this.panel1.Controls.Add(this.dgvOrderFormList);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 224);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1047, 512);
            this.panel1.TabIndex = 43;
            // 
            // dgvArriveList
            // 
            this.dgvArriveList.AllowUserToAddRows = false;
            this.dgvArriveList.AllowUserToDeleteRows = false;
            this.dgvArriveList.AllowUserToResizeRows = false;
            this.dgvArriveList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvArriveList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvArriveList.Location = new System.Drawing.Point(918, 0);
            this.dgvArriveList.Name = "dgvArriveList";
            this.dgvArriveList.ReadOnly = true;
            this.dgvArriveList.RowHeadersWidth = 20;
            this.dgvArriveList.RowTemplate.Height = 23;
            this.dgvArriveList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvArriveList.Size = new System.Drawing.Size(129, 512);
            this.dgvArriveList.TabIndex = 4;
            this.dgvArriveList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvArriveList_CellEnter);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(915, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 512);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // dgvOrderFormList
            // 
            this.dgvOrderFormList.AllowUserToAddRows = false;
            this.dgvOrderFormList.AllowUserToDeleteRows = false;
            this.dgvOrderFormList.AllowUserToResizeRows = false;
            this.dgvOrderFormList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderFormList.Dock = System.Windows.Forms.DockStyle.Left;
            this.dgvOrderFormList.Location = new System.Drawing.Point(0, 0);
            this.dgvOrderFormList.Name = "dgvOrderFormList";
            this.dgvOrderFormList.ReadOnly = true;
            this.dgvOrderFormList.RowHeadersWidth = 20;
            this.dgvOrderFormList.RowTemplate.Height = 23;
            this.dgvOrderFormList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvOrderFormList.Size = new System.Drawing.Size(915, 512);
            this.dgvOrderFormList.TabIndex = 1;
            this.dgvOrderFormList.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrderFormList_CellClick);
            this.dgvOrderFormList.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvOrderFormList_CellEnter);
            this.dgvOrderFormList.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvOrderFormList_DataBindingComplete);
            // 
            // 订单核查清单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1047, 736);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.toolStrip1);
            this.Name = "订单核查清单";
            this.Text = "订单核查清单";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).EndInit();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvArriveList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderFormList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel btnClose;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnFindOrderForm;
        private System.Windows.Forms.Label lbDW;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFindCode;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.TextBox txtBargainNumber;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtOrderFormNumber;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dgvArriveList;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.DataGridView dgvOrderFormList;
        private System.Windows.Forms.Button btnDateDelete;
        private System.Windows.Forms.Button btnDateAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.DateTimePicker dateTimeArrivalDate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtStockQuota;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtStockQuotaCount;
        private System.Windows.Forms.Label label2;
    }
}