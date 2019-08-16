using UniversalControlLibrary;
namespace Expression
{
    partial class 自动生成入库单
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panelPara = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.cmbStorage = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.btnGenerates = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.txtProvider = new UniversalControlLibrary.TextBoxShow();
            this.label1 = new System.Windows.Forms.Label();
            this.txtName = new UniversalControlLibrary.TextBoxShow();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.选 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.订单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订货数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.已到货数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.到货数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订货人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.要求到货日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.入库数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.让步数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.退货数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.检验破坏数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1051, 49);
            this.panel1.TabIndex = 52;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(413, 10);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(201, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "自动生成入库单";
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 49);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1051, 139);
            this.panelPara.TabIndex = 53;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtVersion);
            this.groupBox1.Controls.Add(this.cmbStorage);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.btnGenerates);
            this.groupBox1.Controls.Add(this.btnFind);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.txtSpec);
            this.groupBox1.Controls.Add(this.txtProvider);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtName);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.txtCode);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1051, 106);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "信息设置";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(601, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 220;
            this.label2.Text = "版次号";
            // 
            // txtVersion
            // 
            this.txtVersion.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtVersion.Location = new System.Drawing.Point(682, 25);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(110, 23);
            this.txtVersion.TabIndex = 221;
            // 
            // cmbStorage
            // 
            this.cmbStorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbStorage.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbStorage.FormattingEnabled = true;
            this.cmbStorage.Location = new System.Drawing.Point(440, 25);
            this.cmbStorage.Name = "cmbStorage";
            this.cmbStorage.Size = new System.Drawing.Size(138, 22);
            this.cmbStorage.TabIndex = 219;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label13.Location = new System.Drawing.Point(358, 29);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(63, 14);
            this.label13.TabIndex = 218;
            this.label13.Text = "所属库房";
            // 
            // btnGenerates
            // 
            this.btnGenerates.Location = new System.Drawing.Point(923, 24);
            this.btnGenerates.Name = "btnGenerates";
            this.btnGenerates.Size = new System.Drawing.Size(122, 25);
            this.btnGenerates.TabIndex = 217;
            this.btnGenerates.Tag = "view";
            this.btnGenerates.Text = "自动生成";
            this.btnGenerates.UseVisualStyleBackColor = true;
            this.btnGenerates.Click += new System.EventHandler(this.btnGenerates_Click);
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(812, 24);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(87, 25);
            this.btnFind.TabIndex = 216;
            this.btnFind.Tag = "view";
            this.btnFind.Text = "查询";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(731, 70);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 214;
            this.label10.Text = "规    格";
            // 
            // txtSpec
            // 
            this.txtSpec.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtSpec.Location = new System.Drawing.Point(812, 66);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(233, 23);
            this.txtSpec.TabIndex = 215;
            // 
            // txtProvider
            // 
            this.txtProvider.DataResult = null;
            this.txtProvider.DataTableResult = null;
            this.txtProvider.EditingControlDataGridView = null;
            this.txtProvider.EditingControlFormattedValue = "";
            this.txtProvider.EditingControlRowIndex = 0;
            this.txtProvider.EditingControlValueChanged = false;
            this.txtProvider.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.txtProvider.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtProvider.IsMultiSelect = false;
            this.txtProvider.Location = new System.Drawing.Point(97, 25);
            this.txtProvider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ShowResultForm = true;
            this.txtProvider.Size = new System.Drawing.Size(244, 23);
            this.txtProvider.StrEndSql = null;
            this.txtProvider.TabIndex = 212;
            this.txtProvider.TabStop = false;
            this.txtProvider.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProvider_OnCompleteSearch);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(16, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 213;
            this.label1.Text = "供 应 商";
            // 
            // txtName
            // 
            this.txtName.DataResult = null;
            this.txtName.DataTableResult = null;
            this.txtName.EditingControlDataGridView = null;
            this.txtName.EditingControlFormattedValue = "";
            this.txtName.EditingControlRowIndex = 0;
            this.txtName.EditingControlValueChanged = false;
            this.txtName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtName.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtName.IsMultiSelect = false;
            this.txtName.Location = new System.Drawing.Point(97, 66);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtName.Name = "txtName";
            this.txtName.ShowResultForm = true;
            this.txtName.Size = new System.Drawing.Size(244, 23);
            this.txtName.StrEndSql = null;
            this.txtName.TabIndex = 208;
            this.txtName.TabStop = false;
            this.txtName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtName_OnCompleteSearch);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(358, 70);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(63, 14);
            this.label11.TabIndex = 209;
            this.label11.Text = "图号型号";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(16, 70);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(63, 14);
            this.label12.TabIndex = 210;
            this.label12.Text = "物品名称";
            // 
            // txtCode
            // 
            this.txtCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtCode.Location = new System.Drawing.Point(440, 66);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(250, 23);
            this.txtCode.TabIndex = 211;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选,
            this.订单号,
            this.供应商,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.订货数量,
            this.已到货数,
            this.到货数量,
            this.订货人,
            this.要求到货日期,
            this.入库数,
            this.让步数,
            this.退货数,
            this.检验破坏数});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 21;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1051, 558);
            this.dataGridView1.TabIndex = 54;
            this.dataGridView1.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseUp);
            this.dataGridView1.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEndEdit);
            // 
            // 选
            // 
            this.选.DataPropertyName = "选";
            this.选.HeaderText = "选";
            this.选.Name = "选";
            this.选.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.选.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.选.Width = 20;
            // 
            // 订单号
            // 
            this.订单号.DataPropertyName = "订单号";
            this.订单号.HeaderText = "订单号";
            this.订单号.Name = "订单号";
            this.订单号.ReadOnly = true;
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.ReadOnly = true;
            this.供应商.Width = 65;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "物品名称";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 订货数量
            // 
            this.订货数量.DataPropertyName = "订货数量";
            this.订货数量.HeaderText = "订货数量";
            this.订货数量.Name = "订货数量";
            this.订货数量.ReadOnly = true;
            // 
            // 已到货数
            // 
            this.已到货数.DataPropertyName = "已到货数";
            this.已到货数.HeaderText = "已到货数";
            this.已到货数.Name = "已到货数";
            this.已到货数.ReadOnly = true;
            // 
            // 到货数量
            // 
            this.到货数量.DataPropertyName = "到货数量";
            this.到货数量.HeaderText = "到货数量";
            this.到货数量.Name = "到货数量";
            // 
            // 订货人
            // 
            this.订货人.DataPropertyName = "订货人";
            this.订货人.HeaderText = "订货人";
            this.订货人.Name = "订货人";
            this.订货人.ReadOnly = true;
            // 
            // 要求到货日期
            // 
            this.要求到货日期.DataPropertyName = "要求到货日期";
            this.要求到货日期.HeaderText = "要求到货日期";
            this.要求到货日期.Name = "要求到货日期";
            this.要求到货日期.ReadOnly = true;
            // 
            // 入库数
            // 
            this.入库数.DataPropertyName = "入库数";
            this.入库数.HeaderText = "入库数";
            this.入库数.Name = "入库数";
            this.入库数.ReadOnly = true;
            // 
            // 让步数
            // 
            this.让步数.DataPropertyName = "让步数";
            this.让步数.HeaderText = "让步数";
            this.让步数.Name = "让步数";
            this.让步数.ReadOnly = true;
            // 
            // 退货数
            // 
            this.退货数.DataPropertyName = "退货数";
            this.退货数.HeaderText = "退货数";
            this.退货数.Name = "退货数";
            this.退货数.ReadOnly = true;
            // 
            // 检验破坏数
            // 
            this.检验破坏数.DataPropertyName = "检验破坏数";
            this.检验破坏数.HeaderText = "检验破坏数";
            this.检验破坏数.Name = "检验破坏数";
            this.检验破坏数.ReadOnly = true;
            // 
            // 自动生成入库单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 746);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "自动生成入库单";
            this.Load += new System.EventHandler(this.自动生成入库单_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private TextBoxShow txtProvider;
        private System.Windows.Forms.Label label1;
        private TextBoxShow txtName;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Button btnGenerates;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订货数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 已到货数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 到货数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订货人;
        private System.Windows.Forms.DataGridViewTextBoxColumn 要求到货日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 入库数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 让步数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 退货数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 检验破坏数;
        private System.Windows.Forms.ComboBox cmbStorage;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtVersion;
    }
}
