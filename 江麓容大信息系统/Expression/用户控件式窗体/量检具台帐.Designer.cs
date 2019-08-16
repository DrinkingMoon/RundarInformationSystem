using UniversalControlLibrary;
namespace Expression
{
    partial class 量检具台帐
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(量检具台帐));
            this.panel1 = new System.Windows.Forms.Panel();
            this.chbYBF = new System.Windows.Forms.CheckBox();
            this.chbYLY = new System.Windows.Forms.CheckBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.chbBF = new System.Windows.Forms.CheckBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFactoryNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtEffectiveDate = new System.Windows.Forms.TextBox();
            this.btnRelFiles = new System.Windows.Forms.Button();
            this.chbZK = new System.Windows.Forms.CheckBox();
            this.label15 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.dtpMaterialDate = new System.Windows.Forms.DateTimePicker();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.numValidity = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.dtpInputDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtManufacturer = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGaugeCoding = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSpce = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.dataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.F_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DutyUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.cmbCheckType = new UniversalControlLibrary.CustomComboBox(this.components);
            this.cmbGaugeType = new UniversalControlLibrary.CustomComboBox(this.components);
            this.txtCode = new UniversalControlLibrary.TextBoxShow();
            this.txtDutyUser = new UniversalControlLibrary.TextBoxShow();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnOutput = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtDept = new UniversalControlLibrary.TextBoxShow();
            this.label17 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numValidity)).BeginInit();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.chbYBF);
            this.panel1.Controls.Add(this.chbYLY);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 50);
            this.panel1.TabIndex = 56;
            // 
            // chbYBF
            // 
            this.chbYBF.AutoSize = true;
            this.chbYBF.Location = new System.Drawing.Point(839, 16);
            this.chbYBF.Name = "chbYBF";
            this.chbYBF.Size = new System.Drawing.Size(138, 18);
            this.chbYBF.TabIndex = 244;
            this.chbYBF.Text = "包含已报废量检具";
            this.chbYBF.UseVisualStyleBackColor = true;
            this.chbYBF.CheckedChanged += new System.EventHandler(this.btnRefresh_Click);
            // 
            // chbYLY
            // 
            this.chbYLY.AutoSize = true;
            this.chbYLY.Location = new System.Drawing.Point(687, 16);
            this.chbYLY.Name = "chbYLY";
            this.chbYLY.Size = new System.Drawing.Size(138, 18);
            this.chbYLY.TabIndex = 243;
            this.chbYLY.Text = "包含已领用量检具";
            this.chbYLY.UseVisualStyleBackColor = true;
            this.chbYLY.CheckedChanged += new System.EventHandler(this.btnRefresh_Click);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(435, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(147, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "量检具台帐";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDept);
            this.groupBox2.Controls.Add(this.label17);
            this.groupBox2.Controls.Add(this.label16);
            this.groupBox2.Controls.Add(this.chbBF);
            this.groupBox2.Controls.Add(this.cmbCheckType);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.cmbGaugeType);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.txtFactoryNo);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.txtCode);
            this.groupBox2.Controls.Add(this.txtDutyUser);
            this.groupBox2.Controls.Add(this.txtEffectiveDate);
            this.groupBox2.Controls.Add(this.btnRelFiles);
            this.groupBox2.Controls.Add(this.chbZK);
            this.groupBox2.Controls.Add(this.label15);
            this.groupBox2.Controls.Add(this.txtRemark);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.dtpMaterialDate);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.numValidity);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.dtpInputDate);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtManufacturer);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtGaugeCoding);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtSpce);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(0, 75);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1016, 238);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(34, 167);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(65, 12);
            this.label16.TabIndex = 254;
            this.label16.Text = "量检具状态";
            // 
            // chbBF
            // 
            this.chbBF.AutoSize = true;
            this.chbBF.Enabled = false;
            this.chbBF.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chbBF.Location = new System.Drawing.Point(105, 165);
            this.chbBF.Name = "chbBF";
            this.chbBF.Size = new System.Drawing.Size(48, 16);
            this.chbBF.TabIndex = 253;
            this.chbBF.Text = "报废";
            this.chbBF.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(528, 130);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 251;
            this.label12.Text = "校准类别";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(371, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 249;
            this.label4.Text = "量检具类别";
            // 
            // txtFactoryNo
            // 
            this.txtFactoryNo.BackColor = System.Drawing.Color.White;
            this.txtFactoryNo.ForeColor = System.Drawing.Color.Black;
            this.txtFactoryNo.Location = new System.Drawing.Point(442, 89);
            this.txtFactoryNo.Name = "txtFactoryNo";
            this.txtFactoryNo.Size = new System.Drawing.Size(235, 23);
            this.txtFactoryNo.TabIndex = 248;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(371, 94);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 247;
            this.label3.Text = "出厂编号";
            // 
            // txtEffectiveDate
            // 
            this.txtEffectiveDate.BackColor = System.Drawing.Color.White;
            this.txtEffectiveDate.Enabled = false;
            this.txtEffectiveDate.ForeColor = System.Drawing.Color.Black;
            this.txtEffectiveDate.Location = new System.Drawing.Point(772, 125);
            this.txtEffectiveDate.Name = "txtEffectiveDate";
            this.txtEffectiveDate.ReadOnly = true;
            this.txtEffectiveDate.Size = new System.Drawing.Size(205, 23);
            this.txtEffectiveDate.TabIndex = 244;
            // 
            // btnRelFiles
            // 
            this.btnRelFiles.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRelFiles.Location = new System.Drawing.Point(715, 197);
            this.btnRelFiles.Name = "btnRelFiles";
            this.btnRelFiles.Size = new System.Drawing.Size(145, 23);
            this.btnRelFiles.TabIndex = 243;
            this.btnRelFiles.Text = "相关文件";
            this.btnRelFiles.UseVisualStyleBackColor = true;
            this.btnRelFiles.Click += new System.EventHandler(this.btnRelFiles_Click);
            // 
            // chbZK
            // 
            this.chbZK.AutoSize = true;
            this.chbZK.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chbZK.Location = new System.Drawing.Point(184, 165);
            this.chbZK.Name = "chbZK";
            this.chbZK.Size = new System.Drawing.Size(48, 16);
            this.chbZK.TabIndex = 242;
            this.chbZK.Text = "在库";
            this.chbZK.UseVisualStyleBackColor = true;
            this.chbZK.CheckedChanged += new System.EventHandler(this.chbZK_CheckedChanged);
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(497, 130);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(17, 12);
            this.label15.TabIndex = 241;
            this.label15.Text = "月";
            // 
            // txtRemark
            // 
            this.txtRemark.BackColor = System.Drawing.Color.White;
            this.txtRemark.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtRemark.ForeColor = System.Drawing.Color.Black;
            this.txtRemark.Location = new System.Drawing.Point(105, 199);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(572, 21);
            this.txtRemark.TabIndex = 240;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(34, 203);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(53, 12);
            this.label14.TabIndex = 239;
            this.label14.Text = "备    注";
            // 
            // dtpMaterialDate
            // 
            this.dtpMaterialDate.Location = new System.Drawing.Point(304, 162);
            this.dtpMaterialDate.Name = "dtpMaterialDate";
            this.dtpMaterialDate.Size = new System.Drawing.Size(119, 23);
            this.dtpMaterialDate.TabIndex = 238;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(244, 167);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 237;
            this.label13.Text = "领用日期";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(371, 130);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 236;
            this.label11.Text = "校准周期";
            // 
            // numValidity
            // 
            this.numValidity.Location = new System.Drawing.Point(442, 125);
            this.numValidity.Name = "numValidity";
            this.numValidity.Size = new System.Drawing.Size(51, 23);
            this.numValidity.TabIndex = 235;
            this.numValidity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(528, 167);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 233;
            this.label10.Text = "责 任 人";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(713, 130);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 231;
            this.label8.Text = "有效日期";
            // 
            // dtpInputDate
            // 
            this.dtpInputDate.Location = new System.Drawing.Point(105, 125);
            this.dtpInputDate.Name = "dtpInputDate";
            this.dtpInputDate.Size = new System.Drawing.Size(221, 23);
            this.dtpInputDate.TabIndex = 230;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(34, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 229;
            this.label6.Text = "入库日期";
            // 
            // txtManufacturer
            // 
            this.txtManufacturer.BackColor = System.Drawing.Color.White;
            this.txtManufacturer.ForeColor = System.Drawing.Color.Black;
            this.txtManufacturer.Location = new System.Drawing.Point(105, 89);
            this.txtManufacturer.Name = "txtManufacturer";
            this.txtManufacturer.Size = new System.Drawing.Size(221, 23);
            this.txtManufacturer.TabIndex = 224;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(34, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 223;
            this.label2.Text = "制 造 商";
            // 
            // txtGaugeCoding
            // 
            this.txtGaugeCoding.BackColor = System.Drawing.Color.White;
            this.txtGaugeCoding.ForeColor = System.Drawing.Color.Black;
            this.txtGaugeCoding.Location = new System.Drawing.Point(105, 53);
            this.txtGaugeCoding.Name = "txtGaugeCoding";
            this.txtGaugeCoding.Size = new System.Drawing.Size(221, 23);
            this.txtGaugeCoding.TabIndex = 222;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(34, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 221;
            this.label1.Text = "量检具编号";
            // 
            // txtSpce
            // 
            this.txtSpce.BackColor = System.Drawing.Color.White;
            this.txtSpce.ForeColor = System.Drawing.Color.Black;
            this.txtSpce.Location = new System.Drawing.Point(772, 17);
            this.txtSpce.Name = "txtSpce";
            this.txtSpce.ReadOnly = true;
            this.txtSpce.Size = new System.Drawing.Size(205, 23);
            this.txtSpce.TabIndex = 219;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(713, 22);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 218;
            this.label9.Text = "规    格";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(442, 17);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(235, 23);
            this.txtName.TabIndex = 217;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(371, 22);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 216;
            this.label7.Text = "物品名称";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(34, 22);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 215;
            this.label5.Text = "图号型号";
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparator1,
            this.btnUpdate,
            this.toolStripSeparator2,
            this.btnDelete,
            this.toolStripSeparator7,
            this.btnRefresh,
            this.toolStripSeparator3,
            this.btnOutput});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 55;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::Expression.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(68, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = ((System.Drawing.Image)(resources.GetObject("btnUpdate.Image")));
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(69, 22);
            this.btnUpdate.Tag = "Update";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::Expression.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(69, 22);
            this.btnDelete.Tag = "Delete";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = ((System.Drawing.Image)(resources.GetObject("btnRefresh.Image")));
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(92, 22);
            this.btnRefresh.Tag = "View";
            this.btnRefresh.Text = "刷新数据(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.AutoCreateFilters = true;
            this.dataGridView1.BaseFilter = "";
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.F_Id,
            this.DutyUser});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 345);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 391);
            this.dataGridView1.TabIndex = 58;
            this.dataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // F_Id
            // 
            this.F_Id.DataPropertyName = "F_Id";
            this.F_Id.HeaderText = "F_Id";
            this.F_Id.Name = "F_Id";
            this.F_Id.ReadOnly = true;
            this.F_Id.Visible = false;
            // 
            // DutyUser
            // 
            this.DutyUser.DataPropertyName = "DutyUser";
            this.DutyUser.HeaderText = "DutyUser";
            this.DutyUser.Name = "DutyUser";
            this.DutyUser.ReadOnly = true;
            this.DutyUser.Visible = false;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 313);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1016, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 57;
            // 
            // cmbCheckType
            // 
            this.cmbCheckType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCheckType.FormattingEnabled = true;
            this.cmbCheckType.Items.AddRange(new object[] {
            "内校",
            "外校",
            "游校"});
            this.cmbCheckType.Location = new System.Drawing.Point(588, 126);
            this.cmbCheckType.MaxYear = 0;
            this.cmbCheckType.MinYear = 0;
            this.cmbCheckType.Name = "cmbCheckType";
            this.cmbCheckType.Size = new System.Drawing.Size(89, 21);
            this.cmbCheckType.TabIndex = 252;
            // 
            // cmbGaugeType
            // 
            this.cmbGaugeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbGaugeType.FormattingEnabled = true;
            this.cmbGaugeType.Items.AddRange(new object[] {
            "A类",
            "B类",
            "C类"});
            this.cmbGaugeType.Location = new System.Drawing.Point(442, 54);
            this.cmbGaugeType.MaxYear = 0;
            this.cmbGaugeType.MinYear = 0;
            this.cmbGaugeType.Name = "cmbGaugeType";
            this.cmbGaugeType.Size = new System.Drawing.Size(107, 21);
            this.cmbGaugeType.TabIndex = 250;
            this.cmbGaugeType.SelectedIndexChanged += new System.EventHandler(this.cmbGaugeType_SelectedIndexChanged);
            // 
            // txtCode
            // 
            this.txtCode.DataResult = null;
            this.txtCode.DataTableResult = null;
            this.txtCode.EditingControlDataGridView = null;
            this.txtCode.EditingControlFormattedValue = "";
            this.txtCode.EditingControlRowIndex = 0;
            this.txtCode.EditingControlValueChanged = true;
            this.txtCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtCode.IsMultiSelect = false;
            this.txtCode.Location = new System.Drawing.Point(105, 17);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.ShowResultForm = true;
            this.txtCode.Size = new System.Drawing.Size(221, 23);
            this.txtCode.StrEndSql = null;
            this.txtCode.TabIndex = 246;
            this.txtCode.TabStop = false;
            this.txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtCode_OnCompleteSearch);
            // 
            // txtDutyUser
            // 
            this.txtDutyUser.DataResult = null;
            this.txtDutyUser.DataTableResult = null;
            this.txtDutyUser.EditingControlDataGridView = null;
            this.txtDutyUser.EditingControlFormattedValue = "";
            this.txtDutyUser.EditingControlRowIndex = 0;
            this.txtDutyUser.EditingControlValueChanged = true;
            this.txtDutyUser.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtDutyUser.IsMultiSelect = false;
            this.txtDutyUser.Location = new System.Drawing.Point(588, 162);
            this.txtDutyUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDutyUser.Name = "txtDutyUser";
            this.txtDutyUser.ShowResultForm = true;
            this.txtDutyUser.Size = new System.Drawing.Size(89, 23);
            this.txtDutyUser.StrEndSql = null;
            this.txtDutyUser.TabIndex = 245;
            this.txtDutyUser.TabStop = false;
            this.txtDutyUser.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtDutyUser_OnCompleteSearch);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "F_Id";
            this.dataGridViewTextBoxColumn1.HeaderText = "F_Id";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Visible = false;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "DutyUser";
            this.dataGridViewTextBoxColumn2.HeaderText = "DutyUser";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Visible = false;
            // 
            // btnOutput
            // 
            this.btnOutput.Image = global::Expression.Properties.Resources.Excel;
            this.btnOutput.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOutput.Name = "btnOutput";
            this.btnOutput.Size = new System.Drawing.Size(96, 22);
            this.btnOutput.Tag = "View";
            this.btnOutput.Text = "导出Excel(&E)";
            this.btnOutput.Click += new System.EventHandler(this.btnOutput_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // txtDept
            // 
            this.txtDept.DataResult = null;
            this.txtDept.DataTableResult = null;
            this.txtDept.EditingControlDataGridView = null;
            this.txtDept.EditingControlFormattedValue = "";
            this.txtDept.EditingControlRowIndex = 0;
            this.txtDept.EditingControlValueChanged = true;
            this.txtDept.Enabled = false;
            this.txtDept.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtDept.IsMultiSelect = false;
            this.txtDept.Location = new System.Drawing.Point(772, 162);
            this.txtDept.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDept.Name = "txtDept";
            this.txtDept.ShowResultForm = true;
            this.txtDept.Size = new System.Drawing.Size(89, 23);
            this.txtDept.StrEndSql = null;
            this.txtDept.TabIndex = 256;
            this.txtDept.TabStop = false;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label17.Location = new System.Drawing.Point(712, 167);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 12);
            this.label17.TabIndex = 255;
            this.label17.Text = "责任部门";
            // 
            // 量检具台帐
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "量检具台帐";
            this.Load += new System.EventHandler(this.量检具台帐_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numValidity)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.TextBox txtSpce;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtManufacturer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtGaugeCoding;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DateTimePicker dtpInputDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpMaterialDate;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numValidity;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chbZK;
        private System.Windows.Forms.CheckBox chbYLY;
        private System.Windows.Forms.Button btnRelFiles;
        private System.Windows.Forms.TextBox txtEffectiveDate;
        private System.Windows.Forms.CheckBox chbYBF;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private TextBoxShow txtCode;
        private TextBoxShow txtDutyUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private CustomComboBox cmbCheckType;
        private System.Windows.Forms.Label label12;
        private CustomComboBox cmbGaugeType;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFactoryNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chbBF;
        private System.Windows.Forms.Label label16;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private CustomDataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn DutyUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnOutput;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private TextBoxShow txtDept;
        private System.Windows.Forms.Label label17;
    }
}
