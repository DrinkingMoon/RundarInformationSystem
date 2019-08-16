namespace Expression
{
    partial class FormCopyProductInfo
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbParentName = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSourceProductName = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cmbSourceProductType = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.txtTargetProductType = new System.Windows.Forms.TextBox();
            this.btnFindProduct = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmbParentName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSourceProductName);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.cmbSourceProductType);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Location = new System.Drawing.Point(12, 7);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(267, 135);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "源产品信息";
            // 
            // cmbParentName
            // 
            this.cmbParentName.BackColor = System.Drawing.Color.White;
            this.cmbParentName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbParentName.FormattingEnabled = true;
            this.cmbParentName.Location = new System.Drawing.Point(85, 97);
            this.cmbParentName.Name = "cmbParentName";
            this.cmbParentName.Size = new System.Drawing.Size(154, 22);
            this.cmbParentName.TabIndex = 167;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 101);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 168;
            this.label3.Text = "分总成";
            // 
            // txtSourceProductName
            // 
            this.txtSourceProductName.AcceptsReturn = true;
            this.txtSourceProductName.BackColor = System.Drawing.Color.White;
            this.txtSourceProductName.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtSourceProductName.Location = new System.Drawing.Point(85, 59);
            this.txtSourceProductName.Name = "txtSourceProductName";
            this.txtSourceProductName.ReadOnly = true;
            this.txtSourceProductName.Size = new System.Drawing.Size(154, 23);
            this.txtSourceProductName.TabIndex = 164;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 63);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(63, 14);
            this.label14.TabIndex = 166;
            this.label14.Text = "产品名称";
            // 
            // cmbSourceProductType
            // 
            this.cmbSourceProductType.BackColor = System.Drawing.Color.White;
            this.cmbSourceProductType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSourceProductType.FormattingEnabled = true;
            this.cmbSourceProductType.Location = new System.Drawing.Point(85, 22);
            this.cmbSourceProductType.Name = "cmbSourceProductType";
            this.cmbSourceProductType.Size = new System.Drawing.Size(154, 22);
            this.cmbSourceProductType.TabIndex = 163;
            this.cmbSourceProductType.SelectedIndexChanged += new System.EventHandler(this.cmbSourceProductType_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 165;
            this.label9.Text = "产品类型";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnFindProduct);
            this.groupBox2.Controls.Add(this.txtTargetProductType);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(295, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(267, 135);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "目标产品信息";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 169;
            this.label2.Text = "产品类型";
            // 
            // btnOK
            // 
            this.btnOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOK.Location = new System.Drawing.Point(141, 162);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(110, 36);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "确定(&OK)";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Location = new System.Drawing.Point(312, 165);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(110, 36);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "取消(&Cancel)";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // txtTargetProductType
            // 
            this.txtTargetProductType.Location = new System.Drawing.Point(75, 23);
            this.txtTargetProductType.Multiline = true;
            this.txtTargetProductType.Name = "txtTargetProductType";
            this.txtTargetProductType.Size = new System.Drawing.Size(154, 59);
            this.txtTargetProductType.TabIndex = 170;
            // 
            // btnFindProduct
            // 
            this.btnFindProduct.BackColor = System.Drawing.Color.Transparent;
            this.btnFindProduct.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindProduct.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindProduct.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindProduct.Location = new System.Drawing.Point(235, 43);
            this.btnFindProduct.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFindProduct.Name = "btnFindProduct";
            this.btnFindProduct.Size = new System.Drawing.Size(21, 17);
            this.btnFindProduct.TabIndex = 171;
            this.btnFindProduct.UseVisualStyleBackColor = false;
            this.btnFindProduct.Click += new System.EventHandler(this.btnFindProduct_Click);
            // 
            // FormCopyProductInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 213);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormCopyProductInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择产品信息";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtSourceProductName;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cmbSourceProductType;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cmbParentName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtTargetProductType;
        private System.Windows.Forms.Button btnFindProduct;
    }
}