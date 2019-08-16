using UniversalControlLibrary;
namespace Expression
{
    partial class FormMaterialDetainList
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
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDeleteAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panelTop = new System.Windows.Forms.Panel();
            this.txtBatchNo = new UniversalControlLibrary.TextBoxShow();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtAssociateID = new System.Windows.Forms.TextBox();
            this.txtCode = new UniversalControlLibrary.TextBoxShow();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRecordRow = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtUnit = new System.Windows.Forms.TextBox();
            this.numAmount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.toolStripSeparator1,
            this.btnAdd,
            this.toolStripSeparator2,
            this.btnDelete,
            this.toolStripSeparator7,
            this.btnDeleteAll,
            this.toolStripSeparatorDelete,
            this.btnUpdate});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1095, 25);
            this.toolStrip1.TabIndex = 40;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnNew
            // 
            this.btnNew.Image = global::UniversalControlLibrary.Properties.Resources.File2;
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(67, 22);
            this.btnNew.Text = "新建(&N)";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
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
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "更新";
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 22);
            this.btnDelete.Tag = "Delete";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDeleteAll
            // 
            this.btnDeleteAll.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDeleteAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDeleteAll.Name = "btnDeleteAll";
            this.btnDeleteAll.Size = new System.Drawing.Size(73, 22);
            this.btnDeleteAll.Tag = "Delete";
            this.btnDeleteAll.Text = "删除所有";
            this.btnDeleteAll.ToolTipText = "删除此领料单所有清单信息";
            this.btnDeleteAll.Click += new System.EventHandler(this.btnDeleteAll_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorDelete.Tag = "更新";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.table;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(67, 22);
            this.btnUpdate.Tag = "Update";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1095, 617);
            this.panelMain.TabIndex = 42;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panelTop);
            this.panelCenter.Controls.Add(this.panelBottom);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(8, 46);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1079, 571);
            this.panelCenter.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 160);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1079, 399);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // panelTop
            // 
            this.panelTop.AutoScroll = true;
            this.panelTop.Controls.Add(this.txtBatchNo);
            this.panelTop.Controls.Add(this.btnSearch);
            this.panelTop.Controls.Add(this.label9);
            this.panelTop.Controls.Add(this.txtAssociateID);
            this.panelTop.Controls.Add(this.txtCode);
            this.panelTop.Controls.Add(this.txtRemark);
            this.panelTop.Controls.Add(this.label6);
            this.panelTop.Controls.Add(this.lblAmount);
            this.panelTop.Controls.Add(this.label8);
            this.panelTop.Controls.Add(this.lblRecordRow);
            this.panelTop.Controls.Add(this.label4);
            this.panelTop.Controls.Add(this.txtUnit);
            this.panelTop.Controls.Add(this.numAmount);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Controls.Add(this.txtSpec);
            this.panelTop.Controls.Add(this.label7);
            this.panelTop.Controls.Add(this.txtProvider);
            this.panelTop.Controls.Add(this.label26);
            this.panelTop.Controls.Add(this.label3);
            this.panelTop.Controls.Add(this.label13);
            this.panelTop.Controls.Add(this.txtName);
            this.panelTop.Controls.Add(this.label12);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1079, 160);
            this.panelTop.TabIndex = 31;
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.车间耗用物品批次号;
            this.txtBatchNo.Location = new System.Drawing.Point(367, 49);
            this.txtBatchNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(174, 21);
            this.txtBatchNo.TabIndex = 267;
            this.txtBatchNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBatchNo_OnCompleteSearch);
            this.txtBatchNo.Click += new System.EventHandler(this.txtBatchNo_Click);
            this.txtBatchNo.Enter += new System.EventHandler(this.txtBatchNo_Enter);
            // 
            // btnSearch
            // 
            this.btnSearch.BackColor = System.Drawing.Color.Transparent;
            this.btnSearch.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnSearch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnSearch.Location = new System.Drawing.Point(775, 49);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(21, 21);
            this.btnSearch.TabIndex = 265;
            this.btnSearch.UseVisualStyleBackColor = false;
            this.btnSearch.Visible = false;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaption;
            this.label9.Location = new System.Drawing.Point(547, 53);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 266;
            this.label9.Text = "关联订单号";
            // 
            // txtAssociateID
            // 
            this.txtAssociateID.BackColor = System.Drawing.Color.White;
            this.txtAssociateID.Location = new System.Drawing.Point(618, 49);
            this.txtAssociateID.Name = "txtAssociateID";
            this.txtAssociateID.ReadOnly = true;
            this.txtAssociateID.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtAssociateID.Size = new System.Drawing.Size(153, 21);
            this.txtAssociateID.TabIndex = 264;
            // 
            // txtCode
            // 
            this.txtCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtCode.Location = new System.Drawing.Point(103, 16);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(174, 21);
            this.txtCode.TabIndex = 263;
            this.txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtCode_OnCompleteSearch);
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Location = new System.Drawing.Point(368, 84);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(665, 21);
            this.txtRemark.TabIndex = 255;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(300, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 256;
            this.label6.Text = "备    注";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(110, 88);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(11, 12);
            this.lblAmount.TabIndex = 254;
            this.lblAmount.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(31, 88);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 253;
            this.label8.Text = "总记录数：";
            // 
            // lblRecordRow
            // 
            this.lblRecordRow.AutoSize = true;
            this.lblRecordRow.Location = new System.Drawing.Point(228, 88);
            this.lblRecordRow.Name = "lblRecordRow";
            this.lblRecordRow.Size = new System.Drawing.Size(11, 12);
            this.lblRecordRow.TabIndex = 252;
            this.lblRecordRow.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(142, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 251;
            this.label4.Text = "当前记录行：";
            // 
            // txtUnit
            // 
            this.txtUnit.BackColor = System.Drawing.Color.White;
            this.txtUnit.Location = new System.Drawing.Point(1001, 16);
            this.txtUnit.Name = "txtUnit";
            this.txtUnit.ReadOnly = true;
            this.txtUnit.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtUnit.Size = new System.Drawing.Size(24, 21);
            this.txtUnit.TabIndex = 250;
            // 
            // numAmount
            // 
            this.numAmount.BackColor = System.Drawing.Color.White;
            this.numAmount.DecimalPlaces = 2;
            this.numAmount.Location = new System.Drawing.Point(857, 16);
            this.numAmount.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.numAmount.Name = "numAmount";
            this.numAmount.Size = new System.Drawing.Size(106, 21);
            this.numAmount.TabIndex = 234;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(797, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 249;
            this.label2.Text = "扣 货 数";
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Location = new System.Drawing.Point(618, 16);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(154, 21);
            this.txtSpec.TabIndex = 232;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(547, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 248;
            this.label7.Text = "规      格";
            // 
            // txtProvider
            // 
            this.txtProvider.BackColor = System.Drawing.Color.White;
            this.txtProvider.Location = new System.Drawing.Point(103, 49);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(174, 21);
            this.txtProvider.TabIndex = 233;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(300, 53);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(53, 12);
            this.label26.TabIndex = 242;
            this.label26.Text = "批次批号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 243;
            this.label3.Text = "供 应 商";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(967, 20);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(29, 12);
            this.label13.TabIndex = 241;
            this.label13.Text = "单位";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(368, 16);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtName.Size = new System.Drawing.Size(172, 21);
            this.txtName.TabIndex = 231;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(31, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 240;
            this.label12.Text = "图号型号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(300, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 239;
            this.label1.Text = "物品名称";
            // 
            // panelBottom
            // 
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(0, 559);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(1079, 12);
            this.panelBottom.TabIndex = 27;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1087, 46);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(8, 571);
            this.panelRight.TabIndex = 39;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 46);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(8, 571);
            this.panelLeft.TabIndex = 38;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1095, 46);
            this.panel1.TabIndex = 24;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(460, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "物料扣货清单";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "更新";
            // 
            // FormMaterialDetainList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1095, 642);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.toolStrip1);
            this.Name = "FormMaterialDetainList";
            this.Text = "物料扣货清单";
            this.Load += new System.EventHandler(this.FormMaterialDetainList_Load);
            this.Resize += new System.EventHandler(this.FormMaterialDetainList_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numAmount)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnDeleteAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtAssociateID;
        private TextBoxShow txtCode;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRecordRow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtUnit;
        private System.Windows.Forms.NumericUpDown numAmount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private TextBoxShow txtBatchNo;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}