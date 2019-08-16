using UniversalControlLibrary;
namespace Expression
{
    partial class 打印整台份返修箱条码
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(打印整台份返修箱条码));
            this.panelTop = new System.Windows.Forms.Panel();
            this.m_dataLocalizer = new UniversalControlLibrary.UserControlDataLocalizer();
            this.label6 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.splitter1 = new System.Windows.Forms.Splitter();
            this.panel4 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.序号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零部件编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零部件名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供货单位 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.位置 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.只打印选择行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除选择行记录仅本次打印有效ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除选择行记录永远有效ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.移除选择记录供应商ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移除所有选择记录供应商永久有效ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
            this.向上移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.向下移动ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动到指定行ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动到第一个ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.移动到最后ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbPurpose = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnPrint = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtEdition = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.txtGoodsName = new UniversalControlLibrary.TextBoxShow();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelTop.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panelTop.Controls.Add(this.m_dataLocalizer);
            this.panelTop.Controls.Add(this.label6);
            this.panelTop.Controls.Add(this.labelTitle);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(798, 85);
            this.panelTop.TabIndex = 58;
            // 
            // m_dataLocalizer
            // 
            this.m_dataLocalizer.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_dataLocalizer.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.m_dataLocalizer.Location = new System.Drawing.Point(0, 62);
            this.m_dataLocalizer.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.m_dataLocalizer.Name = "m_dataLocalizer";
            this.m_dataLocalizer.OnlyLocalize = true;
            this.m_dataLocalizer.Size = new System.Drawing.Size(798, 23);
            this.m_dataLocalizer.StartIndex = 0;
            this.m_dataLocalizer.TabIndex = 49;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Red;
            this.label6.Location = new System.Drawing.Point(8, 45);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(497, 14);
            this.label6.TabIndex = 48;
            this.label6.Text = "提示信息: 右击数据显示控件有功能菜单, 点击供货单位数据列可以设置供应商";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(267, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(309, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "总成返修件条码打印管理";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.splitter1);
            this.panel2.Controls.Add(this.panel4);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 85);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(798, 439);
            this.panel2.TabIndex = 59;
            // 
            // splitter1
            // 
            this.splitter1.Dock = System.Windows.Forms.DockStyle.Right;
            this.splitter1.Location = new System.Drawing.Point(567, 0);
            this.splitter1.Name = "splitter1";
            this.splitter1.Size = new System.Drawing.Size(3, 439);
            this.splitter1.TabIndex = 2;
            this.splitter1.TabStop = false;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.dataGridView1);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(0, 0);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(570, 439);
            this.panel4.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.序号,
            this.产品编码,
            this.零部件编码,
            this.零部件名称,
            this.规格,
            this.数量,
            this.供货单位,
            this.位置});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridView1.RowHeadersWidth = 40;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(570, 439);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellValueChanged);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dataGridView1_DataError);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // 序号
            // 
            this.序号.HeaderText = "序号";
            this.序号.Name = "序号";
            this.序号.ReadOnly = true;
            this.序号.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 产品编码
            // 
            this.产品编码.HeaderText = "产品编码";
            this.产品编码.Name = "产品编码";
            this.产品编码.ReadOnly = true;
            this.产品编码.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.产品编码.Visible = false;
            // 
            // 零部件编码
            // 
            this.零部件编码.HeaderText = "零部件编码";
            this.零部件编码.Name = "零部件编码";
            this.零部件编码.ReadOnly = true;
            this.零部件编码.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 零部件名称
            // 
            this.零部件名称.HeaderText = "零部件名称";
            this.零部件名称.Name = "零部件名称";
            this.零部件名称.ReadOnly = true;
            this.零部件名称.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 规格
            // 
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            this.规格.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 数量
            // 
            this.数量.HeaderText = "数量";
            this.数量.Name = "数量";
            this.数量.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 供货单位
            // 
            this.供货单位.HeaderText = "供货单位";
            this.供货单位.Name = "供货单位";
            this.供货单位.ReadOnly = true;
            this.供货单位.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 位置
            // 
            this.位置.HeaderText = "位置";
            this.位置.Name = "位置";
            this.位置.ReadOnly = true;
            this.位置.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.只打印选择行ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.删除选择行记录仅本次打印有效ToolStripMenuItem,
            this.删除选择行记录永远有效ToolStripMenuItem,
            this.toolStripMenuItem1,
            this.移除选择记录供应商ToolStripMenuItem,
            this.移除所有选择记录供应商永久有效ToolStripMenuItem,
            this.toolStripMenuItem4,
            this.向上移动ToolStripMenuItem,
            this.向下移动ToolStripMenuItem,
            this.移动到指定行ToolStripMenuItem,
            this.移动到第一个ToolStripMenuItem,
            this.移动到最后ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(301, 242);
            // 
            // 只打印选择行ToolStripMenuItem
            // 
            this.只打印选择行ToolStripMenuItem.Name = "只打印选择行ToolStripMenuItem";
            this.只打印选择行ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.只打印选择行ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.只打印选择行ToolStripMenuItem.Text = "只打印选择行";
            this.只打印选择行ToolStripMenuItem.Click += new System.EventHandler(this.只打印选择行ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(297, 6);
            // 
            // 删除选择行记录仅本次打印有效ToolStripMenuItem
            // 
            this.删除选择行记录仅本次打印有效ToolStripMenuItem.Name = "删除选择行记录仅本次打印有效ToolStripMenuItem";
            this.删除选择行记录仅本次打印有效ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D)));
            this.删除选择行记录仅本次打印有效ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.删除选择行记录仅本次打印有效ToolStripMenuItem.Text = "删除选择行记录(仅本次打印有效)";
            this.删除选择行记录仅本次打印有效ToolStripMenuItem.Click += new System.EventHandler(this.删除选择行记录仅本次打印有效ToolStripMenuItem_Click);
            // 
            // 删除选择行记录永远有效ToolStripMenuItem
            // 
            this.删除选择行记录永远有效ToolStripMenuItem.Name = "删除选择行记录永远有效ToolStripMenuItem";
            this.删除选择行记录永远有效ToolStripMenuItem.ShortcutKeys = System.Windows.Forms.Keys.Delete;
            this.删除选择行记录永远有效ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.删除选择行记录永远有效ToolStripMenuItem.Text = "删除选择行记录(永远有效)";
            this.删除选择行记录永远有效ToolStripMenuItem.ToolTipText = "需点击保存按钮";
            this.删除选择行记录永远有效ToolStripMenuItem.Click += new System.EventHandler(this.删除选择行记录永远有效ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(297, 6);
            // 
            // 移除选择记录供应商ToolStripMenuItem
            // 
            this.移除选择记录供应商ToolStripMenuItem.Name = "移除选择记录供应商ToolStripMenuItem";
            this.移除选择记录供应商ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.移除选择记录供应商ToolStripMenuItem.Text = "移除所有选择记录供应商(仅本次打印有效)";
            this.移除选择记录供应商ToolStripMenuItem.Click += new System.EventHandler(this.移除选择记录供应商_本次有效ToolStripMenuItem_Click);
            // 
            // 移除所有选择记录供应商永久有效ToolStripMenuItem
            // 
            this.移除所有选择记录供应商永久有效ToolStripMenuItem.Name = "移除所有选择记录供应商永久有效ToolStripMenuItem";
            this.移除所有选择记录供应商永久有效ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.移除所有选择记录供应商永久有效ToolStripMenuItem.Text = "移除所有选择记录供应商(永久有效)";
            this.移除所有选择记录供应商永久有效ToolStripMenuItem.ToolTipText = "需点击保存按钮";
            this.移除所有选择记录供应商永久有效ToolStripMenuItem.Click += new System.EventHandler(this.移除所有选择记录供应商永久有效ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem4
            // 
            this.toolStripMenuItem4.Name = "toolStripMenuItem4";
            this.toolStripMenuItem4.Size = new System.Drawing.Size(297, 6);
            // 
            // 向上移动ToolStripMenuItem
            // 
            this.向上移动ToolStripMenuItem.Name = "向上移动ToolStripMenuItem";
            this.向上移动ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Up)));
            this.向上移动ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.向上移动ToolStripMenuItem.Text = "向 ↑ 移动";
            this.向上移动ToolStripMenuItem.Click += new System.EventHandler(this.向上移动ToolStripMenuItem_Click);
            // 
            // 向下移动ToolStripMenuItem
            // 
            this.向下移动ToolStripMenuItem.Name = "向下移动ToolStripMenuItem";
            this.向下移动ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Down)));
            this.向下移动ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.向下移动ToolStripMenuItem.Text = "向 ↓ 移动";
            this.向下移动ToolStripMenuItem.Click += new System.EventHandler(this.向下移动ToolStripMenuItem_Click);
            // 
            // 移动到指定行ToolStripMenuItem
            // 
            this.移动到指定行ToolStripMenuItem.Name = "移动到指定行ToolStripMenuItem";
            this.移动到指定行ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.R)));
            this.移动到指定行ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.移动到指定行ToolStripMenuItem.Text = "移动到指定行";
            this.移动到指定行ToolStripMenuItem.Click += new System.EventHandler(this.移动到指定行ToolStripMenuItem_Click);
            // 
            // 移动到第一个ToolStripMenuItem
            // 
            this.移动到第一个ToolStripMenuItem.Name = "移动到第一个ToolStripMenuItem";
            this.移动到第一个ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.B)));
            this.移动到第一个ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.移动到第一个ToolStripMenuItem.Text = "移动到开始";
            this.移动到第一个ToolStripMenuItem.Click += new System.EventHandler(this.移动到第一个ToolStripMenuItem_Click);
            // 
            // 移动到最后ToolStripMenuItem
            // 
            this.移动到最后ToolStripMenuItem.Name = "移动到最后ToolStripMenuItem";
            this.移动到最后ToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.End)));
            this.移动到最后ToolStripMenuItem.Size = new System.Drawing.Size(300, 22);
            this.移动到最后ToolStripMenuItem.Text = "移动到最后";
            this.移动到最后ToolStripMenuItem.Click += new System.EventHandler(this.移动到最后ToolStripMenuItem_Click);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.cmbPurpose);
            this.panel3.Controls.Add(this.label5);
            this.panel3.Controls.Add(this.btnSave);
            this.panel3.Controls.Add(this.btnPrint);
            this.panel3.Controls.Add(this.btnDelete);
            this.panel3.Controls.Add(this.btnAdd);
            this.panel3.Controls.Add(this.txtEdition);
            this.panel3.Controls.Add(this.label1);
            this.panel3.Controls.Add(this.txtGoodsName);
            this.panel3.Controls.Add(this.txtGoodsCode);
            this.panel3.Controls.Add(this.label4);
            this.panel3.Controls.Add(this.txtSpec);
            this.panel3.Controls.Add(this.label3);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel3.Location = new System.Drawing.Point(570, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(228, 439);
            this.panel3.TabIndex = 0;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Red;
            this.label7.Location = new System.Drawing.Point(27, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 14);
            this.label7.TabIndex = 50;
            this.label7.Text = "多批次用途";
            // 
            // cmbPurpose
            // 
            this.cmbPurpose.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbPurpose.ForeColor = System.Drawing.Color.Red;
            this.cmbPurpose.FormattingEnabled = true;
            this.cmbPurpose.Location = new System.Drawing.Point(27, 217);
            this.cmbPurpose.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbPurpose.Name = "cmbPurpose";
            this.cmbPurpose.Size = new System.Drawing.Size(190, 22);
            this.cmbPurpose.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Red;
            this.label5.Location = new System.Drawing.Point(118, 12);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 48;
            this.label5.Text = "请选择总成";
            // 
            // btnSave
            // 
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(141, 322);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 47;
            this.btnSave.Text = "保存";
            this.toolTip1.SetToolTip(this.btnSave, "保存改变到数据库，针对永久有效项");
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnPrint
            // 
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnPrint.Location = new System.Drawing.Point(23, 322);
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(75, 23);
            this.btnPrint.TabIndex = 47;
            this.btnPrint.Text = "打印";
            this.btnPrint.UseVisualStyleBackColor = true;
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDelete.Location = new System.Drawing.Point(141, 266);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 46;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.删除选择行记录永远有效ToolStripMenuItem_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(23, 266);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 23);
            this.btnAdd.TabIndex = 45;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtEdition
            // 
            this.txtEdition.DataResult = null;
            this.txtEdition.DataTableResult = null;
            this.txtEdition.EditingControlDataGridView = null;
            this.txtEdition.EditingControlFormattedValue = "";
            this.txtEdition.EditingControlRowIndex = 0;
            this.txtEdition.EditingControlValueChanged = false;
            this.txtEdition.FindItem = UniversalControlLibrary.TextBoxShow.FindType.BOM表零件;
            this.txtEdition.IsMultiSelect = false;
            this.txtEdition.Location = new System.Drawing.Point(118, 26);
            this.txtEdition.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtEdition.Name = "txtEdition";
            this.txtEdition.ShowResultForm = true;
            this.txtEdition.Size = new System.Drawing.Size(98, 23);
            this.txtEdition.StrEndSql = null;
            this.txtEdition.TabIndex = 44;
            this.txtEdition.TabStop = false;
            this.txtEdition.Tag = "";
            this.txtEdition.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtEdition_OnCompleteSearch);
            this.txtEdition.TextChanged += new System.EventHandler(this.txtEdition_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(20, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 43;
            this.label1.Text = "父总成编码";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.DataResult = null;
            this.txtGoodsName.DataTableResult = null;
            this.txtGoodsName.EditingControlDataGridView = null;
            this.txtGoodsName.EditingControlFormattedValue = "";
            this.txtGoodsName.EditingControlRowIndex = 0;
            this.txtGoodsName.EditingControlValueChanged = false;
            this.txtGoodsName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsName.IsMultiSelect = false;
            this.txtGoodsName.Location = new System.Drawing.Point(118, 75);
            this.txtGoodsName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ReadOnly = true;
            this.txtGoodsName.ShowResultForm = true;
            this.txtGoodsName.Size = new System.Drawing.Size(98, 23);
            this.txtGoodsName.StrEndSql = null;
            this.txtGoodsName.TabIndex = 42;
            this.txtGoodsName.TabStop = false;
            this.txtGoodsName.Tag = "";
            this.txtGoodsName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsName_OnCompleteSearch);
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.Location = new System.Drawing.Point(118, 121);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ReadOnly = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(98, 23);
            this.txtGoodsCode.TabIndex = 41;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(27, 169);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 40;
            this.label4.Text = "规    格";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(118, 167);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(98, 23);
            this.txtSpec.TabIndex = 39;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(20, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 38;
            this.label3.Text = "零部件编码";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(20, 78);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 37;
            this.label2.Text = "零部件名称";
            // 
            // 打印整台份返修箱条码
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 524);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panelTop);
            this.Name = "打印整台份返修箱条码";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "打印整台份返修箱条码";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Resize += new System.EventHandler(this.打印整台份返修箱条码_Resize);
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Splitter splitter1;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel3;
        private TextBoxShow txtGoodsName;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnPrint;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private TextBoxShow txtEdition;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 移除选择记录供应商ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 只打印选择行ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 删除选择行记录仅本次打印有效ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 移除所有选择记录供应商永久有效ToolStripMenuItem;
        private System.Windows.Forms.Label label6;
        private UserControlDataLocalizer m_dataLocalizer;
        private System.Windows.Forms.ToolStripMenuItem 删除选择行记录永远有效ToolStripMenuItem;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
        private System.Windows.Forms.ToolStripMenuItem 向上移动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 向下移动ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动到第一个ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动到最后ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 移动到指定行ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn 序号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零部件编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零部件名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供货单位;
        private System.Windows.Forms.DataGridViewTextBoxColumn 位置;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbPurpose;
    }
}