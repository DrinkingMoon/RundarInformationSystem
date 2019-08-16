namespace Form_Peripheral_CompanyQuality
{
    partial class 创意提案
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(创意提案));
            this.btnDirectAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(1034, 272);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(1026, 246);
            // 
            // btnDirectAdd
            // 
            this.btnDirectAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnDirectAdd.Image")));
            this.btnDirectAdd.Name = "btnDirectAdd";
            this.btnDirectAdd.Size = new System.Drawing.Size(92, 22);
            this.btnDirectAdd.Tag = "Confirm_1";
            this.btnDirectAdd.Text = "直接录入(&B)";
            this.btnDirectAdd.Click += new System.EventHandler(this.btnDirectAdd_Click);
            // 
            // toolStripSeparator_1
            // 
            this.toolStripSeparator_1.Name = "toolStripSeparator_1";
            this.toolStripSeparator_1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator_1.Tag = "Confirm_1";
            // 
            // 创意提案
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 435);
            this.Name = "创意提案";
            this.Text = "创意提案";
            this.Form_CommonProcessSubmit += new UniversalControlLibrary.FormCommonProcess.FormSubmit(this.frm_CommonProcessSubmit);
            this.Load += new System.EventHandler(this.创意提案_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_1;
        private System.Windows.Forms.ToolStripButton btnDirectAdd;

        #endregion

    }
}