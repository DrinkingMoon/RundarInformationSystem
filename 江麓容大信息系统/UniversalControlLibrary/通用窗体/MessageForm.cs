using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    public partial class MessageForm : Form
    {
        public MessageForm(string msg)
        {
            InitializeComponent();

            if (msg != null && msg.Trim().Length != 0)
            {
                label1.Text = "正在" + msg + "，请稍后......";
            }
        }
    }
}
