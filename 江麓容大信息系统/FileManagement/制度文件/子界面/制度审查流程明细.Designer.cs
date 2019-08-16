namespace Form_Quality_File
{
    partial class 制度审查流程明细
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
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.customPanel1 = new System.Windows.Forms.Panel();
            this.txtFileSort = new UniversalControlLibrary.TextBoxShow();
            this.label6 = new System.Windows.Forms.Label();
            this.lbPropoerTime = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lbPropoer = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lbSDBStatus = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.llbFileDownLoad = new System.Windows.Forms.LinkLabel();
            this.llbFileUpLoad = new System.Windows.Forms.LinkLabel();
            this.txtFileNo = new System.Windows.Forms.TextBox();
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
            this.customPanel1.Controls.Add(this.txtFileSort);
            this.customPanel1.Controls.Add(this.label6);
            this.customPanel1.Controls.Add(this.lbPropoerTime);
            this.customPanel1.Controls.Add(this.label10);
            this.customPanel1.Controls.Add(this.lbPropoer);
            this.customPanel1.Controls.Add(this.label8);
            this.customPanel1.Controls.Add(this.lbSDBStatus);
            this.customPanel1.Controls.Add(this.label5);
            this.customPanel1.Controls.Add(this.txtRemark);
            this.customPanel1.Controls.Add(this.label3);
            this.customPanel1.Controls.Add(this.llbFileDownLoad);
            this.customPanel1.Controls.Add(this.llbFileUpLoad);
            this.customPanel1.Controls.Add(this.txtFileNo);
            this.customPanel1.Controls.Add(this.txtFileName);
            this.customPanel1.Controls.Add(this.label2);
            this.customPanel1.Controls.Add(this.label13);
            this.customPanel1.Controls.Add(this.txtSDBNo);
            this.customPanel1.Controls.Add(this.label4);
            this.customPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customPanel1.Location = new System.Drawing.Point(0, 0);
            this.customPanel1.Name = "customPanel1";
            this.customPanel1.Size = new System.Drawing.Size(753, 229);
            this.customPanel1.TabIndex = 0;
            // 
            // txtFileSort
            // 
            this.txtFileSort.DataResult = null;
            this.txtFileSort.EditingControlDataGridView = null;
            this.txtFileSort.EditingControlFormattedValue = "";
            this.txtFileSort.EditingControlRowIndex = 0;
            this.txtFileSort.EditingControlValueChanged = false;
            this.txtFileSort.FindItem = UniversalControlLibrary.TextBoxShow.FindType.文件类别;
            this.txtFileSort.Location = new System.Drawing.Point(82, 78);
            this.txtFileSort.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtFileSort.Name = "txtFileSort";
            this.txtFileSort.ShowResultForm = true;
            this.txtFileSort.Size = new System.Drawing.Size(175, 21);
            this.txtFileSort.StrEndSql = null;
            this.txtFileSort.TabIndex = 252;
            this.txtFileSort.TabStop = false;
            this.txtFileSort.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtFileSort_OnCompleteSearch);
            this.txtFileSort.Enter += new System.EventHandler(this.txtFileSort_Enter);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(23, 82);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 251;
            this.label6.Text = "文件类别";
            // 
            // lbPropoerTime
            // 
            this.lbPropoerTime.AutoSize = true;
            this.lbPropoerTime.Location = new System.Drawing.Point(658, 82);
            this.lbPropoerTime.Name = "lbPropoerTime";
            this.lbPropoerTime.Size = new System.Drawing.Size(71, 12);
            this.lbPropoerTime.TabIndex = 250;
            this.lbPropoerTime.Text = "PropoerTime";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(592, 82);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 249;
            this.label10.Text = "申请日期";
            // 
            // lbPropoer
            // 
            this.lbPropoer.AutoSize = true;
            this.lbPropoer.Location = new System.Drawing.Point(488, 82);
            this.lbPropoer.Name = "lbPropoer";
            this.lbPropoer.Size = new System.Drawing.Size(47, 12);
            this.lbPropoer.TabIndex = 248;
            this.lbPropoer.Text = "Propoer";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(429, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 247;
            this.label8.Text = "申 请 人";
            // 
            // lbSDBStatus
            // 
            this.lbSDBStatus.AutoSize = true;
            this.lbSDBStatus.ForeColor = System.Drawing.Color.Red;
            this.lbSDBStatus.Location = new System.Drawing.Point(437, 15);
            this.lbSDBStatus.Name = "lbSDBStatus";
            this.lbSDBStatus.Size = new System.Drawing.Size(59, 12);
            this.lbSDBStatus.TabIndex = 246;
            this.lbSDBStatus.Text = "SDBStatus";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(340, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 245;
            this.label5.Text = "流程状态";
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(82, 115);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtRemark.Size = new System.Drawing.Size(647, 102);
            this.txtRemark.TabIndex = 244;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 160);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 243;
            this.label3.Text = "说    明";
            // 
            // llbFileDownLoad
            // 
            this.llbFileDownLoad.AutoSize = true;
            this.llbFileDownLoad.Location = new System.Drawing.Point(340, 82);
            this.llbFileDownLoad.Name = "llbFileDownLoad";
            this.llbFileDownLoad.Size = new System.Drawing.Size(53, 12);
            this.llbFileDownLoad.TabIndex = 242;
            this.llbFileDownLoad.TabStop = true;
            this.llbFileDownLoad.Text = "下载文件";
            this.llbFileDownLoad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbFileDownLoad_LinkClicked);
            // 
            // llbFileUpLoad
            // 
            this.llbFileUpLoad.AutoSize = true;
            this.llbFileUpLoad.Location = new System.Drawing.Point(271, 82);
            this.llbFileUpLoad.Name = "llbFileUpLoad";
            this.llbFileUpLoad.Size = new System.Drawing.Size(53, 12);
            this.llbFileUpLoad.TabIndex = 241;
            this.llbFileUpLoad.TabStop = true;
            this.llbFileUpLoad.Text = "上传文件";
            this.llbFileUpLoad.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llbFileUpLoad_LinkClicked);
            // 
            // txtFileNo
            // 
            this.txtFileNo.Location = new System.Drawing.Point(81, 43);
            this.txtFileNo.Name = "txtFileNo";
            this.txtFileNo.Size = new System.Drawing.Size(244, 21);
            this.txtFileNo.TabIndex = 234;
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(401, 43);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(328, 21);
            this.txtFileName.TabIndex = 239;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(342, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 238;
            this.label2.Text = "文件名称";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(23, 47);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(53, 12);
            this.label13.TabIndex = 237;
            this.label13.Text = "文件编号";
            // 
            // txtSDBNo
            // 
            this.txtSDBNo.BackColor = System.Drawing.Color.White;
            this.txtSDBNo.ForeColor = System.Drawing.Color.Red;
            this.txtSDBNo.Location = new System.Drawing.Point(81, 11);
            this.txtSDBNo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtSDBNo.Name = "txtSDBNo";
            this.txtSDBNo.ReadOnly = true;
            this.txtSDBNo.Size = new System.Drawing.Size(244, 21);
            this.txtSDBNo.TabIndex = 235;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(23, 15);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 236;
            this.label4.Text = "流程编号";
            // 
            // 制度审查流程明细
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(753, 229);
            this.Controls.Add(this.customPanel1);
            this.Name = "制度审查流程明细";
            this.Text = "制度审查流程明细";
            this.流程控制类型 = GlobalObject.CE_FormFlowType.自定义;
            this.PanelGetDataInfo += new GlobalObject.DelegateCollection.GetDataInfo(this.customForm_PanelGetDateInfo);
            this.customPanel1.ResumeLayout(false);
            this.customPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Panel customPanel1;
        private UniversalControlLibrary.TextBoxShow txtFileSort;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lbPropoerTime;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lbPropoer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lbSDBStatus;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.LinkLabel llbFileDownLoad;
        private System.Windows.Forms.LinkLabel llbFileUpLoad;
        private System.Windows.Forms.TextBox txtFileNo;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtSDBNo;
        private System.Windows.Forms.Label label4;
    }
}