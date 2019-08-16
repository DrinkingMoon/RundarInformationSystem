using UniversalControlLibrary;
namespace Expression
{
    partial class 物料扣货单
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
            this.查看清单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.panelPara = new System.Windows.Forms.Panel();
            this.txtSQE = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.label36 = new System.Windows.Forms.Label();
            this.txtDepotManager = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtFinanceSignatory = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtReason = new System.Windows.Forms.TextBox();
            this.txtProposer = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtProvider = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.dateTime_BillTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBill_ID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblBillStatus = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.labelTitle = new System.Windows.Forms.Label();
            this.编制员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.新建单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.设置清单ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.领料员提交单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.修改单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.综合查询ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.批准ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.质管操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.上级部门ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.审核单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sQE操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.确认单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.采购操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.核实物料信息ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.采购确认ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuItemReresh = new System.Windows.Forms.ToolStripMenuItem();
            this.回退单据ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panelCenter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panelMain.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // 查看清单ToolStripMenuItem
            // 
            this.查看清单ToolStripMenuItem.Name = "查看清单ToolStripMenuItem";
            this.查看清单ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.查看清单ToolStripMenuItem.Text = "查看清单";
            this.查看清单ToolStripMenuItem.Click += new System.EventHandler(this.查看清单ToolStripMenuItem_Click);
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panelPara);
            this.panelCenter.Controls.Add(this.checkBillDateAndStatus1);
            this.panelCenter.Controls.Add(this.panel2);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(10, 43);
            this.panelCenter.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1066, 601);
            this.panelCenter.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 190);
            this.dataGridView1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 46;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1066, 400);
            this.dataGridView1.TabIndex = 239;
            this.toolTip1.SetToolTip(this.dataGridView1, "右击可查看领料清单");
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查看清单ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 26);
            // 
            // panelPara
            // 
            this.panelPara.AutoScroll = true;
            this.panelPara.BackColor = System.Drawing.SystemColors.Control;
            this.panelPara.Controls.Add(this.txtSQE);
            this.panelPara.Controls.Add(this.label4);
            this.panelPara.Controls.Add(this.cmbStorage);
            this.panelPara.Controls.Add(this.label36);
            this.panelPara.Controls.Add(this.txtDepotManager);
            this.panelPara.Controls.Add(this.label9);
            this.panelPara.Controls.Add(this.txtFinanceSignatory);
            this.panelPara.Controls.Add(this.label6);
            this.panelPara.Controls.Add(this.label3);
            this.panelPara.Controls.Add(this.txtReason);
            this.panelPara.Controls.Add(this.txtProposer);
            this.panelPara.Controls.Add(this.label1);
            this.panelPara.Controls.Add(this.btnFind);
            this.panelPara.Controls.Add(this.txtProvider);
            this.panelPara.Controls.Add(this.label22);
            this.panelPara.Controls.Add(this.txtRemark);
            this.panelPara.Controls.Add(this.dateTime_BillTime);
            this.panelPara.Controls.Add(this.label5);
            this.panelPara.Controls.Add(this.label11);
            this.panelPara.Controls.Add(this.txtBill_ID);
            this.panelPara.Controls.Add(this.label2);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 49);
            this.panelPara.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1066, 141);
            this.panelPara.TabIndex = 16;
            // 
            // txtSQE
            // 
            this.txtSQE.BackColor = System.Drawing.Color.White;
            this.txtSQE.ForeColor = System.Drawing.Color.Red;
            this.txtSQE.Location = new System.Drawing.Point(651, 77);
            this.txtSQE.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtSQE.Name = "txtSQE";
            this.txtSQE.ReadOnly = true;
            this.txtSQE.Size = new System.Drawing.Size(170, 23);
            this.txtSQE.TabIndex = 239;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Green;
            this.label4.Location = new System.Drawing.Point(587, 81);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 14);
            this.label4.TabIndex = 238;
            this.label4.Text = "SQE签名";
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(910, 9);
            this.cmbStorage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(138, 21);
            this.cmbStorage.TabIndex = 237;
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.ForeColor = System.Drawing.Color.Blue;
            this.label36.Location = new System.Drawing.Point(841, 13);
            this.label36.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(63, 14);
            this.label36.TabIndex = 236;
            this.label36.Text = "所属库房";
            // 
            // txtDepotManager
            // 
            this.txtDepotManager.BackColor = System.Drawing.Color.White;
            this.txtDepotManager.ForeColor = System.Drawing.Color.Red;
            this.txtDepotManager.Location = new System.Drawing.Point(908, 77);
            this.txtDepotManager.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtDepotManager.Name = "txtDepotManager";
            this.txtDepotManager.ReadOnly = true;
            this.txtDepotManager.Size = new System.Drawing.Size(140, 23);
            this.txtDepotManager.TabIndex = 233;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.ForeColor = System.Drawing.Color.Green;
            this.label9.Location = new System.Drawing.Point(841, 81);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 232;
            this.label9.Text = "采购签名";
            // 
            // txtFinanceSignatory
            // 
            this.txtFinanceSignatory.BackColor = System.Drawing.Color.White;
            this.txtFinanceSignatory.ForeColor = System.Drawing.Color.Red;
            this.txtFinanceSignatory.Location = new System.Drawing.Point(367, 77);
            this.txtFinanceSignatory.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtFinanceSignatory.Name = "txtFinanceSignatory";
            this.txtFinanceSignatory.ReadOnly = true;
            this.txtFinanceSignatory.Size = new System.Drawing.Size(183, 23);
            this.txtFinanceSignatory.TabIndex = 229;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(286, 81);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 228;
            this.label6.Text = "质管签名";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(4, 46);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 227;
            this.label3.Text = "扣货原因";
            // 
            // txtReason
            // 
            this.txtReason.BackColor = System.Drawing.Color.White;
            this.txtReason.Location = new System.Drawing.Point(81, 43);
            this.txtReason.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtReason.Name = "txtReason";
            this.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReason.Size = new System.Drawing.Size(469, 23);
            this.txtReason.TabIndex = 226;
            // 
            // txtProposer
            // 
            this.txtProposer.BackColor = System.Drawing.Color.White;
            this.txtProposer.ForeColor = System.Drawing.Color.Red;
            this.txtProposer.Location = new System.Drawing.Point(81, 77);
            this.txtProposer.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtProposer.Name = "txtProposer";
            this.txtProposer.ReadOnly = true;
            this.txtProposer.Size = new System.Drawing.Size(179, 23);
            this.txtProposer.TabIndex = 225;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Green;
            this.label1.Location = new System.Drawing.Point(4, 81);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 224;
            this.label1.Text = "建 单 人";
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.Transparent;
            this.btnFind.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFind.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFind.Location = new System.Drawing.Point(797, 10);
            this.btnFind.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(24, 23);
            this.btnFind.TabIndex = 223;
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // txtProvider
            // 
            this.txtProvider.BackColor = System.Drawing.Color.White;
            this.txtProvider.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtProvider.Location = new System.Drawing.Point(651, 10);
            this.txtProvider.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ReadOnly = true;
            this.txtProvider.Size = new System.Drawing.Size(140, 23);
            this.txtProvider.TabIndex = 2;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.ForeColor = System.Drawing.Color.Blue;
            this.label22.Location = new System.Drawing.Point(582, 46);
            this.label22.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(63, 14);
            this.label22.TabIndex = 67;
            this.label22.Text = "备    注";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Location = new System.Drawing.Point(651, 43);
            this.txtRemark.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(397, 23);
            this.txtRemark.TabIndex = 9;
            // 
            // dateTime_BillTime
            // 
            this.dateTime_BillTime.Enabled = false;
            this.dateTime_BillTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime_BillTime.Location = new System.Drawing.Point(367, 9);
            this.dateTime_BillTime.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.dateTime_BillTime.Name = "dateTime_BillTime";
            this.dateTime_BillTime.Size = new System.Drawing.Size(183, 23);
            this.dateTime_BillTime.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Green;
            this.label5.Location = new System.Drawing.Point(286, 13);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 38;
            this.label5.Text = "建单时间";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ForeColor = System.Drawing.Color.Blue;
            this.label11.Location = new System.Drawing.Point(582, 13);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 14);
            this.label11.TabIndex = 40;
            this.label11.Text = "供 应 商";
            // 
            // txtBill_ID
            // 
            this.txtBill_ID.BackColor = System.Drawing.Color.White;
            this.txtBill_ID.ForeColor = System.Drawing.Color.Red;
            this.txtBill_ID.Location = new System.Drawing.Point(81, 9);
            this.txtBill_ID.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.txtBill_ID.Name = "txtBill_ID";
            this.txtBill_ID.ReadOnly = true;
            this.txtBill_ID.Size = new System.Drawing.Size(179, 23);
            this.txtBill_ID.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Green;
            this.label2.Location = new System.Drawing.Point(4, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "扣货单号";
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 0);
            this.checkBillDateAndStatus1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(1066, 49);
            this.checkBillDateAndStatus1.TabIndex = 238;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
            // 
            // panel2
            // 
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 590);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1066, 11);
            this.panel2.TabIndex = 27;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.lblBillStatus);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1086, 43);
            this.panel1.TabIndex = 24;
            // 
            // lblBillStatus
            // 
            this.lblBillStatus.AutoSize = true;
            this.lblBillStatus.Location = new System.Drawing.Point(115, 20);
            this.lblBillStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBillStatus.Name = "lblBillStatus";
            this.lblBillStatus.Size = new System.Drawing.Size(0, 14);
            this.lblBillStatus.TabIndex = 191;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(18, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(77, 14);
            this.label7.TabIndex = 190;
            this.label7.Text = "单据状态：";
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 22F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(459, 6);
            this.labelTitle.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(168, 30);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "物料扣货单";
            // 
            // 编制员操作ToolStripMenuItem
            // 
            this.编制员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建单据ToolStripMenuItem,
            this.设置清单ToolStripMenuItem,
            this.领料员提交单据ToolStripMenuItem,
            this.toolStripMenuItem3,
            this.修改单据ToolStripMenuItem,
            this.toolStripMenuItem2,
            this.删除单据ToolStripMenuItem});
            this.编制员操作ToolStripMenuItem.Name = "编制员操作ToolStripMenuItem";
            this.编制员操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.编制员操作ToolStripMenuItem.Tag = "ADD";
            this.编制员操作ToolStripMenuItem.Text = "编制员操作";
            // 
            // 新建单据ToolStripMenuItem
            // 
            this.新建单据ToolStripMenuItem.Name = "新建单据ToolStripMenuItem";
            this.新建单据ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.新建单据ToolStripMenuItem.Tag = "ADD";
            this.新建单据ToolStripMenuItem.Text = "新建单据";
            this.新建单据ToolStripMenuItem.Click += new System.EventHandler(this.新建单据ToolStripMenuItem_Click);
            // 
            // 设置清单ToolStripMenuItem
            // 
            this.设置清单ToolStripMenuItem.Name = "设置清单ToolStripMenuItem";
            this.设置清单ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.设置清单ToolStripMenuItem.Tag = "ADD";
            this.设置清单ToolStripMenuItem.Text = "设置清单";
            this.设置清单ToolStripMenuItem.Click += new System.EventHandler(this.设置清单ToolStripMenuItem_Click);
            // 
            // 领料员提交单据ToolStripMenuItem
            // 
            this.领料员提交单据ToolStripMenuItem.Name = "领料员提交单据ToolStripMenuItem";
            this.领料员提交单据ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.领料员提交单据ToolStripMenuItem.Tag = "ADD";
            this.领料员提交单据ToolStripMenuItem.Text = "提交单据";
            this.领料员提交单据ToolStripMenuItem.Click += new System.EventHandler(this.领料员提交单据ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(115, 6);
            // 
            // 修改单据ToolStripMenuItem
            // 
            this.修改单据ToolStripMenuItem.Name = "修改单据ToolStripMenuItem";
            this.修改单据ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.修改单据ToolStripMenuItem.Tag = "update";
            this.修改单据ToolStripMenuItem.Text = "修改单据";
            this.修改单据ToolStripMenuItem.Click += new System.EventHandler(this.修改单据ToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(115, 6);
            // 
            // 删除单据ToolStripMenuItem
            // 
            this.删除单据ToolStripMenuItem.Name = "删除单据ToolStripMenuItem";
            this.删除单据ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.删除单据ToolStripMenuItem.Tag = "DELETE";
            this.删除单据ToolStripMenuItem.Text = "删除单据";
            this.删除单据ToolStripMenuItem.Click += new System.EventHandler(this.删除单据ToolStripMenuItem_Click);
            // 
            // 综合查询ToolStripMenuItem
            // 
            this.综合查询ToolStripMenuItem.Name = "综合查询ToolStripMenuItem";
            this.综合查询ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.综合查询ToolStripMenuItem.Tag = "view";
            this.综合查询ToolStripMenuItem.Text = "综合查询";
            // 
            // 批准ToolStripMenuItem
            // 
            this.批准ToolStripMenuItem.Name = "批准ToolStripMenuItem";
            this.批准ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.批准ToolStripMenuItem.Tag = "Authorize";
            this.批准ToolStripMenuItem.Text = "质管批准";
            this.批准ToolStripMenuItem.Click += new System.EventHandler(this.批准ToolStripMenuItem_Click);
            // 
            // 质管操作ToolStripMenuItem
            // 
            this.质管操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.批准ToolStripMenuItem});
            this.质管操作ToolStripMenuItem.Name = "质管操作ToolStripMenuItem";
            this.质管操作ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.质管操作ToolStripMenuItem.Tag = "Authorize";
            this.质管操作ToolStripMenuItem.Text = "质管操作";
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panelRight);
            this.panelMain.Controls.Add(this.panelLeft);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 24);
            this.panelMain.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1086, 644);
            this.panelMain.TabIndex = 38;
            // 
            // panelRight
            // 
            this.panelRight.BackColor = System.Drawing.Color.Transparent;
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1076, 43);
            this.panelRight.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(10, 601);
            this.panelRight.TabIndex = 39;
            // 
            // panelLeft
            // 
            this.panelLeft.BackColor = System.Drawing.Color.Transparent;
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 43);
            this.panelLeft.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(10, 601);
            this.panelLeft.TabIndex = 38;
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.编制员操作ToolStripMenuItem,
            this.上级部门ToolStripMenuItem,
            this.质管操作ToolStripMenuItem,
            this.sQE操作ToolStripMenuItem,
            this.采购操作ToolStripMenuItem,
            this.综合查询ToolStripMenuItem,
            this.menuItemReresh,
            this.回退单据ToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1086, 24);
            this.menuStrip.TabIndex = 37;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 上级部门ToolStripMenuItem
            // 
            this.上级部门ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.审核单据ToolStripMenuItem});
            this.上级部门ToolStripMenuItem.Name = "上级部门ToolStripMenuItem";
            this.上级部门ToolStripMenuItem.Size = new System.Drawing.Size(89, 20);
            this.上级部门ToolStripMenuItem.Tag = "";
            this.上级部门ToolStripMenuItem.Text = "上级领导操作";
            this.上级部门ToolStripMenuItem.Visible = false;
            // 
            // 审核单据ToolStripMenuItem
            // 
            this.审核单据ToolStripMenuItem.Name = "审核单据ToolStripMenuItem";
            this.审核单据ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.审核单据ToolStripMenuItem.Tag = "";
            this.审核单据ToolStripMenuItem.Text = "审核单据";
            this.审核单据ToolStripMenuItem.Click += new System.EventHandler(this.审核单据ToolStripMenuItem_Click);
            // 
            // sQE操作ToolStripMenuItem
            // 
            this.sQE操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.确认单据ToolStripMenuItem});
            this.sQE操作ToolStripMenuItem.Name = "sQE操作ToolStripMenuItem";
            this.sQE操作ToolStripMenuItem.Size = new System.Drawing.Size(59, 20);
            this.sQE操作ToolStripMenuItem.Tag = "Confirm_2";
            this.sQE操作ToolStripMenuItem.Text = "SQE操作";
            // 
            // 确认单据ToolStripMenuItem
            // 
            this.确认单据ToolStripMenuItem.Name = "确认单据ToolStripMenuItem";
            this.确认单据ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.确认单据ToolStripMenuItem.Tag = "Confirm_2";
            this.确认单据ToolStripMenuItem.Text = "确认单据";
            this.确认单据ToolStripMenuItem.Click += new System.EventHandler(this.确认单据ToolStripMenuItem_Click);
            // 
            // 采购操作ToolStripMenuItem
            // 
            this.采购操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.核实物料信息ToolStripMenuItem,
            this.采购确认ToolStripMenuItem});
            this.采购操作ToolStripMenuItem.Name = "采购操作ToolStripMenuItem";
            this.采购操作ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.采购操作ToolStripMenuItem.Tag = "Confirm_1";
            this.采购操作ToolStripMenuItem.Text = "采购操作";
            // 
            // 核实物料信息ToolStripMenuItem
            // 
            this.核实物料信息ToolStripMenuItem.Name = "核实物料信息ToolStripMenuItem";
            this.核实物料信息ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.核实物料信息ToolStripMenuItem.Tag = "Confirm_1";
            this.核实物料信息ToolStripMenuItem.Text = "核实物料信息";
            this.核实物料信息ToolStripMenuItem.Click += new System.EventHandler(this.核实物料信息ToolStripMenuItem_Click);
            // 
            // 采购确认ToolStripMenuItem
            // 
            this.采购确认ToolStripMenuItem.Name = "采购确认ToolStripMenuItem";
            this.采购确认ToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.采购确认ToolStripMenuItem.Tag = "Confirm_1";
            this.采购确认ToolStripMenuItem.Text = "采购确认";
            this.采购确认ToolStripMenuItem.Click += new System.EventHandler(this.采购确认ToolStripMenuItem_Click);
            // 
            // menuItemReresh
            // 
            this.menuItemReresh.Name = "menuItemReresh";
            this.menuItemReresh.Size = new System.Drawing.Size(65, 20);
            this.menuItemReresh.Tag = "View";
            this.menuItemReresh.Text = "刷新数据";
            this.menuItemReresh.Click += new System.EventHandler(this.menuItemReresh_Click);
            // 
            // 回退单据ToolStripMenuItem
            // 
            this.回退单据ToolStripMenuItem.Name = "回退单据ToolStripMenuItem";
            this.回退单据ToolStripMenuItem.Size = new System.Drawing.Size(65, 20);
            this.回退单据ToolStripMenuItem.Tag = "view";
            this.回退单据ToolStripMenuItem.Text = "回退单据";
            this.回退单据ToolStripMenuItem.Click += new System.EventHandler(this.回退单据ToolStripMenuItem_Click);
            // 
            // 物料扣货单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 668);
            this.Controls.Add(this.panelMain);
            this.Controls.Add(this.menuStrip);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Name = "物料扣货单";
            this.Load += new System.EventHandler(this.物料扣货单_Load);
            this.panelCenter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panelPara.ResumeLayout(false);
            this.panelPara.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStripMenuItem 查看清单ToolStripMenuItem;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.Label label36;
        private System.Windows.Forms.TextBox txtDepotManager;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtFinanceSignatory;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtReason;
        private System.Windows.Forms.TextBox txtProposer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtProvider;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.DateTimePicker dateTime_BillTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtBill_ID;
        private System.Windows.Forms.Label label2;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblBillStatus;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripMenuItem 编制员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 新建单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 设置清单ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 领料员提交单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem 修改单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 删除单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 综合查询ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 批准ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 质管操作ToolStripMenuItem;
        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem menuItemReresh;
        private System.Windows.Forms.ToolStripMenuItem 采购操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 采购确认ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 核实物料信息ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sQE操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 确认单据ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 上级部门ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 审核单据ToolStripMenuItem;
        private System.Windows.Forms.TextBox txtSQE;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem 回退单据ToolStripMenuItem;
    }
}
