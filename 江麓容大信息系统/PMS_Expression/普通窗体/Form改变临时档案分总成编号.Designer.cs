namespace Expression
{
    partial class Form改变临时档案分总成编号
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
            this.txtOldNumber = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtNewNumber = new System.Windows.Forms.TextBox();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtOldNumber
            // 
            this.txtOldNumber.AcceptsReturn = true;
            this.txtOldNumber.Location = new System.Drawing.Point(140, 21);
            this.txtOldNumber.Name = "txtOldNumber";
            this.txtOldNumber.Size = new System.Drawing.Size(188, 23);
            this.txtOldNumber.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 14);
            this.label1.TabIndex = 12;
            this.label1.Text = "旧的分总成编号";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(105, 14);
            this.label2.TabIndex = 12;
            this.label2.Text = "新的分总成编号";
            // 
            // txtNewNumber
            // 
            this.txtNewNumber.AcceptsReturn = true;
            this.txtNewNumber.Location = new System.Drawing.Point(140, 72);
            this.txtNewNumber.Name = "txtNewNumber";
            this.txtNewNumber.Size = new System.Drawing.Size(188, 23);
            this.txtNewNumber.TabIndex = 13;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(213, 140);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 36);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "取消(&Cancel)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(42, 137);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 36);
            this.btnOK.TabIndex = 14;
            this.btnOK.Text = "确定(&OK)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Form改变临时档案分总成编号
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(364, 188);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtNewNumber);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtOldNumber);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form改变临时档案分总成编号";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "改变临时档案分总成编号";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtOldNumber;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtNewNumber;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
    }
}