using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class FormSelectPersonnel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSelectPersonnel));
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.btnSelectField = new System.Windows.Forms.ToolStripButton();
            this.btnSelectAll = new System.Windows.Forms.ToolStripButton();
            this.btnClearAll = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClose = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnShowSelectedData = new System.Windows.Forms.ToolStripButton();
            this.btnShowUnselectedData = new System.Windows.Forms.ToolStripButton();
            this.panelTop = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.lblSumCount = new System.Windows.Forms.Label();
            this.customContextMenuStrip_Select1 = new UniversalControlLibrary.CustomContextMenuStrip_Select(this.components);
            this.toolStrip.SuspendLayout();
            this.panelTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectField,
            this.btnSelectAll,
            this.btnClearAll,
            this.toolStripSeparator1,
            this.btnClose,
            this.toolStripSeparator2,
            this.btnShowSelectedData,
            this.btnShowUnselectedData});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(744, 25);
            this.toolStrip.TabIndex = 13;
            this.toolStrip.Text = "toolStrip1";
            // 
            // btnSelectField
            // 
            this.btnSelectField.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectField.Name = "btnSelectField";
            this.btnSelectField.Size = new System.Drawing.Size(36, 22);
            this.btnSelectField.Text = "选择";
            this.btnSelectField.Click += new System.EventHandler(this.btnSelectField_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(60, 22);
            this.btnSelectAll.Text = "选择所有";
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // btnClearAll
            // 
            this.btnClearAll.Image = ((System.Drawing.Image)(resources.GetObject("btnClearAll.Image")));
            this.btnClearAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClearAll.Name = "btnClearAll";
            this.btnClearAll.Size = new System.Drawing.Size(100, 22);
            this.btnClearAll.Text = "清除所有选择";
            this.btnClearAll.Click += new System.EventHandler(this.btnClearAll_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClose
            // 
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(52, 22);
            this.btnClose.Text = "退出";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // btnShowSelectedData
            // 
            this.btnShowSelectedData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowSelectedData.Name = "btnShowSelectedData";
            this.btnShowSelectedData.Size = new System.Drawing.Size(96, 22);
            this.btnShowSelectedData.Text = "仅显示选中信息";
            this.btnShowSelectedData.Click += new System.EventHandler(this.btnShowSelectedData_Click);
            // 
            // btnShowUnselectedData
            // 
            this.btnShowUnselectedData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnShowUnselectedData.Name = "btnShowUnselectedData";
            this.btnShowUnselectedData.Size = new System.Drawing.Size(108, 22);
            this.btnShowUnselectedData.Text = "仅显示未选中信息";
            this.btnShowUnselectedData.Click += new System.EventHandler(this.btnShowUnselectedData_Click);
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.userControlDataLocalizer1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(0, 25);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(744, 30);
            this.panelTop.TabIndex = 14;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, -2);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(744, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 0;
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.ContextMenuStrip = this.customContextMenuStrip_Select1;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.dataGridView.Location = new System.Drawing.Point(0, 83);
            this.dataGridView.Margin = new System.Windows.Forms.Padding(3, 1, 3, 1);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.RowHeadersVisible = false;
            this.dataGridView.RowTemplate.Height = 23;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView.Size = new System.Drawing.Size(744, 536);
            this.dataGridView.TabIndex = 13;
            this.dataGridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.dataGridView_CurrentCellDirtyStateChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 61);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 15;
            this.label1.Text = "总行数:";
            // 
            // lblSumCount
            // 
            this.lblSumCount.AutoSize = true;
            this.lblSumCount.ForeColor = System.Drawing.Color.Red;
            this.lblSumCount.Location = new System.Drawing.Point(62, 61);
            this.lblSumCount.Name = "lblSumCount";
            this.lblSumCount.Size = new System.Drawing.Size(41, 12);
            this.lblSumCount.TabIndex = 16;
            this.lblSumCount.Text = "label2";
            // 
            // customContextMenuStrip_Select1
            // 
            this.customContextMenuStrip_Select1.Name = "customContextMenuStrip_Select1";
            this.customContextMenuStrip_Select1.Size = new System.Drawing.Size(137, 136);
            // 
            // FormSelectPersonnel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(744, 619);
            this.Controls.Add(this.lblSumCount);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.toolStrip);
            this.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(760, 800);
            this.Name = "FormSelectPersonnel";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "选择用户";
            this.Load += new System.EventHandler(this.FormSelectUser_Load);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.panelTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton btnSelectField;
        private System.Windows.Forms.ToolStripButton btnClose;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.DataGridView dataGridView;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.ToolStripButton btnSelectAll;
        private System.Windows.Forms.ToolStripButton btnClearAll;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnShowSelectedData;
        private System.Windows.Forms.ToolStripButton btnShowUnselectedData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblSumCount;
        private CustomContextMenuStrip_Select customContextMenuStrip_Select1;
    }
}