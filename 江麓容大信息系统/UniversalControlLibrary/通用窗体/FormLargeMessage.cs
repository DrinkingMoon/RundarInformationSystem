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
    public partial class FormLargeMessage : Form
    {
        public string Message
        {
            get { return rtbMessage.Text; }
            set { rtbMessage.Text = value; }
        }

        public Color MessageColor
        {
            get { return rtbMessage.ForeColor; }
            set { rtbMessage.ForeColor = value; }
        }

        public FormLargeMessage()
        {
            InitializeComponent();
        }

        public FormLargeMessage(string msg, Color msgColor)
        {
            InitializeComponent();

            rtbMessage.Text = msg;
            rtbMessage.ForeColor = msgColor;
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {
            rtbMessage.SelectAll();
            rtbMessage.Copy();
            rtbMessage.SelectionLength = 0;

            MessageDialog.ShowPromptMessage("复制成功，您可将其粘贴到其他编辑器中。");
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName.Length > 0)
            {
                if (saveFileDialog1.FileName.Substring(saveFileDialog1.FileName.Length - 3) == "txt")
                {
                    rtbMessage.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.TextTextOleObjs);
                }
                else
                {
                    rtbMessage.SaveFile(saveFileDialog1.FileName);
                }
                
                MessageDialog.ShowPromptMessage("成功保存文件。");
            }
        }
    }
}
