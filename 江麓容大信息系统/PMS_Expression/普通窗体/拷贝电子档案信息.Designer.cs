namespace Expression
{
    partial class 拷贝电子档案信息
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
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCVTNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbCVTType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 210);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 0;
            this.label6.Text = "说明";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 134);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(56, 17);
            this.label5.TabIndex = 0;
            this.label5.Text = "新箱箱号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(8, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "新箱型号";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 58);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 17);
            this.label3.TabIndex = 0;
            this.label3.Text = "旧箱箱号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "旧箱型号";
            // 
            // txtCVTNumber
            // 
            this.txtCVTNumber.AcceptsReturn = true;
            this.txtCVTNumber.Location = new System.Drawing.Point(92, 65);
            this.txtCVTNumber.Name = "txtCVTNumber";
            this.txtCVTNumber.Size = new System.Drawing.Size(160, 23);
            this.txtCVTNumber.TabIndex = 11;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 68);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "箱号";
            // 
            // cmbCVTType
            // 
            this.cmbCVTType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCVTType.FormattingEnabled = true;
            this.cmbCVTType.Location = new System.Drawing.Point(92, 16);
            this.cmbCVTType.Name = "cmbCVTType";
            this.cmbCVTType.Size = new System.Drawing.Size(160, 22);
            this.cmbCVTType.TabIndex = 5;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 20);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 7;
            this.label9.Text = "型    号";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(280, 12);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(283, 41);
            this.btnGenerate.TabIndex = 12;
            this.btnGenerate.Text = "基于装配BOM生成档案信息";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label7
            // 
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(277, 65);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(307, 64);
            this.label7.TabIndex = 7;
            this.label7.Text = "基础信息不包含：\r\n    供应商、批次号、测量数据、选配数据\r\n\r\n装配时间为箱号中包含的时间";
            // 
            // 拷贝电子档案信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(596, 140);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.txtCVTNumber);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbCVTType);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label9);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "拷贝电子档案信息";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "拷贝电子档案信息";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCVTNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbCVTType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label label7;
    }
}