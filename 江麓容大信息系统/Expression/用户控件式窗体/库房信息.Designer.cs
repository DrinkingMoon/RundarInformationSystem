using UniversalControlLibrary;
namespace Expression
{
    partial class 库房信息
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
            this.panel5 = new System.Windows.Forms.Panel();
            this.labelTitle = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.btNewStorage = new System.Windows.Forms.Button();
            this.btDeleteStorage = new System.Windows.Forms.Button();
            this.btAddStorage = new System.Windows.Forms.Button();
            this.txtStorageName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtStorageID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.tbs_StorageName = new UniversalControlLibrary.TextBoxShow();
            this.tbs_Personnel = new UniversalControlLibrary.TextBoxShow();
            this.label5 = new System.Windows.Forms.Label();
            this.btNew = new System.Windows.Forms.Button();
            this.btDelete = new System.Windows.Forms.Button();
            this.btAdd = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.panel5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.ControlDark;
            this.panel5.Controls.Add(this.labelTitle);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel5.Location = new System.Drawing.Point(0, 0);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(917, 53);
            this.panel5.TabIndex = 55;
            // 
            // labelTitle
            // 
            this.labelTitle.AutoSize = true;
            this.labelTitle.Font = new System.Drawing.Font("宋体", 19F, System.Drawing.FontStyle.Bold);
            this.labelTitle.Location = new System.Drawing.Point(359, 11);
            this.labelTitle.Name = "labelTitle";
            this.labelTitle.Size = new System.Drawing.Size(120, 26);
            this.labelTitle.TabIndex = 0;
            this.labelTitle.Text = "库房信息";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.panel2);
            this.groupBox1.Controls.Add(this.panel1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Location = new System.Drawing.Point(0, 53);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(917, 291);
            this.groupBox1.TabIndex = 56;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "库房信息";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.btNewStorage);
            this.panel2.Controls.Add(this.btDeleteStorage);
            this.panel2.Controls.Add(this.btAddStorage);
            this.panel2.Controls.Add(this.txtStorageName);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.txtStorageID);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(577, 19);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(337, 269);
            this.panel2.TabIndex = 1;
            // 
            // btNewStorage
            // 
            this.btNewStorage.Location = new System.Drawing.Point(47, 177);
            this.btNewStorage.Name = "btNewStorage";
            this.btNewStorage.Size = new System.Drawing.Size(87, 25);
            this.btNewStorage.TabIndex = 7;
            this.btNewStorage.Tag = "add";
            this.btNewStorage.Text = "新建";
            this.btNewStorage.UseVisualStyleBackColor = true;
            this.btNewStorage.Click += new System.EventHandler(this.btNewStorage_Click);
            // 
            // btDeleteStorage
            // 
            this.btDeleteStorage.Location = new System.Drawing.Point(126, 228);
            this.btDeleteStorage.Name = "btDeleteStorage";
            this.btDeleteStorage.Size = new System.Drawing.Size(87, 25);
            this.btDeleteStorage.TabIndex = 6;
            this.btDeleteStorage.Tag = "delete";
            this.btDeleteStorage.Text = "删除";
            this.btDeleteStorage.UseVisualStyleBackColor = true;
            this.btDeleteStorage.Click += new System.EventHandler(this.btDeleteStorage_Click);
            // 
            // btAddStorage
            // 
            this.btAddStorage.Location = new System.Drawing.Point(208, 177);
            this.btAddStorage.Name = "btAddStorage";
            this.btAddStorage.Size = new System.Drawing.Size(87, 25);
            this.btAddStorage.TabIndex = 4;
            this.btAddStorage.Tag = "add";
            this.btAddStorage.Text = "添加";
            this.btAddStorage.UseVisualStyleBackColor = true;
            this.btAddStorage.Click += new System.EventHandler(this.btAddStorage_Click);
            // 
            // txtStorageName
            // 
            this.txtStorageName.Location = new System.Drawing.Point(124, 105);
            this.txtStorageName.Name = "txtStorageName";
            this.txtStorageName.Size = new System.Drawing.Size(158, 23);
            this.txtStorageName.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(33, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 16);
            this.label2.TabIndex = 2;
            this.label2.Text = "库房名称";
            // 
            // txtStorageID
            // 
            this.txtStorageID.Location = new System.Drawing.Point(124, 30);
            this.txtStorageID.Name = "txtStorageID";
            this.txtStorageID.Size = new System.Drawing.Size(158, 23);
            this.txtStorageID.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(72, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "库房编码";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.dataGridView1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(3, 19);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(574, 269);
            this.panel1.TabIndex = 0;
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(574, 269);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.Click += new System.EventHandler(this.dataGridView1_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.panel4);
            this.groupBox2.Controls.Add(this.panel3);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 344);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(917, 267);
            this.groupBox2.TabIndex = 57;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "关系信息";
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.tbs_StorageName);
            this.panel4.Controls.Add(this.tbs_Personnel);
            this.panel4.Controls.Add(this.label5);
            this.panel4.Controls.Add(this.btNew);
            this.panel4.Controls.Add(this.btDelete);
            this.panel4.Controls.Add(this.btAdd);
            this.panel4.Controls.Add(this.label3);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(577, 19);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(337, 245);
            this.panel4.TabIndex = 1;
            // 
            // tbs_StorageName
            // 
            this.tbs_StorageName.DataResult = null;
            this.tbs_StorageName.DataTableResult = null;
            this.tbs_StorageName.EditingControlDataGridView = null;
            this.tbs_StorageName.EditingControlFormattedValue = "";
            this.tbs_StorageName.EditingControlRowIndex = 0;
            this.tbs_StorageName.EditingControlValueChanged = false;
            this.tbs_StorageName.FindItem = UniversalControlLibrary.TextBoxShow.FindType.库房;
            this.tbs_StorageName.IsMultiSelect = false;
            this.tbs_StorageName.Location = new System.Drawing.Point(134, 35);
            this.tbs_StorageName.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbs_StorageName.Name = "tbs_StorageName";
            this.tbs_StorageName.ShowResultForm = true;
            this.tbs_StorageName.Size = new System.Drawing.Size(158, 23);
            this.tbs_StorageName.StrEndSql = null;
            this.tbs_StorageName.TabIndex = 18;
            this.tbs_StorageName.TabStop = false;
            this.tbs_StorageName.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbs_StorageName_OnCompleteSearch);
            // 
            // tbs_Personnel
            // 
            this.tbs_Personnel.DataResult = null;
            this.tbs_Personnel.DataTableResult = null;
            this.tbs_Personnel.EditingControlDataGridView = null;
            this.tbs_Personnel.EditingControlFormattedValue = "";
            this.tbs_Personnel.EditingControlRowIndex = 0;
            this.tbs_Personnel.EditingControlValueChanged = false;
            this.tbs_Personnel.FindItem = UniversalControlLibrary.TextBoxShow.FindType.人员;
            this.tbs_Personnel.IsMultiSelect = false;
            this.tbs_Personnel.Location = new System.Drawing.Point(134, 108);
            this.tbs_Personnel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbs_Personnel.Name = "tbs_Personnel";
            this.tbs_Personnel.ShowResultForm = true;
            this.tbs_Personnel.Size = new System.Drawing.Size(158, 23);
            this.tbs_Personnel.StrEndSql = null;
            this.tbs_Personnel.TabIndex = 17;
            this.tbs_Personnel.TabStop = false;
            this.tbs_Personnel.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.tbs_Personnel_OnCompleteSearch);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(43, 112);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 16;
            this.label5.Text = "人员名称";
            // 
            // btNew
            // 
            this.btNew.Location = new System.Drawing.Point(49, 182);
            this.btNew.Name = "btNew";
            this.btNew.Size = new System.Drawing.Size(87, 25);
            this.btNew.TabIndex = 15;
            this.btNew.Tag = "add";
            this.btNew.Text = "新建";
            this.btNew.UseVisualStyleBackColor = true;
            this.btNew.Click += new System.EventHandler(this.btNew_Click);
            // 
            // btDelete
            // 
            this.btDelete.Location = new System.Drawing.Point(126, 239);
            this.btDelete.Name = "btDelete";
            this.btDelete.Size = new System.Drawing.Size(87, 25);
            this.btDelete.TabIndex = 14;
            this.btDelete.Tag = "delete";
            this.btDelete.Text = "删除";
            this.btDelete.UseVisualStyleBackColor = true;
            this.btDelete.Click += new System.EventHandler(this.btDelete_Click);
            // 
            // btAdd
            // 
            this.btAdd.Location = new System.Drawing.Point(208, 182);
            this.btAdd.Name = "btAdd";
            this.btAdd.Size = new System.Drawing.Size(87, 25);
            this.btAdd.TabIndex = 12;
            this.btAdd.Tag = "add";
            this.btAdd.Text = "添加";
            this.btAdd.UseVisualStyleBackColor = true;
            this.btAdd.Click += new System.EventHandler(this.btAdd_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(43, 40);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(72, 16);
            this.label3.TabIndex = 10;
            this.label3.Text = "库房名称";
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.dataGridView2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel3.Location = new System.Drawing.Point(3, 19);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(574, 245);
            this.panel3.TabIndex = 0;
            // 
            // dataGridView2
            // 
            this.dataGridView2.AllowUserToAddRows = false;
            this.dataGridView2.AllowUserToDeleteRows = false;
            this.dataGridView2.AllowUserToResizeRows = false;
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView2.Location = new System.Drawing.Point(0, 0);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.ReadOnly = true;
            this.dataGridView2.RowTemplate.Height = 23;
            this.dataGridView2.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView2.Size = new System.Drawing.Size(574, 245);
            this.dataGridView2.TabIndex = 0;
            this.dataGridView2.Click += new System.EventHandler(this.dataGridView2_Click);
            // 
            // 库房信息
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(917, 611);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.panel5);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "库房信息";
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label labelTitle;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button btDeleteStorage;
        private System.Windows.Forms.Button btAddStorage;
        private System.Windows.Forms.TextBox txtStorageName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtStorageID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Button btNewStorage;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btNew;
        private System.Windows.Forms.Button btDelete;
        private System.Windows.Forms.Button btAdd;
        private System.Windows.Forms.Label label3;
        private TextBoxShow tbs_Personnel;
        private TextBoxShow tbs_StorageName;

    }
}
