

namespace Form_Economic_Purchase
{
    partial class 报表_采购结算单
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource3 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource4 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource5 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource6 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource7 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource8 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.SettlementDataSet = new Form_Economic_Purchase.SettlementDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewer2 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewer3 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.reportViewer4 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.View_Business_Settlement_ProcurementStatementBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.View_Business_Settlement_ProcurementStatementDetailBindingSource = new System.Windows.Forms.BindingSource(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.SettlementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_Business_Settlement_ProcurementStatementBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_Business_Settlement_ProcurementStatementDetailBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // SettlementDataSet
            // 
            this.SettlementDataSet.DataSetName = "SettlementDataSet";
            this.SettlementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Left;
            reportDataSource1.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatement";
            reportDataSource1.Value = this.View_Business_Settlement_ProcurementStatementBindingSource;
            reportDataSource2.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatementDetail";
            reportDataSource2.Value = this.View_Business_Settlement_ProcurementStatementDetailBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Form_Economic_Purchase.报表.报表_采购结算单.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(97, 356);
            this.reportViewer1.TabIndex = 1;
            this.reportViewer1.Visible = false;
            // 
            // reportViewer2
            // 
            this.reportViewer2.Dock = System.Windows.Forms.DockStyle.Left;
            reportDataSource3.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatement";
            reportDataSource4.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatementDetail";
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource3);
            this.reportViewer2.LocalReport.DataSources.Add(reportDataSource4);
            this.reportViewer2.LocalReport.ReportEmbeddedResource = "Form_Economic_Purchase.报表.报表_采购结算单(发供应商).rdlc";
            this.reportViewer2.Location = new System.Drawing.Point(97, 0);
            this.reportViewer2.Name = "reportViewer2";
            this.reportViewer2.Size = new System.Drawing.Size(138, 356);
            this.reportViewer2.TabIndex = 2;
            this.reportViewer2.Visible = false;
            // 
            // reportViewer3
            // 
            this.reportViewer3.Dock = System.Windows.Forms.DockStyle.Left;
            reportDataSource5.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatement";
            reportDataSource6.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatementDetail";
            this.reportViewer3.LocalReport.DataSources.Add(reportDataSource5);
            this.reportViewer3.LocalReport.DataSources.Add(reportDataSource6);
            this.reportViewer3.LocalReport.ReportEmbeddedResource = "Form_Economic_Purchase.报表.报表_采购结算单_委外.rdlc";
            this.reportViewer3.Location = new System.Drawing.Point(235, 0);
            this.reportViewer3.Name = "reportViewer3";
            this.reportViewer3.Size = new System.Drawing.Size(138, 356);
            this.reportViewer3.TabIndex = 3;
            this.reportViewer3.Visible = false;
            // 
            // reportViewer4
            // 
            this.reportViewer4.Dock = System.Windows.Forms.DockStyle.Left;
            reportDataSource7.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatement";
            reportDataSource8.Name = "SettlementDataSet_View_Business_Settlement_ProcurementStatementDetail";
            this.reportViewer4.LocalReport.DataSources.Add(reportDataSource7);
            this.reportViewer4.LocalReport.DataSources.Add(reportDataSource8);
            this.reportViewer4.LocalReport.ReportEmbeddedResource = "Form_Economic_Purchase.报表.报表_采购结算单_委外(发供应商).rdlc";
            this.reportViewer4.Location = new System.Drawing.Point(373, 0);
            this.reportViewer4.Name = "reportViewer4";
            this.reportViewer4.Size = new System.Drawing.Size(154, 356);
            this.reportViewer4.TabIndex = 4;
            this.reportViewer4.Visible = false;
            // 
            // View_Business_Settlement_ProcurementStatementBindingSource
            // 
            this.View_Business_Settlement_ProcurementStatementBindingSource.DataMember = "View_Business_Settlement_ProcurementStatement";
            this.View_Business_Settlement_ProcurementStatementBindingSource.DataSource = this.SettlementDataSet;
            // 
            // View_Business_Settlement_ProcurementStatementDetailBindingSource
            // 
            this.View_Business_Settlement_ProcurementStatementDetailBindingSource.DataMember = "View_Business_Settlement_ProcurementStatementDetail";
            this.View_Business_Settlement_ProcurementStatementDetailBindingSource.DataSource = this.SettlementDataSet;
            // 
            // 报表_采购结算单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(967, 356);
            this.Controls.Add(this.reportViewer4);
            this.Controls.Add(this.reportViewer3);
            this.Controls.Add(this.reportViewer2);
            this.Controls.Add(this.reportViewer1);
            this.Name = "报表_采购结算单";
            this.Text = "报表_采购结算单";
            this.Load += new System.EventHandler(this.报表_采购结算单_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SettlementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_Business_Settlement_ProcurementStatementBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_Business_Settlement_ProcurementStatementDetailBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private SettlementDataSet SettlementDataSet;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer2;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer3;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer4;
        private System.Windows.Forms.BindingSource View_Business_Settlement_ProcurementStatementBindingSource;
        private System.Windows.Forms.BindingSource View_Business_Settlement_ProcurementStatementDetailBindingSource;
    }
}