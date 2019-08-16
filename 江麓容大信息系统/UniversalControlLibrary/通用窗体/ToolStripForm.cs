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
    public partial class ToolStripForm : Form
    {
        BasicFormTool parentForm;

        public ToolStripForm(BasicFormTool frm)
        {
            InitializeComponent();
            parentForm = frm;
        }

        private void ToolStripForm_Load(object sender, EventArgs e)
        {
            Point p = new Point();

            p.Y = parentForm.Location.Y + 100;
            p.X = parentForm.Location.X + parentForm.Size.Width - 10;

            this.Location = p;
        }

        private void btnPrevious_Click(object sender, EventArgs e)
        {
            if (parentForm.BusinessView != null && parentForm.BusinessView.Rows.Count > 0)
            {
                int indexOf = parentForm.BusinessView.Rows.IndexOf(parentForm.BusinessView_Row);

                if (indexOf == 0)
                {
                    MessageBox.Show("已经是【最顶端记录】");
                    return;
                }
                else
                {
                    parentForm.BusinessView_Row = parentForm.BusinessView.Rows[indexOf - 1];
                    parentForm.LoadFormInfo();
                }
            }
            else
            {
                int indexOf = parentForm.BusinessList.IndexOf(parentForm.BusinessList_Object);

                if (indexOf == 0)
                {
                    MessageBox.Show("已经是【最顶端记录】");
                    return;
                }
                else
                {
                    parentForm.BusinessList_Object = parentForm.BusinessList[indexOf - 1];
                    parentForm.LoadFormInfo();
                }
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (parentForm.BusinessView != null && parentForm.BusinessView.Rows.Count > 0)
            {
                int indexOf = parentForm.BusinessView.Rows.IndexOf(parentForm.BusinessView_Row);

                if (indexOf == parentForm.BusinessView.Rows.Count - 1)
                {
                    MessageBox.Show("已经是【最末端记录】");
                    return;
                }
                else
                {
                    parentForm.BusinessView_Row = parentForm.BusinessView.Rows[indexOf + 1];
                    parentForm.LoadFormInfo();
                }
            }
            else
            {
                int indexOf = parentForm.BusinessList.IndexOf(parentForm.BusinessList_Object);

                if (indexOf == parentForm.BusinessList.Count - 1)
                {
                    MessageBox.Show("已经是【最末端记录】");
                    return;
                }
                else
                {
                    parentForm.BusinessList_Object = parentForm.BusinessList[indexOf + 1];
                    parentForm.LoadFormInfo();
                }
            }
        }
    }
}
