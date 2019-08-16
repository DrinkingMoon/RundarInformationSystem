using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace UniversalControlLibrary
{
    public partial class WaitingForm : Form
    {
        public WaitingForm()
        {
            InitializeComponent();
        }

        private void WaitingForm_Load(object sender, EventArgs e)
        {
            TransparencyKey = this.BackColor;

            label1.BackColor = this.BackColor;
        }
    }
}
