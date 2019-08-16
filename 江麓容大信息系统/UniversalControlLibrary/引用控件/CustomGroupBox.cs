using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GlobalObject;

namespace UniversalControlLibrary
{
    public partial class CustomGroupBox : GroupBox
    {
        public CustomGroupBox()
        {
            InitializeComponent();
        }

        public CustomGroupBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private void CustomGroupBox_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.Clear(this.BackColor);
            e.Graphics.DrawString(this.Text, this.Font, Brushes.Black, 10, 1);
            e.Graphics.DrawLine(Pens.Black, 1, 7, 8, 7);
            e.Graphics.DrawLine(Pens.Black, e.Graphics.MeasureString(this.Text, this.Font).Width + 8, 7, this.Width - 2, 7);
            e.Graphics.DrawLine(Pens.Black, 1, 7, 1, this.Height - 2);
            e.Graphics.DrawLine(Pens.Black, 1, this.Height - 2, this.Width - 2, this.Height - 2);
            e.Graphics.DrawLine(Pens.Black, this.Width - 2, 7, this.Width - 2, this.Height - 2); 
        }
    }
}
