using System;
namespace UniversalControlLibrary
{
    partial class CustomContextMenuStrip_Edit
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
            this.toolStripMenuItem_添加 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_删除 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem_导入 = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.toolStripMenuItem_导出 = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.SuspendLayout();
            // 
            // toolStripMenuItem_添加
            // 
            this.toolStripMenuItem_添加.Name = "toolStripMenuItem_添加";
            this.toolStripMenuItem_添加.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_添加.Text = "添加";
            this.toolStripMenuItem_添加.Click += new System.EventHandler(this.toolStripMenuItem_添加_Click);
            // 
            // toolStripMenuItem_删除
            // 
            this.toolStripMenuItem_删除.Name = "toolStripMenuItem_删除";
            this.toolStripMenuItem_删除.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_删除.Text = "删除";
            this.toolStripMenuItem_删除.Click += new System.EventHandler(this.toolStripMenuItem_删除_Click);
            // 
            // toolStripMenuItem_导入
            // 
            this.toolStripMenuItem_导入.Name = "toolStripMenuItem_导入";
            this.toolStripMenuItem_导入.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_导入.Text = "导入";
            this.toolStripMenuItem_导入.Click += new System.EventHandler(this.toolStripMenuItem_导入_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // toolStripMenuItem_导出
            // 
            this.toolStripMenuItem_导出.Name = "toolStripMenuItem_导出";
            this.toolStripMenuItem_导出.Size = new System.Drawing.Size(100, 22);
            this.toolStripMenuItem_导出.Text = "导出";
            this.toolStripMenuItem_导出.Click += new System.EventHandler(this.toolStripMenuItem_导出_Click);
            // 
            // CustomContextMenuStrip_Edit
            // 
            this.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem_添加,
            this.toolStripMenuItem_删除,
            this.toolStripMenuItem_导出,
            this.toolStripMenuItem_导入});
            this.Size = new System.Drawing.Size(101, 70);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_添加;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_删除;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_导入;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem_导出;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}
