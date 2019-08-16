namespace UniversalControlLibrary
{
    partial class FormFindCondition
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
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.groupBoxParameter = new System.Windows.Forms.GroupBox();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btnAddCondition = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panelParameter = new System.Windows.Forms.Panel();
            this.panelCenter.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.groupBoxParameter.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.panelTop);
            this.panelCenter.Controls.Add(this.panelRight);
            this.panelCenter.Controls.Add(this.panelLeft);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 0);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(893, 118);
            this.panelCenter.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.groupBoxParameter);
            this.panelTop.Controls.Add(this.panel1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelTop.Location = new System.Drawing.Point(20, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(853, 118);
            this.panelTop.TabIndex = 1;
            // 
            // groupBoxParameter
            // 
            this.groupBoxParameter.Controls.Add(this.panelParameter);
            this.groupBoxParameter.Controls.Add(this.panelTitle);
            this.groupBoxParameter.Controls.Add(this.panel2);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.Location = new System.Drawing.Point(0, 7);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(853, 111);
            this.groupBoxParameter.TabIndex = 0;
            this.groupBoxParameter.TabStop = false;
            this.groupBoxParameter.Text = "条件设置区";
            // 
            // panelTitle
            // 
            this.panelTitle.Controls.Add(this.label7);
            this.panelTitle.Controls.Add(this.label1);
            this.panelTitle.Controls.Add(this.label6);
            this.panelTitle.Controls.Add(this.label5);
            this.panelTitle.Controls.Add(this.label4);
            this.panelTitle.Controls.Add(this.label3);
            this.panelTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTitle.Location = new System.Drawing.Point(3, 49);
            this.panelTitle.Name = "panelTitle";
            this.panelTitle.Size = new System.Drawing.Size(847, 23);
            this.panelTitle.TabIndex = 2;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(29, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 14);
            this.label7.TabIndex = 6;
            this.label7.Text = "左括号";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(619, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 5;
            this.label1.Text = "右括号";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(680, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 14);
            this.label6.TabIndex = 4;
            this.label6.Text = "关系符";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(472, 6);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 14);
            this.label5.TabIndex = 3;
            this.label5.Text = "查询值";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(318, 6);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 2;
            this.label4.Text = "查询条件";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(162, 6);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 1;
            this.label3.Text = "查询字段";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btnAddCondition);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.btnFind);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(847, 30);
            this.panel2.TabIndex = 0;
            // 
            // btnAddCondition
            // 
            this.btnAddCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCondition.ForeColor = System.Drawing.Color.Blue;
            this.btnAddCondition.Location = new System.Drawing.Point(99, 4);
            this.btnAddCondition.Name = "btnAddCondition";
            this.btnAddCondition.Size = new System.Drawing.Size(138, 23);
            this.btnAddCondition.TabIndex = 1;
            this.btnAddCondition.Text = "[新增查询条件&A]";
            this.btnAddCondition.UseVisualStyleBackColor = true;
            this.btnAddCondition.MouseLeave += new System.EventHandler(this.btnAddCondition_MouseLeave);
            this.btnAddCondition.Click += new System.EventHandler(this.btnAddCondition_Click);
            this.btnAddCondition.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAddCondition_MouseDown);
            this.btnAddCondition.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnAddCondition_MouseUp);
            this.btnAddCondition.MouseEnter += new System.EventHandler(this.btnAddCondition_MouseEnter);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 8);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "查询范围：";
            // 
            // btnFind
            // 
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Location = new System.Drawing.Point(270, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(129, 27);
            this.btnFind.TabIndex = 0;
            this.btnFind.Text = "提交查询条件(&S)";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnSumbitFindCondition_Click);
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(853, 7);
            this.panel1.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(873, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(20, 118);
            this.panelRight.TabIndex = 3;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(20, 118);
            this.panelLeft.TabIndex = 2;
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.CreatePrompt = true;
            this.saveFileDialog1.DefaultExt = "xls";
            this.saveFileDialog1.FileName = "*.xls";
            this.saveFileDialog1.Filter = "EXCEL 文件|*.xls";
            this.saveFileDialog1.OverwritePrompt = false;
            this.saveFileDialog1.Title = "将查询结果保存成 EXCEL 文件";
            // 
            // panelParameter
            // 
            this.panelParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParameter.Location = new System.Drawing.Point(3, 72);
            this.panelParameter.Name = "panelParameter";
            this.panelParameter.Size = new System.Drawing.Size(847, 36);
            this.panelParameter.TabIndex = 1;
            // 
            // FormFindCondition
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(893, 118);
            this.Controls.Add(this.panelCenter);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormFindCondition";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "创建查询条件";
            this.Load += new System.EventHandler(this.FormFindCondition_Load);
            this.Resize += new System.EventHandler(this.FormFindCondition_Resize);
            this.panelCenter.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.groupBoxParameter.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelCenter;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.GroupBox groupBoxParameter;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Panel panelLeft;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddCondition;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panelParameter;
    }
}