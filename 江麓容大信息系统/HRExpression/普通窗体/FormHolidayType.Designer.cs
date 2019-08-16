using UniversalControlLibrary;
namespace Form_Peripheral_HR
{
    partial class FormHolidayType
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.添加toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.修改toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.删除toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.userControlDataLocalizer1 = new UserControlDataLocalizer();
            this.txtRemark = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTypeName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.rbIsWeekend = new System.Windows.Forms.RadioButton();
            this.rbIsLegalHolidays = new System.Windows.Forms.RadioButton();
            this.rbRegular = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.添加toolStripButton1,
            this.修改toolStripButton2,
            this.删除toolStripButton3});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(556, 25);
            this.toolStrip1.TabIndex = 1;
            // 
            // 添加toolStripButton1
            // 
            this.添加toolStripButton1.Image = global::UniversalControlLibrary.Properties.Resources.greentick;
            this.添加toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.添加toolStripButton1.Name = "添加toolStripButton1";
            this.添加toolStripButton1.Size = new System.Drawing.Size(49, 22);
            this.添加toolStripButton1.Text = "添加";
            this.添加toolStripButton1.Click += new System.EventHandler(this.添加toolStripButton1_Click);
            // 
            // 修改toolStripButton2
            // 
            this.修改toolStripButton2.Image = global::UniversalControlLibrary.Properties.Resources.modification;
            this.修改toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.修改toolStripButton2.Name = "修改toolStripButton2";
            this.修改toolStripButton2.Size = new System.Drawing.Size(49, 22);
            this.修改toolStripButton2.Tag = "update";
            this.修改toolStripButton2.Text = "修改";
            this.修改toolStripButton2.Click += new System.EventHandler(this.修改toolStripButton2_Click);
            // 
            // 删除toolStripButton3
            // 
            this.删除toolStripButton3.Image = global::UniversalControlLibrary.Properties.Resources.delete;
            this.删除toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.删除toolStripButton3.Name = "删除toolStripButton3";
            this.删除toolStripButton3.Size = new System.Drawing.Size(49, 22);
            this.删除toolStripButton3.Tag = "delete";
            this.删除toolStripButton3.Text = "删除";
            this.删除toolStripButton3.Click += new System.EventHandler(this.删除toolStripButton3_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.rbRegular);
            this.panel1.Controls.Add(this.rbIsLegalHolidays);
            this.panel1.Controls.Add(this.rbIsWeekend);
            this.panel1.Controls.Add(this.userControlDataLocalizer1);
            this.panel1.Controls.Add(this.txtRemark);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtTypeName);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 25);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(556, 177);
            this.panel1.TabIndex = 2;
            // 
            // userControlDataLocalizer1
            // 
            this.userControlDataLocalizer1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.userControlDataLocalizer1.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.userControlDataLocalizer1.Location = new System.Drawing.Point(0, 145);
            this.userControlDataLocalizer1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.userControlDataLocalizer1.Name = "userControlDataLocalizer1";
            this.userControlDataLocalizer1.OnlyLocalize = false;
            this.userControlDataLocalizer1.Size = new System.Drawing.Size(556, 32);
            this.userControlDataLocalizer1.StartIndex = 0;
            this.userControlDataLocalizer1.TabIndex = 9;
            // 
            // txtRemark
            // 
            this.txtRemark.Location = new System.Drawing.Point(172, 82);
            this.txtRemark.Multiline = true;
            this.txtRemark.Name = "txtRemark";
            this.txtRemark.Size = new System.Drawing.Size(300, 56);
            this.txtRemark.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Blue;
            this.label2.Location = new System.Drawing.Point(113, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 4;
            this.label2.Text = "备  注";
            // 
            // txtTypeName
            // 
            this.txtTypeName.Location = new System.Drawing.Point(172, 15);
            this.txtTypeName.Name = "txtTypeName";
            this.txtTypeName.Size = new System.Drawing.Size(300, 23);
            this.txtTypeName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.Color.Blue;
            this.label1.Location = new System.Drawing.Point(85, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 0;
            this.label1.Text = "节假日名称";
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 202);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowTemplate.Height = 23;
            this.dataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridView1.Size = new System.Drawing.Size(556, 257);
            this.dataGridView1.TabIndex = 3;
            this.dataGridView1.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellEnter);
            // 
            // rbIsWeekend
            // 
            this.rbIsWeekend.AutoSize = true;
            this.rbIsWeekend.ForeColor = System.Drawing.Color.Blue;
            this.rbIsWeekend.Location = new System.Drawing.Point(174, 50);
            this.rbIsWeekend.Name = "rbIsWeekend";
            this.rbIsWeekend.Size = new System.Drawing.Size(53, 18);
            this.rbIsWeekend.TabIndex = 10;
            this.rbIsWeekend.TabStop = true;
            this.rbIsWeekend.Text = "周末";
            this.rbIsWeekend.UseVisualStyleBackColor = true;
            // 
            // rbIsLegalHolidays
            // 
            this.rbIsLegalHolidays.AutoSize = true;
            this.rbIsLegalHolidays.ForeColor = System.Drawing.Color.Blue;
            this.rbIsLegalHolidays.Location = new System.Drawing.Point(261, 50);
            this.rbIsLegalHolidays.Name = "rbIsLegalHolidays";
            this.rbIsLegalHolidays.Size = new System.Drawing.Size(95, 18);
            this.rbIsLegalHolidays.TabIndex = 11;
            this.rbIsLegalHolidays.TabStop = true;
            this.rbIsLegalHolidays.Text = "法定节假日";
            this.rbIsLegalHolidays.UseVisualStyleBackColor = true;
            // 
            // rbRegular
            // 
            this.rbRegular.AutoSize = true;
            this.rbRegular.ForeColor = System.Drawing.Color.Blue;
            this.rbRegular.Location = new System.Drawing.Point(396, 50);
            this.rbRegular.Name = "rbRegular";
            this.rbRegular.Size = new System.Drawing.Size(67, 18);
            this.rbRegular.TabIndex = 12;
            this.rbRegular.TabStop = true;
            this.rbRegular.Text = "工作日";
            this.rbRegular.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.Color.Blue;
            this.label3.Location = new System.Drawing.Point(85, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 13;
            this.label3.Text = "节假日类型";
            // 
            // FormHolidayType
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(556, 459);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toolStrip1);
            this.Font = new System.Drawing.Font("宋体", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximizeBox = false;
            this.Name = "FormHolidayType";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "设置节假日类别";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton 添加toolStripButton1;
        private System.Windows.Forms.ToolStripButton 修改toolStripButton2;
        private System.Windows.Forms.ToolStripButton 删除toolStripButton3;
        private System.Windows.Forms.Panel panel1;
        private UserControlDataLocalizer userControlDataLocalizer1;
        private System.Windows.Forms.TextBox txtRemark;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTypeName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.RadioButton rbRegular;
        private System.Windows.Forms.RadioButton rbIsLegalHolidays;
        private System.Windows.Forms.RadioButton rbIsWeekend;
        private System.Windows.Forms.Label label3;

    }
}