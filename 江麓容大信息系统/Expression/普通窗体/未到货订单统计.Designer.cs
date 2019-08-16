namespace Expression
{
    partial class 未到货订单统计
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(未到货订单统计));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSpec = new System.Windows.Forms.TextBox();
            this.txtGoodsName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new UniversalControlLibrary.TextBoxShow();
            this.txtProvider = new UniversalControlLibrary.TextBoxShow();
            this.customDataGridView1 = new UniversalControlLibrary.CustomDataGridView();
            this.供应商 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.创建日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.图号型号 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.物品名称 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.规格 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.要求到货日期 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.订单数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.已到货数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.未到货数量 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.状态 = new System.Windows.Forms.DataGridViewButtonColumn();
            this.物品ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.checkBillDateAndStatus1 = new UniversalControlLibrary.CheckBillDateAndStatus();
            this.btnClear = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(11, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 363;
            this.label1.Text = "供 应 商";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(325, 85);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 365;
            this.label2.Text = "物品名称";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(642, 85);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 12);
            this.label3.TabIndex = 367;
            this.label3.Text = "规格";
            // 
            // txtSpec
            // 
            this.txtSpec.Location = new System.Drawing.Point(702, 81);
            this.txtSpec.Name = "txtSpec";
            this.txtSpec.Size = new System.Drawing.Size(202, 21);
            this.txtSpec.TabIndex = 368;
            // 
            // txtGoodsName
            // 
            this.txtGoodsName.Location = new System.Drawing.Point(384, 81);
            this.txtGoodsName.Name = "txtGoodsName";
            this.txtGoodsName.Size = new System.Drawing.Size(235, 21);
            this.txtGoodsName.TabIndex = 370;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(12, 85);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 369;
            this.label4.Text = "图号型号";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.DataResult = null;
            this.txtGoodsCode.EditingControlDataGridView = null;
            this.txtGoodsCode.EditingControlFormattedValue = "";
            this.txtGoodsCode.EditingControlRowIndex = 0;
            this.txtGoodsCode.EditingControlValueChanged = true;
            this.txtGoodsCode.FindItem = UniversalControlLibrary.TextBoxShow.FindType.所有物品;
            this.txtGoodsCode.Location = new System.Drawing.Point(70, 81);
            this.txtGoodsCode.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.ShowResultForm = true;
            this.txtGoodsCode.Size = new System.Drawing.Size(236, 21);
            this.txtGoodsCode.StrEndSql = null;
            this.txtGoodsCode.TabIndex = 366;
            this.txtGoodsCode.TabStop = false;
            this.txtGoodsCode.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtGoodsCode_OnCompleteSearch);
            // 
            // txtProvider
            // 
            this.txtProvider.DataResult = null;
            this.txtProvider.EditingControlDataGridView = null;
            this.txtProvider.EditingControlFormattedValue = "";
            this.txtProvider.EditingControlRowIndex = 0;
            this.txtProvider.EditingControlValueChanged = true;
            this.txtProvider.FindItem = UniversalControlLibrary.TextBoxShow.FindType.供应商;
            this.txtProvider.Location = new System.Drawing.Point(70, 49);
            this.txtProvider.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.txtProvider.Name = "txtProvider";
            this.txtProvider.ShowResultForm = true;
            this.txtProvider.Size = new System.Drawing.Size(236, 21);
            this.txtProvider.StrEndSql = null;
            this.txtProvider.TabIndex = 364;
            this.txtProvider.TabStop = false;
            this.txtProvider.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.txtProvider_OnCompleteSearch);
            // 
            // customDataGridView1
            // 
            this.customDataGridView1.AllowUserToAddRows = false;
            this.customDataGridView1.AllowUserToDeleteRows = false;
            this.customDataGridView1.AllowUserToResizeRows = false;
            this.customDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.customDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.供应商,
            this.订单号,
            this.创建日期,
            this.图号型号,
            this.物品名称,
            this.规格,
            this.要求到货日期,
            this.订单数量,
            this.已到货数量,
            this.未到货数量,
            this.状态,
            this.物品ID});
            this.customDataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.customDataGridView1.Location = new System.Drawing.Point(0, 114);
            this.customDataGridView1.Name = "customDataGridView1";
            this.customDataGridView1.RowTemplate.Height = 23;
            this.customDataGridView1.Size = new System.Drawing.Size(1148, 622);
            this.customDataGridView1.TabIndex = 1;
            this.customDataGridView1.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.customDataGridView1_CellClick);
            // 
            // 供应商
            // 
            this.供应商.DataPropertyName = "供应商";
            this.供应商.HeaderText = "供应商";
            this.供应商.Name = "供应商";
            this.供应商.ReadOnly = true;
            // 
            // 订单号
            // 
            this.订单号.DataPropertyName = "订单号";
            this.订单号.HeaderText = "订单号";
            this.订单号.Name = "订单号";
            this.订单号.ReadOnly = true;
            // 
            // 创建日期
            // 
            this.创建日期.DataPropertyName = "创建日期";
            this.创建日期.HeaderText = "创建日期";
            this.创建日期.Name = "创建日期";
            this.创建日期.ReadOnly = true;
            // 
            // 图号型号
            // 
            this.图号型号.DataPropertyName = "图号型号";
            this.图号型号.HeaderText = "图号型号";
            this.图号型号.Name = "图号型号";
            this.图号型号.ReadOnly = true;
            // 
            // 物品名称
            // 
            this.物品名称.DataPropertyName = "物品名称";
            this.物品名称.HeaderText = "物品名称";
            this.物品名称.Name = "物品名称";
            this.物品名称.ReadOnly = true;
            // 
            // 规格
            // 
            this.规格.DataPropertyName = "规格";
            this.规格.HeaderText = "规格";
            this.规格.Name = "规格";
            this.规格.ReadOnly = true;
            // 
            // 要求到货日期
            // 
            this.要求到货日期.DataPropertyName = "要求到货日期";
            this.要求到货日期.HeaderText = "要求到货日期";
            this.要求到货日期.Name = "要求到货日期";
            this.要求到货日期.ReadOnly = true;
            // 
            // 订单数量
            // 
            this.订单数量.DataPropertyName = "订单数量";
            this.订单数量.HeaderText = "订单数量";
            this.订单数量.Name = "订单数量";
            this.订单数量.ReadOnly = true;
            // 
            // 已到货数量
            // 
            this.已到货数量.DataPropertyName = "已到货数量";
            this.已到货数量.HeaderText = "已到货数量";
            this.已到货数量.Name = "已到货数量";
            this.已到货数量.ReadOnly = true;
            // 
            // 未到货数量
            // 
            this.未到货数量.DataPropertyName = "未到货数量";
            this.未到货数量.HeaderText = "未到货数量";
            this.未到货数量.Name = "未到货数量";
            this.未到货数量.ReadOnly = true;
            // 
            // 状态
            // 
            this.状态.DataPropertyName = "状态";
            this.状态.HeaderText = "状态";
            this.状态.Name = "状态";
            // 
            // 物品ID
            // 
            this.物品ID.DataPropertyName = "物品ID";
            this.物品ID.HeaderText = "物品ID";
            this.物品ID.Name = "物品ID";
            this.物品ID.ReadOnly = true;
            this.物品ID.Visible = false;
            // 
            // checkBillDateAndStatus1
            // 
            this.checkBillDateAndStatus1.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBillDateAndStatus1.ListBillStatus = ((System.Collections.Generic.List<string>)(resources.GetObject("checkBillDateAndStatus1.ListBillStatus")));
            this.checkBillDateAndStatus1.Location = new System.Drawing.Point(0, 0);
            this.checkBillDateAndStatus1.MultiVisible = false;
            this.checkBillDateAndStatus1.Name = "checkBillDateAndStatus1";
            this.checkBillDateAndStatus1.Size = new System.Drawing.Size(1148, 114);
            this.checkBillDateAndStatus1.StatusVisible = true;
            this.checkBillDateAndStatus1.TabIndex = 0;
            this.checkBillDateAndStatus1.OnCompleteSearch += new GlobalObject.DelegateCollection.NonArgumentHandle(this.checkBillDateAndStatus1_OnCompleteSearch);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(791, 52);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(113, 23);
            this.btnClear.TabIndex = 371;
            this.btnClear.Text = "清空查询条件";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // 未到货订单统计
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1148, 736);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.txtGoodsName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtSpec);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtGoodsCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtProvider);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.customDataGridView1);
            this.Controls.Add(this.checkBillDateAndStatus1);
            this.Name = "未到货订单统计";
            this.Text = "未到货订单统计";
            ((System.ComponentModel.ISupportInitialize)(this.customDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private UniversalControlLibrary.CheckBillDateAndStatus checkBillDateAndStatus1;
        private UniversalControlLibrary.CustomDataGridView customDataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn 供应商;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 创建日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 图号型号;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品名称;
        private System.Windows.Forms.DataGridViewTextBoxColumn 规格;
        private System.Windows.Forms.DataGridViewTextBoxColumn 要求到货日期;
        private System.Windows.Forms.DataGridViewTextBoxColumn 订单数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 已到货数量;
        private System.Windows.Forms.DataGridViewTextBoxColumn 未到货数量;
        private System.Windows.Forms.DataGridViewButtonColumn 状态;
        private System.Windows.Forms.DataGridViewTextBoxColumn 物品ID;
        private System.Windows.Forms.Label label1;
        private UniversalControlLibrary.TextBoxShow txtProvider;
        private UniversalControlLibrary.TextBoxShow txtGoodsCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSpec;
        private System.Windows.Forms.TextBox txtGoodsName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClear;
    }
}