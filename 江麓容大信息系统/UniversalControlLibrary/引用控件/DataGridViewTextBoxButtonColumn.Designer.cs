using System.Windows.Forms;
namespace UniversalControlLibrary
{
    partial class DataGridViewTextBoxButtonEditingControl
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;
 
        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
 
        #region 组件设计器生成的代码
 
        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btn = new System.Windows.Forms.Button();
            this.txtEdit = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            //
            // btn
            //
            this.btn.Dock = System.Windows.Forms.DockStyle.Right;
            this.btn.Location = new System.Drawing.Point(298, 0);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(23, 24);
            this.btn.TabIndex = 1;
            this.btn.Text = "…";
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            //
            // txtEdit
            //
            this.txtEdit.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtEdit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtEdit.Location = new System.Drawing.Point(0, 0);
            this.txtEdit.Multiline = false;
            this.txtEdit.Name = "txtEdit";
            this.txtEdit.Size = new System.Drawing.Size(298, 24);
            this.txtEdit.TabIndex = 2;
            this.txtEdit.Text = "";
            this.txtEdit.Leave += new System.EventHandler(this.txtEdit_Leave);
            this.txtEdit.TextChanged += new System.EventHandler(this.txtEdit_TextChanged);
            //
            // DataGridViewTextBoxButtonEditingControl
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtEdit);
            this.Controls.Add(this.btn);
            this.Name = "DataGridViewTextBoxButtonEditingControl";
            this.Size = new System.Drawing.Size(321, 24);
            this.ResumeLayout(false);
 
        }
 
        #endregion
 
        private System.Windows.Forms.Button btn;
        private RichTextBox txtEdit;
    }
}
