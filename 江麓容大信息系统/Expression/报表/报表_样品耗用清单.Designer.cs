namespace Expressio
{
    partial class 报表_样品耗用清单
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
            this.DepotManagementDataSet1 = new Expression.DepotManagementDataSet1();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet1)).BeginInit();
            this.SuspendLayout();
            // 
            // DepotManagementDataSet1
            // 
            this.DepotManagementDataSet1.DataSetName = "DepotManagementDataSet1";
            this.DepotManagementDataSet1.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Expression.报表.报表_样品耗用清单.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ShowExportButton = false;
            this.reportViewer1.Size = new System.Drawing.Size(623, 498);
            this.reportViewer1.TabIndex = 2;
            // 
            // 报表_样品耗用清单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(623, 498);
            this.Controls.Add(this.reportViewer1);
            this.Name = "报表_样品耗用清单";
            this.Text = "报表_样品耗用清单";
            this.Load += new System.EventHandler(this.报表_样品耗用清单_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DepotManagementDataSet1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Expression.DepotManagementDataSet1 DepotManagementDataSet1;
        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;

    }
}