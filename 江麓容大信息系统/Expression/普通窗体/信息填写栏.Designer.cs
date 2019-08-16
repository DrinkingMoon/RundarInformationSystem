namespace Expression
{
    partial class 信息填写栏
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
            this.btnSave = new System.Windows.Forms.Button();
            this.btnOut = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtMeansAndAsk = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtManHour = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // txtReason
            // 
            this.txtReason.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.Location = new System.Drawing.Point(77, 24);
            this.txtReason.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReason.Size = new System.Drawing.Size(362, 52);
            this.txtReason.TabIndex = 10;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(261, 300);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 12;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnOut
            // 
            this.btnOut.Location = new System.Drawing.Point(369, 300);
            this.btnOut.Name = "btnOut";
            this.btnOut.Size = new System.Drawing.Size(75, 23);
            this.btnOut.TabIndex = 13;
            this.btnOut.Text = "退出";
            this.btnOut.UseVisualStyleBackColor = true;
            this.btnOut.Click += new System.EventHandler(this.btnOut_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 50);
            this.label2.TabIndex = 14;
            this.label2.Text = "挑选返工返修方法和要求:";
            // 
            // label22
            // 
            this.label22.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label22.Location = new System.Drawing.Point(12, 24);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(59, 52);
            this.label22.TabIndex = 68;
            this.label22.Text = "挑选返工返修原因:";
            // 
            // txtMeansAndAsk
            // 
            this.txtMeansAndAsk.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMeansAndAsk.BackColor = System.Drawing.Color.White;
            this.txtMeansAndAsk.Location = new System.Drawing.Point(77, 116);
            this.txtMeansAndAsk.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtMeansAndAsk.Multiline = true;
            this.txtMeansAndAsk.Name = "txtMeansAndAsk";
            this.txtMeansAndAsk.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtMeansAndAsk.Size = new System.Drawing.Size(362, 52);
            this.txtMeansAndAsk.TabIndex = 69;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 224);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(59, 50);
            this.label1.TabIndex = 70;
            this.label1.Text = "预计挑选返工返修工时及损失:";
            // 
            // txtManHour
            // 
            this.txtManHour.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtManHour.BackColor = System.Drawing.Color.White;
            this.txtManHour.Location = new System.Drawing.Point(77, 224);
            this.txtManHour.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtManHour.Multiline = true;
            this.txtManHour.Name = "txtManHour";
            this.txtManHour.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtManHour.Size = new System.Drawing.Size(362, 52);
            this.txtManHour.TabIndex = 71;
            // 
            // 信息填写栏
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(451, 332);
            this.Controls.Add(this.txtManHour);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtMeansAndAsk);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnOut);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.txtReason);
            this.Location = new System.Drawing.Point(270, 365);
            this.Name = "信息填写栏";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "信息填写栏";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnOut;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtMeansAndAsk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtManHour;
    }
}