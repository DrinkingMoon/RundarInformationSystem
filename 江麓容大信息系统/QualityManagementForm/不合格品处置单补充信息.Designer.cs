namespace Form_Quality_QC
{
    partial class 不合格品处置单补充信息
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
            this.txtMessage = new UniversalControlLibrary.TextBoxShow();
            this.lbText = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtMessage
            // 
            this.txtMessage.DataResult = null;
            this.txtMessage.DataTableResult = null;
            this.txtMessage.EditingControlDataGridView = null;
            this.txtMessage.EditingControlFormattedValue = "";
            this.txtMessage.EditingControlRowIndex = 0;
            this.txtMessage.EditingControlValueChanged = true;
            this.txtMessage.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtMessage.IsMultiSelect = true;
            this.txtMessage.Location = new System.Drawing.Point(12, 57);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtMessage.Multiline = true;
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.ShowResultForm = false;
            this.txtMessage.Size = new System.Drawing.Size(449, 177);
            this.txtMessage.StrEndSql = null;
            this.txtMessage.TabIndex = 0;
            this.txtMessage.TabStop = false;
            // 
            // lbText
            // 
            this.lbText.AutoSize = true;
            this.lbText.Location = new System.Drawing.Point(31, 21);
            this.lbText.Name = "lbText";
            this.lbText.Size = new System.Drawing.Size(41, 12);
            this.lbText.TabIndex = 1;
            this.lbText.Text = "label1";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(347, 16);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 23);
            this.btnSubmit.TabIndex = 2;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // 不合格品处置单补充信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(473, 247);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.lbText);
            this.Controls.Add(this.txtMessage);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "不合格品处置单补充信息";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "不合格品处置单补充信息";
            this.Load += new System.EventHandler(this.不合格品处置单补充信息_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UniversalControlLibrary.TextBoxShow txtMessage;
        private System.Windows.Forms.Label lbText;
        private System.Windows.Forms.Button btnSubmit;
    }
}