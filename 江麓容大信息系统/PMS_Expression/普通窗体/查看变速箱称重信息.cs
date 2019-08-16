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
using UniversalControlLibrary;

namespace Expression
{
    public partial class 查看变速箱称重信息 : Form
    {
        public 查看变速箱称重信息()
        {
            InitializeComponent();
        }

        private void 查看变速箱称重信息_Load(object sender, EventArgs e)
        {
            dateTimePickerST.Value = ServerTime.Time.Date;
            dateTimePickerET.Value = ServerTime.Time.AddDays(1).Date;
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            IElectronFileServer server = ServerModuleFactory.GetServerModule<IElectronFileServer>();

            this.dataGridView1.DataSource = server.GetCVTWeightInfo(dateTimePickerST.Value.Date, dateTimePickerET.Value.Date);

            userControlDataLocalizer1.Init(dataGridView1, this.Name, 
                UniversalFunction.SelectHideFields(this.Name, dataGridView1.Name, BasicInfo.LoginID));
        }

        private void btnExportExcel_Click(object sender, EventArgs e)
        {
            ExcelHelperP.DatagridviewToExcel(saveFileDialog1, dataGridView1);
        }
    }
}
