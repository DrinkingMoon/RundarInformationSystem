using UniversalControlLibrary;
namespace Expression
{
    partial class 工装台帐
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.查看ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.添加ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.剪切ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.粘贴ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer2 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkIsShowUsing = new System.Windows.Forms.CheckBox();
            this.chkIsShowTree = new System.Windows.Forms.CheckBox();
            this.chkIsShowFinalAssembly = new System.Windows.Forms.CheckBox();
            this.btnRefrsh = new System.Windows.Forms.Button();
            this.chkIsShowStock = new System.Windows.Forms.CheckBox();
            this.chkIsShowInStock = new System.Windows.Forms.CheckBox();
            this.chkIsShowDispensingScrapInfo = new System.Windows.Forms.CheckBox();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1149, 49);
            this.panel3.TabIndex = 54;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(501, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(147, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "工装台帐表";
            // 
            // treeView1
            // 
            this.treeView1.AllowDrop = true;
            this.treeView1.ContextMenuStrip = this.contextMenuStrip1;
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Left;
            this.treeView1.Location = new System.Drawing.Point(0, 49);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(231, 685);
            this.treeView1.TabIndex = 55;
            this.treeView1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tvFormel_DragDrop);
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            this.treeView1.DragEnter += new System.Windows.Forms.DragEventHandler(this.tvFormel_DragEnter);
            this.treeView1.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.treeView1_BeforeSelect);
            this.treeView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.tvFormel_ItemDrag);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看ToolStripMenuItem,
            this.添加ToolStripMenuItem,
            this.剪切ToolStripMenuItem,
            this.粘贴ToolStripMenuItem,
            this.删除ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 114);
            // 
            // 查看ToolStripMenuItem
            // 
            this.查看ToolStripMenuItem.Name = "查看ToolStripMenuItem";
            this.查看ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.查看ToolStripMenuItem.Tag = "";
            this.查看ToolStripMenuItem.Text = "查看";
            this.查看ToolStripMenuItem.Click += new System.EventHandler(this.查看ToolStripMenuItem_Click);
            // 
            // 添加ToolStripMenuItem
            // 
            this.添加ToolStripMenuItem.Name = "添加ToolStripMenuItem";
            this.添加ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.添加ToolStripMenuItem.Text = "添加";
            this.添加ToolStripMenuItem.Click += new System.EventHandler(this.添加ToolStripMenuItem_Click);
            // 
            // 剪切ToolStripMenuItem
            // 
            this.剪切ToolStripMenuItem.Name = "剪切ToolStripMenuItem";
            this.剪切ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.剪切ToolStripMenuItem.Text = "剪切";
            this.剪切ToolStripMenuItem.Click += new System.EventHandler(this.剪切ToolStripMenuItem_Click);
            // 
            // 粘贴ToolStripMenuItem
            // 
            this.粘贴ToolStripMenuItem.Name = "粘贴ToolStripMenuItem";
            this.粘贴ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.粘贴ToolStripMenuItem.Text = "粘贴";
            this.粘贴ToolStripMenuItem.Click += new System.EventHandler(this.粘贴ToolStripMenuItem_Click);
            // 
            // 删除ToolStripMenuItem
            // 
            this.删除ToolStripMenuItem.Name = "删除ToolStripMenuItem";
            this.删除ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.删除ToolStripMenuItem.Text = "删除";
            this.删除ToolStripMenuItem.Click += new System.EventHandler(this.删除ToolStripMenuItem_Click);
            // 
            // splitter1
            // 
            this.splitter1.Location = new System.Drawing.Point(231, 49);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 685);
            this.splitter1.TabIndex = 58;
            this.splitter1.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(231, 49);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(918, 685);
            this.panel1.TabIndex = 59;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 49);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(918, 636);
            this.groupBox2.TabIndex = 62;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "信息显示";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dataGridView2);
            this.panel4.Controls.Add(this.userControlDataLocalizer2);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(391, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(524, 614);
            this.panel4.TabIndex = 5;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 32);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(524, 582);
            this.dataGridView2.TabIndex = 7;
            this.dataGridView2.DoubleClick += new System.EventHandler(this.dataGridView2_DoubleClick);
            // 
            // userControlDataLocalizer2
            // 
            this.userControlDataLocalizer2.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer2.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer2.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer2.Name = "userControlDataLocalizer2";
            this.userControlDataLocalizer2.OnlyLocalize = false;
            this.userControlDataLocalizer2.Size = new System.Drawing.Size(524, 32);
            this.userControlDataLocalizer2.StartIndex = 0;
            this.userControlDataLocalizer2.TabIndex = 6;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel2.Location = new System.Drawing.Point(3, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(388, 614);
            this.panel2.TabIndex = 4;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(388, 582);
            this.dataGridView1.TabIndex = 6;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(388, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 5;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkIsShowUsing);
            this.groupBox1.Controls.Add(this.chkIsShowTree);
            this.groupBox1.Controls.Add(this.chkIsShowFinalAssembly);
            this.groupBox1.Controls.Add(this.btnRefrsh);
            this.groupBox1.Controls.Add(this.chkIsShowStock);
            this.groupBox1.Controls.Add(this.chkIsShowInStock);
            this.groupBox1.Controls.Add(this.chkIsShowDispensingScrapInfo);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(918, 49);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息过滤";
            // 
            // chkIsShowUsing
            // 
            this.chkIsShowUsing.AutoSize = true;
            this.chkIsShowUsing.Checked = true;
            this.chkIsShowUsing.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsShowUsing.Location = new System.Drawing.Point(427, 22);
            this.chkIsShowUsing.Name = "chkIsShowUsing";
            this.chkIsShowUsing.Size = new System.Drawing.Size(124, 18);
            this.chkIsShowUsing.TabIndex = 45;
            this.chkIsShowUsing.Text = "仅显示在用物品";
            this.chkIsShowUsing.UseVisualStyleBackColor = true;
            this.chkIsShowUsing.CheckedChanged += new System.EventHandler(this.chkIsShowUsing_CheckedChanged);
            // 
            // chkIsShowTree
            // 
            this.chkIsShowTree.AutoSize = true;
            this.chkIsShowTree.Location = new System.Drawing.Point(29, 22);
            this.chkIsShowTree.Name = "chkIsShowTree";
            this.chkIsShowTree.Size = new System.Drawing.Size(110, 18);
            this.chkIsShowTree.TabIndex = 44;
            this.chkIsShowTree.Text = "显示树形结构";
            this.chkIsShowTree.UseVisualStyleBackColor = true;
            this.chkIsShowTree.CheckedChanged += new System.EventHandler(this.chkIsShowTree_CheckedChanged);
            // 
            // chkIsShowFinalAssembly
            // 
            this.chkIsShowFinalAssembly.AutoSize = true;
            this.chkIsShowFinalAssembly.Checked = true;
            this.chkIsShowFinalAssembly.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsShowFinalAssembly.Location = new System.Drawing.Point(285, 22);
            this.chkIsShowFinalAssembly.Name = "chkIsShowFinalAssembly";
            this.chkIsShowFinalAssembly.Size = new System.Drawing.Size(124, 18);
            this.chkIsShowFinalAssembly.TabIndex = 43;
            this.chkIsShowFinalAssembly.Text = "仅显示总装物品";
            this.chkIsShowFinalAssembly.UseVisualStyleBackColor = true;
            this.chkIsShowFinalAssembly.CheckedChanged += new System.EventHandler(this.chkIsShowFinalAssembly_CheckedChanged);
            // 
            // btnRefrsh
            // 
            this.btnRefrsh.Location = new System.Drawing.Point(878, 20);
            this.btnRefrsh.Name = "btnRefrsh";
            this.btnRefrsh.Size = new System.Drawing.Size(75, 23);
            this.btnRefrsh.TabIndex = 42;
            this.btnRefrsh.Text = "刷新";
            this.btnRefrsh.UseVisualStyleBackColor = true;
            this.btnRefrsh.Click += new System.EventHandler(this.btnRefrsh_Click);
            // 
            // chkIsShowStock
            // 
            this.chkIsShowStock.AutoSize = true;
            this.chkIsShowStock.Location = new System.Drawing.Point(157, 22);
            this.chkIsShowStock.Name = "chkIsShowStock";
            this.chkIsShowStock.Size = new System.Drawing.Size(110, 18);
            this.chkIsShowStock.TabIndex = 41;
            this.chkIsShowStock.Text = "显示库存信息";
            this.chkIsShowStock.UseVisualStyleBackColor = true;
            this.chkIsShowStock.CheckedChanged += new System.EventHandler(this.chkIsShowStock_CheckedChanged);
            // 
            // chkIsShowInStock
            // 
            this.chkIsShowInStock.AutoSize = true;
            this.chkIsShowInStock.Location = new System.Drawing.Point(569, 22);
            this.chkIsShowInStock.Name = "chkIsShowInStock";
            this.chkIsShowInStock.Size = new System.Drawing.Size(124, 18);
            this.chkIsShowInStock.TabIndex = 40;
            this.chkIsShowInStock.Text = "仅显示在库物品";
            this.chkIsShowInStock.UseVisualStyleBackColor = true;
            this.chkIsShowInStock.CheckedChanged += new System.EventHandler(this.chkIsShowInStock_CheckedChanged);
            // 
            // chkIsShowDispensingScrapInfo
            // 
            this.chkIsShowDispensingScrapInfo.AutoSize = true;
            this.chkIsShowDispensingScrapInfo.Location = new System.Drawing.Point(711, 22);
            this.chkIsShowDispensingScrapInfo.Name = "chkIsShowDispensingScrapInfo";
            this.chkIsShowDispensingScrapInfo.Size = new System.Drawing.Size(152, 18);
            this.chkIsShowDispensingScrapInfo.TabIndex = 39;
            this.chkIsShowDispensingScrapInfo.Text = "显示分装的报废记录";
            this.chkIsShowDispensingScrapInfo.UseVisualStyleBackColor = true;
            this.chkIsShowDispensingScrapInfo.CheckedChanged += new System.EventHandler(this.chkIsShowDispensingScrapInfo_CheckedChanged);
            // 
            // 工装台帐
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 734);
            this.Controls.Add(this.splitter1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.treeView1);
            this.Controls.Add(this.panel3);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "工装台帐";
            this.Load += new System.EventHandler(this.工装台帐_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 查看ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 添加ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除ToolStripMenuItem;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.ToolStripMenuItem 剪切ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 粘贴ToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkIsShowStock;
        private System.Windows.Forms.CheckBox chkIsShowInStock;
        private System.Windows.Forms.CheckBox chkIsShowDispensingScrapInfo;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel4;
        private UserControlDataLocalizer userControlDataLocalizer2;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button btnRefrsh;
        private System.Windows.Forms.CheckBox chkIsShowFinalAssembly;
        private System.Windows.Forms.CheckBox chkIsShowTree;
        private System.Windows.Forms.CheckBox chkIsShowUsing;


    }
}
