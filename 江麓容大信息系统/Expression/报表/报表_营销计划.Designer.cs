namespace Expression
{
    partial class 报表_营销计划
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.depotManagementDataSet = new Expression.DepotManagementDataSet();
            this.viewSMarketingPlanBillBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.view_S_MarketingPlanBillTableAdapter = new Expression.DepotManagementDataSetTableAdapters.View_S_MarketingPlanBillTableAdapter();
            this.viewSMarketingPlanListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.view_S_MarketingPlanListTableAdapter = new Expression.DepotManagementDataSetTableAdapters.View_S_MarketingPlanListTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.depotManagementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSMarketingPlanBillBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSMarketingPlanListBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DepotManagementDataSet_View_S_MarketingPlanBill";
            reportDataSource1.Value = this.viewSMarketingPlanBillBindingSource;
            reportDataSource2.Name = "DepotManagementDataSet_View_S_MarketingPlanList";
            reportDataSource2.Value = this.viewSMarketingPlanListBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Expression.报表.报表_营销计划.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(742, 460);
            this.reportViewer1.TabIndex = 0;
            // 
            // depotManagementDataSet
            // 
            this.depotManagementDataSet.DataSetName = "DepotManagementDataSet";
            this.depotManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // viewSMarketingPlanBillBindingSource
            // 
            this.viewSMarketingPlanBillBindingSource.DataMember = "View_S_MarketingPlanBill";
            this.viewSMarketingPlanBillBindingSource.DataSource = this.depotManagementDataSet;
            // 
            // view_S_MarketingPlanBillTableAdapter
            // 
            this.view_S_MarketingPlanBillTableAdapter.ClearBeforeFill = true;
            // 
            // viewSMarketingPlanListBindingSource
            // 
            this.viewSMarketingPlanListBindingSource.DataMember = "View_S_MarketingPlanList";
            this.viewSMarketingPlanListBindingSource.DataSource = this.depotManagementDataSet;
            // 
            // view_S_MarketingPlanListTableAdapter
            // 
            this.view_S_MarketingPlanListTableAdapter.ClearBeforeFill = true;
            // 
            // 报表_营销计划
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(742, 460);
            this.Controls.Add(this.reportViewer1);
            this.Name = "报表_营销计划";
            this.Text = "报表_营销计划";
            this.Load += new System.EventHandler(this.报表_营销计划_Load);
            ((System.ComponentModel.ISupportInitialize)(this.depotManagementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSMarketingPlanBillBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.viewSMarketingPlanListBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DepotManagementDataSet depotManagementDataSet;
        private System.Windows.Forms.BindingSource viewSMarketingPlanBillBindingSource;
        private Expression.DepotManagementDataSetTableAdapters.View_S_MarketingPlanBillTableAdapter view_S_MarketingPlanBillTableAdapter;
        private System.Windows.Forms.BindingSource viewSMarketingPlanListBindingSource;
        private Expression.DepotManagementDataSetTableAdapters.View_S_MarketingPlanListTableAdapter view_S_MarketingPlanListTableAdapter;
    }
}