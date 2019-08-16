namespace Form_Economic_Financial
{
    partial class 月度预算申请表
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
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // 月度预算申请表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 435);
            this.Name = "月度预算申请表";
            this.Text = "月度预算申请表";
            this.Form_CommonProcessSubmit += new UniversalControlLibrary.FormCommonProcess.FormSubmit(this.月度预算申请表_Form_CommonProcessSubmit);
            this.Form_btnPrint += new GlobalObject.DelegateCollection.ButtonClick(this.月度预算申请表_Form_btnPrint);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
    }
}