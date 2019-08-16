using UniversalControlLibrary;
namespace Form_Economic_Financial
{
    partial class 最低销售定价
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(最低销售定价));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.保存toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.删除toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.导入toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.导出toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.主机厂与系统零件toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtClient = new UniversalControlLibrary.TextBoxShow();
            this.txtClientGoodsName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbsClientGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label11 = new System.Windows.Forms.Label();
            this.numTerminalPrice = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtRecordTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtRecorder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.numLowestPrice = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numUnitPrice = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.lbUnit = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbsGoods = new UniversalControlLibrary.TextBoxShow();
            this.txtSpce = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTerminalPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowestPrice)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitPrice)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.保存toolStripButton,
            this.toolStripSeparator1,
            this.删除toolStripButton,
            this.toolStripSeparator2,
            this.导入toolStripButton,
            this.toolStripSeparator3,
            this.导出toolStripButton,
            this.toolStripSeparator5,
            this.主机厂与系统零件toolStripButton,
            this.toolStripSeparator4,
            this.刷新toolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(958, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 保存toolStripButton
            // 
            this.保存toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.保存toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("保存toolStripButton.Image")));
            this.保存toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.保存toolStripButton.Name = "保存toolStripButton";
            this.保存toolStripButton.Size = new System.Drawing.Size(39, 22);
            this.保存toolStripButton.Tag = "Add";
            this.保存toolStripButton.Text = "保 存";
            this.保存toolStripButton.Click += new System.EventHandler(this.保存toolStripButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // 删除toolStripButton
            // 
            this.删除toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.删除toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("删除toolStripButton.Image")));
            this.删除toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除toolStripButton.Name = "删除toolStripButton";
            this.删除toolStripButton.Size = new System.Drawing.Size(39, 22);
            this.删除toolStripButton.Tag = "Delete";
            this.删除toolStripButton.Text = "删 除";
            this.删除toolStripButton.Click += new System.EventHandler(this.删除toolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 导入toolStripButton
            // 
            this.导入toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.导入toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("导入toolStripButton.Image")));
            this.导入toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导入toolStripButton.Name = "导入toolStripButton";
            this.导入toolStripButton.Size = new System.Drawing.Size(39, 22);
            this.导入toolStripButton.Tag = "Auditing";
            this.导入toolStripButton.Text = "导 入";
            this.导入toolStripButton.Click += new System.EventHandler(this.导入toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "Auditing";
            // 
            // 导出toolStripButton
            // 
            this.导出toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.导出toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("导出toolStripButton.Image")));
            this.导出toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出toolStripButton.Name = "导出toolStripButton";
            this.导出toolStripButton.Size = new System.Drawing.Size(39, 22);
            this.导出toolStripButton.Tag = "Auditing";
            this.导出toolStripButton.Text = "导 出";
            this.导出toolStripButton.Click += new System.EventHandler(this.导出toolStripButton_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator5.Tag = "Auditing";
            // 
            // 主机厂与系统零件toolStripButton
            // 
            this.主机厂与系统零件toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.主机厂与系统零件toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("主机厂与系统零件toolStripButton.Image")));
            this.主机厂与系统零件toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.主机厂与系统零件toolStripButton.Name = "主机厂与系统零件toolStripButton";
            this.主机厂与系统零件toolStripButton.Size = new System.Drawing.Size(165, 22);
            this.主机厂与系统零件toolStripButton.Tag = "Add";
            this.主机厂与系统零件toolStripButton.Text = "主机厂与系统的零件匹配设置";
            this.主机厂与系统零件toolStripButton.Click += new System.EventHandler(this.主机厂与系统零件toolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 刷新toolStripButton
            // 
            this.刷新toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.刷新toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("刷新toolStripButton.Image")));
            this.刷新toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton.Name = "刷新toolStripButton";
            this.刷新toolStripButton.Size = new System.Drawing.Size(57, 22);
            this.刷新toolStripButton.Text = "刷新数据";
            this.刷新toolStripButton.Click += new System.EventHandler(this.刷新toolStripButton_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(958, 53);
            this.panel1.TabIndex = 52;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(384, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "最低销售定价";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.txtClient);
            this.panel2.Controls.Add(this.txtClientGoodsName);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Controls.Add(this.tbsClientGoodsCode);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.numTerminalPrice);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label8);
            this.panel2.Controls.Add(this.txtRemark);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.txtRecordTime);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.txtRecorder);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.numLowestPrice);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.numUnitPrice);
            this.panel2.Controls.Add(this.label16);
            this.panel2.Controls.Add(this.lbUnit);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtGoodsCode);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.tbsGoods);
            this.panel2.Controls.Add(this.txtSpce);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(0, 78);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(958, 129);
            this.panel2.TabIndex = 53;
            // 
            // txtClient
            // 
            this.txtClient.BackColor = System.Drawing.Color.White;
            this.txtClient.EditingControlDataGridView = null;
            this.txtClient.EditingControlFormattedValue = " ";
            this.txtClient.EditingControlRowIndex = 0;
            this.txtClient.EditingControlValueChanged = true;
            this.txtClient.FindItem = UniversalControlLibrary.TextBoxShow.FindType.客户;
            this.txtClient.Location = new System.Drawing.Point(98, 40);
            this.txtClient.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtClient.Name = "txtClient";
            this.txtClient.ReadOnly = true;
            this.txtClient.ShowResultForm = true;
            this.txtClient.Size = new System.Drawing.Size(153, 23);
            this.txtClient.TabIndex = 93;
            this.txtClient.TabStop = false;
            this.txtClient.Text = " ";
            this.txtClient.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtClient_OnCompleteSearch);
            // 
            // txtClientGoodsName
            // 
            this.txtClientGoodsName.BackColor = System.Drawing.Color.White;
            this.txtClientGoodsName.ForeColor = System.Drawing.Color.Black;
            this.txtClientGoodsName.Location = new System.Drawing.Point(371, 70);
            this.txtClientGoodsName.Name = "txtClientGoodsName";
            this.txtClientGoodsName.ReadOnly = true;
            this.txtClientGoodsName.Size = new System.Drawing.Size(151, 23);
            this.txtClientGoodsName.TabIndex = 85;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(274, 74);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(91, 14);
            this.label12.TabIndex = 84;
            this.label12.Text = "客户零件名称";
            // 
            // tbsClientGoodsCode
            // 
            this.tbsClientGoodsCode.EditingControlDataGridView = null;
            this.tbsClientGoodsCode.EditingControlFormattedValue = "";
            this.tbsClientGoodsCode.EditingControlRowIndex = 0;
            this.tbsClientGoodsCode.EditingControlValueChanged = false;
            this.tbsClientGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.主机厂零件;
            this.tbsClientGoodsCode.Location = new System.Drawing.Point(98, 70);
            this.tbsClientGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbsClientGoodsCode.Name = "tbsClientGoodsCode";
            this.tbsClientGoodsCode.ShowResultForm = true;
            this.tbsClientGoodsCode.Size = new System.Drawing.Size(153, 23);
            this.tbsClientGoodsCode.TabIndex = 83;
            this.tbsClientGoodsCode.TabStop = false;
            this.tbsClientGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsClientGoodsCode_OnCompleteSearch);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(3, 74);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(91, 14);
            this.label11.TabIndex = 82;
            this.label11.Text = "客户零件代码";
            // 
            // numTerminalPrice
            // 
            this.numTerminalPrice.DecimalPlaces = 2;
            this.numTerminalPrice.Location = new System.Drawing.Point(371, 100);
            this.numTerminalPrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numTerminalPrice.Name = "numTerminalPrice";
            this.numTerminalPrice.Size = new System.Drawing.Size(151, 23);
            this.numTerminalPrice.TabIndex = 81;
            this.numTerminalPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.ForeColor = System.Drawing.Color.Blue;
            this.label10.Location = new System.Drawing.Point(302, 104);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 80;
            this.label10.Text = "终端价格";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(31, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 78;
            this.label8.Text = "客户名称";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.ForeColor = System.Drawing.Color.Black;
            this.txtRemark.Location = new System.Drawing.Point(618, 71);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(337, 52);
            this.txtRemark.TabIndex = 64;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(549, 90);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 63;
            this.label7.Text = "备    注";
            // 
            // txtRecordTime
            // 
            this.txtRecordTime.BackColor = System.Drawing.Color.White;
            this.txtRecordTime.ForeColor = System.Drawing.Color.Black;
            this.txtRecordTime.Location = new System.Drawing.Point(810, 40);
            this.txtRecordTime.Name = "txtRecordTime";
            this.txtRecordTime.ReadOnly = true;
            this.txtRecordTime.Size = new System.Drawing.Size(145, 23);
            this.txtRecordTime.TabIndex = 75;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(741, 44);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 74;
            this.label6.Text = "定价时间";
            // 
            // txtRecorder
            // 
            this.txtRecorder.BackColor = System.Drawing.Color.White;
            this.txtRecorder.ForeColor = System.Drawing.Color.Black;
            this.txtRecorder.Location = new System.Drawing.Point(618, 40);
            this.txtRecorder.Name = "txtRecorder";
            this.txtRecorder.ReadOnly = true;
            this.txtRecorder.Size = new System.Drawing.Size(103, 23);
            this.txtRecorder.TabIndex = 73;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.Green;
            this.label4.Location = new System.Drawing.Point(549, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 72;
            this.label4.Text = "定 价 人";
            // 
            // numLowestPrice
            // 
            this.numLowestPrice.DecimalPlaces = 2;
            this.numLowestPrice.Location = new System.Drawing.Point(98, 100);
            this.numLowestPrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numLowestPrice.Name = "numLowestPrice";
            this.numLowestPrice.Size = new System.Drawing.Size(153, 23);
            this.numLowestPrice.TabIndex = 71;
            this.numLowestPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(31, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 70;
            this.label3.Text = "最低定价";
            // 
            // numUnitPrice
            // 
            this.numUnitPrice.DecimalPlaces = 2;
            this.numUnitPrice.Enabled = false;
            this.numUnitPrice.Location = new System.Drawing.Point(371, 40);
            this.numUnitPrice.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numUnitPrice.Name = "numUnitPrice";
            this.numUnitPrice.Size = new System.Drawing.Size(151, 23);
            this.numUnitPrice.TabIndex = 69;
            this.numUnitPrice.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(302, 44);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(63, 14);
            this.label16.TabIndex = 68;
            this.label16.Text = "单    价";
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUnit.Location = new System.Drawing.Point(807, 14);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(49, 14);
            this.lbUnit.TabIndex = 67;
            this.lbUnit.Text = "lbUnit";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(752, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 66;
            this.label2.Text = "单位：";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.BackColor = System.Drawing.Color.White;
            this.txtGoodsCode.ForeColor = System.Drawing.Color.Black;
            this.txtGoodsCode.Location = new System.Drawing.Point(371, 10);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ReadOnly = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(151, 23);
            this.txtGoodsCode.TabIndex = 65;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(302, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 64;
            this.label1.Text = "图号型号";
            // 
            // tbsGoods
            // 
            this.tbsGoods.EditingControlDataGridView = null;
            this.tbsGoods.EditingControlFormattedValue = "";
            this.tbsGoods.EditingControlRowIndex = 0;
            this.tbsGoods.EditingControlValueChanged = false;
            this.tbsGoods.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.tbsGoods.Location = new System.Drawing.Point(98, 10);
            this.tbsGoods.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbsGoods.Name = "tbsGoods";
            this.tbsGoods.ShowResultForm = true;
            this.tbsGoods.Size = new System.Drawing.Size(153, 23);
            this.tbsGoods.TabIndex = 63;
            this.tbsGoods.TabStop = false;
            this.tbsGoods.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsGoods_OnCompleteSearch);
            // 
            // txtSpce
            // 
            this.txtSpce.BackColor = System.Drawing.Color.White;
            this.txtSpce.ForeColor = System.Drawing.Color.Black;
            this.txtSpce.Location = new System.Drawing.Point(618, 10);
            this.txtSpce.Name = "txtSpce";
            this.txtSpce.ReadOnly = true;
            this.txtSpce.Size = new System.Drawing.Size(103, 23);
            this.txtSpce.TabIndex = 62;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(549, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 61;
            this.label9.Text = "规    格";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(31, 14);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 60;
            this.label5.Text = "产品名称";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridView1);
            this.panel3.Controls.Add(this.userControlDataLocalizer1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 207);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(958, 352);
            this.panel3.TabIndex = 54;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 41);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(958, 311);
            this.dataGridView1.TabIndex = 2;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(958, 41);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.Title = "将查询结果保存成 EXCEL 文件";
            // 
            // 最低销售定价
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(958, 559);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "最低销售定价";
            this.Text = "最低销售定价";
            this.Load += new System.EventHandler(this.最低销售定价_Load);
            this.Resize += new System.EventHandler(this.最低销售定价_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numTerminalPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLowestPrice)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numUnitPrice)).EndInit();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 保存toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label1;
        private TextBoxShow tbsGoods;
        private System.Windows.Forms.TextBox txtSpce;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown numLowestPrice;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numUnitPrice;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtRecordTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtRecorder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 导入toolStripButton;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numTerminalPrice;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton 主机厂与系统零件toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton;
        private System.Windows.Forms.TextBox txtClientGoodsName;
        private System.Windows.Forms.Label label12;
        private TextBoxShow tbsClientGoodsCode;
        private System.Windows.Forms.Label label11;
        private TextBoxShow txtClient;
        private System.Windows.Forms.ToolStripButton 导出toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}