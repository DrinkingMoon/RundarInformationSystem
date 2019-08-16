namespace Form_Project_Project
{
    partial class 项目工时表
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSetPersonnel = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dtpItemDate = new System.Windows.Forms.DateTimePicker();
            this.numElapsedTime = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbItemName = new System.Windows.Forms.ComboBox();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.txtExecUser = new UniversalControlLibrary.TextBoxShow();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numElapsedTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(521, 61);
            this.panel1.TabIndex = 96;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(187, 17);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(147, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "项目工时表";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtExecUser);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.btnSetPersonnel);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnSubmit);
            this.groupBox1.Controls.Add(this.txtDescription);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.dtpItemDate);
            this.groupBox1.Controls.Add(this.numElapsedTime);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.cmbItemName);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 61);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(521, 299);
            this.groupBox1.TabIndex = 97;
            this.groupBox1.TabStop = false;
            // 
            // btnSetPersonnel
            // 
            this.btnSetPersonnel.Location = new System.Drawing.Point(119, 261);
            this.btnSetPersonnel.Name = "btnSetPersonnel";
            this.btnSetPersonnel.Size = new System.Drawing.Size(133, 23);
            this.btnSetPersonnel.TabIndex = 12;
            this.btnSetPersonnel.Text = "设置填写人员";
            this.btnSetPersonnel.UseVisualStyleBackColor = true;
            this.btnSetPersonnel.Visible = false;
            this.btnSetPersonnel.Click += new System.EventHandler(this.btnSetPersonnel_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(417, 261);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 11;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(312, 261);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 10;
            this.btnSubmit.Text = "提交";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(119, 139);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescription.Size = new System.Drawing.Size(373, 104);
            this.txtDescription.TabIndex = 9;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(46, 185);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "项目描述：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 63);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 12);
            this.label3.TabIndex = 5;
            this.label3.Text = "项目工作日期：";
            // 
            // dtpItemDate
            // 
            this.dtpItemDate.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpItemDate.Location = new System.Drawing.Point(119, 57);
            this.dtpItemDate.Name = "dtpItemDate";
            this.dtpItemDate.Size = new System.Drawing.Size(133, 21);
            this.dtpItemDate.TabIndex = 4;
            this.dtpItemDate.Value = new System.DateTime(2017, 2, 4, 14, 55, 44, 0);
            // 
            // numElapsedTime
            // 
            this.numElapsedTime.DecimalPlaces = 2;
            this.numElapsedTime.Location = new System.Drawing.Point(362, 57);
            this.numElapsedTime.Maximum = new decimal(new int[] {
            24,
            0,
            0,
            0});
            this.numElapsedTime.Name = "numElapsedTime";
            this.numElapsedTime.Size = new System.Drawing.Size(130, 21);
            this.numElapsedTime.TabIndex = 3;
            this.numElapsedTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(287, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "项目工时：";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(46, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "项目名称：";
            // 
            // cmbItemName
            // 
            this.cmbItemName.FormattingEnabled = true;
            this.cmbItemName.Items.AddRange(new object[] {
            "RDC15FK（东风小康风光370）项目",
            "RDC15FH（华晨鑫源金杯750、770）项目",
            "东风小康513项目（RDC15FK、RDC18FD）",
            "力帆X50改型项目",
            "RDC15软件平台升级",
            "RDC18(北汽银翔）项目",
            "RDC18FC（东风小康516）项目",
            "RDC18FC（东风小康580）项目",
            "市场质量提升项目",
            "15万台新基地建设项目",
            "RDH18混合动力CVT项目",
            "RDC25项目",
            "启停Stop-start项目"});
            this.cmbItemName.Location = new System.Drawing.Point(119, 20);
            this.cmbItemName.Name = "cmbItemName";
            this.cmbItemName.Size = new System.Drawing.Size(373, 20);
            this.cmbItemName.TabIndex = 0;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle4;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 360);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowHeadersWidth = 21;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(521, 218);
            this.customDataGridView1.TabIndex = 98;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(46, 102);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 13;
            this.label4.Text = "执 行 人：";
            // 
            // txtExecUser
            // 
            this.txtExecUser.DataResult = null;
            this.txtExecUser.DataTableResult = null;
            this.txtExecUser.EditingControlDataGridView = null;
            this.txtExecUser.EditingControlFormattedValue = "";
            this.txtExecUser.EditingControlRowIndex = 0;
            this.txtExecUser.EditingControlValueChanged = true;
            this.txtExecUser.FindItem = UniversalControlLibrary.TextBoxShow.FindType.员工;
            this.txtExecUser.IsMultiSelect = false;
            this.txtExecUser.Location = new System.Drawing.Point(119, 99);
            this.txtExecUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExecUser.Name = "txtExecUser";
            this.txtExecUser.ShowResultForm = true;
            this.txtExecUser.Size = new System.Drawing.Size(133, 21);
            this.txtExecUser.StrEndSql = null;
            this.txtExecUser.TabIndex = 14;
            this.txtExecUser.TabStop = false;
            this.txtExecUser.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtExecUser_OnCompleteSearch);
            this.txtExecUser.Enter += new System.EventHandler(this.txtExecUser_Enter);
            // 
            // 项目工时表
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(521, 578);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Name = "项目工时表";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "项目工时表";
            this.Load += new System.EventHandler(this.项目工时表_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numElapsedTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbItemName;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dtpItemDate;
        private System.Windows.Forms.NumericUpDown numElapsedTime;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSetPersonnel;
        private System.Windows.Forms.Label label4;
        private UniversalControlLibrary.TextBoxShow txtExecUser;

    }
}