using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using GlobalObject;

namespace UniversalControlLibrary
{
    public partial class Office文件显示 : Form
    {
        [DllImport("User32.dll")]
        public static extern void keybd_event(Byte bVk, Byte bScan, Int32 dwFlags, Int32 dwExtraInfo);

        public Office文件显示(string str, CE_SystemFileType documentType)
        {
            InitializeComponent();

            try
            {
                oframe.Open(@str);

                oframe.Menubar = false;
                oframe.Titlebar = false;
                oframe.Toolbars = false;

                this.oframe.ProtectDoc(1, 2, "pwd");

                switch (documentType)
                {
                    case CE_SystemFileType.Word:

                        Microsoft.Office.Interop.Word.Document wordDoc = (Microsoft.Office.Interop.Word.Document)oframe.ActiveDocument;
                        Microsoft.Office.Interop.Word.Application wordApp = wordDoc.Application;
                        break;
                    case CE_SystemFileType.Excel:

                        Microsoft.Office.Interop.Excel.Workbook excelDoc = (Microsoft.Office.Interop.Excel.Workbook)oframe.ActiveDocument;
                        Microsoft.Office.Interop.Excel.Application excelApp = excelDoc.Application;

                        excelApp.OnKey("^x", "");
                        excelApp.OnKey("^c", "");
                        excelApp.OnKey("^v", "");
                        break;
                    case CE_SystemFileType.PPT:

                        Microsoft.Office.Interop.PowerPoint._Presentation pptDoc =
                            (Microsoft.Office.Interop.PowerPoint._Presentation)oframe.ActiveDocument;
                        Microsoft.Office.Interop.PowerPoint.Application pptApp = pptDoc.Application;

                        keybd_event((byte)Keys.F5, 0, 0, 0);
                        keybd_event((byte)Keys.F5, 0, 2, 0);

                        break;
                    case CE_SystemFileType.PDF:
                        break;
                    case CE_SystemFileType.Miss:
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Office文件显示_FormClosing(object sender, FormClosingEventArgs e)
        {
            oframe.Close();
        }
    }
}