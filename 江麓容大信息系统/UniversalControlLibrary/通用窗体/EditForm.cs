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
    public partial class EditForm : Form
    {
        private string _textString;

        public string TextString
        {
            get { return _textString; }
            set { _textString = value; }
        }

        Point _ptLt = new Point();

        public EditForm(string textString, Point ptLocation)
        {
            InitializeComponent();

            _textString = textString;
            _ptLt = ptLocation;

            if (_ptLt.X + this.Width > Screen.PrimaryScreen.Bounds.Width)
            {
                _ptLt.X = Screen.PrimaryScreen.Bounds.Width - this.Width;
            }

            if (_ptLt.Y + this.Height > Screen.PrimaryScreen.Bounds.Height)
            {
                _ptLt.Y = Screen.PrimaryScreen.Bounds.Height - this.Height;
            }
        }

        private void EditForm_Load(object sender, EventArgs e)
        {
            richTextBox1.Text = _textString;
            this.Location = _ptLt;
        }

        private void EditForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _textString = richTextBox1.Text;
        }

        private void richTextBox1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
            }
        }
    }
}
