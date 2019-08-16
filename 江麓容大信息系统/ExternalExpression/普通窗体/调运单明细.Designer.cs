using UniversalControlLibrary;
namespace Form_Peripheral_External
{
    partial class 调运单明细
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(调运单明细));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnApply = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAuditing = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnShipments = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExcShipper = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExcConfirmor = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.btnReceiving = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnFindBillNo = new System.Windows.Forms.Button();
            this.txtScrapBillNo = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.txtReceiving = new UniversalControlLibrary.TextBoxShow();
            this.txtShipments = new UniversalControlLibrary.TextBoxShow();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.txtBillRemark = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtBill_ID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLogisticsName = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLogisticsBillNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.替换件ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.numProposerCount = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numShipperCount = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.numConfirmorCount = new System.Windows.Forms.NumericUpDown();
            this.txtListRemark = new System.Windows.Forms.TextBox();
            this.lbStock = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.label13 = new System.Windows.Forms.Label();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.lbUnit = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.toolStrip.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProposerCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShipperCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConfirmorCount)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnApply,
            this.toolStripSeparator1,
            this.btnAuditing,
            this.toolStripSeparator3,
            this.btnShipments,
            this.toolStripSeparator2,
            this.btnExcShipper,
            this.toolStripSeparator5,
            this.btnExcConfirmor,
            this.toolStripSeparator6,
            this.btnReceiving,
            this.toolStripSeparator4,
            this.btnClose});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(833, 25);
            this.toolStrip.TabIndex = 7;
            this.toolStrip.TabStop = true;
            // 
            // btnApply
            // 
            this.btnApply.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnApply.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(91, 22);
            this.btnApply.Tag = "Add";
            this.btnApply.Text = "提交申请(&S)";
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnAuditing
            // 
            this.btnAuditing.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAuditing.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnAuditing.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnAuditing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAuditing.Name = "btnAuditing";
            this.btnAuditing.Size = new System.Drawing.Size(91, 22);
            this.btnAuditing.Tag = "Auditing";
            this.btnAuditing.Text = "主管审核(&A)";
            this.btnAuditing.Click += new System.EventHandler(this.btnAuditing_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnShipments
            // 
            this.btnShipments.Image = global::UniversalControlLibrary.Properties.Resources.match;
            this.btnShipments.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShipments.Name = "btnShipments";
            this.btnShipments.Size = new System.Drawing.Size(103, 22);
            this.btnShipments.Tag = "Confirm_1";
            this.btnShipments.Text = "发货方出库(&F)";
            this.btnShipments.Click += new System.EventHandler(this.btnShipments_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExcShipper
            // 
            this.btnExcShipper.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnExcShipper.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExcShipper.Name = "btnExcShipper";
            this.btnExcShipper.Size = new System.Drawing.Size(103, 22);
            this.btnExcShipper.Tag = "Process_1";
            this.btnExcShipper.Text = "发货人发货(&F)";
            this.btnExcShipper.Click += new System.EventHandler(this.btnExcShipper_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExcConfirmor
            // 
            this.btnExcConfirmor.Image = global::UniversalControlLibrary.Properties.Resources.审核1;
            this.btnExcConfirmor.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExcConfirmor.Name = "btnExcConfirmor";
            this.btnExcConfirmor.Size = new System.Drawing.Size(103, 22);
            this.btnExcConfirmor.Tag = "Process_2";
            this.btnExcConfirmor.Text = "收货人收货(&F)";
            this.btnExcConfirmor.Click += new System.EventHandler(this.btnExcConfirmor_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // btnReceiving
            // 
            this.btnReceiving.Image = global::UniversalControlLibrary.Properties.Resources.审核6;
            this.btnReceiving.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnReceiving.Name = "btnReceiving";
            this.btnReceiving.Size = new System.Drawing.Size(103, 22);
            this.btnReceiving.Tag = "Confirm_2";
            this.btnReceiving.Text = "入库方入库(&Q)";
            this.btnReceiving.Click += new System.EventHandler(this.btnReceiving_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = global::UniversalControlLibrary.Properties.Resources.cancle;
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(67, 22);
            this.btnClose.Tag = "view";
            this.btnClose.Text = "退出(&Q)";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnFindBillNo);
            this.groupBox1.Controls.Add(this.txtScrapBillNo);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.txtReceiving);
            this.groupBox1.Controls.Add(this.txtShipments);
            this.groupBox1.Controls.Add(this.lbBillStatus);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.txtBillRemark);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtBill_ID);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(833, 114);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "申请信息";
            // 
            // btnFindBillNo
            // 
            this.btnFindBillNo.BackColor = System.Drawing.Color.Transparent;
            this.btnFindBillNo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindBillNo.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindBillNo.Image = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindBillNo.Location = new System.Drawing.Point(312, 85);
            this.btnFindBillNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFindBillNo.Name = "btnFindBillNo";
            this.btnFindBillNo.Size = new System.Drawing.Size(21, 21);
            this.btnFindBillNo.TabIndex = 21;
            this.btnFindBillNo.UseVisualStyleBackColor = false;
            this.btnFindBillNo.Click += new System.EventHandler(this.btnFindBillNo_Click);
            // 
            // txtScrapBillNo
            // 
            this.txtScrapBillNo.Location = new System.Drawing.Point(83, 85);
            this.txtScrapBillNo.Name = "txtScrapBillNo";
            this.txtScrapBillNo.ReadOnly = true;
            this.txtScrapBillNo.Size = new System.Drawing.Size(223, 21);
            this.txtScrapBillNo.TabIndex = 20;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(12, 88);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(65, 12);
            this.label17.TabIndex = 19;
            this.label17.Text = "关联报废单";
            // 
            // txtReceiving
            // 
            this.txtReceiving.DataResult = null;
            this.txtReceiving.EditingControlDataGridView = null;
            this.txtReceiving.EditingControlFormattedValue = "";
            this.txtReceiving.EditingControlRowIndex = 0;
            this.txtReceiving.EditingControlValueChanged = false;
            this.txtReceiving.FindItem = UniversalControlLibrary.TextBoxShow.FindType.全系统库房信息;
            this.txtReceiving.Location = new System.Drawing.Point(615, 17);
            this.txtReceiving.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReceiving.Name = "txtReceiving";
            this.txtReceiving.ShowResultForm = true;
            this.txtReceiving.Size = new System.Drawing.Size(184, 21);
            this.txtReceiving.TabIndex = 18;
            this.txtReceiving.TabStop = false;
            this.txtReceiving.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtReceiving_OnCompleteSearch);
            // 
            // txtShipments
            // 
            this.txtShipments.DataResult = null;
            this.txtShipments.EditingControlDataGridView = null;
            this.txtShipments.EditingControlFormattedValue = "";
            this.txtShipments.EditingControlRowIndex = 0;
            this.txtShipments.EditingControlValueChanged = false;
            this.txtShipments.FindItem = UniversalControlLibrary.TextBoxShow.FindType.全系统库房信息;
            this.txtShipments.Location = new System.Drawing.Point(344, 17);
            this.txtShipments.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtShipments.Name = "txtShipments";
            this.txtShipments.ShowResultForm = true;
            this.txtShipments.Size = new System.Drawing.Size(184, 21);
            this.txtShipments.TabIndex = 17;
            this.txtShipments.TabStop = false;
            this.txtShipments.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtShipments_OnCompleteSearch);
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(661, 56);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(65, 12);
            this.lbBillStatus.TabIndex = 16;
            this.lbBillStatus.Text = "BillStatus";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(556, 56);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 15;
            this.label11.Text = "单据状态";
            // 
            // txtBillRemark
            // 
            this.txtBillRemark.Location = new System.Drawing.Point(74, 52);
            this.txtBillRemark.Name = "txtBillRemark";
            this.txtBillRemark.Size = new System.Drawing.Size(454, 21);
            this.txtBillRemark.TabIndex = 14;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(15, 56);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 12;
            this.label12.Text = "调运原因\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(556, 22);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "收 货 方";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(285, 21);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 9;
            this.label2.Text = "发 货 方";
            // 
            // txtBill_ID
            // 
            this.txtBill_ID.ForeColor = System.Drawing.Color.Red;
            this.txtBill_ID.Location = new System.Drawing.Point(74, 18);
            this.txtBill_ID.Name = "txtBill_ID";
            this.txtBill_ID.ReadOnly = true;
            this.txtBill_ID.Size = new System.Drawing.Size(184, 21);
            this.txtBill_ID.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(15, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "单 据 号";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtPhone);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtLogisticsName);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.txtLogisticsBillNo);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 139);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(833, 58);
            this.groupBox2.TabIndex = 9;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "发货信息";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(615, 21);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(184, 21);
            this.txtPhone.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(556, 25);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 12;
            this.label6.Text = "联系电话";
            // 
            // txtLogisticsName
            // 
            this.txtLogisticsName.Location = new System.Drawing.Point(344, 21);
            this.txtLogisticsName.Name = "txtLogisticsName";
            this.txtLogisticsName.Size = new System.Drawing.Size(184, 21);
            this.txtLogisticsName.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(285, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 10;
            this.label5.Text = "物流名称";
            // 
            // txtLogisticsBillNo
            // 
            this.txtLogisticsBillNo.Location = new System.Drawing.Point(74, 21);
            this.txtLogisticsBillNo.Name = "txtLogisticsBillNo";
            this.txtLogisticsBillNo.Size = new System.Drawing.Size(184, 21);
            this.txtLogisticsBillNo.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(15, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 8;
            this.label4.Text = "物流单号";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dataGridView1);
            this.groupBox3.Controls.Add(this.panel1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 197);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(833, 347);
            this.groupBox3.TabIndex = 10;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "物品明细";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 144);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(827, 200);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.替换件ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(107, 26);
            // 
            // 替换件ToolStripMenuItem
            // 
            this.替换件ToolStripMenuItem.Name = "替换件ToolStripMenuItem";
            this.替换件ToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.替换件ToolStripMenuItem.Text = "替换件";
            this.替换件ToolStripMenuItem.Click += new System.EventHandler(this.替换件ToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label15);
            this.panel1.Controls.Add(this.numProposerCount);
            this.panel1.Controls.Add(this.label10);
            this.panel1.Controls.Add(this.numShipperCount);
            this.panel1.Controls.Add(this.label16);
            this.panel1.Controls.Add(this.numConfirmorCount);
            this.panel1.Controls.Add(this.txtListRemark);
            this.panel1.Controls.Add(this.lbStock);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Controls.Add(this.txtGoodsCode);
            this.panel1.Controls.Add(this.label13);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.lbUnit);
            this.panel1.Controls.Add(this.txtSpec);
            this.panel1.Controls.Add(this.label9);
            this.panel1.Controls.Add(this.txtGoodsName);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(3, 17);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(827, 127);
            this.panel1.TabIndex = 0;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(193, 55);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 42;
            this.label15.Text = "申请数量";
            // 
            // numProposerCount
            // 
            this.numProposerCount.DecimalPlaces = 2;
            this.numProposerCount.Location = new System.Drawing.Point(264, 51);
            this.numProposerCount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numProposerCount.Name = "numProposerCount";
            this.numProposerCount.ReadOnly = true;
            this.numProposerCount.Size = new System.Drawing.Size(115, 21);
            this.numProposerCount.TabIndex = 41;
            this.numProposerCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(398, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 40;
            this.label10.Text = "发货数量";
            // 
            // numShipperCount
            // 
            this.numShipperCount.DecimalPlaces = 2;
            this.numShipperCount.Location = new System.Drawing.Point(469, 51);
            this.numShipperCount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numShipperCount.Name = "numShipperCount";
            this.numShipperCount.ReadOnly = true;
            this.numShipperCount.Size = new System.Drawing.Size(115, 21);
            this.numShipperCount.TabIndex = 39;
            this.numShipperCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(610, 55);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(53, 12);
            this.label16.TabIndex = 38;
            this.label16.Text = "收货数量";
            // 
            // numConfirmorCount
            // 
            this.numConfirmorCount.DecimalPlaces = 2;
            this.numConfirmorCount.Location = new System.Drawing.Point(681, 51);
            this.numConfirmorCount.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.numConfirmorCount.Name = "numConfirmorCount";
            this.numConfirmorCount.ReadOnly = true;
            this.numConfirmorCount.Size = new System.Drawing.Size(115, 21);
            this.numConfirmorCount.TabIndex = 37;
            this.numConfirmorCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // txtListRemark
            // 
            this.txtListRemark.Location = new System.Drawing.Point(71, 87);
            this.txtListRemark.Name = "txtListRemark";
            this.txtListRemark.Size = new System.Drawing.Size(454, 21);
            this.txtListRemark.TabIndex = 34;
            // 
            // lbStock
            // 
            this.lbStock.AutoSize = true;
            this.lbStock.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbStock.Location = new System.Drawing.Point(71, 55);
            this.lbStock.Name = "lbStock";
            this.lbStock.Size = new System.Drawing.Size(11, 12);
            this.lbStock.TabIndex = 33;
            this.lbStock.Text = "0";
            this.lbStock.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(12, 55);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 32;
            this.label14.Text = "库存数量";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = false;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.外部库存信息;
            this.txtGoodsCode.Location = new System.Drawing.Point(71, 14);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(184, 21);
            this.txtGoodsCode.TabIndex = 31;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            this.txtGoodsCode.Enter += new System.EventHandler(this.txtGoodsCode_Enter);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(12, 90);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 29;
            this.label13.Text = "备    注";
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(628, 84);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(80, 25);
            this.btnModify.TabIndex = 28;
            this.btnModify.Tag = "add";
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Visible = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(716, 84);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 25);
            this.btnDelete.TabIndex = 27;
            this.btnDelete.Tag = "delete";
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(542, 84);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(80, 25);
            this.btnAdd.TabIndex = 26;
            this.btnAdd.Tag = "add";
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbUnit.Location = new System.Drawing.Point(124, 55);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(29, 12);
            this.lbUnit.TabIndex = 18;
            this.lbUnit.Text = "单位";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(612, 14);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(184, 21);
            this.txtSpec.TabIndex = 15;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(553, 18);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 14;
            this.label9.Text = "规    格";
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Location = new System.Drawing.Point(341, 14);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ReadOnly = true;
            this.txtGoodsName.Size = new System.Drawing.Size(184, 21);
            this.txtGoodsName.TabIndex = 13;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(282, 18);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 12;
            this.label8.Text = "物品名称";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(12, 18);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 10;
            this.label7.Text = "图号型号";
            // 
            // 调运单明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 544);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip);
            this.Name = "调运单明细";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "调运单明细";
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numProposerCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numShipperCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numConfirmorCount)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnApply;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAuditing;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnShipments;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnReceiving;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBill_ID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLogisticsName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLogisticsBillNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbUnit;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label lbStock;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBillRemark;
        private System.Windows.Forms.TextBox txtListRemark;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown numConfirmorCount;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown numProposerCount;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numShipperCount;
        private TextBoxShow txtReceiving;
        private TextBoxShow txtShipments;
        private System.Windows.Forms.ToolStripButton btnExcShipper;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton btnExcConfirmor;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.TextBox txtScrapBillNo;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button btnFindBillNo;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 替换件ToolStripMenuItem;
    }
}