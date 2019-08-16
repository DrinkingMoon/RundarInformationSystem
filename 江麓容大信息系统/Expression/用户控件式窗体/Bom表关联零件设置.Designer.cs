using UniversalControlLibrary;
namespace Expression
{
    partial class Bom表关联零件设置
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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.chbIsMath = new System.Windows.Forms.CheckBox();
            this.label8 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.btnUpdate = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.chbIsStock = new System.Windows.Forms.CheckBox();
            this.chbIsJumbly = new System.Windows.Forms.CheckBox();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtName = new UniversalControlLibrary.TextBoxShow();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtBomSpec = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtBomGoodsCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtBomGoodsName = new UniversalControlLibrary.TextBoxShow();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.panel1.SuspendLayout();
            this.panelPara.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel1.Controls.Add(this.labelTitle);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1051, 61);
            this.panel1.TabIndex = 57;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(388, 16);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(243, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "Bom表关联零件设置";
            // 
            // panelPara
            // 
            this.panelPara.Controls.Add(this.groupBox1);
            this.panelPara.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelPara.Location = new System.Drawing.Point(0, 61);
            this.panelPara.Name = "panelPara";
            this.panelPara.Size = new System.Drawing.Size(1051, 260);
            this.panelPara.TabIndex = 62;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1051, 219);
            this.groupBox1.TabIndex = 60;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置信息";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtRemark);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.chbIsMath);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.numericUpDown1);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.btnUpdate);
            this.groupBox3.Controls.Add(this.btnDelete);
            this.groupBox3.Controls.Add(this.btnAdd);
            this.groupBox3.Controls.Add(this.chbIsStock);
            this.groupBox3.Controls.Add(this.chbIsJumbly);
            this.groupBox3.Controls.Add(this.txtSpec);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.txtCode);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.txtName);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 79);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(1045, 137);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "零件信息设置";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(117, 99);
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(888, 23);
            this.txtRemark.TabIndex = 39;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(37, 104);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(63, 14);
            this.label9.TabIndex = 38;
            this.label9.Text = "备    注";
            // 
            // chbIsMath
            // 
            this.chbIsMath.AutoSize = true;
            this.chbIsMath.Location = new System.Drawing.Point(585, 62);
            this.chbIsMath.Name = "chbIsMath";
            this.chbIsMath.Size = new System.Drawing.Size(82, 18);
            this.chbIsMath.TabIndex = 37;
            this.chbIsMath.Text = "库存合计";
            this.chbIsMath.UseVisualStyleBackColor = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(223, 64);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 14);
            this.label8.TabIndex = 36;
            this.label8.Text = "%";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Location = new System.Drawing.Point(117, 60);
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(99, 23);
            this.numericUpDown1.TabIndex = 35;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(37, 64);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 34;
            this.label7.Text = "配    额";
            // 
            // btnUpdate
            // 
            this.btnUpdate.Location = new System.Drawing.Point(810, 59);
            this.btnUpdate.Name = "btnUpdate";
            this.btnUpdate.Size = new System.Drawing.Size(87, 25);
            this.btnUpdate.TabIndex = 33;
            this.btnUpdate.Text = "修改";
            this.btnUpdate.UseVisualStyleBackColor = true;
            this.btnUpdate.Click += new System.EventHandler(this.btnUpdate_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(918, 59);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(87, 25);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(701, 59);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(87, 25);
            this.btnAdd.TabIndex = 31;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // chbIsStock
            // 
            this.chbIsStock.AutoSize = true;
            this.chbIsStock.Location = new System.Drawing.Point(489, 62);
            this.chbIsStock.Name = "chbIsStock";
            this.chbIsStock.Size = new System.Drawing.Size(54, 18);
            this.chbIsStock.TabIndex = 30;
            this.chbIsStock.Text = "采购";
            this.chbIsStock.UseVisualStyleBackColor = true;
            // 
            // chbIsJumbly
            // 
            this.chbIsJumbly.AutoSize = true;
            this.chbIsJumbly.Location = new System.Drawing.Point(365, 62);
            this.chbIsJumbly.Name = "chbIsJumbly";
            this.chbIsJumbly.Size = new System.Drawing.Size(82, 18);
            this.chbIsJumbly.TabIndex = 29;
            this.chbIsJumbly.Text = "混合装配";
            this.chbIsJumbly.UseVisualStyleBackColor = true;
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(779, 21);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(226, 23);
            this.txtSpec.TabIndex = 28;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(699, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 27;
            this.label2.Text = "规    格";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(442, 21);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(226, 23);
            this.txtCode.TabIndex = 26;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(362, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 25;
            this.label3.Text = "图号型号";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 24;
            this.label4.Text = "物品名称";
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
            this.txtName.IsMultiSelect = false;
            this.txtName.Location = new System.Drawing.Point(117, 22);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtName.Name = "txtName";
            this.txtName.ShowResultForm = true;
            this.txtName.Size = new System.Drawing.Size(192, 23);
            this.txtName.StrEndSql = null;
            this.txtName.TabIndex = 23;
            this.txtName.TabStop = false;
            this.txtName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtName_OnCompleteSearch);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtBomSpec);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.txtBomGoodsCode);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtBomGoodsName);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox2.Location = new System.Drawing.Point(3, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1045, 60);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bom表信息";
            // 
            // txtBomSpec
            // 
            this.txtBomSpec.Location = new System.Drawing.Point(779, 22);
            this.txtBomSpec.Name = "txtBomSpec";
            this.txtBomSpec.ReadOnly = true;
            this.txtBomSpec.Size = new System.Drawing.Size(226, 23);
            this.txtBomSpec.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(699, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 21;
            this.label6.Text = "规    格";
            // 
            // txtBomGoodsCode
            // 
            this.txtBomGoodsCode.Location = new System.Drawing.Point(442, 21);
            this.txtBomGoodsCode.Name = "txtBomGoodsCode";
            this.txtBomGoodsCode.ReadOnly = true;
            this.txtBomGoodsCode.Size = new System.Drawing.Size(226, 23);
            this.txtBomGoodsCode.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(362, 26);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 19;
            this.label5.Text = "图号型号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 18;
            this.label1.Text = "物品名称";
            // 
            // txtBomGoodsName
            // 
            this.txtBomGoodsName.DataResult = null;
            this.txtBomGoodsName.DataTableResult = null;
            this.txtBomGoodsName.EditingControlDataGridView = null;
            this.txtBomGoodsName.EditingControlFormattedValue = "";
            this.txtBomGoodsName.EditingControlRowIndex = 0;
            this.txtBomGoodsName.EditingControlValueChanged = false;
            this.txtBomGoodsName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.BOM表零件;
            this.txtBomGoodsName.IsMultiSelect = false;
            this.txtBomGoodsName.Location = new System.Drawing.Point(117, 22);
            this.txtBomGoodsName.Margin = new System.Windows.Forms.Padding(3, 6, 3, 6);
            this.txtBomGoodsName.Name = "txtBomGoodsName";
            this.txtBomGoodsName.ShowResultForm = true;
            this.txtBomGoodsName.Size = new System.Drawing.Size(192, 23);
            this.txtBomGoodsName.StrEndSql = null;
            this.txtBomGoodsName.TabIndex = 17;
            this.txtBomGoodsName.TabStop = false;
            this.txtBomGoodsName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtBomGoodsName_OnCompleteSearch);
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 321);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersWidth = 18;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1051, 425);
            this.dataGridView1.TabIndex = 63;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // Bom表关联零件设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 746);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panelPara);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "Bom表关联零件设置";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panelPara.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panelPara;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button btnUpdate;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.CheckBox chbIsStock;
        private System.Windows.Forms.CheckBox chbIsJumbly;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private TextBoxShow txtName;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtBomSpec;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtBomGoodsCode;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private TextBoxShow txtBomGoodsName;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox chbIsMath;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label9;
    }
}
