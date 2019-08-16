namespace Expression
{
    partial class 单据统计
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.dateTime_endTime = new System.Windows.Forms.DateTimePicker();
            this.dateTime_startTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvStatistical = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.btnApplicantSelect = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistical)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnApplicantSelect);
            this.groupBox1.Controls.Add(this.btnPrint);
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.dateTime_endTime);
            this.groupBox1.Controls.Add(this.dateTime_startTime);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(820, 57);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "根据出库时间查询";
            // 
            // btnPrint
            // 
            this.btnPrint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrint.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(708, 22);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(93, 25);
            this.btnPrint.TabIndex = 12;
            this.btnPrint.Text = "导 出";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Image = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnSelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSelect.Location = new System.Drawing.Point(485, 22);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(93, 25);
            this.btnSelect.TabIndex = 11;
            this.btnSelect.Text = "查 找";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("新宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(263, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 10;
            this.label2.Text = "到";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("新宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(21, 14);
            this.label1.TabIndex = 9;
            this.label1.Text = "从";
            // 
            // dateTime_endTime
            // 
            this.dateTime_endTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime_endTime.Location = new System.Drawing.Point(300, 24);
            this.dateTime_endTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTime_endTime.Name = "dateTime_endTime";
            this.dateTime_endTime.Size = new System.Drawing.Size(180, 21);
            this.dateTime_endTime.TabIndex = 8;
            this.dateTime_endTime.Value = new System.DateTime(2011, 9, 27, 0, 0, 0, 0);
            // 
            // dateTime_startTime
            // 
            this.dateTime_startTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime_startTime.Location = new System.Drawing.Point(54, 24);
            this.dateTime_startTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTime_startTime.Name = "dateTime_startTime";
            this.dateTime_startTime.Size = new System.Drawing.Size(193, 21);
            this.dateTime_startTime.TabIndex = 7;
            this.dateTime_startTime.Value = new System.DateTime(2011, 9, 27, 0, 0, 0, 0);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvStatistical);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(820, 510);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息的显示";
            // 
            // dgvStatistical
            // 
            this.dgvStatistical.AllowUserToAddRows = false;
            this.dgvStatistical.AllowUserToDeleteRows = false;
            this.dgvStatistical.AllowUserToResizeRows = false;
            this.dgvStatistical.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvStatistical.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatistical.Location = new System.Drawing.Point(6, 22);
            this.dgvStatistical.Name = "dgvStatistical";
            this.dgvStatistical.ReadOnly = true;
            this.dgvStatistical.RowTemplate.Height = 23;
            this.dgvStatistical.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvStatistical.Size = new System.Drawing.Size(808, 479);
            this.dgvStatistical.TabIndex = 1;
            this.dgvStatistical.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dgvStatistical_RowPostPaint);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.Title = "将查询结果保存成 EXCEL 文件";
            // 
            // btnApplicantSelect
            // 
            this.btnApplicantSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplicantSelect.Image = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnApplicantSelect.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnApplicantSelect.Location = new System.Drawing.Point(583, 22);
            this.btnApplicantSelect.Name = "btnApplicantSelect";
            this.btnApplicantSelect.Size = new System.Drawing.Size(119, 25);
            this.btnApplicantSelect.TabIndex = 13;
            this.btnApplicantSelect.Text = "按编制人查找";
            this.btnApplicantSelect.UseVisualStyleBackColor = true;
            this.btnApplicantSelect.Click += new System.EventHandler(this.btnApplicantSelect_Click);
            // 
            // 单据统计
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(820, 567);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("新宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "单据统计";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "单据统计";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatistical)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker dateTime_endTime;
        private System.Windows.Forms.DateTimePicker dateTime_startTime;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvStatistical;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnApplicantSelect;
    }
}