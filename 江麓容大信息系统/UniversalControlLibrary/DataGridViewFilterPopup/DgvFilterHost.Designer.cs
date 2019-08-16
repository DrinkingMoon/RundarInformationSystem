namespace UniversalControlLibrary
{
    partial class DgvFilterHost
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DgvFilterHost));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsRemove = new System.Windows.Forms.ToolStripButton();
            this.tsOK = new System.Windows.Forms.ToolStripButton();
            this.tsRemoveAll = new System.Windows.Forms.ToolStripButton();
            this.panel1 = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.lblColumnName = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panelFilterArea = new System.Windows.Forms.Panel();
            this.btnAddFilter = new System.Windows.Forms.Button();
            this.panelFilterText = new System.Windows.Forms.Panel();
            this.comboBox_Cols = new System.Windows.Forms.ComboBox();
            this.otooltip = new System.Windows.Forms.ToolTip(this.components);
            this.toolStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panelFilterArea.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.Transparent;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsRemove,
            this.tsOK,
            this.tsRemoveAll});
            this.toolStrip1.Location = new System.Drawing.Point(0, 259);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(471, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsRemove
            // 
            this.tsRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRemove.Image = global::UniversalControlLibrary.Properties.Resources.Delete_16x161;
            this.tsRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRemove.Margin = new System.Windows.Forms.Padding(5, 1, 0, 2);
            this.tsRemove.Name = "tsRemove";
            this.tsRemove.Size = new System.Drawing.Size(23, 22);
            this.tsRemove.Text = "Remove filter";
            this.tsRemove.ToolTipText = "Remove Filter";
            this.tsRemove.Visible = false;
            this.tsRemove.Click += new System.EventHandler(this.tsRemove_Click);
            // 
            // tsOK
            // 
            this.tsOK.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.tsOK.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsOK.Image = ((System.Drawing.Image)(resources.GetObject("tsOK.Image")));
            this.tsOK.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsOK.Name = "tsOK";
            this.tsOK.Size = new System.Drawing.Size(23, 22);
            this.tsOK.Text = "OK";
            this.tsOK.Click += new System.EventHandler(this.tsOK_Click);
            // 
            // tsRemoveAll
            // 
            this.tsRemoveAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRemoveAll.Image = ((System.Drawing.Image)(resources.GetObject("tsRemoveAll.Image")));
            this.tsRemoveAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRemoveAll.Margin = new System.Windows.Forms.Padding(0);
            this.tsRemoveAll.Name = "tsRemoveAll";
            this.tsRemoveAll.Size = new System.Drawing.Size(23, 25);
            this.tsRemoveAll.Text = "Remove all";
            this.tsRemoveAll.Click += new System.EventHandler(this.tsRemoveAll_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.Gainsboro;
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.lblColumnName);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Location = new System.Drawing.Point(-9, -1);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(481, 22);
            this.panel1.TabIndex = 10;
            this.panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseMove);
            this.panel1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panel1_MouseDown);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(455, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(16, 16);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Tag = "";
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // lblColumnName
            // 
            this.lblColumnName.AutoSize = true;
            this.lblColumnName.BackColor = System.Drawing.Color.Transparent;
            this.lblColumnName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblColumnName.Location = new System.Drawing.Point(34, 5);
            this.lblColumnName.Name = "lblColumnName";
            this.lblColumnName.Size = new System.Drawing.Size(84, 13);
            this.lblColumnName.TabIndex = 12;
            this.lblColumnName.Text = "Column Name";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(16, 16);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 10;
            this.pictureBox1.TabStop = false;
            // 
            // panelFilterArea
            // 
            this.panelFilterArea.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFilterArea.Controls.Add(this.btnAddFilter);
            this.panelFilterArea.Controls.Add(this.panelFilterText);
            this.panelFilterArea.Controls.Add(this.comboBox_Cols);
            this.panelFilterArea.Location = new System.Drawing.Point(3, 24);
            this.panelFilterArea.Name = "panelFilterArea";
            this.panelFilterArea.Size = new System.Drawing.Size(465, 234);
            this.panelFilterArea.TabIndex = 11;
            // 
            // btnAddFilter
            // 
            this.btnAddFilter.Location = new System.Drawing.Point(393, 23);
            this.btnAddFilter.Name = "btnAddFilter";
            this.btnAddFilter.Size = new System.Drawing.Size(64, 23);
            this.btnAddFilter.TabIndex = 3;
            this.btnAddFilter.Text = "ÐÂÔö¹ýÂË";
            this.btnAddFilter.UseVisualStyleBackColor = true;
            this.btnAddFilter.Click += new System.EventHandler(this.pictureBox3_Click);
            // 
            // panelFilterText
            // 
            this.panelFilterText.AutoScroll = true;
            this.panelFilterText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelFilterText.Location = new System.Drawing.Point(7, 74);
            this.panelFilterText.Name = "panelFilterText";
            this.panelFilterText.Size = new System.Drawing.Size(450, 144);
            this.panelFilterText.TabIndex = 2;
            // 
            // comboBox_Cols
            // 
            this.comboBox_Cols.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.comboBox_Cols.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_Cols.FormattingEnabled = true;
            this.comboBox_Cols.Location = new System.Drawing.Point(10, 24);
            this.comboBox_Cols.Name = "comboBox_Cols";
            this.comboBox_Cols.Size = new System.Drawing.Size(135, 20);
            this.comboBox_Cols.TabIndex = 0;
            // 
            // DgvFilterHost
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.panelFilterArea);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panel1);
            this.Name = "DgvFilterHost";
            this.Size = new System.Drawing.Size(471, 284);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panelFilterArea.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsOK;
        private System.Windows.Forms.ToolStripButton tsRemoveAll;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblColumnName;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripButton tsRemove;
        private System.Windows.Forms.Panel panelFilterArea;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.ComboBox comboBox_Cols;
        //private System.Windows.Forms.Label label_filtertext;
        private System.Windows.Forms.Panel panelFilterText;
        private System.Windows.Forms.ToolTip otooltip;
        private System.Windows.Forms.Button btnAddFilter;
    }
}
