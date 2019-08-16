namespace Form_Manufacture_Storage
{
    partial class 发料清单编制明细界面
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
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chbDelete = new System.Windows.Forms.CheckBox();
            this.chbOnceTheWholeIssue = new System.Windows.Forms.CheckBox();
            this.cmbApplicable = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.btn_Import = new System.Windows.Forms.Button();
            this.lbSDBStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtSDBNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtEditionCode = new UniversalControlLibrary.TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.numRedices = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.txtCode = new UniversalControlLibrary.TextBoxShow();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.customPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRedices)).BeginInit();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.AutoScroll = true;
            this.customPanel1.Controls.Add(this.dataGridView1);
            this.customPanel1.Controls.Add(this.userControlDataLocalizer1);
            this.customPanel1.Controls.Add(this.groupBox1);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(816, 552);
            this.customPanel1.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 222);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(816, 330);
            this.dataGridView1.TabIndex = 5;
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择ToolStripMenuItem,
            this.取消ToolStripMenuItem,
            this.全选ToolStripMenuItem,
            this.全消ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(101, 92);
            // 
            // 选择ToolStripMenuItem
            // 
            this.选择ToolStripMenuItem.Name = "选择ToolStripMenuItem";
            this.选择ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.选择ToolStripMenuItem.Text = "选择";
            this.选择ToolStripMenuItem.Click += new System.EventHandler(this.选择ToolStripMenuItem_Click);
            // 
            // 取消ToolStripMenuItem
            // 
            this.取消ToolStripMenuItem.Name = "取消ToolStripMenuItem";
            this.取消ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.取消ToolStripMenuItem.Text = "取消";
            this.取消ToolStripMenuItem.Click += new System.EventHandler(this.取消ToolStripMenuItem_Click);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.全选ToolStripMenuItem.Text = "全选";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 全消ToolStripMenuItem
            // 
            this.全消ToolStripMenuItem.Name = "全消ToolStripMenuItem";
            this.全消ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.全消ToolStripMenuItem.Text = "全消";
            this.全消ToolStripMenuItem.Click += new System.EventHandler(this.全消ToolStripMenuItem_Click);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 190);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = true;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(816, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 4;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chbDelete);
            this.groupBox1.Controls.Add(this.chbOnceTheWholeIssue);
            this.groupBox1.Controls.Add(this.cmbApplicable);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.btn_Import);
            this.groupBox1.Controls.Add(this.lbSDBStatus);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtSDBNo);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnModify);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtEditionCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.numRedices);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(816, 190);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "数据录入";
            // 
            // chbDelete
            // 
            this.chbDelete.AutoSize = true;
            this.chbDelete.Location = new System.Drawing.Point(558, 69);
            this.chbDelete.Name = "chbDelete";
            this.chbDelete.Size = new System.Drawing.Size(180, 18);
            this.chbDelete.TabIndex = 314;
            this.chbDelete.Text = "删除当前总成的发料清单";
            this.chbDelete.UseVisualStyleBackColor = true;
            this.chbDelete.CheckedChanged += new System.EventHandler(this.chbDelete_CheckedChanged);
            // 
            // chbOnceTheWholeIssue
            // 
            this.chbOnceTheWholeIssue.AutoSize = true;
            this.chbOnceTheWholeIssue.Location = new System.Drawing.Point(282, 151);
            this.chbOnceTheWholeIssue.Name = "chbOnceTheWholeIssue";
            this.chbOnceTheWholeIssue.Size = new System.Drawing.Size(124, 18);
            this.chbOnceTheWholeIssue.TabIndex = 313;
            this.chbOnceTheWholeIssue.Text = "一次性整台发料";
            this.chbOnceTheWholeIssue.UseVisualStyleBackColor = true;
            // 
            // cmbApplicable
            // 
            this.cmbApplicable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbApplicable.FormattingEnabled = true;
            this.cmbApplicable.Location = new System.Drawing.Point(84, 65);
            this.cmbApplicable.Name = "cmbApplicable";
            this.cmbApplicable.Size = new System.Drawing.Size(177, 22);
            this.cmbApplicable.TabIndex = 312;
            this.cmbApplicable.SelectedIndexChanged += new System.EventHandler(this.cmbApplicable_SelectedIndexChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(12, 68);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 311;
            this.label6.Text = "适用范围";
            // 
            // btn_Import
            // 
            this.btn_Import.Location = new System.Drawing.Point(729, 146);
            this.btn_Import.Name = "btn_Import";
            this.btn_Import.Size = new System.Drawing.Size(75, 26);
            this.btn_Import.TabIndex = 310;
            this.btn_Import.Text = "导入";
            this.btn_Import.UseVisualStyleBackColor = true;
            this.btn_Import.Click += new System.EventHandler(this.btn_Import_Click);
            // 
            // lbSDBStatus
            // 
            this.lbSDBStatus.AutoSize = true;
            this.lbSDBStatus.ForeColor = System.Drawing.Color.Red;
            this.lbSDBStatus.Location = new System.Drawing.Point(393, 27);
            this.lbSDBStatus.Name = "lbSDBStatus";
            this.lbSDBStatus.Size = new System.Drawing.Size(70, 14);
            this.lbSDBStatus.TabIndex = 309;
            this.lbSDBStatus.Text = "SDBStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(280, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 308;
            this.label5.Text = "业务状态";
            // 
            // txtSDBNo
            // 
            this.txtSDBNo.BackColor = System.Drawing.Color.White;
            this.txtSDBNo.ForeColor = System.Drawing.Color.Red;
            this.txtSDBNo.Location = new System.Drawing.Point(84, 24);
            this.txtSDBNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSDBNo.Name = "txtSDBNo";
            this.txtSDBNo.ReadOnly = true;
            this.txtSDBNo.Size = new System.Drawing.Size(181, 23);
            this.txtSDBNo.TabIndex = 306;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 307;
            this.label4.Text = "业务编号";
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(561, 146);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 26);
            this.btnModify.TabIndex = 305;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(645, 146);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 26);
            this.btnDelete.TabIndex = 304;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(477, 146);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 26);
            this.btnAdd.TabIndex = 303;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtEditionCode
            // 
            this.txtEditionCode.DataResult = null;
            this.txtEditionCode.DataTableResult = null;
            this.txtEditionCode.EditingControlDataGridView = null;
            this.txtEditionCode.EditingControlFormattedValue = "";
            this.txtEditionCode.EditingControlRowIndex = 0;
            this.txtEditionCode.EditingControlValueChanged = false;
            this.txtEditionCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtEditionCode.IsMultiSelect = false;
            this.txtEditionCode.Location = new System.Drawing.Point(351, 65);
            this.txtEditionCode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtEditionCode.Name = "txtEditionCode";
            this.txtEditionCode.ShowResultForm = true;
            this.txtEditionCode.Size = new System.Drawing.Size(181, 23);
            this.txtEditionCode.StrEndSql = null;
            this.txtEditionCode.TabIndex = 301;
            this.txtEditionCode.TabStop = false;
            this.txtEditionCode.Tag = "";
            this.txtEditionCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtEditionCode_OnCompleteSearch);
            this.txtEditionCode.Enter += new System.EventHandler(this.txtEditionCode_Enter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(279, 69);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 302;
            this.label2.Text = "总成图号";
            // 
            // numRedices
            // 
            this.numRedices.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numRedices.Location = new System.Drawing.Point(84, 150);
            this.numRedices.Maximum = new decimal(new int[] {
            10000000,
            0,
            0,
            0});
            this.numRedices.Name = "numRedices";
            this.numRedices.Size = new System.Drawing.Size(181, 23);
            this.numRedices.TabIndex = 300;
            this.numRedices.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 154);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 299;
            this.label3.Text = "基    数";
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSpec.ForeColor = System.Drawing.Color.Black;
            this.txtSpec.Location = new System.Drawing.Point(624, 108);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(181, 23);
            this.txtSpec.TabIndex = 298;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(555, 112);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 297;
            this.label7.Text = "规    格";
            // 
            // txtCode
            // 
            this.txtCode.DataResult = null;
            this.txtCode.DataTableResult = null;
            this.txtCode.EditingControlDataGridView = null;
            this.txtCode.EditingControlFormattedValue = "";
            this.txtCode.EditingControlRowIndex = 0;
            this.txtCode.EditingControlValueChanged = false;
            this.txtCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCode.IsMultiSelect = false;
            this.txtCode.Location = new System.Drawing.Point(84, 108);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtCode.Name = "txtCode";
            this.txtCode.ShowResultForm = true;
            this.txtCode.Size = new System.Drawing.Size(181, 23);
            this.txtCode.StrEndSql = null;
            this.txtCode.TabIndex = 296;
            this.txtCode.TabStop = false;
            this.txtCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtCode_OnCompleteSearch);
            this.txtCode.Enter += new System.EventHandler(this.txtCode_Enter);
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(351, 108);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(181, 23);
            this.txtName.TabIndex = 295;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(279, 112);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 294;
            this.label9.Text = "物品名称";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 112);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 293;
            this.label1.Text = "图号型号";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // 发料清单编制明细界面
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(816, 552);
            this.Controls.Add(this.customPanel1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "发料清单编制明细界面";
            this.Text = "发料清单编制明细界面";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.customPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRedices)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private UniversalControlLibrary.TextBoxShow txtCode;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numRedices;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtEditionCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label lbSDBStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtSDBNo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.Button btn_Import;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox cmbApplicable;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chbOnceTheWholeIssue;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全消ToolStripMenuItem;
        private System.Windows.Forms.CheckBox chbDelete;
    }
}