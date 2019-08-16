namespace Expression
{
    partial class MES数据查询
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
            this.components = new System.ComponentModel.Container();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.数据信息 = new System.Windows.Forms.TabPage();
            this.零件信息 = new System.Windows.Forms.TabPage();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.刷新ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.筛选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示全部ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.返修信息 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.数据信息);
            this.tabControl1.Controls.Add(this.零件信息);
            this.tabControl1.Controls.Add(this.返修信息);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 729);
            this.tabControl1.TabIndex = 0;
            // 
            // 数据信息
            // 
            this.数据信息.Location = new System.Drawing.Point(4, 22);
            this.数据信息.Name = "数据信息";
            this.数据信息.Padding = new System.Windows.Forms.Padding(3);
            this.数据信息.Size = new System.Drawing.Size(1000, 703);
            this.数据信息.TabIndex = 3;
            this.数据信息.Text = "数据信息";
            this.数据信息.UseVisualStyleBackColor = true;
            // 
            // 零件信息
            // 
            this.零件信息.Location = new System.Drawing.Point(4, 22);
            this.零件信息.Name = "零件信息";
            this.零件信息.Padding = new System.Windows.Forms.Padding(3);
            this.零件信息.Size = new System.Drawing.Size(1000, 703);
            this.零件信息.TabIndex = 2;
            this.零件信息.Text = "零件信息";
            this.零件信息.UseVisualStyleBackColor = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.刷新ToolStripMenuItem,
            this.筛选ToolStripMenuItem,
            this.显示全部ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 70);
            // 
            // 刷新ToolStripMenuItem
            // 
            this.刷新ToolStripMenuItem.Name = "刷新ToolStripMenuItem";
            this.刷新ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.刷新ToolStripMenuItem.Text = "刷新";
            // 
            // 筛选ToolStripMenuItem
            // 
            this.筛选ToolStripMenuItem.Name = "筛选ToolStripMenuItem";
            this.筛选ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.筛选ToolStripMenuItem.Text = "筛选";
            // 
            // 显示全部ToolStripMenuItem
            // 
            this.显示全部ToolStripMenuItem.Name = "显示全部ToolStripMenuItem";
            this.显示全部ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.显示全部ToolStripMenuItem.Text = "显示全部";
            // 
            // 返修信息
            // 
            this.返修信息.Location = new System.Drawing.Point(4, 22);
            this.返修信息.Name = "返修信息";
            this.返修信息.Padding = new System.Windows.Forms.Padding(3);
            this.返修信息.Size = new System.Drawing.Size(1000, 703);
            this.返修信息.TabIndex = 4;
            this.返修信息.Text = "返修信息";
            this.返修信息.UseVisualStyleBackColor = true;
            // 
            // MES数据查询
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 729);
            this.Controls.Add(this.tabControl1);
            this.Name = "MES数据查询";
            this.Text = "MES数据查询";
            this.Load += new System.EventHandler(this.MES数据查询_Load);
            this.tabControl1.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 零件信息;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 刷新ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 筛选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示全部ToolStripMenuItem;
        private System.Windows.Forms.TabPage 数据信息;
        private System.Windows.Forms.TabPage 返修信息;
    }
}