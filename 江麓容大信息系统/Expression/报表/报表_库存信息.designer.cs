namespace Expression
{
    partial class 报表_库存信息
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
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.DepotManagementDataSet = new Expression.DepotManagementDataSet();
            this.View_S_StockBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.View_S_StockTableAdapter = new Expression.DepotManagementDataSetTableAdapters.View_S_StockTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_S_StockBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DepotManagementDataSet_View_S_Stock";
            reportDataSource1.Value = this.View_S_StockBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Expression.报表.报表_库存信息.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.Size = new System.Drawing.Size(692, 311);
            this.reportViewer1.TabIndex = 0;
            // 
            // DepotManagementDataSet
            // 
            this.DepotManagementDataSet.DataSetName = "DepotManagementDataSet";
            this.DepotManagementDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // View_S_StockBindingSource
            // 
            this.View_S_StockBindingSource.DataMember = "View_S_Stock";
            this.View_S_StockBindingSource.DataSource = this.DepotManagementDataSet;
            // 
            // View_S_StockTableAdapter
            // 
            this.View_S_StockTableAdapter.ClearBeforeFill = true;
            // 
            // 报表_库存信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(692, 311);
            this.Controls.Add(this.reportViewer1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F);
            this.Name = "报表_库存信息";
            this.Text = "库存信息";
            this.Load += new System.EventHandler(this.报表_库存信息_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.View_S_StockBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private DepotManagementDataSet DepotManagementDataSet;
        private System.Windows.Forms.BindingSource View_S_StockBindingSource;
        private Expression.DepotManagementDataSetTableAdapters.View_S_StockTableAdapter View_S_StockTableAdapter;
    }
}