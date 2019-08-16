namespace Expression
{
    partial class 职位信息
    {
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
       

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.txtPositionID = new System.Windows.Forms.TextBox();
            this.txtPositionName = new System.Windows.Forms.TextBox();
            this.txtPositionRemark = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnInsert = new System.Windows.Forms.Button();
            this.btnPosition = new System.Windows.Forms.Button();
            this.dgvShowPosition = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.positionName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShowPosition)).BeginInit();
            this.SuspendLayout();
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 31);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 0;
            this.label8.Text = "职位编号";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(206, 31);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 1;
            this.label9.Text = "职位名称";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 2;
            this.label10.Text = "备    注";
            // 
            // txtPositionID
            // 
            this.txtPositionID.Location = new System.Drawing.Point(80, 28);
            this.txtPositionID.Name = "txtPositionID";
            this.txtPositionID.Size = new System.Drawing.Size(103, 23);
            this.txtPositionID.TabIndex = 3;
            this.txtPositionID.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtPositionID_KeyPress);
            // 
            // txtPositionName
            // 
            this.txtPositionName.Location = new System.Drawing.Point(275, 28);
            this.txtPositionName.Name = "txtPositionName";
            this.txtPositionName.Size = new System.Drawing.Size(140, 23);
            this.txtPositionName.TabIndex = 4;
            // 
            // txtPositionRemark
            // 
            this.txtPositionRemark.Location = new System.Drawing.Point(80, 70);
            this.txtPositionRemark.Name = "txtPositionRemark";
            this.txtPositionRemark.Size = new System.Drawing.Size(335, 23);
            this.txtPositionRemark.TabIndex = 5;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.btnInsert);
            this.groupBox3.Controls.Add(this.btnPosition);
            this.groupBox3.Controls.Add(this.txtPositionRemark);
            this.groupBox3.Controls.Add(this.txtPositionName);
            this.groupBox3.Controls.Add(this.txtPositionID);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Location = new System.Drawing.Point(14, 14);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(605, 114);
            this.groupBox3.TabIndex = 20;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "职位信息";
            // 
            // btnInsert
            // 
            this.btnInsert.Location = new System.Drawing.Point(458, 22);
            this.btnInsert.Name = "btnInsert";
            this.btnInsert.Size = new System.Drawing.Size(104, 31);
            this.btnInsert.TabIndex = 7;
            this.btnInsert.Text = "新 建";
            this.btnInsert.UseVisualStyleBackColor = true;
            this.btnInsert.Click += new System.EventHandler(this.btnInsert_Click);
            // 
            // btnPosition
            // 
            this.btnPosition.Location = new System.Drawing.Point(458, 64);
            this.btnPosition.Name = "btnPosition";
            this.btnPosition.Size = new System.Drawing.Size(104, 31);
            this.btnPosition.TabIndex = 6;
            this.btnPosition.Text = "提 交";
            this.btnPosition.UseVisualStyleBackColor = true;
            this.btnPosition.Click += new System.EventHandler(this.btnPosition_Click);
            // 
            // dgvShowPosition
            // 
            this.dgvShowPosition.AllowUserToAddRows = false;
            this.dgvShowPosition.AllowUserToDeleteRows = false;
            this.dgvShowPosition.AllowUserToOrderColumns = true;
            this.dgvShowPosition.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvShowPosition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvShowPosition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShowPosition.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.positionName,
            this.remark});
            this.dgvShowPosition.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvShowPosition.Location = new System.Drawing.Point(14, 135);
            this.dgvShowPosition.Name = "dgvShowPosition";
            this.dgvShowPosition.RowTemplate.Height = 23;
            this.dgvShowPosition.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvShowPosition.Size = new System.Drawing.Size(605, 378);
            this.dgvShowPosition.TabIndex = 21;
            this.dgvShowPosition.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvShowPosition_RowPostPaint);
            this.dgvShowPosition.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvShowPosition_CellClick);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.HeaderText = "职位编号";
            this.ID.Name = "ID";
            this.ID.Width = 88;
            // 
            // positionName
            // 
            this.positionName.DataPropertyName = "positionName";
            this.positionName.HeaderText = "职位名称";
            this.positionName.Name = "positionName";
            this.positionName.Width = 88;
            // 
            // remark
            // 
            this.remark.DataPropertyName = "Remark";
            this.remark.HeaderText = "备注";
            this.remark.Name = "remark";
            this.remark.Width = 60;
            // 
            // 职位信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(633, 521);
            this.Controls.Add(this.dgvShowPosition);
            this.Controls.Add(this.groupBox3);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "职位信息";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "职位信息";
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShowPosition)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtPositionID;
        private System.Windows.Forms.TextBox txtPositionName;
        private System.Windows.Forms.TextBox txtPositionRemark;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnPosition;
        private System.Windows.Forms.DataGridView dgvShowPosition;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn positionName;
        private System.Windows.Forms.DataGridViewTextBoxColumn remark;
        private System.Windows.Forms.Button btnInsert;

    }
}