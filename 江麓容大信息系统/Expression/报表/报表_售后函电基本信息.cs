using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using Microsoft.Reporting.WinForms;
using ServerModule;
using GlobalObject;
using System.IO;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 报表_售后函电基本信息 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        string error = "";

        public 报表_售后函电基本信息(string billID, string billName)
        {
            InitializeComponent();

            m_billID = billID;
            m_billName = billName;

            InitData();
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from RundarWebServer.dbo.S_AfterService where ServiceID ='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(this.rundarWebServerDataSet.S_AfterService);

                ReportDataSource rds = new ReportDataSource("RundarWebServerDataSet_S_AfterService", 
                    this.rundarWebServerDataSet.S_AfterService);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                sql = "select * from RundarWebServer.dbo.OF_BugMessageInfo where ServiceID='" + m_billID + "'";

                command = new SqlCommand(sql);
                command.Connection = conn;

                adapter = new SqlDataAdapter(command);
                adapter.Fill(this.rundarWebServerDataSet.OF_BugMessageInfo);

                rds = new ReportDataSource("RundarWebServerDataSet_OF_BugMessageInfo", this.rundarWebServerDataSet.OF_BugMessageInfo);

                reportViewer1.LocalReport.DataSources.Add(rds);
            }

            this.reportViewer1.RefreshReport();
        }

        private void 报表_售后函电基本信息_Load(object sender, EventArgs e)
        {
            //if (BasicInfo.ListRoles.Contains(RoleEnum.会计.ToString()))
            //{
            //    reportViewer1.ShowPrintButton = false;
            //    return;
            //}
        }

        private void reportViewer1_Print(object sender, CancelEventArgs e)
        {
            if (VirtualPrint.IsVirtualPrint(out error))
            {
                e.Cancel = true;
                MessageDialog.ShowPromptMessage(error);
            }
            //else
            //{
            //    IPrintManagement printManagement = BasicServerFactory.GetServerModule<IPrintManagement>();

            //    S_PrintBillTable printInfo = new S_PrintBillTable();
            //    printInfo.Bill_ID = m_billID;
            //    printInfo.Bill_Name = m_billName;
            //    printInfo.PrintDateTime = ServerModule.ServerTime.Time;
            //    printInfo.PrintFlag = true;
            //    printInfo.PrintPersonnelCode = BasicInfo.LoginID;
            //    printInfo.PrintPersonnelName = BasicInfo.LoginName;
            //    printInfo.PrintPersonnelDepartment = BasicInfo.DeptName;

                //if (printManagement.IsExist(printInfo, out m_err))
                //{
                //    MessageDialog.ShowPromptMessage(m_err);
                //}
                //else if (!printManagement.AddPrintInfo(printInfo, out m_err))
                //{
                //    MessageDialog.ShowPromptMessage(m_err);
                //}

                //reportViewer1.ShowPrintButton = false;
            //}
        }

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strFileName = m_billID + m_billName;

            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.AddExtension = true;
            saveDlg.CheckPathExists = true;
            saveDlg.DefaultExt = ".xls";
            saveDlg.Filter = "Excel 文件 (*.xls)|*.xls|PDF 文件 (*.pdf)|*.pdf";
            saveDlg.ShowHelp = false;
            saveDlg.Title = "请选择导出路径及文件名称";
            saveDlg.FileName = (strFileName).Replace("/", "-");

            if (saveDlg.ShowDialog(this) == DialogResult.OK)
            {
                strFileName = saveDlg.FileName;

                if (ReportExport(strFileName))
                {
                    if (MessageBox.Show("文件保存完成！\r\n" + strFileName + "\r\n    要现在打开文件吗？", 
                        "导出成功", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(strFileName);
                    }
                }
            }
        }

        private bool ReportExport(string name)
        {
            bool result = false;

            string Name = name;
            string strFileName;//文件名
            string strExtend = ".xls";//扩展名
            string strFormat = "Excel";//文件格式
            bool validate = false;

            if (name != null || name.Trim() != String.Empty)
            {
                Name = name.Trim();
                strFileName = Path.GetFileName(Name);

                if (strFileName.Trim() != String.Empty)
                {
                    strExtend = Path.GetExtension(Name).Trim().ToLower();

                    switch (strExtend)
                    {
                        case ".xls":
                            strFormat = "Excel";
                            break;
                        case ".pdf":
                            strFormat = "PDF";
                            break;
                        default:
                            strFormat = "Excel";
                            break;
                    }
                    validate = true;
                }
            }

            if (validate)
            {
                Warning[] Warnings;
                string[] strStreamIds;
                string strMimeType;
                string strEncoding;
                string strFileNameExtension;

                byte[] bytes = this.reportViewer1.LocalReport.Render(strFormat, null, out strMimeType,
                    out strEncoding, out strFileNameExtension, out strStreamIds, out Warnings);

                using (System.IO.FileStream fs = new FileStream(Name, FileMode.Create))
                {
                    fs.Write(bytes, 0, bytes.Length);
                }

                result = true;
            }

            return result;
        }

        #region IBillReportInfo 成员

        /// <summary>
        /// 获取单据类型, 如：领料单等
        /// </summary>
        public string BillType
        {
            get
            {
                return m_billName;
            }
        }

        /// <summary>
        /// 获取单据编号
        /// </summary>
        public string BillNo
        {
            get
            {
                return m_billID;
            }
        }

        /// <summary>
        /// 获取本地报表对象
        /// </summary>
        public LocalReport LocalReportObject
        {
            get
            {
                return reportViewer1.LocalReport;
            }
        }

        #endregion
    }
}
