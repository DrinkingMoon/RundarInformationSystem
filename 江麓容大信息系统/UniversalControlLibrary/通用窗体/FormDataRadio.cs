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
    public partial class FormDataRadio : Form
    {

        public event GlobalObject.DelegateCollection.FormDataRodioDelegate OnFormDataRodioDelegate;

        /// <summary>
        /// 返回结果
        /// </summary>
        public string m_strResult = null;

        public FormDataRadio(List<string> source, bool rbClick)
        {
            InitializeComponent();

            panel1.Height = rbClick == true ? 0 : panel1.Height;

            int X = 30;

            int Y = 30;

            foreach (string item in source)
            {
                RadioButton rb = new RadioButton();

                rb.AutoSize = true;
                rb.Location = new System.Drawing.Point(X, Y);
                rb.Name = item;
                rb.Text = item;

                this.groupBox1.Controls.Add(rb);

                if (rbClick)
                {
                    rb.Click += new EventHandler(rb_Click);
                }

                if (X >= 410)
                {
                    Y += 50;
                    X = 30;
                }
                else
                {
                    X += 190;
                }
            }

            this.Size = new Size(Y == 30 ? X : this.Width, X == 30 ? panel1.Height + Y + 20 : panel1.Height + Y + 70);
        }

        void rb_Click(object sender, EventArgs e)
        {
            foreach (RadioButton rb in groupBox1.Controls)
            {
                if (rb is RadioButton && ((RadioButton)rb).Checked)
                {
                    m_strResult = ((RadioButton)rb).Text;
                    break;
                }
            }

            if (OnFormDataRodioDelegate != null)
            {
                if (OnFormDataRodioDelegate(m_strResult) == DialogResult.OK)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (RadioButton rb in groupBox1.Controls)
            {
                if (rb is RadioButton && ((RadioButton)rb).Checked)
                {
                    m_strResult = ((RadioButton)rb).Text;
                    break;
                }
            }

            if (OnFormDataRodioDelegate != null)
            {
                if (OnFormDataRodioDelegate(m_strResult) == DialogResult.OK)
                {
                    this.Close();
                }
                else
                {
                    return;
                }
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
