namespace UniversalControlLibrary
{
    partial class FormConditionFind
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
            this.panelCenter = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.groupBoxParameter = new System.Windows.Forms.GroupBox();
            this.panelParameter = new System.Windows.Forms.Panel();
            this.panelTitle = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.lblCurRow = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.lblAmount = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.btnSaveCondition = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panelSelectSearch = new System.Windows.Forms.Panel();
            this.btnDeleteSearch = new System.Windows.Forms.Button();
            this.cmbSearchName = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnAddCondition = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelRight = new System.Windows.Forms.Panel();
            this.panelLeft = new System.Windows.Forms.Panel();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.数据展示ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.dataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.panelCenter.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.groupBoxParameter.SuspendLayout();
            this.panelTitle.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panelSelectSearch.SuspendLayout();
            this.panel3.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // panelCenter
            // 
            this.panelCenter.Controls.Add(this.dataGridView1);
            this.panelCenter.Controls.Add(this.panelTop);
            this.panelCenter.Controls.Add(this.panelRight);
            this.panelCenter.Controls.Add(this.panelLeft);
            this.panelCenter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelCenter.Location = new System.Drawing.Point(0, 0);
            this.panelCenter.Name = "panelCenter";
            this.panelCenter.Size = new System.Drawing.Size(1076, 616);
            this.panelCenter.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.groupBoxParameter);
            this.panelTop.Controls.Add(this.panel1);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(20, 0);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(1036, 145);
            this.panelTop.TabIndex = 1;
            // 
            // groupBoxParameter
            // 
            this.groupBoxParameter.Controls.Add(this.panelParameter);
            this.groupBoxParameter.Controls.Add(this.panelTitle);
            this.groupBoxParameter.Controls.Add(this.panel4);
            this.groupBoxParameter.Controls.Add(this.panel2);
            this.groupBoxParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBoxParameter.Location = new System.Drawing.Point(0, 7);
            this.groupBoxParameter.Name = "groupBoxParameter";
            this.groupBoxParameter.Size = new System.Drawing.Size(1036, 138);
            this.groupBoxParameter.TabIndex = 0;
            this.groupBoxParameter.TabStop = false;
            this.groupBoxParameter.Text = "条件设置区";
            // 
            // panelParameter
            // 
            this.panelParameter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelParameter.Location = new System.Drawing.Point(3, 72);
            this.panelParameter.Name = "panelParameter";
            this.panelParameter.Size = new System.Drawing.Size(1030, 32);
            this.panelParameter.TabIndex = 1;
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
            this.panelTitle.Size = new System.Drawing.Size(1030, 23);
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
            // panel4
            // 
            this.panel4.Controls.Add(this.lblCurRow);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Controls.Add(this.lblAmount);
            this.panel4.Controls.Add(this.label8);
            this.panel4.Controls.Add(this.btnSaveCondition);
            this.panel4.Controls.Add(this.btnSave);
            this.panel4.Controls.Add(this.btnFind);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel4.Location = new System.Drawing.Point(3, 104);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(1030, 31);
            this.panel4.TabIndex = 1;
            // 
            // lblCurRow
            // 
            this.lblCurRow.AutoSize = true;
            this.lblCurRow.Location = new System.Drawing.Point(231, 9);
            this.lblCurRow.Name = "lblCurRow";
            this.lblCurRow.Size = new System.Drawing.Size(0, 14);
            this.lblCurRow.TabIndex = 5;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(162, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(63, 14);
            this.label10.TabIndex = 4;
            this.label10.Text = "当前行：";
            // 
            // lblAmount
            // 
            this.lblAmount.AutoSize = true;
            this.lblAmount.Location = new System.Drawing.Point(85, 9);
            this.lblAmount.Name = "lblAmount";
            this.lblAmount.Size = new System.Drawing.Size(0, 14);
            this.lblAmount.TabIndex = 3;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(16, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 2;
            this.label8.Text = "总行数：";
            // 
            // btnSaveCondition
            // 
            this.btnSaveCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSaveCondition.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSaveCondition.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSaveCondition.Location = new System.Drawing.Point(873, 2);
            this.btnSaveCondition.Name = "btnSaveCondition";
            this.btnSaveCondition.Size = new System.Drawing.Size(147, 27);
            this.btnSaveCondition.TabIndex = 1;
            this.btnSaveCondition.Text = "  保存我的查询条件";
            this.btnSaveCondition.UseVisualStyleBackColor = true;
            this.btnSaveCondition.Click += new System.EventHandler(this.btnSaveCondition_Click);
            // 
            // btnSave
            // 
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Image = global::UniversalControlLibrary.Properties.Resources.save;
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSave.Location = new System.Drawing.Point(695, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(168, 27);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "  保存我的查询结果(&S)";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFind
            // 
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Location = new System.Drawing.Point(557, 2);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(129, 27);
            this.btnFind.TabIndex = 0;
            this.btnFind.Text = "提交并查询(&F)";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.panelSelectSearch);
            this.panel2.Controls.Add(this.panel3);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(3, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1030, 30);
            this.panel2.TabIndex = 0;
            // 
            // panelSelectSearch
            // 
            this.panelSelectSearch.Controls.Add(this.btnDeleteSearch);
            this.panelSelectSearch.Controls.Add(this.cmbSearchName);
            this.panelSelectSearch.Controls.Add(this.label9);
            this.panelSelectSearch.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSelectSearch.Location = new System.Drawing.Point(272, 0);
            this.panelSelectSearch.Name = "panelSelectSearch";
            this.panelSelectSearch.Size = new System.Drawing.Size(562, 30);
            this.panelSelectSearch.TabIndex = 3;
            this.panelSelectSearch.Visible = false;
            // 
            // btnDeleteSearch
            // 
            this.btnDeleteSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteSearch.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.btnDeleteSearch.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnDeleteSearch.Location = new System.Drawing.Point(389, 2);
            this.btnDeleteSearch.Name = "btnDeleteSearch";
            this.btnDeleteSearch.Size = new System.Drawing.Size(157, 27);
            this.btnDeleteSearch.TabIndex = 2;
            this.btnDeleteSearch.Text = "   删除我的查询(&D)";
            this.btnDeleteSearch.UseVisualStyleBackColor = true;
            this.btnDeleteSearch.Click += new System.EventHandler(this.btnDeleteSearch_Click);
            // 
            // cmbSearchName
            // 
            this.cmbSearchName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSearchName.FormattingEnabled = true;
            this.cmbSearchName.Location = new System.Drawing.Point(130, 4);
            this.cmbSearchName.Name = "cmbSearchName";
            this.cmbSearchName.Size = new System.Drawing.Size(235, 22);
            this.cmbSearchName.TabIndex = 1;
            this.cmbSearchName.SelectedIndexChanged += new System.EventHandler(this.cmbSearchName_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 7);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(105, 14);
            this.label9.TabIndex = 0;
            this.label9.Text = "选择我的查询：";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.btnAddCondition);
            this.panel3.Controls.Add(this.label2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(272, 30);
            this.panel3.TabIndex = 2;
            // 
            // btnAddCondition
            // 
            this.btnAddCondition.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCondition.ForeColor = System.Drawing.Color.Blue;
            this.btnAddCondition.Location = new System.Drawing.Point(95, 3);
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
            this.label2.Location = new System.Drawing.Point(12, 7);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 14);
            this.label2.TabIndex = 0;
            this.label2.Text = "查询范围：";
            // 
            // panel1
            // 
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1036, 7);
            this.panel1.TabIndex = 0;
            // 
            // panelRight
            // 
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(1056, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(20, 616);
            this.panelRight.TabIndex = 3;
            // 
            // panelLeft
            // 
            this.panelLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelLeft.Location = new System.Drawing.Point(0, 0);
            this.panelLeft.Name = "panelLeft";
            this.panelLeft.Size = new System.Drawing.Size(20, 616);
            this.panelLeft.TabIndex = 2;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.数据展示ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 26);
            // 
            // 数据展示ToolStripMenuItem
            // 
            this.数据展示ToolStripMenuItem.Name = "数据展示ToolStripMenuItem";
            this.数据展示ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.数据展示ToolStripMenuItem.Text = "数据展示";
            this.数据展示ToolStripMenuItem.Click += new System.EventHandler(this.数据展示ToolStripMenuItem_Click);
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
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(20, 145);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(1036, 471);
            this.dataGridView1.TabIndex = 4;
            this.dataGridView1.DoubleClick += new System.EventHandler(this.dataGridView1_DoubleClick);
            this.dataGridView1.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDown);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // FormConditionFind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 616);
            this.Controls.Add(this.panelCenter);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FormConditionFind";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "查找条件(双击记录行进行数据定位)";
            this.Load += new System.EventHandler(this.FormFindCondition_Load);
            this.Resize += new System.EventHandler(this.FormFindCondition_Resize);
            this.panelCenter.ResumeLayout(false);
            this.panelTop.ResumeLayout(false);
            this.groupBoxParameter.ResumeLayout(false);
            this.panelTitle.ResumeLayout(false);
            this.panelTitle.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panelSelectSearch.ResumeLayout(false);
            this.panelSelectSearch.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
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
        private System.Windows.Forms.Panel panelParameter;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Panel panelTitle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAddCondition;
        private System.Windows.Forms.Label lblCurRow;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label lblAmount;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 数据展示ToolStripMenuItem;
        private System.Windows.Forms.Button btnSaveCondition;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panelSelectSearch;
        private System.Windows.Forms.ComboBox cmbSearchName;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button btnDeleteSearch;
        private CustomDataGridView dataGridView1;
    }
}