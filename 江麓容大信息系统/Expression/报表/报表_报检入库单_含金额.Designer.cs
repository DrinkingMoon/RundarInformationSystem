namespace Expression
{
    partial class 报表_报检入库单_含金额
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
            this.View_S_CheckOutInDepotBillBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DepotManagementDataSet = new Expression.DepotManagementDataSet();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.View_S_CheckOutInDepotBillTableAdapter = new Expression.DepotManagementDataSetTableAdapters.View_S_CheckOutInDepotBillTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.View_S_CheckOutInDepotBillBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).BeginInit();
            this.SuspendLayout();
            // 
            // View_S_CheckOutInDepotBillBindingSource
            // 
            this.View_S_CheckOutInDepotBillBindingSource.DataMember = "View_S_CheckOutInDepotBill";
            this.View_S_CheckOutInDepotBillBindingSource.DataSource = this.DepotManagementDataSet;
            // 
            // DepotManagementDataSet
            // 
            this.DepotManagementDataSet.DataSetName = "DepotManagementDataSet";
            this.DepotManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DepotManagementDataSet_View_S_CheckOutInDepotBill";
            reportDataSource1.Value = this.View_S_CheckOutInDepotBillBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Expression.报表.报表_报检入库单_含金额.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(758, 311);
            this.reportViewer1.TabIndex = 0;
            // 
            // View_S_CheckOutInDepotBillTableAdapter
            // 
            this.View_S_CheckOutInDepotBillTableAdapter.ClearBeforeFill = true;
            // 
            // 报表_报检入库单_含金额
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 311);
            this.Controls.Add(this.reportViewer1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F);
            this.Name = "报表_报检入库单_含金额";
            this.Text = "零部件报检入库单";
            this.Load += new System.EventHandler(this.FormReportCheckOutInDepotBil1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.View_S_CheckOutInDepotBillBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource View_S_CheckOutInDepotBillBindingSource;
        private DepotManagementDataSet DepotManagementDataSet;
        private Expression.DepotManagementDataSetTableAdapters.View_S_CheckOutInDepotBillTableAdapter View_S_CheckOutInDepotBillTableAdapter;

    }
}