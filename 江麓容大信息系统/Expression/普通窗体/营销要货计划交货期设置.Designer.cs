namespace Expression
{
    partial class 营销要货计划交货期设置
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
            this.txtDJH = new System.Windows.Forms.TextBox();
            this.label35 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.numMonthCount = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.dtpDelivery = new System.Windows.Forms.DateTimePicker();
            this.btnDelete = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numDeliveryCount = new System.Windows.Forms.NumericUpDown();
            this.交货期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.交货数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.numMonthCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeliveryCount)).BeginInit();
            this.SuspendLayout();
            // 
            // txtDJH
            // 
            this.txtDJH.BackColor = System.Drawing.Color.White;
            this.txtDJH.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDJH.ForeColor = System.Drawing.Color.Red;
            this.txtDJH.Location = new System.Drawing.Point(82, 33);
            this.txtDJH.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDJH.Name = "txtDJH";
            this.txtDJH.ReadOnly = true;
            this.txtDJH.Size = new System.Drawing.Size(198, 23);
            this.txtDJH.TabIndex = 186;
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label35.Location = new System.Drawing.Point(13, 36);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(63, 14);
            this.label35.TabIndex = 187;
            this.label35.Text = "计划单号";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(82, 63);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(198, 23);
            this.txtName.TabIndex = 219;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(13, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 220;
            this.label6.Text = "产品型号";
            // 
            // numMonthCount
            // 
            this.numMonthCount.Location = new System.Drawing.Point(180, 93);
            this.numMonthCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numMonthCount.Name = "numMonthCount";
            this.numMonthCount.ReadOnly = true;
            this.numMonthCount.Size = new System.Drawing.Size(100, 21);
            this.numMonthCount.TabIndex = 235;
            this.numMonthCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(13, 96);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(133, 14);
            this.label7.TabIndex = 234;
            this.label7.Text = "当前计划月计划总数";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.交货期,
            this.交货数});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView1.Location = new System.Drawing.Point(0, 214);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 21;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(296, 160);
            this.dataGridView1.TabIndex = 236;
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(28, 185);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 237;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(13, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 238;
            this.label1.Text = "交货期";
            // 
            // dtpDelivery
            // 
            this.dtpDelivery.Location = new System.Drawing.Point(80, 122);
            this.dtpDelivery.Name = "dtpDelivery";
            this.dtpDelivery.Size = new System.Drawing.Size(200, 21);
            this.dtpDelivery.TabIndex = 1;
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(180, 185);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 240;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(47, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(203, 14);
            this.label2.TabIndex = 241;
            this.label2.Text = "2012年12月营销计划交货期设置";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(13, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 242;
            this.label3.Text = "交货数";
            // 
            // numDeliveryCount
            // 
            this.numDeliveryCount.Location = new System.Drawing.Point(80, 153);
            this.numDeliveryCount.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numDeliveryCount.Name = "numDeliveryCount";
            this.numDeliveryCount.Size = new System.Drawing.Size(200, 21);
            this.numDeliveryCount.TabIndex = 243;
            this.numDeliveryCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // 交货期
            // 
            this.交货期.DataPropertyName = "交货期";
            this.交货期.HeaderText = "交货期";
            this.交货期.Name = "交货期";
            this.交货期.ReadOnly = true;
            this.交货期.Width = 180;
            // 
            // 交货数
            // 
            this.交货数.DataPropertyName = "交货数";
            this.交货数.HeaderText = "交货数";
            this.交货数.Name = "交货数";
            this.交货数.ReadOnly = true;
            this.交货数.Width = 90;
            // 
            // 营销要货计划交货期设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 374);
            this.Controls.Add(this.numDeliveryCount);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.dtpDelivery);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.numMonthCount);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtDJH);
            this.Controls.Add(this.label35);
            this.Name = "营销要货计划交货期设置";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "营销要货计划交货期设置";
            ((System.ComponentModel.ISupportInitialize)(this.numMonthCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numDeliveryCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDJH;
        private System.Windows.Forms.Label label35;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown numMonthCount;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dtpDelivery;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numDeliveryCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交货期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 交货数;
    }
}