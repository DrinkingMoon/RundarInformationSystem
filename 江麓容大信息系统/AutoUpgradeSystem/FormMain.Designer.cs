namespace AutoUpgradeSystem
{
    partial class FormMain
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.lblMainPrompt = new System.Windows.Forms.Label();
            this.lblFindNewVersion = new System.Windows.Forms.Label();
            this.lblFileList = new System.Windows.Forms.Label();
            this.listViewFileList = new System.Windows.Forms.ListView();
            this.序号 = new System.Windows.Forms.ColumnHeader();
            this.更新状态 = new System.Windows.Forms.ColumnHeader();
            this.软件系统名称 = new System.Windows.Forms.ColumnHeader();
            this.文件名称 = new System.Windows.Forms.ColumnHeader();
            this.文件版本 = new System.Windows.Forms.ColumnHeader();
            this.文件大小 = new System.Windows.Forms.ColumnHeader();
            this.timerExit = new System.Windows.Forms.Timer(this.components);
            this.timerCheckDbServer = new System.Windows.Forms.Timer(this.components);
            this.lblWaitCursor = new System.Windows.Forms.Label();
            this.timerShowCursor = new System.Windows.Forms.Timer(this.components);
            this.timerShowPrompt = new System.Windows.Forms.Timer(this.components);
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // lblMainPrompt
            // 
            this.lblMainPrompt.AutoSize = true;
            this.lblMainPrompt.BackColor = System.Drawing.Color.Transparent;
            this.lblMainPrompt.Location = new System.Drawing.Point(12, 99);
            this.lblMainPrompt.Name = "lblMainPrompt";
            this.lblMainPrompt.Size = new System.Drawing.Size(504, 14);
            this.lblMainPrompt.TabIndex = 0;
            this.lblMainPrompt.Text = "请耐心等待，这个操作可能会耗费一些时间，正在连接服务器，检查系统更新...";
            // 
            // lblFindNewVersion
            // 
            this.lblFindNewVersion.AutoSize = true;
            this.lblFindNewVersion.BackColor = System.Drawing.Color.Transparent;
            this.lblFindNewVersion.Location = new System.Drawing.Point(12, 129);
            this.lblFindNewVersion.Name = "lblFindNewVersion";
            this.lblFindNewVersion.Size = new System.Drawing.Size(196, 14);
            this.lblFindNewVersion.TabIndex = 1;
            this.lblFindNewVersion.Text = "发现文件新版本，更新系统...";
            this.lblFindNewVersion.Visible = false;
            // 
            // lblFileList
            // 
            this.lblFileList.AutoSize = true;
            this.lblFileList.BackColor = System.Drawing.Color.Transparent;
            this.lblFileList.Location = new System.Drawing.Point(12, 157);
            this.lblFileList.Name = "lblFileList";
            this.lblFileList.Size = new System.Drawing.Size(91, 14);
            this.lblFileList.TabIndex = 2;
            this.lblFileList.Text = "更新文件列表";
            this.lblFileList.Visible = false;
            // 
            // listViewFileList
            // 
            this.listViewFileList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.序号,
            this.更新状态,
            this.软件系统名称,
            this.文件名称,
            this.文件版本,
            this.文件大小});
            this.listViewFileList.FullRowSelect = true;
            this.listViewFileList.Location = new System.Drawing.Point(12, 184);
            this.listViewFileList.Name = "listViewFileList";
            this.listViewFileList.Size = new System.Drawing.Size(635, 133);
            this.listViewFileList.TabIndex = 3;
            this.listViewFileList.UseCompatibleStateImageBehavior = false;
            this.listViewFileList.View = System.Windows.Forms.View.Details;
            this.listViewFileList.Visible = false;
            // 
            // 序号
            // 
            this.序号.Text = "序号";
            this.序号.Width = 50;
            // 
            // 更新状态
            // 
            this.更新状态.Text = "更新状态";
            this.更新状态.Width = 90;
            // 
            // 软件系统名称
            // 
            this.软件系统名称.Text = "软件系统名称";
            this.软件系统名称.Width = 120;
            // 
            // 文件名称
            // 
            this.文件名称.Text = "文件名称";
            this.文件名称.Width = 160;
            // 
            // 文件版本
            // 
            this.文件版本.Text = "文件版本";
            this.文件版本.Width = 80;
            // 
            // 文件大小
            // 
            this.文件大小.Text = "文件大小";
            this.文件大小.Width = 128;
            // 
            // timerExit
            // 
            this.timerExit.Interval = 1500;
            this.timerExit.Tick += new System.EventHandler(this.timerExit_Tick);
            // 
            // timerCheckDbServer
            // 
            this.timerCheckDbServer.Tick += new System.EventHandler(this.timerCheckDbServer_Tick);
            // 
            // lblWaitCursor
            // 
            this.lblWaitCursor.AutoSize = true;
            this.lblWaitCursor.BackColor = System.Drawing.Color.Transparent;
            this.lblWaitCursor.Font = new System.Drawing.Font("新宋体", 10.5F);
            this.lblWaitCursor.ForeColor = System.Drawing.Color.Red;
            this.lblWaitCursor.Location = new System.Drawing.Point(522, 99);
            this.lblWaitCursor.Name = "lblWaitCursor";
            this.lblWaitCursor.Size = new System.Drawing.Size(14, 14);
            this.lblWaitCursor.TabIndex = 4;
            this.lblWaitCursor.Text = "/";
            // 
            // timerShowCursor
            // 
            this.timerShowCursor.Enabled = true;
            this.timerShowCursor.Tick += new System.EventHandler(this.timerShowCursor_Tick);
            // 
            // timerShowPrompt
            // 
            this.timerShowPrompt.Interval = 1500;
            this.timerShowPrompt.Tick += new System.EventHandler(this.timerShowPrompt_Tick);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImage = global::AutoUpgradeSystem.Properties.Resources.RundarJPG;
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(634, 77);
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(658, 321);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblWaitCursor);
            this.Controls.Add(this.listViewFileList);
            this.Controls.Add(this.lblFileList);
            this.Controls.Add(this.lblFindNewVersion);
            this.Controls.Add(this.lblMainPrompt);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "检查系统更新...";
            this.Load += new System.EventHandler(this.FormMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblMainPrompt;
        private System.Windows.Forms.Label lblFindNewVersion;
        private System.Windows.Forms.Label lblFileList;
        private System.Windows.Forms.ListView listViewFileList;
        private System.Windows.Forms.ColumnHeader 序号;
        private System.Windows.Forms.ColumnHeader 更新状态;
        private System.Windows.Forms.ColumnHeader 软件系统名称;
        private System.Windows.Forms.ColumnHeader 文件名称;
        private System.Windows.Forms.ColumnHeader 文件版本;
        private System.Windows.Forms.Timer timerExit;
        private System.Windows.Forms.ColumnHeader 文件大小;
        private System.Windows.Forms.Timer timerCheckDbServer;
        private System.Windows.Forms.Label lblWaitCursor;
        private System.Windows.Forms.Timer timerShowCursor;
        private System.Windows.Forms.Timer timerShowPrompt;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

