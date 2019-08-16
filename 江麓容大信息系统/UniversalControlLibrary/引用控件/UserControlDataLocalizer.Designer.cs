namespace UniversalControlLibrary
{
    partial class UserControlDataLocalizer
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
            this.toolStripFuzzyFind = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cmbFindItem = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.txtContext = new System.Windows.Forms.ToolStripTextBox();
            this.btnFindData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnSetHideFields = new System.Windows.Forms.ToolStripButton();
            this.toolStripFuzzyFind.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripFuzzyFind
            // 
            this.toolStripFuzzyFind.AutoSize = false;
            this.toolStripFuzzyFind.BackColor = System.Drawing.Color.Transparent;
            this.toolStripFuzzyFind.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripFuzzyFind.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.toolStripFuzzyFind.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cmbFindItem,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.txtContext,
            this.btnFindData,
            this.toolStripSeparator2,
            this.btnSetHideFields});
            this.toolStripFuzzyFind.Location = new System.Drawing.Point(0, 4);
            this.toolStripFuzzyFind.Name = "toolStripFuzzyFind";
            this.toolStripFuzzyFind.Size = new System.Drawing.Size(746, 28);
            this.toolStripFuzzyFind.TabIndex = 17;
            this.toolStripFuzzyFind.Text = "toolStrip1";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(63, 25);
            this.toolStripLabel1.Text = "查找条件";
            // 
            // cmbFindItem
            // 
            this.cmbFindItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFindItem.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbFindItem.Name = "cmbFindItem";
            this.cmbFindItem.Size = new System.Drawing.Size(150, 28);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 28);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(98, 25);
            this.toolStripLabel2.Text = "   全部或部分";
            // 
            // txtContext
            // 
            this.txtContext.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtContext.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtContext.Name = "txtContext";
            this.txtContext.Size = new System.Drawing.Size(150, 28);
            this.txtContext.ToolTipText = "按回车键即可进行定位";
            this.txtContext.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtContext_KeyUp);
            this.txtContext.TextChanged += new System.EventHandler(this.txtContext_TextChanged);
            // 
            // btnFindData
            // 
            this.btnFindData.Image = global::UniversalControlLibrary.Properties.Resources.Search;
            this.btnFindData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnFindData.Name = "btnFindData";
            this.btnFindData.Size = new System.Drawing.Size(104, 25);
            this.btnFindData.Text = "数据定位(&G)";
            this.btnFindData.ToolTipText = "数据定位, 多次点击可以查找下一个符合条件的记录";
            this.btnFindData.Click += new System.EventHandler(this.btnFindData_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 28);
            // 
            // btnSetHideFields
            // 
            this.btnSetHideFields.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSetHideFields.Image = global::UniversalControlLibrary.Properties.Resources.提交;
            this.btnSetHideFields.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetHideFields.Name = "btnSetHideFields";
            this.btnSetHideFields.Size = new System.Drawing.Size(132, 25);
            this.btnSetHideFields.Text = "设置隐藏字段(&S)";
            this.btnSetHideFields.Click += new System.EventHandler(this.btnSetHideFields_Click);
            // 
            // UserControlDataLocalizer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStripFuzzyFind);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UserControlDataLocalizer";
            this.Size = new System.Drawing.Size(746, 32);
            this.toolStripFuzzyFind.ResumeLayout(false);
            this.toolStripFuzzyFind.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripFuzzyFind;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        public System.Windows.Forms.ToolStripTextBox txtContext;
        private System.Windows.Forms.ToolStripButton btnFindData;
        private System.Windows.Forms.ToolStripButton btnSetHideFields;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        public System.Windows.Forms.ToolStripComboBox cmbFindItem;
    }
}
