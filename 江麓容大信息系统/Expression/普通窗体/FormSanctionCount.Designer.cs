namespace Expression
{
    partial class FormSanctionCount
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.numSanctionCount1 = new System.Windows.Forms.NumericUpDown();
            this.numSanctionCount2 = new System.Windows.Forms.NumericUpDown();
            this.numSanctionCount3 = new System.Windows.Forms.NumericUpDown();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.numSanctionCount1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSanctionCount2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSanctionCount3)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(189, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "质量工程师最大允许审批数量";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 83);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(175, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "质量主管最大允许审批数量";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 126);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(175, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "质量总监最大允许审批数量";
            // 
            // numSanctionCount1
            // 
            this.numSanctionCount1.Location = new System.Drawing.Point(240, 37);
            this.numSanctionCount1.Name = "numSanctionCount1";
            this.numSanctionCount1.Size = new System.Drawing.Size(110, 23);
            this.numSanctionCount1.TabIndex = 5;
            // 
            // numSanctionCount2
            // 
            this.numSanctionCount2.Location = new System.Drawing.Point(240, 79);
            this.numSanctionCount2.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numSanctionCount2.Name = "numSanctionCount2";
            this.numSanctionCount2.Size = new System.Drawing.Size(110, 23);
            this.numSanctionCount2.TabIndex = 6;
            // 
            // numSanctionCount3
            // 
            this.numSanctionCount3.Location = new System.Drawing.Point(240, 122);
            this.numSanctionCount3.Name = "numSanctionCount3";
            this.numSanctionCount3.Size = new System.Drawing.Size(110, 23);
            this.numSanctionCount3.TabIndex = 7;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnUpdate});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(383, 25);
            this.toolStrip1.TabIndex = 41;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(49, 22);
            this.btnUpdate.Text = "更新";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // FormSanctionCount
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 168);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.numSanctionCount3);
            this.Controls.Add(this.numSanctionCount2);
            this.Controls.Add(this.numSanctionCount1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(391, 200);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(391, 200);
            this.Name = "FormSanctionCount";
            this.Text = "设置报废流程数量审批";
            this.Load += new System.EventHandler(this.FormSanctionCount_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numSanctionCount1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSanctionCount2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numSanctionCount3)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numSanctionCount1;
        private System.Windows.Forms.NumericUpDown numSanctionCount2;
        private System.Windows.Forms.NumericUpDown numSanctionCount3;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnUpdate;
    }
}