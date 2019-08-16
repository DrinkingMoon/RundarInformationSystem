using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using mshtml;

namespace UniversalControlLibrary
{
    public partial class 其他文件显示 : Form
    {
        public 其他文件显示(string str)
        {
            InitializeComponent();

            webBrowser1.Navigate(@str);
        }
    }
}
