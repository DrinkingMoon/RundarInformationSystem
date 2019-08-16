namespace Form_Economic_Purchase
{
    partial class 供应商应付账款
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
            this.btnReAuditing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnReAuditing
            // 
            this.btnReAuditing.Image = global::Form_Economic_Purchase.Properties.Resources.Repair;
            this.btnReAuditing.Name = "btnReAuditing";
            this.btnReAuditing.Size = new System.Drawing.Size(23, 23);
            this.btnReAuditing.Tag = "Auditing";
            this.btnReAuditing.Text = "强制删除(&S)";
            this.btnReAuditing.Click += new System.EventHandler(this.btnReAuditing_Click);
            // 
            // toolStripSeparator_1
            // 
            this.toolStripSeparator_1.Name = "toolStripSeparator_1";
            this.toolStripSeparator_1.Size = new System.Drawing.Size(6, 6);
            this.toolStripSeparator_1.Tag = "Auditing";
            // 
            // 供应商应付账款
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 435);
            this.Name = "供应商应付账款";
            this.Text = "供应商应付账款";
            this.Form_CommonProcessSubmit += new UniversalControlLibrary.FormCommonProcess.FormSubmit(this.供应商应付账款_Form_CommonProcessSubmit);
            this.Load += new System.EventHandler(this.供应商应付账款_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnReAuditing, toolStripSeparator_1 });

        }

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_1;
        private System.Windows.Forms.ToolStripButton btnReAuditing;

        #endregion
    }
}