using UniversalControlLibrary;
namespace Expression
{
    partial class 库存物品防锈
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
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.全部选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部取消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chkIsNormal = new System.Windows.Forms.CheckBox();
            this.labelTitle = new System.Windows.Forms.Label();
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.仓管员操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询物品防锈设置ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.执行防锈操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.质管部操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.审核库存防锈ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.质量工程师操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.确认防锈操作ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.查询库存的防锈状况ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.导出EXCELToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudAntirust = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbsCode = new UniversalControlLibrary.TextBoxShow();
            this.label4 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.menuStrip.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntirust)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全部选择ToolStripMenuItem,
            this.全部取消ToolStripMenuItem,
            this.选择ToolStripMenuItem,
            this.取消ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 92);
            // 
            // 全部选择ToolStripMenuItem
            // 
            this.全部选择ToolStripMenuItem.Name = "全部选择ToolStripMenuItem";
            this.全部选择ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.全部选择ToolStripMenuItem.Text = "全部选择";
            this.全部选择ToolStripMenuItem.Click += new System.EventHandler(this.全部选择ToolStripMenuItem_Click);
            // 
            // 全部取消ToolStripMenuItem
            // 
            this.全部取消ToolStripMenuItem.Name = "全部取消ToolStripMenuItem";
            this.全部取消ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.全部取消ToolStripMenuItem.Text = "全部取消";
            this.全部取消ToolStripMenuItem.Click += new System.EventHandler(this.全部取消ToolStripMenuItem_Click);
            // 
            // 选择ToolStripMenuItem
            // 
            this.选择ToolStripMenuItem.Name = "选择ToolStripMenuItem";
            this.选择ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.选择ToolStripMenuItem.Text = "选择";
            this.选择ToolStripMenuItem.Click += new System.EventHandler(this.选择ToolStripMenuItem_Click);
            // 
            // 取消ToolStripMenuItem
            // 
            this.取消ToolStripMenuItem.Name = "取消ToolStripMenuItem";
            this.取消ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.取消ToolStripMenuItem.Text = "取消";
            this.取消ToolStripMenuItem.Click += new System.EventHandler(this.取消ToolStripMenuItem_Click);
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
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.checkBox3);
            this.panel1.Controls.Add(this.checkBox2);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.chkIsNormal);
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 24);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1051, 49);
            this.panel1.TabIndex = 234;
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Checked = true;
            this.checkBox3.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox3.Location = new System.Drawing.Point(299, 17);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(68, 18);
            this.checkBox3.TabIndex = 4;
            this.checkBox3.Text = "已过期";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.Visible = false;
            this.checkBox3.Click += new System.EventHandler(this.chkIsNormal_CheckedChanged);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Location = new System.Drawing.Point(204, 17);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(68, 18);
            this.checkBox2.TabIndex = 3;
            this.checkBox2.Text = "预过期";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.Visible = false;
            this.checkBox2.Click += new System.EventHandler(this.chkIsNormal_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(105, 14);
            this.label3.TabIndex = 2;
            this.label3.Text = "物品防锈状态：";
            this.label3.Visible = false;
            // 
            // chkIsNormal
            // 
            this.chkIsNormal.AutoSize = true;
            this.chkIsNormal.Checked = true;
            this.chkIsNormal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIsNormal.Location = new System.Drawing.Point(128, 17);
            this.chkIsNormal.Name = "chkIsNormal";
            this.chkIsNormal.Size = new System.Drawing.Size(54, 18);
            this.chkIsNormal.TabIndex = 1;
            this.chkIsNormal.Text = "正常";
            this.chkIsNormal.UseVisualStyleBackColor = true;
            this.chkIsNormal.Visible = false;
            this.chkIsNormal.Click += new System.EventHandler(this.chkIsNormal_CheckedChanged);
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(428, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "库存物品防锈";
            // 
            // menuStrip
            // 
            this.menuStrip.BackColor = System.Drawing.Color.Transparent;
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.仓管员操作ToolStripMenuItem,
            this.质管部操作ToolStripMenuItem,
            this.质量工程师操作ToolStripMenuItem,
            this.查询库存的防锈状况ToolStripMenuItem2,
            this.导出EXCELToolStripMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip.Size = new System.Drawing.Size(1051, 24);
            this.menuStrip.TabIndex = 233;
            this.menuStrip.Text = "menuStrip1";
            // 
            // 仓管员操作ToolStripMenuItem
            // 
            this.仓管员操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.查询物品防锈设置ToolStripMenuItem1,
            this.执行防锈操作ToolStripMenuItem});
            this.仓管员操作ToolStripMenuItem.Name = "仓管员操作ToolStripMenuItem";
            this.仓管员操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.仓管员操作ToolStripMenuItem.Tag = "Add";
            this.仓管员操作ToolStripMenuItem.Text = "仓管员操作";
            // 
            // 查询物品防锈设置ToolStripMenuItem1
            // 
            this.查询物品防锈设置ToolStripMenuItem1.Name = "查询物品防锈设置ToolStripMenuItem1";
            this.查询物品防锈设置ToolStripMenuItem1.Size = new System.Drawing.Size(166, 22);
            this.查询物品防锈设置ToolStripMenuItem1.Tag = "add";
            this.查询物品防锈设置ToolStripMenuItem1.Text = "查询物品防锈设置";
            this.查询物品防锈设置ToolStripMenuItem1.Click += new System.EventHandler(this.查询物品防锈设置ToolStripMenuItem_Click);
            // 
            // 执行防锈操作ToolStripMenuItem
            // 
            this.执行防锈操作ToolStripMenuItem.Name = "执行防锈操作ToolStripMenuItem";
            this.执行防锈操作ToolStripMenuItem.Size = new System.Drawing.Size(166, 22);
            this.执行防锈操作ToolStripMenuItem.Tag = "Add";
            this.执行防锈操作ToolStripMenuItem.Text = "执行防锈操作";
            this.执行防锈操作ToolStripMenuItem.Click += new System.EventHandler(this.执行防锈操作ToolStripMenuItem_Click);
            // 
            // 质管部操作ToolStripMenuItem
            // 
            this.质管部操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.审核库存防锈ToolStripMenuItem});
            this.质管部操作ToolStripMenuItem.Name = "质管部操作ToolStripMenuItem";
            this.质管部操作ToolStripMenuItem.Size = new System.Drawing.Size(77, 20);
            this.质管部操作ToolStripMenuItem.Tag = "Auditing";
            this.质管部操作ToolStripMenuItem.Text = "检验员操作";
            // 
            // 审核库存防锈ToolStripMenuItem
            // 
            this.审核库存防锈ToolStripMenuItem.Name = "审核库存防锈ToolStripMenuItem";
            this.审核库存防锈ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.审核库存防锈ToolStripMenuItem.Tag = "Auditing";
            this.审核库存防锈ToolStripMenuItem.Text = "审核库存防锈";
            this.审核库存防锈ToolStripMenuItem.Click += new System.EventHandler(this.审核库存防锈ToolStripMenuItem_Click);
            // 
            // 质量工程师操作ToolStripMenuItem
            // 
            this.质量工程师操作ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.确认防锈操作ToolStripMenuItem});
            this.质量工程师操作ToolStripMenuItem.Name = "质量工程师操作ToolStripMenuItem";
            this.质量工程师操作ToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.质量工程师操作ToolStripMenuItem.Tag = "Authorize";
            this.质量工程师操作ToolStripMenuItem.Text = "质量工程师操作";
            // 
            // 确认防锈操作ToolStripMenuItem
            // 
            this.确认防锈操作ToolStripMenuItem.Name = "确认防锈操作ToolStripMenuItem";
            this.确认防锈操作ToolStripMenuItem.Size = new System.Drawing.Size(142, 22);
            this.确认防锈操作ToolStripMenuItem.Tag = "Authorize";
            this.确认防锈操作ToolStripMenuItem.Text = "确认防锈操作";
            this.确认防锈操作ToolStripMenuItem.Click += new System.EventHandler(this.确认防锈操作ToolStripMenuItem_Click);
            // 
            // 查询库存的防锈状况ToolStripMenuItem2
            // 
            this.查询库存的防锈状况ToolStripMenuItem2.Name = "查询库存的防锈状况ToolStripMenuItem2";
            this.查询库存的防锈状况ToolStripMenuItem2.Size = new System.Drawing.Size(125, 20);
            this.查询库存的防锈状况ToolStripMenuItem2.Tag = "view";
            this.查询库存的防锈状况ToolStripMenuItem2.Text = "查询库存的防锈状况";
            this.查询库存的防锈状况ToolStripMenuItem2.Click += new System.EventHandler(this.查询库存的防锈状况ToolStripMenuItem2_Click);
            // 
            // 导出EXCELToolStripMenuItem
            // 
            this.导出EXCELToolStripMenuItem.Name = "导出EXCELToolStripMenuItem";
            this.导出EXCELToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.导出EXCELToolStripMenuItem.Tag = "view";
            this.导出EXCELToolStripMenuItem.Text = "导出EXCEL";
            this.导出EXCELToolStripMenuItem.Click += new System.EventHandler(this.导出EXCELToolStripMenuItem_Click);
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 73);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1051, 97);
            this.panelPara.TabIndex = 243;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.nudAntirust);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAdd);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbsCode);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1051, 57);
            this.groupBox1.TabIndex = 240;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置防锈物品";
            // 
            // nudAntirust
            // 
            this.nudAntirust.Location = new System.Drawing.Point(800, 23);
            this.nudAntirust.Name = "nudAntirust";
            this.nudAntirust.Size = new System.Drawing.Size(68, 23);
            this.nudAntirust.TabIndex = 233;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(870, 27);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 14);
            this.label2.TabIndex = 232;
            this.label2.Text = "月";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(738, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 231;
            this.label1.Text = "防锈期";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(974, 22);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(70, 25);
            this.btnDelete.TabIndex = 229;
            this.btnDelete.Tag = "add";
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(897, 22);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(70, 25);
            this.btnAdd.TabIndex = 228;
            this.btnAdd.Tag = "add";
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // txtSpec
            // 
            this.txtSpec.BackColor = System.Drawing.Color.White;
            this.txtSpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSpec.ForeColor = System.Drawing.Color.Black;
            this.txtSpec.Location = new System.Drawing.Point(628, 22);
            this.txtSpec.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(104, 23);
            this.txtSpec.TabIndex = 226;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(580, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(35, 14);
            this.label5.TabIndex = 227;
            this.label5.Text = "规格";
            // 
            // txtName
            // 
            this.txtName.BackColor = System.Drawing.Color.White;
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.ForeColor = System.Drawing.Color.Black;
            this.txtName.Location = new System.Drawing.Point(421, 22);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.Size = new System.Drawing.Size(151, 23);
            this.txtName.TabIndex = 224;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(341, 26);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 225;
            this.label6.Text = "物品名称";
            // 
            // tbsCode
            // 
            this.tbsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.tbsCode.Location = new System.Drawing.Point(93, 23);
            this.tbsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbsCode.Name = "tbsCode";
            this.tbsCode.Size = new System.Drawing.Size(240, 23);
            this.tbsCode.TabIndex = 223;
            this.tbsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsCode_OnCompleteSearch);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(16, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 222;
            this.label4.Text = "图号型号";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 170);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 21;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1051, 708);
            this.dataGridView1.TabIndex = 244;
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            this.dataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellClick);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            // 
            // 库存物品防锈
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 878);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "库存物品防锈";
            this.Load += new System.EventHandler(this.库存物品防锈_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudAntirust)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 全部选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部取消ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消ToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkIsNormal;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem 仓管员操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查询物品防锈设置ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 执行防锈操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 质管部操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 审核库存防锈ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 质量工程师操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 确认防锈操作ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 查询库存的防锈状况ToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem 导出EXCELToolStripMenuItem;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudAntirust;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label6;
        private TextBoxShow tbsCode;
        private System.Windows.Forms.Label label4;

    }
}
