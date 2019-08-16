namespace Form_Peripheral_CompanyQuality
{
    partial class 发布重点工作
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
            this.txtTaskName = new UniversalControlLibrary.TextBoxShow();
            this.label2 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.dtpEndDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpStartDate = new System.Windows.Forms.DateTimePicker();
            this.txtExpectedGoal = new UniversalControlLibrary.TextBoxShow();
            this.label5 = new System.Windows.Forms.Label();
            this.txtTaskDescription = new UniversalControlLibrary.TextBoxShow();
            this.label3 = new System.Windows.Forms.Label();
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
            this.customDataGridView1.Location = new System.Drawing.Point(0, 238);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(902, 423);
            this.customDataGridView1.TabIndex = 97;
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
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.txtDutyUser);
            this.customGroupBox2.Controls.Add(this.txtTaskName);
            this.customGroupBox2.Controls.Add(this.label2);
            this.customGroupBox2.Controls.Add(this.btnDelete);
            this.customGroupBox2.Controls.Add(this.btnModify);
            this.customGroupBox2.Controls.Add(this.btnAdd);
            this.customGroupBox2.Controls.Add(this.label8);
            this.customGroupBox2.Controls.Add(this.label7);
            this.customGroupBox2.Controls.Add(this.dtpEndDate);
            this.customGroupBox2.Controls.Add(this.label6);
            this.customGroupBox2.Controls.Add(this.dtpStartDate);
            this.customGroupBox2.Controls.Add(this.txtExpectedGoal);
            this.customGroupBox2.Controls.Add(this.label5);
            this.customGroupBox2.Controls.Add(this.txtTaskDescription);
            this.customGroupBox2.Controls.Add(this.label3);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(902, 238);
            this.customGroupBox2.TabIndex = 96;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "重点工作";
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
            this.txtDutyUser.Location = new System.Drawing.Point(521, 197);
            this.txtDutyUser.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtDutyUser.Name = "txtDutyUser";
            this.txtDutyUser.ShowResultForm = true;
            this.txtDutyUser.Size = new System.Drawing.Size(101, 21);
            this.txtDutyUser.StrEndSql = null;
            this.txtDutyUser.TabIndex = 6;
            this.txtDutyUser.TabStop = false;
            this.txtDutyUser.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtDutyUser_OnCompleteSearch);
            // 
            // txtTaskName
            // 
            this.txtTaskName.DataResult = null;
            this.txtTaskName.DataTableResult = null;
            this.txtTaskName.EditingControlDataGridView = null;
            this.txtTaskName.EditingControlFormattedValue = "";
            this.txtTaskName.EditingControlRowIndex = 0;
            this.txtTaskName.EditingControlValueChanged = true;
            this.txtTaskName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtTaskName.IsMultiSelect = false;
            this.txtTaskName.Location = new System.Drawing.Point(108, 20);
            this.txtTaskName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTaskName.Name = "txtTaskName";
            this.txtTaskName.ShowResultForm = false;
            this.txtTaskName.Size = new System.Drawing.Size(761, 21);
            this.txtTaskName.StrEndSql = null;
            this.txtTaskName.TabIndex = 1;
            this.txtTaskName.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(33, 24);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 374;
            this.label2.Text = "重点工作：";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(808, 196);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(61, 23);
            this.btnDelete.TabIndex = 371;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(723, 196);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(61, 23);
            this.btnModify.TabIndex = 370;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(638, 196);
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
            this.label8.Location = new System.Drawing.Point(465, 201);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 367;
            this.label8.Text = "责任人：";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(253, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 366;
            this.label7.Text = "完成时间：";
            // 
            // dtpEndDate
            // 
            this.dtpEndDate.Location = new System.Drawing.Point(321, 197);
            this.dtpEndDate.Name = "dtpEndDate";
            this.dtpEndDate.Size = new System.Drawing.Size(139, 21);
            this.dtpEndDate.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(33, 201);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 364;
            this.label6.Text = "启动时间：";
            // 
            // dtpStartDate
            // 
            this.dtpStartDate.Location = new System.Drawing.Point(108, 197);
            this.dtpStartDate.Name = "dtpStartDate";
            this.dtpStartDate.Size = new System.Drawing.Size(139, 21);
            this.dtpStartDate.TabIndex = 4;
            // 
            // txtExpectedGoal
            // 
            this.txtExpectedGoal.DataResult = null;
            this.txtExpectedGoal.DataTableResult = null;
            this.txtExpectedGoal.EditingControlDataGridView = null;
            this.txtExpectedGoal.EditingControlFormattedValue = "";
            this.txtExpectedGoal.EditingControlRowIndex = 0;
            this.txtExpectedGoal.EditingControlValueChanged = true;
            this.txtExpectedGoal.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtExpectedGoal.IsMultiSelect = false;
            this.txtExpectedGoal.Location = new System.Drawing.Point(521, 56);
            this.txtExpectedGoal.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtExpectedGoal.Multiline = true;
            this.txtExpectedGoal.Name = "txtExpectedGoal";
            this.txtExpectedGoal.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtExpectedGoal.ShowResultForm = false;
            this.txtExpectedGoal.Size = new System.Drawing.Size(348, 123);
            this.txtExpectedGoal.StrEndSql = null;
            this.txtExpectedGoal.TabIndex = 3;
            this.txtExpectedGoal.TabStop = false;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(450, 111);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 12);
            this.label5.TabIndex = 361;
            this.label5.Text = "预期目标：";
            // 
            // txtTaskDescription
            // 
            this.txtTaskDescription.DataResult = null;
            this.txtTaskDescription.DataTableResult = null;
            this.txtTaskDescription.EditingControlDataGridView = null;
            this.txtTaskDescription.EditingControlFormattedValue = "";
            this.txtTaskDescription.EditingControlRowIndex = 0;
            this.txtTaskDescription.EditingControlValueChanged = true;
            this.txtTaskDescription.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtTaskDescription.IsMultiSelect = false;
            this.txtTaskDescription.Location = new System.Drawing.Point(108, 56);
            this.txtTaskDescription.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtTaskDescription.Multiline = true;
            this.txtTaskDescription.Name = "txtTaskDescription";
            this.txtTaskDescription.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtTaskDescription.ShowResultForm = false;
            this.txtTaskDescription.Size = new System.Drawing.Size(324, 123);
            this.txtTaskDescription.StrEndSql = null;
            this.txtTaskDescription.TabIndex = 2;
            this.txtTaskDescription.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(33, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 359;
            this.label3.Text = "工作描述：";
            // 
            // 发布重点工作
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(902, 661);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.customGroupBox2);
            this.Name = "发布重点工作";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发布重点工作";
            this.Load += new System.EventHandler(this.发布重点工作_Load);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.customGroupBox2.ResumeLayout(false);
            this.customGroupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DateTimePicker dtpEndDate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpStartDate;
        private UniversalControlLibrary.TextBoxShow txtExpectedGoal;
        private System.Windows.Forms.Label label5;
        private UniversalControlLibrary.TextBoxShow txtTaskDescription;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.Button btnAdd;
        private UniversalControlLibrary.TextBoxShow txtTaskName;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtDutyUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;

    }
}