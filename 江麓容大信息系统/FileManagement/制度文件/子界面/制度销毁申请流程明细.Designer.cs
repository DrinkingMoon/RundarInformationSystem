namespace Form_Quality_File
{
    partial class 制度销毁申请流程明细
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
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.txtVersion = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtFileNo = new UniversalControlLibrary.TextBoxShow();
            this.lbPropoerTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbPropoer = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbSDBStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.txtSDBNo = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.customPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // customPanel1
            // 
            this.customPanel1.AutoScroll = true;
            this.customPanel1.Controls.Add(this.txtVersion);
            this.customPanel1.Controls.Add(this.label18);
            this.customPanel1.Controls.Add(this.txtFileNo);
            this.customPanel1.Controls.Add(this.lbPropoerTime);
            this.customPanel1.Controls.Add(this.label10);
            this.customPanel1.Controls.Add(this.lbPropoer);
            this.customPanel1.Controls.Add(this.label8);
            this.customPanel1.Controls.Add(this.lbSDBStatus);
            this.customPanel1.Controls.Add(this.label5);
            this.customPanel1.Controls.Add(this.txtRemark);
            this.customPanel1.Controls.Add(this.label3);
            this.customPanel1.Controls.Add(this.txtFileName);
            this.customPanel1.Controls.Add(this.label2);
            this.customPanel1.Controls.Add(this.label13);
            this.customPanel1.Controls.Add(this.txtSDBNo);
            this.customPanel1.Controls.Add(this.label4);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(738, 243);
            this.customPanel1.TabIndex = 2;
            // 
            // txtVersion
            // 
            this.txtVersion.Enabled = false;
            this.txtVersion.Location = new System.Drawing.Point(87, 87);
            this.txtVersion.Name = "txtVersion";
            this.txtVersion.Size = new System.Drawing.Size(226, 21);
            this.txtVersion.TabIndex = 302;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(16, 91);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(65, 12);
            this.label18.TabIndex = 301;
            this.label18.Text = "文件版次号";
            // 
            // txtFileNo
            // 
            this.txtFileNo.DataResult = null;
            this.txtFileNo.EditingControlDataGridView = null;
            this.txtFileNo.EditingControlFormattedValue = "";
            this.txtFileNo.EditingControlRowIndex = 0;
            this.txtFileNo.EditingControlValueChanged = false;
            this.txtFileNo.FindItem = UniversalControlLibrary.TextBoxShow.FindType.体系文件;
            this.txtFileNo.Location = new System.Drawing.Point(87, 48);
            this.txtFileNo.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileNo.Name = "txtFileNo";
            this.txtFileNo.ShowResultForm = true;
            this.txtFileNo.Size = new System.Drawing.Size(226, 21);
            this.txtFileNo.StrEndSql = null;
            this.txtFileNo.TabIndex = 297;
            this.txtFileNo.TabStop = false;
            this.txtFileNo.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtFileNo_OnCompleteSearch);
            this.txtFileNo.Enter += new System.EventHandler(this.txtFileNo_Enter);
            // 
            // lbPropoerTime
            // 
            this.lbPropoerTime.AutoSize = true;
            this.lbPropoerTime.Location = new System.Drawing.Point(454, 91);
            this.lbPropoerTime.Name = "lbPropoerTime";
            this.lbPropoerTime.Size = new System.Drawing.Size(71, 12);
            this.lbPropoerTime.TabIndex = 286;
            this.lbPropoerTime.Text = "PropoerTime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(390, 91);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 285;
            this.label10.Text = "申请日期";
            // 
            // lbPropoer
            // 
            this.lbPropoer.AutoSize = true;
            this.lbPropoer.Location = new System.Drawing.Point(638, 91);
            this.lbPropoer.Name = "lbPropoer";
            this.lbPropoer.Size = new System.Drawing.Size(47, 12);
            this.lbPropoer.TabIndex = 284;
            this.lbPropoer.Text = "Propoer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(579, 91);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 283;
            this.label8.Text = "申 请 人";
            // 
            // lbSDBStatus
            // 
            this.lbSDBStatus.AutoSize = true;
            this.lbSDBStatus.ForeColor = System.Drawing.Color.Red;
            this.lbSDBStatus.Location = new System.Drawing.Point(419, 15);
            this.lbSDBStatus.Name = "lbSDBStatus";
            this.lbSDBStatus.Size = new System.Drawing.Size(59, 12);
            this.lbSDBStatus.TabIndex = 282;
            this.lbSDBStatus.Text = "SDBStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(327, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 281;
            this.label5.Text = "流程状态";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(84, 125);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(625, 102);
            this.txtRemark.TabIndex = 280;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(28, 170);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 279;
            this.label3.Text = "说    明";
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(388, 48);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(321, 21);
            this.txtFileName.TabIndex = 276;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(329, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 275;
            this.label2.Text = "文件名称";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(28, 52);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 274;
            this.label13.Text = "文件编号";
            // 
            // txtSDBNo
            // 
            this.txtSDBNo.BackColor = System.Drawing.Color.White;
            this.txtSDBNo.ForeColor = System.Drawing.Color.Red;
            this.txtSDBNo.Location = new System.Drawing.Point(87, 11);
            this.txtSDBNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSDBNo.Name = "txtSDBNo";
            this.txtSDBNo.ReadOnly = true;
            this.txtSDBNo.Size = new System.Drawing.Size(226, 21);
            this.txtSDBNo.TabIndex = 272;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(28, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 273;
            this.label4.Text = "流程编号";
            // 
            // 制度销毁申请流程明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(738, 243);
            this.Controls.Add(this.customPanel1);
            this.Name = "制度销毁申请流程明细";
            this.Text = "制度销毁申请流程明细";
            this.流程控制类型 = GlobalObject.CE_FormFlowType.自定义;
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel customPanel1;
        private System.Windows.Forms.TextBox txtVersion;
        private System.Windows.Forms.Label label18;
        private UniversalControlLibrary.TextBoxShow txtFileNo;
        private System.Windows.Forms.Label lbPropoerTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbPropoer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbSDBStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSDBNo;
        private System.Windows.Forms.Label label4;
    }
}