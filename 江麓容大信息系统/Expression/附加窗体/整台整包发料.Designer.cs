namespace Expression
{
    partial class 整台整包发料
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
            this.numFetchCount = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbProductType = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numFetchCount)).BeginInit();
            this.SuspendLayout();
            // 
            // numFetchCount
            // 
            this.numFetchCount.BackColor = System.Drawing.Color.White;
            this.numFetchCount.DecimalPlaces = 2;
            this.numFetchCount.Location = new System.Drawing.Point(107, 59);
            this.numFetchCount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numFetchCount.Name = "numFetchCount";
            this.numFetchCount.Size = new System.Drawing.Size(172, 21);
            this.numFetchCount.TabIndex = 214;
            this.numFetchCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(37, 63);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 215;
            this.label15.Text = "领用台数";
            // 
            // cmbProductType
            // 
            this.cmbProductType.BackColor = System.Drawing.Color.White;
            this.cmbProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbProductType.FormattingEnabled = true;
            this.cmbProductType.Location = new System.Drawing.Point(107, 19);
            this.cmbProductType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbProductType.Name = "cmbProductType";
            this.cmbProductType.Size = new System.Drawing.Size(172, 20);
            this.cmbProductType.TabIndex = 223;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(38, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 224;
            this.label6.Text = "产品类型";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(128, 95);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 225;
            this.btnGenerate.Text = "生成";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // 整台整包发料
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(316, 133);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.cmbProductType);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.numFetchCount);
            this.Controls.Add(this.label15);
            this.Name = "整台整包发料";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "整台整包发料";
            ((System.ComponentModel.ISupportInitialize)(this.numFetchCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown numFetchCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbProductType;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnGenerate;
    }
}