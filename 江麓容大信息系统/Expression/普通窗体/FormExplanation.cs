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
    public partial class FormExplanation : Form
    {
        /// <summary>
        /// 原因说明
        /// </summary>
        private string m_explanation = "";

        public string Explanation
        {
            get { return m_explanation; }
            set { m_explanation = value; }
        }

        public FormExplanation()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (txtExplanation.Text.Trim() == "")
            {
                MessageDialog.ShowPromptMessage("请填写原因说明！");
                return;
            }

            m_explanation = txtExplanation.Text;

            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
