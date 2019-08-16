using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;

namespace Expression
{
    public partial class FormVirtualPartInfo : Form
    {
        /// <summary>
        /// 获取零件图号
        /// </summary>
        public string PartCode
        {
            get { return txtCode.Text; }
        }

        /// <summary>
        /// 获取零件名称
        /// </summary>
        public string PartName
        {
            get { return txtName.Text; }
        }

        public FormVirtualPartInfo()
        {
            InitializeComponent();
        }

        private bool CheckDataItem()
        {
            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtCode.Text))
            {
                MessageDialog.ShowPromptMessage("零件图号不允许为空");
                txtCode.Focus();
                return false;
            }

            if (txtCode.Text.Length < 5 || txtCode.Text.Substring(0, 4) != "VIR_")
            {
                MessageDialog.ShowPromptMessage("零件图号格式不正确");
                txtCode.Focus();
                return false;
            }

            if (GlobalObject.GeneralFunction.IsNullOrEmpty(txtName.Text))
            {
                MessageDialog.ShowPromptMessage("零件名称不允许为空");
                txtName.Focus();
                return false;
            }

            return true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            if (!CheckDataItem())
            {
                return;
            }

            this.DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }
    }
}
