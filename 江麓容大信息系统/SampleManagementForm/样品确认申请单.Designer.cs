﻿using UniversalControlLibrary;

namespace Form_Project_Project
{
    partial class 样品确认申请单
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
            this.tabControl1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Size = new System.Drawing.Size(1016, 272);
            // 
            // tabPage1
            // 
            this.tabPage1.Size = new System.Drawing.Size(1008, 246);
            // 
            // 样品确认申请单
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1016, 435);
            this.Name = "样品确认申请单";
            this.Text = "样品确认申请单";
            this.Form_CommonProcessSubmit += new UniversalControlLibrary.FormCommonProcess.FormSubmit(this.样品确认申请单_Form_CommonProcessSubmit);
            this.tabControl1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

    }
}