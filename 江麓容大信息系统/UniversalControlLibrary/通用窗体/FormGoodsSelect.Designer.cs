namespace UniversalControlLibrary
{
    partial class FormGoodsSelect
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
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnFindCode = new System.Windows.Forms.Button();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Location = new System.Drawing.Point(67, 66);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(175, 21);
            this.txtSpec.TabIndex = 213;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 216;
            this.label7.Text = "规    格";
            // 
            // btnFindCode
            // 
            this.btnFindCode.BackColor = System.Drawing.Color.Transparent;
            this.btnFindCode.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindCode.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindCode.Image = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindCode.Location = new System.Drawing.Point(248, 12);
            this.btnFindCode.Name = "btnFindCode";
            this.btnFindCode.Size = new System.Drawing.Size(21, 21);
            this.btnFindCode.TabIndex = 211;
            this.btnFindCode.UseVisualStyleBackColor = false;
            this.btnFindCode.Click += new System.EventHandler(this.btnFindCode_Click);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Location = new System.Drawing.Point(67, 39);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtName.Size = new System.Drawing.Size(175, 21);
            this.txtName.TabIndex = 212;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 16);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 215;
            this.label12.Text = "图号型号";
            // 
            // txtCode
            // 
            this.txtCode.BackColor = System.Drawing.Color.White;
            this.txtCode.Location = new System.Drawing.Point(67, 12);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCode.Size = new System.Drawing.Size(175, 21);
            this.txtCode.TabIndex = 210;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 214;
            this.label1.Text = "物品名称";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(100, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 217;
            this.button1.Text = "确定";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormGoodsSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(274, 124);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtSpec);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.btnFindCode);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.txtCode);
            this.Controls.Add(this.label1);
            this.Name = "FormGoodsSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "物品选择";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button btnFindCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
    }
}