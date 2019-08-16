namespace Expression
{
    partial class 退货业务报废物品筛选窗体
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSure = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.Sel = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Provider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ResponsibilityProvider = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bill_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bill_Time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BatchNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CVTNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Quantity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WorkingHours = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remark = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GoodsID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.选择ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全选ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.取消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全消ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSure);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(725, 56);
            this.panel1.TabIndex = 0;
            // 
            // btnSure
            // 
            this.btnSure.Location = new System.Drawing.Point(638, 16);
            this.btnSure.Name = "btnSure";
            this.btnSure.Size = new System.Drawing.Size(75, 23);
            this.btnSure.TabIndex = 0;
            this.btnSure.Text = "提交";
            this.btnSure.UseVisualStyleBackColor = true;
            this.btnSure.Click += new System.EventHandler(this.btnSure_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dataGridView1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 56);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(725, 404);
            this.panel2.TabIndex = 1;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Sel,
            this.Provider,
            this.ResponsibilityProvider,
            this.Bill_ID,
            this.Bill_Time,
            this.GoodsCode,
            this.GoodsName,
            this.Spec,
            this.GoodsType,
            this.BatchNo,
            this.CVTNumber,
            this.Reason,
            this.Quantity,
            this.WorkingHours,
            this.Remark,
            this.GoodsID});
            this.dataGridView1.ContextMenuStrip = this.contextMenuStrip1;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(725, 404);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // Sel
            // 
            this.Sel.DataPropertyName = "Sel";
            this.Sel.FalseValue = ((short)(0));
            this.Sel.HeaderText = "选";
            this.Sel.IndeterminateValue = ((short)(0));
            this.Sel.Name = "Sel";
            this.Sel.ReadOnly = true;
            this.Sel.TrueValue = ((short)(1));
            this.Sel.Width = 20;
            // 
            // Provider
            // 
            this.Provider.DataPropertyName = "Provider";
            this.Provider.HeaderText = "物品供应商";
            this.Provider.Name = "Provider";
            this.Provider.ReadOnly = true;
            // 
            // ResponsibilityProvider
            // 
            this.ResponsibilityProvider.DataPropertyName = "ResponsibilityProvider";
            this.ResponsibilityProvider.HeaderText = "责任供应商";
            this.ResponsibilityProvider.Name = "ResponsibilityProvider";
            this.ResponsibilityProvider.ReadOnly = true;
            // 
            // Bill_ID
            // 
            this.Bill_ID.DataPropertyName = "Bill_ID";
            this.Bill_ID.HeaderText = "报废单号";
            this.Bill_ID.Name = "Bill_ID";
            this.Bill_ID.ReadOnly = true;
            // 
            // Bill_Time
            // 
            this.Bill_Time.DataPropertyName = "Bill_Time";
            this.Bill_Time.HeaderText = "单据时间";
            this.Bill_Time.Name = "Bill_Time";
            this.Bill_Time.ReadOnly = true;
            // 
            // GoodsCode
            // 
            this.GoodsCode.DataPropertyName = "GoodsCode";
            this.GoodsCode.HeaderText = "图号型号";
            this.GoodsCode.Name = "GoodsCode";
            this.GoodsCode.ReadOnly = true;
            // 
            // GoodsName
            // 
            this.GoodsName.DataPropertyName = "GoodsName";
            this.GoodsName.HeaderText = "物品名称";
            this.GoodsName.Name = "GoodsName";
            this.GoodsName.ReadOnly = true;
            // 
            // Spec
            // 
            this.Spec.DataPropertyName = "Spec";
            this.Spec.HeaderText = "规格";
            this.Spec.Name = "Spec";
            this.Spec.ReadOnly = true;
            // 
            // GoodsType
            // 
            this.GoodsType.DataPropertyName = "GoodsType";
            this.GoodsType.HeaderText = "物品类别";
            this.GoodsType.Name = "GoodsType";
            this.GoodsType.ReadOnly = true;
            // 
            // BatchNo
            // 
            this.BatchNo.DataPropertyName = "BatchNo";
            this.BatchNo.HeaderText = "批次号";
            this.BatchNo.Name = "BatchNo";
            this.BatchNo.ReadOnly = true;
            // 
            // CVTNumber
            // 
            this.CVTNumber.DataPropertyName = "CVTNumber";
            this.CVTNumber.HeaderText = "CVT编号";
            this.CVTNumber.Name = "CVTNumber";
            this.CVTNumber.ReadOnly = true;
            // 
            // Reason
            // 
            this.Reason.DataPropertyName = "Reason";
            this.Reason.HeaderText = "原因";
            this.Reason.Name = "Reason";
            this.Reason.ReadOnly = true;
            // 
            // Quantity
            // 
            this.Quantity.DataPropertyName = "Quantity";
            this.Quantity.HeaderText = "数量";
            this.Quantity.Name = "Quantity";
            this.Quantity.ReadOnly = true;
            // 
            // WorkingHours
            // 
            this.WorkingHours.DataPropertyName = "WorkingHours";
            this.WorkingHours.HeaderText = "工时";
            this.WorkingHours.Name = "WorkingHours";
            this.WorkingHours.ReadOnly = true;
            // 
            // Remark
            // 
            this.Remark.DataPropertyName = "Remark";
            this.Remark.HeaderText = "备注";
            this.Remark.Name = "Remark";
            this.Remark.ReadOnly = true;
            // 
            // GoodsID
            // 
            this.GoodsID.DataPropertyName = "GoodsID";
            this.GoodsID.HeaderText = "GoodsID";
            this.GoodsID.Name = "GoodsID";
            this.GoodsID.ReadOnly = true;
            this.GoodsID.Visible = false;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.选择ToolStripMenuItem,
            this.全选ToolStripMenuItem,
            this.取消ToolStripMenuItem,
            this.全消ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(119, 92);
            // 
            // 选择ToolStripMenuItem
            // 
            this.选择ToolStripMenuItem.Name = "选择ToolStripMenuItem";
            this.选择ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.选择ToolStripMenuItem.Text = "选择";
            this.选择ToolStripMenuItem.Click += new System.EventHandler(this.选择ToolStripMenuItem_Click);
            // 
            // 全选ToolStripMenuItem
            // 
            this.全选ToolStripMenuItem.Name = "全选ToolStripMenuItem";
            this.全选ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.全选ToolStripMenuItem.Text = "全部选中";
            this.全选ToolStripMenuItem.Click += new System.EventHandler(this.全选ToolStripMenuItem_Click);
            // 
            // 取消ToolStripMenuItem
            // 
            this.取消ToolStripMenuItem.Name = "取消ToolStripMenuItem";
            this.取消ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.取消ToolStripMenuItem.Text = "取消";
            this.取消ToolStripMenuItem.Click += new System.EventHandler(this.取消ToolStripMenuItem_Click);
            // 
            // 全消ToolStripMenuItem
            // 
            this.全消ToolStripMenuItem.Name = "全消ToolStripMenuItem";
            this.全消ToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.全消ToolStripMenuItem.Text = "全部取消";
            this.全消ToolStripMenuItem.Click += new System.EventHandler(this.全消ToolStripMenuItem_Click);
            // 
            // 退货业务报废物品筛选窗体
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(725, 460);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "退货业务报废物品筛选窗体";
            this.Text = "退货业务报废物品筛选窗体";
            this.Load += new System.EventHandler(this.退货业务报废物品筛选窗体_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSure;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 选择ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全选ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 取消ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全消ToolStripMenuItem;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Sel;
        private System.Windows.Forms.DataGridViewTextBoxColumn Provider;
        private System.Windows.Forms.DataGridViewTextBoxColumn ResponsibilityProvider;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bill_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bill_Time;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsType;
        private System.Windows.Forms.DataGridViewTextBoxColumn BatchNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CVTNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn Reason;
        private System.Windows.Forms.DataGridViewTextBoxColumn Quantity;
        private System.Windows.Forms.DataGridViewTextBoxColumn WorkingHours;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remark;
        private System.Windows.Forms.DataGridViewTextBoxColumn GoodsID;
    }
}