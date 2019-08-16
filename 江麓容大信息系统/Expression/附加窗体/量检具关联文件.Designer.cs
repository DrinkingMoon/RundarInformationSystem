namespace Expression
{
    partial class 量检具关联文件
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
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.F_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GaugeCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.FilePath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F_CreateUser = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dtpFileDate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnDownLoad = new System.Windows.Forms.Button();
            this.btnUpLoad = new System.Windows.Forms.Button();
            this.cmbFileType = new UniversalControlLibrary.CustomComboBox(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.txtFileName = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.customGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.F_Id,
            this.GaugeCode,
            this.FilePath,
            this.F_CreateUser});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 154);
            this.customDataGridView1.MultiSelect = false;
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(541, 316);
            this.customDataGridView1.TabIndex = 2;
            this.customDataGridView1.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellDoubleClick);
            this.customDataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellClick);
            // 
            // F_Id
            // 
            this.F_Id.DataPropertyName = "F_Id";
            this.F_Id.HeaderText = "F_Id";
            this.F_Id.Name = "F_Id";
            this.F_Id.ReadOnly = true;
            this.F_Id.Visible = false;
            // 
            // GaugeCode
            // 
            this.GaugeCode.DataPropertyName = "GaugeCode";
            this.GaugeCode.HeaderText = "GaugeCode";
            this.GaugeCode.Name = "GaugeCode";
            this.GaugeCode.ReadOnly = true;
            this.GaugeCode.Visible = false;
            // 
            // FilePath
            // 
            this.FilePath.DataPropertyName = "FilePath";
            this.FilePath.HeaderText = "FilePath";
            this.FilePath.Name = "FilePath";
            this.FilePath.ReadOnly = true;
            this.FilePath.Visible = false;
            // 
            // F_CreateUser
            // 
            this.F_CreateUser.DataPropertyName = "F_CreateUser";
            this.F_CreateUser.HeaderText = "F_CreateUser";
            this.F_CreateUser.Name = "F_CreateUser";
            this.F_CreateUser.ReadOnly = true;
            this.F_CreateUser.Visible = false;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.dtpFileDate);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.btnDelete);
            this.customGroupBox1.Controls.Add(this.btnDownLoad);
            this.customGroupBox1.Controls.Add(this.btnUpLoad);
            this.customGroupBox1.Controls.Add(this.cmbFileType);
            this.customGroupBox1.Controls.Add(this.label2);
            this.customGroupBox1.Controls.Add(this.txtFileName);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(541, 154);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // dtpFileDate
            // 
            this.dtpFileDate.Location = new System.Drawing.Point(338, 71);
            this.dtpFileDate.Name = "dtpFileDate";
            this.dtpFileDate.Size = new System.Drawing.Size(158, 21);
            this.dtpFileDate.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(268, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 7;
            this.label3.Text = "文件日期";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(421, 111);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 23);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnDownLoad
            // 
            this.btnDownLoad.Location = new System.Drawing.Point(327, 110);
            this.btnDownLoad.Name = "btnDownLoad";
            this.btnDownLoad.Size = new System.Drawing.Size(75, 23);
            this.btnDownLoad.TabIndex = 5;
            this.btnDownLoad.Text = "下载";
            this.btnDownLoad.UseVisualStyleBackColor = true;
            this.btnDownLoad.Click += new System.EventHandler(this.btnDownLoad_Click);
            // 
            // btnUpLoad
            // 
            this.btnUpLoad.Location = new System.Drawing.Point(233, 111);
            this.btnUpLoad.Name = "btnUpLoad";
            this.btnUpLoad.Size = new System.Drawing.Size(75, 23);
            this.btnUpLoad.TabIndex = 4;
            this.btnUpLoad.Text = "上传";
            this.btnUpLoad.UseVisualStyleBackColor = true;
            this.btnUpLoad.Click += new System.EventHandler(this.btnUpLoad_Click);
            // 
            // cmbFileType
            // 
            this.cmbFileType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFileType.FormattingEnabled = true;
            this.cmbFileType.Items.AddRange(new object[] {
            "校准证书",
            "图纸",
            "报废记录",
            "维修记录"});
            this.cmbFileType.Location = new System.Drawing.Point(109, 71);
            this.cmbFileType.MaxYear = 0;
            this.cmbFileType.MinYear = 0;
            this.cmbFileType.Name = "cmbFileType";
            this.cmbFileType.Size = new System.Drawing.Size(133, 20);
            this.cmbFileType.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 75);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "文件类型";
            // 
            // txtFileName
            // 
            this.txtFileName.DataResult = null;
            this.txtFileName.DataTableResult = null;
            this.txtFileName.EditingControlDataGridView = null;
            this.txtFileName.EditingControlFormattedValue = "";
            this.txtFileName.EditingControlRowIndex = 0;
            this.txtFileName.EditingControlValueChanged = true;
            this.txtFileName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtFileName.IsMultiSelect = false;
            this.txtFileName.Location = new System.Drawing.Point(109, 30);
            this.txtFileName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.ShowResultForm = false;
            this.txtFileName.Size = new System.Drawing.Size(387, 21);
            this.txtFileName.StrEndSql = null;
            this.txtFileName.TabIndex = 1;
            this.txtFileName.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(31, 34);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "文 件 名";
            // 
            // 量检具关联文件
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(541, 470);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "量检具关联文件";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "量检具关联文件";
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnDownLoad;
        private System.Windows.Forms.Button btnUpLoad;
        private UniversalControlLibrary.CustomComboBox cmbFileType;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtFileName;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DateTimePicker dtpFileDate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn GaugeCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn FilePath;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_CreateUser;
    }
}