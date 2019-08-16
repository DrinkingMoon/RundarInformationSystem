using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing.Printing;
using Microsoft.Reporting.WinForms;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Threading;
using ServerModule;
using GlobalObject;

namespace UniversalControlLibrary
{
    /// <summary>
    /// 单据类报表信息
    /// </summary>
    public interface IBillReportInfo
    {
        /// <summary>
        /// 获取单据类型, 如：领料单等
        /// </summary>
        string BillType
        {
            get;
        }

        /// <summary>
        /// 获取单据编号
        /// </summary>
        string BillNo
        {
            get;
        }

        /// <summary>
        /// 获取本地报表对象
        /// </summary>
        LocalReport LocalReportObject
        {
            get;
        }
    }

    /// <summary>
    /// 虚拟打印机管理类
    /// </summary>
    public static class VirtualPrint
    {
        /// <summary>
        /// 是否已经获取到虚拟打印机的标志(此过程耗时很长，大约一分钟左右)
        /// </summary>
        static bool m_initPrint;

        /// <summary>
        /// 虚拟打印机列表
        /// </summary>
        static List<string> m_virtualPrint = new List<string>();

        /// <summary>
        /// 线程对象
        /// </summary>
        static Thread m_thread = new Thread(new ThreadStart(VirtualPrint.GetVirtualPrintThread));

        /// <summary>
        /// 初始化
        /// </summary>
        static public void Init()
        {
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        /// <summary>
        /// 获取虚拟打印机名称
        /// </summary>
        static public string[] PrintNames
        {
            get
            {
                string[] prints = new string[m_virtualPrint.Count];
                m_virtualPrint.CopyTo(prints);
                return prints;
            }
        }

        /// <summary>
        /// 判断打印机是否是虚拟打印机
        /// </summary>
        /// <param name="printName">打印机名</param>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>是虚拟打印机返回true</returns>
        static public bool IsVirtualPrint(string printName, out string error)
        {
            error = null;

            if (!m_initPrint)
            {
                error = "正在获取打印设备信息，此过程大约耗时一分钟，请稍候再进行此操作";
                return true;
            }
            else if (m_virtualPrint.Contains(printName))
            {
                error = string.Format("无效的打印机：{0}，不能进行单据打印！", printName);
                return true;
            }

            return false;
        }

        /// <summary>
        /// 判断当前打印机是否是虚拟打印机
        /// </summary>
        /// <param name="error">出错时返回错误信息，无错时返回null</param>
        /// <returns>是虚拟打印机返回true</returns>
        static public bool IsVirtualPrint(out string error)
        {
            // 获取当前打印机名称
            PrintDocument printDoc = new PrintDocument();
            string printName = printDoc.PrinterSettings.PrinterName;

            printDoc.Dispose();

            return IsVirtualPrint(printName, out error);
        }

        /// <summary>
        /// 获取虚拟打印机的线程
        /// </summary>
        static void GetVirtualPrintThread()
        {
            string wmi = "SELECT * FROM Win32_Printer";

            try
            {
                System.Management.ManagementObjectCollection printers = new System.Management.ManagementObjectSearcher(wmi).Get();

                foreach (System.Management.ManagementObject printer in printers)
                {
                    System.Management.PropertyDataCollection.PropertyDataEnumerator pe = printer.Properties.GetEnumerator();
                    string printName = "";
                    string portName = "";

                    while (pe.MoveNext())
                    {
                        if (pe.Current.Name == "Name")
                            printName = pe.Current.Value.ToString();
                        else if (pe.Current.Name == "PortName")
                            portName = pe.Current.Value.ToString();
                    }

                    try
                    {
                        if (portName[portName.Length - 1] == ':')
                        {
                            portName = portName.Remove(portName.Length - 1);
                        }

                        if (portName.Length < 4)
                        {
                            m_virtualPrint.Add(printName);
                        }
                        else
                        {
                            if (portName.Substring(0, 3) == "LPT" || portName.Substring(0, 3) == "COM" || portName.Substring(0, 3) == "USB")
                            {
                                Convert.ToInt32(portName.Substring(3));
                                //StapleInfo.PrintList.Add(printName);
                                continue;
                            }

                            m_virtualPrint.Add(printName);
                        }
                    }
                    catch (Exception exce)
                    {
                        Console.WriteLine(exce.Message);
                        m_virtualPrint.Add(printName);
                    }
                }

                m_initPrint = true;
            }
            catch (Exception exce)
            {
                Console.WriteLine(exce.Message);
            }
        }
    }

    /// <summary>
    /// 直接打印报表（略过通过点击打印按钮来实现打印）
    /// </summary>
    public class PrintReportBill
    {
        #region 打印

        /// <summary>
        /// 当前页索引
        /// </summary>
        private int m_currentPageIndex;

        /// <summary>
        /// 文件流
        /// </summary>
        private IList<Stream> m_streams;

        /// <summary>
        /// 错误信息
        /// </summary>
        private string m_err;

        /// <summary>
        /// 打印页宽度, 以厘米为单位
        /// </summary>
        private double m_pageWidth;

        /// <summary>
        /// 打印页高度, 以厘米为单位
        /// </summary>
        private double m_pageHeight;

        /// <summary>
        /// 是否允许打印的标志
        /// </summary>
        private bool m_allowPrint = true;

        /// <summary>
        /// 单据类报表信息
        /// </summary>
        private IBillReportInfo m_billReportInfo;

        /// <summary>
        /// 在打印前是否显示选择打印机对话框的标志
        /// </summary>
        private bool m_showPrintDialog = true;

        /// <summary>
        /// 获取或设置在打印前是否显示选择打印机对话框的标志
        /// </summary>
        public bool ShowPrintDialog
        {
            get { return m_showPrintDialog; }
            set { m_showPrintDialog = value; }
        }

        /// <summary>
        /// 构造函数(采用默认的A4纸张)
        /// </summary>
        public PrintReportBill()
        {
            m_pageWidth = 21;
            m_pageHeight = 29.7;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageWidth">打印页宽度, 以厘米为单位</param>
        /// <param name="pageHeight">打印页高度, 以厘米为单位</param>
        public PrintReportBill(double pageWidth, double pageHeight)
        {
            m_pageWidth = pageWidth;
            m_pageHeight = pageHeight;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="pageWidth">打印页宽度, 以厘米为单位</param>
        /// <param name="pageHeight">打印页高度, 以厘米为单位</param>
        /// <param name="billReportInfo">单据类报表信息</param>
        public PrintReportBill(double pageWidth, double pageHeight, IBillReportInfo billReportInfo)
        {
            m_pageWidth = pageWidth;
            m_pageHeight = pageHeight;
            m_billReportInfo = billReportInfo;

            IPrintManagement printManagement = BasicServerFactory.GetServerModule<IPrintManagement>();

            S_PrintBillTable printInfo = new S_PrintBillTable();

            printInfo.Bill_ID = billReportInfo.BillNo;
            printInfo.Bill_Name = billReportInfo.BillType;
            printInfo.PrintDateTime = ServerModule.ServerTime.Time;
            printInfo.PrintFlag = true;
            printInfo.PrintPersonnelCode = BasicInfo.LoginID;
            printInfo.PrintPersonnelName = BasicInfo.LoginName;
            printInfo.PrintPersonnelDepartment = BasicInfo.DeptName;

            if (billReportInfo.BillType != "")
            {
                if (!printManagement.AddPrintInfo(printInfo, out m_err))
                {
                    m_allowPrint = false;
                    MessageDialog.ShowPromptMessage(m_err);
                }
            }
       }
        
        // Routine to provide to the report renderer, in order to
        //    save an image for each page of the report.
        private Stream CreateStream(string name, string fileNameExtension, Encoding encoding, string mimeType, bool willSeek)
        {
            string filenameext = ServerTime.Time.Year.ToString() + ServerTime.Time.Day.ToString() 
                + ServerTime.Time.Month.ToString() + ServerTime.Time.Hour.ToString() + ServerTime.Time.Minute.ToString() 
                + ServerTime.Time.Second.ToString();

            Stream stream = new FileStream(name + "." + fileNameExtension, FileMode.Create);
            m_streams.Add(stream);
            return stream;
        }

        // Export the given report as an EMF (Enhanced Metafile) file.
        private void Export(LocalReport report)
        {
            string deviceInfo =
              "<DeviceInfo>" +
              "  <OutputFormat>EMF</OutputFormat>" +
              string.Format("  <PageWidth>{0}cm</PageWidth>", m_pageWidth) +
              //"  <PageWidth>21.0cm</PageWidth>" +
              string.Format("  <PageHeight>{0}cm</PageHeight>", m_pageHeight) +
              //"  <PageHeight>9.7cm</PageHeight>" +
              "  <MarginTop>0cm</MarginTop>" +
              "  <MarginLeft>0cm</MarginLeft>" +
              "  <MarginRight>0cm</MarginRight>" +
              "  <MarginBottom>0cm</MarginBottom>" +
              "</DeviceInfo>";
            Warning[] warnings;
            m_streams = new List<Stream>();

            report.Render("Image", deviceInfo, CreateStream, out warnings);

            foreach (Stream stream in m_streams)
            {
                stream.Position = 0;
            }
        }
        // Handler for PrintPageEvents
        private void PrintPage(object sender, PrintPageEventArgs ev)
        {
            Metafile pageImage = new Metafile(m_streams[m_currentPageIndex]);

            ev.Graphics.DrawImage(pageImage, ev.PageBounds);
            m_currentPageIndex++;

            ev.HasMorePages = (m_currentPageIndex < m_streams.Count);
        }

        /// <summary>
        /// 打印
        /// </summary>
        private void Print()
        {
            if (m_streams == null || m_streams.Count == 0)
                return;

            PrintDocument printDoc = new PrintDocument();

            if (m_showPrintDialog)
            {
                PrintDialog printDialog = new PrintDialog();

                printDialog.Document = printDoc;
                printDialog.AllowPrintToFile = false;
                printDialog.AllowCurrentPage = false;
                printDialog.AllowSelection = false;
                printDialog.AllowSomePages = false;
                printDialog.PrinterSettings.Copies = 1;

                if (printDialog.ShowDialog() != DialogResult.OK)
                {
                    if (MessageDialog.ShowEnquiryMessage("您是否确定取消当前打印？") == DialogResult.Yes)
                    {
                        return;
                    }
                }

                printDialog.PrinterSettings.Copies = 1;
            }


            #region 页面设置
            PageSetupDialog pageDialog = new PageSetupDialog();

            pageDialog.Document = printDoc;
            pageDialog.PageSettings.PaperSize = new PaperSize("Custom", (int)(100 * m_pageWidth / 2.54 + 0.5), 
                (int)(100 * m_pageHeight / 2.54 + 0.5));

            #region 设置为横向打印

            //if (m_pageWidth < m_pageHeight)
            //{
            //    pageDialog.PageSettings.Landscape = true;
            //}

            #endregion

            printDoc.DefaultPageSettings = pageDialog.PageSettings;

            #endregion
            // 指定打印机
            //printDoc.PrinterSettings.PrinterName = printDocument1.PrinterSettings.PrinterName;

            if (!printDoc.PrinterSettings.IsValid)
            {
                string msg = String.Format("Can't find printer \"{0}\".", "默认打印机!");
                MessageBox.Show(msg, "找不到默认打印机");
                return;
            }

            #region 判断打印机是否虚拟打印机,不允许打印到P虚拟设备(如打印到PDF文件),否则可能出现多次打印等单据不可控现象

            //string error = null;

            //if (VirtualPrint.IsVirtualPrint(printDoc.PrinterSettings.PrinterName, out error))
            //{
            //    MessageDialog.ShowPromptMessage(error);
            //    return;
            //}

            #endregion

            printDoc.PrintPage += new PrintPageEventHandler(PrintPage);
            printDoc.Print();
        }

        /// <summary>
        /// 删除打印产生的EMF文件
        /// </summary>
        private void DeleteEmfFile()
        {
            DirectoryInfo path = new DirectoryInfo(Environment.CurrentDirectory);
            FileInfo[] files = path.GetFiles("*.emf");

            try
            {
                foreach (FileInfo file in files)
                {
                    file.Delete();
                }
            }
            catch (Exception exec)
            {
                Console.WriteLine(exec.Message);
            }
        }

        /// <summary>
        /// 直接打印要打印的报表
        /// </summary>
        public void DirectPrintReport()
        {
            if (m_billReportInfo == null || m_billReportInfo.LocalReportObject == null)
            {
                throw new ArgumentException("单据类报表信息为空");
            }

            DirectPrintReport(m_billReportInfo.LocalReportObject);
        }

        /// <summary>
        /// 直接打印要打印的报表
        /// </summary>
        /// <param name="report">报表对象</param>
        public void DirectPrintReport(LocalReport report)
        {
            if (!m_allowPrint)
            {
                return;
            }

            try
            {
                Export(report);
                m_currentPageIndex = 0;
                Print();

                if (m_streams != null)
                {
                    foreach (Stream stream in m_streams)
                    {
                        stream.Close();
                    }

                    m_streams = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("在打印过程中出现异常：" + ex.Message);
            }
            finally
            {
                DeleteEmfFile();
            }
        }

        #endregion

    }
}
