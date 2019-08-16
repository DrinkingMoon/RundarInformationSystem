namespace Expression
{
    partial class FormAfreshFitting
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
            this.label1 = new System.Windows.Forms.Label();
            this.cmbAfreshFittingFashion = new System.Windows.Forms.ComboBox();
            this.btnCancle = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "重装方式";
            // 
            // cmbAfreshFittingFashion
            // 
            this.cmbAfreshFittingFashion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAfreshFittingFashion.FormattingEnabled = true;
            this.cmbAfreshFittingFashion.Items.AddRange(new object[] {
            "总成重装",
            "零部件重装"});
            this.cmbAfreshFittingFashion.Location = new System.Drawing.Point(93, 17);
            this.cmbAfreshFittingFashion.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAfreshFittingFashion.Name = "cmbAfreshFittingFashion";
            this.cmbAfreshFittingFashion.Size = new System.Drawing.Size(135, 22);
            this.cmbAfreshFittingFashion.TabIndex = 1;
            // 
            // btnCancle
            // 
            this.btnCancle.Location = new System.Drawing.Point(153, 52);
            this.btnCancle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancle.Name = "btnCancle";
            this.btnCancle.Size = new System.Drawing.Size(75, 24);
            this.btnCancle.TabIndex = 7;
            this.btnCancle.Text = "取消";
            this.btnCancle.UseVisualStyleBackColor = true;
            this.btnCancle.Click += new System.EventHandler(this.btnCancle_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(22, 52);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 24);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "确定";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // FormAfreshFitting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 87);
            this.Controls.Add(this.cmbAfreshFittingFashion);
            this.Controls.Add(this.btnCancle);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Font = new System.Drawing.Font("新宋体", 10.5F);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(255, 119);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(255, 119);
            this.Name = "FormAfreshFitting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "重新装配";
            this.Load += new System.EventHandler(this.FormAfreshFitting_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbAfreshFittingFashion;
        private System.Windows.Forms.Button btnCancle;
        private System.Windows.Forms.Button btnOk;
    }
}