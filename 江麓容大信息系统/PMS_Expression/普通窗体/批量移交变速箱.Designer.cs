namespace Expression
{
    partial class 批量移交变速箱
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
            this.cmbCVTType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCVTNumber = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.变速箱型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.变速箱箱号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.移交人工号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.移交时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.移交目的地 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnImportBill = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbCVTType
            // 
            this.cmbCVTType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCVTType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbCVTType.FormattingEnabled = true;
            this.cmbCVTType.Location = new System.Drawing.Point(101, 19);
            this.cmbCVTType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbCVTType.Name = "cmbCVTType";
            this.cmbCVTType.Size = new System.Drawing.Size(180, 24);
            this.cmbCVTType.TabIndex = 0;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(7, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(88, 16);
            this.label4.TabIndex = 3;
            this.label4.Text = "变速箱型号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(301, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(88, 16);
            this.label1.TabIndex = 5;
            this.label1.Text = "变速箱箱号";
            // 
            // txtCVTNumber
            // 
            this.txtCVTNumber.AcceptsReturn = true;
            this.txtCVTNumber.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCVTNumber.Location = new System.Drawing.Point(395, 19);
            this.txtCVTNumber.Name = "txtCVTNumber";
            this.txtCVTNumber.Size = new System.Drawing.Size(174, 26);
            this.txtCVTNumber.TabIndex = 1;
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
            this.变速箱型号,
            this.变速箱箱号,
            this.移交人工号,
            this.移交时间,
            this.移交目的地});
            this.dataGridView1.Location = new System.Drawing.Point(11, 68);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.DefaultCellStyle.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1134, 431);
            this.dataGridView1.TabIndex = 7;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // 变速箱型号
            // 
            this.变速箱型号.HeaderText = "变速箱型号";
            this.变速箱型号.Name = "变速箱型号";
            this.变速箱型号.ReadOnly = true;
            this.变速箱型号.Width = 150;
            // 
            // 变速箱箱号
            // 
            this.变速箱箱号.HeaderText = "变速箱箱号";
            this.变速箱箱号.Name = "变速箱箱号";
            this.变速箱箱号.ReadOnly = true;
            this.变速箱箱号.Width = 180;
            // 
            // 移交人工号
            // 
            this.移交人工号.HeaderText = "移交人工号";
            this.移交人工号.Name = "移交人工号";
            this.移交人工号.ReadOnly = true;
            this.移交人工号.Width = 150;
            // 
            // 移交时间
            // 
            this.移交时间.HeaderText = "移交时间";
            this.移交时间.Name = "移交时间";
            this.移交时间.ReadOnly = true;
            this.移交时间.Width = 180;
            // 
            // 移交目的地
            // 
            this.移交目的地.HeaderText = "移交目的地";
            this.移交目的地.Name = "移交目的地";
            this.移交目的地.ReadOnly = true;
            this.移交目的地.Width = 150;
            // 
            // btnAdd
            // 
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(755, 11);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(123, 44);
            this.btnAdd.TabIndex = 3;
            this.btnAdd.Text = "添加(&Add)";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(1019, 11);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(123, 44);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "批量提交(&Submit)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(887, 11);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(123, 44);
            this.btnDelete.TabIndex = 4;
            this.btnDelete.Text = "删除选中行(&Delete)";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnImportBill
            // 
            this.btnImportBill.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImportBill.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnImportBill.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImportBill.Location = new System.Drawing.Point(587, 10);
            this.btnImportBill.Name = "btnImportBill";
            this.btnImportBill.Size = new System.Drawing.Size(159, 44);
            this.btnImportBill.TabIndex = 2;
            this.btnImportBill.Text = "从营销单据中导入";
            this.btnImportBill.UseVisualStyleBackColor = true;
            this.btnImportBill.Click += new System.EventHandler(this.btnImportBill_Click);
            // 
            // 批量移交变速箱
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1155, 508);
            this.Controls.Add(this.btnImportBill);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.txtCVTNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCVTType);
            this.Controls.Add(this.label4);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "批量移交变速箱";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "批量移交变速箱";
            this.Load += new System.EventHandler(this.批量移交变速箱_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbCVTType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtCVTNumber;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 变速箱型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 变速箱箱号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 移交人工号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 移交时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 移交目的地;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnImportBill;
    }
}