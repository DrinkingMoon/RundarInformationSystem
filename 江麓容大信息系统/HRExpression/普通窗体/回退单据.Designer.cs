namespace Form_Peripheral_HR
{
    partial class 回退单据
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
            this.txtReason = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbDJZT = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lbDJZT = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDJH = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.Location = new System.Drawing.Point(145, 168);
            this.txtReason.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.Size = new System.Drawing.Size(203, 58);
            this.txtReason.TabIndex = 119;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(14, 171);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(91, 14);
            this.label4.TabIndex = 127;
            this.label4.Text = "回 退  原 因";
            // 
            // btnClose
            // 
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(253, 249);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(95, 30);
            this.btnClose.TabIndex = 121;
            this.btnClose.Text = "取消";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(145, 249);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(95, 30);
            this.btnOK.TabIndex = 120;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(142, 139);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 14);
            this.label3.TabIndex = 126;
            this.label3.Text = "请选择单据回退后的状态";
            // 
            // cmbDJZT
            // 
            this.cmbDJZT.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDJZT.FormattingEnabled = true;
            this.cmbDJZT.Location = new System.Drawing.Point(145, 111);
            this.cmbDJZT.Name = "cmbDJZT";
            this.cmbDJZT.Size = new System.Drawing.Size(203, 21);
            this.cmbDJZT.TabIndex = 118;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(14, 111);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 125;
            this.label2.Text = "单据退后状态";
            // 
            // lbDJZT
            // 
            this.lbDJZT.ForeColor = System.Drawing.Color.Black;
            this.lbDJZT.Location = new System.Drawing.Point(142, 63);
            this.lbDJZT.Name = "lbDJZT";
            this.lbDJZT.Size = new System.Drawing.Size(206, 34);
            this.lbDJZT.TabIndex = 124;
            this.lbDJZT.Text = "单据状态";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(14, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 14);
            this.label1.TabIndex = 123;
            this.label1.Text = "单据当前状态";
            // 
            // txtDJH
            // 
            this.txtDJH.BackColor = System.Drawing.Color.White;
            this.txtDJH.Location = new System.Drawing.Point(145, 21);
            this.txtDJH.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtDJH.Name = "txtDJH";
            this.txtDJH.ReadOnly = true;
            this.txtDJH.Size = new System.Drawing.Size(203, 23);
            this.txtDJH.TabIndex = 117;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(14, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 122;
            this.label8.Text = "单   据   号";
            // 
            // 回退单据
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 329);
            this.Controls.Add(this.txtReason);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbDJZT);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbDJZT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtDJH);
            this.Controls.Add(this.label8);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.Name = "回退单据";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "回退单据";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbDJZT;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbDJZT;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDJH;
        private System.Windows.Forms.Label label8;
    }
}