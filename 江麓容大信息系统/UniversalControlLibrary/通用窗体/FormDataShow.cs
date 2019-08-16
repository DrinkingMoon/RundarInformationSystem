using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ServerModule;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 数据显示界面
    /// </summary>
    public partial class FormDataShow : Form
    {
        public FormDataShow(object source)
        {
            InitializeComponent();

            dataGridView1.DataSource = source;

            userControlDataLocalizer1.Init(this.dataGridView1, this.Name,
                UniversalFunction.SelectHideFields(this.Name, this.dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
