namespace UniversalControlLibrary
{
    partial class Office文件显示
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Office文件显示));
            this.oframe = new AxDSOFramer.AxFramerControl();
            ((System.ComponentModel.ISupportInitialize)(this.oframe)).BeginInit();
            this.SuspendLayout();
            // 
            // oframe
            // 
            this.oframe.Dock = System.Windows.Forms.DockStyle.Fill;
            this.oframe.Enabled = true;
            this.oframe.Location = new System.Drawing.Point(0, 0);
            this.oframe.Name = "oframe";
            this.oframe.OcxState = ((System.Windows.Forms.AxHost.State)(resources.GetObject("oframe.OcxState")));
            this.oframe.Size = new System.Drawing.Size(969, 541);
            this.oframe.TabIndex = 0;
            // 
            // Office文件显示
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(969, 541);
            this.Controls.Add(this.oframe);
            this.Name = "Office文件显示";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Office文件显示";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Office文件显示_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.oframe)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private AxDSOFramer.AxFramerControl oframe;
    }
}