namespace UniversalControlLibrary
{
    partial class FormViewData
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
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.数据名称1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数据值1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数据名称2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.数据值2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AllowUserToResizeRows = false;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.数据名称1,
            this.数据值1,
            this.数据名称2,
            this.数据值2});
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 25;
            this.dataGridView1.Size = new System.Drawing.Size(520, 221);
            this.dataGridView1.TabIndex = 0;
            // 
            // 数据名称1
            // 
            this.数据名称1.Frozen = true;
            this.数据名称1.HeaderText = "数据名称";
            this.数据名称1.Name = "数据名称1";
            this.数据名称1.ReadOnly = true;
            this.数据名称1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.数据名称1.Width = 120;
            // 
            // 数据值1
            // 
            this.数据值1.HeaderText = "数据值";
            this.数据值1.Name = "数据值1";
            this.数据值1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.数据值1.Width = 120;
            // 
            // 数据名称2
            // 
            this.数据名称2.HeaderText = "数据名称";
            this.数据名称2.Name = "数据名称2";
            this.数据名称2.ReadOnly = true;
            this.数据名称2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.数据名称2.Width = 120;
            // 
            // 数据值2
            // 
            this.数据值2.HeaderText = "数据值";
            this.数据值2.Name = "数据值2";
            this.数据值2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.数据值2.Width = 120;
            // 
            // FormViewData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(520, 221);
            this.Controls.Add(this.dataGridView1);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormViewData";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "数据预览";
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数据名称1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数据值1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数据名称2;
        private System.Windows.Forms.DataGridViewTextBoxColumn 数据值2;
    }
}