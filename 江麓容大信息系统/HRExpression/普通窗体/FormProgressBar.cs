using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_Peripheral_HR
{
    public partial class FormProgressBar : Form
    {
        public FormProgressBar()
        {
            InitializeComponent();

            Inti();
        }

        private void Inti()
        {
            progressBar1.Maximum=100;

            for (int rownum = 1; rownum <= 100; rownum++)
            {
                //要执行的业务。

                rownum++;//循环次数
                //把当前循环次数赋值给进度条的value。
                progressBar1.Value = rownum;
                // 这个很关键，让程序去执行进度条的前进动作
                Application.DoEvents();
            }

        }
    }
}
