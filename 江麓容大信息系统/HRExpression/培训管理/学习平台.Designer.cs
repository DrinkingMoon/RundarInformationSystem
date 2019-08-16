namespace Form_Peripheral_HR
{
    partial class 学习平台
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
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.treeView1 = new System.Windows.Forms.TreeView();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.文件名 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.文件唯一编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.文件ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.课程ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.plExam = new System.Windows.Forms.Panel();
            this.btnBeginExam = new System.Windows.Forms.Button();
            this.customGroupBox1.SuspendLayout();
            this.customGroupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.plExam.SuspendLayout();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.treeView1);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(310, 729);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "课程";
            // 
            // treeView1
            // 
            this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeView1.Location = new System.Drawing.Point(3, 17);
            this.treeView1.Name = "treeView1";
            this.treeView1.Size = new System.Drawing.Size(304, 709);
            this.treeView1.TabIndex = 1;
            this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.tabControl1);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox2.Location = new System.Drawing.Point(310, 0);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(651, 729);
            this.customGroupBox2.TabIndex = 2;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "学习平台";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 17);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(645, 709);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.customDataGridView1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(637, 683);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "课件";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.文件名,
            this.文件唯一编码,
            this.文件ID,
            this.课程ID});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 3);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.ReadOnly = true;
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(631, 677);
            this.customDataGridView1.TabIndex = 0;
            this.customDataGridView1.DoubleClick += new System.EventHandler(this.customDataGridView1_DoubleClick);
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
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.plExam);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(637, 683);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "题库";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // plExam
            // 
            this.plExam.Controls.Add(this.btnBeginExam);
            this.plExam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plExam.Location = new System.Drawing.Point(3, 3);
            this.plExam.Name = "plExam";
            this.plExam.Size = new System.Drawing.Size(631, 677);
            this.plExam.TabIndex = 0;
            // 
            // btnBeginExam
            // 
            this.btnBeginExam.Location = new System.Drawing.Point(193, 101);
            this.btnBeginExam.Name = "btnBeginExam";
            this.btnBeginExam.Size = new System.Drawing.Size(245, 79);
            this.btnBeginExam.TabIndex = 0;
            this.btnBeginExam.Text = "开始考试";
            this.btnBeginExam.UseVisualStyleBackColor = true;
            this.btnBeginExam.Click += new System.EventHandler(this.btnBeginExam_Click);
            // 
            // 学习平台
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(961, 729);
            this.Controls.Add(this.customGroupBox2);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "学习平台";
            this.Text = "学习平台";
            this.Load += new System.EventHandler(this.学习平台_Load);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox2.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.plExam.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private System.Windows.Forms.TreeView treeView1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件名;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件唯一编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 文件ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 课程ID;
        private System.Windows.Forms.Panel plExam;
        private System.Windows.Forms.Button btnBeginExam;

    }
}