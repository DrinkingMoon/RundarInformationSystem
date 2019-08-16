using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Form_Peripheral_HR.普通窗体
{
    public partial class 专家专业人才明细 : Form
    {
        public 专家专业人才明细()
        {
            InitializeComponent();
        }

        private void txtOnAccess_OnCompleteSearch()
        {
            txtOnAccess.Text = txtOnAccess.DataResult["姓名"].ToString();
            txtOnAccess.Tag = txtOnAccess.DataResult["工号"].ToString();
        }
    }
}
