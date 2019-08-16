namespace Form_Manufacture_WorkShop
{
    partial class 车间批次管理变更明细
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.customGroupBox1 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.txtReason = new UniversalControlLibrary.TextBoxShow();
            this.lbBillStatus = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.txtBillNo = new System.Windows.Forms.TextBox();
            this.label68 = new System.Windows.Forms.Label();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.customGroupBox2 = new UniversalControlLibrary.CustomGroupBox(this.components);
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.单据号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new UniversalControlLibrary.DataGridViewTextBoxShowColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.管理方式 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.customContextMenuStrip_Edit1 = new UniversalControlLibrary.CustomContextMenuStrip_Edit(this.components);
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.customGroupBox1.SuspendLayout();
            this.customGroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customGroupBox1
            // 
            this.customGroupBox1.Controls.Add(this.label1);
            this.customGroupBox1.Controls.Add(this.txtReason);
            this.customGroupBox1.Controls.Add(this.lbBillStatus);
            this.customGroupBox1.Controls.Add(this.label69);
            this.customGroupBox1.Controls.Add(this.txtBillNo);
            this.customGroupBox1.Controls.Add(this.label68);
            this.customGroupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.customGroupBox1.Location = new System.Drawing.Point(0, 0);
            this.customGroupBox1.Name = "customGroupBox1";
            this.customGroupBox1.Size = new System.Drawing.Size(873, 163);
            this.customGroupBox1.TabIndex = 0;
            this.customGroupBox1.TabStop = false;
            this.customGroupBox1.Text = "主要信息";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 98);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 415;
            this.label1.Text = "变更原因";
            // 
            // txtReason
            // 
            this.txtReason.DataResult = null;
            this.txtReason.DataTableResult = null;
            this.txtReason.EditingControlDataGridView = null;
            this.txtReason.EditingControlFormattedValue = "";
            this.txtReason.EditingControlRowIndex = 0;
            this.txtReason.EditingControlValueChanged = true;
            this.txtReason.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.txtReason.IsMultiSelect = false;
            this.txtReason.Location = new System.Drawing.Point(94, 72);
            this.txtReason.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtReason.Multiline = true;
            this.txtReason.Name = "txtReason";
            this.txtReason.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtReason.ShowResultForm = false;
            this.txtReason.Size = new System.Drawing.Size(741, 68);
            this.txtReason.StrEndSql = null;
            this.txtReason.TabIndex = 414;
            this.txtReason.TabStop = false;
            // 
            // lbBillStatus
            // 
            this.lbBillStatus.AutoSize = true;
            this.lbBillStatus.ForeColor = System.Drawing.Color.Red;
            this.lbBillStatus.Location = new System.Drawing.Point(430, 35);
            this.lbBillStatus.Name = "lbBillStatus";
            this.lbBillStatus.Size = new System.Drawing.Size(53, 12);
            this.lbBillStatus.TabIndex = 413;
            this.lbBillStatus.Text = "新建单据";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(339, 35);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(53, 12);
            this.label69.TabIndex = 412;
            this.label69.Text = "单据状态";
            // 
            // txtBillNo
            // 
            this.txtBillNo.Location = new System.Drawing.Point(94, 31);
            this.txtBillNo.Name = "txtBillNo";
            this.txtBillNo.Size = new System.Drawing.Size(206, 21);
            this.txtBillNo.TabIndex = 411;
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(26, 35);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(53, 12);
            this.label68.TabIndex = 410;
            this.label68.Text = "单 据 号";
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 163);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = true;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(873, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 1;
            // 
            // customGroupBox2
            // 
            this.customGroupBox2.Controls.Add(this.customDataGridView1);
            this.customGroupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customGroupBox2.Location = new System.Drawing.Point(0, 195);
            this.customGroupBox2.Name = "customGroupBox2";
            this.customGroupBox2.Size = new System.Drawing.Size(873, 435);
            this.customGroupBox2.TabIndex = 2;
            this.customGroupBox2.TabStop = false;
            this.customGroupBox2.Text = "明细信息";
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.单据号,
            this.物品ID,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.管理方式});
            this.customDataGridView1.ContextMenuStrip = this.customContextMenuStrip_Edit1;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(3, 17);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(867, 415);
            this.customDataGridView1.TabIndex = 1;
            // 
            // 单据号
            // 
            this.单据号.DataPropertyName = "单据号";
            this.单据号.HeaderText = "单据号";
            this.单据号.Name = "单据号";
            this.单据号.Visible = false;
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.物品ID.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.物品ID.Visible = false;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.DataResult = null;
            this.图号型号.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "物品名称";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            this.物品名称.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.物品名称.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            this.规格.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.规格.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // 管理方式
            // 
            this.管理方式.DataPropertyName = "管理方式";
            this.管理方式.HeaderText = "管理方式";
            this.管理方式.Name = "管理方式";
            // 
            // customContextMenuStrip_Edit1
            // 
            this.customContextMenuStrip_Edit1.Name = "customContextMenuStrip_Edit1";
            this.customContextMenuStrip_Edit1.Size = new System.Drawing.Size(153, 92);
            this.customContextMenuStrip_Edit1._InputEvent += new GlobalObject.DelegateCollection.DataTableHandle(this.customContextMenuStrip_Edit1__InputEvent);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // 车间批次管理变更明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(873, 630);
            this.Controls.Add(this.customGroupBox2);
            this.Controls.Add(this.userControlDataLocalizer1);
            this.Controls.Add(this.customGroupBox1);
            this.Name = "车间批次管理变更明细";
            this.Text = "车间批次管理变更明细";
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.车间批次管理变更明细_PanelGetDataInfo);
            this.customGroupBox1.ResumeLayout(false);
            this.customGroupBox1.PerformLayout();
            this.customGroupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomGroupBox customGroupBox1;
        private UniversalControlLibrary.UserControlDataLocalizer userControlDataLocalizer1;
        private UniversalControlLibrary.CustomGroupBox customGroupBox2;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.Label lbBillStatus;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.TextBox txtBillNo;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.TextBoxShow txtReason;
        private System.Windows.Forms.DataGridViewTextBoxColumn 单据号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private UniversalControlLibrary.DataGridViewTextBoxShowColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewComboBoxColumn 管理方式;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private UniversalControlLibrary.CustomContextMenuStrip_Edit customContextMenuStrip_Edit1;
    }
}