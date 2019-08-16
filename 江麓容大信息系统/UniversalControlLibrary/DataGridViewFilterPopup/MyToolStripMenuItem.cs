using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Text;
using System.Windows.Forms;

namespace UniversalControlLibrary
{
    public partial class MyToolStripMenuItem : ToolStripMenuItem
    {
        public MyToolStripMenuItem()
        {
            InitializeComponent();
        }

        public MyToolStripMenuItem(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        public MyToolStripMenuItem(string text, System.Drawing.Image image, EventHandler handle)
            : base(text, image, handle)
        { 
             
        }

        //额外加的排序字段

        public DgvBaseColumnFilter DgvBaseColumn { set; get; }

        public ListSortDirection SortDirection { set; get; }
    }
}
