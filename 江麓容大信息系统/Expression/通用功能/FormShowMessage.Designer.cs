namespace Expression
{
    partial class FormShowMessage
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
            this.lbMessageShow = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbMessageShow
            // 
            this.lbMessageShow.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbMessageShow.Location = new System.Drawing.Point(28, 17);
            this.lbMessageShow.Name = "lbMessageShow";
            this.lbMessageShow.Size = new System.Drawing.Size(178, 40);
            this.lbMessageShow.TabIndex = 0;
            this.lbMessageShow.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lbMessageShow_LinkClicked);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.SystemColors.Control;
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(42, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 27);
            this.label1.TabIndex = 1;
            this.label1.Text = "新信息在【消息中心】最顶端并以红色显示";
            // 
            // FormShowMessage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(218, 99);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbMessageShow);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormShowMessage";
            this.Opacity = 0.9;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel lbMessageShow;
        private System.Windows.Forms.Label label1;

    }
}