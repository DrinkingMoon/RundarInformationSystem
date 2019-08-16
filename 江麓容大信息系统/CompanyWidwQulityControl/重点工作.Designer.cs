namespace Form_Peripheral_CompanyQuality
{
    partial class 重点工作
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
            this.btnPublish = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnPublish
            // 
            this.btnPublish.Image = global::Form_Peripheral_CompanyQuality.Properties.Resources.审核1;
            this.btnPublish.Name = "btnPublish";
            this.btnPublish.Size = new System.Drawing.Size(115, 22);
            this.btnPublish.Tag = "Auditing";
            this.btnPublish.Text = "发布重点工作(&P)";
            this.btnPublish.Click += new System.EventHandler(this.btnPublish_Click);
            // 
            // toolStripSeparator_1
            // 
            this.toolStripSeparator_1.Name = "toolStripSeparator_1";
            this.toolStripSeparator_1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator_1.Tag = "Auditing";
            // 
            // 重点工作
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 435);
            this.Name = "重点工作";
            this.Text = "重点工作";
            this.Form_CommonProcessSubmit += new UniversalControlLibrary.FormCommonProcess.FormSubmit(this.重点工作_Form_CommonProcessSubmit);
            this.Load += new System.EventHandler(this.重点工作_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnPublish, toolStripSeparator_1 });

        }

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_1;
        private System.Windows.Forms.ToolStripButton btnPublish;

        #endregion
    }
}