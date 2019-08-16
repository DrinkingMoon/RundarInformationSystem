using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlBatchException
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlBatchException));
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.新建toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.添加toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.审核toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtDirector = new System.Windows.Forms.TextBox();
            this.txtRecorder = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.dtpSignatureDate = new System.Windows.Forms.DateTimePicker();
            this.dtpRecordTime = new System.Windows.Forms.DateTimePicker();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.dtpBeginTime = new System.Windows.Forms.DateTimePicker();
            this.dtpDate = new System.Windows.Forms.DateTimePicker();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel3.Controls.Add(this.labelTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 25);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(892, 57);
            this.panel3.TabIndex = 51;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(336, 14);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(228, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "集体考勤异常信息";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.新建toolStripButton1,
            this.toolStripSeparator3,
            this.添加toolStripButton1,
            this.toolStripSeparator2,
            this.删除toolStripButton2,
            this.toolStripSeparator1,
            this.审核toolStripButton1,
            this.toolStripSeparator6,
            this.刷新toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(892, 25);
            this.toolStrip1.TabIndex = 50;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 新建toolStripButton1
            // 
            this.新建toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.新建toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("新建toolStripButton1.Image")));
            this.新建toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.新建toolStripButton1.Name = "新建toolStripButton1";
            this.新建toolStripButton1.Size = new System.Drawing.Size(40, 22);
            this.新建toolStripButton1.Text = "新 建";
            this.新建toolStripButton1.Click += new System.EventHandler(this.新建toolStripButton1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "Add";
            // 
            // 添加toolStripButton1
            // 
            this.添加toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加toolStripButton1.Name = "添加toolStripButton1";
            this.添加toolStripButton1.Size = new System.Drawing.Size(40, 22);
            this.添加toolStripButton1.Tag = "Add";
            this.添加toolStripButton1.Text = "提 交";
            this.添加toolStripButton1.Click += new System.EventHandler(this.添加toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "Add";
            // 
            // 删除toolStripButton2
            // 
            this.删除toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除toolStripButton2.Name = "删除toolStripButton2";
            this.删除toolStripButton2.Size = new System.Drawing.Size(40, 22);
            this.删除toolStripButton2.Tag = "delete";
            this.删除toolStripButton2.Text = "删 除";
            this.删除toolStripButton2.Click += new System.EventHandler(this.删除toolStripButton2_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Tag = "Add";
            // 
            // 审核toolStripButton1
            // 
            this.审核toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.审核toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("审核toolStripButton1.Image")));
            this.审核toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.审核toolStripButton1.Name = "审核toolStripButton1";
            this.审核toolStripButton1.Size = new System.Drawing.Size(60, 22);
            this.审核toolStripButton1.Tag = "Auditing";
            this.审核toolStripButton1.Text = "主管审核";
            this.审核toolStripButton1.Click += new System.EventHandler(this.审核toolStripButton1_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator6.Tag = "Auditing";
            // 
            // 刷新toolStripButton1
            // 
            this.刷新toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton1.Name = "刷新toolStripButton1";
            this.刷新toolStripButton1.Size = new System.Drawing.Size(60, 22);
            this.刷新toolStripButton1.Tag = "View";
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.txtDirector);
            this.panel1.Controls.Add(this.txtRecorder);
            this.panel1.Controls.Add(this.txtDescription);
            this.panel1.Controls.Add(this.dtpSignatureDate);
            this.panel1.Controls.Add(this.dtpRecordTime);
            this.panel1.Controls.Add(this.dtpEndTime);
            this.panel1.Controls.Add(this.dtpBeginTime);
            this.panel1.Controls.Add(this.dtpDate);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 82);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(892, 144);
            this.panel1.TabIndex = 52;
            // 
            // txtDirector
            // 
            this.txtDirector.BackColor = System.Drawing.SystemColors.Window;
            this.txtDirector.Location = new System.Drawing.Point(536, 107);
            this.txtDirector.Name = "txtDirector";
            this.txtDirector.ReadOnly = true;
            this.txtDirector.Size = new System.Drawing.Size(87, 23);
            this.txtDirector.TabIndex = 15;
            // 
            // txtRecorder
            // 
            this.txtRecorder.BackColor = System.Drawing.SystemColors.Window;
            this.txtRecorder.Location = new System.Drawing.Point(74, 107);
            this.txtRecorder.Name = "txtRecorder";
            this.txtRecorder.ReadOnly = true;
            this.txtRecorder.Size = new System.Drawing.Size(115, 23);
            this.txtRecorder.TabIndex = 14;
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(74, 39);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(811, 60);
            this.txtDescription.TabIndex = 13;
            // 
            // dtpSignatureDate
            // 
            this.dtpSignatureDate.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpSignatureDate.Enabled = false;
            this.dtpSignatureDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpSignatureDate.Location = new System.Drawing.Point(726, 105);
            this.dtpSignatureDate.Name = "dtpSignatureDate";
            this.dtpSignatureDate.Size = new System.Drawing.Size(159, 23);
            this.dtpSignatureDate.TabIndex = 12;
            // 
            // dtpRecordTime
            // 
            this.dtpRecordTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpRecordTime.Enabled = false;
            this.dtpRecordTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpRecordTime.Location = new System.Drawing.Point(305, 105);
            this.dtpRecordTime.Name = "dtpRecordTime";
            this.dtpRecordTime.Size = new System.Drawing.Size(146, 23);
            this.dtpRecordTime.TabIndex = 11;
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpEndTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpEndTime.Location = new System.Drawing.Point(716, 10);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(169, 23);
            this.dtpEndTime.TabIndex = 10;
            // 
            // dtpBeginTime
            // 
            this.dtpBeginTime.CustomFormat = "yyyy-MM-dd HH:mm";
            this.dtpBeginTime.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpBeginTime.Location = new System.Drawing.Point(400, 10);
            this.dtpBeginTime.Name = "dtpBeginTime";
            this.dtpBeginTime.Size = new System.Drawing.Size(169, 23);
            this.dtpBeginTime.TabIndex = 9;
            // 
            // dtpDate
            // 
            this.dtpDate.Location = new System.Drawing.Point(74, 10);
            this.dtpDate.Name = "dtpDate";
            this.dtpDate.Size = new System.Drawing.Size(177, 23);
            this.dtpDate.TabIndex = 8;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Green;
            this.label8.Location = new System.Drawing.Point(209, 110);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(84, 14);
            this.label8.TabIndex = 7;
            this.label8.Text = "记 录 时 间";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.Color.Green;
            this.label7.Location = new System.Drawing.Point(5, 110);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 6;
            this.label7.Text = "记 录 员";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Green;
            this.label6.Location = new System.Drawing.Point(629, 110);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(91, 14);
            this.label6.TabIndex = 5;
            this.label6.Text = "主管签字时间";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Green;
            this.label5.Location = new System.Drawing.Point(467, 110);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 4;
            this.label5.Text = "主管签字";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(5, 58);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 3;
            this.label4.Text = "异常描述";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(619, 15);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "异常截止时间";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(304, 15);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 1;
            this.label2.Text = "异常开始时间";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(5, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "异常日期";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Controls.Add(this.userControlDataLocalizer1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 226);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(892, 314);
            this.panel2.TabIndex = 53;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 32);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(892, 282);
            this.dataGridView1.TabIndex = 1;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(892, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // UserControlBatchException
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 540);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlBatchException";
            this.Load += new System.EventHandler(this.UserControlBatchException_Load);
            this.Resize += new System.EventHandler(this.UserControlBatchException_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 添加toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripButton 审核toolStripButton1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.DateTimePicker dtpBeginTime;
        private System.Windows.Forms.DateTimePicker dtpDate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDirector;
        private System.Windows.Forms.TextBox txtRecorder;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.DateTimePicker dtpSignatureDate;
        private System.Windows.Forms.DateTimePicker dtpRecordTime;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton 新建toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
    }
}
