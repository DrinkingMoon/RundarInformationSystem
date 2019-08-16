using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlPersonnelLaborContract
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.修改toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.变更合同状态toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.导出toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.综合查询toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProposer = new UniversalControlLibrary.TextBoxShow();
            this.btnFindTemplet = new System.Windows.Forms.Button();
            this.txtTemplet = new System.Windows.Forms.TextBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.dtpEndTime = new System.Windows.Forms.DateTimePicker();
            this.label5 = new System.Windows.Forms.Label();
            this.dtpBeginTime = new System.Windows.Forms.DateTimePicker();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.plChoose = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.plChoose.SuspendLayout();
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
            this.panel3.Size = new System.Drawing.Size(1006, 50);
            this.panel3.TabIndex = 39;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(405, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "员工合同信息";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加toolStripButton1,
            this.toolStripSeparator2,
            this.修改toolStripButton,
            this.toolStripSeparator3,
            this.刷新toolStripButton1,
            this.toolStripSeparator1,
            this.变更合同状态toolStripButton,
            this.toolStripSeparator4,
            this.导出toolStripButton3,
            this.toolStripSeparator5,
            this.综合查询toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1006, 25);
            this.toolStrip1.TabIndex = 38;
            this.toolStrip1.Tag = "View";
            this.toolStrip1.Text = "toolStrip1";
            // 
            // 添加toolStripButton1
            // 
            this.添加toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加toolStripButton1.Name = "添加toolStripButton1";
            this.添加toolStripButton1.Size = new System.Drawing.Size(39, 22);
            this.添加toolStripButton1.Tag = "Add";
            this.添加toolStripButton1.Text = "添 加";
            this.添加toolStripButton1.Click += new System.EventHandler(this.添加toolStripButton1_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator2.Tag = "Add";
            // 
            // 修改toolStripButton
            // 
            this.修改toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改toolStripButton.Name = "修改toolStripButton";
            this.修改toolStripButton.Size = new System.Drawing.Size(39, 22);
            this.修改toolStripButton.Tag = "update";
            this.修改toolStripButton.Text = "修 改";
            this.修改toolStripButton.Click += new System.EventHandler(this.修改toolStripButton_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "Add";
            // 
            // 刷新toolStripButton1
            // 
            this.刷新toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.刷新toolStripButton1.Name = "刷新toolStripButton1";
            this.刷新toolStripButton1.Size = new System.Drawing.Size(57, 22);
            this.刷新toolStripButton1.Tag = "Add";
            this.刷新toolStripButton1.Text = "刷新数据";
            this.刷新toolStripButton1.Click += new System.EventHandler(this.刷新toolStripButton1_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator1.Visible = false;
            // 
            // 变更合同状态toolStripButton
            // 
            this.变更合同状态toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.变更合同状态toolStripButton.Name = "变更合同状态toolStripButton";
            this.变更合同状态toolStripButton.Size = new System.Drawing.Size(81, 22);
            this.变更合同状态toolStripButton.Tag = "Add";
            this.变更合同状态toolStripButton.Text = "变更合同状态";
            this.变更合同状态toolStripButton.Visible = false;
            this.变更合同状态toolStripButton.Click += new System.EventHandler(this.变更合同状态toolStripButton_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 导出toolStripButton3
            // 
            this.导出toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.导出toolStripButton3.Name = "导出toolStripButton3";
            this.导出toolStripButton3.Size = new System.Drawing.Size(87, 22);
            this.导出toolStripButton3.Tag = "导出文件";
            this.导出toolStripButton3.Text = "导出Excel文件";
            this.导出toolStripButton3.Click += new System.EventHandler(this.导出toolStripButton3_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // 综合查询toolStripButton3
            // 
            this.综合查询toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.综合查询toolStripButton3.Name = "综合查询toolStripButton3";
            this.综合查询toolStripButton3.Size = new System.Drawing.Size(57, 22);
            this.综合查询toolStripButton3.Tag = "导出文件";
            this.综合查询toolStripButton3.Text = "综合查询";
            this.综合查询toolStripButton3.Click += new System.EventHandler(this.综合查询toolStripButton3_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtProposer);
            this.groupBox1.Controls.Add(this.btnFindTemplet);
            this.groupBox1.Controls.Add(this.txtTemplet);
            this.groupBox1.Controls.Add(this.txtRemark);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.dtpEndTime);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.dtpBeginTime);
            this.groupBox1.Controls.Add(this.cmbStatus);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 75);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1006, 115);
            this.groupBox1.TabIndex = 40;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "合同信息";
            // 
            // txtProposer
            // 
            this.txtProposer.FindItem = UniversalControlLibrary.TextBoxShow.FindType.员工;
            this.txtProposer.Location = new System.Drawing.Point(117, 30);
            this.txtProposer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProposer.Name = "txtProposer";
            this.txtProposer.Size = new System.Drawing.Size(144, 23);
            this.txtProposer.TabIndex = 208;
            this.txtProposer.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProposer_OnCompleteSearch);
            // 
            // btnFindTemplet
            // 
            this.btnFindTemplet.BackColor = System.Drawing.Color.Transparent;
            this.btnFindTemplet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.btnFindTemplet.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.btnFindTemplet.Image = global::UniversalControlLibrary.Properties.Resources.find;
            this.btnFindTemplet.Location = new System.Drawing.Point(489, 30);
            this.btnFindTemplet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnFindTemplet.Name = "btnFindTemplet";
            this.btnFindTemplet.Size = new System.Drawing.Size(26, 22);
            this.btnFindTemplet.TabIndex = 207;
            this.btnFindTemplet.UseVisualStyleBackColor = false;
            this.btnFindTemplet.Click += new System.EventHandler(this.btnFindTemplet_Click);
            // 
            // txtTemplet
            // 
            this.txtTemplet.BackColor = System.Drawing.SystemColors.Window;
            this.txtTemplet.Location = new System.Drawing.Point(370, 30);
            this.txtTemplet.Name = "txtTemplet";
            this.txtTemplet.ReadOnly = true;
            this.txtTemplet.Size = new System.Drawing.Size(116, 23);
            this.txtTemplet.TabIndex = 206;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(622, 69);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(343, 21);
            this.txtRemark.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(575, 74);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 14);
            this.label6.TabIndex = 11;
            this.label6.Text = "备注";
            // 
            // dtpEndTime
            // 
            this.dtpEndTime.Location = new System.Drawing.Point(370, 69);
            this.dtpEndTime.Name = "dtpEndTime";
            this.dtpEndTime.Size = new System.Drawing.Size(140, 23);
            this.dtpEndTime.TabIndex = 10;
            this.dtpEndTime.Value = new System.DateTime(2012, 8, 7, 0, 0, 0, 0);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(273, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(91, 14);
            this.label5.TabIndex = 9;
            this.label5.Text = "合同终止时间";
            // 
            // dtpBeginTime
            // 
            this.dtpBeginTime.Location = new System.Drawing.Point(117, 69);
            this.dtpBeginTime.Name = "dtpBeginTime";
            this.dtpBeginTime.Size = new System.Drawing.Size(144, 23);
            this.dtpBeginTime.TabIndex = 8;
            this.dtpBeginTime.Value = new System.DateTime(2012, 8, 7, 0, 0, 0, 0);
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(622, 30);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(121, 21);
            this.cmbStatus.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(547, 35);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "合同状态";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(301, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "合同模板";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(20, 74);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "合同起始时间";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(48, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "员工姓名";
            // 
            // plChoose
            // 
            this.plChoose.Controls.Add(this.userControlDataLocalizer1);
            this.plChoose.Dock = System.Windows.Forms.DockStyle.Top;
            this.plChoose.Location = new System.Drawing.Point(0, 190);
            this.plChoose.Name = "plChoose";
            this.plChoose.Size = new System.Drawing.Size(1006, 35);
            this.plChoose.TabIndex = 41;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1006, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 225);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1006, 352);
            this.panel2.TabIndex = 42;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1006, 352);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.Resize += new System.EventHandler(this.dataGridView1_Resize);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
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
            // UserControlPersonnelLaborContract
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 577);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.plChoose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlPersonnelLaborContract";
            this.Load += new System.EventHandler(this.UserControlPersonnelLaborContract_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.plChoose.ResumeLayout(false);
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
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton 变更合同状态toolStripButton;
        private System.Windows.Forms.Panel plChoose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dtpEndTime;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DateTimePicker dtpBeginTime;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFindTemplet;
        public System.Windows.Forms.TextBox txtTemplet;
        private System.Windows.Forms.ToolStripButton 修改toolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private TextBoxShow txtProposer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 导出toolStripButton3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton 综合查询toolStripButton3;
        private UserControlDataLocalizer userControlDataLocalizer1;
        public System.Windows.Forms.GroupBox groupBox1;
    }
}
