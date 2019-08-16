namespace Expression
{
    partial class UserControlMessageCenter
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("单据");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("任务管理");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("待处理", new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2});
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("任务", new System.Windows.Forms.TreeNode[] {
            treeNode3});
            System.Windows.Forms.TreeNode treeNode5 = new System.Windows.Forms.TreeNode("会议提醒");
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("预警消息");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("日常事务");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("单据处理后知会");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("通知", new System.Windows.Forms.TreeNode[] {
            treeNode5,
            treeNode6,
            treeNode7,
            treeNode8});
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlMessageCenter));
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtContent = new System.Windows.Forms.TextBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.lblTitle = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panelClient = new System.Windows.Forms.Panel();
            this.dataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.定位单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panelTop = new System.Windows.Forms.Panel();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblRecordRow = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.lblSelectedNode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panelMiddle = new System.Windows.Forms.Panel();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.treeView = new System.Windows.Forms.TreeView();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.btnCreateNewNotice = new System.Windows.Forms.ToolStripButton();
            this.btnModifyNotice = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorUpdate = new System.Windows.Forms.ToolStripSeparator();
            this.btnAlreadyRead = new System.Windows.Forms.ToolStripButton();
            this.toolTipRecord = new System.Windows.Forms.ToolTip(this.components);
            this.panelTool = new System.Windows.Forms.Panel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.会议管理ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.通知类消息操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建通知ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.修改通知ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除通知ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.批示已阅ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批示全部ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新数据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemPrompt = new System.Windows.Forms.ToolStripMenuItem();
            this.timerRefreshData = new System.Windows.Forms.Timer(this.components);
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panelClient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.panelLeft.SuspendLayout();
            this.panelTool.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtContent);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 348);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(658, 95);
            this.panel2.TabIndex = 27;
            // 
            // txtContent
            // 
            this.txtContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtContent.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.txtContent.Location = new System.Drawing.Point(2, 0);
            this.txtContent.Multiline = true;
            this.txtContent.Name = "txtContent";
            this.txtContent.ReadOnly = true;
            this.txtContent.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtContent.Size = new System.Drawing.Size(654, 91);
            this.txtContent.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.lblTitle);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel3.Location = new System.Drawing.Point(0, 314);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(658, 34);
            this.panel3.TabIndex = 29;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(5, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "消息内容：";
            // 
            // lblTitle
            // 
            this.lblTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lblTitle.Location = new System.Drawing.Point(75, 2);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(583, 29);
            this.lblTitle.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 2);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "标题：";
            // 
            // panelClient
            // 
            this.panelClient.BackColor = System.Drawing.Color.Transparent;
            this.panelClient.Controls.Add(this.dataGridView1);
            this.panelClient.Controls.Add(this.userControlDataLocalizer1);
            this.panelClient.Controls.Add(this.panelTop);
            this.panelClient.Controls.Add(this.panel3);
            this.panelClient.Controls.Add(this.panel2);
            this.panelClient.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelClient.Location = new System.Drawing.Point(186, 0);
            this.panelClient.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelClient.Name = "panelClient";
            this.panelClient.Size = new System.Drawing.Size(658, 443);
            this.panelClient.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            //this.dataGridView1.AutoCreateFilters = true;
            //this.dataGridView1.BaseFilter = "";
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 57);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataGridView1.MultiSelect = false;
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowHeadersWidth = 40;
            this.dataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.ShowCellErrors = false;
            this.dataGridView1.ShowCellToolTips = false;
            this.dataGridView1.ShowEditingIcon = false;
            this.dataGridView1.ShowRowErrors = false;
            this.dataGridView1.Size = new System.Drawing.Size(658, 257);
            this.dataGridView1.TabIndex = 30;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.定位单据ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(164, 26);
            // 
            // 定位单据ToolStripMenuItem
            // 
            this.定位单据ToolStripMenuItem.Name = "定位单据ToolStripMenuItem";
            this.定位单据ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.P)));
            this.定位单据ToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.定位单据ToolStripMenuItem.Text = "定位单据";
            this.定位单据ToolStripMenuItem.Click += new System.EventHandler(this.定位单据ToolStripMenuItem_Click);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 30);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(658, 27);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 32;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.lblAmount);
            this.panelTop.Controls.Add(this.label8);
            this.panelTop.Controls.Add(this.lblRecordRow);
            this.panelTop.Controls.Add(this.label4);
            this.panelTop.Controls.Add(this.lblSelectedNode);
            this.panelTop.Controls.Add(this.label2);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(658, 30);
            this.panelTop.TabIndex = 31;
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(310, 9);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(11, 12);
            this.lblAmount.TabIndex = 225;
            this.lblAmount.Text = "0";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(228, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 224;
            this.label8.Text = "总记录数：";
            // 
            // lblRecordRow
            // 
            this.lblRecordRow.AutoSize = true;
            this.lblRecordRow.Location = new System.Drawing.Point(428, 9);
            this.lblRecordRow.Name = "lblRecordRow";
            this.lblRecordRow.Size = new System.Drawing.Size(11, 12);
            this.lblRecordRow.TabIndex = 223;
            this.lblRecordRow.Text = "0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(342, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 12);
            this.label4.TabIndex = 222;
            this.label4.Text = "当前记录行：";
            // 
            // lblSelectedNode
            // 
            this.lblSelectedNode.AutoSize = true;
            this.lblSelectedNode.Location = new System.Drawing.Point(91, 9);
            this.lblSelectedNode.Name = "lblSelectedNode";
            this.lblSelectedNode.Size = new System.Drawing.Size(41, 12);
            this.lblSelectedNode.TabIndex = 1;
            this.lblSelectedNode.Text = "label3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "当前选择事项：";
            // 
            // panelMiddle
            // 
            this.panelMiddle.BackColor = System.Drawing.Color.Transparent;
            this.panelMiddle.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelMiddle.Location = new System.Drawing.Point(183, 0);
            this.panelMiddle.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMiddle.Name = "panelMiddle";
            this.panelMiddle.Size = new System.Drawing.Size(3, 443);
            this.panelMiddle.TabIndex = 39;
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelClient);
            this.panelMain.Controls.Add(this.panelMiddle);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 25);
            this.panelMain.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(844, 443);
            this.panelMain.TabIndex = 35;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Controls.Add(this.treeView);
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(183, 443);
            this.panelLeft.TabIndex = 38;
            // 
            // treeView
            // 
            this.treeView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView.ImageIndex = 0;
            this.treeView.ImageList = this.imageList;
            this.treeView.Indent = 20;
            this.treeView.ItemHeight = 22;
            this.treeView.Location = new System.Drawing.Point(0, 0);
            this.treeView.Name = "treeView";
            treeNode1.Name = "任务_待处理_单据";
            treeNode1.Text = "单据";
            treeNode2.Name = "任务_待处理_任务管理";
            treeNode2.Text = "任务管理";
            treeNode3.Name = "任务_待处理";
            treeNode3.Text = "待处理";
            treeNode4.Name = "任务";
            treeNode4.Text = "任务";
            treeNode5.ForeColor = System.Drawing.Color.Blue;
            treeNode5.Name = "通知_会议提醒";
            treeNode5.Text = "会议提醒";
            treeNode6.ForeColor = System.Drawing.Color.Red;
            treeNode6.Name = "通知_预警消息";
            treeNode6.Text = "预警消息";
            treeNode7.Name = "通知_日常事务";
            treeNode7.Text = "日常事务";
            treeNode8.Name = "通知_单据处理后知会";
            treeNode8.Text = "单据处理后知会";
            treeNode9.Name = "通知";
            treeNode9.Text = "通知";
            this.treeView.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode4,
            treeNode9});
            this.treeView.SelectedImageIndex = 0;
            this.treeView.Size = new System.Drawing.Size(183, 443);
            this.treeView.TabIndex = 0;
            this.treeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView_AfterSelect);
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "Folder.png");
            // 
            // btnCreateNewNotice
            // 
            this.btnCreateNewNotice.Image = ((System.Drawing.Image)(resources.GetObject("btnCreateNewNotice.Image")));
            this.btnCreateNewNotice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnCreateNewNotice.Name = "btnCreateNewNotice";
            this.btnCreateNewNotice.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnCreateNewNotice.Size = new System.Drawing.Size(73, 22);
            this.btnCreateNewNotice.Text = "新建通知";
            this.btnCreateNewNotice.Click += new System.EventHandler(this.btnCreateNewNotice_Click);
            // 
            // btnModifyNotice
            // 
            this.btnModifyNotice.Image = ((System.Drawing.Image)(resources.GetObject("btnModifyNotice.Image")));
            this.btnModifyNotice.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModifyNotice.Name = "btnModifyNotice";
            this.btnModifyNotice.Size = new System.Drawing.Size(73, 22);
            this.btnModifyNotice.Text = "修改通知";
            this.btnModifyNotice.Click += new System.EventHandler(this.btnModifyNotice_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(49, 22);
            this.btnRefresh.Text = "刷新";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(49, 22);
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparatorUpdate
            // 
            this.toolStripSeparatorUpdate.Name = "toolStripSeparatorUpdate";
            this.toolStripSeparatorUpdate.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAlreadyRead
            // 
            this.btnAlreadyRead.Image = ((System.Drawing.Image)(resources.GetObject("btnAlreadyRead.Image")));
            this.btnAlreadyRead.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAlreadyRead.Name = "btnAlreadyRead";
            this.btnAlreadyRead.Size = new System.Drawing.Size(73, 22);
            this.btnAlreadyRead.Text = "批示已阅";
            this.btnAlreadyRead.Click += new System.EventHandler(this.btnReadNotice_Click);
            // 
            // toolTipRecord
            // 
            this.toolTipRecord.AutoPopDelay = 3000;
            this.toolTipRecord.InitialDelay = 500;
            this.toolTipRecord.ReshowDelay = 100;
            this.toolTipRecord.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTipRecord.ToolTipTitle = "信息提示：";
            // 
            // panelTool
            // 
            this.panelTool.Controls.Add(this.menuStrip1);
            this.panelTool.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTool.Location = new System.Drawing.Point(0, 0);
            this.panelTool.Name = "panelTool";
            this.panelTool.Size = new System.Drawing.Size(844, 25);
            this.panelTool.TabIndex = 2;
            // 
            // menuStrip1
            // 
            this.menuStrip1.AutoSize = false;
            this.menuStrip1.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip1.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.会议管理ToolStripMenuItem,
            this.通知类消息操作ToolStripMenuItem,
            this.刷新数据ToolStripMenuItem,
            this.menuItemPrompt});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(5, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(844, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 会议管理ToolStripMenuItem
            // 
            this.会议管理ToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("会议管理ToolStripMenuItem.Image")));
            this.会议管理ToolStripMenuItem.Name = "会议管理ToolStripMenuItem";
            this.会议管理ToolStripMenuItem.Size = new System.Drawing.Size(117, 20);
            this.会议管理ToolStripMenuItem.Text = "会议管理(&M)...";
            this.会议管理ToolStripMenuItem.Click += new System.EventHandler(this.会议管理ToolStripMenuItem_Click);
            // 
            // 通知类消息操作ToolStripMenuItem
            // 
            this.通知类消息操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建通知ToolStripMenuItem,
            this.修改通知ToolStripMenuItem,
            this.删除通知ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.批示已阅ToolStripMenuItem,
            this.批示全部ToolStripMenuItem});
            this.通知类消息操作ToolStripMenuItem.Name = "通知类消息操作ToolStripMenuItem";
            this.通知类消息操作ToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.通知类消息操作ToolStripMenuItem.Text = "通知类消息操作";
            // 
            // 新建通知ToolStripMenuItem
            // 
            this.新建通知ToolStripMenuItem.Name = "新建通知ToolStripMenuItem";
            this.新建通知ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.新建通知ToolStripMenuItem.Text = "新建通知";
            this.新建通知ToolStripMenuItem.Click += new System.EventHandler(this.btnCreateNewNotice_Click);
            // 
            // 修改通知ToolStripMenuItem
            // 
            this.修改通知ToolStripMenuItem.Name = "修改通知ToolStripMenuItem";
            this.修改通知ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.修改通知ToolStripMenuItem.Text = "修改通知";
            this.修改通知ToolStripMenuItem.Click += new System.EventHandler(this.btnModifyNotice_Click);
            // 
            // 删除通知ToolStripMenuItem
            // 
            this.删除通知ToolStripMenuItem.Name = "删除通知ToolStripMenuItem";
            this.删除通知ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.删除通知ToolStripMenuItem.Text = "删除通知";
            this.删除通知ToolStripMenuItem.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(115, 6);
            // 
            // 批示已阅ToolStripMenuItem
            // 
            this.批示已阅ToolStripMenuItem.Name = "批示已阅ToolStripMenuItem";
            this.批示已阅ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.批示已阅ToolStripMenuItem.Text = "批示已阅";
            this.批示已阅ToolStripMenuItem.ToolTipText = "可以选择多行通知类消息进行批量批示";
            this.批示已阅ToolStripMenuItem.Click += new System.EventHandler(this.btnReadNotice_Click);
            // 
            // 批示全部ToolStripMenuItem
            // 
            this.批示全部ToolStripMenuItem.Name = "批示全部ToolStripMenuItem";
            this.批示全部ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.批示全部ToolStripMenuItem.Text = "批示全部";
            this.批示全部ToolStripMenuItem.Click += new System.EventHandler(this.批示全部ToolStripMenuItem_Click);
            // 
            // 刷新数据ToolStripMenuItem
            // 
            this.刷新数据ToolStripMenuItem.Name = "刷新数据ToolStripMenuItem";
            this.刷新数据ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.刷新数据ToolStripMenuItem.Text = "刷新数据";
            this.刷新数据ToolStripMenuItem.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // menuItemPrompt
            // 
            this.menuItemPrompt.ForeColor = System.Drawing.Color.Red;
            this.menuItemPrompt.Name = "menuItemPrompt";
            this.menuItemPrompt.Size = new System.Drawing.Size(233, 20);
            this.menuItemPrompt.Text = "提示：双击单据类消息可直接定位到单据";
            // 
            // timerRefreshData
            // 
            this.timerRefreshData.Enabled = true;
            this.timerRefreshData.Interval = 300000;
            this.timerRefreshData.Tick += new System.EventHandler(this.timerRefreshData_Tick);
            // 
            // UserControlMessageCenter
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(844, 468);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.panelTool);
            this.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UserControlMessageCenter";
            this.Load += new System.EventHandler(this.UserControlMessageCenter_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panelClient.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelLeft.ResumeLayout(false);
            this.panelTool.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panelClient;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panelMiddle;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.ToolStripButton btnCreateNewNotice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnModifyNotice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorUpdate;
        private System.Windows.Forms.TreeView treeView;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAlreadyRead;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtContent;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label lblSelectedNode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolTip toolTipRecord;
        private System.Windows.Forms.Panel panelTool;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 通知类消息操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建通知ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 修改通知ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除通知ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 批示已阅ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 刷新数据ToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 定位单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem menuItemPrompt;
        private System.Windows.Forms.Timer timerRefreshData;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblRecordRow;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem 批示全部ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 会议管理ToolStripMenuItem;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
    }
}
