using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace UniversalControlLibrary
{
    public partial class ToolStripDropDownFilter : ToolStripDropDown
    {
        public ToolStripDropDownFilter()
        {
            InitializeComponent();
        }

        private Point m_MousePoint;
        private Point m_LastPoint;

        public void Mouse_Down(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.m_LastPoint = this.Location;
            this.m_MousePoint = this.PointToScreen(e.Location);
        }

        public void Mouse_Move(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                Point t = this.PointToScreen(e.Location);
                Point l = this.m_LastPoint;

                l.Offset(t.X - this.m_MousePoint.X, t.Y - this.m_MousePoint.Y);
                this.Location = l;
            }
        }
    }
}
