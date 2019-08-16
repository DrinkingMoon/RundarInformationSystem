using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Expression
{
    public partial class 登陆界面温馨提示 : Form
    {
        public 登陆界面温馨提示(string loginNotice)
        {
            InitializeComponent();
            textBox1.Text = loginNotice;
            checkBox1.Focus();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            button1.Enabled = checkBox1.Checked;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
