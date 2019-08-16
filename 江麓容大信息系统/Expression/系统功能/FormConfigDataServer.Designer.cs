namespace Expression
{
    partial class FormConfigDataServer
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnTestCurServer = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtServerIP = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtServerIP);
            this.groupBox2.Controls.Add(this.btnTestCurServer);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(12, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(376, 69);
            this.groupBox2.TabIndex = 7;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "目前配置的数据库服务器IP地址";
            // 
            // btnTestCurServer
            // 
            this.btnTestCurServer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTestCurServer.Location = new System.Drawing.Point(196, 28);
            this.btnTestCurServer.Name = "btnTestCurServer";
            this.btnTestCurServer.Size = new System.Drawing.Size(163, 23);
            this.btnTestCurServer.TabIndex = 6;
            this.btnTestCurServer.Text = "测试当前服务器连接";
            this.btnTestCurServer.UseVisualStyleBackColor = true;
            this.btnTestCurServer.Click += new System.EventHandler(this.btnTestCurServer_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 4;
            this.label1.Text = "IP地址";
            // 
            // txtServerIP
            // 
            this.txtServerIP.Location = new System.Drawing.Point(61, 28);
            this.txtServerIP.Name = "txtServerIP";
            this.txtServerIP.Size = new System.Drawing.Size(129, 23);
            this.txtServerIP.TabIndex = 7;
            // 
            // FormConfigDataServer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(406, 98);
            this.Controls.Add(this.groupBox2);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfigDataServer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "配置数据库服务器";
            this.Load += new System.EventHandler(this.FormConfigDataServer_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FormConfigDataServer_FormClosed);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnTestCurServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtServerIP;

    }
}