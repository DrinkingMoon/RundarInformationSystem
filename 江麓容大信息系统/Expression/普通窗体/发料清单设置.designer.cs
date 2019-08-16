using UniversalControlLibrary;
namespace Expression
{
    partial class 发料清单设置
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

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel1 = new System.Windows.Forms.Panel();
            this.cmbApplicable = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnModify = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.txtGoodsName = new UniversalControlLibrary.TextBoxShow();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtByname = new System.Windows.Forms.TextBox();
            this.tbsEditionName = new UniversalControlLibrary.TextBoxShow();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCount = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Dgv_Main = new System.Windows.Forms.DataGridView();
            this.零件编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.零件名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.基数 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.产品编码 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.顺序位置 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_Main)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cmbApplicable);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.btnImport);
            this.panel1.Controls.Add(this.btnModify);
            this.panel1.Controls.Add(this.btnExport);
            this.panel1.Controls.Add(this.txtGoodsName);
            this.panel1.Controls.Add(this.txtGoodsCode);
            this.panel1.Controls.Add(this.btnSave);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnAdd);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.txtByname);
            this.panel1.Controls.Add(this.tbsEditionName);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.txtCount);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtSpec);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(542, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(186, 668);
            this.panel1.TabIndex = 1;
            // 
            // cmbApplicable
            // 
            this.cmbApplicable.FormattingEnabled = true;
            this.cmbApplicable.Location = new System.Drawing.Point(76, 154);
            this.cmbApplicable.Name = "cmbApplicable";
            this.cmbApplicable.Size = new System.Drawing.Size(98, 20);
            this.cmbApplicable.TabIndex = 314;
            this.cmbApplicable.SelectedIndexChanged += new System.EventHandler(this.cmbApplicable_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(6, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(63, 14);
            this.label7.TabIndex = 313;
            this.label7.Text = "适用范围";
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(99, 630);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 26);
            this.btnImport.TabIndex = 39;
            this.btnImport.Text = "导入";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Visible = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnModify
            // 
            this.btnModify.Location = new System.Drawing.Point(99, 584);
            this.btnModify.Name = "btnModify";
            this.btnModify.Size = new System.Drawing.Size(75, 26);
            this.btnModify.TabIndex = 38;
            this.btnModify.Text = "修改";
            this.btnModify.UseVisualStyleBackColor = true;
            this.btnModify.Visible = false;
            this.btnModify.Click += new System.EventHandler(this.btnModify_Click);
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(99, 237);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 26);
            this.btnExport.TabIndex = 37;
            this.btnExport.Text = "导出";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.DataResult = null;
            this.txtGoodsName.DataTableResult = null;
            this.txtGoodsName.EditingControlDataGridView = null;
            this.txtGoodsName.EditingControlFormattedValue = "";
            this.txtGoodsName.EditingControlRowIndex = 0;
            this.txtGoodsName.EditingControlValueChanged = false;
            this.txtGoodsName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsName.IsMultiSelect = false;
            this.txtGoodsName.Location = new System.Drawing.Point(81, 402);
            this.txtGoodsName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.ShowResultForm = true;
            this.txtGoodsName.Size = new System.Drawing.Size(98, 21);
            this.txtGoodsName.StrEndSql = null;
            this.txtGoodsName.TabIndex = 36;
            this.txtGoodsName.TabStop = false;
            this.txtGoodsName.Tag = "";
            this.txtGoodsName.Visible = false;
            this.txtGoodsName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsName_OnCompleteSearch);
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.Location = new System.Drawing.Point(81, 440);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.Size = new System.Drawing.Size(98, 21);
            this.txtGoodsCode.TabIndex = 35;
            this.txtGoodsCode.Visible = false;
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(5, 237);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 26);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "保存";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(5, 630);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(75, 26);
            this.btnDelete.TabIndex = 32;
            this.btnDelete.Text = "删除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Visible = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(5, 584);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(75, 26);
            this.btnAdd.TabIndex = 31;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Visible = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(12, 515);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 30;
            this.label6.Text = "别    名";
            this.label6.Visible = false;
            // 
            // txtByname
            // 
            this.txtByname.Location = new System.Drawing.Point(81, 512);
            this.txtByname.Name = "txtByname";
            this.txtByname.Size = new System.Drawing.Size(98, 21);
            this.txtByname.TabIndex = 29;
            this.txtByname.Visible = false;
            // 
            // tbsEditionName
            // 
            this.tbsEditionName.DataResult = null;
            this.tbsEditionName.DataTableResult = null;
            this.tbsEditionName.EditingControlDataGridView = null;
            this.tbsEditionName.EditingControlFormattedValue = "";
            this.tbsEditionName.EditingControlRowIndex = 0;
            this.tbsEditionName.EditingControlValueChanged = false;
            this.tbsEditionName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.tbsEditionName.IsMultiSelect = false;
            this.tbsEditionName.Location = new System.Drawing.Point(76, 199);
            this.tbsEditionName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbsEditionName.Name = "tbsEditionName";
            this.tbsEditionName.ShowResultForm = true;
            this.tbsEditionName.Size = new System.Drawing.Size(98, 21);
            this.tbsEditionName.StrEndSql = null;
            this.tbsEditionName.TabIndex = 0;
            this.tbsEditionName.TabStop = false;
            this.tbsEditionName.Tag = "";
            this.tbsEditionName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbsEditionName_OnCompleteSearch);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(12, 552);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 26;
            this.label5.Text = "基    数";
            this.label5.Visible = false;
            // 
            // txtCount
            // 
            this.txtCount.Location = new System.Drawing.Point(81, 549);
            this.txtCount.Name = "txtCount";
            this.txtCount.Size = new System.Drawing.Size(98, 21);
            this.txtCount.TabIndex = 25;
            this.txtCount.Visible = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 479);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 24;
            this.label4.Text = "规    格";
            this.label4.Visible = false;
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(81, 476);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.ReadOnly = true;
            this.txtSpec.Size = new System.Drawing.Size(98, 21);
            this.txtSpec.TabIndex = 23;
            this.txtSpec.Visible = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(12, 443);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 22;
            this.label3.Text = "图号型号";
            this.label3.Visible = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(12, 405);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 20;
            this.label2.Text = "物品名称";
            this.label2.Visible = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(7, 202);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 19;
            this.label1.Text = "总成名称";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.Dgv_Main);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(542, 668);
            this.panel2.TabIndex = 2;
            // 
            // Dgv_Main
            // 
            this.Dgv_Main.AllowDrop = true;
            this.Dgv_Main.AllowUserToAddRows = false;
            this.Dgv_Main.AllowUserToDeleteRows = false;
            this.Dgv_Main.AllowUserToResizeRows = false;
            this.Dgv_Main.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.Dgv_Main.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.零件编码,
            this.零件名称,
            this.规格,
            this.基数,
            this.产品编码,
            this.物品ID,
            this.顺序位置});
            this.Dgv_Main.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Dgv_Main.Location = new System.Drawing.Point(0, 0);
            this.Dgv_Main.MultiSelect = false;
            this.Dgv_Main.Name = "Dgv_Main";
            this.Dgv_Main.RowHeadersWidth = 18;
            this.Dgv_Main.RowTemplate.Height = 23;
            this.Dgv_Main.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.Dgv_Main.Size = new System.Drawing.Size(542, 668);
            this.Dgv_Main.TabIndex = 1;
            this.Dgv_Main.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseDown);
            this.Dgv_Main.MouseMove += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseMove);
            this.Dgv_Main.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView_RowPostPaint);
            this.Dgv_Main.DragOver += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragOver);
            this.Dgv_Main.MouseUp += new System.Windows.Forms.MouseEventHandler(this.dataGridView_MouseUp);
            this.Dgv_Main.DragDrop += new System.Windows.Forms.DragEventHandler(this.dataGridView_DragDrop);
            // 
            // 零件编码
            // 
            this.零件编码.DataPropertyName = "零件编码";
            this.零件编码.HeaderText = "零件编码";
            this.零件编码.Name = "零件编码";
            this.零件编码.ReadOnly = true;
            this.零件编码.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.零件编码.Width = 150;
            // 
            // 零件名称
            // 
            this.零件名称.DataPropertyName = "零件名称";
            this.零件名称.HeaderText = "零件名称";
            this.零件名称.Name = "零件名称";
            this.零件名称.ReadOnly = true;
            this.零件名称.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.零件名称.Width = 150;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            this.规格.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.规格.Width = 150;
            // 
            // 基数
            // 
            this.基数.DataPropertyName = "基数";
            this.基数.HeaderText = "基数";
            this.基数.Name = "基数";
            this.基数.ReadOnly = true;
            this.基数.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.基数.Width = 60;
            // 
            // 产品编码
            // 
            this.产品编码.DataPropertyName = "产品编码";
            this.产品编码.HeaderText = "产品编码";
            this.产品编码.Name = "产品编码";
            this.产品编码.ReadOnly = true;
            this.产品编码.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.产品编码.Visible = false;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            this.物品ID.Width = 60;
            // 
            // 顺序位置
            // 
            this.顺序位置.DataPropertyName = "顺序位置";
            this.顺序位置.HeaderText = "顺序位置";
            this.顺序位置.Name = "顺序位置";
            this.顺序位置.ReadOnly = true;
            this.顺序位置.Visible = false;
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
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "零件编码";
            this.dataGridViewTextBoxColumn1.HeaderText = "零件编码";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn1.Width = 150;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "零件名称";
            this.dataGridViewTextBoxColumn2.HeaderText = "零件名称";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn2.Width = 150;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.DataPropertyName = "规格";
            this.dataGridViewTextBoxColumn3.HeaderText = "规格";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "基数";
            this.dataGridViewTextBoxColumn4.HeaderText = "基数";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn4.Width = 60;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "产品编码";
            this.dataGridViewTextBoxColumn5.HeaderText = "产品编码";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.dataGridViewTextBoxColumn5.Visible = false;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "物品ID";
            this.dataGridViewTextBoxColumn6.HeaderText = "物品ID";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Visible = false;
            this.dataGridViewTextBoxColumn6.Width = 60;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "顺序位置";
            this.dataGridViewTextBoxColumn7.HeaderText = "顺序位置";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Visible = false;
            // 
            // 发料清单设置
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(728, 668);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "发料清单设置";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "发料清单设置";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.Dgv_Main)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtByname;
        private TextBoxShow tbsEditionName;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtCount;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView Dgv_Main;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private TextBoxShow txtGoodsName;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnModify;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 零件名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 基数;
        private System.Windows.Forms.DataGridViewTextBoxColumn 产品编码;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn 顺序位置;
        private System.Windows.Forms.ComboBox cmbApplicable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
    }
}

