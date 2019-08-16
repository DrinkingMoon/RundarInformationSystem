namespace Expression
{
    partial class 批量变更变速箱箱号
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
            this.cmbNewCVTType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNewCVTNumber = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.旧箱箱号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.新箱箱号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.变更人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.变更时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.变更模式 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.cmbOldCVTType = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkIsNewCVT = new System.Windows.Forms.CheckBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnInput = new System.Windows.Forms.ToolStripButton();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnSet = new System.Windows.Forms.ToolStripButton();
            this.btnImportBill = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.txtOldCVTNumber = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmbNewCVTType
            // 
            this.cmbNewCVTType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewCVTType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbNewCVTType.FormattingEnabled = true;
            this.cmbNewCVTType.Location = new System.Drawing.Point(371, 48);
            this.cmbNewCVTType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbNewCVTType.Name = "cmbNewCVTType";
            this.cmbNewCVTType.Size = new System.Drawing.Size(189, 24);
            this.cmbNewCVTType.TabIndex = 2;
            this.cmbNewCVTType.SelectedIndexChanged += new System.EventHandler(this.cmbCVTType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(293, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "新箱型号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(293, 97);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "新箱箱号";
            // 
            // txtNewCVTNumber
            // 
            this.txtNewCVTNumber.AcceptsReturn = true;
            this.txtNewCVTNumber.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtNewCVTNumber.Location = new System.Drawing.Point(371, 92);
            this.txtNewCVTNumber.Name = "txtNewCVTNumber";
            this.txtNewCVTNumber.Size = new System.Drawing.Size(189, 26);
            this.txtNewCVTNumber.TabIndex = 3;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.旧箱箱号,
            this.新箱箱号,
            this.变更人,
            this.变更时间,
            this.变更模式});
            this.dataGridView1.Location = new System.Drawing.Point(11, 145);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1158, 354);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // 旧箱箱号
            // 
            this.旧箱箱号.HeaderText = "旧箱箱号";
            this.旧箱箱号.Name = "旧箱箱号";
            this.旧箱箱号.ReadOnly = true;
            this.旧箱箱号.Width = 180;
            // 
            // 新箱箱号
            // 
            this.新箱箱号.HeaderText = "新箱箱号";
            this.新箱箱号.Name = "新箱箱号";
            this.新箱箱号.ReadOnly = true;
            this.新箱箱号.Width = 180;
            // 
            // 变更人
            // 
            this.变更人.HeaderText = "变更人";
            this.变更人.Name = "变更人";
            this.变更人.ReadOnly = true;
            this.变更人.Width = 180;
            // 
            // 变更时间
            // 
            this.变更时间.HeaderText = "变更时间";
            this.变更时间.Name = "变更时间";
            this.变更时间.ReadOnly = true;
            this.变更时间.Width = 220;
            // 
            // 变更模式
            // 
            this.变更模式.HeaderText = "变更模式";
            this.变更模式.Name = "变更模式";
            this.变更模式.ReadOnly = true;
            this.变更模式.Width = 150;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(588, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 8;
            this.label2.Text = "变更原因";
            // 
            // txtReason
            // 
            this.txtReason.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtReason.Location = new System.Drawing.Point(666, 49);
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(430, 26);
            this.txtReason.TabIndex = 4;
            // 
            // cmbOldCVTType
            // 
            this.cmbOldCVTType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOldCVTType.Font = new System.Drawing.Font("宋体", 12F);
            this.cmbOldCVTType.FormattingEnabled = true;
            this.cmbOldCVTType.Location = new System.Drawing.Point(86, 48);
            this.cmbOldCVTType.Name = "cmbOldCVTType";
            this.cmbOldCVTType.Size = new System.Drawing.Size(189, 24);
            this.cmbOldCVTType.TabIndex = 0;
            this.cmbOldCVTType.SelectedIndexChanged += new System.EventHandler(this.cmbCVTType_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F);
            this.label3.Location = new System.Drawing.Point(8, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 9;
            this.label3.Text = "旧箱型号";
            // 
            // chkIsNewCVT
            // 
            this.chkIsNewCVT.AutoSize = true;
            this.chkIsNewCVT.Location = new System.Drawing.Point(591, 98);
            this.chkIsNewCVT.Name = "chkIsNewCVT";
            this.chkIsNewCVT.Size = new System.Drawing.Size(229, 18);
            this.chkIsNewCVT.TabIndex = 5;
            this.chkIsNewCVT.Text = "变更成新箱箱号(新箱箱号不带F)";
            this.chkIsNewCVT.UseVisualStyleBackColor = true;
            this.chkIsNewCVT.CheckedChanged += new System.EventHandler(this.chkIsNewCVT_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnInput,
            this.btnAdd,
            this.btnSet,
            this.btnImportBill,
            this.toolStripSeparator1,
            this.btnDelete,
            this.btnClear,
            this.toolStripSeparator2,
            this.btnSave});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1179, 38);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnInput
            // 
            this.btnInput.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnInput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnInput.Name = "btnInput";
            this.btnInput.Size = new System.Drawing.Size(153, 35);
            this.btnInput.Text = "导入数据到显示控件";
            this.btnInput.Click += new System.EventHandler(this.btnInput_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(153, 35);
            this.btnAdd.Text = "添加数据到显示控件";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSet
            // 
            this.btnSet.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSet.Name = "btnSet";
            this.btnSet.Size = new System.Drawing.Size(165, 35);
            this.btnSet.Text = "设置当前选中行新箱箱号";
            this.btnSet.Click += new System.EventHandler(this.btnSet_Click);
            // 
            // btnImportBill
            // 
            this.btnImportBill.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnImportBill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnImportBill.Name = "btnImportBill";
            this.btnImportBill.Size = new System.Drawing.Size(195, 35);
            this.btnImportBill.Text = "从营销单据中导入旧箱箱号";
            this.btnImportBill.ToolTipText = "从营销单据中导入旧箱箱号";
            this.btnImportBill.Click += new System.EventHandler(this.btnImportBill_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(118, 35);
            this.btnDelete.Text = "删除选中行(&D)";
            this.btnDelete.ToolTipText = "删除选中行";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(216, 35);
            this.btnClear.Text = "清除数据显示控件所有记录(&C)";
            this.btnClear.ToolTipText = "清除数据显示控件所有记录";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(139, 35);
            this.btnSave.Text = "批量提交(&Submit)";
            this.btnSave.ToolTipText = "将数据显示控件中的所有记录批量提交";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // txtOldCVTNumber
            // 
            this.txtOldCVTNumber.AcceptsReturn = true;
            this.txtOldCVTNumber.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtOldCVTNumber.Location = new System.Drawing.Point(86, 92);
            this.txtOldCVTNumber.Name = "txtOldCVTNumber";
            this.txtOldCVTNumber.Size = new System.Drawing.Size(189, 26);
            this.txtOldCVTNumber.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(8, 95);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 11;
            this.label5.Text = "旧箱箱号";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // 批量变更变速箱箱号
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 508);
            this.Controls.Add(this.txtOldCVTNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkIsNewCVT);
            this.Controls.Add(this.cmbOldCVTType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtNewCVTNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbNewCVTType);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "批量变更变速箱箱号";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量变更变速箱箱号";
            this.Load += new System.EventHandler(this.批量变更变速箱箱号_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbNewCVTType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNewCVTNumber;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.ComboBox cmbOldCVTType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkIsNewCVT;
        private System.Windows.Forms.DataGridViewTextBoxColumn 旧箱箱号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 新箱箱号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 变更人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 变更时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 变更模式;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSet;
        private System.Windows.Forms.ToolStripButton btnImportBill;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.TextBox txtOldCVTNumber;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnInput;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}