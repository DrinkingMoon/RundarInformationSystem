namespace Expression
{
    partial class 报表_质量信息反馈单
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.viewSALLMessMessageFeedbackBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.depotManagementDataSet = new Expression.DepotManagementDataSet();

            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.depotManagementDataSetBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.view_S_ALLMessMessageFeedbackTableAdapter = new Expression.DepotManagementDataSetTableAdapters.View_S_ALLMessMessageFeedbackTableAdapter();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.导出ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.viewSALLMessMessageFeedbackBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.depotManagementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.depotManagementDataSetBindingSource)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // viewSALLMessMessageFeedbackBindingSource
            // 
            this.viewSALLMessMessageFeedbackBindingSource.DataMember = "View_S_ALLMessMessageFeedback";
            this.viewSALLMessMessageFeedbackBindingSource.DataSource = this.depotManagementDataSet;
            // 
            // depotManagementDataSet
            // 
            this.depotManagementDataSet.DataSetName = "DepotManagementDataSet";
            this.depotManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DepotManagementDataSet_View_S_ALLMessMessageFeedback";
            reportDataSource1.Value = this.viewSALLMessMessageFeedbackBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Expression.报表.报表_质量信息反馈单.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 24);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ShowExportButton = false;
            this.reportViewer1.Size = new System.Drawing.Size(639, 477);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Print += new System.ComponentModel.CancelEventHandler(this.reportViewer1_Print);
            // 
            // depotManagementDataSetBindingSource
            // 
            this.depotManagementDataSetBindingSource.DataSource = this.depotManagementDataSet;
            this.depotManagementDataSetBindingSource.Position = 0;
            // 
            // view_S_ALLMessMessageFeedbackTableAdapter
            // 
            this.view_S_ALLMessMessageFeedbackTableAdapter.ClearBeforeFill = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.导出ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(639, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 导出ToolStripMenuItem
            // 
            this.导出ToolStripMenuItem.Name = "导出ToolStripMenuItem";
            this.导出ToolStripMenuItem.Size = new System.Drawing.Size(71, 20);
            this.导出ToolStripMenuItem.Text = "导出EXCEL";
            this.导出ToolStripMenuItem.Click += new System.EventHandler(this.导出ToolStripMenuItem_Click);
            // 
            // 报表_质量信息反馈单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(639, 501);
            this.Controls.Add(this.reportViewer1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "报表_质量信息反馈单";
            this.Text = "报表_质量信息反馈单";
            this.Load += new System.EventHandler(this.报表_质量信息反馈单_Load);
            ((System.ComponentModel.ISupportInitialize)(this.viewSALLMessMessageFeedbackBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.depotManagementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.depotManagementDataSetBindingSource)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DepotManagementDataSet depotManagementDataSet;
        private System.Windows.Forms.BindingSource depotManagementDataSetBindingSource;
        private System.Windows.Forms.BindingSource viewSALLMessMessageFeedbackBindingSource;
        private Expression.DepotManagementDataSetTableAdapters.View_S_ALLMessMessageFeedbackTableAdapter view_S_ALLMessMessageFeedbackTableAdapter;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 导出ToolStripMenuItem;
    }
}