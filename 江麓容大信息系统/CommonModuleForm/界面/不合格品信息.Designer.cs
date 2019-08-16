namespace CommonBusinessModule
{
    partial class 不合格品信息
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSave = new System.Windows.Forms.ToolStripButton();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbXXLYGK = new System.Windows.Forms.RadioButton();
            this.rbXXLYSCGC = new System.Windows.Forms.RadioButton();
            this.rbXXLYJHJY = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbGHZTPL = new System.Windows.Forms.RadioButton();
            this.rbGHZTXPL = new System.Windows.Forms.RadioButton();
            this.rbGHZTYJ = new System.Windows.Forms.RadioButton();
            this.label16 = new System.Windows.Forms.Label();
            this.txtBHGPMS = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtDJH = new System.Windows.Forms.TextBox();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSave,
            this.btnClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(472, 25);
            this.toolStrip1.TabIndex = 46;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSave
            // 
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnSave.Size = new System.Drawing.Size(67, 22);
            this.btnSave.Text = "提交(&S)";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.refer;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnClose.Size = new System.Drawing.Size(67, 22);
            this.btnClose.Text = "关闭(&C)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(472, 55);
            this.panel1.TabIndex = 47;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbXXLYGK);
            this.groupBox2.Controls.Add(this.rbXXLYSCGC);
            this.groupBox2.Controls.Add(this.rbXXLYJHJY);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(229, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(243, 55);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息来源";
            // 
            // rbXXLYGK
            // 
            this.rbXXLYGK.AutoSize = true;
            this.rbXXLYGK.Location = new System.Drawing.Point(190, 25);
            this.rbXXLYGK.Name = "rbXXLYGK";
            this.rbXXLYGK.Size = new System.Drawing.Size(47, 16);
            this.rbXXLYGK.TabIndex = 27;
            this.rbXXLYGK.Text = "顾客";
            this.rbXXLYGK.UseVisualStyleBackColor = true;
            // 
            // rbXXLYSCGC
            // 
            this.rbXXLYSCGC.AutoSize = true;
            this.rbXXLYSCGC.Location = new System.Drawing.Point(98, 25);
            this.rbXXLYSCGC.Name = "rbXXLYSCGC";
            this.rbXXLYSCGC.Size = new System.Drawing.Size(71, 16);
            this.rbXXLYSCGC.TabIndex = 26;
            this.rbXXLYSCGC.Text = "生产过程";
            this.rbXXLYSCGC.UseVisualStyleBackColor = true;
            // 
            // rbXXLYJHJY
            // 
            this.rbXXLYJHJY.AutoSize = true;
            this.rbXXLYJHJY.Checked = true;
            this.rbXXLYJHJY.Location = new System.Drawing.Point(6, 25);
            this.rbXXLYJHJY.Name = "rbXXLYJHJY";
            this.rbXXLYJHJY.Size = new System.Drawing.Size(71, 16);
            this.rbXXLYJHJY.TabIndex = 25;
            this.rbXXLYJHJY.TabStop = true;
            this.rbXXLYJHJY.Text = "进货检验";
            this.rbXXLYJHJY.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rbGHZTPL);
            this.groupBox1.Controls.Add(this.rbGHZTXPL);
            this.groupBox1.Controls.Add(this.rbGHZTYJ);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(229, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "供货状态";
            // 
            // rbGHZTPL
            // 
            this.rbGHZTPL.AutoSize = true;
            this.rbGHZTPL.Location = new System.Drawing.Point(176, 25);
            this.rbGHZTPL.Name = "rbGHZTPL";
            this.rbGHZTPL.Size = new System.Drawing.Size(47, 16);
            this.rbGHZTPL.TabIndex = 23;
            this.rbGHZTPL.Text = "批量";
            this.rbGHZTPL.UseVisualStyleBackColor = true;
            // 
            // rbGHZTXPL
            // 
            this.rbGHZTXPL.AutoSize = true;
            this.rbGHZTXPL.Location = new System.Drawing.Point(88, 25);
            this.rbGHZTXPL.Name = "rbGHZTXPL";
            this.rbGHZTXPL.Size = new System.Drawing.Size(59, 16);
            this.rbGHZTXPL.TabIndex = 22;
            this.rbGHZTXPL.Text = "小批量";
            this.rbGHZTXPL.UseVisualStyleBackColor = true;
            // 
            // rbGHZTYJ
            // 
            this.rbGHZTYJ.AutoSize = true;
            this.rbGHZTYJ.Checked = true;
            this.rbGHZTYJ.Location = new System.Drawing.Point(12, 25);
            this.rbGHZTYJ.Name = "rbGHZTYJ";
            this.rbGHZTYJ.Size = new System.Drawing.Size(47, 16);
            this.rbGHZTYJ.TabIndex = 21;
            this.rbGHZTYJ.TabStop = true;
            this.rbGHZTYJ.Text = "样件";
            this.rbGHZTYJ.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 155);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 49;
            this.label16.Text = "不合格描述：";
            // 
            // txtBHGPMS
            // 
            this.txtBHGPMS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBHGPMS.BackColor = System.Drawing.Color.White;
            this.txtBHGPMS.Location = new System.Drawing.Point(88, 132);
            this.txtBHGPMS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBHGPMS.Multiline = true;
            this.txtBHGPMS.Name = "txtBHGPMS";
            this.txtBHGPMS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtBHGPMS.Size = new System.Drawing.Size(372, 60);
            this.txtBHGPMS.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "单  据  号";
            // 
            // txtDJH
            // 
            this.txtDJH.Enabled = false;
            this.txtDJH.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtDJH.ForeColor = System.Drawing.Color.Red;
            this.txtDJH.Location = new System.Drawing.Point(88, 92);
            this.txtDJH.Name = "txtDJH";
            this.txtDJH.Size = new System.Drawing.Size(218, 23);
            this.txtDJH.TabIndex = 51;
            // 
            // 不合格品信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(472, 203);
            this.Controls.Add(this.txtDJH);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.txtBHGPMS);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "不合格品信息";
            this.Text = "不合格品信息";
            this.Load += new System.EventHandler(this.不合格品信息_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSave;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbGHZTPL;
        private System.Windows.Forms.RadioButton rbGHZTXPL;
        private System.Windows.Forms.RadioButton rbGHZTYJ;
        private System.Windows.Forms.RadioButton rbXXLYGK;
        private System.Windows.Forms.RadioButton rbXXLYSCGC;
        private System.Windows.Forms.RadioButton rbXXLYJHJY;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtBHGPMS;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtDJH;
    }
}