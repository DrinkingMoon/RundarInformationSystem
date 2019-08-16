using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Expression
{
    /// <summary>
    /// 产品隔离处置单选择处理部门的界面
    /// </summary>
    public partial class 不合格品处理部门 : Form
    {
        /// <summary>
        /// 确认标志
        /// </summary>
        private bool m_blFlag = false;

        public bool BlFlag
        {
            get { return m_blFlag; }
            set { m_blFlag = value; }
        }

        /// <summary>
        /// 处理部门
        /// </summary>
        private string m_strCLBM = "";

        public string StrCLBM
        {
            get { return m_strCLBM; }
            set { m_strCLBM = value; }
        }

        public 不合格品处理部门()
        {
            InitializeComponent();
        }

        private void btSure_Click(object sender, EventArgs e)
        {
            foreach (Control rb in this.Controls)
            {
                if (rb is RadioButton)
                {
                    RadioButton rbTemp = rb as RadioButton;

                    if (rbTemp.Checked)
                    {
                        m_strCLBM = rb.Tag.ToString();
                    }
                }
            }

            if (m_strCLBM == "")
            {
                this.Close();
            }
            else
            {
                m_blFlag = true;
            }

            this.Close();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
