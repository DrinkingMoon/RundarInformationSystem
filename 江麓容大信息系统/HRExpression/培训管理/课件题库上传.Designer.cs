namespace Form_Peripheral_HR
{
    partial class 课件题库上传
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.customGroupBox3 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.dgv_QuestionBank = new UniversalControlLibrary.CustomDataGridView();
            this.课程ID1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.考题ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.btnDeleteQuestion = new System.Windows.Forms.Button();
            this.btn_QuestionBank_Input = new System.Windows.Forms.Button();
            this.btn_Delete = new System.Windows.Forms.Button();
            this.btn_Submit = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCoursewareName = new UniversalControlLibrary.TextBoxShow();
            this.dgv_Courseware = new UniversalControlLibrary.CustomDataGridView();
            this.文件名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.文件唯一编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.文件ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.课程ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.numPass = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.numExtraction = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPost = new UniversalControlLibrary.TextBoxShow();
            this.label3 = new System.Windows.Forms.Label();
            this.txtCourseName = new UniversalControlLibrary.TextBoxShow();
            this.btnChange = new System.Windows.Forms.Button();
            this.customGroupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_QuestionBank)).BeginInit();
            this.customGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Courseware)).BeginInit();
            this.customGroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPass)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExtraction)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // customGroupBox3
            // 
            this.customGroupBox3.Controls.Add(this.dgv_QuestionBank);
            this.customGroupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox3.Location = new System.Drawing.Point(0, 272);
            this.customGroupBox3.Name = "customGroupBox3";
            this.customGroupBox3.Size = new System.Drawing.Size(856, 259);
            this.customGroupBox3.TabIndex = 2;
            this.customGroupBox3.TabStop = false;
            this.customGroupBox3.Text = "题库";
            // 
            // dgv_QuestionBank
            // 
            this.dgv_QuestionBank.AllowUserToAddRows = false;
            this.dgv_QuestionBank.AllowUserToDeleteRows = false;
            this.dgv_QuestionBank.AllowUserToResizeRows = false;
            this.dgv_QuestionBank.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_QuestionBank.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.课程ID1,
            this.考题ID});
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_QuestionBank.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgv_QuestionBank.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv_QuestionBank.Location = new System.Drawing.Point(3, 17);
            this.dgv_QuestionBank.Name = "dgv_QuestionBank";
            this.dgv_QuestionBank.RowTemplate.Height = 23;
            this.dgv_QuestionBank.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_QuestionBank.Size = new System.Drawing.Size(850, 239);
            this.dgv_QuestionBank.TabIndex = 0;
            // 
            // 课程ID1
            // 
            this.课程ID1.DataPropertyName = "课程ID";
            this.课程ID1.HeaderText = "课程ID";
            this.课程ID1.Name = "课程ID1";
            this.课程ID1.Visible = false;
            // 
            // 考题ID
            // 
            this.考题ID.DataPropertyName = "考题ID";
            this.考题ID.HeaderText = "考题ID";
            this.考题ID.Name = "考题ID";
            this.考题ID.Visible = false;
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.btnDeleteQuestion);
            this.customGroupBox2.Controls.Add(this.btn_QuestionBank_Input);
            this.customGroupBox2.Controls.Add(this.btn_Delete);
            this.customGroupBox2.Controls.Add(this.btn_Submit);
            this.customGroupBox2.Controls.Add(this.label2);
            this.customGroupBox2.Controls.Add(this.txtCoursewareName);
            this.customGroupBox2.Controls.Add(this.dgv_Courseware);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 94);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(856, 178);
            this.customGroupBox2.TabIndex = 1;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "课件";
            // 
            // btnDeleteQuestion
            // 
            this.btnDeleteQuestion.Location = new System.Drawing.Point(238, 136);
            this.btnDeleteQuestion.Name = "btnDeleteQuestion";
            this.btnDeleteQuestion.Size = new System.Drawing.Size(76, 23);
            this.btnDeleteQuestion.TabIndex = 29;
            this.btnDeleteQuestion.Text = "删除考题";
            this.btnDeleteQuestion.UseVisualStyleBackColor = true;
            this.btnDeleteQuestion.Click += new System.EventHandler(this.btnDeleteQuestion_Click);
            // 
            // btn_QuestionBank_Input
            // 
            this.btn_QuestionBank_Input.Location = new System.Drawing.Point(68, 136);
            this.btn_QuestionBank_Input.Name = "btn_QuestionBank_Input";
            this.btn_QuestionBank_Input.Size = new System.Drawing.Size(140, 23);
            this.btn_QuestionBank_Input.TabIndex = 28;
            this.btn_QuestionBank_Input.Text = "将考题导入到题库中";
            this.btn_QuestionBank_Input.UseVisualStyleBackColor = true;
            this.btn_QuestionBank_Input.Click += new System.EventHandler(this.btn_QuestionBank_Input_Click);
            // 
            // btn_Delete
            // 
            this.btn_Delete.Location = new System.Drawing.Point(214, 88);
            this.btn_Delete.Name = "btn_Delete";
            this.btn_Delete.Size = new System.Drawing.Size(100, 23);
            this.btn_Delete.TabIndex = 27;
            this.btn_Delete.Text = "删除课件";
            this.btn_Delete.UseVisualStyleBackColor = true;
            this.btn_Delete.Click += new System.EventHandler(this.btn_Delete_Click);
            // 
            // btn_Submit
            // 
            this.btn_Submit.Location = new System.Drawing.Point(70, 88);
            this.btn_Submit.Name = "btn_Submit";
            this.btn_Submit.Size = new System.Drawing.Size(100, 23);
            this.btn_Submit.TabIndex = 26;
            this.btn_Submit.Text = "上传并提交课件";
            this.btn_Submit.UseVisualStyleBackColor = true;
            this.btn_Submit.Click += new System.EventHandler(this.btn_Submit_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(68, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "课 件 名：";
            // 
            // txtCoursewareName
            // 
            this.txtCoursewareName.DataResult = null;
            this.txtCoursewareName.DataTableResult = null;
            this.txtCoursewareName.EditingControlDataGridView = null;
            this.txtCoursewareName.EditingControlFormattedValue = "";
            this.txtCoursewareName.EditingControlRowIndex = 0;
            this.txtCoursewareName.EditingControlValueChanged = true;
            this.txtCoursewareName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtCoursewareName.IsMultiSelect = false;
            this.txtCoursewareName.Location = new System.Drawing.Point(139, 48);
            this.txtCoursewareName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCoursewareName.Name = "txtCoursewareName";
            this.txtCoursewareName.ShowResultForm = false;
            this.txtCoursewareName.Size = new System.Drawing.Size(175, 21);
            this.txtCoursewareName.StrEndSql = null;
            this.txtCoursewareName.TabIndex = 13;
            this.txtCoursewareName.TabStop = false;
            // 
            // dgv_Courseware
            // 
            this.dgv_Courseware.AllowUserToAddRows = false;
            this.dgv_Courseware.AllowUserToDeleteRows = false;
            this.dgv_Courseware.AllowUserToResizeRows = false;
            this.dgv_Courseware.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgv_Courseware.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.文件名,
            this.文件唯一编码,
            this.文件ID,
            this.课程ID});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv_Courseware.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgv_Courseware.Dock = System.Windows.Forms.DockStyle.Right;
            this.dgv_Courseware.Location = new System.Drawing.Point(377, 17);
            this.dgv_Courseware.Name = "dgv_Courseware";
            this.dgv_Courseware.ReadOnly = true;
            this.dgv_Courseware.RowTemplate.Height = 23;
            this.dgv_Courseware.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgv_Courseware.Size = new System.Drawing.Size(476, 158);
            this.dgv_Courseware.TabIndex = 0;
            this.dgv_Courseware.DoubleClick += new System.EventHandler(this.dgv_Courseware_DoubleClick);
            this.dgv_Courseware.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgv_Courseware_CellEnter);
            // 
            // 文件名
            // 
            this.文件名.DataPropertyName = "文件名";
            this.文件名.HeaderText = "文件名";
            this.文件名.Name = "文件名";
            this.文件名.ReadOnly = true;
            this.文件名.Width = 200;
            // 
            // 文件唯一编码
            // 
            this.文件唯一编码.DataPropertyName = "文件唯一编码";
            this.文件唯一编码.HeaderText = "文件唯一编码";
            this.文件唯一编码.Name = "文件唯一编码";
            this.文件唯一编码.ReadOnly = true;
            this.文件唯一编码.Visible = false;
            // 
            // 文件ID
            // 
            this.文件ID.DataPropertyName = "文件ID";
            this.文件ID.HeaderText = "文件ID";
            this.文件ID.Name = "文件ID";
            this.文件ID.ReadOnly = true;
            this.文件ID.Visible = false;
            // 
            // 课程ID
            // 
            this.课程ID.DataPropertyName = "课程ID";
            this.课程ID.HeaderText = "课程ID";
            this.课程ID.Name = "课程ID";
            this.课程ID.ReadOnly = true;
            this.课程ID.Visible = false;
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.btnChange);
            this.customGroupBox1.Controls.Add(this.label6);
            this.customGroupBox1.Controls.Add(this.label7);
            this.customGroupBox1.Controls.Add(this.numPass);
            this.customGroupBox1.Controls.Add(this.label5);
            this.customGroupBox1.Controls.Add(this.label4);
            this.customGroupBox1.Controls.Add(this.numExtraction);
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.txtPost);
            this.customGroupBox1.Controls.Add(this.label3);
            this.customGroupBox1.Controls.Add(this.txtCourseName);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(856, 94);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(344, 63);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 12);
            this.label6.TabIndex = 41;
            this.label6.Text = "%";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(223, 63);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 40;
            this.label7.Text = "考试通过率";
            // 
            // numPass
            // 
            this.numPass.Location = new System.Drawing.Point(291, 59);
            this.numPass.Name = "numPass";
            this.numPass.Size = new System.Drawing.Size(50, 21);
            this.numPass.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(196, 63);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(11, 12);
            this.label5.TabIndex = 38;
            this.label5.Text = "%";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 12);
            this.label4.TabIndex = 37;
            this.label4.Text = "考题提取率";
            // 
            // numExtraction
            // 
            this.numExtraction.Location = new System.Drawing.Point(143, 59);
            this.numExtraction.Name = "numExtraction";
            this.numExtraction.Size = new System.Drawing.Size(50, 21);
            this.numExtraction.TabIndex = 36;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(360, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 14;
            this.label1.Text = "所属岗位：";
            // 
            // txtPost
            // 
            this.txtPost.DataResult = null;
            this.txtPost.DataTableResult = null;
            this.txtPost.EditingControlDataGridView = null;
            this.txtPost.EditingControlFormattedValue = "";
            this.txtPost.EditingControlRowIndex = 0;
            this.txtPost.EditingControlValueChanged = true;
            this.txtPost.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtPost.IsMultiSelect = false;
            this.txtPost.Location = new System.Drawing.Point(431, 21);
            this.txtPost.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtPost.Name = "txtPost";
            this.txtPost.ShowResultForm = false;
            this.txtPost.Size = new System.Drawing.Size(400, 21);
            this.txtPost.StrEndSql = null;
            this.txtPost.TabIndex = 13;
            this.txtPost.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(66, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 12;
            this.label3.Text = "课 程 名：";
            // 
            // txtCourseName
            // 
            this.txtCourseName.DataResult = null;
            this.txtCourseName.DataTableResult = null;
            this.txtCourseName.EditingControlDataGridView = null;
            this.txtCourseName.EditingControlFormattedValue = "";
            this.txtCourseName.EditingControlRowIndex = 0;
            this.txtCourseName.EditingControlValueChanged = true;
            this.txtCourseName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtCourseName.IsMultiSelect = false;
            this.txtCourseName.Location = new System.Drawing.Point(139, 21);
            this.txtCourseName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCourseName.Name = "txtCourseName";
            this.txtCourseName.ShowResultForm = false;
            this.txtCourseName.Size = new System.Drawing.Size(175, 21);
            this.txtCourseName.StrEndSql = null;
            this.txtCourseName.TabIndex = 11;
            this.txtCourseName.TabStop = false;
            // 
            // btnChange
            // 
            this.btnChange.Location = new System.Drawing.Point(377, 58);
            this.btnChange.Name = "btnChange";
            this.btnChange.Size = new System.Drawing.Size(75, 23);
            this.btnChange.TabIndex = 42;
            this.btnChange.Text = "变更";
            this.btnChange.UseVisualStyleBackColor = true;
            this.btnChange.Click += new System.EventHandler(this.btnChange_Click);
            // 
            // 课件题库上传
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(856, 531);
            this.Controls.Add(this.customGroupBox3);
            this.Controls.Add(this.customGroupBox2);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "课件题库上传";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "课件题库上传";
            this.Load += new System.EventHandler(this.课件题库上传_Load);
            this.customGroupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv_QuestionBank)).EndInit();
            this.customGroupBox2.ResumeLayout(false);
            this.customGroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv_Courseware)).EndInit();
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPass)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numExtraction)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.TextBoxShow txtPost;
        private System.Windows.Forms.Label label3;
        private UniversalControlLibrary.TextBoxShow txtCourseName;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private System.Windows.Forms.Label label2;
        private UniversalControlLibrary.TextBoxShow txtCoursewareName;
        private UniversalControlLibrary.CustomDataGridView dgv_Courseware;
        private System.Windows.Forms.Button btn_Delete;
        private System.Windows.Forms.Button btn_Submit;
        private UniversalControlLibrary.CustomGroupBox customGroupBox3;
        private UniversalControlLibrary.CustomDataGridView dgv_QuestionBank;
        private System.Windows.Forms.Button btn_QuestionBank_Input;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件唯一编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程ID;
        private System.Windows.Forms.Button btnDeleteQuestion;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程ID1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 考题ID;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numPass;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numExtraction;
        private System.Windows.Forms.Button btnChange;

    }
}