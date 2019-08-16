using UniversalControlLibrary;
namespace Expression
{
    partial class 工装与工位匹配管理
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.panel6 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.userControlDataLocalizer1 = new UniversalControlLibrary.UserControlDataLocalizer();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtFrockNumber = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtCode = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtName = new UniversalControlLibrary.TextBoxShow();
            this.label4 = new System.Windows.Forms.Label();
            this.btnContentNew = new System.Windows.Forms.Button();
            this.btnContentUpdate = new System.Windows.Forms.Button();
            this.btnContentDelete = new System.Windows.Forms.Button();
            this.btnContentAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtWorkBench = new System.Windows.Forms.TextBox();
            this.panel6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.panel2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.panel5.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel6.Controls.Add(this.labelTitle);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel6.Location = new System.Drawing.Point(0, 0);
            this.panel6.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(1223, 57);
            this.panel6.TabIndex = 47;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(524, 15);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(174, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "工装与工位匹配管理";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Right;
            this.panel1.Location = new System.Drawing.Point(1123, 57);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(100, 513);
            this.panel1.TabIndex = 49;
            // 
            // panel3
            // 
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 57);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(100, 513);
            this.panel3.TabIndex = 50;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel2);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(100, 57);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(1023, 513);
            this.groupBox2.TabIndex = 51;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "工装设置";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.groupBox1);
            this.panel2.Controls.Add(this.panel5);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1017, 491);
            this.panel2.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridView1);
            this.groupBox1.Controls.Add(this.userControlDataLocalizer1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1017, 340);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "设置信息";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(3, 48);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1011, 289);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Top;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(3, 19);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(1011, 29);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 2;
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.txtFrockNumber);
            this.panel5.Controls.Add(this.label6);
            this.panel5.Controls.Add(this.txtCode);
            this.panel5.Controls.Add(this.label5);
            this.panel5.Controls.Add(this.txtName);
            this.panel5.Controls.Add(this.label4);
            this.panel5.Controls.Add(this.btnContentNew);
            this.panel5.Controls.Add(this.btnContentUpdate);
            this.panel5.Controls.Add(this.btnContentDelete);
            this.panel5.Controls.Add(this.btnContentAdd);
            this.panel5.Controls.Add(this.label1);
            this.panel5.Controls.Add(this.txtWorkBench);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(1017, 151);
            this.panel5.TabIndex = 1;
            // 
            // txtFrockNumber
            // 
            this.txtFrockNumber.Location = new System.Drawing.Point(118, 66);
            this.txtFrockNumber.Name = "txtFrockNumber";
            this.txtFrockNumber.ReadOnly = true;
            this.txtFrockNumber.Size = new System.Drawing.Size(329, 23);
            this.txtFrockNumber.TabIndex = 53;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(22, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 52;
            this.label6.Text = "工装编号";
            // 
            // txtCode
            // 
            this.txtCode.Location = new System.Drawing.Point(559, 21);
            this.txtCode.Name = "txtCode";
            this.txtCode.ReadOnly = true;
            this.txtCode.Size = new System.Drawing.Size(329, 23);
            this.txtCode.TabIndex = 51;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(462, 25);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 14);
            this.label5.TabIndex = 50;
            this.label5.Text = "工装图号";
            // 
            // txtName
            // 
            this.txtName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.工装编号;
            this.txtName.Location = new System.Drawing.Point(118, 21);
            this.txtName.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.txtName.Name = "txtName";
            this.txtName.ReadOnly = true;
            this.txtName.ShowResultForm = true;
            this.txtName.Size = new System.Drawing.Size(329, 23);
            this.txtName.TabIndex = 49;
            this.txtName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtName_OnCompleteSearch);
            this.txtName.Enter += new System.EventHandler(this.txtName_Enter);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(21, 25);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 48;
            this.label4.Text = "工装名称";
            // 
            // btnContentNew
            // 
            this.btnContentNew.Location = new System.Drawing.Point(448, 109);
            this.btnContentNew.Name = "btnContentNew";
            this.btnContentNew.Size = new System.Drawing.Size(101, 29);
            this.btnContentNew.TabIndex = 47;
            this.btnContentNew.Tag = "Update";
            this.btnContentNew.Text = "新建";
            this.btnContentNew.UseVisualStyleBackColor = true;
            this.btnContentNew.Click += new System.EventHandler(this.btnContentNew_Click);
            // 
            // btnContentUpdate
            // 
            this.btnContentUpdate.Location = new System.Drawing.Point(673, 109);
            this.btnContentUpdate.Name = "btnContentUpdate";
            this.btnContentUpdate.Size = new System.Drawing.Size(101, 29);
            this.btnContentUpdate.TabIndex = 13;
            this.btnContentUpdate.Tag = "Update";
            this.btnContentUpdate.Text = "修改";
            this.btnContentUpdate.UseVisualStyleBackColor = true;
            this.btnContentUpdate.Click += new System.EventHandler(this.btnContentUpdate_Click);
            // 
            // btnContentDelete
            // 
            this.btnContentDelete.Location = new System.Drawing.Point(787, 109);
            this.btnContentDelete.Name = "btnContentDelete";
            this.btnContentDelete.Size = new System.Drawing.Size(101, 29);
            this.btnContentDelete.TabIndex = 12;
            this.btnContentDelete.Tag = "Update";
            this.btnContentDelete.Text = "删除";
            this.btnContentDelete.UseVisualStyleBackColor = true;
            this.btnContentDelete.Click += new System.EventHandler(this.btnContentDelete_Click);
            // 
            // btnContentAdd
            // 
            this.btnContentAdd.Location = new System.Drawing.Point(559, 109);
            this.btnContentAdd.Name = "btnContentAdd";
            this.btnContentAdd.Size = new System.Drawing.Size(101, 29);
            this.btnContentAdd.TabIndex = 11;
            this.btnContentAdd.Tag = "Update";
            this.btnContentAdd.Text = "添加";
            this.btnContentAdd.UseVisualStyleBackColor = true;
            this.btnContentAdd.Click += new System.EventHandler(this.btnContentAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(462, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 8;
            this.label1.Text = "工    位";
            // 
            // txtWorkBench
            // 
            this.txtWorkBench.Location = new System.Drawing.Point(559, 66);
            this.txtWorkBench.Name = "txtWorkBench";
            this.txtWorkBench.Size = new System.Drawing.Size(329, 23);
            this.txtWorkBench.TabIndex = 7;
            // 
            // 工装与工位匹配管理
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1223, 570);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.panel6);
            this.Font = new System.Drawing.Font("宋体", 10F);
            this.Name = "工装与工位匹配管理";
            this.Load += new System.EventHandler(this.工装与工位匹配管理_Load);
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TextBox txtFrockNumber;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtCode;
        private System.Windows.Forms.Label label5;
        private TextBoxShow txtName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnContentNew;
        private System.Windows.Forms.Button btnContentUpdate;
        private System.Windows.Forms.Button btnContentDelete;
        private System.Windows.Forms.Button btnContentAdd;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtWorkBench;
        private System.Windows.Forms.DataGridView dataGridView1;
        private UserControlDataLocalizer userControlDataLocalizer1;
    }
}
