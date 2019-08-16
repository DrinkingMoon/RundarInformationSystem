namespace Form_Peripheral_CompanyQuality
{
    partial class 重点工作关键节点
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
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.重点工作关键节点1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.状态 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.启动时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.完成时间 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.责任人 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.F_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.AutoCreateFilters = true;
            this.customDataGridView1.BaseFilter = "";
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.重点工作关键节点1,
            this.状态,
            this.启动时间,
            this.完成时间,
            this.责任人,
            this.F_Id});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.customDataGridView1.DefaultCellStyle = dataGridViewCellStyle1;
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 0);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.customDataGridView1.Size = new System.Drawing.Size(566, 399);
            this.customDataGridView1.TabIndex = 1;
            this.customDataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.customDataGridView1_DataBindingComplete);
            // 
            // 重点工作关键节点1
            // 
            this.重点工作关键节点1.DataPropertyName = "重点工作关键节点";
            this.重点工作关键节点1.HeaderText = "重点工作关键节点";
            this.重点工作关键节点1.Name = "重点工作关键节点1";
            this.重点工作关键节点1.Width = 150;
            // 
            // 状态
            // 
            this.状态.DataPropertyName = "状态";
            this.状态.HeaderText = "状态";
            this.状态.Items.AddRange(new object[] {
            "已完成",
            "未完成",
            "延期",
            "进行中",
            "待启动"});
            this.状态.Name = "状态";
            this.状态.Width = 80;
            // 
            // 启动时间
            // 
            this.启动时间.DataPropertyName = "启动时间";
            this.启动时间.HeaderText = "启动时间";
            this.启动时间.Name = "启动时间";
            // 
            // 完成时间
            // 
            this.完成时间.DataPropertyName = "完成时间";
            this.完成时间.HeaderText = "完成时间";
            this.完成时间.Name = "完成时间";
            // 
            // 责任人
            // 
            this.责任人.DataPropertyName = "责任人";
            this.责任人.HeaderText = "责任人";
            this.责任人.Name = "责任人";
            this.责任人.Width = 80;
            // 
            // F_Id
            // 
            this.F_Id.DataPropertyName = "F_Id";
            this.F_Id.HeaderText = "F_Id";
            this.F_Id.Name = "F_Id";
            this.F_Id.Visible = false;
            // 
            // 重点工作关键节点
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(566, 399);
            this.Controls.Add(this.customDataGridView1);
            this.Name = "重点工作关键节点";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "重点工作关键节点";
            this.Load += new System.EventHandler(this.重点工作关键节点_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.重点工作关键节点_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 重点工作关键节点1;
        private System.Windows.Forms.DataGridViewComboBoxColumn 状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn 启动时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 完成时间;
        private System.Windows.Forms.DataGridViewTextBoxColumn 责任人;
        private System.Windows.Forms.DataGridViewTextBoxColumn F_Id;

    }
}