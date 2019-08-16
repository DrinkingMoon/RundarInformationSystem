using UniversalControlLibrary;
namespace Expression
{
    partial class 外部库存查询与修改
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(外部库存查询与修改));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tlsbNew = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbAdd = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorDelete = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbUpdate = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorAdd = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbDelete = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparatorFind = new System.Windows.Forms.ToolStripSeparator();
            this.tlsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lbUnit = new System.Windows.Forms.Label();
            this.nudStockCount = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.txtCode = new TextBoxShow();
            this.txtSpce = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStockCount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tlsbNew,
            this.toolStripSeparator1,
            this.tlsbAdd,
            this.toolStripSeparatorDelete,
            this.tlsbUpdate,
            this.toolStripSeparatorAdd,
            this.tlsbDelete,
            this.toolStripSeparatorFind,
            this.tlsbRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(992, 25);
            this.toolStrip1.TabIndex = 58;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tlsbNew
            // 
            this.tlsbNew.Image = ((System.Drawing.Image)(resources.GetObject("tlsbNew.Image")));
            this.tlsbNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbNew.Name = "tlsbNew";
            this.tlsbNew.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tlsbNew.Size = new System.Drawing.Size(67, 22);
            this.tlsbNew.Tag = "Add";
            this.tlsbNew.Text = "新建(&N)";
            this.tlsbNew.Click += new System.EventHandler(this.tlsbNew_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsbAdd
            // 
            this.tlsbAdd.Image = ((System.Drawing.Image)(resources.GetObject("tlsbAdd.Image")));
            this.tlsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbAdd.Name = "tlsbAdd";
            this.tlsbAdd.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.tlsbAdd.Size = new System.Drawing.Size(67, 22);
            this.tlsbAdd.Tag = "Add";
            this.tlsbAdd.Text = "添加(&A)";
            this.tlsbAdd.Click += new System.EventHandler(this.tlsbAdd_Click);
            // 
            // toolStripSeparatorDelete
            // 
            this.toolStripSeparatorDelete.Name = "toolStripSeparatorDelete";
            this.toolStripSeparatorDelete.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsbUpdate
            // 
            this.tlsbUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tlsbUpdate.Image")));
            this.tlsbUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbUpdate.Name = "tlsbUpdate";
            this.tlsbUpdate.Size = new System.Drawing.Size(67, 22);
            this.tlsbUpdate.Tag = "update";
            this.tlsbUpdate.Text = "修改(&U)";
            this.tlsbUpdate.Click += new System.EventHandler(this.tlsbUpdate_Click);
            // 
            // toolStripSeparatorAdd
            // 
            this.toolStripSeparatorAdd.Name = "toolStripSeparatorAdd";
            this.toolStripSeparatorAdd.Size = new System.Drawing.Size(6, 25);
            this.toolStripSeparatorAdd.Visible = false;
            // 
            // tlsbDelete
            // 
            this.tlsbDelete.Image = ((System.Drawing.Image)(resources.GetObject("tlsbDelete.Image")));
            this.tlsbDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbDelete.Name = "tlsbDelete";
            this.tlsbDelete.Size = new System.Drawing.Size(67, 22);
            this.tlsbDelete.Tag = "Delete";
            this.tlsbDelete.Text = "删除(&D)";
            this.tlsbDelete.Visible = false;
            this.tlsbDelete.Click += new System.EventHandler(this.tlsbDelete_Click);
            // 
            // toolStripSeparatorFind
            // 
            this.toolStripSeparatorFind.Name = "toolStripSeparatorFind";
            this.toolStripSeparatorFind.Size = new System.Drawing.Size(6, 25);
            // 
            // tlsbRefresh
            // 
            this.tlsbRefresh.Image = ((System.Drawing.Image)(resources.GetObject("tlsbRefresh.Image")));
            this.tlsbRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tlsbRefresh.Name = "tlsbRefresh";
            this.tlsbRefresh.Size = new System.Drawing.Size(49, 22);
            this.tlsbRefresh.Tag = "view";
            this.tlsbRefresh.Text = "刷新";
            this.tlsbRefresh.Click += new System.EventHandler(this.tlsbRefresh_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(992, 62);
            this.panel1.TabIndex = 59;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(348, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(255, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "外部库存查询与修改";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox2);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.lbUnit);
            this.groupBox1.Controls.Add(this.nudStockCount);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Controls.Add(this.txtSpce);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmbStorage);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 87);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(992, 135);
            this.groupBox1.TabIndex = 63;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息区";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(659, 90);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(287, 23);
            this.textBox2.TabIndex = 219;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(659, 61);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(287, 23);
            this.textBox1.TabIndex = 218;
            // 
            // lbUnit
            // 
            this.lbUnit.AutoSize = true;
            this.lbUnit.Location = new System.Drawing.Point(631, 70);
            this.lbUnit.Name = "lbUnit";
            this.lbUnit.Size = new System.Drawing.Size(0, 14);
            this.lbUnit.TabIndex = 217;
            // 
            // nudStockCount
            // 
            this.nudStockCount.DecimalPlaces = 3;
            this.nudStockCount.Location = new System.Drawing.Point(414, 63);
            this.nudStockCount.Maximum = new decimal(new int[] {
            1410065408,
            2,
            0,
            0});
            this.nudStockCount.Name = "nudStockCount";
            this.nudStockCount.Size = new System.Drawing.Size(209, 23);
            this.nudStockCount.TabIndex = 216;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(345, 67);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 215;
            this.label1.Text = "库存数量";
            // 
            // txtCode
            // 
            this.txtCode.Enabled = false;
            this.txtCode.FindItem = TextBoxShow.FindType.所有物品;
            this.txtCode.Location = new System.Drawing.Point(92, 22);
            this.txtCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtCode.Name = "txtCode";
            this.txtCode.Size = new System.Drawing.Size(224, 23);
            this.txtCode.TabIndex = 214;
            // 
            // txtSpce
            // 
            this.txtSpce.BackColor = System.Drawing.Color.White;
            this.txtSpce.ForeColor = System.Drawing.Color.Black;
            this.txtSpce.Location = new System.Drawing.Point(765, 22);
            this.txtSpce.Name = "txtSpce";
            this.txtSpce.ReadOnly = true;
            this.txtSpce.Size = new System.Drawing.Size(181, 23);
            this.txtSpce.TabIndex = 213;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(699, 26);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(53, 12);
            this.label9.TabIndex = 212;
            this.label9.Text = "规    格";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(414, 22);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(255, 23);
            this.txtName.TabIndex = 211;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(345, 26);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 210;
            this.label7.Text = "物品名称";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(22, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 209;
            this.label5.Text = "图号型号";
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.Enabled = false;
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(92, 63);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(224, 21);
            this.cmbStorage.TabIndex = 208;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(23, 67);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 14);
            this.label13.TabIndex = 207;
            this.label13.Text = "所属库房";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 275);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(992, 592);
            this.dataGridView1.TabIndex = 70;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 222);
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(992, 53);
            this.checkBillDateAndStatus1.TabIndex = 69;
            // 
            // 外部库存查询与修改
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "外部库存查询与修改";
            this.Size = new System.Drawing.Size(992, 867);
            this.Load += new System.EventHandler(this.外部库存查询与修改_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudStockCount)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.ToolStripButton tlsbAdd;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorAdd;
        private System.Windows.Forms.ToolStripButton tlsbDelete;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorDelete;
        private System.Windows.Forms.ToolStripButton tlsbUpdate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparatorFind;
        private System.Windows.Forms.ToolStripButton tlsbRefresh;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.Label label13;
        private TextBoxShow txtCode;
        private System.Windows.Forms.TextBox txtSpce;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown nudStockCount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripButton tlsbNew;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.Label lbUnit;
        private CheckBillDateAndStatus checkBillDateAndStatus1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;

    }
}
