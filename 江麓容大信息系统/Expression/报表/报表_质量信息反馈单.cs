using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using ServerModule;
using Microsoft.Reporting.WinForms;
using System.IO;
using GlobalObject;
using System.Runtime.Serialization.Formatters.Binary;
using UniversalControlLibrary;

namespace Expression
{
    public partial class 报表_质量信息反馈单 : Form, IBillReportInfo
    {
        string m_billID = "";

        string m_billName = "";

        string m_err = "";

        DataTable dt = new DataTable();

        public 报表_质量信息反馈单(string billID, string billName)
        {
            

            InitializeComponent();
            m_billID = billID;
            m_billName = billName;

            this.reportViewer1.LocalReport.EnableExternalImages = true;
            InitData();
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

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void InitData()
        {
            using (SqlConnection conn = new SqlConnection())
            {
                conn.ConnectionString = GlobalObject.GlobalParameter.StorehouseConnectionString;
                conn.Open();

                string sql = "select * from DepotManagement.dbo.View_S_ALLMessMessageFeedback where DJH ='" + m_billID + "'";

                SqlCommand command = new SqlCommand(sql);
                DataSet ds = new DataSet();
                command.Connection = conn;

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(ds);
                adapter.Fill(this.depotManagementDataSet.View_S_ALLMessMessageFeedback);

                if (ds == null || ds.Tables.Count == 0)
                {
                    return;
                }

                dt = ds.Tables[0];

                ReportDataSource rds = new ReportDataSource("DepotManagementDataSet_View_S_ALLMessMessageFeedback", 
                                                            this.depotManagementDataSet.View_S_ALLMessMessageFeedback);

                reportViewer1.LocalReport.DataSources.Clear();
                reportViewer1.LocalReport.DataSources.Add(rds);

                string textString = "";

                if (dt != null && dt.Rows.Count != 0 && dt.Rows[0]["picture"] != null && dt.Rows[0]["picture"].ToString() != "")
                {
                    Byte[] imageBuffer = PackageBinary(dt.Rows[0]["picture"]);
                    textString = System.Convert.ToBase64String(imageBuffer);  
                }

                //设置参数   
                ReportParameter rptParaImage = new ReportParameter("rptParaImage", textString);
                this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rptParaImage }); 
                

            }
            this.reportViewer1.RefreshReport();
        }

        /// <summary>
        /// 解包调用数据
        /// </summary>
        /// <param name="callPackage">需要解包的字节码</param>
        /// <param name="expand">是否解压</param>
        /// <param name="decrypt">是否解密</param>
        /// <returns></returns>
        public static object UnpackageBinary(byte[] callPackage)
        {
            MemoryStream ms = new MemoryStream(callPackage);
            ms.Seek(0, SeekOrigin.Begin);

            BinaryFormatter bf = new BinaryFormatter();
            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesAlways;

            object s = (object)bf.Deserialize(ms);
            return s;

        }


        /// <summary>
        /// 打包成二进制调用
        /// </summary>
        /// <param name="_message">需要打包的object</param>
        /// <param name="compress">是否压缩</param>
        /// <param name="encrypt">是否加密</param>
        /// <returns></returns>
        public static byte[] PackageBinary(object _message)
        {
            MemoryStream ms = new MemoryStream();

            BinaryFormatter bf = new BinaryFormatter();

            bf.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Full;
            bf.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.TypesAlways;

            //将callPackage的内容以二进制序列化到内存中
            bf.Serialize(ms, _message);
            ms.Seek(0, SeekOrigin.Begin);

            byte[] buffer = new byte[ms.Length];
            ms.Read(buffer, 0, (int)ms.Length);
            ms.Close();

            return buffer;

        }




        private void reportViewer1_Print(object sender, CancelEventArgs e)
        {
            if (VirtualPrint.IsVirtualPrint(out m_err))
            {
                e.Cancel = true;
                MessageDialog.ShowPromptMessage(m_err);
            }
            else
            {

                IPrintManagement printManagement = BasicServerFactory.GetServerModule<IPrintManagement>();

                S_PrintBillTable printInfo = new S_PrintBillTable();

                printInfo.Bill_ID = m_billID;
                printInfo.Bill_Name = m_billName;
                printInfo.PrintDateTime = ServerModule.ServerTime.Time;
                printInfo.PrintFlag = true;
                printInfo.PrintPersonnelCode = BasicInfo.LoginID;
                printInfo.PrintPersonnelName = BasicInfo.LoginName;
                printInfo.PrintPersonnelDepartment = BasicInfo.DeptName;

                if (printManagement.IsExist(printInfo, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }
                else if (!printManagement.AddPrintInfo(printInfo, out m_err))
                {
                    MessageDialog.ShowPromptMessage(m_err);
                }

                reportViewer1.ShowPrintButton = false;
            }
        }

        private void 报表_质量信息反馈单_Load(object sender, EventArgs e)
        {
            if (BasicInfo.ListRoles.Contains(CE_RoleEnum.会计.ToString()))
            {
                reportViewer1.ShowPrintButton = false;
                return;
            }
        }

        #region 导出
        /// <summary>
        /// 导出Excel或Pdf文件，通过文件扩展名自动识别
        /// </summary>
        /// <param name="name">文件路径及名称(包括扩展名)</param>
        /// <returns>导出成功返回true，否则返回false</returns>
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
                            strFormat = "EXcel";
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
        #endregion 导出

        private void 导出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strFileName = dt.Rows[0]["DJH"].ToString() + dt.Rows[0]["ProviderName"].ToString();

            SaveFileDialog saveDlg = new SaveFileDialog();

            saveDlg.AddExtension = true;
            saveDlg.CheckPathExists = true;
            saveDlg.DefaultExt = ".xls";
            saveDlg.Filter = "Excel 文件 (*.xls)|*.xls|PDF 文件 (*.pdf)|*.pdf";
            saveDlg.ShowHelp = false;
            saveDlg.Title = "请选择导出路径及文件名称";
            saveDlg.FileName = strFileName;

            if (saveDlg.ShowDialog(this) == DialogResult.OK)
            {
                strFileName = saveDlg.FileName;

                if (ReportExport(strFileName))
                {
                    if (MessageBox.Show("文件保存完成！\r\n" + strFileName + "\r\n    要现在打开文件吗？", "导出成功", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        System.Diagnostics.Process.Start(strFileName);
                    }
                }
            }
        }


    }
}
