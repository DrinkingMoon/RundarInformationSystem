namespace Expression
{
    partial class FormPrintAssemblyBarCodeForRepeatedMode
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPrintAssemblyBarCodeForRepeatedMode));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.label1 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.txtSerialNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.numCount = new System.Windows.Forms.NumericUpDown();
            this.cmbPrductType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbAssemblyName = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnPrint,
            this.toolStripSeparator1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(384, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnPrint
            // 
            this.btnPrint.Image = global::UniversalControlLibrary.Properties.Resources.print;
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(76, 22);
            this.btnPrint.Text = "打印条码";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "产品类型";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(27, 122);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(105, 14);
            this.lblAmount.TabIndex = 3;
            this.lblAmount.Text = "请输入产品数量";
            // 
            // txtSerialNo
            // 
            this.txtSerialNo.BackColor = System.Drawing.Color.White;
            this.txtSerialNo.Location = new System.Drawing.Point(152, 150);
            this.txtSerialNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSerialNo.Name = "txtSerialNo";
            this.txtSerialNo.Size = new System.Drawing.Size(197, 23);
            this.txtSerialNo.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(119, 14);
            this.label3.TabIndex = 5;
            this.label3.Text = "待打印总成流水码";
            // 
            // numCount
            // 
            this.numCount.BackColor = System.Drawing.Color.White;
            this.numCount.Location = new System.Drawing.Point(152, 118);
            this.numCount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numCount.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.numCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numCount.Name = "numCount";
            this.numCount.Size = new System.Drawing.Size(197, 23);
            this.numCount.TabIndex = 7;
            this.numCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cmbPrductType
            // 
            this.cmbPrductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPrductType.FormattingEnabled = true;
            this.cmbPrductType.Location = new System.Drawing.Point(152, 32);
            this.cmbPrductType.Name = "cmbPrductType";
            this.cmbPrductType.Size = new System.Drawing.Size(197, 22);
            this.cmbPrductType.TabIndex = 8;
            this.cmbPrductType.SelectedIndexChanged += new System.EventHandler(this.cmbPrductType_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(27, 91);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 14);
            this.label4.TabIndex = 9;
            this.label4.Text = "分总成名称";
            // 
            // cmbAssemblyName
            // 
            this.cmbAssemblyName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAssemblyName.FormattingEnabled = true;
            this.cmbAssemblyName.Location = new System.Drawing.Point(152, 87);
            this.cmbAssemblyName.Name = "cmbAssemblyName";
            this.cmbAssemblyName.Size = new System.Drawing.Size(197, 22);
            this.cmbAssemblyName.TabIndex = 8;
            this.cmbAssemblyName.SelectedIndexChanged += new System.EventHandler(this.cmbAssemblyName_SelectedIndexChanged);
            // 
            // label5
            // 
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(27, 192);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(332, 32);
            this.label5.TabIndex = 15;
            this.label5.Text = "条形码日期说明：打印的条形码日期需要变更时使用，只有年、月有效";
            // 
            // lblProductName
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(152, 64);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(0, 14);
            this.lblProductName.TabIndex = 16;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(27, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 17;
            this.label7.Text = "产品名称";
            // 
            // FormPrintAssemblyBarCodeForRepeatedMode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(384, 251);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblProductName);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbAssemblyName);
            this.Controls.Add(this.cmbPrductType);
            this.Controls.Add(this.numCount);
            this.Controls.Add(this.txtSerialNo);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblAmount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(400, 290);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(400, 290);
            this.Name = "FormPrintAssemblyBarCodeForRepeatedMode";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印独立装配分总成条形码（重复使用模式）";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.TextBox txtSerialNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numCount;
        private System.Windows.Forms.ComboBox cmbPrductType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ComboBox cmbAssemblyName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.Label label7;
    }
}