namespace Form_Manufacture_Storage
{
    partial class 整台份请领单
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(整台份请领单));
            this.btnShortage = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSign = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSelect = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
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
            // btnShortage
            // 
            this.btnShortage.Image = ((System.Drawing.Image)(resources.GetObject("btnShortage.Image")));
            this.btnShortage.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShortage.Name = "btnShortage";
            this.btnShortage.Size = new System.Drawing.Size(91, 22);
            this.btnShortage.Tag = "StockIn";
            this.btnShortage.Text = "缺料发料(&S)";
            this.btnShortage.Click += new System.EventHandler(this.btnShortage_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "StockIn";
            // 
            // btnSign
            // 
            this.btnSign.Image = ((System.Drawing.Image)(resources.GetObject("btnSign.Image")));
            this.btnSign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(151, 22);
            this.btnSign.Tag = "StockIn";
            this.btnSign.Text = "标记已完成全部发料(&S)";
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "StockIn";
            // 
            // btnSelect
            // 
            this.btnSelect.Image = ((System.Drawing.Image)(resources.GetObject("btnSign.Image")));
            this.btnSelect.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(151, 22);
            this.btnSelect.Tag = "View";
            this.btnSelect.Text = "查看物料状态(&G)";
            this.btnSelect.Click += new System.EventHandler(btnSelect_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "View";
            // 
            // 整台份请领单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 435);
            this.Name = "整台份请领单";
            this.Text = "整台份请领单";
            this.Form_CommonProcessSubmit += new UniversalControlLibrary.FormCommonProcess.FormSubmit(this.整台份请领单_Form_CommonProcessSubmit);
            this.Load += new System.EventHandler(this.整台份请领单_Load);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { btnSelect, toolStripSeparator3, btnShortage, toolStripSeparator1, btnSign, toolStripSeparator2 });

        }

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnShortage;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnSign;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnSelect;

        #endregion

    }
}