﻿namespace Expression
{
    partial class 报表_营销业务单据
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
            this.DepotManagementDataSet = new Expression.DepotManagementDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.View_S_MarketingAllBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.View_S_MarketingAllTableAdapter = new Expression.DepotManagementDataSetTableAdapters.View_S_MarketingAllTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_S_MarketingAllBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // DepotManagementDataSet
            // 
            this.DepotManagementDataSet.DataSetName = "DepotManagementDataSet";
            this.DepotManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DepotManagementDataSet_View_S_MarketingAll";
            reportDataSource1.Value = this.View_S_MarketingAllBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Expression.报表.报表_营销业务单.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ShowExportButton = false;
            this.reportViewer1.Size = new System.Drawing.Size(635, 294);
            this.reportViewer1.TabIndex = 1;
            this.reportViewer1.Print += new System.ComponentModel.CancelEventHandler(this.reportViewer1_Print);
            // 
            // View_S_MarketingAllBindingSource
            // 
            this.View_S_MarketingAllBindingSource.DataMember = "View_S_MarketingAll";
            this.View_S_MarketingAllBindingSource.DataSource = this.DepotManagementDataSet;
            // 
            // View_S_MarketingAllTableAdapter
            // 
            this.View_S_MarketingAllTableAdapter.ClearBeforeFill = true;
            // 
            // 报表_营销业务单据
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(635, 294);
            this.Controls.Add(this.reportViewer1);
            this.Name = "报表_营销业务单据";
            this.Text = "报表_营销业务单据";
            this.Load += new System.EventHandler(this.报表_营销入库单_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_S_MarketingAllBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DepotManagementDataSet DepotManagementDataSet;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource View_S_MarketingAllBindingSource;
        private Expression.DepotManagementDataSetTableAdapters.View_S_MarketingAllTableAdapter View_S_MarketingAllTableAdapter;
    }
}