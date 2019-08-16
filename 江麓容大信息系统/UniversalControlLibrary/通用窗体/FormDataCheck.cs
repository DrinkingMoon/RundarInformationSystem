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
    public partial class FormDataCheck : Form
    {
        public event GlobalObject.DelegateCollection.DelegateTask ExecuteTask;

        private List<string> m_lstResult = new List<string>();

        public List<string> LstResult
        {
            get { return m_lstResult; }
            set { m_lstResult = value; }
        }

        /// <summary>
        /// 选择窗体
        /// </summary>
        /// <param name="listData">需要选择项的列表</param>
        public FormDataCheck(List<string> listData)
        {
            InitializeComponent();
            SetCheckBox(listData);
        }

        /// <summary>
        /// 选择窗体
        /// </summary>
        /// <param name="listData">需要选择项的列表</param>
        /// <param name="hasCheckData">已选择项的列表</param>
        public FormDataCheck(List<string> listData, List<string> hasCheckData)
        {
            InitializeComponent();
            m_lstResult = hasCheckData;
            SetCheckBox(listData);
        }

        void SetCheckBox(List<string> listData)
        {
            if (listData == null)
            {
                return;
            }

            int X = 30;
            int Y = 30;

            foreach (string item in listData)
            {
                CheckBox chb = new CheckBox();

                chb.AutoSize = true;
                chb.Location = new System.Drawing.Point(X, Y);
                chb.Name = item;
                chb.Text = item;
                chb.Checked = m_lstResult != null && m_lstResult.Contains(item) ? true : false;

                chb.Click += new EventHandler(chb_Click);

                this.groupBox1.Controls.Add(chb);

                if (X >= 600)
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

        void chb_Click(object sender, EventArgs e)
        {
            if (radioButton2.Checked || radioButton1.Checked)
            {
                radioButton3.Checked = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton1.Checked)
            {
                foreach (Control chb in groupBox1.Controls)
                {
                    if (chb is CheckBox)
                    {
                        ((CheckBox)chb).Checked = true;
                    }
                }
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton2.Checked)
            {
                foreach (Control chb in groupBox1.Controls)
                {
                    if (chb is CheckBox)
                    {
                        ((CheckBox)chb).Checked = false;
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_lstResult = new List<string>();

            foreach (Control chb in groupBox1.Controls)
            {
                if (chb is CheckBox && ((CheckBox)chb).Checked)
                {
                    m_lstResult.Add(((CheckBox)chb).Text);
                }
            }

            this.DialogResult = DialogResult.OK;

            if (ExecuteTask != null)
            {
                ExecuteTask(m_lstResult);
            }

            this.Close();
        }
    }
}
