namespace Expression
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.paneMain = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel2 = new System.Windows.Forms.Panel();
            this.faTabStrip1 = new CommControl.FATabStrip();
            this.panelTree = new System.Windows.Forms.Panel();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStripTab = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menuItemCloseFaTabItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.重新登录ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改密码ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.登录任务管理系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.设置数据服务器SToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.退出系统ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.功能树显示控制FToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.消息中心显示控制ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.静默待处理消息提示框ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.只显示有权限的功能树ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.帮助ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.关于ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuNotify = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.显示消息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.显示主界面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuItemExit = new System.Windows.Forms.ToolStripMenuItem();
            this.timerGlint = new System.Windows.Forms.Timer(this.components);
            this.lbShow = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.paneMain.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.faTabStrip1)).BeginInit();
            this.panelTree.SuspendLayout();
            this.contextMenuStripTab.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.contextMenuNotify.SuspendLayout();
            this.SuspendLayout();
            // 
            // paneMain
            // 
            this.paneMain.Controls.Add(this.splitter1);
            this.paneMain.Controls.Add(this.panel2);
            this.paneMain.Controls.Add(this.panelTree);
            this.paneMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.paneMain.Location = new System.Drawing.Point(0, 24);
            this.paneMain.Name = "paneMain";
            this.paneMain.Size = new System.Drawing.Size(1008, 592);
            this.paneMain.TabIndex = 0;
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(247, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 592);
            this.splitter1.TabIndex = 0;
            this.splitter1.TabStop = false;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.faTabStrip1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(247, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(761, 592);
            this.panel2.TabIndex = 0;
            // 
            // faTabStrip1
            // 
            this.faTabStrip1.AlwaysShowClose = false;
            this.faTabStrip1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.faTabStrip1.Font = new System.Drawing.Font("Tahoma", 8.25F);
            this.faTabStrip1.Location = new System.Drawing.Point(0, 0);
            this.faTabStrip1.Name = "faTabStrip1";
            this.faTabStrip1.Size = new System.Drawing.Size(761, 592);
            this.faTabStrip1.TabIndex = 0;
            this.faTabStrip1.Text = "faTabStrip1";
            this.faTabStrip1.MouseLeave += new System.EventHandler(this.faTabStrip1_MouseLeave);
            this.faTabStrip1.TabStripItemSelectionChanged += new CommControl.TabStripItemChangedHandler(this.faTabStrip1_TabStripItemSelectionChanged);
            this.faTabStrip1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.faTabStrip1_MouseClick);
            this.faTabStrip1.TabStripItemClosing += new CommControl.TabStripItemClosingHandler(this.faTabStrip1_TabStripItemClosing);
            // 
            // panelTree
            // 
            this.panelTree.Controls.Add(this.treeView1);
            this.panelTree.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelTree.Location = new System.Drawing.Point(0, 0);
            this.panelTree.Name = "panelTree";
            this.panelTree.Size = new System.Drawing.Size(247, 592);
            this.panelTree.TabIndex = 0;
            // 
            // treeView1
            // 
            this.treeView1.BackColor = System.Drawing.SystemColors.Window;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.treeView1.ImageIndex = 0;
            this.treeView1.ImageList = this.imageList;
            this.treeView1.ItemHeight = 20;
            this.treeView1.Location = new System.Drawing.Point(0, 0);
            this.treeView1.Name = "treeView1";
            this.treeView1.SelectedImageIndex = 0;
            this.treeView1.Size = new System.Drawing.Size(247, 592);
            this.treeView1.TabIndex = 0;
            this.treeView1.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.treeView1_NodeMouseDoubleClick);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            this.imageList.Images.SetKeyName(1, "File2.png");
            this.imageList.Images.SetKeyName(2, "null.ico");
            this.imageList.Images.SetKeyName(3, "Logo.ico");
            // 
            // contextMenuStripTab
            // 
            this.contextMenuStripTab.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuItemCloseFaTabItem});
            this.contextMenuStripTab.Name = "contextMenuStripTab";
            this.contextMenuStripTab.Size = new System.Drawing.Size(141, 26);
            // 
            // menuItemCloseFaTabItem
            // 
            this.menuItemCloseFaTabItem.Name = "menuItemCloseFaTabItem";
            this.menuItemCloseFaTabItem.Size = new System.Drawing.Size(140, 22);
            this.menuItemCloseFaTabItem.Text = "关闭(&Close)";
            this.menuItemCloseFaTabItem.Click += new System.EventHandler(this.menuItemCloseFaTabItem_Click);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.系统ToolStripMenuItem,
            this.显示ToolStripMenuItem,
            this.帮助ToolStripMenuItem,
            this.关于ToolStripMenuItem,
            this.导出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(1008, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 系统ToolStripMenuItem
            // 
            this.系统ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.重新登录ToolStripMenuItem,
            this.修改密码ToolStripMenuItem,
            this.toolStripSeparator1,
            this.登录任务管理系统ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.设置数据服务器SToolStripMenuItem,
            this.toolStripSeparator2,
            this.退出系统ToolStripMenuItem});
            this.系统ToolStripMenuItem.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.系统ToolStripMenuItem.Name = "系统ToolStripMenuItem";
            this.系统ToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.系统ToolStripMenuItem.Text = "【系统】(&S)";
            // 
            // 重新登录ToolStripMenuItem
            // 
            this.重新登录ToolStripMenuItem.Name = "重新登录ToolStripMenuItem";
            this.重新登录ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.重新登录ToolStripMenuItem.Text = "重新登录 (&R)";
            this.重新登录ToolStripMenuItem.Click += new System.EventHandler(this.重新登录ToolStripMenuItem_Click);
            // 
            // 修改密码ToolStripMenuItem
            // 
            this.修改密码ToolStripMenuItem.Name = "修改密码ToolStripMenuItem";
            this.修改密码ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.修改密码ToolStripMenuItem.Text = "修改密码";
            this.修改密码ToolStripMenuItem.Click += new System.EventHandler(this.修改密码ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(181, 6);
            // 
            // 登录任务管理系统ToolStripMenuItem
            // 
            this.登录任务管理系统ToolStripMenuItem.Name = "登录任务管理系统ToolStripMenuItem";
            this.登录任务管理系统ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.登录任务管理系统ToolStripMenuItem.Text = "登录任务管理系统 ★";
            this.登录任务管理系统ToolStripMenuItem.Click += new System.EventHandler(this.登录任务管理系统ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(181, 6);
            // 
            // 设置数据服务器SToolStripMenuItem
            // 
            this.设置数据服务器SToolStripMenuItem.Name = "设置数据服务器SToolStripMenuItem";
            this.设置数据服务器SToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.设置数据服务器SToolStripMenuItem.Text = "设置数据服务器 (&D)";
            this.设置数据服务器SToolStripMenuItem.Click += new System.EventHandler(this.设置数据服务器SToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(181, 6);
            // 
            // 退出系统ToolStripMenuItem
            // 
            this.退出系统ToolStripMenuItem.Name = "退出系统ToolStripMenuItem";
            this.退出系统ToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.退出系统ToolStripMenuItem.Text = "退出系统 (&E)";
            this.退出系统ToolStripMenuItem.Click += new System.EventHandler(this.退出系统ToolStripMenuItem_Click);
            // 
            // 显示ToolStripMenuItem
            // 
            this.显示ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.功能树显示控制FToolStripMenuItem,
            this.消息中心显示控制ToolStripMenuItem,
            this.静默待处理消息提示框ToolStripMenuItem,
            this.只显示有权限的功能树ToolStripMenuItem});
            this.显示ToolStripMenuItem.Name = "显示ToolStripMenuItem";
            this.显示ToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.显示ToolStripMenuItem.Text = "【显示】(&V)";
            // 
            // 功能树显示控制FToolStripMenuItem
            // 
            this.功能树显示控制FToolStripMenuItem.Checked = true;
            this.功能树显示控制FToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.功能树显示控制FToolStripMenuItem.Name = "功能树显示控制FToolStripMenuItem";
            this.功能树显示控制FToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F4;
            this.功能树显示控制FToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.功能树显示控制FToolStripMenuItem.Text = "功能树显示控制";
            this.功能树显示控制FToolStripMenuItem.Click += new System.EventHandler(this.功能树显示控制FToolStripMenuItem_Click);
            // 
            // 消息中心显示控制ToolStripMenuItem
            // 
            this.消息中心显示控制ToolStripMenuItem.Checked = true;
            this.消息中心显示控制ToolStripMenuItem.CheckOnClick = true;
            this.消息中心显示控制ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.消息中心显示控制ToolStripMenuItem.Name = "消息中心显示控制ToolStripMenuItem";
            this.消息中心显示控制ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.消息中心显示控制ToolStripMenuItem.Text = "消息中心显示控制";
            this.消息中心显示控制ToolStripMenuItem.Click += new System.EventHandler(this.消息中心显示控制ToolStripMenuItem_Click);
            // 
            // 静默待处理消息提示框ToolStripMenuItem
            // 
            this.静默待处理消息提示框ToolStripMenuItem.Checked = true;
            this.静默待处理消息提示框ToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.静默待处理消息提示框ToolStripMenuItem.Name = "静默待处理消息提示框ToolStripMenuItem";
            this.静默待处理消息提示框ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.静默待处理消息提示框ToolStripMenuItem.Text = "静默待处理消息提示框";
            this.静默待处理消息提示框ToolStripMenuItem.Click += new System.EventHandler(this.静默待处理消息提示框ToolStripMenuItem_Click);
            // 
            // 只显示有权限的功能树ToolStripMenuItem
            // 
            this.只显示有权限的功能树ToolStripMenuItem.Name = "只显示有权限的功能树ToolStripMenuItem";
            this.只显示有权限的功能树ToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.只显示有权限的功能树ToolStripMenuItem.Text = "只显示有权限的功能树";
            this.只显示有权限的功能树ToolStripMenuItem.Click += new System.EventHandler(this.只显示有权限的功能树ToolStripMenuItem_Click);
            // 
            // 帮助ToolStripMenuItem
            // 
            this.帮助ToolStripMenuItem.Name = "帮助ToolStripMenuItem";
            this.帮助ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.F1;
            this.帮助ToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.帮助ToolStripMenuItem.Text = "【帮助】(&H)";
            this.帮助ToolStripMenuItem.Click += new System.EventHandler(this.帮助ToolStripMenuItem_Click);
            // 
            // 关于ToolStripMenuItem
            // 
            this.关于ToolStripMenuItem.Name = "关于ToolStripMenuItem";
            this.关于ToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.关于ToolStripMenuItem.Text = "【关于】(&A)";
            this.关于ToolStripMenuItem.Click += new System.EventHandler(this.关于ToolStripMenuItem_Click);
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.导出ToolStripMenuItem.Text = "【导出】(&O)";
            this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuNotify;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "湖南容大信息化系统";
            this.notifyIcon1.Visible = true;
            this.notifyIcon1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseMove);
            this.notifyIcon1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDown);
            this.notifyIcon1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon1_MouseDoubleClick);
            // 
            // contextMenuNotify
            // 
            this.contextMenuNotify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.显示消息ToolStripMenuItem,
            this.显示主界面ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.menuItemExit});
            this.contextMenuNotify.Name = "contextMenuStripTab";
            this.contextMenuNotify.Size = new System.Drawing.Size(161, 76);
            // 
            // 显示消息ToolStripMenuItem
            // 
            this.显示消息ToolStripMenuItem.Name = "显示消息ToolStripMenuItem";
            this.显示消息ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.显示消息ToolStripMenuItem.Text = "显示消息提示窗";
            this.显示消息ToolStripMenuItem.Click += new System.EventHandler(this.显示消息窗体ToolStripMenuItem_Click);
            // 
            // 显示主界面ToolStripMenuItem
            // 
            this.显示主界面ToolStripMenuItem.Name = "显示主界面ToolStripMenuItem";
            this.显示主界面ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.显示主界面ToolStripMenuItem.Text = "显示主界面";
            this.显示主界面ToolStripMenuItem.Click += new System.EventHandler(this.显示主界面ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(157, 6);
            // 
            // menuItemExit
            // 
            this.menuItemExit.Name = "menuItemExit";
            this.menuItemExit.Size = new System.Drawing.Size(160, 22);
            this.menuItemExit.Text = "退出系统(&Exit)";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // timerGlint
            // 
            this.timerGlint.Interval = 300000;
            this.timerGlint.Tick += new System.EventHandler(this.timerGlint_Tick);
            // 
            // lbShow
            // 
            this.lbShow.AutoSize = true;
            this.lbShow.BackColor = System.Drawing.Color.Transparent;
            this.lbShow.Font = new System.Drawing.Font("黑体", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbShow.ForeColor = System.Drawing.Color.Black;
            this.lbShow.Location = new System.Drawing.Point(604, 4);
            this.lbShow.Name = "lbShow";
            this.lbShow.Size = new System.Drawing.Size(0, 16);
            this.lbShow.TabIndex = 7;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.Multiselect = true;
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 616);
            this.Controls.Add(this.lbShow);
            this.Controls.Add(this.paneMain);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "容大信息系统";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FormMain_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormMain_FormClosing);
            this.paneMain.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.faTabStrip1)).EndInit();
            this.panelTree.ResumeLayout(false);
            this.contextMenuStripTab.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.contextMenuNotify.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel paneMain;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重新登录ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem 退出系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 帮助ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关于ToolStripMenuItem;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Panel panel2;
        private CommControl.FATabStrip faTabStrip1;
        private System.Windows.Forms.Panel panelTree;
        private System.Windows.Forms.ToolStripMenuItem 修改密码ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 功能树显示控制FToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置数据服务器SToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem 消息中心显示控制ToolStripMenuItem;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripTab;
        private System.Windows.Forms.ToolStripMenuItem menuItemCloseFaTabItem;
        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuNotify;
        private System.Windows.Forms.ToolStripMenuItem menuItemExit;
        private System.Windows.Forms.Timer timerGlint;
        private System.Windows.Forms.ToolStripMenuItem 显示消息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 显示主界面ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 静默待处理消息提示框ToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Label lbShow;
        private System.Windows.Forms.ToolStripMenuItem 登录任务管理系统ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 只显示有权限的功能树ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}