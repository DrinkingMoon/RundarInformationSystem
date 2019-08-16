using UniversalControlLibrary;
namespace Expression
{
    partial class 重新打印
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(重新打印));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.tlsbDelete = new System.Windows.Forms.ToolStripButton();
            this.btnAuditing = new System.Windows.Forms.ToolStripButton();
            this.btnAuthorize = new System.Windows.Forms.ToolStripButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.btnSearch = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.dateTime_endTime = new System.Windows.Forms.DateTimePicker();
            this.dateTime_startTime = new System.Windows.Forms.DateTimePicker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnSelect = new System.Windows.Forms.Button();
            this.txtPrintRemark = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtBill_ID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvShowInfo = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.toolStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvShowInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.toolStrip1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.tlsbDelete,
            this.btnAuditing,
            this.btnAuthorize});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(983, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.Image = global::UniversalControlLibrary.Properties.Resources.add;
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnAdd.Size = new System.Drawing.Size(62, 22);
            this.btnAdd.Tag = "Add";
            this.btnAdd.Text = "添 加";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // tlsbDelete
            // 
            this.tlsbDelete.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.tlsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbDelete.Name = "tlsbDelete";
            this.tlsbDelete.Size = new System.Drawing.Size(62, 22);
            this.tlsbDelete.Tag = "DELETE";
            this.tlsbDelete.Text = "删 除";
            this.tlsbDelete.Click += new System.EventHandler(this.tlsbDelete_Click);
            // 
            // btnAuditing
            // 
            this.btnAuditing.Image = ((System.Drawing.Image)(resources.GetObject("btnAuditing.Image")));
            this.btnAuditing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAuditing.Name = "btnAuditing";
            this.btnAuditing.Size = new System.Drawing.Size(83, 22);
            this.btnAuditing.Tag = "AUDITING";
            this.btnAuditing.Text = "主管审核";
            this.btnAuditing.Click += new System.EventHandler(this.btnAuditing_Click);
            // 
            // btnAuthorize
            // 
            this.btnAuthorize.Image = global::UniversalControlLibrary.Properties.Resources.greentick;
            this.btnAuthorize.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAuthorize.Name = "btnAuthorize";
            this.btnAuthorize.Size = new System.Drawing.Size(83, 22);
            this.btnAuthorize.Tag = "AUTHORIZE";
            this.btnAuthorize.Text = "财务批准";
            this.btnAuthorize.Click += new System.EventHandler(this.btnAuthorize_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel2.Controls.Add(this.labelTitle);
            this.panel2.Location = new System.Drawing.Point(0, 27);
            this.panel2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(991, 51);
            this.panel2.TabIndex = 225;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(452, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "重新打印";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox2);
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(983, 146);
            this.panel1.TabIndex = 265;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.btnSearch);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.dateTime_endTime);
            this.groupBox2.Controls.Add(this.dateTime_startTime);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(478, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(505, 146);
            this.groupBox2.TabIndex = 265;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "根据时间查询申请重打单信息";
            // 
            // btnSearch
            // 
            this.btnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSearch.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSearch.Image = global::UniversalControlLibrary.Properties.Resources.Search;
            this.btnSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSearch.Location = new System.Drawing.Point(243, 47);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(91, 38);
            this.btnSearch.TabIndex = 266;
            this.btnSearch.Tag = "View";
            this.btnSearch.Text = "查  询";
            this.btnSearch.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(21, 14);
            this.label3.TabIndex = 265;
            this.label3.Text = "到";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 45);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(21, 14);
            this.label4.TabIndex = 264;
            this.label4.Text = "从";
            // 
            // dateTime_endTime
            // 
            this.dateTime_endTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTime_endTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime_endTime.Location = new System.Drawing.Point(44, 77);
            this.dateTime_endTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTime_endTime.Name = "dateTime_endTime";
            this.dateTime_endTime.Size = new System.Drawing.Size(193, 23);
            this.dateTime_endTime.TabIndex = 263;
            this.dateTime_endTime.Value = new System.DateTime(2011, 9, 27, 0, 0, 0, 0);
            // 
            // dateTime_startTime
            // 
            this.dateTime_startTime.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.dateTime_startTime.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTime_startTime.Location = new System.Drawing.Point(44, 40);
            this.dateTime_startTime.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dateTime_startTime.Name = "dateTime_startTime";
            this.dateTime_startTime.Size = new System.Drawing.Size(193, 23);
            this.dateTime_startTime.TabIndex = 262;
            this.dateTime_startTime.Value = new System.DateTime(2011, 9, 27, 0, 0, 0, 0);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnSelect);
            this.groupBox1.Controls.Add(this.txtPrintRemark);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.txtBill_ID);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Left;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(478, 146);
            this.groupBox1.TabIndex = 264;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "申请重打单";
            // 
            // btnSelect
            // 
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnSelect.Location = new System.Drawing.Point(381, 31);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Size = new System.Drawing.Size(80, 23);
            this.btnSelect.TabIndex = 271;
            this.btnSelect.Text = "单据查询";
            this.toolTip1.SetToolTip(this.btnSelect, "查询指定单据号的打印记录");
            this.btnSelect.UseVisualStyleBackColor = true;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // txtPrintRemark
            // 
            this.txtPrintRemark.Location = new System.Drawing.Point(105, 60);
            this.txtPrintRemark.Multiline = true;
            this.txtPrintRemark.Name = "txtPrintRemark";
            this.txtPrintRemark.Size = new System.Drawing.Size(356, 80);
            this.txtPrintRemark.TabIndex = 270;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.ForeColor = System.Drawing.Color.Blue;
            this.label8.Location = new System.Drawing.Point(8, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 14);
            this.label8.TabIndex = 269;
            this.label8.Text = "重新打印原因";
            // 
            // txtBill_ID
            // 
            this.txtBill_ID.BackColor = System.Drawing.Color.White;
            this.txtBill_ID.ForeColor = System.Drawing.Color.Red;
            this.txtBill_ID.Location = new System.Drawing.Point(105, 30);
            this.txtBill_ID.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtBill_ID.Name = "txtBill_ID";
            this.txtBill_ID.Size = new System.Drawing.Size(270, 23);
            this.txtBill_ID.TabIndex = 268;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(36, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 267;
            this.label2.Text = " 单据号 ";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.groupBox3);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 171);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(983, 387);
            this.panel3.TabIndex = 266;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvShowInfo);
            this.groupBox3.Controls.Add(this.userControlDataLocalizer1);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(0, 0);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(983, 387);
            this.groupBox3.TabIndex = 265;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "信息显示";
            // 
            // dgvShowInfo
            // 
            this.dgvShowInfo.AllowUserToAddRows = false;
            this.dgvShowInfo.AllowUserToDeleteRows = false;
            this.dgvShowInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvShowInfo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvShowInfo.Location = new System.Drawing.Point(3, 51);
            this.dgvShowInfo.Name = "dgvShowInfo";
            this.dgvShowInfo.ReadOnly = true;
            this.dgvShowInfo.RowHeadersWidth = 30;
            this.dgvShowInfo.RowTemplate.Height = 23;
            this.dgvShowInfo.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvShowInfo.Size = new System.Drawing.Size(977, 333);
            this.dgvShowInfo.TabIndex = 268;
            this.dgvShowInfo.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvShowInfo_CellClick);
            this.dgvShowInfo.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dgvShowInfo_DataBindingComplete);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(3, 19);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(977, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 7;
            // 
            // 重新打印
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(983, 558);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "重新打印";
            this.Load += new System.EventHandler(this.重新打印_Load);
            this.Resize += new System.EventHandler(this.重新打印_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvShowInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnAuditing;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripButton btnAuthorize;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.ToolStripButton tlsbDelete;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button btnSearch;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DateTimePicker dateTime_endTime;
        private System.Windows.Forms.DateTimePicker dateTime_startTime;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnSelect;
        private System.Windows.Forms.TextBox txtPrintRemark;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtBill_ID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvShowInfo;
        private UserControlDataLocalizer userControlDataLocalizer1;


    }
}
