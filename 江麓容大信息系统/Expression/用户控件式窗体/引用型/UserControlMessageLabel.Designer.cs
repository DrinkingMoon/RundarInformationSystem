namespace Expression
{
    partial class UserControlMessageLabel
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
            this.components = new System.ComponentModel.Container();
            this.lblMsg = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.lblLine = new System.Windows.Forms.Label();
            this.panelRight = new System.Windows.Forms.Panel();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblName = new System.Windows.Forms.Label();
            this.panelRight.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMsg
            // 
            this.lblMsg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMsg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblMsg.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblMsg.ImageAlign = System.Drawing.ContentAlignment.TopLeft;
            this.lblMsg.Location = new System.Drawing.Point(0, 0);
            this.lblMsg.Name = "lblMsg";
            this.lblMsg.Size = new System.Drawing.Size(258, 40);
            this.lblMsg.TabIndex = 0;
            this.lblMsg.Text = "报检入库单 BJD20101011\r\n等待您的处理，涉及零件：主动带轮轴调整垫片";
            this.lblMsg.MouseLeave += new System.EventHandler(this.lblMsg_MouseLeave);
            this.lblMsg.Click += new System.EventHandler(this.lblMsg_Click);
            this.lblMsg.MouseHover += new System.EventHandler(this.lblMsg_MouseHover);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 3000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            // 
            // lblLine
            // 
            this.lblLine.BackColor = System.Drawing.Color.Transparent;
            this.lblLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lblLine.ForeColor = System.Drawing.Color.Transparent;
            this.lblLine.Image = global::UniversalControlLibrary.Properties.Resources.Line;
            this.lblLine.Location = new System.Drawing.Point(0, 40);
            this.lblLine.Name = "lblLine";
            this.lblLine.Size = new System.Drawing.Size(340, 6);
            this.lblLine.TabIndex = 1;
            // 
            // panelRight
            // 
            this.panelRight.Controls.Add(this.lblTime);
            this.panelRight.Controls.Add(this.lblName);
            this.panelRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelRight.Location = new System.Drawing.Point(258, 0);
            this.panelRight.Name = "panelRight";
            this.panelRight.Size = new System.Drawing.Size(82, 40);
            this.panelRight.TabIndex = 2;
            // 
            // lblTime
            // 
            this.lblTime.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTime.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblTime.Location = new System.Drawing.Point(3, 20);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(78, 15);
            this.lblTime.TabIndex = 1;
            this.lblTime.Text = "11-11 22：22";
            this.lblTime.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblName
            // 
            this.lblName.Font = new System.Drawing.Font("新宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblName.ForeColor = System.Drawing.SystemColors.MenuHighlight;
            this.lblName.Location = new System.Drawing.Point(3, 3);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(78, 14);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // UserControlMessageLabel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::UniversalControlLibrary.Properties.Resources.MessageBackImage;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Controls.Add(this.lblMsg);
            this.Controls.Add(this.panelRight);
            this.Controls.Add(this.lblLine);
            this.Font = new System.Drawing.Font("新宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "UserControlMessageLabel";
            this.Size = new System.Drawing.Size(340, 46);
            this.panelRight.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMsg;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Label lblLine;
        private System.Windows.Forms.Panel panelRight;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Label lblTime;
    }
}
