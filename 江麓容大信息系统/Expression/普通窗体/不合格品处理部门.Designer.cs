namespace Expression
{
    partial class 不合格品处理部门
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
            this.btSure = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.rbYX = new System.Windows.Forms.RadioButton();
            this.rbZZ = new System.Windows.Forms.RadioButton();
            this.rbJSZX = new System.Windows.Forms.RadioButton();
            this.rbZG = new System.Windows.Forms.RadioButton();
            this.rbCG = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btSure
            // 
            this.btSure.Location = new System.Drawing.Point(53, 233);
            this.btSure.Name = "btSure";
            this.btSure.Size = new System.Drawing.Size(75, 23);
            this.btSure.TabIndex = 1;
            this.btSure.Text = "确定";
            this.btSure.UseVisualStyleBackColor = true;
            this.btSure.Click += new System.EventHandler(this.btSure_Click);
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(152, 233);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(75, 23);
            this.btClose.TabIndex = 6;
            this.btClose.Text = "取消";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // rbYX
            // 
            this.rbYX.AutoSize = true;
            this.rbYX.Location = new System.Drawing.Point(25, 116);
            this.rbYX.Name = "rbYX";
            this.rbYX.Size = new System.Drawing.Size(59, 16);
            this.rbYX.TabIndex = 19;
            this.rbYX.TabStop = true;
            this.rbYX.Tag = "YX";
            this.rbYX.Text = "营销部";
            this.rbYX.UseVisualStyleBackColor = true;
            // 
            // rbZZ
            // 
            this.rbZZ.AutoSize = true;
            this.rbZZ.Location = new System.Drawing.Point(172, 116);
            this.rbZZ.Name = "rbZZ";
            this.rbZZ.Size = new System.Drawing.Size(59, 16);
            this.rbZZ.TabIndex = 17;
            this.rbZZ.TabStop = true;
            this.rbZZ.Tag = "SC";
            this.rbZZ.Text = "生产部";
            this.rbZZ.UseVisualStyleBackColor = true;
            // 
            // rbJSZX
            // 
            this.rbJSZX.AutoSize = true;
            this.rbJSZX.Location = new System.Drawing.Point(25, 168);
            this.rbJSZX.Name = "rbJSZX";
            this.rbJSZX.Size = new System.Drawing.Size(83, 16);
            this.rbJSZX.TabIndex = 16;
            this.rbJSZX.TabStop = true;
            this.rbJSZX.Tag = "KF";
            this.rbJSZX.Text = "产品开发部";
            this.rbJSZX.UseVisualStyleBackColor = true;
            // 
            // rbZG
            // 
            this.rbZG.AutoSize = true;
            this.rbZG.Location = new System.Drawing.Point(172, 64);
            this.rbZG.Name = "rbZG";
            this.rbZG.Size = new System.Drawing.Size(59, 16);
            this.rbZG.TabIndex = 15;
            this.rbZG.TabStop = true;
            this.rbZG.Tag = "ZK";
            this.rbZG.Text = "质管部";
            this.rbZG.UseVisualStyleBackColor = true;
            // 
            // rbCG
            // 
            this.rbCG.AutoSize = true;
            this.rbCG.Location = new System.Drawing.Point(25, 64);
            this.rbCG.Name = "rbCG";
            this.rbCG.Size = new System.Drawing.Size(59, 16);
            this.rbCG.TabIndex = 18;
            this.rbCG.TabStop = true;
            this.rbCG.Tag = "CG";
            this.rbCG.Text = "采购部";
            this.rbCG.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(92, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "请求处理部门";
            // 
            // 不合格品处理部门
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 282);
            this.Controls.Add(this.rbYX);
            this.Controls.Add(this.rbZZ);
            this.Controls.Add(this.rbJSZX);
            this.Controls.Add(this.rbZG);
            this.Controls.Add(this.rbCG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btSure);
            this.Name = "不合格品处理部门";
            this.Text = "不合格品处理部门";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSure;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.RadioButton rbYX;
        private System.Windows.Forms.RadioButton rbZZ;
        private System.Windows.Forms.RadioButton rbJSZX;
        private System.Windows.Forms.RadioButton rbZG;
        private System.Windows.Forms.RadioButton rbCG;
        private System.Windows.Forms.Label label1;
    }
}