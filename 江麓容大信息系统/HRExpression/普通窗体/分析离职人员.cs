using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using UniversalControlLibrary;
using Service_Peripheral_HR;

namespace Form_Peripheral_HR
{
    public partial class 分析离职人员 : Form
    {
        /// <summary>
        /// 错误信息呢
        /// </summary>
        string m_error;

        /// <summary>
        /// 人员档案管理类
        /// </summary>
        IPersonnelArchiveServer m_personnerServer = Service_Peripheral_HR.ServerModuleFactory.GetServerModule<IPersonnelArchiveServer>();

        public 分析离职人员()
        {
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt = new DataTable();

                if (!m_personnerServer.ExcelDimission(dtpStartDate.Value,dtpEndDate.Value,out dt, out m_error))
                {
                    MessageDialog.ShowPromptMessage(m_error);                   
                }
                else
                {
                    ExcelHelperP.DataTableToExcel(saveFileDialog1, dt, null);
                }

                this.Close();
            }
            catch (Exception ex)
            {
                MessageDialog.ShowPromptMessage(ex.Message);
                this.Close();
            }
        }
    }
}
