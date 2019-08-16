namespace Form_Quality_QC
{
    partial class 不合格品隔离处置单
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
            System.ComponentModel.ComponentResourceManager resources =
                new System.ComponentModel.ComponentResourceManager(typeof(不合格品隔离处置单));
            this.btnBatchAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddMsg = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator_2 = new System.Windows.Forms.ToolStripSeparator();
            this.SuspendLayout();
            // 
            // 不合格品隔离处置单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1034, 435);
            this.Name = "不合格品隔离处置单";
            this.Text = "不合格品隔离处置单";
            this.ResumeLayout(false);

            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.btnBatchAdd, this.toolStripSeparator_1, this.btnAddMsg, this.toolStripSeparator_2 });

            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator_1.Name = "toolStripSeparator1";
            this.toolStripSeparator_1.Size = new System.Drawing.Size(6, 25);

            // 
            // btnBatchAdd
            // 
            this.btnBatchAdd.Image = global::UniversalControlLibrary.Properties.Resources.ReadMe;
            this.btnBatchAdd.Name = "btnBatchAdd";
            this.btnBatchAdd.Tag = "Add";
            this.btnBatchAdd.Text = "多批隔离(&B)";
            this.btnBatchAdd.Click += new System.EventHandler(btnBatchAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator_2.Name = "toolStripSeparator1";
            this.toolStripSeparator_2.Size = new System.Drawing.Size(6, 25);

            // 
            // btnBatchAdd
            // 
            this.btnAddMsg.Image = global::UniversalControlLibrary.Properties.Resources.ReadMe;
            this.btnAddMsg.Name = "btnBatchAdd";
            this.btnAddMsg.Tag = "View";
            this.btnAddMsg.Text = "补充信息(&B)";
            this.btnAddMsg.Click += new System.EventHandler(btnAddMsg_Click);

        }

        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_1;
        private System.Windows.Forms.ToolStripButton btnBatchAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator_2;
        private System.Windows.Forms.ToolStripButton btnAddMsg;

        #endregion

    }
}