namespace Form_Peripheral_CompanyQuality
{
    partial class 设置关键节点
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.F_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.txtDutyUser = new UniversalControlLibrary.TextBoxShow();
            this.txtKeyPointName = new UniversalControlLibrary.TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.customGroupBox2.SuspendLayout();
            this.SuspendLayout();
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
            this.F_Id});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 89);
            this.customDataGridView1.MultiSelect = false;
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(662, 522);
            this.customDataGridView1.TabIndex = 99;
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
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.txtDutyUser);
            this.customGroupBox2.Controls.Add(this.txtKeyPointName);
            this.customGroupBox2.Controls.Add(this.label2);
            this.customGroupBox2.Controls.Add(this.btnDelete);
            this.customGroupBox2.Controls.Add(this.btnModify);
            this.customGroupBox2.Controls.Add(this.btnAdd);
            this.customGroupBox2.Controls.Add(this.label8);
            this.customGroupBox2.Controls.Add(this.label7);
            this.customGroupBox2.Controls.Add(this.dtpEndDate);
            this.customGroupBox2.Controls.Add(this.label6);
            this.customGroupBox2.Controls.Add(this.dtpStartDate);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(662, 89);
            this.customGroupBox2.TabIndex = 97;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "关键节点";
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
            this.txtDutyUser.Location = new System.Drawing.Point(518, 54);
            this.txtDutyUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDutyUser.Name = "txtDutyUser";
            this.txtDutyUser.ShowResultForm = true;
            this.txtDutyUser.Size = new System.Drawing.Size(131, 21);
            this.txtDutyUser.StrEndSql = null;
            this.txtDutyUser.TabIndex = 4;
            this.txtDutyUser.TabStop = false;
            this.txtDutyUser.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtDutyUser_OnCompleteSearch);
            // 
            // txtKeyPointName
            // 
            this.txtKeyPointName.DataResult = null;
            this.txtKeyPointName.DataTableResult = null;
            this.txtKeyPointName.EditingControlDataGridView = null;
            this.txtKeyPointName.EditingControlFormattedValue = "";
            this.txtKeyPointName.EditingControlRowIndex = 0;
            this.txtKeyPointName.EditingControlValueChanged = true;
            this.txtKeyPointName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtKeyPointName.IsMultiSelect = false;
            this.txtKeyPointName.Location = new System.Drawing.Point(84, 21);
            this.txtKeyPointName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtKeyPointName.Name = "txtKeyPointName";
            this.txtKeyPointName.ShowResultForm = false;
            this.txtKeyPointName.Size = new System.Drawing.Size(338, 21);
            this.txtKeyPointName.StrEndSql = null;
            this.txtKeyPointName.TabIndex = 1;
            this.txtKeyPointName.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(13, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 374;
            this.label2.Text = "关键节点：";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(588, 20);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 23);
            this.btnDelete.TabIndex = 371;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(513, 20);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(61, 23);
            this.btnModify.TabIndex = 370;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(438, 20);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(61, 23);
            this.btnAdd.TabIndex = 369;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(459, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 367;
            this.label8.Text = "责任人：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(235, 58);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 366;
            this.label7.Text = "完成时间：";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(306, 54);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(139, 21);
            this.dtpEndDate.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(13, 58);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 364;
            this.label6.Text = "启动时间：";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(84, 54);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(139, 21);
            this.dtpStartDate.TabIndex = 2;
            // 
            // 设置关键节点
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(662, 611);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox2);
            this.Name = "设置关键节点";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置关键节点";
            this.Load += new System.EventHandler(this.设置关键节点_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customGroupBox2.ResumeLayout(false);
            this.customGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private UniversalControlLibrary.TextBoxShow txtKeyPointName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private UniversalControlLibrary.TextBoxShow txtDutyUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;
    }
}