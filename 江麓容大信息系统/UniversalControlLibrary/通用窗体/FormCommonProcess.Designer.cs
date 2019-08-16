namespace UniversalControlLibrary
{
    partial class FormCommonProcess
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnHold = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnReback = new System.Windows.Forms.Button();
            this.rbIsread = new System.Windows.Forms.RadioButton();
            this.rbDisagree = new System.Windows.Forms.RadioButton();
            this.rbAgree = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnPackUp = new System.Windows.Forms.Button();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.panel6 = new System.Windows.Forms.Panel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.panel7 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.AutoScroll = true;
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1192, 63);
            this.panel2.TabIndex = 62;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoEllipsis = true;
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(394, 19);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(66, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "标题";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(18, 68);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBox1.Size = new System.Drawing.Size(208, 269);
            this.textBox1.TabIndex = 90;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "意见：";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(18, 355);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(65, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnHold
            // 
            this.btnHold.Location = new System.Drawing.Point(166, 355);
            this.btnHold.Name = "btnHold";
            this.btnHold.Size = new System.Drawing.Size(65, 23);
            this.btnHold.TabIndex = 3;
            this.btnHold.Text = "暂存";
            this.btnHold.UseVisualStyleBackColor = true;
            this.btnHold.Click += new System.EventHandler(this.btnHold_Click);
            // 
            // panel1
            // 
            this.panel1.AutoScroll = true;
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.btnReback);
            this.panel1.Controls.Add(this.rbIsread);
            this.panel1.Controls.Add(this.rbDisagree);
            this.panel1.Controls.Add(this.rbAgree);
            this.panel1.Controls.Add(this.btnHold);
            this.panel1.Controls.Add(this.btnSubmit);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(945, 63);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(247, 705);
            this.panel1.TabIndex = 63;
            // 
            // btnReback
            // 
            this.btnReback.Location = new System.Drawing.Point(92, 355);
            this.btnReback.Name = "btnReback";
            this.btnReback.Size = new System.Drawing.Size(65, 23);
            this.btnReback.TabIndex = 94;
            this.btnReback.Text = "回退";
            this.btnReback.UseVisualStyleBackColor = true;
            this.btnReback.Click += new System.EventHandler(this.btnReback_Click);
            // 
            // rbIsread
            // 
            this.rbIsread.AutoSize = true;
            this.rbIsread.Checked = true;
            this.rbIsread.Location = new System.Drawing.Point(18, 17);
            this.rbIsread.Name = "rbIsread";
            this.rbIsread.Size = new System.Drawing.Size(47, 16);
            this.rbIsread.TabIndex = 93;
            this.rbIsread.TabStop = true;
            this.rbIsread.Text = "已阅";
            this.rbIsread.UseVisualStyleBackColor = true;
            // 
            // rbDisagree
            // 
            this.rbDisagree.AutoSize = true;
            this.rbDisagree.Location = new System.Drawing.Point(140, 17);
            this.rbDisagree.Name = "rbDisagree";
            this.rbDisagree.Size = new System.Drawing.Size(59, 16);
            this.rbDisagree.TabIndex = 92;
            this.rbDisagree.Text = "不同意";
            this.rbDisagree.UseVisualStyleBackColor = true;
            // 
            // rbAgree
            // 
            this.rbAgree.AutoSize = true;
            this.rbAgree.Location = new System.Drawing.Point(79, 17);
            this.rbAgree.Name = "rbAgree";
            this.rbAgree.Size = new System.Drawing.Size(47, 16);
            this.rbAgree.TabIndex = 91;
            this.rbAgree.Text = "同意";
            this.rbAgree.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.AutoScroll = true;
            this.panel3.Controls.Add(this.btnPackUp);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Location = new System.Drawing.Point(907, 63);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(38, 705);
            this.panel3.TabIndex = 64;
            // 
            // btnPackUp
            // 
            this.btnPackUp.BackColor = System.Drawing.SystemColors.Control;
            this.btnPackUp.Location = new System.Drawing.Point(3, 345);
            this.btnPackUp.Name = "btnPackUp";
            this.btnPackUp.Size = new System.Drawing.Size(32, 23);
            this.btnPackUp.TabIndex = 3;
            this.btnPackUp.Text = ">>";
            this.btnPackUp.UseVisualStyleBackColor = false;
            this.btnPackUp.Click += new System.EventHandler(this.btnPackUp_Click);
            // 
            // panel4
            // 
            this.panel4.AutoScroll = true;
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel4.Controls.Add(this.tabControl1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 63);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(907, 705);
            this.panel4.TabIndex = 65;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(903, 701);
            this.tabControl1.TabIndex = 19;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.panel6);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(895, 675);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "正文";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // panel6
            // 
            this.panel6.AutoScroll = true;
            this.panel6.BackColor = System.Drawing.Color.Gainsboro;
            this.panel6.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(889, 669);
            this.panel6.TabIndex = 2;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.panel7);
            this.tabPage2.Controls.Add(this.splitter1);
            this.tabPage2.Controls.Add(this.panel5);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(895, 675);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "他人意见/流程";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // panel7
            // 
            this.panel7.AutoScroll = true;
            this.panel7.BackColor = System.Drawing.Color.Gainsboro;
            this.panel7.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(3, 3);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(889, 248);
            this.panel7.TabIndex = 9;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.splitter1.Location = new System.Drawing.Point(3, 251);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(889, 3);
            this.splitter1.TabIndex = 8;
            this.splitter1.TabStop = false;
            // 
            // panel5
            // 
            this.panel5.AutoScroll = true;
            this.panel5.BackColor = System.Drawing.SystemColors.Control;
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel5.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel5.Location = new System.Drawing.Point(3, 254);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(889, 418);
            this.panel5.TabIndex = 7;
            // 
            // FormCommonProcess
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(1192, 768);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Name = "FormCommonProcess";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FormCommonProcess";
            this.Load += new System.EventHandler(this.FormCommonProcess_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnHold;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button btnPackUp;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton rbDisagree;
        private System.Windows.Forms.RadioButton rbAgree;
        private System.Windows.Forms.RadioButton rbIsread;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Button btnReback;
    }
}