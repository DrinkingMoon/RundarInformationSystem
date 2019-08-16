namespace UniversalControlLibrary
{
    partial class FrmShowList
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
            if (disposing)
            {
                if (components != null)
                {
                    components.Dispose();
                }
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnOK = new System.Windows.Forms.Button();
            this.txtShowlist = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.dGShowlist = new System.Windows.Forms.DataGridView();
            this.选 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.btnClear = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dGShowlist)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnClear);
            this.panel1.Controls.Add(this.btnOK);
            this.panel1.Controls.Add(this.txtShowlist);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(503, 39);
            this.panel1.TabIndex = 0;
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(370, 7);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // txtShowlist
            // 
            this.txtShowlist.Location = new System.Drawing.Point(73, 7);
            this.txtShowlist.MaxLength = 32;
            this.txtShowlist.Name = "txtShowlist";
            this.txtShowlist.Size = new System.Drawing.Size(286, 23);
            this.txtShowlist.TabIndex = 0;
            this.txtShowlist.DoubleClick += new System.EventHandler(this.txtShowlist_DoubleClick);
            this.txtShowlist.TextChanged += new System.EventHandler(this.txtShowlist_TextChanged);
            this.txtShowlist.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtShowlist_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "请输入";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.dGShowlist);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 39);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(503, 255);
            this.panel2.TabIndex = 2;
            // 
            // dGShowlist
            // 
            this.dGShowlist.AllowUserToAddRows = false;
            this.dGShowlist.AllowUserToDeleteRows = false;
            this.dGShowlist.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dGShowlist.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.选});
            this.dGShowlist.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dGShowlist.Location = new System.Drawing.Point(0, 0);
            this.dGShowlist.MultiSelect = false;
            this.dGShowlist.Name = "dGShowlist";
            this.dGShowlist.ReadOnly = true;
            this.dGShowlist.RowHeadersWidth = 21;
            this.dGShowlist.RowTemplate.Height = 23;
            this.dGShowlist.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dGShowlist.Size = new System.Drawing.Size(503, 255);
            this.dGShowlist.TabIndex = 5;
            this.dGShowlist.DoubleClick += new System.EventHandler(this.dGShowlist_DoubleClick);
            this.dGShowlist.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dGShowlist_CellClick);
            this.dGShowlist.CurrentCellChanged += new System.EventHandler(this.dGShowlist_CurrentCellChanged);
            this.dGShowlist.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dGShowlist_KeyUp);
            // 
            // 选
            // 
            this.选.HeaderText = "选";
            this.选.Name = "选";
            this.选.ReadOnly = true;
            this.选.Width = 35;
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(435, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(56, 23);
            this.btnClear.TabIndex = 2;
            this.btnClear.Text = "清空";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // FrmShowList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(503, 294);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Name = "FrmShowList";
            this.Text = "输入查找键选择查询项目（使用拼音码或五笔码查询信息）";
            this.Load += new System.EventHandler(this.FrmShowlist_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dGShowlist)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TextBox txtShowlist;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.DataGridView dGShowlist;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridViewCheckBoxColumn 选;
        private System.Windows.Forms.Button btnClear;
    }
}