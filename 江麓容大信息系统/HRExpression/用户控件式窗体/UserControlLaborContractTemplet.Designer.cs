using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class UserControlLaborContractTemplet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UserControlLaborContractTemplet));
            this.panel3 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.修改toolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.删除toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.刷新toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.llbAddAnnex = new System.Windows.Forms.LinkLabel();
            this.llbLoadAnnex = new System.Windows.Forms.LinkLabel();
            this.lblAnnxeName = new System.Windows.Forms.Label();
            this.cmbLaborType = new System.Windows.Forms.ComboBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.plChoose = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel3.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
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
            this.panel3.Size = new System.Drawing.Size(908, 50);
            this.panel3.TabIndex = 37;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(387, 12);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "合同模版";
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加toolStripButton1,
            this.toolStripSeparator4,
            this.修改toolStripButton,
            this.toolStripSeparator2,
            this.删除toolStripButton1,
            this.toolStripSeparator3,
            this.刷新toolStripButton1,
            this.toolStripSeparator1,
            this.toolStripButton2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(908, 25);
            this.toolStrip1.TabIndex = 36;
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
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // 修改toolStripButton
            // 
            this.修改toolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.修改toolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("修改toolStripButton.Image")));
            this.修改toolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改toolStripButton.Name = "修改toolStripButton";
            this.修改toolStripButton.Size = new System.Drawing.Size(33, 22);
            this.修改toolStripButton.Tag = "update";
            this.修改toolStripButton.Text = "修改";
            this.修改toolStripButton.Click += new System.EventHandler(this.修改toolStripButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // 删除toolStripButton1
            // 
            this.删除toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除toolStripButton1.Name = "删除toolStripButton1";
            this.删除toolStripButton1.Size = new System.Drawing.Size(39, 22);
            this.删除toolStripButton1.Tag = "delete";
            this.删除toolStripButton1.Text = "删 除";
            this.删除toolStripButton1.Click += new System.EventHandler(this.删除toolStripButton1_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparator3.Tag = "delete";
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
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(81, 22);
            this.toolStripButton2.Text = "变更合同类别";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.llbAddAnnex);
            this.panel1.Controls.Add(this.llbLoadAnnex);
            this.panel1.Controls.Add(this.lblAnnxeName);
            this.panel1.Controls.Add(this.cmbLaborType);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtVersion);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.panel1.Location = new System.Drawing.Point(0, 75);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(908, 152);
            this.panel1.TabIndex = 30;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(577, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(98, 14);
            this.label4.TabIndex = 1036;
            this.label4.Text = "(例如：10.00)";
            // 
            // llbAddAnnex
            // 
            this.llbAddAnnex.AutoSize = true;
            this.llbAddAnnex.Location = new System.Drawing.Point(262, 101);
            this.llbAddAnnex.Name = "llbAddAnnex";
            this.llbAddAnnex.Size = new System.Drawing.Size(84, 14);
            this.llbAddAnnex.TabIndex = 1034;
            this.llbAddAnnex.TabStop = true;
            this.llbAddAnnex.Text = "上 传 附 件";
            this.llbAddAnnex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbAddAnnex_LinkClicked);
            // 
            // llbLoadAnnex
            // 
            this.llbLoadAnnex.AutoSize = true;
            this.llbLoadAnnex.Location = new System.Drawing.Point(390, 101);
            this.llbLoadAnnex.Name = "llbLoadAnnex";
            this.llbLoadAnnex.Size = new System.Drawing.Size(84, 14);
            this.llbLoadAnnex.TabIndex = 1035;
            this.llbLoadAnnex.TabStop = true;
            this.llbLoadAnnex.Text = "下 载 附 件";
            this.llbLoadAnnex.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbLoadAnnex_LinkClicked);
            // 
            // lblAnnxeName
            // 
            this.lblAnnxeName.AutoSize = true;
            this.lblAnnxeName.Location = new System.Drawing.Point(198, 129);
            this.lblAnnxeName.Name = "lblAnnxeName";
            this.lblAnnxeName.Size = new System.Drawing.Size(63, 14);
            this.lblAnnxeName.TabIndex = 8;
            this.lblAnnxeName.Text = "附件名：";
            // 
            // cmbLaborType
            // 
            this.cmbLaborType.FormattingEnabled = true;
            this.cmbLaborType.Location = new System.Drawing.Point(273, 13);
            this.cmbLaborType.Name = "cmbLaborType";
            this.cmbLaborType.Size = new System.Drawing.Size(140, 21);
            this.cmbLaborType.TabIndex = 6;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(273, 64);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(448, 23);
            this.txtRemark.TabIndex = 5;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(198, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "备  注";
            // 
            // txtVersion
            // 
            this.txtVersion.Location = new System.Drawing.Point(581, 13);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(140, 23);
            this.txtVersion.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(500, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "合同版本";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(192, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "合同类别";
            // 
            // plChoose
            // 
            this.plChoose.Controls.Add(this.userControlDataLocalizer1);
            this.plChoose.Dock = System.Windows.Forms.DockStyle.Top;
            this.plChoose.Location = new System.Drawing.Point(0, 227);
            this.plChoose.Name = "plChoose";
            this.plChoose.Size = new System.Drawing.Size(908, 35);
            this.plChoose.TabIndex = 40;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 0);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(908, 35);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 262);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(908, 317);
            this.panel2.TabIndex = 41;
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
            this.dataGridView1.Size = new System.Drawing.Size(908, 317);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            this.dataGridView1.ColumnWidthChanged += new System.Windows.Forms.DataGridViewColumnEventHandler(this.dataGridView1_ColumnWidthChanged);
            // 
            // UserControlLaborContractTemplet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(908, 579);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.plChoose);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "UserControlLaborContractTemplet";
            this.Load += new System.EventHandler(this.UserControlLaborContractTemplet_Load);
            this.Resize += new System.EventHandler(this.UserControlLaborContractTemplet_Resize);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.ToolStripButton 刷新toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbLaborType;
        private System.Windows.Forms.Label lblAnnxeName;
        private System.Windows.Forms.LinkLabel llbAddAnnex;
        private System.Windows.Forms.LinkLabel llbLoadAnnex;
        private System.Windows.Forms.Panel plChoose;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ToolStripButton 添加toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.Label label4;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton 修改toolStripButton;
    }
}
