namespace Form_Peripheral_External
{
    partial class 报表_外部调运单
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
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource2 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.DepotManagementDataSet = new Form_Peripheral_External.DepotManagementDataSet();
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DepotManagementDataSet_View_Out_ManeuverBill";
            reportDataSource1.Value = null;
            reportDataSource2.Name = "DepotManagementDataSet_View_Out_ManeuverList";
            reportDataSource2.Value = null;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource2);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Form_Peripheral_External.报表.报表_外部调运单.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(860, 350);
            this.reportViewer1.TabIndex = 0;
            this.reportViewer1.Print += new System.ComponentModel.CancelEventHandler(this.reportViewer1_Print);
            // 
            // DepotManagementDataSet
            // 
            this.DepotManagementDataSet.DataSetName = "DepotManagementDataSet";
            this.DepotManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // 报表_外部调运单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(860, 350);
            this.Controls.Add(this.reportViewer1);
            this.Name = "报表_外部调运单";
            this.Text = "报表_外部调运单";
            this.Load += new System.EventHandler(this.报表_外部调运单_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DepotManagementDataSet DepotManagementDataSet;
    }
}