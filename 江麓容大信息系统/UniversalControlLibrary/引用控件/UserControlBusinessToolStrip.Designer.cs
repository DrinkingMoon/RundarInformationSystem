namespace UniversalControlLibrary
{
    partial class UserControlBusinessToolStrip
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPropose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAudit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAuthorize = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPass = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAffrim = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReback = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPropose,
            this.toolStripSeparator4,
            this.btnAudit,
            this.toolStripSeparator1,
            this.btnAuthorize,
            this.toolStripSeparator2,
            this.btnPass,
            this.toolStripSeparator3,
            this.btnAffrim,
            this.toolStripSeparator9,
            this.btnExport,
            this.toolStripSeparator8,
            this.btnPrint,
            this.toolStripSeparator6,
            this.btnRefresh,
            this.toolStripSeparator7,
            this.btnReback,
            this.toolStripSeparator5});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(825, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPropose
            // 
            this.btnPropose.Enabled = false;
            this.btnPropose.Image = global::UniversalControlLibrary.Properties.Resources.提交;
            this.btnPropose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPropose.Name = "btnPropose";
            this.btnPropose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPropose.Size = new System.Drawing.Size(67, 22);
            this.btnPropose.Tag = "新建";
            this.btnPropose.Text = "提交(&T)";
            this.btnPropose.Click += new System.EventHandler(this.btnPropose_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAudit
            // 
            this.btnAudit.Enabled = false;
            this.btnAudit.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnAudit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAudit.Name = "btnAudit";
            this.btnAudit.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAudit.Size = new System.Drawing.Size(67, 22);
            this.btnAudit.Tag = "审核";
            this.btnAudit.Text = "审核(&S)";
            this.btnAudit.Click += new System.EventHandler(this.btnAudit_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAuthorize
            // 
            this.btnAuthorize.Enabled = false;
            this.btnAuthorize.Image = global::UniversalControlLibrary.Properties.Resources.审核6;
            this.btnAuthorize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAuthorize.Name = "btnAuthorize";
            this.btnAuthorize.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAuthorize.Size = new System.Drawing.Size(67, 22);
            this.btnAuthorize.Tag = "批准";
            this.btnAuthorize.Text = "批准(&P)";
            this.btnAuthorize.Click += new System.EventHandler(this.btnAuthorize_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPass
            // 
            this.btnPass.Enabled = false;
            this.btnPass.Image = global::UniversalControlLibrary.Properties.Resources.match;
            this.btnPass.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPass.Name = "btnPass";
            this.btnPass.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPass.Size = new System.Drawing.Size(67, 22);
            this.btnPass.Tag = "通过";
            this.btnPass.Text = "通过(&G)";
            this.btnPass.Click += new System.EventHandler(this.btnPass_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAffrim
            // 
            this.btnAffrim.Enabled = false;
            this.btnAffrim.Image = global::UniversalControlLibrary.Properties.Resources.greentick;
            this.btnAffrim.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAffrim.Name = "btnAffrim";
            this.btnAffrim.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAffrim.Size = new System.Drawing.Size(67, 22);
            this.btnAffrim.Tag = "确认";
            this.btnAffrim.Text = "确认(&Q)";
            this.btnAffrim.Click += new System.EventHandler(this.btnAffrim_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExport
            // 
            this.btnExport.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnExport.Size = new System.Drawing.Size(67, 22);
            this.btnExport.Tag = "";
            this.btnExport.Text = "导出(&C)";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // toolStripSeparator8
            // 
            this.toolStripSeparator8.Name = "toolStripSeparator8";
            this.toolStripSeparator8.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::UniversalControlLibrary.Properties.Resources.Printer2;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnPrint.Size = new System.Drawing.Size(67, 22);
            this.btnPrint.Tag = "";
            this.btnPrint.Text = "打印(&D)";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnRefresh.Size = new System.Drawing.Size(67, 22);
            this.btnRefresh.Tag = "";
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // btnReback
            // 
            this.btnReback.Image = global::UniversalControlLibrary.Properties.Resources.回退;
            this.btnReback.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReback.Name = "btnReback";
            this.btnReback.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnReback.Size = new System.Drawing.Size(67, 22);
            this.btnReback.Tag = "";
            this.btnReback.Text = "回退(&H)";
            this.btnReback.Click += new System.EventHandler(this.btnReback_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // UserControlBusinessToolStrip
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Name = "UserControlBusinessToolStrip";
            this.Size = new System.Drawing.Size(825, 25);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPropose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnAudit;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAuthorize;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnAffrim;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnReback;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnPass;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
