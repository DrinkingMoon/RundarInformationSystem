using UniversalControlLibrary;
namespace Form_Quality_File
{
    partial class 文件发放回收登记表
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSign = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.btnRecover = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnExport = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbGrantTime = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lbGrantPersonnel = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtGrantDepartment = new TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.lbRecoverTime = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbRecoverPersonnel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lbSignTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbSignPersonnel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtRecoverDepartment = new TextBoxShow();
            this.label6 = new System.Windows.Forms.Label();
            this.txtFileNo = new TextBoxShow();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.toolStripSeparator2,
            this.btnUpdate,
            this.toolStripSeparatorAdd,
            this.btnDelete,
            this.toolStripSeparator3,
            this.btnSign,
            this.toolStripSeparatorDelete,
            this.btnRecover,
            this.toolStripSeparator5,
            this.btnRefresh,
            this.toolStripSeparator1,
            this.btnExport});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1016, 25);
            this.toolStrip1.TabIndex = 44;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(67, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添加(&A)";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnUpdate
            // 
            this.btnUpdate.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.btnUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(67, 22);
            this.btnUpdate.Tag = "Delete";
            this.btnUpdate.Text = "修改(&U)";
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            // 
            // btnDelete
            // 
            this.btnDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(67, 22);
            this.btnDelete.Tag = "Delete";
            this.btnDelete.Text = "删除(&D)";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnSign
            // 
            this.btnSign.Image = global::UniversalControlLibrary.Properties.Resources.match;
            this.btnSign.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(67, 22);
            this.btnSign.Tag = "view";
            this.btnSign.Text = "签收(&S)";
            this.btnSign.Click += new System.EventHandler(this.btnSign_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRecover
            // 
            this.btnRecover.Image = global::UniversalControlLibrary.Properties.Resources.ReadMe;
            this.btnRecover.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRecover.Name = "btnRecover";
            this.btnRecover.Size = new System.Drawing.Size(91, 22);
            this.btnRecover.Tag = "view";
            this.btnRecover.Text = "确认回收(&R)";
            this.btnRecover.Click += new System.EventHandler(this.btnRecover_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // btnRefresh
            // 
            this.btnRefresh.Image = global::UniversalControlLibrary.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(67, 22);
            this.btnRefresh.Tag = "view";
            this.btnRefresh.Text = "刷新(&R)";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnExport
            // 
            this.btnExport.Image = global::UniversalControlLibrary.Properties.Resources.Excel;
            this.btnExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(97, 22);
            this.btnExport.Tag = "view";
            this.btnExport.Text = "导出EXCEL(&E)";
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1016, 61);
            this.panel1.TabIndex = 60;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(374, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(269, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "文件发放/回收登记表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbGrantTime);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.lbGrantPersonnel);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtGrantDepartment);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.lbRecoverTime);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.lbRecoverPersonnel);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.lbSignTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.lbSignPersonnel);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtRecoverDepartment);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtFileNo);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 86);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1016, 131);
            this.groupBox1.TabIndex = 61;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息显示";
            // 
            // lbGrantTime
            // 
            this.lbGrantTime.AutoSize = true;
            this.lbGrantTime.Location = new System.Drawing.Point(900, 60);
            this.lbGrantTime.Name = "lbGrantTime";
            this.lbGrantTime.Size = new System.Drawing.Size(59, 12);
            this.lbGrantTime.TabIndex = 201;
            this.lbGrantTime.Text = "GrantTime";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(831, 60);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 200;
            this.label9.Text = "发放日期";
            // 
            // lbGrantPersonnel
            // 
            this.lbGrantPersonnel.AutoSize = true;
            this.lbGrantPersonnel.Location = new System.Drawing.Point(720, 60);
            this.lbGrantPersonnel.Name = "lbGrantPersonnel";
            this.lbGrantPersonnel.Size = new System.Drawing.Size(89, 12);
            this.lbGrantPersonnel.TabIndex = 199;
            this.lbGrantPersonnel.Text = "GrantPersonnel";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(644, 60);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 12);
            this.label12.TabIndex = 198;
            this.label12.Text = "发 放 人";
            // 
            // txtGrantDepartment
            // 
            this.txtGrantDepartment.FindItem = TextBoxShow.FindType.部门;
            this.txtGrantDepartment.Location = new System.Drawing.Point(84, 56);
            this.txtGrantDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGrantDepartment.Name = "txtGrantDepartment";
            this.txtGrantDepartment.Size = new System.Drawing.Size(108, 21);
            this.txtGrantDepartment.TabIndex = 197;
            this.txtGrantDepartment.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGrantDepartment_OnCompleteSearch);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 196;
            this.label2.Text = "发放单位";
            // 
            // lbRecoverTime
            // 
            this.lbRecoverTime.AutoSize = true;
            this.lbRecoverTime.Location = new System.Drawing.Point(900, 110);
            this.lbRecoverTime.Name = "lbRecoverTime";
            this.lbRecoverTime.Size = new System.Drawing.Size(71, 12);
            this.lbRecoverTime.TabIndex = 195;
            this.lbRecoverTime.Text = "RecoverTime";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(831, 110);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 194;
            this.label3.Text = "回收日期";
            // 
            // lbRecoverPersonnel
            // 
            this.lbRecoverPersonnel.AutoSize = true;
            this.lbRecoverPersonnel.Location = new System.Drawing.Point(720, 110);
            this.lbRecoverPersonnel.Name = "lbRecoverPersonnel";
            this.lbRecoverPersonnel.Size = new System.Drawing.Size(101, 12);
            this.lbRecoverPersonnel.TabIndex = 193;
            this.lbRecoverPersonnel.Text = "RecoverPersonnel";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(644, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 192;
            this.label7.Text = "回收确认人";
            // 
            // lbSignTime
            // 
            this.lbSignTime.AutoSize = true;
            this.lbSignTime.Location = new System.Drawing.Point(900, 85);
            this.lbSignTime.Name = "lbSignTime";
            this.lbSignTime.Size = new System.Drawing.Size(53, 12);
            this.lbSignTime.TabIndex = 191;
            this.lbSignTime.Text = "SignTime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(831, 85);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 190;
            this.label10.Text = "签收日期";
            // 
            // lbSignPersonnel
            // 
            this.lbSignPersonnel.AutoSize = true;
            this.lbSignPersonnel.Location = new System.Drawing.Point(720, 85);
            this.lbSignPersonnel.Name = "lbSignPersonnel";
            this.lbSignPersonnel.Size = new System.Drawing.Size(83, 12);
            this.lbSignPersonnel.TabIndex = 189;
            this.lbSignPersonnel.Text = "SignPersonnel";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(644, 85);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 188;
            this.label8.Text = "签 收 人";
            // 
            // txtRecoverDepartment
            // 
            this.txtRecoverDepartment.FindItem = TextBoxShow.FindType.部门;
            this.txtRecoverDepartment.Location = new System.Drawing.Point(371, 56);
            this.txtRecoverDepartment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtRecoverDepartment.Name = "txtRecoverDepartment";
            this.txtRecoverDepartment.Size = new System.Drawing.Size(108, 21);
            this.txtRecoverDepartment.TabIndex = 187;
            this.txtRecoverDepartment.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtRecoverDepartment_OnCompleteSearch);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(309, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 186;
            this.label6.Text = "收文单位";
            // 
            // txtFileNo
            // 
            this.txtFileNo.FindItem = TextBoxShow.FindType.体系文件;
            this.txtFileNo.Location = new System.Drawing.Point(84, 25);
            this.txtFileNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileNo.Name = "txtFileNo";
            this.txtFileNo.Size = new System.Drawing.Size(204, 21);
            this.txtFileNo.TabIndex = 185;
            this.txtFileNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtFileNo_OnCompleteSearch);
            this.txtFileNo.Enter += new System.EventHandler(this.txtFileNo_Enter);
            // 
            // txtVersion
            // 
            this.txtVersion.Enabled = false;
            this.txtVersion.Location = new System.Drawing.Point(708, 25);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(95, 21);
            this.txtVersion.TabIndex = 184;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(644, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 183;
            this.label4.Text = "版 本 号";
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(371, 25);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(256, 21);
            this.txtFileName.TabIndex = 182;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(309, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 181;
            this.label1.Text = "文件名称";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(20, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 180;
            this.label13.Text = "文件编号";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 249);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1016, 487);
            this.dataGridView1.TabIndex = 66;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 217);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1016, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 64;
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
            // 文件发放回收登记表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 736);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Name = "文件发放回收登记表";
            this.Text = "文件发放回收登记表";
            this.Load += new System.EventHandler(this.文件发放回收登记表_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton btnRefresh;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnExport;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private TextBoxShow txtFileNo;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label13;
        private TextBoxShow txtRecoverDepartment;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnSign;
        private System.Windows.Forms.ToolStripButton btnRecover;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private TextBoxShow txtGrantDepartment;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbGrantTime;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lbGrantPersonnel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label lbRecoverTime;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbRecoverPersonnel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lbSignTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbSignPersonnel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
    }
}