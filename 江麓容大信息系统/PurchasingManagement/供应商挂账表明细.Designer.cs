namespace Form_Economic_Purchase
{
    partial class 供应商挂账表明细
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.年月 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.挂账方式 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.审核人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.审核时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.复审人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.复审时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最后打印人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.最后打印时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.btnCancel);
            this.customGroupBox1.Controls.Add(this.btnOK);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 306);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(1008, 55);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.年月,
            this.供应商,
            this.挂账方式,
            this.审核人,
            this.审核时间,
            this.复审人,
            this.复审时间,
            this.最后打印人,
            this.最后打印时间,
            this.F_Id,
            this.GoodsID});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(1008, 306);
            this.customDataGridView1.TabIndex = 2;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(403, 20);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(531, 20);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // 年月
            // 
            this.年月.DataPropertyName = "年月";
            this.年月.HeaderText = "年月";
            this.年月.Name = "年月";
            this.年月.Visible = false;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.Visible = false;
            // 
            // 挂账方式
            // 
            this.挂账方式.DataPropertyName = "挂账方式";
            this.挂账方式.HeaderText = "挂账方式";
            this.挂账方式.Name = "挂账方式";
            this.挂账方式.Visible = false;
            // 
            // 审核人
            // 
            this.审核人.DataPropertyName = "审核人";
            this.审核人.HeaderText = "审核人";
            this.审核人.Name = "审核人";
            this.审核人.Visible = false;
            // 
            // 审核时间
            // 
            this.审核时间.DataPropertyName = "审核时间";
            this.审核时间.HeaderText = "审核时间";
            this.审核时间.Name = "审核时间";
            this.审核时间.Visible = false;
            // 
            // 复审人
            // 
            this.复审人.DataPropertyName = "复审人";
            this.复审人.HeaderText = "复审人";
            this.复审人.Name = "复审人";
            this.复审人.Visible = false;
            // 
            // 复审时间
            // 
            this.复审时间.DataPropertyName = "复审时间";
            this.复审时间.HeaderText = "复审时间";
            this.复审时间.Name = "复审时间";
            this.复审时间.Visible = false;
            // 
            // 最后打印人
            // 
            this.最后打印人.DataPropertyName = "最后打印人";
            this.最后打印人.HeaderText = "最后打印人";
            this.最后打印人.Name = "最后打印人";
            this.最后打印人.Visible = false;
            // 
            // 最后打印时间
            // 
            this.最后打印时间.DataPropertyName = "最后打印时间";
            this.最后打印时间.HeaderText = "最后打印时间";
            this.最后打印时间.Name = "最后打印时间";
            this.最后打印时间.Visible = false;
            // 
            // F_Id
            // 
            this.F_Id.DataPropertyName = "F_Id";
            this.F_Id.HeaderText = "F_Id";
            this.F_Id.Name = "F_Id";
            this.F_Id.Visible = false;
            // 
            // GoodsID
            // 
            this.GoodsID.DataPropertyName = "GoodsID";
            this.GoodsID.HeaderText = "GoodsID";
            this.GoodsID.Name = "GoodsID";
            this.GoodsID.Visible = false;
            // 
            // 供应商挂账表明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 361);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "供应商挂账表明细";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "供应商挂账表明细";
            this.customGroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Button btnOK;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.DataGridViewTextBoxColumn 年月;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.DataGridViewTextBoxColumn 挂账方式;
        private System.Windows.Forms.DataGridViewTextBoxColumn 审核人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 审核时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 复审人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 复审时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最后打印人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 最后打印时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsID;
    }
}