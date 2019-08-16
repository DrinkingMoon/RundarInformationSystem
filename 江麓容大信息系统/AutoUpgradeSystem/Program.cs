using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace AutoUpgradeSystem
{
    static class Program
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                bool createNewInstance;
                System.Threading.Mutex instance = new System.Threading.Mutex(true, "仓库管理系统", out createNewInstance);

                if (createNewInstance)
                {
                    instance.ReleaseMutex();
                    instance.Close();

                    string error;

                    if (!GlobalParameter.Init(out error))
                    {
                        MessageDialog.ShowPromptMessage(error);
                        return;
                    }

                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                    FormMain formMain = new FormMain();
                    formMain.ShowDialog();
                }

                System.Diagnostics.Process process = System.Diagnostics.Process.Start(
                    string.Format(@"{0}\{1}", Environment.CurrentDirectory, "Expression.exe"));

                SetForegroundWindow(process.MainWindowHandle);
            }
            catch (Exception exce)
            {
                MessageDialog.ShowPromptMessage(exce.Message);
            }
        }
    }
}
