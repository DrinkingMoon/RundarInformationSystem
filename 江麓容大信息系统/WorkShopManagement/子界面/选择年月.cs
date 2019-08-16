using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;

namespace Form_Manufacture_WorkShop
{
    public partial class 选择年月 : Form
    {
        public string _Str_YearMonth = "";

        public 选择年月()
        {
            InitializeComponent();
        }

        private void 选择年月_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
            {
                cmbYear.Items.Add(ServerTime.Time.Year - i);
            }

            cmbYear.Text = ServerTime.Time.Year.ToString();
            cmbMonth.Text = ServerTime.Time.Month.ToString("D2");
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (cmbYear.Text.Trim().Length == 0)
            {
                throw new Exception("请选择【年份】");
            }

            if (cmbMonth.Text.Trim().Length == 0)
            {
                throw new Exception("请选择【月份】");
            }

            _Str_YearMonth = cmbYear.Text + cmbMonth.Text;
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
