using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;

namespace UniversalControlLibrary
{
    public sealed class DeveloperExpressControl
    {
        public static void Init()
        {
            RegisterDsoFramer();
        }

        /// <summary>
        /// 注册DsoFramer文档控件
        /// </summary>
        static void RegisterDsoFramer()
        {
            try
            {
                RegistryKey pregkey = 
                    Registry.LocalMachine.OpenSubKey("SOFTWARE", true).OpenSubKey("Classes", true).OpenSubKey("DSOFramer.FramerControl", true);

                if (pregkey == null)
                {
                    string sPath = System.Windows.Forms.Application.StartupPath + "\\dsoframer2007.ocx";
                    ProcessStartInfo psi = new ProcessStartInfo("regsvr32.exe", "/s " + sPath);
                    Process.Start(psi);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("ActiveX控件注册失败 【"+ ex.Message +"】,请重新再打开画面。\r\n如再次出现此错误请联系管理员!");
            }
        }
    }
}
