namespace Form_Economic_Purchase
{
    partial class 采购结算单
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(采购结算单));
            this.btnReAuditing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
            this.SuspendLayout();
            // 
            // 采购结算单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 435);
            this.Name = "采购结算单";
            this.Text = "采购结算单";
            this.Load += new System.EventHandler(this.采购结算单_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
            // 
            // btnReAuditing
            // 
            this.btnReAuditing.Image = ((System.Drawing.Image)(resources.GetObject("btnReAuditing.Image")));
            this.btnReAuditing.Name = "btnReAuditing";
            this.btnReAuditing.Tag = "Auditing";
            this.btnReAuditing.Text = "重审选中的单据(&A)";
            this.btnReAuditing.Click += new System.EventHandler(this.btnReAuditing_Click);
            // 
            // toolStripSeparator_1
            // 
            this.toolStripSeparator_1.Name = "toolStripSeparator_1";
            this.toolStripSeparator_1.Tag = "Auditing";
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