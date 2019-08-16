using UniversalControlLibrary;
namespace Expression
{
    partial class UserControlReceiveSendSaveGatherBill
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
            this.panelMain = new System.Windows.Forms.Panel();
            this.panelCenter = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.月度账务 = new System.Windows.Forms.TabPage();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.收发存 = new System.Windows.Forms.TabPage();
            this.dgv_SFC = new UniversalControlLibrary.CustomDataGridView();
            this.usdl_SFC = new UniversalControlLibrary.UserControlDataLocalizer();
            this.入账明细 = new System.Windows.Forms.TabPage();
            this.dgv_Input = new UniversalControlLibrary.CustomDataGridView();
            this.usdl_Input = new UniversalControlLibrary.UserControlDataLocalizer();
            this.出账明细 = new System.Windows.Forms.TabPage();
            this.dgv_Output = new UniversalControlLibrary.CustomDataGridView();
            this.ucdl_Output = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panelPara = new System.Windows.Forms.Panel();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.rbSingleStorage = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.rbAccountStorage = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbMonth = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbYear = new System.Windows.Forms.ComboBox();
            this.btnOutExcel = new System.Windows.Forms.Button();
            this.btnOk = new System.Windows.Forms.Button();
            this.业务明细 = new System.Windows.Forms.TabPage();
            this.dgv_BusDetail = new UniversalControlLibrary.CustomDataGridView();
            this.ucdlBusDetail = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbAll = new System.Windows.Forms.RadioButton();
            this.cmbSelectType = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStartTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDG = new System.Windows.Forms.ComboBox();
            this.rbDG = new System.Windows.Forms.RadioButton();
            this.label4 = new System.Windows.Forms.Label();
            this.rbZW = new System.Windows.Forms.RadioButton();
            this.btnOutput = new System.Windows.Forms.Button();
            this.btnSelect = new System.Windows.Forms.Button();
            this.月度结存 = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.挂账表 = new System.Windows.Forms.TabPage();
            this.dgv_GZB = new UniversalControlLibrary.CustomDataGridView();
            this.usdl_GZB = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panelMain.SuspendLayout();
            this.panelCenter.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.月度账务.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.收发存.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SFC)).BeginInit();
            this.入账明细.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Input)).BeginInit();
            this.出账明细.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Output)).BeginInit();
            this.panelPara.SuspendLayout();
            this.业务明细.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BusDetail)).BeginInit();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.挂账表.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_GZB)).BeginInit();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panelMain.Controls.Add(this.panelCenter);
            this.panelMain.Controls.Add(this.panel1);
            this.panelMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelMain.Location = new System.Drawing.Point(0, 0);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(1008, 680);
            this.panelMain.TabIndex = 37;
            // 
            // panelCenter
            // 
            this.panelCenter.BackColor = System.Drawing.Color.Transparent;
            this.panelCenter.Controls.Add(this.tabControl1);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 49);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1008, 631);
            this.panelCenter.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.月度账务);
            this.tabControl1.Controls.Add(this.业务明细);
            this.tabControl1.Controls.Add(this.月度结存);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1008, 631);
            this.tabControl1.TabIndex = 0;
            // 
            // 月度账务
            // 
            this.月度账务.Controls.Add(this.tabControl2);
            this.月度账务.Controls.Add(this.panelPara);
            this.月度账务.Location = new System.Drawing.Point(4, 24);
            this.月度账务.Name = "月度账务";
            this.月度账务.Padding = new System.Windows.Forms.Padding(3);
            this.月度账务.Size = new System.Drawing.Size(1000, 603);
            this.月度账务.TabIndex = 0;
            this.月度账务.Text = "月度账务";
            this.月度账务.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Controls.Add(this.收发存);
            this.tabControl2.Controls.Add(this.入账明细);
            this.tabControl2.Controls.Add(this.出账明细);
            this.tabControl2.Controls.Add(this.挂账表);
            this.tabControl2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl2.Location = new System.Drawing.Point(3, 53);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(994, 547);
            this.tabControl2.TabIndex = 36;
            // 
            // 收发存
            // 
            this.收发存.Controls.Add(this.dgv_SFC);
            this.收发存.Controls.Add(this.usdl_SFC);
            this.收发存.Location = new System.Drawing.Point(4, 24);
            this.收发存.Name = "收发存";
            this.收发存.Padding = new System.Windows.Forms.Padding(3);
            this.收发存.Size = new System.Drawing.Size(986, 519);
            this.收发存.TabIndex = 0;
            this.收发存.Text = "收发存";
            this.收发存.UseVisualStyleBackColor = true;
            // 
            // dgv_SFC
            // 
            this.dgv_SFC.AllowUserToAddRows = false;
            this.dgv_SFC.AllowUserToDeleteRows = false;
            this.dgv_SFC.AllowUserToResizeRows = false;
            this.dgv_SFC.AutoCreateFilters = true;
            this.dgv_SFC.BaseFilter = "";
            this.dgv_SFC.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_SFC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_SFC.Location = new System.Drawing.Point(3, 35);
            this.dgv_SFC.Name = "dgv_SFC";
            this.dgv_SFC.RowTemplate.Height = 23;
            this.dgv_SFC.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_SFC.Size = new System.Drawing.Size(980, 481);
            this.dgv_SFC.TabIndex = 33;
            // 
            // usdl_SFC
            // 
            this.usdl_SFC.Dock = System.Windows.Forms.DockStyle.Top;
            this.usdl_SFC.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.usdl_SFC.Location = new System.Drawing.Point(3, 3);
            this.usdl_SFC.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usdl_SFC.Name = "usdl_SFC";
            this.usdl_SFC.OnlyLocalize = false;
            this.usdl_SFC.Size = new System.Drawing.Size(980, 32);
            this.usdl_SFC.StartIndex = 0;
            this.usdl_SFC.TabIndex = 32;
            // 
            // 入账明细
            // 
            this.入账明细.Controls.Add(this.dgv_Input);
            this.入账明细.Controls.Add(this.usdl_Input);
            this.入账明细.Location = new System.Drawing.Point(4, 24);
            this.入账明细.Name = "入账明细";
            this.入账明细.Padding = new System.Windows.Forms.Padding(3);
            this.入账明细.Size = new System.Drawing.Size(986, 519);
            this.入账明细.TabIndex = 1;
            this.入账明细.Text = "入账明细";
            this.入账明细.UseVisualStyleBackColor = true;
            // 
            // dgv_Input
            // 
            this.dgv_Input.AllowUserToAddRows = false;
            this.dgv_Input.AllowUserToDeleteRows = false;
            this.dgv_Input.AllowUserToResizeRows = false;
            this.dgv_Input.AutoCreateFilters = true;
            this.dgv_Input.BaseFilter = "";
            this.dgv_Input.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Input.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Input.Location = new System.Drawing.Point(3, 35);
            this.dgv_Input.Name = "dgv_Input";
            this.dgv_Input.RowTemplate.Height = 23;
            this.dgv_Input.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Input.Size = new System.Drawing.Size(980, 481);
            this.dgv_Input.TabIndex = 35;
            // 
            // usdl_Input
            // 
            this.usdl_Input.Dock = System.Windows.Forms.DockStyle.Top;
            this.usdl_Input.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.usdl_Input.Location = new System.Drawing.Point(3, 3);
            this.usdl_Input.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usdl_Input.Name = "usdl_Input";
            this.usdl_Input.OnlyLocalize = false;
            this.usdl_Input.Size = new System.Drawing.Size(980, 32);
            this.usdl_Input.StartIndex = 0;
            this.usdl_Input.TabIndex = 34;
            // 
            // 出账明细
            // 
            this.出账明细.Controls.Add(this.dgv_Output);
            this.出账明细.Controls.Add(this.ucdl_Output);
            this.出账明细.Location = new System.Drawing.Point(4, 24);
            this.出账明细.Name = "出账明细";
            this.出账明细.Size = new System.Drawing.Size(986, 519);
            this.出账明细.TabIndex = 2;
            this.出账明细.Text = "出账明细";
            this.出账明细.UseVisualStyleBackColor = true;
            // 
            // dgv_Output
            // 
            this.dgv_Output.AllowUserToAddRows = false;
            this.dgv_Output.AllowUserToDeleteRows = false;
            this.dgv_Output.AllowUserToResizeRows = false;
            this.dgv_Output.AutoCreateFilters = true;
            this.dgv_Output.BaseFilter = "";
            this.dgv_Output.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Output.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_Output.Location = new System.Drawing.Point(0, 32);
            this.dgv_Output.Name = "dgv_Output";
            this.dgv_Output.RowTemplate.Height = 23;
            this.dgv_Output.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Output.Size = new System.Drawing.Size(986, 487);
            this.dgv_Output.TabIndex = 35;
            // 
            // ucdl_Output
            // 
            this.ucdl_Output.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucdl_Output.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucdl_Output.Location = new System.Drawing.Point(0, 0);
            this.ucdl_Output.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucdl_Output.Name = "ucdl_Output";
            this.ucdl_Output.OnlyLocalize = false;
            this.ucdl_Output.Size = new System.Drawing.Size(986, 32);
            this.ucdl_Output.StartIndex = 0;
            this.ucdl_Output.TabIndex = 34;
            // 
            // panelPara
            // 
            this.panelPara.BackColor = System.Drawing.SystemColors.Control;
            this.panelPara.Controls.Add(this.cmbStorage);
            this.panelPara.Controls.Add(this.rbSingleStorage);
            this.panelPara.Controls.Add(this.label3);
            this.panelPara.Controls.Add(this.rbAccountStorage);
            this.panelPara.Controls.Add(this.label2);
            this.panelPara.Controls.Add(this.cmbMonth);
            this.panelPara.Controls.Add(this.label1);
            this.panelPara.Controls.Add(this.cmbYear);
            this.panelPara.Controls.Add(this.btnOutExcel);
            this.panelPara.Controls.Add(this.btnOk);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(3, 3);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(994, 50);
            this.panelPara.TabIndex = 35;
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.Enabled = false;
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(599, 15);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(120, 22);
            this.cmbStorage.TabIndex = 172;
            // 
            // rbSingleStorage
            // 
            this.rbSingleStorage.AutoSize = true;
            this.rbSingleStorage.Location = new System.Drawing.Point(512, 16);
            this.rbSingleStorage.Name = "rbSingleStorage";
            this.rbSingleStorage.Size = new System.Drawing.Size(81, 18);
            this.rbSingleStorage.TabIndex = 171;
            this.rbSingleStorage.TabStop = true;
            this.rbSingleStorage.Text = "单个库房";
            this.rbSingleStorage.UseVisualStyleBackColor = true;
            this.rbSingleStorage.CheckedChanged += new System.EventHandler(this.rbSingleStorage_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(332, 18);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 170;
            this.label3.Text = "查询类型";
            // 
            // rbAccountStorage
            // 
            this.rbAccountStorage.AutoSize = true;
            this.rbAccountStorage.Checked = true;
            this.rbAccountStorage.Location = new System.Drawing.Point(413, 16);
            this.rbAccountStorage.Name = "rbAccountStorage";
            this.rbAccountStorage.Size = new System.Drawing.Size(81, 18);
            this.rbAccountStorage.TabIndex = 169;
            this.rbAccountStorage.TabStop = true;
            this.rbAccountStorage.Text = "账务库房";
            this.rbAccountStorage.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 14);
            this.label2.TabIndex = 168;
            this.label2.Text = "月份";
            // 
            // cmbMonth
            // 
            this.cmbMonth.FormattingEnabled = true;
            this.cmbMonth.Items.AddRange(new object[] {
            "01",
            "02",
            "03",
            "04",
            "05",
            "06",
            "07",
            "08",
            "09",
            "10",
            "11",
            "12"});
            this.cmbMonth.Location = new System.Drawing.Point(208, 15);
            this.cmbMonth.Name = "cmbMonth";
            this.cmbMonth.Size = new System.Drawing.Size(76, 22);
            this.cmbMonth.TabIndex = 167;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 14);
            this.label1.TabIndex = 166;
            this.label1.Text = "年份";
            // 
            // cmbYear
            // 
            this.cmbYear.FormattingEnabled = true;
            this.cmbYear.Location = new System.Drawing.Point(70, 15);
            this.cmbYear.Name = "cmbYear";
            this.cmbYear.Size = new System.Drawing.Size(76, 22);
            this.cmbYear.TabIndex = 165;
            // 
            // btnOutExcel
            // 
            this.btnOutExcel.Location = new System.Drawing.Point(901, 12);
            this.btnOutExcel.Name = "btnOutExcel";
            this.btnOutExcel.Size = new System.Drawing.Size(87, 27);
            this.btnOutExcel.TabIndex = 164;
            this.btnOutExcel.Text = "报表导出";
            this.btnOutExcel.UseVisualStyleBackColor = true;
            this.btnOutExcel.Click += new System.EventHandler(this.btnOutExcel_Click);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(787, 12);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(87, 27);
            this.btnOk.TabIndex = 156;
            this.btnOk.Text = "查询";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // 业务明细
            // 
            this.业务明细.Controls.Add(this.dgv_BusDetail);
            this.业务明细.Controls.Add(this.ucdlBusDetail);
            this.业务明细.Controls.Add(this.panel2);
            this.业务明细.Location = new System.Drawing.Point(4, 24);
            this.业务明细.Name = "业务明细";
            this.业务明细.Padding = new System.Windows.Forms.Padding(3);
            this.业务明细.Size = new System.Drawing.Size(1000, 603);
            this.业务明细.TabIndex = 1;
            this.业务明细.Text = "业务明细";
            this.业务明细.UseVisualStyleBackColor = true;
            // 
            // dgv_BusDetail
            // 
            this.dgv_BusDetail.AllowUserToAddRows = false;
            this.dgv_BusDetail.AllowUserToDeleteRows = false;
            this.dgv_BusDetail.AllowUserToResizeRows = false;
            this.dgv_BusDetail.AutoCreateFilters = true;
            this.dgv_BusDetail.BaseFilter = "";
            this.dgv_BusDetail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_BusDetail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_BusDetail.Location = new System.Drawing.Point(3, 142);
            this.dgv_BusDetail.Name = "dgv_BusDetail";
            this.dgv_BusDetail.RowTemplate.Height = 23;
            this.dgv_BusDetail.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_BusDetail.Size = new System.Drawing.Size(994, 458);
            this.dgv_BusDetail.TabIndex = 40;
            // 
            // ucdlBusDetail
            // 
            this.ucdlBusDetail.Dock = System.Windows.Forms.DockStyle.Top;
            this.ucdlBusDetail.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ucdlBusDetail.Location = new System.Drawing.Point(3, 110);
            this.ucdlBusDetail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ucdlBusDetail.Name = "ucdlBusDetail";
            this.ucdlBusDetail.OnlyLocalize = false;
            this.ucdlBusDetail.Size = new System.Drawing.Size(994, 32);
            this.ucdlBusDetail.StartIndex = 0;
            this.ucdlBusDetail.TabIndex = 39;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.Control;
            this.panel2.Controls.Add(this.rbAll);
            this.panel2.Controls.Add(this.cmbSelectType);
            this.panel2.Controls.Add(this.label7);
            this.panel2.Controls.Add(this.dtpEndTime);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.dtpStartTime);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.cmbDG);
            this.panel2.Controls.Add(this.rbDG);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.rbZW);
            this.panel2.Controls.Add(this.btnOutput);
            this.panel2.Controls.Add(this.btnSelect);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(994, 107);
            this.panel2.TabIndex = 36;
            // 
            // rbAll
            // 
            this.rbAll.AutoSize = true;
            this.rbAll.Checked = true;
            this.rbAll.Location = new System.Drawing.Point(408, 65);
            this.rbAll.Name = "rbAll";
            this.rbAll.Size = new System.Drawing.Size(67, 18);
            this.rbAll.TabIndex = 179;
            this.rbAll.TabStop = true;
            this.rbAll.Text = "全库房";
            this.rbAll.UseVisualStyleBackColor = true;
            // 
            // cmbSelectType
            // 
            this.cmbSelectType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectType.FormattingEnabled = true;
            this.cmbSelectType.Items.AddRange(new object[] {
            "入库明细",
            "出库明细",
            "供应商应付账款明细",
            "样品不付款入库明细",
            "样品不付款出库明细"});
            this.cmbSelectType.Location = new System.Drawing.Point(107, 63);
            this.cmbSelectType.Name = "cmbSelectType";
            this.cmbSelectType.Size = new System.Drawing.Size(149, 22);
            this.cmbSelectType.TabIndex = 178;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(16, 67);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 177;
            this.label7.Text = "查询类型";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(408, 19);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(149, 23);
            this.dtpEndTime.TabIndex = 176;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(317, 23);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 175;
            this.label6.Text = "截止时间";
            // 
            // dtpStartTime
            // 
            this.dtpStartTime.Location = new System.Drawing.Point(107, 19);
            this.dtpStartTime.Name = "dtpStartTime";
            this.dtpStartTime.Size = new System.Drawing.Size(149, 23);
            this.dtpStartTime.TabIndex = 174;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 173;
            this.label5.Text = "起始时间";
            // 
            // cmbDG
            // 
            this.cmbDG.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDG.Enabled = false;
            this.cmbDG.FormattingEnabled = true;
            this.cmbDG.Location = new System.Drawing.Point(675, 63);
            this.cmbDG.Name = "cmbDG";
            this.cmbDG.Size = new System.Drawing.Size(120, 22);
            this.cmbDG.TabIndex = 172;
            // 
            // rbDG
            // 
            this.rbDG.AutoSize = true;
            this.rbDG.Location = new System.Drawing.Point(590, 65);
            this.rbDG.Name = "rbDG";
            this.rbDG.Size = new System.Drawing.Size(81, 18);
            this.rbDG.TabIndex = 171;
            this.rbDG.Text = "单个库房";
            this.rbDG.UseVisualStyleBackColor = true;
            this.rbDG.CheckedChanged += new System.EventHandler(this.rbDG_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(317, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 170;
            this.label4.Text = "库房类型";
            // 
            // rbZW
            // 
            this.rbZW.AutoSize = true;
            this.rbZW.Location = new System.Drawing.Point(492, 65);
            this.rbZW.Name = "rbZW";
            this.rbZW.Size = new System.Drawing.Size(81, 18);
            this.rbZW.TabIndex = 169;
            this.rbZW.Text = "账务库房";
            this.rbZW.UseVisualStyleBackColor = true;
            // 
            // btnOutput
            // 
            this.btnOutput.Location = new System.Drawing.Point(892, 17);
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(87, 27);
            this.btnOutput.TabIndex = 164;
            this.btnOutput.Text = "报表导出";
            this.btnOutput.UseVisualStyleBackColor = true;
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // btnSelect
            // 
            this.btnSelect.Location = new System.Drawing.Point(766, 17);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(87, 27);
            this.btnSelect.TabIndex = 156;
            this.btnSelect.Text = "查询";
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // 月度结存
            // 
            this.月度结存.Location = new System.Drawing.Point(4, 24);
            this.月度结存.Name = "月度结存";
            this.月度结存.Padding = new System.Windows.Forms.Padding(3);
            this.月度结存.Size = new System.Drawing.Size(1000, 603);
            this.月度结存.TabIndex = 2;
            this.月度结存.Text = "月度结存";
            this.月度结存.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1008, 49);
            this.panel1.TabIndex = 24;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(444, 13);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "账务查询";
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
            // 挂账表
            // 
            this.挂账表.Controls.Add(this.dgv_GZB);
            this.挂账表.Controls.Add(this.usdl_GZB);
            this.挂账表.Location = new System.Drawing.Point(4, 24);
            this.挂账表.Name = "挂账表";
            this.挂账表.Size = new System.Drawing.Size(986, 519);
            this.挂账表.TabIndex = 3;
            this.挂账表.Text = "挂账表";
            this.挂账表.UseVisualStyleBackColor = true;
            // 
            // dgv_GZB
            // 
            this.dgv_GZB.AllowUserToAddRows = false;
            this.dgv_GZB.AllowUserToDeleteRows = false;
            this.dgv_GZB.AllowUserToResizeRows = false;
            this.dgv_GZB.AutoCreateFilters = true;
            this.dgv_GZB.BaseFilter = "";
            this.dgv_GZB.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_GZB.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_GZB.Location = new System.Drawing.Point(0, 32);
            this.dgv_GZB.Name = "dgv_GZB";
            this.dgv_GZB.RowTemplate.Height = 23;
            this.dgv_GZB.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_GZB.Size = new System.Drawing.Size(986, 487);
            this.dgv_GZB.TabIndex = 35;
            // 
            // usdl_GZB
            // 
            this.usdl_GZB.Dock = System.Windows.Forms.DockStyle.Top;
            this.usdl_GZB.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.usdl_GZB.Location = new System.Drawing.Point(0, 0);
            this.usdl_GZB.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.usdl_GZB.Name = "usdl_GZB";
            this.usdl_GZB.OnlyLocalize = false;
            this.usdl_GZB.Size = new System.Drawing.Size(986, 32);
            this.usdl_GZB.StartIndex = 0;
            this.usdl_GZB.TabIndex = 34;
            // 
            // UserControlReceiveSendSaveGatherBill
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 680);
            this.Controls.Add(this.panelMain);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlReceiveSendSaveGatherBill";
            this.Load += new System.EventHandler(this.UserControlReceiveSendSaveGatherBill_Load);
            this.panelMain.ResumeLayout(false);
            this.panelCenter.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.月度账务.ResumeLayout(false);
            this.tabControl2.ResumeLayout(false);
            this.收发存.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_SFC)).EndInit();
            this.入账明细.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Input)).EndInit();
            this.出账明细.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Output)).EndInit();
            this.panelPara.ResumeLayout(false);
            this.panelPara.PerformLayout();
            this.业务明细.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_BusDetail)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.挂账表.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_GZB)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage 月度账务;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage 收发存;
        private CustomDataGridView dgv_SFC;
        private UserControlDataLocalizer usdl_SFC;
        private System.Windows.Forms.TabPage 入账明细;
        private CustomDataGridView dgv_Input;
        private UserControlDataLocalizer usdl_Input;
        private System.Windows.Forms.TabPage 出账明细;
        private CustomDataGridView dgv_Output;
        private UserControlDataLocalizer ucdl_Output;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.RadioButton rbSingleStorage;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rbAccountStorage;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbYear;
        private System.Windows.Forms.Button btnOutExcel;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.TabPage 业务明细;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DateTimePicker dtpStartTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDG;
        private System.Windows.Forms.RadioButton rbDG;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton rbZW;
        private System.Windows.Forms.Button btnOutput;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.ComboBox cmbSelectType;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label6;
        private CustomDataGridView dgv_BusDetail;
        private UserControlDataLocalizer ucdlBusDetail;
        private System.Windows.Forms.RadioButton rbAll;
        private System.Windows.Forms.TabPage 月度结存;
        private System.Windows.Forms.TabPage 挂账表;
        private CustomDataGridView dgv_GZB;
        private UserControlDataLocalizer usdl_GZB;
    }
}
